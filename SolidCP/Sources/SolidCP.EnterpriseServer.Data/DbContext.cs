using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Diagnostics.Eventing.Reader;

#if NetFX
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
#endif

namespace SolidCP.EnterpriseServer.Data
{
    public partial class DbContext : IDisposable
    {

        public DateTime DateTimeMin = new DateTime(1735, 1, 1);

        public const bool UseStoredProcedures = true;

        string connectionString = null;
        string nativeConnectionString = null;
        public string ConnectionString
        {
            get => connectionString ??= DbSettings.ConnectionString;
            set => connectionString = value;
        }

        public string NativeConnectionString {
            get
            {
                if (nativeConnectionString != null) return nativeConnectionString;
                NativeConnectionString = ConnectionString;
                return nativeConnectionString;
            }
            set {
                nativeConnectionString = value;
				DbType dbType = DbType.Unknown;
				var csb = new ConnectionStringBuilder(value);
                var dbTypeStr = csb["DbType"] as string;
                if (!string.IsNullOrEmpty(dbTypeStr))
                {
                    if (Enum.TryParse<DbType>(dbTypeStr, out dbType)) DbType = dbType;
                    csb.Remove("DbType");
                }

                if (dbType == DbType.Sqlite || dbType == DbType.SqliteFX)
                {
                    var dbFile = (string)csb["Data Source"];
                    if (!Path.IsPathRooted(dbFile))
                    {
                        dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFile);
                        csb["Data Source"] = dbFile;
                    }
#if NETFRAMEWORK
                    csb["BinaryGUID"] = "false";
                    csb["Busy Timeout"] = "10000";
                    csb["Journal Mode"] = "WAL";
                    csb["Synchronous"] = "Normal";
#endif
					csb["Foreign Keys"] = "true";
                }
                nativeConnectionString = csb.ToString();
            }
        }

        DbConnection SqlServerDbConnection => new Microsoft.Data.SqlClient.SqlConnection(NativeConnectionString);
#if NETFRAMEWORK
        DbConnection MySqlDbConnection => new MySql.Data.MySqlClient.MySqlConnection(NativeConnectionString);
#else
        DbConnection MySqlDbConnection => new MySqlConnector.MySqlConnection(NativeConnectionString);

#endif
		DbConnection PostgreSqlDbConnection => new Npgsql.NpgsqlConnection(NativeConnectionString);

#if Oracle
        DbConnection OracleConnection => new Oracle.ManagedDataAccess.Client.OracleConnection(NativeConnectionString);
#endif

        DbConnection SqliteDbConnection
        {
            get
            {
#if NETFRAMEWORK
                return new System.Data.SQLite.SQLiteConnection(NativeConnectionString);
#else
                return new Microsoft.Data.Sqlite.SqliteConnection(NativeConnectionString);
#endif
            }
        }
        
        DbConnection dbConnection = null;
        public DbConnection DbConnection
        {
            get
            {
                if (dbConnection == null)
                {

#if NETFRAMEWORK
                    DbConfiguration.InitDatabaseProvider(DbType);
#endif

                    switch (DbType)
					{
						case DbType.SqlServer:
						    dbConnection = SqlServerDbConnection;
                            break;
						case DbType.MySql:
						case DbType.MariaDb:
							dbConnection = MySqlDbConnection;
							break;
						case DbType.Sqlite:
                        case DbType.SqliteFX:
							dbConnection = SqliteDbConnection;
                            break;
						case DbType.PostgreSql:
							dbConnection = PostgreSqlDbConnection;
							break;
                        case DbType.Oracle:
#if Oracle
                            dbConnection = OracleConnection;
#else
                            throw new NotSupportedException("Oracle is not supported.");
#endif
                            break;
                    }
                }
                return dbConnection;
			}
		}

        DbType dbType = DbType.Unknown;
        public DbType DbType
        {
            get
            {
                if (dbType != DbType.Unknown) return dbType;
                dbType = DbSettings.GetDbType(ConnectionString);
                if (dbType == DbType.Unknown) dbType = DbSettings.DbType;
                if (dbType == DbType.Other) throw new NotSupportedException($"Db type is not supported");
                return dbType;
            }
            set => dbType = value;
        }

        public bool InitSeedData { get; set; } = false;

        static Type contextType = null;
        Type ContextType
        {
            get
            {
                if (contextType == null)
                {
                    switch (DbType)
                    {
                        default:
                        case DbType.SqlServer: contextType = typeof(SqlServerDbContext); break;
                        case DbType.MySql: contextType = typeof(MySqlDbContext); break;
                        case DbType.MariaDb: contextType = typeof(MySqlDbContext); break;
                        case DbType.PostgreSql: contextType = typeof(PostgreSqlDbContext); break;
                        case DbType.Sqlite:
                        case DbType.SqliteFX: contextType = typeof(SqliteDbContext); break;
#if Oracle
                        case DbType.Oracle: contextType = typeof(OracleDbContext); break;
#endif
                    }
                }
                return contextType;
            }
        }

        static bool dbConfigurationSet = false;
        static void SetDbConfiguration()
        {
            if (!dbConfigurationSet)
            {
                dbConfigurationSet = true;
#if NETFRAMEWORK
                System.Data.Entity.DbConfiguration.SetConfiguration(new DbConfiguration());
#endif
			}
		}

        public IGenericDbContext BaseContext = null;

        public DbContext()
        {
            //SetDbConfiguration();

#if NETSTANDARD
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType, this);
#else
			var context = new Context.DbContextBase(this);
			BaseContext = context;
#endif
#if NETFRAMEWORK
            Database.CommandTimeout = 60;
#else
            Database.SetCommandTimeout(60);
#endif

            BaseContext.Log += WriteToLog;
		}

		public DbContext(string connectionString, DbType dbType = DbType.Unknown, bool initSeedData = false)
        {
            /*if (dbType == DbType.Unknown)
            {
                var csb = new ConnectionStringBuilder(connectionString);
                if (!Enum.TryParse<DbType>((string)(csb["DbType"] ?? "Unknown"), out dbType)) dbType = DbType.Other;
            }*/
			ConnectionString = connectionString;
			if (dbType != DbType.Unknown) DbType = dbType;

			InitSeedData = initSeedData;
#if NETSTANDARD
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType, this);
#else
            BaseContext = new Context.DbContextBase(this);
#endif
#if NETFRAMEWORK
            Database.CommandTimeout = 120;
#else
            Database.SetCommandTimeout(120);
#endif
            BaseContext.Log += WriteToLog;
        }

        public bool IsSqlServer => DbType == DbType.SqlServer;
        public bool IsMySql => DbType == DbType.MySql;
		public bool IsSqlite => IsSqliteCore || IsSqliteFX;
        public bool IsSqliteFX => DbType == DbType.SqliteFX;
        public bool IsSqliteCore => DbType == DbType.Sqlite;
		public bool IsPostgreSql => DbType == DbType.PostgreSql;
        public bool IsMariaDb => DbType == DbType.MariaDb;
        public bool IsOracle => DbType == DbType.Oracle;
        public bool HasProcedures => IsSqlServer && UseStoredProcedures;

#if NetCore
        public const bool IsCore = true;
        public const bool IsNetFX = false;
#elif NetFX
		public const bool IsCore = false;
		public const bool IsNetFX = true;
#else
        public const bool IsCore = false;
        public const bool IsNetFX = false;
#endif

#if NETFRAMEWORK
		public Database Database => BaseContext.Database;
        public DbSet<TEntity> Set<TEntity>() where TEntity: class => BaseContext.Set<TEntity>();
        public Context.DbContextBase Context => (Context.DbContextBase)BaseContext;
#else
		public DatabaseFacade Database => BaseContext.Database;
		public DbSet<TEntity> Set<TEntity>() where TEntity: class => BaseContext.Set<TEntity>();
        public Context.DbContextBase Context => (Context.DbContextBase)BaseContext;
#endif

		public int SaveChanges() => BaseContext.SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => BaseContext.SaveChangesAsync(cancellationToken);
        public Action<string> Log { get; set; }
        private void WriteToLog(string msg) => Log?.Invoke(msg);

		private DbContext clone = null;
		public DbContext Clone => clone ??= new DbContext(ConnectionString, DbType);
		public virtual void Dispose()
		{
			clone?.Dispose();
			if (BaseContext is IDisposable baseContext) baseContext.Dispose();
		}

	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;
using System.Configuration;
using System.Linq;
using System.IO;
#if NetFX
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore.Infrastructure;
#endif

namespace SolidCP.EnterpriseServer.Data
{
    public partial class DbContext : IDisposable
    {

        public const bool UseStoredProcedures = true;

        static string connectionString = null;
        public string ConnectionString
        {
            get => connectionString ??= DbSettings.NativeConnectionString;
            set => connectionString = DbSettings.GetNativeConnectionString(value);
        }
        /*        static string providerName = null;
                public string ProviderName
                {
                    get => providerName ??= DbSettings.ProviderName;
                    set => providerName = value;
                }
        */
        /*
                DbProviderFactory DbProviderFactory
                {
                    get
                    {
                        Type factoryType = null;
                        switch (DbType)
                        {
                            case DbType.MsSql:
                                factoryType = Type.GetType("Microsoft.Data.SqlClient.SqlClientFactory, Microsoft.Data.SqlClient");
                                break;
                            case DbType.MySql:
                            case DbType.MariaDb:
                                factoryType = Type.GetType("MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data.EntityFramework, Version=8.4.0.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d");
                                break;
                            case DbType.Sqlite:
        #if NETFRAMEWORK
                                factoryType = Type.GetType("System.Data.SQLite.EF6.SQLiteProviderFactory, System.Data.SQLite.EF6");
        #else
                                factoryType = Type.GetType("Microsoft.Data.SQLite.SQLiteProviderFactory, Microsoft.Data.SQLite");
        #endif                        
                                break;
                            case DbType.PostgreSql:
                                factoryType = Type.GetType("Npgsql.NpgsqlFactory, Npgsql, Version=8.0.3.0, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7");
                                break;
                        }

                        return (DbProviderFactory)Activator.CreateInstance(factoryType);
                    }
                }
                */

        DbConnection MsSqlDbConnection => new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
#if NETFRAMEWORK
        DbConnection MySqlDbConnection => new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
#else
        DbConnection MySqlDbConnection => new MySqlConnector.MySqlConnection(ConnectionString);

#endif
		DbConnection PostgreSqlDbConnection => new Npgsql.NpgsqlConnection(ConnectionString);

        static DbProviderFactory factory = null;
        DbConnection SqliteDbConnection
        {
            get
            {
#if NETFRAMEWORK
                Sqlite.LoadNativeDlls();
                //factory ??= new System.Data.SQLite.EF6.SQLiteProviderFactory();
                factory ??= DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
                var conn = factory.CreateConnection();
                var csb = new DbConnectionStringBuilder();
                csb.ConnectionString = ConnectionString;
                var dbFile = (string)csb["Data Source"];
                if (!Path.IsPathRooted(dbFile))
                {
                    dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFile);
                    csb["Data Source"] = dbFile;
                    ConnectionString = csb.ToString();
                }
                conn.ConnectionString = ConnectionString;
                return conn;
#else
                return new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
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
					switch (DbType)
					{
						case DbType.MsSql:
                            dbConnection = MsSqlDbConnection;
                            break;
						case DbType.MySql:
						case DbType.MariaDb:
							dbConnection = MySqlDbConnection;
							break;
						case DbType.Sqlite:
							dbConnection = SqliteDbConnection;
							break;
						case DbType.PostgreSql:
							dbConnection = PostgreSqlDbConnection;
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
#if NETSTANDARD
                    var dbType = DbType;
                    if (dbType == DbType.MariaDb) dbType = DbType.MySql;
                    if (OSInfo.IsCore) {    
                        contextType = Type.GetType($"SolidCP.EnterpriseServer.Context.{dbType}DbContext, SolidCP.EnterpriseServer.Data.Core"); break;
                    } else {
                        contextType = Type.GetType($"SolidCP.EnterpriseServer.Context.{dbType}DbContext, SolidCP.EnterpriseServer.Data.Core");
                    }
#else
                    switch (DbType)
                    {
                        default:
                        case DbType.MsSql: contextType = typeof(MsSqlDbContext); break;
                        case DbType.MySql: contextType = typeof(MySqlDbContext); break;
                        case DbType.MariaDb: contextType = typeof(MySqlDbContext); break;
                        case DbType.PostgreSql: contextType = typeof(PostgreSqlDbContext); break;
                        case DbType.Sqlite: contextType = typeof(SqliteDbContext); break;
                    }
#endif
                }
                return contextType;
            }
        }

        public IGenericDbContext BaseContext = null;
        public DbContext()
        {
#if NETSTANDARD
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType, this);
#else
            BaseContext = new Context.DbContextBase(this);
#endif
			BaseContext.Log += WriteToLog;
		}

		public DbContext(string connectionString, DbType dbType = DbType.Unknown, bool initSeedData = false)
        {
            if (dbType == DbType.Unknown) DbType = DbSettings.GetDbType(connectionString);
            else DbType = dbType;
            ConnectionString = connectionString;
            InitSeedData = initSeedData;
#if NETSTANDARD
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType, this);
#else
            BaseContext = new Context.DbContextBase(this);
#endif
            BaseContext.Log += WriteToLog;
        }

        public bool IsMsSql => DbType == DbType.MsSql;
        public bool IsMySql => DbType == DbType.MySql;
        public bool IsSqlite => DbType == DbType.Sqlite;
        public bool IsPostgreSql => DbType == DbType.PostgreSql;
        public bool IsMariaDb => DbType == DbType.MariaDb;
        public bool HasProcedures => IsMsSql && UseStoredProcedures;

#if NETFRAMEWORK
        public Database Database => BaseContext.Database;
#else
        public DatabaseFacade Database => BaseContext.Database;
#endif
        public int SaveChanges() => BaseContext.SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => BaseContext.SaveChangesAsync(cancellationToken);
        public virtual void Dispose() => BaseContext.Dispose();
        public Action<string> Log { get; set; }
        private void WriteToLog(string msg) => Log?.Invoke(msg);
        public static void Init()
        {
#if NetFX
			Database.SetInitializer<MsSqlDbContext>(null);
			Database.SetInitializer<MySqlDbContext>(null);
			Database.SetInitializer<MariaDbDbContext>(null);
			Database.SetInitializer<SqliteDbContext>(null);
			Database.SetInitializer<PostgreSqlDbContext>(null);
			Database.SetInitializer<Context.DbContextBase>(null);
#endif
		}
	}
}

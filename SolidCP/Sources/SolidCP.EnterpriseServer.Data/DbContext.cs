﻿using System;
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

        DbConnection MsSqlDbConnection => new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
#if NETFRAMEWORK
        DbConnection MySqlDbConnection => new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
#else
        DbConnection MySqlDbConnection => new MySqlConnector.MySqlConnection(ConnectionString);

#endif
		DbConnection PostgreSqlDbConnection => new Npgsql.NpgsqlConnection(ConnectionString);

        DbConnection SqliteDbConnection
        {
            get
            {
#if NETFRAMEWORK
				//factory ??= new System.Data.SQLite.EF6.SQLiteProviderFactory();
				/* factory ??= DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
                var conn = factory.CreateConnection();*/
				var csb = new DbConnectionStringBuilder();
                csb.ConnectionString = ConnectionString;
                var dbFile = (string)csb["Data Source"];
                if (!Path.IsPathRooted(dbFile))
                {
                    dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFile);
                    csb["Data Source"] = dbFile;
                    ConnectionString = csb.ToString();
                }
				var conn = new System.Data.SQLite.SQLiteConnection(ConnectionString);
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

#if NETFRAMEWORK
                    DbConfiguration.InitDatabaseProvider(DbType);
#endif

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
                    switch (DbType)
                    {
                        default:
                        case DbType.MsSql: contextType = typeof(MsSqlDbContext); break;
                        case DbType.MySql: contextType = typeof(MySqlDbContext); break;
                        case DbType.MariaDb: contextType = typeof(MySqlDbContext); break;
                        case DbType.PostgreSql: contextType = typeof(PostgreSqlDbContext); break;
                        case DbType.Sqlite: contextType = typeof(SqliteDbContext); break;
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

			BaseContext.Log += WriteToLog;
		}

		public DbContext(string connectionString, DbType dbType = DbType.Unknown, bool initSeedData = false)
        {
            /*if (dbType == DbType.Unknown)
            {
                var csb = new DbConnectionStringBuilder() { ConnectionString = connectionString };
                if (!Enum.TryParse<DbType>((string)(csb["DbType"] ?? "Unknown"), out dbType)) dbType = DbType.Other;
            }*/
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
#else
        public DatabaseFacade Database => BaseContext.Database;
#endif
        public int SaveChanges() => BaseContext.SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => BaseContext.SaveChangesAsync(cancellationToken);
        public virtual void Dispose() => BaseContext.Dispose();
        public Action<string> Log { get; set; }
        private void WriteToLog(string msg) => Log?.Invoke(msg);
	}
}

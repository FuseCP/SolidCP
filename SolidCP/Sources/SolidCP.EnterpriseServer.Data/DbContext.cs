using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;
#if NetFX
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore.Infrastructure;
#endif

namespace SolidCP.EnterpriseServer.Data
{
    public partial class DbContext: IDisposable
	{

        public const bool UseStoredProcedures = true;

        static string connectionString = null;
        public string ConnectionString
        {
            get => connectionString ?? (connectionString = DbSettings.NativeConnectionString);
            set => connectionString = value;
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
        }

        public DbContext(string connectionString, DbType dbType = DbType.Unknown, bool initSeedData = false)
        {
            DbType = dbType;
            ConnectionString = connectionString;
            InitSeedData = initSeedData;
#if NETSTANDARD
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType, this);
#else
			BaseContext = new Context.DbContextBase(this);
#endif
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
        public void Dispose() => BaseContext.Dispose();

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

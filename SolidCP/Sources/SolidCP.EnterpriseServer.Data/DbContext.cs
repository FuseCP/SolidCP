using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;

namespace SolidCP.EnterpriseServer.Data
{
    public enum DbFlavor { Unknown, MsSql, MySql, MariaDb, SqlLite, PostgreSql, Other }
    public partial class DbContext: IDisposable
    {
        static string connectionString = null;
        public string ConnectionString
        {
            get => connectionString ?? (connectionString = DbSettings.SpecificConnectionString);
            set => connectionString = value;
        }
        DbFlavor flavor = DbFlavor.Unknown;
        public DbFlavor Flavor
        {
            get
            {
                if (flavor != DbFlavor.Unknown) return flavor;
                var flavorName = Regex.Match(DbSettings.ConnectionString, @"(?<=(?:;|^)\s*Flavor\s*=\s*)[^;$]*", RegexOptions.IgnoreCase)?.Value.Trim();
                if (!string.IsNullOrEmpty(flavorName) && !Enum.TryParse<DbFlavor>(flavorName, true, out flavor)) flavor = DbFlavor.Other;
                if (flavor == DbFlavor.Other) throw new NotSupportedException($"This DB flavor {flavorName} is not supported");
                return flavor;
            }
            set => flavor = value;
        }

        static Type contextType = null;
        static Type ContextType
        {
            get
            {
                if (contextType == null)
                {
#if NETSTANDARD
                    if (OSInfo.IsCore) contextType = Type.GetType("SolidCP.EnterpriseServer.Context.DbContextBase, SolidCP.EnterpriseServer.Data.Core");
                    else contextType = Type.GetType("SolidCP.EnterpriseServer.Context.DbContextBase, SolidCP.EnterpriseServer.Data.NetFX");
#else
                    contextType = typeof(Context.DbContextBase);
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

        public DbContext(string connectionString, DbFlavor flavor = DbFlavor.Unknown)
        {
            Flavor = flavor;
            ConnectionString = connectionString;
#if NETSTANDARD
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType, this);
#else
			BaseContext = new Context.DbContextBase(this);
#endif
		}


        public int SaveChanges() => BaseContext.SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => BaseContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => BaseContext.Dispose();
    }
}

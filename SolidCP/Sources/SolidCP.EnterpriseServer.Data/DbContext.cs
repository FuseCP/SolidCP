using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;

namespace SolidCP.EnterpriseServer.Data
{
    public enum DbFlavor { Other, MsSql, MySql, MariaDb, SqlLite, PostgreSql }
    public partial class DbContext: IDisposable
    {
        static string connectionString = null;
        public string ConnectionString
        {
            get => connectionString ?? (connectionString = DbSettings.SpecificConnectionString);
            set => connectionString = value;
        }
        public DbFlavor Flavor
        {
            get
            {
                var flavorName = Regex.Match(DbSettings.ConnectionString, @"(?<=(?:;|^)\s*Flavor\s*=\s*)[^;$]*", RegexOptions.IgnoreCase).Value.Trim();
                DbFlavor flavor = DbFlavor.Other;
                if (!Enum.TryParse<DbFlavor>(flavorName, true, out flavor)) flavor = DbFlavor.Other;
                if (flavor == DbFlavor.Other) throw new NotSupportedException($"This DB flavor {flavorName} is not supported");
                return flavor;
            }
        }

        static Type contextType = null;
        static Type ContextType
        {
            get
            {
                if (contextType == null)
                {
#if NETSTANDARD
                    if (OSInfo.IsCore) contextType = Type.GetType("SolidCP.EnterpriseServer.Data.SolidCPBaseContext, SolidCP.EnterpriseServer.Data.Core");
                    else contextType = Type.GetType("SolidCP.EnterpriseServer.Data.SolidCPBaseContext, SolidCP.EnterpriseServer.Data.NetFX");
#else
                    //contextType = typeof(Context.SolidCPBaseContext);
#endif
                }
                return contextType;
            }
        }

        protected IGenericDbContext BaseContext = null;
        public DbContext()
        {
#if NETSTANDARD
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType, this);
#else
            //BaseContext = new Context.SolidCPBaseContext(this);
#endif
        }

        public int SaveChanges() => BaseContext.SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => BaseContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => BaseContext.Dispose();
    }
}

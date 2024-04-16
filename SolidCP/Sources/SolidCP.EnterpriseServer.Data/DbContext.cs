using System;
using System.Threading;
using System.Threading.Tasks;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;

namespace SolidCP.EnterpriseServer.Data
{
    public class DbContext: IDisposable
    {
        static Type contextType = null;
        static Type ContextType
        {
            get
            {
                if (contextType == null)
                {
                    if (OSInfo.IsCore) contextType = Type.GetType("SolidCP.EnterpriseServer.Core.Data.CoreDbContext");
                    else contextType = Type.GetType("SolidCP.EnterpriseServer.NetFX.Data.NetFXDbContext");
                }
                return contextType;
            }
        }

        protected IGenericDbContext BaseContext = null;
        public DbContext()
        {
            BaseContext = (IGenericDbContext)Activator.CreateInstance(ContextType);
        }

        public int SaveChanges() => BaseContext.SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => BaseContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => BaseContext.Dispose();

        DbSet<Base.HostedSolution.AccessToken> accessTokens = null;
        public DbSet<Base.HostedSolution.AccessToken> AccessTokens => accessTokens ?? (accessTokens = new DbSet<Base.HostedSolution.AccessToken>(BaseContext));
    }
}

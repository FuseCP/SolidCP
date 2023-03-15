#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IOCSEdgeServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IOCSEdgeServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSEdgeServer/AddDomain", ReplyAction = "http://smbsaas/solidcp/server/IOCSEdgeServer/AddDomainResponse")]
        void AddDomain(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSEdgeServer/AddDomain", ReplyAction = "http://smbsaas/solidcp/server/IOCSEdgeServer/AddDomainResponse")]
        System.Threading.Tasks.Task AddDomainAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSEdgeServer/DeleteDomain", ReplyAction = "http://smbsaas/solidcp/server/IOCSEdgeServer/DeleteDomainResponse")]
        void DeleteDomain(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOCSEdgeServer/DeleteDomain", ReplyAction = "http://smbsaas/solidcp/server/IOCSEdgeServer/DeleteDomainResponse")]
        System.Threading.Tasks.Task DeleteDomainAsync(string domainName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OCSEdgeServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IOCSEdgeServer
    {
        public void AddDomain(string domainName)
        {
            Invoke("SolidCP.Server.OCSEdgeServer", "AddDomain", domainName);
        }

        public async System.Threading.Tasks.Task AddDomainAsync(string domainName)
        {
            await InvokeAsync("SolidCP.Server.OCSEdgeServer", "AddDomain", domainName);
        }

        public void DeleteDomain(string domainName)
        {
            Invoke("SolidCP.Server.OCSEdgeServer", "DeleteDomain", domainName);
        }

        public async System.Threading.Tasks.Task DeleteDomainAsync(string domainName)
        {
            await InvokeAsync("SolidCP.Server.OCSEdgeServer", "DeleteDomain", domainName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OCSEdgeServer : SolidCP.Web.Client.ClientBase<IOCSEdgeServer, OCSEdgeServerAssemblyClient>, IOCSEdgeServer
    {
        public void AddDomain(string domainName)
        {
            base.Client.AddDomain(domainName);
        }

        public async System.Threading.Tasks.Task AddDomainAsync(string domainName)
        {
            await base.Client.AddDomainAsync(domainName);
        }

        public void DeleteDomain(string domainName)
        {
            base.Client.DeleteDomain(domainName);
        }

        public async System.Threading.Tasks.Task DeleteDomainAsync(string domainName)
        {
            await base.Client.DeleteDomainAsync(domainName);
        }
    }
}
#endif
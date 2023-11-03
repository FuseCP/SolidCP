#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesImport", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesImport
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItemTypes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItemTypesResponse")]
        SolidCP.Providers.ServiceProviderItemType[] /*List*/ GetImportableItemTypes(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItemTypes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItemTypesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ServiceProviderItemType[]> GetImportableItemTypesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItems", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItemsResponse")]
        string[] /*List*/ GetImportableItems(int packageId, int itemTypeId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItems", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesImport/GetImportableItemsResponse")]
        System.Threading.Tasks.Task<string[]> GetImportableItemsAsync(int packageId, int itemTypeId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesImport/ImportItems", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesImport/ImportItemsResponse")]
        int ImportItems(bool async, string taskId, int packageId, string[] items);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesImport/ImportItems", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesImport/ImportItemsResponse")]
        System.Threading.Tasks.Task<int> ImportItemsAsync(bool async, string taskId, int packageId, string[] items);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esImportAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesImport
    {
        public SolidCP.Providers.ServiceProviderItemType[] /*List*/ GetImportableItemTypes(int packageId)
        {
            return Invoke<SolidCP.Providers.ServiceProviderItemType[], SolidCP.Providers.ServiceProviderItemType>("SolidCP.EnterpriseServer.esImport", "GetImportableItemTypes", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServiceProviderItemType[]> GetImportableItemTypesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.ServiceProviderItemType[], SolidCP.Providers.ServiceProviderItemType>("SolidCP.EnterpriseServer.esImport", "GetImportableItemTypes", packageId);
        }

        public string[] /*List*/ GetImportableItems(int packageId, int itemTypeId)
        {
            return Invoke<string[], string>("SolidCP.EnterpriseServer.esImport", "GetImportableItems", packageId, itemTypeId);
        }

        public async System.Threading.Tasks.Task<string[]> GetImportableItemsAsync(int packageId, int itemTypeId)
        {
            return await InvokeAsync<string[], string>("SolidCP.EnterpriseServer.esImport", "GetImportableItems", packageId, itemTypeId);
        }

        public int ImportItems(bool async, string taskId, int packageId, string[] items)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esImport", "ImportItems", async, taskId, packageId, items);
        }

        public async System.Threading.Tasks.Task<int> ImportItemsAsync(bool async, string taskId, int packageId, string[] items)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esImport", "ImportItems", async, taskId, packageId, items);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esImport : SolidCP.Web.Client.ClientBase<IesImport, esImportAssemblyClient>, IesImport
    {
        public SolidCP.Providers.ServiceProviderItemType[] /*List*/ GetImportableItemTypes(int packageId)
        {
            return base.Client.GetImportableItemTypes(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServiceProviderItemType[]> GetImportableItemTypesAsync(int packageId)
        {
            return await base.Client.GetImportableItemTypesAsync(packageId);
        }

        public string[] /*List*/ GetImportableItems(int packageId, int itemTypeId)
        {
            return base.Client.GetImportableItems(packageId, itemTypeId);
        }

        public async System.Threading.Tasks.Task<string[]> GetImportableItemsAsync(int packageId, int itemTypeId)
        {
            return await base.Client.GetImportableItemsAsync(packageId, itemTypeId);
        }

        public int ImportItems(bool async, string taskId, int packageId, string[] items)
        {
            return base.Client.ImportItems(async, taskId, packageId, items);
        }

        public async System.Threading.Tasks.Task<int> ImportItemsAsync(bool async, string taskId, int packageId, string[] items)
        {
            return await base.Client.ImportItemsAsync(async, taskId, packageId, items);
        }
    }
}
#endif
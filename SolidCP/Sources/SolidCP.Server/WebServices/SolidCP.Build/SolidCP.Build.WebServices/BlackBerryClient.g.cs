#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IBlackBerry", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IBlackBerry
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUserResponse")]
        SolidCP.Providers.Common.ResultObject CreateBlackBerryUser(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/CreateBlackBerryUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateBlackBerryUserAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUserResponse")]
        SolidCP.Providers.Common.ResultObject DeleteBlackBerryUser(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUser", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteBlackBerryUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteBlackBerryUserAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStats", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStatsResponse")]
        SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStats", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/GetBlackBerryUserStatsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTime", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTimeResponse")]
        SolidCP.Providers.Common.ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTime", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetActivationPasswordWithExpirationTimeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetActivationPasswordWithExpirationTimeAsync(string primaryEmailAddress, string password, int time);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPassword", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPasswordResponse")]
        SolidCP.Providers.Common.ResultObject SetEmailActivationPassword(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPassword", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/SetEmailActivationPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEmailActivationPasswordAsync(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDevice", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDeviceResponse")]
        SolidCP.Providers.Common.ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDevice", ReplyAction = "http://smbsaas/solidcp/server/IBlackBerry/DeleteDataFromBlackBerryDeviceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteDataFromBlackBerryDeviceAsync(string primaryEmailAddress);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class BlackBerryAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IBlackBerry
    {
        public SolidCP.Providers.Common.ResultObject CreateBlackBerryUser(string primaryEmailAddress)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "CreateBlackBerryUser", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "CreateBlackBerryUser", primaryEmailAddress);
        }

        public SolidCP.Providers.Common.ResultObject DeleteBlackBerryUser(string primaryEmailAddress)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "DeleteBlackBerryUser", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "DeleteBlackBerryUser", primaryEmailAddress);
        }

        public SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress)
        {
            return Invoke<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult>("SolidCP.Server.BlackBerry", "GetBlackBerryUserStats", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult>("SolidCP.Server.BlackBerry", "GetBlackBerryUserStats", primaryEmailAddress);
        }

        public SolidCP.Providers.Common.ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "SetActivationPasswordWithExpirationTime", primaryEmailAddress, password, time);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetActivationPasswordWithExpirationTimeAsync(string primaryEmailAddress, string password, int time)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "SetActivationPasswordWithExpirationTime", primaryEmailAddress, password, time);
        }

        public SolidCP.Providers.Common.ResultObject SetEmailActivationPassword(string primaryEmailAddress)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "SetEmailActivationPassword", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEmailActivationPasswordAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "SetEmailActivationPassword", primaryEmailAddress);
        }

        public SolidCP.Providers.Common.ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "DeleteDataFromBlackBerryDevice", primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteDataFromBlackBerryDeviceAsync(string primaryEmailAddress)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.BlackBerry", "DeleteDataFromBlackBerryDevice", primaryEmailAddress);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class BlackBerry : SolidCP.Web.Client.ClientBase<IBlackBerry, BlackBerryAssemblyClient>, IBlackBerry
    {
        public SolidCP.Providers.Common.ResultObject CreateBlackBerryUser(string primaryEmailAddress)
        {
            return base.Client.CreateBlackBerryUser(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await base.Client.CreateBlackBerryUserAsync(primaryEmailAddress);
        }

        public SolidCP.Providers.Common.ResultObject DeleteBlackBerryUser(string primaryEmailAddress)
        {
            return base.Client.DeleteBlackBerryUser(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteBlackBerryUserAsync(string primaryEmailAddress)
        {
            return await base.Client.DeleteBlackBerryUserAsync(primaryEmailAddress);
        }

        public SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress)
        {
            return base.Client.GetBlackBerryUserStats(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BlackBerryUserStatsResult> GetBlackBerryUserStatsAsync(string primaryEmailAddress)
        {
            return await base.Client.GetBlackBerryUserStatsAsync(primaryEmailAddress);
        }

        public SolidCP.Providers.Common.ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time)
        {
            return base.Client.SetActivationPasswordWithExpirationTime(primaryEmailAddress, password, time);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetActivationPasswordWithExpirationTimeAsync(string primaryEmailAddress, string password, int time)
        {
            return await base.Client.SetActivationPasswordWithExpirationTimeAsync(primaryEmailAddress, password, time);
        }

        public SolidCP.Providers.Common.ResultObject SetEmailActivationPassword(string primaryEmailAddress)
        {
            return base.Client.SetEmailActivationPassword(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEmailActivationPasswordAsync(string primaryEmailAddress)
        {
            return await base.Client.SetEmailActivationPasswordAsync(primaryEmailAddress);
        }

        public SolidCP.Providers.Common.ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress)
        {
            return base.Client.DeleteDataFromBlackBerryDevice(primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteDataFromBlackBerryDeviceAsync(string primaryEmailAddress)
        {
            return await base.Client.DeleteDataFromBlackBerryDeviceAsync(primaryEmailAddress);
        }
    }
}
#endif
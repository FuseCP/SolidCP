#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("CommonPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesAuthentication", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesAuthentication
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/AuthenticateUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/AuthenticateUserResponse")]
        int AuthenticateUser(string username, string password, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/AuthenticateUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/AuthenticateUserResponse")]
        System.Threading.Tasks.Task<int> AuthenticateUserAsync(string username, string password, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetUserByUsernamePassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetUserByUsernamePasswordResponse")]
        SolidCP.EnterpriseServer.UserInfo GetUserByUsernamePassword(string username, string password, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetUserByUsernamePassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetUserByUsernamePasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByUsernamePasswordAsync(string username, string password, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ChangeUserPasswordByUsername", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ChangeUserPasswordByUsernameResponse")]
        int ChangeUserPasswordByUsername(string username, string oldPassword, string newPassword, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ChangeUserPasswordByUsername", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ChangeUserPasswordByUsernameResponse")]
        System.Threading.Tasks.Task<int> ChangeUserPasswordByUsernameAsync(string username, string oldPassword, string newPassword, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPasswordReminder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPasswordReminderResponse")]
        int SendPasswordReminder(string username, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPasswordReminder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPasswordReminderResponse")]
        System.Threading.Tasks.Task<int> SendPasswordReminderAsync(string username, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetSystemSetupMode", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetSystemSetupModeResponse")]
        bool GetSystemSetupMode();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetSystemSetupMode", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetSystemSetupModeResponse")]
        System.Threading.Tasks.Task<bool> GetSystemSetupModeAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SetupControlPanelAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SetupControlPanelAccountsResponse")]
        int SetupControlPanelAccounts(string passwordA, string passwordB, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SetupControlPanelAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SetupControlPanelAccountsResponse")]
        System.Threading.Tasks.Task<int> SetupControlPanelAccountsAsync(string passwordA, string passwordB, string ip);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetLoginThemes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetLoginThemesResponse")]
        System.Data.DataSet GetLoginThemes();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetLoginThemes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/GetLoginThemesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetLoginThemesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ValidatePin", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ValidatePinResponse")]
        bool ValidatePin(string username, string pin);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ValidatePin", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/ValidatePinResponse")]
        System.Threading.Tasks.Task<bool> ValidatePinAsync(string username, string pin);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPin", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPinResponse")]
        int SendPin(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPin", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuthentication/SendPinResponse")]
        System.Threading.Tasks.Task<int> SendPinAsync(string username);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esAuthenticationAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesAuthentication
    {
        public int AuthenticateUser(string username, string password, string ip)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esAuthentication", "AuthenticateUser", username, password, ip);
        }

        public async System.Threading.Tasks.Task<int> AuthenticateUserAsync(string username, string password, string ip)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esAuthentication", "AuthenticateUser", username, password, ip);
        }

        public SolidCP.EnterpriseServer.UserInfo GetUserByUsernamePassword(string username, string password, string ip)
        {
            return Invoke<SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esAuthentication", "GetUserByUsernamePassword", username, password, ip);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByUsernamePasswordAsync(string username, string password, string ip)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esAuthentication", "GetUserByUsernamePassword", username, password, ip);
        }

        public int ChangeUserPasswordByUsername(string username, string oldPassword, string newPassword, string ip)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esAuthentication", "ChangeUserPasswordByUsername", username, oldPassword, newPassword, ip);
        }

        public async System.Threading.Tasks.Task<int> ChangeUserPasswordByUsernameAsync(string username, string oldPassword, string newPassword, string ip)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esAuthentication", "ChangeUserPasswordByUsername", username, oldPassword, newPassword, ip);
        }

        public int SendPasswordReminder(string username, string ip)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esAuthentication", "SendPasswordReminder", username, ip);
        }

        public async System.Threading.Tasks.Task<int> SendPasswordReminderAsync(string username, string ip)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esAuthentication", "SendPasswordReminder", username, ip);
        }

        public bool GetSystemSetupMode()
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esAuthentication", "GetSystemSetupMode");
        }

        public async System.Threading.Tasks.Task<bool> GetSystemSetupModeAsync()
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esAuthentication", "GetSystemSetupMode");
        }

        public int SetupControlPanelAccounts(string passwordA, string passwordB, string ip)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esAuthentication", "SetupControlPanelAccounts", passwordA, passwordB, ip);
        }

        public async System.Threading.Tasks.Task<int> SetupControlPanelAccountsAsync(string passwordA, string passwordB, string ip)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esAuthentication", "SetupControlPanelAccounts", passwordA, passwordB, ip);
        }

        public System.Data.DataSet GetLoginThemes()
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuthentication", "GetLoginThemes");
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetLoginThemesAsync()
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuthentication", "GetLoginThemes");
        }

        public bool ValidatePin(string username, string pin)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esAuthentication", "ValidatePin", username, pin);
        }

        public async System.Threading.Tasks.Task<bool> ValidatePinAsync(string username, string pin)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esAuthentication", "ValidatePin", username, pin);
        }

        public int SendPin(string username)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esAuthentication", "SendPin", username);
        }

        public async System.Threading.Tasks.Task<int> SendPinAsync(string username)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esAuthentication", "SendPin", username);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esAuthentication : SolidCP.Web.Client.ClientBase<IesAuthentication, esAuthenticationAssemblyClient>, IesAuthentication
    {
        public int AuthenticateUser(string username, string password, string ip)
        {
            return base.Client.AuthenticateUser(username, password, ip);
        }

        public async System.Threading.Tasks.Task<int> AuthenticateUserAsync(string username, string password, string ip)
        {
            return await base.Client.AuthenticateUserAsync(username, password, ip);
        }

        public SolidCP.EnterpriseServer.UserInfo GetUserByUsernamePassword(string username, string password, string ip)
        {
            return base.Client.GetUserByUsernamePassword(username, password, ip);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByUsernamePasswordAsync(string username, string password, string ip)
        {
            return await base.Client.GetUserByUsernamePasswordAsync(username, password, ip);
        }

        public int ChangeUserPasswordByUsername(string username, string oldPassword, string newPassword, string ip)
        {
            return base.Client.ChangeUserPasswordByUsername(username, oldPassword, newPassword, ip);
        }

        public async System.Threading.Tasks.Task<int> ChangeUserPasswordByUsernameAsync(string username, string oldPassword, string newPassword, string ip)
        {
            return await base.Client.ChangeUserPasswordByUsernameAsync(username, oldPassword, newPassword, ip);
        }

        public int SendPasswordReminder(string username, string ip)
        {
            return base.Client.SendPasswordReminder(username, ip);
        }

        public async System.Threading.Tasks.Task<int> SendPasswordReminderAsync(string username, string ip)
        {
            return await base.Client.SendPasswordReminderAsync(username, ip);
        }

        public bool GetSystemSetupMode()
        {
            return base.Client.GetSystemSetupMode();
        }

        public async System.Threading.Tasks.Task<bool> GetSystemSetupModeAsync()
        {
            return await base.Client.GetSystemSetupModeAsync();
        }

        public int SetupControlPanelAccounts(string passwordA, string passwordB, string ip)
        {
            return base.Client.SetupControlPanelAccounts(passwordA, passwordB, ip);
        }

        public async System.Threading.Tasks.Task<int> SetupControlPanelAccountsAsync(string passwordA, string passwordB, string ip)
        {
            return await base.Client.SetupControlPanelAccountsAsync(passwordA, passwordB, ip);
        }

        public System.Data.DataSet GetLoginThemes()
        {
            return base.Client.GetLoginThemes();
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetLoginThemesAsync()
        {
            return await base.Client.GetLoginThemesAsync();
        }

        public bool ValidatePin(string username, string pin)
        {
            return base.Client.ValidatePin(username, pin);
        }

        public async System.Threading.Tasks.Task<bool> ValidatePinAsync(string username, string pin)
        {
            return await base.Client.ValidatePinAsync(username, pin);
        }

        public int SendPin(string username)
        {
            return base.Client.SendPin(username);
        }

        public async System.Threading.Tasks.Task<int> SendPinAsync(string username)
        {
            return await base.Client.SendPinAsync(username);
        }
    }
}
#endif
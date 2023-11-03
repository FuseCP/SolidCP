#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesUsers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesUsers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UserExists", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UserExistsResponse")]
        bool UserExists(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UserExists", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UserExistsResponse")]
        System.Threading.Tasks.Task<bool> UserExistsAsync(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserByIdResponse")]
        SolidCP.EnterpriseServer.UserInfo GetUserById(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByIdAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserByUsername", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserByUsernameResponse")]
        SolidCP.EnterpriseServer.UserInfo GetUserByUsername(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserByUsername", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserByUsernameResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByUsernameAsync(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersResponse")]
        SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUsers(int ownerId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUsersAsync(int ownerId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserVLan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserVLanResponse")]
        void AddUserVLan(int userId, SolidCP.EnterpriseServer.UserVlan vLan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserVLan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserVLanResponse")]
        System.Threading.Tasks.Task AddUserVLanAsync(int userId, SolidCP.EnterpriseServer.UserVlan vLan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserVLan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserVLanResponse")]
        void DeleteUserVLan(int userId, ushort vLanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserVLan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserVLanResponse")]
        System.Threading.Tasks.Task DeleteUserVLanAsync(int userId, ushort vLanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUsersResponse")]
        System.Data.DataSet GetRawUsers(int ownerId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUsersResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawUsersAsync(int ownerId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPagedResponse")]
        System.Data.DataSet GetUsersPaged(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetUsersPagedAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPagedRecursive", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPagedRecursiveResponse")]
        System.Data.DataSet GetUsersPagedRecursive(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPagedRecursive", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersPagedRecursiveResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetUsersPagedRecursiveAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersSummary", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersSummaryResponse")]
        System.Data.DataSet GetUsersSummary(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersSummary", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUsersSummaryResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetUsersSummaryAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserDomainsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserDomainsPagedResponse")]
        System.Data.DataSet GetUserDomainsPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserDomainsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserDomainsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetUserDomainsPagedAsync(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUserPeers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUserPeersResponse")]
        System.Data.DataSet GetRawUserPeers(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUserPeers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetRawUserPeersResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawUserPeersAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserPeers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserPeersResponse")]
        SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUserPeers(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserPeers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserPeersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUserPeersAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserParents", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserParentsResponse")]
        SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUserParents(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserParents", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserParentsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUserParentsAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserResponse")]
        int AddUser(SolidCP.EnterpriseServer.UserInfo user, bool sendLetter, string password, string[] notes);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserResponse")]
        System.Threading.Tasks.Task<int> AddUserAsync(SolidCP.EnterpriseServer.UserInfo user, bool sendLetter, string password, string[] notes);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserLiteralResponse")]
        int AddUserLiteral(int ownerId, int roleId, int statusId, bool isPeer, bool isDemo, string username, string password, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled, bool sendLetter);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/AddUserLiteralResponse")]
        System.Threading.Tasks.Task<int> AddUserLiteralAsync(int ownerId, int roleId, int statusId, bool isPeer, bool isDemo, string username, string password, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled, bool sendLetter);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTaskResponse")]
        int UpdateUserTask(string taskId, SolidCP.EnterpriseServer.UserInfo user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTaskResponse")]
        System.Threading.Tasks.Task<int> UpdateUserTaskAsync(string taskId, SolidCP.EnterpriseServer.UserInfo user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTaskAsynchronously", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTaskAsynchronouslyResponse")]
        int UpdateUserTaskAsynchronously(string taskId, SolidCP.EnterpriseServer.UserInfo user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTaskAsynchronously", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserTaskAsynchronouslyResponse")]
        System.Threading.Tasks.Task<int> UpdateUserTaskAsynchronouslyAsync(string taskId, SolidCP.EnterpriseServer.UserInfo user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserResponse")]
        int UpdateUser(SolidCP.EnterpriseServer.UserInfo user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserResponse")]
        System.Threading.Tasks.Task<int> UpdateUserAsync(SolidCP.EnterpriseServer.UserInfo user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserLiteralResponse")]
        int UpdateUserLiteral(int userId, int roleId, int statusId, bool isPeer, bool isDemo, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserLiteralResponse")]
        System.Threading.Tasks.Task<int> UpdateUserLiteralAsync(int userId, int roleId, int statusId, bool isPeer, bool isDemo, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserResponse")]
        int DeleteUser(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserResponse")]
        System.Threading.Tasks.Task<int> DeleteUserAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserPasswordResponse")]
        int ChangeUserPassword(int userId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserPasswordResponse")]
        System.Threading.Tasks.Task<int> ChangeUserPasswordAsync(int userId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserMfa", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserMfaResponse")]
        bool UpdateUserMfa(string username, bool activate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserMfa", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserMfaResponse")]
        System.Threading.Tasks.Task<bool> UpdateUserMfaAsync(string username, bool activate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserMfaQrCodeData", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserMfaQrCodeDataResponse")]
        string[] GetUserMfaQrCodeData(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserMfaQrCodeData", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserMfaQrCodeDataResponse")]
        System.Threading.Tasks.Task<string[]> GetUserMfaQrCodeDataAsync(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ActivateUserMfaQrCode", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ActivateUserMfaQrCodeResponse")]
        bool ActivateUserMfaQrCode(string username, string pin);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ActivateUserMfaQrCode", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ActivateUserMfaQrCodeResponse")]
        System.Threading.Tasks.Task<bool> ActivateUserMfaQrCodeAsync(string username, string pin);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserStatusResponse")]
        int ChangeUserStatus(int userId, SolidCP.EnterpriseServer.UserStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/ChangeUserStatusResponse")]
        System.Threading.Tasks.Task<int> ChangeUserStatusAsync(int userId, SolidCP.EnterpriseServer.UserStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserSettingsResponse")]
        SolidCP.EnterpriseServer.UserSettings GetUserSettings(int userId, string settingsName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserSettings> GetUserSettingsAsync(int userId, string settingsName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserSettingsResponse")]
        int UpdateUserSettings(SolidCP.EnterpriseServer.UserSettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserSettingsResponse")]
        System.Threading.Tasks.Task<int> UpdateUserSettingsAsync(SolidCP.EnterpriseServer.UserSettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserThemeSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserThemeSettingsResponse")]
        System.Data.DataSet GetUserThemeSettings(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserThemeSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/GetUserThemeSettingsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetUserThemeSettingsAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserThemeSetting", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserThemeSettingResponse")]
        void UpdateUserThemeSetting(int userId, string PropertyName, string PropertyValue);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserThemeSetting", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/UpdateUserThemeSettingResponse")]
        System.Threading.Tasks.Task UpdateUserThemeSettingAsync(int userId, string PropertyName, string PropertyValue);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserThemeSetting", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserThemeSettingResponse")]
        void DeleteUserThemeSetting(int userId, string PropertyName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserThemeSetting", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesUsers/DeleteUserThemeSettingResponse")]
        System.Threading.Tasks.Task DeleteUserThemeSettingAsync(int userId, string PropertyName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esUsersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesUsers
    {
        public bool UserExists(string username)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esUsers", "UserExists", username);
        }

        public async System.Threading.Tasks.Task<bool> UserExistsAsync(string username)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esUsers", "UserExists", username);
        }

        public SolidCP.EnterpriseServer.UserInfo GetUserById(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserById", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByIdAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserById", userId);
        }

        public SolidCP.EnterpriseServer.UserInfo GetUserByUsername(string username)
        {
            return Invoke<SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserByUsername", username);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByUsernameAsync(string username)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserByUsername", username);
        }

        public SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUsers(int ownerId, bool recursive)
        {
            return Invoke<SolidCP.EnterpriseServer.UserInfo[], SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUsers", ownerId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUsersAsync(int ownerId, bool recursive)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.UserInfo[], SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUsers", ownerId, recursive);
        }

        public void AddUserVLan(int userId, SolidCP.EnterpriseServer.UserVlan vLan)
        {
            Invoke("SolidCP.EnterpriseServer.esUsers", "AddUserVLan", userId, vLan);
        }

        public async System.Threading.Tasks.Task AddUserVLanAsync(int userId, SolidCP.EnterpriseServer.UserVlan vLan)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esUsers", "AddUserVLan", userId, vLan);
        }

        public void DeleteUserVLan(int userId, ushort vLanId)
        {
            Invoke("SolidCP.EnterpriseServer.esUsers", "DeleteUserVLan", userId, vLanId);
        }

        public async System.Threading.Tasks.Task DeleteUserVLanAsync(int userId, ushort vLanId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esUsers", "DeleteUserVLan", userId, vLanId);
        }

        public System.Data.DataSet GetRawUsers(int ownerId, bool recursive)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetRawUsers", ownerId, recursive);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawUsersAsync(int ownerId, bool recursive)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetRawUsers", ownerId, recursive);
        }

        public System.Data.DataSet GetUsersPaged(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUsersPaged", userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUsersPagedAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUsersPaged", userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetUsersPagedRecursive(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUsersPagedRecursive", userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUsersPagedRecursiveAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUsersPagedRecursive", userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetUsersSummary(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUsersSummary", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUsersSummaryAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUsersSummary", userId);
        }

        public System.Data.DataSet GetUserDomainsPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUserDomainsPaged", userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUserDomainsPagedAsync(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUserDomainsPaged", userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetRawUserPeers(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetRawUserPeers", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawUserPeersAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetRawUserPeers", userId);
        }

        public SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUserPeers(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.UserInfo[], SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserPeers", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUserPeersAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.UserInfo[], SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserPeers", userId);
        }

        public SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUserParents(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.UserInfo[], SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserParents", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUserParentsAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.UserInfo[], SolidCP.EnterpriseServer.UserInfo>("SolidCP.EnterpriseServer.esUsers", "GetUserParents", userId);
        }

        public int AddUser(SolidCP.EnterpriseServer.UserInfo user, bool sendLetter, string password, string[] notes)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "AddUser", user, sendLetter, password, notes);
        }

        public async System.Threading.Tasks.Task<int> AddUserAsync(SolidCP.EnterpriseServer.UserInfo user, bool sendLetter, string password, string[] notes)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "AddUser", user, sendLetter, password, notes);
        }

        public int AddUserLiteral(int ownerId, int roleId, int statusId, bool isPeer, bool isDemo, string username, string password, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled, bool sendLetter)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "AddUserLiteral", ownerId, roleId, statusId, isPeer, isDemo, username, password, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled, sendLetter);
        }

        public async System.Threading.Tasks.Task<int> AddUserLiteralAsync(int ownerId, int roleId, int statusId, bool isPeer, bool isDemo, string username, string password, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled, bool sendLetter)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "AddUserLiteral", ownerId, roleId, statusId, isPeer, isDemo, username, password, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled, sendLetter);
        }

        public int UpdateUserTask(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserTask", taskId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserTaskAsync(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserTask", taskId, user);
        }

        public int UpdateUserTaskAsynchronously(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserTaskAsynchronously", taskId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserTaskAsynchronouslyAsync(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserTaskAsynchronously", taskId, user);
        }

        public int UpdateUser(SolidCP.EnterpriseServer.UserInfo user)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUser", user);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserAsync(SolidCP.EnterpriseServer.UserInfo user)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUser", user);
        }

        public int UpdateUserLiteral(int userId, int roleId, int statusId, bool isPeer, bool isDemo, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserLiteral", userId, roleId, statusId, isPeer, isDemo, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserLiteralAsync(int userId, int roleId, int statusId, bool isPeer, bool isDemo, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserLiteral", userId, roleId, statusId, isPeer, isDemo, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled);
        }

        public int DeleteUser(int userId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "DeleteUser", userId);
        }

        public async System.Threading.Tasks.Task<int> DeleteUserAsync(int userId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "DeleteUser", userId);
        }

        public int ChangeUserPassword(int userId, string password)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "ChangeUserPassword", userId, password);
        }

        public async System.Threading.Tasks.Task<int> ChangeUserPasswordAsync(int userId, string password)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "ChangeUserPassword", userId, password);
        }

        public bool UpdateUserMfa(string username, bool activate)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esUsers", "UpdateUserMfa", username, activate);
        }

        public async System.Threading.Tasks.Task<bool> UpdateUserMfaAsync(string username, bool activate)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esUsers", "UpdateUserMfa", username, activate);
        }

        public string[] GetUserMfaQrCodeData(string username)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esUsers", "GetUserMfaQrCodeData", username);
        }

        public async System.Threading.Tasks.Task<string[]> GetUserMfaQrCodeDataAsync(string username)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esUsers", "GetUserMfaQrCodeData", username);
        }

        public bool ActivateUserMfaQrCode(string username, string pin)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esUsers", "ActivateUserMfaQrCode", username, pin);
        }

        public async System.Threading.Tasks.Task<bool> ActivateUserMfaQrCodeAsync(string username, string pin)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esUsers", "ActivateUserMfaQrCode", username, pin);
        }

        public int ChangeUserStatus(int userId, SolidCP.EnterpriseServer.UserStatus status)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "ChangeUserStatus", userId, status);
        }

        public async System.Threading.Tasks.Task<int> ChangeUserStatusAsync(int userId, SolidCP.EnterpriseServer.UserStatus status)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "ChangeUserStatus", userId, status);
        }

        public SolidCP.EnterpriseServer.UserSettings GetUserSettings(int userId, string settingsName)
        {
            return Invoke<SolidCP.EnterpriseServer.UserSettings>("SolidCP.EnterpriseServer.esUsers", "GetUserSettings", userId, settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserSettings> GetUserSettingsAsync(int userId, string settingsName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.UserSettings>("SolidCP.EnterpriseServer.esUsers", "GetUserSettings", userId, settingsName);
        }

        public int UpdateUserSettings(SolidCP.EnterpriseServer.UserSettings settings)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserSettings", settings);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserSettingsAsync(SolidCP.EnterpriseServer.UserSettings settings)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esUsers", "UpdateUserSettings", settings);
        }

        public System.Data.DataSet GetUserThemeSettings(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUserThemeSettings", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUserThemeSettingsAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esUsers", "GetUserThemeSettings", userId);
        }

        public void UpdateUserThemeSetting(int userId, string PropertyName, string PropertyValue)
        {
            Invoke("SolidCP.EnterpriseServer.esUsers", "UpdateUserThemeSetting", userId, PropertyName, PropertyValue);
        }

        public async System.Threading.Tasks.Task UpdateUserThemeSettingAsync(int userId, string PropertyName, string PropertyValue)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esUsers", "UpdateUserThemeSetting", userId, PropertyName, PropertyValue);
        }

        public void DeleteUserThemeSetting(int userId, string PropertyName)
        {
            Invoke("SolidCP.EnterpriseServer.esUsers", "DeleteUserThemeSetting", userId, PropertyName);
        }

        public async System.Threading.Tasks.Task DeleteUserThemeSettingAsync(int userId, string PropertyName)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esUsers", "DeleteUserThemeSetting", userId, PropertyName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esUsers : SolidCP.Web.Client.ClientBase<IesUsers, esUsersAssemblyClient>, IesUsers
    {
        public bool UserExists(string username)
        {
            return base.Client.UserExists(username);
        }

        public async System.Threading.Tasks.Task<bool> UserExistsAsync(string username)
        {
            return await base.Client.UserExistsAsync(username);
        }

        public SolidCP.EnterpriseServer.UserInfo GetUserById(int userId)
        {
            return base.Client.GetUserById(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByIdAsync(int userId)
        {
            return await base.Client.GetUserByIdAsync(userId);
        }

        public SolidCP.EnterpriseServer.UserInfo GetUserByUsername(string username)
        {
            return base.Client.GetUserByUsername(username);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo> GetUserByUsernameAsync(string username)
        {
            return await base.Client.GetUserByUsernameAsync(username);
        }

        public SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUsers(int ownerId, bool recursive)
        {
            return base.Client.GetUsers(ownerId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUsersAsync(int ownerId, bool recursive)
        {
            return await base.Client.GetUsersAsync(ownerId, recursive);
        }

        public void AddUserVLan(int userId, SolidCP.EnterpriseServer.UserVlan vLan)
        {
            base.Client.AddUserVLan(userId, vLan);
        }

        public async System.Threading.Tasks.Task AddUserVLanAsync(int userId, SolidCP.EnterpriseServer.UserVlan vLan)
        {
            await base.Client.AddUserVLanAsync(userId, vLan);
        }

        public void DeleteUserVLan(int userId, ushort vLanId)
        {
            base.Client.DeleteUserVLan(userId, vLanId);
        }

        public async System.Threading.Tasks.Task DeleteUserVLanAsync(int userId, ushort vLanId)
        {
            await base.Client.DeleteUserVLanAsync(userId, vLanId);
        }

        public System.Data.DataSet GetRawUsers(int ownerId, bool recursive)
        {
            return base.Client.GetRawUsers(ownerId, recursive);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawUsersAsync(int ownerId, bool recursive)
        {
            return await base.Client.GetRawUsersAsync(ownerId, recursive);
        }

        public System.Data.DataSet GetUsersPaged(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetUsersPaged(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUsersPagedAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetUsersPagedAsync(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetUsersPagedRecursive(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetUsersPagedRecursive(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUsersPagedRecursiveAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetUsersPagedRecursiveAsync(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetUsersSummary(int userId)
        {
            return base.Client.GetUsersSummary(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUsersSummaryAsync(int userId)
        {
            return await base.Client.GetUsersSummaryAsync(userId);
        }

        public System.Data.DataSet GetUserDomainsPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetUserDomainsPaged(userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUserDomainsPagedAsync(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetUserDomainsPagedAsync(userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetRawUserPeers(int userId)
        {
            return base.Client.GetRawUserPeers(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawUserPeersAsync(int userId)
        {
            return await base.Client.GetRawUserPeersAsync(userId);
        }

        public SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUserPeers(int userId)
        {
            return base.Client.GetUserPeers(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUserPeersAsync(int userId)
        {
            return await base.Client.GetUserPeersAsync(userId);
        }

        public SolidCP.EnterpriseServer.UserInfo[] /*List*/ GetUserParents(int userId)
        {
            return base.Client.GetUserParents(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserInfo[]> GetUserParentsAsync(int userId)
        {
            return await base.Client.GetUserParentsAsync(userId);
        }

        public int AddUser(SolidCP.EnterpriseServer.UserInfo user, bool sendLetter, string password, string[] notes)
        {
            return base.Client.AddUser(user, sendLetter, password, notes);
        }

        public async System.Threading.Tasks.Task<int> AddUserAsync(SolidCP.EnterpriseServer.UserInfo user, bool sendLetter, string password, string[] notes)
        {
            return await base.Client.AddUserAsync(user, sendLetter, password, notes);
        }

        public int AddUserLiteral(int ownerId, int roleId, int statusId, bool isPeer, bool isDemo, string username, string password, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled, bool sendLetter)
        {
            return base.Client.AddUserLiteral(ownerId, roleId, statusId, isPeer, isDemo, username, password, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled, sendLetter);
        }

        public async System.Threading.Tasks.Task<int> AddUserLiteralAsync(int ownerId, int roleId, int statusId, bool isPeer, bool isDemo, string username, string password, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled, bool sendLetter)
        {
            return await base.Client.AddUserLiteralAsync(ownerId, roleId, statusId, isPeer, isDemo, username, password, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled, sendLetter);
        }

        public int UpdateUserTask(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return base.Client.UpdateUserTask(taskId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserTaskAsync(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return await base.Client.UpdateUserTaskAsync(taskId, user);
        }

        public int UpdateUserTaskAsynchronously(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return base.Client.UpdateUserTaskAsynchronously(taskId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserTaskAsynchronouslyAsync(string taskId, SolidCP.EnterpriseServer.UserInfo user)
        {
            return await base.Client.UpdateUserTaskAsynchronouslyAsync(taskId, user);
        }

        public int UpdateUser(SolidCP.EnterpriseServer.UserInfo user)
        {
            return base.Client.UpdateUser(user);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserAsync(SolidCP.EnterpriseServer.UserInfo user)
        {
            return await base.Client.UpdateUserAsync(user);
        }

        public int UpdateUserLiteral(int userId, int roleId, int statusId, bool isPeer, bool isDemo, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled)
        {
            return base.Client.UpdateUserLiteral(userId, roleId, statusId, isPeer, isDemo, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserLiteralAsync(int userId, int roleId, int statusId, bool isPeer, bool isDemo, string firstName, string lastName, string email, string secondaryEmail, string address, string city, string country, string state, string zip, string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail, string companyName, bool ecommerceEnabled)
        {
            return await base.Client.UpdateUserLiteralAsync(userId, roleId, statusId, isPeer, isDemo, firstName, lastName, email, secondaryEmail, address, city, country, state, zip, primaryPhone, secondaryPhone, fax, instantMessenger, htmlMail, companyName, ecommerceEnabled);
        }

        public int DeleteUser(int userId)
        {
            return base.Client.DeleteUser(userId);
        }

        public async System.Threading.Tasks.Task<int> DeleteUserAsync(int userId)
        {
            return await base.Client.DeleteUserAsync(userId);
        }

        public int ChangeUserPassword(int userId, string password)
        {
            return base.Client.ChangeUserPassword(userId, password);
        }

        public async System.Threading.Tasks.Task<int> ChangeUserPasswordAsync(int userId, string password)
        {
            return await base.Client.ChangeUserPasswordAsync(userId, password);
        }

        public bool UpdateUserMfa(string username, bool activate)
        {
            return base.Client.UpdateUserMfa(username, activate);
        }

        public async System.Threading.Tasks.Task<bool> UpdateUserMfaAsync(string username, bool activate)
        {
            return await base.Client.UpdateUserMfaAsync(username, activate);
        }

        public string[] GetUserMfaQrCodeData(string username)
        {
            return base.Client.GetUserMfaQrCodeData(username);
        }

        public async System.Threading.Tasks.Task<string[]> GetUserMfaQrCodeDataAsync(string username)
        {
            return await base.Client.GetUserMfaQrCodeDataAsync(username);
        }

        public bool ActivateUserMfaQrCode(string username, string pin)
        {
            return base.Client.ActivateUserMfaQrCode(username, pin);
        }

        public async System.Threading.Tasks.Task<bool> ActivateUserMfaQrCodeAsync(string username, string pin)
        {
            return await base.Client.ActivateUserMfaQrCodeAsync(username, pin);
        }

        public int ChangeUserStatus(int userId, SolidCP.EnterpriseServer.UserStatus status)
        {
            return base.Client.ChangeUserStatus(userId, status);
        }

        public async System.Threading.Tasks.Task<int> ChangeUserStatusAsync(int userId, SolidCP.EnterpriseServer.UserStatus status)
        {
            return await base.Client.ChangeUserStatusAsync(userId, status);
        }

        public SolidCP.EnterpriseServer.UserSettings GetUserSettings(int userId, string settingsName)
        {
            return base.Client.GetUserSettings(userId, settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.UserSettings> GetUserSettingsAsync(int userId, string settingsName)
        {
            return await base.Client.GetUserSettingsAsync(userId, settingsName);
        }

        public int UpdateUserSettings(SolidCP.EnterpriseServer.UserSettings settings)
        {
            return base.Client.UpdateUserSettings(settings);
        }

        public async System.Threading.Tasks.Task<int> UpdateUserSettingsAsync(SolidCP.EnterpriseServer.UserSettings settings)
        {
            return await base.Client.UpdateUserSettingsAsync(settings);
        }

        public System.Data.DataSet GetUserThemeSettings(int userId)
        {
            return base.Client.GetUserThemeSettings(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetUserThemeSettingsAsync(int userId)
        {
            return await base.Client.GetUserThemeSettingsAsync(userId);
        }

        public void UpdateUserThemeSetting(int userId, string PropertyName, string PropertyValue)
        {
            base.Client.UpdateUserThemeSetting(userId, PropertyName, PropertyValue);
        }

        public async System.Threading.Tasks.Task UpdateUserThemeSettingAsync(int userId, string PropertyName, string PropertyValue)
        {
            await base.Client.UpdateUserThemeSettingAsync(userId, PropertyName, PropertyValue);
        }

        public void DeleteUserThemeSetting(int userId, string PropertyName)
        {
            base.Client.DeleteUserThemeSetting(userId, PropertyName);
        }

        public async System.Threading.Tasks.Task DeleteUserThemeSettingAsync(int userId, string PropertyName)
        {
            await base.Client.DeleteUserThemeSettingAsync(userId, PropertyName);
        }
    }
}
#endif
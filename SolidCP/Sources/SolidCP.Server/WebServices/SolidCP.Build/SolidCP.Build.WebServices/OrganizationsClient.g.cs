#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IOrganizations", Namespace = "http://tempuri.org/")]
    public interface IOrganizations
    {
        [OperationContract(Action = "http://tempuri.org/IOrganizations/OrganizationExists", ReplyAction = "http://tempuri.org/IOrganizations/OrganizationExistsResponse")]
        bool OrganizationExists(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/OrganizationExists", ReplyAction = "http://tempuri.org/IOrganizations/OrganizationExistsResponse")]
        System.Threading.Tasks.Task<bool> OrganizationExistsAsync(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateOrganization", ReplyAction = "http://tempuri.org/IOrganizations/CreateOrganizationResponse")]
        SolidCP.Providers.HostedSolution.Organization CreateOrganization(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateOrganization", ReplyAction = "http://tempuri.org/IOrganizations/CreateOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationAsync(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteOrganization", ReplyAction = "http://tempuri.org/IOrganizations/DeleteOrganizationResponse")]
        void DeleteOrganization(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteOrganization", ReplyAction = "http://tempuri.org/IOrganizations/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task DeleteOrganizationAsync(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateUser", ReplyAction = "http://tempuri.org/IOrganizations/CreateUserResponse")]
        int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateUser", ReplyAction = "http://tempuri.org/IOrganizations/CreateUserResponse")]
        System.Threading.Tasks.Task<int> CreateUserAsync(string organizationId, string loginName, string displayName, string upn, string password, bool enabled);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DisableUser", ReplyAction = "http://tempuri.org/IOrganizations/DisableUserResponse")]
        void DisableUser(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DisableUser", ReplyAction = "http://tempuri.org/IOrganizations/DisableUserResponse")]
        System.Threading.Tasks.Task DisableUserAsync(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteUser", ReplyAction = "http://tempuri.org/IOrganizations/DeleteUserResponse")]
        void DeleteUser(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteUser", ReplyAction = "http://tempuri.org/IOrganizations/DeleteUserResponse")]
        System.Threading.Tasks.Task DeleteUserAsync(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetUserGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/GetUserGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettings(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetUserGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/GetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsAsync(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/CreateSecurityGroupResponse")]
        int CreateSecurityGroup(string organizationId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/CreateSecurityGroupResponse")]
        System.Threading.Tasks.Task<int> CreateSecurityGroupAsync(string organizationId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/GetSecurityGroupGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/GetSecurityGroupGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup> GetSecurityGroupGeneralSettingsAsync(string groupName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetSecurityGroupsNotes", ReplyAction = "http://tempuri.org/IOrganizations/GetSecurityGroupsNotesResponse")]
        string[] GetSecurityGroupsNotes(string[] groupNames, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetSecurityGroupsNotes", ReplyAction = "http://tempuri.org/IOrganizations/GetSecurityGroupsNotesResponse")]
        System.Threading.Tasks.Task<string[]> GetSecurityGroupsNotesAsync(string[] groupNames, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/DeleteSecurityGroupResponse")]
        void DeleteSecurityGroup(string groupName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/DeleteSecurityGroupResponse")]
        System.Threading.Tasks.Task DeleteSecurityGroupAsync(string groupName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/SetSecurityGroupGeneralSettingsResponse")]
        void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/SetSecurityGroupGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetSecurityGroupGeneralSettingsAsync(string organizationId, string groupName, string[] memberAccounts, string notes);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/AddObjectToSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/AddObjectToSecurityGroupResponse")]
        void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/AddObjectToSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/AddObjectToSecurityGroupResponse")]
        System.Threading.Tasks.Task AddObjectToSecurityGroupAsync(string organizationId, string accountName, string groupName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteObjectFromSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/DeleteObjectFromSecurityGroupResponse")]
        void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteObjectFromSecurityGroup", ReplyAction = "http://tempuri.org/IOrganizations/DeleteObjectFromSecurityGroupResponse")]
        System.Threading.Tasks.Task DeleteObjectFromSecurityGroupAsync(string organizationId, string accountName, string groupName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetUserGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/SetUserGeneralSettingsResponse")]
        void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetUserGeneralSettings", ReplyAction = "http://tempuri.org/IOrganizations/SetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetUserGeneralSettingsAsync(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetUserPassword", ReplyAction = "http://tempuri.org/IOrganizations/SetUserPasswordResponse")]
        void SetUserPassword(string organizationId, string accountName, string password);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetUserPassword", ReplyAction = "http://tempuri.org/IOrganizations/SetUserPasswordResponse")]
        System.Threading.Tasks.Task SetUserPasswordAsync(string organizationId, string accountName, string password);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetUserPrincipalName", ReplyAction = "http://tempuri.org/IOrganizations/SetUserPrincipalNameResponse")]
        void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetUserPrincipalName", ReplyAction = "http://tempuri.org/IOrganizations/SetUserPrincipalNameResponse")]
        System.Threading.Tasks.Task SetUserPrincipalNameAsync(string organizationId, string accountName, string userPrincipalName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteOrganizationDomain", ReplyAction = "http://tempuri.org/IOrganizations/DeleteOrganizationDomainResponse")]
        void DeleteOrganizationDomain(string organizationDistinguishedName, string domain);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteOrganizationDomain", ReplyAction = "http://tempuri.org/IOrganizations/DeleteOrganizationDomainResponse")]
        System.Threading.Tasks.Task DeleteOrganizationDomainAsync(string organizationDistinguishedName, string domain);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateOrganizationDomain", ReplyAction = "http://tempuri.org/IOrganizations/CreateOrganizationDomainResponse")]
        void CreateOrganizationDomain(string organizationDistinguishedName, string domain);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateOrganizationDomain", ReplyAction = "http://tempuri.org/IOrganizations/CreateOrganizationDomainResponse")]
        System.Threading.Tasks.Task CreateOrganizationDomainAsync(string organizationDistinguishedName, string domain);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetPasswordPolicy", ReplyAction = "http://tempuri.org/IOrganizations/GetPasswordPolicyResponse")]
        SolidCP.Providers.ResultObjects.PasswordPolicyResult GetPasswordPolicy();
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetPasswordPolicy", ReplyAction = "http://tempuri.org/IOrganizations/GetPasswordPolicyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.PasswordPolicyResult> GetPasswordPolicyAsync();
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetSamAccountNameByUserPrincipalName", ReplyAction = "http://tempuri.org/IOrganizations/GetSamAccountNameByUserPrincipalNameResponse")]
        string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetSamAccountNameByUserPrincipalName", ReplyAction = "http://tempuri.org/IOrganizations/GetSamAccountNameByUserPrincipalNameResponse")]
        System.Threading.Tasks.Task<string> GetSamAccountNameByUserPrincipalNameAsync(string organizationId, string userPrincipalName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DoesSamAccountNameExist", ReplyAction = "http://tempuri.org/IOrganizations/DoesSamAccountNameExistResponse")]
        bool DoesSamAccountNameExist(string accountName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DoesSamAccountNameExist", ReplyAction = "http://tempuri.org/IOrganizations/DoesSamAccountNameExistResponse")]
        System.Threading.Tasks.Task<bool> DoesSamAccountNameExistAsync(string accountName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetDriveMaps", ReplyAction = "http://tempuri.org/IOrganizations/GetDriveMapsResponse")]
        SolidCP.Providers.OS.MappedDrive[] GetDriveMaps(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetDriveMaps", ReplyAction = "http://tempuri.org/IOrganizations/GetDriveMapsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.MappedDrive[]> GetDriveMapsAsync(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateMappedDrive", ReplyAction = "http://tempuri.org/IOrganizations/CreateMappedDriveResponse")]
        int CreateMappedDrive(string organizationId, string drive, string labelAs, string path);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CreateMappedDrive", ReplyAction = "http://tempuri.org/IOrganizations/CreateMappedDriveResponse")]
        System.Threading.Tasks.Task<int> CreateMappedDriveAsync(string organizationId, string drive, string labelAs, string path);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteMappedDrive", ReplyAction = "http://tempuri.org/IOrganizations/DeleteMappedDriveResponse")]
        void DeleteMappedDrive(string organizationId, string drive);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteMappedDrive", ReplyAction = "http://tempuri.org/IOrganizations/DeleteMappedDriveResponse")]
        System.Threading.Tasks.Task DeleteMappedDriveAsync(string organizationId, string drive);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteMappedDriveByPath", ReplyAction = "http://tempuri.org/IOrganizations/DeleteMappedDriveByPathResponse")]
        void DeleteMappedDriveByPath(string organizationId, string path);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteMappedDriveByPath", ReplyAction = "http://tempuri.org/IOrganizations/DeleteMappedDriveByPathResponse")]
        System.Threading.Tasks.Task DeleteMappedDriveByPathAsync(string organizationId, string path);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteMappedDrivesGPO", ReplyAction = "http://tempuri.org/IOrganizations/DeleteMappedDrivesGPOResponse")]
        void DeleteMappedDrivesGPO(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/DeleteMappedDrivesGPO", ReplyAction = "http://tempuri.org/IOrganizations/DeleteMappedDrivesGPOResponse")]
        System.Threading.Tasks.Task DeleteMappedDrivesGPOAsync(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetDriveMapsTargetingFilter", ReplyAction = "http://tempuri.org/IOrganizations/SetDriveMapsTargetingFilterResponse")]
        void SetDriveMapsTargetingFilter(string organizationId, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts, string folderName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetDriveMapsTargetingFilter", ReplyAction = "http://tempuri.org/IOrganizations/SetDriveMapsTargetingFilterResponse")]
        System.Threading.Tasks.Task SetDriveMapsTargetingFilterAsync(string organizationId, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts, string folderName);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/ChangeDriveMapFolderPath", ReplyAction = "http://tempuri.org/IOrganizations/ChangeDriveMapFolderPathResponse")]
        void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/ChangeDriveMapFolderPath", ReplyAction = "http://tempuri.org/IOrganizations/ChangeDriveMapFolderPathResponse")]
        System.Threading.Tasks.Task ChangeDriveMapFolderPathAsync(string organizationId, string oldFolder, string newFolder);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetOrganizationUsersWithExpiredPassword", ReplyAction = "http://tempuri.org/IOrganizations/GetOrganizationUsersWithExpiredPasswordResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetOrganizationUsersWithExpiredPassword", ReplyAction = "http://tempuri.org/IOrganizations/GetOrganizationUsersWithExpiredPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetOrganizationUsersWithExpiredPasswordAsync(string organizationId, int daysBeforeExpiration);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/ApplyPasswordSettings", ReplyAction = "http://tempuri.org/IOrganizations/ApplyPasswordSettingsResponse")]
        void ApplyPasswordSettings(string organizationId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings passwordSettings);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/ApplyPasswordSettings", ReplyAction = "http://tempuri.org/IOrganizations/ApplyPasswordSettingsResponse")]
        System.Threading.Tasks.Task ApplyPasswordSettingsAsync(string organizationId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings passwordSettings);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CheckPhoneNumberIsInUse", ReplyAction = "http://tempuri.org/IOrganizations/CheckPhoneNumberIsInUseResponse")]
        bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/CheckPhoneNumberIsInUse", ReplyAction = "http://tempuri.org/IOrganizations/CheckPhoneNumberIsInUseResponse")]
        System.Threading.Tasks.Task<bool> CheckPhoneNumberIsInUseAsync(string phoneNumber, string userSamAccountName = null);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetOrganizationUserWithExtraData", ReplyAction = "http://tempuri.org/IOrganizations/GetOrganizationUserWithExtraDataResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetOrganizationUserWithExtraData", ReplyAction = "http://tempuri.org/IOrganizations/GetOrganizationUserWithExtraDataResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetOrganizationUserWithExtraDataAsync(string loginName, string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetOUAclPermissions", ReplyAction = "http://tempuri.org/IOrganizations/SetOUAclPermissionsResponse")]
        void SetOUAclPermissions(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/SetOUAclPermissions", ReplyAction = "http://tempuri.org/IOrganizations/SetOUAclPermissionsResponse")]
        System.Threading.Tasks.Task SetOUAclPermissionsAsync(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetUserGroups", ReplyAction = "http://tempuri.org/IOrganizations/GetUserGroupsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] GetUserGroups(string userName, int organizationId);
        [OperationContract(Action = "http://tempuri.org/IOrganizations/GetUserGroups", ReplyAction = "http://tempuri.org/IOrganizations/GetUserGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetUserGroupsAsync(string userName, int organizationId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OrganizationsAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IOrganizations
    {
        public bool OrganizationExists(string organizationId)
        {
            return Invoke<bool>("SolidCP.Server.Organizations", "OrganizationExists", organizationId);
        }

        public async System.Threading.Tasks.Task<bool> OrganizationExistsAsync(string organizationId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.Organizations", "OrganizationExists", organizationId);
        }

        public SolidCP.Providers.HostedSolution.Organization CreateOrganization(string organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.Organizations", "CreateOrganization", organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationAsync(string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.Organizations", "CreateOrganization", organizationId);
        }

        public void DeleteOrganization(string organizationId)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteOrganization", organizationId);
        }

        public async System.Threading.Tasks.Task DeleteOrganizationAsync(string organizationId)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteOrganization", organizationId);
        }

        public int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            return Invoke<int>("SolidCP.Server.Organizations", "CreateUser", organizationId, loginName, displayName, upn, password, enabled);
        }

        public async System.Threading.Tasks.Task<int> CreateUserAsync(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            return await InvokeAsync<int>("SolidCP.Server.Organizations", "CreateUser", organizationId, loginName, displayName, upn, password, enabled);
        }

        public void DisableUser(string loginName, string organizationId)
        {
            Invoke("SolidCP.Server.Organizations", "DisableUser", loginName, organizationId);
        }

        public async System.Threading.Tasks.Task DisableUserAsync(string loginName, string organizationId)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DisableUser", loginName, organizationId);
        }

        public void DeleteUser(string loginName, string organizationId)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteUser", loginName, organizationId);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string loginName, string organizationId)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteUser", loginName, organizationId);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettings(string loginName, string organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.Server.Organizations", "GetUserGeneralSettings", loginName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsAsync(string loginName, string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.Server.Organizations", "GetUserGeneralSettings", loginName, organizationId);
        }

        public int CreateSecurityGroup(string organizationId, string groupName)
        {
            return Invoke<int>("SolidCP.Server.Organizations", "CreateSecurityGroup", organizationId, groupName);
        }

        public async System.Threading.Tasks.Task<int> CreateSecurityGroupAsync(string organizationId, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.Server.Organizations", "CreateSecurityGroup", organizationId, groupName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup>("SolidCP.Server.Organizations", "GetSecurityGroupGeneralSettings", groupName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup> GetSecurityGroupGeneralSettingsAsync(string groupName, string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup>("SolidCP.Server.Organizations", "GetSecurityGroupGeneralSettings", groupName, organizationId);
        }

        public string[] GetSecurityGroupsNotes(string[] groupNames, string organizationId)
        {
            return Invoke<string[]>("SolidCP.Server.Organizations", "GetSecurityGroupsNotes", groupNames, organizationId);
        }

        public async System.Threading.Tasks.Task<string[]> GetSecurityGroupsNotesAsync(string[] groupNames, string organizationId)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.Organizations", "GetSecurityGroupsNotes", groupNames, organizationId);
        }

        public void DeleteSecurityGroup(string groupName, string organizationId)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteSecurityGroup", groupName, organizationId);
        }

        public async System.Threading.Tasks.Task DeleteSecurityGroupAsync(string groupName, string organizationId)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteSecurityGroup", groupName, organizationId);
        }

        public void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes)
        {
            Invoke("SolidCP.Server.Organizations", "SetSecurityGroupGeneralSettings", organizationId, groupName, memberAccounts, notes);
        }

        public async System.Threading.Tasks.Task SetSecurityGroupGeneralSettingsAsync(string organizationId, string groupName, string[] memberAccounts, string notes)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "SetSecurityGroupGeneralSettings", organizationId, groupName, memberAccounts, notes);
        }

        public void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName)
        {
            Invoke("SolidCP.Server.Organizations", "AddObjectToSecurityGroup", organizationId, accountName, groupName);
        }

        public async System.Threading.Tasks.Task AddObjectToSecurityGroupAsync(string organizationId, string accountName, string groupName)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "AddObjectToSecurityGroup", organizationId, accountName, groupName);
        }

        public void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteObjectFromSecurityGroup", organizationId, accountName, groupName);
        }

        public async System.Threading.Tasks.Task DeleteObjectFromSecurityGroupAsync(string organizationId, string accountName, string groupName)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteObjectFromSecurityGroup", organizationId, accountName, groupName);
        }

        public void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword)
        {
            Invoke("SolidCP.Server.Organizations", "SetUserGeneralSettings", organizationId, accountName, displayName, password, hideFromAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, userMustChangePassword);
        }

        public async System.Threading.Tasks.Task SetUserGeneralSettingsAsync(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "SetUserGeneralSettings", organizationId, accountName, displayName, password, hideFromAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, userMustChangePassword);
        }

        public void SetUserPassword(string organizationId, string accountName, string password)
        {
            Invoke("SolidCP.Server.Organizations", "SetUserPassword", organizationId, accountName, password);
        }

        public async System.Threading.Tasks.Task SetUserPasswordAsync(string organizationId, string accountName, string password)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "SetUserPassword", organizationId, accountName, password);
        }

        public void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName)
        {
            Invoke("SolidCP.Server.Organizations", "SetUserPrincipalName", organizationId, accountName, userPrincipalName);
        }

        public async System.Threading.Tasks.Task SetUserPrincipalNameAsync(string organizationId, string accountName, string userPrincipalName)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "SetUserPrincipalName", organizationId, accountName, userPrincipalName);
        }

        public void DeleteOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteOrganizationDomain", organizationDistinguishedName, domain);
        }

        public async System.Threading.Tasks.Task DeleteOrganizationDomainAsync(string organizationDistinguishedName, string domain)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteOrganizationDomain", organizationDistinguishedName, domain);
        }

        public void CreateOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            Invoke("SolidCP.Server.Organizations", "CreateOrganizationDomain", organizationDistinguishedName, domain);
        }

        public async System.Threading.Tasks.Task CreateOrganizationDomainAsync(string organizationDistinguishedName, string domain)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "CreateOrganizationDomain", organizationDistinguishedName, domain);
        }

        public SolidCP.Providers.ResultObjects.PasswordPolicyResult GetPasswordPolicy()
        {
            return Invoke<SolidCP.Providers.ResultObjects.PasswordPolicyResult>("SolidCP.Server.Organizations", "GetPasswordPolicy");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.PasswordPolicyResult> GetPasswordPolicyAsync()
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.PasswordPolicyResult>("SolidCP.Server.Organizations", "GetPasswordPolicy");
        }

        public string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName)
        {
            return Invoke<string>("SolidCP.Server.Organizations", "GetSamAccountNameByUserPrincipalName", organizationId, userPrincipalName);
        }

        public async System.Threading.Tasks.Task<string> GetSamAccountNameByUserPrincipalNameAsync(string organizationId, string userPrincipalName)
        {
            return await InvokeAsync<string>("SolidCP.Server.Organizations", "GetSamAccountNameByUserPrincipalName", organizationId, userPrincipalName);
        }

        public bool DoesSamAccountNameExist(string accountName)
        {
            return Invoke<bool>("SolidCP.Server.Organizations", "DoesSamAccountNameExist", accountName);
        }

        public async System.Threading.Tasks.Task<bool> DoesSamAccountNameExistAsync(string accountName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.Organizations", "DoesSamAccountNameExist", accountName);
        }

        public SolidCP.Providers.OS.MappedDrive[] GetDriveMaps(string organizationId)
        {
            return Invoke<SolidCP.Providers.OS.MappedDrive[]>("SolidCP.Server.Organizations", "GetDriveMaps", organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.MappedDrive[]> GetDriveMapsAsync(string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.MappedDrive[]>("SolidCP.Server.Organizations", "GetDriveMaps", organizationId);
        }

        public int CreateMappedDrive(string organizationId, string drive, string labelAs, string path)
        {
            return Invoke<int>("SolidCP.Server.Organizations", "CreateMappedDrive", organizationId, drive, labelAs, path);
        }

        public async System.Threading.Tasks.Task<int> CreateMappedDriveAsync(string organizationId, string drive, string labelAs, string path)
        {
            return await InvokeAsync<int>("SolidCP.Server.Organizations", "CreateMappedDrive", organizationId, drive, labelAs, path);
        }

        public void DeleteMappedDrive(string organizationId, string drive)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteMappedDrive", organizationId, drive);
        }

        public async System.Threading.Tasks.Task DeleteMappedDriveAsync(string organizationId, string drive)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteMappedDrive", organizationId, drive);
        }

        public void DeleteMappedDriveByPath(string organizationId, string path)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteMappedDriveByPath", organizationId, path);
        }

        public async System.Threading.Tasks.Task DeleteMappedDriveByPathAsync(string organizationId, string path)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteMappedDriveByPath", organizationId, path);
        }

        public void DeleteMappedDrivesGPO(string organizationId)
        {
            Invoke("SolidCP.Server.Organizations", "DeleteMappedDrivesGPO", organizationId);
        }

        public async System.Threading.Tasks.Task DeleteMappedDrivesGPOAsync(string organizationId)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "DeleteMappedDrivesGPO", organizationId);
        }

        public void SetDriveMapsTargetingFilter(string organizationId, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts, string folderName)
        {
            Invoke("SolidCP.Server.Organizations", "SetDriveMapsTargetingFilter", organizationId, accounts, folderName);
        }

        public async System.Threading.Tasks.Task SetDriveMapsTargetingFilterAsync(string organizationId, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts, string folderName)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "SetDriveMapsTargetingFilter", organizationId, accounts, folderName);
        }

        public void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder)
        {
            Invoke("SolidCP.Server.Organizations", "ChangeDriveMapFolderPath", organizationId, oldFolder, newFolder);
        }

        public async System.Threading.Tasks.Task ChangeDriveMapFolderPathAsync(string organizationId, string oldFolder, string newFolder)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "ChangeDriveMapFolderPath", organizationId, oldFolder, newFolder);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.Server.Organizations", "GetOrganizationUsersWithExpiredPassword", organizationId, daysBeforeExpiration);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetOrganizationUsersWithExpiredPasswordAsync(string organizationId, int daysBeforeExpiration)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.Server.Organizations", "GetOrganizationUsersWithExpiredPassword", organizationId, daysBeforeExpiration);
        }

        public void ApplyPasswordSettings(string organizationId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings passwordSettings)
        {
            Invoke("SolidCP.Server.Organizations", "ApplyPasswordSettings", organizationId, passwordSettings);
        }

        public async System.Threading.Tasks.Task ApplyPasswordSettingsAsync(string organizationId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings passwordSettings)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "ApplyPasswordSettings", organizationId, passwordSettings);
        }

        public bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null)
        {
            return Invoke<bool>("SolidCP.Server.Organizations", "CheckPhoneNumberIsInUse", phoneNumber, userSamAccountName);
        }

        public async System.Threading.Tasks.Task<bool> CheckPhoneNumberIsInUseAsync(string phoneNumber, string userSamAccountName = null)
        {
            return await InvokeAsync<bool>("SolidCP.Server.Organizations", "CheckPhoneNumberIsInUse", phoneNumber, userSamAccountName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.Server.Organizations", "GetOrganizationUserWithExtraData", loginName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetOrganizationUserWithExtraDataAsync(string loginName, string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.Server.Organizations", "GetOrganizationUserWithExtraData", loginName, organizationId);
        }

        public void SetOUAclPermissions(string organizationId)
        {
            Invoke("SolidCP.Server.Organizations", "SetOUAclPermissions", organizationId);
        }

        public async System.Threading.Tasks.Task SetOUAclPermissionsAsync(string organizationId)
        {
            await InvokeAsync("SolidCP.Server.Organizations", "SetOUAclPermissions", organizationId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetUserGroups(string userName, int organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.Server.Organizations", "GetUserGroups", userName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetUserGroupsAsync(string userName, int organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.Server.Organizations", "GetUserGroups", userName, organizationId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class Organizations : SolidCP.Web.Client.ClientBase<IOrganizations, OrganizationsAssemblyClient>, IOrganizations
    {
        public bool OrganizationExists(string organizationId)
        {
            return base.Client.OrganizationExists(organizationId);
        }

        public async System.Threading.Tasks.Task<bool> OrganizationExistsAsync(string organizationId)
        {
            return await base.Client.OrganizationExistsAsync(organizationId);
        }

        public SolidCP.Providers.HostedSolution.Organization CreateOrganization(string organizationId)
        {
            return base.Client.CreateOrganization(organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationAsync(string organizationId)
        {
            return await base.Client.CreateOrganizationAsync(organizationId);
        }

        public void DeleteOrganization(string organizationId)
        {
            base.Client.DeleteOrganization(organizationId);
        }

        public async System.Threading.Tasks.Task DeleteOrganizationAsync(string organizationId)
        {
            await base.Client.DeleteOrganizationAsync(organizationId);
        }

        public int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            return base.Client.CreateUser(organizationId, loginName, displayName, upn, password, enabled);
        }

        public async System.Threading.Tasks.Task<int> CreateUserAsync(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            return await base.Client.CreateUserAsync(organizationId, loginName, displayName, upn, password, enabled);
        }

        public void DisableUser(string loginName, string organizationId)
        {
            base.Client.DisableUser(loginName, organizationId);
        }

        public async System.Threading.Tasks.Task DisableUserAsync(string loginName, string organizationId)
        {
            await base.Client.DisableUserAsync(loginName, organizationId);
        }

        public void DeleteUser(string loginName, string organizationId)
        {
            base.Client.DeleteUser(loginName, organizationId);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string loginName, string organizationId)
        {
            await base.Client.DeleteUserAsync(loginName, organizationId);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettings(string loginName, string organizationId)
        {
            return base.Client.GetUserGeneralSettings(loginName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsAsync(string loginName, string organizationId)
        {
            return await base.Client.GetUserGeneralSettingsAsync(loginName, organizationId);
        }

        public int CreateSecurityGroup(string organizationId, string groupName)
        {
            return base.Client.CreateSecurityGroup(organizationId, groupName);
        }

        public async System.Threading.Tasks.Task<int> CreateSecurityGroupAsync(string organizationId, string groupName)
        {
            return await base.Client.CreateSecurityGroupAsync(organizationId, groupName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId)
        {
            return base.Client.GetSecurityGroupGeneralSettings(groupName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup> GetSecurityGroupGeneralSettingsAsync(string groupName, string organizationId)
        {
            return await base.Client.GetSecurityGroupGeneralSettingsAsync(groupName, organizationId);
        }

        public string[] GetSecurityGroupsNotes(string[] groupNames, string organizationId)
        {
            return base.Client.GetSecurityGroupsNotes(groupNames, organizationId);
        }

        public async System.Threading.Tasks.Task<string[]> GetSecurityGroupsNotesAsync(string[] groupNames, string organizationId)
        {
            return await base.Client.GetSecurityGroupsNotesAsync(groupNames, organizationId);
        }

        public void DeleteSecurityGroup(string groupName, string organizationId)
        {
            base.Client.DeleteSecurityGroup(groupName, organizationId);
        }

        public async System.Threading.Tasks.Task DeleteSecurityGroupAsync(string groupName, string organizationId)
        {
            await base.Client.DeleteSecurityGroupAsync(groupName, organizationId);
        }

        public void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes)
        {
            base.Client.SetSecurityGroupGeneralSettings(organizationId, groupName, memberAccounts, notes);
        }

        public async System.Threading.Tasks.Task SetSecurityGroupGeneralSettingsAsync(string organizationId, string groupName, string[] memberAccounts, string notes)
        {
            await base.Client.SetSecurityGroupGeneralSettingsAsync(organizationId, groupName, memberAccounts, notes);
        }

        public void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName)
        {
            base.Client.AddObjectToSecurityGroup(organizationId, accountName, groupName);
        }

        public async System.Threading.Tasks.Task AddObjectToSecurityGroupAsync(string organizationId, string accountName, string groupName)
        {
            await base.Client.AddObjectToSecurityGroupAsync(organizationId, accountName, groupName);
        }

        public void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName)
        {
            base.Client.DeleteObjectFromSecurityGroup(organizationId, accountName, groupName);
        }

        public async System.Threading.Tasks.Task DeleteObjectFromSecurityGroupAsync(string organizationId, string accountName, string groupName)
        {
            await base.Client.DeleteObjectFromSecurityGroupAsync(organizationId, accountName, groupName);
        }

        public void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword)
        {
            base.Client.SetUserGeneralSettings(organizationId, accountName, displayName, password, hideFromAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, userMustChangePassword);
        }

        public async System.Threading.Tasks.Task SetUserGeneralSettingsAsync(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword)
        {
            await base.Client.SetUserGeneralSettingsAsync(organizationId, accountName, displayName, password, hideFromAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, userMustChangePassword);
        }

        public void SetUserPassword(string organizationId, string accountName, string password)
        {
            base.Client.SetUserPassword(organizationId, accountName, password);
        }

        public async System.Threading.Tasks.Task SetUserPasswordAsync(string organizationId, string accountName, string password)
        {
            await base.Client.SetUserPasswordAsync(organizationId, accountName, password);
        }

        public void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName)
        {
            base.Client.SetUserPrincipalName(organizationId, accountName, userPrincipalName);
        }

        public async System.Threading.Tasks.Task SetUserPrincipalNameAsync(string organizationId, string accountName, string userPrincipalName)
        {
            await base.Client.SetUserPrincipalNameAsync(organizationId, accountName, userPrincipalName);
        }

        public void DeleteOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            base.Client.DeleteOrganizationDomain(organizationDistinguishedName, domain);
        }

        public async System.Threading.Tasks.Task DeleteOrganizationDomainAsync(string organizationDistinguishedName, string domain)
        {
            await base.Client.DeleteOrganizationDomainAsync(organizationDistinguishedName, domain);
        }

        public void CreateOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            base.Client.CreateOrganizationDomain(organizationDistinguishedName, domain);
        }

        public async System.Threading.Tasks.Task CreateOrganizationDomainAsync(string organizationDistinguishedName, string domain)
        {
            await base.Client.CreateOrganizationDomainAsync(organizationDistinguishedName, domain);
        }

        public SolidCP.Providers.ResultObjects.PasswordPolicyResult GetPasswordPolicy()
        {
            return base.Client.GetPasswordPolicy();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.PasswordPolicyResult> GetPasswordPolicyAsync()
        {
            return await base.Client.GetPasswordPolicyAsync();
        }

        public string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName)
        {
            return base.Client.GetSamAccountNameByUserPrincipalName(organizationId, userPrincipalName);
        }

        public async System.Threading.Tasks.Task<string> GetSamAccountNameByUserPrincipalNameAsync(string organizationId, string userPrincipalName)
        {
            return await base.Client.GetSamAccountNameByUserPrincipalNameAsync(organizationId, userPrincipalName);
        }

        public bool DoesSamAccountNameExist(string accountName)
        {
            return base.Client.DoesSamAccountNameExist(accountName);
        }

        public async System.Threading.Tasks.Task<bool> DoesSamAccountNameExistAsync(string accountName)
        {
            return await base.Client.DoesSamAccountNameExistAsync(accountName);
        }

        public SolidCP.Providers.OS.MappedDrive[] GetDriveMaps(string organizationId)
        {
            return base.Client.GetDriveMaps(organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.MappedDrive[]> GetDriveMapsAsync(string organizationId)
        {
            return await base.Client.GetDriveMapsAsync(organizationId);
        }

        public int CreateMappedDrive(string organizationId, string drive, string labelAs, string path)
        {
            return base.Client.CreateMappedDrive(organizationId, drive, labelAs, path);
        }

        public async System.Threading.Tasks.Task<int> CreateMappedDriveAsync(string organizationId, string drive, string labelAs, string path)
        {
            return await base.Client.CreateMappedDriveAsync(organizationId, drive, labelAs, path);
        }

        public void DeleteMappedDrive(string organizationId, string drive)
        {
            base.Client.DeleteMappedDrive(organizationId, drive);
        }

        public async System.Threading.Tasks.Task DeleteMappedDriveAsync(string organizationId, string drive)
        {
            await base.Client.DeleteMappedDriveAsync(organizationId, drive);
        }

        public void DeleteMappedDriveByPath(string organizationId, string path)
        {
            base.Client.DeleteMappedDriveByPath(organizationId, path);
        }

        public async System.Threading.Tasks.Task DeleteMappedDriveByPathAsync(string organizationId, string path)
        {
            await base.Client.DeleteMappedDriveByPathAsync(organizationId, path);
        }

        public void DeleteMappedDrivesGPO(string organizationId)
        {
            base.Client.DeleteMappedDrivesGPO(organizationId);
        }

        public async System.Threading.Tasks.Task DeleteMappedDrivesGPOAsync(string organizationId)
        {
            await base.Client.DeleteMappedDrivesGPOAsync(organizationId);
        }

        public void SetDriveMapsTargetingFilter(string organizationId, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts, string folderName)
        {
            base.Client.SetDriveMapsTargetingFilter(organizationId, accounts, folderName);
        }

        public async System.Threading.Tasks.Task SetDriveMapsTargetingFilterAsync(string organizationId, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts, string folderName)
        {
            await base.Client.SetDriveMapsTargetingFilterAsync(organizationId, accounts, folderName);
        }

        public void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder)
        {
            base.Client.ChangeDriveMapFolderPath(organizationId, oldFolder, newFolder);
        }

        public async System.Threading.Tasks.Task ChangeDriveMapFolderPathAsync(string organizationId, string oldFolder, string newFolder)
        {
            await base.Client.ChangeDriveMapFolderPathAsync(organizationId, oldFolder, newFolder);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration)
        {
            return base.Client.GetOrganizationUsersWithExpiredPassword(organizationId, daysBeforeExpiration);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetOrganizationUsersWithExpiredPasswordAsync(string organizationId, int daysBeforeExpiration)
        {
            return await base.Client.GetOrganizationUsersWithExpiredPasswordAsync(organizationId, daysBeforeExpiration);
        }

        public void ApplyPasswordSettings(string organizationId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings passwordSettings)
        {
            base.Client.ApplyPasswordSettings(organizationId, passwordSettings);
        }

        public async System.Threading.Tasks.Task ApplyPasswordSettingsAsync(string organizationId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings passwordSettings)
        {
            await base.Client.ApplyPasswordSettingsAsync(organizationId, passwordSettings);
        }

        public bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null)
        {
            return base.Client.CheckPhoneNumberIsInUse(phoneNumber, userSamAccountName);
        }

        public async System.Threading.Tasks.Task<bool> CheckPhoneNumberIsInUseAsync(string phoneNumber, string userSamAccountName = null)
        {
            return await base.Client.CheckPhoneNumberIsInUseAsync(phoneNumber, userSamAccountName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId)
        {
            return base.Client.GetOrganizationUserWithExtraData(loginName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetOrganizationUserWithExtraDataAsync(string loginName, string organizationId)
        {
            return await base.Client.GetOrganizationUserWithExtraDataAsync(loginName, organizationId);
        }

        public void SetOUAclPermissions(string organizationId)
        {
            base.Client.SetOUAclPermissions(organizationId);
        }

        public async System.Threading.Tasks.Task SetOUAclPermissionsAsync(string organizationId)
        {
            await base.Client.SetOUAclPermissionsAsync(organizationId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetUserGroups(string userName, int organizationId)
        {
            return base.Client.GetUserGroups(userName, organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetUserGroupsAsync(string userName, int organizationId)
        {
            return await base.Client.GetUserGroupsAsync(userName, organizationId);
        }
    }
}
#endif
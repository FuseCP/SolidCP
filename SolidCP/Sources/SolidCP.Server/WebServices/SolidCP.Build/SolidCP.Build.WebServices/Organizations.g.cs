#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IOrganizations
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool OrganizationExists(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Organization CreateOrganization(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteOrganization(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DisableUser(string loginName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteUser(string loginName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        OrganizationUser GetUserGeneralSettings(string loginName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int CreateSecurityGroup(string organizationId, string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetSecurityGroupsNotes(string[] groupNames, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteSecurityGroup(string groupName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetUserPassword(string organizationId, string accountName, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteOrganizationDomain(string organizationDistinguishedName, string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateOrganizationDomain(string organizationDistinguishedName, string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        PasswordPolicyResult GetPasswordPolicy();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DoesSamAccountNameExist(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MappedDrive[] GetDriveMaps(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int CreateMappedDrive(string organizationId, string drive, string labelAs, string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteMappedDrive(string organizationId, string drive);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteMappedDriveByPath(string organizationId, string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteMappedDrivesGPO(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetDriveMapsTargetingFilter(string organizationId, ExchangeAccount[] accounts, string folderName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<OrganizationUser> GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ApplyPasswordSettings(string organizationId, OrganizationPasswordSettings passwordSettings);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetOUAclPermissions(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeAccount[] GetUserGroups(string userName, int organizationId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class Organizations : SolidCP.Server.Organizations, IOrganizations
    {
    }
}
#endif
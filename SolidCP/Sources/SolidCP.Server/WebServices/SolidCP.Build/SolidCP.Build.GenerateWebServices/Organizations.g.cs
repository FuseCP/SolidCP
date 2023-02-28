#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ServiceContract]
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
    public class OrganizationsService : Organizations, IOrganizations
    {
        public new bool OrganizationExists(string organizationId)
        {
            return base.OrganizationExists(organizationId);
        }

        public new Organization CreateOrganization(string organizationId)
        {
            return base.CreateOrganization(organizationId);
        }

        public new void DeleteOrganization(string organizationId)
        {
            base.DeleteOrganization(organizationId);
        }

        public new int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            return base.CreateUser(organizationId, loginName, displayName, upn, password, enabled);
        }

        public new void DisableUser(string loginName, string organizationId)
        {
            base.DisableUser(loginName, organizationId);
        }

        public new void DeleteUser(string loginName, string organizationId)
        {
            base.DeleteUser(loginName, organizationId);
        }

        public new OrganizationUser GetUserGeneralSettings(string loginName, string organizationId)
        {
            return base.GetUserGeneralSettings(loginName, organizationId);
        }

        public new int CreateSecurityGroup(string organizationId, string groupName)
        {
            return base.CreateSecurityGroup(organizationId, groupName);
        }

        public new OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId)
        {
            return base.GetSecurityGroupGeneralSettings(groupName, organizationId);
        }

        public new string[] GetSecurityGroupsNotes(string[] groupNames, string organizationId)
        {
            return base.GetSecurityGroupsNotes(groupNames, organizationId);
        }

        public new void DeleteSecurityGroup(string groupName, string organizationId)
        {
            base.DeleteSecurityGroup(groupName, organizationId);
        }

        public new void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes)
        {
            base.SetSecurityGroupGeneralSettings(organizationId, groupName, memberAccounts, notes);
        }

        public new void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName)
        {
            base.AddObjectToSecurityGroup(organizationId, accountName, groupName);
        }

        public new void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName)
        {
            base.DeleteObjectFromSecurityGroup(organizationId, accountName, groupName);
        }

        public new void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, bool userMustChangePassword)
        {
            base.SetUserGeneralSettings(organizationId, accountName, displayName, password, hideFromAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, userMustChangePassword);
        }

        public new void SetUserPassword(string organizationId, string accountName, string password)
        {
            base.SetUserPassword(organizationId, accountName, password);
        }

        public new void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName)
        {
            base.SetUserPrincipalName(organizationId, accountName, userPrincipalName);
        }

        public new void DeleteOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            base.DeleteOrganizationDomain(organizationDistinguishedName, domain);
        }

        public new void CreateOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            base.CreateOrganizationDomain(organizationDistinguishedName, domain);
        }

        public new PasswordPolicyResult GetPasswordPolicy()
        {
            return base.GetPasswordPolicy();
        }

        public new string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName)
        {
            return base.GetSamAccountNameByUserPrincipalName(organizationId, userPrincipalName);
        }

        public new bool DoesSamAccountNameExist(string accountName)
        {
            return base.DoesSamAccountNameExist(accountName);
        }

        public new MappedDrive[] GetDriveMaps(string organizationId)
        {
            return base.GetDriveMaps(organizationId);
        }

        public new int CreateMappedDrive(string organizationId, string drive, string labelAs, string path)
        {
            return base.CreateMappedDrive(organizationId, drive, labelAs, path);
        }

        public new void DeleteMappedDrive(string organizationId, string drive)
        {
            base.DeleteMappedDrive(organizationId, drive);
        }

        public new void DeleteMappedDriveByPath(string organizationId, string path)
        {
            base.DeleteMappedDriveByPath(organizationId, path);
        }

        public new void DeleteMappedDrivesGPO(string organizationId)
        {
            base.DeleteMappedDrivesGPO(organizationId);
        }

        public new void SetDriveMapsTargetingFilter(string organizationId, ExchangeAccount[] accounts, string folderName)
        {
            base.SetDriveMapsTargetingFilter(organizationId, accounts, folderName);
        }

        public new void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder)
        {
            base.ChangeDriveMapFolderPath(organizationId, oldFolder, newFolder);
        }

        public new List<OrganizationUser> GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration)
        {
            return base.GetOrganizationUsersWithExpiredPassword(organizationId, daysBeforeExpiration);
        }

        public new void ApplyPasswordSettings(string organizationId, OrganizationPasswordSettings passwordSettings)
        {
            base.ApplyPasswordSettings(organizationId, passwordSettings);
        }

        public new bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null)
        {
            return base.CheckPhoneNumberIsInUse(phoneNumber, userSamAccountName);
        }

        public new OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId)
        {
            return base.GetOrganizationUserWithExtraData(loginName, organizationId);
        }

        public new void SetOUAclPermissions(string organizationId)
        {
            base.SetOUAclPermissions(organizationId);
        }

        public new ExchangeAccount[] GetUserGroups(string userName, int organizationId)
        {
            return base.GetUserGroups(userName, organizationId);
        }
    }
}
#endif
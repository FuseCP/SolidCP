// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Services;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;


namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esOrganizations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class esOrganizations : WebService
    {
        #region Organizations

        [WebMethod]
        public bool CheckPhoneNumberIsInUse(int itemId, string phoneNumber, string userSamAccountName = null)
        {
            return OrganizationController.CheckPhoneNumberIsInUse(itemId, phoneNumber, userSamAccountName);
        }

        [WebMethod]
        public void DeletePasswordresetAccessToken(Guid accessToken)
        {
            OrganizationController.DeleteAccessToken(accessToken, AccessTokenTypes.PasswrodReset);
        }

        [WebMethod]
        public void SetAccessTokenResponse(Guid accessToken, string response)
        {
            OrganizationController.SetAccessTokenResponse(accessToken, response);
        }

        [WebMethod]
        public AccessToken GetPasswordresetAccessToken(Guid token)
        {
            return OrganizationController.GetAccessToken(token, AccessTokenTypes.PasswrodReset);
        }

        [WebMethod]
        public void UpdateOrganizationGeneralSettings(int itemId, OrganizationGeneralSettings settings)
        {
            OrganizationController.UpdateOrganizationGeneralSettings(itemId, settings);
        }

        [WebMethod]
        public OrganizationGeneralSettings GetOrganizationGeneralSettings(int itemId)
        {
            return OrganizationController.GetOrganizationGeneralSettings(itemId);
        }

        [WebMethod]
        public void UpdateOrganizationPasswordSettings(int itemId, OrganizationPasswordSettings settings)
        {
            OrganizationController.UpdateOrganizationPasswordSettings(itemId, settings);
        }

        [WebMethod]
        public SystemSettings GetWebDavSystemSettings()
        {
            return OrganizationController.GetWebDavSystemSettings();
        }

        [WebMethod]
        public OrganizationPasswordSettings GetOrganizationPasswordSettings(int itemId)
        {
            return OrganizationController.GetOrganizationPasswordSettings(itemId);
        }

        [WebMethod]
        public bool CheckOrgIdExists(string orgId)
        {
            return OrganizationController.OrganizationIdentifierExists(orgId);
        }

        [WebMethod]
        public int CreateOrganization(int packageId, string organizationID, string organizationName, string domainName)
        {
            return OrganizationController.CreateOrganization(packageId, organizationID, organizationName, domainName);

        }

        [WebMethod]
        public DataSet GetRawOrganizationsPaged(int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return OrganizationController.GetRawOrganizationsPaged(packageId, recursive,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }


        [WebMethod]
        public List<Organization> GetOrganizations(int packageId, bool recursive)
        {
            return OrganizationController.GetOrganizations(packageId, recursive);
        }

        [WebMethod]
        public Organization GetOrganizationById(string organizationId)
        {
            return OrganizationController.GetOrganizationById(organizationId);
        }

        [WebMethod]
        public string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            return OrganizationController.GetOrganizationUserSummuryLetter(itemId, accountId, pmm, emailMode, signup);
        }

        [WebMethod]
        public int SendOrganizationUserSummuryLetter(int itemId, int accountId, bool signup, string to, string cc)
        {
            return OrganizationController.SendSummaryLetter(itemId, accountId, signup, to, cc);
        }

        [WebMethod]
        public int DeleteOrganization(int itemId)
        {
            // domain
            List<string> domainsList = new List<string>();
            List<OrganizationDomainName> domains = OrganizationController.GetOrganizationDomains(itemId);
            foreach (OrganizationDomainName domain in domains)
                domainsList.Add(domain.DomainName);

            // users
            List<string> usersList = new List<string>();
            OrganizationUsersPaged users = OrganizationController.GetOrganizationUsersPaged(itemId, null, null, null, 0, 100000000);
            foreach (OrganizationUser user in users.PageUsers)
                usersList.Add(user.PrimaryEmailAddress);

            int res = OrganizationController.DeleteOrganization(itemId);

            return res;
        }

        [WebMethod]
        public OrganizationStatistics GetOrganizationStatistics(int itemId)
        {
            return OrganizationController.GetOrganizationStatistics(itemId);
        }

        [WebMethod]
        public OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId)
        {
            return OrganizationController.GetOrganizationStatisticsByOrganization(itemId);
        }


        [WebMethod]
        public Organization GetOrganization(int itemId)
        {
            return OrganizationController.GetOrganization(itemId);
        }

        [WebMethod]
        public int GetAccountIdByUserPrincipalName(int itemId, string userPrincipalName)
        {
            return OrganizationController.GetAccountIdByUserPrincipalName(itemId, userPrincipalName);
        }

        [WebMethod]
        public void SetDefaultOrganization(int newDefaultOrganizationId, int currentDefaultOrganizationId)
        {
            OrganizationController.SetDefaultOrganization(newDefaultOrganizationId, currentDefaultOrganizationId);
        }

        [WebMethod]
        public OrganizationUser GetUserGeneralSettingsWithExtraData(int itemId, int accountId)
        {
            return OrganizationController.GetUserGeneralSettingsWithExtraData(itemId, accountId);
        }

        [WebMethod]
        public ResultObject SendResetUserPasswordLinkSms(int itemId, int accountId, string reason, string phoneTo = null)
        {
           return  OrganizationController.SendResetUserPasswordLinkSms(itemId, accountId, reason, phoneTo);
        }


        [WebMethod]
        public ResultObject SendResetUserPasswordPincodeSms(Guid token, string phoneTo = null)
        {
            return OrganizationController.SendResetUserPasswordPincodeSms(token, phoneTo);
        }

        [WebMethod]
        public ResultObject SendResetUserPasswordPincodeEmail(Guid token, string mailTo = null)
        {
            return OrganizationController.SendResetUserPasswordPincodeEmail(token, mailTo);
        }


        [WebMethod]
        public ResultObject SendUserPasswordRequestSms(int itemId, int accountId, string reason, string phoneTo)
        {
            return OrganizationController.SendUserPasswordRequestSms(itemId, accountId, reason, phoneTo);
        }

        [WebMethod]
        public void SendUserPasswordRequestEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            OrganizationController.SendUserPasswordRequestEmail(itemId, accountId, reason, mailTo, finalStep);
        }

    #endregion

        #region Domains

        [WebMethod]
        public int AddOrganizationDomain(int itemId, string domainName)
        {
            return OrganizationController.AddOrganizationDomain(itemId, domainName);
        }

        [WebMethod]
        public int ChangeOrganizationDomainType(int itemId, int domainId, ExchangeAcceptedDomainType newDomainType)
        {
            return OrganizationController.ChangeOrganizationDomainType(itemId, domainId, newDomainType);
        }

        [WebMethod]
        public List<OrganizationDomainName> GetOrganizationDomains(int itemId)
        {
            return OrganizationController.GetOrganizationDomains(itemId);
        }

        [WebMethod]
        public int DeleteOrganizationDomain(int itemId, int domainId)
        {
            return OrganizationController.DeleteOrganizationDomain(itemId, domainId);
        }

        [WebMethod]
        public int SetOrganizationDefaultDomain(int itemId, int domainId)
        {
            return OrganizationController.SetOrganizationDefaultDomain(itemId, domainId);
        }

        [WebMethod]
        public DataSet GetOrganizationObjectsByDomain(int itemId, string domainName)
        {
            return OrganizationController.GetOrganizationObjectsByDomain(itemId, domainName);
        }

        [WebMethod]
        public bool CheckDomainUsedByHostedOrganization(int itemId, int domainId)
        {
            return OrganizationController.CheckDomainUsedByHostedOrganization(itemId, domainId);
        }

        #endregion

        #region Users

        [WebMethod]
        public int CreateUser(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to)
        {
            string accountName;
            int res =  OrganizationController.CreateUser(itemId, displayName, name, domain, password, subscriberNumber, true, sendNotification, to, out accountName);
            return res;
        }


        [WebMethod]
        public int ImportUser(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber)
        {
            int res = OrganizationController.ImportUser(itemId, accountName, displayName, name, domain, password, subscriberNumber);
            return res;
        }

        [WebMethod]
        public OrganizationDeletedUsersPaged GetOrganizationDeletedUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {
            return OrganizationController.GetOrganizationDeletedUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {
            return OrganizationController.GetOrganizationUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public OrganizationUser GetUserGeneralSettings(int itemId, int accountId)
        {
            return OrganizationController.GetUserGeneralSettings(itemId, accountId);
        }

        [WebMethod]
        public int SetUserGeneralSettings(int itemId, int accountId, string displayName,
            string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials,
            string lastName, string address, string city, string state, string zip, string country,
            string jobTitle, string company, string department, string office, string managerAccountName,
            string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, 
            bool userMustChangePassword)
        {
            return OrganizationController.SetUserGeneralSettings(itemId, accountId, displayName,
                password, hideAddressBook, disabled, locked, firstName, initials,
                lastName, address, city, state, zip, country,
                jobTitle, company, department, office, managerAccountName,
                businessPhone, fax, homePhone, mobilePhone, pager,
                webPage, notes, externalEmail, subscriberNumber, levelId, isVIP, userMustChangePassword);
        }


        [WebMethod]
        public int SetUserPrincipalName(int itemId, int accountId, string userPrincipalName, bool inherit)
        {
            return OrganizationController.SetUserPrincipalName(itemId, accountId, userPrincipalName,
                inherit);
        }


        [WebMethod]
        public int SetUserPassword(int itemId, int accountId, string password)
        {
            // load account
            OrganizationUser user = OrganizationController.GetAccount(itemId, accountId);

            int res = OrganizationController.SetUserPassword(itemId, accountId, password);

            return res;
        }


        [WebMethod]
        public List<OrganizationUser> SearchAccounts(int itemId,
            string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return OrganizationController.SearchAccounts(itemId,
                filterColumn, filterValue, sortColumn, includeMailboxes);
        }

        [WebMethod]
        public int SetDeletedUser(int itemId, int accountId, bool enableForceArchive)
        {
            return OrganizationController.SetDeletedUser(itemId, accountId, enableForceArchive);
        }
        
        [WebMethod]
        public byte[] GetArchiveFileBinaryChunk(int packageId, int itemId, int deleteAccountId, int offset, int length)
        {
            return OrganizationController.GetArchiveFileBinaryChunk(packageId, itemId, deleteAccountId, offset, length);
        }

        [WebMethod]
        public int DeleteUser(int itemId, int accountId)
        {
            // load account
            OrganizationUser user = OrganizationController.GetAccount(itemId, accountId);

            int res = OrganizationController.DeleteUser(itemId, accountId);
            return res;
        }


        [WebMethod]
        public PasswordPolicyResult GetPasswordPolicy(int itemId)
        {
            return OrganizationController.GetPasswordPolicy(itemId);
        }

        [WebMethod]
        public void SendResetUserPasswordEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            OrganizationController.SendResetUserPasswordEmail(itemId, accountId, reason, mailTo, finalStep);
        }

        [WebMethod]
        public AccessToken CreatePasswordResetAccessToken(int itemId, int accountId)
        {
            return OrganizationController.CreatePasswordResetAccessToken(itemId, accountId);
        }

        #endregion

        #region Security Groups

        [WebMethod]
        public int CreateSecurityGroup(int itemId, string displayName)
        {
            return OrganizationController.CreateSecurityGroup(itemId, displayName);
        }

        [WebMethod]
        public OrganizationSecurityGroup GetSecurityGroupGeneralSettings(int itemId, int accountId)
        {
            return OrganizationController.GetSecurityGroupGeneralSettings(itemId, accountId);
        }

        [WebMethod]
        public int DeleteSecurityGroup(int itemId, int accountId)
        {
            return OrganizationController.DeleteSecurityGroup(itemId, accountId);
        }

        [WebMethod]
        public int SetSecurityGroupGeneralSettings(int itemId, int accountId, string displayName, string[] memberAccounts, string notes)
        {
            return OrganizationController.SetSecurityGroupGeneralSettings(itemId, accountId, displayName, memberAccounts, notes);
        }

        [WebMethod]
        public ExchangeAccountsPaged GetOrganizationSecurityGroupsPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {
            return OrganizationController.GetOrganizationSecurityGroupsPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public int AddObjectToSecurityGroup(int itemId, int accountId, string groupName)
        {
            return OrganizationController.AddObjectToSecurityGroup(itemId, accountId, groupName);
        }

        [WebMethod]
        public int DeleteObjectFromSecurityGroup(int itemId, int accountId, string groupName)
        {
            return OrganizationController.DeleteObjectFromSecurityGroup(itemId, accountId, groupName);
        }

        [WebMethod]
        public ExchangeAccount[] GetSecurityGroupsByMember(int itemId, int accountId)
        {
            return OrganizationController.GetSecurityGroupsByMember(itemId, accountId);
        }

        [WebMethod]
        public List<ExchangeAccount> SearchOrganizationAccounts(int itemId, string filterColumn, string filterValue,
            string sortColumn, bool includeOnlySecurityGroups)
        {
            return OrganizationController.SearchOrganizationAccounts(itemId, filterColumn, filterValue, sortColumn,
                includeOnlySecurityGroups);
        }

        #endregion

        #region Additional Default Groups

        [WebMethod]
        public List<AdditionalGroup> GetAdditionalGroups(int userId)
        {
            return OrganizationController.GetAdditionalGroups(userId);
        }

        [WebMethod]
        public void UpdateAdditionalGroup(int groupId, string groupName)
        {
            OrganizationController.UpdateAdditionalGroup(groupId, groupName);
        }

        [WebMethod]
        public void DeleteAdditionalGroup(int groupId)
        {
            OrganizationController.DeleteAdditionalGroup(groupId);
        }

        [WebMethod]
        public int AddAdditionalGroup(int userId, string groupName)
        {
            return OrganizationController.AddAdditionalGroup(userId, groupName);
        }

        #endregion

        #region Service Levels

        [WebMethod]
        public ServiceLevel[] GetSupportServiceLevels()
        {
            return OrganizationController.GetSupportServiceLevels();
        }

        [WebMethod]
        public void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription)
        {
            OrganizationController.UpdateSupportServiceLevel(levelID, levelName, levelDescription);
        }

        [WebMethod]
        public ResultObject DeleteSupportServiceLevel(int levelId)
        {
            return OrganizationController.DeleteSupportServiceLevel(levelId);
        }

        [WebMethod]
        public int AddSupportServiceLevel(string levelName, string levelDescription)
        {
            return OrganizationController.AddSupportServiceLevel(levelName, levelDescription);
        }

        [WebMethod]
        public ServiceLevel GetSupportServiceLevel(int levelID)
        {
            return OrganizationController.GetSupportServiceLevel(levelID);
        }

        #endregion

    }
}

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

using System.Collections.Generic;
using SolidCP.Providers.OS;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Providers.HostedSolution
{
    public interface IOrganization
    {
        Organization CreateOrganization(string organizationId);

        void DeleteOrganization(string organizationId);

        int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled);

        void DisableUser(string loginName, string organizationId);

        void DeleteUser(string loginName, string organizationId);

        OrganizationUser GetUserGeneralSettings(string loginName, string organizationId);

        int CreateSecurityGroup(string organizationId, string groupName);

        OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId);

        string[] GetSecurityGroupsNotes(string[] groupNames, string organizationId);

        void DeleteSecurityGroup(string groupName, string organizationId);

        void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes);

        void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName);

        void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName);

        void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password,
                                    bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials,
                                    string lastName,
                                    string address, string city, string state, string zip, string country,
                                    string jobTitle,
                                    string company, string department, string office, string managerAccountName,
                                    string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
                                    string webPage, string notes, string externalEmail,
                                    bool userMustChangePassword);

        void SetUserPassword(string organizationId, string accountName, string password);

        void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName);

        bool OrganizationExists(string organizationId);

        void DeleteOrganizationDomain(string organizationDistinguishedName, string domain);

        void CreateOrganizationDomain(string organizationDistinguishedName, string domain);

        PasswordPolicyResult GetPasswordPolicy();

        string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName);

        bool DoesSamAccountNameExist(string accountName);

        MappedDrive[] GetDriveMaps(string organizationId);

        int CreateMappedDrive(string organizationId, string drive, string labelAs, string path);

        void DeleteMappedDrive(string organizationId, string drive);

        void DeleteMappedDriveByPath(string organizationId, string path);

        void DeleteMappedDrivesGPO(string organizationId);

        void SetDriveMapsTargetingFilter(string organizationId, ExchangeAccount[] accounts, string folderName);

        void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder);

        List<OrganizationUser> GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration);
        void ApplyPasswordSettings(string organizationId, OrganizationPasswordSettings passwordSettings);

        bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null);

        OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId);

        void SetOUAclPermissions(string organizationId);

        ExchangeAccount[] GetUserGroups(string userName, int ouPath);
    }
}

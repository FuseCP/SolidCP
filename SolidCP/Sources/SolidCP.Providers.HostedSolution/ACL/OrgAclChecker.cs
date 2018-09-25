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
using System.DirectoryServices;
using System.IO;
using System.Security.Principal;
using System.Threading;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.HostedSolution.ACL
{
    public class OrgAclChecker : IAclChecker
    {
        private readonly OrganizationProvider _orgProvider;
        private readonly string _organizationId;
        private readonly string _rootDomain;
        private readonly string _rootDomainPath;

        public OrgAclChecker(OrganizationProvider orgProvider, string organizationId, string rootDomain, string rootDomainPath)
        {
            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException(nameof(organizationId));

            _orgProvider = orgProvider;
            _organizationId = organizationId;
            _rootDomain = rootDomain;
            _rootDomainPath = rootDomainPath;

            OuPath = _orgProvider.GetOrganizationPath(_organizationId);
        }

        public string OuPath { get; }

        public AclTestResult GetAclIssues()
        {
            var aclResult = new AclTestResult { Name = _organizationId, OuPath = OuPath };

            var groupAccount = new NTAccount(ActiveDirectoryUtils.GetObjectTargetAccountName(_organizationId, _orgProvider.RootDomain));

            if (ActiveDirectoryUtils.IsInheritanceEnabled(OuPath))
                aclResult.Issues.Add(AclTestIssues.InheritanceEnabled);

            if (ActiveDirectoryUtils.IsIdentityAllowed(OuPath, AdIdentities.EveryoneIdentity))
                aclResult.Issues.Add(AclTestIssues.EveryoneAllowed);
            if (ActiveDirectoryUtils.IsIdentityAllowed(OuPath, AdIdentities.AuthenticatedUsersIdentity))
                aclResult.Issues.Add(AclTestIssues.AuthenticatedUsersAllowed);
            if (ActiveDirectoryUtils.IsIdentityAllowed(OuPath, AdIdentities.PreWindows2000Identity))
                aclResult.Issues.Add(AclTestIssues.PreWindows2000Allowed);

            if (!ActiveDirectoryUtils.HasPermission(OuPath, groupAccount, ActiveDirectoryRights.ListObject))
                aclResult.Issues.Add(AclTestIssues.ListObjectPermission);

            if (!ActiveDirectoryUtils.HasPropertyAccess(OuPath, groupAccount, AdProperties.ReadCn))
                aclResult.Issues.Add(AclTestIssues.ReadCnPropertyAccess);
            if (!ActiveDirectoryUtils.HasPropertyAccess(OuPath, groupAccount, AdProperties.ReadDistinguishedName))
                aclResult.Issues.Add(AclTestIssues.ReadDistinguishedNamePropertyAccess);
            if (!ActiveDirectoryUtils.HasPropertyAccess(OuPath, groupAccount, AdProperties.ReadGpLink))
                aclResult.Issues.Add(AclTestIssues.ReadGpLinkPropertyAccess);
            if (!ActiveDirectoryUtils.HasPropertyAccess(OuPath, groupAccount, AdProperties.ReadGpOption))
                aclResult.Issues.Add(AclTestIssues.ReadGpOptionPropertyAccess);

            if (ActiveDirectoryUtils.GetIdentityAllowedCount(OuPath, groupAccount) > 5)
                aclResult.Issues.Add(AclTestIssues.ManyRulesForOrgGroup);

            this.CheckGroupPermissions(aclResult, _orgProvider.RootDomain, "Recipient Management", ActiveDirectoryRights.GenericAll, AclTestIssues.RecipientManagementFullControl);

            this.CheckGroupPermissions(aclResult, _orgProvider.RootDomain, "Public Folder Management", ActiveDirectoryRights.GenericAll, AclTestIssues.PublicFolderFullControl);

            var privilegedGroup = ActiveDirectoryUtils.GetObjectTargetAccountName("Privileged Services", _rootDomain);
            if (!ActiveDirectoryUtils.AccountExists(privilegedGroup))
                aclResult.Issues.Add(AclTestIssues.PrivelegedServersNotExists);
            else
                if (!ActiveDirectoryUtils.HasPermission(OuPath, new NTAccount(privilegedGroup), ActiveDirectoryRights.GenericRead))
                aclResult.Issues.Add(AclTestIssues.PrivelegedServersReadGeneric);

            return aclResult;
        }

        public void FixAclIssues()
        {
            ActiveDirectoryUtils.DisableInheritance(OuPath);

            ActiveDirectoryUtils.RemoveIdentityAllows(OuPath, AdIdentities.EveryoneIdentity);
            ActiveDirectoryUtils.RemoveIdentityAllows(OuPath, AdIdentities.AuthenticatedUsersIdentity);
            ActiveDirectoryUtils.RemoveIdentityAllows(OuPath, AdIdentities.PreWindows2000Identity);

            this.SetGroupPermissions(_orgProvider.RootDomain, "Recipient Management", ActiveDirectoryRights.GenericAll);

            this.SetGroupPermissions(_orgProvider.RootDomain, "Public Folder Management", ActiveDirectoryRights.GenericAll);

            var groupAccount = ActiveDirectoryUtils.GetObjectTargetAccountName(_organizationId, _orgProvider.RootDomain);
            for (int i = 0; i <= 25; i++)
            {
                if (ActiveDirectoryUtils.AccountExists(groupAccount))
                {
                    Log.WriteWarning($"ACL delay was {i*2} seconds");
                    ActiveDirectoryUtils.AddOrgPermisionsToIdentity(OuPath, new NTAccount(groupAccount));
                    break;
                }

                if (i == 25)
                    throw new Exception($"Can not find {groupAccount} group to set ACL permissions. Probably AD lag, please try again.");

                Thread.Sleep(2000);
            }

            var privilegedGroup = ActiveDirectoryUtils.GetObjectTargetAccountName("Privileged Services", _rootDomain);
            if (!ActiveDirectoryUtils.AccountExists(privilegedGroup))
                ActiveDirectoryUtils.CreateGroup(_rootDomainPath, "Privileged Services");

            ActiveDirectoryUtils.AddPermission(OuPath, new NTAccount(privilegedGroup), ActiveDirectoryRights.GenericRead);
        }
    }
}

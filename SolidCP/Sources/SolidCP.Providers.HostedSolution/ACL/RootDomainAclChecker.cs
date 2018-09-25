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
using System.Security.Principal;

namespace SolidCP.Providers.HostedSolution.ACL
{
    public class RootDomainAclChecker : IAclChecker
    {
        private readonly string _name;
        private readonly string _rootDomain;

        public RootDomainAclChecker(string name, string adOu, string rootDomain)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(adOu))
                throw new ArgumentNullException(nameof(adOu));

            _name = name;
            _rootDomain = rootDomain;
            OuPath = adOu;
        }

        public string OuPath { get; }

        public AclTestResult GetAclIssues()
        {
            var aclResult = new AclTestResult {Name = _name, OuPath = OuPath };

            if (ActiveDirectoryUtils.IsIdentityAllowed(OuPath, AdIdentities.EveryoneIdentity))
                aclResult.Issues.Add(AclTestIssues.EveryoneAllowed);
            if (ActiveDirectoryUtils.IsIdentityAllowed(OuPath, AdIdentities.PreWindows2000Identity))
                aclResult.Issues.Add(AclTestIssues.PreWindows2000Allowed);

            //if (ActiveDirectoryUtils.IsIdentityExistsNotInherited(OuPath, AdIdentities.AuthenticatedUsersIdentity))
            //    aclResult.Issues.Add(AclTestIssues.AuthenticatedUsersNotInheritedPermissions);

            if (!ActiveDirectoryUtils.HasPermission(OuPath, AdIdentities.AuthenticatedUsersIdentity, ActiveDirectoryRights.GenericRead))
                aclResult.Issues.Add(AclTestIssues.AuthenticatedUsersReadGeneric);

            this.CheckGroupPermissions(aclResult, _rootDomain, "Recipient Management", ActiveDirectoryRights.GenericAll, AclTestIssues.RecipientManagementFullControl);

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
            ActiveDirectoryUtils.RemoveIdentityAllows(OuPath, AdIdentities.EveryoneIdentity);
            ActiveDirectoryUtils.RemoveIdentityAllows(OuPath, AdIdentities.PreWindows2000Identity);

            ActiveDirectoryUtils.RemoveIdentityNotInheritedRules(OuPath, AdIdentities.AuthenticatedUsersIdentity);

            ActiveDirectoryUtils.AddPermission(OuPath, AdIdentities.AuthenticatedUsersIdentity, ActiveDirectoryRights.GenericRead);

            this.SetGroupPermissions(_rootDomain, "Recipient Management", ActiveDirectoryRights.GenericAll, ActiveDirectorySecurityInheritance.None);

            var privilegedGroup = ActiveDirectoryUtils.GetObjectTargetAccountName("Privileged Services", _rootDomain);
            if (!ActiveDirectoryUtils.AccountExists(privilegedGroup))
                ActiveDirectoryUtils.CreateGroup(OuPath, "Privileged Services");

            ActiveDirectoryUtils.AddPermission(OuPath, new NTAccount(privilegedGroup), ActiveDirectoryRights.GenericRead);
        }
    }
}

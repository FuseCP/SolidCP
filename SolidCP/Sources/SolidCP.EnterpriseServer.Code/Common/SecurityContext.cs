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
using System.Threading;
using System.Diagnostics;
using System.Security;
using System.Security.Principal;
using System.Web;
using SolidCP.Providers.Common;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Provides security utilities.
    /// </summary>
    public class SecurityContext
    {
        public const string ROLE_ADMINISTRATOR = "Administrator";
        public const string ROLE_RESELLER = "Reseller";
        public const string ROLE_USER = "User";
        public const string ROLE_PLATFORMCSR = "PlatformCSR";
        public const string ROLE_PLATFORMHELPDESK = "PlatformHelpdesk";
        public const string ROLE_RESELLERCSR = "ResellerCSR";
        public const string ROLE_RESELLERHELPDESK = "ResellerHelpdesk";

        public const string CONTEXT_USER_INFO = "CONTEXT_USER_INFO";

        public static void SetThreadPrincipal(int userId)
        {
            UserInfo user = UserController.GetUserInternally(userId);
            if (user == null)
                throw new Exception(String.Format("User '{0}' can not be loaded", userId));

            SetThreadPrincipal(user);
        }

        public static void SetThreadPrincipal(UserInfo user)
        {
            // set roles array
            List<string> roles = new List<string>();
            roles.Add(SecurityContext.ROLE_USER);

            if (user.Role == UserRole.Reseller || user.Role == UserRole.Administrator ||
                user.Role == UserRole.PlatformHelpdesk || user.Role == UserRole.ResellerHelpdesk)
                roles.Add(SecurityContext.ROLE_RESELLERHELPDESK);

            if (user.Role == UserRole.Reseller || user.Role == UserRole.Administrator ||
                user.Role == UserRole.PlatformCSR ||  user.Role == UserRole.ResellerCSR)
                roles.Add(SecurityContext.ROLE_RESELLERCSR);

            if (user.Role == UserRole.Reseller || user.Role == UserRole.Administrator ||
                user.Role == UserRole.PlatformHelpdesk)
                roles.Add(SecurityContext.ROLE_PLATFORMHELPDESK);

            if (user.Role == UserRole.Reseller || user.Role == UserRole.Administrator ||
                user.Role == UserRole.PlatformCSR)
                roles.Add(SecurityContext.ROLE_PLATFORMCSR);
            
            if (user.Role == UserRole.Reseller || user.Role == UserRole.Administrator)
                roles.Add(SecurityContext.ROLE_RESELLER);

            if (user.Role == UserRole.Administrator)
                roles.Add(SecurityContext.ROLE_ADMINISTRATOR);

            // create a new generic principal/identity and place them to context
            EnterpriseServerIdentity identity = new EnterpriseServerIdentity(user.UserId.ToString());
            EnterpriseServerPrincipal principal = new EnterpriseServerPrincipal(identity, roles.ToArray());

            principal.UserId = user.UserId;
            principal.OwnerId = user.OwnerId;
            principal.IsPeer = user.IsPeer;
            principal.IsDemo = user.IsDemo;
            principal.Status = user.Status;

            Thread.CurrentPrincipal = principal;
        }

        public static void SetThreadSupervisorPrincipal()
        {
            UserInfo user = new UserInfo();
            user.UserId = -1;
            user.OwnerId = 0;
            user.IsPeer = false;
            user.IsDemo = false;
            user.Status = UserStatus.Active;
            user.Role = UserRole.Administrator;

            SetThreadPrincipal(user);
        }

        public static EnterpriseServerPrincipal User
        {
            get
            {
                EnterpriseServerPrincipal principal = Thread.CurrentPrincipal as EnterpriseServerPrincipal;
                if(principal != null)
                    return principal;

                // Username Token Manager was unable to set principal
                // or authentication is disabled
                // create supervisor principal
                SetThreadSupervisorPrincipal();

                return (EnterpriseServerPrincipal)Thread.CurrentPrincipal;
            }
        }

        public static bool CheckAccount(ResultObject res, DemandAccount demand)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                res.ErrorCodes.Add(BusinessErrorCodes.ToText(accountCheck));
                return false;
            }
            return true;
        }

        public static int CheckAccount(DemandAccount demand)
        {
            if ((demand & DemandAccount.NotDemo) == DemandAccount.NotDemo)
            {
                // should make a check if the account is not in demo mode
                if (User.IsDemo)
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_DEMO;
            }

            if ((demand & DemandAccount.IsActive) == DemandAccount.IsActive)
            {
                // check is the account is active
                if (User.Status == UserStatus.Pending)
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_PENDING;
                else if (User.Status == UserStatus.Suspended)
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_SUSPENDED;
                else if (User.Status == UserStatus.Cancelled)
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_CANCELLED;
            }

            if ((demand & DemandAccount.IsAdmin) == DemandAccount.IsAdmin)
            {
                // should make a check if the account has Admin role
                if (!User.IsInRole(ROLE_ADMINISTRATOR))
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_SHOULD_BE_ADMINISTRATOR;
            }

            if ((demand & DemandAccount.IsReseller) == DemandAccount.IsReseller)
            {
                // should make a check if the account has Admin role
                if (!User.IsInRole(ROLE_RESELLER))
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_NOT_ENOUGH_PERMISSIONS;
            }

            if ((demand & DemandAccount.IsPlatformCSR) == DemandAccount.IsPlatformCSR)
            {
                // should make a check if the account has Admin role
                if (!User.IsInRole(ROLE_PLATFORMCSR))
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_NOT_ENOUGH_PERMISSIONS;
            }

            if ((demand & DemandAccount.IsPlatformHelpdesk) == DemandAccount.IsPlatformHelpdesk)
            {
                // should make a check if the account has Admin role
                if (!User.IsInRole(ROLE_PLATFORMHELPDESK))
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_NOT_ENOUGH_PERMISSIONS;
            }


            if ((demand & DemandAccount.IsResellerHelpdesk) == DemandAccount.IsResellerHelpdesk)
            {
                // should make a check if the account has Admin role
                if (!User.IsInRole(ROLE_RESELLERHELPDESK))
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_NOT_ENOUGH_PERMISSIONS;
            }


            if ((demand & DemandAccount.IsResellerCSR) == DemandAccount.IsResellerCSR)
            {
                // should make a check if the account has Admin role
                if (!User.IsInRole(ROLE_RESELLERCSR))
                    return BusinessErrorCodes.ERROR_USER_ACCOUNT_NOT_ENOUGH_PERMISSIONS;
            }


            return 0;
        }

        public static bool CheckPackage(ResultObject res, int packageId, DemandPackage demand)
        {
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0)
            {
                res.ErrorCodes.Add(BusinessErrorCodes.ToText(packageCheck));
                return false;
            }
            return true;
        }

        public static int CheckPackage(int packageId, DemandPackage demand)
        {
            // load package
            PackageInfo package = PackageController.GetPackage(packageId);
            if (package == null)
                return BusinessErrorCodes.ERROR_PACKAGE_NOT_FOUND;

            return CheckPackage(package, demand);
        }

        public static int CheckPackage(PackageInfo package, DemandPackage demand)
        {
            if ((demand & DemandPackage.IsActive) == DemandPackage.IsActive)
            {
                // should make a check if the package is active
                if (package.StatusId == (int)PackageStatus.Cancelled)
                    return BusinessErrorCodes.ERROR_PACKAGE_CANCELLED;
                else if (package.StatusId == (int)PackageStatus.Suspended)
                    return BusinessErrorCodes.ERROR_PACKAGE_SUSPENDED;
            }

            return 0;
        }
    }
}

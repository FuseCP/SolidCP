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
using SolidCP.Providers.Filters;
using SolidCP.EnterpriseServer.Base;
using System;
using System.Collections.Specialized;

namespace SolidCP.EnterpriseServer
{
    public class SpamExpertsController
    {
        internal static SpamExperts GetServer(int serviceId)
        {
            SpamExperts ws = new SpamExperts();

            ServiceProviderProxy.Init(ws, serviceId);

            string[] settings = ws.ServiceProviderSettingsSoapHeaderValue.Settings;

            List<string> resSettings = new List<string>(settings);

            ws.ServiceProviderSettingsSoapHeaderValue.Settings = resSettings.ToArray();
            return ws;
        }

        private static int GetServiceId(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.Filters);
        }

        private static bool IsPackageServiceEnabled(int packageId, int serviceId)
        {
            QuotaValueInfo quota = PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE);
            return (serviceId != 0 && Convert.ToBoolean(quota.QuotaAllocatedValue));
        }

        public static int AddDomainFilter(SpamExpertsRoute route)
        {
            int serviceId = GetServiceId(route.PackageId);

            if (!IsPackageServiceEnabled(route.PackageId,serviceId))
                return -1;

            SpamExperts server = GetServer(serviceId);

            var res = server.AddDomainFilter(route.DomainName, "", "postmaster@" + route.DomainName, route.Destinations);
            return (int)res.Status;
        }

        public static void DeleteDomainFilter(DomainInfo domain)
        {
            int serviceId = GetServiceId(domain.PackageId);

            if (IsPackageServiceEnabled(domain.PackageId, serviceId))
            {
                SpamExperts server = GetServer(serviceId);
                var res = server.DeleteDomainFilter(domain.DomainName);
            }
        }

        public static int AddDomainFilterAlias(DomainInfo domain, string alias)
        {
            int serviceId = GetServiceId(domain.PackageId);

            if (!IsPackageServiceEnabled(domain.PackageId, serviceId))
                return -1;

            SpamExperts server = GetServer(serviceId);

            var res = server.AddDomainFilterAlias(domain.DomainName, alias);
            return (int)res.Status;
        }

        public static void DeleteDomainFilterAlias(DomainInfo domain, string alias)
        {
            int serviceId = GetServiceId(domain.PackageId);

            if (IsPackageServiceEnabled(domain.PackageId, serviceId))
            {
                SpamExperts server = GetServer(serviceId);
                var res = server.DeleteDomainFilterAlias(domain.DomainName,alias);
            }
        }

        public static int AddEmailFilter(int packageId, string username, string password, string domain)
        {
            int serviceId = GetServiceId(packageId);

            if (!IsPackageServiceEnabled(packageId, serviceId))
                return -1;
            if (Convert.ToBoolean(PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE_EMAIL_USERS).QuotaAllocatedValue))
            {
                SpamExperts server = GetServer(serviceId);

                var res = server.AddEmailFilter(username, domain, password);
                return (int)res.Status;
            }
            return -1;
        }

        public static void DeleteEmailFilter(int packageId, string email)
        {
            int serviceId = GetServiceId(packageId);

            if (IsPackageServiceEnabled(packageId, serviceId))
            {
                if (Convert.ToBoolean(PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE_EMAIL_USERS).QuotaAllocatedValue))
                {
                    SpamExperts server = GetServer(serviceId);
                    var res = server.DeleteEmailFilter(email);
                }
            }
        }

        public static void SetEmailFilterPassword(int packageId, string email, string password)
        {
            int serviceId = GetServiceId(packageId);

            if (IsPackageServiceEnabled(packageId, serviceId))
            {
                if (Convert.ToBoolean(PackageController.GetPackageQuota(packageId, Quotas.FILTERS_ENABLE_EMAIL_USERS).QuotaAllocatedValue))
                {
                    SpamExperts server = GetServer(serviceId);
                    var res = server.SetEmailFilterUserPassword(email, password);
                }
            }
        }

        public static bool IsSpamExpertsEnabled(int packageId)
        {
            int serviceId = GetServiceId(packageId);
            if (IsPackageServiceEnabled(packageId, serviceId))
            {
                StringDictionary exSettings = ServerController.GetServiceSettings(serviceId);
                return (exSettings != null && Convert.ToBoolean(exSettings["EnableMailFilter"]));
            }
            return false;
        }
    }
}

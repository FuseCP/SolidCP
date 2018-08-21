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

namespace SolidCP.EnterpriseServer
{
    public class SpamExpertsController
    {
        private static string defaultPassword = "5)^KN$w/:9rH";

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

        private static bool ResourceGroupDisabled(int serviceId)
        {
            return serviceId == 0;
        }

        public static void DeleteDomainFilter(DomainInfo domain)
        {
            int serviceId = GetServiceId(domain.PackageId);

            if (ResourceGroupDisabled(serviceId))
                return;

            SpamExperts server = GetServer(serviceId);

            var res = server.DeleteDomainFilter(domain.DomainName);
        }

        public static int AddDomainFilter(SpamExpertsRoute route)
        {
            int serviceId = GetServiceId(route.PackageId);
            serviceId.ToString();

            if (ResourceGroupDisabled(serviceId))
                return -1;

            SpamExperts server = GetServer(serviceId);

            var res = server.AddDomainFilter(route.DomainName,"","",route.Destinations);
            return (int)res.Status;
        }
    }
}

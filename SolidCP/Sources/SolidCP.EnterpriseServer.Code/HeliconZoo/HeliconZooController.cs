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
using System.Data;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using SolidCP.Providers.HeliconZoo;
using SolidCP.Providers.Web;
using SolidCP.Server;

namespace SolidCP.EnterpriseServer
{
    public class HeliconZooController
    {
        public const string HeliconZooQuotaPrefix = "HeliconZoo.";

        public static HeliconZooEngine[] GetEngines(int serviceId)
        {
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            return zooServer.GetEngines();
        }

        public static void SetEngines(int serviceId, HeliconZooEngine[] userEngines)
        {
            // update applicationHost.config
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            zooServer.SetEngines(userEngines);

            // update Helicon Zoo quotas in Quotas table
            UpdateQuotas(serviceId, userEngines);
        }

        public static bool IsEnginesEnabled(int serviceId)
        {
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            return zooServer.IsEnginesEnabled();
        }

        public static void SwithEnginesEnabled(int serviceId, bool enabled)
        {
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            zooServer.SwithEnginesEnabled(enabled);
        }

        public static ShortHeliconZooEngine[] GetAllowedHeliconZooQuotasForPackage(int packageId)
        {
            // first, check IsEnginesAllowed for this server
            
            // get helicon zoo provider serviceId
            int heliconZooProviderId, heliconZooGroupId;
            DataProvider.GetHeliconZooProviderAndGroup("HeliconZoo", out heliconZooProviderId, out heliconZooGroupId);

            // get helicon zoo service id for heliconZooProviderId and packageId
            int serviceId = DataProvider.GetServiceIdForProviderIdAndPackageId(heliconZooProviderId, packageId);

            if (serviceId > 0)
            {
                if (IsEnginesEnabled(serviceId))
                {
                    // all engines allowed by default
                    return new ShortHeliconZooEngine[]
                    {
                        new ShortHeliconZooEngine{Name = "*", DisplayName = "*", Enabled = true} 
                    };
                }
            }


            // all engines is not allowed
            // get allowed engines from hosting plan quotas

            List<ShortHeliconZooEngine> allowedEngines = new List<ShortHeliconZooEngine>();

            IDataReader reader = DataProvider.GetEnabledHeliconZooQuotasForPackage(packageId);

            while (reader.Read())
            {
                allowedEngines.Add( new ShortHeliconZooEngine(){
                    Name = (string)reader["QuotaName"],
                    DisplayName= (string)reader["QuotaDescription"],
                    Enabled = true

                });
            }

            return allowedEngines.ToArray();
        }

        public static string[] GetEnabledEnginesForSite(string siteId, int packageId)
        {
            int serviceId = GetHeliconZooServiceIdByPackageId(packageId);

            if (-1 == serviceId)
            {
                // Helicon Zoo is not enabled for this package
                return new string[0];
            }

            // ask Server to enabled engines for site
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            string[] enabledEngines = zooServer.GetEnabledEnginesForSite(siteId);

            return enabledEngines;
        }

        public static void SetEnabledEnginesForSite(string siteId, int packageId, string[] engines)
        {
            int serviceId = GetHeliconZooServiceIdByPackageId(packageId);

            // tell Server to enable engines for site
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            zooServer.SetEnabledEnginesForSite(siteId, engines);
        }


        public static bool IsWebCosoleEnabled(int serviceId)
        {
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            return zooServer.IsWebCosoleEnabled();
        }

        public static void SetWebCosoleEnabled(int serviceId, bool enabled)
        {
            HeliconZoo zooServer = new HeliconZoo();
            ServiceProviderProxy.Init(zooServer, serviceId);
            zooServer.SetWebCosoleEnabled(enabled);
        }


        #region private helpers
        private static void UpdateQuotas(int serviceId, HeliconZooEngine[] userEngines)
        {
            List<HeliconZooEngine> updatedEngines = new List<HeliconZooEngine>(userEngines);

            int providerId, groupId;
            DataProvider.GetHeliconZooProviderAndGroup("HeliconZoo", out providerId, out groupId);


            // get existing Helicon Zoo quotas
            List<string> existingQuotas = new List<string>();
            IDataReader reader = DataProvider.GetHeliconZooQuotas(providerId);
            while (reader.Read())
            {
                string quota = (string) reader["QuotaName"];
                if (quota.StartsWith(HeliconZooQuotaPrefix))
                {
                    quota = quota.Substring(HeliconZooQuotaPrefix.Length);
                }
                existingQuotas.Add(quota);
            }

            // sort: engine to remove and add
            List<string> engineNamesToRemove = new List<string>();
            List<HeliconZooEngine> enginesToAdd = new List<HeliconZooEngine>();

            // find engine to remove in existing engines
            foreach (string existingEngineName in existingQuotas)
            {
                if (
                    Array.Find(updatedEngines.ToArray(), engine => engine.name == existingEngineName) == null 
                    &&
                    !engineNamesToRemove.Contains(existingEngineName)
                    )
                {
                    engineNamesToRemove.Add(existingEngineName);
                }
            }


            // find engines to add
            foreach (HeliconZooEngine engine in updatedEngines)
            {
                if (!existingQuotas.Contains(engine.name))
                {
                    enginesToAdd.Add(engine);
                }
            }

            // remove engines
            foreach (string engineName in engineNamesToRemove)
            {
                DataProvider.RemoveHeliconZooQuota(groupId, HeliconZooQuotaPrefix+engineName);
            }

            // add engines
            int order = 0;
            foreach (HeliconZooEngine engine in enginesToAdd)
            {
                int quotaId = GenerateIntId(engine);
                DataProvider.AddHeliconZooQuota(groupId, quotaId, 
                    HeliconZooQuotaPrefix+engine.name, 
                    engine.displayName,
                    existingQuotas.Count + order++);
            }
        }

        private static int GenerateIntId(HeliconZooEngine engine)
        {
            return engine.name.GetHashCode();
        }

        private static int GetHeliconZooServiceIdByPackageId(int packageId)
        {
            // get server id
            int serverId = DataProvider.GetServerIdForPackage(packageId);
            if (-1 == serverId)
            {
                throw new Exception(string.Format("Server not found for package {0}", packageId));
            }

            // get Helicon Zoo provider
            int heliconZooProviderId = -1;
            List<ProviderInfo> providers = ServerController.GetProviders();
            foreach (ProviderInfo providerInfo in providers)
            {
                if (string.Equals("HeliconZoo", providerInfo.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    heliconZooProviderId = providerInfo.ProviderId;
                    break;
                }
            }

            if (-1 == heliconZooProviderId)
            {
                throw new Exception("Helicon Zoo provider not found");
            }

            // get Helicon Zoo service for site
            int serviceId = DataProvider.GetServiceIdByProviderForServer(heliconZooProviderId, packageId);
            return serviceId;
        }

        #endregion
    }
}

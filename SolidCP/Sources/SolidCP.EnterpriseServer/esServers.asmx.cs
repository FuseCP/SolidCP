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
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using SolidCP.EnterpriseServer.Base.Common;
using SolidCP.Providers.Common;
using Microsoft.Web.Services3;

using SolidCP.Providers.DNS;
using SolidCP.Server;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers;
using SolidCP.Providers.DomainLookup;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esServers : System.Web.Services.WebService
    {
        /*
        public const string MAIN_WPI_FEED = "https://www.microsoft.com/web/webpi/5.0/WebProductList.xml";
        public const string HELICON_WPI_FEED = "http://www.helicontech.com/zoo/feed/wsp4";
        */

        #region Servers
        [WebMethod]
        public List<ServerInfo> GetAllServers()
        {
            return ServerController.GetAllServers();
        }

        [WebMethod]
        public DataSet GetRawAllServers()
        {
            return ServerController.GetRawAllServers();
        }

        [WebMethod]
        public List<ServerInfo> GetServers()
        {
            return ServerController.GetServers();
        }

        [WebMethod]
        public DataSet GetRawServers()
        {
            return ServerController.GetRawServers();
        }

        [WebMethod]
        public ServerInfo GetServerShortDetails(int serverId)
        {
            return ServerController.GetServerShortDetails(serverId);
        }

        [WebMethod]
        public ServerInfo GetServerById(int serverId)
        {
            return ServerController.GetServerById(serverId, false);
        }

        [WebMethod]
        public ServerInfo GetServerByName(string serverName)
        {
            return ServerController.GetServerByName(serverName);
        }

        [WebMethod]
        public int CheckServerAvailable(string serverUrl, string password)
        {
            return ServerController.CheckServerAvailable(serverUrl, password);
        }

        [WebMethod]
        public int AddServer(ServerInfo server, bool autoDiscovery)
        {
            return ServerController.AddServer(server, autoDiscovery);
        }

        [WebMethod]
        public int UpdateServer(ServerInfo server)
        {
            return ServerController.UpdateServer(server);
        }

        [WebMethod]
        public int UpdateServerConnectionPassword(int serverId, string password)
        {
            return ServerController.UpdateServerConnectionPassword(serverId, password);
        }

        [WebMethod]
        public int UpdateServerADPassword(int serverId, string adPassword)
        {
            return ServerController.UpdateServerADPassword(serverId, adPassword);
        }

        [WebMethod]
        public int DeleteServer(int serverId)
        {
            return ServerController.DeleteServer(serverId);
        }

        [WebMethod]
        public List<string> AutoUpdateServer(List<List<int>> servers, string downloadLink, string fileName) {
            List<string> res = new List<string>();
            using(System.Net.WebClient wc = new System.Net.WebClient()) { 
                byte[] zipFile = wc.DownloadData(downloadLink + fileName);

                foreach(List<int> x in servers) {
                    if(x.Count > 0) { 
                        try { 
                            Dictionary<int,string> s = ServerController.AutoUpdateServer(x[0], x[1], zipFile, fileName);
                            foreach(KeyValuePair<int, string> i in s) { 
                                if(i.Key != 0) {
                                    res.Add(i.Key+"-"+i.Value);
                                }
                            }
                        } catch(Exception e) {
                            res.Add(x[0] + "-" + e.Message);
                        }
                    }
                }
            }
            return res;
        }
        #endregion

        #region Virtual Servers
        [WebMethod]
        public DataSet GetVirtualServers()
        {
            return ServerController.GetVirtualServers();
        }

        [WebMethod]
        public DataSet GetAvailableVirtualServices(int serverId)
        {
            return ServerController.GetAvailableVirtualServices(serverId);
        }

        [WebMethod]
        public DataSet GetVirtualServices(int serverId)
        {
            return ServerController.GetVirtualServices(serverId, false);
        }

        [WebMethod]
        public int AddVirtualServices(int serverId, int[] ids)
        {
            return ServerController.AddVirtualServices(serverId, ids);
        }

        [WebMethod]
        public int DeleteVirtualServices(int serverId, int[] ids)
        {
            return ServerController.DeleteVirtualServices(serverId, ids);
        }

        [WebMethod]
        public int UpdateVirtualGroups(int serverId, VirtualGroupInfo[] groups)
        {
            return ServerController.UpdateVirtualGroups(serverId, groups);
        }
        #endregion

        #region Services
        [WebMethod]
        public DataSet GetRawServicesByServerId(int serverId)
        {
            return ServerController.GetRawServicesByServerId(serverId);
        }

        [WebMethod]
        public List<ServiceInfo> GetServicesByServerId(int serverId)
        {
            return ServerController.GetServicesByServerId(serverId);
        }

        [WebMethod]
        public List<ServiceInfo> GetServicesByServerIdGroupName(int serverId, string groupName)
        {
            return ServerController.GetServicesByServerIdGroupName(serverId, groupName);
        }

        [WebMethod]
        public DataSet GetRawServicesByGroupId(int groupId)
        {
            return ServerController.GetRawServicesByGroupId(groupId);
        }

        [WebMethod]
        public DataSet GetRawServicesByGroupName(string groupName)
        {
            return ServerController.GetRawServicesByGroupName(groupName, false);
        }

        [WebMethod]
        public ServiceInfo GetServiceInfo(int serviceId)
        {
            return ServerController.GetServiceInfoAdmin(serviceId);
        }

        [WebMethod]
        public int AddService(ServiceInfo service)
        {
            return ServerController.AddService(service);
        }

        [WebMethod]
        public int UpdateService(ServiceInfo service)
        {
            return ServerController.UpdateService(service);
        }

        [WebMethod]
        public int DeleteService(int serviceId)
        {
            return ServerController.DeleteService(serviceId);
        }

        [WebMethod]
        public string[] GetServiceSettings(int serviceId)
        {
            return ConvertDictionaryToArray(ServerController.GetServiceSettingsAdmin(serviceId));
        }

        [WebMethod]
        public string[] GetServiceSettingsRDS(int serviceId)
        {
            return ConvertDictionaryToArray(ServerController.GetServiceSettings(serviceId));
        }

        [WebMethod]
        public int UpdateServiceSettings(int serviceId, string[] settings)
        {
            return ServerController.UpdateServiceSettings(serviceId, settings);
        }

        [WebMethod]
        public string[] InstallService(int serviceId)
        {
            return ServerController.InstallService(serviceId);
        }

        [WebMethod]
        public QuotaInfo GetProviderServiceQuota(int providerId)
        {
            return ServerController.GetProviderServiceQuota(providerId);
        }

		[WebMethod]
		public string[] GetMailServiceSettingsByPackage(int packageID)
		{
			return ConvertDictionaryToArray(ServerController.GetMailServiceSettingsByPackage(packageID));
		}

		#endregion

		#region Providers

		[WebMethod]
        public List<ProviderInfo> GetInstalledProviders(int groupId)
        {
            return ServerController.GetInstalledProviders(groupId);
        }

        [WebMethod]
        public List<ResourceGroupInfo> GetResourceGroups()
        {
            return ServerController.GetResourceGroups();
        }

        [WebMethod]
        public ResourceGroupInfo GetResourceGroup(int groupId)
        {
            return ServerController.GetResourceGroup(groupId);
        }

        [WebMethod]
        public ProviderInfo GetProvider(int providerId)
        {
            return ServerController.GetProvider(providerId);
        }

        [WebMethod]
        public List<ProviderInfo> GetProviders()
        {
            return ServerController.GetProviders();
        }

        [WebMethod]
        public List<ProviderInfo> GetProvidersByGroupId(int groupId)
        {
            return ServerController.GetProvidersByGroupID(groupId);
        }

        [WebMethod]
        public ProviderInfo GetPackageServiceProvider(int packageId, string groupName)
        {
            return ServerController.GetPackageServiceProvider(packageId, groupName);
        }

        [WebMethod]
        public String GetMailFilterUrl(int packageId, string groupName)
        {
            return ServerController.GetMailFilterUrl(packageId, groupName);
        }

        [WebMethod]
        public String GetMailFilterUrlByHostingPlan(int PlanID, string groupName)
        {
            return ServerController.GetMailFilterUrlByHostingPlan(PlanID, groupName);
        }

        [WebMethod]
        public BoolResult IsInstalled(int serverId, int providerId)
        {
            return ServerController.IsInstalled(serverId, providerId);
        }

        [WebMethod]
        public string GetServerVersion(int serverId)
        {
            return ServerController.GetServerVersion(serverId);
        }

        [WebMethod]
        public string GetServerFilePath(int serverId) {
            return ServerController.GetServerFilePath(serverId);
        }

        [WebMethod]
        public string GetQuotaHidden(string quotaName, int groupID)
        {
            return ServerController.GetQuotaHidden(quotaName, groupID);
        }

        [WebMethod]
        public int UpdateQuotaHidden(string quotaName, int groupID, bool hideQuota)
        {
            return ServerController.UpdateQuotaHidden(quotaName, groupID, hideQuota);
        }
        #endregion

        #region Private / DMZ Network VLANs
        [WebMethod]
        public VLANsPaged GetPrivateNetworVLANsPaged(int serverId, string filterColumn, 
            string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return ServerController.GetPrivateNetworVLANsPaged(serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public IntResult AddPrivateNetworkVLAN(int serverId, int vlan, string comments)
        {
            return ServerController.AddPrivateNetworkVLAN(serverId, vlan, comments);
        }

        [WebMethod]
        public ResultObject DeletePrivateNetworkVLANs(int[] vlans)
        {
            return ServerController.DeletePrivateNetworkVLANs(vlans);
        }

        [WebMethod]
        public ResultObject AddPrivateNetworkVLANsRange(int serverId, int startVLAN, int endVLAN, string comments)
        {
            return ServerController.AddPrivateNetworkVLANsRange(serverId, startVLAN, endVLAN, comments);
        }

        [WebMethod]
        public VLANInfo GetPrivateNetworVLAN(int vlanId)
        {
            return ServerController.GetPrivateNetworVLAN(vlanId);
        }

        [WebMethod]
        public ResultObject UpdatePrivateNetworVLAN(int vlanId, int serverId, int vlan, string comments)
        {
            return ServerController.UpdatePrivateNetworVLAN(vlanId, serverId, vlan, comments);
        }

        [WebMethod]
        public PackageVLANsPaged GetPackagePrivateNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return ServerController.GetPackagePrivateNetworkVLANs(packageId, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public PackageVLANsPaged GetPackageDmzNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return ServerController.GetPackageDmzNetworkVLANs(packageId, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public ResultObject DeallocatePackageVLANs(int packageId, int[] vlanId)
        {
            return ServerController.DeallocatePackageVLANs(packageId, vlanId);
        }

        [WebMethod]
        public List<VLANInfo> GetUnallottedVLANs(int packageId, string groupName)
        {
            return ServerController.GetUnallottedVLANs(packageId, groupName);
        }

        [WebMethod]
        public ResultObject AllocatePackageVLANs(int packageId, string groupName, bool allocateRandom, int vlansNumber, int[] vlanId, bool isDmz)
        {
            return ServerController.AllocatePackageVLANs(packageId, groupName, allocateRandom, vlansNumber, vlanId, isDmz);
        }
        #endregion

        #region IP Addresses
        [WebMethod]
        public List<IPAddressInfo> GetIPAddresses(IPAddressPool pool, int serverId)
        {
            return ServerController.GetIPAddresses(pool, serverId);
        }

        [WebMethod]
        public IPAddressesPaged GetIPAddressesPaged(IPAddressPool pool, int serverId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return ServerController.GetIPAddressesPaged(pool, serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public IPAddressInfo GetIPAddress(int addressId)
        {
            return ServerController.GetIPAddress(addressId);
        }

        [WebMethod]
        public IntResult AddIPAddress(IPAddressPool pool, int serverId,
            string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return ServerController.AddIPAddress(pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        [WebMethod]
        public ResultObject AddIPAddressesRange(IPAddressPool pool, int serverId,
            string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return ServerController.AddIPAddressesRange(pool, serverId, externalIP, endIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        [WebMethod]
        public ResultObject UpdateIPAddress(int addressId, IPAddressPool pool, int serverId,
            string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return ServerController.UpdateIPAddress(addressId, pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
        }

        [WebMethod]
        public ResultObject UpdateIPAddresses(int[] addresses, IPAddressPool pool, int serverId,
            string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            return ServerController.UpdateIPAddresses(addresses, pool, serverId, subnetMask, defaultGateway, comments, VLAN);
        }

        [WebMethod]
        public ResultObject DeleteIPAddress(int addressId)
        {
            return ServerController.DeleteIPAddress(addressId);
        }

        [WebMethod]
        public ResultObject DeleteIPAddresses(int[] addresses)
        {
            return ServerController.DeleteIPAddresses(addresses);
        }
        #endregion

        #region Package IP Adderesses
        [WebMethod]
        public List<IPAddressInfo> GetUnallottedIPAddresses(int packageId, string groupName, IPAddressPool pool)
        {
            return ServerController.GetUnallottedIPAddresses(packageId, groupName, pool);
        }

        [WebMethod]
        public PackageIPAddressesPaged GetPackageIPAddresses(int packageId, int orgId, IPAddressPool pool,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return ServerController.GetPackageIPAddresses(packageId, orgId, pool,
                filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);
        }

        [WebMethod]
        public int GetPackageIPAddressesCount(int packageId, int orgId, IPAddressPool pool)
        {
            return ServerController.GetPackageIPAddressesCount(packageId, orgId, pool);
        }

        [WebMethod]
        public List<PackageIPAddress> GetPackageUnassignedIPAddresses(int packageId, int orgId, IPAddressPool pool)
        {
            return ServerController.GetPackageUnassignedIPAddresses(packageId, orgId, pool);
        }

        [WebMethod]
        public ResultObject AllocatePackageIPAddresses(int packageId,int orgId, string groupName, IPAddressPool pool, bool allocateRandom, int addressesNumber,
            int[] addressId)
        {
            return ServerController.AllocatePackageIPAddresses(packageId, orgId, groupName, pool, allocateRandom,
                addressesNumber, addressId);
        }

        [WebMethod]
        public ResultObject AllocateMaximumPackageIPAddresses(int packageId, string groupName, IPAddressPool pool)
        {
            return ServerController.AllocateMaximumPackageIPAddresses(packageId, groupName, pool);
        }

        [WebMethod]
        public ResultObject DeallocatePackageIPAddresses(int packageId, int[] addressId)
        {
            return ServerController.DeallocatePackageIPAddresses(packageId, addressId);
        }
        #endregion

        #region Clusters
        [WebMethod]
        public List<ClusterInfo> GetClusters()
        {
            return ServerController.GetClusters();
        }

        [WebMethod]
        public int AddCluster(ClusterInfo cluster)
        {
            return ServerController.AddCluster(cluster);
        }

        [WebMethod]
        public int DeleteCluster(int clusterId)
        {
            return ServerController.DeleteCluster(clusterId);
        }
        #endregion

        #region Global DNS records
        [WebMethod]
        public DataSet GetRawDnsRecordsByService(int serviceId)
        {
            return ServerController.GetRawDnsRecordsByService(serviceId);
        }

        [WebMethod]
        public DataSet GetRawDnsRecordsByServer(int serverId)
        {
            return ServerController.GetRawDnsRecordsByServer(serverId);
        }

        [WebMethod]
        public DataSet GetRawDnsRecordsByPackage(int packageId)
        {
            return ServerController.GetRawDnsRecordsByPackage(packageId);
        }

        [WebMethod]
        public DataSet GetRawDnsRecordsByGroup(int groupId)
        {
            return ServerController.GetRawDnsRecordsByGroup(groupId);
        }

        [WebMethod]
        public List<GlobalDnsRecord> GetDnsRecordsByService(int serviceId)
        {
            return ServerController.GetDnsRecordsByService(serviceId);
        }

        [WebMethod]
        public List<GlobalDnsRecord> GetDnsRecordsByServer(int serverId)
        {
            return ServerController.GetDnsRecordsByServer(serverId);
        }

        [WebMethod]
        public List<GlobalDnsRecord> GetDnsRecordsByPackage(int packageId)
        {
            return ServerController.GetDnsRecordsByPackage(packageId);
        }

        [WebMethod]
        public List<GlobalDnsRecord> GetDnsRecordsByGroup(int groupId)
        {
            return ServerController.GetDnsRecordsByGroup(groupId);
        }

        [WebMethod]
        public GlobalDnsRecord GetDnsRecord(int recordId)
        {
            return ServerController.GetDnsRecord(recordId);
        }

        [WebMethod]
        public int AddDnsRecord(GlobalDnsRecord record)
        {
            return ServerController.AddDnsRecord(record);
        }

        [WebMethod]
        public int UpdateDnsRecord(GlobalDnsRecord record)
        {
            return ServerController.UpdateDnsRecord(record);
        }

        [WebMethod]
        public int DeleteDnsRecord(int recordId)
        {
            return ServerController.DeleteDnsRecord(recordId);
        }
        #endregion

        #region Domains

        [WebMethod]
        public List<DnsRecordInfo> GetDomainDnsRecords(int domainId)
        {
            return ServerController.GetDomainDnsRecords(domainId);
        }

        [WebMethod]
        public List<DomainInfo> GetDomains(int packageId)
        {
            return ServerController.GetDomains(packageId);
        }

        [WebMethod]
        public List<DomainInfo> GetDomainsByDomainId(int domainId)
        {
            return ServerController.GetDomainsByDomainItemId(domainId);
        }
        
        [WebMethod]
        public List<DomainInfo> GetMyDomains(int packageId)
        {
            return ServerController.GetMyDomains(packageId);
        }

        [WebMethod]
        public List<DomainInfo> GetResellerDomains(int packageId)
        {
            return ServerController.GetResellerDomains(packageId);
        }

        [WebMethod]
        public DataSet GetDomainsPaged(int packageId, int serverId, bool recursive, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return ServerController.GetDomainsPaged(packageId, serverId, recursive, filterColumn, filterValue, sortColumn,
                startRow, maximumRows);
        }

        [WebMethod]
        public DomainInfo GetDomain(int domainId)
        {
            return ServerController.GetDomain(domainId);
        }

        [WebMethod]
        public int AddDomain(DomainInfo domain)
        {
            int res = ServerController.AddDomain(domain);
            return res;
        }

        [WebMethod]
        public int AddDomainWithProvisioning(int packageId, string domainName, DomainType domainType,
            bool createWebSite, int pointWebSiteId, int pointMailDomainId,
            bool createDnsZone, bool createPreviewDomain, bool allowSubDomains, string hostName)
        {

            int res = ServerController.AddDomainWithProvisioning(packageId, domainName, domainType,
                        createWebSite, pointWebSiteId, pointMailDomainId,
                        createDnsZone, createPreviewDomain, allowSubDomains, hostName);
            return res;
        }

        [WebMethod]
        public int UpdateDomain(DomainInfo domain)
        {
            return ServerController.UpdateDomain(domain);
        }

        [WebMethod]
        public int DeleteDomain(int domainId)
        {
            string domainname = "";
            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain != null)
                domainname = domain.DomainName;

            int res = ServerController.DeleteDomain(domainId);
            return res;
        }

        [WebMethod]
        public int DetachDomain(int domainId)
        {
            string domainname = "";
            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain != null)
                domainname = domain.DomainName;


            int res = ServerController.DetachDomain(domainId);
            return res;
        }

        [WebMethod]
        public int EnableDomainDns(int domainId)
        {
            return ServerController.EnableDomainDns(domainId);
        }

        [WebMethod]
        public int DisableDomainDns(int domainId)
        {
            return ServerController.DisableDomainDns(domainId);
        }

        [WebMethod]
        public int CreateDomainPreviewDomain(string hostName, int domainId)
        {
            return ServerController.CreateDomainPreviewDomain(hostName, domainId);
        }

        [WebMethod]
        public int DeleteDomainPreviewDomain(int domainId)
        {
            return ServerController.DeleteDomainPreviewDomain(domainId);
        }
        #endregion

        #region DNS Zones
        [WebMethod]
        public DnsRecord[] GetDnsZoneRecords(int domainId)
        {
            return ServerController.GetDnsZoneRecords(domainId);
        }

        [WebMethod]
        public DataSet GetRawDnsZoneRecords(int domainId)
        {
            return ServerController.GetRawDnsZoneRecords(domainId);
        }

        [WebMethod]
        public int AddDnsZoneRecord(int domainId, string recordName, DnsRecordType recordType,
            string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber, int RecordTTL)
        {
            return ServerController.AddDnsZoneRecord(domainId, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber, RecordTTL);
        }

        [WebMethod]
        public int UpdateDnsZoneRecord(int domainId,
            string originalRecordName, string originalRecordData,
            string recordName, DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber,int recordTTL)
        {
            return ServerController.UpdateDnsZoneRecord(domainId, originalRecordName, originalRecordData,
                recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber, recordTTL);
        }

        [WebMethod]
        public int DeleteDnsZoneRecord(int domainId, string recordName, DnsRecordType recordType, string recordData)
        {
            return ServerController.DeleteDnsZoneRecord(domainId, recordName, recordType, recordData);
        }
        #endregion

        #region Terminal Services Sessions
        [WebMethod]
        public TerminalSession[] GetTerminalServicesSessions(int serverId)
        {
            return OperatingSystemController.GetTerminalServicesSessions(serverId);
        }

        [WebMethod]
        public int CloseTerminalServicesSession(int serverId, int sessionId)
        {
            return OperatingSystemController.CloseTerminalServicesSession(serverId, sessionId);
        }
        #endregion

        #region Windows Processes
        [WebMethod]
        public WindowsProcess[] GetWindowsProcesses(int serverId)
        {
            return OperatingSystemController.GetWindowsProcesses(serverId);
        }

        [WebMethod]
        public int TerminateWindowsProcess(int serverId, int pid)
        {
            return OperatingSystemController.TerminateWindowsProcess(serverId, pid);
        }
        #endregion


        #region Web Platform Installer

        [WebMethod]
        public bool CheckLoadUserProfile(int serverId)
        {
            return OperatingSystemController.CheckLoadUserProfile(serverId);
        }

        [WebMethod]
        public void EnableLoadUserProfile(int serverId)
        {
            OperatingSystemController.EnableLoadUserProfile(serverId);
        }

        

        [WebMethod]
        public void InitWPIFeeds(int serverId)
        {
            var wpiSettings = SystemController.GetSystemSettings(SystemSettings.WPI_SETTINGS);
            

            List<string> feeds = new List<string>();
            
            // Microsoft feed
            string mainFeedUrl = wpiSettings[SystemSettings.WPI_MAIN_FEED_KEY];
            if (string.IsNullOrEmpty(mainFeedUrl))
            {
                mainFeedUrl = WebPlatformInstaller.MAIN_FEED_URL;
            }
            feeds.Add(mainFeedUrl);

            // Zoo Feed
            feeds.Add(WebPlatformInstaller.ZOO_FEED);


            // additional feeds
            string additionalFeeds = wpiSettings[SystemSettings.FEED_ULS_KEY];
            if (!string.IsNullOrEmpty(additionalFeeds))
            {
                feeds.AddRange(additionalFeeds.Split(';'));
            }

            OperatingSystemController.InitWPIFeeds(serverId, string.Join(";", feeds));

        }

        [WebMethod]
        public WPITab[] GetWPITabs(int serverId)
        {
            InitWPIFeeds(serverId);
            return OperatingSystemController.GetWPITabs(serverId);
        }

        [WebMethod]
        public WPIKeyword[] GetWPIKeywords(int serverId)
        {
            InitWPIFeeds(serverId);
            return OperatingSystemController.GetWPIKeywords(serverId);
        }

        [WebMethod]
        public WPIProduct[] GetWPIProducts(int serverId, string tabId, string keywordId)
        {
            InitWPIFeeds(serverId);
            return OperatingSystemController.GetWPIProducts(serverId, tabId, keywordId);
        }

        [WebMethod]
        public WPIProduct[] GetWPIProductsFiltered(int serverId, string keywordId)
        {
            InitWPIFeeds(serverId);
            return OperatingSystemController.GetWPIProductsFiltered(serverId, keywordId);
        }
        
        [WebMethod]
        public WPIProduct GetWPIProductById(int serverId, string productdId)
        {
            InitWPIFeeds(serverId);
            return OperatingSystemController.GetWPIProductById(serverId, productdId);
        }

        [WebMethod]
        public WPIProduct[] GetWPIProductsWithDependencies(int serverId, string[] products)
        {
            InitWPIFeeds(serverId);
            return OperatingSystemController.GetWPIProductsWithDependencies(serverId, products);
        }

        [WebMethod]
        public void InstallWPIProducts(int serverId, string[] products)
        {
            InitWPIFeeds(serverId);
            OperatingSystemController.InstallWPIProducts(serverId, products);
        }

        [WebMethod]
        public void CancelInstallWPIProducts(int serviceId)
        {
            OperatingSystemController.CancelInstallWPIProducts(serviceId);
        }


        [WebMethod]
        public string GetWPIStatus(int serverId)
        {
            return OperatingSystemController.GetWPIStatus(serverId);
        }

        [WebMethod]
        public string WpiGetLogFileDirectory(int serverId)
        {
            return OperatingSystemController.WpiGetLogFileDirectory(serverId);
        }

        [WebMethod]
        public SettingPair[] WpiGetLogsInDirectory(int serverId, string Path)
        {
            return OperatingSystemController.WpiGetLogsInDirectory(serverId, Path);
        }
        #endregion



        #region Windows Services
        [WebMethod]
        public WindowsService[] GetWindowsServices(int serverId)
        {
            return OperatingSystemController.GetWindowsServices(serverId);
        }

        [WebMethod]
        public int ChangeWindowsServiceStatus(int serverId, string id, WindowsServiceStatus status)
        {
            return OperatingSystemController.ChangeWindowsServiceStatus(serverId, id, status);
        }
        #endregion

        #region Event Viewer
        [WebMethod]
        public string[] GetLogNames(int serverId)
        {
            return OperatingSystemController.GetLogNames(serverId);
        }

        [WebMethod]
        public SystemLogEntry[] GetLogEntries(int serverId, string logName)
        {
            return OperatingSystemController.GetLogEntries(serverId, logName);
        }

        [WebMethod]
        public SystemLogEntriesPaged GetLogEntriesPaged(int serverId, string logName, int startRow, int maximumRows)
        {
            return OperatingSystemController.GetLogEntriesPaged(serverId, logName, startRow, maximumRows);
        }

        [WebMethod]
        public int ClearLog(int serverId, string logName)
        {
            return OperatingSystemController.ClearLog(serverId, logName);
        }
        #endregion

        #region Server Reboot
        [WebMethod]
        public int RebootSystem(int serverId)
        {
            return OperatingSystemController.RebootSystem(serverId);
        }
        #endregion

        #region OS informations
        [WebMethod]
        public Memory GetMemoryPackageId(int packageId)
        {
            return OperatingSystemController.GetMemoryPackageId(packageId);
        }

        [WebMethod]
        public Memory GetMemory(int serverId)
        {
            return OperatingSystemController.GetMemory(serverId);
        }
        #endregion

        #region Helper methods
        private string[] ConvertDictionaryToArray(StringDictionary settings)
        {
            List<string> r = new List<string>();
            foreach (string key in settings.Keys)
                r.Add(key + "=" + settings[key]);
            return r.ToArray();
        }

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }
        #endregion
    }
}

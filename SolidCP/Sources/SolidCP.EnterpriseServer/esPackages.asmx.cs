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
using Microsoft.Web.Services3;

using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using System.Collections.Specialized;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esPackages : System.Web.Services.WebService
    {
        #region Hosting Plans
        [WebMethod]
        public DataSet GetHostingPlans(int userId)
        {
            return PackageController.GetHostingPlans(userId);
        }

        [WebMethod]
        public DataSet GetHostingAddons(int userId)
        {
            return PackageController.GetHostingAddons(userId);
        }

        [WebMethod]
        public HostingPlanInfo GetHostingPlan(int planId)
        {
            return PackageController.GetHostingPlan(planId);
        }

        [WebMethod]
        public DataSet GetHostingPlanQuotas(int packageId, int planId, int serverId)
        {
            return PackageController.GetHostingPlanQuotas(packageId, planId, serverId);
        }

        [WebMethod]
        public HostingPlanContext GetHostingPlanContext(int planId)
        {
            return PackageController.GetHostingPlanContext(planId);
        }

        [WebMethod]
        public List<HostingPlanInfo> GetUserAvailableHostingPlans(int userId)
        {
            return PackageController.GetUserAvailableHostingPlans(userId);
        }

        [WebMethod]
        public List<HostingPlanInfo> GetUserAvailableHostingAddons(int userId)
        {
            return PackageController.GetUserAvailableHostingAddons(userId);
        }

        [WebMethod]
        public int AddHostingPlan(HostingPlanInfo plan)
        {
            return PackageController.AddHostingPlan(plan);
        }

        [WebMethod]
        public PackageResult UpdateHostingPlan(HostingPlanInfo plan)
        {
            return PackageController.UpdateHostingPlan(plan);
        }

        [WebMethod]
        public int DeleteHostingPlan(int planId)
        {
            return PackageController.DeleteHostingPlan(planId);
        }

        #endregion

        #region Packages
        [WebMethod]
        public List<PackageInfo> GetPackages(int userId)
        {
            return PackageController.GetPackages(userId);
        }

        [WebMethod]
        public DataSet GetNestedPackagesSummary(int packageId)
        {
            return PackageController.GetNestedPackagesSummary(packageId);
        }

        [WebMethod]
        public DataSet GetRawPackages(int userId)
        {
            return PackageController.GetRawPackages(userId);
        }

        [WebMethod]
        public DataSet SearchServiceItemsPaged(int userId, int itemTypeId, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.SearchServiceItemsPaged(userId, itemTypeId, filterValue,
                sortColumn, startRow, maximumRows);
        }

        //TODO START

        [WebMethod]
        public DataSet GetSearchObject(int userId, string filterColumn, string filterValue,
            int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType)
        {
            return PackageController.GetSearchObject(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows, colType, fullType, false);
        }

        [WebMethod]
        public DataSet GetSearchObjectQuickFind(int userId, string filterColumn, string filterValue,
            int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType)
        {
            return PackageController.GetSearchObject(userId, filterColumn, filterValue, statusId, roleId, sortColumn, 0, maximumRows, colType, fullType, true);
        }

        [WebMethod]
        public DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows, 
            bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID,
            string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID,
            string FilterColumns)
        {
            return PackageController.GetSearchTableByColumns(PagedStored, FilterValue, MaximumRows, 
                Recursive, PoolID, ServerID, StatusID, PlanID, OrgID, ItemTypeName, GroupName,
                PackageID, VPSType, RoleID, UserID, FilterColumns);
        }
        //TODO END

        [WebMethod]
        public DataSet GetPackagesPaged(int userId, string filterColumn, string filterValue,
                string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetPackagesPaged(userId,
                    filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetNestedPackagesPaged(int packageId, string filterColumn, string filterValue,
            int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetNestedPackagesPaged(packageId,
                filterColumn, filterValue, statusId, planId, serverId, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<PackageInfo> GetPackagePackages(int packageId)
        {
            return PackageController.GetPackagePackages(packageId, true);
        }

        [WebMethod]
        public List<PackageInfo> GetMyPackages(int userId)
        {
            return PackageController.GetMyPackages(userId);
        }

        [WebMethod]
        public DataSet GetRawMyPackages(int userId)
        {
            return PackageController.GetRawMyPackages(userId);
        }

        [WebMethod]
        public PackageInfo GetPackage(int packageId)
        {
            return PackageController.GetPackage(packageId);
        }

        [WebMethod]
        public PackageContext GetPackageContext(int packageId)
        {
            return PackageController.GetPackageContext(packageId);
        }

        [WebMethod]
        public DataSet GetPackageQuotas(int packageId)
        {
            return PackageController.GetPackageQuotas(packageId);
        }

        [WebMethod]
        public DataSet GetPackageQuotasForEdit(int packageId)
        {
            return PackageController.GetPackageQuotasForEdit(packageId);
        }

        [WebMethod]
        public PackageResult AddPackage(int userId, int planId, string packageName,
            string packageComments, int statusId, DateTime purchaseDate)
        {
            return PackageController.AddPackage(userId, planId, packageName, packageComments, statusId, purchaseDate, true);
        }

        [WebMethod]
        public PackageResult UpdatePackage(PackageInfo package)
        {
            return PackageController.UpdatePackage(package);
        }

        [WebMethod]
        public PackageResult UpdatePackageLiteral(
                    int packageId,
		            int statusId,
		            int planId,
		            DateTime purchaseDate,
		            string packageName,
		            string packageComments)
        {
            PackageInfo p = new PackageInfo();
            p.PackageId = packageId;
            p.StatusId = statusId;
            p.PlanId = planId;
            p.PurchaseDate = purchaseDate;
            p.PackageName = packageName;
            p.PackageComments = packageComments;
            return PackageController.UpdatePackage(p);
        }

        [WebMethod]
        public int UpdatePackageName(int packageId, string packageName, string packageComments)
        {
            return PackageController.UpdatePackageName(packageId, packageName, packageComments);
        }

        [WebMethod]
        public int DeletePackage(int packageId)
        {
            List<string> usersList = new List<string>();
            List<string> domainsList = new List<string>();

            PackageInfo package = PackageController.GetPackage(packageId);

            // get package service items
            List<ServiceProviderItem> items = PackageController.GetServiceItemsForStatistics(
                0, package.PackageId, false, false, false, true); // disposable items

            // order items by service
            Dictionary<int, List<ServiceProviderItem>> orderedItems =
                PackageController.OrderServiceItemsByServices(items);

            int maxItems = 100000000;
            bool mailFilterEnabled = false;
            // delete service items by service sets
            foreach (int serviceId in orderedItems.Keys)
            {

                ServiceInfo service = ServerController.GetServiceInfo(serviceId);
                //Delete Exchange Organization 
                if (service.ProviderId == 103 /*Organizations*/)
                {
                    int itemid = orderedItems[serviceId][0].Id;
                    OrganizationUsersPaged users = OrganizationController.GetOrganizationUsersPaged(itemid, null, null, null, 0, maxItems);
                    StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                    if (settings != null && Convert.ToBoolean(settings["EnableMailFilter"]))
                    {
                        mailFilterEnabled = true;
                        foreach (OrganizationUser user in users.PageUsers)
                            SpamExpertsController.DeleteEmailFilter(packageId, user.PrimaryEmailAddress);
                    }
                }
            }
            if (mailFilterEnabled)
            {
                List<DomainInfo> domains = ServerController.GetDomains(packageId);
                foreach (DomainInfo domain in domains)
                    SpamExpertsController.DeleteDomainFilter(domain);
            }

            //Get VPS Package IPs
            PackageIPAddress[] ips = ServerController.GetPackageIPAddresses(packageId, 0,
                                IPAddressPool.VpsExternalNetwork, "", "", "", 0, maxItems, true).Items;            
            List<int> ipsIdList = new List<int>();
            foreach (PackageIPAddress ip in ips)
                ipsIdList.Add(ip.AddressID);

            //Delete Package
            int res = PackageController.DeletePackage(packageId);

            if (res >= 0)
            {
                // users
                //foreach (string user in usersList)
                    //SEPlugin.SE.DeleteEmail(user);

                //domain
                //foreach (string domain in domainsList)
                    //SEPlugin.SE.DeleteDomain(domain);

                //return IPs back to ParentPackage
                if(package.ParentPackageId != 1) // 1 is System (serveradmin), we don't want assign IP to the serveradmin.
                    ServerController.AllocatePackageIPAddresses(package.ParentPackageId, ipsIdList.ToArray());
            }

            return res;
        }

        [WebMethod]
        public int ChangePackageStatus(int packageId, PackageStatus status)
        {
            return PackageController.ChangePackageStatus(packageId, status, true);
        }

        [WebMethod]
        public string EvaluateUserPackageTempate(int userId, int packageId, string template)
        {
            return PackageController.EvaluateUserPackageTempate(userId, packageId, template);
        }
        #endregion

        #region Package Settings
        [WebMethod]
        public PackageSettings GetPackageSettings(int packageId, string settingsName)
        {
            return PackageController.GetPackageSettings(packageId, settingsName);
        }

        [WebMethod]
        public int UpdatePackageSettings(PackageSettings settings)
        {
            return PackageController.UpdatePackageSettings(settings);
        }

        [WebMethod]
        public bool SetDefaultTopPackage(int userId, int packageId) {
            return PackageController.SetDefaultTopPackage(userId, packageId);
        }

        #endregion

        #region Package Add-ons
        [WebMethod]
        public DataSet GetPackageAddons(int packageId)
        {
            return PackageController.GetPackageAddons(packageId);
        }

        [WebMethod]
        public PackageAddonInfo GetPackageAddon(int packageAddonId)
        {
            return PackageController.GetPackageAddon(packageAddonId);
        }

        [WebMethod]
        public PackageResult AddPackageAddonById(int packageId, int addonPlanId, int quantity)
        {
            return PackageController.AddPackageAddonById(packageId, addonPlanId, quantity);
        }

        [WebMethod]
        public PackageResult AddPackageAddon(PackageAddonInfo addon)
        {
            return PackageController.AddPackageAddon(addon);
        }

        [WebMethod]
        public PackageResult AddPackageAddonLiteral(
		            int packageId,
		            int planId,
		            int quantity,
                    int statusId,
		            DateTime purchaseDate,
		            string comments)
        {
            PackageAddonInfo pa = new PackageAddonInfo();
            pa.PackageId = packageId;
            pa.PlanId = planId;
            pa.Quantity = quantity;
            pa.StatusId = statusId;
            pa.PurchaseDate = purchaseDate;
            pa.Comments = comments;
            return PackageController.AddPackageAddon(pa);
        }

        [WebMethod]
        public PackageResult UpdatePackageAddon(PackageAddonInfo addon)
        {
            return PackageController.UpdatePackageAddon(addon);
        }

        [WebMethod]
        public PackageResult UpdatePackageAddonLiteral(
                    int packageAddonId,
                    int planId,
                    int quantity,
                    int statusId,
                    DateTime purchaseDate,
                    string comments)
        {
            PackageAddonInfo pa = new PackageAddonInfo();
            pa.PackageAddonId = packageAddonId;
            pa.PlanId = planId;
            pa.Quantity = quantity;
            pa.StatusId = statusId;
            pa.PurchaseDate = purchaseDate;
            pa.Comments = comments;
            return PackageController.UpdatePackageAddon(pa);
        }

        [WebMethod]
        public int DeletePackageAddon(int packageAddonId)
        {
            return PackageController.DeletePackageAddon(packageAddonId);
        }
        #endregion

        #region Package Items
        [WebMethod]
        public DataSet GetSearchableServiceItemTypes()
        {
            return PackageController.GetSearchableServiceItemTypes();
        }

        [WebMethod]
        public DataSet GetRawPackageItemsByType(int packageId, string itemTypeName, bool recursive)
        {
            Type itemType = Type.GetType(itemTypeName);
            return PackageController.GetRawPackageItemsByType(packageId, itemType, recursive);
        }

        [WebMethod]
        public DataSet GetRawPackageItemsPaged(int packageId, string groupName, string typeName, int serverId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, groupName, Type.GetType(typeName), serverId, recursive,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetRawPackageItems(int packageId)
        {
            return PackageController.GetRawPackageItems(packageId);
        }

		[WebMethod]
		public int DetachPackageItem(int itemId)
		{
			return PackageController.DetachPackageItem(itemId);
		}

        [WebMethod]
        public int MovePackageItem(int itemId, int destinationServiceId)
        {
            return PackageController.MovePackageItem(itemId, destinationServiceId, false);
        }
        #endregion

        #region Quotas
        [WebMethod]
        public QuotaValueInfo GetPackageQuota(int packageId, string quotaName)
        {
            return PackageController.GetPackageQuota(packageId, quotaName);
        }
        #endregion

        #region Templates
        [WebMethod]
        public int SendAccountSummaryLetter(int userId, string to, string cc)
        {
            return PackageController.SendAccountSummaryLetter(userId, to, cc, false);
        }

        [WebMethod]
        public int SendPackageSummaryLetter(int packageId, string to, string cc)
        {
            return PackageController.SendPackageSummaryLetter(packageId, to, cc, false);
        }

        [WebMethod]
        public string GetEvaluatedPackageTemplateBody(int packageId)
        {
            return PackageController.GetEvaluatedPackageTemplateBody(packageId, false);
        }

        [WebMethod]
        public string GetEvaluatedAccountTemplateBody(int userId)
        {
            return PackageController.GetEvaluatedAccountTemplateBody(userId, false);
        }
        #endregion

        #region Wizards


        [WebMethod]
        public PackageResult AddPackageWithResources(int userId, int planId, string spaceName,
            int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite,
            bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName)
        {
            PackageResult res = PackageController.AddPackageWithResources(userId, planId, spaceName, statusId, sendLetter,
                createResources, domainName, tempDomain, createWebSite,
                createFtpAccount, ftpAccountName, createMailAccount, hostName);
            return res;
        }

        [WebMethod]
        public int CreateUserWizard(int parentPackageId, string username, string password,
                int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail,
                bool sendAccountLetter,
                bool createPackage, int planId, bool sendPackageLetter,
                string domainName, bool tempDomain, bool createWebSite,
                bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {
            return UserCreationWizard.CreateUserAccount(parentPackageId, username, password,
                roleId, firstName, lastName, email, secondaryEmail, htmlMail, sendAccountLetter,
                createPackage, planId,
                sendPackageLetter, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName,
                createMailAccount, hostName, createZoneRecord);
        }
        #endregion

        #region Reports
        [WebMethod]
        public DataSet GetPackagesBandwidthPaged(int userId, int packageId,
            DateTime startDate, DateTime endDate, string sortColumn,
            int startRow, int maximumRows)
        {
            return PackageController.GetPackagesBandwidthPaged(userId, packageId, startDate, endDate, sortColumn,
                startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetPackagesDiskspacePaged(int userId, int packageId, string sortColumn,
            int startRow, int maximumRows)
        {
            return PackageController.GetPackagesDiskspacePaged(userId, packageId, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetPackageBandwidth(int packageId, DateTime startDate, DateTime endDate)
        {
            return PackageController.GetPackageBandwidth(packageId, startDate, endDate);
        }

        [WebMethod]
        public DataSet GetPackageDiskspace(int packageId)
        {
            return PackageController.GetPackageDiskspace(packageId);
        }

		[WebMethod]
		public DataSet GetOverusageSummaryReport(int userId, int packageId, DateTime startDate, DateTime endDate)
		{
			return PackageController.GetOverusageSummaryReport(userId, packageId, startDate, endDate);
		}

		[WebMethod]
		public DataSet GetDiskspaceOverusageDetailsReport(int userId, int packageId)
		{
			return PackageController.GetDiskspaceOverusageDetailsReport(userId, packageId);
		}

		[WebMethod]
		public DataSet GetBandwidthOverusageDetailsReport(int userId, int packageId, DateTime startDate, DateTime endDate)
		{
			return PackageController.GetBandwidthDetailsReport(userId, packageId, startDate, endDate);
		}

        #endregion
    }
}

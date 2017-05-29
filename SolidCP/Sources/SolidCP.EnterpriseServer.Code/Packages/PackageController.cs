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
using System.IO;
using System.Web;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.Net.Mail;

using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.FTP;
using SolidCP.Providers.Mail;
using SolidCP.Providers.OS;
using OS = SolidCP.Providers.OS;
using Reports = SolidCP.EnterpriseServer.Base.Reports;
using System.Diagnostics;
using SolidCP.Templates;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for Packages.
    /// </summary>
    public class PackageController
    {
        #region Hosting Plans
        public static DataSet GetHostingPlans(int userId)
        {
            return DataProvider.GetHostingPlans(SecurityContext.User.UserId, userId);
        }

        public static DataSet GetHostingAddons(int userId)
        {
            return DataProvider.GetHostingAddons(SecurityContext.User.UserId, userId);
        }

        public static HostingPlanInfo GetHostingPlan(int planId)
        {
            return ObjectUtils.FillObjectFromDataReader<HostingPlanInfo>(
                DataProvider.GetHostingPlan(SecurityContext.User.UserId, planId));
        }

        public static DataSet GetHostingPlanQuotas(int packageId, int planId, int serverId)
        {
            return DataProvider.GetHostingPlanQuotas(SecurityContext.User.UserId, packageId,
                planId, serverId);
        }

        public static HostingPlanContext GetHostingPlanContext(int planId)
        {
            HostingPlanContext context = new HostingPlanContext();

            // load hosting plan
            context.HostingPlan = GetHostingPlan(planId);
            if (context.HostingPlan == null)
                return null;

            // load groups and quotas
            DataSet ds = GetHostingPlanQuotas(0, planId, 0);

            List<HostingPlanGroupInfo> groups = new List<HostingPlanGroupInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HostingPlanGroupInfo group = new HostingPlanGroupInfo();
                group.GroupId = (int)dr["GroupID"];
                group.GroupName = (string)dr["GroupName"];

                bool enabled = (bool)dr["Enabled"];
                group.Enabled = enabled;
                group.CalculateBandwidth = (bool)dr["CalculateBandwidth"];
                group.CalculateDiskSpace = (bool)dr["CalculateDiskSpace"];

                if (enabled)
                {
                    groups.Add(group);
                    context.Groups.Add(group.GroupName, group);
                }
            }

            List<QuotaValueInfo> quotas = new List<QuotaValueInfo>();
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                QuotaValueInfo quota = new QuotaValueInfo();
                quota.QuotaId = (int)dr["QuotaId"];
                quota.GroupId = (int)dr["GroupId"];
                quota.QuotaName = (string)dr["QuotaName"];
                quota.QuotaTypeId = (int)dr["QuotaTypeId"];
                quota.QuotaAllocatedValue = (int)dr["QuotaValue"];
                quotas.Add(quota);
                context.Quotas.Add(quota.QuotaName, quota);
            }

            context.GroupsArray = groups.ToArray();
            context.QuotasArray = quotas.ToArray();

            return context;
        }

        private static string BuildPlanQuotasXml(HostingPlanGroupInfo[] groups, HostingPlanQuotaInfo[] quotas)
        {
            // build xml
            /*
            XML Format:

            <plan>
                <groups>
	                <group id="16" enabled="1" calculateDiskSpace="1" calculateBandwidth="1"/>
                </groups>
                <quotas>
	                <quota id="2" value="2"/>
                </quotas>
            </plan>

            */

            if (groups == null || quotas == null)
                return null;

            XmlDocument doc = new XmlDocument();
            XmlElement nodePlan = doc.CreateElement("plan");

            XmlElement nodeGroups = doc.CreateElement("groups");
            nodePlan.AppendChild(nodeGroups);
            XmlElement nodeQuotas = doc.CreateElement("quotas");
            nodePlan.AppendChild(nodeQuotas);

            // groups
            if (groups != null)
            {
                foreach (HostingPlanGroupInfo group in groups)
                {
                    XmlElement nodeGroup = doc.CreateElement("group");
                    nodeGroups.AppendChild(nodeGroup);
                    nodeGroup.SetAttribute("id", group.GroupId.ToString());
                    nodeGroup.SetAttribute("enabled", group.Enabled ? "1" : "0");
                    nodeGroup.SetAttribute("calculateDiskSpace", group.CalculateDiskSpace ? "1" : "0");
                    nodeGroup.SetAttribute("calculateBandwidth", group.CalculateBandwidth ? "1" : "0");
                }
            }

            // quotas
            if (quotas != null)
            {
                foreach (HostingPlanQuotaInfo quota in quotas)
                {
                    XmlElement nodeQuota = doc.CreateElement("quota");
                    nodeQuotas.AppendChild(nodeQuota);
                    nodeQuota.SetAttribute("id", quota.QuotaId.ToString());
                    nodeQuota.SetAttribute("value", quota.QuotaValue.ToString());
                }
            }

            return nodePlan.OuterXml;
        }

        public static List<HostingPlanInfo> GetUserAvailableHostingPlans(int userId)
        {
            return ObjectUtils.CreateListFromDataSet<HostingPlanInfo>(
                DataProvider.GetUserAvailableHostingPlans(SecurityContext.User.UserId, userId));
        }

        public static List<HostingPlanInfo> GetUserAvailableHostingAddons(int userId)
        {
            return ObjectUtils.CreateListFromDataSet<HostingPlanInfo>(
                DataProvider.GetUserAvailableHostingAddons(SecurityContext.User.UserId, userId));
        }

        public static int AddHostingPlan(HostingPlanInfo plan)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsReseller);
            if (accountCheck < 0) return accountCheck;

            string quotasXml = BuildPlanQuotasXml(plan.Groups, plan.Quotas);

            return DataProvider.AddHostingPlan(SecurityContext.User.UserId, plan.UserId, plan.PackageId, plan.PlanName,
                plan.PlanDescription, plan.Available, plan.ServerId, plan.SetupPrice, plan.RecurringPrice,
                plan.RecurrenceUnit, plan.RecurrenceLength, plan.IsAddon, quotasXml);
        }

        public static PackageResult UpdateHostingPlan(HostingPlanInfo plan)
        {
            PackageResult result = new PackageResult();

            // check account
            result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsReseller);
            if (result.Result < 0) return result;

            string quotasXml = BuildPlanQuotasXml(plan.Groups, plan.Quotas);

            result.ExceedingQuotas = DataProvider.UpdateHostingPlan(SecurityContext.User.UserId,
                plan.PlanId, plan.PackageId, plan.ServerId, plan.PlanName,
                plan.PlanDescription, plan.Available, plan.SetupPrice, plan.RecurringPrice,
                plan.RecurrenceUnit, plan.RecurrenceLength, quotasXml);

            if (result.ExceedingQuotas.Tables[0].Rows.Count > 0)
                result.Result = BusinessErrorCodes.ERROR_PACKAGE_QUOTA_EXCEED;

            DataProvider.DistributePackageServices(SecurityContext.User.UserId, plan.PackageId);

            return result;
        }

        public static int DeleteHostingPlan(int planId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsReseller);
            if (accountCheck < 0) return accountCheck;

            int result = DataProvider.DeleteHostingPlan(SecurityContext.User.UserId, planId);
            if (result == -1)
                return BusinessErrorCodes.ERROR_HOSTING_PLAN_USED_IN_PACKAGE;
            else if (result == -2)
                return BusinessErrorCodes.ERROR_HOSTING_ADDON_USED_IN_PACKAGE;

            return 0;
        }

        #endregion

        #region Packages
        public static List<PackageInfo> GetMyPackages(int userId)
        {
            List<PackageInfo> packages = new List<PackageInfo>();
            ObjectUtils.FillCollectionFromDataSet<PackageInfo>(packages,
                GetRawMyPackages(userId));
            return packages;
        }

        public static List<PackageInfo> GetPackages(int userId)
        {
            List<PackageInfo> packages = new List<PackageInfo>();
            ObjectUtils.FillCollectionFromDataSet<PackageInfo>(
                packages, GetRawPackages(userId));
            return packages;
        }

        public static DataSet GetNestedPackagesSummary(int packageId)
        {
            return DataProvider.GetNestedPackagesSummary(SecurityContext.User.UserId, packageId);
        }

        public static List<PackageInfo> GetPackagePackages(int packageId, bool recursive)
        {
            List<PackageInfo> packages = new List<PackageInfo>();
            ObjectUtils.FillCollectionFromDataSet<PackageInfo>(
                packages, GetRawPackagePackages(packageId, recursive));
            return packages;
        }

        public static DataSet GetRawMyPackages(int userId)
        {
            return DataProvider.GetMyPackages(SecurityContext.User.UserId, userId);
        }

        public static DataSet GetRawPackages(int userId)
        {
            return DataProvider.GetPackages(SecurityContext.User.UserId, userId);
        }

        public static DataSet GetRawPackagePackages(int packageId, bool recursive)
        {
            return DataProvider.GetPackagePackages(SecurityContext.User.UserId, packageId, recursive);
        }

        public static DataSet GetPackagesPaged(int userId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return DataProvider.GetPackagesPaged(SecurityContext.User.UserId, userId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static DataSet GetNestedPackagesPaged(int packageId, string filterColumn, string filterValue,
            int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return DataProvider.GetNestedPackagesPaged(SecurityContext.User.UserId, packageId,
                filterColumn, filterValue, statusId, planId, serverId, sortColumn, startRow, maximumRows);
        }

        public static DataSet SearchServiceItemsPaged(int userId, int itemTypeId, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return DataProvider.SearchServiceItemsPaged(SecurityContext.User.UserId, userId, itemTypeId, filterValue,
                sortColumn, startRow, maximumRows);
        }

        //TODO START
        public static DataSet GetSearchObject(int userId, string filterColumn, string filterValue,
            int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, 
            string fullType, bool onlyFind)
        {
            return DataProvider.GetSearchObject(SecurityContext.User.UserId, userId,
                filterColumn, filterValue, statusId, roleId, sortColumn, startRow, 
                maximumRows, colType, fullType, false, onlyFind);
        }
        public static DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows,
            bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID,
            string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID,
            string FilterColumns)
        {
            return DataProvider.GetSearchTableByColumns(PagedStored, FilterValue, MaximumRows,
                Recursive, PoolID, ServerID, SecurityContext.User.UserId, StatusID, PlanID, OrgID, ItemTypeName, GroupName,
                PackageID, VPSType, RoleID, UserID, FilterColumns);
        }
        //TODO END

        public static DataSet GetPackageQuotas(int packageId)
        {
            return DataProvider.GetPackageQuotas(SecurityContext.User.UserId, packageId);
        }

        public static DataSet GetPackageQuotasForEdit(int packageId)
        {
            return DataProvider.GetPackageQuotasForEdit(SecurityContext.User.UserId, packageId);
        }

        public static PackageInfo GetPackage(int packageId)
        {
            return ObjectUtils.FillObjectFromDataReader<PackageInfo>(
                DataProvider.GetPackage(SecurityContext.User.UserId, packageId));
        }

        public static PackageContext GetPackageContext(int packageId)
        {
            PackageContext context = new PackageContext();

            // load package
            context.Package = GetPackage(packageId);
            if (context.Package == null)
                return null;

            // load groups and quotas
            DataSet ds = GetPackageQuotas(packageId);

            List<HostingPlanGroupInfo> groups = new List<HostingPlanGroupInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HostingPlanGroupInfo group = new HostingPlanGroupInfo();
                group.GroupId = (int)dr["GroupID"];
                group.GroupName = (string)dr["GroupName"];
                group.Enabled = true;
                group.CalculateBandwidth = (bool)dr["CalculateBandwidth"];
                group.CalculateDiskSpace = (bool)dr["CalculateDiskSpace"];
                groups.Add(group);
                context.Groups.Add(group.GroupName, group);
            }

            List<QuotaValueInfo> quotas = new List<QuotaValueInfo>();
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                QuotaValueInfo quota = new QuotaValueInfo();
                quota.QuotaId = (int)dr["QuotaId"];
                quota.GroupId = (int)dr["GroupId"];
                quota.QuotaName = (string)dr["QuotaName"];
                quota.QuotaDescription = ((object)dr["QuotaDescription"]).GetType() == typeof(System.DBNull) ? string.Empty : (string)dr["QuotaDescription"];
                quota.QuotaTypeId = (int)dr["QuotaTypeId"];
                quota.QuotaAllocatedValue = (int)dr["QuotaValue"];
                quota.QuotaAllocatedValuePerOrganization = (int)dr["QuotaValuePerOrganization"];
                quota.QuotaUsedValue = (int)dr["QuotaUsedValue"];
                quota.QuotaExhausted = (packageId < 2) || (quota.QuotaAllocatedValue != -1 && quota.QuotaUsedValue >= quota.QuotaAllocatedValue);
                quotas.Add(quota);
                context.Quotas.Add(quota.QuotaName, quota);
            }

            context.GroupsArray = groups.ToArray();
            context.QuotasArray = quotas.ToArray();

            return context;
        }

        public static UserInfo GetPackageOwner(int packageId)
        {
            PackageInfo package = GetPackage(packageId);
            if (package == null)
                return null;

            return UserController.GetUser(package.UserId);
        }

        public static PackageResult AddPackageWithResources(int userId, int planId, string spaceName,
            int statusId, bool sendLetter,
            bool createResources, string domainName, bool createInstantAlias, bool createWebSite,
            bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName)
        {
            try
            {
                TaskManager.StartTask("HOSTING_SPACE_WR", "ADD", spaceName);

                PackageResult result = new PackageResult();

                // check account
                result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                    | DemandAccount.IsResellerCSR);
                if (result.Result < 0) return result;

                // check if domain exists
                result.Result = ServerController.CheckDomain(domainName);
                if (result.Result < 0)
                    return result;

                // load user info
                UserInfoInternal user = UserController.GetUser(userId);

                if (createFtpAccount)
                {
                    // check if FTP account exists
                    if (String.IsNullOrEmpty(ftpAccountName))
                        ftpAccountName = user.Username;

                    if (FtpServerController.FtpAccountExists(ftpAccountName))
                    {
                        result.Result = BusinessErrorCodes.ERROR_ACCOUNT_WIZARD_FTP_ACCOUNT_EXISTS;
                        return result;
                    }
                }

                // load hosting plan
                HostingPlanInfo plan = PackageController.GetHostingPlan(planId);

                string packageName = spaceName;
                if (String.IsNullOrEmpty(packageName) || packageName.Trim() == "")
                    packageName = plan.PlanName;

                // create package
                int packageId = -1;
                try
                {
                    result = PackageController.AddPackage(
                        userId, planId, packageName, "", statusId, DateTime.Now, false);
                }
                catch (Exception ex)
                {
                    // error while adding package
                    throw ex;
                }

                if (result.Result < 0)
                    return result;

                packageId = result.Result;

                // create domain
                if (createResources)
                {
                    int domainId = 0;
                    if (!String.IsNullOrEmpty(domainName))
                    {
                        try
                        {
                            DomainInfo domain = new DomainInfo();
                            domain.PackageId = packageId;
                            domain.DomainName = domainName;
                            domain.HostingAllowed = false;
                            domainId = ServerController.AddDomain(domain, false, true);
                            if (domainId < 0)
                            {
                                result.Result = domainId;
                                DeletePackage(packageId);
                                return result;
                            }

                            domain = ServerController.GetDomain(domainId);
                            if (domain != null)
                            {
                                if (domain.ZoneItemId != 0)
                                {
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Os, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Dns, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Ftp, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2000, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2005, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2008, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2012, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2014, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2016, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MySql4, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MySql5, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MariaDB, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Statistics, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.VPS, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.VPS2012, domain, "");
                                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.VPSForPC, domain, "");
                                }
                            }

                            if (createInstantAlias)
                                ServerController.CreateDomainInstantAlias("", domainId);

                        }
                        catch (Exception ex)
                        {
                            // error while adding domain
                            DeletePackage(packageId);
                            throw new Exception("Could not add domain", ex);
                        }
                    }

                    if (createWebSite && !String.IsNullOrEmpty(domainName))
                    {
                        // create web site
                        try
                        {
                            int webSiteId = WebServerController.AddWebSite(packageId, hostName, domainId, 0, createInstantAlias, false);
                            if (webSiteId < 0)
                            {
                                result.Result = webSiteId;
                                DeletePackage(packageId);
                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            // error while creating web site
                            DeletePackage(packageId);
                            throw new Exception("Could not create web site", ex);
                        }
                    }

                    // create FTP account
                    if (createFtpAccount)
                    {
                        try
                        {
                            FtpAccount ftpAccount = new FtpAccount();
                            ftpAccount.PackageId = packageId;
                            ftpAccount.Name = ftpAccountName;
                            ftpAccount.Password = user.Password;
                            ftpAccount.Folder = "\\";
                            ftpAccount.CanRead = true;
                            ftpAccount.CanWrite = true;

                            int ftpAccountId = FtpServerController.AddFtpAccount(ftpAccount);
                            if (ftpAccountId < 0)
                            {
                                result.Result = ftpAccountId;
                                DeletePackage(packageId);
                                return result;
                            }
                        }
                        catch (Exception ex)
                        {
                            // error while creating ftp account
                            DeletePackage(packageId);
                            throw new Exception("Could not create FTP account", ex);
                        }
                    }

                    if (createMailAccount && !String.IsNullOrEmpty(domainName))
                    {
                        // create default mailbox
                        try
                        {
                            // load mail policy
                            UserSettings settings = UserController.GetUserSettings(userId, UserSettings.MAIL_POLICY);
                            string catchAllName = !String.IsNullOrEmpty(settings["CatchAllName"])
                                ? settings["CatchAllName"] : "mail";

                            MailAccount mailbox = new MailAccount();
                            mailbox.Name = catchAllName + "@" + domainName;
                            mailbox.PackageId = packageId;

                            // gather information from the form
                            mailbox.Enabled = true;

                            mailbox.ResponderEnabled = false;
                            mailbox.ReplyTo = "";
                            mailbox.ResponderSubject = "";
                            mailbox.ResponderMessage = "";

                            // password
                            mailbox.Password = user.Password;

                            // redirection
                            mailbox.ForwardingAddresses = new string[] { };
                            mailbox.DeleteOnForward = false;
                            mailbox.MaxMailboxSize = 0;

                            int mailAccountId = MailServerController.AddMailAccount(mailbox);

                            if (mailAccountId < 0)
                            {
                                result.Result = mailAccountId;
                                DeletePackage(packageId);
                                return result;
                            }

                            // set catch-all account
                            MailDomain mailDomain = MailServerController.GetMailDomain(packageId, domainName);
                            mailDomain.CatchAllAccount = catchAllName;
                            mailDomain.PostmasterAccount = "mail";
                            mailDomain.AbuseAccount = "mail";
                            MailServerController.UpdateMailDomain(mailDomain);

                            int mailDomainId = mailDomain.Id;

                            // set mail domain pointer
                            // load domain instant alias
                            string instantAlias = ServerController.GetDomainAlias(packageId, domainName);
                            DomainInfo instantDomain = ServerController.GetDomain(instantAlias);
                            if (instantDomain == null || instantDomain.MailDomainId > 0)
                                instantAlias = "";

                            if (!String.IsNullOrEmpty(instantAlias))
                                MailServerController.AddMailDomainPointer(mailDomainId, instantDomain.DomainId);
                        }
                        catch (Exception ex)
                        {
                            // error while creating mail account
                            DeletePackage(packageId);
                            throw new Exception("Could not create mail account", ex);
                        }
                    }
                }

                BackgroundTask topTask = TaskManager.TopTask;

                topTask.ItemId = result.Result;
                topTask.UpdateParamValue("SendLetter", sendLetter);

                TaskController.UpdateTaskWithParams(topTask);

                return result;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        /// <summary>
        /// Adds hosting package(space) to the user.
        /// </summary>
        /// <remarks>Added as a part of #1084 item fix.</remarks>
        /// <param name="userId">User id</param>
        /// <param name="planId">Hosting plan id</param>
        /// <param name="packageName">Hosting package(space) name</param>
        /// <param name="packageComments">Hosting package(space) comments</param>
        /// <param name="statusId">Hosting package(space) initial status</param>
        /// <param name="purchaseDate">Date purchased</param>
        /// <param name="sendLetter">Inidicates whether to send e-mail notification or not</param>
        /// <returns>Hosting package(space) creation result</returns>
        public static PackageResult AddPackage(int userId, int planId, string packageName,
            string packageComments, int statusId, DateTime purchaseDate, bool sendLetter)
        {
            return AddPackage(userId, planId, packageName, packageComments, statusId, purchaseDate,
                sendLetter, false);
        }

        /// <summary>
        /// Adds hosting package(space) to the user.
        /// </summary>
        /// <remarks>Modified as a fix for #1084 item.</remarks>
        /// <param name="userId">User id</param>
        /// <param name="planId">Hosting plan id</param>
        /// <param name="packageName">Hosting package(space) name</param>
        /// <param name="packageComments">Hosting package(space) comments</param>
        /// <param name="statusId">Hosting package(space) initial status</param>
        /// <param name="purchaseDate">Date purchased</param>
        /// <param name="sendLetter">Inidicates whether to send e-mail notification or not</param>
        /// <param name="signup">Used for external clients to set #Signup# variable in e-mail notification template</param>
        /// <returns>Hosting package(space) creation result</returns>
        public static PackageResult AddPackage(int userId, int planId, string packageName,
            string packageComments, int statusId, DateTime purchaseDate, bool sendLetter, bool signup)
        {
            TaskManager.StartTask("HOSTING_SPACE", "ADD", packageName);

            PackageResult result = new PackageResult();

            try
            {
                // check account
                result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                    | DemandAccount.IsResellerCSR);
                if (result.Result < 0) return result;

                int packageId = -1;

                result.ExceedingQuotas = DataProvider.AddPackage(SecurityContext.User.UserId, out packageId,
                    userId, planId, packageName, packageComments, statusId, purchaseDate);

                if (result.ExceedingQuotas.Tables[0].Rows.Count > 0)
                {
                    result.Result = BusinessErrorCodes.ERROR_PACKAGE_QUOTA_EXCEED;
                    return result;
                }

                result.Result = packageId;

                // allocate "Web Sites" IP addresses
                int webServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
                StringDictionary webSettings = ServerController.GetServiceSettings(webServiceId);
                if (Utils.ParseBool(webSettings["AutoAssignDedicatedIP"], true))
                    ServerController.AllocateMaximumPackageIPAddresses(packageId, ResourceGroups.Web, IPAddressPool.WebSites);

                // allocate "VPS" IP addresses
                int vpsServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPS);
                StringDictionary vpsSettings = ServerController.GetServiceSettings(vpsServiceId);
                if (Utils.ParseBool(vpsSettings["AutoAssignExternalIP"], true))
                    ServerController.AllocateMaximumPackageIPAddresses(packageId, ResourceGroups.VPS, IPAddressPool.VpsExternalNetwork);

                // allocate "VPS" IP addresses
                int vps2012ServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPS2012);
                StringDictionary vps2012Settings = ServerController.GetServiceSettings(vps2012ServiceId);
                if (Utils.ParseBool(vps2012Settings["AutoAssignExternalIP"], true))
                    ServerController.AllocateMaximumPackageIPAddresses(packageId, ResourceGroups.VPS2012, IPAddressPool.VpsExternalNetwork);

                // allocate "VPSForPC" IP addresses
                int vpsfcpServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.VPSForPC);
                StringDictionary vpfcpsSettings = ServerController.GetServiceSettings(vpsfcpServiceId);
                if (Utils.ParseBool(vpfcpsSettings["AutoAssignExternalIP"], true))
                    ServerController.AllocateMaximumPackageIPAddresses(packageId, ResourceGroups.VPSForPC, IPAddressPool.VpsExternalNetwork);

                // load hosting plan
                HostingPlanInfo plan = GetHostingPlan(planId);

                // create home folder
                int homeId = CreatePackageHome(plan.PackageId, packageId, userId);
                if (homeId < 0)
                    result.Result = homeId;

                BackgroundTask topTask = TaskManager.TopTask;

                topTask.ItemId = result.Result;
                topTask.UpdateParamValue("Signup", signup);
                topTask.UpdateParamValue("UserId", userId);
                topTask.UpdateParamValue("SendLetter", sendLetter);

                TaskController.UpdateTaskWithParams(topTask);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return result;
        }

        public static PackageResult UpdatePackage(PackageInfo package)
        {
            TaskManager.StartTask("HOSTING_SPACE", "UPDATE", package.PackageName);

            PackageResult result = new PackageResult();

            try
            {
                // check account
                result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                    | DemandAccount.IsReseller);

                if (result.Result < 0)
                    result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                    | DemandAccount.IsPlatformCSR);

                if (result.Result < 0)
                    result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                    | DemandAccount.IsResellerCSR);

                if (result.Result < 0) return result;

                // load package
                PackageInfo originalPackage = GetPackage(package.PackageId);
                if (originalPackage == null)
                {
                    result.Result = BusinessErrorCodes.ERROR_PACKAGE_NOT_FOUND;
                    return result;
                }

                string quotasXml = BuildPlanQuotasXml(package.Groups, package.Quotas);

                // update package
                result.ExceedingQuotas = DataProvider.UpdatePackage(SecurityContext.User.UserId,
                    package.PackageId, package.PlanId, package.PackageName, package.PackageComments, package.StatusId,
                    package.PurchaseDate, package.OverrideQuotas, quotasXml, package.DefaultTopPackage);

                if (result.ExceedingQuotas.Tables[0].Rows.Count > 0)
                    result.Result = BusinessErrorCodes.ERROR_PACKAGE_QUOTA_EXCEED;

                // Update the Hard quota on home folder in case it was enabled and in case there was a change in disk space
                UpdatePackageHardQuota(package.PackageId);

                DataProvider.DistributePackageServices(SecurityContext.User.UserId, package.PackageId);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return result;
        }

        public static int UpdatePackageName(int packageId, string packageName,
            string packageComments)
        {
            int result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (result < 0) return result;

            DataProvider.UpdatePackageName(SecurityContext.User.UserId, packageId, packageName, packageComments);

            return 0;
        }

        public static int DeletePackage(int packageId)
        {
            return DeletePackage(null, packageId);
        }

        public static int DeletePackage(string taskId, int packageId)
        {
            TaskManager.StartTask(taskId, "HOSTING_SPACE", "DELETE", packageId);

            try
            {
                // check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                    | DemandAccount.IsReseller);
                if (accountCheck < 0) return accountCheck;

                // check if package has child packages
                if (GetPackagePackages(packageId, true).Count > 0)
                    return BusinessErrorCodes.ERROR_PACKAGE_HAS_PACKAGES;

                // load "root" package
                PackageInfo package = GetPackage(packageId);
                if (package == null)
                    return BusinessErrorCodes.ERROR_PACKAGE_NOT_FOUND;

                // add "root" package
                List<PackageInfo> packages = new List<PackageInfo>();
                packages.Add(package);

                // delete packages
                DeletePackages(packages);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return 0;
        }

        public static int DeletePackages(List<PackageInfo> packages)
        {
            // delete packages asynchronously
            PackageAsyncWorker packageWorker = new PackageAsyncWorker();
            packageWorker.UserId = SecurityContext.User.UserId;
            packageWorker.Packages = packages;

            // invoke worker
            packageWorker.DeletePackagesServiceItemsAsync();

            return 0;
        }

        public static int ChangePackageStatus(int packageId, PackageStatus status, bool async)
        {
            return ChangePackageStatus(null, packageId, status, async);
        }

        public static int ChangePackageStatus(string taskId, int packageId, PackageStatus status, bool async)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsResellerCSR);
            if (accountCheck < 0) return accountCheck;

            List<PackageInfo> packages = new List<PackageInfo>();

            // load package
            PackageInfo package = GetPackage(packageId);
            if (package == null)
                return -1;

            // add "root" package
            packages.Add(package);

            // add "child" packages
            packages.AddRange(GetPackagePackages(packageId, true));

            // change packages status
            ChangePackagesStatus(packages, status, async);

            return 0;
        }

        public static int ChangePackagesStatus(List<PackageInfo> packages, PackageStatus status, bool async)
        {
            int statusId = (int)status;

            List<PackageInfo> changedPackages = new List<PackageInfo>();
            foreach (PackageInfo p in packages)
            {
                // get package details
                PackageInfo package = GetPackage(p.PackageId);
                if (package != null && package.StatusId != statusId)
                {
                    bool currEnabled = (package.StatusId == (int)PackageStatus.Active);
                    bool enabled = (statusId == (int)PackageStatus.Active);

                    // change package status
                    package.StatusId = statusId;

                    // save package
                    UpdatePackage(package);

                    // add to the list of affected packages
                    if (currEnabled != enabled)
                        changedPackages.Add(package);
                }
            }

            // change service items state asynchronously
            PackageAsyncWorker packageWorker = new PackageAsyncWorker();
            packageWorker.UserId = SecurityContext.User.UserId;
            packageWorker.Packages = changedPackages;
            packageWorker.ItemsStatus = status;

            // invoke worker
            if (async)
                packageWorker.ChangePackagesServiceItemsStateAsync();
            else
                packageWorker.ChangePackagesServiceItemsState();

            return 0;
        }

        private static int CreatePackageHome(int resellerPackageId, int packageId, int userId)
        {
            // request OS service
            int osId = GetPackageServiceId(packageId, ResourceGroups.Os);
            if (osId == 0)
                return 0;
            //return BusinessErrorCodes.ERROR_OS_RESOURCE_UNAVAILABLE;

            // load user details
            UserInfo user = UserController.GetUser(userId);

            // load package
            string initialPath = null;

            // load package settings
            PackageSettings packageSettings = PackageController.GetPackageSettings(packageId,
                PackageSettings.SPACES_FOLDER);

            if (!String.IsNullOrEmpty(packageSettings["ChildSpacesFolder"]))
            {
                initialPath = Path.Combine(
                    FilesController.GetFullPackagePath(resellerPackageId, packageSettings["ChildSpacesFolder"]),
                    user.Username);

            }
            else
            {
                // load service settings
                StringDictionary osSesstings = ServerController.GetServiceSettings(osId);

                // build initial path
                string usersHome = osSesstings["UsersHome"];
                if (!usersHome.EndsWith("\\"))
                    usersHome += '\\';

                initialPath = Path.Combine(usersHome, user.Username);
            }

            OS.OperatingSystem os = new OS.OperatingSystem();
            ServiceProviderProxy.Init(os, osId);
            string path = os.CreatePackageFolder(initialPath);

            // store home folder info
            HomeFolder homeFolder = new HomeFolder();
            homeFolder.ServiceId = osId;
            homeFolder.PackageId = packageId;
            homeFolder.Name = path;

            int res = AddPackageItem(homeFolder);

            // Added By Haya
            UpdatePackageHardQuota(packageId);

            // save package item
            return res;
        }

        public static DateTime GetPackageBandwidthUpdate(int packageId)
        {
            return DataProvider.GetPackageBandwidthUpdate(packageId);
        }

        public static void UpdatePackageBandwidthUpdate(int packageId, DateTime updateDate)
        {
            DataProvider.UpdatePackageBandwidthUpdate(packageId, updateDate);
        }

        // This gets the system quota and updates the home folder with the value
        public static void UpdatePackageHardQuota(int packageId)
        {
            // request OS service
            int osId = GetPackageServiceId(packageId, ResourceGroups.Os);
            if (osId == 0)
                return;

            OS.OperatingSystem os = new OS.OperatingSystem();
            ServiceProviderProxy.Init(os, osId);

            //Get operating system settings
            StringDictionary osSesstings = ServerController.GetServiceSettings(osId);
            bool diskQuotaEnabled = (osSesstings["EnableHardQuota"] != null) ? bool.Parse(osSesstings["EnableHardQuota"]) : false;
            string driveName = osSesstings["LocationDrive"];

            if (!diskQuotaEnabled)
                return;

            string homeFolder = FilesController.GetHomeFolder(packageId);
            FilesController.SetFolderQuota(packageId, homeFolder, driveName, Quotas.OS_DISKSPACE);

        }

        //public static void UpdateESHardQuota(int packageId)
        //{
        //    int esServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);

        //    if (esServiceId != 0)
        //    {

        //        StringDictionary esSesstings = ServerController.GetServiceSettings(esServiceId);

        //        string usersHome = esSesstings["UsersHome"];
        //        string usersDomain = esSesstings["UsersDomain"];
        //        string locationDrive = esSesstings["LocationDrive"];

        //        string homePath = string.Format("{0}:\\{1}", locationDrive, usersHome);

        //        int osId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Os);
        //        bool enableHardQuota = (esSesstings["enablehardquota"] != null)
        //            ? bool.Parse(esSesstings["enablehardquota"])
        //            : false;

        //        if (enableHardQuota && osId != 0 && OperatingSystemController.CheckFileServicesInstallation(osId))
        //        {
        //            FilesController.SetFolderQuota(packageId, usersHome, locationDrive, Quotas.ENTERPRISESTORAGE_DISKSTORAGESPACE);
        //        }
        //    }
        //}

        #endregion

        #region Package Add-ons
        public static DataSet GetPackageAddons(int packageId)
        {
            return DataProvider.GetPackageAddons(SecurityContext.User.UserId, packageId);
        }

        public static PackageAddonInfo GetPackageAddon(int packageAddonId)
        {
            return ObjectUtils.FillObjectFromDataReader<PackageAddonInfo>(
                DataProvider.GetPackageAddon(SecurityContext.User.UserId, packageAddonId));
        }

        public static PackageResult AddPackageAddonById(int packageId, int addonPlanId, int quantity)
        {
            PackageAddonInfo addon = new PackageAddonInfo();
            addon.PackageId = packageId;
            addon.PlanId = addonPlanId;
            addon.Quantity = quantity;
            addon.PurchaseDate = DateTime.Now;
            addon.StatusId = 1; // active
            addon.Comments = "";

            return AddPackageAddon(addon);
        }

        public static PackageResult AddPackageAddon(PackageAddonInfo addon)
        {
            PackageResult result = new PackageResult();

            // check account
            result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsReseller);

            if (result.Result < 0)
                result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsPlatformCSR);

            if (result.Result < 0)
                result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsResellerCSR);

            if (result.Result < 0) return result;

            int addonId = 0;

            result.ExceedingQuotas = DataProvider.AddPackageAddon(SecurityContext.User.UserId, out addonId, addon.PackageId,
                addon.PlanId, addon.Quantity, addon.StatusId, addon.PurchaseDate, addon.Comments);

            if (result.ExceedingQuotas.Tables[0].Rows.Count > 0)
            {
                result.Result = BusinessErrorCodes.ERROR_PACKAGE_QUOTA_EXCEED;
                return result;
            }

            result.Result = addonId;

            // Update the Hard quota on home folder in case it was enabled and in case there was a change in disk space
            UpdatePackageHardQuota(addon.PackageId);
            return result;
        }

        public static PackageResult UpdatePackageAddon(PackageAddonInfo addon)
        {
            PackageResult result = new PackageResult();

            // check account
            result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsReseller);

            if (result.Result < 0)
                result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsPlatformCSR);

            if (result.Result < 0)
                result.Result = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsResellerCSR);

            if (result.Result < 0) return result;

            result.ExceedingQuotas = DataProvider.UpdatePackageAddon(SecurityContext.User.UserId, addon.PackageAddonId,
                addon.PlanId, addon.Quantity, addon.StatusId, addon.PurchaseDate, addon.Comments);

            if (result.ExceedingQuotas.Tables[0].Rows.Count > 0)
                result.Result = BusinessErrorCodes.ERROR_PACKAGE_QUOTA_EXCEED;

            // Update the Hard quota on home folder in case it was enabled and in case there was a change in disk space
            UpdatePackageHardQuota(addon.PackageId);
            return result;
        }

        public static int DeletePackageAddon(int packageAddonId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsReseller);
            if (accountCheck < 0) return accountCheck;

            var packageId = GetPackageAddon(packageAddonId).PackageId;

            DataProvider.DeletePackageAddon(SecurityContext.User.UserId, packageAddonId);

            // Update the Hard quota on home folder in case it was enabled and in case there was a change in disk space
            UpdatePackageHardQuota(packageId);

            return 0;
        }

        #endregion

        #region Package Items
        public static DataSet GetSearchableServiceItemTypes()
        {
            return DataProvider.GetSearchableServiceItemTypes();
        }

        public static int GetPackageServiceId(int packageId, string groupName)
        {
            return DataProvider.GetPackageServiceId(SecurityContext.User.UserId, packageId, groupName);
        }

        public static List<ServiceProviderItem> GetPackageItemsByName(int packageId, string itemName)
        {
            DataSet dsItems = DataProvider.GetServiceItemsByName(
                SecurityContext.User.UserId, packageId, itemName);

            return CreateServiceItemsList(dsItems, 0);
        }

        public static List<ServiceProviderItem> GetPackageItemsByType(int packageId, string groupName, Type itemType)
        {
            return GetPackageItemsByType(packageId, groupName, itemType, false);
        }

        public static List<ServiceProviderItem> GetPackageItemsByType(int packageId, Type itemType)
        {
            return GetPackageItemsByType(packageId, null, itemType, false);
        }

        public static List<ServiceProviderItem> GetPackageItemsByType(int packageId, Type itemType, bool recursive)
        {
            return GetPackageItemsByType(packageId, null, itemType, recursive);
        }

        public static List<ServiceProviderItem> GetPackageItemsByType(int packageId, string groupName, Type itemType, bool recursive)
        {
            string typeName = ObjectUtils.GetTypeFullName(itemType);
            DataSet dsItems = DataProvider.GetServiceItems(SecurityContext.User.UserId,
                packageId, groupName, typeName, recursive);

            return CreateServiceItemsList(dsItems, 0);
        }

        public static List<ServiceProviderItem> GetPackageItemsByTypeInternal(int packageId, string groupName, Type itemType, bool recursive)
        {
            string typeName = ObjectUtils.GetTypeFullName(itemType);
            DataSet dsItems = DataProvider.GetServiceItems(-1, packageId, groupName, typeName, recursive);

            return CreateServiceItemsList(dsItems, 0);
        }

        public static DataSet GetRawPackageItemsByType(int packageId, Type itemType, bool recursive)
        {
            return GetRawPackageItemsByType(packageId, null, itemType, recursive);
        }

        public static DataSet GetRawPackageItemsByType(int packageId, string groupName, Type itemType, bool recursive)
        {
            string typeName = ObjectUtils.GetTypeFullName(itemType);
            DataSet dsItems = DataProvider.GetServiceItems(SecurityContext.User.UserId,
                packageId, groupName, typeName, recursive);

            FlatternItemsTable(dsItems, 0, itemType);

            return dsItems;
        }

        public static ServiceItemsPaged GetPackageItemsPaged(int packageId, Type itemType,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetPackageItemsPaged(packageId, null, itemType, false, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        public static ServiceItemsPaged GetPackageItemsPaged(int packageId, Type itemType, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetPackageItemsPaged(packageId, null, itemType, recursive, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        public static DataSet GetRawPackageItemsPaged(int packageId, Type itemType, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetRawPackageItemsPaged(packageId, null, itemType, recursive, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        public static ServiceItemsPaged GetPackageItemsPaged(int packageId, string groupName, Type itemType,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetPackageItemsPaged(packageId, groupName, itemType, false, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        public static ServiceItemsPaged GetPackageItemsPaged(int packageId, string groupName, Type itemType, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            string typeName = ObjectUtils.GetTypeFullName(itemType);
            DataSet dsItems = DataProvider.GetServiceItemsPaged(
                SecurityContext.User.UserId, packageId, groupName, typeName, 0, recursive, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);

            ServiceItemsPaged result = new ServiceItemsPaged();
            result.RecordsCount = (int)dsItems.Tables[0].Rows[0][0];
            result.PageItems = CreateServiceItemsList(dsItems, 1).ToArray();
            return result;
        }

        public static DataSet GetRawPackageItemsPaged(int packageId, string groupName, Type itemType,
            bool recursive, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {
            return GetRawPackageItemsPaged(packageId, groupName, itemType, 0, recursive, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        public static DataSet GetRawPackageItemsPaged(int packageId, string groupName, Type itemType,
            int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {
            string typeName = ObjectUtils.GetTypeFullName(itemType);
            DataSet dsItems = DataProvider.GetServiceItemsPaged(
                SecurityContext.User.UserId, packageId, groupName, typeName, serverId, recursive, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);

            FlatternItemsTable(dsItems, 1, itemType);

            return dsItems;
        }

        public static int GetServiceItemsCount(Type itemType, string groupName)
        {
            return GetServiceItemsCount(itemType, groupName, 0);
        }

        public static int GetServiceItemsCount(Type itemType, string groupName, int serviceId)
        {
            string typeName = ObjectUtils.GetTypeFullName(itemType);
            return DataProvider.GetServiceItemsCount(typeName, groupName, serviceId);
        }

        public static DataSet GetPackageItemsDataSet(int packageId)
        {
            return DataProvider.GetServiceItemsByPackage(SecurityContext.User.UserId,
                packageId);
        }

        public static List<ServiceProviderItem> GetPackageItems(int packageId)
        {
            DataSet dsItems = GetPackageItemsDataSet(packageId);
            return CreateServiceItemsList(dsItems, 0);
        }

        public static DataSet GetRawPackageItems(int packageId)
        {
            return DataProvider.GetServiceItemsByPackage(SecurityContext.User.UserId,
                packageId);
        }

        public static DataSet GetServiceItemsDataSet(int serviceId)
        {
            return DataProvider.GetServiceItemsByService(SecurityContext.User.UserId,
                serviceId);
        }

        public static List<ServiceProviderItem> GetServiceItems(int serviceId)
        {
            DataSet dsItems = GetServiceItemsDataSet(serviceId);
            return CreateServiceItemsList(dsItems, 0);
        }

        public static List<ServiceProviderItemType> GetServiceItemTypes()
        {
            return ObjectUtils.CreateListFromDataReader<ServiceProviderItemType>(
                DataProvider.GetServiceItemTypes());
        }

        public static ServiceProviderItemType GetServiceItemType(int itemTypeId)
        {
            return ObjectUtils.FillObjectFromDataReader<ServiceProviderItemType>(
                DataProvider.GetServiceItemType(itemTypeId));
        }

        public static List<ServiceProviderItem> GetServiceItemsForStatistics(int serviceId, int packageId,
            bool calculateDiskspace, bool calculateBandwidth, bool suspendable, bool disposable)
        {
            DataSet dsItems = DataProvider.GetServiceItemsForStatistics(SecurityContext.User.UserId,
                serviceId, packageId, calculateDiskspace, calculateBandwidth, suspendable, disposable);
            return CreateServiceItemsList(dsItems, 0);
        }

        public static ServiceProviderItem GetPackageItem(int itemId)
        {
            DataSet dsItem = DataProvider.GetServiceItem(SecurityContext.User.UserId, itemId);
            DataView dvItem = dsItem.Tables[0].DefaultView;
            if (dvItem.Count == 0)
                return null;

            return CreateServiceItem(dvItem[0], dsItem.Tables[1].DefaultView);
        }

        public static ServiceProviderItem GetPackageItemByName(int packageId, string itemName, Type itemType)
        {
            return GetPackageItemByName(packageId, null, itemName, itemType);
        }

        public static ServiceProviderItem GetPackageItemByName(int packageId, string groupName, string itemName, Type itemType)
        {
            string itemTypeName = ObjectUtils.GetTypeFullName(itemType);
            DataSet dsItem = DataProvider.GetServiceItemByName(SecurityContext.User.UserId,
                packageId, groupName, itemName, itemTypeName);
            DataView dvItem = dsItem.Tables[0].DefaultView;
            if (dvItem.Count == 0)
                return null;

            return CreateServiceItem(dvItem[0], dsItem.Tables[1].DefaultView);
        }

        public static int GetServiceItemsCountByNameAndServiceId(int serviceId, string groupName, string itemName, Type itemType)
        {
            string itemTypeName = ObjectUtils.GetTypeFullName(itemType);

            return DataProvider.GetServiceItemsCountByNameAndServiceId(SecurityContext.User.UserId,
                serviceId, groupName, itemName, itemTypeName);
        }

        public static bool CheckServiceItemExists(string itemName, Type itemType)
        {
            return CheckServiceItemExists(itemName, null, itemType);
        }

        public static bool CheckServiceItemExists(string itemName, string groupName, Type itemType)
        {
            string itemTypeName = ObjectUtils.GetTypeFullName(itemType);
            return DataProvider.CheckServiceItemExists(itemName, groupName, itemTypeName);
        }

        public static bool CheckServiceItemExists(int serviceId, string itemName, Type itemType)
        {
            string itemTypeName = ObjectUtils.GetTypeFullName(itemType);
            return DataProvider.CheckServiceItemExists(serviceId, itemName, itemTypeName);
        }

        public static int AddPackageItem(ServiceProviderItem item)
        {
            // check parameters
            if (item.PackageId < 0)
                throw new ArgumentException("item.PackageId property value should be specified!");

            if (item.ServiceId < 0)
                throw new ArgumentException("item.ServiceId property value should be specified!");

            string itemTypeName = ObjectUtils.GetTypeFullName(item.GetType());

            // build properties xml
            string xml = BuildPropertiesXml(ObjectUtils.GetObjectProperties(item, true));

            // add item itself
            int itemId = DataProvider.AddServiceItem(SecurityContext.User.UserId,
                item.ServiceId, item.PackageId, item.Name,
                itemTypeName, xml);

            return itemId;
        }

        public static int UpdatePackageItem(ServiceProviderItem item)
        {
            // build properties xml
            string xml = BuildPropertiesXml(ObjectUtils.GetObjectProperties(item, true));

            // update item
            DataProvider.UpdateServiceItem(SecurityContext.User.UserId, item.Id, item.Name, xml);

            return 0;
        }

        private static string BuildPropertiesXml(Hashtable props)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement nodeProps = doc.CreateElement("properties");
            foreach (string propertyName in props.Keys)
            {
                XmlElement nodeProp = doc.CreateElement("property");
                nodeProp.SetAttribute("name", propertyName);
                nodeProp.SetAttribute("value", props[propertyName].ToString());
                nodeProps.AppendChild(nodeProp);
            }
            return nodeProps.OuterXml;
        }

        public static int DetachPackageItem(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // HACK: Mail accounts detach is hard coded.
            MailDomain mailDomain = GetPackageItem(itemId) as MailDomain;
            if (mailDomain != null)
            {
                // Load accounts and detach them as well.
                List<ServiceProviderItem> accounts = GetPackageItemsByName(mailDomain.PackageId, "%@" + mailDomain.Name);
                foreach (ServiceProviderItem account in accounts)
                {
                    DetachPackageItem(account.Id);
                }
            }

            DeletePackageItem(itemId);

            return 0;
        }

        public static int MovePackageItem(int itemId, int destinationServiceId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // move item
            DataProvider.MoveServiceItem(SecurityContext.User.UserId, itemId, destinationServiceId);

            return 0;
        }

        public static int DeletePackageItem(int itemId)
        {
            // delete item
            DataProvider.DeleteServiceItem(SecurityContext.User.UserId, itemId);

            return 0;
        }

        public static int UpdatePackageBandwidth(int packageId, string xml)
        {
            DataProvider.UpdatePackageBandwidth(packageId, xml);
            return 0;
        }

        public static int UpdatePackageDiskSpace(int packageId, string xml)
        {
            DataProvider.UpdatePackageDiskSpace(packageId, xml);
            return 0;
        }

        public static List<ServiceProviderItem> CreateServiceItemsList(DataSet dsItems, int itemsTablePosition)
        {
            List<ServiceProviderItem> items = new List<ServiceProviderItem>();

            DataView dvItems = dsItems.Tables[itemsTablePosition].DefaultView;
            foreach (DataRowView drItem in dvItems)
            {
                DataView dvProps = new DataView(dsItems.Tables[itemsTablePosition + 1], "ItemID=" + drItem["ItemID"].ToString(),
                    "", DataViewRowState.CurrentRows);
                items.Add(CreateServiceItem(drItem, dvProps));
            }

            return items;
        }


        private static ServiceProviderItem CreateServiceItem(DataRowView drItem, DataView dvProps)
        {
            // create item instance
            string itemTypeName = (string)drItem["TypeName"];
            Type itemType = Type.GetType(itemTypeName);

            if (itemType == null)
                itemType = typeof(ServiceProviderItem); // create generic item

            // create item instance and fill other properties
            ServiceProviderItem item = (ServiceProviderItem)ObjectUtils.CreateObjectFromDataview(
                Type.GetType(itemTypeName), dvProps, "PropertyName", "PropertyValue", true);

            // fill item key properties
            item.Id = (int)drItem["ItemID"];
            item.TypeId = (int)drItem["ItemTypeID"];
            item.Name = (string)drItem["ItemName"];
            item.PackageId = (int)drItem["PackageID"];
            item.ServiceId = (int)drItem["ServiceID"];
            item.GroupName = (string)drItem["GroupName"];
            item.CreatedDate = Utils.ParseDate(drItem["CreatedDate"]);

            return item;
        }

        private static void FlatternItemsTable(DataSet dsItems, int itemsTablePosition, Type itemType)
        {
            DataTable dtItems = dsItems.Tables[itemsTablePosition];
            DataTable dtProps = dsItems.Tables[itemsTablePosition + 1];

            // add columns to the table
            PropertyInfo[] props = itemType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in props)
            {
                if (!dtItems.Columns.Contains(prop.Name) && !prop.PropertyType.IsArray)
                    dtItems.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (DataRow drItem in dtItems.Rows)
            {
                DataView dvProps = new DataView(dtProps, "ItemID=" + drItem["ItemID"].ToString(),
                    "", DataViewRowState.CurrentRows);

                foreach (DataRowView drProp in dvProps)
                {
                    object propValue = "";
                    string sVal = (drProp["PropertyValue"] != DBNull.Value) ? drProp["PropertyValue"].ToString() : "";
                    string columnName = drProp["PropertyName"].ToString();

                    if (!dtItems.Columns.Contains(columnName))
                        continue;

                    Type columnType = dtItems.Columns[columnName].DataType;
                    if (columnType == typeof(string))
                        propValue = sVal;
                    if (columnType == typeof(int))
                        propValue = (sVal != "") ? Int32.Parse(sVal) : 0;
                    if (columnType == typeof(long))
                        propValue = (sVal != "") ? Int64.Parse(sVal) : 0;
                    if (columnType == typeof(bool))
                        propValue = (sVal != "") ? Boolean.Parse(sVal) : false;
                    if (columnType == typeof(Guid))
                        propValue = (!string.IsNullOrEmpty(sVal)) ? new Guid(sVal) : Guid.Empty;

                    drItem[columnName] = propValue;
                }
            }
        }
        #endregion

        #region Package Settings
        public static PackageSettings GetPackageSettings(int packageId, string settingsName)
        {
            IDataReader reader = DataProvider.GetPackageSettings(
                SecurityContext.User.UserId, packageId, settingsName);

            PackageSettings settings = new PackageSettings();
            settings.PackageId = packageId;
            settings.SettingsName = settingsName;

            while (reader.Read())
            {
                settings.PackageId = (int)reader["PackageID"];
                settings[(string)reader["PropertyName"]] = (string)reader["PropertyValue"];
            }
            reader.Close();

            // set root settings if required
            if (settings.PackageId < 2)
            {
                // instant alias
                if (String.Compare(PackageSettings.INSTANT_ALIAS, settingsName, true) == 0)
                {
                    // load package info
                    PackageInfo package = GetPackage(packageId);

                    // load package server
                    ServerInfo srv = ServerController.GetServerByIdInternal(package.ServerId);
                    if (srv != null)
                        settings["InstantAlias"] = srv.InstantDomainAlias;
                }

                // name servers
                else if (String.Compare(PackageSettings.NAME_SERVERS, settingsName, true) == 0)
                {
                    // load DNS service settings
                    int dnsServiceId = GetPackageServiceId(packageId, ResourceGroups.Dns);
                    if (dnsServiceId > 0)
                    {
                        StringDictionary dnsSettings = ServerController.GetServiceSettings(dnsServiceId);
                        settings["NameServers"] = dnsSettings["NameServers"];
                    }
                }

                // shared SSL sites
                else if (String.Compare(PackageSettings.SHARED_SSL_SITES, settingsName, true) == 0)
                {
                    // load WEB service settings
                    int webServiceId = GetPackageServiceId(packageId, ResourceGroups.Web);
                    if (webServiceId > 0)
                    {
                        StringDictionary webSettings = ServerController.GetServiceSettings(webServiceId);
                        settings["SharedSslSites"] = webSettings["SharedSslSites"];
                    }
                }

                // child spaces location
                else if (String.Compare(PackageSettings.SPACES_FOLDER, settingsName, true) == 0)
                {
                    settings["ChildSpacesFolder"] = "";
                }

                // Exchange Server
                else if (String.Compare(PackageSettings.EXCHANGE_SERVER, settingsName, true) == 0)
                {
                    // load Exchange service settings
                    int exchServiceId = GetPackageServiceId(packageId, ResourceGroups.Exchange);
                    if (exchServiceId > 0)
                    {
                        StringDictionary exchSettings = ServerController.GetServiceSettings(exchServiceId);
                        settings["TempDomain"] = exchSettings["TempDomain"];
                    }
                }

                // VPS
                else if (String.Compare(PackageSettings.VIRTUAL_PRIVATE_SERVERS, settingsName, true) == 0)
                {
                    // load Exchange service settings
                    int vpsServiceId = GetPackageServiceId(packageId, ResourceGroups.VPS);
                    if (vpsServiceId > 0)
                    {
                        StringDictionary vpsSettings = ServerController.GetServiceSettings(vpsServiceId);
                        settings["HostnamePattern"] = vpsSettings["HostnamePattern"];
                    }
                }

                // VPS2012
                else if (String.Compare(PackageSettings.VIRTUAL_PRIVATE_SERVERS_2012, settingsName, true) == 0)
                {
                    // load Exchange service settings
                    int vpsServiceId = GetPackageServiceId(packageId, ResourceGroups.VPS2012);
                    if (vpsServiceId > 0)
                    {
                        StringDictionary vpsSettings = ServerController.GetServiceSettings(vpsServiceId);
                        settings["HostnamePattern"] = vpsSettings["HostnamePattern"];
                    }
                }

                //vpforCP
                else if (String.Compare(PackageSettings.VIRTUAL_PRIVATE_SERVERS_FOR_PRIVATE_CLOUD, settingsName, true) == 0)
                {
                    int vpsServiceId = GetPackageServiceId(packageId, ResourceGroups.VPSForPC);
                    if (vpsServiceId > 0)
                    {
                        StringDictionary vpsSettings = ServerController.GetServiceSettings(vpsServiceId);
                        settings["HostnamePattern"] = vpsSettings["HostnamePattern"];
                    }
                }
            }

            return settings;
        }

        public static int UpdatePackageSettings(PackageSettings settings)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            //// get user details
            //UserInfo user = GetUserInternally(settings.UserId);

            //// place log record
            //LogRecord record = new LogRecord();
            //record.RecordType = LogRecordType.Information;
            //record.PackageId = 0;
            //record.ItemId = settings.UserId;
            //record.Source = LogSource.USERS;
            //record.Action = "USERS_UPDATE_USER_SETTINGS";
            //record.ActionParameters.Add(user.Username).Add(settings.SettingsName);

            try
            {
                // build xml
                XmlDocument doc = new XmlDocument();
                XmlElement nodeProps = doc.CreateElement("properties");
                if (settings.SettingsArray != null)
                {
                    foreach (string[] pair in settings.SettingsArray)
                    {
                        XmlElement nodeProp = doc.CreateElement("property");
                        nodeProp.SetAttribute("name", pair[0]);
                        nodeProp.SetAttribute("value", pair[1]);
                        nodeProps.AppendChild(nodeProp);
                    }
                }

                string xml = nodeProps.OuterXml;

                // update settings
                DataProvider.UpdatePackageSettings(SecurityContext.User.UserId,
                    settings.PackageId, settings.SettingsName, xml);

                return 0;
            }
            catch (Exception ex)
            {
                //record.RecordType = LogRecordType.Error;
                //record.Description["Error"] = ex.ToString();
                throw new Exception("Could not update package settings", ex);
            }
            //finally
            //{
            //    AuditLog.AddRecord(record);
            //}
        }

        public static bool SetDefaultTopPackage(int userId, int packageId) {
            List<PackageInfo> lpi = GetPackages(userId);
            foreach(PackageInfo pi in lpi) {
                if(pi.DefaultTopPackage) {
                    pi.DefaultTopPackage = false;
                    UpdatePackage(pi);
                }
                if(pi.PackageId == packageId) {
                    pi.DefaultTopPackage = true;
                    UpdatePackage(pi);
                }
            }
            return true;
        }

        #endregion

        #region Quotas
        public static QuotaValueInfo GetPackageQuota(int packageId, string quotaName)
        {
            QuotaValueInfo quota = ObjectUtils.FillObjectFromDataReader<QuotaValueInfo>(
                DataProvider.GetPackageQuota(SecurityContext.User.UserId,
                packageId, quotaName));

            // set exhausted flag
            quota.QuotaExhausted = (quota.QuotaAllocatedValue != -1 && quota.QuotaUsedValue >= quota.QuotaAllocatedValue);

            return quota;
        }
        #endregion

        #region Fix for item #1547 by Pavel Tsurbeleu
        public static string BuildMessageBccField(params string[] bccAddresses)
        {
            List<string> mailAddresses = new List<string>();
            //
            foreach (string bccItem in bccAddresses)
            {
                if (!String.IsNullOrEmpty(bccItem))
                {
                    string bccAddress = bccItem.Trim();
                    // check if value is semicolon-delimited and convert it to the standard format
                    if (bccAddress.Contains(";"))
                        bccAddress = bccAddress.Replace(';', ',');
                    //
                    mailAddresses.Add(bccAddress);
                }
            }
            //
            return String.Join(",", mailAddresses.ToArray());
        }
        #endregion

        #region Templates
        public static int SendAccountSummaryLetter(int userId, string to, string bcc, bool signup)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load user details
            UserInfo user = UserController.GetUser(userId);
            if (user == null)
                return BusinessErrorCodes.ERROR_USER_NOT_FOUND;

            UserSettings settings = UserController.GetUserSettings(userId, UserSettings.ACCOUNT_SUMMARY_LETTER);
            string from = settings["From"];

            #region Fix for item #1547 by Pavel Tsurbeleu
            // Build BCC field
            bcc = BuildMessageBccField(bcc, settings["CC"], user.SecondaryEmail);
            #endregion

            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;
            if (!String.IsNullOrEmpty(settings["Priority"]))
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

            if (String.IsNullOrEmpty(body))
                return 0;// BusinessErrorCodes.ERROR_SETTINGS_ACCOUNT_LETTER_EMPTY_BODY;

            // load user info
            if (to == null)
                to = user.Email;

            subject = EvaluateAccountTempate(userId, subject, signup, true);
            body = EvaluateAccountTempate(userId, body, signup, true);

            // send message
            return MailHelper.SendMessage(from, to, bcc, subject, body, priority, isHtml);
        }

        public static int SendPackageSummaryLetter(int packageId, string to, string bcc, bool signup)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load package
            PackageInfo package = GetPackage(packageId);
            if (package == null)
                return BusinessErrorCodes.ERROR_PACKAGE_NOT_FOUND;

            // load user details
            UserInfo user = UserController.GetUser(package.UserId);
            if (user == null)
                return BusinessErrorCodes.ERROR_USER_NOT_FOUND;

            UserSettings settings = UserController.GetUserSettings(package.UserId, UserSettings.PACKAGE_SUMMARY_LETTER);
            string from = settings["From"];

            #region Fix for item #1547 by Pavel Tsurbeleu
            // Build BCC field
            bcc = BuildMessageBccField(bcc, settings["CC"], user.SecondaryEmail);
            #endregion

            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;
            if (!String.IsNullOrEmpty(settings["Priority"]))
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

            if (String.IsNullOrEmpty(body))
                return 0;// BusinessErrorCodes.ERROR_SETTINGS_ACCOUNT_LETTER_EMPTY_BODY;

            // load user info
            if (to == null)
                to = user.Email;

            subject = EvaluatePackageTempate(packageId, subject, signup, true);
            body = EvaluatePackageTempate(packageId, body, signup, true);

            // send message
            return MailHelper.SendMessage(from, to, bcc, subject, body, priority, isHtml);
        }

        public static string GetEvaluatedPackageTemplateBody(int packageId, bool signup)
        {
            // load user info
            UserInfo user = GetPackageOwner(packageId);
            string settingName = user.HtmlMail ? "HtmlBody" : "TextBody";

            // get space letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.PACKAGE_SUMMARY_LETTER);
            string body = settings[settingName];
            if (String.IsNullOrEmpty(body))
                return null;

            string result = EvaluatePackageTempate(packageId, body, signup, false);
            return user.HtmlMail ? result : result.Replace("\n", "<br/>");
        }

        public static string GetEvaluatedAccountTemplateBody(int userId, bool signup)
        {
            // load user info
            UserInfo user = UserController.GetUser(userId);
            string settingName = user.HtmlMail ? "HtmlBody" : "TextBody";

            // get account letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.ACCOUNT_SUMMARY_LETTER);
            string body = settings[settingName];
            if (String.IsNullOrEmpty(body))
                return null;

            string result = EvaluateAccountTempate(userId, body, signup, false);
            return user.HtmlMail ? result : result.Replace("\n", "<br/>");
        }

        public static string EvaluateUserPackageTempate(int userId, int packageId, string template)
        {
            // package context
            if (packageId > -1)
            {
                return EvaluatePackageTempate(packageId, template, false, false);
            }
            else
            {
                return EvaluateAccountTempate(userId, template, false, false);
            }
        }

        public static string EvaluatePackageTempate(int packageId, string template, bool signup, bool email)
        {
            Hashtable items = new Hashtable();

            items["Signup"] = signup;
            items["Email"] = email;

            // load package
            PackageInfo package = GetPackage(packageId);
            if (package == null)
                return null;

            // add package context
            PackageContext cntx = GetPackageContext(packageId);
            items["space"] = cntx;

            // get hosting plan
            HostingPlanInfo plan = GetHostingPlan(package.PlanId);
            if (plan != null)
                items["plan"] = plan;

            // name servers
            PackageSettings packageSettings = PackageController.GetPackageSettings(packageId, PackageSettings.NAME_SERVERS);
            string[] nameServers = new string[] { };
            if (!String.IsNullOrEmpty(packageSettings["NameServers"]))
                nameServers = packageSettings["NameServers"].Split(';');

            items["NameServers"] = nameServers;

            // instant alias
            string instantAlias = null;
            packageSettings = PackageController.GetPackageSettings(packageId, PackageSettings.INSTANT_ALIAS);
            if (!String.IsNullOrEmpty(packageSettings["InstantAlias"]))
                instantAlias = packageSettings["InstantAlias"];

            items["InstantAlias"] = instantAlias;

            // web sites
            List<SolidCP.Providers.Web.WebSite> webSites =
                WebServerController.GetWebSites(packageId, false);
            items["WebSites"] = webSites;

            // domains
            List<DomainInfo> domains = ServerController.GetDomains(packageId, false);
            items["Domains"] = domains;

            // FTP Server
            int ftpServiceId = GetPackageServiceId(packageId, ResourceGroups.Ftp);
            if (ftpServiceId > 0)
            {
                // get FTP DNS records
                List<GlobalDnsRecord> ftpRecords = ServerController.GetDnsRecordsByService(ftpServiceId);
                if (ftpRecords.Count > 0)
                {
                    GlobalDnsRecord ftpRecord = ftpRecords[0];
                    string ftpIp = ftpRecord.ExternalIP;
                    if (String.IsNullOrEmpty(ftpIp))
                        ftpIp = ftpRecord.RecordData;

                    items["FtpIP"] = ftpIp;
                }
            }

            // FTP accounts
            List<FtpAccount> ftpAccounts =
                FtpServerController.GetFtpAccounts(packageId, false);
            items["FtpAccounts"] = ftpAccounts;

            // Mail Accounts
            List<MailAccount> mailAccounts =
                MailServerController.GetMailAccounts(packageId, false);
            items["MailAccounts"] = mailAccounts;

            // mail server IP
            List<GlobalDnsRecord> mailRecords = new List<GlobalDnsRecord>();
            int mailServiceId = GetPackageServiceId(packageId, ResourceGroups.Mail);
            if (mailServiceId > 0)
            {
                StringDictionary mailSettings = ServerController.GetServiceSettings(mailServiceId);
                if (mailSettings != null)
                    items["MailIP"] = ServerController.GetExternalIPAddress(Utils.ParseInt(mailSettings["ServerIPAddress"], 0));

                mailRecords = ServerController.GetDnsRecordsByService(mailServiceId);
            }
            items["MailRecords"] = mailRecords;

            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MsSql2000);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MsSql2005);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MsSql2008);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MsSql2012);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MsSql2014);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MsSql2016);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MySql4);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MySql5);
            SetSqlServerExternalAddress(packageId, items, ResourceGroups.MariaDB);

            // Exchange organizations
            items["ExchangeOrganizations"] = ExchangeServerController.GetExchangeOrganizations(packageId, false);

            // Exchange service settings
            int exchangeServiceId = GetPackageServiceId(packageId, ResourceGroups.Exchange);
            StringDictionary exchangeSettings = ServerController.GetServiceSettings(exchangeServiceId);
            if (exchangeSettings != null)
            {
                items["TempDomain"] = exchangeSettings["TempDomain"];
                items["AutodiscoverIP"] = exchangeSettings["AutodiscoverIP"];
                items["AutodiscoverDomain"] = exchangeSettings["AutodiscoverDomain"];
                items["OwaUrl"] = exchangeSettings["OwaUrl"];
                items["ActiveSyncServer"] = exchangeSettings["ActiveSyncServer"];
                items["SmtpServers"] = Utils.ParseDelimitedString(exchangeSettings["SmtpServers"], '\n');
            }

            // package root folder
            items["PackageRootFolder"] = FilesController.GetHomeFolder(packageId);

            // get user details
            UserInfo user = UserController.GetUser(package.UserId);
            if (user == null)
                return null;

            items["user"] = user;

            // get reseller details
            UserInfoInternal reseller = UserController.GetUser(user.OwnerId);
            if (reseller != null)
            {
                reseller.Password = "";
                items["reseller"] = reseller;
            }

            return EvaluateTemplate(template, items);
        }

        private static void SetSqlServerExternalAddress(int packageId, Hashtable items, string groupName)
        {
            int serviceId = GetPackageServiceId(packageId, groupName);
            if (serviceId > 0)
            {
                StringDictionary sqlSettings = ServerController.GetServiceSettings(serviceId);
                items[groupName + "Address"] = sqlSettings["ExternalAddress"]; // left for compatibility with existing templates
                items[groupName + "ExternalAddress"] = sqlSettings["ExternalAddress"];
                items[groupName + "InternalAddress"] = sqlSettings["InternalAddress"];
            }
        }

        public static string EvaluateAccountTempate(int userId, string template, bool signup, bool email)
        {
            Hashtable items = new Hashtable();

            items["Signup"] = signup;
            items["Email"] = email;

            // get user details
            UserInfo user = UserController.GetUser(userId);
            if (user == null)
                return null;

            items["user"] = user;

            // get reseller details
            UserInfoInternal reseller = UserController.GetUser(user.OwnerId);
            if (reseller != null)
            {
                reseller.Password = "";
                items["reseller"] = reseller;
            }

            // get all purchased packages
            List<PackageInfo> packages = GetMyPackages(userId);
            items["Spaces"] = packages;

            // hosting plans
            Hashtable plans = new Hashtable();
            foreach (PackageInfo package in packages)
            {
                HostingPlanInfo plan = GetHostingPlan(package.PlanId);

                if (plan != null && !plans.ContainsKey(plan.PlanId))
                    plans.Add(plan.PlanId, plan);
            }
            items["Plans"] = plans;

            //Add ons
            Hashtable addOns = new Hashtable();
            int i = 0;
            foreach (PackageInfo package in packages)
            {
                List<PackageAddonInfo> lstAddOns = ObjectUtils.CreateListFromDataSet<PackageAddonInfo>(GetPackageAddons(package.PackageId));
                foreach (PackageAddonInfo addOn in lstAddOns)
                {
                    addOns.Add(i, addOn);
                    i++;
                }

            }
            items["Addons"] = addOns;

            // package contexts
            Hashtable cntxs = new Hashtable();
            foreach (PackageInfo package in packages)
            {
                PackageContext cntx = GetPackageContext(package.PackageId);
                cntxs.Add(package.PackageId, cntx);
            }
            items["SpaceContexts"] = cntxs;

            return EvaluateTemplate(template, items);
        }

        public static string EvaluateTemplate(string template, Hashtable items)
        {
            StringWriter writer = new StringWriter();

            try
            {
                Template tmp = new Template(template);

                if (items != null)
                {
                    foreach (string key in items.Keys)
                        tmp[key] = items[key];
                }

                tmp.Evaluate(writer);
            }
            catch (ParserException ex)
            {
                return String.Format("Error in template (Line {0}, Column {1}): {2}",
                    ex.Line, ex.Column, ex.Message);
            }
            return writer.ToString();
        }
        #endregion

        #region Reports
        public static DataSet GetPackagesBandwidthPaged(int userId, int packageId,
            DateTime startDate, DateTime endDate, string sortColumn,
            int startRow, int maximumRows)
        {
            return DataProvider.GetPackagesBandwidthPaged(
                SecurityContext.User.UserId, userId, packageId, startDate, endDate, sortColumn,
                startRow, maximumRows);
        }

        public static DataSet GetPackagesDiskspacePaged(int userId, int packageId, string sortColumn,
            int startRow, int maximumRows)
        {
            return DataProvider.GetPackagesDiskspacePaged(
                SecurityContext.User.UserId, userId, packageId, sortColumn,
                startRow, maximumRows);
        }

        public static DataSet GetPackageBandwidth(int packageId, DateTime startDate, DateTime endDate)
        {
            return DataProvider.GetPackageBandwidth(
                SecurityContext.User.UserId, packageId, startDate, endDate);
        }

        public static DataSet GetPackageDiskspace(int packageId)
        {
            return DataProvider.GetPackageDiskspace(
                SecurityContext.User.UserId, packageId);
        }

        #endregion

        #region Overusage Report
        public static Reports.OverusageReport GetOverusageSummaryReport(int userId, int packageId, DateTime startDate, DateTime endDate)
        {
            Reports.OverusageReport report = new Reports.OverusageReport();

            report = GetDiskspaceReport(report, userId, packageId, String.Empty);
            report = GetBandwidthReport(report, userId, packageId, String.Empty, startDate, endDate);

            return report;
        }

        public static Reports.OverusageReport GetDiskspaceOverusageDetailsReport(int userId, int packageId)
        {
            Reports.OverusageReport report = new Reports.OverusageReport();

            report = GetDiskspaceInformationAboutPackage(report, packageId);
            report = GetDiskspaceDetails(report);

            return report;
        }

        public static Reports.OverusageReport GetBandwidthDetailsReport(int userId, int packageId, DateTime startDate, DateTime endDate)
        {
            Reports.OverusageReport report = new Reports.OverusageReport();

            report = GetBandwidthInformationAboutPackage(report, packageId, startDate, endDate);
            report = GetBandwidthDetails(report, startDate, endDate);

            return report;
        }

        private static Reports.OverusageReport GetDiskspaceInformationAboutPackage(Reports.OverusageReport report, int packageId)
        {
            PackageInfo packageInfo = GetPackage(packageId);
            packageInfo.DiskSpaceQuota = GetPackageQuota(packageId, "OS.Diskspace").QuotaAllocatedValue;
            packageInfo.DiskSpaceQuota = packageInfo.DiskSpaceQuota < 1 ? 0 : packageInfo.DiskSpaceQuota;

            // 1. Add HostingSpace row
            report.HostingSpace.AddHostingSpaceRow(
                Reports.OverusageReport.HostingSpaceRow
                .CreateFromPackageInfo(
                      report
                    , packageInfo
                    , UserController.GetUser(packageInfo.UserId)
                    , ServerController.GetServerByIdInternal(packageInfo.ServerId)
                    , packageInfo.DiskSpace > packageInfo.DiskSpaceQuota
                    , packageInfo.BandWidth > packageInfo.BandWidthQuota
                    , String.Empty
                )
            );

            report.DiskspaceOverusage.AddDiskspaceOverusageRow(
                Reports.OverusageReport.DiskspaceOverusageRow
                .CreateFromPackageInfo(
                      report
                    , packageInfo
                )
            );

            return report;
        }


        private static Reports.OverusageReport GetBandwidthInformationAboutPackage(Reports.OverusageReport report, int packageId, DateTime startDate, DateTime endDate)
        {
            PackageInfo packageInfo = GetPackage(packageId);
            packageInfo.BandWidthQuota = GetPackageQuota(packageId, "OS.Bandwidth").QuotaAllocatedValue;
            packageInfo.BandWidthQuota = packageInfo.BandWidthQuota < 1 ? 0 : packageInfo.BandWidthQuota;

            // 1. Add HostingSpace row
            report.HostingSpace.AddHostingSpaceRow(
                Reports.OverusageReport.HostingSpaceRow
                .CreateFromPackageInfo(
                      report
                    , packageInfo
                    , UserController.GetUser(packageInfo.UserId)
                    , ServerController.GetServerByIdInternal(packageInfo.ServerId)
                    , packageInfo.DiskSpace > packageInfo.DiskSpaceQuota
                    , packageInfo.BandWidth > packageInfo.BandWidthQuota
                    , String.Empty
                )
            );

            // 2. Add BandwidthOverusage row
            report.BandwidthOverusage.AddBandwidthOverusageRow(
                Reports.OverusageReport.BandwidthOverusageRow
                .CreateFromPackageInfo(
                      report
                    , packageInfo
                )
            );

            return report;
        }


        private static Reports.OverusageReport GetBandwidthDetails(Reports.OverusageReport report, DateTime startDate, DateTime endDate)
        {
            foreach (Reports.OverusageReport.HostingSpaceRow hsRow in report.HostingSpace.Rows)
            {
                DataSet ds = GetPackageBandwidth((int)hsRow.HostingSpaceId, startDate, endDate);

                foreach (DataRow bwRow in ds.Tables[0].Rows)
                {
                    report.OverusageDetails.AddOverusageDetailsRow(
                        Reports.OverusageReport.OverusageDetailsRow
                        .CreateFromBandwidthRow(report, bwRow, hsRow.HostingSpaceId, "Bandwidth")
                    );
                }
            }

            return report;
        }

        private static Reports.OverusageReport GetDiskspaceDetails(Reports.OverusageReport report)
        {
            foreach (Reports.OverusageReport.HostingSpaceRow hsRow in report.HostingSpace.Rows)
            {
                DataSet ds = GetPackageDiskspace((int)hsRow.HostingSpaceId);

                foreach (DataRow dsRow in ds.Tables[0].Rows)
                {
                    report.OverusageDetails.AddOverusageDetailsRow(
                        Reports.OverusageReport.OverusageDetailsRow
                        .CreateFromPackageDiskspaceRow(report, dsRow, hsRow.HostingSpaceId, "Diskspace")
                    );
                }
            }

            return report;
        }

        private static Reports.OverusageReport GetBandwidthReport(Reports.OverusageReport report, int userId, long packageId, string rootHostingSpacePath, DateTime startDate, DateTime endDate)
        {
            DataTable bwTable = GetPackagesBandwidthPaged(userId, (int)packageId, startDate, endDate);

            Reports.OverusageReport.HostingSpaceRow hsRow = null;

            // 1.
            foreach (DataRow bwRow in bwTable.Rows)
            {
                if (Reports.OverusageReport.HostingSpaceRow
                    .VerifyIfBandwidthOverused(bwRow))
                {
                    hsRow = report.HostingSpace.FindByHostingSpaceId(
                        Reports.OverusageReport.HostingSpaceRow.GetPackageId(bwRow)
                    );

                    if (hsRow != null)
                    {
                        hsRow.IsBandwidthOverused = true;
                    }
                    else
                    {
                        report.HostingSpace.AddHostingSpaceRow(
                            CreateHostingSpacesRow(bwRow, report, false, true, rootHostingSpacePath)
                            );
                    }

                    report.BandwidthOverusage.AddBandwidthOverusageRow(
                             Reports.OverusageReport.BandwidthOverusageRow
                             .CreateFromHostingSpaceDataRow(report, bwRow)
                        );
                }
            }


            // 2.
            foreach (DataRow bwRow in bwTable.Rows)
            {
                if (Reports.OverusageReport.HostingSpaceRow
                    .IsContainChildSpaces(bwRow))
                {
                    report = GetBandwidthReport(
                          report
                        , userId
                        , Reports.OverusageReport.HostingSpaceRow.GetPackageId(bwRow)
                        , String.Format(
                                      "{0} \\ {1}"
                                    , rootHostingSpacePath
                                    , Reports.OverusageReportUtils.GetStringOrDefault(bwRow, "PackageName", String.Empty)
                              )
                        , startDate
                        , endDate
                    );
                }
            }


            return report;
        }

        /// <summary>
        /// Gathers summary information about disksapce usage.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="packageId"></param>
        /// <param name="rootHostingSpacePath"></param>
        /// <returns></returns>
        private static Reports.OverusageReport GetDiskspaceReport(Reports.OverusageReport report, int userId, long packageId, string rootHostingSpacePath)
        {
            DataTable dsTable = GetPackagesDiskspacePaged(userId, (int)packageId);

            Reports.OverusageReport.HostingSpaceRow hsRow = null;

            //1. generate basic list of reports
            foreach (DataRow dsRow in dsTable.Rows)
            {
                if (Reports.OverusageReport.HostingSpaceRow
                    .VerifyIfDiskspaceOverused(dsRow))
                {
                    //determine whether this row already exists in the table
                    hsRow = report.HostingSpace.FindByHostingSpaceId(
                        Reports.OverusageReport.HostingSpaceRow.GetPackageId(dsRow)
                        );
                    if (hsRow != null)
                    {
                        //if so, just update it's Diskspace overusage status
                        hsRow.IsDiskspaceOverused = true;
                    }
                    else
                    {
                        //othrewise, create a new one
                        report.HostingSpace.AddHostingSpaceRow(
                            CreateHostingSpacesRow(dsRow, report, true, false, rootHostingSpacePath)
                        );
                    }

                    report.DiskspaceOverusage.AddDiskspaceOverusageRow(
                        Reports.OverusageReport.DiskspaceOverusageRow
                        .CreateFromHostingSpacesRow(report, dsRow)
                    );
                }
            }

            //2. analyze children
            foreach (DataRow dsRow in dsTable.Rows)
            {
                if (Reports.OverusageReport.HostingSpaceRow.IsContainChildSpaces(dsRow))
                {
                    report = GetDiskspaceReport(
                              report
                            , userId
                            , Reports.OverusageReport.HostingSpaceRow.GetPackageId(dsRow)
                            , String.Format(
                                      "{0} \\ {1}"
                                    , rootHostingSpacePath
                                    , Reports.OverusageReportUtils.GetStringOrDefault(dsRow, "PackageName", String.Empty)
                              )
                        );
                }
            }

            return report;
        }

        /// <summary>
        /// Retrieves child Hosting Spaces (packages) by parent Hosting Space Id
        /// with information about bandwidth usage per Hosting Space
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        private static DataTable GetPackagesDiskspacePaged(int userId, int packageId)
        {
            return GetPackagesDiskspacePaged(
                      userId
                    , packageId
                    , String.Empty
                    , 0
                    , int.MaxValue
                ).Tables[1];
        }

        /// <summary>
        /// Retrieves child Hosting Spaces (packages) using parent Hosting Space Id
        /// with information about bandwidth usage per Hosting Space
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static DataTable GetPackagesBandwidthPaged(int userId, int packageId, DateTime startDate, DateTime endDate)
        {
            return GetPackagesBandwidthPaged(
                      userId
                    , packageId
                    , startDate
                    , endDate
                    , String.Empty
                    , 0
                    , int.MaxValue
                ).Tables[1];
        }

        /// <summary>
        /// Creates HostingSpacesRow class instance.
        /// </summary>
        /// <param name="hsRow">Hosting Space information data row</param>
        /// <param name="currentOverusageReport">The report to update.</param>
        /// <param name="isDiskspaceOverused">Shows whether diskspace is overused.</param>
        /// <param name="isBandwidthOverused">Shows whether bandwidth is overused.</param>
        /// <param name="packageFullTree">Hosting spaces tree.</param>
        /// <returns>HostingSpacesRow class instance.</returns>
        private static Reports.OverusageReport.HostingSpaceRow CreateHostingSpacesRow(DataRow hsRow, Reports.OverusageReport currentOverusageReport, bool isDiskspaceOverused, bool isBandwidthOverused, string packageFullTree)
        {
            Reports.OverusageReport.HostingSpaceRow row = Reports.OverusageReport.HostingSpaceRow.CreateFromHostingSpacesRow(currentOverusageReport, hsRow, isDiskspaceOverused, isBandwidthOverused, packageFullTree);

            //TODO: Optimiza it in some way :)
            PackageInfo packageInfo = GetPackage((int)row.HostingSpaceId);
            row.Location = ServerController.GetServerShortDetails(packageInfo.ServerId).ServerName;
            row.HostingSpaceCreationDate = packageInfo.PurchaseDate;

            return row;
        }
        #endregion

        #region Helper Methods
        public static Dictionary<int, List<ServiceProviderItem>> OrderServiceItemsByServices(
            List<ServiceProviderItem> items)
        {
            // order items by service id
            Dictionary<int, List<ServiceProviderItem>> orderedItems = new Dictionary<int, List<ServiceProviderItem>>();
            foreach (ServiceProviderItem item in items)
            {
                int serviceId = item.ServiceId;
                List<ServiceProviderItem> serviceItems = null;
                if (orderedItems.ContainsKey(serviceId))
                    serviceItems = orderedItems[serviceId];
                else
                {
                    serviceItems = new List<ServiceProviderItem>();
                    orderedItems.Add(serviceId, serviceItems);
                }

                // add item to the appropriate list
                serviceItems.Add(item);
            }

            return orderedItems;
        }
        #endregion
    }
}

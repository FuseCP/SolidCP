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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;

using SolidCP.EnterpriseServer;
using System.Collections;
using System.Collections.Generic;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for PackagesHelper
    /// </summary>
    public class PackagesHelper
    {
        private const int PACKAGE_CACHE_TIMEOUT = 30; // minutes

        public static PackageInfo GetCachedPackage(int packageId)
        {
            string key = "CachedPackageInfo" + packageId;
            if (HttpContext.Current.Cache[key] != null)
                return (PackageInfo)HttpContext.Current.Cache[key];

            // load package info from ES
            PackageInfo package = ES.Services.Packages.GetPackage(packageId);
            
            // place to cache
            if(package != null)
                HttpContext.Current.Cache.Insert(key, package, null,
                    DateTime.Now.AddMinutes(PACKAGE_CACHE_TIMEOUT), Cache.NoSlidingExpiration);

            return package;
        }

        public static bool IsQuotaEnabled(int packageId, string quotaName)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            return cntx.Quotas.ContainsKey(quotaName) && !cntx.Quotas[quotaName].QuotaExhausted;
        }

        public static PackageContext GetCachedPackageContext(int packageId)
        {
            try
            {
                string key = "CachedPackageContext" + packageId.ToString();
                PackageContext cntx = (PackageContext)HttpContext.Current.Items[key];
                if (cntx == null)
                {
                    // load context
                    cntx = ES.Services.Packages.GetPackageContext(packageId);

                    if (cntx != null)
                    {
                        // fill dictionaries
                        foreach (HostingPlanGroupInfo group in cntx.GroupsArray)
                            cntx.Groups.Add(group.GroupName, group);

                        foreach (QuotaValueInfo quota in cntx.QuotasArray)
                            cntx.Quotas.Add(quota.QuotaName, quota);
                    }
                    else
                    {
                        // create empty context
                        cntx = new PackageContext();
                    }

                    // add it to the cach
                    HttpContext.Current.Items[key] = cntx;
                }
                return cntx;
            }
            catch
            {
                return null;
            }
        }

        public static HostingPlanContext GetCachedHostingPlanContext(int planId)
        {
            string key = "CachedHostingPlanContext" + planId.ToString();
            HostingPlanContext cntx = (HostingPlanContext)HttpContext.Current.Items[key];
            if (cntx == null)
            {
                // load context
                cntx = ES.Services.Packages.GetHostingPlanContext(planId);

                if (cntx != null)
                {
                    // fill dictionaries
                    foreach (HostingPlanGroupInfo group in cntx.GroupsArray)
                        cntx.Groups.Add(group.GroupName, group);

                    foreach (QuotaValueInfo quota in cntx.QuotasArray)
                        cntx.Quotas.Add(quota.QuotaName, quota);
                }
                else
                {
                    // create empty context
                    cntx = new HostingPlanContext();
                }

                // add it to the cach
                HttpContext.Current.Items[key] = cntx;
            }
            return cntx;
        }

        public static bool CheckGroupQuotaEnabled(int packageId, string groupName, string quotaName)
        {
            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);

            if (cntx == null)
                return false;

            // check group
            if (!cntx.Groups.ContainsKey(groupName))
                return false;

            // check wildcard quota name
            if (!string.IsNullOrEmpty(groupName) && quotaName.Substring(groupName.Length) == ".*")
                return true;
            
            // check quota
            if (cntx.Quotas.ContainsKey(quotaName))
                return !cntx.Quotas[quotaName].QuotaExhausted;

            return false;
        }

        public DataSet GetMyPackages()
        {
            return ES.Services.Packages.GetRawMyPackages(PanelSecurity.SelectedUserId);
        }

        public Hashtable GetMyPackages(int index, int PackagesPerPage) 
        {
            Hashtable ret = new Hashtable();

            DataTable table = ES.Services.Packages.GetRawMyPackages(PanelSecurity.SelectedUserId).Tables[0];
            if(table.Rows.Count > 0) {
                System.Collections.Generic.IEnumerable<DataRow> dr = table.AsEnumerable().Skip(PackagesPerPage * index - PackagesPerPage).Take(PackagesPerPage);
            
                DataSet set = new DataSet();
                set.Tables.Add(dr.CopyToDataTable());

                ret.Add("DataSet", set);
                ret.Add("RowCount", table.Rows.Count);
            }
            return ret;
        }

        public DataSet GetMyPackage(int packageid) {
            DataSet ret = new DataSet();
            DataTable table = ES.Services.Packages.GetRawMyPackages(PanelSecurity.SelectedUserId).Tables[0];
            if (table.Rows.Count > 0)
            {
                var exists = table.Select("PackageID = " + packageid);
                if (exists.Length != 0)
                {
                    DataTable t = table.Select("PackageID = " + packageid).CopyToDataTable();
                    ret.Tables.Add(t);
                }
            }
            return ret;
        }

        #region Packages Paged ODS Methods
        DataSet dsPackagesPaged;

        public int GetPackagesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsPackagesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetPackagesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsPackagesPaged = ES.Services.Packages.GetPackagesPaged(PanelSecurity.SelectedUserId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);
            return dsPackagesPaged.Tables[1];
        }
        #endregion

        #region Nested Packages Paged ODS Methods
        DataSet dsNestedPackagesPaged;

        public int GetNestedPackagesPagedCount(int packageId, string filterColumn, string filterValue,
            int statusId, int planId, int serverId)
        {
            return (int)dsNestedPackagesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetNestedPackagesPaged(int packageId, string filterColumn, string filterValue,
            int statusId, int planId, int serverId,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            dsNestedPackagesPaged = ES.Services.Packages.GetNestedPackagesPaged(
                packageId, filterColumn, filterValue, statusId, planId, serverId,
                sortColumn, startRowIndex, maximumRows);
            return dsNestedPackagesPaged.Tables[1];
        }
        #endregion

        #region Service Items Paged ODS Methods
        DataSet dsServiceItemsPaged;

        public int SearchServiceItemsPagedCount(int itemTypeId, string filterValue)
        {
            return (int)dsServiceItemsPaged.Tables[0].Rows[0][0];
        }

        public DataTable SearchServiceItemsPaged(int itemTypeId, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            dsServiceItemsPaged = ES.Services.Packages.SearchServiceItemsPaged(PanelSecurity.EffectiveUserId,
                itemTypeId, "%" + filterValue + "%", sortColumn, startRowIndex, maximumRows);
            return dsServiceItemsPaged.Tables[1];
        }
        #endregion

        //TODO START
        #region Service Items Paged Search
        DataSet dsObjectItemsPaged;

        public int SearchObjectItemsPagedCount(string filterColumn, string filterValue, string fullType, string colType)
        {
            return (int)dsObjectItemsPaged.Tables[0].Rows[0][0];
        }

        public DataTable SearchObjectItemsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue, string colType, string fullType)
        {
            dsObjectItemsPaged = ES.Services.Packages.GetSearchObject(PanelSecurity.EffectiveUserId, filterColumn,
                String.Format("%{0}%", filterValue),
                0, 0, sortColumn, startRowIndex, maximumRows, colType, fullType);
            return dsObjectItemsPaged.Tables[2];
        }

        public DataTable SearchObjectTypes(string filterColumn, string filterValue, string fullType, string sortColumn)
        {
            dsObjectItemsPaged = ES.Services.Packages.GetSearchObject(PanelSecurity.EffectiveUserId, filterColumn,
                String.Format("%{0}%", filterValue),
                0, 0, sortColumn, 0, 0, "",fullType);
            return dsObjectItemsPaged.Tables[1];
        }
        //TODO END
        #endregion
    }
}

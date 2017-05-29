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

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for ServerItemsHelper
    /// </summary>
    public class ServiceItemsHelper
    {
        #region Web Sites
        DataSet dsItemsPaged;

        public int GetServiceItemsPagedCount(int packageId, string groupName, string typeName,
            int serverId, bool recursive, string filterColumn, string filterValue)
        {
            return (int)dsItemsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetServiceItemsPaged(int packageId, string groupName, string typeName,
            int serverId, bool recursive, string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            dsItemsPaged = ES.Services.Packages.GetRawPackageItemsPaged(packageId, groupName, typeName, serverId,
                recursive, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsItemsPaged.Tables[1];
        }
        #endregion

        #region Web Sites
        DataSet dsWebSitesPaged;

        public int GetWebSitesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsWebSitesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetWebSitesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsWebSitesPaged = ES.Services.WebServers.GetRawWebSitesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsWebSitesPaged.Tables[1];
        }
        #endregion

        #region Ftp Accounts
        DataSet dsFtpAccountsPaged;

        public int GetFtpAccountsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsFtpAccountsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetFtpAccountsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsFtpAccountsPaged = ES.Services.FtpServers.GetRawFtpAccountsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsFtpAccountsPaged.Tables[1];
        }
        #endregion

        #region Mail Accounts
        DataSet dsMailAccountsPaged;

        public int GetMailAccountsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailAccountsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailAccountsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailAccountsPaged = ES.Services.MailServers.GetRawMailAccountsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailAccountsPaged.Tables[1];
        }
        #endregion

        #region Mail Forwardings
        DataSet dsMailForwardingsPaged;

        public int GetMailForwardingsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailForwardingsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailForwardingsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailForwardingsPaged = ES.Services.MailServers.GetRawMailForwardingsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailForwardingsPaged.Tables[1];
        }
        #endregion

        #region Mail Groups
        DataSet dsMailGroupsPaged;

        public int GetMailGroupsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailGroupsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailGroupsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailGroupsPaged = ES.Services.MailServers.GetRawMailGroupsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailGroupsPaged.Tables[1];
        }
        #endregion

        #region Mail Lists
        DataSet dsMailListsPaged;

        public int GetMailListsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailListsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailListsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailListsPaged = ES.Services.MailServers.GetRawMailListsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailListsPaged.Tables[1];
        }
        #endregion

        #region Mail Domains
        DataSet dsMailDomainsPaged;

        public int GetMailDomainsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsMailDomainsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetMailDomainsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsMailDomainsPaged = ES.Services.MailServers.GetRawMailDomainsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsMailDomainsPaged.Tables[1];
        }
        #endregion

        #region Databases
        DataSet dsSqlDatabasesPaged;

        public int GetSqlDatabasesPagedCount(string groupName, string filterColumn, string filterValue)
        {
            return (int)dsSqlDatabasesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSqlDatabasesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string groupName, string filterColumn, string filterValue)
        {
            dsSqlDatabasesPaged = ES.Services.DatabaseServers.GetRawSqlDatabasesPaged(PanelSecurity.PackageId,
                groupName, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsSqlDatabasesPaged.Tables[1];
        }
        #endregion

        #region Database Users
        DataSet dsSqlUsersPaged;

        public int GetSqlUsersPagedCount(string groupName, string filterColumn, string filterValue)
        {
            return (int)dsSqlUsersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSqlUsersPaged(int maximumRows, int startRowIndex, string sortColumn,
            string groupName, string filterColumn, string filterValue)
        {
            dsSqlUsersPaged = ES.Services.DatabaseServers.GetRawSqlUsersPaged(PanelSecurity.PackageId,
                groupName, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsSqlUsersPaged.Tables[1];
        }
        #endregion

        #region SharePoint Users
        DataSet dsSharePointUsersPaged;

        public int GetSharePointUsersPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharePointUsersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharePointUsersPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharePointUsersPaged = ES.Services.SharePointServers.GetRawSharePointUsersPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharePointUsersPaged.Tables[1];
        }
        #endregion

        #region SharePoint Groups
        DataSet dsSharePointGroupsPaged;

        public int GetSharePointGroupsPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharePointGroupsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharePointGroupsPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharePointGroupsPaged = ES.Services.SharePointServers.GetRawSharePointGroupsPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharePointGroupsPaged.Tables[1];
        }
        #endregion

        #region Statistics Items
        DataSet dsStatisticsItemsPaged;

        public int GetStatisticsSitesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsStatisticsItemsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetStatisticsSitesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsStatisticsItemsPaged = ES.Services.StatisticsServers.GetRawStatisticsSitesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsStatisticsItemsPaged.Tables[1];
        }
        #endregion

        #region SharePoint Sites
        DataSet dsSharePointSitesPaged;

        public int GetSharePointSitesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharePointSitesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharePointSitesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharePointSitesPaged = ES.Services.SharePointServers.GetRawSharePointSitesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharePointSitesPaged.Tables[1];
        }
        #endregion

        #region ODBC DSNs
        DataSet dsOdbcSourcesPaged;

        public int GetOdbcSourcesPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsOdbcSourcesPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetOdbcSourcesPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsOdbcSourcesPaged = ES.Services.OperatingSystems.GetRawOdbcSourcesPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsOdbcSourcesPaged.Tables[1];
        }
        #endregion

        #region Shared SSL Folders
        DataSet dsSharedSSLFoldersPaged;

        public int GetSharedSSLFoldersPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsSharedSSLFoldersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetSharedSSLFoldersPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsSharedSSLFoldersPaged = ES.Services.WebServers.GetRawSSLFoldersPaged(PanelSecurity.PackageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);

            return dsSharedSSLFoldersPaged.Tables[1];
        }
        #endregion
    }
}

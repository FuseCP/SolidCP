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
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;

using SolidCP.Providers.Mail;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esMailServers : System.Web.Services.WebService
    {
        #region Mail Accounts

        [WebMethod]
        public DataSet GetRawMailAccountsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailAccountsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailAccount> GetMailAccounts(int packageId, bool recursive)
        {
            return MailServerController.GetMailAccounts(packageId, recursive);
        }

        [WebMethod]
        public MailAccount GetMailAccount(int itemId)
        {
            return MailServerController.GetMailAccount(itemId);
        }

        [WebMethod]
        public int AddMailAccount(MailAccount item)
        {
            return MailServerController.AddMailAccount(item);
        }

        [WebMethod]
        public int UpdateMailAccount(MailAccount item)
        {
            return MailServerController.UpdateMailAccount(item);
        }

        [WebMethod]
        public int DeleteMailAccount(int itemId)
        {
            return MailServerController.DeleteMailAccount(itemId);
        }
        #endregion

        #region Mail Forwardings
        [WebMethod]
        public DataSet GetRawMailForwardingsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailForwardingsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailAlias> GetMailForwardings(int packageId, bool recursive)
        {
            return MailServerController.GetMailForwardings(packageId, recursive);
        }

        [WebMethod]
        public MailAlias GetMailForwarding(int itemId)
        {
            return MailServerController.GetMailForwarding(itemId);
        }

        [WebMethod]
        public int AddMailForwarding(MailAlias item)
        {
            return MailServerController.AddMailForwarding(item);
        }

        [WebMethod]
        public int UpdateMailForwarding(MailAlias item)
        {
            return MailServerController.UpdateMailForwarding(item);
        }

        [WebMethod]
        public int DeleteMailForwarding(int itemId)
        {
            return MailServerController.DeleteMailForwarding(itemId);
        }
        #endregion

        #region Mail Groups
        [WebMethod]
        public DataSet GetRawMailGroupsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailGroupsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailGroup> GetMailGroups(int packageId, bool recursive)
        {
            return MailServerController.GetMailGroups(packageId, recursive);
        }

        [WebMethod]
        public MailGroup GetMailGroup(int itemId)
        {
            return MailServerController.GetMailGroup(itemId);
        }

        [WebMethod]
        public int AddMailGroup(MailGroup item)
        {
            return MailServerController.AddMailGroup(item);
        }

        [WebMethod]
        public int UpdateMailGroup(MailGroup item)
        {
            return MailServerController.UpdateMailGroup(item);
        }

        [WebMethod]
        public int DeleteMailGroup(int itemId)
        {
            return MailServerController.DeleteMailGroup(itemId);
        }
        #endregion

        #region Mail Lists
        [WebMethod]
        public DataSet GetRawMailListsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailListsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailList> GetMailLists(int packageId, bool recursive)
        {
            return MailServerController.GetMailLists(packageId, recursive);
        }

        [WebMethod]
        public MailList GetMailList(int itemId)
        {
            return MailServerController.GetMailList(itemId);
        }

        [WebMethod]
        public int AddMailList(MailList item)
        {
            return MailServerController.AddMailList(item);
        }

        [WebMethod]
        public int UpdateMailList(MailList item)
        {
            return MailServerController.UpdateMailList(item);
        }

        [WebMethod]
        public int DeleteMailList(int itemId)
        {
            return MailServerController.DeleteMailList(itemId);
        }
        #endregion

        #region Mail Domains
        [WebMethod]
        public DataSet GetRawMailDomainsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return MailServerController.GetRawMailDomainsPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<MailDomain> GetMailDomains(int packageId, bool recursive)
        {
            return MailServerController.GetMailDomains(packageId, recursive);
        }

        [WebMethod]
        public List<DomainInfo> GetMailDomainPointers(int itemId)
        {
            return MailServerController.GetMailDomainPointers(itemId);
        }

        [WebMethod]
        public MailDomain GetMailDomain(int itemId)
        {
            return MailServerController.GetMailDomain(itemId);
        }

        [WebMethod]
        public int AddMailDomain(MailDomain item)
        {
            return MailServerController.AddMailDomain(item);
        }

        [WebMethod]
        public int UpdateMailDomain(MailDomain item)
        {
            return MailServerController.UpdateMailDomain(item);
        }

        [WebMethod]
        public int DeleteMailDomain(int itemId)
        {
            return MailServerController.DeleteMailDomain(itemId);
        }

        [WebMethod]
        public int AddMailDomainPointer(int itemId, int domainId)
        {
            return MailServerController.AddMailDomainPointer(itemId, domainId);
        }

        [WebMethod]
        public int DeleteMailDomainPointer(int itemId, int domainId)
        {
            return MailServerController.DeleteMailDomainPointer(itemId, domainId);
        }
        #endregion
    }
}

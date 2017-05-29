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
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class GlobalDnsRecordsControl : SolidCPControlBase
    {
        public string IPServerIdParam
        {
            get { return ipAddress.ServerIdParam; }
            set { ipAddress.ServerIdParam = value; }
        }

        private string serverIdParam;
        public string ServerIdParam
        {
            get { return serverIdParam; }
            set { serverIdParam = value; }
        }

        private string serviceIdParam;
        public string ServiceIdParam
        {
            get { return serviceIdParam; }
            set { serviceIdParam = value; }
        }

        private string packageIdParam;
        public string PackageIdParam
        {
            get { return packageIdParam; }
            set { packageIdParam = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowPanels(false);

                try
                {
                    BindDnsRecords();
                }
                catch (Exception ex)
                {
                    HostModule.ShowErrorMessage("GDNS_GET_RECORD", ex);
                    return;
                }
            }

        }

        private void BindDnsRecords()
        {
            DataSet ds = null;

            if (ServiceIdParam != null)
                ds = ES.Services.Servers.GetRawDnsRecordsByService(Utils.ParseInt(Request[ServiceIdParam], 0));
            else if (ServerIdParam != null)
                ds = ES.Services.Servers.GetRawDnsRecordsByServer(Utils.ParseInt(Request[ServerIdParam], 0));
            else if (PackageIdParam != null)
                ds = ES.Services.Servers.GetRawDnsRecordsByPackage(Utils.ParseInt(Request[PackageIdParam], 0));

            if (ds != null)
            {
                gvRecords.DataSource = ds;
                gvRecords.DataBind();
            }

            ToggleRecordControls();
        }

        private void BindDnsRecord(int recordId)
        {
            try
            {
                ViewState["RecordID"] = recordId;

                GlobalDnsRecord record = ES.Services.Servers.GetDnsRecord(recordId);
                if (record != null)
                {
                    ddlRecordType.SelectedValue = record.RecordType;
                    txtRecordName.Text = record.RecordName;
                    txtRecordData.Text = record.RecordData;
                    txtMXPriority.Text = record.MxPriority.ToString();
                    txtSRVPriority.Text = record.SrvPriority.ToString();
                    txtSRVWeight.Text = record.SrvWeight.ToString();
                    txtSRVPort.Text = record.SrvPort.ToString();
                    ipAddress.AddressId = record.IpAddressId;
                }

                ToggleRecordControls();
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("GDNS_GET_RECORD", ex);
                return;
            }
        }

        protected void ddlRecordType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleRecordControls();
        }

        private void ToggleRecordControls()
        {
            rowMXPriority.Visible = false;
            rowSRVPriority.Visible = false;
            rowSRVWeight.Visible = false;
            rowSRVPort.Visible = false;
            lblRecordData.Text = "Record Data:";
            ipAddress.Visible = false;

            switch (ddlRecordType.SelectedValue)
            {
                case "A":
                    lblRecordData.Text = "IP:";
                    ipAddress.Visible = true;
                    break;
                case "AAAA":
                    lblRecordData.Text = "IP (v6):";
                    ipAddress.Visible = true;
                    break;
                case "MX":
                    rowMXPriority.Visible = true;
                    break;
                case "SRV":
                    rowSRVPriority.Visible = true;
                    rowSRVWeight.Visible = true;
                    rowSRVPort.Visible = true;

                    lblRecordData.Text = "Host offering this service:";
                    break;
                default:
                    break;
            }
        }
		protected void Validate(object source, ServerValidateEventArgs args) {
/*
			var ip = args.Value;
			System.Net.IPAddress ipaddr;
            if (string.IsNullOrEmpty(args.Value))
                args.IsValid = true;
            else
			    args.IsValid = System.Net.IPAddress.TryParse(ip, out ipaddr) && (ip.Contains(":") || ip.Contains(".")) && 
                    ((ddlRecordType.SelectedValue == "A" && ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) ||
                    (ddlRecordType.SelectedValue == "AAAA" && ipaddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6));
*/
            args.IsValid = true;
		}

        private void SaveRecord()
        {
            if (!string.IsNullOrEmpty(txtRecordData.Text))
			    if (!Page.IsValid) return;

            GlobalDnsRecord record = new GlobalDnsRecord();
            record.RecordId = (int)ViewState["RecordID"];
            record.RecordType = ddlRecordType.SelectedValue;
            record.RecordName = txtRecordName.Text.Trim();
            record.RecordData = txtRecordData.Text.Trim();
            record.MxPriority = Utils.ParseInt(txtMXPriority.Text, 0);
            record.SrvPriority = Utils.ParseInt(txtSRVPriority.Text, 0);
            record.SrvWeight = Utils.ParseInt(txtSRVWeight.Text, 0);
            record.SrvPort = Utils.ParseInt(txtSRVPort.Text, 0);
            record.IpAddressId = ipAddress.AddressId;

            if (ServiceIdParam != null)
                record.ServiceId = Utils.ParseInt(Request[ServiceIdParam], 0);
            else if (ServerIdParam != null)
                record.ServerId = Utils.ParseInt(Request[ServerIdParam], 0);
            else if (PackageIdParam != null)
                record.PackageId = Utils.ParseInt(Request[PackageIdParam], 0);

            if (record.RecordId == 0)
            {
                // add record
                try
                {
                    int result = ES.Services.Servers.AddDnsRecord(record);
                    if (result < 0)
                    {
                        HostModule.ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    HostModule.ShowErrorMessage("GDNS_ADD_RECORD", ex);
                    return;
                }
            }
            else
            {
                // update record
                try
                {
                    int result = ES.Services.Servers.UpdateDnsRecord(record);
                    if (result < 0)
                    {
                        HostModule.ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    HostModule.ShowErrorMessage("GDNS_UPDATE_RECORD", ex);
                    return;
                }
            }

            // rebind and switch
            BindDnsRecords();
            ShowPanels(false);
        }

        private void DeleteRecord(int recordId)
        {
            try
            {
                int result = ES.Services.Servers.DeleteDnsRecord(recordId);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("GDNS_DELETE_RECORD", ex);
                return;
            }
            BindDnsRecords();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ViewState["RecordID"] = 0;

            // erase fields
            ddlRecordType.SelectedIndex = 0;
            txtRecordName.Text = "";
            txtRecordData.Text = "";
            txtMXPriority.Text = "0";
            txtSRVPriority.Text = "0";
            txtSRVWeight.Text = "0";
            txtSRVPort.Text = "0";

            ToggleRecordControls();

            ShowPanels(true);
        }
        private void ShowPanels(bool editMode)
        {
            pnlEdit.Visible = editMode;
            pnlRecords.Visible = !editMode;
            gvRecords.Visible = !editMode;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ShowPanels(false);
        }
        protected void gvRecords_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ShowPanels(true);
            int recordId = (int)gvRecords.DataKeys[e.NewEditIndex].Value;
            BindDnsRecord(recordId);
        }
        protected void gvRecords_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int recordId = (int)gvRecords.DataKeys[e.RowIndex].Value;
            DeleteRecord(recordId);
        }
    }
}

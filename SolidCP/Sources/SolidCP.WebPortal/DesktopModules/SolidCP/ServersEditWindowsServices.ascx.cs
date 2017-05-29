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

using SolidCP.Server;

namespace SolidCP.Portal
{
    public partial class ServersEditWindowsServices : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindServices();
        }

        private void BindServices()
        {
            try
            {
                gvServices.DataSource = ES.Services.Servers.GetWindowsServices(PanelRequest.ServerId);
                gvServices.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_GET_WIN_SERVICES", ex);
                return;
            }
        }

        protected void gvServices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            WindowsServiceStatus status = (WindowsServiceStatus)Enum.Parse(typeof(WindowsServiceStatus), e.CommandName, true);
            string id = (string)e.CommandArgument;

            try
            {
                int result = ES.Services.Servers.ChangeWindowsServiceStatus(PanelRequest.ServerId, id, status);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // rebind
                BindServices();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_CHANGE_WIN_SERVICE_STATE", ex);
                return;
            }
        }

        protected void gvServices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WindowsService serv = (WindowsService)e.Row.DataItem;
            ImageButton cmdStart = (ImageButton)e.Row.FindControl("cmdStart");
            ImageButton cmdPause = (ImageButton)e.Row.FindControl("cmdPause");
            ImageButton cmdContinue = (ImageButton)e.Row.FindControl("cmdContinue");
            ImageButton cmdStop = (ImageButton)e.Row.FindControl("cmdStop");

            if (cmdStart == null)
                return;

            cmdStart.Visible = (serv.Status == WindowsServiceStatus.Stopped);
            cmdPause.Visible = (serv.Status == WindowsServiceStatus.Running && serv.CanPauseAndContinue);
            cmdContinue.Visible = (serv.Status == WindowsServiceStatus.Paused && serv.CanPauseAndContinue);
            cmdStop.Visible = (serv.Status == WindowsServiceStatus.Running
                || serv.Status == WindowsServiceStatus.Paused
                && serv.CanStop);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
        }
    }
}

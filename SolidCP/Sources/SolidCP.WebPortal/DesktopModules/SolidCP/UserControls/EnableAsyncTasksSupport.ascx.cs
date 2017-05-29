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

namespace SolidCP.Portal
{
	public partial class EnableAsyncTasksSupport : SolidCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes["onsubmit"] += "return ShowProgressDialogInternal();";

            // get task ID from request and place it to context
            Context.Items["SolidCPAtlasTaskID"] = taskID.Value;
        }

		public string GetAjaxUtilsUrl()
		{
			return Page.ClientScript.GetWebResourceUrl(
				typeof(EnableAsyncTasksSupport), "SolidCP.Portal.Scripts.AjaxUtils.js");
		}

        protected override void OnPreRender(EventArgs e)
        {
            // call base handler
            base.OnPreRender(e);

            // check if async task was runned
            string asyncScript = "";
            string asyncTaskID = (string)Context.Items["SolidCPAtlasAsyncTaskID"];
            if (!String.IsNullOrEmpty(asyncTaskID))
            {
				string taskTitle = (string)Context.Items["SolidCPAtlasAsyncTaskTitle"];
				if (String.IsNullOrEmpty(taskTitle))
					taskTitle = GetLocalizedString("Text.GenericTitle");

				asyncScript = "ShowProgressDialogAsync('" + asyncTaskID + "', '" + taskTitle + "');";
            }
            else
            {
                string url = (string)Context.Items["RedirectUrl"];

                if (!String.IsNullOrWhiteSpace(url))
                {
                    Response.Redirect(url);
                }
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "Atlas", @"<script>
        function pageLoad()
        {
            " + asyncScript + @"
            ShowProgressDialogInternal();
        }
    </script>
");

            // generate new task ID
            taskID.Value = Guid.NewGuid().ToString("N");
        }
    }
}

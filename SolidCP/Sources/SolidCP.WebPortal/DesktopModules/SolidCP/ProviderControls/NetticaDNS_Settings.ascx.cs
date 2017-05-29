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
using System.Collections.Specialized;
using SolidCP.Providers.Common;

namespace SolidCP.Portal.ProviderControls
{
    public partial class NetticaDNS_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        //public const string Password = "Password";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindSettings(StringDictionary settings)
        {
            txtPassword.Text = settings[Constants.Password];
            ViewState["PWD"] = settings[Constants.Password];
            rowPassword.Visible = ((string)ViewState["PWD"]) != null;
            txtUserName.Text = settings[Constants.UserName];
            secondaryDNSServers.BindSettings(settings);            
            iPAddressesList.BindSettings(settings);
            cbApplyDefaultTemplate.Checked = Utils.ParseBool(settings["ApplyDefaultTemplate"], false);
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings[Constants.UserName] = txtUserName.Text;
            //settings[Constants.Password] = string.IsNullOrEmpty(txtPassword.Text) ? (ViewState[Password] != null ? ViewState[Password].ToString(): string.Empty): txtPassword.Text;
            settings[Constants.Password] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            secondaryDNSServers.SaveSettings(settings);
            iPAddressesList.SaveSettings(settings);
            settings["ApplyDefaultTemplate"] = cbApplyDefaultTemplate.Checked.ToString();
        }
    }
}

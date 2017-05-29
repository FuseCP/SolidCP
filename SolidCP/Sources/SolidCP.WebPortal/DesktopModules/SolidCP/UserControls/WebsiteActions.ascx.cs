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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Web.Services3.Referral;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Portal.UserControls;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal
{
    public enum WebsiteActionTypes
    {
        None = 0,
        Stop = 1,
        Start = 2,
        RestartAppPool = 3,
    }

    public partial class WebsiteActions : ActionListControlBase<WebsiteActionTypes>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override DropDownList ActionsList
        {
            get { return ddlWebsiteActions; }
        }

        protected override int DoAction(List<int> ids)
        {
            switch (SelectedAction)
            {
                case WebsiteActionTypes.Stop:
                    return ChangeWebsiteState(false, ids);
                case WebsiteActionTypes.Start:
                    return ChangeWebsiteState(true, ids);
                case WebsiteActionTypes.RestartAppPool:
                    return RestartAppPool(ids);
            }

            return 0;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            switch (SelectedAction)
            {
                case WebsiteActionTypes.Stop:
                case WebsiteActionTypes.Start:
                case WebsiteActionTypes.RestartAppPool:
                    FireExecuteAction();
                    break;
            }
        }

        private int ChangeWebsiteState(bool enable, List<int> ids)
        {
            foreach (var id in ids)
            {
                var state = enable ? ServerState.Started : ServerState.Paused;
                int result = ES.Services.WebServers.ChangeSiteState(id, state);

                if (result < 0)
                    return result;
            }

            return 0;
        }

        private int RestartAppPool(List<int> ids)
        {
            foreach (var id in ids)
            {
                int result = ES.Services.WebServers.ChangeAppPoolState(id, AppPoolState.Recycle);

                if (result < 0)
                    return result;
            }

            return 0;
        }
    }
}

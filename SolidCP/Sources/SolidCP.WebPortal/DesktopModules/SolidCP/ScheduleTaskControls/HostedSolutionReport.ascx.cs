// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
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
using SolidCP.EnterpriseServer;
using SolidCP.Portal.UserControls.ScheduleTaskView;

namespace SolidCP.Portal.ScheduleTaskControls
{
    public partial class HostedSolutionReport : EmptyView
    {
        private static readonly string EXCHANGE_REPORT = "EXCHANGE_REPORT";
        private static readonly string ORGANIZATION_REPORT = "ORGANIZATION_REPORT";
        private static readonly string SHAREPOINT_REPORT = "SHAREPOINT_REPORT";
        private static readonly string LYNC_REPORT = "LYNC_REPORT";
        private static readonly string SFB_REPORT = "SFB_REPORT";
        private static readonly string CRM_REPORT = "CRM_REPORT";
        private static readonly string EMAIL = "EMAIL";

        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
        {
            base.SetParameters(parameters);
            SetParameter(cbExchange, EXCHANGE_REPORT);
            SetParameter(cbSharePoint, SHAREPOINT_REPORT);
            SetParameter(cbLync, LYNC_REPORT);
            SetParameter(cbSfB, SFB_REPORT);
            SetParameter(cbCRM, CRM_REPORT);
            SetParameter(cbOrganization, ORGANIZATION_REPORT);
            SetParameter(txtMail, EMAIL);         

        }

        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo exchange = GetParameter(cbExchange, EXCHANGE_REPORT);
            ScheduleTaskParameterInfo sharepoint = GetParameter(cbSharePoint, SHAREPOINT_REPORT);
            ScheduleTaskParameterInfo lync = GetParameter(cbLync, LYNC_REPORT);
            ScheduleTaskParameterInfo sfb = GetParameter(cbSfB, SFB_REPORT);
            ScheduleTaskParameterInfo crm = GetParameter(cbCRM, CRM_REPORT);
            ScheduleTaskParameterInfo organization = GetParameter(cbOrganization, ORGANIZATION_REPORT);
            ScheduleTaskParameterInfo email = GetParameter(txtMail, EMAIL);


            return new ScheduleTaskParameterInfo[7] { exchange, sharepoint, lync, sfb, crm, organization, email};
        }
    }
}

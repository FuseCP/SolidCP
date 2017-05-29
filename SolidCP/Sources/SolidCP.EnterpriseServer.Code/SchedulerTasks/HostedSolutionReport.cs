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

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.EnterpriseServer
{
    public class HostedSolutionReportTask : SchedulerTask
    {
        private static readonly string EXCHANGE_REPORT = "EXCHANGE_REPORT";
        private static readonly string ORGANIZATION_REPORT = "ORGANIZATION_REPORT";
        private static readonly string SHAREPOINT_REPORT = "SHAREPOINT_REPORT";
        private static readonly string LYNC_REPORT = "LYNC_REPORT";
        private static readonly string SFB_REPORT = "SFB_REPORT";
        private static readonly string CRM_REPORT = "CRM_REPORT";
        private static readonly string EMAIL = "EMAIL";
        
        
        public override void DoWork()
        {
            try
            {
                TaskManager.Write("Start HostedSolutionReportTask");

                BackgroundTask topTask = TaskManager.TopTask;

                bool isExchange = Utils.ParseBool(topTask.GetParamValue(EXCHANGE_REPORT), false);
                bool isSharePoint = Utils.ParseBool(topTask.GetParamValue(SHAREPOINT_REPORT), false);
                bool isLync = Utils.ParseBool(topTask.GetParamValue(LYNC_REPORT), false);
                bool isSfB = Utils.ParseBool(topTask.GetParamValue(SFB_REPORT), false);
                bool isCRM = Utils.ParseBool(topTask.GetParamValue(CRM_REPORT), false);
                bool isOrganization = Utils.ParseBool(topTask.GetParamValue(ORGANIZATION_REPORT), false);

                string email = topTask.GetParamValue(EMAIL).ToString();

                TaskManager.WriteParameter("isExchange",isExchange);
                TaskManager.WriteParameter("isSharePoint",isSharePoint);
                TaskManager.WriteParameter("isLync", isLync);
                TaskManager.WriteParameter("isSfB", isSfB);
                TaskManager.WriteParameter("isCRM", isCRM);
                TaskManager.WriteParameter("isOrganization", isOrganization);
                TaskManager.WriteParameter("email", email);

                UserInfo user = PackageController.GetPackageOwner(topTask.PackageId);

                TaskManager.WriteParameter("user", user.Username);

                EnterpriseSolutionStatisticsReport report =
                ReportController.GetEnterpriseSolutionStatisticsReport(user.UserId, isExchange, isSharePoint, isCRM,isOrganization, isLync, isSfB);

                TaskManager.WriteParameter("report.ExchangeReport.Items.Count", report.ExchangeReport.Items.Count);
                TaskManager.WriteParameter("report.SharePointReport.Items.Count", report.SharePointReport.Items.Count);
                TaskManager.WriteParameter("report.CRMReport.Items.Count", report.CRMReport.Items.Count);
                TaskManager.WriteParameter("report.OrganizationReport.Items.Count", report.OrganizationReport.Items.Count);
                TaskManager.WriteParameter("report.LyncReport.Items.Count", report.LyncReport.Items.Count);
                TaskManager.WriteParameter("report.SfBReport.Items.Count", report.SfBReport.Items.Count);

                SendMessage(user, email, isExchange && report.ExchangeReport != null ? report.ExchangeReport.ToCSV() : string.Empty,
                            isSharePoint && report.SharePointReport != null ? report.SharePointReport.ToCSV() : string.Empty,
                            isCRM && report.CRMReport != null ? report.CRMReport.ToCSV() : string.Empty,
                            isOrganization && report.OrganizationReport != null ? report.OrganizationReport.ToCSV() : string.Empty,
                            isLync && report.LyncReport != null ? report.LyncReport.ToCSV() : string.Empty,
                            isSfB && report.SfBReport != null ? report.SfBReport.ToCSV() : string.Empty);

            }
            catch(Exception ex)
            {
                TaskManager.WriteError(ex);
            }

            TaskManager.Write("End HostedSolutionReportTask");

        }

        
        private static void PrepareAttament(string name, string csv, List<Attachment> attacments)
        {
            if (!string.IsNullOrEmpty(csv))
            {
                UTF8Encoding encoding = new UTF8Encoding();
                
                byte[] buffer = encoding.GetBytes(csv);
                MemoryStream stream = new MemoryStream(buffer);
                Attachment attachment = new Attachment(stream, name, MediaTypeNames.Text.Plain);

                attacments.Add(attachment);
            }
        }
        
        private void SendMessage(UserInfo user,string email, string exchange_csv, string sharepoint_csv, string crm_csv, string organization_csv, string lync_csv, string sfb_csv)
        {
            TaskManager.Write("SendMessage");

            List<Attachment> attacments = new List<Attachment>();
            PrepareAttament("exchange.csv", exchange_csv, attacments);
            PrepareAttament("sharepoint.csv", sharepoint_csv, attacments);
            PrepareAttament("lync.csv", lync_csv, attacments);
            PrepareAttament("sfb.csv", sfb_csv, attacments);
            PrepareAttament("crm.csv", crm_csv, attacments);
            PrepareAttament("organization.csv", organization_csv, attacments);
            
            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.HOSTED_SOLUTION_REPORT);

            string from = settings["From"];
            string cc = settings["CC"];
            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;

            TaskManager.WriteParameter("from", from);
            TaskManager.WriteParameter("email", email);
            TaskManager.WriteParameter("subject", subject);
            TaskManager.WriteParameter("body", body);
          
            
            int res = MailHelper.SendMessage(from, email, cc,  subject, body, priority, isHtml, attacments.ToArray());

            if (res==0)
            {
                TaskManager.Write("SendMessage OK");
            }
            else
            {
                TaskManager.WriteError("SendMessage error ", "error code", res.ToString());
            }

            TaskManager.WriteParameter("", res);

            TaskManager.Write("End SendMessage");

            
        }
    }
}

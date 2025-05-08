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

using SolidCP.Providers.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Net.Http;

namespace SolidCP.EnterpriseServer
{
    public class CheckWebsitesSslTask : SchedulerTask
    {
        private readonly string domainVariableKey = "[domain]";
        private readonly string urlVariableKey = "[url]";
        private readonly string issuerVariableKey = "[issuer]";
        private readonly string expiresInDaysVariableKey = "[expires_in_days]";
        private readonly string expiresOnDateVariableKey = "[expires_on_date]";
        private readonly string errorVariableKey = "[error]";

        private bool mailToCustomer;
        private bool sendBcc;
        private string bccMail;
        private string expirationMailSubject;
        private string expirationMailBody;
        private bool send30DaysBeforeExpiration;
        private bool send14DaysBeforeExpiration;
        private bool sendTodayExpired;
        private bool sendSslError;
        private string errorMailSubject;
        private string errorMailBody;
        private string mailFrom;

        public override void DoWork()
        {
            BackgroundTask topTask = TaskManager.TopTask;

            // get input parameters
            mailToCustomer = Convert.ToBoolean(topTask.GetParamValue("SEND_MAIL_TO_CUSTOMER"));
            sendBcc = Convert.ToBoolean(topTask.GetParamValue("SEND_BCC"));
            bccMail = (string)topTask.GetParamValue("BCC_MAIL");
            expirationMailSubject = (string)topTask.GetParamValue("EXPIRATION_MAIL_SUBJECT");
            expirationMailBody = (string)topTask.GetParamValue("EXPIRATION_MAIL_BODY");
            send30DaysBeforeExpiration = Convert.ToBoolean(topTask.GetParamValue("SEND_30_DAYS_BEFORE_EXPIRATION"));
            send14DaysBeforeExpiration = Convert.ToBoolean(topTask.GetParamValue("SEND_14_DAYS_BEFORE_EXPIRATION"));
            sendTodayExpired = Convert.ToBoolean(topTask.GetParamValue("SEND_TODAY_EXPIRED"));
            sendSslError = Convert.ToBoolean(topTask.GetParamValue("SEND_SSL_ERROR"));
            errorMailSubject = (string)topTask.GetParamValue("ERROR_MAIL_SUBJECT");
            errorMailBody = (string)topTask.GetParamValue("ERROR_MAIL_BODY");

            // check input parameters
            if (sendBcc && String.IsNullOrEmpty(bccMail))
            {
                TaskManager.WriteWarning("Specify 'BCC Mail To' task parameter");
                sendBcc = false;
            }
            if (!mailToCustomer && !sendBcc)
            {
                TaskManager.WriteWarning("Set 'Send Mail To Customer' or 'BCC Mail To' task parameter");
                return;
            }
            if (send30DaysBeforeExpiration || send14DaysBeforeExpiration || sendTodayExpired)
            {
                if (String.IsNullOrEmpty(expirationMailSubject))
                {
                    TaskManager.WriteWarning("Set 'Expiration Mail Subject' task parameter");
                    return;
                }
                if (String.IsNullOrEmpty(expirationMailBody))
                {
                    TaskManager.WriteWarning("Set 'Expiration Mail Body' task parameter");
                    return;
                }
            }
            if (sendSslError)
            {
                if (String.IsNullOrEmpty(errorMailSubject))
                {
                    TaskManager.WriteWarning("Set 'Error Mail Subject' task parameter");
                    return;
                }
                if (String.IsNullOrEmpty(errorMailBody))
                {
                    TaskManager.WriteWarning("Set 'Error Mail Body' task parameter");
                    return;
                }
            }

            // get mail from
            SystemSettings settings = SystemController.GetSystemSettingsInternal(SystemSettings.SMTP_SETTINGS, false);
            if (settings != null)
            {
                mailFrom = settings["SmtpUsername"];
            }
            if (String.IsNullOrEmpty(mailFrom))
            {
                TaskManager.WriteWarning("You need to configure SMTP settings first");
                return;
            }

            // get websites
            if (topTask.EffectiveUserId == 1)
            {
                // serveradmin - get all websites
                DataSet serviceItems = PackageController.GetRawPackageItemsPaged(1, ResourceGroups.Web, typeof(WebSite),
                        true, "ItemName", "%%", "", 0, Int32.MaxValue);
                checkWebsites(serviceItems);
            }
            else
            {
                // get user packages
                List<PackageInfo> packages = PackageController.GetMyPackages(topTask.EffectiveUserId);
                foreach (PackageInfo package in packages)
                {
                    DataSet serviceItems = PackageController.GetRawPackageItemsPaged(package.PackageId, ResourceGroups.Web, typeof(WebSite),
                        true, "ItemName", "%%", "", 0, Int32.MaxValue);
                    checkWebsites(serviceItems);
                }
            }
        }

        private void checkWebsites(DataSet serviceItems)
        {
            if (serviceItems == null) return;
            int recordsCount = (int)serviceItems.Tables[0].Rows[0][0];
            if (recordsCount == 0) return;
            DataView dvItems = serviceItems.Tables[1].DefaultView;
            foreach (DataRowView drItem in dvItems)
            {
                string itemTypeName = (string)drItem["TypeName"];
                Type itemType = Type.GetType(itemTypeName);
                if (!typeof(WebSite).Equals(itemType)) continue;
                string domain = (string)drItem["ItemName"];
                if (String.IsNullOrEmpty(domain)) continue;
                string url = "https://" + domain;
                string email = (string)drItem["Email"];

                var varList = new List<KeyValuePair<string, string>>();
                varList.Add(new KeyValuePair<string, string>(domainVariableKey, domain));
                varList.Add(new KeyValuePair<string, string>(urlVariableKey, url));

                var task = GetServerCertificateAsync(url, HttpMethod.Head);
                task.Wait();
                X509Certificate2 cert = task.Result.Certificate;
                if (cert == null)
                {
                    task = GetServerCertificateAsync(url, HttpMethod.Get);
                    task.Wait();
                    cert = task.Result.Certificate;
                    if (cert == null)
                    {
                        if (!sendSslError) continue;
                        varList.Add(new KeyValuePair<string, string>(errorVariableKey, task.Result.ErrorMessage));
                        sendEmail(errorMailSubject, errorMailBody, email, varList);
                        continue;
                    }
                }

                string expirationDateString = cert.GetExpirationDateString();
                string issuer = cert.Issuer;
                DateTime expirationDate = DateTime.Parse(expirationDateString);
                string expiresOnDate = expirationDate.ToString("yyyy-MM-dd");
                DateTime current = DateTime.UtcNow.Date;
                int expiresInDays = (expirationDate - current).Days;

                varList.Add(new KeyValuePair<string, string>(issuerVariableKey, issuer));
                varList.Add(new KeyValuePair<string, string>(expiresInDaysVariableKey, expiresInDays.ToString()));
                varList.Add(new KeyValuePair<string, string>(expiresOnDateVariableKey, expiresOnDate));

                if (send30DaysBeforeExpiration && expiresInDays == 30)
                {
                    sendEmail(expirationMailSubject, expirationMailBody, email, varList);
                }
                if (send14DaysBeforeExpiration && expiresInDays == 14)
                {
                    sendEmail(expirationMailSubject, expirationMailBody, email, varList);
                }
                if (sendTodayExpired && expiresInDays == 0)
                {
                    sendEmail(expirationMailSubject, expirationMailBody, email, varList);
                }
            }
        }

        private void sendEmail(string subject, string body, string customerEmail, List<KeyValuePair<string, string>> varList)
        {
            // set variables
            foreach (KeyValuePair<string, string> keyValuePair in varList)
            {
                body = body.Replace(keyValuePair.Key, keyValuePair.Value);
                subject = subject.Replace(keyValuePair.Key, keyValuePair.Value);
            }

            string mailTo = null;
            string bcc = null;

            if (mailToCustomer) mailTo = customerEmail;
            if (sendBcc && String.IsNullOrEmpty(mailTo)) mailTo = bccMail;
            if (sendBcc && !String.IsNullOrEmpty(mailTo)) bcc = bccMail;

            int res = MailHelper.SendMessage(mailFrom, mailTo, bcc, subject, body, true);
            if (res != 0) TaskManager.WriteError("SMTP Error. Code: " + res.ToString());
        }

        private async Task<CheckCertificateResult> GetServerCertificateAsync(string url, HttpMethod httpMethod)
        {
            X509Certificate2 certificate = null;
            HttpResponseMessage httpResponse = null;
            try
            {
                var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, error) =>
                    {
                        certificate = new X509Certificate2(cert.GetRawCertData());
                        return true;
                    }
                };

                var httpClient = new HttpClient(httpClientHandler);
                httpResponse = await httpClient.SendAsync(new HttpRequestMessage(httpMethod, url));
            }
            catch (Exception e)
            {
                string errorMessage = e.InnerException.Message;
                if (httpResponse != null) errorMessage += ", HTTP Response Code: " + httpResponse.StatusCode.ToString();
                return new CheckCertificateResult(certificate, errorMessage);
            }
            return new CheckCertificateResult(certificate, null);
        }

        private class CheckCertificateResult
        {
            private X509Certificate2 certificate;
            private string errorMessage;

            public X509Certificate2 Certificate {
                get
                {
                    return certificate;
                }
            }

            public string ErrorMessage
            {
                get
                {
                    return errorMessage;
                }
            }

            public CheckCertificateResult(X509Certificate2 certificate, string errorMessage)
            {
                this.certificate = certificate;
                this.errorMessage = errorMessage;
            }
        }
    }
}

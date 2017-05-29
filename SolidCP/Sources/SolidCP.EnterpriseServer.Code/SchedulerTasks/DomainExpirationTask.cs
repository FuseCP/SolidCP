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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using SolidCP.Providers.DomainLookup;
using Whois.NET;

namespace SolidCP.EnterpriseServer
{
    public class DomainExpirationTask: SchedulerTask
    {
        private static readonly string TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION";

        // Input parameters:
        private static readonly string DaysBeforeNotify = "DAYS_BEFORE";
        private static readonly string MailToParameter = "MAIL_TO";
        private static readonly string EnableNotification = "ENABLE_NOTIFICATION";
        private static readonly string IncludeNonExistenDomains = "INCLUDE_NONEXISTEN_DOMAINS";


        private static readonly string MailBodyTemplateParameter = "MAIL_BODY";
        private static readonly string MailBodyDomainRecordTemplateParameter = "MAIL_DOMAIN_RECORD";

        public override void DoWork()
        {
            BackgroundTask topTask = TaskManager.TopTask;
            var domainUsers = new Dictionary<int, UserInfo>();
            var checkedDomains = new List<DomainInfo>();
            var expiredDomains = new List<DomainInfo>();
            var nonExistenDomains = new List<DomainInfo>();
            var allDomains = new List<DomainInfo>();
            var allTopLevelDomains = new List<DomainInfo>();

            // get input parameters
            int daysBeforeNotify;
            bool sendEmailNotifcation = Convert.ToBoolean( topTask.GetParamValue(EnableNotification));
            bool includeNonExistenDomains = Convert.ToBoolean(topTask.GetParamValue(IncludeNonExistenDomains));

            // check input parameters
            if (String.IsNullOrEmpty((string)topTask.GetParamValue("MAIL_TO")))
            {
                TaskManager.WriteWarning("The e-mail message has not been sent because 'Mail To' is empty.");
                return;
            }

            int.TryParse((string)topTask.GetParamValue(DaysBeforeNotify), out daysBeforeNotify);

            var user = UserController.GetUser(topTask.EffectiveUserId);

            var packages = GetUserPackages(user.UserId, user.Role);


            foreach (var package in packages)
            {
                var domains = ServerController.GetDomains(package.PackageId);

                allDomains.AddRange(domains);

                domains = domains.Where(x => !x.IsSubDomain && !x.IsDomainPointer).ToList(); //Selecting top-level domains

                allTopLevelDomains.AddRange(domains);

                var domainUser = UserController.GetUser(package.UserId);

                if (!domainUsers.ContainsKey(package.PackageId))
                {
                    domainUsers.Add(package.PackageId, domainUser);
                }

                foreach (var domain in domains)
                {
                    if (checkedDomains.Any(x=> x.DomainId == domain.DomainId))
                    {
                        continue;
                    }

                    checkedDomains.Add(domain);

                    ServerController.UpdateDomainWhoisData(domain);

                    if (CheckDomainExpiration(domain.ExpirationDate, daysBeforeNotify))
                    {
                        expiredDomains.Add(domain);
                    }

                    if (domain.ExpirationDate == null && domain.CreationDate == null)
                    {
                        nonExistenDomains.Add(domain);
                    }

                    Thread.Sleep(100);
                }
            }

            var subDomains = allDomains.Where(x => !checkedDomains.Any(z => z.DomainId == x.DomainId && z.ExpirationDate != null)).GroupBy(p => p.DomainId).Select(g => g.First()).ToList();

            foreach (var subDomain in subDomains)
            {
                var mainDomain = checkedDomains.Where(x => subDomain.DomainId != x.DomainId && subDomain.DomainName.ToLowerInvariant().Contains(x.DomainName.ToLowerInvariant())).OrderByDescending(s => s.DomainName.Length).FirstOrDefault(); ;

                if (mainDomain != null)
                {
                    ServerController.UpdateDomainWhoisData(subDomain, mainDomain.CreationDate, mainDomain.ExpirationDate, mainDomain.RegistrarName);

                    var nonExistenDomain = nonExistenDomains.FirstOrDefault(x => subDomain.DomainId == x.DomainId);

                    if (nonExistenDomain != null)
                    {
                        nonExistenDomains.Remove(nonExistenDomain);
                    }

                    Thread.Sleep(100);
                }
            }

            expiredDomains = expiredDomains.GroupBy(p => p.DomainId).Select(g => g.First()).ToList();

            if (expiredDomains.Count > 0 && sendEmailNotifcation)
            {
                SendMailMessage(user, expiredDomains, domainUsers, nonExistenDomains, includeNonExistenDomains);
            }
        }

        private IEnumerable<PackageInfo> GetUserPackages(int userId,UserRole userRole)
        {
            var packages = new List<PackageInfo>();

            switch (userRole)
            {
                case UserRole.Administrator:
                {
                    packages = ObjectUtils.CreateListFromDataReader<PackageInfo>(DataProvider.GetAllPackages());
                    break;
                }
                default:
                {
                    packages = PackageController.GetMyPackages(userId);
                    break; 
                }
            }

            return packages;
        }

        private bool CheckDomainExpiration(DateTime? date, int daysBeforeNotify)
        {
            if (date == null)
            {
                return false;
            }

            return (date.Value - DateTime.Now).Days < daysBeforeNotify;
        }

        private void SendMailMessage(UserInfo user, IEnumerable<DomainInfo> domains, Dictionary<int, UserInfo> domainUsers, IEnumerable<DomainInfo> nonExistenDomains, bool includeNonExistenDomains)
        {
            BackgroundTask topTask = TaskManager.TopTask;

            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.DOMAIN_EXPIRATION_LETTER);

            string from = settings["From"];

            var bcc = settings["CC"];

            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;
            if (!String.IsNullOrEmpty(settings["Priority"]))
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

            // input parameters
            string mailTo = (string)topTask.GetParamValue("MAIL_TO");

            Hashtable items = new Hashtable();

            items["user"] = user;

            items["Domains"] = domains.Select(x => new { DomainName = x.DomainName, 
                                                         ExpirationDate = x.ExpirationDate  < DateTime.Now ? "Expired" : x.ExpirationDate.ToString(),
                                                         ExpirationDateOrdering = x.ExpirationDate, 
                                                         Registrar = x.RegistrarName,
                                                         Customer = string.Format("{0} {1}", domainUsers[x.PackageId].FirstName, domainUsers[x.PackageId].LastName) })
                                      .OrderBy(x => x.ExpirationDateOrdering).ThenBy(x => x.Customer).ThenBy(x => x.DomainName);
            
            items["IncludeNonExistenDomains"] = includeNonExistenDomains;

            items["NonExistenDomains"] = nonExistenDomains.Select(x => new
            {
                DomainName = x.DomainName,
                Customer = string.Format("{0} {1}", domainUsers[x.PackageId].FirstName, domainUsers[x.PackageId].LastName)
            }).OrderBy(x => x.Customer).ThenBy(x => x.DomainName);


            body = PackageController.EvaluateTemplate(body, items);

            // send mail message
            MailHelper.SendMessage(from, mailTo, bcc, subject, body, priority, isHtml);
        }



    }
}

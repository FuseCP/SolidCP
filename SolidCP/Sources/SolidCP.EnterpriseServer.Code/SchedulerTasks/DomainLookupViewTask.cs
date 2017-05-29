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
using SolidCP.Providers.DNS;
using SolidCP.Providers.DomainLookup;
using SolidCP.Server;

namespace SolidCP.EnterpriseServer
{
    public class DomainLookupViewTask : SchedulerTask
    {
        private static readonly string TaskId = "SCHEDULE_TASK_DOMAIN_LOOKUP";

        // Input parameters:
        private static readonly string DnsServersParameter = "DNS_SERVERS";
        private static readonly string MailToParameter = "MAIL_TO";

        private static readonly string MailBodyTemplateParameter = "MAIL_BODY";
        private static readonly string MailBodyDomainRecordTemplateParameter = "MAIL_DOMAIN_RECORD";
        private static readonly string ServerNameParameter = "SERVER_NAME";
        private static readonly string PauseBetweenQueriesParameter = "PAUSE_BETWEEN_QUERIES";

        private const string MxRecordPattern = @"mail exchanger = (.+)";
        private const string NsRecordPattern = @"nameserver = (.+)";
        private const string DnsTimeOutMessage = @"dns request timed out";
        private const int DnsTimeOutRetryCount = 3;
        
        public override void DoWork()
        {
            BackgroundTask topTask = TaskManager.TopTask;

            List<DomainDnsChanges> domainsChanges = new List<DomainDnsChanges>();
            var domainUsers = new Dictionary<int, UserInfo>();

            // get input parameters
            string dnsServersString = (string)topTask.GetParamValue(DnsServersParameter);
            string serverName = (string)topTask.GetParamValue(ServerNameParameter);

            int pause;

                        // check input parameters
            if (String.IsNullOrEmpty(dnsServersString))
            {
                TaskManager.WriteWarning("Specify 'DNS' task parameter.");
                return;
            }

            if (String.IsNullOrEmpty((string)topTask.GetParamValue("MAIL_TO")))
            {
                TaskManager.WriteWarning("The e-mail message has not been sent because 'Mail To' is empty.");
                return;
            }


            if (!int.TryParse((string)topTask.GetParamValue(PauseBetweenQueriesParameter), out pause))
            {
                TaskManager.WriteWarning("The 'pause between queries' parameter is not valid.");
                return;
            }

            // find server by name
            ServerInfo server = ServerController.GetServerByName(serverName);
            if (server == null)
            {
                TaskManager.WriteWarning(String.Format("Server with the name '{0}' was not found", serverName));
                return;
            }

            WindowsServer winServer = new WindowsServer();
            ServiceProviderProxy.ServerInit(winServer, server.ServerId);

            var user = UserController.GetUser(topTask.UserId);

            var dnsServers = dnsServersString.Split(';');

            var packages = ObjectUtils.CreateListFromDataReader<PackageInfo>(DataProvider.GetAllPackages());


            foreach (var package in packages)
            {
                var domains = ServerController.GetDomains(package.PackageId);

                domains = domains.Where(x => !x.IsSubDomain && !x.IsDomainPointer).ToList(); //Selecting top-level domains

                //domains = domains.Where(x => x.ZoneItemId > 0).ToList(); //Selecting only dns enabled domains

                foreach (var domain in domains)
                {
                    if (domainsChanges.Any(x => x.DomainName == domain.DomainName))
                    {
                        continue;
                    }

                    if (!domainUsers.ContainsKey(domain.PackageId))
                    {
                        var domainUser = UserController.GetUser(packages.First(x=>x.PackageId == domain.PackageId).UserId);

                        domainUsers.Add(domain.PackageId, domainUser);
                    }

                    DomainDnsChanges domainChanges = new DomainDnsChanges();
                    domainChanges.DomainName = domain.DomainName;
                    domainChanges.PackageId = domain.PackageId;
                    domainChanges.Registrar = domain.RegistrarName;
                    domainChanges.ExpirationDate = domain.ExpirationDate;

                    var dbDnsRecords = ObjectUtils.CreateListFromDataReader<DnsRecordInfo>(DataProvider.GetDomainAllDnsRecords(domain.DomainId));

                    //execute server
                    foreach (var dnsServer in dnsServers)
                    {
                        var dnsMxRecords = GetDomainDnsRecords(winServer, domain.DomainName, dnsServer, DnsRecordType.MX, pause) ?? dbDnsRecords.Where(x => x.RecordType == DnsRecordType.MX).ToList();
                        var dnsNsRecords = GetDomainDnsRecords(winServer, domain.DomainName, dnsServer, DnsRecordType.NS, pause) ?? dbDnsRecords.Where(x => x.RecordType == DnsRecordType.NS).ToList();

                        FillRecordData(dnsMxRecords, domain, dnsServer);
                        FillRecordData(dnsNsRecords, domain, dnsServer);

                        domainChanges.DnsChanges.AddRange(ApplyDomainRecordsChanges(dbDnsRecords.Where(x => x.RecordType == DnsRecordType.MX), dnsMxRecords, dnsServer));
                        domainChanges.DnsChanges.AddRange(ApplyDomainRecordsChanges(dbDnsRecords.Where(x => x.RecordType == DnsRecordType.NS), dnsNsRecords, dnsServer));

                        domainChanges.DnsChanges = CombineDnsRecordChanges(domainChanges.DnsChanges, dnsServer).ToList();
                    }

                    domainsChanges.Add(domainChanges);
                }
            }

            var changedDomains = FindDomainsWithChangedRecords(domainsChanges);

            SendMailMessage(user, changedDomains, domainUsers);
        }

        

        #region Helpers

        private IEnumerable<DomainDnsChanges> FindDomainsWithChangedRecords(IEnumerable<DomainDnsChanges> domainsChanges)
        {
            var changedDomains = new List<DomainDnsChanges>();

            foreach (var domainChanges in domainsChanges)
            {
                var firstTimeAdditon = domainChanges.DnsChanges.All(x => x.Status == DomainDnsRecordStatuses.Added);

                if (firstTimeAdditon)
                {
                    continue;
                }

                bool isChanged = domainChanges.DnsChanges.Any(d =>  d.Status != DomainDnsRecordStatuses.NotChanged);

                if (isChanged)
                {
                    changedDomains.Add(domainChanges);
                }
            }

            return changedDomains;
        }

        private IEnumerable<DnsRecordInfoChange> ApplyDomainRecordsChanges(IEnumerable<DnsRecordInfo> dbRecords, List<DnsRecordInfo> dnsRecords, string dnsServer)
        {
            var dnsRecordChanges = new List<DnsRecordInfoChange>();

            var filteredDbRecords = dbRecords.Where(x => x.DnsServer == dnsServer);

            foreach (var record in filteredDbRecords)
            {
                var dnsRecord = dnsRecords.FirstOrDefault(x => x.Value == record.Value);

                if (dnsRecord != null)
                {
                    dnsRecordChanges.Add(new DnsRecordInfoChange { OldRecord = record, NewRecord = dnsRecord, Type = record.RecordType, Status = DomainDnsRecordStatuses.NotChanged, DnsServer = dnsServer });

                    dnsRecords.Remove(dnsRecord);
                }
                else
                {
                    dnsRecordChanges.Add(new DnsRecordInfoChange { OldRecord = record, NewRecord = new DnsRecordInfo { Value = string.Empty}, Type = record.RecordType, Status = DomainDnsRecordStatuses.Removed, DnsServer = dnsServer });

                    RemoveRecord(record);
                }
            }

            foreach (var record in dnsRecords)
            {
                dnsRecordChanges.Add(new DnsRecordInfoChange { OldRecord = new DnsRecordInfo { Value = string.Empty }, NewRecord = record, Type = record.RecordType, Status = DomainDnsRecordStatuses.Added, DnsServer = dnsServer });

                AddRecord(record);
            }

            return dnsRecordChanges;
        }

        private IEnumerable<DnsRecordInfoChange> CombineDnsRecordChanges(IEnumerable<DnsRecordInfoChange> records, string dnsServer)
        {
            var resultRecords = records.Where(x => x.DnsServer == dnsServer).ToList();

            var recordsToRemove = new List<DnsRecordInfoChange>();

            var removedRecords = records.Where(x => x.Status == DomainDnsRecordStatuses.Removed);
            var addedRecords = records.Where(x => x.Status == DomainDnsRecordStatuses.Added);

            foreach (DnsRecordType type in (DnsRecordType[])Enum.GetValues(typeof(DnsRecordType)))
            {
                foreach (var removedRecord in removedRecords.Where(x => x.Type == type))
                {
                    var addedRecord = addedRecords.FirstOrDefault(x => x.Type == type && !recordsToRemove.Contains(x));

                    if (addedRecord != null)
                    {
                        recordsToRemove.Add(addedRecord);

                        removedRecord.NewRecord = addedRecord.NewRecord;
                        removedRecord.Status = DomainDnsRecordStatuses.Updated;
                    }
                }
            }

            foreach (var record in recordsToRemove)
            {
                resultRecords.Remove(record);
            }

            return resultRecords;
        }

        private void FillRecordData(IEnumerable<DnsRecordInfo> records, DomainInfo domain, string dnsServer)
        {
            foreach (var record in records)
            {
                FillRecordData(record, domain, dnsServer);
            }
        }

        private void FillRecordData(DnsRecordInfo record, DomainInfo domain, string dnsServer)
        {
            record.DomainId = domain.DomainId;
            record.Date = DateTime.Now;
            record.DnsServer = dnsServer;
        }

        private void RemoveRecords(IEnumerable<DnsRecordInfo> dnsRecords)
        {
            foreach (var record in dnsRecords)
            {
                RemoveRecord(record);
            }
        }

        private void RemoveRecord(DnsRecordInfo dnsRecord)
        {
            DataProvider.DeleteDomainDnsRecord(dnsRecord.Id);

            Thread.Sleep(100);
        }

        private void AddRecords(IEnumerable<DnsRecordInfo> dnsRecords)
        {
            foreach (var record in dnsRecords)
            {
                AddRecord(record);
            }
        }

        private void AddRecord(DnsRecordInfo dnsRecord)
        {
            DataProvider.AddDomainDnsRecord(dnsRecord);

            Thread.Sleep(100);
        }

        private void SendMailMessage(UserInfo user, IEnumerable<DomainDnsChanges> domainsChanges, Dictionary<int, UserInfo> domainUsers)
        {
            BackgroundTask topTask = TaskManager.TopTask;

            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.DOMAIN_LOOKUP_LETTER);

            string from = settings["From"];

            var bcc = settings["CC"];

            string subject = settings["Subject"];

            MailPriority priority = MailPriority.Normal;
            if (!String.IsNullOrEmpty(settings["Priority"]))
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

            // input parameters
            string mailTo = (string)topTask.GetParamValue("MAIL_TO");

            string body = string.Empty;
            bool isHtml = user.HtmlMail;

            if (domainsChanges.Any())
            {
                body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            }
            else 
            {
                body = user.HtmlMail ? settings["NoChangesHtmlBody"] : settings["NoChangesTextBody"];
            }

            Hashtable items = new Hashtable();

            items["user"] = user;
            items["DomainUsers"] = domainUsers;
            items["Domains"] = domainsChanges;

            body = PackageController.EvaluateTemplate(body, items);

            // send mail message
            MailHelper.SendMessage(from, mailTo, bcc, subject, body, priority, isHtml);
        }

        public List<DnsRecordInfo> GetDomainDnsRecords(WindowsServer winServer, string domain, string dnsServer, DnsRecordType recordType, int pause)
        {
            Thread.Sleep(pause);

            //nslookup -type=mx google.com 195.46.39.39
            var command = "nslookup";
            var args = string.Format("-type={0} {1} {2}", recordType, domain, dnsServer);

            // execute system command
            var raw  = string.Empty;
            int triesCount = 0;

            do
            {
                raw = winServer.ExecuteSystemCommand(command, args);
            } 
            while (raw.ToLowerInvariant().Contains(DnsTimeOutMessage) && ++triesCount < DnsTimeOutRetryCount);

            //timeout check 
            if (raw.ToLowerInvariant().Contains(DnsTimeOutMessage))
            {
                return null;
            }

            var records = ParseNsLookupResult(raw, dnsServer, recordType);

            return records.ToList();
        }

        private IEnumerable<DnsRecordInfo> ParseNsLookupResult(string raw, string dnsServer, DnsRecordType recordType)
        {
            var records = new List<DnsRecordInfo>();

            var recordTypePattern = string.Empty;

            switch (recordType)
            {
                case DnsRecordType.NS:
                    {
                        recordTypePattern = NsRecordPattern;
                        break;
                    }
                case DnsRecordType.MX:
                    {
                        recordTypePattern = MxRecordPattern;
                        break;
                    }
            }

            var regex = new Regex(recordTypePattern, RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(raw))
            {
                if (match.Groups.Count != 2)
                {
                    continue;
                }

                var dnsRecord = new DnsRecordInfo
                {
                    Value = match.Groups[1].Value != null ? match.Groups[1].Value.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").ToLowerInvariant().Trim() : null,
                    RecordType = recordType,
                    DnsServer = dnsServer
                };

                records.Add(dnsRecord);
            }

            return records;
        }

        #endregion
    }
}

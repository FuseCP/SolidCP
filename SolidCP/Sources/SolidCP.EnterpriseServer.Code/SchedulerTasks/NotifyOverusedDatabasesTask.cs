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
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SolidCP.EnterpriseServer
{
    public class NotifyOverusedDatabasesTask : SchedulerTask
    {
        public const string DISKSPACE_FORMAT_STRING = "{0} - {1}Mb ({2}%)";
        public const string ALLOC_FORMAT_STRING = "{0} - {1}Mb";

        public override void DoWork()
        {
            // Input parameters:
            //  - DISKSPACE_OVERUSED
            //  - BANDWIDTH_OVERUSED

            BackgroundTask topTask = TaskManager.TopTask;

            // get the list of all packages
            List<PackageInfo> packages = PackageController.GetPackagePackages(topTask.PackageId, false);
            TaskManager.Write("Packages to verify: " + packages.Count.ToString());

            bool checkMSSQL = (String.Compare((string)topTask.GetParamValue("MSSQL_OVERUSED"), "true", true) == 0);
            bool checkMySQL = (String.Compare((string)topTask.GetParamValue("MYSQL_OVERUSED"), "true", true) == 0);
            bool checkMariaDB = (String.Compare((string)topTask.GetParamValue("MARIADB_OVERUSED"), "true", true) == 0);

            bool sendWarningEmail = Convert.ToBoolean(topTask.GetParamValue("SEND_WARNING_EMAIL"));
            bool sendOverusedEmail = Convert.ToBoolean(topTask.GetParamValue("SEND_OVERUSED_EMAIL"));
			int warningUsageThreshold = Convert.ToInt32(topTask.GetParamValue("WARNING_USAGE_THRESHOLD"));
            int overusedUsageThreshold = Convert.ToInt32(topTask.GetParamValue("OVERUSED_USAGE_THRESHOLD"));
			string warningMailFrom = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_FROM"));
			string warningMailBcc = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_BCC"));
			string warningMailSubject = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_SUBJECT"));
			string warningMailBody = Convert.ToString(topTask.GetParamValue("WARNING_MAIL_BODY"));
            string overusedMailFrom = Convert.ToString(topTask.GetParamValue("OVERUSED_MAIL_FROM"));
            string overusedMailBcc = Convert.ToString(topTask.GetParamValue("OVERUSED_MAIL_BCC"));
            string overusedMailSubject = Convert.ToString(topTask.GetParamValue("OVERUSED_MAIL_SUBJECT"));
            string overusedMailBody = Convert.ToString(topTask.GetParamValue("OVERUSED_MAIL_BODY"));

            int overusedPackages = 0;

            foreach (PackageInfo package in packages)
            {
				UserInfo userInfo = UserController.GetUser(package.UserId);

                List<DatabaseQuota> quotaMSSQL = new List<DatabaseQuota>();
                List<DatabaseQuota> quotaMYSQL = new List<DatabaseQuota>();
                List<DatabaseQuota> quotaMARIADB = new List<DatabaseQuota>();

                if (checkMSSQL || checkMySQL || checkMariaDB)
                {
                    QuotaValueInfo dsQuota = null;
                    DataSet Diskspace = PackageController.GetPackageDiskspace(package.PackageId);
                    foreach (DataRow spaceRow in Diskspace.Tables[0].Rows)
                    {
                       string groupName = spaceRow["GroupName"].ToString();
                       if (checkMSSQL && groupName.ToUpper().Contains("MSSQL"))
                        {
                            dsQuota = PackageController.GetPackageQuota(package.PackageId, groupName + ".MaxDatabaseSize");
                            if (dsQuota.QuotaAllocatedValue > 0)
                            {
                                int databaseSpaceUsage = Convert.ToInt32(spaceRow["Diskspace"]) * 100 / dsQuota.QuotaAllocatedValue;
                                quotaMSSQL.Add(new DatabaseQuota(groupName.ToUpper().Replace("MSSQL","SQL Server "),
                                            Convert.ToInt32(spaceRow["Diskspace"]), dsQuota.QuotaAllocatedValue,
                                            databaseSpaceUsage < warningUsageThreshold,
                                            databaseSpaceUsage < overusedUsageThreshold));
                            }
                        }
                       if (checkMySQL && groupName.ToUpper().Contains("MYSQL"))
                        {
                            dsQuota = PackageController.GetPackageQuota(package.PackageId, groupName + ".MaxDatabaseSize");
                            if (dsQuota.QuotaAllocatedValue > 0)
                            {
                                int databaseSpaceUsage = Convert.ToInt32(spaceRow["Diskspace"]) * 100 / dsQuota.QuotaAllocatedValue;
                                quotaMYSQL.Add(new DatabaseQuota(groupName.ToUpper().Replace("MYSQL", "MySQL "),
                                            Convert.ToInt32(spaceRow["Diskspace"]), dsQuota.QuotaAllocatedValue,
                                            databaseSpaceUsage < warningUsageThreshold,
                                            databaseSpaceUsage < overusedUsageThreshold));
                            }
                        }
                        if (checkMariaDB && groupName.ToUpper().Contains("MARIADB"))
                        {
                            dsQuota = PackageController.GetPackageQuota(package.PackageId, groupName + ".MaxDatabaseSize");
                            if (dsQuota.QuotaAllocatedValue > 0)
                            {
                                int databaseSpaceUsage = Convert.ToInt32(spaceRow["Diskspace"]) * 100 / dsQuota.QuotaAllocatedValue;
                                quotaMARIADB.Add(new DatabaseQuota(groupName.ToUpper().Replace("MARIADB", "MariaDB "),
                                            Convert.ToInt32(spaceRow["Diskspace"]), dsQuota.QuotaAllocatedValue,
                                            databaseSpaceUsage < warningUsageThreshold,
                                            databaseSpaceUsage < overusedUsageThreshold));
                            }
                        }
                    }

                    string userName = String.Format("{0} {1} ({2})/{3}", userInfo.FirstName, userInfo.LastName, userInfo.Username, userInfo.Email);
                    bool notifyOverusedByMail = false;
                    bool notifyWarningByMail = false;
                    List<string> formatItems = new List<string>();
                    List<string> formatWarningThreshold = new List<string>();
                    List<string> formatOverusedThreshold = new List<string>();
                    // add Microsoft SQL usage if enabled
                    if (checkMSSQL)
                    {
                        foreach (DatabaseQuota q in quotaMSSQL)
                        {
                            if (!q.BelowWarningThreshold || !q.BelowUsageThreshold)
                            {
                                formatItems.Add(String.Format(DISKSPACE_FORMAT_STRING, q.ProviderName, q.SpaceUsed, q.SpaceUsed * 100 / q.SpaceAllocated));
                            }
                            if (!q.BelowWarningThreshold)
                            {
                                formatWarningThreshold.Add(String.Format(ALLOC_FORMAT_STRING, q.ProviderName, q.SpaceAllocated));
                                notifyWarningByMail = true;
                            }
                            if (!q.BelowUsageThreshold)
                            {
                                formatOverusedThreshold.Add(String.Format(ALLOC_FORMAT_STRING, q.ProviderName, q.SpaceAllocated));
                                notifyOverusedByMail = true;
                            }
                        }
                    }
                       
                    // add MySQL usage if enabled
                    if (checkMySQL)
                    {
                        foreach (DatabaseQuota q in quotaMYSQL)
                        {
                            if (!q.BelowWarningThreshold || !q.BelowUsageThreshold)
                            {
                                formatItems.Add(String.Format(DISKSPACE_FORMAT_STRING, q.ProviderName, q.SpaceUsed, (q.SpaceUsed * 100) / q.SpaceAllocated));
                            }
                            if (!q.BelowWarningThreshold)
                            {
                                formatWarningThreshold.Add(String.Format(ALLOC_FORMAT_STRING, q.ProviderName, q.SpaceAllocated));
                                notifyWarningByMail = true;
                            }
                            if (!q.BelowUsageThreshold)
                            {
                                formatOverusedThreshold.Add(String.Format(ALLOC_FORMAT_STRING, q.ProviderName, q.SpaceAllocated));
                                notifyOverusedByMail = true;
                            }
                        }
                    }

                    // add MariaDB usage if enabled
                    if (checkMariaDB)
                    {
                        foreach (DatabaseQuota q in quotaMARIADB)
                        {
                            if (!q.BelowWarningThreshold || !q.BelowUsageThreshold)
                            {
                                formatItems.Add(String.Format(DISKSPACE_FORMAT_STRING, q.ProviderName, q.SpaceUsed, (q.SpaceUsed * 100) / q.SpaceAllocated));
                            }
                            if (!q.BelowWarningThreshold)
                            {
                                formatWarningThreshold.Add(String.Format(ALLOC_FORMAT_STRING, q.ProviderName, q.SpaceAllocated));
                                notifyWarningByMail = true;
                            }
                            if (!q.BelowUsageThreshold)
                            {
                                formatOverusedThreshold.Add(String.Format(ALLOC_FORMAT_STRING, q.ProviderName, q.SpaceAllocated));
                                notifyOverusedByMail = true;
                            }
                        }
                    }

                    // build usage strings
                    string usage = String.Join("\n", formatItems.ToArray());
                    string usageWarning = String.Join("\n", formatWarningThreshold.ToArray());
                    string usageOverused = String.Join("\n", formatOverusedThreshold.ToArray());

                    string warningMailSubjectProcessed = ReplaceVariables(warningMailSubject, usageWarning, usage, package.PackageName, userName);
                    string warningMailBodyProcessed = ReplaceVariables(warningMailBody, usageWarning, usage, package.PackageName, userName);

                    string overusedMailSubjectProcessed = ReplaceVariables(overusedMailSubject, usageOverused, usage, package.PackageName, userName);
                    string overusedMailBodyProcessed = ReplaceVariables(overusedMailBody, usageOverused, usage, package.PackageName, userName);


                    // Send email notifications
                    if (sendWarningEmail && notifyWarningByMail)
                    {
                        // Send warning email.
                        this.SendEmail(warningMailFrom, userInfo.Email, warningMailBcc, warningMailSubjectProcessed, warningMailBodyProcessed, false);
                    }

                    if (sendOverusedEmail && notifyOverusedByMail)
                    {
                        // Send overused email.
                        this.SendEmail(overusedMailFrom, userInfo.Email, overusedMailBcc, overusedMailSubjectProcessed, overusedMailBodyProcessed, false);
                    }
                    if (notifyOverusedByMail)
                    {
                        overusedPackages++;
                    }
                }
            }

            // log results
            TaskManager.Write("Total packages overused: " + overusedPackages.ToString());
        }

		private string ReplaceVariables(string content, string threshold, string usage, string spaceName, string customerName)
		{
			if (!String.IsNullOrEmpty(content))
			{
				content = Utils.ReplaceStringVariable(content, "threshold", threshold);
				content = Utils.ReplaceStringVariable(content, "date", DateTime.Now.ToString());
				content = Utils.ReplaceStringVariable(content, "usage", usage);
				content = Utils.ReplaceStringVariable(content, "space", spaceName);
				content = Utils.ReplaceStringVariable(content, "customer", customerName);
			}
			return content;
		}

    	private void SendEmail(string from, string to, string bcc, string subject, string body, bool isHtml)
		{
			// check input parameters
			if (String.IsNullOrEmpty(from))
			{
				TaskManager.WriteWarning("Specify 'Mail From' task parameter");
				return;
			}

			if (String.IsNullOrEmpty(to))
			{
				TaskManager.WriteWarning("Specify 'Mail To' task parameter");
				return;
			}

			// send mail message
			MailHelper.SendMessage(from, to, bcc, subject, body, isHtml);
		}
    }

    internal class DatabaseQuota
    {
        private string providerName = string.Empty;
        private int spaceUsed = 0;
        private int spaceAllocated = 0;
        private bool belowWarningThreshold = false;
        private bool belowUsageThreshold = false;
        public DatabaseQuota(string ProviderName, int SpaceUsed, int SpaceAllocated, bool BelowWarningThreshold, bool BelowUsageThreshold)
        {
            providerName = ProviderName;
            spaceUsed = SpaceUsed;
            spaceAllocated = SpaceAllocated;
            belowWarningThreshold = BelowWarningThreshold;
            belowUsageThreshold = BelowUsageThreshold;
        }

        public string ProviderName
        {
            get { return providerName; }
        }

        public int SpaceUsed
        {
            get { return spaceUsed; }
        }

        public int SpaceAllocated
        {
            get { return spaceAllocated; }
        }

        public bool BelowWarningThreshold
        {
            get { return belowWarningThreshold; }
        }

        public bool BelowUsageThreshold
        {
            get { return belowUsageThreshold; }
        }
    }
}

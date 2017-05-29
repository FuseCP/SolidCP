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
using System.Collections.Generic;
using System.Text;

using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.FTP;
using SolidCP.Providers.Mail;
using SolidCP.Providers.OS;

namespace SolidCP.EnterpriseServer
{
    public class UserCreationWizard
    {
        public UserCreationWizard()
        {
        }

        public static int CreateUserAccount(int parentPackageId, string username, string password,
            int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail,
            bool sendAccountLetter,
            bool createPackage, int planId, bool sendPackageLetter,
            string domainName, bool tempDomain, bool createWebSite,
            bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {
            UserCreationWizard wizard = new UserCreationWizard();

            return wizard.CreateUserAccountInternal(parentPackageId, username, password,
                roleId, firstName, lastName, email, secondaryEmail, htmlMail,
                sendAccountLetter,
                createPackage, planId, sendPackageLetter,
                domainName, tempDomain, createWebSite,
                createFtpAccount, ftpAccountName, createMailAccount, hostName, createZoneRecord);
        }

        // private fields
        bool userCreated = false;
        int createdUserId = 0;
        int createdPackageId = 0;

        public int CreateUserAccountInternal(int parentPackageId, string username, string password,
            int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail,
            bool sendAccountLetter,
            bool createPackage, int planId, bool sendPackageLetter,
            string domainName, bool tempDomain, bool createWebSite,
            bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsReseller);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(parentPackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check if username exists
            if (UserController.UserExists(username))
                return BusinessErrorCodes.ERROR_ACCOUNT_WIZARD_USER_EXISTS;

            // check if domain exists
            int checkDomainResult = ServerController.CheckDomain(domainName);
            if (checkDomainResult < 0)
                return checkDomainResult;

            // check if FTP account exists
            if (String.IsNullOrEmpty(ftpAccountName))
                ftpAccountName = username;

            if (FtpServerController.FtpAccountExists(ftpAccountName))
                return BusinessErrorCodes.ERROR_ACCOUNT_WIZARD_FTP_ACCOUNT_EXISTS;

            // load parent package
            PackageInfo parentPackage = PackageController.GetPackage(parentPackageId);

            /********************************************
             *  CREATE USER ACCOUNT
             * *****************************************/
            UserInfo user = new UserInfo();
            user.RoleId = roleId;
            user.StatusId = (int)UserStatus.Active;
            user.OwnerId = parentPackage.UserId;
            user.IsDemo = false;
            user.IsPeer = false;

            // account info
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.SecondaryEmail = secondaryEmail;
            user.Username = username;
//            user.Password = password;
            user.HtmlMail = htmlMail;

            // add a new user
            createdUserId = UserController.AddUser(user, false, password);
            if (createdUserId < 0)
            {
                // exit
                return createdUserId;
            }
            userCreated = true;

            // create package
            // load hosting plan
            createdPackageId = -1;
            if (createPackage)
            {
                try
                {
                    HostingPlanInfo plan = PackageController.GetHostingPlan(planId);

                    PackageResult packageResult = PackageController.AddPackage(
                        createdUserId, planId, plan.PlanName, "", (int)PackageStatus.Active, DateTime.Now, false);
                    createdPackageId = packageResult.Result;
                }
                catch (Exception ex)
                {
                    // error while adding package

                    // remove user account
                    UserController.DeleteUser(createdUserId);

                    throw ex;
                }

                if (createdPackageId < 0)
                {
                    // rollback wizard
                    Rollback();

                    // return code
                    return createdPackageId;
                }

                // create domain
                int domainId = 0;
                if ((createWebSite || createMailAccount || createZoneRecord) && !String.IsNullOrEmpty(domainName))
                {
                    try
                    {
                        DomainInfo domain = new DomainInfo();
                        domain.PackageId = createdPackageId;
                        domain.DomainName = domainName;
                        domain.HostingAllowed = false;
                        domainId = ServerController.AddDomain(domain, false, false);
                        if (domainId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return domainId;
                        }
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while adding domain
                        throw new Exception("Could not add domain", ex);
                    }
                }

                if (createWebSite && (domainId > 0))
                {
                    // create web site
                    try
                    {
                        int webSiteId = WebServerController.AddWebSite(
                            createdPackageId, hostName, domainId, 0, true, false);
                        if (webSiteId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return webSiteId;
                        }
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while creating web site
                        throw new Exception("Could not create web site", ex);
                    }
                }

                // create FTP account
                if (createFtpAccount)
                {
                    try
                    {
                        FtpAccount ftpAccount = new FtpAccount();
                        ftpAccount.PackageId = createdPackageId;
                        ftpAccount.Name = ftpAccountName;
                        ftpAccount.Password = password;
                        ftpAccount.Folder = "\\";
                        ftpAccount.CanRead = true;
                        ftpAccount.CanWrite = true;

                        int ftpAccountId = FtpServerController.AddFtpAccount(ftpAccount);
                        if (ftpAccountId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return ftpAccountId;
                        }
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while creating ftp account
                        throw new Exception("Could not create FTP account", ex);
                    }
                }

                if (createMailAccount && (domainId > 0))
                {
                    // create default mailbox
                    try
                    {
                        // load mail policy
                        UserSettings settings = UserController.GetUserSettings(createdUserId, UserSettings.MAIL_POLICY);
                        string catchAllName = !String.IsNullOrEmpty(settings["CatchAllName"])
                            ? settings["CatchAllName"] : "mail";

                        MailAccount mailbox = new MailAccount();
                        mailbox.Name = catchAllName + "@" + domainName;
                        mailbox.PackageId = createdPackageId;

                        // gather information from the form
                        mailbox.Enabled = true;

                        mailbox.ResponderEnabled = false;
                        mailbox.ReplyTo = "";
                        mailbox.ResponderSubject = "";
                        mailbox.ResponderMessage = "";

                        // password
                        mailbox.Password = password;

                        // redirection
                        mailbox.ForwardingAddresses = new string[] { };
                        mailbox.DeleteOnForward = false;
                        mailbox.MaxMailboxSize = 0;

                        int mailAccountId = MailServerController.AddMailAccount(mailbox);

                        if (mailAccountId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return mailAccountId;
                        }

                        // set catch-all account
                        MailDomain mailDomain = MailServerController.GetMailDomain(createdPackageId, domainName);
                        mailDomain.CatchAllAccount = "mail";
                        mailDomain.PostmasterAccount = "mail";
                        mailDomain.AbuseAccount = "mail";
                        MailServerController.UpdateMailDomain(mailDomain);

                        int mailDomainId = mailDomain.Id;
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while creating mail account
                        throw new Exception("Could not create mail account", ex);
                    }
                }

                // Instant Alias / Temporary URL
                if (tempDomain && (domainId > 0))
                {
                    int instantAliasId = ServerController.CreateDomainInstantAlias("", domainId);
                    if (instantAliasId < 0)
                    {
                        // rollback wizard
                        Rollback();

                        return instantAliasId;
                    }
                }

                // Domain DNS Zone
                if (createZoneRecord && (domainId > 0))
                {
                    ServerController.EnableDomainDns(domainId);
                }
            }

            // send welcome letters
            if (sendAccountLetter)
            {
                int result = PackageController.SendAccountSummaryLetter(createdUserId, null, null, true);
                if (result < 0)
                {
                    // rollback wizard
                    Rollback();

                    // return
                    return result;
                }
            }

            if (createPackage && sendPackageLetter)
            {
                int result = PackageController.SendPackageSummaryLetter(createdPackageId, null, null, true);
                if (result < 0)
                {
                    // rollback wizard
                    Rollback();

                    // return
                    return result;
                }
            }

            return createdUserId;
        }

        public void Rollback()
        {
            if (userCreated)
            {
                // delete user account and all its packages
                UserController.DeleteUser(createdUserId);
            }
        }
    }
}

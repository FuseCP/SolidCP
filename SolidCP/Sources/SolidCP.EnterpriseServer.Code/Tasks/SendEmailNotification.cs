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

namespace SolidCP.EnterpriseServer.Tasks
{
    public class SendEmailNotification : TaskEventHandler
    {
        public override void OnStart()
        {
            // nothing to-do
        }

        public override void OnComplete()
        {
            BackgroundTask topTask = TaskManager.TopTask;

            if (!TaskManager.HasErrors(topTask))
            {
                // Send user add notification
                if (topTask.Source == "USER" &&
                    topTask.TaskName == "ADD" && topTask.ItemId > 0)
                {
                    SendAddUserNotification();
                }
                // Send hosting package add notification
                if (topTask.Source == "HOSTING_SPACE"
                    && topTask.TaskName == "ADD" && topTask.ItemId > 0)
                {
                    SendAddPackageNotification();
                }
                // Send hosting package add notification
                if (topTask.Source == "HOSTING_SPACE_WR"
                    && topTask.TaskName == "ADD" && topTask.ItemId > 0)
                {
                    SendAddPackageWithResourcesNotification();
                }
            }
        }

        private void CheckSmtpResult(int resultCode)
        {
            if (resultCode != 0)
            {
                TaskManager.WriteWarning("Unable to send an e-mail notification");
                TaskManager.WriteParameter("SMTP Result", resultCode);
            }
        }

        protected void SendAddPackageWithResourcesNotification()
        {
            try
            {
                BackgroundTask topTask = TaskManager.TopTask;
                
                bool sendLetter = Utils.ParseBool(topTask.GetParamValue("SendLetter"), false);

                if (sendLetter)
                {
                    int sendResult = PackageController.SendPackageSummaryLetter(topTask.ItemId, null, null, true);
                    CheckSmtpResult(sendResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }

        protected void SendAddPackageNotification()
        {
            try
            {
                BackgroundTask topTask = TaskManager.TopTask;
                
                int userId = Utils.ParseInt(topTask.GetParamValue("UserId").ToString(), 0);
                bool sendLetter = Utils.ParseBool(topTask.GetParamValue("SendLetter"), false);
                bool signup = Utils.ParseBool(topTask.GetParamValue("Signup"), false);

                // send space letter if enabled
                UserSettings settings = UserController.GetUserSettings(userId, UserSettings.PACKAGE_SUMMARY_LETTER);
                if (sendLetter
                    && !String.IsNullOrEmpty(settings["EnableLetter"])
                    && Utils.ParseBool(settings["EnableLetter"], false))
                {
                    // send letter
                    int smtpResult = PackageController.SendPackageSummaryLetter(topTask.ItemId, null, null, signup);
                    CheckSmtpResult(smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }

        protected void SendAddUserNotification()
        {
            try
            {
                BackgroundTask topTask = TaskManager.TopTask;

                bool sendLetter = Utils.ParseBool(topTask.GetParamValue("SendLetter"), false);

                int userId = topTask.ItemId;
                // send account letter if enabled
                UserSettings settings = UserController.GetUserSettings(userId, UserSettings.ACCOUNT_SUMMARY_LETTER);
                if (sendLetter
                    && !String.IsNullOrEmpty(settings["EnableLetter"])
                    && Utils.ParseBool(settings["EnableLetter"], false))
                {
                    // send letter
                    int smtpResult = PackageController.SendAccountSummaryLetter(userId, null, null, true);
                    CheckSmtpResult(smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }
    }
}

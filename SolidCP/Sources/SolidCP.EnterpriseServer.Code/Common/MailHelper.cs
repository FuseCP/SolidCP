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
using System.Net;
using System.Net.Mail;

namespace SolidCP.EnterpriseServer
{
    public class MailHelper
    {
        public static int SendMessage(string from, string to, string subject, string body, bool isHtml)
        {
            return SendMessage(from, to, null, subject, body, MailPriority.Normal, isHtml);
        }

        public static int SendMessage(string from, string to, string bcc, string subject, string body, bool isHtml)
        {
            return SendMessage(from, to, bcc, subject, body, MailPriority.Normal, isHtml);
        }

        public static int SendMessage(string from, string to, string bcc, string subject, string body,
            MailPriority priority, bool isHtml, Attachment[] attachments)
        {
            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient();

            // load SMTP client settings
            SystemSettings settings = SystemController.GetSystemSettingsInternal(
                SystemSettings.SMTP_SETTINGS,
                true
            );

            client.Host = settings["SmtpServer"];
            client.Port = settings.GetInt("SmtpPort");
            if (!String.IsNullOrEmpty(settings["SmtpUsername"]))
            {
                client.Credentials = new NetworkCredential(
                    settings["SmtpUsername"],
                    settings["SmtpPassword"]
                );
            }

            if (!String.IsNullOrEmpty(settings["SmtpEnableSsl"]))
            {
                client.EnableSsl = Utils.ParseBool(settings["SmtpEnableSsl"], false);
            }

            // create message
            MailMessage message = new MailMessage(from, to);
            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = isHtml;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            if (!String.IsNullOrEmpty(bcc))
                message.Bcc.Add(bcc);
            message.Priority = priority;

            if (attachments != null)
            {
                foreach(Attachment current in attachments)
                {
                    message.Attachments.Add(current);    
                }                
            }
            
            // send message
            try
            {
                client.Send(message);

                return 0;
            }
            catch (SmtpException ex)
            {
                switch (ex.StatusCode)
                {
                    case SmtpStatusCode.BadCommandSequence: return BusinessErrorCodes.SMTP_BAD_COMMAND_SEQUENCE;
                    case SmtpStatusCode.CannotVerifyUserWillAttemptDelivery: return BusinessErrorCodes.SMTP_CANNOT_VERIFY_USER_WILL_ATTEMPT_DELIVERY;
                    case SmtpStatusCode.ClientNotPermitted: return BusinessErrorCodes.SMTP_CLIENT_NOT_PERMITTED;
                    case SmtpStatusCode.CommandNotImplemented: return BusinessErrorCodes.SMTP_COMMAND_NOT_IMPLEMENTED;
                    case SmtpStatusCode.CommandParameterNotImplemented: return BusinessErrorCodes.SMTP_COMMAND_PARAMETER_NOT_IMPLEMENTED;
                    case SmtpStatusCode.CommandUnrecognized: return BusinessErrorCodes.SMTP_COMMAND_UNRECOGNIZED;
                    case SmtpStatusCode.ExceededStorageAllocation: return BusinessErrorCodes.SMTP_EXCEEDED_STORAGE_ALLOCATION;
                    case SmtpStatusCode.GeneralFailure: return BusinessErrorCodes.SMTP_GENERAL_FAILURE;
                    case SmtpStatusCode.InsufficientStorage: return BusinessErrorCodes.SMTP_INSUFFICIENT_STORAGE;
                    case SmtpStatusCode.LocalErrorInProcessing: return BusinessErrorCodes.SMTP_LOCAL_ERROR_IN_PROCESSING;
                    case SmtpStatusCode.MailboxBusy: return BusinessErrorCodes.SMTP_MAILBOX_BUSY;
                    case SmtpStatusCode.MailboxNameNotAllowed: return BusinessErrorCodes.SMTP_MAILBOX_NAME_NOTALLOWED;
                    case SmtpStatusCode.MailboxUnavailable: return BusinessErrorCodes.SMTP_MAILBOX_UNAVAILABLE;
                    case SmtpStatusCode.MustIssueStartTlsFirst: return BusinessErrorCodes.SMTP_MUST_ISSUE_START_TLS_FIRST;
                    case SmtpStatusCode.ServiceClosingTransmissionChannel: return BusinessErrorCodes.SMTP_SERVICE_CLOSING_TRANSMISSION_CHANNEL;
                    case SmtpStatusCode.ServiceNotAvailable: return BusinessErrorCodes.SMTP_SERVICE_NOT_AVAILABLE;
                    case SmtpStatusCode.SyntaxError: return BusinessErrorCodes.SMTP_SYNTAX_ERROR;
                    case SmtpStatusCode.TransactionFailed: return BusinessErrorCodes.SMTP_TRANSACTION_FAILED;
                    case SmtpStatusCode.UserNotLocalTryAlternatePath: return BusinessErrorCodes.SMTP_USER_NOT_LOCAL_TRY_ALTERNATE_PATH;
                    case SmtpStatusCode.UserNotLocalWillForward: return BusinessErrorCodes.SMTP_USER_NOT_LOCAL_WILL_FORWARD;
                    default: return BusinessErrorCodes.SMTP_UNKNOWN_ERROR;
                }
            }
            finally
            {
                // Clean up.
                message.Dispose();
            }
        }
        
        
        public static int SendMessage(string from, string to, string bcc, string subject, string body,
            MailPriority priority, bool isHtml)
        {
            return SendMessage(from, to, bcc, subject, body, priority, isHtml, null);
        }
    }
}

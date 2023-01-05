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

namespace SolidCP.Providers.Mail
{
	[Serializable]
	public class MailAccount : ServiceProviderItem
	{
		private bool enabled = true;
		private string password;
		private string replyTo;
		private bool responderEnabled;
		private string responderSubject;
		private string responderMessage;
		private string firstName; // SM
		private string lastName; // SM
		private bool deleteOnForward;
		private string[] forwardingAddresses;
		private string signature;
		private bool passwordLocked;
		private int maxMailboxSize;
		private bool changePassword;
		private bool isDomainAdmin;
		private bool isDomainAdminEnabled;
		private bool retainLocalCopy;
        private string signatureName;
        private string signatureGuid;
		private int signatureiD;

        public bool UnlimitedSize
		{
			get
			{
				return (maxMailboxSize < 0);
			}
            set
            {
                if (value)
                {
                    maxMailboxSize = 0;
                }
            }
		}

		public string ReplyTo
		{
			get { return this.replyTo; }
			set { this.replyTo = value; }
		}

		public string ResponderSubject
		{
			get { return this.responderSubject; }
			set { this.responderSubject = value; }
		}

		public string ResponderMessage
		{
			get { return this.responderMessage; }
			set { this.responderMessage = value; }
		}

		public bool ResponderEnabled
		{
			get { return this.responderEnabled; }
			set { this.responderEnabled = value; }
		}

		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		[Persistent]
		public string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}

		#region SmarterMail

		/// <summary>
		/// First Name
		/// </summary>
		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		/// <summary>
		/// Last name
		/// </summary>
		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public bool DeleteOnForward
		{
			get { return deleteOnForward; }
			set { deleteOnForward = value; }
		}

		public string[] ForwardingAddresses
		{
			get { return forwardingAddresses; }
			set { forwardingAddresses = value; }
		}

		public string Signature
		{
			get { return signature; }
			set { signature = value; }
		}

		public string SignatureGuid
		{
			get { return signatureGuid; }
			set { signatureGuid = value; }
		}

		public string SignatureName
		{
			get { return signatureName; }
			set { signatureName = value; }
		}

		public int SignatureiD
		{
			get { return signatureiD; }
			set { signatureiD = value; }
		}

		public bool IsDomainAdminEnabled
		{
			get { return isDomainAdminEnabled; }
			set { isDomainAdminEnabled = value; }
		}

		public bool IsDomainAdmin
		{
			get { return isDomainAdmin; }
			set { isDomainAdmin = value; }
		}

		public bool PasswordLocked
		{
			get { return passwordLocked; }
			set { passwordLocked = value; }
		}

		public int MaxMailboxSize
		{
			get { return maxMailboxSize; }
			set { maxMailboxSize = value; }
		}

		public bool ChangePassword
		{
			get { return changePassword; }
			set { changePassword = value; }
		}

		#endregion

		#region MDaemon

		public bool RetainLocalCopy
		{
			get { return retainLocalCopy; }
			set { retainLocalCopy = value; }
		}

		#endregion

		#region hMailServer

		public bool SignatureEnabled { get; set; }
        public string SignatureHTML { get; set; }		
        public bool ForwardingEnabled { get; set; }
        public long Size { get; set; }
        public string LastLogonTime { get; set; }
        public long QuotaUsed { get; set; }
        public bool ResponderExpires {get;set;}
        public string ResponderExpirationDate { get; set; }
        
		#endregion

        #region IceWarp

        public int IceWarpAccountType { get; set; }
        public int IceWarpAccountState { get; set; }
	    public int IceWarpRespondType { get; set; }

        public bool RespondOnlyBetweenDates { get; set; }   // Added this because Calendar Control used did not allow null values
        public DateTime RespondFrom { get; set; }
        public DateTime RespondTo { get; set; }
        public string RespondWithReplyFrom { get; set; }
        public int RespondPeriodInDays { get; set; }

        public bool DeleteOlder { get; set; }
        public int DeleteOlderDays { get; set; }
        public bool ForwardOlder { get; set; }
        public int ForwardOlderDays { get; set; }
        public string ForwardOlderTo { get; set; }

        public int MaxMessageSizeMegaByte { get; set; }
        public int MegaByteSendLimit { get; set; }
        public int NumberSendLimit { get; set; }

        public string FullName { get; set; }
        #endregion
    }
}

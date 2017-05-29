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
	public class MailList : ServiceProviderItem
    {

        private string[] members = new string[0];
		private string description;
	    private string textHeader;
	    private string textFooter;
        private string htmlHeader;
        private string htmlFooter;
        private bool moderated; 
		private bool enabled; 
		private string moderatorAddress;
		private ReplyTo replyToMode;
        private PostingMode postingMode;
		private string password;
        private string subjectPrefix = "";
        private int maxMessageSize;
        private int maxRecipientsPerMessage;
	    private string listToAddress;
        private string listFromAddress;
	    private string listReplytoAddress;
	    private bool digestmode;
	    private bool sendsubscribe;
        private bool sendunsubscribe;
        private bool allowunsubscribe;
        private bool disablelistcommand;
        private bool disablesubscribecommand;


        public int MaxMessageSize
        {
            get { return maxMessageSize; }
            set { maxMessageSize = value; }
        }

        public int MaxRecipientsPerMessage
        {
            get { return maxRecipientsPerMessage; }
            set { maxRecipientsPerMessage = value; }
        }

		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

        public string TextHeader
        {
            get { return this.textHeader; }
            set { this.textHeader = value; }
        }

        public string TextFooter
        {
            get { return this.textFooter; }
            set { this.textFooter = value; }
        }

        public string HtmlHeader
        {
            get { return this.htmlHeader; }
            set { this.htmlHeader = value; }
        }

        public string HtmlFooter
        {
            get { return this.htmlFooter; }
            set { this.htmlFooter = value; }
        }

		public string ModeratorAddress
		{
			get { return this.moderatorAddress; }
			set { this.moderatorAddress = value; }
		}

        public PostingMode PostingMode
		{
			get { return this.postingMode; }
			set { this.postingMode = value; }
		}

		public ReplyTo ReplyToMode
		{
			get { return this.replyToMode; }
			set { this.replyToMode = value; }
		}

		public bool Moderated
		{
			get { return this.moderated; }
			set { this.moderated = value; }
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

		public string[] Members
		{
			get { return this.members; }
			set { this.members = value; }
		}

        public string SubjectPrefix
        {
            get { return subjectPrefix; }
            set { subjectPrefix = value; }
        }

	    #region SmarterMail

		private bool enableSubjectPrefix;
		private bool requirePassword;
		
		
		public bool EnableSubjectPrefix
		{
			get { return enableSubjectPrefix; }
			set { enableSubjectPrefix = value; }
		}

		public bool RequirePassword
		{
			get { return requirePassword; }
			set { requirePassword = value; }
		}

	    public string ListToAddress
	    {
            get { return listToAddress; }
            set { listToAddress = value; }
	    }

	    public string ListFromAddress
	    {
            get { return listFromAddress; }
            set { listFromAddress = value; }
	    }

        public string ListReplyToAddress
        {
            get { return listReplytoAddress; }
            set { listReplytoAddress = value; }
        }

        public bool DigestMode
        {
            get { return digestmode; }
            set { digestmode = value; }
        }

        public bool SendSubscribe
        {
            get { return sendsubscribe; }
            set { sendsubscribe = value; }
        }

        public bool SendUnsubscribe
        {
            get { return sendunsubscribe; }
            set { sendunsubscribe = value; }
        }

        public bool AllowUnsubscribe
        {
            get { return allowunsubscribe; }
            set { allowunsubscribe = value; }
        }

        public bool DisableListcommand
        {
            get { return disablelistcommand; }
            set { disablelistcommand = value; }
        }

        public bool DisableSubscribecommand
        {
            get { return disablesubscribecommand; }
            set { disablesubscribecommand = value; }
        }

		#endregion

        #region MailEnable

	    private int attachHeader;
	    private int attachFooter;
	    private PrefixOption prefixOption;

	    public int AttachHeader
	    {
            get { return this.attachHeader;  }
            set { this.attachHeader = value; }
	    }

        public int AttachFooter
        {
            get { return this.attachFooter; }
            set { this.attachFooter = value; }
        }

        public PrefixOption PrefixOption
        {
            get { return this.prefixOption; }
            set { this.prefixOption = value;}
        }

	    #endregion

        #region Merak

	    private bool maxMessageSizeEnabled;
	    private PasswordProtection passwordProtection;

	    public bool MaxMessageSizeEnabled
	    {
	        get { return maxMessageSizeEnabled; }
	        set { maxMessageSizeEnabled = value;}
	    }

	    public PasswordProtection PasswordProtection
	    {
            get { return passwordProtection;}
	        set { passwordProtection = value; }
	    }

	    #endregion

        #region hMailServer

        private bool requireSmtpAuthentication;

        public bool RequireSmtpAuthentication
        {
            get { return requireSmtpAuthentication; }
            set { requireSmtpAuthentication = value; }
        }
        #endregion

        #region IceWarp

	    public IceWarpListMembersSource MembersSource { get; set; }
	    public IceWarpListFromAndReplyToHeader FromHeader { get; set; }
        public IceWarpListFromAndReplyToHeader ReplyToHeader { get; set; }
	    public bool SetReceipientsToToHeader { get; set; }
	    public IceWarpListOriginator Originator { get; set; }
        public IceWarpListDefaultRights DefaultRights { get; set; }
	    public int MaxMembers { get; set; }
	    public bool SendToSender { get; set; }
	    public int MaxMessagesPerMinute { get; set; }
	    public IceWarpListConfirmSubscription ConfirmSubscription { get; set; }
	    public bool CommandsInSubject { get; set; }
	    public bool DisableWhichCommand { get; set; }
        public bool DisableReviewCommand { get; set; }
        public bool DisableVacationCommand { get; set; }
        public string CommandPassword { get; set; }
        public bool SuppressCommandResponses { get; set; }

        #endregion
    } 
}

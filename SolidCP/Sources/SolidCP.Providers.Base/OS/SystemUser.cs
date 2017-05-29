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

namespace SolidCP.Providers.OS
{
	/// <summary>
	/// Summary description for SystemUserItem.
	/// </summary>
	[Serializable]
	public class SystemUser : ServiceProviderItem
	{
		private bool system;
		private string fullName;
		private string description = "SolidCP system account";
		private string password;
        private string msIIS_FTPDir = "";
        private string msIIS_FTPRoot = "";
		private bool passwordCantChange;
		private bool passwordNeverExpires;
		private bool accountDisabled;
		private string[] memberOf = new string[0];

		public SystemUser()
		{
		}

		[Persistent]
		public bool System
		{
			get { return system; }
			set { system = value; }
		}

		public string FullName
		{
			get { return fullName; }
			set { fullName = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		[Persistent]
		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		public bool PasswordCantChange
		{
			get { return passwordCantChange; }
			set { passwordCantChange = value; }
		}

		public bool PasswordNeverExpires
		{
			get { return passwordNeverExpires; }
			set { passwordNeverExpires = value; }
		}

		public bool AccountDisabled
		{
			get { return accountDisabled; }
			set { accountDisabled = value; }
		}

		public string[] MemberOf
		{
			get { return memberOf; }
			set { memberOf = value; }
		}

        public string MsIIS_FTPDir
        {
            get { return msIIS_FTPDir; }
            set { msIIS_FTPDir = value; }
        }

        public string MsIIS_FTPRoot
        {
            get { return msIIS_FTPRoot; }
            set { msIIS_FTPRoot = value; }
        }

	}
}

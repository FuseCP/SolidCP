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
using SolidCP.EnterpriseServer;
using System.DirectoryServices;

namespace SolidCP.Import.Enterprise
{
	class Global
	{
		private static string rootOU;
		public static string RootOU
		{
			get { return rootOU; }
			set { rootOU = value; }
		}
	
		private static string primaryDomainController;
		public static string PrimaryDomainController
		{
			get { return primaryDomainController; }
			set { primaryDomainController = value; }
		}
	
		private static string aDRootDomain;

		public static string ADRootDomain
		{
			get { return aDRootDomain; }
			set { aDRootDomain = value; }
		}

        private static string netBiosDomain;
        public static string NetBiosDomain
        {
            get { return netBiosDomain; }
            set { netBiosDomain = value; }
        }

		public static PackageInfo Space;
		public static string TempDomain;

		public static string MailboxCluster;
		public static string StorageGroup;
		public static string MailboxDatabase;
		public static string KeepDeletedMailboxesDays;
		public static string KeepDeletedItemsDays;
		public static DirectoryEntry OrgDirectoryEntry;
		public static List<DirectoryEntry> SelectedAccounts;
		public static string OrganizationId;
		public static string OrganizationName;
		public static int ItemId;
		public static string ErrorMessage;
		public static bool ImportAccountsOnly;
		public static bool HasErrors;
        public static int defaultMailboxPlanId;
	
	}
}

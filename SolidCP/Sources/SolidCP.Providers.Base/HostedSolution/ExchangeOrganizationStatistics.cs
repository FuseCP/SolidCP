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

namespace SolidCP.Providers.HostedSolution
{
	public class ExchangeOrganizationStatistics
	{
		private int allocatedMailboxes;
		private int createdMailboxes;

		private int allocatedContacts;
		private int createdContacts;

		private int allocatedDistributionLists;
		private int createdDistributionLists;

		private int allocatedPublicFolders;
		private int createdPublicFolders;

		private int allocatedDomains;
		private int createdDomains;

		private int allocatedDiskSpace;
		private int usedDiskSpace;

		public int AllocatedMailboxes
		{
			get { return this.allocatedMailboxes; }
			set { this.allocatedMailboxes = value; }
		}

		public int CreatedMailboxes
		{
			get { return this.createdMailboxes; }
			set { this.createdMailboxes = value; }
		}

		public int AllocatedContacts
		{
			get { return this.allocatedContacts; }
			set { this.allocatedContacts = value; }
		}

		public int CreatedContacts
		{
			get { return this.createdContacts; }
			set { this.createdContacts = value; }
		}

		public int AllocatedDistributionLists
		{
			get { return this.allocatedDistributionLists; }
			set { this.allocatedDistributionLists = value; }
		}

		public int CreatedDistributionLists
		{
			get { return this.createdDistributionLists; }
			set { this.createdDistributionLists = value; }
		}

		public int AllocatedPublicFolders
		{
			get { return this.allocatedPublicFolders; }
			set { this.allocatedPublicFolders = value; }
		}

		public int CreatedPublicFolders
		{
			get { return this.createdPublicFolders; }
			set { this.createdPublicFolders = value; }
		}

		public int AllocatedDomains
		{
			get { return this.allocatedDomains; }
			set { this.allocatedDomains = value; }
		}

		public int CreatedDomains
		{
			get { return this.createdDomains; }
			set { this.createdDomains = value; }
		}

		public int AllocatedDiskSpace
		{
			get { return this.allocatedDiskSpace; }
			set { this.allocatedDiskSpace = value; }
		}

		public int UsedDiskSpace
		{
			get { return this.usedDiskSpace; }
			set { this.usedDiskSpace = value; }
		}
	}
}

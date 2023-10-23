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

namespace SolidCP.Providers.HostedSolution
{
	public enum CalendarProcessingFlags
    {
		None,
		AutoUpdate,
		AutoAccept
	}

	public class ExchangeResourceMailboxSettings
    {
        string displayName;
        string accountName;
		int resourceCapacity;
		CalendarProcessingFlags automateProcessing;
		int bookingWindowInDays;
		int maximumDurationInMinutes;
		bool allowRecurringMeetings;
		bool enforceSchedulingHorizon;
		bool scheduleOnlyDuringWorkHours;
		ExchangeAccount[] resourceDelegates;
		bool allBookInPolicy;
		bool allRequestInPolicy;
		bool addAdditionalResponse;
		string additionalResponse;

		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public string AccountName
		{
			get { return this.accountName; }
			set { this.accountName = value; }
		}

		public int ResourceCapacity
		{
			get { return this.resourceCapacity; }
			set { this.resourceCapacity = value; }
		}

		public CalendarProcessingFlags AutomateProcessing
		{
			get { return this.automateProcessing; }
			set { this.automateProcessing = value; }
		}

		public int BookingWindowInDays
		{
			get { return this.bookingWindowInDays; }
			set { this.bookingWindowInDays = value; }
		}

		public int MaximumDurationInMinutes
		{
			get { return this.maximumDurationInMinutes; }
			set { this.maximumDurationInMinutes = value; }
		}

		public bool AllowRecurringMeetings
		{
			get { return this.allowRecurringMeetings; }
			set { this.allowRecurringMeetings = value; }
		}

		public bool EnforceSchedulingHorizon
		{
			get { return this.enforceSchedulingHorizon; }
			set { this.enforceSchedulingHorizon = value; }
		}

		public bool ScheduleOnlyDuringWorkHours
		{
			get { return this.scheduleOnlyDuringWorkHours; }
			set { this.scheduleOnlyDuringWorkHours = value; }
		}

		public ExchangeAccount[] ResourceDelegates
		{
			get { return this.resourceDelegates; }
			set { this.resourceDelegates = value; }
		}

		public bool AllBookInPolicy
		{
			get { return this.allBookInPolicy; }
			set { this.allBookInPolicy = value; }
		}

		public bool AllRequestInPolicy
		{
			get { return this.allRequestInPolicy; }
			set { this.allRequestInPolicy = value; }
		}

		public bool AddAdditionalResponse
		{
			get { return this.addAdditionalResponse; }
			set { this.addAdditionalResponse = value; }
		}

		public string AdditionalResponse
		{
			get { return this.additionalResponse; }
			set { this.additionalResponse = value; }
		}
	}
}

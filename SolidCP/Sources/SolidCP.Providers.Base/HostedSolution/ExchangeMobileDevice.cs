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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.HostedSolution
{
	public class ExchangeMobileDevice
	{
		public MobileDeviceStatus Status { get; set; }
		public DateTime FirstSyncTime { get; set; }
		public DateTime LastPolicyUpdateTime { get; set; }
		public DateTime LastSyncAttemptTime { get; set; }
		public DateTime LastSuccessSync { get; set; }
		public string DeviceType { get; set; }
		public string DeviceID { get; set; }
		public string DeviceUserAgent { get; set; }
		public DateTime DeviceWipeSentTime { get; set; }
		public DateTime DeviceWipeRequestTime { get; set; }
		public DateTime DeviceWipeAckTime { get; set; }
		public int LastPingHeartbeat { get; set; }
		public string RecoveryPassword { get; set; }
		public string DeviceModel { get; set; }
		public string DeviceIMEI { get; set; }
		public string DeviceFriendlyName { get; set; }
		public string DeviceOS { get; set; }
		public string DeviceOSLanguage { get; set; }
		public string DevicePhoneNumber { get; set; }
		public string Id { get; set; }
	}
}

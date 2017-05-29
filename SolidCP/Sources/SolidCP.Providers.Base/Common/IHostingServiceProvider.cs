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
using System.Collections;
using SolidCP.Providers.Common;

namespace SolidCP.Providers
{
	/// <summary>
	/// Summary description for IPanelServiceProvider.
	/// </summary>
	public interface IHostingServiceProvider
	{
		/// <summary>
		/// Executes each time when the service is being added to some server.
		/// Prepare environment for service usage (like setting folder permissions,
		/// creating system users, etc.)
		/// </summary>
		string[] Install();

        /// <summary>
        /// Returns the list of additional provider properties.
        /// </summary>
        /// <returns>The array of additional properties.</returns>
        SettingPair[] GetProviderDefaultSettings();

		/// <summary>
		/// Executes when service is being removed from server.
		/// Performs any clean up operations.
		/// </summary>
		void Uninstall();

		/// <summary>
		/// Checks whether service is installed within the system. This method will be
		/// used by server creation wizard for automatic services detection and configuring.
		/// </summary>
		/// <returns>True if service is installed; otherwise - false.</returns>
		bool IsInstalled();

		void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled);
        void DeleteServiceItems(ServiceProviderItem[] items);

        ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items);
        ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since);
	}
}

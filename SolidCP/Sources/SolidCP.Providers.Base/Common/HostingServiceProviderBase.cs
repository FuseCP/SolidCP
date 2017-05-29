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
using System.IO;
using SolidCP.Providers.Common;

namespace SolidCP.Providers
{
    public abstract class HostingServiceProviderBase : IHostingServiceProvider
    {
        RemoteServerSettings serverSettings = new RemoteServerSettings();
        ServiceProviderSettings providerSettings = new ServiceProviderSettings();

        public RemoteServerSettings ServerSettings
        {
            get { return serverSettings; }
            set { serverSettings = value; }
        }

        public ServiceProviderSettings ProviderSettings
        {
            get { return providerSettings; }
            set { providerSettings = value; }
        }

        #region IHostingServiceProvider methods
		public virtual string[] GetProviderDefaults()
		{
			return new string[] { };
		}

        public virtual string[] Install()
        {
            // install in silence
            return new string[] { };
        }

        public virtual SettingPair[] GetProviderDefaultSettings()
        {
            return new SettingPair[] { };
        }

        public virtual void Uninstall()
        {
            // nothing to do
        }

        public abstract bool IsInstalled();

        public virtual void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {
            
            // do nothing
        }

        public virtual void DeleteServiceItems(ServiceProviderItem[] items)
        {
            // do nothing
        }

        public virtual ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            // don't calculate disk space
            return null;
        }

        public virtual ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since)
        {
            // don't calculate bandwidth
            return null;
        }
        #endregion

        #region Helper Methods
        protected void CheckTempPath(string path)
        {
            // check path
            string tempPath = Path.GetTempPath();

			//bug when calling from local machine
            //if (!path.ToLower().StartsWith(tempPath.ToLower()))
            //   throw new Exception("The path is not allowed");
        }
        #endregion
    }
}

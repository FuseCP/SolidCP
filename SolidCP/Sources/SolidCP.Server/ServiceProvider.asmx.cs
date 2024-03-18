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
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Server.Utils;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for ServiceProvider
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class ServiceProvider : HostingServiceProviderWebService
    {
        [WebMethod, SoapHeader("settings")]
        public string[] Install()
        {
            try
            {
                Log.WriteStart("'{0}' Install", ProviderSettings.ProviderName);
                return Provider.Install();
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't Install '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("'{0}' Install", ProviderSettings.ProviderName);
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SettingPair[] GetProviderDefaultSettings()
        {
            try
            {
                Log.WriteStart("'{0}' GetProviderDefaultSettings", ProviderSettings.ProviderName);
                return Provider.GetProviderDefaultSettings();
            }
            catch (Exception ex)
            {
				Log.WriteError(String.Format("Can't GetProviderDefaultSettings '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
            finally
            {
				Log.WriteEnd("'{0}' GetProviderDefaultSettings", ProviderSettings.ProviderName);
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void Uninstall()
        {
            try
            {
                Log.WriteStart("'{0}' Uninstall", ProviderSettings.ProviderName);
                Provider.Uninstall();
                Log.WriteEnd("'{0}' Uninstall", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't Uninstall '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool IsInstalled()
        {
            try
            {
                Log.WriteStart("'{0}' IsInstalled", ProviderSettings.ProviderName);
                return Provider.IsInstalled();
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't check '{0}' provider IsInstalled", ProviderSettings.ProviderName), ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("'{0}' IsInstalled", ProviderSettings.ProviderName);
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ChangeServiceItemsState(SoapServiceProviderItem[] items, bool enabled)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeServiceItemsState", ProviderSettings.ProviderName);
                Provider.ChangeServiceItemsState(UnwrapServiceProviderItems(items), enabled);
                Log.WriteEnd("'{0}' ChangeServiceItemsState", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error on ChangeServiceItemsState() in '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteServiceItems(SoapServiceProviderItem[] items)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteServiceItems", ProviderSettings.ProviderName);
                Provider.DeleteServiceItems(UnwrapServiceProviderItems(items));
                Log.WriteEnd("'{0}' DeleteServiceItems", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error on DeleteServiceItems() in '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(SoapServiceProviderItem[] items)
        {
            try
            {
                Log.WriteStart("'{0}' GetServiceItemsDiskSpace", ProviderSettings.ProviderName);

                if (items.Length == 0) return new ServiceProviderItemDiskSpace[] {};
                return Provider.GetServiceItemsDiskSpace(UnwrapServiceProviderItems(items));
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error on GetServiceItemsDiskSpace() in '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("'{0}' GetServiceItemsDiskSpace", ProviderSettings.ProviderName);
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(SoapServiceProviderItem[] items, DateTime since)
        {
            try
            {
                Log.WriteStart("'{0}' GetServiceItemsBandwidth", ProviderSettings.ProviderName);
                return Provider.GetServiceItemsBandwidth(UnwrapServiceProviderItems(items), since);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error on GetServiceItemsBandwidth() in '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("'{0}' GetServiceItemsBandwidth", ProviderSettings.ProviderName);
            }
        }

        private ServiceProviderItem[] UnwrapServiceProviderItems(SoapServiceProviderItem[] soapItems)
        {
            if (soapItems == null)
                return null;
            ServiceProviderItem[] items = new ServiceProviderItem[soapItems.Length];
            for (int i = 0; i < items.Length; i++)
                items[i] = SoapServiceProviderItem.Unwrap(soapItems[i]);

            return items;
        }
    }
}

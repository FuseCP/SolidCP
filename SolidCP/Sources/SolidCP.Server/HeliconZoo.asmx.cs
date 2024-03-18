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
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Server.Utils;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
using SolidCP.Providers.HeliconZoo;


namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for HeliconZoo
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class HeliconZoo : HostingServiceProviderWebService, IHeliconZooServer
    {

        private IHeliconZooServer ZooProvider
        {
            get { return (IHeliconZooServer)Provider; }
        }


        [WebMethod, SoapHeader("settings")]
        public HeliconZooEngine[] GetEngines()
        {

            return ZooProvider.GetEngines();

        }

        [WebMethod, SoapHeader("settings")]
        public void SetEngines(HeliconZooEngine[] userEngines)
        {
            ZooProvider.SetEngines(userEngines);
        }

        [WebMethod, SoapHeader("settings")]
        public bool IsEnginesEnabled()
        {
            return ZooProvider.IsEnginesEnabled();
        }

        [WebMethod, SoapHeader("settings")]
        public void SwithEnginesEnabled(bool enabled)
        {
            ZooProvider.SwithEnginesEnabled(enabled);
        }


        [WebMethod, SoapHeader("settings")]
        public string[] GetEnabledEnginesForSite(string siteId)
        {
            return ZooProvider.GetEnabledEnginesForSite(siteId);
        }

        [WebMethod, SoapHeader("settings")]
        public void SetEnabledEnginesForSite(string siteId, string[] engineNames)
        {
            ZooProvider.SetEnabledEnginesForSite(siteId, engineNames);
        }

        [WebMethod, SoapHeader("settings")]
        public bool IsWebCosoleEnabled()
        {
            return ZooProvider.IsWebCosoleEnabled();
        }

        [WebMethod, SoapHeader("settings")]
        public void SetWebCosoleEnabled(bool enabled)
        {
            ZooProvider.SetWebCosoleEnabled(enabled);
        }
    }
}

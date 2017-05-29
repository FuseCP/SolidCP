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
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;


using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.HeliconZoo;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esHeliconZoo
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esHeliconZoo : System.Web.Services.WebService
    {

        [WebMethod]
        public HeliconZooEngine[] GetEngines(int serviceId)
        {
            return HeliconZooController.GetEngines(serviceId);
        }

        [WebMethod]
        public void SetEngines(int serviceId, HeliconZooEngine[] userEngines)
        {
            HeliconZooController.SetEngines(serviceId, userEngines);
        }

        [WebMethod]
        public bool IsEnginesEnabled(int serviceId)
        {
            return HeliconZooController.IsEnginesEnabled(serviceId);
        }

        [WebMethod]
        public void SwithEnginesEnabled(int serviceId, bool enabled)
        {
            HeliconZooController.SwithEnginesEnabled(serviceId, enabled);
        }

        [WebMethod]
        public ShortHeliconZooEngine[] GetAllowedHeliconZooQuotasForPackage(int packageId)
        {
            return HeliconZooController.GetAllowedHeliconZooQuotasForPackage(packageId);
        }

        [WebMethod]
        public string[] GetEnabledEnginesForSite(string siteId, int packageId)
        {
            return HeliconZooController.GetEnabledEnginesForSite(siteId, packageId);
        }

        [WebMethod]
        public void SetEnabledEnginesForSite(string siteId, int packageId, string[] engines)
        {
            HeliconZooController.SetEnabledEnginesForSite(siteId, packageId, engines);
        }

        [WebMethod]
        public bool IsWebCosoleEnabled(int serviceId)
        {
            return HeliconZooController.IsWebCosoleEnabled(serviceId);
        }

        [WebMethod]
        public void SetWebCosoleEnabled(int serviceId, bool enabled)
        {
            HeliconZooController.SetWebCosoleEnabled(serviceId, enabled);
        }
    }
}

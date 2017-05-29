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

using SolidCP.Providers.OS;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esOperatingSystems : System.Web.Services.WebService
    {
        [WebMethod]
        public DataSet GetRawOdbcSourcesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return OperatingSystemController.GetRawOdbcSourcesPaged(packageId, filterColumn,
                filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public string[] GetInstalledOdbcDrivers(int packageId)
        {
            return OperatingSystemController.GetInstalledOdbcDrivers(packageId);
        }

        [WebMethod]
        public List<SystemDSN> GetOdbcSources(int packageId, bool recursive)
        {
            return OperatingSystemController.GetOdbcSources(packageId, recursive);
        }

        [WebMethod]
        public SystemDSN GetOdbcSource(int itemId)
        {
            return OperatingSystemController.GetOdbcSource(itemId);
        }

        [WebMethod]
        public int AddOdbcSource(SystemDSN item)
        {
            return OperatingSystemController.AddOdbcSource(item);
        }

        [WebMethod]
        public int UpdateOdbcSource(SystemDSN item)
        {
            return OperatingSystemController.UpdateOdbcSource(item);
        }

        [WebMethod]
        public int DeleteOdbcSource(int itemId)
        {
            return OperatingSystemController.DeleteOdbcSource(itemId);
        }

        [WebMethod]
        public bool CheckFileServicesInstallation(int serviceId)
        {
            return OperatingSystemController.CheckFileServicesInstallation(serviceId);
        }
    }
}

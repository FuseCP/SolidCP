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
using System.Collections.Generic;
using System.Text;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.RemoteDesktopServices;

namespace SolidCP.Portal
{
	public class RDSHelper
	{
        #region RDS Servers

        RdsServersPaged rdsServers;

        public int GetRDSServersPagedCount(string filterValue)
        {
            //return 4;
            return rdsServers.RecordsCount;
        }

        public RdsServer[] GetRDSServersPaged(int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            rdsServers = ES.Services.RDS.GetRdsServersPaged("", filterValue, sortColumn, startRowIndex, maximumRows);

            foreach (var rdsServer in rdsServers.Servers)
            {
                rdsServer.Status = "...";                
                rdsServer.SslAvailable = ES.Services.RDS.GetRdsCertificateByItemId(rdsServer.ItemId) != null;
            }

            return rdsServers.Servers;            
        }

        public int GetOrganizationRdsServersPagedCount(int itemId, string filterValue)
        {
            return rdsServers.RecordsCount;
        }

        public RdsServer[] GetOrganizationRdsServersPaged(int itemId, int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            rdsServers = ES.Services.RDS.GetOrganizationRdsServersPaged(itemId, null, "", filterValue, sortColumn, startRowIndex, maximumRows);

            return rdsServers.Servers;
        }

        public RdsServer[] GetFreeRDSServers(int packageId)
        {
            return ES.Services.RDS.GetFreeRdsServersPaged(packageId, "", "", "", 0, 1000).Servers;
        }

        #endregion

        #region RDS Collectons

        RdsCollectionPaged rdsCollections;

        public int GetRDSCollectonsPagedCount(int itemId, string filterValue)
        {
            //return 3;
            return rdsCollections.RecordsCount;
        }

        public RdsCollection[] GetRDSCollectonsPaged(int itemId, int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            rdsCollections = ES.Services.RDS.GetRdsCollectionsPaged(itemId, "Name", filterValue, sortColumn, startRowIndex, maximumRows);

            return rdsCollections.Collections;

            //return new RdsCollection[] { new RdsCollection { Name = "Collection 1", Description = "" },
            //                             new RdsCollection { Name = "Collection 2", Description = "" },
            //                             new RdsCollection { Name = "Collection 3", Description = "" }};
        }

        #endregion
    }
}

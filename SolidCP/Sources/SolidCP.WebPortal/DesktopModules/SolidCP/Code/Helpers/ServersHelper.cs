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

using SolidCP.Server;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for ServersHelper
    /// </summary>
    public class ServersHelper
    {
        public DataSet GetRawServers()
        {
            return ES.Services.Servers.GetRawServers();
        }

        public DataSet GetRawVirtualServers()
        {
            return ES.Services.Servers.GetVirtualServers();
        }

        #region Domains Paged ODS Methods
        DataSet dsDomainsPaged;

        public int GetDomainsPagedCount(int packageId, int serverId, bool recursive, string filterColumn, string filterValue)
        {
            return (int)dsDomainsPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetDomainsPaged(int maximumRows, int startRowIndex, string sortColumn,
            int packageId, int serverId, bool recursive, string filterColumn, string filterValue)
        {

            dsDomainsPaged = ES.Services.Servers.GetDomainsPaged(packageId, serverId, recursive,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return dsDomainsPaged.Tables[1];
        }
        #endregion

        #region Event Log Entries Paged
        SystemLogEntriesPaged logEntries;

        public int GetEventLogEntriesPagedCount(string logName)
        {
            return logEntries.Count;
        }

        public SystemLogEntry[] GetEventLogEntriesPaged(string logName, int maximumRows, int startRowIndex)
        {
            logEntries = ES.Services.Servers.GetLogEntriesPaged(PanelRequest.ServerId, logName, startRowIndex, maximumRows);
            return logEntries.Entries;
        }
        #endregion

        #region DNS Zone Records
        public DataSet GetRawDnsZoneRecords(int domainId)
        {
            return ES.Services.Servers.GetRawDnsZoneRecords(domainId);
        }
        #endregion
    }
}

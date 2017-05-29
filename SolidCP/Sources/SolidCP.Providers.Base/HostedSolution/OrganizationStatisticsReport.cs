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

﻿using System.Text;

namespace SolidCP.Providers.HostedSolution
{
    public class OrganizationStatisticsReport : BaseReport<OrganizationStatisticsRepotItem>
    {
        public override string ToCSV()
        {
            StringBuilder mainBuilder = new StringBuilder();
            
            AddCSVHeader(mainBuilder);
            foreach (OrganizationStatisticsRepotItem item in Items)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\n");
                sb.AppendFormat("{0},", ToCsvString(item.TopResellerName));
                sb.AppendFormat("{0},", ToCsvString(item.ResellerName));
                sb.AppendFormat("{0},", ToCsvString(item.CustomerName));
                sb.AppendFormat("{0},", ToCsvString(item.CustomerCreated));
                sb.AppendFormat("{0},", ToCsvString(item.HostingSpace));
                sb.AppendFormat("{0},", ToCsvString(item.HostingSpaceCreated));
                sb.AppendFormat("{0},", ToCsvString(item.OrganizationName));
                sb.AppendFormat("{0},", ToCsvString(item.OrganizationCreated));
                sb.AppendFormat("{0},", ToCsvString(item.OrganizationID));

                sb.AppendFormat("{0},", ToCsvString(item.TotalMailboxes));
                sb.AppendFormat("{0},", ToCsvString(item.TotalMailboxesSize / 1024.0 / 1024.0));
                sb.AppendFormat("{0},", ToCsvString(item.TotalPublicFoldersSize / 1024.0 / 1024.0));
                sb.AppendFormat("{0},", ToCsvString(item.TotalSharePointSiteCollections));
                sb.AppendFormat("{0},", ToCsvString(item.TotalSharePointSiteCollectionsSize / 1024.0 / 1024.0));
                sb.AppendFormat("{0},", ToCsvString(item.TotalCRMUsers));
                sb.AppendFormat("{0},", ToCsvString(item.TotalLyncUsers));
                sb.AppendFormat("{0}", ToCsvString(item.TotalLyncEVUsers));
                sb.AppendFormat("{0},", ToCsvString(item.TotalSfBUsers));
                sb.AppendFormat("{0}", ToCsvString(item.TotalSfBEVUsers));

                mainBuilder.Append(sb.ToString());
            }
            return mainBuilder.ToString();
        }

        private static void AddCSVHeader(StringBuilder sb)
        {
            sb.Append("Top Reseller,Reseller,Customer,Customer Created,Hosting Space,Hosting Space Created,Ogranization Name,Ogranization Created,Organization ID,Total mailboxes,Total mailboxes size(Mb),Total Public Folders size(Mb),Total SharePoint site collections,Total SharePoint site collections size(Mb),Total CRM users,Total Lync users,Total Lync EV users, Total SfB users,Total SfB EV users");
        }
    }
}

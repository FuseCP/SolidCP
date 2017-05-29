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

﻿using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.HostedSolution
{
    public class ExchangeStatisticsReport : BaseReport<ExchangeMailboxStatistics>
    {
    
        public override string ToCSV()
        {
            StringBuilder mainBuilder = new StringBuilder();
            StringBuilder sb = null;
            AddCSVHeader(mainBuilder);
            foreach(ExchangeMailboxStatistics item in Items)
            {
                sb = new StringBuilder();
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

				sb.AppendFormat("{0},", ToCsvString(item.DisplayName));
				sb.AppendFormat("{0},", ToCsvString(item.AccountCreated));
				sb.AppendFormat("{0},", ToCsvString(item.PrimaryEmailAddress));
				sb.AppendFormat("{0},", ToCsvString(item.MAPIEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.OWAEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.ActiveSyncEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.POPEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.IMAPEnabled));
				sb.AppendFormat("{0},", ToCsvString(item.TotalSize / 1024.0 / 1024.0));
				sb.AppendFormat("{0},", UnlimitedToCsvString(item.MaxSize == -1 ? (double)item.MaxSize : item.MaxSize / 1024.0 / 1024.0));
				sb.AppendFormat("{0},", ToCsvString(item.LastLogon));
				sb.AppendFormat("{0},", ToCsvString(item.Enabled, "Enabled", "Disabled"));
				sb.AppendFormat("{0},", ToCsvString(item.MailboxType));
                sb.AppendFormat("{0},", ToCsvString(item.BlackberryEnabled));
                sb.AppendFormat("{0}", ToCsvString(item.MailboxPlan));
                mainBuilder.Append(sb.ToString());
            }
            return mainBuilder.ToString();
        }

		private void AddCSVHeader(StringBuilder sb)
		{
            sb.Append("Top Reseller,Reseller,Customer,Customer Created,Hosting Space,Hosting Space Created,Ogranization Name,Organization Created,Organization ID,Mailbox Display Name,Account Created,Primary E-mail Address,MAPI,OWA,ActiveSync,POP 3,IMAP,Mailbox Size (Mb),Max Mailbox Size (Mb),Last Logon,Enabled,Mailbox Type, BlackBerry, Mailbox Plan");
		}
    }
}

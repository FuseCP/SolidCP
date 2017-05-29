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
using System.Management;
using SolidCP.Providers.Utils;

namespace SolidCP.Providers.HostedSolution
{
	public class OCSEdge2007R2 : HostingServiceProviderBase, IOCSEdgeServer
	{
		#region Properties
		private WmiHelper wmi = null;
		/// <summary>
		/// Wmi helper instance
		/// </summary>
		private WmiHelper Wmi
		{
			get
			{
				if (wmi == null)
					wmi = new WmiHelper("root\\cimv2");
				return wmi;
			}
		}
		#endregion

		#region IOCSEdgeServer implementation

		public void AddDomain(string domainName)
		{
			AddDomainInternal(domainName);
		}

		public void DeleteDomain(string domainName)
		{
			DeleteDomainInternal(domainName);
		}

		#endregion

		#region Domains

		private ManagementObject GetDomain(string domainName)
		{
			HostedSolutionLog.LogStart("GetDomain");
			ManagementObject objDomain = Wmi.GetWmiObject("MSFT_SIPFederationInternalDomainData", "SupportedInternalDomain='{0}'", domainName);
			HostedSolutionLog.LogEnd("GetDomain");	
			return objDomain;
				
		}

		private void AddDomainInternal(string domainName)
		{
			HostedSolutionLog.LogStart("AddDomainInternal");
			HostedSolutionLog.DebugInfo("Domain Name: {0}", domainName);
			try
			{
				if (string.IsNullOrEmpty(domainName))
					throw new ArgumentException("domainName");

				if ( GetDomain(domainName) != null )
				{
					HostedSolutionLog.LogWarning("OCS internal domain '{0}' already exists", domainName);
				}
				else
				{
					using (ManagementObject newDomain = Wmi.CreateInstance("MSFT_SIPFederationInternalDomainData"))
					{
						newDomain["SupportedInternalDomain"] = domainName;
						newDomain.Put();
					}
				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("AddDomainInternal", ex);
				throw;
			}
			HostedSolutionLog.LogEnd("AddDomainInternal");	
		}

		private void DeleteDomainInternal(string domainName)
		{
			HostedSolutionLog.LogStart("DeleteDomainInternal");
			HostedSolutionLog.DebugInfo("Domain Name: {0}", domainName);
			try
			{
				if (string.IsNullOrEmpty(domainName))
					throw new ArgumentException("domainName");

				using (ManagementObject domainObj = GetDomain(domainName))
				{
					if (domainObj == null)
						HostedSolutionLog.LogWarning("OCS internal domain '{0}' not found", domainName);
					else
						domainObj.Delete();
				}
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError("DeleteDomainInternal", ex);
				throw;
			}
			HostedSolutionLog.LogEnd("DeleteDomainInternal");
		}

		#endregion

		public override bool IsInstalled()
		{
			try
			{
				Wmi.GetWmiObjects("MSFT_SIPFederationInternalDomainData", null);
				return true;
			}
			catch
			{
				return false;
			}
		}

        
	}
}


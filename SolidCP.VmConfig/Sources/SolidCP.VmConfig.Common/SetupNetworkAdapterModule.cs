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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.VmConfig
{
	public class SetupNetworkAdapterModule : IProvisioningModule
	{
		internal const string RegistryKey = @"SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}";
		#region IProvisioningModule Members

		public ExecutionResult Run(ref ExecutionContext context)
		{
			ExecutionResult ret = new ExecutionResult();
			ret.ResultCode = 0;
			ret.ErrorMessage = null;
			ret.RebootRequired = false;

			context.ActivityDescription = "Configuring network adapter...";
			context.Progress = 0;
			if (!CheckParameter(context, "MAC"))
			{
				ProcessError(context, ret, null, 2, "Parameter 'MAC' is not specified");
				return ret;
			}
			string macAddress = context.Parameters["MAC"];
			if (!IsValidMACAddress(macAddress))
			{
				ProcessError(context, ret, null, 2, "Parameter 'MAC' has invalid format. It should be in 12:34:56:78:90:ab format.");
				return ret;
			}
			
			string adapterId = null;
			ManagementObject objAdapter = null;
			ManagementObjectCollection objAdapters = null;
			int attempts = 0;
			try
			{
				WmiUtils wmi = new WmiUtils("root\\cimv2");
				string query = string.Format("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE MACAddress = '{0}' AND IPEnabled = True", macAddress);
				//try to find adapter for 10 times
				while (true)
				{

					objAdapters = wmi.ExecuteQuery(query);

					if (objAdapters.Count > 0)
					{
						foreach (ManagementObject adapter in objAdapters)
						{
							objAdapter = adapter;
							adapterId = (string)adapter["SettingID"];
						}
						break;

					}
					if (attempts > 9)
					{
						ProcessError(context, ret, null, 2, "Network adapter not found");
						return ret;
					}
					
					attempts++;
                    Log.WriteError(string.Format("Attempt #{0} to find network adapter (mac: {1}) failed!", attempts, macAddress));
					// wait 1 min
					System.Threading.Thread.Sleep(60000);
					//repeat loop
				}
				
			}
			catch (Exception ex)
			{
				ProcessError(context, ret, ex, 2, "Network adapter configuration error: ");
				return ret;
			}

			if (CheckParameter(context, "EnableDHCP", "True"))
			{
				try
				{
					EnableDHCP(objAdapter);
				}
				catch (Exception ex)
				{
					ProcessError(context, ret, ex, 2, "DHCP error: ");
					return ret;
				}

			}
			else if (CheckParameter(context, "EnableDHCP", "False"))
			{
				if (!CheckParameter(context, "DefaultIPGateway"))
				{
					ProcessError(context, ret, null, 2, "Parameter 'DefaultIPGateway' is not specified");
					return ret;
				}
				if (!CheckParameter(context, "IPAddress"))
				{
					ProcessError(context, ret, null, 2, "Parameter 'IPAddresses' is not specified");
					return ret;
				}
				if (!CheckParameter(context, "SubnetMask"))
				{
					ProcessError(context, ret, null, 2, "Parameter 'SubnetMasks' is not specified");
					return ret;
				}
				try
				{
					DisableDHCP(context, objAdapter);
				}
				catch (Exception ex)
				{
					ProcessError(context, ret, ex, 2, "Network adapter configuration error: ");
					return ret;
				}
			}
			if (CheckParameter(context, "PreferredDNSServer"))
			{
				try
				{
					SetDNSServer(context, objAdapter);
				}
				catch (Exception ex)
				{
					ProcessError(context, ret, ex, 2, "DNS error: ");
					return ret;
				}

			}
			context.Progress = 100;
			return ret;
		}

		private void SetDNSServer(ExecutionContext context, ManagementObject objAdapter)
		{
			string[] dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
			Log.WriteStart("Configuring DNS...");
			ManagementBaseObject objDNS = objAdapter.GetMethodParameters("SetDNSServerSearchOrder");
			objDNS["DNSServerSearchOrder"] = dnsServers;
			if (dnsServers.Length == 1 && dnsServers[0] == "0.0.0.0")
			{
				objDNS["DNSServerSearchOrder"] = new string[] { };
			}
			ManagementBaseObject objRet = objAdapter.InvokeMethod("SetDNSServerSearchOrder", objDNS, null);
			Log.WriteEnd("DNS configured");
		}

		private static string[] ParseArray(string array)
		{
			if (string.IsNullOrEmpty(array))
				throw new ArgumentException("array");
			string[] ret = array.Split(';');
			return ret;
		}

		private static void ProcessError(ExecutionContext context, ExecutionResult ret, Exception ex, int errorCode, string errorPrefix)
		{
			ret.ResultCode = errorCode;
			ret.ErrorMessage = errorPrefix;
			if (ex != null)
				ret.ErrorMessage += ex.ToString();
			Log.WriteError(ret.ErrorMessage);
			context.Progress = 100;
		}

		private static void EnableDHCP(ManagementObject adapter)
		{
			Log.WriteStart("Enabling DHCP...");
			object ret1 = adapter.InvokeMethod("EnableDHCP", new object[] { });
			object ret2 = adapter.InvokeMethod("RenewDHCPLease", new object[] { });
			Log.WriteEnd("DHCP enabled");
		}

		private static void DisableDHCP(ExecutionContext context, ManagementObject adapter)
		{
			string[] ipGateways = ParseArray(context.Parameters["DefaultIPGateway"]);
			string[] ipAddresses = ParseArray(context.Parameters["IPAddress"]);
			string[] subnetMasks = ParseArray(context.Parameters["SubnetMask"]);
			if (subnetMasks.Length != ipAddresses.Length)
			{
				throw new ArgumentException("Number of Subnet Masks should be equal to IP Addresses");
			}

			ManagementBaseObject objNewIP = null;
			ManagementBaseObject objSetIP = null;
			ManagementBaseObject objNewGateway = null;


			objNewIP = adapter.GetMethodParameters("EnableStatic");
			objNewGateway = adapter.GetMethodParameters("SetGateways");


			//Set DefaultGateway
			objNewGateway["DefaultIPGateway"] = ipGateways;
			int[] cost = new int[ipGateways.Length];
			for (int i = 0; i < cost.Length; i++)
				cost[i] = 1;		
			objNewGateway["GatewayCostMetric"] = cost;


			//Set IPAddress and Subnet Mask
			objNewIP["IPAddress"] = ipAddresses;
			objNewIP["SubnetMask"] = subnetMasks;

			Log.WriteStart("Configuring static IP...");
			objSetIP = adapter.InvokeMethod("EnableStatic", objNewIP, null);
			Log.WriteEnd("IP configured");

			Log.WriteStart("Configuring default gateway...");
			objSetIP = adapter.InvokeMethod("SetGateways", objNewGateway, null);
			Log.WriteEnd("Default gateway configured");
		}

		private static bool IsValidMACAddress(string mac)
		{
			
			return Regex.IsMatch(mac, "^([0-9a-fA-F]{2}:){5}[0-9a-fA-F]{2}$");
		}



		private static bool CheckParameter(ExecutionContext context, string name)
		{
			return (context.Parameters.ContainsKey(name) && !string.IsNullOrEmpty(context.Parameters[name]));
		}

		private static bool CheckParameter(ExecutionContext context, string name, string value)
		{
			return (context.Parameters.ContainsKey(name) && context.Parameters[name] == value);
		}

		private void ChangeMACAddress(string adapterId, string mac)
		{
			string[] sets = RegistryUtils.GetRegistrySubKeys(RegistryKey);
			foreach (string set in sets)
			{
				string key = string.Format("{0}\\{1}", RegistryKey, set);
				string netCfgInstanceId = RegistryUtils.GetRegistryKeyStringValue(key, "NetCfgInstanceId");
				if (netCfgInstanceId == adapterId)
				{
					Log.WriteStart("Changing MAC address...");
					RegistryUtils.SetRegistryKeyStringValue(key, "NetworkAddress", mac);
					Log.WriteEnd("MAC address has been changed successfully");
					break;
				}
			}
		}

		#endregion
	}
}

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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SolidCP.Server.Utils
{
	public class PInvoke
	{
		public static class RegistryHive
		{
			/// <summary>
			/// Implements common methods to manipulate on a registry section's sub keys and their values
			/// </summary>
			public class RegistryHiveSection
			{
				private UIntPtr HiveSection;

				public RegistryHiveSection(UIntPtr hiveSection)
				{
					this.HiveSection = hiveSection;
				}

				/// <summary>
				/// Provide seamless dev experience bypassing Registry Redirector feature 
				/// to test the registry hive for a key existence on x86/x64 platforms.
				/// </summary>
				/// <param name="name">Registry key path being tested</param>
				/// <returns></returns>
				private bool SubKeyExists(string name, int samDesired)
				{
					UIntPtr hSubKey;
					// Open key
					int result = PInvoke.RegOpenKeyEx(HiveSection, name, 0, samDesired, out hSubKey);
					// Close key
					PInvoke.RegCloseKey(hSubKey);
					//
					return (result == 0);
				}

				private string GetSubKeyValue(string keyPath, string keyValue, int samDesired)
				{
					if (SubKeyExists(keyPath, samDesired))
					{
						UIntPtr hSubKey;
						uint size = 1024;
						uint type;
						string valueStr = null;
						StringBuilder keyBuffer = new StringBuilder(unchecked((int)size));
						// Open key
						if (PInvoke.RegOpenKeyEx(HiveSection, keyPath, 0, samDesired, out hSubKey) == 0)
						{
							try
							{
								//
								if (PInvoke.RegQueryValueEx(hSubKey, keyValue, 0, out type, keyBuffer, ref size) == 0)
									valueStr = keyBuffer.ToString();
							}
							catch (Exception ex)
							{
								Log.WriteError(ex);
							}
							finally
							{
								// Close key
								PInvoke.RegCloseKey(hSubKey);
							}
						}

						return (valueStr);
					}
					//
					return null;
				}

				private int GetDwordSubKeyValue(string keyPath, string keyValue, int samDesired)
				{
					if (SubKeyExists(keyPath, samDesired))
					{
						UIntPtr hSubKey;
						uint size = 1024;
						// REG_DWORD
						uint type = (uint)4;
						//
						int valueInt = 0;
						// Open key
						if (PInvoke.RegOpenKeyEx(HiveSection, keyPath, 0, samDesired, out hSubKey) == 0)
						{
							try
							{
								//
								if (PInvoke.RegQueryValueEx(hSubKey, keyValue, 0, ref type, ref valueInt, ref size) == 0)
									return valueInt;
							}
							catch (Exception ex)
							{
								Log.WriteError(ex);
							}
							finally
							{
								// Close key
								PInvoke.RegCloseKey(hSubKey);
							}
						}
						//
						return (valueInt);
					}
					//
					return 0;
				}

				/// <summary>
				/// Provide seamless dev experience bypassing Registry Redirector feature 
				/// to test the registry for a key existence beneath WOW6432Node on x64 platform (even for 64-bit ASP.NET app pool).
				/// Works seamlessly on x64 platform.
				/// </summary>
				/// <param name="name">Registry key path being tested</param>
				/// <returns></returns>
				public bool SubKeyExists_x86(string name)
				{
					return SubKeyExists(name, KEY_READ | KEY_WOW64_32KEY);
				}

				/// <summary>
				/// Provide seamless dev experience bypassing Registry Redirector feature 
				/// to test the registry for a key existence beneath WOW6464Node on x64 platform (even for 32-bit ASP.NET app pool).
				/// Works seamlessly on x86 platform.
				/// </summary>
				/// <param name="name">Registry key path being tested</param>
				/// <returns></returns>
				public bool SubKeyExists_x64(string name)
				{
					return SubKeyExists(name, KEY_READ | KEY_WOW64_64KEY);
				}

				/// <summary>
				/// Provide seamless dev experience bypassing Registry Redirector feature 
				/// to get registry key value beneath WOW6464Node on x64 platform (even for 32-bit ASP.NET app pool).
				/// Works seamlessly on x86 platform.
				/// </summary>
				/// <param name="keyPath">Registry key path being queried</param>
				/// <param name="keyValue">Registry key value name</param>
				/// <returns></returns>
				public int GetDwordSubKeyValue_x64(string keyPath, string keyValue)
				{
					return GetDwordSubKeyValue(keyPath, keyValue, KEY_READ | KEY_WOW64_64KEY);
				}

				/// <summary>
				/// Provide seamless dev experience bypassing Registry Redirector feature 
				/// to get registry key value beneath WOW6464Node on x64 platform (even for 32-bit ASP.NET app pool).
				/// Works seamlessly on x86 platform.
				/// </summary>
				/// <param name="keyPath">Registry key path being queried</param>
				/// <param name="keyValue">Registry key value name</param>
				/// <returns></returns>
				public string GetSubKeyValue_x64(string keyPath, string keyValue)
				{
					return GetSubKeyValue(keyPath, keyValue, KEY_READ | KEY_WOW64_64KEY);
				}

				/// <summary>
				/// Provide seamless dev experience bypassing Registry Redirector feature 
				/// to get registry key value beneath WOW6432Node section on x64 platform (even for 64-bit ASP.NET app pool).
				/// Works seamlessly on x86 platform.
				/// </summary>
				/// <param name="keyPath">Registry key path being queried</param>
				/// <param name="keyValue">Registry key value name</param>
				/// <returns></returns>
				public string GetSubKeyValue_x86(string keyPath, string keyValue)
				{
					return GetSubKeyValue(keyPath, keyValue, KEY_READ | KEY_WOW64_32KEY);
				}

				/// <summary>
				/// Provide seamless dev experience bypassing Registry Redirector feature 
				/// to get registry key value beneath WOW6432Node section on x64 platform (even for 64-bit ASP.NET app pool).
				/// Works seamlessly on x86 platform.
				/// </summary>
				/// <param name="keyPath">Registry key path being queried</param>
				/// <param name="keyValue">Registry key value name</param>
				/// <returns></returns>
				public int GetDwordSubKeyValue_x86(string keyPath, string keyValue)
				{
					return GetDwordSubKeyValue(keyPath, keyValue, KEY_READ | KEY_WOW64_32KEY);
				}
			};

			/// <summary>
			/// Represents HKEY_LOCAL_MACHINE section
			/// </summary>
			public static RegistryHiveSection HKLM = new RegistryHiveSection(PInvoke.HKEY_LOCAL_MACHINE);
		};

		#region Wrappers definitions for Windows Native API functions

		public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);

		public const int KEY_READ = 0x20019;
		public const int KEY_WOW64_32KEY = 0x0200;
		public const int KEY_WOW64_64KEY = 0x0100;

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		public static extern int RegOpenKeyEx(UIntPtr hKey, string subKey, int ulOptions, int samDesired, out UIntPtr hkResult);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int RegCloseKey(UIntPtr hKey);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
		static extern int RegQueryValueEx(UIntPtr hKey, [MarshalAs(UnmanagedType.LPWStr)]string lpValueName, int lpReserved, out uint lpType, [Optional] System.Text.StringBuilder lpData, ref uint lpcbData);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
		static extern int RegQueryValueEx(UIntPtr hKey, [MarshalAs(UnmanagedType.LPWStr)]string lpValueName, int lpReserved, ref uint lpType, [Optional] ref int lpData, ref uint lpcbData);

		#endregion
	}
}

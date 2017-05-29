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
using Microsoft.Win32;

namespace SolidCP.VmConfig
{
	/// <summary>
	/// Registry helper class.
	/// </summary>
	public sealed class RegistryUtils
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private RegistryUtils()
		{
		}

		/// <summary>
		/// Retrieves the specified value from the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to retrieve.</param>
		/// <returns>The data associated with name.</returns>
		public static string GetRegistryKeyStringValue(string subkey, string name)
		{
			string ret = null;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
			{
				ret = (string)rk.GetValue(name, string.Empty);
			}
			return ret;
		}

		/// <summary>
		/// Retrieves the specified value from the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to retrieve.</param>
		/// <returns>The data associated with name.</returns>
		public static int GetRegistryKeyInt32Value(string subkey, string name)
		{
			int ret = 0;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
			{
				ret = (int)rk.GetValue(name, 0);
			}
			return ret;
		}

		/// <summary>
		/// Retrieves the specified value from the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to retrieve.</param>
		/// <returns>The data associated with name.</returns>
		public static bool GetRegistryKeyBooleanValue(string subkey, string name)
		{
			bool ret = false;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
			{
				string strValue = (string)rk.GetValue(name, "False");
				ret = Boolean.Parse(strValue);
			}
			return ret;
		}

		public static bool RegistryKeyExist(string subkey)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			return (rk != null);
		}


		/// <summary>
		/// Deletes a registry subkey and any child subkeys.
		/// </summary>
		/// <param name="subkey">Subkey to delete.</param>
		public static void DeleteRegistryKey(string subkey)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
				root.DeleteSubKeyTree(subkey);
		}

		/// <summary>
		/// Sets the specified value to the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to store data in.</param>
		/// <param name="value">Data to store. </param>
		public static void SetRegistryKeyStringValue(string subkey, string name, string value)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.CreateSubKey(subkey);
			if (rk != null)
			{
				rk.SetValue(name, value);
			}
		}

		/// <summary>
		/// Sets the specified value to the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to store data in.</param>
		/// <param name="value">Data to store. </param>
		public static void SetRegistryKeyInt32Value(string subkey, string name, int value)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.CreateSubKey(subkey);
			if (rk != null)
			{
				rk.SetValue(name, value);
			}
		}

		/// <summary>
		/// Sets the specified value to the subkey.
		/// </summary>
		/// <param name="subkey">Subkey.</param>
		/// <param name="name">Name of value to store data in.</param>
		/// <param name="value">Data to store. </param>
		public static void SetRegistryKeyBooleanValue(string subkey, string name, bool value)
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.CreateSubKey(subkey);
			if (rk != null)
			{
				rk.SetValue(name, value);
			}
		}

		/// <summary>
		/// Return the list of sub keys for the specified registry key.
		/// </summary>
		/// <param name="subkey">The name of the registry key</param>
		/// <returns>The array of subkey names.</returns>
		public static string[] GetRegistrySubKeys(string subkey)
		{
			string[] ret = new string[0];
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
				ret = rk.GetSubKeyNames();

			return ret;
		}

		public static int GetSubKeyCount(string subkey)
		{
			int ret = 0;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(subkey);
			if (rk != null)
				ret = rk.SubKeyCount;

			return ret;
		}

		/// <summary>
		/// Return the list of value names for the specified registry key.
		/// </summary>
		/// <param name="subkey">The name of the registry key</param>
		/// <returns>The array of value names.</returns>
		public static string[] GetRegistryKeyValueNames(string subkey)
		{
			string[] ret = new string[0];
			RegistryKey root = Registry.LocalMachine;
			using (RegistryKey rk = root.OpenSubKey(subkey))
			{
				if (rk != null)
					ret = rk.GetValueNames();
			}
			return ret;
		}

		/// <summary>
		/// Deletes registry key value.
		/// </summary>
		public static void DeleteRegistryKeyValue(string subkey, string valueName)
		{
			RegistryKey root = Registry.LocalMachine;
			using (RegistryKey rk = root.OpenSubKey(subkey, true))
			{
				if (rk != null)
					rk.DeleteValue(valueName);
			}
		}
	}
}

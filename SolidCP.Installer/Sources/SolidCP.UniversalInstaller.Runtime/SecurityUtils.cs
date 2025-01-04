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
using System.IO;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Generic;
using System.Management;
using System.Globalization;

namespace SolidCP.UniversalInstaller.Runtime;

/// <summary>
/// Security utils class.
/// </summary>
public class SecurityUtils: UniversalInstaller.SecurityUtils
{
	private static WmiHelper wmi = new WmiHelper("root\\cimv2");

	#region private constants
	private const int UF_SCRIPT = 0x0001;
	private const int UF_ACCOUNTDISABLE = 0x0002;
	private const int UF_HOMEDIR_REQUIRED = 0x0008;
	private const int UF_LOCKOUT = 0x0010;
	private const int UF_PASSWD_NOTREQD = 0x0020;
	private const int UF_PASSWD_CANT_CHANGE = 0x0040;
	private const int UF_TEMP_DUPLICATE_ACCOUNT = 0x0100;
	private const int UF_NORMAL_ACCOUNT = 0x0200;
	private const int UF_INTERDOMAIN_TRUST_ACCOUNT = 0x0800;
	private const int UF_WORKSTATION_TRUST_ACCOUNT = 0x1000;
	private const int UF_SERVER_TRUST_ACCOUNT = 0x2000;
	private const int UF_DONT_EXPIRE_PASSWD = 0x10000;
	private const int UF_MNS_LOGON_ACCOUNT = 0x20000;
	#endregion

	#region NTFS permissions
	/// <summary>
	/// Grants NTFS permissions by username
	/// </summary>
	/// <param name="path"></param>
	/// <param name="accountName"></param>
	/// <param name="permissions"></param>
	/// <param name="inheritParentPermissions"></param>
	/// <param name="preserveOriginalPermissions"></param>
	public override void GrantNtfsPermissions(string path, string domain, string accountName,
		NtfsPermission permissions, bool inheritParentPermissions, bool preserveOriginalPermissions)
	{
		GrantNtfsPermissionsBySid(path, GetSid(accountName, domain), permissions, inheritParentPermissions,
			preserveOriginalPermissions);
	}

	/// <summary>
	/// Grants NTFS permissions by SID
	/// </summary>
	/// <param name="path"></param>
	/// <param name="sid"></param>
	/// <param name="permissions"></param>
	/// <param name="inheritParentPermissions"></param>
	/// <param name="preserveOriginalPermissions"></param>
	public override void GrantNtfsPermissionsBySid(string path, string sid,
		NtfsPermission permissions, bool inheritParentPermissions, bool preserveOriginalPermissions)
	{
		// remove trailing slash if any
		if(path.EndsWith("\\"))
			path = path.Substring(0, path.Length - 1);

		// get security settings
		ManagementObject logicalFileSecuritySetting = wmi.GetObject(String.Format(
			"Win32_LogicalFileSecuritySetting.Path='{0}'", path));

		// get original security descriptor
		ManagementBaseObject outParams = logicalFileSecuritySetting.InvokeMethod("GetSecurityDescriptor", null, null);
		ManagementBaseObject originalDescriptor = ((ManagementBaseObject)(outParams.Properties["Descriptor"].Value));
		
		// create new descriptor
		ManagementBaseObject descriptor = wmi.GetClass("Win32_SecurityDescriptor").CreateInstance();
		descriptor.Properties["ControlFlags"].Value = inheritParentPermissions ? (uint)33796 : (uint)37892;

		// get original ACEs
		ManagementBaseObject[] originalAces = ((ManagementBaseObject[])(originalDescriptor.Properties["DACL"].Value ) );

		// create a new ACEs list
		List<ManagementBaseObject> aces = new List<ManagementBaseObject>();

		// copy original ACEs if required
		if(preserveOriginalPermissions)
		{
			foreach(ManagementBaseObject originalAce in originalAces)
			{
				// we don't want to include inherited and current ACEs
				ManagementBaseObject objTrustee = (ManagementBaseObject)originalAce.Properties["Trustee"].Value;
				string trusteeSid = (string)objTrustee.Properties["SIDString"].Value;
				bool inheritedAce = ((AceFlags)originalAce.Properties["AceFlags"].Value & AceFlags.INHERITED_ACE) > 0;
				if(String.Compare(trusteeSid, sid, true) != 0 && !inheritedAce)
					aces.Add(originalAce);
			}
		}

		// create new trustee object
		ManagementObject trustee = GetTrustee(sid);

		// system access mask
		uint mask = 0;
		if((permissions & NtfsPermission.FullControl) > 0)
			mask |= 0x1f01ff;
		if((permissions & NtfsPermission.Modify) > 0)
			mask |= 0x1301bf;
		if((permissions & NtfsPermission.Write) > 0)
			mask |= 0x100116 | 0x10000 | 0x40;
		if((permissions & NtfsPermission.Read) > 0)
			mask |= 0x120089;

		bool executeEnabled = ((permissions & NtfsPermission.Execute) > 0);
		bool listEnabled = ((permissions & NtfsPermission.ListFolderContents) > 0);

		bool equalState = (executeEnabled == listEnabled);

            
		// create and add to be modified ACE
		ManagementObject ace;
		if(equalState ||
			(permissions & NtfsPermission.FullControl) > 0 ||
			(permissions & NtfsPermission.Modify) > 0) // both "Execute" and "List" enabled or disabled
		{
			if((permissions & NtfsPermission.Execute) > 0)
				mask |= (uint)SystemAccessMask.FILE_TRAVERSE;

			ace = wmi.GetClass("Win32_Ace").CreateInstance();
			ace["Trustee"] = trustee;
			ace["AceFlags"] = AceFlags.OBJECT_INHERIT_ACE | AceFlags.CONTAINER_INHERIT_ACE;
			ace["AceType"] = 0; // "Allow" type
			ace["AccessMask"] = mask;
			aces.Add(ace);
		}
		else // either "Execute" or "List" enabled or disabled
		{
			// we should place a separate permissions for folders and files
			// add FOLDER specific permissions
			uint foldersMask = mask;
			if((permissions & NtfsPermission.ListFolderContents) > 0)
				foldersMask |= (uint)SystemAccessMask.FILE_TRAVERSE;

			ace = wmi.GetClass("Win32_Ace").CreateInstance();
			ace["Trustee"] = trustee;
			ace["AceFlags"] = AceFlags.CONTAINER_INHERIT_ACE;
			ace["AceType"] = 0; // "Allow" type
			ace["AccessMask"] = foldersMask; // set default permissions
			aces.Add(ace);

			// add files specific permissions
			uint filesMask = mask;
			if((permissions & NtfsPermission.Execute) > 0)
				filesMask |= (uint)SystemAccessMask.FILE_TRAVERSE;

			ace = wmi.GetClass("Win32_Ace").CreateInstance();
			ace["Trustee"] = trustee;
			ace["AceFlags"] = AceFlags.OBJECT_INHERIT_ACE;
			ace["AceType"] = 0; // "Allow" type
			ace["AccessMask"] = filesMask; // set default permissions
			aces.Add(ace);
		}

		// set newly created ACEs
		ManagementBaseObject[] newAces = aces.ToArray();
		descriptor.Properties["DACL"].Value = newAces;

		// set security descriptor
		ManagementBaseObject inParams = logicalFileSecuritySetting.GetMethodParameters("SetSecurityDescriptor");
		inParams["Descriptor"] = descriptor;
		outParams = logicalFileSecuritySetting.InvokeMethod("SetSecurityDescriptor", inParams, null);

		// check results
		uint result = (uint)(outParams.Properties["ReturnValue"].Value);

		logicalFileSecuritySetting.Dispose();
	}

	/// <summary>
	/// Removes NTFS permissions by username
	/// </summary>
	/// <param name="path"></param>
	/// <param name="accountName"></param>
	public override void RemoveNtfsPermissions(string path, string accountName)
	{
		RemoveNtfsPermissionsBySid(path, GetSid(accountName));
	}

	/// <summary>
	/// Removes NTFS permissions by SID
	/// </summary>
	/// <param name="path"></param>
	/// <param name="sid"></param>
	public override void RemoveNtfsPermissionsBySid(string path, string sid)
	{

		// remove trailing slash if any
		if(path.EndsWith("\\"))
			path = path.Substring(0, path.Length - 1);

		// get security settings
		ManagementObject logicalFileSecuritySetting = wmi.GetObject(String.Format(
			"Win32_LogicalFileSecuritySetting.Path='{0}'", path));

		// get original security descriptor
		ManagementBaseObject outParams = logicalFileSecuritySetting.InvokeMethod("GetSecurityDescriptor", null, null);
		ManagementBaseObject originalDescriptor = ((ManagementBaseObject)(outParams.Properties["Descriptor"].Value));
		
		// create new descriptor
		ManagementBaseObject descriptor = wmi.GetClass("Win32_SecurityDescriptor").CreateInstance();
		descriptor.Properties["ControlFlags"].Value = originalDescriptor.Properties["ControlFlags"].Value;

		// get original ACEs
		ManagementBaseObject[] originalAces = ((ManagementBaseObject[])(originalDescriptor.Properties["DACL"].Value ) );

		// create a new ACEs list
		List<ManagementBaseObject> aces = new List<ManagementBaseObject>();

		// copy original ACEs if required
		foreach(ManagementBaseObject originalAce in originalAces)
		{
			// we don't want to include inherited and current ACEs
			ManagementBaseObject objTrustee = (ManagementBaseObject)originalAce.Properties["Trustee"].Value;
			string trusteeSid = (string)objTrustee.Properties["SIDString"].Value;
			bool inheritedAce = ((AceFlags)originalAce.Properties["AceFlags"].Value & AceFlags.INHERITED_ACE) > 0;
			if(String.Compare(trusteeSid, sid, true) != 0 && !inheritedAce)
				aces.Add(originalAce);
		}

		// set newly created ACEs
		ManagementBaseObject[] newAces = aces.ToArray();
		descriptor.Properties["DACL"].Value = newAces;

		// set security descriptor
		ManagementBaseObject inParams = logicalFileSecuritySetting.GetMethodParameters("SetSecurityDescriptor");
		inParams["Descriptor"] = descriptor;
		outParams = logicalFileSecuritySetting.InvokeMethod("SetSecurityDescriptor", inParams, null);

		// check results
		uint result = (uint)(outParams.Properties["ReturnValue"].Value);

		logicalFileSecuritySetting.Dispose();
	}

	/// <summary>
	/// Resets NTFS permissions by username
	/// </summary>
	/// <param name="path"></param>
	public override void ResetNtfsPermissions(string path)
	{
		// remove trailing slash if any
		if(path.EndsWith("\\"))
			path = path.Substring(0, path.Length - 1);

		ManagementObject logicalFileSecuritySetting = wmi.GetObject(String.Format(
			"Win32_LogicalFileSecuritySetting.Path='{0}'", path));

		// get security descriptor
		ManagementBaseObject outParams =
			logicalFileSecuritySetting.InvokeMethod("GetSecurityDescriptor",
			null, null);
                
		ManagementObject newDescriptor = wmi.GetClass("Win32_SecurityDescriptor").CreateInstance();

		newDescriptor.Properties["DACL"].Value = new ManagementBaseObject[]{};
		newDescriptor.Properties["ControlFlags"].Value = 33796; // inherit permissions

		ManagementBaseObject inParams = null;
		inParams = logicalFileSecuritySetting.GetMethodParameters("SetSecurityDescriptor");
		inParams["Descriptor"] = newDescriptor;
		outParams = logicalFileSecuritySetting.InvokeMethod("SetSecurityDescriptor",
			inParams, null);

		// This line is where I get a result back of 1307 in ASP.NET
		uint result = (uint)(outParams.Properties["ReturnValue"].Value);

		logicalFileSecuritySetting.Dispose();
	}

	private ManagementObject GetTrustee(string sid)
	{
		// try to get user account
		ManagementObject account = wmi.GetObject(String.Format(
			"Win32_SID.SID='{0}'",
			sid));

		string userAccount = (string)account.Properties["AccountName"].Value;
		string domain = (string)account.Properties["ReferencedDomainName"].Value;

		ManagementObject trustee = wmi.GetClass("Win32_Trustee").CreateInstance();
		trustee.Properties["Domain"].Value = domain;
		trustee.Properties["Name"].Value = userAccount;
		trustee.Properties["SIDString"].Value = sid;
		return trustee;
	}

	/// <summary>
	/// Returns SID by account name
	/// </summary>
	/// <param name="userAccount"></param>
	/// <returns></returns>
	public override string GetSid(string userAccount)
	{
		return GetSid(userAccount, null);
	}

	/// <summary>
	/// Returns SID by account name and domain
	/// </summary>
	/// <param name="userAccount"></param>
	/// <param name="domain"></param>
	/// <returns></returns>
	public override string GetSid(string userAccount, string domain)
	{            
		if(string.IsNullOrWhiteSpace(domain))
			domain = Environment.MachineName;

		// try to get user account
		string sid = null;
		ManagementObject account = null;
		try
		{
			account = wmi.GetObject(String.Format(
				"Win32_Account.Name='{0}',Domain='{1}'",
				userAccount, domain));

			sid = account.Properties["SID"].Value.ToString();
		}
		catch{}

		if(sid == null)
		{
			try
			{
				// try to look in DOMAIN
				account = wmi.GetObject(String.Format(
					"Win32_Account.Name='{0}',Domain='{1}'",
					userAccount, Environment.UserDomainName));

				sid = account.Properties["SID"].Value.ToString();
			}
			catch{}
		}

		return sid;
	}

	private string GetUserPath(string netbiosDomain, string userName)
	{
		if (string.IsNullOrEmpty(netbiosDomain))
			netbiosDomain = Environment.MachineName;

		return string.Format("WinNT://{0}/{1}", netbiosDomain, userName);
	}

	/// <summary>
	/// Returns account name by SID
	/// </summary>
	/// <param name="sid"></param>
	/// <returns></returns>
	public override string GetAccountName(string sid)
	{
		// try to get user account
		ManagementObject account = wmi.GetObject(String.Format("Win32_SID.SID='{0}'", sid));
		return (account != null) ? account.Properties["AccountName"].Value.ToString() : null;
	}
	#endregion

	#region User and Group methods 
	
	/// <summary>
	/// Check for existing user
	/// </summary>
    public override bool UserExists(string domain, string userName)
	{
		bool found = false;
		// check whether user account already exists
		if (string.IsNullOrEmpty(domain))
		{

			found = (wmi.ExecuteQuery(
				String.Format("SELECT * FROM Win32_UserAccount WHERE Domain='{0}' AND Name='{1}'",
					Environment.MachineName, userName))).Count > 0;
		}
		else
		{
			string path = GetUserDN(domain, userName);
			if ( !string.IsNullOrEmpty(path))
			{
				found = ADObjectExists(path);
			}
		}
		return found;
	}

	private string GetUserDN(string domain, string user)
	{
		string ret = null;
		string path = GetDomainUsersContainer(domain);
		if (!string.IsNullOrEmpty(path))
		{
			path = path.Substring(7);
			ret = string.Format("LDAP://CN={0},{1}", user, path);
		}
		return ret;
	}

	public override string GetFullDomainName(string friendlyDomainName)
	{
		string ldapPath = null;
		try
		{
			DirectoryContext objContext = new DirectoryContext(DirectoryContextType.Domain, friendlyDomainName);
			Domain objDomain = Domain.GetDomain(objContext);
			ldapPath = objDomain.Name;
		}
		catch(Exception ex)
		{
			Log.WriteError("Get domain name error", ex);
		}
		return ldapPath;
	}

	public override DirectoryEntry GetDomainEntry(string domainName)
	{
		DirectoryEntry de = null;
		try
		{
			DirectoryContext objContext = new DirectoryContext(DirectoryContextType.Domain, domainName);
			Domain objDomain = Domain.GetDomain(objContext);
			de = objDomain.GetDirectoryEntry();
		}
		catch (Exception ex)
		{
			Log.WriteError("Get domain entry error", ex);
		}
		return de;
	}

	/// <summary>
	/// Creates user
	/// </summary>
	/// <param name="userInfo">User</param>
	public override void CreateUser(SystemUserItem userInfo)
	{
		try
		{
			DirectoryEntry root = null;
			DirectoryEntry user = null;

			if (string.IsNullOrEmpty(userInfo.Domain))
			{
				// create user
				root = new DirectoryEntry(String.Format("WinNT://{0}", Environment.MachineName));
				user = root.Children.Add(userInfo.Name, "user");
				user.Invoke("SetPassword", new object[] { userInfo.Password });
				user.Properties["FullName"].Add(userInfo.FullName);
				user.Properties["Description"].Add(userInfo.Description);
				user.Properties["UserFlags"].Add(BuildUserFlags(
					userInfo.PasswordCantChange,
					userInfo.PasswordNeverExpires,
					userInfo.AccountDisabled));

				// save account
				user.CommitChanges();
			}
			else
			{
				// root entry
				string rootPath = GetDomainUsersContainer(userInfo.Domain);
				if (string.IsNullOrEmpty(rootPath))
					throw new Exception(string.Format("Users container not found in domain {0}", userInfo.Domain));
				
				root = new DirectoryEntry(rootPath);
				

				// add user
				user = root.Children.Add("CN=" + userInfo.Name, "user");

				SetADObjectProperty(user, "description", userInfo.Description);
				SetADObjectProperty(user, "UserPrincipalName", userInfo.Name);
				SetADObjectProperty(user, "sAMAccountName", userInfo.Name);
				//SetObjectProperty(user, "UserPassword", userInfo.Password);
				user.Properties["userAccountControl"].Value =
					ADAccountOptions.UF_NORMAL_ACCOUNT | ADAccountOptions.UF_PASSWD_NOTREQD;
				user.CommitChanges();

				// set password
				user.Invoke("SetPassword", new object[] { userInfo.Password });

				ADAccountOptions userFlags = ADAccountOptions.UF_NORMAL_ACCOUNT;

				if (userInfo.PasswordCantChange)
					userFlags |= ADAccountOptions.UF_PASSWD_CANT_CHANGE;

				if (userInfo.PasswordNeverExpires)
					userFlags |= ADAccountOptions.UF_DONT_EXPIRE_PASSWD;

				if (userInfo.AccountDisabled)
					userFlags |= ADAccountOptions.UF_ACCOUNTDISABLE;

				user.Properties["userAccountControl"].Value = userFlags;
				user.CommitChanges();
			}
			AddUserToGroups(userInfo.Domain, userInfo.Name, userInfo.MemberOf);
		}
		catch (Exception ex)
		{
			throw new Exception("Can't create user", ex);
		}
	}

	public override void AddUserToGroup(string domain, string userName, string groupName)
	{
		string groupPath = groupName;
		string userPath = userName;
		DirectoryEntry group = null;
		try
		{
			//AD group
			if (groupPath.StartsWith("AD:"))
			{
				//only AD users can be added to the AD group
				if (!string.IsNullOrEmpty(domain))
				{
					groupPath = groupPath.Substring(3);
					group = FindADGroup(groupPath);

					DirectoryEntry deUser = FindADUser(domain, userName);
					userPath = (deUser != null) ? deUser.Path : null;
				}
			}
			//local group
			else
			{
				DirectoryEntry computer = new DirectoryEntry(string.Format("WinNT://{0}", Environment.MachineName));
				// check if this is a SID
				if (groupPath.StartsWith("SID:"))
					groupPath = GetAccountName(groupName.Substring(4));
				group = computer.Children.Find(groupPath, "group");
				
				string netbiosDomain = domain;
				if (!string.IsNullOrEmpty(domain))
				{
					netbiosDomain = GetNETBIOSDomainName(domain);
				}
				userPath = GetUserPath(netbiosDomain, userName);
			}

			if ((group != null) && !string.IsNullOrEmpty(userPath))
			{
				group.Invoke("Add", new object[] { userPath });
				group.CommitChanges();
				group.Close();
			}
		}
		catch (Exception ex)
		{
			Log.WriteError(
				string.Format("Can't add '{0}' user to '{1}' group", userPath, groupPath),
				ex);
		}
	}

	private DirectoryEntry FindADUser(string domain, string userName)
	{
		DirectoryEntry ret = null;
		DirectorySearcher dSearch = new DirectorySearcher();
		dSearch.SearchRoot = GetDomainEntry(domain);
		dSearch.Filter = string.Format("(&(objectClass=user) (cn={0}))", userName);
		SearchResultCollection results = dSearch.FindAll();
		if (results.Count > 0)
		{
			ret = results[0].GetDirectoryEntry();
		}
		return ret;
	}

	private DirectoryEntry FindADGroup(string groupName)
	{
		DirectoryEntry ret = null;
		DirectorySearcher dSearch = new DirectorySearcher();
            dSearch.SearchRoot = Domain.GetComputerDomain().GetDirectoryEntry();
            dSearch.Filter = string.Format("(&(objectClass=group) (cn={0}))", groupName); 
            SearchResultCollection results = dSearch.FindAll();
		if (results.Count > 0)
		{
			ret = results[0].GetDirectoryEntry();
		}
		return ret;
	}
	public override void RemoveUserFromGroup(string domain, string userName, string groupName)
	{
		string groupPath = groupName;
		string userPath = userName;
		DirectoryEntry group = null;
		try
		{
			//AD group
			if (groupPath.StartsWith("AD:"))
			{
				//only AD users can be added to the AD group
				if (!string.IsNullOrEmpty(domain))
				{
					groupPath = groupPath.Substring(3);
					group = FindADGroup(groupPath);

					DirectoryEntry deUser = FindADUser(domain, userName);
					userPath = (deUser != null) ? deUser.Path : null;
				}
			}
			//local group
			else
			{
				DirectoryEntry computer = new DirectoryEntry(string.Format("WinNT://{0}", Environment.MachineName));
				// check if this is a SID
				if (groupPath.StartsWith("SID:"))
					groupPath = GetAccountName(groupName.Substring(4));
				group = computer.Children.Find(groupPath, "group");

				string netbiosDomain = domain;
				if (!string.IsNullOrEmpty(domain))
				{
					netbiosDomain = GetNETBIOSDomainName(domain);
				}
				userPath = GetUserPath(netbiosDomain, userName);
			}

			if ((group != null) && !string.IsNullOrEmpty(userPath))
			{
				group.Invoke("Remove", new object[] { userPath });
				group.CommitChanges();
				group.Close();
			}
		}
		catch (Exception ex)
		{
			Log.WriteError(
				string.Format("Can't remove '{0}' user from '{1}' group", userPath, groupPath),
				ex);
		}
	}

public override void ChangeUserPassword(string userName, string password)
    {
        try
        {
            // get user entry
            DirectoryEntry user = new DirectoryEntry(
                    String.Format("WinNT://{0}/{1},user", Environment.MachineName, userName));

            // change user password
            user.Invoke("SetPassword", new object[] { password });

            // save account
            user.CommitChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Can't change user password", ex);
        }
    }

	/// <summary>
	/// Deletes user
	/// </summary>
	/// <param name="userName">Username</param>
	public override void DeleteUser(string domain, string userName)
	{
		try
		{
			DirectoryEntry user = null;
			if (String.IsNullOrEmpty(domain))
			{
				//local computer
				user = new DirectoryEntry(
					String.Format("WinNT://{0}/{1},user", Environment.MachineName, userName));

				user.Parent.Children.Remove(user);
			}
			else
			{
				//AD
				string path = GetUserDN(domain, userName);
				user = new DirectoryEntry(path);
				user.Parent.Children.Remove(user);
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Can't delete user", ex);
		}
	}
	private int BuildUserFlags(
		bool passwordCantChange,
		bool passwordNeverExpires,
		bool accountDisabled)
	{
		int flags = UF_NORMAL_ACCOUNT | UF_SCRIPT;

		if(passwordCantChange)
			flags |= UF_PASSWD_CANT_CHANGE;

		if(passwordNeverExpires)
			flags |= UF_DONT_EXPIRE_PASSWD;

		if(accountDisabled)
			flags |= UF_ACCOUNTDISABLE;

		return flags;
	}
	#endregion

	#region AD
	public override string GetDomainUsersContainer(string domainName)
	{
		string ret = null;
		if (string.IsNullOrEmpty(domainName))
			return ret;

		StringBuilder sb = new StringBuilder("LDAP://CN=Users,");
		AppendDomainPath(sb, domainName);
		return sb.ToString();
	}

	private void AppendDomainPath(StringBuilder sb, string domain)
	{
		if (string.IsNullOrEmpty(domain))
			return;

		string[] parts = domain.Split('.');
		for (int i = 0; i < parts.Length; i++)
		{
			sb.Append("DC=").Append(parts[i]);

			if (i < (parts.Length - 1))
				sb.Append(",");
		}
	}

	private void AppendOUPath(StringBuilder sb, string ou)
	{
		if (string.IsNullOrEmpty(ou))
			return;

		string path = ou.Replace("/", "\\");
		string[] parts = path.Split('\\');
		for (int i = parts.Length - 1; i != -1; i--)
			sb.Append("OU=").Append(parts[i]).Append(",");
	}

	public override bool ADObjectExists(string objectPath)
	{
		bool found = false;
		if (!string.IsNullOrEmpty(objectPath))
		{
			string path = objectPath;
			if (!path.StartsWith("LDAP://"))
				path = "LDAP://" + path;
			try
			{
				if (DirectoryEntry.Exists(path))
				{
					found = true;
				}
			}
			catch(Exception ex)
			{
				Log.WriteError("AD error", ex);
			}
		}
		return found;
	}

	private void SetADObjectProperty(DirectoryEntry oDE, string PropertyName, string PropertyValue)
	{
		if ((PropertyValue != string.Empty) && (PropertyValue != null))
		{
			if (oDE.Properties.Contains(PropertyName))
			{
				oDE.Properties[PropertyName][0] = PropertyValue;
			}
			else
			{
				oDE.Properties[PropertyName].Add(PropertyValue);
			}
		}
	}

	private DirectoryEntry GetADObject(string path)
	{
		return new DirectoryEntry(path);
	}

	private object GetADObjectProperty(DirectoryEntry entry, string name)
	{
		if (entry.Properties.Contains(name))
			return entry.Properties[name][0];
		else
			return String.Empty;
	}

	private string ConvertByteToStringSid(Byte[] sidBytes)
	{
		StringBuilder strSid = new StringBuilder();
		strSid.Append("S-");
		// Add SID revision.
		strSid.Append(sidBytes[0].ToString());
		// Next six bytes are SID authority value.
		if (sidBytes[6] != 0 || sidBytes[5] != 0)
		{
			string strAuth = String.Format
				("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
				(Int16)sidBytes[1],
				(Int16)sidBytes[2],
				(Int16)sidBytes[3],
				(Int16)sidBytes[4],
				(Int16)sidBytes[5],
				(Int16)sidBytes[6]);
			strSid.Append("-");
			strSid.Append(strAuth);
		}
		else
		{
			Int64 iVal = (Int32)(sidBytes[1]) +
				(Int32)(sidBytes[2] << 8) +
				(Int32)(sidBytes[3] << 16) +
				(Int32)(sidBytes[4] << 24);
			strSid.Append("-");
			strSid.Append(iVal.ToString());
		}

		// Get sub authority count...
		int iSubCount = Convert.ToInt32(sidBytes[7]);
		int idxAuth = 0;
		for (int i = 0; i < iSubCount; i++)
		{
			idxAuth = 8 + i * 4;
			UInt32 iSubAuth = BitConverter.ToUInt32(sidBytes, idxAuth);
			strSid.Append("-");
			strSid.Append(iSubAuth.ToString());
		}
		return strSid.ToString();
	}

	public override string GetNETBIOSDomainName(string domain)
	{
		Log.WriteStart("GetNETBIOSDomainName");
		string ret = string.Empty;

		string path = string.Format("LDAP://{0}/RootDSE", domain);
		DirectoryEntry rootDSE = GetADObject(path);
		string contextPath = GetADObjectProperty(rootDSE, "ConfigurationNamingContext").ToString();
		string defaultContext = GetADObjectProperty(rootDSE, "defaultNamingContext").ToString();
		DirectoryEntry partitions = GetADObject("LDAP://cn=Partitions," + contextPath);

		DirectorySearcher searcher = new DirectorySearcher();
		searcher.SearchRoot = partitions;
		searcher.Filter = string.Format("(&(objectCategory=crossRef)(nCName={0}))", defaultContext);
		searcher.SearchScope = SearchScope.OneLevel;

		//find the first instance
		SearchResult result = searcher.FindOne();
		if (result != null)
		{
			DirectoryEntry partition = GetADObject(result.Path);
			ret = GetADObjectProperty(partition, "nETBIOSName").ToString();
			partition.Close();
		}
		partitions.Close();
		rootDSE.Close();
		Log.WriteEnd("GetNETBIOSDomainName");
		return ret;
	}

	#endregion

    #region Windows Services

    public override void DeleteService(string serviceName)
    {
        var wmiService = wmi.GetObject(String.Format("Win32_Service.Name='{0}'", serviceName));

        wmiService.Delete();
    }

    public override string GetServicePath(string serviceName)
    {
            var mc = new ManagementClass("Win32_Service");

            foreach (var mo in mc.GetInstances())
            {
                if (mo.GetPropertyValue("Name").ToString() == serviceName)
                {
                    var path = mo.GetPropertyValue("PathName").ToString().Trim('"');

                    var directory = Path.GetDirectoryName(path);

                    return directory;
                }
            }

        return string.Empty;
    }

    #endregion
}

#region Enums
[Flags]
internal enum ADAccountOptions
{
	UF_TEMP_DUPLICATE_ACCOUNT = 256,
	UF_NORMAL_ACCOUNT = 512,
	UF_INTERDOMAIN_TRUST_ACCOUNT = 2048,
	UF_WORKSTATION_TRUST_ACCOUNT = 4096,
	UF_SERVER_TRUST_ACCOUNT = 8192,
	UF_DONT_EXPIRE_PASSWD = 65536,
	UF_SCRIPT = 1,
	UF_ACCOUNTDISABLE = 2,
	UF_HOMEDIR_REQUIRED = 8,
	UF_LOCKOUT = 16,
	UF_PASSWD_NOTREQD = 32,
	UF_PASSWD_CANT_CHANGE = 64,
	UF_ACCOUNT_LOCKOUT = 16,
	UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 128
}
#endregion

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

namespace SolidCP.UniversalInstaller;

/// <summary>
/// Security utils class.
/// </summary>
public abstract class SecurityUtils
{
	#region NTFS permissions
	/// <summary>
	/// Grants NTFS permissions by username
	/// </summary>
	/// <param name="path"></param>
	/// <param name="accountName"></param>
	/// <param name="permissions"></param>
	/// <param name="inheritParentPermissions"></param>
	/// <param name="preserveOriginalPermissions"></param>
	public abstract void GrantNtfsPermissions(string path, string domain, string accountName,
		NtfsPermission permissions, bool inheritParentPermissions, bool preserveOriginalPermissions);
	/// <summary>
	/// Grants NTFS permissions by SID
	/// </summary>
	/// <param name="path"></param>
	/// <param name="sid"></param>
	/// <param name="permissions"></param>
	/// <param name="inheritParentPermissions"></param>
	/// <param name="preserveOriginalPermissions"></param>
	public abstract void GrantNtfsPermissionsBySid(string path, string sid,
		NtfsPermission permissions, bool inheritParentPermissions, bool preserveOriginalPermissions);

	/// <summary>
	/// Removes NTFS permissions by username
	/// </summary>
	/// <param name="path"></param>
	/// <param name="accountName"></param>
	public abstract void RemoveNtfsPermissions(string path, string accountName);

	/// <summary>
	/// Removes NTFS permissions by SID
	/// </summary>
	/// <param name="path"></param>
	/// <param name="sid"></param>
	public abstract void RemoveNtfsPermissionsBySid(string path, string sid);

	/// <summary>
	/// Resets NTFS permissions by username
	/// </summary>
	/// <param name="path"></param>
	public abstract void ResetNtfsPermissions(string path);

	/// <summary>
	/// Returns SID by account name
	/// </summary>
	/// <param name="userAccount"></param>
	/// <returns></returns>
	public abstract string GetSid(string userAccount);

	/// <summary>
	/// Returns SID by account name and domain
	/// </summary>
	/// <param name="userAccount"></param>
	/// <param name="domain"></param>
	/// <returns></returns>
	public abstract string GetSid(string userAccount, string domain);

	/// <summary>
	/// Returns account name by SID
	/// </summary>
	/// <param name="sid"></param>
	/// <returns></returns>
	public abstract string GetAccountName(string sid);
	#endregion

	#region User and Group methods 

	/// <summary>
	/// Check for existing user
	/// </summary>
	public abstract bool UserExists(string domain, string userName);

	public abstract string GetFullDomainName(string friendlyDomainName);
	public abstract DirectoryEntry GetDomainEntry(string domainName);

	/// <summary>
	/// Creates user
	/// </summary>
	/// <param name="userInfo">User</param>
	public abstract void CreateUser(SystemUserItem userInfo);
	public virtual void AddUserToGroups(string domain, string user, IEnumerable<string> groups)
	{
		try
		{
			if (groups != null)
			{
				foreach (string group in groups)
				{
					AddUserToGroup(domain, user, group);
				}
			}
		}
		catch (Exception ex)
		{
			Log.WriteError("Can't add user to groups", ex);
		}
	}
	public abstract void AddUserToGroup(string domain, string userName, string groupName);
	//private DirectoryEntry FindADUser(string domain, string userName)
	//private DirectoryEntry FindADGroup(string groupName)

	public virtual void RemoveUserFromGroups(string domain, string user, string[] groups)
	{
		try
		{
			if (groups != null)
			{
				foreach (string group in groups)
				{
					RemoveUserFromGroup(domain, user, group);
				}
			}
		}
		catch (Exception ex)
		{
			Log.WriteError("Can't remove user from groups", ex);
		}
	}

	public abstract void RemoveUserFromGroup(string domain, string userName, string groupName);
	public abstract void ChangeUserPassword(string userName, string password);

	/// <summary>
	/// Deletes user
	/// </summary>
	/// <param name="userName">Username</param>
	public abstract void DeleteUser(string domain, string userName);
	/*private int BuildUserFlags(
		bool passwordCantChange,
		bool passwordNeverExpires,
		bool accountDisabled)*/
	#endregion

	#region AD
	public abstract string GetDomainUsersContainer(string domainName);
	//private void AppendDomainPath(StringBuilder sb, string domain)
	//private void AppendOUPath(StringBuilder sb, string ou)

	public abstract bool ADObjectExists(string objectPath);
	//private void SetADObjectProperty(DirectoryEntry oDE, string PropertyName, string PropertyValue)
	//private DirectoryEntry GetADObject(string path)
	//private object GetADObjectProperty(DirectoryEntry entry, string name)
	//private string ConvertByteToStringSid(Byte[] sidBytes)
	public abstract string GetNETBIOSDomainName(string domain);
	#endregion

	#region Windows Services

	public abstract void DeleteService(string serviceName);
	public abstract string GetServicePath(string serviceName);

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

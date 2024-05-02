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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

#if NET8_0
using Microsoft.Data.SqlClient;
#else
using System.Data.SqlClient;
#endif

#if !NETFRAMEWORK && !NETSTANDARD
using Microsoft.EntityFrameworkCore;
#elif NETFRAMEWORK
using System.Data.Entity;
#endif
using System.Text.RegularExpressions;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.HostedSolution;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
using Microsoft.Win32;
using SolidCP.Providers.OS;
using SolidCP.Providers.RemoteDesktopServices;
using SolidCP.Providers.DNS;
using SolidCP.Providers.DomainLookup;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for DataProvider.
	/// </summary>
	public class DataProvider : Data.DbContext
	{
#if UseEntityFramework
		public readonly bool UseEntityFramework = true;
#else
		public bool UseEntityFramework => !IsMsSql || !HasProcedures;
#endif
		ControllerBase Provider;
		ServerController serverController;
		protected ServerController ServerController => serverController ?? (serverController = new ServerController(Provider));

		public DataProvider() : this(null) { }
		public DataProvider(ControllerBase provider) { Provider = provider; }

		//public string ConnectionString => ConfigSettings.ConnectionString;
		private string ObjectQualifier
		{
			get
			{
				return "";
			}
		}

		#region System Settings

		public IDataReader GetSystemSettings(string settingsName)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSystemSettings",
				 new SqlParameter("@SettingsName", settingsName)
			);
		}

		public IQueryable<Data.Entities.SystemSetting> GetSystemSettingsEF(string settingsName)
		{
			return SystemSettings.Where(s => s.SettingsName == settingsName);
		}

		public void SetSystemSettings(string settingsName, string xml)
		{
			if (UseEntityFramework)
			{
				var properties = XElement.Parse(xml);
				foreach (var property in properties.Descendants())
				{
					var setting = new Data.Entities.SystemSetting()
					{
						SettingsName = settingsName,
						PropertyName = (string)property.Attribute("name"),
						PropertyValue = (string)property.Attribute("value")
					};
					SystemSettings.Add(setting);
				}
			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					 ConnectionString,
					 CommandType.StoredProcedure,
					 "SetSystemSettings",
					 new SqlParameter("@SettingsName", settingsName),
					 new SqlParameter("@Xml", xml)
				);
			}
		}

		#endregion

		#region Theme Settings

		public DataSet GetThemes()
		{
			if (UseEntityFramework)
			{
				return ObjectUtils.DataSetFromEntitySet(
					Themes
						.Where(t => t.Enabled == 1)
						.OrderBy(t => t.DisplayOrder));
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetThemes");
			}
		}

		public DataSet GetThemeSettings(int ThemeID)
		{
			if (UseEntityFramework)
			{
				return ObjectUtils.DataSetFromEntitySet(
					ThemeSettings
						.Where(ts => ts.ThemeId == ThemeID));
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetThemeSettings",
					 new SqlParameter("@ThemeID", ThemeID));
			}
		}

		public DataSet GetThemeSetting(int ThemeID, string SettingsName)
		{
			if (UseEntityFramework)
			{
				return ObjectUtils.DataSetFromEntitySet(
					ThemeSettings
						.Where(ts => ts.ThemeId == ThemeID && ts.SettingsName == SettingsName));
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetThemeSetting",
					 new SqlParameter("@ThemeID", ThemeID),
					 new SqlParameter("@SettingsName", SettingsName));
			}
		}

		public bool CheckActorUserRights(int actorId, int userId)
		{
			if (actorId == -1 || userId == 0 ||
				// check if the user requests himself
				actorId == userId)
				return true;


			// check if the user requests his owner
			var actor = Users
				.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
				.FirstOrDefault(u => u.UserId == actorId);

			if (actor != null && actor.IsPeer) actorId = actor.OwnerId ?? -1;

			if (actorId == userId) return true;

			var id = userId;
			do // check owners chain
			{
				var user = Users
					.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
					.FirstOrDefault(u => u.UserId == id);
				if (user == null || user.OwnerId == null) return false;
				else
				{
					id = user.OwnerId.Value;
					if (id == actorId) return true;
				}
			} while (true);
		}

		public DataSet GetUserThemeSettings(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				const string SettingsName = "Theme";
				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				var id = userId;
				var setting = UserSettings.Where(s => s.UserId == id && s.SettingsName == SettingsName);
				while (!setting.Any())
				{
					var user = Users.FirstOrDefault(u => u.UserId == id);
					if (user != null && user.OwnerId != null)
					{
						id = user.OwnerId.Value;
						setting = UserSettings.Where(s => s.UserId == id && s.SettingsName == SettingsName);
					}
					else break;
				}
				return ObjectUtils.DataSetFromEntitySet(setting);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserSettings",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@SettingsName", "Theme"));
			}
		}

		public void UpdateUserThemeSetting(int actorId, int userId, string PropertyName, string PropertyValue)
		{
			if (UseEntityFramework)
			{
				const string SettingsName = "Theme";

				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				var setting = UserSettings.FirstOrDefault(s => s.UserId == userId &&
					s.SettingsName == SettingsName &&
					s.PropertyName == PropertyName);
				if (setting != null)
				{
					setting.PropertyValue = PropertyValue;
				} else
				{
					setting = new Data.Entities.UserSetting()
					{
						UserId = userId,
						SettingsName = SettingsName,
						PropertyName = PropertyName,
						PropertyValue = PropertyValue
					};
					UserSettings.Add(setting);
				}
				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateUserThemeSetting",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@PropertyName", PropertyName),
					 new SqlParameter("@PropertyValue", PropertyValue));
			}
		}

		public void DeleteUserThemeSetting(int actorId, int userId, string PropertyName)
		{
			if (UseEntityFramework)
			{
				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				UserSettings.RemoveRange(UserSettings.Where(s => s.UserId == userId &&
					s.SettingsName == "Theme" && s.PropertyName == PropertyName));

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "DeleteUserThemeSetting",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@PropertyName", PropertyName));
			}
		}

		#endregion

		#region Users
		public bool CheckUserExists(string username)
		{
			if (UseEntityFramework)
			{
				return Users.Any(u => u.Username == username);
			}
			else
			{
				SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
				prmExists.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "CheckUserExists",
					 prmExists,
					 new SqlParameter("@username", username));

				return Convert.ToBoolean(prmExists.Value);
			}
		}

		List<int> UserParents(int actorId, int userId)
		{
			var list = new List<int>();
			var user = Users
				.Select(u => new { u.UserId, u.OwnerId })
				.FirstOrDefault(u => u.UserId == userId);
			while (user != null)
			{
				list.Add(userId);
				if (user.OwnerId.HasValue)
				{
					userId = user.OwnerId.Value;
					user = Users
						.Select(u => new { u.UserId, u.OwnerId })
						.FirstOrDefault(u => u.UserId == userId);
				}
				else break;
			}
			return list;
		}

		bool CheckUserParent(int ownerId, int userId)
		{
			if (ownerId == userId) return true;

			var owner = Users
				.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
				.FirstOrDefault(u => u.UserId == ownerId);
			if (owner != null && owner.IsPeer && owner.OwnerId.HasValue) ownerId = owner.OwnerId.Value;

			if (ownerId == userId) return true;

			var id = userId;
			var user = Users
				.Select(u => new { u.UserId, u.OwnerId })
				.FirstOrDefault(u => u.UserId == id);
			while (user != null && user.OwnerId.HasValue && user.OwnerId.Value != ownerId)
			{
				id = user.OwnerId.Value;
				user = Users
					.Select(u => new { u.UserId, u.OwnerId })
					.FirstOrDefault(u => u.UserId == id);
			}

			return user != null && user.OwnerId.HasValue && user.OwnerId.Value == ownerId;
		}

		string GetItemComments(int itemId, string itemTypeId, int actorId)
		{
			var comments = Comments.Join(Users, c => c.UserId, u => u.UserId, (com, user) => new
				{
					com.UserId,
					com.ItemId,
					com.ItemTypeId,
					com.CreatedDate,
					com.CommentText,
					user.Username
				})
				.Where(c => c.ItemId == itemId && c.ItemTypeId == itemTypeId &&
					CheckUserParent(actorId, c.UserId));

			var sb = new StringBuilder();
			foreach (var comment in comments)
			{
				sb.Append(comment.Username);
				sb.Append(" - ");
				sb.AppendLine(comment.CreatedDate.ToShortDateString());
				sb.AppendLine(comment.CommentText);
				sb.AppendLine("--------------------------------------");
			}
			return sb.ToString();
		}

		public DataSet GetUsersPaged(int actorId, int userId, string filterColumn, string filterValue,
			 int statusId, int roleId, string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			if (UseEntityFramework)
			{
				var hasRights = CheckActorUserRights(actorId, userId);
				var users = hasRights ?
					UsersDetailed
						.Where(u => u.UserId != userId && !u.IsPeer &&
						(recursive ? CheckUserParent(userId, u.UserId) : u.OwnerId == userId) &&
						(statusId == 0 || statusId > 0 && statusId == u.StatusId) &&
						(roleId == 0 || roleId > 0 && roleId == u.RoleId)) :
					UsersDetailed.Where(u => false);
				
				if (!string.IsNullOrEmpty(filterValue))
				{
					if (!string.IsNullOrEmpty(filterColumn))
					{
						users = users.Where($"{filterColumn} == @1", filterValue);
					} else
					{
						users = users.Where(u => u.Username == filterValue ||
							u.FullName == filterValue ||
							u.Email == filterValue);
					}
				}

				if (!string.IsNullOrEmpty(sortColumn))
				{
					users = users.OrderBy(sortColumn);
				}

				users = users.Skip(startRow).Take(maximumRows);

				return ObjectUtils.DataSetFromEntitySet(users.Select(u => new
				{
					UserID = u.UserId,
					RoleID = u.RoleId,
					StatusID = u.StatusId,
					SubscriberNumber = u.SubscriberNumber,
					LoginStatusId = u.LoginStatusId,
					FailedLogins = u.FailedLogins,
					OwnerID = u.OwnerId,
					Created = u.Created,
					Changed = u.Changed,
					IsDemo = u.IsDemo,
					Comments = GetItemComments(u.UserId, "USER", actorId),
					IsPeer = u.IsPeer,
					Username = u.Username,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Email = u.Email,
					FullName = u.FullName,
					OwnerUsername = u.OwnerUsername,
					OwnerFirstName = u.OwnerFirstName,
					OwnerLastName = u.OwnerLastName,
					OwnerRoleID = u.OwnerRoleId,
					OwnerFullName = u.OwnerFullName,
					OwnerEmail = u.OwnerEmail,
					PackagesNumber = u.PackagesNumber,
					CompanyName = u.CompanyName,
					EcommerceEnabled = u.EcommerceEnabled
				}));
				
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUsersPaged",
					 new SqlParameter("@actorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
					 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
					 new SqlParameter("@statusId", statusId),
					 new SqlParameter("@roleId", roleId),
					 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
					 new SqlParameter("@startRow", startRow),
					 new SqlParameter("@maximumRows", maximumRows),
					 new SqlParameter("@recursive", recursive));
			}
		}

		//TODO START
		public DataSet GetSearchObject(int actorId, int userId, string filterColumn, string filterValue,
			int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType,
			bool recursive, bool onlyFind)
		{
			if (UseEntityFramework)
			{
				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				if (colType == null) colType = "";

				if (string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue)) filterColumn = "TextSearch";

				//TODO not yet implemented

				throw new NotImplementedException();

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetSearchObject",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
					 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
					 new SqlParameter("@StatusId", statusId),
					 new SqlParameter("@RoleId", roleId),
					 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
					 new SqlParameter("@StartRow", startRow),
					 new SqlParameter("@MaximumRows", maximumRows),
					 new SqlParameter("@Recursive", recursive),
					 new SqlParameter("@ColType", colType),
					 new SqlParameter("@FullType", fullType),
					 new SqlParameter("@OnlyFind", onlyFind));
			}
		}

		public DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows,
			 bool Recursive, int PoolID, int ServerID, int ActorID, int StatusID, int PlanID, int OrgID,
			 string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID,
			 string FilterColumns)
		{
			if (UseEntityFramework)
			{
				throw new NotImplementedException();
			}
			else
			{

				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetSearchTableByColumns",
					 new SqlParameter("@PagedStored", PagedStored),
					 new SqlParameter("@FilterValue", FilterValue),
					 new SqlParameter("@MaximumRows", MaximumRows),
					 new SqlParameter("@Recursive", Recursive),
					 new SqlParameter("@PoolID", PoolID),
					 new SqlParameter("@ServerID", ServerID),
					 new SqlParameter("@ActorID", ActorID),
					 new SqlParameter("@StatusID", StatusID),
					 new SqlParameter("@PlanID", PlanID),
					 new SqlParameter("@OrgID", OrgID),
					 new SqlParameter("@ItemTypeName", ItemTypeName),
					 new SqlParameter("@GroupName", GroupName),
					 new SqlParameter("@PackageID", PackageID),
					 new SqlParameter("@VPSType", VPSType),
					 new SqlParameter("@RoleID", RoleID),
					 new SqlParameter("@UserID", UserID),
					 new SqlParameter("@FilterColumns", FilterColumns));
			}
		}

		//TODO END
		public DataSet GetUsersSummary(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				var users = Users.Where(u => u.OwnerId == userId && !u.IsPeer);
				var nofUsers = new { UsersNumber = users.Count() };
				var usersByStatus = users
					.GroupBy(u => u.StatusId)
					.Select(g => new { StatusId = g.Key, UsersNumber = g.Count() })
					.OrderBy(g => g.StatusId);
				var usersByRole = users
					.GroupBy(u => u.RoleId)
					.Select(g => new { RoleId = g.Key, UsersNumber = g.Count() })
					.OrderByDescending(g => g.RoleId);
				
				var set = new DataSet();
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(new object[] { nofUsers }));
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(usersByStatus));
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(usersByRole));
				
				return set;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUsersSummary",
					 new SqlParameter("@actorId", actorId),
					 new SqlParameter("@UserID", userId));
			}
		}

		int[] UsersTree(int ownerId, bool recursive)
		{
			if (!recursive) return new int[] { ownerId };
			else
			{
				var list = new HashSet<int>();
				list.Add(ownerId);

				var newDescendants = Users
					.Select(u => new { u.UserId, u.OwnerId })
					.Where(u => list.Contains(u.OwnerId ?? -1) && !list.Contains(u.UserId))
					.ToArray();
				while (newDescendants.Any())
				{
					foreach (var d in newDescendants) list.Add(d.UserId);
					newDescendants = Users
						.Select(u => new { u.UserId, u.OwnerId })
						.Where(u => list.Contains(u.OwnerId ?? -1) && !list.Contains(u.UserId))
						.ToArray();
				}
				return list.ToArray();
			}
		}
		public DataSet GetUserDomainsPaged(int actorId, int userId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				var hasRights = CheckActorUserRights(actorId, userId);

				var users = Users
					.Join(UsersTree(userId, true), u => u.UserId, ut => ut, (user, ut) => user)
					.Join(Packages, u => u.UserId, p => p.UserId, (user, package) => new { User = user, PackageId = package.PackageId })
					.Join(Domains, up => up.PackageId, d => d.PackageId, (user, domain) => new
					{
						UserID = user.User.UserId,
						RoleID = user.User.RoleId,
						StatusID = user.User.StatusId,
						user.User.SubscriberNumber,
						user.User.LoginStatusId,
						user.User.FailedLogins,
						OwnerID = user.User.OwnerId,
						user.User.Created,
						user.User.Changed,
						user.User.IsDemo,
						user.User.Comments,
						user.User.IsPeer,
						user.User.Username,
						user.User.FirstName,
						user.User.LastName,
						user.User.Email,
						domain.DomainName
					})
					.Where(u => u.UserID != userId && !u.IsPeer && hasRights);

				if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
				{
					users = users.Where($"{filterColumn} == @0", filterValue);
				}

				if (!string.IsNullOrEmpty(sortColumn))
				{
					users = users.OrderBy(sortColumn);
				}

				return ObjectUtils.DataSetFromEntitySet(users);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserDomainsPaged",
					 new SqlParameter("@actorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@filterColumn", VerifyColumnName(filterColumn)),
					 new SqlParameter("@filterValue", VerifyColumnValue(filterValue)),
					 new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
					 new SqlParameter("@startRow", startRow),
					 new SqlParameter("@maximumRows", maximumRows));
			}
		}

		bool CanGetUserDetails(int actorId, int userId)
		{
			if (actorId == -1 || actorId == userId) return true;

			var actor = Users
				.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
				.FirstOrDefault(u => u.UserId == actorId);
			if (actor != null && actor.IsPeer && actor.OwnerId.HasValue)
			{
				actorId = actor.OwnerId.Value;
				actor = Users
					.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
					.FirstOrDefault(u => u.UserId == actorId);
			}

			if (actor != null && actor.OwnerId.HasValue && userId == actor.OwnerId.Value) return true;

			var id = userId;
			var user = Users
				.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
				.FirstOrDefault(u => u.UserId == id);
			while (user != null && user.OwnerId.HasValue && user.OwnerId != actorId)
			{
				id = user.OwnerId.Value;
				user = Users
					.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
					.FirstOrDefault(u => u.UserId == id);
			}

			return user != null && user.OwnerId.HasValue && user.OwnerId == actorId;
		}
		public DataSet GetUsers(int actorId, int ownerId, bool recursive)
		{
			if (UseEntityFramework)
			{
				var canGetDetails = CanGetUserDetails(actorId, ownerId);

				var users = UsersDetailed
					.Where(u => canGetDetails && u.UserId != ownerId && !u.IsPeer &&
						(recursive ? CheckUserParent(ownerId, u.UserId) : u.OwnerId == ownerId));

				return ObjectUtils.DataSetFromEntitySet(users);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUsers",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@OwnerID", ownerId),
					 new SqlParameter("@Recursive", recursive));
			}
		}

		public DataSet GetUserParents(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				int n = 0;
				var parents = UserParents(actorId, userId)
					.Select(u => new { UserId = u, Order = n++ })
					.ToArray();
				var users = Users
					.Join(parents, u => u.UserId, p => p.UserId, (user, parent) => new { User = user, Order = parent.Order })
					.OrderByDescending(u => u.Order)
					.Select(u => u.User);

				return ObjectUtils.DataSetFromEntitySet(users);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserParents",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId));
			}
		}

		public DataSet GetUserPeers(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				var canGetDetails = CanGetUserDetails(actorId, userId);

				var userPeers = UsersDetailed
					.Where(u => canGetDetails && u.OwnerId == userId && u.IsPeer);

				return ObjectUtils.DataSetFromEntitySet(userPeers);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserPeers",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@userId", userId));
			}
		}

		public IDataReader GetUserByExchangeOrganizationIdInternally(int itemId)
		{
			if (UseEntityFramework)
			{
				var users = Users
					.Join(Packages, u => u.UserId, p => p.UserId, (user, package) => new { User = user, package.PackageId })
					.Join(ServiceItems, u => u.PackageId, s => s.PackageId, (user, serviceItem) => new { user.User, serviceItem.ItemId })
					.Where(u => u.ItemId == itemId)
					.Select(u => u.User);

				return new EntityDataReader<Data.Entities.User>(users);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserByExchangeOrganizationIdInternally",
					 new SqlParameter("@ItemID", itemId));
			}
		}



		public IDataReader GetUserByIdInternally(int userId)
		{
			if (UseEntityFramework)
			{
				return new EntityDataReader<Data.Entities.User>(Users.Where(u => u.UserId == userId));
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserByIdInternally",
					 new SqlParameter("@UserID", userId));
			}
		}

		public IDataReader GetUserByUsernameInternally(string username)
		{
			if (UseEntityFramework)
			{
				return new EntityDataReader<Data.Entities.User>(Users.Where(u => u.Username == username));
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserByUsernameInternally",
					 new SqlParameter("@Username", username));
			}
		}

		bool CanGetUserPassword(int actorId, int userId)
		{
			if (actorId == -1 || actorId == userId) return true;

			var actor = Users
				.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
				.FirstOrDefault(u => u.UserId == actorId);
			if (actor != null && actor.IsPeer)
			{
				// peer can't get the password of his peers and his owner
				if (actor.OwnerId == userId ||
					Users.Any(u => u.IsPeer && u.OwnerId == actor.OwnerId && u.UserId == userId)) return false;

				// set actor to his owner
				actorId = actor.OwnerId ?? -1;
			}

			// get users owner
			var owner = Users
				.Select(u => new { u.UserId, u.OwnerId })
				.FirstOrDefault(u => u.UserId == actorId);
			if (owner != null && owner.OwnerId == userId) return false; // user can't get the password of his owner

			var id = userId;
			var user = Users
				.Select(u => new { u.UserId, u.OwnerId })
				.FirstOrDefault(u => u.UserId == id);
			while (user != null && user.OwnerId.HasValue && user.OwnerId != actorId)
			{
				id = user.OwnerId.Value;
				user = Users
					.Select(u => new { u.UserId, u.OwnerId })
					.FirstOrDefault(u => u.UserId == id);
			}
			return user != null && user.OwnerId == actorId; // actor is owner of user
		}
		public IDataReader GetUserById(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				var canGetUserDetails = CanGetUserDetails(actorId, userId);
				var canGetUserPassword = CanGetUserPassword(actorId, userId);
				var user = Users
					.Where(u => u.UserId == userId && canGetUserDetails)
					.Select(u => new Data.Entities.User() {
						UserId = u.UserId,
						RoleId = u.RoleId,
						StatusId = u.StatusId,
						SubscriberNumber = u.SubscriberNumber,
						LoginStatusId = u.LoginStatusId,
						FailedLogins = u.FailedLogins,
						OwnerId = u.OwnerId,
						Created = u.Created,
						Changed = u.Changed,
						IsDemo = u.IsDemo,
						Comments = u.Comments,
						IsPeer = u.IsPeer,
						Username = u.Username,
						Password = canGetUserPassword ? u.Password : "",
						FirstName = u.FirstName,
						LastName = u.LastName,
						Email = u.Email,
						SecondaryEmail = u.SecondaryEmail,
						Address = u.Address,
						City = u.City,
						State = u.State,
						Country = u.Country,
						Zip = u.Zip,
						PrimaryPhone = u.PrimaryPhone,
						SecondaryPhone = u.SecondaryPhone,
						Fax = u.Fax,
						InstantMessenger = u.InstantMessenger,
						HtmlMail = u.HtmlMail,
						CompanyName = u.CompanyName,
						EcommerceEnabled = u.EcommerceEnabled,
						AdditionalParams = u.AdditionalParams,
						MfaMode = u.MfaMode,
						PinSecret = canGetUserPassword ? u.PinSecret : ""
					});

				return new EntityDataReader<Data.Entities.User>(user);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserById",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId));
			}
		}

		public IDataReader GetUserByUsername(int actorId, string username)
		{
			if (UseEntityFramework)
			{
				var user = Users
					.Where(u => u.Username == username)
					.Select(u => new Data.Entities.User()
					{
						UserId = u.UserId,
						RoleId = u.RoleId,
						StatusId = u.StatusId,
						SubscriberNumber = u.SubscriberNumber,
						LoginStatusId = u.LoginStatusId,
						FailedLogins = u.FailedLogins,
						OwnerId = u.OwnerId,
						Created = u.Created,
						Changed = u.Changed,
						IsDemo = u.IsDemo,
						Comments = u.Comments,
						IsPeer = u.IsPeer,
						Username = u.Username,
						Password = CanGetUserPassword(actorId, u.UserId) ? u.Password : "",
						FirstName = u.FirstName,
						LastName = u.LastName,
						Email = u.Email,
						SecondaryEmail = u.SecondaryEmail,
						Address = u.Address,
						City = u.City,
						State = u.State,
						Country = u.Country,
						Zip = u.Zip,
						PrimaryPhone = u.PrimaryPhone,
						SecondaryPhone = u.SecondaryPhone,
						Fax = u.Fax,
						InstantMessenger = u.InstantMessenger,
						HtmlMail = u.HtmlMail,
						CompanyName = u.CompanyName,
						EcommerceEnabled = u.EcommerceEnabled,
						AdditionalParams = u.AdditionalParams,
						MfaMode = u.MfaMode,
						PinSecret = CanGetUserPassword(actorId, u.UserId) ? u.PinSecret : ""
					})
					.Where(u => CanGetUserDetails(actorId, u.UserId));

				return new EntityDataReader<Data.Entities.User>(user);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserByUsername",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@Username", username));
			}
		}

		bool CanCreateUser(int actorId, int ownerId)
		{
			if (actorId == -1 || actorId == ownerId) return true;

			var actor = Users
				.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
				.FirstOrDefault(u => u.UserId == actorId);
			if (actor != null && actor.IsPeer && actor.OwnerId.HasValue) actorId = actor.OwnerId.Value;
			if (actorId == ownerId) return true;

			var id = ownerId;
			var user = Users
				.Select(u => new { u.UserId, u.OwnerId })
				.FirstOrDefault(u => u.UserId == id);
			while (user != null && user.OwnerId.HasValue && user.OwnerId != actorId)
			{
				id = user.OwnerId.Value;
				user = Users
					.Select(u => new { u.UserId, u.OwnerId })
					.FirstOrDefault(u => u.UserId == id);
			}
			return user != null && user.OwnerId == actorId;
		}

		public int AddUser(int actorId, int ownerId, int roleId, int statusId, string subscriberNumber, int loginStatusId, bool isDemo,
			 bool isPeer, string comments, string username, string password,
			 string firstName, string lastName, string email, string secondaryEmail,
			 string address, string city, string country, string state, string zip,
			 string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail,
			 string companyName, bool ecommerceEnabled)
		{
			if (UseEntityFramework)
			{
				if (Users.Any(u => u.Username == username)) return -1;
				if (!CanCreateUser(actorId, ownerId)) return -2;
				var user = new Data.Entities.User()
				{
					OwnerId = ownerId,
					RoleId = roleId,
					StatusId = statusId,
					SubscriberNumber = subscriberNumber,
					LoginStatusId = loginStatusId,
					IsDemo = isDemo,
					IsPeer = isPeer,
					Comments = comments,
					Username = username,
					Password = password,
					FirstName = firstName,
					LastName = lastName,
					Email = email,
					SecondaryEmail = secondaryEmail,
					Address = address,
					City = city,
					Country = country,
					State = state,
					Zip = zip,
					PrimaryPhone = primaryPhone,
					SecondaryPhone = secondaryPhone,
					Fax = fax,
					InstantMessenger = instantMessenger,
					HtmlMail = htmlMail,
					CompanyName = companyName,
					EcommerceEnabled = ecommerceEnabled
				};
				Users.Add(user);
				SaveChanges();

				return user.UserId;
			}
			else
			{
				SqlParameter prmUserId = new SqlParameter("@UserID", SqlDbType.Int);
				prmUserId.Direction = ParameterDirection.Output;

				// add user to SolidCP Users table
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "AddUser",
					 prmUserId,
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@OwnerID", ownerId),
					 new SqlParameter("@RoleID", roleId),
					 new SqlParameter("@StatusId", statusId),
					 new SqlParameter("@SubscriberNumber", subscriberNumber),
					 new SqlParameter("@LoginStatusId", loginStatusId),
					 new SqlParameter("@IsDemo", isDemo),
					 new SqlParameter("@IsPeer", isPeer),
					 new SqlParameter("@Comments", comments),
					 new SqlParameter("@username", username),
					 new SqlParameter("@password", password),
					 new SqlParameter("@firstName", firstName),
					 new SqlParameter("@lastName", lastName),
					 new SqlParameter("@email", email),
					 new SqlParameter("@secondaryEmail", secondaryEmail),
					 new SqlParameter("@address", address),
					 new SqlParameter("@city", city),
					 new SqlParameter("@country", country),
					 new SqlParameter("@state", state),
					 new SqlParameter("@zip", zip),
					 new SqlParameter("@primaryPhone", primaryPhone),
					 new SqlParameter("@secondaryPhone", secondaryPhone),
					 new SqlParameter("@fax", fax),
					 new SqlParameter("@instantMessenger", instantMessenger),
					 new SqlParameter("@htmlMail", htmlMail),
					 new SqlParameter("@CompanyName", companyName),
					 new SqlParameter("@EcommerceEnabled", ecommerceEnabled));

				return Convert.ToInt32(prmUserId.Value);
			}
		}

		bool CanUpdateUserDetails(int actorId, int userId)
		{
			if (actorId == -1 || actorId == userId) return true;

			var actor = Users
				.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
				.FirstOrDefault(u => u.UserId == actorId);
			if (actor != null && actor.IsPeer && actor.OwnerId.HasValue)
			{
				// check if the peer is trying to update his owner
				if (actor.OwnerId == userId) return false;

				// check if the peer is trying to update his peers
				if (Users.Any(u => u.IsPeer && u.OwnerId == actor.OwnerId && u.UserId == userId)) return false;

				actorId = actor.OwnerId.Value;
			}

			var id = userId;
			var user = Users
				.Select(u => new { u.UserId, u.OwnerId })
				.FirstOrDefault(u => u.UserId == id);
			while (user != null && user.OwnerId.HasValue && user.OwnerId != actorId)
			{
				id = user.OwnerId.Value;
				user = Users
					.Select(u => new { u.UserId, u.OwnerId })
					.FirstOrDefault(u => u.UserId == id);
			}
			return user != null && user.OwnerId == actorId;
		}

		public void UpdateUser(int actorId, int userId, int roleId, int statusId, string subscriberNumber, int loginStatusId, bool isDemo,
			 bool isPeer, string comments, string firstName, string lastName, string email, string secondaryEmail,
			 string address, string city, string country, string state, string zip,
			 string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail,
			 string companyName, bool ecommerceEnabled, string additionalParams)
		{
			if (UseEntityFramework)
			{
				if (CanUpdateUserDetails(actorId, userId))
				{
					var user = Users.FirstOrDefault(u => u.UserId == userId);
					if (user != null)
					{
						if (loginStatusId == 0) user.FailedLogins = 0;
						user.RoleId = roleId;
						user.StatusId = statusId;
						user.SubscriberNumber = subscriberNumber;
						user.LoginStatusId = loginStatusId;
						user.IsDemo = isDemo;
						user.IsPeer = isPeer;
						user.Comments = comments;
						user.FirstName = firstName;
						user.LastName = lastName;
						user.Email = email;
						user.SecondaryEmail = secondaryEmail;
						user.Address = address;
						user.City = city;
						user.Country = country;
						user.State = state;
						user.Zip = zip;
						user.PrimaryPhone = primaryPhone;
						user.SecondaryPhone = secondaryPhone;
						user.Fax = fax;
						user.InstantMessenger = instantMessenger;
						user.HtmlMail = htmlMail;
						user.CompanyName = companyName;
						user.EcommerceEnabled = ecommerceEnabled;
						user.AdditionalParams = additionalParams;

						SaveChanges();
					}
				}
			}
			else
			{
				// update user
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateUser",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@RoleID", roleId),
					 new SqlParameter("@StatusId", statusId),
					 new SqlParameter("@SubscriberNumber", subscriberNumber),
					 new SqlParameter("@LoginStatusId", loginStatusId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@IsDemo", isDemo),
					 new SqlParameter("@IsPeer", isPeer),
					 new SqlParameter("@Comments", comments),
					 new SqlParameter("@firstName", firstName),
					 new SqlParameter("@lastName", lastName),
					 new SqlParameter("@email", email),
					 new SqlParameter("@secondaryEmail", secondaryEmail),
					 new SqlParameter("@address", address),
					 new SqlParameter("@city", city),
					 new SqlParameter("@country", country),
					 new SqlParameter("@state", state),
					 new SqlParameter("@zip", zip),
					 new SqlParameter("@primaryPhone", primaryPhone),
					 new SqlParameter("@secondaryPhone", secondaryPhone),
					 new SqlParameter("@fax", fax),
					 new SqlParameter("@instantMessenger", instantMessenger),
					 new SqlParameter("@htmlMail", htmlMail),
					 new SqlParameter("@CompanyName", companyName),
					 new SqlParameter("@EcommerceEnabled", ecommerceEnabled),
					 new SqlParameter("@AdditionalParams", additionalParams));
			}
		}

		public void UpdateUserFailedLoginAttempt(int userId, int lockOut, bool reset)
		{
			if (UseEntityFramework)
			{
				var user = Users.FirstOrDefault(u => u.UserId == userId);
				if (user == null) return;

				if (reset) user.FailedLogins = 0;
				else if (lockOut <= (user.FailedLogins ?? 0)) user.LoginStatusId = 2;
				else user.FailedLogins = (user.FailedLogins ?? 0) + 1;
				
				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateUserFailedLoginAttempt",
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@LockOut", lockOut),
					 new SqlParameter("@Reset", reset));
			}
		}

		public void DeleteUser(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				if (!CanUpdateUserDetails(actorId, userId)) return;

				// delete user comments
				Comments.RemoveRange(Comments.Where(c => c.ItemId == userId && c.ItemTypeId == "USER"));
				// delete reseller addon
				HostingPlans.RemoveRange(HostingPlans.Where(h => h.UserId == userId && h.IsAddon == true));
				// delete user peers
				Users.RemoveRange(Users.Where(u => u.IsPeer && u.OwnerId == userId));
				// delete user
				Users.RemoveRange(Users.Where(u => u.UserId == userId));

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "DeleteUser",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId));
			}
		}

		public void ChangeUserPassword(int actorId, int userId, string password)
		{
			if (UseEntityFramework)
			{
				if (!CanUpdateUserDetails(actorId, userId)) return;

				var user = Users.FirstOrDefault(u => u.UserId == userId);
				if (user == null) return;

				user.Password = password;
				user.OneTimePasswordState = 0;

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "ChangeUserPassword",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@password", password));
			}
		}

		public void SetUserOneTimePassword(int userId, string password, int auths)
		{
			if (UseEntityFramework)
			{
				var user = Users.FirstOrDefault(u => u.UserId == userId);
				if (user == null) return;
		
				user.Password = password;
				user.OneTimePasswordState = auths;
				
				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "SetUserOneTimePassword",
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@Password", password),
					 new SqlParameter("@OneTimePasswordState", auths));
			}
		}

		public void UpdateUserPinSecret(int actorId, int userId, string pinSecret)
		{
			if (UseEntityFramework)
			{
				if (!CanUpdateUserDetails(actorId, userId)) return;

				var user = Users.FirstOrDefault(u => u.UserId == userId);
				if (user == null) return;
				
				user.PinSecret = pinSecret;
				
				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateUserPinSecret",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@PinSecret", pinSecret)
					 );
			}
		}

		public void UpdateUserMfaMode(int actorId, int userId, int mfaMode)
		{
			if (UseEntityFramework)
			{
				if (!CanUpdateUserDetails(actorId, userId)) return;

				var user = Users.FirstOrDefault(u => u.UserId == userId);
				if (user == null) return;
				
				user.MfaMode = mfaMode;
				
				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateUserMfaMode",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@MfaMode", mfaMode));
			}
		}

		public bool CanUserChangeMfa(int callerId, int changeUserId, bool canPeerChangeMfa)
		{
			if (UseEntityFramework)
			{
				var user = Users
					.Select(u => new { u.UserId, u.OwnerId, u.IsPeer })
					.FirstOrDefault(u => u.UserId == callerId);
				if (user == null) return false;

				if (user.OwnerId == null) return true; // serveradmin user

				// check if the user requests himself
				if (callerId == changeUserId && user.IsPeer && canPeerChangeMfa) return true;

				if (callerId == changeUserId && !user.IsPeer) return true;

				int generationNumber = 0, userId = user.UserId;
				if (user.IsPeer)
				{
					userId = user.OwnerId.Value;
					generationNumber = 1;
				}

				var generation = Users
					.Where(u => u.UserId == userId)
					.Select(u => new
					{
						u.UserId,
						u.Username,
						u.OwnerId,
						u.IsPeer,
						GenerationNumber = 0
					});
				var nextGeneration = Users.Join(generation, u => u.OwnerId, g => g.UserId, (usr, gen) => new {
					usr.UserId,
					usr.Username,
					usr.OwnerId,
					usr.IsPeer,
					GenerationNumber = gen.GenerationNumber + 1
				});
				while (nextGeneration.Any())
				{
					generation = generation.Union(nextGeneration);
				}

				return generation.Join(Users, g => g.OwnerId, u => u.UserId, (gen, usr) => gen)
					.Any(g => (g.GenerationNumber > generationNumber || !g.IsPeer) &&
						g.UserId == changeUserId);
			}
			else
			{
				SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Bit);
				prmResult.Direction = ParameterDirection.Output;
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "CanChangeMfa",
					 new SqlParameter("@CallerID", callerId),
					 new SqlParameter("@ChangeUserID", changeUserId),
					 new SqlParameter("@CanPeerChangeMfa", canPeerChangeMfa ? 1 : 0),
					 prmResult
					 );

				return Convert.ToBoolean(prmResult.Value);
			}
		}

		public IDataReader GetUserPackagesServerUrls(int userId)
		{
			if (UseEntityFramework)
			{
				var serverUrls = Servers.Join(Packages, s => s.ServerId, p => p.ServerId, (server, package) => new
				{
					server.ServerUrl,
					package.UserId
				})
				.Where(s => s.UserId == userId)
				.Select(s => new { s.ServerUrl });

				return new EntityDataReader<object>(serverUrls);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserPackagesServerUrls",
					 new SqlParameter("@UserId", userId));
			}
		}

		#endregion

		#region User Settings
		public IDataReader GetUserSettings(int actorId, int userId, string settingsName)
		{
			if (UseEntityFramework)
			{
				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				var id = userId;
				var setting = UserSettings.FirstOrDefault(s => s.UserId == id);
				while (setting == null)
				{
					var user = Users
						.Select(u => new { u.UserId, u.OwnerId })
						.FirstOrDefault(u => u.UserId == id);
					if (user != null && user.OwnerId.HasValue) id = user.OwnerId.Value;
					setting = UserSettings.FirstOrDefault(s => s.UserId == id);
				}

				return new EntityDataReader<Data.Entities.UserSetting>(new Data.Entities.UserSetting[] { setting });
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserSettings",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@SettingsName", settingsName));
			}
		}
		public void UpdateUserSettings(int actorId, int userId, string settingsName, string xml)
		{
			if (UseEntityFramework)
			{
				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("Not authorized");

				UserSettings.RemoveRange(UserSettings
					.Where(s => s.UserId == userId && s.SettingsName == settingsName));
				
				var properties = XElement.Parse(xml);
				foreach (var property in properties.Descendants())
				{
					var setting = new Data.Entities.UserSetting()
					{
						UserId = userId,
						SettingsName = settingsName,
						PropertyName = (string)property.Attribute("name"),
						PropertyValue = (string)property.Attribute("value")
					};
					UserSettings.Add(setting);
				}

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateUserSettings",
					 new SqlParameter("@UserID", userId),
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@SettingsName", settingsName),
					 new SqlParameter("@Xml", xml));
			}
		}
		#endregion

		#region Servers
		bool CheckIsUserAdmin(int userId)
		{
			return userId == -1 || Users.Any(u => u.UserId == userId && u.RoleId == 1);
		}
		public DataSet GetAllServers(int actorId)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				var servers = Servers
					.Where(s => isAdmin)
					.OrderBy(s => s.VirtualServer)
					.ThenBy(s => s.ServerName)
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.ServerUrl,
						ServicesNumber = VirtualServices.Count(v => v.ServerId == s.ServerId),
						s.Comments
					});

				var serversTable = ObjectUtils.DataTableFromEntitySet(servers);

				var services = Services
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (srvc, p) => new { Service = srvc, ProviderGroupId = p.GroupId })
					.Join(ResourceGroups, s => s.ProviderGroupId, rg => rg.GroupId, (srvc, rg) => new
					{
						Service = srvc.Service,
						GroupOrder = rg.GroupOrder
					})
					.Where(s => isAdmin)
					.OrderBy(s => s.GroupOrder)
					.Select(s => new {
						s.Service.ServiceId,
						s.Service.ServerId,
						s.Service.ProviderId,
						s.Service.ServiceName,
						s.Service.Comments
					});

				var servicesTable = ObjectUtils.DataTableFromEntitySet(services);

				var dataSet = new DataSet();
				dataSet.Tables.Add(serversTable);
				dataSet.Tables.Add(servicesTable);

				return dataSet;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetAllServers",
					 new SqlParameter("@actorId", actorId));
			}
		}
		public DataSet GetServers(int actorId)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				var servers = Servers
					.Where(s => isAdmin && !s.VirtualServer)
					.OrderBy(s => s.ServerName)
					.Select(s => new
					{
						s.ServerId, s.ServerName, s.ServerUrl, 
						ServicesNumber = Services.Where(sc => sc.ServerId == s.ServerId).Count(),
						s.Comments, s.PrimaryGroupId, s.ADEnabled
					});

				var services = Services
					.Where(s => isAdmin)
					.Join(Providers, srvc => srvc.ProviderId, p => p.ProviderId, (srvc, prov) => new
					{
						Service = srvc,
						prov.GroupId
					})
					.Join(ResourceGroups, srvc => srvc.GroupId, rg => rg.GroupId, (srvc, rg) => new
					{
						srvc.Service,
						rg.GroupOrder
					})
					.OrderBy(s => s.GroupOrder)
					.Select(s => new
					{
						s.Service.ServiceId,
						s.Service.ServerId,
						s.Service.ProviderId,
						s.Service.ServiceName,
						s.Service.Comments
					});

				var set = new DataSet();
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(servers));
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(services));

				return set;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetServers",
					 new SqlParameter("@actorId", actorId));
			}
		}

		public IDataReader GetServer(int actorId, int serverId, bool forAutodiscover)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				var server = Servers
					.Where(s => s.ServerId == serverId && (isAdmin || forAutodiscover))
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.ServerUrl,
						s.Password,
						s.Comments,
						s.VirtualServer,
						s.InstantDomainAlias,
						s.PrimaryGroupId,
						s.ADEnabled,
						s.ADRootDomain,
						s.ADUsername,
						s.ADPassword,
						s.ADAuthenticationType,
						s.ADParentDomain,
						s.ADParentDomainController,
						s.OSPlatform,
						s.IsCore,
						s.PasswordIsSHA256
					});

				return new EntityDataReader<object>(server);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetServer",
					 new SqlParameter("@actorId", actorId),
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@forAutodiscover", forAutodiscover));
			}
		}

		public IDataReader GetServerShortDetails(int serverId)
		{
			if (UseEntityFramework)
			{
				var server = Servers
					.Where(s => s.ServerId == serverId)
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.Comments,
						s.VirtualServer,
						s.InstantDomainAlias
					});

				return new EntityDataReader<object>(server);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetServerShortDetails",
					 new SqlParameter("@ServerID", serverId));
			}
		}

		public IDataReader GetServerByName(int actorId, string serverName)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				var server = Servers
					.Where(s => isAdmin && s.ServerName == serverName)
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.ServerUrl,
						s.Password,
						s.Comments,
						s.VirtualServer,
						s.InstantDomainAlias,
						s.PrimaryGroupId,
						s.ADRootDomain,
						s.ADUsername,
						s.ADPassword,
						s.ADAuthenticationType,
						s.ADParentDomain,
						s.ADParentDomainController,
						s.OSPlatform,
						s.IsCore,
						s.PasswordIsSHA256
					});

				return new EntityDataReader<object>(server);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetServerByName",
					 new SqlParameter("@actorId", actorId),
					 new SqlParameter("@ServerName", serverName));
			}
		}

		public IDataReader GetServerInternal(int serverId)
		{
			if (UseEntityFramework)
			{
				var server = Servers
					.Where(s => s.ServerId == serverId)
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.ServerUrl,
						s.Password,
						s.Comments,
						s.VirtualServer,
						s.InstantDomainAlias,
						s.PrimaryGroupId,
						s.ADEnabled,
						s.ADRootDomain,
						s.ADUsername,
						s.ADPassword,
						s.ADAuthenticationType,
						s.ADParentDomain,
						s.ADParentDomainController,
						s.OSPlatform,
						s.IsCore,
						s.PasswordIsSHA256
					});

				return new EntityDataReader<object>(server);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetServerInternal",
					 new SqlParameter("@ServerID", serverId));
			}
		}

		public int AddServer(string serverName, string serverUrl,
			 string password, string comments, bool virtualServer, string instantDomainAlias,
			 int primaryGroupId, bool adEnabled, string adRootDomain, string adUsername, string adPassword,
			 string adAuthenticationType, OSPlatform osPlatform, bool? isCore, bool PasswordIsSHA256)
		{
			if (UseEntityFramework)
			{
				var server = new Data.Entities.Server()
				{
					ServerName = serverName, ServerUrl = serverUrl, Password = password, Comments = comments,
					VirtualServer = virtualServer, InstantDomainAlias = instantDomainAlias,
					PrimaryGroupId = primaryGroupId != 0 ? primaryGroupId : null, ADEnabled = adEnabled, ADRootDomain = adRootDomain,
					ADUsername = adUsername, ADPassword = adPassword, ADAuthenticationType = adAuthenticationType,
					OSPlatform = osPlatform, IsCore = isCore, PasswordIsSHA256 = PasswordIsSHA256
				};
				Servers.Add(server);
				SaveChanges();

				return server.ServerId;
			}
			else
			{
				SqlParameter prmServerId = new SqlParameter("@ServerID", SqlDbType.Int);
				prmServerId.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "AddServer",
					 prmServerId,
					 new SqlParameter("@ServerName", serverName),
					 new SqlParameter("@ServerUrl", serverUrl),
					 new SqlParameter("@Password", password),
					 new SqlParameter("@Comments", comments),
					 new SqlParameter("@VirtualServer", virtualServer),
					 new SqlParameter("@InstantDomainAlias", instantDomainAlias),
					 new SqlParameter("@PrimaryGroupId", primaryGroupId),
					 new SqlParameter("@AdEnabled", adEnabled),
					 new SqlParameter("@AdRootDomain", adRootDomain),
					 new SqlParameter("@AdUsername", adUsername),
					 new SqlParameter("@AdPassword", adPassword),
					 new SqlParameter("@AdAuthenticationType", adAuthenticationType),
					 new SqlParameter("@OSPlatform", osPlatform),
					 new SqlParameter("@IsCore", isCore),
					 new SqlParameter("@PasswordIsSHA256", PasswordIsSHA256));

				return Convert.ToInt32(prmServerId.Value);
			}
		}

		public void UpdateServer(int serverId, string serverName, string serverUrl,
			 string password, string comments, string instantDomainAlias,
			 int primaryGroupId, bool adEnabled, string adRootDomain, string adUsername, string adPassword,
			 string adAuthenticationType, string adParentDomain, String adParentDomainController,
			 OSPlatform osPlatform, bool? isCore, bool PasswordIsSHA256)
		{
			if (UseEntityFramework)
			{
				var server = Servers.FirstOrDefault(s => s.ServerId == serverId);
				if (server == null) return;
				
				server.ServerName = serverName;
				server.ServerUrl = serverUrl;
				server.Password = password;
				server.Comments = comments;
				server.InstantDomainAlias = instantDomainAlias;
				server.PrimaryGroupId = primaryGroupId != 0 ? primaryGroupId : null;
				server.ADEnabled = adEnabled;
				server.ADRootDomain = adRootDomain;
				server.ADUsername = adUsername;
				server.ADPassword = adPassword;
				server.ADAuthenticationType = adAuthenticationType;
				server.ADParentDomain = adParentDomain;
				server.ADParentDomainController = adParentDomainController;
				server.OSPlatform = osPlatform;
				server.IsCore = isCore;
				server.PasswordIsSHA256 = PasswordIsSHA256;

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateServer",
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@ServerName", serverName),
					 new SqlParameter("@ServerUrl", serverUrl),
					 new SqlParameter("@Password", password),
					 new SqlParameter("@Comments", comments),
					 new SqlParameter("@InstantDomainAlias", instantDomainAlias),
					 new SqlParameter("@PrimaryGroupId", primaryGroupId),
					 new SqlParameter("@AdEnabled", adEnabled),
					 new SqlParameter("@AdRootDomain", adRootDomain),
					 new SqlParameter("@AdUsername", adUsername),
					 new SqlParameter("@AdPassword", adPassword),
					 new SqlParameter("@AdAuthenticationType", adAuthenticationType),
					 new SqlParameter("@AdParentDomain", adParentDomain),
					 new SqlParameter("@AdParentDomainController", adParentDomainController),
					 new SqlParameter("@OSPlatform", osPlatform),
					 new SqlParameter("@IsCore", isCore),
					 new SqlParameter("@PasswordIsSHA256", PasswordIsSHA256));
			}
		}

		public int DeleteServer(int serverId)
		{
			if (UseEntityFramework)
			{
				// check related services
				if (Services.Any(svc => svc.ServerId == serverId)) return -1;

				// check related packages
				if (Packages.Any(p => p.ServerId == serverId)) return -2;

				// check related hosting plans
				if (HostingPlans.Any(p => p.ServerId == serverId)) return -3;

				// delete IP addresses
				IpAddresses.RemoveRange(IpAddresses.Where(ip => ip.ServerId == serverId));

				// delete global DNS records
				GlobalDnsRecords.RemoveRange(GlobalDnsRecords.Where(r => r.ServerId == serverId));

				// delete server
				Servers.RemoveRange(Servers.Where(s => s.ServerId == serverId));

				// delete virtual services if any
				VirtualServices.RemoveRange(VirtualServices.Where(vs => vs.ServerId == serverId));

				SaveChanges();

				return 0;
			}
			else
			{
				SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
				prmResult.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "DeleteServer",
					 prmResult,
					 new SqlParameter("@ServerID", serverId));

				return Convert.ToInt32(prmResult.Value);
			}
		}
		#endregion

		#region Virtual Servers
		public DataSet GetVirtualServers(int actorId)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				var server = Servers
					.Where(s => isAdmin && s.VirtualServer)
					.OrderBy(s => s.ServerName)
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.ServerUrl,
						ServicesNumber = VirtualServices.Where(v => v.ServerId == s.ServerId).Count(),
						s.Comments,
						s.PrimaryGroupId
					});

				return ObjectUtils.DataSetFromEntitySet(server);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetVirtualServers",
					 new SqlParameter("@actorId", actorId));
			}
		}

		public DataSet GetAvailableVirtualServices(int actorId, int serverId)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				var servers = Servers
					.Where(s => isAdmin && !s.VirtualServer) // get only physical servers
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.Comments
					});

				var virtualServiceIds = VirtualServices
					.Where(v => v.ServerId == serverId)
					.Select(v => v.ServiceId)
					.ToArray();
				var services = Services
					.Where(s => isAdmin && !virtualServiceIds.Any(id => id == s.ServiceId))
					.Select(s => new
					{
						s.ServiceId,
						s.ServerId,
						s.ProviderId,
						s.ServiceName,
						s.Comments
					});

				var set = new DataSet();
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(servers));
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(services));

				return set;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetAvailableVirtualServices",
					 new SqlParameter("@actorId", actorId),
					 new SqlParameter("@ServerID", serverId));
			}
		}

		public DataSet GetVirtualServices(int actorId, int serverId, bool forAutodiscover)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				// virtual groups
				// TODO is this correct LEFT OUTER JOIN?
				var virtGroups = ResourceGroups
					.GroupJoin(VirtualGroups, rg => rg.GroupId, vg => vg.GroupId, (rg, vgs) => new
					{
						ResourceGroup = rg,
						VirtualGroup = vgs
							.Where(vg => vg.ServerId == serverId)
							.SingleOrDefault()
					})
					.Where(g => (isAdmin || forAutodiscover) && g.ResourceGroup.ShowGroup == true)
					.OrderBy(g => g.ResourceGroup.GroupOrder)
					.Select(g => new
					{
						VirtualGroupId = (int?)(g.VirtualGroup != null ? g.VirtualGroup.VirtualGroupId : null),
						g.ResourceGroup.GroupId,
						g.ResourceGroup.GroupName,
						DistributionType = (g.VirtualGroup != null ? g.VirtualGroup.DistributionType : null) ?? 1,
						BindDistributionToPrimary = (g.VirtualGroup != null ? g.VirtualGroup.BindDistributionToPrimary : null) ?? true
					});

				var services = VirtualServices
					.Where(vs => vs.ServerId == serverId && (isAdmin || forAutodiscover))
					.Join(Services, vs => vs.ServiceId, s => s.ServiceId, (vs, s) => new
					{
						VirtualService = vs,
						Service = s
					})
					.Join(Servers, j => j.Service.ServerId, s => s.ServerId, (j, s) => new
					{
						j.VirtualService,
						j.Service,
						Server = s
					})
					.Join(Providers, j => j.Service.ProviderId, p => p.ProviderId, (j, p) => new
					{
						j.VirtualService.ServerId,
						j.Service.ServiceName,
						j.Service.Comments,
						p.GroupId,
						p.DisplayName,
						j.Server.ServerName
					});

				var set = new DataSet();
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(virtGroups));
				set.Tables.Add(ObjectUtils.DataTableFromEntitySet(services));

				return set;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetVirtualServices",
					 new SqlParameter("@actorId", actorId),
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@forAutodiscover", forAutodiscover));
			}
		}

		public void AddVirtualServices(int serverId, string xml)
		{
			if (UseEntityFramework)
			{
				/* XML Format:
				<services>
					<service id=""16"" />
				</services> */

				var services = XElement.Parse(xml);

				var existingServices = VirtualServices
					.Where(v => v.ServerId == serverId)
					.Select(v => v.ServiceId)
					.ToHashSet();

				bool addedAny = false;
				foreach (var service in services.Descendants())
				{
					var serviceId = (int)service.Attribute("id");
					if (!existingServices.Contains(serviceId))
					{
						var virtualService = new Data.Entities.VirtualService()
						{
							ServerId = serverId,
							ServiceId = serviceId
						};
						VirtualServices.Add(virtualService);
						addedAny = true;
					}
				}

				if (addedAny) SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "AddVirtualServices",
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@xml", xml));
			}
		}

		public void DeleteVirtualServices(int serverId, string xml)
		{
			if (UseEntityFramework)
			{
				/* XML Format:
				<services>
					<service id=""16"" />
				</services> */

				var services = XElement.Parse(xml);
				var serviceIds = services.Descendants()
					.Select(service => (int)service.Attribute("id"))
					.ToArray();
				var toDelete = VirtualServices
					.Where(vs => vs.ServerId == serverId)
					.Join(serviceIds, vs => vs.ServiceId, sid => sid, (vs, sid) => vs);
				VirtualServices.RemoveRange(toDelete);

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "DeleteVirtualServices",
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@xml", xml));
			}
		}

		public void UpdateVirtualGroups(int serverId, string xml)
		{
			if (UseEntityFramework)
			{
				/* XML Format:
				<groups>
					<group id=""16"" distributionType=""1"" bindDistributionToPrimary=""1""/>
				</groups> */

				var groupsXml = XElement.Parse(xml);
				var groups = groupsXml.Descendants()
					.Select(group => new Data.Entities.VirtualGroup()
					{
						ServerId = serverId,
						GroupId = (int)group.Attribute("id"),
						DistributionType = (int?)group.Attribute("distributionType"),
						BindDistributionToPrimary = ((int?)group.Attribute("bindDistributionToPrimary") ?? 1) == 1
					});

				// delete existing groups
				VirtualGroups.RemoveRange(VirtualGroups.Where(vg => vg.ServerId == serverId));

				VirtualGroups.AddRange(groups);

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdateVirtualGroups",
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@xml", xml));
			}
		}
		#endregion

		#region Providers

		// Providers methods

		public DataSet GetProviders()
		{
			if (UseEntityFramework)
			{
				// TODO ProviderName was duplicate in the Stored Procedure. Resolve correctly. I've changed
				// the duplicate ProviderName to ProviderGroupedName
				var providers = Providers
					.Join(ResourceGroups, p => p.GroupId, rg => rg.GroupId, (p, rg) => new
					{
						Provider = p,
						ResourceGroup = rg
					})
					.OrderBy(j => j.ResourceGroup.GroupOrder)
					.ThenBy(j => j.Provider.DisplayName)
					.Select(j => new
					{
						j.Provider.ProviderId,
						j.Provider.GroupId,
						j.Provider.ProviderName,
						j.Provider.EditorControl,
						j.Provider.DisplayName,
						j.Provider.ProviderType,
						ProviderGroupedName = j.ResourceGroup.GroupName + " - " + j.Provider.DisplayName,
						j.Provider.DisableAutoDiscovery
					});

				return ObjectUtils.DataSetFromEntitySet(providers);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetProviders");
			}
		}

		public DataSet GetGroupProviders(int groupId)
		{
			if (UseEntityFramework)
			{
				// TODO ProviderName was duplicate in the Stored Procedure. Resolve correctly. I've changed
				// the duplicate ProviderName to ProviderGroupedName
				var providers = Providers
					.Where(p => p.GroupId == groupId)
					.Join(ResourceGroups, p => p.GroupId, rg => rg.GroupId, (p, rg) => new
					{
						Provider = p,
						ResourceGroup = rg
					})
					.OrderBy(j => j.ResourceGroup.GroupOrder)
					.ThenBy(j => j.Provider.DisplayName)
					.Select(j => new
					{
						j.Provider.ProviderId,
						j.Provider.GroupId,
						j.Provider.ProviderName,
						j.Provider.DisplayName,
						j.Provider.ProviderType,
						ProviderGroupedName = j.ResourceGroup.GroupName + " - " + j.Provider.DisplayName,
					});

				return ObjectUtils.DataSetFromEntitySet(providers);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetGroupProviders",
					 new SqlParameter("@groupId", groupId));
			}
		}

		public IDataReader GetProvider(int providerId)
		{
			if (UseEntityFramework)
			{
				var provider = Providers
					.Where(p => p.ProviderId == providerId)
					.Select(p => new
					{
						p.ProviderId,
						p.GroupId,
						p.ProviderName,
						p.EditorControl,
						p.DisplayName,
						p.ProviderType
					});

				return new EntityDataReader<object>(provider);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetProvider",
					 new SqlParameter("@ProviderID", providerId));
			}
		}

		public IDataReader GetProviderByServiceID(int serviceId)
		{
			if (UseEntityFramework)
			{
				var provider = Services
					.Where(s => s.ServiceId == serviceId)
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						p.ProviderId,
						p.GroupId,
						p.DisplayName,
						p.EditorControl,
						p.ProviderType
					});

				return new EntityDataReader<object>(provider);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetProviderByServiceID",
					 new SqlParameter("@ServiceID", serviceId));
			}
		}

		#endregion

		#region Private Network VLANs
		public int AddPrivateNetworkVLAN(int serverId, int vlan, string comments)
		{
			if (UseEntityFramework)
			{
				var vlanEntity = new Data.Entities.PrivateNetworkVlan()
				{
					ServerId = serverId != 0 ? serverId : null,
					Vlan = vlan,
					Comments = comments
				};
				PrivateNetworkVlans.Add(vlanEntity);
				
				SaveChanges();

				return vlanEntity.VlanId;
			}
			else
			{
				SqlParameter prmAddresId = new SqlParameter("@VlanID", SqlDbType.Int);
				prmAddresId.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "AddPrivateNetworkVlan",
					 prmAddresId,
					 new SqlParameter("@Vlan", vlan),
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@Comments", comments));

				return Convert.ToInt32(prmAddresId.Value);
			}
		}

		public int DeletePrivateNetworkVLAN(int vlanId)
		{
			if (UseEntityFramework)
			{
				if (PackageVlans.Any(vlan => vlan.VlanId == vlanId)) return -2;

				PrivateNetworkVlans.RemoveRange(PrivateNetworkVlans.Where(vlan => vlan.VlanId == vlanId));

				SaveChanges();

				return 0;
			}
			else
			{
				SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
				prmResult.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "DeletePrivateNetworkVLAN",
					 prmResult,
					 new SqlParameter("@VlanID", vlanId));

				return Convert.ToInt32(prmResult.Value);
			}
		}

		public IDataReader GetPrivateNetworVLANsPaged(int actorId, int serverId,
			 string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				var isAdmin = CheckIsUserAdmin(actorId);

				var vlans = PrivateNetworkVlans
					.Where(vlan => isAdmin && (serverId == 0 || serverId != 0 && serverId == vlan.ServerId))
					.Join(Servers, v => v.ServerId, s => s.ServerId, (v, s) => new
					{
						Vlan = v,
						Server = s
					})
					.Join(PackageVlans, j => j.Vlan.VlanId, pv => pv.VlanId, (j, pv) => new
					{
						j.Vlan,
						j.Server,
						PackageVlan = pv
					})
					.Join(Packages, j => j.PackageVlan.PackageId, p => p.PackageId, (j, p) => new
					{
						j.Vlan,
						j.Server,
						j.PackageVlan,
						Package = p
					})
					.Join(Users, j => j.Package.UserId, u => u.UserId, (j, u) => new
					{
						j.Vlan.VlanId,
						j.Vlan.Vlan,
						j.Vlan.Comments,
						j.Vlan.ServerId,
						j.Server.ServerName,
						j.PackageVlan.PackageId,
						j.Package.PackageName,
						j.Package.UserId,
						u.Username
					});

				if (!string.IsNullOrEmpty(filterValue))
				{
					if (!string.IsNullOrEmpty(filterColumn))
					{
						vlans = vlans.Where($"{filterColumn} == @0", filterValue);
					} else
					{
						vlans = vlans.Where(v => v.Vlan.ToString() == filterValue || v.ServerName == filterValue ||
							v.Username == filterValue);
					}
				}

				if (string.IsNullOrEmpty(sortColumn)) sortColumn = "Vlan";

				vlans = vlans.OrderBy(sortColumn);

				vlans = vlans.Skip(startRow).Take(maximumRows);

				return new EntityDataReader<object>(vlans);
			}
			else
			{
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
												 "GetPrivateNetworVLANsPaged",
													 new SqlParameter("@ActorId", actorId),
													 new SqlParameter("@ServerId", serverId),
													 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
													 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
													 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
													 new SqlParameter("@startRow", startRow),
													 new SqlParameter("@maximumRows", maximumRows));
				return reader;
			}
		}

		public IDataReader GetPrivateNetworVLAN(int vlanId)
		{
			if (UseEntityFramework)
			{
				var vlan = PrivateNetworkVlans
					.Where(v => v.VlanId == vlanId);

				return new EntityDataReader<Data.Entities.PrivateNetworkVlan>(vlan);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetPrivateNetworVLAN",
					 new SqlParameter("@VlanID", vlanId));
			}
		}

		public void UpdatePrivateNetworVLAN(int vlanId, int serverId, int vlan, string comments)
		{
			if (UseEntityFramework)
			{
				var vl = PrivateNetworkVlans.FirstOrDefault(v => v.VlanId == vlanId);
				if (vl == null) return;

				vl.ServerId = serverId != 0 ? serverId : null;
				vl.Vlan = vlan;
				vl.Comments = comments;

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "UpdatePrivateNetworVLAN",
					 new SqlParameter("@VlanID", vlanId),
					 new SqlParameter("@ServerID", serverId),
					 new SqlParameter("@Vlan", vlan),
					 new SqlParameter("@Comments", comments));
			}
		}

		bool CheckPackageParent(int parentPackageId, int packageId)
		{
			// check if the user requests hiself
			if (parentPackageId == packageId) return true;

			var id = packageId;
			var package = Packages
				.Select(p => new { p.PackageId, p.ParentPackageId })
				.FirstOrDefault(p => p.PackageId == id);
			while (package != null && package.ParentPackageId.HasValue && package.ParentPackageId != parentPackageId)
			{
				id = package.ParentPackageId.Value;
				package = Packages
					.Select(p => new { p.PackageId, p.ParentPackageId })
					.FirstOrDefault(p => p.PackageId == id);
			}
			return package != null && package.ParentPackageId == parentPackageId;
		}

		public IDataReader GetPackagePrivateNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				var vlans = PackageVlans
					.Where(pv => CheckPackageParent(packageId, pv.PackageId))
					.Join(PrivateNetworkVlans, p => p.VlanId, v => v.VlanId, (pv, vl) => new
					{
						PackageVlan = pv,
						Vlan = vl
					})
					.Join(Packages, j => j.PackageVlan.PackageId, p => p.PackageId, (j, p) => new
					{
						j.PackageVlan, j.Vlan, Package = p
					})
					.Join(Users, j => j.Package.UserId, u => u.UserId, (j, u) => new
					{
						j.PackageVlan.PackageVlanId,
						j.PackageVlan.VlanId,
						j.Vlan.Vlan,
						j.PackageVlan.PackageId,
						j.Package.PackageName,
						j.Package.UserId,
						u.Username
					});

				if (!string.IsNullOrEmpty(sortColumn)) vlans = vlans.OrderBy(sortColumn);
				else vlans = vlans.OrderBy(v => v.Vlan);

				vlans = vlans.Skip(startRow).Take(maximumRows);

				return new EntityDataReader<object>(vlans);
			}
			else
			{
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
												 "GetPackagePrivateNetworkVLANs",
													 new SqlParameter("@PackageID", packageId),
													 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
													 new SqlParameter("@startRow", startRow),
													 new SqlParameter("@maximumRows", maximumRows));
				return reader;
			}
		}

		public void DeallocatePackageVLAN(int id)
		{
			if (UseEntityFramework)
			{
				var packageVlan = PackageVlans
					.Where(pv => pv.PackageVlanId == id)
					.Include(pv => pv.Package)
					.Single();

				if (packageVlan.Package.ParentPackageId == 1) // System space
				{
					PackageVlans.RemoveRange(PackageVlans.Where(pv => pv.PackageVlanId == id));
				}
				else // 2nd level space and below
				{
					packageVlan.PackageId = id;			
				}

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeallocatePackageVLAN",
												  new SqlParameter("@PackageVlanID", id));
			}
		}

		public IDataReader GetUnallottedVLANs(int packageId, int serviceId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetUnallottedVLANs",
												 new SqlParameter("@PackageId", packageId),
												 new SqlParameter("@ServiceId", serviceId));
		}

		public void AllocatePackageVLANs(int packageId, string xml)
		{
			SqlParameter[] param = new[]
												{
														 new SqlParameter("@PackageID", packageId),
														 new SqlParameter("@xml", xml)
													};

			ExecuteLongNonQuery("AllocatePackageVLANs", param);
		}
		#endregion

		#region IPAddresses
		public IDataReader GetIPAddress(int ipAddressId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetIPAddress",
				 new SqlParameter("@AddressID", ipAddressId));
		}

		public IDataReader GetIPAddresses(int actorId, int poolId, int serverId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetIPAddresses",
												 new SqlParameter("@ActorId", actorId),
												 new SqlParameter("@PoolId", poolId),
												 new SqlParameter("@ServerId", serverId));
			return reader;
		}

		public IDataReader GetIPAddressesPaged(int actorId, int poolId, int serverId,
			 string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetIPAddressesPaged",
												 new SqlParameter("@ActorId", actorId),
												 new SqlParameter("@PoolId", poolId),
												 new SqlParameter("@ServerId", serverId),
												 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
												 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
												 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
												 new SqlParameter("@startRow", startRow),
												 new SqlParameter("@maximumRows", maximumRows));
			return reader;
		}

		public int AddIPAddress(int poolId, int serverId, string externalIP, string internalIP,
			 string subnetMask, string defaultGateway, string comments, int VLAN)
		{
			SqlParameter prmAddresId = new SqlParameter("@AddressID", SqlDbType.Int);
			prmAddresId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddIPAddress",
				 prmAddresId,
				 new SqlParameter("@ServerID", serverId),
				 new SqlParameter("@externalIP", externalIP),
				 new SqlParameter("@internalIP", internalIP),
				 new SqlParameter("@PoolId", poolId),
				 new SqlParameter("@SubnetMask", subnetMask),
				 new SqlParameter("@DefaultGateway", defaultGateway),
				 new SqlParameter("@Comments", comments),
				 new SqlParameter("@VLAN", VLAN));

			return Convert.ToInt32(prmAddresId.Value);
		}

		public void UpdateIPAddress(int addressId, int poolId, int serverId,
			 string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateIPAddress",
				 new SqlParameter("@AddressID", addressId),
				 new SqlParameter("@externalIP", externalIP),
				 new SqlParameter("@internalIP", internalIP),
				 new SqlParameter("@ServerID", serverId),
				 new SqlParameter("@PoolId", poolId),
				 new SqlParameter("@SubnetMask", subnetMask),
				 new SqlParameter("@DefaultGateway", defaultGateway),
				 new SqlParameter("@Comments", comments),
				 new SqlParameter("@VLAN", VLAN));
		}

		public void UpdateIPAddresses(string xmlIds, int poolId, int serverId,
			 string subnetMask, string defaultGateway, string comments, int VLAN)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateIPAddresses",
				 new SqlParameter("@Xml", xmlIds),
				 new SqlParameter("@ServerID", serverId),
				 new SqlParameter("@PoolId", poolId),
				 new SqlParameter("@SubnetMask", subnetMask),
				 new SqlParameter("@DefaultGateway", defaultGateway),
				 new SqlParameter("@Comments", comments),
				 new SqlParameter("@VLAN", VLAN));
		}

		public int DeleteIPAddress(int ipAddressId)
		{
			SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
			prmResult.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteIPAddress",
				 prmResult,
				 new SqlParameter("@AddressID", ipAddressId));

			return Convert.ToInt32(prmResult.Value);
		}



		#endregion

		#region Clusters
		public IDataReader GetClusters(int actorId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetClusters",
				 new SqlParameter("@actorId", actorId));
		}

		public int AddCluster(string clusterName)
		{
			SqlParameter prmId = new SqlParameter("@ClusterID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddCluster",
				 prmId,
				 new SqlParameter("@ClusterName", clusterName));

			return Convert.ToInt32(prmId.Value);
		}

		public void DeleteCluster(int clusterId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteCluster",
				 new SqlParameter("@ClusterId", clusterId));
		}

		#endregion

		#region Global DNS records
		public DataSet GetDnsRecordsByService(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByService",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServiceId", serviceId));
		}

		public DataSet GetDnsRecordsByServer(int actorId, int serverId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByServer",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServerId", serverId));
		}

		public DataSet GetDnsRecordsByPackage(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByPackage",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageId", packageId));
		}

		public DataSet GetDnsRecordsByGroup(int groupId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByGroup",
				 new SqlParameter("@GroupId", groupId));
		}

		public DataSet GetDnsRecordsTotal(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsTotal",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId));
		}

		public IDataReader GetDnsRecord(int actorId, int recordId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecord",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@RecordId", recordId));
		}

		public void AddDnsRecord(int actorId, int serviceId, int serverId, int packageId, string recordType,
			 string recordName, string recordData, int mxPriority, int SrvPriority, int SrvWeight, int SrvPort, int ipAddressId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddDnsRecord",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServiceId", serviceId),
				 new SqlParameter("@ServerId", serverId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@RecordType", recordType),
				 new SqlParameter("@RecordName", recordName),
				 new SqlParameter("@RecordData", recordData),
				 new SqlParameter("@MXPriority", mxPriority),
				 new SqlParameter("@SrvPriority", SrvPriority),
				 new SqlParameter("@SrvWeight", SrvWeight),
				 new SqlParameter("@SrvPort", SrvPort),
				 new SqlParameter("@IpAddressId", ipAddressId));
		}

		public void UpdateDnsRecord(int actorId, int recordId, string recordType,
			 string recordName, string recordData, int mxPriority, int SrvPriority, int SrvWeight, int SrvPort, int ipAddressId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateDnsRecord",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@RecordId", recordId),
				 new SqlParameter("@RecordType", recordType),
				 new SqlParameter("@RecordName", recordName),
				 new SqlParameter("@RecordData", recordData),
				 new SqlParameter("@MXPriority", mxPriority),
				 new SqlParameter("@SrvPriority", SrvPriority),
				 new SqlParameter("@SrvWeight", SrvWeight),
				 new SqlParameter("@SrvPort", SrvPort),
				 new SqlParameter("@IpAddressId", ipAddressId));
		}


		public void DeleteDnsRecord(int actorId, int recordId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteDnsRecord",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@RecordId", recordId));
		}
		#endregion

		#region Domains
		public DataSet GetDomains(int actorId, int packageId, bool recursive)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDomains",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@Recursive", recursive));
		}

		public DataSet GetResellerDomains(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetResellerDomains",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId));
		}

		public DataSet GetDomainsPaged(int actorId, int packageId, int serverId, bool recursive, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDomainsPaged",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@serverId", serverId),
				 new SqlParameter("@recursive", recursive),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@StartRow", startRow),
				 new SqlParameter("@MaximumRows", maximumRows));
		}

		public IDataReader GetDomain(int actorId, int domainId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDomain",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@domainId", domainId));
		}

		public IDataReader GetDomainByName(int actorId, string domainName, bool searchOnDomainPointer, bool isDomainPointer)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDomainByName",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@domainName", domainName),
				 new SqlParameter("@SearchOnDomainPointer", searchOnDomainPointer),
				 new SqlParameter("@IsDomainPointer", isDomainPointer));
		}


		public DataSet GetDomainsByZoneId(int actorId, int zoneId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDomainsByZoneID",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@ZoneID", zoneId));
		}

		public DataSet GetDomainsByDomainItemId(int actorId, int domainId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDomainsByDomainItemId",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@DomainID", domainId));
		}



		public int CheckDomain(int packageId, string domainName, bool isDomainPointer)
		{
			SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CheckDomain",
				 prmId,
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@domainName", domainName),
				 new SqlParameter("@isDomainPointer", isDomainPointer));

			return Convert.ToInt32(prmId.Value);
		}



		public int CheckDomainUsedByHostedOrganization(string domainName)
		{
			SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CheckDomainUsedByHostedOrganization",
				 prmId,
				 new SqlParameter("@domainName", domainName));

			return Convert.ToInt32(prmId.Value);
		}


		public int AddDomain(int actorId, int packageId, int zoneItemId, string domainName,
			 bool hostingAllowed, int webSiteId, int mailDomainId, bool isSubDomain, bool isPreviewDomain, bool isDomainPointer)
		{
			SqlParameter prmId = new SqlParameter("@DomainID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddDomain",
				 prmId,
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@ZoneItemId", zoneItemId),
				 new SqlParameter("@DomainName", domainName),
				 new SqlParameter("@HostingAllowed", hostingAllowed),
				 new SqlParameter("@WebSiteId", webSiteId),
				 new SqlParameter("@MailDomainId", mailDomainId),
				 new SqlParameter("@IsSubDomain", isSubDomain),
				 new SqlParameter("@IsPreviewDomain", isPreviewDomain),
				 new SqlParameter("@IsDomainPointer", isDomainPointer));

			return Convert.ToInt32(prmId.Value);
		}

		public void UpdateDomain(int actorId, int domainId, int zoneItemId,
			 bool hostingAllowed, int webSiteId, int mailDomainId, int domainItemId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateDomain",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@DomainId", domainId),
				 new SqlParameter("@ZoneItemId", zoneItemId),
				 new SqlParameter("@HostingAllowed", hostingAllowed),
				 new SqlParameter("@WebSiteId", webSiteId),
				 new SqlParameter("@MailDomainId", mailDomainId),
				 new SqlParameter("@DomainItemId", domainItemId));
		}

		public void DeleteDomain(int actorId, int domainId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteDomain",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@DomainId", domainId));
		}
		#endregion

		#region Services
		public IDataReader GetServicesByServerId(int actorId, int serverId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServicesByServerID",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServerID", serverId));
		}

		public IDataReader GetServicesByServerIdGroupName(int actorId, int serverId, string groupName)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServicesByServerIdGroupName",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@ServerID", serverId),
				 new SqlParameter("@GroupName", groupName));
		}

		public DataSet GetRawServicesByServerId(int actorId, int serverId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetRawServicesByServerID",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServerID", serverId));
		}

		public DataSet GetServicesByGroupId(int actorId, int groupId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServicesByGroupID",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@groupId", groupId));
		}

		public DataSet GetServicesByGroupName(int actorId, string groupName, bool forAutodiscover)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServicesByGroupName",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@GroupName", groupName),
				 new SqlParameter("@forAutodiscover", forAutodiscover));
		}

		public IDataReader GetService(int actorId, int serviceId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				 CommandType.StoredProcedure,
				 ObjectQualifier + "GetService",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServiceID", serviceId));
		}

		public int AddService(int serverId, int providerId, string serviceName, int serviceQuotaValue,
			 int clusterId, string comments)
		{
			SqlParameter prmServiceId = new SqlParameter("@ServiceID", SqlDbType.Int);
			prmServiceId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddService",
				 prmServiceId,
				 new SqlParameter("@ServerID", serverId),
				 new SqlParameter("@ProviderID", providerId),
				 new SqlParameter("@ServiceName", serviceName),
				 new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
				 new SqlParameter("@ClusterId", clusterId),
				 new SqlParameter("@comments", comments));

			UpdateServerPackageServices(serverId);

			return Convert.ToInt32(prmServiceId.Value);
		}
		public void UpdateServiceFully(int serviceId, int providerId, string serviceName, int serviceQuotaValue,
			 int clusterId, string comments)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateServiceFully",
				 new SqlParameter("@ProviderID", providerId),
				 new SqlParameter("@ServiceName", serviceName),
				 new SqlParameter("@ServiceID", serviceId),
				 new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
				 new SqlParameter("@ClusterId", clusterId),
				 new SqlParameter("@Comments", comments));
		}

		public void UpdateService(int serviceId, string serviceName, int serviceQuotaValue,
			 int clusterId, string comments)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateService",
				 new SqlParameter("@ServiceName", serviceName),
				 new SqlParameter("@ServiceID", serviceId),
				 new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
				 new SqlParameter("@ClusterId", clusterId),
				 new SqlParameter("@Comments", comments));
		}

		public int DeleteService(int serviceId)
		{
			SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
			prmResult.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteService",
				 prmResult,
				 new SqlParameter("@ServiceID", serviceId));

			return Convert.ToInt32(prmResult.Value);
		}

		public IDataReader GetServiceProperties(int actorId, int serviceId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				 CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceProperties",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServiceID", serviceId));
		}

		public void UpdateServiceProperties(int serviceId, string xml)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateServiceProperties",
				 new SqlParameter("@ServiceId", serviceId),
				 new SqlParameter("@Xml", xml));
		}

		public IDataReader GetResourceGroup(int groupId)
		{
			return SqlHelper.ExecuteReader(ConnectionString,
				 CommandType.StoredProcedure,
				 ObjectQualifier + "GetResourceGroup",
				 new SqlParameter("@groupId", groupId));
		}

		public DataSet GetResourceGroups()
		{
			return SqlHelper.ExecuteDataset(ConnectionString,
				 CommandType.StoredProcedure,
				 ObjectQualifier + "GetResourceGroups");
		}

		public IDataReader GetResourceGroupByName(string groupName)
		{
			return SqlHelper.ExecuteReader(ConnectionString,
				 CommandType.StoredProcedure,
				 ObjectQualifier + "GetResourceGroupByName",
				 new SqlParameter("@groupName", groupName));
		}

		#endregion

		#region Service Items
		public DataSet GetServiceItems(int actorId, int packageId, string groupName, string itemTypeName, bool recursive)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItems",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@GroupName", groupName),
				 new SqlParameter("@ItemTypeName", itemTypeName),
				 new SqlParameter("@Recursive", recursive));

		}

		public DataSet GetServiceItemsPaged(int actorId, int packageId, string groupName, string itemTypeName,
			 int serverId, bool recursive, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemsPaged",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@groupName", groupName),
				 new SqlParameter("@serverId", serverId),
				 new SqlParameter("@itemTypeName", itemTypeName),
				 new SqlParameter("@recursive", recursive),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetSearchableServiceItemTypes()
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSearchableServiceItemTypes");
		}

		public DataSet GetServiceItemsByService(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemsByService",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServiceID", serviceId));
		}

		public int GetServiceItemsCount(string typeName, string groupName, int serviceId)
		{
			SqlParameter prmTotalNumber = new SqlParameter("@TotalNumber", SqlDbType.Int);
			prmTotalNumber.Direction = ParameterDirection.Output;

			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemsCount",
				 prmTotalNumber,
				 new SqlParameter("@itemTypeName", typeName),
				 new SqlParameter("@groupName", groupName),
				 new SqlParameter("@serviceId", serviceId));

			// read identity
			return Convert.ToInt32(prmTotalNumber.Value);
		}

		public DataSet GetServiceItemsForStatistics(int actorId, int serviceId, int packageId,
			 bool calculateDiskspace, bool calculateBandwidth, bool suspendable, bool disposable)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemsForStatistics",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@ServiceID", serviceId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@calculateDiskspace", calculateDiskspace),
				 new SqlParameter("@calculateBandwidth", calculateBandwidth),
				 new SqlParameter("@suspendable", suspendable),
				 new SqlParameter("@disposable", disposable));
		}

		public DataSet GetServiceItemsByPackage(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemsByPackage",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		public DataSet GetServiceItem(int actorId, int itemId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItem",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@actorId", actorId));
		}

		public bool CheckServiceItemExists(int serviceId, string itemName, string itemTypeName)
		{
			SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
			prmExists.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CheckServiceItemExistsInService",
				 prmExists,
				 new SqlParameter("@serviceId", serviceId),
				 new SqlParameter("@itemName", itemName),
				 new SqlParameter("@itemTypeName", itemTypeName));

			return Convert.ToBoolean(prmExists.Value);
		}

		public bool CheckServiceItemExists(string itemName, string groupName, string itemTypeName)
		{
			SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
			prmExists.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CheckServiceItemExists",
				 prmExists,
				 new SqlParameter("@itemName", itemName),
				 new SqlParameter("@groupName", groupName),
				 new SqlParameter("@itemTypeName", itemTypeName));

			return Convert.ToBoolean(prmExists.Value);
		}

		public DataSet GetServiceItemByName(int actorId, int packageId, string groupName,
			 string itemName, string itemTypeName)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemByName",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@itemName", itemName),
				 new SqlParameter("@itemTypeName", itemTypeName),
				 new SqlParameter("@groupName", groupName));
		}

		public DataSet GetServiceItemsByName(int actorId, int packageId, string itemName)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemsByName",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@itemName", itemName));
		}

		public int GetServiceItemsCountByNameAndServiceId(int actorId, int serviceId, string groupName,
			 string itemName, string itemTypeName)
		{
			int res = 0;

			object obj = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetServiceItemsCountByNameAndServiceId",
				new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@ServiceId", serviceId),
				 new SqlParameter("@ItemName", itemName),
				 new SqlParameter("@GroupName", groupName),
				 new SqlParameter("@ItemTypeName", itemTypeName));

			if (!int.TryParse(obj.ToString(), out res)) return -1;

			return res;
		}

		public int AddServiceItem(int actorId, int serviceId, int packageId, string itemName,
			 string itemTypeName, string xmlProperties)
		{
			// add item
			SqlParameter prmItemId = new SqlParameter("@ItemID", SqlDbType.Int);
			prmItemId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddServiceItem",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@ServiceID", serviceId),
				 new SqlParameter("@ItemName", itemName),
				 new SqlParameter("@ItemTypeName", itemTypeName),
				 new SqlParameter("@xmlProperties", xmlProperties),
				 new SqlParameter("@CreatedDate", DateTime.Now),
				 prmItemId);

			return Convert.ToInt32(prmItemId.Value);
		}

		public void UpdateServiceItem(int actorId, int itemId, string itemName, string xmlProperties)
		{
			// update item
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateServiceItem",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ItemName", itemName),
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@XmlProperties", xmlProperties));
		}

		public void DeleteServiceItem(int actorId, int itemId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteServiceItem",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ItemID", itemId));
		}

		public void MoveServiceItem(int actorId, int itemId, int destinationServiceId, bool forAutodiscover)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "MoveServiceItem",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@DestinationServiceID", destinationServiceId),
				 new SqlParameter("@forAutodiscover", forAutodiscover));
		}

		public int GetPackageServiceId(int actorId, int packageId, string groupName)
		{
			SqlParameter prmServiceId = new SqlParameter("@ServiceID", SqlDbType.Int);
			prmServiceId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageServiceID",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@groupName", groupName),
				 prmServiceId);

			return Convert.ToInt32(prmServiceId.Value);
		}

		public string GetMailFilterURL(int actorId, int packageId, string groupName)
		{
			SqlParameter prmFilterUrl = new SqlParameter("@FilterUrl", SqlDbType.NVarChar, 200);
			prmFilterUrl.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetFilterURL",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@GroupName", groupName),
				 prmFilterUrl);

			return Convert.ToString(prmFilterUrl.Value);
		}

		public string GetMailFilterUrlByHostingPlan(int actorId, int PlanID, string groupName)
		{
			SqlParameter prmFilterUrl = new SqlParameter("@FilterUrl", SqlDbType.NVarChar, 200);
			prmFilterUrl.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetFilterURLByHostingPlan",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@PlanID", PlanID),
				 new SqlParameter("@GroupName", groupName),
				 prmFilterUrl);

			return Convert.ToString(prmFilterUrl.Value);
		}


		public void UpdatePackageDiskSpace(int packageId, string xml)
		{
			ExecuteLongNonQuery(
				 ObjectQualifier + "UpdatePackageDiskSpace",
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@xml", xml));
		}

		public void UpdatePackageBandwidth(int packageId, string xml)
		{
			ExecuteLongNonQuery(
				 ObjectQualifier + "UpdatePackageBandwidth",
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@xml", xml));
		}

		public DateTime GetPackageBandwidthUpdate(int packageId)
		{
			SqlParameter prmUpdateDate = new SqlParameter("@UpdateDate", SqlDbType.DateTime);
			prmUpdateDate.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageBandwidthUpdate",
				 prmUpdateDate,
				 new SqlParameter("@packageId", packageId));

			return (prmUpdateDate.Value != DBNull.Value) ? Convert.ToDateTime(prmUpdateDate.Value) : DateTime.MinValue;
		}

		public void UpdatePackageBandwidthUpdate(int packageId, DateTime updateDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdatePackageBandwidthUpdate",
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@updateDate", updateDate));
		}

		public IDataReader GetServiceItemType(int itemTypeId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetServiceItemType",
				 new SqlParameter("@ItemTypeID", itemTypeId)
			);
		}

		public IDataReader GetServiceItemTypes()
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetServiceItemTypes"
			);
		}
		#endregion

		#region Plans
		// Plans methods
		public DataSet GetHostingPlans(int actorId, int userId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetHostingPlans",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId));
		}

		public DataSet GetHostingAddons(int actorId, int userId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetHostingAddons",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId));
		}

		public DataSet GetUserAvailableHostingPlans(int actorId, int userId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetUserAvailableHostingPlans",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId));
		}

		public DataSet GetUserAvailableHostingAddons(int actorId, int userId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetUserAvailableHostingAddons",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId));
		}

		public IDataReader GetHostingPlan(int actorId, int planId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetHostingPlan",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PlanId", planId));
		}

		public DataSet GetHostingPlanQuotas(int actorId, int packageId, int planId, int serverId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetHostingPlanQuotas",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@planId", planId),
				 new SqlParameter("@serverId", serverId));
		}

		public int AddHostingPlan(int actorId, int userId, int packageId, string planName,
			 string planDescription, bool available, int serverId, decimal setupPrice, decimal recurringPrice,
			 int recurrenceUnit, int recurrenceLength, bool isAddon, string quotasXml)
		{
			SqlParameter prmPlanId = new SqlParameter("@PlanID", SqlDbType.Int);
			prmPlanId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddHostingPlan",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@planName", planName),
				 new SqlParameter("@planDescription", planDescription),
				 new SqlParameter("@available", available),
				 new SqlParameter("@serverId", serverId),
				 new SqlParameter("@setupPrice", setupPrice),
				 new SqlParameter("@recurringPrice", recurringPrice),
				 new SqlParameter("@recurrenceUnit", recurrenceUnit),
				 new SqlParameter("@recurrenceLength", recurrenceLength),
				 new SqlParameter("@isAddon", isAddon),
				 new SqlParameter("@quotasXml", quotasXml),
				 prmPlanId);

			// read identity
			return Convert.ToInt32(prmPlanId.Value);
		}

		public DataSet UpdateHostingPlan(int actorId, int planId, int packageId, int serverId, string planName,
			 string planDescription, bool available, decimal setupPrice, decimal recurringPrice,
			 int recurrenceUnit, int recurrenceLength, string quotasXml)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateHostingPlan",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@planId", planId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@serverId", serverId),
				 new SqlParameter("@planName", planName),
				 new SqlParameter("@planDescription", planDescription),
				 new SqlParameter("@available", available),
				 new SqlParameter("@setupPrice", setupPrice),
				 new SqlParameter("@recurringPrice", recurringPrice),
				 new SqlParameter("@recurrenceUnit", recurrenceUnit),
				 new SqlParameter("@recurrenceLength", recurrenceLength),
				 new SqlParameter("@quotasXml", quotasXml));
		}

		public int CopyHostingPlan(int planId, int userId, int packageId)
		{
			SqlParameter prmPlanId = new SqlParameter("@DestinationPlanID", SqlDbType.Int);
			prmPlanId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CopyHostingPlan",
				 new SqlParameter("@SourcePlanID", planId),
				 new SqlParameter("@UserID", userId),
				 new SqlParameter("@PackageID", packageId),
				 prmPlanId);

			return Convert.ToInt32(prmPlanId.Value);
		}

		public int DeleteHostingPlan(int actorId, int planId)
		{
			SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
			prmResult.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteHostingPlan",
				 prmResult,
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PlanId", planId));

			return Convert.ToInt32(prmResult.Value);
		}
		#endregion

		#region Packages

		// Packages
		public DataSet GetMyPackages(int actorId, int userId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetMyPackages",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@UserID", userId));
		}

		public DataSet GetPackages(int actorId, int userId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackages",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@UserID", userId));
		}

		public DataSet GetNestedPackagesSummary(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetNestedPackagesSummary",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		public DataSet SearchServiceItemsPaged(int actorId, int userId, int itemTypeId, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "SearchServiceItemsPaged",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@UserID", userId),
				 new SqlParameter("@itemTypeId", itemTypeId),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetPackagesPaged(int actorId, int userId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackagesPaged",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@UserID", userId),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetNestedPackagesPaged(int actorId, int packageId, string filterColumn, string filterValue,
			 int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetNestedPackagesPaged",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@statusId", statusId),
				 new SqlParameter("@planId", planId),
				 new SqlParameter("@serverId", serverId),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetPackagePackages(int actorId, int packageId, bool recursive)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackagePackages",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@recursive", recursive));
		}

		public IDataReader GetPackage(int actorId, int packageId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackage",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		public DataSet GetPackageQuotas(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageQuotas",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		public DataSet GetParentPackageQuotas(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetParentPackageQuotas",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		public DataSet GetPackageQuotasForEdit(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageQuotasForEdit",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		public DataSet AddPackage(int actorId, out int packageId, int userId, int planId, string packageName,
			 string packageComments, int statusId, DateTime purchaseDate)
		{
			SqlParameter prmPackageId = new SqlParameter("@PackageID", SqlDbType.Int);
			prmPackageId.Direction = ParameterDirection.Output;

			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddPackage",
				 prmPackageId,
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@userId", userId),
				 new SqlParameter("@packageName", packageName),
				 new SqlParameter("@packageComments", packageComments),
				 new SqlParameter("@statusId", statusId),
				 new SqlParameter("@planId", planId),
				 new SqlParameter("@purchaseDate", purchaseDate));

			// read identity
			packageId = Convert.ToInt32(prmPackageId.Value);

			DistributePackageServices(actorId, packageId);

			return ds;
		}

		public DataSet UpdatePackage(int actorId, int packageId, int planId, string packageName,
			 string packageComments, int statusId, DateTime purchaseDate,
			 bool overrideQuotas, string quotasXml, bool defaultTopPackage)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdatePackage",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@packageName", packageName),
				 new SqlParameter("@packageComments", packageComments),
				 new SqlParameter("@statusId", statusId),
				 new SqlParameter("@planId", planId),
				 new SqlParameter("@purchaseDate", purchaseDate),
				 new SqlParameter("@overrideQuotas", overrideQuotas),
				 new SqlParameter("@quotasXml", quotasXml),
				 new SqlParameter("@defaultTopPackage", defaultTopPackage));
		}
		public void ChangePackageUser(int actorId, int packageId, int userId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "ChangePackageUser",
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@UserId", userId));
		}

		public void UpdatePackageName(int actorId, int packageId, string packageName,
			 string packageComments)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdatePackageName",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@packageName", packageName),
				 new SqlParameter("@packageComments", packageComments));
		}

		public void DeletePackage(int actorId, int packageId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeletePackage",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		// Package Add-ons
		public DataSet GetPackageAddons(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageAddons",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		public IDataReader GetPackageAddon(int actorId, int packageAddonId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageAddon",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageAddonID", packageAddonId));
		}

		public DataSet AddPackageAddon(int actorId, out int addonId, int packageId, int planId, int quantity,
			 int statusId, DateTime purchaseDate, string comments)
		{
			SqlParameter prmPackageAddonId = new SqlParameter("@PackageAddonID", SqlDbType.Int);
			prmPackageAddonId.Direction = ParameterDirection.Output;

			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddPackageAddon",
				 prmPackageAddonId,
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@planId", planId),
				 new SqlParameter("@Quantity", quantity),
				 new SqlParameter("@statusId", statusId),
				 new SqlParameter("@PurchaseDate", purchaseDate),
				 new SqlParameter("@Comments", comments));

			// read identity
			addonId = Convert.ToInt32(prmPackageAddonId.Value);

			return ds;
		}

		public DataSet UpdatePackageAddon(int actorId, int packageAddonId, int planId, int quantity,
			 int statusId, DateTime purchaseDate, string comments)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdatePackageAddon",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageAddonID", packageAddonId),
				 new SqlParameter("@planId", planId),
				 new SqlParameter("@Quantity", quantity),
				 new SqlParameter("@statusId", statusId),
				 new SqlParameter("@PurchaseDate", purchaseDate),
				 new SqlParameter("@Comments", comments));
		}

		public void DeletePackageAddon(int actorId, int packageAddonId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeletePackageAddon",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageAddonID", packageAddonId));
		}

		public void UpdateServerPackageServices(int serverId)
		{
			// FIXME
			int defaultActorID = 1;

			// get server packages
			IDataReader packagesReader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT PackageID FROM Packages WHERE ServerID = @ServerID",
				 new SqlParameter("@ServerID", serverId)
			);

			// call DistributePackageServices for all packages on this server
			while (packagesReader.Read())
			{
				int packageId = (int)packagesReader["PackageID"];
				DistributePackageServices(defaultActorID, packageId);
			}
		}

		public void DistributePackageServices(int actorId, int packageId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DistributePackageServices",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageID", packageId));
		}

		#endregion

		#region Packages Settings
		public IDataReader GetPackageSettings(int actorId, int packageId, string settingsName)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageSettings",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@SettingsName", settingsName));
		}
		public void UpdatePackageSettings(int actorId, int packageId, string settingsName, string xml)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdatePackageSettings",
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@SettingsName", settingsName),
				 new SqlParameter("@Xml", xml));
		}
		#endregion

		#region Quotas
		public IDataReader GetProviderServiceQuota(int providerId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetProviderServiceQuota",
				 new SqlParameter("@providerId", providerId));
		}

		public IDataReader GetPackageQuota(int actorId, int packageId, string quotaName)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPackageQuota",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@QuotaName", quotaName));
		}
		#endregion

		#region Log
		public void AddAuditLogRecord(string recordId, int severityId,
			 int userId, string username, int packageId, int itemId, string itemName, DateTime startDate, DateTime finishDate, string sourceName,
			 string taskName, string executionLog)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddAuditLogRecord",
				 new SqlParameter("@recordId", recordId),
				 new SqlParameter("@severityId", severityId),
				 new SqlParameter("@UserID", userId),
				 new SqlParameter("@username", username),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@itemName", itemName),
				 new SqlParameter("@startDate", startDate),
				 new SqlParameter("@finishDate", finishDate),
				 new SqlParameter("@sourceName", sourceName),
				 new SqlParameter("@taskName", taskName),
				 new SqlParameter("@executionLog", executionLog));
		}

		public DataSet GetAuditLogRecordsPaged(int actorId, int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate,
			 int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetAuditLogRecordsPaged",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@UserID", userId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@itemId", itemId),
				 new SqlParameter("@itemName", itemName),
				 new SqlParameter("@StartDate", startDate),
				 new SqlParameter("@EndDate", endDate),
				 new SqlParameter("@severityId", severityId),
				 new SqlParameter("@sourceName", sourceName),
				 new SqlParameter("@taskName", taskName),
				 new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetAuditLogSources()
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetAuditLogSources");
		}

		public DataSet GetAuditLogTasks(string sourceName)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetAuditLogTasks",
				 new SqlParameter("@sourceName", sourceName));
		}

		public IDataReader GetAuditLogRecord(string recordId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetAuditLogRecord",
				 new SqlParameter("@recordId", recordId));
		}

		public void DeleteAuditLogRecords(int actorId, int userId, int itemId, string itemName, DateTime startDate, DateTime endDate,
			 int severityId, string sourceName, string taskName)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteAuditLogRecords",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId),
				 new SqlParameter("@itemId", itemId),
				 new SqlParameter("@itemName", itemName),
				 new SqlParameter("@startDate", startDate),
				 new SqlParameter("@endDate", endDate),
				 new SqlParameter("@severityId", severityId),
				 new SqlParameter("@sourceName", sourceName),
				 new SqlParameter("@taskName", taskName));
		}

		public void DeleteAuditLogRecordsComplete()
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteAuditLogRecordsComplete");
		}

		#endregion

		#region Reports
		public DataSet GetPackagesBandwidthPaged(int actorId, int userId, int packageId,
			 DateTime startDate, DateTime endDate, string sortColumn,
			 int startRow, int maximumRows)
		{
			return ExecuteLongDataSet(
				 ObjectQualifier + "GetPackagesBandwidthPaged",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@StartDate", startDate),
				 new SqlParameter("@EndDate", endDate),
				 new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetPackagesDiskspacePaged(int actorId, int userId, int packageId, string sortColumn,
			 int startRow, int maximumRows)
		{
			return ExecuteLongDataSet(
				 ObjectQualifier + "GetPackagesDiskspacePaged",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetPackageBandwidth(int actorId, int packageId, DateTime startDate, DateTime endDate)
		{
			return ExecuteLongDataSet(
				 ObjectQualifier + "GetPackageBandwidth",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@StartDate", startDate),
				 new SqlParameter("@EndDate", endDate));
		}

		public DataSet GetPackageDiskspace(int actorId, int packageId)
		{
			return ExecuteLongDataSet(
				 ObjectQualifier + "GetPackageDiskspace",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageId", packageId));
		}

		#endregion

		#region Scheduler

		public IDataReader GetBackgroundTask(string taskId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetBackgroundTask",
													 new SqlParameter("@taskId", taskId));
		}

		public IDataReader GetScheduleBackgroundTasks(int scheduleId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetScheduleBackgroundTasks",
													 new SqlParameter("@scheduleId", scheduleId));
		}

		public IDataReader GetBackgroundTasks(int actorId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetBackgroundTasks",
													 new SqlParameter("@actorId", actorId));
		}

		public IDataReader GetBackgroundTasks(Guid guid)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetThreadBackgroundTasks",
													 new SqlParameter("@guid", guid));
		}

		public IDataReader GetProcessBackgroundTasks(BackgroundTaskStatus status)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetProcessBackgroundTasks",
													 new SqlParameter("@status", (int)status));
		}

		public IDataReader GetBackgroundTopTask(Guid guid)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetBackgroundTopTask",
													 new SqlParameter("@guid", guid));
		}

		public int AddBackgroundTask(Guid guid, string taskId, int scheduleId, int packageId, int userId,
			 int effectiveUserId, string taskName, int itemId, string itemName, DateTime startDate,
			 int indicatorCurrent, int indicatorMaximum, int maximumExecutionTime, string source,
			 int severity, bool completed, bool notifyOnComplete, BackgroundTaskStatus status)
		{
			SqlParameter prmId = new SqlParameter("@BackgroundTaskID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "AddBackgroundTask",
											  prmId,
											  new SqlParameter("@guid", guid),
											  new SqlParameter("@taskId", taskId),
											  new SqlParameter("@scheduleId", scheduleId),
											  new SqlParameter("@packageId", packageId),
											  new SqlParameter("@userId", userId),
											  new SqlParameter("@effectiveUserId", effectiveUserId),
											  new SqlParameter("@taskName", taskName),
											  new SqlParameter("@itemId", itemId),
											  new SqlParameter("@itemName", itemName),
											  new SqlParameter("@startDate", startDate),
											  new SqlParameter("@indicatorCurrent", indicatorCurrent),
											  new SqlParameter("@indicatorMaximum", indicatorMaximum),
											  new SqlParameter("@maximumExecutionTime", maximumExecutionTime),
											  new SqlParameter("@source", source),
											  new SqlParameter("@severity", severity),
											  new SqlParameter("@completed", completed),
											  new SqlParameter("@notifyOnComplete", notifyOnComplete),
											  new SqlParameter("@status", status));

			// read identity
			return Convert.ToInt32(prmId.Value);
		}

		public void AddBackgroundTaskLog(int taskId, DateTime date, string exceptionStackTrace,
			 bool innerTaskStart, int severity, string text, int textIdent, string xmlParameters)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "AddBackgroundTaskLog",
											  new SqlParameter("@taskId", taskId),
											  new SqlParameter("@date", date),
											  new SqlParameter("@exceptionStackTrace", exceptionStackTrace),
											  new SqlParameter("@innerTaskStart", innerTaskStart),
											  new SqlParameter("@severity", severity),
											  new SqlParameter("@text", text),
											  new SqlParameter("@textIdent", textIdent),
											  new SqlParameter("@xmlParameters", xmlParameters));
		}

		public IDataReader GetBackgroundTaskLogs(int taskId, DateTime startLogTime)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetBackgroundTaskLogs",
													 new SqlParameter("@taskId", taskId),
													 new SqlParameter("@startLogTime", startLogTime));
		}

		public void UpdateBackgroundTask(Guid guid, int taskId, int scheduleId, int packageId, string taskName, int itemId,
			 string itemName, DateTime finishDate, int indicatorCurrent, int indicatorMaximum, int maximumExecutionTime,
			 string source, int severity, bool completed, bool notifyOnComplete, BackgroundTaskStatus status)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "UpdateBackgroundTask",
											  new SqlParameter("@Guid", guid),
											  new SqlParameter("@taskId", taskId),
											  new SqlParameter("@scheduleId", scheduleId),
											  new SqlParameter("@packageId", packageId),
											  new SqlParameter("@taskName", taskName),
											  new SqlParameter("@itemId", itemId),
											  new SqlParameter("@itemName", itemName),
											  new SqlParameter("@finishDate",
																	 finishDate == DateTime.MinValue
																		  ? DBNull.Value
																		  : (object)finishDate),
											  new SqlParameter("@indicatorCurrent", indicatorCurrent),
											  new SqlParameter("@indicatorMaximum", indicatorMaximum),
											  new SqlParameter("@maximumExecutionTime", maximumExecutionTime),
											  new SqlParameter("@source", source),
											  new SqlParameter("@severity", severity),
											  new SqlParameter("@completed", completed),
											  new SqlParameter("@notifyOnComplete", notifyOnComplete),
											  new SqlParameter("@status", (int)status));

		}

		public IDataReader GetBackgroundTaskParams(int taskId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetBackgroundTaskParams",
													 new SqlParameter("@taskId", taskId));
		}

		public void AddBackgroundTaskParam(int taskId, string name, string value, string typeName)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "AddBackgroundTaskParam",
											  new SqlParameter("@taskId", taskId),
											  new SqlParameter("@name", name),
											  new SqlParameter("@value", value),
											  new SqlParameter("@typeName", typeName));
		}

		public void DeleteBackgroundTaskParams(int taskId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "DeleteBackgroundTaskParams",
											  new SqlParameter("@taskId", taskId));
		}

		public void AddBackgroundTaskStack(int taskId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "AddBackgroundTaskStack",
											  new SqlParameter("@taskId", taskId));
		}

		public void DeleteBackgroundTasks(Guid guid)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "DeleteBackgroundTasks",
											  new SqlParameter("@guid", guid));
		}

		public void DeleteBackgroundTask(int taskId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
											  ObjectQualifier + "DeleteBackgroundTask",
											  new SqlParameter("@id", taskId));
		}

		public IDataReader GetScheduleTasks(int actorId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetScheduleTasks",
				 new SqlParameter("@actorId", actorId));
		}

		public IDataReader GetScheduleTask(int actorId, string taskId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetScheduleTask",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@taskId", taskId));
		}

		public DataSet GetSchedules(int actorId, int packageId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSchedules",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@recursive", true));
		}

		public DataSet GetSchedulesPaged(int actorId, int packageId, bool recursive,
			 string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSchedulesPaged",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@recursive", recursive),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows));
		}

		public DataSet GetSchedule(int actorId, int scheduleId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSchedule",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleId", scheduleId));
		}
		public IDataReader GetScheduleInternal(int scheduleId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetScheduleInternal",
				 new SqlParameter("@scheduleId", scheduleId));
		}
		public DataSet GetNextSchedule()
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetNextSchedule");
		}
		public IDataReader GetScheduleParameters(int actorId, string taskId, int scheduleId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetScheduleParameters",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@taskId", taskId),
				 new SqlParameter("@scheduleId", scheduleId));
		}

		/// <summary>
		/// Loads view configuration for the task with specified id.
		/// </summary>
		/// <param name="taskId">Task id which points to task for which view configuration will be loaded.</param>
		/// <returns>View configuration for the task with supplied id.</returns>
		public IDataReader GetScheduleTaskViewConfigurations(string taskId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
													 ObjectQualifier + "GetScheduleTaskViewConfigurations",
													 new SqlParameter("@taskId", taskId));
		}

		public int AddSchedule(int actorId, string taskId, int packageId,
			 string scheduleName, string scheduleTypeId, int interval,
			 DateTime fromTime, DateTime toTime, DateTime startTime,
			 DateTime nextRun, bool enabled, string priorityId, int historiesNumber,
			 int maxExecutionTime, int weekMonthDay, string xmlParameters)
		{
			SqlParameter prmId = new SqlParameter("@ScheduleID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddSchedule",
				 prmId,
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@taskId", taskId),
				 new SqlParameter("@packageId", packageId),
				 new SqlParameter("@scheduleName", scheduleName),
				 new SqlParameter("@scheduleTypeId", scheduleTypeId),
				 new SqlParameter("@interval", interval),
				 new SqlParameter("@fromTime", fromTime),
				 new SqlParameter("@toTime", toTime),
				 new SqlParameter("@startTime", startTime),
				 new SqlParameter("@nextRun", nextRun),
				 new SqlParameter("@enabled", enabled),
				 new SqlParameter("@priorityId", priorityId),
				 new SqlParameter("@historiesNumber", historiesNumber),
				 new SqlParameter("@maxExecutionTime", maxExecutionTime),
				 new SqlParameter("@weekMonthDay", weekMonthDay),
				 new SqlParameter("@xmlParameters", xmlParameters));

			// read identity
			return Convert.ToInt32(prmId.Value);
		}

		public void UpdateSchedule(int actorId, int scheduleId, string taskId,
			 string scheduleName, string scheduleTypeId, int interval,
			 DateTime fromTime, DateTime toTime, DateTime startTime,
			 DateTime lastRun, DateTime nextRun, bool enabled, string priorityId,
			 int historiesNumber, int maxExecutionTime, int weekMonthDay, string xmlParameters)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateSchedule",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleId", scheduleId),
				 new SqlParameter("@taskId", taskId),
				 new SqlParameter("@scheduleName", scheduleName),
				 new SqlParameter("@scheduleTypeId", scheduleTypeId),
				 new SqlParameter("@interval", interval),
				 new SqlParameter("@fromTime", fromTime),
				 new SqlParameter("@toTime", toTime),
				 new SqlParameter("@startTime", startTime),
				 new SqlParameter("@lastRun", (lastRun == DateTime.MinValue) ? DBNull.Value : (object)lastRun),
				 new SqlParameter("@nextRun", nextRun),
				 new SqlParameter("@enabled", enabled),
				 new SqlParameter("@priorityId", priorityId),
				 new SqlParameter("@historiesNumber", historiesNumber),
				 new SqlParameter("@maxExecutionTime", maxExecutionTime),
				 new SqlParameter("@weekMonthDay", weekMonthDay),
				 new SqlParameter("@xmlParameters", xmlParameters));
		}

		public void DeleteSchedule(int actorId, int scheduleId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteSchedule",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleId", scheduleId));
		}

		public DataSet GetScheduleHistories(int actorId, int scheduleId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetScheduleHistories",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleId", scheduleId));
		}
		public IDataReader GetScheduleHistory(int actorId, int scheduleHistoryId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetScheduleHistory",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleHistoryId", scheduleHistoryId));
		}
		public int AddScheduleHistory(int actorId, int scheduleId,
			 DateTime startTime, DateTime finishTime, string statusId, string executionLog)
		{
			SqlParameter prmId = new SqlParameter("@ScheduleHistoryID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddScheduleHistory",
				 prmId,
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleId", scheduleId),
				 new SqlParameter("@startTime", (startTime == DateTime.MinValue) ? DBNull.Value : (object)startTime),
				 new SqlParameter("@finishTime", (finishTime == DateTime.MinValue) ? DBNull.Value : (object)finishTime),
				 new SqlParameter("@statusId", statusId),
				 new SqlParameter("@executionLog", executionLog));

			// read identity
			return Convert.ToInt32(prmId.Value);
		}
		public void UpdateScheduleHistory(int actorId, int scheduleHistoryId,
			 DateTime startTime, DateTime finishTime, string statusId, string executionLog)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateScheduleHistory",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleHistoryId", scheduleHistoryId),
				 new SqlParameter("@startTime", (startTime == DateTime.MinValue) ? DBNull.Value : (object)startTime),
				 new SqlParameter("@finishTime", (finishTime == DateTime.MinValue) ? DBNull.Value : (object)finishTime),
				 new SqlParameter("@statusId", statusId),
				 new SqlParameter("@executionLog", executionLog));
		}
		public void DeleteScheduleHistories(int actorId, int scheduleId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteScheduleHistories",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@scheduleId", scheduleId));
		}
		#endregion

		#region Comments
		public DataSet GetComments(int actorId, int userId, string itemTypeId, int itemId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetComments",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@userId", userId),
				 new SqlParameter("@itemTypeId", itemTypeId),
				 new SqlParameter("@itemId", itemId));
		}

		public void AddComment(int actorId, string itemTypeId, int itemId,
			 string commentText, int severityId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddComment",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@itemTypeId", itemTypeId),
				 new SqlParameter("@itemId", itemId),
				 new SqlParameter("@commentText", commentText),
				 new SqlParameter("@severityId", severityId));
		}

		public void DeleteComment(int actorId, int commentId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteComment",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@commentId", commentId));
		}
		#endregion

		#region Helper Methods
		private string VerifyColumnName(string str)
		{
			if (str == null)
				str = "";
			return Regex.Replace(str, @"[^\w\. ]", "");
		}

		private string VerifyColumnValue(string str)
		{
			return String.IsNullOrEmpty(str) ? str : str.Replace("'", "''");
		}

		private DataSet ExecuteLongDataSet(string spName, params SqlParameter[] parameters)
		{
			return ExecuteLongDataSet(spName, CommandType.StoredProcedure, parameters);
		}

		private DataSet ExecuteLongQueryDataSet(string spName, params SqlParameter[] parameters)
		{
			return ExecuteLongDataSet(spName, CommandType.Text, parameters);
		}

		private DataSet ExecuteLongDataSet(string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand cmd = new SqlCommand(commandText, conn);
			cmd.CommandType = commandType;
			cmd.CommandTimeout = 300;

			if (parameters != null)
			{
				foreach (SqlParameter prm in parameters)
				{
					cmd.Parameters.Add(prm);
				}
			}

			DataSet ds = new DataSet();
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(ds);
			}
			finally
			{
				if (conn.State == ConnectionState.Open)
					conn.Close();
			}

			return ds;
		}

		private void ExecuteLongNonQuery(string spName, params SqlParameter[] parameters)
		{
			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand cmd = new SqlCommand(spName, conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 300;

			if (parameters != null)
			{
				foreach (SqlParameter prm in parameters)
				{
					cmd.Parameters.Add(prm);
				}
			}

			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
			}
			finally
			{
				if (conn.State == ConnectionState.Open)
					conn.Close();
			}
		}
		#endregion

		#region Exchange Server

		public int AddExchangeAccount(int itemId, int accountType, string accountName,
			 string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
			 string mailboxManagerActions, string samAccountName, int mailboxPlanId, string subscriberNumber)
		{
			SqlParameter outParam = new SqlParameter("@AccountID", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeAccount",
				 outParam,
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountType", accountType),
				 new SqlParameter("@AccountName", accountName),
				 new SqlParameter("@DisplayName", displayName),
				 new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress),
				 new SqlParameter("@MailEnabledPublicFolder", mailEnabledPublicFolder),
				 new SqlParameter("@MailboxManagerActions", mailboxManagerActions),
				 new SqlParameter("@SamAccountName", samAccountName),
				 new SqlParameter("@MailboxPlanId", (mailboxPlanId == 0) ? (object)DBNull.Value : (object)mailboxPlanId),
				 new SqlParameter("@SubscriberNumber", (string.IsNullOrEmpty(subscriberNumber) ? (object)DBNull.Value : (object)subscriberNumber))
			);

			return Convert.ToInt32(outParam.Value);
		}

		public void AddExchangeAccountEmailAddress(int accountId, string emailAddress)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeAccountEmailAddress",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@EmailAddress", emailAddress)
			);
		}

		public void AddExchangeOrganization(int itemId, string organizationId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeOrganization",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@OrganizationID", organizationId)
			);
		}

		public void AddExchangeOrganizationDomain(int itemId, int domainId, bool isHost)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeOrganizationDomain",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@DomainID", domainId),
				 new SqlParameter("@IsHost", isHost)
			);
		}

		public void ChangeExchangeAcceptedDomainType(int itemId, int domainId, int domainTypeId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "ChangeExchangeAcceptedDomainType",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@DomainID", domainId),
				 new SqlParameter("@DomainTypeID", domainTypeId)
			);
		}

		public IDataReader GetExchangeOrganizationStatistics(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeOrganizationStatistics",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public void DeleteUserEmailAddresses(int accountId, string primaryAddress)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteUserEmailAddresses",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@PrimaryEmailAddress", primaryAddress)
			);
		}

		public void DeleteExchangeAccount(int itemId, int accountId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeAccount",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountID", accountId)
			);
		}


		public void DeleteExchangeAccountEmailAddress(int accountId, string emailAddress)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeAccountEmailAddress",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@EmailAddress", emailAddress)
			);
		}

		public void DeleteExchangeOrganization(int itemId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeOrganization",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public void DeleteExchangeOrganizationDomain(int itemId, int domainId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeOrganizationDomain",
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@DomainID", domainId)
			);
		}

		public bool ExchangeAccountEmailAddressExists(string emailAddress, bool checkContacts)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "ExchangeAccountEmailAddressExists",
				 new SqlParameter("@EmailAddress", emailAddress),
				 new SqlParameter("@checkContacts", checkContacts),
				 outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public bool ExchangeOrganizationDomainExists(int domainId)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "ExchangeOrganizationDomainExists",
				 new SqlParameter("@DomainID", domainId),
				 outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public bool ExchangeOrganizationExists(string organizationId)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "ExchangeOrganizationExists",
				 new SqlParameter("@OrganizationID", organizationId),
				 outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public bool ExchangeAccountExists(string accountName)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "ExchangeAccountExists",
				 new SqlParameter("@AccountName", accountName),
				 outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public void UpdateExchangeAccount(int accountId, string accountName, ExchangeAccountType accountType,
			 string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
			 string mailboxManagerActions, string samAccountName, int mailboxPlanId, int archivePlanId, string subscriberNumber,
			 bool EnableArchiving)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateExchangeAccount",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@AccountName", accountName),
				 new SqlParameter("@DisplayName", displayName),
				 new SqlParameter("@AccountType", (int)accountType),
				 new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress),
				 new SqlParameter("@MailEnabledPublicFolder", mailEnabledPublicFolder),
				 new SqlParameter("@MailboxManagerActions", mailboxManagerActions),
				 new SqlParameter("@SamAccountName", samAccountName),
				 new SqlParameter("@MailboxPlanId", (mailboxPlanId == 0) ? (object)DBNull.Value : (object)mailboxPlanId),
				 new SqlParameter("@ArchivingMailboxPlanId", (archivePlanId < 1) ? (object)DBNull.Value : (object)archivePlanId),
				 new SqlParameter("@SubscriberNumber", (string.IsNullOrEmpty(subscriberNumber) ? (object)DBNull.Value : (object)subscriberNumber)),
				 new SqlParameter("@EnableArchiving", EnableArchiving)
			);
		}

		public void UpdateExchangeAccountServiceLevelSettings(int accountId, int levelId, bool isVIP)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateExchangeAccountSLSettings",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@LevelID", (levelId == 0) ? (object)DBNull.Value : (object)levelId),
				 new SqlParameter("@IsVIP", isVIP)
			);
		}

		public void UpdateExchangeAccountUserPrincipalName(int accountId, string userPrincipalName)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateExchangeAccountUserPrincipalName",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@UserPrincipalName", userPrincipalName));
		}

		public IDataReader GetExchangeAccount(int itemId, int accountId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeAccount",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountID", accountId)
			);
		}

		public IDataReader GetExchangeAccountByAccountName(int itemId, string accountName)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeAccountByAccountName",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountName", accountName)
			);
		}

		public IDataReader GetExchangeAccountByMailboxPlanId(int itemId, int MailboxPlanId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeAccountByMailboxPlanId",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@MailboxPlanId", MailboxPlanId)
			);
		}


		public IDataReader GetExchangeAccountEmailAddresses(int accountId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeAccountEmailAddresses",
				 new SqlParameter("@AccountID", accountId)
			);
		}

		public IDataReader GetExchangeOrganizationDomains(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeOrganizationDomains",
				 new SqlParameter("@ItemID", itemId)
			);
		}


		public IDataReader GetExchangeAccounts(int itemId, int accountType)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeAccounts",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountType", accountType)
			);
		}

		public IDataReader GetExchangeAccountByAccountNameWithoutItemId(string userPrincipalName)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeAccountByAccountNameWithoutItemId",
				 new SqlParameter("@UserPrincipalName", userPrincipalName)
			);
		}


		public IDataReader GetExchangeMailboxes(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeMailboxes",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public IDataReader SearchExchangeAccountsByTypes(int actorId, int itemId, string accountTypes,
			 string filterColumn, string filterValue, string sortColumn)
		{
			// check input parameters
			string[] types = accountTypes.Split(',');
			for (int i = 0; i < types.Length; i++)
			{
				try
				{
					int type = Int32.Parse(types[i]);
				}
				catch
				{
					throw new ArgumentException("Wrong patameter", "accountTypes");
				}
			}

			string searchTypes = String.Join(",", types);

			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SearchExchangeAccountsByTypes",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountTypes", searchTypes),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn))
			);
		}

		public DataSet GetExchangeAccountsPaged(int actorId, int itemId, string accountTypes,
				  string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving)
		{
			// check input parameters
			string[] types = accountTypes.Split(',');
			for (int i = 0; i < types.Length; i++)
			{
				try
				{
					int type = Int32.Parse(types[i]);
				}
				catch
				{
					throw new ArgumentException("Wrong patameter", "accountTypes");
				}
			}

			string searchTypes = String.Join(",", types);

			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeAccountsPaged",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountTypes", searchTypes),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@StartRow", startRow),
				 new SqlParameter("@MaximumRows", maximumRows),
				 new SqlParameter("@Archiving", archiving)
			);
		}

		public IDataReader SearchExchangeAccounts(int actorId, int itemId, bool includeMailboxes,
				  bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox,
				  bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SearchExchangeAccounts",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@IncludeMailboxes", includeMailboxes),
				 new SqlParameter("@IncludeContacts", includeContacts),
				 new SqlParameter("@IncludeDistributionLists", includeDistributionLists),
				 new SqlParameter("@IncludeRooms", includeRooms),
				 new SqlParameter("@IncludeEquipment", includeEquipment),
				 new SqlParameter("@IncludeSharedMailbox", IncludeSharedMailbox),
				 new SqlParameter("@IncludeSecurityGroups", includeSecurityGroups),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn))
			);
		}

		public IDataReader SearchExchangeAccount(int actorId, int accountType, string primaryEmailAddress)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SearchExchangeAccount",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@AccountType", accountType),
				 new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress)
			);
		}

		#endregion

		#region Exchange Mailbox Plans
		public int AddExchangeMailboxPlan(int itemID, string mailboxPlan, bool enableActiveSync, bool enableIMAP, bool enableMAPI, bool enableOWA, bool enablePOP, bool enableAutoReply,
																  bool isDefault, int issueWarningPct, int keepDeletedItemsDays, int mailboxSizeMB, int maxReceiveMessageSizeKB, int maxRecipients,
																  int maxSendMessageSizeKB, int prohibitSendPct, int prohibitSendReceivePct, bool hideFromAddressBook, int mailboxPlanType,
																  bool enabledLitigationHold, int recoverabelItemsSpace, int recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg,
																  bool archiving, bool EnableArchiving, int ArchiveSizeMB, int ArchiveWarningPct, bool enableForceArchiveDeletion, bool isForJournaling)
		{
			SqlParameter outParam = new SqlParameter("@MailboxPlanId", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeMailboxPlan",
				 outParam,
				 new SqlParameter("@ItemID", itemID),
				 new SqlParameter("@MailboxPlan", mailboxPlan),
				 new SqlParameter("@EnableActiveSync", enableActiveSync),
				 new SqlParameter("@EnableIMAP", enableIMAP),
				 new SqlParameter("@EnableMAPI", enableMAPI),
				 new SqlParameter("@EnableOWA", enableOWA),
				 new SqlParameter("@EnablePOP", enablePOP),
				 new SqlParameter("@EnableAutoReply", enableAutoReply),
				 new SqlParameter("@IsDefault", isDefault),
				 new SqlParameter("@IssueWarningPct", issueWarningPct),
				 new SqlParameter("@KeepDeletedItemsDays", keepDeletedItemsDays),
				 new SqlParameter("@MailboxSizeMB", mailboxSizeMB),
				 new SqlParameter("@MaxReceiveMessageSizeKB", maxReceiveMessageSizeKB),
				 new SqlParameter("@MaxRecipients", maxRecipients),
				 new SqlParameter("@MaxSendMessageSizeKB", maxSendMessageSizeKB),
				 new SqlParameter("@ProhibitSendPct", prohibitSendPct),
				 new SqlParameter("@ProhibitSendReceivePct", prohibitSendReceivePct),
				 new SqlParameter("@HideFromAddressBook", hideFromAddressBook),
				 new SqlParameter("@MailboxPlanType", mailboxPlanType),
				 new SqlParameter("@AllowLitigationHold", enabledLitigationHold),
				 new SqlParameter("@RecoverableItemsWarningPct", recoverabelItemsWarning),
				 new SqlParameter("@RecoverableItemsSpace", recoverabelItemsSpace),
				 new SqlParameter("@LitigationHoldUrl", litigationHoldUrl),
				 new SqlParameter("@LitigationHoldMsg", litigationHoldMsg),
				 new SqlParameter("@Archiving", archiving),
				 new SqlParameter("@EnableArchiving", EnableArchiving),
				 new SqlParameter("@ArchiveSizeMB", ArchiveSizeMB),
				 new SqlParameter("@ArchiveWarningPct", ArchiveWarningPct),
				 new SqlParameter("@EnableForceArchiveDeletion", enableForceArchiveDeletion),
				 new SqlParameter("@IsForJournaling", isForJournaling)
			);

			return Convert.ToInt32(outParam.Value);
		}



		public void UpdateExchangeMailboxPlan(int mailboxPlanID, string mailboxPlan, bool enableActiveSync, bool enableIMAP, bool enableMAPI, bool enableOWA, bool enablePOP, bool enableAutoReply,
														bool isDefault, int issueWarningPct, int keepDeletedItemsDays, int mailboxSizeMB, int maxReceiveMessageSizeKB, int maxRecipients,
														int maxSendMessageSizeKB, int prohibitSendPct, int prohibitSendReceivePct, bool hideFromAddressBook, int mailboxPlanType,
														bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg,
														bool Archiving, bool EnableArchiving, int ArchiveSizeMB, int ArchiveWarningPct, bool enableForceArchiveDeletion, bool isForJournaling)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateExchangeMailboxPlan",
				 new SqlParameter("@MailboxPlanID", mailboxPlanID),
				 new SqlParameter("@MailboxPlan", mailboxPlan),
				 new SqlParameter("@EnableActiveSync", enableActiveSync),
				 new SqlParameter("@EnableIMAP", enableIMAP),
				 new SqlParameter("@EnableMAPI", enableMAPI),
				 new SqlParameter("@EnableOWA", enableOWA),
				 new SqlParameter("@EnablePOP", enablePOP),
				 new SqlParameter("@EnableAutoReply", enableAutoReply),
				 new SqlParameter("@IsDefault", isDefault),
				 new SqlParameter("@IssueWarningPct", issueWarningPct),
				 new SqlParameter("@KeepDeletedItemsDays", keepDeletedItemsDays),
				 new SqlParameter("@MailboxSizeMB", mailboxSizeMB),
				 new SqlParameter("@MaxReceiveMessageSizeKB", maxReceiveMessageSizeKB),
				 new SqlParameter("@MaxRecipients", maxRecipients),
				 new SqlParameter("@MaxSendMessageSizeKB", maxSendMessageSizeKB),
				 new SqlParameter("@ProhibitSendPct", prohibitSendPct),
				 new SqlParameter("@ProhibitSendReceivePct", prohibitSendReceivePct),
				 new SqlParameter("@HideFromAddressBook", hideFromAddressBook),
				 new SqlParameter("@MailboxPlanType", mailboxPlanType),
				 new SqlParameter("@AllowLitigationHold", enabledLitigationHold),
				 new SqlParameter("@RecoverableItemsWarningPct", recoverabelItemsWarning),
				 new SqlParameter("@RecoverableItemsSpace", recoverabelItemsSpace),
				 new SqlParameter("@LitigationHoldUrl", litigationHoldUrl),
				 new SqlParameter("@LitigationHoldMsg", litigationHoldMsg),
				 new SqlParameter("@Archiving", Archiving),
				new SqlParameter("@EnableArchiving", EnableArchiving),
				 new SqlParameter("@ArchiveSizeMB", ArchiveSizeMB),
				 new SqlParameter("@ArchiveWarningPct", ArchiveWarningPct),
				 new SqlParameter("@EnableForceArchiveDeletion", enableForceArchiveDeletion),
				 new SqlParameter("@IsForJournaling", isForJournaling)
			);
		}



		public void DeleteExchangeMailboxPlan(int mailboxPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeMailboxPlan",
				 new SqlParameter("@MailboxPlanId", mailboxPlanId)
			);
		}


		public IDataReader GetExchangeMailboxPlan(int mailboxPlanId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeMailboxPlan",
				 new SqlParameter("@MailboxPlanId", mailboxPlanId)
			);
		}

		public IDataReader GetExchangeMailboxPlans(int itemId, bool archiving)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeMailboxPlans",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@Archiving", archiving)
			);
		}


		public IDataReader GetExchangeOrganization(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeOrganization",
				 new SqlParameter("@ItemID", itemId)
			);
		}


		public void SetOrganizationDefaultExchangeMailboxPlan(int itemId, int mailboxPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SetOrganizationDefaultExchangeMailboxPlan",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@MailboxPlanId", mailboxPlanId)
			);
		}

		public void SetExchangeAccountMailboxPlan(int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SetExchangeAccountMailboxplan",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@MailboxPlanId", (mailboxPlanId == 0) ? (object)DBNull.Value : (object)mailboxPlanId),
				 new SqlParameter("@ArchivingMailboxPlanId", (archivePlanId < 1) ? (object)DBNull.Value : (object)archivePlanId),
				 new SqlParameter("@EnableArchiving", EnableArchiving)
			);
		}

		#endregion

		#region Exchange Retention Policy Tags
		public int AddExchangeRetentionPolicyTag(int ItemID, string TagName, int TagType, int AgeLimitForRetention, int RetentionAction)
		{
			SqlParameter outParam = new SqlParameter("@TagID", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeRetentionPolicyTag",
				 outParam,
				 new SqlParameter("@ItemID", ItemID),
				 new SqlParameter("@TagName", TagName),
				 new SqlParameter("@TagType", TagType),
				 new SqlParameter("@AgeLimitForRetention", AgeLimitForRetention),
				 new SqlParameter("@RetentionAction", RetentionAction)
			);

			return Convert.ToInt32(outParam.Value);
		}

		public void UpdateExchangeRetentionPolicyTag(int TagID, int ItemID, string TagName, int TagType, int AgeLimitForRetention, int RetentionAction)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateExchangeRetentionPolicyTag",
				 new SqlParameter("@TagID", TagID),
				 new SqlParameter("@ItemID", ItemID),
				 new SqlParameter("@TagName", TagName),
				 new SqlParameter("@TagType", TagType),
				 new SqlParameter("@AgeLimitForRetention", AgeLimitForRetention),
				 new SqlParameter("@RetentionAction", RetentionAction)
			);
		}

		public void DeleteExchangeRetentionPolicyTag(int TagID)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeRetentionPolicyTag",
				 new SqlParameter("@TagID", TagID)
			);
		}

		public IDataReader GetExchangeRetentionPolicyTag(int TagID)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeRetentionPolicyTag",
				 new SqlParameter("@TagID", TagID)
			);
		}

		public IDataReader GetExchangeRetentionPolicyTags(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeRetentionPolicyTags",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public int AddExchangeMailboxPlanRetentionPolicyTag(int TagID, int MailboxPlanId)
		{
			SqlParameter outParam = new SqlParameter("@PlanTagID", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeMailboxPlanRetentionPolicyTag",
				 outParam,
				 new SqlParameter("@TagID", TagID),
				 new SqlParameter("@MailboxPlanId", MailboxPlanId)
			);

			return Convert.ToInt32(outParam.Value);
		}

		public void DeleteExchangeMailboxPlanRetentionPolicyTag(int PlanTagID)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeMailboxPlanRetentionPolicyTag",
				 new SqlParameter("@PlanTagID", PlanTagID)
			);
		}

		public IDataReader GetExchangeMailboxPlanRetentionPolicyTags(int MailboxPlanId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeMailboxPlanRetentionPolicyTags",
				 new SqlParameter("@MailboxPlanId", MailboxPlanId)
			);
		}


		#endregion

		#region Exchange Disclaimers
		public int AddExchangeDisclaimer(int itemID, ExchangeDisclaimer disclaimer)
		{
			SqlParameter outParam = new SqlParameter("@ExchangeDisclaimerId", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddExchangeDisclaimer",
				 outParam,

				 new SqlParameter("@ItemID", itemID),
				 new SqlParameter("@DisclaimerName", disclaimer.DisclaimerName),
				 new SqlParameter("@DisclaimerText", disclaimer.DisclaimerText)
			);

			return Convert.ToInt32(outParam.Value);
		}

		public void UpdateExchangeDisclaimer(int itemID, ExchangeDisclaimer disclaimer)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateExchangeDisclaimer",
				 new SqlParameter("@ExchangeDisclaimerId", disclaimer.ExchangeDisclaimerId),
				 new SqlParameter("@DisclaimerName", disclaimer.DisclaimerName),
				 new SqlParameter("@DisclaimerText", disclaimer.DisclaimerText)
			);
		}

		public void DeleteExchangeDisclaimer(int exchangeDisclaimerId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExchangeDisclaimer",
				 new SqlParameter("@ExchangeDisclaimerId", exchangeDisclaimerId)
			);
		}

		public IDataReader GetExchangeDisclaimer(int exchangeDisclaimerId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeDisclaimer",
				 new SqlParameter("@ExchangeDisclaimerId", exchangeDisclaimerId)
			);
		}

		public IDataReader GetExchangeDisclaimers(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetExchangeDisclaimers",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public void SetExchangeAccountDisclaimerId(int AccountID, int ExchangeDisclaimerId)
		{
			object id = null;
			if (ExchangeDisclaimerId != -1) id = ExchangeDisclaimerId;

			SqlHelper.ExecuteNonQuery(
								 ConnectionString,
								 CommandType.StoredProcedure,
								 "SetExchangeAccountDisclaimerId",
								 new SqlParameter("@AccountID", AccountID),
								 new SqlParameter("@ExchangeDisclaimerId", id)
							);
		}

		public int GetExchangeAccountDisclaimerId(int AccountID)
		{
			object objReturn = SqlHelper.ExecuteScalar(
								 ConnectionString,
								 CommandType.StoredProcedure,
								 "GetExchangeAccountDisclaimerId",
								 new SqlParameter("@AccountID", AccountID)
							);

			int ret;
			if (!int.TryParse(objReturn.ToString(), out ret)) return -1;
			return ret;
		}

		#endregion

		#region Organizations

		public int AddAccessToken(AccessToken token)
		{
			return AddAccessToken(token.AccessTokenGuid, token.AccountId, token.ItemId, token.ExpirationDate, token.TokenType);
		}

		public int AddAccessToken(Guid accessToken, int accountId, int itemId, DateTime expirationDate, AccessTokenTypes type)
		{
			SqlParameter prmId = new SqlParameter("@TokenID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddAccessToken",
				 prmId,
				 new SqlParameter("@AccessToken", accessToken),
				 new SqlParameter("@ExpirationDate", expirationDate),
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@TokenType", (int)type)
			);

			// read identity
			return Convert.ToInt32(prmId.Value);
		}

		public void SetAccessTokenResponseMessage(Guid accessToken, string response)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SetAccessTokenSmsResponse",
				 new SqlParameter("@AccessToken", accessToken),
				 new SqlParameter("@SmsResponse", response)
			);
		}

		public void DeleteExpiredAccessTokens()
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExpiredAccessTokenTokens"
			);
		}

		public IDataReader GetAccessTokenByAccessToken(Guid accessToken, AccessTokenTypes type)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetAccessTokenByAccessToken",
				 new SqlParameter("@AccessToken", accessToken),
				 new SqlParameter("@TokenType", type)
			);
		}

		public void DeleteAccessToken(Guid accessToken, AccessTokenTypes type)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteAccessToken",
				 new SqlParameter("@AccessToken", accessToken),
				 new SqlParameter("@TokenType", type)
			);
		}

		public void UpdateOrganizationSettings(int itemId, string settingsName, string xml)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateExchangeOrganizationSettings",
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@SettingsName", settingsName),
				 new SqlParameter("@Xml", xml));
		}

		public IDataReader GetOrganizationSettings(int itemId, string settingsName)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetExchangeOrganizationSettings",
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@SettingsName", settingsName));
		}

		public int AddOrganizationDeletedUser(int accountId, int originAT, string storagePath, string folderName, string fileName, DateTime expirationDate)
		{
			SqlParameter outParam = new SqlParameter("@ID", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddOrganizationDeletedUser",
				 outParam,
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@OriginAT", originAT),
				 new SqlParameter("@StoragePath", storagePath),
				 new SqlParameter("@FolderName", folderName),
				 new SqlParameter("@FileName", fileName),
				 new SqlParameter("@ExpirationDate", expirationDate)
			);

			return Convert.ToInt32(outParam.Value);
		}

		public void DeleteOrganizationDeletedUser(int id)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteOrganizationDeletedUser",
				 new SqlParameter("@ID", id));
		}

		public IDataReader GetOrganizationDeletedUser(int accountId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOrganizationDeletedUser",
				 new SqlParameter("@AccountID", accountId)
			);
		}

		public IDataReader GetAdditionalGroups(int userId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetAdditionalGroups",
				 new SqlParameter("@UserID", userId)
			);
		}


		public int AddAdditionalGroup(int userId, string groupName)
		{
			SqlParameter prmId = new SqlParameter("@GroupID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddAdditionalGroup",
				 prmId,
				 new SqlParameter("@UserID", userId),
				 new SqlParameter("@GroupName", groupName));

			// read identity
			return Convert.ToInt32(prmId.Value);
		}

		public void DeleteAdditionalGroup(int groupId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteAdditionalGroup",
				 new SqlParameter("@GroupID", groupId));
		}

		public void UpdateAdditionalGroup(int groupId, string groupName)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateAdditionalGroup",
				 new SqlParameter("@GroupID", groupId),
				 new SqlParameter("@GroupName", groupName)
			);
		}

		public void DeleteOrganizationUser(int itemId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteOrganizationUsers", new SqlParameter("@ItemID", itemId));
		}

		public int GetItemIdByOrganizationId(string id)
		{
			object obj = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetItemIdByOrganizationId",
											new SqlParameter("@OrganizationId", id));

			return (obj == null || DBNull.Value == obj) ? 0 : (int)obj;

		}

		public IDataReader GetOrganizationStatistics(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOrganizationStatistics",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public IDataReader GetOrganizationGroupsByDisplayName(int itemId, string displayName)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOrganizationGroupsByDisplayName",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@DisplayName", displayName)
			);
		}

		public IDataReader SearchOrganizationAccounts(int actorId, int itemId,
				  string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SearchOrganizationAccounts",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@IncludeMailboxes", includeMailboxes)
			);
		}

		public DataSet GetOrganizationObjectsByDomain(int itemId, string domainName)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOrganizationObjectsByDomain",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@DomainName", domainName)
			);
		}


		#endregion

		#region CRM

		public int GetCRMUsersCount(int itemId, string name, string email, int CALType)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
				 {
						  new SqlParameter("@ItemID", itemId),
						  GetFilterSqlParam("@Name", name),
						  GetFilterSqlParam("@Email", email),
						  new SqlParameter("@CALType", CALType)
				 };

			return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetCRMUsersCount", sqlParams);

		}

		private SqlParameter GetFilterSqlParam(string paramName, string value)
		{
			if (string.IsNullOrEmpty(value))
				return new SqlParameter(paramName, DBNull.Value);

			return new SqlParameter(paramName, value);
		}

		public IDataReader GetCrmUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
				 {
						  new SqlParameter("@ItemID", itemId),
						  new SqlParameter("@SortColumn", sortColumn),
						  new SqlParameter("@SortDirection", sortDirection),
						  GetFilterSqlParam("@Name", name),
						  GetFilterSqlParam("@Email", email),
						  new SqlParameter("@StartRow", startRow),
						  new SqlParameter("Count", count)
				 };


			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetCRMUsers", sqlParams);
		}

		public IDataReader GetCRMOrganizationUsers(int itemId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMOrganizationUsers",
													 new SqlParameter[] { new SqlParameter("@ItemID", itemId) });
		}

		public void CreateCRMUser(int itemId, Guid crmId, Guid businessUnitId, int CALType)
		{
			SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "InsertCRMUser",
											new SqlParameter[]
												 {
														  new SqlParameter("@ItemID", itemId),
														  new SqlParameter("@CrmUserID", crmId),
														  new SqlParameter("@BusinessUnitId", businessUnitId),
														  new SqlParameter("@CALType", CALType)
												 });

		}

		public void UpdateCRMUser(int itemId, int CALType)
		{
			SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "UpdateCRMUser",
											new SqlParameter[]
												 {
														  new SqlParameter("@ItemID", itemId),
														  new SqlParameter("@CALType", CALType)
												 });

		}

		public IDataReader GetCrmUser(int itemId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMUser",
											new SqlParameter[]
												 {
														  new SqlParameter("@AccountID", itemId)
												 });
			return reader;

		}

		public int GetCrmUserCount(int itemId)
		{
			return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOrganizationCRMUserCount",
				 new SqlParameter[] { new SqlParameter("@ItemID", itemId) });
		}

		public void DeleteCrmOrganization(int organizationId)
		{
			SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "DeleteCRMOrganization",
											new SqlParameter[] { new SqlParameter("@ItemID", organizationId) });
		}

		#endregion

		#region VPS - Virtual Private Servers

		public IDataReader GetVirtualMachinesPaged(int actorId, int packageId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetVirtualMachinesPaged",
												 new SqlParameter("@ActorID", actorId),
												 new SqlParameter("@PackageID", packageId),
												 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
												 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
												 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
												 new SqlParameter("@StartRow", startRow),
												 new SqlParameter("@MaximumRows", maximumRows),
												 new SqlParameter("@Recursive", recursive));
			return reader;
		}
		public IDataReader GetVirtualMachinesPaged2012(int actorId, int packageId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetVirtualMachinesPaged2012",
												 new SqlParameter("@ActorID", actorId),
												 new SqlParameter("@PackageID", packageId),
												 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
												 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
												 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
												 new SqlParameter("@StartRow", startRow),
												 new SqlParameter("@MaximumRows", maximumRows),
												 new SqlParameter("@Recursive", recursive));
			return reader;
		}

		public IDataReader GetVirtualMachinesPagedProxmox(int actorId, int packageId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetVirtualMachinesPagedProxmox",
												 new SqlParameter("@ActorID", actorId),
												 new SqlParameter("@PackageID", packageId),
												 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
												 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
												 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
												 new SqlParameter("@StartRow", startRow),
												 new SqlParameter("@MaximumRows", maximumRows),
												 new SqlParameter("@Recursive", recursive));
			return reader;
		}

		#endregion

		public IDataReader GetVirtualMachinesForPCPaged(int actorId, int packageId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetVirtualMachinesPagedForPC",
												 new SqlParameter("@ActorID", actorId),
												 new SqlParameter("@PackageID", packageId),
												 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
												 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
												 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
												 new SqlParameter("@StartRow", startRow),
												 new SqlParameter("@MaximumRows", maximumRows),
												 new SqlParameter("@Recursive", recursive));
			return reader;
		}


		#region VPS - External Network

		public IDataReader GetUnallottedIPAddresses(int packageId, int serviceId, int poolId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetUnallottedIPAddresses",
												 new SqlParameter("@PackageId", packageId),
												 new SqlParameter("@ServiceId", serviceId),
												 new SqlParameter("@PoolId", poolId));
		}


		public void AllocatePackageIPAddresses(int packageId, int orgId, string xml)
		{
			SqlParameter[] param = new[]
												{
														 new SqlParameter("@PackageID", packageId),
														 new SqlParameter("@OrgID", orgId),
														 new SqlParameter("@xml", xml)
													};

			ExecuteLongNonQuery("AllocatePackageIPAddresses", param);
		}

		public IDataReader GetPackageIPAddresses(int packageId, int orgId, int poolId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetPackageIPAddresses",
												 new SqlParameter("@PackageID", packageId),
												 new SqlParameter("@OrgID", orgId),
												 new SqlParameter("@PoolId", poolId),
												 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
												 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
												 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
												 new SqlParameter("@startRow", startRow),
												 new SqlParameter("@maximumRows", maximumRows),
												 new SqlParameter("@Recursive", recursive));
			return reader;
		}

		public int GetPackageIPAddressesCount(int packageId, int orgId, int poolId)
		{
			object obj = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure,
											 "GetPackageIPAddressesCount",
												 new SqlParameter("@PackageID", packageId),
												 new SqlParameter("@OrgID", orgId),
												 new SqlParameter("@PoolId", poolId));
			int res = 0;
			int.TryParse(obj.ToString(), out res);
			return res;
		}

		public void DeallocatePackageIPAddress(int id)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeallocatePackageIPAddress",
											  new SqlParameter("@PackageAddressID", id));
		}
		#endregion

		#region VPS - Private Network

		public IDataReader GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetPackagePrivateIPAddressesPaged",
												 new SqlParameter("@PackageID", packageId),
												 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
												 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
												 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
												 new SqlParameter("@startRow", startRow),
												 new SqlParameter("@maximumRows", maximumRows));
			return reader;
		}

		public IDataReader GetPackagePrivateIPAddresses(int packageId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
											 "GetPackagePrivateIPAddresses",
												 new SqlParameter("@PackageID", packageId));
			return reader;
		}
		#endregion

		#region VPS - External Network Adapter
		public IDataReader GetPackageUnassignedIPAddresses(int actorId, int packageId, int orgId, int poolId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
									  "GetPackageUnassignedIPAddresses",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@PackageID", packageId),
									  new SqlParameter("@OrgID", orgId),
									  new SqlParameter("@PoolId", poolId));
		}

		public IDataReader GetPackageIPAddress(int packageAddressId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
									  "GetPackageIPAddress",
									  new SqlParameter("@PackageAddressId", packageAddressId));
		}

		public IDataReader GetItemIPAddresses(int actorId, int itemId, int poolId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
									  "GetItemIPAddresses",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId),
									  new SqlParameter("@PoolID", poolId));
		}

		public int AddItemIPAddress(int actorId, int itemId, int packageAddressId)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "AddItemIPAddress",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId),
									  new SqlParameter("@PackageAddressID", packageAddressId));
		}

		public int SetItemPrimaryIPAddress(int actorId, int itemId, int packageAddressId)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "SetItemPrimaryIPAddress",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId),
									  new SqlParameter("@PackageAddressID", packageAddressId));
		}

		public int DeleteItemIPAddress(int actorId, int itemId, int packageAddressId)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "DeleteItemIPAddress",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId),
									  new SqlParameter("@PackageAddressID", packageAddressId));
		}

		public int DeleteItemIPAddresses(int actorId, int itemId)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "DeleteItemIPAddresses",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId));
		}
		#endregion

		#region VPS - Private Network Adapter
		public IDataReader GetItemPrivateIPAddresses(int actorId, int itemId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
									  "GetItemPrivateIPAddresses",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId));
		}

		public int AddItemPrivateIPAddress(int actorId, int itemId, string ipAddress)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "AddItemPrivateIPAddress",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId),
									  new SqlParameter("@IPAddress", ipAddress));
		}

		public int SetItemPrivatePrimaryIPAddress(int actorId, int itemId, int privateAddressId)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "SetItemPrivatePrimaryIPAddress",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId),
									  new SqlParameter("@PrivateAddressID", privateAddressId));
		}

		public int DeleteItemPrivateIPAddress(int actorId, int itemId, int privateAddressId)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "DeleteItemPrivateIPAddress",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId),
									  new SqlParameter("@PrivateAddressID", privateAddressId));
		}

		public int DeleteItemPrivateIPAddresses(int actorId, int itemId)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
									  "DeleteItemPrivateIPAddresses",
									  new SqlParameter("@ActorID", actorId),
									  new SqlParameter("@ItemID", itemId));
		}
		#endregion

		#region BlackBerry

		public void AddBlackBerryUser(int accountId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "AddBlackBerryUser",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId)
													});
		}


		public bool CheckBlackBerryUserExists(int accountId)
		{
			int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckBlackBerryUserExists",
											new SqlParameter("@AccountID", accountId));
			return res > 0;
		}


		public IDataReader GetBlackBerryUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
				 {
						  new SqlParameter("@ItemID", itemId),
						  new SqlParameter("@SortColumn", sortColumn),
						  new SqlParameter("@SortDirection", sortDirection),
						  GetFilterSqlParam("@Name", name),
						  GetFilterSqlParam("@Email", email),
						  new SqlParameter("@StartRow", startRow),
						  new SqlParameter("Count", count)
				 };


			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetBlackBerryUsers", sqlParams);
		}

		public int GetBlackBerryUsersCount(int itemId, string name, string email)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
													 {
															  new SqlParameter("@ItemID", itemId),
															  GetFilterSqlParam("@Name", name),
															  GetFilterSqlParam("@Email", email),
													 };

			return
				 (int)
				 SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetBlackBerryUsersCount", sqlParams);
		}

		public void DeleteBlackBerryUser(int accountId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "DeleteBlackBerryUser",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId)
													});

		}

		#endregion

		#region OCS

		public void AddOCSUser(int accountId, string instanceId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "AddOCSUser",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId),
															 new SqlParameter("@InstanceID", instanceId)
													});
		}


		public bool CheckOCSUserExists(int accountId)
		{
			int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckOCSUserExists",
											new SqlParameter("@AccountID", accountId));
			return res > 0;
		}


		public IDataReader GetOCSUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
				 {
						  new SqlParameter("@ItemID", itemId),
						  new SqlParameter("@SortColumn", sortColumn),
						  new SqlParameter("@SortDirection", sortDirection),
						  GetFilterSqlParam("@Name", name),
						  GetFilterSqlParam("@Email", email),
						  new SqlParameter("@StartRow", startRow),
						  new SqlParameter("Count", count)
				 };


			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOCSUsers", sqlParams);
		}

		public int GetOCSUsersCount(int itemId, string name, string email)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
													 {
															  new SqlParameter("@ItemID", itemId),
															  GetFilterSqlParam("@Name", name),
															  GetFilterSqlParam("@Email", email),
													 };

			return
				 (int)
				 SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOCSUsersCount", sqlParams);
		}

		public void DeleteOCSUser(string instanceId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "DeleteOCSUser",
											  new[]
													{
															 new SqlParameter("@InstanceId", instanceId)
													});

		}

		public string GetOCSUserInstanceID(int accountId)
		{
			return (string)SqlHelper.ExecuteScalar(ConnectionString,
											  CommandType.StoredProcedure,
											  "GetInstanceID",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId)
													});
		}

		#endregion

		#region SSL
		public int AddSSLRequest(int actorId, int packageId, int siteID, int userID, string friendlyname, string hostname, string csr, int csrLength, string distinguishedName, bool isRenewal, int previousID)
		{
			SqlParameter prmId = new SqlParameter("@SSLID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddSSLRequest", prmId,
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@UserID", userID),
				 new SqlParameter("@WebSiteID", siteID),
				 new SqlParameter("@FriendlyName", friendlyname),
				 new SqlParameter("@HostName", hostname),
				 new SqlParameter("@CSR", csr),
				 new SqlParameter("@CSRLength", csrLength),
				 new SqlParameter("@DistinguishedName", distinguishedName),
				 new SqlParameter("@IsRenewal", isRenewal),
				 new SqlParameter("@PreviousId", previousID)
				 );
			return Convert.ToInt32(prmId.Value);

		}

		public void CompleteSSLRequest(int actorId, int packageId, int id, string certificate, string distinguishedName, string serialNumber, byte[] hash, DateTime validFrom, DateTime expiryDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CompleteSSLRequest",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@ID", id),
				 new SqlParameter("@DistinguishedName", distinguishedName),
				 new SqlParameter("@Certificate", certificate),
				 new SqlParameter("@SerialNumber", serialNumber),
				 new SqlParameter("@Hash", Convert.ToBase64String(hash)),
				 new SqlParameter("@ValidFrom", validFrom),
				 new SqlParameter("@ExpiryDate", expiryDate));

		}

		public void AddPFX(int actorId, int packageId, int siteID, int userID, string hostname, string friendlyName, string distinguishedName, int csrLength, string serialNumber, DateTime validFrom, DateTime expiryDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "AddPFX",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@UserID", userID),
				 new SqlParameter("@WebSiteID", siteID),
				 new SqlParameter("@FriendlyName", friendlyName),
				 new SqlParameter("@HostName", hostname),
				 new SqlParameter("@CSRLength", csrLength),
				 new SqlParameter("@DistinguishedName", distinguishedName),
				 new SqlParameter("@SerialNumber", serialNumber),
				 new SqlParameter("@ValidFrom", validFrom),
				 new SqlParameter("@ExpiryDate", expiryDate));

		}

		public DataSet GetSSL(int actorId, int packageId, int id)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSSL",
				 new SqlParameter("@SSLID", id));

		}

		public DataSet GetCertificatesForSite(int actorId, int packageId, int siteId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetCertificatesForSite",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@websiteid", siteId));

		}

		public DataSet GetPendingCertificates(int actorId, int packageId, int id, bool recursive)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetPendingSSLForWebsite",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@websiteid", id),
				 new SqlParameter("@Recursive", recursive));

		}

		public IDataReader GetSSLCertificateByID(int actorId, int id)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSSLCertificateByID",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@ID", id));
		}

		public int CheckSSL(int siteID, bool renewal)
		{
			SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CheckSSL",
				 prmId,
				 new SqlParameter("@siteID", siteID),
				 new SqlParameter("@Renewal", renewal));

			return Convert.ToInt32(prmId.Value);
		}

		public IDataReader GetSiteCert(int actorId, int siteID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSSLCertificateByID",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@ID", siteID));
		}

		public void DeleteCertificate(int actorId, int packageId, int id)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteCertificate",
				 new SqlParameter("@ActorID", actorId),
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@id", id));
		}

		public bool CheckSSLExistsForWebsite(int siteId)
		{
			SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Bit);
			prmId.Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CheckSSLExistsForWebsite", prmId,
				 new SqlParameter("@siteID", siteId),
				 new SqlParameter("@SerialNumber", ""));
			return Convert.ToBoolean(prmId.Value);
		}
		#endregion

		#region Lync

		public void AddLyncUser(int accountId, int lyncUserPlanId, string sipAddress)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "AddLyncUser",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId),
															 new SqlParameter("@LyncUserPlanID", lyncUserPlanId),
															 new SqlParameter("@SipAddress", sipAddress)
													});
		}

		public void UpdateLyncUser(int accountId, string sipAddress)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "UpdateLyncUser",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId),
															 new SqlParameter("@SipAddress", sipAddress)
													});
		}


		public bool CheckLyncUserExists(int accountId)
		{
			int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckLyncUserExists",
											new SqlParameter("@AccountID", accountId));
			return res > 0;
		}

		public bool LyncUserExists(int accountId, string sipAddress)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "LyncUserExists",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@SipAddress", sipAddress),
				 outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}



		public IDataReader GetLyncUsers(int itemId, string sortColumn, string sortDirection, int startRow, int count)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
				 {
						  new SqlParameter("@ItemID", itemId),
						  new SqlParameter("@SortColumn", sortColumn),
						  new SqlParameter("@SortDirection", sortDirection),
						  new SqlParameter("@StartRow", startRow),
						  new SqlParameter("Count", count)
				 };


			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetLyncUsers", sqlParams);
		}


		public IDataReader GetLyncUsersByPlanId(int itemId, int planId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetLyncUsersByPlanId",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@PlanId", planId)
			);
		}

		public int GetLyncUsersCount(int itemId)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
													 {
															  new SqlParameter("@ItemID", itemId)
													 };

			return
				 (int)
				 SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetLyncUsersCount", sqlParams);
		}

		public void DeleteLyncUser(int accountId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "DeleteLyncUser",
											  new[]
													{
															 new SqlParameter("@AccountId", accountId)
													});

		}

		public int AddLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
		{
			SqlParameter outParam = new SqlParameter("@LyncUserPlanId", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddLyncUserPlan",
				 outParam,

				 new SqlParameter("@ItemID", itemID),
				 new SqlParameter("@LyncUserPlanName", lyncUserPlan.LyncUserPlanName),
				 new SqlParameter("@LyncUserPlanType", lyncUserPlan.LyncUserPlanType),
				 new SqlParameter("@IM", lyncUserPlan.IM),
				 new SqlParameter("@Mobility", lyncUserPlan.Mobility),
				 new SqlParameter("@MobilityEnableOutsideVoice", lyncUserPlan.MobilityEnableOutsideVoice),
				 new SqlParameter("@Federation", lyncUserPlan.Federation),
				 new SqlParameter("@Conferencing", lyncUserPlan.Conferencing),
				 new SqlParameter("@EnterpriseVoice", lyncUserPlan.EnterpriseVoice),
				 new SqlParameter("@VoicePolicy", lyncUserPlan.VoicePolicy),
				 new SqlParameter("@IsDefault", lyncUserPlan.IsDefault),

				 new SqlParameter("@RemoteUserAccess", lyncUserPlan.RemoteUserAccess),
				 new SqlParameter("@PublicIMConnectivity", lyncUserPlan.PublicIMConnectivity),

				 new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", lyncUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

				 new SqlParameter("@Telephony", lyncUserPlan.Telephony),

				 new SqlParameter("@ServerURI", lyncUserPlan.ServerURI),

				 new SqlParameter("@ArchivePolicy", lyncUserPlan.ArchivePolicy),
				 new SqlParameter("@TelephonyDialPlanPolicy", lyncUserPlan.TelephonyDialPlanPolicy),
				 new SqlParameter("@TelephonyVoicePolicy", lyncUserPlan.TelephonyVoicePolicy)
			);

			return Convert.ToInt32(outParam.Value);
		}


		public void UpdateLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateLyncUserPlan",
				 new SqlParameter("@LyncUserPlanId", lyncUserPlan.LyncUserPlanId),
				 new SqlParameter("@LyncUserPlanName", lyncUserPlan.LyncUserPlanName),
				 new SqlParameter("@LyncUserPlanType", lyncUserPlan.LyncUserPlanType),
				 new SqlParameter("@IM", lyncUserPlan.IM),
				 new SqlParameter("@Mobility", lyncUserPlan.Mobility),
				 new SqlParameter("@MobilityEnableOutsideVoice", lyncUserPlan.MobilityEnableOutsideVoice),
				 new SqlParameter("@Federation", lyncUserPlan.Federation),
				 new SqlParameter("@Conferencing", lyncUserPlan.Conferencing),
				 new SqlParameter("@EnterpriseVoice", lyncUserPlan.EnterpriseVoice),
				 new SqlParameter("@VoicePolicy", lyncUserPlan.VoicePolicy),
				 new SqlParameter("@IsDefault", lyncUserPlan.IsDefault),

				 new SqlParameter("@RemoteUserAccess", lyncUserPlan.RemoteUserAccess),
				 new SqlParameter("@PublicIMConnectivity", lyncUserPlan.PublicIMConnectivity),

				 new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", lyncUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

				 new SqlParameter("@Telephony", lyncUserPlan.Telephony),

				 new SqlParameter("@ServerURI", lyncUserPlan.ServerURI),

				 new SqlParameter("@ArchivePolicy", lyncUserPlan.ArchivePolicy),
				 new SqlParameter("@TelephonyDialPlanPolicy", lyncUserPlan.TelephonyDialPlanPolicy),
				 new SqlParameter("@TelephonyVoicePolicy", lyncUserPlan.TelephonyVoicePolicy)
			);
		}

		public void DeleteLyncUserPlan(int lyncUserPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteLyncUserPlan",
				 new SqlParameter("@LyncUserPlanId", lyncUserPlanId)
			);
		}

		public IDataReader GetLyncUserPlan(int lyncUserPlanId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetLyncUserPlan",
				 new SqlParameter("@LyncUserPlanId", lyncUserPlanId)
			);
		}


		public IDataReader GetLyncUserPlans(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetLyncUserPlans",
				 new SqlParameter("@ItemID", itemId)
			);
		}


		public void SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SetOrganizationDefaultLyncUserPlan",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@LyncUserPlanId", lyncUserPlanId)
			);
		}

		public IDataReader GetLyncUserPlanByAccountId(int AccountId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetLyncUserPlanByAccountId",
				 new SqlParameter("@AccountID", AccountId)
			);
		}


		public void SetLyncUserLyncUserplan(int accountId, int lyncUserPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SetLyncUserLyncUserplan",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@LyncUserPlanId", (lyncUserPlanId == 0) ? (object)DBNull.Value : (object)lyncUserPlanId)
			);
		}


		#endregion

		#region SfB

		public void AddSfBUser(int accountId, int sfbUserPlanId, string sipAddress)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "AddSfBUser",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId),
															 new SqlParameter("@SfBUserPlanID", sfbUserPlanId),
															 new SqlParameter("@SipAddress", sipAddress)
													});
		}

		public void UpdateSfBUser(int accountId, string sipAddress)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "UpdateSfBUser",
											  new[]
													{
															 new SqlParameter("@AccountID", accountId),
															 new SqlParameter("@SipAddress", sipAddress)
													});
		}


		public bool CheckSfBUserExists(int accountId)
		{
			int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckSfBUserExists",
											new SqlParameter("@AccountID", accountId));
			return res > 0;
		}

		public bool SfBUserExists(int accountId, string sipAddress)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SfBUserExists",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@SipAddress", sipAddress),
				 outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}



		public IDataReader GetSfBUsers(int itemId, string sortColumn, string sortDirection, int startRow, int count)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
				 {
						  new SqlParameter("@ItemID", itemId),
						  new SqlParameter("@SortColumn", sortColumn),
						  new SqlParameter("@SortDirection", sortDirection),
						  new SqlParameter("@StartRow", startRow),
						  new SqlParameter("Count", count)
				 };


			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSfBUsers", sqlParams);
		}


		public IDataReader GetSfBUsersByPlanId(int itemId, int planId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSfBUsersByPlanId",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@PlanId", planId)
			);
		}

		public int GetSfBUsersCount(int itemId)
		{
			SqlParameter[] sqlParams = new SqlParameter[]
													 {
															  new SqlParameter("@ItemID", itemId)
													 };

			return
				 (int)
				 SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetSfBUsersCount", sqlParams);
		}

		public void DeleteSfBUser(int accountId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
											  CommandType.StoredProcedure,
											  "DeleteSfBUser",
											  new[]
													{
															 new SqlParameter("@AccountId", accountId)
													});

		}

		public int AddSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
		{
			SqlParameter outParam = new SqlParameter("@SfBUserPlanId", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddSfBUserPlan",
				 outParam,

				 new SqlParameter("@ItemID", itemID),
				 new SqlParameter("@SfBUserPlanName", sfbUserPlan.SfBUserPlanName),
				 new SqlParameter("@SfBUserPlanType", sfbUserPlan.SfBUserPlanType),
				 new SqlParameter("@IM", sfbUserPlan.IM),
				 new SqlParameter("@Mobility", sfbUserPlan.Mobility),
				 new SqlParameter("@MobilityEnableOutsideVoice", sfbUserPlan.MobilityEnableOutsideVoice),
				 new SqlParameter("@Federation", sfbUserPlan.Federation),
				 new SqlParameter("@Conferencing", sfbUserPlan.Conferencing),
				 new SqlParameter("@EnterpriseVoice", sfbUserPlan.EnterpriseVoice),
				 new SqlParameter("@VoicePolicy", sfbUserPlan.VoicePolicy),
				 new SqlParameter("@IsDefault", sfbUserPlan.IsDefault),

				 new SqlParameter("@RemoteUserAccess", sfbUserPlan.RemoteUserAccess),
				 new SqlParameter("@PublicIMConnectivity", sfbUserPlan.PublicIMConnectivity),

				 new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", sfbUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

				 new SqlParameter("@Telephony", sfbUserPlan.Telephony),

				 new SqlParameter("@ServerURI", sfbUserPlan.ServerURI),

				 new SqlParameter("@ArchivePolicy", sfbUserPlan.ArchivePolicy),
				 new SqlParameter("@TelephonyDialPlanPolicy", sfbUserPlan.TelephonyDialPlanPolicy),
				 new SqlParameter("@TelephonyVoicePolicy", sfbUserPlan.TelephonyVoicePolicy)
			);

			return Convert.ToInt32(outParam.Value);
		}


		public void UpdateSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateSfBUserPlan",
				 new SqlParameter("@SfBUserPlanId", sfbUserPlan.SfBUserPlanId),
				 new SqlParameter("@SfBUserPlanName", sfbUserPlan.SfBUserPlanName),
				 new SqlParameter("@SfBUserPlanType", sfbUserPlan.SfBUserPlanType),
				 new SqlParameter("@IM", sfbUserPlan.IM),
				 new SqlParameter("@Mobility", sfbUserPlan.Mobility),
				 new SqlParameter("@MobilityEnableOutsideVoice", sfbUserPlan.MobilityEnableOutsideVoice),
				 new SqlParameter("@Federation", sfbUserPlan.Federation),
				 new SqlParameter("@Conferencing", sfbUserPlan.Conferencing),
				 new SqlParameter("@EnterpriseVoice", sfbUserPlan.EnterpriseVoice),
				 new SqlParameter("@VoicePolicy", sfbUserPlan.VoicePolicy),
				 new SqlParameter("@IsDefault", sfbUserPlan.IsDefault),

				 new SqlParameter("@RemoteUserAccess", sfbUserPlan.RemoteUserAccess),
				 new SqlParameter("@PublicIMConnectivity", sfbUserPlan.PublicIMConnectivity),

				 new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", sfbUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

				 new SqlParameter("@Telephony", sfbUserPlan.Telephony),

				 new SqlParameter("@ServerURI", sfbUserPlan.ServerURI),

				 new SqlParameter("@ArchivePolicy", sfbUserPlan.ArchivePolicy),
				 new SqlParameter("@TelephonyDialPlanPolicy", sfbUserPlan.TelephonyDialPlanPolicy),
				 new SqlParameter("@TelephonyVoicePolicy", sfbUserPlan.TelephonyVoicePolicy)
			);
		}

		public void DeleteSfBUserPlan(int sfbUserPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteSfBUserPlan",
				 new SqlParameter("@SfBUserPlanId", sfbUserPlanId)
			);
		}

		public IDataReader GetSfBUserPlan(int sfbUserPlanId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSfBUserPlan",
				 new SqlParameter("@SfBUserPlanId", sfbUserPlanId)
			);
		}


		public IDataReader GetSfBUserPlans(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSfBUserPlans",
				 new SqlParameter("@ItemID", itemId)
			);
		}


		public void SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SetOrganizationDefaultSfBUserPlan",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@SfBUserPlanId", sfbUserPlanId)
			);
		}

		public IDataReader GetSfBUserPlanByAccountId(int AccountId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSfBUserPlanByAccountId",
				 new SqlParameter("@AccountID", AccountId)
			);
		}


		public void SetSfBUserSfBUserplan(int accountId, int sfbUserPlanId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "SetSfBUserSfBUserplan",
				 new SqlParameter("@AccountID", accountId),
				 new SqlParameter("@SfBUserPlanId", (sfbUserPlanId == 0) ? (object)DBNull.Value : (object)sfbUserPlanId)
			);
		}


		#endregion

		public int GetPackageIdByName(string Name)
		{
			int packageId = -1;
			List<ProviderInfo> providers = ServerController.GetProviders();
			foreach (ProviderInfo providerInfo in providers)
			{
				if (string.Equals(Name, providerInfo.ProviderName, StringComparison.OrdinalIgnoreCase))
				{
					packageId = providerInfo.ProviderId;
					break;
				}
			}

			//if (-1 == packageId)
			//{
			//    throw new Exception("Provider not found");
			//}

			return packageId;
		}

		public int GetServiceIdByProviderForServer(int providerId, int packageId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT TOP 1 
                    PackageServices.ServiceID
                  FROM PackageServices
                  LEFT JOIN Services ON Services.ServiceID = PackageServices.ServiceID
                  WHERE PackageServices.PackageID = @PackageID AND Services.ProviderID = @ProviderID",
				 new SqlParameter("@ProviderID", providerId),
				 new SqlParameter("@PackageID", packageId));

			if (reader.Read())
			{
				return (int)reader["ServiceID"];
			}

			return -1;
		}

		#region Helicon Zoo

		public void GetHeliconZooProviderAndGroup(string providerName, out int providerId, out int groupId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT TOP 1 
                    ProviderID, GroupID
                  FROM Providers
                  WHERE ProviderName = @ProviderName",
				 new SqlParameter("@ProviderName", providerName));

			reader.Read();

			providerId = (int)reader["ProviderID"];
			groupId = (int)reader["GroupID"];

		}

		public IDataReader GetHeliconZooQuotas(int providerId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT
	                Q.QuotaID,
	                Q.GroupID,
	                Q.QuotaName,
	                Q.QuotaDescription,
	                Q.QuotaTypeID,
	                Q.ServiceQuota
                FROM Providers AS P
                INNER JOIN Quotas AS Q ON P.GroupID = Q.GroupID
                WHERE P.ProviderID = @ProviderID",
				 new SqlParameter("@ProviderID", providerId));

			return reader;
		}

		public void RemoveHeliconZooQuota(int groupId, string engineName)
		{
			int quotaId;

			// find quota id
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT TOP 1 
                    QuotaID
                  FROM Quotas
                  WHERE QuotaName = @QuotaName AND GroupID = @GroupID",
				 new SqlParameter("@QuotaName", engineName),
				 new SqlParameter("@GroupID", groupId));

			reader.Read();
			quotaId = (int)reader["QuotaID"];

			// delete references from HostingPlanQuotas
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
				 "DELETE FROM HostingPlanQuotas WHERE QuotaID = @QuotaID",
				 new SqlParameter("@QuotaID", quotaId)
			);

			// delete from Quotas
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
				 "DELETE FROM Quotas WHERE QuotaID = @QuotaID",
				 new SqlParameter("@QuotaID", quotaId)
			);

		}

		public void AddHeliconZooQuota(int groupId, int quotaId, string engineName, string engineDescription, int quotaOrder)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
					  @"INSERT INTO Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota)
                    VALUES (@QuotaID, @GroupID, @QuotaOrder, @QuotaName, @QuotaDescription, 1, 0)",
					  new SqlParameter("@QuotaID", quotaId),
					  new SqlParameter("@GroupID", groupId),
					  new SqlParameter("@QuotaOrder", quotaOrder),
					  new SqlParameter("@QuotaName", engineName),
					  new SqlParameter("@QuotaDescription", engineDescription)
				 );
		}

		public IDataReader GetEnabledHeliconZooQuotasForPackage(int packageId)
		{
			int providerId, groupId;

			GetHeliconZooProviderAndGroup("HeliconZoo", out providerId, out groupId);

			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT     HostingPlanQuotas.QuotaID, Quotas.QuotaName, Quotas.QuotaDescription
                FROM         HostingPlanQuotas 
                    INNER JOIN Packages ON HostingPlanQuotas.PlanID = Packages.PlanID 
                        INNER JOIN Quotas ON HostingPlanQuotas.QuotaID = Quotas.QuotaID
                WHERE     
                    (Packages.PackageID = @PackageID) AND (Quotas.GroupID = @GroupID) AND (HostingPlanQuotas.QuotaValue = 1)",
				 new SqlParameter("@PackageID", packageId),
				 new SqlParameter("@GroupID", groupId)
			);

			return reader;
		}

		public int GetServiceIdForProviderIdAndPackageId(int providerId, int packageId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT PackageServices.ServiceID 
                FROM PackageServices
                INNER JOIN Services ON PackageServices.ServiceID = Services.ServiceID
                WHERE Services.ProviderID = @ProviderID and PackageID = @PackageID",
				 new SqlParameter("@ProviderID", providerId),
				 new SqlParameter("@PackageID", packageId)
			);

			if (reader.Read())
			{
				return (int)reader["ServiceID"];
			}

			return -1;

		}

		public int GetServerIdForPackage(int packageId)
		{
			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
				 @"SELECT TOP 1 
                    ServerID
                  FROM Packages
                  WHERE PackageID = @PackageID",
				 new SqlParameter("@PackageID", packageId)
			);

			if (reader.Read())
			{
				return (int)reader["ServerID"];
			}

			return -1;
		}

		#endregion

		#region Enterprise Storage

		public int AddWebDavAccessToken(Base.HostedSolution.WebDavAccessToken accessToken)
		{
			SqlParameter prmId = new SqlParameter("@TokenID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddWebDavAccessToken",
				 prmId,
				 new SqlParameter("@AccessToken", accessToken.AccessToken),
				 new SqlParameter("@FilePath", accessToken.FilePath),
				 new SqlParameter("@AuthData", accessToken.AuthData),
				 new SqlParameter("@ExpirationDate", accessToken.ExpirationDate),
				 new SqlParameter("@AccountID", accessToken.AccountId),
				 new SqlParameter("@ItemId", accessToken.ItemId)
			);

			// read identity
			return Convert.ToInt32(prmId.Value);
		}

		public void DeleteExpiredWebDavAccessTokens()
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteExpiredWebDavAccessTokens"
			);
		}

		public IDataReader GetWebDavAccessTokenById(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetWebDavAccessTokenById",
				 new SqlParameter("@Id", id)
			);
		}

		public IDataReader GetWebDavAccessTokenByAccessToken(Guid accessToken)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetWebDavAccessTokenByAccessToken",
				 new SqlParameter("@AccessToken", accessToken)
			);
		}

		public int AddEntepriseFolder(int itemId, string folderName, int folderQuota, string locationDrive, string homeFolder, string domain, int? storageSpaceFolderId)
		{
			SqlParameter prmId = new SqlParameter("@FolderID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddEnterpriseFolder",
				 prmId,
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderName", folderName),
				 new SqlParameter("@FolderQuota", folderQuota),
				 new SqlParameter("@LocationDrive", locationDrive),
				 new SqlParameter("@HomeFolder", homeFolder),
				 new SqlParameter("@Domain", domain),
				 new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId)
			);

			// read identity
			return Convert.ToInt32(prmId.Value);
		}

		public void UpdateEntepriseFolderStorageSpaceFolder(int itemId, string folderName, int? storageSpaceFolderId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateEntepriseFolderStorageSpaceFolder",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderName", folderName),
				 new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId)
			);
		}

		public void DeleteEnterpriseFolder(int itemId, string folderName)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteEnterpriseFolder",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderName", folderName));
		}

		public void UpdateEnterpriseFolder(int itemId, string folderID, string folderName, int folderQuota)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateEnterpriseFolder",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderID", folderID),
				 new SqlParameter("@FolderName", folderName),
				 new SqlParameter("@FolderQuota", folderQuota));
		}

		public IDataReader GetEnterpriseFolders(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetEnterpriseFolders",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public DataSet GetEnterpriseFoldersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetEnterpriseFoldersPaged",
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows)
			);
		}

		public IDataReader GetEnterpriseFolder(int itemId, string folderName)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetEnterpriseFolder",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderName", folderName)
			);
		}

		public IDataReader GetWebDavPortalUserSettingsByAccountId(int accountId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetWebDavPortalUsersSettingsByAccountId",
				 new SqlParameter("@AccountId", accountId)
			);
		}

		public int AddWebDavPortalUsersSettings(int accountId, string settings)
		{
			SqlParameter settingsId = new SqlParameter("@WebDavPortalUsersSettingsId", SqlDbType.Int);
			settingsId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddWebDavPortalUsersSettings",
				 settingsId,
				 new SqlParameter("@AccountId", accountId),
				 new SqlParameter("@Settings", settings)
			);

			// read identity
			return Convert.ToInt32(settingsId.Value);
		}

		public void UpdateWebDavPortalUsersSettings(int accountId, string settings)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateWebDavPortalUsersSettings",
				 new SqlParameter("@AccountId", accountId),
				 new SqlParameter("@Settings", settings)
			);
		}

		public void DeleteAllEnterpriseFolderOwaUsers(int itemId, int folderId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteAllEnterpriseFolderOwaUsers",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderID", folderId)
			);
		}

		public int AddEnterpriseFolderOwaUser(int itemId, int folderId, int accountId)
		{
			SqlParameter id = new SqlParameter("@ESOwsaUserId", SqlDbType.Int);
			id.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddEnterpriseFolderOwaUser",
				 id,
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderID", folderId),
				 new SqlParameter("@AccountId", accountId)
			);

			// read identity
			return Convert.ToInt32(id.Value);
		}

		public IDataReader GetEnterpriseFolderOwaUsers(int itemId, int folderId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetEnterpriseFolderOwaUsers",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderID", folderId)
			);
		}

		public IDataReader GetEnterpriseFolderId(int itemId, string folderName)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetEnterpriseFolderId",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@FolderName", folderName)
			);
		}

		public IDataReader GetUserEnterpriseFolderWithOwaEditPermission(int itemId, int accountId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetUserEnterpriseFolderWithOwaEditPermission",
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@AccountID", accountId)
			);
		}

		#endregion

		#region Support Service Levels

		public IDataReader GetSupportServiceLevels()
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetSupportServiceLevels");
		}

		public int AddSupportServiceLevel(string levelName, string levelDescription)
		{
			SqlParameter outParam = new SqlParameter("@LevelID", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddSupportServiceLevel",
				 outParam,
				 new SqlParameter("@LevelName", levelName),
				 new SqlParameter("@LevelDescription", levelDescription)
			);

			return Convert.ToInt32(outParam.Value);
		}

		public void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateSupportServiceLevel",
				 new SqlParameter("@LevelID", levelID),
				 new SqlParameter("@LevelName", levelName),
				 new SqlParameter("@LevelDescription", levelDescription)
			);
		}

		public void DeleteSupportServiceLevel(int levelID)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteSupportServiceLevel",
				 new SqlParameter("@LevelID", levelID)
			);
		}

		public IDataReader GetSupportServiceLevel(int levelID)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSupportServiceLevel",
				 new SqlParameter("@LevelID", levelID)
			);
		}

		public bool CheckServiceLevelUsage(int levelID)
		{
			int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckServiceLevelUsage",
											new SqlParameter("@LevelID", levelID));
			return res > 0;
		}

		#endregion

		#region Storage Spaces 

		public DataSet GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpaceLevelsPaged",
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows)
			);
		}

		public IDataReader GetStorageSpaceLevelById(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpaceLevelById",
				 new SqlParameter("@ID", id)
			);
		}

		public int UpdateStorageSpaceLevel(StorageSpaceLevel level)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateStorageSpaceLevel",
				 new SqlParameter("@ID", level.Id),
				 new SqlParameter("@Name", level.Name),
				 new SqlParameter("@Description", level.Description)
			);

			return level.Id;
		}

		public int InsertStorageSpaceLevel(StorageSpaceLevel level)
		{
			SqlParameter id = new SqlParameter("@ID", SqlDbType.Int);
			id.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "InsertStorageSpaceLevel",
				 id,
				 new SqlParameter("@Name", level.Name),
				 new SqlParameter("@Description", level.Description)
			);

			// read identity
			return Convert.ToInt32(id.Value);
		}

		public void RemoveStorageSpaceLevel(int id)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "RemoveStorageSpaceLevel",
				 new SqlParameter("@ID", id)
			);
		}

		public IDataReader GetStorageSpaceLevelResourceGroups(int levelId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetLevelResourceGroups",
				 new SqlParameter("@LevelId", levelId)
			);
		}

		public void RemoveStorageSpaceLevelResourceGroups(int levelId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteLevelResourceGroups",
				 new SqlParameter("@LevelId", levelId)
			);
		}

		public void AddStorageSpaceLevelResourceGroup(int levelId, int groupId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddLevelResourceGroups",
				 new SqlParameter("@LevelId", levelId),
				 new SqlParameter("@GroupId", groupId)
			);
		}

		public DataSet GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpacesPaged",
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows)
			);
		}

		public IDataReader GetStorageSpaceById(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpaceById",
				 new SqlParameter("@ID", id)
			);
		}

		public IDataReader GetStorageSpaceByServiceAndPath(int serverId, string path)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpaceByServiceAndPath",
				 new SqlParameter("@ServerId", serverId),
				 new SqlParameter("@Path", path)

			);
		}

		public int UpdateStorageSpace(StorageSpace space)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateStorageSpace",
				 new SqlParameter("@ID", space.Id),
				 new SqlParameter("@Name", space.Name),
				 new SqlParameter("@ServiceId", space.ServiceId),
				 new SqlParameter("@ServerId", space.ServerId),
				 new SqlParameter("@LevelId", space.LevelId),
				 new SqlParameter("@Path", space.Path),
				 new SqlParameter("@FsrmQuotaType", space.FsrmQuotaType),
				 new SqlParameter("@FsrmQuotaSizeBytes", space.FsrmQuotaSizeBytes),
				 new SqlParameter("@IsShared", space.IsShared),
				 new SqlParameter("@IsDisabled", space.IsDisabled),
				 new SqlParameter("@UncPath", space.UncPath)
			);

			return space.Id;
		}

		public int InsertStorageSpace(StorageSpace space)
		{
			SqlParameter id = new SqlParameter("@ID", SqlDbType.Int);
			id.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "InsertStorageSpace",
				 id,
				 new SqlParameter("@Name", space.Name),
				 new SqlParameter("@ServiceId", space.ServiceId),
				 new SqlParameter("@ServerId", space.ServerId),
				 new SqlParameter("@LevelId", space.LevelId),
				 new SqlParameter("@Path", space.Path),
				 new SqlParameter("@FsrmQuotaType", space.FsrmQuotaType),
				 new SqlParameter("@FsrmQuotaSizeBytes", space.FsrmQuotaSizeBytes),
				 new SqlParameter("@IsShared", space.IsShared),
				 new SqlParameter("@IsDisabled", space.IsDisabled),
				 new SqlParameter("@UncPath", space.UncPath)
			);

			// read identity
			return Convert.ToInt32(id.Value);
		}

		public void RemoveStorageSpace(int id)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "RemoveStorageSpace",
				 new SqlParameter("@ID", id)
			);
		}

		public DataSet GetStorageSpacesByLevelId(int levelId)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpacesByLevelId",
				 new SqlParameter("@LevelId", levelId)
			);
		}

		public IDataReader GetStorageSpacesByResourceGroupName(string groupName)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpacesByResourceGroupName",
				 new SqlParameter("@ResourceGroupName", groupName)
			);
		}

		public int CreateStorageSpaceFolder(StorageSpaceFolder folder)
		{
			folder.Id = CreateStorageSpaceFolder(folder.Name, folder.StorageSpaceId, folder.Path, folder.UncPath, folder.IsShared, folder.FsrmQuotaType, folder.FsrmQuotaSizeBytes);

			return folder.Id;
		}

		public int CreateStorageSpaceFolder(string name, int storageSpaceId, string path, string uncPath, bool isShared, QuotaType quotaType, long fsrmQuotaSizeBytes)
		{
			SqlParameter id = new SqlParameter("@ID", SqlDbType.Int);
			id.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "CreateStorageSpaceFolder",
				 id,
				 new SqlParameter("@Name", name),
				 new SqlParameter("@StorageSpaceId", storageSpaceId),
				 new SqlParameter("@Path", path),
				 new SqlParameter("@UncPath", uncPath),
				 new SqlParameter("@IsShared", isShared),
				 new SqlParameter("@FsrmQuotaType", quotaType),
				 new SqlParameter("@FsrmQuotaSizeBytes", fsrmQuotaSizeBytes)
			);

			// read identity
			return Convert.ToInt32(id.Value);
		}

		public int UpdateStorageSpaceFolder(StorageSpaceFolder folder)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateStorageSpaceFolder",
				 new SqlParameter("@ID", folder.Id),
				 new SqlParameter("@Name", folder.Name),
				 new SqlParameter("@StorageSpaceId", folder.StorageSpaceId),
				 new SqlParameter("@Path", folder.Path),
				 new SqlParameter("@UncPath", folder.UncPath),
				 new SqlParameter("@IsShared", folder.IsShared),
				 new SqlParameter("@FsrmQuotaType", folder.FsrmQuotaType),
				 new SqlParameter("@FsrmQuotaSizeBytes", folder.FsrmQuotaSizeBytes)
			);

			return folder.Id;
		}

		public int UpdateStorageSpaceFolder(int id, string folderName, int storageSpaceId, string path, string uncPath, bool isShared, QuotaType type, long fsrmQuotaSizeBytes)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateStorageSpaceFolder",
				 new SqlParameter("@ID", id),
				 new SqlParameter("@Name", folderName),
				 new SqlParameter("@StorageSpaceId", storageSpaceId),
				 new SqlParameter("@Path", path),
				 new SqlParameter("@UncPath", uncPath),
				 new SqlParameter("@IsShared", isShared),
				 new SqlParameter("@FsrmQuotaType", type),
				 new SqlParameter("@FsrmQuotaSizeBytes", fsrmQuotaSizeBytes)
			);

			return id;
		}

		public IDataReader GetStorageSpaceFoldersByStorageSpaceId(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpaceFoldersByStorageSpaceId",
				 new SqlParameter("@StorageSpaceId", id)
			);
		}

		public IDataReader GetStorageSpaceFolderById(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetStorageSpaceFolderById",
				 new SqlParameter("@ID", id)
			);
		}

		public void RemoveStorageSpaceFolder(int id)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "RemoveStorageSpaceFolder",
				 new SqlParameter("@ID", id)
			);
		}

		#endregion

		#region RDS

		public int CheckRDSServer(string ServerFQDN)
		{
			SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "CheckRDSServer",
				 prmId,
				 new SqlParameter("@ServerFQDN", ServerFQDN));

			return Convert.ToInt32(prmId.Value);
		}

		public IDataReader GetRdsServerSettings(int serverId, string settingsName)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetRDSServerSettings",
				 new SqlParameter("@ServerId", serverId),
				 new SqlParameter("@SettingsName", settingsName));
		}

		public void UpdateRdsServerSettings(int serverId, string settingsName, string xml)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "UpdateRDSServerSettings",
				 new SqlParameter("@ServerId", serverId),
				 new SqlParameter("@SettingsName", settingsName),
				 new SqlParameter("@Xml", xml));
		}

		public int AddRdsCertificate(int serviceId, string content, byte[] hash, string fileName, DateTime? validFrom, DateTime? expiryDate)
		{
			SqlParameter rdsCertificateId = new SqlParameter("@RDSCertificateID", SqlDbType.Int);
			rdsCertificateId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddRDSCertificate",
				 rdsCertificateId,
				 new SqlParameter("@ServiceId", serviceId),
				 new SqlParameter("@Content", content),
				 new SqlParameter("@Hash", Convert.ToBase64String(hash)),
				 new SqlParameter("@FileName", fileName),
				 new SqlParameter("@ValidFrom", validFrom),
				 new SqlParameter("@ExpiryDate", expiryDate)
			);

			return Convert.ToInt32(rdsCertificateId.Value);
		}

		public IDataReader GetRdsCertificateByServiceId(int serviceId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSCertificateByServiceId",
				 new SqlParameter("@ServiceId", serviceId)
			);
		}

		public IDataReader GetRdsCollectionSettingsByCollectionId(int collectionId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSCollectionSettingsByCollectionId",
				 new SqlParameter("@RDSCollectionID", collectionId)
			);
		}

		public int AddRdsCollectionSettings(RdsCollectionSettings settings)
		{
			return AddRdsCollectionSettings(settings.RdsCollectionId, settings.DisconnectedSessionLimitMin, settings.ActiveSessionLimitMin, settings.IdleSessionLimitMin, settings.BrokenConnectionAction,
				 settings.AutomaticReconnectionEnabled, settings.TemporaryFoldersDeletedOnExit, settings.TemporaryFoldersPerSession, settings.ClientDeviceRedirectionOptions, settings.ClientPrinterRedirected,
				 settings.ClientPrinterAsDefault, settings.RDEasyPrintDriverEnabled, settings.MaxRedirectedMonitors, settings.SecurityLayer, settings.EncryptionLevel, settings.AuthenticateUsingNLA);
		}

		private int AddRdsCollectionSettings(int rdsCollectionId, int disconnectedSessionLimitMin, int activeSessionLimitMin, int idleSessionLimitMin, string brokenConnectionAction,
			 bool automaticReconnectionEnabled, bool temporaryFoldersDeletedOnExit, bool temporaryFoldersPerSession, string clientDeviceRedirectionOptions, bool ClientPrinterRedirected,
			 bool clientPrinterAsDefault, bool rdEasyPrintDriverEnabled, int maxRedirectedMonitors, string SecurityLayer, string EncryptionLevel, bool AuthenticateUsingNLA)
		{
			SqlParameter rdsCollectionSettingsId = new SqlParameter("@RDSCollectionSettingsID", SqlDbType.Int);
			rdsCollectionSettingsId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddRDSCollectionSettings",
				 rdsCollectionSettingsId,
				 new SqlParameter("@RdsCollectionId", rdsCollectionId),
				 new SqlParameter("@DisconnectedSessionLimitMin", disconnectedSessionLimitMin),
				 new SqlParameter("@ActiveSessionLimitMin", activeSessionLimitMin),
				 new SqlParameter("@IdleSessionLimitMin", idleSessionLimitMin),
				 new SqlParameter("@BrokenConnectionAction", brokenConnectionAction),
				 new SqlParameter("@AutomaticReconnectionEnabled", automaticReconnectionEnabled),
				 new SqlParameter("@TemporaryFoldersDeletedOnExit", temporaryFoldersDeletedOnExit),
				 new SqlParameter("@TemporaryFoldersPerSession", temporaryFoldersPerSession),
				 new SqlParameter("@ClientDeviceRedirectionOptions", clientDeviceRedirectionOptions),
				 new SqlParameter("@ClientPrinterRedirected", ClientPrinterRedirected),
				 new SqlParameter("@ClientPrinterAsDefault", clientPrinterAsDefault),
				 new SqlParameter("@RDEasyPrintDriverEnabled", rdEasyPrintDriverEnabled),
				 new SqlParameter("@MaxRedirectedMonitors", maxRedirectedMonitors),
				 new SqlParameter("@SecurityLayer", SecurityLayer),
				 new SqlParameter("@EncryptionLevel", EncryptionLevel),
				 new SqlParameter("@AuthenticateUsingNLA", AuthenticateUsingNLA)
			);

			return Convert.ToInt32(rdsCollectionSettingsId.Value);
		}

		public void UpdateRDSCollectionSettings(RdsCollectionSettings settings)
		{
			UpdateRDSCollectionSettings(settings.Id, settings.RdsCollectionId, settings.DisconnectedSessionLimitMin, settings.ActiveSessionLimitMin, settings.IdleSessionLimitMin, settings.BrokenConnectionAction,
				 settings.AutomaticReconnectionEnabled, settings.TemporaryFoldersDeletedOnExit, settings.TemporaryFoldersPerSession, settings.ClientDeviceRedirectionOptions, settings.ClientPrinterRedirected,
				 settings.ClientPrinterAsDefault, settings.RDEasyPrintDriverEnabled, settings.MaxRedirectedMonitors, settings.SecurityLayer, settings.EncryptionLevel, settings.AuthenticateUsingNLA);
		}

		public void UpdateRDSCollectionSettings(int id, int rdsCollectionId, int disconnectedSessionLimitMin, int activeSessionLimitMin, int idleSessionLimitMin, string brokenConnectionAction,
			 bool automaticReconnectionEnabled, bool temporaryFoldersDeletedOnExit, bool temporaryFoldersPerSession, string clientDeviceRedirectionOptions, bool ClientPrinterRedirected,
			 bool clientPrinterAsDefault, bool rdEasyPrintDriverEnabled, int maxRedirectedMonitors, string SecurityLayer, string EncryptionLevel, bool AuthenticateUsingNLA)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateRDSCollectionSettings",
				 new SqlParameter("@Id", id),
				 new SqlParameter("@RdsCollectionId", rdsCollectionId),
				 new SqlParameter("@DisconnectedSessionLimitMin", disconnectedSessionLimitMin),
				 new SqlParameter("@ActiveSessionLimitMin", activeSessionLimitMin),
				 new SqlParameter("@IdleSessionLimitMin", idleSessionLimitMin),
				 new SqlParameter("@BrokenConnectionAction", brokenConnectionAction),
				 new SqlParameter("@AutomaticReconnectionEnabled", automaticReconnectionEnabled),
				 new SqlParameter("@TemporaryFoldersDeletedOnExit", temporaryFoldersDeletedOnExit),
				 new SqlParameter("@TemporaryFoldersPerSession", temporaryFoldersPerSession),
				 new SqlParameter("@ClientDeviceRedirectionOptions", clientDeviceRedirectionOptions),
				 new SqlParameter("@ClientPrinterRedirected", ClientPrinterRedirected),
				 new SqlParameter("@ClientPrinterAsDefault", clientPrinterAsDefault),
				 new SqlParameter("@RDEasyPrintDriverEnabled", rdEasyPrintDriverEnabled),
				 new SqlParameter("@MaxRedirectedMonitors", maxRedirectedMonitors),
				 new SqlParameter("@SecurityLayer", SecurityLayer),
				 new SqlParameter("@EncryptionLevel", EncryptionLevel),
				 new SqlParameter("@AuthenticateUsingNLA", AuthenticateUsingNLA)
			);
		}

		public void DeleteRDSCollectionSettings(int id)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteRDSCollectionSettings",
				 new SqlParameter("@Id", id)
			);
		}

		public IDataReader GetRDSCollectionsByItemId(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSCollectionsByItemId",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public IDataReader GetRDSCollectionByName(string name)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSCollectionByName",
				 new SqlParameter("@Name", name)
			);
		}

		public IDataReader GetRDSCollectionById(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSCollectionById",
				 new SqlParameter("@ID", id)
			);
		}

		public DataSet GetRDSCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSCollectionsPaged",
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@itemId", itemId),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@maximumRows", maximumRows)
			);
		}

		public int AddRDSCollection(int itemId, string name, string description, string displayName)
		{
			SqlParameter rdsCollectionId = new SqlParameter("@RDSCollectionID", SqlDbType.Int);
			rdsCollectionId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddRDSCollection",
				 rdsCollectionId,
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@Name", name),
				 new SqlParameter("@Description", description),
				 new SqlParameter("@DisplayName", displayName)
			);

			// read identity
			return Convert.ToInt32(rdsCollectionId.Value);
		}

		public int GetOrganizationRdsUsersCount(int itemId)
		{
			SqlParameter count = new SqlParameter("@TotalNumber", SqlDbType.Int);
			count.Direction = ParameterDirection.Output;

			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetOrganizationRdsUsersCount",
				 count,
				 new SqlParameter("@ItemId", itemId));

			// read identity
			return Convert.ToInt32(count.Value);
		}

		public int GetOrganizationRdsCollectionsCount(int itemId)
		{
			SqlParameter count = new SqlParameter("@TotalNumber", SqlDbType.Int);
			count.Direction = ParameterDirection.Output;

			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetOrganizationRdsCollectionsCount",
				 count,
				 new SqlParameter("@ItemId", itemId));

			// read identity
			return Convert.ToInt32(count.Value);
		}

		public int GetOrganizationRdsServersCount(int itemId)
		{
			SqlParameter count = new SqlParameter("@TotalNumber", SqlDbType.Int);
			count.Direction = ParameterDirection.Output;

			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetOrganizationRdsServersCount",
				 count,
				 new SqlParameter("@ItemId", itemId));

			// read identity
			return Convert.ToInt32(count.Value);
		}

		public void UpdateRDSCollection(RdsCollection collection)
		{
			UpdateRDSCollection(collection.Id, collection.ItemId, collection.Name, collection.Description, collection.DisplayName);
		}

		public void UpdateRDSCollection(int id, int itemId, string name, string description, string displayName)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateRDSCollection",
				 new SqlParameter("@Id", id),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@Name", name),
				 new SqlParameter("@Description", description),
				 new SqlParameter("@DisplayName", displayName)
			);
		}

		public void DeleteRDSServerSettings(int serverId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteRDSServerSettings",
				 new SqlParameter("@ServerId", serverId)
			);
		}

		public void DeleteRDSCollection(int id)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteRDSCollection",
				 new SqlParameter("@Id", id)
			);
		}

		public int AddRDSServer(string name, string fqdName, string description, string controller)
		{
			SqlParameter rdsServerId = new SqlParameter("@RDSServerID", SqlDbType.Int);
			rdsServerId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddRDSServer",
				 rdsServerId,
				 new SqlParameter("@FqdName", fqdName),
				 new SqlParameter("@Name", name),
				 new SqlParameter("@Description", description),
				 new SqlParameter("@Controller", controller)
			);

			// read identity
			return Convert.ToInt32(rdsServerId.Value);
		}

		public IDataReader GetRDSServersByItemId(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSServersByItemId",
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public DataSet GetRDSServersPaged(int? itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string controller, bool ignoreItemId = false, bool ignoreRdsCollectionId = false)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSServersPaged",
				 new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				 new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				 new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				 new SqlParameter("@startRow", startRow),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@RdsCollectionId", collectionId),
				 new SqlParameter("@IgnoreItemId", ignoreItemId),
				 new SqlParameter("@IgnoreRdsCollectionId", ignoreRdsCollectionId),
				 new SqlParameter("@maximumRows", maximumRows),
				 new SqlParameter("@Controller", controller)
			);
		}

		public IDataReader GetRDSServerById(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSServerById",
				 new SqlParameter("@ID", id)
			);
		}

		public IDataReader GetRDSServersByCollectionId(int collectionId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSServersByCollectionId",
				 new SqlParameter("@RdsCollectionId", collectionId)
			);
		}

		public void DeleteRDSServer(int id)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteRDSServer",
				 new SqlParameter("@Id", id)
			);
		}

		public void UpdateRDSServer(RdsServer server)
		{
			UpdateRDSServer(server.Id, server.ItemId, server.Name, server.FqdName, server.Description,
				 server.RdsCollectionId, server.ConnectionEnabled);
		}

		public void UpdateRDSServer(int id, int? itemId, string name, string fqdName, string description, int? rdsCollectionId, string connectionEnabled)
		{
			byte connEnabled = 1;
			if (!String.IsNullOrEmpty(connectionEnabled))
			{
				if (connectionEnabled.Equals("false") || connectionEnabled.Equals("no") || connectionEnabled.Equals("0")) connEnabled = 0;
			}
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateRDSServer",
				 new SqlParameter("@Id", id),
				 new SqlParameter("@ItemID", itemId),
				 new SqlParameter("@Name", name),
				 new SqlParameter("@FqdName", fqdName),
				 new SqlParameter("@Description", description),
				 new SqlParameter("@RDSCollectionId", rdsCollectionId),
				 new SqlParameter("@ConnectionEnabled", connEnabled)
			);
		}

		public void AddRDSServerToCollection(int serverId, int rdsCollectionId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddRDSServerToCollection",
				 new SqlParameter("@Id", serverId),
				 new SqlParameter("@RDSCollectionId", rdsCollectionId)
			);
		}

		public void AddRDSServerToOrganization(int itemId, int serverId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddRDSServerToOrganization",
				 new SqlParameter("@Id", serverId),
				 new SqlParameter("@ItemID", itemId)
			);
		}

		public void RemoveRDSServerFromOrganization(int serverId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "RemoveRDSServerFromOrganization",
				 new SqlParameter("@Id", serverId)
			);
		}

		public void RemoveRDSServerFromCollection(int serverId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "RemoveRDSServerFromCollection",
				 new SqlParameter("@Id", serverId)
			);
		}

		public IDataReader GetRDSCollectionUsersByRDSCollectionId(int id)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSCollectionUsersByRDSCollectionId",
				 new SqlParameter("@id", id)
			);
		}

		public void AddRDSUserToRDSCollection(int rdsCollectionId, int accountId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddUserToRDSCollection",
				 new SqlParameter("@RDSCollectionId", rdsCollectionId),
				 new SqlParameter("@AccountID", accountId)
			);
		}

		public void RemoveRDSUserFromRDSCollection(int rdsCollectionId, int accountId)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "RemoveRDSUserFromRDSCollection",
				 new SqlParameter("@RDSCollectionId", rdsCollectionId),
				 new SqlParameter("@AccountID", accountId)
			);
		}

		public int GetRDSControllerServiceIDbyFQDN(string fqdnName)
		{
			SqlParameter prmController = new SqlParameter("@Controller", SqlDbType.Int);
			prmController.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetRDSControllerServiceIDbyFQDN",
				 new SqlParameter("@RdsfqdnName", fqdnName),
				 prmController);

			return Convert.ToInt32(prmController.Value);
		}

		#endregion

		#region MX|NX Services

		public IDataReader GetAllPackages()
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetAllPackages"
			);
		}

		public IDataReader GetDomainDnsRecords(int domainId, DnsRecordType recordType)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetDomainDnsRecords",
				 new SqlParameter("@DomainId", domainId),
				 new SqlParameter("@RecordType", recordType)
			);
		}

		public IDataReader GetDomainAllDnsRecords(int domainId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetDomainAllDnsRecords",
				 new SqlParameter("@DomainId", domainId)
			);
		}

		public void AddDomainDnsRecord(DnsRecordInfo domainDnsRecord)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddDomainDnsRecord",
				 new SqlParameter("@DomainId", domainDnsRecord.DomainId),
				 new SqlParameter("@RecordType", domainDnsRecord.RecordType),
				 new SqlParameter("@DnsServer", domainDnsRecord.DnsServer),
				 new SqlParameter("@Value", domainDnsRecord.Value),
				 new SqlParameter("@Date", domainDnsRecord.Date)
			);
		}

		public IDataReader GetScheduleTaskEmailTemplate(string taskId)
		{
			return SqlHelper.ExecuteReader(
					  ConnectionString,
					  CommandType.StoredProcedure,
					  "GetScheduleTaskEmailTemplate",
					  new SqlParameter("@taskId", taskId)
				 );
		}

		public void DeleteDomainDnsRecord(int id)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteDomainDnsRecord",
				 new SqlParameter("@Id", id)
			);
		}

		public void UpdateDomainCreationDate(int domainId, DateTime date)
		{
			UpdateDomainDate(domainId, "UpdateDomainCreationDate", date);
		}

		public void UpdateDomainExpirationDate(int domainId, DateTime date)
		{
			UpdateDomainDate(domainId, "UpdateDomainExpirationDate", date);
		}

		public void UpdateDomainLastUpdateDate(int domainId, DateTime date)
		{
			UpdateDomainDate(domainId, "UpdateDomainLastUpdateDate", date);
		}

		private void UpdateDomainDate(int domainId, string stroredProcedure, DateTime date)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 stroredProcedure,
				 new SqlParameter("@DomainId", domainId),
				 new SqlParameter("@Date", date)
			);
		}

		public void UpdateDomainDates(int domainId, DateTime? domainCreationDate, DateTime? domainExpirationDate, DateTime? domainLastUpdateDate)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateDomainDates",
				 new SqlParameter("@DomainId", domainId),
				 new SqlParameter("@DomainCreationDate", domainCreationDate),
				 new SqlParameter("@DomainExpirationDate", domainExpirationDate),
				 new SqlParameter("@DomainLastUpdateDate", domainLastUpdateDate)
			);
		}

		public void UpdateWhoisDomainInfo(int domainId, DateTime? domainCreationDate, DateTime? domainExpirationDate, DateTime? domainLastUpdateDate, string registrarName)
		{
			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "UpdateWhoisDomainInfo",
				 new SqlParameter("@DomainId", domainId),
				 new SqlParameter("@DomainCreationDate", domainCreationDate),
				 new SqlParameter("@DomainExpirationDate", domainExpirationDate),
				 new SqlParameter("@DomainLastUpdateDate", domainLastUpdateDate),
				 new SqlParameter("@DomainRegistrarName", registrarName)
			);
		}

		#endregion

		#region Organization Storage Space Folders
		public IDataReader GetOrganizationStoragSpaceFolders(int itemId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOrganizationStoragSpaceFolders",
				 new SqlParameter("@ItemId", itemId)
			);
		}

		public IDataReader GetOrganizationStoragSpacesFolderByType(int itemId, string type)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOrganizationStoragSpacesFolderByType",
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@Type", type)
			);
		}

		public void DeleteOrganizationStoragSpacesFolder(int id)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				 CommandType.StoredProcedure,
				 "DeleteOrganizationStoragSpacesFolder",
				 new SqlParameter("@ID", id));
		}

		public int AddOrganizationStoragSpacesFolder(int itemId, string type, int storageSpaceFolderId)
		{
			SqlParameter outParam = new SqlParameter("@ID", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddOrganizationStoragSpacesFolder",
				 outParam,
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@Type", type),
				 new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId)
			);

			return Convert.ToInt32(outParam.Value);
		}

		public IDataReader GetOrganizationStorageSpacesFolderById(int itemId, int folderId)
		{
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetOrganizationStorageSpacesFolderById",
				 new SqlParameter("@ItemId", itemId),
				 new SqlParameter("@ID", folderId)
			);
		}
		#endregion

		#region RDS Messages        

		public DataSet GetRDSMessagesByCollectionId(int rdsCollectionId)
		{
			return SqlHelper.ExecuteDataset(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetRDSMessages",
				 new SqlParameter("@RDSCollectionId", rdsCollectionId)
			);
		}

		public int AddRDSMessage(int rdsCollectionId, string messageText, string userName)
		{
			SqlParameter rdsMessageId = new SqlParameter("@RDSMessageID", SqlDbType.Int);
			rdsMessageId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "AddRDSMessage",
				 rdsMessageId,
				 new SqlParameter("@RDSCollectionId", rdsCollectionId),
				 new SqlParameter("@MessageText", messageText),
				 new SqlParameter("@UserName", userName),
				 new SqlParameter("@Date", DateTime.Now)
			);

			return Convert.ToInt32(rdsMessageId.Value);
		}

		#endregion

	}
}

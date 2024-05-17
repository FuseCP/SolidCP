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
using SolidCP.EnterpriseServer.Data;
using Twilio.Base;
using System.Net;
using static Mysqlx.Notice.Warning.Types;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for DataProvider.
	/// </summary>
	public class DataProvider : Data.DbContext
	{
#if UseEntityFramework
		public bool UseEntityFramework => !IsMsSql || !HasProcedures;
#else
		public const bool UseEntityFramework = false;
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
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetSystemSettings]
	@SettingsName nvarchar(50)
AS
BEGIN

	SET NOCOUNT ON;

    SELECT
		[PropertyName],
		[PropertyValue]
	FROM
		[dbo].[SystemSettings]
	WHERE
		[SettingsName] = @SettingsName;

END
			*/
				#endregion

				return EntityDataReader(SystemSettings.Where(s => s.SettingsName == settingsName));
			}
			return SqlHelper.ExecuteReader(
				 ConnectionString,
				 CommandType.StoredProcedure,
				 "GetSystemSettings",
				 new SqlParameter("@SettingsName", settingsName)
			);
		}

		public void SetSystemSettings(string settingsName, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[SetSystemSettings]
	@SettingsName nvarchar(50),
	@Xml ntext
AS
BEGIN
/*
XML Format:
<properties>
	<property name="" value=""/>
</properties>
*//*
				SET NOCOUNT ON;

				BEGIN TRAN
		DECLARE @idoc int;
				--Create an internal representation of the XML document.
				EXEC sp_xml_preparedocument @idoc OUTPUT, @xml;

		DELETE FROM[dbo].[SystemSettings] WHERE[SettingsName] = @SettingsName;

		INSERT INTO[dbo].[SystemSettings]
		(
			[SettingsName],
			[PropertyName],
			[PropertyValue]
		)
		SELECT
			@SettingsName,
			[XML].[PropertyName],
			[XML].[PropertyValue]
		FROM OPENXML(@idoc, '/properties/property',1) WITH
		(
			[PropertyName] nvarchar(50) '@name',
			[PropertyValue] ntext '@value'
		) AS XML;

		-- remove document
		EXEC sp_xml_removedocument @idoc;

	COMMIT TRAN;

		END
						*/
				#endregion

				var properties = XElement.Parse(xml);
				bool hasChanges = false;
				foreach (var property in properties.Elements())
				{
					var setting = new Data.Entities.SystemSetting()
					{
						SettingsName = settingsName,
						PropertyName = (string)property.Attribute("name"),
						PropertyValue = (string)property.Attribute("value")
					};
					SystemSettings.Add(setting);
					hasChanges = true;
				}
				if (hasChanges) SaveChanges();
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetThemes]
AS
BEGIN
	SET NOCOUNT ON;
    SELECT
		ThemeID,
		DisplayName,
		LTRName,
		RTLName,
		DisplayOrder
	FROM
		Themes
	WHERE
		Enabled = '1'
	ORDER BY 
		DisplayOrder;
END
				*/
				#endregion

				return EntityDataSet(
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetThemeSettings]
(
	@ThemeID int
)
AS
BEGIN
	SET NOCOUNT ON;
    SELECT
		ThemeID,
		SettingsName,
		PropertyName,
		PropertyValue
	FROM
		ThemeSettings
	WHERE
		ThemeID = @ThemeID;
END
				*/
				#endregion

				return EntityDataSet(
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetThemeSetting]
(
	@ThemeID int,
	@SettingsName NVARCHAR(255)
)
AS
BEGIN
	SET NOCOUNT ON;
    SELECT
		ThemeID,
		SettingsName,
		PropertyName,
		PropertyValue
	FROM
		ThemeSettings
	WHERE
		ThemeID = @ThemeID
		AND SettingsName = @SettingsName;
END
				*/
				#endregion

				return EntityDataSet(
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
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CheckActorUserRights]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1 OR @UserID IS NULL
RETURN 1

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
SET @ActorID = @OwnerID

-- check if the user requests his owner
/*
IF @ActorID = @UserID
BEGIN
	RETURN 0
END
*//*
			IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL--reset var

	--get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL --the last parent
		BREAK

	IF @ParentUserID = @ActorID
	RETURN 1

	SET @TmpUserID = @ParentUserID
END

RETURN 0
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserSettings]
(
	@ActorID int,
	@UserID int,
	@SettingsName nvarchar(50)
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- find which parent package has overriden NS
DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	IF EXISTS
	(
		SELECT PropertyName FROM UserSettings
		WHERE SettingsName = @SettingsName AND UserID = @TmpUserID
	)
	BEGIN
		SELECT
			UserID,
			PropertyName,
			PropertyValue
		FROM
			UserSettings
		WHERE
			UserID = @TmpUserID AND
			SettingsName = @SettingsName

		BREAK
	END

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
	BREAK

	SET @TmpUserID = @ParentUserID
END

RETURN
				*/
				#endregion

				const string SettingsName = "Theme";
				if (!CheckActorUserRights(actorId, userId))
					throw new AccessViolationException("You are not allowed to access this account");

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
				return EntityDataSet(setting);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateUserThemeSetting]
(
	@ActorID int,
	@UserID int,
	@PropertyName NVARCHAR(255),
	@PropertyValue NVARCHAR(255)
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

BEGIN
-- Update if present
IF EXISTS ( SELECT * FROM UserSettings 
						WHERE UserID = @UserID
						AND SettingsName = N'Theme'
						AND PropertyName = @PropertyName)
		BEGIN
			UPDATE UserSettings SET	PropertyValue = @PropertyValue
				WHERE UserID = @UserID
				AND SettingsName = N'Theme'
				AND PropertyName = @PropertyName
			Return
		END
	ELSE
		BEGIN
			INSERT UserSettings (UserID, SettingsName, PropertyName, PropertyValue) VALUES (@UserID, N'Theme', @PropertyName, @PropertyValue)
		END
END
				*/
				#endregion

				const string SettingsName = "Theme";

				if (!CheckActorUserRights(actorId, userId))
					throw new AccessViolationException("You are not allowed to access this account");

				var setting = UserSettings.FirstOrDefault(s => s.UserId == userId &&
					s.SettingsName == SettingsName &&
					s.PropertyName == PropertyName);
				if (setting != null)
				{
					setting.PropertyValue = PropertyValue;
				}
				else
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteUserThemeSetting]
(
	@ActorID int,
	@UserID int,
	@PropertyName NVARCHAR(255)
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DELETE FROM UserSettings
WHERE UserID = @UserID
AND SettingsName = N'Theme'
AND PropertyName = @PropertyName

RETURN
				*/
				#endregion

				if (!CheckActorUserRights(actorId, userId))
					throw new AccessViolationException("You are not allowed to access this account");

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[CheckUserExists]
(
	@Exists bit OUTPUT,
	@Username nvarchar(100)
)
AS

SET @Exists = 0

IF EXISTS (SELECT UserID FROM Users
WHERE Username = @Username)
SET @Exists = 1

RETURN
				*/
				#endregion

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

		public List<int> UserParents(int actorId, int userId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[UserParents]
(
	@ActorID int,
	@UserID int
)
RETURNS @T TABLE (UserOrder int IDENTITY(1,1), UserID int)
AS
BEGIN
	-- insert current user
	INSERT @T VALUES (@UserID)

	DECLARE @TopUserID int
	IF @ActorID = -1
	BEGIN
		SELECT @TopUserID = UserID FROM Users WHERE OwnerID IS NULL
	END
	ELSE
	BEGIN
		SET @TopUserID = @ActorID

		IF EXISTS (SELECT UserID FROM Users WHERE UserID = @ActorID AND IsPeer = 1)
		SELECT @TopUserID = OwnerID FROM Users WHERE UserID = @ActorID AND IsPeer = 1
	END

	-- owner
	DECLARE @OwnerID int, @TmpUserID int

	SET @TmpUserID = @UserID

	WHILE (@TmpUserID <> @TopUserID)
	BEGIN

		SET @OwnerID = NULL
		SELECT @OwnerID = OwnerID FROM Users WHERE UserID = @TmpUserID

		IF @OwnerID IS NOT NULL
		BEGIN
			INSERT @T VALUES (@OwnerID)
			SET @TmpUserID = @OwnerID
		END
	END

RETURN
END
			*/
			#endregion

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

		public bool CheckUserParent(int ownerId, int userId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CheckUserParent]
(
	@OwnerID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

-- check if the user requests himself
IF @OwnerID = @UserID
BEGIN
	RETURN 1
END

-- check if the owner is peer
DECLARE @IsPeer int, @TmpOwnerID int
SELECT @IsPeer = IsPeer, @TmpOwnerID = OwnerID FROM Users
WHERE UserID = @OwnerID

IF @IsPeer = 1
SET @OwnerID = @TmpOwnerID

-- check if the user requests himself
IF @OwnerID = @UserID
BEGIN
	RETURN 1
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK

	IF @ParentUserID = @OwnerID
	RETURN 1

	SET @TmpUserID = @ParentUserID
END

RETURN 0
END
			*/
			#endregion

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

		public string GetItemComments(int itemId, string itemTypeId, int actorId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[GetItemComments]
(
	@ItemID int,
	@ItemTypeID varchar(50),
	@ActorID int
)
RETURNS nvarchar(3000)
AS
BEGIN
DECLARE @text nvarchar(3000)
SET @text = ''

SELECT @text = @text + U.Username + ' - ' + CONVERT(nvarchar(50), C.CreatedDate) + '
' + CommentText + '
--------------------------------------
' FROM Comments AS C
INNER JOIN UsersDetailed AS U ON C.UserID = U.UserID
WHERE
	ItemID = @ItemID
	AND ItemTypeID = @ItemTypeID
	AND dbo.CheckUserParent(@ActorID, C.UserID) = 1
ORDER BY C.CreatedDate DESC

RETURN @text
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUsersPaged]
(
	@ActorID int,
	@UserID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@StatusID int,
	@RoleID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Recursive bit
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '

DECLARE @HasUserRights bit
SET @HasUserRights = dbo.CheckActorUserRights(@ActorID, @UserID)

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Users TABLE
(
	ItemPosition int IDENTITY(0,1),
	UserID int
)
INSERT INTO @Users (UserID)
SELECT
	U.UserID
FROM UsersDetailed AS U
WHERE 
	U.UserID <> @UserID AND U.IsPeer = 0 AND
	(
		(@Recursive = 0 AND OwnerID = @UserID) OR
		(@Recursive = 1 AND dbo.CheckUserParent(@UserID, U.UserID) = 1)
	)
	AND ((@StatusID = 0) OR (@StatusID > 0 AND U.StatusID = @StatusID))
	AND ((@RoleID = 0) OR (@RoleID > 0 AND U.RoleID = @RoleID))
	AND @HasUserRights = 1 '

IF @FilterValue <> ''
BEGIN
	IF @FilterColumn <> ''
		SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '
	ELSE
		SET @sql = @sql + '
			AND (Username LIKE @FilterValue
			OR FullName LIKE @FilterValue
			OR Email LIKE @FilterValue) '
END

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(UserID) FROM @Users;
SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.SubscriberNumber,
	U.LoginStatusId,
	U.FailedLogins,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	dbo.GetItemComments(U.UserID, ''USER'', @ActorID) AS Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.FullName,
	U.OwnerUsername,
	U.OwnerFirstName,
	U.OwnerLastName,
	U.OwnerRoleID,
	U.OwnerFullName,
	U.OwnerEmail,
	U.PackagesNumber,
	U.CompanyName,
	U.EcommerceEnabled
FROM @Users AS TU
INNER JOIN UsersDetailed AS U ON TU.UserID = U.UserID
WHERE TU.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ActorID int, @Recursive bit, @StatusID int, @RoleID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ActorID, @Recursive, @StatusID, @RoleID

RETURN
				*/
				#endregion

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
					}
					else
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

				return EntityDataSet(users.Select(u => new
				{
					u.UserId,
					u.RoleId,
					u.StatusId,
					u.SubscriberNumber,
					u.LoginStatusId,
					u.FailedLogins,
					u.OwnerId,
					u.Created,
					u.Changed,
					u.IsDemo,
					Comments = GetItemComments(u.UserId, "USER", actorId),
					u.IsPeer,
					u.Username,
					u.FirstName,
					u.LastName,
					u.Email,
					u.FullName,
					u.OwnerUsername,
					u.OwnerFirstName,
					u.OwnerLastName,
					u.OwnerRoleId,
					u.OwnerFullName,
					u.OwnerEmail,
					u.PackagesNumber,
					u.CompanyName,
					u.EcommerceEnabled
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetSearchObject]
(
	@ActorID int,
	@UserID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@StatusID int,
	@RoleID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int = 0,
	@Recursive bit,
	@ColType nvarchar(500) = '',
	@FullType nvarchar(50) = '',
	@OnlyFind bit
)
AS

IF @ColType IS NULL
	SET @ColType = ''

DECLARE @HasUserRights bit
SET @HasUserRights = dbo.CheckActorUserRights(@ActorID, @UserID)

IF @HasUserRights = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @curAll CURSOR
DECLARE @curUsers CURSOR
DECLARE @ItemID int
DECLARE @TextSearch nvarchar(500)
DECLARE @ColumnType nvarchar(50)
DECLARE @FullTypeAll nvarchar(50)
DECLARE @PackageID int
DECLARE @AccountID int
DECLARE @Username nvarchar(50)
DECLARE @Fullname nvarchar(50)
DECLARE @ItemsAll TABLE
 (
  ItemID int,
  TextSearch nvarchar(500),
  ColumnType nvarchar(50),
  FullType nvarchar(50),
  PackageID int,
  AccountID int,
  Username nvarchar(100),
  Fullname nvarchar(100)
 )
DECLARE @sql nvarchar(max)

/*------------------------------------------------Users---------------------------------------------------------------*//*
DECLARE @columnUsername nvarchar(20)
SET @columnUsername = 'Username'

DECLARE @columnEmail nvarchar(20)
SET @columnEmail = 'Email'

DECLARE @columnCompanyName nvarchar(20)
SET @columnCompanyName = 'CompanyName'

DECLARE @columnFullName nvarchar(20)
SET @columnFullName = 'FullName'

IF @FilterColumn = '' AND @FilterValue<> ''
SET @FilterColumn = 'TextSearch'

SET @sql = '
DECLARE @Users TABLE
(
 ItemPosition int IDENTITY(0, 1),
 UserID int,
 Username nvarchar(100),
 Fullname nvarchar(100)
)
INSERT INTO @Users(UserID, Username, Fullname)
SELECT
 U.UserID,
 U.Username,
 U.FirstName + '' '' + U.LastName as Fullname
FROM UsersDetailed AS U
WHERE
 U.UserID<> @UserID AND U.IsPeer = 0 AND
 (
  (@Recursive = 0 AND OwnerID = @UserID) OR
  (@Recursive = 1 AND dbo.CheckUserParent(@UserID, U.UserID) = 1)
 )
 AND((@StatusID = 0) OR(@StatusID > 0 AND U.StatusID = @StatusID))
 AND((@RoleID = 0) OR(@RoleID > 0 AND U.RoleID = @RoleID))
 AND ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
 SET @curValue = cursor local for
SELECT '

IF @OnlyFind = 1
	SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + 'U.ItemID,
 U.TextSearch,
 U.ColumnType,
 ''AccountHome'' as FullType,
 0 as PackageID,
 0 as AccountID,
 TU.Username,
 TU.Fullname
FROM @Users AS TU
INNER JOIN
(
SELECT ItemID, TextSearch, ColumnType
FROM(
SELECT U0.UserID as ItemID, U0.Username as TextSearch, @columnUsername as ColumnType
FROM dbo.Users AS U0
UNION
SELECT U1.UserID as ItemID, U1.Email as TextSearch, @columnEmail as ColumnType
FROM dbo.Users AS U1
UNION
SELECT U2.UserID as ItemID, U2.CompanyName as TextSearch, @columnCompanyName as ColumnType
FROM dbo.Users AS U2
UNION
SELECT U3.UserID as ItemID, U3.FirstName + '' '' + U3.LastName as TextSearch, @columnFullName as ColumnType
FROM dbo.Users AS U3) as U
WHERE TextSearch<>'' '' OR ISNULL(TextSearch, 0) > 0
)
 AS U ON TU.UserID = U.ItemID'
IF @FilterValue<> ''
 SET @sql = @sql + ' WHERE TextSearch LIKE ''' + @FilterValue + ''''
SET @sql = @sql + ' ORDER BY TextSearch'

SET @sql = @sql + ';open @curValue'

exec sp_executesql @sql, N'@UserID int, @FilterValue nvarchar(50), @Recursive bit, @StatusID int, @RoleID int, @columnUsername nvarchar(20), @columnEmail nvarchar(20), @columnCompanyName nvarchar(20), @columnFullName nvarchar(20), @curValue cursor output',
@UserID, @FilterValue, @Recursive, @StatusID, @RoleID, @columnUsername, @columnEmail, @columnCompanyName, @columnFullName, @curUsers output

/*--------------------------------------------Space----------------------------------------------------------*//*
DECLARE @sqlNameAccountType nvarchar(4000)
SET @sqlNameAccountType = '
WHEN 1 THEN ''Mailbox''
WHEN 2 THEN ''Contact''
WHEN 3 THEN ''DistributionList''
WHEN 4 THEN ''PublicFolder''
WHEN 5 THEN ''Room''
WHEN 6 THEN ''Equipment''
WHEN 7 THEN ''User''
WHEN 8 THEN ''SecurityGroup''
WHEN 9 THEN ''DefaultSecurityGroup''
WHEN 10 THEN ''SharedMailbox''
WHEN 11 THEN ''DeletedUser''
WHEN 12 THEN ''JournalingMailbox''
'

SET @sql = '
 DECLARE @ItemsService TABLE
 (
  ItemID int,
  ItemTypeID int,
  Username nvarchar(100),
  Fullname nvarchar(100)
 )
 INSERT INTO @ItemsService(ItemID, ItemTypeID, Username, Fullname)
 SELECT
  SI.ItemID,
  SI.ItemTypeID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
 FROM ServiceItems AS SI
 INNER JOIN Packages AS P ON P.PackageID = SI.PackageID
 INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
 WHERE
  dbo.CheckUserParent(@UserID, P.UserID) = 1
 DECLARE @ItemsDomain TABLE
 (
  ItemID int,
  Username nvarchar(100),
  Fullname nvarchar(100)
 )
 INSERT INTO @ItemsDomain(ItemID, Username, Fullname)
 SELECT
  D.DomainID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
 FROM Domains AS D
 INNER JOIN Packages AS P ON P.PackageID = D.PackageID
 INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
 WHERE
  dbo.CheckUserParent(@UserID, P.UserID) = 1

 SET @curValue = cursor local for
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  SI.ItemID as ItemID,
  SI.ItemName as TextSearch,
  STYPE.DisplayName as ColumnType,
  STYPE.DisplayName as FullType,
  SI.PackageID as PackageID,
  0 as AccountID,
  I.Username,
  I.Fullname
 FROM @ItemsService AS I
 INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
 INNER JOIN ServiceItemTypes AS STYPE ON SI.ItemTypeID = STYPE.ItemTypeID
 WHERE(STYPE.Searchable = 1
 AND STYPE.ItemTypeID<> 200 AND STYPE.ItemTypeID<> 201)'
IF @FilterValue<> ''
 SET @sql = @sql + ' AND (SI.ItemName LIKE ''' + @FilterValue + ''')'
SET @sql = @sql + '
 UNION(
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  D.DomainID AS ItemID,
  D.DomainName as TextSearch,
  ''Domain'' as ColumnType,
  ''Domains'' as FullType,
  D.PackageID as PackageID,
  0 as AccountID,
  I.Username,
  I.Fullname
 FROM @ItemsDomain AS I
 INNER JOIN Domains AS D ON I.ItemID = D.DomainID
 WHERE(D.IsDomainPointer = 0)'
IF @FilterValue<> ''
 SET @sql = @sql + ' AND (D.DomainName LIKE ''' + @FilterValue + ''')'
SET @sql = @sql + '
 UNION
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  EA.ItemID AS ItemID,
  EA.DisplayName as TextSearch,
  ''ExchangeAccount'' as ColumnType,
  FullType = CASE EA.AccountType ' + @sqlNameAccountType + ' ELSE CAST(EA.AccountType AS varchar(12)) END,
  SI2.PackageID as PackageID,
  EA.AccountID as AccountID,
  I2.Username,
  I2.Fullname
 FROM @ItemsService AS I2
 INNER JOIN ServiceItems AS SI2 ON I2.ItemID = SI2.ItemID
 INNER JOIN ExchangeAccounts AS EA ON I2.ItemID = EA.ItemID'
IF @FilterValue<> ''
 SET @sql = @sql + ' WHERE (EA.DisplayName LIKE ''' + @FilterValue + ''')'
SET @sql = @sql + '
 UNION
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  EA4.ItemID AS ItemID,
  EA4.PrimaryEmailAddress as TextSearch,
  ''ExchangeAccount'' as ColumnType,
  FullType = CASE EA4.AccountType ' + @sqlNameAccountType + ' ELSE CAST(EA4.AccountType AS varchar(12)) END,
  SI4.PackageID as PackageID,
  EA4.AccountID as AccountID,
  I4.Username,
  I4.Fullname
 FROM @ItemsService AS I4
 INNER JOIN ServiceItems AS SI4 ON I4.ItemID = SI4.ItemID
 INNER JOIN ExchangeAccounts AS EA4 ON I4.ItemID = EA4.ItemID'
IF @FilterValue<> ''
 SET @sql = @sql + ' WHERE (EA4.PrimaryEmailAddress LIKE ''' + @FilterValue + ''')'
SET @sql = @sql + '
 UNION
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  I3.ItemID AS ItemID,
  EAEA.EmailAddress as TextSearch,
  ''ExchangeAccount'' as ColumnType,
  FullType = CASE EA.AccountType ' + @sqlNameAccountType + ' ELSE CAST(EA.AccountType AS varchar(12)) END,
  SI3.PackageID as PackageID,
  EAEA.AccountID as AccountID,
  I3.Username,
  I3.Fullname
 FROM @ItemsService AS I3
 INNER JOIN ServiceItems AS SI3 ON I3.ItemID = SI3.ItemID
 INNER JOIN ExchangeAccounts AS EA ON I3.ItemID = EA.ItemID
 INNER JOIN ExchangeAccountEmailAddresses AS EAEA ON EA.AccountID = EAEA.AccountID
 WHERE I3.ItemTypeID = 29'
IF @FilterValue<> ''
 SET @sql = @sql + ' AND (EAEA.EmailAddress LIKE ''' + @FilterValue + ''')'
 SET @sql = @sql + ')'
IF @OnlyFind = 1
	SET @sql = @sql + ' ORDER BY TextSearch';

SET @sql = @sql + ';open @curValue'

exec sp_executesql @sql, N'@UserID int, @FilterValue nvarchar(50), @curValue cursor output',
@UserID, @FilterValue, @curAll output

FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
END

/*-------------------------------------------Lync-----------------------------------------------------*//*
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SET @sql = '
SET @curValue = cursor local for
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  SI.ItemID as ItemID,
  ea.AccountName as TextSearch,
  ''LyncAccount'' as ColumnType,
  ''LyncUsers'' as FullType,
  SI.PackageID as PackageID,
  ea.AccountID as AccountID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
 FROM
  ExchangeAccounts as ea
 INNER JOIN
  LyncUsers as LU
 INNER JOIN
  LyncUserPlans as lp
  ON
  LU.LyncUserPlanId = lp.LyncUserPlanId
 ON
  ea.AccountID = LU.AccountID
 INNER JOIN
  ServiceItems AS SI ON ea.ItemID = SI.ItemID
 INNER JOIN
  Packages AS P ON SI.PackageID = P.PackageID
 INNER JOIN
  Users AS U ON U.UserID = P.UserID
WHERE ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
  AND(' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
IF @FilterValue<> ''
 SET @sql = @sql + ' AND ea.AccountName LIKE ''' + @FilterValue + ''''
IF @OnlyFind = 1
	SET @sql = @sql + ' ORDER BY TextSearch'
SET @sql = @sql + ' ;open @curValue'

CLOSE @curAll
DEALLOCATE @curAll
exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
END

/*-------------------------------------------SfB-----------------------------------------------------*//*

SET @sql = '
SET @curValue = cursor local for
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  SI.ItemID as ItemID,
  ea.AccountName as TextSearch,
  ''SfBAccount'' as ColumnType,
  ''SfBUsers'' as FullType,
  SI.PackageID as PackageID,
  ea.AccountID as AccountID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
 FROM
  ExchangeAccounts as ea
 INNER JOIN
  SfBUsers as LU
 INNER JOIN
  SfBUserPlans as lp
  ON
  LU.SfBUserPlanId = lp.SfBUserPlanId
 ON
  ea.AccountID = LU.AccountID
 INNER JOIN
  ServiceItems AS SI ON ea.ItemID = SI.ItemID
 INNER JOIN
  Packages AS P ON SI.PackageID = P.PackageID
 INNER JOIN
  Users AS U ON U.UserID = P.UserID
WHERE ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
  AND(' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
IF @FilterValue<> ''
 SET @sql = @sql + ' AND ea.AccountName LIKE ''' + @FilterValue + ''''
IF @OnlyFind = 1
	SET @sql = @sql + ' ORDER BY TextSearch'
SET @sql = @sql + ' ;open @curValue'

CLOSE @curAll
DEALLOCATE @curAll
exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
END

/*------------------------------------RDS------------------------------------------------*//*
IF @IsAdmin = 1
BEGIN
	SET @sql = '
	SET @curValue = cursor local for
	 SELECT '

	IF @OnlyFind = 1
	SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

	SET @sql = @sql + '
	  RDSCol.ItemID as ItemID,
	  RDSCol.Name as TextSearch,
	  ''RDSCollection'' as ColumnType,
	  ''RDSCollections'' as FullType,
	  P.PackageID as PackageID,
	  RDSCol.ID as AccountID,
	  U.Username,
	  U.FirstName + '' '' + U.LastName as Fullname
	 FROM
	  RDSCollections AS RDSCol
	 INNER JOIN
	  ServiceItems AS SI ON RDSCol.ItemID = SI.ItemID
	 INNER JOIN
	  Packages AS P ON SI.PackageID = P.PackageID
	 INNER JOIN
	  Users AS U ON U.UserID = P.UserID
	 WHERE ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
	 AND(' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
	IF @FilterValue<> ''
		SET @sql = @sql + ' AND RDSCol.Name LIKE ''' + @FilterValue + ''''
	IF @OnlyFind = 1
		SET @sql = @sql + ' ORDER BY TextSearch'
	SET @sql = @sql + ' ;open @curValue'

	CLOSE @curAll
	DEALLOCATE @curAll
	exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

	FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
	WHILE @@FETCH_STATUS = 0
	BEGIN
	INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
	VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
	FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
	END
END

/*------------------------------------CRM------------------------------------------------*//*
SET @sql = '
SET @curValue = cursor local for
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  @UserID as ItemID,
  ea.AccountName as TextSearch,
  ''CRMSite'' as ColumnType,
  ''CRMSites'' as FullType,
  SI.PackageID as PackageID,
  ea.AccountID as AccountID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
 FROM
  ExchangeAccounts as ea
 INNER JOIN
  CRMUsers AS CRMU ON ea.AccountID = CRMU.AccountID
 INNER JOIN
  ServiceItems AS SI ON ea.ItemID = SI.ItemID
 INNER JOIN
  Packages AS P ON SI.PackageID = P.PackageID
 INNER JOIN
  Users AS U ON U.UserID = P.UserID
 WHERE ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
  AND(' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
IF @FilterValue<> ''
	SET @sql = @sql + ' AND ea.AccountName LIKE ''' + @FilterValue + ''''
IF @OnlyFind = 1
	SET @sql = @sql + ' ORDER BY TextSearch'
SET @sql = @sql + ' ;open @curValue'

CLOSE @curAll
DEALLOCATE @curAll
exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
END

/*------------------------------------VirtualServer------------------------------------------------*//*
IF @IsAdmin = 1
BEGIN
	SET @sql = '
	SET @curValue = cursor local for
	 SELECT '

	IF @OnlyFind = 1
	SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

	SET @sql = @sql + '
	  @UserID as ItemID,
	  S.ServerName as TextSearch,
	  ''VirtualServer'' as ColumnType,
	  ''VirtualServers'' as FullType,
	  (SELECT MIN(PackageID) FROM Packages WHERE UserID = @UserID) as PackageID,
	  0 as AccountID,
	  U.Username,
	  U.FirstName + '' '' + U.LastName as Fullname
	 FROM
	  Servers AS S
	 INNER JOIN

	  Packages AS P ON P.ServerID = S.ServerID

	 INNER JOIN

	  Users AS U ON U.UserID = P.UserID
	 WHERE
	  VirtualServer = 1'
	IF @FilterValue<> ''
		SET @sql = @sql + ' AND S.ServerName LIKE ''' + @FilterValue + ''''
	IF @OnlyFind = 1
		SET @sql = @sql + ' ORDER BY TextSearch'
	SET @sql = @sql + ' ;open @curValue'

	CLOSE @curAll
	DEALLOCATE @curAll
	exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

	FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
	WHILE @@FETCH_STATUS = 0
	BEGIN
	INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
	VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
	FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
	END
END

/*------------------------------------WebDAVFolder------------------------------------------------*//*
SET @sql = '
SET @curValue = cursor local for
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  EF.ItemID as ItemID,
  EF.FolderName as TextSearch,
  ''WebDAVFolder'' as ColumnType,
  ''Folders'' as FullType,
  P.PackageID as PackageID,
  EF.EnterpriseFolderID as AccountID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
 FROM
  EnterpriseFolders as EF
 INNER JOIN
  ServiceItems AS SI ON EF.ItemID = SI.ItemID
 INNER JOIN
  Packages AS P ON SI.PackageID = P.PackageID
 INNER JOIN
  Users AS U ON U.UserID = P.UserID
 WHERE ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
  AND(' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
IF @FilterValue<> ''
	SET @sql = @sql + ' AND EF.FolderName LIKE ''' + @FilterValue + ''''
IF @OnlyFind = 1
	SET @sql = @sql + ' ORDER BY TextSearch'
SET @sql = @sql + ';open @curValue'

CLOSE @curAll
DEALLOCATE @curAll
exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
END

/*------------------------------------VPS-IP------------------------------------------------*//*
SET @sql = '
SET @curValue = cursor local for
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  SI.ItemID as ItemID,
  SI.ItemName as TextSearch,
  SIT.DisplayName as ColumnType,
  SIT.DisplayName as FullType,
  P.PackageID as PackageID,
  0 as AccountID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
 FROM ServiceItems AS SI
 INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
 INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
 INNER JOIN Users AS U ON U.UserID = P.UserID
 LEFT JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID
 LEFT JOIN PackageIPAddresses AS PACIP ON PACIP.ItemID = SI.ItemID
 LEFT JOIN IPAddresses AS IPS ON IPS.AddressID = PACIP.AddressID
 WHERE SIT.DisplayName = ''VirtualMachine''
  AND ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
  AND dbo.CheckUserParent(@UserID, P.UserID) = 1
  AND(''' + @FilterValue + ''' LIKE '' %.% '' OR ''' + @FilterValue + ''' LIKE '' %:% '')
  AND(PIP.IPAddress LIKE ''' + @FilterValue + ''' OR IPS.ExternalIP LIKE ''' + @FilterValue + ''')'
IF @OnlyFind = 1
	SET @sql = @sql + ' ORDER BY TextSearch'
SET @sql = @sql + ';open @curValue'

CLOSE @curAll
DEALLOCATE @curAll
exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
END

/*------------------------------------SharePoint------------------------------------------------*//*
SET @sql = '
SET @curValue = cursor local for
 SELECT '

IF @OnlyFind = 1
SET @sql = @sql + 'TOP ' + CAST(@MaximumRows AS varchar(12)) + ' '

SET @sql = @sql + '
  SIP.PropertyValue as ItemID,
  T.PropertyValue as TextSearch,
  SIT.DisplayName as ColumnType,
  ''SharePointSiteCollections'' as FullType,
  P.PackageID as PackageID,
  SI.ItemID as AccountID,
  U.Username,
  U.FirstName + '' '' + U.LastName as Fullname
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Users AS U ON U.UserID = P.UserID
INNER JOIN ServiceItemProperties AS SIP ON SIP.ItemID = SI.ItemID
RIGHT JOIN ServiceItemProperties AS T ON T.ItemID = SIP.ItemID
WHERE ' + CAST((@HasUserRights) AS varchar(12)) + ' = 1
AND(' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)
AND(SIT.DisplayName = ''SharePointFoundationSiteCollection''
	OR SIT.DisplayName = ''SharePointEnterpriseSiteCollection'')
AND SIP.PropertyName = ''OrganizationId''
AND T.PropertyName = ''PhysicalAddress'''
IF @FilterValue<> ''
	SET @sql = @sql + ' AND T.PropertyValue LIKE ''' + @FilterValue + ''''
IF @OnlyFind = 1
	SET @sql = @sql + ' ORDER BY TextSearch'
SET @sql = @sql + ';open @curValue'

CLOSE @curAll
DEALLOCATE @curAll
exec sp_executesql @sql, N'@UserID int, @curValue cursor output', @UserID, @curAll output

FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO @ItemsAll(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
VALUES(@ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname)
FETCH NEXT FROM @curAll INTO @ItemID, @TextSearch, @ColumnType, @FullTypeAll, @PackageID, @AccountID, @Username, @Fullname
END

/*-------------------------------------------@curAll-------------------------------------------------------*//*
CLOSE @curAll
DEALLOCATE @curAll
SET @curAll = CURSOR LOCAL FOR
 SELECT
	ItemID,
	TextSearch,
	ColumnType,
	FullType,
	PackageID,
	AccountID,
	Username,
	Fullname
 FROM @ItemsAll
OPEN @curAll

/*-------------------------------------------Return-------------------------------------------------------*//*
IF @SortColumn = ''
	SET @SortColumn = 'TextSearch'

SET @sql = '
DECLARE @ItemID int
DECLARE @TextSearch nvarchar(500)
DECLARE @ColumnType nvarchar(50)
DECLARE @FullType nvarchar(50)
DECLARE @PackageID int
DECLARE @AccountID int
DECLARE @EndRow int
DECLARE @Username nvarchar(100)
DECLARE @Fullname nvarchar(100)
SET @EndRow = @StartRow + @MaximumRows'

IF(@ColType = '' OR @ColType IN('AccountHome'))
BEGIN
	SET @sql = @sql + '
	DECLARE @ItemsUser TABLE
	(
		ItemID int,
		TextSearch nvarchar(500),
		ColumnType nvarchar(50),
		FullType nvarchar(50),
		PackageID int,
		AccountID int,
		Username nvarchar(100),
		Fullname nvarchar(100)
	)

	FETCH NEXT FROM @curUsersValue INTO @ItemID, @TextSearch, @ColumnType, @FullType, @PackageID, @AccountID, @Username, @Fullname
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF(1 = 1)'

	IF @FullType<> ''
		SET @sql = @sql + ' AND @FullType = ''' + @FullType + '''';

	SET @sql = @sql + '
		BEGIN
			INSERT INTO @ItemsUser(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
			VALUES(@ItemID, @TextSearch, @ColumnType, @FullType, @PackageID, @AccountID, @Username, @Fullname)
		END
		FETCH NEXT FROM @curUsersValue INTO @ItemID, @TextSearch, @ColumnType, @FullType, @PackageID, @AccountID, @Username, @Fullname
	END'
END

SET @sql = @sql + '
DECLARE @ItemsFilter TABLE
 (
  ItemID int,
  TextSearch nvarchar(500),
  ColumnType nvarchar(50),
  FullType nvarchar(50),
  PackageID int,
  AccountID int,
  Username nvarchar(100),
  Fullname nvarchar(100)
 )

FETCH NEXT FROM @curAllValue INTO @ItemID, @TextSearch, @ColumnType, @FullType, @PackageID, @AccountID, @Username, @Fullname
WHILE @@FETCH_STATUS = 0
BEGIN
	IF(1 = 1)'

IF @ColType<> ''
SET @sql = @sql + ' AND @ColumnType in ( ' + @ColType + ' ) ';

				IF @FullType<> ''
SET @sql = @sql + ' AND @FullType = ''' + @FullType + '''';

				SET @sql = @sql + '
	BEGIN
		INSERT INTO @ItemsFilter(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
		VALUES(@ItemID, @TextSearch, @ColumnType, @FullType, @PackageID, @AccountID, @Username, @Fullname)
	END
	FETCH NEXT FROM @curAllValue INTO @ItemID, @TextSearch, @ColumnType, @FullType, @PackageID, @AccountID, @Username, @Fullname
END

DECLARE @ItemsReturn TABLE
 (
  ItemPosition int IDENTITY(1, 1),
  ItemID int,
  TextSearch nvarchar(500),
  ColumnType nvarchar(50),
  FullType nvarchar(50),
  PackageID int,
  AccountID int,
  Username nvarchar(100),
  Fullname nvarchar(100)
 )'

IF(@ColType = '' OR @ColType IN('AccountHome'))
BEGIN
	SET @sql = @sql + '
		INSERT INTO '
	IF @SortColumn = 'TextSearch'
		SET @sql = @sql + '@ItemsReturn'
	ELSE
		SET @sql = @sql + '@ItemsFilter'
	SET @sql = @sql + ' (ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
		SELECT ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname
		FROM @ItemsUser'
END

SET @sql = @sql + '
INSERT INTO @ItemsReturn(ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname)
SELECT
	ItemID,
	TextSearch,
	ColumnType,
	FullType,
	PackageID,
	AccountID,
	Username,
	Fullname
FROM @ItemsFilter'
SET @sql = @sql + ' ORDER BY ' + @SortColumn

SET @sql = @sql + ';
SELECT COUNT(ItemID) FROM @ItemsReturn;
				SELECT DISTINCT(ColumnType) FROM @ItemsReturn';
IF @FullType<> ''
	SET @sql = @sql + ' WHERE FullType = ''' + @FullType + '''';

				SET @sql = @sql + ';
SELECT ItemPosition, ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname
FROM @ItemsReturn AS IR'

IF @MaximumRows > 0
	SET @sql = @sql + ' WHERE IR.ItemPosition BETWEEN @StartRow AND @EndRow';

				exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @FilterValue nvarchar(50), @curUsersValue cursor, @curAllValue cursor',
	@StartRow, @MaximumRows, @FilterValue, @curUsers, @curAll

CLOSE @curAll
DEALLOCATE @curAll

RETURN
				*/
				#endregion

				if (!CheckActorUserRights(actorId, userId))
					throw new AccessViolationException("You are not allowed to access this account");

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetSearchTableByColumns]
(
	@PagedStored nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@MaximumRows int,

	@Recursive bit,
	@PoolID int,
	@ServerID int,
	@ActorID int,
	@StatusID int,
	@PlanID int,
	@OrgID int,
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL,
	@PackageID int,
	@VPSType nvarchar(100) = NULL,
	@UserID int,
	@RoleID int,
	@FilterColumns nvarchar(200)
)
AS

DECLARE @VPSTypeID int
IF @VPSType <> '' AND @VPSType IS NOT NULL
BEGIN
	SET @VPSTypeID = CASE @VPSType
		WHEN 'VPS' THEN 33
		WHEN 'VPS2012' THEN 41
		WHEN 'Proxmox' THEN 143
		WHEN 'VPSForPC' THEN 35
		ELSE 33
		END
END

DECLARE @sql nvarchar(3000)
SET @sql = CASE @PagedStored
WHEN 'Domains' THEN '
	DECLARE @Domains TABLE
	(
		DomainID int,
		DomainName nvarchar(100),
		Username nvarchar(100),
		FullName nvarchar(100),
		Email nvarchar(100)
	)
	INSERT INTO @Domains (DomainID, DomainName, Username, FullName, Email)
	SELECT
		D.DomainID,
		D.DomainName,
		U.Username,
		U.FullName,
		U.Email
	FROM Domains AS D
	INNER JOIN Packages AS P ON D.PackageID = P.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
	LEFT OUTER JOIN Services AS S ON Z.ServiceID = S.ServiceID
	LEFT OUTER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
	WHERE
		(D.IsPreviewDomain = 0 AND D.IsDomainPointer = 0)
		AND ((@Recursive = 0 AND D.PackageID = @PackageID)
		OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, D.PackageID) = 1))
		AND (@ServerID = 0 OR (@ServerID > 0 AND S.ServerID = @ServerID))
	'
WHEN 'IPAddresses' THEN '
	DECLARE @IPAddresses TABLE
	(
		AddressesID int,
		ExternalIP nvarchar(100),
		InternalIP nvarchar(100),
		DefaultGateway nvarchar(100),
		ServerName nvarchar(100),
		UserName nvarchar(100),
		ItemName nvarchar(100)
	)
	DECLARE @IsAdmin bit
	SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)
	INSERT INTO @IPAddresses (AddressesID, ExternalIP, InternalIP, DefaultGateway, ServerName, UserName, ItemName)
	SELECT
		IP.AddressID,
		IP.ExternalIP,
		IP.InternalIP,
		IP.DefaultGateway,
		S.ServerName,
		U.UserName,
		SI.ItemName
	FROM dbo.IPAddresses AS IP
	LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
	LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
	LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
	LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
	LEFT JOIN dbo.Users U ON P.UserID = U.UserID
	WHERE
		@IsAdmin = 1
		AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
		AND (@ServerID = 0 OR @ServerID <> 0 AND IP.ServerID = @ServerID)
	'
WHEN 'Schedules' THEN '
	DECLARE @Schedules TABLE
	(
		ScheduleID int,
		ScheduleName nvarchar(100),
		Username nvarchar(100),
		FullName nvarchar(100),
		Email nvarchar(100)
	)
	INSERT INTO @Schedules (ScheduleID, ScheduleName, Username, FullName, Email)
	SELECT
		S.ScheduleID,
		S.ScheduleName,
		U.Username,
		U.FullName,
		U.Email
	FROM Schedule AS S
	INNER JOIN Packages AS P ON S.PackageID = P.PackageID
	INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON S.PackageID = PT.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	'
WHEN 'NestedPackages' THEN '
	DECLARE @NestedPackages TABLE
	(
		PackageID int,
		PackageName nvarchar(100),
		Username nvarchar(100),
		FullName nvarchar(100),
		Email nvarchar(100)
	)
	INSERT INTO @NestedPackages (PackageID, PackageName, Username, FullName, Email)
	SELECT
		P.PackageID,
		P.PackageName,
		U.Username,
		U.FullName,
		U.Email
	FROM Packages AS P
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	INNER JOIN Servers AS S ON P.ServerID = S.ServerID
	INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
	WHERE
		P.ParentPackageID = @PackageID
		AND ((@StatusID = 0) OR (@StatusID > 0 AND P.StatusID = @StatusID))
		AND ((@PlanID = 0) OR (@PlanID > 0 AND P.PlanID = @PlanID))
		AND ((@ServerID = 0) OR (@ServerID > 0 AND P.ServerID = @ServerID))
	'
WHEN 'PackageIPAddresses' THEN '
	DECLARE @PackageIPAddresses TABLE
	(
		PackageAddressID int,
		ExternalIP nvarchar(100),
		InternalIP nvarchar(100),
		DefaultGateway nvarchar(100),
		ItemName nvarchar(100),
		UserName nvarchar(100)
	)
	INSERT INTO @PackageIPAddresses (PackageAddressID, ExternalIP, InternalIP, DefaultGateway, ItemName, UserName)
	SELECT
		PA.PackageAddressID,
		IP.ExternalIP,
		IP.InternalIP,
		IP.DefaultGateway,
		SI.ItemName,
		U.UserName
	FROM dbo.PackageIPAddresses PA
	INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
	INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
	INNER JOIN dbo.Users U ON U.UserID = P.UserID
	LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
	WHERE
		((@Recursive = 0 AND PA.PackageID = @PackageID)
		OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, PA.PackageID) = 1))
		AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
		AND (@OrgID = 0 OR @OrgID <> 0 AND PA.OrgID = @OrgID)
	'
WHEN 'ServiceItems' THEN '
	IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	DECLARE @ServiceItems TABLE
	(
		ItemID int,
		ItemName nvarchar(100),
		Username nvarchar(100),
		FullName nvarchar(100),
		Email nvarchar(100)
	)
	DECLARE @GroupID int
	SELECT @GroupID = GroupID FROM ResourceGroups
	WHERE GroupName = @GroupName
	DECLARE @ItemTypeID int
	SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
	WHERE TypeName = @ItemTypeName
	AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND GroupID = @GroupID))
	INSERT INTO @ServiceItems (ItemID, ItemName, Username, FullName, Email)
	SELECT
		SI.ItemID,
		SI.ItemName,
		U.Username,
		U.FirstName,
		U.Email
	FROM Packages AS P
	INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
	INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
	INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
	WHERE
		SI.ItemTypeID = @ItemTypeID
		AND ((@Recursive = 0 AND P.PackageID = @PackageID)
			OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
		AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND IT.GroupID = @GroupID))
		AND (@ServerID = 0 OR (@ServerID > 0 AND S.ServerID = @ServerID))
	'
WHEN 'Users' THEN '
	DECLARE @Users TABLE
	(
		UserID int,
		Username nvarchar(100),
		FullName nvarchar(100),
		Email nvarchar(100),
		CompanyName nvarchar(100)
	)
	DECLARE @HasUserRights bit
	SET @HasUserRights = dbo.CheckActorUserRights(@ActorID, @UserID)
	INSERT INTO @Users (UserID, Username, FullName, Email, CompanyName)
	SELECT
		U.UserID,
		U.Username,
		U.FullName,
		U.Email,
		U.CompanyName
	FROM UsersDetailed AS U
	WHERE 
		U.UserID <> @UserID AND U.IsPeer = 0 AND
		(
			(@Recursive = 0 AND OwnerID = @UserID) OR
			(@Recursive = 1 AND dbo.CheckUserParent(@UserID, U.UserID) = 1)
		)
		AND ((@StatusID = 0) OR (@StatusID > 0 AND U.StatusID = @StatusID))
		AND ((@RoleID = 0) OR (@RoleID > 0 AND U.RoleID = @RoleID))
		AND @HasUserRights = 1 
	'
WHEN 'VirtualMachines' THEN '
	IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	DECLARE @VirtualMachines TABLE
	(
		ItemID int,
		ItemName nvarchar(100),
		Username nvarchar(100),
		ExternalIP nvarchar(100),
		IPAddress nvarchar(100)
	)
	INSERT INTO @VirtualMachines (ItemID, ItemName, Username, ExternalIP, IPAddress)
	SELECT
		SI.ItemID,
		SI.ItemName,
		U.Username,
		EIP.ExternalIP,
		PIP.IPAddress
	FROM Packages AS P
	INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
	INNER JOIN Users AS U ON P.UserID = U.UserID
	LEFT OUTER JOIN (
		SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
	) AS EIP ON SI.ItemID = EIP.ItemID
	LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
	WHERE
		SI.ItemTypeID = ' + CAST(@VPSTypeID AS nvarchar(12)) + '
		AND ((@Recursive = 0 AND P.PackageID = @PackageID)
		OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
	'
WHEN 'PackagePrivateIPAddresses' THEN '
	DECLARE @PackagePrivateIPAddresses TABLE
	(
		PrivateAddressID int,
		IPAddress nvarchar(100),
		ItemName nvarchar(100)
	)
	INSERT INTO @PackagePrivateIPAddresses (PrivateAddressID, IPAddress, ItemName)
	SELECT
		PA.PrivateAddressID,
		PA.IPAddress,
		SI.ItemName
	FROM dbo.PrivateIPAddresses AS PA
	INNER JOIN dbo.ServiceItems AS SI ON PA.ItemID = SI.ItemID
	WHERE SI.PackageID = @PackageID
	'
ELSE ''
END + 'SELECT TOP ' + CAST(@MaximumRows AS nvarchar(12)) + ' MIN(ItemID) as [ItemID], TextSearch, ColumnType, COUNT(*) AS [Count]' + CASE @PagedStored
WHEN 'Domains' THEN '
	FROM(
	SELECT D0.DomainID AS ItemID, D0.DomainName AS TextSearch, ''DomainName'' AS ColumnType
	FROM @Domains AS D0
	UNION
	SELECT D1.DomainID AS ItemID, D1.Username AS TextSearch, ''Username'' AS ColumnType
	FROM @Domains AS D1
	UNION
	SELECT D2.DomainID as ItemID, D2.FullName AS TextSearch, ''FullName'' AS ColumnType
	FROM @Domains AS D2
	UNION
	SELECT D3.DomainID as ItemID, D3.Email AS TextSearch, ''Email'' AS ColumnType
	FROM @Domains AS D3) AS D'
WHEN 'IPAddresses' THEN '
	FROM(
	SELECT D0.AddressesID AS ItemID, D0.ExternalIP AS TextSearch, ''ExternalIP'' AS ColumnType
	FROM @IPAddresses AS D0
	UNION
	SELECT D1.AddressesID AS ItemID, D1.InternalIP AS TextSearch, ''InternalIP'' AS ColumnType
	FROM @IPAddresses AS D1
	UNION
	SELECT D2.AddressesID AS ItemID, D2.DefaultGateway AS TextSearch, ''DefaultGateway'' AS ColumnType
	FROM @IPAddresses AS D2
	UNION
	SELECT D3.AddressesID AS ItemID, D3.ServerName AS TextSearch, ''ServerName'' AS ColumnType
	FROM @IPAddresses AS D3
	UNION
	SELECT D4.AddressesID AS ItemID, D4.UserName AS TextSearch, ''UserName'' AS ColumnType
	FROM @IPAddresses AS D4
	UNION
	SELECT D6.AddressesID AS ItemID, D6.ItemName AS TextSearch, ''ItemName'' AS ColumnType
	FROM @IPAddresses AS D6) AS D'
WHEN 'Schedules' THEN '
	FROM(
	SELECT D0.ScheduleID AS ItemID, D0.ScheduleName AS TextSearch, ''ScheduleName'' AS ColumnType
	FROM @Schedules AS D0
	UNION
	SELECT D1.ScheduleID AS ItemID, D1.Username AS TextSearch, ''Username'' AS ColumnType
	FROM @Schedules AS D1
	UNION
	SELECT D2.ScheduleID AS ItemID, D2.FullName AS TextSearch, ''FullName'' AS ColumnType
	FROM @Schedules AS D2
	UNION
	SELECT D3.ScheduleID AS ItemID, D3.Email AS TextSearch, ''Email'' AS ColumnType
	FROM @Schedules AS D3) AS D'
WHEN 'NestedPackages' THEN '
	FROM(
	SELECT D0.PackageID AS ItemID, D0.PackageName AS TextSearch, ''PackageName'' AS ColumnType
	FROM @NestedPackages AS D0
	UNION
	SELECT D1.PackageID AS ItemID, D1.Username AS TextSearch, ''Username'' AS ColumnType
	FROM @NestedPackages AS D1
	UNION
	SELECT D2.PackageID as ItemID, D2.FullName AS TextSearch, ''FullName'' AS ColumnType
	FROM @NestedPackages AS D2
	UNION
	SELECT D3.PackageID as ItemID, D3.Email AS TextSearch, ''Email'' AS ColumnType
	FROM @NestedPackages AS D3) AS D'
WHEN 'PackageIPAddresses' THEN '
	FROM(
	SELECT D0.PackageAddressID AS ItemID, D0.ExternalIP AS TextSearch, ''ExternalIP'' AS ColumnType
	FROM @PackageIPAddresses AS D0
	UNION
	SELECT D1.PackageAddressID AS ItemID, D1.InternalIP AS TextSearch, ''InternalIP'' AS ColumnType
	FROM @PackageIPAddresses AS D1
	UNION
	SELECT D2.PackageAddressID as ItemID, D2.DefaultGateway AS TextSearch, ''DefaultGateway'' AS ColumnType
	FROM @PackageIPAddresses AS D2
	UNION
	SELECT D3.PackageAddressID as ItemID, D3.ItemName AS TextSearch, ''ItemName'' AS ColumnType
	FROM @PackageIPAddresses AS D3
	UNION
	SELECT D5.PackageAddressID as ItemID, D5.UserName AS TextSearch, ''UserName'' AS ColumnType
	FROM @PackageIPAddresses AS D5) AS D'
WHEN 'ServiceItems' THEN '
	FROM(
	SELECT D0.ItemID AS ItemID, D0.ItemName AS TextSearch, ''ItemName'' AS ColumnType
	FROM @ServiceItems AS D0
	UNION
	SELECT D1.ItemID AS ItemID, D1.Username AS TextSearch, ''Username'' AS ColumnType
	FROM @ServiceItems AS D1
	UNION
	SELECT D2.ItemID as ItemID, D2.FullName AS TextSearch, ''FullName'' AS ColumnType
	FROM @ServiceItems AS D2
	UNION
	SELECT D3.ItemID as ItemID, D3.Email AS TextSearch, ''Email'' AS ColumnType
	FROM @ServiceItems AS D3) AS D'
WHEN 'Users' THEN '
	FROM(
	SELECT D0.UserID AS ItemID, D0.Username AS TextSearch, ''Username'' AS ColumnType
	FROM @Users AS D0
	UNION
	SELECT D1.UserID AS ItemID, D1.FullName AS TextSearch, ''FullName'' AS ColumnType
	FROM @Users AS D1
	UNION
	SELECT D2.UserID as ItemID, D2.Email AS TextSearch, ''Email'' AS ColumnType
	FROM @Users AS D2
	UNION
	SELECT D3.UserID as ItemID, D3.CompanyName AS TextSearch, ''CompanyName'' AS ColumnType
	FROM @Users AS D3) AS D'
WHEN 'VirtualMachines' THEN '
	FROM(
	SELECT D0.ItemID AS ItemID, D0.ItemName AS TextSearch, ''ItemName'' AS ColumnType
	FROM @VirtualMachines AS D0
	UNION
	SELECT D1.ItemID AS ItemID, D1.ExternalIP AS TextSearch, ''ExternalIP'' AS ColumnType
	FROM @VirtualMachines AS D1
	UNION
	SELECT D2.ItemID as ItemID, D2.Username AS TextSearch, ''Username'' AS ColumnType
	FROM @VirtualMachines AS D2
	UNION
	SELECT D3.ItemID as ItemID, D3.IPAddress AS TextSearch, ''IPAddress'' AS ColumnType
	FROM @VirtualMachines AS D3) AS D'
WHEN 'PackagePrivateIPAddresses' THEN '
	FROM(
	SELECT D0.PrivateAddressID AS ItemID, D0.IPAddress AS TextSearch, ''IPAddress'' AS ColumnType
	FROM @PackagePrivateIPAddresses AS D0
	UNION
	SELECT D1.PrivateAddressID AS ItemID, D1.ItemName AS TextSearch, ''ItemName'' AS ColumnType
	FROM @PackagePrivateIPAddresses AS D1) AS D'
END + '
	WHERE (TextSearch LIKE @FilterValue)'
IF @FilterColumns <> '' AND @FilterColumns IS NOT NULL
	SET @sql = @sql + '
		AND (ColumnType IN (' + @FilterColumns + '))'
SET @sql = @sql + '
	GROUP BY TextSearch, ColumnType
	ORDER BY TextSearch'

exec sp_executesql @sql, N'@FilterValue nvarchar(50), @Recursive bit, @PoolID int, @ServerID int, @ActorID int, @StatusID int, @PlanID int, @OrgID int, @ItemTypeName nvarchar(200), @GroupName nvarchar(100), @PackageID int, @VPSTypeID int, @UserID int, @RoleID int', 
@FilterValue, @Recursive, @PoolID, @ServerID, @ActorID, @StatusID, @PlanID, @OrgID, @ItemTypeName, @GroupName, @PackageID, @VPSTypeID, @UserID, @RoleID

RETURN
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUsersSummary]
(
	@ActorID int,
	@UserID int
)
AS
-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- ALL users
SELECT COUNT(UserID) AS UsersNumber FROM Users
WHERE OwnerID = @UserID AND IsPeer = 0

-- BY STATUS users
SELECT StatusID, COUNT(UserID) AS UsersNumber FROM Users
WHERE OwnerID = @UserID AND IsPeer = 0
GROUP BY StatusID
ORDER BY StatusID

-- BY ROLE users
SELECT RoleID, COUNT(UserID) AS UsersNumber FROM Users
WHERE OwnerID = @UserID AND IsPeer = 0
GROUP BY RoleID
ORDER BY RoleID DESC

RETURN
				*/
				#endregion

				if (!CheckActorUserRights(actorId, userId))
					throw new AccessViolationException("You are not allowed to access this account");

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
				set.Tables.Add(EntityDataTable(new object[] { nofUsers }));
				set.Tables.Add(EntityDataTable(usersByStatus));
				set.Tables.Add(EntityDataTable(usersByRole));

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

		public int[] UsersTree(int ownerId, bool recursive)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[UsersTree]
(
	@OwnerID int,
	@Recursive bit = 0
)
RETURNS @T TABLE (UserID int)
AS
BEGIN

	IF @Recursive = 1
	BEGIN
		-- insert ""root"" user
		INSERT @T VALUES(@OwnerID)

		-- get all children recursively
		WHILE @@ROWCOUNT > 0
		BEGIN
			INSERT @T SELECT UserID
			FROM Users
			WHERE OwnerID IN(SELECT UserID from @T) AND UserID NOT IN(SELECT UserID FROM @T)
		END
	END
	ELSE
	BEGIN
		INSERT @T VALUES(@OwnerID)
	END

RETURN
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserDomainsPaged]
(
	@ActorID int,
	@UserID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
DECLARE @HasUserRights bit
SET @HasUserRights = dbo.CheckActorUserRights(@ActorID, @UserID)

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Users TABLE
(
	ItemPosition int IDENTITY(1,1),
	UserID int,
	DomainID int
)
INSERT INTO @Users (UserID, DomainID)
SELECT
	U.UserID,
	D.DomainID
FROM Users AS U
INNER JOIN UsersTree(@UserID, 1) AS UT ON U.UserID = UT.UserID
LEFT OUTER JOIN Packages AS P ON U.UserID = P.UserID
LEFT OUTER JOIN Domains AS D ON P.PackageID = D.PackageID
WHERE
	U.UserID <> @UserID AND U.IsPeer = 0
	AND @HasUserRights = 1 '

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(UserID) FROM @Users;
SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.SubscriberNumber,
	U.LoginStatusId,
	U.FailedLogins,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	D.DomainName
FROM @Users AS TU
INNER JOIN Users AS U ON TU.UserID = U.UserID
LEFT OUTER JOIN Domains AS D ON TU.DomainID = D.DomainID
WHERE TU.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ActorID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ActorID

RETURN
				*/
				#endregion

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

				return EntityDataSet(users);
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

		public bool CanGetUserDetails(int actorId, int userId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CanGetUserDetails]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
SET @ActorID = @OwnerID

-- get user's owner
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @UserID = @OwnerID
RETURN 1 -- user can get the details of his owner

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK

	IF @ParentUserID = @ActorID
	RETURN 1

	SET @TmpUserID = @ParentUserID
END

RETURN 0
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUsers]
(
	@ActorID int,
	@OwnerID int,
	@Recursive bit = 0
)
AS

DECLARE @CanGetDetails bit
SET @CanGetDetails = dbo.CanGetUserDetails(@ActorID, @OwnerID)

SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.SubscriberNumber,
	U.LoginStatusId,
	U.FailedLogins,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.FullName,
	U.OwnerUsername,
	U.OwnerFirstName,
	U.OwnerLastName,
	U.OwnerRoleID,
	U.OwnerFullName,
	U.PackagesNumber,
	U.CompanyName,
	U.EcommerceEnabled
FROM UsersDetailed AS U
WHERE U.UserID <> @OwnerID AND
((@Recursive = 1 AND dbo.CheckUserParent(@OwnerID, U.UserID) = 1) OR
(@Recursive = 0 AND U.OwnerID = @OwnerID))
AND U.IsPeer = 0
AND @CanGetDetails = 1 -- actor user rights

RETURN
				*/
				#endregion

				var canGetDetails = CanGetUserDetails(actorId, ownerId);

				var users = UsersDetailed
					.Where(u => canGetDetails && u.UserId != ownerId && !u.IsPeer &&
						(recursive ? CheckUserParent(ownerId, u.UserId) : u.OwnerId == ownerId));

				return EntityDataSet(users);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserParents]
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.SubscriberNumber,
	U.LoginStatusId,
	U.FailedLogins,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.CompanyName,
	U.EcommerceEnabled
FROM UserParents(@ActorID, @UserID) AS UP
INNER JOIN Users AS U ON UP.UserID = U.UserID
ORDER BY UP.UserOrder DESC
RETURN
				*/
				#endregion

				if (!CheckActorUserRights(actorId, userId)) throw new AccessViolationException("You are not allowed to access this account");

				int n = 0;
				var parents = UserParents(actorId, userId)
					.Select(u => new { UserId = u, Order = n++ })
					.ToArray();
				var users = Users
					.Join(parents, u => u.UserId, p => p.UserId, (user, parent) => new { User = user, Order = parent.Order })
					.OrderByDescending(u => u.Order)
					.Select(u => u.User);

				return EntityDataSet(users);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserPeers]
(
	@ActorID int,
	@UserID int
)
AS

DECLARE @CanGetDetails bit
SET @CanGetDetails = dbo.CanGetUserDetails(@ActorID, @UserID)

SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.LoginStatusId,
	U.FailedLogins,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.FullName,
	(U.FirstName + ' ' + U.LastName) AS FullName,
	U.CompanyName,
	U.EcommerceEnabled
FROM UsersDetailed AS U
WHERE U.OwnerID = @UserID AND IsPeer = 1
AND @CanGetDetails = 1 -- actor rights

RETURN
				*/
				#endregion

				var canGetDetails = CanGetUserDetails(actorId, userId);

				var userPeers = UsersDetailed
					.Where(u => canGetDetails && u.OwnerId == userId && u.IsPeer);

				return EntityDataSet(userPeers);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserByExchangeOrganizationIdInternally]
(
	@ItemID int
)
AS
	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.SubscriberNumber,
		U.LoginStatusId,
		U.FailedLogins,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		U.Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.UserID IN (SELECT UserID FROM Packages WHERE PackageID IN (
	SELECT PackageID FROM ServiceItems WHERE ItemID = @ItemID))

RETURN
				*/
				#endregion

				var users = Users
					.Join(Packages, u => u.UserId, p => p.UserId, (user, package) => new { User = user, package.PackageId })
					.Join(ServiceItems, u => u.PackageId, s => s.PackageId, (user, serviceItem) => new { user.User, serviceItem.ItemId })
					.Where(u => u.ItemId == itemId)
					.Select(u => u.User);

				return EntityDataReader(users);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserByIdInternally]
(
	@UserID int
)
AS
	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.SubscriberNumber,
		U.LoginStatusId,
		U.FailedLogins,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		U.Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams],
		U.OneTimePasswordState,
		U.MfaMode,
		U.PinSecret
	FROM Users AS U
	WHERE U.UserID = @UserID

	RETURN
				*/
				#endregion

				return EntityDataReader(Users.Where(u => u.UserId == userId));
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserByUsernameInternally]
(
	@Username nvarchar(50)
)
AS
	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.SubscriberNumber,
		U.LoginStatusId,
		U.FailedLogins,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		U.Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams],
		U.OneTimePasswordState,
		U.MfaMode,
		U.PinSecret

	FROM Users AS U
	WHERE U.Username = @Username

	RETURN
				*/
				#endregion

				return EntityDataReader(Users.Where(u => u.Username == username));
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserByUsernameInternally",
					 new SqlParameter("@Username", username));
			}
		}

		public bool CanGetUserPassword(int actorId, int userId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CanGetUserPassword]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1 -- unauthenticated mode

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
BEGIN
	-- peer can't get the password of his peers
	-- and his owner
	IF @UserID = @OwnerID
	RETURN 0

	IF EXISTS (
		SELECT UserID FROM Users
		WHERE IsPeer = 1 AND OwnerID = @OwnerID AND UserID = @UserID
	) RETURN 0

	-- set actor to his owner
	SET @ActorID = @OwnerID
END

-- get user's owner
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @UserID = @OwnerID
RETURN 0 -- user can't get the password of his owner

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK

	IF @ParentUserID = @ActorID
	RETURN 1

	SET @TmpUserID = @ParentUserID
END

RETURN 0
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserById]
(
	@ActorID int,
	@UserID int
)
AS
	-- user can retrieve his own account, his users accounts
	-- and his reseller account (without password)
	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.SubscriberNumber,
		U.LoginStatusId,
		U.FailedLogins,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, @UserID) = 1 THEN U.Password
		ELSE '' END AS Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams],
		U.MfaMode,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, @UserID) = 1 THEN U.PinSecret
		ELSE '' END AS PinSecret
	FROM Users AS U
	WHERE U.UserID = @UserID
	AND dbo.CanGetUserDetails(@ActorID, @UserID) = 1 -- actor user rights

	RETURN
				*/
				#endregion

				var canGetUserDetails = CanGetUserDetails(actorId, userId);
				var canGetUserPassword = CanGetUserPassword(actorId, userId);
				var user = Users
					.Where(u => u.UserId == userId && canGetUserDetails)
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

				return EntityDataReader(user);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserByUsername]
(
	@ActorID int,
	@Username nvarchar(50)
)
AS

	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.SubscriberNumber,
		U.LoginStatusId,
		U.FailedLogins,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, UserID) = 1 THEN U.Password
		ELSE '' END AS Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams],
		U.MfaMode,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, UserID) = 1 THEN U.PinSecret
		ELSE '' END AS PinSecret
	FROM Users AS U
	WHERE U.Username = @Username
	AND dbo.CanGetUserDetails(@ActorID, UserID) = 1 -- actor user rights

	RETURN
				*/
				#endregion

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

				return EntityDataReader(user);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "GetUserByUsername",
					 new SqlParameter("@ActorId", actorId),
					 new SqlParameter("@Username", username));
			}
		}

		public bool CanCreateUser(int actorId, int ownerId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CanCreateUser]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

-- check if the user requests himself
IF @ActorID = @UserID
RETURN 1

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
BEGIN
	SET @ActorID = @OwnerID
END

IF @ActorID = @UserID
RETURN 1

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK

	IF @ParentUserID = @ActorID
	RETURN 1

	SET @TmpUserID = @ParentUserID
END

RETURN 0
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddUser]
(
	@ActorID int,
	@UserID int OUTPUT,
	@OwnerID int,
	@RoleID int,
	@StatusID int,
	@SubscriberNumber nvarchar(32),
	@LoginStatusID int,
	@IsDemo bit,
	@IsPeer bit,
	@Comments ntext,
	@Username nvarchar(50),
	@Password nvarchar(200),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(255),
	@SecondaryEmail nvarchar(255),
	@Address nvarchar(200),
	@City nvarchar(50),
	@State nvarchar(50),
	@Country nvarchar(50),
	@Zip varchar(20),
	@PrimaryPhone varchar(30),
	@SecondaryPhone varchar(30),
	@Fax varchar(30),
	@InstantMessenger nvarchar(200),
	@HtmlMail bit,
	@CompanyName nvarchar(100),
	@EcommerceEnabled bit
)
AS

-- check if the user already exists
IF EXISTS(SELECT UserID FROM Users WHERE Username = @Username)
BEGIN
	SET @UserID = -1
	RETURN
END

-- check actor rights
IF dbo.CanCreateUser(@ActorID, @OwnerID) = 0
BEGIN
	SET @UserID = -2
	RETURN
END

INSERT INTO Users
(
	OwnerID,
	RoleID,
	StatusID,
	SubscriberNumber,
	LoginStatusID,
	Created,
	Changed,
	IsDemo,
	IsPeer,
	Comments,
	Username,
	Password,
	FirstName,
	LastName,
	Email,
	SecondaryEmail,
	Address,
	City,
	State,
	Country,
	Zip,
	PrimaryPhone,
	SecondaryPhone,
	Fax,
	InstantMessenger,
	HtmlMail,
	CompanyName,
	EcommerceEnabled
)
VALUES
(
	@OwnerID,
	@RoleID,
	@StatusID,
	@SubscriberNumber,
	@LoginStatusID,
	GetDate(),
	GetDate(),
	@IsDemo,
	@IsPeer,
	@Comments,
	@Username,
	@Password,
	@FirstName,
	@LastName,
	@Email,
	@SecondaryEmail,
	@Address,
	@City,
	@State,
	@Country,
	@Zip,
	@PrimaryPhone,
	@SecondaryPhone,
	@Fax,
	@InstantMessenger,
	@HtmlMail,
	@CompanyName,
	@EcommerceEnabled
)

SET @UserID = SCOPE_IDENTITY()

RETURN
				*/
				#endregion

				if (Users.Any(u => u.Username == username)) return -1;
				if (!CanCreateUser(actorId, ownerId)) return -2;
				var user = new Data.Entities.User()
				{
					OwnerId = ownerId,
					RoleId = roleId,
					StatusId = statusId,
					SubscriberNumber = subscriberNumber,
					LoginStatusId = loginStatusId,
					Created = DateTime.Now,
					Changed = DateTime.Now,
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

		public bool CanUpdateUserDetails(int actorId, int userId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CanUpdateUserDetails]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
BEGIN
	-- check if the peer is trying to update his owner
	IF @UserID = @OwnerID
	RETURN 0

	-- check if the peer is trying to update his peers
	IF EXISTS (SELECT UserID FROM Users
	WHERE IsPeer = 1 AND OwnerID = @OwnerID AND UserID = @UserID)
	RETURN 0

	SET @ActorID = @OwnerID
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK

	IF @ParentUserID = @ActorID
	RETURN 1

	SET @TmpUserID = @ParentUserID
END

RETURN 0
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateUser]
(
	@ActorID int,
	@UserID int,
	@RoleID int,
	@StatusID int,
	@SubscriberNumber nvarchar(32),
	@LoginStatusId int,
	@IsDemo bit,
	@IsPeer bit,
	@Comments ntext,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(255),
	@SecondaryEmail nvarchar(255),
	@Address nvarchar(200),
	@City nvarchar(50),
	@State nvarchar(50),
	@Country nvarchar(50),
	@Zip varchar(20),
	@PrimaryPhone varchar(30),
	@SecondaryPhone varchar(30),
	@Fax varchar(30),
	@InstantMessenger nvarchar(200),
	@HtmlMail bit,
	@CompanyName nvarchar(100),
	@EcommerceEnabled BIT,
	@AdditionalParams NVARCHAR(max)
)
AS

	-- check actor rights
	IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
	BEGIN
		RETURN
	END

	IF @LoginStatusId = 0
	BEGIN
		UPDATE Users SET
			FailedLogins = 0
		WHERE UserID = @UserID
	END

	UPDATE Users SET
		RoleID = @RoleID,
		StatusID = @StatusID,
		SubscriberNumber = @SubscriberNumber,
		LoginStatusId = @LoginStatusId,
		Changed = GetDate(),
		IsDemo = @IsDemo,
		IsPeer = @IsPeer,
		Comments = @Comments,
		FirstName = @FirstName,
		LastName = @LastName,
		Email = @Email,
		SecondaryEmail = @SecondaryEmail,
		Address = @Address,
		City = @City,
		State = @State,
		Country = @Country,
		Zip = @Zip,
		PrimaryPhone = @PrimaryPhone,
		SecondaryPhone = @SecondaryPhone,
		Fax = @Fax,
		InstantMessenger = @InstantMessenger,
		HtmlMail = @HtmlMail,
		CompanyName = @CompanyName,
		EcommerceEnabled = @EcommerceEnabled,
		[AdditionalParams] = @AdditionalParams
	WHERE UserID = @UserID

	RETURN
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateUserFailedLoginAttempt]
(
	@UserID int,
	@LockOut int,
	@Reset int
)
AS

IF (@Reset = 1)
BEGIN
	UPDATE Users SET FailedLogins = 0 WHERE UserID = @UserID
END
ELSE
BEGIN
	IF (@LockOut <= (SELECT FailedLogins FROM USERS WHERE UserID = @UserID))
	BEGIN
		UPDATE Users SET LoginStatusId = 2 WHERE UserID = @UserID
	END
	ELSE
	BEGIN
		IF ((SELECT FailedLogins FROM Users WHERE UserID = @UserID) IS NULL)
		BEGIN
			UPDATE Users SET FailedLogins = 1 WHERE UserID = @UserID
		END
		ELSE
			UPDATE Users SET FailedLogins = FailedLogins + 1 WHERE UserID = @UserID
	END
END
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteUser]
(
	@ActorID int,
	@UserID int
)
AS

-- check actor rights
IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
RETURN

BEGIN TRAN
-- delete user comments
DELETE FROM Comments
WHERE ItemID = @UserID AND ItemTypeID = 'USER'

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

--delete reseller addon
DELETE FROM HostingPlans WHERE UserID = @UserID AND IsAddon = 'True'

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

-- delete user peers
DELETE FROM Users
WHERE IsPeer = 1 AND OwnerID = @UserID

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

-- delete user
DELETE FROM Users
WHERE UserID = @UserID

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

COMMIT TRAN

RETURN
				*/
				#endregion

				if (!CanUpdateUserDetails(actorId, userId)) return;

				using (var transaction = Database.BeginTransaction())
				{
					// delete user comments
					Comments.Where(c => c.ItemId == userId && c.ItemTypeId == "USER").ExecuteDelete(Comments);
					// delete reseller addon
					HostingPlans.Where(h => h.UserId == userId && h.IsAddon == true).ExecuteDelete(HostingPlans);
					// delete user peers
					Users.Where(u => u.IsPeer && u.OwnerId == userId).ExecuteDelete(Users);
					// delete user
					Users.Where(u => u.UserId == userId).ExecuteDelete(Users);

					transaction.Commit();
				}
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[ChangeUserPassword]
(
	@ActorID int,
	@UserID int,
	@Password nvarchar(200)
)
AS

-- check actor rights
IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
RETURN

UPDATE Users
SET Password = @Password, OneTimePasswordState = 0
WHERE UserID = @UserID

RETURN 
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[SetUserOneTimePassword]
(
	@UserID int,
	@Password nvarchar(200),
	@OneTimePasswordState int
)
AS
UPDATE Users
SET Password = @Password, OneTimePasswordState = @OneTimePasswordState
WHERE UserID = @UserID
RETURN 
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateUserPinSecret]
(
	@ActorID int,
	@UserID int,
	@PinSecret NVARCHAR(255)
)
AS
	-- check actor rights
	IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
	BEGIN
		RETURN
	END
	UPDATE Users SET
		PinSecret = @PinSecret 
	WHERE UserID = @UserID

	RETURN
				*/
				#endregion
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateUserMfaMode]
(
	@ActorID int,
	@UserID int,
	@MfaMode int
)
AS
	-- check actor rights
	IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
	BEGIN
		RETURN
	END
	UPDATE Users SET
		MfaMode = @MfaMode 
	WHERE UserID = @UserID

	RETURN
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE FUNCTION [dbo].[CanChangeMfaFunc]
(
	@CallerID int,
	@ChangeUserID int,
	@CanPeerChangeMfa bit
)
RETURNS bit
AS
BEGIN

DECLARE @IsPeer int, @OwnerID int, @Result int,  @UserId int, @GenerationNumber int
SET @Result = 0;
SET @GenerationNumber = 0;
-- get data for user
SELECT @IsPeer = IsPeer, @OwnerID = OwnerID, @UserId = UserID FROM Users
WHERE UserID = @CallerID;

-- userif not found
IF(@UserId IS NULL)
BEGIN
	RETURN 0
END

-- is rootuser serveradmin
IF (@OwnerID IS NULL)
BEGIN
	RETURN 1
END

-- check if the user requests himself
IF (@CallerID = @ChangeUserID AND @IsPeer > 0 AND @CanPeerChangeMfa <> 0)
BEGIN
	RETURN 1
END

IF (@CallerID = @ChangeUserID AND @IsPeer = 0)
BEGIN
	RETURN 1
END

IF (@IsPeer = 1)
BEGIN
	SET @UserID = @OwnerID
	SET @GenerationNumber = 1;
END;

WITH generation AS (
    SELECT UserID,
           Username,
		   OwnerID,
		   IsPeer,
           0 AS generation_number
    FROM Users
	where UserID = @UserID
UNION ALL
    SELECT child.UserID,
         child.Username,
         child.OwnerId,
		 child.IsPeer,
		 generation_number + 1 AS generation_number
    FROM Users child
    JOIN generation g
      ON g.UserID = child.OwnerId
)

Select @Result = count(*)
FROM generation g
JOIN Users parent
ON g.OwnerID = parent.UserID
where (g.generation_number > @GenerationNumber or g.IsPeer <> 1) and g.UserID = @ChangeUserID;

if(@Result > 0)
BEGIN
	RETURN 1
END
ELSE
BEGIN
	RETURN 0
END

RETURN 0
END
			
CREATE PROCEDURE [dbo].[CanChangeMfa]
(
	@CallerID int,
	@ChangeUserID int,
	@CanPeerChangeMfa bit,
	@Result bit OUTPUT
)
AS
	SET @Result = dbo.CanChangeMfaFunc(@CallerID, @ChangeUserID, @CanPeerChangeMfa)
	RETURN
				 */
				#endregion

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
				var nextGeneration = Users.Join(generation, u => u.OwnerId, g => g.UserId, (usr, gen) => new
				{
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserPackagesServerUrls]
(
	@UserId INT
)
AS
	SELECT DISTINCT Servers.ServerUrl
	FROM Servers
	INNER JOIN Packages
	ON Servers.ServerId = Packages.ServerId
	WHERE Packages.UserID = @UserId
	RETURN
				*/
				#endregion

				var serverUrls = Servers.Join(Packages, s => s.ServerId, p => p.ServerId, (server, package) => new
				{
					server.ServerUrl,
					package.UserId
				})
				.Where(s => s.UserId == userId)
				.Select(s => new { s.ServerUrl });

				return EntityDataReader(serverUrls);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUserSettings]
(
	@ActorID int,
	@UserID int,
	@SettingsName nvarchar(50)
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- find which parent package has overriden NS
DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	IF EXISTS
	(
		SELECT PropertyName FROM UserSettings
		WHERE SettingsName = @SettingsName AND UserID = @TmpUserID
	)
	BEGIN
		SELECT
			UserID,
			PropertyName,
			PropertyValue
		FROM
			UserSettings
		WHERE
			UserID = @TmpUserID AND
			SettingsName = @SettingsName

		BREAK
	END

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
	BREAK

	SET @TmpUserID = @ParentUserID
END

RETURN
				*/
				#endregion

				if (!CheckActorUserRights(actorId, userId))
					throw new AccessViolationException("You are not allowed to access this account");

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

				return EntityDataReader(new Data.Entities.UserSetting[] { setting });
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
				#region Stored Procedure
				/*
	CREATE PROCEDURE [dbo].[UpdateUserSettings]
(
	@ActorID int,
	@UserID int,
	@SettingsName nvarchar(50),
	@Xml ntext
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- delete old properties
BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM UserSettings
WHERE UserID = @UserID AND SettingsName = @SettingsName

INSERT INTO UserSettings
(
	UserID,
	SettingsName,
	PropertyName,
	PropertyValue
)
SELECT
	@UserID,
	@SettingsName,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH
(
	PropertyName nvarchar(50) '@name',
	PropertyValue ntext '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN
				*/
				#endregion

				if (!CheckActorUserRights(actorId, userId))
					throw new AccessViolationException("You are not allowed to access this account");

				UserSettings.RemoveRange(UserSettings
					.Where(s => s.UserId == userId && s.SettingsName == settingsName));

				var properties = XElement.Parse(xml);
				foreach (var property in properties.Elements())
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
		public bool CheckIsUserAdmin(int userId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CheckIsUserAdmin]
(
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @UserID = -1
RETURN 1

IF EXISTS (SELECT UserID FROM Users
WHERE UserID = @UserID AND RoleID = 1) -- administrator
RETURN 1

RETURN 0
END
			*/
			#endregion

			return userId == -1 || Users.Any(u => u.UserId == userId && u.RoleId == 1);
		}
		public DataSet GetAllServers(int actorId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetAllServers]
(
	@ActorID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServerID,
	S.ServerName,
	S.ServerUrl,
	(SELECT COUNT(SRV.ServiceID) FROM VirtualServices AS SRV WHERE S.ServerID = SRV.ServerID) AS ServicesNumber,
	S.Comments
FROM Servers AS S
WHERE @IsAdmin = 1
ORDER BY S.VirtualServer, S.ServerName
				*/
				#endregion

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

				var serversTable = EntityDataTable(servers);

				var services = Services
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (srvc, p) => new { Service = srvc, ProviderGroupId = p.GroupId })
					.Join(ResourceGroups, s => s.ProviderGroupId, rg => rg.GroupId, (srvc, rg) => new
					{
						Service = srvc.Service,
						GroupOrder = rg.GroupOrder
					})
					.Where(s => isAdmin)
					.OrderBy(s => s.GroupOrder)
					.Select(s => new
					{
						s.Service.ServiceId,
						s.Service.ServerId,
						s.Service.ProviderId,
						s.Service.ServiceName,
						s.Service.Comments
					});

				var servicesTable = EntityDataTable(services);

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServers]
(
	@ActorID int
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServerID,
	S.ServerName,
	S.ServerUrl,
	(SELECT COUNT(SRV.ServiceID) FROM Services AS SRV WHERE S.ServerID = SRV.ServerID) AS ServicesNumber,
	S.Comments,
	PrimaryGroupID,
	S.ADEnabled
FROM Servers AS S
WHERE VirtualServer = 0
AND @IsAdmin = 1
ORDER BY S.ServerName

-- services
SELECT
	S.ServiceID,
	S.ServerID,
	S.ProviderID,
	S.ServiceName,
	S.Comments
FROM Services AS S
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
INNER JOIN ResourceGroups AS RG ON P.GroupID = RG.GroupID
WHERE @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var servers = Servers
					.Where(s => isAdmin && !s.VirtualServer)
					.OrderBy(s => s.ServerName)
					.Select(s => new
					{
						s.ServerId,
						s.ServerName,
						s.ServerUrl,
						ServicesNumber = Services.Where(sc => sc.ServerId == s.ServerId).Count(),
						s.Comments,
						s.PrimaryGroupId,
						s.ADEnabled
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
				set.Tables.Add(EntityDataTable(servers));
				set.Tables.Add(EntityDataTable(services));

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServer]
(
	@ActorID int,
	@ServerID int,
	@forAutodiscover bit
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	ServerID,
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADEnabled,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType,
	ADParentDomain,
	ADParentDomainController,
	OSPlatform,
	IsCore,
	PasswordIsSHA256

FROM Servers
WHERE
	ServerID = @ServerID
	AND (@IsAdmin = 1 OR @forAutodiscover = 1)

RETURN
				*/
				#endregion

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

				return EntityDataReader(server);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServerShortDetails]
(
	@ServerID int
)
AS

SELECT
	ServerID,
	ServerName,
	Comments,
	VirtualServer,
	InstantDomainAlias
FROM Servers
WHERE
	ServerID = @ServerID

RETURN
				*/
				#endregion

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

				return EntityDataReader(server);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServerByName]
(
	@ActorID int,
	@ServerName nvarchar(100)
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	ServerID,
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType,
	ADParentDomain,
	ADParentDomainController,
	OSPlatform,
	IsCore,
	PasswordIsSHA256
FROM Servers
WHERE
	ServerName = @ServerName
	AND @IsAdmin = 1

RETURN
				*/
				#endregion

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

				return EntityDataReader(server);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServerInternal]
(
	@ServerID int
)
AS
SELECT
	ServerID,
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADEnabled,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType,
	ADParentDomain,
	ADParentDomainController,
	OSPlatform,
	IsCore,
	PasswordIsSHA256
FROM Servers
WHERE
	ServerID = @ServerID

RETURN
				*/
				#endregion

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

				return EntityDataReader(server);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddServer]
(
	@ServerID int OUTPUT,
	@ServerName nvarchar(100),
	@ServerUrl nvarchar(255),
	@Password nvarchar(100),
	@Comments ntext,
	@VirtualServer bit,
	@InstantDomainAlias nvarchar(200),
	@PrimaryGroupID int,
	@ADEnabled bit,
	@ADRootDomain nvarchar(200),
	@ADUsername nvarchar(100),
	@ADPassword nvarchar(100),
	@ADAuthenticationType varchar(50),
	@OSPlatform int,
	@IsCore bit,
	@PasswordIsSHA256 bit
)
AS

IF @PrimaryGroupID = 0

SET @PrimaryGroupID = NULL

INSERT INTO Servers
(
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADEnabled,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType,
	OSPlatform,
	IsCore,
	PasswordIsSHA256
)
VALUES
(
	@ServerName,
	@ServerUrl,
	@Password,
	@Comments,
	@VirtualServer,
	@InstantDomainAlias,
	@PrimaryGroupID,
	@ADEnabled,
	@ADRootDomain,
	@ADUsername,
	@ADPassword,
	@ADAuthenticationType,
	@OSPlatform,
	@IsCore,
	@PasswordIsSHA256
)

SET @ServerID = SCOPE_IDENTITY()

RETURN
				*/
				#endregion

				var server = new Data.Entities.Server()
				{
					ServerName = serverName,
					ServerUrl = serverUrl,
					Password = password,
					Comments = comments,
					VirtualServer = virtualServer,
					InstantDomainAlias = instantDomainAlias,
					PrimaryGroupId = primaryGroupId != 0 ? primaryGroupId : null,
					ADEnabled = adEnabled,
					ADRootDomain = adRootDomain,
					ADUsername = adUsername,
					ADPassword = adPassword,
					ADAuthenticationType = adAuthenticationType,
					OSPlatform = osPlatform,
					IsCore = isCore,
					PasswordIsSHA256 = PasswordIsSHA256
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateServer]
(
	@ServerID int,
	@ServerName nvarchar(100),
	@ServerUrl nvarchar(255),
	@Password nvarchar(100),
	@Comments ntext,
	@InstantDomainAlias nvarchar(200),
	@PrimaryGroupID int,
	@ADEnabled bit,
	@ADRootDomain nvarchar(200),
	@ADUsername nvarchar(100),
	@ADPassword nvarchar(100),
	@ADAuthenticationType varchar(50),
	@ADParentDomain nvarchar(200),
	@ADParentDomainController nvarchar(200),
	@OSPlatform int,
	@IsCore bit,
	@PasswordIsSHA256 bit
)
AS

IF @PrimaryGroupID = 0
SET @PrimaryGroupID = NULL

UPDATE Servers SET
	ServerName = @ServerName,
	ServerUrl = @ServerUrl,
	Password = @Password,
	Comments = @Comments,
	InstantDomainAlias = @InstantDomainAlias,
	PrimaryGroupID = @PrimaryGroupID,
	ADEnabled = @ADEnabled,
	ADRootDomain = @ADRootDomain,
	ADUsername = @ADUsername,
	ADPassword = @ADPassword,
	ADAuthenticationType = @ADAuthenticationType,
	ADParentDomain = @ADParentDomain,
	ADParentDomainController = @ADParentDomainController,
	OSPlatform = @OSPlatform,
	IsCore = @IsCore,
	PasswordIsSHA256 = @PasswordIsSHA256
WHERE ServerID = @ServerID
RETURN
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteServer]
(
	@ServerID int,
	@Result int OUTPUT
)
AS
SET @Result = 0

-- check related services
IF EXISTS (SELECT ServiceID FROM Services WHERE ServerID = @ServerID)
BEGIN
	SET @Result = -1
	RETURN
END

-- check related packages
IF EXISTS (SELECT PackageID FROM Packages WHERE ServerID = @ServerID)
BEGIN
	SET @Result = -2
	RETURN
END

-- check related hosting plans
IF EXISTS (SELECT PlanID FROM HostingPlans WHERE ServerID = @ServerID)
BEGIN
	SET @Result = -3
	RETURN
END

BEGIN TRAN

-- delete IP addresses
DELETE FROM IPAddresses
WHERE ServerID = @ServerID

-- delete global DNS records
DELETE FROM GlobalDnsRecords
WHERE ServerID = @ServerID

-- delete server
DELETE FROM Servers
WHERE ServerID = @ServerID

-- delete virtual services if any
DELETE FROM VirtualServices
WHERE ServerID = @ServerID
COMMIT TRAN

RETURN
				*/
				#endregion
				// check related services
				if (Services.Any(svc => svc.ServerId == serverId)) return -1;

				// check related packages
				if (Packages.Any(p => p.ServerId == serverId)) return -2;

				// check related hosting plans
				if (HostingPlans.Any(p => p.ServerId == serverId)) return -3;

				using (var transaction = Database.BeginTransaction())
				{
					// delete IP addresses
					IpAddresses.Where(ip => ip.ServerId == serverId).ExecuteDelete(IpAddresses);

					// delete global DNS records
					GlobalDnsRecords.Where(r => r.ServerId == serverId).ExecuteDelete(GlobalDnsRecords);

					// delete server
					Servers.Where(s => s.ServerId == serverId).ExecuteDelete(Servers);

					// delete virtual services if any
					VirtualServices.Where(vs => vs.ServerId == serverId).ExecuteDelete(VirtualServices);

					transaction.Commit();
				}
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetVirtualServers]
(
	@ActorID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServerID,
	S.ServerName,
	S.ServerUrl,
	(SELECT COUNT(SRV.ServiceID) FROM VirtualServices AS SRV WHERE S.ServerID = SRV.ServerID) AS ServicesNumber,
	S.Comments,
	PrimaryGroupID
FROM Servers AS S
WHERE
	VirtualServer = 1
	AND @IsAdmin = 1
ORDER BY S.ServerName

RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var servers = Servers
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

				return EntityDataSet(servers);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetAvailableVirtualServices]
(
	@ActorID int,
	@ServerID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServerID,
	S.ServerName,
	S.Comments
FROM Servers AS S
WHERE
	VirtualServer = 0 -- get only physical servers
	AND @IsAdmin = 1

-- services
SELECT
	ServiceID,
	ServerID,
	ProviderID,
	ServiceName,
	Comments
FROM Services
WHERE
	ServiceID NOT IN (SELECT ServiceID FROM VirtualServices WHERE ServerID = @ServerID)
	AND @IsAdmin = 1

RETURN
				*/
				#endregion

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
				set.Tables.Add(EntityDataTable(servers));
				set.Tables.Add(EntityDataTable(services));

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetVirtualServices]
(
	@ActorID int,
	@ServerID int,
	@forAutodiscover bit
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- virtual groups
SELECT
	VRG.VirtualGroupID,
	RG.GroupID,
	RG.GroupName,
	ISNULL(VRG.DistributionType, 1) AS DistributionType,
	ISNULL(VRG.BindDistributionToPrimary, 1) AS BindDistributionToPrimary
FROM ResourceGroups AS RG
LEFT OUTER JOIN VirtualGroups AS VRG ON RG.GroupID = VRG.GroupID AND VRG.ServerID = @ServerID
WHERE
	(@IsAdmin = 1 OR @forAutodiscover = 1) AND (ShowGroup = 1)
ORDER BY RG.GroupOrder

-- services
SELECT
	VS.ServiceID,
	S.ServiceName,
	S.Comments,
	P.GroupID,
	P.DisplayName,
	SRV.ServerName
FROM VirtualServices AS VS
INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE
	VS.ServerID = @ServerID
	AND (@IsAdmin = 1 OR @forAutodiscover = 1)

RETURN
				*/
				#endregion

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
				set.Tables.Add(EntityDataTable(virtGroups));
				set.Tables.Add(EntityDataTable(services));

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddVirtualServices]
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:

<services>
	<service id=""16"" />
</services>

*//*

				BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- update HP resources
INSERT INTO VirtualServices
(
	ServerID,
	ServiceID
)
SELECT
	@ServerID,
	ServiceID
FROM OPENXML(@idoc, '/services/service',1) WITH
(
	ServiceID int '@id'
) as XS
WHERE XS.ServiceID NOT IN(SELECT ServiceID FROM VirtualServices WHERE ServerID = @ServerID)

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN
				*/
				#endregion

				/* XML Format:
				<services>
					<service id="16" />
				</services> */

				var services = XElement.Parse(xml);

				var existingServices = VirtualServices
					.Where(v => v.ServerId == serverId)
					.Select(v => v.ServiceId)
					.ToHashSet();

				bool addedAny = false;
				foreach (var service in services.Elements())
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteVirtualServices]
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:

<services>
	<service id=""16"" />
</services>
*//*

				BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- update HP resources
DELETE FROM VirtualServices
WHERE ServiceID IN(
SELECT
	ServiceID
FROM OPENXML(@idoc, '/services/service',1) WITH
(
	ServiceID int '@id'
) as XS)
AND ServerID = @ServerID

-- remove document
EXEC sp_xml_removedocument @idoc

COMMIT TRAN
RETURN
				*/
				#endregion

				/* XML Format:
				<services>
					<service id=""16"" />
				</services> */

				var services = XElement.Parse(xml);
				var serviceIds = services.Elements()
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateVirtualGroups]
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:
<groups>
	<group id="16" distributionType="1" bindDistributionToPrimary="1"/>
</groups>
*//*

				BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- delete old virtual groups
DELETE FROM VirtualGroups
WHERE ServerID = @ServerID

-- update HP resources
INSERT INTO VirtualGroups
(
	ServerID,
	GroupID,
	DistributionType,
	BindDistributionToPrimary
)
SELECT
	@ServerID,
	GroupID,
	DistributionType,
	BindDistributionToPrimary
FROM OPENXML(@idoc, '/groups/group',1) WITH
(
	GroupID int '@id',
	DistributionType int '@distributionType',
	BindDistributionToPrimary bit '@bindDistributionToPrimary'
) as XRG

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN
				*/
				#endregion

				/* XML Format:
				<groups>
					<group id="16" distributionType="1" bindDistributionToPrimary="1"/>
				</groups> */

				var groupsXml = XElement.Parse(xml);
				var groups = groupsXml.Elements()
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetProviders]
AS
SELECT
	PROV.ProviderID,
	PROV.GroupID,
	PROV.ProviderName,
	PROV.EditorControl,
	PROV.DisplayName,
	PROV.ProviderType,
	RG.GroupName + ' - ' + PROV.DisplayName AS ProviderName,
	PROV.DisableAutoDiscovery
FROM Providers AS PROV
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
ORDER BY RG.GroupOrder, PROV.DisplayName
RETURN
				*/
				#endregion

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

				return EntityDataSet(providers);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetGroupProviders]
(
	@GroupID int
)
AS
SELECT
	PROV.ProviderID,
	PROV.GroupID,
	PROV.ProviderName,
	PROV.DisplayName,
	PROV.ProviderType,
	RG.GroupName + ' - ' + PROV.DisplayName AS ProviderName
FROM Providers AS PROV
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE RG.GroupID = @GroupId
ORDER BY RG.GroupOrder, PROV.DisplayName
RETURN
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetProvider]
(
	@ProviderID int
)
AS
SELECT
	ProviderID,
	GroupID,
	ProviderName,
	EditorControl,
	DisplayName,
	ProviderType
FROM Providers
WHERE
	ProviderID = @ProviderID

RETURN
				*/
				#endregion

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

				return EntityDataReader(provider);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetProviderByServiceID]
(
	@ServiceID int
)
AS
SELECT
	P.ProviderID,
	P.GroupID,
	P.DisplayName,
	P.EditorControl,
	P.ProviderType
FROM Services AS S
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE
	S.ServiceID = @ServiceID

RETURN
				*/
				#endregion

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

				return EntityDataReader(provider);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddPrivateNetworkVlan]
(
 @VlanID int OUTPUT,
 @Vlan int,
 @ServerID int,
 @Comments ntext
)
AS
BEGIN
 IF @ServerID = 0
 SET @ServerID = NULL

 INSERT INTO PrivateNetworkVLANs(Vlan, ServerID, Comments)
 VALUES (@Vlan, @ServerID, @Comments)

 SET @VlanID = SCOPE_IDENTITY()

 RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRDSCertificate]
(
	@RDSCertificateId INT OUTPUT,
	@ServiceId INT,
	@Content NTEXT,
	@Hash NVARCHAR(255),
	@FileName NVARCHAR(255),
	@ValidFrom DATETIME,
	@ExpiryDate DATETIME
)
AS
INSERT INTO RDSCertificates
(
	ServiceId,
	Content,
	Hash,
	FileName,
	ValidFrom,
	ExpiryDate	
)
VALUES
(
	@ServiceId,
	@Content,
	@Hash,
	@FileName,
	@ValidFrom,
	@ExpiryDate
)

SET @RDSCertificateId = SCOPE_IDENTITY()

RETURN
				*/
				#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeletePrivateNetworkVLAN]
(
	@VlanID int,
	@Result int OUTPUT
)
AS

SET @Result = 0
IF EXISTS(SELECT VlanID FROM PackageVLANs WHERE VlanID = @VlanID)
BEGIN
	SET @Result = -2
	RETURN
END

DELETE FROM PrivateNetworkVLANs
WHERE VlanID = @VlanID
RETURN
				*/
				#endregion

				if (PackageVlans.Any(vlan => vlan.VlanId == vlanId)) return -2;

				PrivateNetworkVlans.Where(vlan => vlan.VlanId == vlanId).ExecuteDelete(PrivateNetworkVlans);

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetPrivateNetworVLANsPaged]
(
 @ActorID int,
 @ServerID int,
 @FilterColumn nvarchar(50) = '',
 @FilterValue nvarchar(50) = '',
 @SortColumn nvarchar(50),
 @StartRow int,
 @MaximumRows int
)
AS
BEGIN

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
@IsAdmin = 1
AND (@ServerID = 0 OR @ServerID <> 0 AND V.ServerID = @ServerID)
'

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
 IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
  SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
 ELSE
  SET @condition = @condition + '
   AND (Vlan LIKE ''' + @FilterValue + '''
   OR ServerName LIKE ''' + @FilterValue + '''
   OR Username LIKE ''' + @FilterValue + ''')'
END

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'V.Vlan ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(V.VlanID)
FROM dbo.PrivateNetworkVLANs AS V
LEFT JOIN Servers AS S ON V.ServerID = S.ServerID
LEFT JOIN PackageVLANs AS PA ON V.VlanID = PA.VlanID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON P.UserID = U.UserID
WHERE ' + @condition + '

DECLARE @VLANs AS TABLE
(
 VlanID int
);

WITH TempItems AS (
 SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
  V.VlanID
 FROM dbo.PrivateNetworkVLANs AS V
 LEFT JOIN Servers AS S ON V.ServerID = S.ServerID
 LEFT JOIN PackageVLANs AS PA ON V.VlanID = PA.VlanID
 LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
 LEFT JOIN dbo.Users U ON U.UserID = P.UserID
 WHERE ' + @condition + '
)

INSERT INTO @VLANs
SELECT VlanID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
 V.VlanID,
 V.Vlan,
 V.Comments,
 V.ServerID,
 S.ServerName,
 PA.PackageID,
 P.PackageName,
 P.UserID,
 U.UserName
FROM @VLANs AS TA
INNER JOIN dbo.PrivateNetworkVLANs AS V ON TA.VlanID = V.VlanID
LEFT JOIN Servers AS S ON V.ServerID = S.ServerID
LEFT JOIN PackageVLANs AS PA ON V.VlanID = PA.VlanID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON U.UserID = P.UserID
'

exec sp_executesql @sql, N'@IsAdmin bit, @ServerID int, @StartRow int, @MaximumRows int',
@IsAdmin, @ServerID, @StartRow, @MaximumRows

END
				*/
				#endregion

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
					}
					else
					{
						vlans = vlans.Where(v => v.Vlan.ToString() == filterValue || v.ServerName == filterValue ||
							v.Username == filterValue);
					}
				}

				if (string.IsNullOrEmpty(sortColumn)) sortColumn = "Vlan";

				vlans = vlans.OrderBy(sortColumn);

				vlans = vlans.Skip(startRow).Take(maximumRows);

				return EntityDataReader(vlans);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetPrivateNetworVLAN]
(
 @VlanID int
)
AS
BEGIN
 -- select
 SELECT
  VlanID,
  Vlan,
  ServerID,
  Comments
 FROM PrivateNetworkVLANs
 WHERE
  VlanID = @VlanID
 RETURN
END
				*/
				#endregion

				var vlan = PrivateNetworkVlans
					.Where(v => v.VlanId == vlanId);

				return EntityDataReader(vlan);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdatePrivateNetworVLAN]
(
 @VlanID int,
 @ServerID int,
 @Vlan int,
 @Comments ntext
)
AS
BEGIN
 IF @ServerID = 0
 SET @ServerID = NULL

 UPDATE PrivateNetworkVLANs SET
  Vlan = @Vlan,
  ServerID = @ServerID,
  Comments = @Comments
 WHERE VlanID = @VlanID
 RETURN
END
				*/
				#endregion

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

		public bool CheckPackageParent(int parentPackageId, int packageId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CheckPackageParent]
(
	@ParentPackageID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

-- check if the user requests hiself
IF @ParentPackageID = @PackageID
BEGIN
	RETURN 1
END

DECLARE @TmpParentPackageID int, @TmpPackageID int
SET @TmpPackageID = @PackageID

WHILE 10 = 10
BEGIN

	SET @TmpParentPackageID = NULL --reset var

	-- get owner
	SELECT
		@TmpParentPackageID = ParentPackageID
	FROM Packages
	WHERE PackageID = @TmpPackageID

	IF @TmpParentPackageID IS NULL -- the last parent package
		BREAK

	IF @TmpParentPackageID = @ParentPackageID
	RETURN 1

	SET @TmpPackageID = @TmpParentPackageID
END

RETURN 0
END
			*/
			#endregion

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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetPackagePrivateNetworkVLANs]
(
 @PackageID int,
 @SortColumn nvarchar(50),
 @StartRow int,
 @MaximumRows int
)
AS
BEGIN
-- start
DECLARE @condition nvarchar(700)
SET @condition = '
dbo.CheckPackageParent(@PackageID, PA.PackageID) = 1
'

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'V.Vlan ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(PA.PackageVlanID)
FROM dbo.PackageVLANs PA
INNER JOIN dbo.PrivateNetworkVLANs AS V ON PA.VlanID = V.VlanID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
WHERE ' + @condition + '

DECLARE @VLANs AS TABLE
(
 PackageVlanID int
);

WITH TempItems AS (
 SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
  PA.PackageVlanID
 FROM dbo.PackageVLANs PA
 INNER JOIN dbo.PrivateNetworkVLANs AS V ON PA.VlanID = V.VlanID
 INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
 INNER JOIN dbo.Users U ON U.UserID = P.UserID
 WHERE ' + @condition + '
)

INSERT INTO @VLANs
SELECT PackageVlanID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
 PA.PackageVlanID,
 PA.VlanID,
 V.Vlan,
 PA.PackageID,
 P.PackageName,
 P.UserID,
 U.UserName
FROM @VLANs AS TA
INNER JOIN dbo.PackageVLANs AS PA ON TA.PackageVlanID = PA.PackageVlanID
INNER JOIN dbo.PrivateNetworkVLANs AS V ON PA.VlanID = V.VlanID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
'

print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int',
@PackageID, @StartRow, @MaximumRows

END
				*/
				#endregion

				var vlans = PackageVlans
					.Where(pv => CheckPackageParent(packageId, pv.PackageId))
					.Join(PrivateNetworkVlans, p => p.VlanId, v => v.VlanId, (pv, vl) => new
					{
						PackageVlan = pv,
						Vlan = vl
					})
					.Join(Packages, j => j.PackageVlan.PackageId, p => p.PackageId, (j, p) => new
					{
						j.PackageVlan,
						j.Vlan,
						Package = p
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

				return EntityDataReader(vlans);
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
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeallocatePackageVLAN]
	@PackageVlanID int
AS
BEGIN

	SET NOCOUNT ON;

	-- check parent package
	DECLARE @ParentPackageID int

	SELECT @ParentPackageID = P.ParentPackageID
	FROM PackageVLANs AS PV
	INNER JOIN Packages AS P ON PV.PackageID = P.PackageId
	WHERE PV.PackageVlanID = @PackageVlanID

	IF (@ParentPackageID = 1) -- ""System"" space
	BEGIN
		DELETE FROM dbo.PackageVLANs
		WHERE PackageVlanID = @PackageVlanID
	END
	ELSE -- 2rd level space and below
	BEGIN
		UPDATE PackageVLANs
		SET PackageID = @ParentPackageID
		WHERE PackageVlanID = @PackageVlanID
	END

END
				*/
				#endregion

				var packageVlan = PackageVlans
					.Where(pv => pv.PackageVlanId == id)
					.Include(pv => pv.Package)
					.Single();

				if (packageVlan.Package.ParentPackageId == 1) // System space
				{
					PackageVlans.Where(pv => pv.PackageVlanId == id).ExecuteDelete(PackageVlans);
				}
				else // 2nd level space and below
				{
					packageVlan.PackageId = id;

					SaveChanges();
				}
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeallocatePackageVLAN",
												  new SqlParameter("@PackageVlanID", id));
			}
		}

		public IDataReader GetUnallottedVLANs(int packageId, int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetUnallottedVLANs]
 @PackageID int,
 @ServiceID int
AS
BEGIN
 DECLARE @ParentPackageID int
 DECLARE @ServerID int
IF (@PackageID = -1) -- NO PackageID defined, use ServerID from ServiceID (VPS Import)
BEGIN
 SELECT
  @ServerID = ServerID,
  @ParentPackageID = 1
 FROM Services
 WHERE ServiceID = @ServiceID
END
ELSE
BEGIN
 SELECT
  @ParentPackageID = ParentPackageID,
  @ServerID = ServerID
 FROM Packages
 WHERE PackageID = @PackageId
END

IF @ParentPackageID = 1 -- ""System"" space
BEGIN
  -- check if server is physical
  IF EXISTS(SELECT * FROM Servers WHERE ServerID = @ServerID AND VirtualServer = 0)
  BEGIN
   -- physical server
   SELECT
    V.VlanID,
    V.Vlan,
    V.ServerID
   FROM dbo.PrivateNetworkVLANs AS V
   WHERE
    (V.ServerID = @ServerID OR V.ServerID IS NULL)
    AND V.VlanID NOT IN (SELECT PV.VlanID FROM dbo.PackageVLANs AS PV)
   ORDER BY V.ServerID DESC, V.Vlan
  END
  ELSE
  BEGIN
   -- virtual server
   -- get resource group by service
   DECLARE @GroupID int
   SELECT @GroupID = P.GroupID FROM Services AS S
   INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
   WHERE S.ServiceID = @ServiceID
   SELECT
    V.VlanID,
    V.Vlan,
    V.ServerID
   FROM dbo.PrivateNetworkVLANs AS V
   WHERE
    (V.ServerID IN (
     SELECT SVC.ServerID FROM [dbo].[Services] AS SVC
     INNER JOIN [dbo].[Providers] AS P ON SVC.ProviderID = P.ProviderID
     WHERE [SVC].[ServiceID] = @ServiceId AND P.GroupID = @GroupID
    ) OR V.ServerID IS NULL)
    AND V.VlanID NOT IN (SELECT PV.VlanID FROM dbo.PackageVLANs AS PV)
   ORDER BY V.ServerID DESC, V.Vlan
  END
 END
 ELSE -- 2rd level space and below
 BEGIN
  -- get service location
  SELECT @ServerID = S.ServerID FROM Services AS S
  WHERE S.ServiceID = @ServiceID
  SELECT
   V.VlanID,
   V.Vlan,
   V.ServerID
  FROM dbo.PackageVLANs AS PV
  INNER JOIN PrivateNetworkVLANs AS V ON PV.VlanID = V.VlanID
  WHERE
   PV.PackageID = @ParentPackageID
   AND (V.ServerID = @ServerID OR V.ServerID IS NULL)
  ORDER BY V.ServerID DESC, V.Vlan
 END
END
				*/
				#endregion

				int parentPackageId = -1;
				int serverId = -1;

				if (packageId == -1)
				{ // No PackageID defined, use ServerID from ServiceID (VPS Import)
					serverId = Services
						.Where(service => service.ServiceId == serviceId)
						.Select(service => service.ServerId)
						.SingleOrDefault();
					parentPackageId = 1;
				}
				else
				{
					var package = Packages
						.Where(p => p.PackageId == packageId)
						.Select(p => new { p.ServerId, p.ParentPackageId })
						.SingleOrDefault();
					if (package != null)
					{
						parentPackageId = package.ParentPackageId ?? 1;
						serverId = package.ServerId ?? -1;
					}
				}

				if (parentPackageId == 1) // "System" space
				{
					// check if server is physical
					if (Servers.Any(s => s.ServerId == serverId && !s.VirtualServer))
					{
						// physical server
						var vlans = PrivateNetworkVlans
							.GroupJoin(PackageVlans, v => v.VlanId, p => p.VlanId, (v, ps) => new
							{
								Vlan = v,
								HasPackage = ps.Any(),
							})
							.Where(g => !g.HasPackage && (g.Vlan.ServerId == serverId || g.Vlan.ServerId == null))
							.Select(g => new
							{
								g.Vlan.VlanId,
								g.Vlan.Vlan,
								g.Vlan.ServerId
							});
						return EntityDataReader(vlans);
					}
					else
					{ // virtual server, get resource group by service
						var groupId = Services
							.Where(s => s.ServiceId == serviceId)
							.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => p.GroupId)
							.Single();
						var vlans = PrivateNetworkVlans
							.Join(Services, v => v.ServerId, s => s.ServerId, (v, s) => new
							{
								Vlan = v,
								Service = s
							})
							.Join(Providers, g => g.Service.ProviderId, p => p.ProviderId, (g, p) => new
							{
								g.Vlan,
								g.Service,
								Provider = p
							})
							.Where(g => (g.Service.ServiceId == serviceId && g.Provider.GroupId == groupId) ||
								g.Vlan.ServerId == null)
							.GroupJoin(PackageVlans, g => g.Vlan.VlanId, p => p.VlanId, (g, ps) => new
							{
								Vlan = g.Vlan,
								HasPackages = ps.Any()
							})
							.Where(g => !g.HasPackages)
							.Select(g => new
							{
								g.Vlan.VlanId,
								g.Vlan.Vlan,
								g.Vlan.ServerId,
							})
							.OrderByDescending(g => g.ServerId)
							.ThenBy(g => g.Vlan);
						return EntityDataReader(vlans);
					}
				}
				else
				{ // 2nd level space and below, get service location
					serverId = Services
						.Where(s => s.ServiceId == serviceId)
						.Select(s => s.ServerId)
						.Single();
					var vlans = PrivateNetworkVlans
						.Join(PackageVlans, v => v.VlanId, p => p.VlanId, (v, p) => new
						{
							Vlan = v,
							Package = p
						})
						.Where(g => g.Package.PackageId == packageId &&
							(g.Vlan.ServerId == serverId || g.Vlan.ServerId == null))
						.Select(g => new
						{
							g.Vlan.ServerId,
							g.Vlan.Vlan,
							g.Vlan.VlanId
						})
						.OrderByDescending(g => g.ServerId)
						.ThenBy(g => g.Vlan);
					return EntityDataReader(vlans);
				}
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
												 "GetUnallottedVLANs",
													 new SqlParameter("@PackageId", packageId),
													 new SqlParameter("@ServiceId", serviceId));
			}
		}

		public void AllocatePackageVLANs(int packageId, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AllocatePackageVLANs]
(
	@PackageID int,
	@xml ntext
)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @idoc int
	--Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

	-- delete
	DELETE FROM PackageVLANs
	FROM PackageVLANs AS PV
	INNER JOIN OPENXML(@idoc, '/items/item', 1) WITH 
	(
		VlanID int '@id'
	) as PX ON PV.VlanID = PX.VlanID

	-- insert
	INSERT INTO dbo.PackageVLANs
	(		
		PackageID,
		VlanID	
	)
	SELECT		
		@PackageID,
		VlanID

	FROM OPENXML(@idoc, '/items/item', 1) WITH 
	(
		VlanID int '@id'
	) as PX

	-- remove document
	exec sp_xml_removedocument @idoc

END
				*/
				#endregion

				var items = XElement.Parse(xml);
				var ids = items
					.Elements()
					.Select(e => (int)e.Attribute("id"))
					.ToArray();
				// delete
				var toDelete = PackageVlans
					.Join(ids, p => p.VlanId, id => id, (p, id) => p);
				PackageVlans.RemoveRange(toDelete);

				// insert
				PackageVlans.AddRange(
					ids.Select(id => new Data.Entities.PackageVlan()
					{
						PackageId = packageId,
						VlanId = id
					}));

				SaveChanges();
			}
			else
			{
				SqlParameter[] param = new[] {
					new SqlParameter("@PackageID", packageId),
					new SqlParameter("@xml", xml)
				};

				ExecuteLongNonQuery("AllocatePackageVLANs", param);
			}
		}
		#endregion

		#region IPAddresses
		public IDataReader GetIPAddress(int ipAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetIPAddress]
(
 @AddressID int
)
AS
BEGIN
 -- select
 SELECT
  AddressID,
  ServerID,
  ExternalIP,
  InternalIP,
  PoolID,
  SubnetMask,
  DefaultGateway,
  Comments,
  VLAN
 FROM IPAddresses
 WHERE
  AddressID = @AddressID
 RETURN
END
				*/
				#endregion

				var address = IpAddresses
					.Where(ip => ip.AddressId == ipAddressId)
					.Select(ip => new
					{
						ip.AddressId,
						ip.ServerId,
						ip.ExternalIp,
						ip.InternalIp,
						ip.PoolId,
						ip.SubnetMask,
						ip.DefaultGateway,
						ip.Comments,
						ip.Vlan
					});
				return EntityDataReader(address);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetIPAddress",
					new SqlParameter("@AddressID", ipAddressId));
			}
		}

		public IDataReader GetIPAddresses(int actorId, int poolId, int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetIPAddresses]
(
 @ActorID int,
 @PoolID int,
 @ServerID int
)
AS
BEGIN

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
 IP.AddressID,
 IP.PoolID,
 IP.ExternalIP,
 IP.InternalIP,
 IP.SubnetMask,
 IP.DefaultGateway,
 IP.Comments,
 IP.VLAN,
 IP.ServerID,
 S.ServerName,
 PA.ItemID,
 SI.ItemName,
 PA.PackageID,
 P.PackageName,
 P.UserID,
 U.UserName
FROM dbo.IPAddresses AS IP
LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON U.UserID = P.UserID
WHERE @IsAdmin = 1
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
AND (@ServerID = 0 OR @ServerID <> 0 AND IP.ServerID = @ServerID)
END
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var addresses = IpAddresses
					.Where(ip => isAdmin &&
						(poolId == 0 || (poolId != 0 && poolId == ip.PoolId)) &&
						(serverId == 0 || (serverId != 0 && serverId == ip.ServerId)))
					.Join(Servers, ip => ip.ServerId, s => s.ServerId, (ip, s) => new { Ip = ip, Server = s })
					.Join(PackageIpAddresses, g => g.Ip.AddressId, p => p.AddressId, (g, p) => new
					{
						g.Ip,
						g.Server,
						PackageIp = p
					})
					.Join(ServiceItems, g => g.PackageIp.ItemId, s => s.ItemId, (g, s) => new
					{
						g.Ip,
						g.Server,
						g.PackageIp,
						Item = s
					})
					.Join(Packages, g => g.PackageIp.PackageId, p => p.PackageId, (g, p) => new
					{
						g.Ip,
						g.Server,
						g.PackageIp,
						g.Item,
						Package = p
					})
					.Join(Users, g => g.Package.UserId, u => u.UserId, (g, u) => new
					{
						g.Ip.AddressId,
						g.Ip.PoolId,
						g.Ip.ExternalIp,
						g.Ip.InternalIp,
						g.Ip.SubnetMask,
						g.Ip.DefaultGateway,
						g.Ip.Comments,
						g.Ip.Vlan,
						g.Ip.ServerId,
						g.Server.ServerName,
						g.PackageIp.ItemId,
						g.Item.ItemName,
						g.PackageIp.PackageId,
						g.Package.PackageName,
						g.Package.UserId,
						u.Username
					});

				return EntityDataReader(addresses);
			}
			else
			{
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					"GetIPAddresses",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PoolId", poolId),
					new SqlParameter("@ServerId", serverId));
				return reader;
			}
		}

		public IDataReader GetIPAddressesPaged(int actorId, int poolId, int serverId,
			string filterColumn, string filterValue,
			string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetIPAddressesPaged]
(
 @ActorID int,
 @PoolID int,
 @ServerID int,
 @FilterColumn nvarchar(50) = '',
 @FilterValue nvarchar(50) = '',
 @SortColumn nvarchar(50),
 @StartRow int,
 @MaximumRows int
)
AS
BEGIN

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
@IsAdmin = 1
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
AND (@ServerID = 0 OR @ServerID <> 0 AND IP.ServerID = @ServerID)
'

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
 IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
  SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
 ELSE
  SET @condition = @condition + '
   AND (ExternalIP LIKE ''' + @FilterValue + '''
   OR InternalIP LIKE ''' + @FilterValue + '''
   OR DefaultGateway LIKE ''' + @FilterValue + '''
   OR ServerName LIKE ''' + @FilterValue + '''
   OR ItemName LIKE ''' + @FilterValue + '''
   OR Username LIKE ''' + @FilterValue + ''')'
END

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'IP.ExternalIP ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(IP.AddressID)
FROM dbo.IPAddresses AS IP
LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON P.UserID = U.UserID
WHERE ' + @condition + '

DECLARE @Addresses AS TABLE
(
 AddressID int
);

WITH TempItems AS (
 SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
  IP.AddressID
 FROM dbo.IPAddresses AS IP
 LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
 LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
 LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
 LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
 LEFT JOIN dbo.Users U ON U.UserID = P.UserID
 WHERE ' + @condition + '
)

INSERT INTO @Addresses
SELECT AddressID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
 IP.AddressID,
 IP.PoolID,
 IP.ExternalIP,
 IP.InternalIP,
 IP.SubnetMask,
 IP.DefaultGateway,
 IP.Comments,
 IP.VLAN,
 IP.ServerID,
 S.ServerName,
 PA.ItemID,
 SI.ItemName,
 PA.PackageID,
 P.PackageName,
 P.UserID,
 U.UserName
FROM @Addresses AS TA
INNER JOIN dbo.IPAddresses AS IP ON TA.AddressID = IP.AddressID
LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON U.UserID = P.UserID
'

exec sp_executesql @sql, N'@IsAdmin bit, @PoolID int, @ServerID int, @StartRow int, @MaximumRows int',
@IsAdmin, @PoolID, @ServerID, @StartRow, @MaximumRows

END
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var addresses = IpAddresses
					.Where(ip => isAdmin &&
						(poolId == 0 || (poolId != 0 && poolId == ip.PoolId)) &&
						(serverId == 0 || (serverId != 0 && serverId == ip.ServerId)))
					.Join(Servers, ip => ip.ServerId, s => s.ServerId, (ip, s) => new { Ip = ip, Server = s })
					.Join(PackageIpAddresses, g => g.Ip.AddressId, p => p.AddressId, (g, p) => new
					{
						g.Ip,
						g.Server,
						PackageIp = p
					})
					.Join(ServiceItems, g => g.PackageIp.ItemId, s => s.ItemId, (g, s) => new
					{
						g.Ip,
						g.Server,
						g.PackageIp,
						Item = s
					})
					.Join(Packages, g => g.PackageIp.PackageId, p => p.PackageId, (g, p) => new
					{
						g.Ip,
						g.Server,
						g.PackageIp,
						g.Item,
						Package = p
					})
					.Join(Users, g => g.Package.UserId, u => u.UserId, (g, u) => new
					{
						g.Ip.AddressId,
						g.Ip.PoolId,
						g.Ip.ExternalIp,
						g.Ip.InternalIp,
						g.Ip.SubnetMask,
						g.Ip.DefaultGateway,
						g.Ip.Comments,
						g.Ip.Vlan,
						g.Ip.ServerId,
						g.Server.ServerName,
						g.PackageIp.ItemId,
						g.Item.ItemName,
						g.PackageIp.PackageId,
						g.Package.PackageName,
						g.Package.UserId,
						u.Username
					});

				if (!string.IsNullOrEmpty(filterValue))
				{
					if (!string.IsNullOrEmpty(filterColumn))
					{
						addresses = addresses.Where($"{filterColumn}=@0", filterValue);
					}
					else
					{
						addresses = addresses.Where(a => a.ExternalIp == filterValue ||
							a.InternalIp == filterValue || a.DefaultGateway == filterValue ||
							a.ServerName == filterValue || a.ItemName == filterValue || a.Username == filterValue);
					}
				}

				if (string.IsNullOrEmpty(sortColumn)) addresses = addresses.OrderBy(a => a.ExternalIp);
				else addresses = addresses.OrderBy(sortColumn);

				addresses = addresses.Skip(startRow).Take(maximumRows);

				return EntityDataReader(addresses);
			}
			else
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
		}

		public int AddIPAddress(int poolId, int serverId, string externalIP, string internalIP,
			 string subnetMask, string defaultGateway, string comments, int VLAN)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddIPAddress]
(
 @AddressID int OUTPUT,
 @ServerID int,
 @ExternalIP varchar(24),
 @InternalIP varchar(24),
 @PoolID int,
 @SubnetMask varchar(15),
 @DefaultGateway varchar(15),
 @Comments ntext,
 @VLAN int
)
AS
BEGIN
 IF @ServerID = 0
 SET @ServerID = NULL

 INSERT INTO IPAddresses (ServerID, ExternalIP, InternalIP, PoolID, SubnetMask, DefaultGateway, Comments, VLAN)
 VALUES (@ServerID, @ExternalIP, @InternalIP, @PoolID, @SubnetMask, @DefaultGateway, @Comments, @VLAN)

 SET @AddressID = SCOPE_IDENTITY()

 RETURN
END
				*/
				#endregion

				var address = new Data.Entities.IpAddress()
				{
					PoolId = poolId,
					ServerId = serverId != 0 ? serverId : null,
					ExternalIp = externalIP,
					InternalIp = internalIP,
					SubnetMask = subnetMask,
					DefaultGateway = defaultGateway,
					Comments = comments,
					Vlan = VLAN
				};

				IpAddresses.Add(address);

				SaveChanges();

				return address.AddressId;
			}
			else
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
		}

		public void UpdateIPAddress(int addressId, int poolId, int serverId,
			 string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateIPAddress]
(
 @AddressID int,
 @ServerID int,
 @ExternalIP varchar(24),
 @InternalIP varchar(24),
 @PoolID int,
 @SubnetMask varchar(15),
 @DefaultGateway varchar(15),
 @Comments ntext,
 @VLAN int
)
AS
BEGIN
 IF @ServerID = 0
 SET @ServerID = NULL

 UPDATE IPAddresses SET
  ExternalIP = @ExternalIP,
  InternalIP = @InternalIP,
  ServerID = @ServerID,
  PoolID = @PoolID,
  SubnetMask = @SubnetMask,
  DefaultGateway = @DefaultGateway,
  Comments = @Comments,
  VLAN = @VLAN
 WHERE AddressID = @AddressID
 RETURN
END
				*/
				#endregion

				var address = IpAddresses.FirstOrDefault(ip => ip.AddressId == addressId);

				if (address != null)
				{
					address.PoolId = poolId;
					address.ServerId = serverId != null ? serverId : null;
					address.ExternalIp = externalIP;
					address.InternalIp = internalIP;
					address.SubnetMask = subnetMask;
					address.DefaultGateway = defaultGateway;
					address.Comments = comments;
					address.Vlan = VLAN;

					SaveChanges();
				}
			}
			else
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
		}

		public void UpdateIPAddresses(string xmlIds, int poolId, int serverId,
			 string subnetMask, string defaultGateway, string comments, int VLAN)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateIPAddresses]
(
 @xml ntext,
 @PoolID int,
 @ServerID int,
 @SubnetMask varchar(15),
 @DefaultGateway varchar(15),
 @Comments ntext,
 @VLAN int
)
AS
BEGIN
 SET NOCOUNT ON;
 IF @ServerID = 0
 SET @ServerID = NULL
 DECLARE @idoc int
 --Create an internal representation of the XML document.
 EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
 -- update
 UPDATE IPAddresses SET
  ServerID = @ServerID,
  PoolID = @PoolID,
  SubnetMask = @SubnetMask,
  DefaultGateway = @DefaultGateway,
  Comments = @Comments,
  VLAN = @VLAN
 FROM IPAddresses AS IP
 INNER JOIN OPENXML(@idoc, '/items/item', 1) WITH
 (
  AddressID int '@id'
 ) as PV ON IP.AddressID = PV.AddressID
 -- remove document
 exec sp_xml_removedocument @idoc
END
				*/
				#endregion

				var items = XElement.Parse(xmlIds);
				var addressIds = items
					.Elements()
					.Select(e => (int)e.Attribute("id"))
					.ToArray();

				var addresses = IpAddresses
					.Join(addressIds, ip => ip.AddressId, id => id, (ip, id) => ip)
					.ToArray();

				foreach (var ip in addresses)
				{
					ip.PoolId = poolId;
					ip.ServerId = serverId;
					ip.SubnetMask = subnetMask;
					ip.DefaultGateway = defaultGateway;
					ip.Comments = comments;
					ip.Vlan = VLAN;
				}

				if (addresses.Any()) SaveChanges();
			}
			else
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
		}

		public int DeleteIPAddress(int ipAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteIPAddress]
(
	@AddressID int,
	@Result int OUTPUT
)
AS

SET @Result = 0

IF EXISTS(SELECT RecordID FROM GlobalDnsRecords WHERE IPAddressID = @AddressID)
BEGIN
	SET @Result = -1
	RETURN
END

IF EXISTS(SELECT AddressID FROM PackageIPAddresses WHERE AddressID = @AddressID AND ItemID IS NOT NULL)
BEGIN
	SET @Result = -2

	RETURN
END

-- delete package-IP relation
DELETE FROM PackageIPAddresses
WHERE AddressID = @AddressID

-- delete IP address
DELETE FROM IPAddresses
WHERE AddressID = @AddressID

RETURN
				*/
				#endregion

				if (GlobalDnsRecords.Any(r => r.IpAddressId == ipAddressId)) return -1;
				if (PackageIpAddresses.Any(p => p.AddressId == ipAddressId && p.ItemId != null)) return -2;

				using (var transaction = Database.BeginTransaction())
				{
					// delete package-IP relation
					PackageIpAddresses.Where(p => p.AddressId == ipAddressId).ExecuteDelete(PackageIpAddresses);

					// delete IP address
					IpAddresses.Where(a => a.AddressId == ipAddressId).ExecuteDelete(IpAddresses);

					transaction.Commit();
				}

				return 0;
			}
			else
			{
				SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
				prmResult.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "DeleteIPAddress",
					 prmResult,
					 new SqlParameter("@AddressID", ipAddressId));

				return Convert.ToInt32(prmResult.Value);
			}
		}
		#endregion

		#region Clusters
		public IDataReader GetClusters(int actorId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetClusters]
(
	@ActorID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- get the list
SELECT
	ClusterID,
	ClusterName
FROM Clusters
WHERE @IsAdmin = 1

RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var clusters = Clusters.Where(c => isAdmin);

				return EntityDataReader(clusters);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetClusters",
				 new SqlParameter("@actorId", actorId));
			}
		}

		public int AddCluster(string clusterName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddCluster]
(
	@ClusterID int OUTPUT,
	@ClusterName nvarchar(100)
)
AS
INSERT INTO Clusters
(
	ClusterName
)
VALUES
(
	@ClusterName
)

SET @ClusterID = SCOPE_IDENTITY()
RETURN
				*/
				#endregion

				var cluster = new Data.Entities.Cluster() { ClusterName = clusterName };

				Clusters.Add(cluster);
				SaveChanges();

				return cluster.ClusterId;
			}
			else
			{
				SqlParameter prmId = new SqlParameter("@ClusterID", SqlDbType.Int);
				prmId.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					 ObjectQualifier + "AddCluster",
					 prmId,
					 new SqlParameter("@ClusterName", clusterName));

				return Convert.ToInt32(prmId.Value);
			}
		}

		public void DeleteCluster(int clusterId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteCluster]
(
	@ClusterID int
)
AS

-- reset cluster in services
UPDATE Services
SET ClusterID = NULL
WHERE ClusterID = @ClusterID

-- delete cluster
DELETE FROM Clusters
WHERE ClusterID = @ClusterID
RETURN
				*/
				#endregion

				using (var transaction = Database.BeginTransaction())
				{
					// reset cluster in services
#if NETFRAMEWORK
					var services = Services
						.Where(s => s.ClusterId == clusterId);
					foreach (var service in services) service.ClusterId = null;
#else
					Services.Where(s => s.ClusterId == clusterId).ExecuteUpdate(set => set.SetProperty(p => p.ClusterId, null as int?));
#endif
					// delete cluster
					Clusters
						.Where(c => c.ClusterId == clusterId)
						.ExecuteDelete(Clusters);

					transaction.Commit();
				}
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "DeleteCluster",
				 new SqlParameter("@ClusterId", clusterId));
			}
		}

		#endregion

		#region Global DNS records

		public string GetFullIPAddress(string externalIp, string internalIp)
		{
			var ip = string.Empty;

			if (!string.IsNullOrEmpty(externalIp)) ip = externalIp;

			if (!string.IsNullOrEmpty(internalIp))
			{
				if (ip == string.Empty) ip = internalIp;
				else ip = $"{ip} ({internalIp})";
			}

			return ip;
		}

		public DataSet GetDnsRecordsByService(int actorId, int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDnsRecordsByService]
(
	@ActorID int,
	@ServiceID int
)
AS

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		WHEN NR.RecordType = 'SRV' THEN CONVERT(varchar(3), NR.SrvPort) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	NR.RecordData,
	NR.MXPriority,
	NR.SrvPriority,
	NR.SrvWeight,
	NR.SrvPort,
	NR.IPAddressID,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress,
	IP.ExternalIP,
	IP.InternalIP
FROM
	GlobalDnsRecords AS NR
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID
WHERE
	NR.ServiceID = @ServiceID
RETURN
				*/
				#endregion

				var records = GlobalDnsRecords
					.Where(r => r.ServiceId == serviceId)
					.GroupJoin(IpAddresses, r => r.IpAddressId, ip => ip.AddressId, (r, ips) => new
					{
						Record = r,
						IpAddress = ips.SingleOrDefault()
					})
					.Select(g => new
					{
						g.Record.RecordId,
						g.Record.ServiceId,
						g.Record.ServerId,
						g.Record.PackageId,
						g.Record.RecordType,
						g.Record.RecordName,
						FullRecordData = g.Record.RecordType == "A" && string.IsNullOrEmpty(g.Record.RecordData) ?
							(g.IpAddress != null ? GetFullIPAddress(g.IpAddress.ExternalIp, g.IpAddress.InternalIp) : "") :
							(g.Record.RecordType == "MX" ?
								$"{g.Record.MXPriority}, {g.Record.RecordData}" :
								(g.Record.RecordType == "SRV" ? $"{g.Record.SrvPort}, {g.Record.RecordData}" :
									g.Record.RecordData)),
						g.Record.RecordData,
						g.Record.MXPriority,
						g.Record.SrvPriority,
						g.Record.SrvWeight,
						g.Record.SrvPort,
						g.Record.IpAddressId,
						IPAddress = g.IpAddress != null ? GetFullIPAddress(g.IpAddress.ExternalIp, g.IpAddress.InternalIp) : "",
						ExternalIp = g.IpAddress != null ? g.IpAddress.ExternalIp : null,
						InternalIp = g.IpAddress != null ? g.IpAddress.InternalIp : null
					});

				return EntityDataSet(records);

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByService",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServiceId", serviceId));
			}
		}

		public DataSet GetDnsRecordsByServer(int actorId, int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDnsRecordsByServer]
(
	@ActorID int,
	@ServerID int
)
AS

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		WHEN NR.RecordType = 'SRV' THEN CONVERT(varchar(3), NR.SrvPort) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	NR.MXPriority,
	NR.SrvPriority,
	NR.SrvWeight,
	NR.SrvPort,
	NR.IPAddressID,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress,
	IP.ExternalIP,
	IP.InternalIP
FROM
	GlobalDnsRecords AS NR
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID
WHERE
	NR.ServerID = @ServerID
RETURN
				*/
				#endregion

				var records = GlobalDnsRecords
					.Where(r => r.ServerId == serverId)
					.GroupJoin(IpAddresses, r => r.IpAddressId, ip => ip.AddressId, (r, ips) => new
					{
						Record = r,
						IpAddress = ips.SingleOrDefault()
					})
					.Select(g => new
					{
						g.Record.RecordId,
						g.Record.ServiceId,
						g.Record.ServerId,
						g.Record.PackageId,
						g.Record.RecordType,
						g.Record.RecordName,
						FullRecordData = g.Record.RecordType == "A" && string.IsNullOrEmpty(g.Record.RecordData) ?
							(g.IpAddress != null ? GetFullIPAddress(g.IpAddress.ExternalIp, g.IpAddress.InternalIp) : "") :
							(g.Record.RecordType == "MX" ?
								$"{g.Record.MXPriority}, {g.Record.RecordData}" :
								(g.Record.RecordType == "SRV" ? $"{g.Record.SrvPort}, {g.Record.RecordData}" :
									g.Record.RecordData)),
						g.Record.RecordData,
						g.Record.MXPriority,
						g.Record.SrvPriority,
						g.Record.SrvWeight,
						g.Record.SrvPort,
						g.Record.IpAddressId,
						IPAddress = g.IpAddress != null ? GetFullIPAddress(g.IpAddress.ExternalIp, g.IpAddress.InternalIp) : "",
						ExternalIp = g.IpAddress != null ? g.IpAddress.ExternalIp : null,
						InternalIp = g.IpAddress != null ? g.IpAddress.InternalIp : null
					});

				return EntityDataSet(records);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByServer",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@ServerId", serverId));
			}
		}

		public bool CheckActorPackageRights(int actorId, int? packageId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[CheckActorPackageRights]
(
	@ActorID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1 OR @PackageID IS NULL
RETURN 1

-- check if this is a 'system' package
IF @PackageID < 2 AND @PackageID > -1 AND dbo.CheckIsUserAdmin(@ActorID) = 0
RETURN 0

-- get package owner
DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

IF @UserID IS NULL
RETURN 1 -- unexisting package

-- check user
RETURN dbo.CheckActorUserRights(@ActorID, @UserID)

RETURN 0
END
			*/
			#endregion

			if (actorId == -1 || packageId == null) return true;

			// check if this is a 'system' package
			if (packageId >= 0 && packageId <= 1 && !CheckIsUserAdmin(actorId)) return false;

			// get package owner
			var ownerId = Packages
				.Where(p => p.PackageId == packageId)
				.Select(p => (int?)p.UserId)
				.SingleOrDefault();

			if (ownerId == null) return true; // unexisting package

			return CheckActorUserRights(actorId, ownerId.Value);
		}

		public DataSet GetDnsRecordsByPackage(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDnsRecordsByPackage]
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	NR.MXPriority,
	NR.SrvPriority,
	NR.SrvWeight,
	NR.SrvPort,
	NR.IPAddressID,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		WHEN NR.RecordType = 'SRV' THEN CONVERT(varchar(3), NR.SrvPort) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress,
	IP.ExternalIP,
	IP.InternalIP
FROM
	GlobalDnsRecords AS NR
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID
WHERE NR.PackageID = @PackageID
RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId)) throw new AccessViolationException("You are not allowed to access this package");

				var records = GlobalDnsRecords
					.Where(r => r.PackageId == packageId)
					.GroupJoin(IpAddresses, r => r.IpAddressId, ip => ip.AddressId, (r, ips) => new
					{
						Record = r,
						IpAddress = ips.SingleOrDefault()
					})
					.Select(g => new
					{
						g.Record.RecordId,
						g.Record.ServiceId,
						g.Record.ServerId,
						g.Record.PackageId,
						g.Record.RecordType,
						g.Record.RecordName,
						FullRecordData = g.Record.RecordType == "A" && string.IsNullOrEmpty(g.Record.RecordData) ?
							(g.IpAddress != null ? GetFullIPAddress(g.IpAddress.ExternalIp, g.IpAddress.InternalIp) : "") :
							(g.Record.RecordType == "MX" ?
								$"{g.Record.MXPriority}, {g.Record.RecordData}" :
								(g.Record.RecordType == "SRV" ? $"{g.Record.SrvPort}, {g.Record.RecordData}" :
									g.Record.RecordData)),
						g.Record.RecordData,
						g.Record.MXPriority,
						g.Record.SrvPriority,
						g.Record.SrvWeight,
						g.Record.SrvPort,
						g.Record.IpAddressId,
						IPAddress = g.IpAddress != null ? GetFullIPAddress(g.IpAddress.ExternalIp, g.IpAddress.InternalIp) : "",
						ExternalIp = g.IpAddress != null ? g.IpAddress.ExternalIp : null,
						InternalIp = g.IpAddress != null ? g.IpAddress.InternalIp : null
					});

				return EntityDataSet(records);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByPackage",
				 new SqlParameter("@actorId", actorId),
				 new SqlParameter("@PackageId", packageId));
			}
		}

		public DataSet GetDnsRecordsByGroup(int groupId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDnsRecordsByGroup]
(
	@GroupID int
)
AS
SELECT
	RGR.RecordID,
	RGR.RecordOrder,
	RGR.GroupID,
	RGR.RecordType,
	RGR.RecordName,
	RGR.RecordData,
	RGR.MXPriority
FROM
	ResourceGroupDnsRecords AS RGR
WHERE RGR.GroupID = @GroupID
ORDER BY RGR.RecordOrder
RETURN
				*/
				#endregion

				var records = ResourceGroupDnsRecords
					.Where(r => r.GroupId == groupId)
					.OrderBy(r => r.RecordOrder)
					.Select(r => new
					{
						r.RecordId,
						r.RecordOrder,
						r.GroupId,
						r.RecordType,
						r.RecordName,
						r.RecordData,
						r.MXPriority
					});

				return EntityDataSet(records);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDnsRecordsByGroup",
				 new SqlParameter("@GroupId", groupId));
			}
		}
		public DataSet GetDnsRecordsTotal(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDnsRecordsTotal]
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- create temp table for DNS records
DECLARE @Records TABLE
(
	RecordID int,
	RecordType nvarchar(10),
	RecordName nvarchar(50)
)

-- select PACKAGES DNS records
DECLARE @ParentPackageID int, @TmpPackageID int
SET @TmpPackageID = @PackageID

WHILE 10 = 10
BEGIN

	-- get DNS records for the current package
	INSERT INTO @Records (RecordID, RecordType, RecordName)
	SELECT
		GR.RecordID,
		GR.RecordType,
		GR.RecordName
	FROM GlobalDNSRecords AS GR
	WHERE GR.PackageID = @TmpPackageID
	AND GR.RecordType + GR.RecordName NOT IN (SELECT RecordType + RecordName FROM @Records)

	SET @ParentPackageID = NULL

	-- get parent package
	SELECT
		@ParentPackageID = ParentPackageID
	FROM Packages
	WHERE PackageID = @TmpPackageID

	IF @ParentPackageID IS NULL -- the last parent
	BREAK

	SET @TmpPackageID = @ParentPackageID
END

-- select VIRTUAL SERVER DNS records
DECLARE @ServerID int
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

INSERT INTO @Records (RecordID, RecordType, RecordName)
SELECT
	GR.RecordID,
	GR.RecordType,
	GR.RecordName
FROM GlobalDNSRecords AS GR
WHERE GR.ServerID = @ServerID
AND GR.RecordType + GR.RecordName NOT IN (SELECT RecordType + RecordName FROM @Records)

-- select SERVER DNS records
INSERT INTO @Records (RecordID, RecordType, RecordName)
SELECT
	GR.RecordID,
	GR.RecordType,
	GR.RecordName
FROM GlobalDNSRecords AS GR
WHERE GR.ServerID IN (SELECT
	SRV.ServerID
FROM VirtualServices AS VS
INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
WHERE VS.ServerID = @ServerID)
AND GR.RecordType + GR.RecordName NOT IN (SELECT RecordType + RecordName FROM @Records)

-- select SERVICES DNS records
-- re-distribute package services
EXEC DistributePackageServices @ActorID, @PackageID

--INSERT INTO @Records (RecordID, RecordType, RecordName)
--SELECT
--	GR.RecordID,
--	GR.RecordType,
	-- GR.RecordName
-- FROM GlobalDNSRecords AS GR
-- WHERE GR.ServiceID IN (SELECT ServiceID FROM PackageServices WHERE PackageID = @PackageID)
-- AND GR.RecordType + GR.RecordName NOT IN (SELECT RecordType + RecordName FROM @Records)

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	NR.MXPriority,
	NR.SrvPriority,
	NR.SrvWeight,
	NR.SrvPort,
	NR.IPAddressID,
	ISNULL(IP.ExternalIP, '') AS ExternalIP,
	ISNULL(IP.InternalIP, '') AS InternalIP,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		WHEN NR.RecordType = 'SRV' THEN CONVERT(varchar(3), NR.SrvPort) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress
FROM @Records AS TR
INNER JOIN GlobalDnsRecords AS NR ON TR.RecordID = NR.RecordID
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID

RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId)) throw new AccessViolationException("You are not allowed to access this package");

				// select PACKAGES DNS records
				var pid = (int?)packageId;

				IQueryable<Data.Entities.GlobalDnsRecord> records = GlobalDnsRecords
					.Where(r => r.PackageId == pid);
				pid = Packages
					.Where(p => p.PackageId == pid)
					.Select(p => p.ParentPackageId)
					.SingleOrDefault();

				while (pid != null)
				{
					records = records
						.Union(GlobalDnsRecords
							.Where(r => r.PackageId == pid));
					pid = Packages
						.Where(p => p.PackageId == pid)
						.Select(p => p.ParentPackageId)
						.SingleOrDefault();
				}

				// select VIRTUAL SERVER DNS records
				var serverId = Packages
					.Where(p => p.PackageId == packageId)
					.Select(p => p.ServerId)
					.SingleOrDefault();

				records = records
					.Union(GlobalDnsRecords
						.Where(r => r.ServerId == serverId));

				// select SERVER DNS records
				records = records
					.Union(GlobalDnsRecords
						.Join(Servers, r => r.ServerId, s => s.ServerId, (r, s) => new
						{
							Record = r,
							s.ServerId
						})
						.Join(Services, g => g.ServerId, s => s.ServerId, (g, s) => new
						{
							g.Record,
							s.ServiceId
						})
						.Join(VirtualServices, g => g.ServiceId, v => v.ServiceId, (g, v) => new
						{
							g.Record,
							v.ServerId
						})
						.Where(g => g.ServerId == serverId)
						.Select(g => g.Record));

				// select SERVICES DNS records

				// re-distribute package services
				DistributePackageServices(actorId, packageId);

				/* TODO uncomment this?
				records = records
					.Union(GlobalDnsRecords
						.Where(r => Packages
							.Include(p => p.Services)
							.Any(p => p.PackageId == packageId && p.Services.Any(s => s.ServiceId == r.ServiceId))));
				*/

#if NETCOREAPP
				records = records.DistinctBy(r => r.RecordType + r.RecordName);
#else
				records = records.GroupBy(r => new { r.RecordType, r.RecordName })
					.Select(g => g.First());
#endif
				var recordsSelected = records
					.GroupJoin(IpAddresses, r => r.IpAddressId, ip => ip.AddressId, (r, ip) => new
					{
						Record = r,
						Ip = ip.SingleOrDefault()
					})
					.Select(r => new
					{
						r.Record.RecordId,
						r.Record.ServiceId,
						r.Record.ServerId,
						r.Record.PackageId,
						r.Record.RecordType,
						r.Record.RecordName,
						r.Record.RecordData,
						r.Record.MXPriority,
						r.Record.SrvPriority,
						r.Record.SrvWeight,
						r.Record.SrvPort,
						r.Record.IpAddressId,
						ExternalIp = r.Ip != null && r.Ip.ExternalIp != null ? r.Ip.ExternalIp : "",
						InternalIp = r.Ip != null && r.Ip.InternalIp != null ? r.Ip.InternalIp : "",
						FullRecordData = r.Record.RecordType == "A" && string.IsNullOrEmpty(r.Record.RecordData) ?
							(r.Ip != null ? GetFullIPAddress(r.Ip.ExternalIp, r.Ip.InternalIp) : "") :
							(r.Record.RecordType == "MX" ?
								$"{r.Record.MXPriority}, {r.Record.RecordData}" :
								(r.Record.RecordType == "SRV" ? $"{r.Record.SrvPort}, {r.Record.RecordData}" :
									r.Record.RecordData)),
						IPAddress = r.Ip != null ? GetFullIPAddress(r.Ip.ExternalIp, r.Ip.InternalIp) : ""
					});

				return EntityDataSet(recordsSelected);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetDnsRecordsTotal",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@packageId", packageId));
			}
		}

		public IDataReader GetDnsRecord(int actorId, int recordId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDnsRecord]
(
	@ActorID int,
	@RecordID int
)
AS

-- check rights
DECLARE @ServiceID int, @ServerID int, @PackageID int
SELECT
	@ServiceID = ServiceID,
	@ServerID = ServerID,
	@PackageID = PackageID
FROM GlobalDnsRecords
WHERE
	RecordID = @RecordID

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RAISERROR('You are not allowed to perform this operation', 16, 1)

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	NR.MXPriority,
	NR.SrvPriority,
	NR.SrvWeight,
	NR.SrvPort,
	NR.IPAddressID
FROM
	GlobalDnsRecords AS NR
WHERE NR.RecordID = @RecordID
RETURN
				*/
				#endregion

				// check rights
				var records = GlobalDnsRecords
					.Where(r => r.RecordId == recordId)
					.Select(r => new
					{
						r.RecordId,
						r.ServiceId,
						r.ServerId,
						r.PackageId,
						r.RecordType,
						r.RecordName,
						r.RecordData,
						r.MXPriority,
						r.SrvPriority,
						r.SrvWeight,
						r.SrvPort,
						r.IpAddressId
					})
					.ToArray();
				var record = records
					.SingleOrDefault();

				if (record != null && (record.ServiceId > 0 || record.ServerId > 0) && !CheckIsUserAdmin(actorId))
					throw new AccessViolationException("You are not allowed to perform this operation");

				if (record != null && record.PackageId > 0 && !CheckActorPackageRights(actorId, record.PackageId))
					throw new AccessViolationException("You are not allowed to access this package");

				return EntityDataReader(records);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetDnsRecord",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@RecordId", recordId));
			}
		}

		public void AddDnsRecord(int actorId, int serviceId, int serverId, int packageId, string recordType,
			 string recordName, string recordData, int mxPriority, int SrvPriority, int SrvWeight, int SrvPort, int ipAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddDnsRecord]
(
	@ActorID int,
	@ServiceID int,
	@ServerID int,
	@PackageID int,
	@RecordType nvarchar(10),
	@RecordName nvarchar(50),
	@RecordData nvarchar(500),
	@MXPriority int,
	@SrvPriority int,
	@SrvWeight int,
	@SrvPort int,
	@IPAddressID int
)
AS

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RAISERROR('You should have administrator role to perform such operation', 16, 1)

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

IF @ServiceID = 0 SET @ServiceID = NULL
IF @ServerID = 0 SET @ServerID = NULL
IF @PackageID = 0 SET @PackageID = NULL
IF @IPAddressID = 0 SET @IPAddressID = NULL

IF EXISTS
(
	SELECT RecordID FROM GlobalDnsRecords WHERE
	ServiceID = @ServiceID AND ServerID = @ServerID AND PackageID = @PackageID
	AND RecordName = @RecordName AND RecordType = @RecordType
)

	UPDATE GlobalDnsRecords
	SET
		RecordData = RecordData,
		MXPriority = MXPriority,
		SrvPriority = SrvPriority,
		SrvWeight = SrvWeight,
		SrvPort = SrvPort,

		IPAddressID = @IPAddressID
	WHERE
		ServiceID = @ServiceID AND ServerID = @ServerID AND PackageID = @PackageID
ELSE
	INSERT INTO GlobalDnsRecords
	(
		ServiceID,
		ServerID,
		PackageID,
		RecordType,
		RecordName,
		RecordData,
		MXPriority,
		SrvPriority,
		SrvWeight,
		SrvPort,
		IPAddressID
	)
	VALUES
	(
		@ServiceID,
		@ServerID,
		@PackageID,
		@RecordType,
		@RecordName,
		@RecordData,
		@MXPriority,
		@SrvPriority,
		@SrvWeight,
		@SrvPort,
		@IPAddressID
	)

RETURN
				*/
				#endregion

				if ((serviceId > 0 || serverId > 0) && !CheckIsUserAdmin(actorId))
					throw new AccessViolationException("You should have administrator role to perform such operation");

				if (packageId > 0 && !CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				int? serverIdOrNull = serverId != 0 ? serverId : null;
				int? serviceIdOrNull = serviceId != 0 ? serviceId : null;
				int? packageIdOrNull = packageId != 0 ? packageId : null;
				int? ipAddressIdOrNull = ipAddressId != 0 ? ipAddressId : null;

				var record = GlobalDnsRecords.FirstOrDefault(r => r.ServiceId == serviceIdOrNull && r.ServerId == serverIdOrNull &&
					r.PackageId == packageIdOrNull && r.RecordName == recordName && r.RecordType == recordType);

				if (record == null)
				{
					record = new Data.Entities.GlobalDnsRecord()
					{
						RecordType = recordType,
						RecordName = recordName,
						ServiceId = serviceIdOrNull,
						ServerId = serverIdOrNull,
						PackageId = packageIdOrNull
					};
					GlobalDnsRecords.Add(record);
				}

				record.RecordData = recordData;
				record.MXPriority = mxPriority;
				record.SrvPriority = SrvPriority;
				record.SrvWeight = SrvWeight;
				record.SrvPort = SrvPort;
				record.IpAddressId = ipAddressIdOrNull;

				SaveChanges();
			}
			else
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
		}
		public void UpdateDnsRecord(int actorId, int recordId, string recordType,
			 string recordName, string recordData, int mxPriority, int SrvPriority, int SrvWeight, int SrvPort, int ipAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateDnsRecord]
(
	@ActorID int,
	@RecordID int,
	@RecordType nvarchar(10),
	@RecordName nvarchar(50),
	@RecordData nvarchar(500),
	@MXPriority int,
	@SrvPriority int,
	@SrvWeight int,
	@SrvPort int,
	@IPAddressID int
)
AS

IF @IPAddressID = 0 SET @IPAddressID = NULL

-- check rights
DECLARE @ServiceID int, @ServerID int, @PackageID int
SELECT
	@ServiceID = ServiceID,
	@ServerID = ServerID,
	@PackageID = PackageID
FROM GlobalDnsRecords
WHERE
	RecordID = @RecordID

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RAISERROR('You are not allowed to perform this operation', 16, 1)

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- update record
UPDATE GlobalDnsRecords
SET
	RecordType = @RecordType,
	RecordName = @RecordName,
	RecordData = @RecordData,
	MXPriority = @MXPriority,
	SrvPriority = @SrvPriority,
	SrvWeight = @SrvWeight,
	SrvPort = @SrvPort,
	IPAddressID = @IPAddressID
WHERE
	RecordID = @RecordID
RETURN
				*/
				#endregion

				int? ipAddressIdOrNull = ipAddressId != 0 ? ipAddressId : null;

				var record = GlobalDnsRecords
					.FirstOrDefault(r => r.RecordId == recordId);

				if (record != null)
				{
					// check rights
					if ((record.ServiceId > 0 || record.ServerId > 0) && !CheckIsUserAdmin(actorId))
						throw new AccessViolationException("You are not allowed to perform this operation");

					if (record.PackageId > 0 && !CheckActorPackageRights(actorId, record.PackageId))
						throw new AccessViolationException("You are not allowed to access this package");

					record.RecordType = recordType;
					record.RecordName = recordName;
					record.RecordData = recordData;
					record.MXPriority = mxPriority;
					record.SrvPriority = SrvPriority;
					record.SrvWeight = SrvWeight;
					record.SrvPort = SrvPort;
					record.IpAddressId = ipAddressIdOrNull;

					SaveChanges();
				}
			}
			else
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
		}


		public void DeleteDnsRecord(int actorId, int recordId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteDnsRecord]
(
	@ActorID int,
	@RecordID int
)
AS

-- check rights
DECLARE @ServiceID int, @ServerID int, @PackageID int
SELECT
	@ServiceID = ServiceID,
	@ServerID = ServerID,
	@PackageID = PackageID
FROM GlobalDnsRecords
WHERE
	RecordID = @RecordID

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RETURN

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RETURN

-- delete record
DELETE FROM GlobalDnsRecords
WHERE RecordID = @RecordID

RETURN
				*/
				#endregion

				var record = GlobalDnsRecords
					.FirstOrDefault(r => r.RecordId == recordId);

				if (record != null)
				{
					// check rights
					if ((record.ServerId > 0 || record.ServiceId > 0) && !CheckIsUserAdmin(actorId) ||
						record.PackageId > 0 && !CheckActorPackageRights(actorId, record.PackageId))
						return;

					GlobalDnsRecords.Remove(record);

					SaveChanges();
				}
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteDnsRecord",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@RecordId", recordId));
			}
		}
		#endregion

		#region Domains

		public IEnumerable<int> PackagesTree(int packageId, bool recursive = false)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[PackagesTree]
(
	@PackageID int,
	@Recursive bit = 0
)
RETURNS @T TABLE (PackageID int)
AS
BEGIN

INSERT INTO @T VALUES (@PackageID)

IF @Recursive = 1
BEGIN
	WITH RecursivePackages(ParentPackageID, PackageID, PackageLevel) AS
	(
		SELECT ParentPackageID, PackageID, 0 AS PackageLevel
		FROM Packages
		WHERE ParentPackageID = @PackageID
		UNION ALL
		SELECT p.ParentPackageID, p.PackageID, PackageLevel + 1
		FROM Packages p
			INNER JOIN RecursivePackages d
			ON p.ParentPackageID = d.PackageID
		WHERE @Recursive = 1
	)
	INSERT INTO @T
	SELECT PackageID
	FROM RecursivePackages
END

RETURN
END
			*/
			#endregion

			IEnumerable<int> packages = new int[] { packageId };

			if (recursive)
			{
				var children = packages;
				children = children
					.Join(Packages, child => child, pkg => pkg.ParentPackageId, (ch, pkg) => pkg.PackageId);

				while (children.Any())
				{
					packages = packages.Union(children);
					children = children
						.Join(Packages, child => child, pkg => pkg.ParentPackageId, (ch, pkg) => pkg.PackageId);
				}
			}

			return packages;
		}
		public DataSet GetDomains(int actorId, int packageId, bool recursive = true)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDomains]
(
	@ActorID int,
	@PackageID int,
	@Recursive bit = 1
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(WS.ItemID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(MD.ItemID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsPreviewDomain,
	D.CreationDate,
	D.ExpirationDate,
	D.LastUpdateDate,
	D.IsDomainPointer,
	D.RegistrarName
FROM Domains AS D
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON D.PackageID = PT.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
RETURN
				*/
				#endregion

				// check rights
				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var domains = Domains
					.Join(PackagesTree(actorId, recursive), d => d.PackageId, t => t, (d, t) => d)
					.GroupJoin(ServiceItems, d => d.WebSiteId, it => it.ItemId, (d, s) => new
					{
						Domain = d,
						WebSite = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.MailDomainId, it => it.ItemId, (d, s) => new
					{
						d.Domain,
						d.WebSite,
						MailDomain = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.ZoneItemId, it => it.ItemId, (d, s) => new
					{
						d.Domain,
						d.WebSite,
						d.MailDomain,
						Zone = s.SingleOrDefault()
					})
					.Select(d => new
					{
						d.Domain.DomainId,
						d.Domain.PackageId,
						d.Domain.ZoneItemId,
						d.Domain.DomainItemId,
						d.Domain.DomainName,
						d.Domain.HostingAllowed,
						WebSiteId = d.WebSite != null ? d.WebSite.ItemId : 0,
						WebSiteName = d.WebSite != null ? d.WebSite.ItemName : null,
						MailDomainId = d.MailDomain != null ? d.MailDomain.ItemId : 0,
						MailDomainName = d.MailDomain != null ? d.MailDomain.ItemName : null,
						ZoneName = d.Zone != null ? d.Zone.ItemName : null,
						d.Domain.IsSubDomain,
						d.Domain.IsPreviewDomain,
						d.Domain.IsDomainPointer
					});

				return EntityDataSet(domains);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				 ObjectQualifier + "GetDomains",
				 new SqlParameter("@ActorId", actorId),
				 new SqlParameter("@PackageId", packageId),
				 new SqlParameter("@Recursive", recursive));
			}
		}

		public DataSet GetResellerDomains(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetResellerDomains]
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- load parent package
DECLARE @ParentPackageID int
SELECT @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainName,
	D.HostingAllowed,
	D.WebSiteID,
	WS.ItemName,
	D.MailDomainID,
	MD.ItemName
FROM Domains AS D
INNER JOIN PackagesTree(@ParentPackageID, 0) AS PT ON D.PackageID = PT.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
WHERE HostingAllowed = 1
RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				// load parent package
				var parentPackageId = Packages
					.Where(p => p.PackageId == packageId)
					.Select(p => p.ParentPackageId)
					.FirstOrDefault();

				var domains = Domains
					.Where(d => d.HostingAllowed)
					.Join(PackagesTree(parentPackageId ?? -1, false), d => d.PackageId, t => t, (d, t) => d)
					.GroupJoin(ServiceItems, d => d.WebSiteId, it => it.ItemId, (d, s) => new
					{
						Domain = d,
						WebSite = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.MailDomainId, it => it.ItemId, (d, s) => new
					{
						d.Domain,
						d.WebSite,
						MailDomain = s.SingleOrDefault()
					})
					.Select(d => new
					{
						d.Domain.DomainId,
						d.Domain.PackageId,
						d.Domain.ZoneItemId,
						d.Domain.DomainName,
						d.Domain.HostingAllowed,
						d.Domain.WebSiteId,
						WebSiteName = d.WebSite != null ? d.WebSite.ItemName : null,
						d.Domain.MailDomainId,
						MailDomainName = d.MailDomain != null ? d.MailDomain.ItemName : null
					});

				return EntityDataSet(domains);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetResellerDomains",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageId", packageId));
			}
		}

		public DataSet GetDomainsPaged(int actorId, int packageId, int serverId, bool recursive, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored procedure
				/*
				CREATE PROCEDURE [dbo].[GetDomainsPaged]
				(
					@ActorID int,
					@PackageID int,
					@ServerID int,
					@Recursive bit,
					@FilterColumn nvarchar(50) = '',
					@FilterValue nvarchar(50) = '',
					@SortColumn nvarchar(50),
					@StartRow int,
					@MaximumRows int
				)
				AS
				SET NOCOUNT ON

				-- check rights
				IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
				RAISERROR('You are not allowed to access this package', 16, 1)

				-- build query and run it to the temporary table
				DECLARE @sql nvarchar(2500)

				IF @SortColumn = '' OR @SortColumn IS NULL
				SET @SortColumn = 'DomainName'

				SET @sql = '
				DECLARE @Domains TABLE
				(
					ItemPosition int IDENTITY(1,1),
					DomainID int
				)
				INSERT INTO @Domains (DomainID)
				SELECT
					D.DomainID
				FROM Domains AS D
				INNER JOIN Packages AS P ON D.PackageID = P.PackageID
				INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
				LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
				LEFT OUTER JOIN Services AS S ON Z.ServiceID = S.ServiceID
				LEFT OUTER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
				WHERE (D.IsPreviewDomain = 0 AND D.IsDomainPointer = 0) AND
						((@Recursive = 0 AND D.PackageID = @PackageID)
						OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, D.PackageID) = 1))
				AND (@ServerID = 0 OR (@ServerID > 0 AND S.ServerID = @ServerID))
				'

				IF @FilterValue <> ''
				BEGIN
					IF @FilterColumn <> ''
					BEGIN
						SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '
					END
					ELSE
						SET @sql = @sql + '
						AND (DomainName LIKE @FilterValue 
						OR Username LIKE @FilterValue
						OR ServerName LIKE @FilterValue
						OR PackageName LIKE @FilterValue) '
				END

				IF @SortColumn <> '' AND @SortColumn IS NOT NULL
				SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

				SET @sql = @sql + ' SELECT COUNT(DomainID) FROM @Domains;SELECT
					D.DomainID,
					D.PackageID,
					D.ZoneItemID,
					D.DomainItemID,
					D.DomainName,
					D.HostingAllowed,
					ISNULL(WS.ItemID, 0) AS WebSiteID,
					WS.ItemName AS WebSiteName,
					ISNULL(MD.ItemID, 0) AS MailDomainID,
					MD.ItemName AS MailDomainName,
					D.IsSubDomain,
					D.IsPreviewDomain,
					D.IsDomainPointer,
					D.ExpirationDate,
					D.LastUpdateDate,
					D.RegistrarName,
					P.PackageName,
					ISNULL(SRV.ServerID, 0) AS ServerID,
					ISNULL(SRV.ServerName, '''') AS ServerName,
					ISNULL(SRV.Comments, '''') AS ServerComments,
					ISNULL(SRV.VirtualServer, 0) AS VirtualServer,
					P.UserID,
					U.Username,
					U.FirstName,
					U.LastName,
					U.FullName,
					U.RoleID,
					U.Email
				FROM @Domains AS SD
				INNER JOIN Domains AS D ON SD.DomainID = D.DomainID
				INNER JOIN Packages AS P ON D.PackageID = P.PackageID
				INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
				LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
				LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
				LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
				LEFT OUTER JOIN Services AS S ON Z.ServiceID = S.ServiceID
				LEFT OUTER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
				WHERE SD.ItemPosition BETWEEN @StartRow + 1 AND @StartRow + @MaximumRows'

				exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @PackageID int, @FilterValue nvarchar(50), @ServerID int, @Recursive bit', 
				@StartRow, @MaximumRows, @PackageID, @FilterValue, @ServerID, @Recursive

				RETURN
				*/
				#endregion

				// check rights
				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var domains = Domains
					.Where(d => !recursive && d.PackageId == packageId ||
						recursive && CheckPackageParent(packageId, d.PackageId))
					.Join(Packages, d => d.PackageId, p => p.PackageId, (d, p) => new
					{
						Domain = d,
						Package = p
					})
					.Join(UsersDetailed, d => d.Package.UserId, u => u.UserId, (d, u) => new
					{
						d.Domain,
						d.Package,
						User = u
					})
					.GroupJoin(ServiceItems, d => d.Domain.WebSiteId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.User,
						WebSite = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.MailDomainId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.User,
						d.WebSite,
						MailDomain = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.ZoneItemId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.User,
						d.WebSite,
						d.MailDomain,
						Zone = s.SingleOrDefault()
					})
					.GroupJoin(Services, d => d.Zone != null ? d.Zone.ServiceId : null, s => s.ServiceId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.User,
						d.WebSite,
						d.MailDomain,
						d.Zone,
						Service = s.SingleOrDefault()
					})
					.GroupJoin(Servers, d => d.Service != null ? (int?)d.Service.ServerId : null, s => (int?)s.ServerId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.User,
						d.WebSite,
						d.MailDomain,
						d.Zone,
						d.Service,
						Server = s.SingleOrDefault()
					})
					.Select(d => new
					{
						d.Domain.DomainId,
						d.Domain.PackageId,
						d.Domain.ZoneItemId,
						d.Domain.DomainItemId,
						d.Domain.DomainName,
						d.Domain.HostingAllowed,
						WebSiteId = d.WebSite != null ? d.WebSite.ItemId : 0,
						WebSiteName = d.WebSite != null ? d.WebSite.ItemName : null,
						MailDomainId = d.MailDomain != null ? d.MailDomain.ItemId : 0,
						MailDomainName = d.MailDomain != null ? d.MailDomain.ItemName : null,
						d.Domain.IsSubDomain,
						d.Domain.IsPreviewDomain,
						d.Domain.IsDomainPointer,
						d.Domain.ExpirationDate,
						d.Domain.LastUpdateDate,
						d.Domain.RegistrarName,
						d.Package.PackageName,
						ServerId = d.Server != null ? d.Server.ServerId : 0,
						ServerName = d.Server != null ? d.Server.ServerName : "",
						ServerComments = d.Server != null ? d.Server.Comments : "",
						VirtualServer = d.Server != null ? d.Server.VirtualServer : false,
						d.Package.UserId,
						d.User.Username,
						d.User.FirstName,
						d.User.LastName,
						d.User.FullName,
						d.User.RoleId,
						d.User.Email
					});

				if (!string.IsNullOrEmpty(filterValue))
				{
					if (!string.IsNullOrEmpty(filterColumn))
					{
						domains = domains.Where($"{filterColumn}=@0", filterValue);
					}
					else
					{
						domains = domains.Where(d => d.DomainName == filterValue || d.Username == filterValue ||
							d.ServerName == filterValue || d.PackageName == filterValue);
					}
				}

				if (!string.IsNullOrEmpty(sortColumn)) domains = domains.OrderBy(sortColumn);
				else domains = domains.OrderBy(d => d.DomainName);

				domains = domains.Skip(startRow).Take(maximumRows);

				return EntityDataSet(domains);
			}
			else
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
		}

		public IDataReader GetDomain(int actorId, int domainId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDomain]
(
	@ActorID int,
	@DomainID int
)
AS

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(WS.ItemID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(MD.ItemID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsPreviewDomain,
	D.IsDomainPointer
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
WHERE
	D.DomainID = @DomainID
	AND dbo.CheckActorPackageRights(@ActorID, P.PackageID) = 1
RETURN
				*/
				#endregion

				var domains = Domains
					.Where(d => d.DomainId == domainId && CheckActorPackageRights(actorId, d.PackageId))
					.Join(Packages, d => d.PackageId, p => p.PackageId, (d, p) => new
					{
						Domain = d,
						Package = p
					})
					.GroupJoin(ServiceItems, d => d.Domain.WebSiteId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						WebSite = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.MailDomainId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						MailDomain = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.ZoneItemId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						d.MailDomain,
						Zone = s.SingleOrDefault()
					})
					.Select(d => new
					{
						d.Domain.DomainId,
						d.Domain.PackageId,
						d.Domain.ZoneItemId,
						d.Domain.DomainItemId,
						d.Domain.DomainName,
						d.Domain.HostingAllowed,
						WebSiteId = d.WebSite != null ? d.WebSite.ItemId : 0,
						WebSiteName = d.WebSite != null ? d.WebSite.ItemName : null,
						MailDomainId = d.MailDomain != null ? d.MailDomain.ItemId : 0,
						MailDomainName = d.MailDomain != null ? d.MailDomain.ItemName : null,
						ZoneName = d.Zone != null ? d.Zone.ItemName : null,
						d.Domain.IsSubDomain,
						d.Domain.IsPreviewDomain,
						d.Domain.IsDomainPointer
					});

				return EntityDataReader(domains);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetDomain",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@domainId", domainId));
			}
		}

		public IDataReader GetDomainByName(int actorId, string domainName, bool searchOnDomainPointer, bool isDomainPointer)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDomainByName]
(
	@ActorID int,
	@DomainName nvarchar(100),
	@SearchOnDomainPointer bit,
	@IsDomainPointer bit
)
AS

IF (@SearchOnDomainPointer = 1)
BEGIN
	SELECT
		D.DomainID,
		D.PackageID,
		D.ZoneItemID,
		D.DomainItemID,
		D.DomainName,
		D.HostingAllowed,
		ISNULL(D.WebSiteID, 0) AS WebSiteID,
		WS.ItemName AS WebSiteName,
		ISNULL(D.MailDomainID, 0) AS MailDomainID,
		MD.ItemName AS MailDomainName,
		Z.ItemName AS ZoneName,
		D.IsSubDomain,
		D.IsPreviewDomain,
		D.IsDomainPointer
	FROM Domains AS D
	INNER JOIN Packages AS P ON D.PackageID = P.PackageID
	LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
	LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
	LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
	WHERE
		D.DomainName = @DomainName
		AND D.IsDomainPointer = @IsDomainPointer
		AND dbo.CheckActorPackageRights(@ActorID, P.PackageID) = 1
	RETURN
END
ELSE
BEGIN
	SELECT
		D.DomainID,
		D.PackageID,
		D.ZoneItemID,
		D.DomainItemID,
		D.DomainName,
		D.HostingAllowed,
		ISNULL(D.WebSiteID, 0) AS WebSiteID,
		WS.ItemName AS WebSiteName,
		ISNULL(D.MailDomainID, 0) AS MailDomainID,
		MD.ItemName AS MailDomainName,
		Z.ItemName AS ZoneName,
		D.IsSubDomain,
		D.IsPreviewDomain,
		D.IsDomainPointer
	FROM Domains AS D
	INNER JOIN Packages AS P ON D.PackageID = P.PackageID
	LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
	LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
	LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
	WHERE
		D.DomainName = @DomainName
		AND dbo.CheckActorPackageRights(@ActorID, P.PackageID) = 1
	RETURN
END
				*/
				#endregion

				var domainsRaw = Domains
					.Where(d => d.DomainName == domainName && CheckActorPackageRights(actorId, d.PackageId));

				if (searchOnDomainPointer) domainsRaw = domainsRaw.Where(d => d.IsDomainPointer == isDomainPointer);

				var domains = domainsRaw
					.Join(Packages, d => d.PackageId, p => p.PackageId, (d, p) => new
					{
						Domain = d,
						Package = p
					})
					.GroupJoin(ServiceItems, d => d.Domain.WebSiteId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						WebSite = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.MailDomainId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						MailDomain = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.ZoneItemId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						d.MailDomain,
						Zone = s.SingleOrDefault()
					})
					.Select(d => new
					{
						d.Domain.DomainId,
						d.Domain.PackageId,
						d.Domain.ZoneItemId,
						d.Domain.DomainItemId,
						d.Domain.DomainName,
						d.Domain.HostingAllowed,
						WebSiteId = d.WebSite != null ? d.WebSite.ItemId : 0,
						WebSiteName = d.WebSite != null ? d.WebSite.ItemName : null,
						MailDomainId = d.MailDomain != null ? d.MailDomain.ItemId : 0,
						MailDomainName = d.MailDomain != null ? d.MailDomain.ItemName : null,
						ZoneName = d.Zone != null ? d.Zone.ItemName : null,
						d.Domain.IsSubDomain,
						d.Domain.IsPreviewDomain,
						d.Domain.IsDomainPointer
					});

				return EntityDataReader(domains);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetDomainByName",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@domainName", domainName),
					new SqlParameter("@SearchOnDomainPointer", searchOnDomainPointer),
					new SqlParameter("@IsDomainPointer", isDomainPointer));
			}
		}


		public DataSet GetDomainsByZoneId(int actorId, int zoneId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDomainsByZoneID]
(
	@ActorID int,
	@ZoneID int
)
AS

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(D.WebSiteID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(D.MailDomainID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsPreviewDomain,
	D.IsDomainPointer
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
WHERE
	D.ZoneItemID = @ZoneID
	AND dbo.CheckActorPackageRights(@ActorID, P.PackageID) = 1
RETURN
				*/
				#endregion

				var domains = Domains
					.Where(d => d.ZoneItemId == zoneId && CheckActorPackageRights(actorId, d.PackageId))
					.Join(Packages, d => d.PackageId, p => p.PackageId, (d, p) => new
					{
						Domain = d,
						Package = p
					})
					.GroupJoin(ServiceItems, d => d.Domain.WebSiteId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						WebSite = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.MailDomainId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						MailDomain = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.ZoneItemId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						d.MailDomain,
						Zone = s.SingleOrDefault()
					})
					.Select(d => new
					{
						d.Domain.DomainId,
						d.Domain.PackageId,
						d.Domain.ZoneItemId,
						d.Domain.DomainItemId,
						d.Domain.DomainName,
						d.Domain.HostingAllowed,
						WebSiteId = d.WebSite != null ? d.WebSite.ItemId : 0,
						WebSiteName = d.WebSite != null ? d.WebSite.ItemName : null,
						MailDomainId = d.MailDomain != null ? d.MailDomain.ItemId : 0,
						MailDomainName = d.MailDomain != null ? d.MailDomain.ItemName : null,
						ZoneName = d.Zone != null ? d.Zone.ItemName : null,
						d.Domain.IsSubDomain,
						d.Domain.IsPreviewDomain,
						d.Domain.IsDomainPointer
					});

				return EntityDataSet(domains);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetDomainsByZoneID",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@ZoneID", zoneId));
			}
		}

		public DataSet GetDomainsByDomainItemId(int actorId, int domainId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetDomainsByDomainItemID]
(
	@ActorID int,
	@DomainID int
)
AS

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(D.WebSiteID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(D.MailDomainID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsPreviewDomain,
	D.IsDomainPointer
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
WHERE
	D.DomainItemID = @DomainID
	AND dbo.CheckActorPackageRights(@ActorID, P.PackageID) = 1
RETURN
				*/
				#endregion

				var domains = Domains
					.Where(d => d.DomainItemId == domainId && CheckActorPackageRights(actorId, d.PackageId))
					.Join(Packages, d => d.PackageId, p => p.PackageId, (d, p) => new
					{
						Domain = d,
						Package = p
					})
					.GroupJoin(ServiceItems, d => d.Domain.WebSiteId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						WebSite = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.MailDomainId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						MailDomain = s.SingleOrDefault()
					})
					.GroupJoin(ServiceItems, d => d.Domain.ZoneItemId, s => s.ItemId, (d, s) => new
					{
						d.Domain,
						d.Package,
						d.WebSite,
						d.MailDomain,
						Zone = s.SingleOrDefault()
					})
					.Select(d => new
					{
						d.Domain.DomainId,
						d.Domain.PackageId,
						d.Domain.ZoneItemId,
						d.Domain.DomainItemId,
						d.Domain.DomainName,
						d.Domain.HostingAllowed,
						WebSiteId = d.WebSite != null ? d.WebSite.ItemId : 0,
						WebSiteName = d.WebSite != null ? d.WebSite.ItemName : null,
						MailDomainId = d.MailDomain != null ? d.MailDomain.ItemId : 0,
						MailDomainName = d.MailDomain != null ? d.MailDomain.ItemName : null,
						ZoneName = d.Zone != null ? d.Zone.ItemName : null,
						d.Domain.IsSubDomain,
						d.Domain.IsPreviewDomain,
						d.Domain.IsDomainPointer
					});

				return EntityDataSet(domains);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetDomainsByDomainItemID",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@DomainID", domainId));
			}
		}



		public int CheckDomain(int packageId, string domainName, bool isDomainPointer)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[CheckDomain]
(
	@PackageID int,
	@DomainName nvarchar(100),
	@IsDomainPointer bit,
	@Result int OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
	-2 - sub-domain of prohibited domain
*/ /*

SET @Result = 0 -- OK

-- check if the domain already exists
IF EXISTS(
	SELECT DomainID FROM Domains
	WHERE DomainName = @DomainName AND IsDomainPointer = @IsDomainPointer
)
BEGIN
	SET @Result = -1
	RETURN
END

--check if this is a sub - domain of other domain
-- that is not allowed for 3rd level hosting

DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

-- find sub - domains
DECLARE @DomainUserID int, @HostingAllowed bit
SELECT
	@DomainUserID = P.UserID,
	@HostingAllowed = D.HostingAllowed
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
WHERE CHARINDEX('.' + DomainName, @DomainName) > 0
AND(CHARINDEX('.' + DomainName, @DomainName) + LEN('.' + DomainName)) = LEN(@DomainName) + 1
AND IsDomainPointer = 0

-- this is a domain of other user
IF @UserID<> @DomainUserID AND @HostingAllowed = 0
BEGIN
	SET @Result = -2
	RETURN
END

RETURN

				*/
				#endregion

				// check if the domain already exists
				if (Domains.Any(d => d.DomainName == domainName && d.IsDomainPointer == isDomainPointer)) return -1;

				// check if this is a sub-domain of other domain that is not allowed for 3rd level hosting
				var userId = Packages
					.Where(p => p.PackageId == packageId)
					.Select(p => p.UserId)
					.FirstOrDefault();

				// find sub-domains
				var subdomains = Domains
					.Where(d => domainName.IndexOf("." + d.DomainName) >= 0 &&
						domainName.IndexOf("." + d.DomainName) + d.DomainName.Length + 1 == domainName.Length + 1 &&
						!d.IsDomainPointer)
					.Join(Packages, d => d.PackageId, p => p.PackageId, (d, p) => new
					{
						p.UserId,
						d.HostingAllowed
					});

				if (subdomains.Any(d => d.UserId != userId && !d.HostingAllowed)) return -2;

				return 0;
			}
			else
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
		}



		public int CheckDomainUsedByHostedOrganization(string domainName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[CheckDomainUsedByHostedOrganization] 
	@DomainName nvarchar(100),
	@Result int OUTPUT
AS
	SET @Result = 0
	IF EXISTS(SELECT 1 FROM ExchangeAccounts WHERE UserPrincipalName LIKE '%@'+ @DomainName AND AccountType!=2)
	BEGIN
		SET @Result = 1
	END
	ELSE
	IF EXISTS(SELECT 1 FROM ExchangeAccountEmailAddresses WHERE EmailAddress LIKE '%@'+ @DomainName)
	BEGIN
		SET @Result = 1
	END
	ELSE
	IF EXISTS(SELECT 1 FROM LyncUsers WHERE SipAddress LIKE '%@'+ @DomainName)
	BEGIN
		SET @Result = 1
	END
	ELSE
	IF EXISTS(SELECT 1 FROM SfBUsers WHERE SipAddress LIKE '%@'+ @DomainName)
	BEGIN
		SET @Result = 1
	END

	RETURN @Result
				*/
				#endregion

				var mailDomain = $"@{domainName}";

				if (ExchangeAccounts.Any(a => a.UserPrincipalName.EndsWith(mailDomain)) ||
					ExchangeAccountEmailAddresses.Any(e => e.EmailAddress.EndsWith(mailDomain)) ||
					LyncUsers.Any(u => u.SipAddress.EndsWith(mailDomain)) ||
					SfBusers.Any(u => u.SipAddress.EndsWith(mailDomain))) return 1;
				else return 0;

			}
			else
			{
				SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
				prmId.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "CheckDomainUsedByHostedOrganization",
					prmId,
					new SqlParameter("@domainName", domainName));

				return Convert.ToInt32(prmId.Value);
			}
		}


		public int AddDomain(int actorId, int packageId, int zoneItemId, string domainName,
			 bool hostingAllowed, int webSiteId, int mailDomainId, bool isSubDomain, bool isPreviewDomain, bool isDomainPointer)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddDomain]
(
	@DomainID int OUTPUT,
	@ActorID int,
	@PackageID int,
	@ZoneItemID int,
	@DomainName nvarchar(200),
	@HostingAllowed bit,
	@WebSiteID int,
	@MailDomainID int,
	@IsSubDomain bit,
	@IsPreviewDomain bit,
	@IsDomainPointer bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

IF @ZoneItemID = 0 SET @ZoneItemID = NULL
IF @WebSiteID = 0 SET @WebSiteID = NULL
IF @MailDomainID = 0 SET @MailDomainID = NULL

-- insert record
INSERT INTO Domains
(
	PackageID,
	ZoneItemID,
	DomainName,
	HostingAllowed,
	WebSiteID,
	MailDomainID,
	IsSubDomain,
	IsPreviewDomain,
	IsDomainPointer
)
VALUES
(
	@PackageID,
	@ZoneItemID,
	@DomainName,
	@HostingAllowed,
	@WebSiteID,
	@MailDomainID,
	@IsSubDomain,
	@IsPreviewDomain,
	@IsDomainPointer
)

SET @DomainID = SCOPE_IDENTITY()
RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var domain = new Data.Entities.Domain()
				{
					PackageId = packageId,
					ZoneItemId = zoneItemId != 0 ? zoneItemId : null,
					DomainName = domainName,
					HostingAllowed = hostingAllowed,
					WebSiteId = webSiteId != 0 ? webSiteId : null,
					MailDomainId = mailDomainId != 0 ? mailDomainId : null,
					IsSubDomain = isSubDomain,
					IsPreviewDomain = isPreviewDomain,
					IsDomainPointer = isDomainPointer
				};

				Domains.Add(domain);

				SaveChanges();

				return domain.DomainId;
			}
			else
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
		}

		public void UpdateDomain(int actorId, int domainId, int zoneItemId,
			 bool hostingAllowed, int webSiteId, int mailDomainId, int domainItemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateDomain]
(
	@DomainID int,
	@ActorID int,
	@ZoneItemID int,
	@HostingAllowed bit,
	@WebSiteID int,
	@MailDomainID int,
	@DomainItemID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Domains
WHERE DomainID = @DomainID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

IF @ZoneItemID = 0 SET @ZoneItemID = NULL
IF @WebSiteID = 0 SET @WebSiteID = NULL
IF @MailDomainID = 0 SET @MailDomainID = NULL

-- update record
UPDATE Domains
SET
	ZoneItemID = @ZoneItemID,
	HostingAllowed = @HostingAllowed,
	WebSiteID = @WebSiteID,
	MailDomainID = @MailDomainID,
	DomainItemID = @DomainItemID
WHERE
	DomainID = @DomainID
	RETURN
				*/
				#endregion

				var domain = Domains.FirstOrDefault(d => d.DomainId == domainId);
				if (domain == null) return;

				if (!CheckActorPackageRights(actorId, domain.PackageId))
					throw new AccessViolationException("You are not allowed to access this package");

				domain.ZoneItemId = zoneItemId != 0 ? zoneItemId : null;
				domain.HostingAllowed = hostingAllowed;
				domain.WebSiteId = webSiteId != 0 ? webSiteId : null;
				domain.MailDomainId = mailDomainId != 0 ? mailDomainId : null;
				domain.DomainItemId = domainItemId;

				SaveChanges();
			}
			else
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
		}

		public void DeleteDomain(int actorId, int domainId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteDomain]
(
	@DomainID int,
	@ActorID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Domains
WHERE DomainID = @DomainID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)
DELETE FROM Domains
WHERE DomainID = @DomainID

RETURN
				*/
				#endregion

				var domain = Domains.FirstOrDefault(d => d.DomainId == domainId);
				if (domain == null) return;

				if (!CheckActorPackageRights(actorId, domain.PackageId))
					throw new AccessViolationException("You are not allowed to access this package");

				Domains.Remove(domain);

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteDomain",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@DomainId", domainId));
			}
		}
		#endregion

		#region Services
		public IDataReader GetServicesByServerId(int actorId, int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServicesByServerID]
(
	@ActorID int,
	@ServerID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServiceID,
	S.ServerID,
	S.ServiceName,
	S.Comments,
	S.ServiceQuotaValue,
	RG.GroupName,
	S.ProviderID,
	PROV.DisplayName AS ProviderName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	S.ServerID = @ServerID
	AND @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var services = Services
					.Where(s => isAdmin && s.ServerId == serverId)
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						Service = s,
						Provider = p
					})
					.Join(ResourceGroups, s => s.Provider.GroupId, r => r.GroupId, (s, r) => new
					{
						s.Service,
						s.Provider,
						ResourceGroup = r
					})
					.OrderBy(s => s.ResourceGroup.GroupOrder)
					.Select(s => new
					{
						s.Service.ServiceId,
						s.Service.ServerId,
						s.Service.ServiceName,
						s.Service.Comments,
						s.Service.ServiceQuotaValue,
						s.ResourceGroup.GroupName,
						s.Service.ProviderId,
						ProviderName = s.Provider.DisplayName
					});

				return EntityDataReader(services);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServicesByServerID",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ServerID", serverId));
			}
		}

		public IDataReader GetServicesByServerIdGroupName(int actorId, int serverId, string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServicesByServerIDGroupName]
(
	@ActorID int,
	@ServerID int,
	@GroupName nvarchar(50)
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServiceID,
	S.ServerID,
	S.ServiceName,
	S.Comments,
	S.ServiceQuotaValue,
	RG.GroupName,
	PROV.DisplayName AS ProviderName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	S.ServerID = @ServerID AND RG.GroupName = @GroupName
	AND @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var services = Services
					.Where(s => isAdmin && s.ServerId == serverId)
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						Service = s,
						Provider = p
					})
					.Join(ResourceGroups, s => s.Provider.GroupId, r => r.GroupId, (s, r) => new
					{
						s.Service.ServiceId,
						s.Service.ServerId,
						s.Service.ServiceName,
						s.Service.Comments,
						s.Service.ServiceQuotaValue,
						r.GroupName,
						ProviderName = s.Provider.DisplayName
					})
					.Where(s => s.GroupName == groupName);

				return EntityDataReader(services);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServicesByServerIDGroupName",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@ServerID", serverId),
					new SqlParameter("@GroupName", groupName));
			}
		}

		public DataSet GetRawServicesByServerId(int actorId, int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetRawServicesByServerID]
(
	@ActorID int,
	@ServerID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- resource groups
SELECT
	GroupID,
	GroupName
FROM ResourceGroups
WHERE @IsAdmin = 1 AND (ShowGroup = 1)
ORDER BY GroupOrder

-- services
SELECT
	S.ServiceID,
	S.ServerID,
	S.ServiceName,
	S.Comments,
	RG.GroupID,
	PROV.DisplayName AS ProviderName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	S.ServerID = @ServerID
	AND @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var groups = ResourceGroups
					.Where(g => isAdmin && g.ShowGroup == true)
					.OrderBy(g => g.GroupOrder)
					.Select(g => new { g.GroupId, g.GroupName });

				var services = Services
					.Where(s => isAdmin && s.ServerId == serverId)
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						Service = s,
						Provider = p
					})
					.Join(ResourceGroups, s => s.Provider.GroupId, r => r.GroupId, (s, r) => new
					{
						s.Service.ServiceId,
						s.Service.ServerId,
						s.Service.ServiceName,
						s.Service.Comments,
						r.GroupId,
						ProviderName = s.Provider.DisplayName
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(groups));
				dataSet.Tables.Add(EntityDataTable(services));

				return dataSet;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetRawServicesByServerID",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ServerID", serverId));
			}
		}

		public DataSet GetServicesByGroupId(int actorId, int groupId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServicesByGroupID]
(
	@ActorID int,
	@GroupID int
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServiceID,
	S.ServiceName,
	S.ServerID,
	S.ServiceQuotaValue,
	SRV.ServerName,
	S.ProviderID,
	S.ServiceName+' on '+SRV.ServerName AS FullServiceName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
WHERE
	PROV.GroupID = @GroupID
	AND @IsAdmin = 1
RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var services = Services
					.Where(s => isAdmin)
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						Service = s,
						Provider = p
					})
					.Where(s => s.Provider.GroupId == groupId)
					.Join(Servers, s.Service.ServerId, s => s.ServerId, (s, t) => new
					{
						s.Service.ServiceId,
						s.Service.ServiceName,
						s.Service.ServerId,
						s.Service.ServiceQuotaValue,
						t.ServerName,
						s.Service.ProviderId,
						FullServiceName = s.Service.ServiceName + " on " + t.ServerName
					});

				return EntityDataSet(services);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServicesByGroupID",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@groupId", groupId));
			}
		}

		public DataSet GetServicesByGroupName(int actorId, string groupName, bool forAutodiscover)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServicesByGroupName]
(
	@ActorID int,
	@GroupName nvarchar(100),
	@forAutodiscover bit
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServiceID,
	S.ServiceName,
	S.ServerID,
	S.ServiceQuotaValue,
	SRV.ServerName,
	S.ProviderID,
    PROV.ProviderName,
	S.ServiceName + ' on ' + SRV.ServerName AS FullServiceName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	RG.GroupName = @GroupName
	AND (@IsAdmin = 1 OR @forAutodiscover = 1)
RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				var services = Services
					.Where(s => isAdmin)
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						Service = s,
						Provider = p
					})
					.Join(Servers, s => s.Service.ServerId, t => t.ServerId, (s, t) => new
					{
						s.Service,
						s.Provider,
						Server = t
					})
					.Join(ResourceGroups, s => s.Provider.GroupId, r => r.GroupId, (s, r) => new
					{
						s.Service,
						s.Provider,
						s.Server,
						ResourceGroup = r
					})
					.Where(s => s.ResourceGroup.GroupName == groupName)
					.Select(s => new
					{
						s.Service.ServiceId,
						s.Service.ServiceName,
						s.Service.ServerId,
						s.Service.ServiceQuotaValue,
						s.Server.ServerName,
						s.Service.ProviderId,
						s.Provider.ProviderName,
						FullServiceName = s.Service.ServiceName + " on " + t.ServerName
					});

				return EntityDataSet(services);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServicesByGroupName",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@GroupName", groupName),
					new SqlParameter("@forAutodiscover", forAutodiscover));
			}
		}

		public IDataReader GetService(int actorId, int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetService]
(
	@ActorID int,
	@ServiceID int
)
AS

SELECT
	ServiceID,
	Services.ServerID,
	ProviderID,
	ServiceName,
	ServiceQuotaValue,
	ClusterID,
	Services.Comments,
	Servers.ServerName
FROM Services INNER JOIN Servers ON Services.ServerID = Servers.ServerID
WHERE
	ServiceID = @ServiceID

RETURN
				*/
				#endregion

				var services = Services
					.Where(s => s.ServiceId == serviceId)
					.Join(Servers, s => s.ServerId, t => t.ServerId, (s, t) => new
					{
						s.ServiceId,
						s.ServerId,
						s.ProviderId,
						s.ServiceName,
						s.ServiceQuotaValue,
						s.ClusterId,
						s.Comments,
						t.ServerName
					});

				return EntityDataReader(services);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
					CommandType.StoredProcedure,
					ObjectQualifier + "GetService",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ServiceID", serviceId));
			}
		}

		public int AddService(int serverId, int providerId, string serviceName, int serviceQuotaValue,
			 int clusterId, string comments)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddService]
(
	@ServiceID int OUTPUT,
	@ServerID int,
	@ProviderID int,
	@ServiceQuotaValue int,
	@ServiceName nvarchar(50),
	@ClusterID int,
	@Comments ntext
)
AS
BEGIN

BEGIN TRAN
IF @ClusterID = 0 SET @ClusterID = NULL

INSERT INTO Services
(
	ServerID,
	ProviderID,
	ServiceName,
	ServiceQuotaValue,
	ClusterID,
	Comments
)
VALUES
(
	@ServerID,
	@ProviderID,
	@ServiceName,
	@ServiceQuotaValue,
	@ClusterID,
	@Comments
)

SET @ServiceID = SCOPE_IDENTITY()

-- copy default service settings
INSERT INTO ServiceProperties (ServiceID, PropertyName, PropertyValue)
SELECT @ServiceID, PropertyName, PropertyValue
FROM ServiceDefaultProperties
WHERE ProviderID = @ProviderID

-- copy all default DNS records for the given service
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM Providers
WHERE ProviderID = @ProviderID

-- default IP address for added records
DECLARE @AddressID int
SELECT TOP 1 @AddressID = AddressID FROM IPAddresses
WHERE ServerID = @ServerID

INSERT INTO GlobalDnsRecords
(
	RecordType,
	RecordName,
	RecordData,
	MXPriority,
	IPAddressID,
	ServiceID,
	ServerID,
	PackageID
)
SELECT
	RecordType,
	RecordName,
	CASE WHEN RecordData = '[ip]' THEN ''
	ELSE RecordData END,
	MXPriority,
	CASE WHEN RecordData = '[ip]' THEN @AddressID
	ELSE NULL END,
	@ServiceID,
	NULL, -- server
	NULL -- package
FROM
	ResourceGroupDnsRecords
WHERE GroupID = @GroupID
ORDER BY RecordOrder
COMMIT TRAN

END
RETURN
				*/
				#endregion

				int serviceId = 0;

				using (var transaction = Database.BeginTransaction())
				{
					var service = new Data.Entities.Service()
					{
						ServerId = serverId,
						ProviderId = providerId,
						ServiceName = serviceName,
						ServiceQuotaValue = serviceQuotaValue,
						ClusterId = clusterId != 0 ? clusterId : null,
						Comments = comments
					};
					Services.Add(service);
					SaveChanges();

					serviceId = service.ServiceId;

					// copy default service settings
					var properties = ServiceDefaultProperties
						.Where(p => p.ProviderId == providerId)
						.Select(p => new Data.Entities.ServiceProperty()
						{
							ServiceId = serviceId,
							PropertyName = p.PropertyName,
							PropertyValue = p.PropertyValue
						});
					ServiceProperties.AddRange(properties);

					// copy all default DNS records for the given service
					var groupId = Providers
						.Where(p => p.ProviderId == providerId)
						.Select(p => p.GroupId)
						.FirstOrDefault();

					// default IP address for added records
					var addressId = IpAddresses
						.Where(ip => ip.ServerId == serverId)
						.Select(ip => ip.AddressId)
						.FirstOrDefault();

					var dnsRecords = ResourceGroupDnsRecords
						.Where(r => r.GroupId == groupId)
						.OrderBy(r => r.RecordOrder)
						.Select(r => new Data.Entities.GlobalDnsRecord()
						{
							RecordType = r.RecordType,
							RecordName = r.RecordName,
							RecordData = r.RecordData == "[ip]" ? "" : r.RecordData,
							MXPriority = r.MXPriority ?? 0,
							IpAddressId = r.RecordData == "[ip]" ? addressId : null,
							ServiceId = serviceId,
							ServerId = null,
							PackageId = null
						});
					GlobalDnsRecords.AddRange(dnsRecords);
					SaveChanges();

					transaction.Commit();
				}

				return serviceId;
			}
			else
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
		}

		public void UpdateServiceFully(int serviceId, int providerId, string serviceName, int serviceQuotaValue,
			 int clusterId, string comments)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateServiceFully]
(
	@ServiceID int,
	@ProviderID int,
	@ServiceName nvarchar(50),
	@Comments ntext,
	@ServiceQuotaValue int,
	@ClusterID int
)
AS

IF @ClusterID = 0 SET @ClusterID = NULL

UPDATE Services
SET
	ProviderID = @ProviderID,
	ServiceName = @ServiceName,
	ServiceQuotaValue = @ServiceQuotaValue,
	Comments = @Comments,
	ClusterID = @ClusterID
WHERE ServiceID = @ServiceID

RETURN
				*/
				#endregion

				var service = Services
					.FirstOrDefault(s => s.ServiceId == serviceId);

				if (service != null)
				{
					service.ProviderId = providerId;
					service.ServiceName = serviceName;
					service.ServiceQuotaValue = serviceQuotaValue;
					service.Comments = comments;
					service.ClusterId = clusterId != 0 ? clusterId : null;

					SaveChanges();
				}
			}
			else
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
		}

		public void UpdateService(int serviceId, string serviceName, int serviceQuotaValue,
			 int clusterId, string comments)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateService]
(
	@ServiceID int,
	@ServiceName nvarchar(50),
	@Comments ntext,
	@ServiceQuotaValue int,
	@ClusterID int
)
AS

IF @ClusterID = 0 SET @ClusterID = NULL

UPDATE Services
SET
	ServiceName = @ServiceName,
	ServiceQuotaValue = @ServiceQuotaValue,
	Comments = @Comments,
	ClusterID = @ClusterID
WHERE ServiceID = @ServiceID

RETURN
				*/
				#endregion

				var service = Services
					.FirstOrDefault(s => s.ServiceId == serviceId);

				if (service != null)
				{
					service.ServiceName = serviceName;
					service.ServiceQuotaValue = serviceQuotaValue;
					service.Comments = comments;
					service.ClusterId = clusterId != 0 ? clusterId : null;

					SaveChanges();
				}
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdateService",
					new SqlParameter("@ServiceName", serviceName),
					new SqlParameter("@ServiceID", serviceId),
					new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
					new SqlParameter("@ClusterId", clusterId),
					new SqlParameter("@Comments", comments));
			}
		}

		public int DeleteService(int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteService]
(
	@ServiceID int,
	@Result int OUTPUT
)
AS

SET @Result = 0

-- check related service items
IF EXISTS (SELECT ItemID FROM ServiceItems WHERE ServiceID = @ServiceID)
BEGIN
	SET @Result = -1
	RETURN
END

IF EXISTS (SELECT ServiceID FROM VirtualServices WHERE ServiceID = @ServiceID)
BEGIN
	SET @Result = -2
	RETURN
END

BEGIN TRAN
-- delete global DNS records
DELETE FROM GlobalDnsRecords
WHERE ServiceID = @ServiceID

-- delete service
DELETE FROM Services
WHERE ServiceID = @ServiceID

COMMIT TRAN

RETURN
				*/
				#endregion

				// check related service items
				if (ServiceItems.Any(s => s.ServiceId == serviceId)) return -1;
				if (VirtualServices.Any(s => s.ServiceId == serviceId)) return -2;

				using (var transaction = Database.BeginTransaction())
				{
					GlobalDnsRecords.Where(r => r.ServiceId == serviceId).ExecuteDelete(GlobalDnsRecords);
					Services.Where(s => s.ServiceId == serviceId).ExecuteDelete(Services);

					transaction.Commit();
					return 0;
				}
			}
			else
			{
				SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
				prmResult.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteService",
					prmResult,
					new SqlParameter("@ServiceID", serviceId));

				return Convert.ToInt32(prmResult.Value);
			}
		}

		public IDataReader GetServiceProperties(int actorId, int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceProperties]
(
	@ActorID int,
	@ServiceID int
)
AS

SELECT ServiceID, PropertyName, PropertyValue
FROM ServiceProperties
WHERE
	ServiceID = @ServiceID

RETURN
				*/
				#endregion

				var properties = ServiceProperties
					.Where(s => s.ServiceId == serviceId);

				return EntityDataReader(properties);
			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
					CommandType.StoredProcedure,
					ObjectQualifier + "GetServiceProperties",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ServiceID", serviceId));
			}
		}

		public void UpdateServiceProperties(int serviceId, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateServiceProperties]
(
	@ServiceID int,
	@Xml ntext
)
AS

-- delete old properties
BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ServiceProperties
WHERE ServiceID = @ServiceID 
AND PropertyName IN
(
	SELECT PropertyName
	FROM OPENXML(@idoc, '/properties/property', 1)
	WITH (PropertyName nvarchar(50) '@name')
)

INSERT INTO ServiceProperties
(
	ServiceID,
	PropertyName,
	PropertyValue
)
SELECT
	@ServiceID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(MAX) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN 
				*/
				#endregion

				var properties = XElement.Parse(xml)
					.Elements()
					.Select(e => new
					{
						Name = (string)e.Attribute("name"),
						Value = (string)e.Attribute("value")
					})
					.ToArray();

				// delete old properties
				var serviceProperties = ServiceProperties
					.Where(s => s.ServiceId == serviceId && properties.Any(p => p.Name == s.PropertyName));
				ServiceProperties.RemoveRange(serviceProperties);

				ServiceProperties.AddRange(properties
					.Select(p => new Data.Entities.ServiceProperty()
					{
						ServiceId = serviceId,
						PropertyName = p.Name,
						PropertyValue = p.Value
					}));

				SaveChanges();
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdateServiceProperties",
					new SqlParameter("@ServiceId", serviceId),
					new SqlParameter("@Xml", xml));
			}
		}

		public IDataReader GetResourceGroup(int groupId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetResourceGroup]
(
	@GroupID int
)
AS
SELECT
	RG.GroupID,
	RG.GroupOrder,
	RG.GroupName,
	RG.GroupController
FROM ResourceGroups AS RG
WHERE RG.GroupID = @GroupID

RETURN
				*/
				#endregion

				var groups = ResourceGroups
					.Where(g => g.GroupId == groupId)
					.Select(g => new
					{
						g.GroupId,
						g.GroupOrder,
						g.GroupName,
						g.GroupController
					});

				return EntityDataReader(groups);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString,
					CommandType.StoredProcedure,
					ObjectQualifier + "GetResourceGroup",
					new SqlParameter("@groupId", groupId));
			}
		}

		public DataSet GetResourceGroups()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetResourceGroups]
AS
SELECT
	GroupID,
	GroupName,
	GroupController
FROM ResourceGroups
ORDER BY GroupOrder
RETURN
				*/
				#endregion

				var groups = ResourceGroups
					.OrderBy(g => g.GroupOrder)
					.Select(g => new
					{
						g.GroupId,
						g.GroupName,
						g.GroupController
					});

				return EntityDataSet(groups);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString,
					CommandType.StoredProcedure,
					ObjectQualifier + "GetResourceGroups");
			}
		}

		public IDataReader GetResourceGroupByName(string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetResourceGroupByName]
(
	@GroupName nvarchar(100)
)
AS
SELECT
	RG.GroupID,
	RG.GroupOrder,
	RG.GroupName,
	RG.GroupController
FROM ResourceGroups AS RG
WHERE RG.GroupName = @GroupName
				*/
				#endregion

				var group = ResourceGroups
					.Where(g => g.GroupName == groupName)
					.Select(g => new
					{
						g.GroupId,
						g.GroupOrder,
						g.GroupName,
						g.GroupController
					});
				return EntityDataReader(group);
			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString,
					CommandType.StoredProcedure,
					ObjectQualifier + "GetResourceGroupByName",
					new SqlParameter("@groupName", groupName));
			}
		}

		#endregion

		#region Service Items
		public DataSet GetServiceItems(int actorId, int packageId, string groupName, string itemTypeName, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItems]
(
	@ActorID int,
	@PackageID int,
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL,
	@Recursive bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON SI.PackageID = PT.PackageID
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON IT.GroupID = RG.GroupID
WHERE IT.TypeName = @ItemTypeName
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	(U.FirstName + U.LastName) AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN Users AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN
				*/
				#endregion

				// check rights
				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var items = ServiceItems
					.Join(PackagesTree(packageId, recursive), s => s.PackageId, p => p, (s, p) => s)
					.Join(ServiceItemTypes, s => s.ItemTypeId, t => t.ItemTypeId, (s, t) => new
					{
						Item = s,
						Type = t
					})
					.Join(ResourceGroups, s => s.Type.GroupId, r => r.GroupId, (s, r) => new
					{
						s.Item,
						s.Type,
						ResourceGroup = r
					})
					.Where(s => s.Type.TypeName == itemTypeName &&
						(groupName == null || groupName != null && groupName == s.ResourceGroup.GroupName))
					.Select(s => s.Item.ItemId)
					.ToArray();

				// select service items
				var serviceItems = items
					.Join(ServiceItems, i => i, s => s.ItemId, (i, s) => s)
					.Join(ServiceItemTypes, s => s.ItemTypeId, t => t.ItemTypeId, (s, t) => new
					{
						Item = s,
						Type = t
					})
					.Join(Packages, s => s.Item.PackageId, p => p.PackageId, (s, p) => new
					{
						s.Item,
						s.Type,
						Package = p
					})
					.Join(Services, s => s.Item.ServiceId, s => s.ServiceId, (s, t) => new
					{
						s.Item,
						s.Type,
						s.Package,
						Service = t
					})
					.Join(Servers, s => s.Service.ServerId, t => t.ServerId, (s, t) => new
					{
						s.Item,
						s.Type,
						s.Package,
						s.Service,
						Server = t
					})
					.Join(ResourceGroups, s => s.Type.GroupId, r => r.GroupId, (s, r) => new
					{
						s.Item,
						s.Type,
						s.Package,
						s.Service,
						s.Server,
						ResourceGroup = r
					})
					.Join(Users, s => s.Package.UserId, u => u.UserId, (s, u) => new
					{
						s.Item.ItemId,
						s.Item.ItemName,
						s.Item.ItemTypeId,
						s.Type.TypeName,
						s.Item.ServiceId,
						s.Item.PackageId,
						s.Package.PackageName,
						s.Service.ServiceName,
						s.Server.ServerId,
						s.Server.ServerName,
						s.ResourceGroup.GroupName,
						u.UserId,
						u.Username,
						UserFullName = u.FirstName + " " + u.LastName,
						s.Item.CreatedDate
					});

				// select item properties
				// get corresponding item properties
				var itemProperties = ServiceItemProperties
					.Join(items, p => p.ItemId, i => i, (p, i) => new
					{
						p.ItemId,
						p.PropertyName,
						p.PropertyValue
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(serviceItems));
				dataSet.Tables.Add(EntityDataTable(itemProperties));
				return dataSet;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServiceItems",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageID", packageId),
					new SqlParameter("@GroupName", groupName),
					new SqlParameter("@ItemTypeName", itemTypeName),
					new SqlParameter("@Recursive", recursive));
			}
		}

		public DataSet GetServiceItemsPaged(int actorId, int packageId, string groupName, string itemTypeName,
			int serverId, bool recursive, string filterColumn, string filterValue,
			string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemsPaged]
(
	@ActorID int,
	@PackageID int,
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL,
	@ServerID int,
	@Recursive bit,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM ResourceGroups
WHERE GroupName = @GroupName

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName
AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND GroupID = @GroupID))

DECLARE @condition nvarchar(700)
SET @condition = 'SI.ItemTypeID = @ItemTypeID
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
		OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND IT.GroupID = @GroupID))
AND (@ServerID = 0 OR (@ServerID > 0 AND S.ServerID = @ServerID))
'

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (ItemName LIKE ''' + @FilterValue + '''
			OR Username ''' + @FilterValue + '''
			OR FullName ''' + @FilterValue + '''
			OR Email ''' + @FilterValue + ''')'
END

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'SI.ItemName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(SI.ItemID) FROM Packages AS P
INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
WHERE ' + @condition + '

DECLARE @Items AS TABLE
(
	ItemID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		SI.ItemID
	FROM Packages AS P
	INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
	INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
	INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
	WHERE ' + @condition + '
)

INSERT INTO @Items
SELECT ItemID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	IT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	SI.CreatedDate,
	RG.GroupName,

	-- packages
	P.PackageName,

	-- server
	ISNULL(SRV.ServerID, 0) AS ServerID,
	ISNULL(SRV.ServerName, '''') AS ServerName,
	ISNULL(SRV.Comments, '''') AS ServerComments,
	ISNULL(SRV.VirtualServer, 0) AS VirtualServer,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Items AS TSI
INNER JOIN ServiceItems AS SI ON TSI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON IT.GroupID = RG.GroupID

SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS TSI ON IP.ItemID = TSI.ItemID'

--print @sql

exec sp_executesql @sql, N'@ItemTypeID int, @PackageID int, @GroupID int, @StartRow int, @MaximumRows int, @Recursive bit, @ServerID int',
@ItemTypeID, @PackageID, @GroupID, @StartRow, @MaximumRows, @Recursive, @ServerID

RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var groupId = ResourceGroups
					.Where(r => r.GroupName == groupName)
					.Select(r => (int?)r.GroupId)
					.FirstOrDefault();

				var itemTypeId = ServiceItemTypes
					.Where(t => t.TypeName == itemTypeName &&
						(groupId == null || groupId != null && t.GroupId == groupId))
					.Select(t => t.ItemTypeId)
					.FirstOrDefault();

				var items = ServiceItems
					.Where(s => s.ItemTypeId == itemTypeId &&
						(!recursive && s.PackageId == packageId ||
						recursive && CheckPackageParent(packageId, s.PackageId ?? 0)))
					.Join(Packages, i => i.PackageId, p => p.PackageId, (si, p) => new
					{
						Item = si,
						Package = p
					})
					.Join(ServiceItemTypes, i => i.Item.ItemTypeId, t => t.ItemTypeId, (i, t) => new
					{
						i.Item,
						i.Package,
						Type = t
					})
					.Where(i => groupId == null || groupId != null && i.Type.GroupId == groupId)
					.Join(UsersDetailed, i => i.Package.UserId, u => u.UserId, (i, u) => new
					{
						i.Item,
						i.Package,
						i.Type,
						User = u
					})
					.Join(Services, i => i.Item.ServiceId, s => s.ServiceId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						Service = s
					})
					.Where(i => serverId == 0 || serverId > 0 && i.Service.ServerId == serverId)
					.Join(Servers, i => i.Service.ServerId, s => s.ServerId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						i.Service,
						Server = s
					})
					.Join(ResourceGroups, i => t.Type.GroupId, r => r.GroupId, (i, r) => new
					{
						i.Item.ItemId,
						i.Item.ItemName,
						i.Item.ItemTypeId,
						i.Type.TypeName,
						i.Item.ServiceId,
						i.Item.PackageId,
						i.Item.CreatedDate,
						r.GroupName,
						i.Package.PackageName,
						i.Server.ServerId,
						i.Server.ServerName,
						ServerComments = i.Server.Comments,
						i.Server.VirtualServer,
						i.Package.UserId,
						i.User.Username,
						i.User.FirstName,
						i.User.LastName,
						i.User.FullName,
						i.User.RoleId,
						i.User.Email
					});

				if (!string.IsNullOrEmpty(filterValue))
				{
					if (!string.IsNullOrEmpty(filterColumn))
					{
						items = items.Where($"{filterColumn}=@0", filterValue);
					}
					else
					{
						items = items
							.Where(i => i.Item.ItemName == filterValue ||
								i.User.Username == filterValue ||
								i.User.FullName == filterValue ||
								i.User.Email == filterValue);
					}
				}

				if (!string.IsNullOrEmpty(sortColumn)) items = items.OrderBy(sortColumn);
				else items = items.OrderBy(i => i.Item.ItemName);

				items = items.Skip(startRow).Take(maximumRows);

				var properties = ServiceItemProperties
					.Join(items, s => s.ItemId, i => i.ItemId, (s, i) => new
					{
						s.ItemId,
						s.PropertyName,
						s.PropertyValue
					});

				// TODO return also count of items?
				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(items));
				dataSet.Tables.Add(EntityDataTable(properties));
				return dataSet;
			}
			else
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
		}

		public DataSet GetSearchableServiceItemTypes()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetSearchableServiceItemTypes]

AS
SELECT
	ItemTypeID,
	DisplayName
FROM
	ServiceItemTypes
WHERE Searchable = 1
ORDER BY TypeOrder
RETURN
				*/
				#endregion

				var types = ServiceItemTypes
					.Where(t => t.Searchable == true)
					.OrderBy(t => t.TypeOrder)
					.Select(t => new
					{
						t.ItemTypeId,
						t.DisplayName
					});

				return EntityDataSet(types);
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetSearchableServiceItemTypes");
			}
		}

		public DataSet GetServiceItemsByService(int actorId, int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemsByService]
(
	@ActorID int,
	@ServiceID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
WHERE SI.ServiceID = @ServiceID

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	(U.FirstName + U.LastName) AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN Users AS U ON P.UserID = U.UserID
WHERE @IsAdmin = 1

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID
WHERE @IsAdmin = 1

RETURN
				*/
				#endregion

				var isAdmin = CheckIsUserAdmin(actorId);

				// select service items
				var items = ServiceItems
					.Where(s => isAdmin && s.ServiceId == serviceId)
					.Join(Packages, i => i.PackageId, p => p.PackageId, (si, p) => new
					{
						Item = si,
						Package = p
					})
					.Join(ServiceItemTypes, i => i.Item.ItemTypeId, t => t.ItemTypeId, (i, t) => new
					{
						i.Item,
						i.Package,
						Type = t
					})
					.Join(UsersDetailed, i => i.Package.UserId, u => u.UserId, (i, u) => new
					{
						i.Item,
						i.Package,
						i.Type,
						User = u
					})
					.Join(Services, i => i.Item.ServiceId, s => s.ServiceId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						Service = s
					})
					.Join(Servers, i => i.Service.ServerId, s => s.ServerId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						i.Service,
						Server = s
					})
					.Join(ResourceGroups, i => t.Type.GroupId, r => r.GroupId, (i, r) => new
					{
						i.Item.ItemId,
						i.Item.ItemName,
						i.Item.ItemTypeId,
						i.Type.TypeName,
						i.Item.ServiceId,
						i.Item.PackageId,
						i.Package.PackageName,
						i.Service.ServiceName,
						i.Server.ServerId,
						i.Server.ServerName,
						r.GroupName,
						i.User.UserId,
						i.User.Username,
						UserFullName = i.User.FirstName + " " + i.User.LastName,
						i.Item.CreatedDate
					});

				// select item properties, get corresponding item properties
				var properties = ServiceItemProperties
					.Join(items, p => p.ItemId, i => i.ItemId, (p, i) => new
					{
						p.ItemId,
						p.PropertyName,
						p.PropertyValue
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(items));
				dataSet.Tables.Add(EntityDataTable(properties));
				return dataSet;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServiceItemsByService",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ServiceID", serviceId));
			}
		}

		public int GetServiceItemsCount(string typeName, string groupName, int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemsCount]
(
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL,
	@ServiceID int = 0,
	@TotalNumber int OUTPUT
)
AS

SET @TotalNumber = 0

-- find service items
SELECT
	@TotalNumber = COUNT(SI.ItemID)
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON IT.GroupID = RG.GroupID
WHERE IT.TypeName = @ItemTypeName
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))
AND ((@ServiceID = 0) OR (@ServiceID > 0 AND SI.ServiceID = @ServiceID))

RETURN
				*/
				#endregion

				return ServiceItems
					.Where(s => serviceId == 0 || serviceId > 0 && s.ServiceId == serviceId)
					.Join(ServiceItemTypes, s => s.ItemTypeId, t => t.ItemTypeId, (s, t) => new
					{
						Item = s,
						Type = t
					})
					.Where(s => s.Type.TypeName == typeName)
					.Join(ResourceGroups, s => s.Type.GroupId, r => r.GroupId, (s, r) => r)
					.Where(r => groupName == null || groupName != null && r.GroupName == groupName)
					.Count();
			}
			else
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
		}

		public DataSet GetServiceItemsForStatistics(int actorId, int serviceId, int packageId,
			bool calculateDiskspace, bool calculateBandwidth, bool suspendable, bool disposable)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemsForStatistics]
(
	@ActorID int,
	@ServiceID int,
	@PackageID int,
	@CalculateDiskspace bit,
	@CalculateBandwidth bit,
	@Suspendable bit,
	@Disposable bit
)
AS
DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
WHERE
	((@ServiceID = 0) OR (@ServiceID > 0 AND SI.ServiceID = @ServiceID))
	AND ((@PackageID = 0) OR (@PackageID > 0 AND SI.PackageID = @PackageID))
	AND ((@CalculateDiskspace = 0) OR (@CalculateDiskspace = 1 AND SIT.CalculateDiskspace = @CalculateDiskspace))
	AND ((@CalculateBandwidth = 0) OR (@CalculateBandwidth = 1 AND SIT.CalculateBandwidth = @CalculateBandwidth))
	AND ((@Suspendable = 0) OR (@Suspendable = 1 AND SIT.Suspendable = @Suspendable))
	AND ((@Disposable = 0) OR (@Disposable = 1 AND SIT.Disposable = @Disposable))

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	RG.GroupName,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
ORDER BY RG.GroupOrder DESC, SI.ItemName

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN
				*/
				#endregion

				var items = ServiceItems
					.Where(s => (serviceId == 0 || serviceId > 0 && s.ServiceId == serviceId) &&
						(packageId == 0 || packageId != 0 && s.PackageId == packageId))
					.Join(ServiceItemTypes, s => s.ItemTypeId, t => t.ItemTypeId, (s, t) => new
					{
						Item = s,
						Type = t
					})
					// TODO is this correct?
					.Where(s => (!calculateDiskspace || s.Type.CalculateDiskspace == true) &&
						(!calculateBandwidth || s.Type.CalculateBandwidth == true) &&
						(!suspendable || s.Type.Suspendable == true) &&
						(!disposable || s.Type.Disposable == true));

				var serviceItems = items
					.Join(ResourceGroups, s => s.Type.GroupId, r => r.GroupId, (s, r) => new
					{
						s.Item,
						s.Type,
						ResourceGroup = r
					})
					.OrderBy(s => s.ResourceGroup.GroupOrder)
					.Select(s => new
					{
						s.Item.ItemId,
						s.Item.ItemName,
						s.Item.ItemTypeId,
						s.ResourceGroup.GroupName,
						s.Type.TypeName,
						s.Item.ServiceId,
						s.Item.PackageId,
						s.Item.CreatedDate
					});

				var properties = ServiceItemProperties
					.Join(items, p => p.ItemId, i => i.Item.ItemId, (p, i) => new
					{
						p.ItemId,
						p.PropertyName,
						p.PropertyValue
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(serviceItems));
				dataSet.Tables.Add(EntityDataTable(properties));
				return dataSet;
			}
			else
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
		}

		public DataSet GetServiceItemsByPackage(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemsByPackage]
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
WHERE SI.PackageID = @PackageID

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SIT.DisplayName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	(U.FirstName + U.LastName) AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN Users AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				// select service items
				var items = ServiceItems
					.Where(s => s.PackageId == packageId)
					.Join(Packages, i => i.PackageId, p => p.PackageId, (si, p) => new
					{
						Item = si,
						Package = p
					})
					.Join(ServiceItemTypes, i => i.Item.ItemTypeId, t => t.ItemTypeId, (i, t) => new
					{
						i.Item,
						i.Package,
						Type = t
					})
					.Join(Users, i => i.Package.UserId, u => u.UserId, (i, u) => new
					{
						i.Item,
						i.Package,
						i.Type,
						User = u
					})
					.Join(Services, i => i.Item.ServiceId, s => s.ServiceId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						Service = s
					})
					.Join(Servers, i => i.Service.ServerId, s => s.ServerId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						i.Service,
						Server = s
					})
					.Join(ResourceGroups, i => t.Type.GroupId, r => r.GroupId, (i, r) => new
					{
						i.Item.ItemId,
						i.Item.ItemName,
						i.Item.ItemTypeId,
						i.Type.DisplayName,
						i.Item.ServiceId,
						i.Item.PackageId,
						i.Package.PackageName,
						i.Service.ServiceName,
						i.Server.ServerId,
						i.Server.ServerName,
						r.GroupName,
						i.User.UserId,
						i.User.Username,
						UserFullName = i.User.FirstName + " " + i.User.LastName,
						i.Item.CreatedDate
					});

				// select item properties, get corresponding item properties
				var properties = ServiceItemProperties
					.Join(items, p => p.ItemId, i => i.ItemId, (p, i) => new
					{
						p.ItemId,
						p.PropertyName,
						p.PropertyValue
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(items));
				dataSet.Tables.Add(EntityDataTable(properties));
				return dataSet;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServiceItemsByPackage",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		public DataSet GetServiceItem(int actorId, int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItem]
(
	@ActorID int,
	@ItemID int
)
AS

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
WHERE
	SI.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	U.FullName AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN
				*/
				#endregion

				// select service items
				var items = ServiceItems
					.Where(s => s.ItemId == itemId && CheckActorPackageRights(actorId, s.PackageId))
					.Join(Packages, i => i.PackageId, p => p.PackageId, (si, p) => new
					{
						Item = si,
						Package = p
					})
					.Join(ServiceItemTypes, i => i.Item.ItemTypeId, t => t.ItemTypeId, (i, t) => new
					{
						i.Item,
						i.Package,
						Type = t
					})
					.Join(Users, i => i.Package.UserId, u => u.UserId, (i, u) => new
					{
						i.Item,
						i.Package,
						i.Type,
						User = u
					})
					.Join(Services, i => i.Item.ServiceId, s => s.ServiceId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						Service = s
					})
					.Join(Servers, i => i.Service.ServerId, s => s.ServerId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						i.Service,
						Server = s
					})
					.Join(ResourceGroups, i => t.Type.GroupId, r => r.GroupId, (i, r) => new
					{
						i.Item.ItemId,
						i.Item.ItemName,
						i.Item.ItemTypeId,
						i.Type.TypeName,
						i.Item.ServiceId,
						i.Item.PackageId,
						i.Package.PackageName,
						i.Service.ServiceName,
						i.Server.ServerId,
						i.Server.ServerName,
						r.GroupName,
						i.User.UserId,
						i.User.Username,
						UserFullName = i.User.FirstName + " " + i.User.LastName,
						i.Item.CreatedDate
					});

				// select item properties, get corresponding item properties
				var properties = ServiceItemProperties
					.Join(items, p => p.ItemId, i => i.ItemId, (p, i) => new
					{
						p.ItemId,
						p.PropertyName,
						p.PropertyValue
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(items));
				dataSet.Tables.Add(EntityDataTable(properties));
				return dataSet;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServiceItem",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@actorId", actorId));
			}
		}

		public bool CheckServiceItemExists(int serviceId, string itemName, string itemTypeName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[CheckServiceItemExistsInService]
(
	@Exists bit OUTPUT,
	@ServiceID int,
	@ItemName nvarchar(500),
	@ItemTypeName nvarchar(200)
)
AS

SET @Exists = 0

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName

IF EXISTS (SELECT ItemID FROM ServiceItems
WHERE ItemName = @ItemName AND ItemTypeID = @ItemTypeID AND ServiceID = @ServiceID)
SET @Exists = 1

RETURN
				*/
				#endregion

				var itemTypeId = ServiceItemTypes
					.Where(t => t.TypeName == itemTypeName)
					.Select(t => t.ItemTypeId)
					.FirstOrDefault();

				return ServiceItems.Any(s => s.ServiceId == serviceId && s.ItemName == itemName &&
					s.ItemTypeId == itemTypeId);
			}
			else
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
		}

		public bool CheckServiceItemExists(string itemName, string groupName, string itemTypeName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[CheckServiceItemExists]
(
	@Exists bit OUTPUT,
	@ItemName nvarchar(500),
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL
)
AS

SET @Exists = 0

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName

IF EXISTS (
SELECT ItemID FROM ServiceItems AS SI
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE SI.ItemName = @ItemName AND SI.ItemTypeID = @ItemTypeID
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))
)
SET @Exists = 1

RETURN
				*/
				#endregion

				var itemTypeId = ServiceItemTypes
					.Where(t => t.TypeName == itemTypeName)
					.Select(t => t.ItemTypeId)
					.FirstOrDefault();

				return ServiceItems
					.Where(s => s.ItemName == itemName && s.ItemTypeId == itemTypeId)
					.Join(Services, i => i.ServiceId, s => s.ServiceId, (i, s) => new
					{
						Item = i,
						Service = s,
					})
					.Join(Providers, i => i.Service.ProviderId, p => p.ProviderId, (i, p) => new
					{
						i.Item,
						i.Service,
						Provider = p
					})
					.Join(ResourceGroups, i => i.Provider.GroupId, r => r.GroupId, (i, r) => new
					{
						i.Item,
						i.Service,
						i.Provider,
						ResourceGroup = r
					})
					.Where(s => groupName == null || groupName != null && s.ResourceGroup.GroupName == groupName)
					.Any();
			}
			else
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
		}

		public DataSet GetServiceItemByName(int actorId, int packageId, string groupName,
			string itemName, string itemTypeName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemByName]
(
	@ActorID int,
	@PackageID int,
	@ItemName nvarchar(500),
	@GroupName nvarchar(100) = NULL,
	@ItemTypeName nvarchar(200)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
WHERE SI.PackageID = @PackageID AND SIT.TypeName = @ItemTypeName
AND SI.ItemName = @ItemName
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	U.FullName AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				// select service items
				var items = ServiceItems
					.Where(s => s.PackageId == packageId && s.ItemName == itemName)
					.Join(Packages, i => i.PackageId, p => p.PackageId, (si, p) => new
					{
						Item = si,
						Package = p
					})
					.Join(ServiceItemTypes, i => i.Item.ItemTypeId, t => t.ItemTypeId, (i, t) => new
					{
						i.Item,
						i.Package,
						Type = t
					})
					.Where(s => s.Type.TypeName == itemTypeName)
					.Join(Users, i => i.Package.UserId, u => u.UserId, (i, u) => new
					{
						i.Item,
						i.Package,
						i.Type,
						User = u
					})
					.Join(Services, i => i.Item.ServiceId, s => s.ServiceId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						Service = s
					})
					.Join(Servers, i => i.Service.ServerId, s => s.ServerId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						i.Service,
						Server = s
					})
					.Join(ResourceGroups, i => i.Type.GroupId, r => r.GroupId, (i, r) => new
					{
						i.Item.ItemId,
						i.Item.ItemName,
						i.Item.ItemTypeId,
						i.Type.DisplayName,
						i.Item.ServiceId,
						i.Item.PackageId,
						i.Package.PackageName,
						i.Service.ServiceName,
						i.Server.ServerId,
						i.Server.ServerName,
						r.GroupName,
						i.User.UserId,
						i.User.Username,
						UserFullName = i.User.FirstName + " " + i.User.LastName,
						i.Item.CreatedDate
					})
					.Where(i => groupName == null || groupName != null && i.GroupName == groupName);

				// select item properties, get corresponding item properties
				var properties = ServiceItemProperties
					.Join(items, p => p.ItemId, i => i.ItemId, (p, i) => new
					{
						p.ItemId,
						p.PropertyName,
						p.PropertyValue
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(items));
				dataSet.Tables.Add(EntityDataTable(properties));
				return dataSet;
			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServiceItemByName",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@itemName", itemName),
					new SqlParameter("@itemTypeName", itemTypeName),
					new SqlParameter("@groupName", groupName));
			}
		}

		public DataSet GetServiceItemsByName(int actorId, int packageId, string itemName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemsByName]
(
	@ActorID int,
	@PackageID int,
	@ItemName nvarchar(500)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
WHERE SI.PackageID = @PackageID
AND SI.ItemName LIKE @ItemName

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	U.FullName AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				// select service items
				var items = ServiceItems
					.Where(s => s.PackageId == packageId && s.ItemName == itemName)
					.Join(Packages, i => i.PackageId, p => p.PackageId, (si, p) => new
					{
						Item = si,
						Package = p
					})
					.Join(ServiceItemTypes, i => i.Item.ItemTypeId, t => t.ItemTypeId, (i, t) => new
					{
						i.Item,
						i.Package,
						Type = t
					})
					.Join(Users, i => i.Package.UserId, u => u.UserId, (i, u) => new
					{
						i.Item,
						i.Package,
						i.Type,
						User = u
					})
					.Join(Services, i => i.Item.ServiceId, s => s.ServiceId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						Service = s
					})
					.Join(Servers, i => i.Service.ServerId, s => s.ServerId, (i, s) => new
					{
						i.Item,
						i.Package,
						i.Type,
						i.User,
						i.Service,
						Server = s
					})
					.Join(ResourceGroups, i => i.Type.GroupId, r => r.GroupId, (i, r) => new
					{
						i.Item.ItemId,
						i.Item.ItemName,
						i.Item.ItemTypeId,
						i.Type.DisplayName,
						i.Item.ServiceId,
						i.Item.PackageId,
						i.Package.PackageName,
						i.Service.ServiceName,
						i.Server.ServerId,
						i.Server.ServerName,
						r.GroupName,
						i.User.UserId,
						i.User.Username,
						UserFullName = i.User.FirstName + " " + i.User.LastName,
						i.Item.CreatedDate
					});

				// select item properties, get corresponding item properties
				var properties = ServiceItemProperties
					.Join(items, p => p.ItemId, i => i.ItemId, (p, i) => new
					{
						p.ItemId,
						p.PropertyName,
						p.PropertyValue
					});

				var dataSet = new DataSet();
				dataSet.Tables.Add(EntityDataTable(items));
				dataSet.Tables.Add(EntityDataTable(properties));
				return dataSet;

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetServiceItemsByName",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@itemName", itemName));
			}
		}

		public int GetServiceItemsCountByNameAndServiceId(int actorId, int serviceId, string groupName,
			string itemName, string itemTypeName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetServiceItemsCountByNameAndServiceId]
(
	@ActorID int,
	@ServiceId int,
	@ItemName nvarchar(500),
	@GroupName nvarchar(100) = NULL,
	@ItemTypeName nvarchar(200)
)
AS
SELECT Count(*)
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
WHERE S.ServiceID = @ServiceId 
AND SIT.TypeName = @ItemTypeName
AND SI.ItemName = @ItemName
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))
RETURN 
				*/
				#endregion

				// select service items
				return ServiceItems
					.Where(s => s.ServiceId == serviceId && s.ItemName == itemName)
					.Join(ServiceItemTypes, i => i.ItemTypeId, t => t.ItemTypeId, (i, t) => new
					{
						Item = i,
						Type = t
					})
					.Where(s => s.Type.TypeName == itemTypeName)
					.Join(ResourceGroups, i => i.Type.GroupId, r => r.GroupId, (i, r) => r)
					.Where(r => groupName == null || groupName != null && r.GroupName == groupName)
					.Count();
			}
			else
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
		}

		public int AddServiceItem(int actorId, int serviceId, int packageId, string itemName,
			string itemTypeName, string xmlProperties)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[AddServiceItem]
(
	@ActorID int,
	@PackageID int,
	@ServiceID int,
	@ItemName nvarchar(500),
	@ItemTypeName nvarchar(200),
	@ItemID int OUTPUT,
	@XmlProperties ntext,
	@CreatedDate datetime
)
AS
BEGIN TRAN

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- get GroupID
DECLARE @GroupID int
SELECT
	@GroupID = PROV.GroupID
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
WHERE S.ServiceID = @ServiceID

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName
AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND GroupID = @GroupID))

-- Fix to allow plans assigned to serveradmin
IF (@ItemTypeName = 'SolidCP.Providers.HostedSolution.Organization, SolidCP.Providers.Base')
BEGIN
	IF NOT EXISTS (SELECT * FROM ServiceItems WHERE PackageID = 1)
	BEGIN
		INSERT INTO ServiceItems (PackageID, ItemTypeID,ServiceID,ItemName,CreatedDate)
		VALUES(1, @ItemTypeID, @ServiceID, 'System',  @CreatedDate)

		DECLARE @TempItemID int

		SET @TempItemID = SCOPE_IDENTITY()
		INSERT INTO ExchangeOrganizations (ItemID, OrganizationID)
		VALUES(@TempItemID, 'System')
	END
END

-- add item
INSERT INTO ServiceItems
(
	PackageID,
	ServiceID,
	ItemName,
	ItemTypeID,
	CreatedDate
)
VALUES
(
	@PackageID,
	@ServiceID,
	@ItemName,
	@ItemTypeID,
	@CreatedDate
)

SET @ItemID = SCOPE_IDENTITY()

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlProperties

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ServiceItemProperties
WHERE ItemID = @ItemID

CREATE TABLE #TempTable(
	ItemID int,
	PropertyName nvarchar(50),
	PropertyValue  nvarchar(max))

INSERT INTO #TempTable (ItemID, PropertyName, PropertyValue)
SELECT
	@ItemID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(max) '@value'
) as PV

-- Move data from temp table to real table
INSERT INTO ServiceItemProperties
(
	ItemID,
	PropertyName,
	PropertyValue
)
SELECT 
	ItemID, 
	PropertyName, 
	PropertyValue
FROM #TempTable

DROP TABLE #TempTable

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN 
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var groupId = Services
					.Where(s => s.ServiceId == serviceId)
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => (int?)p.GroupId)
					.FirstOrDefault();

				var itemTypeId = ServiceItemTypes
					.Where(t => t.TypeName == itemTypeName &&
						(groupId == null || groupId != null && t.GroupId == groupId))
					.Select(t => (int?)t.ItemTypeId)
					.FirstOrDefault();

				using (var transaction = Database.BeginTransaction())
				{
					// Fix to allow plans assigned to serveradmin
					if (itemTypeName == "SolidCP.Providers.HostedSolution.Organization, SolidCP.Providers.Base")
					{
						if (!ServiceItems.Any(s => s.PackageId == 1))
						{
							var serviceItem = new Data.Entities.ServiceItem()
							{
								PackageId = 1,
								ItemTypeId = itemTypeId,
								ServiceId = serviceId,
								ItemName = "System",
								CreatedDate = DateTime.Now
							};
							ServiceItems.Add(serviceItem);
							ExchangeOrganizations.Add(new Data.Entities.ExchangeOrganization()
							{
								Item = serviceItem,
								OrganizationId = "System"
							});
						}
					}

					// add item
					var item = new Data.Entities.ServiceItem()
					{
						PackageId = packageId,
						ItemTypeId = itemTypeId,
						ServiceId = serviceId,
						ItemName = itemName,
						CreatedDate = DateTime.Now
					};
					ServiceItems.Add(item);

					SaveChanges();

					ServiceItemProperties.Where(p => p.ItemId == item.ItemId).ExecuteDelete(ServiceItemProperties);

					var properties = XElement.Parse(xmlProperties)
						.Elements()
						.Select(e => new Data.Entities.ServiceItemProperty()
						{
							ItemId = item.ItemId,
							PropertyName = (string)e.Attribute("name"),
							PropertyValue = (string)e.Attribute("value")
						});

					ServiceItemProperties.AddRange(properties);

					SaveChanges();

					transaction.Commit();

					return item.ItemId;
				}
			}
			else
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
		}

		public void UpdateServiceItem(int actorId, int itemId, string itemName, string xmlProperties)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdateServiceItem]
(
	@ActorID int,
	@ItemID int,
	@ItemName nvarchar(500),
	@XmlProperties ntext
)
AS
BEGIN TRAN

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- update item
UPDATE ServiceItems SET ItemName = @ItemName
WHERE ItemID=@ItemID

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlProperties

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ServiceItemProperties
WHERE ItemID = @ItemID

-- Add the xml data into a temp table for the capability and robust
IF OBJECT_ID('tempdb..#TempTable') IS NOT NULL DROP TABLE #TempTable

CREATE TABLE #TempTable(
	ItemID int,
	PropertyName nvarchar(50),
	PropertyValue  nvarchar(max))

INSERT INTO #TempTable (ItemID, PropertyName, PropertyValue)
SELECT
	@ItemID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(max) '@value'
) as PV

-- Move data from temp table to real table
INSERT INTO ServiceItemProperties
(
	ItemID,
	PropertyName,
	PropertyValue
)
SELECT 
	ItemID, 
	PropertyName, 
	PropertyValue
FROM #TempTable

DROP TABLE #TempTable

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN
				 */
				#endregion

				var item = ServiceItems
					.FirstOrDefault(s => s.ItemId == itemId);
				var packageId = item?.PackageId;

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				using (var transaction = Database.BeginTransaction())
				{
					// update item
					item.ItemName = itemName;

					ServiceItemProperties.Where(p => p.ItemId == itemId).ExecuteDelete(ServiceItemProperties);

					var properties = XElement.Parse(xmlProperties)
						.Elements()
						.Select(e => new Data.Entities.ServiceItemProperty()
						{
							ItemId = itemId,
							PropertyName = (string)e.Attribute("name"),
							PropertyValue = (string)e.Attribute("value")
						});
					ServiceItemProperties.AddRange(properties);

					SaveChanges();

					transaction.Commit();
				}
			}
			else
			{
				// update item
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdateServiceItem",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ItemName", itemName),
					new SqlParameter("@ItemId", itemId),
					new SqlParameter("@XmlProperties", xmlProperties));
			}
		}

		public void DeleteServiceItem(int actorId, int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[DeleteServiceItem]
(
	@ActorID int,
	@ItemID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE Domains
SET ZoneItemID = NULL
WHERE ZoneItemID = @ItemID

DELETE FROM Domains
WHERE WebSiteID = @ItemID AND IsDomainPointer = 1

UPDATE Domains
SET WebSiteID = NULL
WHERE WebSiteID = @ItemID

UPDATE Domains
SET MailDomainID = NULL
WHERE MailDomainID = @ItemID

-- delete item comments
DELETE FROM Comments
WHERE ItemID = @ItemID AND ItemTypeID = 'SERVICE_ITEM'

-- delete item properties
DELETE FROM ServiceItemProperties
WHERE ItemID = @ItemID

-- delete external IP addresses
EXEC dbo.DeleteItemIPAddresses @ActorID, @ItemID

-- delete item
DELETE FROM ServiceItems
WHERE ItemID = @ItemID

COMMIT TRAN

RETURN
				*/
				#endregion

				var packageId = ServiceItems
					.Where(s => s.ItemId == itemId)
					.Select(s => s.PackageId)
					.FirstOrDefault();

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				using (var transaction = Database.BeginTransaction())
				{
					Domains.Where(d => d.WebSiteId == itemId && d.IsDomainPointer).ExecuteDelete(Domains);
#if NETCOREAPP
					Domains.Where(d => d.ZoneItemId == itemId).ExecuteUpdate(s => s.SetPropery(p => p.ZoneItemId, null));
					Domains.Where(d => d.WebSiteId == itemId).ExecuteUpdate(s => s.SetProperty(p => p.WebSiteId, null));
					Domains.Where(d => d.MailDomainId == itemId).ExecuteUpdate(s => s.SetProperty(p => p.MailDomainId, null));
#else
					foreach (var domain in Domains.Where(d => d.ZoneItemId == itemId)) domain.ZoneItemId = null;
					foreach (var domain in Domains.Where(d => d.WebSiteId == itemId)) domain.WebSiteId = null;
					foreach (var domain in Domains.Where(d => d.MailDomainId == itemId)) domain.MailDomainId = null;
#endif
					// delete item comments
					Comments.Where(c => c.ItemId == itemId && c.ItemTypeId == "SERVICE_ITEM").ExecuteDelete(Comments);

					// delete item properties
					ServiceItemProperties.Where(p => p.ItemId == itemId).ExecuteDelete(ServiceItemProperties);

					// delete external IP addresses
					DeleteItemIPAddresses(actorId, itemId);

					// delete item
					ServiceItems.Where(s => s.ItemId == itemId).ExecuteDelete(ServiceItems);

					SaveChanges();

					transaction.Commit();
				}
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteServiceItem",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ItemID", itemId));
			}
		}

		public void MoveServiceItem(int actorId, int itemId, int destinationServiceId, bool forAutodiscover)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[MoveServiceItem]
(
	@ActorID int,
	@ItemID int,
	@DestinationServiceID int,
	@forAutodiscover bit
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0 AND @forAutodiscover = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE ServiceItems
SET ServiceID = @DestinationServiceID
WHERE ItemID = @ItemID

COMMIT TRAN

RETURN
				*/
				#endregion

				var packageId = ServiceItems
					.Where(s => s.ItemId == itemId)
					.Select(s => s.PackageId)
					.FirstOrDefault();

				if (!forAutodiscover && !CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				using (var transaction = Database.BeginTransaction())
				{
#if NETCOREAPP
					ServiceItems.Where(s => s.ItemId == itemId).ExecuteUpdate(s => s.SetProperty(p => p.ServiceId, destinationServiceId));
#else
					foreach (var item in ServiceItems.Where(s => s.ItemId == itemId)) item.ServiceId = destinationServiceId;
					SaveChanges();
#endif
					transaction.Commit();
				}
			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "MoveServiceItem",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@DestinationServiceID", destinationServiceId),
					new SqlParameter("@forAutodiscover", forAutodiscover));
			}
		}

		public bool GetPackageAllocatedResource(int? packageId, int groupId, int? serverId)
		{
			#region Stored Procedure
			/*
CREATE FUNCTION [dbo].[GetPackageAllocatedResource]
(
	@PackageID int,
	@GroupID int,
	@ServerID int
)
RETURNS bit
AS
BEGIN

IF @PackageID IS NULL
RETURN 1

DECLARE @Result bit
SET @Result = 1 -- enabled

DECLARE @PID int, @ParentPackageID int
SET @PID = @PackageID

DECLARE @OverrideQuotas bit

IF @ServerID IS NULL OR @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

WHILE 1 = 1
BEGIN

	DECLARE @GroupEnabled int

	-- get package info
	SELECT
		@ParentPackageID = ParentPackageID,
		@OverrideQuotas = OverrideQuotas
	FROM Packages WHERE PackageID = @PID

	-- check if this is a root 'System' package
	SET @GroupEnabled = 1 -- enabled
	IF @ParentPackageID IS NULL
	BEGIN

		IF @ServerID = -1 OR @ServerID IS NULL
		RETURN 1

		IF EXISTS (SELECT VirtualServer FROM Servers WHERE ServerID = @ServerID AND VirtualServer = 1)
		BEGIN
			IF NOT EXISTS(
				SELECT
					DISTINCT(PROV.GroupID)
				FROM VirtualServices AS VS
				INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
				INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
				WHERE PROV.GroupID = @GroupID AND VS.ServerID = @ServerID
			)
			SET @GroupEnabled = 0
		END
		ELSE
		BEGIN
			IF NOT EXISTS(
				SELECT
					DISTINCT(PROV.GroupID)
				FROM Services AS S
				INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
				WHERE PROV.GroupID = @GroupID AND  S.ServerID = @ServerID
			)
			SET @GroupEnabled = 0
		END

		RETURN @GroupEnabled -- exit from the loop
	END
	ELSE -- parentpackage is not null
	BEGIN
		-- check the current package
		IF @OverrideQuotas = 1
		BEGIN
			IF NOT EXISTS(
				SELECT GroupID FROM PackageResources WHERE GroupID = @GroupID AND PackageID = @PID
			)
			SET @GroupEnabled = 0
		END
		ELSE
		BEGIN
			IF NOT EXISTS(
				SELECT HPR.GroupID FROM Packages AS P
				INNER JOIN HostingPlanResources AS HPR ON P.PlanID = HPR.PlanID
				WHERE HPR.GroupID = @GroupID AND P.PackageID = @PID
			)
			SET @GroupEnabled = 0
		END

		-- check addons
		IF EXISTS(
			SELECT HPR.GroupID FROM PackageAddons AS PA
			INNER JOIN HostingPlanResources AS HPR ON PA.PlanID = HPR.PlanID
			WHERE HPR.GroupID = @GroupID AND PA.PackageID = @PID
			AND PA.StatusID = 1 -- active add-on
		)
		SET @GroupEnabled = 1
	END

	IF @GroupEnabled = 0
		RETURN 0

	SET @PID = @ParentPackageID

END -- end while

RETURN @Result
END
			*/
			#endregion

			if (packageId == null) return true;

			if (serverId == null || serverId == 0)
			{
				serverId = Packages
					.Where(p => p.PackageId == packageId)
					.Select(p => p.ServerId)
					.FirstOrDefault();
			}

			var id = packageId;
			bool groupEnabled = true;

			while (groupEnabled)
			{
				var package = Packages
					.Where(p => p.PackageId == id)
					.Select(p => new { p.ParentPackageId, p.OverrideQuotas })
					.FirstOrDefault();

				// check if this is a root 'System' package
				if (package.ParentPackageId == null)
				{
					if (serverId == -1 || serverId == null) return true;

					if (Servers.Any(s => s.ServerId == serverId && s.VirtualServer))
					{
						if (!VirtualServices
							.Where(v => v.ServerId == serverId)
							.Join(Services, v => v.ServerId, s => s.ServiceId, (v, s) => s)
							.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => p)
							.Any(p => p.GroupId == groupId))
						{
							groupEnabled = false;
						}
					}
					else
					{
						if (!Services
							.Where(s => s.ServerId == serverId)
							.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => p)
							.Any(p => p.GroupId == groupId))
						{
							groupEnabled = false;
						}
					}

					return groupEnabled;
				}
				else // parentPackageId != null
				{
					// check the current package
					if (package.OverrideQuotas)
					{
						if (!PackageResources.Any(r => r.GroupId == groupId && r.PackageId == id))
						{
							groupEnabled = false;
						}
					}
					else
					{
						if (!Packages
							.Where(p => p.PackageId == id)
							.Join(HostingPlanResources, p => p.PlanId, r => r.PlanId, (p, r) => r)
							.Any(r => r.GroupId == groupId))
						{
							groupEnabled = false;
						}
					}

					// check addons
					if (PackageAddons
						.Where(p => p.PackageId == id && p.StatusId == 1)
						.Join(HostingPlanResources, p => p.PlanId, r => r.PlanId, (p, r) => r)
						.Any(r => r.GroupId == groupId))
					{
						groupEnabled = true;
					}
				}

				id = package.ParentPackageId;
			}

			return false;
		}

		public int GetPackageServiceId(int actorId, int packageId, string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetPackageServiceID]
(
	@ActorID int,
	@PackageID int,
	@GroupName nvarchar(100),
	@ServiceID int OUTPUT
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SET @ServiceID = 0

-- load group info
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM ResourceGroups
WHERE GroupName = @GroupName

-- check if user has this resource enabled
IF dbo.GetPackageAllocatedResource(@PackageID, @GroupID, NULL) = 0
BEGIN
	-- remove all resource services from the space
	DELETE FROM PackageServices FROM PackageServices AS PS
	INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
	INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
	WHERE P.GroupID = @GroupID AND PS.PackageID = @PackageID
	RETURN
END

-- check if the service is already distributed
SELECT
	@ServiceID = PS.ServiceID
FROM PackageServices AS PS
INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE PS.PackageID = @PackageID AND P.GroupID = @GroupID

IF @ServiceID <> 0
RETURN

-- distribute services
EXEC DistributePackageServices @ActorID, @PackageID

-- get distributed service again
SELECT
	@ServiceID = PS.ServiceID
FROM PackageServices AS PS
INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE PS.PackageID = @PackageID AND P.GroupID = @GroupID

RETURN
				*/
				#endregion

				// check rights
				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var groupId = ResourceGroups
					.Where(g => g.GroupName == groupName)
					.Select(g => g.GroupId)
					.FirstOrDefault();

				var package = Packages
					.Where(p => p.PackageId == packageId)
					.Include(p => p.Services)
					.FirstOrDefault();

				// check if user has this resource enabled
				if (!GetPackageAllocatedResource(packageId, groupId, null))
				{
					// remove all resource services from the space
					var servicesToRemove = package.Services
						.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
						{
							Service = s,
							Provider = p
						})
						.Where(g => g.Provider.GroupId == groupId)
						.Select(g => g.Service);

					foreach (var service in servicesToRemove) package.Services.Remove(service);
					
					SaveChanges();
				}

				// check if the service is already distributed
				var serviceId = package.Services
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						Service = s,
						Provider = p
					})
					.Where(g => g.Provider.GroupId == groupId)
					.Select(g => g.Service.ServiceId)
					.FirstOrDefault();

				if (serviceId != 0) return serviceId;

				// distribute services
				DistributePackageServices(actorId, packageId);

				// get distributed service again
				package = Packages
					.Where(p => p.PackageId == packageId)
					.Include(p => p.Services)
					.FirstOrDefault();
				
				serviceId = package.Services
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						Service = s,
						Provider = p
					})
					.Where(g => g.Provider.GroupId == groupId)
					.Select(g => g.Service.ServiceId)
					.FirstOrDefault();

				return serviceId;
			}
			else
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
		}

		public string GetMailFilterURL(int actorId, int packageId, string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetFilterURL]
(
 @ActorID int,
 @PackageID int,
 @GroupName nvarchar(100),
 @FilterUrl nvarchar(200) OUTPUT
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- load group info
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM ResourceGroups
WHERE GroupName = @GroupName

--print @GroupID 

Declare @ServiceID int
SELECT @ServiceID = PS.ServiceID FROM PackageServices AS PS
INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE PS.PackageID = @PackageID AND P.GroupID = @GroupID

SELECT
 @FilterUrl = PropertyValue
 FROM ServiceProperties AS SP
 WHERE @ServiceID = SP.ServiceID AND PropertyName = 'apiurl'
-- print  @FilterUrl
RETURN
				*/
				#endregion

				if (!CheckActorPackageRights(actorId, packageId))
					throw new AccessViolationException("You are not allowed to access this package");

				var groupId = ResourceGroups
					.Where(g => g.GroupName == groupName)
					.Select(g => g.GroupId)
					.FirstOrDefault();
				var package = Packages
					.Include(p => p.Services)
					.FirstOrDefault(p => p.PackageId == packageId);
				var serviceId = package.Services
					.Join(Providers, s => s.ProviderId, p => p.ProviderId, (s, p) => new
					{
						s.ServiceId,
						p.GroupId
					})
					.Where(s => s.GroupId == groupId)
					.Select(s => s.ServiceId)
					.FirstOrDefault();
				var filterUrl = ServiceProperties
					.Where(p => p.ServiceId == serviceId && p.PropertyName == "apiurl")
					.Select(p => p.PropertyValue)
					.FirstOrDefault();
				return filterUrl;
			}
			else
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
		}

		public string GetMailFilterUrlByHostingPlan(int actorId, int PlanID, string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[GetFilterURLByHostingPlan]
(
 @ActorID int,
 @PlanID int,
 @GroupName nvarchar(100),
 @FilterUrl nvarchar(200) OUTPUT
)
AS 

-- load ServerID info
DECLARE @ServerID int
select @ServerID = HostingPlans.ServerID from HostingPlans where PlanID = @PlanID
--print @ServerID 

--Check Server Type
DECLARE @IsVirtualServer int
select @IsVirtualServer = VirtualServer from Servers where ServerID = @ServerID

-- load group info
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM ResourceGroups
WHERE GroupName = @GroupName
--print @GroupID 

-- load ProviderID info
DECLARE @ProviderID int
select @ProviderID = providerid from Providers 
where GroupID = @GroupID  and ProviderName = 'MailCleaner'

Declare @ServiceID int
if  (@IsVirtualServer = 1)
	select @ServiceID = Services.ServiceID from Services   
	Join VirtualServices vs on vs.ServerID = @ServerID and vs.ServiceID = Services.ServiceID
	where ProviderID = @ProviderID
ELSE
 BEGIN
	select  @ServiceID = Services.ServiceID from Services  
	Where Services.ProviderID = @ProviderID and Services.ServerID = @ServerID
END; 

SELECT
 @FilterUrl = PropertyValue
 FROM ServiceProperties AS SP
 WHERE @ServiceID = SP.ServiceID AND PropertyName = 'apiurl'
 --print @FilterUrl
RETURN
				*/
				#endregion

				// load ServerID info
				var serverId = HostingPlans
					.Where(p => p.PlanId == PlanID)
					.Select(p => p.ServerId)
					.FirstOrDefault();
				var isVirtualServer = Servers
					.Where(s => s.ServerId == serverId)
					.Select(s => s.VirtualServer)
					.FirstOrDefault();
				var providerId = ResourceGroups
					.Where(g => g.GroupName == groupName)
					.Join(Providers, r => r.GroupId, p => p.GroupId, (r, p) => p)
					.Where(p => p.ProviderName == "MailCleaner")
					.Select(p => p.ProviderId)
					.FirstOrDefault();
				int serviceId;
				if (isVirtualServer)
				{
					serviceId = Services
						.Join(VirtualServices, s => s.ServiceId, v => v.ServiceId, (s, v) => new
						{
							Service = s,
							VirtualService = v
						})
						.Where(s => s.VirtualService.ServerId == serverId && s.Service.ProviderId == providerId)
						.Select(s => s.Service.ServiceId)
						.FirstOrDefault();
				} else
				{
					serviceId = Services
						.Where(s => s.ServerId == serverId && s.ProviderId == providerId)
						.Select(s => s.ServiceId)
						.FirstOrDefault();
				}
				var filterUrl = ServiceProperties
					.Where(p => p.ServiceId == serviceId && p.PropertyName == "apiurl")
					.Select(p => p.PropertyValue)
					.FirstOrDefault();
				return filterUrl;
			}
			else
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
		}


		public void UpdatePackageDiskSpace(int packageId, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdatePackageDiskSpace]
(
	@PackageID int,
	@xml ntext
)
AS
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
-- Execute a SELECT statement that uses the OPENXML rowset provider.

DECLARE @Items TABLE
(
	ItemID int,
	Bytes bigint
)

INSERT INTO @Items (ItemID, Bytes)
SELECT ItemID, DiskSpace FROM OPENXML (@idoc, '/items/item',1)
WITH
(
	ItemID int '@id',
	DiskSpace bigint '@bytes'
) as XSI

-- remove current diskspace
DELETE FROM PackagesDiskspace
WHERE PackageID = @PackageID

-- update package diskspace
INSERT INTO PackagesDiskspace (PackageID, GroupID, Diskspace)
SELECT
	@PackageID,
	SIT.GroupID,
	SUM(I.Bytes)
FROM @Items AS I
INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
GROUP BY SIT.GroupID

-- remove document
exec sp_xml_removedocument @idoc

RETURN
				*/
				#endregion

				// remove current diskspace
				PackagesDiskspaces.Where(d => d.PackageId == packageId).ExecuteDelete(PackagesDiskspaces);

				var items = XElement.Parse(xml)
					.Elements()
					.Select(e => new
					{
						ItemId = (int)e.Attribute("id"),
						Bytes = (long)e.Attribute("bytes")
					});
				var diskspace = items
					.Join(ServiceItems, it => it.ItemId, s => s.ItemId, (it, s) => new
					{
						Item = it,
						Service = s
					})
					.Join(ServiceItemTypes, s => s.Service.ItemTypeId, t => t.ItemTypeId, (s, t) => new
					{
						t.GroupId,
						s.Item.Bytes
					})
					.GroupBy(s => s.GroupId)
					.Select(g => new Data.Entities.PackagesDiskspace()
					{
						PackageId = packageId,
						GroupId = g.Key,
						DiskSpace = g.Sum(s => s.Bytes)
					});
				PackagesDiskspaces.AddRange(diskspace);
				SaveChanges();
			}
			else
			{
				ExecuteLongNonQuery(
					ObjectQualifier + "UpdatePackageDiskSpace",
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@xml", xml));
			}
		}

		public void UpdatePackageBandwidth(int packageId, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
CREATE PROCEDURE [dbo].[UpdatePackageBandwidth]
(
	@PackageID int,
	@xml ntext
)
AS
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

DECLARE @Items TABLE
(
	ItemID int,
	LogDate datetime,
	BytesSent bigint,
	BytesReceived bigint
)

INSERT INTO @Items
(
	ItemID,
	LogDate,
	BytesSent,
	BytesReceived
)
SELECT
	ItemID,
	CONVERT(datetime, LogDate, 101),
	BytesSent,
	BytesReceived
FROM OPENXML(@idoc, '/items/item',1) WITH
(
	ItemID int '@id',
	LogDate nvarchar(10) '@date',
    BytesSent bigint '@sent',
    BytesReceived bigint '@received'
)

-- delete current statistics
DELETE FROM PackagesBandwidth
FROM PackagesBandwidth AS PB
INNER JOIN (
	SELECT
		SIT.GroupID,
		I.LogDate
	FROM @Items AS I
	INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
	INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
	GROUP BY I.LogDate, SIT.GroupID
) AS STAT ON PB.LogDate = STAT.LogDate AND PB.GroupID = STAT.GroupID
WHERE PB.PackageID = @PackageID

-- insert new statistics
INSERT INTO PackagesBandwidth (PackageID, GroupID, LogDate, BytesSent, BytesReceived)
SELECT
	@PackageID,
	SIT.GroupID,
	I.LogDate,
	SUM(I.BytesSent),
	SUM(I.BytesReceived)
FROM @Items AS I
INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
GROUP BY I.LogDate, SIT.GroupID

-- remove document
exec sp_xml_removedocument @idoc

RETURN
				*/
				#endregion

			}
			else
			{
				ExecuteLongNonQuery(
					ObjectQualifier + "UpdatePackageBandwidth",
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@xml", xml));
			}
		}

		public DateTime GetPackageBandwidthUpdate(int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter prmUpdateDate = new SqlParameter("@UpdateDate", SqlDbType.DateTime);
				prmUpdateDate.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackageBandwidthUpdate",
					prmUpdateDate,
					new SqlParameter("@packageId", packageId));

				return (prmUpdateDate.Value != DBNull.Value) ? Convert.ToDateTime(prmUpdateDate.Value) : DateTime.MinValue;
			}
		}

		public void UpdatePackageBandwidthUpdate(int packageId, DateTime updateDate)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdatePackageBandwidthUpdate",
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@updateDate", updateDate));
			}
		}

		public IDataReader GetServiceItemType(int itemTypeId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetServiceItemType",
					new SqlParameter("@ItemTypeID", itemTypeId));
			}
		}

		public IDataReader GetServiceItemTypes()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetServiceItemTypes");
			}
		}
		#endregion

		#region Plans
		// Plans methods
		public DataSet GetHostingPlans(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "GetHostingPlans",
				new SqlParameter("@actorId", actorId),
				new SqlParameter("@userId", userId));
			}
		}

		public DataSet GetHostingAddons(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetHostingAddons",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@userId", userId));
			}
		}

		public DataSet GetUserAvailableHostingPlans(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetUserAvailableHostingPlans",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@userId", userId));
			}
		}

		public DataSet GetUserAvailableHostingAddons(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetUserAvailableHostingAddons",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@userId", userId));
			}
		}

		public IDataReader GetHostingPlan(int actorId, int planId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetHostingPlan",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PlanId", planId));
			}
		}

		public DataSet GetHostingPlanQuotas(int actorId, int packageId, int planId, int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetHostingPlanQuotas",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@planId", planId),
					new SqlParameter("@serverId", serverId));
			}
		}

		public int AddHostingPlan(int actorId, int userId, int packageId, string planName,
			string planDescription, bool available, int serverId, decimal setupPrice, decimal recurringPrice,
			int recurrenceUnit, int recurrenceLength, bool isAddon, string quotasXml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet UpdateHostingPlan(int actorId, int planId, int packageId, int serverId, string planName,
			string planDescription, bool available, decimal setupPrice, decimal recurringPrice,
			int recurrenceUnit, int recurrenceLength, string quotasXml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public int CopyHostingPlan(int planId, int userId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public int DeleteHostingPlan(int actorId, int planId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}
		#endregion

		#region Packages

		// Packages
		public DataSet GetMyPackages(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetMyPackages",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@UserID", userId));
			}
		}

		public DataSet GetPackages(int actorId, int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackages",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@UserID", userId));
			}
		}

		public DataSet GetNestedPackagesSummary(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetNestedPackagesSummary",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		public DataSet SearchServiceItemsPaged(int actorId, int userId, int itemTypeId, string filterValue,
			string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetPackagesPaged(int actorId, int userId, string filterColumn, string filterValue,
			string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetNestedPackagesPaged(int actorId, int packageId, string filterColumn, string filterValue,
			int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetPackagePackages(int actorId, int packageId, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackagePackages",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@recursive", recursive));
			}
		}

		public IDataReader GetPackage(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackage",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		public DataSet GetPackageQuotas(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackageQuotas",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		public DataSet GetParentPackageQuotas(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetParentPackageQuotas",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		public DataSet GetPackageQuotasForEdit(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackageQuotasForEdit",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		public DataSet AddPackage(int actorId, out int packageId, int userId, int planId, string packageName,
			string packageComments, int statusId, DateTime purchaseDate)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

				packageId = 0;
			}
			else
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
		}

		public DataSet UpdatePackage(int actorId, int packageId, int planId, string packageName,
			string packageComments, int statusId, DateTime purchaseDate,
			bool overrideQuotas, string quotasXml, bool defaultTopPackage)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}
		public void ChangePackageUser(int actorId, int packageId, int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "ChangePackageUser",
					new SqlParameter("@PackageId", packageId),
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@UserId", userId));
			}
		}

		public void UpdatePackageName(int actorId, int packageId, string packageName,
			string packageComments)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdatePackageName",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@packageName", packageName),
					new SqlParameter("@packageComments", packageComments));
			}
		}

		public void DeletePackage(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeletePackage",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		// Package Add-ons
		public DataSet GetPackageAddons(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackageAddons",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		public IDataReader GetPackageAddon(int actorId, int packageAddonId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackageAddon",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageAddonID", packageAddonId));
			}
		}

		public DataSet AddPackageAddon(int actorId, out int addonId, int packageId, int planId, int quantity,
			int statusId, DateTime purchaseDate, string comments)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

				addonId = 0;
			}
			else
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
		}

		public DataSet UpdatePackageAddon(int actorId, int packageAddonId, int planId, int quantity,
			int statusId, DateTime purchaseDate, string comments)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void DeletePackageAddon(int actorId, int packageAddonId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeletePackageAddon",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageAddonID", packageAddonId));
			}
		}

		public void UpdateServerPackageServices(int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				// FIXME
				int defaultActorID = 1;

				// get server packages
				IDataReader packagesReader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
					 @"SELECT PackageID FROM Packages WHERE ServerID = @ServerID",
					 new SqlParameter("@ServerID", serverId));

				// call DistributePackageServices for all packages on this server
				while (packagesReader.Read())
				{
					int packageId = (int)packagesReader["PackageID"];
					DistributePackageServices(defaultActorID, packageId);
				}
			}
		}

		public void DistributePackageServices(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DistributePackageServices",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageID", packageId));
			}
		}

		#endregion

		#region Packages Settings
		public IDataReader GetPackageSettings(int actorId, int packageId, string settingsName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackageSettings",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageId", packageId),
					new SqlParameter("@SettingsName", settingsName));
			}
		}
		public void UpdatePackageSettings(int actorId, int packageId, string settingsName, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdatePackageSettings",
					new SqlParameter("@PackageId", packageId),
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@SettingsName", settingsName),
					new SqlParameter("@Xml", xml));
			}
		}
		#endregion

		#region Quotas
		public IDataReader GetProviderServiceQuota(int providerId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetProviderServiceQuota",
					new SqlParameter("@providerId", providerId));
			}
		}

		public IDataReader GetPackageQuota(int actorId, int packageId, string quotaName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPackageQuota",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageID", packageId),
					new SqlParameter("@QuotaName", quotaName));
			}
		}
		#endregion

		#region Log
		public void AddAuditLogRecord(string recordId, int severityId,
			int userId, string username, int packageId, int itemId, string itemName, DateTime startDate, DateTime finishDate, string sourceName,
			string taskName, string executionLog)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetAuditLogRecordsPaged(int actorId, int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate,
			int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetAuditLogSources()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetAuditLogSources");
			}
		}

		public DataSet GetAuditLogTasks(string sourceName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetAuditLogTasks",
					new SqlParameter("@sourceName", sourceName));
			}
		}

		public IDataReader GetAuditLogRecord(string recordId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetAuditLogRecord",
					new SqlParameter("@recordId", recordId));
			}
		}

		public void DeleteAuditLogRecords(int actorId, int userId, int itemId, string itemName, DateTime startDate, DateTime endDate,
			int severityId, string sourceName, string taskName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void DeleteAuditLogRecordsComplete()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteAuditLogRecordsComplete");
			}
		}

		#endregion

		#region Reports
		public DataSet GetPackagesBandwidthPaged(int actorId, int userId, int packageId,
			DateTime startDate, DateTime endDate, string sortColumn,
			int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetPackagesDiskspacePaged(int actorId, int userId, int packageId, string sortColumn,
			int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetPackageBandwidth(int actorId, int packageId, DateTime startDate, DateTime endDate)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return ExecuteLongDataSet(
					ObjectQualifier + "GetPackageBandwidth",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageId", packageId),
					new SqlParameter("@StartDate", startDate),
					new SqlParameter("@EndDate", endDate));
			}
		}

		public DataSet GetPackageDiskspace(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return ExecuteLongDataSet(
					ObjectQualifier + "GetPackageDiskspace",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@PackageId", packageId));
			}
		}

		#endregion

		#region Scheduler

		public IDataReader GetBackgroundTask(string taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetBackgroundTask",
					new SqlParameter("@taskId", taskId));
			}
		}

		public IDataReader GetScheduleBackgroundTasks(int scheduleId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleBackgroundTasks",
					new SqlParameter("@scheduleId", scheduleId));
			}
		}

		public IDataReader GetBackgroundTasks(int actorId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetBackgroundTasks",
					new SqlParameter("@actorId", actorId));
			}
		}

		public IDataReader GetBackgroundTasks(Guid guid)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetThreadBackgroundTasks",
					new SqlParameter("@guid", guid));
			}
		}

		public IDataReader GetProcessBackgroundTasks(BackgroundTaskStatus status)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetProcessBackgroundTasks",
					new SqlParameter("@status", (int)status));
			}
		}

		public IDataReader GetBackgroundTopTask(Guid guid)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetBackgroundTopTask",
					new SqlParameter("@guid", guid));
			}
		}

		public int AddBackgroundTask(Guid guid, string taskId, int scheduleId, int packageId, int userId,
			int effectiveUserId, string taskName, int itemId, string itemName, DateTime startDate,
			int indicatorCurrent, int indicatorMaximum, int maximumExecutionTime, string source,
			int severity, bool completed, bool notifyOnComplete, BackgroundTaskStatus status)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void AddBackgroundTaskLog(int taskId, DateTime date, string exceptionStackTrace,
			bool innerTaskStart, int severity, string text, int textIdent, string xmlParameters)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public IDataReader GetBackgroundTaskLogs(int taskId, DateTime startLogTime)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetBackgroundTaskLogs",
					new SqlParameter("@taskId", taskId),
					new SqlParameter("@startLogTime", startLogTime));
			}
		}

		public void UpdateBackgroundTask(Guid guid, int taskId, int scheduleId, int packageId, string taskName, int itemId,
			string itemName, DateTime finishDate, int indicatorCurrent, int indicatorMaximum, int maximumExecutionTime,
			string source, int severity, bool completed, bool notifyOnComplete, BackgroundTaskStatus status)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@finishDate", finishDate == DateTime.MinValue ? DBNull.Value : (object)finishDate),
					new SqlParameter("@indicatorCurrent", indicatorCurrent),
					new SqlParameter("@indicatorMaximum", indicatorMaximum),
					new SqlParameter("@maximumExecutionTime", maximumExecutionTime),
					new SqlParameter("@source", source),
					new SqlParameter("@severity", severity),
					new SqlParameter("@completed", completed),
					new SqlParameter("@notifyOnComplete", notifyOnComplete),
					new SqlParameter("@status", (int)status));

			}
		}

		public IDataReader GetBackgroundTaskParams(int taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetBackgroundTaskParams",
					new SqlParameter("@taskId", taskId));
			}
		}

		public void AddBackgroundTaskParam(int taskId, string name, string value, string typeName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "AddBackgroundTaskParam",
					new SqlParameter("@taskId", taskId),
					new SqlParameter("@name", name),
					new SqlParameter("@value", value),
					new SqlParameter("@typeName", typeName));
			}
		}

		public void DeleteBackgroundTaskParams(int taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteBackgroundTaskParams",
					new SqlParameter("@taskId", taskId));
			}
		}

		public void AddBackgroundTaskStack(int taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "AddBackgroundTaskStack",
					new SqlParameter("@taskId", taskId));
			}
		}

		public void DeleteBackgroundTasks(Guid guid)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteBackgroundTasks",
					new SqlParameter("@guid", guid));
			}
		}

		public void DeleteBackgroundTask(int taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteBackgroundTask",
					new SqlParameter("@id", taskId));
			}
		}

		public IDataReader GetScheduleTasks(int actorId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleTasks",
					new SqlParameter("@actorId", actorId));
			}
		}

		public IDataReader GetScheduleTask(int actorId, string taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleTask",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@taskId", taskId));
			}
		}

		public DataSet GetSchedules(int actorId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetSchedules",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@packageId", packageId),
					new SqlParameter("@recursive", true));
			}
		}

		public DataSet GetSchedulesPaged(int actorId, int packageId, bool recursive,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetSchedule(int actorId, int scheduleId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetSchedule",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@scheduleId", scheduleId));
			}
		}
		public IDataReader GetScheduleInternal(int scheduleId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleInternal",
					new SqlParameter("@scheduleId", scheduleId));
			}
		}
		public DataSet GetNextSchedule()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetNextSchedule");
			}
		}
		public IDataReader GetScheduleParameters(int actorId, string taskId, int scheduleId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleParameters",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@taskId", taskId),
					new SqlParameter("@scheduleId", scheduleId));
			}
		}

		/// <summary>
		/// Loads view configuration for the task with specified id.
		/// </summary>
		/// <param name="taskId">Task id which points to task for which view configuration will be loaded.</param>
		/// <returns>View configuration for the task with supplied id.</returns>
		public IDataReader GetScheduleTaskViewConfigurations(string taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleTaskViewConfigurations",
					new SqlParameter("@taskId", taskId));
			}
		}

		public int AddSchedule(int actorId, string taskId, int packageId,
			string scheduleName, string scheduleTypeId, int interval,
			DateTime fromTime, DateTime toTime, DateTime startTime,
			DateTime nextRun, bool enabled, string priorityId, int historiesNumber,
			int maxExecutionTime, int weekMonthDay, string xmlParameters)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void UpdateSchedule(int actorId, int scheduleId, string taskId,
			string scheduleName, string scheduleTypeId, int interval,
			DateTime fromTime, DateTime toTime, DateTime startTime,
			DateTime lastRun, DateTime nextRun, bool enabled, string priorityId,
			int historiesNumber, int maxExecutionTime, int weekMonthDay, string xmlParameters)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void DeleteSchedule(int actorId, int scheduleId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteSchedule",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@scheduleId", scheduleId));
			}
		}

		public DataSet GetScheduleHistories(int actorId, int scheduleId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleHistories",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@scheduleId", scheduleId));
			}
		}
		public IDataReader GetScheduleHistory(int actorId, int scheduleHistoryId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetScheduleHistory",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@scheduleHistoryId", scheduleHistoryId));
			}
		}
		public int AddScheduleHistory(int actorId, int scheduleId,
			DateTime startTime, DateTime finishTime, string statusId, string executionLog)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}
		public void UpdateScheduleHistory(int actorId, int scheduleHistoryId,
			DateTime startTime, DateTime finishTime, string statusId, string executionLog)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}
		public void DeleteScheduleHistories(int actorId, int scheduleId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteScheduleHistories",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@scheduleId", scheduleId));
			}
		}
		#endregion

		#region Comments
		public DataSet GetComments(int actorId, int userId, string itemTypeId, int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetComments",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@userId", userId),
					new SqlParameter("@itemTypeId", itemTypeId),
					new SqlParameter("@itemId", itemId));
			}
		}

		public void AddComment(int actorId, string itemTypeId, int itemId,
			 string commentText, int severityId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "AddComment",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@itemTypeId", itemTypeId),
					new SqlParameter("@itemId", itemId),
					new SqlParameter("@commentText", commentText),
					new SqlParameter("@severityId", severityId));
			}
		}

		public void DeleteComment(int actorId, int commentId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteComment",
					new SqlParameter("@actorId", actorId),
					new SqlParameter("@commentId", commentId));
			}
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

		public DataTable EntityDataTable<TEntity>(IEnumerable<TEntity> set) => ObjectUtils.DataTableFromEntitySet<TEntity>(set);
		public DataSet EntityDataSet<TEntity>(IEnumerable<TEntity> set) => ObjectUtils.DataSetFromEntitySet<TEntity>(set);
		public EntityDataReader<TEntity> EntityDataReader<TEntity>(IEnumerable<TEntity> set) where TEntity : class => new EntityDataReader<TEntity>(set);

		#endregion

		#region Exchange Server

		public int AddExchangeAccount(int itemId, int accountType, string accountName,
			string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
			string mailboxManagerActions, string samAccountName, int mailboxPlanId, string subscriberNumber)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@SubscriberNumber", (string.IsNullOrEmpty(subscriberNumber) ? (object)DBNull.Value : (object)subscriberNumber)));

				return Convert.ToInt32(outParam.Value);
			}
		}

		public void AddExchangeAccountEmailAddress(int accountId, string emailAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddExchangeAccountEmailAddress",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@EmailAddress", emailAddress));
			}
		}

		public void AddExchangeOrganization(int itemId, string organizationId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddExchangeOrganization",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@OrganizationID", organizationId));
			}
		}

		public void AddExchangeOrganizationDomain(int itemId, int domainId, bool isHost)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddExchangeOrganizationDomain",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@DomainID", domainId),
					new SqlParameter("@IsHost", isHost));
			}
		}

		public void ChangeExchangeAcceptedDomainType(int itemId, int domainId, int domainTypeId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"ChangeExchangeAcceptedDomainType",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@DomainID", domainId),
					new SqlParameter("@DomainTypeID", domainTypeId));
			}
		}

		public IDataReader GetExchangeOrganizationStatistics(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeOrganizationStatistics",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public void DeleteUserEmailAddresses(int accountId, string primaryAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteUserEmailAddresses",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@PrimaryEmailAddress", primaryAddress));
			}
		}

		public void DeleteExchangeAccount(int itemId, int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeAccount",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@AccountID", accountId));
			}
		}


		public void DeleteExchangeAccountEmailAddress(int accountId, string emailAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeAccountEmailAddress",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@EmailAddress", emailAddress));
			}
		}

		public void DeleteExchangeOrganization(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeOrganization",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public void DeleteExchangeOrganizationDomain(int itemId, int domainId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeOrganizationDomain",
					new SqlParameter("@ItemId", itemId),
					new SqlParameter("@DomainID", domainId));
			}
		}

		public bool ExchangeAccountEmailAddressExists(string emailAddress, bool checkContacts)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"ExchangeAccountEmailAddressExists",
					new SqlParameter("@EmailAddress", emailAddress),
					new SqlParameter("@checkContacts", checkContacts),
					outParam);

				return Convert.ToBoolean(outParam.Value);
			}
		}

		public bool ExchangeOrganizationDomainExists(int domainId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"ExchangeOrganizationDomainExists",
					new SqlParameter("@DomainID", domainId),
					outParam);

				return Convert.ToBoolean(outParam.Value);
			}
		}
		public bool ExchangeOrganizationExists(string organizationId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"ExchangeOrganizationExists",
					new SqlParameter("@OrganizationID", organizationId),
					outParam);

				return Convert.ToBoolean(outParam.Value);
			}
		}
		public bool ExchangeAccountExists(string accountName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"ExchangeAccountExists",
					new SqlParameter("@AccountName", accountName),
					outParam);

				return Convert.ToBoolean(outParam.Value);
			}
		}
		public void UpdateExchangeAccount(int accountId, string accountName, ExchangeAccountType accountType,
			string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
			string mailboxManagerActions, string samAccountName, int mailboxPlanId, int archivePlanId, string subscriberNumber,
			bool EnableArchiving)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@EnableArchiving", EnableArchiving));
			}
		}
		public void UpdateExchangeAccountServiceLevelSettings(int accountId, int levelId, bool isVIP)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateExchangeAccountSLSettings",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@LevelID", (levelId == 0) ? (object)DBNull.Value : (object)levelId),
					new SqlParameter("@IsVIP", isVIP));
			}
		}

		public void UpdateExchangeAccountUserPrincipalName(int accountId, string userPrincipalName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateExchangeAccountUserPrincipalName",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@UserPrincipalName", userPrincipalName));
			}
		}
		public IDataReader GetExchangeAccount(int itemId, int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccount",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@AccountID", accountId));
			}
		}
		public IDataReader GetExchangeAccountByAccountName(int itemId, string accountName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccountByAccountName",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@AccountName", accountName));
			}
		}
		public IDataReader GetExchangeAccountByMailboxPlanId(int itemId, int MailboxPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccountByMailboxPlanId",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@MailboxPlanId", MailboxPlanId));
			}
		}

		public IDataReader GetExchangeAccountEmailAddresses(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccountEmailAddresses",
					new SqlParameter("@AccountID", accountId));
			}
		}
		public IDataReader GetExchangeOrganizationDomains(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeOrganizationDomains",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public IDataReader GetExchangeAccounts(int itemId, int accountType)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccounts",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@AccountType", accountType));
			}
		}
		public IDataReader GetExchangeAccountByAccountNameWithoutItemId(string userPrincipalName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccountByAccountNameWithoutItemId",
					new SqlParameter("@UserPrincipalName", userPrincipalName));
			}
		}

		public IDataReader GetExchangeMailboxes(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeMailboxes",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public IDataReader SearchExchangeAccountsByTypes(int actorId, int itemId, string accountTypes,
			string filterColumn, string filterValue, string sortColumn)
		{
			// check input parameters
			/*string[] types = accountTypes.Split(',');
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
			}*/

			if (!Regex.IsMatch(accountTypes, @"^\s*[0-9]+(\s*,\s*[0-9]+)*\s*$", RegexOptions.Singleline))
			{
				throw new ArgumentException("Wrong patameter", "accountTypes");
			}

			accountTypes = Regex.Replace(accountTypes, @"[ \t]", ""); // remove whitespace

			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"SearchExchangeAccountsByTypes",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@AccountTypes", accountTypes),
					new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
					new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
					new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)));
			}
		}

		public DataSet GetExchangeAccountsPaged(int actorId, int itemId, string accountTypes,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving)
		{
			// check input parameters
			/* string[] types = accountTypes.Split(',');
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

			string searchTypes = String.Join(",", types); */

			if (!Regex.IsMatch(accountTypes, @"^\s*[0-9]+(\s*,\s*[0-9]+)*\s*$", RegexOptions.Singleline))
			{
				throw new ArgumentException("Wrong patameter", "accountTypes");
			}

			accountTypes = Regex.Replace(accountTypes, @"[ \t]", ""); // remove whitespace

			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccountsPaged",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@AccountTypes", accountTypes),
					new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
					new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
					new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
					new SqlParameter("@StartRow", startRow),
					new SqlParameter("@MaximumRows", maximumRows),
					new SqlParameter("@Archiving", archiving));
			}
		}
		public IDataReader SearchExchangeAccounts(int actorId, int itemId, bool includeMailboxes,
			bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox,
			bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)));
			}
		}
		public IDataReader SearchExchangeAccount(int actorId, int accountType, string primaryEmailAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"SearchExchangeAccount",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@AccountType", accountType),
					new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress));
			}
		}
		#endregion

		#region Exchange Mailbox Plans
		public int AddExchangeMailboxPlan(int itemID, string mailboxPlan, bool enableActiveSync, bool enableIMAP, bool enableMAPI, bool enableOWA, bool enablePOP, bool enableAutoReply,
			bool isDefault, int issueWarningPct, int keepDeletedItemsDays, int mailboxSizeMB, int maxReceiveMessageSizeKB, int maxRecipients,
			int maxSendMessageSizeKB, int prohibitSendPct, int prohibitSendReceivePct, bool hideFromAddressBook, int mailboxPlanType,
			bool enabledLitigationHold, int recoverabelItemsSpace, int recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg,
			bool archiving, bool EnableArchiving, int ArchiveSizeMB, int ArchiveWarningPct, bool enableForceArchiveDeletion, bool isForJournaling)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@IsForJournaling", isForJournaling));

				return Convert.ToInt32(outParam.Value);
			}
		}

		public void UpdateExchangeMailboxPlan(int mailboxPlanID, string mailboxPlan, bool enableActiveSync, bool enableIMAP, bool enableMAPI, bool enableOWA, bool enablePOP, bool enableAutoReply,
			bool isDefault, int issueWarningPct, int keepDeletedItemsDays, int mailboxSizeMB, int maxReceiveMessageSizeKB, int maxRecipients,
			int maxSendMessageSizeKB, int prohibitSendPct, int prohibitSendReceivePct, bool hideFromAddressBook, int mailboxPlanType,
			bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg,
			bool Archiving, bool EnableArchiving, int ArchiveSizeMB, int ArchiveWarningPct, bool enableForceArchiveDeletion, bool isForJournaling)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@IsForJournaling", isForJournaling));
			}
		}

		public void DeleteExchangeMailboxPlan(int mailboxPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeMailboxPlan",
					new SqlParameter("@MailboxPlanId", mailboxPlanId));
			}
		}

		public IDataReader GetExchangeMailboxPlan(int mailboxPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeMailboxPlan",
					new SqlParameter("@MailboxPlanId", mailboxPlanId));
			}
		}

		public IDataReader GetExchangeMailboxPlans(int itemId, bool archiving)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeMailboxPlans",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@Archiving", archiving));
			}
		}

		public IDataReader GetExchangeOrganization(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeOrganization",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public void SetOrganizationDefaultExchangeMailboxPlan(int itemId, int mailboxPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetOrganizationDefaultExchangeMailboxPlan",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@MailboxPlanId", mailboxPlanId));
			}
		}

		public void SetExchangeAccountMailboxPlan(int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetExchangeAccountMailboxplan",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@MailboxPlanId", (mailboxPlanId == 0) ? (object)DBNull.Value : (object)mailboxPlanId),
					new SqlParameter("@ArchivingMailboxPlanId", (archivePlanId < 1) ? (object)DBNull.Value : (object)archivePlanId),
					new SqlParameter("@EnableArchiving", EnableArchiving));
			}
		}
		#endregion

		#region Exchange Retention Policy Tags
		public int AddExchangeRetentionPolicyTag(int ItemID, string TagName, int TagType, int AgeLimitForRetention, int RetentionAction)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@RetentionAction", RetentionAction));

				return Convert.ToInt32(outParam.Value);
			}
		}

		public void UpdateExchangeRetentionPolicyTag(int TagID, int ItemID, string TagName, int TagType, int AgeLimitForRetention, int RetentionAction)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@RetentionAction", RetentionAction));
			}
		}

		public void DeleteExchangeRetentionPolicyTag(int TagID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeRetentionPolicyTag",
					new SqlParameter("@TagID", TagID));
			}
		}

		public IDataReader GetExchangeRetentionPolicyTag(int TagID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeRetentionPolicyTag",
					new SqlParameter("@TagID", TagID));
			}
		}

		public IDataReader GetExchangeRetentionPolicyTags(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeRetentionPolicyTags",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public int AddExchangeMailboxPlanRetentionPolicyTag(int TagID, int MailboxPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@PlanTagID", SqlDbType.Int);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddExchangeMailboxPlanRetentionPolicyTag",
					outParam,
					new SqlParameter("@TagID", TagID),
					new SqlParameter("@MailboxPlanId", MailboxPlanId));

				return Convert.ToInt32(outParam.Value);
			}
		}
		public void DeleteExchangeMailboxPlanRetentionPolicyTag(int PlanTagID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeMailboxPlanRetentionPolicyTag",
					new SqlParameter("@PlanTagID", PlanTagID));
			}
		}

		public IDataReader GetExchangeMailboxPlanRetentionPolicyTags(int MailboxPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeMailboxPlanRetentionPolicyTags",
					new SqlParameter("@MailboxPlanId", MailboxPlanId));
			}
		}
		#endregion

		#region Exchange Disclaimers
		public int AddExchangeDisclaimer(int itemID, ExchangeDisclaimer disclaimer)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@DisclaimerText", disclaimer.DisclaimerText));

				return Convert.ToInt32(outParam.Value);
			}
		}

		public void UpdateExchangeDisclaimer(int itemID, ExchangeDisclaimer disclaimer)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateExchangeDisclaimer",
					new SqlParameter("@ExchangeDisclaimerId", disclaimer.ExchangeDisclaimerId),
					new SqlParameter("@DisclaimerName", disclaimer.DisclaimerName),
					new SqlParameter("@DisclaimerText", disclaimer.DisclaimerText));
			}
		}

		public void DeleteExchangeDisclaimer(int exchangeDisclaimerId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExchangeDisclaimer",
					new SqlParameter("@ExchangeDisclaimerId", exchangeDisclaimerId));
			}
		}

		public IDataReader GetExchangeDisclaimer(int exchangeDisclaimerId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeDisclaimer",
					new SqlParameter("@ExchangeDisclaimerId", exchangeDisclaimerId));
			}
		}

		public IDataReader GetExchangeDisclaimers(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeDisclaimers",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public void SetExchangeAccountDisclaimerId(int AccountID, int ExchangeDisclaimerId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				object id = null;
				if (ExchangeDisclaimerId != -1) id = ExchangeDisclaimerId;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetExchangeAccountDisclaimerId",
					new SqlParameter("@AccountID", AccountID),
					new SqlParameter("@ExchangeDisclaimerId", id));
			}
		}

		public int GetExchangeAccountDisclaimerId(int AccountID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				object objReturn = SqlHelper.ExecuteScalar(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetExchangeAccountDisclaimerId",
					new SqlParameter("@AccountID", AccountID));

				int ret;
				if (!int.TryParse(objReturn.ToString(), out ret)) return -1;
				return ret;
			}
		}
		#endregion

		#region Organizations

		public int AddAccessToken(AccessToken token)
		{
			return AddAccessToken(token.AccessTokenGuid, token.AccountId, token.ItemId, token.ExpirationDate, token.TokenType);
		}

		public int AddAccessToken(Guid accessToken, int accountId, int itemId, DateTime expirationDate, AccessTokenTypes type)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@TokenType", (int)type));

				// read identity
				return Convert.ToInt32(prmId.Value);
			}
		}

		public void SetAccessTokenResponseMessage(Guid accessToken, string response)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetAccessTokenSmsResponse",
					new SqlParameter("@AccessToken", accessToken),
					new SqlParameter("@SmsResponse", response));
			}
		}

		public void DeleteExpiredAccessTokens()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExpiredAccessTokenTokens");
			}
		}

		public IDataReader GetAccessTokenByAccessToken(Guid accessToken, AccessTokenTypes type)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetAccessTokenByAccessToken",
					new SqlParameter("@AccessToken", accessToken),
					new SqlParameter("@TokenType", type));
			}
		}

		public void DeleteAccessToken(Guid accessToken, AccessTokenTypes type)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteAccessToken",
					new SqlParameter("@AccessToken", accessToken),
					new SqlParameter("@TokenType", type));
			}
		}

		public void UpdateOrganizationSettings(int itemId, string settingsName, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdateExchangeOrganizationSettings",
					new SqlParameter("@ItemId", itemId),
					new SqlParameter("@SettingsName", settingsName),
					new SqlParameter("@Xml", xml));
			}
		}

		public IDataReader GetOrganizationSettings(int itemId, string settingsName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetExchangeOrganizationSettings",
					new SqlParameter("@ItemId", itemId),
					new SqlParameter("@SettingsName", settingsName));
			}
		}

		public int AddOrganizationDeletedUser(int accountId, int originAT, string storagePath, string folderName, string fileName, DateTime expirationDate)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@ExpirationDate", expirationDate));

				return Convert.ToInt32(outParam.Value);
			}
		}

		public void DeleteOrganizationDeletedUser(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"DeleteOrganizationDeletedUser",
					new SqlParameter("@ID", id));
			}
		}

		public IDataReader GetOrganizationDeletedUser(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetOrganizationDeletedUser",
					new SqlParameter("@AccountID", accountId));
			}
		}

		public IDataReader GetAdditionalGroups(int userId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetAdditionalGroups",
					new SqlParameter("@UserID", userId));
			}
		}

		public int AddAdditionalGroup(int userId, string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void DeleteAdditionalGroup(int groupId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"DeleteAdditionalGroup",
					new SqlParameter("@GroupID", groupId));
			}
		}

		public void UpdateAdditionalGroup(int groupId, string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateAdditionalGroup",
					new SqlParameter("@GroupID", groupId),
					new SqlParameter("@GroupName", groupName));
			}
		}
		public void DeleteOrganizationUser(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteOrganizationUsers",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public int GetItemIdByOrganizationId(string id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				object obj = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetItemIdByOrganizationId",
					new SqlParameter("@OrganizationId", id));

				return (obj == null || DBNull.Value == obj) ? 0 : (int)obj;
			}
		}

		public IDataReader GetOrganizationStatistics(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetOrganizationStatistics",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public IDataReader GetOrganizationGroupsByDisplayName(int itemId, string displayName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetOrganizationGroupsByDisplayName",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@DisplayName", displayName));
			}
		}

		public IDataReader SearchOrganizationAccounts(int actorId, int itemId,
			string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@IncludeMailboxes", includeMailboxes));
			}
		}

		public DataSet GetOrganizationObjectsByDomain(int itemId, string domainName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetOrganizationObjectsByDomain",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@DomainName", domainName));
			}
		}
		#endregion

		#region CRM

		public int GetCRMUsersCount(int itemId, string name, string email, int CALType)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		private SqlParameter GetFilterSqlParam(string paramName, string value)
		{
			if (string.IsNullOrEmpty(value))
				return new SqlParameter(paramName, DBNull.Value);

			return new SqlParameter(paramName, value);
		}

		public IDataReader GetCrmUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] sqlParams = new SqlParameter[] {
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
		}

		public IDataReader GetCRMOrganizationUsers(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMOrganizationUsers",
					new SqlParameter[] { new SqlParameter("@ItemID", itemId) });
			}
		}

		public void CreateCRMUser(int itemId, Guid crmId, Guid businessUnitId, int CALType)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "InsertCRMUser",
					new SqlParameter[] {
						new SqlParameter("@ItemID", itemId),
						new SqlParameter("@CrmUserID", crmId),
						new SqlParameter("@BusinessUnitId", businessUnitId),
						new SqlParameter("@CALType", CALType)
					});

			}
		}

		public void UpdateCRMUser(int itemId, int CALType)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "UpdateCRMUser",
					new SqlParameter[] {
						new SqlParameter("@ItemID", itemId),
						new SqlParameter("@CALType", CALType)
					});

			}
		}

		public IDataReader GetCrmUser(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMUser",
					new SqlParameter[] { new SqlParameter("@AccountID", itemId) });
				return reader;
			}
		}

		public int GetCrmUserCount(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOrganizationCRMUserCount",
					new SqlParameter[] { new SqlParameter("@ItemID", itemId) });
			}
		}

		public void DeleteCrmOrganization(int organizationId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "DeleteCRMOrganization",
					new SqlParameter[] { new SqlParameter("@ItemID", organizationId) });
			}
		}
		#endregion

		#region VPS - Virtual Private Servers

		public IDataReader GetVirtualMachinesPaged(int actorId, int packageId, string filterColumn, string filterValue,
			string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public IDataReader GetVirtualMachinesPaged2012(int actorId, int packageId, string filterColumn, string filterValue,
			string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public IDataReader GetVirtualMachinesPagedProxmox(int actorId, int packageId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}
		#endregion

		public IDataReader GetVirtualMachinesForPCPaged(int actorId, int packageId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		#region VPS - External Network

		public IDataReader GetUnallottedIPAddresses(int packageId, int serviceId, int poolId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					"GetUnallottedIPAddresses",
					new SqlParameter("@PackageId", packageId),
					new SqlParameter("@ServiceId", serviceId),
					new SqlParameter("@PoolId", poolId));
			}
		}

		public void AllocatePackageIPAddresses(int packageId, int orgId, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] param = new[] {
					new SqlParameter("@PackageID", packageId),
					new SqlParameter("@OrgID", orgId),
					new SqlParameter("@xml", xml)
				};

				ExecuteLongNonQuery("AllocatePackageIPAddresses", param);
			}
		}

		public IDataReader GetPackageIPAddresses(int packageId, int orgId, int poolId, string filterColumn, string filterValue,
			 string sortColumn, int startRow, int maximumRows, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public int GetPackageIPAddressesCount(int packageId, int orgId, int poolId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void DeallocatePackageIPAddress(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeallocatePackageIPAddress",
					new SqlParameter("@PackageAddressID", id));
			}
		}
		#endregion

		#region VPS - Private Network

		public IDataReader GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue,
			string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public IDataReader GetPackagePrivateIPAddresses(int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					"GetPackagePrivateIPAddresses",
					new SqlParameter("@PackageID", packageId));
				return reader;
			}
		}
		#endregion

		#region VPS - External Network Adapter
		public IDataReader GetPackageUnassignedIPAddresses(int actorId, int packageId, int orgId, int poolId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					"GetPackageUnassignedIPAddresses",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@PackageID", packageId),
					new SqlParameter("@OrgID", orgId),
					new SqlParameter("@PoolId", poolId));
			}
		}

		public IDataReader GetPackageIPAddress(int packageAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					"GetPackageIPAddress",
					new SqlParameter("@PackageAddressId", packageAddressId));
			}
		}

		public IDataReader GetItemIPAddresses(int actorId, int itemId, int poolId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					"GetItemIPAddresses",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PoolID", poolId));
			}
		}

		public int AddItemIPAddress(int actorId, int itemId, int packageAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"AddItemIPAddress",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PackageAddressID", packageAddressId));
			}
		}

		public int SetItemPrimaryIPAddress(int actorId, int itemId, int packageAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"SetItemPrimaryIPAddress",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PackageAddressID", packageAddressId));
			}
		}

		public int DeleteItemIPAddress(int actorId, int itemId, int packageAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"DeleteItemIPAddress",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PackageAddressID", packageAddressId));
			}
		}

		public int DeleteItemIPAddresses(int actorId, int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"DeleteItemIPAddresses",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId));
			}
		}
		#endregion

		#region VPS - Private Network Adapter
		public IDataReader GetItemPrivateIPAddresses(int actorId, int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					"GetItemPrivateIPAddresses",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId));
			}
		}

		public int AddItemPrivateIPAddress(int actorId, int itemId, string ipAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"AddItemPrivateIPAddress",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@IPAddress", ipAddress));
			}
		}

		public int SetItemPrivatePrimaryIPAddress(int actorId, int itemId, int privateAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"SetItemPrivatePrimaryIPAddress",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PrivateAddressID", privateAddressId));
			}
		}

		public int DeleteItemPrivateIPAddress(int actorId, int itemId, int privateAddressId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"DeleteItemPrivateIPAddress",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PrivateAddressID", privateAddressId));
			}
		}

		public int DeleteItemPrivateIPAddresses(int actorId, int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					"DeleteItemPrivateIPAddresses",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@ItemID", itemId));
			}
		}
		#endregion

		#region BlackBerry

		public void AddBlackBerryUser(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"AddBlackBerryUser",
					new[] { new SqlParameter("@AccountID", accountId) });
			}
		}

		public bool CheckBlackBerryUserExists(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckBlackBerryUserExists",
					new SqlParameter("@AccountID", accountId));
				return res > 0;
			}
		}

		public IDataReader GetBlackBerryUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public int GetBlackBerryUsersCount(int itemId, string name, string email)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] sqlParams = new SqlParameter[]
				{
					new SqlParameter("@ItemID", itemId),
					GetFilterSqlParam("@Name", name),
					GetFilterSqlParam("@Email", email),
				};

				return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetBlackBerryUsersCount", sqlParams);
			}
		}
		public void DeleteBlackBerryUser(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"DeleteBlackBerryUser",
					new[] { new SqlParameter("@AccountID", accountId) });
			}
		}
		#endregion

		#region OCS

		public void AddOCSUser(int accountId, string instanceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"AddOCSUser",
					new[] {
						new SqlParameter("@AccountID", accountId),
						new SqlParameter("@InstanceID", instanceId)
					});
			}
		}

		public bool CheckOCSUserExists(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckOCSUserExists",
					new SqlParameter("@AccountID", accountId));
				return res > 0;
			}
		}

		public IDataReader GetOCSUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}
		public int GetOCSUsersCount(int itemId, string name, string email)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] sqlParams = new SqlParameter[]
				{
					new SqlParameter("@ItemID", itemId),
					GetFilterSqlParam("@Name", name),
					GetFilterSqlParam("@Email", email),
				};

				return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOCSUsersCount", sqlParams);
			}
		}

		public void DeleteOCSUser(string instanceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"DeleteOCSUser",
					new[] { new SqlParameter("@InstanceId", instanceId) });

			}
		}

		public string GetOCSUserInstanceID(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return (string)SqlHelper.ExecuteScalar(ConnectionString,
					CommandType.StoredProcedure,
					"GetInstanceID",
					new[] { new SqlParameter("@AccountID", accountId) });
			}
		}

		#endregion

		#region SSL
		public int AddSSLRequest(int actorId, int packageId, int siteID, int userID, string friendlyname, string hostname, string csr, int csrLength, string distinguishedName, bool isRenewal, int previousID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@PreviousId", previousID));

				return Convert.ToInt32(prmId.Value);
			}
		}

		public void CompleteSSLRequest(int actorId, int packageId, int id, string certificate, string distinguishedName, string serialNumber, byte[] hash, DateTime validFrom, DateTime expiryDate)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void AddPFX(int actorId, int packageId, int siteID, int userID, string hostname, string friendlyName, string distinguishedName, int csrLength, string serialNumber, DateTime validFrom, DateTime expiryDate)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public DataSet GetSSL(int actorId, int packageId, int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetSSL",
					new SqlParameter("@SSLID", id));
			}
		}

		public DataSet GetCertificatesForSite(int actorId, int packageId, int siteId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetCertificatesForSite",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageId", packageId),
					new SqlParameter("@websiteid", siteId));
			}
		}

		public DataSet GetPendingCertificates(int actorId, int packageId, int id, bool recursive)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetPendingSSLForWebsite",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@PackageId", packageId),
					new SqlParameter("@websiteid", id),
					new SqlParameter("@Recursive", recursive));
			}
		}

		public IDataReader GetSSLCertificateByID(int actorId, int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetSSLCertificateByID",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@ID", id));
			}
		}

		public int CheckSSL(int siteID, bool renewal)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public IDataReader GetSiteCert(int actorId, int siteID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetSSLCertificateByID",
					new SqlParameter("@ActorId", actorId),
					new SqlParameter("@ID", siteID));
			}
		}

		public void DeleteCertificate(int actorId, int packageId, int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "DeleteCertificate",
					new SqlParameter("@ActorID", actorId),
					new SqlParameter("@PackageID", packageId),
					new SqlParameter("@id", id));
			}
		}

		public bool CheckSSLExistsForWebsite(int siteId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Bit);
				prmId.Direction = ParameterDirection.Output;
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "CheckSSLExistsForWebsite", prmId,
					new SqlParameter("@siteID", siteId),
					new SqlParameter("@SerialNumber", ""));
				return Convert.ToBoolean(prmId.Value);
			}
		}
		#endregion

		#region Lync

		public void AddLyncUser(int accountId, int lyncUserPlanId, string sipAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"AddLyncUser",
					new[] {
						new SqlParameter("@AccountID", accountId),
						new SqlParameter("@LyncUserPlanID", lyncUserPlanId),
						new SqlParameter("@SipAddress", sipAddress)
					});
			}
		}

		public void UpdateLyncUser(int accountId, string sipAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"UpdateLyncUser",
					new[] {
						new SqlParameter("@AccountID", accountId),
						new SqlParameter("@SipAddress", sipAddress)
					});
			}
		}


		public bool CheckLyncUserExists(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckLyncUserExists",
					new SqlParameter("@AccountID", accountId));
				return res > 0;
			}
		}

		public bool LyncUserExists(int accountId, string sipAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"LyncUserExists",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@SipAddress", sipAddress),
					outParam);

				return Convert.ToBoolean(outParam.Value);
			}
		}



		public IDataReader GetLyncUsers(int itemId, string sortColumn, string sortDirection, int startRow, int count)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] sqlParams = new SqlParameter[] {
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
		}


		public IDataReader GetLyncUsersByPlanId(int itemId, int planId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetLyncUsersByPlanId",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PlanId", planId));
			}
		}

		public int GetLyncUsersCount(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] sqlParams = new[] { new SqlParameter("@ItemID", itemId) };

				return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetLyncUsersCount", sqlParams);
			}
		}

		public void DeleteLyncUser(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"DeleteLyncUser",
					new[] { new SqlParameter("@AccountId", accountId) });

			}
		}

		public int AddLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@TelephonyVoicePolicy", lyncUserPlan.TelephonyVoicePolicy));

				return Convert.ToInt32(outParam.Value);
			}
		}


		public void UpdateLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@TelephonyVoicePolicy", lyncUserPlan.TelephonyVoicePolicy));
			}
		}

		public void DeleteLyncUserPlan(int lyncUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteLyncUserPlan",
					new SqlParameter("@LyncUserPlanId", lyncUserPlanId));
			}
		}

		public IDataReader GetLyncUserPlan(int lyncUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetLyncUserPlan",
					new SqlParameter("@LyncUserPlanId", lyncUserPlanId));
			}
		}


		public IDataReader GetLyncUserPlans(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetLyncUserPlans",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public void SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetOrganizationDefaultLyncUserPlan",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@LyncUserPlanId", lyncUserPlanId));
			}
		}

		public IDataReader GetLyncUserPlanByAccountId(int AccountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetLyncUserPlanByAccountId",
					new SqlParameter("@AccountID", AccountId));
			}
		}

		public void SetLyncUserLyncUserplan(int accountId, int lyncUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetLyncUserLyncUserplan",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@LyncUserPlanId", (lyncUserPlanId == 0) ? (object)DBNull.Value : (object)lyncUserPlanId));
			}
		}
		#endregion

		#region SfB

		public void AddSfBUser(int accountId, int sfbUserPlanId, string sipAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"AddSfBUser",
					new[] {
						new SqlParameter("@AccountID", accountId),
						new SqlParameter("@SfBUserPlanID", sfbUserPlanId),
						new SqlParameter("@SipAddress", sipAddress)
					});
			}
		}

		public void UpdateSfBUser(int accountId, string sipAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"UpdateSfBUser",
					new[] {
						new SqlParameter("@AccountID", accountId),
						new SqlParameter("@SipAddress", sipAddress)
					});
			}
		}

		public bool CheckSfBUserExists(int accountId)
		{
			int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckSfBUserExists",
				new SqlParameter("@AccountID", accountId));
			return res > 0;
		}

		public bool SfBUserExists(int accountId, string sipAddress)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SfBUserExists",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@SipAddress", sipAddress),
					outParam);

				return Convert.ToBoolean(outParam.Value);
			}
		}

		public IDataReader GetSfBUsers(int itemId, string sortColumn, string sortDirection, int startRow, int count)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] sqlParams = new[]
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
		}

		public IDataReader GetSfBUsersByPlanId(int itemId, int planId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetSfBUsersByPlanId",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@PlanId", planId));
			}
		}

		public int GetSfBUsersCount(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter[] sqlParams = new[] { new SqlParameter("@ItemID", itemId) };

				return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetSfBUsersCount", sqlParams);
			}
		}

		public void DeleteSfBUser(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"DeleteSfBUser",
					new[] { new SqlParameter("@AccountId", accountId) });
			}
		}

		public int AddSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@TelephonyVoicePolicy", sfbUserPlan.TelephonyVoicePolicy));

				return Convert.ToInt32(outParam.Value);
			}
		}


		public void UpdateSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@TelephonyVoicePolicy", sfbUserPlan.TelephonyVoicePolicy));
			}
		}

		public void DeleteSfBUserPlan(int sfbUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteSfBUserPlan",
					new SqlParameter("@SfBUserPlanId", sfbUserPlanId));
			}
		}

		public IDataReader GetSfBUserPlan(int sfbUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetSfBUserPlan",
					new SqlParameter("@SfBUserPlanId", sfbUserPlanId));
			}
		}


		public IDataReader GetSfBUserPlans(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetSfBUserPlans",
					new SqlParameter("@ItemID", itemId));
			}
		}


		public void SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetOrganizationDefaultSfBUserPlan",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@SfBUserPlanId", sfbUserPlanId));
			}
		}

		public IDataReader GetSfBUserPlanByAccountId(int AccountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetSfBUserPlanByAccountId",
					new SqlParameter("@AccountID", AccountId));
			}
		}


		public void SetSfBUserSfBUserplan(int accountId, int sfbUserPlanId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"SetSfBUserSfBUserplan",
					new SqlParameter("@AccountID", accountId),
					new SqlParameter("@SfBUserPlanId", (sfbUserPlanId == 0) ? (object)DBNull.Value : (object)sfbUserPlanId));
			}
		}
		#endregion

		public int GetPackageIdByName(string Name)
		{
			const bool UseEntityFrameworkForGetPackageIdByName = true;

			int packageId = -1;

			if (UseEntityFrameworkForGetPackageIdByName || UseEntityFramework)
			{
				packageId = Packages
					.Where(p => string.Equals(Name, p.PackageName, StringComparison.OrdinalIgnoreCase))
					.Select(p => (int?)p.PackageId)
					.SingleOrDefault() ?? -1;
				return packageId;
			}
			else
			{   // TODO is this a bug? Querying Providers instead of Packages? But this routine has no
				// references, so the bug does not show up
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
		}

		public int GetServiceIdByProviderForServer(int providerId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		#region Helicon Zoo

		public void GetHeliconZooProviderAndGroup(string providerName, out int providerId, out int groupId)
		{
			if (UseEntityFramework)
			{
				providerId = 0; groupId = 0;
			}
			else
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
		}

		public IDataReader GetHeliconZooQuotas(int providerId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void RemoveHeliconZooQuota(int groupId, string engineName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				int quotaId;

				// find quota id
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
@"SELECT TOP 1 QuotaID
FROM Quotas
WHERE QuotaName = @QuotaName AND GroupID = @GroupID",
					new SqlParameter("@QuotaName", engineName),
					new SqlParameter("@GroupID", groupId));

				reader.Read();
				quotaId = (int)reader["QuotaID"];

				// delete references from HostingPlanQuotas
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
					"DELETE FROM HostingPlanQuotas WHERE QuotaID = @QuotaID",
					new SqlParameter("@QuotaID", quotaId));

				// delete from Quotas
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
					"DELETE FROM Quotas WHERE QuotaID = @QuotaID",
					new SqlParameter("@QuotaID", quotaId));
			}
		}

		public void AddHeliconZooQuota(int groupId, int quotaId, string engineName, string engineDescription, int quotaOrder)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
@"INSERT INTO Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota)
VALUES (@QuotaID, @GroupID, @QuotaOrder, @QuotaName, @QuotaDescription, 1, 0)",
					new SqlParameter("@QuotaID", quotaId),
					new SqlParameter("@GroupID", groupId),
					new SqlParameter("@QuotaOrder", quotaOrder),
					new SqlParameter("@QuotaName", engineName),
					new SqlParameter("@QuotaDescription", engineDescription));
			}
		}

		public IDataReader GetEnabledHeliconZooQuotasForPackage(int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				int providerId, groupId;

				GetHeliconZooProviderAndGroup("HeliconZoo", out providerId, out groupId);

				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
@"SELECT HostingPlanQuotas.QuotaID, Quotas.QuotaName, Quotas.QuotaDescription
FROM HostingPlanQuotas 
INNER JOIN Packages ON HostingPlanQuotas.PlanID = Packages.PlanID 
INNER JOIN Quotas ON HostingPlanQuotas.QuotaID = Quotas.QuotaID
WHERE (Packages.PackageID = @PackageID) AND (Quotas.GroupID = @GroupID) AND (HostingPlanQuotas.QuotaValue = 1)",
					new SqlParameter("@PackageID", packageId),
					new SqlParameter("@GroupID", groupId));

				return reader;
			}
		}

		public int GetServiceIdForProviderIdAndPackageId(int providerId, int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
@"SELECT PackageServices.ServiceID 
FROM PackageServices
INNER JOIN Services ON PackageServices.ServiceID = Services.ServiceID
WHERE Services.ProviderID = @ProviderID and PackageID = @PackageID",
					new SqlParameter("@ProviderID", providerId),
					new SqlParameter("@PackageID", packageId));

				if (reader.Read())
				{
					return (int)reader["ServiceID"];
				}

				return -1;
			}
		}

		public int GetServerIdForPackage(int packageId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
@"SELECT TOP 1 
ServerID
FROM Packages
WHERE PackageID = @PackageID",
					new SqlParameter("@PackageID", packageId));

				if (reader.Read())
				{
					return (int)reader["ServerID"];
				}

				return -1;
			}
		}
		#endregion

		#region Enterprise Storage

		public int AddWebDavAccessToken(Base.HostedSolution.WebDavAccessToken accessToken)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@ItemId", accessToken.ItemId));

				// read identity
				return Convert.ToInt32(prmId.Value);
			}
		}

		public void DeleteExpiredWebDavAccessTokens()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteExpiredWebDavAccessTokens");
			}
		}

		public IDataReader GetWebDavAccessTokenById(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetWebDavAccessTokenById",
					new SqlParameter("@Id", id));
			}
		}

		public IDataReader GetWebDavAccessTokenByAccessToken(Guid accessToken)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetWebDavAccessTokenByAccessToken",
					new SqlParameter("@AccessToken", accessToken));
			}
		}

		public int AddEntepriseFolder(int itemId, string folderName, int folderQuota, string locationDrive, string homeFolder, string domain, int? storageSpaceFolderId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId));

				// read identity
				return Convert.ToInt32(prmId.Value);
			}
		}

		public void UpdateEntepriseFolderStorageSpaceFolder(int itemId, string folderName, int? storageSpaceFolderId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateEntepriseFolderStorageSpaceFolder",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@FolderName", folderName),
					new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId));
			}
		}

		public void DeleteEnterpriseFolder(int itemId, string folderName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteEnterpriseFolder",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@FolderName", folderName));
			}
		}

		public void UpdateEnterpriseFolder(int itemId, string folderID, string folderName, int folderQuota)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public IDataReader GetEnterpriseFolders(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetEnterpriseFolders",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public DataSet GetEnterpriseFoldersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@maximumRows", maximumRows));
			}
		}

		public IDataReader GetEnterpriseFolder(int itemId, string folderName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetEnterpriseFolder",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@FolderName", folderName));
			}
		}

		public IDataReader GetWebDavPortalUserSettingsByAccountId(int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetWebDavPortalUsersSettingsByAccountId",
					new SqlParameter("@AccountId", accountId));
			}
		}

		public int AddWebDavPortalUsersSettings(int accountId, string settings)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter settingsId = new SqlParameter("@WebDavPortalUsersSettingsId", SqlDbType.Int);
				settingsId.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddWebDavPortalUsersSettings",
					settingsId,
					new SqlParameter("@AccountId", accountId),
					new SqlParameter("@Settings", settings));

				// read identity
				return Convert.ToInt32(settingsId.Value);
			}
		}

		public void UpdateWebDavPortalUsersSettings(int accountId, string settings)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateWebDavPortalUsersSettings",
					new SqlParameter("@AccountId", accountId),
					new SqlParameter("@Settings", settings));
			}
		}

		public void DeleteAllEnterpriseFolderOwaUsers(int itemId, int folderId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteAllEnterpriseFolderOwaUsers",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@FolderID", folderId));
			}
		}

		public int AddEnterpriseFolderOwaUser(int itemId, int folderId, int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@AccountId", accountId));

				// read identity
				return Convert.ToInt32(id.Value);
			}
		}

		public IDataReader GetEnterpriseFolderOwaUsers(int itemId, int folderId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetEnterpriseFolderOwaUsers",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@FolderID", folderId));
			}
		}

		public IDataReader GetEnterpriseFolderId(int itemId, string folderName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetEnterpriseFolderId",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@FolderName", folderName));
			}
		}

		public IDataReader GetUserEnterpriseFolderWithOwaEditPermission(int itemId, int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetUserEnterpriseFolderWithOwaEditPermission",
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@AccountID", accountId));
			}
		}
		#endregion

		#region Support Service Levels

		public IDataReader GetSupportServiceLevels()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetSupportServiceLevels");
			}
		}

		public int AddSupportServiceLevel(string levelName, string levelDescription)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter outParam = new SqlParameter("@LevelID", SqlDbType.Int);
				outParam.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddSupportServiceLevel",
					outParam,
					new SqlParameter("@LevelName", levelName),
					new SqlParameter("@LevelDescription", levelDescription));

				return Convert.ToInt32(outParam.Value);
			}
		}

		public void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateSupportServiceLevel",
					new SqlParameter("@LevelID", levelID),
					new SqlParameter("@LevelName", levelName),
					new SqlParameter("@LevelDescription", levelDescription));
			}
		}

		public void DeleteSupportServiceLevel(int levelID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteSupportServiceLevel",
					new SqlParameter("@LevelID", levelID));
			}
		}

		public IDataReader GetSupportServiceLevel(int levelID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetSupportServiceLevel",
					new SqlParameter("@LevelID", levelID));
			}
		}

		public bool CheckServiceLevelUsage(int levelID)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckServiceLevelUsage",
					new SqlParameter("@LevelID", levelID));
				return res > 0;
			}
		}

		#endregion

		#region Storage Spaces 

		public DataSet GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpaceLevelsPaged",
					new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
					new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
					new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
					new SqlParameter("@startRow", startRow),
					new SqlParameter("@maximumRows", maximumRows));
			}
		}

		public IDataReader GetStorageSpaceLevelById(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpaceLevelById",
					new SqlParameter("@ID", id));
			}
		}

		public int UpdateStorageSpaceLevel(StorageSpaceLevel level)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateStorageSpaceLevel",
					new SqlParameter("@ID", level.Id),
					new SqlParameter("@Name", level.Name),
					new SqlParameter("@Description", level.Description));

				return level.Id;
			}
		}

		public int InsertStorageSpaceLevel(StorageSpaceLevel level)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter id = new SqlParameter("@ID", SqlDbType.Int);
				id.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"InsertStorageSpaceLevel",
					id,
					new SqlParameter("@Name", level.Name),
					new SqlParameter("@Description", level.Description));

				// read identity
				return Convert.ToInt32(id.Value);
			}
		}

		public void RemoveStorageSpaceLevel(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"RemoveStorageSpaceLevel",
					new SqlParameter("@ID", id));
			}
		}

		public IDataReader GetStorageSpaceLevelResourceGroups(int levelId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetLevelResourceGroups",
					new SqlParameter("@LevelId", levelId));
			}
		}

		public void RemoveStorageSpaceLevelResourceGroups(int levelId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteLevelResourceGroups",
					new SqlParameter("@LevelId", levelId));
			}
		}

		public void AddStorageSpaceLevelResourceGroup(int levelId, int groupId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddLevelResourceGroups",
					new SqlParameter("@LevelId", levelId),
					new SqlParameter("@GroupId", groupId));
			}
		}

		public DataSet GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpacesPaged",
					new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
					new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
					new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
					new SqlParameter("@startRow", startRow),
					new SqlParameter("@maximumRows", maximumRows));
			}
		}

		public IDataReader GetStorageSpaceById(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpaceById",
					new SqlParameter("@ID", id));
			}
		}

		public IDataReader GetStorageSpaceByServiceAndPath(int serverId, string path)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpaceByServiceAndPath",
					new SqlParameter("@ServerId", serverId),
					new SqlParameter("@Path", path));
			}
		}

		public int UpdateStorageSpace(StorageSpace space)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@UncPath", space.UncPath));

				return space.Id;
			}
		}

		public int InsertStorageSpace(StorageSpace space)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@UncPath", space.UncPath));

				// read identity
				return Convert.ToInt32(id.Value);
			}
		}

		public void RemoveStorageSpace(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"RemoveStorageSpace",
					new SqlParameter("@ID", id));
			}
		}

		public DataSet GetStorageSpacesByLevelId(int levelId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpacesByLevelId",
					new SqlParameter("@LevelId", levelId));
			}
		}

		public IDataReader GetStorageSpacesByResourceGroupName(string groupName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpacesByResourceGroupName",
					new SqlParameter("@ResourceGroupName", groupName));
			}
		}

		public int CreateStorageSpaceFolder(StorageSpaceFolder folder)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				folder.Id = CreateStorageSpaceFolder(folder.Name, folder.StorageSpaceId, folder.Path, folder.UncPath, folder.IsShared, folder.FsrmQuotaType, folder.FsrmQuotaSizeBytes);

				return folder.Id;
			}
		}

		public int CreateStorageSpaceFolder(string name, int storageSpaceId, string path, string uncPath, bool isShared, QuotaType quotaType, long fsrmQuotaSizeBytes)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@FsrmQuotaSizeBytes", fsrmQuotaSizeBytes));

				// read identity
				return Convert.ToInt32(id.Value);
			}
		}

		public int UpdateStorageSpaceFolder(StorageSpaceFolder folder)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@FsrmQuotaSizeBytes", folder.FsrmQuotaSizeBytes));

				return folder.Id;
			}
		}

		public int UpdateStorageSpaceFolder(int id, string folderName, int storageSpaceId, string path, string uncPath, bool isShared, QuotaType type, long fsrmQuotaSizeBytes)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@FsrmQuotaSizeBytes", fsrmQuotaSizeBytes));

				return id;
			}
		}

		public IDataReader GetStorageSpaceFoldersByStorageSpaceId(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpaceFoldersByStorageSpaceId",
					new SqlParameter("@StorageSpaceId", id));
			}
		}

		public IDataReader GetStorageSpaceFolderById(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetStorageSpaceFolderById",
					new SqlParameter("@ID", id));
			}
		}

		public void RemoveStorageSpaceFolder(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"RemoveStorageSpaceFolder",
					new SqlParameter("@ID", id));
			}
		}
		#endregion

		#region RDS

		public int CheckRDSServer(string ServerFQDN)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
				prmId.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "CheckRDSServer",
					prmId,
					new SqlParameter("@ServerFQDN", ServerFQDN));

				return Convert.ToInt32(prmId.Value);
			}
		}

		public IDataReader GetRdsServerSettings(int serverId, string settingsName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetRDSServerSettings",
					new SqlParameter("@ServerId", serverId),
					new SqlParameter("@SettingsName", settingsName));
			}
		}

		public void UpdateRdsServerSettings(int serverId, string settingsName, string xml)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "UpdateRDSServerSettings",
					new SqlParameter("@ServerId", serverId),
					new SqlParameter("@SettingsName", settingsName),
					new SqlParameter("@Xml", xml));
			}
		}

		public int AddRdsCertificate(int serviceId, string content, byte[] hash, string fileName, DateTime? validFrom, DateTime? expiryDate)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@ExpiryDate", expiryDate));

				return Convert.ToInt32(rdsCertificateId.Value);
			}
		}

		public IDataReader GetRdsCertificateByServiceId(int serviceId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSCertificateByServiceId",
					new SqlParameter("@ServiceId", serviceId));
			}
		}

		public IDataReader GetRdsCollectionSettingsByCollectionId(int collectionId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSCollectionSettingsByCollectionId",
					new SqlParameter("@RDSCollectionID", collectionId));
			}
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
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@AuthenticateUsingNLA", AuthenticateUsingNLA));

				return Convert.ToInt32(rdsCollectionSettingsId.Value);
			}
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
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@AuthenticateUsingNLA", AuthenticateUsingNLA));
			}
		}

		public void DeleteRDSCollectionSettings(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteRDSCollectionSettings",
					new SqlParameter("@Id", id));
			}
		}

		public IDataReader GetRDSCollectionsByItemId(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSCollectionsByItemId",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public IDataReader GetRDSCollectionByName(string name)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSCollectionByName",
					new SqlParameter("@Name", name));
			}
		}

		public IDataReader GetRDSCollectionById(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSCollectionById",
					new SqlParameter("@ID", id));
			}
		}

		public DataSet GetRDSCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@maximumRows", maximumRows));
			}
		}

		public int AddRDSCollection(int itemId, string name, string description, string displayName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@DisplayName", displayName));

				// read identity
				return Convert.ToInt32(rdsCollectionId.Value);
			}
		}

		public int GetOrganizationRdsUsersCount(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public int GetOrganizationRdsCollectionsCount(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public int GetOrganizationRdsServersCount(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
		}

		public void UpdateRDSCollection(RdsCollection collection)
		{
			UpdateRDSCollection(collection.Id, collection.ItemId, collection.Name, collection.Description, collection.DisplayName);
		}

		public void UpdateRDSCollection(int id, int itemId, string name, string description, string displayName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateRDSCollection",
					new SqlParameter("@Id", id),
					new SqlParameter("@ItemID", itemId),
					new SqlParameter("@Name", name),
					new SqlParameter("@Description", description),
					new SqlParameter("@DisplayName", displayName));
			}
		}

		public void DeleteRDSServerSettings(int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteRDSServerSettings",
					new SqlParameter("@ServerId", serverId));
			}
		}

		public void DeleteRDSCollection(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteRDSCollection",
					new SqlParameter("@Id", id));
			}
		}

		public int AddRDSServer(string name, string fqdName, string description, string controller)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@Controller", controller));

				// read identity
				return Convert.ToInt32(rdsServerId.Value);
			}
		}

		public IDataReader GetRDSServersByItemId(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSServersByItemId",
					new SqlParameter("@ItemID", itemId));
			}
		}

		public DataSet GetRDSServersPaged(int? itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string controller, bool ignoreItemId = false, bool ignoreRdsCollectionId = false)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@Controller", controller));
			}
		}

		public IDataReader GetRDSServerById(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSServerById",
					new SqlParameter("@ID", id));
			}
		}

		public IDataReader GetRDSServersByCollectionId(int collectionId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSServersByCollectionId",
					new SqlParameter("@RdsCollectionId", collectionId));
			}
		}

		public void DeleteRDSServer(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteRDSServer",
					new SqlParameter("@Id", id));
			}
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

			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{

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
					new SqlParameter("@ConnectionEnabled", connEnabled));
			}
		}

		public void AddRDSServerToCollection(int serverId, int rdsCollectionId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddRDSServerToCollection",
					new SqlParameter("@Id", serverId),
					new SqlParameter("@RDSCollectionId", rdsCollectionId));
			}
		}

		public void AddRDSServerToOrganization(int itemId, int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddRDSServerToOrganization",
					new SqlParameter("@Id", serverId),
					new SqlParameter("@ItemID", itemId));
			}
		}

		public void RemoveRDSServerFromOrganization(int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"RemoveRDSServerFromOrganization",
					new SqlParameter("@Id", serverId));
			}
		}

		public void RemoveRDSServerFromCollection(int serverId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"RemoveRDSServerFromCollection",
					new SqlParameter("@Id", serverId));
			}
		}

		public IDataReader GetRDSCollectionUsersByRDSCollectionId(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSCollectionUsersByRDSCollectionId",
					new SqlParameter("@id", id));
			}
		}

		public void AddRDSUserToRDSCollection(int rdsCollectionId, int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddUserToRDSCollection",
					new SqlParameter("@RDSCollectionId", rdsCollectionId),
					new SqlParameter("@AccountID", accountId));
			}
		}

		public void RemoveRDSUserFromRDSCollection(int rdsCollectionId, int accountId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"RemoveRDSUserFromRDSCollection",
					new SqlParameter("@RDSCollectionId", rdsCollectionId),
					new SqlParameter("@AccountID", accountId));
			}
		}

		public int GetRDSControllerServiceIDbyFQDN(string fqdnName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlParameter prmController = new SqlParameter("@Controller", SqlDbType.Int);
				prmController.Direction = ParameterDirection.Output;

				SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
					ObjectQualifier + "GetRDSControllerServiceIDbyFQDN",
					new SqlParameter("@RdsfqdnName", fqdnName),
					prmController);

				return Convert.ToInt32(prmController.Value);
			}
		}

		#endregion

		#region MX|NX Services

		public IDataReader GetAllPackages()
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetAllPackages");
			}
		}

		public IDataReader GetDomainDnsRecords(int domainId, DnsRecordType recordType)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetDomainDnsRecords",
					new SqlParameter("@DomainId", domainId),
					new SqlParameter("@RecordType", recordType));
			}
		}

		public IDataReader GetDomainAllDnsRecords(int domainId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetDomainAllDnsRecords",
					new SqlParameter("@DomainId", domainId));
			}
		}

		public void AddDomainDnsRecord(DnsRecordInfo domainDnsRecord)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"AddDomainDnsRecord",
					new SqlParameter("@DomainId", domainDnsRecord.DomainId),
					new SqlParameter("@RecordType", domainDnsRecord.RecordType),
					new SqlParameter("@DnsServer", domainDnsRecord.DnsServer),
					new SqlParameter("@Value", domainDnsRecord.Value),
					new SqlParameter("@Date", domainDnsRecord.Date));
			}
		}

		public IDataReader GetScheduleTaskEmailTemplate(string taskId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetScheduleTaskEmailTemplate",
					new SqlParameter("@taskId", taskId));
			}
		}

		public void DeleteDomainDnsRecord(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"DeleteDomainDnsRecord",
					new SqlParameter("@Id", id));
			}
		}

		public void UpdateDomainCreationDate(int domainId, DateTime date)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				UpdateDomainDate(domainId, "UpdateDomainCreationDate", date);
			}
		}

		public void UpdateDomainExpirationDate(int domainId, DateTime date)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				UpdateDomainDate(domainId, "UpdateDomainExpirationDate", date);
			}
		}

		public void UpdateDomainLastUpdateDate(int domainId, DateTime date)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				UpdateDomainDate(domainId, "UpdateDomainLastUpdateDate", date);
			}
		}

		private void UpdateDomainDate(int domainId, string stroredProcedure, DateTime date)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				stroredProcedure,
				new SqlParameter("@DomainId", domainId),
				new SqlParameter("@Date", date));
		}

		public void UpdateDomainDates(int domainId, DateTime? domainCreationDate, DateTime? domainExpirationDate, DateTime? domainLastUpdateDate)
		{
			if (UseEntityFramework)
			{

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateDomainDates",
					new SqlParameter("@DomainId", domainId),
					new SqlParameter("@DomainCreationDate", domainCreationDate),
					new SqlParameter("@DomainExpirationDate", domainExpirationDate),
					new SqlParameter("@DomainLastUpdateDate", domainLastUpdateDate));
			}
		}

		public void UpdateWhoisDomainInfo(int domainId, DateTime? domainCreationDate, DateTime? domainExpirationDate, DateTime? domainLastUpdateDate, string registrarName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(
					ConnectionString,
					CommandType.StoredProcedure,
					"UpdateWhoisDomainInfo",
					new SqlParameter("@DomainId", domainId),
					new SqlParameter("@DomainCreationDate", domainCreationDate),
					new SqlParameter("@DomainExpirationDate", domainExpirationDate),
					new SqlParameter("@DomainLastUpdateDate", domainLastUpdateDate),
					new SqlParameter("@DomainRegistrarName", registrarName));
			}
		}
		#endregion

		#region Organization Storage Space Folders
		public IDataReader GetOrganizationStoragSpaceFolders(int itemId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetOrganizationStoragSpaceFolders",
					new SqlParameter("@ItemId", itemId));
			}
		}

		public IDataReader GetOrganizationStoragSpacesFolderByType(int itemId, string type)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetOrganizationStoragSpacesFolderByType",
					new SqlParameter("@ItemId", itemId),
					new SqlParameter("@Type", type));
			}
		}

		public void DeleteOrganizationStoragSpacesFolder(int id)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				SqlHelper.ExecuteNonQuery(ConnectionString,
					CommandType.StoredProcedure,
					"DeleteOrganizationStoragSpacesFolder",
					new SqlParameter("@ID", id));
			}
		}

		public int AddOrganizationStoragSpacesFolder(int itemId, string type, int storageSpaceFolderId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId));

				return Convert.ToInt32(outParam.Value);
			}
		}

		public IDataReader GetOrganizationStorageSpacesFolderById(int itemId, int folderId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteReader(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetOrganizationStorageSpacesFolderById",
					new SqlParameter("@ItemId", itemId),
					new SqlParameter("@ID", folderId));
			}
		}
		#endregion

		#region RDS Messages        

		public DataSet GetRDSMessagesByCollectionId(int rdsCollectionId)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
			{
				return SqlHelper.ExecuteDataset(
					ConnectionString,
					CommandType.StoredProcedure,
					"GetRDSMessages",
					new SqlParameter("@RDSCollectionId", rdsCollectionId));
			}
		}

		public int AddRDSMessage(int rdsCollectionId, string messageText, string userName)
		{
			if (UseEntityFramework)
			{
				#region Stored Procedure
				/*
				*/
				#endregion

			}
			else
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
					new SqlParameter("@Date", DateTime.Now));

				return Convert.ToInt32(rdsMessageId.Value);
			}
		}

		#endregion

	}
}

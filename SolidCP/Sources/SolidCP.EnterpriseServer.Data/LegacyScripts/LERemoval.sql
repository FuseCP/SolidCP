SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TrustedSites')
DROP Table [dbo].[TrustedSites];  
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @ObjectName NVARCHAR(100)
SELECT @ObjectName = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS
WHERE [object_id] = OBJECT_ID('[dbo].[SSLCertificates]') AND [name] = 'LetsEncryptID';
If (@ObjectName IS NOT NULL)
BEGIN
EXEC('ALTER TABLE [dbo].[SSLCertificates] DROP CONSTRAINT ' + @ObjectName)
END
GO

DECLARE @ObjectName NVARCHAR(100)
SELECT @ObjectName = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS
WHERE [object_id] = OBJECT_ID('[dbo].[SSLCertificates]') AND [name] = 'IsLetsEncryptCertificate';
If (@ObjectName IS NOT NULL)
BEGIN
EXEC('ALTER TABLE [dbo].[SSLCertificates] DROP CONSTRAINT ' + @ObjectName)
END
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SSLCertificates' AND COLUMN_NAME = 'IsLetsEncryptCertificate')
BEGIN
ALTER TABLE [dbo].[SSLCertificates] DROP COLUMN [IsLetsEncryptCertificate];
END
GO

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SSLCertificates' AND COLUMN_NAME = 'SAN')
BEGIN
ALTER TABLE [dbo].[SSLCertificates] DROP COLUMN [SAN];
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM [dbo].[SystemSettings] WHERE [SettingsName] = 'LetsEncrypt')
BEGIN
	DELETE FROM [dbo].[SystemSettings]
	Where [SettingsName] like 'LetsEncrypt';
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DELETE FROM [HostingPlanQuotas] WHERE [QuotaID] = '335'
GO
DELETE FROM [PackageQuotas] WHERE [QuotaID] = '335'
GO

IF EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '335')
BEGIN
	DELETE FROM [dbo].[Quotas] 
	Where [QuotaName] like 'Web.LetsEncryptSSL';
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [PropertyName] = 'ACMESharpVaultProfilesRootPath')
BEGIN
	DELETE FROM [dbo].[ServiceDefaultProperties]
	WHERE [PropertyName] = 'ACMESharpVaultProfilesRootPath';
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM [dbo].[Schedule] WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL' )
BEGIN
	DELETE FROM [dbo].[Schedule]
	WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL';
END

IF EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL' )
BEGIN
	DELETE FROM [dbo].[ScheduleTasks]
	WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL';
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL')
BEGIN
	DELETE FROM [dbo].[ScheduleTaskParameters]
	WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL';
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM [dbo].[ScheduleTaskViewConfiguration] WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL' )
BEGIN
	DELETE FROM [dbo].[ScheduleTaskViewConfiguration] 
	WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL';
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [SettingsName] like 'LetsEncryptRenewal%NotificationLetter' )
BEGIN
	DELETE FROM [dbo].[UserSettings]
	WHERE [SettingsName] like 'LetsEncryptRenewal%NotificationLetter';
END
GO


ALTER PROCEDURE [dbo].[AddPFX]
(
	@ActorID int,
	@PackageID int,
	@UserID int,
	@WebSiteID int,
	@FriendlyName nvarchar(255),
	@HostName nvarchar(255),
	@CSRLength int,
	@DistinguishedName nvarchar(500),
	@SerialNumber nvarchar(250),
	@ValidFrom datetime,
	@ExpiryDate datetime

)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- insert record
INSERT INTO [dbo].[SSLCertificates]
	([UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], [CSRLength], [SerialNumber], [ValidFrom], [ExpiryDate], [Installed])
VALUES
	(@UserID, @WebSiteID, @FriendlyName, @HostName, @DistinguishedName, @CSRLength, @SerialNumber, @ValidFrom, @ExpiryDate, 1)

RETURN
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


IF OBJECTPROPERTY(object_id('dbo.GetAllSslCertificatesToRenew'), N'IsProcedure') = 1
DROP PROCEDURE [dbo].[GetAllSslCertificatesToRenew]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECTPROPERTY(object_id('dbo.GetAllTrustedSites'), N'IsProcedure') = 1
DROP PROCEDURE [dbo].[GetAllTrustedSites]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetCertificatesForSite]
(
	@ActorID int,
	@PackageID int,
	@websiteid int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

SELECT
	[ID], [UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName],
	[CSR], [CSRLength], [ValidFrom], [ExpiryDate], [Installed], [IsRenewal],
	[PreviousId], [SerialNumber]
FROM
	[dbo].[SSLCertificates]
WHERE
	[SiteID] = @websiteid
RETURN


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetSiteCert]
(
	@ActorID int,
	@ID int
)
AS

SELECT
	[UserID], [SiteID], [Hostname], [CSR], [Certificate], [Hash], [Installed], [IsRenewal]
FROM
	[dbo].[SSLCertificates]
INNER JOIN
	[dbo].[ServiceItems] AS [SI] ON [SSLCertificates].[SiteID] = [SI].[ItemID]
WHERE
	[SiteID] = @ID AND [Installed] = 1 AND [dbo].CheckActorPackageRights(@ActorID, [SI].[PackageID]) = 1
RETURN

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


ALTER PROCEDURE [dbo].[GetSSLCertificateByID]
(
	@ActorID int,
	@ID int
)
AS

SELECT
	[ID], [UserID], [SiteID], [Hostname], [FriendlyName], [CSR], [Certificate], [Hash], [Installed], [IsRenewal], [PreviousId]
FROM
	[dbo].[SSLCertificates]
INNER JOIN
	[dbo].[ServiceItems] AS [SI] ON [SSLCertificates].[SiteID] = [SI].[ItemID]
WHERE
	[ID] = @ID AND [dbo].CheckActorPackageRights(@ActorID, [SI].[PackageID]) = 1

RETURN


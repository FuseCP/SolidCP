USE [${install.database}]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[AddAuditLogRecord]
(
	@RecordID varchar(32),
	@SeverityID int,
	@UserID int,
	@PackageID int,
	@Username nvarchar(50),
	@ItemID int,
	@StartDate datetime,
	@FinishDate datetime,
	@SourceName varchar(50),
	@TaskName varchar(50),
	@ItemName nvarchar(50),
	@ExecutionLog ntext
)
AS

IF @ItemID = 0 SET @ItemID = NULL
IF @UserID = 0 OR @UserID = -1 SET @UserID = NULL


INSERT INTO AuditLog
(
	RecordID,
	SeverityID,
	UserID,
	PackageID,
	Username,
	ItemID,
	SourceName,
	StartDate,
	FinishDate,
	TaskName,
	ItemName,
	ExecutionLog
)
VALUES
(
	@RecordID,
	@SeverityID,
	@UserID,
	@PackageID,
	@Username,
	@ItemID,
	@SourceName,
	@StartDate,
	@FinishDate,
	@TaskName,
	@ItemName,
	@ExecutionLog
)
RETURN





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













CREATE PROCEDURE [dbo].[AddBlackBerryUser]
	@AccountID int
AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO
	dbo.BlackBerryUsers
	(

	 AccountID,
	 CreatedDate,
	 ModifiedDate)
VALUES
(
	@AccountID,
	getdate(),
	getdate()
)
END



















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE AddCluster
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddComment
(
	@ActorID int,
	@ItemTypeID varchar(50),
	@ItemID int,
	@CommentText nvarchar(1000),
	@SeverityID int
)
AS
INSERT INTO Comments
(
	ItemTypeID,
	ItemID,
	UserID,
	CreatedDate,
	CommentText,
	SeverityID
)
VALUES
(
	@ItemTypeID,
	@ItemID,
	@ActorID,
	GETDATE(),
	@CommentText,
	@SeverityID
)
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE AddDomain
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
	@IsInstantAlias bit,
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
	IsInstantAlias,
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
	@IsInstantAlias,
	@IsDomainPointer
)

SET @DomainID = SCOPE_IDENTITY()
RETURN















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




/****** Object:  Table [dbo].[AddExchangeAccount]    ******/
CREATE PROCEDURE [dbo].[AddExchangeAccount]
(
	@AccountID int OUTPUT,
	@ItemID int,
	@AccountType int,
	@AccountName nvarchar(300),
	@DisplayName nvarchar(300),
	@PrimaryEmailAddress nvarchar(300),
	@MailEnabledPublicFolder bit,
	@MailboxManagerActions varchar(200),
	@SamAccountName nvarchar(100),
	@AccountPassword nvarchar(200),
	@MailboxPlanId int,
	@SubscriberNumber nvarchar(32)
)
AS

INSERT INTO ExchangeAccounts
(
	ItemID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder,
	MailboxManagerActions,
	SamAccountName,
	AccountPassword,
	MailboxPlanId,
	SubscriberNumber,
	UserPrincipalName
)
VALUES
(
	@ItemID,
	@AccountType,
	@AccountName,
	@DisplayName,
	@PrimaryEmailAddress,
	@MailEnabledPublicFolder,
	@MailboxManagerActions,
	@SamAccountName,
	@AccountPassword,
	@MailboxPlanId,
	@SubscriberNumber,
	@PrimaryEmailAddress
)

SET @AccountID = SCOPE_IDENTITY()

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddExchangeAccountEmailAddress
(
	@AccountID int,
	@EmailAddress nvarchar(300)
)
AS
INSERT INTO ExchangeAccountEmailAddresses
(
	AccountID,
	EmailAddress
)
VALUES
(
	@AccountID,
	@EmailAddress
)
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[AddExchangeMailboxPlan]
(
	@MailboxPlanId int OUTPUT,
	@ItemID int,
	@MailboxPlan	nvarchar(300),
	@EnableActiveSync bit,
	@EnableIMAP bit,
	@EnableMAPI bit,
	@EnableOWA bit,
	@EnablePOP bit,
	@IsDefault bit,
	@IssueWarningPct int,
	@KeepDeletedItemsDays int,
	@MailboxSizeMB int,
	@MaxReceiveMessageSizeKB int,
	@MaxRecipients int,
	@MaxSendMessageSizeKB int,
	@ProhibitSendPct int,
	@ProhibitSendReceivePct int	,
	@HideFromAddressBook bit,
	@MailboxPlanType int
)
AS

IF (((SELECT Count(*) FROM ExchangeMailboxPlans WHERE ItemId = @ItemID) = 0) AND (@MailboxPlanType=0))
BEGIN
	SET @IsDefault = 1
END
ELSE
BEGIN
	IF ((@IsDefault = 1) AND (@MailboxPlanType=0))
	BEGIN
		UPDATE ExchangeMailboxPlans SET IsDefault = 0 WHERE ItemID = @ItemID
	END
END

INSERT INTO ExchangeMailboxPlans
(
	ItemID,
	MailboxPlan,
	EnableActiveSync,
	EnableIMAP,
	EnableMAPI,
	EnableOWA,
	EnablePOP,
	IsDefault,
	IssueWarningPct,
	KeepDeletedItemsDays,
	MailboxSizeMB,
	MaxReceiveMessageSizeKB,
	MaxRecipients,
	MaxSendMessageSizeKB,
	ProhibitSendPct,
	ProhibitSendReceivePct,
	HideFromAddressBook,
	MailboxPlanType
)
VALUES
(
	@ItemID,
	@MailboxPlan,
	@EnableActiveSync,
	@EnableIMAP,
	@EnableMAPI,
	@EnableOWA,
	@EnablePOP,
	@IsDefault,
	@IssueWarningPct,
	@KeepDeletedItemsDays,
	@MailboxSizeMB,
	@MaxReceiveMessageSizeKB,
	@MaxRecipients,
	@MaxSendMessageSizeKB,
	@ProhibitSendPct,
	@ProhibitSendReceivePct,
	@HideFromAddressBook,
	@MailboxPlanType
)

SET @MailboxPlanId = SCOPE_IDENTITY()

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[AddExchangeOrganization]
(
	@ItemID int,
	@OrganizationID nvarchar(128)
)
AS

IF NOT EXISTS(SELECT * FROM ExchangeOrganizations WHERE OrganizationID = @OrganizationID)
BEGIN
	INSERT INTO ExchangeOrganizations
	(ItemID, OrganizationID)
	VALUES
	(@ItemID, @OrganizationID)
END

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddExchangeOrganizationDomain
(
	@ItemID int,
	@DomainID int,
	@IsHost bit
)
AS
INSERT INTO ExchangeOrganizationDomains
(ItemID, DomainID, IsHost)
VALUES
(@ItemID, @DomainID, @IsHost)
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[AddIPAddress]
(
	@AddressID int OUTPUT,
	@ServerID int,
	@ExternalIP varchar(24),
	@InternalIP varchar(24),
	@PoolID int,
	@SubnetMask varchar(15),
	@DefaultGateway varchar(15),
	@Comments ntext
)
AS
BEGIN
	IF @ServerID = 0
	SET @ServerID = NULL

	INSERT INTO IPAddresses (ServerID, ExternalIP, InternalIP, PoolID, SubnetMask, DefaultGateway, Comments)
	VALUES (@ServerID, @ExternalIP, @InternalIP, @PoolID, @SubnetMask, @DefaultGateway, @Comments)

	SET @AddressID = SCOPE_IDENTITY()

	RETURN
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[AddItemIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PackageAddressID int
)
AS
BEGIN
	UPDATE PackageIPAddresses
	SET
		ItemID = @ItemID,
		IsPrimary = 0
	FROM PackageIPAddresses AS PIP
	WHERE
		PIP.PackageAddressID = @PackageAddressID
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[AddItemPrivateIPAddress]
(
	@ActorID int,
	@ItemID int,
	@IPAddress varchar(15)
)
AS


IF EXISTS (SELECT ItemID FROM ServiceItems AS SI WHERE dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1)
BEGIN

	INSERT INTO PrivateIPAddresses
	(
		ItemID,
		IPAddress,
		IsPrimary
	)
	VALUES
	(
		@ItemID,
		@IPAddress,
		0 -- not primary
	)

END

RETURN








GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO





CREATE PROCEDURE [dbo].[AddLyncUser]
	@AccountID int,
	@LyncUserPlanID int,
	@SipAddress nvarchar(300)
AS
INSERT INTO
	dbo.LyncUsers
	(AccountID,
	 LyncUserPlanID,
	 CreatedDate,
	 ModifiedDate,
	 SipAddress)
VALUES
(
	@AccountID,
	@LyncUserPlanID,
	getdate(),
	getdate(),
	@SipAddress
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO









CREATE PROCEDURE [dbo].[AddLyncUserPlan]
(
	@LyncUserPlanId int OUTPUT,
	@ItemID int,
	@LyncUserPlanName	nvarchar(300),
	@LyncUserPlanType int,
	@IM bit,
	@Mobility bit,
	@MobilityEnableOutsideVoice bit,
	@Federation bit,
	@Conferencing bit,
	@EnterpriseVoice bit,
	@VoicePolicy int,
	@IsDefault bit
)
AS

IF (((SELECT Count(*) FROM LyncUserPlans WHERE ItemId = @ItemID) = 0) AND (@LyncUserPlanType=0))
BEGIN
	SET @IsDefault = 1
END
ELSE
BEGIN
	IF ((@IsDefault = 1) AND (@LyncUserPlanType=0))
	BEGIN
		UPDATE LyncUserPlans SET IsDefault = 0 WHERE ItemID = @ItemID
	END
END


INSERT INTO LyncUserPlans
(
	ItemID,
	LyncUserPlanName,
	LyncUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault
)
VALUES
(
	@ItemID,
	@LyncUserPlanName,
	@LyncUserPlanType,
	@IM,
	@Mobility,
	@MobilityEnableOutsideVoice,
	@Federation,
	@Conferencing,
	@EnterpriseVoice,
	@VoicePolicy,
	@IsDefault
)

SET @LyncUserPlanId = SCOPE_IDENTITY()

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[AddSfBUser]
	@AccountID int,
	@SfBUserPlanID int,
	@SipAddress nvarchar(300)
AS
INSERT INTO
	dbo.SfBUsers
	(AccountID,
	 SfBUserPlanID,
	 CreatedDate,
	 ModifiedDate,
	 SipAddress)
VALUES
(
	@AccountID,
	@SfBUserPlanID,
	getdate(),
	getdate(),
	@SipAddress
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO









CREATE PROCEDURE [dbo].[AddSfBUserPlan]
(
	@SfBUserPlanId int OUTPUT,
	@ItemID int,
	@SfBUserPlanName	nvarchar(300),
	@SfBUserPlanType int,
	@IM bit,
	@Mobility bit,
	@MobilityEnableOutsideVoice bit,
	@Federation bit,
	@Conferencing bit,
	@EnterpriseVoice bit,
	@VoicePolicy int,
	@IsDefault bit,
	@RemoteUserAccess bit,
	@PublicIMConnectivity bit,
	@AllowOrganizeMeetingsWithExternalAnonymous bit,
	@Telephony int,
	@ServerURI nvarchar(300),
	@ArchivePolicy  nvarchar(300),
	@TelephonyDialPlanPolicy nvarchar(300),
	@TelephonyVoicePolicy nvarchar(300)

)
AS

IF (((SELECT Count(*) FROM SfBUserPlans WHERE ItemId = @ItemID) = 0) AND (@SfBUserPlanType=0))
BEGIN
	SET @IsDefault = 1
END
ELSE
BEGIN
	IF ((@IsDefault = 1) AND (@SfBUserPlanType=0))
	BEGIN
		UPDATE SfBUserPlans SET IsDefault = 0 WHERE ItemID = @ItemID
	END
END

INSERT INTO SfBUserPlans
(
	ItemID,
	SfBUserPlanName,
	SfBUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault,
	RemoteUserAccess,
	PublicIMConnectivity,
	AllowOrganizeMeetingsWithExternalAnonymous,
	Telephony,
	ServerURI,
	ArchivePolicy,
	TelephonyDialPlanPolicy,
	TelephonyVoicePolicy

)
VALUES
(
	@ItemID,
	@SfBUserPlanName,
	@SfBUserPlanType,
	@IM,
	@Mobility,
	@MobilityEnableOutsideVoice,
	@Federation,
	@Conferencing,
	@EnterpriseVoice,
	@VoicePolicy,
	@IsDefault,
	@RemoteUserAccess,
	@PublicIMConnectivity,
	@AllowOrganizeMeetingsWithExternalAnonymous,
	@Telephony,
	@ServerURI,
	@ArchivePolicy,
	@TelephonyDialPlanPolicy,
	@TelephonyVoicePolicy
)

SET @SfBUserPlanId = SCOPE_IDENTITY()
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddOCSUser]
	@AccountID int,
	@InstanceID nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO
	dbo.OCSUsers
	(

	 AccountID,
     InstanceID,
	 CreatedDate,
	 ModifiedDate)
VALUES
(
	@AccountID,
	@InstanceID,
	getdate(),
	getdate()
)
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[AddPackage]
(
	@ActorID int,
	@PackageID int OUTPUT,
	@UserID int,
	@PackageName nvarchar(300),
	@PackageComments ntext,
	@StatusID int,
	@PlanID int,
	@PurchaseDate datetime
)
AS


DECLARE @ParentPackageID int, @PlanServerID int
SELECT @ParentPackageID = PackageID, @PlanServerID = ServerID FROM HostingPlans
WHERE PlanID = @PlanID

IF @ParentPackageID = 0 OR @ParentPackageID IS NULL
SELECT @ParentPackageID = PackageID FROM Packages
WHERE ParentPackageID IS NULL -- root space

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @ParentPackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1);
	RETURN;
END

BEGIN TRAN
-- insert package
INSERT INTO Packages
(
	ParentPackageID,
	UserID,
	PackageName,
	PackageComments,
	ServerID,
	StatusID,
	PlanID,
	PurchaseDate
)
VALUES
(
	@ParentPackageID,
	@UserID,
	@PackageName,
	@PackageComments,
	@PlanServerID,
	@StatusID,
	@PlanID,
	@PurchaseDate
)

SET @PackageID = SCOPE_IDENTITY()

-- add package to packages cache
INSERT INTO PackagesTreeCache (ParentPackageID, PackageID)
SELECT PackageID, @PackageID FROM dbo.PackageParents(@PackageID)

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN

RETURN









































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE AddPackageAddon
(
	@ActorID int,
	@PackageAddonID int OUTPUT,
	@PackageID int,
	@PlanID int,
	@Quantity int,
	@StatusID int,
	@PurchaseDate datetime,
	@Comments ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

DECLARE @ParentPackageID int
SELECT @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

-- insert record
INSERT INTO PackageAddons
(
	PackageID,
	PlanID,
	PurchaseDate,
	Quantity,
	StatusID,
	Comments
)
VALUES
(
	@PackageID,
	@PlanID,
	@PurchaseDate,
	@Quantity,
	@StatusID,
	@Comments
)

SET @PackageAddonID = SCOPE_IDENTITY()

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[AddPFX]
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
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddSchedule
(
	@ActorID int,
	@ScheduleID int OUTPUT,
	@TaskID nvarchar(100),
	@PackageID int,
	@ScheduleName nvarchar(100),
	@ScheduleTypeID nvarchar(50),
	@Interval int,
	@FromTime datetime,
	@ToTime datetime,
	@StartTime datetime,
	@NextRun datetime,
	@Enabled bit,
	@PriorityID nvarchar(50),
	@HistoriesNumber int,
	@MaxExecutionTime int,
	@WeekMonthDay int,
	@XmlParameters ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- insert record
BEGIN TRAN
INSERT INTO Schedule
(
	TaskID,
	PackageID,
	ScheduleName,
	ScheduleTypeID,
	Interval,
	FromTime,
	ToTime,
	StartTime,
	NextRun,
	Enabled,
	PriorityID,
	HistoriesNumber,
	MaxExecutionTime,
	WeekMonthDay
)
VALUES
(
	@TaskID,
	@PackageID,
	@ScheduleName,
	@ScheduleTypeID,
	@Interval,
	@FromTime,
	@ToTime,
	@StartTime,
	@NextRun,
	@Enabled,
	@PriorityID,
	@HistoriesNumber,
	@MaxExecutionTime,
	@WeekMonthDay
)

SET @ScheduleID = SCOPE_IDENTITY()

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlParameters

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ScheduleParameters
WHERE ScheduleID = @ScheduleID

INSERT INTO ScheduleParameters
(
	ScheduleID,
	ParameterID,
	ParameterValue
)
SELECT
	@ScheduleID,
	ParameterID,
	ParameterValue
FROM OPENXML(@idoc, '/parameters/parameter',1) WITH
(
	ParameterID nvarchar(50) '@id',
	ParameterValue nvarchar(3000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddServer
(
	@ServerID int OUTPUT,
	@ServerName nvarchar(100),
	@ServerUrl nvarchar(100),
	@Password nvarchar(100),
	@Comments ntext,
	@VirtualServer bit,
	@InstantDomainAlias nvarchar(200),
	@PrimaryGroupID int,
	@ADEnabled bit,
	@ADRootDomain nvarchar(200),
	@ADUsername nvarchar(100),
	@ADPassword nvarchar(100),
	@ADAuthenticationType varchar(50)
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
	ADAuthenticationType
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
	@ADAuthenticationType
)

SET @ServerID = SCOPE_IDENTITY()

RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE AddService
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


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

INSERT INTO ServiceItemProperties
(
	ItemID,
	PropertyName,
	PropertyValue
)
SELECT
	@ItemID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(3000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[AddSSLRequest]
(
	@SSLID int OUTPUT,
	@ActorID int,
	@PackageID int,
	@UserID int,
	@WebSiteID int,
	@FriendlyName nvarchar(255),
	@HostName nvarchar(255),
	@CSR ntext,
	@CSRLength int,
	@DistinguishedName nvarchar(500),
	@IsRenewal bit = 0,
	@PreviousId int = NULL

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
	([UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], [CSR], [CSRLength], [IsRenewal], [PreviousId])
VALUES
	(@UserID, @WebSiteID, @FriendlyName, @HostName, @DistinguishedName, @CSR, @CSRLength, @IsRenewal, @PreviousId)

SET @SSLID = SCOPE_IDENTITY()
RETURN





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE AddVirtualServices
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:

<services>
	<service id="16" />
</services>

*/

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
WHERE XS.ServiceID NOT IN (SELECT ServiceID FROM VirtualServices WHERE ServerID = @ServerID)

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[AllocatePackageIPAddresses]
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
	DELETE FROM PackageIPAddresses
	FROM PackageIPAddresses AS PIP
	INNER JOIN OPENXML(@idoc, '/items/item', 1) WITH
	(
		AddressID int '@id'
	) as PV ON PIP.AddressID = PV.AddressID


	-- insert
	INSERT INTO dbo.PackageIPAddresses
	(
		PackageID,
		AddressID
	)
	SELECT
		@PackageID,
		AddressID

	FROM OPENXML(@idoc, '/items/item', 1) WITH
	(
		AddressID int '@id'
	) as PV

	-- remove document
	exec sp_xml_removedocument @idoc

END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE PROCEDURE [dbo].ChangeExchangeAcceptedDomainType
(
	@ItemID int,
	@DomainID int,
	@DomainTypeID int
)
AS
UPDATE ExchangeOrganizationDomains
SET DomainTypeID=@DomainTypeID
WHERE ItemID=ItemID AND DomainID=@DomainID
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































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
SET Password = @Password
WHERE UserID = @UserID

RETURN






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[CheckBlackBerryUserExists]
	@AccountID int
AS
BEGIN
	SELECT
		COUNT(AccountID)
	FROM
		dbo.BlackBerryUsers
	WHERE AccountID = @AccountID
END



















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








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
*/

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

-- check if this is a sub-domain of other domain
-- that is not allowed for 3rd level hosting

DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

-- find sub-domains
DECLARE @DomainUserID int, @HostingAllowed bit
SELECT
	@DomainUserID = P.UserID,
	@HostingAllowed = D.HostingAllowed
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
WHERE CHARINDEX('.' + DomainName, @DomainName) > 0
AND (CHARINDEX('.' + DomainName, @DomainName) + LEN('.' + DomainName)) = LEN(@DomainName) + 1
AND IsDomainPointer = 0

-- this is a domain of other user
IF @UserID <> @DomainUserID AND @HostingAllowed = 0
BEGIN
	SET @Result = -2
	RETURN
END

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[CheckDomainUsedByHostedOrganization]
	@DomainName nvarchar(100),
	@Result int OUTPUT
AS
	SET @Result = 0
	IF EXISTS(SELECT 1 FROM ExchangeAccounts WHERE UserPrincipalName LIKE '%@'+ @DomainName)
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[CheckLyncUserExists]
	@AccountID int
AS
BEGIN
	SELECT
		COUNT(AccountID)
	FROM
		dbo.LyncUsers
	WHERE AccountID = @AccountID
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[CheckSfBUserExists]
	@AccountID int
AS
BEGIN
	SELECT
		COUNT(AccountID)
	FROM
		dbo.SfBUsers
	WHERE AccountID = @AccountID
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[CheckOCSUserExists]
	@AccountID int
AS
BEGIN
	SELECT
		COUNT(AccountID)
	FROM
		dbo.OCSUsers
	WHERE AccountID = @AccountID
END















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE CheckServiceItemExists
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




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE CheckServiceItemExistsInService
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




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckSSL]
(
	@siteID int,
	@Renewal bit = 0,
	@Result int OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
*/

SET @Result = 0 -- OK

-- check if a SSL Certificate is installed for domain
IF EXISTS(SELECT [ID] FROM [dbo].[SSLCertificates] WHERE [SiteID] = @siteID)
BEGIN
	SET @Result = -1
	RETURN
END

--To Do add renewal stuff

RETURN

SET ANSI_NULLS ON




GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckSSLExistsForWebsite]
(
	@siteID int,
	@SerialNumber nvarchar(250),
	@Result bit OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
*/

SET @Result = 0 -- OK

-- check if a SSL Certificate is installed for domain
IF EXISTS(SELECT [ID] FROM [dbo].[SSLCertificates] WHERE [SiteID] = @siteID
--AND SerialNumber=@SerialNumber
)
BEGIN
	SET @Result = 1
	RETURN
END

RETURN

SET ANSI_NULLS ON




GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE CheckUserExists
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




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[CompleteSSLRequest]
(
	@ActorID int,
	@PackageID int,
	@ID int,
	@Certificate ntext,
	@SerialNumber nvarchar(250),
	@Hash ntext,
	@DistinguishedName nvarchar(500),
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
UPDATE
	[dbo].[SSLCertificates]
SET
	[Certificate] = @Certificate,
	[Installed] = 1,
	[SerialNumber] = @SerialNumber,
	[DistinguishedName] = @DistinguishedName,
	[Hash] = @Hash,
	[ValidFrom] = @ValidFrom,
	[ExpiryDate] = @ExpiryDate
WHERE
	[ID] = @ID;





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[ConvertToExchangeOrganization]
(
	@ItemID int
)
AS

UPDATE
	[dbo].[ServiceItems]
SET
	[ItemTypeID] = 26
WHERE
	[ItemID] = @ItemID

RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[DeallocatePackageIPAddress]
	@PackageAddressID int
AS
BEGIN

	SET NOCOUNT ON;

	-- check parent package
	DECLARE @ParentPackageID int

	SELECT @ParentPackageID = P.ParentPackageID
	FROM PackageIPAddresses AS PIP
	INNER JOIN Packages AS P ON PIP.PackageID = P.PackageId
	WHERE PIP.PackageAddressID = @PackageAddressID

	IF (@ParentPackageID = 1) -- "System" space
	BEGIN
		DELETE FROM dbo.PackageIPAddresses
		WHERE PackageAddressID = @PackageAddressID
	END
	ELSE -- 2rd level space and below
	BEGIN
		UPDATE PackageIPAddresses
		SET PackageID = @ParentPackageID
		WHERE PackageAddressID = @PackageAddressID
	END

END





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE DeleteAllLogRecords
AS

DELETE FROM Log

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteAuditLogRecords
(
	@ActorID int,
	@UserID int,
	@ItemID int,
	@ItemName nvarchar(100),
	@StartDate datetime,
	@EndDate datetime,
	@SeverityID int,
	@SourceName varchar(100),
	@TaskName varchar(100)
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @IsAdmin bit
SET @IsAdmin = 0
IF EXISTS(SELECT UserID FROM Users WHERE UserID = @ActorID AND RoleID = 1)
SET @IsAdmin = 1

DELETE FROM AuditLog
WHERE (dbo.CheckUserParent(@UserID, UserID) = 1 OR (UserID IS NULL AND @IsAdmin = 1))
AND StartDate BETWEEN @StartDate AND @EndDate
AND ((@SourceName = '') OR (@SourceName <> '' AND SourceName = @SourceName))
AND ((@TaskName = '') OR (@TaskName <> '' AND TaskName = @TaskName))
AND ((@ItemID = 0) OR (@ItemID > 0 AND ItemID = @ItemID))
AND ((@ItemName = '') OR (@ItemName <> '' AND ItemName LIKE @ItemName))

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteAuditLogRecordsComplete
AS

TRUNCATE TABLE AuditLog

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













CREATE PROCEDURE [dbo].[DeleteBlackBerryUser]
(
	@AccountID int
)
AS

DELETE FROM
	BlackBerryUsers
WHERE
	AccountID = @AccountID

RETURN




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[DeleteCertificate]
(
	@ActorID int,
	@PackageID int,
	@id int

)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- insert record
DELETE FROM
	[dbo].[SSLCertificates]
WHERE
	[ID] = @id

RETURN





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE DeleteCluster
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE DeleteComment
(
	@ActorID int,
	@CommentID int
)
AS

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM Comments
WHERE CommentID = @CommentID

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to perform this operation', 16, 1)


-- delete comment
DELETE FROM Comments
WHERE CommentID = @CommentID

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[DeleteCRMOrganization]
	@ItemID int
AS
BEGIN
	SET NOCOUNT ON
DELETE FROM dbo.CRMUsers WHERE AccountID IN (SELECT AccountID FROM dbo.ExchangeAccounts WHERE ItemID = @ItemID)
END

























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE DeleteDnsRecord
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE DeleteDomain
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteExchangeAccount
(
	@ItemID int,
	@AccountID int
)
AS

-- delete e-mail addresses
DELETE FROM ExchangeAccountEmailAddresses
WHERE AccountID = @AccountID

-- delete account
DELETE FROM ExchangeAccounts
WHERE ItemID = @ItemID AND AccountID = @AccountID

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteExchangeAccountEmailAddress
(
	@AccountID int,
	@EmailAddress nvarchar(300)
)
AS
DELETE FROM ExchangeAccountEmailAddresses
WHERE AccountID = @AccountID AND EmailAddress = @EmailAddress
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO













CREATE PROCEDURE [dbo].[DeleteExchangeMailboxPlan]
(
	@MailboxPlanId int
)
AS

-- delete mailboxplan
DELETE FROM ExchangeMailboxPlans
WHERE MailboxPlanId = @MailboxPlanId

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeleteExchangeOrganization]
(
	@ItemID int
)
AS
BEGIN TRAN
	DELETE FROM ExchangeMailboxPlans WHERE ItemID = @ItemID
	DELETE FROM ExchangeOrganizations WHERE ItemID = @ItemID
COMMIT TRAN
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteExchangeOrganizationDomain
(
	@ItemID int,
	@DomainID int
)
AS
DELETE FROM ExchangeOrganizationDomains
WHERE DomainID = @DomainID AND ItemID = @ItemID
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE DeleteHostingPlan
(
	@ActorID int,
	@PlanID int,
	@Result int OUTPUT
)
AS
SET @Result = 0

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM HostingPlans
WHERE PlanID = @PlanID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- check if some packages uses this plan
IF EXISTS (SELECT PackageID FROM Packages WHERE PlanID = @PlanID)
BEGIN
	SET @Result = -1
	RETURN
END

-- check if some package addons uses this plan
IF EXISTS (SELECT PackageID FROM PackageAddons WHERE PlanID = @PlanID)
BEGIN
	SET @Result = -2
	RETURN
END

-- delete hosting plan
DELETE FROM HostingPlans
WHERE PlanID = @PlanID

RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















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





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeleteItemIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PackageAddressID int
)
AS
BEGIN
	UPDATE PackageIPAddresses
	SET
		ItemID = NULL,
		IsPrimary = 0
	FROM PackageIPAddresses AS PIP
	WHERE
		PIP.PackageAddressID = @PackageAddressID
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeleteItemIPAddresses]
(
	@ActorID int,
	@ItemID int
)
AS
BEGIN
	UPDATE PackageIPAddresses
	SET
		ItemID = NULL,
		IsPrimary = 0
	FROM PackageIPAddresses AS PIP
	WHERE
		PIP.ItemID = @ItemID
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE DeleteItemPrivateIPAddress
(
	@ActorID int,
	@ItemID int,
	@PrivateAddressID int
)
AS
BEGIN
	DELETE FROM PrivateIPAddresses
	FROM PrivateIPAddresses AS PIP
	INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
	WHERE PIP.PrivateAddressID = @PrivateAddressID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE DeleteItemPrivateIPAddresses
(
	@ActorID int,
	@ItemID int
)
AS
BEGIN
	DELETE FROM PrivateIPAddresses
	FROM PrivateIPAddresses AS PIP
	INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
	WHERE PIP.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END








GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[DeleteLyncUser]
(
	@AccountId int
)
AS

DELETE FROM
	LyncUsers
WHERE
	AccountId = @AccountId

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[DeleteLyncUserPlan]
(
	@LyncUserPlanId int
)
AS

-- delete lyncuserplan
DELETE FROM LyncUserPlans
WHERE LyncUserPlanId = @LyncUserPlanId

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DeleteSfBUser]
(
	@AccountId int
)
AS

DELETE FROM
	LyncUsers
WHERE
	AccountId = @AccountId

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[DeleteSfBUserPlan]
(
	@SfBUserPlanId int
)
AS

-- delete sfbuserplan
DELETE FROM SfBUserPlans
WHERE SfBUserPlanId = @SfBUserPlanId

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DeleteOCSUser]
(
	@InstanceId nvarchar(50)
)
AS

DELETE FROM
	OCSUsers
WHERE
	InstanceId = @InstanceId

RETURN








GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE PROCEDURE [dbo].[DeleteOrganizationUsers]
	@ItemID int
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM ExchangeAccounts WHERE ItemID = @ItemID
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeletePackage]
(
	@ActorID int,
	@PackageID int
)
AS
BEGIN
	-- check rights
	IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
	RAISERROR('You are not allowed to access this package', 16, 1)

	BEGIN TRAN

	-- remove package from cache
	DELETE FROM PackagesTreeCache
	WHERE
		ParentPackageID = @PackageID OR
		PackageID = @PackageID

	-- delete package comments
	DELETE FROM Comments
	WHERE ItemID = @PackageID AND ItemTypeID = 'PACKAGE'

	-- delete diskspace
	DELETE FROM PackagesDiskspace
	WHERE PackageID = @PackageID

	-- delete bandwidth
	DELETE FROM PackagesBandwidth
	WHERE PackageID = @PackageID

	-- delete settings
	DELETE FROM PackageSettings
	WHERE PackageID = @PackageID

	-- delete domains
	DELETE FROM Domains
	WHERE PackageID = @PackageID

	-- delete package IP addresses
	DELETE FROM PackageIPAddresses
	WHERE PackageID = @PackageID

	-- delete service items
	DELETE FROM ServiceItems
	WHERE PackageID = @PackageID

	-- delete global DNS records
	DELETE FROM GlobalDnsRecords
	WHERE PackageID = @PackageID

	-- delete package services
	DELETE FROM PackageServices
	WHERE PackageID = @PackageID

	-- delete package quotas
	DELETE FROM PackageQuotas
	WHERE PackageID = @PackageID

	-- delete package resources
	DELETE FROM PackageResources
	WHERE PackageID = @PackageID

	-- delete package
	DELETE FROM Packages
	WHERE PackageID = @PackageID

	COMMIT TRAN
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE DeletePackageAddon
(
	@ActorID int,
	@PackageAddonID int
)
AS

DECLARE @PackageID int
SELECT @PackageID = PackageID FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- delete record
DELETE FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteSchedule
(
	@ActorID int,
	@ScheduleID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Schedule
WHERE ScheduleID = @ScheduleID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN
-- delete schedule parameters
DELETE FROM ScheduleParameters
WHERE ScheduleID = @ScheduleID

-- delete schedule
DELETE FROM Schedule
WHERE ScheduleID = @ScheduleID

COMMIT TRAN

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteServer
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE DeleteService
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














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




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






















-- =============================================
-- Description:	Delete user email addresses except primary email
-- =============================================
CREATE PROCEDURE [dbo].[DeleteUserEmailAddresses]
	@AccountId int,
	@PrimaryEmailAddress nvarchar(300)
AS
BEGIN

DELETE FROM
	ExchangeAccountEmailAddresses
WHERE
	AccountID = @AccountID AND LOWER(EmailAddress) <> LOWER(@PrimaryEmailAddress)
END





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE PROCEDURE [dbo].[DeleteVirtualServices]
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:

<services>
	<service id="16" />
</services>

*/

BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- update HP resources
DELETE FROM VirtualServices
WHERE ServiceID IN (
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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[DistributePackageServices]
(
	@ActorID int,
	@PackageID int
)
AS

-- get primary distribution group
DECLARE @PrimaryGroupID int
DECLARE @VirtualServer bit
DECLARE @PlanID int
DECLARE @ServerID int
SELECT
	@PrimaryGroupID = ISNULL(S.PrimaryGroupID, 0),
	@VirtualServer = S.VirtualServer,
	@PlanID = P.PlanID,
	@ServerID = P.ServerID
FROM Packages AS P
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
WHERE P.PackageID = @PackageID


-- get the list of available groups from hosting plan
DECLARE @Groups TABLE
(
	GroupID int,
	PrimaryGroup bit
)

INSERT INTO @Groups (GroupID, PrimaryGroup)
SELECT
	RG.GroupID,
	CASE WHEN RG.GroupID = @PrimaryGroupID THEN 1 -- mark primary group
	ELSE 0
	END
FROM ResourceGroups AS RG
WHERE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, NULL) = 1
AND RG.GroupID NOT IN
(
	SELECT P.GroupID
	FROM PackageServices AS PS
	INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
	INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
	WHERE PS.PackageID = @PackageID
)

IF @VirtualServer <> 1
BEGIN
	-- PHYSICAL SERVER
	-- just return the list of services based on the plan
	INSERT INTO PackageServices (PackageID, ServiceID)
	SELECT
		@PackageID,
		S.ServiceID
	FROM Services AS S
	INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
	INNER JOIN @Groups AS G ON P.GroupID = G.GroupID
	WHERE S.ServerID = @ServerID
		AND S.ServiceID NOT IN (SELECT ServiceID FROM PackageServices WHERE PackageID = @PackageID)
END
ELSE
BEGIN
	-- VIRTUAL SERVER

	DECLARE @GroupID int, @PrimaryGroup int
	DECLARE GroupsCursor CURSOR FOR
	SELECT GroupID, PrimaryGroup FROM @Groups
	ORDER BY PrimaryGroup DESC

	OPEN GroupsCursor

	WHILE (10 = 10)
	BEGIN    --LOOP 10: thru groups
		FETCH NEXT FROM GroupsCursor
		INTO @GroupID, @PrimaryGroup

		IF (@@fetch_status <> 0)
		BEGIN
			DEALLOCATE GroupsCursor
			BREAK
		END

		-- read group information
		DECLARE @DistributionType int, @BindDistributionToPrimary int
		SELECT
			@DistributionType = DistributionType,
			@BindDistributionToPrimary = BindDistributionToPrimary
		FROM VirtualGroups AS VG
		WHERE ServerID = @ServerID AND GroupID = @GroupID

		-- bind distribution to primary
		IF @BindDistributionToPrimary = 1 AND @PrimaryGroup = 0 AND @PrimaryGroupID <> 0
		BEGIN
			-- if only one service found just use it and do not distribute
			IF (SELECT COUNT(*) FROM VirtualServices AS VS
				INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
				INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
				WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID) = 1
				BEGIN
					INSERT INTO PackageServices (PackageID, ServiceID)
					SELECT
						@PackageID,
						VS.ServiceID
					FROM VirtualServices AS VS
					INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
					INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
					WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID
				END
			ELSE
				BEGIN
					DECLARE @PrimaryServerID int
					-- try to get primary distribution server
					SELECT
						@PrimaryServerID = S.ServerID
					FROM PackageServices AS PS
					INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
					INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
					WHERE PS.PackageID = @PackageID AND P.GroupID = @PrimaryGroupID

					INSERT INTO PackageServices (PackageID, ServiceID)
					SELECT
						@PackageID,
						VS.ServiceID
					FROM VirtualServices AS VS
					INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
					INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
					WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID AND S.ServerID = @PrimaryServerID
				END
		END
		ELSE
		BEGIN

			-- DISTRIBUTION
			DECLARE @Services TABLE
			(
				ServiceID int,
				ItemsNumber int,
				RandomNumber int
			)

			DELETE FROM @Services

			INSERT INTO @Services (ServiceID, ItemsNumber, RandomNumber)
			SELECT
				VS.ServiceID,
				(SELECT COUNT(ItemID) FROM ServiceItems WHERE ServiceID = VS.ServiceID),
				RAND()
			FROM VirtualServices AS VS
			INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
			INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
			WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID

			-- BALANCED DISTRIBUTION
			IF @DistributionType = 1
			BEGIN
				-- get the less allocated service
				INSERT INTO PackageServices (PackageID, ServiceID)
				SELECT TOP 1
					@PackageID,
					ServiceID
				FROM @Services
				ORDER BY ItemsNumber
			END
			ELSE
			-- RANDOMIZED DISTRIBUTION
			BEGIN
				-- get the less allocated service
				INSERT INTO PackageServices (PackageID, ServiceID)
				SELECT TOP 1
					@PackageID,
					ServiceID
				FROM @Services
				ORDER BY RandomNumber
			END
		END

		IF @PrimaryGroup = 1
		SET @PrimaryGroupID = @GroupID

	END -- while groups

END -- end virtual server

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ExchangeAccountEmailAddressExists]
(
	@EmailAddress nvarchar(300),
	@Exists bit OUTPUT
)
AS

	SET @Exists = 0
	IF EXISTS(SELECT * FROM [dbo].[ExchangeAccountEmailAddresses] WHERE [EmailAddress] = @EmailAddress)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [PrimaryEmailAddress] = @EmailAddress AND [AccountType] <> 2)
		BEGIN
			SET @Exists = 1
		END

	RETURN




GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








CREATE PROCEDURE [dbo].[ExchangeAccountExists]
(
	@AccountName nvarchar(20),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeAccounts WHERE sAMAccountName LIKE '%\'+@AccountName)
BEGIN
	SET @Exists = 1
END

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE ExchangeOrganizationDomainExists
(
	@DomainID int,
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeOrganizationDomains WHERE DomainID = @DomainID)
BEGIN
	SET @Exists = 1
END
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE ExchangeOrganizationExists
(
	@OrganizationID nvarchar(10),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeOrganizations WHERE OrganizationID = @OrganizationID)
BEGIN
	SET @Exists = 1
END

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































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






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetAuditLogRecord]
(
	@RecordID varchar(32)
)
AS

SELECT
	L.RecordID,
    L.SeverityID,
    L.StartDate,
    L.FinishDate,
    L.ItemID,
    L.SourceName,
    L.TaskName,
    L.ItemName,
    L.ExecutionLog,

    ISNULL(L.UserID, 0) AS UserID,
	L.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	ISNULL(U.RoleID, 0) AS RoleID,
	U.Email
FROM AuditLog AS L
LEFT OUTER JOIN UsersDetailed AS U ON L.UserID = U.UserID
WHERE RecordID = @RecordID
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetAuditLogRecordsPaged]
(
	@ActorID int,
	@UserID int,
	@PackageID int,
	@ItemID int,
	@ItemName nvarchar(100),
	@StartDate datetime,
	@EndDate datetime,
	@SeverityID int,
	@SourceName varchar(100),
	@TaskName varchar(100),
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

IF @SourceName IS NULL SET @SourceName = ''
IF @TaskName IS NULL SET @TaskName = ''
IF @ItemName IS NULL SET @ItemName = ''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'L.StartDate DESC'

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
DECLARE @IsAdmin bit
SET @IsAdmin = 0
IF EXISTS(SELECT UserID FROM Users WHERE UserID = @ActorID AND RoleID = 1)
SET @IsAdmin = 1

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Records TABLE
(
	ItemPosition int IDENTITY(1,1),
	RecordID varchar(32)
)
INSERT INTO @Records (RecordID)
SELECT
	L.RecordID
FROM AuditLog AS L
WHERE
((@PackageID = 0 AND dbo.CheckUserParent(@UserID, L.UserID) = 1 OR (L.UserID IS NULL AND @IsAdmin = 1))
	OR (@PackageID > 0 AND L.PackageID = @PackageID))
AND L.StartDate BETWEEN @StartDate AND @EndDate
AND ((@SourceName = '''') OR (@SourceName <> '''' AND L.SourceName = @SourceName))
AND ((@TaskName = '''') OR (@TaskName <> '''' AND L.TaskName = @TaskName))
AND ((@ItemID = 0) OR (@ItemID > 0 AND L.ItemID = @ItemID))
AND ((@ItemName = '''') OR (@ItemName <> '''' AND L.ItemName LIKE @ItemName))
AND ((@SeverityID = -1) OR (@SeverityID > -1 AND L.SeverityID = @SeverityID)) '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(RecordID) FROM @Records;
SELECT
	TL.RecordID,
    L.SeverityID,
    L.StartDate,
    L.FinishDate,
    L.ItemID,
    L.SourceName,
    L.TaskName,
    L.ItemName,
    L.ExecutionLog,

    ISNULL(L.UserID, 0) AS UserID,
	L.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	ISNULL(U.RoleID, 0) AS RoleID,
	U.Email,
	CASE U.IsPeer
		WHEN 1 THEN U.OwnerID
		ELSE U.UserID
	END EffectiveUserID
FROM @Records AS TL
INNER JOIN AuditLog AS L ON TL.RecordID = L.RecordID
LEFT OUTER JOIN UsersDetailed AS U ON L.UserID = U.UserID
WHERE TL.ItemPosition BETWEEN @StartRow + 1 AND @EndRow'

exec sp_executesql @sql, N'@TaskName varchar(100), @SourceName varchar(100), @PackageID int, @ItemID int, @ItemName nvarchar(100), @StartDate datetime,
@EndDate datetime, @StartRow int, @MaximumRows int, @UserID int, @ActorID int, @SeverityID int',
@TaskName, @SourceName, @PackageID, @ItemID, @ItemName, @StartDate, @EndDate, @StartRow, @MaximumRows, @UserID, @ActorID,
@SeverityID


RETURN





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetAuditLogSources
AS

SELECT SourceName FROM AuditLogSources

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetAuditLogTasks
(
	@SourceName varchar(100)
)
AS

IF @SourceName = '' SET @SourceName = NULL

SELECT SourceName, TaskName FROM AuditLogTasks
WHERE (@SourceName = NULL OR @SourceName IS NOT NULL AND SourceName = @SourceName)

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetAvailableVirtualServices
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










CREATE PROCEDURE [dbo].[GetBlackBerryUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@Name nvarchar(400),
	@Email nvarchar(400),
	@StartRow int,
	@Count int
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

CREATE TABLE #TempBlackBerryUsers
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](300) NOT NULL,
	[DisplayName] [nvarchar](300) NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL
)


IF (@SortColumn = 'DisplayName')
BEGIN
	INSERT INTO
		#TempBlackBerryUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		BlackBerryUsers bu
	ON
		ea.AccountID = bu.AccountID
	WHERE
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email
	ORDER BY
		ea.DisplayName
END
ELSE
BEGIN
	INSERT INTO
		#TempBlackBerryUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		BlackBerryUsers bu
	ON
		ea.AccountID = bu.AccountID
	WHERE
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email
	ORDER BY
		ea.PrimaryEmailAddress
END

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempBlackBerryUsers

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempBlackBerryUsers
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count)
END
ELSE
BEGIN
	IF (@SortColumn = 'DisplayName')
	BEGIN
		SELECT * FROM #TempBlackBerryUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
	END
	ELSE
	BEGIN
		SELECT * FROM #TempBlackBerryUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY PrimaryEmailAddress DESC
	END

END


DROP TABLE #TempBlackBerryUsers

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetBlackBerryUsersCount]
(
	@ItemID int,
	@Name nvarchar(400),
	@Email nvarchar(400)

)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

SELECT
	COUNT(ea.AccountID)
FROM
	ExchangeAccounts ea
INNER JOIN
	BlackBerryUsers bu
ON
	ea.AccountID = bu.AccountID
WHERE
	ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetCertificatesForSite]
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





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetClusters
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetComments
(
	@ActorID int,
	@UserID int,
	@ItemTypeID varchar(50),
	@ItemID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	C.CommentID,
	C.ItemTypeID,
	C.ItemID,
	C.UserID,
	C.CreatedDate,
	C.CommentText,
	C.SeverityID,

	-- user
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM Comments AS C
INNER JOIN UsersDetailed AS U ON C.UserID = U.UserID
WHERE
	ItemTypeID = @ItemTypeID
	AND ItemID = @ItemID
	AND dbo.CheckUserParent(@UserID, C.UserID) = 1
ORDER BY C.CreatedDate ASC
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE PROCEDURE [dbo].[GetCRMOrganizationUsers]
	@ItemID int
AS
BEGIN
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		CRMUsers cu
	ON
		ea.AccountID = cu.AccountID
	WHERE
		ea.ItemID = @ItemID
END























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetCRMUser]
	@AccountID int
AS
BEGIN
	SET NOCOUNT ON;
SELECT
	CRMUserGUID as CRMUserID,
	BusinessUnitID
FROM
	CRMUsers
WHERE
	AccountID = @AccountID
END

























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[GetCRMUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@Name nvarchar(400),
	@Email nvarchar(400),
	@StartRow int,
	@Count int
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

CREATE TABLE #TempCRMUsers
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](300) NOT NULL,
	[DisplayName] [nvarchar](300) NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL
)


IF (@SortColumn = 'DisplayName')
BEGIN
	INSERT INTO
		#TempCRMUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		CRMUsers cu
	ON
		ea.AccountID = cu.AccountID
	WHERE
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email
	ORDER BY
		ea.DisplayName
END
ELSE
BEGIN
	INSERT INTO
		#TempCRMUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		CRMUsers cu
	ON
		ea.AccountID = cu.AccountID
	WHERE
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email
	ORDER BY
		ea.PrimaryEmailAddress
END

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempCRMUsers

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempCRMUsers
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count)
END
ELSE
BEGIN
	IF (@SortColumn = 'DisplayName')
	BEGIN
		SELECT * FROM #TempCRMUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
	END
	ELSE
	BEGIN
		SELECT * FROM #TempCRMUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY PrimaryEmailAddress DESC
	END

END



DROP TABLE #TempCRMUsers

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetCRMUsersCount]
(
	@ItemID int,
	@Name nvarchar(400),
	@Email nvarchar(400)

)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

SELECT
	COUNT(ea.AccountID)
FROM
	ExchangeAccounts ea
INNER JOIN
	CRMUsers cu
ON
	ea.AccountID = cu.AccountID
WHERE
	ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email


























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetDnsRecordsByGroup
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO









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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO





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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



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
	D.IsInstantAlias,
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




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
		D.IsInstantAlias,
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
		D.IsInstantAlias,
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




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
	D.IsInstantAlias,
	D.IsDomainPointer
FROM Domains AS D
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON D.PackageID = PT.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
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
	D.IsInstantAlias,
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




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
	D.IsInstantAlias,
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




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
DECLARE @sql nvarchar(2000)

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
WHERE (D.IsInstantAlias = 0 AND D.IsDomainPointer = 0) AND
		((@Recursive = 0 AND D.PackageID = @PackageID)
		OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, D.PackageID) = 1))
AND (@ServerID = 0 OR (@ServerID > 0 AND S.ServerID = @ServerID))
'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

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
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsInstantAlias,
	D.IsDomainPointer,

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










CREATE PROCEDURE [dbo].[GetExchangeAccount]
(
	@ItemID int,
	@AccountID int
)
AS
SELECT
	E.AccountID,
	E.ItemID,
	E.AccountType,
	E.AccountName,
	E.DisplayName,
	E.PrimaryEmailAddress,
	E.MailEnabledPublicFolder,
	E.MailboxManagerActions,
	E.SamAccountName,
	E.AccountPassword,
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.AccountID = @AccountID
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[GetExchangeAccountByAccountName]
(
	@ItemID int,
	@AccountName nvarchar(300)
)
AS
SELECT
	E.AccountID,
	E.ItemID,
	E.AccountType,
	E.AccountName,
	E.DisplayName,
	E.PrimaryEmailAddress,
	E.MailEnabledPublicFolder,
	E.MailboxManagerActions,
	E.SamAccountName,
	E.AccountPassword,
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.AccountName = @AccountName
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[GetExchangeAccountByMailboxPlanId]
(
	@ItemID int,
	@MailboxPlanId int
)
AS

IF (@MailboxPlanId < 0)
BEGIN
SELECT
	E.AccountID,
	E.ItemID,
	E.AccountType,
	E.AccountName,
	E.DisplayName,
	E.PrimaryEmailAddress,
	E.MailEnabledPublicFolder,
	E.MailboxManagerActions,
	E.SamAccountName,
	E.AccountPassword,
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.MailboxPlanId IS NULL AND
	E.AccountType IN (1,5)
RETURN

END
ELSE
IF (@ItemId = 0)
BEGIN
SELECT
	E.AccountID,
	E.ItemID,
	E.AccountType,
	E.AccountName,
	E.DisplayName,
	E.PrimaryEmailAddress,
	E.MailEnabledPublicFolder,
	E.MailboxManagerActions,
	E.SamAccountName,
	E.AccountPassword,
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId
WHERE
	E.MailboxPlanId = @MailboxPlanId AND
	E.AccountType IN (1,5)
END
ELSE
BEGIN
SELECT
	E.AccountID,
	E.ItemID,
	E.AccountType,
	E.AccountName,
	E.DisplayName,
	E.PrimaryEmailAddress,
	E.MailEnabledPublicFolder,
	E.MailboxManagerActions,
	E.SamAccountName,
	E.AccountPassword,
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.MailboxPlanId = @MailboxPlanId AND
	E.AccountType IN (1,5)
RETURN
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetExchangeAccountEmailAddresses
(
	@AccountID int
)
AS
SELECT
	AddressID,
	AccountID,
	EmailAddress
FROM
	ExchangeAccountEmailAddresses
WHERE
	AccountID = @AccountID
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO












CREATE PROCEDURE [dbo].[GetExchangeAccounts]
(
	@ItemID int,
	@AccountType int
)
AS
SELECT
	E.AccountID,
	E.ItemID,
	E.AccountType,
	E.AccountName,
	E.DisplayName,
	E.PrimaryEmailAddress,
	E.MailEnabledPublicFolder,
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName
FROM
	ExchangeAccounts  AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	(E.AccountType = @AccountType OR @AccountType IS NULL)
ORDER BY DisplayName
RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










CREATE PROCEDURE [dbo].[GetExchangeAccountsPaged]
(
	@ActorID int,
	@ItemID int,
	@AccountTypes nvarchar(30),
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
EA.AccountType IN (' + @AccountTypes + ')
AND EA.ItemID = @ItemID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn = 'PrimaryEmailAddress' AND @AccountTypes <> '2'
	BEGIN
		SET @condition = @condition + ' AND EA.AccountID IN (SELECT EAEA.AccountID FROM ExchangeAccountEmailAddresses EAEA WHERE EAEA.EmailAddress LIKE ''' + @FilterValue + ''')'
	END
	ELSE
	BEGIN
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	END
END

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'EA.DisplayName ASC'

DECLARE @joincondition nvarchar(700)
	SET @joincondition = ',P.MailboxPlan FROM ExchangeAccounts AS EA
	LEFT OUTER JOIN ExchangeMailboxPlans AS P ON EA.MailboxPlanId = P.MailboxPlanId'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(EA.AccountID) FROM ExchangeAccounts AS EA
WHERE ' + @condition + ';

WITH Accounts AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		EA.AccountID,
		EA.ItemID,
		EA.AccountType,
		EA.AccountName,
		EA.DisplayName,
		EA.PrimaryEmailAddress,
		EA.MailEnabledPublicFolder,
		EA.MailboxPlanId,
		EA.SubscriberNumber,
		EA.UserPrincipalName ' + @joincondition +
	' WHERE ' + @condition + '
)

SELECT * FROM Accounts
WHERE Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows
'

print @sql

exec sp_executesql @sql, N'@ItemID int, @StartRow int, @MaximumRows int',
@ItemID, @StartRow, @MaximumRows

RETURN





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO









CREATE PROCEDURE [dbo].[GetExchangeMailboxes]
	@ItemID int
AS
BEGIN
SELECT
	AccountID,
	ItemID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder,
	SubscriberNumber,
	UserPrincipalName
FROM
	ExchangeAccounts
WHERE
	ItemID = @ItemID AND
	(AccountType =1  OR AccountType=5 OR AccountType=6)
ORDER BY 1

END


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[GetExchangeMailboxPlan]
(
	@MailboxPlanId int
)
AS
SELECT
	MailboxPlanId,
	ItemID,
	MailboxPlan,
	EnableActiveSync,
	EnableIMAP,
	EnableMAPI,
	EnableOWA,
	EnablePOP,
	IsDefault,
	IssueWarningPct,
	KeepDeletedItemsDays,
	MailboxSizeMB,
	MaxReceiveMessageSizeKB,
	MaxRecipients,
	MaxSendMessageSizeKB,
	ProhibitSendPct,
	ProhibitSendReceivePct,
	HideFromAddressBook,
	MailboxPlanType
FROM
	ExchangeMailboxPlans
WHERE
	MailboxPlanId = @MailboxPlanId
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








CREATE PROCEDURE [dbo].[GetExchangeMailboxPlans]
(
	@ItemID int
)
AS
SELECT
	MailboxPlanId,
	ItemID,
	MailboxPlan,
	EnableActiveSync,
	EnableIMAP,
	EnableMAPI,
	EnableOWA,
	EnablePOP,
	IsDefault,
	IssueWarningPct,
	KeepDeletedItemsDays,
	MailboxSizeMB,
	MaxReceiveMessageSizeKB,
	MaxRecipients,
	MaxSendMessageSizeKB,
	ProhibitSendPct,
	ProhibitSendReceivePct,
	HideFromAddressBook,
	MailboxPlanType
FROM
	ExchangeMailboxPlans
WHERE
	ItemID = @ItemID
ORDER BY MailboxPlan
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetExchangeOrganization]
(
	@ItemID int
)
AS
SELECT
	ItemID,
	ExchangeMailboxPlanID,
	LyncUserPlanID,
	SfBUserPlanID
FROM
	ExchangeOrganizations
WHERE
	ItemID = @ItemID
RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




CREATE PROCEDURE [dbo].[GetExchangeOrganizationDomains]
(
	@ItemID int
)
AS
SELECT
	ED.DomainID,
	D.DomainName,
	ED.IsHost,
	ED.DomainTypeID
FROM
	ExchangeOrganizationDomains AS ED
INNER JOIN Domains AS D ON ED.DomainID = D.DomainID
WHERE ED.ItemID = @ItemID
RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

















CREATE PROCEDURE [dbo].[GetExchangeOrganizationStatistics]
(
	@ItemID int
)
AS

IF -1 IN (SELECT B.MailboxSizeMB FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID)
BEGIN
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 1 OR AccountType = 5 OR AccountType = 6) AND ItemID = @ItemID) AS CreatedMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 2 AND ItemID = @ItemID) AS CreatedContacts,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 3 AND ItemID = @ItemID) AS CreatedDistributionLists,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 4 AND ItemID = @ItemID) AS CreatedPublicFolders,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains,
	(SELECT MIN(B.MailboxSizeMB) FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID) AS UsedDiskSpace
END
ELSE
BEGIN
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 1 OR AccountType = 5 OR AccountType = 6) AND ItemID = @ItemID) AS CreatedMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 2 AND ItemID = @ItemID) AS CreatedContacts,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 3 AND ItemID = @ItemID) AS CreatedDistributionLists,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 4 AND ItemID = @ItemID) AS CreatedPublicFolders,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains,
	(SELECT SUM(B.MailboxSizeMB) FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID) AS UsedDiskSpace
END


RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetGroupProviders
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetHostingAddons]
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	PlanID,
	UserID,
	PackageID,
	PlanName,
	PlanDescription,
	Available,
	SetupPrice,
	RecurringPrice,
	RecurrenceLength,
	RecurrenceUnit,
	IsAddon,
	(SELECT COUNT(P.PackageID) FROM PackageAddons AS P WHERE P.PlanID = HP.PlanID) AS PackagesNumber
FROM
	HostingPlans AS HP
WHERE
	UserID = @UserID
	AND IsAddon = 1
ORDER BY PlanName
RETURN










































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetHostingPlan
(
	@ActorID int,
	@PlanID int
)
AS

SELECT
	PlanID,
	UserID,
	PackageID,
	ServerID,
	PlanName,
	PlanDescription,
	Available,
	SetupPrice,
	RecurringPrice,
	RecurrenceLength,
	RecurrenceUnit,
	IsAddon
FROM HostingPlans AS HP
WHERE HP.PlanID = @PlanID

RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetHostingPlanQuotas]
(
	@ActorID int,
	@PlanID int,
	@PackageID int,
	@ServerID int
)
AS

-- check rights
IF dbo.CheckActorParentPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @IsAddon bit

IF @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

-- get resource groups
SELECT
	RG.GroupID,
	RG.GroupName,
	CASE
		WHEN HPR.CalculateDiskSpace IS NULL THEN CAST(0 as bit)
		ELSE CAST(1 as bit)
	END AS Enabled,
	dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID) AS ParentEnabled,
	ISNULL(HPR.CalculateDiskSpace, 1) AS CalculateDiskSpace,
	ISNULL(HPR.CalculateBandwidth, 1) AS CalculateBandwidth
FROM ResourceGroups AS RG
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
WHERE (RG.ShowGroup = 1)
ORDER BY RG.GroupOrder

-- get quotas by groups
SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	ISNULL(HPQ.QuotaValue, 0) AS QuotaValue,
	dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) AS ParentQuotaValue
FROM Quotas AS Q
LEFT OUTER JOIN HostingPlanQuotas AS HPQ ON Q.QuotaID = HPQ.QuotaID AND HPQ.PlanID = @PlanID
WHERE Q.HideQuota IS NULL OR Q.HideQuota = 0
ORDER BY Q.QuotaOrder
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetHostingPlans]
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	HP.PlanID,
	HP.UserID,
	HP.PackageID,
	HP.PlanName,
	HP.PlanDescription,
	HP.Available,
	HP.SetupPrice,
	HP.RecurringPrice,
	HP.RecurrenceLength,
	HP.RecurrenceUnit,
	HP.IsAddon,

	(SELECT COUNT(P.PackageID) FROM Packages AS P WHERE P.PlanID = HP.PlanID) AS PackagesNumber,

	-- server
	ISNULL(HP.ServerID, 0) AS ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,

	-- package
	ISNULL(HP.PackageID, 0) AS PackageID,
	ISNULL(P.PackageName, 'None') AS PackageName

FROM HostingPlans AS HP
LEFT OUTER JOIN Servers AS S ON HP.ServerID = S.ServerID
LEFT OUTER JOIN Packages AS P ON HP.PackageID = P.PackageID
WHERE
	HP.UserID = @UserID
	AND HP.IsAddon = 0
ORDER BY HP.PlanName
RETURN











































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[GetInstanceID]
	 @AccountID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT InstanceID FROM OCSUsers WHERE AccountID = @AccountID
END














GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














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
		Comments
	FROM IPAddresses
	WHERE
		AddressID = @AddressID

	RETURN
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














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




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














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

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

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




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[GetItemIdByOrganizationId]
	@OrganizationId nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		ItemID
	FROM
		dbo.ExchangeOrganizations
	WHERE
		OrganizationId = @OrganizationId
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE [dbo].[GetItemIPAddresses]
(
	@ActorID int,
	@ItemID int,
	@PoolID int
)
AS

SELECT
	PIP.PackageAddressID AS AddressID,
	IP.ExternalIP AS IPAddress,
	IP.InternalIP AS NATAddress,
	IP.SubnetMask,
	IP.DefaultGateway,
	PIP.IsPrimary
FROM PackageIPAddresses AS PIP
INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
WHERE PIP.ItemID = @ItemID
AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
ORDER BY PIP.IsPrimary DESC

RETURN

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetItemPrivateIPAddresses]
(
	@ActorID int,
	@ItemID int
)
AS

SELECT
	PIP.PrivateAddressID AS AddressID,
	PIP.IPAddress,
	PIP.IsPrimary
FROM PrivateIPAddresses AS PIP
INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
WHERE PIP.ItemID = @ItemID
AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
ORDER BY PIP.IsPrimary DESC

RETURN








GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO





CREATE PROCEDURE [dbo].[GetLyncUserPlan]
(
	@LyncUserPlanId int
)
AS
SELECT
	LyncUserPlanId,
	ItemID,
	LyncUserPlanName,
	LyncUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault
FROM
	LyncUserPlans
WHERE
	LyncUserPlanId = @LyncUserPlanId
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO







CREATE PROCEDURE [dbo].[GetLyncUserPlanByAccountId]
(
	@AccountID int
)
AS
SELECT
	LyncUserPlanId,
	ItemID,
	LyncUserPlanName,
	LyncUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault
FROM
	LyncUserPlans
WHERE
	LyncUserPlanId IN (SELECT LyncUserPlanId FROM LyncUsers WHERE AccountID = @AccountID)
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[GetLyncUserPlans]
(
	@ItemID int
)
AS
SELECT
	LyncUserPlanId,
	ItemID,
	LyncUserPlanName,
	LyncUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault
FROM
	LyncUserPlans
WHERE
	ItemID = @ItemID
ORDER BY LyncUserPlanName
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




CREATE PROCEDURE [dbo].[GetLyncUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@StartRow int,
	@Count int
)
AS

CREATE TABLE #TempLyncUsers
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](300)  NOT NULL,
	[DisplayName] [nvarchar](300)  NOT NULL,
	[UserPrincipalName] [nvarchar](300) NULL,
	[SipAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL,
	[LyncUserPlanId] [int] NOT NULL,
	[LyncUserPlanName] [nvarchar] (300) NOT NULL,
)

CREATE PROCEDURE [dbo].[GetSfBUserPlan]
(
	@SfBUserPlanId int
)
AS
SELECT
	SfBUserPlanId,
	ItemID,
	SfBUserPlanName,
	SfBUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault
FROM
	SfBUserPlans
WHERE
	SfBUserPlanId = @SfBUserPlanId
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO







CREATE PROCEDURE [dbo].[GetSfBUserPlanByAccountId]
(
	@AccountID int
)
AS
SELECT
	SfBUserPlanId,
	ItemID,
	SfBUserPlanName,
	SfBUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault
FROM
	SfBUserPlans
WHERE
	SfBUserPlanId IN (SELECT SfBUserPlanId FROM SfBUsers WHERE AccountID = @AccountID)
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[GetSfBUserPlans]
(
	@ItemID int
)
AS
SELECT
	SfBUserPlanId,
	ItemID,
	SfBUserPlanName,
	SfBUserPlanType,
	IM,
	Mobility,
	MobilityEnableOutsideVoice,
	Federation,
	Conferencing,
	EnterpriseVoice,
	VoicePolicy,
	IsDefault
FROM
	SfBUserPlans
WHERE
	ItemID = @ItemID
ORDER BY SfBUserPlanName
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




CREATE PROCEDURE [dbo].[GetSfBUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@StartRow int,
	@Count int
)
AS

CREATE TABLE #TempSfBUsers
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](300)  NOT NULL,
	[DisplayName] [nvarchar](300)  NOT NULL,
	[UserPrincipalName] [nvarchar](300) NULL,
	[SipAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL,
	[SfBUserPlanId] [int] NOT NULL,
	[SfBUserPlanName] [nvarchar] (300) NOT NULL,
)


DECLARE @condition nvarchar(700)
SET @condition = ''

IF (@SortColumn = 'DisplayName')
BEGIN
	SET @condition = 'ORDER BY ea.DisplayName'
END

IF (@SortColumn = 'UserPrincipalName')
BEGIN
	SET @condition = 'ORDER BY ea.UserPrincipalName'
END

IF (@SortColumn = 'SipAddress')
BEGIN
	SET @condition = 'ORDER BY ou.SipAddress'
END


IF (@SortColumn = 'LyncUserPlanName')
BEGIN
	SET @condition = 'ORDER BY lp.LyncUserPlanName'
END

IF (@SortColumn = 'SfBUserPlanName')
BEGIN
	SET @condition = 'ORDER BY lp.SfBUserPlanName'
END

DECLARE @sql nvarchar(3500)

set @sql = ''
	INSERT INTO
		#TempLyncUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.UserPrincipalName,
		ou.SipAddress,
		ea.SamAccountName,
		ou.LyncUserPlanId,
		lp.LyncUserPlanName,
		ou.SfBUserPlanId,
		lp.SfBUserPlanName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		LyncUsers ou
	INNER JOIN
		LyncUserPlans lp
	INNER JOIN
		SfBUsers ou
	INNER JOIN
		SfBUserPlans lp
	ON
		ou.LyncUserPlanId = lp.LyncUserPlanId
	ON
		ou.SfBUserPlanId = lp.SfBUserPlanId
	ON
		ea.AccountID = ou.AccountID
	WHERE
		ea.ItemID = @ItemID + @condition

exec sp_executesql @sql, N'@ItemID int',@ItemID

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempLyncUsers

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempLyncUsers
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count)
END
ELSE
BEGIN
	IF @SortColumn <> '' AND @SortColumn IS NOT NULL
	BEGIN
		IF (@SortColumn = 'DisplayName')
		BEGIN
			SELECT * FROM #TempLyncUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
		END
		IF (@SortColumn = 'UserPrincipalName')
		BEGIN
			SELECT * FROM #TempLyncUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY UserPrincipalName DESC
		END

		IF (@SortColumn = 'SipAddress')
		BEGIN
			SELECT * FROM #TempLyncUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY SipAddress DESC
		END

		IF (@SortColumn = 'LyncUserPlanName')
		BEGIN
			SELECT * FROM #TempLyncUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY LyncUserPlanName DESC
		END
		IF (@SortColumn = 'SfBUserPlanName')
		BEGIN
			SELECT * FROM #TempLyncUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY SfBUserPlanName DESC
		END
	END
	ELSE
	BEGIN
		SELECT * FROM #TempLyncUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY UserPrincipalName DESC
	END


END

DROP TABLE #TempLyncUsers

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


DECLARE @sql nvarchar(3500)

set @sql = ''
	INSERT INTO
		#TempSfBUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.UserPrincipalName,
		ou.SipAddress,
		ea.SamAccountName,
		ou.SfBUserPlanId,
		lp.SfBUserPlanName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		SfBUsers ou
	INNER JOIN
		SfBUserPlans lp
	ON
		ou.SfBUserPlanId = lp.SfBUserPlanId
	ON
		ea.AccountID = ou.AccountID
	WHERE
		ea.ItemID = @ItemID + @condition

exec sp_executesql @sql, N'@ItemID int',@ItemID

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempSfBUsers

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempSfBUsers
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count)
END
ELSE
BEGIN
	IF @SortColumn <> '' AND @SortColumn IS NOT NULL
	BEGIN
		IF (@SortColumn = 'DisplayName')
		BEGIN
			SELECT * FROM #TempSfBUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
		END
		IF (@SortColumn = 'UserPrincipalName')
		BEGIN
			SELECT * FROM #TempSfBUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY UserPrincipalName DESC
		END

		IF (@SortColumn = 'SipAddress')
		BEGIN
			SELECT * FROM #TempSfBUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY SipAddress DESC
		END

		IF (@SortColumn = 'LyncUserPlanName')
		BEGIN
			SELECT * FROM #TempSfBUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY SfBUserPlanName DESC
		END
		IF (@SortColumn = 'SfBUserPlanName')
		BEGIN
			SELECT * FROM #TempSfBUsers
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY SfBUserPlanName DESC
		END
	END
	ELSE
	BEGIN
		SELECT * FROM #TempSfBUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY UserPrincipalName DESC
	END


END

DROP TABLE #TempSfBUsers

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




CREATE PROCEDURE [dbo].[GetLyncUsersByPlanId]
(
	@ItemID int,
	@PlanId int
)
AS

	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.UserPrincipalName,
		ea.SamAccountName,
		ou.LyncUserPlanId,
		lp.LyncUserPlanName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		LyncUsers ou
	INNER JOIN
		LyncUserPlans lp
	ON
		ou.LyncUserPlanId = lp.LyncUserPlanId
	ON
		ea.AccountID = ou.AccountID
	WHERE
		ea.ItemID = @ItemID AND
		ou.LyncUserPlanId = @PlanId

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[GetLyncUsersCount]
(
	@ItemID int
)
AS

SELECT
	COUNT(ea.AccountID)
FROM
	ExchangeAccounts ea
INNER JOIN
	LyncUsers ou
ON
	ea.AccountID = ou.AccountID
WHERE
	ea.ItemID = @ItemID

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetSfBUsersByPlanId]
(
	@ItemID int,
	@PlanId int
)
AS

	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.UserPrincipalName,
		ea.SamAccountName,
		ou.SfBUserPlanId,
		lp.SfBUserPlanName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		SfBUsers ou
	INNER JOIN
		SfBUserPlans lp
	ON
		ou.SfBUserPlanId = lp.SfBUserPlanId
	ON
		ea.AccountID = ou.AccountID
	WHERE
		ea.ItemID = @ItemID AND
		ou.SfBUserPlanId = @PlanId

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[GetSfBUsersCount]
(
	@ItemID int
)
AS

SELECT
	COUNT(ea.AccountID)
FROM
	ExchangeAccounts ea
INNER JOIN
	SfBUsers ou
ON
	ea.AccountID = ou.AccountID
WHERE
	ea.ItemID = @ItemID

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE GetMyPackages
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.PackageName,
	P.StatusID,
	P.PlanID,
	P.PurchaseDate,

	dbo.GetItemComments(P.PackageID, 'PACKAGE', @ActorID) AS Comments,

	-- server
	ISNULL(P.ServerID, 0) AS ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,

	-- hosting plan
	HP.PlanName,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM Packages AS P
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
LEFT OUTER JOIN Servers AS S ON P.ServerID = S.ServerID
LEFT OUTER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE P.UserID = @UserID
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetNestedPackagesPaged]
(
	@ActorID int,
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@StatusID int,
	@PlanID int,
	@ServerID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR(''You are not allowed to access this package'', 16, 1)

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Packages TABLE
(
	ItemPosition int IDENTITY(1,1),
	PackageID int
)
INSERT INTO @Packages (PackageID)
SELECT
	P.PackageID
FROM Packages AS P
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	P.ParentPackageID = @PackageID
	AND ((@StatusID = 0) OR (@StatusID > 0 AND P.StatusID = @StatusID))
	AND ((@PlanID = 0) OR (@PlanID > 0 AND P.PlanID = @PlanID))
	AND ((@ServerID = 0) OR (@ServerID > 0 AND P.ServerID = @ServerID)) '

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(PackageID) FROM @Packages;
SELECT
	P.PackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,

	dbo.GetItemComments(P.PackageID, ''PACKAGE'', @ActorID) AS Comments,

	-- server
	P.ServerID,
	ISNULL(S.ServerName, ''None'') AS ServerName,
	ISNULL(S.Comments, '''') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,

	-- hosting plan
	P.PlanID,
	HP.PlanName,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Packages AS TP
INNER JOIN Packages AS P ON TP.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE TP.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @PackageID int, @FilterValue nvarchar(50), @ActorID int, @StatusID int, @PlanID int, @ServerID int',
@StartRow, @MaximumRows, @PackageID, @FilterValue, @ActorID, @StatusID, @PlanID, @ServerID


RETURN






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetNestedPackagesSummary
(
	@ActorID int,
	@PackageID int
)
AS
-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- ALL spaces
SELECT COUNT(PackageID) AS PackagesNumber FROM Packages
WHERE ParentPackageID = @PackageID

-- BY STATUS spaces
SELECT StatusID, COUNT(PackageID) AS PackagesNumber FROM Packages
WHERE ParentPackageID = @PackageID AND StatusID > 0
GROUP BY StatusID
ORDER BY StatusID

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetNextSchedule
AS

-- find next schedule
DECLARE @ScheduleID int
DECLARE @TaskID nvarchar(100)
SELECT TOP 1
	@ScheduleID = ScheduleID,
	@TaskID = TaskID
FROM Schedule AS S
WHERE Enabled = 1
ORDER BY NextRun ASC

-- select schedule
SELECT TOP 1
	S.ScheduleID,
	S.TaskID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	S.HistoriesNumber,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	1 AS StatusID
FROM Schedule AS S
WHERE S.ScheduleID = @ScheduleID
ORDER BY NextRun ASC

-- select task
SELECT
	TaskID,
	TaskType,
	RoleID
FROM ScheduleTasks
WHERE TaskID = @TaskID

-- select schedule parameters
SELECT
	S.ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	ISNULL(SP.ParameterValue, STP.DefaultValue) AS ParameterValue
FROM Schedule AS S
INNER JOIN ScheduleTaskParameters AS STP ON S.TaskID = STP.TaskID
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = S.ScheduleID
WHERE S.ScheduleID = @ScheduleID
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










CREATE PROCEDURE [dbo].[GetOCSUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@Name nvarchar(400),
	@Email nvarchar(400),
	@StartRow int,
	@Count int
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

CREATE TABLE #TempOCSUsers
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](300)  NOT NULL,
	[DisplayName] [nvarchar](300)  NOT NULL,
	[InstanceID] [nvarchar](50)  NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL
)


IF (@SortColumn = 'DisplayName')
BEGIN
	INSERT INTO
		#TempOCSUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ou.InstanceID,
		ea.PrimaryEmailAddress,
		ea.SamAccountName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		OCSUsers ou
	ON
		ea.AccountID = ou.AccountID
	WHERE
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email
	ORDER BY
		ea.DisplayName
END
ELSE
BEGIN
	INSERT INTO
		#TempOCSUsers
	SELECT
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ou.InstanceID,
		ea.PrimaryEmailAddress,
		ea.SamAccountName
	FROM
		ExchangeAccounts ea
	INNER JOIN
		OCSUsers ou
	ON
		ea.AccountID = ou.AccountID
	WHERE
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email
	ORDER BY
		ea.PrimaryEmailAddress
END

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempOCSUsers

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempOCSUsers
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count)
END
ELSE
BEGIN
	IF (@SortColumn = 'DisplayName')
	BEGIN
		SELECT * FROM #TempOCSUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
	END
	ELSE
	BEGIN
		SELECT * FROM #TempOCSUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY PrimaryEmailAddress DESC
	END

END


DROP TABLE #TempOCSUsers


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[GetOCSUsersCount]
(
	@ItemID int,
	@Name nvarchar(400),
	@Email nvarchar(400)

)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

SELECT
	COUNT(ea.AccountID)
FROM
	ExchangeAccounts ea
INNER JOIN
	OCSUsers ou
ON
	ea.AccountID = ou.AccountID
WHERE
	ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetOrganizationCRMUserCount]
	@ItemID int
AS
BEGIN
SELECT
 COUNT(CRMUserID)
FROM
	CrmUsers CU
INNER JOIN
	ExchangeAccounts EA
ON
	CU.AccountID = EA.AccountID
WHERE EA.ItemID = @ItemID
END

























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[GetOrganizationStatistics]
(
	@ItemID int
)
AS
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 7 OR AccountType = 1 OR AccountType = 6 OR AccountType = 5)  AND ItemID = @ItemID) AS CreatedUsers,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetPackage]
(
	@PackageID int,
	@ActorID int
)
AS

-- Note: ActorID is not verified
-- check both requested and parent package

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.UserID,
	P.PackageName,
	P.PackageComments,
	P.ServerID,
	P.StatusID,
	P.PlanID,
	P.PurchaseDate,
	P.OverrideQuotas
FROM Packages AS P
WHERE P.PackageID = @PackageID
RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[GetPackageAddon]
(
	@ActorID int,
	@PackageAddonID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	PackageAddonID,
	PackageID,
	PlanID,
	PurchaseDate,
	Quantity,
	StatusID,
	Comments
FROM PackageAddons AS PA
WHERE PA.PackageAddonID = @PackageAddonID
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetPackageAddons
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	PA.PackageAddonID,
	PA.PackageID,
	PA.PlanID,
	PA.Quantity,
	PA.PurchaseDate,
	PA.StatusID,
	PA.Comments,
	HP.PlanName,
	HP.PlanDescription
FROM PackageAddons AS PA
INNER JOIN HostingPlans AS HP ON PA.PlanID = HP.PlanID
WHERE PA.PackageID = @PackageID
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageBandwidth
(
	@ActorID int,
	@PackageID int,
	@StartDate datetime,
	@EndDate datetime
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	RG.GroupID,
	RG.GroupName,
	ROUND(CONVERT(float, ISNULL(GB.BytesSent, 0)) / 1024 / 1024, 0) AS MegaBytesSent,
	ROUND(CONVERT(float, ISNULL(GB.BytesReceived, 0)) / 1024 / 1024, 0) AS MegaBytesReceived,
	ROUND(CONVERT(float, ISNULL(GB.BytesTotal, 0)) / 1024 / 1024, 0) AS MegaBytesTotal,
	ISNULL(GB.BytesSent, 0) AS BytesSent,
	ISNULL(GB.BytesReceived, 0) AS BytesReceived,
	ISNULL(GB.BytesTotal, 0) AS BytesTotal
FROM ResourceGroups AS RG
LEFT OUTER JOIN
(
	SELECT
		PB.GroupID,
		SUM(ISNULL(PB.BytesSent, 0)) AS BytesSent,
		SUM(ISNULL(PB.BytesReceived, 0)) AS BytesReceived,
		SUM(ISNULL(PB.BytesSent, 0)) + SUM(ISNULL(PB.BytesReceived, 0)) AS BytesTotal
	FROM PackagesTreeCache AS PT
	INNER JOIN PackagesBandwidth AS PB ON PT.PackageID = PB.PackageID
	INNER JOIN Packages AS P ON PB.PackageID = P.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PB.GroupID = HPR.GroupID AND HPR.PlanID = P.PlanID
		AND HPR.CalculateBandwidth = 1
	WHERE
		PT.ParentPackageID = @PackageID
		AND PB.LogDate BETWEEN @StartDate AND @EndDate
	GROUP BY PB.GroupID
) AS GB ON RG.GroupID = GB.GroupID
WHERE GB.BytesTotal > 0
ORDER BY RG.GroupOrder

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageBandwidthUpdate
(
	@PackageID int,
	@UpdateDate datetime OUTPUT
)
AS
	SELECT @UpdateDate = BandwidthUpdated FROM Packages
	WHERE PackageID = @PackageID
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageDiskspace
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	RG.GroupID,
	RG.GroupName,
	ROUND(CONVERT(float, ISNULL(GD.Diskspace, 0)) / 1024 / 1024, 0) AS Diskspace,
	ISNULL(GD.Diskspace, 0) AS DiskspaceBytes
FROM ResourceGroups AS RG
LEFT OUTER JOIN
(
	SELECT
		PD.GroupID,
		SUM(ISNULL(PD.DiskSpace, 0)) AS Diskspace -- in megabytes
	FROM PackagesTreeCache AS PT
	INNER JOIN PackagesDiskspace AS PD ON PT.PackageID = PD.PackageID
	INNER JOIN Packages AS P ON PT.PackageID = P.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PD.GroupID = HPR.GroupID
		AND HPR.PlanID = P.PlanID AND HPR.CalculateDiskspace = 1
	WHERE PT.ParentPackageID = @PackageID
	GROUP BY PD.GroupID
) AS GD ON RG.GroupID = GD.GroupID
WHERE GD.Diskspace <> 0
ORDER BY RG.GroupOrder

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetPackageIPAddress]
	@PackageAddressID int
AS
BEGIN


SELECT
	PA.PackageAddressID,
	PA.AddressID,
	IP.ExternalIP,
	IP.InternalIP,
	IP.SubnetMask,
	IP.DefaultGateway,
	PA.ItemID,
	SI.ItemName,
	PA.PackageID,
	P.PackageName,
	P.UserID,
	U.UserName,
	PA.IsPrimary
FROM dbo.PackageIPAddresses AS PA
INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
WHERE PA.PackageAddressID = @PackageAddressID

END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetPackageIPAddresses]
(
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@PoolID int = 0,
	@Recursive bit = 0
)
AS
BEGIN


-- start
DECLARE @condition nvarchar(700)
SET @condition = '
((@Recursive = 0 AND PA.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, PA.PackageID) = 1))
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'IP.ExternalIP ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(PA.PackageAddressID)
FROM dbo.PackageIPAddresses PA
INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
WHERE ' + @condition + '

DECLARE @Addresses AS TABLE
(
	PackageAddressID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		PA.PackageAddressID
	FROM dbo.PackageIPAddresses PA
	INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
	INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
	INNER JOIN dbo.Users U ON U.UserID = P.UserID
	LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
	WHERE ' + @condition + '
)

INSERT INTO @Addresses
SELECT PackageAddressID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	PA.PackageAddressID,
	PA.AddressID,
	IP.ExternalIP,
	IP.InternalIP,
	IP.SubnetMask,
	IP.DefaultGateway,
	PA.ItemID,
	SI.ItemName,
	PA.PackageID,
	P.PackageName,
	P.UserID,
	U.UserName,
	PA.IsPrimary
FROM @Addresses AS TA
INNER JOIN dbo.PackageIPAddresses AS PA ON TA.PackageAddressID = PA.PackageAddressID
INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
'

print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit, @PoolID int',
@PackageID, @StartRow, @MaximumRows, @Recursive, @PoolID

END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetPackagePackages
(
	@ActorID int,
	@PackageID int,
	@Recursive bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,

	-- server
	P.ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,

	-- hosting plan
	P.PlanID,
	HP.PlanName,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.RoleID,
	U.Email
FROM Packages AS P
INNER JOIN Users AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	((@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1)
		OR (@Recursive = 0 AND P.ParentPackageID = @PackageID))
	AND P.PackageID <> @PackageID
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetPackagePrivateIPAddresses]
	@PackageID int
AS
BEGIN

	SELECT
		PA.PrivateAddressID,
		PA.IPAddress,
		PA.ItemID,
		SI.ItemName,
		PA.IsPrimary
	FROM PrivateIPAddresses AS PA
	INNER JOIN ServiceItems AS SI ON PA.ItemID = SI.ItemID
	WHERE SI.PackageID = @PackageID

END





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetPackagePrivateIPAddressesPaged]
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
AS
BEGIN


-- start
DECLARE @condition nvarchar(700)
SET @condition = '
SI.PackageID = @PackageID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'PA.IPAddress ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(PA.PrivateAddressID)
FROM dbo.PrivateIPAddresses AS PA
INNER JOIN dbo.ServiceItems AS SI ON PA.ItemID = SI.ItemID
WHERE ' + @condition + '

DECLARE @Addresses AS TABLE
(
	PrivateAddressID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		PA.PrivateAddressID
	FROM dbo.PrivateIPAddresses AS PA
	INNER JOIN dbo.ServiceItems AS SI ON PA.ItemID = SI.ItemID
	WHERE ' + @condition + '
)

INSERT INTO @Addresses
SELECT PrivateAddressID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	PA.PrivateAddressID,
	PA.IPAddress,
	PA.ItemID,
	SI.ItemName,
	PA.IsPrimary
FROM @Addresses AS TA
INNER JOIN dbo.PrivateIPAddresses AS PA ON TA.PrivateAddressID = PA.PrivateAddressID
INNER JOIN dbo.ServiceItems AS SI ON PA.ItemID = SI.ItemID
'

print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int',
@PackageID, @StartRow, @MaximumRows

END





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetPackageQuota]
(
	@ActorID int,
	@PackageID int,
	@QuotaName nvarchar(50)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- return quota
SELECT
	Q.QuotaID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	ISNULL(dbo.GetPackageAllocatedQuota(@PackageId, Q.QuotaID), 0) AS QuotaAllocatedValue,
	ISNULL(dbo.CalculateQuotaUsage(@PackageId, Q.QuotaID), 0) AS QuotaUsedValue
FROM Quotas AS Q
WHERE Q.QuotaName = @QuotaName

RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

































CREATE PROCEDURE GetPackageQuotas
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @PlanID int, @ParentPackageID int
SELECT @PlanID = PlanID, @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

-- get resource groups
SELECT
	RG.GroupID,
	RG.GroupName,
	ISNULL(HPR.CalculateDiskSpace, 0) AS CalculateDiskSpace,
	ISNULL(HPR.CalculateBandwidth, 0) AS CalculateBandwidth,
	dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, 0) AS ParentEnabled
FROM ResourceGroups AS RG
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
WHERE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, 0) = 1
ORDER BY RG.GroupOrder


-- return quotas
SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) AS QuotaValue,
	dbo.GetPackageAllocatedQuota(@ParentPackageID, Q.QuotaID) AS ParentQuotaValue,
	ISNULL(dbo.CalculateQuotaUsage(@PackageID, Q.QuotaID), 0) AS QuotaUsedValue
FROM Quotas AS Q
ORDER BY Q.QuotaOrder
RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetPackageQuotasForEdit
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @ServerID int, @ParentPackageID int, @PlanID int
SELECT @ServerID = ServerID, @ParentPackageID = ParentPackageID, @PlanID = PlanID FROM Packages
WHERE PackageID = @PackageID

-- get resource groups
SELECT
	RG.GroupID,
	RG.GroupName,
	ISNULL(PR.CalculateDiskSpace, ISNULL(HPR.CalculateDiskSpace, 0)) AS CalculateDiskSpace,
	ISNULL(PR.CalculateBandwidth, ISNULL(HPR.CalculateBandwidth, 0)) AS CalculateBandwidth,
	dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID) AS Enabled,
	dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, @ServerID) AS ParentEnabled
FROM ResourceGroups AS RG
LEFT OUTER JOIN PackageResources AS PR ON RG.GroupID = PR.GroupID AND PR.PackageID = @PackageID
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
ORDER BY RG.GroupOrder


-- return quotas
SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	CASE
		WHEN PQ.QuotaValue IS NULL THEN dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID)
		ELSE PQ.QuotaValue
	END QuotaValue,
	dbo.GetPackageAllocatedQuota(@ParentPackageID, Q.QuotaID) AS ParentQuotaValue
FROM Quotas AS Q
LEFT OUTER JOIN PackageQuotas AS PQ ON PQ.QuotaID = Q.QuotaID AND PQ.PackageID = @PackageID
ORDER BY Q.QuotaOrder

RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










CREATE PROCEDURE [dbo].[GetPackages]
(
	@ActorID int,
	@UserID int
)
AS

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,

	-- server
	ISNULL(P.ServerID, 0) AS ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,

	-- hosting plan
	P.PlanID,
	HP.PlanName,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.RoleID,
	U.Email
FROM Packages AS P
INNER JOIN Users AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	P.UserID = @UserID
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackagesBandwidthPaged
(
	@ActorID int,
	@UserID int,
	@PackageID int,
	@StartDate datetime,
	@EndDate datetime,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @sql nvarchar(4000)

SET @sql = '
DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows

DECLARE @Report TABLE
(
	ItemPosition int IDENTITY(0,1),
	PackageID int,
	QuotaValue int,
	Bandwidth int,
	UsagePercentage int,
	PackagesNumber int
)

INSERT INTO @Report (PackageID, QuotaValue, Bandwidth, UsagePercentage, PackagesNumber)
SELECT
	P.PackageID,
	PB.QuotaValue,
	PB.Bandwidth,
	UsagePercentage = 	CASE
							WHEN PB.QuotaValue = -1 THEN 0
							WHEN PB.QuotaValue <> 0 THEN PB.Bandwidth * 100 / PB.QuotaValue
							ELSE 0
						END,
	(SELECT COUNT(NP.PackageID) FROM Packages AS NP WHERE NP.ParentPackageID = P.PackageID) AS PackagesNumber
FROM Packages AS P
LEFT OUTER JOIN
(
	SELECT
		P.PackageID,
		dbo.GetPackageAllocatedQuota(P.PackageID, 51) AS QuotaValue, -- bandwidth
		ROUND(CONVERT(float, SUM(ISNULL(PB.BytesSent + PB.BytesReceived, 0))) / 1024 / 1024, 0) AS Bandwidth -- in megabytes
	FROM Packages AS P
	INNER JOIN PackagesTreeCache AS PT ON P.PackageID = PT.ParentPackageID
	INNER JOIN Packages AS PC ON PT.PackageID = PC.PackageID
	INNER JOIN PackagesBandwidth AS PB ON PT.PackageID = PB.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PB.GroupID = HPR.GroupID
		AND HPR.PlanID = PC.PlanID
	WHERE PB.LogDate BETWEEN @StartDate AND @EndDate
		AND HPR.CalculateBandwidth = 1
	GROUP BY P.PackageID
) AS PB ON P.PackageID = PB.PackageID
WHERE (@PackageID = -1 AND P.UserID = @UserID) OR
	(@PackageID <> -1 AND P.ParentPackageID = @PackageID) '

IF @SortColumn = '' OR @SortColumn IS NULL
SET @SortColumn = 'UsagePercentage DESC'

SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + '
SELECT COUNT(PackageID) FROM @Report

SELECT
	R.PackageID,
	ISNULL(R.QuotaValue, 0) AS QuotaValue,
	ISNULL(R.Bandwidth, 0) AS Bandwidth,
	ISNULL(R.UsagePercentage, 0) AS UsagePercentage,

	-- package
	P.PackageName,
	ISNULL(R.PackagesNumber, 0) AS PackagesNumber,
	P.StatusID,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email,
	dbo.GetItemComments(U.UserID, ''USER'', @ActorID) AS UserComments
FROM @Report AS R
INNER JOIN Packages AS P ON R.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
WHERE R.ItemPosition BETWEEN @StartRow AND @EndRow
'

exec sp_executesql @sql, N'@ActorID int, @UserID int, @PackageID int, @StartDate datetime, @EndDate datetime, @StartRow int, @MaximumRows int',
@ActorID, @UserID, @PackageID, @StartDate, @EndDate, @StartRow, @MaximumRows

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackagesDiskspacePaged
(
	@ActorID int,
	@UserID int,
	@PackageID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @sql nvarchar(4000)

SET @sql = '
DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows

DECLARE @Report TABLE
(
	ItemPosition int IDENTITY(0,1),
	PackageID int,
	QuotaValue int,
	Diskspace int,
	UsagePercentage int,
	PackagesNumber int
)

INSERT INTO @Report (PackageID, QuotaValue, Diskspace, UsagePercentage, PackagesNumber)
SELECT
	P.PackageID,
	PD.QuotaValue,
	PD.Diskspace,
	UsagePercentage = 	CASE
							WHEN PD.QuotaValue = -1 THEN 0
							WHEN PD.QuotaValue <> 0 THEN PD.Diskspace * 100 / PD.QuotaValue
							ELSE 0
						END,
	(SELECT COUNT(NP.PackageID) FROM Packages AS NP WHERE NP.ParentPackageID = P.PackageID) AS PackagesNumber
FROM Packages AS P
LEFT OUTER JOIN
(
	SELECT
		P.PackageID,
		dbo.GetPackageAllocatedQuota(P.PackageID, 52) AS QuotaValue, -- diskspace
		ROUND(CONVERT(float, SUM(ISNULL(PD.DiskSpace, 0))) / 1024 / 1024, 0) AS Diskspace -- in megabytes
	FROM Packages AS P
	INNER JOIN PackagesTreeCache AS PT ON P.PackageID = PT.ParentPackageID
	INNER JOIN Packages AS PC ON PT.PackageID = PC.PackageID
	INNER JOIN PackagesDiskspace AS PD ON PT.PackageID = PD.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PD.GroupID = HPR.GroupID
		AND HPR.PlanID = PC.PlanID
	WHERE HPR.CalculateDiskspace = 1
	GROUP BY P.PackageID
) AS PD ON P.PackageID = PD.PackageID
WHERE (@PackageID = -1 AND P.UserID = @UserID) OR
	(@PackageID <> -1 AND P.ParentPackageID = @PackageID)
'

IF @SortColumn = '' OR @SortColumn IS NULL
SET @SortColumn = 'UsagePercentage DESC'

SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + '
SELECT COUNT(PackageID) FROM @Report

SELECT
	R.PackageID,
	ISNULL(R.QuotaValue, 0) AS QuotaValue,
	ISNULL(R.Diskspace, 0) AS Diskspace,
	ISNULL(R.UsagePercentage, 0) AS UsagePercentage,

	-- package
	P.PackageName,
	ISNULL(R.PackagesNumber, 0) AS PackagesNumber,
	P.StatusID,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email,
	dbo.GetItemComments(U.UserID, ''USER'', @ActorID) AS UserComments
FROM @Report AS R
INNER JOIN Packages AS P ON R.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
WHERE R.ItemPosition BETWEEN @StartRow AND @EndRow
'

exec sp_executesql @sql, N'@ActorID int, @UserID int, @PackageID int, @StartRow int, @MaximumRows int',
@ActorID, @UserID, @PackageID, @StartRow, @MaximumRows

RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetPackageServiceID
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







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageSettings
(
	@ActorID int,
	@PackageID int,
	@SettingsName nvarchar(50)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @ParentPackageID int, @TmpPackageID int
SET @TmpPackageID = @PackageID

WHILE 10 = 10
BEGIN
	IF @TmpPackageID < 2 -- system package
	BEGIN
		SELECT
			@TmpPackageID AS PackageID,
			'Dump' AS PropertyName,
			'' AS PropertyValue
	END
	ELSE
	BEGIN
		-- user package
		IF EXISTS
		(
			SELECT PropertyName FROM PackageSettings
			WHERE SettingsName = @SettingsName AND PackageID = @TmpPackageID
		)
		BEGIN
			SELECT
				PackageID,
				PropertyName,
				PropertyValue
			FROM
				PackageSettings
			WHERE
				PackageID = @TmpPackageID AND
				SettingsName = @SettingsName

			BREAK
		END
	END


	SET @ParentPackageID = NULL --reset var

	-- get owner
	SELECT
		@ParentPackageID = ParentPackageID
	FROM Packages
	WHERE PackageID = @TmpPackageID

	IF @ParentPackageID IS NULL -- the last parent
	BREAK

	SET @TmpPackageID = @ParentPackageID
END

RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetPackagesPaged]
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
DECLARE @Packages TABLE
(
	ItemPosition int IDENTITY(1,1),
	PackageID int
)
INSERT INTO @Packages (PackageID)
SELECT
	P.PackageID
FROM Packages AS P
--INNER JOIN UsersTree(@UserID, 1) AS UT ON P.UserID = UT.UserID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	P.UserID <> @UserID AND dbo.CheckUserParent(@UserID, P.UserID) = 1
	AND @HasUserRights = 1 '

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(PackageID) FROM @Packages;
SELECT
	P.PackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,

	dbo.GetItemComments(P.PackageID, ''PACKAGE'', @ActorID) AS Comments,

	-- server
	P.ServerID,
	ISNULL(S.ServerName, ''None'') AS ServerName,
	ISNULL(S.Comments, '''') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,

	-- hosting plan
	P.PlanID,
	HP.PlanName,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Packages AS TP
INNER JOIN Packages AS P ON TP.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE TP.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ActorID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ActorID


RETURN






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetPackageUnassignedIPAddresses]
(
	@ActorID int,
	@PackageID int,
	@PoolID int = 0
)
AS
BEGIN
	SELECT
		PIP.PackageAddressID,
		IP.AddressID,
		IP.ExternalIP,
		IP.InternalIP,
		IP.ServerID,
		IP.PoolID,
		PIP.IsPrimary,
		IP.SubnetMask,
		IP.DefaultGateway
	FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE
		PIP.ItemID IS NULL
		AND PIP.PackageID = @PackageID
		AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
	ORDER BY IP.DefaultGateway, IP.ExternalIP
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[GetPendingSSLForWebsite]
(
	@ActorID int,
	@PackageID int,
	@websiteid int,
	@Recursive bit = 1
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

SELECT
	[ID], [UserID], [SiteID], [Hostname], [CSR], [Certificate], [Hash], [Installed]
FROM
	[dbo].[SSLCertificates]
WHERE
	@websiteid = 2 AND [Installed] = 0 AND [IsRenewal] = 0

RETURN





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetProvider
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetProviderByServiceID
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















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






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetProviderServiceQuota
(
	@ProviderID int
)
AS

SELECT TOP 1
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	Q.ServiceQuota
FROM Providers AS P
INNER JOIN Quotas AS Q ON P.GroupID = Q.GroupID
WHERE P.ProviderID = @ProviderID AND Q.ServiceQuota = 1


RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetQuotas
AS
SELECT
	Q.GroupID,
	Q.QuotaID,
	RG.GroupName,
	Q.QuotaDescription,
	Q.QuotaTypeID
FROM Quotas AS Q
INNER JOIN ResourceGroups AS RG ON Q.GroupID = RG.GroupID
ORDER BY RG.GroupOrder, Q.QuotaOrder
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetResellerDomains
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetResourceGroup
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetResourceGroups
AS
SELECT
	GroupID,
	GroupName,
	GroupController
FROM ResourceGroups
ORDER BY GroupOrder
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSchedule
(
	@ActorID int,
	@ScheduleID int
)
AS

-- select schedule
SELECT TOP 1
	S.ScheduleID,
	S.TaskID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	S.HistoriesNumber,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	1 AS StatusID
FROM Schedule AS S
WHERE
	S.ScheduleID = @ScheduleID
	AND dbo.CheckActorPackageRights(@ActorID, S.PackageID) = 1

-- select task
SELECT
	ST.TaskID,
	ST.TaskType,
	ST.RoleID
FROM Schedule AS S
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
WHERE
	S.ScheduleID = @ScheduleID
	AND dbo.CheckActorPackageRights(@ActorID, S.PackageID) = 1

-- select schedule parameters
SELECT
	S.ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	ISNULL(SP.ParameterValue, STP.DefaultValue) AS ParameterValue
FROM Schedule AS S
INNER JOIN ScheduleTaskParameters AS STP ON S.TaskID = STP.TaskID
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = S.ScheduleID
WHERE
	S.ScheduleID = @ScheduleID
	AND dbo.CheckActorPackageRights(@ActorID, S.PackageID) = 1

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetScheduleInternal
(
	@ScheduleID int
)
AS

-- select schedule
SELECT
	S.ScheduleID,
	S.TaskID,
	ST.TaskType,
	ST.RoleID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	1 AS StatusID,
	S.PriorityID,
	S.HistoriesNumber,
	S.MaxExecutionTime,
	S.WeekMonthDay
FROM Schedule AS S
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
WHERE ScheduleID = @ScheduleID
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[GetScheduleParameters]
(
	@ActorID int,
	@TaskID nvarchar(100),
	@ScheduleID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Schedule
WHERE ScheduleID = @ScheduleID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	@ScheduleID AS ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	SP.ParameterValue,
	STP.DefaultValue
FROM ScheduleTaskParameters AS STP
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = @ScheduleID
WHERE STP.TaskID = @TaskID
ORDER BY STP.ParameterOrder

RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSchedules
(
	@ActorID int,
	@PackageID int,
	@Recursive bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Schedules TABLE
(
	ScheduleID int
)

INSERT INTO @Schedules (ScheduleID)
SELECT
	S.ScheduleID
FROM Schedule AS S
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON S.PackageID = PT.PackageID
ORDER BY S.Enabled DESC, S.NextRun


-- select schedules
SELECT
	S.ScheduleID,
	S.TaskID,
	ST.TaskType,
	ST.RoleID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	1 AS StatusID,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	ISNULL(0, (SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = 'SCHEDULER' ORDER BY StartDate DESC)) AS LastResult,

	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Schedules AS STEMP
INNER JOIN Schedule AS S ON STEMP.ScheduleID = S.ScheduleID
INNER JOIN Packages AS P ON S.PackageID = P.PackageID
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select schedule parameters
SELECT
	S.ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	ISNULL(SP.ParameterValue, STP.DefaultValue) AS ParameterValue
FROM @Schedules AS STEMP
INNER JOIN Schedule AS S ON STEMP.ScheduleID = S.ScheduleID
INNER JOIN ScheduleTaskParameters AS STP ON S.TaskID = STP.TaskID
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = S.ScheduleID
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSchedulesPaged
(
	@ActorID int,
	@PackageID int,
	@Recursive bit,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
BEGIN

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @condition nvarchar(400)
SET @condition = ' 1 = 1 '

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'S.ScheduleName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(S.ScheduleID) FROM Schedule AS S
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON S.PackageID = PT.PackageID
INNER JOIN Packages AS P ON S.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
WHERE ' + @condition + '

DECLARE @Schedules AS TABLE
(
	ScheduleID int
);

WITH TempSchedules AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		S.ScheduleID
	FROM Schedule AS S
	INNER JOIN Packages AS P ON S.PackageID = P.PackageID
	INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON S.PackageID = PT.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE ' + @condition + '
)

INSERT INTO @Schedules
SELECT ScheduleID FROM TempSchedules
WHERE TempSchedules.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	S.ScheduleID,
	S.TaskID,
	ST.TaskType,
	ST.RoleID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	1 AS StatusID,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	ISNULL(0, (SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = ''SCHEDULER'' ORDER BY StartDate DESC)) AS LastResult,

	-- packages
	P.PackageID,
	P.PackageName,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Schedules AS STEMP
INNER JOIN Schedule AS S ON STEMP.ScheduleID = S.ScheduleID
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
INNER JOIN Packages AS P ON S.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID'

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

END
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetScheduleTask
(
	@ActorID int,
	@TaskID nvarchar(100)
)
AS

-- get user role
DECLARE @RoleID int
SELECT @RoleID = RoleID FROM Users
WHERE UserID = @ActorID

SELECT
	TaskID,
	TaskType,
	RoleID
FROM ScheduleTasks
WHERE
	TaskID = @TaskID
	AND @RoleID >= RoleID
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetScheduleTasks
(
	@ActorID int
)
AS

-- get user role
DECLARE @RoleID int
SELECT @RoleID = RoleID FROM Users
WHERE UserID = @ActorID

SELECT
	TaskID,
	TaskType,
	RoleID
FROM ScheduleTasks
WHERE @RoleID <= RoleID
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

























/****** Object:  StoredProcedure [dbo].[GetScheduleTaskViewConfigurations]    Script Date: 09/10/2007 17:53:56 ******/

CREATE PROCEDURE [dbo].[GetScheduleTaskViewConfigurations]
(
	@TaskID nvarchar(100)
)
AS

SELECT
	@TaskID AS TaskID,
	STVC.ConfigurationID,
	STVC.Environment,
	STVC.Description
FROM ScheduleTaskViewConfiguration AS STVC
WHERE STVC.TaskID = @TaskID

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSearchableServiceItemTypes

AS
SELECT
	ItemTypeID,
	DisplayName
FROM
	ServiceItemTypes
WHERE Searchable = 1
ORDER BY TypeOrder
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetServer]
(
	@ActorID int,
	@ServerID int
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
	ADAuthenticationType
FROM Servers
WHERE
	ServerID = @ServerID
	AND @IsAdmin = 1

RETURN









































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE GetServerByName
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
	ADAuthenticationType
FROM Servers
WHERE
	ServerName = @ServerName
	AND @IsAdmin = 1

RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetServerInternal
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
	ADAuthenticationType
FROM Servers
WHERE
	ServerID = @ServerID

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetServers
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetServerShortDetails
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






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










GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















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

























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















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

























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















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
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















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


























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















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
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















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

























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















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





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















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
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















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

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

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





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetServiceItemType]
(
	@ItemTypeID int
)
AS
SELECT
	[ItemTypeID],
	[GroupID],
	[DisplayName],
	[TypeName],
	[TypeOrder],
	[CalculateDiskspace],
	[CalculateBandwidth],
	[Suspendable],
	[Disposable],
	[Searchable],
	[Importable],
	[Backupable]
FROM
	[ServiceItemTypes]
WHERE
	[ItemTypeID] = @ItemTypeID


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetServiceItemTypes]
AS
SELECT
	[ItemTypeID],
	[GroupID],
	[DisplayName],
	[TypeName],
	[TypeOrder],
	[CalculateDiskspace],
	[CalculateBandwidth],
	[Suspendable],
	[Disposable],
	[Searchable],
	[Importable],
	[Backupable]
FROM
	[ServiceItemTypes]
ORDER BY TypeOrder


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE GetServiceProperties
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetServicesByGroupID
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







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[GetServicesByGroupName]
(
	@ActorID int,
	@GroupName nvarchar(100)
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
	AND @IsAdmin = 1
RETURN
















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetServicesByServerID
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetServicesByServerIDGroupName
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[GetSiteCert]
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






GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[GetSSLCertificateByID]
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





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE GetSystemSettings
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



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetUnallottedIPAddresses]
	@PackageID int,
	@ServiceID int,
	@PoolID int = 0
AS
BEGIN

	DECLARE @ParentPackageID int
	DECLARE @ServerID int

	SELECT
		@ParentPackageID = ParentPackageID,
		@ServerID = ServerID
	FROM Packages
	WHERE PackageID = @PackageId

    IF (@ParentPackageID = 1 OR @PoolID = 4 /* management network */) -- "System" space
    BEGIN
		-- check if server is physical
		IF EXISTS(SELECT * FROM Servers WHERE ServerID = @ServerID AND VirtualServer = 0)
		BEGIN
			-- physical server
			SELECT
				IP.AddressID,
				IP.ExternalIP,
				IP.InternalIP,
				IP.ServerID,
				IP.PoolID,
				IP.SubnetMask,
				IP.DefaultGateway
			FROM dbo.IPAddresses AS IP
			WHERE
				IP.ServerID = @ServerID
				AND IP.AddressID NOT IN (SELECT PIP.AddressID FROM dbo.PackageIPAddresses AS PIP)
				AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
			ORDER BY IP.DefaultGateway, IP.ExternalIP
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
				IP.AddressID,
				IP.ExternalIP,
				IP.InternalIP,
				IP.ServerID,
				IP.PoolID,
				IP.SubnetMask,
				IP.DefaultGateway
			FROM dbo.IPAddresses AS IP
			WHERE
				IP.ServerID IN (
					SELECT SVC.ServerID FROM [dbo].[Services] AS SVC
					INNER JOIN [dbo].[Providers] AS P ON SVC.ProviderID = P.ProviderID
					WHERE [SVC].[ServiceID] = @ServiceId AND P.GroupID = @GroupID
				)
				AND IP.AddressID NOT IN (SELECT PIP.AddressID FROM dbo.PackageIPAddresses AS PIP)
				AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
			ORDER BY IP.DefaultGateway, IP.ExternalIP
		END
	END
	ELSE -- 2rd level space and below
	BEGIN
		-- get service location
		SELECT @ServerID = S.ServerID FROM Services AS S
		WHERE S.ServiceID = @ServiceID

		SELECT
			IP.AddressID,
			IP.ExternalIP,
			IP.InternalIP,
			IP.ServerID,
			IP.PoolID,
			IP.SubnetMask,
			IP.DefaultGateway
		FROM dbo.PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE
			PIP.PackageID = @ParentPackageID
			AND PIP.ItemID IS NULL
			AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
			AND IP.ServerID = @ServerID
		ORDER BY IP.DefaultGateway, IP.ExternalIP
	END
END



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetUserAvailableHostingAddons
(
	@ActorID int,
	@UserID int
)
AS

-- user should see the plans only of his reseller
-- also user can create packages based on his own plans (admins and resellers)

DECLARE @Plans TABLE
(
	PlanID int
)

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @OwnerID int
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @UserID

SELECT
	HP.PlanID,
	HP.PackageID,
	HP.PlanName,
	HP.PlanDescription,
	HP.Available,
	HP.ServerID,
	HP.SetupPrice,
	HP.RecurringPrice,
	HP.RecurrenceLength,
	HP.RecurrenceUnit,
	HP.IsAddon
FROM
	HostingPlans AS HP
WHERE HP.UserID = @OwnerID
AND HP.IsAddon = 1
ORDER BY PlanName
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetUserAvailableHostingPlans
(
	@ActorID int,
	@UserID int
)
AS

-- user should see the plans only of his reseller
-- also user can create packages based on his own plans (admins and resellers)

DECLARE @Plans TABLE
(
	PlanID int
)

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @OwnerID int
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @UserID

SELECT
	HP.PlanID,
	HP.PackageID,
	HP.PlanName,
	HP.PlanDescription,
	HP.Available,
	HP.ServerID,
	HP.SetupPrice,
	HP.RecurringPrice,
	HP.RecurrenceLength,
	HP.RecurrenceUnit,
	HP.IsAddon
FROM
	HostingPlans AS HP
WHERE HP.UserID = @OwnerID
AND HP.IsAddon = 0
ORDER BY PlanName
RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[GetUserById]
(
	@ActorID int,
	@UserID int
)
AS
	-- user can retrieve his own account, his users accounts
	-- and his reseller account (without pasword)
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
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.UserID = @UserID
	AND dbo.CanGetUserDetails(@ActorID, @UserID) = 1 -- actor user rights

	RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













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
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.UserID = @UserID

	RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










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
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.Username = @Username
	AND dbo.CanGetUserDetails(@ActorID, UserID) = 1 -- actor user rights

	RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








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
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.Username = @Username

	RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO












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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































/*
Algorythm:
	0. Get the primary distribution resource from hosting plan
	1. Check whether user has Resource of requested type in his user plans/add-ons
		EXCEPTION "The requested service is not available for the user. The resource of the requested type {type} should be assigned to him through hosting plan or add-on"
		1.1 If the number of returned reources is greater than 1
			EXCEPTION "User has several resources assigned of the requested type"

	2. If the requested resource has 0 services
		EXCEPTION "The resource {name} of type {type} should contain atleast one service
	3. If the requested resource has one service
		remember the ID of this single service
	4. If the requested resource has several services DO distribution:

		4.1. If the resource is NOT BOUNDED or is PRIMARY DISTRIBUTION RESOURCE
			if PRIMARY DISTRIBUTION RESOURCE and exists in UserServices
				return serviceId from UserServices table

			remember any service from that resource according to distribution type ("BALANCED" or "RANDOM") - get the number of ServiceItems for each service

		4.2. If the resource is BOUNDED to primary distribution resource
			- If the primary distribution resource is NULL
			EXCEPTION "Requested resource marked as bound to primary distribution resource, but there is no any resources in hosting plan marked as primary"

			- Get the service id of the primary distribution resource
			GetServiceId(userId, primaryResourceId)


		Get from user assigned hosting plan

	5. If it is PRIMARY DISTRIBUTION RESOURCE
		Save it's ID to UserServices table

	6. return serviceId

ERROR CODES:
	-1 - there are several hosting plans with PDR assigned to that user
	-2 - The requested service is not available for the user. The resource of the
		requested type {type} should be assigned to him through hosting plan or add-on
	-3 - several resources of the same type was assigned through hosting plan or add-on
	-4 - The resource {name} of type {type} should contain atleast one service
	-5 - Requested resource marked as bound to primary distribution resource,
		but there is no any resources in hosting plan marked as primary
	-6 - the server where PDR is located doesn't contain the service of requested resource type
*/
CREATE PROCEDURE GetUserServiceID
(
	@UserID int,
	@TypeName nvarchar(1000),
	@ServiceID int OUTPUT
)
AS
	DECLARE @PrimaryResourceID int -- primary distribution resource assigned through hosting plan

	----------------------------------------
	-- Get the primary distribution resource
	----------------------------------------
	IF (SELECT COUNT (HP.PrimaryResourceID) FROM PurchasedHostingPlans AS PHP
	INNER JOIN HostingPlans AS HP ON PHP.PlanID = HP.PlanID
	WHERE PHP.UserID = @UserID AND HP.PrimaryResourceID IS NOT NULL AND HP.PrimaryResourceID <> 0) > 1
	BEGIN
		SET @ServiceID = -1
		RETURN
	END

	SELECT @PrimaryResourceID = HP.PrimaryResourceID FROM PurchasedHostingPlans AS PHP
	INNER JOIN HostingPlans AS HP ON PHP.PlanID = HP.PlanID
	WHERE PHP.UserID = @UserID AND HP.PrimaryResourceID IS NOT NULL AND HP.PrimaryResourceID <> 0


	----------------------------------------------
	-- Check whether user has a resource
	-- of this type in his hosting plans or addons
	----------------------------------------------
	DECLARE @UserResourcesTable TABLE
	(
		ResourceID int
	)
	INSERT INTO @UserResourcesTable
	SELECT DISTINCT HPR.ResourceID FROM PurchasedHostingPlans AS PHP
		INNER JOIN HostingPlans AS HP ON PHP.PlanID = HP.PlanID
		INNER JOIN HostingPlanResources AS HPR ON HP.PlanID = HPR.PlanID
		INNER JOIN Resources AS R ON HPR.ResourceID = R.ResourceID
		INNER JOIN ServiceTypes AS ST ON R.ServiceTypeID = ST.ServiceTypeID
		WHERE PHP.UserID = @UserID AND (ST.ImplementedTypeNames LIKE @TypeName OR ST.TypeName LIKE @TypeName)

	----------------------------------------
	-- Check resources number
	----------------------------------------
	DECLARE @ResourcesCount int
	SET @ResourcesCount = @@ROWCOUNT
	IF @ResourcesCount = 0
	BEGIN
		SET @ServiceID = -2 -- user doesn't have requested service assigned
		RETURN
	END
	IF @ResourcesCount > 1
	BEGIN
		SET @ServiceID = -3 -- several resources of the same type was assigned
		RETURN
	END

	----------------------------------------
	-- Check services number
	----------------------------------------
	DECLARE @ResourceID int
	SET @ResourceID = (SELECT TOP 1 ResourceID FROM @UserResourcesTable)

	DECLARE @UserServicesTable TABLE
	(
		ServiceID int,
		ServerID int,
		ItemsNumber int,
		Randomizer float
	)
	INSERT INTO @UserServicesTable
	SELECT
		RS.ServiceID,
		S.ServerID,
		(SELECT COUNT(ItemID) FROM ServiceItems AS SI WHERE SI.ServiceID = RS.ServiceID),
		RAND()
	FROM ResourceServices AS RS
	INNER JOIN Services AS S ON RS.ServiceID = S.ServiceID
	WHERE RS.ResourceID = @ResourceID

	DECLARE @ServicesCount int
	SET @ServicesCount = @@ROWCOUNT
	IF @ServicesCount = 0
	BEGIN
		SET @ServiceID = -4 -- The resource {name} of type {type} should contain atleast one service
		RETURN
	END

	-- try to return from UserServices
	-- if it is a PDR
	IF @ResourceID = @PrimaryResourceID
	BEGIN
		-- check in UserServices table
		SELECT @ServiceID = US.ServiceID FROM ResourceServices AS RS
		INNER JOIN UserServices AS US ON RS.ServiceID = US.ServiceID
		WHERE RS.ResourceID = @ResourceID AND US.UserID = @UserID

		-- check validness of the current primary service id
		IF @ServiceID IS NOT NULL
		BEGIN
			IF EXISTS(SELECT ResourceServiceID FROM ResourceServices
			WHERE ResourceID = @ResourceID AND ServiceID = @ServiceID)
				RETURN
			ELSE -- invalidate service
				DELETE FROM UserServices WHERE UserID = @UserID
		END
	END

	IF @ServicesCount = 1
	BEGIN
		-- nothing to distribute
		-- just remember this single service id
		SET @ServiceID = (SELECT TOP 1 ServiceID FROM @UserServicesTable)
	END
	ELSE
	BEGIN
		-- the service should be distributed
		DECLARE @DistributionTypeID int
		DECLARE @BoundToPrimaryResource bit
		SELECT @DistributionTypeID = R.DistributionTypeID, @BoundToPrimaryResource = R.BoundToPrimaryResource
		FROM Resources AS R WHERE R.ResourceID = @ResourceID

		IF @BoundToPrimaryResource = 0 OR @ResourceID = @PrimaryResourceID
		BEGIN
			IF @ResourceID = @PrimaryResourceID -- it's PDR itself
			BEGIN
				-- check in UserServices table
				SELECT @ServiceID = US.ServiceID FROM ResourceServices AS RS
				INNER JOIN UserServices AS US ON RS.ServiceID = US.ServiceID
				WHERE RS.ResourceID = @ResourceID AND US.UserID = @UserID

				-- check validness of the current primary service id
				IF @ServiceID IS NOT NULL
				BEGIN
					IF EXISTS(SELECT ResourceServiceID FROM ResourceServices
					WHERE ResourceID = @ResourceID AND ServiceID = @ServiceID)
						RETURN
					ELSE -- invalidate service
						DELETE FROM UserServices WHERE UserID = @UserID
				END
			END

			-- distribute
			IF @DistributionTypeID = 1 -- BALANCED distribution
				SELECT @ServiceID = ServiceID FROM @UserServicesTable
				ORDER BY ItemsNumber ASC
			ELSE -- RANDOM distribution
				SELECT @ServiceID = ServiceID FROM @UserServicesTable
				ORDER BY Randomizer
		END
		ELSE -- BOUND to PDR resource
		BEGIN
			IF @PrimaryResourceID IS NULL
			BEGIN
				SET @ServiceID = -5 -- Requested resource marked as bound to primary distribution resource,
									-- but there is no any resources in hosting plan marked as primary
				RETURN
			END

			-- get the type of primary resource
			DECLARE @PrimaryTypeName nvarchar(200)
			SELECT @PrimaryTypeName = ST.TypeName FROM  Resources AS R
			INNER JOIN ServiceTypes AS ST ON R.ServiceTypeID = ST.ServiceTypeID
			WHERE R.ResourceID = @PrimaryResourceID


			DECLARE @PrimaryServiceID int
			EXEC GetUserServiceID @UserID, @PrimaryTypeName, @PrimaryServiceID OUTPUT

			IF @PrimaryServiceID < 0
			BEGIN
				SET @ServiceID = @PrimaryServiceID
				RETURN
			END

			DECLARE @ServerID int
			SET @ServerID = (SELECT ServerID FROM Services WHERE ServiceID = @PrimaryServiceID)

			-- try to get the service of the requested type on PDR server
			SET @ServiceID = (SELECT ServiceID FROM @UserServicesTable WHERE ServerID = @ServerID)

			IF @ServiceID IS NULL
			BEGIN
				SET @ServiceID = -6 -- the server where PDR is located doesn't contain the service of requested resource type
			END
		END
	END

	IF @ResourceID = @PrimaryResourceID -- it's PDR
	BEGIN
		DELETE FROM UserServices WHERE UserID = @UserID

		INSERT INTO UserServices (UserID, ServiceID)
		VALUES (@UserID, @ServiceID)
	END

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetUserSettings
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetUsersSummary
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



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetVirtualMachinesPaged]
(
	@ActorID int,
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Recursive bit
)
AS


-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
SI.ItemTypeID = 33 -- VPS
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'SI.ItemName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(SI.ItemID) FROM Packages AS P
INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
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
	INNER JOIN Users AS U ON P.UserID = U.UserID
	LEFT OUTER JOIN (
		SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
	) AS EIP ON SI.ItemID = EIP.ItemID
	LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
	WHERE ' + @condition + '
)

INSERT INTO @Items
SELECT ItemID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	SI.ItemID,
	SI.ItemName,
	SI.PackageID,
	P.PackageName,
	P.UserID,
	U.Username,

	EIP.ExternalIP,
	PIP.IPAddress
FROM @Items AS TSI
INNER JOIN ServiceItems AS SI ON TSI.ItemID = SI.ItemID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
'

--print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

RETURN




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetVirtualMachinesPagedForPC]
(
	@ActorID int,
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Recursive bit
)
AS


-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
SI.ItemTypeID = 35 -- VPS
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'SI.ItemName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(SI.ItemID) FROM Packages AS P
INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
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
	INNER JOIN Users AS U ON P.UserID = U.UserID
	LEFT OUTER JOIN (
		SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
	) AS EIP ON SI.ItemID = EIP.ItemID
	LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
	WHERE ' + @condition + '
)

INSERT INTO @Items
SELECT ItemID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	SI.ItemID,
	SI.ItemName,
	SI.PackageID,
	P.PackageName,
	P.UserID,
	U.Username,

	EIP.ExternalIP,
	PIP.IPAddress
FROM @Items AS TSI
INNER JOIN ServiceItems AS SI ON TSI.ItemID = SI.ItemID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
'

--print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

RETURN



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























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









































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













CREATE PROCEDURE [dbo].[GetVirtualServices]
(
	@ActorID int,
	@ServerID int
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
	@IsAdmin = 1 AND (ShowGroup = 1)
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
	AND @IsAdmin = 1

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE PROCEDURE [dbo].[InsertCRMUser]
	@ItemID int,
	@CrmUserID uniqueidentifier,
	@BusinessUnitID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO
	CRMUsers
(
	AccountID,
	CRMUserGuid,
	BusinessUnitID
)
VALUES
(
	@ItemID,
	@CrmUserID,
	@BusinessUnitID
)



END



























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO




CREATE PROCEDURE [dbo].[LyncUserExists]
(
	@AccountID int,
	@SipAddress nvarchar(300),
	@Exists bit OUTPUT
)
AS

	SET @Exists = 0
	IF EXISTS(SELECT * FROM [dbo].[ExchangeAccountEmailAddresses] WHERE [EmailAddress] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [PrimaryEmailAddress] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [UserPrincipalName] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [AccountName] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[LyncUsers] WHERE [SipAddress] = @SipAddress)
		BEGIN
			SET @Exists = 1
		END


	RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[SfBUserExists]
(
	@AccountID int,
	@SipAddress nvarchar(300),
	@Exists bit OUTPUT
)
AS

	SET @Exists = 0
	IF EXISTS(SELECT * FROM [dbo].[ExchangeAccountEmailAddresses] WHERE [EmailAddress] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [PrimaryEmailAddress] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [UserPrincipalName] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [AccountName] = @SipAddress AND [AccountID] <> @AccountID)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[SfBUsers] WHERE [SipAddress] = @SipAddress)
		BEGIN
			SET @Exists = 1
		END


	RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO







CREATE PROCEDURE [dbo].[MoveServiceItem]
(
	@ActorID int,
	@ItemID int,
	@DestinationServiceID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE ServiceItems
SET ServiceID = @DestinationServiceID
WHERE ItemID = @ItemID

COMMIT TRAN

RETURN













GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[OrganizationExists]
(
	@OrganizationID nvarchar(10),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM Organizations WHERE OrganizationID = @OrganizationID)
BEGIN
	SET @Exists = 1
END

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[OrganizationUserExists]
(
	@LoginName nvarchar(20),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeAccounts WHERE AccountName = @LoginName)
BEGIN
	SET @Exists = 1
END

RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO












CREATE PROCEDURE [dbo].[SearchExchangeAccount]
(
      @ActorID int,
      @AccountType int,
      @PrimaryEmailAddress nvarchar(300)
)
AS

DECLARE @PackageID int
DECLARE @ItemID int
DECLARE @AccountID int

SELECT
      @AccountID = AccountID,
      @ItemID = ItemID
FROM ExchangeAccounts
WHERE PrimaryEmailAddress = @PrimaryEmailAddress
AND AccountType = @AccountType


-- check space rights
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	AccountID,
	ItemID,
	@PackageID AS PackageID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder,
	MailboxManagerActions,
	SamAccountName,
	AccountPassword,
	SubscriberNumber,
	UserPrincipalName
FROM ExchangeAccounts
WHERE AccountID = @AccountID

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE [dbo].[SearchExchangeAccounts]
(
	@ActorID int,
	@ItemID int,
	@IncludeMailboxes bit,
	@IncludeContacts bit,
	@IncludeDistributionLists bit,
	@IncludeRooms bit,
	@IncludeEquipment bit,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50)
)
AS
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
((@IncludeMailboxes = 1 AND EA.AccountType = 1)
OR (@IncludeContacts = 1 AND EA.AccountType = 2)
OR (@IncludeDistributionLists = 1 AND EA.AccountType = 3)
OR (@IncludeRooms = 1 AND EA.AccountType = 5)
OR (@IncludeEquipment = 1 AND EA.AccountType = 6))
AND EA.ItemID = @ItemID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'EA.DisplayName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT
	EA.AccountID,
	EA.ItemID,
	EA.AccountType,
	EA.AccountName,
	EA.DisplayName,
	EA.PrimaryEmailAddress,
	EA.MailEnabledPublicFolder,
	EA.SubscriberNumber,
	EA.UserPrincipalName
FROM ExchangeAccounts AS EA
WHERE ' + @condition

print @sql

exec sp_executesql @sql, N'@ItemID int, @IncludeMailboxes int, @IncludeContacts int,
    @IncludeDistributionLists int, @IncludeRooms bit, @IncludeEquipment bit',
@ItemID, @IncludeMailboxes, @IncludeContacts, @IncludeDistributionLists, @IncludeRooms, @IncludeEquipment

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


















CREATE PROCEDURE [dbo].[SearchOrganizationAccounts]
(
	@ActorID int,
	@ItemID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@IncludeMailboxes bit
)
AS
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
(EA.AccountType = 7 OR (EA.AccountType = 1 AND @IncludeMailboxes = 1)  )
AND EA.ItemID = @ItemID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'EA.DisplayName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT
	EA.AccountID,
	EA.ItemID,
	EA.AccountType,
	EA.AccountName,
	EA.DisplayName,
	EA.PrimaryEmailAddress,
	EA.SubscriberNumber,
	EA.UserPrincipalName
FROM ExchangeAccounts AS EA
WHERE ' + @condition

print @sql

exec sp_executesql @sql, N'@ItemID int, @IncludeMailboxes bit',
@ItemID, @IncludeMailboxes

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






CREATE PROCEDURE [dbo].[SearchServiceItemsPaged]
(
	@ActorID int,
	@UserID int,
	@ItemTypeID int,
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS


-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

IF @ItemTypeID <> 13
BEGIN
	SET @sql = '
	DECLARE @EndRow int
	SET @EndRow = @StartRow + @MaximumRows
	DECLARE @Items TABLE
	(
		ItemPosition int IDENTITY(1,1),
		ItemID int
	)
	INSERT INTO @Items (ItemID)
	SELECT
		SI.ItemID
	FROM ServiceItems AS SI
	INNER JOIN Packages AS P ON P.PackageID = SI.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE
		dbo.CheckUserParent(@UserID, P.UserID) = 1
		AND SI.ItemTypeID = @ItemTypeID
	'

	IF @FilterValue <> ''
	SET @sql = @sql + ' AND SI.ItemName LIKE @FilterValue '

	IF @SortColumn = '' OR @SortColumn IS NULL
	SET @SortColumn = 'ItemName'

	SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

	SET @sql = @sql + ' SELECT COUNT(ItemID) FROM @Items;
	SELECT

		SI.ItemID,
		SI.ItemName,

		P.PackageID,
		P.PackageName,
		P.StatusID,
		P.PurchaseDate,

		-- user
		P.UserID,
		U.Username,
		U.FirstName,
		U.LastName,
		U.FullName,
		U.RoleID,
		U.Email
	FROM @Items AS I
	INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
	INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE I.ItemPosition BETWEEN @StartRow AND @EndRow'
END
ELSE
BEGIN

	SET @SortColumn = REPLACE(@SortColumn, 'ItemName', 'DomainName')

	SET @sql = '
	DECLARE @EndRow int
	SET @EndRow = @StartRow + @MaximumRows
	DECLARE @Items TABLE
	(
		ItemPosition int IDENTITY(1,1),
		ItemID int
	)
	INSERT INTO @Items (ItemID)
	SELECT
		D.DomainID
	FROM Domains AS D
	INNER JOIN Packages AS P ON P.PackageID = D.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE
		dbo.CheckUserParent(@UserID, P.UserID) = 1
	'

	IF @FilterValue <> ''
	SET @sql = @sql + ' AND D.DomainName LIKE @FilterValue '

	IF @SortColumn = '' OR @SortColumn IS NULL
	SET @SortColumn = 'DomainName'

	SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

	SET @sql = @sql + ' SELECT COUNT(ItemID) FROM @Items;
	SELECT

		D.DomainID AS ItemID,
		D.DomainName AS ItemName,

		P.PackageID,
		P.PackageName,
		P.StatusID,
		P.PurchaseDate,

		-- user
		P.UserID,
		U.Username,
		U.FirstName,
		U.LastName,
		U.FullName,
		U.RoleID,
		U.Email
	FROM @Items AS I
	INNER JOIN Domains AS D ON I.ItemID = D.DomainID
	INNER JOIN Packages AS P ON D.PackageID = P.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE I.ItemPosition BETWEEN @StartRow AND @EndRow AND D.IsDomainPointer=0'
END

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ItemTypeID int, @ActorID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ItemTypeID, @ActorID

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE PROCEDURE [dbo].[SetExchangeAccountMailboxplan]
(
	@AccountID int,
	@MailboxPlanId int
)
AS

UPDATE ExchangeAccounts SET
	MailboxPlanId = @MailboxPlanId
WHERE
	AccountID = @AccountID

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[SetItemPrimaryIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PackageAddressID int
)
AS
BEGIN

	-- read item pool
	DECLARE @PoolID int
	SELECT @PoolID = IP.PoolID FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.PackageAddressID = @PackageAddressID

	-- update all IP addresses of the specified pool
	UPDATE PackageIPAddresses
	SET IsPrimary = CASE PIP.PackageAddressID WHEN @PackageAddressID THEN 1 ELSE 0 END
	FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.ItemID = @ItemID
	AND IP.PoolID = @PoolID
	AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[SetItemPrivatePrimaryIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PrivateAddressID int
)
AS
BEGIN
	UPDATE PrivateIPAddresses
	SET IsPrimary = CASE PIP.PrivateAddressID WHEN @PrivateAddressID THEN 1 ELSE 0 END
	FROM PrivateIPAddresses AS PIP
	INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
	WHERE PIP.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[SetLyncUserLyncUserPlan]
(
	@AccountID int,
	@LyncUserPlanId int
)
AS

UPDATE LyncUsers SET
	LyncUserPlanId = @LyncUserPlanId
WHERE
	AccountID = @AccountID

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[SetSfBUserSfBUserPlan]
(
	@AccountID int,
	@SfBUserPlanId int
)
AS

UPDATE SfBUsers SET
	SfBUserPlanId = @SfBUserPlanId
WHERE
	AccountID = @AccountID

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[SetOrganizationDefaultExchangeMailboxPlan]
(
	@ItemID int,
	@MailboxPlanId int
)
AS

UPDATE ExchangeOrganizations SET
	ExchangeMailboxPlanID = @MailboxPlanId
WHERE
	ItemID = @ItemID

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








CREATE PROCEDURE [dbo].[SetOrganizationDefaultLyncUserPlan]
(
	@ItemID int,
	@LyncUserPlanId int
)
AS

UPDATE ExchangeOrganizations SET
	LyncUserPlanID = @LyncUserPlanId
WHERE
	ItemID = @ItemID

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
























CREATE PROCEDURE [dbo].[SetOrganizationDefaultSfBUserPlan]
(
	@ItemID int,
	@SfBUserPlanId int
)
AS

UPDATE ExchangeOrganizations SET
	SfBUserPlanID = @SfBUserPlanId
WHERE
	ItemID = @ItemID

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























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
*/
	SET NOCOUNT ON;

	BEGIN TRAN
		DECLARE @idoc int;
		--Create an internal representation of the XML document.
		EXEC sp_xml_preparedocument @idoc OUTPUT, @xml;

		DELETE FROM [dbo].[SystemSettings] WHERE [SettingsName] = @SettingsName;

		INSERT INTO [dbo].[SystemSettings]
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




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO










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


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO





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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


















CREATE PROCEDURE [dbo].[UpdateExchangeAccount]
(
	@AccountID int,
	@AccountName nvarchar(300),
	@DisplayName nvarchar(300),
	@PrimaryEmailAddress nvarchar(300),
	@AccountType int,
	@SamAccountName nvarchar(100),
	@MailEnabledPublicFolder bit,
	@MailboxManagerActions varchar(200),
	@Password varchar(200),
	@MailboxPlanId int,
	@SubscriberNumber varchar(32)
)
AS

BEGIN TRAN

IF (@MailboxPlanId = -1)
BEGIN
	SET @MailboxPlanId = NULL
END

UPDATE ExchangeAccounts SET
	AccountName = @AccountName,
	DisplayName = @DisplayName,
	PrimaryEmailAddress = @PrimaryEmailAddress,
	MailEnabledPublicFolder = @MailEnabledPublicFolder,
	MailboxManagerActions = @MailboxManagerActions,
	AccountType =@AccountType,
	SamAccountName = @SamAccountName,
	MailboxPlanId = @MailboxPlanId,
	SubscriberNumber = @SubscriberNumber

WHERE
	AccountID = @AccountID

IF (@@ERROR <> 0 )
	BEGIN
		ROLLBACK TRANSACTION
		RETURN -1
	END

UPDATE ExchangeAccounts SET
	AccountPassword = @Password WHERE AccountID = @AccountID AND @Password IS NOT NULL

IF (@@ERROR <> 0 )
	BEGIN
		ROLLBACK TRANSACTION
		RETURN -1
	END
COMMIT TRAN
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
 CREATE PROCEDURE [dbo].[UpdateExchangeAccountUserPrincipalName]
(
	@AccountID int,
	@UserPrincipalName nvarchar(300)
)
AS

UPDATE ExchangeAccounts SET
	UserPrincipalName = @UserPrincipalName
WHERE
	AccountID = @AccountID

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[UpdateExchangeMailboxPlan]
(
	@MailboxPlanId int,
	@MailboxPlan	nvarchar(300),
	@EnableActiveSync bit,
	@EnableIMAP bit,
	@EnableMAPI bit,
	@EnableOWA bit,
	@EnablePOP bit,
	@IsDefault bit,
	@IssueWarningPct int,
	@KeepDeletedItemsDays int,
	@MailboxSizeMB int,
	@MaxReceiveMessageSizeKB int,
	@MaxRecipients int,
	@MaxSendMessageSizeKB int,
	@ProhibitSendPct int,
	@ProhibitSendReceivePct int	,
	@HideFromAddressBook bit,
	@MailboxPlanType int
)
AS

UPDATE ExchangeMailboxPlans SET
	MailboxPlan = @MailboxPlan,
	EnableActiveSync = @EnableActiveSync,
	EnableIMAP = @EnableIMAP,
	EnableMAPI = @EnableMAPI,
	EnableOWA = @EnableOWA,
	EnablePOP = @EnablePOP,
	IsDefault = @IsDefault,
	IssueWarningPct= @IssueWarningPct,
	KeepDeletedItemsDays = @KeepDeletedItemsDays,
	MailboxSizeMB= @MailboxSizeMB,
	MaxReceiveMessageSizeKB= @MaxReceiveMessageSizeKB,
	MaxRecipients= @MaxRecipients,
	MaxSendMessageSizeKB= @MaxSendMessageSizeKB,
	ProhibitSendPct= @ProhibitSendPct,
	ProhibitSendReceivePct = @ProhibitSendReceivePct,
	HideFromAddressBook = @HideFromAddressBook,
	MailboxPlanType = @MailboxPlanType
WHERE MailboxPlanId = @MailboxPlanId

RETURN









































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdateHostingPlanQuotas
(
	@ActorID int,
	@PlanID int,
	@Xml ntext
)
AS

/*
XML Format:

<plan>
	<groups>
		<group id="16" enabled="1" calculateDiskSpace="1" calculateBandwidth="1"/>
	</groups>
	<quotas>
		<quota id="2" value="2"/>
	</quotas>
</plan>

*/

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM HostingPlans
WHERE PlanID = @PlanID

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- delete old HP resources
DELETE FROM HostingPlanResources
WHERE PlanID = @PlanID

-- delete old HP quotas
DELETE FROM HostingPlanQuotas
WHERE PlanID = @PlanID

-- update HP resources
INSERT INTO HostingPlanResources
(
	PlanID,
	GroupID,
	CalculateDiskSpace,
	CalculateBandwidth
)
SELECT
	@PlanID,
	GroupID,
	CalculateDiskSpace,
	CalculateBandwidth
FROM OPENXML(@idoc, '/plan/groups/group',1) WITH
(
	GroupID int '@id',
	CalculateDiskSpace bit '@calculateDiskSpace',
	CalculateBandwidth bit '@calculateBandwidth'
) as XRG

-- update HP quotas
INSERT INTO HostingPlanQuotas
(
	PlanID,
	QuotaID,
	QuotaValue
)
SELECT
	@PlanID,
	QuotaID,
	QuotaValue
FROM OPENXML(@idoc, '/plan/quotas/quota',1) WITH
(
	QuotaID int '@id',
	QuotaValue int '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

RETURN











GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE UpdateHostingPlan
(
	@ActorID int,
	@PlanID int,
	@PackageID int,
	@ServerID int,
	@PlanName nvarchar(200),
	@PlanDescription ntext,
	@Available bit,
	@SetupPrice money,
	@RecurringPrice money,
	@RecurrenceLength int,
	@RecurrenceUnit int,
	@QuotasXml ntext
)
AS

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM HostingPlans
WHERE PlanID = @PlanID

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

IF @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

IF @PackageID = 0 SET @PackageID = NULL
IF @ServerID = 0 SET @ServerID = NULL

-- update record
UPDATE HostingPlans SET
	PackageID = @PackageID,
	ServerID = @ServerID,
	PlanName = @PlanName,
	PlanDescription = @PlanDescription,
	Available = @Available,
	SetupPrice = @SetupPrice,
	RecurringPrice = @RecurringPrice,
	RecurrenceLength = @RecurrenceLength,
	RecurrenceUnit = @RecurrenceUnit
WHERE PlanID = @PlanID

BEGIN TRAN

-- update quotas
EXEC UpdateHostingPlanQuotas @ActorID, @PlanID, @QuotasXml

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@PackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN

RETURN




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE AddHostingPlan
(
	@ActorID int,
	@PlanID int OUTPUT,
	@UserID int,
	@PackageID int,
	@PlanName nvarchar(200),
	@PlanDescription ntext,
	@Available bit,
	@ServerID int,
	@SetupPrice money,
	@RecurringPrice money,
	@RecurrenceLength int,
	@RecurrenceUnit int,
	@IsAddon bit,
	@QuotasXml ntext
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

BEGIN TRAN

IF @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

IF @IsAddon = 1
SET @ServerID = NULL

IF @PackageID = 0 SET @PackageID = NULL

INSERT INTO HostingPlans
(
	UserID,
	PackageID,
	PlanName,
	PlanDescription,
	Available,
	ServerID,
	SetupPrice,
	RecurringPrice,
	RecurrenceLength,
	RecurrenceUnit,
	IsAddon
)
VALUES
(
	@UserID,
	@PackageID,
	@PlanName,
	@PlanDescription,
	@Available,
	@ServerID,
	@SetupPrice,
	@RecurringPrice,
	@RecurrenceLength,
	@RecurrenceUnit,
	@IsAddon
)

SET @PlanID = SCOPE_IDENTITY()

-- save quotas
EXEC UpdateHostingPlanQuotas @ActorID, @PlanID, @QuotasXml

COMMIT TRAN
RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[UpdateIPAddress]
(
	@AddressID int,
	@ServerID int,
	@ExternalIP varchar(24),
	@InternalIP varchar(24),
	@PoolID int,
	@SubnetMask varchar(15),
	@DefaultGateway varchar(15),
	@Comments ntext
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
		Comments = @Comments
	WHERE AddressID = @AddressID

	RETURN
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[UpdateIPAddresses]
(
	@xml ntext,
	@PoolID int,
	@ServerID int,
	@SubnetMask varchar(15),
	@DefaultGateway varchar(15),
	@Comments ntext
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
		Comments = @Comments
	FROM IPAddresses AS IP
	INNER JOIN OPENXML(@idoc, '/items/item', 1) WITH
	(
		AddressID int '@id'
	) as PV ON IP.AddressID = PV.AddressID

	-- remove document
	exec sp_xml_removedocument @idoc
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[UpdateLyncUser]
(
	@AccountID int,
	@SipAddress nvarchar(300)
)
AS

UPDATE LyncUsers SET
	SipAddress = @SipAddress
WHERE
	AccountID = @AccountID

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[UpdateLyncUserPlan]
(
	@LyncUserPlanId int,
	@LyncUserPlanName	nvarchar(300),
	@LyncUserPlanType int,
	@IM bit,
	@Mobility bit,
	@MobilityEnableOutsideVoice bit,
	@Federation bit,
	@Conferencing bit,
	@EnterpriseVoice bit,
	@VoicePolicy int,
	@IsDefault bit
)
AS

UPDATE LyncUserPlans SET
	LyncUserPlanName = @LyncUserPlanName,
	LyncUserPlanType = @LyncUserPlanType,
	IM = @IM,
	Mobility = @Mobility,
	MobilityEnableOutsideVoice = @MobilityEnableOutsideVoice,
	Federation = @Federation,
	Conferencing =@Conferencing,
	EnterpriseVoice = @EnterpriseVoice,
	VoicePolicy = @VoicePolicy,
	IsDefault = @IsDefault
WHERE LyncUserPlanId = @LyncUserPlanId


RETURN









GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE PROCEDURE [dbo].[UpdateSfBUser]
(
	@AccountID int,
	@SipAddress nvarchar(300)
)
AS

UPDATE SfBUsers SET
	SipAddress = @SipAddress
WHERE
	AccountID = @AccountID

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[UpdateSfBUserPlan]
(
	@SfBUserPlanId int,
	@SfBUserPlanName	nvarchar(300),
	@SfBUserPlanType int,
	@IM bit,
	@Mobility bit,
	@MobilityEnableOutsideVoice bit,
	@Federation bit,
	@Conferencing bit,
	@EnterpriseVoice bit,
	@VoicePolicy int,
	@IsDefault bit
)
AS

UPDATE SfBUserPlans SET
	SfBUserPlanName = @SfBUserPlanName,
	SfBUserPlanType = @SfBUserPlanType,
	IM = @IM,
	Mobility = @Mobility,
	MobilityEnableOutsideVoice = @MobilityEnableOutsideVoice,
	Federation = @Federation,
	Conferencing =@Conferencing,
	EnterpriseVoice = @EnterpriseVoice,
	VoicePolicy = @VoicePolicy,
	IsDefault = @IsDefault
WHERE SfBUserPlanId = @SfBUserPlanId


RETURN









GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE UpdatePackageAddon
(
	@ActorID int,
	@PackageAddonID int,
	@PlanID int,
	@Quantity int,
	@PurchaseDate datetime,
	@StatusID int,
	@Comments ntext
)
AS

DECLARE @PackageID int
SELECT @PackageID = PackageID FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

DECLARE @ParentPackageID int
SELECT @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

-- update record
UPDATE PackageAddons SET
	PlanID = @PlanID,
	Quantity = @Quantity,
	PurchaseDate = @PurchaseDate,
	StatusID = @StatusID,
	Comments = @Comments
WHERE PackageAddonID = @PackageAddonID

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdatePackageBandwidth
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







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdatePackageBandwidthUpdate
(
	@PackageID int,
	@UpdateDate datetime
)
AS

UPDATE Packages SET BandwidthUpdated = @UpdateDate
WHERE PackageID = @PackageID

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE UpdatePackageDiskSpace
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


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE UpdatePackageName
(
	@ActorID int,
	@PackageID int,
	@PackageName nvarchar(300),
	@PackageComments ntext
)
AS

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

IF NOT(dbo.CheckActorPackageRights(@ActorID, @PackageID) = 1
	OR @UserID = @ActorID
	OR EXISTS(SELECT UserID FROM Users WHERE UserID = @ActorID AND OwnerID = @UserID AND IsPeer = 1))
RAISERROR('You are not allowed to access this package', 16, 1)

-- update package
UPDATE Packages SET
	PackageName = @PackageName,
	PackageComments = @PackageComments
WHERE
	PackageID = @PackageID

RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdatePackageQuotas
(
	@ActorID int,
	@PackageID int,
	@Xml ntext
)
AS

/*
XML Format:

<plan>
	<groups>
		<group id="16" enabled="1" calculateDiskSpace="1" calculateBandwidth="1"/>
	</groups>
	<quotas>
		<quota id="2" value="2"/>
	</quotas>
</plan>

*/

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @OverrideQuotas bit
SELECT @OverrideQuotas = OverrideQuotas FROM Packages
WHERE PackageID = @PackageID

IF @OverrideQuotas = 0
BEGIN
	-- delete old Package resources
	DELETE FROM PackageResources
	WHERE PackageID = @PackageID

	-- delete old Package quotas
	DELETE FROM PackageQuotas
	WHERE PackageID = @PackageID
END

IF @OverrideQuotas = 1 AND @Xml IS NOT NULL
BEGIN
	-- delete old Package resources
	DELETE FROM PackageResources
	WHERE PackageID = @PackageID

	-- delete old Package quotas
	DELETE FROM PackageQuotas
	WHERE PackageID = @PackageID

	DECLARE @idoc int
	--Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

	-- update Package resources
	INSERT INTO PackageResources
	(
		PackageID,
		GroupID,
		CalculateDiskSpace,
		CalculateBandwidth
	)
	SELECT
		@PackageID,
		GroupID,
		CalculateDiskSpace,
		CalculateBandwidth
	FROM OPENXML(@idoc, '/plan/groups/group',1) WITH
	(
		GroupID int '@id',
		CalculateDiskSpace bit '@calculateDiskSpace',
		CalculateBandwidth bit '@calculateBandwidth'
	) as XRG

	-- update Package quotas
	INSERT INTO PackageQuotas
	(
		PackageID,
		QuotaID,
		QuotaValue
	)
	SELECT
		@PackageID,
		QuotaID,
		QuotaValue
	FROM OPENXML(@idoc, '/plan/quotas/quota',1) WITH
	(
		QuotaID int '@id',
		QuotaValue int '@value'
	) as PV

	-- remove document
	exec sp_xml_removedocument @idoc
END
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdatePackageSettings
(
	@ActorID int,
	@PackageID int,
	@SettingsName nvarchar(50),
	@Xml ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- delete old properties
BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM PackageSettings
WHERE PackageID = @PackageID AND SettingsName = @SettingsName

INSERT INTO PackageSettings
(
	PackageID,
	SettingsName,
	PropertyName,
	PropertyValue
)
SELECT
	@PackageID,
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdateSchedule
(
	@ActorID int,
	@ScheduleID int,
	@TaskID nvarchar(100),
	@ScheduleName nvarchar(100),
	@ScheduleTypeID nvarchar(50),
	@Interval int,
	@FromTime datetime,
	@ToTime datetime,
	@StartTime datetime,
	@LastRun datetime,
	@NextRun datetime,
	@Enabled bit,
	@PriorityID nvarchar(50),
	@HistoriesNumber int,
	@MaxExecutionTime int,
	@WeekMonthDay int,
	@XmlParameters ntext
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Schedule
WHERE ScheduleID = @ScheduleID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE Schedule
SET
	TaskID = @TaskID,
	ScheduleName = @ScheduleName,
	ScheduleTypeID = @ScheduleTypeID,
	Interval = @Interval,
	FromTime = @FromTime,
	ToTime = @ToTime,
	StartTime = @StartTime,
	LastRun = @LastRun,
	NextRun = @NextRun,
	Enabled = @Enabled,
	PriorityID = @PriorityID,
	HistoriesNumber = @HistoriesNumber,
	MaxExecutionTime = @MaxExecutionTime,
	WeekMonthDay = @WeekMonthDay
WHERE
	ScheduleID = @ScheduleID

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlParameters

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ScheduleParameters
WHERE ScheduleID = @ScheduleID

INSERT INTO ScheduleParameters
(
	ScheduleID,
	ParameterID,
	ParameterValue
)
SELECT
	@ScheduleID,
	ParameterID,
	ParameterValue
FROM OPENXML(@idoc, '/parameters/parameter',1) WITH
(
	ParameterID nvarchar(50) '@id',
	ParameterValue nvarchar(3000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdateServer
(
	@ServerID int,
	@ServerName nvarchar(100),
	@ServerUrl nvarchar(100),
	@Password nvarchar(100),
	@Comments ntext,
	@InstantDomainAlias nvarchar(200),
	@PrimaryGroupID int,
	@ADEnabled bit,
	@ADRootDomain nvarchar(200),
	@ADUsername nvarchar(100),
	@ADPassword nvarchar(100),
	@ADAuthenticationType varchar(50)
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
	ADAuthenticationType = @ADAuthenticationType
WHERE ServerID = @ServerID
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE UpdateService
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE UpdateServiceItem
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
	PropertyValue  nvarchar(3000))

INSERT INTO #TempTable (ItemID, PropertyName, PropertyValue)
SELECT
	@ItemID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(3000) '@value'
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






























CREATE PROCEDURE dbo.UpdateServiceProperties
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
	PropertyValue nvarchar(1000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO








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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















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






GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE UpdateUserSettings
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








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdateVirtualGroups
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

*/

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




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION CalculatePackageBandwidth
(
	@PackageID int
)
RETURNS int
AS
BEGIN

DECLARE @d datetime, @StartDate datetime, @EndDate datetime
SET @d = GETDATE()
SET @StartDate = DATEADD(Day, -DAY(@d) + 1, @d)
SET @EndDate = DATEADD(Day, -1, DATEADD(Month, 1, @StartDate))
--SET @EndDate =  GETDATE()
--SET @StartDate = DATEADD(month, -1, @EndDate)

-- remove hours and minutes
SET @StartDate = CONVERT(datetime, CONVERT(nvarchar, @StartDate, 112))
SET @EndDate = CONVERT(datetime, CONVERT(nvarchar, @EndDate, 112))

DECLARE @Bandwidth int
SELECT
	@Bandwidth = ROUND(CONVERT(float, SUM(ISNULL(PB.BytesSent + PB.BytesReceived, 0))) / 1024 / 1024, 0) -- in megabytes
FROM PackagesTreeCache AS PT
INNER JOIN Packages AS P ON PT.PackageID = P.PackageID
INNER JOIN PackagesBandwidth AS PB ON PT.PackageID = PB.PackageID
INNER JOIN HostingPlanResources AS HPR ON PB.GroupID = HPR.GroupID
	AND HPR.PlanID = P.PlanID AND HPR.CalculateBandwidth = 1
WHERE
	PT.ParentPackageID = @PackageID
	AND PB.LogDate BETWEEN @StartDate AND @EndDate

IF @Bandwidth IS NULL
SET @Bandwidth = 0

RETURN @Bandwidth
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION CalculatePackageDiskspace
(
	@PackageID int
)
RETURNS int
AS
BEGIN

DECLARE @Diskspace int

SELECT
	@Diskspace = ROUND(CONVERT(float, SUM(ISNULL(PD.DiskSpace, 0))) / 1024 / 1024, 0) -- in megabytes
FROM PackagesTreeCache AS PT
INNER JOIN Packages AS P ON PT.PackageID = P.PackageID
INNER JOIN PackagesDiskspace AS PD ON P.PackageID = PD.PackageID
INNER JOIN HostingPlanResources AS HPR ON PD.GroupID = HPR.GroupID
	AND HPR.PlanID = P.PlanID AND HPR.CalculateDiskspace = 1
WHERE PT.ParentPackageID = @PackageID

RETURN @Diskspace
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













CREATE FUNCTION [dbo].[CalculateQuotaUsage]
(
	@PackageID int,
	@QuotaID int
)
RETURNS int
AS
	BEGIN

		DECLARE @QuotaTypeID int
		SELECT @QuotaTypeID = QuotaTypeID FROM Quotas
		WHERE QuotaID = @QuotaID

		IF @QuotaTypeID <> 2
			RETURN 0

		DECLARE @Result int

		IF @QuotaID = 52 -- diskspace
			SET @Result = dbo.CalculatePackageDiskspace(@PackageID)
		ELSE IF @QuotaID = 51 -- bandwidth
			SET @Result = dbo.CalculatePackageBandwidth(@PackageID)
		ELSE IF @QuotaID = 53 -- domains
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsSubDomain = 0 AND IsInstantAlias = 0 AND IsDomainPointer = 0 AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 54 -- sub-domains
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsSubDomain = 1 AND IsInstantAlias = 0 AND IsDomainPointer = 0 AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 220 -- domain pointers
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsDomainPointer = 1 AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 71 -- scheduled tasks
			SET @Result = (SELECT COUNT(S.ScheduleID) FROM PackagesTreeCache AS PT
				INNER JOIN Schedule AS S ON S.PackageID = PT.PackageID
				WHERE PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 305 -- RAM of VPS
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'RamSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 306 -- HDD of VPS
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 309 -- External IP addresses of VPS
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 3)
		ELSE IF @QuotaID = 100 -- Dedicated Web IP addresses
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 2)
		ELSE IF @QuotaID = 350 -- RAM of VPSforPc
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'Memory' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 351 -- HDD of VPSforPc
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 354 -- External IP addresses of VPSforPc
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 3)
		ELSE IF @QuotaID = 319 -- BB Users
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts ea
							INNER JOIN BlackBerryUsers bu ON ea.AccountID = bu.AccountID
							INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
							INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
							WHERE pt.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 320 -- OCS Users
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts ea
							INNER JOIN OCSUsers ocs ON ea.AccountID = ocs.AccountID
							INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
							INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
							WHERE pt.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 206 -- HostedSolution.Users
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID AND ea.AccountType IN (1,5,6,7))
		ELSE IF @QuotaID = 78 -- Exchange2007.Mailboxes
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID
				AND ea.AccountType IN (1)
				AND ea.MailboxPlanId IS NOT NULL)
		ELSE IF @QuotaID = 77 -- Exchange2007.DiskSpace
			SET @Result = (SELECT SUM(B.MailboxSizeMB) FROM ExchangeAccounts AS ea
			INNER JOIN ExchangeMailboxPlans AS B ON ea.MailboxPlanId = B.MailboxPlanId
			INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
			INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
			WHERE pt.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 370 -- Lync.Users
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN LyncUsers lu ON ea.AccountID = lu.AccountID
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 376 -- Lync.EVUsers
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN LyncUsers lu ON ea.AccountID = lu.AccountID
				INNER JOIN LyncUserPlans lp ON lu.LyncUserPlanId = lp.LyncUserPlanId
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID AND lp.EnterpriseVoice = 1)
		ELSE IF @QuotaID = 570 -- SfB.Users
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN SfBUsers lu ON ea.AccountID = lu.AccountID
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 576 -- SfB.EVUsers
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN SfBUsers lu ON ea.AccountID = lu.AccountID
				INNER JOIN SfBUserPlans lp ON lu.SfBUserPlanId = lp.SfBUserPlanId
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID AND lp.EnterpriseVoice = 1)
		ELSE
			SET @Result = (SELECT COUNT(SI.ItemID) FROM Quotas AS Q
			INNER JOIN ServiceItems AS SI ON SI.ItemTypeID = Q.ItemTypeID
			INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID AND PT.ParentPackageID = @PackageID
			WHERE Q.QuotaID = @QuotaID)

		RETURN @Result
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[UpdatePackage]
(
	@ActorID int,
	@PackageID int,
	@PackageName nvarchar(300),
	@PackageComments ntext,
	@StatusID int,
	@PlanID int,
	@PurchaseDate datetime,
	@OverrideQuotas bit,
	@QuotasXml ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

DECLARE @ParentPackageID int
DECLARE @OldPlanID int

SELECT @ParentPackageID = ParentPackageID, @OldPlanID = PlanID FROM Packages
WHERE PackageID = @PackageID

-- update package
UPDATE Packages SET
	PackageName = @PackageName,
	PackageComments = @PackageComments,
	StatusID = @StatusID,
	PlanID = @PlanID,
	PurchaseDate = @PurchaseDate,
	OverrideQuotas = @OverrideQuotas
WHERE
	PackageID = @PackageID

-- update quotas (if required)
EXEC UpdatePackageQuotas @ActorID, @PackageID, @QuotasXml

-- check resulting quotas
DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)

-- check exceeding quotas if plan has been changed
IF (@OldPlanID <> @PlanID) OR (@OverrideQuotas = 1)
BEGIN
	INSERT INTO @ExceedingQuotas
	SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0
END

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END


COMMIT TRAN
RETURN

























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CanUpdatePackageDetails]
(
	@ActorID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

-- check if the user requests himself
IF @ActorID = @UserID
RETURN 1


DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
SET @ActorID = @OwnerID

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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CheckActorParentPackageRights]
(
	@ActorID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1 OR @PackageID IS NULL
RETURN 1

-- get package owner
DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

IF @UserID IS NULL
RETURN 1 -- unexisting package

-- check user
RETURN dbo.CanGetUserDetails(@ActorID, @UserID)

RETURN 0
END





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































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
*/
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





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE FUNCTION dbo.CheckExceedingQuota
(
	@PackageID int,
	@QuotaID int,
	@QuotaTypeID int
)
RETURNS int
AS
BEGIN

DECLARE @ExceedValue int
SET @ExceedValue = 0

DECLARE @PackageQuotaValue int
SET @PackageQuotaValue = dbo.GetPackageAllocatedQuota(@PackageID, @QuotaID)

-- check boolean quota
IF @QuotaTypeID = 1-- AND @PackageQuotaValue > 0 -- enabled
RETURN 0 -- can exceed

-- check numeric quota
IF @QuotaTypeID = 2 AND @PackageQuotaValue = -1 -- unlimited
RETURN 0 -- can exceed

-- get summary usage for the numeric quota
DECLARE @UsedQuantity int
DECLARE @UsedPlans int
DECLARE @UsedOverrides int
DECLARE @UsedAddons int

	-- limited by hosting plans
	SELECT @UsedPlans = SUM(HPQ.QuotaValue) FROM Packages AS P
	INNER JOIN HostingPlanQuotas AS HPQ ON P.PlanID = HPQ.PlanID
	WHERE HPQ.QuotaID = @QuotaID
		AND P.ParentPackageID = @PackageID
		AND P.OverrideQuotas = 0

	-- overrides
	SELECT @UsedOverrides = SUM(PQ.QuotaValue) FROM Packages AS P
	INNER JOIN PackageQuotas AS PQ ON P.PackageID = PQ.PackageID AND PQ.QuotaID = @QuotaID
	WHERE P.ParentPackageID = @PackageID
		AND P.OverrideQuotas = 1

	-- addons
	SELECT @UsedAddons = SUM(HPQ.QuotaValue * PA.Quantity)
	FROM Packages AS P
	INNER JOIN PackageAddons AS PA ON P.PackageID = PA.PackageID
	INNER JOIN HostingPlanQuotas AS HPQ ON PA.PlanID = HPQ.PlanID
	WHERE P.ParentPackageID = @PackageID AND HPQ.QuotaID = @QuotaID AND PA.StatusID = 1 -- active

--SET @UsedQuantity = (SELECT SUM(dbo.GetPackageAllocatedQuota(PackageID, @QuotaID)) FROM Packages WHERE ParentPackageID = @PackageID)

SET @UsedQuantity = @UsedPlans + @UsedOverrides + @UsedAddons

IF @UsedQuantity IS NULL
RETURN 0 -- can exceed

SET @ExceedValue = @UsedQuantity - @PackageQuotaValue

RETURN @ExceedValue
END








GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























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






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























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







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























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






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE FUNCTION GetFullIPAddress
(
	@ExternalIP varchar(24),
	@InternalIP varchar(24)
)
RETURNS varchar(60)
AS
BEGIN
DECLARE @IP varchar(60)
SET @IP = ''

IF @ExternalIP IS NOT NULL AND @ExternalIP <> ''
SET @IP = @ExternalIP

IF @InternalIP IS NOT NULL AND @InternalIP <> ''
SET @IP = @IP + ' (' + @InternalIP + ')'

RETURN @IP
END




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION GetItemComments
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


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION dbo.GetPackageAllocatedQuota
(
	@PackageID int,
	@QuotaID int
)
RETURNS int
AS
BEGIN

DECLARE @Result int

DECLARE @QuotaTypeID int
SELECT @QuotaTypeID = QuotaTypeID FROM Quotas
WHERE QuotaID = @QuotaID

IF @QuotaTypeID = 1
	SET @Result = 1 -- enabled
ELSE
	SET @Result = -1 -- unlimited

DECLARE @PID int, @ParentPackageID int
SET @PID = @PackageID

DECLARE @OverrideQuotas bit

WHILE 1 = 1
BEGIN

	DECLARE @QuotaValue int

	-- get package info
	SELECT
		@ParentPackageID = ParentPackageID,
		@OverrideQuotas = OverrideQuotas
	FROM Packages WHERE PackageID = @PID

	SET @QuotaValue = NULL

	-- check if this is a root 'System' package
	IF @ParentPackageID IS NULL
	BEGIN
		IF @QuotaTypeID = 1 -- boolean
			SET @QuotaValue = 1 -- enabled
		ELSE IF @QuotaTypeID > 1 -- numeric
			SET @QuotaValue = -1 -- unlimited
	END
	ELSE
	BEGIN
		-- check the current package
		IF @OverrideQuotas = 1
			SELECT @QuotaValue = QuotaValue FROM PackageQuotas WHERE QuotaID = @QuotaID AND PackageID = @PID
		ELSE
			SELECT @QuotaValue = HPQ.QuotaValue FROM Packages AS P
			INNER JOIN HostingPlanQuotas AS HPQ ON P.PlanID = HPQ.PlanID
			WHERE HPQ.QuotaID = @QuotaID AND P.PackageID = @PID

		IF @QuotaValue IS NULL
		SET @QuotaValue = 0

		-- check package addons
		DECLARE @QuotaAddonValue int
		SELECT
			@QuotaAddonValue = SUM(HPQ.QuotaValue * PA.Quantity)
		FROM PackageAddons AS PA
		INNER JOIN HostingPlanQuotas AS HPQ ON PA.PlanID = HPQ.PlanID
		WHERE PA.PackageID = @PID AND HPQ.QuotaID = @QuotaID AND PA.StatusID = 1 -- active

		-- process bool quota
		IF @QuotaAddonValue IS NOT NULL
		BEGIN
			IF @QuotaTypeID = 1
			BEGIN
				IF @QuotaAddonValue > 0 AND @QuotaValue = 0 -- enabled
				SET @QuotaValue = 1
			END
			ELSE
			BEGIN -- numeric quota
				IF @QuotaAddonValue < 0 -- unlimited
					SET @QuotaValue = -1
				ELSE
					SET @QuotaValue = @QuotaValue + @QuotaAddonValue
			END
		END
	END

	-- process bool quota
	IF @QuotaTypeID = 1
	BEGIN
		IF @QuotaValue = 0 OR @QuotaValue IS NULL -- disabled
		RETURN 0
	END
	ELSE
	BEGIN -- numeric quota
		IF @QuotaValue = 0 OR @QuotaValue IS NULL -- zero quantity
		RETURN 0

		IF (@QuotaValue <> -1 AND @Result = -1) OR (@QuotaValue < @Result AND @QuotaValue <> -1)
			SET @Result = @QuotaValue
	END

	IF @ParentPackageID IS NULL
	RETURN @Result -- exit from the loop

	SET @PID = @ParentPackageID

END -- end while

RETURN @Result
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION dbo.GetPackageAllocatedResource
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


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION dbo.GetPackageExceedingQuotas
(
	@PackageID int
)
RETURNS @quotas TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
AS
BEGIN

DECLARE @ParentPackageID int
DECLARE @PlanID int
DECLARE @OverrideQuotas bit

SELECT
	@ParentPackageID = ParentPackageID,
	@PlanID = PlanID,
	@OverrideQuotas = OverrideQuotas
FROM Packages WHERE PackageID = @PackageID


IF @ParentPackageID IS NOT NULL -- not root package
BEGIN

	IF @OverrideQuotas = 0 -- hosting plan quotas
		BEGIN
			INSERT INTO @quotas (QuotaID, QuotaName, QuotaValue)
			SELECT
				Q.QuotaID,
				Q.QuotaName,
				dbo.CheckExceedingQuota(@PackageID, Q.QuotaID, Q.QuotaTypeID) AS QuotaValue
			FROM HostingPlanQuotas AS HPQ
			INNER JOIN Quotas AS Q ON HPQ.QuotaID = Q.QuotaID
			WHERE HPQ.PlanID = @PlanID AND Q.QuotaTypeID <> 3
		END
	ELSE -- overriden quotas
		BEGIN
			INSERT INTO @quotas (QuotaID, QuotaName, QuotaValue)
			SELECT
				Q.QuotaID,
				Q.QuotaName,
				dbo.CheckExceedingQuota(@PackageID, Q.QuotaID, Q.QuotaTypeID) AS QuotaValue
			FROM PackageQuotas AS PQ
			INNER JOIN Quotas AS Q ON PQ.QuotaID = Q.QuotaID
			WHERE PQ.PackageID = @PackageID AND Q.QuotaTypeID <> 3
		END
END -- if 'root' package

RETURN
END








GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.PackageParents
(
	@PackageID int
)
RETURNS @T TABLE (PackageOrder int IDENTITY(1,1), PackageID int)
AS
BEGIN
	-- insert current user
	INSERT @T VALUES (@PackageID)

	-- owner
	DECLARE @ParentPackageID int, @TmpPackageID int
	SET @TmpPackageID = @PackageID

	WHILE 10 = 10
	BEGIN

		SET @ParentPackageID = NULL --reset var
		SELECT @ParentPackageID = ParentPackageID FROM Packages
		WHERE PackageID = @TmpPackageID

		IF @ParentPackageID IS NULL -- parent not found
		BREAK

		INSERT @T VALUES (@ParentPackageID)

		SET @TmpPackageID = @ParentPackageID
	END

RETURN
END







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.PackagesTree
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






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.UserParents
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







































GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.UsersTree
(
	@OwnerID int,
	@Recursive bit = 0
)
RETURNS @T TABLE (UserID int)
AS
BEGIN

	IF @Recursive = 1
	BEGIN
		-- insert "root" user
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






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLog](
	[RecordID] [varchar](32) NOT NULL,
	[UserID] [int] NULL,
	[Username] [nvarchar](50) NULL,
	[ItemID] [int] NULL,
	[SeverityID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[FinishDate] [datetime] NOT NULL,
	[SourceName] [varchar](50) NOT NULL,
	[TaskName] [varchar](50) NOT NULL,
	[ItemName] [nvarchar](100) NULL,
	[ExecutionLog] [ntext] NULL,
	[PackageID] [int] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogSources](
	[SourceName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_AuditLogSources] PRIMARY KEY CLUSTERED
(
	[SourceName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogTasks](
	[SourceName] [varchar](100) NOT NULL,
	[TaskName] [varchar](100) NOT NULL,
	[TaskDescription] [nvarchar](100) NULL,
 CONSTRAINT [PK_LogActions] PRIMARY KEY CLUSTERED
(
	[SourceName] ASC,
	[TaskName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlackBerryUsers](
	[BlackBerryUserId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BlackBerryUsers] PRIMARY KEY CLUSTERED
(
	[BlackBerryUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clusters](
	[ClusterID] [int] IDENTITY(1,1) NOT NULL,
	[ClusterName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Clusters] PRIMARY KEY CLUSTERED
(
	[ClusterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[ItemTypeID] [varchar](50) NOT NULL,
	[ItemID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CommentText] [nvarchar](1000) NULL,
	[SeverityID] [int] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CRMUsers](
	[CRMUserID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
	[CRMUserGuid] [uniqueidentifier] NULL,
	[BusinessUnitID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CRMUsers] PRIMARY KEY CLUSTERED
(
	[CRMUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domains](
	[DomainID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NOT NULL,
	[ZoneItemID] [int] NULL,
	[DomainName] [nvarchar](100) NOT NULL,
	[HostingAllowed] [bit] NOT NULL,
	[WebSiteID] [int] NULL,
	[MailDomainID] [int] NULL,
	[IsSubDomain] [bit] NOT NULL,
	[IsInstantAlias] [bit] NOT NULL,
	[IsDomainPointer] [bit] NOT NULL,
	[DomainItemId] [int] NULL,
 CONSTRAINT [PK_Domains] PRIMARY KEY CLUSTERED
(
	[DomainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeAccountEmailAddresses](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[EmailAddress] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_ExchangeAccountEmailAddresses] PRIMARY KEY CLUSTERED
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeAccounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[AccountType] [int] NOT NULL,
	[AccountName] [nvarchar](300) NOT NULL,
	[DisplayName] [nvarchar](300) NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) NULL,
	[MailEnabledPublicFolder] [bit] NULL,
	[MailboxManagerActions] [varchar](200) NULL,
	[SamAccountName] [nvarchar](100) NULL,
	[AccountPassword] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[MailboxPlanId] [int] NULL,
	[SubscriberNumber] [nvarchar](32) NULL,
	[UserPrincipalName] [nvarchar](300) NULL,
 CONSTRAINT [PK_ExchangeAccounts] PRIMARY KEY CLUSTERED
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeMailboxPlans](
	[MailboxPlanId] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[MailboxPlan] [nvarchar](300) NOT NULL,
	[MailboxPlanType] [int] NULL,
	[EnableActiveSync] [bit] NOT NULL,
	[EnableIMAP] [bit] NOT NULL,
	[EnableMAPI] [bit] NOT NULL,
	[EnableOWA] [bit] NOT NULL,
	[EnablePOP] [bit] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IssueWarningPct] [int] NOT NULL,
	[KeepDeletedItemsDays] [int] NOT NULL,
	[MailboxSizeMB] [int] NOT NULL,
	[MaxReceiveMessageSizeKB] [int] NOT NULL,
	[MaxRecipients] [int] NOT NULL,
	[MaxSendMessageSizeKB] [int] NOT NULL,
	[ProhibitSendPct] [int] NOT NULL,
	[ProhibitSendReceivePct] [int] NOT NULL,
	[HideFromAddressBook] [bit] NOT NULL,
 CONSTRAINT [PK_ExchangeMailboxPlans] PRIMARY KEY CLUSTERED
(
	[MailboxPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeOrganizationDomains](
	[OrganizationDomainID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[DomainID] [int] NULL,
	[IsHost] [bit] NULL,
	[DomainTypeID] [int] NOT NULL,
 CONSTRAINT [PK_ExchangeOrganizationDomains] PRIMARY KEY CLUSTERED
(
	[OrganizationDomainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeOrganizations](
	[ItemID] [int] NOT NULL,
	[OrganizationID] [nvarchar](128) NOT NULL,
	[ExchangeMailboxPlanID] [int] NULL,
	[LyncUserPlanID] [int] NULL,
	[SfBUserPlanID] [int] NULL,
 CONSTRAINT [PK_ExchangeOrganizations] PRIMARY KEY CLUSTERED
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GlobalDnsRecords](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[RecordType] [varchar](10) NOT NULL,
	[RecordName] [nvarchar](50) NOT NULL,
	[RecordData] [nvarchar](500) NOT NULL,
	[MXPriority] [int] NOT NULL,
	[ServiceID] [int] NULL,
	[ServerID] [int] NULL,
	[PackageID] [int] NULL,
	[IPAddressID] [int] NULL,
	[SrvPriority] [int] NULL,
	[SrvWeight] [int] NULL,
	[SrvPort] [int] NULL,
 CONSTRAINT [PK_GlobalDnsRecords] PRIMARY KEY CLUSTERED
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostingPlanQuotas](
	[PlanID] [int] NOT NULL,
	[QuotaID] [int] NOT NULL,
	[QuotaValue] [int] NOT NULL,
 CONSTRAINT [PK_HostingPlanQuotas_1] PRIMARY KEY CLUSTERED
(
	[PlanID] ASC,
	[QuotaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostingPlanResources](
	[PlanID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[CalculateDiskSpace] [bit] NULL,
	[CalculateBandwidth] [bit] NULL,
 CONSTRAINT [PK_HostingPlanResources] PRIMARY KEY CLUSTERED
(
	[PlanID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostingPlans](
	[PlanID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[PackageID] [int] NULL,
	[ServerID] [int] NULL,
	[PlanName] [nvarchar](200) NOT NULL,
	[PlanDescription] [ntext] NULL,
	[Available] [bit] NOT NULL,
	[SetupPrice] [money] NULL,
	[RecurringPrice] [money] NULL,
	[RecurrenceUnit] [int] NULL,
	[RecurrenceLength] [int] NULL,
	[IsAddon] [bit] NULL,
 CONSTRAINT [PK_HostingPlans] PRIMARY KEY CLUSTERED
(
	[PlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IPAddresses](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[ExternalIP] [varchar](24) NOT NULL,
	[InternalIP] [varchar](24) NULL,
	[ServerID] [int] NULL,
	[Comments] [ntext] NULL,
	[SubnetMask] [varchar](15) NULL,
	[DefaultGateway] [varchar](15) NULL,
	[PoolID] [int] NULL,
 CONSTRAINT [PK_IPAddresses] PRIMARY KEY CLUSTERED
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LyncUserPlans](
	[LyncUserPlanId] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[LyncUserPlanName] [nvarchar](300) NOT NULL,
	[LyncUserPlanType] [int] NULL,
	[IM] [bit] NOT NULL,
	[Mobility] [bit] NOT NULL,
	[MobilityEnableOutsideVoice] [bit] NOT NULL,
	[Federation] [bit] NOT NULL,
	[Conferencing] [bit] NOT NULL,
	[EnterpriseVoice] [bit] NOT NULL,
	[VoicePolicy] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_LyncUserPlans] PRIMARY KEY CLUSTERED
(
	[LyncUserPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LyncUsers](
	[LyncUserID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[LyncUserPlanID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[SipAddress] [nvarchar](300) NULL,
 CONSTRAINT [PK_LyncUsers] PRIMARY KEY CLUSTERED
(
	[LyncUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SfBUserPlans](
	[SfBUserPlanId] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[SfBUserPlanName] [nvarchar](300) NOT NULL,
	[SfBUserPlanType] [int] NULL,
	[IM] [bit] NOT NULL,
	[Mobility] [bit] NOT NULL,
	[MobilityEnableOutsideVoice] [bit] NOT NULL,
	[Federation] [bit] NOT NULL,
	[Conferencing] [bit] NOT NULL,
	[EnterpriseVoice] [bit] NOT NULL,
	[VoicePolicy] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_SfBUserPlans] PRIMARY KEY CLUSTERED
(
	[SfBUserPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SfBUsers](
	[SfBUserID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[SfBUserPlanID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[SipAddress] [nvarchar](300) NULL,
 CONSTRAINT [PK_SfBUsers] PRIMARY KEY CLUSTERED
(
	[SfBUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OCSUsers](
	[OCSUserID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[InstanceID] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OCSUsers] PRIMARY KEY CLUSTERED
(
	[OCSUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageAddons](
	[PackageAddonID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NULL,
	[PlanID] [int] NULL,
	[Quantity] [int] NULL,
	[PurchaseDate] [datetime] NULL,
	[Comments] [ntext] NULL,
	[StatusID] [int] NULL,
 CONSTRAINT [PK_PackageAddons] PRIMARY KEY CLUSTERED
(
	[PackageAddonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageIPAddresses](
	[PackageAddressID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NOT NULL,
	[AddressID] [int] NOT NULL,
	[ItemID] [int] NULL,
	[IsPrimary] [bit] NULL,
 CONSTRAINT [PK_PackageIPAddresses] PRIMARY KEY CLUSTERED
(
	[PackageAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageQuotas](
	[PackageID] [int] NOT NULL,
	[QuotaID] [int] NOT NULL,
	[QuotaValue] [int] NOT NULL,
 CONSTRAINT [PK_PackageQuotas] PRIMARY KEY CLUSTERED
(
	[PackageID] ASC,
	[QuotaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageResources](
	[PackageID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[CalculateDiskspace] [bit] NOT NULL,
	[CalculateBandwidth] [bit] NOT NULL,
 CONSTRAINT [PK_PackageResources_1] PRIMARY KEY CLUSTERED
(
	[PackageID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Packages](
	[PackageID] [int] IDENTITY(1,1) NOT NULL,
	[ParentPackageID] [int] NULL,
	[UserID] [int] NOT NULL,
	[PackageName] [nvarchar](300) NULL,
	[PackageComments] [ntext] NULL,
	[ServerID] [int] NULL,
	[StatusID] [int] NOT NULL,
	[PlanID] [int] NULL,
	[PurchaseDate] [datetime] NULL,
	[OverrideQuotas] [bit] NOT NULL,
	[BandwidthUpdated] [datetime] NULL,
 CONSTRAINT [PK_Packages] PRIMARY KEY CLUSTERED
(
	[PackageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackagesBandwidth](
	[PackageID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[LogDate] [datetime] NOT NULL,
	[BytesSent] [bigint] NOT NULL,
	[BytesReceived] [bigint] NOT NULL,
 CONSTRAINT [PK_PackagesBandwidth] PRIMARY KEY CLUSTERED
(
	[PackageID] ASC,
	[GroupID] ASC,
	[LogDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackagesDiskspace](
	[PackageID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[DiskSpace] [bigint] NOT NULL,
 CONSTRAINT [PK_PackagesDiskspace] PRIMARY KEY CLUSTERED
(
	[PackageID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageServices](
	[PackageID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
 CONSTRAINT [PK_PackageServices] PRIMARY KEY CLUSTERED
(
	[PackageID] ASC,
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageSettings](
	[PackageID] [int] NOT NULL,
	[SettingsName] [nvarchar](50) NOT NULL,
	[PropertyName] [nvarchar](50) NOT NULL,
	[PropertyValue] [ntext] NULL,
 CONSTRAINT [PK_PackageSettings] PRIMARY KEY CLUSTERED
(
	[PackageID] ASC,
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackagesTreeCache](
	[ParentPackageID] [int] NOT NULL,
	[PackageID] [int] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrivateIPAddresses](
	[PrivateAddressID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[IPAddress] [varchar](15) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_PrivateIPAddresses] PRIMARY KEY CLUSTERED
(
	[PrivateAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Providers](
	[ProviderID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[ProviderName] [nvarchar](100) NULL,
	[DisplayName] [nvarchar](200) NOT NULL,
	[ProviderType] [nvarchar](400) NULL,
	[EditorControl] [nvarchar](100) NULL,
	[DisableAutoDiscovery] [bit] NULL,
 CONSTRAINT [PK_ServiceTypes] PRIMARY KEY CLUSTERED
(
	[ProviderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotas](
	[QuotaID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[QuotaOrder] [int] NOT NULL,
	[QuotaName] [nvarchar](50) NOT NULL,
	[QuotaDescription] [nvarchar](200) NULL,
	[QuotaTypeID] [int] NOT NULL,
	[ServiceQuota] [bit] NULL,
	[ItemTypeID] [int] NULL,
	[HideQuota] [bit] NULL,
 CONSTRAINT [PK_Quotas] PRIMARY KEY CLUSTERED
(
	[QuotaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceGroupDnsRecords](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[RecordOrder] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[RecordType] [varchar](50) NOT NULL,
	[RecordName] [nvarchar](50) NOT NULL,
	[RecordData] [nvarchar](200) NOT NULL,
	[MXPriority] [int] NULL,
 CONSTRAINT [PK_ResourceGroupDnsRecords] PRIMARY KEY CLUSTERED
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceGroups](
	[GroupID] [int] NOT NULL,
	[GroupName] [nvarchar](100) NOT NULL,
	[GroupOrder] [int] NOT NULL,
	[GroupController] [nvarchar](1000) NULL,
	[ShowGroup] [bit] NULL,
 CONSTRAINT [PK_ResourceGroups] PRIMARY KEY CLUSTERED
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[ScheduleID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [nvarchar](100) NOT NULL,
	[PackageID] [int] NULL,
	[ScheduleName] [nvarchar](100) NULL,
	[ScheduleTypeID] [nvarchar](50) NULL,
	[Interval] [int] NULL,
	[FromTime] [datetime] NULL,
	[ToTime] [datetime] NULL,
	[StartTime] [datetime] NULL,
	[LastRun] [datetime] NULL,
	[NextRun] [datetime] NULL,
	[Enabled] [bit] NOT NULL,
	[PriorityID] [nvarchar](50) NULL,
	[HistoriesNumber] [int] NULL,
	[MaxExecutionTime] [int] NULL,
	[WeekMonthDay] [int] NULL,
 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED
(
	[ScheduleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleParameters](
	[ScheduleID] [int] NOT NULL,
	[ParameterID] [nvarchar](100) NOT NULL,
	[ParameterValue] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ScheduleParameters] PRIMARY KEY CLUSTERED
(
	[ScheduleID] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleTaskParameters](
	[TaskID] [nvarchar](100) NOT NULL,
	[ParameterID] [nvarchar](100) NOT NULL,
	[DataTypeID] [nvarchar](50) NOT NULL,
	[DefaultValue] [nvarchar](140) NULL,
	[ParameterOrder] [int] NOT NULL,
 CONSTRAINT [PK_ScheduleTaskParameters] PRIMARY KEY CLUSTERED
(
	[TaskID] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleTasks](
	[TaskID] [nvarchar](100) NOT NULL,
	[TaskType] [nvarchar](500) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_ScheduleTasks] PRIMARY KEY CLUSTERED
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleTaskViewConfiguration](
	[TaskID] [nvarchar](100) NOT NULL,
	[ConfigurationID] [nvarchar](100) NOT NULL,
	[Environment] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ScheduleTaskViewConfiguration] PRIMARY KEY CLUSTERED
(
	[ConfigurationID] ASC,
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servers](
	[ServerID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](100) NOT NULL,
	[ServerUrl] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[Comments] [ntext] NULL,
	[VirtualServer] [bit] NOT NULL,
	[InstantDomainAlias] [nvarchar](200) NULL,
	[PrimaryGroupID] [int] NULL,
	[ADRootDomain] [nvarchar](200) NULL,
	[ADUsername] [nvarchar](100) NULL,
	[ADPassword] [nvarchar](100) NULL,
	[ADAuthenticationType] [varchar](50) NULL,
	[ADEnabled] [bit] NULL,
 CONSTRAINT [PK_Servers] PRIMARY KEY CLUSTERED
(
	[ServerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceDefaultProperties](
	[ProviderID] [int] NOT NULL,
	[PropertyName] [nvarchar](50) NOT NULL,
	[PropertyValue] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ServiceDefaultProperties_1] PRIMARY KEY CLUSTERED
(
	[ProviderID] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceItemProperties](
	[ItemID] [int] NOT NULL,
	[PropertyName] [nvarchar](50) NOT NULL,
	[PropertyValue] [nvarchar](3000) NULL,
 CONSTRAINT [PK_ServiceItemProperties] PRIMARY KEY CLUSTERED
(
	[ItemID] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceItems](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NULL,
	[ItemTypeID] [int] NULL,
	[ServiceID] [int] NULL,
	[ItemName] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_ServiceItems] PRIMARY KEY CLUSTERED
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceItemTypes](
	[ItemTypeID] [int] NOT NULL,
	[GroupID] [int] NULL,
	[DisplayName] [nvarchar](50) NULL,
	[TypeName] [nvarchar](200) NULL,
	[TypeOrder] [int] NOT NULL,
	[CalculateDiskspace] [bit] NULL,
	[CalculateBandwidth] [bit] NULL,
	[Suspendable] [bit] NULL,
	[Disposable] [bit] NULL,
	[Searchable] [bit] NULL,
	[Importable] [bit] NOT NULL,
	[Backupable] [bit] NOT NULL,
 CONSTRAINT [PK_ServiceItemTypes] PRIMARY KEY CLUSTERED
(
	[ItemTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceProperties](
	[ServiceID] [int] NOT NULL,
	[PropertyName] [nvarchar](50) NOT NULL,
	[PropertyValue] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ServiceProperties_1] PRIMARY KEY CLUSTERED
(
	[ServiceID] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[ProviderID] [int] NOT NULL,
	[ServiceName] [nvarchar](50) NOT NULL,
	[Comments] [ntext] NULL,
	[ServiceQuotaValue] [int] NULL,
	[ClusterID] [int] NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SSLCertificates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[FriendlyName] [nvarchar](255) NULL,
	[Hostname] [nvarchar](255) NULL,
	[DistinguishedName] [nvarchar](500) NULL,
	[CSR] [ntext] NULL,
	[CSRLength] [int] NULL,
	[Certificate] [ntext] NULL,
	[Hash] [ntext] NULL,
	[Installed] [bit] NULL,
	[IsRenewal] [bit] NULL,
	[ValidFrom] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[SerialNumber] [nvarchar](250) NULL,
	[Pfx] [ntext] NULL,
	[PreviousId] [int] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemSettings](
	[SettingsName] [nvarchar](50) NOT NULL,
	[PropertyName] [nvarchar](50) NOT NULL,
	[PropertyValue] [ntext] NULL,
 CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED
(
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[OwnerID] [int] NULL,
	[RoleID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[IsDemo] [bit] NOT NULL,
	[IsPeer] [bit] NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](200) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[Created] [datetime] NULL,
	[Changed] [datetime] NULL,
	[Comments] [ntext] NULL,
	[SecondaryEmail] [nvarchar](255) NULL,
	[Address] [nvarchar](200) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Zip] [varchar](20) NULL,
	[PrimaryPhone] [varchar](30) NULL,
	[SecondaryPhone] [varchar](30) NULL,
	[Fax] [varchar](30) NULL,
	[InstantMessenger] [varchar](100) NULL,
	[HtmlMail] [bit] NULL,
	[CompanyName] [nvarchar](100) NULL,
	[EcommerceEnabled] [bit] NULL,
	[AdditionalParams] [nvarchar](max) NULL,
	[LoginStatusId] [int] NULL,
	[FailedLogins] [int] NULL,
	[SubscriberNumber] [nvarchar](32) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSettings](
	[UserID] [int] NOT NULL,
	[SettingsName] [nvarchar](50) NOT NULL,
	[PropertyName] [nvarchar](50) NOT NULL,
	[PropertyValue] [ntext] NULL,
 CONSTRAINT [PK_UserSettings] PRIMARY KEY CLUSTERED
(
	[UserID] ASC,
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Versions](
	[DatabaseVersion] [varchar](50) NOT NULL,
	[BuildDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Versions] PRIMARY KEY CLUSTERED
(
	[DatabaseVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualGroups](
	[VirtualGroupID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[DistributionType] [int] NULL,
	[BindDistributionToPrimary] [bit] NULL,
 CONSTRAINT [PK_VirtualGroups] PRIMARY KEY CLUSTERED
(
	[VirtualGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualServices](
	[VirtualServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
 CONSTRAINT [PK_VirtualServices] PRIMARY KEY CLUSTERED
(
	[VirtualServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[UsersDetailed]
AS
SELECT     U.UserID, U.RoleID, U.StatusID, U.LoginStatusId, U.SubscriberNumber, U.FailedLogins, U.OwnerID, U.Created, U.Changed, U.IsDemo, U.Comments, U.IsPeer, U.Username, U.FirstName, U.LastName, U.Email,
                      U.CompanyName, U.FirstName + ' ' + U.LastName AS FullName, UP.Username AS OwnerUsername, UP.FirstName AS OwnerFirstName,
                      UP.LastName AS OwnerLastName, UP.RoleID AS OwnerRoleID, UP.FirstName + ' ' + UP.LastName AS OwnerFullName, UP.Email AS OwnerEmail, UP.RoleID AS Expr1,
                          (SELECT     COUNT(PackageID) AS Expr1
                            FROM          dbo.Packages AS P
                            WHERE      (UserID = U.UserID)) AS PackagesNumber, U.EcommerceEnabled
FROM         dbo.Users AS U LEFT OUTER JOIN
                      dbo.Users AS UP ON U.OwnerID = UP.UserID

GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'APP_INSTALLER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'BACKUP')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'DNS_ZONE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'DOMAIN')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'EXCHANGE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'FILES')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'FTP_ACCOUNT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'GLOBAL_DNS')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'IMPORT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'IP_ADDRESS')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_ACCOUNT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_DOMAIN')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_FORWARDING')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_GROUP')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_LIST')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'ODBC_DSN')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SCHEDULER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SERVER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SHAREPOINT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SPACE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SQL_DATABASE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SQL_USER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'STATS_SITE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'USER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'VIRTUAL_SERVER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'VPS')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'WEB_SITE')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'APP_INSTALLER', N'INSTALL_APPLICATION', N'Install application')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'BACKUP', N'BACKUP', N'Backup')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'BACKUP', N'RESTORE', N'Restore')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DNS_ZONE', N'ADD_RECORD', N'Add record')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DNS_ZONE', N'DELETE_RECORD', N'Delete record')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DNS_ZONE', N'UPDATE_RECORD', N'Update record')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DOMAIN', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DOMAIN', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DOMAIN', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_DISTR_LIST_ADDRESS', N'Add distribution list e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_DOMAIN', N'Add organization domain')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_MAILBOX_ADDRESS', N'Add mailbox e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_PUBLIC_FOLDER_ADDRESS', N'Add public folder e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CALCULATE_DISKSPACE', N'Calculate organization disk space')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_CONTACT', N'Create contact')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_DISTR_LIST', N'Create distribution list')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_MAILBOX', N'Create mailbox')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_ORG', N'Create organization')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_PUBLIC_FOLDER', N'Create public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_CONTACT', N'Delete contact')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_DISTR_LIST', N'Delete distribution list')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_DISTR_LIST_ADDRESSES', N'Delete distribution list e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_DOMAIN', N'Delete organization domain')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_MAILBOX', N'Delete mailbox')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_MAILBOX_ADDRESSES', N'Delete mailbox e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_ORG', N'Delete organization')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_PUBLIC_FOLDER', N'Delete public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_PUBLIC_FOLDER_ADDRESSES', N'Delete public folder e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DISABLE_MAIL_PUBLIC_FOLDER', N'Disable mail public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ENABLE_MAIL_PUBLIC_FOLDER', N'Enable mail public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_CONTACT_GENERAL', N'Get contact general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_CONTACT_MAILFLOW', N'Get contact mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTR_LIST_ADDRESSES', N'Get distribution list e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTR_LIST_GENERAL', N'Get distribution list general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTR_LIST_MAILFLOW', N'Get distribution list mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_FOLDERS_STATS', N'Get organization public folder statistics')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_ADDRESSES', N'Get mailbox e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_ADVANCED', N'Get mailbox advanced settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_GENERAL', N'Get mailbox general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_MAILFLOW', N'Get mailbox mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOXES_STATS', N'Get organization mailboxes statistics')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_ORG_LIMITS', N'Get organization storage limits')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_ORG_STATS', N'Get organization statistics')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_PUBLIC_FOLDER_ADDRESSES', N'Get public folder e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_PUBLIC_FOLDER_GENERAL', N'Get public folder general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_PUBLIC_FOLDER_MAILFLOW', N'Get public folder mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_ORG_LIMITS', N'Update organization storage limits')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_PRIMARY_DISTR_LIST_ADDRESS', N'Set distribution list primary e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_PRIMARY_MAILBOX_ADDRESS', N'Set mailbox primary e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_PRIMARY_PUBLIC_FOLDER_ADDRESS', N'Set public folder primary e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_CONTACT_GENERAL', N'Update contact general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_CONTACT_MAILFLOW', N'Update contact mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_DISTR_LIST_GENERAL', N'Update distribution list general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_DISTR_LIST_MAILFLOW', N'Update distribution list mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_MAILBOX_ADVANCED', N'Update mailbox advanced settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_MAILBOX_GENERAL', N'Update mailbox general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_MAILBOX_MAILFLOW', N'Update mailbox mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_PUBLIC_FOLDER_GENERAL', N'Update public folder general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_PUBLIC_FOLDER_MAILFLOW', N'Update public folder mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'COPY_FILES', N'Copy files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'CREATE_ACCESS_DATABASE', N'Create MS Access database')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'CREATE_FILE', N'Create file')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'CREATE_FOLDER', N'Create folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'DELETE_FILES', N'Delete files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'MOVE_FILES', N'Move files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'RENAME_FILE', N'Rename file')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'SET_PERMISSIONS', NULL)
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'UNZIP_FILES', N'Unzip files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'UPDATE_BINARY_CONTENT', N'Update file binary content')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'ZIP_FILES', N'Zip files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FTP_ACCOUNT', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FTP_ACCOUNT', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FTP_ACCOUNT', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'GLOBAL_DNS', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'GLOBAL_DNS', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'GLOBAL_DNS', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IMPORT', N'IMPORT', N'Import')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'ADD_RANGE', N'Add range')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'ALLOCATE_PACKAGE_IP', N'Allocate package IP addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'DEALLOCATE_PACKAGE_IP', N'Deallocate package IP addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'DELETE_RANGE', N'Delete IP Addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'UPDATE_RANGE', N'Update IP Addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_ACCOUNT', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_ACCOUNT', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_ACCOUNT', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'ADD_POINTER', N'Add pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'DELETE_POINTER', N'Update pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_FORWARDING', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_FORWARDING', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_FORWARDING', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_GROUP', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_GROUP', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_GROUP', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_LIST', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_LIST', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_LIST', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ODBC_DSN', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ODBC_DSN', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ODBC_DSN', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SCHEDULER', N'RUN_SCHEDULE', NULL)
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'ADD_SERVICE', N'Add service')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'CHANGE_WINDOWS_SERVICE_STATUS', N'Change Windows service status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'CHECK_AVAILABILITY', N'Check availability')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'CLEAR_EVENT_LOG', N'Clear Windows event log')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'DELETE_SERVICE', N'Delete service')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'REBOOT', N'Reboot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'RESET_TERMINAL_SESSION', N'Reset terminal session')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'TERMINATE_SYSTEM_PROCESS', N'Terminate system process')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE_AD_PASSWORD', N'Update active directory password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE_PASSWORD', N'Update access password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE_SERVICE', N'Update service')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'ADD_GROUP', N'Add group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'ADD_SITE', N'Add site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'ADD_USER', N'Add user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'BACKUP_SITE', N'Backup site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'DELETE_GROUP', N'Delete group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'DELETE_SITE', N'Delete site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'DELETE_USER', N'Delete user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'INSTALL_WEBPARTS', N'Install Web Parts package')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'RESTORE_SITE', N'Restore site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'UNINSTALL_WEBPARTS', N'Uninstall Web Parts package')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'UPDATE_GROUP', N'Update group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'UPDATE_USER', N'Update user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'CALCULATE_DISKSPACE', N'Calculate disk space')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'CHANGE_ITEMS_STATUS', N'Change hosting items status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'CHANGE_STATUS', N'Change hostng space status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'DELETE', N'Delete hosting space')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'DELETE_ITEMS', N'Delete hosting items')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'BACKUP', N'Backup')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'RESTORE', N'Restore')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'TRUNCATE', N'Truncate')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_USER', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_USER', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_USER', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STATS_SITE', N'ADD', N'Add statistics site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STATS_SITE', N'DELETE', N'Delete statistics site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STATS_SITE', N'UPDATE', N'Update statistics site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'AUTHENTICATE', N'Authenticate')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'CHANGE_PASSWORD', N'Change password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'CHANGE_PASSWORD_BY_USERNAME_PASSWORD', N'Change password by username/password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'CHANGE_STATUS', N'Change status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'GET_BY_USERNAME_PASSWORD', N'Get by username/password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'SEND_REMINDER', N'Send password reminder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'UPDATE_SETTINGS', N'Update settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VIRTUAL_SERVER', N'ADD_SERVICES', N'Add services')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VIRTUAL_SERVER', N'DELETE_SERVICES', N'Delete services')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'ADD_EXTERNAL_IP', N'Add external IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'ADD_PRIVATE_IP', N'Add private IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'APPLY_SNAPSHOT', N'Apply VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CANCEL_JOB', N'Cancel Job')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CHANGE_ADMIN_PASSWORD', N'Change administrator password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CHANGE_STATE', N'Change VPS state')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CREATE', N'Create VPS')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE', N'Delete VPS')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_EXTERNAL_IP', N'Delete external IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_PRIVATE_IP', N'Delete private IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_SNAPSHOT', N'Delete VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_SNAPSHOT_SUBTREE', N'Delete VPS snapshot subtree')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'EJECT_DVD_DISK', N'Eject DVD disk')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'INSERT_DVD_DISK', N'Insert DVD disk')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'REINSTALL', N'Re-install VPS')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'RENAME_SNAPSHOT', N'Rename VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'SEND_SUMMARY_LETTER', N'Send VPS summary letter')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'SET_PRIMARY_EXTERNAL_IP', N'Set primary external IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'SET_PRIMARY_PRIVATE_IP', N'Set primary private IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'TAKE_SNAPSHOT', N'Take VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_CONFIGURATION', N'Update VPS configuration')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_HOSTNAME', N'Update host name')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_IP', N'Update IP Address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_PERMISSIONS', N'Update VPS permissions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_VDC_PERMISSIONS', N'Update space permissions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD_POINTER', N'Add domain pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD_SSL_FOLDER', N'Add shared SSL folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD_VDIR', N'Add virtual directory')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'CHANGE_FP_PASSWORD', N'Change FrontPage account password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'CHANGE_STATE', N'Change state')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_POINTER', N'Delete domain pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SECURED_FOLDER', N'Delete secured folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SECURED_GROUP', N'Delete secured group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SECURED_USER', N'Delete secured user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SSL_FOLDER', N'Delete shared SSL folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_VDIR', N'Delete virtual directory')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'INSTALL_FP', N'Install FrontPage Extensions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'INSTALL_SECURED_FOLDERS', N'Install secured folders')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UNINSTALL_FP', N'Uninstall FrontPage Extensions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UNINSTALL_SECURED_FOLDERS', N'Uninstall secured folders')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SECURED_FOLDER', N'Add/update secured folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SECURED_GROUP', N'Add/update secured group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SECURED_USER', N'Add/update secured user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SSL_FOLDER', N'Update shared SSL folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_VDIR', N'Update virtual directory')
GO
SET IDENTITY_INSERT [dbo].[Packages] ON
GO
SET LANGUAGE us_english
INSERT [dbo].[Packages] ([PackageID], [ParentPackageID], [UserID], [PackageName], [PackageComments], [ServerID], [StatusID], [PlanID], [OverrideQuotas], [BandwidthUpdated]) VALUES (1, NULL, 1, N'System', N'', NULL, 1, NULL, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[Packages] OFF
GO
INSERT [dbo].[PackagesTreeCache] ([ParentPackageID], [PackageID]) VALUES (1, 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1, 1, N'Windows2003', N'Windows Server 2003', N'SolidCP.Providers.OS.Windows2003, SolidCP.Providers.OS.Windows2003', N'Windows2003', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (2, 2, N'IIS60', N'Internet Information Services 6.0', N'SolidCP.Providers.Web.IIs60, SolidCP.Providers.Web.IIs60', N'IIS60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (3, 3, N'MSFTP60', N'Microsoft FTP Server 6.0', N'SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs60', N'MSFTP60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (4, 4, N'MailEnable', N'MailEnable Server 1.x - 7.x', N'SolidCP.Providers.Mail.MailEnable, SolidCP.Providers.Mail.MailEnable', N'MailEnable', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (5, 5, N'MSSQL', N'Microsoft SQL Server 2000', N'SolidCP.Providers.Database.MsSqlServer, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (6, 6, N'MySQL', N'MySQL Server 4.x', N'SolidCP.Providers.Database.MySqlServer, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (7, 7, N'MSDNS', N'Microsoft DNS Server', N'SolidCP.Providers.DNS.MsDNS, SolidCP.Providers.DNS.MsDNS', N'MSDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (8, 8, N'AWStats', N'AWStats Statistics Service', N'SolidCP.Providers.Statistics.AWStats, SolidCP.Providers.Statistics.AWStats', N'AWStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (9, 7, N'SimpleDNS', N'SimpleDNS Plus 4.x', N'SolidCP.Providers.DNS.SimpleDNS, SolidCP.Providers.DNS.SimpleDNS', N'SimpleDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (10, 8, N'SmarterStats', N'SmarterStats 3.x', N'SolidCP.Providers.Statistics.SmarterStats, SolidCP.Providers.Statistics.SmarterStats', N'SmarterStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (11, 4, N'SmarterMail', N'SmarterMail 2.x', N'SolidCP.Providers.Mail.SmarterMail2, SolidCP.Providers.Mail.SmarterMail2', N'SmarterMail', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (12, 3, N'Gene6FTP', N'Gene6 FTP Server 3.x', N'SolidCP.Providers.FTP.Gene6, SolidCP.Providers.FTP.Gene6', N'Gene6FTP', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (13, 4, N'Merak', N'Merak Mail Server 8.0.3 - 9.2.x', N'SolidCP.Providers.Mail.Merak, SolidCP.Providers.Mail.Merak', N'Merak', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (14, 4, N'SmarterMail', N'SmarterMail 3.x - 4.x', N'SolidCP.Providers.Mail.SmarterMail3, SolidCP.Providers.Mail.SmarterMail3', N'SmarterMail', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (15, 9, N'Sps20', N'Windows SharePoint Services 2.0', N'SolidCP.Providers.SharePoint.Sps20, SolidCP.Providers.SharePoint.Sps20', N'Sps20', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (16, 10, N'MSSQL', N'Microsoft SQL Server 2005', N'SolidCP.Providers.Database.MsSqlServer2005, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (17, 11, N'MySQL', N'MySQL Server 5.0', N'SolidCP.Providers.Database.MySqlServer50, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (18, 4, N'MDaemon', N'MDaemon 9.x - 11.x', N'SolidCP.Providers.Mail.MDaemon, SolidCP.Providers.Mail.MDaemon', N'MDaemon', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (19, 4, N'ArgoMail', N'ArGoSoft Mail Server 1.x', N'SolidCP.Providers.Mail.ArgoMail, SolidCP.Providers.Mail.ArgoMail', N'ArgoMail', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (20, 4, N'hMailServer', N'hMailServer 4.2', N'SolidCP.Providers.Mail.hMailServer, SolidCP.Providers.Mail.hMailServer', N'hMailServer', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (21, 4, N'AbilityMailServer', N'Ability Mail Server 2.x', N'SolidCP.Providers.Mail.AbilityMailServer, SolidCP.Providers.Mail.AbilityMailServer', N'AbilityMailServer', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (22, 4, N'hMailServer43', N'hMailServer 4.3', N'SolidCP.Providers.Mail.hMailServer43, SolidCP.Providers.Mail.hMailServer43', N'hMailServer43', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (23, 9, N'Sps20', N'Windows SharePoint Services 3.0', N'SolidCP.Providers.SharePoint.Sps30, SolidCP.Providers.SharePoint.Sps30', N'Sps20', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (24, 7, N'Bind', N'ISC BIND 8.x - 9.x', N'SolidCP.Providers.DNS.IscBind, SolidCP.Providers.DNS.Bind', N'Bind', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (25, 3, N'ServU', N'Serv-U FTP 6.x', N'SolidCP.Providers.FTP.ServU, SolidCP.Providers.FTP.ServU', N'ServU', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (26, 3, N'FileZilla', N'FileZilla FTP Server 0.9', N'SolidCP.Providers.FTP.FileZilla, SolidCP.Providers.FTP.FileZilla', N'FileZilla', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (27, 12, N'Exchange2007', N'Hosted Microsoft Exchange Server 2007', N'SolidCP.Providers.HostedSolution.Exchange2007, SolidCP.Providers.HostedSolution', N'Exchange', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (28, 7, N'SimpleDNS', N'SimpleDNS Plus 5.x', N'SolidCP.Providers.DNS.SimpleDNS5, SolidCP.Providers.DNS.SimpleDNS50', N'SimpleDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (29, 4, N'SmarterMail', N'SmarterMail 5.x', N'SolidCP.Providers.Mail.SmarterMail5, SolidCP.Providers.Mail.SmarterMail5', N'SmarterMail50', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (30, 11, N'MySQL', N'MySQL Server 5.1', N'SolidCP.Providers.Database.MySqlServer51, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (31, 8, N'SmarterStats', N'SmarterStats 4.x', N'SolidCP.Providers.Statistics.SmarterStats4, SolidCP.Providers.Statistics.SmarterStats', N'SmarterStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (32, 12, N'Exchange2010', N'Hosted Microsoft Exchange Server 2010', N'SolidCP.Providers.HostedSolution.Exchange2010, SolidCP.Providers.HostedSolution', N'Exchange', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (55, 7, N'NetticaDNS', N'Nettica DNS', N'SolidCP.Providers.DNS.Nettica, SolidCP.Providers.DNS.Nettica', N'NetticaDNS', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (56, 7, N'PowerDNS', N'PowerDNS', N'SolidCP.Providers.DNS.PowerDNS, SolidCP.Providers.DNS.PowerDNS', N'PowerDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (60, 4, N'SmarterMail', N'SmarterMail 6.x', N'SolidCP.Providers.Mail.SmarterMail6, SolidCP.Providers.Mail.SmarterMail6', N'SmarterMail60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (61, 4, N'Merak', N'Merak Mail Server 10.x', N'SolidCP.Providers.Mail.Merak10, SolidCP.Providers.Mail.Merak10', N'Merak', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (62, 8, N'SmarterStats', N'SmarterStats 5.x +', N'SolidCP.Providers.Statistics.SmarterStats5, SolidCP.Providers.Statistics.SmarterStats', N'SmarterStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (63, 4, N'hMailServer5', N'hMailServer 5.x', N'SolidCP.Providers.Mail.hMailServer5, SolidCP.Providers.Mail.hMailServer5', N'hMailServer5', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (64, 4, N'SmarterMail', N'SmarterMail 7.x - 8.x', N'SolidCP.Providers.Mail.SmarterMail7, SolidCP.Providers.Mail.SmarterMail7', N'SmarterMail60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (65, 4, N'SmarterMail', N'SmarterMail 9.x', N'SolidCP.Providers.Mail.SmarterMail9, SolidCP.Providers.Mail.SmarterMail9', N'SmarterMail60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (66, 4, N'SmarterMail', N'SmarterMail 10.x +', N'SolidCP.Providers.Mail.SmarterMail10, SolidCP.Providers.Mail.SmarterMail10', N'SmarterMail100', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (90, 12, N'Exchange2010SP2', N'Hosted Microsoft Exchange Server 2010 SP2', N'SolidCP.Providers.HostedSolution.Exchange2010SP2, SolidCP.Providers.HostedSolution', N'Exchange', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (100, 1, N'Windows2008', N'Windows Server 2008', N'SolidCP.Providers.OS.Windows2008, SolidCP.Providers.OS.Windows2008', N'Windows2008', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (101, 2, N'IIS70', N'Internet Information Services 7.0', N'SolidCP.Providers.Web.IIs70, SolidCP.Providers.Web.IIs70', N'IIS70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (102, 3, N'MSFTP70', N'Microsoft FTP Server 7.0', N'SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs70', N'MSFTP70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (103, 13, N'Organizations', N'Hosted Organizations', N'SolidCP.Providers.HostedSolution.OrganizationProvider, SolidCP.Providers.HostedSolution', N'Organizations', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (104, 1, N'Windows2012', N'Windows Server 2012', N'SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012', N'Windows2008', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (105, 2, N'IIS80', N'Internet Information Services 8.0', N'SolidCP.Providers.Web.IIs80, SolidCP.Providers.Web.IIs80', N'IIS70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (106, 3, N'MSFTP80', N'Microsoft FTP Server 8.0', N'SolidCP.Providers.FTP.MsFTP80, SolidCP.Providers.FTP.IIs80', N'MSFTP70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (111, 1, N'Windows2016', N'Windows Server 2016', N'SolidCP.Providers.OS.Windows2016, SolidCP.Providers.OS.Windows2016', N'Windows2008', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (112, 2, N'IIS100', N'Internet Information Services 10.0', N'SolidCP.Providers.Web.IIs100, SolidCP.Providers.Web.IIs100', N'IIS70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (113, 3, N'MSFTP100', N'Microsoft FTP Server 10.0', N'SolidCP.Providers.FTP.MsFTP100, SolidCP.Providers.FTP.IIs100', N'MSFTP70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (200, 20, N'HostedSharePoint30', N'Hosted Windows SharePoint Services 3.0', N'SolidCP.Providers.HostedSolution.HostedSharePointServer, SolidCP.Providers.HostedSolution', N'HostedSharePoint30', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (201, 21, N'CRM', N'Hosted MS CRM 4.0', N'SolidCP.Providers.HostedSolution.CRMProvider, SolidCP.Providers.HostedSolution', N'CRM', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (202, 22, N'MsSQL', N'Microsoft SQL Server 2008', N'SolidCP.Providers.Database.MsSqlServer2008, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (203, 31, N'BlackBerry 4.1', N'BlackBerry 4.1', N'SolidCP.Providers.HostedSolution.BlackBerryProvider, SolidCP.Providers.HostedSolution', N'BlackBerry', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (204, 31, N'BlackBerry 5.0', N'BlackBerry 5.0', N'SolidCP.Providers.HostedSolution.BlackBerry5Provider, SolidCP.Providers.HostedSolution', N'BlackBerry5', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (205, 32, N'OCS', N'Office Communications Server 2007 R2', N'SolidCP.Providers.HostedSolution.OCS2007R2, SolidCP.Providers.HostedSolution', N'OCS', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (206, 32, N'OCSEdge', N'OCS Edge server', N'SolidCP.Providers.HostedSolution.OCSEdge2007R2, SolidCP.Providers.HostedSolution', N'OCS_Edge', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (208, 20, N'HostedSharePoint2010', N'Hosted SharePoint Foundation 2010', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2010, SolidCP.Providers.HostedSolution', N'HostedSharePoint30', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (209, 23, N'MsSQL', N'Microsoft SQL Server 2012', N'SolidCP.Providers.Database.MsSqlServer2012, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (250, 41, N'Lync2010', N'Microsoft Lync Server 2010 Multitenant Hosting Pack', N'SolidCP.Providers.HostedSolution.Lync2010, SolidCP.Providers.HostedSolution', N'Lync', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (300, 30, N'HyperV', N'Microsoft Hyper-V', N'SolidCP.Providers.Virtualization.HyperV, SolidCP.Providers.Virtualization.HyperV', N'HyperV', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (301, 11, N'MySQL', N'MySQL Server 5.5', N'SolidCP.Providers.Database.MySqlServer55, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (400, 40, N'HyperVForPC', N'Microsoft Hyper-V For Private Cloud', N'SolidCP.Providers.VirtualizationForPC.HyperVForPC, SolidCP.Providers.VirtualizationForPC.HyperVForPC', N'HyperVForPrivateCloud', 1)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (2, 6, 1, N'MySQL4.Databases', N'Databases', 2, 1, 7, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (3, 5, 1, N'MsSQL2000.Databases', N'Databases', 2, 1, 5, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (4, 3, 1, N'FTP.Accounts', N'FTP Accounts', 2, 1, 9, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (11, 9, 2, N'SharePoint.Users', N'SharePoint Users', 2, 0, 1, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (12, 8, 1, N'Stats.Sites', N'Statistics Sites', 2, 1, 14, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (13, 2, 1, N'Web.Sites', N'Web Sites', 2, 1, 10, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (14, 4, 1, N'Mail.Accounts', N'Mail Accounts', 2, 1, 15, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (15, 5, 2, N'MsSQL2000.Users', N'Users', 2, 0, 6, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (18, 4, 3, N'Mail.Forwardings', N'Mail Forwardings', 2, 0, 16, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (19, 6, 2, N'MySQL4.Users', N'Users', 2, 0, 8, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (20, 4, 6, N'Mail.Lists', N'Mail Lists', 2, 0, 17, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (22, 9, 3, N'SharePoint.Groups', N'SharePoint Groups', 2, 0, 3, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (24, 4, 4, N'Mail.Groups', N'Mail Groups', 2, 0, 18, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (25, 2, 3, N'Web.AspNet11', N'ASP.NET 1.1', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (26, 2, 4, N'Web.AspNet20', N'ASP.NET 2.0', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (27, 2, 2, N'Web.Asp', N'ASP', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (28, 2, 5, N'Web.Php4', N'PHP 4.x', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (29, 2, 6, N'Web.Php5', N'PHP 5.x', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (30, 2, 7, N'Web.Perl', N'Perl', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (31, 2, 8, N'Web.Python', N'Python', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (32, 2, 9, N'Web.VirtualDirs', N'Virtual Directories', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (33, 2, 10, N'Web.FrontPage', N'FrontPage', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (34, 2, 11, N'Web.Security', N'Custom Security Settings', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (35, 2, 12, N'Web.DefaultDocs', N'Custom Default Documents', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (36, 2, 13, N'Web.AppPools', N'Dedicated Application Pools', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (37, 2, 14, N'Web.Headers', N'Custom Headers', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (38, 2, 15, N'Web.Errors', N'Custom Errors', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (39, 2, 16, N'Web.Mime', N'Custom MIME Types', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (40, 4, 2, N'Mail.MaxBoxSize', N'Max Mailbox Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (41, 5, 3, N'MsSQL2000.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (42, 5, 5, N'MsSQL2000.Backup', N'Database Backups', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (43, 5, 6, N'MsSQL2000.Restore', N'Database Restores', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (44, 5, 7, N'MsSQL2000.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (45, 6, 4, N'MySQL4.Backup', N'Database Backups', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (47, 1, 6, N'OS.ODBC', N'ODBC DSNs', 2, 0, 20, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (48, 7, 1, N'DNS.Editor', N'DNS Editor', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (49, 4, 5, N'Mail.MaxGroupMembers', N'Max Group Recipients', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (50, 4, 7, N'Mail.MaxListMembers', N'Max List Recipients', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (51, 1, 2, N'OS.Bandwidth', N'Bandwidth, MB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (52, 1, 1, N'OS.Diskspace', N'Disk space, MB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (53, 1, 3, N'OS.Domains', N'Domains', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (54, 1, 4, N'OS.SubDomains', N'Sub-Domains', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (55, 1, 6, N'OS.FileManager', N'File Manager', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (56, 9, 1, N'SharePoint.Sites', N'SharePoint Sites', 2, 0, 19, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (57, 2, 8, N'Web.CgiBin', N'CGI-BIN Folder', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (58, 2, 8, N'Web.SecuredFolders', N'Secured Folders', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (59, 2, 8, N'Web.SharedSSL', N'Shared SSL Folders', 2, 0, 25, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (60, 2, 8, N'Web.Redirections', N'Web Sites Redirection', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (61, 2, 8, N'Web.HomeFolders', N'Changing Sites Root Folders', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (62, 10, 1, N'MsSQL2005.Databases', N'Databases', 2, 0, 21, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (63, 10, 2, N'MsSQL2005.Users', N'Users', 2, 0, 22, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (64, 10, 3, N'MsSQL2005.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (65, 10, 5, N'MsSQL2005.Backup', N'Database Backups', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (66, 10, 6, N'MsSQL2005.Restore', N'Database Restores', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (67, 10, 7, N'MsSQL2005.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (68, 11, 1, N'MySQL5.Databases', N'Databases', 2, 0, 23, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (69, 11, 2, N'MySQL5.Users', N'Users', 2, 0, 24, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (70, 11, 4, N'MySQL5.Backup', N'Database Backups', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (71, 1, 9, N'OS.ScheduledTasks', N'Scheduled Tasks', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (72, 1, 10, N'OS.ScheduledIntervalTasks', N'Interval Tasks Allowed', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (73, 1, 11, N'OS.MinimumTaskInterval', N'Minimum Tasks Interval, minutes', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (74, 1, 7, N'OS.AppInstaller', N'Applications Installer', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (75, 1, 8, N'OS.ExtraApplications', N'Extra Application Packs', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (77, 12, 2, N'Exchange2007.DiskSpace', N'Organization Disk Space, MB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (78, 12, 3, N'Exchange2007.Mailboxes', N'Mailboxes per Organization', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (79, 12, 4, N'Exchange2007.Contacts', N'Contacts per Organization', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (80, 12, 5, N'Exchange2007.DistributionLists', N'Distribution Lists per Organization', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (81, 12, 6, N'Exchange2007.PublicFolders', N'Public Folders per Organization', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (83, 12, 9, N'Exchange2007.POP3Allowed', N'POP3 Access', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (84, 12, 11, N'Exchange2007.IMAPAllowed', N'IMAP Access', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (85, 12, 13, N'Exchange2007.OWAAllowed', N'OWA/HTTP Access', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (86, 12, 15, N'Exchange2007.MAPIAllowed', N'MAPI Access', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (87, 12, 17, N'Exchange2007.ActiveSyncAllowed', N'ActiveSync Access', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (88, 12, 8, N'Exchange2007.MailEnabledPublicFolders', N'Mail Enabled Public Folders Allowed', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (94, 2, 17, N'Web.ColdFusion', N'ColdFusion', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (95, 2, 1, N'Web.WebAppGallery', N'Web Application Gallery', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (96, 2, 18, N'Web.CFVirtualDirectories', N'ColdFusion Virtual Directories', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (97, 2, 20, N'Web.RemoteManagement', N'Remote web management allowed', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (100, 2, 19, N'Web.IPAddresses', N'Dedicated IP Addresses', 2, 1, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (102, 4, 8, N'Mail.DisableSizeEdit', N'Disable Mailbox Size Edit', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (103, 6, 3, N'MySQL4.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (104, 6, 5, N'MySQL4.Restore', N'Database Restores', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (105, 6, 6, N'MySQL4.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (106, 11, 3, N'MySQL5.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (107, 11, 5, N'MySQL5.Restore', N'Database Restores', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (108, 11, 6, N'MySQL5.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (200, 20, 1, N'HostedSharePoint.Sites', N'SharePoint Site Collections', 2, 0, 200, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (203, 10, 4, N'MsSQL2005.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (204, 5, 4, N'MsSQL2000.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (205, 13, 1, N'HostedSolution.Organizations', N'Organizations', 2, 0, 29, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (206, 13, 2, N'HostedSolution.Users', N'Users', 2, 0, 30, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (207, 13, 3, N'HostedSolution.Domains', N'Domains per Organizations', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (208, 20, 2, N'HostedSharePoint.MaxStorage', N'Max site storage, MB', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (209, 21, 2, N'HostedCRM.Users', N'Users', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (210, 21, 1, N'HostedCRM.Organization', N'CRM Organization', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (211, 22, 1, N'MsSQL2008.Databases', N'Databases', 2, 0, 31, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (212, 22, 2, N'MsSQL2008.Users', N'Users', 2, 0, 32, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (213, 22, 3, N'MsSQL2008.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (214, 22, 5, N'MsSQL2008.Backup', N'Database Backups', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (215, 22, 6, N'MsSQL2008.Restore', N'Database Restores', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (216, 22, 7, N'MsSQL2008.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (217, 22, 4, N'MsSQL2008.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (218, 23, 1, N'MsSQL2012.Databases', N'Databases', 2, 0, 37, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (219, 23, 2, N'MsSQL2012.Users', N'Users', 2, 0, 38, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (220, 1, 5, N'OS.DomainPointers', N'Domain Pointers', 2, 0, NULL, 1)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (221, 23, 3, N'MsSQL2012.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (222, 23, 5, N'MsSQL2012.Backup', N'Database Backups', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (223, 23, 6, N'MsSQL2012.Restore', N'Database Restores', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (224, 23, 7, N'MsSQL2012.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (225, 23, 4, N'MsSQL2012.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (230, 13, 4, N'HostedSolution.AllowChangeUPN', N'Allow to Change UserPrincipalName', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (300, 30, 1, N'VPS.ServersNumber', N'Number of VPS', 2, 0, 33, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (301, 30, 2, N'VPS.ManagingAllowed', N'Allow user to create VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (302, 30, 3, N'VPS.CpuNumber', N'Number of CPU cores', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (303, 30, 7, N'VPS.BootCdAllowed', N'Boot from CD allowed', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (304, 30, 8, N'VPS.BootCdEnabled', N'Boot from CD', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (305, 30, 4, N'VPS.Ram', N'RAM size, MB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (306, 30, 5, N'VPS.Hdd', N'Hard Drive size, GB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (307, 30, 6, N'VPS.DvdEnabled', N'DVD drive', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (308, 30, 10, N'VPS.ExternalNetworkEnabled', N'External Network', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (309, 30, 11, N'VPS.ExternalIPAddressesNumber', N'Number of External IP addresses', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (310, 30, 13, N'VPS.PrivateNetworkEnabled', N'Private Network', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (311, 30, 14, N'VPS.PrivateIPAddressesNumber', N'Number of Private IP addresses per VPS', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (312, 30, 9, N'VPS.SnapshotsNumber', N'Number of Snaphots', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (313, 30, 15, N'VPS.StartShutdownAllowed', N'Allow user to Start, Turn off and Shutdown VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (314, 30, 16, N'VPS.PauseResumeAllowed', N'Allow user to Pause, Resume VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (315, 30, 17, N'VPS.RebootAllowed', N'Allow user to Reboot VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (316, 30, 18, N'VPS.ResetAlowed', N'Allow user to Reset VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (317, 30, 19, N'VPS.ReinstallAllowed', N'Allow user to Re-install VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (318, 30, 12, N'VPS.Bandwidth', N'Monthly bandwidth, GB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (319, 31, 1, N'BlackBerry.Users', NULL, 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (320, 32, 1, N'OCS.Users', NULL, 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (321, 32, 2, N'OCS.Federation', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (322, 32, 3, N'OCS.FederationByDefault', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (323, 32, 4, N'OCS.PublicIMConnectivity', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (324, 32, 5, N'OCS.PublicIMConnectivityByDefault', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (325, 32, 6, N'OCS.ArchiveIMConversation', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (326, 32, 7, N'OCS.ArchiveIMConvervationByDefault', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (327, 32, 8, N'OCS.ArchiveFederatedIMConversation', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (328, 32, 9, N'OCS.ArchiveFederatedIMConversationByDefault', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (329, 32, 10, N'OCS.PresenceAllowed', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (330, 32, 10, N'OCS.PresenceAllowedByDefault', NULL, 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (331, 2, 4, N'Web.AspNet40', N'ASP.NET 4.0', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (332, 2, 21, N'Web.SSL', N'SSL', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (333, 2, 22, N'Web.AllowIPAddressModeSwitch', N'Allow IP Address Mode Switch', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (334, 2, 23, N'Web.EnableHostNameSupport', N'Enable Hostname Support', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (344, 2, 9, N'Web.Htaccess', N'htaccess', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (345, 40, 1, N'VPSForPC.ServersNumber', N'Number of VPS', 2, 0, 35, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (346, 40, 2, N'VPSForPC.ManagingAllowed', N'Allow user to create VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (347, 40, 3, N'VPSForPC.CpuNumber', N'Number of CPU cores', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (348, 40, 7, N'VPSForPC.BootCdAllowed', N'Boot from CD allowed', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (349, 40, 7, N'VPSForPC.BootCdEnabled', N'Boot from CD', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (350, 40, 4, N'VPSForPC.Ram', N'RAM size, MB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (351, 40, 5, N'VPSForPC.Hdd', N'Hard Drive size, GB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (352, 40, 6, N'VPSForPC.DvdEnabled', N'DVD drive', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (353, 40, 10, N'VPSForPC.ExternalNetworkEnabled', N'External Network', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (354, 40, 11, N'VPSForPC.ExternalIPAddressesNumber', N'Number of External IP addresses', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (355, 40, 13, N'VPSForPC.PrivateNetworkEnabled', N'Private Network', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (356, 40, 14, N'VPSForPC.PrivateIPAddressesNumber', N'Number of Private IP addresses per VPS', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (357, 40, 9, N'VPSForPC.SnapshotsNumber', N'Number of Snaphots', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (358, 40, 15, N'VPSForPC.StartShutdownAllowed', N'Allow user to Start, Turn off and Shutdown VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (359, 40, 16, N'VPSForPC.PauseResumeAllowed', N'Allow user to Pause, Resume VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (360, 40, 17, N'VPSForPC.RebootAllowed', N'Allow user to Reboot VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (361, 40, 18, N'VPSForPC.ResetAlowed', N'Allow user to Reset VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (362, 40, 19, N'VPSForPC.ReinstallAllowed', N'Allow user to Re-install VPS', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (363, 40, 12, N'VPSForPC.Bandwidth', N'Monthly bandwidth, GB', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (364, 12, 19, N'Exchange2007.KeepDeletedItemsDays', N'Keep Deleted Items (days)', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (365, 12, 20, N'Exchange2007.MaxRecipients', N'Maximum Recipients', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (366, 12, 21, N'Exchange2007.MaxSendMessageSizeKB', N'Maximum Send Message Size (Kb)', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (367, 12, 22, N'Exchange2007.MaxReceiveMessageSizeKB', N'Maximum Receive Message Size (Kb)', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (368, 12, 1, N'Exchange2007.IsConsumer', N'Is Consumer Organization', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (369, 12, 23, N'Exchange2007.EnablePlansEditing', N'Enable Plans Editing', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (370, 41, 1, N'Lync.Users', N'Users', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (371, 41, 2, N'Lync.Federation', N'Allow Federation', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (372, 41, 3, N'Lync.Conferencing', N'Allow Conferencing', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (373, 41, 4, N'Lync.MaxParticipants', N'Maximum Conference Particiapants', 3, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (374, 41, 5, N'Lync.AllowVideo', N'Allow Video in Conference', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (375, 41, 6, N'Lync.EnterpriseVoice', N'Allow EnterpriseVoice', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (376, 41, 7, N'Lync.EVUsers', N'Number of Enterprise Voice Users', 2, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (377, 41, 8, N'Lync.EVNational', N'Allow National Calls', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (378, 41, 9, N'Lync.EVMobile', N'Allow Mobile Calls', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (379, 41, 10, N'Lync.EVInternational', N'Allow International Calls', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (380, 41, 11, N'Lync.EnablePlansEditing', N'Enable Plans Editing', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (400, 20, 3, N'HostedSharePoint.UseSharedSSL', N'Use shared SSL Root', 1, 0, NULL, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (410, 1, 12, N'OS.AllowTenantCreateDomains', N'Allow Tenants to Create Top Level Domains', 1, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ResourceGroupDnsRecords] ON

GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (1, 1, 2, N'A', N'', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (2, 2, 2, N'A', N'*', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (3, 3, 2, N'A', N'www', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (4, 1, 3, N'A', N'ftp', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (5, 1, 4, N'A', N'mail', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (6, 2, 4, N'A', N'mail2', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (7, 3, 4, N'MX', N'', N'mail.[DOMAIN_NAME]', 10)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (9, 4, 4, N'MX', N'', N'mail2.[DOMAIN_NAME]', 21)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (10, 1, 5, N'A', N'mssql', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (11, 1, 6, N'A', N'mysql', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (12, 1, 8, N'A', N'stats', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (13, 5, 4, N'TXT', N'', N'v=spf1 a mx -all', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (14, 1, 12, N'A', N'smtp', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (15, 2, 12, N'MX', N'', N'smtp.[DOMAIN_NAME]', 10)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (16, 3, 12, N'CNAME', N'autodiscover', N'', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (17, 4, 12, N'CNAME', N'owa', N'', 0)
GO
SET IDENTITY_INSERT [dbo].[ResourceGroupDnsRecords] OFF
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (1, N'OS', 1, N'SolidCP.EnterpriseServer.OperatingSystemController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (2, N'Web', 2, N'SolidCP.EnterpriseServer.WebServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (3, N'FTP', 3, N'SolidCP.EnterpriseServer.FtpServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (4, N'Mail', 4, N'SolidCP.EnterpriseServer.MailServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (5, N'MsSQL2000', 7, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (6, N'MySQL4', 11, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (7, N'DNS', 16, N'SolidCP.EnterpriseServer.DnsServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (8, N'Statistics', 17, N'SolidCP.EnterpriseServer.StatisticsServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (9, N'SharePoint', 13, N'SolidCP.EnterpriseServer.SharePointServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (10, N'MsSQL2005', 8, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (11, N'MySQL5', 12, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (12, N'Exchange', 5, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (13, N'Hosted Organizations', 6, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (20, N'Hosted SharePoint', 14, N'SolidCP.EnterpriseServer.HostedSharePointServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (21, N'Hosted CRM', 15, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (22, N'MsSQL2008', 9, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (23, N'MsSQL2012', 10, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (30, N'VPS', 18, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (31, N'BlackBerry', 20, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (32, N'OCS', 21, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (40, N'VPSForPC', 19, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (41, N'Lync', 23, NULL, 1)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (52, N'SfB', 25, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Schedule] ON

GO
INSERT [dbo].[Schedule] ([ScheduleID], [TaskID], [PackageID], [ScheduleName], [ScheduleTypeID], [Interval], [FromTime], [ToTime], [StartTime], [LastRun], [NextRun], [Enabled], [PriorityID], [HistoriesNumber], [MaxExecutionTime], [WeekMonthDay]) VALUES (1, N'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE', 1, N'Calculate Disk Space', N'Daily', 0, CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00CDFE60 AS DateTime), NULL, CAST(0x00009DB500F547F5 AS DateTime), 1, N'Normal', 7, 3600, 1)
GO
INSERT [dbo].[Schedule] ([ScheduleID], [TaskID], [PackageID], [ScheduleName], [ScheduleTypeID], [Interval], [FromTime], [ToTime], [StartTime], [LastRun], [NextRun], [Enabled], [PriorityID], [HistoriesNumber], [MaxExecutionTime], [WeekMonthDay]) VALUES (2, N'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH', 1, N'Calculate Bandwidth', N'Daily', 0, CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00C5C100 AS DateTime), NULL, CAST(0x00009DB500F547F7 AS DateTime), 1, N'Normal', 7, 3600, 1)
GO
SET IDENTITY_INSERT [dbo].[Schedule] OFF
GO
INSERT [dbo].[ScheduleParameters] ([ScheduleID], [ParameterID], [ParameterValue]) VALUES (1, N'SUSPEND_OVERUSED', N'false')
GO
INSERT [dbo].[ScheduleParameters] ([ScheduleID], [ParameterID], [ParameterValue]) VALUES (2, N'SUSPEND_OVERUSED', N'false')
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'BACKUP_FILE_NAME', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'DELETE_TEMP_BACKUP', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'STORE_PACKAGE_FOLDER', N'String', N'\', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'STORE_PACKAGE_ID', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'STORE_SERVER_FOLDER', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'BACKUP_FOLDER', N'String', N'\backups', 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'BACKUP_NAME', N'String', N'database_backup.bak', 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'DATABASE_GROUP', N'List', N'MsSQL2008=SQL Server 2008;MsSQL2012=SQL Server 2012;MsSQL2014=SQL Server 2014;MySQL4=MySQL 4.0;MySQL5=MySQL 5.0,MariaDB=MariaDB', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'DATABASE_NAME', N'String', N'', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'ZIP_BACKUP', N'List', N'true=Yes;false=No', 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_BODY', N'MultiString', N'', 10)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_FROM', N'String', N'admin@mysite.com', 7)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_SUBJECT', N'String', N'Web Site is unavailable', 9)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_TO', N'String', N'admin@mysite.com', 8)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'PASSWORD', N'String', NULL, 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'RESPONSE_CONTAIN', N'String', NULL, 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'RESPONSE_DOESNT_CONTAIN', N'String', NULL, 6)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'RESPONSE_STATUS', N'String', N'500', 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'URL', N'String', N'http://', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USE_RESPONSE_CONTAIN', N'Boolean', N'false', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USE_RESPONSE_DOESNT_CONTAIN', N'Boolean', N'false', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USE_RESPONSE_STATUS', N'Boolean', N'false', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USERNAME', N'String', NULL, 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FILE_PATH', N'String', N'\', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_FOLDER', N'String', NULL, 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_PASSWORD', N'String', NULL, 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_SERVER', N'String', N'ftp.myserver.com', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_USERNAME', N'String', NULL, 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'CRM_REPORT', N'Boolean', N'true', 6)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'EMAIL', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'EXCHANGE_REPORT', N'Boolean', N'true', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'LYNC_REPORT', N'Boolean', N'true', 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'SFB_REPORT', N'Boolean', N'true', 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'ORGANIZATION_REPORT', N'Boolean', N'true', 7)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'SHAREPOINT_REPORT', N'Boolean', N'true', 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MSSQL_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MYSQL_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MARIADB_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_BCC', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_BODY', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_FROM', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_SUBJECT', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_USAGE_THRESHOLD', N'String', N'100', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'SEND_OVERUSED_EMAIL', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'SEND_WARNING_EMAIL', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_BCC', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_BODY', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_FROM', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_SUBJECT', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_USAGE_THRESHOLD', N'String', N'80', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'EXECUTABLE_PARAMS', N'String', N'', 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'EXECUTABLE_PATH', N'String', N'Executable.exe', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'SERVER_NAME', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_BODY', N'MultiString', NULL, 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_FROM', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_SUBJECT', N'String', NULL, 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_TO', N'String', NULL, 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'BANDWIDTH_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'DISKSPACE_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SEND_SUSPENSION_EMAIL', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SEND_WARNING_EMAIL', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPEND_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_BCC', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_BODY', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_FROM', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_SUBJECT', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_USAGE_THRESHOLD', N'String', N'100', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_BCC', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_BODY', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_FROM', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_SUBJECT', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_USAGE_THRESHOLD', N'String', N'80', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'FOLDER', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'ZIP_FILE', N'String', N'\archive.zip', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES', N'SolidCP.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, SolidCP.EnterpriseServer', 0)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_BACKUP', N'SolidCP.EnterpriseServer.BackupTask, SolidCP.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'SolidCP.EnterpriseServer.BackupDatabaseTask, SolidCP.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE', N'SolidCP.EnterpriseServer.CalculateExchangeDiskspaceTask, SolidCP.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH', N'SolidCP.EnterpriseServer.CalculatePackagesBandwidthTask, SolidCP.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE', N'SolidCP.EnterpriseServer.CalculatePackagesDiskspaceTask, SolidCP.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES', N'SolidCP.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, SolidCP.EnterpriseServer', 0)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'SolidCP.EnterpriseServer.CheckWebSiteTask, SolidCP.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'SolidCP.EnterpriseServer.FTPFilesTask, SolidCP.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_GENERATE_INVOICES', N'SolidCP.Ecommerce.EnterpriseServer.GenerateInvoicesTask, SolidCP.EnterpriseServer', 0)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'SolidCP.EnterpriseServer.HostedSolutionReportTask, SolidCP.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'SolidCP.EnterpriseServer.NotifyOverusedDatabasesTask, SolidCP.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_RUN_PAYMENT_QUEUE', N'SolidCP.Ecommerce.EnterpriseServer.RunPaymentQueueTask, SolidCP.EnterpriseServer', 0)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'SolidCP.EnterpriseServer.RunSystemCommandTask, SolidCP.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'SolidCP.EnterpriseServer.SendMailNotificationTask, SolidCP.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES', N'SolidCP.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, SolidCP.EnterpriseServer', 0)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SolidCP.EnterpriseServer.SuspendOverusedPackagesTask, SolidCP.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'SolidCP.EnterpriseServer.ZipFilesTask, SolidCP.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_BACKUP', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/Backup.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/BackupDatabase.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/CheckWebsite.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/SendFilesViaFtp.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_GENERATE_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/HostedSolutionReport.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/NotifyOverusedDatabases.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_RUN_PAYMENT_QUEUE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/ExecuteSystemCommand.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/SendEmailNotification.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/SuspendOverusedSpaces.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/ZipFiles.ascx')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet11Path', N'%SYSTEMROOT%\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet11Pool', N'ASP.NET V1.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet20Path', N'%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet20Pool', N'ASP.NET V2.0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet40Path', N'%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet40Pool', N'ASP.NET V4.0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspPath', N'%SYSTEMROOT%\System32\InetSrv\asp.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'CFFlashRemotingDirectory', N'C:\ColdFusion9\runtime\lib\wsconfig\1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'CFScriptsDirectory', N'C:\Inetpub\wwwroot\CFIDE')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ColdFusionPath', N'C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'GalleryXmlFeedUrl', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'PerlPath', N'%SYSTEMDRIVE%\Perl\bin\Perl.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'Php4Path', N'%PROGRAMFILES%\PHP\php.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'Php5Path', N'%PROGRAMFILES%\PHP\php-cgi.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedAccessFile', N'.htaccess')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedFoldersFile', N'.htfolders')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedGroupsFile', N'.htgroup')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedUsersFile', N'.htpasswd')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'PythonPath', N'%SYSTEMDRIVE%\Python\python.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'SecuredFoldersFilterPath', N'%SYSTEMROOT%\System32\InetSrv\IISPasswordFilter.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'WebGroupName', N'SCPWebUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (3, N'FtpGroupName', N'SCPFtpUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (3, N'SiteId', N'MSFTPSVC/1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'DatabaseLocation', N'%SYSTEMDRIVE%\SQL2000Databases\[USER_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'ExternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'InternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'SaLogin', N'sa')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'SaPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'UseDefaultDatabaseLocation', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'UseTrustedConnection', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'ExternalAddress', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 4.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'InternalAddress', N'localhost,3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'RootLogin', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'RootPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'AwStatsFolder', N'%SYSTEMDRIVE%\AWStats\wwwroot\cgi-bin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'BatchFileName', N'UpdateStats.bat')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'BatchLineTemplate', N'%SYSTEMDRIVE%\perl\bin\perl.exe awstats.pl config=[DOMAIN_NAME] -update')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'ConfigFileName', N'awstats.[DOMAIN_NAME].conf')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'ConfigFileTemplate', N'LogFormat = "%time2 %other %other %other %method %url %other %other %logname %host %other %ua %other %referer %other %code %other %other %bytesd %other %other"
LogSeparator = " "
DNSLookup = 2
DirCgi = "/cgi-bin"
DirIcons = "/icon"
AllowFullYearView=3
AllowToUpdateStatsFromBrowser = 0
UseFramesWhenCGI = 1
ShowFlagLinks = "en fr de it nl es"
LogFile = "[LOGS_FOLDER]\ex%YY-3%MM-3%DD-3.log"
DirData = "%SYSTEMDRIVE%\AWStats\data"
SiteDomain = "[DOMAIN_NAME]"
HostAliases = [DOMAIN_ALIASES]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'StatisticsURL', N'http://127.0.0.1/AWStats/cgi-bin/awstats.pl?config=[domain_name]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'AdminLogin', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'SimpleDnsUrl', N'http://127.0.0.1:8053')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'LogDeleteDays', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'LogFormat', N'W3Cex')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'LogWildcard', N'*.log')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'Password', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'ServerID', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'SmarterLogDeleteMonths', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'SmarterLogsPath', N'%SYSTEMDRIVE%\SmarterLogs')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'SmarterUrl', N'http://127.0.0.1:9999/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'StatisticsURL', N'http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'TimeZoneId', N'27')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'Username', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'ServiceUrl', N'http://127.0.0.1:9998/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (12, N'InstallFolder', N'%PROGRAMFILES%\Gene6 FTP Server')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (12, N'LogsFolder', N'%PROGRAMFILES%\Gene6 FTP Server\Log')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'ServiceUrl', N'http://127.0.0.1:9998/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'BrowseMethod', N'POST')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'BrowseParameters', N'ServerName=[SERVER]
Login=[USER]
Password=[PASSWORD]
Protocol=dbmssocn')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'BrowseURL', N'http://localhost/MLA/silentlogon.aspx')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'DatabaseLocation', N'%SYSTEMDRIVE%\SQL2005Databases\[USER_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'ExternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'InternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'SaLogin', N'sa')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'SaPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'UseDefaultDatabaseLocation', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'UseTrustedConnection', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'ExternalAddress', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 5.0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'InternalAddress', N'localhost,3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'RootLogin', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'RootPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (22, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (22, N'AdminUsername', N'Administrator')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'BindConfigPath', N'c:\BIND\dns\etc\named.conf')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'BindReloadBatch', N'c:\BIND\dns\reload.bat')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ZoneFileNameTemplate', N'db.[domain_name].txt')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ZonesFolderPath', N'c:\BIND\dns\zones')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (25, N'DomainId', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'KeepDeletedItemsDays', N'14')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'KeepDeletedMailboxesDays', N'30')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'MailboxDatabase', N'Hosted Exchange Database')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'RootOU', N'SCP Hosting')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'StorageGroup', N'Hosted Exchange Storage Group')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'TempDomain', N'my-temp-domain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'AdminLogin', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'SimpleDnsUrl', N'http://127.0.0.1:8053')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'AdminPassword', N' ')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'ExternalAddress', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 5.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'InternalAddress', N'localhost,3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'RootLogin', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'RootPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'LogDeleteDays', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'LogFormat', N'W3Cex')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'LogWildcard', N'*.log')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'Password', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'ServerID', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'SmarterLogDeleteMonths', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'SmarterLogsPath', N'%SYSTEMDRIVE%\SmarterLogs')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'SmarterUrl', N'http://127.0.0.1:9999/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'StatisticsURL', N'http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'TimeZoneId', N'27')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'Username', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'KeepDeletedItemsDays', N'14')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'KeepDeletedMailboxesDays', N'30')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'MailboxDatabase', N'Hosted Exchange Database')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'RootOU', N'SCP Hosting')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'TempDomain', N'my-temp-domain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbName', N'pdnsdb')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbPort', N'3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbServer', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbUser', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'AdminPassword', N' ')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'LogDeleteDays', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'LogFormat', N'W3Cex')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'LogWildcard', N'*.log')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'Password', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'ServerID', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'SmarterLogDeleteMonths', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'SmarterLogsPath', N'%SYSTEMDRIVE%\SmarterLogs')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'SmarterUrl', N'http://127.0.0.1:9999/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'StatisticsURL', N'http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'TimeZoneId', N'27')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'Username', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (63, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (63, N'AdminUsername', N'Administrator')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (65, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (65, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (65, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (65, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (65, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (66, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (66, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (66, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (66, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (66, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (100, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNet11Pool', N'ASP.NET 1.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNet40Path', N'%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNet40x64Path', N'%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNetBitnessMode', N'32')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'CFFlashRemotingDirectory', N'C:\ColdFusion9\runtime\lib\wsconfig\1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'CFScriptsDirectory', N'C:\Inetpub\wwwroot\CFIDE')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ClassicAspNet20Pool', N'ASP.NET 2.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ClassicAspNet40Pool', N'ASP.NET 4.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ColdFusionPath', N'C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'GalleryXmlFeedUrl', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'IntegratedAspNet20Pool', N'ASP.NET 2.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'IntegratedAspNet40Pool', N'ASP.NET 4.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'PerlPath', N'%SYSTEMDRIVE%\Perl\bin\PerlEx30.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'Php4Path', N'%PROGRAMFILES(x86)%\PHP\php.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'PhpMode', N'FastCGI')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'PhpPath', N'%PROGRAMFILES(x86)%\PHP\php-cgi.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ProtectedGroupsFile', N'.htgroup')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ProtectedUsersFile', N'.htpasswd')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'SecureFoldersModuleAssembly', N'SolidCP.IIsModules.SecureFolders, SolidCP.IIsModules, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=37f9c58a0aa32ff0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'WebGroupName', N'SCP_IUSRS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'WmSvc.CredentialsMode', N'WINDOWS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'WmSvc.Port', N'8172')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (102, N'FtpGroupName', N'SCPFtpUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (102, N'SiteId', N'Default FTP Site')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (104, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'AspNet11Pool', N'ASP.NET 1.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'AspNet40Path', N'%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'AspNet40x64Path', N'%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'AspNetBitnessMode', N'32')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'CFFlashRemotingDirectory', N'C:\ColdFusion9\runtime\lib\wsconfig\1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'CFScriptsDirectory', N'C:\Inetpub\wwwroot\CFIDE')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'ClassicAspNet20Pool', N'ASP.NET 2.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'ClassicAspNet40Pool', N'ASP.NET 4.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'ColdFusionPath', N'C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'GalleryXmlFeedUrl', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'IntegratedAspNet20Pool', N'ASP.NET 2.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'IntegratedAspNet40Pool', N'ASP.NET 4.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'PerlPath', N'%SYSTEMDRIVE%\Perl\bin\PerlEx30.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'Php4Path', N'%PROGRAMFILES(x86)%\PHP\php.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'PhpMode', N'FastCGI')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'PhpPath', N'%PROGRAMFILES(x86)%\PHP\php-cgi.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'ProtectedGroupsFile', N'.htgroup')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'ProtectedUsersFile', N'.htpasswd')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'SecureFoldersModuleAssembly', N'SolidCP.IIsModules.SecureFolders, SolidCP.IIsModules, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=37f9c58a0aa32ff0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'WebGroupName', N'SCP_IUSRS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'WmSvc.CredentialsMode', N'WINDOWS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'WmSvc.Port', N'8172')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (106, N'FtpGroupName', N'SCPFtpUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (106, N'SiteId', N'Default FTP Site')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (106, N'sslusesni', N'False')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (111, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'AspNet11Pool', N'ASP.NET 1.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'AspNet40Path', N'%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'AspNet40x64Path', N'%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'AspNetBitnessMode', N'32')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'CFFlashRemotingDirectory', N'C:\ColdFusion9\runtime\lib\wsconfig\1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'CFScriptsDirectory', N'C:\Inetpub\wwwroot\CFIDE')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'ClassicAspNet20Pool', N'ASP.NET 2.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'ClassicAspNet40Pool', N'ASP.NET 4.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'ColdFusionPath', N'C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'GalleryXmlFeedUrl', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'IntegratedAspNet20Pool', N'ASP.NET 2.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'IntegratedAspNet40Pool', N'ASP.NET 4.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'PerlPath', N'%SYSTEMDRIVE%\Perl\bin\PerlEx30.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'Php4Path', N'%PROGRAMFILES(x86)%\PHP\php.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'PhpMode', N'FastCGI')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'PhpPath', N'%PROGRAMFILES(x86)%\PHP\php-cgi.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'ProtectedGroupsFile', N'.htgroup')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'ProtectedUsersFile', N'.htpasswd')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'SecureFoldersModuleAssembly', N'SolidCP.IIsModules.SecureFolders, SolidCP.IIsModules, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=37f9c58a0aa32ff0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'WebGroupName', N'SCP_IUSRS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'WmSvc.CredentialsMode', N'WINDOWS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'WmSvc.Port', N'8172')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (113, N'FtpGroupName', N'SCPFtpUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (113, N'SiteId', N'Default FTP Site')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (113, N'sslusesni', N'False')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (200, N'RootWebApplicationIpAddress', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (204, N'UserName', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (204, N'UtilityPath', N'C:\Program Files\Research In Motion\BlackBerry Enterprise Server Resource Kit\BlackBerry Enterprise Server User Administration Tool')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'CpuLimit', N'100')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'CpuReserve', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'CpuWeight', N'100')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'DvdLibraryPath', N'C:\Hyper-V\Library')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'ExportedVpsPath', N'C:\Hyper-V\Exported')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'HostnamePattern', N'vps[user_id].hosterdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'OsTemplatesPath', N'C:\Hyper-V\Templates')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'PrivateNetworkFormat', N'192.168.0.1/16')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'RootFolder', N'C:\Hyper-V\VirtualMachines\[VPS_HOSTNAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'StartAction', N'start')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'StartupDelay', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'StopAction', N'shutDown')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'VirtualDiskType', N'dynamic')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (301, N'ExternalAddress', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (301, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 5.5')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (301, N'InternalAddress', N'localhost,3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (301, N'RootLogin', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (301, N'RootPassword', N'')
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (1, 9, N'SharePointUser', N'SolidCP.Providers.OS.SystemUser, SolidCP.Providers.Base', 19, 0, 0, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (2, 1, N'HomeFolder', N'SolidCP.Providers.OS.HomeFolder, SolidCP.Providers.Base', 15, 1, 0, 0, 1, 0, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (3, 9, N'SharePointGroup', N'SolidCP.Providers.OS.SystemGroup, SolidCP.Providers.Base', 20, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (5, 5, N'MsSQL2000Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 9, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (6, 5, N'MsSQL2000User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 10, 0, 0, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (7, 6, N'MySQL4Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 13, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (8, 6, N'MySQL4User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 14, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (9, 3, N'FTPAccount', N'SolidCP.Providers.FTP.FtpAccount, SolidCP.Providers.Base', 3, 0, 1, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (10, 2, N'WebSite', N'SolidCP.Providers.Web.WebSite, SolidCP.Providers.Base', 2, 1, 1, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (11, 4, N'MailDomain', N'SolidCP.Providers.Mail.MailDomain, SolidCP.Providers.Base', 8, 0, 1, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (12, 7, N'DNSZone', N'SolidCP.Providers.DNS.DnsZone, SolidCP.Providers.Base', 0, 0, 0, 1, 1, 0, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (13, 1, N'Domain', N'SolidCP.Providers.OS.Domain, SolidCP.Providers.Base', 1, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (14, 8, N'StatisticsSite', N'SolidCP.Providers.Statistics.StatsSite, SolidCP.Providers.Base', 17, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (15, 4, N'MailAccount', N'SolidCP.Providers.Mail.MailAccount, SolidCP.Providers.Base', 4, 1, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (16, 4, N'MailAlias', N'SolidCP.Providers.Mail.MailAlias, SolidCP.Providers.Base', 5, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (17, 4, N'MailList', N'SolidCP.Providers.Mail.MailList, SolidCP.Providers.Base', 7, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (18, 4, N'MailGroup', N'SolidCP.Providers.Mail.MailGroup, SolidCP.Providers.Base', 6, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (19, 9, N'SharePointSite', N'SolidCP.Providers.SharePoint.SharePointSite, SolidCP.Providers.Base', 18, 0, 0, 0, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (20, 1, N'ODBCDSN', N'SolidCP.Providers.OS.SystemDSN, SolidCP.Providers.Base', 22, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (21, 10, N'MsSQL2005Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 11, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (22, 10, N'MsSQL2005User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 12, 0, 0, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (23, 11, N'MySQL5Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 15, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (24, 11, N'MySQL5User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 16, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (25, 2, N'SharedSSLFolder', N'SolidCP.Providers.Web.SharedSSLFolder, SolidCP.Providers.Base', 21, 0, 0, 0, 1, 1, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (28, 7, N'SecondaryDNSZone', N'SolidCP.Providers.DNS.SecondaryDnsZone, SolidCP.Providers.Base', 0, 0, 0, 1, 1, 0, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (29, 13, N'Organization', N'SolidCP.Providers.HostedSolution.Organization, SolidCP.Providers.Base', 1, 1, 0, 1, 1, 1, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (30, 13, N'OrganizationDomain', N'SolidCP.Providers.HostedSolution.OrganizationDomain, SolidCP.Providers.Base', 1, NULL, NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (31, 22, N'MsSQL2008Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (32, 22, N'MsSQL2008User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (33, 30, N'VirtualMachine', N'SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base', 1, 0, 0, 1, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (34, 30, N'VirtualSwitch', N'SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base', 2, 0, 0, 1, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (35, 40, N'VMInfo', N'SolidCP.Providers.Virtualization.VMInfo, SolidCP.Providers.Base', 1, 0, 0, 1, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (36, 40, N'VirtualSwitch', N'SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base', 2, 0, 0, 1, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (37, 23, N'MsSQL2012Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (38, 23, N'MsSQL2012User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (200, 20, N'SharePointSiteCollection', N'SolidCP.Providers.SharePoint.SharePointSiteCollection, SolidCP.Providers.Base', 25, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'BackupSettings', N'BackupsPath', N'c:\HostingBackups')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpEnableSsl', N'False')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpPort', N'25')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpServer', N'127.0.0.1')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpUsername', N'postmaster')
GO
SET IDENTITY_INSERT [dbo].[Users] ON

GO
INSERT [dbo].[Users] ([UserID], [OwnerID], [RoleID], [StatusID], [IsDemo], [IsPeer], [Username], [Password], [FirstName], [LastName], [Email], [Created], [Changed], [Comments], [SecondaryEmail], [Address], [City], [State], [Country], [Zip], [PrimaryPhone], [SecondaryPhone], [Fax], [InstantMessenger], [HtmlMail], [CompanyName], [EcommerceEnabled], [AdditionalParams], [LoginStatusId], [FailedLogins], [SubscriberNumber]) VALUES (1, NULL, 1, 1, 0, 0, N'serveradmin', N'', N'Enterprise', N'Administrator', N'serveradmin@myhosting.com', CAST(0x00009DB500D45270 AS DateTime), CAST(0x00009DB500D45270 AS DateTime), N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 1, NULL, 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'CC', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'EnableLetter', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'From', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'HtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Account Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.3em; color: ##1F4978; }
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>
<div class="Header">
	Hosting Account Information
</div>

<ad:if test="#Signup#">
<p>
Hello #user.FirstName#,
</p>

<p>
New user account has been created and below you can find its summary information.
</p>

<h1>Control Panel URL</h1>
<table>
    <thead>
        <tr>
            <th>Control Panel URL</th>
            <th>Username</th>
            <th>Password</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>
            <td>#user.Username#</td>
            <td>#user.Password#</td>
        </tr>
    </tbody>
</table>
</ad:if>

<h1>Hosting Spaces</h1>
<p>
    The following hosting spaces have been created under your account:
</p>
<ad:foreach collection="#Spaces#" var="Space" index="i">
<h2>#Space.PackageName#</h2>
<table>
	<tbody>
		<tr>
			<td class="Label">Hosting Plan:</td>
			<td>
				<ad:if test="#not(isnull(Plans[Space.PlanId]))#">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>
			</td>
		</tr>
		<ad:if test="#not(isnull(Plans[Space.PlanId]))#">
		<tr>
			<td class="Label">Purchase Date:</td>
			<td>
				#Space.PurchaseDate#
			</td>
		</tr>
		<tr>
			<td class="Label">Disk Space, MB:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Diskspace" /></td>
		</tr>
		<tr>
			<td class="Label">Bandwidth, MB/Month:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Bandwidth" /></td>
		</tr>
		<tr>
			<td class="Label">Maximum Number of Domains:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Domains" /></td>
		</tr>
		<tr>
			<td class="Label">Maximum Number of Sub-Domains:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.SubDomains" /></td>
		</tr>
		</ad:if>
	</tbody>
</table>
</ad:foreach>

<ad:if test="#Signup#">
<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards,<br />
SolidCP.<br />
Web Site: <a href="https://solidcp.com">https://solidcp.com</a><br />
E-Mail: <a href="mailto:support@solidcp.com">support@solidcp.com</a>
</p>
</ad:if>

<ad:template name="NumericQuota">
	<ad:if test="#space.Quotas.ContainsKey(quota)#">
		<ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>
	<ad:else>
		0
	</ad:if>
</ad:template>

</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'Priority', N'Normal')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'Subject', N'<ad:if test="#Signup#">SolidCP  account has been created for<ad:else>SolidCP  account summary for</ad:if> #user.FirstName# #user.LastName#')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'TextBody', N'=================================
   Hosting Account Information
=================================
<ad:if test="#Signup#">Hello #user.FirstName#,

New user account has been created and below you can find its summary information.

Control Panel URL: https://panel.solidcp.com
Username: #user.Username#
Password: #user.Password#
</ad:if>

Hosting Spaces
==============
The following hosting spaces have been created under your account:

<ad:foreach collection="#Spaces#" var="Space" index="i">
=== #Space.PackageName# ===
Hosting Plan: <ad:if test="#not(isnull(Plans[Space.PlanId]))#">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>
<ad:if test="#not(isnull(Plans[Space.PlanId]))#">Purchase Date: #Space.PurchaseDate#
Disk Space, MB: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Diskspace" />
Bandwidth, MB/Month: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Bandwidth" />
Maximum Number of Domains: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Domains" />
Maximum Number of Sub-Domains: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.SubDomains" />
</ad:if>
</ad:foreach>

<ad:if test="#Signup#">If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
SolidCP.
Web Site: https://solidcp.com">
E-Mail: support@solidcp.com
</ad:if><ad:template name="NumericQuota"><ad:if test="#space.Quotas.ContainsKey(quota)#"><ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if><ad:else>0</ad:if></ad:template>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DisplayPreferences', N'GridItems', N'10')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'ExchangePolicy', N'MailboxPasswordPolicy', N'True;8;20;0;2;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'FtpPolicy', N'UserNamePolicy', N'True;-;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'FtpPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MailPolicy', N'AccountNamePolicy', N'True;;1;50;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MailPolicy', N'AccountPasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MailPolicy', N'CatchAllName', N'mail')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MsSqlPolicy', N'DatabaseNamePolicy', N'True;-;1;120;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MsSqlPolicy', N'UserNamePolicy', N'True;-;1;120;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MsSqlPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MySqlPolicy', N'DatabaseNamePolicy', N'True;;1;40;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MySqlPolicy', N'UserNamePolicy', N'True;;1;16;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MySqlPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OsPolicy', N'DsnNamePolicy', N'True;-;2;40;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'CC', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'EnableLetter', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'From', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'HtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Hosting Space Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.2em; }
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>

<div class="Header">
	Hosting Space Information
</div>

<ad:if test="#Signup#">
<p>
Hello #user.FirstName#,
</p>

<p>
&quot;#space.Package.PackageName#&quot; hosting space has been created under your user account
and below is the summary information for its resources.
</p>
</ad:if>

<ul>
    <ad:if test="#Signup#">
		<li><a href="##cp">Control Panel URL</a></li>
	</ad:if>
    <li><a href="##overview">Hosting Space Overview</a></li>
    <ad:if test="#space.Groups.ContainsKey("Web")#">
    <li><a href="##web">Web</a></li>
    <ul>
        <li><a href="##weblimits">Limits</a></li>
        <li><a href="##dns">Name Servers</a></li>
        <li><a href="##sites">Web Sites</a></li>
        <li><a href="##tempurl">Temporary URL</a></li>
        <li><a href="##files">Files Location</a></li>
    </ul>
    </ad:if>
    <ad:if test="#space.Groups.ContainsKey("FTP")#">
		<li><a href="##ftp">FTP</a></li>
		<ul>
			<li><a href="##ftplimits">Limits</a></li>
			<li><a href="##ftpserver">FTP Server</a></li>
			<li><a href="##ftpaccounts">FTP Accounts</a></li>
		</ul>
	</ad:if>
    <ad:if test="#space.Groups.ContainsKey("Mail")#">
		<li><a href="##mail">Mail</a></li>
		<ul>
			<li><a href="##maillimits">Limits</a></li>
			<li><a href="##smtp">SMTP/POP3 Server</a></li>
			<li><a href="##mailaccounts">Mail Accounts</a></li>
		</ul>
    </ad:if>
    <li><a href="##db">Databases</a></li>
    <ul>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2000")#"><li><a href="##mssql2000">SQL Server 2000</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2005")#"><li><a href="##mssql2005">SQL Server 2005</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2008")#"><li><a href="##mssql2008">SQL Server 2008</a></li></ad:if>
	    <ad:if test="#space.Groups.ContainsKey("MsSQL2012")#"><li><a href="##mssql2012">SQL Server 2012</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2014")#"><li><a href="##mssql2012">SQL Server 2014</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2016")#"><li><a href="##mssql2012">SQL Server 2016</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MySQL4")#"><li><a href="##mysql4">My SQL 4.x</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MySQL5")#"><li><a href="##mysql5">My SQL 5.x</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MariaDB")#"><li><a href="##MariaDB">MariaDB</a></li></ad:if>
        <li><a href="##msaccess">Microsoft Access</a></li>
    </ul>
    <ad:if test="#space.Groups.ContainsKey("Statistics")#"><li><a href="##stats">Statistics</a></li></ad:if>
	
	
	<li><a href="##db">Virtual Machine</a></li>
    <ul>
        <ad:if test="#space.Groups.ContainsKey("VPSForPC")#"><li><a href="##VPSForPC">VPSForPC</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("VPS2012")#"><li><a href="##vps2012">VPS2012</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("PROXMOX")#"><li><a href="##proxmox">Proxmox</a></li></ad:if>
    </ul>
</ul>

<ad:if test="#Signup#">
<a name="cp"></a>
<h1>Control Panel URL</h1>
<table>
    <thead>
        <tr>
            <th>Control Panel URL</th>
            <th>Username</th>
            <th>Password</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>
            <td>#user.Username#</td>
            <td>#user.Password#</td>
        </tr>
    </tbody>
</table>
</ad:if>

<a name="overview"></a>
<h1>Hosting Space Overview</h1>

<p>
    General hosting space limits:
</p>
<table>
    <tr>
        <td class="Label">Disk Space, MB:</td>
        <td><ad:NumericQuota quota="OS.Diskspace" /></td>
    </tr>
    <tr>
        <td class="Label">Bandwidth, MB/Month:</td>
        <td><ad:NumericQuota quota="OS.Bandwidth" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Domains:</td>
        <td><ad:NumericQuota quota="OS.Domains" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Sub-Domains:</td>
        <td><ad:NumericQuota quota="OS.SubDomains" /></td>
    </tr>
</table>

<ad:if test="#space.Groups.ContainsKey("Web")#">
<a name="web"></a>
<h1>Web</h1>
<a name="weblimits"></a>
<h2>
    Limits
</h2>
<table>
    <tr>
        <td class="Label">Maximum Number of Web Sites:</td>
        <td><ad:NumericQuota quota="Web.Sites" /></td>
    </tr>
	<tr>
        <td class="Label">Web Application Gallery:</td>
        <td><ad:BooleanQuota quota="Web.WebAppGallery" /></td>
    </tr>
    <tr>
        <td class="Label">Classic ASP:</td>
        <td><ad:BooleanQuota quota="Web.Asp" /></td>
    </tr>
    <tr>
        <td class="Label">ASP.NET 1.1:</td>
        <td><ad:BooleanQuota quota="Web.AspNet11" /></td>
    </tr>
    <tr>
        <td class="Label">ASP.NET 2.0:</td>
        <td><ad:BooleanQuota quota="Web.AspNet20" /></td>
    </tr>
    <tr>
        <td class="Label">ASP.NET 4.0:</td>
        <td><ad:BooleanQuota quota="Web.AspNet40" /></td>
    </tr>
    <tr>
        <td class="Label">PHP 4:</td>
        <td><ad:BooleanQuota quota="Web.Php4" /></td>
    </tr>
    <tr>
        <td class="Label">PHP 5:</td>
        <td><ad:BooleanQuota quota="Web.Php5" /></td>
    </tr>
    <tr>
        <td class="Label">Perl:</td>
        <td><ad:BooleanQuota quota="Web.Perl" /></td>
    </tr>
    <tr>
        <td class="Label">CGI-BIN:</td>
        <td><ad:BooleanQuota quota="Web.CgiBin" /></td>
    </tr>
</table>


<a name="dns"></a>
<h2>Name Servers</h2>
<p>
    In order to point your domain to the web site in this hosting space you should use the following Name Servers:
</p>
<table>
    <ad:foreach collection="#NameServers#" var="NameServer" index="i">
        <tr>
            <td class="Label">#NameServer#</td>
        </tr>
    </ad:foreach>
</table>
<p>
    You should change the name servers in domain registrar (Register.com, GoDaddy.com, etc.) control panel.
    Please, study domain registrar's user manual for directions how to change name servers or contact your domain
    registrar directly by e-mail or phone.
</p>
<p>
    Please note, the changes in domain registrar database do not reflect immediately and sometimes it requires from
    12 to 48 hours till the end of DNS propagation.
</p>

<a name="sites"></a>
<h2>Web Sites</h2>
<p>
    The following web sites have been created under hosting space:
</p>
<table>
    <ad:foreach collection="#WebSites#" var="WebSite">
        <tr>
            <td><a href="http://#WebSite.Name#" target="_blank">http://#WebSite.Name#</a></td>
        </tr>
    </ad:foreach>
</table>
<p>
    * Please note, your web sites may not be accessible from 12 to 48 hours after you've changed name servers for their respective domains.
</p>

<ad:if test="#isnotempty(PreviewDomain)#">
<a name="tempurl"></a>
<h2>Temporary URL</h2>
<p>
    You can access your web sites right now using their respective temporary URLs (instant aliases).
    Temporary URL is a sub-domain of the form http://yourdomain.com.providerdomain.com where &quot;yourdomain.com&quot; is your
    domain and &quot;providerdomain.com&quot; is the domain of your hosting provider.
</p>
<p>
    You can use the following Temporary URL for all your web sites:
</p>
<table>
    <tr>
        <td>
            http://YourDomain.com.<b>#PreviewDomain#</b>
        </td>
    </tr>
</table>
</ad:if>

<a name="files"></a>
<h2>Files Location</h2>
<p>
    Sometimes it is required to know the physical location of the hosting space folder (absolute path).
    Hosting space folder is the folder where all hosting space files such as web sites content, web logs, data files, etc. are located.
</p>
<p>
    The root of your hosting space on our HDD is here:
</p>
<table>
    <tr>
        <td>
             #PackageRootFolder#
        </td>
    </tr>
</table>
<p>
    By default the root folder of any web site within your hosting space is built as following (you can change it anytime from the control panel):
</p>
<table>
    <tr>
        <td>
             #PackageRootFolder#\YourDomain.com\wwwroot
        </td>
    </tr>
</table>
</ad:if>


<ad:if test="#space.Groups.ContainsKey("FTP")#">
<a name="ftp"></a>
<h1>FTP</h1>

<a name="ftplimits"></a>
<h2>Limits</h2>
<table>
    <tr>
        <td class="Label">Maximum Number of FTP Accounts:</td>
        <td><ad:NumericQuota quota="FTP.Accounts" /></td>
    </tr>
</table>


<a name="ftpserver"></a>
<h2>FTP Server</h2>
<p>
Your hosting space allows working with your files by FTP.
You can use the following FTP server to access your space files remotely:
</p>
<table>
    <tr>
        <td><a href="ftp://#FtpIP#">ftp://#FtpIP#</a></td>
    </tr>
</table>
<p>
    Also, you can use the following domain names to access your FTP server:
</p>
<table>
    <tr>
        <td>ftp://ftp.YourDomain.com</td>
    </tr>
</table>
<ad:if test="#isnotempty(PreviewDomain)#">
<p>
    During DNS propagation period (when domain name servers have been changed), similar to web sites, FTP server can be access with Temporary URL too:
</p>
<table>
    <tr>
        <td>ftp://ftp.YourDomain.com.<b>#PreviewDomain#</b></td>
    </tr>
</table>
</ad:if>
<a name="ftpaccounts"></a>
<h2>FTP Accounts</h2>
<p>
    The following FTP accounts have been created under your hosting space and can be used to access FTP server:
</p>
<table>
    <thead>
        <tr>
            <th>Username</th>
            <ad:if test="#Signup#">
                <th>Password</th>
            </ad:if>
            <th>Folder</th>
        </tr>
    </thead>
    <tbody>
        <ad:foreach collection="#FtpAccounts#" var="FtpAcocunt" index="i">
            <tr>
                <td>#FtpAcocunt.Name#</td>
                <ad:if test="#Signup#">
                    <td>
                        #FtpAcocunt.Password#
                    </td>
                </ad:if>
                <td>#FtpAcocunt.Folder#</td>
            </tr>
        </ad:foreach>
    </tbody>
</table>
</ad:if>


<ad:if test="#space.Groups.ContainsKey("Mail")#">
<a name="mail"></a>
<h1>Mail</h1>

<a name="maillimits"></a>
<h2>Limits</h2>
<table>
    <tr>
        <td class="Label">Maximum Number of Mail Accounts:</td>
        <td><ad:NumericQuota quota="Mail.Accounts" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Mail Forwardings:</td>
        <td><ad:NumericQuota quota="Mail.Forwardings" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Mail Groups (Aliases):</td>
        <td><ad:NumericQuota quota="Mail.Groups" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Mailing Lists:</td>
        <td><ad:NumericQuota quota="Mail.Lists" /></td>
    </tr>
</table>

<a name="smtp"></a>
<h2>SMTP/POP3 Server</h2>
<p>
Below is the IP address of your POP3/SMTP/IMAP server. You can always access your mailbox(es)
using this IP address instead of actual POP3/SMTP/IMAP servers name:
</p>
<table>
	<tr>
		<td>
			#MailRecords[0].ExternalIP#
		</td>
	</tr>
</table>

<p>
    Also, you can use the following domain names to access SMTP/POP3 server from your favourite e-mail client software:
</p>
<table>
    <tr>
        <td>mail.YourDomain.com</td>
    </tr>
</table>

<ad:if test="#isnotempty(PreviewDomain)#">
<p>
    During DNS propagation period (when domain name servers have been changed), similar to web sites, SMTP/POP3 server can be access with temporary domain too:
</p>
<table>
    <tr>
        <td>mail.YourDomain.com.<b>#PreviewDomain#</b></td>
    </tr>
</table>
</ad:if>

<a name="mailaccounts"></a>
<h2>Mail Accounts</h2>
<p>
	The following mail accounts have been created under your hosting space:
</p>
<table>
	<thead>
		<tr>
			<th>E-mail</th>
			<th>Username (for POP3/SMTP/IMAP/WebMail)</th>
			<ad:if test="#Signup#">
				<th>Password</th>
			</ad:if>
		</tr>
	</thead>
	<tbody>
		<ad:foreach collection="#MailAccounts#" var="MailAccount">
			<tr>
				<td>#MailAccount.Name#</td>
				<td>#MailAccount.Name#</td>
				<ad:if test="#Signup#">
					<td>
						 #MailAccount.Password#
					</td>
				</ad:if>
			</tr>
		</ad:foreach>
	</tbody>
</table>
</ad:if>

<a name="db"></a>
<h1>Databases</h1>

<p>
	You can create databases and database users on "Space Home -&gt; Databases" screen in the control panel.
</p>

<ad:if test="#space.Groups.ContainsKey("MsSQL2000")#">
<a name="mssql2000"></a>

<h2>SQL Server 2000</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2000.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2000.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to SQL Server 2000 from Management Studio, Enterprise Manager, Query Analyzer
	or other client software you can use the following SQL Server address:
</p>
<table>
	<tr>
		<td>#MsSQL2000Address#</td>
	</tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2000Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2005")#">
<a name="mssql2005"></a>

<h2>SQL Server 2005</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2005.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2005.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to SQL Server 2005 from Management Studio, Enterprise Manager, Query Analyzer
	or other client software you can use the following SQL Server address:
</p>
<table>
	<tr>
		<td>#MsSQL2005Address#</td>
	</tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2005Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2008")#">
<a name="mssql2008"></a>

<h2>SQL Server 2008</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2008.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2008.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to SQL Server 2008 from Management Studio, Enterprise Manager, Query Analyzer
	or other client software you can use the following SQL Server address:
</p>
<table>
	<tr>
		<td>#MsSQL2008Address#</td>
	</tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2008Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2012")#">
<a name="mssql2012"></a>

<h2>SQL Server 2012</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2012.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2012.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to SQL Server 2012 from Management Studio, Enterprise Manager, Query Analyzer
	or other client software you can use the following SQL Server address:
</p>
<table>
	<tr>
		<td>#MsSQL2012Address#</td>
	</tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2012Address#" />
</ad:if>


<ad:if test="#space.Groups.ContainsKey("MsSQL2014")#">
<a name="mssql2014"></a>

<h2>SQL Server 2014</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2014.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2014.Users" /></td>
    </tr>
</table>

<p>
    In order to connect to SQL Server 2014 from Management Studio, Enterprise Manager, Query Analyzer
    or other client software you can use the following SQL Server address:
</p>
<table>
    <tr>
        <td>#MsSQL2014Address#</td>
    </tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2014Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2016")#">
<a name="mssql2016"></a>

<h2>SQL Server 2016</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2016.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2016.Users" /></td>
    </tr>
</table>

<p>
    In order to connect to SQL Server 2016 from Management Studio, Enterprise Manager, Query Analyzer
    or other client software you can use the following SQL Server address:
</p>
<table>
    <tr>
        <td>#MsSQL2016Address#</td>
    </tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2016Address#" />
</ad:if>


<ad:if test="#space.Groups.ContainsKey("MySQL4")#">
<a name="mysql4"></a>
<h2>MySQL 4.x</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MySQL4.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MySQL4.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to MySQL 4.x server you can use the following address:
</p>
<table>
	<tr>
		<td>#MySQL4Address#</td>
	</tr>
</table>
</ad:if>


<ad:if test="#space.Groups.ContainsKey("MySQL5")#">
<a name="mysql5"></a>
<h2>MySQL 5.x</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MySQL5.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MySQL5.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to MySQL 5.x server you can use the following address:
</p>
<table>
	<tr>
		<td>#MySQL5Address#</td>
	</tr>
</table>
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MariaDB")#">
<a name="mysql5"></a>
<h2>MariaDB</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MariaDB.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MariaDB.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to MariaDB server you can use the following address:
</p>
<table>
	<tr>
		<td>#MariaDBAddress#</td>
	</tr>
</table>
</ad:if>


<a name="msaccess"></a>
<h2>Microsoft Access</h2>
<p>
	Microsoft Access database are automatically allowed in any hosting plan. You can create/upload any number of Access
	database from File Manager in control panel.
</p>


<ad:if test="#space.Groups.ContainsKey("Statistics")#">
<a name="stats"></a>
<h1>Web Statistics</h1>

<table>
    <tr>
        <td class="Label">Maximum Number of Statistics Sites:</td>
        <td><ad:NumericQuota quota="Stats.Sites" /></td>
    </tr>
</table>

<p>
	You can view advanced statistics from your domain using URL of the following form:
</p>
<table>
	<tr>
		<td>http://stats.YourDomain.com</td>
	</tr>
</table>
<ad:if test="#isnotempty(PreviewDomain)#">
<p>
    During DNS propagation period (when domain name servers have been changed), you can access web site statistics with Temporary URL:
</p>
<table>
    <tr>
        <td>http://stats.YourDomain.com.<b>#PreviewDomain#</b></td>
    </tr>
</table>
</ad:if>
</ad:if>

<!-- Templates -->
<ad:template name="MsSqlConnectionStrings">
<p>
	You may also use SQL Server address above in your application connection strings, for example:
</p>
<table>
	<tr>
		<td class="Label">Classic ASP (ADO Library)</td>
		<td>Provider=SQLOLEDB;Data source=<b>#server#</b>;Initial catalog=databaseName;User Id=userName;Password=password;</td>
	</tr>
	<tr>
		<td class="Label">ASP.NET (ADO.NET Library)</td>
		<td>Server=<b>#server#</b>;Database=databaseName;Uid=userName;Password=password;</td>
	</tr>
</table>
</ad:template>

<ad:template name="NumericQuota">
	<ad:if test="#space.Quotas.ContainsKey(quota)#">
		<ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>
	<ad:else>
		0
	</ad:if>
</ad:template>

<ad:template name="BooleanQuota">
	<ad:if test="#space.Quotas.ContainsKey(quota)#">
		<ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot 0#">Enabled<ad:else>Disabled</ad:if>
	<ad:else>
		Disabled
	</ad:if>
</ad:template>


<a name="vm"></a>
<h1>Virtual Machines</h1>


<ad:if test="#space.Groups.ContainsKey("VPSForPC")#">
<a name="VPSForPC"></a>
<h2>VPSForPC</h2>

<table>
    <tr>
        <td class="Label">CPUs: </td>
        <td><ad:NumericQuota quota="VPSForPC.CpuNumber" /></td>
    </tr>
    <tr>
        <td class="Label">RAM: </td>
        <td><ad:NumericQuota quota="VPSForPC.Ram" /></td>
    </tr>
    <tr>
        <td class="Label">HDD: </td>
        <td><ad:NumericQuota quota="VPSForPC.Hdd" /></td>
    </tr>
</table>
</ad:if>

<ad:if test="#space.Groups.ContainsKey("VPS2012")#">
<a name="vps2012"></a>
<h2>HyperV</h2>

<table>
    <tr>
        <td class="Label">CPUs: </td>
        <td><ad:NumericQuota quota="VPS2012.CpuNumber" /></td>
    </tr>
    <tr>
        <td class="Label">RAM: </td>
        <td><ad:NumericQuota quota="VPS2012.Ram" /></td>
    </tr>
    <tr>
        <td class="Label">HDD: </td>
        <td><ad:NumericQuota quota="VPS2012.Hdd" /></td>
    </tr>
</table>
</ad:if>

<ad:if test="#space.Groups.ContainsKey("PROXMOX")#">
<a name="proxmox"></a>
<h2>Proxmox</h2>

<table>
    <tr>
        <td class="Label">CPUs: </td>
        <td><ad:NumericQuota quota="PROXMOX.CpuNumber" /></td>
    </tr>
    <tr>
        <td class="Label">RAM: </td>
        <td><ad:NumericQuota quota="PROXMOX.Ram" /></td>
    </tr>
    <tr>
        <td class="Label">HDD: </td>
        <td><ad:NumericQuota quota="PROXMOX.Hdd" /></td>
    </tr>
</table>
</ad:if>

<ad:if test="#Signup#">
<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards,<br />
HostingCompany.<br />
Web Site: <a href="HostingCompany.com">HostingCompany.com</a><br />
E-Mail: <a href="mailto:support@HostingCompany.com">support@HostingCompany.com</a>
</p>
</ad:if>

</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'Priority', N'Normal')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'Subject', N'"#space.Package.PackageName#" <ad:if test="#Signup#">hosting space has been created for<ad:else>hosting space summary for</ad:if> #user.FirstName# #user.LastName#')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'TextBody', N'================================
   Hosting Space Information
================================

<ad:if test="#Signup#">
Hello #user.FirstName#,

"#space.Package.PackageName#" hosting space has been created under your user account
and below is the summary information for its resources.
</ad:if>

Control Panel
=============
Control Panel URL: https://panel.HostingCompany.com
Username: #user.Username#
Password: #user.Password#

<ad:if test="#space.Groups.ContainsKey("OS")#">
Hosting Space Overview
======================
General hosting space limits:
Disk Space, MB: <ad:NumericQuota quota="OS.Diskspace" />
Bandwidth, MB/Month: <ad:NumericQuota quota="OS.Bandwidth" />
Maximum Number of Domains: <ad:NumericQuota quota="OS.Domains" />
Maximum Number of Sub-Domains: <ad:NumericQuota quota="OS.SubDomains" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("Web")#">Web
======

Limits
------
Maximum Number of Web Sites: <ad:NumericQuota quota="Web.Sites" />
Web Application Gallery: <ad:BooleanQuota quota="Web.WebAppGallery" />
Classic ASP: <ad:BooleanQuota quota="Web.Asp" />
ASP.NET 1.1: <ad:BooleanQuota quota="Web.AspNet11" />
ASP.NET 2.0: <ad:BooleanQuota quota="Web.AspNet20" />
ASP.NET 4.0: <ad:BooleanQuota quota="Web.AspNet40" />
PHP 4: <ad:BooleanQuota quota="Web.Php4" />
PHP 5: <ad:BooleanQuota quota="Web.Php5" />
Perl: <ad:BooleanQuota quota="Web.Perl" />
CGI-BIN: <ad:BooleanQuota quota="Web.CgiBin" />

Name Servers
------------
In order to point your domain to the web site in this hosting space you should use the following Name Servers:
<ad:foreach collection="#NameServers#" var="NameServer" index="i">
   #NameServer#</ad:foreach>

You should change the name servers in domain registrar (Register.com, GoDaddy.com, etc.) control panel. Please, study domain registrar's user manual for directions how to change name servers or contact your domain registrar directly by e-mail or phone.

Please note, the changes in domain registrar database do not reflect immediately and sometimes it requires from 12 to 48 hours till the end of DNS propagation.


Web Sites
---------
The following web sites have been created under hosting space:
<ad:foreach collection="#WebSites#" var="WebSite">
   http://#WebSite.Name#</ad:foreach>

* Please note, your web sites may not be accessible from 12 to 48 hours after you've changed name servers for their respective domains.

<ad:if test="#isnotempty(PreviewDomain)#">Temporary URL
-------------
You can access your web sites right now using their respective temporary URLs (Preview Domains). Temporary URL is a sub-domain of the form http://yourdomain.com.providerdomain.com where "yourdomain.com" is your domain and "providerdomain.com" is the domain of your hosting provider.

You can use the following Temporary URL for all your web sites:

    http://YourDomain.com.#PreviewDomain#
</ad:if>


Files Location
--------------
Sometimes it is required to know the physical location of the hosting space folder (absolute path).
Hosting space folder is the folder where all hosting space files such as web sites content, web logs, data files, etc. are located.

The root of your hosting space on our HDD is here:

   #PackageRootFolder#

By default the root folder of any web site within your hosting space is built as following (you can change it anytime from the control panel):

   #PackageRootFolder#\YourDomain.com\wwwroot

</ad:if>


<ad:if test="#space.Groups.ContainsKey("FTP")#">FTP
=====

Limits
------
Maximum Number of FTP Accounts: <ad:NumericQuota quota="FTP.Accounts" />

FTP Server
----------
Your hosting space allows working with your files by FTP.
You can use the following FTP server to access your space files remotely:

   ftp://#FtpIP#

Also, you can use the following domain names to access your FTP server:

   ftp://ftp.YourDomain.com

<ad:if test="#isnotempty(PreviewDomain)#">During DNS propagation period (when domain name servers have been changed), similar to web sites, FTP server can be access with Temporary URL too:

    ftp://ftp.YourDomain.com.#PreviewDomain#
</ad:if>

FTP Accounts
------------
The following FTP accounts have been created under your hosting space and can be used to access FTP server:

<ad:foreach collection="#FtpAccounts#" var="FtpAcocunt" index="i">Username: #FtpAcocunt.Name#
<ad:if test="#Signup#">Password: #FtpAcocunt.Password#
</ad:if>Folder: #FtpAcocunt.Folder#

</ad:foreach>
</ad:if>

<ad:if test="#space.Groups.ContainsKey("Mail")#">Mail
========

Limits
------
Maximum Number of Mail Accounts: <ad:NumericQuota quota="Mail.Accounts" />
Maximum Number of Mail Forwardings: <ad:NumericQuota quota="Mail.Forwardings" />
Maximum Number of Mail Groups (Aliases): <ad:NumericQuota quota="Mail.Groups" />
Maximum Number of Mailing Lists: <ad:NumericQuota quota="Mail.Lists" />

SMTP/POP3 Server
----------------
Below is the IP address of your POP3/SMTP/IMAP server. You can always access your mailbox(es) using this IP address instead of actual POP3/SMTP/IMAP servers name:

   #MailRecords[0].ExternalIP#

Also, you can use the following domain names to access SMTP/POP3 server from your favourite e-mail client software:

   mail.YourDomain.com

<ad:if test="#isnotempty(PreviewDomain)#">During DNS propagation period (when domain name servers have been changed), similar to web sites, SMTP/POP3 server can be access with temporary domain too:

   mail.YourDomain.com.#PreviewDomain#
</ad:if>

Mail Accounts
-------------
The following mail accounts have been created under your hosting space:
<ad:foreach collection="#MailAccounts#" var="MailAccount">E-mail: #MailAccount.Name#
<ad:if test="#Signup#">Password: #MailAccount.Password#</ad:if>
</ad:foreach>
</ad:if>


Databases
=========
You can create databases and database users on "Space Home -&gt; Databases" screen in the control panel.

<ad:if test="#space.Groups.ContainsKey("MsSQL2000")#">SQL Server 2000
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2000.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2000.Users" />

In order to connect to SQL Server 2000 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:

    #MsSQL2000Address#

<ad:MsSqlConnectionStrings server="#MsSQL2000Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2005")#">SQL Server 2005
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2005.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2005.Users" />

In order to connect to SQL Server 2005 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:

    #MsSQL2005Address#

<ad:MsSqlConnectionStrings server="#MsSQL2005Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2008")#">SQL Server 2008
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2008.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2008.Users" />

In order to connect to SQL Server 2008 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:

    #MsSQL2008Address#

<ad:MsSqlConnectionStrings server="#MsSQL2008Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2012")#">SQL Server 2012
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2012.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2012.Users" />

In order to connect to SQL Server 2012 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:

    #MsSQL2012Address#

<ad:MsSqlConnectionStrings server="#MsSQL2012Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2014")#">SQL Server 2014
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2014.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2014.Users" />

In order to connect to SQL Server 2014 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:

    #MsSQL2014Address#

<ad:MsSqlConnectionStrings server="#MsSQL2014Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2016")#">SQL Server 2016
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2016.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2016.Users" />

In order to connect to SQL Server 2016 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:

    #MsSQL2016Address#

<ad:MsSqlConnectionStrings server="#MsSQL2016Address#" />
</ad:if>


<ad:if test="#space.Groups.ContainsKey("MySQL4")#">MySQL 4.x
---------
Maximum Number of Databases: <ad:NumericQuota quota="MySQL4.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MySQL4.Users" />

In order to connect to MySQL 4.x server you can use the following address:

   #MySQL4Address#
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MySQL5")#">MySQL 5.x
---------
Maximum Number of Databases: <ad:NumericQuota quota="MySQL5.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MySQL5.Users" />

In order to connect to MySQL 5.x server you can use the following address:

   #MySQL5Address#
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MariaDB")#">MariaDB
---------
Maximum Number of Databases: <ad:NumericQuota quota="MariaDB.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MariaDB.Users" />

</ad:if>

Microsoft Access
----------------
Microsoft Access database are automatically allowed in any hosting plan. You can create/upload any number of Access database from File Manager in control panel.

<ad:if test="#space.Groups.ContainsKey("Statistics")#">Web Statistics
==============
Maximum Number of Statistics Sites: <ad:NumericQuota quota="Stats.Sites" />

You can view advanced statistics from your domain using URL of the following form:

   http://stats.YourDomain.com
<ad:if test="#isnotempty(PreviewDomain)#">
During DNS propagation period (when domain name servers have been changed), you can access web site statistics with Temporary URL:

   http://stats.YourDomain.com.#PreviewDomain#
</ad:if>
</ad:if>


<ad:template name="MsSqlConnectionStrings">You may also use SQL Server address above in your application connection strings, for example:

Classic ASP (ADO Library): Provider=SQLOLEDB;Data source=#server#;Initial catalog=databaseName;User Id=userName;Password=password;
ASP.NET (ADO.NET Library): Server=#server#;Database=databaseName;Uid=userName;Password=password;
</ad:template>
<ad:template name="NumericQuota"><ad:if test="#space.Quotas.ContainsKey(quota)#"><ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if><ad:else>0</ad:if></ad:template>
<ad:template name="BooleanQuota"><ad:if test="#space.Quotas.ContainsKey(quota)#"><ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot 0#">Enabled<ad:else>Disabled</ad:if><ad:else>Disabled</ad:if></ad:template>

<ad:if test="#space.Groups.ContainsKey("VPS2012")#">VPS2012
========
CPUs: <ad:NumericQuota quota="VPS2012.CpuNumber" />
RAM: <ad:NumericQuota quota="VPS2012.Ram" />
HDD: <ad:NumericQuota quota="VPS2012.Hdd" /></ad:if>

<ad:if test="#space.Groups.ContainsKey("VPSForPC")#">VPSForPC
========
CPUs: <ad:NumericQuota quota="VPSForPC.CpuNumber" />
RAM: <ad:NumericQuota quota="VPSForPC.Ram" />
HDD: <ad:NumericQuota quota="VPSForPC.Hdd" /></ad:if>

<ad:if test="#space.Groups.ContainsKey("PROXMOX")#">PROXMOX
========
CPUs: <ad:NumericQuota quota="PROXMOX.CpuNumber" />
RAM: <ad:NumericQuota quota="PROXMOX.Ram" />
HDD: <ad:NumericQuota quota="PROXMOX.Hdd" /></ad:if>

<ad:if test="#Signup#">If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
HostingCompany.
Web Site: https://HostingCompany.com"
E-Mail: support@HostingCompany.com
</ad:if>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'CC', N'')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'From', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'HtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Account Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.3em; color: ##1F4978; }
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>
<div class="Header">
	Hosting Account Information
</div>

<p>
Hello #user.FirstName#,
</p>

<p>
Please, find below details of your control panel account. The one time password was generated for you. You should change the password after login. 
</p>

<h1>Control Panel URL</h1>
<table>
    <thead>
        <tr>
            <th>Control Panel URL</th>
            <th>Username</th>
            <th>One Time Password</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>
            <td>#user.Username#</td>
            <td>#user.Password#</td>
        </tr>
    </tbody>
</table>


<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards,<br />
SolidCP.<br />
Web Site: <a href="https://solidcp.com">https://solidcp.com</a><br />
E-Mail: <a href="mailto:support@solidcp.com">support@solidcp.com</a>
</p>

</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'Priority', N'Normal')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'Subject', N'Password reminder for #user.FirstName# #user.LastName#')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'TextBody', N'=================================
   Hosting Account Information
=================================

Hello #user.FirstName#,

Please, find below details of your control panel account. The one time password was generated for you. You should change the password after login.

Control Panel URL: https://panel.solidcp.com
Username: #user.Username#
One Time Password: #user.Password#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
SolidCP.
Web Site: https://solidcp.com"
E-Mail: support@solidcp.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SharePointPolicy', N'GroupNamePolicy', N'True;-;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SharePointPolicy', N'UserNamePolicy', N'True;-;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SharePointPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AddParkingPage', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AddRandomDomainString', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AnonymousAccountPolicy', N'True;;5;20;;_web;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AspInstalled', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AspNetInstalled', N'2')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'CgiBinInstalled', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'DefaultDocuments', N'Default.htm,Default.asp,index.htm,Default.aspx')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableAnonymousAccess', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableBasicAuthentication', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableDedicatedPool', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableDirectoryBrowsing', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableParentPaths', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableParkingPageTokens', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableWindowsAuthentication', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableWritePermissions', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'FrontPageAccountPolicy', N'True;;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'FrontPagePasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'ParkingPageContent', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>The web site is under construction</title>
<style type="text/css">
	H1 { font-size: 16pt; margin-bottom: 4px; }
	H2 { font-size: 14pt; margin-bottom: 4px; font-weight: normal; }
</style>
</head>
<body>
<div id="PageOutline">
	<h1>This web site has just been created from <a href="https://www.SolidCP.com">SolidCP </a> and it is still under construction.</h1>
	<h2>The web site is hosted by <a href="https://solidcp.com">SolidCP</a>.</h2>
</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'ParkingPageName', N'default.aspx')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'PerlInstalled', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'PhpInstalled', N'')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'PublishingProfile', N'<?xml version="1.0" encoding="utf-8"?>
<publishData>
<ad:if test="#WebSite.WebDeploySitePublishingEnabled#">
	<publishProfile
		profileName="#WebSite.Name# - Web Deploy"
		publishMethod="MSDeploy"
		publishUrl="#WebSite["WmSvcServiceUrl"]#:#WebSite["WmSvcServicePort"]#"
		msdeploySite="#WebSite.Name#"
		userName="#WebSite.WebDeployPublishingAccount#"
		userPWD="#WebSite.WebDeployPublishingPassword#"
		destinationAppUrl="http://#WebSite.Name#/"
		<ad:if test="#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#">SQLServerDBConnectionString="server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#"</ad:if>
		<ad:if test="#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#">mySQLDBConnectionString="server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#"</ad:if>
		<ad:if test="#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#">MariaDBDBConnectionString="server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#"</ad:if>
		hostingProviderForumLink="https://solidcp.com/support"
		controlPanelLink="https://panel.solidcp.com/"
	/>
</ad:if>
<ad:if test="#IsDefined("FtpAccount")#">
	<publishProfile
		profileName="#WebSite.Name# - FTP"
		publishMethod="FTP"
		publishUrl="ftp://#FtpServiceAddress#"
		ftpPassiveMode="True"
		userName="#FtpAccount.Name#"
		userPWD="#FtpAccount.Password#"
		destinationAppUrl="http://#WebSite.Name#/"
		<ad:if test="#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#">SQLServerDBConnectionString="server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#"</ad:if>
		<ad:if test="#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#">mySQLDBConnectionString="server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#"</ad:if>
		<ad:if test="#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#">MariaDBDBConnectionString="server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#"</ad:if>
		hostingProviderForumLink="https://solidcp.com/support"
		controlPanelLink="https://panel.solidcp.com/"
    />
</ad:if>
</publishData>

<!--
Control Panel:
Username: #User.Username#
Password: #User.Password#

Technical Contact:
support@solidcp.com
-->')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'PythonInstalled', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'SecuredGroupNamePolicy', N'True;;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'SecuredUserNamePolicy', N'True;;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'SecuredUserPasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'VirtDirNamePolicy', N'True;-;3;50;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'WebDataFolder', N'\[DOMAIN_NAME]\data')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'WebLogsFolder', N'\[DOMAIN_NAME]\logs')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'WebRootFolder', N'\[DOMAIN_NAME]\wwwroot')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SolidCPPolicy', N'DemoMessage', N'When user account is in demo mode the majority of operations are
disabled, especially those ones that modify or delete records.
You are welcome to ask your questions or place comments about
this demo on  <a href="http://forum.SolidCP.net"
target="_blank">SolidCP  Support Forum</a>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SolidCPPolicy', N'ForbiddenIP', N'')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SolidCPPolicy', N'PasswordPolicy', N'True;6;20;0;1;0;True')
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.0', CAST(0x00009D5400000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.0.1.0', CAST(0x00009DB500D453BD AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.0.2.0', CAST(0x00009DE600000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.1.0.9', CAST(0x00009E3000000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.1.2.13', CAST(0x00009EC600000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.2.0.38', CAST(0x00009F1F00000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.2.1.6', CAST(0x0000A02300000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'2.0.0.228', CAST(0x0000A12000000000 AS DateTime))
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[ExchangeAccountEmailAddresses] ADD  CONSTRAINT [IX_ExchangeAccountEmailAddresses_UniqueEmail] UNIQUE NONCLUSTERED
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
ALTER TABLE [dbo].[ExchangeAccounts] ADD  CONSTRAINT [IX_ExchangeAccounts_UniqueAccountName] UNIQUE NONCLUSTERED
(
	[AccountName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD  CONSTRAINT [IX_ExchangeMailboxPlans] UNIQUE NONCLUSTERED
(
	[MailboxPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains] ADD  CONSTRAINT [IX_ExchangeOrganizationDomains_UniqueDomain] UNIQUE NONCLUSTERED
(
	[DomainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
ALTER TABLE [dbo].[ExchangeOrganizations] ADD  CONSTRAINT [IX_ExchangeOrganizations_UniqueOrg] UNIQUE NONCLUSTERED
(
	[OrganizationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[LyncUserPlans] ADD  CONSTRAINT [IX_LyncUserPlans] UNIQUE NONCLUSTERED
(
	[LyncUserPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [IX_Users_Username] UNIQUE NONCLUSTERED
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[BlackBerryUsers] ADD  CONSTRAINT [DF_BlackBerryUsers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF_Comments_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[CRMUsers] ADD  CONSTRAINT [DF_Table_1_CreateDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[CRMUsers] ADD  CONSTRAINT [DF_CRMUsers_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_AllowedForHosting]  DEFAULT ((0)) FOR [HostingAllowed]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_SubDomainID]  DEFAULT ((0)) FOR [IsSubDomain]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_IsInstantAlias]  DEFAULT ((0)) FOR [IsInstantAlias]
GO
ALTER TABLE [dbo].[ExchangeAccounts] ADD  CONSTRAINT [DF__ExchangeA__Creat__59B045BD]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains] ADD  CONSTRAINT [DF_ExchangeOrganizationDomains_IsHost]  DEFAULT ((0)) FOR [IsHost]
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains] ADD  CONSTRAINT [DF_ExchangeOrganizationDomains_DomainTypeID]  DEFAULT ((0)) FOR [DomainTypeID]
GO
ALTER TABLE [dbo].[LyncUsers] ADD  CONSTRAINT [DF_LyncUsers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[LyncUsers] ADD  CONSTRAINT [DF_LyncUsers_ChangedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[OCSUsers] ADD  CONSTRAINT [DF_OCSUsers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OCSUsers] ADD  CONSTRAINT [DF_OCSUsers_ChangedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_Packages_OverrideQuotas]  DEFAULT ((0)) FOR [OverrideQuotas]
GO
ALTER TABLE [dbo].[Quotas] ADD  CONSTRAINT [DF_ResourceGroupQuotas_QuotaOrder]  DEFAULT ((1)) FOR [QuotaOrder]
GO
ALTER TABLE [dbo].[Quotas] ADD  CONSTRAINT [DF_ResourceGroupQuotas_QuotaTypeID]  DEFAULT ((2)) FOR [QuotaTypeID]
GO
ALTER TABLE [dbo].[Quotas] ADD  CONSTRAINT [DF_Quotas_ServiceQuota]  DEFAULT ((0)) FOR [ServiceQuota]
GO
ALTER TABLE [dbo].[ResourceGroupDnsRecords] ADD  CONSTRAINT [DF_ResourceGroupDnsRecords_RecordOrder]  DEFAULT ((1)) FOR [RecordOrder]
GO
ALTER TABLE [dbo].[ResourceGroups] ADD  CONSTRAINT [DF_ResourceGroups_GroupOrder]  DEFAULT ((1)) FOR [GroupOrder]
GO
ALTER TABLE [dbo].[ScheduleTaskParameters] ADD  CONSTRAINT [DF_ScheduleTaskParameters_ParameterOrder]  DEFAULT ((0)) FOR [ParameterOrder]
GO
ALTER TABLE [dbo].[Servers] ADD  CONSTRAINT [DF_Servers_DisplayName]  DEFAULT ('') FOR [ServerUrl]
GO
ALTER TABLE [dbo].[Servers] ADD  CONSTRAINT [DF_Servers_VirtualServer]  DEFAULT ((0)) FOR [VirtualServer]
GO
ALTER TABLE [dbo].[Servers] ADD  CONSTRAINT [DF_Servers_ADEnabled]  DEFAULT ((0)) FOR [ADEnabled]
GO
ALTER TABLE [dbo].[ServiceItemTypes] ADD  CONSTRAINT [DF_ServiceItemTypes_TypeOrder]  DEFAULT ((1)) FOR [TypeOrder]
GO
ALTER TABLE [dbo].[ServiceItemTypes] ADD  CONSTRAINT [DF_ServiceItemTypes_Importable]  DEFAULT ((1)) FOR [Importable]
GO
ALTER TABLE [dbo].[ServiceItemTypes] ADD  CONSTRAINT [DF_ServiceItemTypes_Backup]  DEFAULT ((1)) FOR [Backupable]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Demo]  DEFAULT ((0)) FOR [IsDemo]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsPeer]  DEFAULT ((0)) FOR [IsPeer]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_HtmlLetters]  DEFAULT ((1)) FOR [HtmlMail]
GO
ALTER TABLE [dbo].[BlackBerryUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlackBerryUsers_ExchangeAccounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
GO
ALTER TABLE [dbo].[BlackBerryUsers] CHECK CONSTRAINT [FK_BlackBerryUsers_ExchangeAccounts]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users]
GO
ALTER TABLE [dbo].[CRMUsers]  WITH CHECK ADD  CONSTRAINT [FK_CRMUsers_ExchangeAccounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
GO
ALTER TABLE [dbo].[CRMUsers] CHECK CONSTRAINT [FK_CRMUsers_ExchangeAccounts]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_Packages]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_ServiceItems_MailDomain] FOREIGN KEY([MailDomainID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_ServiceItems_MailDomain]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_ServiceItems_WebSite] FOREIGN KEY([WebSiteID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_ServiceItems_WebSite]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_ServiceItems_ZoneItem] FOREIGN KEY([ZoneItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_ServiceItems_ZoneItem]
GO
ALTER TABLE [dbo].[ExchangeAccountEmailAddresses]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeAccountEmailAddresses_ExchangeAccounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeAccountEmailAddresses] CHECK CONSTRAINT [FK_ExchangeAccountEmailAddresses_ExchangeAccounts]
GO
ALTER TABLE [dbo].[ExchangeAccounts]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeAccounts_ExchangeMailboxPlans] FOREIGN KEY([MailboxPlanId])
REFERENCES [dbo].[ExchangeMailboxPlans] ([MailboxPlanId])
GO
ALTER TABLE [dbo].[ExchangeAccounts] CHECK CONSTRAINT [FK_ExchangeAccounts_ExchangeMailboxPlans]
GO
ALTER TABLE [dbo].[ExchangeAccounts]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeAccounts_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeAccounts] CHECK CONSTRAINT [FK_ExchangeAccounts_ServiceItems]
GO
ALTER TABLE [dbo].[ExchangeMailboxPlans]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeMailboxPlans_ExchangeOrganizations] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ExchangeOrganizations] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeMailboxPlans] CHECK CONSTRAINT [FK_ExchangeMailboxPlans_ExchangeOrganizations]
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeOrganizationDomains_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains] CHECK CONSTRAINT [FK_ExchangeOrganizationDomains_ServiceItems]
GO
ALTER TABLE [dbo].[ExchangeOrganizations]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeOrganizations_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeOrganizations] CHECK CONSTRAINT [FK_ExchangeOrganizations_ServiceItems]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_IPAddresses] FOREIGN KEY([IPAddressID])
REFERENCES [dbo].[IPAddresses] ([AddressID])
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_IPAddresses]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_Packages]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_Servers]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_Services]
GO
ALTER TABLE [dbo].[HostingPlanQuotas]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanQuotas_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HostingPlanQuotas] CHECK CONSTRAINT [FK_HostingPlanQuotas_HostingPlans]
GO
ALTER TABLE [dbo].[HostingPlanQuotas]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanQuotas_Quotas] FOREIGN KEY([QuotaID])
REFERENCES [dbo].[Quotas] ([QuotaID])
GO
ALTER TABLE [dbo].[HostingPlanQuotas] CHECK CONSTRAINT [FK_HostingPlanQuotas_Quotas]
GO
ALTER TABLE [dbo].[HostingPlanResources]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanResources_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HostingPlanResources] CHECK CONSTRAINT [FK_HostingPlanResources_HostingPlans]
GO
ALTER TABLE [dbo].[HostingPlanResources]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanResources_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[HostingPlanResources] CHECK CONSTRAINT [FK_HostingPlanResources_ResourceGroups]
GO
ALTER TABLE [dbo].[HostingPlans]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlans_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HostingPlans] CHECK CONSTRAINT [FK_HostingPlans_Packages]
GO
ALTER TABLE [dbo].[HostingPlans]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlans_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[HostingPlans] CHECK CONSTRAINT [FK_HostingPlans_Servers]
GO
ALTER TABLE [dbo].[HostingPlans]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlans_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[HostingPlans] CHECK CONSTRAINT [FK_HostingPlans_Users]
GO
ALTER TABLE [dbo].[IPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_IPAddresses_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IPAddresses] CHECK CONSTRAINT [FK_IPAddresses_Servers]
GO
ALTER TABLE [dbo].[LyncUserPlans]  WITH CHECK ADD  CONSTRAINT [FK_LyncUserPlans_ExchangeOrganizations] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ExchangeOrganizations] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LyncUserPlans] CHECK CONSTRAINT [FK_LyncUserPlans_ExchangeOrganizations]
GO
ALTER TABLE [dbo].[LyncUsers]  WITH CHECK ADD  CONSTRAINT [FK_LyncUsers_LyncUserPlans] FOREIGN KEY([LyncUserPlanID])
REFERENCES [dbo].[LyncUserPlans] ([LyncUserPlanId])
GO
ALTER TABLE [dbo].[LyncUsers] CHECK CONSTRAINT [FK_LyncUsers_LyncUserPlans]
GO
ALTER TABLE [dbo].[PackageAddons]  WITH CHECK ADD  CONSTRAINT [FK_PackageAddons_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
GO
ALTER TABLE [dbo].[PackageAddons] CHECK CONSTRAINT [FK_PackageAddons_HostingPlans]
GO
ALTER TABLE [dbo].[PackageAddons]  WITH CHECK ADD  CONSTRAINT [FK_PackageAddons_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageAddons] CHECK CONSTRAINT [FK_PackageAddons_Packages]
GO
ALTER TABLE [dbo].[PackageIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PackageIPAddresses_IPAddresses] FOREIGN KEY([AddressID])
REFERENCES [dbo].[IPAddresses] ([AddressID])
GO
ALTER TABLE [dbo].[PackageIPAddresses] CHECK CONSTRAINT [FK_PackageIPAddresses_IPAddresses]
GO
ALTER TABLE [dbo].[PackageIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PackageIPAddresses_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageIPAddresses] CHECK CONSTRAINT [FK_PackageIPAddresses_Packages]
GO
ALTER TABLE [dbo].[PackageIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PackageIPAddresses_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[PackageIPAddresses] CHECK CONSTRAINT [FK_PackageIPAddresses_ServiceItems]
GO
ALTER TABLE [dbo].[PackageQuotas]  WITH CHECK ADD  CONSTRAINT [FK_PackageQuotas_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackageQuotas] CHECK CONSTRAINT [FK_PackageQuotas_Packages]
GO
ALTER TABLE [dbo].[PackageQuotas]  WITH CHECK ADD  CONSTRAINT [FK_PackageQuotas_Quotas] FOREIGN KEY([QuotaID])
REFERENCES [dbo].[Quotas] ([QuotaID])
GO
ALTER TABLE [dbo].[PackageQuotas] CHECK CONSTRAINT [FK_PackageQuotas_Quotas]
GO
ALTER TABLE [dbo].[PackageResources]  WITH CHECK ADD  CONSTRAINT [FK_PackageResources_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackageResources] CHECK CONSTRAINT [FK_PackageResources_Packages]
GO
ALTER TABLE [dbo].[PackageResources]  WITH CHECK ADD  CONSTRAINT [FK_PackageResources_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[PackageResources] CHECK CONSTRAINT [FK_PackageResources_ResourceGroups]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_HostingPlans]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_Packages] FOREIGN KEY([ParentPackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_Packages]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_Servers]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_Users]
GO
ALTER TABLE [dbo].[PackagesBandwidth]  WITH CHECK ADD  CONSTRAINT [FK_PackagesBandwidth_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesBandwidth] CHECK CONSTRAINT [FK_PackagesBandwidth_Packages]
GO
ALTER TABLE [dbo].[PackagesBandwidth]  WITH CHECK ADD  CONSTRAINT [FK_PackagesBandwidth_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[PackagesBandwidth] CHECK CONSTRAINT [FK_PackagesBandwidth_ResourceGroups]
GO
ALTER TABLE [dbo].[PackagesDiskspace]  WITH CHECK ADD  CONSTRAINT [FK_PackagesDiskspace_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesDiskspace] CHECK CONSTRAINT [FK_PackagesDiskspace_Packages]
GO
ALTER TABLE [dbo].[PackagesDiskspace]  WITH CHECK ADD  CONSTRAINT [FK_PackagesDiskspace_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[PackagesDiskspace] CHECK CONSTRAINT [FK_PackagesDiskspace_ResourceGroups]
GO
ALTER TABLE [dbo].[PackageServices]  WITH CHECK ADD  CONSTRAINT [FK_PackageServices_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageServices] CHECK CONSTRAINT [FK_PackageServices_Packages]
GO
ALTER TABLE [dbo].[PackageServices]  WITH CHECK ADD  CONSTRAINT [FK_PackageServices_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageServices] CHECK CONSTRAINT [FK_PackageServices_Services]
GO
ALTER TABLE [dbo].[PackagesTreeCache]  WITH CHECK ADD  CONSTRAINT [FK_PackagesTreeCache_Packages] FOREIGN KEY([ParentPackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesTreeCache] CHECK CONSTRAINT [FK_PackagesTreeCache_Packages]
GO
ALTER TABLE [dbo].[PackagesTreeCache]  WITH CHECK ADD  CONSTRAINT [FK_PackagesTreeCache_Packages1] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesTreeCache] CHECK CONSTRAINT [FK_PackagesTreeCache_Packages1]
GO
ALTER TABLE [dbo].[PrivateIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PrivateIPAddresses_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrivateIPAddresses] CHECK CONSTRAINT [FK_PrivateIPAddresses_ServiceItems]
GO
ALTER TABLE [dbo].[Providers]  WITH CHECK ADD  CONSTRAINT [FK_Providers_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[Providers] CHECK CONSTRAINT [FK_Providers_ResourceGroups]
GO
ALTER TABLE [dbo].[Quotas]  WITH CHECK ADD  CONSTRAINT [FK_Quotas_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Quotas] CHECK CONSTRAINT [FK_Quotas_ResourceGroups]
GO
ALTER TABLE [dbo].[Quotas]  WITH CHECK ADD  CONSTRAINT [FK_Quotas_ServiceItemTypes] FOREIGN KEY([ItemTypeID])
REFERENCES [dbo].[ServiceItemTypes] ([ItemTypeID])
GO
ALTER TABLE [dbo].[Quotas] CHECK CONSTRAINT [FK_Quotas_ServiceItemTypes]
GO
ALTER TABLE [dbo].[ResourceGroupDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_ResourceGroupDnsRecords_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResourceGroupDnsRecords] CHECK CONSTRAINT [FK_ResourceGroupDnsRecords_ResourceGroups]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK_Schedule_Packages]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_ScheduleTasks] FOREIGN KEY([TaskID])
REFERENCES [dbo].[ScheduleTasks] ([TaskID])
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK_Schedule_ScheduleTasks]
GO
ALTER TABLE [dbo].[ScheduleParameters]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleParameters_Schedule] FOREIGN KEY([ScheduleID])
REFERENCES [dbo].[Schedule] ([ScheduleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduleParameters] CHECK CONSTRAINT [FK_ScheduleParameters_Schedule]
GO
ALTER TABLE [dbo].[ScheduleTaskParameters]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleTaskParameters_ScheduleTasks] FOREIGN KEY([TaskID])
REFERENCES [dbo].[ScheduleTasks] ([TaskID])
GO
ALTER TABLE [dbo].[ScheduleTaskParameters] CHECK CONSTRAINT [FK_ScheduleTaskParameters_ScheduleTasks]
GO
ALTER TABLE [dbo].[ScheduleTaskViewConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration] FOREIGN KEY([TaskID])
REFERENCES [dbo].[ScheduleTasks] ([TaskID])
GO
ALTER TABLE [dbo].[ScheduleTaskViewConfiguration] CHECK CONSTRAINT [FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration]
GO
ALTER TABLE [dbo].[Servers]  WITH CHECK ADD  CONSTRAINT [FK_Servers_ResourceGroups] FOREIGN KEY([PrimaryGroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[Servers] CHECK CONSTRAINT [FK_Servers_ResourceGroups]
GO
ALTER TABLE [dbo].[ServiceDefaultProperties]  WITH CHECK ADD  CONSTRAINT [FK_ServiceDefaultProperties_Providers] FOREIGN KEY([ProviderID])
REFERENCES [dbo].[Providers] ([ProviderID])
GO
ALTER TABLE [dbo].[ServiceDefaultProperties] CHECK CONSTRAINT [FK_ServiceDefaultProperties_Providers]
GO
ALTER TABLE [dbo].[ServiceItemProperties]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItemProperties_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceItemProperties] CHECK CONSTRAINT [FK_ServiceItemProperties_ServiceItems]
GO
ALTER TABLE [dbo].[ServiceItems]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItems_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[ServiceItems] CHECK CONSTRAINT [FK_ServiceItems_Packages]
GO
ALTER TABLE [dbo].[ServiceItems]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItems_ServiceItemTypes] FOREIGN KEY([ItemTypeID])
REFERENCES [dbo].[ServiceItemTypes] ([ItemTypeID])
GO
ALTER TABLE [dbo].[ServiceItems] CHECK CONSTRAINT [FK_ServiceItems_ServiceItemTypes]
GO
ALTER TABLE [dbo].[ServiceItems]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItems_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
GO
ALTER TABLE [dbo].[ServiceItems] CHECK CONSTRAINT [FK_ServiceItems_Services]
GO
ALTER TABLE [dbo].[ServiceItemTypes]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItemTypes_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[ServiceItemTypes] CHECK CONSTRAINT [FK_ServiceItemTypes_ResourceGroups]
GO
ALTER TABLE [dbo].[ServiceProperties]  WITH CHECK ADD  CONSTRAINT [FK_ServiceProperties_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceProperties] CHECK CONSTRAINT [FK_ServiceProperties_Services]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_Clusters] FOREIGN KEY([ClusterID])
REFERENCES [dbo].[Clusters] ([ClusterID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Clusters]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_Providers] FOREIGN KEY([ProviderID])
REFERENCES [dbo].[Providers] ([ProviderID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Providers]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Servers]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users]
GO
ALTER TABLE [dbo].[UserSettings]  WITH CHECK ADD  CONSTRAINT [FK_UserSettings_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSettings] CHECK CONSTRAINT [FK_UserSettings_Users]
GO
ALTER TABLE [dbo].[VirtualGroups]  WITH CHECK ADD  CONSTRAINT [FK_VirtualGroups_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[VirtualGroups] CHECK CONSTRAINT [FK_VirtualGroups_ResourceGroups]
GO
ALTER TABLE [dbo].[VirtualGroups]  WITH CHECK ADD  CONSTRAINT [FK_VirtualGroups_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualGroups] CHECK CONSTRAINT [FK_VirtualGroups_Servers]
GO
ALTER TABLE [dbo].[VirtualServices]  WITH CHECK ADD  CONSTRAINT [FK_VirtualServices_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualServices] CHECK CONSTRAINT [FK_VirtualServices_Servers]
GO
ALTER TABLE [dbo].[VirtualServices]  WITH CHECK ADD  CONSTRAINT [FK_VirtualServices_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
GO
ALTER TABLE [dbo].[VirtualServices] CHECK CONSTRAINT [FK_VirtualServices_Services]
GO

/****** Object:  StoredProcedure [dbo].[GetSearchObject]    Script Date: 07.09.2015 15:29:17 ******/
DROP PROCEDURE [dbo].[GetSearchObject]
GO

/****** Object:  StoredProcedure [dbo].[GetSearchObject]    Script Date: 07.09.2015 15:29:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
DECLARE @sql nvarchar(4000)

/*------------------------------------------------Users---------------------------------------------------------------*/
DECLARE @columnUsername nvarchar(20)  
SET @columnUsername = 'Username'

DECLARE @columnEmail nvarchar(20)  
SET @columnEmail = 'Email'

DECLARE @columnCompanyName nvarchar(20)  
SET @columnCompanyName = 'CompanyName'

DECLARE @columnFullName nvarchar(20)  
SET @columnFullName = 'FullName'

IF @FilterColumn = '' AND @FilterValue <> ''
SET @FilterColumn = 'TextSearch'

SET @sql = '
DECLARE @Users TABLE
(
 ItemPosition int IDENTITY(0,1),
 UserID int,
 Username nvarchar(100),
 Fullname nvarchar(100)
)
INSERT INTO @Users (UserID, Username, Fullname)
SELECT 
 U.UserID,
 U.Username,
 U.FirstName + '' '' + U.LastName as Fullname
FROM UsersDetailed AS U
WHERE 
 U.UserID <> @UserID AND U.IsPeer = 0 AND
 (
  (@Recursive = 0 AND OwnerID = @UserID) OR
  (@Recursive = 1 AND dbo.CheckUserParent(@UserID, U.UserID) = 1)
 )
 AND ((@StatusID = 0) OR (@StatusID > 0 AND U.StatusID = @StatusID))
 AND ((@RoleID = 0) OR (@RoleID > 0 AND U.RoleID = @RoleID))
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
IF @FilterValue <> ''
 SET @sql = @sql + ' WHERE TextSearch LIKE ''' + @FilterValue + ''''
SET @sql = @sql + ' ORDER BY TextSearch'

SET @sql = @sql + ';open @curValue'

exec sp_executesql @sql, N'@UserID int, @FilterValue nvarchar(50), @Recursive bit, @StatusID int, @RoleID int, @columnUsername nvarchar(20), @columnEmail nvarchar(20), @columnCompanyName nvarchar(20), @columnFullName nvarchar(20), @curValue cursor output',
@UserID, @FilterValue, @Recursive, @StatusID, @RoleID, @columnUsername, @columnEmail, @columnCompanyName, @columnFullName, @curUsers output

/*--------------------------------------------Space----------------------------------------------------------*/
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
'

SET @sql = '
 DECLARE @ItemsService TABLE
 (
  ItemID int,
  Username nvarchar(100),
  Fullname nvarchar(100)
 )
 INSERT INTO @ItemsService (ItemID, Username, Fullname)
 SELECT
  SI.ItemID,
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
 INSERT INTO @ItemsDomain (ItemID, Username, Fullname)
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
 WHERE (STYPE.Searchable = 1
 AND STYPE.ItemTypeID <> 200 AND STYPE.ItemTypeID <> 201)'
IF @FilterValue <> ''
 SET @sql = @sql + ' AND (SI.ItemName LIKE ''' + @FilterValue + ''')'
SET @sql = @sql + '
 UNION (
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
 WHERE (D.IsDomainPointer=0)'
IF @FilterValue <> ''
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
IF @FilterValue <> ''
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
IF @FilterValue <> ''
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
  ''Mailbox'' as FullType,
  SI3.PackageID as PackageID,
  EAEA.AccountID as AccountID,
  I3.Username,
  I3.Fullname
 FROM @ItemsService AS I3
 INNER JOIN ServiceItems AS SI3 ON I3.ItemID = SI3.ItemID
 INNER JOIN ExchangeAccountEmailAddresses AS EAEA ON I3.ItemID = EAEA.AccountID'
IF @FilterValue <> ''
 SET @sql = @sql + ' WHERE (EAEA.EmailAddress LIKE ''' + @FilterValue + ''')'
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

/*-------------------------------------------Lync-----------------------------------------------------*/
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
  AND (' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
IF @FilterValue <> ''
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

/*------------------------------------RDS------------------------------------------------*/
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
	 AND (' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
	IF @FilterValue <> ''
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

/*------------------------------------CRM------------------------------------------------*/
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
  AND (' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
IF @FilterValue <> ''
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

/*------------------------------------VirtualServer------------------------------------------------*/
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
	IF @FilterValue <> ''
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

/*------------------------------------WebDAVFolder------------------------------------------------*/
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
  AND (' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)'
IF @FilterValue <> ''
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

/*------------------------------------SharePoint------------------------------------------------*/
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
AND (' + CAST((@IsAdmin) AS varchar(12)) + ' = 1 OR P.UserID = @UserID)
AND (SIT.DisplayName = ''SharePointFoundationSiteCollection''
	OR SIT.DisplayName = ''SharePointEnterpriseSiteCollection'')
AND SIP.PropertyName = ''OrganizationId''
AND T.PropertyName = ''PhysicalAddress'''
IF @FilterValue <> ''
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

/*-------------------------------------------@curAll-------------------------------------------------------*/
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

/*-------------------------------------------Return-------------------------------------------------------*/
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

IF (@ColType = '' OR @ColType IN ('AccountHome'))
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
		IF (1 = 1)'

	IF @FullType <> ''
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
	IF (1 = 1)'

IF @ColType <> ''
SET @sql = @sql + ' AND @ColumnType in ( ' + @ColType + ' ) ';

IF @FullType <> ''
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
  ItemPosition int IDENTITY(1,1),
  ItemID int,
  TextSearch nvarchar(500),
  ColumnType nvarchar(50),
  FullType nvarchar(50),
  PackageID int,
  AccountID int,
  Username nvarchar(100),
  Fullname nvarchar(100)
 )'

IF (@ColType = '' OR @ColType IN ('AccountHome'))
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
SET @sql = @sql + ' ORDER BY ' +  @SortColumn

SET @sql = @sql + ';
SELECT COUNT(ItemID) FROM @ItemsReturn;
SELECT DISTINCT(ColumnType) FROM @ItemsReturn';
IF @FullType <> ''
	SET @sql = @sql + ' WHERE FullType = ''' + @FullType + '''';

SET @sql = @sql + ';
SELECT ItemPosition, ItemID, TextSearch, ColumnType, FullType, PackageID, AccountID, Username, Fullname
FROM @ItemsReturn AS IR'

IF  @MaximumRows > 0
	SET @sql = @sql + ' WHERE IR.ItemPosition BETWEEN @StartRow AND @EndRow';

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @FilterValue nvarchar(50), @curUsersValue cursor, @curAllValue cursor',
	@StartRow, @MaximumRows, @FilterValue, @curUsers, @curAll

CLOSE @curAll
DEALLOCATE @curAll

RETURN
GO

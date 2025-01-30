-- USE [${install.database}]
-- GO
-- update database version
-- DECLARE @build_version nvarchar(10), @build_date datetime
-- SET @build_version = N'${release.version}'
-- SET @build_date = '${release.date}T00:00:00' -- ISO 8601 Format (YYYY-MM-DDTHH:MM:SS)

-- IF NOT EXISTS (SELECT * FROM [dbo].[Versions] WHERE [DatabaseVersion] = @build_version)
-- BEGIN
-- 	INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
-- END
-- GO

-- Fix for Some problems with collate in GetDnsRecordsTotal
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetDnsRecordsTotal')
DROP PROCEDURE GetDnsRecordsTotal
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


-- SimpleDNS 6.x
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1703')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1703, 7, N'SimpleDNS', N'SimpleDNS Plus 6.x', N'SolidCP.Providers.DNS.SimpleDNS6, SolidCP.Providers.DNS.SimpleDNS60', N'SimpleDNS', NULL)
END
GO

-- SimpleDNS 8.x
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1901')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1901, 7, N'SimpleDNS', N'SimpleDNS Plus 8.x', N'SolidCP.Providers.DNS.SimpleDNS8, SolidCP.Providers.DNS.SimpleDNS80', N'SimpleDNS', NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1901')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'AdminLogin', N'Admin')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'ExpireLimit', N'1209600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'MinimumTTL', N'86400')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'RefreshInterval', N'3600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'RetryDelay', N'600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'SimpleDnsUrl', N'http://127.0.0.1:8053')
END
GO

-- MSSQL 2016 / Maria DB quota fix:

UPDATE [Quotas] SET [ItemTypeID] = '71' WHERE [QuotaID] = '701' AND [ItemTypeID] = '39'
GO
UPDATE [Quotas] SET [ItemTypeID] = '72' WHERE [QuotaID] = '702' AND [ItemTypeID] = '40'
GO

-- Mailcleaner additions

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupID] = '61')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (61, N'Exchange_anti_spam_Filters', 5, NULL, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1601')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1601, 61, N'MailCleaner', N'Mail Cleaner', N'SolidCP.Providers.Filters.MailCleaner, SolidCP.Providers.Filters.MailCleaner', N'MailCleaner', 'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = N'447')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (447, 61, 1, N'Filters.Enable', N'Enable Spam Filter', 1, 0, NULL, NULL)
END
GO

IF EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'Filter.Enable')
BEGIN
	UPDATE [dbo].[Quotas] SET [QuotaName] = 'Filters.Enable' WHERE [QuotaName] = 'Filter.Enable'
END
GO

IF EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Filters')
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupName] = 'Exchange_anti_spam_Filters' WHERE [GroupName] = 'Filters'
END
GO


IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetFilterURL')
DROP PROCEDURE [dbo].[GetFilterURL]
GO

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
GO





IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetFilterURLByHostingPlan')
DROP PROCEDURE [dbo].[GetFilterURLByHostingPlan]
GO





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
GO

--- Fix on version 1.0.1
DELETE FROM HostingPlanQuotas WHERE QuotaID = 340
GO
DELETE FROM HostingPlanQuotas WHERE QuotaID = 341
GO
DELETE FROM HostingPlanQuotas WHERE QuotaID = 342
GO
DELETE FROM HostingPlanQuotas WHERE QuotaID = 343
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE GroupID = 33 AND [GroupName] = 'VPS2012')
BEGIN
DELETE FROM HostingPlanResources WHERE GroupID = 33
END
GO


-- Version 1.0.1 section


IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted Microsoft Exchange Server 2013')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(91, 12, N'Exchange2013', N'Hosted Microsoft Exchange Server 2013', N'SolidCP.Providers.HostedSolution.Exchange2013, SolidCP.Providers.HostedSolution.Exchange2013', N'Exchange',	1)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted Microsoft Exchange Server 2013'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted Microsoft Exchange Server 2016')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(92, 12, N'Exchange2016', N'Hosted Microsoft Exchange Server 2016', N'SolidCP.Providers.HostedSolution.Exchange2016, SolidCP.Providers.HostedSolution.Exchange2016', N'Exchange',	NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted Microsoft Exchange Server 2016'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2007.AllowLitigationHold')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (420, 12, 24,N'Exchange2007.AllowLitigationHold',N'Allow Litigation Hold',1, 0 , NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2007.RecoverableItemsSpace')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (421, 12, 25,N'Exchange2007.RecoverableItemsSpace',N'Recoverable Items Space',2, 0 , NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'HeliconZoo')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (42, N'HeliconZoo', 2, N'SolidCP.EnterpriseServer.HeliconZooController', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HeliconZoo')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (135, 42, N'HeliconZoo', N'Web Application Engines', N'SolidCP.Providers.Web.HeliconZoo.HeliconZoo, SolidCP.Providers.Web.HeliconZoo', N'HeliconZoo', NULL)
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='AllowLitigationHold')
BEGIN
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD
	[AllowLitigationHold] [bit] NULL,
	[RecoverableItemsWarningPct] [int] NULL,
	[RecoverableItemsSpace] [int] NULL
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='LitigationHoldUrl')
BEGIN
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD
	[LitigationHoldUrl] [nvarchar] (256) NULL,
	[LitigationHoldMsg] [nvarchar] (512) NULL
END
GO

-- 	UPDATE Domains SET IsDomainPointer=0, DomainItemID=NULL WHERE MailDomainID IS NOT NULL AND isDomainPointer=1

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted MS CRM 2011')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1201, 21, N'CRM', N'Hosted MS CRM 2011', N'SolidCP.Providers.HostedSolution.CRMProvider2011, SolidCP.Providers.HostedSolution.CRM2011', N'CRM', NULL)
END
GO


UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.CRMProvider2011, SolidCP.Providers.HostedSolution.CRM2011' WHERE ProviderID = 1201
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted SharePoint Foundation 2013')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery])
VALUES (1301, 20, N'HostedSharePoint2013', N'Hosted SharePoint Foundation 2013', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2013, SolidCP.Providers.HostedSolution.SharePoint2013', N'HostedSharePoint30', NULL)
END
GO

UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.HostedSharePointServer2013, SolidCP.Providers.HostedSolution.SharePoint2013' WHERE ProviderID = 1301
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted SharePoint Foundation 2016')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery])
VALUES (1306, 20, N'HostedSharePoint2016', N'Hosted SharePoint Foundation 2016', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2016, SolidCP.Providers.HostedSolution.SharePoint2016', N'HostedSharePoint30', NULL)
END
GO

UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.HostedSharePointServer2016, SolidCP.Providers.HostedSolution.SharePoint2016' WHERE ProviderID = 1306
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'Lync2013')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery])
VALUES (1401, 41, N'Lync2013', N'Microsoft Lync Server 2013 Multitenant Hosting Pack', N'SolidCP.Providers.HostedSolution.Lync2013, SolidCP.Providers.HostedSolution.Lync2013', N'Lync', NULL)
END
GO

UPDATE Providers SET DisplayName = N'Microsoft Lync Server 2013 Enterprise Edition' WHERE ProviderID = 1401
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'Lync2013HP')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery])
VALUES (1402, 41, N'Lync2013HP', N'Microsoft Lync Server 2013 Multitenant Hosting Pack', N'SolidCP.Providers.HostedSolution.Lync2013HP, SolidCP.Providers.HostedSolution.Lync2013HP', N'Lync', NULL)
END
GO

-- add Application Pools Restart Quota

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE ([QuotaName] = N'Web.AppPoolsRestart'))
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (411, 2, 13, N'Web.AppPoolsRestart', N'Application Pools Restart', 1, 0, NULL, NULL)
END
GO
-------------------------------- Scheduler Service------------------------------------------------------

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'Schedule' 
           AND  COLUMN_NAME = 'LastFinish')
ALTER TABLE Schedule
DROP COLUMN LastFinish
GO

ALTER PROCEDURE [dbo].[GetSchedule]
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

ALTER PROCEDURE [dbo].[GetScheduleInternal]
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

ALTER PROCEDURE [dbo].[GetSchedules]
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
	-- bug ISNULL(0, ...) always is not NULL
	-- ISNULL(0, (SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = ''SCHEDULER'' ORDER BY StartDate DESC)) AS LastResult,
	ISNULL((SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = 'SCHEDULER' ORDER BY StartDate DESC), 0) AS LastResult,

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

ALTER PROCEDURE [dbo].[UpdateSchedule]
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

UPDATE ScheduleTasks SET TaskType = RTRIM(TaskType) + '.Code'
WHERE SUBSTRING(RTRIM(TaskType), LEN(RTRIM(TaskType)) - 3, 4) <> 'Code'
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRunningSchedules')
DROP PROCEDURE GetRunningSchedules
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'BackgroundTaskStack')
DROP TABLE BackgroundTaskStack
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'BackgroundTaskLogs')
DROP TABLE BackgroundTaskLogs
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'BackgroundTaskParameters')
DROP TABLE BackgroundTaskParameters
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'BackgroundTasks')
DROP TABLE BackgroundTasks
GO

CREATE TABLE BackgroundTasks
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Guid UNIQUEIDENTIFIER NOT NULL,
	TaskID NVARCHAR(255),
	ScheduleID INT NOT NULL,
	PackageID INT NOT NULL,
	UserID INT NOT NULL,
	EffectiveUserID INT NOT NULL,
	TaskName NVARCHAR(255),
	ItemID INT,
	ItemName NVARCHAR(255),
	StartDate DATETIME NOT NULL,
	FinishDate DATETIME,
	IndicatorCurrent INT NOT NULL,
	IndicatorMaximum INT NOT NULL,
	MaximumExecutionTime INT NOT NULL,
	Source NVARCHAR(MAX),
	Severity INT NOT NULL,
	Completed BIT,
	NotifyOnComplete BIT,
	Status INT NOT NULL
)
GO

CREATE TABLE BackgroundTaskParameters
(
	ParameterID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TaskID INT NOT NULL,
	Name NVARCHAR(255),
	SerializerValue NTEXT,
	TypeName NVARCHAR(255),
	FOREIGN KEY (TaskID) REFERENCES BackgroundTasks (ID)
)
GO

CREATE TABLE BackgroundTaskLogs
(
	LogID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TaskID INT NOT NULL,
	Date DATETIME,
	ExceptionStackTrace NTEXT,
	InnerTaskStart INT,
	Severity INT,
	Text NTEXT,
	TextIdent INT,
	XmlParameters NTEXT,
	FOREIGN KEY (TaskID) REFERENCES BackgroundTasks (ID)
)
GO

CREATE TABLE BackgroundTaskStack
(
	TaskStackID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TaskID INT NOT NULL,
	FOREIGN KEY (TaskID) REFERENCES BackgroundTasks (ID)
)
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddBackgroundTask')
DROP PROCEDURE AddBackgroundTask
GO

CREATE PROCEDURE [dbo].[AddBackgroundTask]
(
	@BackgroundTaskID INT OUTPUT,
	@Guid UNIQUEIDENTIFIER,
	@TaskID NVARCHAR(255),
	@ScheduleID INT,
	@PackageID INT,
	@UserID INT,
	@EffectiveUserID INT,
	@TaskName NVARCHAR(255),
	@ItemID INT,
	@ItemName NVARCHAR(255),
	@StartDate DATETIME,
	@IndicatorCurrent INT,
	@IndicatorMaximum INT,
	@MaximumExecutionTime INT,
	@Source NVARCHAR(MAX),
	@Severity INT,
	@Completed BIT,
	@NotifyOnComplete BIT,
	@Status INT
)
AS

INSERT INTO BackgroundTasks
(
	Guid,
	TaskID,
	ScheduleID,
	PackageID,
	UserID,
	EffectiveUserID,
	TaskName,
	ItemID,
	ItemName,
	StartDate,
	IndicatorCurrent,
	IndicatorMaximum,
	MaximumExecutionTime,
	Source,
	Severity,
	Completed,
	NotifyOnComplete,
	Status
)
VALUES
(
	@Guid,
	@TaskID,
	@ScheduleID,
	@PackageID,
	@UserID,
	@EffectiveUserID,
	@TaskName,
	@ItemID,
	@ItemName,
	@StartDate,
	@IndicatorCurrent,
	@IndicatorMaximum,
	@MaximumExecutionTime,
	@Source,
	@Severity,
	@Completed,
	@NotifyOnComplete,
	@Status
)

SET @BackgroundTaskID = SCOPE_IDENTITY()

RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetBackgroundTask')
DROP PROCEDURE GetBackgroundTask
GO

CREATE PROCEDURE [dbo].[GetBackgroundTask]
(
	@TaskID NVARCHAR(255)
)
AS

SELECT TOP 1
	T.ID,
	T.Guid,
	T.TaskID,
	T.ScheduleID,
	T.PackageID,
	T.UserID,
	T.EffectiveUserID,
	T.TaskName,
	T.ItemID,
	T.ItemName,
	T.StartDate,
	T.FinishDate,
	T.IndicatorCurrent,
	T.IndicatorMaximum,
	T.MaximumExecutionTime,
	T.Source,
	T.Severity,
	T.Completed,
	T.NotifyOnComplete,
	T.Status
FROM BackgroundTasks AS T
INNER JOIN BackgroundTaskStack AS TS
	ON TS.TaskId = T.ID
WHERE T.TaskID = @TaskID 
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetBackgroundTasks')
DROP PROCEDURE GetBackgroundTasks
GO

CREATE PROCEDURE [dbo].[GetBackgroundTasks]
(
	@ActorID INT
)
AS

 with GetChildUsersId(id) as (
    select UserID
    from Users
    where UserID = @ActorID
    union all
    select C.UserId
    from GetChildUsersId P
    inner join Users C on P.id = C.OwnerID
)

SELECT 
	T.ID,
	T.Guid,
	T.TaskID,
	T.ScheduleId,
	T.PackageId,
	T.UserId,
	T.EffectiveUserId,
	T.TaskName,
	T.ItemId,
	T.ItemName,
	T.StartDate,
	T.FinishDate,
	T.IndicatorCurrent,
	T.IndicatorMaximum,
	T.MaximumExecutionTime,
	T.Source,
	T.Severity,
	T.Completed,
	T.NotifyOnComplete,
	T.Status
FROM BackgroundTasks AS T
INNER JOIN (SELECT T.Guid, MIN(T.StartDate) AS Date
			FROM BackgroundTasks AS T
			INNER JOIN BackgroundTaskStack AS TS
				ON TS.TaskId = T.ID
			WHERE T.UserID in (select id from GetChildUsersId)
			GROUP BY T.Guid) AS TT ON TT.Guid = T.Guid AND TT.Date = T.StartDate
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetThreadBackgroundTasks')
DROP PROCEDURE GetThreadBackgroundTasks
GO

CREATE PROCEDURE [dbo].GetThreadBackgroundTasks
(
	@Guid UNIQUEIDENTIFIER
)
AS

SELECT
	T.ID,
	T.Guid,
	T.TaskID,
	T.ScheduleId,
	T.PackageId,
	T.UserId,
	T.EffectiveUserId,
	T.TaskName,
	T.ItemId,
	T.ItemName,
	T.StartDate,
	T.FinishDate,
	T.IndicatorCurrent,
	T.IndicatorMaximum,
	T.MaximumExecutionTime,
	T.Source,
	T.Severity,
	T.Completed,
	T.NotifyOnComplete,
	T.Status
FROM BackgroundTasks AS T
INNER JOIN BackgroundTaskStack AS TS
	ON TS.TaskId = T.ID
WHERE T.Guid = @Guid
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetBackgroundTopTask')
DROP PROCEDURE GetBackgroundTopTask
GO

CREATE PROCEDURE [dbo].[GetBackgroundTopTask]
(
	@Guid UNIQUEIDENTIFIER
)
AS

SELECT TOP 1
	T.ID,
	T.Guid,
	T.TaskID,
	T.ScheduleId,
	T.PackageId,
	T.UserId,
	T.EffectiveUserId,
	T.TaskName,
	T.ItemId,
	T.ItemName,
	T.StartDate,
	T.FinishDate,
	T.IndicatorCurrent,
	T.IndicatorMaximum,
	T.MaximumExecutionTime,
	T.Source,
	T.Severity,
	T.Completed,
	T.NotifyOnComplete,
	T.Status
FROM BackgroundTasks AS T
INNER JOIN BackgroundTaskStack AS TS
	ON TS.TaskId = T.ID
WHERE T.Guid = @Guid
ORDER BY T.StartDate ASC
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddBackgroundTaskLog')
DROP PROCEDURE AddBackgroundTaskLog
GO

CREATE PROCEDURE [dbo].[AddBackgroundTaskLog]
(
	@TaskID INT,
	@Date DATETIME,
	@ExceptionStackTrace NTEXT,
	@InnerTaskStart INT,
	@Severity INT,
	@Text NTEXT,
	@TextIdent INT,
	@XmlParameters NTEXT
)
AS

INSERT INTO BackgroundTaskLogs
(
	TaskID,
	Date,
	ExceptionStackTrace,
	InnerTaskStart,
	Severity,
	Text,
	TextIdent,
	XmlParameters
)
VALUES
(
	@TaskID,
	@Date,
	@ExceptionStackTrace,
	@InnerTaskStart,
	@Severity,
	@Text,
	@TextIdent,
	@XmlParameters
)
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetBackgroundTaskLogs')
DROP PROCEDURE GetBackgroundTaskLogs
GO

CREATE PROCEDURE [dbo].[GetBackgroundTaskLogs]
(
	@TaskID INT,
	@StartLogTime DATETIME
)
AS

SELECT
	L.LogID,
	L.TaskID,
	L.Date,
	L.ExceptionStackTrace,
	L.InnerTaskStart,
	L.Severity,
	L.Text,
	L.XmlParameters
FROM BackgroundTaskLogs AS L
WHERE L.TaskID = @TaskID AND L.Date >= @StartLogTime
ORDER BY L.Date
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateBackgroundTask')
DROP PROCEDURE UpdateBackgroundTask
GO

CREATE PROCEDURE [dbo].[UpdateBackgroundTask]
(
	@Guid UNIQUEIDENTIFIER,
	@TaskID INT,
	@ScheduleID INT,
	@PackageID INT,
	@TaskName NVARCHAR(255),
	@ItemID INT,
	@ItemName NVARCHAR(255),
	@FinishDate DATETIME,
	@IndicatorCurrent INT,
	@IndicatorMaximum INT,
	@MaximumExecutionTime INT,
	@Source NVARCHAR(MAX),
	@Severity INT,
	@Completed BIT,
	@NotifyOnComplete BIT,
	@Status INT
)
AS

UPDATE BackgroundTasks
SET
	Guid = @Guid,
	ScheduleID = @ScheduleID,
	PackageID = @PackageID,
	TaskName = @TaskName,
	ItemID = @ItemID,
	ItemName = @ItemName,
	FinishDate = @FinishDate,
	IndicatorCurrent = @IndicatorCurrent,
	IndicatorMaximum = @IndicatorMaximum,
	MaximumExecutionTime = @MaximumExecutionTime,
	Source = @Source,
	Severity = @Severity,
	Completed = @Completed,
	NotifyOnComplete = @NotifyOnComplete,
	Status = @Status
WHERE ID = @TaskID
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetBackgroundTaskParams')
DROP PROCEDURE GetBackgroundTaskParams
GO

CREATE PROCEDURE [dbo].[GetBackgroundTaskParams]
(
	@TaskID INT
)
AS

SELECT
	P.ParameterID,
	P.TaskID,
	P.Name,
	P.SerializerValue,
	P.TypeName
FROM BackgroundTaskParameters AS P
WHERE P.TaskID = @TaskID
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddBackgroundTaskParam')
DROP PROCEDURE AddBackgroundTaskParam
GO

CREATE PROCEDURE [dbo].[AddBackgroundTaskParam]
(
	@TaskID INT,
	@Name NVARCHAR(255),
	@Value NTEXT,
	@TypeName NVARCHAR(255)
)
AS

INSERT INTO BackgroundTaskParameters
(
	TaskID,
	Name,
	SerializerValue,
	TypeName
)
VALUES
(
	@TaskID,
	@Name,
	@Value,
	@TypeName
)
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteBackgroundTaskParams')
DROP PROCEDURE DeleteBackgroundTaskParams
GO

CREATE PROCEDURE [dbo].[DeleteBackgroundTaskParams]
(
	@TaskID INT
)
AS

DELETE FROM BackgroundTaskParameters
WHERE TaskID = @TaskID
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddBackgroundTaskStack')
DROP PROCEDURE AddBackgroundTaskStack
GO

CREATE PROCEDURE [dbo].[AddBackgroundTaskStack]
(
	@TaskID INT
)
AS

INSERT INTO BackgroundTaskStack
(
	TaskID
)
VALUES
(
	@TaskID
)
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteBackgroundTasks')
DROP PROCEDURE DeleteBackgroundTasks
GO

CREATE PROCEDURE [dbo].[DeleteBackgroundTasks]
(
	@Guid UNIQUEIDENTIFIER
)
AS

DELETE FROM BackgroundTaskStack
WHERE TaskID IN (SELECT ID FROM BackgroundTasks WHERE Guid = @Guid)

DELETE FROM BackgroundTaskLogs
WHERE TaskID IN (SELECT ID FROM BackgroundTasks WHERE Guid = @Guid)

DELETE FROM BackgroundTaskParameters
WHERE TaskID IN (SELECT ID FROM BackgroundTasks WHERE Guid = @Guid)

DELETE FROM BackgroundTasks
WHERE ID IN (SELECT ID FROM BackgroundTasks WHERE Guid = @Guid)
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteBackgroundTask')
DROP PROCEDURE DeleteBackgroundTask
GO

CREATE PROCEDURE [dbo].[DeleteBackgroundTask]
(
	@ID INT
)
AS

DELETE FROM BackgroundTaskStack
WHERE TaskID = @ID

DELETE FROM BackgroundTaskLogs
WHERE TaskID = @ID

DELETE FROM BackgroundTaskParameters
WHERE TaskID = @ID

DELETE FROM BackgroundTasks
WHERE ID = @ID
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetProcessBackgroundTasks')
DROP PROCEDURE GetProcessBackgroundTasks
GO

CREATE PROCEDURE [dbo].[GetProcessBackgroundTasks]
(	
	@Status INT
)
AS

SELECT
	T.ID,
	T.TaskID,
	T.ScheduleId,
	T.PackageId,
	T.UserId,
	T.EffectiveUserId,
	T.TaskName,
	T.ItemId,
	T.ItemName,
	T.StartDate,
	T.FinishDate,
	T.IndicatorCurrent,
	T.IndicatorMaximum,
	T.MaximumExecutionTime,
	T.Source,
	T.Severity,
	T.Completed,
	T.NotifyOnComplete,
	T.Status
FROM BackgroundTasks AS T
WHERE T.Completed = 0 AND T.Status = @Status
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetScheduleBackgroundTasks')
DROP PROCEDURE GetScheduleBackgroundTasks
GO

CREATE PROCEDURE [dbo].[GetScheduleBackgroundTasks]
(
	@ScheduleID INT
)
AS

SELECT
	T.ID,
	T.Guid,
	T.TaskID,
	T.ScheduleId,
	T.PackageId,
	T.UserId,
	T.EffectiveUserId,
	T.TaskName,
	T.ItemId,
	T.ItemName,
	T.StartDate,
	T.FinishDate,
	T.IndicatorCurrent,
	T.IndicatorMaximum,
	T.MaximumExecutionTime,
	T.Source,
	T.Severity,
	T.Completed,
	T.NotifyOnComplete,
	T.Status
FROM BackgroundTasks AS T
WHERE T.Guid = (
	SELECT Guid FROM BackgroundTasks
	WHERE ScheduleID = @ScheduleID
		AND Completed = 0 AND Status IN (1, 3))
GO

-- Disclaimers


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ExchangeDisclaimers')
ALTER TABLE [dbo].[ExchangeDisclaimers] ALTER COLUMN [DisclaimerText] [nvarchar](MAX) NULL; 
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ExchangeDisclaimers')

CREATE TABLE [dbo].[ExchangeDisclaimers](
        [ExchangeDisclaimerId] [int] IDENTITY(1,1) NOT NULL,
        [ItemID] [int] NOT NULL,
        [DisclaimerName] [nvarchar](300) NOT NULL,
        [DisclaimerText] [nvarchar](MAX),
 CONSTRAINT [PK_ExchangeDisclaimers] PRIMARY KEY CLUSTERED
(
        [ExchangeDisclaimerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetResourceGroupByName')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetResourceGroupByName]
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

RETURN'
END

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetExchangeDisclaimers')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetExchangeDisclaimers]
(
	@ItemID int
)
AS
SELECT
	ExchangeDisclaimerId,
	ItemID,
	DisclaimerName,
	DisclaimerText
FROM
	ExchangeDisclaimers
WHERE
	ItemID = @ItemID 
ORDER BY DisclaimerName
RETURN'
END
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'DeleteExchangeDisclaimer')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[DeleteExchangeDisclaimer]
(
	@ExchangeDisclaimerId int
)
AS

DELETE FROM ExchangeDisclaimers
WHERE ExchangeDisclaimerId = @ExchangeDisclaimerId

RETURN'
END
GO

--

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateExchangeDisclaimer')
DROP PROCEDURE UpdateExchangeDisclaimer
GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'UpdateExchangeDisclaimer')
BEGIN
EXEC sp_executesql N' CREATE PROCEDURE [dbo].[UpdateExchangeDisclaimer] 
(
	@ExchangeDisclaimerId int,
	@DisclaimerName nvarchar(300),
	@DisclaimerText nvarchar(MAX)
)
AS

UPDATE ExchangeDisclaimers SET
	DisclaimerName = @DisclaimerName,
	DisclaimerText = @DisclaimerText

WHERE ExchangeDisclaimerId = @ExchangeDisclaimerId

RETURN'
END
GO

--

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddExchangeDisclaimer')
DROP PROCEDURE AddExchangeDisclaimer
GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'AddExchangeDisclaimer')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[AddExchangeDisclaimer] 
(
	@ExchangeDisclaimerId int OUTPUT,
	@ItemID int,
	@DisclaimerName	nvarchar(300),
	@DisclaimerText	nvarchar(MAX)
)
AS

INSERT INTO ExchangeDisclaimers
(
	ItemID,
	DisclaimerName,
	DisclaimerText
)
VALUES
(
	@ItemID,
	@DisclaimerName,
	@DisclaimerText
)

SET @ExchangeDisclaimerId = SCOPE_IDENTITY()

RETURN'
END
GO

--


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetExchangeDisclaimer')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetExchangeDisclaimer] 
(
	@ExchangeDisclaimerId int
)
AS
SELECT
	ExchangeDisclaimerId,
	ItemID,
	DisclaimerName,
	DisclaimerText
FROM
	ExchangeDisclaimers
WHERE
	ExchangeDisclaimerId = @ExchangeDisclaimerId
RETURN'
END
GO



IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeAccounts' AND COLS.name='ExchangeDisclaimerId')
BEGIN

ALTER TABLE [dbo].[ExchangeAccounts] ADD

[ExchangeDisclaimerId] [int] NULL

END
Go


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'SetExchangeAccountDisclaimerId')
BEGIN
EXEC sp_executesql N' CREATE PROCEDURE [dbo].[SetExchangeAccountDisclaimerId] 
(
	@AccountID int,
	@ExchangeDisclaimerId int
)
AS
UPDATE ExchangeAccounts SET
	ExchangeDisclaimerId = @ExchangeDisclaimerId
WHERE AccountID = @AccountID

RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetExchangeAccountDisclaimerId')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetExchangeAccountDisclaimerId] 
(
	@AccountID int
)
AS
SELECT
	ExchangeDisclaimerId
FROM
	ExchangeAccounts
WHERE
	AccountID= @AccountID
RETURN'
END
GO


-- add Disclaimers Quota   
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE ([QuotaName] = N'Exchange2007.DisclaimersAllowed'))
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (422, 12, 26, N'Exchange2007.DisclaimersAllowed', N'Disclaimers Allowed', 1, 0, NULL, NULL)   
END
GO



--add SecurityGroupManagement Quota
--IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedSolution.SecurityGroupManagement')
--BEGIN
--INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (423, 13, 5, N'HostedSolution.SecurityGroupManagement', N'Allow Security Group Management', 1, 0, NULL, NULL)
--END
--GO  

-- Lync Enterprise Voice

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='LyncUserPlans' AND COLS.name='TelephonyVoicePolicy')
BEGIN
ALTER TABLE [dbo].[LyncUserPlans] ADD 

[RemoteUserAccess] [bit] NOT NULL DEFAULT 0,
[PublicIMConnectivity] [bit] NOT NULL  DEFAULT 0,

[AllowOrganizeMeetingsWithExternalAnonymous] [bit] NOT NULL DEFAULT 0,

[Telephony] [int] NULL,

[ServerURI] [nvarchar](300) NULL,

[ArchivePolicy] [nvarchar](300) NULL,
[TelephonyDialPlanPolicy] [nvarchar](300) NULL,
[TelephonyVoicePolicy] [nvarchar](300) NULL


END
Go

-- 

DROP PROCEDURE AddLyncUserPlan;

DROP PROCEDURE UpdateLyncUserPlan;

DROP PROCEDURE DeleteLyncUserPlan;

--

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'DeleteLyncUserPlan')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[DeleteLyncUserPlan]
(
	@LyncUserPlanId int
)
AS

-- delete lyncuserplan
DELETE FROM LyncUserPlans
WHERE LyncUserPlanId = @LyncUserPlanId

RETURN'
END
GO

--

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'UpdateLyncUserPlan')
BEGIN
EXEC sp_executesql N' CREATE PROCEDURE [dbo].[UpdateLyncUserPlan] 
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
	@IsDefault bit,

	@RemoteUserAccess bit,
	@PublicIMConnectivity bit,

	@AllowOrganizeMeetingsWithExternalAnonymous bit,

	@Telephony int,

	@ServerURI nvarchar(300),
	
	@ArchivePolicy nvarchar(300),
	
	@TelephonyDialPlanPolicy nvarchar(300),
	@TelephonyVoicePolicy nvarchar(300)
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
	IsDefault = @IsDefault,

	RemoteUserAccess = @RemoteUserAccess,
	PublicIMConnectivity = @PublicIMConnectivity,

	AllowOrganizeMeetingsWithExternalAnonymous = @AllowOrganizeMeetingsWithExternalAnonymous,

	Telephony = @Telephony,

	ServerURI = @ServerURI,
	
	ArchivePolicy = @ArchivePolicy,
	TelephonyDialPlanPolicy = @TelephonyDialPlanPolicy,
	TelephonyVoicePolicy = @TelephonyVoicePolicy

WHERE LyncUserPlanId = @LyncUserPlanId


RETURN'
END
GO

--

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'AddLyncUserPlan')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[AddLyncUserPlan] 
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
	@LyncUserPlanName,
	@LyncUserPlanType,
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

SET @LyncUserPlanId = SCOPE_IDENTITY()

RETURN'		
END
GO

--

ALTER PROCEDURE [dbo].[GetLyncUserPlan] 
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
	IsDefault,

	RemoteUserAccess,
	PublicIMConnectivity,

	AllowOrganizeMeetingsWithExternalAnonymous,

	Telephony,

	ServerURI,
	
	ArchivePolicy,
	TelephonyDialPlanPolicy,
	TelephonyVoicePolicy

FROM
	LyncUserPlans
WHERE
	LyncUserPlanId = @LyncUserPlanId
RETURN
GO

--SfB
IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[SfBUserPlans]') AND type in (N'U'))
BEGIN
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
	[RemoteUserAccess] [bit] NOT NULL DEFAULT 0,
	[PublicIMConnectivity] [bit] NOT NULL  DEFAULT 0,
	[AllowOrganizeMeetingsWithExternalAnonymous] [bit] NOT NULL DEFAULT 0,
	[Telephony] [int] NULL,
	[ServerURI] [nvarchar](300) NULL,
	[ArchivePolicy] [nvarchar](300) NULL,
	[TelephonyDialPlanPolicy] [nvarchar](300) NULL,
	[TelephonyVoicePolicy] [nvarchar](300) NULL
 CONSTRAINT [PK_SfBUserPlans] PRIMARY KEY CLUSTERED
(
	[SfBUserPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
End


IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[SfBUsers]') AND type in (N'U'))
BEGIN
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
END

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'SfB')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (52, N'SfB', 25, NULL, 1)
END
GO

IF EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = 51 AND [QuotaName] LIKE ('SfB.%'))
BEGIN
DELETE FROM  [dbo].[Quotas] WHERE [GroupID] = 51 AND [QuotaName] LIKE ('SfB.%')
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='Quotas' AND COLS.name='PerOrganization')
BEGIN
	ALTER TABLE [dbo].[Quotas] ADD [PerOrganization] int NULL
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = 52 AND [QuotaName] LIKE ('SfB.%'))
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (581, 52, 12, N'SfB.PhoneNumbers', N'Phone Numbers', 2, 0, NULL, NULL, NULL), (582, 52, 1, N'SfB.Users', N'Users', 2, 0, NULL, NULL, 1), (583, 52, 2, N'SfB.Federation', N'Allow Federation', 1, 0, NULL, NULL, NULL), (584, 52, 3, N'SfB.Conferencing', N'Allow Conferencing', 1, 0, NULL, NULL, NULL), (585, 52, 4, N'SfB.MaxParticipants', N'Maximum Conference Particiapants', 3, 0, NULL, NULL, NULL), (586, 52, 5, N'SfB.AllowVideo', N'Allow Video in Conference', 1, 0, NULL, NULL, NULL), (587, 52, 6, N'SfB.EnterpriseVoice', N'Allow EnterpriseVoice', 1, 0, NULL, NULL, NULL), (588, 52, 7, N'SfB.EVUsers', N'Number of Enterprise Voice Users', 2, 0, NULL, NULL, NULL), (589, 52, 8, N'SfB.EVNational', N'Allow National Calls', 1, 0, NULL, NULL, NULL), (590, 52, 9, N'SfB.EVMobile', N'Allow Mobile Calls', 1, 0, NULL, NULL, NULL), (591, 52, 10, N'SfB.EVInternational', N'Allow International Calls', 1, 0, NULL, NULL, NULL), (592, 52, 11, N'SfB.EnablePlansEditing', N'Enable Plans Editing', 1, 0, NULL, NULL, NULL)
END
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'RemoteUserAccess', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [RemoteUserAccess] [bit] NOT NULL DEFAULT 0
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'PublicIMConnectivity', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [PublicIMConnectivity] [bit] NOT NULL  DEFAULT 0
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'AllowOrganizeMeetingsWithExternalAnonymous', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [AllowOrganizeMeetingsWithExternalAnonymous] [bit] NOT NULL DEFAULT 0
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'Telephony', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [Telephony] [int] NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'ServerURI', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [ServerURI] [nvarchar](300) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'ArchivePolicy', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [ArchivePolicy] [nvarchar](300) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'TelephonyDialPlanPolicy', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [TelephonyDialPlanPolicy] [nvarchar](300) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.SfBUserPlans'), 'TelephonyVoicePolicy', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE SfBUserPlans 
    ADD [TelephonyVoicePolicy] [nvarchar](300) NULL
END

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'SfBUserPlanID' AND Object_ID = Object_ID(N'ExchangeOrganizations'))
BEGIN
ALTER TABLE [dbo].[ExchangeOrganizations] ADD [SfBUserPlanID] [int] NULL;
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'SfB2015')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery])
VALUES (1403, 52, N'SfB2015', N'Microsoft Skype for Business Server 2015', N'SolidCP.Providers.HostedSolution.SfB2015, SolidCP.Providers.HostedSolution.SfB2015', N'SfB', NULL)
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'AddSfBUser')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[AddSfBUser]
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

RETURN'
END
GO

-- This is a check to ensure the AddSfBUserPlan is the correct version. If not drop and reimport
IF NOT EXISTS (Select OBJECT_NAME(object_id), OBJECT_DEFINITION(object_id) from sys.procedures Where name like 'AddSfBUserPlan' AND OBJECT_DEFINITION(object_id) like '%TelephonyVoicePolicy%')
Begin
DROP PROCEDURE [dbo].[AddSfBUserPlan]
END
GO

IF NOT EXISTS (Select OBJECT_NAME(object_id), OBJECT_DEFINITION(object_id) from sys.procedures Where name like 'AddSfBUserPlan' AND OBJECT_DEFINITION(object_id) like '%TelephonyVoicePolicy%')
Begin
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[AddSfBUserPlan] 
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
RETURN'
End
Go

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'CheckSfBUserExists')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[CheckSfBUserExists]
	@AccountID int
AS
BEGIN
	SELECT
		COUNT(AccountID)
	FROM
		dbo.SfBUsers
	WHERE AccountID = @AccountID
END
RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'DeleteSfBUser')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[DeleteSfBUser]
(
	@AccountId int
)
AS
DELETE FROM	SfBUsers WHERE	AccountId = @AccountId
RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'DeleteSfBUserPlan')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[DeleteSfBUserPlan]
(
	@SfBUserPlanId int
)
AS
-- delete SfBuserplan
DELETE FROM SfBUserPlans
WHERE SfBUserPlanId = @SfBUserPlanId
RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetSfBUserPlan')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetSfBUserPlan] 
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
	IsDefault,
	RemoteUserAccess,
	PublicIMConnectivity,
	AllowOrganizeMeetingsWithExternalAnonymous,
	Telephony,
	ServerURI,
	ArchivePolicy,
	TelephonyDialPlanPolicy,
	TelephonyVoicePolicy

FROM
	SfBUserPlans
WHERE
	SfBUserPlanId = @SfBUserPlanId
RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetSfBUserPlanByAccountId')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetSfBUserPlanByAccountId]
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
RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetSfBUserPlans')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetSfBUserPlans]
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
RETURN'
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetSfBUsers')
DROP PROCEDURE GetSfBUsers
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
BEGIN
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

	IF (@SortColumn = 'SfBUserPlanName')
	BEGIN
		SET @condition = 'ORDER BY lp.SfBUserPlanName'
	END

	DECLARE @sql nvarchar(3500)

	set @sql = '
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
			ea.ItemID = @ItemID ' + @condition

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
END
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetSfBUsersByPlanId')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetSfBUsersByPlanId]
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
		ou.SfBUserPlanId = @PlanId'
END
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetSfBUsersCount')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetSfBUsersCount]
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
	ea.ItemID = @ItemID'
END
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'SfBUserExists')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[SfBUserExists]
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


RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'SetOrganizationDefaultSfBUserPlan')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[SetOrganizationDefaultSfBUserPlan]
(
	@ItemID int,
	@SfBUserPlanId int
)
AS

UPDATE ExchangeOrganizations SET
	SfBUserPlanID = @SfBUserPlanId
WHERE
	ItemID = @ItemID

RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'SetSfBUserSfBUserPlan')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[SetSfBUserSfBUserPlan]
(
	@AccountID int,
	@SfBUserPlanId int
)
AS

UPDATE SfBUsers SET
	SfBUserPlanId = @SfBUserPlanId
WHERE
	AccountID = @AccountID

RETURN'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'UpdateSfBUser')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[UpdateSfBUser]
(
	@AccountID int,
	@SipAddress nvarchar(300)
)
AS

UPDATE SfBUsers SET
	SipAddress = @SipAddress
WHERE
	AccountID = @AccountID

RETURN'
END
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'UpdateSfBUserPlan')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[UpdateSfBUserPlan] 
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
	@IsDefault bit,
	@RemoteUserAccess bit,
	@PublicIMConnectivity bit,
	@AllowOrganizeMeetingsWithExternalAnonymous bit,
	@Telephony int,
	@ServerURI nvarchar(300),
	@ArchivePolicy nvarchar(300),
	@TelephonyDialPlanPolicy nvarchar(300),
	@TelephonyVoicePolicy nvarchar(300)
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
	IsDefault = @IsDefault,
	RemoteUserAccess = @RemoteUserAccess,
	PublicIMConnectivity = @PublicIMConnectivity,
	AllowOrganizeMeetingsWithExternalAnonymous = @AllowOrganizeMeetingsWithExternalAnonymous,
	Telephony = @Telephony,
	ServerURI = @ServerURI,
	ArchivePolicy = @ArchivePolicy,
	TelephonyDialPlanPolicy = @TelephonyDialPlanPolicy,
	TelephonyVoicePolicy = @TelephonyVoicePolicy

WHERE SfBUserPlanId = @SfBUserPlanId


RETURN'
END
GO

-- Lync Phone Numbers Quota

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Lync.PhoneNumbers')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (381, 41, 12, N'Lync.PhoneNumbers', N'Phone Numbers', 2, 0, NULL, NULL)
END
GO


-- Enterprise Storage Provider
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'EnterpriseStorage')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (44, N'EnterpriseStorage', 25, N'SolidCP.EnterpriseServer.EnterpriseStorageController', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Enterprise Storage Windows 2012')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(600, 44, N'EnterpriseStorage2012', N'Enterprise Storage Windows 2012', N'SolidCP.Providers.EnterpriseStorage.Windows2012, SolidCP.Providers.EnterpriseStorage.Windows2012', N'EnterpriseStorage',	1)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Enterprise Storage Windows 2012'
END
GO

-- Enterprise Storage Quotas
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'EnterpriseStorage.DiskStorageSpace')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (430, 44, 1,N'EnterpriseStorage.DiskStorageSpace',N'Disk Storage Space (Mb)',2, 0 , NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'EnterpriseStorage.Folders')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (431, 44, 1,N'EnterpriseStorage.Folders',N'Number of Root Folders',2, 0 , NULL)
END
GO


ALTER PROCEDURE [dbo].[SearchExchangeAccounts]
(
	@ActorID int,
	@ItemID int,
	@IncludeMailboxes bit,
	@IncludeContacts bit,
	@IncludeDistributionLists bit,
	@IncludeRooms bit,
	@IncludeEquipment bit,
	@IncludeSharedMailbox bit,
	@IncludeSecurityGroups bit,
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
OR (@IncludeEquipment = 1 AND EA.AccountType = 6)
OR (@IncludeSharedMailbox = 1 AND EA.AccountType = 10)
OR (@IncludeSecurityGroups = 1 AND EA.AccountType = 8))
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
    @IncludeDistributionLists int, @IncludeRooms bit, @IncludeEquipment bit, @IncludeSharedMailbox bit, @IncludeSecurityGroups bit',
@ItemID, @IncludeMailboxes, @IncludeContacts, @IncludeDistributionLists, @IncludeRooms, @IncludeEquipment, @IncludeSharedMailbox, @IncludeSecurityGroups

RETURN
GO


IF EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'SearchExchangeAccountsByTypes')
DROP PROCEDURE [dbo].[SearchExchangeAccountsByTypes]
GO


CREATE PROCEDURE [dbo].[SearchExchangeAccountsByTypes]
(
	@ActorID int,
	@ItemID int,
	@AccountTypes nvarchar(30),
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

DECLARE @condition nvarchar(700)
SET @condition = 'EA.ItemID = @ItemID AND EA.AccountType IN (' + @AccountTypes + ')'

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

DECLARE @sql nvarchar(3500)
SET @sql = '
SELECT
	EA.AccountID,
	EA.ItemID,
	EA.AccountType,
	EA.AccountName,
	EA.DisplayName,
	EA.PrimaryEmailAddress,
	EA.MailEnabledPublicFolder,
	EA.MailboxPlanId,
	P.MailboxPlan, 
	EA.SubscriberNumber,
	EA.UserPrincipalName
FROM
	ExchangeAccounts  AS EA
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON EA.MailboxPlanId = P.MailboxPlanId
	WHERE ' + @condition
	+ ' ORDER BY ' + @SortColumn

EXEC sp_executesql @sql, N'@ItemID int', @ItemID

RETURN
GO

---- Additional Default Groups-------------

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'AdditionalGroups')
DROP TABLE AdditionalGroups
GO

CREATE TABLE AdditionalGroups
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	UserID INT NOT NULL,
	GroupName NVARCHAR(255)
)
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetAdditionalGroups')
DROP PROCEDURE GetAdditionalGroups
GO

CREATE PROCEDURE [dbo].[GetAdditionalGroups]
(
	@UserID INT
)
AS

SELECT
	AG.ID,
	AG.UserID,
	AG.GroupName
FROM AdditionalGroups AS AG
WHERE AG.UserID = @UserID
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddAdditionalGroup')
DROP PROCEDURE AddAdditionalGroup
GO

CREATE PROCEDURE [dbo].[AddAdditionalGroup]
(
	@GroupID INT OUTPUT,
	@UserID INT,
	@GroupName NVARCHAR(255)
)
AS

INSERT INTO AdditionalGroups
(
	UserID,
	GroupName
)
VALUES
(
	@UserID,
	@GroupName
)

SET @GroupID = SCOPE_IDENTITY()

RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteAdditionalGroup')
DROP PROCEDURE DeleteAdditionalGroup
GO

CREATE PROCEDURE [dbo].[DeleteAdditionalGroup]
(
	@GroupID INT
)
AS

DELETE FROM AdditionalGroups
WHERE ID = @GroupID
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateAdditionalGroup')
DROP PROCEDURE UpdateAdditionalGroup
GO

CREATE PROCEDURE [dbo].[UpdateAdditionalGroup]
(
	@GroupID INT,
	@GroupName NVARCHAR(255)
)
AS

UPDATE AdditionalGroups SET
	GroupName = @GroupName
WHERE ID = @GroupID
GO



-- Remote Desktop Services

-- RDS ResourceGroup

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'RDS')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (45, N'RDS', 26, NULL, 1)
END
GO

-- RDS Quotas

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'RDS.Users')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (450, 45, 1, N'RDS.Users',N'Remote Desktop Users',2, 0 , NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'RDS.Servers')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (451, 45, 2, N'RDS.Servers',N'Remote Desktop Servers',2, 0 , NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'RDS.Collections')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (491, 45, 2, N'RDS.Collections',N'Remote Desktop Servers',2, 0 , NULL)
END
GO

-- RDS Provider

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Remote Desktop Services Windows 2012')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) 
VALUES(1501, 45, N'RemoteDesktopServices2012', N'Remote Desktop Services Windows 2012', N'SolidCP.Providers.RemoteDesktopServices.Windows2012,SolidCP.Providers.RemoteDesktopServices.Windows2012', N'RDS',	1)
END
GO

-- Added phone numbers in the hosted organization.

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='PackageIPAddresses' AND COLS.name='OrgID')
BEGIN
ALTER TABLE [dbo].[PackageIPAddresses] ADD
	[OrgID] [int] NULL
END
GO

ALTER PROCEDURE [dbo].[AllocatePackageIPAddresses]
(
	@PackageID int,
	@OrgID int,
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
		OrgID,
		AddressID	
	)
	SELECT		
		@PackageID,
		@OrgID,
		AddressID

	FROM OPENXML(@idoc, '/items/item', 1) WITH 
	(
		AddressID int '@id'
	) as PV

	-- remove document
	exec sp_xml_removedocument @idoc

END
GO

-- DNS.2013

IF NOT EXISTS ( SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 410 )
BEGIN
	INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES
	(410, 7, N'MSDNS.2012', N'Microsoft DNS Server 2012+', N'SolidCP.Providers.DNS.MsDNS2012, SolidCP.Providers.DNS.MsDNS2012', N'MSDNS', NULL)
END
GO

-- MS DNS.2016

IF NOT EXISTS ( SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 1902 )
BEGIN
	INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES
	(1902, 7, N'MSDNS.2016', N'Microsoft DNS Server 2016', N'SolidCP.Providers.DNS.MsDNS2016, SolidCP.Providers.DNS.MsDNS2016', N'MSDNS', NULL)
END
GO

-- CRM Provider fix

UPDATE Providers SET EditorControl = 'CRM2011' Where ProviderID = 1201;

-- CRM Quota

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM.MaxDatabaseSize')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (460, 21, 4, N'HostedCRM.MaxDatabaseSize', N'Max Database Size, MB',3, 0 , NULL)
END
GO

BEGIN
UPDATE [dbo].[Quotas] SET QuotaDescription = 'Full licenses per organization'  WHERE [QuotaName] = 'HostedCRM.Users'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM.LimitedUsers')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (461, 21, 3, N'HostedCRM.LimitedUsers', N'Limited licenses per organization',3, 0 , NULL)
END
GO

-- CRM Users

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='CRMUsers' AND COLS.name='CALType')
BEGIN
ALTER TABLE [dbo].[CRMUsers] ADD
	[CALType] [int] NULL
END
GO

BEGIN
UPDATE [dbo].[CRMUsers]
   SET 
      CALType = 0 WHERE CALType IS NULL 
END
GO


ALTER PROCEDURE [dbo].[InsertCRMUser] 
(
	@ItemID int,
	@CrmUserID uniqueidentifier,
	@BusinessUnitID uniqueidentifier,
	@CALType int
)
AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO
	CRMUsers
(
	AccountID,
	CRMUserGuid,
	BusinessUnitID,
	CALType
)
VALUES 
(
	@ItemID, 
	@CrmUserID,
	@BusinessUnitID,
	@CALType
)
    
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateCRMUser')
DROP PROCEDURE UpdateCRMUser
GO

CREATE PROCEDURE [dbo].[UpdateCRMUser]
(
	@ItemID int,
	@CALType int
)
AS
BEGIN
	SET NOCOUNT ON;


UPDATE [dbo].[CRMUsers]
   SET 
      CALType = @CALType
 WHERE AccountID = @ItemID

    
END
GO

ALTER PROCEDURE [dbo].[GetCRMUsersCount] 
(
	@ItemID int,
	@Name nvarchar(400),
	@Email nvarchar(400),
	@CALType int
)
AS
BEGIN

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
	AND ((cu.CALType = @CALType) OR (@CALType = -1))
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 5.6')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(302, 11, N'MySQL', N'MySQL Server 5.6', N'SolidCP.Providers.Database.MySqlServer56, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'MySQL Server 5.6'
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 5.7')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(303, 11, N'MySQL', N'MySQL Server 5.7', N'SolidCP.Providers.Database.MySqlServer57, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'MySQL Server 5.7'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Windows Server 2016')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(111, 1, N'Windows2016', N'Windows Server 2016', N'SolidCP.Providers.OS.Windows2016, SolidCP.Providers.OS.Windows2016', N'Windows2016', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Windows Server 2016'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Internet Information Services 10.0')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(112, 2, N'IIS100', N'Internet Information Services 10.0', N'SolidCP.Providers.Web.IIs100, SolidCP.Providers.Web.IIs100', N'IIS70', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Internet Information Services 10.0'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Microsoft FTP Server 10.0')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(113, 3, N'MSFTP100', N'Microsoft FTP Server 10.0', N'SolidCP.Providers.FTP.MsFTP100, SolidCP.Providers.FTP.IIs100', N'MSFTP70', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Microsoft FTP Server 10.0'
END
GO


-- CRM Quota

BEGIN
UPDATE [dbo].[Quotas] SET QuotaOrder = 5  WHERE [QuotaName] = 'HostedCRM.MaxDatabaseSize'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM.ESSUsers')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (462, 21, 4, N'HostedCRM.ESSUsers', N'ESS licenses per organization',3, 0 , NULL)
END
GO

-- UpdateService         //TODO: Add the ability to transfer to another node (ServerID)
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateServiceFully')
DROP PROCEDURE UpdateServiceFully
GO

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
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPendingSSLForWebsite')
DROP PROCEDURE GetPendingSSLForWebsite
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
	@websiteid = [SiteID] AND [Installed] = 0 AND [IsRenewal] = 0 -- bugfix Simon Egli, 27.6.2024

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Lync

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackageIPAddressesCount')
DROP PROCEDURE GetPackageIPAddressesCount
GO

CREATE PROCEDURE [dbo].[GetPackageIPAddressesCount]
(
	@PackageID int,
	@OrgID int,
	@PoolID int = 0
)
AS
BEGIN

SELECT 
	COUNT(PA.PackageAddressID)
FROM 
	dbo.PackageIPAddresses PA
INNER JOIN 
	dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
INNER JOIN 
	dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN 
	dbo.Users U ON U.UserID = P.UserID
LEFT JOIN 
	ServiceItems SI ON PA.ItemId = SI.ItemID
WHERE
	(@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
AND (@OrgID = 0 OR @OrgID <> 0 AND PA.OrgID = @OrgID)

END
GO

-- Enterprise Storage Quotas
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'EnterpriseStorage.DiskStorageSpace')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (430, 44, 1,N'EnterpriseStorage.DiskStorageSpace',N'Disk Storage Space (Gb)',2, 0 , NULL)
END
GO

UPDATE [dbo].[Quotas] SET [QuotaDescription] = N'Disk Storage Space (Gb)' WHERE [QuotaName] = 'EnterpriseStorage.DiskStorageSpace'
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'EnterpriseFolders')

CREATE TABLE [dbo].[EnterpriseFolders](
        [EnterpriseFolderID] [int] IDENTITY(1,1) NOT NULL,
        [ItemID] [int] NOT NULL,
        [FolderName] [nvarchar](255) NOT NULL,
        [FolderQuota] [int] NOT NULL DEFAULT 0,
 CONSTRAINT [PK_EnterpriseFolders] PRIMARY KEY CLUSTERED
(
        [EnterpriseFolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteEnterpriseFolder')
DROP PROCEDURE DeleteEnterpriseFolder
GO

CREATE PROCEDURE [dbo].[DeleteEnterpriseFolder]
(
	@ItemID INT,
	@FolderName NVARCHAR(255)
)
AS

DELETE FROM EnterpriseFolders
WHERE ItemID = @ItemID AND FolderName = @FolderName
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateEnterpriseFolder')
DROP PROCEDURE UpdateEnterpriseFolder
GO

CREATE PROCEDURE [dbo].[UpdateEnterpriseFolder]
(
	@ItemID INT,
	@FolderID NVARCHAR(255),
	@FolderName NVARCHAR(255),
	@FolderQuota INT
)
AS

UPDATE EnterpriseFolders SET
	FolderName = @FolderName,
	FolderQuota = @FolderQuota
WHERE ItemID = @ItemID AND FolderName = @FolderID
GO

-- Enterprise Storage Quotas
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'EnterpriseStorage.DiskStorageSpace')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (430, 44, 1,N'EnterpriseStorage.DiskStorageSpace',N'Disk Storage Space (Mb)',2, 0 , NULL)
END
GO

UPDATE [dbo].[Quotas] SET [QuotaDescription] = N'Disk Storage Space (Mb)' WHERE [QuotaName] = 'EnterpriseStorage.DiskStorageSpace'
GO

--Enterprise Storage
IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='EnterpriseFolders' AND COLS.name='LocationDrive')
BEGIN
ALTER TABLE [dbo].[EnterpriseFolders] ADD
	[LocationDrive] NVARCHAR(255) NULL
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='EnterpriseFolders' AND COLS.name='HomeFolder')
BEGIN
ALTER TABLE [dbo].[EnterpriseFolders] ADD
	[HomeFolder] NVARCHAR(255) NULL
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='EnterpriseFolders' AND COLS.name='Domain')
BEGIN
ALTER TABLE [dbo].[EnterpriseFolders] ADD
	[Domain] NVARCHAR(255) NULL
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetEnterpriseFolders')
DROP PROCEDURE GetEnterpriseFolders
GO

CREATE PROCEDURE [dbo].[GetEnterpriseFolders]
(
	@ItemID INT
)
AS

SELECT DISTINCT LocationDrive, HomeFolder, Domain FROM EnterpriseFolders
WHERE ItemID = @ItemID
GO

DECLARE @serviceId int
SET @serviceId = (SELECT TOP(1) ServiceId FROM Services WHERE ProviderID = 600)

DECLARE @locationDrive nvarchar(255)
SET @locationDrive = (SELECT TOP(1) PropertyValue FROM ServiceProperties WHERE PropertyName = 'locationdrive' AND ServiceID = @serviceId)
DECLARE @homeFolder nvarchar(255)
SET @homeFolder = (SELECT TOP(1) PropertyValue FROM ServiceProperties WHERE PropertyName = 'usershome' AND ServiceID = @serviceId)
DECLARE @domain nvarchar(255)
SET @domain = (SELECT TOP(1) PropertyValue FROM ServiceProperties WHERE PropertyName = 'usersdomain' AND ServiceID = @serviceId)

UPDATE EnterpriseFolders SET
	LocationDrive = @locationDrive,
	HomeFolder = @homeFolder,
	Domain = @domain
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationGroupsByDisplayName')
DROP PROCEDURE [dbo].[GetOrganizationGroupsByDisplayName]
GO

CREATE PROCEDURE [dbo].[GetOrganizationGroupsByDisplayName]
(
	@ItemID int,
	@DisplayName NVARCHAR(255)
)
AS
SELECT
	AccountID,
	ItemID,
	AccountType,
	AccountName,
	DisplayName,
	UserPrincipalName
FROM
	ExchangeAccounts
WHERE
	ItemID = @ItemID AND DisplayName = @DisplayName AND (AccountType IN (8, 9))
RETURN
GO

-- Security Groups Quota update

IF EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedSolution.SecurityGroupManagement' AND [QuotaID] = 423)
BEGIN
	UPDATE [dbo].[Quotas] 
	SET [QuotaDescription] = N'Security Groups', 
		[QuotaName] = N'HostedSolution.SecurityGroups',
		[QuotaTypeID] = 2
	WHERE [QuotaID] = 423

	UPDATE [dbo].[HostingPlanQuotas] 
	SET [QuotaValue] = -1
	WHERE [QuotaID] = 423

	UPDATE [dbo].[PackageQuotas] 
	SET [QuotaValue] = -1
	WHERE [QuotaID] = 423
END
ELSE
	BEGIN
	--add Security Groups Quota
	IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedSolution.SecurityGroups')
		INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (423, 13, 5, N'HostedSolution.SecurityGroups', N'Security Groups', 2, 0, NULL, NULL) 
	END
GO

-- CRM2013

-- CRM2013 ResourceGroup

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'Hosted CRM2013')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (24, N'Hosted CRM2013', 15, NULL, 1)
END
GO

-- CRM2013 Provider

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted MS CRM 2013')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) 
VALUES(1202, 24, N'CRM', N'Hosted MS CRM 2013', N'SolidCP.Providers.HostedSolution.CRMProvider2013, SolidCP.Providers.HostedSolution.Crm2013', N'CRM2011', NULL)
END
GO

-- CRM2013 Quotas

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM2013.Organization')
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) 
VALUES (463, 24, 1, N'HostedCRM2013.Organization', N'CRM Organization', 1, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM2013.MaxDatabaseSize')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) 
VALUES (464, 24, 5, N'HostedCRM2013.MaxDatabaseSize', N'Max Database Size, MB',3, 0 , NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM2013.EssentialUsers')
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) 
VALUES (465, 24, 2, N'HostedCRM2013.EssentialUsers', N'Essential licenses per organization', 3, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM2013.BasicUsers')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) 
VALUES (466, 24, 3, N'HostedCRM2013.BasicUsers', N'Basic licenses per organization',3, 0 , NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedCRM2013.ProfessionalUsers')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) 
VALUES (467, 24, 4, N'HostedCRM2013.ProfessionalUsers', N'Professional licenses per organization',3, 0 , NULL)
END
GO

-- Exchange2013 Archiving

-- Exchange2013 Archiving Quotas

IF EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = 424)
BEGIN
UPDATE [dbo].[Quotas] SET [QuotaName]=N'Exchange2013.AllowRetentionPolicy', [QuotaDescription]=N'Allow Retention Policy'
WHERE [QuotaID] = 424
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.AllowRetentionPolicy')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota])
VALUES (424, 12, 27,N'Exchange2013.AllowRetentionPolicy',N'Allow Retention Policy',1, 0 , NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.ArchivingStorage')
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) 
VALUES (425, 12, 29, N'Exchange2013.ArchivingStorage', N'Archiving storage, MB', 2, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.ArchivingMailboxes')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) 
VALUES (426, 12, 28, N'Exchange2013.ArchivingMailboxes', N'Archiving Mailboxes per Organization', 2, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.AllowArchiving')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota])
VALUES (427, 12, 27,N'Exchange2013.AllowArchiving',N'Allow Archiving',1, 0 , NULL, NULL)
END
GO

-- Exchange2013 Archiving Plans
IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='Archiving')
BEGIN
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD
[Archiving] [bit] NULL
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='EnableArchiving')
BEGIN
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD
[EnableArchiving] [bit] NULL
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='ArchiveSizeMB')
BEGIN
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD
[ArchiveSizeMB] [int] NULL,
[ArchiveWarningPct] [int] NULL
END
GO

-- Exchange2013 Auto Reply

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='EnableAutoReply')
BEGIN
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD
[EnableAutoReply] [bit] NULL
END
GO

-- Exchange2013 Journaling
IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='IsForJournaling')
BEGIN
ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD
[IsForJournaling] [bit] NULL
END
GO

-- Exchange2013 ExchangeAccount

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeAccounts' AND COLS.name='ArchivingMailboxPlanId')
BEGIN
ALTER TABLE [dbo].[ExchangeAccounts] ADD
[ArchivingMailboxPlanId] [int] NULL
END
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeAccounts' AND COLS.name='EnableArchiving')
BEGIN
ALTER TABLE [dbo].[ExchangeAccounts] ADD
[EnableArchiving] [bit] NULL
END
GO

-- Password column removed
ALTER PROCEDURE [dbo].[GetExchangeAccountByAccountName] 
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
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName,
	E.ArchivingMailboxPlanId, 
	AP.MailboxPlan as 'ArchivingMailboxPlan',
	E.EnableArchiving
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId	
LEFT OUTER JOIN ExchangeMailboxPlans AS AP ON E.ArchivingMailboxPlanId = AP.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.AccountName = @AccountName
RETURN
GO

ALTER PROCEDURE [dbo].[SetExchangeAccountMailboxplan] 
(
	@AccountID int,
	@MailboxPlanId int,
	@ArchivingMailboxPlanId int,
	@EnableArchiving bit
)
AS

UPDATE ExchangeAccounts SET
	MailboxPlanId = @MailboxPlanId,
	ArchivingMailboxPlanId = @ArchivingMailboxPlanId,
	EnableArchiving = @EnableArchiving
WHERE
	AccountID = @AccountID

RETURN

GO

-- Password column removed
ALTER PROCEDURE [dbo].[UpdateExchangeAccount] 
(
	@AccountID int,
	@AccountName nvarchar(300),
	@DisplayName nvarchar(300),
	@PrimaryEmailAddress nvarchar(300),
	@AccountType int,
	@SamAccountName nvarchar(100),
	@MailEnabledPublicFolder bit,
	@MailboxManagerActions varchar(200),
	@MailboxPlanId int,
	@ArchivingMailboxPlanId int,
	@SubscriberNumber varchar(32),
	@EnableArchiving bit
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
	SubscriberNumber = @SubscriberNumber,
	ArchivingMailboxPlanId = @ArchivingMailboxPlanId,
	EnableArchiving = @EnableArchiving

WHERE
	AccountID = @AccountID

IF (@@ERROR <> 0 )
	BEGIN
		ROLLBACK TRANSACTION
		RETURN -1
	END

COMMIT TRAN
RETURN

GO

-- Exchange2013 Archiving ExchangeRetentionPolicyTags

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ExchangeRetentionPolicyTags')
CREATE TABLE ExchangeRetentionPolicyTags
(
[TagID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
[ItemID] [int] NOT NULL,
[TagName] NVARCHAR(255),
[TagType] [int] NOT NULL,
[AgeLimitForRetention] [int] NOT NULL,
[RetentionAction] [int] NOT NULL
)
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddExchangeRetentionPolicyTag')
DROP PROCEDURE [dbo].[AddExchangeRetentionPolicyTag]
GO

CREATE PROCEDURE [dbo].[AddExchangeRetentionPolicyTag] 
(
	@TagID int OUTPUT,
	@ItemID int,
	@TagName nvarchar(255),
	@TagType int,
	@AgeLimitForRetention int,
	@RetentionAction int
)
AS
BEGIN

INSERT INTO ExchangeRetentionPolicyTags
(
	ItemID,
	TagName,
	TagType,
	AgeLimitForRetention,
	RetentionAction
)
VALUES
(
	@ItemID,
	@TagName,
	@TagType,
	@AgeLimitForRetention,
	@RetentionAction
)

SET @TagID = SCOPE_IDENTITY()

RETURN

END
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateExchangeRetentionPolicyTag')
DROP PROCEDURE [dbo].[UpdateExchangeRetentionPolicyTag]
GO

CREATE PROCEDURE [dbo].[UpdateExchangeRetentionPolicyTag] 
(
	@TagID int,
	@ItemID int,
	@TagName nvarchar(255),
	@TagType int,
	@AgeLimitForRetention int,
	@RetentionAction int
)
AS

UPDATE ExchangeRetentionPolicyTags SET
	ItemID = @ItemID,
	TagName = @TagName,
	TagType = @TagType,
	AgeLimitForRetention = @AgeLimitForRetention,
	RetentionAction = @RetentionAction
WHERE TagID = @TagID

RETURN
	
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetExchangeRetentionPolicyTags')
DROP PROCEDURE [dbo].[GetExchangeRetentionPolicyTags]
GO

CREATE PROCEDURE [dbo].[GetExchangeRetentionPolicyTags]
(
	@ItemID int
)
AS
SELECT
	TagID,
	ItemID,
	TagName,
	TagType,
	AgeLimitForRetention,
	RetentionAction
FROM
	ExchangeRetentionPolicyTags
WHERE
	ItemID = @ItemID 
ORDER BY TagName
RETURN

GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetExchangeRetentionPolicyTag')
DROP PROCEDURE [dbo].[GetExchangeRetentionPolicyTag]
GO

CREATE PROCEDURE [dbo].[GetExchangeRetentionPolicyTag] 
(
	@TagID int
)
AS
SELECT
	TagID,
	ItemID,
	TagName,
	TagType,
	AgeLimitForRetention,
	RetentionAction
FROM
	ExchangeRetentionPolicyTags
WHERE
	TagID = @TagID
RETURN

GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteExchangeRetentionPolicyTag')
DROP PROCEDURE [dbo].[DeleteExchangeRetentionPolicyTag]
GO


CREATE PROCEDURE [dbo].[DeleteExchangeRetentionPolicyTag]
(
        @TagID int
)
AS
DELETE FROM ExchangeRetentionPolicyTags
WHERE
	TagID = @TagID
RETURN

GO


-- Exchange2013 Archiving ExchangeMailboxPlanRetentionPolicyTags

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ExchangeMailboxPlanRetentionPolicyTags')
CREATE TABLE ExchangeMailboxPlanRetentionPolicyTags
(
[PlanTagID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
[TagID] [int] NOT NULL,
[MailboxPlanId] [int] NOT NULL
)
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetExchangeMailboxPlanRetentionPolicyTags')
DROP PROCEDURE [dbo].[GetExchangeMailboxPlanRetentionPolicyTags]
GO

CREATE PROCEDURE [dbo].[GetExchangeMailboxPlanRetentionPolicyTags]
(
	@MailboxPlanId int
)
AS
SELECT
D.PlanTagID,
D.TagID,
D.MailboxPlanId,
P.MailboxPlan,
T.TagName
FROM
	ExchangeMailboxPlanRetentionPolicyTags AS D
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON P.MailboxPlanId = D.MailboxPlanId	
LEFT OUTER JOIN ExchangeRetentionPolicyTags AS T ON T.TagID = D.TagID	
WHERE
	D.MailboxPlanId = @MailboxPlanId 
RETURN

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddExchangeMailboxPlanRetentionPolicyTag')
DROP PROCEDURE [dbo].[AddExchangeMailboxPlanRetentionPolicyTag]
GO

CREATE PROCEDURE [dbo].[AddExchangeMailboxPlanRetentionPolicyTag] 
(
	@PlanTagID int OUTPUT,
	@TagID int,
	@MailboxPlanId int
)
AS
BEGIN

INSERT INTO ExchangeMailboxPlanRetentionPolicyTags
(
	TagID,
	MailboxPlanId
)
VALUES
(
	@TagID,
	@MailboxPlanId
)

SET @PlanTagID = SCOPE_IDENTITY()

RETURN

END
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteExchangeMailboxPlanRetentionPolicyTag')
DROP PROCEDURE [dbo].[DeleteExchangeMailboxPlanRetentionPolicyTag]
GO

CREATE PROCEDURE [dbo].[DeleteExchangeMailboxPlanRetentionPolicyTag]
(
        @PlanTagID int
)
AS
DELETE FROM ExchangeMailboxPlanRetentionPolicyTags
WHERE
	PlanTagID = @PlanTagID
RETURN

GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'EnterpriseStorage.DriveMaps')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (468, 44, 2, N'EnterpriseStorage.DriveMaps', N'Use Drive Maps', 1, 0, NULL)
END
GO

-- Exchange2013 Archiving

BEGIN
DELETE FROM [dbo].[HostingPlanQuotas] WHERE QuotaID = 427
END
GO

BEGIN
DELETE FROM [dbo].[PackageQuotas] WHERE QuotaID = 427
END
GO


BEGIN
DELETE FROM [dbo].[Quotas] WHERE QuotaID = 427
END
GO

-- Set SQL 2008 and SQL 2012 Users on suspendable
BEGIN
UPDATE [dbo].[ServiceItemTypes] SET [Suspendable] = 1 WHERE [ItemTypeID] = 32 AND [GroupID] = 22
END
GO

BEGIN
UPDATE [dbo].[ServiceItemTypes] SET [Suspendable] = 1 WHERE [ItemTypeID] = 38 AND [GroupID] = 23
END
GO

/* ICE Warp */ 
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'IceWarp')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (160, 4, N'IceWarp', N'IceWarp Mail Server', N'SolidCP.Providers.Mail.IceWarp, SolidCP.Providers.Mail.IceWarp', N'IceWarp', NULL)
END
GO
 
/* SQL 2014 Provider */
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'MsSQL2014')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (46, N'MsSQL2014', 10, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
ELSE
BEGIN
UPDATE [dbo].[ResourceGroups] SET [ShowGroup] = 1 WHERE [GroupName] = 'MsSQL2014'
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Microsoft SQL Server 2014')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1203, 46, N'MsSQL', N'Microsoft SQL Server 2014', N'SolidCP.Providers.Database.MsSqlServer2014, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (39, 46, N'MsSQL2014Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (40, 46, N'MsSQL2014User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (470, 46, 1, N'MsSQL2014.Databases', N'Databases', 2, 0, 39, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (471, 46, 2, N'MsSQL2014.Users', N'Users', 2, 0, 40, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (472, 46, 3, N'MsSQL2014.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (473, 46, 5, N'MsSQL2014.Backup', N'Database Backups', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (474, 46, 6, N'MsSQL2014.Restore', N'Database Restores', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (475, 46, 7, N'MsSQL2014.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (476, 46, 4, N'MsSQL2014.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 46 WHERE [DisplayName] = 'Microsoft SQL Server 2014'
END
GO

/* SQL 2016 Provider */
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'MsSQL2016')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (71, N'MsSQL2016', 10, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
ELSE
BEGIN
UPDATE [dbo].[ResourceGroups] SET [ShowGroup] = 1 WHERE [GroupName] = 'MsSQL2016'
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Microsoft SQL Server 2016')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1701, 71, N'MsSQL', N'Microsoft SQL Server 2016', N'SolidCP.Providers.Database.MsSqlServer2016, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (71, 71, N'MsSQL2016Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (72, 71, N'MsSQL2016User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (701, 71, 1, N'MsSQL2016.Databases', N'Databases', 2, 0, 39, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (702, 71, 2, N'MsSQL2016.Users', N'Users', 2, 0, 40, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (703, 71, 3, N'MsSQL2016.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (704, 71, 5, N'MsSQL2016.Backup', N'Database Backups', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (705, 71, 6, N'MsSQL2016.Restore', N'Database Restores', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (706, 71, 7, N'MsSQL2016.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (707, 71, 4, N'MsSQL2016.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 71 WHERE [DisplayName] = 'Microsoft SQL Server 2016'
END
GO

/* MariaDB */
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupID] = '50')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (50, N'MariaDB', 11, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
ELSE
BEGIN
UPDATE [dbo].[ResourceGroups] SET [ShowGroup] = 1 WHERE [GroupName] = 'MariaDB'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1550')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1550, 50, N'MariaDB', N'MariaDB 10.1', N'SolidCP.Providers.Database.MariaDB101, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 50 WHERE [ProviderID] = '1550'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [SettingsName] = 'MariaDBPolicy')
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MariaDBPolicy', N'DatabaseNamePolicy', N'True;;1;40;;;')
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MariaDBPolicy', N'UserNamePolicy', N'True;;1;16;;;')
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MariaDBPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;False')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceItemTypes] WHERE [DisplayName] = 'MariaDBDatabase')
BEGIN
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (202, 50, N'MariaDBDatabase', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceItemTypes] WHERE [DisplayName] = 'MariaDBUser')
BEGIN
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (203, 50, N'MariaDBUser', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1550')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1550, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1550, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.1')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1550, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1550, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1550, N'RootPassword', N'')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '573')
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (573, 50, 1, N'MariaDB.Databases', N'Databases', 2, 0, 202, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (574, 50, 2, N'MariaDB.Users', N'Users', 2, 0, 203, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (575, 50, 3, N'MariaDB.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (576, 50, 5, N'MariaDB.Backup', N'Database Backups', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (577, 50, 6, N'MariaDB.Restore', N'Database Restores', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (578, 50, 7, N'MariaDB.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (579, 50, 4, N'MariaDB.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Quotas] SET [ItemTypeID] = '202' WHERE [QuotaID] = '573'
UPDATE [dbo].[Quotas] SET [ItemTypeID] = '203' WHERE [QuotaID] = '574'
END
GO

/* Modify column size to store more data*/
ALTER TABLE [dbo].[ScheduleTaskParameters] ALTER COLUMN [DefaultValue] nvarchar(max) NULL;
GO

/*This should be [DefaultValue]= N'MsSQL2000=SQL Server 2000;MsSQL2005=SQL Server 2005;MsSQL2008=SQL Server 2008;MsSQL2012=SQL Server 2012;MsSQL2014=SQL Server 2014;MySQL4=MySQL 4.0;MySQL5=MySQL 5.0' but the field is not large enough!! */
UPDATE [dbo].[ScheduleTaskParameters] SET [DefaultValue]= N'MsSQL2014=SQL Server 2014;MsSQL2016=SQL Server 2016;MsSQL2017=SQL Server 2017;MsSQL2019=SQL Server 2019;MsSQL2022=SQL Server 2022;MySQL5=MySQL 5.0;MariaDB=MariaDB' WHERE [TaskID]= 'SCHEDULE_TASK_BACKUP_DATABASE' AND [ParameterID]='DATABASE_GROUP'
GO

/*SUPPORT SERVICE LEVELS*/

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'Service Levels')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (47, N'Service Levels', 2, NULL, 1)
END
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'SupportServiceLevels')
CREATE TABLE SupportServiceLevels
(
[LevelID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
[LevelName] NVARCHAR(100) NOT NULL,
[LevelDescription] NVARCHAR(1000) NULL
)
GO

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeAccounts' AND COLS.name='LevelID')
ALTER TABLE [dbo].[ExchangeAccounts] ADD
	[LevelID] [int] NULL,
	[IsVIP] [bit] NOT NULL DEFAULT 0
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetSupportServiceLevels')
DROP PROCEDURE GetSupportServiceLevels
GO

CREATE PROCEDURE [dbo].[GetSupportServiceLevels]
AS
SELECT *
FROM SupportServiceLevels
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetSupportServiceLevel')
DROP PROCEDURE GetSupportServiceLevel
GO

CREATE PROCEDURE [dbo].[GetSupportServiceLevel]
(
	@LevelID int
)
AS
SELECT *
FROM SupportServiceLevels
WHERE LevelID = @LevelID
RETURN 
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddSupportServiceLevel')
DROP PROCEDURE AddSupportServiceLevel
GO

CREATE PROCEDURE [dbo].[AddSupportServiceLevel]
(
	@LevelID int OUTPUT,
	@LevelName nvarchar(100),
	@LevelDescription nvarchar(1000)
)
AS
BEGIN

	IF EXISTS (SELECT * FROM SupportServiceLevels WHERE LevelName = @LevelName)
	BEGIN
		SET @LevelID = -1

		RETURN
	END

	INSERT INTO SupportServiceLevels
	(
		LevelName,
		LevelDescription
	)
	VALUES
	(
		@LevelName,
		@LevelDescription
	)

	SET @LevelID = SCOPE_IDENTITY()

	DECLARE @ResourseGroupID int

	IF EXISTS (SELECT * FROM ResourceGroups WHERE GroupName = 'Service Levels')
	BEGIN
		DECLARE @QuotaLastID int, @CurQuotaName nvarchar(100), 
			@CurQuotaDescription nvarchar(1000), @QuotaOrderInGroup int

		SET @CurQuotaName = N'ServiceLevel.' + @LevelName
		SET @CurQuotaDescription = @LevelName + N', users'

		SELECT @ResourseGroupID = GroupID FROM ResourceGroups WHERE GroupName = 'Service Levels'

		SELECT @QuotaLastID = MAX(QuotaID) FROM Quotas

		SELECT @QuotaOrderInGroup = MAX(QuotaOrder) FROM Quotas WHERE GroupID = @ResourseGroupID

		IF @QuotaOrderInGroup IS NULL SET @QuotaOrderInGroup = 0

		IF NOT EXISTS (SELECT * FROM Quotas WHERE QuotaName = @CurQuotaName)
		BEGIN
			INSERT Quotas 
				(QuotaID, 
				GroupID, 
				QuotaOrder, 
				QuotaName, 
				QuotaDescription, 
				QuotaTypeID, 
				ServiceQuota, 
				ItemTypeID) 
			VALUES 
				(@QuotaLastID + 1, 
				@ResourseGroupID, 
				@QuotaOrderInGroup + 1, 
				@CurQuotaName, 
				@CurQuotaDescription,
				2, 
				0, 
				NULL)
		END
	END

END

RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteSupportServiceLevel')
DROP PROCEDURE DeleteSupportServiceLevel
GO

CREATE PROCEDURE [dbo].[DeleteSupportServiceLevel]
(
	@LevelID int
)
AS
BEGIN

	DECLARE @LevelName nvarchar(100), @QuotaName nvarchar(100), @QuotaID int

	SELECT @LevelName = LevelName FROM SupportServiceLevels WHERE LevelID = @LevelID

	SET @QuotaName = N'ServiceLevel.' + @LevelName

	SELECT @QuotaID = QuotaID FROM Quotas WHERE QuotaName = @QuotaName

	IF @QuotaID IS NOT NULL
	BEGIN
		DELETE FROM HostingPlanQuotas WHERE QuotaID = @QuotaID
		DELETE FROM PackageQuotas WHERE QuotaID = @QuotaID
		DELETE FROM Quotas WHERE QuotaID = @QuotaID
	END

	IF EXISTS (SELECT * FROM ExchangeAccounts WHERE LevelID = @LevelID)
	UPDATE ExchangeAccounts
	   SET LevelID = NULL
	 WHERE LevelID = @LevelID

	DELETE FROM SupportServiceLevels WHERE LevelID = @LevelID

END

RETURN 
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateSupportServiceLevel')
DROP PROCEDURE UpdateSupportServiceLevel
GO

CREATE PROCEDURE [dbo].[UpdateSupportServiceLevel]
(
	@LevelID int,
	@LevelName nvarchar(100),
	@LevelDescription nvarchar(1000)
)
AS
BEGIN

	DECLARE @PrevQuotaName nvarchar(100), @PrevLevelName nvarchar(100)

	SELECT @PrevLevelName = LevelName FROM SupportServiceLevels WHERE LevelID = @LevelID

	SET @PrevQuotaName = N'ServiceLevel.' + @PrevLevelName

	UPDATE SupportServiceLevels
	SET LevelName = @LevelName,
		LevelDescription = @LevelDescription
	WHERE LevelID = @LevelID

	IF EXISTS (SELECT * FROM Quotas WHERE QuotaName = @PrevQuotaName)
	BEGIN
		DECLARE @QuotaID INT

		SELECT @QuotaID = QuotaID FROM Quotas WHERE QuotaName = @PrevQuotaName
		 
		UPDATE Quotas
		SET QuotaName = N'ServiceLevel.' + @LevelName,
			QuotaDescription = @LevelName + ', users'
		WHERE QuotaID = @QuotaID
	END

END

RETURN 
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type IN ('FN', 'IF', 'TF') AND name = 'GetPackageServiceLevelResource') 
DROP FUNCTION GetPackageServiceLevelResource
GO

CREATE FUNCTION dbo.GetPackageServiceLevelResource
(
	@PackageID int,
	@GroupID int,
	@ServerID int
)
RETURNS bit
AS
BEGIN

IF NOT EXISTS (SELECT * FROM dbo.ResourceGroups WHERE GroupID = @GroupID AND GroupName = 'Service Levels')
RETURN 0

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

		IF @ServerID = 0
		RETURN 0
		ELSE IF @PID = -1
		RETURN 1
		ELSE IF @ServerID IS NULL
		RETURN 1
		ELSE IF @ServerID > 0
		RETURN 1
		ELSE RETURN 0
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

ALTER PROCEDURE [dbo].[GetPackageQuotasForEdit]
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
		--dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID) AS Enabled,
	CASE
		WHEN RG.GroupName = 'Service Levels' THEN dbo.GetPackageServiceLevelResource(PackageID, RG.GroupID, @ServerID)
		ELSE dbo.GetPackageAllocatedResource(PackageID, RG.GroupID, @ServerID)
	END AS Enabled,
	--dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, @ServerID) AS ParentEnabled
	CASE
		WHEN RG.GroupName = 'Service Levels' THEN dbo.GetPackageServiceLevelResource(@ParentPackageID, RG.GroupID, @ServerID)
		ELSE dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, @ServerID)
	END AS ParentEnabled
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

ALTER PROCEDURE [dbo].[GetHostingPlanQuotas]
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
	--dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID) AS ParentEnabled,
	CASE
		WHEN RG.GroupName = 'Service Levels' THEN dbo.GetPackageServiceLevelResource(@PackageID, RG.GroupID, @ServerID)
		ELSE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID)
	END AS ParentEnabled,
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

-- Password column removed
ALTER PROCEDURE [dbo].[GetExchangeAccount] 
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
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName,
	E.ArchivingMailboxPlanId, 
	AP.MailboxPlan as 'ArchivingMailboxPlan',
	E.EnableArchiving,
	E.LevelID,
	E.IsVIP
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId	
LEFT OUTER JOIN ExchangeMailboxPlans AS AP ON E.ArchivingMailboxPlanId = AP.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.AccountID = @AccountID
RETURN
GO

ALTER PROCEDURE [dbo].[GetExchangeAccountsPaged]
(
	@ActorID int,
	@ItemID int,
	@AccountTypes nvarchar(30),
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Archiving bit
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
		SET @condition = @condition + ' AND EA.AccountID IN (SELECT EAEA.AccountID FROM ExchangeAccountEmailAddresses EAEA WHERE EAEA.EmailAddress LIKE ''%' + @FilterValue + '%'')'
	END
	ELSE
	BEGIN		
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''%' + @FilterValue + '%'''
	END
END

if @Archiving = 1
BEGIN
	SET @condition = @condition + ' AND (EA.ArchivingMailboxPlanId > 0) ' 
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
		EA.UserPrincipalName,
		EA.LevelID,
		EA.IsVIP ' + @joincondition +
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

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetExchangeAccounts')
DROP PROCEDURE GetExchangeAccounts
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
	(E.AccountType = @AccountType OR @AccountType = 0)
ORDER BY DisplayName
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateExchangeAccountSLSettings')
DROP PROCEDURE UpdateExchangeAccountSLSettings
GO

CREATE PROCEDURE [dbo].[UpdateExchangeAccountSLSettings]
(
	@AccountID int,
	@LevelID int,
	@IsVIP bit
)
AS

BEGIN TRAN	

	IF (@LevelID = -1) 
	BEGIN
		SET @LevelID = NULL
	END

	UPDATE ExchangeAccounts SET
		LevelID = @LevelID,
		IsVIP = @IsVIP
	WHERE
		AccountID = @AccountID

	IF (@@ERROR <> 0 )
		BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END
COMMIT TRAN
RETURN 
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'CheckServiceLevelUsage')
DROP PROCEDURE CheckServiceLevelUsage
GO

CREATE PROCEDURE [dbo].[CheckServiceLevelUsage]
(
	@LevelID int
)
AS
SELECT COUNT(EA.AccountID)
FROM SupportServiceLevels AS SL
INNER JOIN ExchangeAccounts AS EA ON SL.LevelID = EA.LevelID
WHERE EA.LevelID = @LevelID
RETURN 
GO

-- Service Level Quotas, change type 
UPDATE Quotas
SET QuotaTypeID = 2 
WHERE QuotaName like 'ServiceLevel.%'
GO


-- IIS80 Provider update for SNI and CCS support
-- Add default serviceproperties for all existing IIS80 Services (if any). These properties are used as markers in the IIS70 Controls in WebPortal to know the version of the IIS Provider
declare c cursor read_only for 
select ServiceID from Services where ProviderID in(select ProviderID from Providers where ProviderName='IIS80')

declare @ServiceID int

open c

fetch next from c 
into @ServiceID

while @@FETCH_STATUS = 0
begin
	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'sslccscommonpassword')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'sslccscommonpassword', '')

	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'sslccsuncpath')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'sslccsuncpath', '')

	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'ssluseccs')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'ssluseccs', 'False')

	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'sslusesni')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'sslusesni', 'False')

	fetch next from c 
	into @ServiceID
end

close c

deallocate c

GO

-- IIS100 Provider update for SNI and CCS support
-- Add default serviceproperties for all existing IIS100 Services (if any). These properties are used as markers in the IIS70 Controls in WebPortal to know the version of the IIS Provider
declare c cursor read_only for 
select ServiceID from Services where ProviderID in(select ProviderID from Providers where ProviderName='IIS100')

declare @ServiceID int

open c

fetch next from c 
into @ServiceID

while @@FETCH_STATUS = 0
begin
	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'sslccscommonpassword')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'sslccscommonpassword', '')

	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'sslccsuncpath')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'sslccsuncpath', '')

	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'ssluseccs')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'ssluseccs', 'False')

	if not exists(select null from ServiceProperties where ServiceID = @ServiceID and PropertyName = 'sslusesni')
		insert into ServiceProperties(ServiceID, PropertyName, PropertyValue)
		values(@ServiceID, 'sslusesni', 'False')

	fetch next from c 
	into @ServiceID
end

close c

deallocate c

GO

/*Remote Desktop Services*/

/*Remote Desktop Services Tables*/
IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'RDSCollectionUsers')
CREATE TABLE RDSCollectionUsers
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	RDSCollectionId INT NOT NULL, 
	AccountID INT NOT NULL 
)
GO


IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'RDSServers')
CREATE TABLE RDSServers
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ItemID INT,
	Name NVARCHAR(255),
	FqdName NVARCHAR(255),
	Description NVARCHAR(255),
	RDSCollectionId INT,
	ConnectionEnabled BIT NOT NULL DEFAULT(1)
)
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'RDSServers' AND COLUMN_NAME = 'ConnectionEnabled')
BEGIN
	ALTER TABLE [dbo].[RDSServers]
		ADD ConnectionEnabled BIT NOT NULL DEFAULT(1)
END
GO


IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'RDSCollections')
CREATE TABLE RDSCollections
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ItemID INT NOT NULL,
	Name NVARCHAR(255),
	Description NVARCHAR(255)
)
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'RDSCollections' AND COLUMN_NAME = 'DisplayName')
BEGIN
	ALTER TABLE [dbo].[RDSCollections]
		ADD DisplayName NVARCHAR(255)
END
GO

UPDATE [dbo].[RDSCollections] SET DisplayName = [Name]	 WHERE DisplayName IS NULL

IF NOT EXISTS(SELECT * FROM SYS.TABLES WHERE name = 'RDSCollectionSettings')
CREATE TABLE [dbo].[RDSCollectionSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RDSCollectionId] [int] NOT NULL,
	[DisconnectedSessionLimitMin] [int] NULL,
	[ActiveSessionLimitMin] [int] NULL,
	[IdleSessionLimitMin] [int] NULL,
	[BrokenConnectionAction] [nvarchar](20) NULL,
	[AutomaticReconnectionEnabled] [bit] NULL,
	[TemporaryFoldersDeletedOnExit] [bit] NULL,
	[TemporaryFoldersPerSession] [bit] NULL,
	[ClientDeviceRedirectionOptions] [nvarchar](250) NULL,
	[ClientPrinterRedirected] [bit] NULL,
	[ClientPrinterAsDefault] [bit] NULL,
	[RDEasyPrintDriverEnabled] [bit] NULL,
	[MaxRedirectedMonitors] [int] NULL,
 CONSTRAINT [PK_RDSCollectionSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'SecurityLayer' AND [object_id] = OBJECT_ID(N'RDSCollectionSettings'))
BEGIN
	ALTER TABLE [dbo].[RDSCollectionSettings] ADD SecurityLayer NVARCHAR(20) null;
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'EncryptionLevel' AND [object_id] = OBJECT_ID(N'RDSCollectionSettings'))
BEGIN
	ALTER TABLE [dbo].[RDSCollectionSettings] ADD EncryptionLevel NVARCHAR(20) null;
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'AuthenticateUsingNLA' AND [object_id] = OBJECT_ID(N'RDSCollectionSettings'))
BEGIN
	ALTER TABLE [dbo].[RDSCollectionSettings] ADD AuthenticateUsingNLA BIT null;
END
GO



IF NOT EXISTS(SELECT * FROM SYS.TABLES WHERE name = 'RDSCertificates')
CREATE TABLE [dbo].[RDSCertificates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[Content] [ntext] NOT NULL,
	[Hash] [nvarchar](255) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[ValidFrom] [datetime] NULL,
	[ExpiryDate] [datetime] NULL
 CONSTRAINT [PK_RDSCertificates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_RDSCollectionUsers_RDSCollectionId')
BEGIN
	ALTER TABLE [dbo].[RDSCollectionUsers]
	DROP CONSTRAINT [FK_RDSCollectionUsers_RDSCollectionId]
END
ELSE
	PRINT 'FK_RDSCollectionUsers_RDSCollectionId not EXISTS'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_RDSCollectionUsers_UserId')
BEGIN
	ALTER TABLE [dbo].[RDSCollectionUsers]
	DROP CONSTRAINT [FK_RDSCollectionUsers_UserId]
END	
ELSE
	PRINT 'FK_RDSCollectionUsers_UserId not EXISTS'
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_RDSServers_RDSCollectionId')
BEGIN
	ALTER TABLE [dbo].[RDSServers]
	DROP CONSTRAINT [FK_RDSServers_RDSCollectionId]
END	
ELSE
	PRINT 'FK_RDSServers_RDSCollectionId not EXISTS'	
GO

ALTER TABLE [dbo].[RDSCollectionUsers]  WITH CHECK ADD  CONSTRAINT [FK_RDSCollectionUsers_RDSCollectionId] FOREIGN KEY([RDSCollectionId])
REFERENCES [dbo].[RDSCollections] ([ID])
ON DELETE CASCADE
GO


ALTER TABLE [dbo].[RDSCollectionUsers]  WITH CHECK ADD  CONSTRAINT [FK_RDSCollectionUsers_UserId] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RDSServers]  WITH CHECK ADD  CONSTRAINT [FK_RDSServers_RDSCollectionId] FOREIGN KEY([RDSCollectionId])
REFERENCES [dbo].[RDSCollections] ([ID])
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_RDSCollectionSettings_RDSCollections')
ALTER TABLE [dbo].[RDSCollectionSettings]  WITH CHECK ADD  CONSTRAINT [FK_RDSCollectionSettings_RDSCollections] FOREIGN KEY([RDSCollectionId])
REFERENCES [dbo].[RDSCollections] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RDSCollectionSettings] CHECK CONSTRAINT [FK_RDSCollectionSettings_RDSCollections]
GO

/*Remote Desktop Services Procedures*/

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddRDSCertificate')
DROP PROCEDURE AddRDSCertificate
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
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSCertificateByServiceId')
DROP PROCEDURE GetRDSCertificateByServiceId
GO
CREATE PROCEDURE [dbo].[GetRDSCertificateByServiceId]
(
	@ServiceId INT
)
AS
SELECT TOP 1
	Id,
	ServiceId,
	Content, 
	Hash,
	FileName,
	ValidFrom,
	ExpiryDate
	FROM RDSCertificates
	WHERE ServiceId = @ServiceId
	ORDER BY Id DESC
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteRDSServer')
DROP PROCEDURE DeleteRDSServer
GO
CREATE PROCEDURE [dbo].[DeleteRDSServer]
(
	@Id  int
)
AS
DELETE FROM RDSServers
WHERE Id = @Id
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddRDSServerToOrganization')
DROP PROCEDURE AddRDSServerToOrganization
GO
CREATE PROCEDURE [dbo].[AddRDSServerToOrganization]
(
	@Id  INT,
	@ItemID INT
)
AS

UPDATE RDSServers
SET
	ItemID = @ItemID
WHERE ID = @Id
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'RemoveRDSServerFromOrganization')
DROP PROCEDURE RemoveRDSServerFromOrganization
GO
CREATE PROCEDURE [dbo].[RemoveRDSServerFromOrganization]
(
	@Id  INT
)
AS

UPDATE RDSServers
SET
	ItemID = NULL
WHERE ID = @Id
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddRDSServerToCollection')
DROP PROCEDURE AddRDSServerToCollection
GO
CREATE PROCEDURE [dbo].[AddRDSServerToCollection]
(
	@Id  INT,
	@RDSCollectionId INT
)
AS

UPDATE RDSServers
SET
	RDSCollectionId = @RDSCollectionId
WHERE ID = @Id
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'RemoveRDSServerFromCollection')
DROP PROCEDURE RemoveRDSServerFromCollection
GO
CREATE PROCEDURE [dbo].[RemoveRDSServerFromCollection]
(
	@Id  INT
)
AS

UPDATE RDSServers
SET
	RDSCollectionId = NULL
WHERE ID = @Id
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSServersByItemId')
DROP PROCEDURE GetRDSServersByItemId
GO
CREATE PROCEDURE [dbo].[GetRDSServersByItemId]
(
	@ItemID INT
)
AS
SELECT 
	RS.Id,
	RS.ItemID,
	RS.Name, 
	RS.FqdName,
	RS.Description,
	RS.RdsCollectionId,
	SI.ItemName
	FROM RDSServers AS RS
	LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = RS.ItemId
	WHERE RS.ItemID = @ItemID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSServers')
DROP PROCEDURE GetRDSServers
GO
CREATE PROCEDURE [dbo].[GetRDSServers]
AS
SELECT 
	RS.Id,
	RS.ItemID,
	RS.Name, 
	RS.FqdName,
	RS.Description,
	RS.RdsCollectionId,
	SI.ItemName
	FROM RDSServers AS RS
	LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = RS.ItemId
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSServersByCollectionId')
DROP PROCEDURE GetRDSServersByCollectionId
GO
CREATE PROCEDURE [dbo].[GetRDSServersByCollectionId]
(
	@RdsCollectionId INT
)
AS
SELECT 
	RS.Id,
	RS.ItemID,
	RS.Name, 
	RS.FqdName,
	RS.Description,
	RS.RdsCollectionId,
	SI.ItemName
	FROM RDSServers AS RS
	LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = RS.ItemId
	WHERE RdsCollectionId = @RdsCollectionId
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSCollectionsPaged')
DROP PROCEDURE GetRDSCollectionsPaged
GO
CREATE PROCEDURE [dbo].[GetRDSCollectionsPaged]
(
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@ItemID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @RDSCollections TABLE
(
	ItemPosition int IDENTITY(0,1),
	RDSCollectionId int
)
INSERT INTO @RDSCollections (RDSCollectionId)
SELECT
	S.ID
FROM RDSCollections AS S
WHERE 
	((@ItemID is Null AND S.ItemID is null)
		or (@ItemID is not Null AND S.ItemID = @ItemID))'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE ''%' + @FilterValue + '%'' '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(RDSCollectionId) FROM @RDSCollections;
SELECT
	CR.ID,
	CR.ItemID,
	CR.Name,
	CR.Description,
	CR.DisplayName
FROM @RDSCollections AS C
INNER JOIN RDSCollections AS CR ON C.RDSCollectionId = CR.ID
WHERE C.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int,  @FilterValue nvarchar(50),  @ItemID int',
@StartRow, @MaximumRows,  @FilterValue,  @ItemID


RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSCollectionsByItemId')
DROP PROCEDURE GetRDSCollectionsByItemId
GO
CREATE PROCEDURE [dbo].[GetRDSCollectionsByItemId]
(
	@ItemID INT
)
AS
SELECT 
	Id,
	ItemId,
	Name, 
	Description,
	DisplayName
	FROM RDSCollections
	WHERE ItemID = @ItemID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSCollectionByName')
DROP PROCEDURE GetRDSCollectionByName
GO
CREATE PROCEDURE [dbo].[GetRDSCollectionByName]
(
	@Name NVARCHAR(255)
)
AS

SELECT TOP 1
	Id,
	Name, 
	ItemId,
	Description,
	DisplayName
	FROM RDSCollections
	WHERE DisplayName = @Name
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSCollectionById')
DROP PROCEDURE GetRDSCollectionById
GO
CREATE PROCEDURE [dbo].[GetRDSCollectionById]
(
	@ID INT
)
AS

SELECT TOP 1
	Id,
	ItemId,
	Name, 
	Description,
	DisplayName 
	FROM RDSCollections
	WHERE ID = @ID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddRDSCollection')
DROP PROCEDURE AddRDSCollection
GO
CREATE PROCEDURE [dbo].[AddRDSCollection]
(
	@RDSCollectionID INT OUTPUT,
	@ItemID INT,
	@Name NVARCHAR(255),
	@Description NVARCHAR(255),
	@DisplayName NVARCHAR(255)
)
AS

INSERT INTO RDSCollections
(
	ItemID,
	Name,
	Description,
	DisplayName
)
VALUES
(
	@ItemID,
	@Name,
	@Description,
	@DisplayName
)

SET @RDSCollectionID = SCOPE_IDENTITY()

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateRDSCollection')
DROP PROCEDURE UpdateRDSCollection
GO
CREATE PROCEDURE [dbo].[UpdateRDSCollection]
(
	@ID INT,
	@ItemID INT,
	@Name NVARCHAR(255),
	@Description NVARCHAR(255),
	@DisplayName NVARCHAR(255)
)
AS

UPDATE RDSCollections
SET
	ItemID = @ItemID,
	Name = @Name,
	Description = @Description,
	DisplayName = @DisplayName
WHERE ID = @Id
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteRDSCollection')
DROP PROCEDURE DeleteRDSCollection
GO
CREATE PROCEDURE [dbo].[DeleteRDSCollection]
(
	@Id  int
)
AS

UPDATE RDSServers
SET
	RDSCollectionId = Null
WHERE RDSCollectionId = @Id

DELETE FROM RDSCollections
WHERE Id = @Id
GO


-- Password column removed
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSCollectionUsersByRDSCollectionId')
DROP PROCEDURE GetRDSCollectionUsersByRDSCollectionId
GO
CREATE PROCEDURE [dbo].[GetRDSCollectionUsersByRDSCollectionId]
(
	@ID INT
)
AS
SELECT 
	  [AccountID],
	  [ItemID],
	  [AccountType],
	  [AccountName],
	  [DisplayName],
	  [PrimaryEmailAddress],
	  [MailEnabledPublicFolder],
	  [MailboxManagerActions],
	  [SamAccountName],
	  [CreatedDate],
	  [MailboxPlanId],
	  [SubscriberNumber],
	  [UserPrincipalName],
	  [ExchangeDisclaimerId],
	  [ArchivingMailboxPlanId],
	  [EnableArchiving],
	  [LevelID],
	  [IsVIP]
	FROM ExchangeAccounts
	WHERE AccountID IN (Select AccountId from RDSCollectionUsers where RDSCollectionId = @Id)
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddUserToRDSCollection')
DROP PROCEDURE AddUserToRDSCollection
GO
CREATE PROCEDURE [dbo].[AddUserToRDSCollection]
(
	@RDSCollectionID INT,
	@AccountId INT
)
AS

INSERT INTO RDSCollectionUsers
(
	RDSCollectionId, 
	AccountID
)
VALUES
(
	@RDSCollectionID,
	@AccountId
)
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'RemoveRDSUserFromRDSCollection')
DROP PROCEDURE RemoveRDSUserFromRDSCollection
GO
CREATE PROCEDURE [dbo].[RemoveRDSUserFromRDSCollection]
(
	@AccountId  INT,
	@RDSCollectionId INT
)
AS


DELETE FROM RDSCollectionUsers
WHERE AccountId = @AccountId AND RDSCollectionId = @RDSCollectionId
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationRdsUsersCount')
DROP PROCEDURE GetOrganizationRdsUsersCount
GO
CREATE PROCEDURE [dbo].GetOrganizationRdsUsersCount
(
	@ItemID INT,
	@TotalNumber int OUTPUT
)
AS
SELECT
  @TotalNumber = Count(DISTINCT([AccountId]))
  FROM [dbo].[RDSCollectionUsers]
  WHERE [RDSCollectionId] in (SELECT [ID] FROM [RDSCollections] where [ItemId]  = @ItemId )
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationRdsCollectionsCount')
DROP PROCEDURE GetOrganizationRdsCollectionsCount
GO
CREATE PROCEDURE [dbo].GetOrganizationRdsCollectionsCount
(
	@ItemID INT,
	@TotalNumber int OUTPUT
)
AS
SELECT
  @TotalNumber = Count([Id])
  FROM [dbo].[RDSCollections] WHERE [ItemId]  = @ItemId
RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationRdsServersCount')
DROP PROCEDURE GetOrganizationRdsServersCount
GO
CREATE PROCEDURE [dbo].GetOrganizationRdsServersCount
(
	@ItemID INT,
	@TotalNumber int OUTPUT
)
AS
SELECT
  @TotalNumber = Count([Id])
  FROM [dbo].[RDSServers] WHERE [ItemId]  = @ItemId
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSCollectionSettingsByCollectionId')
DROP PROCEDURE GetRDSCollectionSettingsByCollectionId
GO
CREATE PROCEDURE [dbo].[GetRDSCollectionSettingsByCollectionId]
(
	@RDSCollectionID INT
)
AS

SELECT TOP 1
	Id,
	RDSCollectionId,
	DisconnectedSessionLimitMin, 
	ActiveSessionLimitMin,
	IdleSessionLimitMin,
	BrokenConnectionAction,
	AutomaticReconnectionEnabled,
	TemporaryFoldersDeletedOnExit,
	TemporaryFoldersPerSession,
	ClientDeviceRedirectionOptions,
	ClientPrinterRedirected,
	ClientPrinterAsDefault,
	RDEasyPrintDriverEnabled,
	MaxRedirectedMonitors,
	SecurityLayer,
	EncryptionLevel,
	AuthenticateUsingNLA
	
	FROM RDSCollectionSettings
	WHERE RDSCollectionID = @RDSCollectionID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddRDSCollectionSettings')
DROP PROCEDURE AddRDSCollectionSettings
GO
CREATE PROCEDURE [dbo].[AddRDSCollectionSettings]
(
	@RDSCollectionSettingsID INT OUTPUT,
	@RDSCollectionId INT,
	@DisconnectedSessionLimitMin INT, 
	@ActiveSessionLimitMin INT,
	@IdleSessionLimitMin INT,
	@BrokenConnectionAction NVARCHAR(20),
	@AutomaticReconnectionEnabled BIT,
	@TemporaryFoldersDeletedOnExit BIT,
	@TemporaryFoldersPerSession BIT,
	@ClientDeviceRedirectionOptions NVARCHAR(250),
	@ClientPrinterRedirected BIT,
	@ClientPrinterAsDefault BIT,
	@RDEasyPrintDriverEnabled BIT,
	@MaxRedirectedMonitors INT,
	@SecurityLayer NVARCHAR(20),
	@EncryptionLevel NVARCHAR(20),
	@AuthenticateUsingNLA BIT
)
AS

INSERT INTO RDSCollectionSettings
(
	RDSCollectionId,
	DisconnectedSessionLimitMin, 
	ActiveSessionLimitMin,
	IdleSessionLimitMin,
	BrokenConnectionAction,
	AutomaticReconnectionEnabled,
	TemporaryFoldersDeletedOnExit,
	TemporaryFoldersPerSession,
	ClientDeviceRedirectionOptions,
	ClientPrinterRedirected,
	ClientPrinterAsDefault,
	RDEasyPrintDriverEnabled,
	MaxRedirectedMonitors,
	SecurityLayer,
	EncryptionLevel,
	AuthenticateUsingNLA
)
VALUES
(
	@RDSCollectionId,
	@DisconnectedSessionLimitMin, 
	@ActiveSessionLimitMin,
	@IdleSessionLimitMin,
	@BrokenConnectionAction,
	@AutomaticReconnectionEnabled,
	@TemporaryFoldersDeletedOnExit,
	@TemporaryFoldersPerSession,
	@ClientDeviceRedirectionOptions,
	@ClientPrinterRedirected,
	@ClientPrinterAsDefault,
	@RDEasyPrintDriverEnabled,
	@MaxRedirectedMonitors,
	@SecurityLayer,
	@EncryptionLevel,
	@AuthenticateUsingNLA
)

SET @RDSCollectionSettingsID = SCOPE_IDENTITY()

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateRDSCollectionSettings')
DROP PROCEDURE UpdateRDSCollectionSettings
GO
CREATE PROCEDURE [dbo].[UpdateRDSCollectionSettings]
(
	@ID INT,
	@RDSCollectionId INT,
	@DisconnectedSessionLimitMin INT, 
	@ActiveSessionLimitMin INT,
	@IdleSessionLimitMin INT,
	@BrokenConnectionAction NVARCHAR(20),
	@AutomaticReconnectionEnabled BIT,
	@TemporaryFoldersDeletedOnExit BIT,
	@TemporaryFoldersPerSession BIT,
	@ClientDeviceRedirectionOptions NVARCHAR(250),
	@ClientPrinterRedirected BIT,
	@ClientPrinterAsDefault BIT,
	@RDEasyPrintDriverEnabled BIT,
	@MaxRedirectedMonitors INT,
	@SecurityLayer NVARCHAR(20),
	@EncryptionLevel NVARCHAR(20),
	@AuthenticateUsingNLA BIT
)
AS

UPDATE RDSCollectionSettings
SET
	RDSCollectionId = @RDSCollectionId,
	DisconnectedSessionLimitMin = @DisconnectedSessionLimitMin,
	ActiveSessionLimitMin = @ActiveSessionLimitMin,
	IdleSessionLimitMin = @IdleSessionLimitMin,
	BrokenConnectionAction = @BrokenConnectionAction,
	AutomaticReconnectionEnabled = @AutomaticReconnectionEnabled,
	TemporaryFoldersDeletedOnExit = @TemporaryFoldersDeletedOnExit,
	TemporaryFoldersPerSession = @TemporaryFoldersPerSession,
	ClientDeviceRedirectionOptions = @ClientDeviceRedirectionOptions,
	ClientPrinterRedirected = @ClientPrinterRedirected,
	ClientPrinterAsDefault = @ClientPrinterAsDefault,
	RDEasyPrintDriverEnabled = @RDEasyPrintDriverEnabled,
	MaxRedirectedMonitors = @MaxRedirectedMonitors,
	SecurityLayer = @SecurityLayer,
	EncryptionLevel = @EncryptionLevel,
	AuthenticateUsingNLA = @AuthenticateUsingNLA
WHERE ID = @Id
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteRDSCollectionSettings')
DROP PROCEDURE DeleteRDSCollectionSettings
GO
CREATE PROCEDURE [dbo].[DeleteRDSCollectionSettings]
(
	@Id  int
)
AS

DELETE FROM DeleteRDSCollectionSettings
WHERE Id = @Id
GO

-- scp-10269: Changed php extension path in default properties for IIS70 and IIS80 provider
update ServiceDefaultProperties
set PropertyValue='%PROGRAMFILES(x86)%\PHP\php-cgi.exe'
where PropertyName='PhpPath' and ProviderId in(101, 105, 112)

update ServiceDefaultProperties
set PropertyValue='%PROGRAMFILES(x86)%\PHP\php.exe'
where PropertyName='Php4Path' and ProviderId in(101, 105, 112)

GO

-- Exchange2013 Shared and resource mailboxes

-- Exchange2013 Shared and resource mailboxes Quotas
 
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.SharedMailboxes')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) 
VALUES (429, 12, 30, N'Exchange2013.SharedMailboxes', N'Shared Mailboxes per Organization', 2, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.ResourceMailboxes')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) 
VALUES (428, 12, 31, N'Exchange2013.ResourceMailboxes', N'Resource Mailboxes per Organization', 2, 0, NULL, NULL)
END
GO

-- Exchange2013 Automatic Replies Quota

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.AutoReply')
BEGIN
INSERT [dbo].[Quotas]  ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) 
VALUES (729, 12, 32, N'Exchange2013.AutoReply', N'Automatic Replies via SolidCP Allowed', 1, 0, NULL, NULL)
END
GO

-- Exchange2013 Journaling mailboxes
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'Exchange2013.JournalingMailboxes')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota])
	VALUES (731, 12, 31, N'Exchange2013.JournalingMailboxes', N'Journaling Mailboxes per Organization', 2, 0, NULL, NULL)
END
GO

-- Domain lookup tasks

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP')
BEGIN
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_DOMAIN_LOOKUP', N'SolidCP.EnterpriseServer.DomainLookupViewTask, SolidCP.EnterpriseServer.Code', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskViewConfiguration] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP')
BEGIN
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_DOMAIN_LOOKUP', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/DomainLookupView.ascx')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP' AND [ParameterID]= N'DNS_SERVERS' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_LOOKUP', N'DNS_SERVERS', N'String', NULL, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP' AND [ParameterID]= N'MAIL_TO' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_LOOKUP', N'MAIL_TO', N'String', NULL, 2)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP' AND [ParameterID]= N'SERVER_NAME' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_LOOKUP', N'SERVER_NAME', N'String', N'', 3)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP' AND [ParameterID]= N'PAUSE_BETWEEN_QUERIES' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_LOOKUP', N'PAUSE_BETWEEN_QUERIES', N'String', N'100', 4)
END
GO

IF EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP' AND [ParameterID]= N'SERVER_NAME' )
BEGIN
UPDATE [dbo].[ScheduleTaskParameters] SET [DefaultValue] = N'' WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_LOOKUP' AND [ParameterID]= N'SERVER_NAME'
END
GO

-- Domain Expiration Task


IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION')
BEGIN
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_DOMAIN_EXPIRATION', N'SolidCP.EnterpriseServer.DomainExpirationTask, SolidCP.EnterpriseServer.Code', 3)
END
GO

IF EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION' AND [RoleID] = 1)
BEGIN
UPDATE [dbo].[ScheduleTasks] SET [RoleID] = 3 WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION'
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskViewConfiguration] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION')
BEGIN
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_DOMAIN_EXPIRATION', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/DomainExpirationView.ascx')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION' AND [ParameterID]= N'DAYS_BEFORE' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_EXPIRATION', N'DAYS_BEFORE', N'String', NULL, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION' AND [ParameterID]= N'MAIL_TO' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_EXPIRATION', N'MAIL_TO', N'String', NULL, 2)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION' AND [ParameterID]= N'ENABLE_NOTIFICATION' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_EXPIRATION', N'ENABLE_NOTIFICATION', N'Boolean', N'false', 3)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_DOMAIN_EXPIRATION' AND [ParameterID]= N'INCLUDE_NONEXISTEN_DOMAINS' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_DOMAIN_EXPIRATION', N'INCLUDE_NONEXISTEN_DOMAINS', N'Boolean', N'false', 4)
END
GO


-- Domain lookup tables

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'DomainDnsRecords')
DROP TABLE DomainDnsRecords
GO
CREATE TABLE DomainDnsRecords
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	DomainId INT NOT NULL,
	RecordType INT NOT NULL,
	DnsServer NVARCHAR(255),
	Value NVARCHAR(255),
	Date DATETIME
)
GO

ALTER TABLE [dbo].[DomainDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_DomainDnsRecords_DomainId] FOREIGN KEY([DomainId])
REFERENCES [dbo].[Domains] ([DomainID])
ON DELETE CASCADE
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'CreationDate' AND [object_id] = OBJECT_ID(N'Domains'))
BEGIN
	ALTER TABLE [dbo].[Domains] ADD CreationDate DateTime null;
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'ExpirationDate' AND [object_id] = OBJECT_ID(N'Domains'))
BEGIN
	ALTER TABLE [dbo].[Domains] ADD ExpirationDate DateTime null;
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'LastUpdateDate' AND [object_id] = OBJECT_ID(N'Domains'))
BEGIN
	ALTER TABLE [dbo].[Domains] ADD LastUpdateDate DateTime null;
END
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ScheduleTasksEmailTemplates')
DROP TABLE ScheduleTasksEmailTemplates
GO

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'CC' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainExpirationLetter', N'CC', N'support@HostingCompany.com')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainExpirationLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @DomainExpirationLetterHtmlBody nvarchar(2500)

Set @DomainExpirationLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Domain Expiration Information</title>
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
	Domain Expiration Information
</div>

<ad:if test="#user#">
<p>
Hello #user.FirstName#,
</p>
</ad:if>

<p>
Please, find below details of your domain expiration information.
</p>

<table>
    <thead>
        <tr>
            <th>Domain</th>
			<th>Registrar</th>
			<th>Customer</th>
            <th>Expiration Date</th>
        </tr>
    </thead>
    <tbody>
            <ad:foreach collection="#Domains#" var="Domain" index="i">
        <tr>
            <td>#Domain.DomainName#</td>
			<td>#iif(isnull(Domain.Registrar), "", Domain.Registrar)#</td>
			<td>#Domain.Customer#</td>
            <td>#iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#</td>
        </tr>
    </ad:foreach>
    </tbody>
</table>

<ad:if test="#IncludeNonExistenDomains#">
	<p>
	Please, find below details of your non-existen domains.
	</p>

	<table>
		<thead>
			<tr>
				<th>Domain</th>
				<th>Customer</th>
			</tr>
		</thead>
		<tbody>
				<ad:foreach collection="#NonExistenDomains#" var="Domain" index="i">
			<tr>
				<td>#Domain.DomainName#</td>
				<td>#Domain.Customer#</td>
			</tr>
		</ad:foreach>
		</tbody>
	</table>
</ad:if>


<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards
</p>';

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainExpirationLetter', N'HtmlBody', @DomainExpirationLetterHtmlBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @DomainExpirationLetterHtmlBody WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'HtmlBody'
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainExpirationLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainExpirationLetter', N'Subject', N'Domain expiration notification')
END
GO

DECLARE @DomainExpirationLetterTextBody nvarchar(2500)

Set @DomainExpirationLetterTextBody = N'=================================
   Domain Expiration Information
=================================
<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

Please, find below details of your domain expiration information.


<ad:foreach collection="#Domains#" var="Domain" index="i">
	Domain: #Domain.DomainName#
	Registrar: #iif(isnull(Domain.Registrar), "", Domain.Registrar)#
	Customer: #Domain.Customer#
	Expiration Date: #iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#

</ad:foreach>

<ad:if test="#IncludeNonExistenDomains#">
Please, find below details of your non-existen domains.

<ad:foreach collection="#NonExistenDomains#" var="Domain" index="i">
	Domain: #Domain.DomainName#
	Customer: #Domain.Customer#

</ad:foreach>
</ad:if>

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainExpirationLetter', N'TextBody', @DomainExpirationLetterTextBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @DomainExpirationLetterTextBody WHERE [UserID] = 1 AND [SettingsName]= N'DomainExpirationLetter' AND [PropertyName]= N'TextBody'
GO




IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'CC' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'CC', N'support@HostingCompany.com')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @DomainLookupLetterHtmlBody nvarchar(2500)

Set @DomainLookupLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>MX and NS Changes Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.3em; color: ##1F4978; } 
		.Summary H3 { font-size: 1em; color: ##1F4978; } 
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
	MX and NS Changes Information
</div>

<ad:if test="#user#">
<p>
Hello #user.FirstName#,
</p>
</ad:if>

<p>
Please, find below details of MX and NS changes.
</p>

    <ad:foreach collection="#Domains#" var="Domain" index="i">
	<h2>#Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#</h2>
	<h3>#iif(isnull(Domain.Registrar), "", Domain.Registrar)# #iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#</h3>

	<table>
	    <thead>
	        <tr>
	            <th>DNS</th>
				<th>Type</th>
				<th>Status</th>
	            <th>Old Value</th>
                <th>New Value</th>
	        </tr>
	    </thead>
	    <tbody>
	        <ad:foreach collection="#Domain.DnsChanges#" var="DnsChange" index="j">
	        <tr>
	            <td>#DnsChange.DnsServer#</td>
	            <td>#DnsChange.Type#</td>
				<td>#DnsChange.Status#</td>
                <td>#DnsChange.OldRecord.Value#</td>
	            <td>#DnsChange.NewRecord.Value#</td>
	        </tr>
	    	</ad:foreach>
	    </tbody>
	</table>
	
    </ad:foreach>

<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards
</p>'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'HtmlBody', @DomainLookupLetterHtmlBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @DomainLookupLetterHtmlBody WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'HtmlBody'
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'Subject', N'MX and NS changes notification')
END
GO

DECLARE @DomainLookupLetterTextBody nvarchar(2500)

Set @DomainLookupLetterTextBody = N'=================================
   MX and NS Changes Information
=================================
<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

Please, find below details of MX and NS changes.


<ad:foreach collection="#Domains#" var="Domain" index="i">

 #Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#
 Registrar:      #iif(isnull(Domain.Registrar), "", Domain.Registrar)#
 ExpirationDate: #iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#

        <ad:foreach collection="#Domain.DnsChanges#" var="DnsChange" index="j">
            DNS:       #DnsChange.DnsServer#
            Type:      #DnsChange.Type#
	    Status:    #DnsChange.Status#
            Old Value: #DnsChange.OldRecord.Value#
            New Value: #DnsChange.NewRecord.Value#

    	</ad:foreach>
</ad:foreach>



If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards
'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'TextBody',@DomainLookupLetterTextBody )
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @DomainLookupLetterTextBody WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'TextBody'
GO



IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'NoChangesHtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'NoChangesHtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>MX and NS Changes Information</title>
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
	MX and NS Changes Information
</div>

<ad:if test="#user#">
<p>
Hello #user.FirstName#,
</p>
</ad:if>

<p>
No MX and NS changes have been found.
</p>

<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards
</p>')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'DomainLookupLetter' AND [PropertyName]= N'NoChangesTextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DomainLookupLetter', N'NoChangesTextBody', N'=================================
   MX and NS Changes Information
=================================
<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

No MX and NS changes have been founded.

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards
')
END
GO


-- Procedures for Domain lookup service

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetAllPackages')
DROP PROCEDURE GetAllPackages
GO
CREATE PROCEDURE [dbo].[GetAllPackages]
AS
SELECT
	   [PackageID]
      ,[ParentPackageID]
      ,[UserID]
      ,[PackageName]
      ,[PackageComments]
      ,[ServerID]
      ,[StatusID]
      ,[PlanID]
      ,[PurchaseDate]
      ,[OverrideQuotas]
      ,[BandwidthUpdated]
  FROM [dbo].[Packages]
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetScheduleTaskEmailTemplate')
DROP PROCEDURE GetScheduleTaskEmailTemplate
GO
CREATE PROCEDURE [dbo].GetScheduleTaskEmailTemplate
(
	@TaskID [nvarchar](100) 
)
AS
SELECT
	[TaskID],
	[From] ,
	[Subject] ,
	[Template]
  FROM [dbo].[ScheduleTasksEmailTemplates] where [TaskID] = @TaskID 
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetScheduleTask')
DROP PROCEDURE GetScheduleTask
GO
CREATE PROCEDURE GetScheduleTask
(
	@ActorID int,
	@TaskID nvarchar(100)
)
AS
BEGIN
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
		AND @RoleID <= RoleID -- was >= but this seems like a bug, since lower RoleID is more privileged, and in GetScheduleTasks it is also <=.
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetDomainDnsRecords')
DROP PROCEDURE GetDomainDnsRecords
GO
CREATE PROCEDURE [dbo].GetDomainDnsRecords
(
	@DomainId INT,
	@RecordType INT
)
AS
SELECT
	ID,
	DomainId,
	DnsServer,
	RecordType,
	Value,
	Date
  FROM [dbo].[DomainDnsRecords]
  WHERE [DomainId]  = @DomainId AND [RecordType] = @RecordType
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetDomainAllDnsRecords')
DROP PROCEDURE GetDomainAllDnsRecords
GO
CREATE PROCEDURE [dbo].GetDomainAllDnsRecords
(
	@DomainId INT
)
AS
SELECT
	ID,
	DomainId,
	DnsServer,
	RecordType,
	Value,
	Date
  FROM [dbo].[DomainDnsRecords]
  WHERE [DomainId]  = @DomainId 
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddDomainDnsRecord')
DROP PROCEDURE AddDomainDnsRecord
GO
CREATE PROCEDURE [dbo].[AddDomainDnsRecord]
(
	@DomainId INT,
	@RecordType INT,
	@DnsServer NVARCHAR(255),
	@Value NVARCHAR(255),
	@Date DATETIME
)
AS

INSERT INTO DomainDnsRecords
(
	DomainId,
	DnsServer,
	RecordType,
	Value,
	Date
)
VALUES
(
	@DomainId,
	@DnsServer,
	@RecordType,
	@Value,
	@Date
)
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteDomainDnsRecord')
DROP PROCEDURE DeleteDomainDnsRecord
GO
CREATE PROCEDURE [dbo].[DeleteDomainDnsRecord]
(
	@Id  INT
)
AS
DELETE FROM DomainDnsRecords
WHERE Id = @Id
GO

--Domain Expiration Stored Procedures

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateDomainCreationDate')
DROP PROCEDURE UpdateDomainCreationDate
GO
CREATE PROCEDURE [dbo].UpdateDomainCreationDate
(
	@DomainId INT,
	@Date DateTime
)
AS
UPDATE [dbo].[Domains] SET [CreationDate] = @Date WHERE [DomainID] = @DomainId
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateDomainExpirationDate')
DROP PROCEDURE UpdateDomainExpirationDate
GO
CREATE PROCEDURE [dbo].UpdateDomainExpirationDate
(
	@DomainId INT,
	@Date DateTime
)
AS
UPDATE [dbo].[Domains] SET [ExpirationDate] = @Date WHERE [DomainID] = @DomainId
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateDomainLastUpdateDate')
DROP PROCEDURE UpdateDomainLastUpdateDate
GO
CREATE PROCEDURE [dbo].UpdateDomainLastUpdateDate
(
	@DomainId INT,
	@Date DateTime
)
AS
UPDATE [dbo].[Domains] SET [LastUpdateDate] = @Date WHERE [DomainID] = @DomainId
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateDomainDates')
DROP PROCEDURE UpdateDomainDates
GO
CREATE PROCEDURE [dbo].UpdateDomainDates
(
	@DomainId INT,
	@DomainCreationDate DateTime,
	@DomainExpirationDate DateTime,
	@DomainLastUpdateDate DateTime 
)
AS
UPDATE [dbo].[Domains] SET [CreationDate] = @DomainCreationDate, [ExpirationDate] = @DomainExpirationDate, [LastUpdateDate] = @DomainLastUpdateDate WHERE [DomainID] = @DomainId
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateRDSServer')
DROP PROCEDURE UpdateRDSServer
GO
CREATE PROCEDURE [dbo].[UpdateRDSServer]
(
	@Id  INT,
	@ItemID INT,
	@Name NVARCHAR(255),
	@FqdName NVARCHAR(255),
	@Description NVARCHAR(255),
	@RDSCollectionId INT,
	@ConnectionEnabled BIT
)
AS

UPDATE RDSServers
SET
	ItemID = @ItemID,
	Name = @Name,
	FqdName = @FqdName,
	Description = @Description,
	RDSCollectionId = @RDSCollectionId,
	ConnectionEnabled = @ConnectionEnabled
WHERE ID = @Id
GO


-- fix Windows 2012 Provider
BEGIN
UPDATE [dbo].[Providers] SET [EditorControl] = 'Windows2012' WHERE [ProviderName] = 'Windows2012'
END
GO

-- fix check domain used by HostedOrganization

ALTER PROCEDURE [dbo].[CheckDomainUsedByHostedOrganization] 
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
GO


-- check domain used by hosted organization

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationObjectsByDomain')
DROP PROCEDURE GetOrganizationObjectsByDomain
GO

CREATE PROCEDURE [dbo].[GetOrganizationObjectsByDomain]
(
        @ItemID int,
        @DomainName nvarchar(100)
)
AS
SELECT
	'ExchangeAccounts' as ObjectName,
        AccountID as ObjectID,
	AccountType as ObjectType,
        DisplayName as DisplayName,
	0 as OwnerID
FROM
        ExchangeAccounts
WHERE
	UserPrincipalName LIKE '%@'+ @DomainName AND AccountType!=2
UNION
SELECT
	'ExchangeAccountEmailAddresses' as ObjectName,
	eam.AddressID as ObjectID,
	ea.AccountType as ObjectType,
	eam.EmailAddress as DisplayName,
	eam.AccountID as OwnerID
FROM
	ExchangeAccountEmailAddresses as eam
INNER JOIN 
	ExchangeAccounts ea
ON 
	ea.AccountID = eam.AccountID
WHERE
	(ea.PrimaryEmailAddress != eam.EmailAddress)
	AND (ea.UserPrincipalName != eam.EmailAddress)
	AND (eam.EmailAddress LIKE '%@'+ @DomainName)
UNION
SELECT 
	'SfBUsers' as ObjectName,
	ea.AccountID as ObjectID,
	ea.AccountType as ObjectType,
	ea.DisplayName as DisplayName,
	0 as OwnerID
FROM 
	ExchangeAccounts ea 
INNER JOIN 
	SfBUsers ou
ON 
	ea.AccountID = ou.AccountID
WHERE 
	ou.SipAddress LIKE '%@'+ @DomainName
UNION
SELECT 
	'LyncUsers' as ObjectName,
	ea.AccountID as ObjectID,
	ea.AccountType as ObjectType,
	ea.DisplayName as DisplayName,
	0 as OwnerID
FROM 
	ExchangeAccounts ea 
INNER JOIN 
	LyncUsers ou
ON 
	ea.AccountID = ou.AccountID
WHERE 
	ou.SipAddress LIKE '%@'+ @DomainName
ORDER BY 
	DisplayName
RETURN
GO
IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'RegistrarName' AND [object_id] = OBJECT_ID(N'Domains'))
BEGIN
	ALTER TABLE [dbo].[Domains] ADD RegistrarName nvarchar(max);
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateWhoisDomainInfo')
DROP PROCEDURE UpdateWhoisDomainInfo
GO
CREATE PROCEDURE [dbo].UpdateWhoisDomainInfo
(
	@DomainId INT,
	@DomainCreationDate DateTime,
	@DomainExpirationDate DateTime,
	@DomainLastUpdateDate DateTime,
	@DomainRegistrarName nvarchar(max)
)
AS
UPDATE [dbo].[Domains] SET [CreationDate] = @DomainCreationDate, [ExpirationDate] = @DomainExpirationDate, [LastUpdateDate] = @DomainLastUpdateDate, [RegistrarName] = @DomainRegistrarName WHERE [DomainID] = @DomainId
GO


IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='Packages' AND COLS.name='DefaultTopPackage')
BEGIN
ALTER TABLE [dbo].[Packages] ADD
	[DefaultTopPackage] [bit] DEFAULT 0 NOT NULL
END
GO

-- add Status ID Change date
IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='Packages' AND COLS.name='StatusIDchangeDate')
BEGIN
ALTER TABLE [dbo].[Packages] ADD
	[StatusIDchangeDate] [datetime] DEFAULT (GETDATE()) NOT NULL
END
GO

-- add Triger update
IF NOT EXISTS (select * from sys.objects where type = 'TR' and name = 'Update_StatusIDchangeDate')
EXEC dbo.sp_executesql @statement = N' 
CREATE TRIGGER [dbo].[Update_StatusIDchangeDate]
   ON [dbo].[Packages]
   AFTER UPDATE
AS BEGIN
    UPDATE Packages 
		SET StatusIDchangeDate = GETDATE()

    FROM Packages P 
    INNER JOIN Inserted I ON P.PackageID = I.PackageID
    INNER JOIN Deleted D ON P.PackageID = D.PackageID                  
    WHERE  D.StatusID <> I.StatusID AND I.StatusID > 1 --dont update if nothing change and keep ChangeDate if server back to active  
    --AND P.StatusID <> I.StatusID
END
'
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetMyPackages')
DROP PROCEDURE GetMyPackages
GO
CREATE PROCEDURE [dbo].[GetMyPackages]
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
  	P.StatusIDchangeDate,
	
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
	U.Email,

	P.DefaultTopPackage
FROM Packages AS P
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
LEFT OUTER JOIN Servers AS S ON P.ServerID = S.ServerID
LEFT OUTER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE P.UserID = @UserID
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackages')
DROP PROCEDURE GetPackages
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
  	P.StatusIDchangeDate,
	
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
	U.Email,

	P.DefaultTopPackage
FROM Packages AS P
INNER JOIN Users AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	P.UserID = @UserID	
RETURN

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackage')
DROP PROCEDURE GetPackage
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
  	P.StatusIDchangeDate,
	P.OverrideQuotas,
	P.DefaultTopPackage
FROM Packages AS P
WHERE P.PackageID = @PackageID
RETURN

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdatePackage')
DROP PROCEDURE UpdatePackage
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
	@QuotasXml ntext,
	@DefaultTopPackage bit
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
	OverrideQuotas = @OverrideQuotas,
	DefaultTopPackage = @DefaultTopPackage
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


-- WebDAv portal

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'WebDavAccessTokens')
DROP TABLE WebDavAccessTokens
GO
CREATE TABLE WebDavAccessTokens
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FilePath NVARCHAR(MAX) NOT NULL,
	AuthData NVARCHAR(MAX) NOT NULL,
	AccessToken UNIQUEIDENTIFIER NOT NULL,
	ExpirationDate DATETIME NOT NULL,
	AccountID INT NOT NULL ,
	ItemId INT NOT NULL
)
GO

ALTER TABLE [dbo].[WebDavAccessTokens]  WITH CHECK ADD  CONSTRAINT [FK_WebDavAccessTokens_UserId] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddWebDavAccessToken')
DROP PROCEDURE AddWebDavAccessToken
GO
CREATE PROCEDURE [dbo].[AddWebDavAccessToken]
(
	@TokenID INT OUTPUT,
	@FilePath NVARCHAR(MAX),
	@AccessToken UNIQUEIDENTIFIER,
	@AuthData NVARCHAR(MAX),
	@ExpirationDate DATETIME,
	@AccountID INT,
	@ItemId INT
)
AS
INSERT INTO WebDavAccessTokens
(
	FilePath,
	AccessToken,
	AuthData,
	ExpirationDate,
	AccountID  ,
	ItemId
)
VALUES
(
	@FilePath ,
	@AccessToken  ,
	@AuthData,
	@ExpirationDate ,
	@AccountID,
	@ItemId
)

SET @TokenID = SCOPE_IDENTITY()

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteExpiredWebDavAccessTokens')
DROP PROCEDURE DeleteExpiredWebDavAccessTokens
GO
CREATE PROCEDURE [dbo].[DeleteExpiredWebDavAccessTokens]
AS
DELETE FROM WebDavAccessTokens
WHERE ExpirationDate < getdate()
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetWebDavAccessTokenById')
DROP PROCEDURE GetWebDavAccessTokenById
GO
CREATE PROCEDURE [dbo].[GetWebDavAccessTokenById]
(
	@Id int
)
AS
SELECT 
	ID ,
	FilePath ,
	AuthData ,
	AccessToken,
	ExpirationDate,
	AccountID,
	ItemId
	FROM WebDavAccessTokens 
	Where ID = @Id AND ExpirationDate > getdate()
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetWebDavAccessTokenByAccessToken')
DROP PROCEDURE GetWebDavAccessTokenByAccessToken
GO
CREATE PROCEDURE [dbo].[GetWebDavAccessTokenByAccessToken]
(
	@AccessToken UNIQUEIDENTIFIER
)
AS
SELECT 
	ID ,
	FilePath ,
	AuthData ,
	AccessToken,
	ExpirationDate,
	AccountID,
	ItemId
	FROM WebDavAccessTokens 
	Where AccessToken = @AccessToken AND ExpirationDate > getdate()
GO

--add Deleted Users Quota
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedSolution.DeletedUsers')
BEGIN
	INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (495, 13, 6, N'HostedSolution.DeletedUsers', N'Deleted Users', 2, 0, NULL, NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'HostedSolution.DeletedUsersBackupStorageSpace')
BEGIN
	INSERT [dbo].[Quotas]  ([QuotaID], [GroupID],[QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (496, 13, 6, N'HostedSolution.DeletedUsersBackupStorageSpace', N'Deleted Users Backup Storage Space, Mb', 2, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ExchangeDeletedAccounts')
CREATE TABLE ExchangeDeletedAccounts 
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	AccountID INT NOT NULL,
	OriginAT INT NOT NULL,
	StoragePath NVARCHAR(255) NULL,
	FolderName NVARCHAR(128) NULL,
	FileName NVARCHAR(128) NULL,
	ExpirationDate DATETIME NOT NULL
)
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationStatistics')
DROP PROCEDURE [dbo].[GetOrganizationStatistics]
GO

CREATE PROCEDURE [dbo].[GetOrganizationStatistics]
(
	@ItemID int
)
AS
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 7 OR AccountType = 1 OR AccountType = 6 OR AccountType = 5)  AND ItemID = @ItemID) AS CreatedUsers,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 8 OR AccountType = 9)  AND ItemID = @ItemID) AS CreatedGroups,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 11  AND ItemID = @ItemID) AS DeletedUsers
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteOrganizationDeletedUser')
DROP PROCEDURE [dbo].[DeleteOrganizationDeletedUser]
GO

CREATE PROCEDURE [dbo].[DeleteOrganizationDeletedUser]
(
	@ID int
)
AS
DELETE FROM	ExchangeDeletedAccounts WHERE AccountID = @ID
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationDeletedUser')
DROP PROCEDURE [dbo].[GetOrganizationDeletedUser]
GO

CREATE PROCEDURE [dbo].[GetOrganizationDeletedUser]
(
	@AccountID int
)
AS
SELECT
	EDA.AccountID,
	EDA.OriginAT,
	EDA.StoragePath,
	EDA.FolderName,
	EDA.FileName,
	EDA.ExpirationDate
FROM
	ExchangeDeletedAccounts AS EDA
WHERE
	EDA.AccountID = @AccountID
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddOrganizationDeletedUser')
DROP PROCEDURE [dbo].[AddOrganizationDeletedUser]
GO

CREATE PROCEDURE [dbo].[AddOrganizationDeletedUser] 
(
	@ID int OUTPUT,
	@AccountID int,
	@OriginAT int,
	@StoragePath nvarchar(255),
	@FolderName nvarchar(128),
	@FileName nvarchar(128),
	@ExpirationDate datetime
)
AS

INSERT INTO ExchangeDeletedAccounts
(
	AccountID,
	OriginAT,
	StoragePath,
	FolderName,
	FileName,
	ExpirationDate
)
VALUES
(
	@AccountID,
	@OriginAT,
	@StoragePath,
	@FolderName,
	@FileName,
	@ExpirationDate
)

SET @ID = SCOPE_IDENTITY()

RETURN
GO



IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='ExchangeMailboxPlans' AND COLS.name='EnableForceArchiveDeletion')
BEGIN
	ALTER TABLE [dbo].[ExchangeMailboxPlans] ADD [EnableForceArchiveDeletion] [bit] NULL
END
GO

ALTER PROCEDURE [dbo].[AddExchangeMailboxPlan] 
(
	@MailboxPlanId int OUTPUT,
	@ItemID int,
	@MailboxPlan	nvarchar(300),
	@EnableActiveSync bit,
	@EnableIMAP bit,
	@EnableMAPI bit,
	@EnableOWA bit,
	@EnablePOP bit,
	@EnableAutoReply bit,
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
	@MailboxPlanType int,
	@AllowLitigationHold bit,
	@RecoverableItemsWarningPct int,
	@RecoverableItemsSpace int,
	@LitigationHoldUrl nvarchar(256),
	@LitigationHoldMsg nvarchar(512),
	@Archiving bit,
	@EnableArchiving bit,
	@ArchiveSizeMB int,
	@ArchiveWarningPct int,
	@EnableForceArchiveDeletion bit,
	@IsForJournaling bit
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
	EnableAutoReply,
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
	MailboxPlanType,
	AllowLitigationHold,
	RecoverableItemsWarningPct,
	RecoverableItemsSpace,
	LitigationHoldUrl,
	LitigationHoldMsg,
	Archiving,
	EnableArchiving,
	ArchiveSizeMB,
	ArchiveWarningPct,
	EnableForceArchiveDeletion,
	IsForJournaling
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
	@EnableAutoReply,
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
	@MailboxPlanType,
	@AllowLitigationHold,
	@RecoverableItemsWarningPct,
	@RecoverableItemsSpace,
	@LitigationHoldUrl,
	@LitigationHoldMsg,
	@Archiving,
	@EnableArchiving,
	@ArchiveSizeMB,
	@ArchiveWarningPct,
	@EnableForceArchiveDeletion,
	@IsForJournaling
)

SET @MailboxPlanId = SCOPE_IDENTITY()

RETURN
GO

ALTER PROCEDURE [dbo].[UpdateExchangeMailboxPlan] 
(
	@MailboxPlanId int,
	@MailboxPlan	nvarchar(300),
	@EnableActiveSync bit,
	@EnableIMAP bit,
	@EnableMAPI bit,
	@EnableOWA bit,
	@EnablePOP bit,
	@EnableAutoReply bit,
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
	@MailboxPlanType int,
	@AllowLitigationHold bit,
	@RecoverableItemsWarningPct int,
	@RecoverableItemsSpace int,
	@LitigationHoldUrl nvarchar(256),
	@LitigationHoldMsg nvarchar(512),
	@Archiving bit,
	@EnableArchiving bit,
	@ArchiveSizeMB int,
	@ArchiveWarningPct int,
	@EnableForceArchiveDeletion bit,
	@IsForJournaling bit
)
AS

UPDATE ExchangeMailboxPlans SET
	MailboxPlan = @MailboxPlan,
	EnableActiveSync = @EnableActiveSync,
	EnableIMAP = @EnableIMAP,
	EnableMAPI = @EnableMAPI,
	EnableOWA = @EnableOWA,
	EnablePOP = @EnablePOP,
	EnableAutoReply = @EnableAutoReply,
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
	MailboxPlanType = @MailboxPlanType,
	AllowLitigationHold = @AllowLitigationHold,
	RecoverableItemsWarningPct = @RecoverableItemsWarningPct,
	RecoverableItemsSpace = @RecoverableItemsSpace, 
	LitigationHoldUrl = @LitigationHoldUrl,
	LitigationHoldMsg = @LitigationHoldMsg,
	Archiving = @Archiving,
	EnableArchiving = @EnableArchiving,
	ArchiveSizeMB = @ArchiveSizeMB,
	ArchiveWarningPct = @ArchiveWarningPct,
	EnableForceArchiveDeletion = @EnableForceArchiveDeletion,
	IsForJournaling = @IsForJournaling
WHERE MailboxPlanId = @MailboxPlanId

RETURN
GO

ALTER PROCEDURE [dbo].[GetExchangeMailboxPlan] 
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
	EnableAutoReply,
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
	MailboxPlanType,
	AllowLitigationHold,
	RecoverableItemsWarningPct,
	RecoverableItemsSpace,
	LitigationHoldUrl,
	LitigationHoldMsg,
	Archiving,
	EnableArchiving,
	ArchiveSizeMB,
	ArchiveWarningPct,
	EnableForceArchiveDeletion,
	IsForJournaling
FROM
	ExchangeMailboxPlans
WHERE
	MailboxPlanId = @MailboxPlanId
RETURN
GO

ALTER PROCEDURE [dbo].[GetExchangeMailboxPlans]
(
	@ItemID int,
	@Archiving bit
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
	EnableAutoReply,
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
	MailboxPlanType,
	Archiving,
	EnableArchiving,
	ArchiveSizeMB,
	ArchiveWarningPct,
	EnableForceArchiveDeletion,
	IsForJournaling
FROM
	ExchangeMailboxPlans
WHERE
	ItemID = @ItemID 
AND ((Archiving=@Archiving) OR ((@Archiving=0) AND (Archiving IS NULL)))
ORDER BY MailboxPlan
RETURN
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = 'SCHEDULE_TASK_DELETE_EXCHANGE_ACCOUNTS')
BEGIN
INSERT INTO [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_DELETE_EXCHANGE_ACCOUNTS', N'SolidCP.EnterpriseServer.DeleteExchangeAccountsTask, SolidCP.EnterpriseServer.Code', 3)
END
GO


ALTER PROCEDURE [dbo].[UpdateServiceItem]
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
GO


-- Password column removed
IF OBJECTPROPERTY(object_id('dbo.GetExchangeAccountByAccountNameWithoutItemId'), N'IsProcedure') = 1
DROP PROCEDURE [dbo].[GetExchangeAccountByAccountNameWithoutItemId]
GO
CREATE PROCEDURE [dbo].[GetExchangeAccountByAccountNameWithoutItemId] 
(
	@UserPrincipalName nvarchar(300)
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
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName,
	E.ArchivingMailboxPlanId, 
	AP.MailboxPlan as 'ArchivingMailboxPlan',
	E.EnableArchiving
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId	
LEFT OUTER JOIN ExchangeMailboxPlans AS AP ON E.ArchivingMailboxPlanId = AP.MailboxPlanId
WHERE
	E.UserPrincipalName = @UserPrincipalName
RETURN
GO



--Webdav portal users settings

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'WebDavPortalUsersSettings')
CREATE TABLE WebDavPortalUsersSettings
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	AccountId INT NOT NULL,
	Settings NVARCHAR(max)
)
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_WebDavPortalUsersSettings_UserId')
ALTER TABLE [dbo].[WebDavPortalUsersSettings]
DROP CONSTRAINT [FK_WebDavPortalUsersSettings_UserId]
GO

ALTER TABLE [dbo].[WebDavPortalUsersSettings]  WITH CHECK ADD  CONSTRAINT [FK_WebDavPortalUsersSettings_UserId] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
ON DELETE CASCADE
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetWebDavPortalUsersSettingsByAccountId')
DROP PROCEDURE GetWebDavPortalUsersSettingsByAccountId
GO
CREATE PROCEDURE [dbo].[GetWebDavPortalUsersSettingsByAccountId]
(
	@AccountId INT
)
AS
SELECT TOP 1
	US.Id,
	US.AccountId,
	US.Settings
	FROM WebDavPortalUsersSettings AS US
	WHERE AccountId = @AccountId
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddWebDavPortalUsersSettings')
DROP PROCEDURE AddWebDavPortalUsersSettings
GO
CREATE PROCEDURE [dbo].[AddWebDavPortalUsersSettings]
(
	@WebDavPortalUsersSettingsId INT OUTPUT,
	@AccountId INT,
	@Settings NVARCHAR(max)
)
AS

INSERT INTO WebDavPortalUsersSettings
(
	AccountId,
	Settings
)
VALUES
(
	@AccountId,
	@Settings
)

SET @WebDavPortalUsersSettingsId = SCOPE_IDENTITY()

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateWebDavPortalUsersSettings')
DROP PROCEDURE UpdateWebDavPortalUsersSettings
GO
CREATE PROCEDURE [dbo].[UpdateWebDavPortalUsersSettings]
(
	@AccountId INT,
	@Settings NVARCHAR(max)
)
AS

UPDATE WebDavPortalUsersSettings
SET
	Settings = @Settings
WHERE AccountId = @AccountId
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'SmarterMail 10.x +')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(66, 4, N'SmarterMail', N'SmarterMail 10.x +', N'SolidCP.Providers.Mail.SmarterMail10, SolidCP.Providers.Mail.SmarterMail10', N'SmarterMail100', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [EditorControl] = 'SmarterMail100' WHERE [DisplayName] = 'SmarterMail 10.x +'
END
GO


-- Service items count by name and serviceid

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetServiceItemsCountByNameAndServiceId')
DROP PROCEDURE GetServiceItemsCountByNameAndServiceId
GO

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
GO

-- Hyper-V 2012 R2 Provider
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'VPS2012')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (33, N'VPS2012', 19, NULL, 1)

INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (41, 33, N'VirtualMachine', N'SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base', 1, 0, 0, 1, 1, 1, 0, 0)

INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (42, 33, N'VirtualSwitch', N'SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base', 2, 0, 0, 1, 1, 1, 0, 0)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (553, 33, 1, N'VPS2012.ServersNumber', N'Number of VPS', 2, 0, 41, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (554, 33, 2, N'VPS2012.ManagingAllowed', N'Allow user to create VPS', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (555, 33, 3, N'VPS2012.CpuNumber', N'Number of CPU cores', 3, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (556, 33, 7, N'VPS2012.BootCdAllowed', N'Boot from CD allowed', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (557, 33, 8, N'VPS2012.BootCdEnabled', N'Boot from CD', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (558, 33, 4, N'VPS2012.Ram', N'RAM size, MB', 2, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (559, 33, 5, N'VPS2012.Hdd', N'Hard Drive size, GB', 2, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (560, 33, 6, N'VPS2012.DvdEnabled', N'DVD drive', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (561, 33, 10, N'VPS2012.ExternalNetworkEnabled', N'External Network', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (562, 33, 11, N'VPS2012.ExternalIPAddressesNumber', N'Number of External IP addresses', 2, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (563, 33, 13, N'VPS2012.PrivateNetworkEnabled', N'Private Network', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (564, 33, 14, N'VPS2012.PrivateIPAddressesNumber', N'Number of Private IP addresses per VPS', 3, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (565, 33, 9, N'VPS2012.SnapshotsNumber', N'Number of Snaphots', 3, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (566, 33, 15, N'VPS2012.StartShutdownAllowed', N'Allow user to Start, Turn off and Shutdown VPS', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (567, 33, 16, N'VPS2012.PauseResumeAllowed', N'Allow user to Pause, Resume VPS', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (568, 33, 17, N'VPS2012.RebootAllowed', N'Allow user to Reboot VPS', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (569, 33, 18, N'VPS2012.ResetAlowed', N'Allow user to Reset VPS', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (570, 33, 19, N'VPS2012.ReinstallAllowed', N'Allow user to Re-install VPS', 1, 0, NULL, NULL)

INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (571, 33, 12, N'VPS2012.Bandwidth', N'Monthly bandwidth, GB', 2, 0, NULL, NULL)

END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '572')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (572, 33, 20, N'VPS2012.ReplicationEnabled', N'Allow user to Replication', 1, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HyperV2012R2')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (350, 33, N'HyperV2012R2', N'Microsoft Hyper-V 2012 R2', N'SolidCP.Providers.Virtualization.HyperV2012R2, SolidCP.Providers.Virtualization.HyperV2012R2', N'HyperV2012R2', 1)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [EditorControl] = N'HyperV2012R2', [GroupID] = 33 WHERE [ProviderName] = 'HyperV2012R2'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HyperVvmm')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (351, 33, N'HyperVvmm', N'Microsoft Hyper-V Virtual Machine Management', N'SolidCP.Providers.Virtualization.HyperVvmm, SolidCP.Providers.Virtualization.HyperVvmm', N'HyperVvmm', 1)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [EditorControl] = N'HyperVvmm', [GroupID] = 33 WHERE [ProviderName] = 'HyperVvmm'
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetVirtualMachinesPagedForPC')
DROP PROCEDURE GetVirtualMachinesPagedForPC
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

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (ItemName LIKE ''' + @FilterValue + '''
			OR Username LIKE ''' + @FilterValue + '''
			OR ExternalIP LIKE ''' + @FilterValue + '''
			OR IPAddress LIKE ''' + @FilterValue + ''')'
END

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

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetVirtualMachinesPaged2012')
DROP PROCEDURE GetVirtualMachinesPaged2012
GO
CREATE PROCEDURE [dbo].[GetVirtualMachinesPaged2012]
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
SI.ItemTypeID = 41 -- VPS2012
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
'

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (ItemName LIKE ''' + @FilterValue + '''
			OR Username LIKE ''' + @FilterValue + '''
			OR ExternalIP LIKE ''' + @FilterValue + '''
			OR IPAddress LIKE ''' + @FilterValue + ''')'
END

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
	LEFT OUTER JOIN DmzIPAddresses AS DIP ON DIP.ItemID = SI.ItemID AND DIP.IsPrimary = 1
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
	PIP.IPAddress,
	DIP.IPAddress AS DmzIP
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
LEFT OUTER JOIN DmzIPAddresses AS DIP ON DIP.ItemID = SI.ItemID AND DIP.IsPrimary = 1
'

--print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

RETURN 
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetVirtualMachinesPaged')
DROP PROCEDURE GetVirtualMachinesPaged
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

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (ItemName LIKE ''' + @FilterValue + '''
			OR Username LIKE ''' + @FilterValue + '''
			OR ExternalIP LIKE ''' + @FilterValue + '''
			OR IPAddress LIKE ''' + @FilterValue + ''')'
END

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

--ES OWA Editing
IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'EnterpriseFoldersOwaPermissions')
CREATE TABLE EnterpriseFoldersOwaPermissions
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ItemID INT NOT NULL,
	FolderID INT NOT NULL, 
	AccountID INT NOT NULL 
)
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_EnterpriseFoldersOwaPermissions_AccountId')
ALTER TABLE [dbo].[EnterpriseFoldersOwaPermissions]
DROP CONSTRAINT [FK_EnterpriseFoldersOwaPermissions_AccountId]
GO

ALTER TABLE [dbo].[EnterpriseFoldersOwaPermissions]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseFoldersOwaPermissions_AccountId] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_EnterpriseFoldersOwaPermissions_FolderId')
ALTER TABLE [dbo].[EnterpriseFoldersOwaPermissions]
DROP CONSTRAINT [FK_EnterpriseFoldersOwaPermissions_FolderId]
GO

ALTER TABLE [dbo].[EnterpriseFoldersOwaPermissions]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseFoldersOwaPermissions_FolderId] FOREIGN KEY([FolderID])
REFERENCES [dbo].[EnterpriseFolders] ([EnterpriseFolderID])
ON DELETE CASCADE
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteAllEnterpriseFolderOwaUsers')
DROP PROCEDURE DeleteAllEnterpriseFolderOwaUsers
GO
CREATE PROCEDURE [dbo].[DeleteAllEnterpriseFolderOwaUsers]
(
	@ItemID  int,
	@FolderID int
)
AS
DELETE FROM EnterpriseFoldersOwaPermissions
WHERE ItemId = @ItemID AND FolderID = @FolderID
GO





IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddEnterpriseFolderOwaUser')
DROP PROCEDURE AddEnterpriseFolderOwaUser
GO
CREATE PROCEDURE [dbo].[AddEnterpriseFolderOwaUser]
(
	@ESOwsaUserId INT OUTPUT,
	@ItemID INT,
	@FolderID INT, 
	@AccountID INT 
)
AS
INSERT INTO EnterpriseFoldersOwaPermissions
(
	ItemID ,
	FolderID, 
	AccountID
)
VALUES
(
	@ItemID,
	@FolderID, 
	@AccountID 
)

SET @ESOwsaUserId = SCOPE_IDENTITY()

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetEnterpriseFolderOwaUsers')
DROP PROCEDURE GetEnterpriseFolderOwaUsers
GO
CREATE PROCEDURE [dbo].[GetEnterpriseFolderOwaUsers]
(
	@ItemID INT,
	@FolderID INT
)
AS
SELECT 
	EA.AccountID,
	EA.ItemID,
	EA.AccountType,
	EA.AccountName,
	EA.DisplayName,
	EA.PrimaryEmailAddress,
	EA.MailEnabledPublicFolder,
	EA.MailboxPlanId,
	EA.SubscriberNumber,
	EA.UserPrincipalName 
	FROM EnterpriseFoldersOwaPermissions AS EFOP
	LEFT JOIN  ExchangeAccounts AS EA ON EA.AccountID = EFOP.AccountID
	WHERE EFOP.ItemID = @ItemID AND EFOP.FolderID = @FolderID
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetEnterpriseFolderId')
DROP PROCEDURE GetEnterpriseFolderId
GO
CREATE PROCEDURE [dbo].[GetEnterpriseFolderId]
(
	@ItemID INT,
	@FolderName varchar(max)
)
AS
SELECT TOP 1
	EnterpriseFolderID
	FROM EnterpriseFolders
	WHERE ItemId = @ItemID AND FolderName = @FolderName
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetUserEnterpriseFolderWithOwaEditPermission')
DROP PROCEDURE GetUserEnterpriseFolderWithOwaEditPermission
GO
CREATE PROCEDURE [dbo].[GetUserEnterpriseFolderWithOwaEditPermission]
(
	@ItemID INT,
	@AccountID INT
)
AS
SELECT 
	EF.FolderName
	FROM EnterpriseFoldersOwaPermissions AS EFOP
	LEFT JOIN  [dbo].[EnterpriseFolders] AS EF ON EF.EnterpriseFolderID = EFOP.FolderID
	WHERE EFOP.ItemID = @ItemID AND EFOP.AccountID = @AccountID
GO


-- CRM2015 Provider

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted MS CRM 2015')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) 
VALUES(1205, 24, N'CRM', N'Hosted MS CRM 2015', N'SolidCP.Providers.HostedSolution.CRMProvider2015, SolidCP.Providers.HostedSolution.Crm2015', N'CRM2011', NULL)
END
GO

-- RDS Setup Instructions

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'CC' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'RDSSetupLetter', N'CC', N'support@HostingCompany.com')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'RDSSetupLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @RDSSetupLetterHtmlBody nvarchar(2500)

Set @RDSSetupLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>RDS Setup Information</title>
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
	RDS Setup Information
</div>
</div>
</body>';

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'RDSSetupLetter', N'HtmlBody', @RDSSetupLetterHtmlBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @RDSSetupLetterHtmlBody WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'HtmlBody'
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'RDSSetupLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'RDSSetupLetter', N'Subject', N'RDS setup')
END
GO

DECLARE @RDSSetupLetterTextBody nvarchar(2500)

Set @RDSSetupLetterTextBody = N'=================================
   RDS Setup Information
=================================
<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

Please, find below RDS setup instructions.

If you have any questions, feel free to contact our support department at any time.

Best regards'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'RDSSetupLetter', N'TextBody', @RDSSetupLetterTextBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @RDSSetupLetterTextBody WHERE [UserID] = 1 AND [SettingsName]= N'RDSSetupLetter' AND [PropertyName]= N'TextBody'
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Foundation Server')
BEGIN	
	DECLARE @group_order AS INT
	DECLARE @group_controller AS NVARCHAR(1000)
	DECLARE @group_id AS INT
	DECLARE @provider_id AS INT

	UPDATE [dbo].[ResourceGroups] SET GroupName = 'Sharepoint Foundation Server' WHERE GroupName = 'Hosted Sharepoint'
	SELECT @group_order = GroupOrder, @group_controller = GroupController FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Foundation Server'	
	SELECT TOP 1 @group_id = GroupId + 1 From [dbo].[ResourceGroups] ORDER BY GroupID DESC
	SELECT TOP 1 @provider_id = ProviderId + 1 From [dbo].[Providers] ORDER BY ProviderID DESC
	UPDATE [dbo].[ResourceGroups] SET GroupOrder = GroupOrder + 1 WHERE GroupOrder > @group_order
	INSERT INTO [dbo].[ResourceGroups] (GroupID, GroupName, GroupOrder, GroupController, ShowGroup) VALUES (@group_id, 'Sharepoint Server', @group_order + 1, @group_controller, 1)

	INSERT INTO [dbo].[Quotas] (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota)
		VALUES (550, @group_id, 1, 'HostedSharePointServer.Sites', 'SharePoint Site Collections', 2, 0)
	INSERT INTO [dbo].[Quotas] (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota)
		VALUES (551, @group_id, 2, 'HostedSharePointServer.MaxStorage', 'Max site storage, MB', 3, 0)
	INSERT INTO [dbo].[Quotas] (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota)
		VALUES (552, @group_id, 3, 'HostedSharePointServer.UseSharedSSL', 'Use shared SSL Root', 1, 0)
END

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetLyncUsers')
DROP PROCEDURE GetLyncUsers
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
BEGIN

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

	DECLARE @sql nvarchar(3500)

	set @sql = '
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
			ea.ItemID = @ItemID ' + @condition

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
		END
		ELSE
		BEGIN
			SELECT * FROM #TempLyncUsers 
				WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY UserPrincipalName DESC
		END	
	END

	DROP TABLE #TempLyncUsers
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'SearchOrganizationAccounts')
DROP PROCEDURE SearchOrganizationAccounts
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
 EA.UserPrincipalName,
 EA.LevelID,
 EA.IsVIP,
 (CASE WHEN LU.AccountID IS NULL THEN ''false'' ELSE ''true'' END) as IsLyncUser,
 (CASE WHEN SfB.AccountID IS NULL THEN ''false'' ELSE ''true'' END) as IsSfBUser
FROM ExchangeAccounts AS EA
LEFT JOIN LyncUsers AS LU
ON LU.AccountID = EA.AccountID
LEFT JOIN SfBUsers AS SfB  
ON SfB.AccountID = EA.AccountID
WHERE ' + @condition + '
ORDER BY ' + @sortColumn

print @sql

exec sp_executesql @sql, N'@ItemID int, @IncludeMailboxes bit', 
@ItemID, @IncludeMailboxes

RETURN 

GO


-- RDS GPO

IF NOT EXISTS(SELECT * FROM SYS.TABLES WHERE name = 'RDSServerSettings')
CREATE TABLE [dbo].[RDSServerSettings](
	[RdsServerId] [int] NOT NULL,
	[SettingsName] [nvarchar](50) NOT NULL,
	[PropertyName] [nvarchar](50) NOT NULL,
	[PropertyValue] [ntext] NULL,
	[ApplyUsers] [BIT] NOT NULL,
	[ApplyAdministrators] [BIT] NOT NULL
 CONSTRAINT [PK_RDSServerSettings] PRIMARY KEY CLUSTERED 
(
	[RdsServerId] ASC,
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSServerSettings')
DROP PROCEDURE GetRDSServerSettings
GO
CREATE PROCEDURE GetRDSServerSettings
(
	@ServerId int,
	@SettingsName nvarchar(50)
)
AS
	SELECT RDSServerId, PropertyName, PropertyValue, ApplyUsers, ApplyAdministrators
	FROM RDSServerSettings
	WHERE RDSServerId = @ServerId AND SettingsName = @SettingsName			
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteRDSServerSettings')
DROP PROCEDURE DeleteRDSServerSettings
GO
CREATE PROCEDURE DeleteRDSServerSettings
(
	@ServerId int
)
AS
	DELETE FROM RDSServerSettings WHERE RDSServerId = @ServerId
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateRDSServerSettings')
DROP PROCEDURE UpdateRDSServerSettings
GO
CREATE PROCEDURE UpdateRDSServerSettings
(
	@ServerId int,
	@SettingsName nvarchar(50),
	@Xml ntext
)
AS

BEGIN TRAN
DECLARE @idoc int
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

DELETE FROM RDSServerSettings
WHERE RDSServerId = @ServerId AND SettingsName = @SettingsName

INSERT INTO RDSServerSettings
(
	RDSServerId,
	SettingsName,
	ApplyUsers,
	ApplyAdministrators,
	PropertyName,
	PropertyValue	
)
SELECT
	@ServerId,
	@SettingsName,
	ApplyUsers,
	ApplyAdministrators,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue ntext '@value',
	ApplyUsers BIT '@applyUsers',
	ApplyAdministrators BIT '@applyAdministrators'
) as PV

exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN 

GO


IF EXISTS (SELECT * FROM ResourceGroups WHERE GroupName = 'SharePoint')
BEGIN
	DECLARE @group_id INT
	SELECT @group_id = GroupId FROM ResourceGroups WHERE GroupName = 'SharePoint'
	DELETE FROM PackageQuotas WHERE QuotaID IN (SELECT QuotaID FROM Quotas WHERE GroupID = @group_id)
	DELETE FROM HostingPlanQuotas WHERE QuotaID IN (SELECT QuotaID FROM Quotas WHERE GroupID = @group_id)
	DELETE FROM HostingPlanResources WHERE GroupId = @group_id
	DELETE FROM PackagesBandwidth WHERE GroupId = @group_id
	DELETE FROM PackagesDiskspace WHERE GroupId = @group_id
	DELETE FROM PackageResources WHERE GroupId = @group_id
	DELETE FROM ResourceGroupDnsRecords WHERE GroupId = @group_id
	DELETE FROM Providers WHERE GroupID = @group_id
	DELETE FROM Quotas WHERE GroupID = @group_id
	DELETE FROM VirtualGroups WHERE GroupID = @group_id
	DELETE FROM ServiceItemTypes WHERE GroupID = @group_id	
	DELETE FROM ResourceGroups WHERE GroupID = @group_id
END

GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceItemTypes] WHERE DisplayName = 'SharePointFoundationSiteCollection')
BEGIN	
	DECLARE @group_id AS INT
	DECLARE @item_type_id INT
	SELECT TOP 1 @item_type_id = ItemTypeId + 1 FROM [dbo].[ServiceItemTypes] ORDER BY ItemTypeId DESC
	UPDATE [dbo].[ServiceItemTypes] SET DisplayName = 'SharePointFoundationSiteCollection' WHERE DisplayName = 'SharePointSiteCollection'
	SELECT @group_id = GroupId FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Server'	

	INSERT INTO [dbo].[ServiceItemTypes] (ItemTypeId, GroupId, DisplayName, TypeName, TypeOrder, CalculateDiskSpace, CalculateBandwidth, Suspendable, Disposable, Searchable, Importable, Backupable) 
		(SELECT TOP 1 @item_type_id, @group_id, 'SharePointSiteCollection', TypeName, 100, CalculateDiskSpace, CalculateBandwidth, Suspendable, Disposable, Searchable, Importable, Backupable FROM [dbo].[ServiceItemTypes] WHERE DisplayName = 'SharePointFoundationSiteCollection')
END




GO

UPDATE [dbo].[Quotas] SET GroupID = 45 WHERE QuotaName = 'EnterpriseStorage.DriveMaps'
GO


UPDATE [dbo].[ResourceGroups] SET GroupName = 'Sharepoint Enterprise Server' WHERE GroupName = 'Sharepoint Server'
GO

UPDATE [dbo].[ResourceGroups] SET GroupController = 'SolidCP.EnterpriseServer.HostedSharePointServerEntController' WHERE GroupName = 'Sharepoint Enterprise Server'
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted SharePoint Enterprise 2013')
BEGIN
DECLARE @group_id AS INT
SELECT @group_id = GroupId FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Enterprise Server'
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1552, @group_id, N'HostedSharePoint2013Ent', N'Hosted SharePoint Enterprise 2013', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2013Ent, SolidCP.Providers.HostedSolution.SharePoint2013Ent', N'HostedSharePoint30', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted SharePoint Enterprise 2013'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted SharePoint Enterprise 2016')
BEGIN
DECLARE @group_id AS INT
SELECT @group_id = GroupId FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Enterprise Server'
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1702, @group_id, N'HostedSharePoint2016Ent', N'Hosted SharePoint Enterprise 2016', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2016Ent, SolidCP.Providers.HostedSolution.SharePoint2016Ent', N'HostedSharePoint30', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted SharePoint Enterprise 2016'
END
GO

UPDATE [dbo].[Quotas] SET QuotaName = 'HostedSharePointEnterprise.Sites' WHERE QuotaId = 550
GO

UPDATE [dbo].[Quotas] SET QuotaName = 'HostedSharePointEnterprise.MaxStorage' WHERE QuotaId = 551
GO

UPDATE [dbo].[Quotas] SET QuotaName = 'HostedSharePointEnterprise.UseSharedSSL' WHERE QuotaId = 552
GO

UPDATE [dbo].[ServiceItemTypes] SET DisplayName = 'SharePointEnterpriseSiteCollection' WHERE DisplayName = 'SharePointSiteCollection'
GO

		
ALTER PROCEDURE [dbo].[AddServiceItem]
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
GO

UPDATE [dbo].[ServiceItemTypes] SET TypeName ='SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection, SolidCP.Providers.Base' WHERE DisplayName = 'SharePointEnterpriseSiteCollection'
GO

-- USER PASSWORD EXPIRATION NOTIFICATION tasks

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = N'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION')
BEGIN
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION', N'SolidCP.EnterpriseServer.UserPasswordExpirationNotificationTask, SolidCP.EnterpriseServer.Code', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskViewConfiguration] WHERE [TaskID] = N'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION')
BEGIN
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/UserPasswordExpirationNotificationView.ascx')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION' AND [ParameterID]= N'DAYS_BEFORE_EXPIRATION' )
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION', N'DAYS_BEFORE_EXPIRATION', N'String', NULL, 1)
END
GO


-- USER PASSWORD EXPIRATION EMAIL TEMPLATE

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordExpirationLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordExpirationLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @UserPasswordExpirationLetterHtmlBody nvarchar(2500)

Set @UserPasswordExpirationLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Password expiration notification</title>
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
<div class="Header">
<img src="#logoUrl#">
</div>
<h1>Password expiration notification</h1>

<ad:if test="#user#">
<p>
Hello #user.FirstName#,
</p>
</ad:if>

<p>
Your password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:
</p>

<a href="#passwordResetLink#" target="_blank">#passwordResetLink#</a>


<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards
</p>
</div>
</body>';

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordExpirationLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordExpirationLetter', N'HtmlBody', @UserPasswordExpirationLetterHtmlBody)
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordExpirationLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordExpirationLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordExpirationLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordExpirationLetter', N'Subject', N'Password expiration notification')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordExpirationLetter' AND [PropertyName]= N'LogoUrl' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordExpirationLetter', N'LogoUrl', N'')
END
GO


DECLARE @UserPasswordExpirationLetterTextBody nvarchar(2500)

Set @UserPasswordExpirationLetterTextBody = N'=========================================
   Password expiration notification
=========================================

<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

Your password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:

#passwordResetLink#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordExpirationLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordExpirationLetter', N'TextBody', @UserPasswordExpirationLetterTextBody)
END
GO


-- USER PASSWORD RESET EMAIL TEMPLATE



IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @UserPasswordResetLetterHtmlBody nvarchar(2500)

Set @UserPasswordResetLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Password reset notification</title>
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
<div class="Header">
<img src="#logoUrl#">
</div>
<h1>Password reset notification</h1>

<ad:if test="#user#">
<p>
Hello #user.FirstName#,
</p>
</ad:if>

<p>
We received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.
</p>

<a href="#passwordResetLink#" target="_blank">#passwordResetLink#</a>


<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards
</p>
</div>
</body>';

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetLetter', N'HtmlBody', @UserPasswordResetLetterHtmlBody)
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetLetter', N'Subject', N'Password reset notification')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetLetter' AND [PropertyName]= N'LogoUrl' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetLetter', N'LogoUrl', N'')
END
GO


DECLARE @UserPasswordResetLetterTextBody nvarchar(2500)

Set @UserPasswordResetLetterTextBody = N'=========================================
   Password reset notification
=========================================

<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

We received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.

#passwordResetLink#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetLetter', N'TextBody', @UserPasswordResetLetterTextBody)
END
GO




DECLARE @UserPasswordResetSMSBody nvarchar(2500)

Set @UserPasswordResetSMSBody = N'Password reset link:
#passwordResetLink#
'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetLetter' AND [PropertyName]= N'PasswordResetLinkSmsBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetLetter', N'PasswordResetLinkSmsBody', @UserPasswordResetSMSBody)
END
GO

-- USER PASSWORD RESET EMAIL PINCODE TEMPLATE

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetPincodeLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetPincodeLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @UserPasswordResetPincodeLetterHtmlBody nvarchar(2500)

Set @UserPasswordResetPincodeLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Password reset notification</title>
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
<div class="Header">
<img src="#logoUrl#">
</div>
<h1>Password reset notification</h1>

<ad:if test="#user#">
<p>
Hello #user.FirstName#,
</p>
</ad:if>

<p>
We received a request to reset the password for your account. Your password reset pincode:
</p>

#passwordResetPincode#

<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards
</p>
</div>
</body>';

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetPincodeLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetPincodeLetter', N'HtmlBody', @UserPasswordResetPincodeLetterHtmlBody)
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetPincodeLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetPincodeLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetPincodeLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetPincodeLetter', N'Subject', N'Password reset notification')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetPincodeLetter' AND [PropertyName]= N'LogoUrl' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetPincodeLetter', N'LogoUrl', N'')
END
GO


DECLARE @UserPasswordResetPincodeLetterTextBody nvarchar(2500)

Set @UserPasswordResetPincodeLetterTextBody = N'=========================================
   Password reset notification
=========================================

<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

We received a request to reset the password for your account. Your password reset pincode:

#passwordResetPincode#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetPincodeLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetPincodeLetter', N'TextBody', @UserPasswordResetPincodeLetterTextBody)
END
GO

DECLARE @UserPasswordPincodeSMSBody nvarchar(2500)

Set @UserPasswordPincodeSMSBody = N'
Your password reset pincode:
#passwordResetPincode#'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'UserPasswordResetPincodeLetter' AND [PropertyName]= N'PasswordResetPincodeSmsBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'UserPasswordResetPincodeLetter', N'PasswordResetPincodeSmsBody', @UserPasswordPincodeSMSBody)
END
GO


-- USER PASSWORD REQUEST EMAIL TEMPLATE

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'OrganizationUserPasswordRequestLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OrganizationUserPasswordRequestLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @OrganizationUserPasswordRequestLetterHtmlBody nvarchar(2500)

Set @OrganizationUserPasswordRequestLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Password request notification</title>
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
<div class="Header">
<img src="#logoUrl#">
</div>
<h1>Password request notification</h1>

<ad:if test="#user#">
<p>
Hello #user.FirstName#,
</p>
</ad:if>

<p>
Your account have been created. In order to create a password for your account, please follow next link:
</p>

<a href="#passwordResetLink#" target="_blank">#passwordResetLink#</a>

<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards
</p>
</div>
</body>';

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'OrganizationUserPasswordRequestLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OrganizationUserPasswordRequestLetter', N'HtmlBody', @OrganizationUserPasswordRequestLetterHtmlBody)
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'OrganizationUserPasswordRequestLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OrganizationUserPasswordRequestLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'OrganizationUserPasswordRequestLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OrganizationUserPasswordRequestLetter', N'Subject', N'Password request notification')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'OrganizationUserPasswordRequestLetter' AND [PropertyName]= N'LogoUrl' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OrganizationUserPasswordRequestLetter', N'LogoUrl', N'')
END
GO


DECLARE @OrganizationUserPasswordRequestLetterTextBody nvarchar(2500)

Set @OrganizationUserPasswordRequestLetterTextBody = N'=========================================
   Password request notification
=========================================

<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

Your account have been created. In order to create a password for your account, please follow next link:

#passwordResetLink#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'OrganizationUserPasswordRequestLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OrganizationUserPasswordRequestLetter', N'TextBody', @OrganizationUserPasswordRequestLetterTextBody)
END
GO

DECLARE @OrganizationUserPasswordRequestLetterSMSBody nvarchar(2500)

Set @OrganizationUserPasswordRequestLetterSMSBody = N'
User have been created. Password request url:
#passwordResetLink#'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'OrganizationUserPasswordRequestLetter' AND [PropertyName]= N'SMSBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OrganizationUserPasswordRequestLetter', N'SMSBody', @OrganizationUserPasswordRequestLetterSMSBody)
END
GO


-- Exchange setup EMAIL TEMPLATE
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'ExchangeMailboxSetupLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @ExchangeMailboxSetupLetterHtmlBody nvarchar(max)

Set @ExchangeMailboxSetupLetterHtmlBody = (SELECT PropertyValue FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'HtmlBody')

IF (@ExchangeMailboxSetupLetterHtmlBody = '' OR @ExchangeMailboxSetupLetterHtmlBody IS NULL)
BEGIN
Set @ExchangeMailboxSetupLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Account Summary Information</title>
    <style type="text/css">
        body {font-family: ''Segoe UI Light'',''Open Sans'',Arial!important;color:black;}
        p {color:black;}
		.Summary { background-color: ##ffffff; padding: 5px; }
		.SummaryHeader { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.5em; color: ##1F4978; border-bottom: dotted 3px ##efefef; font-weight:normal; }
        .Summary H2 { font-size: 1.2em; color: ##1F4978; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; color:black;}
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
        .Label { color:##1F4978; }
        .menu-bar a {padding: 15px 0;display: inline-block;}
    </style>
</head>
<body>
<table border="0" cellspacing="0" cellpadding="0" width="100%"><!-- was 800 -->
<tbody>
<tr>
<td style="padding: 10px 20px 10px 20px; background-color: ##e1e1e1;">
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="text-align: left; padding: 0px 0px 2px 0px;"><a href=""><img src="" border="0" alt="" /></a></td>
</tr>
</tbody>
</table>
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="padding-bottom: 10px;">
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="background-color: ##2e8bcc; padding: 3px;">
<table class="menu-bar" border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""</a></td>
<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>
<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>
<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>
<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="background-color: ##ffffff;">
<table border="0" cellspacing="0" cellpadding="0" width="100%"><!-- was 759 -->
<tbody>
<tr>
<td style="vertical-align: top; padding: 10px 10px 0px 10px;" width="100%">
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="font-family: ''Segoe UI Light'',''Open Sans'',Arial; padding: 0px 10px 0px 0px;">
<!-- Begin Content -->
<div class="Summary">
    <ad:if test="#Email#">
    <p>
    Hello #Account.DisplayName#,
    </p>
    <p>
    Thanks for choosing as your Exchange hosting provider.
    </p>
    </ad:if>
    <ad:if test="#not(PMM)#">
    <h1>User Accounts</h1>
    <p>
    The following user accounts have been created for you.
    </p>
    <table>
        <tr>
            <td class="Label">Username:</td>
            <td>#Account.UserPrincipalName#</td>
        </tr>
        <tr>
            <td class="Label">E-mail:</td>
            <td>#Account.PrimaryEmailAddress#</td>
        </tr>
		<ad:if test="#PswResetUrl#">
        <tr>
            <td class="Label">Password Reset Url:</td>
            <td><a href="#PswResetUrl#" target="_blank">Click here</a></td>
        </tr>
		</ad:if>
    </table>
    </ad:if>
    <h1>DNS</h1>
    <p>
    In order for us to accept mail for your domain, you will need to point your MX records to:
    </p>
    <table>
        <ad:foreach collection="#SmtpServers#" var="SmtpServer" index="i">
            <tr>
                <td class="Label">#SmtpServer#</td>
            </tr>
        </ad:foreach>
    </table>
   <h1>
    Webmail (OWA, Outlook Web Access)</h1>
    <p>
    <a href="" target="_blank"></a>
    </p>
    <h1>
    Outlook (Windows Clients)</h1>
    <p>
    To configure MS Outlook to work with the servers, please reference:
    </p>
    <p>
    <a href="" target="_blank"></a>
    </p>
    <p>
    If you need to download and install the Outlook client:</p>
        
        <table>
            <tr><td colspan="2" class="Label"><font size="3">MS Outlook Client</font></td></tr>
            <tr>
                <td class="Label">
                    Download URL:</td>
                <td><a href=""></a></td>
            </tr>
<tr>
                <td class="Label"></td>
                <td><a href=""></a></td>
            </tr>
            <tr>
                <td class="Label">
                    KEY:</td>
                <td></td>
            </tr>
        </table>
 
       <h1>
    ActiveSync, iPhone, iPad</h1>
    <table>
        <tr>
            <td class="Label">Server:</td>
            <td>#ActiveSyncServer#</td>
        </tr>
        <tr>
            <td class="Label">Domain:</td>
            <td>#SamDomain#</td>
        </tr>
        <tr>
            <td class="Label">SSL:</td>
            <td>must be checked</td>
        </tr>
        <tr>
            <td class="Label">Your username:</td>
            <td>#SamUsername#</td>
        </tr>
    </table>
 
    <h1>Password Changes</h1>
    <p>
    Passwords can be changed at any time using Webmail or the <a href="" target="_blank">Control Panel</a>.</p>
    <h1>Control Panel</h1>
    <p>
    If you need to change the details of your account, you can easily do this using <a href="" target="_blank">Control Panel</a>.</p>
    <h1>Support</h1>
    <p>
    You have 2 options, email <a href="mailto:"></a> or use the web interface at <a href=""></a></p>
    
</div>
<!-- End Content -->
<br></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style="background-color: ##ffffff; border-top: 1px solid ##999999;">
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="vertical-align: top; padding: 0px 20px 15px 20px;">
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="font-family: Arial, Helvetica, sans-serif; text-align: left; font-size: 9px; color: ##717073; padding: 20px 0px 0px 0px;">
<table border="0" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr>
<td style="font-family: Arial, Helvetica, sans-serif; font-size: 9px; text-align: left; color: ##1666af; vertical-align: top;" width="33%"><a style="font-weight: bold; text-transform: uppercase; text-decoration: underline; color: ##1666af;" href=""></a><br />Learn more about the services can provide to improve your business.</td>
<td style="font-family: Arial, Helvetica, sans-serif; font-size: 9px; text-align: left; color: ##1666af; padding: 0px 10px 0px 10px; vertical-align: top;" width="34%"><a style="font-weight: bold; text-transform: uppercase; text-decoration: underline; color: ##1666af;" href="">Privacy Policy</a><br /> follows strict guidelines in protecting your privacy. Learn about our <a style="font-weight: bold; text-decoration: underline; color: ##1666af;" href="">Privacy Policy</a>.</td>
<td style="font-family: Arial, Helvetica, sans-serif; font-size: 9px; text-align: left; color: ##1666af; vertical-align: top;" width="33%"><a style="font-weight: bold; text-transform: uppercase; text-decoration: underline; color: ##1666af;" href="">Contact Us</a><br />Questions? For more information, <a style="font-weight: bold; text-decoration: underline; color: ##1666af;" href="">contact us</a>.</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
</body>
</html>';
END

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'ExchangeMailboxSetupLetter', N'HtmlBody', @ExchangeMailboxSetupLetterHtmlBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @ExchangeMailboxSetupLetterHtmlBody WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'HtmlBody'
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'ExchangeMailboxSetupLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'ExchangeMailboxSetupLetter', N'Subject', N' Hosted Exchange Mailbox Setup')
END
GO


DECLARE @ExchangeMailboxSetupLetterTextBody nvarchar(2500)
Set @ExchangeMailboxSetupLetterTextBody = N'<ad:if test="#Email#">
Hello #Account.DisplayName#,

Thanks for choosing as your Exchange hosting provider.
</ad:if>
<ad:if test="#not(PMM)#">
User Accounts

The following user accounts have been created for you.

Username: #Account.UserPrincipalName#
E-mail: #Account.PrimaryEmailAddress#
<ad:if test="#PswResetUrl#">
Password Reset Url: #PswResetUrl#
</ad:if>
</ad:if>

=================================
DNS
=================================

In order for us to accept mail for your domain, you will need to point your MX records to:

<ad:foreach collection="#SmtpServers#" var="SmtpServer" index="i">#SmtpServer#</ad:foreach>

=================================
Webmail (OWA, Outlook Web Access)
=================================



=================================
Outlook (Windows Clients)
=================================

To configure MS Outlook to work with servers, please reference:



If you need to download and install the MS Outlook client:

MS Outlook Download URL:

KEY: 

=================================
ActiveSync, iPhone, iPad
=================================

Server: #ActiveSyncServer#
Domain: #SamDomain#
SSL: must be checked
Your username: #SamUsername#

=================================
Password Changes
=================================

Passwords can be changed at any time using Webmail or the Control Panel


=================================
Control Panel
=================================

If you need to change the details of your account, you can easily do this using the Control Panel 


=================================
Support
=================================

You have 2 options, email or use the web interface at '
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'ExchangeMailboxSetupLetter', N'TextBody', @ExchangeMailboxSetupLetterTextBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @ExchangeMailboxSetupLetterTextBody WHERE [UserID] = 1 AND [SettingsName]= N'ExchangeMailboxSetupLetter' AND [PropertyName]= N'TextBody'
GO



-- ORGANIZATION USER PASSWORD RESET TOKENS


IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'AccessTokens')
DROP TABLE AccessTokens
GO
CREATE TABLE AccessTokens
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	AccessTokenGuid UNIQUEIDENTIFIER NOT NULL,
	ExpirationDate DATETIME NOT NULL,
	AccountID INT NOT NULL ,
	ItemId INT NOT NULL,
	TokenType INT NOT NULL,
	SmsResponse varchar(100)
)
GO

ALTER TABLE [dbo].[AccessTokens]  WITH CHECK ADD  CONSTRAINT [FK_AccessTokens_UserId] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddAccessToken')
DROP PROCEDURE AddAccessToken
GO
CREATE PROCEDURE [dbo].[AddAccessToken]
(
	@TokenID INT OUTPUT,
	@AccessToken UNIQUEIDENTIFIER,
	@ExpirationDate DATETIME,
	@AccountID INT,
	@ItemId INT,
	@TokenType INT
)
AS
INSERT INTO AccessTokens
(
	AccessTokenGuid,
	ExpirationDate,
	AccountID  ,
	ItemId,
	TokenType
)
VALUES
(
	@AccessToken  ,
	@ExpirationDate ,
	@AccountID,
	@ItemId,
	@TokenType
)

SET @TokenID = SCOPE_IDENTITY()

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'SetAccessTokenSmsResponse')
DROP PROCEDURE SetAccessTokenSmsResponse
GO
CREATE PROCEDURE [dbo].[SetAccessTokenSmsResponse]
(
	@AccessToken UNIQUEIDENTIFIER,
	@SmsResponse varchar(100)
)
AS
UPDATE [dbo].[AccessTokens] SET [SmsResponse] = @SmsResponse WHERE [AccessTokenGuid] = @AccessToken
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteAccessToken')
DROP PROCEDURE DeleteAccessToken
GO
CREATE PROCEDURE [dbo].[DeleteAccessToken]
(
	@AccessToken UNIQUEIDENTIFIER,
	@TokenType INT
)
AS
DELETE FROM AccessTokens
WHERE AccessTokenGuid = @AccessToken AND TokenType = @TokenType
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteExpiredAccessTokenTokens')
DROP PROCEDURE DeleteExpiredAccessTokenTokens
GO
CREATE PROCEDURE [dbo].[DeleteExpiredAccessTokenTokens]
AS
DELETE FROM AccessTokens
WHERE ExpirationDate < getdate()
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetAccessTokenByAccessToken')
DROP PROCEDURE GetAccessTokenByAccessToken
GO
CREATE PROCEDURE [dbo].[GetAccessTokenByAccessToken]
(
	@AccessToken UNIQUEIDENTIFIER,
	@TokenType INT
)
AS
SELECT 
	ID ,
	AccessTokenGuid,
	ExpirationDate,
	AccountID,
	ItemId,
	TokenType,
	SmsResponse
	FROM AccessTokens 
	Where AccessTokenGuid = @AccessToken AND ExpirationDate > getdate() AND TokenType = @TokenType
GO


-- ORGANIZATION SETTINGS


IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ExchangeOrganizationSettings')
BEGIN
	CREATE TABLE ExchangeOrganizationSettings
	(
		ItemId INT NOT NULL,
		SettingsName nvarchar(100)  NOT NULL,
		Xml nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ExchangeOrganizationSettings] PRIMARY KEY ([ItemId], [SettingsName]),
        CONSTRAINT [FK_ExchangeOrganizationSettings_ExchangeOrganizations_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [ExchangeOrganizations] ([ItemID]) ON DELETE CASCADE
	);
END




IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateExchangeOrganizationSettings')
DROP PROCEDURE UpdateExchangeOrganizationSettings
GO
CREATE PROCEDURE [dbo].[UpdateExchangeOrganizationSettings]
(
	@ItemId INT ,
	@SettingsName nvarchar(100) ,
	@Xml nvarchar(max)
)
AS
IF NOT EXISTS (SELECT * FROM [dbo].[ExchangeOrganizationSettings] WHERE [ItemId] = @ItemId AND [SettingsName]= @SettingsName )
BEGIN
INSERT [dbo].[ExchangeOrganizationSettings] ([ItemId], [SettingsName], [Xml]) VALUES (@ItemId, @SettingsName, @Xml)
END
ELSE
UPDATE [dbo].[ExchangeOrganizationSettings] SET [Xml] = @Xml WHERE [ItemId] = @ItemId AND [SettingsName]= @SettingsName
GO




IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetExchangeOrganizationSettings')
DROP PROCEDURE GetExchangeOrganizationSettings
GO
CREATE PROCEDURE [dbo].[GetExchangeOrganizationSettings]
(
	@ItemId INT ,
	@SettingsName nvarchar(100)
)
AS
SELECT 
	ItemId,
	SettingsName,
	Xml

FROM ExchangeOrganizationSettings 
Where ItemId = @ItemId AND SettingsName = @SettingsName
GO


-- Exchange Account password column removed

if exists(select * from sys.columns 
            where Name = N'AccountPassword' and Object_ID = Object_ID(N'ExchangeAccounts'))
begin
  ALTER TABLE [ExchangeAccounts] DROP COLUMN [AccountPassword]
end

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddExchangeAccount')
DROP PROCEDURE AddExchangeAccount
GO
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



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'SearchExchangeAccount')
DROP PROCEDURE SearchExchangeAccount
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

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackagePrivateIPAddressesPaged')
DROP PROCEDURE GetPackagePrivateIPAddressesPaged
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

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (IPAddress LIKE ''' + @FilterValue + '''
			OR ItemName LIKE ''' + @FilterValue + ''')'
END

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

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetNestedPackagesPaged')
DROP PROCEDURE GetNestedPackagesPaged
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
BEGIN

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
ELSE
SET @sql = @sql + ' ORDER BY P.PackageName '

SET @sql = @sql + ' SELECT COUNT(PackageID) FROM @Packages;
SELECT
	P.PackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,    
  	P.StatusIDchangeDate,
	
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
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetUsersPaged')
DROP PROCEDURE GetUsersPaged
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetServiceItemsPaged')
DROP PROCEDURE GetServiceItemsPaged
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

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (ItemName LIKE ''' + @FilterValue + '''
			OR Username LIKE ''' + @FilterValue + '''
			OR FullName LIKE ''' + @FilterValue + '''
			OR Email LIKE ''' + @FilterValue + ''')'
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
GO


IF EXISTS (SELECT TOP 1 * FROM ServiceItemTypes WHERE DisplayName = 'SharePointEnterpriseSiteCollection')
BEGIN
	DECLARE @item_type_id AS INT
	SELECT @item_type_id = ItemTypeId FROM ServiceItemTypes WHERE DisplayName = 'SharePointEnterpriseSiteCollection'
	UPDATE [dbo].[Quotas] SET ItemTypeID = @item_type_id WHERE QuotaId = 550
END
GO

-- OneTimePassword
IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='Users' AND COLS.name='OneTimePasswordState')
BEGIN
ALTER TABLE [dbo].[Users] ADD
	[OneTimePasswordState] int NULL
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'SetUserOneTimePassword')
DROP PROCEDURE SetUserOneTimePassword
GO
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
GO


ALTER PROCEDURE [dbo].[ChangeUserPassword]
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
GO

-- HyperV for config file
alter table ServiceProperties 
alter column PropertyValue nvarchar(MAX) NULL
Go





ALTER PROCEDURE [dbo].[UpdateServiceProperties]
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
Go

-- Storage Spaces

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'StorageSpaceServices')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (49, N'StorageSpaceServices', 26, N'SolidCP.EnterpriseServer.StorageSpacesController', 1)
END
ELSE
BEGIN
UPDATE [dbo].[ResourceGroups] SET [GroupController] = N'SolidCP.EnterpriseServer.StorageSpacesController' WHERE [GroupName] = 'StorageSpaceServices'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Storage Spaces Windows 2012')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(700, 49, N'StorageSpace2012', N'Storage Spaces Windows 2012', N'SolidCP.Providers.StorageSpaces.Windows2012, SolidCP.Providers.StorageSpaces.Windows2012', N'StorageSpaceServices',	1)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Storage Spaces Windows 2012'
END
GO


-- STORAGE SPACE LEVELS

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'StorageSpaceLevels')
BEGIN
	CREATE TABLE StorageSpaceLevels
	(
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Name nvarchar(300) NOT NULL,
		Description nvarchar(max) NOT NULL
	)
END


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetStorageSpaceLevelsPaged')
DROP PROCEDURE GetStorageSpaceLevelsPaged
GO
CREATE PROCEDURE [dbo].[GetStorageSpaceLevelsPaged]
(
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

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @SSLevels TABLE
(
	ItemPosition int IDENTITY(0,1),
	SSLevelId int
)
INSERT INTO @SSLevels (SSLevelId)
SELECT
	S.ID
FROM StorageSpaceLevels AS S'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' WHERE ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(SSLevelId) FROM @SSLevels;
SELECT
	CR.ID,
	CR.Name,
	CR.Description
FROM @SSLevels AS C
INNER JOIN StorageSpaceLevels AS CR ON C.SSLevelId = CR.ID
WHERE C.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int,  @FilterValue nvarchar(50)',
@StartRow, @MaximumRows,  @FilterValue


RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS  WHERE type = 'P' AND name = 'GetStorageSpaceLevelById')
DROP PROCEDURE GetStorageSpaceLevelById
GO
CREATE PROCEDURE GetStorageSpaceLevelById 
(
@ID INT
)
AS
SELECT TOP 1
	SL.Id,
	Sl.Name,
	SL.Description
FROM StorageSpaceLevels AS SL
WHERE SL.Id = @ID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type ='P' AND name ='UpdateStorageSpaceLevel')
DROP PROCEDURE UpdateStorageSpaceLevel
GO
CREATE PROCEDURE UpdateStorageSpaceLevel
(
	@ID INT,
	@Name nvarchar(300),
	@Description nvarchar(max)
)
AS
	UPDATE StorageSpaceLevels
	SET Name = @Name, Description = @Description
	WHERE ID = @ID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type ='P' AND name ='InsertStorageSpaceLevel')
	DROP PROCEDURE InsertStorageSpaceLevel
GO
CREATE PROCEDURE InsertStorageSpaceLevel
(
	@ID INT OUTPUT,
	@Name nvarchar(300),
	@Description nvarchar(max)
)
AS

INSERT INTO StorageSpaceLevels 
(
	Name, 
	Description
)
VALUES 
(
	@Name,
	@Description
)

SET @ID = SCOPE_IDENTITY()

RETURN

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type ='P' AND name = 'RemoveStorageSpaceLevel')
	DROP PROCEDURE RemoveStorageSpaceLevel
GO
CREATE PROCEDURE RemoveStorageSpaceLevel
(
	@ID INT
)
AS
	DELETE FROM StorageSpaceLevels WHERE ID = @ID
GO


--STORAGE SPACE LEVEL RESOURCE GROUPS

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'StorageSpaceLevelResourceGroups')
BEGIN
	CREATE TABLE StorageSpaceLevelResourceGroups
	(
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		LevelId INT NOT NULL,
		GroupId INT NOT NULL
	)
END


IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_StorageSpaceLevelResourceGroups_LevelId')
BEGIN
	ALTER TABLE [dbo].[StorageSpaceLevelResourceGroups]
	DROP CONSTRAINT [FK_StorageSpaceLevelResourceGroups_LevelId]
END	

ALTER TABLE [dbo].[StorageSpaceLevelResourceGroups]  WITH CHECK ADD  CONSTRAINT [FK_StorageSpaceLevelResourceGroups_LevelId] FOREIGN KEY([LevelId])
REFERENCES [dbo].[StorageSpaceLevels] ([Id])
ON DELETE CASCADE
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_StorageSpaceLevelResourceGroups_GroupId')
BEGIN
	ALTER TABLE [dbo].[StorageSpaceLevelResourceGroups]
	DROP CONSTRAINT [FK_StorageSpaceLevelResourceGroups_GroupId]
END	

ALTER TABLE [dbo].[StorageSpaceLevelResourceGroups]  WITH CHECK ADD  CONSTRAINT [FK_StorageSpaceLevelResourceGroups_GroupId] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupId])
ON DELETE CASCADE
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type='P' AND name='GetLevelResourceGroups')
	DROP PROCEDURE GetLevelResourceGroups
GO

CREATE PROCEDURE GetLevelResourceGroups
(
	@LevelId INT
)
AS
	SELECT 
	G.[GroupID],
	G.[GroupName],
	G.[GroupOrder],
	G.[GroupController],
	G.[ShowGroup]
	FROM [dbo].[StorageSpaceLevelResourceGroups] AS SG
	INNER JOIN [dbo].[ResourceGroups] AS G
	ON SG.GroupId = G.GroupId
	WHERE SG.LevelId = @LevelId
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type='P' AND name='DeleteLevelResourceGroups')
	DROP PROCEDURE DeleteLevelResourceGroups
GO

CREATE PROCEDURE DeleteLevelResourceGroups
(
	@LevelId INT
)
AS
	DELETE 
	FROM [dbo].[StorageSpaceLevelResourceGroups]
	WHERE LevelId = @LevelId
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type='P' AND name='AddLevelResourceGroups')
	DROP PROCEDURE AddLevelResourceGroups
GO

CREATE PROCEDURE AddLevelResourceGroups
(
	@LevelId INT,
	@GroupId INT
)
AS
	INSERT INTO [dbo].[StorageSpaceLevelResourceGroups] (LevelId, GroupId)
	VALUES (@LevelId, @GroupId)
GO


--STORAGE SPACES


IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'StorageSpaces')
BEGIN
	CREATE TABLE StorageSpaces
	(
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Name varchar(300) NOT NULL,
		ServiceId INT NOT NULL,
		ServerId INT NOT NULL,
		LevelId INT NOT NULL,
		Path varchar(max) NOT NULL,
		IsShared BIT NOT NULL,
		UncPath varchar(max),
		FsrmQuotaType INT NOT NULL,
		FsrmQuotaSizeBytes BIGINT NOT NULL
	)
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'StorageSpaces' AND COLUMN_NAME = 'IsDisabled')
BEGIN
	ALTER TABLE [dbo].[StorageSpaces]
		ADD IsDisabled BIT NOT NULL DEFAULT(0)
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_StorageSpaces_ServiceId')
BEGIN
	ALTER TABLE [dbo].[StorageSpaces]
	DROP CONSTRAINT [FK_StorageSpaces_ServiceId]
END	

ALTER TABLE [dbo].[StorageSpaces]  WITH CHECK ADD  CONSTRAINT [FK_StorageSpaces_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_StorageSpaces_ServerId')
BEGIN
	ALTER TABLE [dbo].[StorageSpaces]
	DROP CONSTRAINT [FK_StorageSpaces_ServerId]
END	

ALTER TABLE [dbo].[StorageSpaces]  WITH CHECK ADD  CONSTRAINT [FK_StorageSpaces_ServerId] FOREIGN KEY([ServerId])
REFERENCES [dbo].[Servers] ([ServerID])
ON DELETE CASCADE
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetStorageSpacesPaged')
DROP PROCEDURE GetStorageSpacesPaged
GO
CREATE PROCEDURE [dbo].[GetStorageSpacesPaged]
(
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2500)

SET @sql = '

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Spaces TABLE
(
	ItemPosition int IDENTITY(0,1),
	SpaceId int
)
INSERT INTO @Spaces (SpaceId)
SELECT
	S.Id
FROM StorageSpaces AS S'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' WHERE ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(SpaceId) FROM @Spaces;
SELECT
		CR.Id,
		CR.Name ,
		CR.ServiceId ,
		CR.ServerId ,
		CR.LevelId,
		CR.Path,
		CR.FsrmQuotaType,
		CR.FsrmQuotaSizeBytes,
		CR.IsShared,
		CR.IsDisabled,
		CR.UncPath,
		ISNULL((SELECT SUM(SSF.FsrmQuotaSizeBytes) FROM StorageSpaceFolders AS SSF WHERE SSF.StorageSpaceId = CR.Id), 0) UsedSizeBytes
FROM @Spaces AS C
INNER JOIN StorageSpaces AS CR ON C.SpaceId = CR.Id
WHERE C.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int,  @FilterValue nvarchar(50)',
@StartRow, @MaximumRows,  @FilterValue


RETURN
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS  WHERE type = 'P' AND name = 'GetStorageSpacesByLevelId')
DROP PROCEDURE GetStorageSpacesByLevelId
GO
CREATE PROCEDURE GetStorageSpacesByLevelId 
(
	@LevelId INT
)
AS
SELECT
		SS.Id,
		SS.Name ,
		SS.ServiceId ,
		SS.ServerId ,
		SS.LevelId,
		SS.Path,
		SS.FsrmQuotaType,
		SS.FsrmQuotaSizeBytes,
		SS.IsShared,
		SS.UncPath,
		SS.IsDisabled,
		ISNULL((SELECT SUM(SSF.FsrmQuotaSizeBytes) FROM StorageSpaceFolders AS SSF WHERE SSF.StorageSpaceId = SS.Id), 0) UsedSizeBytes
FROM StorageSpaces AS SS
INNER JOIN StorageSpaceLevels AS SSL
ON SSL.Id = SS.LevelId
WHERE SS.LevelId = @LevelId
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type='P' AND name='GetStorageSpaceById')
	DROP PROCEDURE GetStorageSpaceById
GO

CREATE PROCEDURE GetStorageSpaceById
(
	@Id INT
)
AS
	SELECT TOP 1
		SS.Id,
		SS.Name ,
		SS.ServiceId ,
		SS.ServerId ,
		SS.LevelId,
		SS.Path,
		SS.FsrmQuotaType,
		SS.FsrmQuotaSizeBytes,
		SS.IsShared,
		SS.UncPath,
		SS.IsDisabled,
		ISNULL((SELECT SUM(SSF.FsrmQuotaSizeBytes) FROM StorageSpaceFolders AS SSF WHERE SSF.StorageSpaceId = SS.Id), 0) UsedSizeBytes
	FROM [dbo].[StorageSpaces] AS SS
	WHERE SS.Id = @Id
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type ='P' AND name ='UpdateStorageSpace')
DROP PROCEDURE UpdateStorageSpace
GO
CREATE PROCEDURE UpdateStorageSpace
(
	@ID INT,
	@Name nvarchar(300),
	@ServiceId INT ,
	@ServerId INT,
	@LevelId INT,
	@Path varchar(max),
	@FsrmQuotaType INT,
	@FsrmQuotaSizeBytes BIGINT,
	@IsShared BIT,
	@IsDisabled BIT,
	@UncPath varchar(max)
)
AS
	UPDATE StorageSpaces
	SET Name = @Name, ServiceId = @ServiceId,ServerId=@ServerId,LevelId=@LevelId, Path=@Path,FsrmQuotaType=@FsrmQuotaType,FsrmQuotaSizeBytes=@FsrmQuotaSizeBytes,IsShared=@IsShared,UncPath=@UncPath,IsDisabled=@IsDisabled
	WHERE ID = @ID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type ='P' AND name ='InsertStorageSpace')
	DROP PROCEDURE InsertStorageSpace
GO
CREATE PROCEDURE InsertStorageSpace
(
	@ID INT OUTPUT,
	@Name nvarchar(300),
	@ServiceId INT ,
	@ServerId INT,
	@LevelId INT,
	@Path varchar(max),
	@FsrmQuotaType INT,
	@FsrmQuotaSizeBytes BIGINT,
	@IsShared BIT,
	@IsDisabled BIT,
	@UncPath varchar(max)
)
AS

INSERT INTO StorageSpaces 
(
	Name,
	ServiceId,
	ServerId,
	LevelId,
	Path,
	FsrmQuotaType,
	FsrmQuotaSizeBytes,
	IsShared,
	UncPath,
	IsDisabled
)
VALUES 
(
	@Name,
	@ServiceId,
	@ServerId,
	@LevelId,
	@Path,
	@FsrmQuotaType,
	@FsrmQuotaSizeBytes,
	@IsShared,
	@UncPath,
	@IsDisabled
)

SET @ID = SCOPE_IDENTITY()

RETURN

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type ='P' AND name = 'RemoveStorageSpace')
	DROP PROCEDURE RemoveStorageSpace
GO
CREATE PROCEDURE RemoveStorageSpace
(
	@ID INT
)
AS
	DELETE FROM StorageSpaces WHERE ID = @ID
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS  WHERE type = 'P' AND name = 'GetStorageSpacesByResourceGroupName')
DROP PROCEDURE GetStorageSpacesByResourceGroupName
GO
CREATE PROCEDURE GetStorageSpacesByResourceGroupName 
(
	@ResourceGroupName varchar(max)
)
AS
SELECT
		SS.Id,
		SS.Name ,
		SS.ServiceId ,
		SS.ServerId ,
		SS.LevelId,
		SS.Path,
		SS.FsrmQuotaType,
		SS.FsrmQuotaSizeBytes,
		SS.IsShared,
		SS.UncPath,
		SS.IsDisabled,
		ISNULL((SELECT SUM(SSF.FsrmQuotaSizeBytes) FROM StorageSpaceFolders AS SSF WHERE SSF.StorageSpaceId = SS.Id), 0) UsedSizeBytes
FROM StorageSpaces AS SS
INNER JOIN StorageSpaceLevelResourceGroups AS SSLRG ON SSLRG.LevelId = SS.LevelId
INNER JOIN ResourceGroups AS RG ON SSLRG.GroupID = RG.GroupID
WHERE RG.GroupName = @ResourceGroupName
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS  WHERE type = 'P' AND name = 'GetStorageSpaceByServiceAndPath')
DROP PROCEDURE GetStorageSpaceByServiceAndPath
GO
CREATE PROCEDURE GetStorageSpaceByServiceAndPath 
(
	@ServerId INT,
	@Path varchar(max)
)
AS
SELECT TOP 1
		SS.Id,
		SS.Name ,
		SS.ServiceId ,
		SS.ServerId ,
		SS.LevelId,
		SS.Path,
		SS.FsrmQuotaType,
		SS.FsrmQuotaSizeBytes,
		SS.IsShared,
		SS.UncPath,
		SS.IsDisabled,
		ISNULL((SELECT SUM(SSF.FsrmQuotaSizeBytes) FROM StorageSpaceFolders AS SSF WHERE SSF.StorageSpaceId = SS.Id), 0) UsedSizeBytes
FROM StorageSpaces AS SS
WHERE SS.ServerId = @ServerId AND SS.Path = @Path
GO

-- STORAGE SPACE FOLDER


IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'StorageSpaceFolders')
BEGIN
	CREATE TABLE StorageSpaceFolders
	(
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Name varchar(300) NOT NULL,
		StorageSpaceId INT NOT NULL,
		Path varchar(max) NOT NULL,
		UncPath varchar(max),
		IsShared BIT NOT NULL,
		FsrmQuotaType INT NOT NULL,
		FsrmQuotaSizeBytes BIGINT NOT NULL,
	)
END


IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_StorageSpaceFolders_StorageSpaceId')
BEGIN
	ALTER TABLE [dbo].[StorageSpaceFolders]
	DROP CONSTRAINT [FK_StorageSpaceFolders_StorageSpaceId]
END	

ALTER TABLE [dbo].[StorageSpaceFolders]  WITH CHECK ADD  CONSTRAINT [FK_StorageSpaceFolders_StorageSpaceId] FOREIGN KEY([StorageSpaceId])
REFERENCES [dbo].[StorageSpaces] ([ID])
ON DELETE CASCADE
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name ='CreateStorageSpaceFolder')
	DROP PROCEDURE CreateStorageSpaceFolder
GO

CREATE PROCEDURE CreateStorageSpaceFolder
(
	@ID INT OUTPUT,
	@Name varchar(300),
	@StorageSpaceId INT,
	@Path varchar(max),
	@UncPath varchar(max),
	@IsShared BIT,
	@FsrmQuotaType INT,
	@FsrmQuotaSizeBytes BIGINT 
)
AS
INSERT INTO StorageSpaceFolders (	
	Name,
	StorageSpaceId,
	Path,
	UncPath,
	IsShared,
	FsrmQuotaType,
	FsrmQuotaSizeBytes)
VALUES (
	@Name,
	@StorageSpaceId,
	@Path,
	@UncPath,
	@IsShared,
	@FsrmQuotaType,
	@FsrmQuotaSizeBytes)

SET @ID = SCOPE_IDENTITY()

RETURN

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name ='UpdateStorageSpaceFolder')
	DROP PROCEDURE UpdateStorageSpaceFolder
GO

CREATE PROCEDURE UpdateStorageSpaceFolder
(
	@ID INT,
	@Name varchar(300),
	@StorageSpaceId INT,
	@Path varchar(max),
	@UncPath varchar(max),
	@IsShared BIT,
	@FsrmQuotaType INT,
	@FsrmQuotaSizeBytes BIGINT 
)
AS
UPDATE StorageSpaceFolders
SET
	Name=@Name,
	StorageSpaceId=@StorageSpaceId,
	Path=@Path,
	UncPath=@UncPath,
	IsShared=@IsShared,
	FsrmQuotaType=@FsrmQuotaType,
	FsrmQuotaSizeBytes=@FsrmQuotaSizeBytes
WHERE ID = @ID

GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name ='RemoveStorageSpaceFolder')
	DROP PROCEDURE RemoveStorageSpaceFolder
GO

CREATE PROCEDURE RemoveStorageSpaceFolder
(
	@ID INT
)
AS
DELETE
FROM StorageSpaceFolders
WHERE ID=@ID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name ='GetStorageSpaceFolderById')
	DROP PROCEDURE GetStorageSpaceFolderById
GO

CREATE PROCEDURE GetStorageSpaceFolderById
(
	@ID INT
)
AS
SELECT TOP 1
	Id,
	Name,
	StorageSpaceId,
	Path,
	UncPath,
	IsShared,
	FsrmQuotaType,
	FsrmQuotaSizeBytes
FROM StorageSpaceFolders
WHERE Id = @ID
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name ='GetStorageSpaceFoldersByStorageSpaceId')
	DROP PROCEDURE GetStorageSpaceFoldersByStorageSpaceId
GO

CREATE PROCEDURE GetStorageSpaceFoldersByStorageSpaceId
(
	@StorageSpaceId INT
)
AS
SELECT 
	Id,
	Name,
	StorageSpaceId,
	Path,
	UncPath,
	IsShared,
	FsrmQuotaType,
	FsrmQuotaSizeBytes
FROM StorageSpaceFolders
WHERE StorageSpaceId = @StorageSpaceId
GO


-- ENTERPRISE STORAGE UPDATE

IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='EnterpriseFolders' AND COLS.name='StorageSpaceFolderId')
BEGIN
	ALTER TABLE [dbo].[EnterpriseFolders] ADD [StorageSpaceFolderId] INT NULL

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_EnterpriseFolders_StorageSpaceFolderId')
BEGIN
	ALTER TABLE [dbo].[EnterpriseFolders]
	DROP CONSTRAINT [FK_EnterpriseFolders_StorageSpaceFolderId]
END	
GO

ALTER TABLE [dbo].[EnterpriseFolders]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseFolders_StorageSpaceFolderId] FOREIGN KEY([StorageSpaceFolderId])
											REFERENCES [dbo].[StorageSpaceFolders] ([ID]) ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddEnterpriseFolder')
DROP PROCEDURE [dbo].[AddEnterpriseFolder]
GO

CREATE PROCEDURE [dbo].[AddEnterpriseFolder]
(
	@FolderID INT OUTPUT,
	@ItemID INT,
	@FolderName NVARCHAR(255),
	@FolderQuota INT,
	@LocationDrive NVARCHAR(255),
	@HomeFolder NVARCHAR(255),
	@Domain NVARCHAR(255),
	@StorageSpaceFolderId INT
)
AS

INSERT INTO EnterpriseFolders
(
	ItemID,
	FolderName,
	FolderQuota,
	LocationDrive,
	HomeFolder,
	Domain,
	StorageSpaceFolderId
)
VALUES
(
	@ItemID,
	@FolderName,
	@FolderQuota,
	@LocationDrive,
	@HomeFolder,
	@Domain,
	@StorageSpaceFolderId
)


SET @FolderID = SCOPE_IDENTITY()

RETURN
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetEnterpriseFoldersPaged')
DROP PROCEDURE GetEnterpriseFoldersPaged
GO
CREATE PROCEDURE [dbo].[GetEnterpriseFoldersPaged]
(
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@ItemID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows

DECLARE @Folders TABLE
(
	ItemPosition int IDENTITY(1,1),
	Id int
)
INSERT INTO @Folders (Id)
SELECT
	S.EnterpriseFolderID
FROM EnterpriseFolders AS S
WHERE @ItemID = S.ItemID'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(Id) FROM @Folders;
SELECT
	ST.EnterpriseFolderID,
	ST.ItemID,
	ST.FolderName,
	ST.FolderQuota,
	ST.LocationDrive,
	ST.HomeFolder,
	ST.Domain,
	ST.StorageSpaceFolderId,
	ssf.Name,
	ssf.StorageSpaceId,
	ssf.Path,
	ssf.UncPath,
	ssf.IsShared,
	ssf.FsrmQuotaType,
	ssf.FsrmQuotaSizeBytes
FROM @Folders AS S
INNER JOIN EnterpriseFolders AS ST ON S.Id = ST.EnterpriseFolderID
LEFT OUTER JOIN StorageSpaceFolders as ssf on ssf.Id = ST.StorageSpaceFolderId
WHERE S.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int,  @FilterValue nvarchar(50),  @ItemID int',
@StartRow, @MaximumRows,  @FilterValue,  @ItemID


RETURN

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetEnterpriseFolder')
DROP PROCEDURE GetEnterpriseFolder
GO

CREATE PROCEDURE [dbo].[GetEnterpriseFolder]
(
	@ItemID INT,
	@FolderName NVARCHAR(255)
)
AS

SELECT TOP 1
	ST.EnterpriseFolderID,
	ST.ItemID,
	ST.FolderName,
	ST.FolderQuota,
	ST.LocationDrive,
	ST.HomeFolder,
	ST.Domain,
	ST.StorageSpaceFolderId,
	ssf.Name,
	ssf.StorageSpaceId,
	ssf.Path,
	ssf.UncPath,
	ssf.IsShared,
	ssf.FsrmQuotaType,
	ssf.FsrmQuotaSizeBytes
FROM EnterpriseFolders AS ST
LEFT OUTER JOIN StorageSpaceFolders as ssf on ssf.Id = ST.StorageSpaceFolderId
WHERE ItemID = @ItemID AND FolderName = @FolderName
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateEntepriseFolderStorageSpaceFolder')
DROP PROCEDURE UpdateEntepriseFolderStorageSpaceFolder
GO

CREATE PROCEDURE [dbo].[UpdateEntepriseFolderStorageSpaceFolder]
(
	@ItemID INT,
	@FolderName NVARCHAR(255),
	@StorageSpaceFolderId INT
)
AS

UPDATE EnterpriseFolders
SET StorageSpaceFolderId = @StorageSpaceFolderId
WHERE ItemID = @ItemID AND FolderName = @FolderName
GO

--Deleted Users + Storage Spaces START

--TODO REMOVE DROP
IF EXISTS (SELECT name FROM SYS.OBJECTS WHERE name='ExchangeOrganizationSsFolders')
	DROP TABLE ExchangeOrganizationSsFolders
GO

IF NOT EXISTS (SELECT name FROM SYS.OBJECTS WHERE name='ExchangeOrganizationSsFolders')
	CREATE TABLE ExchangeOrganizationSsFolders
	(
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		ItemId INT NOT NULL,
		Type varchar(100) NOT NULL,
		StorageSpaceFolderId INT NOT NULL
	)
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId')
BEGIN
	ALTER TABLE [dbo].[ExchangeOrganizationSsFolders]
	DROP CONSTRAINT [FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId]
END	
GO

ALTER TABLE [dbo].[ExchangeOrganizationSsFolders]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId] FOREIGN KEY([StorageSpaceFolderId])
											REFERENCES [dbo].[StorageSpaceFolders] ([ID]) ON DELETE CASCADE
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE type = 'F' AND name = 'FK_ExchangeOrganizationSsFolders_ItemId')
BEGIN
	ALTER TABLE [dbo].[ExchangeOrganizationSsFolders]
	DROP CONSTRAINT [FK_ExchangeOrganizationSsFolders_ItemId]
END	
GO

ALTER TABLE [dbo].[ExchangeOrganizationSsFolders]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeOrganizationSsFolders_ItemId] FOREIGN KEY([ItemId])
											REFERENCES [dbo].[ExchangeOrganizations] ([ItemID]) ON DELETE CASCADE
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationStoragSpaceFolders' )
	DROP PROCEDURE GetOrganizationStoragSpaceFolders
GO

CREATE PROCEDURE GetOrganizationStoragSpaceFolders
(
	@ItemId INT
)
AS
	SELECT
		SSF.Id,
		SSF.Name,
		SSF.StorageSpaceId,
		SSF.Path,
		SSF.UncPath,
		SSF.IsShared,
		SSF.FsrmQuotaType,
		SSF.FsrmQuotaSizeBytes
	FROM [ExchangeOrganizationSsFolders] AS OSSF
	INNER JOIN [StorageSpaceFolders] AS SSF ON SSF.Id = OSSF.StorageSpaceFolderId
	WHERE ItemId = @ItemId
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetOrganizationStoragSpacesFolderByType' )
	DROP PROCEDURE GetOrganizationStoragSpacesFolderByType
GO

CREATE PROCEDURE GetOrganizationStoragSpacesFolderByType
(
	@ItemId INT,
	@Type varchar(100)
)
AS
	SELECT
		SSF.Id,
		SSF.Name,
		SSF.StorageSpaceId,
		SSF.Path,
		SSF.UncPath,
		SSF.IsShared,
		SSF.FsrmQuotaType,
		SSF.FsrmQuotaSizeBytes
	FROM [ExchangeOrganizationSsFolders] AS OSSF
	INNER JOIN [StorageSpaceFolders] AS SSF ON SSF.Id = OSSF.StorageSpaceFolderId
	WHERE ItemId = @ItemId AND Type = @Type
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteOrganizationStoragSpacesFolder' )
	DROP PROCEDURE DeleteOrganizationStoragSpacesFolder
GO

CREATE PROCEDURE DeleteOrganizationStoragSpacesFolder
(
	@Id INT
)
AS
	DELETE
	FROM [ExchangeOrganizationSsFolders]
	WHERE StorageSpaceFolderId = @Id
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddOrganizationStoragSpacesFolder' )
	DROP PROCEDURE AddOrganizationStoragSpacesFolder
GO

CREATE PROCEDURE AddOrganizationStoragSpacesFolder
(
	@Id INT OUTPUT,
	@ItemId INT,
	@Type varchar(100),
	@StorageSpaceFolderId INT
)
AS
	INSERT INTO [ExchangeOrganizationSsFolders]
	(
		ItemId,
		Type,
		StorageSpaceFolderId
	)
	VALUES 
	(
		@ItemId,
		@Type,
		@StorageSpaceFolderId
	)

	SET @Id = @StorageSpaceFolderId
GO

--Deleted Users + Storage Spaces END

-- REMOVE ECOMMERCE 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecWriteSupportedPluginLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecWriteSupportedPluginLog]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecVoidCustomerInvoice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecVoidCustomerInvoice]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateTopLevelDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateTopLevelDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateTaxation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateTaxation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateSystemTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateSystemTrigger]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateServiceHandlersResponses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateServiceHandlersResponses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateInvoice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateInvoice]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateHostingPlanSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateHostingPlanSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateHostingAddonSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateHostingAddonSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateHostingAddon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateHostingAddon]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateDomainNameSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateDomainNameSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateCustomerPayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateCustomerPayment]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateContract]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateContract]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateCategory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecUpdateBillingCycle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecUpdateBillingCycle]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSetSvcsUsageRecordsClosed]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecSetSvcsUsageRecordsClosed]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSetStoreSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecSetStoreSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSetPluginProperties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecSetPluginProperties]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSetPaymentProfile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecSetPaymentProfile]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSetPaymentMethod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecSetPaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSetInvoiceItemProcessed]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecSetInvoiceItemProcessed]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecPaymentProfileExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecPaymentProfileExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecLookupForTransaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecLookupForTransaction]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecIsSupportedPluginActive]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecIsSupportedPluginActive]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetWholeCategoriesSet]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetWholeCategoriesSet]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetUnpaidInvoices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetUnpaidInvoices]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetTopLevelDomainsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetTopLevelDomainsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetTopLevelDomainCycles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetTopLevelDomainCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetTopLevelDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetTopLevelDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetTaxationsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetTaxationsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetTaxationsCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetTaxationsCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetTaxation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetTaxation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetSystemTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetSystemTrigger]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetSvcsSuspendDateAligned]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetSvcsSuspendDateAligned]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetSupportedPluginsByGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetSupportedPluginsByGroup]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetSupportedPluginByID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetSupportedPluginByID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetSupportedPlugin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetSupportedPlugin]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStoreSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStoreSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStorefrontProductsInCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStorefrontProductsInCategory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStorefrontProductsByType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStorefrontProductsByType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStorefrontProduct]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStorefrontProduct]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStorefrontPath]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStorefrontPath]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStorefrontHostingPlanAddons]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStorefrontHostingPlanAddons]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStorefrontCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStorefrontCategory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStorefrontCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStorefrontCategories]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetStoreDefaultSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetStoreDefaultSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetServiceSuspendDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetServiceSuspendDate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetServicesToInvoice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetServicesToInvoice]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetServiceItemType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetServiceItemType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetServiceHandlersResponsesByReseller]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetServiceHandlersResponsesByReseller]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetResellerTopLevelDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetResellerTopLevelDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetResellerPMPlugin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetResellerPMPlugin]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetResellerPaymentMethods]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetResellerPaymentMethods]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetResellerPaymentMethod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetResellerPaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductTypeControl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductTypeControl]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductsPagedByType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductsPagedByType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductsCountByType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductsCountByType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductsByType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductsByType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductHighlights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductHighlights]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductCategoriesIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductCategoriesIds]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetProductCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetProductCategories]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetPluginProperties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetPluginProperties]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetPaymentProfile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetPaymentProfile]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetPaymentMethod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetPaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetInvoicesItemsToActivate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetInvoicesItemsToActivate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetInvoicesItemsOverdue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetInvoicesItemsOverdue]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingPlansTaken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingPlansTaken]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingPlanCycles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingPlanCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingPackageSvcHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingPackageSvcHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingPackageSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingPackageSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingAddonSvcHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingAddonSvcHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingAddonSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingAddonSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingAddonsTaken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingAddonsTaken]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingAddonCycles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingAddonCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetHostingAddon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetHostingAddon]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetDomainNameSvcHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetDomainNameSvcHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetDomainNameSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetDomainNameSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomerTaxation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomerTaxation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomersServicesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomersServicesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomersServicesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomersServicesCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomersPaymentsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomersPaymentsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomersPaymentsCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomersPaymentsCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomersInvoicesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomersInvoicesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomersInvoicesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomersInvoicesCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomerService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomerService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomerPayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomerPayment]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomerInvoiceItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomerInvoiceItems]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomerInvoice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomerInvoice]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCustomerContract]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCustomerContract]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetContract]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetContract]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCategory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCategoriesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCategoriesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetCategoriesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetCategoriesCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetBillingCyclesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetBillingCyclesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetBillingCyclesFree]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetBillingCyclesFree]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetBillingCyclesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetBillingCyclesCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetBillingCycle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetBillingCycle]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetAddonProductsIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetAddonProductsIds]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecGetAddonProducts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecGetAddonProducts]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteTaxation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteTaxation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteSystemTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteSystemTrigger]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteProduct]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteProduct]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeletePaymentProfile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeletePaymentProfile]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeletePaymentMethod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeletePaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteCustomerService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteCustomerService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteCustomerPayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteCustomerPayment]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteContract]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteContract]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteCategory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDeleteBillingCycle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecDeleteBillingCycle]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecCheckCustomerContractExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecCheckCustomerContractExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecChangeHostingPlanSvcCycle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecChangeHostingPlanSvcCycle]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecBulkServiceDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecBulkServiceDelete]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddTopLevelDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddTopLevelDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddTaxation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddTaxation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddSystemTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddSystemTrigger]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddServiceUsageRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddServiceUsageRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddServiceHandlerTextResponse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddServiceHandlerTextResponse]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddInvoice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddInvoice]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddHostingPlanSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddHostingPlanSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddHostingAddonSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddHostingAddonSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddHostingAddon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddHostingAddon]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddDomainNameSvc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddDomainNameSvc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddCustomerPayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddCustomerPayment]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddContract]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddContract]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddCategory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddBillingCycle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ecAddBillingCycle]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecTopLevelDomainsCycles_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecTopLevelDomainsCycles]'))
ALTER TABLE [dbo].[ecTopLevelDomainsCycles] DROP CONSTRAINT [FK_ecTopLevelDomainsCycles_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecTopLevelDomains_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecTopLevelDomains]'))
ALTER TABLE [dbo].[ecTopLevelDomains] DROP CONSTRAINT [FK_ecTopLevelDomains_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecSvcsUsageLog_ecService]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecSvcsUsageLog]'))
ALTER TABLE [dbo].[ecSvcsUsageLog] DROP CONSTRAINT [FK_ecSvcsUsageLog_ecService]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecService_ecProductType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecService]'))
ALTER TABLE [dbo].[ecService] DROP CONSTRAINT [FK_ecService_ecProductType]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecProductsHighlights_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecProductsHighlights]'))
ALTER TABLE [dbo].[ecProductsHighlights] DROP CONSTRAINT [FK_ecProductsHighlights_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EC_ProductsToCategories_EC_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecProductCategories]'))
ALTER TABLE [dbo].[ecProductCategories] DROP CONSTRAINT [FK_EC_ProductsToCategories_EC_Products]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EC_ProductsToCategories_EC_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecProductCategories]'))
ALTER TABLE [dbo].[ecProductCategories] DROP CONSTRAINT [FK_EC_ProductsToCategories_EC_Categories]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecProduct_ecProductType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecProduct]'))
ALTER TABLE [dbo].[ecProduct] DROP CONSTRAINT [FK_ecProduct_ecProductType]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecPaymentMethods_ecSupportedPlugins]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecPaymentMethods]'))
ALTER TABLE [dbo].[ecPaymentMethods] DROP CONSTRAINT [FK_ecPaymentMethods_ecSupportedPlugins]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecInvoiceItems_ecInvoice]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecInvoiceItems]'))
ALTER TABLE [dbo].[ecInvoiceItems] DROP CONSTRAINT [FK_ecInvoiceItems_ecInvoice]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecHostingPlansBillingCycles_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingPlansBillingCycles]'))
ALTER TABLE [dbo].[ecHostingPlansBillingCycles] DROP CONSTRAINT [FK_ecHostingPlansBillingCycles_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecHostingPlansBillingCycles_ecBillingCycles]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingPlansBillingCycles]'))
ALTER TABLE [dbo].[ecHostingPlansBillingCycles] DROP CONSTRAINT [FK_ecHostingPlansBillingCycles_ecBillingCycles]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecHostingPlans_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingPlans]'))
ALTER TABLE [dbo].[ecHostingPlans] DROP CONSTRAINT [FK_ecHostingPlans_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecPackagesSvcsCycles_ecService]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingPackageSvcsCycles]'))
ALTER TABLE [dbo].[ecHostingPackageSvcsCycles] DROP CONSTRAINT [FK_ecPackagesSvcsCycles_ecService]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecPackagesSvcs_ecService]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingPackageSvcs]'))
ALTER TABLE [dbo].[ecHostingPackageSvcs] DROP CONSTRAINT [FK_ecPackagesSvcs_ecService]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecAddonPackagesSvcsCycles_ecService]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingAddonSvcsCycles]'))
ALTER TABLE [dbo].[ecHostingAddonSvcsCycles] DROP CONSTRAINT [FK_ecAddonPackagesSvcsCycles_ecService]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecAddonPackagesSvcs_ecService]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingAddonSvcs]'))
ALTER TABLE [dbo].[ecHostingAddonSvcs] DROP CONSTRAINT [FK_ecAddonPackagesSvcs_ecService]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecHostingAddonsCycles_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingAddonsCycles]'))
ALTER TABLE [dbo].[ecHostingAddonsCycles] DROP CONSTRAINT [FK_ecHostingAddonsCycles_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecHostingAddonsCycles_ecBillingCycles]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingAddonsCycles]'))
ALTER TABLE [dbo].[ecHostingAddonsCycles] DROP CONSTRAINT [FK_ecHostingAddonsCycles_ecBillingCycles]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecHostingAddons_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecHostingAddons]'))
ALTER TABLE [dbo].[ecHostingAddons] DROP CONSTRAINT [FK_ecHostingAddons_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecDomainsSvcsCycles_ecService]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecDomainSvcsCycles]'))
ALTER TABLE [dbo].[ecDomainSvcsCycles] DROP CONSTRAINT [FK_ecDomainsSvcsCycles_ecService]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecDomainsSvcs_ecService]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecDomainSvcs]'))
ALTER TABLE [dbo].[ecDomainSvcs] DROP CONSTRAINT [FK_ecDomainsSvcs_ecService]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ecAddonProducts_ecProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ecAddonProducts]'))
ALTER TABLE [dbo].[ecAddonProducts] DROP CONSTRAINT [FK_ecAddonProducts_ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecTopLevelDomainsCycles]') AND type in (N'U'))
DROP TABLE [dbo].[ecTopLevelDomainsCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecTopLevelDomains]') AND type in (N'U'))
DROP TABLE [dbo].[ecTopLevelDomains]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecTaxations]') AND type in (N'U'))
DROP TABLE [dbo].[ecTaxations]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSystemTriggers]') AND type in (N'U'))
DROP TABLE [dbo].[ecSystemTriggers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSvcsUsageLog]') AND type in (N'U'))
DROP TABLE [dbo].[ecSvcsUsageLog]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSupportedPlugins]') AND type in (N'U'))
DROP TABLE [dbo].[ecSupportedPlugins]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecSupportedPluginLog]') AND type in (N'U'))
DROP TABLE [dbo].[ecSupportedPluginLog]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecStoreSettings]') AND type in (N'U'))
DROP TABLE [dbo].[ecStoreSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecStoreDefaultSettings]') AND type in (N'U'))
DROP TABLE [dbo].[ecStoreDefaultSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecServiceHandlersResponses]') AND type in (N'U'))
DROP TABLE [dbo].[ecServiceHandlersResponses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecService]') AND type in (N'U'))
DROP TABLE [dbo].[ecService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecProductTypeControls]') AND type in (N'U'))
DROP TABLE [dbo].[ecProductTypeControls]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecProductType]') AND type in (N'U'))
DROP TABLE [dbo].[ecProductType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecProductsHighlights]') AND type in (N'U'))
DROP TABLE [dbo].[ecProductsHighlights]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecProductCategories]') AND type in (N'U'))
DROP TABLE [dbo].[ecProductCategories]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecProduct]') AND type in (N'U'))
DROP TABLE [dbo].[ecProduct]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecPluginsProperties]') AND type in (N'U'))
DROP TABLE [dbo].[ecPluginsProperties]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecPaymentProfiles]') AND type in (N'U'))
DROP TABLE [dbo].[ecPaymentProfiles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecPaymentMethods]') AND type in (N'U'))
DROP TABLE [dbo].[ecPaymentMethods]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecInvoiceItems]') AND type in (N'U'))
DROP TABLE [dbo].[ecInvoiceItems]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecInvoice]') AND type in (N'U'))
DROP TABLE [dbo].[ecInvoice]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingPlansBillingCycles]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingPlansBillingCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingPlans]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingPlans]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingPackageSvcsCycles]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingPackageSvcsCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingPackageSvcs]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingPackageSvcs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingAddonSvcsCycles]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingAddonSvcsCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingAddonSvcs]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingAddonSvcs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingAddonsCycles]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingAddonsCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecHostingAddons]') AND type in (N'U'))
DROP TABLE [dbo].[ecHostingAddons]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDomainSvcsCycles]') AND type in (N'U'))
DROP TABLE [dbo].[ecDomainSvcsCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecDomainSvcs]') AND type in (N'U'))
DROP TABLE [dbo].[ecDomainSvcs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecCustomersPayments]') AND type in (N'U'))
DROP TABLE [dbo].[ecCustomersPayments]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecContracts]') AND type in (N'U'))
DROP TABLE [dbo].[ecContracts]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecCategory]') AND type in (N'U'))
DROP TABLE [dbo].[ecCategory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecBillingCycles]') AND type in (N'U'))
DROP TABLE [dbo].[ecBillingCycles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ecAddonProducts]') AND type in (N'U'))
DROP TABLE [dbo].[ecAddonProducts]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ServiceHandlersResponsesDetailed]'))
DROP VIEW [dbo].[ServiceHandlersResponsesDetailed]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ContractsServicesDetailed]'))
DROP VIEW [dbo].[ContractsServicesDetailed]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ContractsInvoicesDetailed]'))
DROP VIEW [dbo].[ContractsInvoicesDetailed]
GO



ALTER PROCEDURE [dbo].[GetExchangeAccountByMailboxPlanId] 
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
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName,
	E.ArchivingMailboxPlanId, 
	AP.MailboxPlan as 'ArchivingMailboxPlan',
	E.EnableArchiving
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId	
LEFT OUTER JOIN ExchangeMailboxPlans AS AP ON E.ArchivingMailboxPlanId = AP.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.MailboxPlanId IS NULL AND
	E.AccountType IN (1,5,6,10,12) 
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
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName,
	E.ArchivingMailboxPlanId, 
	AP.MailboxPlan as 'ArchivingMailboxPlan',
	E.EnableArchiving
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId	
LEFT OUTER JOIN ExchangeMailboxPlans AS AP ON E.ArchivingMailboxPlanId = AP.MailboxPlanId
WHERE
	E.MailboxPlanId = @MailboxPlanId AND
	E.AccountType IN (1,5,6,10,12) 
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
	E.MailboxPlanId,
	P.MailboxPlan,
	E.SubscriberNumber,
	E.UserPrincipalName,
	E.ArchivingMailboxPlanId, 
	AP.MailboxPlan as 'ArchivingMailboxPlan',
	E.EnableArchiving
FROM
	ExchangeAccounts AS E
LEFT OUTER JOIN ExchangeMailboxPlans AS P ON E.MailboxPlanId = P.MailboxPlanId	
LEFT OUTER JOIN ExchangeMailboxPlans AS AP ON E.ArchivingMailboxPlanId = AP.MailboxPlanId
WHERE
	E.ItemID = @ItemID AND
	E.MailboxPlanId = @MailboxPlanId AND
	E.AccountType IN (1,5,6,10,12) 
RETURN
END
Go

-- Quotas Per Organization
IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='Quotas' AND COLS.name='PerOrganization')
BEGIN
	ALTER TABLE [dbo].[Quotas] ADD [PerOrganization] int NULL
END
GO

UPDATE Quotas
SET PerOrganization = 1
WHERE QuotaName in (
	'Exchange2007.DiskSpace',
	'Exchange2007.Mailboxes',
	'Exchange2007.Contacts',
	'Exchange2007.DistributionLists',
	'Exchange2007.PublicFolders',
	'HostedSolution.Users',
	'HostedSolution.Domains',
	'Exchange2007.RecoverableItemsSpace',
	'HostedSolution.SecurityGroups',
	'Exchange2013.ArchivingStorage',
	'Exchange2013.ArchivingMailboxes',
	'Exchange2013.ResourceMailboxes',
	'Exchange2013.SharedMailboxes',
	'Exchange2013.JournalingMailboxes',
	'HostedSolution.DeletedUsers',
	'HostedSolution.DeletedUsersBackupStorageSpace',
	
	'HostedSharePoint.Sites',
	'HostedSharePointEnterprise.Sites',
	'HostedCRM.Users',
	'HostedCRM.LimitedUsers',
	'HostedCRM.ESSUsers',
	'HostedCRM2013.ProfessionalUsers',
	'HostedCRM2013.BasicUsers',
	'HostedCRM2013.EssentialUsers',
	'BlackBerry.Users',
	'OCS.Users',
	'Lync.Users',
	'SfB.Users',
	'EnterpriseStorage.Folders',
	'EnterpriseStorage.DiskStorageSpace',
	'RDS.Servers',
	'RDS.Collections',
	'RDS.Users'
	)
GO



ALTER PROCEDURE [dbo].[GetPackageQuotas]
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
	--dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, 0) AS ParentEnabled
	CASE
		WHEN RG.GroupName = 'Service Levels' THEN dbo.GetPackageServiceLevelResource(@ParentPackageID, RG.GroupID, 0)
		ELSE dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, 0)
	END AS ParentEnabled
FROM ResourceGroups AS RG
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
--WHERE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, 0) = 1
WHERE (dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, 0) = 1 AND RG.GroupName <> 'Service Levels') OR
	  (dbo.GetPackageServiceLevelResource(@PackageID, RG.GroupID, 0) = 1 AND RG.GroupName = 'Service Levels')
ORDER BY RG.GroupOrder

-- return quotas
DECLARE @OrgsCount INT
SET @OrgsCount = dbo.GetPackageAllocatedQuota(@PackageID, 205) -- 205 - HostedSolution.Organizations
SET @OrgsCount = CASE WHEN ISNULL(@OrgsCount, 0) < 1 THEN 1 ELSE @OrgsCount END

SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	QuotaValue = CASE WHEN Q.PerOrganization = 1 AND dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) <> -1 THEN 
					dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) * @OrgsCount 
				 ELSE 
					dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) 
				 END,
	QuotaValuePerOrganization = dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID),
	dbo.GetPackageAllocatedQuota(@ParentPackageID, Q.QuotaID) AS ParentQuotaValue,
	ISNULL(dbo.CalculateQuotaUsage(@PackageID, Q.QuotaID), 0) AS QuotaUsedValue,
	Q.PerOrganization
FROM Quotas AS Q
WHERE Q.HideQuota IS NULL OR Q.HideQuota = 0
ORDER BY Q.QuotaOrder

RETURN
GO



ALTER PROCEDURE [dbo].[GetPackageQuota]
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
DECLARE @OrgsCount INT
SET @OrgsCount = dbo.GetPackageAllocatedQuota(@PackageID, 205) -- 205 - HostedSolution.Organizations
SET @OrgsCount = CASE WHEN ISNULL(@OrgsCount, 0) < 1 THEN 1 ELSE @OrgsCount END

SELECT
	Q.QuotaID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	QuotaAllocatedValue = CASE WHEN Q.PerOrganization = 1 AND ISNULL(dbo.GetPackageAllocatedQuota(@PackageId, Q.QuotaID), 0) <> -1 THEN 
					ISNULL(dbo.GetPackageAllocatedQuota(@PackageId, Q.QuotaID), 0) * @OrgsCount 
				 ELSE 
					ISNULL(dbo.GetPackageAllocatedQuota(@PackageId, Q.QuotaID), 0)
				 END,
	QuotaAllocatedValuePerOrganization = ISNULL(dbo.GetPackageAllocatedQuota(@PackageId, Q.QuotaID), 0),
	ISNULL(dbo.CalculateQuotaUsage(@PackageId, Q.QuotaID), 0) AS QuotaUsedValue
FROM Quotas AS Q
WHERE Q.QuotaName = @QuotaName

RETURN
GO


GO

-- RDS Messages

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'RDSMessages')
CREATE TABLE [dbo].[RDSMessages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RDSCollectionId] [int] NOT NULL,
	[MessageText] [ntext] NOT NULL,
	[UserName] [nchar](250) NOT NULL,
	[Date] [datetime] NOT NULL
CONSTRAINT [PK_RDSMessages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='FK_RDSMessages_RDSCollections')
ALTER TABLE [dbo].[RDSMessages]  WITH CHECK ADD  CONSTRAINT [FK_RDSMessages_RDSCollections] FOREIGN KEY([RDSCollectionId])
REFERENCES [dbo].[RDSCollections] ([ID])
ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddRDSMessage')
DROP PROCEDURE AddRDSMessage
GO
CREATE PROCEDURE [dbo].[AddRDSMessage]
(
	@RDSMessageId INT OUTPUT,
	@RDSCollectionId INT,
	@MessageText NTEXT,
	@UserName NVARCHAR(255),
	@Date DATETIME
)
AS
INSERT INTO RDSMEssages
(
	RDSCollectionId,
	[MessageText],
	UserName,
	[Date]
)
VALUES
(
	@RDSCollectionId,
	@MessageText,
	@UserName,
	@Date
)

SET @RDSMessageId = SCOPE_IDENTITY()

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSMessages')
DROP PROCEDURE GetRDSMessages
GO
CREATE PROCEDURE [dbo].[GetRDSMessages]
(
	@RDSCollectionId INT
)
AS
SELECT Id, RDSCollectionId, MessageText, UserName, [Date] FROM [dbo].[RDSMessages] WHERE RDSCollectionId = @RDSCollectionId
RETURN
GO

-- Changes for VPS Autodiscover

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetServicesByGroupName')
BEGIN
DROP PROCEDURE GetServicesByGroupName
END
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'MoveServiceItem')
BEGIN
DROP PROCEDURE MoveServiceItem
END
GO

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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

--- ChangePackageUser ---
-------------------------
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'ChangePackageUser')
BEGIN
DROP PROCEDURE ChangePackageUser
END
GO

CREATE PROCEDURE [dbo].[ChangePackageUser]
(
	@PackageID int,
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE Packages
SET UserID = @UserID
WHERE PackageID = @PackageID

COMMIT TRAN

RETURN
GO

--- GetVirtualServices ---
--------------------------
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetVirtualServices')
BEGIN
DROP PROCEDURE GetVirtualServices
END
GO

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
GO

-- Private Network VLANs

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'PrivateNetworkVLANs')
BEGIN
CREATE TABLE [dbo].[PrivateNetworkVLANs]
(
	[VlanID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Vlan] INT NOT NULL,
	[ServerID] INT,
	[Comments] NTEXT,
	CONSTRAINT [FK_ServerID]
		FOREIGN KEY ([ServerID]) REFERENCES [dbo].[Servers] ([ServerID])
		ON DELETE CASCADE
)
END
ELSE
BEGIN
ALTER TABLE [dbo].[PrivateNetworkVLANs] ALTER COLUMN [ServerID] INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'PackageVLANs')
BEGIN
CREATE TABLE [dbo].[PackageVLANs]
(
	[PackageVlanID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[VlanID] INT NOT NULL,
	[PackageID] INT NOT NULL,
	CONSTRAINT [FK_VlanID]
		FOREIGN KEY ([VlanID]) REFERENCES [dbo].[PrivateNetworkVLANs] ([VlanID])
		ON DELETE CASCADE,
	CONSTRAINT [FK_PackageID]
		FOREIGN KEY ([PackageID]) REFERENCES [dbo].[Packages] ([PackageID])
		ON DELETE CASCADE
)
END
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.PackageVLANs'), 'IsDmz', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE PackageVLANs 
    ADD IsDmz [bit] NOT NULL DEFAULT 0
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddPrivateNetworkVlan')
BEGIN
DROP PROCEDURE AddPrivateNetworkVlan
END
GO

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

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeletePrivateNetworkVLAN')
BEGIN
DROP PROCEDURE DeletePrivateNetworkVLAN
END
GO

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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPrivateNetworVLANsPaged')
BEGIN
DROP PROCEDURE GetPrivateNetworVLANsPaged
END
GO

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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPrivateNetworVLAN')
BEGIN
DROP PROCEDURE GetPrivateNetworVLAN
END
GO

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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdatePrivateNetworVLAN')
BEGIN
DROP PROCEDURE UpdatePrivateNetworVLAN
END
GO

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
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackagePrivateNetworkVLANs')
BEGIN
DROP PROCEDURE GetPackagePrivateNetworkVLANs
END
GO

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
AND PA.IsDmz = 0
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeallocatePackageVLAN')
BEGIN
DROP PROCEDURE DeallocatePackageVLAN
END
GO

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

	IF (@ParentPackageID = 1) -- "System" space
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetUnallottedVLANs')
BEGIN
DROP PROCEDURE GetUnallottedVLANs
END
GO

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

IF @ParentPackageID = 1 -- "System" space
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AllocatePackageVLANs')
BEGIN
DROP PROCEDURE AllocatePackageVLANs
END
GO

CREATE PROCEDURE [dbo].[AllocatePackageVLANs]
(
	@PackageID int,
	@IsDmz bit,
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
		VlanID,
		IsDmz
	)
	SELECT		
		@PackageID,
		VlanID,
		@IsDmz

	FROM OPENXML(@idoc, '/items/item', 1) WITH 
	(
		VlanID int '@id'
	) as PX

	-- remove document
	exec sp_xml_removedocument @idoc

END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'VPS2012.PrivateVLANsNumber')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (728, 33, 14, N'VPS2012.PrivateVLANsNumber', N'Number of Private Network VLANs', 2, 0, NULL, NULL)
END
GO

-- Additional VHD count per VM
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'VPS2012.AdditionalVhdCount')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (730, 33, 6, N'VPS2012.AdditionalVhdCount', N'Additional Hard Drives per VPS', 3, 0, NULL, NULL)
END
GO

IF EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'ServiceItemProperties')
ALTER TABLE [dbo].[ServiceItemProperties] ALTER COLUMN [PropertyValue] [nvarchar](MAX) NULL; 
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type IN ('FN', 'IF', 'TF') AND name = 'SplitString')
DROP FUNCTION [dbo].[SplitString]
GO

CREATE FUNCTION dbo.SplitString (@stringToSplit VARCHAR(MAX), @separator CHAR)
RETURNS
 @returnList TABLE ([value] [nvarchar] (500))
AS
BEGIN

 DECLARE @value NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(@separator, @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(@separator, @stringToSplit)  
  SELECT @value = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT @value

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @stringToSplit

 RETURN
END
GO


-- Exchange2013 Shared and resource mailboxes Organization statistics

ALTER PROCEDURE [dbo].[GetExchangeOrganizationStatistics] 
(
	@ItemID int
)
AS

DECLARE @ARCHIVESIZE INT
IF -1 in (SELECT B.ArchiveSizeMB FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID)
BEGIN
	SET @ARCHIVESIZE = -1
END
ELSE
BEGIN
	SET @ARCHIVESIZE = (SELECT SUM(B.ArchiveSizeMB) FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID AND A.AccountType in (1, 5, 6, 10, 12) AND B.EnableArchiving = 1)
END

IF -1 IN (SELECT B.MailboxSizeMB FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID)
BEGIN
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 1) AND ItemID = @ItemID) AS CreatedMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 10) AND ItemID = @ItemID) AS CreatedSharedMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 5 OR AccountType = 6) AND ItemID = @ItemID) AS CreatedResourceMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 2 AND ItemID = @ItemID) AS CreatedContacts,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 3 AND ItemID = @ItemID) AS CreatedDistributionLists,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 4 AND ItemID = @ItemID) AS CreatedPublicFolders,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 12 AND ItemID = @ItemID) AS CreatedJournalingMailboxes,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains,
	(SELECT MIN(B.MailboxSizeMB) FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID AND A.AccountType in (1, 5, 6, 10, 12)) AS UsedDiskSpace,
	(SELECT MIN(B.RecoverableItemsSpace) FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID AND A.AccountType in (1, 5, 6, 10, 12) AND B.AllowLitigationHold = 1) AS UsedLitigationHoldSpace,
	@ARCHIVESIZE AS UsedArchingStorage
END
ELSE
BEGIN
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 1) AND ItemID = @ItemID) AS CreatedMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 10) AND ItemID = @ItemID) AS CreatedSharedMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 5 OR AccountType = 6) AND ItemID = @ItemID) AS CreatedResourceMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 2 AND ItemID = @ItemID) AS CreatedContacts,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 3 AND ItemID = @ItemID) AS CreatedDistributionLists,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 4 AND ItemID = @ItemID) AS CreatedPublicFolders,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 12 AND ItemID = @ItemID) AS CreatedJournalingMailboxes,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains,
	(SELECT SUM(B.MailboxSizeMB) FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID AND A.AccountType in (1, 5, 6, 10, 12)) AS UsedDiskSpace,
	(SELECT SUM(B.RecoverableItemsSpace) FROM ExchangeAccounts AS A INNER JOIN ExchangeMailboxPlans AS B ON A.MailboxPlanId = B.MailboxPlanId WHERE A.ItemID=@ItemID AND A.AccountType in (1, 5, 6, 10, 12) AND B.AllowLitigationHold = 1) AS UsedLitigationHoldSpace,
	@ARCHIVESIZE AS UsedArchingStorage
END


RETURN
GO
-- Search fix
DROP PROCEDURE [dbo].[GetSearchObject]
GO

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
DECLARE @sql nvarchar(max)

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
 INSERT INTO @ItemsService (ItemID, ItemTypeID, Username, Fullname)
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
IF @FilterValue <> ''
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

/*-------------------------------------------SfB-----------------------------------------------------*/

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

/*------------------------------------VPS-IP------------------------------------------------*/
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
  AND (''' + @FilterValue + ''' LIKE ''%.%'' OR ''' + @FilterValue + ''' LIKE ''%:%'')
  AND (PIP.IPAddress LIKE ''' + @FilterValue + ''' OR IPS.ExternalIP LIKE ''' + @FilterValue + ''')'
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

-- Administrative IP restrictions
IF NOT EXISTS (SELECT * FROM [dbo].[SystemSettings] WHERE [SettingsName] = 'AccessIpsSettings')
BEGIN
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'AccessIpsSettings', N'AccessIps', N'')
END
GO


-- SolidCP Migration from Other panels
UPDATE Providers SET ProviderType = N'SolidCP.Providers.OS.Windows2003, SolidCP.Providers.OS.Windows2003' WHERE ProviderID = 1
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Web.IIs60, SolidCP.Providers.Web.IIs60' WHERE ProviderID = 2
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs60' WHERE ProviderID = 3
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.MailEnable, SolidCP.Providers.Mail.MailEnable' WHERE ProviderID = 4
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MsSqlServer, SolidCP.Providers.Database.SqlServer' WHERE ProviderID = 5
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MySqlServer, SolidCP.Providers.Database.MySQL' WHERE ProviderID = 6
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.DNS.MsDNS, SolidCP.Providers.DNS.MsDNS' WHERE ProviderID = 7
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Statistics.AWStats, SolidCP.Providers.Statistics.AWStats' WHERE ProviderID = 8
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.DNS.SimpleDNS, SolidCP.Providers.DNS.SimpleDNS' WHERE ProviderID = 9
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Statistics.SmarterStats, SolidCP.Providers.Statistics.SmarterStats' WHERE ProviderID = 10
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.SmarterMail2, SolidCP.Providers.Mail.SmarterMail2' WHERE ProviderID = 11
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.FTP.Gene6, SolidCP.Providers.FTP.Gene6' WHERE ProviderID = 12
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.Merak, SolidCP.Providers.Mail.Merak' WHERE ProviderID = 13
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.SmarterMail3, SolidCP.Providers.Mail.SmarterMail3' WHERE ProviderID = 14
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MsSqlServer2005, SolidCP.Providers.Database.SqlServer' WHERE ProviderID = 16
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MySqlServer50, SolidCP.Providers.Database.MySQL' WHERE ProviderID = 17
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.MDaemon, SolidCP.Providers.Mail.MDaemon' WHERE ProviderID = 18
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.ArgoMail, SolidCP.Providers.Mail.ArgoMail' WHERE ProviderID = 19
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.hMailServer, SolidCP.Providers.Mail.hMailServer' WHERE ProviderID = 20
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.AbilityMailServer, SolidCP.Providers.Mail.AbilityMailServer' WHERE ProviderID = 21
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.hMailServer43, SolidCP.Providers.Mail.hMailServer43' WHERE ProviderID = 22
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.DNS.IscBind, SolidCP.Providers.DNS.Bind' WHERE ProviderID = 24
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.FTP.ServU, SolidCP.Providers.FTP.ServU' WHERE ProviderID = 25
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.FTP.FileZilla, SolidCP.Providers.FTP.FileZilla' WHERE ProviderID = 26
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.Exchange2007, SolidCP.Providers.HostedSolution' WHERE ProviderID = 27
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.DNS.SimpleDNS5, SolidCP.Providers.DNS.SimpleDNS50' WHERE ProviderID = 28
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.SmarterMail5, SolidCP.Providers.Mail.SmarterMail5' WHERE ProviderID = 29
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MySqlServer51, SolidCP.Providers.Database.MySQL' WHERE ProviderID = 30
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Statistics.SmarterStats4, SolidCP.Providers.Statistics.SmarterStats' WHERE ProviderID = 31
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.Exchange2010, SolidCP.Providers.HostedSolution' WHERE ProviderID = 32
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.DNS.Nettica, SolidCP.Providers.DNS.Nettica' WHERE ProviderID = 55
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.DNS.PowerDNS, SolidCP.Providers.DNS.PowerDNS' WHERE ProviderID = 56
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.SmarterMail6, SolidCP.Providers.Mail.SmarterMail6' WHERE ProviderID = 60
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.Merak10, SolidCP.Providers.Mail.Merak10' WHERE ProviderID = 61
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Statistics.SmarterStats5, SolidCP.Providers.Statistics.SmarterStats' WHERE ProviderID = 62
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.hMailServer5, SolidCP.Providers.Mail.hMailServer5' WHERE ProviderID = 63
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.SmarterMail7, SolidCP.Providers.Mail.SmarterMail7' WHERE ProviderID = 64
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.SmarterMail9, SolidCP.Providers.Mail.SmarterMail9' WHERE ProviderID = 65
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.SmarterMail10, SolidCP.Providers.Mail.SmarterMail10' WHERE ProviderID = 66
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.Exchange2010SP2, SolidCP.Providers.HostedSolution' WHERE ProviderID = 90
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.Exchange2013, SolidCP.Providers.HostedSolution.Exchange2013' WHERE ProviderID = 91
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.OS.Windows2008, SolidCP.Providers.OS.Windows2008' WHERE ProviderID = 100
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Web.IIs70, SolidCP.Providers.Web.IIs70' WHERE ProviderID = 101
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs70' WHERE ProviderID = 102
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.OrganizationProvider, SolidCP.Providers.HostedSolution' WHERE ProviderID = 103
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012' WHERE ProviderID = 104
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Web.IIs80, SolidCP.Providers.Web.IIs80' WHERE ProviderID = 105
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.FTP.MsFTP80, SolidCP.Providers.FTP.IIs80' WHERE ProviderID = 106
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Web.HeliconZoo.HeliconZoo, SolidCP.Providers.Web.HeliconZoo' WHERE ProviderID = 135
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Mail.IceWarp, SolidCP.Providers.Mail.IceWarp' WHERE ProviderID = 160
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.HostedSharePointServer, SolidCP.Providers.HostedSolution' WHERE ProviderID = 200
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.CRMProvider, SolidCP.Providers.HostedSolution' WHERE ProviderID = 201
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MsSqlServer2008, SolidCP.Providers.Database.SqlServer' WHERE ProviderID = 202
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.BlackBerryProvider, SolidCP.Providers.HostedSolution' WHERE ProviderID = 203
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.BlackBerry5Provider, SolidCP.Providers.HostedSolution' WHERE ProviderID = 204
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.OCS2007R2, SolidCP.Providers.HostedSolution' WHERE ProviderID = 205
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.OCSEdge2007R2, SolidCP.Providers.HostedSolution' WHERE ProviderID = 206
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.HostedSharePointServer2010, SolidCP.Providers.HostedSolution' WHERE ProviderID = 208
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MsSqlServer2012, SolidCP.Providers.Database.SqlServer' WHERE ProviderID = 209
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.Lync2010, SolidCP.Providers.HostedSolution' WHERE ProviderID = 250
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Virtualization.HyperV, SolidCP.Providers.Virtualization.HyperV' WHERE ProviderID = 300
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MySqlServer55, SolidCP.Providers.Database.MySQL' WHERE ProviderID = 301
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MySqlServer56, SolidCP.Providers.Database.MySQL' WHERE ProviderID = 302
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Virtualization.HyperV2012R2, SolidCP.Providers.Virtualization.HyperV2012R2' WHERE ProviderID = 350
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.VirtualizationForPC.HyperVForPC, SolidCP.Providers.VirtualizationForPC.HyperVForPC' WHERE ProviderID = 400
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.DNS.MsDNS2012, SolidCP.Providers.DNS.MsDNS2012' WHERE ProviderID = 410
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.EnterpriseStorage.Windows2012, SolidCP.Providers.EnterpriseStorage.Windows2012' WHERE ProviderID = 600
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.StorageSpaces.Windows2012, SolidCP.Providers.StorageSpaces.Windows2012' WHERE ProviderID = 700
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.CRMProvider2011, SolidCP.Providers.HostedSolution.CRM2011' WHERE ProviderID = 1201
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.CRMProvider2013, SolidCP.Providers.HostedSolution.Crm2013' WHERE ProviderID = 1202
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.Database.MsSqlServer2014, SolidCP.Providers.Database.SqlServer' WHERE ProviderID = 1203
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.CRMProvider2015, SolidCP.Providers.HostedSolution.Crm2015' WHERE ProviderID = 1205
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.HostedSharePointServer2013, SolidCP.Providers.HostedSolution.SharePoint2013' WHERE ProviderID = 1301
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.Lync2013, SolidCP.Providers.HostedSolution.Lync2013' WHERE ProviderID = 1401
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.Lync2013HP, SolidCP.Providers.HostedSolution.Lync2013HP' WHERE ProviderID = 1402
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.RemoteDesktopServices.Windows2012,SolidCP.Providers.RemoteDesktopServices.Windows2012' WHERE ProviderID = 1501
Go
UPDATE Providers SET ProviderType = N'SolidCP.Providers.HostedSolution.HostedSharePointServer2013Ent, SolidCP.Providers.HostedSolution.SharePoint2013Ent' WHERE ProviderName = 'HostedSharePoint2013Ent'
Go
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.OS.HomeFolder, SolidCP.Providers.Base' WHERE ItemTypeID = 2
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base' WHERE ItemTypeID = 5
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base' WHERE ItemTypeID = 6
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base' WHERE ItemTypeID = 7
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base' WHERE ItemTypeID = 8
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.FTP.FtpAccount, SolidCP.Providers.Base' WHERE ItemTypeID = 9
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Web.WebSite, SolidCP.Providers.Base' WHERE ItemTypeID = 10
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Mail.MailDomain, SolidCP.Providers.Base' WHERE ItemTypeID = 11
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.DNS.DnsZone, SolidCP.Providers.Base' WHERE ItemTypeID = 12
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.OS.Domain, SolidCP.Providers.Base' WHERE ItemTypeID = 13
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Statistics.StatsSite, SolidCP.Providers.Base' WHERE ItemTypeID = 14
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Mail.MailAccount, SolidCP.Providers.Base' WHERE ItemTypeID = 15
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Mail.MailAlias, SolidCP.Providers.Base' WHERE ItemTypeID = 16
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Mail.MailList, SolidCP.Providers.Base' WHERE ItemTypeID = 17
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Mail.MailGroup, SolidCP.Providers.Base' WHERE ItemTypeID = 18
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.OS.SystemDSN, SolidCP.Providers.Base' WHERE ItemTypeID = 20
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base' WHERE ItemTypeID = 21
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base' WHERE ItemTypeID = 22
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base' WHERE ItemTypeID = 23
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base' WHERE ItemTypeID = 24
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Web.SharedSSLFolder, SolidCP.Providers.Base' WHERE ItemTypeID = 25
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.DNS.SecondaryDnsZone, SolidCP.Providers.Base' WHERE ItemTypeID = 28
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.HostedSolution.Organization, SolidCP.Providers.Base' WHERE ItemTypeID = 29
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.HostedSolution.OrganizationDomain, SolidCP.Providers.Base' WHERE ItemTypeID = 30
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base' WHERE ItemTypeID = 31
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base' WHERE ItemTypeID = 32
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base' WHERE ItemTypeID = 33
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base' WHERE ItemTypeID = 34
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Virtualization.VMInfo, SolidCP.Providers.Base' WHERE ItemTypeID = 35
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base' WHERE ItemTypeID = 36
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base' WHERE ItemTypeID = 37
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base' WHERE ItemTypeID = 38
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base' WHERE ItemTypeID = 39
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base' WHERE ItemTypeID = 40
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base' WHERE ItemTypeID = 41
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base' WHERE ItemTypeID = 42
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.SharePoint.SharePointSiteCollection, SolidCP.Providers.Base' WHERE ItemTypeID = 200
GO
UPDATE ServiceItemTypes SET TypeName = N'SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection, SolidCP.Providers.Base' WHERE ItemTypeID = 201
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.OperatingSystemController' WHERE GroupName = 'OS'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.WebServerController' WHERE GroupName = 'Web'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.FtpServerController' WHERE GroupName = 'FTP'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.MailServerController' WHERE GroupName = 'Mail'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DatabaseServerController' WHERE GroupName = 'MsSQL2000'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DatabaseServerController' WHERE GroupName = 'MySQL4'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DnsServerController' WHERE GroupName = 'DNS'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.StatisticsServerController' WHERE GroupName = 'Statistics'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DatabaseServerController' WHERE GroupName = 'MsSQL2005'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DatabaseServerController' WHERE GroupName = 'MySQL5'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.HostedSharePointServerController' WHERE GroupName = 'Sharepoint Foundation Server'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DatabaseServerController' WHERE GroupName = 'MsSQL2008'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DatabaseServerController' WHERE GroupName = 'MsSQL2012'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.HeliconZooController' WHERE GroupName = 'HeliconZoo'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.EnterpriseStorageController' WHERE GroupName = 'EnterpriseStorage'
GO
UPDATE ResourceGroups SET GroupController = N'SolidCP.EnterpriseServer.DatabaseServerController' WHERE GroupName = 'MsSQL2014'
GO
UPDATE ResourceGroups SET GroupOrder = N'10' WHERE GroupName = 'MsSQL2014'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.ZipFilesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_ZIP_FILES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.BackupTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_BACKUP'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.BackupDatabaseTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_BACKUP_DATABASE'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.CalculateExchangeDiskspaceTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.CalculatePackagesBandwidthTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.CalculatePackagesDiskspaceTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.CheckWebSiteTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_CHECK_WEBSITE'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.DeleteExchangeAccountsTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_DELETE_EXCHANGE_ACCOUNTS'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.DomainExpirationTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_DOMAIN_EXPIRATION'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.DomainLookupViewTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_DOMAIN_LOOKUP'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.FTPFilesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_FTP_FILES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.Ecommerce.EnterpriseServer.GenerateInvoicesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_GENERATE_INVOICES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.HostedSolutionReportTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.NotifyOverusedDatabasesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.Ecommerce.EnterpriseServer.RunPaymentQueueTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_RUN_PAYMENT_QUEUE'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.RunSystemCommandTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_RUN_SYSTEM_COMMAND'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.SendMailNotificationTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_SEND_MAIL'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.SuspendOverusedPackagesTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_SUSPEND_PACKAGES'
GO
UPDATE ScheduleTasks SET TaskType = N'SolidCP.EnterpriseServer.UserPasswordExpirationNotificationTask, SolidCP.EnterpriseServer.Code' WHERE TaskID = 'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/Backup.ascx' WHERE TaskID = 'SCHEDULE_TASK_BACKUP'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/BackupDatabase.ascx' WHERE TaskID = 'SCHEDULE_TASK_BACKUP_DATABASE'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/CheckWebsite.ascx' WHERE TaskID = 'SCHEDULE_TASK_CHECK_WEBSITE'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/DomainExpirationView.ascx' WHERE TaskID = 'SCHEDULE_TASK_DOMAIN_EXPIRATION'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/DomainLookupView.ascx' WHERE TaskID = 'SCHEDULE_TASK_DOMAIN_LOOKUP'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/SendFilesViaFtp.ascx' WHERE TaskID = 'SCHEDULE_TASK_FTP_FILES'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_GENERATE_INVOICES'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/HostedSolutionReport.ascx' WHERE TaskID = 'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/NotifyOverusedDatabases.ascx' WHERE TaskID = 'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_RUN_PAYMENT_QUEUE'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/ExecuteSystemCommand.ascx' WHERE TaskID = 'SCHEDULE_TASK_RUN_SYSTEM_COMMAND'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/SendEmailNotification.ascx' WHERE TaskID = 'SCHEDULE_TASK_SEND_MAIL'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx' WHERE TaskID = 'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/SuspendOverusedSpaces.ascx' WHERE TaskID = 'SCHEDULE_TASK_SUSPEND_PACKAGES'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/UserPasswordExpirationNotificationView.ascx' WHERE TaskID = 'SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION'
GO
UPDATE ScheduleTaskViewConfiguration SET Description = N'~/DesktopModules/SolidCP/ScheduleTaskControls/ZipFiles.ascx' WHERE TaskID = 'SCHEDULE_TASK_ZIP_FILES'
GO
UPDATE ScheduleTaskParameters SET DefaultValue = N'MsSQL2014=SQL Server 2014;MsSQL2016=SQL Server 2016;MsSQL2017=SQL Server 2017;MsSQL2019=SQL Server 2019;MsSQL2022=SQL Server 2022;MySQL5=MySQL 5.0;MariaDB=MariaDB' WHERE TaskID = 'SCHEDULE_TASK_BACKUP_DATABASE' AND ParameterID = 'DATABASE_GROUP'
GO



/* New reporting templates */



DELETE FROM [dbo].[UserSettings] WHERE [SettingsName] in ('BandwidthXLST','DiskspaceXLST');

INSERT INTO [dbo].[UserSettings] ([UserID],[SettingsName],[PropertyName],[PropertyValue])
	VALUES (1, 'BandwidthXLST','Transform','<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
  <html>
  <body>
  <img alt="Embedded Image" width="299" height="60" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASsAAAA8CAYAAAA+AJwjAAAACXBIWXMAAAsTAAALEwEAmpwYAAA4G2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMTQgNzkuMTUxNDgxLCAyMDEzLzAzLzEzLTEyOjA5OjE1ICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgICAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgICAgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIKICAgICAgICAgICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmV4aWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vZXhpZi8xLjAvIj4KICAgICAgICAgPHhtcDpDcmVhdG9yVG9vbD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC94bXA6Q3JlYXRvclRvb2w+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMTYtMDMtMDFUMTQ6NTE6NTgrMDE6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE2LTAzLTAxVDE0OjUxOjU4KzAxOjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8ZGM6Zm9ybWF0PmltYWdlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3A6Q29sb3JNb2RlPgogICAgICAgICA8eG1wTU06SW5zdGFuY2VJRD54bXAuaWlkOjZhNTdmMWYyLTgyZjYtMjk0MS1hYjFmLTNkOWQ0YjdmMTY2YjwveG1wTU06SW5zdGFuY2VJRD4KICAgICAgICAgPHhtcE1NOkRvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6NmE1N2YxZjItODJmNi0yOTQxLWFiMWYtM2Q5ZDRiN2YxNjZiPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjY1NTM1PC9leGlmOkNvbG9yU3BhY2U+CiAgICAgICAgIDxleGlmOlBpeGVsWERpbWVuc2lvbj4yOTk8L2V4aWY6UGl4ZWxYRGltZW5zaW9uPgogICAgICAgICA8ZXhpZjpQaXhlbFlEaW1lbnNpb24+NjA8L2V4aWY6UGl4ZWxZRGltZW5zaW9uPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/Pq+oh5kAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAALU1JREFUeNrsnXmcnFWV97/neZ6q6jXd2XcI+76DoqACKgq4gPsAggvIoDjovOq4oDMqCu7buOAuKg4iyCCCG4uyCI4CYQlJCIGErJ1O70tVPc+95/3j3uqurtTWSTpErZPP80lVddXz3PV3zzn3d86Vh+YtoSABIP7/QGTsNf5z/OfF74VxUbYVG4TM6e1i9dw9Oeb7D2A0gO5eCIPir4VAB3AAsAQ4ApgDzAfaAQP0ARuAx/21GlgH5KglxoKWKV0UQv8gbNoMYehqYP1lDFjrXieJu0eSQBxD03Quu+cqPv7IDaxpnVXz8WkgLG1PEaZK1NdVAev/N4Ap1wa+D7cYg0XH+5kdL1+iyoJUilYRDLuXKLA+jjElYxggVmVuFNEZhiQV2myyMqu/i2+e+CbeeeHXYLDLjbcoKuoEgVCmvuKJnwvqx3U2B81N0NrsxvtUi9XyQAHjz1c//2B8zgHRszxmWoAjgVOBFwInAqk6f7sK+A1wC3Av0E9DGtKQf1h5NsHqROBtwBs9aE1W9gUuAf4VuBn4NvA7r0Q0pCEN+QeTYOofIagEpJM4UBEQmQ9cAdwBvHU7gaoUcM8Efg18Gdij0a0NaUhDsyqVVmAezr8027tIImBPoElFrBXpmDbUG62fPueKKI73ygfpryM8f0pQ0WlazwfeA9zV6N6GNOSfG6yagROA4/z/ewILLEz3JlgIiKhigpDFXWsYaGm76rK3Xf4SNXyFwcGFBFOq0B0N/C9wDnBro4sb0pB/PrBKAa9T1XOAk9RpVaWO/bH7mSBk4ZZ1jASpK8993w9vufWs11zNmuGFxPmJuyBTI9OBa4CzG4DVkIb8Y0i9Ks4rcDtvP7VwhoVW622+4mt8q1wJk5iujtnXveW937nh1jNe823WDC0hl4VUalfVrRP4qTcLG9KQhvyDa1bNwGeA86zjQW0j5SgT2SjNgeufXPG+8z/xlV+d9cZP0TVyIHEOUmF5vtOEG+oAIvcDDwNP+U/TwN6oHggcj0jbJDSsn4CeDKyhmNukCkHAFJukDWlIQ3YBWO2j8B0LJxd/WI42JkUkxCQImdW3Jf/AXod87OYTX30GvfFLGRp2oBBXZRX0IfwAkRsQWQ5sLcHCEJiO6gFY+1KUC4CFNWso7AX6IRJzKarjBNIggP5R6Ol3ZulOIv81pCEN2bVg9RxVvVodo7ymJlUQE4TMHOwhH4a/uOiSq3qfWnDg29iypfDXauX4I4F8hDD8CyJxpdsD3e7SezH2Oqz9KI6nVQ2sAN6O1Ru9KTuuWcUx5PIQBQ2wasjUiiqB2kY77EywEjhU4XvlgKoWWIlamnLD9qcnvO6Gx/Y49HwdGJhLHEP10JJrCIN/h2jzZLoeeAz0AhLzFPABqvvfIuDVWHsHkEPE0fj7B+szTXeuhL48nd5M7QQSoAmY4etRb4HEK7s9QN4D+hZ/5dkNCLJ1VqSD8d1kW1K/EBjymnZBZuPCsLJMjJYpfH8A6K3VaPWUWyrXQXx/tQPlFtiUL0ePqBJHKUbSLZXGWlg0LjqANmBmUf/NwG1oTaY/A//8QV/dLX6xz/nxVq2Tip+vO2kYpHFRJsMT1IiJRputVsdSsJqr8CUcYJXtqDIlj4EhK5JvHx1JNnfM/d53XvaOocSYkxnqr7Xa/JIouhQJurerCUSGgI+TJAL8R41vvwTHCVszBlbW7kqfVRPwXOA04CBVnQ/M8oOiwE/b3t2HvL9HgoufXAU8ATwI/L5kou+6lVCETUnMwjAiI1ININ4FXOgHsymZcM3Abf47BfmQ16g3lgz6woS/AfgYVVT0LmPKxgUCpETYmrj53FG53K3AfwJn4OJWS4EsA/xaRT4wZ7CbWw48kQ+/4WPQt7GwGx4Ac3FulqOAuRid48dDh/+bLbqXbCdIFBat9YzH1v4Z+C3QRxjA6CgEAplMIT7vdcB7/T2SnQRWTX5R3eDrXhp23OPBdC1wJy7ud7AiWCl8SN2kHnuCVl53RoDlwM+shH8SbO8+fZtzZ/zH9WsfPfRFV7H+yQWg1Zr4UZAPYG03xtTSviqMOgPWZrH2Sqw9FOGMaj44rC4G1mCsC+DcdUA1A7hC4UIDUxGtmil6PRM4rOj9Qzhm/4/rUCimRLPKqhJVB6sDcQHslaS7BAiWAAv8VU6WVypLBGwyCXl1yFZRLRGh2xhsENAeBOXKnvHl3rvKbVYH1shApk0fmb8/2XDMN9oJ+k7i+K24sLGpXiTxmuuhuDjcS4F7gI/7xQziZNw94riThz+LCvlW4C/Af+NC6BKAqEjdfami7y50qhT/vy1iLQM+C1wjQmyCkD16NrFq+jy6Oue2MTj4Imf+VQSDmICPI6wiZyCbhc5Ot+LUE/kdhG6k9Y+CWhD6UK7A6otBmipq/8ICVJ2fSmSXmH8C7QrfS+DMZ8MrJi5Q/LtAi8A3y03ggKlB0IKq020NFshUXhyGatymv6TItVb7gUp20ajqmLpST9l7rMUCLUFQungn3gytAtQyNHeoj3v3PIwP/svl0Lse0tEMrLkaa87g2XWTnoBwDcjrCcI7SYybS05GvcWUepbKNhM4DeE04JOI/BdgI+sKOE2Qj+CUwQmdUka7ulHgvQJPmyBkWn6YhV1reHrmQl777qtZvvDgU9j09DyCsNpCfguB3Oye5tFwNOuAasH8ykueAH2j0NfjUrYYO/4M5W8Yez2OuV7pBrM9GBZ8DZuoJ8VMGbDsyA0SxKMYCQirOE4VLk7gTH12HfiRwOXAbQorS5t0VHVKnVsB0GsNsyprz1Ibc3cYtMmr0med+RfVecsQ6LcWAzRX1w63/a1NZGNrJ78/8AQYHQAJMlj7FRJ7xs6p1Q6rvbMQvolyAlZ7HIo/24UqWZaEjwKPY+zPIq89vdjCi4rbT4sgqmia/UKQN4tfUdpzwzw+Z29uO+hkvvT8N7Fy0eGwcfUhhFGHA5KyYgmC69EgS1J05/wgkclz8oO/JVKLlmm09uF+rnvZW8GmoGsdpNPFf84C12NMNbDqw5hWovBHwGKvIa4Alno/z6N1NWJumD8uOoYXb3qM2dkBRsJ0pQkyLxA5dzfZZ5yh8Bbgw6VAstUmqAqpKR6no9bSNMkJXwF36uEHbuPtHrSWRCefNioAhqwbk5lJlL85n+Vviw/hitddBlvWggSnkui57EZ4ABxIkpxJYr9PKirNM7e7ANZl5JM/ROr68awCJJkKo0PhfnFO0Cw4msI+PZu5/pBT+PD7roEnV8CGJyGKFpJUXafXEOp9YwnAxpahiCTTzpVfuYhpSb5sOaYDx6y8jw/+yxeheRrkhiauBMpjWLMRZX6FZz8FHEliXuHfH1H0t6e9/f5b4Eaq7byM9HLzIa/ikK4VfHjp9fS3zao0yA+J4LDdiBRxlFcWTLFtHLJrkuP1W4sJAppEdtQCqpWpo7XU/h9SS6K63WlGAmDYm5CpOrWPJAiIkjxsXgdJHpCTnJ90N6PJqBxDGHzf+YB3ywxLB6McGVllD3FOtwlaVEkG0GGE9wn0CaASsGCwm0fm7sGvjz0LnlwO2SHnS7I6o2pnKA+QJKvGHGKFJ0ybx7m//296O2fRp1pWs3o6jHjN7deS9A5w2blfAo0gN1J8oz5EViJlwSqPtQlB8G9liycswYHxhbj8WN/zoFXeZ5YfYr/+9TSbfFlT0AP8vnFdg0XB5VDsBYakvjCogoXeAXTWqb7P8/6ArkIZR9SQqBJO8XJfuPugNaTCaLu0K3GXAt8BHmGi472AKSng/ybYwMCoWvKqpAPZoTqMqiWQsC64SYDEWkhiiJNmJFiCbAcYTMqFIEULeN2/O9y7RnrYbUWOjBQ9BEdZmDCoSgbTNSh3F77Tkh/m4VlL+PfXfoJlS46G7jUQRsVmfjVZgYhOaMsow1tv/QLvuf0qels7SYLyWn5kElbPWcSZj/4G87OI/3z9p12JTVwoeS+wscLEXY3qxVj7+jpa5hXAS4GfAJ8E1mxr0/TyzSNex4KRHg7sW89gqqnc2N6/roEmcrW4Z/UDo0H93gz1cZovFWs/gOPH1JpvqfGOEkbVYph6sCogiQEGjZm0dlXgRnm6wfW4q87fqQPkHfTHFMowai2pOqAg8b4ugmbAdpLk5tbdzFYB/oRINyLGA0muzPwqLFrTgA5UFdV24EBEFtX5vCavrfZMEkQHUUaRSWdvSYBOlDQidY52OSBSOF5Lal6CycZP2rG/hEmWZTMXs2yPY2DjKqdpmHhc564+MfMTTTcLzTM445HfkU01EQcpqg3j0FrWdy7gVY/8FlHl8pf/P/JhCtSA28FYWeGn04E31wGmBckAbwc5Fpcn6+6Jetowf9vreax96DqO27yCnlQzwcRyZ3DkxVpyWwgXCWSlaFJPzqTnPuNyil1S4+tRAdWdlmAwqmN59UtkD6+JzfL/p/wgU8b5YeIXiE2Mc5wMjitT+HxtaXnzqqS3w3fliGRaP5L7toxVsW5mlPbPQcAiHKdJfNnn+PoO+LILjqu03Ps0R3ybCSBadUYqBoEgA2SnkcvNdxtPNUv+CEHwPpClBAz5xX2kjr51K6a1GZS5oG9DeTeOkFlN0mzPzp/wQUTuRrV5kr+MvTVwJKrvQMsT0EvaZI9IXYdVhAdFHxBk+QSzUAKa4xwM9ri+LKYbqCY10DiY8P1MK5fedDkzR/sYzrRQz3orqvS2dnLi6r+QJ3SDIT9cKPCyCrWZu50L6hGIXANcjMtG6gsRQu8zfO6485ib7eOA3nUMpzLl5lcteSJvNbu9fgz1GlIo3GNcmuiA8ozqdopIl4Uv2Yno/ULgtcAxis7ymlqzX3UL27vKOHenIAUmecEhM+g/G1anlf7Om9bLC5rOqFpS2+G78mX9T+B0xtnQxcqb4PLyf8ahj6JM8FUdBbxT0aP9YtJW5AMrrVuuaMHuBTYL/FLh83m1uVDEVg0/A6xaJclDkk+TJE1IzSExCPwXmfTvJqnsJhQoIIEMgWwln/8PjHkOyAvqcA/MYjxxQH06fTr9F4Lg4R2gAN2OSR7C6PW4SI5q0hxZdEYNM3mZogMyQUNVbLkCioAxXTW4UhO1DYk4fP1jTMsO0dPcSb3DNwlCrIT8+KfvZOWsvfnkKz4CSQ6S5C6S5G+IHLPzzGVZDPLdIn+WH40xT87ej82ZaRxkE4xmKrlqqsnJBj1e0fu2t8sTlFDllgg5qWieFLl5EL+R0gd0C5Dz2oZ34eyj8FGrnOEH7WRBpBS8Zha93l8cS/s9wA+BTyrkjI6j33b4vp7jr0rSTZGqVMQbvMwq7wLm1Vm/TIlTf6HAkeo2Tf7NKj3VOtj4g5KIE4gTS5wkNYnIyuOEwS/J5twJOM1Nk7S1AxjJQT4HiEHpqq7/jYG8TopLoYDJttHc7AhPOrmfjw0wq/eSJH/GRXZUExNZrT4u1anCE8hvRgRrrSNzZnOlKLea8Yyh5eQwb5K52K0kYTjdgglCJuvFCNWwV8869u9aTWgsl7/8QyTWbMCYj6J8B5E5Xj0uuDtM0QIdTdLimofIlxDZBPx1DJyHt/ChEy/m27//FHv1byQbpYtV3aE67nuAwLXi0jAP+t9t8Gp/xq+Yz+CIeoUwhRDY7M2sCEglqhsT9G+UUOOs/0HGE3SdRqXE4+bUYkV/lqDHTaG7KsJlyPiQIDME3qtoLr/9u2K1TKLBUpcz8JkE+152PJV3ALxBHAt9pLoZaEnUQj4P+VhJYq1ClC4sjKsIUoq1447yyRKYVSExDuysfhXltirrTxoX2vK4c6Xo5NaOOIa2NgdYVicHVqOjYK2i+nQ9v4gmnuJV9mGbyj5JLeTjsTO9Jqh2biJVCoU4GLVHoHonCAR5muJcsb9nujfZch5cYj9hR/zruFBQRRhOtyBRhtOX/wHUcuVpl5GNMrcyMvBC4vxJSLCfdxw+g4slK8Rk7Ys7YefISZiI+yLyaUReOWYiqNLbMh2xFqzFjC8wZhJq9R5MJLOWKATE477lMXNryE/KwJ84N2SdWWR9nz2D45DdZ9DHh60ZjERokgmhI6LKfxiXonpXSAB6vsKfE9UfB+riB7cDsrSev6s3OQPkFQLvZuee5nSqHwO20qJnFdSq06ySJCQxYQ0tR4HNY6Ev1rqQsukd3gCfREuN+yH/5K/a3zf+eZPdiOjth6Fhigd/7ZEg0NLsDyiVdD0/iSwalKrZJU3SWcaPRT6IIMp4x/aEEj6K5eEqYDUD9GWo3ulWgJinpi3k2M3LiNSQSDjD+xVcwj2XU31Pdf6CJ3B8qM1e+1gJ9FgJBntbOnnF8t8TBxE/P/481ki4Ot8+dzWj/WCS8h3guF4HA+9EeTWqi+roqJeCvhfkyrHBE+dZNW0+84e2ABY73h5PbudECMtoJuVMr1l13CsRuFnhi3m1dw3bhIigABL7inA6u1ZagAv87qeaKeYcCYSKvkG3NVV3hmSqbgiId5c4M3AOSTKrhmaVTFAOVKGn1wHIrJnufTUXi6o/PDV05M4gcA4AraOVrD/Et16wKt6JGxn15ZqMGSkwPApRFJIO96ynK6M6siosUQcc+cIHeYmYMdrHnl0rWdM8fXwncEwvsF9G9YWIVCLvnY2x12D1EeI+rnjexSTGcvbq2+hLtz1pJHjSP3Nf5zPj+QqnAC8u2ug0XnN4FLhHkT90tc1++uTV946c/eBNfPM5b+Te/U/hgc5FMG0uDG8Fm5Rr0GUol2DtVah+HtVT6+isd5HYW7D6sLtdlg8e/y7+8L/vojnJkQvTBdBfZpUu3A7TsyURcKb3G50XIDdZlNjtaJ2CyoI679MD9Hue05CfWIWsCGnvgppH7Z0ngL0E9ldYMZVRSL4P9gHq9V/mvE8vV6TNF9L3NAOBjqf1qSlGwdgxsGolNk11+I+2rcWWHmfWLZwHUboyYIUhDI24hJLGeNOsDj1UPP0nMUV7xXW1bi+5PBNM1slr2q8mNs+tZwGPVFlfrjZFn+ynbsdkjIPRl27m+PVLufie7/HB138buldBbtDtkLl6/BZrbgZ5QxWz5/2g52EUhnv53KHnEcQxb157O0kQ0ZXuyAu6zIPVtaiGCudalyrkOK99HOyvNwBbRfWWOIi+tWrW4nvPevx3XHL/tVx54vmsWHAYf150FLTOhMEtlXrvEVRPw9jvA+fXaLhFKOeAPop1kdRic9w253BO3vAAGFMgta7AkUs/xLMvHcC31GVhWOsmkx4LmqnDP/RTgV+KyJO+8Xr94iVeU2q3bvv6uep2TY+scc92328rprrSis4RZFEdX70duCaApYj0+3oPFgF+B5C2qgco/AvwppoqrSpGLeTHHOzU9FlVAoaePlK5hHQqRbk4UwUknSLu6ibe2FUailZdrIXWFmhrqd9nJQJJ/CKgnWC7bPkm0FNJzAV+PNSS/kjRZVrdKfAcHBelp4CngSq9mTZacgO8+s9Xcf+sfdm01/Oh7xnIDTvQslyGSY4E2b9C+78Zq/eRJN8gicEGfOaQ8xiJWlgw0sXrNtxLLAHDUTNd6WlEmhjgR6jeYF2uon8VaCsiws/E8ajOjWzy3dEo86UVMxc8/va//g8t2R/xuRe8nXUz9uTugz1Zf7Cr3ApigYuIk2nAWVVXGOH1WP06qmudVhtxxTFv5VXP3MNo1Iz1/Zeofk0dJeCE3QCw5qvqhcBHHU2opmmUFfiEeBpApUHkfYEAS9UFS1+t1Q/qaKI+DtoOAhWgzBSZGH5TBgp+E7qsGLkamiXACoVfWbfjeEktm86gBbCS7QIrVUhHpPKGBcuW0TQ4ignDMlaVEFjL1plt9MzppEbI27Zeh2zOmY6pSaX4/nKltCx11WtSZH59KlJ4sNyjLGO51VvUHcH18Pg2kzIcNrNP10pOXf1XfrPHETy0YSk37Hk8A4uOgp6nIY6fwJjzUa4F2aPCKPk8IoME4Y+dM3GUrx18Pox2syk9g6FMB0f0Luc1G++nPwzZmu5gIGwaFLXvV/iTwjcEFpVkhhBRe6EoJwdq/l9vc8dNW5qFd9/zPfJByPe2rmbN9D2469g3OdNwsNuBq4iz8VVyaPw2jD0A5OBqpgzoiS7NBqAJKWv5wiFv5PzHbiCRgLQITVG0Ma/6JmPtx63q6d5Uol5le3JGT13yXCBE1TCRYlBOVonwtUikEnG0RKcXgCcT9P3G6m+pzKhv2oWm8SJUqyHEgKJfRCQXBcHYeQI16qhW7eWJcpZUOQfAWMUaH26TmNCZWTUntpkwoVMR0XCWeWu6SSVKrqO1rPKjIgSq2HRq+8AjFLdhtvvKisioPuad1QuK1cuS1K+XKNyicJ/x/BxVQz5qYumMFg7qfoKXrV3Kfvs8yLpZ+3LVYWdipy+B7qfuI47fgOoPgIPKzKdmkG/jeI0/xCqMbAIJ+cpB50OQYV7vcp5omc/Glnm8Zt3tnLB1JStbZzEaRr8S1S1WuVZgDx2br2N12Be4DrgoUP3hpvbZBGp5z90/ZFPrNA7bsor75x3M3w49A3rWuNUv78N2jO3D2stQvb4qI99yMsK1boBZFMtQEjOYxCQEpIKAWJVEdV0o8vaUyHOt6tF+06DdoNOAWZ6Ok4hz2C7wjttR6mfBTwasFvp7bqJMdoISeUKVESPO5msKIxzBRMs+eVSN4zapbrTKGuCQ3WCQ17KH+gUeyqtiVEkFbpExZSZ8IMKITRw3xDJqHFVkYWUz0GKsdTvmcbyVOBlCgrYanThe3iggGskyb3MfKWOJ09U3EwUwgThflZnw8Uy/QCQVfJohIt0YM+zoxLtRWoixOS0PRQrrVPV3wFt0W+dkQaYLfAF4ucBgcZhkZA1D6VYezkzjuese4PQn/8zcnqfoap/HN444B9Kd9zOw6dUol6F6XhlHXBNwFcqewJdB+gNriEa3EFjDpsx0vrbvORCmWdE0h70Xbuata25men6Qvqj5PtC3qAs4nlYmrVQauAq3e3irlYCnO+eQMjHvuO8aXtHSzmcF7jz4FFi/jjEintPPfkli7sHRGyrZGftgbSeqW5GQllw/lzx0DUPp1rHMmIM+PW4kQhIE96vq/UVmSgDMSQcSWDCBc1TPAtICWXVgtciDViEZ2gIm7g5ahU7UHo3wsjpGWrpI46mliK9FPbkRJVZLZypDWsIxwPLgxEASE6uOBxvXjqbZVVlLayXq0wCJUYjVkrOWaakUTUGILUmTNBDnyXnntvdDV62DEYtRK+QTJZ90ESdbaoBVhOPzQSikRmLmbh0ilVjiVEhYIzGlCYRpI3nyqYhcJkKMReEIgc+pW6BMeRuQAOUTwC9cEr7dS6US1Ts0Ch+JVDVRuMHnOioy9Lax/5+PyzT5DsqQ8iJr6G+aRm9zwAvW/oXIxCzofYZ1bfP51pFvfoKRgQsweh1qLhR4lW47gf4Ll8/6CuBXQKIS0GxyRLHLCPrItL15ZOYRrGydz+ce+QbNJsdokL7DKh9zQLdtyf2u4pdwjuWNokoSRDzROZd9+zZz6LI/cef8k2BwyFNHbTEMXI/q86lMHt0X1XmgWwu7v93N02ky+YJiPT8U6QASC+Ssxe/PFLhieYGNeYV04AwMq/qM+lXcr+5/qXPXa7q1ernCO+sAiXq9BbZ01m/MjjjukBQ/X0gFIUVZ0GQSIXzP+lxI0FSx37grN4qxus26mgrCidXW6rwtYxVjrZJYgsQGUWykDjNwXiYxNMWGKDGkjCWJgjqidMCKkIoNc3qH6WlvIpsKCa0epPCSOjrjcOAXuxVQCYglZwL5lGL6C9SFu1T1ToWTKjvaFeAcx9zl/bjE89uqymrpbXLnoT5/w4MID4JN+NZRF8SMDt1MfuRuQY4OrX2lwLEKx447euU4kJ8g+hDCDSh3A/c7mzygLRlB4mEeb9+HDxx6MR99/AdMS4bJE37XomegvLQc0AocoOh7repHKMTNqeXp9g5OWv9/LL//x/xhz1MI4/7SHy4XW5n0502AxcBjVlyO6IL/TJ1v6ipvCtkKyrVR+Fps7FdHkwRFaQ4jAhH68jnaUmkyQVA+tKnYNDEJidXelIQ3KnouLgK/mt9Z67QdW4ufk00ShpPY7UYVzWQBWqIUmSAqwFUo8HdzeqxFpVDHxCQMxTFGFSmpY1NoaQ5Txb6lfFWwUr/25QwSm6A5MYGtBVbC4enEzGjLxj1WIJlkMjwTBgRWmTGYJZeOyEXhbBOQiNYkxM7z43y3OS9MlH4C/n00jG7DKJFxtlOfwqcoA1aUOPvVaT+HAz8CvoU7NaP89klTB5E1vOyZezm2exn3zDyUHxx6Xp9m+24XuCtSOlH2R+3egWVvXFD1vghHgOyr6FsE1qg7nfkeYKmKrG+Lh3R52x4MBilajMEEMqzwac8lKssdU3iHOhrB2HZ5NkixZKibJYPrwEY0bXsI62ZVtlKZ4R54Ew0jkEoMRi1WrYu2d2BWi/B2mLGWBAsKI+qOLjOqDCcxo/VMNp8UjoA26k8vDj6vVTXntCAYtcRqsdZ6wvG2j8hZQ6KWACGUYGEozNf6yjDVUmu2pwOYY1S7RtWOgVS5BHuxKtbELt5JwrZAKiZ5HPNZWbUQG5piI81xIrb2buABolxpguBSRUe3C3wDIbCQSswMRc8SrSuLc8RkDcCp08KWC9xgkV/kMuGDhdWieGL/QZWvAJeWpzJM+HQxLj3u2QK/VvgDjlnex/ixUh1AKglCq8jmBUOb15/d5zJuXH3wOcT5kTgw8ZZAzRbgHnGDNwPajNIOmhLnK5utjoT4DG6bXBVoNVlEbbGasNS6NC4nVWjEDtzJPSvGtUAlG0IiacjnSW8LVj2489bm1tq9MaKkY4N6R611nw/U0TEnK5waIH9D6FdQfO4lVa3rHCRxSkGrUX2N1M5pFRb5vIZrfPdoxZ5krL3T1lDFCr4rl5xRTzbK9Cr3jZl4CMRUSi1AnmHgDKP20YIGW18d7clW5bjqmpWiqiJqNTRmuClv+o3o4jqs+gvURXH8Bpd6O/Jza32FbjBM9G9aG7BvYPXMprw5us52embS8CPchrKW7Y8OKNRjk59n62woKxMJnhLYikwc/qVHcX1W0aOAF9ZR6gDYG+Xd6vxYee/IHvW7THMLq6egK4ZSLZ8Mg/Qvz151M4mE/GavlxObPNkgg7isMsb7wkao45y7XJh2avZ4BoheHLHvpEqgr/BCq3pVsdM1DwTGEsaWTLKNBjwNqk66In8BpBMXuBqoxUes1hOLtg/wS3Ha4//5+qeoz04rBGfPRjnRYPerQ7MaLQKpWoHW8wSuQuSDonqHuubSIp9bYRxEOJZOM+jZRu3FNe6b9YNzilUqQYTuRDUnlUNjUri4wTXiOGIjTPQrFtcxEOcDPcuqfkzRqPrzXTjZ/N4tpG1+QFQ3R2IOrdNbcyxu1zgpMnBM5R8okdGwqP8j0MnYkD2ieIO4LhMNE8lnbBjcuRP8k7boqqi0RclEf8gGhQ+gXCfo4jpCcQqfZzyitxd/WFSDo0T5ZiJhb09Tx52vXHsH71hxAx8++lLuWvQiWkY2MZntUhVh8fAmQpNgVIqDvZf6YkqFZBJHej9M/0Q/m9OKUnFpamKdpyK1qANhAaxSxpJYS+AOFxim/sNFW4Dj/TXVshHo8q29SpmYYLqM7I/Iz0AfwOWmGvUrYSERXxMwX9E2hUMt7FdH9H1WfJD3VPtzBfrV9Xc1XtdC4McIS72FMODrWPBJNfs6NgN7JXBMPRkGAmtILKiJSCUjgyjrAtXJ1DmgvvClHXNiA4qsHecraT0/I50jn2uK4lqUip0l5VaG+60LJfgfb+7VhVjluqBET5kLvBPkThUhG0YcMLCau8zzyIbNtNrRugBaVBlp6uTSB77AnOxWhqMJIQKbFM1SmT80U50P6eFijShIDJlsTLqEFKciC2xQ1bdiCif9WAkIkwTr/RSK9AOP4VIk707yGJAXEVCWKzpYwyEPSgZ4nr92hv9ik0jwV2fxTvkoX+2vOTXngnIM1eIIJ1nURCGdDLPXwBNsTc3JB9i/iNYM5dq14ur0jI1kmQ0CQmNrEmOLwbRlMMdoW5pcU4rATm1fBhUqcK8qr7Kq91hnd49fqKc/ll4TdsLUus0QSq59FJoV2Nw0nTev+hUXPv4jbNjEaNhUVz6rOExxcPcjpEyOXBBiscXlMBbiMZ3Sm4hFl6hqW6EuBiFtoT0/RDpvycTJhCs05iVRYsIocdvIpVcqNmsDY58OjKVwlcSB/p6dc/z2zpRbnE2hKHofldNAT2kZVG2yi85S7MadWDS1070cWAVpZue7eOWGaxkNW8Blmn2G3U9uDYyuiPJmMkDlKh8IzcN50tkYDWTXg5V7pD6k8EoLVxvImyIHiS25tGBQu2u9Ue636gLOrbXF10xr7f7WWsQkrG2ZxTmrbuWCZT8iiZrIBZmqgBWoJdu2gLc88XMWDq1lWCIStWOXURtYa1OF5+m2ZfVZp9z7tMnyZNM8lrUcSJMZHTsdzF/7RIk9NRUbKl1RYldGxm4MjFK4SuROxvhfu4V8A7cZQpHP6pNsz0GvO6bpfL26El5LSa/LB1Is3wbu28n1ML4tu6tMZasCA6kORsJ2QmvXAB9kN6IHIKxH/BiVSeuQWnDLtHjAslMIWLUccL0o56vqm63qbcVaVgkIoV7Hsdj3WOxDFpt27yeARbuF2WOeNGtY0zqTNz71Gy5Y/mPyqeaKJ9sAZFMtPGft7bTFwwyHrQToeN5e95U5xSagqo6bZa7cgVV3CnWiMCM/zMrmfbiz8yQ68v2I1eLrg2J0jhioeFkeCmM7kMobUnkHYCXdbICPA1/dDYblT/xEKZWbcBskG3dBGVYAF5XRLmrtYHaUrKW1DjcojeLfgMuksTMB62PAR6gestQWS4rZuS6O7b2bbNgMcA1wNu4osWdbbgdew7acyWbqO0AiU+xHbhnOk8klUwZY9WZO/DkuNvDVqno28GIt3V1xntrfAb8A3l5BU+4AFk+Y0GpY3zyds1bfSqLCDw853wUWl1NHW+dw+oNfZkn/Bja2TifY9juHleYaG88RZgHiQGSDFaEz6WNDpoO/TjuW+bmNGJngmjoVeEsNF1oM+sex6SMQiGVa0s9oOGH8DgGX+oFxmneiHzTljlMnA7gUzD/H5T+vpEFdjTu95R04Ht28KdCm/tdrOMvL/D2c5DhNbce4XgmcCbzXT9D9trMua3F8vU/7vqwaPhNLmvnZbp7T+yfumfES9hxdhZXwWtzO75uAFwCH4pz8u4L1v8E/+zYcV3JgO5SYiUbY2BwTmodyLjSjKUVgdq4CKdd3zt4GVtRCQStSR/wZ9zuptuFY58co+jxgCS4/1WyQV6pyM8JjuHxFvkYTQOXbqlw0scaCoLTHQ9wy93jiMDVmOxfHKCZhmkO2Psb87FbibY9sbxPhRlFerCX3dnaBArJCRY5sSYazubCZby9+F081LWFOvqsYrPbH8Vv2qtF293hQGyl0VIByRP/9vHDLb6lC/luCc/LP8Ndi3A6l4ngy7ZPwc4mf6H24PNqFMxjWA6twu2DLmdx5cEf7chzkJ9BCxpMvBl57LU181OfNoSb/+Ub/zDW4Xb91OL5QJTkOl4poqMREKgT2PgncUfT5ybhc/r1lJlnon1VNizoQl4X2cN8fc3y5W/3rwr7YFsbpNKtwIVurGd+gWQC8mJLklEXa3wrg7kgTskErt805naUdz6HFDJdqgXu7+cMMXGzoXoynsV6AixqZjO8zxOXiKvCyuvzrbl+n5ZScq1AiBzN+IEc5ukTKP+MmHF1pvOIOMBhty5DPRJP2gdlQSPzcsSLkMiGjqcjN3u0Aq6LvaiswQx3PZzZwh6KdKPfgsh6Uk6cVTlX0iUKvJtY9QVDassMVl5cA6IsiSDcTbtsGpwC3VtJY1Pm8bggxb8oFmfgHiy7imaY9mZ70YscXkhMYD5GpJW8Hvj9B1ZIMGR3lklWXY7cryRoZP+HNJMAqwNEJ8lO0Erf5gVlIs9ru27i4B0Y90BQ0nkF2u3DYqm1YWCwyvr6FNXLIt2t+R9pXEWbmh1naeQg/2PNSZuS7J/PztDfLJpP9KfBadO5ZaVCjJOmQoY6mSe8QVgOrHU2gP+yvZ8Dtvila64SlJcBZqvrZwgeJFvYToS/TUkdjmLE85+Lo222CjB3mWI5GIUDaZm8bjtrinyy8gE2ZhXQmfQWgynjz4NI6TaA7gJ9u+wxLazK4I+35rA2wKjJU4z0loPX3JlpUp2Gm4Ah1QYkDGAnbCHTSptEOAeWzBf9TsTMY7cTyFXp+s1c1t8kQWqSVXYTbvn7Ujq09MqnRVRyLq3Cmoq+a+KwSvdXmzU1zX3vXyraDGQqn0WYGWy1BQY2/EDiC+uLVutj1O2gNacg/veyUyHgXM+XoA6pqVXWZqqX0GjMzYW+FzyjM2Qm2wgtU+awqwUQ+2Dj7yqJE1vyiOz27c3Nm/qczNvtdS3Az7qy+L3s/Tb2BtV8t8Z+MI78m9KZmceOCc8nYbGN0NaQhu5tmpWNG3Nj7Pymc55nPJd8ce306jvfzTmoHm1aSF6nq9xWtGP1uEKbHw/pwx5E/39i0+Nz2ZOCCHQDpq4Arq7WEkZDBaBryd+OyaUhD/gk0K5cszpI1CUbtGLtd4deq+rSy7T8mvOK1uC31Ayf56DTwOpyDe+/K0CE0WYMJghsf7jg2P5jqPCOl8fbW+X9webxMLf9EqKYxshrSkN0JrBTGclWXhNX0KnxRKRvyMjF8R/U0XBjC+3A7iNVOImnBHXjwWRy5ripQqcCcONv/VOt+X1o27fA3tiRDC2XyIR7WA+rFlBxL3pCGNOTvwAwUgdhYskniTz7ZBgS+j+MhvbaO2+0NfA53tNHdwJ9xAbcDHizacA7w43GBwZ21EUbojIfYkGl//52zTt0rUnNWpPFks1l0+XJ9vjFUGtKQv1OwKjYFK0gCvAdHMDuoztvt6a9zPEgNebNrGnU6wBXBiDA9zjIctn/9R3te+NdVrYffNDu3sTXA1gtWimP4fgoX39eQhjTk79kMrEPWAW/F5QbanrIVkt/VDVQKdMZDzDfJL/4644SvLWs/6oud8dZFIUk9QKW4kKGzgZc3gKohDfkH0qzqkPv95P8pVM9ZvSPimFqWlMkzkur4441zjvvdvTNedN3M3JbD0jaLrYx3hfCDPwP3eo1qqDE0GtKQfz6wAsdLeg2OqnDUVD0kY/OkrfnBDbNP+8Btc886bUZu8zOtycBtiBQnMhRvWvYyHrO2ml2QZrchDWnI7g9W4AJLz8Sl1XjHFNy/N0A/CXxjS3pOLq9cNy3u/ZkJouTv5wi7hjSkIZVkV5/tthaXnP+1wJ920j0HcDuPL1d3mGkuY3OkNMlaCZJGFzekIQ3NanslD9zgweoFwBuB09k2YVotWYpLV3szLmfTaKM7G9KQBlhNhXQDv8Ttvu2HO3nmRBzNYT7juZEKNtxmnG/pIVzysKX+HnGjGxvSkH98+f8DACeR8Z8W+T8oAAAAAElFTkSuQmCC" />
  <h2>Bandwidth Report</h2>
  <table border="1">
    <tr bgcolor="#66ccff">
		<th>PackageID</th>
        <th>QuotaValue</th>
        <th>Diskspace</th>
        <th>UsagePercentage</th>
        <th>PackageName</th>
        <th>PackagesNumber</th>
        <th>StatusID</th>
        <th>UserID</th>
      <th>Username</th>
        <th>FirstName</th>
        <th>LastName</th>
        <th>FullName</th>
        <th>RoleID</th>
        <th>Email</th>
        <th>UserComments</th> 
    </tr>
    <xsl:for-each select="//Table1">
    <tr>
	<td><xsl:value-of select="PackageID"/></td>
        <td><xsl:value-of select="QuotaValue"/></td>
        <td><xsl:value-of select="Diskspace"/></td>
        <td><xsl:value-of select="UsagePercentage"/>%</td>
        <td><xsl:value-of select="PackageName"/></td>
        <td><xsl:value-of select="PackagesNumber"/></td>
        <td><xsl:value-of select="StatusID"/></td>
        <td><xsl:value-of select="UserID"/></td>
      <td><xsl:value-of select="Username"/></td>
        <td><xsl:value-of select="FirstName"/></td>
        <td><xsl:value-of select="LastName"/></td>
        <td><xsl:value-of select="FullName"/></td>
        <td><xsl:value-of select="RoleID"/></td>
        <td><xsl:value-of select="Email"/></td>
        <td><xsl:value-of select="UserComments"/></td>
    </tr>
    </xsl:for-each>
  </table>
  </body>
  </html>
</xsl:template>
</xsl:stylesheet>'),
			(1,'BandwidthXLST','TransformSuffix','.htm'),
			(1,'BandwidthXLST','TransformContentType','test/html'),
			(1,'DiskspaceXLST','Transform','<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
  <html>
  <body>
  <img alt="Embedded Image" width="299" height="60" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASsAAAA8CAYAAAA+AJwjAAAACXBIWXMAAAsTAAALEwEAmpwYAAA4G2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMTQgNzkuMTUxNDgxLCAyMDEzLzAzLzEzLTEyOjA5OjE1ICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgICAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgICAgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIKICAgICAgICAgICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmV4aWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vZXhpZi8xLjAvIj4KICAgICAgICAgPHhtcDpDcmVhdG9yVG9vbD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC94bXA6Q3JlYXRvclRvb2w+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMTYtMDMtMDFUMTQ6NTE6NTgrMDE6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE2LTAzLTAxVDE0OjUxOjU4KzAxOjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8ZGM6Zm9ybWF0PmltYWdlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3A6Q29sb3JNb2RlPgogICAgICAgICA8eG1wTU06SW5zdGFuY2VJRD54bXAuaWlkOjZhNTdmMWYyLTgyZjYtMjk0MS1hYjFmLTNkOWQ0YjdmMTY2YjwveG1wTU06SW5zdGFuY2VJRD4KICAgICAgICAgPHhtcE1NOkRvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6NmE1N2YxZjItODJmNi0yOTQxLWFiMWYtM2Q5ZDRiN2YxNjZiPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjY1NTM1PC9leGlmOkNvbG9yU3BhY2U+CiAgICAgICAgIDxleGlmOlBpeGVsWERpbWVuc2lvbj4yOTk8L2V4aWY6UGl4ZWxYRGltZW5zaW9uPgogICAgICAgICA8ZXhpZjpQaXhlbFlEaW1lbnNpb24+NjA8L2V4aWY6UGl4ZWxZRGltZW5zaW9uPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/Pq+oh5kAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAALU1JREFUeNrsnXmcnFWV97/neZ6q6jXd2XcI+76DoqACKgq4gPsAggvIoDjovOq4oDMqCu7buOAuKg4iyCCCG4uyCI4CYQlJCIGErJ1O70tVPc+95/3j3uqurtTWSTpErZPP80lVddXz3PV3zzn3d86Vh+YtoSABIP7/QGTsNf5z/OfF74VxUbYVG4TM6e1i9dw9Oeb7D2A0gO5eCIPir4VAB3AAsAQ4ApgDzAfaAQP0ARuAx/21GlgH5KglxoKWKV0UQv8gbNoMYehqYP1lDFjrXieJu0eSQBxD03Quu+cqPv7IDaxpnVXz8WkgLG1PEaZK1NdVAev/N4Ap1wa+D7cYg0XH+5kdL1+iyoJUilYRDLuXKLA+jjElYxggVmVuFNEZhiQV2myyMqu/i2+e+CbeeeHXYLDLjbcoKuoEgVCmvuKJnwvqx3U2B81N0NrsxvtUi9XyQAHjz1c//2B8zgHRszxmWoAjgVOBFwInAqk6f7sK+A1wC3Av0E9DGtKQf1h5NsHqROBtwBs9aE1W9gUuAf4VuBn4NvA7r0Q0pCEN+QeTYOofIagEpJM4UBEQmQ9cAdwBvHU7gaoUcM8Efg18Gdij0a0NaUhDsyqVVmAezr8027tIImBPoElFrBXpmDbUG62fPueKKI73ygfpryM8f0pQ0WlazwfeA9zV6N6GNOSfG6yagROA4/z/ewILLEz3JlgIiKhigpDFXWsYaGm76rK3Xf4SNXyFwcGFBFOq0B0N/C9wDnBro4sb0pB/PrBKAa9T1XOAk9RpVaWO/bH7mSBk4ZZ1jASpK8993w9vufWs11zNmuGFxPmJuyBTI9OBa4CzG4DVkIb8Y0i9Ks4rcDtvP7VwhoVW622+4mt8q1wJk5iujtnXveW937nh1jNe823WDC0hl4VUalfVrRP4qTcLG9KQhvyDa1bNwGeA86zjQW0j5SgT2SjNgeufXPG+8z/xlV+d9cZP0TVyIHEOUmF5vtOEG+oAIvcDDwNP+U/TwN6oHggcj0jbJDSsn4CeDKyhmNukCkHAFJukDWlIQ3YBWO2j8B0LJxd/WI42JkUkxCQImdW3Jf/AXod87OYTX30GvfFLGRp2oBBXZRX0IfwAkRsQWQ5sLcHCEJiO6gFY+1KUC4CFNWso7AX6IRJzKarjBNIggP5R6Ol3ZulOIv81pCEN2bVg9RxVvVodo7ymJlUQE4TMHOwhH4a/uOiSq3qfWnDg29iypfDXauX4I4F8hDD8CyJxpdsD3e7SezH2Oqz9KI6nVQ2sAN6O1Ru9KTuuWcUx5PIQBQ2wasjUiiqB2kY77EywEjhU4XvlgKoWWIlamnLD9qcnvO6Gx/Y49HwdGJhLHEP10JJrCIN/h2jzZLoeeAz0AhLzFPABqvvfIuDVWHsHkEPE0fj7B+szTXeuhL48nd5M7QQSoAmY4etRb4HEK7s9QN4D+hZ/5dkNCLJ1VqSD8d1kW1K/EBjymnZBZuPCsLJMjJYpfH8A6K3VaPWUWyrXQXx/tQPlFtiUL0ePqBJHKUbSLZXGWlg0LjqANmBmUf/NwG1oTaY/A//8QV/dLX6xz/nxVq2Tip+vO2kYpHFRJsMT1IiJRputVsdSsJqr8CUcYJXtqDIlj4EhK5JvHx1JNnfM/d53XvaOocSYkxnqr7Xa/JIouhQJurerCUSGgI+TJAL8R41vvwTHCVszBlbW7kqfVRPwXOA04CBVnQ/M8oOiwE/b3t2HvL9HgoufXAU8ATwI/L5kou+6lVCETUnMwjAiI1ININ4FXOgHsymZcM3Abf47BfmQ16g3lgz6woS/AfgYVVT0LmPKxgUCpETYmrj53FG53K3AfwJn4OJWS4EsA/xaRT4wZ7CbWw48kQ+/4WPQt7GwGx4Ac3FulqOAuRid48dDh/+bLbqXbCdIFBat9YzH1v4Z+C3QRxjA6CgEAplMIT7vdcB7/T2SnQRWTX5R3eDrXhp23OPBdC1wJy7ud7AiWCl8SN2kHnuCVl53RoDlwM+shH8SbO8+fZtzZ/zH9WsfPfRFV7H+yQWg1Zr4UZAPYG03xtTSviqMOgPWZrH2Sqw9FOGMaj44rC4G1mCsC+DcdUA1A7hC4UIDUxGtmil6PRM4rOj9Qzhm/4/rUCimRLPKqhJVB6sDcQHslaS7BAiWAAv8VU6WVypLBGwyCXl1yFZRLRGh2xhsENAeBOXKnvHl3rvKbVYH1shApk0fmb8/2XDMN9oJ+k7i+K24sLGpXiTxmuuhuDjcS4F7gI/7xQziZNw94riThz+LCvlW4C/Af+NC6BKAqEjdfami7y50qhT/vy1iLQM+C1wjQmyCkD16NrFq+jy6Oue2MTj4Imf+VQSDmICPI6wiZyCbhc5Ot+LUE/kdhG6k9Y+CWhD6UK7A6otBmipq/8ICVJ2fSmSXmH8C7QrfS+DMZ8MrJi5Q/LtAi8A3y03ggKlB0IKq020NFshUXhyGatymv6TItVb7gUp20ajqmLpST9l7rMUCLUFQungn3gytAtQyNHeoj3v3PIwP/svl0Lse0tEMrLkaa87g2XWTnoBwDcjrCcI7SYybS05GvcWUepbKNhM4DeE04JOI/BdgI+sKOE2Qj+CUwQmdUka7ulHgvQJPmyBkWn6YhV1reHrmQl777qtZvvDgU9j09DyCsNpCfguB3Oye5tFwNOuAasH8ykueAH2j0NfjUrYYO/4M5W8Yez2OuV7pBrM9GBZ8DZuoJ8VMGbDsyA0SxKMYCQirOE4VLk7gTH12HfiRwOXAbQorS5t0VHVKnVsB0GsNsyprz1Ibc3cYtMmr0med+RfVecsQ6LcWAzRX1w63/a1NZGNrJ78/8AQYHQAJMlj7FRJ7xs6p1Q6rvbMQvolyAlZ7HIo/24UqWZaEjwKPY+zPIq89vdjCi4rbT4sgqmia/UKQN4tfUdpzwzw+Z29uO+hkvvT8N7Fy0eGwcfUhhFGHA5KyYgmC69EgS1J05/wgkclz8oO/JVKLlmm09uF+rnvZW8GmoGsdpNPFf84C12NMNbDqw5hWovBHwGKvIa4Alno/z6N1NWJumD8uOoYXb3qM2dkBRsJ0pQkyLxA5dzfZZ5yh8Bbgw6VAstUmqAqpKR6no9bSNMkJXwF36uEHbuPtHrSWRCefNioAhqwbk5lJlL85n+Vviw/hitddBlvWggSnkui57EZ4ABxIkpxJYr9PKirNM7e7ANZl5JM/ROr68awCJJkKo0PhfnFO0Cw4msI+PZu5/pBT+PD7roEnV8CGJyGKFpJUXafXEOp9YwnAxpahiCTTzpVfuYhpSb5sOaYDx6y8jw/+yxeheRrkhiauBMpjWLMRZX6FZz8FHEliXuHfH1H0t6e9/f5b4Eaq7byM9HLzIa/ikK4VfHjp9fS3zao0yA+J4LDdiBRxlFcWTLFtHLJrkuP1W4sJAppEdtQCqpWpo7XU/h9SS6K63WlGAmDYm5CpOrWPJAiIkjxsXgdJHpCTnJ90N6PJqBxDGHzf+YB3ywxLB6McGVllD3FOtwlaVEkG0GGE9wn0CaASsGCwm0fm7sGvjz0LnlwO2SHnS7I6o2pnKA+QJKvGHGKFJ0ybx7m//296O2fRp1pWs3o6jHjN7deS9A5w2blfAo0gN1J8oz5EViJlwSqPtQlB8G9liycswYHxhbj8WN/zoFXeZ5YfYr/+9TSbfFlT0AP8vnFdg0XB5VDsBYakvjCogoXeAXTWqb7P8/6ArkIZR9SQqBJO8XJfuPugNaTCaLu0K3GXAt8BHmGi472AKSng/ybYwMCoWvKqpAPZoTqMqiWQsC64SYDEWkhiiJNmJFiCbAcYTMqFIEULeN2/O9y7RnrYbUWOjBQ9BEdZmDCoSgbTNSh3F77Tkh/m4VlL+PfXfoJlS46G7jUQRsVmfjVZgYhOaMsow1tv/QLvuf0qels7SYLyWn5kElbPWcSZj/4G87OI/3z9p12JTVwoeS+wscLEXY3qxVj7+jpa5hXAS4GfAJ8E1mxr0/TyzSNex4KRHg7sW89gqqnc2N6/roEmcrW4Z/UDo0H93gz1cZovFWs/gOPH1JpvqfGOEkbVYph6sCogiQEGjZm0dlXgRnm6wfW4q87fqQPkHfTHFMowai2pOqAg8b4ugmbAdpLk5tbdzFYB/oRINyLGA0muzPwqLFrTgA5UFdV24EBEFtX5vCavrfZMEkQHUUaRSWdvSYBOlDQidY52OSBSOF5Lal6CycZP2rG/hEmWZTMXs2yPY2DjKqdpmHhc564+MfMTTTcLzTM445HfkU01EQcpqg3j0FrWdy7gVY/8FlHl8pf/P/JhCtSA28FYWeGn04E31wGmBckAbwc5Fpcn6+6Jetowf9vreax96DqO27yCnlQzwcRyZ3DkxVpyWwgXCWSlaFJPzqTnPuNyil1S4+tRAdWdlmAwqmN59UtkD6+JzfL/p/wgU8b5YeIXiE2Mc5wMjitT+HxtaXnzqqS3w3fliGRaP5L7toxVsW5mlPbPQcAiHKdJfNnn+PoO+LILjqu03Ps0R3ybCSBadUYqBoEgA2SnkcvNdxtPNUv+CEHwPpClBAz5xX2kjr51K6a1GZS5oG9DeTeOkFlN0mzPzp/wQUTuRrV5kr+MvTVwJKrvQMsT0EvaZI9IXYdVhAdFHxBk+QSzUAKa4xwM9ri+LKYbqCY10DiY8P1MK5fedDkzR/sYzrRQz3orqvS2dnLi6r+QJ3SDIT9cKPCyCrWZu50L6hGIXANcjMtG6gsRQu8zfO6485ib7eOA3nUMpzLl5lcteSJvNbu9fgz1GlIo3GNcmuiA8ozqdopIl4Uv2Yno/ULgtcAxis7ymlqzX3UL27vKOHenIAUmecEhM+g/G1anlf7Om9bLC5rOqFpS2+G78mX9T+B0xtnQxcqb4PLyf8ahj6JM8FUdBbxT0aP9YtJW5AMrrVuuaMHuBTYL/FLh83m1uVDEVg0/A6xaJclDkk+TJE1IzSExCPwXmfTvJqnsJhQoIIEMgWwln/8PjHkOyAvqcA/MYjxxQH06fTr9F4Lg4R2gAN2OSR7C6PW4SI5q0hxZdEYNM3mZogMyQUNVbLkCioAxXTW4UhO1DYk4fP1jTMsO0dPcSb3DNwlCrIT8+KfvZOWsvfnkKz4CSQ6S5C6S5G+IHLPzzGVZDPLdIn+WH40xT87ej82ZaRxkE4xmKrlqqsnJBj1e0fu2t8sTlFDllgg5qWieFLl5EL+R0gd0C5Dz2oZ34eyj8FGrnOEH7WRBpBS8Zha93l8cS/s9wA+BTyrkjI6j33b4vp7jr0rSTZGqVMQbvMwq7wLm1Vm/TIlTf6HAkeo2Tf7NKj3VOtj4g5KIE4gTS5wkNYnIyuOEwS/J5twJOM1Nk7S1AxjJQT4HiEHpqq7/jYG8TopLoYDJttHc7AhPOrmfjw0wq/eSJH/GRXZUExNZrT4u1anCE8hvRgRrrSNzZnOlKLea8Yyh5eQwb5K52K0kYTjdgglCJuvFCNWwV8869u9aTWgsl7/8QyTWbMCYj6J8B5E5Xj0uuDtM0QIdTdLimofIlxDZBPx1DJyHt/ChEy/m27//FHv1byQbpYtV3aE67nuAwLXi0jAP+t9t8Gp/xq+Yz+CIeoUwhRDY7M2sCEglqhsT9G+UUOOs/0HGE3SdRqXE4+bUYkV/lqDHTaG7KsJlyPiQIDME3qtoLr/9u2K1TKLBUpcz8JkE+152PJV3ALxBHAt9pLoZaEnUQj4P+VhJYq1ClC4sjKsIUoq1447yyRKYVSExDuysfhXltirrTxoX2vK4c6Xo5NaOOIa2NgdYVicHVqOjYK2i+nQ9v4gmnuJV9mGbyj5JLeTjsTO9Jqh2biJVCoU4GLVHoHonCAR5muJcsb9nujfZch5cYj9hR/zruFBQRRhOtyBRhtOX/wHUcuVpl5GNMrcyMvBC4vxJSLCfdxw+g4slK8Rk7Ys7YefISZiI+yLyaUReOWYiqNLbMh2xFqzFjC8wZhJq9R5MJLOWKATE477lMXNryE/KwJ84N2SdWWR9nz2D45DdZ9DHh60ZjERokgmhI6LKfxiXonpXSAB6vsKfE9UfB+riB7cDsrSev6s3OQPkFQLvZuee5nSqHwO20qJnFdSq06ySJCQxYQ0tR4HNY6Ev1rqQsukd3gCfREuN+yH/5K/a3zf+eZPdiOjth6Fhigd/7ZEg0NLsDyiVdD0/iSwalKrZJU3SWcaPRT6IIMp4x/aEEj6K5eEqYDUD9GWo3ulWgJinpi3k2M3LiNSQSDjD+xVcwj2XU31Pdf6CJ3B8qM1e+1gJ9FgJBntbOnnF8t8TBxE/P/481ki4Ot8+dzWj/WCS8h3guF4HA+9EeTWqi+roqJeCvhfkyrHBE+dZNW0+84e2ABY73h5PbudECMtoJuVMr1l13CsRuFnhi3m1dw3bhIigABL7inA6u1ZagAv87qeaKeYcCYSKvkG3NVV3hmSqbgiId5c4M3AOSTKrhmaVTFAOVKGn1wHIrJnufTUXi6o/PDV05M4gcA4AraOVrD/Et16wKt6JGxn15ZqMGSkwPApRFJIO96ynK6M6siosUQcc+cIHeYmYMdrHnl0rWdM8fXwncEwvsF9G9YWIVCLvnY2x12D1EeI+rnjexSTGcvbq2+hLtz1pJHjSP3Nf5zPj+QqnAC8u2ug0XnN4FLhHkT90tc1++uTV946c/eBNfPM5b+Te/U/hgc5FMG0uDG8Fm5Rr0GUol2DtVah+HtVT6+isd5HYW7D6sLtdlg8e/y7+8L/vojnJkQvTBdBfZpUu3A7TsyURcKb3G50XIDdZlNjtaJ2CyoI679MD9Hue05CfWIWsCGnvgppH7Z0ngL0E9ldYMZVRSL4P9gHq9V/mvE8vV6TNF9L3NAOBjqf1qSlGwdgxsGolNk11+I+2rcWWHmfWLZwHUboyYIUhDI24hJLGeNOsDj1UPP0nMUV7xXW1bi+5PBNM1slr2q8mNs+tZwGPVFlfrjZFn+ynbsdkjIPRl27m+PVLufie7/HB138buldBbtDtkLl6/BZrbgZ5QxWz5/2g52EUhnv53KHnEcQxb157O0kQ0ZXuyAu6zIPVtaiGCudalyrkOK99HOyvNwBbRfWWOIi+tWrW4nvPevx3XHL/tVx54vmsWHAYf150FLTOhMEtlXrvEVRPw9jvA+fXaLhFKOeAPop1kdRic9w253BO3vAAGFMgta7AkUs/xLMvHcC31GVhWOsmkx4LmqnDP/RTgV+KyJO+8Xr94iVeU2q3bvv6uep2TY+scc92328rprrSis4RZFEdX70duCaApYj0+3oPFgF+B5C2qgco/AvwppoqrSpGLeTHHOzU9FlVAoaePlK5hHQqRbk4UwUknSLu6ibe2FUailZdrIXWFmhrqd9nJQJJ/CKgnWC7bPkm0FNJzAV+PNSS/kjRZVrdKfAcHBelp4CngSq9mTZacgO8+s9Xcf+sfdm01/Oh7xnIDTvQslyGSY4E2b9C+78Zq/eRJN8gicEGfOaQ8xiJWlgw0sXrNtxLLAHDUTNd6WlEmhjgR6jeYF2uon8VaCsiws/E8ajOjWzy3dEo86UVMxc8/va//g8t2R/xuRe8nXUz9uTugz1Zf7Cr3ApigYuIk2nAWVVXGOH1WP06qmudVhtxxTFv5VXP3MNo1Iz1/Zeofk0dJeCE3QCw5qvqhcBHHU2opmmUFfiEeBpApUHkfYEAS9UFS1+t1Q/qaKI+DtoOAhWgzBSZGH5TBgp+E7qsGLkamiXACoVfWbfjeEktm86gBbCS7QIrVUhHpPKGBcuW0TQ4ignDMlaVEFjL1plt9MzppEbI27Zeh2zOmY6pSaX4/nKltCx11WtSZH59KlJ4sNyjLGO51VvUHcH18Pg2kzIcNrNP10pOXf1XfrPHETy0YSk37Hk8A4uOgp6nIY6fwJjzUa4F2aPCKPk8IoME4Y+dM3GUrx18Pox2syk9g6FMB0f0Luc1G++nPwzZmu5gIGwaFLXvV/iTwjcEFpVkhhBRe6EoJwdq/l9vc8dNW5qFd9/zPfJByPe2rmbN9D2469g3OdNwsNuBq4iz8VVyaPw2jD0A5OBqpgzoiS7NBqAJKWv5wiFv5PzHbiCRgLQITVG0Ma/6JmPtx63q6d5Uol5le3JGT13yXCBE1TCRYlBOVonwtUikEnG0RKcXgCcT9P3G6m+pzKhv2oWm8SJUqyHEgKJfRCQXBcHYeQI16qhW7eWJcpZUOQfAWMUaH26TmNCZWTUntpkwoVMR0XCWeWu6SSVKrqO1rPKjIgSq2HRq+8AjFLdhtvvKisioPuad1QuK1cuS1K+XKNyicJ/x/BxVQz5qYumMFg7qfoKXrV3Kfvs8yLpZ+3LVYWdipy+B7qfuI47fgOoPgIPKzKdmkG/jeI0/xCqMbAIJ+cpB50OQYV7vcp5omc/Glnm8Zt3tnLB1JStbZzEaRr8S1S1WuVZgDx2br2N12Be4DrgoUP3hpvbZBGp5z90/ZFPrNA7bsor75x3M3w49A3rWuNUv78N2jO3D2stQvb4qI99yMsK1boBZFMtQEjOYxCQEpIKAWJVEdV0o8vaUyHOt6tF+06DdoNOAWZ6Ok4hz2C7wjttR6mfBTwasFvp7bqJMdoISeUKVESPO5msKIxzBRMs+eVSN4zapbrTKGuCQ3WCQ17KH+gUeyqtiVEkFbpExZSZ8IMKITRw3xDJqHFVkYWUz0GKsdTvmcbyVOBlCgrYanThe3iggGskyb3MfKWOJ09U3EwUwgThflZnw8Uy/QCQVfJohIt0YM+zoxLtRWoixOS0PRQrrVPV3wFt0W+dkQaYLfAF4ucBgcZhkZA1D6VYezkzjuese4PQn/8zcnqfoap/HN444B9Kd9zOw6dUol6F6XhlHXBNwFcqewJdB+gNriEa3EFjDpsx0vrbvORCmWdE0h70Xbuata25men6Qvqj5PtC3qAs4nlYmrVQauAq3e3irlYCnO+eQMjHvuO8aXtHSzmcF7jz4FFi/jjEintPPfkli7sHRGyrZGftgbSeqW5GQllw/lzx0DUPp1rHMmIM+PW4kQhIE96vq/UVmSgDMSQcSWDCBc1TPAtICWXVgtciDViEZ2gIm7g5ahU7UHo3wsjpGWrpI46mliK9FPbkRJVZLZypDWsIxwPLgxEASE6uOBxvXjqbZVVlLayXq0wCJUYjVkrOWaakUTUGILUmTNBDnyXnntvdDV62DEYtRK+QTJZ90ESdbaoBVhOPzQSikRmLmbh0ilVjiVEhYIzGlCYRpI3nyqYhcJkKMReEIgc+pW6BMeRuQAOUTwC9cEr7dS6US1Ts0Ch+JVDVRuMHnOioy9Lax/5+PyzT5DsqQ8iJr6G+aRm9zwAvW/oXIxCzofYZ1bfP51pFvfoKRgQsweh1qLhR4lW47gf4Ll8/6CuBXQKIS0GxyRLHLCPrItL15ZOYRrGydz+ce+QbNJsdokL7DKh9zQLdtyf2u4pdwjuWNokoSRDzROZd9+zZz6LI/cef8k2BwyFNHbTEMXI/q86lMHt0X1XmgWwu7v93N02ky+YJiPT8U6QASC+Ssxe/PFLhieYGNeYV04AwMq/qM+lXcr+5/qXPXa7q1ernCO+sAiXq9BbZ01m/MjjjukBQ/X0gFIUVZ0GQSIXzP+lxI0FSx37grN4qxus26mgrCidXW6rwtYxVjrZJYgsQGUWykDjNwXiYxNMWGKDGkjCWJgjqidMCKkIoNc3qH6WlvIpsKCa0epPCSOjrjcOAXuxVQCYglZwL5lGL6C9SFu1T1ToWTKjvaFeAcx9zl/bjE89uqymrpbXLnoT5/w4MID4JN+NZRF8SMDt1MfuRuQY4OrX2lwLEKx447euU4kJ8g+hDCDSh3A/c7mzygLRlB4mEeb9+HDxx6MR99/AdMS4bJE37XomegvLQc0AocoOh7repHKMTNqeXp9g5OWv9/LL//x/xhz1MI4/7SHy4XW5n0502AxcBjVlyO6IL/TJ1v6ipvCtkKyrVR+Fps7FdHkwRFaQ4jAhH68jnaUmkyQVA+tKnYNDEJidXelIQ3KnouLgK/mt9Z67QdW4ufk00ShpPY7UYVzWQBWqIUmSAqwFUo8HdzeqxFpVDHxCQMxTFGFSmpY1NoaQ5Txb6lfFWwUr/25QwSm6A5MYGtBVbC4enEzGjLxj1WIJlkMjwTBgRWmTGYJZeOyEXhbBOQiNYkxM7z43y3OS9MlH4C/n00jG7DKJFxtlOfwqcoA1aUOPvVaT+HAz8CvoU7NaP89klTB5E1vOyZezm2exn3zDyUHxx6Xp9m+24XuCtSOlH2R+3egWVvXFD1vghHgOyr6FsE1qg7nfkeYKmKrG+Lh3R52x4MBilajMEEMqzwac8lKssdU3iHOhrB2HZ5NkixZKibJYPrwEY0bXsI62ZVtlKZ4R54Ew0jkEoMRi1WrYu2d2BWi/B2mLGWBAsKI+qOLjOqDCcxo/VMNp8UjoA26k8vDj6vVTXntCAYtcRqsdZ6wvG2j8hZQ6KWACGUYGEozNf6yjDVUmu2pwOYY1S7RtWOgVS5BHuxKtbELt5JwrZAKiZ5HPNZWbUQG5piI81xIrb2buABolxpguBSRUe3C3wDIbCQSswMRc8SrSuLc8RkDcCp08KWC9xgkV/kMuGDhdWieGL/QZWvAJeWpzJM+HQxLj3u2QK/VvgDjlnex/ixUh1AKglCq8jmBUOb15/d5zJuXH3wOcT5kTgw8ZZAzRbgHnGDNwPajNIOmhLnK5utjoT4DG6bXBVoNVlEbbGasNS6NC4nVWjEDtzJPSvGtUAlG0IiacjnSW8LVj2489bm1tq9MaKkY4N6R611nw/U0TEnK5waIH9D6FdQfO4lVa3rHCRxSkGrUX2N1M5pFRb5vIZrfPdoxZ5krL3T1lDFCr4rl5xRTzbK9Cr3jZl4CMRUSi1AnmHgDKP20YIGW18d7clW5bjqmpWiqiJqNTRmuClv+o3o4jqs+gvURXH8Bpd6O/Jza32FbjBM9G9aG7BvYPXMprw5us52embS8CPchrKW7Y8OKNRjk59n62woKxMJnhLYikwc/qVHcX1W0aOAF9ZR6gDYG+Xd6vxYee/IHvW7THMLq6egK4ZSLZ8Mg/Qvz151M4mE/GavlxObPNkgg7isMsb7wkao45y7XJh2avZ4BoheHLHvpEqgr/BCq3pVsdM1DwTGEsaWTLKNBjwNqk66In8BpBMXuBqoxUes1hOLtg/wS3Ha4//5+qeoz04rBGfPRjnRYPerQ7MaLQKpWoHW8wSuQuSDonqHuubSIp9bYRxEOJZOM+jZRu3FNe6b9YNzilUqQYTuRDUnlUNjUri4wTXiOGIjTPQrFtcxEOcDPcuqfkzRqPrzXTjZ/N4tpG1+QFQ3R2IOrdNbcyxu1zgpMnBM5R8okdGwqP8j0MnYkD2ieIO4LhMNE8lnbBjcuRP8k7boqqi0RclEf8gGhQ+gXCfo4jpCcQqfZzyitxd/WFSDo0T5ZiJhb09Tx52vXHsH71hxAx8++lLuWvQiWkY2MZntUhVh8fAmQpNgVIqDvZf6YkqFZBJHej9M/0Q/m9OKUnFpamKdpyK1qANhAaxSxpJYS+AOFxim/sNFW4Dj/TXVshHo8q29SpmYYLqM7I/Iz0AfwOWmGvUrYSERXxMwX9E2hUMt7FdH9H1WfJD3VPtzBfrV9Xc1XtdC4McIS72FMODrWPBJNfs6NgN7JXBMPRkGAmtILKiJSCUjgyjrAtXJ1DmgvvClHXNiA4qsHecraT0/I50jn2uK4lqUip0l5VaG+60LJfgfb+7VhVjluqBET5kLvBPkThUhG0YcMLCau8zzyIbNtNrRugBaVBlp6uTSB77AnOxWhqMJIQKbFM1SmT80U50P6eFijShIDJlsTLqEFKciC2xQ1bdiCif9WAkIkwTr/RSK9AOP4VIk707yGJAXEVCWKzpYwyEPSgZ4nr92hv9ik0jwV2fxTvkoX+2vOTXngnIM1eIIJ1nURCGdDLPXwBNsTc3JB9i/iNYM5dq14ur0jI1kmQ0CQmNrEmOLwbRlMMdoW5pcU4rATm1fBhUqcK8qr7Kq91hnd49fqKc/ll4TdsLUus0QSq59FJoV2Nw0nTev+hUXPv4jbNjEaNhUVz6rOExxcPcjpEyOXBBiscXlMBbiMZ3Sm4hFl6hqW6EuBiFtoT0/RDpvycTJhCs05iVRYsIocdvIpVcqNmsDY58OjKVwlcSB/p6dc/z2zpRbnE2hKHofldNAT2kZVG2yi85S7MadWDS1070cWAVpZue7eOWGaxkNW8Blmn2G3U9uDYyuiPJmMkDlKh8IzcN50tkYDWTXg5V7pD6k8EoLVxvImyIHiS25tGBQu2u9Ue636gLOrbXF10xr7f7WWsQkrG2ZxTmrbuWCZT8iiZrIBZmqgBWoJdu2gLc88XMWDq1lWCIStWOXURtYa1OF5+m2ZfVZp9z7tMnyZNM8lrUcSJMZHTsdzF/7RIk9NRUbKl1RYldGxm4MjFK4SuROxvhfu4V8A7cZQpHP6pNsz0GvO6bpfL26El5LSa/LB1Is3wbu28n1ML4tu6tMZasCA6kORsJ2QmvXAB9kN6IHIKxH/BiVSeuQWnDLtHjAslMIWLUccL0o56vqm63qbcVaVgkIoV7Hsdj3WOxDFpt27yeARbuF2WOeNGtY0zqTNz71Gy5Y/mPyqeaKJ9sAZFMtPGft7bTFwwyHrQToeN5e95U5xSagqo6bZa7cgVV3CnWiMCM/zMrmfbiz8yQ68v2I1eLrg2J0jhioeFkeCmM7kMobUnkHYCXdbICPA1/dDYblT/xEKZWbcBskG3dBGVYAF5XRLmrtYHaUrKW1DjcojeLfgMuksTMB62PAR6gestQWS4rZuS6O7b2bbNgMcA1wNu4osWdbbgdew7acyWbqO0AiU+xHbhnOk8klUwZY9WZO/DkuNvDVqno28GIt3V1xntrfAb8A3l5BU+4AFk+Y0GpY3zyds1bfSqLCDw853wUWl1NHW+dw+oNfZkn/Bja2TifY9juHleYaG88RZgHiQGSDFaEz6WNDpoO/TjuW+bmNGJngmjoVeEsNF1oM+sex6SMQiGVa0s9oOGH8DgGX+oFxmneiHzTljlMnA7gUzD/H5T+vpEFdjTu95R04Ht28KdCm/tdrOMvL/D2c5DhNbce4XgmcCbzXT9D9trMua3F8vU/7vqwaPhNLmvnZbp7T+yfumfES9hxdhZXwWtzO75uAFwCH4pz8u4L1v8E/+zYcV3JgO5SYiUbY2BwTmodyLjSjKUVgdq4CKdd3zt4GVtRCQStSR/wZ9zuptuFY58co+jxgCS4/1WyQV6pyM8JjuHxFvkYTQOXbqlw0scaCoLTHQ9wy93jiMDVmOxfHKCZhmkO2Psb87FbibY9sbxPhRlFerCX3dnaBArJCRY5sSYazubCZby9+F081LWFOvqsYrPbH8Vv2qtF293hQGyl0VIByRP/9vHDLb6lC/luCc/LP8Ndi3A6l4ngy7ZPwc4mf6H24PNqFMxjWA6twu2DLmdx5cEf7chzkJ9BCxpMvBl57LU181OfNoSb/+Ub/zDW4Xb91OL5QJTkOl4poqMREKgT2PgncUfT5ybhc/r1lJlnon1VNizoQl4X2cN8fc3y5W/3rwr7YFsbpNKtwIVurGd+gWQC8mJLklEXa3wrg7kgTskErt805naUdz6HFDJdqgXu7+cMMXGzoXoynsV6AixqZjO8zxOXiKvCyuvzrbl+n5ZScq1AiBzN+IEc5ukTKP+MmHF1pvOIOMBhty5DPRJP2gdlQSPzcsSLkMiGjqcjN3u0Aq6LvaiswQx3PZzZwh6KdKPfgsh6Uk6cVTlX0iUKvJtY9QVDassMVl5cA6IsiSDcTbtsGpwC3VtJY1Pm8bggxb8oFmfgHiy7imaY9mZ70YscXkhMYD5GpJW8Hvj9B1ZIMGR3lklWXY7cryRoZP+HNJMAqwNEJ8lO0Erf5gVlIs9ru27i4B0Y90BQ0nkF2u3DYqm1YWCwyvr6FNXLIt2t+R9pXEWbmh1naeQg/2PNSZuS7J/PztDfLJpP9KfBadO5ZaVCjJOmQoY6mSe8QVgOrHU2gP+yvZ8Dtvila64SlJcBZqvrZwgeJFvYToS/TUkdjmLE85+Lo222CjB3mWI5GIUDaZm8bjtrinyy8gE2ZhXQmfQWgynjz4NI6TaA7gJ9u+wxLazK4I+35rA2wKjJU4z0loPX3JlpUp2Gm4Ah1QYkDGAnbCHTSptEOAeWzBf9TsTMY7cTyFXp+s1c1t8kQWqSVXYTbvn7Ujq09MqnRVRyLq3Cmoq+a+KwSvdXmzU1zX3vXyraDGQqn0WYGWy1BQY2/EDiC+uLVutj1O2gNacg/veyUyHgXM+XoA6pqVXWZqqX0GjMzYW+FzyjM2Qm2wgtU+awqwUQ+2Dj7yqJE1vyiOz27c3Nm/qczNvtdS3Az7qy+L3s/Tb2BtV8t8Z+MI78m9KZmceOCc8nYbGN0NaQhu5tmpWNG3Nj7Pymc55nPJd8ce306jvfzTmoHm1aSF6nq9xWtGP1uEKbHw/pwx5E/39i0+Nz2ZOCCHQDpq4Arq7WEkZDBaBryd+OyaUhD/gk0K5cszpI1CUbtGLtd4deq+rSy7T8mvOK1uC31Ayf56DTwOpyDe+/K0CE0WYMJghsf7jg2P5jqPCOl8fbW+X9webxMLf9EqKYxshrSkN0JrBTGclWXhNX0KnxRKRvyMjF8R/U0XBjC+3A7iNVOImnBHXjwWRy5ripQqcCcONv/VOt+X1o27fA3tiRDC2XyIR7WA+rFlBxL3pCGNOTvwAwUgdhYskniTz7ZBgS+j+MhvbaO2+0NfA53tNHdwJ9xAbcDHizacA7w43GBwZ21EUbojIfYkGl//52zTt0rUnNWpPFks1l0+XJ9vjFUGtKQv1OwKjYFK0gCvAdHMDuoztvt6a9zPEgNebNrGnU6wBXBiDA9zjIctn/9R3te+NdVrYffNDu3sTXA1gtWimP4fgoX39eQhjTk79kMrEPWAW/F5QbanrIVkt/VDVQKdMZDzDfJL/4644SvLWs/6oud8dZFIUk9QKW4kKGzgZc3gKohDfkH0qzqkPv95P8pVM9ZvSPimFqWlMkzkur4441zjvvdvTNedN3M3JbD0jaLrYx3hfCDPwP3eo1qqDE0GtKQfz6wAsdLeg2OqnDUVD0kY/OkrfnBDbNP+8Btc886bUZu8zOtycBtiBQnMhRvWvYyHrO2ml2QZrchDWnI7g9W4AJLz8Sl1XjHFNy/N0A/CXxjS3pOLq9cNy3u/ZkJouTv5wi7hjSkIZVkV5/tthaXnP+1wJ920j0HcDuPL1d3mGkuY3OkNMlaCZJGFzekIQ3NanslD9zgweoFwBuB09k2YVotWYpLV3szLmfTaKM7G9KQBlhNhXQDv8Ttvu2HO3nmRBzNYT7juZEKNtxmnG/pIVzysKX+HnGjGxvSkH98+f8DACeR8Z8W+T8oAAAAAElFTkSuQmCC" />
  <h2>DiskSpace Report</h2>
  <table border="1">
    <tr bgcolor="#66ccff">
		<th>PackageID</th>
        <th>QuotaValue</th>
        <th>Bandwidth</th>
        <th>UsagePercentage</th>
        <th>PackageName</th>
        <th>PackagesNumber</th>
        <th>StatusID</th>
        <th>UserID</th>
      <th>Username</th>
        <th>FirstName</th>
        <th>LastName</th>
        <th>FullName</th>
        <th>RoleID</th>
        <th>Email</th>
    </tr>
    <xsl:for-each select="//Table1">
    <tr>
	<td><xsl:value-of select="PackageID"/></td>
        <td><xsl:value-of select="QuotaValue"/></td>
        <td><xsl:value-of select="Bandwidth"/></td>
        <td><xsl:value-of select="UsagePercentage"/>%</td>
        <td><xsl:value-of select="PackageName"/></td>
        <td><xsl:value-of select="PackagesNumber"/></td>
        <td><xsl:value-of select="StatusID"/></td>
        <td><xsl:value-of select="UserID"/></td>
      <td><xsl:value-of select="Username"/></td>
        <td><xsl:value-of select="FirstName"/></td>
        <td><xsl:value-of select="LastName"/></td>
        <td><xsl:value-of select="FullName"/></td>
        <td><xsl:value-of select="RoleID"/></td>
        <td><xsl:value-of select="Email"/></td>
        <td><xsl:value-of select="UserComments"/></td>
    </tr>
    </xsl:for-each>
  </table>
  </body>
  </html>
</xsl:template>
</xsl:stylesheet>'),
			(1,'DiskspaceXLST','TransformSuffix','.htm'),
			(1,'DiskspaceXLST','TransformContentType','text/html')

			
-- Fix for SP2013 wrong ProviderID

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1552' AND [DisplayName] = 'Hosted SharePoint Enterprise 2013')
BEGIN
DECLARE @group_id AS INT
SELECT @group_id = GroupId FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Enterprise Server'
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1552, @group_id, N'HostedSharePoint2013Ent', N'Hosted SharePoint Enterprise 2013', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2013Ent, SolidCP.Providers.HostedSolution.SharePoint2013Ent', N'HostedSharePoint30', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted SharePoint Enterprise 2013'
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE DisplayName = 'Hosted SharePoint Enterprise 2013' AND NOT ProviderID = '1552')
BEGIN
DECLARE @SP2013provider_id INT
SET @SP2013provider_id = (SELECT ProviderID FROM [dbo].[Providers] WHERE DisplayName = 'Hosted SharePoint Enterprise 2013' AND NOT ProviderID = '1552')
UPDATE [ServiceDefaultProperties] SET [ProviderID]='1552' WHERE [ProviderID] = @SP2013provider_id
UPDATE [Services] SET [ProviderID]='1552' WHERE [ProviderID] = @SP2013provider_id
DELETE FROM [Providers] WHERE [ProviderID] = @SP2013provider_id AND DisplayName = 'Hosted SharePoint Enterprise 2013'
END
GO

-- Fix for SP2016 wrong ProviderID

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1702' AND [DisplayName] = 'Hosted SharePoint Enterprise 2016')
BEGIN
DECLARE @group_id AS INT
SELECT @group_id = GroupId FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Enterprise Server'
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1702, @group_id, N'HostedSharePoint2016Ent', N'Hosted SharePoint Enterprise 2016', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2016Ent, SolidCP.Providers.HostedSolution.SharePoint2016Ent', N'HostedSharePoint30', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted SharePoint Enterprise 2016'
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE DisplayName = 'Hosted SharePoint Enterprise 2016' AND NOT ProviderID = '1702')
BEGIN
DECLARE @SP2016provider_id INT
SET @SP2016provider_id = (SELECT ProviderID FROM [dbo].[Providers] WHERE DisplayName = 'Hosted SharePoint Enterprise 2016' AND NOT ProviderID = '1702')
UPDATE [ServiceDefaultProperties] SET [ProviderID]='1702' WHERE [ProviderID] = @SP2016provider_id
UPDATE [Services] SET [ProviderID]='1702' WHERE [ProviderID] = @SP2016provider_id
DELETE FROM [Providers] WHERE [ProviderID] = @SP2016provider_id AND DisplayName = 'Hosted SharePoint Enterprise 2016'
END
GO

-- SimpleDNS 6.x
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1703' AND DisplayName = 'SimpleDNS Plus 6.x')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1703, 7, N'SimpleDNS', N'SimpleDNS Plus 6.x', N'SolidCP.Providers.DNS.SimpleDNS6, SolidCP.Providers.DNS.SimpleDNS60', N'SimpleDNS', NULL)
END
GO

-- SimpleDNS 8.x
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1901' AND DisplayName = 'SimpleDNS Plus 8.x')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1901, 7, N'SimpleDNS', N'SimpleDNS Plus 8.x', N'SolidCP.Providers.DNS.SimpleDNS8, SolidCP.Providers.DNS.SimpleDNS80', N'SimpleDNS', NULL)
END
GO

-- Proxmox Provider

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'Proxmox')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (167, N'Proxmox', 20, NULL, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (143, 167, N'VirtualMachine', N'SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base', 1, 0, 0, 1, 1, 1, 0, 0)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (144, 167, N'VirtualSwitch', N'SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base', 2, 0, 0, 1, 1, 1, 0, 0)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (673, 167, 1, N'PROXMOX.ServersNumber', N'Number of VPS', 2, 0, 41, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (674, 167, 2, N'PROXMOX.ManagingAllowed', N'Allow user to create VPS', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (675, 167, 3, N'PROXMOX.CpuNumber', N'Number of CPU cores', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (676, 167, 7, N'PROXMOX.BootCdAllowed', N'Boot from CD allowed', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (677, 167, 8, N'PROXMOX.BootCdEnabled', N'Boot from CD', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (678, 167, 4, N'PROXMOX.Ram', N'RAM size, MB', 2, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (679, 167, 5, N'PROXMOX.Hdd', N'Hard Drive size, GB', 2, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (680, 167, 6, N'PROXMOX.DvdEnabled', N'DVD drive', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (681, 167, 10, N'PROXMOX.ExternalNetworkEnabled', N'External Network', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (682, 167, 11, N'PROXMOX.ExternalIPAddressesNumber', N'Number of External IP addresses', 2, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (683, 167, 13, N'PROXMOX.PrivateNetworkEnabled', N'Private Network', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (684, 167, 14, N'PROXMOX.PrivateIPAddressesNumber', N'Number of Private IP addresses per VPS', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (685, 167, 9, N'PROXMOX.SnapshotsNumber', N'Number of Snaphots', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (686, 167, 15, N'PROXMOX.StartShutdownAllowed', N'Allow user to Start, Turn off and Shutdown VPS', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (687, 167, 16, N'PROXMOX.PauseResumeAllowed', N'Allow user to Pause, Resume VPS', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (688, 167, 17, N'PROXMOX.RebootAllowed', N'Allow user to Reboot VPS', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (689, 167, 18, N'PROXMOX.ResetAlowed', N'Allow user to Reset VPS', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (690, 167, 19, N'PROXMOX.ReinstallAllowed', N'Allow user to Re-install VPS', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (691, 167, 12, N'PROXMOX.Bandwidth', N'Monthly bandwidth, GB', 2, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '692')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (692, 167, 20, N'PROXMOX.ReplicationEnabled', N'Allow user to Replication', 1, 0, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'Proxmox')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (370, 167, N'Proxmox', N'Proxmox Virtualization', N'SolidCP.Providers.Virtualization.Proxmoxvps, SolidCP.Providers.Virtualization.Proxmoxvps', N'Proxmox', 1)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [EditorControl] = N'Proxmox', [GroupID] = 167 WHERE [ProviderName] = 'Proxmox'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'Proxmox (remote)')
BEGIN

	IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'Proxmox (local)')
	BEGIN
	UPDATE [dbo].[Providers] SET [ProviderName] = 'Proxmox (remote)', [DisplayName] = 'Proxmox Virtualization (remote)' WHERE [ProviderName] = 'Proxmox'
	INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (371, 167, N'Proxmox', N'Proxmox Virtualization', N'SolidCP.Providers.Virtualization.ProxmoxvpsLocal, SolidCP.Providers.Virtualization.Proxmoxvps', N'Proxmox', 0)
	END
	ELSE
	BEGIN
	UPDATE [dbo].[Providers] SET [ProviderName] = 'Proxmox (remote)', [DisplayName] = 'Proxmox Virtualization (remote)' WHERE [ProviderName] = 'Proxmox'
	UPDATE [dbo].[Providers] SET [ProviderName] = 'Proxmox', [DisplayName] = 'Proxmox Virtualization' WHERE [ProviderName] = 'Proxmox (local)'
	END

END

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetVirtualMachinesPagedProxmox')
DROP PROCEDURE GetVirtualMachinesPagedProxmox
GO
CREATE PROCEDURE [dbo].[GetVirtualMachinesPagedProxmox]
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
SI.ItemTypeID = 143 -- Proxmox
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
'

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (ItemName LIKE ''' + @FilterValue + '''
			OR Username LIKE ''' + @FilterValue + '''
			OR ExternalIP LIKE ''' + @FilterValue + '''
			OR IPAddress LIKE ''' + @FilterValue + ''')'
END

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


-- VLAN tag in IpAddresses
IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'IPAddresses' AND COLUMN_NAME = 'VLAN')
BEGIN
ALTER TABLE [dbo].[IPAddresses] ADD [VLAN] int NULL;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[GetIPAddress]
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[UpdateIPAddress]
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[UpdateIPAddresses]
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetIPAddresses]
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[AddIPAddress]
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetUnallottedIPAddresses]
 @PackageID int,
 @ServiceID int,
 @PoolID int = 0
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
    IP.DefaultGateway,
    IP.VLAN
   FROM dbo.IPAddresses AS IP
   WHERE
    (IP.ServerID = @ServerID OR IP.ServerID IS NULL)
    AND IP.AddressID NOT IN (SELECT PIP.AddressID FROM dbo.PackageIPAddresses AS PIP)
    AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
   ORDER BY IP.ServerID DESC, IP.DefaultGateway, IP.ExternalIP
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
    IP.DefaultGateway,
    IP.VLAN
   FROM dbo.IPAddresses AS IP
   WHERE
    (IP.ServerID IN (
     SELECT SVC.ServerID FROM [dbo].[Services] AS SVC
     INNER JOIN [dbo].[Providers] AS P ON SVC.ProviderID = P.ProviderID
     WHERE [SVC].[ServiceID] = @ServiceId AND P.GroupID = @GroupID
    ) OR IP.ServerID IS NULL)
    AND IP.AddressID NOT IN (SELECT PIP.AddressID FROM dbo.PackageIPAddresses AS PIP)
    AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
   ORDER BY IP.ServerID DESC, IP.DefaultGateway, IP.ExternalIP
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
   IP.DefaultGateway,
   IP.VLAN
  FROM dbo.PackageIPAddresses AS PIP
  INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
  WHERE
   PIP.PackageID = @ParentPackageID
   AND PIP.ItemID IS NULL
   AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
   AND (IP.ServerID = @ServerID OR IP.ServerID IS NULL)
  ORDER BY IP.ServerID DESC, IP.DefaultGateway, IP.ExternalIP
 END
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetIPAddressesPaged')
DROP PROCEDURE GetIPAddressesPaged
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetPackageUnassignedIPAddresses]
(
 @ActorID int,
 @PackageID int,
 @OrgID int,
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
  IP.DefaultGateway,
  IP.VLAN
 FROM PackageIPAddresses AS PIP
 INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
 WHERE
  PIP.ItemID IS NULL
  AND PIP.PackageID = @PackageID
  AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
  AND (@OrgID = 0 OR @OrgID <> 0 AND PIP.OrgID = @OrgID)
  AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
 ORDER BY IP.DefaultGateway, IP.ExternalIP
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetPackageIPAddresses]
(
 @PackageID int,
 @OrgID int,
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
AND (@OrgID = 0 OR @OrgID <> 0 AND PA.OrgID = @OrgID)
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
   OR ItemName LIKE ''' + @FilterValue + '''
   OR Username LIKE ''' + @FilterValue + ''')'
END

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
 IP.VLAN,
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

exec sp_executesql @sql, N'@PackageID int, @OrgID int, @StartRow int, @MaximumRows int, @Recursive bit, @PoolID int',
@PackageID, @OrgID, @StartRow, @MaximumRows, @Recursive, @PoolID

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetPackageIPAddress]
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
 IP.VLAN,
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

-- Fix for Enterprise Solution Report scheduled task page
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = 'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT' AND [ParameterID] = 'SFB_REPORT')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'SFB_REPORT', N'Boolean', N'true', 5)
END
GO

-- Fix for Send Database Usage Notifications scheduled task page
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = 'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = 'MARIADB_OVERUSED')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MARIADB_OVERUSED', N'Boolean', N'true', 1)
END
GO

-- Fix for MaximumRows in a GridView for serveradmin
IF EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName] = 'DisplayPreferences' AND [PropertyName] = 'GridItems' AND cast([PropertyValue] as nvarchar(max)) = '11')
BEGIN
UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'10' WHERE [UserID] = 1 AND [SettingsName] = 'DisplayPreferences' AND [PropertyName] = 'GridItems'
END
GO

-- Fix for scheduled tasks page - was returning MaximumRows - 1 instead of MaximumRows
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetSchedulesPaged]
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

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (ScheduleName LIKE ''' + @FilterValue + '''
			OR Username LIKE ''' + @FilterValue + '''
			OR FullName LIKE ''' + @FilterValue + '''
			OR Email LIKE ''' + @FilterValue + ''')'
END

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
	-- bug ISNULL(0, ...) always is not NULL
	-- ISNULL(0, (SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = ''SCHEDULER'' ORDER BY StartDate DESC)) AS LastResult,
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
GO

-- Hiding of Ecommerce scheduled tasks by changing to RoleID 0 (doesn't match even serveradmin), because Ecommerce module is currently not implemented
IF EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = 'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, SolidCP.EnterpriseServer.Code' AND [RoleID] = 2)
BEGIN
UPDATE [dbo].[ScheduleTasks] SET [RoleID] = 0 WHERE [TaskID] = 'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, SolidCP.EnterpriseServer.Code' 
END
GO
IF EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = 'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, SolidCP.EnterpriseServer.Code' AND [RoleID] = 2)
BEGIN
UPDATE [dbo].[ScheduleTasks] SET [RoleID] = 0 WHERE [TaskID] = 'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, SolidCP.EnterpriseServer.Code' 
END
GO
IF EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = 'SCHEDULE_TASK_GENERATE_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.GenerateInvoicesTask, SolidCP.EnterpriseServer.Code' AND [RoleID] = 2)
BEGIN
UPDATE [dbo].[ScheduleTasks] SET [RoleID] = 0 WHERE [TaskID] = 'SCHEDULE_TASK_GENERATE_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.GenerateInvoicesTask, SolidCP.EnterpriseServer.Code' 
END
GO
IF EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = 'SCHEDULE_TASK_RUN_PAYMENT_QUEUE' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.RunPaymentQueueTask, SolidCP.EnterpriseServer.Code' AND [RoleID] = 2)
BEGIN
UPDATE [dbo].[ScheduleTasks] SET [RoleID] = 0 WHERE [TaskID] = 'SCHEDULE_TASK_RUN_PAYMENT_QUEUE' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.RunPaymentQueueTask, SolidCP.EnterpriseServer.Code' 
END
GO
IF EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = 'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, SolidCP.EnterpriseServer.Code' AND [RoleID] = 2)
BEGIN
UPDATE [dbo].[ScheduleTasks] SET [RoleID] = 0 WHERE [TaskID] = 'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES' AND [TaskType] = 'SolidCP.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, SolidCP.EnterpriseServer.Code' 
END
GO

-- LE Removal
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
IF EXISTS (SELECT * FROM [dbo].[Schedule] WHERE [TaskID] = 'SCHEDULE_TASK_LETSENCRYPT_RENEWAL' )
BEGIN
	DELETE FROM [dbo].[Schedule]
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
GO

-- This is for SQL updates after 1.2.1
--
-- Please ensure additions are added to the BOTTOM of this file
--
--

-- RDS Adding seperate controllers

-- # Adding Column for controller
IF NOT EXISTS (select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='RDSServers' AND COLS.name='Controller')
BEGIN
	ALTER TABLE [dbo].[RDSServers] ADD [Controller] [int] NULL
END
GO

-- # Filling the Controller column on the RDS TABLE
IF EXISTS (SELECT * FROM RDSServers WHERE Controller IS NULL OR Controller = '0')
BEGIN
	DECLARE @SystemController nvarchar(max)
	SET @SystemController = (SELECT PropertyValue FROM [SystemSettings] Where SettingsName = 'RdsSettings' AND PropertyName = 'RdsMainController');
	IF(@SystemController is not NULL)
		BEGIN
			UPDATE RDSServers Set Controller = @SystemController WHERE Controller IS NULL OR Controller = '0';
		END
	ELSE
		BEGIN
			RAISERROR(N'Please set the global RDS Controller and rerun the script', 16, 1);
		END
END
GO

-- # Editing Stored PROCEDURE
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddRDSServer')
DROP PROCEDURE AddRDSServer
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRDSServer]
(
	@RDSServerID INT OUTPUT,
	@Name NVARCHAR(255),
	@FqdName NVARCHAR(255),
	@Description NVARCHAR(255),
	@Controller INT
)
AS
INSERT INTO RDSServers
(
	Name,
	FqdName,
	Description,
	Controller
)
VALUES
(
	@Name,
	@FqdName,
	@Description,
	@Controller
)

SET @RDSServerID = SCOPE_IDENTITY()

RETURN
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE type_desc = N'SQL_STORED_PROCEDURE' AND name = N'GetRDSControllerServiceIDbyFQDN')
BEGIN
EXEC sp_executesql N'CREATE PROCEDURE [dbo].[GetRDSControllerServiceIDbyFQDN]
(
	@RdsfqdnName NVARCHAR(255),
	@Controller int OUTPUT
)
AS

SELECT @Controller = Controller
	FROM RDSServers
	WHERE FqdName = @RdsfqdnName

RETURN'
END
GO

-- RDS Disable user from adding server
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = N'452')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (452, 45, 3, N'RDS.DisableUserAddServer', N'Disable user from adding server', 1, 0, NULL, NULL)
END
GO

-- RDS Disable user delete server
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = N'453')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (453, 45, 3, N'RDS.DisableUserDeleteServer', N'Disable user from removing server', 1, 0, NULL, NULL)
END
GO

-- HyperV2016

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HyperV2016')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (352, 33, N'HyperV2016', N'Microsoft Hyper-V 2016', N'SolidCP.Providers.Virtualization.HyperV2016, SolidCP.Providers.Virtualization.HyperV2016', N'HyperV2012R2', 1)
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HyperV2016' AND [EditorControl] = 'HyperV2016')
BEGIN
UPDATE [dbo].[Providers] SET [EditorControl] = 'HyperV2012R2' WHERE [ProviderName] = 'HyperV2016'
END
GO


-- Name Change for:
--   Instant Alias = Preview Domain
--   InstantAlias = PreviewDomain
--   instantAlias = previewDomain
--   Instantalias = Previewdomain

-- Change Domains Table is Isinstantalias exists
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Domains' AND COLUMN_NAME = 'IsInstantAlias')
BEGIN
ALTER TABLE [dbo].[Domains] DROP CONSTRAINT [DF_Domains_IsInstantAlias]
END
GO


-- Rename Column
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Domains' AND COLUMN_NAME = 'IsInstantAlias')
BEGIN
EXEC sp_rename '[dbo].[Domains].IsInstantAlias', 'IsPreviewDomain', 'COLUMN';
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_IsPreviewDomain]  DEFAULT ((0)) FOR [IsPreviewDomain]
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


ALTER PROCEDURE [dbo].[GetDomain]
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
	D.IsDomainPointer,
	Z.ServiceID AS ZoneServiceID
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


ALTER PROCEDURE [dbo].[GetDomainByName]
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetDomains')
DROP PROCEDURE GetDomains
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
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[GetDomainsByDomainItemID]
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
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


ALTER PROCEDURE [dbo].[GetDomainsByZoneID]
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetDomainsPaged]
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetSearchTableByColumns')
DROP PROCEDURE GetSearchTableByColumns
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[AddDomain]
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
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupID] = '73' AND [GroupName] = 'Sharepoint Enterprise Server')
BEGIN
DECLARE @group_id INT
SELECT @group_id = GroupId FROM ResourceGroups WHERE GroupName = 'Sharepoint Enterprise Server'
ALTER TABLE PackagesDiskspace NOCHECK CONSTRAINT ALL
ALTER TABLE HostingPlanResources NOCHECK CONSTRAINT ALL
ALTER TABLE ResourceGroups NOCHECK CONSTRAINT ALL
ALTER TABLE VirtualGroups NOCHECK CONSTRAINT ALL
ALTER TABLE Providers NOCHECK CONSTRAINT ALL
ALTER TABLE Quotas NOCHECK CONSTRAINT FK_Quotas_ResourceGroups
ALTER TABLE ServiceItemTypes NOCHECK CONSTRAINT FK_ServiceItemTypes_ResourceGroups
UPDATE [dbo].[HostingPlanResources] SET [GroupID] = '73' WHERE [GroupID] = @group_id
UPDATE [dbo].[PackagesDiskspace] SET [GroupID] = '73' WHERE [GroupID] = @group_id
UPDATE [dbo].[VirtualGroups] SET [GroupID] = '73' WHERE [GroupID] = @group_id
UPDATE [dbo].[ResourceGroups] SET [GroupID] = '73' WHERE [GroupName] = 'Sharepoint Enterprise Server'
UPDATE [dbo].[Providers] SET [GroupID] = '73' WHERE [ProviderName] = 'HostedSharePoint2013Ent'
UPDATE [dbo].[Providers] SET [GroupID] = '73' WHERE [ProviderName] = 'HostedSharePoint2016Ent'
UPDATE [dbo].[Quotas] SET [GroupID] = '73' WHERE [QuotaName] = 'HostedSharePointEnterprise.Sites'
UPDATE [dbo].[Quotas] SET [GroupID] = '73' WHERE [QuotaName] = 'HostedSharePointEnterprise.MaxStorage'
UPDATE [dbo].[Quotas] SET [GroupID] = '73' WHERE [QuotaName] = 'HostedSharePointEnterprise.UseSharedSSL'
UPDATE [dbo].[ServiceItemTypes] SET [GroupID] = '73' WHERE [DisplayName] = 'SharePointEnterpriseSiteCollection'
ALTER TABLE ServiceItemTypes WITH CHECK CHECK CONSTRAINT FK_ServiceItemTypes_ResourceGroups
ALTER TABLE Quotas WITH CHECK CHECK CONSTRAINT FK_Quotas_ResourceGroups
ALTER TABLE Providers WITH CHECK CHECK CONSTRAINT ALL
ALTER TABLE VirtualGroups WITH CHECK CHECK CONSTRAINT ALL
ALTER TABLE ResourceGroups WITH CHECK CHECK CONSTRAINT ALL
ALTER TABLE HostingPlanResources WITH CHECK CHECK CONSTRAINT ALL
ALTER TABLE PackagesDiskspace WITH CHECK CHECK CONSTRAINT ALL
END
GO

/* SQL 2017 Provider */
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'MsSQL2017')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (72, N'MsSQL2017', 10, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
ELSE
BEGIN
UPDATE [dbo].[ResourceGroups] SET [ShowGroup] = 1 WHERE [GroupName] = 'MsSQL2017'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Microsoft SQL Server 2017')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1704, 72, N'MsSQL', N'Microsoft SQL Server 2017', N'SolidCP.Providers.Database.MsSqlServer2017, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (73, 72, N'MsSQL2017Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (74, 72, N'MsSQL2017User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (711, 72, 1, N'MsSQL2017.Databases', N'Databases', 2, 0, 73, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (712, 72, 2, N'MsSQL2017.Users', N'Users', 2, 0, 74, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (713, 72, 3, N'MsSQL2017.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (714, 72, 5, N'MsSQL2017.Backup', N'Database Backups', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (715, 72, 6, N'MsSQL2017.Restore', N'Database Restores', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (716, 72, 7, N'MsSQL2017.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (717, 72, 4, N'MsSQL2017.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 72 WHERE [DisplayName] = 'Microsoft SQL Server 2017'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1560')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1560, 50, N'MariaDB', N'MariaDB 10.2', N'SolidCP.Providers.Database.MariaDB102, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 50 WHERE [ProviderID] = '1560'
END
GO
-- Change QuotaType for VPS CPU Cores
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '302' AND [QuotaTypeID] = '2' )
BEGIN
UPDATE [Quotas] SET [QuotaTypeID] = '2' WHERE [QuotaID] = '302'
END
GO
-- Change QuotaType for VPSforPC CPU Cores
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '347' AND [QuotaTypeID] = '2' )
BEGIN
UPDATE [Quotas] SET [QuotaTypeID] = '2' WHERE [QuotaID] = '347'
END
GO

-- Change QuotaType for VPS2012 CPU Cores
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '555' AND [QuotaTypeID] = '2' )
BEGIN
UPDATE [Quotas] SET [QuotaTypeID] = '2' WHERE [QuotaID] = '555'
END
GO

-- Fix for CPU Core Quota
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER FUNCTION [dbo].[CalculateQuotaUsage]
(
	@PackageID int,
	@QuotaID int
)
RETURNS int
AS
	BEGIN

		DECLARE @QuotaTypeID int
		DECLARE @QuotaName nvarchar(50)
		SELECT @QuotaTypeID = QuotaTypeID, @QuotaName = QuotaName FROM Quotas
		WHERE QuotaID = @QuotaID

		IF @QuotaTypeID <> 2
			RETURN 0

		DECLARE @Result int
		DECLARE @vhd TABLE (Size int)

		IF @QuotaID = 52 -- diskspace
			SET @Result = dbo.CalculatePackageDiskspace(@PackageID)
		ELSE IF @QuotaID = 51 -- bandwidth
			SET @Result = dbo.CalculatePackageBandwidth(@PackageID)
		ELSE IF @QuotaID = 53 -- domains
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsSubDomain = 0 AND IsPreviewDomain = 0 AND IsDomainPointer = 0 AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 54 -- sub-domains
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsSubDomain = 1 AND IsPreviewDomain = 0 AND IsDomainPointer = 0 AND PT.ParentPackageID = @PackageID)
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
		ELSE IF @QuotaID = 302 -- CpuNumber of VPS
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'CpuCores' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 306 -- HDD of VPS
		BEGIN
			INSERT INTO @vhd
			SELECT (SELECT SUM(CAST([value] AS int)) AS value FROM dbo.SplitString(SIP.PropertyValue,';')) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID
			SET @Result = (SELECT SUM(Size) FROM @vhd)
		END
		ELSE IF @QuotaID = 309 -- External IP addresses of VPS
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 3)
		ELSE IF @QuotaID = 555 -- CpuNumber of VPS2012
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'CpuCores' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 558 BEGIN -- RAM of VPS2012
			DECLARE @Result1 int
			SET @Result1 = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'RamSize' AND PT.ParentPackageID = @PackageID)
			DECLARE @Result2 int
			SET @Result2 = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN ServiceItemProperties AS SIP2 ON 
								SIP2.ItemID = SI.ItemID AND SIP2.PropertyName = 'DynamicMemory.Enabled' AND SIP2.PropertyValue = 'True'
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'DynamicMemory.Maximum' AND PT.ParentPackageID = @PackageID)
			SET @Result = CASE WHEN isnull(@Result1,0) > isnull(@Result2,0) THEN @Result1 ELSE @Result2 END
		END
		ELSE IF @QuotaID = 559 -- HDD of VPS2012
		BEGIN
			INSERT INTO @vhd
			SELECT (SELECT SUM(CAST([value] AS int)) AS value FROM dbo.SplitString(SIP.PropertyValue,';')) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID
			SET @Result = (SELECT SUM(Size) FROM @vhd)
		END
		ELSE IF @QuotaID = 562 -- External IP addresses of VPS2012
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 3)
		ELSE IF @QuotaID = 728 -- Private Network VLANs of VPS2012
			SET @Result = (SELECT COUNT(PV.PackageVlanID) FROM PackageVLANs AS PV
							INNER JOIN PrivateNetworkVLANs AS V ON PV.VlanID = V.VlanID
							INNER JOIN PackagesTreeCache AS PT ON PV.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND PV.IsDmz = 0)
		ELSE IF @QuotaID = 752 -- DMZ Network VLANs of VPS2012
			SET @Result = (SELECT COUNT(PV.PackageVlanID) FROM PackageVLANs AS PV
							INNER JOIN PrivateNetworkVLANs AS V ON PV.VlanID = V.VlanID
							INNER JOIN PackagesTreeCache AS PT ON PV.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND PV.IsDmz = 1)
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
		ELSE IF @QuotaID = 347 -- CpuNumber of VPSforPc
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'CpuCores' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 351 -- HDD of VPSforPc
		BEGIN
			INSERT INTO @vhd
			SELECT (SELECT SUM(CAST([value] AS int)) AS value FROM dbo.SplitString(SIP.PropertyValue,';')) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID
			SET @Result = (SELECT SUM(Size) FROM @vhd)
		END
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
		ELSE IF @QuotaID = 731 -- Exchange2013.JournalingMailboxes
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID 
				AND ea.AccountType IN (12)
				AND ea.MailboxPlanId IS NOT NULL)
		ELSE IF @QuotaID = 77 -- Exchange2007.DiskSpace
			SET @Result = (SELECT SUM(B.MailboxSizeMB) FROM ExchangeAccounts AS ea 
			INNER JOIN ExchangeMailboxPlans AS B ON ea.MailboxPlanId = B.MailboxPlanId 
			INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
			INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
			WHERE pt.ParentPackageID = @PackageID AND ea.AccountType in (1, 5, 6, 10, 12))
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
		ELSE IF @QuotaID = 381 -- Dedicated Lync Phone Numbers
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 5)
		ELSE IF @QuotaID = 430 -- Enterprise Storage
			SET @Result = (SELECT SUM(ESF.FolderQuota) FROM EnterpriseFolders AS ESF
							INNER JOIN ServiceItems  SI ON ESF.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache PT ON SI.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 431 -- Enterprise Storage Folders
			SET @Result = (SELECT COUNT(ESF.EnterpriseFolderID) FROM EnterpriseFolders AS ESF
							INNER JOIN ServiceItems  SI ON ESF.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache PT ON SI.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 423 -- HostedSolution.SecurityGroups
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID AND ea.AccountType IN (8,9))
		ELSE IF @QuotaID = 495 -- HostedSolution.DeletedUsers
			SET @Result = (SELECT COUNT(ea.AccountID) FROM ExchangeAccounts AS ea
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE pt.ParentPackageID = @PackageID AND ea.AccountType = 11)
		ELSE IF @QuotaID = 450
			SET @Result = (SELECT COUNT(DISTINCT(RCU.[AccountId])) FROM [dbo].[RDSCollectionUsers] RCU
				INNER JOIN ExchangeAccounts EA ON EA.AccountId = RCU.AccountId
				INNER JOIN ServiceItems  si ON ea.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 451
			SET @Result = (SELECT COUNT(RS.[ID]) FROM [dbo].[RDSServers] RS				
				INNER JOIN ServiceItems  si ON RS.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 491
			SET @Result = (SELECT COUNT(RC.[ID]) FROM [dbo].[RDSCollections] RC
				INNER JOIN ServiceItems  si ON RC.ItemID = si.ItemID
				INNER JOIN PackagesTreeCache pt ON si.PackageID = pt.PackageID
				WHERE PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaName like 'ServiceLevel.%' -- Support Service Level Quota
		BEGIN
			DECLARE @LevelID int

			SELECT @LevelID = LevelID FROM SupportServiceLevels
			WHERE LevelName = REPLACE(@QuotaName,'ServiceLevel.','')

			IF (@LevelID IS NOT NULL)
			SET @Result = (SELECT COUNT(EA.AccountID)
				FROM SupportServiceLevels AS SL
				INNER JOIN ExchangeAccounts AS EA ON SL.LevelID = EA.LevelID
				INNER JOIN ServiceItems  SI ON EA.ItemID = SI.ItemID
				INNER JOIN PackagesTreeCache PT ON SI.PackageID = PT.PackageID
				WHERE EA.LevelID = @LevelID AND PT.ParentPackageID = @PackageID)
			ELSE SET @Result = 0
		END
		ELSE
			SET @Result = (SELECT COUNT(SI.ItemID) FROM Quotas AS Q
			INNER JOIN ServiceItems AS SI ON SI.ItemTypeID = Q.ItemTypeID
			INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID AND PT.ParentPackageID = @PackageID
			WHERE Q.QuotaID = @QuotaID)

		RETURN @Result
	END
GO

-- CRM2016 Provider

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted MS CRM 2016')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) 
VALUES(1206, 24, N'CRM', N'Hosted MS CRM 2016', N'SolidCP.Providers.HostedSolution.CRMProvider2016, SolidCP.Providers.HostedSolution.Crm2016', N'CRM2011', NULL)
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = '711' AND [ItemTypeID] = '73')
BEGIN
	UPDATE [dbo].[Quotas] SET [ItemTypeID] = '73' WHERE QuotaID = '711' AND [QuotaName] = N'MsSQL2017.Databases'
	UPDATE [dbo].[Quotas] SET [ItemTypeID] = '74' WHERE QuotaID = '712' AND [QuotaName] = N'MsSQL2017.Users'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'MSFTP100' AND [EditorControl] = 'MSFTP70')
BEGIN
	UPDATE [dbo].[Providers] SET [EditorControl] = 'MSFTP70' WHERE [ProviderName] = 'MSFTP100'
END
GO

-- cerberus FTP additions

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Cerberus FTP Server 6.x')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(110, 3, N'CerberusFTP6', N'Cerberus FTP Server 6.x', N'SolidCP.Providers.FTP.CerberusFTP6, SolidCP.Providers.FTP.CerberusFTP6', N'CerberusFTP6',	NULL)
END
GO

-- spamexperts additions

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'SpamExperts Mail Filter')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1602, 61, N'SpamExperts', N'SpamExperts Mail Filter', N'SolidCP.Providers.Filters.SpamExperts, SolidCP.Providers.Filters.SpamExperts', N'SpamExperts',	1)
END
GO

IF EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Exchange_anti_spam_Filters')
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupName] = 'MailFilters' WHERE [GroupName] = 'Exchange_anti_spam_Filters'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'Filters.EnableEmailUsers')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (448, 61, 2, N'Filters.EnableEmailUsers', N'Enable Per-Mailbox Login', 1, 0, NULL, NULL)
END
GO

UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = '1' WHERE [DisplayName] = 'PowerDNS'

-- Server 2019
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Windows Server 2019')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1800, 1, N'Windows2019', N'Windows Server 2019', N'SolidCP.Providers.OS.Windows2019, SolidCP.Providers.OS.Windows2019', N'Windows2012', null)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1800')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1800, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
END
GO

-- Server 2022
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1802')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1802, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
END

-- HyperV2019

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HyperV2019')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1801, 33, N'HyperV2019', N'Microsoft Hyper-V 2019', N'SolidCP.Providers.Virtualization.HyperV2019, SolidCP.Providers.Virtualization.HyperV2019', N'HyperV2012R2', 1)
END
GO

-- MySQL 8

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'MySQL8')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (90, N'MySQL8', 12, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 8.0')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(304, 90, N'MySQL', N'MySQL Server 8.0', N'SolidCP.Providers.Database.MySqlServer80, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'MySQL Server 8.0'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 8.1')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(305, 90, N'MySQL', N'MySQL Server 8.1', N'SolidCP.Providers.Database.MySqlServer81, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 8.2')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(306, 90, N'MySQL', N'MySQL Server 8.2', N'SolidCP.Providers.Database.MySqlServer82, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 8.3')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(307, 90, N'MySQL', N'MySQL Server 8.3', N'SolidCP.Providers.Database.MySqlServer83, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 8.4')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(308, 90, N'MySQL', N'MySQL Server 8.4', N'SolidCP.Providers.Database.MySqlServer84, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '304')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (304, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (304, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 8.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (304, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (304, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (304, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (304, N'sslmode', N'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '305')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 8.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'sslmode', N'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '306')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 8.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'sslmode', N'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '307')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (307, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (307, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 8.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (307, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (307, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (307, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (307, N'sslmode', N'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '308')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (308, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (308, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 8.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (308, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (308, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (308, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (308, N'sslmode', N'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceItemTypes] WHERE [GroupID] = '90')
BEGIN
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (75, 90, N'MySQL8Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 18, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (76, 90, N'MySQL8User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 19, 0, 0, 0, 1, 1, 1, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = '90')
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (110, 90, 1, N'MySQL8.Databases', N'Databases', 2, 0, 75, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (111, 90, 2, N'MySQL8.Users', N'Users', 2, 0, 76, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (112, 90, 4, N'MySQL8.Backup', N'Database Backups', 1, 0, NULL, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (113, 90, 3, N'MySQL8.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (114, 90, 5, N'MySQL8.Restore', N'Database Restores', 1, 0, NULL, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (115, 90, 6, N'MySQL8.Truncate', N'Database Truncate', 1, 0, NULL, NULL, NULL)
END
GO

-- MySQL 9.0

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'MySQL9')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (91, N'MySQL9', 12, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 9.0')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(320, 90, N'MySQL', N'MySQL Server 9.0', N'SolidCP.Providers.Database.MySqlServer90, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '320')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (320, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (320, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 9.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (320, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (320, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (320, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (320, N'sslmode', N'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceItemTypes] WHERE [GroupID] = '91')
BEGIN
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (90, 91, N'MySQL9Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 20, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (91, 91, N'MySQL9User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 21, 0, 0, 0, 1, 1, 1, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = '91')
BEGIN
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (120, 91, 1, N'MySQL9.Databases', N'Databases', 2, 0, 75, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (121, 91, 2, N'MySQL9.Users', N'Users', 2, 0, 76, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (122, 91, 4, N'MySQL9.Backup', N'Database Backups', 1, 0, NULL, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (123, 91, 3, N'MySQL9.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (124, 91, 5, N'MySQL9.Restore', N'Database Restores', 1, 0, NULL, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (125, 91, 6, N'MySQL9.Truncate', N'Database Truncate', 1, 0, NULL, NULL, NULL)
END
GO


-- RDS Provider

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Remote Desktop Services Windows 2016')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) 
VALUES(1502, 45, N'RemoteDesktopServices2012', N'Remote Desktop Services Windows 2016', N'SolidCP.Providers.RemoteDesktopServices.Windows2016,SolidCP.Providers.RemoteDesktopServices.Windows2016', N'RDS',	1)
END
GO

-- RDS Provider 2019

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Remote Desktop Services Windows 2019')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) 
VALUES(1503, 45, N'RemoteDesktopServices2019', N'Remote Desktop Services Windows 2019', N'SolidCP.Providers.RemoteDesktopServices.Windows2019,SolidCP.Providers.RemoteDesktopServices.Windows2019', N'RDS',	1)
END
GO


-- MariaDB 10.3

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1570')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1570, 50, N'MariaDB', N'MariaDB 10.3', N'SolidCP.Providers.Database.MariaDB103, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 50 WHERE [ProviderID] = '1570'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1570')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1570, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1570, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.3')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1570, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1570, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1570, N'RootPassword', N'')
END
GO


-- MariaDB 10.4-11.7

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1571')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1571, 50, N'MariaDB', N'MariaDB 10.4', N'SolidCP.Providers.Database.MariaDB104, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1572')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1572, 50, N'MariaDB', N'MariaDB 10.5', N'SolidCP.Providers.Database.MariaDB105, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1573')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1573, 50, N'MariaDB', N'MariaDB 10.6', N'SolidCP.Providers.Database.MariaDB106, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1574')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1574, 50, N'MariaDB', N'MariaDB 10.7', N'SolidCP.Providers.Database.MariaDB107, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1575')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1575, 50, N'MariaDB', N'MariaDB 10.8', N'SolidCP.Providers.Database.MariaDB108, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1576')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1576, 50, N'MariaDB', N'MariaDB 10.9', N'SolidCP.Providers.Database.MariaDB109, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1577')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1577, 50, N'MariaDB', N'MariaDB 10.10', N'SolidCP.Providers.Database.MariaDB1010, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1578')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1578, 50, N'MariaDB', N'MariaDB 10.11', N'SolidCP.Providers.Database.MariaDB1011, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1579')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1579, 50, N'MariaDB', N'MariaDB 11.0', N'SolidCP.Providers.Database.MariaDB110, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1580')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1580, 50, N'MariaDB', N'MariaDB 11.1', N'SolidCP.Providers.Database.MariaDB111, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1581')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1581, 50, N'MariaDB', N'MariaDB 11.2', N'SolidCP.Providers.Database.MariaDB112, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1582')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1582, 50, N'MariaDB', N'MariaDB 11.3', N'SolidCP.Providers.Database.MariaDB113, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1583')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1583, 50, N'MariaDB', N'MariaDB 11.4', N'SolidCP.Providers.Database.MariaDB114, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1584')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1584, 50, N'MariaDB', N'MariaDB 11.5', N'SolidCP.Providers.Database.MariaDB115, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1585')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1585, 50, N'MariaDB', N'MariaDB 11.6', N'SolidCP.Providers.Database.MariaDB116, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1586')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1586, 50, N'MariaDB', N'MariaDB 11.7', N'SolidCP.Providers.Database.MariaDB117, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END


IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1571')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.4')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'RootPassword', N'')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1572')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.5')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1573')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.6')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1574')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1574, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1574, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.7')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1574, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1574, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1574, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1575')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1575, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1575, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.8')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1575, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1575, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1575, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1576')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1576, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1576, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.9')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1576, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1576, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1576, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1577')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1577, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1577, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.10')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1577, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1577, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1577, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1578')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1578, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1578, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.11')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1578, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1578, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1578, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1579')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1579, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1579, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1579, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1579, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1579, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1580')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1580, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1580, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.1')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1580, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1580, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1580, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1581')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1581, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1581, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.2')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1581, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1581, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1581, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1582')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1582, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1582, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.3')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1582, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1582, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1582, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1583')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1583, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1583, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.4')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1583, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1583, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1583, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1584')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1584, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1584, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.5')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1584, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1584, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1584, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1585')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1585, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1585, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.6')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1585, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1585, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1585, N'RootPassword', N'')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1586')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1586, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1586, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 11.7')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1586, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1586, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1586, N'RootPassword', N'')
END
GO

--


IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted Microsoft Exchange Server 2019')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(93, 12, N'Exchange2016', N'Hosted Microsoft Exchange Server 2019', N'SolidCP.Providers.HostedSolution.Exchange2019, SolidCP.Providers.HostedSolution.Exchange2019', N'Exchange',	NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted Microsoft Exchange Server 2019'
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'SfB2019')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery])
VALUES (1404, 52, N'SfB2019', N'Microsoft Skype for Business Server 2019', N'SolidCP.Providers.HostedSolution.SfB2019, SolidCP.Providers.HostedSolution.SfB2019', N'SfB', NULL)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Hosted SharePoint 2019')
BEGIN
DECLARE @group_id AS INT
SELECT @group_id = GroupId FROM [dbo].[ResourceGroups] WHERE GroupName = 'Sharepoint Enterprise Server'
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1711, @group_id, N'HostedSharePoint2019', N'Hosted SharePoint 2019', N'SolidCP.Providers.HostedSolution.HostedSharePointServer2019, SolidCP.Providers.HostedSolution.SharePoint2019', N'HostedSharePoint30', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL WHERE [DisplayName] = 'Hosted SharePoint 2019'
END
GO

-- RDS
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'CheckRDSServer')
DROP PROCEDURE CheckRDSServer
GO

CREATE PROCEDURE [dbo].[CheckRDSServer]
(
	@ServerFQDN nvarchar(100),
	@Result int OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
*/

SET @Result = 0 -- OK

-- check if the domain already exists
IF EXISTS(
SELECT FqdName FROM RDSServers
WHERE FqdName = @ServerFQDN
)
BEGIN
	SET @Result = -1
	RETURN
END

RETURN



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSServerById')
DROP PROCEDURE GetRDSServerById
GO


CREATE PROCEDURE [dbo].[GetRDSServerById]
(
	@ID INT
)
AS
SELECT TOP 1
	RS.Id,
	RS.ItemID,
	RS.Name, 
	RS.FqdName,
	RS.Description,
	RS.RdsCollectionId,
	RS.ConnectionEnabled,
	SI.ItemName,
	RC.Name AS CollectionName
	FROM RDSServers AS RS
	LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = RS.ItemId
	LEFT OUTER JOIN  RDSCollections AS RC ON RC.ID = RdsCollectionId
	WHERE RS.Id = @Id
	
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetRDSServersPaged')
DROP PROCEDURE GetRDSServersPaged
GO

CREATE PROCEDURE [dbo].[GetRDSServersPaged]
(
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@ItemID int,
	@IgnoreItemId bit,
	@RdsCollectionId int,
	@IgnoreRdsCollectionId bit,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Controller int,
	@ControllerName nvarchar(50) = ''
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows

DECLARE @RDSServer TABLE
(
	ItemPosition int IDENTITY(0,1),
	RDSServerId int
)
INSERT INTO @RDSServer (RDSServerId)
SELECT
	S.ID
FROM RDSServers AS S
LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = S.ItemId
LEFT OUTER JOIN  Services AS SE ON SE.ServiceID = S.Controller
LEFT OUTER JOIN  RDSCollections AS RC ON RC.ID = S.RdsCollectionId
WHERE 
	((((@ItemID is Null AND S.ItemID is null ) or (@IgnoreItemId = 1 ))
		or (@ItemID is not Null AND S.ItemID = @ItemID ))
	and
	(((@RdsCollectionId is Null AND S.RDSCollectionId is null) or @IgnoreRdsCollectionId = 1)
		or (@RdsCollectionId is not Null AND S.RDSCollectionId = @RdsCollectionId)))'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE ''%' + @FilterValue + '%'''

IF @Controller <> ''
SET @sql = @sql + ' AND Controller = @Controller '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(RDSServerId) FROM @RDSServer;
SELECT
	ST.ID,
	ST.ItemID,
	ST.Name, 
	ST.FqdName,
	ST.Description,
	ST.RdsCollectionId,
	SI.ItemName,
	ST.ConnectionEnabled,
	ST.Controller,
	SE.ServiceName as ControllerName,
	RC.Name as CollectionName
FROM @RDSServer AS S
INNER JOIN RDSServers AS ST ON S.RDSServerId = ST.ID
LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = ST.ItemId
LEFT OUTER JOIN  Services AS SE ON SE.ServiceID = ST.Controller
LEFT OUTER JOIN  RDSCollections AS RC ON RC.ID = ST.RdsCollectionId
WHERE S.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int,  @FilterValue nvarchar(50),  @ItemID int, @RdsCollectionId int, @IgnoreItemId bit, @IgnoreRdsCollectionId bit, @Controller int, @ControllerName nvarchar(50)',
@StartRow, @MaximumRows,  @FilterValue,  @ItemID, @RdsCollectionId, @IgnoreItemId , @IgnoreRdsCollectionId, @Controller, @ControllerName

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Fix for MySQL8 Quota
UPDATE [Quotas] SET [ItemTypeID] = '75' WHERE [QuotaID] = '110' AND [ItemTypeID] = '23'
GO
UPDATE [Quotas] SET [ItemTypeID] = '76' WHERE [QuotaID] = '111' AND [ItemTypeID] = '24'
GO


/* SQL 2019 Provider */
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'MsSQL2019')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (74, N'MsSQL2019', 10, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
ELSE
BEGIN
UPDATE [dbo].[ResourceGroups] SET [ShowGroup] = 1 WHERE [GroupName] = 'MsSQL2019'
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Microsoft SQL Server 2019')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1705, 74, N'MsSQL', N'Microsoft SQL Server 2019', N'SolidCP.Providers.Database.MsSqlServer2019, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (77, 74, N'MsSQL2019Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (78, 74, N'MsSQL2019User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (721, 74, 1, N'MsSQL2019.Databases', N'Databases', 2, 0, 77, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (722, 74, 2, N'MsSQL2019.Users', N'Users', 2, 0, 78, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (723, 74, 3, N'MsSQL2019.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (724, 74, 5, N'MsSQL2019.Backup', N'Database Backups', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (725, 74, 6, N'MsSQL2019.Restore', N'Database Restores', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (726, 74, 7, N'MsSQL2019.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (727, 74, 4, N'MsSQL2019.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 74 WHERE [DisplayName] = 'Microsoft SQL Server 2019'
END
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetParentPackageQuotas')
DROP PROCEDURE GetParentPackageQuotas
GO

CREATE PROCEDURE [dbo].[GetParentPackageQuotas]
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorParentPackageRights(@ActorID, @PackageID) = 0
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
	--dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, 0) AS ParentEnabled
	CASE
		WHEN RG.GroupName = 'Service Levels' THEN dbo.GetPackageServiceLevelResource(@ParentPackageID, RG.GroupID, 0)
		ELSE dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, 0)
	END AS ParentEnabled
FROM ResourceGroups AS RG
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
--WHERE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, 0) = 1
WHERE (dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, 0) = 1 AND RG.GroupName <> 'Service Levels') OR
	  (dbo.GetPackageServiceLevelResource(@PackageID, RG.GroupID, 0) = 1 AND RG.GroupName = 'Service Levels')
ORDER BY RG.GroupOrder

-- return quotas
DECLARE @OrgsCount INT
SET @OrgsCount = dbo.GetPackageAllocatedQuota(@PackageID, 205) -- 205 - HostedSolution.Organizations
SET @OrgsCount = CASE WHEN ISNULL(@OrgsCount, 0) < 1 THEN 1 ELSE @OrgsCount END

SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	QuotaValue = CASE WHEN Q.PerOrganization = 1 AND dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) <> -1 THEN 
					dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) * @OrgsCount 
				 ELSE 
					dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) 
				 END,
	QuotaValuePerOrganization = dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID),
	dbo.GetPackageAllocatedQuota(@ParentPackageID, Q.QuotaID) AS ParentQuotaValue,
	ISNULL(dbo.CalculateQuotaUsage(@PackageID, Q.QuotaID), 0) AS QuotaUsedValue,
	Q.PerOrganization
FROM Quotas AS Q
WHERE Q.HideQuota IS NULL OR Q.HideQuota = 0
ORDER BY Q.QuotaOrder

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='Servers' AND COLS.name='AdParentDomain')
BEGIN
ALTER TABLE [dbo].[Servers] ADD
	[AdParentDomain] [nvarchar](200) NULL,
	[AdParentDomainController] [nvarchar](200) NULL
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateServer')
DROP PROCEDURE UpdateServer
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
	@ADAuthenticationType varchar(50),
	@ADParentDomain nvarchar(200),
	@ADParentDomainController nvarchar(200)
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
	ADParentDomainController = @ADParentDomainController
WHERE ServerID = @ServerID
RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetServer')
DROP PROCEDURE GetServer
GO

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
	ADParentDomainController
FROM Servers
WHERE
	ServerID = @ServerID
	AND (@IsAdmin = 1 OR @forAutodiscover = 1)

RETURN
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetServerInternal')
DROP PROCEDURE GetServerInternal
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
	ADAuthenticationType,
	ADParentDomain,
	ADParentDomainController
FROM Servers
WHERE
	ServerID = @ServerID

RETURN
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetServerByName')
DROP PROCEDURE GetServerByName
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
	ADAuthenticationType,
	ADParentDomain,
	ADParentDomainController
FROM Servers
WHERE
	ServerName = @ServerName
	AND @IsAdmin = 1

RETURN
GO

-- Audit Log Sources and Tasks
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'REMOTE_DESKTOP_SERVICES')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'REMOTE_DESKTOP_SERVICES')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'OCS')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'OCS')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'ORGANIZATION')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'ORGANIZATION')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'VPS2012')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'VPS2012')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'AUTO_DISCOVERY')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'AUTO_DISCOVERY')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'STORAGE_SPACES')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'STORAGE_SPACES')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'ENTERPRISE_STORAGE')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'ENTERPRISE_STORAGE')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'VLAN')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'VLAN')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'WAG_INSTALLER')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'WAG_INSTALLER')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'HOSTING_SPACE_WR')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'HOSTING_SPACE_WR')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogSources] WHERE [SourceName] = 'HOSTING_SPACE')
BEGIN
	INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'HOSTING_SPACE')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'HOSTING_SPACE_WR' AND [TaskName] = 'ADD')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'HOSTING_SPACE_WR', N'ADD', N'Add')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'HOSTING_SPACE' AND [TaskName] = 'ADD')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'HOSTING_SPACE', N'ADD', N'Add')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'REMOTE_DESKTOP_SERVICES' AND [TaskName] = 'ADD_RDS_SERVER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'REMOTE_DESKTOP_SERVICES', N'ADD_RDS_SERVER', N'Add RDS server')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'REMOTE_DESKTOP_SERVICES' AND [TaskName] = 'RESTART_RDS_SERVER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'REMOTE_DESKTOP_SERVICES', N'RESTART_RDS_SERVER', N'Restart RDS server')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'REMOTE_DESKTOP_SERVICES' AND [TaskName] = 'SET_RDS_SERVER_NEW_CONNECTIONS_ALLOWED')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'REMOTE_DESKTOP_SERVICES', N'SET_RDS_SERVER_NEW_CONNECTIONS_ALLOWED', N'Set RDS new connection allowed')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'CREATE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'CREATE', N'Create VM')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'REINSTALL')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'REINSTALL', N'Reinstall VM')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ENTERPRISE_STORAGE' AND [TaskName] = 'GET_ORG_STATS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ENTERPRISE_STORAGE', N'GET_ORG_STATS', N'Get organization statistics')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ENTERPRISE_STORAGE' AND [TaskName] = 'CREATE_MAPPED_DRIVE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ENTERPRISE_STORAGE', N'CREATE_MAPPED_DRIVE', N'Create mapped drive')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ENTERPRISE_STORAGE' AND [TaskName] = 'DELETE_MAPPED_DRIVE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ENTERPRISE_STORAGE', N'DELETE_MAPPED_DRIVE', N'Delete mapped drive')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ENTERPRISE_STORAGE' AND [TaskName] = 'CREATE_FOLDER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ENTERPRISE_STORAGE', N'CREATE_FOLDER', N'Create folder')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ENTERPRISE_STORAGE' AND [TaskName] = 'DELETE_FOLDER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ENTERPRISE_STORAGE', N'DELETE_FOLDER', N'Delete folder')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ENTERPRISE_STORAGE' AND [TaskName] = 'SET_ENTERPRISE_FOLDER_GENERAL_SETTINGS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ENTERPRISE_STORAGE', N'SET_ENTERPRISE_FOLDER_GENERAL_SETTINGS', N'Set enterprise folder general settings')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'GET_ORG_STATS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'GET_ORG_STATS', N'Get organization statistics')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'GET_SUPPORT_SERVICE_LEVELS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'GET_SUPPORT_SERVICE_LEVELS', N'Get support service levels')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'GET_SECURITY_GROUPS_BYMEMBER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'GET_SECURITY_GROUPS_BYMEMBER', N'Get security groups by member')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'GET_SECURITY_GROUP_GENERAL')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'GET_SECURITY_GROUP_GENERAL', N'Get security group general settings')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'SET_USER_PASSWORD')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'SET_USER_PASSWORD', N'Set user password')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'SET_USER_USERPRINCIPALNAME')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'SET_USER_USERPRINCIPALNAME', N'Set user principal name')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'UPDATE_USER_GENERAL')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'UPDATE_USER_GENERAL', N'Update user general settings')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'CREATE_USER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'CREATE_USER', N'Create user')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'REMOVE_USER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'REMOVE_USER', N'Remove user')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'CREATE_ORG')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'CREATE_ORG', N'Create organization')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'DELETE_ORG')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'DELETE_ORG', N'Delete organization')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'CREATE_ORGANIZATION_ENTERPRISE_STORAGE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'CREATE_ORGANIZATION_ENTERPRISE_STORAGE', N'Create organization enterprise storage')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'SEND_USER_PASSWORD_RESET_EMAIL_PINCODE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'SEND_USER_PASSWORD_RESET_EMAIL_PINCODE', N'Send user password reset email pincode')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'UPDATE_PASSWORD_SETTINGS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'UPDATE_PASSWORD_SETTINGS', N'Update password settings')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'CREATE_SECURITY_GROUP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'CREATE_SECURITY_GROUP', N'Create security group')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'UPDATE_SECURITY_GROUP_GENERAL')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'UPDATE_SECURITY_GROUP_GENERAL', N'Update security group general settings')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'ORGANIZATION' AND [TaskName] = 'DELETE_SECURITY_GROUP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ORGANIZATION', N'DELETE_SECURITY_GROUP', N'Delete security group')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'AUTO_DISCOVERY' AND [TaskName] = 'IS_INSTALLED')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'AUTO_DISCOVERY', N'IS_INSTALLED', N'Is installed')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_EXCHANGE_MAILBOXPLANS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_EXCHANGE_MAILBOXPLANS', N'Get Exchange Mailbox plans')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_EXCHANGE_MAILBOXPLAN')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_EXCHANGE_MAILBOXPLAN', N'Get Exchange Mailbox plan')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_EXCHANGE_ACCOUNTDISCLAIMERID')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_EXCHANGE_ACCOUNTDISCLAIMERID', N'Get Exchange account disclaimer id')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_EXCHANGE_EXCHANGEDISCLAIMER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_EXCHANGE_EXCHANGEDISCLAIMER', N'Get Exchange disclaimer')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'ADD_EXCHANGE_EXCHANGEDISCLAIMER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_EXCHANGE_EXCHANGEDISCLAIMER', N'Add Exchange disclaimer')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_MAILBOX_STATS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_STATS', N'Get Mailbox statistics')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_ACTIVESYNC_POLICY')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_ACTIVESYNC_POLICY', N'Get Activesync policy')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'SET_MAILBOXPLAN_RETENTIONPOLICY_ARCHIVING')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_MAILBOXPLAN_RETENTIONPOLICY_ARCHIVING', N'Set Mailbox plan retention policy archiving')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_EXCHANGE_RETENTIONPOLICYTAG')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_EXCHANGE_RETENTIONPOLICYTAG', N'Get Exchange retention policy tag')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'ADD_EXCHANGE_RETENTIONPOLICYTAG')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_EXCHANGE_RETENTIONPOLICYTAG', N'Add Exchange retention policy tag')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_EXCHANGE_RETENTIONPOLICYTAGS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_EXCHANGE_RETENTIONPOLICYTAGS', N'Get Exchange retention policy tags')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'DELETE_EXCHANGE_RETENTIONPOLICYTAG')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_EXCHANGE_RETENTIONPOLICYTAG', N'Delete Exchange retention policy tag')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'UPDATE_EXCHANGE_RETENTIONPOLICYTAG')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_EXCHANGE_RETENTIONPOLICYTAG', N'Update Exchange retention policy tag')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'ADD_EXCHANGE_MAILBOXPLAN_RETENTIONPOLICY_ARCHIVING')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_EXCHANGE_MAILBOXPLAN_RETENTIONPOLICY_ARCHIVING', N'Add Exchange archiving retention policy')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'DELETE_EXCHANGE_MAILBOXPLAN_RETENTIONPOLICY_ARCHIV')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_EXCHANGE_MAILBOXPLAN_RETENTIONPOLICY_ARCHIV', N'Delete Exchange archiving retention policy')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_PICTURE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_PICTURE', N'Get picture')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'UPDATE_MAILBOX_AUTOREPLY')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_MAILBOX_AUTOREPLY', N'Update Mailbox autoreply')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_MAILBOX_AUTOREPLY')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_AUTOREPLY', N'Get Mailbox autoreply')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_DISTR_LIST_BYMEMBER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTR_LIST_BYMEMBER', N'Get distributions list by member')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_DISTRIBUTION_LIST_RESULT')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTRIBUTION_LIST_RESULT', N'Get distributions list result')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_MOBILE_DEVICES')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MOBILE_DEVICES', N'Get mobile devices')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_MAILBOX_PERMISSIONS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_PERMISSIONS', N'Get Mailbox permissions')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'DISABLE_MAILBOX')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DISABLE_MAILBOX', N'Disable Mailbox')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'SET_EXCHANGE_ACCOUNTDISCLAIMERID')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_EXCHANGE_ACCOUNTDISCLAIMERID', N'Set exchange account disclaimer id')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'SET_EXCHANGE_MAILBOXPLAN')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_EXCHANGE_MAILBOXPLAN', N'Set exchange Mailbox plan')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'GET_RESOURCE_MAILBOX')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_RESOURCE_MAILBOX', N'Get resource Mailbox settings')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'EXCHANGE' AND [TaskName] = 'UPDATE_RESOURCE_MAILBOX')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_RESOURCE_MAILBOX', N'Update resource Mailbox settings')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'STORAGE_SPACES' AND [TaskName] = 'SAVE_STORAGE_SPACE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STORAGE_SPACES', N'SAVE_STORAGE_SPACE', N'Save storage space')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'STORAGE_SPACES' AND [TaskName] = 'SAVE_STORAGE_SPACE_LEVEL')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STORAGE_SPACES', N'SAVE_STORAGE_SPACE_LEVEL', N'Save storage space level')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'STORAGE_SPACES' AND [TaskName] = 'REMOVE_STORAGE_SPACE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STORAGE_SPACES', N'REMOVE_STORAGE_SPACE', N'Remove storage space')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'DOMAIN' AND [TaskName] = 'ENABLE_DNS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DOMAIN', N'ENABLE_DNS', N'Enable DNS')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VLAN' AND [TaskName] = 'ADD')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VLAN', N'ADD', N'Add')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VLAN' AND [TaskName] = 'ADD_RANGE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VLAN', N'ADD_RANGE', N'Add range')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VLAN' AND [TaskName] = 'DELETE_RANGE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VLAN', N'DELETE_RANGE', N'Delete range')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VLAN' AND [TaskName] = 'UPDATE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VLAN', N'UPDATE', N'Update')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VLAN' AND [TaskName] = 'ALLOCATE_PACKAGE_VLAN')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VLAN', N'ALLOCATE_PACKAGE_VLAN', N'Allocate package VLAN')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VLAN' AND [TaskName] = 'DEALLOCATE_PACKAGE_VLAN')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VLAN', N'DEALLOCATE_PACKAGE_VLAN', N'Deallocate package VLAN')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'WAG_INSTALLER' AND [TaskName] = 'GET_GALLERY_APPS_TASK')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WAG_INSTALLER', N'GET_GALLERY_APPS_TASK', N'Get gallery applications')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'WAG_INSTALLER' AND [TaskName] = 'GET_GALLERY_CATEGORIES_TASK')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WAG_INSTALLER', N'GET_GALLERY_CATEGORIES_TASK', N'Get gallery categories')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'WAG_INSTALLER' AND [TaskName] = 'GET_GALLERY_APP_DETAILS_TASK')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WAG_INSTALLER', N'GET_GALLERY_APP_DETAILS_TASK', N'Get gallery application details')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'WAG_INSTALLER' AND [TaskName] = 'INSTALL_WEB_APP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WAG_INSTALLER', N'INSTALL_WEB_APP', N'Install Web application')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'WAG_INSTALLER' AND [TaskName] = 'GET_APP_PARAMS_TASK')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WAG_INSTALLER', N'GET_APP_PARAMS_TASK', N'Get application parameters')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'WAG_INSTALLER' AND [TaskName] = 'GET_SRV_GALLERY_APPS_TASK')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WAG_INSTALLER', N'GET_SRV_GALLERY_APPS_TASK', N'Get server gallery applications')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'WEB_SITE' AND [TaskName] = 'GET_STATE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'GET_STATE', N'Get state')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'OCS' AND [TaskName] = 'CREATE_OCS_USER')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'OCS', N'CREATE_OCS_USER', N'Create OCS user')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'OCS' AND [TaskName] = 'GET_OCS_USERS_COUNT')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'OCS', N'GET_OCS_USERS_COUNT', N'Get OCS users count')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'OCS' AND [TaskName] = 'GET_OCS_USERS')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'OCS', N'GET_OCS_USERS', N'Get OCS users')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'ADD_EXTERNAL_IP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'ADD_EXTERNAL_IP', N'Add external IP')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'ADD_PRIVATE_IP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'ADD_PRIVATE_IP', N'Add private IP')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'APPLY_SNAPSHOT')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'APPLY_SNAPSHOT', N'Apply VM snapshot')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'CHANGE_ADMIN_PASSWORD')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'CHANGE_ADMIN_PASSWORD', N'Change administrator password')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'CHANGE_STATE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'CHANGE_STATE', N'Change VM state')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'DELETE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'DELETE', N'Delete VM')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'DELETE_EXTERNAL_IP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'DELETE_EXTERNAL_IP', N'Delete external IP')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'DELETE_PRIVATE_IP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'DELETE_PRIVATE_IP', N'Delete private IP')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'DELETE_SNAPSHOT')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'DELETE_SNAPSHOT', N'Delete VM snapshot')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'DELETE_SNAPSHOT_SUBTREE')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'DELETE_SNAPSHOT_SUBTREE', N'Delete VM snapshot subtree')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'EJECT_DVD_DISK')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'EJECT_DVD_DISK', N'Eject DVD disk')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'INSERT_DVD_DISK')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'INSERT_DVD_DISK', N'Insert DVD disk')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'RENAME_SNAPSHOT')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'RENAME_SNAPSHOT', N'Rename VM snapshot')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'SET_PRIMARY_EXTERNAL_IP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'SET_PRIMARY_EXTERNAL_IP', N'Set primary external IP')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'SET_PRIMARY_PRIVATE_IP')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'SET_PRIMARY_PRIVATE_IP', N'Set primary private IP')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'TAKE_SNAPSHOT')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'TAKE_SNAPSHOT', N'Take VM snapshot')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'UPDATE_CONFIGURATION')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'UPDATE_CONFIGURATION', N'Update VM configuration')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[AuditLogTasks] WHERE [SourceName] = 'VPS2012' AND [TaskName] = 'UPDATE_HOSTNAME')
BEGIN
	INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS2012', N'UPDATE_HOSTNAME', N'Update host name')
END
GO



IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddPackage')
DROP PROCEDURE AddPackage
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


DECLARE @datelastyear datetime = DATEADD(year,-1,GETDATE())

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
	PurchaseDate,
	BandwidthUpdated
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
	@PurchaseDate,
	@datelastyear
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


UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = '1' WHERE [DisplayName] = 'Web Application Engines'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;6;20;0;1;0;True;;0;;;False;False;0;' Where [SettingsName] = 'SolidCPPolicy' AND [PropertyName] = 'PasswordPolicy' AND [PropertyValue] LIKE N'True;6;20;0;1;0;True'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;False;;0;0;0;False;False;0;' Where [SettingsName] = 'WebPolicy' AND [PropertyName] = 'SecuredUserPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;False'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;False;;0;0;0;False;False;0;' Where [SettingsName] = 'WebPolicy' AND [PropertyName] = 'FrontPagePasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;False'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;True;;0;;;False;False;0;' Where [SettingsName] = 'FtpPolicy' AND [PropertyName] = 'UserPasswordPolicy' AND [PropertyValue] LIKE N'True;-;1;20;;;'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;False;;0;;;False;False;0;' Where [SettingsName] = 'MailPolicy' AND [PropertyName] = 'AccountPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;False'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;True;;0;0;0;False;False;0;' Where [SettingsName] = 'MsSqlPolicy' AND [PropertyName] = 'UserPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;True'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;False;;0;0;0;False;False;0;' Where [SettingsName] = 'MySqlPolicy' AND [PropertyName] = 'UserPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;False'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;False;;0;;;False;False;0;' Where [SettingsName] = 'MariaDBPolicy' AND [PropertyName] = 'UserPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;False'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;True;;0;;;False;False;0;' Where [SettingsName] = 'SharePointPolicy' AND [PropertyName] = 'UserPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;True'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;1;0;True;;0;;;False;False;0;' Where [SettingsName] = 'SharePointPolicy' AND [PropertyName] = 'UserPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;True'
GO

UPDATE [dbo].[UserSettings] SET [PropertyValue] = N'True;5;20;0;2;0;True;;0;;;False;False;0;' Where [SettingsName] = 'ExchangePolicy' AND [PropertyName] = 'MailboxPasswordPolicy' AND [PropertyValue] LIKE N'True;5;20;0;1;0;True'
GO

UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = '1' WHERE [DisplayName] = 'Microsoft SQL Server 2017'
GO

UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = '1' WHERE [DisplayName] = 'Microsoft SQL Server 2019'
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '410')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'admode', N'False')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'expirelimit', N'1209600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'minimumttl', N'86400')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'nameservers', N'ns1.yourdomain.com;ns2.yourdomain.com')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'refreshinterval', N'3600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'responsibleperson', N'hostmaster.[DOMAIN_NAME]')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'retrydelay', N'600')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1902')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'admode', N'False')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'expirelimit', N'1209600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'minimumttl', N'86400')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'nameservers', N'ns1.yourdomain.com;ns2.yourdomain.com')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'refreshinterval', N'3600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'responsibleperson', N'hostmaster.[DOMAIN_NAME]')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'retrydelay', N'600')
END
GO

UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = '1' WHERE [DisplayName] = 'MariaDB 10.3'
GO

-- send audit log report task

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT')
BEGIN
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'SolidCP.EnterpriseServer.AuditLogReportTask, SolidCP.EnterpriseServer.Code', 3)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT' AND [ParameterID] = N'MAIL_TO')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'MAIL_TO', N'String', NULL, 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT' AND [ParameterID] = N'AUDIT_LOG_SEVERITY')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'AUDIT_LOG_SEVERITY', N'List', N'-1=All;0=Information;1=Warning;2=Error', 2)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT' AND [ParameterID] = N'AUDIT_LOG_SOURCE')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'AUDIT_LOG_SOURCE', N'List', N'', 3)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT' AND [ParameterID] = N'AUDIT_LOG_TASK')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'AUDIT_LOG_TASK', N'List', N'', 4)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT' AND [ParameterID] = N'AUDIT_LOG_DATE')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'AUDIT_LOG_DATE', N'List', N'today=Today;yesterday=Yesterday;schedule=Schedule', 5)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT' AND [ParameterID] = N'SHOW_EXECUTION_LOG')
BEGIN
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'SHOW_EXECUTION_LOG', N'List', N'0=No;1=Yes', 6)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskViewConfiguration] WHERE [TaskID] = N'SCHEDULE_TASK_AUDIT_LOG_REPORT')
BEGIN
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_AUDIT_LOG_REPORT', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/SolidCP/ScheduleTaskControls/AuditLogReportView.ascx')
END
GO


-- MariaDB 10.4

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1571')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1571, 50, N'MariaDB', N'MariaDB 10.4', N'SolidCP.Providers.Database.MariaDB104, SolidCP.Providers.Database.MariaDB', N'MariaDB', N'1')
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 50 WHERE [ProviderID] = '1571'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1571')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.4')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1571, N'RootPassword', N'')
END
GO

-- MariaDB 10.5

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1572')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1572, 50, N'MariaDB', N'MariaDB 10.5', N'SolidCP.Providers.Database.MariaDB105, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 50 WHERE [ProviderID] = '1572'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1572')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.5')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1572, N'RootPassword', N'')
END
GO

-- Fix for IIS8 and IIS10 SSL

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '105' AND [PropertyName] = N'sslusesni')
BEGIN
INSERT  [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (105, N'sslusesni', N'True')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '112' AND [PropertyName] = N'sslusesni')
BEGIN
INSERT  [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (112, N'sslusesni', N'True')
END
GO

----------------------------------------------
-- INDEXES SECTION - ADD/REMOVE/MODIFY HERE --
----------------------------------------------
---[dbo].[AccessTokens] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'AccessTokensIdx_AccountID' AND [object_id] = OBJECT_ID('[dbo].[AccessTokens]'))
BEGIN
	CREATE INDEX AccessTokensIdx_AccountID ON [dbo].[AccessTokens] ([AccountID]);
END
GO

---[dbo].[AuditLog] Indexes
--- TODO: add missed FKs and indexes?
--- PS: There are problems with type of data we keep in possible FKs, like 0 in ID columns (it is wrong)

---[dbo].[BackgroundTaskLogs] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'BackgroundTaskLogsIdx_TaskID' AND [object_id] = OBJECT_ID('[dbo].[BackgroundTaskLogs]'))
BEGIN
	CREATE INDEX BackgroundTaskLogsIdx_TaskID ON [dbo].[BackgroundTaskLogs] ([TaskID]);
END
GO

---[dbo].[BackgroundTaskParameters] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'BackgroundTaskParametersIdx_TaskID' AND [object_id] = OBJECT_ID('[dbo].[BackgroundTaskParameters]'))
BEGIN
	CREATE INDEX BackgroundTaskParametersIdx_TaskID ON [dbo].[BackgroundTaskParameters] ([TaskID]);
END
GO

---[dbo].[BackgroundTasks] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[BackgroundTaskStack] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'BackgroundTaskStackIdx_TaskID' AND [object_id] = OBJECT_ID('[dbo].[BackgroundTaskStack]'))
BEGIN
	CREATE INDEX BackgroundTaskStackIdx_TaskID ON [dbo].[BackgroundTaskStack] ([TaskID]);
END
GO

---[dbo].[BlackBerryUsers] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'BlackBerryUsersIdx_AccountId' AND [object_id] = OBJECT_ID('[dbo].[BlackBerryUsers]'))
BEGIN
	CREATE INDEX BlackBerryUsersIdx_AccountId ON [dbo].[BlackBerryUsers] ([AccountId]);
END
GO

---[dbo].[Comments] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'CommentsIdx_UserID' AND [object_id] = OBJECT_ID('[dbo].[Comments]'))
BEGIN
	CREATE INDEX CommentsIdx_UserID ON [dbo].[Comments] ([UserID]);
END
GO
--- TODO: add missed FKs and indexes?

---[dbo].[CRMUsers] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'CRMUsersIdx_AccountID' AND [object_id] = OBJECT_ID('[dbo].[CRMUsers]'))
BEGIN
	CREATE INDEX CRMUsersIdx_AccountID ON [dbo].[CRMUsers] ([AccountID]);
END
GO

---[dbo].[DomainDnsRecords] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'DomainDnsRecordsIdx_DomainId' AND [object_id] = OBJECT_ID('[dbo].[DomainDnsRecords]'))
BEGIN
	CREATE INDEX DomainDnsRecordsIdx_DomainId ON [dbo].[DomainDnsRecords] ([DomainId]);
END
GO

---[dbo].[Domains] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'DomainsIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[Domains]'))
BEGIN
	CREATE INDEX DomainsIdx_PackageID ON [dbo].[Domains] ([PackageID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'DomainsIdx_ZoneItemID' AND [object_id] = OBJECT_ID('[dbo].[Domains]'))
BEGIN
	CREATE INDEX DomainsIdx_ZoneItemID ON [dbo].[Domains] ([ZoneItemID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'DomainsIdx_WebSiteID' AND [object_id] = OBJECT_ID('[dbo].[Domains]'))
BEGIN
	CREATE INDEX DomainsIdx_WebSiteID ON [dbo].[Domains] ([WebSiteID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'DomainsIdx_MailDomainID' AND [object_id] = OBJECT_ID('[dbo].[Domains]'))
BEGIN
	CREATE INDEX DomainsIdx_MailDomainID ON [dbo].[Domains] ([MailDomainID]);
END
GO

---[dbo].[EnterpriseFolders] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'EnterpriseFoldersIdx_StorageSpaceFolderId' AND [object_id] = OBJECT_ID('[dbo].[EnterpriseFolders]'))
BEGIN
	CREATE INDEX EnterpriseFoldersIdx_StorageSpaceFolderId ON [dbo].[EnterpriseFolders] ([StorageSpaceFolderId]);
END
GO

---[dbo].[EnterpriseFoldersOwaPermissions] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'EnterpriseFoldersOwaPermissionsIdx_FolderID' AND [object_id] = OBJECT_ID('[dbo].[EnterpriseFoldersOwaPermissions]'))
BEGIN
	CREATE INDEX EnterpriseFoldersOwaPermissionsIdx_FolderID ON [dbo].[EnterpriseFoldersOwaPermissions] ([FolderID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'EnterpriseFoldersOwaPermissionsIdx_AccountID' AND [object_id] = OBJECT_ID('[dbo].[EnterpriseFoldersOwaPermissions]'))
BEGIN
	CREATE INDEX EnterpriseFoldersOwaPermissionsIdx_AccountID ON [dbo].[EnterpriseFoldersOwaPermissions] ([AccountID]);
END
GO

---[dbo].[ExchangeAccountEmailAddresses] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeAccountEmailAddressesIdx_AccountID' AND [object_id] = OBJECT_ID('[dbo].[ExchangeAccountEmailAddresses]'))
BEGIN
	CREATE INDEX ExchangeAccountEmailAddressesIdx_AccountID ON [dbo].[ExchangeAccountEmailAddresses] ([AccountID]);
END
GO

---[dbo].[ExchangeAccounts] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeAccountsIdx_ItemID' AND [object_id] = OBJECT_ID('[dbo].[ExchangeAccounts]'))
BEGIN
	CREATE INDEX ExchangeAccountsIdx_ItemID ON [dbo].[ExchangeAccounts] ([ItemID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeAccountsIdx_MailboxPlanId' AND [object_id] = OBJECT_ID('[dbo].[ExchangeAccounts]'))
BEGIN
	CREATE INDEX ExchangeAccountsIdx_MailboxPlanId ON [dbo].[ExchangeAccounts] ([MailboxPlanId]);
END
GO

---[dbo].[ExchangeDeletedAccounts] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[ExchangeDisclaimers] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[ExchangeMailboxPlanRetentionPolicyTags] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[ExchangeMailboxPlans] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeMailboxPlansIdx_ItemID' AND [object_id] = OBJECT_ID('[dbo].[ExchangeMailboxPlans]'))
BEGIN
	CREATE INDEX ExchangeMailboxPlansIdx_ItemID ON [dbo].[ExchangeMailboxPlans] ([ItemID]);
END
GO

---[dbo].[ExchangeOrganizationDomains] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeOrganizationDomainsIdx_ItemID' AND [object_id] = OBJECT_ID('[dbo].[ExchangeOrganizationDomains]'))
BEGIN
	CREATE INDEX ExchangeOrganizationDomainsIdx_ItemID ON [dbo].[ExchangeOrganizationDomains] ([ItemID]);
END
GO
--- TODO: add missed FKs and indexes?

---[dbo].[ExchangeOrganizations] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[ExchangeOrganizationSettings] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeOrganizationSettingsIdx_ItemId' AND [object_id] = OBJECT_ID('[dbo].[ExchangeOrganizationSettings]'))
BEGIN
	CREATE INDEX ExchangeOrganizationSettingsIdx_ItemId ON [dbo].[ExchangeOrganizationSettings] ([ItemId]);
END
GO

---[dbo].[ExchangeOrganizationSsFolders] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeOrganizationSsFoldersIdx_ItemId' AND [object_id] = OBJECT_ID('[dbo].[ExchangeOrganizationSsFolders]'))
BEGIN
	CREATE INDEX ExchangeOrganizationSsFoldersIdx_ItemId ON [dbo].[ExchangeOrganizationSsFolders] ([ItemId]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ExchangeOrganizationSsFoldersIdx_StorageSpaceFolderId' AND [object_id] = OBJECT_ID('[dbo].[ExchangeOrganizationSsFolders]'))
BEGIN
	CREATE INDEX ExchangeOrganizationSsFoldersIdx_StorageSpaceFolderId ON [dbo].[ExchangeOrganizationSsFolders] ([StorageSpaceFolderId]);
END
GO

---[dbo].[ExchangeRetentionPolicyTags] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[GlobalDnsRecords] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'GlobalDnsRecordsIdx_ServiceID' AND [object_id] = OBJECT_ID('[dbo].[GlobalDnsRecords]'))
BEGIN
	CREATE INDEX GlobalDnsRecordsIdx_ServiceID ON [dbo].[GlobalDnsRecords] ([ServiceID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'GlobalDnsRecordsIdx_ServerID' AND [object_id] = OBJECT_ID('[dbo].[GlobalDnsRecords]'))
BEGIN
	CREATE INDEX GlobalDnsRecordsIdx_ServerID ON [dbo].[GlobalDnsRecords] ([ServerID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'GlobalDnsRecordsIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[GlobalDnsRecords]'))
BEGIN
	CREATE INDEX GlobalDnsRecordsIdx_PackageID ON [dbo].[GlobalDnsRecords] ([PackageID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'GlobalDnsRecordsIdx_IPAddressID' AND [object_id] = OBJECT_ID('[dbo].[GlobalDnsRecords]'))
BEGIN
	CREATE INDEX GlobalDnsRecordsIdx_IPAddressID ON [dbo].[GlobalDnsRecords] ([IPAddressID]);
END
GO

---[dbo].[HostingPlans] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'HostingPlansIdx_UserID' AND [object_id] = OBJECT_ID('[dbo].[HostingPlans]'))
BEGIN
	CREATE INDEX HostingPlansIdx_UserID ON [dbo].[HostingPlans] ([UserID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'HostingPlansIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[HostingPlans]'))
BEGIN
	CREATE INDEX HostingPlansIdx_PackageID ON [dbo].[HostingPlans] ([PackageID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'HostingPlansIdx_ServerID' AND [object_id] = OBJECT_ID('[dbo].[HostingPlans]'))
BEGIN
	CREATE INDEX HostingPlansIdx_ServerID ON [dbo].[HostingPlans] ([ServerID]);
END
GO

---[dbo].[IPAddresses] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'IPAddressesIdx_ServerID' AND [object_id] = OBJECT_ID('[dbo].[IPAddresses]'))
BEGIN
	CREATE INDEX IPAddressesIdx_ServerID ON [dbo].[IPAddresses] ([ServerID]);
END
GO

---[dbo].[LyncUserPlans] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'LyncUserPlansIdx_ItemID' AND [object_id] = OBJECT_ID('[dbo].[LyncUserPlans]'))
BEGIN
	CREATE INDEX LyncUserPlansIdx_ItemID ON [dbo].[LyncUserPlans] ([ItemID]);
END
GO

---[dbo].[LyncUsers] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'LyncUsersIdx_LyncUserPlanID' AND [object_id] = OBJECT_ID('[dbo].[LyncUsers]'))
BEGIN
	CREATE INDEX LyncUsersIdx_LyncUserPlanID ON [dbo].[LyncUsers] ([LyncUserPlanID]);
END
GO

---[dbo].[OCSUsers] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[PackageAddons] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageAddonsIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[PackageAddons]'))
BEGIN
	CREATE INDEX PackageAddonsIdx_PackageID ON [dbo].[PackageAddons] ([PackageID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageAddonsIdx_PlanID' AND [object_id] = OBJECT_ID('[dbo].[PackageAddons]'))
BEGIN
	CREATE INDEX PackageAddonsIdx_PlanID ON [dbo].[PackageAddons] ([PlanID]);
END
GO

---[dbo].[PackageIPAddresses] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageIPAddressesIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[PackageIPAddresses]'))
BEGIN
	CREATE INDEX PackageIPAddressesIdx_PackageID ON [dbo].[PackageIPAddresses] ([PackageID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageIPAddressesIdx_AddressID' AND [object_id] = OBJECT_ID('[dbo].[PackageIPAddresses]'))
BEGIN
	CREATE INDEX PackageIPAddressesIdx_AddressID ON [dbo].[PackageIPAddresses] ([AddressID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageIPAddressesIdx_ItemID' AND [object_id] = OBJECT_ID('[dbo].[PackageIPAddresses]'))
BEGIN
	CREATE INDEX PackageIPAddressesIdx_ItemID ON [dbo].[PackageIPAddresses] ([ItemID]);
END
GO

---[dbo].[Packages] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageIndex_ParentPackageID' AND [object_id] = OBJECT_ID('[dbo].[Packages]'))
BEGIN
	CREATE INDEX PackageIndex_ParentPackageID ON [dbo].[Packages] ([ParentPackageID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageIndex_UserID' AND [object_id] = OBJECT_ID('[dbo].[Packages]'))
BEGIN
	CREATE INDEX PackageIndex_UserID ON [dbo].[Packages] ([UserID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageIndex_ServerID' AND [object_id] = OBJECT_ID('[dbo].[Packages]'))
BEGIN
	CREATE INDEX PackageIndex_ServerID ON [dbo].[Packages] ([ServerID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageIndex_PlanID' AND [object_id] = OBJECT_ID('[dbo].[Packages]'))
BEGIN
	CREATE INDEX PackageIndex_PlanID ON [dbo].[Packages] ([PlanID]);
END
GO

---[dbo].[PackagesTreeCache] Indexes TODO: should be it as a Primary key (PK)?
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackagesTreeCacheIndex' AND [object_id] = OBJECT_ID('[dbo].[PackagesTreeCache]'))
BEGIN
	CREATE CLUSTERED INDEX PackagesTreeCacheIndex ON [dbo].[PackagesTreeCache] ([ParentPackageID], [PackageID]); --Clustered cause those columns look like must be the PK, not just FKs
END
GO

---[dbo].[PackageVLANs] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageVLANsIdx_VlanID' AND [object_id] = OBJECT_ID('[dbo].[PackageVLANs]'))
BEGIN
	CREATE INDEX PackageVLANsIdx_VlanID ON [dbo].[PackageVLANs] ([VlanID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PackageVLANsIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[PackageVLANs]'))
BEGIN
	CREATE INDEX PackageVLANsIdx_PackageID ON [dbo].[PackageVLANs] ([PackageID]);
END
GO

---[dbo].[PrivateIPAddresses] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PrivateIPAddressesIdx_ItemID' AND [object_id] = OBJECT_ID('[dbo].[PrivateIPAddresses]'))
BEGIN
	CREATE INDEX PrivateIPAddressesIdx_ItemID ON [dbo].[PrivateIPAddresses] ([ItemID]);
END
GO

---[dbo].[PrivateNetworkVLANs] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'PrivateNetworkVLANsIdx_ServerID' AND [object_id] = OBJECT_ID('[dbo].[PrivateNetworkVLANs]'))
BEGIN
	CREATE INDEX PrivateNetworkVLANsIdx_ServerID ON [dbo].[PrivateNetworkVLANs] ([ServerID]);
END
GO

---[dbo].[Providers] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ProvidersIdx_GroupID' AND [object_id] = OBJECT_ID('[dbo].[Providers]'))
BEGIN
	CREATE INDEX ProvidersIdx_GroupID ON [dbo].[Providers] ([GroupID]);
END
GO

---[dbo].[Quotas] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'QuotasIdx_GroupID' AND [object_id] = OBJECT_ID('[dbo].[Quotas]'))
BEGIN
	CREATE INDEX QuotasIdx_GroupID ON [dbo].[Quotas] ([GroupID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'QuotasIdx_ItemTypeID' AND [object_id] = OBJECT_ID('[dbo].[Quotas]'))
BEGIN
	CREATE INDEX QuotasIdx_ItemTypeID ON [dbo].[Quotas] ([ItemTypeID]);
END
GO
--- TODO: add missed FKs and indexes?

---[dbo].[RDSCertificates] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[RDSCollections] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[RDSCollectionSettings] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'RDSCollectionSettingsIdx_RDSCollectionId' AND [object_id] = OBJECT_ID('[dbo].[RDSCollectionSettings]'))
BEGIN
	CREATE INDEX RDSCollectionSettingsIdx_RDSCollectionId ON [dbo].[RDSCollectionSettings] ([RDSCollectionId]);
END
GO

---[dbo].[RDSCollectionUsers] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'RDSCollectionUsersIdx_RDSCollectionId' AND [object_id] = OBJECT_ID('[dbo].[RDSCollectionUsers]'))
BEGIN
	CREATE INDEX RDSCollectionUsersIdx_RDSCollectionId ON [dbo].[RDSCollectionUsers] ([RDSCollectionId]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'RDSCollectionUsersIdx_AccountID' AND [object_id] = OBJECT_ID('[dbo].[RDSCollectionUsers]'))
BEGIN
	CREATE INDEX RDSCollectionUsersIdx_AccountID ON [dbo].[RDSCollectionUsers] ([AccountID]);
END
GO

---[dbo].[RDSMessages] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'RDSMessagesIdx_RDSCollectionId' AND [object_id] = OBJECT_ID('[dbo].[RDSMessages]'))
BEGIN
	CREATE INDEX RDSMessagesIdx_RDSCollectionId ON [dbo].[RDSMessages] ([RDSCollectionId]);
END
GO

---[dbo].[RDSServers] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'RDSServersIdx_RDSCollectionId' AND [object_id] = OBJECT_ID('[dbo].[RDSServers]'))
BEGIN
	CREATE INDEX RDSServersIdx_RDSCollectionId ON [dbo].[RDSServers] ([RDSCollectionId]);
END
GO
--- TODO: add missed FKs and indexes?

---[dbo].[ResourceGroupDnsRecords] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ResourceGroupDnsRecordsIdx_GroupID' AND [object_id] = OBJECT_ID('[dbo].[ResourceGroupDnsRecords]'))
BEGIN
	CREATE INDEX ResourceGroupDnsRecordsIdx_GroupID ON [dbo].[ResourceGroupDnsRecords] ([GroupID]);
END
GO

---[dbo].[Schedule] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ScheduleIdx_TaskID' AND [object_id] = OBJECT_ID('[dbo].[Schedule]'))
BEGIN
	CREATE INDEX ScheduleIdx_TaskID ON [dbo].[Schedule] ([TaskID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ScheduleIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[Schedule]'))
BEGIN
	CREATE INDEX ScheduleIdx_PackageID ON [dbo].[Schedule] ([PackageID]);
END
GO

---[dbo].[ScheduleTasks] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[Servers] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServersIdx_PrimaryGroupID' AND [object_id] = OBJECT_ID('[dbo].[Servers]'))
BEGIN
	CREATE INDEX ServersIdx_PrimaryGroupID ON [dbo].[Servers] ([PrimaryGroupID]);
END
GO

---[dbo].[ServiceItems] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServiceItemsIdx_PackageID' AND [object_id] = OBJECT_ID('[dbo].[ServiceItems]'))
BEGIN
	CREATE INDEX ServiceItemsIdx_PackageID ON [dbo].[ServiceItems] ([PackageID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServiceItemsIdx_ItemTypeID' AND [object_id] = OBJECT_ID('[dbo].[ServiceItems]'))
BEGIN
	CREATE INDEX ServiceItemsIdx_ItemTypeID ON [dbo].[ServiceItems] ([ItemTypeID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServiceItemsIdx_ServiceID' AND [object_id] = OBJECT_ID('[dbo].[ServiceItems]'))
BEGIN
	CREATE INDEX ServiceItemsIdx_ServiceID ON [dbo].[ServiceItems] ([ServiceID]);
END
GO

---[dbo].[ServiceItemTypes] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServiceItemTypesIdx_GroupID' AND [object_id] = OBJECT_ID('[dbo].[ServiceItemTypes]'))
BEGIN
	CREATE INDEX ServiceItemTypesIdx_GroupID ON [dbo].[ServiceItemTypes] ([GroupID]);
END
GO

---[dbo].[Services] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServicesIdx_ServerID' AND [object_id] = OBJECT_ID('[dbo].[Services]'))
BEGIN
	CREATE INDEX ServicesIdx_ServerID ON [dbo].[Services] ([ServerID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServicesIdx_ProviderID' AND [object_id] = OBJECT_ID('[dbo].[Services]'))
BEGIN
	CREATE INDEX ServicesIdx_ProviderID ON [dbo].[Services] ([ProviderID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'ServicesIdx_ClusterID' AND [object_id] = OBJECT_ID('[dbo].[Services]'))
BEGIN
	CREATE INDEX ServicesIdx_ClusterID ON [dbo].[Services] ([ClusterID]);
END
GO

---[dbo].[SfBUserPlans] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[SfBUsers] Indexes
--- TODO: add missed FKs and indexes?

---[dbo].[SSLCertificates] Indexes
--- TODO: this table missed everything! 

---[dbo].[StorageSpaceFolders] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'StorageSpaceFoldersIdx_StorageSpaceId' AND [object_id] = OBJECT_ID('[dbo].[StorageSpaceFolders]'))
BEGIN
	CREATE INDEX StorageSpaceFoldersIdx_StorageSpaceId ON [dbo].[StorageSpaceFolders] ([StorageSpaceId]);
END
GO

---[dbo].[StorageSpaceLevelResourceGroups] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'StorageSpaceLevelResourceGroupsIdx_LevelId' AND [object_id] = OBJECT_ID('[dbo].[StorageSpaceLevelResourceGroups]'))
BEGIN
	CREATE INDEX StorageSpaceLevelResourceGroupsIdx_LevelId ON [dbo].[StorageSpaceLevelResourceGroups] ([LevelId]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'StorageSpaceLevelResourceGroupsIdx_GroupId' AND [object_id] = OBJECT_ID('[dbo].[StorageSpaceLevelResourceGroups]'))
BEGIN
	CREATE INDEX StorageSpaceLevelResourceGroupsIdx_GroupId ON [dbo].[StorageSpaceLevelResourceGroups] ([GroupId]);
END
GO

---[dbo].[StorageSpaces] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'StorageSpacesIdx_ServiceId' AND [object_id] = OBJECT_ID('[dbo].[StorageSpaces]'))
BEGIN
	CREATE INDEX StorageSpacesIdx_ServiceId ON [dbo].[StorageSpaces] ([ServiceId]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'StorageSpacesIdx_ServerId' AND [object_id] = OBJECT_ID('[dbo].[StorageSpaces]'))
BEGIN
	CREATE INDEX StorageSpacesIdx_ServerId ON [dbo].[StorageSpaces] ([ServerId]);
END
GO

---[dbo].[Users] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'UsersIdx_OwnerID' AND [object_id] = OBJECT_ID('[dbo].[Users]'))
BEGIN
	CREATE INDEX UsersIdx_OwnerID ON [dbo].[Users] ([OwnerID]);
END
GO

---[dbo].[VirtualGroups] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'VirtualGroupsIdx_ServerID' AND [object_id] = OBJECT_ID('[dbo].[VirtualGroups]'))
BEGIN
	CREATE INDEX VirtualGroupsIdx_ServerID ON [dbo].[VirtualGroups] ([ServerID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'VirtualGroupsIdx_GroupID' AND [object_id] = OBJECT_ID('[dbo].[VirtualGroups]'))
BEGIN
	CREATE INDEX VirtualGroupsIdx_GroupID ON [dbo].[VirtualGroups] ([GroupID]);
END
GO

---[dbo].[VirtualServices] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'VirtualServicesIdx_ServerID' AND [object_id] = OBJECT_ID('[dbo].[VirtualServices]'))
BEGIN
	CREATE INDEX VirtualServicesIdx_ServerID ON [dbo].[VirtualServices] ([ServerID]);
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'VirtualServicesIdx_ServiceID' AND [object_id] = OBJECT_ID('[dbo].[VirtualServices]'))
BEGIN
	CREATE INDEX VirtualServicesIdx_ServiceID ON [dbo].[VirtualServices] ([ServiceID]);
END
GO

---[dbo].[WebDavAccessTokens] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'WebDavAccessTokensIdx_AccountID' AND [object_id] = OBJECT_ID('[dbo].[WebDavAccessTokens]'))
BEGIN
	CREATE INDEX WebDavAccessTokensIdx_AccountID ON [dbo].[WebDavAccessTokens] ([AccountID]);
END
GO
--- TODO: add missed FKs and indexes?

---[dbo].[WebDavPortalUsersSettings] Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'WebDavPortalUsersSettingsIdx_AccountId' AND [object_id] = OBJECT_ID('[dbo].[WebDavPortalUsersSettings]'))
BEGIN
	CREATE INDEX WebDavPortalUsersSettingsIdx_AccountId ON [dbo].[WebDavPortalUsersSettings] ([AccountId]);
END
GO

-------------------------
-- END INDEXES SECTION --
-------------------------

-- Themes

IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[Themes]') AND type in (N'U'))
BEGIN
CREATE TABLE Themes
(
	ThemeID INT NOT NULL,
	DisplayName NVARCHAR(255),
	LTRName NVARCHAR(255),
	RTLName NVARCHAR(255),
	Enabled INT NOT NULL,
	DisplayOrder INT NOT NULL
)
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[ThemeSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE ThemeSettings
(
	ThemeID INT NOT NULL,
	SettingsName NVARCHAR(255) NOT NULL,
	PropertyName NVARCHAR(255) NOT NULL,
	PropertyValue NVARCHAR(255) NOT NULL
)
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetThemes')
DROP PROCEDURE GetThemes
GO
CREATE PROCEDURE GetThemes
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetThemeSettings')
DROP PROCEDURE GetThemeSettings
GO
CREATE PROCEDURE GetThemeSettings
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetThemeSetting')
DROP PROCEDURE GetThemeSetting
GO
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateUserThemeSetting')
DROP PROCEDURE UpdateUserThemeSetting
GO
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteUserThemeSetting')
DROP PROCEDURE DeleteUserThemeSetting
GO
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
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Themes] WHERE [ThemeID] = '1')
BEGIN
INSERT [dbo].[Themes] ([ThemeID], [DisplayName], [LTRName], [RTLName], [Enabled], [DisplayOrder]) VALUES (1, N'SolidCP v1', N'Default', N'Default', 1, 1)
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'Style', N'Light', N'light-theme')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'Style', N'Dark', N'dark-theme')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'Style', N'Semi Dark', N'semi-dark')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'Style', N'Minimal', N'minimal-theme')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#0727d7', N'headercolor1')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#23282c', N'headercolor2')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#e10a1f', N'headercolor3')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#157d4c', N'headercolor4')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#673ab7', N'headercolor5')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#795548', N'headercolor6')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#d3094e', N'headercolor7')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-header', N'#ff9800', N'headercolor8')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#6c85ec', N'sidebarcolor1')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#5b737f', N'sidebarcolor2')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#408851', N'sidebarcolor3')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#230924', N'sidebarcolor4')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#903a85', N'sidebarcolor5')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#a04846', N'sidebarcolor6')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#a65314', N'sidebarcolor7')
INSERT [dbo].[ThemeSettings] ([ThemeID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'color-Sidebar', N'#1f0e3b', N'sidebarcolor8')
END
GO


-- SimpleDNS 9.x
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1903' AND DisplayName = 'SimpleDNS Plus 9.x')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1903, 7, N'SimpleDNS', N'SimpleDNS Plus 9.x', N'SolidCP.Providers.DNS.SimpleDNS9, SolidCP.Providers.DNS.SimpleDNS90', N'SimpleDNS', NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1903' AND [PropertyName] = 'SimpleDnsUrl')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'AdminLogin', N'Admin')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'ExpireLimit', N'1209600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'RefreshInterval', N'3600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'RetryDelay', N'600')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'SimpleDnsUrl', N'http://127.0.0.1:8053')
END
GO

-- User MFA
IF NOT EXISTS(select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='Users' AND COLS.name='MfaMode')
BEGIN
ALTER TABLE [dbo].[Users] ADD
	[MfaMode] int NOT NULL DEFAULT(0),
	[PinSecret] NVARCHAR(255)
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateUserPinSecret')
DROP PROCEDURE UpdateUserPinSecret
GO
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateUserMfaMode')
DROP PROCEDURE UpdateUserMfaMode
GO
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
GO

ALTER PROCEDURE [dbo].[GetUserByUsernameInternally]
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
GO

ALTER PROCEDURE [dbo].[GetUserByIdInternally]
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
GO

ALTER PROCEDURE [dbo].[GetUserByUsername]
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
GO

ALTER PROCEDURE [dbo].[GetUserById]
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
		U.[AdditionalParams],
		U.MfaMode,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, @UserID) = 1 THEN U.PinSecret
		ELSE '' END AS PinSecret
	FROM Users AS U
	WHERE U.UserID = @UserID
	AND dbo.CanGetUserDetails(@ActorID, @UserID) = 1 -- actor user rights

	RETURN
GO

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'CC' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'VerificationCodeLetter', N'CC', N'support@HostingCompany.com')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'From' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'VerificationCodeLetter', N'From', N'support@HostingCompany.com')
END
GO

DECLARE @VerificationCodeLetterHtmlBody nvarchar(2500)

Set @VerificationCodeLetterHtmlBody = N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Verification code</title>
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
	Verification code
</div>

<p>
Hello #user.FirstName#,
</p>

<p>
to complete the sign in, enter the verification code on the device. 
</p>

<table>
    <thead>
        <tr>
            <th>Verification code</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>#verificationCode#</td>
        </tr>
    </tbody>
</table>

<p>
Best regards,<br />

</p>

</div>
</body>
</html>';

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'HtmlBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'VerificationCodeLetter', N'HtmlBody', @VerificationCodeLetterHtmlBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @VerificationCodeLetterHtmlBody WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'HtmlBody'
GO


IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'Priority' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'VerificationCodeLetter', N'Priority', N'Normal')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'Subject' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'VerificationCodeLetter', N'Subject', N'Verification code')
END
GO

DECLARE @VerificationCodeLetterTextBody nvarchar(2500)

Set @VerificationCodeLetterTextBody = N'=================================
   Verification code
=================================
<ad:if test="#user#">
Hello #user.FirstName#,
</ad:if>

to complete the sign in, enter the verification code on the device.

Verification code
#verificationCode#

Best regards,
'

IF NOT EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'TextBody' )
BEGIN
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'VerificationCodeLetter', N'TextBody', @VerificationCodeLetterTextBody)
END
ELSE
UPDATE [dbo].[UserSettings] SET [PropertyValue] = @VerificationCodeLetterTextBody WHERE [UserID] = 1 AND [SettingsName]= N'VerificationCodeLetter' AND [PropertyName]= N'TextBody'
GO
GO

-- SmarterMail 100.x
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'SmarterMail 100.x +')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(67, 4, N'SmarterMail', N'SmarterMail 100.x +', N'SolidCP.Providers.Mail.SmarterMail100, SolidCP.Providers.Mail.SmarterMail100', N'SmarterMail100x', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [EditorControl] = 'SmarterMail100x' WHERE [DisplayName] = 'SmarterMail 100.x +'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '67' AND [PropertyName] = N'AdminUsername')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (67, N'AdminPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (67, N'AdminUsername', N'admin')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (67, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail\Domains')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (67, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (67, N'ServiceUrl', N'http://localhost:9998')
END
GO


-- Server 2022
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Windows Server 2022')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1802, 1, N'Windows2022', N'Windows Server 2022', N'SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022', N'Windows2012', null)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1802')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1802, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
END
GO

-- Unix
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Unix System')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(500, 1, N'UnixSystem', N'Unix System', N'SolidCP.Providers.OS.Unix, SolidCP.Providers.OS.Unix', N'Unix',	NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '500')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (500, N'UsersHome', N'/var/www/HostingSpaces')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (500, N'LogDir', N'/var/log')
END
GO

-- HyperV2022

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HyperV2022')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1803, 33, N'HyperV2022', N'Microsoft Hyper-V 2022', N'SolidCP.Providers.Virtualization.HyperV2022, SolidCP.Providers.Virtualization.HyperV2022', N'HyperV2012R2', 1)
END
GO

-- RDS Provider 2022

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Remote Desktop Services Windows 2022')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) 
VALUES(1504, 45, N'RemoteDesktopServices2022', N'Remote Desktop Services Windows 2022', N'SolidCP.Providers.RemoteDesktopServices.Windows2019,SolidCP.Providers.RemoteDesktopServices.Windows2019', N'RDS',	1)
END
GO

/* SQL 2022 Provider */
IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'MsSQL2022')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (75, N'MsSQL2022', 10, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
ELSE
BEGIN
UPDATE [dbo].[ResourceGroups] SET [ShowGroup] = 1 WHERE [GroupName] = 'MsSQL2022'
END
GO


IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Microsoft SQL Server 2022')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1706, 75, N'MsSQL', N'Microsoft SQL Server 2022', N'SolidCP.Providers.Database.MsSqlServer2022, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (79, 75, N'MsSQL2022Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (80, 75, N'MsSQL2022User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (732, 75, 1, N'MsSQL2022.Databases', N'Databases', 2, 0, 79, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (733, 75, 2, N'MsSQL2022.Users', N'Users', 2, 0, 80, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (734, 75, 3, N'MsSQL2022.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (735, 75, 5, N'MsSQL2022.Backup', N'Database Backups', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (736, 75, 6, N'MsSQL2022.Restore', N'Database Restores', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (737, 75, 7, N'MsSQL2022.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (738, 75, 4, N'MsSQL2022.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 75 WHERE [DisplayName] = 'Microsoft SQL Server 2022'
END
GO

-- Authenticaion Settings
IF NOT EXISTS (SELECT * FROM [dbo].[SystemSettings] WHERE [SettingsName] = 'AuthenticationSettings')
BEGIN
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'AuthenticationSettings', N'MfaTokenAppDisplayName', N'SolidCP')
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'AuthenticationSettings', N'CanPeerChangeMfa', N'True')
END
GO

-- CanChangeMfa Function --
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type IN ('FN', 'IF', 'TF') AND name = 'CanChangeMfaFunc') 
DROP FUNCTION [dbo].[CanChangeMfaFunc]
GO

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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'CanChangeMfa')
DROP PROCEDURE [dbo].[CanChangeMfa]
GO

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
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = 409)
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (409, 1, 13, N'OS.NotAllowTenantDeleteDomains', N'Not allow Tenants to Delete Top Level Domains', 1, 0, NULL, NULL)
	UPDATE [dbo].[Quotas] SET [QuotaName] = N'OS.NotAllowTenantCreateDomains', [QuotaDescription] = N'Not allow Tenants to Create Top Level Domains' WHERE [QuotaID] = 410
END
GO


-- Add Platform and IsCode columns to Servers
DECLARE @NoPlatform bit, @NoIsCore bit

SET @NoPlatform = 0
SET @NoIsCore = 0

IF NOT EXISTS (
  SELECT * FROM sys.columns 
  WHERE object_id = OBJECT_ID(N'[dbo].[Servers]') 
  AND name = 'OSPlatform'
)
BEGIN
	SET @NoPlatform = 1
END

IF NOT EXISTS (
  SELECT * FROM sys.columns 
  WHERE object_id = OBJECT_ID(N'[dbo].[Servers]') 
  AND name = 'IsCore'
)
BEGIN
	SET @NoIsCore = 1
END

-- Add PasswordIsSHA256 column to Servers
DECLARE @NoPasswordIsSHA256 bit

SET @NoPasswordIsSHA256 = 0

IF NOT EXISTS (
  SELECT * FROM sys.columns 
  WHERE object_id = OBJECT_ID(N'[dbo].[Servers]') 
  AND name = 'PasswordIsSHA256'
)
BEGIN
	SET @NoPasswordIsSHA256 = 1
END

DECLARE @NoSHA256Password bit

SET @NoSHA256Password = 0

IF NOT EXISTS (
  SELECT * FROM sys.columns 
  WHERE object_id = OBJECT_ID(N'[dbo].[Servers]') 
  AND name = 'SHA256Password'
)
BEGIN
	SET @NoSHA256Password = 1
END

IF @NoPlatform = 1
BEGIN
	ALTER TABLE [dbo].[Servers] ADD [OSPlatform] INT NOT NULL DEFAULT 0
END

IF @NoIsCore = 1
BEGIN
	ALTER TABLE [dbo].[Servers] ADD [IsCore] BIT NULL
END

IF @NoPasswordIsSHA256 = 1 AND @NoSHA256Password = 1
BEGIN
	ALTER TABLE [dbo].[Servers] ADD [PasswordIsSHA256] BIT NOT NULL DEFAULT 0
END
ELSE IF @NoPasswordIsSHA256 = 1 AND @NoSHA256Password = 0
BEGIN
    EXEC sp_RENAME '[dbo].[Servers].SHA256Password', 'PasswordIsSHA256', 'COLUMN';
END

IF @NoPlatform = 1 OR @NoIsCore = 1 OR @NoPasswordIsSHA256 = 1
EXEC ('
	ALTER PROCEDURE AddServer
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
	')

IF @NoPlatform = 1 OR @NoIsCore = 1 OR @NoPasswordIsSHA256 = 1
EXEC('
	ALTER PROCEDURE GetServerInternal
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
	')
	
IF @NoPlatform = 1 OR @NoIsCore = 1 OR @NoPasswordIsSHA256 = 1
EXEC('
	ALTER PROCEDURE GetServerByName
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
	')

IF @NoPlatform = 1 OR @NoIsCore = 1 OR @NoPasswordIsSHA256 = 1
EXEC('
	ALTER PROCEDURE UpdateServer
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
	')

IF @NoPlatform = 1 OR @NoIsCore = 1 OR @NoPasswordIsSHA256 = 1
EXEC('
ALTER PROCEDURE [dbo].[GetServer]
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
')

GO

-- VsFtp
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1910')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1910, 3, N'vsftpd', N'vsftpd FTP Server 3', N'SolidCP.Providers.FTP.VsFtp3, SolidCP.Providers.FTP.VsFtp', N'vsftpd', NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1910')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1910, N'ConfigFile', N'/etc/vsftpd.conf')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'vsftpd FTP Server 3')
BEGIN
UPDATE [dbo].[Providers] SET [ProviderName] = 'vsftpd FTP Server 3' WHERE [ProviderName] = 'vsftpd FTP Server 3'
END
GO


-- Apache
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1911')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1911, 2, N'Apache', N'Apache Web Server 2.4', N'SolidCP.Providers.Web.Apache24, SolidCP.Providers.Web.Apache', N'Apache', NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1911')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1911, N'ConfigPath', N'/etc/apache2')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1911, N'ConfigFile', N'/etc/apache2/apache2.conf')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1911, N'BinPath', N'')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'Apache Web Server 2.4')
BEGIN
UPDATE [dbo].[Providers] SET [ProviderName] = 'Apache Web Server 2.4 (Experimental)' WHERE [ProviderName] = 'Apache Web Server 2.4'
END
GO

-- MariaDB 10.6

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = '1573')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1573, 50, N'MariaDB', N'MariaDB 10.6', N'SolidCP.Providers.Database.MariaDB106, SolidCP.Providers.Database.MariaDB', N'MariaDB', NULL)
END
ELSE
BEGIN
UPDATE [dbo].[Providers] SET [DisableAutoDiscovery] = NULL, GroupID = 50 WHERE [ProviderID] = '1573'
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1573')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'InstallFolder', N'%PROGRAMFILES%\MariaDB 10.6')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'InternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1573, N'RootPassword', N'')
END
GO

-- MySql 8.1
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 8.1')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(305, 90, N'MySQL', N'MySQL Server 8.1', N'SolidCP.Providers.Database.MySqlServer81, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '305')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 8.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (305, N'sslmode', N'True')
END
GO

-- MySql 8.2
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'MySQL Server 8.2')
BEGIN
INSERT [Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(306, 90, N'MySQL', N'MySQL Server 8.2', N'SolidCP.Providers.Database.MySqlServer82, SolidCP.Providers.Database.MySQL', N'MySQL', NULL)
END

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '306')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'ExternalAddress', N'localhost')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 8.0')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'InternalAddress', N'localhost,3306')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'RootLogin', N'root')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'RootPassword', N'')
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (306, N'sslmode', N'True')
END
GO

-- Increase size of ServerUrl column for encrypted urls
ALTER TABLE [dbo].[Servers] ALTER COLUMN [ServerUrl] nvarchar(255) NULL;
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddServer')
DROP PROCEDURE AddServer
GO

CREATE PROCEDURE AddServer
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'UpdateServer')
DROP PROCEDURE UpdateServer
GO

CREATE PROCEDURE UpdateServer
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'ExchangeAccountEmailAddressExists')
DROP PROCEDURE ExchangeAccountEmailAddressExists
GO
CREATE PROCEDURE [dbo].[ExchangeAccountEmailAddressExists]
(
	@EmailAddress nvarchar(300),
	@checkContacts bit,
	@Exists bit OUTPUT
)
AS
	SET @Exists = 0
	IF EXISTS(SELECT * FROM [dbo].[ExchangeAccountEmailAddresses] WHERE [EmailAddress] = @EmailAddress)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [PrimaryEmailAddress] = @EmailAddress AND ([AccountType] <> 2 OR @checkContacts = 1))
		BEGIN
			SET @Exists = 1
		END

	RETURN
GO

-- Initialize user's SSH Tunnel server connections on Login

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetUserPackagesServerUrls')
DROP PROCEDURE GetUserPackagesServerUrls
GO

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
GO

-- AddItemPrivateIPAddress bugfix added by Simon Egli, 27.6.2024

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddItemPrivateIPAddress')
DROP PROCEDURE AddItemPrivateIPAddress
GO

CREATE PROCEDURE [dbo].[AddItemPrivateIPAddress]
(
	@ActorID int,
	@ItemID int,
	@IPAddress varchar(15)
)
AS

IF EXISTS (SELECT ItemID FROM ServiceItems AS SI WHERE
	ItemID = @ItemID AND -- bugfix added by Simon Egli, 27.6.2024
	dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1)
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

-- DMZ Network
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'VPS2012.DMZNetworkEnabled')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (750, 33, 22, N'VPS2012.DMZNetworkEnabled', N'DMZ Network', 1, 0, NULL, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'VPS2012.DMZIPAddressesNumber')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (751, 33, 23, N'VPS2012.DMZIPAddressesNumber', N'Number of DMZ IP addresses per VPS', 3, 0, NULL, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'VPS2012.DMZVLANsNumber')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (752, 33, 24, N'VPS2012.DMZVLANsNumber', N'Number of DMZ Network VLANs', 2, 0, NULL, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'DmzIPAddresses')
BEGIN
CREATE TABLE [dbo].[DmzIPAddresses](
	[DmzAddressID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[IPAddress] [varchar](15) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	CONSTRAINT [FK_DmzIPAddresses_ServiceItems]
		FOREIGN KEY ([ItemID]) REFERENCES [dbo].[ServiceItems] ([ItemID])
		ON DELETE CASCADE,
 CONSTRAINT [PK_DmzIPAddresses] PRIMARY KEY CLUSTERED
(
	[DmzAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE [name] = 'DmzIPAddressesIdx_ItemID' AND [object_id] = OBJECT_ID('[dbo].[DmzIPAddresses]'))
BEGIN
	CREATE INDEX DmzIPAddressesIdx_ItemID ON [dbo].[DmzIPAddresses] ([ItemID]);
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackageDmzIPAddresses')
DROP PROCEDURE GetPackageDmzIPAddresses
GO
CREATE PROCEDURE [dbo].[GetPackageDmzIPAddresses]
	@PackageID int
AS
BEGIN

	SELECT
		DA.DmzAddressID,
		DA.IPAddress,
		DA.ItemID,
		SI.ItemName,
		DA.IsPrimary
	FROM DmzIPAddresses AS DA
	INNER JOIN ServiceItems AS SI ON DA.ItemID = SI.ItemID
	WHERE SI.PackageID = @PackageID

END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackageDmzIPAddressesPaged')
DROP PROCEDURE GetPackageDmzIPAddressesPaged
GO
CREATE PROCEDURE [dbo].[GetPackageDmzIPAddressesPaged]
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

IF @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	ELSE
		SET @condition = @condition + '
			AND (IPAddress LIKE ''' + @FilterValue + '''
			OR ItemName LIKE ''' + @FilterValue + ''')'
END

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'DA.IPAddress ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(DA.DmzAddressID)
FROM dbo.DmzIPAddresses AS DA
INNER JOIN dbo.ServiceItems AS SI ON DA.ItemID = SI.ItemID
WHERE ' + @condition + '

DECLARE @Addresses AS TABLE
(
	DmzAddressID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		DA.DmzAddressID
	FROM dbo.DmzIPAddresses AS DA
	INNER JOIN dbo.ServiceItems AS SI ON DA.ItemID = SI.ItemID
	WHERE ' + @condition + '
)

INSERT INTO @Addresses
SELECT DmzAddressID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	DA.DmzAddressID,
	DA.IPAddress,
	DA.ItemID,
	SI.ItemName,
	DA.IsPrimary
FROM @Addresses AS TA
INNER JOIN dbo.DmzIPAddresses AS DA ON TA.DmzAddressID = DA.DmzAddressID
INNER JOIN dbo.ServiceItems AS SI ON DA.ItemID = SI.ItemID
'

print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int',
@PackageID, @StartRow, @MaximumRows

END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'AddItemDmzIPAddress')
DROP PROCEDURE AddItemDmzIPAddress
GO
CREATE PROCEDURE [dbo].[AddItemDmzIPAddress]
(
	@ActorID int,
	@ItemID int,
	@IPAddress varchar(15)
)
AS
BEGIN

	IF EXISTS (SELECT ItemID FROM ServiceItems AS SI WHERE ItemID = @ItemID AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1)
	BEGIN

		INSERT INTO DmzIPAddresses
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
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'SetItemDmzPrimaryIPAddress')
DROP PROCEDURE SetItemDmzPrimaryIPAddress
GO
CREATE PROCEDURE [dbo].[SetItemDmzPrimaryIPAddress]
(
	@ActorID int,
	@ItemID int,
	@DmzAddressID int
)
AS
BEGIN
	UPDATE DmzIPAddresses
	SET IsPrimary = CASE DIP.DmzAddressID WHEN @DmzAddressID THEN 1 ELSE 0 END
	FROM DmzIPAddresses AS DIP
	INNER JOIN ServiceItems AS SI ON DIP.ItemID = SI.ItemID
	WHERE DIP.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteItemDmzIPAddress')
DROP PROCEDURE DeleteItemDmzIPAddress
GO
CREATE PROCEDURE DeleteItemDmzIPAddress
(
	@ActorID int,
	@ItemID int,
	@DmzAddressID int
)
AS
BEGIN
	DELETE FROM DmzIPAddresses
	FROM DmzIPAddresses AS DIP
	INNER JOIN ServiceItems AS SI ON DIP.ItemID = SI.ItemID
	WHERE DIP.DmzAddressID = @DmzAddressID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetItemDmzIPAddresses')
DROP PROCEDURE GetItemDmzIPAddresses
GO
CREATE PROCEDURE [dbo].[GetItemDmzIPAddresses]
(
	@ActorID int,
	@ItemID int
)
AS
BEGIN
SELECT
	DIP.DmzAddressID AS AddressID,
	DIP.IPAddress,
	DIP.IsPrimary
FROM DmzIPAddresses AS DIP
INNER JOIN ServiceItems AS SI ON DIP.ItemID = SI.ItemID
WHERE DIP.ItemID = @ItemID
AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
ORDER BY DIP.IsPrimary DESC
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteItemDmzIPAddresses')
DROP PROCEDURE DeleteItemDmzIPAddresses
GO
CREATE PROCEDURE DeleteItemDmzIPAddresses
(
	@ActorID int,
	@ItemID int
)
AS
BEGIN
	DELETE FROM DmzIPAddresses
	FROM DmzIPAddresses AS DIP
	INNER JOIN ServiceItems AS SI ON DIP.ItemID = SI.ItemID
	WHERE DIP.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackageDmzNetworkVLANs')
BEGIN
DROP PROCEDURE GetPackageDmzNetworkVLANs
END
GO

CREATE PROCEDURE [dbo].[GetPackageDmzNetworkVLANs]
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
AND PA.IsDmz = 1
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
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetPackageServiceID')
DROP PROCEDURE GetPackageServiceID
GO
CREATE PROCEDURE GetPackageServiceID
(
	@ActorID int,
	@PackageID int,
	@GroupName nvarchar(100),
	@UpdatePackage bit,
	@ServiceID int OUTPUT
)
AS
BEGIN

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SET @ServiceID = 0

-- optimized run when we don't need any changes
IF @UpdatePackage = 0
BEGIN
SELECT
	@ServiceID = PS.ServiceID
FROM PackageServices AS PS
INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
INNER JOIN ResourceGroups AS RG ON RG.GroupID = P.GroupID
WHERE PS.PackageID = @PackageID AND RG.GroupName = @GroupName
RETURN
END

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

END
GO


-- Fix ordering of GetHostingPlanQuotas

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetHostingPlanQuotas')
BEGIN
DROP PROCEDURE GetHostingPlanQuotas
END
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
	--dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID) AS ParentEnabled,
	CASE
		WHEN RG.GroupName = 'Service Levels' THEN dbo.GetPackageServiceLevelResource(@PackageID, RG.GroupID, @ServerID)
		ELSE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID)
	END AS ParentEnabled,
	ISNULL(HPR.CalculateDiskSpace, 1) AS CalculateDiskSpace,
	ISNULL(HPR.CalculateBandwidth, 1) AS CalculateBandwidth
FROM ResourceGroups AS RG 
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
WHERE (RG.ShowGroup = 1)
ORDER BY RG.GroupOrder, RG.GroupName

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


-- Support for EntityFramework
-- Remaining Migrations MySql9AndMaraiDB11, AddMariaDB11, Bugfix_for_MySQL_8_x, BugfixMySQL8TruncateQuota,
-- & FixUsersHomeForUnix

IF OBJECT_ID(N'[TempIds]') IS NULL
BEGIN
    CREATE TABLE [TempIds] (
        [Key] int NOT NULL IDENTITY,
        [Created] datetime2 NOT NULL,
        [Scope] uniqueidentifier NOT NULL,
        [Level] int NOT NULL,
        [Id] int NOT NULL,
        [Date] datetime2 NOT NULL,
        CONSTRAINT [PK_TempIds] PRIMARY KEY ([Key])
    );

	CREATE INDEX [IX_TempIds_Created_Scope_Level] ON [TempIds] ([Created], [Scope], [Level]);
END;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241012060936_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ThemeSettingID', N'PropertyName', N'PropertyValue', N'SettingsName', N'ThemeID') AND [object_id] = OBJECT_ID(N'[ThemeSettings]'))
        SET IDENTITY_INSERT [ThemeSettings] ON;

	DELETE FROM ThemeSettings;

    EXEC(N'INSERT INTO [ThemeSettings] ([ThemeSettingID], [PropertyName], [PropertyValue], [SettingsName], [ThemeID])
    VALUES (1, N''Light'', N''light-theme'', N''Style'', 1),
    (2, N''Dark'', N''dark-theme'', N''Style'', 1),
    (3, N''Semi Dark'', N''semi-dark'', N''Style'', 1),
    (4, N''Minimal'', N''minimal-theme'', N''Style'', 1),
    (5, N''#0727d7'', N''headercolor1'', N''color-header'', 1),
    (6, N''#23282c'', N''headercolor2'', N''color-header'', 1),
    (7, N''#e10a1f'', N''headercolor3'', N''color-header'', 1),
    (8, N''#157d4c'', N''headercolor4'', N''color-header'', 1),
    (9, N''#673ab7'', N''headercolor5'', N''color-header'', 1),
    (10, N''#795548'', N''headercolor6'', N''color-header'', 1),
    (11, N''#d3094e'', N''headercolor7'', N''color-header'', 1),
    (12, N''#ff9800'', N''headercolor8'', N''color-header'', 1),
    (13, N''#6c85ec'', N''sidebarcolor1'', N''color-Sidebar'', 1),
    (14, N''#5b737f'', N''sidebarcolor2'', N''color-Sidebar'', 1),
    (15, N''#408851'', N''sidebarcolor3'', N''color-Sidebar'', 1),
    (16, N''#230924'', N''sidebarcolor4'', N''color-Sidebar'', 1),
    (17, N''#903a85'', N''sidebarcolor5'', N''color-Sidebar'', 1),
    (18, N''#a04846'', N''sidebarcolor6'', N''color-Sidebar'', 1),
    (19, N''#a65314'', N''sidebarcolor7'', N''color-Sidebar'', 1),
    (20, N''#1f0e3b'', N''sidebarcolor8'', N''color-Sidebar'', 1)');

    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ThemeSettingID', N'PropertyName', N'PropertyValue', N'SettingsName', N'ThemeID') AND [object_id] = OBJECT_ID(N'[ThemeSettings]'))
        SET IDENTITY_INSERT [ThemeSettings] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241012060936_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES
		(N'20241012060936_InitialCreate', N'8.0.10');
END;
GO

COMMIT;
GO

-- End of Migrations

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'DeleteServiceItem')
DROP PROCEDURE DeleteServiceItem
GO

CREATE PROCEDURE [dbo].[DeleteServiceItem]
(
	@ActorID int,
	@ItemID int
)
AS

SET QUOTED_IDENTIFIER ON

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


-- Changes from Master branch 07.12.2024

-- Server 2025
IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Windows Server 2025')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1804, 1, N'Windows2025', N'Windows Server 2025', N'SolidCP.Providers.OS.Windows2025, SolidCP.Providers.OS.Windows2025', N'Windows2012', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '1804')
BEGIN
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1804, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
END
GO

-- HyperV2025

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = 'HyperV2025')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1805, 33, N'HyperV2025', N'Microsoft Hyper-V 2025', N'SolidCP.Providers.Virtualization.HyperV2025, SolidCP.Providers.Virtualization.HyperV2025', N'HyperV2012R2', 1)
END
GO

-- RDS Provider 2025

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Remote Desktop Services Windows 2025')
BEGIN
INSERT [dbo].[Providers] ([ProviderId], [GroupId], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES(1505, 45, N'RemoteDesktopServices2025', N'Remote Desktop Services Windows 2025', N'SolidCP.Providers.RemoteDesktopServices.Windows2025,SolidCP.Providers.RemoteDesktopServices.Windows2019', N'RDS',	1)
END
GO

-- DNSEditor TTL
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = 'DNS.EditTTL')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota], [PerOrganization]) VALUES (753, 7, 2, N'DNS.EditTTL', N'Allow editing TTL in DNS Editor', 1, 0, NULL, NULL, NULL)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [PropertyName] = 'RecordDefaultTTL')
BEGIN
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (55, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1703, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'RecordDefaultTTL', N'86400')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'RecordDefaultTTL', N'86400')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [PropertyName] = 'RecordMinimumTTL')
BEGIN
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (55, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (410, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1703, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1901, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1902, N'RecordMinimumTTL', N'3600')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1903, N'RecordMinimumTTL', N'3600')
END
GO

--SmarterMail100 Support for new options
IF NOT EXISTS (SELECT * FROM [dbo].[ServiceDefaultProperties] WHERE [ProviderID] = '67' AND [PropertyName] = N'defaultdomainhostname')
BEGIN
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (65, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (66, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (67, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
END
GO

-- Add defaultdomainhostname property to existing SM providers
IF NOT EXISTS (Select * from [ServiceProperties] INNER JOIN Services ON ServiceProperties.ServiceID=Services.ServiceID Where Services.ProviderID IN (11, 14, 29, 60, 64, 65, 65, 66, 67) AND ServiceProperties.PropertyName = N'defaultdomainhostname')
BEGIN
DECLARE service_cursor CURSOR FOR SELECT ServiceId FROM Services WHERE ProviderID IN (11, 14, 29, 60, 64, 65, 65, 66, 67)
DECLARE @ServiceID INT
OPEN service_cursor
FETCH NEXT FROM service_cursor INTO @ServiceID
WHILE @@FETCH_STATUS = 0
BEGIN
	BEGIN
		INSERT [dbo].[ServiceProperties] ([ServiceID], [PropertyName], [PropertyValue]) VALUES (@ServiceID, N'defaultdomainhostname', N'mail.[DOMAIN_NAME]')
	END
	
	FETCH NEXT FROM service_cursor INTO @ServiceID
END

CLOSE service_cursor
DEALLOCATE service_cursor
END
GO

-- Add RecordDefaultTTL and RecordMinimumTTL property to existing DNS providers
IF NOT EXISTS (Select * from [ServiceProperties] INNER JOIN Services ON ServiceProperties.ServiceID=Services.ServiceID Where Services.ProviderID IN (7, 9, 24, 28, 55, 56, 410, 1703, 1901, 1902, 1903) AND ServiceProperties.PropertyName = N'RecordMinimumTTL')
BEGIN
DECLARE service_cursor CURSOR FOR SELECT ServiceId FROM Services WHERE ProviderID IN (7, 9, 24, 28, 55, 56, 410, 1703, 1901, 1902, 1903)
DECLARE @ServiceID INT
OPEN service_cursor
FETCH NEXT FROM service_cursor INTO @ServiceID
WHILE @@FETCH_STATUS = 0
BEGIN
	BEGIN
		INSERT [dbo].[ServiceProperties] ([ServiceID], [PropertyName], [PropertyValue]) VALUES (@ServiceID, N'RecordMinimumTTL', N'3600')
		INSERT [dbo].[ServiceProperties] ([ServiceID], [PropertyName], [PropertyValue]) VALUES (@ServiceID, N'RecordDefaultTTL', N'86400')
	END
	
	FETCH NEXT FROM service_cursor INTO @ServiceID
END

CLOSE service_cursor
DEALLOCATE service_cursor
END
GO

-- Fix Provider 2025 types

UPDATE [Providers] SET [ProviderType] = 'SolidCP.Providers.RemoteDesktopServices.Windows2022,SolidCP.Providers.RemoteDesktopServices.Windows2022' WHERE [ProviderID] = '1504'
UPDATE [Providers] SET [ProviderType] = 'SolidCP.Providers.RemoteDesktopServices.Windows2025,SolidCP.Providers.RemoteDesktopServices.Windows2025' WHERE [ProviderID] = '1505'
GO

-- Add SqlServer 2025

IF NOT EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = 'MsSQL2025')
BEGIN
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController], [ShowGroup]) VALUES (76, N'MsSQL2025', 10, N'SolidCP.EnterpriseServer.DatabaseServerController', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Providers] WHERE [DisplayName] = 'Microsoft SQL Server 2025')
BEGIN
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1707, 76, N'MsSQL', N'Microsoft SQL Server 2025', N'SolidCP.Providers.Database.MsSqlServer2025, SolidCP.Providers.Database.SqlServer', N'MSSQL', NULL)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (205, 76, N'MsSQL2022Database', N'SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (206, 75, N'MsSQL2022User', N'SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (760, 76, 1, N'MsSQL2025.Databases', N'Databases', 2, 0, 79, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (761, 76, 2, N'MsSQL2025.Users', N'Users', 2, 0, 80, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (762, 76, 3, N'MsSQL2025.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (763, 76, 5, N'MsSQL2025.Backup', N'Database Backups', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (764, 76, 6, N'MsSQL2025.Restore', N'Database Restores', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (765, 75, 7, N'MsSQL2025.Truncate', N'Database Truncate', 1, 0, NULL, NULL)
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (766, 75, 4, N'MsSQL2025.MaxLogSize', N'Max Log Size', 3, 0, NULL, NULL)
END
GO
-- This is for SQL updates after 1.2.1
--
-- Please ensure additions are added to the BOTTOM of this file
--
--

USE [${install.database}]
GO
-- update database version
DECLARE @build_version nvarchar(10), @build_date datetime
SET @build_version = N'${release.version}'
SET @build_date = '${release.date}T00:00:00' -- ISO 8601 Format (YYYY-MM-DDTHH:MM:SS)

IF NOT EXISTS (SELECT * FROM [dbo].[Versions] WHERE [DatabaseVersion] = @build_version)
BEGIN
	INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
END
GO


-- RDS Adding seperate controllers

-- # Adding Column for controller
IF NOT EXISTS (select 1 from sys.columns COLS INNER JOIN sys.objects OBJS ON OBJS.object_id=COLS.object_id and OBJS.type='U' AND OBJS.name='RDSServers' AND COLS.name='Controller')
BEGIN
	ALTER TABLE [dbo].[RDSServers] ADD [Controller] [int] NULL
END
GO

-- # Filling the Controller column on the RDS TABLE
IF EXISTS (SELECT * FROM RDSServers WHERE Controller IS NULL)
BEGIN
	DECLARE @SystemController nvarchar(max)
	SET @SystemController = (SELECT PropertyValue FROM [SystemSettings] Where SettingsName = 'RdsSettings' AND PropertyName = 'RdsMainController');
	IF(@SystemController is not NULL)
		BEGIN
			UPDATE RDSServers Set Controller = @SystemController WHERE Controller IS NULL;
		END
	ELSE
		BEGIN
			RAISERROR(N'Please set the global RDS Controller and rerun the script', 16, 1);
		END
END
GO

-- # Editing Stored PROCEDURE

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[AddRDSServer]
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
ALTER PROCEDURE [dbo].[GetRDSServersPaged]
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
WHERE 
	((((@ItemID is Null AND S.ItemID is null ) or (@IgnoreItemId = 1 ))
		or (@ItemID is not Null AND S.ItemID = @ItemID ))
	and
	(((@RdsCollectionId is Null AND S.RDSCollectionId is null) or @IgnoreRdsCollectionId = 1)
		or (@RdsCollectionId is not Null AND S.RDSCollectionId = @RdsCollectionId)))'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

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
	SE.ServiceName as ControllerName
FROM @RDSServer AS S
INNER JOIN RDSServers AS ST ON S.RDSServerId = ST.ID
LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = ST.ItemId
LEFT OUTER JOIN  Services AS SE ON SE.ServiceID = ST.Controller
WHERE S.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int,  @FilterValue nvarchar(50),  @ItemID int, @RdsCollectionId int, @IgnoreItemId bit, @IgnoreRdsCollectionId bit, @Controller int, @ControllerName nvarchar(50)',
@StartRow, @MaximumRows,  @FilterValue,  @ItemID, @RdsCollectionId, @IgnoreItemId , @IgnoreRdsCollectionId, @Controller, @ControllerName

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

IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = N'452')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID], [HideQuota]) VALUES (452, 45, 3, N'RDS.DisableUserAddServer', N'Disable user from adding server', 1, 0, NULL, NULL)
END
GO
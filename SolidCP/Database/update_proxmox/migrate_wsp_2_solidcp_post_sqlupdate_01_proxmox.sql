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

GO

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























IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE type = 'P' AND name = 'GetSearchTableByColumns')
DROP PROCEDURE GetSearchTableByColumns
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
		(D.IsInstantAlias = 0 AND D.IsDomainPointer = 0)
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
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 562 -- External IP addresses of VPS2012
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
			WHERE pt.ParentPackageID = @PackageID AND ea.AccountType in (1, 5, 6, 10))
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






-- VLAN tag in IpAddresses
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.IPAddresses ADD
	VLAN int NULL
GO
ALTER TABLE dbo.IPAddresses SET (LOCK_ESCALATION = TABLE)
GO
COMMIT













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
    IP.DefaultGateway,
    IP.VLAN
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
   IP.DefaultGateway,
   IP.VLAN
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











SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetIPAddressesPaged] 
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



using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Data.Migrations.SqlServer
{
	public partial class InitialCreate
	{
		partial void StoredProceduresUp(MigrationBuilder migrationBuilder)
		{
			StoredProceduresDown(migrationBuilder);

			if (migrationBuilder.IsSqlServer()) migrationBuilder.Sql(@"
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[CalculatePackageBandwidth]
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

CREATE FUNCTION [dbo].[CalculatePackageDiskspace]
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
SET QUOTED_IDENTIFIER ON
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
							WHERE PT.ParentPackageID = @PackageID)
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

CREATE FUNCTION [dbo].[CheckExceedingQuota]
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

CREATE FUNCTION [dbo].[GetFullIPAddress]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetPackageAllocatedQuota]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetPackageExceedingQuotas]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetPackageServiceLevelResource]
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
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[PackageParents]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[SplitString] (@stringToSplit VARCHAR(MAX), @separator CHAR)
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddComment]
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
SET QUOTED_IDENTIFIER ON
GO
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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

CREATE PROCEDURE [dbo].[AddExchangeAccountEmailAddress]
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
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddExchangeDisclaimer] 
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

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

CREATE PROCEDURE [dbo].[AddExchangeOrganizationDomain]
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddHostingPlan]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[AddLevelResourceGroups]
(
	@LevelId INT,
	@GroupId INT
)
AS
	INSERT INTO [dbo].[StorageSpaceLevelResourceGroups] (LevelId, GroupId)
	VALUES (@LevelId, @GroupId)
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[AddOrganizationStoragSpacesFolder]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddPackageAddon]
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
SET QUOTED_IDENTIFIER ON
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
SET QUOTED_IDENTIFIER OFF
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
INSERT INTO RDSMessages
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddSchedule]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AllocatePackageIPAddresses]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ChangeExchangeAcceptedDomainType]
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
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET Password = @Password, OneTimePasswordState = 0
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
SET QUOTED_IDENTIFIER ON
GO

-- fix check domain used by HostedOrganization

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

CREATE PROCEDURE [dbo].[CreateStorageSpaceFolder]
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

	IF (@ParentPackageID = 1) -- ""System"" space
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
SET QUOTED_IDENTIFIER OFF
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteAdditionalGroup]
(
	@GroupID INT
)
AS

DELETE FROM AdditionalGroups
WHERE ID = @GroupID
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteAllLogRecords]
AS

DELETE FROM Log

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteAuditLogRecords]
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

CREATE PROCEDURE [dbo].[DeleteAuditLogRecordsComplete]
AS

TRUNCATE TABLE AuditLog

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteBackgroundTaskParams]
(
	@TaskID INT
)
AS

DELETE FROM BackgroundTaskParameters
WHERE TaskID = @TaskID
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteComment]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteDomainDnsRecord]
(
	@Id  INT
)
AS
DELETE FROM DomainDnsRecords
WHERE Id = @Id
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteExchangeAccount]
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

CREATE PROCEDURE [dbo].[DeleteExchangeAccountEmailAddress]
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
CREATE PROCEDURE [dbo].[DeleteExchangeDisclaimer]
(
	@ExchangeDisclaimerId int
)
AS

DELETE FROM ExchangeDisclaimers
WHERE ExchangeDisclaimerId = @ExchangeDisclaimerId

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
SET QUOTED_IDENTIFIER ON
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

CREATE PROCEDURE [dbo].[DeleteExchangeOrganizationDomain]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteExpiredAccessTokenTokens]
AS
DELETE FROM AccessTokens
WHERE ExpirationDate < getdate()
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteExpiredWebDavAccessTokens]
AS
DELETE FROM WebDavAccessTokens
WHERE ExpirationDate < getdate()
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteHostingPlan]
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

CREATE PROCEDURE [dbo].[DeleteItemPrivateIPAddress]
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

CREATE PROCEDURE [dbo].[DeleteItemPrivateIPAddresses]
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
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[DeleteLevelResourceGroups]
(
	@LevelId INT
)
AS
	DELETE 
	FROM [dbo].[StorageSpaceLevelResourceGroups]
	WHERE LevelId = @LevelId
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

CREATE PROCEDURE [dbo].[DeleteOrganizationDeletedUser]
(
	@ID int
)
AS
DELETE FROM	ExchangeDeletedAccounts WHERE AccountID = @ID
RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[DeleteOrganizationStoragSpacesFolder]
(
	@Id INT
)
AS
	DELETE
	FROM [ExchangeOrganizationSsFolders]
	WHERE StorageSpaceFolderId = @Id
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

CREATE PROCEDURE [dbo].[DeletePackageAddon]
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
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRDSCollectionSettings]
(
	@Id  int
)
AS

DELETE FROM DeleteRDSCollectionSettings
WHERE Id = @Id
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRDSServer]
(
	@Id  int
)
AS
DELETE FROM RDSServers
WHERE Id = @Id
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteRDSServerSettings]
(
	@ServerId int
)
AS
	DELETE FROM RDSServerSettings WHERE RDSServerId = @ServerId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteSchedule]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
	<service id=""16"" />
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

CREATE PROCEDURE [dbo].[ExchangeOrganizationDomainExists]
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

CREATE PROCEDURE [dbo].[ExchangeOrganizationExists]
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
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

CREATE PROCEDURE [dbo].[GetAuditLogSources]
AS

SELECT SourceName FROM AuditLogSources

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAuditLogTasks]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

SET ANSI_NULLS ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetComments]
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
SET QUOTED_IDENTIFIER ON
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDomainAllDnsRecords]
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
CREATE PROCEDURE [dbo].[GetDomainDnsRecords]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetEnterpriseFolders]
(
	@ItemID INT
)
AS

SELECT DISTINCT LocationDrive, HomeFolder, Domain FROM EnterpriseFolders
WHERE ItemID = @ItemID
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Password column removed
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Password column removed
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetExchangeAccountDisclaimerId] 
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
RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetExchangeAccountEmailAddresses]
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
	(E.AccountType = @AccountType OR @AccountType = 0)
ORDER BY DisplayName
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetExchangeDisclaimer] 
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
RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetExchangeDisclaimers]
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetExchangeMailboxPlans]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- Exchange2013 Shared and resource mailboxes Organization statistics

CREATE PROCEDURE [dbo].[GetExchangeOrganizationStatistics] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

CREATE PROCEDURE [dbo].[GetHostingPlan]
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
SET QUOTED_IDENTIFIER ON
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

CREATE PROCEDURE [dbo].[GetLevelResourceGroups]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetNestedPackagesSummary]
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

CREATE PROCEDURE [dbo].[GetNextSchedule]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetOrganizationRdsCollectionsCount]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetOrganizationRdsServersCount]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetOrganizationRdsUsersCount]
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
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 8 OR AccountType = 9)  AND ItemID = @ItemID) AS CreatedGroups,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 11  AND ItemID = @ItemID) AS DeletedUsers
RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetOrganizationStoragSpaceFolders]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetOrganizationStoragSpacesFolderByType]
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
  	P.StatusIDchangeDate,
	P.OverrideQuotas,
	P.DefaultTopPackage
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

CREATE PROCEDURE [dbo].[GetPackageAddons]
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

CREATE PROCEDURE [dbo].[GetPackageBandwidth]
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

CREATE PROCEDURE [dbo].[GetPackageBandwidthUpdate]
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

CREATE PROCEDURE [dbo].[GetPackageDiskspace]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetPackageIPAddresses]
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPackagePackages]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetPackageQuotas]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPackageQuotasForEdit]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPackagesBandwidthPaged]
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

CREATE PROCEDURE [dbo].[GetPackagesDiskspacePaged]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPackageSettings]
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
SET QUOTED_IDENTIFIER ON
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

GOSSL
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
	@websiteid = [SiteID] AND [Installed] = 0 AND [IsRenewal] = 0 -- bugfix Simon Egli, 27.6.2024

RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

CREATE PROCEDURE [dbo].[GetProviderServiceQuota]
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

CREATE PROCEDURE [dbo].[GetQuotas]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRDSControllerServiceIDbyFQDN]
(
	@RdsfqdnName NVARCHAR(255),
	@Controller int OUTPUT
)
AS

SELECT @Controller = Controller
	FROM RDSServers
	WHERE FqdName = @RdsfqdnName

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetRDSMessages]
(
	@RDSCollectionId INT
)
AS
SELECT Id, RDSCollectionId, MessageText, UserName, [Date] FROM [dbo].[RDSMessages] WHERE RDSCollectionId = @RDSCollectionId
RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	RC.Name AS ""CollectionName""
	FROM RDSServers AS RS
	LEFT OUTER JOIN  ServiceItems AS SI ON SI.ItemId = RS.ItemId
	LEFT OUTER JOIN  RDSCollections AS RC ON RC.ID = RdsCollectionId
	WHERE RS.Id = @Id

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetRDSServerSettings]
(
	@ServerId int,
	@SettingsName nvarchar(50)
)
AS
	SELECT RDSServerId, PropertyName, PropertyValue, ApplyUsers, ApplyAdministrators
	FROM RDSServerSettings
	WHERE RDSServerId = @ServerId AND SettingsName = @SettingsName			
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetResourceGroups]
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

CREATE PROCEDURE [dbo].[GetSchedule]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetScheduleInternal]
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

CREATE PROCEDURE [dbo].[GetSchedules]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSchedulesPaged]
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
	ISNULL((SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = ''SCHEDULER'' ORDER BY StartDate DESC), 0) AS LastResult,

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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetScheduleTask]
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
	AND @RoleID <= RoleID -- was >= but this seems like a bug, since lower RoleID is more privileged, and in GetScheduleTasks it is also <=.
RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScheduleTaskEmailTemplate]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetScheduleTasks]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
SET QUOTED_IDENTIFIER ON
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

SET ANSI_NULLS ON
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
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetStorageSpaceById]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetStorageSpaceByServiceAndPath] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetStorageSpaceFolderById]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetStorageSpaceFoldersByStorageSpaceId]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetStorageSpaceLevelById] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetStorageSpacesByLevelId] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetStorageSpacesByResourceGroupName] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetSupportServiceLevels]
AS
SELECT *
FROM SupportServiceLevels
RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetThreadBackgroundTasks]
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

IF (@ParentPackageID = 1 OR @PoolID = 4 /* management network */) -- ""System"" space
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserAvailableHostingAddons]
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

CREATE PROCEDURE [dbo].[GetUserAvailableHostingPlans]
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
SET QUOTED_IDENTIFIER ON
GO

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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		U.[AdditionalParams],
		U.OneTimePasswordState,
		U.MfaMode,
		U.PinSecret
	FROM Users AS U
	WHERE U.UserID = @UserID

	RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		U.[AdditionalParams],
		U.MfaMode,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, UserID) = 1 THEN U.PinSecret
		ELSE '' END AS PinSecret
	FROM Users AS U
	WHERE U.Username = @Username
	AND dbo.CanGetUserDetails(@ActorID, UserID) = 1 -- actor user rights

	RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		U.[AdditionalParams],
		U.OneTimePasswordState,
		U.MfaMode,
		U.PinSecret

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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		EXCEPTION ""The requested service is not available for the user. The resource of the requested type {type} should be assigned to him through hosting plan or add-on""
		1.1 If the number of returned reources is greater than 1
			EXCEPTION ""User has several resources assigned of the requested type""

	2. If the requested resource has 0 services
		EXCEPTION ""The resource {name} of type {type} should contain atleast one service
	3. If the requested resource has one service
		remember the ID of this single service
	4. If the requested resource has several services DO distribution:

		4.1. If the resource is NOT BOUNDED or is PRIMARY DISTRIBUTION RESOURCE
			if PRIMARY DISTRIBUTION RESOURCE and exists in UserServices
				return serviceId from UserServices table

			remember any service from that resource according to distribution type (""BALANCED"" or ""RANDOM"") - get the number of ServiceItems for each service

		4.2. If the resource is BOUNDED to primary distribution resource
			- If the primary distribution resource is NULL
			EXCEPTION ""Requested resource marked as bound to primary distribution resource, but there is no any resources in hosting plan marked as primary""

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
CREATE PROCEDURE [dbo].[GetUserServiceID]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertCRMUser] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[InsertStorageSpace]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[InsertStorageSpaceLevel]
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RemoveStorageSpace]
(
	@ID INT
)
AS
	DELETE FROM StorageSpaces WHERE ID = @ID
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[RemoveStorageSpaceFolder]
(
	@ID INT
)
AS
DELETE
FROM StorageSpaceFolders
WHERE ID=@ID
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RemoveStorageSpaceLevel]
(
	@ID INT
)
AS
	DELETE FROM StorageSpaceLevels WHERE ID = @ID
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE [dbo].[SetExchangeAccountDisclaimerId] 
(
	@AccountID int,
	@ExchangeDisclaimerId int
)
AS
UPDATE ExchangeAccounts SET
	ExchangeDisclaimerId = @ExchangeDisclaimerId
WHERE AccountID = @AccountID

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SetExchangeAccountMailboxplan] 
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
	<property name="""" value=""""/>
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
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateDomainCreationDate]
(
	@DomainId INT,
	@Date DateTime
)
AS
UPDATE [dbo].[Domains] SET [CreationDate] = @Date WHERE [DomainID] = @DomainId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateDomainDates]
(
	@DomainId INT,
	@DomainCreationDate DateTime,
	@DomainExpirationDate DateTime,
	@DomainLastUpdateDate DateTime 
)
AS
UPDATE [dbo].[Domains] SET [CreationDate] = @DomainCreationDate, [ExpirationDate] = @DomainExpirationDate, [LastUpdateDate] = @DomainLastUpdateDate WHERE [DomainID] = @DomainId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateDomainExpirationDate]
(
	@DomainId INT,
	@Date DateTime
)
AS
UPDATE [dbo].[Domains] SET [ExpirationDate] = @Date WHERE [DomainID] = @DomainId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateDomainLastUpdateDate]
(
	@DomainId INT,
	@Date DateTime
)
AS
UPDATE [dbo].[Domains] SET [LastUpdateDate] = @Date WHERE [DomainID] = @DomainId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Password column removed
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
 CREATE PROCEDURE [dbo].[UpdateExchangeDisclaimer] 
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateHostingPlan]
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

CREATE PROCEDURE [dbo].[UpdateHostingPlanQuotas]
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
		<group id=""16"" enabled=""1"" calculateDiskSpace=""1"" calculateBandwidth=""1""/>
	</groups>
	<quotas>
		<quota id=""2"" value=""2""/>
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

RETURN
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdatePackageAddon]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdatePackageBandwidthUpdate]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdatePackageName]
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

CREATE PROCEDURE [dbo].[UpdatePackageQuotas]
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
		<group id=""16"" enabled=""1"" calculateDiskSpace=""1"" calculateBandwidth=""1""/>
	</groups>
	<quotas>
		<quota id=""2"" value=""2""/>
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

CREATE PROCEDURE [dbo].[UpdatePackageSettings]
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
SET QUOTED_IDENTIFIER OFF
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[UpdateRDSServerSettings]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateSchedule]
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

SET ANSI_NULLS ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

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
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[UpdateStorageSpace]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[UpdateStorageSpaceFolder]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[UpdateStorageSpaceLevel]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateVirtualGroups]
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:

<groups>
	<group id=""16"" distributionType=""1"" bindDistributionToPrimary=""1""/>
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateWhoisDomainInfo]
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
			");
		}

		partial void StoredProceduresDown(MigrationBuilder migrationBuilder)
		{
			if (migrationBuilder.IsSqlServer()) migrationBuilder.Sql(@"
DROP PROCEDURE IF EXISTS [dbo].[UpdateWhoisDomainInfo]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateWebDavPortalUsersSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateVirtualGroups]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateUserThemeSetting]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateUserSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateUserPinSecret]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateUserMfaMode]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateUserFailedLoginAttempt]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateSupportServiceLevel]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateStorageSpaceLevel]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateStorageSpaceFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateStorageSpace]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateSfBUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateSfBUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateServiceProperties]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateServiceItem]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateServiceFully]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateService]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateSchedule]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateRDSServerSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateRDSServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateRDSCollectionSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateRDSCollection]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePrivateNetworVLAN]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackageSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackageQuotas]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackageName]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackageDiskSpace]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackageBandwidthUpdate]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackageBandwidth]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackageAddon]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdatePackage]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateLyncUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateLyncUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateHostingPlanQuotas]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateHostingPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateExchangeRetentionPolicyTag]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateExchangeOrganizationSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateExchangeMailboxPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateExchangeDisclaimer]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateExchangeAccountUserPrincipalName]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateExchangeAccountSLSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateExchangeAccount]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateEnterpriseFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateEntepriseFolderStorageSpaceFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateDomainLastUpdateDate]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateDomainExpirationDate]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateDomainDates]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateDomainCreationDate]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateDnsRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateCRMUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateBackgroundTask]
GO
DROP PROCEDURE IF EXISTS [dbo].[UpdateAdditionalGroup]
GO
DROP PROCEDURE IF EXISTS [dbo].[SfBUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetUserOneTimePassword]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetSystemSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetSfBUserSfBUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetOrganizationDefaultSfBUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetOrganizationDefaultLyncUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetOrganizationDefaultExchangeMailboxPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetLyncUserLyncUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetItemPrivatePrimaryIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetItemPrimaryIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetExchangeAccountMailboxplan]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetExchangeAccountDisclaimerId]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetAccessTokenSmsResponse]
GO
DROP PROCEDURE IF EXISTS [dbo].[SearchServiceItemsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[SearchOrganizationAccounts]
GO
DROP PROCEDURE IF EXISTS [dbo].[SearchExchangeAccountsByTypes]
GO
DROP PROCEDURE IF EXISTS [dbo].[SearchExchangeAccounts]
GO
DROP PROCEDURE IF EXISTS [dbo].[SearchExchangeAccount]
GO
DROP PROCEDURE IF EXISTS [dbo].[RemoveStorageSpaceLevel]
GO
DROP PROCEDURE IF EXISTS [dbo].[RemoveStorageSpaceFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[RemoveStorageSpace]
GO
DROP PROCEDURE IF EXISTS [dbo].[RemoveRDSUserFromRDSCollection]
GO
DROP PROCEDURE IF EXISTS [dbo].[RemoveRDSServerFromOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[RemoveRDSServerFromCollection]
GO
DROP PROCEDURE IF EXISTS [dbo].[OrganizationUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[OrganizationExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[MoveServiceItem]
GO
DROP PROCEDURE IF EXISTS [dbo].[LyncUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[InsertStorageSpaceLevel]
GO
DROP PROCEDURE IF EXISTS [dbo].[InsertStorageSpace]
GO
DROP PROCEDURE IF EXISTS [dbo].[InsertCRMUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetWebDavPortalUsersSettingsByAccountId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetWebDavAccessTokenById]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetWebDavAccessTokenByAccessToken]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetVirtualServices]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetVirtualServers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetVirtualMachinesPagedProxmox]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetVirtualMachinesPagedForPC]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetVirtualMachinesPaged2012]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetVirtualMachinesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUsersSummary]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUsersPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserServiceID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserPeers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserParents]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserPackagesServerUrls]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserEnterpriseFolderWithOwaEditPermission]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserDomainsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserByUsernameInternally]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserByUsername]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserByIdInternally]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserById]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserByExchangeOrganizationIdInternally]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserAvailableHostingPlans]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUserAvailableHostingAddons]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUnallottedVLANs]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetUnallottedIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetThreadBackgroundTasks]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetThemeSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetThemeSetting]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetThemes]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSystemSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSupportServiceLevels]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSupportServiceLevel]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpacesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpacesByResourceGroupName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpacesByLevelId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpaceLevelsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpaceLevelById]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpaceFoldersByStorageSpaceId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpaceFolderById]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpaceByServiceAndPath]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetStorageSpaceById]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSSLCertificateByID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSiteCert]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSfBUsersCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSfBUsersByPlanId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSfBUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSfBUserPlans]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSfBUserPlanByAccountId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSfBUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServicesByServerIDGroupName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServicesByServerID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServicesByGroupName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServicesByGroupID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceProperties]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemTypes]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemType]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemsForStatistics]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemsCountByNameAndServiceId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemsCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemsByService]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemsByPackage]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemsByName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItems]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItemByName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServiceItem]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetService]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServerShortDetails]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServerInternal]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServerByName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSearchTableByColumns]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSearchObject]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSearchableServiceItemTypes]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetScheduleTaskViewConfigurations]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetScheduleTasks]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetScheduleTaskEmailTemplate]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetScheduleTask]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSchedulesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSchedules]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetScheduleParameters]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetScheduleInternal]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetScheduleBackgroundTasks]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetSchedule]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetResourceGroups]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetResourceGroupByName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetResourceGroup]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetResellerDomains]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSServersPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSServerSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSServersByItemId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSServersByCollectionId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSServers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSServerById]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSMessages]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSControllerServiceIDbyFQDN]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSCollectionUsersByRDSCollectionId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSCollectionsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSCollectionSettingsByCollectionId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSCollectionsByItemId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSCollectionByName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSCollectionById]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRDSCertificateByServiceId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetRawServicesByServerID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetQuotas]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetProviderServiceQuota]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetProviders]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetProviderByServiceID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetProvider]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetProcessBackgroundTasks]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPrivateNetworVLANsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPrivateNetworVLAN]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPendingSSLForWebsite]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetParentPackageQuotas]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageUnassignedIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackagesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageServiceID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackagesDiskspacePaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackagesBandwidthPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackages]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageQuotasForEdit]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageQuotas]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageQuota]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackagePrivateNetworkVLANs]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackagePrivateIPAddressesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackagePrivateIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackagePackages]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageIPAddressesCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageDiskspace]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageBandwidthUpdate]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageBandwidth]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageAddons]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageAddon]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackage]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationStoragSpacesFolderByType]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationStoragSpaceFolders]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationStatistics]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationRdsUsersCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationRdsServersCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationRdsCollectionsCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationObjectsByDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationGroupsByDisplayName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationDeletedUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOrganizationCRMUserCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOCSUsersCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetOCSUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetNextSchedule]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetNestedPackagesSummary]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetNestedPackagesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetMyPackages]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetLyncUsersCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetLyncUsersByPlanId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetLyncUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetLyncUserPlans]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetLyncUserPlanByAccountId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetLyncUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetLevelResourceGroups]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetItemPrivateIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetItemIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetItemIdByOrganizationId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetIPAddressesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetInstanceID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetHostingPlans]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetHostingPlanQuotas]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetHostingPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetHostingAddons]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetGroupProviders]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetFilterURLByHostingPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetFilterURL]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeRetentionPolicyTags]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeRetentionPolicyTag]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeOrganizationStatistics]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeOrganizationSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeOrganizationDomains]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeMailboxPlans]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeMailboxPlanRetentionPolicyTags]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeMailboxPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeMailboxes]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeDisclaimers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeDisclaimer]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccountsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccounts]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccountEmailAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccountDisclaimerId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccountByMailboxPlanId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccountByAccountNameWithoutItemId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccountByAccountName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetExchangeAccount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetEnterpriseFoldersPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetEnterpriseFolders]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetEnterpriseFolderOwaUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetEnterpriseFolderId]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetEnterpriseFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomainsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomainsByZoneID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomainsByDomainItemID]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomains]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomainDnsRecords]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomainByName]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomainAllDnsRecords]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDnsRecordsTotal]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDnsRecordsByService]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDnsRecordsByServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDnsRecordsByPackage]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDnsRecordsByGroup]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDnsRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetCRMUsersCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetCRMUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetCRMUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetCRMOrganizationUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetComments]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetClusters]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetCertificatesForSite]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetBlackBerryUsersCount]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetBlackBerryUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetBackgroundTopTask]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetBackgroundTasks]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetBackgroundTaskParams]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetBackgroundTaskLogs]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetBackgroundTask]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAvailableVirtualServices]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAuditLogTasks]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAuditLogSources]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAuditLogRecordsPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAuditLogRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAllServers]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAllPackages]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAdditionalGroups]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetAccessTokenByAccessToken]
GO
DROP PROCEDURE IF EXISTS [dbo].[ExchangeOrganizationExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[ExchangeOrganizationDomainExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[ExchangeAccountExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[ExchangeAccountEmailAddressExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[DistributePackageServices]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteVirtualServices]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteUserThemeSetting]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteUserEmailAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteSupportServiceLevel]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteSfBUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteSfBUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteServiceItem]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteService]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteSchedule]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteRDSServerSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteRDSServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteRDSCollectionSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteRDSCollection]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeletePrivateNetworkVLAN]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeletePackageAddon]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeletePackage]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteOrganizationUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteOrganizationStoragSpacesFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteOrganizationDeletedUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteOCSUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteLyncUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteLyncUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteLevelResourceGroups]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteItemPrivateIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteItemPrivateIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteItemIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteItemIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteHostingPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExpiredWebDavAccessTokens]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExpiredAccessTokenTokens]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeRetentionPolicyTag]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeOrganizationDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeMailboxPlanRetentionPolicyTag]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeMailboxPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeDisclaimer]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeAccountEmailAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteExchangeAccount]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteEnterpriseFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteDomainDnsRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteDnsRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteCRMOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteComment]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteCluster]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteCertificate]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteBlackBerryUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteBackgroundTasks]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteBackgroundTaskParams]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteBackgroundTask]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteAuditLogRecordsComplete]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteAuditLogRecords]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteAllLogRecords]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteAllEnterpriseFolderOwaUsers]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteAdditionalGroup]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteAccessToken]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeallocatePackageVLAN]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeallocatePackageIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[CreateStorageSpaceFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[ConvertToExchangeOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[CompleteSSLRequest]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckSSLExistsForWebsite]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckSSL]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckSfBUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckServiceLevelUsage]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckServiceItemExistsInService]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckServiceItemExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckRDSServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckOCSUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckLyncUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckDomainUsedByHostedOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[CheckBlackBerryUserExists]
GO
DROP PROCEDURE IF EXISTS [dbo].[ChangeUserPassword]
GO
DROP PROCEDURE IF EXISTS [dbo].[ChangePackageUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[ChangeExchangeAcceptedDomainType]
GO
DROP PROCEDURE IF EXISTS [dbo].[CanChangeMfa]
GO
DROP PROCEDURE IF EXISTS [dbo].[AllocatePackageVLANs]
GO
DROP PROCEDURE IF EXISTS [dbo].[AllocatePackageIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddWebDavPortalUsersSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddWebDavAccessToken]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddVirtualServices]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddUserToRDSCollection]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddSupportServiceLevel]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddSSLRequest]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddSfBUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddSfBUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddServiceItem]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddService]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddSchedule]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddRDSServerToOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddRDSServerToCollection]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddRDSServer]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddRDSMessage]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddRDSCollectionSettings]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddRDSCollection]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddRDSCertificate]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddPrivateNetworkVlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddPFX]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddPackageAddon]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddPackage]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddOrganizationStoragSpacesFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddOrganizationDeletedUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddOCSUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddLyncUserPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddLyncUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddLevelResourceGroups]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddItemPrivateIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddItemIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddHostingPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeRetentionPolicyTag]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeOrganizationDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeOrganization]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeMailboxPlanRetentionPolicyTag]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeMailboxPlan]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeDisclaimer]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeAccountEmailAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddExchangeAccount]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddEnterpriseFolderOwaUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddEnterpriseFolder]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddDomainDnsRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddDomain]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddDnsRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddComment]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddCluster]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddBlackBerryUser]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddBackgroundTaskStack]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddBackgroundTaskParam]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddBackgroundTaskLog]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddBackgroundTask]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddAuditLogRecord]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddAdditionalGroup]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddAccessToken]
GO
DROP FUNCTION IF EXISTS [dbo].[UsersTree]
GO
DROP FUNCTION IF EXISTS [dbo].[UserParents]
GO
DROP FUNCTION IF EXISTS [dbo].[SplitString]
GO
DROP FUNCTION IF EXISTS [dbo].[PackagesTree]
GO
DROP FUNCTION IF EXISTS [dbo].[PackageParents]
GO
DROP FUNCTION IF EXISTS [dbo].[GetPackageServiceLevelResource]
GO
DROP FUNCTION IF EXISTS [dbo].[GetPackageExceedingQuotas]
GO
DROP FUNCTION IF EXISTS [dbo].[GetPackageAllocatedResource]
GO
DROP FUNCTION IF EXISTS [dbo].[GetPackageAllocatedQuota]
GO
DROP FUNCTION IF EXISTS [dbo].[GetItemComments]
GO
DROP FUNCTION IF EXISTS [dbo].[GetFullIPAddress]
GO
DROP FUNCTION IF EXISTS [dbo].[CheckUserParent]
GO
DROP FUNCTION IF EXISTS [dbo].[CheckPackageParent]
GO
DROP FUNCTION IF EXISTS [dbo].[CheckIsUserAdmin]
GO
DROP FUNCTION IF EXISTS [dbo].[CheckExceedingQuota]
GO
DROP FUNCTION IF EXISTS [dbo].[CheckActorUserRights]
GO
DROP FUNCTION IF EXISTS [dbo].[CheckActorParentPackageRights]
GO
DROP FUNCTION IF EXISTS [dbo].[CheckActorPackageRights]
GO
DROP FUNCTION IF EXISTS [dbo].[CanUpdateUserDetails]
GO
DROP FUNCTION IF EXISTS [dbo].[CanUpdatePackageDetails]
GO
DROP FUNCTION IF EXISTS [dbo].[CanGetUserPassword]
GO
DROP FUNCTION IF EXISTS [dbo].[CanGetUserDetails]
GO
DROP FUNCTION IF EXISTS [dbo].[CanCreateUser]
GO
DROP FUNCTION IF EXISTS [dbo].[CanChangeMfaFunc]
GO
DROP FUNCTION IF EXISTS [dbo].[CalculateQuotaUsage]
GO
DROP FUNCTION IF EXISTS [dbo].[CalculatePackageDiskspace]
GO
DROP FUNCTION IF EXISTS [dbo].[CalculatePackageBandwidth]
GO
			");
		}
	}
}

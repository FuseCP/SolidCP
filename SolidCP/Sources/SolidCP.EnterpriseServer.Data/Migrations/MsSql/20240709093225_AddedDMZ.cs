using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.MsSql
{
    /// <inheritdoc />
    public partial class AddedDMZ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__06ADD4BD",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__03D16812",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__098A4168",
                table: "BackgroundTaskStack");

            migrationBuilder.DropForeignKey(
                name: "FK_HostingPlans_Packages",
                table: "HostingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_HostingPlans",
                table: "Packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WebDavAc__3214EC27B27DC571",
                table: "WebDavAccessTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DomainDn__3214EC2758B0A6F1",
                table: "DomainDnsRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E44466F62E48BE6",
                table: "BackgroundTaskStack");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__3214EC271AFAB817",
                table: "BackgroundTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__F80C6297E2E5AF88",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E5499A830A1D5BF",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Addition__3214EC272F1861EB",
                table: "AdditionalGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccessTo__3214EC27A32557FE",
                table: "AccessTokens");

            migrationBuilder.DeleteData(
                table: "Versions",
                keyColumn: "DatabaseVersion",
                keyValue: "2.0.0.228");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "TempIds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDmz",
                table: "PackageVLANs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WebDavAc__3214EC2708781F08",
                table: "WebDavAccessTokens",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DomainDn__3214EC27A6FC0498",
                table: "DomainDnsRecords",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E44466FB8A5F217",
                table: "BackgroundTaskStack",
                column: "TaskStackID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__3214EC273A1145AC",
                table: "BackgroundTasks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__F80C629777BF580B",
                table: "BackgroundTaskParameters",
                column: "ParameterID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E5499A86067A6E5",
                table: "BackgroundTaskLogs",
                column: "LogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Addition__3214EC27E665DDE2",
                table: "AdditionalGroups",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccessTo__3214EC27DEAEF66E",
                table: "AccessTokens",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "DmzIPAddresses",
                columns: table => new
                {
                    DmzAddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    IPAddress = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmzIPAddresses", x => x.DmzAddressID);
                    table.ForeignKey(
                        name: "FK_DmzIPAddresses_ServiceItems",
                        column: x => x.ItemID,
                        principalTable: "ServiceItems",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 701,
                column: "ItemTypeID",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 702,
                column: "ItemTypeID",
                value: 72);

            migrationBuilder.InsertData(
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[,]
                {
                    { 750, 33, null, null, null, "DMZ Network", "VPS2012.DMZNetworkEnabled", 22, 1, false },
                    { 751, 33, null, null, null, "Number of DMZ IP addresses per VPS", "VPS2012.DMZIPAddressesNumber", 23, 3, false },
                    { 752, 33, null, null, null, "Number of DMZ Network VLANs", "VPS2012.DMZVLANsNumber", 24, 2, false }
                });

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "AccountSummaryLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Account Summary Information</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; }\r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\"></a>\r\n<div class=\"Header\">\r\n	Hosting Account Information\r\n</div>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n\r\n<p>\r\nNew user account has been created and below you can find its summary information.\r\n</p>\r\n\r\n<h1>Control Panel URL</h1>\r\n<table>\r\n    <thead>\r\n        <tr>\r\n            <th>Control Panel URL</th>\r\n            <th>Username</th>\r\n            <th>Password</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n            <td><a href=\"http://panel.HostingCompany.com\">http://panel.HostingCompany.com</a></td>\r\n            <td>#user.Username#</td>\r\n            <td>#user.Password#</td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n</ad:if>\r\n\r\n<h1>Hosting Spaces</h1>\r\n<p>\r\n    The following hosting spaces have been created under your account:\r\n</p>\r\n<ad:foreach collection=\"#Spaces#\" var=\"Space\" index=\"i\">\r\n<h2>#Space.PackageName#</h2>\r\n<table>\r\n	<tbody>\r\n		<tr>\r\n			<td class=\"Label\">Hosting Plan:</td>\r\n			<td>\r\n				<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>\r\n			</td>\r\n		</tr>\r\n		<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">\r\n		<tr>\r\n			<td class=\"Label\">Purchase Date:</td>\r\n			<td>\r\n# Space.PurchaseDate#\r\n			</td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Disk Space, MB:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Diskspace\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Bandwidth, MB/Month:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Bandwidth\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Domains\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Sub-Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.SubDomains\" /></td>\r\n		</tr>\r\n		</ad:if>\r\n	</tbody>\r\n</table>\r\n</ad:foreach>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards,<br />\r\nSolidCP.<br />\r\nWeb Site: <a href=\"https://solidcp.com\">https://solidcp.com</a><br />\r\nE-Mail: <a href=\"mailto:support@solidcp.com\">support@solidcp.com</a>\r\n</p>\r\n</ad:if>\r\n\r\n<ad:template name=\"NumericQuota\">\r\n	<ad:if test=\"#space.Quotas.ContainsKey(quota)#\">\r\n		<ad:if test=\"#space.Quotas[quota].QuotaAllocatedValue isnot -1#\">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>\r\n	<ad:else>\r\n		0\r\n	</ad:if>\r\n</ad:template>\r\n\r\n</div>\r\n</body>\r\n</html>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "DomainLookupLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   MX and NS Changes Information\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nPlease, find below details of MX and NS changes.\r\n\r\n\r\n<ad:foreach collection=\"#Domains#\" var=\"Domain\" index=\"i\">\r\n\r\n# Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#\r\n Registrar:      #iif(isnull(Domain.Registrar), \"\", Domain.Registrar)#\r\n ExpirationDate: #iif(isnull(Domain.ExpirationDate), \"\", Domain.ExpirationDate)#\r\n\r\n        <ad:foreach collection=\"#Domain.DnsChanges#\" var=\"DnsChange\" index=\"j\">\r\n            DNS:       #DnsChange.DnsServer#\r\n            Type:      #DnsChange.Type#\r\n	    Status:    #DnsChange.Status#\r\n            Old Value: #DnsChange.OldRecord.Value#\r\n            New Value: #DnsChange.NewRecord.Value#\r\n\r\n    	</ad:foreach>\r\n</ad:foreach>\r\n\r\n\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "SMSBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "\r\nUser have been created. Password request url:\r\n# passwordResetLink#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password request notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour account have been created. In order to create a password for your account, please follow next link:\r\n\r\n# passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordExpirationLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password expiration notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:\r\n\r\n# passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetLinkSmsBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "Password reset link:\r\n# passwordResetLink#\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.\r\n\r\n# passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password reset notification</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class=\"Header\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password reset notification</h1>\r\n\r\n<ad:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n</p>\r\n\r\n# passwordResetPincode#\r\n\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p>\r\n</div>\r\n</body>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetPincodeSmsBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "\r\nYour password reset pincode:\r\n# passwordResetPincode#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n\r\n# passwordResetPincode#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "VerificationCodeLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   Verification code\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nto complete the sign in, enter the verification code on the device.\r\n\r\nVerification code\r\n# verificationCode#\r\n\r\nBest regards,\r\n");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Changed",
                value: new DateTime(2010, 7, 16, 10, 53, 2, 453, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "DmzIPAddressesIdx_ItemID",
                table: "DmzIPAddresses",
                column: "ItemID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__7D8391DF",
                table: "BackgroundTaskLogs",
                column: "TaskID",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__7AA72534",
                table: "BackgroundTaskParameters",
                column: "TaskID",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__005FFE8A",
                table: "BackgroundTaskStack",
                column: "TaskID",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_HostingPlans",
                table: "Packages",
                column: "PlanID",
                principalTable: "HostingPlans",
                principalColumn: "PlanID",
                onDelete: ReferentialAction.Cascade);

			migrationBuilder.Sql(@"
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

DROP PROCEDURE IF EXISTS [dbo].[GetPackageServiceID]
GO
CREATE PROCEDURE [dbo].[GetPackageServiceID]
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
            ");

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__7D8391DF",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__7AA72534",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__005FFE8A",
                table: "BackgroundTaskStack");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_HostingPlans",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "DmzIPAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WebDavAc__3214EC2708781F08",
                table: "WebDavAccessTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DomainDn__3214EC27A6FC0498",
                table: "DomainDnsRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E44466FB8A5F217",
                table: "BackgroundTaskStack");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__3214EC273A1145AC",
                table: "BackgroundTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__F80C629777BF580B",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E5499A86067A6E5",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Addition__3214EC27E665DDE2",
                table: "AdditionalGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccessTo__3214EC27DEAEF66E",
                table: "AccessTokens");

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 750);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 751);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 752);

            migrationBuilder.DropColumn(
                name: "Date",
                table: "TempIds");

            migrationBuilder.DropColumn(
                name: "IsDmz",
                table: "PackageVLANs");

            migrationBuilder.AddPrimaryKey(
                name: "PK__WebDavAc__3214EC27B27DC571",
                table: "WebDavAccessTokens",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DomainDn__3214EC2758B0A6F1",
                table: "DomainDnsRecords",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E44466F62E48BE6",
                table: "BackgroundTaskStack",
                column: "TaskStackID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__3214EC271AFAB817",
                table: "BackgroundTasks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__F80C6297E2E5AF88",
                table: "BackgroundTaskParameters",
                column: "ParameterID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E5499A830A1D5BF",
                table: "BackgroundTaskLogs",
                column: "LogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Addition__3214EC272F1861EB",
                table: "AdditionalGroups",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccessTo__3214EC27A32557FE",
                table: "AccessTokens",
                column: "ID");

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 701,
                column: "ItemTypeID",
                value: 39);

            migrationBuilder.UpdateData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 702,
                column: "ItemTypeID",
                value: 40);

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "AccountSummaryLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Account Summary Information</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; }\r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n\r\n<a name=\"top\"></a>\r\n<div class=\"Header\">\r\n	Hosting Account Information\r\n</div>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n\r\n<p>\r\nNew user account has been created and below you can find its summary information.\r\n</p>\r\n\r\n<h1>Control Panel URL</h1>\r\n<table>\r\n    <thead>\r\n        <tr>\r\n            <th>Control Panel URL</th>\r\n            <th>Username</th>\r\n            <th>Password</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n        <tr>\r\n            <td><a href=\"http://panel.HostingCompany.com\">http://panel.HostingCompany.com</a></td>\r\n            <td>#user.Username#</td>\r\n            <td>#user.Password#</td>\r\n        </tr>\r\n    </tbody>\r\n</table>\r\n</ad:if>\r\n\r\n<h1>Hosting Spaces</h1>\r\n<p>\r\n    The following hosting spaces have been created under your account:\r\n</p>\r\n<ad:foreach collection=\"#Spaces#\" var=\"Space\" index=\"i\">\r\n<h2>#Space.PackageName#</h2>\r\n<table>\r\n	<tbody>\r\n		<tr>\r\n			<td class=\"Label\">Hosting Plan:</td>\r\n			<td>\r\n				<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>\r\n			</td>\r\n		</tr>\r\n		<ad:if test=\"#not(isnull(Plans[Space.PlanId]))#\">\r\n		<tr>\r\n			<td class=\"Label\">Purchase Date:</td>\r\n			<td>\r\n				#Space.PurchaseDate#\r\n			</td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Disk Space, MB:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Diskspace\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Bandwidth, MB/Month:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Bandwidth\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.Domains\" /></td>\r\n		</tr>\r\n		<tr>\r\n			<td class=\"Label\">Maximum Number of Sub-Domains:</td>\r\n			<td><ad:NumericQuota space=\"#SpaceContexts[Space.PackageId]#\" quota=\"OS.SubDomains\" /></td>\r\n		</tr>\r\n		</ad:if>\r\n	</tbody>\r\n</table>\r\n</ad:foreach>\r\n\r\n<ad:if test=\"#Signup#\">\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards,<br />\r\nSolidCP.<br />\r\nWeb Site: <a href=\"https://solidcp.com\">https://solidcp.com</a><br />\r\nE-Mail: <a href=\"mailto:support@solidcp.com\">support@solidcp.com</a>\r\n</p>\r\n</ad:if>\r\n\r\n<ad:template name=\"NumericQuota\">\r\n	<ad:if test=\"#space.Quotas.ContainsKey(quota)#\">\r\n		<ad:if test=\"#space.Quotas[quota].QuotaAllocatedValue isnot -1#\">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>\r\n	<ad:else>\r\n		0\r\n	</ad:if>\r\n</ad:template>\r\n\r\n</div>\r\n</body>\r\n</html>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "DomainLookupLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   MX and NS Changes Information\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nPlease, find below details of MX and NS changes.\r\n\r\n\r\n<ad:foreach collection=\"#Domains#\" var=\"Domain\" index=\"i\">\r\n\r\n #Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#\r\n Registrar:      #iif(isnull(Domain.Registrar), \"\", Domain.Registrar)#\r\n ExpirationDate: #iif(isnull(Domain.ExpirationDate), \"\", Domain.ExpirationDate)#\r\n\r\n        <ad:foreach collection=\"#Domain.DnsChanges#\" var=\"DnsChange\" index=\"j\">\r\n            DNS:       #DnsChange.DnsServer#\r\n            Type:      #DnsChange.Type#\r\n	    Status:    #DnsChange.Status#\r\n            Old Value: #DnsChange.OldRecord.Value#\r\n            New Value: #DnsChange.NewRecord.Value#\r\n\r\n    	</ad:foreach>\r\n</ad:foreach>\r\n\r\n\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "SMSBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "\r\nUser have been created. Password request url:\r\n#passwordResetLink#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "OrganizationUserPasswordRequestLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password request notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour account have been created. In order to create a password for your account, please follow next link:\r\n\r\n#passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordExpirationLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password expiration notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nYour password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:\r\n\r\n#passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetLinkSmsBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "Password reset link:\r\n#passwordResetLink#\r\n");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.\r\n\r\n#passwordResetLink#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "HtmlBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>Password reset notification</title>\r\n    <style type=\"text/css\">\r\n		.Summary { background-color: ##ffffff; padding: 5px; }\r\n		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }\r\n        .Summary A { color: ##0153A4; }\r\n        .Summary { font-family: Tahoma; font-size: 9pt; }\r\n        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }\r\n        .Summary H2 { font-size: 1.3em; color: ##1F4978; } \r\n        .Summary TABLE { border: solid 1px ##e5e5e5; }\r\n        .Summary TH,\r\n        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }\r\n        .Summary TD { padding: 8px; font-size: 9pt; }\r\n        .Summary UL LI { font-size: 1.1em; font-weight: bold; }\r\n        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"Summary\">\r\n<div class=\"Header\">\r\n<img src=\"#logoUrl#\">\r\n</div>\r\n<h1>Password reset notification</h1>\r\n\r\n<ad:if test=\"#user#\">\r\n<p>\r\nHello #user.FirstName#,\r\n</p>\r\n</ad:if>\r\n\r\n<p>\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n</p>\r\n\r\n#passwordResetPincode#\r\n\r\n<p>\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n</p>\r\n\r\n<p>\r\nBest regards\r\n</p>\r\n</div>\r\n</body>");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "PasswordResetPincodeSmsBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "\r\nYour password reset pincode:\r\n#passwordResetPincode#");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "UserPasswordResetPincodeLetter", 1 },
                column: "PropertyValue",
                value: "=========================================\r\n   Password reset notification\r\n=========================================\r\n\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nWe received a request to reset the password for your account. Your password reset pincode:\r\n\r\n#passwordResetPincode#\r\n\r\nIf you have any questions regarding your hosting account, feel free to contact our support department at any time.\r\n\r\nBest regards");

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "UserID" },
                keyValues: new object[] { "TextBody", "VerificationCodeLetter", 1 },
                column: "PropertyValue",
                value: "=================================\r\n   Verification code\r\n=================================\r\n<ad:if test=\"#user#\">\r\nHello #user.FirstName#,\r\n</ad:if>\r\n\r\nto complete the sign in, enter the verification code on the device.\r\n\r\nVerification code\r\n#verificationCode#\r\n\r\nBest regards,\r\n");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Changed",
                value: new DateTime(2010, 7, 16, 12, 53, 2, 453, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "Versions",
                columns: new[] { "DatabaseVersion", "BuildDate" },
                values: new object[] { "2.0.0.228", new DateTime(2012, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__06ADD4BD",
                table: "BackgroundTaskLogs",
                column: "TaskID",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__03D16812",
                table: "BackgroundTaskParameters",
                column: "TaskID",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__098A4168",
                table: "BackgroundTaskStack",
                column: "TaskID",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HostingPlans_Packages",
                table: "HostingPlans",
                column: "PackageID",
                principalTable: "Packages",
                principalColumn: "PackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_HostingPlans",
                table: "Packages",
                column: "PlanID",
                principalTable: "HostingPlans",
                principalColumn: "PlanID");

			migrationBuilder.Sql(@"
DROP PROCEDURE IF EXISTS [dbo].[GetPackageDmzNetworkVLANs]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteItemDmzIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetItemDmzIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[DeleteItemDmzIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[SetItemDmzPrimaryIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[AddItemDmzIPAddress]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageDmzIPAddressesPaged]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageDmzIPAddresses]
GO
DROP PROCEDURE IF EXISTS [dbo].[GetPackageServiceID]
GO

CREATE PROCEDURE [dbo].[GetPackageServiceID]
(
	@ActorID int,
	@PackageID int,
	@GroupName nvarchar(100),
	@ServiceID int OUTPUT
)
AS
BEGIN
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
END
GO
            ");

		}
	}
}

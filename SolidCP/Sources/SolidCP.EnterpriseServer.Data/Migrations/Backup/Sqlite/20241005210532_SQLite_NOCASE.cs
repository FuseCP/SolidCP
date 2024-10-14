using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolidCP.EnterpriseServer.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class SQLite_NOCASE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "UserSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "UserSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ThemeSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "ThemeSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "SystemSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "SystemSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "VoicePolicy",
                table: "SfBUserPlans",
                type: "TEXT COLLATE NOCASE",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyVoicePolicy",
                table: "SfBUserPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyDialPlanPolicy",
                table: "SfBUserPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArchivePolicy",
                table: "SfBUserPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ServiceProperties",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ServiceItemProperties",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ServiceDefaultProperties",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ADAuthenticationType",
                table: "Servers",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfigurationID",
                table: "ScheduleTaskViewConfiguration",
                type: "TEXT COLLATE NOCASE",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ParameterID",
                table: "ScheduleTaskParameters",
                type: "TEXT COLLATE NOCASE",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ParameterID",
                table: "ScheduleParameters",
                type: "TEXT COLLATE NOCASE",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ScheduleTypeID",
                table: "Schedule",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "ResourceGroups",
                type: "TEXT COLLATE NOCASE",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RecordType",
                table: "ResourceGroupDnsRecords",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "RDSServerSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "RDSServerSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RDSCollections",
                type: "TEXT COLLATE NOCASE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuotaName",
                table: "Quotas",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "PrivateIPAddresses",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "PackageSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "PackageSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "InstanceID",
                table: "OCSUsers",
                type: "TEXT COLLATE NOCASE",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SipAddress",
                table: "LyncUsers",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VoicePolicy",
                table: "LyncUserPlans",
                type: "TEXT COLLATE NOCASE",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyVoicePolicy",
                table: "LyncUserPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyDialPlanPolicy",
                table: "LyncUserPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LyncUserPlanName",
                table: "LyncUserPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "ArchivePolicy",
                table: "LyncUserPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubnetMask",
                table: "IPAddresses",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternalIP",
                table: "IPAddresses",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalIP",
                table: "IPAddresses",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultGateway",
                table: "IPAddresses",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlanName",
                table: "HostingPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "RecordType",
                table: "GlobalDnsRecords",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "ExchangeRetentionPolicyTags",
                type: "TEXT COLLATE NOCASE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ExchangeOrganizationSsFolders",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "ExchangeOrganizationSettings",
                type: "TEXT COLLATE NOCASE",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationID",
                table: "ExchangeOrganizations",
                type: "TEXT COLLATE NOCASE",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "MailboxPlan",
                table: "ExchangeMailboxPlans",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "DisclaimerName",
                table: "ExchangeDisclaimers",
                type: "TEXT COLLATE NOCASE",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Domain",
                table: "EnterpriseFolders",
                type: "TEXT COLLATE NOCASE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DomainName",
                table: "Domains",
                type: "TEXT COLLATE NOCASE",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "DmzIPAddresses",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "ItemTypeID",
                table: "Comments",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BackgroundTaskParameters",
                type: "TEXT COLLATE NOCASE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecordID",
                table: "AuditLog",
                type: "TEXT COLLATE NOCASE",
                unicode: false,
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldUnicode: false,
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "AdditionalGroups",
                type: "TEXT COLLATE NOCASE",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "UserSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "UserSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ThemeSettings",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "ThemeSettings",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "SystemSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "SystemSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "VoicePolicy",
                table: "SfBUserPlans",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "TEXT COLLATE NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyVoicePolicy",
                table: "SfBUserPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyDialPlanPolicy",
                table: "SfBUserPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArchivePolicy",
                table: "SfBUserPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ServiceProperties",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ServiceItemProperties",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "ServiceDefaultProperties",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ADAuthenticationType",
                table: "Servers",
                type: "TEXT",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfigurationID",
                table: "ScheduleTaskViewConfiguration",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ParameterID",
                table: "ScheduleTaskParameters",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ParameterID",
                table: "ScheduleParameters",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ScheduleTypeID",
                table: "Schedule",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "ResourceGroups",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RecordType",
                table: "ResourceGroupDnsRecords",
                type: "TEXT",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "RDSServerSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "RDSServerSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RDSCollections",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuotaName",
                table: "Quotas",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "PrivateIPAddresses",
                type: "TEXT",
                unicode: false,
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "PackageSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "PackageSettings",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "InstanceID",
                table: "OCSUsers",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SipAddress",
                table: "LyncUsers",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VoicePolicy",
                table: "LyncUserPlans",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "TEXT COLLATE NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyVoicePolicy",
                table: "LyncUserPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TelephonyDialPlanPolicy",
                table: "LyncUserPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LyncUserPlanName",
                table: "LyncUserPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "ArchivePolicy",
                table: "LyncUserPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubnetMask",
                table: "IPAddresses",
                type: "TEXT",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternalIP",
                table: "IPAddresses",
                type: "TEXT",
                unicode: false,
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalIP",
                table: "IPAddresses",
                type: "TEXT",
                unicode: false,
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultGateway",
                table: "IPAddresses",
                type: "TEXT",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlanName",
                table: "HostingPlans",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "RecordType",
                table: "GlobalDnsRecords",
                type: "TEXT",
                unicode: false,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "ExchangeRetentionPolicyTags",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ExchangeOrganizationSsFolders",
                type: "TEXT",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                table: "ExchangeOrganizationSettings",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationID",
                table: "ExchangeOrganizations",
                type: "TEXT",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "MailboxPlan",
                table: "ExchangeMailboxPlans",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "DisclaimerName",
                table: "ExchangeDisclaimers",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Domain",
                table: "EnterpriseFolders",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DomainName",
                table: "Domains",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "DmzIPAddresses",
                type: "TEXT",
                unicode: false,
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "ItemTypeID",
                table: "Comments",
                type: "TEXT",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BackgroundTaskParameters",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecordID",
                table: "AuditLog",
                type: "TEXT",
                unicode: false,
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldUnicode: false,
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "AdditionalGroups",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT COLLATE NOCASE",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class MySql9AndMaraiDB11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "ProviderID", "DisableAutoDiscovery", "DisplayName", "EditorControl", "GroupID", "ProviderName", "ProviderType" },
                values: new object[,]
                {
                    { 307, null, "MySQL Server 8.3", "MySQL", 90, "MySQL", "SolidCP.Providers.Database.MySqlServer83, SolidCP.Providers.Database.MySQL" },
                    { 308, null, "MySQL Server 8.4", "MySQL", 90, "MySQL", "SolidCP.Providers.Database.MySqlServer84, SolidCP.Providers.Database.MySQL" },
                    { 320, null, "MySQL Server 9.0", "MySQL", 90, "MySQL", "SolidCP.Providers.Database.MySqlServer90, SolidCP.Providers.Database.MySQL" }
                });

            migrationBuilder.InsertData(
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[] { 125, 90, null, null, null, "Database Truncate", "MySQL9.Truncate", 6, 1, false });

            migrationBuilder.InsertData(
                table: "ResourceGroups",
                columns: new[] { "GroupID", "GroupController", "GroupName", "GroupOrder", "ShowGroup" },
                values: new object[] { 91, "SolidCP.EnterpriseServer.DatabaseServerController", "MySQL9", 12, true });

            migrationBuilder.InsertData(
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[,]
                {
                    { 120, 91, null, 75, null, "Databases", "MySQL9.Databases", 1, 2, false },
                    { 121, 91, null, 76, null, "Users", "MySQL9.Users", 2, 2, false },
                    { 122, 91, null, null, null, "Database Backups", "MySQL9.Backup", 4, 1, false },
                    { 123, 91, null, null, null, "Max Database Size", "MySQL9.MaxDatabaseSize", 3, 3, false },
                    { 124, 91, null, null, null, "Database Restores", "MySQL9.Restore", 5, 1, false }
                });

            migrationBuilder.InsertData(
                table: "ServiceDefaultProperties",
                columns: new[] { "PropertyName", "ProviderID", "PropertyValue" },
                values: new object[,]
                {
                    { "ExternalAddress", 307, "localhost" },
                    { "InstallFolder", 307, "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0" },
                    { "InternalAddress", 307, "localhost,3306" },
                    { "RootLogin", 307, "root" },
                    { "RootPassword", 307, "" },
                    { "sslmode", 307, "True" },
                    { "ExternalAddress", 308, "localhost" },
                    { "InstallFolder", 308, "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0" },
                    { "InternalAddress", 308, "localhost,3306" },
                    { "RootLogin", 308, "root" },
                    { "RootPassword", 308, "" },
                    { "sslmode", 308, "True" },
                    { "ExternalAddress", 320, "localhost" },
                    { "InstallFolder", 320, "%PROGRAMFILES%\\MySQL\\MySQL Server 9.0" },
                    { "InternalAddress", 320, "localhost,3306" },
                    { "RootLogin", 320, "root" },
                    { "RootPassword", 320, "" },
                    { "sslmode", 320, "True" }
                });

            migrationBuilder.InsertData(
                table: "ServiceItemTypes",
                columns: new[] { "ItemTypeID", "Backupable", "CalculateBandwidth", "CalculateDiskspace", "DisplayName", "Disposable", "GroupID", "Importable", "Searchable", "Suspendable", "TypeName", "TypeOrder" },
                values: new object[,]
                {
                    { 90, true, false, true, "MySQL9Database", true, 91, true, true, false, "SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base", 20 },
                    { 91, true, false, false, "MySQL9User", true, 91, true, true, false, "SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base", 21 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 307 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 308 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "sslmode", 320 });

            migrationBuilder.DeleteData(
                table: "ServiceItemTypes",
                keyColumn: "ItemTypeID",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "ServiceItemTypes",
                keyColumn: "ItemTypeID",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "ResourceGroups",
                keyColumn: "GroupID",
                keyValue: 91);
        }
    }
}

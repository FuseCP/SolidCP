using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolidCP.EnterpriseServer.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class Bugfix_for_MySQL_8_x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 305 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.1");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 306 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.2");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 307 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.3");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 308 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 305 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 306 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 307 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 308 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MySQL\\MySQL Server 8.0");
        }
    }
}

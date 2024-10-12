using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolidCP.EnterpriseServer.Data.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class FixUsersHomeForUnix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 500 },
                column: "PropertyValue",
                value: "/var/www/HostingSpaces");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "UsersHome", 500 },
                column: "PropertyValue",
                value: "%HOME%");
        }
    }
}

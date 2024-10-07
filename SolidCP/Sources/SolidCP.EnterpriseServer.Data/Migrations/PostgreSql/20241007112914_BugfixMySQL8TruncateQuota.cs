using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolidCP.EnterpriseServer.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class BugfixMySQL8TruncateQuota : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 125,
                column: "GroupID",
                value: 91);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 125,
                column: "GroupID",
                value: 90);
        }
    }
}

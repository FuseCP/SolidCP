using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class AddedDMZ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__06ADD4BD",
                schema: "public",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__03D16812",
                schema: "public",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__098A4168",
                schema: "public",
                table: "BackgroundTaskStack");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WebDavAc__3214EC27B27DC571",
                schema: "public",
                table: "WebDavAccessTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DomainDn__3214EC2758B0A6F1",
                schema: "public",
                table: "DomainDnsRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E44466F62E48BE6",
                schema: "public",
                table: "BackgroundTaskStack");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__3214EC271AFAB817",
                schema: "public",
                table: "BackgroundTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__F80C6297E2E5AF88",
                schema: "public",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E5499A830A1D5BF",
                schema: "public",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Addition__3214EC272F1861EB",
                schema: "public",
                table: "AdditionalGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccessTo__3214EC27A32557FE",
                schema: "public",
                table: "AccessTokens");

            migrationBuilder.AddColumn<bool>(
                name: "IsDmz",
                schema: "public",
                table: "PackageVLANs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WebDavAc__3214EC2708781F08",
                schema: "public",
                table: "WebDavAccessTokens",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DomainDn__3214EC27A6FC0498",
                schema: "public",
                table: "DomainDnsRecords",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E44466FB8A5F217",
                schema: "public",
                table: "BackgroundTaskStack",
                column: "TaskStackID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__3214EC273A1145AC",
                schema: "public",
                table: "BackgroundTasks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__F80C629777BF580B",
                schema: "public",
                table: "BackgroundTaskParameters",
                column: "ParameterID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E5499A86067A6E5",
                schema: "public",
                table: "BackgroundTaskLogs",
                column: "LogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Addition__3214EC27E665DDE2",
                schema: "public",
                table: "AdditionalGroups",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccessTo__3214EC27DEAEF66E",
                schema: "public",
                table: "AccessTokens",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "DmzIPAddresses",
                schema: "public",
                columns: table => new
                {
                    DmzAddressID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemID = table.Column<int>(type: "integer", nullable: false),
                    IPAddress = table.Column<string>(type: "character varying(15)", unicode: false, maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmzIPAddresses", x => x.DmzAddressID);
                    table.ForeignKey(
                        name: "FK_DmzIPAddresses_ServiceItems",
                        column: x => x.ItemID,
                        principalSchema: "public",
                        principalTable: "ServiceItems",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 701,
                column: "ItemTypeID",
                value: 71);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 702,
                column: "ItemTypeID",
                value: 72);

            migrationBuilder.InsertData(
                schema: "public",
                table: "Quotas",
                columns: new[] { "QuotaID", "GroupID", "HideQuota", "ItemTypeID", "PerOrganization", "QuotaDescription", "QuotaName", "QuotaOrder", "QuotaTypeID", "ServiceQuota" },
                values: new object[,]
                {
                    { 750, 33, null, null, null, "DMZ Network", "VPS2012.DMZNetworkEnabled", 22, 1, false },
                    { 751, 33, null, null, null, "Number of DMZ IP addresses per VPS", "VPS2012.DMZIPAddressesNumber", 23, 3, false },
                    { 752, 33, null, null, null, "Number of DMZ Network VLANs", "VPS2012.DMZVLANsNumber", 24, 2, false }
                });

            migrationBuilder.CreateIndex(
                name: "DmzIPAddressesIdx_ItemID",
                schema: "public",
                table: "DmzIPAddresses",
                column: "ItemID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__7D8391DF",
                schema: "public",
                table: "BackgroundTaskLogs",
                column: "TaskID",
                principalSchema: "public",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__7AA72534",
                schema: "public",
                table: "BackgroundTaskParameters",
                column: "TaskID",
                principalSchema: "public",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__005FFE8A",
                schema: "public",
                table: "BackgroundTaskStack",
                column: "TaskID",
                principalSchema: "public",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__7D8391DF",
                schema: "public",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__7AA72534",
                schema: "public",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropForeignKey(
                name: "FK__Backgroun__TaskI__005FFE8A",
                schema: "public",
                table: "BackgroundTaskStack");

            migrationBuilder.DropTable(
                name: "DmzIPAddresses",
                schema: "public");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WebDavAc__3214EC2708781F08",
                schema: "public",
                table: "WebDavAccessTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DomainDn__3214EC27A6FC0498",
                schema: "public",
                table: "DomainDnsRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E44466FB8A5F217",
                schema: "public",
                table: "BackgroundTaskStack");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__3214EC273A1145AC",
                schema: "public",
                table: "BackgroundTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__F80C629777BF580B",
                schema: "public",
                table: "BackgroundTaskParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Backgrou__5E5499A86067A6E5",
                schema: "public",
                table: "BackgroundTaskLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Addition__3214EC27E665DDE2",
                schema: "public",
                table: "AdditionalGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccessTo__3214EC27DEAEF66E",
                schema: "public",
                table: "AccessTokens");

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 750);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 751);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 752);

            migrationBuilder.DropColumn(
                name: "IsDmz",
                schema: "public",
                table: "PackageVLANs");

            migrationBuilder.AddPrimaryKey(
                name: "PK__WebDavAc__3214EC27B27DC571",
                schema: "public",
                table: "WebDavAccessTokens",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DomainDn__3214EC2758B0A6F1",
                schema: "public",
                table: "DomainDnsRecords",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E44466F62E48BE6",
                schema: "public",
                table: "BackgroundTaskStack",
                column: "TaskStackID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__3214EC271AFAB817",
                schema: "public",
                table: "BackgroundTasks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__F80C6297E2E5AF88",
                schema: "public",
                table: "BackgroundTaskParameters",
                column: "ParameterID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Backgrou__5E5499A830A1D5BF",
                schema: "public",
                table: "BackgroundTaskLogs",
                column: "LogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Addition__3214EC272F1861EB",
                schema: "public",
                table: "AdditionalGroups",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccessTo__3214EC27A32557FE",
                schema: "public",
                table: "AccessTokens",
                column: "ID");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 701,
                column: "ItemTypeID",
                value: 39);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Quotas",
                keyColumn: "QuotaID",
                keyValue: 702,
                column: "ItemTypeID",
                value: 40);

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__06ADD4BD",
                schema: "public",
                table: "BackgroundTaskLogs",
                column: "TaskID",
                principalSchema: "public",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__03D16812",
                schema: "public",
                table: "BackgroundTaskParameters",
                column: "TaskID",
                principalSchema: "public",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Backgroun__TaskI__098A4168",
                schema: "public",
                table: "BackgroundTaskStack",
                column: "TaskID",
                principalSchema: "public",
                principalTable: "BackgroundTasks",
                principalColumn: "ID");
        }
    }
}

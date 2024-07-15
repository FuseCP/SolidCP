using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.Sqlite
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

            migrationBuilder.AddColumn<bool>(
                name: "IsDmz",
                table: "PackageVLANs",
                type: "INTEGER",
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
                    DmzAddressID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    IPAddress = table.Column<string>(type: "TEXT", unicode: false, maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false)
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
        }
    }
}

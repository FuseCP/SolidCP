using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class RemoveCompositeKeyInThemeSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#0727d7", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#157d4c", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#23282c", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#673ab7", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#795548", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#d3094e", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#e10a1f", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#ff9800", "color-header", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#1f0e3b", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#230924", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#408851", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#5b737f", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#6c85ec", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#903a85", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#a04846", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "#a65314", "color-Sidebar", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "Dark", "Style", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "Light", "Style", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "Minimal", "Style", 1 });

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumns: new[] { "PropertyName", "SettingsName", "ThemeID" },
                keyValues: new object[] { "Semi Dark", "Style", 1 });

			migrationBuilder.DropPrimaryKey(
            	name: "PK_ThemeSettings",
	            schema: "public",
	            table: "ThemeSettings");

			migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                schema: "public",
                table: "ThemeSettings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                schema: "public",
                table: "ThemeSettings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "ThemeID",
                schema: "public",
                table: "ThemeSettings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "ThemeSettingID",
                schema: "public",
                table: "ThemeSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThemeSettings",
                schema: "public",
                table: "ThemeSettings",
                column: "ThemeSettingID");

            migrationBuilder.InsertData(
                schema: "public",
                table: "ThemeSettings",
                columns: new[] { "ThemeSettingID", "PropertyName", "PropertyValue", "SettingsName", "ThemeID" },
                values: new object[,]
                {
                    { 1, "Light", "light-theme", "Style", 1 },
                    { 2, "Dark", "dark-theme", "Style", 1 },
                    { 3, "Semi Dark", "semi-dark", "Style", 1 },
                    { 4, "Minimal", "minimal-theme", "Style", 1 },
                    { 5, "#0727d7", "headercolor1", "color-header", 1 },
                    { 6, "#23282c", "headercolor2", "color-header", 1 },
                    { 7, "#e10a1f", "headercolor3", "color-header", 1 },
                    { 8, "#157d4c", "headercolor4", "color-header", 1 },
                    { 9, "#673ab7", "headercolor5", "color-header", 1 },
                    { 10, "#795548", "headercolor6", "color-header", 1 },
                    { 11, "#d3094e", "headercolor7", "color-header", 1 },
                    { 12, "#ff9800", "headercolor8", "color-header", 1 },
                    { 13, "#6c85ec", "sidebarcolor1", "color-Sidebar", 1 },
                    { 14, "#5b737f", "sidebarcolor2", "color-Sidebar", 1 },
                    { 15, "#408851", "sidebarcolor3", "color-Sidebar", 1 },
                    { 16, "#230924", "sidebarcolor4", "color-Sidebar", 1 },
                    { 17, "#903a85", "sidebarcolor5", "color-Sidebar", 1 },
                    { 18, "#a04846", "sidebarcolor6", "color-Sidebar", 1 },
                    { 19, "#a65314", "sidebarcolor7", "color-Sidebar", 1 },
                    { 20, "#1f0e3b", "sidebarcolor8", "color-Sidebar", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "ThemeSettingsIdx_ThemeID",
                schema: "public",
                table: "ThemeSettings",
                column: "ThemeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ThemeSettingsIdx_ThemeID",
                schema: "public",
                table: "ThemeSettings");

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "ThemeSettings",
                keyColumn: "ThemeSettingID",
                keyColumnType: "integer",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "ThemeSettingID",
                schema: "public",
                table: "ThemeSettings");

			migrationBuilder.DropPrimaryKey(
            	name: "PK_ThemeSettings",
	            schema: "public",
	            table: "ThemeSettings");

			migrationBuilder.AlterColumn<int>(
                name: "ThemeID",
                schema: "public",
                table: "ThemeSettings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                schema: "public",
                table: "ThemeSettings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                schema: "public",
                table: "ThemeSettings",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThemeSettings",
                schema: "public",
                table: "ThemeSettings",
                columns: new[] { "ThemeID", "SettingsName", "PropertyName" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "ThemeSettings",
                columns: new[] { "PropertyName", "SettingsName", "ThemeID", "PropertyValue" },
                values: new object[,]
                {
                    { "#0727d7", "color-header", 1, "headercolor1" },
                    { "#157d4c", "color-header", 1, "headercolor4" },
                    { "#23282c", "color-header", 1, "headercolor2" },
                    { "#673ab7", "color-header", 1, "headercolor5" },
                    { "#795548", "color-header", 1, "headercolor6" },
                    { "#d3094e", "color-header", 1, "headercolor7" },
                    { "#e10a1f", "color-header", 1, "headercolor3" },
                    { "#ff9800", "color-header", 1, "headercolor8" },
                    { "#1f0e3b", "color-Sidebar", 1, "sidebarcolor8" },
                    { "#230924", "color-Sidebar", 1, "sidebarcolor4" },
                    { "#408851", "color-Sidebar", 1, "sidebarcolor3" },
                    { "#5b737f", "color-Sidebar", 1, "sidebarcolor2" },
                    { "#6c85ec", "color-Sidebar", 1, "sidebarcolor1" },
                    { "#903a85", "color-Sidebar", 1, "sidebarcolor5" },
                    { "#a04846", "color-Sidebar", 1, "sidebarcolor6" },
                    { "#a65314", "color-Sidebar", 1, "sidebarcolor7" },
                    { "Dark", "Style", 1, "dark-theme" },
                    { "Light", "Style", 1, "light-theme" },
                    { "Minimal", "Style", 1, "minimal-theme" },
                    { "Semi Dark", "Style", 1, "semi-dark" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolidCP.EnterpriseServer.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class AddMariaDB11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "ProviderID", "DisableAutoDiscovery", "DisplayName", "EditorControl", "GroupID", "ProviderName", "ProviderType" },
                values: new object[,]
                {
                    { 1574, null, "MariaDB 10.7", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB107, SolidCP.Providers.Database.MariaDB" },
                    { 1575, null, "MariaDB 10.8", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB108, SolidCP.Providers.Database.MariaDB" },
                    { 1576, null, "MariaDB 10.9", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB109, SolidCP.Providers.Database.MariaDB" },
                    { 1577, null, "MariaDB 10.10", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB1010, SolidCP.Providers.Database.MariaDB" },
                    { 1578, null, "MariaDB 10.11", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB1011, SolidCP.Providers.Database.MariaDB" },
                    { 1579, null, "MariaDB 11.0", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB110, SolidCP.Providers.Database.MariaDB" },
                    { 1580, null, "MariaDB 11.1", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB111, SolidCP.Providers.Database.MariaDB" },
                    { 1581, null, "MariaDB 11.2", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB112, SolidCP.Providers.Database.MariaDB" },
                    { 1582, null, "MariaDB 11.3", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB113, SolidCP.Providers.Database.MariaDB" },
                    { 1583, null, "MariaDB 11.4", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB114, SolidCP.Providers.Database.MariaDB" },
                    { 1584, null, "MariaDB 11.5", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB115, SolidCP.Providers.Database.MariaDB" },
                    { 1585, null, "MariaDB 11.6", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB116, SolidCP.Providers.Database.MariaDB" },
                    { 1586, null, "MariaDB 11.7", "MariaDB", 50, "MariaDB", "SolidCP.Providers.Database.MariaDB117, SolidCP.Providers.Database.MariaDB" }
                });

            migrationBuilder.UpdateData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1573 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MariaDB 10.6");

            migrationBuilder.InsertData(
                table: "ServiceDefaultProperties",
                columns: new[] { "PropertyName", "ProviderID", "PropertyValue" },
                values: new object[,]
                {
                    { "ExternalAddress", 1574, "localhost" },
                    { "InstallFolder", 1574, "%PROGRAMFILES%\\MariaDB 10.7" },
                    { "InternalAddress", 1574, "localhost" },
                    { "RootLogin", 1574, "root" },
                    { "RootPassword", 1574, "" },
                    { "ExternalAddress", 1575, "localhost" },
                    { "InstallFolder", 1575, "%PROGRAMFILES%\\MariaDB 10.8" },
                    { "InternalAddress", 1575, "localhost" },
                    { "RootLogin", 1575, "root" },
                    { "RootPassword", 1575, "" },
                    { "ExternalAddress", 1576, "localhost" },
                    { "InstallFolder", 1576, "%PROGRAMFILES%\\MariaDB 10.9" },
                    { "InternalAddress", 1576, "localhost" },
                    { "RootLogin", 1576, "root" },
                    { "RootPassword", 1576, "" },
                    { "ExternalAddress", 1577, "localhost" },
                    { "InstallFolder", 1577, "%PROGRAMFILES%\\MariaDB 10.10" },
                    { "InternalAddress", 1577, "localhost" },
                    { "RootLogin", 1577, "root" },
                    { "RootPassword", 1577, "" },
                    { "ExternalAddress", 1578, "localhost" },
                    { "InstallFolder", 1578, "%PROGRAMFILES%\\MariaDB 10.11" },
                    { "InternalAddress", 1578, "localhost" },
                    { "RootLogin", 1578, "root" },
                    { "RootPassword", 1578, "" },
                    { "ExternalAddress", 1579, "localhost" },
                    { "InstallFolder", 1579, "%PROGRAMFILES%\\MariaDB 11.0" },
                    { "InternalAddress", 1579, "localhost" },
                    { "RootLogin", 1579, "root" },
                    { "RootPassword", 1579, "" },
                    { "ExternalAddress", 1580, "localhost" },
                    { "InstallFolder", 1580, "%PROGRAMFILES%\\MariaDB 11.1" },
                    { "InternalAddress", 1580, "localhost" },
                    { "RootLogin", 1580, "root" },
                    { "RootPassword", 1580, "" },
                    { "ExternalAddress", 1581, "localhost" },
                    { "InstallFolder", 1581, "%PROGRAMFILES%\\MariaDB 11.2" },
                    { "InternalAddress", 1581, "localhost" },
                    { "RootLogin", 1581, "root" },
                    { "RootPassword", 1581, "" },
                    { "ExternalAddress", 1582, "localhost" },
                    { "InstallFolder", 1582, "%PROGRAMFILES%\\MariaDB 11.3" },
                    { "InternalAddress", 1582, "localhost" },
                    { "RootLogin", 1582, "root" },
                    { "RootPassword", 1582, "" },
                    { "ExternalAddress", 1583, "localhost" },
                    { "InstallFolder", 1583, "%PROGRAMFILES%\\MariaDB 11.4" },
                    { "InternalAddress", 1583, "localhost" },
                    { "RootLogin", 1583, "root" },
                    { "RootPassword", 1583, "" },
                    { "ExternalAddress", 1584, "localhost" },
                    { "InstallFolder", 1584, "%PROGRAMFILES%\\MariaDB 11.5" },
                    { "InternalAddress", 1584, "localhost" },
                    { "RootLogin", 1584, "root" },
                    { "RootPassword", 1584, "" },
                    { "ExternalAddress", 1585, "localhost" },
                    { "InstallFolder", 1585, "%PROGRAMFILES%\\MariaDB 11.6" },
                    { "InternalAddress", 1585, "localhost" },
                    { "RootLogin", 1585, "root" },
                    { "RootPassword", 1585, "" },
                    { "ExternalAddress", 1586, "localhost" },
                    { "InstallFolder", 1586, "%PROGRAMFILES%\\MariaDB 11.7" },
                    { "InternalAddress", 1586, "localhost" },
                    { "RootLogin", 1586, "root" },
                    { "RootPassword", 1586, "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1574 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1575 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1576 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1577 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1578 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1579 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1580 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1581 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1582 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1583 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1584 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1585 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "ExternalAddress", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InternalAddress", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootLogin", 1586 });

            migrationBuilder.DeleteData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "RootPassword", 1586 });

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1574);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1575);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1576);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1577);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1578);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1579);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1580);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1581);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1582);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1583);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1584);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1585);

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "ProviderID",
                keyValue: 1586);

            migrationBuilder.UpdateData(
                table: "ServiceDefaultProperties",
                keyColumns: new[] { "PropertyName", "ProviderID" },
                keyValues: new object[] { "InstallFolder", 1573 },
                column: "PropertyValue",
                value: "%PROGRAMFILES%\\MariaDB 10.5");
        }
    }
}

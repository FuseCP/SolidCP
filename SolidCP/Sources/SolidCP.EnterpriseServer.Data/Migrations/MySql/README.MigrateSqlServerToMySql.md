# Migrating an existing SQL Server Database to MySQL
(There's a [video on Youtube](https://www.youtube.com/watch?v=1nKWa6v4lGA) explaining the process
of migrating the EnterpriseServer database to MySQL.) In Order to migrate an existing SQL Server
database to MySQL follow these steps:

# Create a fresh MySQL database
- Open MySQL Workbench and create a new schema for the database.
- Select the schema you've created as default schema.
- Open SolidCP.EnterpriseServer.sln or SolidCP.WebPortalAndEnterpriseServerCombined.sln in
  VisualStudio.
- Open the file SolidCP.EnterpriseServer.Data\Migrations\MySql\install.mysql.sql
- Copy the text of the install.mysql.sql script to the Clipboard, create a new query in MySQL
  Workbench and paste the copied text.
- Run the script on the database. This schould create the schema and seed data on your MySQL
  server.

# Create an ODBC Datasource for your SQL Server SolidCP database
- Open **Start > Windows Administrative Tools**.
- Choose the 32-bit or 64-bit ODBC drivers according to your system.
- A window pops up, click **System DSN** tab **> Add**.
- Choose SQL Server from the Data Sources drop-down and click Finish.
- Provide the details in the Connect a New Data Source to SQL Server window.

# Run the Migration Wizard in MySQL Workbench
- In the MySQL Workbench run **Database > Migration Wizard...**
- In the Migration Wizard select as data source the ODBC datasource you've created above,
  and as destination the database schema you've created above.
- When copying the objects, copy all tables but no views and no stored pocedures.
- When prompted to copy the schema objects, select not to copy and not to save to a script.
  (We only want to copy the data, not the schema, since the schema was created by the
  install.mysql.sql script.).
- When prompted to copy the data, select Truncate target tables, so we overwrite the seed data
  in the fresh MySQL database.


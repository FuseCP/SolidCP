set DOTNET_HOST_FACTORY_RESOLVER_DEFAULT_TIMEOUT_IN_SECONDS=0

echo "Remove migration for Sqlite"
dotnet ef migrations remove --framework net8.0 --context SqliteDbContext -- "DbType=Sqlite;Data Source=..\SolidCP.EnterpriseServer\App_Data\SolidCP.sqlite;"

echo "Remove migration for SQL Server"
dotnet ef migrations remove --framework net8.0 --no-build --context SqlServerDbContext -- "DbType=SqlServer;Server=(local);Database=SolidCP;Uid=sa;Pwd=Password12;"

echo "Remove migration for MySQL and MariaDB"
dotnet ef migrations remove --framework net8.0 --no-build --context MySqlDbContext -- "DbType=MySql;Server=localhost;Database=SolidCP;Uid=root;Pwd=Password12;"

echo "Remove migration for PostgreSQL"
dotnet ef migrations remove --framework net8.0 --no-build --context PostgreSqlDbContext -- "DbType=PostgreSql;Host=localhost;User ID=postgres;Password=Password12;Port=5433;Database=SolidCP;"

echo "Create install.sqlite.sql for Sqlite"
dotnet ef migrations script --framework net8.0 -o Migrations\Sqlite\install.sqlite.sql --context SqliteDbContext -- "DbType=Sqlite;Data Source=..\SolidCP.EnterpriseServer\App_Data\SolidCP.sqlite;"

echo "Create install.sqlserver.sql for SQL Server"
dotnet ef migrations script --framework net8.0 --no-build -o Migrations\SqlServer\install.sqlserver.sql --context SqlServerDbContext -i -- "DbType=SqlServer;Server=(local);Database=SolidCP;Uid=sa;Pwd=Password12;"

echo "Create install.mysql.sql for MySQL and MariaDB"
dotnet ef migrations script --framework net8.0 --no-build -o Migrations\MySql\install.mysql.sql --context MySqlDbContext -i -- "DbType=MySql;Server=localhost;Database=SolidCP;Uid=root;Pwd=Password12;"

echo "Create install.postgresql.sql for PostgreSQL"
dotnet ef migrations script --framework net8.0 --no-build -o Migrations\PostgreSql\install.postgresql.sql --context PostgreSqlDbContext -i -- "DbType=PostgreSql;Host=localhost;User ID=postgres;Password=Password12;Port=5433;Database=SolidCP;"

echo "Create install.sqlite.bundle.exe bundle for Sqlite"
REM dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\Sqlite\install.sqlite.bundle.exe --context SqliteDbContext -- "DbType=Sqlite;Data Source=..\SolidCP.EnterpriseServer\App_Data\SolidCP.sqlite;"

echo "Create install.sqlserver.bundle.exe bundle for SQL Server"
REM dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\SqlServer\install.sqlserver.bundle.exe --context SqlServerDbContext -- "DbType=SqlServer;Server=(local);Database=SolidCP;Uid=sa;Pwd=Password12;"

echo "Create install.mysql.bundle.exe bundle for MySQL and MariaDB"
REM dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\MySql\install.mysql.bundle.exe --context MySqlDbContext -i -- "DbType=MySql;Server=localhost;Database=SolidCP;Uid=root;Pwd=Password12;"

echo "Create install.postgresql.bundle.exe bundle for PostgreSQL"
REM dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\PostgreSql\install.postgresql.bundle.exe --context PostgreSqlDbContext -i -- "DbType=PostgreSql;Host=localhost;User ID=postgres;Password=Password12;Port=5433;Database=SolidCP;"

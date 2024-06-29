set /P migration="Please enter a name for the migration: "

echo "Add migration for Sqlite"
dotnet ef migrations add --framework net8.0 -o Migrations\Sqlite --context SqliteDbContext %migration% -- "DbType=Sqlite;Data Source=SolidCP.db;"

echo "Add migration for MS SQL"
dotnet ef migrations add --framework net8.0 --no-build -o Migrations\MsSql --context MsSqlDbContext %migration% -- "DbType=MsSql;Server=(local);Database=SolidCP;Uid=sa;Pwd=Password12;"

echo "Add migration for MySQL and MariaDB"
dotnet ef migrations add --framework net8.0 --no-build -o Migrations\MySql --context MySqlDbContext %migration% -- "DbType=MySql;Server=localhost;Database=SolidCP;Uid=root;Pwd=Password12;"

echo "Add migration for PostgreSQL"
dotnet ef migrations add --framework net8.0 --no-build -o Migrations\PostgreSql --context PostgreSqlDbContext %migration% -- "DbType=PostgreSql;Host=localhost;User ID=root;Password=Password12;Port=5432;Database=SolidCP;"

echo "Create install.sqlite.sql for Sqlite"
dotnet ef migrations script --framework net8.0 --no-build -o Migrations\Sqlite\install.sqlite.sql --context SqliteDbContext -- "DbType=Sqlite;Data Source=SolidCP.db;"

echo "Create install.sqlite.exe for Sqlite"
dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\Sqlite\install.sqlite.exe --context SqliteDbContext -- "DbType=Sqlite;Data Source=SolidCP.db;"

echo "Create install.mssql.sql for MS SQL"
dotnet ef migrations script --framework net8.0 --no-build -o Migrations\MsSql\install.mssql.sql --context MsSqlDbContext -i -- "DbType=MsSql;Server=(local);Database=SolidCP;Uid=sa;Pwd=Password12;"

echo "Create install.mssql.exe for MS SQL"
dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\MsSql\install.mssql.exe --context MsSqlDbContext -- "DbType=MsSql;Server=(local);Database=SolidCP;Uid=sa;Pwd=Password12;"

echo "Create install.mysql.sql for MySQL and MariaDB"
dotnet ef migrations script --framework net8.0 --no-build -o Migrations\MySql\install.mysql.sql --context MySqlDbContext -i -- "DbType=MySql;Server=localhost;Database=SolidCP;Uid=root;Pwd=Password12;"

echo "Create install.mysql.exe for MySQL and MariaDB"
dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\MySql\install.mysql.exe --context MySqlDbContext -i -- "DbType=MySql;Server=localhost;Database=SolidCP;Uid=root;Pwd=Password12;"

echo "Create install.postgresql.sql for PostgreSQL"
dotnet ef migrations script --framework net8.0 --no-build -o Migrations\PostgreSql\install.postgresql.sql --context PostgreSqlDbContext -i -- "DbType=PostgreSql;Host=localhost;User ID=root;Password=Password12;Port=5432;Database=SolidCP;"

echo "Create install.postgresql.exe for PostgreSQL"
dotnet ef migrations bundle --framework net8.0 --no-build -o Migrations\PostgreSql\install.postgresql.exe --context PostgreSqlDbContext -i -- "DbType=PostgreSql;Host=localhost;User ID=root;Password=Password12;Port=5432;Database=SolidCP;"

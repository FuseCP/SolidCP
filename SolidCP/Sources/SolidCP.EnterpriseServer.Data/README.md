# SolidCP.EnterpriseServer.Data
This project contains the EntityFramework database layer of SolidCP.EnterpriseServer. It uses EF Core 8 when running
in NET 8 or EF 6 when running in NET Framework.

# Folder Structure
The entity classes for the entities are contained in the Entities folder. The entity model configuration classes are
contained in the Configuration folder. Migrations are contained in the Migrations folder and in the subfolder
corresponding to the database flavor. The folder CodeTemplates contains the T4 templates used for database scaffolding.
The Extensions folder contains extension classes.

# Scaffolding of the Database
The folder CodeTemplates contains the T4 templates used by the "dotnet ef dbcontext scaffold" command to create the
entity, dbcontext and configuration classes from an existing database. You can run the scaffolding of the database
by executing Scaffold.bat. The connection string for scaffolding must be set in the
SolidCP.EnterpriseServer.Data.csproj in the ScaffoldConnectionString property.
You can also apply changes to the database model done through the old way with update_db.sql, in that you apply
update_db.sql to the database, and then scaffold the database. You will see all the changes to the model in the
Entities\Sources\.., Configuraton\Sources\.. and DbContextBase.Source.cs files. You can then use the diff viewer
to track all changes in those classes and can then apply the changes to the classes in the Entities and Configuration
folders and to DbContextBase.cs.
For portability accross different database flavors, you'll have to tweak the classes in the Entities\Sources folder a
bit. In particular, you'll have to comment out all manual assignmens of a database type by the
[Column(TypeName="...")]. If you want to assign a specific database type, you'll have to do it in the Configuration
classes through the Fluent API, and you need to distinguish the different database flavors, so for a
```
[Colum(TypeName="ntext")]
public string ExecutionLog { get; set; }
```
for example you would write in the Fluent API:
```
if (IsMsSql) {
    Property(e => e.ExecutionLog).HasColumnType("ntext");
} else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql)) {
    Property(e => e.ExecutionLog).HasColumnType("TEXT");
}
```
and comment out the Column attribute like so:
```
// [Colum(TypeName="ntext")]
public string ExecutionLog { get; set; }
```

# Migrations
Migrations are only managed with EF Core, not with EF 6. They are always executed with NET 8.0 by the intaller,
not with NET Framework.

To create a new migration, you can run AddMigration.bat, or if you just want a migration for the database flavor
you're working with, copy the individual lines in AddMigration.bat to a command line shell.

When you create SolidCP release with deploy-release.bat, deploy-release.bat creates backups of the Model Snapshots
..DbContextModelSnapshot.cs files, so you can always create migrations based on the last SolidCP release. When
creating a SolidCP release, one can combine all new migrations into one by reverting the Model Snapshot to that
of the last release and creating a new migration.

# Usage of SolidCP.EnterpriseServer.Data
SolidCP.EnterpriseServer.Data provides a class DbContext, that can be used as EF DbContext to access the database,
It has properties to access the DbSet's of the Entities, and the usual SaveChanges etc. commands. In order to
consume SoldiCP.EnterpriseServer.Data, you don't have to import the assemblies for EF Core 8 or EF 6, just use the
SolidCP.EnterpriseServer.Data.DbContext class. It will use either EF Core 8 or EF 6 for accessing the database
depending on wether you run on NET 8 or on NET Framework.
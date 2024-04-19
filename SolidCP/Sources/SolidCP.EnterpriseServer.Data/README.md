# SolidCP.EnterpriseServer.Data

This project contains the database layer of SolidCP.EnterpriseServer. It uses EF Core 8 when running in NET 8 or 
EF 6 when running in NET Framework. 

# Scaffolding of the Database

The folder CodeTemplates contains the T4 templates used by the "dotnet ef dbcontext scaffold" command to create the
entity, dbcontext and configuration classes from a database. You can run the scaffolding of the database
by executing Scaffold.bat. The connection string for scaffolding must be set in the
SolidCP.EnterpriseServer.Data.csproj.

# Migrations
Migrations are only managed with EF Core, not with EF 6. They are always executed with NET 8.0 by the intaller,
not with NET Framework. 

# Usage of SolidCP.EnterpriseServer.Data
SolidCP.EnterpriseServer.Data provides a class DbContext, that can be used as EF DbContext to access the database,
It has properties to access the DbSet's of the Entities, and the usual SaveChanges etc. commands. The DbSet's are
of type SolidCP.EnterpriseServer.Data.DbSet. In order to consume SoldiCP.EnterpriseServer.Data, you don't have
to import the assemblies for EF Core 8 or EF 6, just use the SolidCP.EnterpriseServer.Data.DbContext and
SolidCP.EnterpriseServer.Data.DbSet classes. They will use either EF Core 8 or EF 6 for accessing the database
depending on wether you run on NET 8 or on NET Framework.

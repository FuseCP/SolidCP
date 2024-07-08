-- Support for EntityFramework

IF OBJECT_ID(N'[TempIds]') IS NULL
BEGIN
    CREATE TABLE [TempIds] (
        [Key] int NOT NULL IDENTITY,
        [Created] datetime2 NOT NULL,
        [Scope] uniqueidentifier NOT NULL,
        [Level] int NOT NULL,
        [Id] int NOT NULL,
        [Date] datetime2 NOT NULL,
        CONSTRAINT [PK_TempIds] PRIMARY KEY ([Key])
    );

	CREATE INDEX [IX_TempIds_Created_Scope_Level] ON [TempIds] ([Created], [Scope], [Level]);
END;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] LIKE N'20240627111421_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240627111421_InitialCreate', N'8.0.6');
END;
GO
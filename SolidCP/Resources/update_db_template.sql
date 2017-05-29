USE [${install.database}]
GO

-- update database version
declare @build_version nvarchar(10), @build_date datetime
set @build_version = '1.0.0.0'
set @build_date = '01/01/2010'
IF NOT EXISTS (SELECT * FROM dbo.Versions WHERE DatabaseVersion = @build_version)
INSERT INTO [Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
GO


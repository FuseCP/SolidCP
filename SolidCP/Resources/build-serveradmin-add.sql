-- Start add serveradmin
SET IDENTITY_INSERT [dbo].[Users] ON
GO 
INSERT INTO [dbo].[Users]
           (UserID
           ,[OwnerID]
           ,[RoleID]
           ,[StatusID]
           ,[IsDemo]
           ,[IsPeer]
           ,[Username]
           ,[Password]
           ,[FirstName]
           ,[LastName]
           ,[Email]
           ,[Created]
           ,[Changed]
           ,[Comments]
           ,[SecondaryEmail]
           ,[Address]
           ,[City]
           ,[State]
           ,[Country]
           ,[Zip]
           ,[PrimaryPhone]
           ,[SecondaryPhone]
           ,[Fax]
           ,[InstantMessenger]
           ,[HtmlMail]
		   ,[EcommerceEnabled])
     VALUES
           (1,
           NULL,
           1,
           1,
           0,
           0
           ,'serveradmin'
           ,''
           ,'Enterprise'
           ,'Administrator'
           ,'serveradmin@myhosting.com'
           ,GETDATE()
           ,GETDATE()
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,1
	       ,1)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO

-- system package
INSERT INTO [dbo].[Packages]
           ([ParentPackageID]
           ,[UserID]
           ,[PackageName]
           ,[PackageComments]
           ,[ServerID]
           ,[StatusID]
           ,[PlanID]
           ,[PurchaseDate]
           ,[OverrideQuotas])
     VALUES
           (null
           ,1
           ,'System'
           ,''
           ,null
           ,1
           ,null
           ,GETDATE()
           ,0)
GO

INSERT INTO [dbo].[PackagesTreeCache] (ParentPackageID, PackageID)
VALUES (1,1)
GO

-- scheduled tasks
INSERT INTO [dbo].[Schedule]
           ([TaskID]
           ,[PackageID]
           ,[ScheduleName]
           ,[ScheduleTypeID]
           ,[Interval]
           ,[FromTime]
           ,[ToTime]
           ,[StartTime]
           ,[LastRun]
           ,[NextRun]
           ,[Enabled]
           ,[PriorityID]
           ,[HistoriesNumber]
           ,[MaxExecutionTime]
           ,[WeekMonthDay])
     VALUES
           ('SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE'
           ,1
           ,'Calculate Disk Space'
           ,'Daily'
           ,0
           ,CONVERT(datetime, '1/1/2000 12:00:00 PM')
           ,CONVERT(datetime, '1/1/2000 12:00:00 PM')
           ,CONVERT(datetime, '1/1/2000 12:30:00 PM')
           ,NULL
           ,DATEADD(Hour, 2, GETDATE())
           ,1
           ,'Normal'
           ,7
           ,3600
           ,1)
GO
INSERT INTO [dbo].[ScheduleParameters] (ScheduleID, ParameterID, ParameterValue)
VALUES(1, 'SUSPEND_OVERUSED', 'false')
GO

INSERT INTO [dbo].[Schedule]
           ([TaskID]
           ,[PackageID]
           ,[ScheduleName]
           ,[ScheduleTypeID]
           ,[Interval]
           ,[FromTime]
           ,[ToTime]
           ,[StartTime]
           ,[LastRun]
           ,[NextRun]
           ,[Enabled]
           ,[PriorityID]
           ,[HistoriesNumber]
           ,[MaxExecutionTime]
           ,[WeekMonthDay])
     VALUES
           ('SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH'
           ,1
           ,'Calculate Bandwidth'
           ,'Daily'
           ,0
           ,CONVERT(datetime, '1/1/2000 12:00:00 PM')
           ,CONVERT(datetime, '1/1/2000 12:00:00 PM')
           ,CONVERT(datetime, '1/1/2000 12:00:00 PM')
           ,NULL
           ,DATEADD(Hour, 2, GETDATE())
           ,1
           ,'Normal'
           ,7
           ,3600
           ,1)
GO
INSERT INTO [dbo].[ScheduleParameters] (ScheduleID, ParameterID, ParameterValue)
VALUES(2, 'SUSPEND_OVERUSED', 'false')
GO
-- End add serveradmin
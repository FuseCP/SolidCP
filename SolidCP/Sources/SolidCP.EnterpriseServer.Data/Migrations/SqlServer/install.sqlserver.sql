IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [AdditionalGroups] (
        [ID] int NOT NULL IDENTITY,
        [UserID] int NOT NULL,
        [GroupName] nvarchar(255) NULL,
        CONSTRAINT [PK__Addition__3214EC27E665DDE2] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditLog] (
        [RecordID] varchar(32) NOT NULL,
        [UserID] int NULL,
        [Username] nvarchar(50) NULL,
        [ItemID] int NULL,
        [SeverityID] int NOT NULL,
        [StartDate] datetime NOT NULL,
        [FinishDate] datetime NOT NULL,
        [SourceName] varchar(50) NOT NULL,
        [TaskName] varchar(50) NOT NULL,
        [ItemName] nvarchar(100) NULL,
        [ExecutionLog] ntext NULL,
        [PackageID] int NULL,
        CONSTRAINT [PK_Log] PRIMARY KEY ([RecordID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditLogSources] (
        [SourceName] varchar(100) NOT NULL,
        CONSTRAINT [PK_AuditLogSources] PRIMARY KEY ([SourceName])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [AuditLogTasks] (
        [SourceName] varchar(100) NOT NULL,
        [TaskName] varchar(100) NOT NULL,
        [TaskDescription] nvarchar(100) NULL,
        CONSTRAINT [PK_LogActions] PRIMARY KEY ([SourceName], [TaskName])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [BackgroundTasks] (
        [ID] int NOT NULL IDENTITY,
        [Guid] uniqueidentifier NOT NULL,
        [TaskID] nvarchar(255) NULL,
        [ScheduleID] int NOT NULL,
        [PackageID] int NOT NULL,
        [UserID] int NOT NULL,
        [EffectiveUserID] int NOT NULL,
        [TaskName] nvarchar(255) NULL,
        [ItemID] int NULL,
        [ItemName] nvarchar(255) NULL,
        [StartDate] datetime NOT NULL,
        [FinishDate] datetime NULL,
        [IndicatorCurrent] int NOT NULL,
        [IndicatorMaximum] int NOT NULL,
        [MaximumExecutionTime] int NOT NULL,
        [Source] nvarchar(max) NULL,
        [Severity] int NOT NULL,
        [Completed] bit NULL,
        [NotifyOnComplete] bit NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK__Backgrou__3214EC273A1145AC] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Clusters] (
        [ClusterID] int NOT NULL IDENTITY,
        [ClusterName] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Clusters] PRIMARY KEY ([ClusterID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeDeletedAccounts] (
        [ID] int NOT NULL IDENTITY,
        [AccountID] int NOT NULL,
        [OriginAT] int NOT NULL,
        [StoragePath] nvarchar(255) NULL,
        [FolderName] nvarchar(128) NULL,
        [FileName] nvarchar(128) NULL,
        [ExpirationDate] datetime NOT NULL,
        CONSTRAINT [PK__Exchange__3214EC27EF1C22C1] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeDisclaimers] (
        [ExchangeDisclaimerId] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [DisclaimerName] nvarchar(300) NOT NULL,
        [DisclaimerText] nvarchar(max) NULL,
        CONSTRAINT [PK_ExchangeDisclaimers] PRIMARY KEY ([ExchangeDisclaimerId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeMailboxPlanRetentionPolicyTags] (
        [PlanTagID] int NOT NULL IDENTITY,
        [TagID] int NOT NULL,
        [MailboxPlanId] int NOT NULL,
        CONSTRAINT [PK__Exchange__E467073C50CD805B] PRIMARY KEY ([PlanTagID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeRetentionPolicyTags] (
        [TagID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [TagName] nvarchar(255) NULL,
        [TagType] int NOT NULL,
        [AgeLimitForRetention] int NOT NULL,
        [RetentionAction] int NOT NULL,
        CONSTRAINT [PK__Exchange__657CFA4C02667D37] PRIMARY KEY ([TagID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [OCSUsers] (
        [OCSUserID] int NOT NULL IDENTITY,
        [AccountID] int NOT NULL,
        [InstanceID] nvarchar(50) NOT NULL,
        [CreatedDate] datetime NOT NULL DEFAULT ((getdate())),
        [ModifiedDate] datetime NOT NULL DEFAULT ((getdate())),
        CONSTRAINT [PK_OCSUsers] PRIMARY KEY ([OCSUserID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageSettings] (
        [PackageID] int NOT NULL,
        [SettingsName] nvarchar(50) NOT NULL,
        [PropertyName] nvarchar(50) NOT NULL,
        [PropertyValue] ntext NULL,
        CONSTRAINT [PK_PackageSettings] PRIMARY KEY ([PackageID], [SettingsName], [PropertyName])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [RDSCertificates] (
        [ID] int NOT NULL IDENTITY,
        [ServiceId] int NOT NULL,
        [Content] ntext NOT NULL,
        [Hash] nvarchar(255) NOT NULL,
        [FileName] nvarchar(255) NOT NULL,
        [ValidFrom] datetime NULL,
        [ExpiryDate] datetime NULL,
        CONSTRAINT [PK_RDSCertificates] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [RDSCollections] (
        [ID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [Name] nvarchar(255) NULL,
        [Description] nvarchar(255) NULL,
        [DisplayName] nvarchar(255) NULL,
        CONSTRAINT [PK__RDSColle__3214EC27346D361D] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [RDSServerSettings] (
        [RdsServerId] int NOT NULL,
        [SettingsName] nvarchar(50) NOT NULL,
        [PropertyName] nvarchar(50) NOT NULL,
        [PropertyValue] ntext NULL,
        [ApplyUsers] bit NOT NULL,
        [ApplyAdministrators] bit NOT NULL,
        CONSTRAINT [PK_RDSServerSettings] PRIMARY KEY ([RdsServerId], [SettingsName], [PropertyName])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ResourceGroups] (
        [GroupID] int NOT NULL,
        [GroupName] nvarchar(100) NOT NULL,
        [GroupOrder] int NOT NULL DEFAULT 1,
        [GroupController] nvarchar(1000) NULL,
        [ShowGroup] bit NULL,
        CONSTRAINT [PK_ResourceGroups] PRIMARY KEY ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ScheduleTasks] (
        [TaskID] nvarchar(100) NOT NULL,
        [TaskType] nvarchar(500) NOT NULL,
        [RoleID] int NOT NULL,
        CONSTRAINT [PK_ScheduleTasks] PRIMARY KEY ([TaskID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [SfBUserPlans] (
        [SfBUserPlanId] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [SfBUserPlanName] nvarchar(300) NOT NULL,
        [SfBUserPlanType] int NULL,
        [IM] bit NOT NULL,
        [Mobility] bit NOT NULL,
        [MobilityEnableOutsideVoice] bit NOT NULL,
        [Federation] bit NOT NULL,
        [Conferencing] bit NOT NULL,
        [EnterpriseVoice] bit NOT NULL,
        [VoicePolicy] int NOT NULL,
        [IsDefault] bit NOT NULL,
        [RemoteUserAccess] bit NOT NULL DEFAULT CAST(0 AS bit),
        [PublicIMConnectivity] bit NOT NULL DEFAULT CAST(0 AS bit),
        [AllowOrganizeMeetingsWithExternalAnonymous] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Telephony] int NULL,
        [ServerURI] nvarchar(300) NULL,
        [ArchivePolicy] nvarchar(300) NULL,
        [TelephonyDialPlanPolicy] nvarchar(300) NULL,
        [TelephonyVoicePolicy] nvarchar(300) NULL,
        CONSTRAINT [PK_SfBUserPlans] PRIMARY KEY ([SfBUserPlanId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [SfBUsers] (
        [SfBUserID] int NOT NULL IDENTITY,
        [AccountID] int NOT NULL,
        [SfBUserPlanID] int NOT NULL,
        [CreatedDate] datetime NOT NULL,
        [ModifiedDate] datetime NOT NULL,
        [SipAddress] nvarchar(300) NULL,
        CONSTRAINT [PK_SfBUsers] PRIMARY KEY ([SfBUserID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [SSLCertificates] (
        [ID] int NOT NULL IDENTITY,
        [UserID] int NOT NULL,
        [SiteID] int NOT NULL,
        [FriendlyName] nvarchar(255) NULL,
        [Hostname] nvarchar(255) NULL,
        [DistinguishedName] nvarchar(500) NULL,
        [CSR] ntext NULL,
        [CSRLength] int NULL,
        [Certificate] ntext NULL,
        [Hash] ntext NULL,
        [Installed] bit NULL,
        [IsRenewal] bit NULL,
        [ValidFrom] datetime NULL,
        [ExpiryDate] datetime NULL,
        [SerialNumber] nvarchar(250) NULL,
        [Pfx] ntext NULL,
        [PreviousId] int NULL,
        CONSTRAINT [PK_SSLCertificates] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [StorageSpaceLevels] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(300) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK__StorageS__3214EC07B8D82363] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [SupportServiceLevels] (
        [LevelID] int NOT NULL IDENTITY,
        [LevelName] nvarchar(100) NOT NULL,
        [LevelDescription] nvarchar(1000) NULL,
        CONSTRAINT [PK__SupportS__09F03C065BA08AFB] PRIMARY KEY ([LevelID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [SystemSettings] (
        [SettingsName] nvarchar(50) NOT NULL,
        [PropertyName] nvarchar(50) NOT NULL,
        [PropertyValue] ntext NULL,
        CONSTRAINT [PK_SystemSettings] PRIMARY KEY ([SettingsName], [PropertyName])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Themes] (
        [ThemeID] int NOT NULL IDENTITY,
        [DisplayName] nvarchar(255) NULL,
        [LTRName] nvarchar(255) NULL,
        [RTLName] nvarchar(255) NULL,
        [Enabled] int NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_Themes] PRIMARY KEY ([ThemeID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ThemeSettings] (
        [ThemeSettingID] int NOT NULL IDENTITY,
        [ThemeID] int NOT NULL,
        [SettingsName] nvarchar(255) NOT NULL,
        [PropertyName] nvarchar(255) NOT NULL,
        [PropertyValue] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_ThemeSettings] PRIMARY KEY ([ThemeSettingID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [UserID] int NOT NULL IDENTITY,
        [OwnerID] int NULL,
        [RoleID] int NOT NULL,
        [StatusID] int NOT NULL,
        [IsDemo] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsPeer] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Username] nvarchar(50) NULL,
        [Password] nvarchar(200) NULL,
        [FirstName] nvarchar(50) NULL,
        [LastName] nvarchar(50) NULL,
        [Email] nvarchar(255) NULL,
        [Created] datetime NULL,
        [Changed] datetime NULL,
        [Comments] ntext NULL,
        [SecondaryEmail] nvarchar(255) NULL,
        [Address] nvarchar(200) NULL,
        [City] nvarchar(50) NULL,
        [State] nvarchar(50) NULL,
        [Country] nvarchar(50) NULL,
        [Zip] varchar(20) NULL,
        [PrimaryPhone] varchar(30) NULL,
        [SecondaryPhone] varchar(30) NULL,
        [Fax] varchar(30) NULL,
        [InstantMessenger] varchar(100) NULL,
        [HtmlMail] bit NULL DEFAULT CAST(1 AS bit),
        [CompanyName] nvarchar(100) NULL,
        [EcommerceEnabled] bit NULL,
        [AdditionalParams] nvarchar(max) NULL,
        [LoginStatusId] int NULL,
        [FailedLogins] int NULL,
        [SubscriberNumber] nvarchar(32) NULL,
        [OneTimePasswordState] int NULL,
        [MfaMode] int NOT NULL DEFAULT 0,
        [PinSecret] nvarchar(255) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserID]),
        CONSTRAINT [FK_Users_Users] FOREIGN KEY ([OwnerID]) REFERENCES [Users] ([UserID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Versions] (
        [DatabaseVersion] varchar(50) NOT NULL,
        [BuildDate] datetime NOT NULL,
        CONSTRAINT [PK_Versions] PRIMARY KEY ([DatabaseVersion])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [BackgroundTaskLogs] (
        [LogID] int NOT NULL IDENTITY,
        [TaskID] int NOT NULL,
        [Date] datetime NULL,
        [ExceptionStackTrace] ntext NULL,
        [InnerTaskStart] int NULL,
        [Severity] int NULL,
        [Text] ntext NULL,
        [TextIdent] int NULL,
        [XmlParameters] ntext NULL,
        CONSTRAINT [PK__Backgrou__5E5499A86067A6E5] PRIMARY KEY ([LogID]),
        CONSTRAINT [FK__Backgroun__TaskI__7D8391DF] FOREIGN KEY ([TaskID]) REFERENCES [BackgroundTasks] ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [BackgroundTaskParameters] (
        [ParameterID] int NOT NULL IDENTITY,
        [TaskID] int NOT NULL,
        [Name] nvarchar(255) NULL,
        [SerializerValue] ntext NULL,
        [TypeName] nvarchar(255) NULL,
        CONSTRAINT [PK__Backgrou__F80C629777BF580B] PRIMARY KEY ([ParameterID]),
        CONSTRAINT [FK__Backgroun__TaskI__7AA72534] FOREIGN KEY ([TaskID]) REFERENCES [BackgroundTasks] ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [BackgroundTaskStack] (
        [TaskStackID] int NOT NULL IDENTITY,
        [TaskID] int NOT NULL,
        CONSTRAINT [PK__Backgrou__5E44466FB8A5F217] PRIMARY KEY ([TaskStackID]),
        CONSTRAINT [FK__Backgroun__TaskI__005FFE8A] FOREIGN KEY ([TaskID]) REFERENCES [BackgroundTasks] ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [RDSCollectionSettings] (
        [ID] int NOT NULL IDENTITY,
        [RDSCollectionId] int NOT NULL,
        [DisconnectedSessionLimitMin] int NULL,
        [ActiveSessionLimitMin] int NULL,
        [IdleSessionLimitMin] int NULL,
        [BrokenConnectionAction] nvarchar(20) NULL,
        [AutomaticReconnectionEnabled] bit NULL,
        [TemporaryFoldersDeletedOnExit] bit NULL,
        [TemporaryFoldersPerSession] bit NULL,
        [ClientDeviceRedirectionOptions] nvarchar(250) NULL,
        [ClientPrinterRedirected] bit NULL,
        [ClientPrinterAsDefault] bit NULL,
        [RDEasyPrintDriverEnabled] bit NULL,
        [MaxRedirectedMonitors] int NULL,
        [SecurityLayer] nvarchar(20) NULL,
        [EncryptionLevel] nvarchar(20) NULL,
        [AuthenticateUsingNLA] bit NULL,
        CONSTRAINT [PK_RDSCollectionSettings] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RDSCollectionSettings_RDSCollections] FOREIGN KEY ([RDSCollectionId]) REFERENCES [RDSCollections] ([ID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [RDSMessages] (
        [Id] int NOT NULL IDENTITY,
        [RDSCollectionId] int NOT NULL,
        [MessageText] ntext NOT NULL,
        [UserName] nchar(250) NOT NULL,
        [Date] datetime NOT NULL,
        CONSTRAINT [PK_RDSMessages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RDSMessages_RDSCollections] FOREIGN KEY ([RDSCollectionId]) REFERENCES [RDSCollections] ([ID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [RDSServers] (
        [ID] int NOT NULL IDENTITY,
        [ItemID] int NULL,
        [Name] nvarchar(255) NULL,
        [FqdName] nvarchar(255) NULL,
        [Description] nvarchar(255) NULL,
        [RDSCollectionId] int NULL,
        [ConnectionEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
        [Controller] int NULL,
        CONSTRAINT [PK__RDSServe__3214EC27DBEBD4B5] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RDSServers_RDSCollectionId] FOREIGN KEY ([RDSCollectionId]) REFERENCES [RDSCollections] ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Providers] (
        [ProviderID] int NOT NULL,
        [GroupID] int NOT NULL,
        [ProviderName] nvarchar(100) NULL,
        [DisplayName] nvarchar(200) NOT NULL,
        [ProviderType] nvarchar(400) NULL,
        [EditorControl] nvarchar(100) NULL,
        [DisableAutoDiscovery] bit NULL,
        CONSTRAINT [PK_ServiceTypes] PRIMARY KEY ([ProviderID]),
        CONSTRAINT [FK_Providers_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ResourceGroupDnsRecords] (
        [RecordID] int NOT NULL IDENTITY,
        [RecordOrder] int NOT NULL DEFAULT 1,
        [GroupID] int NOT NULL,
        [RecordType] varchar(50) NOT NULL,
        [RecordName] nvarchar(50) NOT NULL,
        [RecordData] nvarchar(200) NOT NULL,
        [MXPriority] int NULL,
        CONSTRAINT [PK_ResourceGroupDnsRecords] PRIMARY KEY ([RecordID]),
        CONSTRAINT [FK_ResourceGroupDnsRecords_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Servers] (
        [ServerID] int NOT NULL IDENTITY,
        [ServerName] nvarchar(100) NOT NULL,
        [ServerUrl] nvarchar(255) NULL DEFAULT N'',
        [Password] nvarchar(100) NULL,
        [Comments] ntext NULL,
        [VirtualServer] bit NOT NULL DEFAULT CAST(0 AS bit),
        [InstantDomainAlias] nvarchar(200) NULL,
        [PrimaryGroupID] int NULL,
        [ADRootDomain] nvarchar(200) NULL,
        [ADUsername] nvarchar(100) NULL,
        [ADPassword] nvarchar(100) NULL,
        [ADAuthenticationType] varchar(50) NULL,
        [ADEnabled] bit NULL DEFAULT CAST(0 AS bit),
        [ADParentDomain] nvarchar(200) NULL,
        [ADParentDomainController] nvarchar(200) NULL,
        [OSPlatform] int NOT NULL DEFAULT 0,
        [IsCore] bit NULL,
        [PasswordIsSHA256] bit NOT NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK_Servers] PRIMARY KEY ([ServerID]),
        CONSTRAINT [FK_Servers_ResourceGroups] FOREIGN KEY ([PrimaryGroupID]) REFERENCES [ResourceGroups] ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ServiceItemTypes] (
        [ItemTypeID] int NOT NULL,
        [GroupID] int NULL,
        [DisplayName] nvarchar(50) NULL,
        [TypeName] nvarchar(200) NULL,
        [TypeOrder] int NOT NULL DEFAULT 1,
        [CalculateDiskspace] bit NULL,
        [CalculateBandwidth] bit NULL,
        [Suspendable] bit NULL,
        [Disposable] bit NULL,
        [Searchable] bit NULL,
        [Importable] bit NOT NULL DEFAULT CAST(1 AS bit),
        [Backupable] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_ServiceItemTypes] PRIMARY KEY ([ItemTypeID]),
        CONSTRAINT [FK_ServiceItemTypes_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ScheduleTaskParameters] (
        [TaskID] nvarchar(100) NOT NULL,
        [ParameterID] nvarchar(100) NOT NULL,
        [DataTypeID] nvarchar(50) NOT NULL,
        [DefaultValue] nvarchar(max) NULL,
        [ParameterOrder] int NOT NULL DEFAULT 0,
        CONSTRAINT [PK_ScheduleTaskParameters] PRIMARY KEY ([TaskID], [ParameterID]),
        CONSTRAINT [FK_ScheduleTaskParameters_ScheduleTasks] FOREIGN KEY ([TaskID]) REFERENCES [ScheduleTasks] ([TaskID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ScheduleTaskViewConfiguration] (
        [TaskID] nvarchar(100) NOT NULL,
        [ConfigurationID] nvarchar(100) NOT NULL,
        [Environment] nvarchar(100) NOT NULL,
        [Description] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_ScheduleTaskViewConfiguration] PRIMARY KEY ([ConfigurationID], [TaskID]),
        CONSTRAINT [FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration] FOREIGN KEY ([TaskID]) REFERENCES [ScheduleTasks] ([TaskID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [StorageSpaceLevelResourceGroups] (
        [Id] int NOT NULL IDENTITY,
        [LevelId] int NOT NULL,
        [GroupId] int NOT NULL,
        CONSTRAINT [PK__StorageS__3214EC07EBEBED98] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StorageSpaceLevelResourceGroups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [ResourceGroups] ([GroupID]) ON DELETE CASCADE,
        CONSTRAINT [FK_StorageSpaceLevelResourceGroups_LevelId] FOREIGN KEY ([LevelId]) REFERENCES [StorageSpaceLevels] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Comments] (
        [CommentID] int NOT NULL IDENTITY,
        [ItemTypeID] varchar(50) NOT NULL,
        [ItemID] int NOT NULL,
        [UserID] int NOT NULL,
        [CreatedDate] datetime NOT NULL DEFAULT ((getdate())),
        [CommentText] nvarchar(1000) NULL,
        [SeverityID] int NULL,
        CONSTRAINT [PK_Comments] PRIMARY KEY ([CommentID]),
        CONSTRAINT [FK_Comments_Users] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [UserSettings] (
        [UserID] int NOT NULL,
        [SettingsName] nvarchar(50) NOT NULL,
        [PropertyName] nvarchar(50) NOT NULL,
        [PropertyValue] ntext NULL,
        CONSTRAINT [PK_UserSettings] PRIMARY KEY ([UserID], [SettingsName], [PropertyName]),
        CONSTRAINT [FK_UserSettings_Users] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ServiceDefaultProperties] (
        [ProviderID] int NOT NULL,
        [PropertyName] nvarchar(50) NOT NULL,
        [PropertyValue] nvarchar(1000) NULL,
        CONSTRAINT [PK_ServiceDefaultProperties_1] PRIMARY KEY ([ProviderID], [PropertyName]),
        CONSTRAINT [FK_ServiceDefaultProperties_Providers] FOREIGN KEY ([ProviderID]) REFERENCES [Providers] ([ProviderID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [HostingPlans] (
        [PlanID] int NOT NULL IDENTITY,
        [UserID] int NULL,
        [PackageID] int NULL,
        [ServerID] int NULL,
        [PlanName] nvarchar(200) NOT NULL,
        [PlanDescription] ntext NULL,
        [Available] bit NOT NULL,
        [SetupPrice] money NULL,
        [RecurringPrice] money NULL,
        [RecurrenceUnit] int NULL,
        [RecurrenceLength] int NULL,
        [IsAddon] bit NULL,
        CONSTRAINT [PK_HostingPlans] PRIMARY KEY ([PlanID]),
        CONSTRAINT [FK_HostingPlans_Servers] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID]),
        CONSTRAINT [FK_HostingPlans_Users] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [IPAddresses] (
        [AddressID] int NOT NULL IDENTITY,
        [ExternalIP] varchar(24) NOT NULL,
        [InternalIP] varchar(24) NULL,
        [ServerID] int NULL,
        [Comments] ntext NULL,
        [SubnetMask] varchar(15) NULL,
        [DefaultGateway] varchar(15) NULL,
        [PoolID] int NULL,
        [VLAN] int NULL,
        CONSTRAINT [PK_IPAddresses] PRIMARY KEY ([AddressID]),
        CONSTRAINT [FK_IPAddresses_Servers] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PrivateNetworkVLANs] (
        [VlanID] int NOT NULL IDENTITY,
        [Vlan] int NOT NULL,
        [ServerID] int NULL,
        [Comments] ntext NULL,
        CONSTRAINT [PK__PrivateN__8348135581B53618] PRIMARY KEY ([VlanID]),
        CONSTRAINT [FK_ServerID] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Services] (
        [ServiceID] int NOT NULL IDENTITY,
        [ServerID] int NOT NULL,
        [ProviderID] int NOT NULL,
        [ServiceName] nvarchar(50) NOT NULL,
        [Comments] ntext NULL,
        [ServiceQuotaValue] int NULL,
        [ClusterID] int NULL,
        CONSTRAINT [PK_Services] PRIMARY KEY ([ServiceID]),
        CONSTRAINT [FK_Services_Clusters] FOREIGN KEY ([ClusterID]) REFERENCES [Clusters] ([ClusterID]),
        CONSTRAINT [FK_Services_Providers] FOREIGN KEY ([ProviderID]) REFERENCES [Providers] ([ProviderID]),
        CONSTRAINT [FK_Services_Servers] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [VirtualGroups] (
        [VirtualGroupID] int NOT NULL IDENTITY,
        [ServerID] int NOT NULL,
        [GroupID] int NOT NULL,
        [DistributionType] int NULL,
        [BindDistributionToPrimary] bit NULL,
        CONSTRAINT [PK_VirtualGroups] PRIMARY KEY ([VirtualGroupID]),
        CONSTRAINT [FK_VirtualGroups_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID]),
        CONSTRAINT [FK_VirtualGroups_Servers] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Quotas] (
        [QuotaID] int NOT NULL,
        [GroupID] int NOT NULL,
        [QuotaOrder] int NOT NULL DEFAULT 1,
        [QuotaName] nvarchar(50) NOT NULL,
        [QuotaDescription] nvarchar(200) NULL,
        [QuotaTypeID] int NOT NULL DEFAULT 2,
        [ServiceQuota] bit NULL DEFAULT CAST(0 AS bit),
        [ItemTypeID] int NULL,
        [HideQuota] bit NULL,
        [PerOrganization] int NULL,
        CONSTRAINT [PK_Quotas] PRIMARY KEY ([QuotaID]),
        CONSTRAINT [FK_Quotas_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Quotas_ServiceItemTypes] FOREIGN KEY ([ItemTypeID]) REFERENCES [ServiceItemTypes] ([ItemTypeID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [HostingPlanResources] (
        [PlanID] int NOT NULL,
        [GroupID] int NOT NULL,
        [CalculateDiskSpace] bit NULL,
        [CalculateBandwidth] bit NULL,
        CONSTRAINT [PK_HostingPlanResources] PRIMARY KEY ([PlanID], [GroupID]),
        CONSTRAINT [FK_HostingPlanResources_HostingPlans] FOREIGN KEY ([PlanID]) REFERENCES [HostingPlans] ([PlanID]) ON DELETE CASCADE,
        CONSTRAINT [FK_HostingPlanResources_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Packages] (
        [PackageID] int NOT NULL IDENTITY,
        [ParentPackageID] int NULL,
        [UserID] int NOT NULL,
        [PackageName] nvarchar(300) NULL,
        [PackageComments] ntext NULL,
        [ServerID] int NULL,
        [StatusID] int NOT NULL,
        [PlanID] int NULL,
        [PurchaseDate] datetime NULL,
        [OverrideQuotas] bit NOT NULL DEFAULT CAST(0 AS bit),
        [BandwidthUpdated] datetime NULL,
        [DefaultTopPackage] bit NOT NULL DEFAULT CAST(0 AS bit),
        [StatusIDchangeDate] datetime NOT NULL DEFAULT ((getdate())),
        CONSTRAINT [PK_Packages] PRIMARY KEY ([PackageID]),
        CONSTRAINT [FK_Packages_HostingPlans] FOREIGN KEY ([PlanID]) REFERENCES [HostingPlans] ([PlanID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Packages_Packages] FOREIGN KEY ([ParentPackageID]) REFERENCES [Packages] ([PackageID]),
        CONSTRAINT [FK_Packages_Servers] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID]),
        CONSTRAINT [FK_Packages_Users] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ServiceProperties] (
        [ServiceID] int NOT NULL,
        [PropertyName] nvarchar(50) NOT NULL,
        [PropertyValue] nvarchar(max) NULL,
        CONSTRAINT [PK_ServiceProperties_1] PRIMARY KEY ([ServiceID], [PropertyName]),
        CONSTRAINT [FK_ServiceProperties_Services] FOREIGN KEY ([ServiceID]) REFERENCES [Services] ([ServiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [StorageSpaces] (
        [Id] int NOT NULL IDENTITY,
        [Name] varchar(300) NOT NULL,
        [ServiceId] int NOT NULL,
        [ServerId] int NOT NULL,
        [LevelId] int NOT NULL,
        [Path] varchar(max) NOT NULL,
        [IsShared] bit NOT NULL,
        [UncPath] varchar(max) NULL,
        [FsrmQuotaType] int NOT NULL,
        [FsrmQuotaSizeBytes] bigint NOT NULL,
        [IsDisabled] bit NOT NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK__StorageS__3214EC07B8B9A6D1] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StorageSpaces_ServerId] FOREIGN KEY ([ServerId]) REFERENCES [Servers] ([ServerID]) ON DELETE CASCADE,
        CONSTRAINT [FK_StorageSpaces_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [Services] ([ServiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [VirtualServices] (
        [VirtualServiceID] int NOT NULL IDENTITY,
        [ServerID] int NOT NULL,
        [ServiceID] int NOT NULL,
        CONSTRAINT [PK_VirtualServices] PRIMARY KEY ([VirtualServiceID]),
        CONSTRAINT [FK_VirtualServices_Servers] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID]) ON DELETE CASCADE,
        CONSTRAINT [FK_VirtualServices_Services] FOREIGN KEY ([ServiceID]) REFERENCES [Services] ([ServiceID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [HostingPlanQuotas] (
        [PlanID] int NOT NULL,
        [QuotaID] int NOT NULL,
        [QuotaValue] int NOT NULL,
        CONSTRAINT [PK_HostingPlanQuotas_1] PRIMARY KEY ([PlanID], [QuotaID]),
        CONSTRAINT [FK_HostingPlanQuotas_HostingPlans] FOREIGN KEY ([PlanID]) REFERENCES [HostingPlans] ([PlanID]) ON DELETE CASCADE,
        CONSTRAINT [FK_HostingPlanQuotas_Quotas] FOREIGN KEY ([QuotaID]) REFERENCES [Quotas] ([QuotaID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [GlobalDnsRecords] (
        [RecordID] int NOT NULL IDENTITY,
        [RecordType] varchar(10) NOT NULL,
        [RecordName] nvarchar(50) NOT NULL,
        [RecordData] nvarchar(500) NOT NULL,
        [MXPriority] int NOT NULL,
        [ServiceID] int NULL,
        [ServerID] int NULL,
        [PackageID] int NULL,
        [IPAddressID] int NULL,
        [SrvPriority] int NULL,
        [SrvWeight] int NULL,
        [SrvPort] int NULL,
        CONSTRAINT [PK_GlobalDnsRecords] PRIMARY KEY ([RecordID]),
        CONSTRAINT [FK_GlobalDnsRecords_IPAddresses] FOREIGN KEY ([IPAddressID]) REFERENCES [IPAddresses] ([AddressID]),
        CONSTRAINT [FK_GlobalDnsRecords_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]) ON DELETE CASCADE,
        CONSTRAINT [FK_GlobalDnsRecords_Servers] FOREIGN KEY ([ServerID]) REFERENCES [Servers] ([ServerID]),
        CONSTRAINT [FK_GlobalDnsRecords_Services] FOREIGN KEY ([ServiceID]) REFERENCES [Services] ([ServiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageAddons] (
        [PackageAddonID] int NOT NULL IDENTITY,
        [PackageID] int NULL,
        [PlanID] int NULL,
        [Quantity] int NULL,
        [PurchaseDate] datetime NULL,
        [Comments] ntext NULL,
        [StatusID] int NULL,
        CONSTRAINT [PK_PackageAddons] PRIMARY KEY ([PackageAddonID]),
        CONSTRAINT [FK_PackageAddons_HostingPlans] FOREIGN KEY ([PlanID]) REFERENCES [HostingPlans] ([PlanID]),
        CONSTRAINT [FK_PackageAddons_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageQuotas] (
        [PackageID] int NOT NULL,
        [QuotaID] int NOT NULL,
        [QuotaValue] int NOT NULL,
        CONSTRAINT [PK_PackageQuotas] PRIMARY KEY ([PackageID], [QuotaID]),
        CONSTRAINT [FK_PackageQuotas_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]),
        CONSTRAINT [FK_PackageQuotas_Quotas] FOREIGN KEY ([QuotaID]) REFERENCES [Quotas] ([QuotaID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageResources] (
        [PackageID] int NOT NULL,
        [GroupID] int NOT NULL,
        [CalculateDiskspace] bit NOT NULL,
        [CalculateBandwidth] bit NOT NULL,
        CONSTRAINT [PK_PackageResources_1] PRIMARY KEY ([PackageID], [GroupID]),
        CONSTRAINT [FK_PackageResources_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]),
        CONSTRAINT [FK_PackageResources_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackagesBandwidth] (
        [PackageID] int NOT NULL,
        [GroupID] int NOT NULL,
        [LogDate] datetime NOT NULL,
        [BytesSent] bigint NOT NULL,
        [BytesReceived] bigint NOT NULL,
        CONSTRAINT [PK_PackagesBandwidth] PRIMARY KEY ([PackageID], [GroupID], [LogDate]),
        CONSTRAINT [FK_PackagesBandwidth_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]),
        CONSTRAINT [FK_PackagesBandwidth_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackagesDiskspace] (
        [PackageID] int NOT NULL,
        [GroupID] int NOT NULL,
        [DiskSpace] bigint NOT NULL,
        CONSTRAINT [PK_PackagesDiskspace] PRIMARY KEY ([PackageID], [GroupID]),
        CONSTRAINT [FK_PackagesDiskspace_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]),
        CONSTRAINT [FK_PackagesDiskspace_ResourceGroups] FOREIGN KEY ([GroupID]) REFERENCES [ResourceGroups] ([GroupID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageServices] (
        [PackageID] int NOT NULL,
        [ServiceID] int NOT NULL,
        CONSTRAINT [PK_PackageServices] PRIMARY KEY ([PackageID], [ServiceID]),
        CONSTRAINT [FK_PackageServices_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]) ON DELETE CASCADE,
        CONSTRAINT [FK_PackageServices_Services] FOREIGN KEY ([ServiceID]) REFERENCES [Services] ([ServiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackagesTreeCache] (
        [ParentPackageID] int NOT NULL,
        [PackageID] int NOT NULL,
        CONSTRAINT [PK_PackagesTreeCache] PRIMARY KEY ([ParentPackageID], [PackageID]),
        CONSTRAINT [FK_PackagesTreeCache_Packages] FOREIGN KEY ([ParentPackageID]) REFERENCES [Packages] ([PackageID]),
        CONSTRAINT [FK_PackagesTreeCache_Packages1] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageVLANs] (
        [PackageVlanID] int NOT NULL IDENTITY,
        [VlanID] int NOT NULL,
        [PackageID] int NOT NULL,
        [IsDmz] bit NOT NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK__PackageV__A9AABBF9C0C25CB3] PRIMARY KEY ([PackageVlanID]),
        CONSTRAINT [FK_PackageID] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]) ON DELETE CASCADE,
        CONSTRAINT [FK_VlanID] FOREIGN KEY ([VlanID]) REFERENCES [PrivateNetworkVLANs] ([VlanID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Schedule] (
        [ScheduleID] int NOT NULL IDENTITY,
        [TaskID] nvarchar(100) NOT NULL,
        [PackageID] int NULL,
        [ScheduleName] nvarchar(100) NULL,
        [ScheduleTypeID] nvarchar(50) NULL,
        [Interval] int NULL,
        [FromTime] datetime NULL,
        [ToTime] datetime NULL,
        [StartTime] datetime NULL,
        [LastRun] datetime NULL,
        [NextRun] datetime NULL,
        [Enabled] bit NOT NULL,
        [PriorityID] nvarchar(50) NULL,
        [HistoriesNumber] int NULL,
        [MaxExecutionTime] int NULL,
        [WeekMonthDay] int NULL,
        CONSTRAINT [PK_Schedule] PRIMARY KEY ([ScheduleID]),
        CONSTRAINT [FK_Schedule_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Schedule_ScheduleTasks] FOREIGN KEY ([TaskID]) REFERENCES [ScheduleTasks] ([TaskID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ServiceItems] (
        [ItemID] int NOT NULL IDENTITY,
        [PackageID] int NULL,
        [ItemTypeID] int NULL,
        [ServiceID] int NULL,
        [ItemName] nvarchar(500) NULL,
        [CreatedDate] datetime NULL,
        CONSTRAINT [PK_ServiceItems] PRIMARY KEY ([ItemID]),
        CONSTRAINT [FK_ServiceItems_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]),
        CONSTRAINT [FK_ServiceItems_ServiceItemTypes] FOREIGN KEY ([ItemTypeID]) REFERENCES [ServiceItemTypes] ([ItemTypeID]),
        CONSTRAINT [FK_ServiceItems_Services] FOREIGN KEY ([ServiceID]) REFERENCES [Services] ([ServiceID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [StorageSpaceFolders] (
        [Id] int NOT NULL IDENTITY,
        [Name] varchar(300) NOT NULL,
        [StorageSpaceId] int NOT NULL,
        [Path] varchar(max) NOT NULL,
        [UncPath] varchar(max) NULL,
        [IsShared] bit NOT NULL,
        [FsrmQuotaType] int NOT NULL,
        [FsrmQuotaSizeBytes] bigint NOT NULL,
        CONSTRAINT [PK__StorageS__3214EC07AC0C9EB6] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StorageSpaceFolders_StorageSpaceId] FOREIGN KEY ([StorageSpaceId]) REFERENCES [StorageSpaces] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ScheduleParameters] (
        [ScheduleID] int NOT NULL,
        [ParameterID] nvarchar(100) NOT NULL,
        [ParameterValue] nvarchar(1000) NULL,
        CONSTRAINT [PK_ScheduleParameters] PRIMARY KEY ([ScheduleID], [ParameterID]),
        CONSTRAINT [FK_ScheduleParameters_Schedule] FOREIGN KEY ([ScheduleID]) REFERENCES [Schedule] ([ScheduleID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [DmzIPAddresses] (
        [DmzAddressID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [IPAddress] varchar(15) NOT NULL,
        [IsPrimary] bit NOT NULL,
        CONSTRAINT [PK_DmzIPAddresses] PRIMARY KEY ([DmzAddressID]),
        CONSTRAINT [FK_DmzIPAddresses_ServiceItems] FOREIGN KEY ([ItemID]) REFERENCES [ServiceItems] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [Domains] (
        [DomainID] int NOT NULL IDENTITY,
        [PackageID] int NOT NULL,
        [ZoneItemID] int NULL,
        [DomainName] nvarchar(100) NOT NULL,
        [HostingAllowed] bit NOT NULL DEFAULT CAST(0 AS bit),
        [WebSiteID] int NULL,
        [MailDomainID] int NULL,
        [IsSubDomain] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsPreviewDomain] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsDomainPointer] bit NOT NULL,
        [DomainItemId] int NULL,
        [CreationDate] datetime NULL,
        [ExpirationDate] datetime NULL,
        [LastUpdateDate] datetime NULL,
        [RegistrarName] nvarchar(max) NULL,
        CONSTRAINT [PK_Domains] PRIMARY KEY ([DomainID]),
        CONSTRAINT [FK_Domains_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Domains_ServiceItems_MailDomain] FOREIGN KEY ([MailDomainID]) REFERENCES [ServiceItems] ([ItemID]),
        CONSTRAINT [FK_Domains_ServiceItems_WebSite] FOREIGN KEY ([WebSiteID]) REFERENCES [ServiceItems] ([ItemID]),
        CONSTRAINT [FK_Domains_ServiceItems_ZoneItem] FOREIGN KEY ([ZoneItemID]) REFERENCES [ServiceItems] ([ItemID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeOrganizationDomains] (
        [OrganizationDomainID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [DomainID] int NULL,
        [IsHost] bit NULL DEFAULT CAST(0 AS bit),
        [DomainTypeID] int NOT NULL DEFAULT 0,
        CONSTRAINT [PK_ExchangeOrganizationDomains] PRIMARY KEY ([OrganizationDomainID]),
        CONSTRAINT [FK_ExchangeOrganizationDomains_ServiceItems] FOREIGN KEY ([ItemID]) REFERENCES [ServiceItems] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeOrganizations] (
        [ItemID] int NOT NULL,
        [OrganizationID] nvarchar(128) NOT NULL,
        [ExchangeMailboxPlanID] int NULL,
        [LyncUserPlanID] int NULL,
        [SfBUserPlanID] int NULL,
        CONSTRAINT [PK_ExchangeOrganizations] PRIMARY KEY ([ItemID]),
        CONSTRAINT [FK_ExchangeOrganizations_ServiceItems] FOREIGN KEY ([ItemID]) REFERENCES [ServiceItems] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageIPAddresses] (
        [PackageAddressID] int NOT NULL IDENTITY,
        [PackageID] int NOT NULL,
        [AddressID] int NOT NULL,
        [ItemID] int NULL,
        [IsPrimary] bit NULL,
        [OrgID] int NULL,
        CONSTRAINT [PK_PackageIPAddresses] PRIMARY KEY ([PackageAddressID]),
        CONSTRAINT [FK_PackageIPAddresses_IPAddresses] FOREIGN KEY ([AddressID]) REFERENCES [IPAddresses] ([AddressID]),
        CONSTRAINT [FK_PackageIPAddresses_Packages] FOREIGN KEY ([PackageID]) REFERENCES [Packages] ([PackageID]) ON DELETE CASCADE,
        CONSTRAINT [FK_PackageIPAddresses_ServiceItems] FOREIGN KEY ([ItemID]) REFERENCES [ServiceItems] ([ItemID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [PrivateIPAddresses] (
        [PrivateAddressID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [IPAddress] varchar(15) NOT NULL,
        [IsPrimary] bit NOT NULL,
        CONSTRAINT [PK_PrivateIPAddresses] PRIMARY KEY ([PrivateAddressID]),
        CONSTRAINT [FK_PrivateIPAddresses_ServiceItems] FOREIGN KEY ([ItemID]) REFERENCES [ServiceItems] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ServiceItemProperties] (
        [ItemID] int NOT NULL,
        [PropertyName] nvarchar(50) NOT NULL,
        [PropertyValue] nvarchar(max) NULL,
        CONSTRAINT [PK_ServiceItemProperties] PRIMARY KEY ([ItemID], [PropertyName]),
        CONSTRAINT [FK_ServiceItemProperties_ServiceItems] FOREIGN KEY ([ItemID]) REFERENCES [ServiceItems] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [EnterpriseFolders] (
        [EnterpriseFolderID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [FolderName] nvarchar(255) NOT NULL,
        [FolderQuota] int NOT NULL DEFAULT 0,
        [LocationDrive] nvarchar(255) NULL,
        [HomeFolder] nvarchar(255) NULL,
        [Domain] nvarchar(255) NULL,
        [StorageSpaceFolderId] int NULL,
        CONSTRAINT [PK_EnterpriseFolders] PRIMARY KEY ([EnterpriseFolderID]),
        CONSTRAINT [FK_EnterpriseFolders_StorageSpaceFolderId] FOREIGN KEY ([StorageSpaceFolderId]) REFERENCES [StorageSpaceFolders] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [DomainDnsRecords] (
        [ID] int NOT NULL IDENTITY,
        [DomainId] int NOT NULL,
        [RecordType] int NOT NULL,
        [DnsServer] nvarchar(255) NULL,
        [Value] nvarchar(255) NULL,
        [Date] datetime NULL,
        CONSTRAINT [PK__DomainDn__3214EC27A6FC0498] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_DomainDnsRecords_DomainId] FOREIGN KEY ([DomainId]) REFERENCES [Domains] ([DomainID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeMailboxPlans] (
        [MailboxPlanId] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [MailboxPlan] nvarchar(300) NOT NULL,
        [MailboxPlanType] int NULL,
        [EnableActiveSync] bit NOT NULL,
        [EnableIMAP] bit NOT NULL,
        [EnableMAPI] bit NOT NULL,
        [EnableOWA] bit NOT NULL,
        [EnablePOP] bit NOT NULL,
        [IsDefault] bit NOT NULL,
        [IssueWarningPct] int NOT NULL,
        [KeepDeletedItemsDays] int NOT NULL,
        [MailboxSizeMB] int NOT NULL,
        [MaxReceiveMessageSizeKB] int NOT NULL,
        [MaxRecipients] int NOT NULL,
        [MaxSendMessageSizeKB] int NOT NULL,
        [ProhibitSendPct] int NOT NULL,
        [ProhibitSendReceivePct] int NOT NULL,
        [HideFromAddressBook] bit NOT NULL,
        [AllowLitigationHold] bit NULL,
        [RecoverableItemsWarningPct] int NULL,
        [RecoverableItemsSpace] int NULL,
        [LitigationHoldUrl] nvarchar(256) NULL,
        [LitigationHoldMsg] nvarchar(512) NULL,
        [Archiving] bit NULL,
        [EnableArchiving] bit NULL,
        [ArchiveSizeMB] int NULL,
        [ArchiveWarningPct] int NULL,
        [EnableAutoReply] bit NULL,
        [IsForJournaling] bit NULL,
        [EnableForceArchiveDeletion] bit NULL,
        CONSTRAINT [PK_ExchangeMailboxPlans] PRIMARY KEY ([MailboxPlanId]),
        CONSTRAINT [FK_ExchangeMailboxPlans_ExchangeOrganizations] FOREIGN KEY ([ItemID]) REFERENCES [ExchangeOrganizations] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeOrganizationSettings] (
        [ItemId] int NOT NULL,
        [SettingsName] nvarchar(100) NOT NULL,
        [Xml] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ExchangeOrganizationSettings] PRIMARY KEY ([ItemId], [SettingsName]),
        CONSTRAINT [FK_ExchangeOrganizationSettings_ExchangeOrganizations_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [ExchangeOrganizations] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeOrganizationSsFolders] (
        [Id] int NOT NULL IDENTITY,
        [ItemId] int NOT NULL,
        [Type] varchar(100) NOT NULL,
        [StorageSpaceFolderId] int NOT NULL,
        CONSTRAINT [PK__Exchange__3214EC072DDBA072] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ExchangeOrganizationSsFolders_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [ExchangeOrganizations] ([ItemID]) ON DELETE CASCADE,
        CONSTRAINT [FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId] FOREIGN KEY ([StorageSpaceFolderId]) REFERENCES [StorageSpaceFolders] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [LyncUserPlans] (
        [LyncUserPlanId] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [LyncUserPlanName] nvarchar(300) NOT NULL,
        [LyncUserPlanType] int NULL,
        [IM] bit NOT NULL,
        [Mobility] bit NOT NULL,
        [MobilityEnableOutsideVoice] bit NOT NULL,
        [Federation] bit NOT NULL,
        [Conferencing] bit NOT NULL,
        [EnterpriseVoice] bit NOT NULL,
        [VoicePolicy] int NOT NULL,
        [IsDefault] bit NOT NULL,
        [RemoteUserAccess] bit NOT NULL DEFAULT CAST(0 AS bit),
        [PublicIMConnectivity] bit NOT NULL DEFAULT CAST(0 AS bit),
        [AllowOrganizeMeetingsWithExternalAnonymous] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Telephony] int NULL,
        [ServerURI] nvarchar(300) NULL,
        [ArchivePolicy] nvarchar(300) NULL,
        [TelephonyDialPlanPolicy] nvarchar(300) NULL,
        [TelephonyVoicePolicy] nvarchar(300) NULL,
        CONSTRAINT [PK_LyncUserPlans] PRIMARY KEY ([LyncUserPlanId]),
        CONSTRAINT [FK_LyncUserPlans_ExchangeOrganizations] FOREIGN KEY ([ItemID]) REFERENCES [ExchangeOrganizations] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeAccounts] (
        [AccountID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [AccountType] int NOT NULL,
        [AccountName] nvarchar(300) NOT NULL,
        [DisplayName] nvarchar(300) NOT NULL,
        [PrimaryEmailAddress] nvarchar(300) NULL,
        [MailEnabledPublicFolder] bit NULL,
        [MailboxManagerActions] varchar(200) NULL,
        [SamAccountName] nvarchar(100) NULL,
        [CreatedDate] datetime NOT NULL DEFAULT ((getdate())),
        [MailboxPlanId] int NULL,
        [SubscriberNumber] nvarchar(32) NULL,
        [UserPrincipalName] nvarchar(300) NULL,
        [ExchangeDisclaimerId] int NULL,
        [ArchivingMailboxPlanId] int NULL,
        [EnableArchiving] bit NULL,
        [LevelID] int NULL,
        [IsVIP] bit NOT NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK_ExchangeAccounts] PRIMARY KEY ([AccountID]),
        CONSTRAINT [FK_ExchangeAccounts_ExchangeMailboxPlans] FOREIGN KEY ([MailboxPlanId]) REFERENCES [ExchangeMailboxPlans] ([MailboxPlanId]),
        CONSTRAINT [FK_ExchangeAccounts_ServiceItems] FOREIGN KEY ([ItemID]) REFERENCES [ServiceItems] ([ItemID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [LyncUsers] (
        [LyncUserID] int NOT NULL IDENTITY,
        [AccountID] int NOT NULL,
        [LyncUserPlanID] int NOT NULL,
        [CreatedDate] datetime NOT NULL DEFAULT ((getdate())),
        [ModifiedDate] datetime NOT NULL DEFAULT ((getdate())),
        [SipAddress] nvarchar(300) NULL,
        CONSTRAINT [PK_LyncUsers] PRIMARY KEY ([LyncUserID]),
        CONSTRAINT [FK_LyncUsers_LyncUserPlans] FOREIGN KEY ([LyncUserPlanID]) REFERENCES [LyncUserPlans] ([LyncUserPlanId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [AccessTokens] (
        [ID] int NOT NULL IDENTITY,
        [AccessTokenGuid] uniqueidentifier NOT NULL,
        [ExpirationDate] datetime NOT NULL,
        [AccountID] int NOT NULL,
        [ItemId] int NOT NULL,
        [TokenType] int NOT NULL,
        [SmsResponse] varchar(100) NULL,
        CONSTRAINT [PK__AccessTo__3214EC27DEAEF66E] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_AccessTokens_UserId] FOREIGN KEY ([AccountID]) REFERENCES [ExchangeAccounts] ([AccountID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [BlackBerryUsers] (
        [BlackBerryUserId] int NOT NULL IDENTITY,
        [AccountId] int NOT NULL,
        [CreatedDate] datetime NOT NULL DEFAULT ((getdate())),
        [ModifiedDate] datetime NOT NULL,
        CONSTRAINT [PK_BlackBerryUsers] PRIMARY KEY ([BlackBerryUserId]),
        CONSTRAINT [FK_BlackBerryUsers_ExchangeAccounts] FOREIGN KEY ([AccountId]) REFERENCES [ExchangeAccounts] ([AccountID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [CRMUsers] (
        [CRMUserID] int NOT NULL IDENTITY,
        [AccountID] int NOT NULL,
        [CreatedDate] datetime NOT NULL DEFAULT ((getdate())),
        [ChangedDate] datetime NOT NULL DEFAULT ((getdate())),
        [CRMUserGuid] uniqueidentifier NULL,
        [BusinessUnitID] uniqueidentifier NULL,
        [CALType] int NULL,
        CONSTRAINT [PK_CRMUsers] PRIMARY KEY ([CRMUserID]),
        CONSTRAINT [FK_CRMUsers_ExchangeAccounts] FOREIGN KEY ([AccountID]) REFERENCES [ExchangeAccounts] ([AccountID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [EnterpriseFoldersOwaPermissions] (
        [ID] int NOT NULL IDENTITY,
        [ItemID] int NOT NULL,
        [FolderID] int NOT NULL,
        [AccountID] int NOT NULL,
        CONSTRAINT [PK__Enterpri__3214EC27D1B48691] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_EnterpriseFoldersOwaPermissions_AccountId] FOREIGN KEY ([AccountID]) REFERENCES [ExchangeAccounts] ([AccountID]) ON DELETE CASCADE,
        CONSTRAINT [FK_EnterpriseFoldersOwaPermissions_FolderId] FOREIGN KEY ([FolderID]) REFERENCES [EnterpriseFolders] ([EnterpriseFolderID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [ExchangeAccountEmailAddresses] (
        [AddressID] int NOT NULL IDENTITY,
        [AccountID] int NOT NULL,
        [EmailAddress] nvarchar(300) NOT NULL,
        CONSTRAINT [PK_ExchangeAccountEmailAddresses] PRIMARY KEY ([AddressID]),
        CONSTRAINT [FK_ExchangeAccountEmailAddresses_ExchangeAccounts] FOREIGN KEY ([AccountID]) REFERENCES [ExchangeAccounts] ([AccountID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [RDSCollectionUsers] (
        [ID] int NOT NULL IDENTITY,
        [RDSCollectionId] int NOT NULL,
        [AccountID] int NOT NULL,
        CONSTRAINT [PK__RDSColle__3214EC2780141EF7] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RDSCollectionUsers_RDSCollectionId] FOREIGN KEY ([RDSCollectionId]) REFERENCES [RDSCollections] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_RDSCollectionUsers_UserId] FOREIGN KEY ([AccountID]) REFERENCES [ExchangeAccounts] ([AccountID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [WebDavAccessTokens] (
        [ID] int NOT NULL IDENTITY,
        [FilePath] nvarchar(max) NOT NULL,
        [AuthData] nvarchar(max) NOT NULL,
        [AccessToken] uniqueidentifier NOT NULL,
        [ExpirationDate] datetime NOT NULL,
        [AccountID] int NOT NULL,
        [ItemId] int NOT NULL,
        CONSTRAINT [PK__WebDavAc__3214EC2708781F08] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_WebDavAccessTokens_UserId] FOREIGN KEY ([AccountID]) REFERENCES [ExchangeAccounts] ([AccountID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE TABLE [WebDavPortalUsersSettings] (
        [ID] int NOT NULL IDENTITY,
        [AccountId] int NOT NULL,
        [Settings] nvarchar(max) NULL,
        CONSTRAINT [PK__WebDavPo__3214EC278AF5195E] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_WebDavPortalUsersSettings_UserId] FOREIGN KEY ([AccountId]) REFERENCES [ExchangeAccounts] ([AccountID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SourceName') AND [object_id] = OBJECT_ID(N'[AuditLogSources]'))
        SET IDENTITY_INSERT [AuditLogSources] ON;
    EXEC(N'INSERT INTO [AuditLogSources] ([SourceName])
    VALUES (''APP_INSTALLER''),
    (''AUTO_DISCOVERY''),
    (''BACKUP''),
    (''DNS_ZONE''),
    (''DOMAIN''),
    (''ENTERPRISE_STORAGE''),
    (''EXCHANGE''),
    (''FILES''),
    (''FTP_ACCOUNT''),
    (''GLOBAL_DNS''),
    (''HOSTING_SPACE''),
    (''HOSTING_SPACE_WR''),
    (''IMPORT''),
    (''IP_ADDRESS''),
    (''MAIL_ACCOUNT''),
    (''MAIL_DOMAIN''),
    (''MAIL_FORWARDING''),
    (''MAIL_GROUP''),
    (''MAIL_LIST''),
    (''OCS''),
    (''ODBC_DSN''),
    (''ORGANIZATION''),
    (''REMOTE_DESKTOP_SERVICES''),
    (''SCHEDULER''),
    (''SERVER''),
    (''SHAREPOINT''),
    (''SPACE''),
    (''SQL_DATABASE''),
    (''SQL_USER''),
    (''STATS_SITE''),
    (''STORAGE_SPACES''),
    (''USER''),
    (''VIRTUAL_SERVER''),
    (''VLAN''),
    (''VPS''),
    (''VPS2012''),
    (''WAG_INSTALLER''),
    (''WEB_SITE'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SourceName') AND [object_id] = OBJECT_ID(N'[AuditLogSources]'))
        SET IDENTITY_INSERT [AuditLogSources] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SourceName', N'TaskName', N'TaskDescription') AND [object_id] = OBJECT_ID(N'[AuditLogTasks]'))
        SET IDENTITY_INSERT [AuditLogTasks] ON;
    EXEC(N'INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''APP_INSTALLER'', ''INSTALL_APPLICATION'', N''Install application''),
    (''AUTO_DISCOVERY'', ''IS_INSTALLED'', N''Is installed''),
    (''BACKUP'', ''BACKUP'', N''Backup''),
    (''BACKUP'', ''RESTORE'', N''Restore''),
    (''DNS_ZONE'', ''ADD_RECORD'', N''Add record''),
    (''DNS_ZONE'', ''DELETE_RECORD'', N''Delete record''),
    (''DNS_ZONE'', ''UPDATE_RECORD'', N''Update record''),
    (''DOMAIN'', ''ADD'', N''Add''),
    (''DOMAIN'', ''DELETE'', N''Delete''),
    (''DOMAIN'', ''ENABLE_DNS'', N''Enable DNS''),
    (''DOMAIN'', ''UPDATE'', N''Update''),
    (''ENTERPRISE_STORAGE'', ''CREATE_FOLDER'', N''Create folder''),
    (''ENTERPRISE_STORAGE'', ''CREATE_MAPPED_DRIVE'', N''Create mapped drive''),
    (''ENTERPRISE_STORAGE'', ''DELETE_FOLDER'', N''Delete folder''),
    (''ENTERPRISE_STORAGE'', ''DELETE_MAPPED_DRIVE'', N''Delete mapped drive''),
    (''ENTERPRISE_STORAGE'', ''GET_ORG_STATS'', N''Get organization statistics''),
    (''ENTERPRISE_STORAGE'', ''SET_ENTERPRISE_FOLDER_GENERAL_SETTINGS'', N''Set enterprise folder general settings''),
    (''EXCHANGE'', ''ADD_DISTR_LIST_ADDRESS'', N''Add distribution list e-mail address''),
    (''EXCHANGE'', ''ADD_DOMAIN'', N''Add organization domain''),
    (''EXCHANGE'', ''ADD_EXCHANGE_EXCHANGEDISCLAIMER'', N''Add Exchange disclaimer''),
    (''EXCHANGE'', ''ADD_EXCHANGE_MAILBOXPLAN_RETENTIONPOLICY_ARCHIVING'', N''Add Exchange archiving retention policy''),
    (''EXCHANGE'', ''ADD_EXCHANGE_RETENTIONPOLICYTAG'', N''Add Exchange retention policy tag''),
    (''EXCHANGE'', ''ADD_MAILBOX_ADDRESS'', N''Add mailbox e-mail address''),
    (''EXCHANGE'', ''ADD_PUBLIC_FOLDER_ADDRESS'', N''Add public folder e-mail address''),
    (''EXCHANGE'', ''CALCULATE_DISKSPACE'', N''Calculate organization disk space''),
    (''EXCHANGE'', ''CREATE_CONTACT'', N''Create contact''),
    (''EXCHANGE'', ''CREATE_DISTR_LIST'', N''Create distribution list''),
    (''EXCHANGE'', ''CREATE_MAILBOX'', N''Create mailbox''),
    (''EXCHANGE'', ''CREATE_ORG'', N''Create organization''),
    (''EXCHANGE'', ''CREATE_PUBLIC_FOLDER'', N''Create public folder''),
    (''EXCHANGE'', ''DELETE_CONTACT'', N''Delete contact''),
    (''EXCHANGE'', ''DELETE_DISTR_LIST'', N''Delete distribution list''),
    (''EXCHANGE'', ''DELETE_DISTR_LIST_ADDRESSES'', N''Delete distribution list e-mail addresses''),
    (''EXCHANGE'', ''DELETE_DOMAIN'', N''Delete organization domain''),
    (''EXCHANGE'', ''DELETE_EXCHANGE_MAILBOXPLAN_RETENTIONPOLICY_ARCHIV'', N''Delete Exchange archiving retention policy''),
    (''EXCHANGE'', ''DELETE_EXCHANGE_RETENTIONPOLICYTAG'', N''Delete Exchange retention policy tag''),
    (''EXCHANGE'', ''DELETE_MAILBOX'', N''Delete mailbox''),
    (''EXCHANGE'', ''DELETE_MAILBOX_ADDRESSES'', N''Delete mailbox e-mail addresses''),
    (''EXCHANGE'', ''DELETE_ORG'', N''Delete organization''),
    (''EXCHANGE'', ''DELETE_PUBLIC_FOLDER'', N''Delete public folder''),
    (''EXCHANGE'', ''DELETE_PUBLIC_FOLDER_ADDRESSES'', N''Delete public folder e-mail addresses''),
    (''EXCHANGE'', ''DISABLE_MAIL_PUBLIC_FOLDER'', N''Disable mail public folder'');
    INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''EXCHANGE'', ''DISABLE_MAILBOX'', N''Disable Mailbox''),
    (''EXCHANGE'', ''ENABLE_MAIL_PUBLIC_FOLDER'', N''Enable mail public folder''),
    (''EXCHANGE'', ''GET_ACTIVESYNC_POLICY'', N''Get Activesync policy''),
    (''EXCHANGE'', ''GET_CONTACT_GENERAL'', N''Get contact general settings''),
    (''EXCHANGE'', ''GET_CONTACT_MAILFLOW'', N''Get contact mail flow settings''),
    (''EXCHANGE'', ''GET_DISTR_LIST_ADDRESSES'', N''Get distribution list e-mail addresses''),
    (''EXCHANGE'', ''GET_DISTR_LIST_BYMEMBER'', N''Get distributions list by member''),
    (''EXCHANGE'', ''GET_DISTR_LIST_GENERAL'', N''Get distribution list general settings''),
    (''EXCHANGE'', ''GET_DISTR_LIST_MAILFLOW'', N''Get distribution list mail flow settings''),
    (''EXCHANGE'', ''GET_DISTRIBUTION_LIST_RESULT'', N''Get distributions list result''),
    (''EXCHANGE'', ''GET_EXCHANGE_ACCOUNTDISCLAIMERID'', N''Get Exchange account disclaimer id''),
    (''EXCHANGE'', ''GET_EXCHANGE_EXCHANGEDISCLAIMER'', N''Get Exchange disclaimer''),
    (''EXCHANGE'', ''GET_EXCHANGE_MAILBOXPLAN'', N''Get Exchange Mailbox plan''),
    (''EXCHANGE'', ''GET_EXCHANGE_MAILBOXPLANS'', N''Get Exchange Mailbox plans''),
    (''EXCHANGE'', ''GET_EXCHANGE_RETENTIONPOLICYTAG'', N''Get Exchange retention policy tag''),
    (''EXCHANGE'', ''GET_EXCHANGE_RETENTIONPOLICYTAGS'', N''Get Exchange retention policy tags''),
    (''EXCHANGE'', ''GET_FOLDERS_STATS'', N''Get organization public folder statistics''),
    (''EXCHANGE'', ''GET_MAILBOX_ADDRESSES'', N''Get mailbox e-mail addresses''),
    (''EXCHANGE'', ''GET_MAILBOX_ADVANCED'', N''Get mailbox advanced settings''),
    (''EXCHANGE'', ''GET_MAILBOX_AUTOREPLY'', N''Get Mailbox autoreply''),
    (''EXCHANGE'', ''GET_MAILBOX_GENERAL'', N''Get mailbox general settings''),
    (''EXCHANGE'', ''GET_MAILBOX_MAILFLOW'', N''Get mailbox mail flow settings''),
    (''EXCHANGE'', ''GET_MAILBOX_PERMISSIONS'', N''Get Mailbox permissions''),
    (''EXCHANGE'', ''GET_MAILBOX_STATS'', N''Get Mailbox statistics''),
    (''EXCHANGE'', ''GET_MAILBOXES_STATS'', N''Get organization mailboxes statistics''),
    (''EXCHANGE'', ''GET_MOBILE_DEVICES'', N''Get mobile devices''),
    (''EXCHANGE'', ''GET_ORG_LIMITS'', N''Get organization storage limits''),
    (''EXCHANGE'', ''GET_ORG_STATS'', N''Get organization statistics''),
    (''EXCHANGE'', ''GET_PICTURE'', N''Get picture''),
    (''EXCHANGE'', ''GET_PUBLIC_FOLDER_ADDRESSES'', N''Get public folder e-mail addresses''),
    (''EXCHANGE'', ''GET_PUBLIC_FOLDER_GENERAL'', N''Get public folder general settings''),
    (''EXCHANGE'', ''GET_PUBLIC_FOLDER_MAILFLOW'', N''Get public folder mail flow settings''),
    (''EXCHANGE'', ''GET_RESOURCE_MAILBOX'', N''Get resource Mailbox settings''),
    (''EXCHANGE'', ''SET_EXCHANGE_ACCOUNTDISCLAIMERID'', N''Set exchange account disclaimer id''),
    (''EXCHANGE'', ''SET_EXCHANGE_MAILBOXPLAN'', N''Set exchange Mailbox plan''),
    (''EXCHANGE'', ''SET_MAILBOXPLAN_RETENTIONPOLICY_ARCHIVING'', N''Set Mailbox plan retention policy archiving''),
    (''EXCHANGE'', ''SET_ORG_LIMITS'', N''Update organization storage limits''),
    (''EXCHANGE'', ''SET_PRIMARY_DISTR_LIST_ADDRESS'', N''Set distribution list primary e-mail address''),
    (''EXCHANGE'', ''SET_PRIMARY_MAILBOX_ADDRESS'', N''Set mailbox primary e-mail address''),
    (''EXCHANGE'', ''SET_PRIMARY_PUBLIC_FOLDER_ADDRESS'', N''Set public folder primary e-mail address''),
    (''EXCHANGE'', ''UPDATE_CONTACT_GENERAL'', N''Update contact general settings''),
    (''EXCHANGE'', ''UPDATE_CONTACT_MAILFLOW'', N''Update contact mail flow settings'');
    INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''EXCHANGE'', ''UPDATE_DISTR_LIST_GENERAL'', N''Update distribution list general settings''),
    (''EXCHANGE'', ''UPDATE_DISTR_LIST_MAILFLOW'', N''Update distribution list mail flow settings''),
    (''EXCHANGE'', ''UPDATE_EXCHANGE_RETENTIONPOLICYTAG'', N''Update Exchange retention policy tag''),
    (''EXCHANGE'', ''UPDATE_MAILBOX_ADVANCED'', N''Update mailbox advanced settings''),
    (''EXCHANGE'', ''UPDATE_MAILBOX_AUTOREPLY'', N''Update Mailbox autoreply''),
    (''EXCHANGE'', ''UPDATE_MAILBOX_GENERAL'', N''Update mailbox general settings''),
    (''EXCHANGE'', ''UPDATE_MAILBOX_MAILFLOW'', N''Update mailbox mail flow settings''),
    (''EXCHANGE'', ''UPDATE_PUBLIC_FOLDER_GENERAL'', N''Update public folder general settings''),
    (''EXCHANGE'', ''UPDATE_PUBLIC_FOLDER_MAILFLOW'', N''Update public folder mail flow settings''),
    (''EXCHANGE'', ''UPDATE_RESOURCE_MAILBOX'', N''Update resource Mailbox settings''),
    (''FILES'', ''COPY_FILES'', N''Copy files''),
    (''FILES'', ''CREATE_ACCESS_DATABASE'', N''Create MS Access database''),
    (''FILES'', ''CREATE_FILE'', N''Create file''),
    (''FILES'', ''CREATE_FOLDER'', N''Create folder''),
    (''FILES'', ''DELETE_FILES'', N''Delete files''),
    (''FILES'', ''MOVE_FILES'', N''Move files''),
    (''FILES'', ''RENAME_FILE'', N''Rename file''),
    (''FILES'', ''SET_PERMISSIONS'', NULL),
    (''FILES'', ''UNZIP_FILES'', N''Unzip files''),
    (''FILES'', ''UPDATE_BINARY_CONTENT'', N''Update file binary content''),
    (''FILES'', ''ZIP_FILES'', N''Zip files''),
    (''FTP_ACCOUNT'', ''ADD'', N''Add''),
    (''FTP_ACCOUNT'', ''DELETE'', N''Delete''),
    (''FTP_ACCOUNT'', ''UPDATE'', N''Update''),
    (''GLOBAL_DNS'', ''ADD'', N''Add''),
    (''GLOBAL_DNS'', ''DELETE'', N''Delete''),
    (''GLOBAL_DNS'', ''UPDATE'', N''Update''),
    (''HOSTING_SPACE'', ''ADD'', N''Add''),
    (''HOSTING_SPACE_WR'', ''ADD'', N''Add''),
    (''IMPORT'', ''IMPORT'', N''Import''),
    (''IP_ADDRESS'', ''ADD'', N''Add''),
    (''IP_ADDRESS'', ''ADD_RANGE'', N''Add range''),
    (''IP_ADDRESS'', ''ALLOCATE_PACKAGE_IP'', N''Allocate package IP addresses''),
    (''IP_ADDRESS'', ''DEALLOCATE_PACKAGE_IP'', N''Deallocate package IP addresses''),
    (''IP_ADDRESS'', ''DELETE'', N''Delete''),
    (''IP_ADDRESS'', ''DELETE_RANGE'', N''Delete IP Addresses''),
    (''IP_ADDRESS'', ''UPDATE'', N''Update''),
    (''IP_ADDRESS'', ''UPDATE_RANGE'', N''Update IP Addresses''),
    (''MAIL_ACCOUNT'', ''ADD'', N''Add''),
    (''MAIL_ACCOUNT'', ''DELETE'', N''Delete''),
    (''MAIL_ACCOUNT'', ''UPDATE'', N''Update''),
    (''MAIL_DOMAIN'', ''ADD'', N''Add'');
    INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''MAIL_DOMAIN'', ''ADD_POINTER'', N''Add pointer''),
    (''MAIL_DOMAIN'', ''DELETE'', N''Delete''),
    (''MAIL_DOMAIN'', ''DELETE_POINTER'', N''Update pointer''),
    (''MAIL_DOMAIN'', ''UPDATE'', N''Update''),
    (''MAIL_FORWARDING'', ''ADD'', N''Add''),
    (''MAIL_FORWARDING'', ''DELETE'', N''Delete''),
    (''MAIL_FORWARDING'', ''UPDATE'', N''Update''),
    (''MAIL_GROUP'', ''ADD'', N''Add''),
    (''MAIL_GROUP'', ''DELETE'', N''Delete''),
    (''MAIL_GROUP'', ''UPDATE'', N''Update''),
    (''MAIL_LIST'', ''ADD'', N''Add''),
    (''MAIL_LIST'', ''DELETE'', N''Delete''),
    (''MAIL_LIST'', ''UPDATE'', N''Update''),
    (''OCS'', ''CREATE_OCS_USER'', N''Create OCS user''),
    (''OCS'', ''GET_OCS_USERS'', N''Get OCS users''),
    (''OCS'', ''GET_OCS_USERS_COUNT'', N''Get OCS users count''),
    (''ODBC_DSN'', ''ADD'', N''Add''),
    (''ODBC_DSN'', ''DELETE'', N''Delete''),
    (''ODBC_DSN'', ''UPDATE'', N''Update''),
    (''ORGANIZATION'', ''CREATE_ORG'', N''Create organization''),
    (''ORGANIZATION'', ''CREATE_ORGANIZATION_ENTERPRISE_STORAGE'', N''Create organization enterprise storage''),
    (''ORGANIZATION'', ''CREATE_SECURITY_GROUP'', N''Create security group''),
    (''ORGANIZATION'', ''CREATE_USER'', N''Create user''),
    (''ORGANIZATION'', ''DELETE_ORG'', N''Delete organization''),
    (''ORGANIZATION'', ''DELETE_SECURITY_GROUP'', N''Delete security group''),
    (''ORGANIZATION'', ''GET_ORG_STATS'', N''Get organization statistics''),
    (''ORGANIZATION'', ''GET_SECURITY_GROUP_GENERAL'', N''Get security group general settings''),
    (''ORGANIZATION'', ''GET_SECURITY_GROUPS_BYMEMBER'', N''Get security groups by member''),
    (''ORGANIZATION'', ''GET_SUPPORT_SERVICE_LEVELS'', N''Get support service levels''),
    (''ORGANIZATION'', ''REMOVE_USER'', N''Remove user''),
    (''ORGANIZATION'', ''SEND_USER_PASSWORD_RESET_EMAIL_PINCODE'', N''Send user password reset email pincode''),
    (''ORGANIZATION'', ''SET_USER_PASSWORD'', N''Set user password''),
    (''ORGANIZATION'', ''SET_USER_USERPRINCIPALNAME'', N''Set user principal name''),
    (''ORGANIZATION'', ''UPDATE_PASSWORD_SETTINGS'', N''Update password settings''),
    (''ORGANIZATION'', ''UPDATE_SECURITY_GROUP_GENERAL'', N''Update security group general settings''),
    (''ORGANIZATION'', ''UPDATE_USER_GENERAL'', N''Update user general settings''),
    (''REMOTE_DESKTOP_SERVICES'', ''ADD_RDS_SERVER'', N''Add RDS server''),
    (''REMOTE_DESKTOP_SERVICES'', ''RESTART_RDS_SERVER'', N''Restart RDS server''),
    (''REMOTE_DESKTOP_SERVICES'', ''SET_RDS_SERVER_NEW_CONNECTIONS_ALLOWED'', N''Set RDS new connection allowed''),
    (''SCHEDULER'', ''RUN_SCHEDULE'', NULL),
    (''SERVER'', ''ADD'', N''Add''),
    (''SERVER'', ''ADD_SERVICE'', N''Add service'');
    INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''SERVER'', ''CHANGE_WINDOWS_SERVICE_STATUS'', N''Change Windows service status''),
    (''SERVER'', ''CHECK_AVAILABILITY'', N''Check availability''),
    (''SERVER'', ''CLEAR_EVENT_LOG'', N''Clear Windows event log''),
    (''SERVER'', ''DELETE'', N''Delete''),
    (''SERVER'', ''DELETE_SERVICE'', N''Delete service''),
    (''SERVER'', ''REBOOT'', N''Reboot''),
    (''SERVER'', ''RESET_TERMINAL_SESSION'', N''Reset terminal session''),
    (''SERVER'', ''TERMINATE_SYSTEM_PROCESS'', N''Terminate system process''),
    (''SERVER'', ''UPDATE'', N''Update''),
    (''SERVER'', ''UPDATE_AD_PASSWORD'', N''Update active directory password''),
    (''SERVER'', ''UPDATE_PASSWORD'', N''Update access password''),
    (''SERVER'', ''UPDATE_SERVICE'', N''Update service''),
    (''SHAREPOINT'', ''ADD_GROUP'', N''Add group''),
    (''SHAREPOINT'', ''ADD_SITE'', N''Add site''),
    (''SHAREPOINT'', ''ADD_USER'', N''Add user''),
    (''SHAREPOINT'', ''BACKUP_SITE'', N''Backup site''),
    (''SHAREPOINT'', ''DELETE_GROUP'', N''Delete group''),
    (''SHAREPOINT'', ''DELETE_SITE'', N''Delete site''),
    (''SHAREPOINT'', ''DELETE_USER'', N''Delete user''),
    (''SHAREPOINT'', ''INSTALL_WEBPARTS'', N''Install Web Parts package''),
    (''SHAREPOINT'', ''RESTORE_SITE'', N''Restore site''),
    (''SHAREPOINT'', ''UNINSTALL_WEBPARTS'', N''Uninstall Web Parts package''),
    (''SHAREPOINT'', ''UPDATE_GROUP'', N''Update group''),
    (''SHAREPOINT'', ''UPDATE_USER'', N''Update user''),
    (''SPACE'', ''CALCULATE_DISKSPACE'', N''Calculate disk space''),
    (''SPACE'', ''CHANGE_ITEMS_STATUS'', N''Change hosting items status''),
    (''SPACE'', ''CHANGE_STATUS'', N''Change hostng space status''),
    (''SPACE'', ''DELETE'', N''Delete hosting space''),
    (''SPACE'', ''DELETE_ITEMS'', N''Delete hosting items''),
    (''SQL_DATABASE'', ''ADD'', N''Add''),
    (''SQL_DATABASE'', ''BACKUP'', N''Backup''),
    (''SQL_DATABASE'', ''DELETE'', N''Delete''),
    (''SQL_DATABASE'', ''RESTORE'', N''Restore''),
    (''SQL_DATABASE'', ''TRUNCATE'', N''Truncate''),
    (''SQL_DATABASE'', ''UPDATE'', N''Update''),
    (''SQL_USER'', ''ADD'', N''Add''),
    (''SQL_USER'', ''DELETE'', N''Delete''),
    (''SQL_USER'', ''UPDATE'', N''Update''),
    (''STATS_SITE'', ''ADD'', N''Add statistics site''),
    (''STATS_SITE'', ''DELETE'', N''Delete statistics site''),
    (''STATS_SITE'', ''UPDATE'', N''Update statistics site''),
    (''STORAGE_SPACES'', ''REMOVE_STORAGE_SPACE'', N''Remove storage space'');
    INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''STORAGE_SPACES'', ''SAVE_STORAGE_SPACE'', N''Save storage space''),
    (''STORAGE_SPACES'', ''SAVE_STORAGE_SPACE_LEVEL'', N''Save storage space level''),
    (''USER'', ''ADD'', N''Add''),
    (''USER'', ''AUTHENTICATE'', N''Authenticate''),
    (''USER'', ''CHANGE_PASSWORD'', N''Change password''),
    (''USER'', ''CHANGE_PASSWORD_BY_USERNAME_PASSWORD'', N''Change password by username/password''),
    (''USER'', ''CHANGE_STATUS'', N''Change status''),
    (''USER'', ''DELETE'', N''Delete''),
    (''USER'', ''GET_BY_USERNAME_PASSWORD'', N''Get by username/password''),
    (''USER'', ''SEND_REMINDER'', N''Send password reminder''),
    (''USER'', ''UPDATE'', N''Update''),
    (''USER'', ''UPDATE_SETTINGS'', N''Update settings''),
    (''VIRTUAL_SERVER'', ''ADD_SERVICES'', N''Add services''),
    (''VIRTUAL_SERVER'', ''DELETE_SERVICES'', N''Delete services''),
    (''VLAN'', ''ADD'', N''Add''),
    (''VLAN'', ''ADD_RANGE'', N''Add range''),
    (''VLAN'', ''ALLOCATE_PACKAGE_VLAN'', N''Allocate package VLAN''),
    (''VLAN'', ''DEALLOCATE_PACKAGE_VLAN'', N''Deallocate package VLAN''),
    (''VLAN'', ''DELETE_RANGE'', N''Delete range''),
    (''VLAN'', ''UPDATE'', N''Update''),
    (''VPS'', ''ADD_EXTERNAL_IP'', N''Add external IP''),
    (''VPS'', ''ADD_PRIVATE_IP'', N''Add private IP''),
    (''VPS'', ''APPLY_SNAPSHOT'', N''Apply VPS snapshot''),
    (''VPS'', ''CANCEL_JOB'', N''Cancel Job''),
    (''VPS'', ''CHANGE_ADMIN_PASSWORD'', N''Change administrator password''),
    (''VPS'', ''CHANGE_STATE'', N''Change VPS state''),
    (''VPS'', ''CREATE'', N''Create VPS''),
    (''VPS'', ''DELETE'', N''Delete VPS''),
    (''VPS'', ''DELETE_EXTERNAL_IP'', N''Delete external IP''),
    (''VPS'', ''DELETE_PRIVATE_IP'', N''Delete private IP''),
    (''VPS'', ''DELETE_SNAPSHOT'', N''Delete VPS snapshot''),
    (''VPS'', ''DELETE_SNAPSHOT_SUBTREE'', N''Delete VPS snapshot subtree''),
    (''VPS'', ''EJECT_DVD_DISK'', N''Eject DVD disk''),
    (''VPS'', ''INSERT_DVD_DISK'', N''Insert DVD disk''),
    (''VPS'', ''REINSTALL'', N''Re-install VPS''),
    (''VPS'', ''RENAME_SNAPSHOT'', N''Rename VPS snapshot''),
    (''VPS'', ''SEND_SUMMARY_LETTER'', N''Send VPS summary letter''),
    (''VPS'', ''SET_PRIMARY_EXTERNAL_IP'', N''Set primary external IP''),
    (''VPS'', ''SET_PRIMARY_PRIVATE_IP'', N''Set primary private IP''),
    (''VPS'', ''TAKE_SNAPSHOT'', N''Take VPS snapshot''),
    (''VPS'', ''UPDATE_CONFIGURATION'', N''Update VPS configuration''),
    (''VPS'', ''UPDATE_HOSTNAME'', N''Update host name'');
    INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''VPS'', ''UPDATE_IP'', N''Update IP Address''),
    (''VPS'', ''UPDATE_PERMISSIONS'', N''Update VPS permissions''),
    (''VPS'', ''UPDATE_VDC_PERMISSIONS'', N''Update space permissions''),
    (''VPS2012'', ''ADD_EXTERNAL_IP'', N''Add external IP''),
    (''VPS2012'', ''ADD_PRIVATE_IP'', N''Add private IP''),
    (''VPS2012'', ''APPLY_SNAPSHOT'', N''Apply VM snapshot''),
    (''VPS2012'', ''CHANGE_ADMIN_PASSWORD'', N''Change administrator password''),
    (''VPS2012'', ''CHANGE_STATE'', N''Change VM state''),
    (''VPS2012'', ''CREATE'', N''Create VM''),
    (''VPS2012'', ''DELETE'', N''Delete VM''),
    (''VPS2012'', ''DELETE_EXTERNAL_IP'', N''Delete external IP''),
    (''VPS2012'', ''DELETE_PRIVATE_IP'', N''Delete private IP''),
    (''VPS2012'', ''DELETE_SNAPSHOT'', N''Delete VM snapshot''),
    (''VPS2012'', ''DELETE_SNAPSHOT_SUBTREE'', N''Delete VM snapshot subtree''),
    (''VPS2012'', ''EJECT_DVD_DISK'', N''Eject DVD disk''),
    (''VPS2012'', ''INSERT_DVD_DISK'', N''Insert DVD disk''),
    (''VPS2012'', ''REINSTALL'', N''Reinstall VM''),
    (''VPS2012'', ''RENAME_SNAPSHOT'', N''Rename VM snapshot''),
    (''VPS2012'', ''SET_PRIMARY_EXTERNAL_IP'', N''Set primary external IP''),
    (''VPS2012'', ''SET_PRIMARY_PRIVATE_IP'', N''Set primary private IP''),
    (''VPS2012'', ''TAKE_SNAPSHOT'', N''Take VM snapshot''),
    (''VPS2012'', ''UPDATE_CONFIGURATION'', N''Update VM configuration''),
    (''VPS2012'', ''UPDATE_HOSTNAME'', N''Update host name''),
    (''WAG_INSTALLER'', ''GET_APP_PARAMS_TASK'', N''Get application parameters''),
    (''WAG_INSTALLER'', ''GET_GALLERY_APP_DETAILS_TASK'', N''Get gallery application details''),
    (''WAG_INSTALLER'', ''GET_GALLERY_APPS_TASK'', N''Get gallery applications''),
    (''WAG_INSTALLER'', ''GET_GALLERY_CATEGORIES_TASK'', N''Get gallery categories''),
    (''WAG_INSTALLER'', ''GET_SRV_GALLERY_APPS_TASK'', N''Get server gallery applications''),
    (''WAG_INSTALLER'', ''INSTALL_WEB_APP'', N''Install Web application''),
    (''WEB_SITE'', ''ADD'', N''Add''),
    (''WEB_SITE'', ''ADD_POINTER'', N''Add domain pointer''),
    (''WEB_SITE'', ''ADD_SSL_FOLDER'', N''Add shared SSL folder''),
    (''WEB_SITE'', ''ADD_VDIR'', N''Add virtual directory''),
    (''WEB_SITE'', ''CHANGE_FP_PASSWORD'', N''Change FrontPage account password''),
    (''WEB_SITE'', ''CHANGE_STATE'', N''Change state''),
    (''WEB_SITE'', ''DELETE'', N''Delete''),
    (''WEB_SITE'', ''DELETE_POINTER'', N''Delete domain pointer''),
    (''WEB_SITE'', ''DELETE_SECURED_FOLDER'', N''Delete secured folder''),
    (''WEB_SITE'', ''DELETE_SECURED_GROUP'', N''Delete secured group''),
    (''WEB_SITE'', ''DELETE_SECURED_USER'', N''Delete secured user''),
    (''WEB_SITE'', ''DELETE_SSL_FOLDER'', N''Delete shared SSL folder''),
    (''WEB_SITE'', ''DELETE_VDIR'', N''Delete virtual directory'');
    INSERT INTO [AuditLogTasks] ([SourceName], [TaskName], [TaskDescription])
    VALUES (''WEB_SITE'', ''GET_STATE'', N''Get state''),
    (''WEB_SITE'', ''INSTALL_FP'', N''Install FrontPage Extensions''),
    (''WEB_SITE'', ''INSTALL_SECURED_FOLDERS'', N''Install secured folders''),
    (''WEB_SITE'', ''UNINSTALL_FP'', N''Uninstall FrontPage Extensions''),
    (''WEB_SITE'', ''UNINSTALL_SECURED_FOLDERS'', N''Uninstall secured folders''),
    (''WEB_SITE'', ''UPDATE'', N''Update''),
    (''WEB_SITE'', ''UPDATE_SECURED_FOLDER'', N''Add/update secured folder''),
    (''WEB_SITE'', ''UPDATE_SECURED_GROUP'', N''Add/update secured group''),
    (''WEB_SITE'', ''UPDATE_SECURED_USER'', N''Add/update secured user''),
    (''WEB_SITE'', ''UPDATE_SSL_FOLDER'', N''Update shared SSL folder''),
    (''WEB_SITE'', ''UPDATE_VDIR'', N''Update virtual directory'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SourceName', N'TaskName', N'TaskDescription') AND [object_id] = OBJECT_ID(N'[AuditLogTasks]'))
        SET IDENTITY_INSERT [AuditLogTasks] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'QuotaID', N'GroupID', N'HideQuota', N'ItemTypeID', N'PerOrganization', N'QuotaDescription', N'QuotaName', N'QuotaOrder', N'QuotaTypeID', N'ServiceQuota') AND [object_id] = OBJECT_ID(N'[Quotas]'))
        SET IDENTITY_INSERT [Quotas] ON;
    EXEC(N'INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (750, 36, NULL, NULL, NULL, N''DMZ Network'', N''VPS2012.DMZNetworkEnabled'', 22, 1, CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'QuotaID', N'GroupID', N'HideQuota', N'ItemTypeID', N'PerOrganization', N'QuotaDescription', N'QuotaName', N'QuotaOrder', N'QuotaTypeID', N'ServiceQuota') AND [object_id] = OBJECT_ID(N'[Quotas]'))
        SET IDENTITY_INSERT [Quotas] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GroupID', N'GroupController', N'GroupName', N'GroupOrder', N'ShowGroup') AND [object_id] = OBJECT_ID(N'[ResourceGroups]'))
        SET IDENTITY_INSERT [ResourceGroups] ON;
    EXEC(N'INSERT INTO [ResourceGroups] ([GroupID], [GroupController], [GroupName], [GroupOrder], [ShowGroup])
    VALUES (1, N''SolidCP.EnterpriseServer.OperatingSystemController'', N''OS'', 1, CAST(1 AS bit)),
    (2, N''SolidCP.EnterpriseServer.WebServerController'', N''Web'', 2, CAST(1 AS bit)),
    (3, N''SolidCP.EnterpriseServer.FtpServerController'', N''FTP'', 3, CAST(1 AS bit)),
    (4, N''SolidCP.EnterpriseServer.MailServerController'', N''Mail'', 4, CAST(1 AS bit)),
    (5, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2000'', 7, CAST(1 AS bit)),
    (6, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MySQL4'', 11, CAST(1 AS bit)),
    (7, N''SolidCP.EnterpriseServer.DnsServerController'', N''DNS'', 17, CAST(1 AS bit)),
    (8, N''SolidCP.EnterpriseServer.StatisticsServerController'', N''Statistics'', 18, CAST(1 AS bit)),
    (10, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2005'', 8, CAST(1 AS bit)),
    (11, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MySQL5'', 12, CAST(1 AS bit)),
    (12, NULL, N''Exchange'', 5, CAST(1 AS bit)),
    (13, NULL, N''Hosted Organizations'', 6, CAST(1 AS bit)),
    (20, N''SolidCP.EnterpriseServer.HostedSharePointServerController'', N''Sharepoint Foundation Server'', 14, CAST(1 AS bit)),
    (21, NULL, N''Hosted CRM'', 16, CAST(1 AS bit)),
    (22, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2008'', 9, CAST(1 AS bit)),
    (23, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2012'', 10, CAST(1 AS bit)),
    (24, NULL, N''Hosted CRM2013'', 16, CAST(1 AS bit)),
    (30, NULL, N''VPS'', 19, CAST(1 AS bit)),
    (31, NULL, N''BlackBerry'', 21, CAST(1 AS bit)),
    (32, NULL, N''OCS'', 22, CAST(1 AS bit)),
    (33, NULL, N''VPS2012'', 20, CAST(1 AS bit)),
    (40, NULL, N''VPSForPC'', 20, CAST(1 AS bit)),
    (41, NULL, N''Lync'', 24, CAST(1 AS bit)),
    (42, N''SolidCP.EnterpriseServer.HeliconZooController'', N''HeliconZoo'', 2, CAST(1 AS bit)),
    (44, N''SolidCP.EnterpriseServer.EnterpriseStorageController'', N''EnterpriseStorage'', 26, CAST(1 AS bit)),
    (45, NULL, N''RDS'', 27, CAST(1 AS bit)),
    (46, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2014'', 10, CAST(1 AS bit)),
    (47, NULL, N''Service Levels'', 2, CAST(1 AS bit)),
    (49, N''SolidCP.EnterpriseServer.StorageSpacesController'', N''StorageSpaceServices'', 26, CAST(1 AS bit)),
    (50, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MariaDB'', 11, CAST(1 AS bit)),
    (52, NULL, N''SfB'', 26, CAST(1 AS bit)),
    (61, NULL, N''MailFilters'', 5, CAST(1 AS bit)),
    (71, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2016'', 10, CAST(1 AS bit)),
    (72, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2017'', 10, CAST(1 AS bit)),
    (73, N''SolidCP.EnterpriseServer.HostedSharePointServerEntController'', N''Sharepoint Enterprise Server'', 15, CAST(1 AS bit)),
    (74, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2019'', 10, CAST(1 AS bit)),
    (75, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2022'', 10, CAST(1 AS bit)),
    (76, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MsSQL2025'', 10, CAST(1 AS bit)),
    (90, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MySQL8'', 12, CAST(1 AS bit)),
    (91, N''SolidCP.EnterpriseServer.DatabaseServerController'', N''MySQL9'', 12, CAST(1 AS bit)),
    (167, NULL, N''Proxmox'', 20, CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GroupID', N'GroupController', N'GroupName', N'GroupOrder', N'ShowGroup') AND [object_id] = OBJECT_ID(N'[ResourceGroups]'))
        SET IDENTITY_INSERT [ResourceGroups] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TaskID', N'RoleID', N'TaskType') AND [object_id] = OBJECT_ID(N'[ScheduleTasks]'))
        SET IDENTITY_INSERT [ScheduleTasks] ON;
    EXEC(N'INSERT INTO [ScheduleTasks] ([TaskID], [RoleID], [TaskType])
    VALUES (N''SCHEDULE_TASK_ACTIVATE_PAID_INVOICES'', 0, N''SolidCP.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', 3, N''SolidCP.EnterpriseServer.AuditLogReportTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_BACKUP'', 1, N''SolidCP.EnterpriseServer.BackupTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_BACKUP_DATABASE'', 3, N''SolidCP.EnterpriseServer.BackupDatabaseTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE'', 2, N''SolidCP.EnterpriseServer.CalculateExchangeDiskspaceTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH'', 1, N''SolidCP.EnterpriseServer.CalculatePackagesBandwidthTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE'', 1, N''SolidCP.EnterpriseServer.CalculatePackagesDiskspaceTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES'', 0, N''SolidCP.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_CHECK_WEBSITE'', 3, N''SolidCP.EnterpriseServer.CheckWebSiteTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_DELETE_EXCHANGE_ACCOUNTS'', 3, N''SolidCP.EnterpriseServer.DeleteExchangeAccountsTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_DOMAIN_EXPIRATION'', 3, N''SolidCP.EnterpriseServer.DomainExpirationTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_DOMAIN_LOOKUP'', 1, N''SolidCP.EnterpriseServer.DomainLookupViewTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_FTP_FILES'', 3, N''SolidCP.EnterpriseServer.FTPFilesTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_GENERATE_INVOICES'', 0, N''SolidCP.Ecommerce.EnterpriseServer.GenerateInvoicesTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', 2, N''SolidCP.EnterpriseServer.HostedSolutionReportTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', 2, N''SolidCP.EnterpriseServer.NotifyOverusedDatabasesTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_RUN_PAYMENT_QUEUE'', 0, N''SolidCP.Ecommerce.EnterpriseServer.RunPaymentQueueTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_RUN_SYSTEM_COMMAND'', 1, N''SolidCP.EnterpriseServer.RunSystemCommandTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_SEND_MAIL'', 3, N''SolidCP.EnterpriseServer.SendMailNotificationTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES'', 0, N''SolidCP.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_SUSPEND_PACKAGES'', 2, N''SolidCP.EnterpriseServer.SuspendOverusedPackagesTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION'', 1, N''SolidCP.EnterpriseServer.UserPasswordExpirationNotificationTask, SolidCP.EnterpriseServer.Code''),
    (N''SCHEDULE_TASK_ZIP_FILES'', 3, N''SolidCP.EnterpriseServer.ZipFilesTask, SolidCP.EnterpriseServer.Code'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TaskID', N'RoleID', N'TaskType') AND [object_id] = OBJECT_ID(N'[ScheduleTasks]'))
        SET IDENTITY_INSERT [ScheduleTasks] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyName', N'SettingsName', N'PropertyValue') AND [object_id] = OBJECT_ID(N'[SystemSettings]'))
        SET IDENTITY_INSERT [SystemSettings] ON;
    EXEC(N'INSERT INTO [SystemSettings] ([PropertyName], [SettingsName], [PropertyValue])
    VALUES (N''AccessIps'', N''AccessIpsSettings'', N''''),
    (N''CanPeerChangeMfa'', N''AuthenticationSettings'', N''True''),
    (N''MfaTokenAppDisplayName'', N''AuthenticationSettings'', N''SolidCP''),
    (N''BackupsPath'', N''BackupSettings'', N''c:\HostingBackups''),
    (N''SmtpEnableSsl'', N''SmtpSettings'', N''False''),
    (N''SmtpPort'', N''SmtpSettings'', N''25''),
    (N''SmtpServer'', N''SmtpSettings'', N''127.0.0.1''),
    (N''SmtpUsername'', N''SmtpSettings'', N''postmaster'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyName', N'SettingsName', N'PropertyValue') AND [object_id] = OBJECT_ID(N'[SystemSettings]'))
        SET IDENTITY_INSERT [SystemSettings] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ThemeSettingID', N'PropertyName', N'PropertyValue', N'SettingsName', N'ThemeID') AND [object_id] = OBJECT_ID(N'[ThemeSettings]'))
        SET IDENTITY_INSERT [ThemeSettings] ON;
    EXEC(N'INSERT INTO [ThemeSettings] ([ThemeSettingID], [PropertyName], [PropertyValue], [SettingsName], [ThemeID])
    VALUES (1, N''Light'', N''light-theme'', N''Style'', 1),
    (2, N''Dark'', N''dark-theme'', N''Style'', 1),
    (3, N''Semi Dark'', N''semi-dark'', N''Style'', 1),
    (4, N''Minimal'', N''minimal-theme'', N''Style'', 1),
    (5, N''#0727d7'', N''headercolor1'', N''color-header'', 1),
    (6, N''#23282c'', N''headercolor2'', N''color-header'', 1),
    (7, N''#e10a1f'', N''headercolor3'', N''color-header'', 1),
    (8, N''#157d4c'', N''headercolor4'', N''color-header'', 1),
    (9, N''#673ab7'', N''headercolor5'', N''color-header'', 1),
    (10, N''#795548'', N''headercolor6'', N''color-header'', 1),
    (11, N''#d3094e'', N''headercolor7'', N''color-header'', 1),
    (12, N''#ff9800'', N''headercolor8'', N''color-header'', 1),
    (13, N''#6c85ec'', N''sidebarcolor1'', N''color-Sidebar'', 1),
    (14, N''#5b737f'', N''sidebarcolor2'', N''color-Sidebar'', 1),
    (15, N''#408851'', N''sidebarcolor3'', N''color-Sidebar'', 1),
    (16, N''#230924'', N''sidebarcolor4'', N''color-Sidebar'', 1),
    (17, N''#903a85'', N''sidebarcolor5'', N''color-Sidebar'', 1),
    (18, N''#a04846'', N''sidebarcolor6'', N''color-Sidebar'', 1),
    (19, N''#a65314'', N''sidebarcolor7'', N''color-Sidebar'', 1),
    (20, N''#1f0e3b'', N''sidebarcolor8'', N''color-Sidebar'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ThemeSettingID', N'PropertyName', N'PropertyValue', N'SettingsName', N'ThemeID') AND [object_id] = OBJECT_ID(N'[ThemeSettings]'))
        SET IDENTITY_INSERT [ThemeSettings] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ThemeID', N'DisplayName', N'DisplayOrder', N'Enabled', N'LTRName', N'RTLName') AND [object_id] = OBJECT_ID(N'[Themes]'))
        SET IDENTITY_INSERT [Themes] ON;
    EXEC(N'INSERT INTO [Themes] ([ThemeID], [DisplayName], [DisplayOrder], [Enabled], [LTRName], [RTLName])
    VALUES (1, N''SolidCP v1'', 1, 1, N''Default'', N''Default'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ThemeID', N'DisplayName', N'DisplayOrder', N'Enabled', N'LTRName', N'RTLName') AND [object_id] = OBJECT_ID(N'[Themes]'))
        SET IDENTITY_INSERT [Themes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserID', N'AdditionalParams', N'Address', N'Changed', N'City', N'Comments', N'CompanyName', N'Country', N'Created', N'EcommerceEnabled', N'Email', N'FailedLogins', N'Fax', N'FirstName', N'HtmlMail', N'InstantMessenger', N'LastName', N'LoginStatusId', N'OneTimePasswordState', N'OwnerID', N'Password', N'PinSecret', N'PrimaryPhone', N'RoleID', N'SecondaryEmail', N'SecondaryPhone', N'State', N'StatusID', N'SubscriberNumber', N'Username', N'Zip') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([UserID], [AdditionalParams], [Address], [Changed], [City], [Comments], [CompanyName], [Country], [Created], [EcommerceEnabled], [Email], [FailedLogins], [Fax], [FirstName], [HtmlMail], [InstantMessenger], [LastName], [LoginStatusId], [OneTimePasswordState], [OwnerID], [Password], [PinSecret], [PrimaryPhone], [RoleID], [SecondaryEmail], [SecondaryPhone], [State], [StatusID], [SubscriberNumber], [Username], [Zip])
    VALUES (1, NULL, N'''', ''2010-07-16T10:53:02.453'', N'''', N'''', NULL, N'''', ''2010-07-16T12:53:02.453'', CAST(1 AS bit), N''serveradmin@myhosting.com'', NULL, '''', N''Enterprise'', CAST(1 AS bit), '''', N''Administrator'', NULL, NULL, NULL, N'''', NULL, '''', 1, N'''', '''', N'''', 1, NULL, N''serveradmin'', '''')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserID', N'AdditionalParams', N'Address', N'Changed', N'City', N'Comments', N'CompanyName', N'Country', N'Created', N'EcommerceEnabled', N'Email', N'FailedLogins', N'Fax', N'FirstName', N'HtmlMail', N'InstantMessenger', N'LastName', N'LoginStatusId', N'OneTimePasswordState', N'OwnerID', N'Password', N'PinSecret', N'PrimaryPhone', N'RoleID', N'SecondaryEmail', N'SecondaryPhone', N'State', N'StatusID', N'SubscriberNumber', N'Username', N'Zip') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DatabaseVersion', N'BuildDate') AND [object_id] = OBJECT_ID(N'[Versions]'))
        SET IDENTITY_INSERT [Versions] ON;
    EXEC(N'INSERT INTO [Versions] ([DatabaseVersion], [BuildDate])
    VALUES (''1.0'', ''2010-04-10T00:00:00.000''),
    (''1.0.1.0'', ''2010-07-16T12:53:03.563''),
    (''1.0.2.0'', ''2010-09-03T00:00:00.000''),
    (''1.1.0.9'', ''2010-11-16T00:00:00.000''),
    (''1.1.2.13'', ''2011-04-15T00:00:00.000''),
    (''1.2.0.38'', ''2011-07-13T00:00:00.000''),
    (''1.2.1.6'', ''2012-03-29T00:00:00.000''),
    (''1.4.9'', ''2024-04-20T00:00:00.000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DatabaseVersion', N'BuildDate') AND [object_id] = OBJECT_ID(N'[Versions]'))
        SET IDENTITY_INSERT [Versions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PackageID', N'BandwidthUpdated', N'PackageComments', N'PackageName', N'ParentPackageID', N'PlanID', N'PurchaseDate', N'ServerID', N'StatusID', N'StatusIDchangeDate', N'UserID') AND [object_id] = OBJECT_ID(N'[Packages]'))
        SET IDENTITY_INSERT [Packages] ON;
    EXEC(N'INSERT INTO [Packages] ([PackageID], [BandwidthUpdated], [PackageComments], [PackageName], [ParentPackageID], [PlanID], [PurchaseDate], [ServerID], [StatusID], [StatusIDchangeDate], [UserID])
    VALUES (1, NULL, N'''', N''System'', NULL, NULL, NULL, NULL, 1, ''2024-10-12T19:29:19.927'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PackageID', N'BandwidthUpdated', N'PackageComments', N'PackageName', N'ParentPackageID', N'PlanID', N'PurchaseDate', N'ServerID', N'StatusID', N'StatusIDchangeDate', N'UserID') AND [object_id] = OBJECT_ID(N'[Packages]'))
        SET IDENTITY_INSERT [Packages] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProviderID', N'DisableAutoDiscovery', N'DisplayName', N'EditorControl', N'GroupID', N'ProviderName', N'ProviderType') AND [object_id] = OBJECT_ID(N'[Providers]'))
        SET IDENTITY_INSERT [Providers] ON;
    EXEC(N'INSERT INTO [Providers] ([ProviderID], [DisableAutoDiscovery], [DisplayName], [EditorControl], [GroupID], [ProviderName], [ProviderType])
    VALUES (1, NULL, N''Windows Server 2003'', N''Windows2003'', 1, N''Windows2003'', N''SolidCP.Providers.OS.Windows2003, SolidCP.Providers.OS.Windows2003''),
    (2, NULL, N''Internet Information Services 6.0'', N''IIS60'', 2, N''IIS60'', N''SolidCP.Providers.Web.IIs60, SolidCP.Providers.Web.IIs60''),
    (3, NULL, N''Microsoft FTP Server 6.0'', N''MSFTP60'', 3, N''MSFTP60'', N''SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs60''),
    (4, NULL, N''MailEnable Server 1.x - 7.x'', N''MailEnable'', 4, N''MailEnable'', N''SolidCP.Providers.Mail.MailEnable, SolidCP.Providers.Mail.MailEnable''),
    (5, NULL, N''Microsoft SQL Server 2000'', N''MSSQL'', 5, N''MSSQL'', N''SolidCP.Providers.Database.MsSqlServer, SolidCP.Providers.Database.SqlServer''),
    (6, NULL, N''MySQL Server 4.x'', N''MySQL'', 6, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer, SolidCP.Providers.Database.MySQL''),
    (7, NULL, N''Microsoft DNS Server'', N''MSDNS'', 7, N''MSDNS'', N''SolidCP.Providers.DNS.MsDNS, SolidCP.Providers.DNS.MsDNS''),
    (8, NULL, N''AWStats Statistics Service'', N''AWStats'', 8, N''AWStats'', N''SolidCP.Providers.Statistics.AWStats, SolidCP.Providers.Statistics.AWStats''),
    (9, NULL, N''SimpleDNS Plus 4.x'', N''SimpleDNS'', 7, N''SimpleDNS'', N''SolidCP.Providers.DNS.SimpleDNS, SolidCP.Providers.DNS.SimpleDNS''),
    (10, NULL, N''SmarterStats 3.x'', N''SmarterStats'', 8, N''SmarterStats'', N''SolidCP.Providers.Statistics.SmarterStats, SolidCP.Providers.Statistics.SmarterStats''),
    (11, NULL, N''SmarterMail 2.x'', N''SmarterMail'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail2, SolidCP.Providers.Mail.SmarterMail2''),
    (12, NULL, N''Gene6 FTP Server 3.x'', N''Gene6FTP'', 3, N''Gene6FTP'', N''SolidCP.Providers.FTP.Gene6, SolidCP.Providers.FTP.Gene6''),
    (13, NULL, N''Merak Mail Server 8.0.3 - 9.2.x'', N''Merak'', 4, N''Merak'', N''SolidCP.Providers.Mail.Merak, SolidCP.Providers.Mail.Merak''),
    (14, NULL, N''SmarterMail 3.x - 4.x'', N''SmarterMail'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail3, SolidCP.Providers.Mail.SmarterMail3''),
    (16, NULL, N''Microsoft SQL Server 2005'', N''MSSQL'', 10, N''MSSQL'', N''SolidCP.Providers.Database.MsSqlServer2005, SolidCP.Providers.Database.SqlServer''),
    (17, NULL, N''MySQL Server 5.0'', N''MySQL'', 11, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer50, SolidCP.Providers.Database.MySQL''),
    (18, NULL, N''MDaemon 9.x - 11.x'', N''MDaemon'', 4, N''MDaemon'', N''SolidCP.Providers.Mail.MDaemon, SolidCP.Providers.Mail.MDaemon''),
    (19, CAST(1 AS bit), N''ArGoSoft Mail Server 1.x'', N''ArgoMail'', 4, N''ArgoMail'', N''SolidCP.Providers.Mail.ArgoMail, SolidCP.Providers.Mail.ArgoMail''),
    (20, NULL, N''hMailServer 4.2'', N''hMailServer'', 4, N''hMailServer'', N''SolidCP.Providers.Mail.hMailServer, SolidCP.Providers.Mail.hMailServer''),
    (21, NULL, N''Ability Mail Server 2.x'', N''AbilityMailServer'', 4, N''AbilityMailServer'', N''SolidCP.Providers.Mail.AbilityMailServer, SolidCP.Providers.Mail.AbilityMailServer''),
    (22, NULL, N''hMailServer 4.3'', N''hMailServer43'', 4, N''hMailServer43'', N''SolidCP.Providers.Mail.hMailServer43, SolidCP.Providers.Mail.hMailServer43''),
    (24, NULL, N''ISC BIND 8.x - 9.x'', N''Bind'', 7, N''Bind'', N''SolidCP.Providers.DNS.IscBind, SolidCP.Providers.DNS.Bind''),
    (25, NULL, N''Serv-U FTP 6.x'', N''ServU'', 3, N''ServU'', N''SolidCP.Providers.FTP.ServU, SolidCP.Providers.FTP.ServU''),
    (26, NULL, N''FileZilla FTP Server 0.9'', N''FileZilla'', 3, N''FileZilla'', N''SolidCP.Providers.FTP.FileZilla, SolidCP.Providers.FTP.FileZilla''),
    (27, NULL, N''Hosted Microsoft Exchange Server 2007'', N''Exchange'', 12, N''Exchange2007'', N''SolidCP.Providers.HostedSolution.Exchange2007, SolidCP.Providers.HostedSolution''),
    (28, NULL, N''SimpleDNS Plus 5.x'', N''SimpleDNS'', 7, N''SimpleDNS'', N''SolidCP.Providers.DNS.SimpleDNS5, SolidCP.Providers.DNS.SimpleDNS50''),
    (29, NULL, N''SmarterMail 5.x'', N''SmarterMail50'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail5, SolidCP.Providers.Mail.SmarterMail5''),
    (30, NULL, N''MySQL Server 5.1'', N''MySQL'', 11, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer51, SolidCP.Providers.Database.MySQL''),
    (31, NULL, N''SmarterStats 4.x'', N''SmarterStats'', 8, N''SmarterStats'', N''SolidCP.Providers.Statistics.SmarterStats4, SolidCP.Providers.Statistics.SmarterStats''),
    (32, NULL, N''Hosted Microsoft Exchange Server 2010'', N''Exchange'', 12, N''Exchange2010'', N''SolidCP.Providers.HostedSolution.Exchange2010, SolidCP.Providers.HostedSolution''),
    (55, CAST(1 AS bit), N''Nettica DNS'', N''NetticaDNS'', 7, N''NetticaDNS'', N''SolidCP.Providers.DNS.Nettica, SolidCP.Providers.DNS.Nettica''),
    (56, CAST(1 AS bit), N''PowerDNS'', N''PowerDNS'', 7, N''PowerDNS'', N''SolidCP.Providers.DNS.PowerDNS, SolidCP.Providers.DNS.PowerDNS''),
    (60, NULL, N''SmarterMail 6.x'', N''SmarterMail60'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail6, SolidCP.Providers.Mail.SmarterMail6''),
    (61, NULL, N''Merak Mail Server 10.x'', N''Merak'', 4, N''Merak'', N''SolidCP.Providers.Mail.Merak10, SolidCP.Providers.Mail.Merak10''),
    (62, NULL, N''SmarterStats 5.x +'', N''SmarterStats'', 8, N''SmarterStats'', N''SolidCP.Providers.Statistics.SmarterStats5, SolidCP.Providers.Statistics.SmarterStats''),
    (63, NULL, N''hMailServer 5.x'', N''hMailServer5'', 4, N''hMailServer5'', N''SolidCP.Providers.Mail.hMailServer5, SolidCP.Providers.Mail.hMailServer5''),
    (64, NULL, N''SmarterMail 7.x - 8.x'', N''SmarterMail60'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail7, SolidCP.Providers.Mail.SmarterMail7''),
    (65, NULL, N''SmarterMail 9.x'', N''SmarterMail60'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail9, SolidCP.Providers.Mail.SmarterMail9''),
    (66, NULL, N''SmarterMail 10.x +'', N''SmarterMail100'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail10, SolidCP.Providers.Mail.SmarterMail10''),
    (67, NULL, N''SmarterMail 100.x +'', N''SmarterMail100x'', 4, N''SmarterMail'', N''SolidCP.Providers.Mail.SmarterMail100, SolidCP.Providers.Mail.SmarterMail100''),
    (90, NULL, N''Hosted Microsoft Exchange Server 2010 SP2'', N''Exchange'', 12, N''Exchange2010SP2'', N''SolidCP.Providers.HostedSolution.Exchange2010SP2, SolidCP.Providers.HostedSolution''),
    (91, NULL, N''Hosted Microsoft Exchange Server 2013'', N''Exchange'', 12, N''Exchange2013'', N''SolidCP.Providers.HostedSolution.Exchange2013, SolidCP.Providers.HostedSolution.Exchange2013'');
    INSERT INTO [Providers] ([ProviderID], [DisableAutoDiscovery], [DisplayName], [EditorControl], [GroupID], [ProviderName], [ProviderType])
    VALUES (92, NULL, N''Hosted Microsoft Exchange Server 2016'', N''Exchange'', 12, N''Exchange2016'', N''SolidCP.Providers.HostedSolution.Exchange2016, SolidCP.Providers.HostedSolution.Exchange2016''),
    (93, NULL, N''Hosted Microsoft Exchange Server 2019'', N''Exchange'', 12, N''Exchange2016'', N''SolidCP.Providers.HostedSolution.Exchange2019, SolidCP.Providers.HostedSolution.Exchange2019''),
    (100, NULL, N''Windows Server 2008'', N''Windows2008'', 1, N''Windows2008'', N''SolidCP.Providers.OS.Windows2008, SolidCP.Providers.OS.Windows2008''),
    (101, NULL, N''Internet Information Services 7.0'', N''IIS70'', 2, N''IIS70'', N''SolidCP.Providers.Web.IIs70, SolidCP.Providers.Web.IIs70''),
    (102, NULL, N''Microsoft FTP Server 7.0'', N''MSFTP70'', 3, N''MSFTP70'', N''SolidCP.Providers.FTP.MsFTP, SolidCP.Providers.FTP.IIs70''),
    (103, NULL, N''Hosted Organizations'', N''Organizations'', 13, N''Organizations'', N''SolidCP.Providers.HostedSolution.OrganizationProvider, SolidCP.Providers.HostedSolution''),
    (104, NULL, N''Windows Server 2012'', N''Windows2012'', 1, N''Windows2012'', N''SolidCP.Providers.OS.Windows2012, SolidCP.Providers.OS.Windows2012''),
    (105, NULL, N''Internet Information Services 8.0'', N''IIS70'', 2, N''IIS80'', N''SolidCP.Providers.Web.IIs80, SolidCP.Providers.Web.IIs80''),
    (106, NULL, N''Microsoft FTP Server 8.0'', N''MSFTP70'', 3, N''MSFTP80'', N''SolidCP.Providers.FTP.MsFTP80, SolidCP.Providers.FTP.IIs80''),
    (110, NULL, N''Cerberus FTP Server 6.x'', N''CerberusFTP6'', 3, N''CerberusFTP6'', N''SolidCP.Providers.FTP.CerberusFTP6, SolidCP.Providers.FTP.CerberusFTP6''),
    (111, NULL, N''Windows Server 2016'', N''Windows2008'', 1, N''Windows2016'', N''SolidCP.Providers.OS.Windows2016, SolidCP.Providers.OS.Windows2016''),
    (112, NULL, N''Internet Information Services 10.0'', N''IIS70'', 2, N''IIS100'', N''SolidCP.Providers.Web.IIs100, SolidCP.Providers.Web.IIs100''),
    (113, NULL, N''Microsoft FTP Server 10.0'', N''MSFTP70'', 3, N''MSFTP100'', N''SolidCP.Providers.FTP.MsFTP100, SolidCP.Providers.FTP.IIs100''),
    (135, CAST(1 AS bit), N''Web Application Engines'', N''HeliconZoo'', 42, N''HeliconZoo'', N''SolidCP.Providers.Web.HeliconZoo.HeliconZoo, SolidCP.Providers.Web.HeliconZoo''),
    (160, NULL, N''IceWarp Mail Server'', N''IceWarp'', 4, N''IceWarp'', N''SolidCP.Providers.Mail.IceWarp, SolidCP.Providers.Mail.IceWarp''),
    (200, NULL, N''Hosted Windows SharePoint Services 3.0'', N''HostedSharePoint30'', 20, N''HostedSharePoint30'', N''SolidCP.Providers.HostedSolution.HostedSharePointServer, SolidCP.Providers.HostedSolution''),
    (201, NULL, N''Hosted MS CRM 4.0'', N''CRM'', 21, N''CRM'', N''SolidCP.Providers.HostedSolution.CRMProvider, SolidCP.Providers.HostedSolution''),
    (202, NULL, N''Microsoft SQL Server 2008'', N''MSSQL'', 22, N''MsSQL'', N''SolidCP.Providers.Database.MsSqlServer2008, SolidCP.Providers.Database.SqlServer''),
    (203, CAST(1 AS bit), N''BlackBerry 4.1'', N''BlackBerry'', 31, N''BlackBerry 4.1'', N''SolidCP.Providers.HostedSolution.BlackBerryProvider, SolidCP.Providers.HostedSolution''),
    (204, CAST(1 AS bit), N''BlackBerry 5.0'', N''BlackBerry5'', 31, N''BlackBerry 5.0'', N''SolidCP.Providers.HostedSolution.BlackBerry5Provider, SolidCP.Providers.HostedSolution''),
    (205, CAST(1 AS bit), N''Office Communications Server 2007 R2'', N''OCS'', 32, N''OCS'', N''SolidCP.Providers.HostedSolution.OCS2007R2, SolidCP.Providers.HostedSolution''),
    (206, CAST(1 AS bit), N''OCS Edge server'', N''OCS_Edge'', 32, N''OCSEdge'', N''SolidCP.Providers.HostedSolution.OCSEdge2007R2, SolidCP.Providers.HostedSolution''),
    (208, NULL, N''Hosted SharePoint Foundation 2010'', N''HostedSharePoint30'', 20, N''HostedSharePoint2010'', N''SolidCP.Providers.HostedSolution.HostedSharePointServer2010, SolidCP.Providers.HostedSolution''),
    (209, NULL, N''Microsoft SQL Server 2012'', N''MSSQL'', 23, N''MsSQL'', N''SolidCP.Providers.Database.MsSqlServer2012, SolidCP.Providers.Database.SqlServer''),
    (250, NULL, N''Microsoft Lync Server 2010 Multitenant Hosting Pack'', N''Lync'', 41, N''Lync2010'', N''SolidCP.Providers.HostedSolution.Lync2010, SolidCP.Providers.HostedSolution''),
    (300, CAST(1 AS bit), N''Microsoft Hyper-V'', N''HyperV'', 30, N''HyperV'', N''SolidCP.Providers.Virtualization.HyperV, SolidCP.Providers.Virtualization.HyperV''),
    (301, NULL, N''MySQL Server 5.5'', N''MySQL'', 11, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer55, SolidCP.Providers.Database.MySQL''),
    (302, NULL, N''MySQL Server 5.6'', N''MySQL'', 11, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer56, SolidCP.Providers.Database.MySQL''),
    (303, NULL, N''MySQL Server 5.7'', N''MySQL'', 11, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer57, SolidCP.Providers.Database.MySQL''),
    (304, NULL, N''MySQL Server 8.0'', N''MySQL'', 90, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer80, SolidCP.Providers.Database.MySQL''),
    (305, NULL, N''MySQL Server 8.1'', N''MySQL'', 90, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer81, SolidCP.Providers.Database.MySQL''),
    (306, NULL, N''MySQL Server 8.2'', N''MySQL'', 90, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer82, SolidCP.Providers.Database.MySQL''),
    (307, NULL, N''MySQL Server 8.3'', N''MySQL'', 90, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer83, SolidCP.Providers.Database.MySQL''),
    (308, NULL, N''MySQL Server 8.4'', N''MySQL'', 90, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer84, SolidCP.Providers.Database.MySQL''),
    (320, NULL, N''MySQL Server 9.0'', N''MySQL'', 90, N''MySQL'', N''SolidCP.Providers.Database.MySqlServer90, SolidCP.Providers.Database.MySQL''),
    (350, CAST(1 AS bit), N''Microsoft Hyper-V 2012 R2'', N''HyperV2012R2'', 33, N''HyperV2012R2'', N''SolidCP.Providers.Virtualization.HyperV2012R2, SolidCP.Providers.Virtualization.HyperV2012R2''),
    (351, CAST(1 AS bit), N''Microsoft Hyper-V Virtual Machine Management'', N''HyperVvmm'', 33, N''HyperVvmm'', N''SolidCP.Providers.Virtualization.HyperVvmm, SolidCP.Providers.Virtualization.HyperVvmm''),
    (352, CAST(1 AS bit), N''Microsoft Hyper-V 2016'', N''HyperV2012R2'', 33, N''HyperV2016'', N''SolidCP.Providers.Virtualization.HyperV2016, SolidCP.Providers.Virtualization.HyperV2016''),
    (370, CAST(1 AS bit), N''Proxmox Virtualization (remote)'', N''Proxmox'', 167, N''Proxmox (remote)'', N''SolidCP.Providers.Virtualization.Proxmoxvps, SolidCP.Providers.Virtualization.Proxmoxvps''),
    (371, CAST(0 AS bit), N''Proxmox Virtualization'', N''Proxmox'', 167, N''Proxmox'', N''SolidCP.Providers.Virtualization.ProxmoxvpsLocal, SolidCP.Providers.Virtualization.Proxmoxvps''),
    (400, CAST(1 AS bit), N''Microsoft Hyper-V For Private Cloud'', N''HyperVForPrivateCloud'', 40, N''HyperVForPC'', N''SolidCP.Providers.VirtualizationForPC.HyperVForPC, SolidCP.Providers.VirtualizationForPC.HyperVForPC''),
    (410, NULL, N''Microsoft DNS Server 2012+'', N''MSDNS'', 7, N''MSDNS.2012'', N''SolidCP.Providers.DNS.MsDNS2012, SolidCP.Providers.DNS.MsDNS2012'');
    INSERT INTO [Providers] ([ProviderID], [DisableAutoDiscovery], [DisplayName], [EditorControl], [GroupID], [ProviderName], [ProviderType])
    VALUES (500, NULL, N''Unix System'', N''Unix'', 1, N''UnixSystem'', N''SolidCP.Providers.OS.Unix, SolidCP.Providers.OS.Unix''),
    (600, NULL, N''Enterprise Storage Windows 2012'', N''EnterpriseStorage'', 44, N''EnterpriseStorage2012'', N''SolidCP.Providers.EnterpriseStorage.Windows2012, SolidCP.Providers.EnterpriseStorage.Windows2012''),
    (700, NULL, N''Storage Spaces Windows 2012'', N''StorageSpaceServices'', 49, N''StorageSpace2012'', N''SolidCP.Providers.StorageSpaces.Windows2012, SolidCP.Providers.StorageSpaces.Windows2012''),
    (1201, NULL, N''Hosted MS CRM 2011'', N''CRM2011'', 21, N''CRM'', N''SolidCP.Providers.HostedSolution.CRMProvider2011, SolidCP.Providers.HostedSolution.CRM2011''),
    (1202, NULL, N''Hosted MS CRM 2013'', N''CRM2011'', 24, N''CRM'', N''SolidCP.Providers.HostedSolution.CRMProvider2013, SolidCP.Providers.HostedSolution.Crm2013''),
    (1203, NULL, N''Microsoft SQL Server 2014'', N''MSSQL'', 46, N''MsSQL'', N''SolidCP.Providers.Database.MsSqlServer2014, SolidCP.Providers.Database.SqlServer''),
    (1205, NULL, N''Hosted MS CRM 2015'', N''CRM2011'', 24, N''CRM'', N''SolidCP.Providers.HostedSolution.CRMProvider2015, SolidCP.Providers.HostedSolution.Crm2015''),
    (1206, NULL, N''Hosted MS CRM 2016'', N''CRM2011'', 24, N''CRM'', N''SolidCP.Providers.HostedSolution.CRMProvider2016, SolidCP.Providers.HostedSolution.Crm2016''),
    (1301, NULL, N''Hosted SharePoint Foundation 2013'', N''HostedSharePoint30'', 20, N''HostedSharePoint2013'', N''SolidCP.Providers.HostedSolution.HostedSharePointServer2013, SolidCP.Providers.HostedSolution.SharePoint2013''),
    (1306, NULL, N''Hosted SharePoint Foundation 2016'', N''HostedSharePoint30'', 20, N''HostedSharePoint2016'', N''SolidCP.Providers.HostedSolution.HostedSharePointServer2016, SolidCP.Providers.HostedSolution.SharePoint2016''),
    (1401, NULL, N''Microsoft Lync Server 2013 Enterprise Edition'', N''Lync'', 41, N''Lync2013'', N''SolidCP.Providers.HostedSolution.Lync2013, SolidCP.Providers.HostedSolution.Lync2013''),
    (1402, NULL, N''Microsoft Lync Server 2013 Multitenant Hosting Pack'', N''Lync'', 41, N''Lync2013HP'', N''SolidCP.Providers.HostedSolution.Lync2013HP, SolidCP.Providers.HostedSolution.Lync2013HP''),
    (1403, NULL, N''Microsoft Skype for Business Server 2015'', N''SfB'', 52, N''SfB2015'', N''SolidCP.Providers.HostedSolution.SfB2015, SolidCP.Providers.HostedSolution.SfB2015''),
    (1404, NULL, N''Microsoft Skype for Business Server 2019'', N''SfB'', 52, N''SfB2019'', N''SolidCP.Providers.HostedSolution.SfB2019, SolidCP.Providers.HostedSolution.SfB2019''),
    (1501, CAST(1 AS bit), N''Remote Desktop Services Windows 2012'', N''RDS'', 45, N''RemoteDesktopServices2012'', N''SolidCP.Providers.RemoteDesktopServices.Windows2012,SolidCP.Providers.RemoteDesktopServices.Windows2012''),
    (1502, CAST(1 AS bit), N''Remote Desktop Services Windows 2016'', N''RDS'', 45, N''RemoteDesktopServices2012'', N''SolidCP.Providers.RemoteDesktopServices.Windows2016,SolidCP.Providers.RemoteDesktopServices.Windows2016''),
    (1503, CAST(1 AS bit), N''Remote Desktop Services Windows 2019'', N''RDS'', 45, N''RemoteDesktopServices2019'', N''SolidCP.Providers.RemoteDesktopServices.Windows2019,SolidCP.Providers.RemoteDesktopServices.Windows2019''),
    (1504, CAST(1 AS bit), N''Remote Desktop Services Windows 2022'', N''RDS'', 45, N''RemoteDesktopServices2022'', N''SolidCP.Providers.RemoteDesktopServices.Windows2022,SolidCP.Providers.RemoteDesktopServices.Windows2022''),
    (1505, CAST(1 AS bit), N''Remote Desktop Services Windows 2025'', N''RDS'', 45, N''RemoteDesktopServices2025'', N''SolidCP.Providers.RemoteDesktopServices.Windows2025,SolidCP.Providers.RemoteDesktopServices.Windows2025''),
    (1550, NULL, N''MariaDB 10.1'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB101, SolidCP.Providers.Database.MariaDB''),
    (1552, NULL, N''Hosted SharePoint Enterprise 2013'', N''HostedSharePoint30'', 73, N''HostedSharePoint2013Ent'', N''SolidCP.Providers.HostedSolution.HostedSharePointServer2013Ent, SolidCP.Providers.HostedSolution.SharePoint2013Ent''),
    (1560, NULL, N''MariaDB 10.2'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB102, SolidCP.Providers.Database.MariaDB''),
    (1570, NULL, N''MariaDB 10.3'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB103, SolidCP.Providers.Database.MariaDB''),
    (1571, NULL, N''MariaDB 10.4'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB104, SolidCP.Providers.Database.MariaDB''),
    (1572, NULL, N''MariaDB 10.5'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB105, SolidCP.Providers.Database.MariaDB''),
    (1573, NULL, N''MariaDB 10.6'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB106, SolidCP.Providers.Database.MariaDB''),
    (1574, NULL, N''MariaDB 10.7'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB107, SolidCP.Providers.Database.MariaDB''),
    (1575, NULL, N''MariaDB 10.8'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB108, SolidCP.Providers.Database.MariaDB''),
    (1576, NULL, N''MariaDB 10.9'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB109, SolidCP.Providers.Database.MariaDB''),
    (1577, NULL, N''MariaDB 10.10'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB1010, SolidCP.Providers.Database.MariaDB''),
    (1578, NULL, N''MariaDB 10.11'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB1011, SolidCP.Providers.Database.MariaDB''),
    (1579, NULL, N''MariaDB 11.0'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB110, SolidCP.Providers.Database.MariaDB''),
    (1580, NULL, N''MariaDB 11.1'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB111, SolidCP.Providers.Database.MariaDB''),
    (1581, NULL, N''MariaDB 11.2'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB112, SolidCP.Providers.Database.MariaDB''),
    (1582, NULL, N''MariaDB 11.3'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB113, SolidCP.Providers.Database.MariaDB''),
    (1583, NULL, N''MariaDB 11.4'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB114, SolidCP.Providers.Database.MariaDB''),
    (1584, NULL, N''MariaDB 11.5'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB115, SolidCP.Providers.Database.MariaDB''),
    (1585, NULL, N''MariaDB 11.6'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB116, SolidCP.Providers.Database.MariaDB''),
    (1586, NULL, N''MariaDB 11.7'', N''MariaDB'', 50, N''MariaDB'', N''SolidCP.Providers.Database.MariaDB117, SolidCP.Providers.Database.MariaDB''),
    (1601, CAST(1 AS bit), N''Mail Cleaner'', N''MailCleaner'', 61, N''MailCleaner'', N''SolidCP.Providers.Filters.MailCleaner, SolidCP.Providers.Filters.MailCleaner''),
    (1602, CAST(1 AS bit), N''SpamExperts Mail Filter'', N''SpamExperts'', 61, N''SpamExperts'', N''SolidCP.Providers.Filters.SpamExperts, SolidCP.Providers.Filters.SpamExperts''),
    (1701, NULL, N''Microsoft SQL Server 2016'', N''MSSQL'', 71, N''MsSQL'', N''SolidCP.Providers.Database.MsSqlServer2016, SolidCP.Providers.Database.SqlServer'');
    INSERT INTO [Providers] ([ProviderID], [DisableAutoDiscovery], [DisplayName], [EditorControl], [GroupID], [ProviderName], [ProviderType])
    VALUES (1702, NULL, N''Hosted SharePoint Enterprise 2016'', N''HostedSharePoint30'', 73, N''HostedSharePoint2016Ent'', N''SolidCP.Providers.HostedSolution.HostedSharePointServer2016Ent, SolidCP.Providers.HostedSolution.SharePoint2016Ent''),
    (1703, NULL, N''SimpleDNS Plus 6.x'', N''SimpleDNS'', 7, N''SimpleDNS'', N''SolidCP.Providers.DNS.SimpleDNS6, SolidCP.Providers.DNS.SimpleDNS60''),
    (1704, NULL, N''Microsoft SQL Server 2017'', N''MSSQL'', 72, N''MsSQL'', N''SolidCP.Providers.Database.MsSqlServer2017, SolidCP.Providers.Database.SqlServer''),
    (1705, NULL, N''Microsoft SQL Server 2019'', N''MSSQL'', 74, N''MsSQL'', N''SolidCP.Providers.Database.MsSqlServer2019, SolidCP.Providers.Database.SqlServer''),
    (1706, NULL, N''Microsoft SQL Server 2022'', N''MSSQL'', 75, N''MsSQL'', N''SolidCP.Providers.Database.MsSqlServer2022, SolidCP.Providers.Database.SqlServer''),
    (1711, NULL, N''Hosted SharePoint 2019'', N''HostedSharePoint30'', 73, N''HostedSharePoint2019'', N''SolidCP.Providers.HostedSolution.HostedSharePointServer2019, SolidCP.Providers.HostedSolution.SharePoint2019''),
    (1800, NULL, N''Windows Server 2019'', N''Windows2012'', 1, N''Windows2019'', N''SolidCP.Providers.OS.Windows2019, SolidCP.Providers.OS.Windows2019''),
    (1801, CAST(1 AS bit), N''Microsoft Hyper-V 2019'', N''HyperV2012R2'', 33, N''HyperV2019'', N''SolidCP.Providers.Virtualization.HyperV2019, SolidCP.Providers.Virtualization.HyperV2019''),
    (1802, NULL, N''Windows Server 2022'', N''Windows2012'', 1, N''Windows2022'', N''SolidCP.Providers.OS.Windows2022, SolidCP.Providers.OS.Windows2022''),
    (1803, CAST(1 AS bit), N''Microsoft Hyper-V 2022'', N''HyperV2012R2'', 33, N''HyperV2022'', N''SolidCP.Providers.Virtualization.HyperV2022, SolidCP.Providers.Virtualization.HyperV2022''),
    (1804, NULL, N''Windows Server 2025'', N''Windows2012'', 1, N''Windows2025'', N''SolidCP.Providers.OS.Windows2025, SolidCP.Providers.OS.Windows2025''),
    (1805, CAST(1 AS bit), N''Microsoft Hyper-V 2025'', N''HyperV2012R2'', 33, N''HyperV2025'', N''SolidCP.Providers.Virtualization.HyperV2025, SolidCP.Providers.Virtualization.HyperV2025''),
    (1901, NULL, N''SimpleDNS Plus 8.x'', N''SimpleDNS'', 7, N''SimpleDNS'', N''SolidCP.Providers.DNS.SimpleDNS8, SolidCP.Providers.DNS.SimpleDNS80''),
    (1902, NULL, N''Microsoft DNS Server 2016'', N''MSDNS'', 7, N''MSDNS.2016'', N''SolidCP.Providers.DNS.MsDNS2016, SolidCP.Providers.DNS.MsDNS2016''),
    (1903, NULL, N''SimpleDNS Plus 9.x'', N''SimpleDNS'', 7, N''SimpleDNS'', N''SolidCP.Providers.DNS.SimpleDNS9, SolidCP.Providers.DNS.SimpleDNS90''),
    (1910, NULL, N''vsftpd FTP Server 3'', N''vsftpd'', 3, N''vsftpd'', N''SolidCP.Providers.FTP.VsFtp3, SolidCP.Providers.FTP.VsFtp''),
    (1911, NULL, N''Apache Web Server 2.4'', N''Apache'', 2, N''Apache'', N''SolidCP.Providers.Web.Apache24, SolidCP.Providers.Web.Apache'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProviderID', N'DisableAutoDiscovery', N'DisplayName', N'EditorControl', N'GroupID', N'ProviderName', N'ProviderType') AND [object_id] = OBJECT_ID(N'[Providers]'))
        SET IDENTITY_INSERT [Providers] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'QuotaID', N'GroupID', N'HideQuota', N'ItemTypeID', N'PerOrganization', N'QuotaDescription', N'QuotaName', N'QuotaOrder', N'QuotaTypeID', N'ServiceQuota') AND [object_id] = OBJECT_ID(N'[Quotas]'))
        SET IDENTITY_INSERT [Quotas] ON;
    EXEC(N'INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (25, 2, NULL, NULL, NULL, N''ASP.NET 1.1'', N''Web.AspNet11'', 3, 1, CAST(0 AS bit)),
    (26, 2, NULL, NULL, NULL, N''ASP.NET 2.0'', N''Web.AspNet20'', 4, 1, CAST(0 AS bit)),
    (27, 2, NULL, NULL, NULL, N''ASP'', N''Web.Asp'', 2, 1, CAST(0 AS bit)),
    (28, 2, NULL, NULL, NULL, N''PHP 4.x'', N''Web.Php4'', 5, 1, CAST(0 AS bit)),
    (29, 2, NULL, NULL, NULL, N''PHP 5.x'', N''Web.Php5'', 6, 1, CAST(0 AS bit)),
    (30, 2, NULL, NULL, NULL, N''Perl'', N''Web.Perl'', 7, 1, CAST(0 AS bit)),
    (31, 2, NULL, NULL, NULL, N''Python'', N''Web.Python'', 8, 1, CAST(0 AS bit)),
    (32, 2, NULL, NULL, NULL, N''Virtual Directories'', N''Web.VirtualDirs'', 9, 1, CAST(0 AS bit)),
    (33, 2, NULL, NULL, NULL, N''FrontPage'', N''Web.FrontPage'', 10, 1, CAST(0 AS bit)),
    (34, 2, NULL, NULL, NULL, N''Custom Security Settings'', N''Web.Security'', 11, 1, CAST(0 AS bit)),
    (35, 2, NULL, NULL, NULL, N''Custom Default Documents'', N''Web.DefaultDocs'', 12, 1, CAST(0 AS bit)),
    (36, 2, NULL, NULL, NULL, N''Dedicated Application Pools'', N''Web.AppPools'', 13, 1, CAST(0 AS bit)),
    (37, 2, NULL, NULL, NULL, N''Custom Headers'', N''Web.Headers'', 14, 1, CAST(0 AS bit)),
    (38, 2, NULL, NULL, NULL, N''Custom Errors'', N''Web.Errors'', 15, 1, CAST(0 AS bit)),
    (39, 2, NULL, NULL, NULL, N''Custom MIME Types'', N''Web.Mime'', 16, 1, CAST(0 AS bit)),
    (40, 4, NULL, NULL, NULL, N''Max Mailbox Size'', N''Mail.MaxBoxSize'', 2, 3, CAST(0 AS bit)),
    (41, 5, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2000.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (42, 5, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2000.Backup'', 5, 1, CAST(0 AS bit)),
    (43, 5, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2000.Restore'', 6, 1, CAST(0 AS bit)),
    (44, 5, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2000.Truncate'', 7, 1, CAST(0 AS bit)),
    (45, 6, NULL, NULL, NULL, N''Database Backups'', N''MySQL4.Backup'', 4, 1, CAST(0 AS bit)),
    (48, 7, NULL, NULL, NULL, N''DNS Editor'', N''DNS.Editor'', 1, 1, CAST(0 AS bit)),
    (49, 4, NULL, NULL, NULL, N''Max Group Recipients'', N''Mail.MaxGroupMembers'', 5, 3, CAST(0 AS bit)),
    (50, 4, NULL, NULL, NULL, N''Max List Recipients'', N''Mail.MaxListMembers'', 7, 3, CAST(0 AS bit)),
    (51, 1, NULL, NULL, NULL, N''Bandwidth, MB'', N''OS.Bandwidth'', 2, 2, CAST(0 AS bit)),
    (52, 1, NULL, NULL, NULL, N''Disk space, MB'', N''OS.Diskspace'', 1, 2, CAST(0 AS bit)),
    (53, 1, NULL, NULL, NULL, N''Domains'', N''OS.Domains'', 3, 2, CAST(0 AS bit)),
    (54, 1, NULL, NULL, NULL, N''Sub-Domains'', N''OS.SubDomains'', 4, 2, CAST(0 AS bit)),
    (55, 1, NULL, NULL, NULL, N''File Manager'', N''OS.FileManager'', 6, 1, CAST(0 AS bit)),
    (57, 2, NULL, NULL, NULL, N''CGI-BIN Folder'', N''Web.CgiBin'', 8, 1, CAST(0 AS bit)),
    (58, 2, NULL, NULL, NULL, N''Secured Folders'', N''Web.SecuredFolders'', 8, 1, CAST(0 AS bit)),
    (60, 2, NULL, NULL, NULL, N''Web Sites Redirection'', N''Web.Redirections'', 8, 1, CAST(0 AS bit)),
    (61, 2, NULL, NULL, NULL, N''Changing Sites Root Folders'', N''Web.HomeFolders'', 8, 1, CAST(0 AS bit)),
    (64, 10, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2005.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (65, 10, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2005.Backup'', 5, 1, CAST(0 AS bit)),
    (66, 10, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2005.Restore'', 6, 1, CAST(0 AS bit)),
    (67, 10, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2005.Truncate'', 7, 1, CAST(0 AS bit)),
    (70, 11, NULL, NULL, NULL, N''Database Backups'', N''MySQL5.Backup'', 4, 1, CAST(0 AS bit)),
    (71, 1, NULL, NULL, NULL, N''Scheduled Tasks'', N''OS.ScheduledTasks'', 9, 2, CAST(0 AS bit)),
    (72, 1, NULL, NULL, NULL, N''Interval Tasks Allowed'', N''OS.ScheduledIntervalTasks'', 10, 1, CAST(0 AS bit)),
    (73, 1, NULL, NULL, NULL, N''Minimum Tasks Interval, minutes'', N''OS.MinimumTaskInterval'', 11, 3, CAST(0 AS bit)),
    (74, 1, NULL, NULL, NULL, N''Applications Installer'', N''OS.AppInstaller'', 7, 1, CAST(0 AS bit));
    INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (75, 1, NULL, NULL, NULL, N''Extra Application Packs'', N''OS.ExtraApplications'', 8, 1, CAST(0 AS bit)),
    (77, 12, NULL, NULL, 1, N''Organization Disk Space, MB'', N''Exchange2007.DiskSpace'', 2, 2, CAST(0 AS bit)),
    (78, 12, NULL, NULL, 1, N''Mailboxes per Organization'', N''Exchange2007.Mailboxes'', 3, 2, CAST(0 AS bit)),
    (79, 12, NULL, NULL, 1, N''Contacts per Organization'', N''Exchange2007.Contacts'', 4, 3, CAST(0 AS bit)),
    (80, 12, NULL, NULL, 1, N''Distribution Lists per Organization'', N''Exchange2007.DistributionLists'', 5, 3, CAST(0 AS bit)),
    (81, 12, NULL, NULL, 1, N''Public Folders per Organization'', N''Exchange2007.PublicFolders'', 6, 3, CAST(0 AS bit)),
    (83, 12, NULL, NULL, NULL, N''POP3 Access'', N''Exchange2007.POP3Allowed'', 9, 1, CAST(0 AS bit)),
    (84, 12, NULL, NULL, NULL, N''IMAP Access'', N''Exchange2007.IMAPAllowed'', 11, 1, CAST(0 AS bit)),
    (85, 12, NULL, NULL, NULL, N''OWA/HTTP Access'', N''Exchange2007.OWAAllowed'', 13, 1, CAST(0 AS bit)),
    (86, 12, NULL, NULL, NULL, N''MAPI Access'', N''Exchange2007.MAPIAllowed'', 15, 1, CAST(0 AS bit)),
    (87, 12, NULL, NULL, NULL, N''ActiveSync Access'', N''Exchange2007.ActiveSyncAllowed'', 17, 1, CAST(0 AS bit)),
    (88, 12, NULL, NULL, NULL, N''Mail Enabled Public Folders Allowed'', N''Exchange2007.MailEnabledPublicFolders'', 8, 1, CAST(0 AS bit)),
    (94, 2, NULL, NULL, NULL, N''ColdFusion'', N''Web.ColdFusion'', 17, 1, CAST(0 AS bit)),
    (95, 2, NULL, NULL, NULL, N''Web Application Gallery'', N''Web.WebAppGallery'', 1, 1, CAST(0 AS bit)),
    (96, 2, NULL, NULL, NULL, N''ColdFusion Virtual Directories'', N''Web.CFVirtualDirectories'', 18, 1, CAST(0 AS bit)),
    (97, 2, NULL, NULL, NULL, N''Remote web management allowed'', N''Web.RemoteManagement'', 20, 1, CAST(0 AS bit)),
    (100, 2, NULL, NULL, NULL, N''Dedicated IP Addresses'', N''Web.IPAddresses'', 19, 2, CAST(1 AS bit)),
    (102, 4, NULL, NULL, NULL, N''Disable Mailbox Size Edit'', N''Mail.DisableSizeEdit'', 8, 1, CAST(0 AS bit)),
    (103, 6, NULL, NULL, NULL, N''Max Database Size'', N''MySQL4.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (104, 6, NULL, NULL, NULL, N''Database Restores'', N''MySQL4.Restore'', 5, 1, CAST(0 AS bit)),
    (105, 6, NULL, NULL, NULL, N''Database Truncate'', N''MySQL4.Truncate'', 6, 1, CAST(0 AS bit)),
    (106, 11, NULL, NULL, NULL, N''Max Database Size'', N''MySQL5.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (107, 11, NULL, NULL, NULL, N''Database Restores'', N''MySQL5.Restore'', 5, 1, CAST(0 AS bit)),
    (108, 11, NULL, NULL, NULL, N''Database Truncate'', N''MySQL5.Truncate'', 6, 1, CAST(0 AS bit)),
    (112, 90, NULL, NULL, NULL, N''Database Backups'', N''MySQL8.Backup'', 4, 1, CAST(0 AS bit)),
    (113, 90, NULL, NULL, NULL, N''Max Database Size'', N''MySQL8.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (114, 90, NULL, NULL, NULL, N''Database Restores'', N''MySQL8.Restore'', 5, 1, CAST(0 AS bit)),
    (115, 90, NULL, NULL, NULL, N''Database Truncate'', N''MySQL8.Truncate'', 6, 1, CAST(0 AS bit)),
    (122, 91, NULL, NULL, NULL, N''Database Backups'', N''MySQL9.Backup'', 4, 1, CAST(0 AS bit)),
    (123, 91, NULL, NULL, NULL, N''Max Database Size'', N''MySQL9.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (124, 91, NULL, NULL, NULL, N''Database Restores'', N''MySQL9.Restore'', 5, 1, CAST(0 AS bit)),
    (125, 91, NULL, NULL, NULL, N''Database Truncate'', N''MySQL9.Truncate'', 6, 1, CAST(0 AS bit)),
    (203, 10, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2005.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (204, 5, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2000.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (207, 13, NULL, NULL, 1, N''Domains per Organizations'', N''HostedSolution.Domains'', 3, 3, CAST(0 AS bit)),
    (208, 20, NULL, NULL, NULL, N''Max site storage, MB'', N''HostedSharePoint.MaxStorage'', 2, 3, CAST(0 AS bit)),
    (209, 21, NULL, NULL, 1, N''Full licenses per organization'', N''HostedCRM.Users'', 2, 3, CAST(0 AS bit)),
    (210, 21, NULL, NULL, NULL, N''CRM Organization'', N''HostedCRM.Organization'', 1, 1, CAST(0 AS bit)),
    (213, 22, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2008.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (214, 22, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2008.Backup'', 5, 1, CAST(0 AS bit)),
    (215, 22, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2008.Restore'', 6, 1, CAST(0 AS bit)),
    (216, 22, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2008.Truncate'', 7, 1, CAST(0 AS bit));
    INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (217, 22, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2008.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (220, 1, CAST(1 AS bit), NULL, NULL, N''Domain Pointers'', N''OS.DomainPointers'', 5, 2, CAST(0 AS bit)),
    (221, 23, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2012.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (222, 23, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2012.Backup'', 5, 1, CAST(0 AS bit)),
    (223, 23, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2012.Restore'', 6, 1, CAST(0 AS bit)),
    (224, 23, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2012.Truncate'', 7, 1, CAST(0 AS bit)),
    (225, 23, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2012.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (230, 13, NULL, NULL, NULL, N''Allow to Change UserPrincipalName'', N''HostedSolution.AllowChangeUPN'', 4, 1, CAST(0 AS bit)),
    (301, 30, NULL, NULL, NULL, N''Allow user to create VPS'', N''VPS.ManagingAllowed'', 2, 1, CAST(0 AS bit)),
    (302, 30, NULL, NULL, NULL, N''Number of CPU cores'', N''VPS.CpuNumber'', 3, 2, CAST(0 AS bit)),
    (303, 30, NULL, NULL, NULL, N''Boot from CD allowed'', N''VPS.BootCdAllowed'', 7, 1, CAST(0 AS bit)),
    (304, 30, NULL, NULL, NULL, N''Boot from CD'', N''VPS.BootCdEnabled'', 8, 1, CAST(0 AS bit)),
    (305, 30, NULL, NULL, NULL, N''RAM size, MB'', N''VPS.Ram'', 4, 2, CAST(0 AS bit)),
    (306, 30, NULL, NULL, NULL, N''Hard Drive size, GB'', N''VPS.Hdd'', 5, 2, CAST(0 AS bit)),
    (307, 30, NULL, NULL, NULL, N''DVD drive'', N''VPS.DvdEnabled'', 6, 1, CAST(0 AS bit)),
    (308, 30, NULL, NULL, NULL, N''External Network'', N''VPS.ExternalNetworkEnabled'', 10, 1, CAST(0 AS bit)),
    (309, 30, NULL, NULL, NULL, N''Number of External IP addresses'', N''VPS.ExternalIPAddressesNumber'', 11, 2, CAST(0 AS bit)),
    (310, 30, NULL, NULL, NULL, N''Private Network'', N''VPS.PrivateNetworkEnabled'', 13, 1, CAST(0 AS bit)),
    (311, 30, NULL, NULL, NULL, N''Number of Private IP addresses per VPS'', N''VPS.PrivateIPAddressesNumber'', 14, 3, CAST(0 AS bit)),
    (312, 30, NULL, NULL, NULL, N''Number of Snaphots'', N''VPS.SnapshotsNumber'', 9, 3, CAST(0 AS bit)),
    (313, 30, NULL, NULL, NULL, N''Allow user to Start, Turn off and Shutdown VPS'', N''VPS.StartShutdownAllowed'', 15, 1, CAST(0 AS bit)),
    (314, 30, NULL, NULL, NULL, N''Allow user to Pause, Resume VPS'', N''VPS.PauseResumeAllowed'', 16, 1, CAST(0 AS bit)),
    (315, 30, NULL, NULL, NULL, N''Allow user to Reboot VPS'', N''VPS.RebootAllowed'', 17, 1, CAST(0 AS bit)),
    (316, 30, NULL, NULL, NULL, N''Allow user to Reset VPS'', N''VPS.ResetAlowed'', 18, 1, CAST(0 AS bit)),
    (317, 30, NULL, NULL, NULL, N''Allow user to Re-install VPS'', N''VPS.ReinstallAllowed'', 19, 1, CAST(0 AS bit)),
    (318, 30, NULL, NULL, NULL, N''Monthly bandwidth, GB'', N''VPS.Bandwidth'', 12, 2, CAST(0 AS bit)),
    (319, 31, NULL, NULL, 1, NULL, N''BlackBerry.Users'', 1, 2, CAST(0 AS bit)),
    (320, 32, NULL, NULL, 1, NULL, N''OCS.Users'', 1, 2, CAST(0 AS bit)),
    (321, 32, NULL, NULL, NULL, NULL, N''OCS.Federation'', 2, 1, CAST(0 AS bit)),
    (322, 32, NULL, NULL, NULL, NULL, N''OCS.FederationByDefault'', 3, 1, CAST(0 AS bit)),
    (323, 32, NULL, NULL, NULL, NULL, N''OCS.PublicIMConnectivity'', 4, 1, CAST(0 AS bit)),
    (324, 32, NULL, NULL, NULL, NULL, N''OCS.PublicIMConnectivityByDefault'', 5, 1, CAST(0 AS bit)),
    (325, 32, NULL, NULL, NULL, NULL, N''OCS.ArchiveIMConversation'', 6, 1, CAST(0 AS bit)),
    (326, 32, NULL, NULL, NULL, NULL, N''OCS.ArchiveIMConvervationByDefault'', 7, 1, CAST(0 AS bit)),
    (327, 32, NULL, NULL, NULL, NULL, N''OCS.ArchiveFederatedIMConversation'', 8, 1, CAST(0 AS bit)),
    (328, 32, NULL, NULL, NULL, NULL, N''OCS.ArchiveFederatedIMConversationByDefault'', 9, 1, CAST(0 AS bit)),
    (329, 32, NULL, NULL, NULL, NULL, N''OCS.PresenceAllowed'', 10, 1, CAST(0 AS bit)),
    (330, 32, NULL, NULL, NULL, NULL, N''OCS.PresenceAllowedByDefault'', 10, 1, CAST(0 AS bit)),
    (331, 2, NULL, NULL, NULL, N''ASP.NET 4.0'', N''Web.AspNet40'', 4, 1, CAST(0 AS bit)),
    (332, 2, NULL, NULL, NULL, N''SSL'', N''Web.SSL'', 21, 1, CAST(0 AS bit)),
    (333, 2, NULL, NULL, NULL, N''Allow IP Address Mode Switch'', N''Web.AllowIPAddressModeSwitch'', 22, 1, CAST(0 AS bit)),
    (334, 2, NULL, NULL, NULL, N''Enable Hostname Support'', N''Web.EnableHostNameSupport'', 23, 1, CAST(0 AS bit));
    INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (344, 2, NULL, NULL, NULL, N''htaccess'', N''Web.Htaccess'', 9, 1, CAST(0 AS bit)),
    (346, 40, NULL, NULL, NULL, N''Allow user to create VPS'', N''VPSForPC.ManagingAllowed'', 2, 1, CAST(0 AS bit)),
    (347, 40, NULL, NULL, NULL, N''Number of CPU cores'', N''VPSForPC.CpuNumber'', 3, 2, CAST(0 AS bit)),
    (348, 40, NULL, NULL, NULL, N''Boot from CD allowed'', N''VPSForPC.BootCdAllowed'', 7, 1, CAST(0 AS bit)),
    (349, 40, NULL, NULL, NULL, N''Boot from CD'', N''VPSForPC.BootCdEnabled'', 7, 1, CAST(0 AS bit)),
    (350, 40, NULL, NULL, NULL, N''RAM size, MB'', N''VPSForPC.Ram'', 4, 2, CAST(0 AS bit)),
    (351, 40, NULL, NULL, NULL, N''Hard Drive size, GB'', N''VPSForPC.Hdd'', 5, 2, CAST(0 AS bit)),
    (352, 40, NULL, NULL, NULL, N''DVD drive'', N''VPSForPC.DvdEnabled'', 6, 1, CAST(0 AS bit)),
    (353, 40, NULL, NULL, NULL, N''External Network'', N''VPSForPC.ExternalNetworkEnabled'', 10, 1, CAST(0 AS bit)),
    (354, 40, NULL, NULL, NULL, N''Number of External IP addresses'', N''VPSForPC.ExternalIPAddressesNumber'', 11, 2, CAST(0 AS bit)),
    (355, 40, NULL, NULL, NULL, N''Private Network'', N''VPSForPC.PrivateNetworkEnabled'', 13, 1, CAST(0 AS bit)),
    (356, 40, NULL, NULL, NULL, N''Number of Private IP addresses per VPS'', N''VPSForPC.PrivateIPAddressesNumber'', 14, 3, CAST(0 AS bit)),
    (357, 40, NULL, NULL, NULL, N''Number of Snaphots'', N''VPSForPC.SnapshotsNumber'', 9, 3, CAST(0 AS bit)),
    (358, 40, NULL, NULL, NULL, N''Allow user to Start, Turn off and Shutdown VPS'', N''VPSForPC.StartShutdownAllowed'', 15, 1, CAST(0 AS bit)),
    (359, 40, NULL, NULL, NULL, N''Allow user to Pause, Resume VPS'', N''VPSForPC.PauseResumeAllowed'', 16, 1, CAST(0 AS bit)),
    (360, 40, NULL, NULL, NULL, N''Allow user to Reboot VPS'', N''VPSForPC.RebootAllowed'', 17, 1, CAST(0 AS bit)),
    (361, 40, NULL, NULL, NULL, N''Allow user to Reset VPS'', N''VPSForPC.ResetAlowed'', 18, 1, CAST(0 AS bit)),
    (362, 40, NULL, NULL, NULL, N''Allow user to Re-install VPS'', N''VPSForPC.ReinstallAllowed'', 19, 1, CAST(0 AS bit)),
    (363, 40, NULL, NULL, NULL, N''Monthly bandwidth, GB'', N''VPSForPC.Bandwidth'', 12, 2, CAST(0 AS bit)),
    (364, 12, NULL, NULL, NULL, N''Keep Deleted Items (days)'', N''Exchange2007.KeepDeletedItemsDays'', 19, 3, CAST(0 AS bit)),
    (365, 12, NULL, NULL, NULL, N''Maximum Recipients'', N''Exchange2007.MaxRecipients'', 20, 3, CAST(0 AS bit)),
    (366, 12, NULL, NULL, NULL, N''Maximum Send Message Size (Kb)'', N''Exchange2007.MaxSendMessageSizeKB'', 21, 3, CAST(0 AS bit)),
    (367, 12, NULL, NULL, NULL, N''Maximum Receive Message Size (Kb)'', N''Exchange2007.MaxReceiveMessageSizeKB'', 22, 3, CAST(0 AS bit)),
    (368, 12, NULL, NULL, NULL, N''Is Consumer Organization'', N''Exchange2007.IsConsumer'', 1, 1, CAST(0 AS bit)),
    (369, 12, NULL, NULL, NULL, N''Enable Plans Editing'', N''Exchange2007.EnablePlansEditing'', 23, 1, CAST(0 AS bit)),
    (370, 41, NULL, NULL, 1, N''Users'', N''Lync.Users'', 1, 2, CAST(0 AS bit)),
    (371, 41, NULL, NULL, NULL, N''Allow Federation'', N''Lync.Federation'', 2, 1, CAST(0 AS bit)),
    (372, 41, NULL, NULL, NULL, N''Allow Conferencing'', N''Lync.Conferencing'', 3, 1, CAST(0 AS bit)),
    (373, 41, NULL, NULL, NULL, N''Maximum Conference Particiapants'', N''Lync.MaxParticipants'', 4, 3, CAST(0 AS bit)),
    (374, 41, NULL, NULL, NULL, N''Allow Video in Conference'', N''Lync.AllowVideo'', 5, 1, CAST(0 AS bit)),
    (375, 41, NULL, NULL, NULL, N''Allow EnterpriseVoice'', N''Lync.EnterpriseVoice'', 6, 1, CAST(0 AS bit)),
    (376, 41, NULL, NULL, NULL, N''Number of Enterprise Voice Users'', N''Lync.EVUsers'', 7, 2, CAST(0 AS bit)),
    (377, 41, NULL, NULL, NULL, N''Allow National Calls'', N''Lync.EVNational'', 8, 1, CAST(0 AS bit)),
    (378, 41, NULL, NULL, NULL, N''Allow Mobile Calls'', N''Lync.EVMobile'', 9, 1, CAST(0 AS bit)),
    (379, 41, NULL, NULL, NULL, N''Allow International Calls'', N''Lync.EVInternational'', 10, 1, CAST(0 AS bit)),
    (380, 41, NULL, NULL, NULL, N''Enable Plans Editing'', N''Lync.EnablePlansEditing'', 11, 1, CAST(0 AS bit)),
    (381, 41, NULL, NULL, NULL, N''Phone Numbers'', N''Lync.PhoneNumbers'', 12, 2, CAST(0 AS bit)),
    (400, 20, NULL, NULL, NULL, N''Use shared SSL Root'', N''HostedSharePoint.UseSharedSSL'', 3, 1, CAST(0 AS bit)),
    (409, 1, NULL, NULL, NULL, N''Not allow Tenants to Delete Top Level Domains'', N''OS.NotAllowTenantDeleteDomains'', 13, 1, CAST(0 AS bit)),
    (410, 1, NULL, NULL, NULL, N''Not allow Tenants to Create Top Level Domains'', N''OS.NotAllowTenantCreateDomains'', 12, 1, CAST(0 AS bit)),
    (411, 2, NULL, NULL, NULL, N''Application Pools Restart'', N''Web.AppPoolsRestart'', 13, 1, CAST(0 AS bit)),
    (420, 12, NULL, NULL, NULL, N''Allow Litigation Hold'', N''Exchange2007.AllowLitigationHold'', 24, 1, CAST(0 AS bit));
    INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (421, 12, NULL, NULL, 1, N''Recoverable Items Space'', N''Exchange2007.RecoverableItemsSpace'', 25, 2, CAST(0 AS bit)),
    (422, 12, NULL, NULL, NULL, N''Disclaimers Allowed'', N''Exchange2007.DisclaimersAllowed'', 26, 1, CAST(0 AS bit)),
    (423, 13, NULL, NULL, 1, N''Security Groups'', N''HostedSolution.SecurityGroups'', 5, 2, CAST(0 AS bit)),
    (424, 12, NULL, NULL, NULL, N''Allow Retention Policy'', N''Exchange2013.AllowRetentionPolicy'', 27, 1, CAST(0 AS bit)),
    (425, 12, NULL, NULL, 1, N''Archiving storage, MB'', N''Exchange2013.ArchivingStorage'', 29, 2, CAST(0 AS bit)),
    (426, 12, NULL, NULL, 1, N''Archiving Mailboxes per Organization'', N''Exchange2013.ArchivingMailboxes'', 28, 2, CAST(0 AS bit)),
    (428, 12, NULL, NULL, 1, N''Resource Mailboxes per Organization'', N''Exchange2013.ResourceMailboxes'', 31, 2, CAST(0 AS bit)),
    (429, 12, NULL, NULL, 1, N''Shared Mailboxes per Organization'', N''Exchange2013.SharedMailboxes'', 30, 2, CAST(0 AS bit)),
    (430, 44, NULL, NULL, 1, N''Disk Storage Space (Mb)'', N''EnterpriseStorage.DiskStorageSpace'', 1, 2, CAST(0 AS bit)),
    (431, 44, NULL, NULL, 1, N''Number of Root Folders'', N''EnterpriseStorage.Folders'', 1, 2, CAST(0 AS bit)),
    (447, 61, NULL, NULL, NULL, N''Enable Spam Filter'', N''Filters.Enable'', 1, 1, CAST(0 AS bit)),
    (448, 61, NULL, NULL, NULL, N''Enable Per-Mailbox Login'', N''Filters.EnableEmailUsers'', 2, 1, CAST(0 AS bit)),
    (450, 45, NULL, NULL, 1, N''Remote Desktop Users'', N''RDS.Users'', 1, 2, CAST(0 AS bit)),
    (451, 45, NULL, NULL, 1, N''Remote Desktop Servers'', N''RDS.Servers'', 2, 2, CAST(0 AS bit)),
    (452, 45, NULL, NULL, NULL, N''Disable user from adding server'', N''RDS.DisableUserAddServer'', 3, 1, CAST(0 AS bit)),
    (453, 45, NULL, NULL, NULL, N''Disable user from removing server'', N''RDS.DisableUserDeleteServer'', 3, 1, CAST(0 AS bit)),
    (460, 21, NULL, NULL, NULL, N''Max Database Size, MB'', N''HostedCRM.MaxDatabaseSize'', 5, 3, CAST(0 AS bit)),
    (461, 21, NULL, NULL, 1, N''Limited licenses per organization'', N''HostedCRM.LimitedUsers'', 3, 3, CAST(0 AS bit)),
    (462, 21, NULL, NULL, 1, N''ESS licenses per organization'', N''HostedCRM.ESSUsers'', 4, 3, CAST(0 AS bit)),
    (463, 24, NULL, NULL, NULL, N''CRM Organization'', N''HostedCRM2013.Organization'', 1, 1, CAST(0 AS bit)),
    (464, 24, NULL, NULL, NULL, N''Max Database Size, MB'', N''HostedCRM2013.MaxDatabaseSize'', 5, 3, CAST(0 AS bit)),
    (465, 24, NULL, NULL, 1, N''Essential licenses per organization'', N''HostedCRM2013.EssentialUsers'', 2, 3, CAST(0 AS bit)),
    (466, 24, NULL, NULL, 1, N''Basic licenses per organization'', N''HostedCRM2013.BasicUsers'', 3, 3, CAST(0 AS bit)),
    (467, 24, NULL, NULL, 1, N''Professional licenses per organization'', N''HostedCRM2013.ProfessionalUsers'', 4, 3, CAST(0 AS bit)),
    (468, 45, NULL, NULL, NULL, N''Use Drive Maps'', N''EnterpriseStorage.DriveMaps'', 2, 1, CAST(0 AS bit)),
    (472, 46, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2014.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (473, 46, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2014.Backup'', 5, 1, CAST(0 AS bit)),
    (474, 46, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2014.Restore'', 6, 1, CAST(0 AS bit)),
    (475, 46, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2014.Truncate'', 7, 1, CAST(0 AS bit)),
    (476, 46, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2014.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (491, 45, NULL, NULL, 1, N''Remote Desktop Servers'', N''RDS.Collections'', 2, 2, CAST(0 AS bit)),
    (495, 13, NULL, NULL, 1, N''Deleted Users'', N''HostedSolution.DeletedUsers'', 6, 2, CAST(0 AS bit)),
    (496, 13, NULL, NULL, 1, N''Deleted Users Backup Storage Space, Mb'', N''HostedSolution.DeletedUsersBackupStorageSpace'', 6, 2, CAST(0 AS bit)),
    (551, 73, NULL, NULL, NULL, N''Max site storage, MB'', N''HostedSharePointEnterprise.MaxStorage'', 2, 3, CAST(0 AS bit)),
    (552, 73, NULL, NULL, NULL, N''Use shared SSL Root'', N''HostedSharePointEnterprise.UseSharedSSL'', 3, 1, CAST(0 AS bit)),
    (554, 33, NULL, NULL, NULL, N''Allow user to create VPS'', N''VPS2012.ManagingAllowed'', 2, 1, CAST(0 AS bit)),
    (555, 33, NULL, NULL, NULL, N''Number of CPU cores'', N''VPS2012.CpuNumber'', 3, 2, CAST(0 AS bit)),
    (556, 33, NULL, NULL, NULL, N''Boot from CD allowed'', N''VPS2012.BootCdAllowed'', 7, 1, CAST(0 AS bit)),
    (557, 33, NULL, NULL, NULL, N''Boot from CD'', N''VPS2012.BootCdEnabled'', 8, 1, CAST(0 AS bit)),
    (558, 33, NULL, NULL, NULL, N''RAM size, MB'', N''VPS2012.Ram'', 4, 2, CAST(0 AS bit)),
    (559, 33, NULL, NULL, NULL, N''Hard Drive size, GB'', N''VPS2012.Hdd'', 5, 2, CAST(0 AS bit)),
    (560, 33, NULL, NULL, NULL, N''DVD drive'', N''VPS2012.DvdEnabled'', 6, 1, CAST(0 AS bit));
    INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (561, 33, NULL, NULL, NULL, N''External Network'', N''VPS2012.ExternalNetworkEnabled'', 10, 1, CAST(0 AS bit)),
    (562, 33, NULL, NULL, NULL, N''Number of External IP addresses'', N''VPS2012.ExternalIPAddressesNumber'', 11, 2, CAST(0 AS bit)),
    (563, 33, NULL, NULL, NULL, N''Private Network'', N''VPS2012.PrivateNetworkEnabled'', 13, 1, CAST(0 AS bit)),
    (564, 33, NULL, NULL, NULL, N''Number of Private IP addresses per VPS'', N''VPS2012.PrivateIPAddressesNumber'', 14, 3, CAST(0 AS bit)),
    (565, 33, NULL, NULL, NULL, N''Number of Snaphots'', N''VPS2012.SnapshotsNumber'', 9, 3, CAST(0 AS bit)),
    (566, 33, NULL, NULL, NULL, N''Allow user to Start, Turn off and Shutdown VPS'', N''VPS2012.StartShutdownAllowed'', 15, 1, CAST(0 AS bit)),
    (567, 33, NULL, NULL, NULL, N''Allow user to Pause, Resume VPS'', N''VPS2012.PauseResumeAllowed'', 16, 1, CAST(0 AS bit)),
    (568, 33, NULL, NULL, NULL, N''Allow user to Reboot VPS'', N''VPS2012.RebootAllowed'', 17, 1, CAST(0 AS bit)),
    (569, 33, NULL, NULL, NULL, N''Allow user to Reset VPS'', N''VPS2012.ResetAlowed'', 18, 1, CAST(0 AS bit)),
    (570, 33, NULL, NULL, NULL, N''Allow user to Re-install VPS'', N''VPS2012.ReinstallAllowed'', 19, 1, CAST(0 AS bit)),
    (571, 33, NULL, NULL, NULL, N''Monthly bandwidth, GB'', N''VPS2012.Bandwidth'', 12, 2, CAST(0 AS bit)),
    (572, 33, NULL, NULL, NULL, N''Allow user to Replication'', N''VPS2012.ReplicationEnabled'', 20, 1, CAST(0 AS bit)),
    (575, 50, NULL, NULL, NULL, N''Max Database Size'', N''MariaDB.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (576, 50, NULL, NULL, NULL, N''Database Backups'', N''MariaDB.Backup'', 5, 1, CAST(0 AS bit)),
    (577, 50, NULL, NULL, NULL, N''Database Restores'', N''MariaDB.Restore'', 6, 1, CAST(0 AS bit)),
    (578, 50, NULL, NULL, NULL, N''Database Truncate'', N''MariaDB.Truncate'', 7, 1, CAST(0 AS bit)),
    (579, 50, NULL, NULL, NULL, N''Max Log Size'', N''MariaDB.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (581, 52, NULL, NULL, NULL, N''Phone Numbers'', N''SfB.PhoneNumbers'', 12, 2, CAST(0 AS bit)),
    (582, 52, NULL, NULL, 1, N''Users'', N''SfB.Users'', 1, 2, CAST(0 AS bit)),
    (583, 52, NULL, NULL, NULL, N''Allow Federation'', N''SfB.Federation'', 2, 1, CAST(0 AS bit)),
    (584, 52, NULL, NULL, NULL, N''Allow Conferencing'', N''SfB.Conferencing'', 3, 1, CAST(0 AS bit)),
    (585, 52, NULL, NULL, NULL, N''Maximum Conference Particiapants'', N''SfB.MaxParticipants'', 4, 3, CAST(0 AS bit)),
    (586, 52, NULL, NULL, NULL, N''Allow Video in Conference'', N''SfB.AllowVideo'', 5, 1, CAST(0 AS bit)),
    (587, 52, NULL, NULL, NULL, N''Allow EnterpriseVoice'', N''SfB.EnterpriseVoice'', 6, 1, CAST(0 AS bit)),
    (588, 52, NULL, NULL, NULL, N''Number of Enterprise Voice Users'', N''SfB.EVUsers'', 7, 2, CAST(0 AS bit)),
    (589, 52, NULL, NULL, NULL, N''Allow National Calls'', N''SfB.EVNational'', 8, 1, CAST(0 AS bit)),
    (590, 52, NULL, NULL, NULL, N''Allow Mobile Calls'', N''SfB.EVMobile'', 9, 1, CAST(0 AS bit)),
    (591, 52, NULL, NULL, NULL, N''Allow International Calls'', N''SfB.EVInternational'', 10, 1, CAST(0 AS bit)),
    (592, 52, NULL, NULL, NULL, N''Enable Plans Editing'', N''SfB.EnablePlansEditing'', 11, 1, CAST(0 AS bit)),
    (674, 167, NULL, NULL, NULL, N''Allow user to create VPS'', N''PROXMOX.ManagingAllowed'', 2, 1, CAST(0 AS bit)),
    (675, 167, NULL, NULL, NULL, N''Number of CPU cores'', N''PROXMOX.CpuNumber'', 3, 3, CAST(0 AS bit)),
    (676, 167, NULL, NULL, NULL, N''Boot from CD allowed'', N''PROXMOX.BootCdAllowed'', 7, 1, CAST(0 AS bit)),
    (677, 167, NULL, NULL, NULL, N''Boot from CD'', N''PROXMOX.BootCdEnabled'', 8, 1, CAST(0 AS bit)),
    (678, 167, NULL, NULL, NULL, N''RAM size, MB'', N''PROXMOX.Ram'', 4, 2, CAST(0 AS bit)),
    (679, 167, NULL, NULL, NULL, N''Hard Drive size, GB'', N''PROXMOX.Hdd'', 5, 2, CAST(0 AS bit)),
    (680, 167, NULL, NULL, NULL, N''DVD drive'', N''PROXMOX.DvdEnabled'', 6, 1, CAST(0 AS bit)),
    (681, 167, NULL, NULL, NULL, N''External Network'', N''PROXMOX.ExternalNetworkEnabled'', 10, 1, CAST(0 AS bit)),
    (682, 167, NULL, NULL, NULL, N''Number of External IP addresses'', N''PROXMOX.ExternalIPAddressesNumber'', 11, 2, CAST(0 AS bit)),
    (683, 167, NULL, NULL, NULL, N''Private Network'', N''PROXMOX.PrivateNetworkEnabled'', 13, 1, CAST(0 AS bit)),
    (684, 167, NULL, NULL, NULL, N''Number of Private IP addresses per VPS'', N''PROXMOX.PrivateIPAddressesNumber'', 14, 3, CAST(0 AS bit)),
    (685, 167, NULL, NULL, NULL, N''Number of Snaphots'', N''PROXMOX.SnapshotsNumber'', 9, 3, CAST(0 AS bit)),
    (686, 167, NULL, NULL, NULL, N''Allow user to Start, Turn off and Shutdown VPS'', N''PROXMOX.StartShutdownAllowed'', 15, 1, CAST(0 AS bit));
    INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (687, 167, NULL, NULL, NULL, N''Allow user to Pause, Resume VPS'', N''PROXMOX.PauseResumeAllowed'', 16, 1, CAST(0 AS bit)),
    (688, 167, NULL, NULL, NULL, N''Allow user to Reboot VPS'', N''PROXMOX.RebootAllowed'', 17, 1, CAST(0 AS bit)),
    (689, 167, NULL, NULL, NULL, N''Allow user to Reset VPS'', N''PROXMOX.ResetAlowed'', 18, 1, CAST(0 AS bit)),
    (690, 167, NULL, NULL, NULL, N''Allow user to Re-install VPS'', N''PROXMOX.ReinstallAllowed'', 19, 1, CAST(0 AS bit)),
    (691, 167, NULL, NULL, NULL, N''Monthly bandwidth, GB'', N''PROXMOX.Bandwidth'', 12, 2, CAST(0 AS bit)),
    (692, 167, NULL, NULL, NULL, N''Allow user to Replication'', N''PROXMOX.ReplicationEnabled'', 20, 1, CAST(0 AS bit)),
    (703, 71, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2016.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (704, 71, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2016.Backup'', 5, 1, CAST(0 AS bit)),
    (705, 71, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2016.Restore'', 6, 1, CAST(0 AS bit)),
    (706, 71, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2016.Truncate'', 7, 1, CAST(0 AS bit)),
    (707, 71, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2016.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (713, 72, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2017.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (714, 72, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2017.Backup'', 5, 1, CAST(0 AS bit)),
    (715, 72, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2017.Restore'', 6, 1, CAST(0 AS bit)),
    (716, 72, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2017.Truncate'', 7, 1, CAST(0 AS bit)),
    (717, 72, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2017.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (723, 74, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2019.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (724, 74, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2019.Backup'', 5, 1, CAST(0 AS bit)),
    (725, 74, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2019.Restore'', 6, 1, CAST(0 AS bit)),
    (726, 74, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2019.Truncate'', 7, 1, CAST(0 AS bit)),
    (727, 74, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2019.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (728, 33, NULL, NULL, NULL, N''Number of Private Network VLANs'', N''VPS2012.PrivateVLANsNumber'', 14, 2, CAST(0 AS bit)),
    (729, 12, NULL, NULL, NULL, N''Automatic Replies via SolidCP Allowed'', N''Exchange2013.AutoReply'', 32, 1, CAST(0 AS bit)),
    (730, 33, NULL, NULL, NULL, N''Additional Hard Drives per VPS'', N''VPS2012.AdditionalVhdCount'', 6, 3, CAST(0 AS bit)),
    (731, 12, NULL, NULL, 1, N''Journaling Mailboxes per Organization'', N''Exchange2013.JournalingMailboxes'', 31, 2, CAST(0 AS bit)),
    (734, 75, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2022.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (735, 75, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2022.Backup'', 5, 1, CAST(0 AS bit)),
    (736, 75, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2022.Restore'', 6, 1, CAST(0 AS bit)),
    (737, 75, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2022.Truncate'', 7, 1, CAST(0 AS bit)),
    (738, 75, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2022.MaxLogSize'', 4, 3, CAST(0 AS bit)),
    (751, 33, NULL, NULL, NULL, N''Number of DMZ IP addresses per VPS'', N''VPS2012.DMZIPAddressesNumber'', 23, 3, CAST(0 AS bit)),
    (752, 33, NULL, NULL, NULL, N''Number of DMZ Network VLANs'', N''VPS2012.DMZVLANsNumber'', 24, 2, CAST(0 AS bit)),
    (753, 7, NULL, NULL, NULL, N''Allow editing TTL in DNS Editor'', N''DNS.EditTTL'', 2, 1, CAST(0 AS bit)),
    (762, 76, NULL, NULL, NULL, N''Max Database Size'', N''MsSQL2025.MaxDatabaseSize'', 3, 3, CAST(0 AS bit)),
    (763, 76, NULL, NULL, NULL, N''Database Backups'', N''MsSQL2025.Backup'', 5, 1, CAST(0 AS bit)),
    (764, 76, NULL, NULL, NULL, N''Database Restores'', N''MsSQL2025.Restore'', 6, 1, CAST(0 AS bit)),
    (765, 76, NULL, NULL, NULL, N''Database Truncate'', N''MsSQL2025.Truncate'', 7, 1, CAST(0 AS bit)),
    (766, 76, NULL, NULL, NULL, N''Max Log Size'', N''MsSQL2025.MaxLogSize'', 4, 3, CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'QuotaID', N'GroupID', N'HideQuota', N'ItemTypeID', N'PerOrganization', N'QuotaDescription', N'QuotaName', N'QuotaOrder', N'QuotaTypeID', N'ServiceQuota') AND [object_id] = OBJECT_ID(N'[Quotas]'))
        SET IDENTITY_INSERT [Quotas] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RecordID', N'GroupID', N'MXPriority', N'RecordData', N'RecordName', N'RecordOrder', N'RecordType') AND [object_id] = OBJECT_ID(N'[ResourceGroupDnsRecords]'))
        SET IDENTITY_INSERT [ResourceGroupDnsRecords] ON;
    EXEC(N'INSERT INTO [ResourceGroupDnsRecords] ([RecordID], [GroupID], [MXPriority], [RecordData], [RecordName], [RecordOrder], [RecordType])
    VALUES (1, 2, 0, N''[IP]'', N'''', 1, ''A''),
    (2, 2, 0, N''[IP]'', N''*'', 2, ''A''),
    (3, 2, 0, N''[IP]'', N''www'', 3, ''A''),
    (4, 3, 0, N''[IP]'', N''ftp'', 1, ''A''),
    (5, 4, 0, N''[IP]'', N''mail'', 1, ''A''),
    (6, 4, 0, N''[IP]'', N''mail2'', 2, ''A''),
    (7, 4, 10, N''mail.[DOMAIN_NAME]'', N'''', 3, ''MX''),
    (9, 4, 21, N''mail2.[DOMAIN_NAME]'', N'''', 4, ''MX''),
    (10, 5, 0, N''[IP]'', N''mssql'', 1, ''A''),
    (11, 6, 0, N''[IP]'', N''mysql'', 1, ''A''),
    (12, 8, 0, N''[IP]'', N''stats'', 1, ''A''),
    (13, 4, 0, N''v=spf1 a mx -all'', N'''', 5, ''TXT''),
    (14, 12, 0, N''[IP]'', N''smtp'', 1, ''A''),
    (15, 12, 10, N''smtp.[DOMAIN_NAME]'', N'''', 2, ''MX''),
    (16, 12, 0, N'''', N''autodiscover'', 3, ''CNAME''),
    (17, 12, 0, N'''', N''owa'', 4, ''CNAME'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RecordID', N'GroupID', N'MXPriority', N'RecordData', N'RecordName', N'RecordOrder', N'RecordType') AND [object_id] = OBJECT_ID(N'[ResourceGroupDnsRecords]'))
        SET IDENTITY_INSERT [ResourceGroupDnsRecords] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ParameterID', N'TaskID', N'DataTypeID', N'DefaultValue', N'ParameterOrder') AND [object_id] = OBJECT_ID(N'[ScheduleTaskParameters]'))
        SET IDENTITY_INSERT [ScheduleTaskParameters] ON;
    EXEC(N'INSERT INTO [ScheduleTaskParameters] ([ParameterID], [TaskID], [DataTypeID], [DefaultValue], [ParameterOrder])
    VALUES (N''AUDIT_LOG_DATE'', N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', N''List'', N''today=Today;yesterday=Yesterday;schedule=Schedule'', 5),
    (N''AUDIT_LOG_SEVERITY'', N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', N''List'', N''-1=All;0=Information;1=Warning;2=Error'', 2),
    (N''AUDIT_LOG_SOURCE'', N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', N''List'', N'''', 3),
    (N''AUDIT_LOG_TASK'', N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', N''List'', N'''', 4),
    (N''MAIL_TO'', N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', N''String'', NULL, 1),
    (N''SHOW_EXECUTION_LOG'', N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', N''List'', N''0=No;1=Yes'', 6),
    (N''BACKUP_FILE_NAME'', N''SCHEDULE_TASK_BACKUP'', N''String'', N'''', 1),
    (N''DELETE_TEMP_BACKUP'', N''SCHEDULE_TASK_BACKUP'', N''Boolean'', N''true'', 1),
    (N''STORE_PACKAGE_FOLDER'', N''SCHEDULE_TASK_BACKUP'', N''String'', N''\'', 1),
    (N''STORE_PACKAGE_ID'', N''SCHEDULE_TASK_BACKUP'', N''String'', N'''', 1),
    (N''STORE_SERVER_FOLDER'', N''SCHEDULE_TASK_BACKUP'', N''String'', N'''', 1),
    (N''BACKUP_FOLDER'', N''SCHEDULE_TASK_BACKUP_DATABASE'', N''String'', N''\backups'', 3),
    (N''BACKUP_NAME'', N''SCHEDULE_TASK_BACKUP_DATABASE'', N''String'', N''database_backup.bak'', 4),
    (N''DATABASE_GROUP'', N''SCHEDULE_TASK_BACKUP_DATABASE'', N''List'', N''MsSQL2014=SQL Server 2014;MsSQL2016=SQL Server 2016;MsSQL2017=SQL Server 2017;MsSQL2019=SQL Server 2019;MsSQL2022=SQL Server 2022;MySQL5=MySQL 5.0;MariaDB=MariaDB'', 1),
    (N''DATABASE_NAME'', N''SCHEDULE_TASK_BACKUP_DATABASE'', N''String'', N'''', 2),
    (N''ZIP_BACKUP'', N''SCHEDULE_TASK_BACKUP_DATABASE'', N''List'', N''true=Yes;false=No'', 5),
    (N''MAIL_BODY'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''MultiString'', N'''', 10),
    (N''MAIL_FROM'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', N''admin@mysite.com'', 7),
    (N''MAIL_SUBJECT'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', N''Web Site is unavailable'', 9),
    (N''MAIL_TO'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', N''admin@mysite.com'', 8),
    (N''PASSWORD'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', NULL, 3),
    (N''RESPONSE_CONTAIN'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', NULL, 5),
    (N''RESPONSE_DOESNT_CONTAIN'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', NULL, 6),
    (N''RESPONSE_STATUS'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', N''500'', 4),
    (N''URL'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', N''http://'', 1),
    (N''USE_RESPONSE_CONTAIN'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''Boolean'', N''false'', 1),
    (N''USE_RESPONSE_DOESNT_CONTAIN'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''Boolean'', N''false'', 1),
    (N''USE_RESPONSE_STATUS'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''Boolean'', N''false'', 1),
    (N''USERNAME'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''String'', NULL, 2),
    (N''DAYS_BEFORE'', N''SCHEDULE_TASK_DOMAIN_EXPIRATION'', N''String'', NULL, 1),
    (N''ENABLE_NOTIFICATION'', N''SCHEDULE_TASK_DOMAIN_EXPIRATION'', N''Boolean'', N''false'', 3),
    (N''INCLUDE_NONEXISTEN_DOMAINS'', N''SCHEDULE_TASK_DOMAIN_EXPIRATION'', N''Boolean'', N''false'', 4),
    (N''MAIL_TO'', N''SCHEDULE_TASK_DOMAIN_EXPIRATION'', N''String'', NULL, 2),
    (N''DNS_SERVERS'', N''SCHEDULE_TASK_DOMAIN_LOOKUP'', N''String'', NULL, 1),
    (N''MAIL_TO'', N''SCHEDULE_TASK_DOMAIN_LOOKUP'', N''String'', NULL, 2),
    (N''PAUSE_BETWEEN_QUERIES'', N''SCHEDULE_TASK_DOMAIN_LOOKUP'', N''String'', N''100'', 4),
    (N''SERVER_NAME'', N''SCHEDULE_TASK_DOMAIN_LOOKUP'', N''String'', N'''', 3),
    (N''FILE_PATH'', N''SCHEDULE_TASK_FTP_FILES'', N''String'', N''\'', 1),
    (N''FTP_FOLDER'', N''SCHEDULE_TASK_FTP_FILES'', N''String'', NULL, 5),
    (N''FTP_PASSWORD'', N''SCHEDULE_TASK_FTP_FILES'', N''String'', NULL, 4),
    (N''FTP_SERVER'', N''SCHEDULE_TASK_FTP_FILES'', N''String'', N''ftp.myserver.com'', 2),
    (N''FTP_USERNAME'', N''SCHEDULE_TASK_FTP_FILES'', N''String'', NULL, 3);
    INSERT INTO [ScheduleTaskParameters] ([ParameterID], [TaskID], [DataTypeID], [DefaultValue], [ParameterOrder])
    VALUES (N''CRM_REPORT'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''Boolean'', N''true'', 6),
    (N''EMAIL'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''String'', NULL, 1),
    (N''EXCHANGE_REPORT'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''Boolean'', N''true'', 2),
    (N''LYNC_REPORT'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''Boolean'', N''true'', 4),
    (N''ORGANIZATION_REPORT'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''Boolean'', N''true'', 7),
    (N''SFB_REPORT'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''Boolean'', N''true'', 5),
    (N''SHAREPOINT_REPORT'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''Boolean'', N''true'', 3),
    (N''MARIADB_OVERUSED'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''Boolean'', N''true'', 1),
    (N''MSSQL_OVERUSED'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''Boolean'', N''true'', 1),
    (N''MYSQL_OVERUSED'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''Boolean'', N''true'', 1),
    (N''OVERUSED_MAIL_BCC'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''OVERUSED_MAIL_BODY'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''OVERUSED_MAIL_FROM'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''OVERUSED_MAIL_SUBJECT'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''OVERUSED_USAGE_THRESHOLD'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N''100'', 1),
    (N''SEND_OVERUSED_EMAIL'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''Boolean'', N''true'', 1),
    (N''SEND_WARNING_EMAIL'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''Boolean'', N''true'', 1),
    (N''WARNING_MAIL_BCC'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''WARNING_MAIL_BODY'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''WARNING_MAIL_FROM'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''WARNING_MAIL_SUBJECT'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N'''', 1),
    (N''WARNING_USAGE_THRESHOLD'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''String'', N''80'', 1),
    (N''EXECUTABLE_PARAMS'', N''SCHEDULE_TASK_RUN_SYSTEM_COMMAND'', N''String'', N'''', 3),
    (N''EXECUTABLE_PATH'', N''SCHEDULE_TASK_RUN_SYSTEM_COMMAND'', N''String'', N''Executable.exe'', 2),
    (N''SERVER_NAME'', N''SCHEDULE_TASK_RUN_SYSTEM_COMMAND'', N''String'', NULL, 1),
    (N''MAIL_BODY'', N''SCHEDULE_TASK_SEND_MAIL'', N''MultiString'', NULL, 4),
    (N''MAIL_FROM'', N''SCHEDULE_TASK_SEND_MAIL'', N''String'', NULL, 1),
    (N''MAIL_SUBJECT'', N''SCHEDULE_TASK_SEND_MAIL'', N''String'', NULL, 3),
    (N''MAIL_TO'', N''SCHEDULE_TASK_SEND_MAIL'', N''String'', NULL, 2),
    (N''BANDWIDTH_OVERUSED'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''Boolean'', N''true'', 1),
    (N''DISKSPACE_OVERUSED'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''Boolean'', N''true'', 1),
    (N''SEND_SUSPENSION_EMAIL'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''Boolean'', N''true'', 1),
    (N''SEND_WARNING_EMAIL'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''Boolean'', N''true'', 1),
    (N''SUSPEND_OVERUSED'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''Boolean'', N''true'', 1),
    (N''SUSPENSION_MAIL_BCC'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1),
    (N''SUSPENSION_MAIL_BODY'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1),
    (N''SUSPENSION_MAIL_FROM'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1),
    (N''SUSPENSION_MAIL_SUBJECT'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1),
    (N''SUSPENSION_USAGE_THRESHOLD'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', N''100'', 1),
    (N''WARNING_MAIL_BCC'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1),
    (N''WARNING_MAIL_BODY'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1),
    (N''WARNING_MAIL_FROM'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1);
    INSERT INTO [ScheduleTaskParameters] ([ParameterID], [TaskID], [DataTypeID], [DefaultValue], [ParameterOrder])
    VALUES (N''WARNING_MAIL_SUBJECT'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', NULL, 1),
    (N''WARNING_USAGE_THRESHOLD'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''String'', N''80'', 1),
    (N''DAYS_BEFORE_EXPIRATION'', N''SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION'', N''String'', NULL, 1),
    (N''FOLDER'', N''SCHEDULE_TASK_ZIP_FILES'', N''String'', NULL, 1),
    (N''ZIP_FILE'', N''SCHEDULE_TASK_ZIP_FILES'', N''String'', N''\archive.zip'', 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ParameterID', N'TaskID', N'DataTypeID', N'DefaultValue', N'ParameterOrder') AND [object_id] = OBJECT_ID(N'[ScheduleTaskParameters]'))
        SET IDENTITY_INSERT [ScheduleTaskParameters] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ConfigurationID', N'TaskID', N'Description', N'Environment') AND [object_id] = OBJECT_ID(N'[ScheduleTaskViewConfiguration]'))
        SET IDENTITY_INSERT [ScheduleTaskViewConfiguration] ON;
    EXEC(N'INSERT INTO [ScheduleTaskViewConfiguration] ([ConfigurationID], [TaskID], [Description], [Environment])
    VALUES (N''ASP_NET'', N''SCHEDULE_TASK_ACTIVATE_PAID_INVOICES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_AUDIT_LOG_REPORT'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/AuditLogReportView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_BACKUP'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/Backup.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_BACKUP_DATABASE'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/BackupDatabase.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_CHECK_WEBSITE'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/CheckWebsite.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_DOMAIN_EXPIRATION'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/DomainExpirationView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_DOMAIN_LOOKUP'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/DomainLookupView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_FTP_FILES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/SendFilesViaFtp.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_GENERATE_INVOICES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_HOSTED_SOLUTION_REPORT'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/HostedSolutionReport.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/NotifyOverusedDatabases.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_RUN_PAYMENT_QUEUE'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_RUN_SYSTEM_COMMAND'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/ExecuteSystemCommand.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_SEND_MAIL'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/SendEmailNotification.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/EmptyView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_SUSPEND_PACKAGES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/SuspendOverusedSpaces.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_USER_PASSWORD_EXPIRATION_NOTIFICATION'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/UserPasswordExpirationNotificationView.ascx'', N''ASP.NET''),
    (N''ASP_NET'', N''SCHEDULE_TASK_ZIP_FILES'', N''~/DesktopModules/SolidCP/ScheduleTaskControls/ZipFiles.ascx'', N''ASP.NET'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ConfigurationID', N'TaskID', N'Description', N'Environment') AND [object_id] = OBJECT_ID(N'[ScheduleTaskViewConfiguration]'))
        SET IDENTITY_INSERT [ScheduleTaskViewConfiguration] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (2, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''HomeFolder'', CAST(1 AS bit), 1, CAST(0 AS bit), CAST(0 AS bit), N''SolidCP.Providers.OS.HomeFolder, SolidCP.Providers.Base'', 15)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (5, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2000Database'', CAST(1 AS bit), 5, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 9),
    (6, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2000User'', CAST(1 AS bit), 5, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 10),
    (7, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MySQL4Database'', CAST(1 AS bit), 6, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 13),
    (8, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MySQL4User'', CAST(1 AS bit), 6, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 14),
    (9, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''FTPAccount'', CAST(1 AS bit), 3, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.FTP.FtpAccount, SolidCP.Providers.Base'', 3),
    (10, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''WebSite'', CAST(1 AS bit), 2, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Web.WebSite, SolidCP.Providers.Base'', 2),
    (11, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''MailDomain'', CAST(1 AS bit), 4, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Mail.MailDomain, SolidCP.Providers.Base'', 8)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName])
    VALUES (12, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''DNSZone'', CAST(1 AS bit), 7, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''SolidCP.Providers.DNS.DnsZone, SolidCP.Providers.Base'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (13, CAST(0 AS bit), CAST(0 AS bit), N''Domain'', CAST(0 AS bit), 1, CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.OS.Domain, SolidCP.Providers.Base'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (14, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''StatisticsSite'', CAST(1 AS bit), 8, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Statistics.StatsSite, SolidCP.Providers.Base'', 17)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (15, CAST(0 AS bit), CAST(1 AS bit), N''MailAccount'', CAST(0 AS bit), 4, CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Mail.MailAccount, SolidCP.Providers.Base'', 4),
    (16, CAST(0 AS bit), CAST(0 AS bit), N''MailAlias'', CAST(0 AS bit), 4, CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Mail.MailAlias, SolidCP.Providers.Base'', 5),
    (17, CAST(0 AS bit), CAST(0 AS bit), N''MailList'', CAST(0 AS bit), 4, CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Mail.MailList, SolidCP.Providers.Base'', 7),
    (18, CAST(0 AS bit), CAST(0 AS bit), N''MailGroup'', CAST(0 AS bit), 4, CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Mail.MailGroup, SolidCP.Providers.Base'', 6)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (20, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''ODBCDSN'', CAST(1 AS bit), 1, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.OS.SystemDSN, SolidCP.Providers.Base'', 22),
    (21, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2005Database'', CAST(1 AS bit), 10, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 11),
    (22, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2005User'', CAST(1 AS bit), 10, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 12),
    (23, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MySQL5Database'', CAST(1 AS bit), 11, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 15),
    (24, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MySQL5User'', CAST(1 AS bit), 11, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 16)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (25, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''SharedSSLFolder'', CAST(1 AS bit), 2, CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Web.SharedSSLFolder, SolidCP.Providers.Base'', 21)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName])
    VALUES (28, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''SecondaryDNSZone'', CAST(1 AS bit), 7, CAST(0 AS bit), CAST(1 AS bit), N''SolidCP.Providers.DNS.SecondaryDnsZone, SolidCP.Providers.Base'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (29, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''Organization'', CAST(1 AS bit), 13, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.HostedSolution.Organization, SolidCP.Providers.Base'', 1),
    (30, CAST(1 AS bit), NULL, NULL, N''OrganizationDomain'', NULL, 13, NULL, NULL, N''SolidCP.Providers.HostedSolution.OrganizationDomain, SolidCP.Providers.Base'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (31, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2008Database'', CAST(1 AS bit), 22, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (32, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2008User'', CAST(1 AS bit), 22, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (33, CAST(0 AS bit), CAST(0 AS bit), N''VirtualMachine'', CAST(1 AS bit), 30, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base'', 1),
    (34, CAST(0 AS bit), CAST(0 AS bit), N''VirtualSwitch'', CAST(1 AS bit), 30, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base'', 2),
    (35, CAST(0 AS bit), CAST(0 AS bit), N''VMInfo'', CAST(1 AS bit), 40, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VMInfo, SolidCP.Providers.Base'', 1),
    (36, CAST(0 AS bit), CAST(0 AS bit), N''VirtualSwitch'', CAST(1 AS bit), 40, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base'', 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (37, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2012Database'', CAST(1 AS bit), 23, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (38, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2012User'', CAST(1 AS bit), 23, CAST(1 AS bit), CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1),
    (39, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2014Database'', CAST(1 AS bit), 46, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (40, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2014User'', CAST(1 AS bit), 46, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (41, CAST(0 AS bit), CAST(0 AS bit), N''VirtualMachine'', CAST(1 AS bit), 33, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base'', 1),
    (42, CAST(0 AS bit), CAST(0 AS bit), N''VirtualSwitch'', CAST(1 AS bit), 33, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base'', 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (71, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2016Database'', CAST(1 AS bit), 71, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (72, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2016User'', CAST(1 AS bit), 71, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1),
    (73, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2017Database'', CAST(1 AS bit), 72, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (74, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2017User'', CAST(1 AS bit), 72, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1),
    (75, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MySQL8Database'', CAST(1 AS bit), 90, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 18),
    (76, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MySQL8User'', CAST(1 AS bit), 90, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 19),
    (77, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2019Database'', CAST(1 AS bit), 74, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (78, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2019User'', CAST(1 AS bit), 74, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1),
    (79, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MsSQL2022Database'', CAST(1 AS bit), 75, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (80, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MsSQL2022User'', CAST(1 AS bit), 75, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1),
    (90, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MySQL9Database'', CAST(1 AS bit), 91, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 20),
    (91, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MySQL9User'', CAST(1 AS bit), 91, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 21)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (143, CAST(0 AS bit), CAST(0 AS bit), N''VirtualMachine'', CAST(1 AS bit), 167, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VirtualMachine, SolidCP.Providers.Base'', 1),
    (144, CAST(0 AS bit), CAST(0 AS bit), N''VirtualSwitch'', CAST(1 AS bit), 167, CAST(1 AS bit), CAST(1 AS bit), N''SolidCP.Providers.Virtualization.VirtualSwitch, SolidCP.Providers.Base'', 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] ON;
    EXEC(N'INSERT INTO [ServiceItemTypes] ([ItemTypeID], [Backupable], [CalculateBandwidth], [CalculateDiskspace], [DisplayName], [Disposable], [GroupID], [Importable], [Searchable], [Suspendable], [TypeName], [TypeOrder])
    VALUES (200, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''SharePointFoundationSiteCollection'', CAST(1 AS bit), 20, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.SharePoint.SharePointSiteCollection, SolidCP.Providers.Base'', 25),
    (202, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''MariaDBDatabase'', CAST(1 AS bit), 50, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlDatabase, SolidCP.Providers.Base'', 1),
    (203, CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''MariaDBUser'', CAST(1 AS bit), 50, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.Database.SqlUser, SolidCP.Providers.Base'', 1),
    (204, CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''SharePointEnterpriseSiteCollection'', CAST(1 AS bit), 73, CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), N''SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection, SolidCP.Providers.Base'', 100)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ItemTypeID', N'Backupable', N'CalculateBandwidth', N'CalculateDiskspace', N'DisplayName', N'Disposable', N'GroupID', N'Importable', N'Searchable', N'Suspendable', N'TypeName', N'TypeOrder') AND [object_id] = OBJECT_ID(N'[ServiceItemTypes]'))
        SET IDENTITY_INSERT [ServiceItemTypes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyName', N'SettingsName', N'UserID', N'PropertyValue') AND [object_id] = OBJECT_ID(N'[UserSettings]'))
        SET IDENTITY_INSERT [UserSettings] ON;
    EXEC(N'INSERT INTO [UserSettings] ([PropertyName], [SettingsName], [UserID], [PropertyValue])
    VALUES (N''CC'', N''AccountSummaryLetter'', 1, N''support@HostingCompany.com''),
    (N''EnableLetter'', N''AccountSummaryLetter'', 1, N''False''),
    (N''From'', N''AccountSummaryLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''AccountSummaryLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Account Summary Information</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; }'', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a name="top"></a>'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''	Hosting Account Information'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#Signup#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''New user account has been created and below you can find its summary information.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<h1>Control Panel URL</h1>'', nchar(13), nchar(10), N''<table>'', nchar(13), nchar(10), N''    <thead>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <th>Control Panel URL</th>'', nchar(13), nchar(10), N''            <th>Username</th>'', nchar(13), nchar(10), N''            <th>Password</th>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </thead>'', nchar(13), nchar(10), N''    <tbody>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>'', nchar(13), nchar(10), N''            <td>#user.Username#</td>'', nchar(13), nchar(10), N''            <td>#user.Password#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<h1>Hosting Spaces</h1>'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''    The following hosting spaces have been created under your account:'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''<ad:foreach collection="#Spaces#" var="Space" index="i">'', nchar(13), nchar(10), N''<h2>#Space.PackageName#</h2>'', nchar(13), nchar(10), N''<table>'', nchar(13), nchar(10), N''	<tbody>'', nchar(13), nchar(10), N''		<tr>'', nchar(13), nchar(10), N''			<td class="Label">Hosting Plan:</td>'', nchar(13), nchar(10), N''			<td>'', nchar(13), nchar(10), N''				<ad:if test="#not(isnull(Plans[Space.PlanId]))#">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>'', nchar(13), nchar(10), N''			</td>'', nchar(13), nchar(10), N''		</tr>'', nchar(13), nchar(10), N''		<ad:if test="#not(isnull(Plans[Space.PlanId]))#">'', nchar(13), nchar(10), N''		<tr>'', nchar(13), nchar(10), N''			<td class="Label">Purchase Date:</td>'', nchar(13), nchar(10), N''			<td>'', nchar(13), nchar(10), N''# Space.PurchaseDate#'', nchar(13), nchar(10), N''			</td>'', nchar(13), nchar(10), N''		</tr>'', nchar(13), nchar(10), N''		<tr>'', nchar(13), nchar(10), N''			<td class="Label">Disk Space, MB:</td>'', nchar(13), nchar(10), N''			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Diskspace" /></td>'', nchar(13), nchar(10), N''		</tr>'', nchar(13), nchar(10), N''		<tr>'', nchar(13), nchar(10), N''			<td class="Label">Bandwidth, MB/Month:</td>'', nchar(13), nchar(10), N''			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Bandwidth" /></td>'', nchar(13), nchar(10), N''		</tr>'', nchar(13), nchar(10), N''		<tr>'', nchar(13), nchar(10), N''			<td class="Label">Maximum Number of Domains:</td>'', nchar(13), nchar(10), N''			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Domains" /></td>'', nchar(13), nchar(10), CONCAT(CAST(N''		</tr>'' AS nvarchar(max)), nchar(13), nchar(10), N''		<tr>'', nchar(13), nchar(10), N''			<td class="Label">Maximum Number of Sub-Domains:</td>'', nchar(13), nchar(10), N''			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.SubDomains" /></td>'', nchar(13), nchar(10), N''		</tr>'', nchar(13), nchar(10), N''		</ad:if>'', nchar(13), nchar(10), N''	</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</ad:foreach>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#Signup#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards,<br />'', nchar(13), nchar(10), N''SolidCP.<br />'', nchar(13), nchar(10), N''Web Site: <a href="https://solidcp.com">https://solidcp.com</a><br />'', nchar(13), nchar(10), N''E-Mail: <a href="mailto:support@solidcp.com">support@solidcp.com</a>'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:template name="NumericQuota">'', nchar(13), nchar(10), N''	<ad:if test="#space.Quotas.ContainsKey(quota)#">'', nchar(13), nchar(10), N''		<ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>'', nchar(13), nchar(10), N''	<ad:else>'', nchar(13), nchar(10), N''		0'', nchar(13), nchar(10), N''	</ad:if>'', nchar(13), nchar(10), N''</ad:template>'', nchar(13), nchar(10), nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'', nchar(13), nchar(10), N''</html>''))),
    (N''Priority'', N''AccountSummaryLetter'', 1, N''Normal''),
    (N''Subject'', N''AccountSummaryLetter'', 1, N''<ad:if test="#Signup#">SolidCP  account has been created for<ad:else>SolidCP  account summary for</ad:if> #user.FirstName# #user.LastName#''),
    (N''TextBody'', N''AccountSummaryLetter'', 1, CONCAT(CAST(N''================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Hosting Account Information'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''<ad:if test="#Signup#">Hello #user.FirstName#,'', nchar(13), nchar(10), nchar(13), nchar(10), N''New user account has been created and below you can find its summary information.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Control Panel URL: https://panel.solidcp.com'', nchar(13), nchar(10), N''Username: #user.Username#'', nchar(13), nchar(10), N''Password: #user.Password#'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''Hosting Spaces'', nchar(13), nchar(10), N''=============='', nchar(13), nchar(10), N''The following hosting spaces have been created under your account:'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:foreach collection="#Spaces#" var="Space" index="i">'', nchar(13), nchar(10), N''=== #Space.PackageName# ==='', nchar(13), nchar(10), N''Hosting Plan: <ad:if test="#not(isnull(Plans[Space.PlanId]))#">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>'', nchar(13), nchar(10), N''<ad:if test="#not(isnull(Plans[Space.PlanId]))#">Purchase Date: #Space.PurchaseDate#'', nchar(13), nchar(10), N''Disk Space, MB: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Diskspace" />'', nchar(13), nchar(10), N''Bandwidth, MB/Month: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Bandwidth" />'', nchar(13), nchar(10), N''Maximum Number of Domains: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Domains" />'', nchar(13), nchar(10), N''Maximum Number of Sub-Domains: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.SubDomains" />'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), N''</ad:foreach>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#Signup#">If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards,'', nchar(13), nchar(10), N''SolidCP.'', nchar(13), nchar(10), N''Web Site: https://solidcp.com">'', nchar(13), nchar(10), N''E-Mail: support@solidcp.com'', nchar(13), nchar(10), N''</ad:if><ad:template name="NumericQuota"><ad:if test="#space.Quotas.ContainsKey(quota)#"><ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if><ad:else>0</ad:if></ad:template>'')),
    (N''Transform'', N''BandwidthXLST'', 1, CONCAT(CAST(N''<?xml version="1.0" encoding="UTF-8"?>'' AS nvarchar(max)), nchar(13), nchar(10), N''<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">'', nchar(13), nchar(10), N''<xsl:template match="/">'', nchar(13), nchar(10), N''  <html>'', nchar(13), nchar(10), N''  <body>'', nchar(13), nchar(10), N''  <img alt="Embedded Image" width="299" height="60" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASsAAAA8CAYAAAA+AJwjAAAACXBIWXMAAAsTAAALEwEAmpwYAAA4G2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMTQgNzkuMTUxNDgxLCAyMDEzLzAzLzEzLTEyOjA5OjE1ICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgICAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgICAgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIKICAgICAgICAgICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmV4aWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vZXhpZi8xLjAvIj4KICAgICAgICAgPHhtcDpDcmVhdG9yVG9vbD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC94bXA6Q3JlYXRvclRvb2w+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMTYtMDMtMDFUMTQ6NTE6NTgrMDE6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE2LTAzLTAxVDE0OjUxOjU4KzAxOjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8ZGM6Zm9ybWF0PmltYWdlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3A6Q29sb3JNb2RlPgogICAgICAgICA8eG1wTU06SW5zdGFuY2VJRD54bXAuaWlkOjZhNTdmMWYyLTgyZjYtMjk0MS1hYjFmLTNkOWQ0YjdmMTY2YjwveG1wTU06SW5zdGFuY2VJRD4KICAgICAgICAgPHhtcE1NOkRvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6NmE1N2YxZjItODJmNi0yOTQxLWFiMWYtM2Q5ZDRiN2YxNjZiPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjY1NTM1PC9leGlmOkNvbG9yU3BhY2U+CiAgICAgICAgIDxleGlmOlBpeGVsWERpbWVuc2lvbj4yOTk8L2V4aWY6UGl4ZWxYRGltZW5zaW9uPgogICAgICAgICA8ZXhpZjpQaXhlbFlEaW1lbnNpb24+NjA8L2V4aWY6UGl4ZWxZRGltZW5zaW9uPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAg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" />'', nchar(13), nchar(10), N''  <h2>Bandwidth Report</h2>'', nchar(13), nchar(10), N''  <table border="1">'', nchar(13), nchar(10), N''    <tr bgcolor="#66ccff">'', nchar(13), nchar(10), N''		<th>PackageID</th>'', nchar(13), nchar(10), N''        <th>QuotaValue</th>'', nchar(13), nchar(10), N''        <th>Diskspace</th>'', nchar(13), nchar(10), N''        <th>UsagePercentage</th>'', nchar(13), nchar(10), N''        <th>PackageName</th>'', nchar(13), nchar(10), N''        <th>PackagesNumber</th>'', nchar(13), nchar(10), N''        <th>StatusID</th>'', nchar(13), nchar(10), N''        <th>UserID</th>'', nchar(13), nchar(10), N''      <th>Username</th>'', nchar(13), nchar(10), N''        <th>FirstName</th>'', nchar(13), nchar(10), N''        <th>LastName</th>'', nchar(13), nchar(10), N''        <th>FullName</th>'', nchar(13), nchar(10), N''        <th>RoleID</th>'', nchar(13), nchar(10), N''        <th>Email</th>'', nchar(13), nchar(10), N''        <th>UserComments</th> '', nchar(13), nchar(10), N''    </tr>'', nchar(13), nchar(10), N''    <xsl:for-each select="//Table1">'', nchar(13), nchar(10), N''    <tr>'', nchar(13), nchar(10), N''	<td><xsl:value-of select="PackageID"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="QuotaValue"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="Diskspace"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="UsagePercentage"/>%</td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="PackageName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="PackagesNumber"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="StatusID"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="UserID"/></td>'', nchar(13), nchar(10), N''      <td><xsl:value-of select="Username"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="FirstName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="LastName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="FullName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="RoleID"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="Email"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="UserComments"/></td>'', nchar(13), nchar(10), N''    </tr>'', nchar(13), nchar(10), N''    </xsl:for-each>'', nchar(13), nchar(10), N''  </table>'', nchar(13), nchar(10), N''  </body>'', nchar(13), nchar(10), N''  </html>'', nchar(13), nchar(10), N''</xsl:template>'', nchar(13), nchar(10), N''</xsl:stylesheet>'')),
    (N''TransformContentType'', N''BandwidthXLST'', 1, N''test/html''),
    (N''TransformSuffix'', N''BandwidthXLST'', 1, N''.htm''),
    (N''Transform'', N''DiskspaceXLST'', 1, CONCAT(CAST(N''<?xml version="1.0" encoding="UTF-8"?>'' AS nvarchar(max)), nchar(13), nchar(10), N''<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">'', nchar(13), nchar(10), N''<xsl:template match="/">'', nchar(13), nchar(10), N''  <html>'', nchar(13), nchar(10), N''  <body>'', nchar(13), nchar(10), N''  <img alt="Embedded Image" width="299" height="60" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASsAAAA8CAYAAAA+AJwjAAAACXBIWXMAAAsTAAALEwEAmpwYAAA4G2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS41LWMwMTQgNzkuMTUxNDgxLCAyMDEzLzAzLzEzLTEyOjA5OjE1ICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgICAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgICAgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIKICAgICAgICAgICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmV4aWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vZXhpZi8xLjAvIj4KICAgICAgICAgPHhtcDpDcmVhdG9yVG9vbD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC94bXA6Q3JlYXRvclRvb2w+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMTYtMDMtMDFUMTQ6NTE6NTgrMDE6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE2LTAzLTAxVDE0OjUxOjU4KzAxOjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8ZGM6Zm9ybWF0PmltYWdlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3A6Q29sb3JNb2RlPgogICAgICAgICA8eG1wTU06SW5zdGFuY2VJRD54bXAuaWlkOjZhNTdmMWYyLTgyZjYtMjk0MS1hYjFmLTNkOWQ0YjdmMTY2YjwveG1wTU06SW5zdGFuY2VJRD4KICAgICAgICAgPHhtcE1NOkRvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDo2YTU3ZjFmMi04MmY2LTI5NDEtYWIxZi0zZDlkNGI3ZjE2NmI8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkhpc3Rvcnk+CiAgICAgICAgICAgIDxyZGY6U2VxPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5jcmVhdGVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6NmE1N2YxZjItODJmNi0yOTQxLWFiMWYtM2Q5ZDRiN2YxNjZiPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE2LTAzLTAxVDE0OjUwOjQzKzAxOjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpPC9zdEV2dDpzb2Z0d2FyZUFnZW50PgogICAgICAgICAgICAgICA8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L3htcE1NOkhpc3Rvcnk+CiAgICAgICAgIDx0aWZmOk9yaWVudGF0aW9uPjE8L3RpZmY6T3JpZW50YXRpb24+CiAgICAgICAgIDx0aWZmOlhSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpYUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6WVJlc29sdXRpb24+NzIwMDAwLzEwMDAwPC90aWZmOllSZXNvbHV0aW9uPgogICAgICAgICA8dGlmZjpSZXNvbHV0aW9uVW5pdD4yPC90aWZmOlJlc29sdXRpb25Vbml0PgogICAgICAgICA8ZXhpZjpDb2xvclNwYWNlPjY1NTM1PC9leGlmOkNvbG9yU3BhY2U+CiAgICAgICAgIDxleGlmOlBpeGVsWERpbWVuc2lvbj4yOTk8L2V4aWY6UGl4ZWxYRGltZW5zaW9uPgogICAgICAgICA8ZXhpZjpQaXhlbFlEaW1lbnNpb24+NjA8L2V4aWY6UGl4ZWxZRGltZW5zaW9uPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAg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" />'', nchar(13), nchar(10), N''  <h2>DiskSpace Report</h2>'', nchar(13), nchar(10), N''  <table border="1">'', nchar(13), nchar(10), N''    <tr bgcolor="#66ccff">'', nchar(13), nchar(10), N''		<th>PackageID</th>'', nchar(13), nchar(10), N''        <th>QuotaValue</th>'', nchar(13), nchar(10), N''        <th>Bandwidth</th>'', nchar(13), nchar(10), N''        <th>UsagePercentage</th>'', nchar(13), nchar(10), N''        <th>PackageName</th>'', nchar(13), nchar(10), N''        <th>PackagesNumber</th>'', nchar(13), nchar(10), N''        <th>StatusID</th>'', nchar(13), nchar(10), N''        <th>UserID</th>'', nchar(13), nchar(10), N''      <th>Username</th>'', nchar(13), nchar(10), N''        <th>FirstName</th>'', nchar(13), nchar(10), N''        <th>LastName</th>'', nchar(13), nchar(10), N''        <th>FullName</th>'', nchar(13), nchar(10), N''        <th>RoleID</th>'', nchar(13), nchar(10), N''        <th>Email</th>'', nchar(13), nchar(10), N''    </tr>'', nchar(13), nchar(10), N''    <xsl:for-each select="//Table1">'', nchar(13), nchar(10), N''    <tr>'', nchar(13), nchar(10), N''	<td><xsl:value-of select="PackageID"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="QuotaValue"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="Bandwidth"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="UsagePercentage"/>%</td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="PackageName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="PackagesNumber"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="StatusID"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="UserID"/></td>'', nchar(13), nchar(10), N''      <td><xsl:value-of select="Username"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="FirstName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="LastName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="FullName"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="RoleID"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="Email"/></td>'', nchar(13), nchar(10), N''        <td><xsl:value-of select="UserComments"/></td>'', nchar(13), nchar(10), N''    </tr>'', nchar(13), nchar(10), N''    </xsl:for-each>'', nchar(13), nchar(10), N''  </table>'', nchar(13), nchar(10), N''  </body>'', nchar(13), nchar(10), N''  </html>'', nchar(13), nchar(10), N''</xsl:template>'', nchar(13), nchar(10), N''</xsl:stylesheet>'')),
    (N''TransformContentType'', N''DiskspaceXLST'', 1, N''text/html''),
    (N''TransformSuffix'', N''DiskspaceXLST'', 1, N''.htm''),
    (N''GridItems'', N''DisplayPreferences'', 1, N''10''),
    (N''CC'', N''DomainExpirationLetter'', 1, N''support@HostingCompany.com''),
    (N''From'', N''DomainExpirationLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''DomainExpirationLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Domain Expiration Information</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a name="top"></a>'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''	Domain Expiration Information'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Please, find below details of your domain expiration information.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<table>'', nchar(13), nchar(10), N''    <thead>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <th>Domain</th>'', nchar(13), nchar(10), N''			<th>Registrar</th>'', nchar(13), nchar(10), N''			<th>Customer</th>'', nchar(13), nchar(10), N''            <th>Expiration Date</th>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </thead>'', nchar(13), nchar(10), N''    <tbody>'', nchar(13), nchar(10), N''            <ad:foreach collection="#Domains#" var="Domain" index="i">'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td>#Domain.DomainName#</td>'', nchar(13), nchar(10), N''			<td>#iif(isnull(Domain.Registrar), "", Domain.Registrar)#</td>'', nchar(13), nchar(10), N''			<td>#Domain.Customer#</td>'', nchar(13), nchar(10), N''            <td>#iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </ad:foreach>'', nchar(13), nchar(10), N''    </tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#IncludeNonExistenDomains#">'', nchar(13), nchar(10), N''	<p>'', nchar(13), nchar(10), N''	Please, find below details of your non-existen domains.'', nchar(13), nchar(10), N''	</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''	<table>'', nchar(13), nchar(10), N''		<thead>'', nchar(13), nchar(10), N''			<tr>'', nchar(13), nchar(10), N''				<th>Domain</th>'', nchar(13), nchar(10), N''				<th>Customer</th>'', nchar(13), nchar(10), N''			</tr>'', nchar(13), nchar(10), N''		</thead>'', nchar(13), nchar(10), N''		<tbody>'', nchar(13), nchar(10), N''				<ad:foreach collection="#NonExistenDomains#" var="Domain" index="i">'', nchar(13), nchar(10), N''			<tr>'', nchar(13), nchar(10), N''				<td>#Domain.DomainName#</td>'', nchar(13), nchar(10), N''				<td>#Domain.Customer#</td>'', nchar(13), nchar(10), N''			</tr>'', nchar(13), nchar(10), N''		</ad:foreach>'', nchar(13), nchar(10), N''		</tbody>'', nchar(13), nchar(10), N''	</table>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10), N''</p>'')),
    (N''Priority'', N''DomainExpirationLetter'', 1, N''Normal''),
    (N''Subject'', N''DomainExpirationLetter'', 1, N''Domain expiration notification''),
    (N''TextBody'', N''DomainExpirationLetter'', 1, CONCAT(CAST(N''================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Domain Expiration Information'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''Please, find below details of your domain expiration information.'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:foreach collection="#Domains#" var="Domain" index="i">'', nchar(13), nchar(10), N''	Domain: #Domain.DomainName#'', nchar(13), nchar(10), N''	Registrar: #iif(isnull(Domain.Registrar), "", Domain.Registrar)#'', nchar(13), nchar(10), N''	Customer: #Domain.Customer#'', nchar(13), nchar(10), N''	Expiration Date: #iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#'', nchar(13), nchar(10), nchar(13), nchar(10), N''</ad:foreach>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#IncludeNonExistenDomains#">'', nchar(13), nchar(10), N''Please, find below details of your non-existen domains.'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:foreach collection="#NonExistenDomains#" var="Domain" index="i">'', nchar(13), nchar(10), N''	Domain: #Domain.DomainName#'', nchar(13), nchar(10), N''	Customer: #Domain.Customer#'', nchar(13), nchar(10), nchar(13), nchar(10), N''</ad:foreach>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'')),
    (N''CC'', N''DomainLookupLetter'', 1, N''support@HostingCompany.com''),
    (N''From'', N''DomainLookupLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''DomainLookupLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>MX and NS Changes Information</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''		.Summary H3 { font-size: 1em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a name="top"></a>'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''	MX and NS Changes Information'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Please, find below details of MX and NS changes.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''    <ad:foreach collection="#Domains#" var="Domain" index="i">'', nchar(13), nchar(10), N''	<h2>#Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#</h2>'', nchar(13), nchar(10), N''	<h3>#iif(isnull(Domain.Registrar), "", Domain.Registrar)# #iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#</h3>'', nchar(13), nchar(10), nchar(13), nchar(10), N''	<table>'', nchar(13), nchar(10), N''	    <thead>'', nchar(13), nchar(10), N''	        <tr>'', nchar(13), nchar(10), N''	            <th>DNS</th>'', nchar(13), nchar(10), N''				<th>Type</th>'', nchar(13), nchar(10), N''				<th>Status</th>'', nchar(13), nchar(10), N''	            <th>Old Value</th>'', nchar(13), nchar(10), N''                <th>New Value</th>'', nchar(13), nchar(10), N''	        </tr>'', nchar(13), nchar(10), N''	    </thead>'', nchar(13), nchar(10), N''	    <tbody>'', nchar(13), nchar(10), N''	        <ad:foreach collection="#Domain.DnsChanges#" var="DnsChange" index="j">'', nchar(13), nchar(10), N''	        <tr>'', nchar(13), nchar(10), N''	            <td>#DnsChange.DnsServer#</td>'', nchar(13), nchar(10), N''	            <td>#DnsChange.Type#</td>'', nchar(13), nchar(10), N''				<td>#DnsChange.Status#</td>'', nchar(13), nchar(10), N''                <td>#DnsChange.OldRecord.Value#</td>'', nchar(13), nchar(10), N''	            <td>#DnsChange.NewRecord.Value#</td>'', nchar(13), nchar(10), N''	        </tr>'', nchar(13), nchar(10), N''	    	</ad:foreach>'', nchar(13), nchar(10), N''	    </tbody>'', nchar(13), nchar(10), N''	</table>'', nchar(13), nchar(10), N''	'', nchar(13), nchar(10), N''    </ad:foreach>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10), N''</p>'')),
    (N''NoChangesHtmlBody'', N''DomainLookupLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>MX and NS Changes Information</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a name="top"></a>'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''	MX and NS Changes Information'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''No MX and NS changes have been found.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10), N''</p>'')),
    (N''NoChangesTextBody'', N''DomainLookupLetter'', 1, CONCAT(CAST(N''================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   MX and NS Changes Information'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''No MX and NS changes have been founded.'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10))),
    (N''Priority'', N''DomainLookupLetter'', 1, N''Normal''),
    (N''Subject'', N''DomainLookupLetter'', 1, N''MX and NS changes notification''),
    (N''TextBody'', N''DomainLookupLetter'', 1, CONCAT(CAST(N''================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   MX and NS Changes Information'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''Please, find below details of MX and NS changes.'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:foreach collection="#Domains#" var="Domain" index="i">'', nchar(13), nchar(10), nchar(13), nchar(10), N''# Domain.DomainName# - #DomainUsers[Domain.PackageId].FirstName# #DomainUsers[Domain.PackageId].LastName#'', nchar(13), nchar(10), N'' Registrar:      #iif(isnull(Domain.Registrar), "", Domain.Registrar)#'', nchar(13), nchar(10), N'' ExpirationDate: #iif(isnull(Domain.ExpirationDate), "", Domain.ExpirationDate)#'', nchar(13), nchar(10), nchar(13), nchar(10), N''        <ad:foreach collection="#Domain.DnsChanges#" var="DnsChange" index="j">'', nchar(13), nchar(10), N''            DNS:       #DnsChange.DnsServer#'', nchar(13), nchar(10), N''            Type:      #DnsChange.Type#'', nchar(13), nchar(10), N''	    Status:    #DnsChange.Status#'', nchar(13), nchar(10), N''            Old Value: #DnsChange.OldRecord.Value#'', nchar(13), nchar(10), N''            New Value: #DnsChange.NewRecord.Value#'', nchar(13), nchar(10), nchar(13), nchar(10), N''    	</ad:foreach>'', nchar(13), nchar(10), N''</ad:foreach>'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10))),
    (N''From'', N''ExchangeMailboxSetupLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''ExchangeMailboxSetupLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Account Summary Information</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''        body {font-family: ''''Segoe UI Light'''',''''Open Sans'''',Arial!important;color:black;}'', nchar(13), nchar(10), N''        p {color:black;}'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.SummaryHeader { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.5em; color: ##1F4978; border-bottom: dotted 3px ##efefef; font-weight:normal; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.2em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; color:black;}'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''        .Label { color:##1F4978; }'', nchar(13), nchar(10), N''        .menu-bar a {padding: 15px 0;display: inline-block;}'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%"><!-- was 800 -->'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="padding: 10px 20px 10px 20px; background-color: ##e1e1e1;">'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="text-align: left; padding: 0px 0px 2px 0px;"><a href=""><img src="" border="0" alt="" /></a></td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="padding-bottom: 10px;">'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="background-color: ##2e8bcc; padding: 3px;">'', nchar(13), nchar(10), N''<table class="menu-bar" border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""</a></td>'', nchar(13), nchar(10), N''<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>'', nchar(13), nchar(10), N''<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>'', nchar(13), nchar(10), N''<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>'', nchar(13), nchar(10), N''<td style="text-align: center;" width="20%"><a style="color: ##ffffff; text-transform: uppercase; font-size: 9px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; text-decoration: none;" href=""></a></td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="background-color: ##ffffff;">'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%"><!-- was 759 -->'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="vertical-align: top; padding: 10px 10px 0px 10px;" width="100%">'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="font-family: ''''Segoe UI Light'''',''''Open Sans'''',Arial; padding: 0px 10px 0px 0px;">'', nchar(13), nchar(10), N''<!-- Begin Content -->'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), N''    <ad:if test="#Email#">'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    Hello #Account.DisplayName#,'', nchar(13), nchar(10), N''    </p>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    Thanks for choosing as your Exchange hosting provider.'', nchar(13), nchar(10), N''    </p>'', nchar(13), nchar(10), N''    </ad:if>'', nchar(13), nchar(10), N''    <ad:if test="#not(PMM)#">'', nchar(13), nchar(10), N''    <h1>User Accounts</h1>'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    The following user accounts have been created for you.'', nchar(13), nchar(10), N''    </p>'', nchar(13), nchar(10), N''    <table>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td class="Label">Username:</td>'', nchar(13), nchar(10), N''            <td>#Account.UserPrincipalName#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td class="Label">E-mail:</td>'', nchar(13), nchar(10), N''            <td>#Account.PrimaryEmailAddress#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''		<ad:if test="#PswResetUrl#">'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td class="Label">Password Reset Url:</td>'', nchar(13), nchar(10), N''            <td><a href="#PswResetUrl#" target="_blank">Click here</a></td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''		</ad:if>'', nchar(13), nchar(10), N''    </table>'', nchar(13), nchar(10), N''    </ad:if>'', nchar(13), nchar(10), N''    <h1>DNS</h1>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    In order for us to accept mail for your domain, you will need to point your MX records to:'', nchar(13), nchar(10), N''    </p>'', nchar(13), nchar(10), N''    <table>'', nchar(13), nchar(10), N''        <ad:foreach collection="#SmtpServers#" var="SmtpServer" index="i">'', nchar(13), nchar(10), N''            <tr>'', nchar(13), nchar(10), N''                <td class="Label">#SmtpServer#</td>'', nchar(13), nchar(10), N''            </tr>'', nchar(13), nchar(10), N''        </ad:foreach>'', nchar(13), nchar(10), N''    </table>'', nchar(13), nchar(10), N''   <h1>'', nchar(13), nchar(10), N''    Webmail (OWA, Outlook Web Access)</h1>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    <a href="" target="_blank"></a>'', nchar(13), nchar(10), N''    </p>'', nchar(13), nchar(10), N''    <h1>'', nchar(13), nchar(10), N''    Outlook (Windows Clients)</h1>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    To configure MS Outlook to work with the servers, please reference:'', nchar(13), nchar(10), N''    </p>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    <a href="" target="_blank"></a>'', nchar(13), nchar(10), N''    </p>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    If you need to download and install the Outlook client:</p>'', nchar(13), nchar(10), N''        '', nchar(13), nchar(10), N''        <table>'', nchar(13), nchar(10), N''            <tr><td colspan="2" class="Label"><font size="3">MS Outlook Client</font></td></tr>'', nchar(13), nchar(10), N''            <tr>'', nchar(13), nchar(10), N''                <td class="Label">'', nchar(13), nchar(10), N''                    Download URL:</td>'', nchar(13), nchar(10), N''                <td><a href=""></a></td>'', nchar(13), nchar(10), N''            </tr>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''                <td class="Label"></td>'', nchar(13), nchar(10), N''                <td><a href=""></a></td>'', nchar(13), nchar(10), N''            </tr>'', nchar(13), nchar(10), N''            <tr>'', nchar(13), nchar(10), N''                <td class="Label">'', nchar(13), nchar(10), N''                    KEY:</td>'', nchar(13), nchar(10), N''                <td></td>'', nchar(13), nchar(10), N''            </tr>'', nchar(13), nchar(10), N''        </table>'', nchar(13), nchar(10), N'' '', nchar(13), nchar(10), N''       <h1>'', nchar(13), nchar(10), N''    ActiveSync, iPhone, iPad</h1>'', nchar(13), nchar(10), N''    <table>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td class="Label">Server:</td>'', nchar(13), nchar(10), N''            <td>#ActiveSyncServer#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td class="Label">Domain:</td>'', nchar(13), nchar(10), N''            <td>#SamDomain#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td class="Label">SSL:</td>'', nchar(13), nchar(10), N''            <td>must be checked</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td class="Label">Your username:</td>'', nchar(13), nchar(10), N''            <td>#SamUsername#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), CONCAT(CAST(nchar(10) AS nvarchar(max)), N''    </table>'', nchar(13), nchar(10), N'' '', nchar(13), nchar(10), N''    <h1>Password Changes</h1>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    Passwords can be changed at any time using Webmail or the <a href="" target="_blank">Control Panel</a>.</p>'', nchar(13), nchar(10), N''    <h1>Control Panel</h1>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    If you need to change the details of your account, you can easily do this using <a href="" target="_blank">Control Panel</a>.</p>'', nchar(13), nchar(10), N''    <h1>Support</h1>'', nchar(13), nchar(10), N''    <p>'', nchar(13), nchar(10), N''    You have 2 options, email <a href="mailto:"></a> or use the web interface at <a href=""></a></p>'', nchar(13), nchar(10), N''    '', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''<!-- End Content -->'', nchar(13), nchar(10), N''<br></td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="background-color: ##ffffff; border-top: 1px solid ##999999;">'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="vertical-align: top; padding: 0px 20px 15px 20px;">'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="font-family: Arial, Helvetica, sans-serif; text-align: left; font-size: 9px; color: ##717073; padding: 20px 0px 0px 0px;">'', nchar(13), nchar(10), N''<table border="0" cellspacing="0" cellpadding="0" width="100%">'', nchar(13), nchar(10), N''<tbody>'', nchar(13), nchar(10), N''<tr>'', nchar(13), nchar(10), N''<td style="font-family: Arial, Helvetica, sans-serif; font-size: 9px; text-align: left; color: ##1666af; vertical-align: top;" width="33%"><a style="font-weight: bold; text-transform: uppercase; text-decoration: underline; color: ##1666af;" href=""></a><br />Learn more about the services can provide to improve your business.</td>'', nchar(13), nchar(10), N''<td style="font-family: Arial, Helvetica, sans-serif; font-size: 9px; text-align: left; color: ##1666af; padding: 0px 10px 0px 10px; vertical-align: top;" width="34%"><a style="font-weight: bold; text-transform: uppercase; text-decoration: underline; color: ##1666af;" href="">Privacy Policy</a><br /> follows strict guidelines in protecting your privacy. Learn about our <a style="font-weight: bold; text-decoration: underline; color: ##1666af;" href="">Privacy Policy</a>.</td>'', nchar(13), nchar(10), N''<td style="font-family: Arial, Helvetica, sans-serif; font-size: 9px; text-align: left; color: ##1666af; vertical-align: top;" width="33%"><a style="font-weight: bold; text-transform: uppercase; text-decoration: underline; color: ##1666af;" href="">Contact Us</a><br />Questions? For more information, <a style="font-weight: bold; text-decoration: underline; color: ##1666af;" href="">contact us</a>.</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</td>'', nchar(13), nchar(10), N''</tr>'', nchar(13), nchar(10), N''</tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), N''</body>'', nchar(13), nchar(10), N''</html>'')))),
    (N''Priority'', N''ExchangeMailboxSetupLetter'', 1, N''Normal''),
    (N''Subject'', N''ExchangeMailboxSetupLetter'', 1, N'' Hosted Exchange Mailbox Setup''),
    (N''TextBody'', N''ExchangeMailboxSetupLetter'', 1, CONCAT(CAST(N''<ad:if test="#Email#">'' AS nvarchar(max)), nchar(13), nchar(10), N''Hello #Account.DisplayName#,'', nchar(13), nchar(10), nchar(13), nchar(10), N''Thanks for choosing as your Exchange hosting provider.'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), N''<ad:if test="#not(PMM)#">'', nchar(13), nchar(10), N''User Accounts'', nchar(13), nchar(10), nchar(13), nchar(10), N''The following user accounts have been created for you.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Username: #Account.UserPrincipalName#'', nchar(13), nchar(10), N''E-mail: #Account.PrimaryEmailAddress#'', nchar(13), nchar(10), N''<ad:if test="#PswResetUrl#">'', nchar(13), nchar(10), N''Password Reset Url: #PswResetUrl#'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''DNS'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''In order for us to accept mail for your domain, you will need to point your MX records to:'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:foreach collection="#SmtpServers#" var="SmtpServer" index="i">#SmtpServer#</ad:foreach>'', nchar(13), nchar(10), nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''Webmail (OWA, Outlook Web Access)'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''Outlook (Windows Clients)'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''To configure MS Outlook to work with servers, please reference:'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''If you need to download and install the MS Outlook client:'', nchar(13), nchar(10), nchar(13), nchar(10), N''MS Outlook Download URL:'', nchar(13), nchar(10), nchar(13), nchar(10), N''KEY: '', nchar(13), nchar(10), nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''ActiveSync, iPhone, iPad'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''Server: #ActiveSyncServer#'', nchar(13), nchar(10), N''Domain: #SamDomain#'', nchar(13), nchar(10), N''SSL: must be checked'', nchar(13), nchar(10), N''Your username: #SamUsername#'', nchar(13), nchar(10), nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''Password Changes'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''Passwords can be changed at any time using Webmail or the Control Panel'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''Control Panel'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''If you need to change the details of your account, you can easily do this using the Control Panel '', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''Support'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''You have 2 options, email or use the web interface at '')),
    (N''MailboxPasswordPolicy'', N''ExchangePolicy'', 1, N''True;8;20;0;2;0;True''),
    (N''UserNamePolicy'', N''FtpPolicy'', 1, N''True;-;1;20;;;''),
    (N''UserPasswordPolicy'', N''FtpPolicy'', 1, N''True;5;20;0;1;0;True''),
    (N''AccountNamePolicy'', N''MailPolicy'', 1, N''True;;1;50;;;''),
    (N''AccountPasswordPolicy'', N''MailPolicy'', 1, N''True;5;20;0;1;0;False;;0;;;False;False;0;''),
    (N''CatchAllName'', N''MailPolicy'', 1, N''mail''),
    (N''DatabaseNamePolicy'', N''MariaDBPolicy'', 1, N''True;;1;40;;;''),
    (N''UserNamePolicy'', N''MariaDBPolicy'', 1, N''True;;1;16;;;''),
    (N''UserPasswordPolicy'', N''MariaDBPolicy'', 1, N''True;5;20;0;1;0;False;;0;;;False;False;0;'');
    INSERT INTO [UserSettings] ([PropertyName], [SettingsName], [UserID], [PropertyValue])
    VALUES (N''DatabaseNamePolicy'', N''MsSqlPolicy'', 1, N''True;-;1;120;;;''),
    (N''UserNamePolicy'', N''MsSqlPolicy'', 1, N''True;-;1;120;;;''),
    (N''UserPasswordPolicy'', N''MsSqlPolicy'', 1, N''True;5;20;0;1;0;True;;0;0;0;False;False;0;''),
    (N''DatabaseNamePolicy'', N''MySqlPolicy'', 1, N''True;;1;40;;;''),
    (N''UserNamePolicy'', N''MySqlPolicy'', 1, N''True;;1;16;;;''),
    (N''UserPasswordPolicy'', N''MySqlPolicy'', 1, N''True;5;20;0;1;0;False;;0;0;0;False;False;0;''),
    (N''From'', N''OrganizationUserPasswordRequestLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''OrganizationUserPasswordRequestLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Password request notification</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''<img src="#logoUrl#">'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''<h1>Password request notification</h1>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Your account have been created. In order to create a password for your account, please follow next link:'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a href="#passwordResetLink#" target="_blank">#passwordResetLink#</a>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'')),
    (N''LogoUrl'', N''OrganizationUserPasswordRequestLetter'', 1, N''''),
    (N''Priority'', N''OrganizationUserPasswordRequestLetter'', 1, N''Normal''),
    (N''SMSBody'', N''OrganizationUserPasswordRequestLetter'', 1, CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''User have been created. Password request url:'', nchar(13), nchar(10), N''# passwordResetLink#'')),
    (N''Subject'', N''OrganizationUserPasswordRequestLetter'', 1, N''Password request notification''),
    (N''TextBody'', N''OrganizationUserPasswordRequestLetter'', 1, CONCAT(CAST(N''========================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Password request notification'', nchar(13), nchar(10), N''========================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''Your account have been created. In order to create a password for your account, please follow next link:'', nchar(13), nchar(10), nchar(13), nchar(10), N''# passwordResetLink#'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'')),
    (N''DsnNamePolicy'', N''OsPolicy'', 1, N''True;-;2;40;;;''),
    (N''CC'', N''PackageSummaryLetter'', 1, N''support@HostingCompany.com''),
    (N''EnableLetter'', N''PackageSummaryLetter'', 1, N''True''),
    (N''From'', N''PackageSummaryLetter'', 1, N''support@HostingCompany.com''),
    (N''Priority'', N''PackageSummaryLetter'', 1, N''Normal''),
    (N''Subject'', N''PackageSummaryLetter'', 1, N''"#space.Package.PackageName#" <ad:if test="#Signup#">hosting space has been created for<ad:else>hosting space summary for</ad:if> #user.FirstName# #user.LastName#''),
    (N''CC'', N''PasswordReminderLetter'', 1, N''''),
    (N''From'', N''PasswordReminderLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''PasswordReminderLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Account Summary Information</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; }'', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a name="top"></a>'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''	Hosting Account Information'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Please, find below details of your control panel account. The one time password was generated for you. You should change the password after login. '', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<h1>Control Panel URL</h1>'', nchar(13), nchar(10), N''<table>'', nchar(13), nchar(10), N''    <thead>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <th>Control Panel URL</th>'', nchar(13), nchar(10), N''            <th>Username</th>'', nchar(13), nchar(10), N''            <th>One Time Password</th>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </thead>'', nchar(13), nchar(10), N''    <tbody>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>'', nchar(13), nchar(10), N''            <td>#user.Username#</td>'', nchar(13), nchar(10), N''            <td>#user.Password#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards,<br />'', nchar(13), nchar(10), N''SolidCP.<br />'', nchar(13), nchar(10), N''Web Site: <a href="https://solidcp.com">https://solidcp.com</a><br />'', nchar(13), nchar(10), N''E-Mail: <a href="mailto:support@solidcp.com">support@solidcp.com</a>'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'', nchar(13), nchar(10), N''</html>'')),
    (N''Priority'', N''PasswordReminderLetter'', 1, N''Normal''),
    (N''Subject'', N''PasswordReminderLetter'', 1, N''Password reminder for #user.FirstName# #user.LastName#''),
    (N''TextBody'', N''PasswordReminderLetter'', 1, CONCAT(CAST(N''================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Hosting Account Information'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), nchar(13), nchar(10), N''Please, find below details of your control panel account. The one time password was generated for you. You should change the password after login.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Control Panel URL: https://panel.solidcp.com'', nchar(13), nchar(10), N''Username: #user.Username#'', nchar(13), nchar(10), N''One Time Password: #user.Password#'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards,'', nchar(13), nchar(10), N''SolidCP.'', nchar(13), nchar(10), N''Web Site: https://solidcp.com"'', nchar(13), nchar(10), N''E-Mail: support@solidcp.com'')),
    (N''CC'', N''RDSSetupLetter'', 1, N''support@HostingCompany.com''),
    (N''From'', N''RDSSetupLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''RDSSetupLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>RDS Setup Information</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a name="top"></a>'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''	RDS Setup Information'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'')),
    (N''Priority'', N''RDSSetupLetter'', 1, N''Normal''),
    (N''Subject'', N''RDSSetupLetter'', 1, N''RDS setup''),
    (N''TextBody'', N''RDSSetupLetter'', 1, CONCAT(CAST(N''================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   RDS Setup Information'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''Please, find below RDS setup instructions.'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'')),
    (N''GroupNamePolicy'', N''SharePointPolicy'', 1, N''True;-;1;20;;;''),
    (N''UserNamePolicy'', N''SharePointPolicy'', 1, N''True;-;1;20;;;''),
    (N''UserPasswordPolicy'', N''SharePointPolicy'', 1, N''True;5;20;0;1;0;True;;0;;;False;False;0;''),
    (N''DemoMessage'', N''SolidCPPolicy'', 1, CONCAT(CAST(N''When user account is in demo mode the majority of operations are'' AS nvarchar(max)), nchar(13), nchar(10), N''disabled, especially those ones that modify or delete records.'', nchar(13), nchar(10), N''You are welcome to ask your questions or place comments about'', nchar(13), nchar(10), N''this demo on  <a href="http://forum.SolidCP.net"'', nchar(13), nchar(10), N''target="_blank">SolidCP  Support Forum</a>'')),
    (N''ForbiddenIP'', N''SolidCPPolicy'', 1, N''''),
    (N''PasswordPolicy'', N''SolidCPPolicy'', 1, N''True;6;20;0;1;0;True;;0;;;False;False;0;''),
    (N''From'', N''UserPasswordExpirationLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''UserPasswordExpirationLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Password expiration notification</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''<img src="#logoUrl#">'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''<h1>Password expiration notification</h1>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Your password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a href="#passwordResetLink#" target="_blank">#passwordResetLink#</a>'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'')),
    (N''LogoUrl'', N''UserPasswordExpirationLetter'', 1, N''''),
    (N''Priority'', N''UserPasswordExpirationLetter'', 1, N''Normal''),
    (N''Subject'', N''UserPasswordExpirationLetter'', 1, N''Password expiration notification'');
    INSERT INTO [UserSettings] ([PropertyName], [SettingsName], [UserID], [PropertyValue])
    VALUES (N''TextBody'', N''UserPasswordExpirationLetter'', 1, CONCAT(CAST(N''========================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Password expiration notification'', nchar(13), nchar(10), N''========================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''Your password expiration date is #user.PasswordExpirationDateTime#. You can reset your own password by visiting the following page:'', nchar(13), nchar(10), nchar(13), nchar(10), N''# passwordResetLink#'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'')),
    (N''From'', N''UserPasswordResetLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''UserPasswordResetLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Password reset notification</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''<img src="#logoUrl#">'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''<h1>Password reset notification</h1>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''We received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a href="#passwordResetLink#" target="_blank">#passwordResetLink#</a>'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'')),
    (N''LogoUrl'', N''UserPasswordResetLetter'', 1, N''''),
    (N''PasswordResetLinkSmsBody'', N''UserPasswordResetLetter'', 1, CONCAT(CAST(N''Password reset link:'' AS nvarchar(max)), nchar(13), nchar(10), N''# passwordResetLink#'', nchar(13), nchar(10))),
    (N''Priority'', N''UserPasswordResetLetter'', 1, N''Normal''),
    (N''Subject'', N''UserPasswordResetLetter'', 1, N''Password reset notification''),
    (N''TextBody'', N''UserPasswordResetLetter'', 1, CONCAT(CAST(N''========================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Password reset notification'', nchar(13), nchar(10), N''========================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''We received a request to reset the password for your account. If you made this request, click the link below. If you did not make this request, you can ignore this email.'', nchar(13), nchar(10), nchar(13), nchar(10), N''# passwordResetLink#'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'')),
    (N''From'', N''UserPasswordResetPincodeLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''UserPasswordResetPincodeLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Password reset notification</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; } '', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''<img src="#logoUrl#">'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''<h1>Password reset notification</h1>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''We received a request to reset the password for your account. Your password reset pincode:'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''# passwordResetPincode#'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'')),
    (N''LogoUrl'', N''UserPasswordResetPincodeLetter'', 1, N''''),
    (N''PasswordResetPincodeSmsBody'', N''UserPasswordResetPincodeLetter'', 1, CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''Your password reset pincode:'', nchar(13), nchar(10), N''# passwordResetPincode#'')),
    (N''Priority'', N''UserPasswordResetPincodeLetter'', 1, N''Normal''),
    (N''Subject'', N''UserPasswordResetPincodeLetter'', 1, N''Password reset notification''),
    (N''TextBody'', N''UserPasswordResetPincodeLetter'', 1, CONCAT(CAST(N''========================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Password reset notification'', nchar(13), nchar(10), N''========================================='', nchar(13), nchar(10), nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''We received a request to reset the password for your account. Your password reset pincode:'', nchar(13), nchar(10), nchar(13), nchar(10), N''# passwordResetPincode#'', nchar(13), nchar(10), nchar(13), nchar(10), N''If you have any questions regarding your hosting account, feel free to contact our support department at any time.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards'')),
    (N''CC'', N''VerificationCodeLetter'', 1, N''support@HostingCompany.com''),
    (N''From'', N''VerificationCodeLetter'', 1, N''support@HostingCompany.com''),
    (N''HtmlBody'', N''VerificationCodeLetter'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>Verification code</title>'', nchar(13), nchar(10), N''    <style type="text/css">'', nchar(13), nchar(10), N''		.Summary { background-color: ##ffffff; padding: 5px; }'', nchar(13), nchar(10), N''		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }'', nchar(13), nchar(10), N''        .Summary A { color: ##0153A4; }'', nchar(13), nchar(10), N''        .Summary { font-family: Tahoma; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }'', nchar(13), nchar(10), N''        .Summary H2 { font-size: 1.3em; color: ##1F4978; }'', nchar(13), nchar(10), N''        .Summary TABLE { border: solid 1px ##e5e5e5; }'', nchar(13), nchar(10), N''        .Summary TH,'', nchar(13), nchar(10), N''        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }'', nchar(13), nchar(10), N''        .Summary TD { padding: 8px; font-size: 9pt; }'', nchar(13), nchar(10), N''        .Summary UL LI { font-size: 1.1em; font-weight: bold; }'', nchar(13), nchar(10), N''        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }'', nchar(13), nchar(10), N''    </style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div class="Summary">'', nchar(13), nchar(10), nchar(13), nchar(10), N''<a name="top"></a>'', nchar(13), nchar(10), N''<div class="Header">'', nchar(13), nchar(10), N''	Verification code'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''to complete the sign in, enter the verification code on the device. '', nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<table>'', nchar(13), nchar(10), N''    <thead>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <th>Verification code</th>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </thead>'', nchar(13), nchar(10), N''    <tbody>'', nchar(13), nchar(10), N''        <tr>'', nchar(13), nchar(10), N''            <td>#verificationCode#</td>'', nchar(13), nchar(10), N''        </tr>'', nchar(13), nchar(10), N''    </tbody>'', nchar(13), nchar(10), N''</table>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<p>'', nchar(13), nchar(10), N''Best regards,<br />'', nchar(13), nchar(10), nchar(13), nchar(10), N''</p>'', nchar(13), nchar(10), nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'', nchar(13), nchar(10), N''</html>'')),
    (N''Priority'', N''VerificationCodeLetter'', 1, N''Normal''),
    (N''Subject'', N''VerificationCodeLetter'', 1, N''Verification code''),
    (N''TextBody'', N''VerificationCodeLetter'', 1, CONCAT(CAST(N''================================='' AS nvarchar(max)), nchar(13), nchar(10), N''   Verification code'', nchar(13), nchar(10), N''================================='', nchar(13), nchar(10), N''<ad:if test="#user#">'', nchar(13), nchar(10), N''Hello #user.FirstName#,'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), nchar(13), nchar(10), N''to complete the sign in, enter the verification code on the device.'', nchar(13), nchar(10), nchar(13), nchar(10), N''Verification code'', nchar(13), nchar(10), N''# verificationCode#'', nchar(13), nchar(10), nchar(13), nchar(10), N''Best regards,'', nchar(13), nchar(10))),
    (N''AddParkingPage'', N''WebPolicy'', 1, N''True''),
    (N''AddRandomDomainString'', N''WebPolicy'', 1, N''False''),
    (N''AnonymousAccountPolicy'', N''WebPolicy'', 1, N''True;;5;20;;_web;''),
    (N''AspInstalled'', N''WebPolicy'', 1, N''True''),
    (N''AspNetInstalled'', N''WebPolicy'', 1, N''2''),
    (N''CgiBinInstalled'', N''WebPolicy'', 1, N''False''),
    (N''DefaultDocuments'', N''WebPolicy'', 1, N''Default.htm,Default.asp,index.htm,Default.aspx''),
    (N''EnableAnonymousAccess'', N''WebPolicy'', 1, N''True''),
    (N''EnableBasicAuthentication'', N''WebPolicy'', 1, N''False''),
    (N''EnableDedicatedPool'', N''WebPolicy'', 1, N''False''),
    (N''EnableDirectoryBrowsing'', N''WebPolicy'', 1, N''False''),
    (N''EnableParentPaths'', N''WebPolicy'', 1, N''False''),
    (N''EnableParkingPageTokens'', N''WebPolicy'', 1, N''False''),
    (N''EnableWindowsAuthentication'', N''WebPolicy'', 1, N''True''),
    (N''EnableWritePermissions'', N''WebPolicy'', 1, N''False''),
    (N''FrontPageAccountPolicy'', N''WebPolicy'', 1, N''True;;1;20;;;''),
    (N''FrontPagePasswordPolicy'', N''WebPolicy'', 1, N''True;5;20;0;1;0;False;;0;0;0;False;False;0;''),
    (N''ParkingPageContent'', N''WebPolicy'', 1, CONCAT(CAST(N''<html xmlns="http://www.w3.org/1999/xhtml">'' AS nvarchar(max)), nchar(13), nchar(10), N''<head>'', nchar(13), nchar(10), N''    <title>The web site is under construction</title>'', nchar(13), nchar(10), N''<style type="text/css">'', nchar(13), nchar(10), N''	H1 { font-size: 16pt; margin-bottom: 4px; }'', nchar(13), nchar(10), N''	H2 { font-size: 14pt; margin-bottom: 4px; font-weight: normal; }'', nchar(13), nchar(10), N''</style>'', nchar(13), nchar(10), N''</head>'', nchar(13), nchar(10), N''<body>'', nchar(13), nchar(10), N''<div id="PageOutline">'', nchar(13), nchar(10), N''	<h1>This web site has just been created from <a href="https://www.SolidCP.com">SolidCP </a> and it is still under construction.</h1>'', nchar(13), nchar(10), N''	<h2>The web site is hosted by <a href="https://solidcp.com">SolidCP</a>.</h2>'', nchar(13), nchar(10), N''</div>'', nchar(13), nchar(10), N''</body>'', nchar(13), nchar(10), N''</html>'')),
    (N''ParkingPageName'', N''WebPolicy'', 1, N''default.aspx''),
    (N''PerlInstalled'', N''WebPolicy'', 1, N''False''),
    (N''PhpInstalled'', N''WebPolicy'', 1, N'''');
    INSERT INTO [UserSettings] ([PropertyName], [SettingsName], [UserID], [PropertyValue])
    VALUES (N''PublishingProfile'', N''WebPolicy'', 1, CONCAT(CAST(N''<?xml version="1.0" encoding="utf-8"?>'' AS nvarchar(max)), nchar(13), nchar(10), N''<publishData>'', nchar(13), nchar(10), N''<ad:if test="#WebSite.WebDeploySitePublishingEnabled#">'', nchar(13), nchar(10), N''	<publishProfile'', nchar(13), nchar(10), N''		profileName="#WebSite.Name# - Web Deploy"'', nchar(13), nchar(10), N''		publishMethod="MSDeploy"'', nchar(13), nchar(10), N''		publishUrl="#WebSite["WmSvcServiceUrl"]#:#WebSite["WmSvcServicePort"]#"'', nchar(13), nchar(10), N''		msdeploySite="#WebSite.Name#"'', nchar(13), nchar(10), N''		userName="#WebSite.WebDeployPublishingAccount#"'', nchar(13), nchar(10), N''		userPWD="#WebSite.WebDeployPublishingPassword#"'', nchar(13), nchar(10), N''		destinationAppUrl="http://#WebSite.Name#/"'', nchar(13), nchar(10), N''		<ad:if test="#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#">SQLServerDBConnectionString="server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#"</ad:if>'', nchar(13), nchar(10), N''		<ad:if test="#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#">mySQLDBConnectionString="server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#"</ad:if>'', nchar(13), nchar(10), N''		<ad:if test="#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#">MariaDBDBConnectionString="server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#"</ad:if>'', nchar(13), nchar(10), N''		hostingProviderForumLink="https://solidcp.com/support"'', nchar(13), nchar(10), N''		controlPanelLink="https://panel.solidcp.com/"'', nchar(13), nchar(10), N''	/>'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), N''<ad:if test="#IsDefined("FtpAccount")#">'', nchar(13), nchar(10), N''	<publishProfile'', nchar(13), nchar(10), N''		profileName="#WebSite.Name# - FTP"'', nchar(13), nchar(10), N''		publishMethod="FTP"'', nchar(13), nchar(10), N''		publishUrl="ftp://#FtpServiceAddress#"'', nchar(13), nchar(10), N''		ftpPassiveMode="True"'', nchar(13), nchar(10), N''		userName="#FtpAccount.Name#"'', nchar(13), nchar(10), N''		userPWD="#FtpAccount.Password#"'', nchar(13), nchar(10), N''		destinationAppUrl="http://#WebSite.Name#/"'', nchar(13), nchar(10), N''		<ad:if test="#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#">SQLServerDBConnectionString="server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#"</ad:if>'', nchar(13), nchar(10), N''		<ad:if test="#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#">mySQLDBConnectionString="server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#"</ad:if>'', nchar(13), nchar(10), N''		<ad:if test="#Not(IsNull(MariaDBDatabase)) and Not(IsNull(MariaDBUser))#">MariaDBDBConnectionString="server=#MariaDBAddress#;database=#MariaDBDatabase.Name#;uid=#MariaDBUser.Name#;pwd=#MariaDBUser.Password#"</ad:if>'', nchar(13), nchar(10), N''		hostingProviderForumLink="https://solidcp.com/support"'', nchar(13), nchar(10), N''		controlPanelLink="https://panel.solidcp.com/"'', nchar(13), nchar(10), N''    />'', nchar(13), nchar(10), N''</ad:if>'', nchar(13), nchar(10), N''</publishData>'', nchar(13), nchar(10), nchar(13), nchar(10), N''<!--'', nchar(13), nchar(10), N''Control Panel:'', nchar(13), nchar(10), N''Username: #User.Username#'', nchar(13), nchar(10), N''Password: #User.Password#'', nchar(13), nchar(10), nchar(13), nchar(10), N''Technical Contact:'', nchar(13), nchar(10), N''support@solidcp.com'', nchar(13), nchar(10), N''-->'')),
    (N''PythonInstalled'', N''WebPolicy'', 1, N''False''),
    (N''SecuredGroupNamePolicy'', N''WebPolicy'', 1, N''True;;1;20;;;''),
    (N''SecuredUserNamePolicy'', N''WebPolicy'', 1, N''True;;1;20;;;''),
    (N''SecuredUserPasswordPolicy'', N''WebPolicy'', 1, N''True;5;20;0;1;0;False;;0;0;0;False;False;0;''),
    (N''VirtDirNamePolicy'', N''WebPolicy'', 1, N''True;-;3;50;;;''),
    (N''WebDataFolder'', N''WebPolicy'', 1, N''\[DOMAIN_NAME]\data''),
    (N''WebLogsFolder'', N''WebPolicy'', 1, N''\[DOMAIN_NAME]\logs''),
    (N''WebRootFolder'', N''WebPolicy'', 1, N''\[DOMAIN_NAME]\wwwroot'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyName', N'SettingsName', N'UserID', N'PropertyValue') AND [object_id] = OBJECT_ID(N'[UserSettings]'))
        SET IDENTITY_INSERT [UserSettings] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PackageID', N'ParentPackageID') AND [object_id] = OBJECT_ID(N'[PackagesTreeCache]'))
        SET IDENTITY_INSERT [PackagesTreeCache] ON;
    EXEC(N'INSERT INTO [PackagesTreeCache] ([PackageID], [ParentPackageID])
    VALUES (1, 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PackageID', N'ParentPackageID') AND [object_id] = OBJECT_ID(N'[PackagesTreeCache]'))
        SET IDENTITY_INSERT [PackagesTreeCache] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'QuotaID', N'GroupID', N'HideQuota', N'ItemTypeID', N'PerOrganization', N'QuotaDescription', N'QuotaName', N'QuotaOrder', N'QuotaTypeID', N'ServiceQuota') AND [object_id] = OBJECT_ID(N'[Quotas]'))
        SET IDENTITY_INSERT [Quotas] ON;
    EXEC(N'INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (2, 6, NULL, 7, NULL, N''Databases'', N''MySQL4.Databases'', 1, 2, CAST(1 AS bit)),
    (3, 5, NULL, 5, NULL, N''Databases'', N''MsSQL2000.Databases'', 1, 2, CAST(1 AS bit)),
    (4, 3, NULL, 9, NULL, N''FTP Accounts'', N''FTP.Accounts'', 1, 2, CAST(1 AS bit)),
    (12, 8, NULL, 14, NULL, N''Statistics Sites'', N''Stats.Sites'', 1, 2, CAST(1 AS bit)),
    (13, 2, NULL, 10, NULL, N''Web Sites'', N''Web.Sites'', 1, 2, CAST(1 AS bit)),
    (14, 4, NULL, 15, NULL, N''Mail Accounts'', N''Mail.Accounts'', 1, 2, CAST(1 AS bit)),
    (15, 5, NULL, 6, NULL, N''Users'', N''MsSQL2000.Users'', 2, 2, CAST(0 AS bit)),
    (18, 4, NULL, 16, NULL, N''Mail Forwardings'', N''Mail.Forwardings'', 3, 2, CAST(0 AS bit)),
    (19, 6, NULL, 8, NULL, N''Users'', N''MySQL4.Users'', 2, 2, CAST(0 AS bit)),
    (20, 4, NULL, 17, NULL, N''Mail Lists'', N''Mail.Lists'', 6, 2, CAST(0 AS bit)),
    (24, 4, NULL, 18, NULL, N''Mail Groups'', N''Mail.Groups'', 4, 2, CAST(0 AS bit)),
    (47, 1, NULL, 20, NULL, N''ODBC DSNs'', N''OS.ODBC'', 6, 2, CAST(0 AS bit)),
    (59, 2, NULL, 25, NULL, N''Shared SSL Folders'', N''Web.SharedSSL'', 8, 2, CAST(0 AS bit)),
    (62, 10, NULL, 21, NULL, N''Databases'', N''MsSQL2005.Databases'', 1, 2, CAST(0 AS bit)),
    (63, 10, NULL, 22, NULL, N''Users'', N''MsSQL2005.Users'', 2, 2, CAST(0 AS bit)),
    (68, 11, NULL, 23, NULL, N''Databases'', N''MySQL5.Databases'', 1, 2, CAST(0 AS bit)),
    (69, 11, NULL, 24, NULL, N''Users'', N''MySQL5.Users'', 2, 2, CAST(0 AS bit)),
    (110, 90, NULL, 75, NULL, N''Databases'', N''MySQL8.Databases'', 1, 2, CAST(0 AS bit)),
    (111, 90, NULL, 76, NULL, N''Users'', N''MySQL8.Users'', 2, 2, CAST(0 AS bit)),
    (120, 91, NULL, 75, NULL, N''Databases'', N''MySQL9.Databases'', 1, 2, CAST(0 AS bit)),
    (121, 91, NULL, 76, NULL, N''Users'', N''MySQL9.Users'', 2, 2, CAST(0 AS bit)),
    (200, 20, NULL, 200, 1, N''SharePoint Site Collections'', N''HostedSharePoint.Sites'', 1, 2, CAST(0 AS bit)),
    (205, 13, NULL, 29, NULL, N''Organizations'', N''HostedSolution.Organizations'', 1, 2, CAST(0 AS bit)),
    (206, 13, NULL, 30, 1, N''Users'', N''HostedSolution.Users'', 2, 2, CAST(0 AS bit)),
    (211, 22, NULL, 31, NULL, N''Databases'', N''MsSQL2008.Databases'', 1, 2, CAST(0 AS bit)),
    (212, 22, NULL, 32, NULL, N''Users'', N''MsSQL2008.Users'', 2, 2, CAST(0 AS bit)),
    (218, 23, NULL, 37, NULL, N''Databases'', N''MsSQL2012.Databases'', 1, 2, CAST(0 AS bit)),
    (219, 23, NULL, 38, NULL, N''Users'', N''MsSQL2012.Users'', 2, 2, CAST(0 AS bit)),
    (300, 30, NULL, 33, NULL, N''Number of VPS'', N''VPS.ServersNumber'', 1, 2, CAST(0 AS bit)),
    (345, 40, NULL, 35, NULL, N''Number of VPS'', N''VPSForPC.ServersNumber'', 1, 2, CAST(0 AS bit)),
    (470, 46, NULL, 39, NULL, N''Databases'', N''MsSQL2014.Databases'', 1, 2, CAST(0 AS bit)),
    (471, 46, NULL, 40, NULL, N''Users'', N''MsSQL2014.Users'', 2, 2, CAST(0 AS bit)),
    (550, 73, NULL, 204, 1, N''SharePoint Site Collections'', N''HostedSharePointEnterprise.Sites'', 1, 2, CAST(0 AS bit)),
    (553, 33, NULL, 41, NULL, N''Number of VPS'', N''VPS2012.ServersNumber'', 1, 2, CAST(0 AS bit)),
    (573, 50, NULL, 202, NULL, N''Databases'', N''MariaDB.Databases'', 1, 2, CAST(0 AS bit)),
    (574, 50, NULL, 203, NULL, N''Users'', N''MariaDB.Users'', 2, 2, CAST(0 AS bit)),
    (673, 167, NULL, 41, NULL, N''Number of VPS'', N''PROXMOX.ServersNumber'', 1, 2, CAST(0 AS bit)),
    (701, 71, NULL, 71, NULL, N''Databases'', N''MsSQL2016.Databases'', 1, 2, CAST(0 AS bit)),
    (702, 71, NULL, 72, NULL, N''Users'', N''MsSQL2016.Users'', 2, 2, CAST(0 AS bit)),
    (711, 72, NULL, 73, NULL, N''Databases'', N''MsSQL2017.Databases'', 1, 2, CAST(0 AS bit)),
    (712, 72, NULL, 74, NULL, N''Users'', N''MsSQL2017.Users'', 2, 2, CAST(0 AS bit)),
    (721, 74, NULL, 77, NULL, N''Databases'', N''MsSQL2019.Databases'', 1, 2, CAST(0 AS bit));
    INSERT INTO [Quotas] ([QuotaID], [GroupID], [HideQuota], [ItemTypeID], [PerOrganization], [QuotaDescription], [QuotaName], [QuotaOrder], [QuotaTypeID], [ServiceQuota])
    VALUES (722, 74, NULL, 78, NULL, N''Users'', N''MsSQL2019.Users'', 2, 2, CAST(0 AS bit)),
    (732, 75, NULL, 79, NULL, N''Databases'', N''MsSQL2022.Databases'', 1, 2, CAST(0 AS bit)),
    (733, 75, NULL, 80, NULL, N''Users'', N''MsSQL2022.Users'', 2, 2, CAST(0 AS bit)),
    (760, 76, NULL, 79, NULL, N''Databases'', N''MsSQL2025.Databases'', 1, 2, CAST(0 AS bit)),
    (761, 76, NULL, 80, NULL, N''Users'', N''MsSQL2025.Users'', 2, 2, CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'QuotaID', N'GroupID', N'HideQuota', N'ItemTypeID', N'PerOrganization', N'QuotaDescription', N'QuotaName', N'QuotaOrder', N'QuotaTypeID', N'ServiceQuota') AND [object_id] = OBJECT_ID(N'[Quotas]'))
        SET IDENTITY_INSERT [Quotas] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ScheduleID', N'Enabled', N'FromTime', N'HistoriesNumber', N'Interval', N'LastRun', N'MaxExecutionTime', N'NextRun', N'PackageID', N'PriorityID', N'ScheduleName', N'ScheduleTypeID', N'StartTime', N'TaskID', N'ToTime', N'WeekMonthDay') AND [object_id] = OBJECT_ID(N'[Schedule]'))
        SET IDENTITY_INSERT [Schedule] ON;
    EXEC(N'INSERT INTO [Schedule] ([ScheduleID], [Enabled], [FromTime], [HistoriesNumber], [Interval], [LastRun], [MaxExecutionTime], [NextRun], [PackageID], [PriorityID], [ScheduleName], [ScheduleTypeID], [StartTime], [TaskID], [ToTime], [WeekMonthDay])
    VALUES (1, CAST(1 AS bit), ''2000-01-01T12:00:00.000'', 7, 0, NULL, 3600, ''2010-07-16T14:53:02.470'', 1, N''Normal'', N''Calculate Disk Space'', N''Daily'', ''2000-01-01T12:30:00.000'', N''SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE'', ''2000-01-01T12:00:00.000'', 1),
    (2, CAST(1 AS bit), ''2000-01-01T12:00:00.000'', 7, 0, NULL, 3600, ''2010-07-16T14:53:02.477'', 1, N''Normal'', N''Calculate Bandwidth'', N''Daily'', ''2000-01-01T12:00:00.000'', N''SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH'', ''2000-01-01T12:00:00.000'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ScheduleID', N'Enabled', N'FromTime', N'HistoriesNumber', N'Interval', N'LastRun', N'MaxExecutionTime', N'NextRun', N'PackageID', N'PriorityID', N'ScheduleName', N'ScheduleTypeID', N'StartTime', N'TaskID', N'ToTime', N'WeekMonthDay') AND [object_id] = OBJECT_ID(N'[Schedule]'))
        SET IDENTITY_INSERT [Schedule] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyName', N'ProviderID', N'PropertyValue') AND [object_id] = OBJECT_ID(N'[ServiceDefaultProperties]'))
        SET IDENTITY_INSERT [ServiceDefaultProperties] ON;
    EXEC(N'INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''UsersHome'', 1, N''%SYSTEMDRIVE%\HostingSpaces''),
    (N''AspNet11Path'', 2, N''%SYSTEMROOT%\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll''),
    (N''AspNet11Pool'', 2, N''ASP.NET V1.1''),
    (N''AspNet20Path'', 2, N''%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll''),
    (N''AspNet20Pool'', 2, N''ASP.NET V2.0''),
    (N''AspNet40Path'', 2, N''%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll''),
    (N''AspNet40Pool'', 2, N''ASP.NET V4.0''),
    (N''AspPath'', 2, N''%SYSTEMROOT%\System32\InetSrv\asp.dll''),
    (N''CFFlashRemotingDirectory'', 2, N''C:\ColdFusion9\runtime\lib\wsconfig\1''),
    (N''CFScriptsDirectory'', 2, N''C:\Inetpub\wwwroot\CFIDE''),
    (N''ColdFusionPath'', 2, N''C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll''),
    (N''GalleryXmlFeedUrl'', 2, N''''),
    (N''PerlPath'', 2, N''%SYSTEMDRIVE%\Perl\bin\Perl.exe''),
    (N''Php4Path'', 2, N''%PROGRAMFILES%\PHP\php.exe''),
    (N''Php5Path'', 2, N''%PROGRAMFILES%\PHP\php-cgi.exe''),
    (N''ProtectedAccessFile'', 2, N''.htaccess''),
    (N''ProtectedFoldersFile'', 2, N''.htfolders''),
    (N''ProtectedGroupsFile'', 2, N''.htgroup''),
    (N''ProtectedUsersFile'', 2, N''.htpasswd''),
    (N''PythonPath'', 2, N''%SYSTEMDRIVE%\Python\python.exe''),
    (N''SecuredFoldersFilterPath'', 2, N''%SYSTEMROOT%\System32\InetSrv\IISPasswordFilter.dll''),
    (N''WebGroupName'', 2, N''SCPWebUsers''),
    (N''FtpGroupName'', 3, N''SCPFtpUsers''),
    (N''SiteId'', 3, N''MSFTPSVC/1''),
    (N''DatabaseLocation'', 5, N''%SYSTEMDRIVE%\SQL2000Databases\[USER_NAME]''),
    (N''ExternalAddress'', 5, N''(local)''),
    (N''InternalAddress'', 5, N''(local)''),
    (N''SaLogin'', 5, N''sa''),
    (N''SaPassword'', 5, N''''),
    (N''UseDefaultDatabaseLocation'', 5, N''True''),
    (N''UseTrustedConnection'', 5, N''True''),
    (N''ExternalAddress'', 6, N''localhost''),
    (N''InstallFolder'', 6, N''%PROGRAMFILES%\MySQL\MySQL Server 4.1''),
    (N''InternalAddress'', 6, N''localhost,3306''),
    (N''RootLogin'', 6, N''root''),
    (N''RootPassword'', 6, N''''),
    (N''ExpireLimit'', 7, N''1209600''),
    (N''MinimumTTL'', 7, N''86400''),
    (N''NameServers'', 7, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 7, N''86400''),
    (N''RecordMinimumTTL'', 7, N''3600''),
    (N''RefreshInterval'', 7, N''3600'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''ResponsiblePerson'', 7, N''hostmaster.[DOMAIN_NAME]''),
    (N''RetryDelay'', 7, N''600''),
    (N''AwStatsFolder'', 8, N''%SYSTEMDRIVE%\AWStats\wwwroot\cgi-bin''),
    (N''BatchFileName'', 8, N''UpdateStats.bat''),
    (N''BatchLineTemplate'', 8, N''%SYSTEMDRIVE%\perl\bin\perl.exe awstats.pl config=[DOMAIN_NAME] -update''),
    (N''ConfigFileName'', 8, N''awstats.[DOMAIN_NAME].conf''),
    (N''ConfigFileTemplate'', 8, CONCAT(CAST(N''LogFormat = "%time2 %other %other %other %method %url %other %other %logname %host %other %ua %other %referer %other %code %other %other %bytesd %other %other"'' AS nvarchar(max)), nchar(13), nchar(10), N''LogSeparator = " "'', nchar(13), nchar(10), N''DNSLookup = 2'', nchar(13), nchar(10), N''DirCgi = "/cgi-bin"'', nchar(13), nchar(10), N''DirIcons = "/icon"'', nchar(13), nchar(10), N''AllowFullYearView=3'', nchar(13), nchar(10), N''AllowToUpdateStatsFromBrowser = 0'', nchar(13), nchar(10), N''UseFramesWhenCGI = 1'', nchar(13), nchar(10), N''ShowFlagLinks = "en fr de it nl es"'', nchar(13), nchar(10), N''LogFile = "[LOGS_FOLDER]\ex%YY-3%MM-3%DD-3.log"'', nchar(13), nchar(10), N''DirData = "%SYSTEMDRIVE%\AWStats\data"'', nchar(13), nchar(10), N''SiteDomain = "[DOMAIN_NAME]"'', nchar(13), nchar(10), N''HostAliases = [DOMAIN_ALIASES]'')),
    (N''StatisticsURL'', 8, N''http://127.0.0.1/AWStats/cgi-bin/awstats.pl?config=[domain_name]''),
    (N''AdminLogin'', 9, N''Admin''),
    (N''ExpireLimit'', 9, N''1209600''),
    (N''MinimumTTL'', 9, N''86400''),
    (N''NameServers'', 9, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 9, N''86400''),
    (N''RecordMinimumTTL'', 9, N''3600''),
    (N''RefreshInterval'', 9, N''3600''),
    (N''ResponsiblePerson'', 9, N''hostmaster.[DOMAIN_NAME]''),
    (N''RetryDelay'', 9, N''600''),
    (N''SimpleDnsUrl'', 9, N''http://127.0.0.1:8053''),
    (N''LogDeleteDays'', 10, N''0''),
    (N''LogFormat'', 10, N''W3Cex''),
    (N''LogWildcard'', 10, N''*.log''),
    (N''Password'', 10, N''''),
    (N''ServerID'', 10, N''1''),
    (N''SmarterLogDeleteMonths'', 10, N''0''),
    (N''SmarterLogsPath'', 10, N''%SYSTEMDRIVE%\SmarterLogs''),
    (N''SmarterUrl'', 10, N''http://127.0.0.1:9999/services''),
    (N''StatisticsURL'', 10, N''http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin''),
    (N''TimeZoneId'', 10, N''27''),
    (N''Username'', 10, N''Admin''),
    (N''AdminPassword'', 11, N''''),
    (N''AdminUsername'', 11, N''admin''),
    (N''defaultdomainhostname'', 11, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 11, N''%SYSTEMDRIVE%\SmarterMail''),
    (N''ServerIPAddress'', 11, N''127.0.0.1;127.0.0.1''),
    (N''ServiceUrl'', 11, N''http://127.0.0.1:9998/services''),
    (N''InstallFolder'', 12, N''%PROGRAMFILES%\Gene6 FTP Server''),
    (N''LogsFolder'', 12, N''%PROGRAMFILES%\Gene6 FTP Server\Log''),
    (N''AdminPassword'', 14, N''''),
    (N''AdminUsername'', 14, N''admin''),
    (N''defaultdomainhostname'', 14, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 14, N''%SYSTEMDRIVE%\SmarterMail''),
    (N''ServerIPAddress'', 14, N''127.0.0.1;127.0.0.1'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''ServiceUrl'', 14, N''http://127.0.0.1:9998/services''),
    (N''BrowseMethod'', 16, N''POST''),
    (N''BrowseParameters'', 16, CONCAT(CAST(N''ServerName=[SERVER]'' AS nvarchar(max)), nchar(13), nchar(10), N''Login=[USER]'', nchar(13), nchar(10), N''Password=[PASSWORD]'', nchar(13), nchar(10), N''Protocol=dbmssocn'')),
    (N''BrowseURL'', 16, N''http://localhost/MLA/silentlogon.aspx''),
    (N''DatabaseLocation'', 16, N''%SYSTEMDRIVE%\SQL2005Databases\[USER_NAME]''),
    (N''ExternalAddress'', 16, N''(local)''),
    (N''InternalAddress'', 16, N''(local)''),
    (N''SaLogin'', 16, N''sa''),
    (N''SaPassword'', 16, N''''),
    (N''UseDefaultDatabaseLocation'', 16, N''True''),
    (N''UseTrustedConnection'', 16, N''True''),
    (N''ExternalAddress'', 17, N''localhost''),
    (N''InstallFolder'', 17, N''%PROGRAMFILES%\MySQL\MySQL Server 5.0''),
    (N''InternalAddress'', 17, N''localhost,3306''),
    (N''RootLogin'', 17, N''root''),
    (N''RootPassword'', 17, N''''),
    (N''AdminPassword'', 22, N''''),
    (N''AdminUsername'', 22, N''Administrator''),
    (N''BindConfigPath'', 24, N''c:\BIND\dns\etc\named.conf''),
    (N''BindReloadBatch'', 24, N''c:\BIND\dns\reload.bat''),
    (N''ExpireLimit'', 24, N''1209600''),
    (N''MinimumTTL'', 24, N''86400''),
    (N''NameServers'', 24, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 24, N''86400''),
    (N''RecordMinimumTTL'', 24, N''3600''),
    (N''RefreshInterval'', 24, N''3600''),
    (N''ResponsiblePerson'', 24, N''hostmaster.[DOMAIN_NAME]''),
    (N''RetryDelay'', 24, N''600''),
    (N''ZoneFileNameTemplate'', 24, N''db.[domain_name].txt''),
    (N''ZonesFolderPath'', 24, N''c:\BIND\dns\zones''),
    (N''DomainId'', 25, N''1''),
    (N''KeepDeletedItemsDays'', 27, N''14''),
    (N''KeepDeletedMailboxesDays'', 27, N''30''),
    (N''MailboxDatabase'', 27, N''Hosted Exchange Database''),
    (N''RootOU'', 27, N''SCP Hosting''),
    (N''StorageGroup'', 27, N''Hosted Exchange Storage Group''),
    (N''TempDomain'', 27, N''my-temp-domain.com''),
    (N''AdminLogin'', 28, N''Admin''),
    (N''ExpireLimit'', 28, N''1209600''),
    (N''MinimumTTL'', 28, N''86400''),
    (N''NameServers'', 28, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 28, N''86400'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''RecordMinimumTTL'', 28, N''3600''),
    (N''RefreshInterval'', 28, N''3600''),
    (N''ResponsiblePerson'', 28, N''hostmaster.[DOMAIN_NAME]''),
    (N''RetryDelay'', 28, N''600''),
    (N''SimpleDnsUrl'', 28, N''http://127.0.0.1:8053''),
    (N''AdminPassword'', 29, N'' ''),
    (N''AdminUsername'', 29, N''admin''),
    (N''defaultdomainhostname'', 29, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 29, N''%SYSTEMDRIVE%\SmarterMail''),
    (N''ServerIPAddress'', 29, N''127.0.0.1;127.0.0.1''),
    (N''ServiceUrl'', 29, N''http://localhost:9998/services/''),
    (N''ExternalAddress'', 30, N''localhost''),
    (N''InstallFolder'', 30, N''%PROGRAMFILES%\MySQL\MySQL Server 5.1''),
    (N''InternalAddress'', 30, N''localhost,3306''),
    (N''RootLogin'', 30, N''root''),
    (N''RootPassword'', 30, N''''),
    (N''LogDeleteDays'', 31, N''0''),
    (N''LogFormat'', 31, N''W3Cex''),
    (N''LogWildcard'', 31, N''*.log''),
    (N''Password'', 31, N''''),
    (N''ServerID'', 31, N''1''),
    (N''SmarterLogDeleteMonths'', 31, N''0''),
    (N''SmarterLogsPath'', 31, N''%SYSTEMDRIVE%\SmarterLogs''),
    (N''SmarterUrl'', 31, N''http://127.0.0.1:9999/services''),
    (N''StatisticsURL'', 31, N''http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin''),
    (N''TimeZoneId'', 31, N''27''),
    (N''Username'', 31, N''Admin''),
    (N''KeepDeletedItemsDays'', 32, N''14''),
    (N''KeepDeletedMailboxesDays'', 32, N''30''),
    (N''MailboxDatabase'', 32, N''Hosted Exchange Database''),
    (N''RootOU'', 32, N''SCP Hosting''),
    (N''TempDomain'', 32, N''my-temp-domain.com''),
    (N''RecordDefaultTTL'', 55, N''86400''),
    (N''RecordMinimumTTL'', 55, N''3600''),
    (N''ExpireLimit'', 56, N''1209600''),
    (N''MinimumTTL'', 56, N''86400''),
    (N''NameServers'', 56, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''PDNSDbName'', 56, N''pdnsdb''),
    (N''PDNSDbPort'', 56, N''3306''),
    (N''PDNSDbServer'', 56, N''localhost''),
    (N''PDNSDbUser'', 56, N''root''),
    (N''RecordDefaultTTL'', 56, N''86400'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''RecordMinimumTTL'', 56, N''3600''),
    (N''RefreshInterval'', 56, N''3600''),
    (N''ResponsiblePerson'', 56, N''hostmaster.[DOMAIN_NAME]''),
    (N''RetryDelay'', 56, N''600''),
    (N''AdminPassword'', 60, N'' ''),
    (N''AdminUsername'', 60, N''admin''),
    (N''defaultdomainhostname'', 60, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 60, N''%SYSTEMDRIVE%\SmarterMail''),
    (N''ServerIPAddress'', 60, N''127.0.0.1;127.0.0.1''),
    (N''ServiceUrl'', 60, N''http://localhost:9998/services/''),
    (N''LogDeleteDays'', 62, N''0''),
    (N''LogFormat'', 62, N''W3Cex''),
    (N''LogWildcard'', 62, N''*.log''),
    (N''Password'', 62, N''''),
    (N''ServerID'', 62, N''1''),
    (N''SmarterLogDeleteMonths'', 62, N''0''),
    (N''SmarterLogsPath'', 62, N''%SYSTEMDRIVE%\SmarterLogs''),
    (N''SmarterUrl'', 62, N''http://127.0.0.1:9999/services''),
    (N''StatisticsURL'', 62, N''http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin''),
    (N''TimeZoneId'', 62, N''27''),
    (N''Username'', 62, N''Admin''),
    (N''AdminPassword'', 63, N''''),
    (N''AdminUsername'', 63, N''Administrator''),
    (N''AdminPassword'', 64, N''''),
    (N''AdminUsername'', 64, N''admin''),
    (N''defaultdomainhostname'', 64, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 64, N''%SYSTEMDRIVE%\SmarterMail''),
    (N''ServerIPAddress'', 64, N''127.0.0.1;127.0.0.1''),
    (N''ServiceUrl'', 64, N''http://localhost:9998/services/''),
    (N''AdminPassword'', 65, N''''),
    (N''AdminUsername'', 65, N''admin''),
    (N''defaultdomainhostname'', 65, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 65, N''%SYSTEMDRIVE%\SmarterMail''),
    (N''ServerIPAddress'', 65, N''127.0.0.1;127.0.0.1''),
    (N''ServiceUrl'', 65, N''http://localhost:9998/services/''),
    (N''AdminPassword'', 66, N''''),
    (N''AdminUsername'', 66, N''admin''),
    (N''defaultdomainhostname'', 66, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 66, N''%SYSTEMDRIVE%\SmarterMail''),
    (N''ServerIPAddress'', 66, N''127.0.0.1;127.0.0.1''),
    (N''ServiceUrl'', 66, N''http://localhost:9998/services/''),
    (N''AdminPassword'', 67, N'''');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''AdminUsername'', 67, N''admin''),
    (N''defaultdomainhostname'', 67, N''mail.[DOMAIN_NAME]''),
    (N''DomainsPath'', 67, N''%SYSTEMDRIVE%\SmarterMail\Domains''),
    (N''ServerIPAddress'', 67, N''127.0.0.1;127.0.0.1''),
    (N''ServiceUrl'', 67, N''http://localhost:9998''),
    (N''UsersHome'', 100, N''%SYSTEMDRIVE%\HostingSpaces''),
    (N''AspNet11Pool'', 101, N''ASP.NET 1.1''),
    (N''AspNet40Path'', 101, N''%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll''),
    (N''AspNet40x64Path'', 101, N''%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll''),
    (N''AspNetBitnessMode'', 101, N''32''),
    (N''CFFlashRemotingDirectory'', 101, N''C:\ColdFusion9\runtime\lib\wsconfig\1''),
    (N''CFScriptsDirectory'', 101, N''C:\Inetpub\wwwroot\CFIDE''),
    (N''ClassicAspNet20Pool'', 101, N''ASP.NET 2.0 (Classic)''),
    (N''ClassicAspNet40Pool'', 101, N''ASP.NET 4.0 (Classic)''),
    (N''ColdFusionPath'', 101, N''C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll''),
    (N''GalleryXmlFeedUrl'', 101, N''''),
    (N''IntegratedAspNet20Pool'', 101, N''ASP.NET 2.0 (Integrated)''),
    (N''IntegratedAspNet40Pool'', 101, N''ASP.NET 4.0 (Integrated)''),
    (N''PerlPath'', 101, N''%SYSTEMDRIVE%\Perl\bin\PerlEx30.dll''),
    (N''Php4Path'', 101, N''%PROGRAMFILES(x86)%\PHP\php.exe''),
    (N''PhpMode'', 101, N''FastCGI''),
    (N''PhpPath'', 101, N''%PROGRAMFILES(x86)%\PHP\php-cgi.exe''),
    (N''ProtectedGroupsFile'', 101, N''.htgroup''),
    (N''ProtectedUsersFile'', 101, N''.htpasswd''),
    (N''SecureFoldersModuleAssembly'', 101, N''SolidCP.IIsModules.SecureFolders, SolidCP.IIsModules, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=37f9c58a0aa32ff0''),
    (N''WebGroupName'', 101, N''SCP_IUSRS''),
    (N''WmSvc.CredentialsMode'', 101, N''WINDOWS''),
    (N''WmSvc.Port'', 101, N''8172''),
    (N''FtpGroupName'', 102, N''SCPFtpUsers''),
    (N''SiteId'', 102, N''Default FTP Site''),
    (N''UsersHome'', 104, N''%SYSTEMDRIVE%\HostingSpaces''),
    (N''AspNet11Pool'', 105, N''ASP.NET 1.1''),
    (N''AspNet40Path'', 105, N''%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll''),
    (N''AspNet40x64Path'', 105, N''%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll''),
    (N''AspNetBitnessMode'', 105, N''32''),
    (N''CFFlashRemotingDirectory'', 105, N''C:\ColdFusion9\runtime\lib\wsconfig\1''),
    (N''CFScriptsDirectory'', 105, N''C:\Inetpub\wwwroot\CFIDE''),
    (N''ClassicAspNet20Pool'', 105, N''ASP.NET 2.0 (Classic)''),
    (N''ClassicAspNet40Pool'', 105, N''ASP.NET 4.0 (Classic)''),
    (N''ColdFusionPath'', 105, N''C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll''),
    (N''GalleryXmlFeedUrl'', 105, N''''),
    (N''IntegratedAspNet20Pool'', 105, N''ASP.NET 2.0 (Integrated)'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''IntegratedAspNet40Pool'', 105, N''ASP.NET 4.0 (Integrated)''),
    (N''PerlPath'', 105, N''%SYSTEMDRIVE%\Perl\bin\PerlEx30.dll''),
    (N''Php4Path'', 105, N''%PROGRAMFILES(x86)%\PHP\php.exe''),
    (N''PhpMode'', 105, N''FastCGI''),
    (N''PhpPath'', 105, N''%PROGRAMFILES(x86)%\PHP\php-cgi.exe''),
    (N''ProtectedGroupsFile'', 105, N''.htgroup''),
    (N''ProtectedUsersFile'', 105, N''.htpasswd''),
    (N''SecureFoldersModuleAssembly'', 105, N''SolidCP.IIsModules.SecureFolders, SolidCP.IIsModules, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=37f9c58a0aa32ff0''),
    (N''sslusesni'', 105, N''True''),
    (N''WebGroupName'', 105, N''SCP_IUSRS''),
    (N''WmSvc.CredentialsMode'', 105, N''WINDOWS''),
    (N''WmSvc.Port'', 105, N''8172''),
    (N''FtpGroupName'', 106, N''SCPFtpUsers''),
    (N''SiteId'', 106, N''Default FTP Site''),
    (N''sslusesni'', 106, N''False''),
    (N''UsersHome'', 111, N''%SYSTEMDRIVE%\HostingSpaces''),
    (N''AspNet11Pool'', 112, N''ASP.NET 1.1''),
    (N''AspNet40Path'', 112, N''%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll''),
    (N''AspNet40x64Path'', 112, N''%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll''),
    (N''AspNetBitnessMode'', 112, N''32''),
    (N''CFFlashRemotingDirectory'', 112, N''C:\ColdFusion9\runtime\lib\wsconfig\1''),
    (N''CFScriptsDirectory'', 112, N''C:\Inetpub\wwwroot\CFIDE''),
    (N''ClassicAspNet20Pool'', 112, N''ASP.NET 2.0 (Classic)''),
    (N''ClassicAspNet40Pool'', 112, N''ASP.NET 4.0 (Classic)''),
    (N''ColdFusionPath'', 112, N''C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll''),
    (N''GalleryXmlFeedUrl'', 112, N''''),
    (N''IntegratedAspNet20Pool'', 112, N''ASP.NET 2.0 (Integrated)''),
    (N''IntegratedAspNet40Pool'', 112, N''ASP.NET 4.0 (Integrated)''),
    (N''PerlPath'', 112, N''%SYSTEMDRIVE%\Perl\bin\PerlEx30.dll''),
    (N''Php4Path'', 112, N''%PROGRAMFILES(x86)%\PHP\php.exe''),
    (N''PhpMode'', 112, N''FastCGI''),
    (N''PhpPath'', 112, N''%PROGRAMFILES(x86)%\PHP\php-cgi.exe''),
    (N''ProtectedGroupsFile'', 112, N''.htgroup''),
    (N''ProtectedUsersFile'', 112, N''.htpasswd''),
    (N''SecureFoldersModuleAssembly'', 112, N''SolidCP.IIsModules.SecureFolders, SolidCP.IIsModules, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=37f9c58a0aa32ff0''),
    (N''sslusesni'', 112, N''True''),
    (N''WebGroupName'', 112, N''SCP_IUSRS''),
    (N''WmSvc.CredentialsMode'', 112, N''WINDOWS''),
    (N''WmSvc.Port'', 112, N''8172''),
    (N''FtpGroupName'', 113, N''SCPFtpUsers''),
    (N''SiteId'', 113, N''Default FTP Site''),
    (N''sslusesni'', 113, N''False'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''RootWebApplicationIpAddress'', 200, N''''),
    (N''UserName'', 204, N''admin''),
    (N''UtilityPath'', 204, N''C:\Program Files\Research In Motion\BlackBerry Enterprise Server Resource Kit\BlackBerry Enterprise Server User Administration Tool''),
    (N''CpuLimit'', 300, N''100''),
    (N''CpuReserve'', 300, N''0''),
    (N''CpuWeight'', 300, N''100''),
    (N''DvdLibraryPath'', 300, N''C:\Hyper-V\Library''),
    (N''ExportedVpsPath'', 300, N''C:\Hyper-V\Exported''),
    (N''HostnamePattern'', 300, N''vps[user_id].hosterdomain.com''),
    (N''OsTemplatesPath'', 300, N''C:\Hyper-V\Templates''),
    (N''PrivateNetworkFormat'', 300, N''192.168.0.1/16''),
    (N''RootFolder'', 300, N''C:\Hyper-V\VirtualMachines\[VPS_HOSTNAME]''),
    (N''StartAction'', 300, N''start''),
    (N''StartupDelay'', 300, N''0''),
    (N''StopAction'', 300, N''shutDown''),
    (N''VirtualDiskType'', 300, N''dynamic''),
    (N''ExternalAddress'', 301, N''localhost''),
    (N''InstallFolder'', 301, N''%PROGRAMFILES%\MySQL\MySQL Server 5.5''),
    (N''InternalAddress'', 301, N''localhost,3306''),
    (N''RootLogin'', 301, N''root''),
    (N''RootPassword'', 301, N''''),
    (N''ExternalAddress'', 304, N''localhost''),
    (N''InstallFolder'', 304, N''%PROGRAMFILES%\MySQL\MySQL Server 8.0''),
    (N''InternalAddress'', 304, N''localhost,3306''),
    (N''RootLogin'', 304, N''root''),
    (N''RootPassword'', 304, N''''),
    (N''sslmode'', 304, N''True''),
    (N''ExternalAddress'', 305, N''localhost''),
    (N''InstallFolder'', 305, N''%PROGRAMFILES%\MySQL\MySQL Server 8.0''),
    (N''InternalAddress'', 305, N''localhost,3306''),
    (N''RootLogin'', 305, N''root''),
    (N''RootPassword'', 305, N''''),
    (N''sslmode'', 305, N''True''),
    (N''ExternalAddress'', 306, N''localhost''),
    (N''InstallFolder'', 306, N''%PROGRAMFILES%\MySQL\MySQL Server 8.0''),
    (N''InternalAddress'', 306, N''localhost,3306''),
    (N''RootLogin'', 306, N''root''),
    (N''RootPassword'', 306, N''''),
    (N''sslmode'', 306, N''True''),
    (N''ExternalAddress'', 307, N''localhost''),
    (N''InstallFolder'', 307, N''%PROGRAMFILES%\MySQL\MySQL Server 8.0''),
    (N''InternalAddress'', 307, N''localhost,3306'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''RootLogin'', 307, N''root''),
    (N''RootPassword'', 307, N''''),
    (N''sslmode'', 307, N''True''),
    (N''ExternalAddress'', 308, N''localhost''),
    (N''InstallFolder'', 308, N''%PROGRAMFILES%\MySQL\MySQL Server 8.0''),
    (N''InternalAddress'', 308, N''localhost,3306''),
    (N''RootLogin'', 308, N''root''),
    (N''RootPassword'', 308, N''''),
    (N''sslmode'', 308, N''True''),
    (N''ExternalAddress'', 320, N''localhost''),
    (N''InstallFolder'', 320, N''%PROGRAMFILES%\MySQL\MySQL Server 9.0''),
    (N''InternalAddress'', 320, N''localhost,3306''),
    (N''RootLogin'', 320, N''root''),
    (N''RootPassword'', 320, N''''),
    (N''sslmode'', 320, N''True''),
    (N''admode'', 410, N''False''),
    (N''expirelimit'', 410, N''1209600''),
    (N''minimumttl'', 410, N''86400''),
    (N''nameservers'', 410, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 410, N''86400''),
    (N''RecordMinimumTTL'', 410, N''3600''),
    (N''refreshinterval'', 410, N''3600''),
    (N''responsibleperson'', 410, N''hostmaster.[DOMAIN_NAME]''),
    (N''retrydelay'', 410, N''600''),
    (N''LogDir'', 500, N''/var/log''),
    (N''UsersHome'', 500, N''/var/www/HostingSpaces''),
    (N''ExternalAddress'', 1550, N''localhost''),
    (N''InstallFolder'', 1550, N''%PROGRAMFILES%\MariaDB 10.1''),
    (N''InternalAddress'', 1550, N''localhost''),
    (N''RootLogin'', 1550, N''root''),
    (N''RootPassword'', 1550, N''''),
    (N''ExternalAddress'', 1570, N''localhost''),
    (N''InstallFolder'', 1570, N''%PROGRAMFILES%\MariaDB 10.3''),
    (N''InternalAddress'', 1570, N''localhost''),
    (N''RootLogin'', 1570, N''root''),
    (N''RootPassword'', 1570, N''''),
    (N''ExternalAddress'', 1571, N''localhost''),
    (N''InstallFolder'', 1571, N''%PROGRAMFILES%\MariaDB 10.4''),
    (N''InternalAddress'', 1571, N''localhost''),
    (N''RootLogin'', 1571, N''root''),
    (N''RootPassword'', 1571, N''''),
    (N''ExternalAddress'', 1572, N''localhost'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''InstallFolder'', 1572, N''%PROGRAMFILES%\MariaDB 10.5''),
    (N''InternalAddress'', 1572, N''localhost''),
    (N''RootLogin'', 1572, N''root''),
    (N''RootPassword'', 1572, N''''),
    (N''ExternalAddress'', 1573, N''localhost''),
    (N''InstallFolder'', 1573, N''%PROGRAMFILES%\MariaDB 10.6''),
    (N''InternalAddress'', 1573, N''localhost''),
    (N''RootLogin'', 1573, N''root''),
    (N''RootPassword'', 1573, N''''),
    (N''ExternalAddress'', 1574, N''localhost''),
    (N''InstallFolder'', 1574, N''%PROGRAMFILES%\MariaDB 10.7''),
    (N''InternalAddress'', 1574, N''localhost''),
    (N''RootLogin'', 1574, N''root''),
    (N''RootPassword'', 1574, N''''),
    (N''ExternalAddress'', 1575, N''localhost''),
    (N''InstallFolder'', 1575, N''%PROGRAMFILES%\MariaDB 10.8''),
    (N''InternalAddress'', 1575, N''localhost''),
    (N''RootLogin'', 1575, N''root''),
    (N''RootPassword'', 1575, N''''),
    (N''ExternalAddress'', 1576, N''localhost''),
    (N''InstallFolder'', 1576, N''%PROGRAMFILES%\MariaDB 10.9''),
    (N''InternalAddress'', 1576, N''localhost''),
    (N''RootLogin'', 1576, N''root''),
    (N''RootPassword'', 1576, N''''),
    (N''ExternalAddress'', 1577, N''localhost''),
    (N''InstallFolder'', 1577, N''%PROGRAMFILES%\MariaDB 10.10''),
    (N''InternalAddress'', 1577, N''localhost''),
    (N''RootLogin'', 1577, N''root''),
    (N''RootPassword'', 1577, N''''),
    (N''ExternalAddress'', 1578, N''localhost''),
    (N''InstallFolder'', 1578, N''%PROGRAMFILES%\MariaDB 10.11''),
    (N''InternalAddress'', 1578, N''localhost''),
    (N''RootLogin'', 1578, N''root''),
    (N''RootPassword'', 1578, N''''),
    (N''ExternalAddress'', 1579, N''localhost''),
    (N''InstallFolder'', 1579, N''%PROGRAMFILES%\MariaDB 11.0''),
    (N''InternalAddress'', 1579, N''localhost''),
    (N''RootLogin'', 1579, N''root''),
    (N''RootPassword'', 1579, N''''),
    (N''ExternalAddress'', 1580, N''localhost''),
    (N''InstallFolder'', 1580, N''%PROGRAMFILES%\MariaDB 11.1''),
    (N''InternalAddress'', 1580, N''localhost'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''RootLogin'', 1580, N''root''),
    (N''RootPassword'', 1580, N''''),
    (N''ExternalAddress'', 1581, N''localhost''),
    (N''InstallFolder'', 1581, N''%PROGRAMFILES%\MariaDB 11.2''),
    (N''InternalAddress'', 1581, N''localhost''),
    (N''RootLogin'', 1581, N''root''),
    (N''RootPassword'', 1581, N''''),
    (N''ExternalAddress'', 1582, N''localhost''),
    (N''InstallFolder'', 1582, N''%PROGRAMFILES%\MariaDB 11.3''),
    (N''InternalAddress'', 1582, N''localhost''),
    (N''RootLogin'', 1582, N''root''),
    (N''RootPassword'', 1582, N''''),
    (N''ExternalAddress'', 1583, N''localhost''),
    (N''InstallFolder'', 1583, N''%PROGRAMFILES%\MariaDB 11.4''),
    (N''InternalAddress'', 1583, N''localhost''),
    (N''RootLogin'', 1583, N''root''),
    (N''RootPassword'', 1583, N''''),
    (N''ExternalAddress'', 1584, N''localhost''),
    (N''InstallFolder'', 1584, N''%PROGRAMFILES%\MariaDB 11.5''),
    (N''InternalAddress'', 1584, N''localhost''),
    (N''RootLogin'', 1584, N''root''),
    (N''RootPassword'', 1584, N''''),
    (N''ExternalAddress'', 1585, N''localhost''),
    (N''InstallFolder'', 1585, N''%PROGRAMFILES%\MariaDB 11.6''),
    (N''InternalAddress'', 1585, N''localhost''),
    (N''RootLogin'', 1585, N''root''),
    (N''RootPassword'', 1585, N''''),
    (N''ExternalAddress'', 1586, N''localhost''),
    (N''InstallFolder'', 1586, N''%PROGRAMFILES%\MariaDB 11.7''),
    (N''InternalAddress'', 1586, N''localhost''),
    (N''RootLogin'', 1586, N''root''),
    (N''RootPassword'', 1586, N''''),
    (N''RecordDefaultTTL'', 1703, N''86400''),
    (N''RecordMinimumTTL'', 1703, N''3600''),
    (N''UsersHome'', 1800, N''%SYSTEMDRIVE%\HostingSpaces''),
    (N''UsersHome'', 1802, N''%SYSTEMDRIVE%\HostingSpaces''),
    (N''UsersHome'', 1804, N''%SYSTEMDRIVE%\HostingSpaces''),
    (N''AdminLogin'', 1901, N''Admin''),
    (N''ExpireLimit'', 1901, N''1209600''),
    (N''MinimumTTL'', 1901, N''86400''),
    (N''NameServers'', 1901, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 1901, N''86400'');
    INSERT INTO [ServiceDefaultProperties] ([PropertyName], [ProviderID], [PropertyValue])
    VALUES (N''RecordMinimumTTL'', 1901, N''3600''),
    (N''RefreshInterval'', 1901, N''3600''),
    (N''ResponsiblePerson'', 1901, N''hostmaster.[DOMAIN_NAME]''),
    (N''RetryDelay'', 1901, N''600''),
    (N''SimpleDnsUrl'', 1901, N''http://127.0.0.1:8053''),
    (N''admode'', 1902, N''False''),
    (N''expirelimit'', 1902, N''1209600''),
    (N''minimumttl'', 1902, N''86400''),
    (N''nameservers'', 1902, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 1902, N''86400''),
    (N''RecordMinimumTTL'', 1902, N''3600''),
    (N''refreshinterval'', 1902, N''3600''),
    (N''responsibleperson'', 1902, N''hostmaster.[DOMAIN_NAME]''),
    (N''retrydelay'', 1902, N''600''),
    (N''AdminLogin'', 1903, N''Admin''),
    (N''ExpireLimit'', 1903, N''1209600''),
    (N''NameServers'', 1903, N''ns1.yourdomain.com;ns2.yourdomain.com''),
    (N''RecordDefaultTTL'', 1903, N''86400''),
    (N''RecordMinimumTTL'', 1903, N''3600''),
    (N''RefreshInterval'', 1903, N''3600''),
    (N''ResponsiblePerson'', 1903, N''hostmaster.[DOMAIN_NAME]''),
    (N''RetryDelay'', 1903, N''600''),
    (N''SimpleDnsUrl'', 1903, N''http://127.0.0.1:8053''),
    (N''ConfigFile'', 1910, N''/etc/vsftpd.conf''),
    (N''BinPath'', 1911, N''''),
    (N''ConfigFile'', 1911, N''/etc/apache2/apache2.conf''),
    (N''ConfigPath'', 1911, N''/etc/apache2'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PropertyName', N'ProviderID', N'PropertyValue') AND [object_id] = OBJECT_ID(N'[ServiceDefaultProperties]'))
        SET IDENTITY_INSERT [ServiceDefaultProperties] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ParameterID', N'ScheduleID', N'ParameterValue') AND [object_id] = OBJECT_ID(N'[ScheduleParameters]'))
        SET IDENTITY_INSERT [ScheduleParameters] ON;
    EXEC(N'INSERT INTO [ScheduleParameters] ([ParameterID], [ScheduleID], [ParameterValue])
    VALUES (N''SUSPEND_OVERUSED'', 1, N''false''),
    (N''SUSPEND_OVERUSED'', 2, N''false'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ParameterID', N'ScheduleID', N'ParameterValue') AND [object_id] = OBJECT_ID(N'[ScheduleParameters]'))
        SET IDENTITY_INSERT [ScheduleParameters] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [AccessTokensIdx_AccountID] ON [AccessTokens] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [BackgroundTaskLogsIdx_TaskID] ON [BackgroundTaskLogs] ([TaskID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [BackgroundTaskParametersIdx_TaskID] ON [BackgroundTaskParameters] ([TaskID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [BackgroundTaskStackIdx_TaskID] ON [BackgroundTaskStack] ([TaskID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [BlackBerryUsersIdx_AccountId] ON [BlackBerryUsers] ([AccountId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [CommentsIdx_UserID] ON [Comments] ([UserID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [CRMUsersIdx_AccountID] ON [CRMUsers] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [DmzIPAddressesIdx_ItemID] ON [DmzIPAddresses] ([ItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [DomainDnsRecordsIdx_DomainId] ON [DomainDnsRecords] ([DomainId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [DomainsIdx_MailDomainID] ON [Domains] ([MailDomainID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [DomainsIdx_PackageID] ON [Domains] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [DomainsIdx_WebSiteID] ON [Domains] ([WebSiteID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [DomainsIdx_ZoneItemID] ON [Domains] ([ZoneItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [EnterpriseFoldersIdx_StorageSpaceFolderId] ON [EnterpriseFolders] ([StorageSpaceFolderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [EnterpriseFoldersOwaPermissionsIdx_AccountID] ON [EnterpriseFoldersOwaPermissions] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [EnterpriseFoldersOwaPermissionsIdx_FolderID] ON [EnterpriseFoldersOwaPermissions] ([FolderID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeAccountEmailAddressesIdx_AccountID] ON [ExchangeAccountEmailAddresses] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ExchangeAccountEmailAddresses_UniqueEmail] ON [ExchangeAccountEmailAddresses] ([EmailAddress]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeAccountsIdx_ItemID] ON [ExchangeAccounts] ([ItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeAccountsIdx_MailboxPlanId] ON [ExchangeAccounts] ([MailboxPlanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ExchangeAccounts_UniqueAccountName] ON [ExchangeAccounts] ([AccountName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeMailboxPlansIdx_ItemID] ON [ExchangeMailboxPlans] ([ItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ExchangeMailboxPlans] ON [ExchangeMailboxPlans] ([MailboxPlanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeOrganizationDomainsIdx_ItemID] ON [ExchangeOrganizationDomains] ([ItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_ExchangeOrganizationDomains_UniqueDomain] ON [ExchangeOrganizationDomains] ([DomainID]) WHERE [DomainID] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ExchangeOrganizations_UniqueOrg] ON [ExchangeOrganizations] ([OrganizationID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeOrganizationSettingsIdx_ItemId] ON [ExchangeOrganizationSettings] ([ItemId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeOrganizationSsFoldersIdx_ItemId] ON [ExchangeOrganizationSsFolders] ([ItemId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ExchangeOrganizationSsFoldersIdx_StorageSpaceFolderId] ON [ExchangeOrganizationSsFolders] ([StorageSpaceFolderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [GlobalDnsRecordsIdx_IPAddressID] ON [GlobalDnsRecords] ([IPAddressID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [GlobalDnsRecordsIdx_PackageID] ON [GlobalDnsRecords] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [GlobalDnsRecordsIdx_ServerID] ON [GlobalDnsRecords] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [GlobalDnsRecordsIdx_ServiceID] ON [GlobalDnsRecords] ([ServiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HostingPlanQuotas_QuotaID] ON [HostingPlanQuotas] ([QuotaID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HostingPlanResources_GroupID] ON [HostingPlanResources] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [HostingPlansIdx_PackageID] ON [HostingPlans] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [HostingPlansIdx_ServerID] ON [HostingPlans] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [HostingPlansIdx_UserID] ON [HostingPlans] ([UserID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IPAddressesIdx_ServerID] ON [IPAddresses] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_LyncUserPlans] ON [LyncUserPlans] ([LyncUserPlanId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [LyncUserPlansIdx_ItemID] ON [LyncUserPlans] ([ItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [LyncUsersIdx_LyncUserPlanID] ON [LyncUsers] ([LyncUserPlanID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageAddonsIdx_PackageID] ON [PackageAddons] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageAddonsIdx_PlanID] ON [PackageAddons] ([PlanID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageIPAddressesIdx_AddressID] ON [PackageIPAddresses] ([AddressID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageIPAddressesIdx_ItemID] ON [PackageIPAddresses] ([ItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageIPAddressesIdx_PackageID] ON [PackageIPAddresses] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackageQuotas_QuotaID] ON [PackageQuotas] ([QuotaID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackageResources_GroupID] ON [PackageResources] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageIndex_ParentPackageID] ON [Packages] ([ParentPackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageIndex_PlanID] ON [Packages] ([PlanID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageIndex_ServerID] ON [Packages] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageIndex_UserID] ON [Packages] ([UserID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackagesBandwidth_GroupID] ON [PackagesBandwidth] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackagesDiskspace_GroupID] ON [PackagesDiskspace] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackageServices_ServiceID] ON [PackageServices] ([ServiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackagesTreeCache_PackageID] ON [PackagesTreeCache] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageVLANsIdx_PackageID] ON [PackageVLANs] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PackageVLANsIdx_VlanID] ON [PackageVLANs] ([VlanID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PrivateIPAddressesIdx_ItemID] ON [PrivateIPAddresses] ([ItemID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [PrivateNetworkVLANsIdx_ServerID] ON [PrivateNetworkVLANs] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ProvidersIdx_GroupID] ON [Providers] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [QuotasIdx_GroupID] ON [Quotas] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [QuotasIdx_ItemTypeID] ON [Quotas] ([ItemTypeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [RDSCollectionSettingsIdx_RDSCollectionId] ON [RDSCollectionSettings] ([RDSCollectionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [RDSCollectionUsersIdx_AccountID] ON [RDSCollectionUsers] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [RDSCollectionUsersIdx_RDSCollectionId] ON [RDSCollectionUsers] ([RDSCollectionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [RDSMessagesIdx_RDSCollectionId] ON [RDSMessages] ([RDSCollectionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [RDSServersIdx_RDSCollectionId] ON [RDSServers] ([RDSCollectionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ResourceGroupDnsRecordsIdx_GroupID] ON [ResourceGroupDnsRecords] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ScheduleIdx_PackageID] ON [Schedule] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ScheduleIdx_TaskID] ON [Schedule] ([TaskID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ScheduleTaskViewConfiguration_TaskID] ON [ScheduleTaskViewConfiguration] ([TaskID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServersIdx_PrimaryGroupID] ON [Servers] ([PrimaryGroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServiceItemsIdx_ItemTypeID] ON [ServiceItems] ([ItemTypeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServiceItemsIdx_PackageID] ON [ServiceItems] ([PackageID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServiceItemsIdx_ServiceID] ON [ServiceItems] ([ServiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServiceItemTypesIdx_GroupID] ON [ServiceItemTypes] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServicesIdx_ClusterID] ON [Services] ([ClusterID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServicesIdx_ProviderID] ON [Services] ([ProviderID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ServicesIdx_ServerID] ON [Services] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [StorageSpaceFoldersIdx_StorageSpaceId] ON [StorageSpaceFolders] ([StorageSpaceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [StorageSpaceLevelResourceGroupsIdx_GroupId] ON [StorageSpaceLevelResourceGroups] ([GroupId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [StorageSpaceLevelResourceGroupsIdx_LevelId] ON [StorageSpaceLevelResourceGroups] ([LevelId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [StorageSpacesIdx_ServerId] ON [StorageSpaces] ([ServerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [StorageSpacesIdx_ServiceId] ON [StorageSpaces] ([ServiceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_TempIds_Created_Scope_Level] ON [TempIds] ([Created], [Scope], [Level]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [ThemeSettingsIdx_ThemeID] ON [ThemeSettings] ([ThemeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]) WHERE [Username] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [UsersIdx_OwnerID] ON [Users] ([OwnerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [VirtualGroupsIdx_GroupID] ON [VirtualGroups] ([GroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [VirtualGroupsIdx_ServerID] ON [VirtualGroups] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [VirtualServicesIdx_ServerID] ON [VirtualServices] ([ServerID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [VirtualServicesIdx_ServiceID] ON [VirtualServices] ([ServiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [WebDavAccessTokensIdx_AccountID] ON [WebDavAccessTokens] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    CREATE INDEX [WebDavPortalUsersSettingsIdx_AccountId] ON [WebDavPortalUsersSettings] ([AccountId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250123133717_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250123133717_InitialCreate', N'9.0.1');
END;

COMMIT;
GO


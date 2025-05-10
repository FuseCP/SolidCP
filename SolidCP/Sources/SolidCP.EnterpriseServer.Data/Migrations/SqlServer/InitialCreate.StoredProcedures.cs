using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SolidCP.EnterpriseServer.Data.Migrations.SqlServer
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		#region Stored Procedures
		protected void StoredProceduresUp(MigrationBuilder migrationBuilder)
		{
			StoredProceduresDown(migrationBuilder);

			if (migrationBuilder.IsSqlServer())
				migrationBuilder.SqlScript(@"!\.SqlServer\.StoredProcedures\.2\.0\.0\..*\.sql$");
		}

		protected void StoredProceduresDown(MigrationBuilder migrationBuilder)
		{
			if (migrationBuilder.IsSqlServer()) migrationBuilder.SafeSql(@"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateWhoisDomainInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateWhoisDomainInfo]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateWebDavPortalUsersSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateWebDavPortalUsersSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateVirtualGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateVirtualGroups]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserThemeSetting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserThemeSetting]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserPinSecret]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserPinSecret]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserMfaMode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserMfaMode]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserFailedLoginAttempt]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserFailedLoginAttempt]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateSupportServiceLevel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateSupportServiceLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateStorageSpaceLevel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateStorageSpaceLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateStorageSpaceFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateStorageSpaceFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateStorageSpace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateStorageSpace]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateSfBUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateSfBUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateSfBUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateSfBUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateServiceProperties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateServiceProperties]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateServiceItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateServiceItem]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateServiceFully]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateServiceFully]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateSchedule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateSchedule]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRDSServerSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateRDSServerSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRDSServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateRDSServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRDSCollectionSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateRDSCollectionSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRDSCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateRDSCollection]
GO
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE object_id = OBJECT_ID(N'[dbo].[UpdateQuotaHidden]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateQuotaHidden]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePrivateNetworVLAN]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePrivateNetworVLAN]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackageSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackageQuotas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackageQuotas]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackageName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackageName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackageDiskSpace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackageDiskSpace]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackageBandwidthUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackageBandwidthUpdate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackageBandwidth]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackageBandwidth]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackageAddon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackageAddon]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdatePackage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateLyncUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateLyncUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateLyncUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateLyncUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateHostingPlanQuotas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateHostingPlanQuotas]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExchangeRetentionPolicyTag]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExchangeRetentionPolicyTag]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExchangeOrganizationSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExchangeOrganizationSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExchangeMailboxPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExchangeMailboxPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExchangeDisclaimer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExchangeDisclaimer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExchangeAccountUserPrincipalName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExchangeAccountUserPrincipalName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExchangeAccountSLSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExchangeAccountSLSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExchangeAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExchangeAccount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEnterpriseFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateEnterpriseFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEntepriseFolderStorageSpaceFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateEntepriseFolderStorageSpaceFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDomainLastUpdateDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDomainLastUpdateDate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDomainExpirationDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDomainExpirationDate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDomainDates]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDomainDates]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDomainCreationDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDomainCreationDate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDnsRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDnsRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCRMUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateCRMUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateBackgroundTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateBackgroundTask]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAdditionalGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateAdditionalGroup]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SfBUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SfBUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetUserOneTimePassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetUserOneTimePassword]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetSystemSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetSystemSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetSfBUserSfBUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetSfBUserSfBUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetOrganizationDefaultSfBUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetOrganizationDefaultSfBUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetOrganizationDefaultLyncUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetOrganizationDefaultLyncUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetOrganizationDefaultExchangeMailboxPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetOrganizationDefaultExchangeMailboxPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetLyncUserLyncUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetLyncUserLyncUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetItemPrivatePrimaryIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetItemPrivatePrimaryIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetItemPrimaryIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetItemPrimaryIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetItemDmzPrimaryIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetItemDmzPrimaryIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetExchangeAccountMailboxplan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetExchangeAccountMailboxplan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetExchangeAccountDisclaimerId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetExchangeAccountDisclaimerId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetAccessTokenSmsResponse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetAccessTokenSmsResponse]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchServiceItemsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchServiceItemsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchOrganizationAccounts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchOrganizationAccounts]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchExchangeAccountsByTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchExchangeAccountsByTypes]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchExchangeAccounts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchExchangeAccounts]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchExchangeAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchExchangeAccount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveStorageSpaceLevel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RemoveStorageSpaceLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveStorageSpaceFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RemoveStorageSpaceFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveStorageSpace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RemoveStorageSpace]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveRDSUserFromRDSCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RemoveRDSUserFromRDSCollection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveRDSServerFromOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RemoveRDSServerFromOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveRDSServerFromCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RemoveRDSServerFromCollection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrganizationUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrganizationUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrganizationExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrganizationExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoveServiceItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MoveServiceItem]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LyncUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[LyncUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertStorageSpaceLevel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertStorageSpaceLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertStorageSpace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertStorageSpace]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertCRMUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertCRMUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWebDavPortalUsersSettingsByAccountId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetWebDavPortalUsersSettingsByAccountId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWebDavAccessTokenById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetWebDavAccessTokenById]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWebDavAccessTokenByAccessToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetWebDavAccessTokenByAccessToken]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualServices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVirtualServices]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualServers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVirtualServers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualMachinesPagedProxmox]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVirtualMachinesPagedProxmox]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualMachinesPagedForPC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVirtualMachinesPagedForPC]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualMachinesPaged2012]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVirtualMachinesPaged2012]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualMachinesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVirtualMachinesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUsersSummary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUsersSummary]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUsersPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUsersPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserServiceID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserServiceID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserPeers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserPeers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserParents]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserParents]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserPackagesServerUrls]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserPackagesServerUrls]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserEnterpriseFolderWithOwaEditPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserEnterpriseFolderWithOwaEditPermission]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserDomainsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserDomainsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserByUsernameInternally]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserByUsernameInternally]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserByUsername]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserByUsername]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserByIdInternally]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserByIdInternally]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserById]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserByExchangeOrganizationIdInternally]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserByExchangeOrganizationIdInternally]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAvailableHostingPlans]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserAvailableHostingPlans]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAvailableHostingAddons]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserAvailableHostingAddons]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUnallottedVLANs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUnallottedVLANs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUnallottedIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUnallottedIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetThreadBackgroundTasks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetThreadBackgroundTasks]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetThemeSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetThemeSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetThemeSetting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetThemeSetting]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetThemes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetThemes]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSystemSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSystemSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSupportServiceLevels]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSupportServiceLevels]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSupportServiceLevel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSupportServiceLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpacesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpacesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpacesByResourceGroupName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpacesByResourceGroupName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpacesByLevelId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpacesByLevelId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpaceLevelsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpaceLevelsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpaceLevelById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpaceLevelById]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpaceFoldersByStorageSpaceId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpaceFoldersByStorageSpaceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpaceFolderById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpaceFolderById]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpaceByServiceAndPath]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpaceByServiceAndPath]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStorageSpaceById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStorageSpaceById]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSSLCertificateByID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSSLCertificateByID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSiteCert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSiteCert]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSfBUsersCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSfBUsersCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSfBUsersByPlanId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSfBUsersByPlanId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSfBUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSfBUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSfBUserPlans]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSfBUserPlans]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSfBUserPlanByAccountId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSfBUserPlanByAccountId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSfBUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSfBUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServicesByServerIDGroupName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServicesByServerIDGroupName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServicesByServerID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServicesByServerID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServicesByGroupName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServicesByGroupName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServicesByGroupID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServicesByGroupID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceProperties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceProperties]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemTypes]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemsForStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemsForStatistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemsCountByNameAndServiceId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemsCountByNameAndServiceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemsCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemsCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemsByService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemsByService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemsByPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemsByPackage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemsByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemsByName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItems]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItemByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItemByName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServiceItem]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServerShortDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServerShortDetails]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServerInternal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServerInternal]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServerByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServerByName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSearchTableByColumns]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSearchTableByColumns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSearchObject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSearchObject]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSearchableServiceItemTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSearchableServiceItemTypes]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduleTaskViewConfigurations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetScheduleTaskViewConfigurations]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduleTasks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetScheduleTasks]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduleTaskEmailTemplate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetScheduleTaskEmailTemplate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduleTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetScheduleTask]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSchedulesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSchedulesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSchedules]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSchedules]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduleParameters]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetScheduleParameters]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduleInternal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetScheduleInternal]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduleBackgroundTasks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetScheduleBackgroundTasks]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSchedule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSchedule]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetResourceGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetResourceGroups]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetResourceGroupByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetResourceGroupByName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetResourceGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetResourceGroup]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetResellerDomains]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetResellerDomains]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSServersPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSServersPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSServerSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSServerSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSServersByItemId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSServersByItemId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSServersByCollectionId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSServersByCollectionId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSServers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSServers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSServerById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSServerById]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSMessages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSMessages]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSControllerServiceIDbyFQDN]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSControllerServiceIDbyFQDN]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSCollectionUsersByRDSCollectionId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSCollectionUsersByRDSCollectionId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSCollectionsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSCollectionsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSCollectionSettingsByCollectionId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSCollectionSettingsByCollectionId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSCollectionsByItemId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSCollectionsByItemId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSCollectionByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSCollectionByName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSCollectionById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSCollectionById]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRDSCertificateByServiceId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRDSCertificateByServiceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRawServicesByServerID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRawServicesByServerID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetQuotas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetQuotas]
GO
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE object_id = OBJECT_ID(N'[dbo].[GetQuotaHidden]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetQuotaHidden]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProviderServiceQuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetProviderServiceQuota]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProviders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetProviders]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProviderByServiceID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetProviderByServiceID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProvider]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetProvider]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProcessBackgroundTasks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetProcessBackgroundTasks]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPrivateNetworVLANsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPrivateNetworVLANsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPrivateNetworVLAN]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPrivateNetworVLAN]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPendingSSLForWebsite]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPendingSSLForWebsite]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetParentPackageQuotas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetParentPackageQuotas]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageUnassignedIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageUnassignedIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackagesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackagesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageServiceID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageServiceID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackagesDiskspacePaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackagesDiskspacePaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackagesBandwidthPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackagesBandwidthPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackages]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageQuotasForEdit]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageQuotasForEdit]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageQuotas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageQuotas]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageQuota]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageQuota]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackagePrivateNetworkVLANs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackagePrivateNetworkVLANs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackagePrivateIPAddressesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackagePrivateIPAddressesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackagePrivateIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackagePrivateIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackagePackages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackagePackages]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageIPAddressesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageIPAddressesCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageDmzNetworkVLANs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageDmzNetworkVLANs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageDmzIPAddressesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageDmzIPAddressesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageDmzIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageDmzIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageDiskspace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageDiskspace]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageBandwidthUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageBandwidthUpdate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageBandwidth]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageBandwidth]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageAddons]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageAddons]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageAddon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackageAddon]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetPackage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationStoragSpacesFolderByType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationStoragSpacesFolderByType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationStoragSpaceFolders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationStoragSpaceFolders]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationStatistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationRdsUsersCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationRdsUsersCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationRdsServersCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationRdsServersCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationRdsCollectionsCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationRdsCollectionsCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationObjectsByDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationObjectsByDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationGroupsByDisplayName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationGroupsByDisplayName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationDeletedUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationDeletedUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationCRMUserCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationCRMUserCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOCSUsersCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOCSUsersCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOCSUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOCSUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNextSchedule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNextSchedule]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNestedPackagesSummary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNestedPackagesSummary]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNestedPackagesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNestedPackagesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMyPackages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetMyPackages]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLyncUsersCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLyncUsersCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLyncUsersByPlanId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLyncUsersByPlanId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLyncUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLyncUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLyncUserPlans]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLyncUserPlans]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLyncUserPlanByAccountId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLyncUserPlanByAccountId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLyncUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLyncUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLevelResourceGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLevelResourceGroups]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemPrivateIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetItemPrivateIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetItemIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemIdByOrganizationId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetItemIdByOrganizationId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemDmzIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetItemDmzIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetIPAddressesPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetIPAddressesPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInstanceID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetInstanceID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHostingPlans]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHostingPlans]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHostingPlanQuotas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHostingPlanQuotas]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHostingAddons]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHostingAddons]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGroupProviders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetGroupProviders]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFilterURLByHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetFilterURLByHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFilterURL]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetFilterURL]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeRetentionPolicyTags]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeRetentionPolicyTags]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeRetentionPolicyTag]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeRetentionPolicyTag]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeOrganizationStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeOrganizationStatistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeOrganizationSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeOrganizationSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeOrganizationDomains]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeOrganizationDomains]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeMailboxPlans]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeMailboxPlans]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeMailboxPlanRetentionPolicyTags]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeMailboxPlanRetentionPolicyTags]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeMailboxPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeMailboxPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeMailboxes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeMailboxes]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeDisclaimers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeDisclaimers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeDisclaimer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeDisclaimer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccountsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccountsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccounts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccounts]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccountEmailAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccountEmailAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccountDisclaimerId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccountDisclaimerId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccountByMailboxPlanId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccountByMailboxPlanId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccountByAccountNameWithoutItemId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccountByAccountNameWithoutItemId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccountByAccountName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccountByAccountName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeAccount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetEnterpriseFoldersPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetEnterpriseFoldersPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetEnterpriseFolders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetEnterpriseFolders]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetEnterpriseFolderOwaUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetEnterpriseFolderOwaUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetEnterpriseFolderId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetEnterpriseFolderId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetEnterpriseFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetEnterpriseFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomainsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomainsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomainsByZoneID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomainsByZoneID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomainsByDomainItemID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomainsByDomainItemID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomains]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomains]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomainDnsRecords]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomainDnsRecords]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomainByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomainByName]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomainAllDnsRecords]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomainAllDnsRecords]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDnsRecordsTotal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDnsRecordsTotal]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDnsRecordsByService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDnsRecordsByService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDnsRecordsByServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDnsRecordsByServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDnsRecordsByPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDnsRecordsByPackage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDnsRecordsByGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDnsRecordsByGroup]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDnsRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDnsRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCRMUsersCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCRMUsersCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCRMUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCRMUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCRMUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCRMUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCRMOrganizationUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCRMOrganizationUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetComments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetComments]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetClusters]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetClusters]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCertificatesForSite]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCertificatesForSite]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBlackBerryUsersCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBlackBerryUsersCount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBlackBerryUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBlackBerryUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBackgroundTopTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBackgroundTopTask]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBackgroundTasks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBackgroundTasks]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBackgroundTaskParams]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBackgroundTaskParams]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBackgroundTaskLogs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBackgroundTaskLogs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBackgroundTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBackgroundTask]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAvailableVirtualServices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAvailableVirtualServices]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAuditLogTasks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAuditLogTasks]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAuditLogSources]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAuditLogSources]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAuditLogRecordsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAuditLogRecordsPaged]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAuditLogRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAuditLogRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllServers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAllServers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllPackages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAllPackages]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAdditionalGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAdditionalGroups]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccessTokenByAccessToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAccessTokenByAccessToken]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeOrganizationExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExchangeOrganizationExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeOrganizationDomainExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExchangeOrganizationDomainExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeAccountExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExchangeAccountExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeAccountEmailAddressExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExchangeAccountEmailAddressExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DistributePackageServices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DistributePackageServices]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteVirtualServices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteVirtualServices]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUserThemeSetting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteUserThemeSetting]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUserEmailAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteUserEmailAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteSupportServiceLevel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteSupportServiceLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteSfBUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteSfBUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteSfBUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteSfBUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteServiceItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteServiceItem]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteSchedule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteSchedule]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRDSServerSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteRDSServerSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRDSServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteRDSServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRDSCollectionSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteRDSCollectionSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRDSCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteRDSCollection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeletePrivateNetworkVLAN]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeletePrivateNetworkVLAN]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeletePackageAddon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeletePackageAddon]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeletePackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeletePackage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteOrganizationUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteOrganizationUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteOrganizationStoragSpacesFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteOrganizationStoragSpacesFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteOrganizationDeletedUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteOrganizationDeletedUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteOCSUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteOCSUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteLyncUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteLyncUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteLyncUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteLyncUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteLevelResourceGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteLevelResourceGroups]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteItemPrivateIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteItemPrivateIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteItemPrivateIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteItemPrivateIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteItemIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteItemIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteItemIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteItemIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteItemDmzIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteItemDmzIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteItemDmzIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteItemDmzIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExpiredWebDavAccessTokens]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExpiredWebDavAccessTokens]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExpiredAccessTokenTokens]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExpiredAccessTokenTokens]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeRetentionPolicyTag]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeRetentionPolicyTag]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeOrganizationDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeOrganizationDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeMailboxPlanRetentionPolicyTag]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeMailboxPlanRetentionPolicyTag]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeMailboxPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeMailboxPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeDisclaimer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeDisclaimer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeAccountEmailAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeAccountEmailAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteExchangeAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteExchangeAccount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteEnterpriseFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteEnterpriseFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteDomainDnsRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteDomainDnsRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteDnsRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteDnsRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCRMOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteCRMOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteComment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteComment]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCluster]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteCluster]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCertificate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteCertificate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteBlackBerryUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteBlackBerryUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteBackgroundTasks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteBackgroundTasks]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteBackgroundTaskParams]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteBackgroundTaskParams]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteBackgroundTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteBackgroundTask]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAuditLogRecordsComplete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAuditLogRecordsComplete]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAuditLogRecords]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAuditLogRecords]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAllLogRecords]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAllLogRecords]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAllEnterpriseFolderOwaUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAllEnterpriseFolderOwaUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAdditionalGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAdditionalGroup]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAccessToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAccessToken]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeallocatePackageVLAN]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeallocatePackageVLAN]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeallocatePackageIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeallocatePackageIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateStorageSpaceFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateStorageSpaceFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConvertToExchangeOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ConvertToExchangeOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CompleteSSLRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CompleteSSLRequest]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckSSLExistsForWebsite]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckSSLExistsForWebsite]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckSSL]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckSSL]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckSfBUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckSfBUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckServiceLevelUsage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckServiceLevelUsage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckServiceItemExistsInService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckServiceItemExistsInService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckServiceItemExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckServiceItemExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckRDSServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckRDSServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckOCSUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckOCSUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckLyncUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckLyncUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckDomainUsedByHostedOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckDomainUsedByHostedOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckBlackBerryUserExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckBlackBerryUserExists]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChangeUserPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ChangeUserPassword]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChangePackageUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ChangePackageUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChangeExchangeAcceptedDomainType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ChangeExchangeAcceptedDomainType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanChangeMfa]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CanChangeMfa]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AllocatePackageVLANs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AllocatePackageVLANs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AllocatePackageIPAddresses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AllocatePackageIPAddresses]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddWebDavPortalUsersSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddWebDavPortalUsersSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddWebDavAccessToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddWebDavAccessToken]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddVirtualServices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddVirtualServices]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddUserToRDSCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddUserToRDSCollection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddSupportServiceLevel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddSupportServiceLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddSSLRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddSSLRequest]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddSfBUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddSfBUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddSfBUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddSfBUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddServiceItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddServiceItem]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddService]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddSchedule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddSchedule]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddRDSServerToOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddRDSServerToOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddRDSServerToCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddRDSServerToCollection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddRDSServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddRDSServer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddRDSMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddRDSMessage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddRDSCollectionSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddRDSCollectionSettings]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddRDSCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddRDSCollection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddRDSCertificate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddRDSCertificate]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddPrivateNetworkVlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddPrivateNetworkVlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddPFX]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddPFX]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddPackageAddon]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddPackageAddon]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddPackage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddOrganizationStoragSpacesFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddOrganizationStoragSpacesFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddOrganizationDeletedUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddOrganizationDeletedUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddOCSUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddOCSUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddLyncUserPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddLyncUserPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddLyncUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddLyncUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddLevelResourceGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddLevelResourceGroups]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddItemPrivateIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddItemPrivateIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddItemIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddItemIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddItemDmzIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddItemDmzIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddIPAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddHostingPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddHostingPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeRetentionPolicyTag]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeRetentionPolicyTag]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeOrganizationDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeOrganizationDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeOrganization]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeMailboxPlanRetentionPolicyTag]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeMailboxPlanRetentionPolicyTag]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeMailboxPlan]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeMailboxPlan]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeDisclaimer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeDisclaimer]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeAccountEmailAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeAccountEmailAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddExchangeAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddExchangeAccount]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddEnterpriseFolderOwaUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddEnterpriseFolderOwaUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddEnterpriseFolder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddEnterpriseFolder]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddDomainDnsRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddDomainDnsRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddDomain]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddDnsRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddDnsRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddComment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddComment]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddCluster]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddCluster]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddBlackBerryUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddBlackBerryUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddBackgroundTaskStack]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddBackgroundTaskStack]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddBackgroundTaskParam]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddBackgroundTaskParam]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddBackgroundTaskLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddBackgroundTaskLog]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddBackgroundTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddBackgroundTask]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAuditLogRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAuditLogRecord]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAdditionalGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAdditionalGroup]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAccessToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAccessToken]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[UsersDetailed]'))
DROP VIEW [dbo].[UsersDetailed]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersTree]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UsersTree]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserParents]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UserParents]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SplitString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[SplitString]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PackagesTree]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[PackagesTree]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PackageParents]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[PackageParents]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageServiceLevelResource]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetPackageServiceLevelResource]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageExceedingQuotas]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetPackageExceedingQuotas]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageAllocatedResource]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetPackageAllocatedResource]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageAllocatedQuota]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetPackageAllocatedQuota]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemComments]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetItemComments]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFullIPAddress]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetFullIPAddress]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckUserParent]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CheckUserParent]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckPackageParent]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CheckPackageParent]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckIsUserAdmin]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CheckIsUserAdmin]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckExceedingQuota]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CheckExceedingQuota]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckActorUserRights]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CheckActorUserRights]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckActorParentPackageRights]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CheckActorParentPackageRights]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckActorPackageRights]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CheckActorPackageRights]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanUpdateUserDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanUpdateUserDetails]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanUpdatePackageDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanUpdatePackageDetails]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanGetUserPassword]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanGetUserPassword]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanGetUserDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanGetUserDetails]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanCreateUser]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanCreateUser]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanChangeMfaFunc]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanChangeMfaFunc]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateQuotaUsage]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CalculateQuotaUsage]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculatePackageDiskspace]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CalculatePackageDiskspace]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculatePackageBandwidth]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CalculatePackageBandwidth]
GO
			");
		}
		#endregion

	}
}

using System;
using SolidCP.EnterpriseServer.Code;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.EnterpriseServer.Code.SharePoint;

namespace SolidCP.EnterpriseServer
{
	public class WebService: WebServiceBase, IDisposable
	{
		DataProvider db = null;
		public override DataProvider Db => db ?? (db = new DataProvider());

		WebApplicationsInstaller webApplicationsInstaller = null;
		public override WebApplicationsInstaller WebApplicationsInstaller => webApplicationsInstaller ?? (webApplicationsInstaller = new WebApplicationsInstaller(this));

		AuditLog auditLog = null;
		public override AuditLog AuditLog => auditLog ?? (auditLog = new AuditLog(this));

		BackupController backupController = null;
		public override BackupController BackupController => backupController ?? (backupController = new BackupController(this));

		BlackBerryController blackBerryController = null;
		public override BlackBerryController BlackBerryController => blackBerryController ?? (blackBerryController = new BlackBerryController(this));

		CommentsController commentsController = null;
		public override CommentsController CommentsController => commentsController ?? (commentsController = new CommentsController(this));

		CRMController crmController = null;
		public override CRMController CRMController => crmController ?? (crmController = new CRMController(this));

		DatabaseServerController databaseServerController = null;
		public override DatabaseServerController DatabaseServerController => databaseServerController ?? (databaseServerController = new DatabaseServerController(this));

		EnterpriseStorageController enterpriseStorageController = null;
		public override EnterpriseStorageController EnterpriseStorageController => enterpriseStorageController ?? (enterpriseStorageController = new EnterpriseStorageController(this));

		ExchangeServerController exchangeServerController = null;
		public override ExchangeServerController ExchangeServerController => exchangeServerController ?? (exchangeServerController = new ExchangeServerController(this));

		FilesController filesController = null;
		public override FilesController FilesController => filesController ?? (filesController = new FilesController(this));

		FtpServerController ftpServerController = null;
		public override FtpServerController FtpServerController => ftpServerController ?? (ftpServerController = new FtpServerController(this));

		HeliconZooController heliconZooController = null;
		public override HeliconZooController HeliconZooController => heliconZooController ?? (heliconZooController = new HeliconZooController(this));

		HostedSharePointServerController hostedSharePointServerController = null;
		public override HostedSharePointServerController HostedSharePointServerController => hostedSharePointServerController ?? (hostedSharePointServerController = new HostedSharePointServerController(this));

		ImportController importController = null;
		public override ImportController ImportController => importController ?? (importController = new ImportController(this));

		LyncController lyncController = null;
		public override LyncController LyncController => lyncController ?? (lyncController = new LyncController(this));

		MailServerController mailServerController = null;
		public override MailServerController MailServerController => mailServerController ?? (mailServerController = new MailServerController(this));

		OCSController ocsController = null;
		public override OCSController OCSController => ocsController ?? (ocsController = new OCSController(this));

		OperatingSystemController operatingSystemController = null;
		public override OperatingSystemController OperatingSystemController => operatingSystemController ?? (operatingSystemController = new OperatingSystemController(this));

		OrganizationController organizationController = null;
		public override OrganizationController OrganizationController => organizationController ?? (organizationController = new OrganizationController(this));

		PackageController packageController = null;
		public override PackageController PackageController => packageController ?? (packageController = new PackageController(this));

		RemoteDesktopServicesController remoteDesktopServicesController = null;
		public override RemoteDesktopServicesController RemoteDesktopServicesController => remoteDesktopServicesController ?? (remoteDesktopServicesController = new RemoteDesktopServicesController(this));

		SchedulerController schedulerController = null;
		public override SchedulerController SchedulerController => schedulerController ?? (schedulerController = new SchedulerController(this));

		ServerController serverController = null;
		public override ServerController ServerController => serverController ?? (serverController = new ServerController(this));

		SfBController sfBController = null;
		public override SfBController SfBController => sfBController ?? (sfBController = new SfBController(this));

		SharePointServerController sharePointServerController = null;
		public override SharePointServerController SharePointServerController => sharePointServerController ?? (sharePointServerController = new SharePointServerController(this));

		SpamExpertsController spamExpertsController = null;
		public override SpamExpertsController SpamExpertsController => spamExpertsController ?? (spamExpertsController = new SpamExpertsController(this));

		StatisticsServerController statisticsServerController = null;
		public override StatisticsServerController StatisticsServerController => statisticsServerController ?? (statisticsServerController = new StatisticsServerController(this));

		StorageSpacesController storageSpacesController = null;
		public override StorageSpacesController StorageSpacesController => storageSpacesController ?? (storageSpacesController = new StorageSpacesController(this));

		SystemController systemController = null;
		public override SystemController SystemController => systemController ?? (systemController = new SystemController(this));

		TaskManager taskManager = null;
		public override TaskManager TaskManager => taskManager ?? (taskManager = new TaskManager(this));

		UserController userController = null;
		public override UserController UserController => userController ?? (userController = new UserController(this));

		VirtualizationServerController virtualizationServerController = null;
		public override VirtualizationServerController VirtualizationServerController => virtualizationServerController ?? (virtualizationServerController = new VirtualizationServerController(this));

		VirtualizationServerController2012 virtualizationServerController2012 = null;
		public override VirtualizationServerController2012 VirtualizationServerController2012 => virtualizationServerController2012 ?? (virtualizationServerController2012 = new VirtualizationServerController2012(this));

		VirtualizationServerControllerForPrivateCloud virtualizationServerControllerForPrivateCloud = null;
		public override VirtualizationServerControllerForPrivateCloud VirtualizationServerControllerForPrivateCloud => virtualizationServerControllerForPrivateCloud ?? (virtualizationServerControllerForPrivateCloud = new VirtualizationServerControllerForPrivateCloud(this));

		VirtualizationServerControllerProxmox virtualizationServerControllerProxmox = null;
		public override VirtualizationServerControllerProxmox VirtualizationServerControllerProxmox => virtualizationServerControllerProxmox ?? (virtualizationServerControllerProxmox = new VirtualizationServerControllerProxmox(this));

		WebAppGalleryController webAppGalleryController = null;
		public override WebAppGalleryController WebAppGalleryController => webAppGalleryController ?? (webAppGalleryController = new WebAppGalleryController(this));

		WebServerController webServerController = null;
		public override WebServerController WebServerController => webServerController ?? (webServerController = new WebServerController(this));


		bool isDisposed = false;
		public void Dispose()
		{
			if (!isDisposed)
			{
				isDisposed = true;
				db?.Dispose();
			}
		}
	}
}

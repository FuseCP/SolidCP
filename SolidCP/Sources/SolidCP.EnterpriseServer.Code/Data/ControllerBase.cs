using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.EnterpriseServer.Code.SharePoint;
using SolidCP.EnterpriseServer.Code.VirtualizationProxmox;

namespace SolidCP.EnterpriseServer
{
	public class ControllerBase: IDisposable
	{
		DataProvider db = null;
		public DataProvider Db {
			get => db ?? Provider?.Db ?? (db = new DataProvider(this));
			private set => db = value;
		}
		public DataProvider DataProvider => Db;

		ControllerBase Provider = null;
		public ControllerBase() { }
		public ControllerBase(ControllerBase provider) { Provider = provider; }

		public ControllerBase Clone()
		{
			var clone = (ControllerBase)Activator.CreateInstance(GetType(), Provider);
			clone.Provider = Provider;
			clone.db = db;
			return clone;
		}
		protected T AsAsync<T>() where T : ControllerBase
		{
			var clone = Clone();
			clone.db = new DataProvider();
			return (T)clone;
		}

		WebApplicationsInstaller webApplicationsInstaller = null;
		protected WebApplicationsInstaller WebApplicationsInstaller => webApplicationsInstaller ?? (webApplicationsInstaller = new WebApplicationsInstaller(this));

		AuditLog auditLog = null;
		protected AuditLog AuditLog => auditLog ?? Provider.AuditLog ?? (auditLog = new AuditLog(this));

		BackupController backupController = null;
		protected BackupController BackupController => backupController ?? (backupController = new BackupController(this));

		BlackBerryController blackBerryController = null;
		protected BlackBerryController BlackBerryController => blackBerryController ?? (blackBerryController = new BlackBerryController(this));

		CommentsController commentsController = null;
		protected CommentsController CommentsController => commentsController ?? (commentsController = new CommentsController(this));

		CRMController crmController = null;
		protected CRMController CRMController => crmController ?? (crmController = new CRMController(this));

		DnsServerController dnsServerController = null;
		protected DnsServerController DnsServerController => dnsServerController ?? (dnsServerController = new DnsServerController(this));

		DatabaseServerController databaseServerController = null;
		protected DatabaseServerController DatabaseServerController => databaseServerController ?? (databaseServerController = new DatabaseServerController(this));

		EnterpriseStorageController enterpriseStorageController = null;
		protected EnterpriseStorageController EnterpriseStorageController => enterpriseStorageController ?? (enterpriseStorageController = new EnterpriseStorageController(this));

		ExchangeServerController exchangeServerController = null;
		protected ExchangeServerController ExchangeServerController => exchangeServerController ?? (exchangeServerController = new ExchangeServerController(this));

		FilesController filesController = null;
		protected FilesController FilesController => filesController ?? (filesController = new FilesController(this));

		FtpServerController ftpServerController = null;
		protected FtpServerController FtpServerController => ftpServerController ?? (ftpServerController = new FtpServerController(this));

		HeliconZooController heliconZooController = null;
		protected HeliconZooController HeliconZooController => heliconZooController ?? (heliconZooController = new HeliconZooController(this));

		HostedSharePointServerController hostedSharePointServerController = null;
		protected HostedSharePointServerController HostedSharePointServerController => hostedSharePointServerController ?? (hostedSharePointServerController = new HostedSharePointServerController(this));

		HostedSharePointServerEntController hostedSharePointServerEntController = null;
		protected HostedSharePointServerEntController HostedSharePointServerEntController => hostedSharePointServerEntController ?? (hostedSharePointServerEntController = new HostedSharePointServerEntController(this));

		ImportController importController = null;
		protected ImportController ImportController => importController ?? (importController = new ImportController(this));

		LyncController lyncController = null;
		protected LyncController LyncController => lyncController ?? (lyncController = new LyncController(this));

		MailServerController mailServerController = null;
		protected MailServerController MailServerController => mailServerController ?? (mailServerController = new MailServerController(this));

		OCSController ocsController = null;
		protected OCSController OCSController => ocsController ?? (ocsController = new OCSController(this));

		OperatingSystemController operatingSystemController = null;
		protected OperatingSystemController OperatingSystemController => operatingSystemController ?? (operatingSystemController = new OperatingSystemController(this));

		OrganizationController organizationController = null;
		protected OrganizationController OrganizationController => organizationController ?? (organizationController = new OrganizationController(this));

		PackageController packageController = null;
		protected PackageController PackageController => packageController ?? (packageController = new PackageController(this));

		RemoteDesktopServicesController remoteDesktopServicesController = null;
		protected RemoteDesktopServicesController RemoteDesktopServicesController => remoteDesktopServicesController ?? (remoteDesktopServicesController = new RemoteDesktopServicesController(this));

		ReportController reportController = null;
		protected ReportController ReportController => reportController ?? (reportController = new ReportController(this));

		SchedulerController schedulerController = null;
		protected SchedulerController SchedulerController => schedulerController ?? (schedulerController = new SchedulerController(this));

		ServerController serverController = null;
		protected ServerController ServerController => serverController ?? (serverController = new ServerController(this));

		SfBController sfBController = null;
		protected SfBController SfBController => sfBController ?? (sfBController = new SfBController(this));

		SharePointServerController sharePointServerController = null;
		protected SharePointServerController SharePointServerController => sharePointServerController ?? (sharePointServerController = new SharePointServerController(this));

		SpamExpertsController spamExpertsController = null;
		protected SpamExpertsController SpamExpertsController => spamExpertsController ?? (spamExpertsController = new SpamExpertsController(this));

		StatisticsServerController statisticsServerController = null;
		protected StatisticsServerController StatisticsServerController => statisticsServerController ?? (statisticsServerController = new StatisticsServerController(this));

		StorageSpacesController storageSpacesController = null;
		protected StorageSpacesController StorageSpacesController => storageSpacesController ?? (storageSpacesController = new StorageSpacesController(this));

		SystemController systemController = null;
		protected SystemController SystemController => systemController ?? (systemController = new SystemController(this));

		TaskManager taskManager = null;
		protected TaskManager TaskManager => taskManager ?? (taskManager = new TaskManager(this));

		TaskController taskController = null;
		protected TaskController TaskController => taskController ?? (taskController = new TaskController(this));

		UserController userController = null;
		protected UserController UserController => userController ?? (userController = new UserController(this));

		VirtualizationServerController virtualizationServerController = null;
		protected VirtualizationServerController VirtualizationServerController => virtualizationServerController ?? (virtualizationServerController = new VirtualizationServerController(this));

		VirtualizationServerController2012 virtualizationServerController2012 = null;
		protected VirtualizationServerController2012 VirtualizationServerController2012 => virtualizationServerController2012 ?? (virtualizationServerController2012 = new VirtualizationServerController2012(this));

		VirtualizationServerControllerForPrivateCloud virtualizationServerControllerForPrivateCloud = null;
		protected VirtualizationServerControllerForPrivateCloud VirtualizationServerControllerForPrivateCloud => virtualizationServerControllerForPrivateCloud ?? (virtualizationServerControllerForPrivateCloud = new VirtualizationServerControllerForPrivateCloud(this));

		VirtualizationServerControllerProxmox virtualizationServerControllerProxmox = null;
		protected VirtualizationServerControllerProxmox VirtualizationServerControllerProxmox => virtualizationServerControllerProxmox ?? (virtualizationServerControllerProxmox = new VirtualizationServerControllerProxmox(this));

		WebAppGalleryController webAppGalleryController = null;
		protected WebAppGalleryController WebAppGalleryController => webAppGalleryController ?? (webAppGalleryController = new WebAppGalleryController(this));

		WebServerController webServerController = null;
		protected WebServerController WebServerController => webServerController ?? (webServerController = new WebServerController(this));

		APIMailCleanerHelper apiMailCleanerHelper = null;
		protected APIMailCleanerHelper APIMailCleanerHelper => apiMailCleanerHelper ?? (apiMailCleanerHelper = new APIMailCleanerHelper(this));

		UserCreationWizard userCreationWizard = null;
		protected UserCreationWizard UserCreationWizard => userCreationWizard ?? (userCreationWizard = new UserCreationWizard(this));

		Code.Virtualization2012.UseCase.ChangeVirtualMachineAdministratorPasswordHandler changeVirtualMachineAdministratorPasswordHandler = null;
		protected Code.Virtualization2012.UseCase.ChangeVirtualMachineAdministratorPasswordHandler ChangeVirtualMachineAdministratorPasswordHandler => changeVirtualMachineAdministratorPasswordHandler ?? (changeVirtualMachineAdministratorPasswordHandler = new Code.Virtualization2012.UseCase.ChangeVirtualMachineAdministratorPasswordHandler(this));

		Code.Virtualization2012.UseCase.ChangeVirtualMachineStateHandler changeVirtualMachineStateHandler = null;
		protected Code.Virtualization2012.UseCase.ChangeVirtualMachineStateHandler ChangeVirtualMachineStateHandler => changeVirtualMachineStateHandler ?? (changeVirtualMachineStateHandler = new Code.Virtualization2012.UseCase.ChangeVirtualMachineStateHandler(this));

		Code.Virtualization2012.UseCase.CreateVirtualMachineHandler createVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.CreateVirtualMachineHandler CreateVirtualMachineHandler => createVirtualMachineHandler ?? (createVirtualMachineHandler = new Code.Virtualization2012.UseCase.CreateVirtualMachineHandler(this));

		Code.Virtualization2012.UseCase.DeleteVirtualMachineHandler deleteVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.DeleteVirtualMachineHandler DeleteVirtualMachineHandler => deleteVirtualMachineHandler ?? (deleteVirtualMachineHandler = new Code.Virtualization2012.UseCase.DeleteVirtualMachineHandler(this));

		Code.Virtualization2012.UseCase.ImportVirtualMachineHandler importVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.ImportVirtualMachineHandler ImportVirtualMachineHandler => importVirtualMachineHandler ?? (importVirtualMachineHandler = new Code.Virtualization2012.UseCase.ImportVirtualMachineHandler(this));

		Code.Virtualization2012.UseCase.ReinstallVirtualMachineHandler reinstallVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.ReinstallVirtualMachineHandler ReinstallVirtualMachineHandler => reinstallVirtualMachineHandler ?? (reinstallVirtualMachineHandler = new Code.Virtualization2012.UseCase.ReinstallVirtualMachineHandler(this));

		Code.Virtualization2012.UseCase.UpdateVirtualMachineHostNameHandler updateVirtualMachineHostNameHandler = null;
		protected Code.Virtualization2012.UseCase.UpdateVirtualMachineHostNameHandler UpdateVirtualMachineHostNameHandler => updateVirtualMachineHostNameHandler ?? (updateVirtualMachineHostNameHandler = new Code.Virtualization2012.UseCase.UpdateVirtualMachineHostNameHandler(this));

		Code.Virtualization2012.UseCase.UpdateVirtualMachineResourceHandler updateVirtualMachineResourceHandler = null;
		protected Code.Virtualization2012.UseCase.UpdateVirtualMachineResourceHandler UpdateVirtualMachineResourceHandler => updateVirtualMachineResourceHandler ?? (updateVirtualMachineResourceHandler = new Code.Virtualization2012.UseCase.UpdateVirtualMachineResourceHandler(this));

		Code.Virtualization2012.Tasks.CreateVirtualMachineTask createVirtualMachineTask = null;
		protected Code.Virtualization2012.Tasks.CreateVirtualMachineTask CreateVirtualMachineTask => createVirtualMachineTask ?? (createVirtualMachineTask = new Code.Virtualization2012.Tasks.CreateVirtualMachineTask(this));

		Code.Virtualization2012.Tasks.DeleteVirtualMachineTask deleteVirtualMachineTask = null;
		protected Code.Virtualization2012.Tasks.DeleteVirtualMachineTask DeleteVirtualMachineTask => deleteVirtualMachineTask ?? (deleteVirtualMachineTask = new Code.Virtualization2012.Tasks.DeleteVirtualMachineTask(this));

		Code.Virtualization2012.Tasks.ReinstallVirtualMachineTask reinstallVirtualMachineTask = null;
		protected Code.Virtualization2012.Tasks.ReinstallVirtualMachineTask ReinstallVirtualMachineTask => reinstallVirtualMachineTask ?? (reinstallVirtualMachineTask = new Code.Virtualization2012.Tasks.ReinstallVirtualMachineTask(this));

		SecurityContext securityContext = null;
		protected SecurityContext SecurityContext => securityContext ?? (securityContext = new SecurityContext(this));

		UserHelper userHelper = null;
		protected UserHelper UserHelper => userHelper ?? (userHelper = new UserHelper(this));

		OneTimePasswordHelper oneTimePasswordHelper = null;
		protected OneTimePasswordHelper OneTimePasswordHelper => oneTimePasswordHelper ?? (oneTimePasswordHelper = new OneTimePasswordHelper(this));

		RemoteDesktopServicesHelpers remoteDesktioServiceHelpers = null;
		protected RemoteDesktopServicesHelpers RemoteDesktopServicesHelpers => remoteDesktioServiceHelpers ?? (remoteDesktioServiceHelpers = new RemoteDesktopServicesHelpers(this));

		ServiceProviderProxy serviceProviderProxy = null;
		protected ServiceProviderProxy ServiceProviderProxy => serviceProviderProxy ?? (serviceProviderProxy = new ServiceProviderProxy(this));

		VirtualizationHelperProxmox virtualizationHelperProxmox = null;
		protected VirtualizationHelperProxmox VirtualizationHelperProxmox => virtualizationHelperProxmox ?? (virtualizationHelperProxmox = new VirtualizationHelperProxmox(this));
		Code.Virtualization2012.Helpers.PS.PowerShellScript powerShellScript = null;
		protected Code.Virtualization2012.Helpers.PS.PowerShellScript PowerShellScript => powerShellScript ?? (powerShellScript = new Code.Virtualization2012.Helpers.PS.PowerShellScript(this));

		Code.Virtualization2012.Helpers.VM.IpAddressExternalHelper ipAddressExternalHelper = null;
		protected Code.Virtualization2012.Helpers.VM.IpAddressExternalHelper IpAddressExternalHelper => ipAddressExternalHelper ?? (ipAddressExternalHelper = new Code.Virtualization2012.Helpers.VM.IpAddressExternalHelper(this));

		Code.Virtualization2012.Helpers.VM.IpAddressPrivateHelper ipAddressPrivateHelper = null;
		protected Code.Virtualization2012.Helpers.VM.IpAddressPrivateHelper IpAddressPrivateHelper => ipAddressPrivateHelper ?? (ipAddressPrivateHelper = new Code.Virtualization2012.Helpers.VM.IpAddressPrivateHelper(this));

		Code.Virtualization2012.Helpers.VM.KvpExchangeHelper kvpExchangeHelper = null;
		protected Code.Virtualization2012.Helpers.VM.KvpExchangeHelper KvpExchangeHelper => kvpExchangeHelper ?? (kvpExchangeHelper = new Code.Virtualization2012.Helpers.VM.KvpExchangeHelper(this));

		Code.Virtualization2012.Helpers.VM.NetworkAdapterDetailsHelper networkAdapterDetailsHelper = null;
		protected Code.Virtualization2012.Helpers.VM.NetworkAdapterDetailsHelper NetworkAdapterDetailsHelper => networkAdapterDetailsHelper ?? (networkAdapterDetailsHelper = new Code.Virtualization2012.Helpers.VM.NetworkAdapterDetailsHelper(this));

		Code.Virtualization2012.Helpers.VM.NetworkHelper networkHelper = null;
		protected Code.Virtualization2012.Helpers.VM.NetworkHelper NetworkHelper => networkHelper ?? (networkHelper = new Code.Virtualization2012.Helpers.VM.NetworkHelper(this));

		Code.Virtualization2012.Helpers.VM.NetworkVLANHelper networkVLANHelper = null;
		protected Code.Virtualization2012.Helpers.VM.NetworkVLANHelper NetworkVLANHelper => networkVLANHelper ?? (networkVLANHelper = new Code.Virtualization2012.Helpers.VM.NetworkVLANHelper(this));

		Code.Virtualization2012.Helpers.GuacaHelper guacaHelper = null;
		protected Code.Virtualization2012.Helpers.GuacaHelper GuacaHelper => guacaHelper ?? (guacaHelper = new Code.Virtualization2012.Helpers.GuacaHelper(this));

		Code.Virtualization2012.Helpers.JobHelper jobHelper = null;
		protected Code.Virtualization2012.Helpers.JobHelper JobHelper => jobHelper ?? (jobHelper = new Code.Virtualization2012.Helpers.JobHelper(this));

		Code.Virtualization2012.ReplicationHelper replicationHelper = null;
		protected Code.Virtualization2012.ReplicationHelper ReplicationHelper => replicationHelper ?? (replicationHelper = new Code.Virtualization2012.ReplicationHelper(this));

		Code.Virtualization2012.VirtualizationHelper virtualizationHelper = null;
		protected Code.Virtualization2012.VirtualizationHelper VirtualizationHelper => virtualizationHelper ?? (virtualizationHelper = new Code.Virtualization2012.VirtualizationHelper(this));

		Code.Virtualization2012.Helpers.VirtualizationUtils virtualizationUtils = null;
		protected Code.Virtualization2012.Helpers.VirtualizationUtils VirtualizationUtils => virtualizationUtils ?? (virtualizationUtils = new Code.Virtualization2012.Helpers.VirtualizationUtils(this));

		Code.Virtualization2012.Helpers.VirtualMachineHelper virtualMachineHelper = null;
		protected Code.Virtualization2012.Helpers.VirtualMachineHelper VirtualMachineHelper => virtualMachineHelper ?? (virtualMachineHelper = new Code.Virtualization2012.Helpers.VirtualMachineHelper(this));

		Scheduler scheduler = null;
		protected Scheduler Scheduler => scheduler ?? (scheduler = new Scheduler(this));

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

	public class ControllerAsyncBase : ControllerBase { }
}

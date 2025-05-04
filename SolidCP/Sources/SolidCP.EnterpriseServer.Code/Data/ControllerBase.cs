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
		public DataProvider Database {
			get => db ?? Provider?.Database ?? (db = new DataProvider(this));
			private set => db = value;
		}

		ControllerBase Provider = null;
		public ControllerBase() { }
		public ControllerBase(ControllerBase provider) { Provider = provider; }
		public ControllerBase(DataProvider database) {
			Provider = null;
			Database = database;
		}

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
			clone.Provider = null;
			clone.db = Database != null ? Database.Context : new DataProvider();
			return (T)clone;
		}

		WebApplicationsInstaller webApplicationsInstaller = null;
		protected WebApplicationsInstaller WebApplicationsInstaller => webApplicationsInstaller ??= new WebApplicationsInstaller(this);

		AuditLog auditLog = null;
		public AuditLog AuditLog => auditLog ??= new AuditLog(this);

		BackupController backupController = null;
		protected BackupController BackupController => backupController ??= new BackupController(this);

		BlackBerryController blackBerryController = null;
		protected BlackBerryController BlackBerryController => blackBerryController ??= new BlackBerryController(this);

		CommentsController commentsController = null;
		protected CommentsController CommentsController => commentsController ??= new CommentsController(this);

		CRMController crmController = null;
		protected CRMController CRMController => crmController ??= new CRMController(this);

		DnsServerController dnsServerController = null;
		protected DnsServerController DnsServerController => dnsServerController ??= new DnsServerController(this);

		DatabaseServerController databaseServerController = null;
		protected DatabaseServerController DatabaseServerController => databaseServerController ??= new DatabaseServerController(this);

		EnterpriseStorageController enterpriseStorageController = null;
		protected EnterpriseStorageController EnterpriseStorageController => enterpriseStorageController ??= new EnterpriseStorageController(this);

		ExchangeServerController exchangeServerController = null;
		protected ExchangeServerController ExchangeServerController => exchangeServerController ??= new ExchangeServerController(this);

		FilesController filesController = null;
		protected FilesController FilesController => filesController ??= new FilesController(this);

		FtpServerController ftpServerController = null;
		protected FtpServerController FtpServerController => ftpServerController ??= new FtpServerController(this);

		HeliconZooController heliconZooController = null;
		protected HeliconZooController HeliconZooController => heliconZooController ??= new HeliconZooController(this);

		HostedSharePointServerController hostedSharePointServerController = null;
		protected HostedSharePointServerController HostedSharePointServerController => hostedSharePointServerController ??= new HostedSharePointServerController(this);

		HostedSharePointServerEntController hostedSharePointServerEntController = null;
		protected HostedSharePointServerEntController HostedSharePointServerEntController => hostedSharePointServerEntController ??= new HostedSharePointServerEntController(this);

		ImportController importController = null;
		protected ImportController ImportController => importController ??= new ImportController(this);

		LyncController lyncController = null;
		protected LyncController LyncController => lyncController ??= new LyncController(this);

		MailServerController mailServerController = null;
		protected MailServerController MailServerController => mailServerController ??= new MailServerController(this);

		OCSController ocsController = null;
		protected OCSController OCSController => ocsController ??= new OCSController(this);

		OperatingSystemController operatingSystemController = null;
		protected OperatingSystemController OperatingSystemController => operatingSystemController ??= new OperatingSystemController(this);

		OrganizationController organizationController = null;
		protected OrganizationController OrganizationController => organizationController ??= new OrganizationController(this);

		PackageController packageController = null;
		protected PackageController PackageController => packageController ??= new PackageController(this);

		RemoteDesktopServicesController remoteDesktopServicesController = null;
		protected RemoteDesktopServicesController RemoteDesktopServicesController => remoteDesktopServicesController ??= new RemoteDesktopServicesController(this);

		ReportController reportController = null;
		protected ReportController ReportController => reportController ??= new ReportController(this);

		SchedulerController schedulerController = null;
		protected SchedulerController SchedulerController => schedulerController ??= new SchedulerController(this);

		ServerController serverController = null;
		protected ServerController ServerController => serverController ??= new ServerController(this);

		SfBController sfBController = null;
		protected SfBController SfBController => sfBController ??= new SfBController(this);

		SharePointServerController sharePointServerController = null;
		protected SharePointServerController SharePointServerController => sharePointServerController ??= new SharePointServerController(this);

		SpamExpertsController spamExpertsController = null;
		protected SpamExpertsController SpamExpertsController => spamExpertsController ??= new SpamExpertsController(this);

		StatisticsServerController statisticsServerController = null;
		protected StatisticsServerController StatisticsServerController => statisticsServerController ??= new StatisticsServerController(this);

		StorageSpacesController storageSpacesController = null;
		protected StorageSpacesController StorageSpacesController => storageSpacesController ??= new StorageSpacesController(this);

		SystemController systemController = null;
		protected SystemController SystemController => systemController ??= new SystemController(this);

		TaskManager taskManager = null;
		protected TaskManager TaskManager => taskManager ??= new TaskManager(this);

		TaskController taskController = null;
		protected TaskController TaskController => taskController ??= new TaskController(this);

		UserController userController = null;
		protected UserController UserController => userController ??= new UserController(this);

		VirtualizationServerController virtualizationServerController = null;
		protected VirtualizationServerController VirtualizationServerController => virtualizationServerController ??= new VirtualizationServerController(this);

		VirtualizationServerController2012 virtualizationServerController2012 = null;
		protected VirtualizationServerController2012 VirtualizationServerController2012 => virtualizationServerController2012 ?? (virtualizationServerController2012 = new VirtualizationServerController2012(this));

		VirtualizationServerControllerForPrivateCloud virtualizationServerControllerForPrivateCloud = null;
		protected VirtualizationServerControllerForPrivateCloud VirtualizationServerControllerForPrivateCloud => virtualizationServerControllerForPrivateCloud ??= new VirtualizationServerControllerForPrivateCloud(this);

		VirtualizationServerControllerProxmox virtualizationServerControllerProxmox = null;
		protected VirtualizationServerControllerProxmox VirtualizationServerControllerProxmox => virtualizationServerControllerProxmox ??= new VirtualizationServerControllerProxmox(this);

		WebAppGalleryController webAppGalleryController = null;
		protected WebAppGalleryController WebAppGalleryController => webAppGalleryController ??= new WebAppGalleryController(this);

		WebServerController webServerController = null;
		protected WebServerController WebServerController => webServerController ??= new WebServerController(this);

		APIMailCleanerHelper apiMailCleanerHelper = null;
		protected APIMailCleanerHelper APIMailCleanerHelper => apiMailCleanerHelper ??= new APIMailCleanerHelper(this);

		UserCreationWizard userCreationWizard = null;
		protected UserCreationWizard UserCreationWizard => userCreationWizard ??= new UserCreationWizard(this);

		Code.Virtualization2012.UseCase.ChangeVirtualMachineAdministratorPasswordHandler changeVirtualMachineAdministratorPasswordHandler = null;
		protected Code.Virtualization2012.UseCase.ChangeVirtualMachineAdministratorPasswordHandler ChangeVirtualMachineAdministratorPasswordHandler => changeVirtualMachineAdministratorPasswordHandler ??= new Code.Virtualization2012.UseCase.ChangeVirtualMachineAdministratorPasswordHandler(this);

		Code.Virtualization2012.UseCase.ChangeVirtualMachineStateHandler changeVirtualMachineStateHandler = null;
		protected Code.Virtualization2012.UseCase.ChangeVirtualMachineStateHandler ChangeVirtualMachineStateHandler => changeVirtualMachineStateHandler ??= new Code.Virtualization2012.UseCase.ChangeVirtualMachineStateHandler(this);

		Code.Virtualization2012.UseCase.CreateVirtualMachineHandler createVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.CreateVirtualMachineHandler CreateVirtualMachineHandler => createVirtualMachineHandler ??= new Code.Virtualization2012.UseCase.CreateVirtualMachineHandler(this);

		Code.Virtualization2012.UseCase.DeleteVirtualMachineHandler deleteVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.DeleteVirtualMachineHandler DeleteVirtualMachineHandler => deleteVirtualMachineHandler ??= new Code.Virtualization2012.UseCase.DeleteVirtualMachineHandler(this);

		Code.Virtualization2012.UseCase.ImportVirtualMachineHandler importVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.ImportVirtualMachineHandler ImportVirtualMachineHandler => importVirtualMachineHandler ??= new Code.Virtualization2012.UseCase.ImportVirtualMachineHandler(this);

		Code.Virtualization2012.UseCase.ReinstallVirtualMachineHandler reinstallVirtualMachineHandler = null;
		protected Code.Virtualization2012.UseCase.ReinstallVirtualMachineHandler ReinstallVirtualMachineHandler => reinstallVirtualMachineHandler ??= new Code.Virtualization2012.UseCase.ReinstallVirtualMachineHandler(this);

		Code.Virtualization2012.UseCase.UpdateVirtualMachineHostNameHandler updateVirtualMachineHostNameHandler = null;
		protected Code.Virtualization2012.UseCase.UpdateVirtualMachineHostNameHandler UpdateVirtualMachineHostNameHandler => updateVirtualMachineHostNameHandler ??= new Code.Virtualization2012.UseCase.UpdateVirtualMachineHostNameHandler(this);

		Code.Virtualization2012.UseCase.UpdateVirtualMachineResourceHandler updateVirtualMachineResourceHandler = null;
		protected Code.Virtualization2012.UseCase.UpdateVirtualMachineResourceHandler UpdateVirtualMachineResourceHandler => updateVirtualMachineResourceHandler ??= new Code.Virtualization2012.UseCase.UpdateVirtualMachineResourceHandler(this);

		Code.Virtualization2012.Tasks.CreateVirtualMachineTask createVirtualMachineTask = null;
		protected Code.Virtualization2012.Tasks.CreateVirtualMachineTask CreateVirtualMachineTask => createVirtualMachineTask ??= new Code.Virtualization2012.Tasks.CreateVirtualMachineTask(this);

		Code.Virtualization2012.Tasks.DeleteVirtualMachineTask deleteVirtualMachineTask = null;
		protected Code.Virtualization2012.Tasks.DeleteVirtualMachineTask DeleteVirtualMachineTask => deleteVirtualMachineTask ??= new Code.Virtualization2012.Tasks.DeleteVirtualMachineTask(this);

		Code.Virtualization2012.Tasks.ReinstallVirtualMachineTask reinstallVirtualMachineTask = null;
		protected Code.Virtualization2012.Tasks.ReinstallVirtualMachineTask ReinstallVirtualMachineTask => reinstallVirtualMachineTask ??= new Code.Virtualization2012.Tasks.ReinstallVirtualMachineTask(this);

		SecurityContext securityContext = null;
		protected SecurityContext SecurityContext => securityContext ??= new SecurityContext(this);

		UserHelper userHelper = null;
		protected UserHelper UserHelper => userHelper ??= new UserHelper(this);

		OneTimePasswordHelper oneTimePasswordHelper = null;
		protected OneTimePasswordHelper OneTimePasswordHelper => oneTimePasswordHelper ??= new OneTimePasswordHelper(this);

		RemoteDesktopServicesHelpers remoteDesktioServiceHelpers = null;
		protected RemoteDesktopServicesHelpers RemoteDesktopServicesHelpers => remoteDesktioServiceHelpers ??= new RemoteDesktopServicesHelpers(this);

		ServiceProviderProxy serviceProviderProxy = null;
		protected ServiceProviderProxy ServiceProviderProxy => serviceProviderProxy ??= new ServiceProviderProxy(this);

		VirtualizationHelperProxmox virtualizationHelperProxmox = null;
		protected VirtualizationHelperProxmox VirtualizationHelperProxmox => virtualizationHelperProxmox ??= new VirtualizationHelperProxmox(this);
		Code.Virtualization2012.Helpers.PS.PowerShellScript powerShellScript = null;
		protected Code.Virtualization2012.Helpers.PS.PowerShellScript PowerShellScript => powerShellScript ??= new Code.Virtualization2012.Helpers.PS.PowerShellScript(this);

		Code.Virtualization2012.Helpers.VM.IpAddressExternalHelper ipAddressExternalHelper = null;
		protected Code.Virtualization2012.Helpers.VM.IpAddressExternalHelper IpAddressExternalHelper => ipAddressExternalHelper ??= new Code.Virtualization2012.Helpers.VM.IpAddressExternalHelper(this);

		Code.Virtualization2012.Helpers.VM.IpAddressPrivateHelper ipAddressPrivateHelper = null;
		protected Code.Virtualization2012.Helpers.VM.IpAddressPrivateHelper IpAddressPrivateHelper => ipAddressPrivateHelper ??= new Code.Virtualization2012.Helpers.VM.IpAddressPrivateHelper(this);

		Code.Virtualization2012.Helpers.VM.KvpExchangeHelper kvpExchangeHelper = null;
		protected Code.Virtualization2012.Helpers.VM.KvpExchangeHelper KvpExchangeHelper => kvpExchangeHelper ??= new Code.Virtualization2012.Helpers.VM.KvpExchangeHelper(this);

		Code.Virtualization2012.Helpers.VM.NetworkAdapterDetailsHelper networkAdapterDetailsHelper = null;
		protected Code.Virtualization2012.Helpers.VM.NetworkAdapterDetailsHelper NetworkAdapterDetailsHelper => networkAdapterDetailsHelper ??= new Code.Virtualization2012.Helpers.VM.NetworkAdapterDetailsHelper(this);

		Code.Virtualization2012.Helpers.VM.NetworkHelper networkHelper = null;
		protected Code.Virtualization2012.Helpers.VM.NetworkHelper NetworkHelper => networkHelper ??= new Code.Virtualization2012.Helpers.VM.NetworkHelper(this);

		Code.Virtualization2012.Helpers.VM.NetworkVLANHelper networkVLANHelper = null;
		protected Code.Virtualization2012.Helpers.VM.NetworkVLANHelper NetworkVLANHelper => networkVLANHelper ??= new Code.Virtualization2012.Helpers.VM.NetworkVLANHelper(this);

		Code.Virtualization2012.Helpers.GuacaHelper guacaHelper = null;
		protected Code.Virtualization2012.Helpers.GuacaHelper GuacaHelper => guacaHelper ??= new Code.Virtualization2012.Helpers.GuacaHelper(this);

		Code.Virtualization2012.Helpers.JobHelper jobHelper = null;
		protected Code.Virtualization2012.Helpers.JobHelper JobHelper => jobHelper ??= new Code.Virtualization2012.Helpers.JobHelper(this);

		Code.Virtualization2012.ReplicationHelper replicationHelper = null;
		protected Code.Virtualization2012.ReplicationHelper ReplicationHelper => replicationHelper ??= new Code.Virtualization2012.ReplicationHelper(this);

		Code.Virtualization2012.VirtualizationHelper virtualizationHelper = null;
		protected Code.Virtualization2012.VirtualizationHelper VirtualizationHelper => virtualizationHelper ??= new Code.Virtualization2012.VirtualizationHelper(this);

		Code.Virtualization2012.Helpers.VirtualizationUtils virtualizationUtils = null;
		protected Code.Virtualization2012.Helpers.VirtualizationUtils VirtualizationUtils => virtualizationUtils ??= new Code.Virtualization2012.Helpers.VirtualizationUtils(this);

		Code.Virtualization2012.Helpers.VirtualMachineHelper virtualMachineHelper = null;
		protected Code.Virtualization2012.Helpers.VirtualMachineHelper VirtualMachineHelper => virtualMachineHelper ??= new Code.Virtualization2012.Helpers.VirtualMachineHelper(this);

		Scheduler scheduler = null;
		protected Scheduler Scheduler => scheduler ??= new Scheduler(this);

		bool isDisposed = false;
		public virtual void Dispose()
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

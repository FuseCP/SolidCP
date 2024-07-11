using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer
{
	public class EnterpriseServerController: ControllerBase
	{
		public EnterpriseServerController(ControllerBase provider) : base(provider) { }
		public EnterpriseServerController(DataProvider dataProvider) : base(dataProvider) { }
		public EnterpriseServerController() : base() { }

		public new AuditLog AuditLog => base.AuditLog;
		public new BackupController BackupController => base.BackupController;

		public new Code.HostedSolution.BlackBerryController BlackBerryController => base.BlackBerryController;

		public new CommentsController CommentsController => base.CommentsController;

		public new CRMController CRMController => base.CRMController;

		public new DnsServerController DnsServerController => base.DnsServerController;

		public new DatabaseServerController DatabaseServerController => base.DatabaseServerController;

		public new EnterpriseStorageController EnterpriseStorageController => base.EnterpriseStorageController;

		public new ExchangeServerController ExchangeServerController => base.ExchangeServerController;

		public new FilesController FilesController => base.FilesController;

		public new FtpServerController FtpServerController => base.FtpServerController;

		public new HeliconZooController HeliconZooController => base.HeliconZooController;

		public new Code.SharePoint.HostedSharePointServerController HostedSharePointServerController => base.HostedSharePointServerController;

		public new Code.SharePoint.HostedSharePointServerEntController HostedSharePointServerEntController => base.HostedSharePointServerEntController;

		public new ImportController ImportController => base.ImportController;

		public new Code.HostedSolution.LyncController LyncController => base.LyncController;

		public new MailServerController MailServerController => base.MailServerController;

		public new Code.HostedSolution.OCSController OCSController => base.OCSController;

		public new OperatingSystemController OperatingSystemController => base.OperatingSystemController;

		public new OrganizationController OrganizationController => base.OrganizationController;

		public new PackageController PackageController => base.PackageController;

		public new RemoteDesktopServicesController RemoteDesktopServicesController => base.RemoteDesktopServicesController;

		public new Code.HostedSolution.ReportController ReportController => base.ReportController;

		public new SchedulerController SchedulerController => base.SchedulerController;

		public new ServerController ServerController => base.ServerController;

		public new Code.HostedSolution.SfBController SfBController => base.SfBController;

		public new SharePointServerController SharePointServerController => base.SharePointServerController;

		public new SpamExpertsController SpamExpertsController => base.SpamExpertsController;

		public new StatisticsServerController StatisticsServerController => base.StatisticsServerController;

		public new StorageSpacesController StorageSpacesController => base.StorageSpacesController;

		public new SystemController SystemController => base.SystemController;

		public new TaskManager TaskManager => base.TaskManager;

		public new TaskController TaskController => base.TaskController;

		public new UserController UserController => base.UserController;

		public new VirtualizationServerController VirtualizationServerController => base.VirtualizationServerController;

		public new VirtualizationServerController2012 VirtualizationServerController2012 => base.VirtualizationServerController2012;

		public new VirtualizationServerControllerForPrivateCloud VirtualizationServerControllerForPrivateCloud => base.VirtualizationServerControllerForPrivateCloud;

		public new VirtualizationServerControllerProxmox VirtualizationServerControllerProxmox => base.VirtualizationServerControllerProxmox;

		public new WebAppGalleryController WebAppGalleryController => base.WebAppGalleryController;

		public new WebServerController WebServerController => base.WebServerController;

		public new APIMailCleanerHelper APIMailCleanerHelper => base.APIMailCleanerHelper;

		public new UserCreationWizard UserCreationWizard => base.UserCreationWizard;

		public new Code.Virtualization2012.UseCase.ChangeVirtualMachineAdministratorPasswordHandler ChangeVirtualMachineAdministratorPasswordHandler => base.ChangeVirtualMachineAdministratorPasswordHandler;

		public new Code.Virtualization2012.UseCase.ChangeVirtualMachineStateHandler ChangeVirtualMachineStateHandler => base.ChangeVirtualMachineStateHandler;

		public new Code.Virtualization2012.UseCase.CreateVirtualMachineHandler CreateVirtualMachineHandler => base.CreateVirtualMachineHandler;

		public new Code.Virtualization2012.UseCase.DeleteVirtualMachineHandler DeleteVirtualMachineHandler => base.DeleteVirtualMachineHandler;

		public new Code.Virtualization2012.UseCase.ImportVirtualMachineHandler ImportVirtualMachineHandler => base.ImportVirtualMachineHandler;

		public new Code.Virtualization2012.UseCase.ReinstallVirtualMachineHandler ReinstallVirtualMachineHandler => base.ReinstallVirtualMachineHandler;

		public new Code.Virtualization2012.UseCase.UpdateVirtualMachineHostNameHandler UpdateVirtualMachineHostNameHandler => base.UpdateVirtualMachineHostNameHandler;

		public new Code.Virtualization2012.UseCase.UpdateVirtualMachineResourceHandler UpdateVirtualMachineResourceHandler => base.UpdateVirtualMachineResourceHandler;

		public new Code.Virtualization2012.Tasks.CreateVirtualMachineTask CreateVirtualMachineTask => base.CreateVirtualMachineTask;

		public new Code.Virtualization2012.Tasks.DeleteVirtualMachineTask DeleteVirtualMachineTask => base.DeleteVirtualMachineTask;

		public new Code.Virtualization2012.Tasks.ReinstallVirtualMachineTask ReinstallVirtualMachineTask => base.ReinstallVirtualMachineTask;

		public new SecurityContext SecurityContext => base.SecurityContext;

		public new UserHelper UserHelper => base.UserHelper;

		public new OneTimePasswordHelper OneTimePasswordHelper => base.OneTimePasswordHelper;

		public new RemoteDesktopServicesHelpers RemoteDesktopServicesHelpers => base.RemoteDesktopServicesHelpers;

		public new ServiceProviderProxy ServiceProviderProxy => base.ServiceProviderProxy;

		public new Code.VirtualizationProxmox.VirtualizationHelperProxmox VirtualizationHelperProxmox => base.VirtualizationHelperProxmox;
		public new Code.Virtualization2012.Helpers.PS.PowerShellScript PowerShellScript => base.PowerShellScript;

		public new Code.Virtualization2012.Helpers.VM.IpAddressExternalHelper IpAddressExternalHelper => base.IpAddressExternalHelper;

		public new Code.Virtualization2012.Helpers.VM.IpAddressPrivateHelper IpAddressPrivateHelper => base.IpAddressPrivateHelper;

		public new Code.Virtualization2012.Helpers.VM.KvpExchangeHelper KvpExchangeHelper => base.KvpExchangeHelper;

		public new Code.Virtualization2012.Helpers.VM.NetworkAdapterDetailsHelper NetworkAdapterDetailsHelper => base.NetworkAdapterDetailsHelper;

		public new Code.Virtualization2012.Helpers.VM.NetworkHelper NetworkHelper => base.NetworkHelper;

		public new Code.Virtualization2012.Helpers.VM.NetworkVLANHelper NetworkVLANHelper => base.NetworkVLANHelper;

		public new Code.Virtualization2012.Helpers.GuacaHelper GuacaHelper => base.GuacaHelper;

		public new Code.Virtualization2012.Helpers.JobHelper JobHelper => base.JobHelper;

		public new Code.Virtualization2012.ReplicationHelper ReplicationHelper => base.ReplicationHelper;

		public new Code.Virtualization2012.VirtualizationHelper VirtualizationHelper => base.VirtualizationHelper;

		public new Code.Virtualization2012.Helpers.VirtualizationUtils VirtualizationUtils => base.VirtualizationUtils;

		public new Code.Virtualization2012.Helpers.VirtualMachineHelper VirtualMachineHelper => base.VirtualMachineHelper;

		public new Scheduler Scheduler => base.Scheduler;
	}
}

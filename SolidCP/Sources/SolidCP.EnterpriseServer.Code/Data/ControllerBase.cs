using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer
{
	public abstract class ControllerBase
	{
		public DataProvider Db { get; private set; }

		ControllerBase Provider = null;
		public ControllerBase(ControllerBase provider) { Provider = provider; }

		public ControllerBase AsAsync()
		{
			var clone = (ControllerBase)Activator.CreateInstance(GetType(), Provider);
			clone.UseAsyncDb();
			return clone;
		}
		public virtual void UseAsyncDb() { Db = new DataProvider(); }
		public virtual WebApplicationsInstaller WebApplicationsInstaller => Provider.WebApplicationsInstaller;
		public virtual AuditLog AuditLog => Provider.AuditLog;
		public virtual BackupController BackupController => Provider.BackupController;
		public virtual BlackBerryController BlackBerryController => Provider.BlackBerryController;
		public virtual CommentsController CommentsController => Provider.CommentsController;
		public virtual CRMController CRMController => Provider.CRMController;
		public virtual DatabaseServerController DatabaseServerController => Provider.DatabaseServerController;
		public virtual EnterpriseStorageController EnterpriseStorageController => Provider.EnterpriseStorageController;
		public virtual ExchangeServerController ExchangeServerController => Provider.ExchangeServerController;
		public virtual FilesController FilesController => Provideer.FilesController;
		public virtual FtpServerController FtpServerController => Provider.FtpServerController;
		public virtual HeliconZooController HeliconZooController => Provider.HeliconZooController;
		public virtual HostedSharePointServerController HostedSharePointServerController => Provider.HostedSharePointServerController;
		public virtual ImportController ImportController => Provider.ImportController;
		public virtual LyncController LyncController => Provider.LyncController;
		public virtual LyncControllerAsync LyncController => Provider.LyncController;
		public virtual MailServerController MailServerController => Provider.MailServerController;
		public virtual OCSController OCSController => Provider.OCSController;
		public virtual OperatingSystemController OperatingSystemController => Provider.OperatingSystemController;
		public virtual OrganizationController OrganizationController => Provider.OrganizationController;
		public virtual OrganizationFoldersManager OrganizationFoldersManager => Provider.OrganizationFoldersManager;
		public virtual PackageController PackageController => Provider.PackageController;
		public virtual RemoteDesktopServicesController RemoteDesktopServicesController => Provider.RemoteDesktopServicesController
		public virtual SchedulerController SchedulerController => Provider.SchedulerController;
		public virtual ServerController ServerController => Provider.ServerController;
		public virtual SfBController SfBController => Provider.SfBController;
		public virtual SfBControllerAsync SfBControllerAsync => Provider.SfBControllerAsync;
		public virtual SharePointServerController SharePointServerController => Provider.SharePointServerController
		public virtual SpamExpertsController SpamExpertsController => Provider.SpamExpertsController;
		public virtual StatisticsServerController StatisticsServerController => Provider.StatisticsServerController;
		public virtual StorageSpacesController StorageSpacesController => Provider.StorageSpacesController;
		public virtual SystemController SystemController => Provider.SystemController;
		public virtual TaskManager TaskManager => Provider.TaskManager;
		public virtual UserController UserController => Provider.UserController;
		public virtual VirtualizationServerController VirtualizationServerController => Provider.VirtualizationServerController;
		public virtual VirtualizationServerController2012 VirtualizationServerController2012 => Provider.VirtualizationServerController2012;
		public virtual VirtualizationServerControllerForPrivateCloud VirtualizationServerControllerForPrivateCloud => Provider.VirtualizationServerControllerForPrivateCloud;
		public virtual VirtualizationServerControllerProxmox VirtualizationServerControllerProxmox => Provider.VirtualizationServerControllerProxmox;
		public virtual WebAppGalleryController WebAppGalleryController => Provider.WebAppGalleryController;
		public virtual WebServerController WebServerController => Provider.WebServerController;


	}
}

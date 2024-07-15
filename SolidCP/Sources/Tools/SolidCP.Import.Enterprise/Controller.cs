using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer;

namespace SolidCP.Import.Enterprise
{
	public class Controller : ControllerBase
	{
		public Controller(ControllerBase provider) : base(provider) { }
		public Controller() : base((DataProvider)null) { }

		public new SecurityContext SecurityContext => base.SecurityContext;
		public new UserController UserController => base.UserController;
		public new PackageController PackageController => base.PackageController;
		public new ServerController ServerController => base.ServerController;
		public new OrganizationController OrganizationController => base.OrganizationController;
		public new ExchangeServerController ExchangeServerController => base.ExchangeServerController;

		OrganizationImporter organizationImporter = null;
		public OrganizationImporter OrganizationImporter => organizationImporter ??= new OrganizationImporter(this);
	}
}

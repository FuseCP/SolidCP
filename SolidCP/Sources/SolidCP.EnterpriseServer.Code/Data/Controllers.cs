using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer
{
	public class Controllers: ControllerBase
	{
		public new UserController UserController => base.UserController;
		public new ServerController ServerController => base.ServerController;
		public new PackageController PackageController => base.PackageController;
		public new SecurityContext SecurityContext => base.SecurityContext;
	}
}

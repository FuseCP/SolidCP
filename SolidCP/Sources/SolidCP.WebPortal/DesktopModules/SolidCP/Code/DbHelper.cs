using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidCP.Portal
{
	public class DbHelper
	{

		static EnterpriseServer.Data.DbType? dbType = null;
		public static EnterpriseServer.Data.DbType DbType => dbType ??= ES.Services.System.GetDatabaseType();

		static bool? useEntityFramework = null;
		public static bool UseEntityFramework
		{
			get
			{
				return useEntityFramework ??= ES.Services.System.GetUseEntityFramework();
            }
			set
			{
				ES.Services.System.SetUseEntityFramework(value);
				useEntityFramework = value;
			}
		}

		public static int SetUseEntityFramework(bool useEntityFramework)
		{
			var result = ES.Services.System.SetUseEntityFramework(useEntityFramework);
			DbHelper.useEntityFramework = useEntityFramework;
			if (useEntityFramework) showUseEntityFrameworkCheckbox = true;
			return result;
		}

		static bool? showUseEntityFrameworkCheckbox = null;
		public static bool ShowUseEntityFrameworkCheckbox => PortalUtils.AuthTicket != null && PortalUtils.AuthTicket.Name == "serveradmin" && (showUseEntityFrameworkCheckbox ??=
			DbType == EnterpriseServer.Data.DbType.SqlServer && UseEntityFramework);
	}
}
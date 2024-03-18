using System;
using System.Collections.Generic;
using System.Text;
using SolidCP.Providers.SharePoint;

namespace SolidCP.EnterpriseServer.Base.HostedSolution
{
	public class CalculateSharePointSitesDiskSpaceResult
	{
		public SharePointSiteDiskSpace[] Result { get; set; }
		public int ErrorCode { get; set; }
	}
}

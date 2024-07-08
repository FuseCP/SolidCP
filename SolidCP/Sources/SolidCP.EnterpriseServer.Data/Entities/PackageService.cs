using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities
{
	[Table("PackageServices")]
#if NetCore
	[PrimaryKey("PackageId", "ServiceId")]
#endif
	public class PackageService
	{
		[Key, Column("PackageID", Order = 1)]
		public int PackageId { get; set; }
		[Key, Column("ServiceID", Order = 2)]
		public int ServiceId { get; set; }
	}
}

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
	public class PackageService
	{
		[Key, Column("PackageID")]
		public int PackageId { get; set; }
		[Key, Column("ServiceID")]
		public int ServiceId { get; set; }
	}
}

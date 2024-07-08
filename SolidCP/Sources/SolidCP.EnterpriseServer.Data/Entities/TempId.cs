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
	[Table("TempIds")]
#if NetCore
	[Index("Created", "Scope", "Level")]
#endif
	public class TempId
	{
		public DateTime Created { get; set; }

		public Guid Scope { get; set; }

		public int Level { get; set; }

		[Key]
		public int Key { get; set; }

		public int Id { get; set; }

		public DateTime Date { get; set; }
	}
}

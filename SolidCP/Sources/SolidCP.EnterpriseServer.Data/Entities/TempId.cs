using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidCP.EnterpriseServer.Data.Entities
{
	public class TempId
	{
		[Key]
		public int Key { get; set; }
		public Guid Scope { get; set; }
		public int Id { get; set; }
		public DateTime Created { get; set; }
	}
}

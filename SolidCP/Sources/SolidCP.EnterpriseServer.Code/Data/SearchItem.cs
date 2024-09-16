using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code
{
	public class SearchItem
	{
		public int ItemId { get; set; }
		public string TextSearch { get; set; }
		public string ColumnType { get; set; }
		public string FullType { get; set; }
		public int? PackageId { get; set; }
		public int AccountId { get; set; }
		public string Username { get; set; }
		public string FullName { get; set; }
	}
}

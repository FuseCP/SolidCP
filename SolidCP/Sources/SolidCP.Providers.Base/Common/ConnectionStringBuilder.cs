using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Common
{

	public class ConnectionStringBuilder : System.Data.Common.DbConnectionStringBuilder
	{
		public override object this[string keyword]
		{
			get
			{
				object val;
				if (TryGetValue(keyword, out val)) return val;
				return null;
			}
			set
			{
				if (value == null) Remove(keyword);
				else base[keyword] = value;
			}
		}

		public ConnectionStringBuilder() : base() { }
		public ConnectionStringBuilder(string connectionString): base() { ConnectionString = connectionString; }
	}
}
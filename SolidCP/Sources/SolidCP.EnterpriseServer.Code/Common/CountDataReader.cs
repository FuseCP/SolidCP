using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Collections;

namespace SolidCP.EnterpriseServer
{

	public class CountDataReader<TEntity>: EntityDataReader<TEntity> where TEntity: class
	{
		public CountDataReader(IEnumerable<TEntity> set, int count = -1, bool setRecordsAffected = false) : base(set, setRecordsAffected) {
			this.count = count;
		}

		bool isReadingCount = true;

		int count = -1;
		int Count => count != -1 ? count : Set.Count();
		public override int FieldCount => isReadingCount ? 1 : base.FieldCount;
		public override IDataReader GetData(int i)
		{
			if (isReadingCount) throw new NotSupportedException();
			return base.GetData(i);
		}
		public override string GetDataTypeName(int i)
		{
			if (isReadingCount) return typeof(int).FullName;
			return base.GetDataTypeName(i);
		}

		public override Type GetFieldType(int i)
		{
			if (isReadingCount) return typeof(int);
			return base.GetFieldType(i);
		}

		public override string GetName(int i)
		{
			if (isReadingCount) return "Count";
			return base.GetName(i);
		}

		public override int GetOrdinal(string name)
		{
			if (isReadingCount) return 0;
			return base.GetOrdinal(name);
		}

		public override DataTable GetSchemaTable()
		{
			if (isReadingCount)
			{
				var schema = new DataTable();
				schema.Columns.Add(new DataColumn("Count", typeof(int)));
				return schema;
			}
			else
			{
				return base.GetSchemaTable();
			}
		}

		public override object GetValue(int i)
		{
			if (isReadingCount) return Count;
			return base.GetValue(i);
		}

		public override object this[int i] => isReadingCount ? Count : base[i];
		public override object this[string name] => isReadingCount ? Count : base[name];

		public override int GetValues(object[] values)
		{
			if (isReadingCount)
			{
				values[0] = Count;
				return 1;
			}
			return base.GetValues(values);
		}
		public override bool Read()
		{
			if (isReadingCount) return false;
			return base.Read();
		}
		public override bool NextResult()
		{
			if (isReadingCount)
			{
				isReadingCount = false;
				return true;
			}
			return base.NextResult();
		}
	}
}

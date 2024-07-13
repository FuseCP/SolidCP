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

namespace SolidCP.EnterpriseServer.Data
{
	public class CountDataReader<TEntity>: EntityDataReader<TEntity> where TEntity: class
	{
		public CountDataReader(IEnumerable<TEntity> set, int count = -1) : base(set) {
			this.count = count;
		}

		bool isReadingCount = true;

		int count = -1;
		int Count
		{
			get
			{
				if (count == -1)
				{
#if NETCOREAPP
					if (!Set.TryGetNonEnumeratedCount(out count)) count = Set.Count();
#else
					count = Set.Count();
#endif
				}
				return count;
			}
		}
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
				var table = new DataTable();
				table.Columns.Add("AllowDBNull", typeof(bool));
				var baseCatalogName = new DataColumn("BaseCatalogName", typeof(string)) { AllowDBNull = true };
				table.Columns.Add(baseCatalogName);
				var baseColumnName = new DataColumn("BaseColumnName", typeof(string)) { AllowDBNull = true };
				table.Columns.Add(baseColumnName);
				var baseSchemaName = new DataColumn("BaseSchemaName", typeof(string)) { AllowDBNull = true };
				table.Columns.Add(baseSchemaName);
				var baseServerName = new DataColumn("BaseServerName", typeof(string)) { AllowDBNull = true };
				table.Columns.Add(baseServerName);
				var baseTableName = new DataColumn("BaseTableName", typeof(string)) { AllowDBNull = true };
				table.Columns.Add(baseTableName);
				table.Columns.Add("ColumnName", typeof(string));
				table.Columns.Add("ColumnOrdinal", typeof(int));
				table.Columns.Add("ColumnSize", typeof(int));
				table.Columns.Add("DataType", typeof(Type));
				table.Columns.Add("DataTypeName", typeof(string));
				table.Columns.Add("IsAliased", typeof(bool));
				table.Columns.Add("IsAutoIncrement", typeof(bool));
				table.Columns.Add("IsColumnSet", typeof(bool));
				table.Columns.Add("IsExpression", typeof(bool));
				table.Columns.Add("IsHidden", typeof(bool));
				table.Columns.Add("IsIdentity", typeof(bool));
				table.Columns.Add("IsKey", typeof(bool));
				table.Columns.Add("IsLong", typeof(bool));
				table.Columns.Add("IsReadOnly", typeof(bool));
				table.Columns.Add("IsRowVersion", typeof(bool));
				table.Columns.Add("IsUnique", typeof(bool));
				var row = table.NewRow();
				row["AllowDBNull"] = false;
				row["BaseCatalogName"] = DBNull.Value;
				row["BaseColumnName"] = DBNull.Value;
				row["BaseSchemaName"] = DBNull.Value;
				row["BaseServerName"] = "";
				row["BaseTableName"] = "Count";
				row["ColumnName"] = "Count";
				row["ColumnOrdinal"] = 0;
				row["ColumnSize"] = int.MaxValue;
				row["DataType"] = typeof(int);
				row["DataTypeName"] = typeof(int).FullName;
				row["IsAliased"] = false;
				row["IsAutoIncrement"] = false;
				row["IsColumnSet"] = false;
				row["IsExpression"] = false;
				row["IsHidden"] = false;
				row["IsIdentity"] = false;
				row["IsKey"] = false;
				row["IsLong"] = false;
				row["IsReadOnly"] = false;
				row["IsRowVersion"] = false;
				row["IsUnique"] = false;
				table.Rows.Add(row);
				return table;
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
		bool hasReadCount = false;
		public override bool Read()
		{
			if (isReadingCount)
			{
				var success = !hasReadCount;
				hasReadCount = true;
				return success;
			}
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

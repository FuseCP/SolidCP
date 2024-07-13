using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Runtime.CompilerServices;

namespace SolidCP.EnterpriseServer.Data
{
	public class PropertyDescription
	{
		public PropertyInfo Info;
		public bool IsNullable;
		public bool IsString;
		public DataColumn Column;
		public Type Type;
	}

	public class PropertyCollection: KeyedCollection<string, PropertyDescription>
	{
		public PropertyCollection(): base(StringComparer.OrdinalIgnoreCase) { }
		protected override string GetKeyForItem(PropertyDescription item) => item.Info.Name;
	}

	public interface IEntityDataSet
	{
		IEnumerable Set { get; }
		Type Type { get; }
		PropertyCollection Properties { get; }
	}

	public class EntityDataReader<TEntity>: IDataReader, IEntityDataSet, IEnumerable<TEntity> where TEntity : class
	{
		public virtual IEnumerable<TEntity> Set { get; set; }
		IEnumerable IEntityDataSet.Set => Set;
        public virtual Type Type { get; private set; }

        PropertyCollection properties = null;

		public virtual PropertyCollection Properties
        {
            get
            {
                if (properties == null)
                {
					var props = ObjectUtils.GetTypeProperties(Type)
						// Filter out navigation & hidden properties
						.Where(p => (!p.PropertyType.IsClass || p.PropertyType == typeof(string) ||
						p.PropertyType == typeof(Guid) || p.PropertyType == typeof(byte[])) &&
						!p.PropertyType.IsInterface &&
						(p.CanWrite || ObjectUtils.IsAnonymous(Type)) && p.CanRead &&
						p.GetCustomAttribute<NotMappedAttribute>() == null);
					var propDescs = props
						.Select(p =>
						{
							var ptype = p.PropertyType;
							var isString = ptype == typeof(string);
							var isNullable = ptype.IsGenericType && ptype.GetGenericTypeDefinition() == typeof(Nullable<>) &&
									!ptype.IsGenericTypeDefinition;
							//var column = new DataColumn(EntityDataTable<TEntity>.ColumnName<TEntity>(p.Name));
							var column = new DataColumn(p.Name);
							column.AllowDBNull = isNullable || isString;
							var ctype = isNullable ? Nullable.GetUnderlyingType(ptype) : ptype;
							column.DataType = ctype;

							return new PropertyDescription()
							{
								Info = p,
								Column = column,
								Type = ctype,
								IsNullable = isNullable,
								IsString = isString
							};
						});
					properties = new PropertyCollection();
                    foreach (var prop in propDescs) properties.Add(prop);
                }
                return properties;
            }
			private set { properties = value; }
        }

        public EntityDataReader(IEnumerable<TEntity> set) {
			Set = set;
			RecordsAffected = -1;
			Type = typeof(TEntity);
			if (Type == typeof(object)) Type = Set.FirstOrDefault(e => e != null)?.GetType() ?? typeof(object);
		}

		IEnumerator<TEntity> enumerator = null;
		IEnumerator<TEntity> Enumerator => enumerator ??= ((IEnumerable<TEntity>)Set).GetEnumerator();

		TEntity Current
		{
			get
			{
				if (IsClosed) throw new ArgumentException("DataReader is closed");
				return Enumerator.Current;
			}
		}

		public virtual int Depth => 1;

		public virtual bool IsClosed { get; private set; } = false;

		public virtual int RecordsAffected { get; private set; }

		public virtual int FieldCount => Properties.Count;

		private object GetValueFromProperty(PropertyDescription p)
		{
			try {
				var val = p.Info.GetValue(Enumerator.Current);
				if (p.IsNullable || p.IsString)
				{
					if (val == null) return DBNull.Value;
					else if (p.IsNullable)
					{
						// get val.Value property value
						var valueProperty = p.Info.PropertyType.GetProperty("Value");
						return valueProperty.GetValue(val);
					}
				}
				return val;
			} catch (Exception ex)
			{
				throw;
			}
		}

		public virtual object this[int i] => GetValueFromProperty(Properties[i]);

		public virtual object this[string name] => GetValueFromProperty(Properties[name]);
		
		public virtual IEnumerator<TEntity> GetEnumerator() => ((IEnumerable<TEntity>)Set).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Set).GetEnumerator();

		public virtual void Close() => Dispose();

		private DataTable schemaTable = null;
		public virtual DataTable GetSchemaTable()
		{
			if (schemaTable == null)
			{
				schemaTable = new DataTable();
				schemaTable.CaseSensitive = false;
				schemaTable.Columns.Add("ColumnName", typeof(string));
				schemaTable.Columns.Add("ColumnOrdinal", typeof(int));
				schemaTable.Columns.Add("ColumnSize", typeof(int));
				schemaTable.Columns.Add("NumericPrecision", typeof(int));
				schemaTable.Columns.Add("NumericScale", typeof(int));
				schemaTable.Columns.Add("DataType", typeof(Type));
				schemaTable.Columns.Add("ProviderType", typeof(object));
				schemaTable.Columns.Add("IsLong", typeof(bool));
				schemaTable.Columns.Add("AllowDBNull", typeof(bool));
				schemaTable.Columns.Add("IsReadOnly", typeof(bool));
				schemaTable.Columns.Add("IsRowVersion", typeof(bool));
				schemaTable.Columns.Add("IsUnique", typeof(bool));
				schemaTable.Columns.Add("IsKey", typeof(bool));
				schemaTable.Columns.Add("IsAutoIncrement", typeof(bool));
				var baseCatalogName = new DataColumn("BaseCatalogName", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(baseCatalogName);
				var baseSchemaName = new DataColumn("BaseSchemaName", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(baseSchemaName);
				var baseTableName = new DataColumn("BaseTableName", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(baseTableName);
				var baseColumnName = new DataColumn("BaseColumnName", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(baseColumnName);
				schemaTable.Columns.Add("AutoIncrementSeed", typeof(int));
				schemaTable.Columns.Add("AutoIncrementStep", typeof(int));
				var defaultValue = new DataColumn("DefaultValue", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(defaultValue);
				var expression = new DataColumn("Expression", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(expression);
				schemaTable.Columns.Add("ColumnMapping", typeof(MappingType));
				var baseTableNamespace = new DataColumn("BaseTableNamespace", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(baseTableNamespace);
				var baseColumnNamespace = new DataColumn("BaseColumnNamespace", typeof(string)) { AllowDBNull = true };
				schemaTable.Columns.Add(baseColumnNamespace);
				int i = 0;
				foreach (var prop in Properties)
				{
					var row = schemaTable.NewRow();
					row["ColumnName"] = prop.Info.Name;
					row["ColumnOrdinal"] = i++;
					row["ColumnSize"] = int.MaxValue;
					row["NumericPrecision"] = DBNull.Value;
					row["NumericScale"] = DBNull.Value;
					row["DataType"] = prop.Type;
					row["ProviderType"] = prop.Type;
					row["IsLong"] = false;
					row["AllowDBNull"] = prop.IsNullable;
					row["IsReadOnly"] = false;
					row["IsRowVersion"] = false;
					row["IsUnique"] = false;
					row["IsKey"] = false;
					row["IsAutoIncrement"] = false;
					row["BaseCatalogName"] = DBNull.Value;
					row["BaseSchemaName"] = DBNull.Value;
					row["BaseTableName"] = Type.Name;
					row["BaseColumnName"] = DBNull.Value;
					row["AutoIncrementSeed"] = 1;
					row["AutoIncrementStep"] = 1;
					row["DefaultValue"] = DBNull.Value;
					row["Expression"] = DBNull.Value;
					row["ColumnMapping"] = MappingType.Element;
					row["BaseTableNamespace"] = "";
					row["BaseColumnNamespace"] = "";
					schemaTable.Rows.Add(row);
				}
			}
			return schemaTable;
		}

		public virtual bool NextResult() => false;
		public virtual bool Read() => !(IsClosed = !Enumerator.MoveNext());

		public void Dispose() {
			IsClosed = true;
			if (enumerator != null && enumerator is IDisposable disposableEnum) disposableEnum.Dispose();
			enumerator = null;
			if (Set != null && Set is IDisposable disposableSet) disposableSet.Dispose();
			Set = null;
		}
		public virtual string GetName(int i) => Properties[i].Info.Name;
		public virtual string GetDataTypeName(int i) => Properties[i].Type.FullName;
		public virtual Type GetFieldType(int i) => Properties[i].Type;
		public virtual object GetValue(int i) => this[i];
		public virtual int GetValues(object[] values) {
			int n = 0;
			foreach (var prop in Properties)
			{
				if (n >= values.Length) break;
				values[n++] = GetValueFromProperty(prop);
			}
			return n;
		}
		public virtual int GetOrdinal(string name) => Properties.IndexOf(Properties[name]);
		public bool GetBoolean(int i) => (bool)this[i];
		public byte GetByte(int i) => (byte)this[i];
		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			var bytes = (byte[])this[i];
			long len = Math.Min(length, Math.Min(bytes.Length - fieldOffset, buffer.Length - bufferoffset));
			Array.Copy(bytes, fieldOffset, buffer, bufferoffset, len);
			return len;
		}
		public char GetChar(int i) => (char)this[i];
		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			var chars = (char[])this[i];
			long len = Math.Min(length, Math.Min(chars.Length - fieldoffset, buffer.Length - bufferoffset));
			Array.Copy(chars, fieldoffset, buffer, bufferoffset, len);
			return len;
		}
		public Guid GetGuid(int i) => (Guid)this[i];
		public short GetInt16(int i) => (Int16)this[i];
		public int GetInt32(int i) => (Int32)this[i];
		public long GetInt64(int i) => (Int64)this[i];
		public float GetFloat(int i) => (float)this[i];
		public double GetDouble(int i) => (double)this[i];
		public string GetString(int i) => (string)this[i];
		public decimal GetDecimal(int i) => (decimal)this[i];
		public DateTime GetDateTime(int i) => (DateTime)this[i];
		public virtual IDataReader GetData(int i)
		{
			var reader = new EntityDataReader<TEntity>(Set);
			var props = new PropertyCollection() { Properties[i] };
			reader.Properties = props;
			return reader;
		}
		public bool IsDBNull(int i) => Properties[i].Info.GetValue(Enumerator.Current) == null;
	}
}

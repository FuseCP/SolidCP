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

	public class PropertyCollection: KeyedCollection<string, PropertyInfo>
	{
		protected override string GetKeyForItem(PropertyInfo item) => item.Name;
	}

	public interface IEntityDataSet
	{
		IEnumerable Set { get; }
		Type Type { get; }
		PropertyCollection Properties { get; }
	}

	public class EntityDataReader<TEntity>: IDataReader, IEntityDataSet, IEnumerable<TEntity> where TEntity : class
	{
		public TEntity[] Set { get; private set; }
		IEnumerable IEntityDataSet.Set => Set;
        public Type Type { get; private set; }

        PropertyCollection properties = null;
        public PropertyCollection Properties
        {
            get
            {
                if (properties == null)
                {
					var props = ObjectUtils.GetTypeProperties(Type)
						.Where(p => p.CanWrite && p.CanRead && p.GetCustomAttribute<NotMappedAttribute>() == null);
					properties = new PropertyCollection();
                    foreach (var prop in props) properties.Add(prop);
                }
                return properties;
            }
			private set { properties = value; }
        }

        public EntityDataReader(IEnumerable<TEntity> set, bool setRecordsAffected = false) {
			Set = set.ToArray();
			if (setRecordsAffected) RecordsAffected = Set.Length;
			else RecordsAffected = -1;
			Type = typeof(TEntity);
			if (Type == typeof(object)) Type = Set.FirstOrDefault(e => e != null)?.GetType() ?? typeof(object);
		}

		IEnumerator<TEntity> enumerator = null;
		IEnumerator<TEntity> Enumerator => enumerator ?? (enumerator = ((IEnumerable<TEntity>)Set).GetEnumerator());

		TEntity Current
		{
			get
			{
				if (IsClosed) throw new ArgumentException("DataReader is closed");
				return Enumerator.Current;
			}
		}

		public int Depth => 1;

		public bool IsClosed { get; private set; } = false;

		public int RecordsAffected { get; private set; }

		public int FieldCount => Properties.Count;

		public object this[string name] => Properties[name].GetValue(Enumerator.Current);

		public object this[int i] => Properties[i].GetValue(Enumerator.Current);

		public IEnumerator<TEntity> GetEnumerator() => ((IEnumerable<TEntity>)Set).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Set).GetEnumerator();

		public void Close() => Dispose();

		public DataTable GetSchemaTable()
		{
			var table = new DataTable();
			foreach (var prop in Properties) table.Columns.Add(prop.Name, prop.PropertyType);
			return table;
		}

		public bool NextResult() => false;
		public bool Read() => !(IsClosed = !Enumerator.MoveNext());
		public void Dispose() {
			IsClosed = true;
			Set = null;
			enumerator = null;
		}
		public string GetName(int i) => Properties[i].Name;
		public string GetDataTypeName(int i) => Properties[i].PropertyType.FullName;
		public Type GetFieldType(int i) => Properties[i].PropertyType;
		public object GetValue(int i) => Properties[i].GetValue(Enumerator.Current);
		public int GetValues(object[] values) {
			int n = 0;
			foreach (var prop in Properties)
			{
				if (n >= values.Length) break;
				values[n++] = prop.GetValue(Enumerator.Current);
			}
			return n;
		}
		public int GetOrdinal(string name) => Properties.IndexOf(Properties[name]);
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
		public IDataReader GetData(int i)
		{
			var reader = new EntityDataReader<TEntity>(Set, RecordsAffected != -1);
			var props = new PropertyCollection() { Properties[i] };
			reader.Properties = props;
			return reader;
		}
		public bool IsDBNull(int i) => this[i] == null;
	}
}

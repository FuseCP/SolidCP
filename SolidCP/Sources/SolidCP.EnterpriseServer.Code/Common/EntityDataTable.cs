using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Collections;
using Twilio.TwiML.Voice;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;

namespace SolidCP.EnterpriseServer.Code
{
    public class EntityDataTable<TEntity>: DataTable, IEntityDataSet where TEntity: class
    {
        [IgnoreDataMember]
        public TEntity[] Set { get; set; } = null;
        IEnumerable IEntityDataSet.Set => Set;

        PropertyCollection properties = null;
        public PropertyCollection Properties {
            get {
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
        }
        [IgnoreDataMember]
        public Type Type { get; set; } = null;

        public EntityDataTable(IEnumerable<TEntity> set)
        {
            Set = set.ToArray();
            Type = typeof(TEntity);
            if (Type == typeof(object)) Type = Set.FirstOrDefault(e => e != null)?.GetType() ?? typeof(object);
            Populate();
        }

        public DataTable Populate()
        {
			foreach (var p in Properties) Columns.Add(p.Column);

            foreach (var entity in Set)
            {
                var row = NewRow();
                foreach (var prop in Properties)
                {
                    var val = prop.Info.GetValue(entity);
					if (prop.IsNullable || prop.IsString)
					{
						if (val == null) val = DBNull.Value;
						else if (prop.IsNullable)
						{
							// get val.Value property value
							var valueProperty = prop.Info.PropertyType.GetProperty("Value");
							val = valueProperty.GetValue(val);
						}
					}
					row[prop.Info.Name] = val;
                }
				Rows.Add(row);
            }
            return this;
        }

        public void OnSerialize(StreamingContext context) => Populate();
        public static void Init()
        {

        }

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidCP.EnterpriseServer.Data
{
    public class EntityDataTable<TEntity>: DataTable, IEntityDataSet where TEntity: class
    {
        [XmlIgnore, IgnoreDataMember]
        public TEntity[] Set { get; set; } = null;
        IEnumerable IEntityDataSet.Set => Set;

		/*
		static HashSet<string> lowerCaseColumns = new HashSet<string>(new[] { "LoginStatusId", "PreviousId",
			"SfBUserPlanId", "LyncUserPlanId", "MailboxPlanId", "DomainItemId", "BlackBerryUserId",
			"ArchivingMailboxPlanId" });
		public static string ColumnName<TEntity1>(string propertyName)
		{
			//return propertyName;
			if (propertyName == "IpAddressId") return "IPAddressID";
			if (propertyName == "CrmUserId") return "CRMUserID";
			if (propertyName.EndsWith("Id") && !lowerCaseColumns.Contains(propertyName))
				return propertyName.Substring(0, propertyName.Length - 1) + "D";
			if ((propertyName.StartsWith("Csr") || propertyName.StartsWith("Crm")) &&
				!lowerCaseColumns.Contains(propertyName))
				return propertyName.Substring(0,3).ToUpper() + propertyName.Substring(3);
			if (propertyName.EndsWith("Ip") && !lowerCaseColumns.Contains(propertyName))
				return propertyName.Substring(0, propertyName.Length - 1) + "P";
			if (propertyName == "EnableImap" || propertyName == "EnableMapi" || propertyName == "EnableOwa" ||
				propertyName == "EnablePop") return "Enable" + propertyName.Substring(6).ToUpper();
			if (propertyName.EndsWith("Mb") && !lowerCaseColumns.Contains(propertyName))
				return propertyName.Substring(0, propertyName.Length - 1) + "B";
			if (propertyName.EndsWith("Kb") && !lowerCaseColumns.Contains(propertyName))
				return propertyName.Substring(0, propertyName.Length - 1) + "B";
			if (propertyName == "ServerUri") return "ServerURI";
			if (propertyName == "AuthenticateUsingNla") return "AuthenticateUsingNLA";
			if (propertyName == "StatusIdChangeDate") return "StatusIDchangeDate";
			if (propertyName == "Vlan") return "VLAN";
			if (propertyName == "IpAddress") return "IPAddress";
			if (propertyName == "IsVip") return "IsVIP";
			if (propertyName == "CalType") return "CALType";
			return propertyName;
		}*/

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
							//var column = new DataColumn(ColumnName<TEntity>(p.Name));
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
			CaseSensitive = false;
            Set = set.ToArray();
			if (set is IDisposable disposable) disposable.Dispose();
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

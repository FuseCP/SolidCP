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

namespace SolidCP.EnterpriseServer.Code.Common
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
                        .Where(p => p.CanWrite && p.CanRead && p.GetCustomAttribute<NotMappedAttribute>() == null);
                    properties = new PropertyCollection();
                    foreach (var prop in props) properties.Add(prop);
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
            foreach (var entity in Set)
            {
                var row = NewRow();
                foreach (var prop in Properties)
                {
                    row[prop.Name] = prop.GetValue(entity);
                }
            }
            return this;
        }

        public void OnSerialize(StreamingContext context) => Populate();
        protected override Type GetRowType() => Type;

        public static void Init()
        {

        }

	}
}

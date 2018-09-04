// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace SolidCP.Providers
{
	/// <summary>
	/// Summary description for SoapObject.
	/// </summary>
	[Serializable]
	public class SoapServiceProviderItem
	{
        // static fields
        private static Hashtable typeProperties = new Hashtable();

		private string[] properties;
		private string typeName;

        public SoapServiceProviderItem()
		{
		}

		public string[] Properties
		{
			get { return properties; }
			set { properties = value; }
		}

		public string TypeName
		{
			get { return typeName; }
			set { typeName = value; }
		}

        // static methods
        public static SoapServiceProviderItem Wrap(ServiceProviderItem item)
        {
            if (item == null)
                return null;

            // wrap only "persistent" properties
            SoapServiceProviderItem sobj = new SoapServiceProviderItem();
            sobj.TypeName = item.GetType().AssemblyQualifiedName;

            // add common properties
            Hashtable props = GetObjectProperties(item, true);
            props.Add("Id", item.Id.ToString());
            props.Add("Name", item.Name);
            props.Add("ServiceId", item.ServiceId.ToString());
            props.Add("PackageId", item.PackageId.ToString());

            List<string> wrProps = new List<string>();
            foreach (string key in props.Keys)
            {
                wrProps.Add(key + "=" + props[key].ToString());
            }

            sobj.Properties = wrProps.ToArray();
            return sobj;
        }

        public static ServiceProviderItem Unwrap(SoapServiceProviderItem sobj)
        {
            Type type = Type.GetType(sobj.TypeName);
            ServiceProviderItem item = (ServiceProviderItem)Activator.CreateInstance(type);

            // get properties
            if (sobj.Properties != null)
            {
                // get type properties and add it to the hash
                Dictionary<string, PropertyInfo> hash = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] propInfos = GetTypeProperties(type);

                foreach (PropertyInfo propInfo in propInfos)
                    hash.Add(propInfo.Name, propInfo);

                // set service item properties
                foreach (string pair in sobj.Properties)
                {
                    try //TODO: that try catch is a dirty fix. Without it we get that issue System.ArgumentException: Object of type 'System.String' cannot be converted to type 'SolidCP.Providers.Virtualization.VirtualHardDiskInfo[]'.
                        //possible that method works only with simple objects.
                    {
                        int idx = pair.IndexOf('=');
                        string name = pair.Substring(0, idx);
                        string val = pair.Substring(idx + 1);
                        if (hash.ContainsKey(name))
                        {
                            // set value
                            PropertyInfo propInfo = hash[name];
                            propInfo.SetValue(item, Cast(val, propInfo.PropertyType), null);
                        }
                    }
                    catch { }
                }
            }

            return item;
        }

        private static Hashtable GetObjectProperties(object obj, bool persistentOnly)
        {
            Hashtable hash = new Hashtable();

            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Instance
                | BindingFlags.Public);
            foreach (PropertyInfo prop in props)
            {
                // check for persistent attribute
                object[] attrs = prop.GetCustomAttributes(typeof(PersistentAttribute), false);
                if (!persistentOnly || (persistentOnly && attrs.Length > 0))
                {
                    object val = prop.GetValue(obj, null);
                    string s = "";
                    if (val != null)
                    {
                        if (prop.PropertyType == typeof(string[]))
                            s = String.Join(";", (string[])val);
                        else if (prop.PropertyType == typeof(int[]))
                        {
                            int[] ivals = (int[])val;
                            string[] svals = new string[ivals.Length];
                            for (int i = 0; i < svals.Length; i++)
                                svals[i] = ivals[i].ToString();
                            s = String.Join(";", svals);
                        }
                        else
                            s = val.ToString();
                    }

                    // add property to hash
                    hash.Add(prop.Name, s);
                }
            }

            return hash;
        }

        private static PropertyInfo[] GetTypeProperties(Type type)
        {
            string typeName = type.AssemblyQualifiedName;
            if (typeProperties[typeName] != null)
                return (PropertyInfo[])typeProperties[typeName];

            PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            typeProperties[typeName] = props;
            return props;
        }

        private static object Cast(string val, Type type)
        {
            if (type == typeof(string))
                return val;
            if (type == typeof(Int32))
                return Int32.Parse(val);
            if (type == typeof(Int64))
                return Int64.Parse(val);
            if (type == typeof(Boolean))
                return Boolean.Parse(val);
            if (type == typeof(Decimal))
                return Decimal.Parse(val);
            if (type == typeof(Guid))
                return new Guid(val);
            if (type.IsEnum)
                return Enum.Parse(type, val, true);
            if (type == typeof(string[]) && val != null)
            {
                return val.Split(';');
            }
            if (type == typeof(int[]) && val != null)
            {
                string[] sarr = val.Split(';');
                int[] iarr = new int[sarr.Length];
                for (int i = 0; i < sarr.Length; i++)
                    iarr[i] = Int32.Parse(sarr[i]);
                return iarr;
            }
            else
                return val;
        }
	}
}

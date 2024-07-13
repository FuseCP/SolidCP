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
using System.IO;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for ObjectUtils.
	/// </summary>
	public class ObjectUtils
	{
		public static DT ConvertObject<ST, DT>(ST so)
		{
			Dictionary<string, PropertyInfo> sProps = GetTypePropertiesHash(typeof(ST));
			Dictionary<string, PropertyInfo> dProps = GetTypePropertiesHash(typeof(DT));

			DT dobj = (DT)Activator.CreateInstance(typeof(DT));

			// copy properties
			foreach (string propName in sProps.Keys)
			{
				if (dProps.ContainsKey(propName) && sProps[propName].Name != "Item")
				{
					if (sProps[propName].CanRead)
					{
						object val = sProps[propName].GetValue(so, null);
						if (dProps[propName] != null)
						{
							if (val != null && dProps[propName].CanWrite)
							{
								dProps[propName].SetValue(dobj, val, null);
							}
						}
					}
				}
			}
			return dobj;
		}

		public static bool IsAnonymous(Type type) => Data.ObjectUtils.IsAnonymous(type);
		public static PropertyInfo[] GetTypeProperties(Type type) => Data.ObjectUtils.GetTypeProperties(type);
		public static Hashtable GetObjectProperties(object obj, bool persistentOnly)
		{
			Hashtable hash = new Hashtable();

			Type type = obj.GetType();
			PropertyInfo[] props = type.GetProperties(BindingFlags.Instance
				| BindingFlags.Public);
			foreach (PropertyInfo prop in props)
			{
				// check for persistent attribute
				object[] attrs = prop.GetCustomAttributes(typeof(PersistentAttribute), false);
				if (!persistentOnly || (persistentOnly && attrs.Length > 0) && !hash.ContainsKey(prop.Name))
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
						// when property is custom class with Persistent attribute
						else if (prop.PropertyType.GetCustomAttributes(typeof(PersistentAttribute), false).Length > 0)
						{
							// add sub-class properties to hash
							var childHash = GetObjectProperties(val, persistentOnly);
							foreach (var hashKey in childHash.Keys)
							{
								var value = childHash[hashKey];
								hash.Add(prop.Name + "." + hashKey, value);
							}
							// exit
							continue;
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

		public static void FillCollectionFromDataSet<T>(List<T> list, DataSet ds)
		{
			if (ds.Tables.Count == 0)
				return;

			FillCollectionFromDataTable<T>(list, ds.Tables[0]);
		}

		public static void FillCollectionFromDataTable<T>(List<T> list, DataTable table)
		{
			if (table is Data.IEntityDataSet entitySet) FillCollectionFromEntitySet(list, entitySet);
			else FillCollectionFromDataView(list, table.DefaultView);
		}

		public static void FillCollectionFromDataView<T>(List<T> list, DataView dv)
		{
			Type type = typeof(T);

			PropertyInfo[] props = GetTypeProperties(type);

			foreach (DataRowView dr in dv)
			{
				// create an instance
				T obj = (T)Activator.CreateInstance(type);
				list.Add(obj);

				// fill properties
				for (int i = 0; i < props.Length; i++)
				{
					string propName = props[i].Name;
					if (dv.Table.Columns[propName] == null)
						continue;

					object propVal = dr[propName];
					if (propVal == DBNull.Value || propVal == null)
						props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
					else
					{
						try
						{
							// try implicit type conversion
							props[i].SetValue(obj, propVal, null);
						}
						catch
						{
							// convert to string and then set property value
							try
							{
								string strVal = propVal.ToString();
								props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
							}
							catch
							{
								// skip property init
							}
						}
					}
				} // for properties
			} // for rows
		}

		public static void FillCollectionFromEntitySet<T>(List<T> list, Data.IEntityDataSet set) =>
			FillCollectionFromEntitySet(list, set.Set, set.Type);

		public static void FillCollectionFromEntitySet<T, TEntity>(List<T> list, IEnumerable<TEntity> set) =>
			FillCollectionFromEntitySet(list, set, typeof(TEntity));

		public static void FillCollectionFromEntitySet<T>(List<T> list, IEnumerable set, Type tentity)
		{
			try
			{
				Type type = typeof(T);

				if (type == tentity)
				{
					list.AddRange(set.OfType<T>());
				}
				else
				{

					PropertyInfo[] props = GetTypeProperties(type);
					PropertyInfo[] eprops = GetTypeProperties(tentity);
					var pairs = props.Join(eprops, p => p.Name, ep => ep.Name, (p, ep) => new { Property = p, EntityProperty = ep }, StringComparer.OrdinalIgnoreCase)
						.ToArray();

					foreach (object entity in set)
					{
						// create an instance
						T obj = (T)Activator.CreateInstance(type);
						list.Add(obj);

						// fill properties
						foreach (var prop in pairs)
						{

							object propVal = prop.EntityProperty.GetValue(entity, null);
							if (propVal == DBNull.Value || propVal == null)
								prop.Property.SetValue(obj, GetNull(prop.Property.PropertyType), null);
							else
							{
								try
								{
									// try implicit type conversion
									prop.Property.SetValue(obj, propVal, null);
								}
								catch
								{
									// convert to string and then set property value
									try
									{
										string strVal = propVal.ToString();
										prop.Property.SetValue(obj, Cast(strVal, prop.Property.PropertyType), null);
									}
									catch
									{
										// skip property init
									}
								}
							}
						} // for properties
					} // for rows
				}
			}
			finally
			{
				if (set is IDisposable disposable) disposable.Dispose();
			}
		}

		public static List<T> CreateListFromDataReader<T>(IDataReader reader)
		{
			List<T> list = new List<T>();
			FillCollectionFromDataReader<T>(list, reader);
			return list;
		}

		public static List<T> CreateListFromDataSet<T>(DataSet ds)
		{
			List<T> list = new List<T>();
			FillCollectionFromDataSet<T>(list, ds);
			return list;
		}

		public static List<T> CreateListFromEntitySet<T, TEntity>(IEnumerable<TEntity> set)
		{
			List<T> list = new List<T>();
			FillCollectionFromEntitySet<T, TEntity>(list, set);
			return list;
		}

		public static void FillCollectionFromDataReader<T>(List<T> list, IDataReader reader)
		{
			if (reader is Data.IEntityDataSet entitySet)
			{
				FillCollectionFromEntitySet(list, entitySet);
			}
			else
			{

				Type type = typeof(T);

				try
				{
					// get type properties
					PropertyInfo[] props = GetTypeProperties(type);

					// leave only a property from the DataReader
					DataTable readerSchema = reader.GetSchemaTable();
					if (readerSchema != null)
					{
						List<PropertyInfo> propslist = new List<PropertyInfo>();
						foreach (DataRow field in readerSchema.Rows)
						{
							string columnName = System.Convert.ToString(field["ColumnName"]);

							foreach (PropertyInfo prop in props)
								if (columnName.ToLower() == prop.Name.ToLower())
									propslist.Add(prop);
						}
						props = propslist.ToArray();
					}

					// iterate through reader
					while (reader.Read())
					{
						T obj = (T)Activator.CreateInstance(type);
						list.Add(obj);

						// set properties
						for (int i = 0; i < props.Length; i++)
						{
							string propName = props[i].Name;

							try
							{

								object propVal = reader[propName];
								if (propVal == DBNull.Value || propVal == null)
									props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
								else
								{
									try
									{
										// try implicit type conversion
										props[i].SetValue(obj, propVal, null);
									}
									catch
									{
										// convert to string and then set property value
										try
										{
											string strVal = propVal.ToString();
											props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
										}
										catch { }
									}
								}
							}
							catch { } // just skip
						} // for properties
					}
				}
				finally
				{
					reader.Close();
				}
			}
		}

		public static T FillObjectFromDataView<T>(DataView dv)
		{
			Type type = typeof(T);
			T obj = default(T);

			// get type properties
			PropertyInfo[] props = GetTypeProperties(type);

			// iterate through reader
			foreach (DataRowView dr in dv)
			{
				obj = (T)Activator.CreateInstance(type);

				// set properties
				for (int i = 0; i < props.Length; i++)
				{
					string propName = props[i].Name;

					try
					{
						// verify if there is such a column
						if (!dr.Row.Table.Columns.Contains(propName.ToLower()))
						{
							// if not, we move to another property
							// because this one we cannot set
							continue;
						}

						object propVal = dr[propName];
						if (propVal == DBNull.Value || propVal == null)
							props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
						else
						{
							try
							{
								string strVal = propVal.ToString();

								//convert to DateTime 
								if (props[i].PropertyType.UnderlyingSystemType.FullName == typeof(DateTime).FullName)
								{
									DateTime date = DateTime.MinValue;
									if (DateTime.TryParse(strVal, out date))
									{
										props[i].SetValue(obj, date, null);
									}
								}
								else
								{
									//Convert generic
									props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
								}
							}
							catch
							{
								// skip property init 
							}
						}
					}
					catch { } // just skip
				} // for properties
			}

			return obj;
		}

		public static T FillObjectFromEntity<T, TEntity>(TEntity entity) where T : class =>
			FillObjectFromEntity<T>(entity, typeof(TEntity));
		public static T FillObjectFromEntity<T>(object entity, Type etype) where T : class
		{
			if (entity == null) return null;

			Type type = typeof(T);

			if (type == etype) return entity as T;

			T obj = default(T);

			// get type properties
			PropertyInfo[] props = GetTypeProperties(type);
			PropertyInfo[] eprops = GetTypeProperties(etype);
			var pairs = props.Join(eprops, p => p.Name, ep => ep.Name, (p, ep) => new { Property = p, EntityProperty = ep }, StringComparer.OrdinalIgnoreCase)
				.ToArray();

			obj = (T)Activator.CreateInstance(type);

			// set properties
			foreach (var prop in pairs)
			{

				try
				{
					object propVal = prop.EntityProperty.GetValue(entity);
					if (propVal == DBNull.Value || propVal == null)
						prop.Property.SetValue(obj, GetNull(prop.Property.PropertyType), null);
					else
					{
						try
						{
							string strVal = propVal.ToString();

							//convert to DateTime 
							if (prop.Property.PropertyType.UnderlyingSystemType.FullName == typeof(DateTime).FullName)
							{
								DateTime date = DateTime.MinValue;
								if (DateTime.TryParse(strVal, out date))
								{
									prop.Property.SetValue(obj, date, null);
								}
							}
							else
							{
								//Convert generic
								prop.Property.SetValue(obj, Cast(strVal, prop.Property.PropertyType), null);
							}
						}
						catch
						{
							// skip property init 
						}
					}
				}
				catch { } // just skip
			} // for properties

			return obj;
		}

		public static DataTable DataTableFromEntitySet<TEntity>(IEnumerable<TEntity> set)
		{
			var type = typeof(TEntity);
			if (type == typeof(object)) type = set.FirstOrDefault(e => e != null)?.GetType() ?? typeof(object);
			var table = new DataTable(type.Name);
			var eprops = GetTypeProperties(type)
				// Filter out navigation & hidden properties
				.Where(p => (!p.PropertyType.IsClass || p.PropertyType == typeof(string) ||
					p.PropertyType == typeof(Guid) || p.PropertyType == typeof(byte[])) &&
					!p.PropertyType.IsInterface &&
					(p.CanWrite || IsAnonymous(type)) && p.CanRead &&
					p.GetCustomAttribute<NotMappedAttribute>() == null);
			var props = eprops.Select(p =>
			{
				var ptype = p.PropertyType;
				var isString = ptype == typeof(string);
				var isNullable = ptype.IsGenericType && ptype.GetGenericTypeDefinition() == typeof(Nullable<>) &&
						!ptype.IsGenericTypeDefinition;
				var column = new DataColumn(p.Name);
				column.AllowDBNull = isNullable || isString;
				var ctype = isNullable ? Nullable.GetUnderlyingType(ptype) : ptype;
				column.DataType = ctype;

				return new { Property = p, Column = column, Type = ctype, IsNullable = isNullable,
					IsString = isString };
			})
			.ToArray();

			foreach (var p in props) table.Columns.Add(p.Column);

			foreach (var entity in set)
			{
				var row = table.NewRow();
				foreach (var prop in props)
				{
					var val = prop.Property.GetValue(entity);
					if (prop.IsNullable || prop.IsString)
					{
						if (val == null) val = DBNull.Value;
						else if (prop.IsNullable)
						{
							// get val.Value property value
							var valueProperty = prop.Property.PropertyType.GetProperty("Value");
							val = valueProperty.GetValue(val);
						}
					}
					row[prop.Column] = val;
				}
				table.Rows.Add(row);
			}
			return table;
		}

		public static DataSet DataSetFromEntitySet<TEntity>(IEnumerable<TEntity> set)
		{
			var type = typeof(TEntity);
			DataSet dataSet = new DataSet(type.Name);
			dataSet.Tables.Add(DataTableFromEntitySet(set));
			return dataSet;
		}

		public static T FillObjectFromEntitySet<T, TEntity>(IEnumerable<TEntity> set) where T : class => FillObjectFromEntity<T, TEntity>(set.LastOrDefault());
		public static T FillObjectFromEntitySet<T>(IEnumerable set, Type etype) where T : class => FillObjectFromEntity<T>(set.OfType<object>().LastOrDefault(), etype);
		public static T FillObjectFromEntitySet<T>(Data.IEntityDataSet set) where T : class => FillObjectFromEntitySet<T>(set.Set, set.Type);

		public static T FillObjectFromDataReader<T>(IDataReader reader) where T: class
		{
			if (reader is Data.IEntityDataSet entitySet) return FillObjectFromEntitySet<T>(entitySet);
			
			Type type = typeof(T);

			T obj = default(T);

			try
			{
				// get type properties
				PropertyInfo[] props = GetTypeProperties(type);

				var columns = reader.GetSchemaTable()
					.Rows
					.OfType<DataRow>()
					.Select(row => (string)row[0])
					.ToHashSet(StringComparer.OrdinalIgnoreCase);

				// iterate through reader
				while (reader.Read())
				{
					obj = (T)Activator.CreateInstance(type);

					// set properties
					for (int i = 0; i < props.Length; i++)
					{
						string propName = props[i].Name;

						try
						{
							if (!columns.Contains(propName)) // !IsColumnExists(propName, reader.GetSchemaTable()))
							{
								continue;
							}
							object propVal = reader[propName];

							if (propVal == DBNull.Value || propVal == null)
								props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
							else
							{
								try
								{
									//try string first
									if (props[i].PropertyType.UnderlyingSystemType.FullName == typeof(String).FullName)
									{
										props[i].SetValue(obj, propVal.ToString(), null);
									}
									else
									{
										// then, try implicit type conversion
										props[i].SetValue(obj, propVal, null);
									}
								}
								catch
								{
									// convert to string and then set property value
									try
									{
										string strVal = propVal.ToString();
										props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
									}
									catch
									{
										// skip property init
									}
								}
							}
						}
						catch { } // just skip
					} // for properties
				}
			}
			finally
			{
				reader.Close();
			}

			return obj;
		}

		private static Hashtable propertiesCache = new Hashtable();

		public static object CreateObjectFromDataTable(Type type, DataTable table,
			string nameColumn, string valueColumn, bool persistentOnly)
		{
			if (table is Data.IEntityDataSet entitySet) return CreateObjectFromEntitySet(type, entitySet, nameColumn, valueColumn, persistentOnly);
			else return CreateObjectFromDataView(type, table.DefaultView, nameColumn, valueColumn, persistentOnly);
		}

		public static object CreateObjectFromDataView(Type type, DataView dv,
			string nameColumn, string valueColumn, bool persistentOnly)
		{
			// create hash of properties from datareader
			Hashtable propValues = new Hashtable();
			foreach (DataRowView dr in dv)
			{
				if (propValues[dr[nameColumn]] == null && !propValues.ContainsKey(dr[nameColumn]))
					propValues.Add(dr[nameColumn], dr[valueColumn]);
			}

			return CreateObjectFromHash(type, propValues, persistentOnly);
		}

		public static object CreateObjectFromDataReader(Type type, IDataReader reader,
			string nameColumn, string valueColumn, bool persistentOnly)
		{
			if (reader is Data.IEntityDataSet entitySet) return CreateObjectFromEntitySet(type, entitySet, nameColumn, valueColumn, persistentOnly);

			// create hash of properties from datareader
			Hashtable propValues = new Hashtable();
			try
			{
				while (reader.Read())
				{
					if (propValues[reader[nameColumn]] == null && !propValues.ContainsKey(reader[nameColumn]))
						propValues.Add(reader[nameColumn], reader[valueColumn]);
				}
			}
			finally
			{
				reader.Close();
			}
			return CreateObjectFromHash(type, propValues, persistentOnly);
		}

		public static object CreateObjectFromHash(Type type, Hashtable propValues, bool persistentOnly)
		{
			// create object
			object obj = Activator.CreateInstance(type);

			CreateObjectFromHash(obj, propValues, persistentOnly);

			return obj;
		}

		public static object CreateObjectFromEntitySet<TEntity>(Type type, IEnumerable<TEntity> set,
			string nameColumn, string valueColumn, bool persistentOnly) =>
			CreateObjectFromEntitySet(type, set, typeof(TEntity), nameColumn, valueColumn, persistentOnly);

		public static object CreateObjectFromEntitySet(Type type, Data.IEntityDataSet set,
			string nameColumn, string valueColumn, bool persistentOnly) =>
			CreateObjectFromEntitySet(type, set.Set, set.Type, nameColumn, valueColumn, persistentOnly);
		
		public static object CreateObjectFromEntitySet(Type type, IEnumerable set, Type etype,
			string nameColumn, string valueColumn, bool persistentOnly)
		{
			var eprops = GetTypeProperties(etype);
			var nameProp = eprops.FirstOrDefault(p => string.Equals(p.Name, nameColumn, StringComparison.OrdinalIgnoreCase));
			var valueProp = eprops.FirstOrDefault(p => string.Equals(p.Name, valueColumn, StringComparison.OrdinalIgnoreCase));

			if (nameProp == null || valueProp == null) throw new NotSupportedException($"Property {nameColumn} or {valueColumn} not found in type {etype.Name}");

			// create hash of properties from datareader
			Hashtable propValues = new Hashtable();
			foreach (var entity in set)
			{
				var propName = nameProp.GetValue(entity) as string;

				if (propValues[propName] == null && !propValues.ContainsKey(propName))
				{
					var propValue = valueProp.GetValue(entity);
					propValues.Add(propName, propValue);
				}
			}

			return CreateObjectFromHash(type, propValues, persistentOnly);
		}

		public static void CopyPersistentPropertiesFromSource<T>(T source, T target)
			where T : ServiceProviderItem
		{
			//
			var typeSource = source.GetType();
			var typeTarget = target.GetType();
			// get all property infos
			Hashtable props = null;
			if (propertiesCache[typeSource.Name] != null)
			{
				// load properties from cache
				props = (Hashtable)propertiesCache[typeSource.Name];
			}
			else
			{
				// create properties cache
				props = new Hashtable();
				//
				PropertyInfo[] objProps = typeSource.GetProperties(BindingFlags.Instance
					//| BindingFlags.DeclaredOnly
					| BindingFlags.Public);
				foreach (PropertyInfo prop in objProps)
				{
					// check for persistent attribute
					object[] attrs = prop.GetCustomAttributes(typeof(PersistentAttribute), false);
					// Persistent only
					if (attrs.Length > 0 && !props.ContainsKey(prop.Name))
					{
						// add property to hash
						props.Add(prop.Name, prop);
					}
				}

				if (!propertiesCache.ContainsKey(typeSource.Name))
				{
					// add to cache
					propertiesCache.Add(typeSource.Name, props);
				}
			}

			// Copy the data
			foreach (PropertyInfo propertyInfo in props.Values)
			{
				propertyInfo.SetValue(target, propertyInfo.GetValue(source, null), null);
			}
		}

		private static Hashtable GetPropertiesForCache(Type type, bool persistentOnly)
		{
			// create properties cache
			var props = new Hashtable();
			PropertyInfo[] objProps = type.GetProperties(BindingFlags.Instance
				//| BindingFlags.DeclaredOnly
				| BindingFlags.Public);
			foreach (PropertyInfo prop in objProps)
			{
				// check for persistent attribute
				object[] attrs = prop.GetCustomAttributes(typeof(PersistentAttribute), false);
				if (!persistentOnly || (persistentOnly && attrs.Length > 0) && !props.ContainsKey(prop.Name))
				{
					// when property is custom class with Persistent attribute
					if (prop.PropertyType.GetCustomAttributes(typeof(PersistentAttribute), false).Length > 0)
					{
						// add sub-class properties to hash
						var childHash = GetPropertiesForCache(prop.PropertyType, persistentOnly);
						foreach (var hashKey in childHash.Keys)
						{
							var value = childHash[hashKey];
							props.Add(prop.Name + "." + hashKey, value);
						}
						// exit
						continue;
					}

					// add property to hash
					props.Add(prop.Name, prop);
				}
			}

			return props;
		}

		public static void CreateObjectFromHash(object obj, Hashtable propValues, bool persistentOnly)
		{
			Type type = obj.GetType();

			// get all property infos
			Hashtable props = null;
			if (propertiesCache[type.Name] != null)
			{
				// load properties from cache
				props = (Hashtable)propertiesCache[type.Name];
			}
			else
			{
				props = GetPropertiesForCache(type, persistentOnly);

				if (!propertiesCache.ContainsKey(type.Name))
				{
					// add to cache
					propertiesCache.Add(type.Name, props);
				}
			}

			// fill properties
			foreach (string propName in propValues.Keys)
			{
				// try to locate specified property
				if (props[propName] != null)
				{
					PropertyInfo prop = (PropertyInfo)props[propName];
					string val = propValues[propName].ToString();
					var currentObj = obj;

					// when property is custom class with Persistent attribute
					if (propName.Contains("."))
					{
						var mainPropertyName = propName.Split('.')[0];
						var childPropertyName = propName.Split('.')[1];

						var mainProperty = type.GetProperty(mainPropertyName);
						if (mainProperty == null) continue;

						var mainVal = mainProperty.GetValue(obj, null);
						if (mainVal == null)
						{
							mainVal = Activator.CreateInstance(mainProperty.PropertyType);
							mainProperty.SetValue(obj, mainVal, null);
						}
						currentObj = mainVal;

						var childProperty = mainProperty.PropertyType.GetProperty(childPropertyName);
						if (childProperty == null) continue;
						prop = childProperty;
					}

					// set property
					// we support:
					//	String
					//	Int32
					//	Boolean
					//	Float

					if (prop.PropertyType == typeof(String))
						prop.SetValue(currentObj, val, null);
					else if (prop.PropertyType == typeof(Int32))
						prop.SetValue(currentObj, Int32.Parse(val), null);
					else
						if (prop.PropertyType == typeof(long))
						prop.SetValue(currentObj, long.Parse(val), null);
					else
							if (prop.PropertyType == typeof(Boolean))
						prop.SetValue(currentObj, Boolean.Parse(val), null);
					else if (prop.PropertyType == typeof(Single))
						prop.SetValue(currentObj, Single.Parse(val), null);
					else if (prop.PropertyType.IsEnum)
						prop.SetValue(currentObj, Enum.Parse(prop.PropertyType, val, true), null);
					else
						if (prop.PropertyType == typeof(Guid))
						prop.SetValue(currentObj, new Guid(val), null);
					else
							if (prop.PropertyType == typeof(string[]))
					{
						if (val == "")
							prop.SetValue(currentObj, new string[0], null);
						else
							prop.SetValue(currentObj, val.Split(';'), null);
					}
					else if (prop.PropertyType == typeof(int[]))
					{
						string[] svals = val.Split(';');
						int[] ivals = new int[svals.Length];

						for (int i = 0; i < svals.Length; i++)
							ivals[i] = Int32.Parse(svals[i]);

						if (val == "")
							ivals = new int[0];

						prop.SetValue(currentObj, ivals, null);
					}
				}
			}
		}

		private static Dictionary<string, PropertyInfo> GetTypePropertiesHash(Type type)
		{
			Dictionary<string, PropertyInfo> hash = new Dictionary<string, PropertyInfo>();
			PropertyInfo[] props = GetTypeProperties(type);
			foreach (PropertyInfo prop in props)
			{
				if (!hash.ContainsKey(prop.Name))
				{
					hash.Add(prop.Name, prop);
				}
			}
			return hash;
		}

		public static object GetNull(Type type)
		{
			if (type == typeof(string))
				return null;
			if (type == typeof(Int32))
				return 0;
			if (type == typeof(Int64))
				return 0;
			if (type == typeof(Boolean))
				return false;
			if (type == typeof(Decimal))
				return 0M;
			if (type == typeof(DateTime))
				return default(DateTime);
			if (type == typeof(TimeSpan))
				return TimeSpan.Zero;
			else
				return null;
		}

		public static object Cast(string val, Type type)
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
			if (type == typeof(string[]) && val != null)
			{
				return val.Split(';');
			}
			if (type.IsEnum)
				return Enum.Parse(type, val, true);

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

		public static string GetTypeFullName(Type type)
		{
			return type.FullName + ", " + type.Assembly.GetName().Name;
		}

		public static TResult Deserialize<TResult>(string inputString)
		{
			TResult result;

			var serializer = new XmlSerializer(typeof(TResult));

			using (TextReader reader = new StringReader(inputString))
			{
				result = (TResult)serializer.Deserialize(reader);
			}

			return result;
		}

		public static string Serialize<TEntity>(TEntity entity)
		{
			string result = string.Empty;

			var xmlSerializer = new XmlSerializer(typeof(TEntity));

			using (var stringWriter = new StringWriter())
			{
				using (XmlWriter writer = XmlWriter.Create(stringWriter))
				{
					xmlSerializer.Serialize(writer, entity);
					result = stringWriter.ToString();
				}
			}

			return result;
		}

		public static PropertyInfo GetProperty<T, U>(T obj, Expression<Func<T, U>> expression)
		{
			var member = expression.Body as MemberExpression;

			if (member == null || member.Member is PropertyInfo == false)
				throw new ArgumentException("Expression is not a Property", "expression");

			return (PropertyInfo)member.Member;
		}

		#region Helper Functions

		/// <summary>
		/// This function is used to determine whether IDataReader contains a Column.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="schemaTable">The schema <see cref="DataTable"/> that decribes result-set <see cref="IDataReader"/> contains.</param>
		/// <returns>True, when required column exists in the <paramref name="schemaTable"/>. Otherwise, false.</returns>
		/// <remark>
		/// The followin example shows how to look for the "Role" column in the <see cref="IDataReader"/>.
		/// <example>
		/// IDataReader reader = ....
		/// if (!IsColumnExists("Role", reader.GetSchemaTable())
		/// {
		///		continue;
		/// }
		/// 
		/// object roleValue = reader["Role"];
		/// </example>
		/// </remark>
		static bool IsColumnExists(string columnName, DataTable schemaTable)
		{
			foreach (DataRow row in schemaTable.Rows)
			{
				if (String.Compare(row[0].ToString(), columnName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}

			return false;
		}

		#endregion

		/// <summary>
		/// Perform a deep Copy of the object.
		/// </summary>
		/// <typeparam name="T">The type of object being copied.</typeparam>
		/// <param name="source">The object instance to copy.</param>
		/// <returns>The copied object.</returns>
		public static T Clone<T>(T source)
		{
			/*if (!typeof (T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable: " + typeof(T), "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }*/
			return (T)Web.Clients.DataContractCopier.Clone(source);
		}
	}
}

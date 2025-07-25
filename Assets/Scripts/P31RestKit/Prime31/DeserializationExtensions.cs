using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Prime31
{
	public static class DeserializationExtensions
	{
		public static List<T> toList<T>(this ArrayList self)
		{
			List<T> list = new List<T>();
			foreach (Hashtable item in self)
			{
				//list.Add(item.toClass<T>());
                list.Add(toClass<T>(item));
            }
			return list;
		}

		public static T toClass<T>(this Hashtable self)
		{
			object obj = Activator.CreateInstance(typeof(T));
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true);
				foreach (object obj2 in customAttributes)
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj2 as P31DeserializeableFieldAttribute;
					if (!self.ContainsKey(p31DeserializeableFieldAttribute.key))
					{
						continue;
					}
					object obj3 = self[p31DeserializeableFieldAttribute.key];
					if (obj3 is Hashtable)
					{
						MethodInfo methodInfo = typeof(DeserializationExtensions).GetMethod("toClass").MakeGenericMethod(p31DeserializeableFieldAttribute.type);
						object value = methodInfo.Invoke(null, new object[1] { obj3 });
						fieldInfo.SetValue(obj, value);
						self.Remove(p31DeserializeableFieldAttribute.key);
					}
					else if (obj3 is ArrayList)
					{
						if (!p31DeserializeableFieldAttribute.isCollection)
						{
							Debug.LogError("found an ArrayList but the field is not a collection: " + p31DeserializeableFieldAttribute.key);
							continue;
						}
						MethodInfo methodInfo2 = typeof(DeserializationExtensions).GetMethod("toList").MakeGenericMethod(p31DeserializeableFieldAttribute.type);
						object value2 = methodInfo2.Invoke(null, new object[1] { obj3 });
						fieldInfo.SetValue(obj, value2);
						self.Remove(p31DeserializeableFieldAttribute.key);
					}
					else
					{
						fieldInfo.SetValue(obj, Convert.ChangeType(obj3, fieldInfo.FieldType));
						self.Remove(p31DeserializeableFieldAttribute.key);
					}
				}
			}
			return (T)obj;
		}

		public static Hashtable toHashtable(this object self)
		{
			Hashtable hashtable = new Hashtable();
			FieldInfo[] fields = self.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(P31DeserializeableFieldAttribute), true);
				foreach (object obj in customAttributes)
				{
					P31DeserializeableFieldAttribute p31DeserializeableFieldAttribute = obj as P31DeserializeableFieldAttribute;
					if (p31DeserializeableFieldAttribute.isCollection)
					{
						IEnumerable enumerable = fieldInfo.GetValue(self) as IEnumerable;
						ArrayList arrayList = new ArrayList();
						foreach (object item in enumerable)
						{
							//arrayList.Add(item.toHashtable());
                            arrayList.Add(toHashtable(item));
                        }
						hashtable[p31DeserializeableFieldAttribute.key] = arrayList;
					}
					else if (p31DeserializeableFieldAttribute.type != null)
					{
						//hashtable[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self).toHashtable();
                        hashtable[p31DeserializeableFieldAttribute.key] = Prime31.DeserializationExtensions.toHashtable(fieldInfo.GetValue(self));
                    }
					else
					{
						hashtable[p31DeserializeableFieldAttribute.key] = fieldInfo.GetValue(self);
					}
				}
			}
			return hashtable;
		}
	}
}

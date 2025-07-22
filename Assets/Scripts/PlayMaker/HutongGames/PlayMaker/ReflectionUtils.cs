using System;
using System.Collections.Generic;
using System.Reflection;

namespace HutongGames.PlayMaker
{
	public static class ReflectionUtils
	{
		public static bool CanReadMemberValue(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return true;
			case MemberTypes.Property:
				return ((PropertyInfo)member).CanRead;
			default:
				return false;
			}
		}

		public static bool CanSetMemberValue(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return true;
			case MemberTypes.Property:
				return ((PropertyInfo)member).CanWrite;
			default:
				return false;
			}
		}

		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).FieldType;
			case MemberTypes.Property:
				return ((PropertyInfo)member).PropertyType;
			case MemberTypes.Event:
				return ((EventInfo)member).EventHandlerType;
			default:
				throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", "member");
			}
		}

		public static object GetMemberValue(MemberInfo member, object target)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).GetValue(target);
			case MemberTypes.Property:
				try
				{
					return ((PropertyInfo)member).GetValue(target, null);
				}
				catch (TargetParameterCountException innerException)
				{
					throw new ArgumentException("MemberInfo has index parameters", "member", innerException);
				}
			default:
				throw new ArgumentException("MemberInfo is not of type FieldInfo or PropertyInfo", "member");
			}
		}

		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				((FieldInfo)member).SetValue(target, value);
				break;
			case MemberTypes.Property:
				((PropertyInfo)member).SetValue(target, value, null);
				break;
			default:
				throw new ArgumentException("MemberInfo must be if type FieldInfo or PropertyInfo", "member");
			}
		}

		public static List<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
		{
			return GetFieldsAndProperties(typeof(T), bindingAttr);
		}

		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(type.GetFields(bindingAttr));
			list.AddRange(type.GetProperties(bindingAttr));
			return list;
		}
	}
}

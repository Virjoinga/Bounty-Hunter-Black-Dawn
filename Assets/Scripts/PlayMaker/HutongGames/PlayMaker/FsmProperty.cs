using System;
using System.Reflection;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmProperty
	{
		public FsmObject TargetObject = new FsmObject();

		public string TargetTypeName = "";

		public Type TargetType;

		public string PropertyName = "";

		public Type PropertyType;

		public FsmBool BoolParameter;

		public FsmFloat FloatParameter;

		public FsmInt IntParameter;

		public FsmGameObject GameObjectParameter;

		public FsmString StringParameter;

		public FsmVector2 Vector2Parameter;

		public FsmVector3 Vector3Parameter;

		public FsmRect RectParamater;

		public FsmQuaternion QuaternionParameter;

		public FsmObject ObjectParameter;

		public FsmMaterial MaterialParameter;

		public FsmTexture TextureParameter;

		public FsmColor ColorParameter;

		public bool setProperty;

		private bool initialized;

		[NonSerialized]
		private UnityEngine.Object targetObjectCached;

		private MemberInfo memberInfo;

		public FsmProperty()
		{
			ResetParameters();
		}

		public FsmProperty(FsmProperty source)
		{
			setProperty = source.setProperty;
			TargetObject = new FsmObject(source.TargetObject);
			TargetTypeName = source.TargetTypeName;
			TargetType = source.TargetType;
			PropertyName = source.PropertyName;
			PropertyType = source.PropertyType;
			BoolParameter = new FsmBool(source.BoolParameter);
			FloatParameter = new FsmFloat(source.FloatParameter);
			IntParameter = new FsmInt(source.IntParameter);
			GameObjectParameter = new FsmGameObject(source.GameObjectParameter);
			StringParameter = new FsmString(source.StringParameter);
			Vector2Parameter = new FsmVector2(source.Vector2Parameter);
			Vector3Parameter = new FsmVector3(source.Vector3Parameter);
			RectParamater = new FsmRect(source.RectParamater);
			QuaternionParameter = new FsmQuaternion(source.QuaternionParameter);
			ObjectParameter = new FsmObject(source.ObjectParameter);
			MaterialParameter = new FsmMaterial(source.MaterialParameter);
			TextureParameter = new FsmTexture(source.TextureParameter);
			ColorParameter = new FsmColor(source.ColorParameter);
		}

		public void SetPropertyName(string propertyName)
		{
			ResetParameters();
			PropertyName = propertyName;
			if (!string.IsNullOrEmpty(PropertyName))
			{
				if (TargetType != null)
				{
					PropertyInfo property = TargetType.GetProperty(PropertyName);
					PropertyType = ((property != null) ? property.PropertyType : null);
					if (TargetType.IsSubclassOf(typeof(UnityEngine.Object)) && PropertyType != null)
					{
						ObjectParameter.ObjectType = PropertyType;
					}
				}
			}
			else
			{
				PropertyType = null;
			}
			Init();
		}

		public void SetValue()
		{
			if (!initialized)
			{
				Init();
			}
			if (!(targetObjectCached == null) && memberInfo != null && ReflectionUtils.CanSetMemberValue(memberInfo))
			{
				Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
				if (memberUnderlyingType.IsAssignableFrom(typeof(bool)) && !BoolParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, BoolParameter.Value);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(int)) && !IntParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, IntParameter.Value);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(float)) && !FloatParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, FloatParameter.Value);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(string)) && !StringParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, StringParameter.Value);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Vector2)) && !Vector2Parameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, Vector2Parameter.Value);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Vector3)) && !Vector3Parameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, Vector3Parameter.Value);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Rect)) && !RectParamater.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, RectParamater.Value);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Quaternion)) && !QuaternionParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, QuaternionParameter.Value);
				}
				else if (memberUnderlyingType == typeof(GameObject) && !GameObjectParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, GameObjectParameter.Value);
				}
				else if (memberUnderlyingType == typeof(Material) && !MaterialParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, MaterialParameter.Value);
				}
				else if (memberUnderlyingType == typeof(Texture) && !TextureParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, TextureParameter.Value);
				}
				else if (memberUnderlyingType == typeof(Color) && !ColorParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, ColorParameter.Value);
				}
				else if (memberUnderlyingType.IsSubclassOf(typeof(UnityEngine.Object)) && !ObjectParameter.IsNone)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, ObjectParameter.Value);
				}
			}
		}

		public void GetValue()
		{
			if (!initialized)
			{
				Init();
			}
			if (!(targetObjectCached == null) && memberInfo != null && ReflectionUtils.CanReadMemberValue(memberInfo))
			{
				Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
				if (memberUnderlyingType.IsAssignableFrom(typeof(bool)))
				{
					BoolParameter.Value = (bool)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(int)))
				{
					IntParameter.Value = (int)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(float)))
				{
					FloatParameter.Value = (float)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(string)))
				{
					StringParameter.Value = (string)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Vector2)))
				{
					Vector2Parameter.Value = (Vector2)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Vector3)))
				{
					Vector3Parameter.Value = (Vector3)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Rect)))
				{
					RectParamater.Value = (Rect)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsAssignableFrom(typeof(Quaternion)))
				{
					QuaternionParameter.Value = (Quaternion)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType == typeof(GameObject))
				{
					GameObjectParameter.Value = (GameObject)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType == typeof(Material))
				{
					MaterialParameter.Value = (Material)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType == typeof(Texture))
				{
					TextureParameter.Value = (Texture)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType == typeof(Color))
				{
					ColorParameter.Value = (Color)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
				else if (memberUnderlyingType.IsSubclassOf(typeof(UnityEngine.Object)))
				{
					ObjectParameter.Value = (UnityEngine.Object)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				}
			}
		}

		public void Init()
		{
			initialized = true;
			targetObjectCached = TargetObject.Value;
			if (TargetObject.UseVariable)
			{
				TargetTypeName = TargetObject.TypeName;
				TargetType = TargetObject.ObjectType;
			}
			else if (TargetObject.Value != null)
			{
				TargetType = TargetObject.Value.GetType();
				TargetTypeName = TargetType.FullName;
			}
			if (!string.IsNullOrEmpty(PropertyName))
			{
				memberInfo = (MemberInfo)(((object)TargetType.GetProperty(PropertyName)) ?? ((object)TargetType.GetField(PropertyName)));
				if (memberInfo == null)
				{
					PropertyName = "";
					PropertyType = null;
					ResetParameters();
				}
				else
				{
					PropertyType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
				}
			}
		}

		public void CheckForReinitialize()
		{
			if (!initialized || targetObjectCached != TargetObject.Value || (TargetObject.UseVariable && TargetType != TargetObject.ObjectType))
			{
				Init();
			}
		}

		public void ResetParameters()
		{
			BoolParameter = false;
			FloatParameter = 0f;
			IntParameter = 0;
			StringParameter = "";
			GameObjectParameter = new FsmGameObject("");
			Vector2Parameter = new FsmVector2();
			Vector3Parameter = new FsmVector3();
			RectParamater = new FsmRect();
			QuaternionParameter = new FsmQuaternion();
			ObjectParameter = new FsmObject();
			MaterialParameter = new FsmMaterial();
			TextureParameter = new FsmTexture();
			ColorParameter = new FsmColor();
		}
	}
}

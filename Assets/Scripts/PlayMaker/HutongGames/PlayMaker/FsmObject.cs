using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmObject : NamedVariable
	{
		[SerializeField]
		private string typeName;

		[SerializeField]
		private UnityEngine.Object value;

		private Type objectType;

		public Type ObjectType
		{
			get
			{
				if (objectType == null)
				{
					if (string.IsNullOrEmpty(typeName))
					{
						typeName = typeof(UnityEngine.Object).FullName;
					}
					objectType = FsmUtility.GetType(typeName);
				}
				return objectType;
			}
			set
			{
				objectType = value ?? typeof(UnityEngine.Object);
				if (this.value != null)
				{
					Type type = this.value.GetType();
					if (!type.IsAssignableFrom(objectType) && !type.IsSubclassOf(objectType))
					{
						this.value = null;
					}
				}
				typeName = objectType.FullName;
			}
		}

		public string TypeName
		{
			get
			{
				return typeName;
			}
		}

		public UnityEngine.Object Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}

		public FsmObject()
		{
		}

		public FsmObject(string name)
			: base(name)
		{
			typeName = typeof(UnityEngine.Object).FullName;
			objectType = typeof(UnityEngine.Object);
		}

		public FsmObject(FsmObject source)
			: base(source)
		{
			value = source.value;
			typeName = source.typeName;
			objectType = source.objectType;
		}

		public override string ToString()
		{
			if (!(value == null))
			{
				return value.ToString();
			}
			return "None";
		}

		public static implicit operator FsmObject(UnityEngine.Object value)
		{
			FsmObject fsmObject = new FsmObject();
			fsmObject.value = value;
			return fsmObject;
		}
	}
}

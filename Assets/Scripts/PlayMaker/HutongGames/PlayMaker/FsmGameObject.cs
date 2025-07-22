using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmGameObject : NamedVariable
	{
		[SerializeField]
		private GameObject value;

		public GameObject Value
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

		public FsmGameObject()
		{
		}

		public FsmGameObject(string name)
			: base(name)
		{
		}

		public FsmGameObject(FsmGameObject source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override string ToString()
		{
			if (!(value == null))
			{
				return value.name;
			}
			return "None";
		}

		public static implicit operator FsmGameObject(GameObject value)
		{
			FsmGameObject fsmGameObject = new FsmGameObject(string.Empty);
			fsmGameObject.value = value;
			return fsmGameObject;
		}
	}
}

using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmQuaternion : NamedVariable
	{
		[SerializeField]
		private Quaternion value;

		public Quaternion Value
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

		public FsmQuaternion()
		{
		}

		public FsmQuaternion(string name)
			: base(name)
		{
		}

		public FsmQuaternion(FsmQuaternion source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator FsmQuaternion(Quaternion value)
		{
			FsmQuaternion fsmQuaternion = new FsmQuaternion(string.Empty);
			fsmQuaternion.value = value;
			return fsmQuaternion;
		}
	}
}

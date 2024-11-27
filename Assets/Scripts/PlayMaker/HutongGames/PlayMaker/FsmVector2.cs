using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmVector2 : NamedVariable
	{
		[SerializeField]
		private Vector2 value;

		public Vector2 Value
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

		public FsmVector2()
		{
		}

		public FsmVector2(string name)
			: base(name)
		{
		}

		public FsmVector2(FsmVector2 source)
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

		public static implicit operator FsmVector2(Vector2 value)
		{
			FsmVector2 fsmVector = new FsmVector2(string.Empty);
			fsmVector.value = value;
			return fsmVector;
		}
	}
}

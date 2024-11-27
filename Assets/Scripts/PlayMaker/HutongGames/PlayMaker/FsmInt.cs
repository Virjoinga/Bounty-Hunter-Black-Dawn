using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmInt : NamedVariable
	{
		[SerializeField]
		private int value;

		public int Value
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

		public FsmInt()
		{
		}

		public FsmInt(string name)
			: base(name)
		{
		}

		public FsmInt(FsmInt source)
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

		public static implicit operator FsmInt(int value)
		{
			FsmInt fsmInt = new FsmInt(string.Empty);
			fsmInt.value = value;
			return fsmInt;
		}
	}
}

using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmBool : NamedVariable
	{
		[SerializeField]
		private bool value;

		public bool Value
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

		public FsmBool()
		{
		}

		public FsmBool(string name)
			: base(name)
		{
		}

		public FsmBool(FsmBool toCopy)
			: base(toCopy.Name)
		{
			value = toCopy.value;
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator FsmBool(bool value)
		{
			FsmBool fsmBool = new FsmBool(string.Empty);
			fsmBool.value = value;
			return fsmBool;
		}
	}
}

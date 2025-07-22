using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmString : NamedVariable
	{
		[SerializeField]
		private string value = "";

		public string Value
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

		public FsmString()
		{
		}

		public FsmString(string name)
			: base(name)
		{
		}

		public FsmString(FsmString source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override string ToString()
		{
			return value;
		}

		public static implicit operator FsmString(string value)
		{
			FsmString fsmString = new FsmString(string.Empty);
			fsmString.value = value;
			return fsmString;
		}
	}
}

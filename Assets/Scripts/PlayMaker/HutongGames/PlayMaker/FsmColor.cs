using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmColor : NamedVariable
	{
		[SerializeField]
		private Color value = Color.black;

		public Color Value
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

		public FsmColor()
		{
		}

		public FsmColor(string name)
			: base(name)
		{
		}

		public FsmColor(FsmColor source)
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

		public static implicit operator FsmColor(Color value)
		{
			FsmColor fsmColor = new FsmColor(string.Empty);
			fsmColor.value = value;
			return fsmColor;
		}
	}
}

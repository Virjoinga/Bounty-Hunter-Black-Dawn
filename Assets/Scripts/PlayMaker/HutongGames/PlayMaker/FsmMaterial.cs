using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmMaterial : FsmObject
	{
		public new Material Value
		{
			get
			{
				return base.Value as Material;
			}
			set
			{
				base.Value = value;
			}
		}

		public FsmMaterial()
		{
		}

		public FsmMaterial(string name)
			: base(name)
		{
		}

		public FsmMaterial(FsmObject source)
			: base(source)
		{
		}
	}
}

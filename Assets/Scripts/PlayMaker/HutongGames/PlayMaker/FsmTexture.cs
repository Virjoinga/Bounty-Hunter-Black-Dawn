using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmTexture : FsmObject
	{
		public new Texture Value
		{
			get
			{
				return base.Value as Texture;
			}
			set
			{
				base.Value = value;
			}
		}

		public FsmTexture()
		{
		}

		public FsmTexture(string name)
			: base(name)
		{
		}

		public FsmTexture(FsmObject source)
			: base(source)
		{
		}
	}
}

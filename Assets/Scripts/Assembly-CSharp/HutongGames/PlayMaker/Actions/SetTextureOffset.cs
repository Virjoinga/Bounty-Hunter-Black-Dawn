using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the Offset of a named texture in a Game Object's Material. Useful for scrolling texture effects.")]
	public class SetTextureOffset : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		public FsmInt materialIndex;

		[RequiredField]
		[UIHint(UIHint.NamedColor)]
		public FsmString namedTexture;

		[RequiredField]
		public FsmFloat offsetX;

		[RequiredField]
		public FsmFloat offsetY;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			namedTexture = "_MainTex";
			offsetX = 0f;
			offsetY = 0f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetTextureOffset();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetTextureOffset();
		}

		private void DoSetTextureOffset()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				if (ownerDefaultTarget.GetComponent<Renderer>() == null)
				{
					LogError("Missing Renderer!");
				}
				else if (ownerDefaultTarget.GetComponent<Renderer>().material == null)
				{
					LogError("Missing Material!");
				}
				else if (materialIndex.Value == 0)
				{
					ownerDefaultTarget.GetComponent<Renderer>().material.SetTextureOffset(namedTexture.Value, new Vector2(offsetX.Value, offsetY.Value));
				}
				else if (ownerDefaultTarget.GetComponent<Renderer>().materials.Length > materialIndex.Value)
				{
					Material[] materials = ownerDefaultTarget.GetComponent<Renderer>().materials;
					materials[materialIndex.Value].SetTextureOffset(namedTexture.Value, new Vector2(offsetX.Value, offsetY.Value));
					ownerDefaultTarget.GetComponent<Renderer>().materials = materials;
				}
			}
		}
	}
}

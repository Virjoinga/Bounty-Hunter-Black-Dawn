using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a named texture in a game object's material.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetMaterialTexture : FsmStateAction
	{
		[Tooltip("The GameObject that the material is applied to.")]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		[UIHint(UIHint.NamedTexture)]
		[Tooltip("A named parameter in the shader.")]
		public FsmString namedTexture;

		public FsmTexture texture;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			material = null;
			namedTexture = "_MainTex";
			texture = null;
		}

		public override void OnEnter()
		{
			DoSetMaterialTexture();
			Finish();
		}

		private void DoSetMaterialTexture()
		{
			string text = namedTexture.Value;
			if (text == string.Empty)
			{
				text = "_MainTex";
			}
			if (material.Value != null)
			{
				material.Value.SetTexture(text, texture.Value);
				return;
			}
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
					ownerDefaultTarget.GetComponent<Renderer>().material.SetTexture(text, texture.Value);
				}
				else if (ownerDefaultTarget.GetComponent<Renderer>().materials.Length > materialIndex.Value)
				{
					Material[] materials = ownerDefaultTarget.GetComponent<Renderer>().materials;
					materials[materialIndex.Value].SetTexture(text, texture.Value);
					ownerDefaultTarget.GetComponent<Renderer>().materials = materials;
				}
			}
		}
	}
}

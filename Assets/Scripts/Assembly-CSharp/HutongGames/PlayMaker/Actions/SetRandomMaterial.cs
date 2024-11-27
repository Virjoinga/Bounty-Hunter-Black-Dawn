using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Game Object's material randomly from an array of Materials.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetRandomMaterial : FsmStateAction
	{
		[CheckForComponent(typeof(Renderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmInt materialIndex;

		public FsmMaterial[] materials;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			materials = new FsmMaterial[3];
		}

		public override void OnEnter()
		{
			DoSetRandomMaterial();
			Finish();
		}

		private void DoSetRandomMaterial()
		{
			if (materials == null || materials.Length == 0)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				if (ownerDefaultTarget.GetComponent<Renderer>() == null)
				{
					LogError("SetMaterial: Missing Renderer!");
				}
				else if (materialIndex.Value == 0)
				{
					ownerDefaultTarget.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)].Value;
				}
				else if (ownerDefaultTarget.GetComponent<Renderer>().materials.Length > materialIndex.Value)
				{
					Material[] array = ownerDefaultTarget.GetComponent<Renderer>().materials;
					array[materialIndex.Value] = materials[Random.Range(0, materials.Length)].Value;
					ownerDefaultTarget.GetComponent<Renderer>().materials = array;
				}
			}
		}
	}
}

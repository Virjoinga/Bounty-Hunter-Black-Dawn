using HutongGames.PlayMaker;
using UnityEngine;

[HutongGames.PlayMaker.Tooltip("Get a material at index on a gameObject and store it in a variable")]
[ActionCategory(ActionCategory.Material)]
public class GetMaterial : FsmStateAction
{
	[CheckForComponent(typeof(Renderer))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	public FsmInt materialIndex;

	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmMaterial material;

	[HutongGames.PlayMaker.Tooltip("Get the shared material of this object. NOTE: Modifying the shared material will change the appearance of all objects using this material, and change material settings that are stored in the project too.")]
	public bool getSharedMaterial;

	public override void Reset()
	{
		gameObject = null;
		material = null;
		materialIndex = 0;
		getSharedMaterial = false;
	}

	public override void OnEnter()
	{
		DoGetMaterial();
		Finish();
	}

	private void DoGetMaterial()
	{
		GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
		if (!(ownerDefaultTarget == null))
		{
			if (ownerDefaultTarget.GetComponent<Renderer>() == null)
			{
				LogError("Missing Renderer!");
			}
			else if (materialIndex.Value == 0 && !getSharedMaterial)
			{
				material.Value = ownerDefaultTarget.GetComponent<Renderer>().material;
			}
			else if (materialIndex.Value == 0 && getSharedMaterial)
			{
				material.Value = ownerDefaultTarget.GetComponent<Renderer>().sharedMaterial;
			}
			else if (ownerDefaultTarget.GetComponent<Renderer>().materials.Length > materialIndex.Value && !getSharedMaterial)
			{
				Material[] materials = ownerDefaultTarget.GetComponent<Renderer>().materials;
				material.Value = materials[materialIndex.Value];
				ownerDefaultTarget.GetComponent<Renderer>().materials = materials;
			}
			else if (ownerDefaultTarget.GetComponent<Renderer>().materials.Length > materialIndex.Value && getSharedMaterial)
			{
				Material[] sharedMaterials = ownerDefaultTarget.GetComponent<Renderer>().sharedMaterials;
				material.Value = sharedMaterials[materialIndex.Value];
				ownerDefaultTarget.GetComponent<Renderer>().sharedMaterials = sharedMaterials;
			}
		}
	}
}

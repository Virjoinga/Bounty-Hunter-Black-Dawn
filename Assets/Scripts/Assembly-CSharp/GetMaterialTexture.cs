using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Material)]
[Tooltip("Get a texture from a material on a GameObject")]
public class GetMaterialTexture : FsmStateAction
{
	[CheckForComponent(typeof(Renderer))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	public FsmInt materialIndex;

	[UIHint(UIHint.NamedTexture)]
	public FsmString namedTexture;

	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmTexture storedTexture;

	public bool getFromSharedMaterial;

	public override void Reset()
	{
		gameObject = null;
		materialIndex = 0;
		namedTexture = "_MainTex";
		storedTexture = null;
		getFromSharedMaterial = false;
	}

	public override void OnEnter()
	{
		DoGetMaterialTexture();
		Finish();
	}

	private void DoGetMaterialTexture()
	{
		GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			return;
		}
		if (ownerDefaultTarget.GetComponent<Renderer>() == null)
		{
			LogError("Missing Renderer!");
			return;
		}
		string text = namedTexture.Value;
		if (text == string.Empty)
		{
			text = "_MainTex";
		}
		if (materialIndex.Value == 0 && !getFromSharedMaterial)
		{
			storedTexture.Value = ownerDefaultTarget.GetComponent<Renderer>().material.GetTexture(text);
		}
		else if (materialIndex.Value == 0 && getFromSharedMaterial)
		{
			storedTexture.Value = ownerDefaultTarget.GetComponent<Renderer>().sharedMaterial.GetTexture(text);
		}
		else if (ownerDefaultTarget.GetComponent<Renderer>().materials.Length > materialIndex.Value && !getFromSharedMaterial)
		{
			Material[] materials = ownerDefaultTarget.GetComponent<Renderer>().materials;
			storedTexture.Value = ownerDefaultTarget.GetComponent<Renderer>().materials[materialIndex.Value].GetTexture(text);
			ownerDefaultTarget.GetComponent<Renderer>().materials = materials;
		}
		else if (ownerDefaultTarget.GetComponent<Renderer>().materials.Length > materialIndex.Value && getFromSharedMaterial)
		{
			Material[] sharedMaterials = ownerDefaultTarget.GetComponent<Renderer>().sharedMaterials;
			storedTexture.Value = ownerDefaultTarget.GetComponent<Renderer>().sharedMaterials[materialIndex.Value].GetTexture(text);
			ownerDefaultTarget.GetComponent<Renderer>().materials = sharedMaterials;
		}
	}
}

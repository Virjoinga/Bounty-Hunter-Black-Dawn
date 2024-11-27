using UnityEngine;

public class UIUtil
{
	public int armorIndex;

	public int subArmorIndex;

	public int weaponIndex;

	public bool loadNoBreak;

	public UIUtil()
	{
		armorIndex = 0;
		subArmorIndex = 0;
		weaponIndex = 0;
		loadNoBreak = false;
	}

	public void SetShader(GameObject obj, Shader shader)
	{
		Material[] materials = obj.GetComponent<Renderer>().materials;
		foreach (Material material in materials)
		{
			Texture texture = material.mainTexture;
			if (texture == null)
			{
				texture = material.GetTexture("_MainTex");
			}
			material.shader = shader;
			material.SetTexture("_MainTex", texture);
		}
	}
}

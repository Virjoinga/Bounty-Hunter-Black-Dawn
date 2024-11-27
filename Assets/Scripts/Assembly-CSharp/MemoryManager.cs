using System.Collections.Generic;
using UnityEngine;

public class MemoryManager
{
	private static string[] TextureNameInShader = new string[7] { "_MainTex", "_BumpMap", "_Mask", "_texBase", "_texLightmap", "_tex2", "_DetailTex" };

	private static string[] MaterialNotToFree = new string[7] { "Font_Atlas", "UsualUIAtlas", "ItemIconAtlas", "IconSkill1", "IconSkill2", "IconSkill3", "IconSkill4" };

	public static void FreeTextureForNGUI(GameObject obj)
	{
		List<Material> list = new List<Material>();
		UIWidget[] componentsInChildren = obj.GetComponentsInChildren<UIWidget>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i] != null) || !(componentsInChildren[i].material != null) || list.Contains(componentsInChildren[i].material))
			{
				continue;
			}
			list.Add(componentsInChildren[i].material);
			string[] textureNameInShader = TextureNameInShader;
			foreach (string propertyName in textureNameInShader)
			{
				if (!IsExitString(componentsInChildren[i].material.name, MaterialNotToFree) && componentsInChildren[i].material.HasProperty(propertyName))
				{
					Texture texture = componentsInChildren[i].material.GetTexture(propertyName);
					Resources.UnloadAsset(texture);
				}
			}
			Resources.UnloadAsset(componentsInChildren[i].material);
		}
	}

	private static bool IsExitString(string name, string[] names)
	{
		foreach (string text in names)
		{
			if (text.Equals(name))
			{
				return true;
			}
		}
		return false;
	}

	public static void FreeAll(GameObject obj)
	{
		FreeTextureForNGUI(obj);
		FreeMesh(obj);
		FreeTexture(obj);
		FreeAnimation(obj);
		FreeAudioClip(obj);
		Object.Destroy(obj);
	}

	public static void FreeNGUI(GameObject obj)
	{
		FreeNGUI(obj, true);
	}

	public static void FreeNGUI(GameObject obj, bool freeAnimation)
	{
		FreeTextureForNGUI(obj);
		if (freeAnimation)
		{
			FreeAnimation(obj);
		}
		Object.Destroy(obj);
	}

	public static void Free(GameObject obj)
	{
		FreeMesh(obj);
		FreeTexture(obj);
		FreeAnimation(obj);
		FreeAudioClip(obj);
		Object.Destroy(obj);
	}

	public static void FreeMesh(GameObject obj)
	{
		SkinnedMeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (null != componentsInChildren[i] && componentsInChildren[i].sharedMesh != null)
			{
				Debug.Log("skinMeshList[i].mesh" + componentsInChildren[i].sharedMesh.name);
				Object.Destroy(componentsInChildren[i].sharedMesh);
			}
		}
		MeshFilter[] componentsInChildren2 = obj.GetComponentsInChildren<MeshFilter>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j] != null && componentsInChildren2[j].mesh != null)
			{
				Debug.Log("meshFilterList[i].mesh" + componentsInChildren2[j].mesh.name);
				Object.Destroy(componentsInChildren2[j].mesh);
			}
		}
	}

	public static void FreeTexture(GameObject obj)
	{
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(null != componentsInChildren[i]))
			{
				continue;
			}
			for (int j = 0; j < componentsInChildren[i].sharedMaterials.Length; j++)
			{
				if (!(componentsInChildren[i].sharedMaterials[j] != null))
				{
					continue;
				}
				string[] textureNameInShader = TextureNameInShader;
				foreach (string propertyName in textureNameInShader)
				{
					if (componentsInChildren[i].sharedMaterials[j].HasProperty(propertyName))
					{
						Texture texture = componentsInChildren[i].sharedMaterials[j].GetTexture(propertyName);
						Resources.UnloadAsset(texture);
					}
				}
			}
		}
	}

	public static void FreeAnimation(GameObject obj)
	{
		Animation[] componentsInChildren = obj.GetComponentsInChildren<Animation>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i] != null))
			{
				continue;
			}
			foreach (AnimationState item in componentsInChildren[i])
			{
				Resources.UnloadAsset(item.clip);
			}
		}
	}

	public static void FreeAudioClip(GameObject obj)
	{
		AudioSource[] componentsInChildren = obj.GetComponentsInChildren<AudioSource>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				Resources.UnloadAsset(componentsInChildren[i].clip);
			}
		}
	}

	public static void FreeAudioClipForNGUI(GameObject obj)
	{
		UIButtonSound[] componentsInChildren = obj.GetComponentsInChildren<UIButtonSound>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				Resources.UnloadAsset(componentsInChildren[i].audioClip);
			}
		}
	}
}

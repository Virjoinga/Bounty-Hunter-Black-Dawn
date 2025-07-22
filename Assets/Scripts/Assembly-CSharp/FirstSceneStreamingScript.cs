using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneStreamingScript : MonoBehaviour
{
	public int PortalID;

	public string[] BlockNames;

	private bool mFirstVolumeLoaded;

	public List<string> MaterialNames = new List<string>();

	public List<string> TextureNames = new List<string>();

	public List<string> ShaderNames = new List<string>();

	public bool FirstVolumeLoaded
	{
		get
		{
			return mFirstVolumeLoaded;
		}
	}

	private IEnumerator Start()
	{
		if (PortalID != GameApp.GetInstance().GetGameWorld().PortalID)
		{
			yield break;
		}
		foreach (string shaderName in ShaderNames)
		{
			StreamingData data4 = new StreamingData
			{
				mType = StreamingDataType.SHADER,
				mIsFirstVolume = true
			};
			data4.mRefCount++;
			data4.mName = "scenes/" + Application.loadedLevelName.ToLower() + "/shaders/android_prefab_" + shaderName;
			string strPath4 = "jar:file://" + Application.dataPath + "!/assets/assetbundles_android/" + data4.mName + ".assetbundle";
			WWW www4 = new WWW(strPath4);
			yield return www4;
			data4.mAssetBundle = www4.assetBundle;
			GameApp.GetInstance().GetSceneStreaingManager().AddLoadedData(data4);
		}
		foreach (string textureName in TextureNames)
		{
			StreamingData data3 = new StreamingData
			{
				mType = StreamingDataType.TEXTURE,
				mIsFirstVolume = true
			};
			data3.mRefCount++;
			data3.mName = "scenes/" + Application.loadedLevelName.ToLower() + "/textures/android_prefab_" + textureName;
			string strPath3 = "jar:file://" + Application.dataPath + "!/assets/assetbundles_android/" + data3.mName + ".assetbundle";
			WWW www3 = new WWW(strPath3);
			yield return www3;
			data3.mAssetBundle = www3.assetBundle;
			GameApp.GetInstance().GetSceneStreaingManager().AddLoadedData(data3);
		}
		foreach (string materialName in MaterialNames)
		{
			StreamingData data2 = new StreamingData
			{
				mType = StreamingDataType.MATERIAL,
				mIsFirstVolume = true
			};
			data2.mRefCount++;
			data2.mName = "scenes/" + Application.loadedLevelName.ToLower() + "/materials/android_prefab_" + materialName;
			string strPath2 = "jar:file://" + Application.dataPath + "!/assets/assetbundles_android/" + data2.mName + ".assetbundle";
			Debug.Log("strPath = " + strPath2);
			Debug.Log("strPath = " + strPath2);
			WWW www2 = new WWW(strPath2);
			yield return www2;
			data2.mAssetBundle = www2.assetBundle;
			GameApp.GetInstance().GetSceneStreaingManager().AddLoadedData(data2);
		}
		int counter = 0;
		string[] blockNames = BlockNames;
		foreach (string blockName in blockNames)
		{
			counter++;
			StreamingData data = new StreamingData
			{
				mType = StreamingDataType.SCENE,
				mIsFirstVolume = true
			};
			data.mRefCount++;
			data.mName = "scenes/" + Application.loadedLevelName.ToLower() + "/android_prefab_" + blockName.ToLower();
			string strPath = "jar:file://" + Application.dataPath + "!/assets/assetbundles_android/" + data.mName + ".assetbundle";
			Debug.Log("strPath = " + strPath);
			WWW www = new WWW(strPath);
			yield return www;
			data.mAssetBundle = www.assetBundle;
			data.mGameObject = Object.Instantiate(data.mAssetBundle.mainAsset) as GameObject;
			GameObject blockRoot = GameObject.Find("BlockRoot");
			if (null != blockRoot)
			{
				data.mGameObject.transform.parent = blockRoot.transform;
			}
			GameApp.GetInstance().GetSceneStreaingManager().AddLoadedData(data);
		}
		mFirstVolumeLoaded = true;
	}

	private void Update()
	{
	}
}

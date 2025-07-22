using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStreamingScript : MonoBehaviour
{
	private bool firstVolumeLoaded;

	private IEnumerator Start()
	{
		StreamingManager streamingManager = GameApp.GetInstance().GetSceneStreaingManager();
		while (!firstVolumeLoaded)
		{
			GameObject firstVolume = GameApp.GetInstance().GetGameWorld().GetPlayerSpawnPoint();
			if (null != firstVolume)
			{
				FirstSceneStreamingScript firstVolumeScript = firstVolume.GetComponent<FirstSceneStreamingScript>();
				if (null != firstVolumeScript && firstVolumeScript.enabled)
				{
					firstVolumeLoaded = firstVolumeScript.FirstVolumeLoaded;
				}
				else
				{
					firstVolumeLoaded = true;
				}
			}
			else
			{
				firstVolumeLoaded = true;
			}
			yield return 0;
		}
		while (true)
		{
			List<StreamingData> dataList = streamingManager.GetStreamingDataList();
			foreach (StreamingData data2 in dataList)
			{
				if (data2.mRefCount <= 0)
				{
					if (data2.mGameObject != null)
					{
						Object.Destroy(data2.mGameObject);
						data2.mGameObject = null;
					}
					if (data2.mAssetBundle != null)
					{
						data2.mAssetBundle.Unload(true);
						data2.mAssetBundle = null;
					}
				}
			}
			yield return 0;
			foreach (StreamingData data in dataList)
			{
				if (data.mRefCount <= 0 || !(data.mGameObject == null) || !(data.mAssetBundle == null))
				{
					continue;
				}
				string strPath = "jar:file://" + Application.dataPath + "!/assets/assetbundles_android/" + data.mName + ".assetbundle";
				WWW www = new WWW(strPath)
				{
					threadPriority = Global.Priority
				};
				yield return www;
				data.mAssetBundle = www.assetBundle;
				if (data.mType == StreamingDataType.SCENE)
				{
					data.mGameObject = Object.Instantiate(data.mAssetBundle.mainAsset) as GameObject;
					GameObject blockRoot = GameObject.Find("BlockRoot");
					if (null != blockRoot)
					{
						data.mGameObject.transform.parent = blockRoot.transform;
					}
				}
				break;
			}
			yield return 0;
		}
	}

	private void Update()
	{
	}
}

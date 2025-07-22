using System.Collections.Generic;
using UnityEngine;

public class StreamingManager
{
	private StreamingManager mInstance;

	private List<StreamingData> mStreamingDataList = new List<StreamingData>();

	private List<SceneStreamingTriggerScript> mTriggersToUnload = new List<SceneStreamingTriggerScript>();

	public List<StreamingData> GetStreamingDataList()
	{
		return mStreamingDataList;
	}

	public bool IsSceneStreamingCompleted()
	{
		foreach (StreamingData mStreamingData in mStreamingDataList)
		{
			if (mStreamingData.mType == StreamingDataType.SCENE && mStreamingData.mRefCount > 0 && mStreamingData.mGameObject == null && mStreamingData.mAssetBundle == null)
			{
				return false;
			}
		}
		return true;
	}

	public void AddToLoadData(string dataName, StreamingDataType type)
	{
		foreach (StreamingData mStreamingData in mStreamingDataList)
		{
			if (mStreamingData.mName == dataName)
			{
				if (mStreamingData.mIsFirstVolume)
				{
					mStreamingData.mIsFirstVolume = false;
				}
				else
				{
					mStreamingData.mRefCount++;
				}
				return;
			}
		}
		foreach (SceneStreamingTriggerScript item in mTriggersToUnload)
		{
			item.Unload();
		}
		mTriggersToUnload.Clear();
		StreamingData streamingData = new StreamingData();
		streamingData.mName = dataName;
		streamingData.mType = type;
		streamingData.mRefCount++;
		mStreamingDataList.Add(streamingData);
	}

	public void AddToUnloadData(string dataName)
	{
		foreach (StreamingData mStreamingData in mStreamingDataList)
		{
			if (mStreamingData.mName == dataName)
			{
				mStreamingData.mRefCount--;
				return;
			}
		}
		Debug.LogError("[Streaming Error] Not in mStreamingDataList when unload: dataName = " + dataName);
	}

	public void AddLoadedData(StreamingData data)
	{
		if (!mStreamingDataList.Contains(data))
		{
			mStreamingDataList.Add(data);
		}
	}

	public void UnloadAllImmediately()
	{
		foreach (StreamingData mStreamingData in mStreamingDataList)
		{
			if (mStreamingData.mGameObject != null)
			{
				Object.Destroy(mStreamingData.mGameObject);
				mStreamingData.mGameObject = null;
			}
			if (mStreamingData.mAssetBundle != null)
			{
				mStreamingData.mAssetBundle.Unload(true);
				mStreamingData.mAssetBundle = null;
			}
		}
		mTriggersToUnload.Clear();
		mStreamingDataList.Clear();
	}

	public bool isLoad(string dataName)
	{
		foreach (StreamingData mStreamingData in mStreamingDataList)
		{
			if (dataName == mStreamingData.mName && null != mStreamingData.mAssetBundle && null != mStreamingData.mAssetBundle.mainAsset)
			{
				return true;
			}
		}
		return false;
	}

	public GameObject CloneGameObject(string dataName)
	{
		foreach (StreamingData mStreamingData in mStreamingDataList)
		{
			if (dataName == mStreamingData.mName && null != mStreamingData.mAssetBundle && null != mStreamingData.mAssetBundle.mainAsset)
			{
				return Object.Instantiate(mStreamingData.mAssetBundle.mainAsset, Vector3.zero, Quaternion.identity) as GameObject;
			}
		}
		return null;
	}

	public Texture CloneTexture(string dataName)
	{
		foreach (StreamingData mStreamingData in mStreamingDataList)
		{
			if (dataName == mStreamingData.mName && null != mStreamingData.mAssetBundle && null != mStreamingData.mAssetBundle.mainAsset)
			{
				return Object.Instantiate(mStreamingData.mAssetBundle.mainAsset, Vector3.zero, Quaternion.identity) as Texture;
			}
		}
		return null;
	}

	public void AddTriggerToUnload(SceneStreamingTriggerScript script)
	{
		if (!mTriggersToUnload.Contains(script))
		{
			mTriggersToUnload.Add(script);
		}
	}

	public void RemoveTriggerToUnload(SceneStreamingTriggerScript script)
	{
		if (mTriggersToUnload.Contains(script))
		{
			mTriggersToUnload.Remove(script);
		}
	}
}

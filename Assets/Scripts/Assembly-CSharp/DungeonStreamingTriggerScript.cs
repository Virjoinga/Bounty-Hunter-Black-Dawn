using System.Collections;
using UnityEngine;

public class DungeonStreamingTriggerScript : MonoBehaviour
{
	public DungeonMap mMapToUnload;

	public DungeonMap mMapToLoad;

	public int mCurrentMapId;

	public bool mCanLoad = true;

	public bool mIsLoading;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private IEnumerator OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != PhysicsLayer.PLAYER_COLLIDER)
		{
			yield break;
		}
		mIsLoading = true;
		if (mMapToUnload != null)
		{
			DungeonMap fromMap2 = DungeonManager.GetInstance().CurrentMap;
			if (fromMap2.TriggerScript.mIsLoading)
			{
				fromMap2.TriggerScript.mCanLoad = false;
			}
			if (null != mMapToUnload.MapObject)
			{
				Object.Destroy(mMapToUnload.MapObject);
				yield return 0;
			}
		}
		if (mMapToLoad != null && mCanLoad)
		{
			mMapToLoad.CreateMapObject();
			DungeonMap fromMap = DungeonManager.GetInstance().CurrentMap;
			mMapToLoad.TriggerScript.mMapToUnload = fromMap;
			if (mCurrentMapId > DungeonManager.GetInstance().CurrentMapId)
			{
				mMapToLoad.TriggerScript.mMapToLoad = DungeonManager.GetInstance()[mCurrentMapId + 2];
				if (fromMap != null)
				{
					fromMap.TriggerScript.mMapToLoad = DungeonManager.GetInstance()[mCurrentMapId - 2];
					fromMap.TriggerScript.mMapToUnload = DungeonManager.GetInstance()[mCurrentMapId + 1];
				}
			}
			else
			{
				mMapToLoad.TriggerScript.mMapToLoad = DungeonManager.GetInstance()[mCurrentMapId - 2];
				if (fromMap != null)
				{
					fromMap.TriggerScript.mMapToLoad = DungeonManager.GetInstance()[mCurrentMapId + 2];
					fromMap.TriggerScript.mMapToUnload = DungeonManager.GetInstance()[mCurrentMapId - 1];
				}
			}
			DungeonManager.GetInstance().CurrentMapId = mCurrentMapId;
			for (int i = 0; i < mMapToLoad.RoomNum; i++)
			{
				if (!mCanLoad)
				{
					break;
				}
				mMapToLoad.SetRoomPosition(i);
				DungeonRoom room = mMapToLoad[i];
				if (room == null)
				{
					continue;
				}
				for (int x = 0; x < room.LengthX; x++)
				{
					if (!mCanLoad)
					{
						break;
					}
					for (int y = 0; y < room.LengthY; y++)
					{
						if (!mCanLoad)
						{
							break;
						}
						room.LoadBlock(x, y);
						yield return 0;
					}
				}
			}
		}
		mCanLoad = true;
		mIsLoading = false;
	}
}

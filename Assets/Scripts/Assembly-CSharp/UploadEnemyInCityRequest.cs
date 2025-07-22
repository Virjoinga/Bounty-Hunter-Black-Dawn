using System.Collections.Generic;

public class UploadEnemyInCityRequest : Request
{
	private List<SceneStatus> mSceneStatusList = new List<SceneStatus>();

	private int mBufferLength;

	public UploadEnemyInCityRequest(Dictionary<byte, SceneInfo> sceneDictionary)
	{
		requestID = 136;
		mBufferLength = 0;
		foreach (KeyValuePair<byte, SceneInfo> item2 in sceneDictionary)
		{
			SceneStatus sceneStatus = new SceneStatus
			{
				mSceneID = item2.Key
			};
			foreach (KeyValuePair<byte, SpawnPointInfo> item3 in item2.Value.mSpawnPointDictionary)
			{
				PointStatus pointStatus = new PointStatus
				{
					mPointID = item3.Key
				};
				foreach (KeyValuePair<byte, Enemy> item4 in item3.Value.mEnemyDictionary)
				{
					EnemyStatus item = new EnemyStatus
					{
						mEnemyID = item4.Key,
						mUniqueID = item4.Value.UniqueID,
						mEnemyType = item4.Value.EnemyType,
						mEnemyLevel = item4.Value.Level,
						mIsElite = item4.Value.IsElite,
						mHp = item4.Value.Hp,
						mShield = item4.Value.Shield
					};
					pointStatus.mEnemyStatusList.Add(item);
					mBufferLength += 16;
				}
				sceneStatus.mPointStatusList.Add(pointStatus);
				mBufferLength += 2;
			}
			mSceneStatusList.Add(sceneStatus);
			mBufferLength += 2;
		}
		mBufferLength++;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(mBufferLength);
		bytesBuffer.AddByte((byte)mSceneStatusList.Count);
		foreach (SceneStatus mSceneStatus in mSceneStatusList)
		{
			bytesBuffer.AddByte(mSceneStatus.mSceneID);
			bytesBuffer.AddByte((byte)mSceneStatus.mPointStatusList.Count);
			foreach (PointStatus mPointStatus in mSceneStatus.mPointStatusList)
			{
				bytesBuffer.AddByte(mPointStatus.mPointID);
				bytesBuffer.AddByte((byte)mPointStatus.mEnemyStatusList.Count);
				foreach (EnemyStatus mEnemyStatus in mPointStatus.mEnemyStatusList)
				{
					bytesBuffer.AddByte(mEnemyStatus.mEnemyID);
					bytesBuffer.AddInt(mEnemyStatus.mUniqueID);
					bytesBuffer.AddByte((byte)mEnemyStatus.mEnemyType);
					bytesBuffer.AddByte(mEnemyStatus.mEnemyLevel);
					bytesBuffer.AddBool(mEnemyStatus.mIsElite);
					bytesBuffer.AddInt(mEnemyStatus.mHp);
					bytesBuffer.AddInt(mEnemyStatus.mShield);
				}
			}
		}
		return bytesBuffer.GetBytes();
	}
}

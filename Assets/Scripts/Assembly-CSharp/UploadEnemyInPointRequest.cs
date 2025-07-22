using System.Collections.Generic;

public class UploadEnemyInPointRequest : Request
{
	private byte mPointID;

	private bool mOneEnemy;

	private byte mEnemyID;

	private bool mIsBoss;

	private List<EnemyStatus> mEnemyStatusList = new List<EnemyStatus>();

	private EnemyStatus mEnemyStatus;

	public UploadEnemyInPointRequest(byte pointID, bool oneEnemy, byte enemyID)
	{
		requestID = 137;
		mPointID = pointID;
		mOneEnemy = oneEnemy;
		mEnemyID = enemyID;
		Dictionary<byte, Enemy> enemies = GameApp.GetInstance().GetGameWorld().GetEnemies(mPointID);
		mEnemyStatusList = new List<EnemyStatus>();
		if (mOneEnemy)
		{
			Enemy enemy = GameApp.GetInstance().GetGameWorld().GetEnemy(mPointID, mEnemyID);
			mEnemyStatus = new EnemyStatus();
			if (enemy != null)
			{
				mEnemyStatus.mEnemyID = enemy.EnemyID;
				mEnemyStatus.mUniqueID = enemy.UniqueID;
				mEnemyStatus.mEnemyType = enemy.EnemyType;
				mEnemyStatus.mEnemyLevel = enemy.Level;
				mEnemyStatus.mIsElite = enemy.IsElite;
				mEnemyStatus.mSpawnPosition = enemy.SpawnPosition;
			}
			return;
		}
		mIsBoss = false;
		foreach (KeyValuePair<byte, Enemy> item2 in enemies)
		{
			if (item2.Value.IsBoss())
			{
				mIsBoss = true;
				break;
			}
		}
		foreach (KeyValuePair<byte, Enemy> item3 in enemies)
		{
			EnemyStatus item = new EnemyStatus
			{
				mEnemyID = item3.Value.EnemyID,
				mUniqueID = item3.Value.UniqueID,
				mEnemyType = item3.Value.EnemyType,
				mEnemyLevel = item3.Value.Level,
				mIsElite = item3.Value.IsElite,
				mSpawnPosition = item3.Value.SpawnPosition
			};
			mEnemyStatusList.Add(item);
		}
	}

	public UploadEnemyInPointRequest(byte pointID, List<EnemyStatus> enemyStatusList)
	{
		requestID = 137;
		mPointID = pointID;
		mOneEnemy = false;
		mIsBoss = false;
		mEnemyStatusList = enemyStatusList;
	}

	public override byte[] GetBody()
	{
		int num = 0;
		num = ((!mOneEnemy) ? (4 + 14 * mEnemyStatusList.Count) : 17);
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddBool(mOneEnemy);
		if (mOneEnemy)
		{
			bytesBuffer.AddByte(mEnemyID);
			bytesBuffer.AddByte(mEnemyStatus.mEnemyID);
			bytesBuffer.AddInt(mEnemyStatus.mUniqueID);
			bytesBuffer.AddByte((byte)mEnemyStatus.mEnemyType);
			bytesBuffer.AddByte(mEnemyStatus.mEnemyLevel);
			bytesBuffer.AddBool(mEnemyStatus.mIsElite);
			bytesBuffer.AddShort((short)(mEnemyStatus.mSpawnPosition.x * 10f));
			bytesBuffer.AddShort((short)(mEnemyStatus.mSpawnPosition.y * 10f));
			bytesBuffer.AddShort((short)(mEnemyStatus.mSpawnPosition.z * 10f));
		}
		else
		{
			bytesBuffer.AddByte((byte)mEnemyStatusList.Count);
			bytesBuffer.AddBool(mIsBoss);
			foreach (EnemyStatus mEnemyStatus in mEnemyStatusList)
			{
				bytesBuffer.AddByte(mEnemyStatus.mEnemyID);
				bytesBuffer.AddInt(mEnemyStatus.mUniqueID);
				bytesBuffer.AddByte((byte)mEnemyStatus.mEnemyType);
				bytesBuffer.AddByte(mEnemyStatus.mEnemyLevel);
				bytesBuffer.AddBool(mEnemyStatus.mIsElite);
				bytesBuffer.AddShort((short)(mEnemyStatus.mSpawnPosition.x * 10f));
				bytesBuffer.AddShort((short)(mEnemyStatus.mSpawnPosition.y * 10f));
				bytesBuffer.AddShort((short)(mEnemyStatus.mSpawnPosition.z * 10f));
			}
		}
		return bytesBuffer.GetBytes();
	}
}

using System.Collections.Generic;
using UnityEngine;

internal class DownloadEnemyInPointResponse : Response
{
	private bool mIsMasterPlayer;

	private bool mOneEnemy;

	private bool mExistInGameWorld;

	private byte mPointID;

	private byte mEnemyID;

	private List<EnemyStatus> mEnemyStatusList = new List<EnemyStatus>();

	private EnemyStatus mOneEnemyStatus;

	private List<int> mPlayerIDList = new List<int>();

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mOneEnemy = bytesBuffer.ReadBool();
		mPointID = bytesBuffer.ReadByte();
		if (mOneEnemy)
		{
			mEnemyID = bytesBuffer.ReadByte();
		}
		else
		{
			mExistInGameWorld = bytesBuffer.ReadBool();
		}
		if (mOneEnemy)
		{
			mOneEnemyStatus = new EnemyStatus();
			mOneEnemyStatus.mHp = bytesBuffer.ReadInt();
			mOneEnemyStatus.mShield = bytesBuffer.ReadInt();
			mOneEnemyStatus.mSpeedRate = bytesBuffer.ReadShort();
			mOneEnemyStatus.mEnemyLevel = bytesBuffer.ReadByte();
			mOneEnemyStatus.mIsElite = bytesBuffer.ReadBool();
			short num = bytesBuffer.ReadShort();
			short num2 = bytesBuffer.ReadShort();
			short num3 = bytesBuffer.ReadShort();
			mOneEnemyStatus.mSpawnPosition = new Vector3((float)num / 10f, (float)num2 / 10f, (float)num3 / 10f);
		}
		else
		{
			byte b = bytesBuffer.ReadByte();
			for (byte b2 = 0; b2 < b; b2++)
			{
				EnemyStatus enemyStatus = new EnemyStatus();
				enemyStatus.mEnemyID = bytesBuffer.ReadByte();
				enemyStatus.mHp = bytesBuffer.ReadInt();
				enemyStatus.mShield = bytesBuffer.ReadInt();
				enemyStatus.mSpeedRate = bytesBuffer.ReadShort();
				if (!mExistInGameWorld)
				{
					enemyStatus.mEnemyType = (EnemyType)bytesBuffer.ReadByte();
					enemyStatus.mUniqueID = bytesBuffer.ReadInt();
					enemyStatus.mEnemyLevel = bytesBuffer.ReadByte();
					enemyStatus.mIsElite = bytesBuffer.ReadBool();
				}
				short num4 = bytesBuffer.ReadShort();
				short num5 = bytesBuffer.ReadShort();
				short num6 = bytesBuffer.ReadShort();
				enemyStatus.mSpawnPosition = new Vector3((float)num4 / 10f, (float)num5 / 10f, (float)num6 / 10f);
				mEnemyStatusList.Add(enemyStatus);
			}
		}
		byte b3 = bytesBuffer.ReadByte();
		for (byte b4 = 0; b4 < b3; b4++)
		{
			int item = bytesBuffer.ReadInt();
			mPlayerIDList.Add(item);
		}
	}

	public override void ProcessLogic()
	{
		if (mOneEnemy)
		{
			GameApp.GetInstance().GetGameWorld().DownloadEnemy(mPlayerIDList, mPointID, mEnemyID, mOneEnemyStatus);
		}
		else
		{
			GameApp.GetInstance().GetGameWorld().DownloadEnemyInPoint(mPlayerIDList, mExistInGameWorld, mPointID, mEnemyStatusList);
		}
	}
}

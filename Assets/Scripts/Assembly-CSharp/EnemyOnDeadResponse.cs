using UnityEngine;

internal class EnemyOnDeadResponse : Response
{
	protected int mUniqueID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mUniqueID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player localPlayer = gameWorld.GetLocalPlayer();
			if (GameConfig.GetInstance().enemyConfigId.ContainsKey(mUniqueID))
			{
				short groupID = GameConfig.GetInstance().enemyConfigId[mUniqueID].GroupID;
				localPlayer.GetUserState().GetQuestProgress().OnQuestProgressEnemyKill(groupID);
			}
			else
			{
				Debug.Log("enemy uniqueID is error......");
			}
		}
	}
}

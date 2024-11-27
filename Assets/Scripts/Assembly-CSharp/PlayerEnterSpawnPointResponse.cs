using System.Collections.Generic;

internal class PlayerEnterSpawnPointResponse : Response
{
	private bool mOneEnemy;

	private byte mPointID;

	private byte mEnemyID;

	private int mPlayerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mOneEnemy = bytesBuffer.ReadBool();
		mPointID = bytesBuffer.ReadByte();
		if (mOneEnemy)
		{
			mEnemyID = bytesBuffer.ReadByte();
		}
		mPlayerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Dictionary<byte, Enemy> enemies = GameApp.GetInstance().GetGameWorld().GetEnemies(mPointID);
		if (mOneEnemy)
		{
			if (enemies.ContainsKey(mEnemyID))
			{
				enemies[mEnemyID].AddPlayer(mPlayerID);
			}
			return;
		}
		foreach (KeyValuePair<byte, Enemy> item in enemies)
		{
			item.Value.AddPlayer(mPlayerID);
		}
	}
}

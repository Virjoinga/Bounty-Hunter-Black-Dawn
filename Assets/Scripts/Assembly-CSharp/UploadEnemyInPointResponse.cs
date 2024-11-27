using System.Collections.Generic;

internal class UploadEnemyInPointResponse : Response
{
	private byte mPointID;

	private bool mOneEnemy;

	private byte mEnemyID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPointID = bytesBuffer.ReadByte();
		mOneEnemy = bytesBuffer.ReadBool();
		if (mOneEnemy)
		{
			mEnemyID = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		if (mOneEnemy)
		{
			Enemy enemy = GameApp.GetInstance().GetGameWorld().GetEnemy(mPointID, mEnemyID);
			if (enemy != null)
			{
				enemy.UploadResponse();
			}
			GameApp.GetInstance().GetGameWorld().InitEnemy(mPointID, mEnemyID);
			return;
		}
		Dictionary<byte, Enemy> enemies = GameApp.GetInstance().GetGameWorld().GetEnemies(mPointID);
		foreach (KeyValuePair<byte, Enemy> item in enemies)
		{
			item.Value.UploadResponse();
		}
		GameApp.GetInstance().GetGameWorld().InitEnemyInPoint(mPointID);
	}
}

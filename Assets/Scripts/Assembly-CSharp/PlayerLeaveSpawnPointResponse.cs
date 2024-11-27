using System.Collections.Generic;

internal class PlayerLeaveSpawnPointResponse : Response
{
	private byte mPointID;

	private int mPlayerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPointID = bytesBuffer.ReadByte();
		mPlayerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Dictionary<byte, Enemy> enemies = GameApp.GetInstance().GetGameWorld().GetEnemies(mPointID);
		foreach (KeyValuePair<byte, Enemy> item in enemies)
		{
			item.Value.RemovePlayer(mPlayerID);
		}
	}
}

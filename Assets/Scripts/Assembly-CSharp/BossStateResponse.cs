public class BossStateResponse : Response
{
	private EBossState mBossState;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mBossState = (EBossState)bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (mBossState == EBossState.WIN)
		{
			gameWorld.OnWinBossBattle();
		}
		else if (mBossState == EBossState.LOSE)
		{
			gameWorld.OnLoseBossBattle();
		}
	}
}

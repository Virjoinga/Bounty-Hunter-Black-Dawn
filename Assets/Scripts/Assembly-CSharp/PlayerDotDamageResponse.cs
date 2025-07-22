internal class PlayerDotDamageResponse : Response
{
	protected int playerID;

	protected int realDamage;

	protected byte elementType;

	protected short elementDotDamage;

	protected short elementDotTime;

	protected bool isPenetration;

	protected int attackerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		realDamage = bytesBuffer.ReadInt();
		elementType = bytesBuffer.ReadByte();
		elementDotDamage = bytesBuffer.ReadShort();
		elementDotTime = bytesBuffer.ReadShort();
		isPenetration = bytesBuffer.ReadBool();
		attackerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			localPlayer.AddElementDotData((ElementType)elementType, (int)((float)realDamage * (float)elementDotDamage / 10f), elementDotTime, isPenetration, 0, 0, WeaponType.NoGun, attackerID);
		}
	}
}

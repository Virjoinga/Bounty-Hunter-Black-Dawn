internal class PlayerChangeWeaponResponse : Response
{
	protected int playerID;

	protected byte bagIndex;

	protected ElementType elementType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		bagIndex = bytesBuffer.ReadByte();
		elementType = (ElementType)bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.ChangeWeaponInBag(bagIndex);
			remotePlayerByUserID.GetWeapon().mCurrentElementType = elementType;
			if (elementType == ElementType.AllElement)
			{
				remotePlayerByUserID.GetWeapon().SetAllElementParaForRemotePlayer();
			}
		}
	}
}

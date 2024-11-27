internal class PlayerChangeSceneResponse : Response
{
	public int channelID;

	public byte currentCityID;

	public byte currentSceneID;

	private WeaponInfo[] mWeaponInfoList;

	private byte mCurrentWeaponIndex;

	private ElementType elementType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		channelID = bytesBuffer.ReadInt();
		currentCityID = bytesBuffer.ReadByte();
		currentSceneID = bytesBuffer.ReadByte();
		mCurrentWeaponIndex = bytesBuffer.ReadByte();
		elementType = (ElementType)bytesBuffer.ReadByte();
		mWeaponInfoList = new WeaponInfo[Global.BAG_MAX_NUM];
		for (int i = 0; i < Global.BAG_MAX_NUM; i++)
		{
			mWeaponInfoList[i].mWeaponType = (WeaponType)bytesBuffer.ReadByte();
			mWeaponInfoList[i].mWeaponNameNumber = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(channelID);
		if (remotePlayerByUserID == null)
		{
			return;
		}
		remotePlayerByUserID.SetCurrentCityAndSceneID(currentCityID, currentSceneID);
		bool flag = remotePlayerByUserID.RefreshWeaponList(mWeaponInfoList, mCurrentWeaponIndex);
		if (currentCityID == GameApp.GetInstance().GetUserState().GetCurrentCityID() && currentSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID && flag)
		{
			remotePlayerByUserID.GetObject().SetActive(true);
			remotePlayerByUserID.ActivatePlayer(true);
			remotePlayerByUserID.DropAtSpawnPosition();
			if (remotePlayerByUserID.State == Player.DEAD_STATE)
			{
				remotePlayerByUserID.SetState(Player.IDLE_STATE);
			}
			remotePlayerByUserID.GetWeapon().mCurrentElementType = elementType;
			if (elementType == ElementType.AllElement)
			{
				remotePlayerByUserID.GetWeapon().SetAllElementParaForRemotePlayer();
			}
			GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
		}
		else
		{
			remotePlayerByUserID.GetObject().SetActive(false);
		}
	}
}

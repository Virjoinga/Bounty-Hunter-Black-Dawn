internal class PlayerRefreshWeaponResponse : Response
{
	protected int mPlayerID;

	private WeaponInfo[] mWeaponInfoList;

	private byte mCurrentWeaponIndex;

	private ElementType elementType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPlayerID = bytesBuffer.ReadInt();
		mWeaponInfoList = new WeaponInfo[Global.BAG_MAX_NUM];
		for (int i = 0; i < Global.BAG_MAX_NUM; i++)
		{
			mWeaponInfoList[i].mWeaponType = (WeaponType)bytesBuffer.ReadByte();
			mWeaponInfoList[i].mWeaponNameNumber = bytesBuffer.ReadByte();
		}
		mCurrentWeaponIndex = bytesBuffer.ReadByte();
		elementType = (ElementType)bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(mPlayerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.RefreshWeaponList(mWeaponInfoList, mCurrentWeaponIndex);
			remotePlayerByUserID.GetWeapon().mCurrentElementType = elementType;
			if (elementType == ElementType.AllElement)
			{
				remotePlayerByUserID.GetWeapon().SetAllElementParaForRemotePlayer();
			}
		}
	}
}

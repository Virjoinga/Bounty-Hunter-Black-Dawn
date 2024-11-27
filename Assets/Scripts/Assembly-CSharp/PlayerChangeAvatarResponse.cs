public class PlayerChangeAvatarResponse : Response
{
	private int userId;

	private byte[] armors = new byte[Global.DECORATION_PART_NUM];

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		userId = bytesBuffer.ReadInt();
		for (int i = 0; i < armors.Length; i++)
		{
			armors[i] = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userId);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.GetUserState().SetDecoration(armors);
			remotePlayerByUserID.RefreshAvatar();
		}
	}
}

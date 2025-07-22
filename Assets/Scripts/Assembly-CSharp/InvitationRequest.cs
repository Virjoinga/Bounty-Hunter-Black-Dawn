public class InvitationRequest : Request
{
	public enum Type
	{
		BossRoom = 0,
		Arena = 1,
		Story = 2,
		VS = 3,
		BossRush = 4
	}

	private int m_HostID;

	private Type m_Type;

	private SubMode m_SubMode;

	private byte m_SceneID;

	private short m_Param;

	private SubMode m_CurSubMode;

	public InvitationRequest(int hostID, Type type, SubMode mode, byte sceneID, short param)
	{
		requestID = 162;
		m_HostID = hostID;
		m_Type = type;
		m_SubMode = mode;
		m_CurSubMode = GameApp.GetInstance().GetGameMode().SubModePlay;
		m_SceneID = sceneID;
		m_Param = param;
	}

	public override byte[] GetBody()
	{
		int num = 10;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddInt(m_HostID);
		bytesBuffer.AddByte((byte)m_Type);
		bytesBuffer.AddByte((byte)m_SubMode);
		bytesBuffer.AddByte((byte)m_CurSubMode);
		bytesBuffer.AddByte(m_SceneID);
		bytesBuffer.AddShort(m_Param);
		return bytesBuffer.GetBytes();
	}
}

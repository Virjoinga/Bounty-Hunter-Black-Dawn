using UnityEngine;

public class InvitationResponse : Response
{
	private const byte TYPE_INVITATION_WAIT = 0;

	private const byte TYPE_INVITATION_SUCCESS = 1;

	private const byte TYPE_INVITATION_FAILURE = 2;

	private static BossRoomHandler mBossRoomHandler = new BossRoomHandler();

	private static ArenaHandler mArenaHandler = new ArenaHandler();

	private static StoryHandler mStoryHandler = new StoryHandler();

	private static VSHandler mVSHandler = new VSHandler();

	private static BossRushHandler mBossRushHandler = new BossRushHandler();

	private byte lastType;

	private InvitationHandler handler;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		lastType = bytesBuffer.ReadByte();
		Debug.Log("lastType : " + lastType);
		InvitationRequest.Type type = (InvitationRequest.Type)bytesBuffer.ReadByte();
		Debug.Log("invitationType : " + type);
		handler = CreateHandler(type);
		switch (lastType)
		{
		case 0:
			handler.ReadWaitData(bytesBuffer);
			break;
		case 2:
			handler.ReadFailData(bytesBuffer);
			break;
		case 1:
			handler.ReadSucceedData(bytesBuffer);
			break;
		}
	}

	private InvitationHandler CreateHandler(InvitationRequest.Type type)
	{
		switch (type)
		{
		case InvitationRequest.Type.BossRoom:
			return mBossRoomHandler;
		case InvitationRequest.Type.Arena:
			return mArenaHandler;
		case InvitationRequest.Type.Story:
			return mStoryHandler;
		case InvitationRequest.Type.VS:
			return mVSHandler;
		case InvitationRequest.Type.BossRush:
			return mBossRushHandler;
		default:
			return null;
		}
	}

	public override void ProcessLogic()
	{
		int channelID = Lobby.GetInstance().GetChannelID();
		Debug.Log("lastType : " + lastType);
		switch (lastType)
		{
		case 0:
			handler.Wait();
			break;
		case 2:
			handler.Fail();
			break;
		case 1:
			handler.Succeed();
			break;
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		switch (lastType)
		{
		case 0:
			handler.RobotWait(robot);
			break;
		case 2:
			handler.RobotFail(robot);
			break;
		case 1:
			handler.RobotSucceed(robot);
			break;
		}
	}
}

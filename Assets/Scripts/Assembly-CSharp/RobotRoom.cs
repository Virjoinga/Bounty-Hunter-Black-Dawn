using UnityEngine;

public class RobotRoom
{
	private const byte idle = 0;

	private const byte ready = 1;

	private const byte end = 2;

	private const byte wait = 3;

	private byte state;

	private Timer createRoomTime = new Timer();

	private Timer joinRoomTime = new Timer();

	private Timer createVSRoomTime = new Timer();

	private Timer joinVSRoomTime = new Timer();

	public void Init()
	{
		state = 0;
	}

	public void DoLoop(RobotUser robot)
	{
		if (robot.GetNetworkManager() == null)
		{
			return;
		}
		if (state == 0)
		{
			createRoomTime.SetTimer(Random.Range(2, 5), false);
			createVSRoomTime.SetTimer(Random.Range(2, 5), false);
			joinRoomTime.SetTimer(Random.Range(1, 4), false);
			joinVSRoomTime.SetTimer(Random.Range(1, 4), false);
			state = 1;
		}
		else if (state == 1)
		{
			if (createRoomTime.Ready())
			{
				CreateRoomRequest request = new CreateRoomRequest(robot.GetLobby().GetUserName(), 0, 4, false, 0, robot.GetGameScene().mCityID, robot.GetUserState().GetCharLevel(), robot.GetTimeManager().Ping, 1, 999, 0, "xxx", robot.GetGameScene().mCityID, robot.GetGameScene().mSceneID);
				robot.GetNetworkManager().SendRequestAsRobot(request, robot);
				state = 2;
				Debug.Log("robot create room ...........................................");
			}
			else if (joinRoomTime.Ready())
			{
				GetRoomListRequest request2 = new GetRoomListRequest(0, robot.GetUserState().GetCharLevel(), 0, 0);
				robot.GetNetworkManager().SendRequestAsRobot(request2, robot);
				state = 2;
				Debug.Log("robot join room ...........................................");
			}
			else if (createVSRoomTime.Ready())
			{
				CreateVSRoomRequest request3 = new CreateVSRoomRequest(robot.GetLobby().GetUserName(), 0, false, robot.GetTimeManager().Ping, 1, 999);
				robot.GetNetworkManager().SendRequestAsRobot(request3, robot);
				state = 2;
			}
			else if (joinVSRoomTime.Ready())
			{
				GetRoomListRequest request4 = new GetRoomListRequest(4, robot.GetUserState().GetCharLevel(), 0, 0);
				robot.GetNetworkManager().SendRequestAsRobot(request4, robot);
				state = 2;
			}
		}
	}

	public void CreateRoomFailed()
	{
		state = 0;
	}

	public void CreateRoomSuccess()
	{
		state = 0;
	}

	public void JoinRoomFailed()
	{
		state = 0;
	}

	public void JoinRoomSuccess()
	{
		state = 0;
	}
}

public class RobotRoomState : RobotUserState
{
	protected override void OnCreate()
	{
		base.OnCreate();
		GetRobotUser().GetRobotRoom().Init();
	}

	protected override void OnUpdate(RobotStateEvent eventID)
	{
		base.OnUpdate(eventID);
		switch (eventID)
		{
		case RobotStateEvent.CreateRoomSuccess:
		case RobotStateEvent.JoinRoomSuccess:
			break;
		case RobotStateEvent.StartVSGame:
			GetRobotUser().SwitchStateTo(RobotUserStateID.VSPlaying);
			break;
		case RobotStateEvent.StartGame:
			GetRobotUser().SwitchStateTo(RobotUserStateID.Playing);
			break;
		default:
			GetRobotUser().GetRobotRoom().DoLoop(GetRobotUser());
			break;
		}
	}
}

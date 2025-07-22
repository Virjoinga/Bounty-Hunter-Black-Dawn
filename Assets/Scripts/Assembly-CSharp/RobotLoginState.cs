using UnityEngine;

public class RobotLoginState : RobotUserState
{
	private Timer LoginTimer = new Timer();

	protected override void OnCreate()
	{
		base.OnCreate();
		LoginTimer.SetTimer(Random.Range(5, 10), false);
	}

	protected override void OnUpdate(RobotStateEvent eventID)
	{
		base.OnUpdate(eventID);
		if (eventID == RobotStateEvent.LoginSuccess)
		{
			GetRobotUser().SwitchStateTo(RobotUserStateID.Room);
		}
		else if (LoginTimer.Ready())
		{
			GetRobotUser().GetRobotLogin().LoginAsRobot(GetRobotUser());
			LoginTimer.Pause();
		}
	}
}

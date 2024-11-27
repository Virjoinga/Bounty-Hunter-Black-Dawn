using UnityEngine;

public class RobotVSPlayingState : RobotUserState
{
	private Timer mSendingTimer = new Timer();

	private Timer mMoveDirectionTimer = new Timer();

	private Timer mDisconnectiongTimer = new Timer();

	protected override void OnCreate()
	{
		base.OnCreate();
		GetRobotUser().GetGameScene().ResetAll();
		mSendingTimer.SetTimer(0.2f, false);
		mMoveDirectionTimer.SetTimer(0.5f, false);
		mDisconnectiongTimer.SetTimer(Random.Range(120f, 180f), false);
	}

	protected override void OnUpdate(RobotStateEvent eventID)
	{
		base.OnUpdate(eventID);
		if (mMoveDirectionTimer.Ready())
		{
			mMoveDirectionTimer.Do();
			GetRobotUser().Move();
		}
		if (mSendingTimer.Ready())
		{
			mSendingTimer.Do();
			GetRobotUser().SendInput();
		}
	}
}

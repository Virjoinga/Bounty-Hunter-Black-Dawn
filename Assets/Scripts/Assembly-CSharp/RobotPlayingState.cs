using UnityEngine;

public class RobotPlayingState : RobotUserState
{
	private const byte idle = 0;

	private const byte selectVS = 1;

	private const byte joinVS = 2;

	private const byte invitationWait = 3;

	private const byte wait = 4;

	private Timer mSendingTimer = new Timer();

	private Timer mMoveDirectionTimer = new Timer();

	private Timer mDisconnectiongTimer = new Timer();

	private byte state;

	private Timer joinVSRoom = new Timer();

	private Timer readyTimer = new Timer();

	protected override void OnCreate()
	{
		base.OnCreate();
		GetRobotUser().VSInvitaionOK = false;
		GetRobotUser().GetGameScene().ResetAll();
		joinVSRoom.SetTimer(Random.Range(5f, 15f), false);
		mSendingTimer.SetTimer(0.2f, false);
		mMoveDirectionTimer.SetTimer(0.5f, false);
		mDisconnectiongTimer.SetTimer(Random.Range(120f, 180f), false);
	}

	protected override void OnUpdate(RobotStateEvent eventID)
	{
		base.OnUpdate(eventID);
		Debug.Log("state : " + state);
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
		if (state == 0)
		{
			if (joinVSRoom.Ready())
			{
				InvitationRequest request = new InvitationRequest(GetRobotUser().GetLobby().GetChannelID(), InvitationRequest.Type.VS, SubMode.Story, 37, 0);
				GetRobotUser().GetNetworkManager().SendRequestAsRobot(request, GetRobotUser());
				state = 3;
			}
		}
		else if (state == 1)
		{
			if (GetRobotUser().GetLobby().IsMasterPlayer)
			{
				VSChangeSubModeRequest request2 = new VSChangeSubModeRequest(UIVS.Mode.CaptureHold_4v4);
				GetRobotUser().GetNetworkManager().SendRequestAsRobot(request2, GetRobotUser());
			}
			state = 2;
			readyTimer.SetTimer(2f, false);
		}
		else if (state == 2)
		{
			if (readyTimer.Ready())
			{
				VSReadyRequest request3 = new VSReadyRequest(GetRobotUser().GetLobby().GetChannelID());
				GetRobotUser().GetNetworkManager().SendRequestAsRobot(request3, GetRobotUser());
				state = 4;
			}
		}
		else if (state == 3)
		{
			if (GetRobotUser().VSInvitaionOK)
			{
				GetRobotUser().VSInvitaionOK = false;
				state = 4;
			}
		}
		else if (state != 4)
		{
		}
	}
}

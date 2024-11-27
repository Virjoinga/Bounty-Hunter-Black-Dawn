using System.Collections.Generic;
using UnityEngine;

public abstract class InvitationHandler : UIMsgListener
{
	private int m_HostID;

	private List<int> m_uidList = new List<int>();

	protected int HostID
	{
		get
		{
			return m_HostID;
		}
	}

	protected List<int> UIDList
	{
		get
		{
			return m_uidList;
		}
	}

	protected int UserID
	{
		get
		{
			return Lobby.GetInstance().GetChannelID();
		}
	}

	public virtual void ReadWaitData(BytesBuffer bb)
	{
		m_HostID = bb.ReadInt();
	}

	public virtual void ReadSucceedData(BytesBuffer bb)
	{
		int num = bb.ReadInt();
		int num2 = bb.ReadInt();
		for (int i = 0; i < num2; i++)
		{
			m_uidList.Add(bb.ReadInt());
		}
	}

	public virtual void ReadFailData(BytesBuffer bb)
	{
		int num = bb.ReadInt();
		int num2 = bb.ReadInt();
		for (int i = 0; i < num2; i++)
		{
			m_uidList.Add(bb.ReadInt());
		}
	}

	public void Wait()
	{
		OnWaiting();
	}

	public void Succeed()
	{
		OnSuccess();
	}

	public void Fail()
	{
		OnFailure();
	}

	public virtual void RobotWait(RobotUser robot)
	{
		robot.GetGameScene().ResetLastSendInvitationTime();
		int num = Random.Range(0, 100);
		if (num < 90)
		{
			InvitationConfirmRequest request = new InvitationConfirmRequest(robot.GetLobby().GetChannelID(), true);
			robot.GetNetworkManager().SendRequestAsRobot(request, robot);
			return;
		}
		InvitationConfirmRequest request2 = new InvitationConfirmRequest(robot.GetLobby().GetChannelID(), false);
		robot.GetNetworkManager().SendRequestAsRobot(request2, robot);
		InvitaionFailMessageRequest request3 = new InvitaionFailMessageRequest(robot.GetLobby().GetChannelID(), InvitaionFailMessageRequest.Type.Reject);
		robot.GetNetworkManager().SendRequestAsRobot(request3, robot);
	}

	public virtual void RobotSucceed(RobotUser robot)
	{
	}

	public void RobotFail(RobotUser robot)
	{
	}

	protected abstract void OnWaiting();

	protected abstract void OnSuccess();

	protected abstract void OnFailure();

	protected virtual void DoWaitingConfirm(string messageText)
	{
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript.VerifyEnter() && GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.CanTeleport())
		{
			UIMsgBox.instance.ShowMessage(this, messageText, 6, 4);
			return;
		}
		InvitationConfirmRequest request = new InvitationConfirmRequest(Lobby.GetInstance().GetChannelID(), false);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		if (!GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.CanTeleport())
		{
			InvitaionFailMessageRequest request2 = new InvitaionFailMessageRequest(Lobby.GetInstance().GetChannelID(), InvitaionFailMessageRequest.Type.Dying);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			return;
		}
		InvitaionFailMessageRequest request3 = new InvitaionFailMessageRequest(Lobby.GetInstance().GetChannelID(), InvitaionFailMessageRequest.Type.InMenu);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
		if (StateBar.instance != null)
		{
			StateBar.instance.InvitationRecieved();
		}
	}

	public abstract void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId);
}

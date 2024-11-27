using UnityEngine;

public class BossRushHandler : InvitationHandler
{
	protected override void OnWaiting()
	{
		Debug.Log("VSHandler OnWaiting ");
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.Block = true;
		if (base.UserID == base.HostID)
		{
			InvitationConfirmRequest request = new InvitationConfirmRequest(base.UserID, true);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			UIMsgBox.instance.ShowMessage(UIMsg.EMPTY, LocalizationManager.GetInstance().GetString("MSG_INVITATION_WAIT"), 4);
		}
		else
		{
			DoWaitingConfirm(LocalizationManager.GetInstance().GetString("MSG_INVITATION_PVP"));
		}
	}

	protected override void OnSuccess()
	{
		UIMsgBox.instance.CloseMessage();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.Block = false;
		Debug.Log("Success");
	}

	protected override void OnFailure()
	{
		if (UIMsgBox.instance.GetCurrentMessageEventID() == 4 || UIMsgBox.instance.GetCurrentMessageType() == 7 || UIMsgBox.instance.GetCurrentMessageType() == 4)
		{
			UIMsgBox.instance.CloseMessage();
		}
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript.VerifyEnter() && GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.CanTeleport())
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = false;
		}
		Debug.Log("Failure");
	}

	public override void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 4)
		{
			int channelID = Lobby.GetInstance().GetChannelID();
			switch (buttonId)
			{
			case UIMsg.UIMsgButton.Ok:
			{
				InvitationConfirmRequest request3 = new InvitationConfirmRequest(channelID, true);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
				UIMsgBox.instance.ShowMessage(UIMsg.EMPTY, LocalizationManager.GetInstance().GetString("MSG_INVITATION_WAIT"), 4);
				break;
			}
			case UIMsg.UIMsgButton.Cancel:
			{
				InvitationConfirmRequest request = new InvitationConfirmRequest(channelID, false);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				InvitaionFailMessageRequest request2 = new InvitaionFailMessageRequest(Lobby.GetInstance().GetChannelID(), InvitaionFailMessageRequest.Type.Reject);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				break;
			}
			}
		}
	}

	public override void RobotSucceed(RobotUser robot)
	{
	}
}

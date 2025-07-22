using UnityEngine;

public class BossRoomHandler : InvitationHandler, EffectsCameraListener
{
	private SubMode mSubMode;

	public override void ReadWaitData(BytesBuffer bb)
	{
		base.ReadWaitData(bb);
		mSubMode = (SubMode)bb.ReadByte();
	}

	protected override void OnWaiting()
	{
		Debug.Log("BossRoomHandler OnWaiting ");
		Lobby.GetInstance().GetInvitationInfo().mode = mSubMode;
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
			DoWaitingConfirm(LocalizationManager.GetInstance().GetString("MSG_INVITATION_BOSS_AREA"));
		}
	}

	protected override void OnSuccess()
	{
		UIMsgBox.instance.CloseMessage();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.Block = false;
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.LoadSceneOfBoss();
		EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
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

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		switch (type)
		{
		case EffectsCamera.Type.FadeIn:
			EffectsCamera.instance.StartEffect(EffectsCamera.Type.Teleport, this);
			break;
		case EffectsCamera.Type.Teleport:
		{
			GameApp.GetInstance().GetGameWorld().BossState = EBossState.BATTLE;
			Debug.Log("Dodge_This --- Start");
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Dodge_This, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger);
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetCharacterController()
				.enabled = false;
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.DropAtSpawnInBoss();
			BossArea.instance.m_Trigger.StartAnimation();
			break;
		}
		}
	}
}

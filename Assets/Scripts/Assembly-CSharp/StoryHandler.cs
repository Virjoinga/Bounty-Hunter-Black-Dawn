using UnityEngine;

public class StoryHandler : InvitationHandler
{
	private SubMode mSubMode;

	private byte targetSceneID;

	private int mPortalID;

	public override void ReadWaitData(BytesBuffer bb)
	{
		base.ReadWaitData(bb);
		mSubMode = (SubMode)bb.ReadByte();
		targetSceneID = bb.ReadByte();
		mPortalID = bb.ReadShort();
	}

	protected override void OnWaiting()
	{
		Debug.Log("StoryHandler OnWaiting ");
		Lobby.GetInstance().GetInvitationInfo().mode = mSubMode;
		GameApp.GetInstance().GetGameWorld().PortalID = mPortalID;
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.Block = true;
		if (base.UserID == base.HostID || targetSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID)
		{
			InvitationConfirmRequest request = new InvitationConfirmRequest(base.UserID, true);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			if (!UIMsgBox.instance.IsMessageShow())
			{
				UIMsgBox.instance.ShowMessage(UIMsg.EMPTY, LocalizationManager.GetInstance().GetString("MSG_INVITATION_WAIT") + "\n" + LocalizationManager.GetInstance().GetString(GameApp.GetInstance().GetGameWorld().GetSceneConfig(targetSceneID)
					.SceneName), 4);
				}
			}
			else
			{
				DoWaitingConfirm(LocalizationManager.GetInstance().GetString("MSG_INVITATION_TELEPORT") + "\n" + LocalizationManager.GetInstance().GetString(GameApp.GetInstance().GetGameWorld().GetSceneConfig(targetSceneID)
					.SceneName));
				}
			}

			protected override void OnSuccess()
			{
				Debug.Log("Success: " + targetSceneID);
				UIMsgBox.instance.CloseMessage();
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.InputController.Block = false;
				ReturnMenu.GetInstance().ConfirmReturnIfNeed();
				if (targetSceneID != GameApp.GetInstance().GetGameWorld().CurrentSceneID)
				{
					if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
					{
						EnemySpawnScript.GetInstance().OnArenaGameEnd(GameApp.GetInstance().GetGameWorld().GetLocalPlayer(), false);
						return;
					}
					InGameUIScript inGameUIScript = (InGameUIScript)GameApp.GetInstance().GetUIStateManager();
					inGameUIScript.Enter(targetSceneID);
				}
			}

			protected override void OnFailure()
			{
				Debug.Log("Failure");
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
				ReturnMenu.GetInstance().CancelReturnIfNeed();
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

			public override void RobotWait(RobotUser robot)
			{
				robot.GetGameScene().SetPortalID(mPortalID);
				base.RobotWait(robot);
			}

			public override void RobotSucceed(RobotUser robot)
			{
				robot.GetGameScene().ChangeScene(targetSceneID);
			}
		}

public class ReturnMenu : EffectsCameraListener, UIMsgListener
{
	private static ReturnMenu instance;

	private bool bToReturn;

	private bool bPayMithrilToReturn;

	private ReturnMenu()
	{
	}

	public static ReturnMenu GetInstance()
	{
		if (instance == null)
		{
			instance = new ReturnMenu();
		}
		return instance;
	}

	public void Clear()
	{
		bToReturn = false;
		bPayMithrilToReturn = false;
	}

	public bool Show()
	{
		SceneConfig currentSceneConfig = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetCurrentSceneConfig();
		if (currentSceneConfig.FatherSceneID != currentSceneConfig.SceneID)
		{
			GameApp.GetInstance().GetGameScene().UIPause(true);
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
			if (UserStateHUD.GetInstance().IsReturnCDOK() || Arena.GetInstance().IsCurrentSceneArena())
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_RETURN_TO_CITY_FREE"), 3, 6);
			}
			else
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_RETURN_TO_CITY_MITHRIL").Replace("%d", string.Empty + UserStateHUD.GetInstance().GetMithrilCostOfReturn()), 3, 7);
			}
			return true;
		}
		UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_GO_HOME_IN_HOME"));
		return false;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 6)
		{
			switch (buttonId)
			{
			case UIMsg.UIMsgButton.Ok:
				bToReturn = true;
				CloseMsg();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					int fatherSceneID = GameApp.GetInstance().GetGameWorld().GetSceneConfig(GameApp.GetInstance().GetGameWorld().CurrentSceneID)
						.FatherSceneID;
					InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
						.GetUserID(), InvitationRequest.Type.Story, SubMode.Story, (byte)fatherSceneID, 0);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					break;
				}
				ConfirmReturnIfNeed();
				if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
				{
					EnemySpawnScript.GetInstance().OnArenaGameEnd(GameApp.GetInstance().GetGameWorld().GetLocalPlayer(), false);
				}
				else
				{
					EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
				}
				break;
			case UIMsg.UIMsgButton.Cancel:
				CloseMsg();
				break;
			}
		}
		else if (whichMsg.EventId == 7)
		{
			switch (buttonId)
			{
			case UIMsg.UIMsgButton.Ok:
				if (GameApp.GetInstance().GetGlobalState().GetMithril() >= UserStateHUD.GetInstance().GetMithrilCostOfReturn())
				{
					bToReturn = true;
					bPayMithrilToReturn = true;
					CloseMsg();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						int fatherSceneID2 = GameApp.GetInstance().GetGameWorld().GetSceneConfig(GameApp.GetInstance().GetGameWorld().CurrentSceneID)
							.FatherSceneID;
						InvitationRequest request2 = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
							.GetUserID(), InvitationRequest.Type.Story, SubMode.Story, (byte)fatherSceneID2, 0);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						break;
					}
					ConfirmReturnIfNeed();
					if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
					{
						EnemySpawnScript.GetInstance().OnArenaGameEnd(GameApp.GetInstance().GetGameWorld().GetLocalPlayer(), false);
					}
					else
					{
						EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
					}
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
				break;
			case UIMsg.UIMsgButton.Cancel:
				CloseMsg();
				break;
			}
		}
		else if (whichMsg.EventId == 9 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			CloseMsg();
			UIIAP.Show(UIIAP.Type.IAP, true);
		}
	}

	private void CloseMsg()
	{
		UIMsgBox.instance.CloseMessage();
		GameApp.GetInstance().GetGameScene().UIPause(false);
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.Block = false;
	}

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		EffectsCamera.instance.RemoveListener();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.ReturnToCity();
	}

	public void ConfirmReturnIfNeed()
	{
		if (bPayMithrilToReturn)
		{
			bPayMithrilToReturn = false;
			GameApp.GetInstance().GetGlobalState().BuyWithMithril(UserStateHUD.GetInstance().GetMithrilCostOfReturn());
		}
		if (bToReturn)
		{
			bToReturn = false;
			if (!Arena.GetInstance().IsCurrentSceneArena())
			{
				UserStateHUD.GetInstance().ResetReturnCD();
			}
		}
	}

	public void CancelReturnIfNeed()
	{
		if (bPayMithrilToReturn)
		{
			bPayMithrilToReturn = false;
		}
		if (bToReturn)
		{
			bToReturn = false;
		}
	}
}

public class HUDMultiplayBattle : HUDBattle, UIMsgListener
{
	protected override void OnShow()
	{
		base.OnShow();
		HUDManager.instance.m_ChatManager.SetAllActiveRecursively(true);
		HUDManager.instance.m_HotKeyManager.m_OffLine.SetActive(true);
	}

	public override void OnHotKeyEvent(UIButtonX.ButtonInfo info)
	{
		base.OnHotKeyEvent(info);
		switch (info.buttonId)
		{
		case 15:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Pressing)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.InputController.RotateCamera = false;
			}
			else if (info.buttonEvent == UIButtonX.ButtonInfo.Event.ReleaseAfterPressing)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.InputController.RotateCamera = true;
			}
			break;
		case 7:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				ShowInput();
			}
			break;
		case 20:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_CONFIRM_TO_DISCONNECT"), 3, 47);
			}
			break;
		}
	}

	protected void ShowInput()
	{
		HUDManager.instance.m_ChatManager.PopOrPushInputBar();
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId != 47)
		{
			return;
		}
		switch (buttonId)
		{
		case UIMsg.UIMsgButton.Ok:
			if (Arena.GetInstance().IsCurrentSceneArena())
			{
				EnemySpawnScript.GetInstance().DisconnectMulitplay();
			}
			else
			{
				GameApp.GetInstance().GetGameWorld().ExitMultiplayerMode();
			}
			UIMsgBox.instance.CloseMessage();
			break;
		case UIMsg.UIMsgButton.Cancel:
			UIMsgBox.instance.CloseMessage();
			break;
		}
	}
}

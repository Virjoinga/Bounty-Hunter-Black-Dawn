public class HUDVSTDMBattle : HUDMultiplayBattle
{
	private bool newBattle;

	protected override void OnShow()
	{
		base.OnShow();
		HUDManager.instance.m_HotKeyManager.m_Mission.SetActive(false);
		HUDManager.instance.m_HotKeyManager.m_Return.SetActive(false);
		newBattle = false;
	}

	protected override void OnPlayerRespawn()
	{
		base.OnPlayerRespawn();
		HUDManager.instance.m_HotKeyManager.m_Mission.SetActive(false);
		HUDManager.instance.m_HotKeyManager.m_Return.SetActive(false);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		UserStateHUD.GetInstance().UpdateVSBattleFieldState();
		if (newBattle)
		{
			UserStateHUD.GetInstance().GetVSBattleFieldState().NewBattle = false;
			newBattle = false;
		}
		else if (UserStateHUD.GetInstance().GetVSBattleFieldState().NewBattle)
		{
			newBattle = true;
		}
		if (UserStateHUD.GetInstance().IsUserDead() && !UIVSBattleShop.IsShow())
		{
			UIVSBattleShop.ShowRevive();
		}
	}

	public override void OnHotKeyEvent(UIButtonX.ButtonInfo info)
	{
		base.OnHotKeyEvent(info);
		int buttonId = info.buttonId;
		if (buttonId == 21 && info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
		{
			UserStateHUD.VSBattleField vSBattleFieldState = UserStateHUD.GetInstance().GetVSBattleFieldState();
			vSBattleFieldState.RedTeam.SetScoreVisible(false);
			vSBattleFieldState.BlueTeam.SetScoreVisible(false);
			UIVSBattleResult.ShowResult(vSBattleFieldState.RedTeam, vSBattleFieldState.BlueTeam, UIVSBattleResult.Condition.InVS);
		}
	}
}

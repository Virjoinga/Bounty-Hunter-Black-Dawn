public class UIAdsNoVideo : UIGameMenu, UIMsgListener
{
	private static byte prevPhase;

	private bool isAdsShow;

	protected override byte InitMask()
	{
		return 0;
	}

	protected override void Awake()
	{
		base.Awake();
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_ADS_OUT"), 3, 49);
		SetMenuCloseOnDestroy(true);
		isAdsShow = false;
	}

	private void Update()
	{
		if (isAdsShow && !UIAds.IsShow())
		{
			isAdsShow = false;
			Close();
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 49)
		{
			switch (buttonId)
			{
			case UIMsg.UIMsgButton.Ok:
				UIAds.Show();
				UIMsgBox.instance.CloseMessage();
				isAdsShow = true;
				break;
			case UIMsg.UIMsgButton.Cancel:
				UIMsgBox.instance.CloseMessage();
				Close();
				break;
			}
		}
	}

	public static void Show()
	{
		prevPhase = 6;
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(36, false, false, true);
	}

	public static void Close()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(prevPhase, false, false, true);
	}
}

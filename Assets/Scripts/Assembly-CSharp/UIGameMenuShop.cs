public class UIGameMenuShop : UIGameMenu
{
	protected override byte InitMask()
	{
		return (byte)(mMask | 8u);
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		GameApp.GetInstance().Save();
	}
}

public class UIGameMenuNormal : UIGameMenu
{
	protected override byte InitMask()
	{
		return (byte)(mMask | 4u);
	}
}

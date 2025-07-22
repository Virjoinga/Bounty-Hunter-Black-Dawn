public class NGUIDropButtonScript : NGUIBackPackObject
{
	private void OnClick()
	{
		if (NGUIBackPackUIScript.mInstance != null && !NGUIBackPackUIScript.mInstance.IsBagLocked && NGUIItemSlot.mSelectedSlot != null)
		{
			NGUIItemSlot.mSelectedSlot.ThrowItem();
		}
	}
}

using UnityEngine;

public class ShopRefreshTimeScript : MonoBehaviour
{
	public UILabel label;

	public bool IsShopRefreshTimer = true;

	private void Update()
	{
		if (label != null)
		{
			float num = 0f;
			if (IsShopRefreshTimer)
			{
				num = GameApp.GetInstance().GetGameWorld().mShopItemRefreshTimer.GetTimeNeededToNextReady();
			}
			int num2 = (int)(num / 60f);
			int num3 = (int)(num % 60f);
			string text = num2.ToString();
			string text2 = num3.ToString();
			if (num2 < 10)
			{
				text = "0" + text;
			}
			if (num3 < 10)
			{
				text2 = "0" + text2;
			}
			label.text = text + ":" + text2;
		}
	}
}

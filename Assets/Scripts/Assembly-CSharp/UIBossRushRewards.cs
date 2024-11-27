using UnityEngine;

public class UIBossRushRewards : MonoBehaviour
{
	public UILabel priceLabel;

	public UISprite itemIcon;

	public UISprite itemQuality;

	public void SetRewards(int price, string iconName, string qualityName)
	{
		priceLabel.text = string.Empty + price;
		itemIcon.spriteName = iconName;
		itemQuality.spriteName = qualityName;
	}
}

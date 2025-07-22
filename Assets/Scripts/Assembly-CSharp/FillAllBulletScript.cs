using UnityEngine;

public class FillAllBulletScript : MonoBehaviour
{
	public UILabel PriceLabel;

	public UISprite buttonImageSprite;

	public string blockSpriteName;

	protected int mPrice;

	private UserState userState;

	private void OnEnable()
	{
		userState = GameApp.GetInstance().GetUserState();
	}

	private void OnClick()
	{
		if (mPrice < 0 || userState == null)
		{
			return;
		}
		if (userState.GetCash() >= mPrice)
		{
			for (int i = 0; i < 8; i++)
			{
				WeaponType type = (WeaponType)(i + 1);
				userState.AddBulletByWeaponType(type, 1000);
			}
			userState.Buy(mPrice);
			if (ShopUIScript.mInstance != null)
			{
				BuyBulletButtonScript[] componentsInChildren = ShopUIScript.mInstance.transform.GetComponentsInChildren<BuyBulletButtonScript>();
				BuyBulletButtonScript[] array = componentsInChildren;
				foreach (BuyBulletButtonScript buyBulletButtonScript in array)
				{
					buyBulletButtonScript.Refresh();
				}
			}
		}
		else if (ShopUIScript.mInstance != null)
		{
			ShopUIScript.mInstance.MoneyNotEnough();
		}
	}

	private void Update()
	{
		if (userState == null)
		{
			return;
		}
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int num = 0;
		for (int i = 0; i < 8; i++)
		{
			WeaponType weaponType = (WeaponType)(i + 1);
			int num2 = userState.GetMaxBulletByWeaponType(weaponType) - userState.GetBulletByWeaponType(weaponType) - localPlayer.GetBulletInGuns(weaponType);
			int num3 = Mathf.CeilToInt((float)num2 / (float)(int)userState.GetBulletInMagsByWeaponType(weaponType));
			num += num3 * userState.GetBulletPriceFactorByWeaponType(weaponType) * userState.GetCharLevel();
		}
		mPrice = num;
		if (!(PriceLabel != null))
		{
			return;
		}
		if (mPrice > userState.GetCash())
		{
			PriceLabel.text = "$[FF0000]" + mPrice + "[-]";
			base.gameObject.GetComponent<Collider>().enabled = false;
			if (buttonImageSprite != null && !buttonImageSprite.spriteName.Equals(blockSpriteName))
			{
				buttonImageSprite.spriteName = blockSpriteName;
				buttonImageSprite.MakePixelPerfect();
			}
		}
		else
		{
			base.gameObject.GetComponent<Collider>().enabled = true;
			PriceLabel.text = "$" + mPrice;
		}
	}
}

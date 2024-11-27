using UnityEngine;

public class BuyBulletButtonScript : MonoBehaviour
{
	public WeaponType BulletType;

	public UILabel CurrentBullet;

	public UILabel Price;

	private void OnEnable()
	{
		Refresh();
	}

	private void OnClick()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState.GetBulletByWeaponType(BulletType) + localPlayer.GetBulletInGuns(BulletType) < userState.GetMaxBulletByWeaponType(BulletType))
		{
			int num = userState.GetBulletPriceFactorByWeaponType(BulletType) * userState.GetCharLevel();
			if (userState.GetCash() >= num)
			{
				GameApp.GetInstance().GetUserState().AddBulletByWeaponType(BulletType, userState.GetBulletInMagsByWeaponType(BulletType));
				userState.Buy(num);
				Refresh();
			}
			else if (ShopUIScript.mInstance != null)
			{
				ShopUIScript.mInstance.MoneyNotEnough();
			}
		}
	}

	public void Refresh()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		CurrentBullet.text = userState.GetBulletInMagsByWeaponType(BulletType).ToString();
		int num = userState.GetBulletPriceFactorByWeaponType(BulletType) * userState.GetCharLevel();
		if (num > userState.GetCash())
		{
			Price.text = "[FF0000]" + num + "[-]";
		}
		else
		{
			Price.text = num.ToString();
		}
	}
}

using UnityEngine;

public class WeaponIcon : MonoBehaviour
{
	public UISprite[] m_WeaponIcon;

	private string[] userWeapon = new string[4]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	private float mLastUpdateTime;

	private void OnEnable()
	{
	}

	private void Update()
	{
		if (!(Time.time - mLastUpdateTime > 0.2f))
		{
			return;
		}
		mLastUpdateTime = Time.time;
		string[] userWeapons = UserStateHUD.GetInstance().GetUserWeapons();
		for (int i = 0; i < userWeapon.Length; i++)
		{
			if (userWeapon[i] != userWeapons[i])
			{
				userWeapon[i] = userWeapons[i];
				ChangeWeaponIcon(m_WeaponIcon[i], userWeapon[i]);
			}
		}
	}

	private void ChangeWeaponIcon(UISprite weaponIcon, string icon)
	{
		weaponIcon.spriteName = icon;
		weaponIcon.MakePixelPerfect();
		if (!icon.Equals("none") && !icon.Equals("gun"))
		{
			weaponIcon.transform.localScale *= 0.58f;
		}
	}
}

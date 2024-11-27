using UnityEngine;

public class HPShieldStatesScript : MonoBehaviour
{
	public UIFilledSprite HpSprite;

	public UILabel HpLabel;

	public UIFilledSprite ShieldSprite;

	public UILabel ShieldLabel;

	private void Update()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer != null)
		{
			int hp = localPlayer.Hp;
			int maxHp = localPlayer.MaxHp;
			int shield = localPlayer.Shield;
			int maxShield = localPlayer.MaxShield;
			if (HpSprite != null)
			{
				HpSprite.fillAmount = (float)hp / (float)maxHp;
			}
			if (HpLabel != null)
			{
				HpLabel.text = hp + "/" + maxHp;
			}
			if (ShieldSprite != null)
			{
				ShieldSprite.fillAmount = (float)shield / (float)maxShield;
			}
			if (ShieldLabel != null)
			{
				ShieldLabel.text = shield + "/" + maxShield;
			}
		}
	}
}

using UnityEngine;

public class StateDamage : MonoBehaviour
{
	private float mLastUpdateTime;

	private void Update()
	{
		UpdateNewDamage();
	}

	private void UpdateNewDamage()
	{
		UserStateHUD.DamageHUD damageHUD = UserStateHUD.GetInstance().PopDamage();
		if (damageHUD != null)
		{
			NumberManager.GetInstance().ShowDamage(damageHUD);
		}
	}
}

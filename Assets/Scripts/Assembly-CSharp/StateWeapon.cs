using UnityEngine;

public class StateWeapon : MonoBehaviour
{
	[SerializeField]
	private UILabel m_LabelBullet;

	private void Update()
	{
		if (base.gameObject.active)
		{
			m_LabelBullet.text = UserStateHUD.GetInstance().GetBulletInfo();
		}
	}
}

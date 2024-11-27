using UnityEngine;

public class StateGrenade : MonoBehaviour
{
	[SerializeField]
	private UILabel m_LabelGrenade;

	private void Update()
	{
		if (UserStateHUD.GetInstance().IsHasGrenade())
		{
			if (!m_LabelGrenade.gameObject.activeSelf)
			{
				m_LabelGrenade.gameObject.SetActive(true);
			}
			m_LabelGrenade.text = UserStateHUD.GetInstance().GetGrenadeInfo();
		}
		else if (m_LabelGrenade.gameObject.activeSelf)
		{
			m_LabelGrenade.gameObject.SetActive(false);
		}
	}
}

using UnityEngine;

public class StateSummon : MonoBehaviour
{
	public UISprite m_BG;

	public UIFilledSprite m_Hp;

	public UIFilledSprite m_Shield;

	private void Update()
	{
		if (UserStateHUD.GetInstance().IsUserSummonExit())
		{
			if (!isShow())
			{
				Show();
				return;
			}
			m_Hp.fillAmount = UserStateHUD.GetInstance().GetSummonHPPercent();
			m_Shield.fillAmount = 1f - UserStateHUD.GetInstance().GetSummonShieldPercent();
		}
		else if (isShow())
		{
			Hide();
		}
	}

	private bool isShow()
	{
		return m_BG.gameObject.activeSelf && m_Hp.gameObject.activeSelf && m_Shield.gameObject.activeSelf;
	}

	private void Show()
	{
		NGUITools.SetActive(m_BG.gameObject, true);
		NGUITools.SetActive(m_Hp.gameObject, true);
		NGUITools.SetActive(m_Shield.gameObject, true);
	}

	private void Hide()
	{
		NGUITools.SetActive(m_BG.gameObject, false);
		NGUITools.SetActive(m_Hp.gameObject, false);
		NGUITools.SetActive(m_Shield.gameObject, false);
	}
}

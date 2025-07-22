using UnityEngine;

public class StateDead : MonoBehaviour
{
	private const float DURATION = 1f;

	public GameObject m_Text;

	public GameObject m_CountDownBar;

	public GameObject m_RebornBar;

	public UIFilledSprite m_CountDownForeBar;

	public UIFilledSprite m_RebornForeBar;

	private bool bStart;

	private float startTime;

	private void OnEnable()
	{
		NGUITools.SetActive(m_Text, false);
		NGUITools.SetActive(m_CountDownBar, false);
		NGUITools.SetActive(m_RebornBar, false);
	}

	private void Update()
	{
		if (UserStateHUD.GetInstance().IsUserDying())
		{
			if (!bStart)
			{
				bStart = true;
				NGUITools.SetActive(m_Text, true);
				NGUITools.SetActive(m_CountDownBar, true);
				startTime = Time.time;
				return;
			}
			float percentOfUserSave = UserStateHUD.GetInstance().GetPercentOfUserSave();
			if (percentOfUserSave > 0f)
			{
				if (!m_RebornBar.activeSelf)
				{
					NGUITools.SetActive(m_RebornBar, true);
				}
				m_RebornForeBar.fillAmount = percentOfUserSave;
			}
			else if (m_RebornBar.activeSelf)
			{
				NGUITools.SetActive(m_RebornBar, false);
			}
			m_CountDownForeBar.fillAmount = UserStateHUD.GetInstance().GetPercentOfAliveTime();
			if (m_Text.activeSelf && Time.time - startTime > 1f)
			{
				NGUITools.SetActive(m_Text, false);
			}
		}
		else
		{
			if (bStart)
			{
				bStart = false;
			}
			if (m_Text.activeSelf)
			{
				NGUITools.SetActive(m_Text, false);
			}
			if (m_CountDownBar.activeSelf)
			{
				NGUITools.SetActive(m_CountDownBar, false);
			}
			if (m_RebornBar.activeSelf)
			{
				NGUITools.SetActive(m_RebornBar, false);
			}
		}
	}
}

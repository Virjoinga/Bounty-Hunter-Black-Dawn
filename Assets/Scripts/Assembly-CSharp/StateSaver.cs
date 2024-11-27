using UnityEngine;

public class StateSaver : MonoBehaviour
{
	public GameObject m_Button;

	public GameObject m_ProgressBar;

	public UIFilledSprite m_ForeBar;

	private GameObject callBack;

	private bool bStart;

	private void OnEnable()
	{
		NGUITools.SetActive(m_Button, false);
		NGUITools.SetActive(m_ProgressBar, false);
	}

	private void Update()
	{
		if (UserStateHUD.GetInstance().HasDyingRemotePlayerInSight())
		{
			if (!m_Button.activeSelf)
			{
				NGUITools.SetActive(m_Button, true);
			}
			callBack = UserStateHUD.GetInstance().CallBackWhenSave;
			if (callBack == null)
			{
				bStart = false;
				if (m_ProgressBar.activeSelf)
				{
					NGUITools.SetActive(m_ProgressBar, false);
				}
			}
			else if (bStart)
			{
				m_ForeBar.fillAmount = UserStateHUD.GetInstance().GetPercentOfSaveTime();
				AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/Player/saving_teammate");
				if (m_ForeBar.fillAmount == 1f)
				{
					callBack.SendMessage("OnSaveSuccess", SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				NGUITools.SetActive(m_ProgressBar, true);
				m_ForeBar.fillAmount = 0f;
				bStart = true;
			}
		}
		else
		{
			if (m_ProgressBar.activeSelf)
			{
				NGUITools.SetActive(m_ProgressBar, false);
			}
			if (m_Button.activeSelf)
			{
				NGUITools.SetActive(m_Button, false);
			}
		}
	}
}

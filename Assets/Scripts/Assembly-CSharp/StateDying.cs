using UnityEngine;

public class StateDying : MonoBehaviour
{
	private const byte HIGH = 0;

	private const byte MIDDLE = 1;

	private const byte LOW = 2;

	public GameObject m_TextContainer;

	public GameObject m_Text;

	public GameObject m_CountDownContainer;

	public UILabel m_CountDownNumber;

	public GameObject m_RespawnBar;

	public UIFilledSprite m_RespawnForeBar;

	public GameObject m_CountDown;

	public GameObject[] m_Electrocardiogram;

	public GameObject m_ButtonContainer;

	public UILabel m_RespawnGoldLabel;

	public GameObject m_OtherRespawnBar;

	private bool bStart;

	private float startTime;

	private void OnEnable()
	{
		NGUITools.SetActive(m_TextContainer, false);
		NGUITools.SetActive(m_CountDownContainer, false);
		m_ButtonContainer.SetActive(false);
	}

	private void CloseAllElectrocardiogram()
	{
		GameObject[] electrocardiogram = m_Electrocardiogram;
		foreach (GameObject gameObject in electrocardiogram)
		{
			if (gameObject.activeSelf)
			{
				NGUITools.SetActive(gameObject, false);
			}
		}
	}

	private void Update()
	{
		if (UserStateHUD.GetInstance().IsUserDying())
		{
			if (!bStart)
			{
				NGUITools.SetActive(m_TextContainer, true);
				NGUITools.SetActive(m_CountDownContainer, true);
				UITweenX component = m_CountDownContainer.GetComponent<UITweenX>();
				component.Play();
				NGUITools.SetActive(m_OtherRespawnBar, false);
				CloseAllElectrocardiogram();
				bStart = true;
				NGUITools.SetActive(m_Electrocardiogram[0], true);
				startTime = Time.time;
				m_RespawnGoldLabel.text = string.Empty + GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetInstanceRespawnCost();
				return;
			}
			float percentOfUserSave = UserStateHUD.GetInstance().GetPercentOfUserSave();
			if (percentOfUserSave > 0f)
			{
				AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/Player/saving_teammate");
				if (!m_RespawnBar.activeSelf)
				{
					NGUITools.SetActive(m_RespawnBar, true);
				}
				m_RespawnForeBar.fillAmount = percentOfUserSave;
				if (m_Electrocardiogram[2].activeSelf)
				{
					NGUITools.SetActive(m_Electrocardiogram[2], true);
				}
				if (m_Electrocardiogram[1].activeSelf)
				{
					NGUITools.SetActive(m_Electrocardiogram[1], false);
				}
				if (!m_Electrocardiogram[0].activeSelf)
				{
					NGUITools.SetActive(m_Electrocardiogram[0], true);
				}
				if (m_ButtonContainer.activeSelf)
				{
					m_ButtonContainer.SetActive(false);
				}
			}
			else
			{
				if (!m_ButtonContainer.activeSelf && !Arena.GetInstance().IsCurrentSceneArena() && (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || GameApp.GetInstance().GetGameMode().SubModePlay != SubMode.Boss))
				{
					m_ButtonContainer.SetActive(true);
				}
				if (m_RespawnBar.activeSelf)
				{
					NGUITools.SetActive(m_RespawnBar, false);
				}
				float percentOfAliveTime = UserStateHUD.GetInstance().GetPercentOfAliveTime();
				if (percentOfAliveTime > 0.5f)
				{
					if (m_Electrocardiogram[0].activeSelf)
					{
						NGUITools.SetActive(m_Electrocardiogram[0], false);
					}
					if (!m_Electrocardiogram[1].activeSelf)
					{
						NGUITools.SetActive(m_Electrocardiogram[1], true);
					}
				}
				else if (percentOfAliveTime > 0f && !m_Electrocardiogram[0].activeSelf)
				{
					NGUITools.SetActive(m_Electrocardiogram[0], true);
				}
			}
			m_CountDownNumber.text = string.Empty + ((int)UserStateHUD.GetInstance().GetAliveTime() + 1);
		}
		else if (UserStateHUD.GetInstance().IsUserDead())
		{
			m_CountDownNumber.text = "0";
			if (m_Electrocardiogram[0].activeSelf)
			{
				NGUITools.SetActive(m_Electrocardiogram[0], false);
			}
			if (m_Electrocardiogram[1].activeSelf)
			{
				NGUITools.SetActive(m_Electrocardiogram[1], false);
			}
			if (!m_Electrocardiogram[2].activeSelf)
			{
				NGUITools.SetActive(m_Electrocardiogram[2], true);
			}
			if (m_Text.activeSelf)
			{
				NGUITools.SetActive(m_Text, false);
			}
			if (m_ButtonContainer.activeSelf)
			{
				m_ButtonContainer.SetActive(false);
			}
		}
		else
		{
			if (bStart)
			{
				bStart = false;
			}
			if (m_TextContainer.activeSelf)
			{
				NGUITools.SetActive(m_TextContainer, false);
			}
			if (m_CountDownContainer.activeSelf)
			{
				NGUITools.SetActive(m_CountDownContainer, false);
			}
		}
	}
}

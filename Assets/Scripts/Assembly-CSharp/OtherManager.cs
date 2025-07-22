using UnityEngine;

public class OtherManager : MonoBehaviour
{
	public GameObject m_SaveIcon;

	public GameObject m_Achievement;

	public GameObject m_InjuredEffect;

	public void SetAllActiveRecursively(bool state)
	{
		m_SaveIcon.SetActiveRecursively(state);
		m_Achievement.SetActiveRecursively(state);
		m_InjuredEffect.SetActiveRecursively(state);
	}

	public void ShowSaveIcon()
	{
	}

	public void HideSaveIcon()
	{
	}

	public void ShowAchievement()
	{
	}

	public void HideAchievement()
	{
	}
}

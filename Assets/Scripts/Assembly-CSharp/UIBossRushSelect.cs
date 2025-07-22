using UnityEngine;

public class UIBossRushSelect : UIDelegateMenu
{
	public UIBossRush m_UIBossRush;

	public GameObject m_ButtonRookie;

	public GameObject m_ButtonElite;

	private void Awake()
	{
		AddDelegate(m_ButtonRookie);
		AddDelegate(m_ButtonElite);
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (go.Equals(m_ButtonRookie))
		{
			m_UIBossRush.m_RookiePanel.SetActive(true);
			base.gameObject.SetActive(false);
		}
		else if (go.Equals(m_ButtonElite))
		{
			m_UIBossRush.m_ElitePanel.SetActive(true);
			base.gameObject.SetActive(false);
		}
	}
}

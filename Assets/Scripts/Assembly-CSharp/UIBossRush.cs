using UnityEngine;

public class UIBossRush : UIGameMenu
{
	public GameObject m_SelectPanel;

	public GameObject m_RookiePanel;

	public GameObject m_ElitePanel;

	protected override void Awake()
	{
		base.Awake();
		m_SelectPanel.SetActive(true);
		m_RookiePanel.SetActive(false);
		m_ElitePanel.SetActive(false);
	}

	protected override byte InitMask()
	{
		return 2;
	}

	public static void Show()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(42, false, false, false);
	}

	public static void Close()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, false, false, false);
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}
}

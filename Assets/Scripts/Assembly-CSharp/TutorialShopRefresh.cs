using System;
using UnityEngine;

public class TutorialShopRefresh : TutorialScript
{
	public GameObject m_ClickRefresh;

	public GameObject m_ClickYes;

	private GameObject m_RefreshButton;

	private bool bPressedRefresh;

	protected override void OnTutorialStart()
	{
		bPressedRefresh = false;
		m_ClickRefresh.SetActive(true);
		m_ClickYes.SetActive(false);
		m_RefreshButton = ShopUIScript.mInstance.RefreshButton;
		UIEventListener uIEventListener = UIEventListener.Get(m_RefreshButton);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickButton));
		ShopUIScript.mInstance.BlockForRefreshTutorial(true);
		InGameMenuManager.GetInstance().Lock = true;
	}

	protected override void OnTutorialEnd()
	{
		ShopUIScript.mInstance.BlockForRefreshTutorial(false);
		InGameMenuManager.GetInstance().Lock = false;
	}

	private void OnClickButton(GameObject go)
	{
		if (go.Equals(m_RefreshButton))
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.ShopRefresh;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.ShopBuyFirstWeapon) && !IsTutorialOk(TutorialManager.TutorialType.ShopRefresh) && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 28;
	}
}

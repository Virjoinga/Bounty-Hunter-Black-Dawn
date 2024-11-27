using System;
using UnityEngine;

public class TutorialShopSell : TutorialScript
{
	public GameObject m_ClickSellPage;

	public GameObject m_ClickFirstSlot;

	public GameObject m_ClickSellButton;

	private GameObject m_SellButton;

	private bool bInSellPage;

	protected override void OnTutorialStart()
	{
		bInSellPage = false;
		m_ClickSellPage.SetActive(false);
		m_ClickFirstSlot.SetActive(false);
		m_ClickSellButton.SetActive(false);
		m_SellButton = ShopUIScript.mInstance.SellButton;
		UIEventListener uIEventListener = UIEventListener.Get(m_SellButton);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickButton));
		InGameMenuManager.GetInstance().Lock = true;
	}

	protected override void OnTutorialUpdate()
	{
		if (IsNotInSellPage())
		{
			if (!m_ClickSellPage.activeSelf)
			{
				m_ClickSellPage.SetActive(true);
				ShopUIScript.mInstance.BlockAllButton(true);
				ShopUIScript.mInstance.OpenChangeToSellPageButton();
			}
		}
		else if (!bInSellPage)
		{
			bInSellPage = true;
			if (m_ClickSellPage.activeSelf)
			{
				m_ClickSellPage.SetActive(false);
			}
			if (!m_ClickFirstSlot.activeSelf)
			{
				m_ClickFirstSlot.SetActive(true);
			}
			ShopUIScript.mInstance.BlockForSellItemTutorial(true);
		}
		else if (m_ClickFirstSlot.activeSelf && ShopUIScript.mInstance.SelectedItem != null)
		{
			m_ClickFirstSlot.SetActive(false);
			m_ClickSellButton.SetActive(true);
		}
	}

	protected override void OnTutorialEnd()
	{
		ShopUIScript.mInstance.BlockForSellItemTutorial(false);
		InGameMenuManager.GetInstance().Lock = false;
	}

	private bool IsNotInSellPage()
	{
		return ShopUIScript.mInstance.CurrentPage != ShopPageType.Sell;
	}

	private void OnClickButton(GameObject go)
	{
		if (m_ClickSellButton.activeSelf && go.Equals(m_SellButton))
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.ShopBuySell;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.ShopBulletBuy) && !IsTutorialOk(TutorialManager.TutorialType.ShopBuySell) && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 28;
	}
}

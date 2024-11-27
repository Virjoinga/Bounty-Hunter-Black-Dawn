using System;
using UnityEngine;

public class TutorialShopBuyBack : TutorialScript
{
	public GameObject m_ClickShopPage;

	public GameObject m_ClickBuyBackPage;

	public GameObject m_ClickFirstSlot;

	public GameObject m_ClickBuyBackButton;

	private GameObject m_BuyButton;

	private bool bInBuyBackPage;

	protected override void OnTutorialStart()
	{
		bInBuyBackPage = false;
		m_ClickShopPage.SetActive(false);
		m_ClickBuyBackPage.SetActive(false);
		m_ClickFirstSlot.SetActive(false);
		m_ClickBuyBackButton.SetActive(false);
		m_BuyButton = ShopUIScript.mInstance.BuyButton;
		UIEventListener uIEventListener = UIEventListener.Get(m_BuyButton);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickButton));
		InGameMenuManager.GetInstance().Lock = true;
	}

	protected override void OnTutorialUpdate()
	{
		if (IsNotInShopPage())
		{
			if (!m_ClickShopPage.activeSelf)
			{
				m_ClickShopPage.SetActive(true);
				ShopUIScript.mInstance.BlockAllButton(true);
				ShopUIScript.mInstance.OpenChangeToBuyPageButton();
			}
		}
		else if (IsNotInBuyBackPage())
		{
			if (!m_ClickBuyBackPage.activeSelf)
			{
				m_ClickBuyBackPage.SetActive(true);
				m_ClickShopPage.SetActive(false);
				ShopUIScript.mInstance.BlockAllButton(true);
				ShopUIScript.mInstance.OpenChangeToBuyBackPageButton();
			}
		}
		else if (!bInBuyBackPage)
		{
			bInBuyBackPage = true;
			if (m_ClickBuyBackPage.activeSelf)
			{
				m_ClickBuyBackPage.SetActive(false);
			}
			if (!m_ClickFirstSlot.activeSelf)
			{
				m_ClickFirstSlot.SetActive(true);
			}
			ShopUIScript.mInstance.BlockForBuyBackTutorial(true);
		}
		else if (m_ClickFirstSlot.activeSelf && ShopUIScript.mInstance.SelectedItem != null)
		{
			m_ClickFirstSlot.SetActive(false);
			m_ClickBuyBackButton.SetActive(true);
		}
	}

	protected override void OnTutorialEnd()
	{
		ShopUIScript.mInstance.BlockForBuyBackTutorial(false);
		InGameMenuManager.GetInstance().Lock = false;
	}

	private bool IsNotInShopPage()
	{
		return ShopUIScript.mInstance.CurrentPage == ShopPageType.Sell;
	}

	private bool IsNotInBuyBackPage()
	{
		return ShopUIScript.mInstance.CurrentPage != ShopPageType.BuyBack;
	}

	private void OnClickButton(GameObject go)
	{
		if (m_ClickBuyBackButton.activeSelf && go.Equals(m_BuyButton))
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.ShopBuyBack;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.ShopBuySell) && !IsTutorialOk(TutorialManager.TutorialType.ShopBuyBack) && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 28;
	}
}

using System;
using UnityEngine;

public class TutorialShopBulletBuy : TutorialScript
{
	public GameObject m_ClickBullet;

	public GameObject m_ClickBuy;

	private GameObject m_BuyButton;

	protected override void OnTutorialStart()
	{
		m_ClickBullet.SetActive(false);
		m_ClickBuy.SetActive(false);
		m_BuyButton = ShopUIScript.mInstance.BuySMGButton;
		UIEventListener uIEventListener = UIEventListener.Get(m_BuyButton);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickButton));
		InGameMenuManager.GetInstance().Lock = true;
	}

	protected override void OnTutorialUpdate()
	{
		if (IsNotInBulletPage())
		{
			if (!m_ClickBullet.activeSelf)
			{
				m_ClickBullet.SetActive(true);
				ShopUIScript.mInstance.BlockAllButton(true);
				ShopUIScript.mInstance.OpenChangeToBulletPageButton();
			}
		}
		else if (!m_ClickBuy.activeSelf)
		{
			m_ClickBullet.SetActive(false);
			m_ClickBuy.SetActive(true);
			ShopUIScript.mInstance.BlockForBulletTutorial(true);
			ShopUIScript.mInstance.CloseExtendSMGButton();
		}
	}

	private bool IsNotInBulletPage()
	{
		return ShopUIScript.mInstance.CurrentPage != ShopPageType.Bullet;
	}

	protected override void OnTutorialEnd()
	{
		ShopUIScript.mInstance.BlockForBulletTutorial(false);
		InGameMenuManager.GetInstance().Lock = false;
	}

	private void OnClickButton(GameObject go)
	{
		if (go.Equals(m_BuyButton))
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.ShopBulletBuy;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.ShopBulletExtend) && !IsTutorialOk(TutorialManager.TutorialType.ShopBulletBuy) && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 28;
	}
}

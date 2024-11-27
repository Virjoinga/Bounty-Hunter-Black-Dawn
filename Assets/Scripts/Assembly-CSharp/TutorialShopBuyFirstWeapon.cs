using System;
using UnityEngine;

public class TutorialShopBuyFirstWeapon : TutorialScript
{
	public GameObject m_ClickWeaponSlot;

	public GameObject m_ClickBuy;

	private GameObject m_BuyButton;

	protected override void OnTutorialStart()
	{
		m_ClickWeaponSlot.SetActive(true);
		m_ClickBuy.SetActive(false);
		m_BuyButton = ShopUIScript.mInstance.BuyButton;
		UIEventListener uIEventListener = UIEventListener.Get(m_BuyButton);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickButton));
		ShopUIScript.mInstance.BlockForBuyFirstGunTutorial(true);
		InGameMenuManager.GetInstance().Lock = true;
	}

	protected override void OnTutorialUpdate()
	{
		if (m_ClickWeaponSlot.activeSelf && ShopUIScript.mInstance.SelectedItem != null)
		{
			m_ClickWeaponSlot.SetActive(false);
			m_ClickBuy.SetActive(true);
		}
	}

	protected override void OnTutorialEnd()
	{
		ShopUIScript.mInstance.BlockForBuyFirstGunTutorial(false);
		InGameMenuManager.GetInstance().Lock = false;
	}

	private void OnClickButton(GameObject go)
	{
		if (m_ClickBuy.activeSelf && go.Equals(m_BuyButton))
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.ShopBuyFirstWeapon;
	}

	protected override bool IsTutorialCanStart()
	{
		return !IsTutorialOk(TutorialManager.TutorialType.ShopBuyFirstWeapon) && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 28;
	}
}

using System;
using UnityEngine;

public class TutorialShopBulletExtend : TutorialScript
{
	public GameObject m_ClickBullet;

	public GameObject m_ClickExtend;

	public GameObject m_ClickYes;

	private GameObject m_ExtendSMGButton;

	protected override void OnTutorialStart()
	{
		m_ClickBullet.SetActive(false);
		m_ClickExtend.SetActive(false);
		m_ClickYes.SetActive(false);
		InGameMenuManager.GetInstance().Lock = true;
		m_ExtendSMGButton = ShopUIScript.mInstance.ExtendSMGButton;
		UIEventListener uIEventListener = UIEventListener.Get(m_ExtendSMGButton);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickButton));
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
		else if (!m_ClickExtend.activeSelf && !UIMsgBox.instance.IsMessageShow())
		{
			m_ClickBullet.SetActive(false);
			m_ClickExtend.SetActive(true);
			ShopUIScript.mInstance.BlockForBulletTutorial(true);
		}
	}

	private bool IsNotInBulletPage()
	{
		return ShopUIScript.mInstance.CurrentPage != ShopPageType.Bullet;
	}

	private void OnClickButton(GameObject go)
	{
		if (go.Equals(m_ExtendSMGButton))
		{
			EndTutorial();
		}
	}

	protected override void OnTutorialEnd()
	{
		InGameMenuManager.GetInstance().Lock = false;
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.ShopBulletExtend;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.ShopRefresh) && !IsTutorialOk(TutorialManager.TutorialType.ShopBulletExtend) && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 28;
	}
}

using System;
using UnityEngine;

public class TutorialEquipWeapon : TutorialScript
{
	private float TIME_STEP1 = 1000f;

	private float TIME_STEP2 = 2000f;

	private float TIME_STEP3 = 3000f;

	public GameObject shield;

	public GameObject slot;

	public GameObject close;

	private bool isTakeShield;

	protected override void OnTutorialStart()
	{
		shield.SetActive(true);
		slot.SetActive(false);
		close.SetActive(false);
		GameObject closeButton = CloseButton.instance.m_CloseButton;
		UIEventListener uIEventListener = UIEventListener.Get(closeButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickCloseButton));
		isTakeShield = false;
		InGameMenuManager.GetInstance().Lock = true;
		StateBar.instance.Lock = true;
		NGUIBackPackUIScript.mInstance.SetBackPackLockState(true);
	}

	protected override void OnTutorialEnd()
	{
		InGameMenuManager.GetInstance().Lock = false;
		StateBar.instance.Lock = false;
		NGUIBackPackUIScript.mInstance.SetBackPackLockState(false);
	}

	protected override void OnTutorialUpdate()
	{
		if (GameApp.GetInstance().GetUserState().ItemInfoData.IsShieldEquiped)
		{
			if (shield.activeSelf)
			{
				shield.SetActive(false);
			}
			if (slot.activeSelf)
			{
				slot.SetActive(false);
			}
			if (!close.activeSelf)
			{
				close.SetActive(true);
			}
			return;
		}
		if (close.activeSelf)
		{
			close.SetActive(false);
		}
		if (NGUIItemSlot.mDragingSlot != null)
		{
			isTakeShield = true;
		}
		if (isTakeShield)
		{
			if (shield.activeSelf)
			{
				shield.SetActive(false);
			}
			if (!slot.activeSelf)
			{
				slot.SetActive(true);
			}
		}
		else
		{
			if (!shield.activeSelf)
			{
				shield.SetActive(true);
			}
			if (slot.activeSelf)
			{
				slot.SetActive(false);
			}
		}
	}

	private void OnClickCloseButton(GameObject go)
	{
		if (GameApp.GetInstance().GetUserState().ItemInfoData.IsShieldEquiped)
		{
			EndTutorial();
			InGameMenuManager.GetInstance().Close();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.EquipWeapon;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.OpenMenuToEquipWeapon) && !IsTutorialOk(TutorialManager.TutorialType.EquipWeapon) && CloseButton.instance != null;
	}
}

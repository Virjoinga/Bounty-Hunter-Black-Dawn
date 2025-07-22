using System;
using UnityEngine;

public class TutorialOpenMenuToEquipWeapon : TutorialScript
{
	private GameObject button;

	public GameObject content;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
			HUDManager.instance.m_HotKeyManager.ForbidAllWithout(6);
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			button = HUDManager.instance.m_HotKeyManager.m_Menu;
			if (button != null)
			{
				UIEventListener uIEventListener = UIEventListener.Get(button);
				uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressMenuButton));
			}
			if (content != null && button != null)
			{
				content.transform.localPosition = button.transform.localPosition;
			}
		}
	}

	protected override void OnTutorialEnd()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = false;
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = false;
		}
	}

	private void OnPressMenuButton(GameObject go, bool isPressed)
	{
		EndTutorial();
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.OpenMenuToEquipWeapon;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.RotateCamera) && !IsTutorialOk(TutorialManager.TutorialType.OpenMenuToEquipWeapon);
	}
}

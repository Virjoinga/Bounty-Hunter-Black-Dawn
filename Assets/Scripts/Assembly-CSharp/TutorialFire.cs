using System;
using UnityEngine;

public class TutorialFire : TutorialScript
{
	private GameObject button;

	public GameObject shootJoystick;

	protected override void OnTutorialStart()
	{
		if (!(HUDManager.instance != null))
		{
			return;
		}
		HUDManager.instance.m_HotKeyManager.ForbidAll();
		Transform transform = Camera.main.transform.Find("Npc_Collision");
		transform.GetComponent<Collider>().enabled = true;
		button = HUDManager.instance.m_Joystick.m_ShootJoystick.gameObject;
		if (button != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(button.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressMenuButton));
			if (shootJoystick != null)
			{
				shootJoystick.transform.localPosition = button.transform.localPosition;
			}
		}
	}

	protected override void OnTutorialEnd()
	{
		if (HUDManager.instance != null)
		{
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = false;
		}
	}

	private void OnPressMenuButton(GameObject go, bool isPressed)
	{
		if (!isPressed)
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.Fire;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.EquipWeapon) && !IsTutorialOk(TutorialManager.TutorialType.Fire);
	}
}

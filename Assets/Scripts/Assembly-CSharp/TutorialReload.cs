using System;
using UnityEngine;

public class TutorialReload : TutorialScript
{
	public UISprite m_Sprite;

	private GameObject button;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			HUDManager.instance.m_HotKeyManager.ForbidAllWithout(3);
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			button = HUDManager.instance.m_HotKeyManager.m_SwapOrReload;
			m_Sprite.spriteName = UserStateHUD.GetInstance().GetUserWeapons()[0];
			m_Sprite.MakePixelPerfect();
			m_Sprite.transform.localScale *= 1.2f;
			if (button != null)
			{
				UIEventListener uIEventListener = UIEventListener.Get(button);
				uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickMenuButton));
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

	private void OnClickMenuButton(GameObject go)
	{
		EndTutorial();
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.Reload;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.Fire) && !IsTutorialOk(TutorialManager.TutorialType.Reload);
	}
}

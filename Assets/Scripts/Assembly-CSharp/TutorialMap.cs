using System;
using UnityEngine;

public class TutorialMap : TutorialScript
{
	private GameObject button;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
			HUDManager.instance.m_HotKeyManager.ForbidAllWithout(16);
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			button = HUDManager.instance.m_HotKeyManager.m_Map;
			if (button != null)
			{
				UIEventListener uIEventListener = UIEventListener.Get(button);
				uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressMenuButton));
			}
		}
	}

	protected override void OnTutorialEnd()
	{
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.Block = false;
		HUDManager.instance.m_HotKeyManager.CancelFobid();
		Transform transform = Camera.main.transform.Find("Npc_Collision");
		transform.GetComponent<Collider>().enabled = false;
	}

	protected override void OnTutorialUpdate()
	{
		if (InGameMenu.CurrentIndex == 1)
		{
			EndTutorial();
		}
	}

	private void OnPressMenuButton(GameObject go, bool isPressed)
	{
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.Map;
	}

	protected override bool IsTutorialCanStart()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			SceneConfig currentSceneConfig = gameWorld.GetLocalPlayer().GetCurrentSceneConfig();
			return currentSceneConfig.FatherSceneID != currentSceneConfig.SceneID && !IsTutorialOk(TutorialManager.TutorialType.Map);
		}
		return false;
	}
}

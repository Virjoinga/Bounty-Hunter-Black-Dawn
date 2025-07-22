using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("GameEvent")]
[HutongGames.PlayMaker.Tooltip("Custom Action...")]
public class FrShop : FsmStateAction
{
	public GameObject m_ShopUI;

	public override void Reset()
	{
		m_ShopUI = null;
	}

	public override void OnEnter()
	{
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript.FrGetCurrentPhase() != 28)
		{
			InGameMenuManager.GetInstance().ShowMenuForShop();
		}
	}

	public override void OnUpdate()
	{
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript.FrGetCurrentPhase() != 28)
		{
			Finish();
		}
	}
}

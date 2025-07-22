using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
[Tooltip("Custion Action...")]
public class FrGoldFruitMachine : FsmStateAction
{
	public FsmEvent closeEvent;

	public override void Reset()
	{
		closeEvent = null;
	}

	public override void OnEnter()
	{
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckSubQuestCompleted(101))
		{
			UIFruitMachine.Show(GambleType.GoldFruitMachine);
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckSubQuestCompleted(101))
		{
			InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
			if (inGameUIScript.FrGetCurrentPhase() != 34)
			{
				base.Fsm.DelayedEvent(closeEvent, 0.5f);
			}
		}
		else
		{
			base.Fsm.DelayedEvent(closeEvent, 2f);
		}
	}

	public override string ErrorCheck()
	{
		return null;
	}
}

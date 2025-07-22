using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
[Tooltip("Custom Action...")]
public class FrQuest : FsmStateAction
{
	private QuestScript questScript;

	public override void OnEnter()
	{
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		questScript = base.Owner.GetComponent<QuestScript>();
		UIBubble.m_npcId = (short)questScript.GetNpcId();
		UIBubble.m_text = string.Empty;
		inGameUIScript.FrGoToPhase(21, false, true, false);
	}

	public override void OnUpdate()
	{
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript.FrGetCurrentPhase() != 21)
		{
			Finish();
		}
	}
}

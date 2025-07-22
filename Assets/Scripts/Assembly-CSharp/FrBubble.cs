using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
public class FrBubble : FsmStateAction
{
	[RequiredField]
	public FsmString stringValue;

	public override void Reset()
	{
		stringValue = string.Empty;
	}

	public override void OnEnter()
	{
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		UIBubble.m_text = LocalizationManager.GetInstance().GetString(stringValue.Value);
		UIBubble.m_bubbleState = NpcBubbleState.TALK_STATE;
		QuestScript component = base.Owner.gameObject.GetComponent<QuestScript>();
		UIBubble.m_npcId = (short)component.GetNpcId();
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

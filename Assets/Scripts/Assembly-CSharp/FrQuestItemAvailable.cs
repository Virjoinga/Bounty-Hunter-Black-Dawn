using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
[Tooltip("Custom Action...")]
public class FrQuestItemAvailable : FsmStateAction
{
	[UIHint(UIHint.Variable)]
	[RequiredField]
	public FsmInt ItemId;

	public FsmEvent equalTrueEvent;

	public FsmEvent notEqualTrueEvent;

	[Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
	public bool everyFrame;

	public override void Reset()
	{
		equalTrueEvent = null;
		notEqualTrueEvent = null;
	}

	public override void OnEnter()
	{
		DoQuestItemAvailable();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoQuestItemAvailable();
	}

	private void DoQuestItemAvailable()
	{
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailable(ItemId.Value))
		{
			if (equalTrueEvent != null)
			{
				base.Fsm.Event(equalTrueEvent);
			}
		}
		else if (notEqualTrueEvent != null)
		{
			base.Fsm.Event(notEqualTrueEvent);
		}
	}
}

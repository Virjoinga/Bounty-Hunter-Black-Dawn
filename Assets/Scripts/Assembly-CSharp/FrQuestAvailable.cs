using HutongGames.PlayMaker;

[Tooltip("Custom Action...")]
[ActionCategory("GameEvent")]
public class FrQuestAvailable : FsmStateAction
{
	public FsmEvent equalTrueEvent;

	public FsmEvent notEqualTrueEvent;

	public override void Reset()
	{
		equalTrueEvent = null;
		notEqualTrueEvent = null;
	}

	public override void OnEnter()
	{
		DoQuestAvailable();
	}

	private void DoQuestAvailable()
	{
		QuestScript component = base.Owner.GetComponent<QuestScript>();
		if (component.UpdateFlag())
		{
			base.Fsm.Event(equalTrueEvent);
		}
		else
		{
			base.Fsm.Event(notEqualTrueEvent);
		}
	}
}

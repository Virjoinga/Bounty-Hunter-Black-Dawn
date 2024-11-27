using HutongGames.PlayMaker;

[ActionCategory("Script/GameEvent")]
public class FrQuestItemsEnough : FsmStateAction
{
	public FsmEvent equalTrueEvent;

	public FsmEvent notEqualTrueEvent;

	[Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
	public bool everyFrame;

	private int m_itemID;

	public override void Reset()
	{
		equalTrueEvent = null;
		notEqualTrueEvent = null;
	}

	public override void OnEnter()
	{
		m_itemID = base.Owner.GetComponent<ExplorItemList>().ExplorItemIDs[0];
		DoQuestItemsEnough();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoQuestItemsEnough();
	}

	private void DoQuestItemsEnough()
	{
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsEnough(m_itemID))
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

using System.Collections.Generic;
using HutongGames.PlayMaker;

[Tooltip("Custom Action...")]
[ActionCategory("GameEvent")]
public class FrGainQuestItems : FsmStateAction
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
		ExplorItemList component = base.Owner.GetComponent<ExplorItemList>();
		int num = component.ExplorItemIDs[0];
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PickUpQuestItemRequest request = new PickUpQuestItemRequest((short)num);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		else
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			List<QuestState> accStateLst = userState.m_questStateContainer.m_accStateLst;
			for (int i = 0; i < accStateLst.Count; i++)
			{
				QuestState questState = accStateLst[i];
				if (questState.m_conter.ContainsKey(num))
				{
					GameApp.GetInstance().GetUserState().ItemInfoData.AddStoryItem((short)num);
					GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressItemCollection((short)num);
					break;
				}
			}
		}
		if (component.m_DestroyAfterFinished)
		{
			base.Fsm.Event(equalTrueEvent);
		}
		else
		{
			base.Fsm.Event(notEqualTrueEvent);
		}
	}
}

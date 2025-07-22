using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("GameEvent")]
public class FrDeactiveEffect : FsmStateAction
{
	public override void OnEnter()
	{
		GameObject effectObject = base.Owner.GetComponent<ExplorItemList>().EffectObject;
		ExplorItemList component = base.Owner.GetComponent<ExplorItemList>();
		QuestManager.GetInstance().m_questLst[component.m_questId].SetQuestPoint(component.ExplorItemIDs[0], component.m_posId, QuestPointState.disable);
		effectObject.SetActiveRecursively(false);
		byte b = 0;
		ExplorItemBlockScript component2 = component.transform.parent.GetComponent<ExplorItemBlockScript>();
		if (component2 != null)
		{
			b = component2.BlockID;
		}
		Debug.Log("BlockID-----" + b);
		ExploreItemStatesInfo exploreItemStatesInfo = new ExploreItemStatesInfo();
		exploreItemStatesInfo.mQuestID = (short)component.m_questId;
		if (component.m_DestroyAfterFinished)
		{
			exploreItemStatesInfo.mState = ExploreItemStates.OBJECT_DISABLE;
		}
		else
		{
			exploreItemStatesInfo.mState = ExploreItemStates.EFFECT_INVISIBLE;
		}
		GameApp.GetInstance().GetGameWorld().UpdateExploreItem(b, component.ID_in_Block, exploreItemStatesInfo);
		Finish();
	}
}

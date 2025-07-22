using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
public class SendItemExploredRequest : FsmStateAction
{
	public override void OnEnter()
	{
		DoSendItemExploredRequest();
	}

	private void DoSendItemExploredRequest()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			byte blockID = base.Owner.transform.parent.gameObject.GetComponent<ExplorItemBlockScript>().BlockID;
			byte iD_in_Block = base.Owner.GetComponent<ExplorItemList>().ID_in_Block;
			if (base.Owner.GetComponent<ExplorItemList>().EffectObject.activeSelf)
			{
				ExploreItemStates state = ExploreItemStates.EFFECT_INVISIBLE;
				if (base.Owner.GetComponent<ExplorItemList>().m_DestroyAfterFinished)
				{
					state = ExploreItemStates.OBJECT_DISABLE;
				}
				ItemExploredRequest request = new ItemExploredRequest(blockID, iD_in_Block, state, (short)base.Owner.GetComponent<ExplorItemList>().m_questId);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				Finish();
			}
		}
		else
		{
			Finish();
		}
	}
}

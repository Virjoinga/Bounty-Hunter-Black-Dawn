using UnityEngine;

public class NpcIdleState : NpcState
{
	public override void NextState(Npc npc, float deltaTime)
	{
		if (!npc.IsPlayingAnimation())
		{
			if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() != 21 && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() != 17)
			{
				npc.TurnRound(npc.m_initRotation);
			}
			int num = 1;
			int num2 = Random.Range(0, 100);
			num = ((!Npc.IdelRate.ContainsKey(npc.Name)) ? Random.Range(1, 3) : ((num2 <= Npc.IdelRate[npc.Name][0]) ? 1 : 2));
			if (Vector3.Angle(npc.GetTransform().forward, npc.m_initForward) < 10f)
			{
				npc.PlayAnimationAllLayers(AnimationString.NPC_IDLE + string.Format("{0:D2}", num), WrapMode.Once);
			}
		}
	}
}

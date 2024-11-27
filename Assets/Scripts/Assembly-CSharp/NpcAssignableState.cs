public class NpcAssignableState : NpcState
{
	public override void NextState(Npc npc, float deltaTime)
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		npc.TurnRound(localPlayer.GetPosition());
		if (npc.AnimationPlayed(AnimationString.NPC_ASSIGNABLE, 1f))
		{
			npc.StopAnimation();
			npc.State = Npc.IDLE_STATE;
			npc.StartTurnRoundTime();
		}
	}
}
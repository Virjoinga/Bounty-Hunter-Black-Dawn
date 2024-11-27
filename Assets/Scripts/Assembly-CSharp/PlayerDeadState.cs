using UnityEngine;

public class PlayerDeadState : PlayerState, EffectsCameraListener
{
	private Player player;

	public override void NextState(Player player, float deltaTime)
	{
		if (player.IsLocal())
		{
			LocalPlayer localPlayer = (LocalPlayer)player;
			localPlayer.m_deadRotateAngle += 200f * Time.deltaTime;
			if (player.DeadAnimationCompleted())
			{
				player.LoadSceneOnDead();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					PlayerRebirthRequest request = new PlayerRebirthRequest(-1);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				else if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
				{
					EnemySpawnScript.GetInstance().OnArenaGameEnd(player, false);
				}
				else if (player.IsDeadJustNow)
				{
					AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Easy_Task, AchievementTrigger.Type.Stop);
					AchievementManager.GetInstance().Trigger(trigger);
					player.IsDeadJustNow = false;
					this.player = player;
					EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
				}
			}
		}
		else
		{
			player.PlayAnimationAllLayers(AnimationString.DeadTPS, WrapMode.ClampForever);
		}
	}

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		switch (type)
		{
		case EffectsCamera.Type.FadeIn:
			EffectsCamera.instance.StartEffect(EffectsCamera.Type.Teleport, this);
			break;
		case EffectsCamera.Type.Teleport:
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerRebirthRequest request = new PlayerRebirthRequest(player.CurrentRespawnPointID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else
			{
				player.GetCharacterController().enabled = false;
				player.ReSpawnAtPoint(player.CurrentRespawnPointID);
				GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
				if (gameWorld.BossState == EBossState.BATTLE)
				{
					gameWorld.ChangeSubModeFromBossToStory();
					gameWorld.OnLoseBossBattle();
				}
			}
			player = null;
			break;
		}
	}
}

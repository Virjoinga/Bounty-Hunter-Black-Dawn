using System;
using UnityEngine;

public class DyingState : PlayerState
{
	protected Timer dyingTimer = new Timer();

	protected Player player;

	protected Vector3 from;

	public bool InDyingState { get; set; }

	public DyingState(Player player)
	{
		this.player = player;
	}

	public void OnDying()
	{
		InDyingState = true;
		dyingTimer.SetTimer(player.DyingTimeLength, false);
		if (player.IsLocal())
		{
			GameApp.GetInstance().GetUserState().QuestInfo.UpdateDeadTime(GameApp.GetInstance().GetUserState().GetCurrentQuest());
		}
	}

	public void OnRecoverFromDying()
	{
		InDyingState = false;
		player.Hp = (int)((float)player.MaxHp * player.RebirthHealthPercentage);
		player.Shield = player.MaxShield;
		if (player.IsLocal())
		{
			LocalPlayer localPlayer = (LocalPlayer)player;
			localPlayer.RestoreEntityLocalPosition();
			AudioManager.GetInstance().PlaySound("RPG_Audio/Player/saved_teammate");
		}
		player.StartUnhurt();
	}

	public void OnRecoverFromDying(int hp, int shield)
	{
		InDyingState = false;
		player.Hp = Math.Max(Math.Min(hp, player.MaxHp), 0);
		player.Shield = Math.Max(Math.Min(shield, player.MaxShield), 0);
		if (player.IsLocal())
		{
			LocalPlayer localPlayer = (LocalPlayer)player;
			localPlayer.RestoreEntityLocalPosition();
			AudioManager.GetInstance().PlaySound("RPG_Audio/Player/saved_teammate");
		}
		player.StartUnhurt();
	}

	public void SetInitPos(Vector3 pos)
	{
		from = pos;
	}

	public override void NextState(Player player, float deltaTime)
	{
		if (player.IsLocal() && InDyingState && dyingTimer.Ready())
		{
			OnDead();
		}
	}

	public void OnDead()
	{
		if (player.IsLocal())
		{
			LocalPlayer localPlayer = (LocalPlayer)player;
			localPlayer.GetTransform().rotation = Quaternion.Euler(0f, localPlayer.GetTransform().rotation.eulerAngles.y, 0f);
			localPlayer.RestoreEntityLocalPosition();
		}
		player.IsDeadJustNow = true;
		player.OnDead();
		player.SetState(Player.DEAD_STATE);
		InDyingState = false;
		dyingTimer.Do();
	}

	public Timer GetDyingTimer()
	{
		return dyingTimer;
	}
}

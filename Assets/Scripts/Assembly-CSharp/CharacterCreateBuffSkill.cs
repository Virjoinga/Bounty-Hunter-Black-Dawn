using System.Collections.Generic;
using UnityEngine;

internal class CharacterCreateBuffSkill : CharacterInstantSkill
{
	private List<CharacterStateSkill> stateSkillList = new List<CharacterStateSkill>();

	public float Duration { get; set; }

	public float DurationInit { get; set; }

	public void AddStateBuff(CharacterStateSkill skill)
	{
		bool flag = true;
		for (int i = 0; i < stateSkillList.Count; i++)
		{
			if (stateSkillList[i].BuffType == skill.BuffType)
			{
				if (stateSkillList[i].SkillLevel > skill.SkillLevel)
				{
					flag = false;
					continue;
				}
				stateSkillList.Remove(stateSkillList[i]);
				i--;
			}
		}
		if (flag)
		{
			skill.Duration = Duration;
			skill.IconName = base.IconName;
			stateSkillList.Add(skill);
		}
	}

	public override void ApplySkill(GameUnit unit)
	{
		ApplySkill(unit, null);
	}

	public void ApplySkill(GameUnit unit, GameUnit target)
	{
		if (!CanApplySkill())
		{
			return;
		}
		if (base.skillID == 1004)
		{
			if (GameApp.GetInstance().GetUserState().GetSex() == Sex.M)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Player/Soldier_M_Skill");
			}
			else
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Player/Soldier_F_Skill");
			}
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Hulk, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger);
		}
		if (base.Target == SkillTarget.Enemy)
		{
			if (base.SEffectType == SkillEffectType.CreateBuff)
			{
				Enemy enemy = target as Enemy;
				if (enemy != null)
				{
					CharacterSkillManager characterSkillManager = enemy.GetCharacterSkillManager();
					foreach (CharacterStateSkill stateSkill in stateSkillList)
					{
						if (!characterSkillManager.GetStateSkillList().Contains(stateSkill))
						{
							characterSkillManager.AddSkill(stateSkill);
						}
						stateSkill.StartBuff();
						byte speedDownRate = (byte)stateSkill.BuffValueY1;
						if (stateSkill.BuffValueY1 > 0f)
						{
							speedDownRate = (byte)(0f - stateSkill.BuffValueY1);
						}
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && stateSkill.FunctionType1 == BuffFunctionType.SpeedDown)
						{
							EnemySpeedDownRequest request = new EnemySpeedDownRequest(enemy.PointID, enemy.EnemyID, stateSkill.skillID, (short)(stateSkill.Duration * 10f), speedDownRate);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
					}
				}
				else
				{
					RemotePlayer remotePlayer = target as RemotePlayer;
					if (remotePlayer != null)
					{
						foreach (CharacterStateSkill stateSkill2 in stateSkillList)
						{
							ChangeRemotePlayerStateRequest request2 = new ChangeRemotePlayerStateRequest(remotePlayer.GetUserID(), stateSkill2, base.skillID);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						}
					}
				}
			}
		}
		else
		{
			Player player = unit as Player;
			if (base.SEffectType == SkillEffectType.CreateBuff)
			{
				CharacterSkillManager characterSkillManager2 = player.GetCharacterSkillManager();
				foreach (CharacterStateSkill stateSkill3 in stateSkillList)
				{
					if (!characterSkillManager2.GetStateSkillList().Contains(stateSkill3))
					{
						Debug.Log("Buff Start!!");
						characterSkillManager2.AddSkill(stateSkill3);
					}
					stateSkill3.StartBuff();
				}
			}
		}
		lastCastTime = Time.time;
	}

	public void RefreshBuffDuration()
	{
		foreach (CharacterStateSkill stateSkill in stateSkillList)
		{
			stateSkill.Duration = Duration;
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class CharacterMorphineSkill : CharacterInstantSkill
{
	private List<CharacterStateSkill> stateSkillList = new List<CharacterStateSkill>();

	public float EffectValueX2 { get; set; }

	public float EffectValueY2 { get; set; }

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

	public void RefreshBuffDuration()
	{
		foreach (CharacterStateSkill stateSkill in stateSkillList)
		{
			stateSkill.Duration = Duration;
		}
	}

	public override void ApplySkill(GameUnit unit)
	{
		if (!CanApplySkill())
		{
			return;
		}
		LocalPlayer localPlayer = unit as LocalPlayer;
		if (localPlayer != null && base.SEffectType == SkillEffectType.Morphine)
		{
			int num = Mathf.CeilToInt((float)localPlayer.MaxHp * base.EffectValueX + base.EffectValueY);
			int hp = localPlayer.Hp;
			if (-num >= hp)
			{
				num = -(hp - 1);
			}
			localPlayer.RecoverHP(num);
			CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
			foreach (CharacterStateSkill stateSkill in stateSkillList)
			{
				if (!characterSkillManager.GetStateSkillList().Contains(stateSkill))
				{
					Debug.Log("Buff Start!!");
					characterSkillManager.AddSkill(stateSkill);
				}
				stateSkill.StartBuff();
			}
			localPlayer.IsInMorphine = true;
		}
		lastCastTime = Time.time;
	}
}

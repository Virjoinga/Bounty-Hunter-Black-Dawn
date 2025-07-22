using UnityEngine;

public class CharacterKeepHealingSkill : CharacterInstantSkill
{
	public override void ApplySkill(GameUnit unit)
	{
		if (CanApplySkill())
		{
			LocalPlayer localPlayer = unit as LocalPlayer;
			if (base.SEffectType == SkillEffectType.KeepHealing)
			{
				HpRecoverTimer hpRecoverTimer = new HpRecoverTimer();
				hpRecoverTimer.SkillID = base.skillID;
				hpRecoverTimer.RecoverPercentage = base.EffectValueX;
				hpRecoverTimer.TotalCount = Mathf.CeilToInt(base.EffectValueY);
				hpRecoverTimer.TriggeredCount = 0;
				hpRecoverTimer.timer.SetTimer(1f, false);
				localPlayer.AddHpRecoverTimer(hpRecoverTimer);
			}
			lastCastTime = Time.time;
		}
	}
}

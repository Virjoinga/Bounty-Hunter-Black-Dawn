using UnityEngine;

public class CharacterExtraShieldSkill : CharacterInstantSkill
{
	public override void ApplySkill(GameUnit unit)
	{
		if (!CanApplySkill())
		{
			return;
		}
		LocalPlayer localPlayer = unit as LocalPlayer;
		if (localPlayer != null && base.SEffectType == SkillEffectType.CreateShield)
		{
			if (localPlayer.HealingEffect == null)
			{
				localPlayer.HealingEffect = EffectPlayer.GetInstance().PlayHealingEffect();
				localPlayer.IsInstantHealing = false;
			}
			localPlayer.CreateExtraShield(Mathf.CeilToInt((float)localPlayer.MaxHp * base.EffectValueX * 0.01f + base.EffectValueY), 10f);
			HpRecoverTimer hpRecoverTimer = new HpRecoverTimer();
			hpRecoverTimer.SkillID = base.skillID;
			hpRecoverTimer.RecoverPercentage = 0.1f;
			hpRecoverTimer.TotalCount = 10;
			hpRecoverTimer.TriggeredCount = 0;
			hpRecoverTimer.timer.SetTimer(1f, false);
			localPlayer.AddHpRecoverTimer(hpRecoverTimer);
		}
		lastCastTime = Time.time;
	}
}

using UnityEngine;

public class CharacterBulletRecoverByTimeSkill : CharacterInstantSkill
{
	public override void ApplySkill(GameUnit unit)
	{
		Debug.Log("Apply!");
		if (!CanApplySkill())
		{
			return;
		}
		LocalPlayer localPlayer = unit as LocalPlayer;
		if (base.SEffectType == SkillEffectType.RecoverBullet)
		{
			BulletRecoverTimer bulletRecoverTimer = new BulletRecoverTimer();
			bulletRecoverTimer.SkillID = base.skillID;
			foreach (SkillTargetType targetType in base.TargetTypes)
			{
				bulletRecoverTimer.wTypes.Add(targetType);
			}
			bulletRecoverTimer.TotalCount = 6;
			bulletRecoverTimer.TriggeredCount = 0;
			bulletRecoverTimer.RecoverPercentageTick = Mathf.CeilToInt(base.EffectValueX);
			bulletRecoverTimer.timer.SetTimer(10f, false);
			localPlayer.AddBulletRecoverTimer(bulletRecoverTimer);
		}
		lastCastTime = Time.time;
	}
}

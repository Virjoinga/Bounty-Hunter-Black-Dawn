using System.Collections.Generic;
using UnityEngine;

public class CharacterInstantSkill : CharacterSkill
{
	protected float lastCastTime = -9999f;

	public float CoolDownTime { get; set; }

	public float CoolDownTimeInit { get; set; }

	public SkillTriggerType STriggerType { get; set; }

	public short STriggerTypeSubValue { get; set; }

	public SkillEffectType SEffectType { get; set; }

	public float EffectValueX { get; set; }

	public float EffectValueY { get; set; }

	public float CastingTime { get; set; }

	public bool ApplySuccess { get; set; }

	public override void ApplySkill(GameUnit unit)
	{
		ApplySkill(unit, unit.GetPosition());
	}

	public void ApplySkill(GameUnit unit, Vector3 applyPosition)
	{
		if (!CanApplySkill())
		{
			return;
		}
		if (SEffectType == SkillEffectType.Heal)
		{
			LocalPlayer localPlayer = unit as LocalPlayer;
			if (localPlayer != null)
			{
				EffectPlayer.GetInstance().PlayHealingWave(applyPosition + Vector3.up * 1f);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					PlayThirdPersonSkillEffectRequest request = new PlayThirdPersonSkillEffectRequest(PlayThirdPersonSkillEffectRequest.SkillEffectType.HealingWave);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				if (Vector3.Distance(localPlayer.GetPosition(), applyPosition) < (float)base.Range)
				{
					localPlayer.RecoverHP((float)localPlayer.MaxHp * EffectValueX + EffectValueY);
				}
				if (base.Target == SkillTarget.Partner)
				{
					List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
					foreach (RemotePlayer item in remotePlayers)
					{
						if ((GameApp.GetInstance().GetGameMode().IsCoopMode() || item.IsSameTeam(localPlayer)) && item.CurrentCityID == GameApp.GetInstance().GetUserState().GetCurrentCityID() && item.CurrentSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID)
						{
							float num = Vector3.Distance(item.GetPosition(), applyPosition);
							if (num <= (float)base.Range)
							{
								item.RecoveHP((float)item.MaxHp * EffectValueX + EffectValueY);
							}
						}
					}
				}
			}
		}
		else if (SEffectType == SkillEffectType.RecoverShield)
		{
			Debug.Log(unit.MaxShield + " +++ " + EffectValueX);
			LocalPlayer localPlayer2 = unit as LocalPlayer;
			if (localPlayer2 != null)
			{
				localPlayer2.RecoverShiled((float)localPlayer2.MaxShield * EffectValueX + EffectValueY);
			}
		}
		else if (SEffectType != SkillEffectType.Rush)
		{
			if (SEffectType == SkillEffectType.ExtraBuff)
			{
				Player player = unit as Player;
				if (player != null)
				{
					short num2 = (short)EffectValueX;
					short buffID = (short)EffectValueY;
					CharacterStateSkill characterStateSkill = new CharacterStateSkill(buffID);
					characterStateSkill.Target = base.Target;
					characterStateSkill.TargetTypes = base.TargetTypes;
					List<CharacterInstantSkill> initiativeSkillList = player.GetCharacterSkillManager().GetInitiativeSkillList();
					for (int i = 0; i < initiativeSkillList.Count; i++)
					{
						CharacterCreateBuffSkill characterCreateBuffSkill = initiativeSkillList[i] as CharacterCreateBuffSkill;
						if (characterCreateBuffSkill != null && characterCreateBuffSkill.skillID == num2)
						{
							Debug.Log("ExtraBuff!!!");
							characterCreateBuffSkill.AddStateBuff(characterStateSkill);
							break;
						}
					}
				}
			}
			else if (SEffectType == SkillEffectType.ChangeCD)
			{
				Player player2 = unit as Player;
				if (player2 != null)
				{
					short num3 = (short)EffectValueX;
					List<CharacterInstantSkill> initiativeSkillList2 = player2.GetCharacterSkillManager().GetInitiativeSkillList();
					for (int j = 0; j < initiativeSkillList2.Count; j++)
					{
						CharacterInstantSkill characterInstantSkill = initiativeSkillList2[j];
						if (initiativeSkillList2[j].skillID == num3)
						{
							initiativeSkillList2[j].CoolDownTime = initiativeSkillList2[j].CoolDownTimeInit - EffectValueY;
							break;
						}
					}
				}
			}
			else if (SEffectType == SkillEffectType.ChangeSummonDuration)
			{
				Player player3 = unit as Player;
				if (player3 != null)
				{
					short num4 = (short)EffectValueX;
					List<CharacterInstantSkill> initiativeSkillList3 = player3.GetCharacterSkillManager().GetInitiativeSkillList();
					for (int k = 0; k < initiativeSkillList3.Count; k++)
					{
						CharacterSummonSkill characterSummonSkill = initiativeSkillList3[k] as CharacterSummonSkill;
						if (characterSummonSkill != null && characterSummonSkill.skillID == num4)
						{
							characterSummonSkill.Duration = characterSummonSkill.DurationInit + EffectValueY;
							break;
						}
					}
				}
			}
			else if (SEffectType == SkillEffectType.ChangeBuffDuration)
			{
				Player player4 = unit as Player;
				if (player4 != null)
				{
					short num5 = (short)EffectValueX;
					List<CharacterInstantSkill> initiativeSkillList4 = player4.GetCharacterSkillManager().GetInitiativeSkillList();
					for (int l = 0; l < initiativeSkillList4.Count; l++)
					{
						CharacterCreateBuffSkill characterCreateBuffSkill2 = initiativeSkillList4[l] as CharacterCreateBuffSkill;
						if (characterCreateBuffSkill2 != null && characterCreateBuffSkill2.skillID == num5)
						{
							characterCreateBuffSkill2.Duration = characterCreateBuffSkill2.DurationInit + EffectValueY;
							characterCreateBuffSkill2.RefreshBuffDuration();
							break;
						}
					}
				}
			}
			else if (SEffectType != SkillEffectType.CannonMove_Explosion)
			{
			}
		}
		lastCastTime = Time.time;
	}

	public override bool IsInstantSkill()
	{
		return true;
	}

	public bool CoolDown()
	{
		return Time.time - lastCastTime > CoolDownTime;
	}

	public bool CanApplySkill()
	{
		ApplySuccess = false;
		if (!CoolDown())
		{
			if (base.SkillType == SkillTypes.InitiativeSkill)
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_SKILL_COOLDOWN"));
			}
			return false;
		}
		if (base.SkillType == SkillTypes.InitiativeSkill && GameApp.GetInstance().GetGameScene() != null && GameApp.GetInstance().GetGameScene().mapType == MapType.City)
		{
			UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_SKILL_IN_CITY"));
			return false;
		}
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer.State == Player.FALL_DOWN_STATE || localPlayer.State == Player.DEAD_STATE || (localPlayer.DYING_STATE != null && localPlayer.InDyingState()))
		{
			if (base.SkillType == SkillTypes.InitiativeSkill)
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_SKILL_FORBIDDEN"));
			}
			return false;
		}
		ApplySuccess = true;
		return true;
	}

	public int GetCoolDownLeftTime()
	{
		return Mathf.CeilToInt(GetCoolDownLeftTimeMS());
	}

	public float GetCoolDownLeftTimeMS()
	{
		float num = CoolDownTime - (Time.time - lastCastTime);
		if (num < 0f)
		{
			num = 0f;
		}
		return num;
	}

	public void ResetCD()
	{
		lastCastTime = -9999f;
	}
}

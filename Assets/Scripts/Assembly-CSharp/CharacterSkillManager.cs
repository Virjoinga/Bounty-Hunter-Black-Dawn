using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillManager
{
	private Player player;

	private List<CharacterInstantSkill> initiativeSkills = new List<CharacterInstantSkill>();

	private List<CharacterInstantSkill> triggerSkills = new List<CharacterInstantSkill>();

	private List<CharacterStateSkill> stateSkills = new List<CharacterStateSkill>();

	private int LastShieldValue;

	private float LastShieldPercentage;

	private int LastHPValue;

	private float LastHPPercentage;

	public void LoadCharacterSkills(Player _player)
	{
		player = _player;
		if (!player.IsLocal())
		{
			return;
		}
		int num = 3;
		int num2 = 4;
		int num3 = 2;
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		if (skillTreeManager != null)
		{
			skillTreeManager.InitSkillIDs();
			AddSkillByID(skillTreeManager.GetSkillId(-1, -1, -1), 1);
			for (int i = 0; i < num2; i++)
			{
				skillTreeManager.SkillLayer[i].LoadSkill(this);
			}
			if (skillTreeManager.FinalSkillSlot.GetSkillID() != 0 && skillTreeManager.FinalSkillSlot.GetLevel() > 0)
			{
				AddSkillByID(skillTreeManager.FinalSkillSlot.GetSkillID(), skillTreeManager.FinalSkillSlot.GetLevel());
			}
		}
	}

	public void ApplyAllStateSkills(GameUnit unit)
	{
		RemoveTimeUpStateKills();
		foreach (CharacterStateSkill stateSkill in stateSkills)
		{
			stateSkill.ApplySkill(unit);
		}
	}

	public void RemoveTimeUpStateKills()
	{
		for (int num = stateSkills.Count - 1; num >= 0; num--)
		{
			if (stateSkills[num].TimeUp())
			{
				if (stateSkills[num].skillID == 31001)
				{
					AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Hulk, AchievementTrigger.Type.Stop);
					AchievementManager.GetInstance().Trigger(trigger);
				}
				if (stateSkills[num].skillID == 31076)
				{
					GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
						.IsInMorphine = false;
				}
				if (stateSkills[num].BuffValueY1 < 0f && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					byte effectType = 0;
					if (stateSkills[num].PropertyChangeTypeOfBuff1 == PropertyChangeType.ChangeMoveSpeed)
					{
						effectType = 8;
					}
					ClearRemotePlayerBuffEffectRequest request = new ClearRemotePlayerBuffEffectRequest(effectType);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				stateSkills.RemoveAt(num);
			}
		}
	}

	public void RemoveAllBuff()
	{
		for (int num = stateSkills.Count - 1; num >= 0; num--)
		{
			if (!stateSkills[num].IsPermanent)
			{
				if (stateSkills[num].skillID == 31001)
				{
					AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Hulk, AchievementTrigger.Type.Stop);
					AchievementManager.GetInstance().Trigger(trigger);
				}
				if (stateSkills[num].skillID == 31076)
				{
					GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
						.IsInMorphine = false;
				}
				if (stateSkills[num].BuffValueY1 < 0f && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					byte effectType = 0;
					if (stateSkills[num].PropertyChangeTypeOfBuff1 == PropertyChangeType.ChangeMoveSpeed)
					{
						effectType = 8;
					}
					ClearRemotePlayerBuffEffectRequest request = new ClearRemotePlayerBuffEffectRequest(effectType);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				stateSkills.RemoveAt(num);
			}
		}
	}

	public List<CharacterInstantSkill> GetInitiativeSkillList()
	{
		return initiativeSkills;
	}

	public List<CharacterInstantSkill> GetTriggerSkillList()
	{
		return triggerSkills;
	}

	public List<CharacterStateSkill> GetStateSkillList()
	{
		return stateSkills;
	}

	public void OnEnemyKillTrigger(GameUnit unit)
	{
		foreach (CharacterInstantSkill triggerSkill in triggerSkills)
		{
			if (triggerSkill != null && triggerSkill.STriggerType == SkillTriggerType.OnKillEnemy)
			{
				triggerSkill.ApplySkill(unit);
			}
		}
	}

	public void OnEnemyCriticalKillTrigger(GameUnit unit)
	{
		foreach (CharacterInstantSkill triggerSkill in triggerSkills)
		{
			if (triggerSkill != null && triggerSkill.STriggerType == SkillTriggerType.OnCriticalKillEnemy)
			{
				triggerSkill.ApplySkill(unit);
			}
		}
	}

	public void OnHitFriendsTrigger(GameUnit unit, WeaponType wType, Vector3 applyPosition)
	{
		Debug.Log(triggerSkills.Count);
		foreach (CharacterInstantSkill triggerSkill in triggerSkills)
		{
			if (triggerSkill == null)
			{
				continue;
			}
			Debug.Log(triggerSkill.STriggerType.ToString());
			if (triggerSkill.STriggerType == SkillTriggerType.OnHitFriends)
			{
				Debug.Log(wType.ToString() + "----" + triggerSkill.TargetTypes.Count);
				if (triggerSkill.TargetTypes.Contains(SkillTargetType.AllWeapon) || triggerSkill.TargetTypes.Contains((SkillTargetType)wType) || triggerSkill.TargetTypes[0] == SkillTargetType.None)
				{
					triggerSkill.ApplySkill(unit, applyPosition);
				}
			}
		}
	}

	public void OnHitEnemyTrigger(GameUnit unit, WeaponType wType, GameUnit target)
	{
		foreach (CharacterInstantSkill triggerSkill in triggerSkills)
		{
			if (triggerSkill != null && triggerSkill.STriggerType == SkillTriggerType.OnHitEnemy && (triggerSkill.TargetTypes.Contains(SkillTargetType.AllWeapon) || triggerSkill.TargetTypes.Contains((SkillTargetType)wType) || triggerSkill.TargetTypes[0] == SkillTargetType.None))
			{
				if (triggerSkill is CharacterCreateBuffSkill)
				{
					Debug.Log("APPLY!!!!!!");
					(triggerSkill as CharacterCreateBuffSkill).ApplySkill(unit, target);
				}
				else
				{
					triggerSkill.ApplySkill(unit);
				}
			}
		}
	}

	public void OnShieldValueTrigger(GameUnit unit, int currentShieldValue, float currentShieldPercentage)
	{
		foreach (CharacterInstantSkill triggerSkill in triggerSkills)
		{
			if (triggerSkill != null)
			{
				if (triggerSkill.STriggerType == SkillTriggerType.OnShieldLessThanValue && LastShieldValue > triggerSkill.STriggerTypeSubValue && currentShieldValue <= triggerSkill.STriggerTypeSubValue)
				{
					triggerSkill.ApplySkill(unit);
				}
				if (triggerSkill.STriggerType == SkillTriggerType.OnShieldLessThanPercentage && LastShieldPercentage > (float)triggerSkill.STriggerTypeSubValue * 0.01f && currentShieldPercentage <= (float)triggerSkill.STriggerTypeSubValue * 0.01f)
				{
					triggerSkill.ApplySkill(unit);
				}
			}
		}
		LastShieldValue = currentShieldValue;
		LastShieldPercentage = currentShieldPercentage;
	}

	public void OnHPValueTrigger(GameUnit unit, int currentHPValue, float currentHPPercentage)
	{
		foreach (CharacterInstantSkill triggerSkill in triggerSkills)
		{
			if (triggerSkill != null)
			{
				if (triggerSkill.STriggerType == SkillTriggerType.OnHPLessThanValue && LastHPValue > triggerSkill.STriggerTypeSubValue && currentHPValue <= triggerSkill.STriggerTypeSubValue)
				{
					triggerSkill.ApplySkill(unit);
				}
				if (triggerSkill.STriggerType == SkillTriggerType.OnHPLessThanPercentage && LastHPPercentage > (float)triggerSkill.STriggerTypeSubValue * 0.01f && currentHPPercentage <= (float)triggerSkill.STriggerTypeSubValue * 0.01f)
				{
					triggerSkill.ApplySkill(unit);
				}
			}
		}
		LastHPValue = currentHPValue;
		LastHPPercentage = currentHPPercentage;
	}

	public void AddSkill(CharacterSkill skill)
	{
		bool flag = true;
		if (skill.IsInstantSkill())
		{
			CharacterInstantSkill characterInstantSkill = skill as CharacterInstantSkill;
			if (characterInstantSkill.STriggerType == SkillTriggerType.Initiative)
			{
				for (int i = 0; i < initiativeSkills.Count; i++)
				{
					if (initiativeSkills[i].skillID == characterInstantSkill.skillID)
					{
						if (initiativeSkills[i].SkillLevel > characterInstantSkill.SkillLevel)
						{
							flag = false;
							continue;
						}
						initiativeSkills.Remove(initiativeSkills[i]);
						i--;
					}
				}
				if (flag)
				{
					initiativeSkills.Add(characterInstantSkill);
				}
				return;
			}
			for (int j = 0; j < triggerSkills.Count; j++)
			{
				if (triggerSkills[j].skillID == characterInstantSkill.skillID)
				{
					if (triggerSkills[j].SkillLevel > characterInstantSkill.SkillLevel)
					{
						flag = false;
						continue;
					}
					triggerSkills.Remove(triggerSkills[j]);
					j--;
				}
			}
			if (flag)
			{
				triggerSkills.Add(characterInstantSkill);
			}
			return;
		}
		CharacterStateSkill characterStateSkill = skill as CharacterStateSkill;
		for (int k = 0; k < stateSkills.Count; k++)
		{
			if (stateSkills[k].skillID == characterStateSkill.skillID)
			{
				if (stateSkills[k].SkillLevel > characterStateSkill.SkillLevel)
				{
					flag = false;
					continue;
				}
				stateSkills.Remove(stateSkills[k]);
				k--;
			}
		}
		if (flag)
		{
			stateSkills.Add(characterStateSkill);
		}
	}

	public void AddSkillByID(short skillID, int skillLevel)
	{
		Debug.Log(skillID + "--" + skillLevel);
		SkillConfig skillConfig = GameConfig.GetInstance().skillConfig[skillID * 10 + skillLevel];
		switch (skillConfig.FunctionType1)
		{
		case SkillFunctionType.PropertyChange:
		{
			CharacterStateSkill characterStateSkill4 = new CharacterStateSkill();
			int x = skillConfig.X1;
			characterStateSkill4.skillID = skillID;
			characterStateSkill4.IsPermanent = true;
			characterStateSkill4.ModifierOfBuff1 = (BuffModifier)(x / 100);
			characterStateSkill4.PropertyChangeTypeOfBuff1 = (PropertyChangeType)(x % 100);
			characterStateSkill4.BuffValueY1 = skillConfig.Y1;
			characterStateSkill4.FunctionType1 = BuffFunctionType.PropertyChange;
			characterStateSkill4.STriggerType = skillConfig.STriggerType;
			characterStateSkill4.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterStateSkill4.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterStateSkill4);
			break;
		}
		case SkillFunctionType.Summon:
		{
			CharacterSummonSkill characterSummonSkill = new CharacterSummonSkill();
			characterSummonSkill.skillID = skillID;
			characterSummonSkill.STriggerType = SkillTriggerType.Initiative;
			characterSummonSkill.SEffectType = SkillEffectType.Summon;
			characterSummonSkill.CoolDownTime = skillConfig.CoolDownTime;
			characterSummonSkill.CoolDownTimeInit = characterSummonSkill.CoolDownTime;
			characterSummonSkill.SummonedType = (byte)skillConfig.X1;
			characterSummonSkill.Duration = (short)skillConfig.Y1;
			characterSummonSkill.DurationInit = characterSummonSkill.Duration;
			characterSummonSkill.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterSummonSkill);
			break;
		}
		case SkillFunctionType.CreateBuff:
		{
			CharacterCreateBuffSkill characterCreateBuffSkill = new CharacterCreateBuffSkill();
			characterCreateBuffSkill.skillID = skillID;
			characterCreateBuffSkill.STriggerType = skillConfig.STriggerType;
			characterCreateBuffSkill.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterCreateBuffSkill.SEffectType = SkillEffectType.CreateBuff;
			characterCreateBuffSkill.CoolDownTime = skillConfig.CoolDownTime;
			characterCreateBuffSkill.CoolDownTimeInit = characterCreateBuffSkill.CoolDownTime;
			characterCreateBuffSkill.Duration = (short)skillConfig.Y1;
			characterCreateBuffSkill.DurationInit = characterCreateBuffSkill.Duration;
			short buffID3 = (short)skillConfig.X1;
			CharacterStateSkill characterStateSkill3 = new CharacterStateSkill(buffID3);
			characterCreateBuffSkill.ReadGeneralSkillProperty(skillConfig);
			characterStateSkill3.Target = characterCreateBuffSkill.Target;
			characterStateSkill3.TargetTypes = characterCreateBuffSkill.TargetTypes;
			characterCreateBuffSkill.AddStateBuff(characterStateSkill3);
			AddSkill(characterCreateBuffSkill);
			break;
		}
		case SkillFunctionType.MakeDamage:
		{
			CharacterMakeDamageSkill characterMakeDamageSkill = new CharacterMakeDamageSkill();
			characterMakeDamageSkill.skillID = skillID;
			characterMakeDamageSkill.STriggerType = skillConfig.STriggerType;
			characterMakeDamageSkill.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterMakeDamageSkill.SEffectType = SkillEffectType.MakeDamage;
			characterMakeDamageSkill.CoolDownTime = skillConfig.CoolDownTime;
			characterMakeDamageSkill.CoolDownTimeInit = characterMakeDamageSkill.CoolDownTime;
			characterMakeDamageSkill.ElementType = (ElementType)(skillConfig.X1 / 100000);
			characterMakeDamageSkill.EffectValueX = (float)(skillConfig.X1 % 100000) * 0.01f;
			characterMakeDamageSkill.EffectValueY = skillConfig.Y1 % 100000;
			characterMakeDamageSkill.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterMakeDamageSkill);
			break;
		}
		case SkillFunctionType.MakeHeal:
		{
			CharacterInstantSkill characterInstantSkill8 = new CharacterInstantSkill();
			characterInstantSkill8.skillID = skillID;
			characterInstantSkill8.STriggerType = skillConfig.STriggerType;
			characterInstantSkill8.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill8.SEffectType = SkillEffectType.Heal;
			characterInstantSkill8.CoolDownTime = skillConfig.CoolDownTime;
			characterInstantSkill8.CoolDownTimeInit = characterInstantSkill8.CoolDownTime;
			characterInstantSkill8.EffectValueX = (float)skillConfig.X1 * 0.01f;
			characterInstantSkill8.EffectValueY = skillConfig.Y1;
			characterInstantSkill8.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterInstantSkill8);
			break;
		}
		case SkillFunctionType.Rush:
		{
			CharacterInstantSkill characterInstantSkill7 = new CharacterInstantSkill();
			characterInstantSkill7.skillID = skillID;
			characterInstantSkill7.STriggerType = skillConfig.STriggerType;
			characterInstantSkill7.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill7.SEffectType = SkillEffectType.Rush;
			characterInstantSkill7.CoolDownTime = skillConfig.CoolDownTime;
			characterInstantSkill7.CoolDownTimeInit = characterInstantSkill7.CoolDownTime;
			characterInstantSkill7.EffectValueX = skillConfig.X1;
			characterInstantSkill7.EffectValueY = skillConfig.Y1;
			characterInstantSkill7.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterInstantSkill7);
			break;
		}
		case SkillFunctionType.ClearBuff:
			break;
		case SkillFunctionType.ShieldRecover:
		{
			CharacterInstantSkill characterInstantSkill6 = new CharacterInstantSkill();
			characterInstantSkill6.skillID = skillID;
			characterInstantSkill6.STriggerType = skillConfig.STriggerType;
			characterInstantSkill6.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill6.SEffectType = SkillEffectType.RecoverShield;
			characterInstantSkill6.CoolDownTime = skillConfig.CoolDownTime;
			characterInstantSkill6.CoolDownTimeInit = characterInstantSkill6.CoolDownTime;
			characterInstantSkill6.EffectValueX = (float)skillConfig.X1 * 0.01f;
			characterInstantSkill6.EffectValueY = skillConfig.Y1;
			characterInstantSkill6.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterInstantSkill6);
			break;
		}
		case SkillFunctionType.BulletRecover:
		{
			CharacterBulletRecoverByTimeSkill characterBulletRecoverByTimeSkill = new CharacterBulletRecoverByTimeSkill();
			characterBulletRecoverByTimeSkill.skillID = skillID;
			characterBulletRecoverByTimeSkill.STriggerType = skillConfig.STriggerType;
			characterBulletRecoverByTimeSkill.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterBulletRecoverByTimeSkill.SEffectType = SkillEffectType.RecoverBullet;
			characterBulletRecoverByTimeSkill.CoolDownTime = skillConfig.CoolDownTime;
			characterBulletRecoverByTimeSkill.CoolDownTimeInit = characterBulletRecoverByTimeSkill.CoolDownTime;
			characterBulletRecoverByTimeSkill.EffectValueX = skillConfig.X1;
			characterBulletRecoverByTimeSkill.EffectValueY = skillConfig.Y1;
			characterBulletRecoverByTimeSkill.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterBulletRecoverByTimeSkill);
			break;
		}
		case SkillFunctionType.ExtraBuff:
		{
			CharacterInstantSkill characterInstantSkill5 = new CharacterInstantSkill();
			characterInstantSkill5.skillID = skillID;
			characterInstantSkill5.STriggerType = skillConfig.STriggerType;
			characterInstantSkill5.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill5.SEffectType = SkillEffectType.ExtraBuff;
			characterInstantSkill5.EffectValueX = skillConfig.X1;
			characterInstantSkill5.EffectValueY = skillConfig.Y1;
			characterInstantSkill5.ReadGeneralSkillProperty(skillConfig);
			if (skillConfig.STriggerType != SkillTriggerType.OnNearSummonedItem)
			{
				characterInstantSkill5.ApplySkill(player);
			}
			else
			{
				AddSkill(characterInstantSkill5);
			}
			break;
		}
		case SkillFunctionType.ShortCD:
		{
			CharacterInstantSkill characterInstantSkill4 = new CharacterInstantSkill();
			characterInstantSkill4.skillID = skillID;
			characterInstantSkill4.STriggerType = skillConfig.STriggerType;
			characterInstantSkill4.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill4.SEffectType = SkillEffectType.ChangeCD;
			characterInstantSkill4.EffectValueX = skillConfig.X1;
			characterInstantSkill4.EffectValueY = skillConfig.Y1;
			characterInstantSkill4.ReadGeneralSkillProperty(skillConfig);
			if (skillConfig.STriggerType != SkillTriggerType.OnNearSummonedItem)
			{
				characterInstantSkill4.ApplySkill(player);
			}
			else
			{
				AddSkill(characterInstantSkill4);
			}
			break;
		}
		case SkillFunctionType.SummonDurationChange:
		{
			CharacterInstantSkill characterInstantSkill3 = new CharacterInstantSkill();
			characterInstantSkill3.skillID = skillID;
			characterInstantSkill3.STriggerType = skillConfig.STriggerType;
			characterInstantSkill3.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill3.SEffectType = SkillEffectType.ChangeSummonDuration;
			characterInstantSkill3.EffectValueX = skillConfig.X1;
			characterInstantSkill3.EffectValueY = skillConfig.Y1;
			characterInstantSkill3.ReadGeneralSkillProperty(skillConfig);
			characterInstantSkill3.ApplySkill(player);
			break;
		}
		case SkillFunctionType.BuffDurationChange:
		{
			CharacterInstantSkill characterInstantSkill2 = new CharacterInstantSkill();
			characterInstantSkill2.skillID = skillID;
			characterInstantSkill2.STriggerType = skillConfig.STriggerType;
			characterInstantSkill2.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill2.SEffectType = SkillEffectType.ChangeBuffDuration;
			characterInstantSkill2.EffectValueX = skillConfig.X1;
			characterInstantSkill2.EffectValueY = skillConfig.Y1;
			characterInstantSkill2.ReadGeneralSkillProperty(skillConfig);
			Debug.Log(characterInstantSkill2.Name);
			characterInstantSkill2.ApplySkill(player);
			break;
		}
		case SkillFunctionType.CreateShield:
		{
			CharacterExtraShieldSkill characterExtraShieldSkill = new CharacterExtraShieldSkill();
			characterExtraShieldSkill.skillID = skillID;
			characterExtraShieldSkill.STriggerType = skillConfig.STriggerType;
			characterExtraShieldSkill.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterExtraShieldSkill.SEffectType = SkillEffectType.CreateShield;
			characterExtraShieldSkill.CoolDownTime = skillConfig.CoolDownTime;
			characterExtraShieldSkill.CoolDownTimeInit = characterExtraShieldSkill.CoolDownTime;
			characterExtraShieldSkill.EffectValueX = skillConfig.X1;
			characterExtraShieldSkill.EffectValueY = skillConfig.Y1;
			characterExtraShieldSkill.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterExtraShieldSkill);
			break;
		}
		case SkillFunctionType.CannonMove_Explosion:
		{
			CharacterInstantSkill characterInstantSkill = new CharacterInstantSkill();
			characterInstantSkill.skillID = skillID;
			characterInstantSkill.STriggerType = skillConfig.STriggerType;
			characterInstantSkill.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterInstantSkill.SEffectType = SkillEffectType.CannonMove_Explosion;
			characterInstantSkill.EffectValueX = skillConfig.X1;
			characterInstantSkill.ReadGeneralSkillProperty(skillConfig);
			characterInstantSkill.ApplySkill(player);
			break;
		}
		case SkillFunctionType.Morphine:
		{
			CharacterMorphineSkill characterMorphineSkill = new CharacterMorphineSkill();
			characterMorphineSkill.skillID = skillID;
			characterMorphineSkill.skillID = skillID;
			characterMorphineSkill.STriggerType = skillConfig.STriggerType;
			characterMorphineSkill.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterMorphineSkill.SEffectType = SkillEffectType.Morphine;
			characterMorphineSkill.CoolDownTime = skillConfig.CoolDownTime;
			characterMorphineSkill.CoolDownTimeInit = characterMorphineSkill.CoolDownTime;
			characterMorphineSkill.EffectValueX = (float)skillConfig.X1 * 0.01f;
			characterMorphineSkill.EffectValueY = skillConfig.Y1;
			characterMorphineSkill.ReadGeneralSkillProperty(skillConfig);
			characterMorphineSkill.Duration = (short)skillConfig.Y2;
			characterMorphineSkill.DurationInit = characterMorphineSkill.Duration;
			short buffID2 = (short)skillConfig.X2;
			CharacterStateSkill characterStateSkill2 = new CharacterStateSkill(buffID2);
			characterStateSkill2.Target = characterMorphineSkill.Target;
			characterStateSkill2.TargetTypes = characterMorphineSkill.TargetTypes;
			characterMorphineSkill.AddStateBuff(characterStateSkill2);
			AddSkill(characterMorphineSkill);
			break;
		}
		case SkillFunctionType.KeepHealing:
		{
			CharacterKeepHealingSkill characterKeepHealingSkill = new CharacterKeepHealingSkill();
			characterKeepHealingSkill.skillID = skillID;
			characterKeepHealingSkill.STriggerType = skillConfig.STriggerType;
			characterKeepHealingSkill.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterKeepHealingSkill.SEffectType = SkillEffectType.KeepHealing;
			characterKeepHealingSkill.CoolDownTime = skillConfig.CoolDownTime;
			characterKeepHealingSkill.CoolDownTimeInit = characterKeepHealingSkill.CoolDownTime;
			short buffID = (short)skillConfig.X1;
			CharacterStateSkill characterStateSkill = new CharacterStateSkill(buffID);
			characterKeepHealingSkill.EffectValueX = characterStateSkill.BuffValueX1;
			characterKeepHealingSkill.EffectValueY = (short)skillConfig.Y1;
			characterKeepHealingSkill.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterKeepHealingSkill);
			break;
		}
		}
	}

	public void RemoveSkillByID(short skillID)
	{
		foreach (CharacterInstantSkill initiativeSkill in initiativeSkills)
		{
			if (initiativeSkill.skillID == skillID)
			{
				initiativeSkills.Remove(initiativeSkill);
				break;
			}
		}
		foreach (CharacterInstantSkill triggerSkill in triggerSkills)
		{
			if (triggerSkill.skillID == skillID)
			{
				triggerSkills.Remove(triggerSkill);
				break;
			}
		}
		foreach (CharacterStateSkill stateSkill in stateSkills)
		{
			if (stateSkill.skillID == skillID)
			{
				stateSkills.Remove(stateSkill);
				break;
			}
		}
	}

	public void ClearAllSkills()
	{
		while (initiativeSkills.Count > 1)
		{
			initiativeSkills.RemoveAt(1);
		}
		triggerSkills.Clear();
		stateSkills.Clear();
	}

	public static int CalculateExplosionSkillDamage()
	{
		int num = 0;
		Weapon weapon = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetWeapon();
		num = Mathf.CeilToInt(weapon.Damage * (float)weapon.GunCapacity / ((float)weapon.GunCapacity * weapon.AttackFrequency + weapon.ReloadTime / weapon.ReloadTimeScale));
		if (weapon.GetWeaponType() == WeaponType.ShotGun)
		{
			num *= 4;
		}
		return num;
	}

	public void ResetAllInstantSkills()
	{
		foreach (CharacterInstantSkill initiativeSkill in initiativeSkills)
		{
			initiativeSkill.ResetCD();
		}
	}
}

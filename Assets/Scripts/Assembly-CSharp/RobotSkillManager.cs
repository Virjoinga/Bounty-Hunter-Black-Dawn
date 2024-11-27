using System.Collections.Generic;
using UnityEngine;

public class RobotSkillManager
{
	private RobotUser mRobotUser;

	private List<CharacterInstantSkill> initiativeSkills = new List<CharacterInstantSkill>();

	private List<CharacterInstantSkill> triggerSkills = new List<CharacterInstantSkill>();

	private List<CharacterStateSkill> stateSkills = new List<CharacterStateSkill>();

	private Timer mRandomSkillTimer = new Timer();

	private Timer mClearSkillTimer = new Timer();

	private Timer mRecoverTimer = new Timer();

	private Timer mDamageTimer = new Timer();

	public void SetRobotUser(RobotUser robotUser)
	{
		mRobotUser = robotUser;
	}

	protected RobotUser GetRobotUser()
	{
		return mRobotUser;
	}

	public void LoadRobotSkills(RobotUser robotUser)
	{
		int num = 3;
		int num2 = 4;
		int num3 = 2;
		SkillTreeMgr skillTreeManager = robotUser.GetUserState().SkillTreeManager;
		if (skillTreeManager == null)
		{
			return;
		}
		skillTreeManager.InitSkillIDs(robotUser);
		AddSkillByID(skillTreeManager.GetSkillId(-1, -1, -1), 1);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < num3; k++)
				{
					if (j != 3 || k != 1)
					{
						int num4 = 1;
						if (num4 > 0)
						{
							short skillId = skillTreeManager.GetSkillId(i, j, k);
							AddSkillByID(skillId, num4);
						}
					}
				}
			}
		}
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
		SkillConfig skillConfig = GameConfig.GetInstance().skillConfig[skillID * 10 + skillLevel];
		switch (skillConfig.FunctionType1)
		{
		case SkillFunctionType.PropertyChange:
		{
			CharacterStateSkill characterStateSkill3 = new CharacterStateSkill();
			int x = skillConfig.X1;
			characterStateSkill3.skillID = skillID;
			characterStateSkill3.IsPermanent = true;
			characterStateSkill3.ModifierOfBuff1 = (BuffModifier)(x / 100);
			characterStateSkill3.PropertyChangeTypeOfBuff1 = (PropertyChangeType)(x % 100);
			characterStateSkill3.BuffValueY1 = skillConfig.Y1;
			characterStateSkill3.FunctionType1 = BuffFunctionType.PropertyChange;
			characterStateSkill3.STriggerType = skillConfig.STriggerType;
			characterStateSkill3.STriggerTypeSubValue = skillConfig.STriggerTypeSubValue;
			characterStateSkill3.ReadGeneralSkillProperty(skillConfig);
			AddSkill(characterStateSkill3);
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
			CharacterStateSkill characterStateSkill4 = new CharacterStateSkill(buffID3);
			characterCreateBuffSkill.ReadGeneralSkillProperty(skillConfig);
			characterStateSkill4.Target = characterCreateBuffSkill.Target;
			characterStateSkill4.TargetTypes = characterCreateBuffSkill.TargetTypes;
			characterCreateBuffSkill.AddStateBuff(characterStateSkill4);
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
			if (skillConfig.STriggerType == SkillTriggerType.OnNearSummonedItem)
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
			if (skillConfig.STriggerType == SkillTriggerType.OnNearSummonedItem)
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

	public void Init(RobotUser robotUser)
	{
		SetRobotUser(robotUser);
		mRandomSkillTimer.SetTimer(60f, false);
		mClearSkillTimer.SetTimer(25f, false);
		mRecoverTimer.SetTimer(1f, false);
		mDamageTimer.SetTimer(30f, false);
	}

	public void Update()
	{
		if (mRandomSkillTimer.Ready())
		{
			mRandomSkillTimer.Do();
			ApplyRandomSkill();
		}
		if (mClearSkillTimer.Ready())
		{
			mClearSkillTimer.Do();
			ControllableItemDisappearRequest request = new ControllableItemDisappearRequest(EControllableType.SUMMONED, GetNextSummonedID(mRobotUser));
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request, mRobotUser);
			ClearExtraShieldRequest request2 = new ClearExtraShieldRequest();
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request2, mRobotUser);
		}
		if (mRecoverTimer.Ready())
		{
			mRecoverTimer.Do();
			PlayerHpRecoveryRequest request3 = new PlayerHpRecoveryRequest(0, 30, (short)mRobotUser.MaxHp);
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request3, mRobotUser);
			PlayerShieldRecoveryRequest request4 = new PlayerShieldRecoveryRequest(0, 30, (short)mRobotUser.MaxShield);
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request4, mRobotUser);
		}
		if (mDamageTimer.Ready())
		{
			mDamageTimer.Do();
			PlayerHpRecoveryRequest request5 = new PlayerHpRecoveryRequest(0, -150, (short)mRobotUser.MaxHp);
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request5, mRobotUser);
			PlayerShieldRecoveryRequest request6 = new PlayerShieldRecoveryRequest(0, -100, (short)mRobotUser.MaxShield);
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request6, mRobotUser);
		}
	}

	public void ApplyRandomSkill()
	{
		CharacterInstantSkill characterInstantSkill = initiativeSkills[Random.Range(0, initiativeSkills.Count)];
		if (mRobotUser.GetGameScene().mMapType == MapType.City)
		{
			return;
		}
		switch (characterInstantSkill.SEffectType)
		{
		case SkillEffectType.Summon:
		{
			Vector3 position = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			switch (mRobotUser.GetGameScene().mSceneID)
			{
			case 33:
				position = new Vector3(10.19911f, 0.8746616f, 2.963272f);
				break;
			case 39:
				position = new Vector3(-0.5275726f, 0f, -1.316895f);
				break;
			case 34:
				position = new Vector3(171.4607f, 0.04998789f, 129.5167f);
				break;
			case 38:
				position = new Vector3(-0.5275726f, 0f, -1.316895f);
				break;
			}
			ControllableItemCreateRequest request2 = new ControllableItemCreateRequest(EControllableType.SUMMONED, GetNextSummonedID(mRobotUser), (byte)mRobotUser.GetUserState().GetCharLevel(), (characterInstantSkill as CharacterSummonSkill).SummonedType, position, identity, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request2, mRobotUser);
			break;
		}
		case SkillEffectType.CreateShield:
		{
			CreateExtraShieldRequest request = new CreateExtraShieldRequest(1000);
			mRobotUser.GetNetworkManager().SendRequestAsRobot(request, mRobotUser);
			break;
		}
		}
	}

	public byte GetNextSummonedID(RobotUser robotUser)
	{
		byte currentSeatID = robotUser.GetLobby().CurrentSeatID;
		return (byte)((uint)(currentSeatID << 4) | 0u);
	}
}

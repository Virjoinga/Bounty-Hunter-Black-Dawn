using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : GameEntity
{
	public struct ElementDotData
	{
		public ElementType type;

		public int damage;

		public int time;

		public bool isPenetration;

		public int unitLevel;

		public int weaponLevel;

		public WeaponType weaponType;

		public int attackerID;
	}

	protected UnitDetailedProperties detailedProperties = new UnitDetailedProperties();

	protected Rigidbody rigidbody;

	protected Collider collider;

	protected CharacterController cc;

	protected Vector3 characterControllerCenter;

	protected string mDisplayName;

	protected float BasicSpeed;

	protected int BasicMaxHp;

	protected int BasicMeleeATK;

	protected int BasicMaxShield;

	protected int BasicShieldRecovery;

	protected float BasicDyingTimeLength;

	protected int BasicDamageReduction;

	protected int BasicExplosionRduction;

	protected int BasicCriticalRate;

	protected int BasicCriticalDamage;

	protected int BasicDamageToHealthPercentage;

	protected float BasicDropRate;

	protected int[] ElementResistance = new int[4];

	protected int[] ElementResistanceInit = new int[4];

	protected ShieldType mShieldType;

	protected ArrayList mElementDotList = new ArrayList();

	protected Timer mDotTimer = new Timer();

	protected Timer mShieldRecoverySecondTimer = new Timer();

	protected Timer mShieldRecoveryStartTimer = new Timer();

	protected int mPrevShield;

	protected float mDeltaShield;

	protected bool isSpeedDownEffectAdded;

	public EGameUnitType GameUnitType { get; set; }

	public float Speed { get; set; }

	public virtual int Hp { get; set; }

	public int MaxHp { get; set; }

	public int MeleeATK { get; set; }

	public virtual int Shield { get; set; }

	public int MaxShield { get; set; }

	public int ShieldRecovery { get; set; }

	public float DyingTimeLength { get; set; }

	public int DamageReduction { get; set; }

	public int ExplosionDamageReduction { get; set; }

	public int CriticalRate { get; set; }

	public int CriticalDamage { get; set; }

	public int DamageToHealthPercentage { get; set; }

	public float DropRate { get; set; }

	public ShieldType ShieldType
	{
		get
		{
			return mShieldType;
		}
	}

	public virtual bool InPlayingState()
	{
		return false;
	}

	public void SetDisplayName(string name)
	{
		mDisplayName = name;
	}

	public string GetDisplayName()
	{
		return mDisplayName;
	}

	public int GetElementResistance(ElementType element)
	{
		return ElementResistance[(int)(element - 1)];
	}

	public void IncreaseElementResistance(ElementType element, int value)
	{
		ElementResistance[(int)(element - 1)] = ElementResistanceInit[(int)(element - 1)] + value;
		ElementResistance[(int)(element - 1)] = Mathf.Clamp(ElementResistance[(int)(element - 1)], 0, 70);
	}

	public Collider GetCollider()
	{
		return collider;
	}

	public CharacterController GetCharacterController()
	{
		return cc;
	}

	public Vector3 GetCharacterControllerCenter()
	{
		return characterControllerCenter;
	}

	public override void SetObject(GameObject obj)
	{
		base.SetObject(obj);
		collider = entityObject.GetComponent<Collider>();
		if (entityObject.GetComponent<CharacterController>() != null)
		{
			cc = entityObject.GetComponent<CharacterController>();
			characterControllerCenter = cc.center;
		}
	}

	public virtual void PlaySound(string name)
	{
		AudioManager.GetInstance().PlaySoundAt(name, entityObject, entityTransform.position, AudioRolloffMode.Linear, 50f);
	}

	public virtual void PlaySoundSingle(string name)
	{
		AudioManager.GetInstance().PlaySoundSingleAt(name, entityObject, entityTransform.position, AudioRolloffMode.Linear, 50f);
	}

	public virtual void PlayLogarithmicSound(string name)
	{
		AudioManager.GetInstance().PlaySoundAt(name, entityObject, entityTransform.position, AudioRolloffMode.Logarithmic, 1000f);
	}

	public virtual void PlayLogarithmicSingleSound(string name)
	{
		AudioManager.GetInstance().PlaySoundSingleAt(name, entityObject, entityTransform.position, AudioRolloffMode.Logarithmic, 1000f);
	}

	public virtual void StopSound(string name)
	{
		AudioManager.GetInstance().StopSound(name, entityObject);
	}

	protected virtual void DoShieldRecovery(float _deltaTime)
	{
		if (MaxShield <= 0)
		{
			return;
		}
		if (mShieldRecoveryStartTimer.Ready())
		{
			if (Shield < MaxShield)
			{
				mDeltaShield += (float)ShieldRecovery * _deltaTime;
				if (mDeltaShield > 1f)
				{
					Shield += (int)mDeltaShield;
					mDeltaShield = 0f;
				}
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && mShieldRecoverySecondTimer.Ready())
			{
				mShieldRecoverySecondTimer.Do();
				SendShieldRecoveryRequest();
			}
		}
		else
		{
			mPrevShield = Shield;
		}
	}

	protected virtual void SendShieldRecoveryRequest()
	{
	}

	protected virtual void ResetShieldRecoveryStartTimer()
	{
		mShieldRecoveryStartTimer.Do();
	}

	public void AddBuff(PropertyChangeType buffType, BuffModifier buffModifier, float buffValue, SkillTarget skillTarget, List<SkillTargetType> skillTargetTypes)
	{
		switch (buffType)
		{
		case PropertyChangeType.ChangeDamage:
		{
			foreach (SkillTargetType skillTargetType in skillTargetTypes)
			{
				if (skillTargetType == SkillTargetType.AllWeapon)
				{
					detailedProperties.DamageBonusAll += buffValue * 0.01f;
				}
				else if (SkillTargetType.None < skillTargetType && skillTargetType <= SkillTargetType.Grenade)
				{
					detailedProperties.DamageBonus[(int)(skillTargetType - 1)] += buffValue * 0.01f;
				}
				else if (skillTargetType == SkillTargetType.Melee)
				{
					detailedProperties.MeleeDamageBonus += buffValue * 0.01f;
				}
			}
			break;
		}
		case PropertyChangeType.ChangeAccuracy:
		{
			foreach (SkillTargetType skillTargetType2 in skillTargetTypes)
			{
				if (skillTargetType2 == SkillTargetType.AllWeapon)
				{
					detailedProperties.AccuracyBonusAll += buffValue * 0.01f;
				}
				else if (SkillTargetType.None < skillTargetType2 && skillTargetType2 <= SkillTargetType.Grenade)
				{
					detailedProperties.AccuracyBonus[(int)(skillTargetType2 - 1)] += buffValue * 0.01f;
				}
			}
			break;
		}
		case PropertyChangeType.ChangeFireRate:
		{
			foreach (SkillTargetType skillTargetType3 in skillTargetTypes)
			{
				if (skillTargetType3 == SkillTargetType.AllWeapon)
				{
					detailedProperties.AttackIntervalPercentageBonusAll += buffValue * 0.01f;
				}
				else if (SkillTargetType.None < skillTargetType3 && skillTargetType3 <= SkillTargetType.Grenade)
				{
					detailedProperties.AttackIntervalPercentageBonus[(int)(skillTargetType3 - 1)] += buffValue * 0.01f;
				}
			}
			break;
		}
		case PropertyChangeType.ChangeReloadSpeed:
		{
			foreach (SkillTargetType skillTargetType4 in skillTargetTypes)
			{
				if (skillTargetType4 == SkillTargetType.AllWeapon)
				{
					detailedProperties.ReloadTimeBonusAll -= buffValue * 0.01f;
				}
				else if (SkillTargetType.None < skillTargetType4 && skillTargetType4 <= SkillTargetType.Grenade)
				{
					detailedProperties.ReloadTimeBonus[(int)(skillTargetType4 - 1)] -= buffValue * 0.01f;
				}
			}
			break;
		}
		case PropertyChangeType.ChangeMags:
			if (buffModifier == BuffModifier.PERCENTAGE)
			{
				foreach (SkillTargetType skillTargetType5 in skillTargetTypes)
				{
					if (skillTargetType5 == SkillTargetType.AllWeapon)
					{
						detailedProperties.MagsBonusAll += buffValue * 0.01f;
					}
					else if (SkillTargetType.None < skillTargetType5 && skillTargetType5 <= SkillTargetType.Grenade)
					{
						detailedProperties.MagsBonus[(int)(skillTargetType5 - 1)] += buffValue * 0.01f;
					}
				}
				break;
			}
			{
				foreach (SkillTargetType skillTargetType6 in skillTargetTypes)
				{
					if (skillTargetType6 == SkillTargetType.AllWeapon)
					{
						detailedProperties.MagsBonusValueAll += (int)buffValue;
					}
					else if (SkillTargetType.None < skillTargetType6 && skillTargetType6 <= SkillTargetType.Grenade)
					{
						detailedProperties.MagsBonusValue[(int)(skillTargetType6 - 1)] += (int)buffValue;
					}
				}
				break;
			}
		case PropertyChangeType.ChangeRecoil:
		{
			foreach (SkillTargetType skillTargetType7 in skillTargetTypes)
			{
				if (skillTargetType7 == SkillTargetType.AllWeapon)
				{
					detailedProperties.RecoilBonusAll -= buffValue * 0.01f;
				}
				else if (SkillTargetType.None < skillTargetType7 && skillTargetType7 <= SkillTargetType.Grenade)
				{
					detailedProperties.RecoilBonus[(int)(skillTargetType7 - 1)] -= buffValue * 0.01f;
				}
			}
			break;
		}
		case PropertyChangeType.ChangeCriticalRate:
		{
			foreach (SkillTargetType skillTargetType8 in skillTargetTypes)
			{
				if (skillTargetType8 == SkillTargetType.AllWeapon)
				{
					detailedProperties.CriticalRateBonusAll += buffValue * 0.01f;
				}
				else if (SkillTargetType.None < skillTargetType8 && skillTargetType8 <= SkillTargetType.Grenade)
				{
					detailedProperties.CriticalRateBonus[(int)(skillTargetType8 - 1)] += buffValue * 0.01f;
				}
			}
			break;
		}
		case PropertyChangeType.ChangeCriticalDamage:
		{
			foreach (SkillTargetType skillTargetType9 in skillTargetTypes)
			{
				if (skillTargetType9 == SkillTargetType.AllWeapon)
				{
					detailedProperties.CriticalDamageBonusAll += buffValue * 0.01f;
				}
				else if (SkillTargetType.None < skillTargetType9 && skillTargetType9 <= SkillTargetType.Grenade)
				{
					detailedProperties.CriticalDamageBonus[(int)(skillTargetType9 - 1)] += buffValue * 0.01f;
				}
			}
			break;
		}
		case PropertyChangeType.ChangeMeleeDamage:
			detailedProperties.MeleeDamageBonus += buffValue * 0.01f;
			break;
		case PropertyChangeType.ChangeShieldCapacity:
			if (buffModifier == BuffModifier.VALUE)
			{
				detailedProperties.MaxShieldBonus += (int)buffValue;
			}
			else
			{
				detailedProperties.MaxShieldPercentageBonus += buffValue * 0.01f;
			}
			break;
		case PropertyChangeType.ChangeShieldRecoverySpeed:
			if (buffModifier == BuffModifier.VALUE)
			{
				detailedProperties.ShieldRecoveryBonus += (int)buffValue;
			}
			else
			{
				detailedProperties.ShieldRecoveryPercentageBonus += buffValue * 0.01f;
			}
			break;
		case PropertyChangeType.ChangeFireResistance:
			detailedProperties.ElementsResistanceBonus[0] += (int)buffValue;
			break;
		case PropertyChangeType.ChangeShockResistance:
			detailedProperties.ElementsResistanceBonus[1] += (int)buffValue;
			break;
		case PropertyChangeType.ChangeCorrosiveResistance:
			detailedProperties.ElementsResistanceBonus[2] += (int)buffValue;
			break;
		case PropertyChangeType.ChangeMaxHP:
			if (buffModifier == BuffModifier.VALUE)
			{
				detailedProperties.HpBonus += (int)buffValue;
			}
			else
			{
				detailedProperties.HpPercentageBonus += buffValue * 0.01f;
			}
			break;
		case PropertyChangeType.ChangeHPRecoverSpeed:
			if (buffModifier == BuffModifier.VALUE)
			{
				detailedProperties.HpPercentageBonus += buffValue;
			}
			else
			{
				detailedProperties.HpRecoveryPercentageBonus += buffValue * 0.01f;
			}
			break;
		case PropertyChangeType.ChangeExplosionRange:
			detailedProperties.ExplosionRangeBonus += buffValue * 0.01f;
			break;
		case PropertyChangeType.ChangeMoveSpeed:
			detailedProperties.SpeedBonus += buffValue * 0.01f;
			break;
		case PropertyChangeType.ChangeDamageReduction:
			detailedProperties.DamageReductionBonus += (int)buffValue;
			break;
		case PropertyChangeType.ChangeConvertedToHealth:
			detailedProperties.DamageToHealthBonus = (int)buffValue;
			break;
		case PropertyChangeType.ChangeAllResistance:
			detailedProperties.ElementsResistanceBonus[0] += (int)buffValue;
			detailedProperties.ElementsResistanceBonus[1] += (int)buffValue;
			detailedProperties.ElementsResistanceBonus[2] += (int)buffValue;
			break;
		case PropertyChangeType.ChangeExplosionDamageReduction:
			detailedProperties.ExplosionDamageReductionBonus += (int)buffValue;
			break;
		case PropertyChangeType.ChangeDamageImmunityRate:
			detailedProperties.DamageImmunityRateBonus += buffValue * 0.01f;
			break;
		case PropertyChangeType.ChangeShieldRecoveryDelay:
			if (buffModifier == BuffModifier.VALUE)
			{
				detailedProperties.ShieldRecoverDelayBonus += buffValue;
			}
			else
			{
				detailedProperties.ShieldRecoverDelayBonusPercentage += buffValue * 0.01f;
			}
			break;
		case PropertyChangeType.ChangeDeathTime:
			detailedProperties.DeathTimeBonus += buffValue * 0.01f;
			break;
		case PropertyChangeType.ChangeTherapeuticGain:
			if (buffModifier == BuffModifier.VALUE)
			{
				detailedProperties.HpPercentageBonus += buffValue;
			}
			else
			{
				detailedProperties.HpRecoveryPercentageBonus += buffValue * 0.01f;
			}
			break;
		case PropertyChangeType.CapacityToHealth:
			detailedProperties.CapacitToHealthBonus += buffValue * 0.01f;
			break;
		case PropertyChangeType.ChangeRebirthHealth:
			detailedProperties.RebirthHealthBonus += buffValue * 0.01f;
			break;
		case PropertyChangeType.Penetration:
		{
			foreach (SkillTargetType skillTargetType10 in skillTargetTypes)
			{
				if (skillTargetType10 == SkillTargetType.AllWeapon)
				{
					detailedProperties.PenetrationBonusAll = true;
				}
				else if (SkillTargetType.None < skillTargetType10 && skillTargetType10 <= SkillTargetType.Grenade)
				{
					detailedProperties.PenetrationBonus[(int)(skillTargetType10 - 1)] = true;
				}
			}
			break;
		}
		}
	}

	public virtual void Move(Vector3 motion)
	{
	}

	public virtual void OnHit(DamageProperty dp)
	{
	}

	public virtual void OnDead()
	{
	}

	public virtual void OnKnocked(float speed)
	{
	}

	public virtual int DamageLevelSuppress(int originalDamage, int attackerLevel, int attackerWeaponLevel, int selfLevel)
	{
		return originalDamage;
	}
}

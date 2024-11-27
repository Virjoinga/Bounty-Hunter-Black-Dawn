using UnityEngine;

public class CharacterStateSkill : CharacterSkill
{
	public SkillTriggerType STriggerType;

	public short STriggerTypeSubValue;

	protected float startTime;

	public float Duration { get; set; }

	public byte BuffType { get; set; }

	public bool IsPermanent { get; set; }

	public PropertyChangeType PropertyChangeTypeOfBuff1 { get; set; }

	public BuffModifier ModifierOfBuff1 { get; set; }

	public BuffFunctionType FunctionType1 { get; set; }

	public ElementType ElementType1 { get; set; }

	public float BuffValueX1 { get; set; }

	public float BuffValueY1 { get; set; }

	public PropertyChangeType PropertyChangeTypeOfBuff2 { get; set; }

	public BuffModifier ModifierOfBuff2 { get; set; }

	public BuffFunctionType FunctionType2 { get; set; }

	public ElementType ElementType2 { get; set; }

	public float BuffValueX2 { get; set; }

	public float BuffValueY2 { get; set; }

	public PropertyChangeType PropertyChangeTypeOfBuff3 { get; set; }

	public BuffModifier ModifierOfBuff3 { get; set; }

	public BuffFunctionType FunctionType3 { get; set; }

	public ElementType ElementType3 { get; set; }

	public float BuffValueX3 { get; set; }

	public float BuffValueY3 { get; set; }

	public CharacterStateSkill()
	{
		IsPermanent = false;
	}

	public CharacterStateSkill(short buffID)
	{
		IsPermanent = false;
		BuffConfig buffConfig = GameConfig.GetInstance().buffConfig[buffID];
		BuffConfig buffConfig2 = null;
		if (GameConfig.GetInstance().buffConfig.ContainsKey((short)(buffID + 1)) && GameConfig.GetInstance().buffConfig[(short)(buffID + 1)].BuffType == buffConfig.BuffType)
		{
			buffConfig2 = GameConfig.GetInstance().buffConfig[(short)(buffID + 1)];
		}
		buffConfig.CurrentDescribValue = "Current Rank: [00eaff]";
		buffConfig.NextDescribValue = "Next Rank: [00eaff]";
		base.skillID = buffID;
		BuffType = buffConfig.BuffType;
		FunctionType1 = buffConfig.FunctionType1;
		if (FunctionType1 != 0)
		{
			switch (FunctionType1)
			{
			case BuffFunctionType.PropertyChange:
			{
				int x = buffConfig.X1;
				ModifierOfBuff1 = (BuffModifier)(x / 100);
				PropertyChangeTypeOfBuff1 = (PropertyChangeType)(x % 100);
				BuffValueY1 = buffConfig.Y1;
				if (PropertyChangeTypeOfBuff1 == PropertyChangeType.ChangeReloadSpeed || PropertyChangeTypeOfBuff1 == PropertyChangeType.ChangeShieldRecoveryDelay)
				{
					buffConfig.CurrentDescribValue += -buffConfig.Y1;
				}
				else
				{
					buffConfig.CurrentDescribValue += buffConfig.Y1;
				}
				if (ModifierOfBuff1 == BuffModifier.PERCENTAGE)
				{
					buffConfig.CurrentDescribValue += "%";
				}
				if (buffConfig2 != null)
				{
					if (PropertyChangeTypeOfBuff1 == PropertyChangeType.ChangeReloadSpeed || PropertyChangeTypeOfBuff1 == PropertyChangeType.ChangeShieldRecoveryDelay)
					{
						buffConfig.NextDescribValue += -buffConfig2.Y1;
					}
					else
					{
						buffConfig.NextDescribValue += buffConfig2.Y1;
					}
					if (ModifierOfBuff1 == BuffModifier.PERCENTAGE)
					{
						buffConfig.NextDescribValue += "%";
					}
				}
				break;
			}
			case BuffFunctionType.MakeDamage:
				ElementType1 = (ElementType)((float)buffConfig.X1 * 1E-05f);
				BuffValueX1 = (float)(buffConfig.X1 % 100000) * 0.01f;
				BuffValueY1 = buffConfig.Y1 % 100000;
				break;
			case BuffFunctionType.MakeHeal:
				BuffValueX1 = (float)buffConfig.X1 * 0.01f;
				BuffValueY1 = buffConfig.Y1;
				if (buffConfig.X1 != 0)
				{
					buffConfig.CurrentDescribValue = buffConfig.CurrentDescribValue + buffConfig.X1 + "%";
					if (buffConfig2 != null)
					{
						buffConfig.NextDescribValue = buffConfig.NextDescribValue + buffConfig2.X1 + "%";
					}
				}
				else
				{
					buffConfig.CurrentDescribValue += buffConfig.Y1;
					if (buffConfig2 != null)
					{
						buffConfig.NextDescribValue += buffConfig2.Y1;
					}
				}
				break;
			case BuffFunctionType.MakeStun:
				BuffValueY1 = (float)buffConfig.Y1 * 0.01f;
				buffConfig.CurrentDescribValue = buffConfig.CurrentDescribValue + buffConfig.Y1 + "%";
				if (buffConfig2 != null)
				{
					buffConfig.NextDescribValue = buffConfig.NextDescribValue + buffConfig2.Y1 + "%";
				}
				break;
			case BuffFunctionType.SpeedDown:
				BuffValueX1 = buffConfig.X1;
				BuffValueY1 = buffConfig.Y1;
				if (buffConfig.BuffType == 5 || buffConfig.BuffType == 15)
				{
					buffConfig.CurrentDescribValue += buffConfig.X1;
					if (buffConfig2 != null)
					{
						buffConfig.NextDescribValue += buffConfig2.X1;
					}
				}
				else
				{
					buffConfig.CurrentDescribValue += buffConfig.Y1;
					if (buffConfig2 != null)
					{
						buffConfig.NextDescribValue += buffConfig2.Y1;
					}
				}
				break;
			}
			buffConfig.CurrentDescribValue += "[-]";
			buffConfig.NextDescribValue += "[-]";
		}
		FunctionType2 = buffConfig.FunctionType2;
		if (FunctionType2 != 0)
		{
			switch (FunctionType2)
			{
			case BuffFunctionType.PropertyChange:
			{
				int x2 = buffConfig.X2;
				ModifierOfBuff2 = (BuffModifier)(x2 / 100);
				PropertyChangeTypeOfBuff2 = (PropertyChangeType)(x2 % 100);
				BuffValueY2 = buffConfig.Y2;
				break;
			}
			case BuffFunctionType.MakeDamage:
				ElementType2 = (ElementType)((float)buffConfig.X2 * 1E-05f);
				BuffValueX2 = (float)(buffConfig.X2 % 100000) * 0.01f;
				BuffValueY2 = buffConfig.Y2 % 100000;
				break;
			case BuffFunctionType.MakeHeal:
				BuffValueX2 = (float)buffConfig.X2 * 0.01f;
				BuffValueY2 = buffConfig.Y2;
				break;
			case BuffFunctionType.MakeStun:
				BuffValueY2 = (float)buffConfig.Y2 * 0.01f;
				break;
			case BuffFunctionType.SpeedDown:
				BuffValueX2 = buffConfig.X2;
				BuffValueY2 = buffConfig.Y2;
				break;
			}
		}
		FunctionType3 = buffConfig.FunctionType3;
		if (FunctionType3 != 0)
		{
			switch (FunctionType3)
			{
			case BuffFunctionType.PropertyChange:
			{
				int x3 = buffConfig.X3;
				ModifierOfBuff3 = (BuffModifier)(x3 / 100);
				PropertyChangeTypeOfBuff3 = (PropertyChangeType)(x3 % 100);
				BuffValueY3 = buffConfig.Y3;
				break;
			}
			case BuffFunctionType.MakeDamage:
				ElementType3 = (ElementType)((float)buffConfig.X3 * 1E-05f);
				BuffValueX3 = (float)(buffConfig.X3 % 100000) * 0.01f;
				BuffValueY3 = buffConfig.Y3 % 100000;
				break;
			case BuffFunctionType.MakeHeal:
				BuffValueX3 = (float)buffConfig.X3 * 0.01f;
				BuffValueY3 = buffConfig.Y3;
				break;
			case BuffFunctionType.MakeStun:
				BuffValueY3 = (float)buffConfig.Y3 * 0.01f;
				break;
			case BuffFunctionType.Steal:
				break;
			case BuffFunctionType.SpeedDown:
				BuffValueX3 = buffConfig.X3;
				BuffValueY3 = buffConfig.Y3;
				break;
			case BuffFunctionType.CanNotSteal:
				break;
			case (BuffFunctionType)2:
			case (BuffFunctionType)3:
				break;
			}
		}
	}

	protected bool MeetTriggerType()
	{
		switch (STriggerType)
		{
		case SkillTriggerType.OnShieldLessThanPercentage:
			if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.MaxShield > 0)
			{
				float num = (float)GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.Shield / (float)GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.MaxShield;
				if (num * 100f < (float)STriggerTypeSubValue)
				{
					return true;
				}
			}
			return false;
		case SkillTriggerType.OnNearSummonedItem:
			return false;
		default:
			return true;
		}
	}

	public override void ApplySkill(GameUnit unit)
	{
		if (MeetTriggerType())
		{
			switch (FunctionType1)
			{
			case BuffFunctionType.PropertyChange:
				unit.AddBuff(PropertyChangeTypeOfBuff1, ModifierOfBuff1, BuffValueY1, base.Target, base.TargetTypes);
				break;
			case BuffFunctionType.SpeedDown:
				unit.AddBuff(PropertyChangeType.ChangeMoveSpeed, BuffModifier.PERCENTAGE, 0f - BuffValueX1, base.Target, base.TargetTypes);
				break;
			}
			switch (FunctionType2)
			{
			case BuffFunctionType.PropertyChange:
				unit.AddBuff(PropertyChangeTypeOfBuff2, ModifierOfBuff2, BuffValueY2, base.Target, base.TargetTypes);
				break;
			case BuffFunctionType.SpeedDown:
				unit.AddBuff(PropertyChangeType.ChangeMoveSpeed, BuffModifier.PERCENTAGE, 0f - BuffValueX2, base.Target, base.TargetTypes);
				break;
			}
			switch (FunctionType3)
			{
			case BuffFunctionType.PropertyChange:
				unit.AddBuff(PropertyChangeTypeOfBuff3, ModifierOfBuff3, BuffValueY3, base.Target, base.TargetTypes);
				break;
			case BuffFunctionType.MakeDamage:
				break;
			case BuffFunctionType.MakeHeal:
				break;
			case BuffFunctionType.MakeStun:
				break;
			case BuffFunctionType.Steal:
				break;
			case BuffFunctionType.SpeedDown:
				unit.AddBuff(PropertyChangeType.ChangeMoveSpeed, BuffModifier.PERCENTAGE, 0f - BuffValueX3, base.Target, base.TargetTypes);
				break;
			case BuffFunctionType.CanNotSteal:
				break;
			case (BuffFunctionType)2:
			case (BuffFunctionType)3:
				break;
			}
		}
	}

	public override bool IsInstantSkill()
	{
		return false;
	}

	public bool TimeUp()
	{
		if (IsPermanent)
		{
			return false;
		}
		if (Time.time - startTime > Duration)
		{
			return true;
		}
		return false;
	}

	public int GetLeftTime()
	{
		return Mathf.CeilToInt(Duration - (Time.time - startTime));
	}

	public void StartBuff()
	{
		startTime = Time.time;
	}
}

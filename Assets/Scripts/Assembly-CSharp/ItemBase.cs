using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : ItemScript
{
	public const int MAX_SKILL_COUNT = 4;

	public NGUIBaseItem mNGUIBaseItem = new NGUIBaseItem();

	protected List<SkillInfo> Skills = new List<SkillInfo>();

	protected List<string> FixedSkillProperties = new List<string>();

	private InGameUIScript mInGameUIScript;

	protected bool HasAddedToItemRoot;

	protected bool HasAddedLine;

	public static float[] QUALITY_TYPE_PARAs = new float[6] { 0f, 0.4f, 1f, 1.2f, 1.4f, 1.6f };

	public static float[] QUALITY_PRICE_TYPE_PARAs = new float[6] { 0f, 0.4f, 1f, 1.4f, 2f, 3f };

	public short SpecialID { get; set; }

	public ItemQuality Quality { get; set; }

	public string ItemName { get; set; }

	public string LegendaryName { get; set; }

	public ItemClasses ItemClass { get; set; }

	public ItemCompanyName ItemCompany { get; set; }

	public byte ItemType { get; set; }

	public string ItemStyle { get; set; }

	public string ItemColor { get; set; }

	public byte ItemLevel { get; set; }

	public short FloorLimitScore { get; set; }

	public short UpperLimitScore { get; set; }

	public float FormulaParameter1 { get; set; }

	public float FormulaParameter2 { get; set; }

	public virtual void generateItemProperties()
	{
		SetItemClass();
	}

	public abstract void affectItemPropertiesByLevelAndFormula();

	public virtual void generateEquipmentSkills()
	{
		Skills.Clear();
		int[] array = new int[4];
		bool flag = false;
		int num = 0;
		int num2 = -1;
		while (!flag)
		{
			num = 0;
			if (Quality == ItemQuality.Legendary)
			{
				if (ItemType == 0)
				{
					for (int i = 0; i < array.Length - 1; i++)
					{
						array[i] = Random.Range(1, 5) * 2 - 1;
					}
					if (ItemCompany == ItemCompanyName.Kypton)
					{
						array[3] = Random.Range(30, 34);
					}
					else
					{
						array[3] = Random.Range(30, 35);
					}
				}
				else if (ItemType == 1)
				{
					for (int j = 0; j < array.Length - 1; j++)
					{
						array[j] = Random.Range(1, 5) * 2 - 1;
					}
					if (ItemCompany == ItemCompanyName.Kypton)
					{
						array[3] = Random.Range(30, 33);
					}
					else
					{
						array[3] = Random.Range(30, 32);
					}
				}
				else if (ItemType == 2)
				{
					array[0] = Random.Range(1, 5) * 4 - 2;
					if (ItemCompany == ItemCompanyName.Kypton)
					{
						array[1] = Random.Range(30, 33);
					}
					else
					{
						array[1] = Random.Range(30, 36);
					}
				}
			}
			else if (ItemType == 0 || ItemType == 1)
			{
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = Random.Range(1, 5) * 2 - 1;
				}
			}
			else if (ItemType == 2)
			{
				for (int l = 0; l < 2; l++)
				{
					array[l] = Random.Range(1, 5) * 4 - 2;
				}
				array[2] = 0;
				array[3] = 0;
			}
			if (ItemType == 0 || ItemType == 1)
			{
				for (int m = 0; m < array.Length - 1; m++)
				{
					for (int n = 0; n < array.Length - 1 - m; n++)
					{
						if (array[n] > array[n + 1])
						{
							int num3 = array[n];
							array[n] = array[n + 1];
							array[n + 1] = num3;
						}
					}
				}
			}
			else if (ItemType == 2 && array[0] > array[1])
			{
				int num4 = array[0];
				array[0] = array[1];
				array[1] = num4;
			}
			int num5 = 0;
			for (int num6 = 0; num6 < array.Length; num6++)
			{
				num5 += array[num6];
				if (FloorLimitScore <= num5 && num5 <= UpperLimitScore)
				{
					num = num6 + 1;
					flag = true;
					for (int num7 = num6 + 1; num7 < array.Length; num7++)
					{
						array[num7] = 0;
					}
					break;
				}
			}
		}
		if (Quality == ItemQuality.Legendary)
		{
			num2 = num - 1;
		}
		else if (ItemCompany == ItemCompanyName.Kypton)
		{
			num2 = Random.Range(0, num);
		}
		for (int num8 = 0; num8 < array.Length; num8++)
		{
			if (array[num8] != 0)
			{
				bool isSpecialPrefix = num8 == num2;
				generateSkill(array[num8], isSpecialPrefix, false);
			}
		}
	}

	public virtual void generateNGUIBaseItem()
	{
		mNGUIBaseItem.SetPrice((50 + (int)((float)(ItemLevel * ItemLevel * (50 + calculateSkillScore() * 5)) * QUALITY_PRICE_TYPE_PARAs[(int)Quality] / 20f)) / 2);
		mNGUIBaseItem.Quality = Quality;
		mNGUIBaseItem.ItemLevel = ItemLevel;
		mNGUIBaseItem.Company = ItemCompany;
		mNGUIBaseItem.iconName = ItemName;
		mNGUIBaseItem.previewIconName = "p_" + ItemName;
	}

	protected abstract void GetIntoBackPack();

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		CheckIfGoThroughGround();
		if (base.GetComponent<Rigidbody>() != null && base.GetComponent<Rigidbody>().IsSleeping() && !HasAddedLine)
		{
			HasAddedLine = true;
			GameApp.GetInstance().GetLootManager().AddPromptLine(base.transform, Quality, ItemClass);
		}
		if (base.transform.position.y < -75f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected new virtual void OnTriggerEnter(Collider c)
	{
		base.OnTriggerEnter(c);
	}

	public void generateLimitScore()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[22];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			ItemQuality data = (ItemQuality)unitDataTable.GetData(i, 0, 0, false);
			if (Quality == data)
			{
				FloorLimitScore = (short)unitDataTable.GetData(i, 1, 0, false);
				UpperLimitScore = (short)unitDataTable.GetData(i, 2, 0, false);
				break;
			}
		}
	}

	protected int calculateSkillScore()
	{
		int num = 0;
		for (int i = 0; i < Skills.Count; i++)
		{
			num += Skills[i].Score;
		}
		return num;
	}

	public virtual void PickUpItem()
	{
		PickUpItem(true);
	}

	public virtual void PickUpItem(bool needDestroySelf)
	{
		GetIntoBackPack();
		GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressItemCollection(SpecialID);
		if (needDestroySelf)
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected void CheckIfFloatStringIsEmpty(ref string _string)
	{
		if (string.IsNullOrEmpty(_string))
		{
			_string = "0";
		}
	}

	protected void SetItemClass()
	{
		mNGUIBaseItem.ItemClass = ItemClass;
	}

	protected void generateSkill(int skillScore, bool isSpecialPrefix, bool isScopePrefix)
	{
		int[] array;
		if (ItemType == 0)
		{
			if (!isScopePrefix)
			{
				if (!isSpecialPrefix)
				{
					array = ((ItemClass != ItemClasses.RPG) ? new int[9] { 1, 2, 3, 4, 5, 6, 11, 12, 13 } : new int[10] { 1, 2, 3, 4, 5, 6, 11, 12, 13, 22 });
				}
				else if (Quality == ItemQuality.Legendary)
				{
					switch (ItemCompany)
					{
					case ItemCompanyName.D_haka:
						array = new int[1] { 40 };
						break;
					case ItemCompanyName.Mata:
						array = new int[1] { 41 };
						break;
					case ItemCompanyName.Kypton:
						array = new int[1] { 42 };
						break;
					case ItemCompanyName.Freyr:
						array = new int[1] { 43 };
						break;
					default:
						Debug.Log("Legendary Weapon MUST be company 1~4");
						return;
					}
				}
				else
				{
					if (ItemCompany != ItemCompanyName.Kypton)
					{
						Debug.Log("NONE-Legendary Weapon must be K company to have a Special Prefix");
						return;
					}
					array = new int[3] { 7, 8, 9 };
				}
			}
			else
			{
				array = new int[1] { 99 };
			}
		}
		else if (ItemType == 1)
		{
			array = ((Quality == ItemQuality.Legendary) ? ((!isSpecialPrefix) ? new int[7] { 1, 11, 12, 13, 20, 21, 22 } : ((ItemCompany != ItemCompanyName.Kypton) ? new int[1] { 47 } : new int[1] { 48 })) : ((ItemCompany != ItemCompanyName.Kypton || !isSpecialPrefix) ? new int[7] { 1, 11, 12, 13, 20, 21, 22 } : new int[3] { 7, 8, 9 }));
		}
		else
		{
			if (ItemType != 2)
			{
				Debug.Log("This type of item should not have a prefix.");
				return;
			}
			if (isSpecialPrefix)
			{
				if (Quality == ItemQuality.Legendary)
				{
					array = ((ItemCompany != ItemCompanyName.Kypton) ? new int[1] { 44 } : new int[1] { 46 });
				}
				else
				{
					if (ItemCompany != ItemCompanyName.Kypton)
					{
						Debug.Log("NONE-Legendary Shield must be K company to have a Special Prefix");
						return;
					}
					array = new int[3] { 17, 18, 19 };
				}
			}
			else
			{
				array = new int[5] { 14, 15, 16, 20, 21 };
			}
		}
		int num = array[Random.Range(0, array.Length)];
		if (ItemType == 1 && num >= 14 && num <= 21)
		{
			skillScore *= 2;
		}
		short skillID = (short)(skillScore * 100 + num);
		AddSkillInfo(skillID);
	}

	public void AddSkillInfo(short skillID)
	{
		SkillInfo item = default(SkillInfo);
		EquipPrefixConfig equipPrefixConfig = GameConfig.GetInstance().equipPrefixConfig[skillID];
		item.ID = equipPrefixConfig.ID;
		item.Name = equipPrefixConfig.Name;
		item.Quality = equipPrefixConfig.Quality;
		item.Score = equipPrefixConfig.Score;
		item.PrefixType = equipPrefixConfig.PrefixType;
		item.Damage = equipPrefixConfig.Damage;
		item.Accurancy = equipPrefixConfig.Accurancy;
		item.AttackInterval = equipPrefixConfig.AttackInterval;
		item.ReloadTime = equipPrefixConfig.ReloadTime;
		item.Mags = equipPrefixConfig.Mags;
		item.Recoil = equipPrefixConfig.Recoil;
		item.FireElement = equipPrefixConfig.FireElement;
		item.ShockElement = equipPrefixConfig.ShockElement;
		item.CorrosiveElement = equipPrefixConfig.CorrosiveElement;
		item.BulletRecovery = equipPrefixConfig.BulletRecovery;
		item.CriticalRate = equipPrefixConfig.CriticalRate;
		item.CriticalDamage = equipPrefixConfig.CriticalDamage;
		item.MeleeDamage = equipPrefixConfig.MeleeDamage;
		item.ShieldCapacity = equipPrefixConfig.ShieldCapacity;
		item.ShieldRecovery = equipPrefixConfig.ShieldRecovery;
		item.ShieldRecoveryDelay = equipPrefixConfig.ShieldRecoveryDelay;
		item.FireResistance = equipPrefixConfig.FireResistance;
		item.ShockResistance = equipPrefixConfig.ShockResistance;
		item.CorrosiveResistance = equipPrefixConfig.CorrosiveResistance;
		item.MaxHp = equipPrefixConfig.MaxHp;
		item.HpRecovery = equipPrefixConfig.HpRecovery;
		item.ExplosionRange = equipPrefixConfig.ExplosionRange;
		item.Speed = equipPrefixConfig.Speed;
		item.DamageReduction = equipPrefixConfig.DamageReduction;
		item.DamageToHealth = equipPrefixConfig.DamageToHealth;
		item.AllElementResistance = equipPrefixConfig.AllElementResistance;
		item.ExplosionDamageReduction = equipPrefixConfig.ExplosionDamageReduction;
		item.DamageImmune = equipPrefixConfig.DamageImmune;
		item.DropRate = equipPrefixConfig.DropRate;
		item.ElementTriggerRate = equipPrefixConfig.ElementTriggerRate;
		item.ScopeRate = equipPrefixConfig.ScopeRate;
		Skills.Add(item);
	}

	public void AddNGUIItemStat(NGUIItemStat.StatType statType, float statValue)
	{
		if (mNGUIBaseItem != null)
		{
			NGUIItemStat nGUIItemStat = new NGUIItemStat();
			nGUIItemStat.statType = statType;
			nGUIItemStat.statValue = statValue;
			mNGUIBaseItem.itemStats.Add(nGUIItemStat);
		}
	}

	public bool ItemCanBePickedUp()
	{
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		bool flag = !itemInfoData.BackPackIsFull();
		bool flag2 = false;
		switch ((ItemTypes)ItemType)
		{
		case ItemTypes.ITEM_TYPE_WEAPON:
			flag2 = !itemInfoData.IsWeapon1Equiped || !itemInfoData.IsWeapon2Equiped || (itemInfoData.IsWeapon3Enable && !itemInfoData.IsWeapon3Equiped) || (itemInfoData.IsWeapon4Enable && !itemInfoData.IsWeapon4Equiped);
			break;
		case ItemTypes.ITEM_TYPE_WEAPON_G:
			flag2 = !itemInfoData.IsHandGrenadeEquiped;
			break;
		case ItemTypes.ITEM_TYPE_SHIELD:
			flag2 = !itemInfoData.IsShieldEquiped;
			break;
		case ItemTypes.ITEM_TYPE_SLOT:
			flag2 = !itemInfoData.IsSlot1Equiped || (itemInfoData.IsSlot2Enable && !itemInfoData.IsSlot2Equiped) || (itemInfoData.IsSlot3Enable && !itemInfoData.IsSlot3Equiped) || (itemInfoData.IsSlot4Enable && !itemInfoData.IsSlot4Equiped);
			break;
		case ItemTypes.ITEM_TYPE_PILL:
			flag2 = false;
			break;
		}
		return flag || flag2;
	}

	public static string GetItemClassString(ItemTypes itemType)
	{
		return itemType.ToString();
	}

	public void CheckIfGoThroughGround()
	{
		if (!(base.GetComponent<Rigidbody>() != null))
		{
			return;
		}
		RaycastHit hitInfo = default(RaycastHit);
		Ray ray = new Ray(base.transform.position + Vector3.up * 5f, Vector3.down);
		if (Physics.Raycast(ray, out hitInfo, 5f, 1 << PhysicsLayer.FLOOR))
		{
			base.transform.position = new Vector3(base.transform.position.x, hitInfo.point.y, base.transform.position.z);
			base.transform.rotation = Quaternion.identity;
			if (ItemType != 7)
			{
				base.transform.RotateAroundLocal(Vector3.right, 90f);
			}
			base.GetComponent<Rigidbody>().Sleep();
		}
	}

	public static string ItemClassName(ItemClasses itemClass)
	{
		string key = string.Empty;
		switch (itemClass)
		{
		case ItemClasses.SubmachineGun:
			key = "LOC_WEAPON_SMG";
			break;
		case ItemClasses.AssultRifle:
			key = "LOC_WEAPON_ASSAULT";
			break;
		case ItemClasses.Pistol:
			key = "LOC_WEAPON_PISTOL";
			break;
		case ItemClasses.Revolver:
			key = "LOC_WEAPON_REVOLVER";
			break;
		case ItemClasses.Shotgun:
			key = "LOC_WEAPON_SHOTGUN";
			break;
		case ItemClasses.Sniper:
			key = "LOC_WEAPON_SNIPER";
			break;
		case ItemClasses.RPG:
			key = "LOC_WEAPON_RPG";
			break;
		case ItemClasses.Grenade:
			key = "LOC_WEAPON_GRENADE";
			break;
		case ItemClasses.U_Shield:
			key = "LOC_ITEM_SHILED";
			break;
		case ItemClasses.V_Slot:
			key = "LOC_ITEM_CHIP";
			break;
		case ItemClasses.W_Pills:
			key = "LOC_ITEM_HP";
			break;
		}
		return LocalizationManager.GetInstance().GetString(key);
	}
}

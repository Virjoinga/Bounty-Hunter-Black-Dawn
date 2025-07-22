using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class GameConfig
{
	protected static GameConfig instance;

	public Dictionary<short, DropConfig> enemyDropConfig = new Dictionary<short, DropConfig>();

	public Dictionary<string, ChestDropConfig> chestDropConfigName = new Dictionary<string, ChestDropConfig>();

	public Dictionary<string, WeaponConfig> weaponConfig = new Dictionary<string, WeaponConfig>();

	public Dictionary<string, ShieldConfig> shieldConfig = new Dictionary<string, ShieldConfig>();

	public Dictionary<short, EquipPrefixConfig> equipPrefixConfig = new Dictionary<short, EquipPrefixConfig>();

	public Dictionary<short, ChipPrefixConfig> chipPrefixConfig = new Dictionary<short, ChipPrefixConfig>();

	public Dictionary<string, short> equipIconConfig = new Dictionary<string, short>();

	public Dictionary<short, SpecialItemConfig> specialItemConfig = new Dictionary<short, SpecialItemConfig>();

	public Dictionary<string, SpecialItemConfig> specialItemConfigCallName = new Dictionary<string, SpecialItemConfig>();

	public Dictionary<int, SkillConfig> skillConfig = new Dictionary<int, SkillConfig>();

	public Dictionary<short, BuffConfig> buffConfig = new Dictionary<short, BuffConfig>();

	public Dictionary<string, NpcConfig> npcConfig = new Dictionary<string, NpcConfig>();

	public Dictionary<string, SceneConfig> sceneConfig = new Dictionary<string, SceneConfig>();

	public Dictionary<int, AreaConfig> areaConfig = new Dictionary<int, AreaConfig>();

	public Dictionary<byte, List<InstancePortalConfig>> instacePortalConfig = new Dictionary<byte, List<InstancePortalConfig>>();

	public Dictionary<string, EnemyConfig> enemyConfig = new Dictionary<string, EnemyConfig>();

	public Dictionary<int, EnemyConfig> enemyConfigId = new Dictionary<int, EnemyConfig>();

	public Dictionary<string, QuestEnemySpawnConfig> questEnemySpawnConfig = new Dictionary<string, QuestEnemySpawnConfig>();

	public Dictionary<byte, List<string>> tipsConfig = new Dictionary<byte, List<string>>();

	public GlobalConfig globalConf;

	public PlayerConfig playerConf;

	public ArrayList avatarConfTable = new ArrayList();

	public Hashtable monsterConfTable = new Hashtable();

	public ArrayList weaponConfTable = new ArrayList();

	public Hashtable equipConfTable = new Hashtable();

	public static GameConfig GetInstance()
	{
		if (instance == null)
		{
			instance = new GameConfig();
		}
		return instance;
	}

	public void LoadTipsConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[74];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			byte key = (byte)unitDataTable.GetData(i, 0, 0, false);
			string data = unitDataTable.GetData(i, 1, string.Empty, false);
			if (!tipsConfig.ContainsKey(key))
			{
				tipsConfig.Add(key, new List<string>());
			}
			tipsConfig[key].Add(data);
		}
	}

	public void LoadEnemyConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[38];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			int iRow = i;
			int num = 0;
			EnemyConfig enemyConfig = new EnemyConfig();
			enemyConfig.UniqueID = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.CallName = unitDataTable.GetData(iRow, num, string.Empty, false);
			num++;
			enemyConfig.GroupID = (short)unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.DisplayName = unitDataTable.GetData(iRow, num, string.Empty, false);
			num++;
			enemyConfig.EnemyType = unitDataTable.GetData(iRow, num, string.Empty, false);
			num++;
			enemyConfig.MaxHp = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.MaxShield = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.Experience = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.Gold = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.IdleTime = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.IdleTime);
			num++;
			enemyConfig.OnHitInterval = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.OnHitInterval);
			num++;
			enemyConfig.ShieldRecoveryInterval = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.ShieldRecoveryInterval);
			num++;
			enemyConfig.ShieldRecoverySpeed = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.PatrolIdleTime = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.PatrolIdleTime);
			num++;
			enemyConfig.PatrolSpeed = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.PatrolSpeed);
			num++;
			enemyConfig.WalkSpeed = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.WalkSpeed);
			num++;
			enemyConfig.RunSpeed = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RunSpeed);
			num++;
			enemyConfig.DetectRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.DetectSectorRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.DetectSectorAngle = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.ActivityRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.MeleeAttackRadius = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.MeleeAttackRadius);
			num++;
			enemyConfig.MeleeAttackProbability = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.MeleeAttackDamage1 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.MeleeAttackDamage2 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RushAttackRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RushAttackProbability = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RushAttackDamage1 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RushAttackSpeed1 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RushAttackSpeed1);
			num++;
			enemyConfig.RushAttackDamage2 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RushAttackSpeed2 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RushAttackSpeed2);
			num++;
			enemyConfig.RangedStandAttackRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedMoveAttackRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedAttackToCatchRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedAttackProbability = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedAttackDamage1 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedExtraDamage1 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedOneShotTime1 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedOneShotTime1);
			num++;
			enemyConfig.RangedInterval1 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedInterval1);
			num++;
			enemyConfig.RangedBulletCount1 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedBulletSpeed1 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedBulletSpeed1);
			num++;
			enemyConfig.RangedExplosionRadius1 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedExplosionRadius1);
			num++;
			enemyConfig.RangedAttackDamage2 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedExtraDamage2 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedOneShotTime2 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedOneShotTime2);
			num++;
			enemyConfig.RangedInterval2 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedInterval2);
			num++;
			enemyConfig.RangedBulletCount2 = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.RangedBulletSpeed2 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedBulletSpeed2);
			num++;
			enemyConfig.RangedExplosionRadius2 = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.RangedExplosionRadius2);
			num++;
			enemyConfig.CoverSearchRadius = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.CoverInterval = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.CoverInterval);
			num++;
			enemyConfig.CoverHideIdleTime = 0f;
			float.TryParse(unitDataTable.GetData(iRow, num, string.Empty, false), out enemyConfig.CoverHideIdleTime);
			num++;
			enemyConfig.CoverExposeCount = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.CoverAttackCount = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfig.CoverBulletCount = unitDataTable.GetData(iRow, num, 0, false);
			num++;
			enemyConfigId.Add(enemyConfig.UniqueID, enemyConfig);
			this.enemyConfig.Add(enemyConfig.CallName, enemyConfig);
		}
	}

	public void LoadQuestEnemySpawnConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[51];
		QuestEnemySpawnConfig questEnemySpawnConfig = null;
		string text = string.Empty;
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string empty = string.Empty;
			empty = unitDataTable.GetData(i, 0, string.Empty, false);
			if (empty != string.Empty)
			{
				if (text != string.Empty && questEnemySpawnConfig != null)
				{
					if (!this.questEnemySpawnConfig.ContainsKey(text))
					{
						this.questEnemySpawnConfig.Add(text, questEnemySpawnConfig);
					}
					else
					{
						Debug.LogError("Quest Spawn Point Name Conflict: " + text);
					}
				}
				text = empty;
				questEnemySpawnConfig = new QuestEnemySpawnConfig();
				questEnemySpawnConfig.QuestId = (short)unitDataTable.GetData(i, 1, 0, false);
			}
			EnemySpawnInfo enemySpawnInfo = new EnemySpawnInfo();
			string data = unitDataTable.GetData(i, 2, string.Empty, false);
			EnemyConfig enemyConfig = GetInstance().enemyConfig[data];
			enemySpawnInfo.EType = (EnemyType)(int)Enum.Parse(typeof(EnemyType), enemyConfig.EnemyType, true);
			enemySpawnInfo.UniqueID = enemyConfig.UniqueID;
			enemySpawnInfo.From = SpawnFromType.Door;
			enemySpawnInfo.Count = unitDataTable.GetData(i, 3, 0, false);
			enemySpawnInfo.MinLevel = unitDataTable.GetData(i, 4, 0, false);
			enemySpawnInfo.MaxLevel = unitDataTable.GetData(i, 5, 0, false);
			questEnemySpawnConfig.EnemyInfos.Add(enemySpawnInfo);
		}
		if (text != string.Empty && questEnemySpawnConfig != null)
		{
			if (!this.questEnemySpawnConfig.ContainsKey(text))
			{
				this.questEnemySpawnConfig.Add(text, questEnemySpawnConfig);
			}
			else
			{
				Debug.LogError("Quest Spawn Point Name Conflict: " + text);
			}
		}
	}

	public void LoadAreaConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[44];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			AreaConfig areaConfig = new AreaConfig();
			areaConfig.Id = (byte)unitDataTable.GetData(i, 0, 0, false);
			string data = unitDataTable.GetData(i, 1, string.Empty, false);
			areaConfig.Name = LocalizationManager.GetInstance().GetString(data);
			this.areaConfig.Add(areaConfig.Id, areaConfig);
		}
	}

	public void LoadSceneConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[45];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			SceneConfig sceneConfig = new SceneConfig();
			string data = unitDataTable.GetData(i, 1, string.Empty, false);
			sceneConfig.SceneID = (byte)unitDataTable.GetData(i, 0, 0, false);
			sceneConfig.SceneFileName = data;
			sceneConfig.AreaID = (byte)unitDataTable.GetData(i, 2, 0, false);
			sceneConfig.FatherSceneID = (byte)unitDataTable.GetData(i, 3, 0, false);
			string data2 = unitDataTable.GetData(i, 4, string.Empty, false);
			sceneConfig.SceneName = LocalizationManager.GetInstance().GetString(data2);
			string data3 = unitDataTable.GetData(i, 5, string.Empty, false);
			sceneConfig.SceneIntro = LocalizationManager.GetInstance().GetString(data3);
			sceneConfig.MiniMapSize = SplitString2(unitDataTable.GetData(i, 6, string.Empty, false));
			sceneConfig.MapTopLeft = SplitString3(unitDataTable.GetData(i, 7, string.Empty, false));
			sceneConfig.MapBottomLeft = SplitString3(unitDataTable.GetData(i, 8, string.Empty, false));
			sceneConfig.MapTopRight = SplitString3(unitDataTable.GetData(i, 9, string.Empty, false));
			sceneConfig.MapBottomRight = SplitString3(unitDataTable.GetData(i, 10, string.Empty, false));
			sceneConfig.ArenaBelongToWhichSceneID = (byte)unitDataTable.GetData(i, 11, 0, false);
			sceneConfig.Hide = (byte)unitDataTable.GetData(i, 12, 0, false) == 1;
			this.sceneConfig.Add(data, sceneConfig);
		}
	}

	public void LoadInstancePortalConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[52];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			InstancePortalConfig instancePortalConfig = new InstancePortalConfig();
			instancePortalConfig.SceneID = (byte)unitDataTable.GetData(i, 0, 0, false);
			instancePortalConfig.Index = (byte)unitDataTable.GetData(i, 1, 0, false);
			instancePortalConfig.Pos = SplitString3(unitDataTable.GetData(i, 2, string.Empty, false));
			if (instacePortalConfig.ContainsKey(instancePortalConfig.SceneID))
			{
				List<InstancePortalConfig> list = instacePortalConfig[instancePortalConfig.SceneID];
				list.Add(instancePortalConfig);
			}
			else
			{
				List<InstancePortalConfig> list2 = new List<InstancePortalConfig>();
				list2.Add(instancePortalConfig);
				instacePortalConfig.Add(instancePortalConfig.SceneID, list2);
			}
		}
	}

	private Vector3 SplitString2(string str)
	{
		string[] array = SplitString(str, 2);
		return new Vector3(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]));
	}

	private Vector3 SplitString3(string str)
	{
		string[] array = SplitString(str, 3);
		return new Vector3(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]));
	}

	private string[] SplitString(string str, int count)
	{
		string[] separator = new string[1] { "|" };
		return str.Split(separator, count, StringSplitOptions.None);
	}

	public void LoadNpcConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[37];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			NpcConfig npcConfig = new NpcConfig();
			npcConfig.m_callName = unitDataTable.GetData(i, 1, string.Empty, false);
			npcConfig.m_id = (short)unitDataTable.GetData(i, 0, 0, false);
			npcConfig.m_name = unitDataTable.GetData(i, 2, string.Empty, false);
			npcConfig.m_state = unitDataTable.GetData(i, 3, string.Empty, false);
			npcConfig.m_point = new QuestPoint();
			npcConfig.m_point.m_siteId = (byte)unitDataTable.GetData(i, 4, 0, false);
			string data = unitDataTable.GetData(i, 5, string.Empty, false);
			npcConfig.m_point.SetPos(data);
			npcConfig.m_dialog = unitDataTable.GetData(i, 6, string.Empty, false);
			byte type = (byte)unitDataTable.GetData(i, 7, 0, false);
			npcConfig.m_type = (NpcType)type;
			this.npcConfig.Add(npcConfig.m_callName, npcConfig);
		}
	}

	public void LoadDropConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[0];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			int iCol = 0;
			DropConfig dropConfig = new DropConfig();
			dropConfig.ID = (short)unitDataTable.GetData(i, iCol, 0, false);
			iCol = 2;
			dropConfig.SMG_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.AssualtRifle_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.Pistol_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.Revolver_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.Shotgun_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.Sniper_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.RPG_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.Grenade_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.Shield_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.Slot_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.FirstAid_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			iCol++;
			dropConfig.BackPack_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP1_ID = (short)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP1_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP2_ID = (short)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP2_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP3_ID = (short)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP3_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP4_ID = (short)unitDataTable.GetData(i, iCol++, 0, false);
			dropConfig.SP4_Rate = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			iCol = 25;
			dropConfig.DropType = (byte)unitDataTable.GetData(i, iCol++, 0, false);
			enemyDropConfig.Add(dropConfig.ID, dropConfig);
		}
	}

	public void LoadChestDropConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[1];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			int num = 0;
			ChestDropConfig chestDropConfig = new ChestDropConfig();
			chestDropConfig.ChestName = unitDataTable.GetData(i, num++, string.Empty, false);
			chestDropConfig.ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SMG_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.AssualtRifle_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.Pistol_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.Revolver_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.Shotgun_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.Sniper_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.RPG_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.Grenade_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.Shield_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.Slot_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.FirstAid_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			num++;
			chestDropConfig.BackPack_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP1_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP1_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP2_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP2_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP3_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP3_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP4_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP4_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP5_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP5_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP6_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP6_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP7_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP7_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP8_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP8_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP9_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP9_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP10_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP10_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP11_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP11_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP12_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP12_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP13_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP13_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP14_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP14_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP15_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP15_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP16_ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfig.SP16_Rate = (byte)unitDataTable.GetData(i, num++, 0, false);
			chestDropConfigName.Add(chestDropConfig.ChestName, chestDropConfig);
		}
	}

	public void LoadEquipConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[2];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string data = unitDataTable.GetData(i, 0, string.Empty, false);
			string data2 = unitDataTable.GetData(i, 15, string.Empty, false);
			ItemClasses data3 = (ItemClasses)unitDataTable.GetData(i, 2, 0, false);
			ItemCompanyName data4 = (ItemCompanyName)unitDataTable.GetData(i, 1, 0, false);
			if (data3 == ItemClasses.U_Shield)
			{
				ShieldConfig shieldConfig = new ShieldConfig();
				shieldConfig.ItemName = data;
				shieldConfig.LegendaryName = data2;
				shieldConfig.ItemClass = data3;
				shieldConfig.ItemCompany = data4;
				shieldConfig.ShieldCapacity = (short)unitDataTable.GetData(i, 13, 0, false);
				shieldConfig.ShieldRecovery = (short)unitDataTable.GetData(i, 14, 0, false);
				this.shieldConfig.Add(shieldConfig.ItemName, shieldConfig);
				continue;
			}
			WeaponConfig weaponConfig = new WeaponConfig();
			weaponConfig.ItemName = data;
			weaponConfig.LegendaryName = data2;
			weaponConfig.ItemClass = data3;
			weaponConfig.ItemCompany = data4;
			weaponConfig.BasicDamage = (byte)unitDataTable.GetData(i, 3, 0, false);
			weaponConfig.Accurancy = (byte)unitDataTable.GetData(i, 4, 0, false);
			weaponConfig.BasicAttackInterval = Convert.ToSingle(unitDataTable.GetData(i, 5, string.Empty, false));
			weaponConfig.BasicReloadTime = Convert.ToSingle(unitDataTable.GetData(i, 6, string.Empty, false));
			weaponConfig.BasicMags = (byte)unitDataTable.GetData(i, 7, 0, false);
			weaponConfig.BasicRecoil = (byte)unitDataTable.GetData(i, 8, 0, false);
			weaponConfig.BasicMeleeDamage = (byte)unitDataTable.GetData(i, 9, 0, false);
			weaponConfig.BasicCriticalRate = (byte)unitDataTable.GetData(i, 10, 0, false);
			weaponConfig.BasicExplosionRange = (byte)unitDataTable.GetData(i, 11, 0, false);
			weaponConfig.BasicHaveScope = (byte)unitDataTable.GetData(i, 12, 0, false);
			this.weaponConfig.Add(weaponConfig.ItemName, weaponConfig);
			short value = (short)unitDataTable.GetData(i, 16, 0, false);
			equipIconConfig.Add(weaponConfig.ItemName, value);
		}
	}

	public void LoadSpecialItemConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[8];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			SpecialItemConfig specialItemConfig = new SpecialItemConfig();
			specialItemConfig.ID = (short)unitDataTable.GetData(i, 0, 0, false);
			specialItemConfig.CallName = unitDataTable.GetData(i, 1, string.Empty, false);
			specialItemConfig.ItemName = unitDataTable.GetData(i, 2, string.Empty, false);
			specialItemConfig.ItemClass = (ItemClasses)unitDataTable.GetData(i, 3, 0, false);
			specialItemConfig.ItemType = (ItemTypes)unitDataTable.GetData(i, 4, 0, false);
			specialItemConfig.ItemNumber = (byte)unitDataTable.GetData(i, 5, 0, false);
			specialItemConfig.Quality = (ItemQuality)unitDataTable.GetData(i, 6, 0, false);
			specialItemConfig.Description = unitDataTable.GetData(i, 7, string.Empty, false);
			specialItemConfig.IconName = unitDataTable.GetData(i, 8, string.Empty, false);
			specialItemConfig.PreviewIconName = unitDataTable.GetData(i, 9, string.Empty, false);
			specialItemConfig.PrefabName = unitDataTable.GetData(i, 10, string.Empty, false);
			this.specialItemConfig.Add(specialItemConfig.ID, specialItemConfig);
			specialItemConfigCallName.Add(specialItemConfig.CallName, specialItemConfig);
		}
	}

	public void LoadEquipPrefixConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[43];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			EquipPrefixConfig equipPrefixConfig = new EquipPrefixConfig();
			int num = 0;
			equipPrefixConfig.ID = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.Name = unitDataTable.GetData(i, num++, string.Empty, false);
			equipPrefixConfig.Quality = (ItemQuality)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.Score = (byte)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.PrefixType = (byte)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.Damage = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.Accurancy = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.AttackInterval = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ReloadTime = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.Mags = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.Recoil = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.FireElement = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ShockElement = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.CorrosiveElement = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.BulletRecovery = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.CriticalRate = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.CriticalDamage = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.MeleeDamage = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ShieldCapacity = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ShieldRecovery = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ShieldRecoveryDelay = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.FireResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ShockResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.CorrosiveResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.MaxHp = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.HpRecovery = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ExplosionRange = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.Speed = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.DamageReduction = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.DamageToHealth = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.AllElementResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ExplosionDamageReduction = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.DamageImmune = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.DropRate = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ElementTriggerRate = (short)unitDataTable.GetData(i, num++, 0, false);
			equipPrefixConfig.ScopeRate = (short)unitDataTable.GetData(i, num++, 0, false);
			this.equipPrefixConfig.Add(equipPrefixConfig.ID, equipPrefixConfig);
		}
	}

	public void LoadChipPrefixConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[73];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			ChipPrefixConfig chipPrefixConfig = new ChipPrefixConfig();
			int num = 0;
			chipPrefixConfig.ID = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Name = unitDataTable.GetData(i, num++, string.Empty, false);
			chipPrefixConfig.Quality = (ItemQuality)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Score = (byte)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.PrefixType = (byte)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Damage = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Accurancy = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.AttackInterval = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.ReloadTime = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Mags = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Recoil = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.BulletRecovery = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.CriticalRate = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.CriticalDamage = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.MeleeDamage = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.ShieldCapacity = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.ShieldRecovery = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.ShieldRecoveryDelay = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.FireResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.ShockResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.CorrosiveResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.MaxHp = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.HpRecovery = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Speed = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.DamageReduction = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.DamageToHealth = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.AllElementResistance = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.ExplosionDamageReduction = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.DamageImmune = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.DropRate = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.ElementTriggerRate = (short)unitDataTable.GetData(i, num++, 0, false);
			chipPrefixConfig.Company = (ItemCompanyName)unitDataTable.GetData(i, num++, 0, false);
			this.chipPrefixConfig.Add(chipPrefixConfig.ID, chipPrefixConfig);
		}
	}

	public void LoadSkillConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[40];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			SkillConfig skillConfig = new SkillConfig();
			skillConfig.ID = (short)unitDataTable.GetData(i, 0, 0, false);
			skillConfig.Level = (byte)unitDataTable.GetData(i, 1, 0, false);
			skillConfig.Name = unitDataTable.GetData(i, 2, string.Empty, false);
			skillConfig.SkillType = (SkillTypes)unitDataTable.GetData(i, 3, 0, false);
			skillConfig.STriggerType = (SkillTriggerType)unitDataTable.GetData(i, 4, 0, false);
			skillConfig.STriggerTypeSubValue = (short)unitDataTable.GetData(i, 5, 0, false);
			skillConfig.Target = (SkillTarget)unitDataTable.GetData(i, 8, 0, false);
			skillConfig.TargetTypes = new List<SkillTargetType>();
			SkillTargetType data = (SkillTargetType)unitDataTable.GetData(i, 9, 0, false);
			SkillTargetType data2 = (SkillTargetType)unitDataTable.GetData(i, 10, 0, false);
			SkillTargetType data3 = (SkillTargetType)unitDataTable.GetData(i, 11, 0, false);
			skillConfig.TargetTypes.Add(data);
			skillConfig.TargetTypes.Add(data2);
			skillConfig.TargetTypes.Add(data3);
			skillConfig.ApplyDelay = Convert.ToSingle(unitDataTable.GetData(i, 12, string.Empty, false));
			skillConfig.Distance = (short)unitDataTable.GetData(i, 13, 0, false);
			skillConfig.Range = (short)unitDataTable.GetData(i, 14, 0, false);
			skillConfig.CoolDownTime = Convert.ToSingle(unitDataTable.GetData(i, 15, string.Empty, false));
			skillConfig.FunctionType1 = (SkillFunctionType)unitDataTable.GetData(i, 16, 0, false);
			skillConfig.X1 = unitDataTable.GetData(i, 17, 0, false);
			skillConfig.Y1 = unitDataTable.GetData(i, 18, 0, false);
			skillConfig.FunctionType2 = (SkillFunctionType)unitDataTable.GetData(i, 19, 0, false);
			skillConfig.X2 = unitDataTable.GetData(i, 20, 0, false);
			skillConfig.Y2 = unitDataTable.GetData(i, 21, 0, false);
			skillConfig.FunctionType3 = (SkillFunctionType)unitDataTable.GetData(i, 22, 0, false);
			skillConfig.X3 = unitDataTable.GetData(i, 23, 0, false);
			skillConfig.Y3 = unitDataTable.GetData(i, 24, 0, false);
			skillConfig.IconName = unitDataTable.GetData(i, 25, string.Empty, false);
			skillConfig.ResourceName = unitDataTable.GetData(i, 26, string.Empty, false);
			skillConfig.Description1 = unitDataTable.GetData(i, 27, string.Empty, false);
			skillConfig.Description2 = unitDataTable.GetData(i, 28, string.Empty, false);
			skillConfig.CurrentDescribValue = string.Empty;
			skillConfig.NextDescribValue = string.Empty;
			this.skillConfig.Add(skillConfig.ID * 10 + skillConfig.Level, skillConfig);
		}
	}

	public void LoadBuffConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[41];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			BuffConfig buffConfig = new BuffConfig();
			buffConfig.ID = (short)unitDataTable.GetData(i, 0, 0, false);
			buffConfig.Level = (byte)unitDataTable.GetData(i, 1, 0, false);
			buffConfig.Name = unitDataTable.GetData(i, 2, string.Empty, false);
			buffConfig.BuffType = (byte)unitDataTable.GetData(i, 3, 0, false);
			buffConfig.FunctionType1 = (BuffFunctionType)unitDataTable.GetData(i, 4, 0, false);
			buffConfig.X1 = unitDataTable.GetData(i, 5, 0, false);
			buffConfig.Y1 = unitDataTable.GetData(i, 6, 0, false);
			buffConfig.FunctionType2 = (BuffFunctionType)unitDataTable.GetData(i, 7, 0, false);
			buffConfig.X2 = unitDataTable.GetData(i, 8, 0, false);
			buffConfig.Y2 = unitDataTable.GetData(i, 9, 0, false);
			buffConfig.FunctionType3 = (BuffFunctionType)unitDataTable.GetData(i, 10, 0, false);
			buffConfig.X3 = unitDataTable.GetData(i, 11, 0, false);
			buffConfig.Y3 = unitDataTable.GetData(i, 12, 0, false);
			buffConfig.IconName = unitDataTable.GetData(i, 13, string.Empty, false);
			buffConfig.ResourceName = unitDataTable.GetData(i, 14, string.Empty, false);
			buffConfig.Description1 = unitDataTable.GetData(i, 15, string.Empty, false);
			buffConfig.Description2 = unitDataTable.GetData(i, 16, string.Empty, false);
			buffConfig.CurrentDescribValue = null;
			buffConfig.NextDescribValue = null;
			this.buffConfig.Add(buffConfig.ID, buffConfig);
		}
	}

	public void LoadFromXML(string path)
	{
		globalConf = new GlobalConfig();
		playerConf = new PlayerConfig();
		XmlReader xmlReader = null;
		StringReader stringReader = null;
		Stream stream = null;
		if (path != null)
		{
			path = Application.dataPath + path;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			stream = File.Open(path + "config.xml", FileMode.Open);
			xmlReader = XmlReader.Create(stream);
		}
		else
		{
			TextAsset textAsset = Resources.Load("Config/config") as TextAsset;
			stringReader = new StringReader(textAsset.text);
			xmlReader = XmlReader.Create(stringReader);
		}
		WeaponConfig weaponConfig = null;
		AvatarConfig avatarConfig = null;
		while (xmlReader.Read())
		{
			switch (xmlReader.NodeType)
			{
			}
		}
		if (xmlReader != null)
		{
			xmlReader.Close();
		}
		if (stringReader != null)
		{
			stringReader.Close();
		}
		if (stream != null)
		{
			stream.Close();
		}
	}
}

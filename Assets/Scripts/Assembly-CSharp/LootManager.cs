using System;
using System.Collections.Generic;
using UnityEngine;

public class LootManager
{
	public Dictionary<int, ItemClasses> mDropColumnName = new Dictionary<int, ItemClasses>();

	public Dictionary<ItemClasses, byte> mClassToTableName = new Dictionary<ItemClasses, byte>();

	private static float[,] EnemyDropQualityRate = new float[4, 5]
	{
		{ 0f, 10f, 3f, 1f, 0f },
		{ 0f, 11f, 4f, 1f, 0f },
		{ 0f, 12f, 5f, 1f, 0f },
		{ 0f, 13f, 6f, 1f, 0f }
	};

	private static float[,] EliteDropQualityRate = new float[4, 5]
	{
		{ 0f, 40f, 8f, 2f, 0f },
		{ 0f, 50f, 8f, 2f, 0f },
		{ 0f, 60f, 9f, 3f, 0f },
		{ 0f, 70f, 10f, 4f, 0f }
	};

	private static float[,] BossDropQualityRate = new float[4, 5]
	{
		{ 0f, 70f, 15.5f, 5f, 0.5f },
		{ 0f, 70f, 15.5f, 5f, 0.5f },
		{ 0f, 70f, 16.5f, 6f, 0.5f },
		{ 0f, 70f, 17.5f, 7f, 0.5f }
	};

	private static float[,] RareDropQualityRate = new float[4, 5]
	{
		{ 0f, 50f, 18f, 5f, 0f },
		{ 0f, 50f, 18f, 5f, 0f },
		{ 0f, 50f, 19f, 6f, 0f },
		{ 0f, 50f, 20f, 7f, 0f }
	};

	private static float[,] EnemyChipQualityRate = new float[4, 5]
	{
		{ 0f, 10f, 0f, 0f, 0f },
		{ 0f, 10f, 0f, 0f, 0f },
		{ 0f, 10f, 0f, 0f, 0f },
		{ 0f, 10f, 0f, 0f, 0f }
	};

	private static float[,] EliteChipQualityRate = new float[4, 5]
	{
		{ 0f, 40f, 0f, 0f, 0f },
		{ 0f, 45f, 0f, 0f, 0f },
		{ 0f, 50f, 0f, 0f, 0f },
		{ 0f, 55f, 0f, 0f, 0f }
	};

	private static float[,] BossChipQualityRate = new float[4, 5]
	{
		{ 0f, 30f, 1f, 0f, 0f },
		{ 0f, 35f, 1f, 0f, 0f },
		{ 0f, 40f, 1f, 0f, 0f },
		{ 0f, 45f, 2f, 0f, 0f }
	};

	private static float[,] RareChipQualityRate = new float[4, 5]
	{
		{ 0f, 50f, 1f, 0f, 0f },
		{ 0f, 60f, 1f, 0f, 0f },
		{ 0f, 70f, 1f, 0f, 0f },
		{ 0f, 80f, 2f, 0f, 0f }
	};

	public void Init()
	{
		InitEnemyDropColumnName();
		InitClassToTableName();
	}

	protected void InitEnemyDropColumnName()
	{
		mDropColumnName.Add(2, ItemClasses.SubmachineGun);
		mDropColumnName.Add(3, ItemClasses.AssultRifle);
		mDropColumnName.Add(4, ItemClasses.Pistol);
		mDropColumnName.Add(5, ItemClasses.Revolver);
		mDropColumnName.Add(6, ItemClasses.Shotgun);
		mDropColumnName.Add(7, ItemClasses.Sniper);
		mDropColumnName.Add(8, ItemClasses.RPG);
		mDropColumnName.Add(9, ItemClasses.Grenade);
		mDropColumnName.Add(10, ItemClasses.U_Shield);
		mDropColumnName.Add(11, ItemClasses.V_Slot);
		mDropColumnName.Add(12, ItemClasses.FirstAid);
		mDropColumnName.Add(13, ItemClasses.X_Money);
		mDropColumnName.Add(14, ItemClasses.Z_BackPack);
	}

	protected void InitClassToTableName()
	{
		mClassToTableName.Add(ItemClasses.SubmachineGun, 9);
		mClassToTableName.Add(ItemClasses.AssultRifle, 10);
		mClassToTableName.Add(ItemClasses.Pistol, 11);
		mClassToTableName.Add(ItemClasses.Revolver, 12);
		mClassToTableName.Add(ItemClasses.Shotgun, 13);
		mClassToTableName.Add(ItemClasses.Sniper, 14);
		mClassToTableName.Add(ItemClasses.RPG, 15);
		mClassToTableName.Add(ItemClasses.Grenade, 16);
		mClassToTableName.Add(ItemClasses.U_Shield, 17);
		mClassToTableName.Add(ItemClasses.V_Slot, 18);
		mClassToTableName.Add(ItemClasses.W_Pills, 19);
		mClassToTableName.Add(ItemClasses.Z_BackPack, 21);
	}

	public void OnLoot(Enemy _enemy)
	{
		if (_enemy == null)
		{
			return;
		}
		string callName = _enemy.GetCallName();
		byte level = _enemy.Level;
		Vector3 dropPosition = new Vector3(_enemy.GetTransform().position.x, _enemy.GetFloorHeight() + 0.5f, _enemy.GetTransform().position.z);
		if (!GameConfig.GetInstance().enemyDropConfig.ContainsKey((short)_enemy.UniqueID))
		{
			Debug.Log("ENEMY ID Not Found!");
			return;
		}
		DropConfig dropConfig = GameConfig.GetInstance().enemyDropConfig[(short)_enemy.UniqueID];
		if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
		{
			ItemClasses itemClass = ItemClasses.X_Money;
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				itemClass = ItemClasses.Bullet;
			}
			PreSpawnItem(_enemy, _enemy.Level, itemClass, ItemQuality.Common, dropPosition);
		}
		else
		{
			for (int i = 2; i < 24; i++)
			{
				if (i == 15 || i == 17 || i == 19 || i == 21 || i == 23)
				{
					continue;
				}
				int num = 0;
				int num2 = UnityEngine.Random.Range(0, 100);
				short specialID = 0;
				switch (i)
				{
				case 2:
					num = dropConfig.SMG_Rate;
					break;
				case 3:
					num = dropConfig.AssualtRifle_Rate;
					break;
				case 4:
					num = dropConfig.Pistol_Rate;
					break;
				case 5:
					num = dropConfig.Revolver_Rate;
					break;
				case 6:
					num = dropConfig.Shotgun_Rate;
					break;
				case 7:
					num = dropConfig.Sniper_Rate;
					break;
				case 8:
					num = dropConfig.RPG_Rate;
					if (level < 9)
					{
						num = 0;
					}
					break;
				case 9:
					num = dropConfig.Grenade_Rate;
					break;
				case 10:
					num = dropConfig.Shield_Rate;
					break;
				case 11:
					num = dropConfig.Slot_Rate;
					break;
				case 12:
					num = dropConfig.FirstAid_Rate;
					break;
				case 13:
					num = 100;
					break;
				case 14:
					num = dropConfig.BackPack_Rate;
					break;
				case 16:
					specialID = dropConfig.SP1_ID;
					num = dropConfig.SP1_Rate;
					break;
				case 18:
					specialID = dropConfig.SP2_ID;
					num = dropConfig.SP2_Rate;
					break;
				case 20:
					specialID = dropConfig.SP3_ID;
					num = dropConfig.SP3_Rate;
					break;
				case 22:
					specialID = dropConfig.SP4_ID;
					num = dropConfig.SP4_Rate;
					break;
				}
				num = (int)((float)num * GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.DropRate);
				if (num2 >= num)
				{
					continue;
				}
				bool isChip = false;
				if (i == 11)
				{
					isChip = true;
				}
				ItemQuality itemQuality = RandomQualtiy(_enemy, isChip);
				byte level2 = _enemy.Level;
				int num3 = 0;
				int num4 = UnityEngine.Random.Range(0, 100);
				switch (itemQuality)
				{
				case ItemQuality.Common:
					num3 = 0;
					break;
				case ItemQuality.Uncommon:
					num3 = ((num4 >= 20) ? 1 : 0);
					break;
				case ItemQuality.Rare:
					num3 = ((num4 >= 20) ? 1 : 0);
					break;
				case ItemQuality.Epic:
					num3 = ((num4 < 20) ? 1 : 2);
					break;
				case ItemQuality.Legendary:
					num3 = _enemy.Level - 1;
					break;
				}
				if (num3 >= _enemy.Level)
				{
					num3 = _enemy.Level - 1;
				}
				level2 = (byte)(_enemy.Level - num3);
				switch (i)
				{
				case 16:
				case 18:
				case 20:
				case 22:
					PreSpawnSpecialItem(_enemy, specialID, dropPosition, level2);
					continue;
				case 23:
					continue;
				}
				ItemClasses itemClasses = mDropColumnName[i];
				if (itemClasses == ItemClasses.X_Money && UnityEngine.Random.Range(0, 2) == 0)
				{
					itemClasses = ItemClasses.Bullet;
				}
				PreSpawnItem(_enemy, level2, itemClasses, itemQuality, dropPosition);
			}
		}
		SpawnItem(_enemy);
		_enemy.GetPreSpawnBuffer().Clear();
	}

	private ItemQuality RandomQualtiy(Enemy _enemy, bool isChip)
	{
		ItemQuality result = ItemQuality.Common;
		float num = UnityEngine.Random.Range(0f, 100f);
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		int num6 = 0;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			num6 = GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
				.Count;
		}
		DropConfig dropConfig = GameConfig.GetInstance().enemyDropConfig[(short)_enemy.UniqueID];
		if (dropConfig.DropType == 3)
		{
			if (isChip)
			{
				num2 = RareChipQualityRate[num6, 4];
				num3 = RareChipQualityRate[num6, 3];
				num4 = RareChipQualityRate[num6, 2];
				num5 = RareChipQualityRate[num6, 1];
			}
			else
			{
				num2 = RareDropQualityRate[num6, 4];
				num3 = RareDropQualityRate[num6, 3];
				num4 = RareDropQualityRate[num6, 2];
				num5 = RareDropQualityRate[num6, 1];
			}
		}
		else if (dropConfig.DropType == 2)
		{
			if (isChip)
			{
				num2 = BossChipQualityRate[num6, 4];
				num3 = BossChipQualityRate[num6, 3];
				num4 = BossChipQualityRate[num6, 2];
				num5 = BossChipQualityRate[num6, 1];
			}
			else
			{
				num2 = BossDropQualityRate[num6, 4];
				num3 = BossDropQualityRate[num6, 3];
				num4 = BossDropQualityRate[num6, 2];
				num5 = BossDropQualityRate[num6, 1];
			}
		}
		else if (dropConfig.DropType == 1)
		{
			if (isChip)
			{
				num2 = EliteChipQualityRate[num6, 4];
				num3 = EliteChipQualityRate[num6, 3];
				num4 = EliteChipQualityRate[num6, 2];
				num5 = EliteChipQualityRate[num6, 1];
			}
			else
			{
				num2 = EliteDropQualityRate[num6, 4];
				num3 = EliteDropQualityRate[num6, 3];
				num4 = EliteDropQualityRate[num6, 2];
				num5 = EliteDropQualityRate[num6, 1];
			}
		}
		else if (isChip)
		{
			num2 = EnemyChipQualityRate[num6, 4];
			num3 = EnemyChipQualityRate[num6, 3];
			num4 = EnemyChipQualityRate[num6, 2];
			num5 = EnemyChipQualityRate[num6, 1];
		}
		else
		{
			num2 = EnemyDropQualityRate[num6, 4];
			num3 = EnemyDropQualityRate[num6, 3];
			num4 = EnemyDropQualityRate[num6, 2];
			num5 = EnemyDropQualityRate[num6, 1];
		}
		if (num < num2)
		{
			result = ItemQuality.Legendary;
		}
		else if (num < num3)
		{
			result = ItemQuality.Epic;
		}
		else if (num < num4)
		{
			result = ItemQuality.Rare;
		}
		else if (num < num5)
		{
			result = ItemQuality.Uncommon;
		}
		return result;
	}

	private byte GenerateItemLevel(byte enemyLevel, float para1, float para2)
	{
		return Convert.ToByte(enemyLevel / 2);
	}

	private void PreSpawnItem(Enemy enemy, byte level, ItemClasses itemClass, ItemQuality quality, Vector3 dropPosition)
	{
		if (enemy != null)
		{
			PreSpawnBuffer item = CreatePreSpawnItem(itemClass, level, quality, dropPosition);
			enemy.GetPreSpawnBuffer().Add(item);
		}
	}

	public PreSpawnBuffer CreatePreSpawnItem(ItemClasses itemClass, byte level, ItemQuality quality, Vector3 dropPosition)
	{
		short specialID = 0;
		int number = 1;
		PreSpawnBuffer result = default(PreSpawnBuffer);
		byte itemType = 0;
		string itemName = string.Empty;
		GetRandomNameNumberTypeByItemClass(itemClass, ref itemName, ref number, ref itemType);
		result.itemName = itemName;
		result.itemType = itemType;
		result.itemLevel = level;
		result.itemClass = itemClass;
		result.itemQuality = quality;
		result.dropPosition = dropPosition;
		result.isSpecialItem = false;
		result.specialID = specialID;
		return result;
	}

	private void FilterPreSpawnItem(Enemy enemy, int whiteLimit, int greenLimit, int blueLimit, int purpleLimit, int orangeLimit, int totalLimit)
	{
	}

	private void PreSpawnSpecialItem(Enemy enemy, short specialID, Vector3 dropPosition, byte level)
	{
		if (enemy != null)
		{
			SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[specialID];
			PreSpawnBuffer item = default(PreSpawnBuffer);
			item.itemType = (byte)specialItemConfig.ItemType;
			item.itemClass = specialItemConfig.ItemClass;
			bool isChip = false;
			if (specialItemConfig.ItemClass == ItemClasses.V_Slot)
			{
				isChip = true;
			}
			item.itemQuality = RandomQualtiy(enemy, isChip);
			item.dropPosition = dropPosition;
			item.isSpecialItem = true;
			item.specialID = specialID;
			item.prefabName = "Item/Special/" + specialItemConfig.PrefabName;
			item.itemNumber = specialItemConfig.ItemNumber;
			item.itemLevel = level;
			enemy.GetPreSpawnBuffer().Add(item);
		}
	}

	private void SpawnItem(Enemy enemy)
	{
		if (enemy == null)
		{
			return;
		}
		for (int i = 0; i < enemy.GetPreSpawnBuffer().Count; i++)
		{
			GameObject gameObject = null;
			GameObject gameObject2 = null;
			byte itemLevel = enemy.GetPreSpawnBuffer()[i].itemLevel;
			byte itemType = enemy.GetPreSpawnBuffer()[i].itemType;
			ItemClasses itemClass = enemy.GetPreSpawnBuffer()[i].itemClass;
			string itemStyle = enemy.GetPreSpawnBuffer()[i].itemStyle;
			ItemQuality quality = enemy.GetPreSpawnBuffer()[i].itemQuality;
			Vector3 dropPosition = enemy.GetPreSpawnBuffer()[i].dropPosition;
			float x = UnityEngine.Random.Range(-1.5f, 1.5f);
			float y = UnityEngine.Random.Range(1.5f, 2.5f);
			float z = UnityEngine.Random.Range(-1.5f, 1.5f);
			bool flag = true;
			if (!enemy.GetPreSpawnBuffer()[i].isSpecialItem)
			{
				string itemName = enemy.GetPreSpawnBuffer()[i].itemName;
				string text = GeneratePrefabName(itemType, itemName);
				gameObject = Resources.Load(text) as GameObject;
				if (gameObject == null)
				{
					Debug.Log("itemPrefab not found! ------ " + text);
					continue;
				}
				float yAngle = UnityEngine.Random.Range(0f, 360f);
				gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
				gameObject2.transform.position = dropPosition;
				gameObject2.transform.Rotate(0f, yAngle, 0f, Space.World);
				gameObject2.GetComponent<ItemBase>().ItemName = itemName;
				if (itemType == 4 || itemType == 9 || itemType == 5 || itemType == 6 || itemType == 8)
				{
					quality = ItemQuality.Common;
				}
			}
			else
			{
				string prefabName = enemy.GetPreSpawnBuffer()[i].prefabName;
				if (enemy.GetPreSpawnBuffer()[i].itemType == 7)
				{
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						if (!GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailableForNet(enemy.GetPreSpawnBuffer()[i].specialID))
						{
							continue;
						}
					}
					else if (!GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailable(enemy.GetPreSpawnBuffer()[i].specialID))
					{
						Debug.Log("SpecialID = " + enemy.GetPreSpawnBuffer()[i].specialID + " No Request");
						continue;
					}
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						if (enemy.IsMasterPlayer)
						{
							ItemSpawnRequest request = new ItemSpawnRequest(enemy.GetPreSpawnBuffer()[i].specialID, dropPosition, new Vector3(x, y, z));
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
						break;
					}
				}
				gameObject = Resources.Load(prefabName) as GameObject;
				if (gameObject == null)
				{
					Debug.Log("itemPrefab not found!");
					continue;
				}
				gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
				gameObject2.transform.position = dropPosition;
				if (enemy.GetPreSpawnBuffer()[i].itemType != 7)
				{
					SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[enemy.GetPreSpawnBuffer()[i].specialID];
					bool isChip = false;
					if (enemy.GetPreSpawnBuffer()[i].itemClass == ItemClasses.V_Slot)
					{
						isChip = true;
					}
					CreateItemBaseFromGameObject(gameObject2, itemLevel, specialItemConfig.ItemClass, enemy.GetPreSpawnBuffer()[i].itemNumber, RandomQualtiy(enemy, isChip), enemy.GetPreSpawnBuffer()[i].specialID);
					flag = false;
				}
				else
				{
					quality = ItemQuality.Common;
				}
			}
			if (itemType != 5)
			{
				gameObject2.GetComponent<Rigidbody>().detectCollisions = true;
				gameObject2.GetComponent<Rigidbody>().AddForce(x, y, z, ForceMode.Impulse);
			}
			else
			{
				gameObject2.transform.position = new Vector3(enemy.GetTransform().position.x, enemy.GetFloorHeight(), enemy.GetTransform().position.z);
				AddPromptLine(gameObject2.transform, "Prompt_Arrow_Yellow", "Prompt_Plane_Yellow");
			}
			if (flag)
			{
				byte itemNumber = 1;
				if (itemType != 8 && itemType != 7 && itemType != 9)
				{
					string value = enemy.GetPreSpawnBuffer()[i].itemName[enemy.GetPreSpawnBuffer()[i].itemName.Length - 2].ToString() + enemy.GetPreSpawnBuffer()[i].itemName[enemy.GetPreSpawnBuffer()[i].itemName.Length - 1];
					itemNumber = Convert.ToByte(value);
				}
				CreateItemBaseFromGameObject(gameObject2, itemLevel, itemClass, itemNumber, quality, enemy.GetPreSpawnBuffer()[i].specialID);
				if (gameObject2.GetComponent<ItemMoney>() != null)
				{
					int gold = enemy.GetGold();
					int num = UnityEngine.Random.Range(-10, 11);
					int num2 = gold * num / 100;
					if (num2 == 0)
					{
						if (num > 0)
						{
							num2 = 1;
						}
						else if (num < 0)
						{
							num2 = -1;
						}
					}
					gameObject2.GetComponent<ItemMoney>().Money = gold + num2;
				}
			}
			if (GameApp.GetInstance().GetGameScene() != null)
			{
				GameObject itemRoot = GameApp.GetInstance().GetGameScene().GetItemRoot();
				if (null != itemRoot)
				{
					gameObject2.transform.parent = itemRoot.transform;
				}
			}
		}
	}

	public string GeneratePrefabName(byte itemType, string itemName)
	{
		string text = "Item/";
		switch (itemType)
		{
		case 6:
			return text + "BackPack/" + itemName;
		case 5:
			return text + "Money/" + itemName;
		case 4:
		case 9:
			return text + "Pills/" + itemName;
		case 2:
			return text + "Shield/" + itemName;
		case 3:
			return text + "Slot/" + itemName;
		case 7:
			return text + "Story/" + itemName;
		case 0:
		case 1:
			return text + "Weapon/" + itemName;
		case 8:
			return text + "Bullet/" + itemName;
		default:
			return string.Empty;
		}
	}

	public void CheckIfRateStringIsEmpty(ref string _string)
	{
		if (string.IsNullOrEmpty(_string))
		{
			_string = "0";
		}
	}

	public void GenerateQuestRewardItemAndPickUp(short Id, byte level)
	{
		Debug.Log("reward id: " + Id);
		SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[Id];
		int num = UnityEngine.Random.Range(1, specialItemConfig.ItemNumber + 1);
		ItemBase itemBase = CreateItemBase(level, specialItemConfig.ItemClass, (byte)num, specialItemConfig.Quality, Id);
		itemBase.PickUpItem(false);
		UnityEngine.Object.Destroy(itemBase);
	}

	public void GenerateQuestRewardItemAndPickUp(short Id, byte level, byte number)
	{
		Debug.Log("reward id: " + Id);
		SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[Id];
		ItemBase itemBase = CreateItemBase(level, specialItemConfig.ItemClass, number, specialItemConfig.Quality, Id);
		if (itemBase.ItemCanBePickedUp())
		{
			itemBase.PickUpItem(false);
		}
		else
		{
			GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(itemBase.mNGUIBaseItem, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition() + Vector3.up, Vector3.zero);
			if (UIQuest.m_instance != null && UIQuest.m_instance.m_quests.activeSelf)
			{
				UIMsgBox.instance.ShowMessage(UIQuest.m_instance.m_quests.GetComponent<UIQuestEntities>(), LocalizationManager.GetInstance().GetString("MSG_BAG_FULL"), 2, 30);
			}
		}
		UnityEngine.Object.Destroy(itemBase);
	}

	public void SpawnQuestItem(Vector3 position, Vector3 force, short specialID, short sequenceID)
	{
		SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[specialID];
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		string path = "Item/Special/" + specialItemConfig.PrefabName;
		gameObject = Resources.Load(path) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("itemPrefab not found!");
			return;
		}
		gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		gameObject2.transform.position = position;
		gameObject2.name = "QuestItem_" + sequenceID;
		gameObject2.GetComponent<Rigidbody>().detectCollisions = true;
		gameObject2.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		CreateItemBaseFromGameObject(gameObject2, 1, specialItemConfig.ItemClass, 0, ItemQuality.Common, specialID);
	}

	public ItemBase CreateItemBase(byte level, ItemClasses itemClass, byte itemNumber, ItemQuality quality, short specialID)
	{
		ItemBase itemBase = null;
		GameObject obj = new GameObject();
		return CreateItemBaseFromGameObject(obj, level, itemClass, itemNumber, quality, specialID);
	}

	public ItemBase CreateItemBaseFromGameObject(GameObject obj, byte level, ItemClasses itemClass, byte itemNumber, ItemQuality quality, short specialID)
	{
		ItemBase component = obj.GetComponent<ItemBase>();
		byte itemType = 0;
		NGUIBaseItem.EquipmentSlot equipmentSlot = NGUIBaseItem.EquipmentSlot.None;
		if (component == null)
		{
			switch (itemClass)
			{
			case ItemClasses.SubmachineGun:
			case ItemClasses.AssultRifle:
			case ItemClasses.Pistol:
			case ItemClasses.Revolver:
			case ItemClasses.Shotgun:
			case ItemClasses.Sniper:
			case ItemClasses.RPG:
			case ItemClasses.Grenade:
				obj.AddComponent<ItemWeapon>();
				component = obj.GetComponent<ItemWeapon>();
				itemType = 0;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Weapon;
				if (itemClass == ItemClasses.Grenade)
				{
					itemType = 1;
					equipmentSlot = NGUIBaseItem.EquipmentSlot.WeaponG;
				}
				break;
			case ItemClasses.U_Shield:
				obj.AddComponent<ItemShield>();
				component = obj.GetComponent<ItemShield>();
				itemType = 2;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Shield;
				break;
			case ItemClasses.V_Slot:
				obj.AddComponent<ItemSlot>();
				component = obj.GetComponent<ItemSlot>();
				itemType = 3;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.SkillSlot;
				break;
			case ItemClasses.W_Pills:
				obj.AddComponent<ItemPill>();
				component = obj.GetComponent<ItemPill>();
				itemType = 4;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Pill;
				break;
			case ItemClasses.StoryItem:
				obj.AddComponent<ItemSpecial>();
				component = obj.GetComponent<ItemSpecial>();
				itemType = 7;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.None;
				break;
			case ItemClasses.FirstAid:
				obj.AddComponent<ItemFirstAid>();
				component = obj.GetComponent<ItemFirstAid>();
				itemType = 9;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.None;
				break;
			default:
				return null;
			}
			component.mNGUIBaseItem = new NGUIBaseItem();
			if (specialID != 0)
			{
			}
			component.ItemName = GetPrefabNameByItemClassAndNumber(itemClass, itemNumber);
			component.ItemLevel = level;
			component.ItemClass = itemClass;
			component.ItemType = itemType;
			component.Quality = quality;
			component.SpecialID = specialID;
			component.mNGUIBaseItem.equipmentSlot = equipmentSlot;
		}
		else
		{
			switch (itemClass)
			{
			case ItemClasses.SubmachineGun:
			case ItemClasses.AssultRifle:
			case ItemClasses.Pistol:
			case ItemClasses.Revolver:
			case ItemClasses.Shotgun:
			case ItemClasses.Sniper:
			case ItemClasses.RPG:
			case ItemClasses.Grenade:
				itemType = 0;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Weapon;
				if (itemClass == ItemClasses.Grenade)
				{
					itemType = 1;
					equipmentSlot = NGUIBaseItem.EquipmentSlot.WeaponG;
				}
				break;
			case ItemClasses.U_Shield:
				itemType = 2;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Shield;
				break;
			case ItemClasses.V_Slot:
				itemType = 3;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.SkillSlot;
				break;
			case ItemClasses.W_Pills:
				itemType = 4;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Pill;
				break;
			case ItemClasses.StoryItem:
				itemType = 7;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.None;
				break;
			case ItemClasses.FirstAid:
				itemType = 9;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.None;
				break;
			}
			component.ItemName = GetPrefabNameByItemClassAndNumber(itemClass, itemNumber);
			component.ItemType = itemType;
			component.ItemClass = itemClass;
			component.Quality = quality;
			component.ItemLevel = level;
			component.SpecialID = specialID;
		}
		component.mNGUIBaseItem.iconAtlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		component.generateItemProperties();
		component.generateLimitScore();
		component.generateEquipmentSkills();
		component.generateNGUIBaseItem();
		return component;
	}

	public static string GetPrefabNameByItemClassAndNumber(ItemClasses itemClass, int number)
	{
		string text = string.Empty;
		switch (itemClass)
		{
		case ItemClasses.SubmachineGun:
			text = "SMG";
			break;
		case ItemClasses.AssultRifle:
			text = "assault";
			break;
		case ItemClasses.Pistol:
			text = "pistol";
			break;
		case ItemClasses.Revolver:
			text = "revolver";
			break;
		case ItemClasses.Shotgun:
			text = "shotgun";
			break;
		case ItemClasses.Sniper:
			text = "sniper";
			break;
		case ItemClasses.RPG:
			text = "RPG";
			break;
		case ItemClasses.Grenade:
			text = "Grenade";
			break;
		case ItemClasses.U_Shield:
			text = "Shield";
			break;
		case ItemClasses.V_Slot:
			text = "Chip";
			number = 1;
			break;
		case ItemClasses.W_Pills:
			text = "Pill";
			number = 1;
			break;
		case ItemClasses.FirstAid:
			return "FirstAid";
		}
		string text2 = number.ToString();
		if (number < 10)
		{
			text2 = "0" + text2;
		}
		return text + text2;
	}

	public GameObject GetIconForSpecialItem(short itemID, byte nameNumber)
	{
		SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[itemID];
		if (specialItemConfig.ItemClass == ItemClasses.U_Shield || specialItemConfig.ItemClass == ItemClasses.V_Slot)
		{
			nameNumber = 1;
		}
		return CreateIcon(specialItemConfig.Quality, specialItemConfig.IconName + nameNumber, null);
	}

	public GameObject CreateIcon(ItemBase itemBase)
	{
		return CreateIcon(itemBase.Quality, itemBase.mNGUIBaseItem.iconName, itemBase);
	}

	public GameObject CreateIcon(ItemQuality quality, string iconName)
	{
		return CreateIcon(quality, iconName, null);
	}

	public GameObject CreateIcon(ItemQuality quality, string iconName, ItemBase itemBase)
	{
		GameObject original = ResourceLoad.GetInstance().LoadUI("HUD", "ItemIcon");
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		ItemIcon component = gameObject.GetComponent<ItemIcon>();
		component.SetItemIcon(iconName);
		if (itemBase != null)
		{
			component.SetItemInfo(itemBase.mNGUIBaseItem);
			component.background.spriteName = itemBase.mNGUIBaseItem.GetBackGroundColorStringByQuality();
		}
		else
		{
			component.SetItemInfo(null);
			switch (quality)
			{
			case ItemQuality.Uncommon:
				component.background.spriteName = "LVB";
				break;
			case ItemQuality.Rare:
				component.background.spriteName = "LVC";
				break;
			case ItemQuality.Epic:
				component.background.spriteName = "LVD";
				break;
			case ItemQuality.Legendary:
				component.background.spriteName = "LVE";
				break;
			default:
				component.background.spriteName = "LVA";
				break;
			}
		}
		component.gameObject.layer = LayerMask.NameToLayer("UI");
		return component.gameObject;
	}

	public GameObject CreateUnknownIcon()
	{
		GameObject original = ResourceLoad.GetInstance().LoadUI("HUD", "ItemIcon");
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		ItemIcon component = gameObject.GetComponent<ItemIcon>();
		component.SetItemIcon("besticon");
		component.background.spriteName = "besticon";
		component.gameObject.layer = LayerMask.NameToLayer("UI");
		return component.gameObject;
	}

	public void SpawnItemByNGUIBaseItem(NGUIBaseItem baseItem, Vector3 dropPosition, Vector3 dropVelocity)
	{
		byte itemLevel = baseItem.ItemLevel;
		string name = baseItem.name;
		byte itemType = 0;
		switch (baseItem.equipmentSlot)
		{
		case NGUIBaseItem.EquipmentSlot.Weapon:
			itemType = 0;
			break;
		case NGUIBaseItem.EquipmentSlot.WeaponG:
			itemType = 1;
			break;
		case NGUIBaseItem.EquipmentSlot.Shield:
			itemType = 2;
			break;
		case NGUIBaseItem.EquipmentSlot.SkillSlot:
			itemType = 3;
			break;
		case NGUIBaseItem.EquipmentSlot.Pill:
			itemType = 4;
			break;
		}
		string text = GeneratePrefabName(itemType, name);
		GameObject gameObject = Resources.Load(text) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("itemPrefab not found! ----" + text);
			return;
		}
		float yAngle = UnityEngine.Random.Range(0f, 360f);
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		gameObject2.transform.position = dropPosition;
		gameObject2.transform.Rotate(0f, yAngle, 0f, Space.World);
		gameObject2.GetComponent<Rigidbody>().detectCollisions = true;
		gameObject2.GetComponent<Rigidbody>().AddForce(dropVelocity, ForceMode.Impulse);
		gameObject2.GetComponent<ItemBase>().ItemName = name;
		gameObject2.GetComponent<ItemBase>().ItemType = itemType;
		gameObject2.GetComponent<ItemBase>().ItemLevel = itemLevel;
		gameObject2.GetComponent<ItemBase>().SpecialID = 0;
		gameObject2.GetComponent<ItemBase>().Quality = baseItem.Quality;
		gameObject2.GetComponent<ItemBase>().mNGUIBaseItem = baseItem;
	}

	public void CheckAndSpawnAreanaReward()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null && userState.ArenaRewards != null && userState.ArenaRewards.item != null)
		{
			SpawnItemByNGUIBaseItem(userState.ArenaRewards.item, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition(), Vector3.zero);
			userState.ArenaRewards.item = null;
		}
	}

	public void GetRandomNameNumberTypeByItemClass(ItemClasses itemClass, ref string itemName, ref int number, ref byte itemType)
	{
		switch (itemClass)
		{
		case ItemClasses.SubmachineGun:
			number = UnityEngine.Random.Range(1, 4);
			itemName = "SMG0" + number;
			itemType = 0;
			break;
		case ItemClasses.AssultRifle:
			number = UnityEngine.Random.Range(1, 5);
			itemName = "assault0" + number;
			itemType = 0;
			break;
		case ItemClasses.Pistol:
			number = UnityEngine.Random.Range(1, 3);
			itemName = "pistol0" + number;
			itemType = 0;
			break;
		case ItemClasses.Revolver:
			number = UnityEngine.Random.Range(1, 3);
			itemName = "revolver0" + number;
			itemType = 0;
			break;
		case ItemClasses.Shotgun:
			number = UnityEngine.Random.Range(1, 4);
			itemName = "shotgun0" + number;
			itemType = 0;
			break;
		case ItemClasses.Sniper:
			number = UnityEngine.Random.Range(1, 4);
			itemName = "sniper0" + number;
			itemType = 0;
			break;
		case ItemClasses.RPG:
			number = UnityEngine.Random.Range(1, 3);
			itemName = "RPG0" + number;
			itemType = 0;
			break;
		case ItemClasses.Grenade:
			number = UnityEngine.Random.Range(1, 5);
			itemName = "Grenade0" + number;
			itemType = 1;
			break;
		case ItemClasses.U_Shield:
			number = UnityEngine.Random.Range(1, 6);
			itemName = "Shield0" + number;
			itemType = 2;
			break;
		case ItemClasses.V_Slot:
			number = 1;
			itemName = "Chip01";
			itemType = 3;
			break;
		case ItemClasses.W_Pills:
			itemName = "Pill01";
			itemType = 4;
			break;
		case ItemClasses.X_Money:
			itemName = "Money01";
			itemType = 5;
			break;
		case ItemClasses.Z_BackPack:
			itemType = 6;
			break;
		case ItemClasses.Bullet:
		{
			int num = 0;
			WeaponType weaponType = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetWeapon()
				.GetWeaponType();
			if (GameApp.GetInstance().GetUserState().GetBulletByWeaponType(weaponType) < GameApp.GetInstance().GetUserState().GetMaxBulletByWeaponType(weaponType))
			{
				num = 20;
			}
			int num2 = UnityEngine.Random.Range(0, 100);
			WeaponType weaponType2 = ((num2 >= num) ? ((WeaponType)UnityEngine.Random.Range(1, 9)) : weaponType);
			itemName = "Bullet_" + weaponType2;
			itemType = 8;
			break;
		}
		case ItemClasses.FirstAid:
			itemName = "FirstAid";
			itemType = 9;
			break;
		case ItemClasses.StoryItem:
			break;
		}
	}

	public void GetRandomNameNumberTypeByItemClassFromAll(ItemClasses itemClass, ref string itemName, ref int number, ref byte itemType)
	{
		int num = 0;
		if (itemClass <= ItemClasses.U_Shield)
		{
			num = Global.EQUIP_TYPE_COUNT[(int)itemClass];
			number = UnityEngine.Random.Range(1, num + 1);
		}
		switch (itemClass)
		{
		case ItemClasses.SubmachineGun:
			itemName = "SMG0" + number;
			itemType = 0;
			break;
		case ItemClasses.AssultRifle:
			itemName = "assault0" + number;
			itemType = 0;
			break;
		case ItemClasses.Pistol:
			itemName = "pistol0" + number;
			itemType = 0;
			break;
		case ItemClasses.Revolver:
			itemName = "revolver0" + number;
			itemType = 0;
			break;
		case ItemClasses.Shotgun:
			itemName = "shotgun0" + number;
			itemType = 0;
			break;
		case ItemClasses.Sniper:
			itemName = "sniper0" + number;
			itemType = 0;
			break;
		case ItemClasses.RPG:
			itemName = "RPG0" + number;
			itemType = 0;
			break;
		case ItemClasses.Grenade:
			itemName = "Grenade0" + number;
			itemType = 1;
			break;
		case ItemClasses.U_Shield:
			itemName = "Shield0" + number;
			itemType = 2;
			break;
		case ItemClasses.V_Slot:
			number = 1;
			itemName = "Chip01";
			itemType = 3;
			break;
		case ItemClasses.W_Pills:
			itemName = "Pill01";
			itemType = 4;
			break;
		case ItemClasses.X_Money:
			itemName = "Money01";
			itemType = 5;
			break;
		case ItemClasses.Z_BackPack:
			itemType = 6;
			break;
		case ItemClasses.Bullet:
		{
			int num2 = 0;
			WeaponType weaponType = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetWeapon()
				.GetWeaponType();
			if (GameApp.GetInstance().GetUserState().GetBulletByWeaponType(weaponType) < GameApp.GetInstance().GetUserState().GetMaxBulletByWeaponType(weaponType))
			{
				num2 = 20;
			}
			int num3 = UnityEngine.Random.Range(0, 100);
			WeaponType weaponType2 = ((num3 >= num2) ? ((WeaponType)UnityEngine.Random.Range(1, 9)) : weaponType);
			itemName = "Bullet_" + weaponType2;
			itemType = 8;
			break;
		}
		case ItemClasses.FirstAid:
			itemName = "FirstAid";
			itemType = 9;
			break;
		case ItemClasses.StoryItem:
			break;
		}
	}

	public void AddPromptLine(Transform transform, ItemQuality quality, ItemClasses itemClass)
	{
		string resourceName = "Prompt_Arrow_White";
		string panelName = "Prompt_Plane_Orange";
		switch (quality)
		{
		case ItemQuality.Common:
			resourceName = "Prompt_Arrow_White";
			panelName = "Prompt_Plane_White";
			break;
		case ItemQuality.Uncommon:
			resourceName = "Prompt_Arrow_Green";
			panelName = "Prompt_Plane_Green";
			break;
		case ItemQuality.Rare:
			resourceName = "Prompt_Arrow_Blue";
			panelName = "Prompt_Plane_Blue";
			break;
		case ItemQuality.Epic:
			resourceName = "Prompt_Arrow_Pink";
			panelName = "Prompt_Plane_Pink";
			break;
		case ItemQuality.Legendary:
			resourceName = "Prompt_Arrow_Orange";
			panelName = "Prompt_Plane_Orange";
			break;
		}
		if (itemClass == ItemClasses.StoryItem)
		{
			resourceName = "line_quest_001";
			panelName = string.Empty;
		}
		AddPromptLine(transform, resourceName, panelName);
	}

	public void AddPromptLine(Transform transform, string resourceName, string panelName)
	{
		if (transform.Find("Prompt") != null)
		{
			return;
		}
		GameObject original = Resources.Load("Item/Prompt/" + resourceName) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = transform;
		gameObject.transform.rotation = Quaternion.identity;
		gameObject.transform.position = transform.position - Vector3.up * 0.8f;
		gameObject.transform.localScale = Vector3.one;
		gameObject.name = "Prompt";
		PositionTweenScript component = gameObject.GetComponent<PositionTweenScript>();
		if (component != null)
		{
			component.SetTweenPos(gameObject.transform.position, gameObject.transform.position + Vector3.up * 1f);
		}
		if (!string.IsNullOrEmpty(panelName))
		{
			GameObject original2 = Resources.Load("Item/Prompt/" + panelName) as GameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(original2) as GameObject;
			gameObject2.transform.parent = transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.one;
			RaycastHit hitInfo = default(RaycastHit);
			Ray ray = new Ray(transform.position + Vector3.up * 2f, Vector3.down);
			if (Physics.Raycast(ray, out hitInfo, 10f, 1 << PhysicsLayer.FLOOR))
			{
				gameObject2.transform.position = hitInfo.point;
				gameObject2.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
			}
		}
	}

	public NGUIBaseItem UpgradeItem(NGUIBaseItem baseItem)
	{
		if (baseItem == null)
		{
			return null;
		}
		byte upgradeTimes = baseItem.UpgradeTimes;
		if (upgradeTimes >= baseItem.GetMaxUpgradeCount())
		{
			Debug.Log("Fully Upgraded!");
			return null;
		}
		byte itemLevel = baseItem.ItemLevel;
		if (itemLevel >= GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			Debug.Log("Can't upgrade to level higher than you.");
			return null;
		}
		if (itemLevel >= Global.MAX_CHAR_LEVEL)
		{
			Debug.Log("Already Max Level!");
			return null;
		}
		List<short> skillIDs = baseItem.skillIDs;
		ItemBase itemBase = null;
		GameObject obj = new GameObject();
		itemBase = UpgradeItemBase(obj, (byte)(itemLevel + 1), baseItem.ItemClass, baseItem.name, baseItem.Quality, 0);
		if (itemBase != null)
		{
			itemBase.generateItemProperties();
			foreach (short item in skillIDs)
			{
				itemBase.AddSkillInfo(item);
			}
			itemBase.generateNGUIBaseItem();
			baseItem = itemBase.mNGUIBaseItem;
			baseItem.UpgradeTimes = (byte)(upgradeTimes + 1);
			UnityEngine.Object.Destroy(obj);
			return baseItem;
		}
		return null;
	}

	public ItemBase UpgradeItemBase(GameObject obj, byte level, ItemClasses itemClass, string itemName, ItemQuality quality, short specialID)
	{
		ItemBase component = obj.GetComponent<ItemBase>();
		byte b = 0;
		NGUIBaseItem.EquipmentSlot equipmentSlot = NGUIBaseItem.EquipmentSlot.None;
		if (component == null)
		{
			switch (itemClass)
			{
			case ItemClasses.SubmachineGun:
			case ItemClasses.AssultRifle:
			case ItemClasses.Pistol:
			case ItemClasses.Revolver:
			case ItemClasses.Shotgun:
			case ItemClasses.Sniper:
			case ItemClasses.RPG:
			case ItemClasses.Grenade:
				obj.AddComponent<ItemWeapon>();
				component = obj.GetComponent<ItemWeapon>();
				b = 0;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Weapon;
				if (itemClass == ItemClasses.Grenade)
				{
					b = 1;
					equipmentSlot = NGUIBaseItem.EquipmentSlot.WeaponG;
				}
				break;
			case ItemClasses.U_Shield:
				obj.AddComponent<ItemShield>();
				component = obj.GetComponent<ItemShield>();
				b = 2;
				equipmentSlot = NGUIBaseItem.EquipmentSlot.Shield;
				break;
			default:
				return null;
			}
			component.mNGUIBaseItem = new NGUIBaseItem();
			if (specialID != 0)
			{
			}
			component.ItemName = itemName;
			component.ItemLevel = level;
			component.ItemClass = itemClass;
			component.ItemType = b;
			component.Quality = quality;
			component.SpecialID = specialID;
			component.mNGUIBaseItem.equipmentSlot = equipmentSlot;
			component.mNGUIBaseItem.iconAtlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
			return component;
		}
		return null;
	}

	public NGUIBaseItem CreateSpecificEquip(byte level, ItemClasses itemClass, ItemQuality quality, int itemNumber, List<short> prefixIDs, short specificID)
	{
		NGUIBaseItem result = null;
		ItemBase itemBase = null;
		GameObject obj = new GameObject();
		string prefabNameByItemClassAndNumber = GetPrefabNameByItemClassAndNumber(itemClass, itemNumber);
		itemBase = UpgradeItemBase(obj, level, itemClass, prefabNameByItemClassAndNumber, quality, specificID);
		if (itemBase != null)
		{
			itemBase.generateItemProperties();
			foreach (short prefixID in prefixIDs)
			{
				itemBase.AddSkillInfo(prefixID);
			}
			itemBase.generateNGUIBaseItem();
			result = itemBase.mNGUIBaseItem;
		}
		UnityEngine.Object.Destroy(obj);
		return result;
	}

	public NGUIBaseItem CreateSpecificChip(ItemQuality quality, short prefixID, short specificID)
	{
		NGUIBaseItem nGUIBaseItem = null;
		GameObject obj = new GameObject();
		ItemBase itemBase = CreateItemBaseFromGameObject(obj, 1, ItemClasses.V_Slot, 1, quality, specificID);
		if (itemBase is ItemSlot)
		{
			(itemBase as ItemSlot).MakeDefaultChip(prefixID);
			nGUIBaseItem = itemBase.mNGUIBaseItem;
			nGUIBaseItem.ItemClass = ItemClasses.V_Slot;
		}
		UnityEngine.Object.Destroy(obj);
		return nGUIBaseItem;
	}
}

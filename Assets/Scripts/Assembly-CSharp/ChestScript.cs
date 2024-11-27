using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
	private List<PreSpawnBuffer> mPreSpawnItems = new List<PreSpawnBuffer>();

	private InGameUIScript mInGameUIScript;

	protected List<short> RequestItemIDs = new List<short>();

	private static float[] ChestDropQualityRate = new float[5] { 0f, 35f, 3f, 0f, 0f };

	public int mQuestCommonId;

	public int mPrevQuestId;

	public int mSearchItemId;

	protected short mId;

	public GameObject mEffect;

	public ChestType mChestType;

	public bool IsAlreadyOpen { get; set; }

	private void Start()
	{
		IsAlreadyOpen = false;
		mInGameUIScript = GameObject.Find("GameUI").GetComponent<InGameUIScript>();
		ChestDropConfig chestDropConfig = GameConfig.GetInstance().chestDropConfigName[base.gameObject.name];
		if (chestDropConfig.SP1_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP1_ID);
		}
		if (chestDropConfig.SP2_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP2_ID);
		}
		if (chestDropConfig.SP3_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP3_ID);
		}
		if (chestDropConfig.SP4_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP4_ID);
		}
		if (chestDropConfig.SP5_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP5_ID);
		}
		if (chestDropConfig.SP6_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP6_ID);
		}
		if (chestDropConfig.SP7_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP7_ID);
		}
		if (chestDropConfig.SP8_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP8_ID);
		}
		if (chestDropConfig.SP9_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP9_ID);
		}
		if (chestDropConfig.SP10_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP10_ID);
		}
		if (chestDropConfig.SP11_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP11_ID);
		}
		if (chestDropConfig.SP12_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP12_ID);
		}
		if (chestDropConfig.SP13_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP13_ID);
		}
		if (chestDropConfig.SP14_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP14_ID);
		}
		if (chestDropConfig.SP15_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP15_ID);
		}
		if (chestDropConfig.SP16_ID != 0)
		{
			RequestItemIDs.Add(chestDropConfig.SP16_ID);
		}
		if (mEffect != null)
		{
			PositionTweenScript component = mEffect.GetComponent<PositionTweenScript>();
			if (component != null)
			{
				mEffect.transform.position = base.transform.position - Vector3.up * 0.8f;
				component.SetTweenPos(mEffect.transform.position, mEffect.transform.position + Vector3.up * 1f);
			}
			SetActiveEffect(false);
		}
		mId = GameConfig.GetInstance().chestDropConfigName[base.gameObject.name].ID;
		if (GameApp.GetInstance() != null && GameApp.GetInstance().GetGameWorld() != null)
		{
			IsAlreadyOpen = GameApp.GetInstance().GetGameWorld().GetChestOpenState(mId);
			if (IsAlreadyOpen)
			{
				PlayOpen();
			}
		}
	}

	public List<short> GetRequestItemIDs()
	{
		return RequestItemIDs;
	}

	public void SetActiveEffect(bool active)
	{
		if (mEffect != null)
		{
			mEffect.SetActive(active);
		}
	}

	private void Update()
	{
		if (mEffect != null && !IsAlreadyOpen)
		{
			if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckHasBeenAcceptedWithCommonID(mQuestCommonId))
			{
				SetActiveEffect(true);
			}
			else
			{
				SetActiveEffect(false);
			}
		}
	}

	public short GetChestId()
	{
		return mId;
	}

	public void PlayOpen()
	{
		PlayOpenAnimation("open", WrapMode.ClampForever);
		SetActiveEffect(false);
	}

	private void PlayOpenAnimation(string name, WrapMode mode)
	{
		if (!(base.gameObject.GetComponent<Animation>() == null) && !(base.gameObject.GetComponent<Animation>()[name] == null) && (!base.gameObject.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever))
		{
			base.gameObject.GetComponent<Animation>()[name].wrapMode = mode;
			base.gameObject.GetComponent<Animation>().Play(name);
		}
	}

	public void OnLoot()
	{
		if (IsAlreadyOpen)
		{
			Debug.Log("IsAlreadyOpen?");
			return;
		}
		PlayOpen();
		if (mId != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			OpenChestRequest request = new OpenChestRequest(mId);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		Vector3 position = base.transform.position;
		position.y += 1.5f;
		if (!GameConfig.GetInstance().chestDropConfigName.ContainsKey(base.gameObject.name))
		{
			Debug.Log("Chest Data Not Found!");
			return;
		}
		ChestDropConfig chestDropConfig = GameConfig.GetInstance().chestDropConfigName[base.gameObject.name];
		int num = UnityEngine.Random.Range(0, 100);
		int num2 = 0;
		if (num > 20)
		{
			num2 = 1;
		}
		if (num2 >= GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			num2 = 0;
		}
		byte level = (byte)(GameApp.GetInstance().GetUserState().GetCharLevel() - num2);
		for (int i = 2; i < 45; i++)
		{
			if (i == 15 || i == 17 || i == 19 || i == 21 || i == 23 || i == 25 || i == 27 || i == 29 || i == 31 || i == 33 || i == 35 || i == 37 || i == 39 || i == 41 || i == 43 || i == 45)
			{
				continue;
			}
			int num3 = 0;
			int num4 = UnityEngine.Random.Range(0, 100);
			short specialID = 0;
			switch (i)
			{
			case 2:
				num3 = chestDropConfig.SMG_Rate;
				break;
			case 3:
				num3 = chestDropConfig.AssualtRifle_Rate;
				break;
			case 4:
				num3 = chestDropConfig.Pistol_Rate;
				break;
			case 5:
				num3 = chestDropConfig.Revolver_Rate;
				break;
			case 6:
				num3 = chestDropConfig.Shotgun_Rate;
				break;
			case 7:
				num3 = chestDropConfig.Sniper_Rate;
				break;
			case 8:
				num3 = chestDropConfig.RPG_Rate;
				break;
			case 9:
				num3 = chestDropConfig.Grenade_Rate;
				break;
			case 10:
				num3 = chestDropConfig.Shield_Rate;
				break;
			case 11:
				num3 = chestDropConfig.Slot_Rate;
				break;
			case 12:
				num3 = chestDropConfig.FirstAid_Rate;
				break;
			case 13:
				num3 = ((mId <= 1000) ? 100 : 0);
				break;
			case 14:
				num3 = chestDropConfig.BackPack_Rate;
				break;
			case 16:
				specialID = chestDropConfig.SP1_ID;
				num3 = chestDropConfig.SP1_Rate;
				break;
			case 18:
				specialID = chestDropConfig.SP2_ID;
				num3 = chestDropConfig.SP2_Rate;
				break;
			case 20:
				specialID = chestDropConfig.SP3_ID;
				num3 = chestDropConfig.SP3_Rate;
				break;
			case 22:
				specialID = chestDropConfig.SP4_ID;
				num3 = chestDropConfig.SP4_Rate;
				break;
			case 24:
				specialID = chestDropConfig.SP5_ID;
				num3 = chestDropConfig.SP5_Rate;
				break;
			case 26:
				specialID = chestDropConfig.SP6_ID;
				num3 = chestDropConfig.SP6_Rate;
				break;
			case 28:
				specialID = chestDropConfig.SP7_ID;
				num3 = chestDropConfig.SP7_Rate;
				break;
			case 30:
				specialID = chestDropConfig.SP8_ID;
				num3 = chestDropConfig.SP8_Rate;
				break;
			case 32:
				specialID = chestDropConfig.SP9_ID;
				num3 = chestDropConfig.SP9_Rate;
				break;
			case 34:
				specialID = chestDropConfig.SP10_ID;
				num3 = chestDropConfig.SP10_Rate;
				break;
			case 36:
				specialID = chestDropConfig.SP11_ID;
				num3 = chestDropConfig.SP11_Rate;
				break;
			case 38:
				specialID = chestDropConfig.SP12_ID;
				num3 = chestDropConfig.SP12_Rate;
				break;
			case 40:
				specialID = chestDropConfig.SP13_ID;
				num3 = chestDropConfig.SP13_Rate;
				break;
			case 42:
				specialID = chestDropConfig.SP14_ID;
				num3 = chestDropConfig.SP14_Rate;
				break;
			case 44:
				specialID = chestDropConfig.SP15_ID;
				num3 = chestDropConfig.SP15_Rate;
				break;
			case 46:
				specialID = chestDropConfig.SP16_ID;
				num3 = chestDropConfig.SP16_Rate;
				break;
			}
			num3 = (int)((float)num3 * GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.DropRate);
			if (num4 >= num3)
			{
				continue;
			}
			if (i == 16 || i == 18 || i == 20 || i == 22 || i == 24 || i == 26 || i == 28 || i == 30 || i == 32 || i == 34 || i == 36 || i == 38 || i == 40 || i == 42 || i == 44 || i == 46)
			{
				PreSpawnSpecialItem(specialID, position);
				continue;
			}
			ItemClasses itemClasses = GameApp.GetInstance().GetLootManager().mDropColumnName[i];
			if (itemClasses == ItemClasses.X_Money)
			{
				itemClasses = ItemClasses.Bullet;
			}
			ItemQuality quality = ItemQuality.Common;
			float num5 = UnityEngine.Random.Range(0f, 100f);
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			num6 = ChestDropQualityRate[4];
			num7 = ChestDropQualityRate[3];
			num8 = ChestDropQualityRate[2];
			num9 = ChestDropQualityRate[1];
			if (num5 < num6)
			{
				quality = ItemQuality.Legendary;
			}
			else if (num5 < num7)
			{
				quality = ItemQuality.Epic;
			}
			else if (num5 < num8)
			{
				quality = ItemQuality.Rare;
			}
			else if (num5 < num9)
			{
				quality = ItemQuality.Uncommon;
			}
			PreSpawnItem(level, itemClasses, quality, position);
		}
		SpawnItem();
		IsAlreadyOpen = true;
		GameApp.GetInstance().GetGameWorld().SetChestOpenState(mId, true);
	}

	private void PreSpawnItem(byte level, ItemClasses itemClass, ItemQuality quality, Vector3 dropPosition)
	{
		int num = 1;
		if (itemClass == ItemClasses.Bullet)
		{
			num = 2;
		}
		for (int i = 0; i < num; i++)
		{
			short specialID = 0;
			int number = 1;
			PreSpawnBuffer item = default(PreSpawnBuffer);
			byte itemType = 0;
			string itemName = string.Empty;
			GetRandomNameNumberTypeByItemClass(itemClass, ref itemName, ref number, ref itemType);
			item.itemName = itemName;
			item.itemType = itemType;
			item.itemLevel = level;
			item.itemClass = itemClass;
			item.itemQuality = quality;
			item.dropPosition = dropPosition;
			item.isSpecialItem = false;
			item.specialID = specialID;
			mPreSpawnItems.Add(item);
		}
	}

	private void FilterPreSpawnItem(int whiteLimit, int greenLimit, int blueLimit, int purpleLimit, int orangeLimit, int totalLimit)
	{
	}

	private void PreSpawnSpecialItem(short specialID, Vector3 dropPosition)
	{
		SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[specialID];
		PreSpawnBuffer item = default(PreSpawnBuffer);
		item.itemName = specialItemConfig.ItemName;
		item.itemType = (byte)specialItemConfig.ItemType;
		item.itemClass = specialItemConfig.ItemClass;
		item.itemQuality = specialItemConfig.Quality;
		item.dropPosition = dropPosition;
		item.isSpecialItem = true;
		item.specialID = specialID;
		item.prefabName = "Item/Special/" + specialItemConfig.PrefabName;
		mPreSpawnItems.Add(item);
	}

	private void SpawnItem()
	{
		for (int i = 0; i < mPreSpawnItems.Count; i++)
		{
			GameObject gameObject = null;
			GameObject gameObject2 = null;
			byte itemLevel = mPreSpawnItems[i].itemLevel;
			byte itemType = mPreSpawnItems[i].itemType;
			ItemClasses itemClass = mPreSpawnItems[i].itemClass;
			string itemStyle = mPreSpawnItems[i].itemStyle;
			ItemQuality quality = mPreSpawnItems[i].itemQuality;
			Vector3 dropPosition = mPreSpawnItems[i].dropPosition;
			Vector3 vector = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition() - dropPosition;
			float x = UnityEngine.Random.Range(-3.5f, 3.5f);
			float num = UnityEngine.Random.Range(0.5f, 0.9f);
			float z = UnityEngine.Random.Range(-3.5f, 3.5f);
			vector.y += num;
			Debug.Log(vector);
			if (!mPreSpawnItems[i].isSpecialItem)
			{
				string itemName = mPreSpawnItems[i].itemName;
				string text = GameApp.GetInstance().GetLootManager().GeneratePrefabName(itemType, itemName);
				gameObject = Resources.Load(text) as GameObject;
				if (gameObject == null)
				{
					Debug.Log("itemPrefab not found! ------ " + text);
					continue;
				}
				float y = UnityEngine.Random.Range(0, 360);
				Quaternion rotation = Quaternion.Euler(0f, y, 90f);
				gameObject2 = UnityEngine.Object.Instantiate(gameObject, dropPosition, rotation) as GameObject;
				gameObject2.GetComponent<ItemBase>().ItemName = itemName;
				if (itemType == 4 || itemType == 9 || itemType == 6 || itemType == 8)
				{
					quality = ItemQuality.Common;
				}
			}
			else
			{
				string prefabName = mPreSpawnItems[i].prefabName;
				if (mPreSpawnItems[i].itemType == 7)
				{
					if (!GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailable(mPreSpawnItems[i].specialID))
					{
						continue;
					}
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						ItemSpawnRequest request = new ItemSpawnRequest(mPreSpawnItems[i].specialID, dropPosition, new Vector3(x, num, z));
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						break;
					}
				}
				gameObject = Resources.Load(prefabName) as GameObject;
				if (gameObject == null)
				{
					Debug.Log("itemPrefab not found! ----------" + prefabName);
					continue;
				}
				gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
				gameObject2.transform.position = dropPosition;
			}
			gameObject2.GetComponent<Rigidbody>().detectCollisions = true;
			gameObject2.GetComponent<Rigidbody>().AddForce(vector, ForceMode.Impulse);
			byte itemNumber = 1;
			if (itemType != 8 && itemType != 7 && itemType != 9)
			{
				string value = mPreSpawnItems[i].itemName[mPreSpawnItems[i].itemName.Length - 2].ToString() + mPreSpawnItems[i].itemName[mPreSpawnItems[i].itemName.Length - 1];
				itemNumber = Convert.ToByte(value);
			}
			GameApp.GetInstance().GetLootManager().CreateItemBaseFromGameObject(gameObject2, itemLevel, itemClass, itemNumber, quality, mPreSpawnItems[i].specialID);
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

	private bool GenerateNameTypeStyle(byte level, ItemClasses itemClass, ref string outputItemName, ref byte outputItemType, ref string outputItemStyle, ref short outputSpecialID)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[GameApp.GetInstance().GetLootManager().mClassToTableName[itemClass]];
		List<int> list = new List<int>();
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			int data = unitDataTable.GetData(i, 2, 0, false);
			int data2 = unitDataTable.GetData(i, 3, 0, false);
			if (data <= level && level <= data2)
			{
				list.Add(i);
			}
		}
		if (list.Count <= 0)
		{
			Debug.Log("level limit no match");
			return false;
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		int iRow = list[index];
		outputItemName = unitDataTable.GetData(iRow, 0, string.Empty, false);
		outputItemType = (byte)unitDataTable.GetData(iRow, 4, 0, false);
		outputItemStyle = unitDataTable.GetData(iRow, 1, string.Empty, false);
		if (outputItemType == 4)
		{
			outputSpecialID = (short)unitDataTable.GetData(iRow, 5, 0, false);
		}
		outputItemName = "aas";
		outputItemType = 0;
		outputItemStyle = "All";
		return true;
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
			WeaponType weaponType = (WeaponType)UnityEngine.Random.Range(1, 9);
			itemName = "Bullet_" + weaponType;
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
}

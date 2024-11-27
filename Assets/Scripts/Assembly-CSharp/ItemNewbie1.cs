using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemNewbie1 : GlobalIAPItem
{
	private bool bNewbie1;

	public void ReadData(BinaryReader br)
	{
		bNewbie1 = br.ReadBoolean();
	}

	public void WriteData(BinaryWriter bw)
	{
		bw.Write(bNewbie1);
	}

	public void Resume()
	{
		if (!bNewbie1)
		{
			bNewbie1 = true;
		}
		List<RoleStateInfo> roles = GameApp.GetInstance().GetGlobalState().GetRoles();
		for (int i = 0; i < roles.Count; i++)
		{
			roles[i].bNewbie1 = false;
		}
		GameApp.GetInstance().Save();
		if (GameApp.GetInstance().GetGameScene() != null)
		{
			CheckItem();
		}
	}

	public void CheckItem()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		ItemInfo itemInfoData = userState.ItemInfoData;
		if (userState.HasNewbie1() || !bNewbie1)
		{
			return;
		}
		userState.SetNewbie1(true);
		NGUIBaseItem nGUIBaseItem = new NGUIBaseItem();
		List<short> list = new List<short>();
		list.Add(303);
		list.Add(304);
		list.Add(306);
		list.Add(3443);
		nGUIBaseItem = GameApp.GetInstance().GetLootManager().CreateSpecificEquip(4, ItemClasses.SubmachineGun, ItemQuality.Legendary, 6, list, 9000);
		nGUIBaseItem.SetPrice(0);
		if (nGUIBaseItem != null)
		{
			NGUIGameItem value = new NGUIGameItem(9000, nGUIBaseItem);
			if (!itemInfoData.BackPackIsFull())
			{
				for (int i = 0; i < itemInfoData.BackpackSlotCount; i++)
				{
					if (itemInfoData.BackPackItems[i] == null)
					{
						itemInfoData.BackPackItems[i] = value;
						break;
					}
				}
			}
			else
			{
				GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(nGUIBaseItem, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetPosition() + Vector3.up, Vector3.zero);
			}
		}
		NGUIBaseItem nGUIBaseItem2 = new NGUIBaseItem();
		List<short> list2 = new List<short>();
		list2.Add(620);
		list2.Add(3044);
		nGUIBaseItem2 = GameApp.GetInstance().GetLootManager().CreateSpecificEquip(4, ItemClasses.U_Shield, ItemQuality.Legendary, 4, list2, 9001);
		nGUIBaseItem2.SetPrice(0);
		if (nGUIBaseItem2 != null)
		{
			NGUIGameItem value2 = new NGUIGameItem(9001, nGUIBaseItem2);
			if (!itemInfoData.BackPackIsFull())
			{
				for (int j = 0; j < itemInfoData.BackpackSlotCount; j++)
				{
					if (itemInfoData.BackPackItems[j] == null)
					{
						itemInfoData.BackPackItems[j] = value2;
						break;
					}
				}
			}
			else
			{
				GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(nGUIBaseItem2, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetPosition() + Vector3.up, Vector3.zero);
			}
		}
		NGUIBaseItem nGUIBaseItem3 = new NGUIBaseItem();
		nGUIBaseItem3 = GameApp.GetInstance().GetLootManager().CreateSpecificChip(ItemQuality.Uncommon, 301, 9002);
		nGUIBaseItem3.SetPrice(0);
		if (nGUIBaseItem3 == null)
		{
			return;
		}
		NGUIGameItem value3 = new NGUIGameItem(9002, nGUIBaseItem3);
		if (!itemInfoData.BackPackIsFull())
		{
			for (int k = 0; k < itemInfoData.BackpackSlotCount; k++)
			{
				if (itemInfoData.BackPackItems[k] == null)
				{
					itemInfoData.BackPackItems[k] = value3;
					break;
				}
			}
		}
		else
		{
			GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(nGUIBaseItem3, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition() + Vector3.up, Vector3.zero);
		}
	}
}

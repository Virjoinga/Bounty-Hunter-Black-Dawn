using System.Collections.Generic;
using UnityEngine;

public class UIEquipmenetSelectionStorage : NGUIStorage
{
	private void Start()
	{
		if (!(template != null))
		{
			return;
		}
		Transform parent = base.transform;
		int num = 0;
		Bounds bounds = default(Bounds);
		List<UIEquipmentSelectionSlot> list = new List<UIEquipmentSelectionSlot>();
		bool flag = false;
		for (int i = 0; i < maxColumns; i++)
		{
			if (flag)
			{
				break;
			}
			for (int j = 0; j < maxRows; j++)
			{
				if (flag)
				{
					break;
				}
				GameObject gameObject = Object.Instantiate(template) as GameObject;
				NGUITools.SetActive(gameObject, true);
				Transform transform = gameObject.transform;
				transform.parent = parent;
				transform.localPosition = new Vector3((float)padding + ((float)i + 0.5f) * (float)spacing, (float)(-padding) - ((float)j + 0.5f) * (float)spacing, -1f);
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				UIEquipmentSelectionSlot component = gameObject.GetComponent<UIEquipmentSelectionSlot>();
				if (component != null)
				{
					int slotNumber = i * maxRows + j;
					component.EquipItem = null;
					component.SlotNumber = slotNumber;
					list.Add(component);
				}
				bounds.Encapsulate(new Vector3((float)padding * 2f + (float)((i + 1) * spacing), (float)(-padding) * 2f - (float)((j + 1) * spacing), 0f));
				if (++num >= maxItemCount)
				{
					if (background != null)
					{
						background.transform.localScale = bounds.size;
					}
					flag = true;
				}
			}
		}
		if (background != null)
		{
			background.transform.localScale = bounds.size;
		}
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		foreach (NGUIGameItem backPackItem in itemInfoData.BackPackItems)
		{
			if (backPackItem == null || backPackItem.baseItem.GetMaxUpgradeCount() <= 0)
			{
				continue;
			}
			foreach (UIEquipmentSelectionSlot item in list)
			{
				if (item.EquipItem == null)
				{
					item.EquipItem = backPackItem;
					break;
				}
			}
		}
	}

	public override void Refresh()
	{
		int childCount = base.transform.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Object.Destroy(base.transform.GetChild(i).gameObject);
		}
		Start();
	}
}

using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentSelection : UIDelegateMenu
{
	public static UIEquipmentSelection mInstance;

	public UIEquipmentSelectionSlot Weapon1;

	public UIEquipmentSelectionSlot Weapon2;

	public UIEquipmentSelectionSlot Weapon3;

	public UIEquipmentSelectionSlot Weapon4;

	public UIEquipmentSelectionSlot Grenade;

	public UIEquipmentSelectionSlot Shield;

	public UIEquipmenetSelectionStorage Backpack;

	public GameObject SelectButton;

	private string mSelectButtonName;

	private NGUIGameItem mSelected;

	public EquipmentUpgrade mEquipmentUpgrade { get; set; }

	public NGUIGameItem SelectedEquip
	{
		get
		{
			return mSelected;
		}
		set
		{
			mSelected = value;
			if (mSelected == null)
			{
				mEquipmentUpgrade.SetUpgradeItem(null);
			}
			else
			{
				mEquipmentUpgrade.SetUpgradeItem(mSelected.baseItem);
			}
		}
	}

	private void Start()
	{
		mInstance = this;
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		Weapon1.EquipItem = itemInfoData.Weapon1;
		Weapon2.EquipItem = itemInfoData.Weapon2;
		Weapon3.EquipItem = itemInfoData.Weapon3;
		Weapon4.EquipItem = itemInfoData.Weapon4;
		Grenade.EquipItem = itemInfoData.HandGrenade;
		Shield.EquipItem = itemInfoData.Shield;
		Backpack.ClearItem();
		Backpack.maxItemCount = itemInfoData.BackpackSlotCount;
		List<NGUIGameItem> items = Backpack.items;
		for (int i = 0; i < itemInfoData.BackpackSlotCount; i++)
		{
			items[i] = itemInfoData.BackPackItems[i];
		}
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void Update()
	{
	}

	public void SetEquipmentUpgrade(EquipmentUpgrade upgrade)
	{
		mEquipmentUpgrade = upgrade;
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (IsThisObject(go, mSelectButtonName))
		{
			Object.Destroy(base.gameObject);
		}
	}
}

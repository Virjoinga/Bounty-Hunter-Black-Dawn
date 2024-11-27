using System;
using System.Collections.Generic;
using UnityEngine;

public class NGUIItemEquipmentSlot : NGUIItemSlot, UIMsgListener
{
	public NGUIBaseItem.EquipmentSlot equipmentSlot;

	public NGUIGameItem GameItem
	{
		get
		{
			return mItem;
		}
	}

	public bool SlotEnabled { get; set; }

	protected override NGUIGameItem observedItem
	{
		get
		{
			return mItem;
		}
	}

	protected override NGUIGameItem Replace(NGUIGameItem item)
	{
		if (!SlotEnabled)
		{
			if (item.baseItem.equipmentSlot == equipmentSlot && NGUIBackPackUIScript.mInstance != null)
			{
				NGUIBackPackUIScript.mInstance.UnlockSlot(base.gameObject);
			}
			return item;
		}
		NGUIGameItem result = null;
		if (mItem != null)
		{
			result = new NGUIGameItem(0, mItem.baseItem);
		}
		if (item == null)
		{
			mItem = null;
		}
		else
		{
			if (item.baseItem.equipmentSlot != equipmentSlot || item == mItem)
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_EQUIP_WRONG_SLOT_WARNING"), 2);
				return item;
			}
			if (item.baseItem.ItemLevel > GameApp.GetInstance().GetUserState().GetCharLevel())
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_EQUIP_DENY_WARNING"), 2);
				return item;
			}
			if (item.baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.SkillSlot && ChipConflict(item))
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_EQUIP_CHIP_WARNING"), 2);
				Debug.Log("Only one chip of each type can be equipped!");
				return item;
			}
			mItem = item;
		}
		needUpdate = true;
		GameApp.GetInstance().GetUserState().ItemInfoData.RefreshCurrentInfo();
		if (equipmentSlot == NGUIBaseItem.EquipmentSlot.Weapon || equipmentSlot == NGUIBaseItem.EquipmentSlot.WeaponG)
		{
			if (mItem != null || equipmentSlot != NGUIBaseItem.EquipmentSlot.Weapon || GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.CanUnEquipWeapon(this))
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
			}
		}
		else if (equipmentSlot == NGUIBaseItem.EquipmentSlot.Shield)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.RefreshShieldFromItemInfo();
		}
		return result;
	}

	protected override void OnClick()
	{
		if (SlotEnabled)
		{
			base.OnClick();
		}
		else if (NGUIBackPackUIScript.mInstance != null)
		{
			NGUIBackPackUIScript.mInstance.UnlockSlot(base.gameObject);
		}
	}

	public void SetItem(NGUIGameItem item)
	{
		if (SlotEnabled)
		{
			mItem = item;
			needUpdate = true;
		}
	}

	public override void ThrowItem()
	{
		if (equipmentSlot != NGUIBaseItem.EquipmentSlot.Weapon || GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.CanUnEquipWeapon(this))
		{
			base.ThrowItem();
		}
	}

	public void Equip(NGUIGameItem item)
	{
		Replace(item);
	}

	protected bool ChipConflict(NGUIGameItem item)
	{
		List<NGUIGameItem> list = new List<NGUIGameItem>();
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		list.Add(itemInfoData.Slot1);
		list.Add(itemInfoData.Slot2);
		list.Add(itemInfoData.Slot3);
		list.Add(itemInfoData.Slot4);
		int num = Convert.ToInt32(base.gameObject.name[base.gameObject.name.Length - 1].ToString());
		Debug.Log(base.gameObject.name + "----" + num);
		list.RemoveAt(num - 1);
		foreach (NGUIGameItem item2 in list)
		{
			if (item2 != null)
			{
				Debug.Log(item2.baseItem.skillIDs[0]);
				Debug.Log(item.baseItem.skillIDs[0]);
				if (item2.baseItem.skillIDs[0] % 100 == item.baseItem.skillIDs[0] % 100)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected override void UpdateIcons(NGUIBaseItem baseItem)
	{
		if (baseItem == null || baseItem.iconAtlas == null)
		{
			icon.enabled = false;
			backgroundIcon.enabled = false;
		}
		else
		{
			icon.atlas = baseItem.iconAtlas;
			icon.spriteName = baseItem.iconName;
			icon.enabled = true;
			backgroundIcon.enabled = true;
			backgroundIcon.atlas = baseItem.iconAtlas;
			backgroundIcon.spriteName = baseItem.GetBackGroundColorStringByQuality();
		}
		if ((baseItem == null || (baseItem.Quality != ItemQuality.Legendary && baseItem.Quality != ItemQuality.Epic)) && QualityEffectObject != null)
		{
			UnityEngine.Object.Destroy(QualityEffectObject);
			QualityEffectObject = null;
		}
		if (baseItem != null && QualityEffectObject == null)
		{
			if (baseItem.Quality == ItemQuality.Legendary)
			{
				GameObject original = Resources.Load("RPG_effect/RPG_UI_Orange001") as GameObject;
				QualityEffectObject = UnityEngine.Object.Instantiate(original) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = Vector3.zero + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(7f, 7f, 7f);
			}
			else if (baseItem.Quality == ItemQuality.Epic)
			{
				GameObject original2 = Resources.Load("RPG_effect/RPG_UI_Purple001") as GameObject;
				QualityEffectObject = UnityEngine.Object.Instantiate(original2) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = Vector3.zero + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(5f, 5f, 5f);
			}
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}
}

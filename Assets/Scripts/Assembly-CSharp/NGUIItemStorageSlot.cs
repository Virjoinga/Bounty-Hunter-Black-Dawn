public class NGUIItemStorageSlot : NGUIItemSlot
{
	public NGUIStorage storage;

	public int slot;

	public NGUIGameItem GameItem
	{
		get
		{
			return observedItem;
		}
	}

	protected override NGUIGameItem observedItem
	{
		get
		{
			return (!(storage != null)) ? null : storage.GetItem(slot);
		}
	}

	protected override NGUIGameItem Replace(NGUIGameItem item)
	{
		if (storage != null)
		{
			NGUIGameItem nGUIGameItem = storage.Replace(slot, item);
			if ((nGUIGameItem == null || (nGUIGameItem.baseItem.equipmentSlot != NGUIBaseItem.EquipmentSlot.Weapon && nGUIGameItem.baseItem.equipmentSlot != NGUIBaseItem.EquipmentSlot.WeaponG)) && !GameApp.GetInstance().GetUserState().ItemInfoData.HaveWeapon())
			{
				storage.Replace(slot, nGUIGameItem);
				return item;
			}
			GameApp.GetInstance().GetUserState().ItemInfoData.RefreshCurrentInfo();
			return nGUIGameItem;
		}
		GameApp.GetInstance().GetUserState().ItemInfoData.RefreshCurrentInfo();
		return item;
	}

	public void SetItem(NGUIGameItem item)
	{
		mItem = item;
	}

	public override void ThrowItem()
	{
		base.ThrowItem();
	}
}

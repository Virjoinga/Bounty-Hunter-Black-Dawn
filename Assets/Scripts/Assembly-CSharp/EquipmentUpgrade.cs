using UnityEngine;

public abstract class EquipmentUpgrade
{
	protected UIEquipmentUpgrade uiEquipmentUpgrade;

	protected NGUIBaseItem beforeUpgradeItem;

	protected NGUIBaseItem afterUpgradeItem;

	private UIEquipmentUpgrade.UpgradeType upgradeType;

	public UIEquipmentUpgrade.UpgradeType UpgradeType
	{
		get
		{
			return upgradeType;
		}
	}

	public NGUIBaseItem.EquipmentSlot mSlot { get; set; }

	public int mSlotNumber { get; set; }

	public UIEquipmentUpgrade UIEquipmentUpgrade
	{
		get
		{
			return uiEquipmentUpgrade;
		}
	}

	public void Init(UIEquipmentUpgrade ui, UIEquipmentUpgrade.UpgradeType type)
	{
		uiEquipmentUpgrade = ui;
		upgradeType = type;
		uiEquipmentUpgrade.spriteUpgradeDisable.SetActive(true);
		OnCreate();
	}

	protected abstract void OnCreate();

	public void Click(GameObject gameObject)
	{
		OnClick(gameObject);
	}

	protected abstract void OnClick(GameObject gameObject);

	public void Destroy()
	{
		OnDestroy();
		uiEquipmentUpgrade = null;
		beforeUpgradeItem = null;
		afterUpgradeItem = null;
	}

	protected virtual void OnDestroy()
	{
	}

	public bool SetUpgradeItem(NGUIBaseItem item)
	{
		beforeUpgradeItem = item;
		afterUpgradeItem = GameApp.GetInstance().GetLootManager().UpgradeItem(beforeUpgradeItem);
		uiEquipmentUpgrade.desciptionBefore.SetObserveItem(beforeUpgradeItem);
		if (item != null && beforeUpgradeItem.UpgradeTimes >= beforeUpgradeItem.GetMaxUpgradeCount())
		{
			uiEquipmentUpgrade.afterTipsLabel.gameObject.SetActive(true);
			uiEquipmentUpgrade.desciptionAfter.gameObject.SetActive(false);
			uiEquipmentUpgrade.spriteUpgradeDisable.SetActive(true);
			uiEquipmentUpgrade.buttonUpgrade.SetActive(false);
			uiEquipmentUpgrade.afterTipsLabel.text = LocalizationManager.GetInstance().GetString("MSG_FULLY_UPGRADE");
			uiEquipmentUpgrade.costLabel.gameObject.SetActive(false);
			return false;
		}
		if (item != null && beforeUpgradeItem.ItemLevel >= GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			uiEquipmentUpgrade.afterTipsLabel.gameObject.SetActive(true);
			uiEquipmentUpgrade.desciptionAfter.gameObject.SetActive(false);
			uiEquipmentUpgrade.spriteUpgradeDisable.SetActive(true);
			uiEquipmentUpgrade.buttonUpgrade.SetActive(false);
			uiEquipmentUpgrade.afterTipsLabel.text = LocalizationManager.GetInstance().GetString("MSG_CANT_UPGRADE");
			uiEquipmentUpgrade.costLabel.gameObject.SetActive(false);
			return false;
		}
		uiEquipmentUpgrade.afterTipsLabel.gameObject.SetActive(false);
		uiEquipmentUpgrade.desciptionAfter.gameObject.SetActive(true);
		if (afterUpgradeItem != null)
		{
			uiEquipmentUpgrade.spriteUpgradeDisable.SetActive(false);
			uiEquipmentUpgrade.buttonUpgrade.SetActive(true);
		}
		uiEquipmentUpgrade.desciptionAfter.SetObserveItem(afterUpgradeItem);
		if (item != null)
		{
			uiEquipmentUpgrade.costLabel.gameObject.SetActive(true);
			uiEquipmentUpgrade.costLabel.text = string.Empty + beforeUpgradeItem.GetUpgradePrice();
		}
		else
		{
			uiEquipmentUpgrade.costLabel.gameObject.SetActive(false);
		}
		return true;
	}

	public void Update()
	{
		OnUpdate();
	}

	protected virtual void OnUpdate()
	{
	}
}

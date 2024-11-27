using UnityEngine;

public class AttackUpgrade : EquipmentUpgrade, UIMsgListener
{
	protected override void OnCreate()
	{
		uiEquipmentUpgrade.selfBlock.enabled = false;
		uiEquipmentUpgrade.spriteCostGold.SetActive(true);
		uiEquipmentUpgrade.AddListenerTo(uiEquipmentUpgrade.buttonUpgrade);
		uiEquipmentUpgrade.uiEquipmentSelection.SetEquipmentUpgrade(this);
	}

	protected override void OnClick(GameObject gameObject)
	{
		if (!gameObject.Equals(uiEquipmentUpgrade.buttonUpgrade) || beforeUpgradeItem == null || beforeUpgradeItem.UpgradeTimes >= beforeUpgradeItem.GetMaxUpgradeCount())
		{
			return;
		}
		if (GameApp.GetInstance().GetUserState().Buy(beforeUpgradeItem.GetUpgradePrice()))
		{
			ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
			switch (base.mSlot)
			{
			case NGUIBaseItem.EquipmentSlot.None:
			{
				for (int i = 0; i < itemInfoData.BackpackSlotCount; i++)
				{
					if (itemInfoData.BackPackItems[i] != null && itemInfoData.BackPackItems[i].baseItem == beforeUpgradeItem)
					{
						itemInfoData.BackPackItems[i] = new NGUIGameItem(itemInfoData.BackPackItems[i].baseItemID, afterUpgradeItem);
						break;
					}
				}
				uiEquipmentUpgrade.Backpack.Refresh();
				break;
			}
			case NGUIBaseItem.EquipmentSlot.Weapon:
				switch (base.mSlotNumber)
				{
				case 1:
					itemInfoData.Weapon1 = new NGUIGameItem(itemInfoData.Weapon1.baseItemID, afterUpgradeItem);
					uiEquipmentUpgrade.uiEquipmentSelection.Weapon1.EquipItem = itemInfoData.Weapon1;
					break;
				case 2:
					itemInfoData.Weapon2 = new NGUIGameItem(itemInfoData.Weapon2.baseItemID, afterUpgradeItem);
					uiEquipmentUpgrade.uiEquipmentSelection.Weapon2.EquipItem = itemInfoData.Weapon2;
					break;
				case 3:
					itemInfoData.Weapon3 = new NGUIGameItem(itemInfoData.Weapon3.baseItemID, afterUpgradeItem);
					uiEquipmentUpgrade.uiEquipmentSelection.Weapon3.EquipItem = itemInfoData.Weapon3;
					break;
				case 4:
					itemInfoData.Weapon4 = new NGUIGameItem(itemInfoData.Weapon4.baseItemID, afterUpgradeItem);
					uiEquipmentUpgrade.uiEquipmentSelection.Weapon4.EquipItem = itemInfoData.Weapon4;
					break;
				}
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
				break;
			case NGUIBaseItem.EquipmentSlot.WeaponG:
				itemInfoData.HandGrenade = new NGUIGameItem(itemInfoData.HandGrenade.baseItemID, afterUpgradeItem);
				uiEquipmentUpgrade.uiEquipmentSelection.Grenade.EquipItem = itemInfoData.HandGrenade;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
				break;
			case NGUIBaseItem.EquipmentSlot.Shield:
				itemInfoData.Shield = new NGUIGameItem(itemInfoData.Shield.baseItemID, afterUpgradeItem);
				uiEquipmentUpgrade.uiEquipmentSelection.Shield.EquipItem = itemInfoData.Shield;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshShieldFromItemInfo();
				break;
			}
			uiEquipmentUpgrade.selfBlock.enabled = true;
			uiEquipmentUpgrade.gearAnimation.PlayForward(OnUpgradeAnimationEnd);
			uiEquipmentUpgrade.beforeGearRailAnimation.PlayForward();
			uiEquipmentUpgrade.afterGearRailAnimation.PlayForward();
			AudioManager.GetInstance().StopSound("RPG_Audio/Menu/upgradeing");
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/Menu/upgradeing");
			EffectPlayer.GetInstance().PlaySparkOfUpgrade(uiEquipmentUpgrade.sparkCamera.transform);
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GOLD_NOT_ENOUGH"), 2, 99);
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (beforeUpgradeItem != null)
		{
			string newValue = ((beforeUpgradeItem.GetMaxUpgradeCount() < 99) ? (string.Empty + (beforeUpgradeItem.GetMaxUpgradeCount() - beforeUpgradeItem.UpgradeTimes)) : "/max");
			uiEquipmentUpgrade.upgradeTimesLabel.text = LocalizationManager.GetInstance().GetString("MENU_UPGRADE_TIMES_LEFT").Replace("%d", newValue);
		}
	}

	private void OnUpgradeAnimationEnd()
	{
		Debug.Log("END");
		AudioManager.GetInstance().StopSound("RPG_Audio/Menu/upgrade_over");
		AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/Menu/upgrade_over");
		uiEquipmentUpgrade.gearAnimation.Reset();
		uiEquipmentUpgrade.beforeGearRailAnimation.Reset();
		uiEquipmentUpgrade.afterGearRailAnimation.Reset();
		if (beforeUpgradeItem != null && afterUpgradeItem != null)
		{
			if (SetUpgradeItem(afterUpgradeItem))
			{
				PlayUpgradeFinishAnimation(uiEquipmentUpgrade.afterUpgradeAnimation);
			}
			PlayUpgradeFinishAnimation(uiEquipmentUpgrade.beforeUpgradeAnimation);
		}
		uiEquipmentUpgrade.selfBlock.enabled = false;
	}

	private void PlayUpgradeFinishAnimation(GameObject go)
	{
		GameObject gameObject = Object.Instantiate(go) as GameObject;
		gameObject.transform.parent = go.transform.parent;
		gameObject.transform.localPosition = go.transform.localPosition;
		gameObject.AddComponent<AutoDestroyScript>();
		UITweenX component = gameObject.GetComponent<UITweenX>();
		component.PlayForward();
		AutoDestroyScript component2 = gameObject.GetComponent<AutoDestroyScript>();
		component2.life = 1.5f;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 9)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.IAP);
			}
		}
		else if (whichMsg.EventId == 99 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
			UIIAP.Show(UIIAP.Type.Exchange);
		}
	}
}

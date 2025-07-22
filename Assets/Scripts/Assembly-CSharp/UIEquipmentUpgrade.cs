using UnityEngine;

public class UIEquipmentUpgrade : UIGameMenu
{
	public enum UpgradeType
	{
		Attack = 0
	}

	public GameObject buttonUpgrade;

	public GameObject spriteUpgradeDisable;

	public GameObject beforeUpgradeAnimation;

	public GameObject afterUpgradeAnimation;

	public UITweenX beforeGearRailAnimation;

	public UITweenX afterGearRailAnimation;

	public UITweenX gearAnimation;

	public ItemDescriptionScript desciptionBefore;

	public ItemDescriptionScript desciptionAfter;

	public UILabel afterTipsLabel;

	public UILabel costLabel;

	public GameObject spriteCostGold;

	public GameObject spriteCostMithril;

	public UIEquipmentSelection uiEquipmentSelection;

	public UIEquipmenetSelectionStorage Backpack;

	public GameObject sparkCamera;

	public UILabel upgradeTimesLabel;

	public UpgradeType upgradeType;

	public BoxCollider selfBlock;

	private EquipmentUpgrade upgrade;

	private static int prevPhase;

	protected override void Awake()
	{
		base.Awake();
		selfBlock = base.gameObject.GetComponent<Collider>() as BoxCollider;
		selfBlock.size = new Vector3(Screen.width, Screen.height, 0f);
		buttonUpgrade.SetActive(false);
		spriteUpgradeDisable.SetActive(false);
		afterTipsLabel.gameObject.SetActive(false);
		costLabel.gameObject.SetActive(false);
		spriteCostGold.SetActive(false);
		spriteCostMithril.SetActive(false);
		upgradeTimesLabel.text = string.Empty;
	}

	private void Update()
	{
		if (upgrade != null)
		{
			if (upgrade.UpgradeType != upgradeType)
			{
				upgrade = CreateUpgrade(upgradeType);
				upgrade.Init(this, upgradeType);
			}
			else
			{
				upgrade.Update();
			}
		}
		else
		{
			upgrade = CreateUpgrade(upgradeType);
			upgrade.Init(this, upgradeType);
		}
	}

	private EquipmentUpgrade CreateUpgrade(UpgradeType type)
	{
		if (type != 0)
		{
		}
		return new AttackUpgrade();
	}

	public void AddListenerTo(GameObject go)
	{
		if (go != null)
		{
			AddDelegate(go);
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (upgrade != null)
		{
			upgrade.Click(go);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (upgrade != null)
		{
			upgrade.Destroy();
		}
	}

	public static void Show()
	{
		if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() != 40)
		{
			prevPhase = 6;
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(40, false, false, true);
		}
	}

	public static void Close()
	{
		if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 40)
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		}
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}
}

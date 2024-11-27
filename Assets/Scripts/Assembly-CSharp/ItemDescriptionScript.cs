using UnityEngine;

public class ItemDescriptionScript : UIDelegateMenu
{
	public static ItemDescriptionScript mInstance;

	public UILabel Name;

	public UILabel Level;

	public GameObject LevelBackground;

	public GameObject NameBackground;

	public UILabel Mag;

	public UILabel Price;

	public UILabel DescriptionLabel;

	public UISprite DescriptionBackground;

	public UISprite CompanyLogo;

	public UISprite QualityColor;

	public UISprite ItemPreview;

	public UISprite BulletIcon;

	public UISprite CurrencyIcon;

	public UISlicedSprite DescBackground;

	public GameObject MagAndPriceLabels;

	public UpgradeInfoScript UpgradeInfoObject;

	public ShopItemSlotScript SpecialSell;

	public UILabel SpecialSellName;

	public GameObject FireElementIcon;

	public GameObject ShockElementIcon;

	public GameObject CorrosiveElementIcon;

	public GameObject ExplosiveElementIcon;

	public UIGrid ElementGrid;

	public GameObject EmptyInformation;

	public ItemStatsScript ItemStats;

	public bool IsForShop;

	public bool IsForUpgrade;

	public GameObject CloseBtn;

	private string CloseBtnName;

	protected NGUIBaseItem m_ObserveItem;

	protected GameObject QualityEffectObject;

	private NGUIBaseItem m_LastObserveItem;

	public NGUIBaseItem GetObjserveItem()
	{
		return m_ObserveItem;
	}

	private void Awake()
	{
		if (CloseBtn != null)
		{
			AddDelegate(CloseBtn, out CloseBtnName);
		}
	}

	private void OnEnable()
	{
		mInstance = this;
		ClearDescription();
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void OnDisable()
	{
		mInstance = null;
	}

	private void Update()
	{
		if (m_LastObserveItem == m_ObserveItem)
		{
			return;
		}
		m_LastObserveItem = m_ObserveItem;
		if (m_ObserveItem != null)
		{
			LevelBackground.SetActive(true);
			NameBackground.SetActive(true);
			Name.text = m_ObserveItem.GetStringColorPrefixByQuality() + m_ObserveItem.DisplayName + "[-]";
			if (m_ObserveItem.ItemClass != ItemClasses.V_Slot)
			{
				Level.text = "Lv: " + m_ObserveItem.ItemLevel;
				if (m_ObserveItem.ItemLevel > GameApp.GetInstance().GetUserState().GetCharLevel())
				{
					Level.text = "[ff0000]" + Level.text + "[-]";
				}
			}
			else
			{
				Level.text = string.Empty;
				LevelBackground.SetActive(false);
				NameBackground.SetActive(false);
			}
			Mag.text = "--";
			if (m_ObserveItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Weapon)
			{
				Mag.text = ((int)m_ObserveItem.itemStats[4].statValue).ToString();
			}
			if (CurrencyIcon != null)
			{
				if (m_ObserveItem.MithrilItem)
				{
					CurrencyIcon.spriteName = "mithril";
				}
				else
				{
					CurrencyIcon.spriteName = "gold2";
				}
			}
			if (IsForShop)
			{
				if (Price != null)
				{
					if (m_ObserveItem.MithrilItem)
					{
						if (GameApp.GetInstance().GetGlobalState().GetMithril() < m_ObserveItem.MithrilPrice)
						{
							Price.text = "[FF0000]" + m_ObserveItem.MithrilPrice + "[-]";
						}
						else
						{
							Price.text = m_ObserveItem.MithrilPrice.ToString();
						}
					}
					else if (m_ObserveItem.GetPrice() > GameApp.GetInstance().GetUserState().GetCash())
					{
						Price.text = "[FF0000]" + m_ObserveItem.GetPrice() + "[-]";
					}
					else
					{
						Price.text = m_ObserveItem.GetPrice().ToString();
					}
				}
			}
			else if (Price != null)
			{
				Price.text = m_ObserveItem.GetPrice().ToString();
			}
			if (m_ObserveItem.equipmentSlot != NGUIBaseItem.EquipmentSlot.Weapon && m_ObserveItem.equipmentSlot != NGUIBaseItem.EquipmentSlot.WeaponG && m_ObserveItem.equipmentSlot != NGUIBaseItem.EquipmentSlot.Shield)
			{
				DescriptionLabel.text = m_ObserveItem.Description;
				DescriptionBackground.gameObject.SetActive(true);
			}
			else
			{
				DescriptionLabel.text = string.Empty;
				DescriptionBackground.gameObject.SetActive(false);
			}
			if (m_ObserveItem.Company != 0)
			{
				CompanyLogo.enabled = true;
				CompanyLogo.spriteName = GetCompanyLogoString(m_ObserveItem.Company);
			}
			else
			{
				CompanyLogo.enabled = false;
			}
			QualityColor.enabled = true;
			QualityColor.spriteName = m_ObserveItem.GetBackGroundColorStringByQuality();
			EmptyInformation.SetActive(false);
			DescBackground.gameObject.SetActive(true);
			DescBackground.spriteName = m_ObserveItem.GetDescBackGroundColorStringByQuality();
			ItemPreview.enabled = true;
			ItemPreview.spriteName = m_ObserveItem.previewIconName;
			ItemPreview.MakePixelPerfect();
			ItemPreview.transform.localScale *= 1.2f;
			if (MagAndPriceLabels != null)
			{
				NGUITools.SetActive(MagAndPriceLabels, true);
			}
			if (m_ObserveItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Weapon || m_ObserveItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.WeaponG)
			{
				BulletIcon.enabled = true;
				BulletIcon.spriteName = GetBulletIconNameByItemClass(m_ObserveItem.ItemClass);
				float x = NGUIMath.CalculateRelativeWidgetBounds(Mag.transform).size.x;
				BulletIcon.transform.localPosition = new Vector3(Mag.transform.localPosition.x - x / 2f - BulletIcon.transform.localScale.x, BulletIcon.transform.localPosition.y, BulletIcon.transform.localPosition.z);
			}
			else
			{
				BulletIcon.enabled = false;
			}
			UpdateElementIcons();
			if (m_ObserveItem.Quality != ItemQuality.Legendary && m_ObserveItem.Quality != ItemQuality.Epic && QualityEffectObject != null)
			{
				Object.Destroy(QualityEffectObject);
				QualityEffectObject = null;
			}
			if (QualityEffectObject == null)
			{
				if (m_ObserveItem.Quality == ItemQuality.Legendary)
				{
					GameObject original = Resources.Load("RPG_effect/RPG_UI_Orange002") as GameObject;
					QualityEffectObject = Object.Instantiate(original) as GameObject;
					QualityEffectObject.transform.parent = base.transform;
					QualityEffectObject.transform.localPosition = QualityColor.transform.localPosition + Vector3.forward * -100f;
					QualityEffectObject.transform.localScale = new Vector3(103f, 110f, 1f);
				}
				else if (m_ObserveItem.Quality == ItemQuality.Epic)
				{
					GameObject original2 = Resources.Load("RPG_effect/RPG_UI_Purple002") as GameObject;
					QualityEffectObject = Object.Instantiate(original2) as GameObject;
					QualityEffectObject.transform.parent = base.transform;
					QualityEffectObject.transform.localPosition = QualityColor.transform.localPosition + Vector3.forward * -100f;
					QualityEffectObject.transform.localScale = new Vector3(103f, 110f, 1f);
				}
			}
			if (UpgradeInfoObject != null)
			{
				if (m_ObserveItem.GetMaxUpgradeCount() > 0)
				{
					UpgradeInfoObject.gameObject.SetActive(true);
					int maxUpgradeCount = m_ObserveItem.GetMaxUpgradeCount();
					string text = maxUpgradeCount.ToString();
					if (maxUpgradeCount >= 99)
					{
						text = "/max";
					}
					UpgradeInfoObject.UpgradeInfo.text = "[ffeea0]" + m_ObserveItem.UpgradeTimes + "/" + text + "[-]";
				}
				else
				{
					UpgradeInfoObject.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			ClearDescription();
		}
		if (SpecialSell != null)
		{
			if (SpecialSell.ShopItem != null)
			{
				SpecialSellName.text = SpecialSell.ShopItem.baseItem.GetStringColorPrefixByQuality() + SpecialSell.ShopItem.baseItem.DisplayName + "[-]";
			}
			else
			{
				SpecialSellName.text = string.Empty;
			}
		}
	}

	public void ClearDescription()
	{
		Name.text = string.Empty;
		Level.text = string.Empty;
		LevelBackground.SetActive(false);
		NameBackground.SetActive(false);
		Mag.text = string.Empty;
		if (Price != null)
		{
			Price.text = string.Empty;
		}
		DescriptionLabel.text = string.Empty;
		DescriptionBackground.gameObject.SetActive(false);
		CompanyLogo.enabled = false;
		QualityColor.enabled = false;
		ItemPreview.enabled = false;
		UpgradeInfoObject.gameObject.SetActive(false);
		if (MagAndPriceLabels != null)
		{
			NGUITools.SetActive(MagAndPriceLabels, false);
		}
		BulletIcon.enabled = false;
		EmptyInformation.SetActive(true);
		DescBackground.gameObject.SetActive(false);
		FireElementIcon.SetActive(false);
		ShockElementIcon.SetActive(false);
		CorrosiveElementIcon.SetActive(false);
		ExplosiveElementIcon.SetActive(false);
		if (QualityEffectObject != null)
		{
			Object.Destroy(QualityEffectObject);
			QualityEffectObject = null;
		}
	}

	public void SetObserveItem(NGUIBaseItem item)
	{
		if (!IsForUpgrade && (!IsForShop || !(ShopUIScript.mInstance != null) || ShopUIScript.mInstance.CurrentPage == ShopPageType.Sell))
		{
			if (item != null && !base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			if (item == null)
			{
				m_LastObserveItem = null;
				base.gameObject.SetActive(false);
				if (NGUIBackPackUIScript.mInstance != null)
				{
					int childCount = NGUIBackPackUIScript.mInstance.mBackPack.transform.GetChildCount();
					for (int i = 0; i < childCount; i++)
					{
						Transform child = NGUIBackPackUIScript.mInstance.mBackPack.transform.GetChild(i);
						for (int j = 0; j < child.GetChildCount(); j++)
						{
							if (child.GetChild(j).tag == TagName.QUALITY_EFFECT)
							{
								child.GetChild(j).gameObject.SetActive(true);
							}
						}
					}
				}
				if (!(ShopSellPageScript.mInstance != null))
				{
					return;
				}
				int childCount2 = ShopSellPageScript.mInstance.m_BackPack.transform.GetChildCount();
				for (int k = 0; k < childCount2; k++)
				{
					Transform child2 = ShopSellPageScript.mInstance.m_BackPack.transform.GetChild(k);
					for (int l = 0; l < child2.GetChildCount(); l++)
					{
						if (child2.GetChild(l).tag == TagName.QUALITY_EFFECT)
						{
							child2.GetChild(l).gameObject.SetActive(true);
						}
					}
				}
				return;
			}
		}
		LogForItemScore(item);
		bool flag = item == null || item != m_ObserveItem;
		m_ObserveItem = item;
		UILabelRolling component = Name.gameObject.GetComponent<UILabelRolling>();
		if (component != null)
		{
			component.ResetLabelPosition();
		}
		if (SpecialSellName != null)
		{
			UILabelRolling component2 = SpecialSellName.gameObject.GetComponent<UILabelRolling>();
			if (component2 != null)
			{
				component2.ResetLabelPosition();
			}
		}
		if (flag)
		{
			FireElementIcon.SetActive(false);
			ShockElementIcon.SetActive(false);
			CorrosiveElementIcon.SetActive(false);
			ExplosiveElementIcon.SetActive(false);
		}
		if (ItemStats != null)
		{
			ItemStats.SetObserveItem(item);
		}
	}

	private string GetBulletIconNameByItemClass(ItemClasses itemClass)
	{
		string result = string.Empty;
		switch (itemClass)
		{
		case ItemClasses.SubmachineGun:
			result = "b_SMG";
			break;
		case ItemClasses.AssultRifle:
			result = "b_assault";
			break;
		case ItemClasses.Pistol:
			result = "b_pistol";
			break;
		case ItemClasses.Revolver:
			result = "b_revolver";
			break;
		case ItemClasses.Shotgun:
			result = "b_shotgun";
			break;
		case ItemClasses.Sniper:
			result = "b_sniper";
			break;
		case ItemClasses.RPG:
			result = "b_RPG";
			break;
		case ItemClasses.Grenade:
			result = "dilei";
			break;
		}
		return result;
	}

	public static string GetCompanyLogoString(ItemCompanyName company)
	{
		string result = string.Empty;
		switch (company)
		{
		case ItemCompanyName.D_haka:
			result = "HAKA";
			break;
		case ItemCompanyName.Mata:
			result = "MATA";
			break;
		case ItemCompanyName.Kypton:
			result = "KYPTON";
			break;
		case ItemCompanyName.Freyr:
			result = "REYR";
			break;
		case ItemCompanyName.Linp:
			result = "LINP";
			break;
		}
		return result;
	}

	private void UpdateElementIcons()
	{
		if (m_ObserveItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Weapon || m_ObserveItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.WeaponG)
		{
			if (m_ObserveItem.ItemClass == ItemClasses.RPG || m_ObserveItem.ItemClass == ItemClasses.Grenade)
			{
				ExplosiveElementIcon.SetActive(true);
			}
			if (m_ObserveItem.itemStats[11].statValue > 0f)
			{
				ExplosiveElementIcon.SetActive(false);
				FireElementIcon.SetActive(true);
			}
			if (m_ObserveItem.itemStats[12].statValue > 0f)
			{
				ExplosiveElementIcon.SetActive(false);
				ShockElementIcon.SetActive(true);
			}
			if (m_ObserveItem.itemStats[13].statValue > 0f)
			{
				ExplosiveElementIcon.SetActive(false);
				CorrosiveElementIcon.SetActive(true);
			}
		}
		else if (m_ObserveItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Shield)
		{
			if (m_ObserveItem.itemStats[3].statValue > 0f)
			{
				FireElementIcon.SetActive(true);
			}
			if (m_ObserveItem.itemStats[4].statValue > 0f)
			{
				ShockElementIcon.SetActive(true);
			}
			if (m_ObserveItem.itemStats[5].statValue > 0f)
			{
				CorrosiveElementIcon.SetActive(true);
			}
		}
		if (ElementGrid != null)
		{
			ElementGrid.repositionNow = true;
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (IsThisObject(go, CloseBtnName))
		{
			SetObserveItem(null);
		}
	}

	public void LogForItemScore(NGUIBaseItem baseItem)
	{
		if (baseItem == null)
		{
			return;
		}
		int num = 0;
		foreach (short skillID in baseItem.skillIDs)
		{
			if (GameConfig.GetInstance().equipPrefixConfig.ContainsKey(skillID))
			{
				EquipPrefixConfig equipPrefixConfig = GameConfig.GetInstance().equipPrefixConfig[skillID];
				num += equipPrefixConfig.Score;
			}
		}
	}
}

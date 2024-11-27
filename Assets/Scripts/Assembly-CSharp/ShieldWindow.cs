using UnityEngine;

public class ShieldWindow : ItemPopMenuWindow
{
	public UILabel Name;

	public UILabel Level;

	public UILabel Price;

	public UILabel Capacity;

	public UILabel RecoverySpeed;

	public UILabel RecoveryDelay;

	public UISprite ItemCompany;

	public UISlicedSprite QualityColor;

	public UISprite ItemPreview;

	public GameObject Label_Element;

	public GameObject Icon_Fire;

	public GameObject Icon_Thunder;

	public GameObject Icon_Toxic;

	public GameObject Icon_Explosive;

	public GameObject m_Normal;

	public GameObject m_BagFull;

	private NGUIBaseItem mNGUIBaseItem;

	private void OnEnable()
	{
		NGUITools.SetActive(Label_Element, false);
		NGUITools.SetActive(Icon_Fire, false);
		NGUITools.SetActive(Icon_Thunder, false);
		NGUITools.SetActive(Icon_Toxic, false);
		NGUITools.SetActive(Icon_Explosive, false);
		if (m_Normal != null)
		{
			NGUITools.SetActive(m_Normal, false);
		}
		if (m_BagFull != null)
		{
			NGUITools.SetActive(m_BagFull, false);
		}
	}

	protected override void OnSetNGUIBaseItem(NGUIBaseItem nguiBaseItem)
	{
		mNGUIBaseItem = nguiBaseItem;
		Name.text = AddItemQualityColor(nguiBaseItem, mNGUIBaseItem.DisplayName);
		if (Level != null)
		{
			Level.text = "Lv." + nguiBaseItem.ItemLevel;
			Level.color = GetItemLevelColor(nguiBaseItem.ItemLevel);
		}
		if (Price != null)
		{
			Price.text = string.Empty + mNGUIBaseItem.GetPrice();
		}
		if (ItemCompany != null)
		{
			ItemCompany.spriteName = ItemDescriptionScript.GetCompanyLogoString(mNGUIBaseItem.Company);
		}
		QualityColor.atlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		ItemPreview.atlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		QualityColor.spriteName = ConvertItemQualityToSpriteIconName(mNGUIBaseItem);
		ItemPreview.spriteName = mNGUIBaseItem.previewIconName;
		ItemPreview.MakePixelPerfect();
		ItemPreview.transform.localScale *= 1.2f;
		if (mNGUIBaseItem.itemStats[3].statValue > 0f)
		{
			NGUITools.SetActive(Icon_Fire, true);
			if (!Label_Element.activeSelf)
			{
				NGUITools.SetActive(Label_Element, true);
			}
		}
		if (mNGUIBaseItem.itemStats[4].statValue > 0f)
		{
			NGUITools.SetActive(Icon_Thunder, true);
			if (!Label_Element.activeSelf)
			{
				NGUITools.SetActive(Label_Element, true);
			}
		}
		if (mNGUIBaseItem.itemStats[5].statValue > 0f)
		{
			NGUITools.SetActive(Icon_Toxic, true);
			if (!Label_Element.activeSelf)
			{
				NGUITools.SetActive(Label_Element, true);
			}
		}
		if (!GameApp.GetInstance().GetUserState().ItemInfoData.CanPickUpItem(nguiBaseItem))
		{
			if (m_BagFull != null)
			{
				NGUITools.SetActive(m_BagFull, true);
			}
		}
		else if (m_Normal != null)
		{
			NGUITools.SetActive(m_Normal, true);
		}
	}

	private void Update()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		string origString = string.Empty + (int)mNGUIBaseItem.itemStats[0].statValue;
		origString = CompareValue(origString, localPlayer.GetShieldCapacity(), mNGUIBaseItem.itemStats[0].statValue, false);
		Capacity.text = origString;
		string origString2 = string.Empty + (int)mNGUIBaseItem.itemStats[1].statValue;
		origString2 = CompareValue(origString2, localPlayer.GetShieldRecovery(), mNGUIBaseItem.itemStats[1].statValue, false);
		RecoverySpeed.text = origString2;
		string origString3 = string.Empty + (int)mNGUIBaseItem.itemStats[2].statValue;
		origString3 = CompareValue(origString3, localPlayer.GetShieldRecoverDelay(), mNGUIBaseItem.itemStats[2].statValue, true);
		RecoveryDelay.text = origString3;
	}
}

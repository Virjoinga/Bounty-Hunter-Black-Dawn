using UnityEngine;

public class GrenadeWindow : ItemPopMenuWindow
{
	public UILabel Name;

	public UILabel Level;

	public UILabel Price;

	public UILabel Attack;

	public UILabel Range;

	public UILabel Bullet;

	public UISprite ItemCompany;

	public UISlicedSprite QualityColor;

	public UISprite ItemPreview;

	public GameObject Icon_Fire;

	public GameObject Icon_Thunder;

	public GameObject Icon_Toxic;

	public GameObject Icon_Explosive;

	public GameObject m_Normal;

	public GameObject m_BagFull;

	private NGUIBaseItem mNGUIBaseItem;

	private void OnEnable()
	{
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
		if (mNGUIBaseItem.itemStats[11].statValue > 0f)
		{
			NGUITools.SetActive(Icon_Fire, true);
		}
		if (mNGUIBaseItem.itemStats[12].statValue > 0f)
		{
			NGUITools.SetActive(Icon_Thunder, true);
		}
		if (mNGUIBaseItem.itemStats[13].statValue > 0f)
		{
			NGUITools.SetActive(Icon_Toxic, true);
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
		GrenadeInfo handGrenade = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.HandGrenade;
		int num = ((handGrenade != null) ? handGrenade.DamageInit : 0);
		int num2 = ((handGrenade != null) ? ((int)handGrenade.ExplosionRangeInit) : 0);
		int num3 = (int)mNGUIBaseItem.itemStats[0].statValue;
		int num4 = (int)mNGUIBaseItem.itemStats[10].statValue;
		string origString = string.Empty + num3;
		origString = CompareValue(origString, num, num3, false);
		Attack.text = origString;
		string origString2 = string.Empty + num4;
		origString2 = CompareValue(origString2, num2, num4, false);
		Range.text = origString2;
		Bullet.text = string.Empty + GameApp.GetInstance().GetUserState().GetBulletByWeaponType((WeaponType)mNGUIBaseItem.ItemClass);
	}
}

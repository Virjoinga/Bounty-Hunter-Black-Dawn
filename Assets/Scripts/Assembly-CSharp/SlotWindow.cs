using UnityEngine;

public class SlotWindow : ItemPopMenuWindow
{
	public UILabel Name;

	public UILabel Level;

	public UILabel Price;

	public UILabel Description;

	public UISprite ItemCompany;

	public UISlicedSprite QualityColor;

	public UISprite ItemPreview;

	public GameObject m_Normal;

	public GameObject m_BagFull;

	private void OnEnable()
	{
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
		Name.text = AddItemQualityColor(nguiBaseItem, nguiBaseItem.DisplayName);
		if (Level != null)
		{
			Level.text = "Lv." + nguiBaseItem.ItemLevel;
			Level.color = GetItemLevelColor(nguiBaseItem.ItemLevel);
		}
		if (Price != null)
		{
			Price.text = string.Empty + nguiBaseItem.GetPrice();
		}
		if (ItemCompany != null)
		{
			ItemCompany.spriteName = ItemDescriptionScript.GetCompanyLogoString(nguiBaseItem.Company);
		}
		Description.text = nguiBaseItem.Description;
		QualityColor.atlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		ItemPreview.atlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		QualityColor.spriteName = ConvertItemQualityToSpriteIconName(nguiBaseItem);
		ItemPreview.spriteName = nguiBaseItem.previewIconName;
		ItemPreview.MakePixelPerfect();
		ItemPreview.transform.localScale *= 1.2f;
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
}

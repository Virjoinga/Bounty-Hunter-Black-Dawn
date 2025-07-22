using UnityEngine;

public class PillWindow : ItemPopMenuWindow
{
	public UILabel Name;

	public UILabel Level;

	public UILabel Price;

	public UILabel Description;

	public UISlicedSprite QualityColor;

	public UISprite ItemPreview;

	public GameObject m_Normal;

	public GameObject m_BagFull;

	private void OnEnable()
	{
		NGUITools.SetActive(m_Normal, false);
		NGUITools.SetActive(m_BagFull, false);
	}

	protected override void OnSetNGUIBaseItem(NGUIBaseItem nguiBaseItem)
	{
		Name.text = AddItemQualityColor(nguiBaseItem, nguiBaseItem.DisplayName);
		Level.text = "Lv." + nguiBaseItem.ItemLevel;
		Level.color = GetItemLevelColor(nguiBaseItem.ItemLevel);
		Price.text = string.Empty + nguiBaseItem.GetPrice();
		Description.text = nguiBaseItem.Description;
		QualityColor.atlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		ItemPreview.atlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		QualityColor.spriteName = ConvertItemQualityToSpriteIconName(nguiBaseItem);
		ItemPreview.spriteName = nguiBaseItem.previewIconName;
		ItemPreview.MakePixelPerfect();
		ItemPreview.transform.localScale *= 1.2f;
		if (!GameApp.GetInstance().GetUserState().ItemInfoData.BackPackIsFull())
		{
			NGUITools.SetActive(m_BagFull, true);
		}
		else
		{
			NGUITools.SetActive(m_Normal, true);
		}
	}
}

using UnityEngine;

public abstract class ItemPopMenuWindow : MonoBehaviour
{
	private ItemPopMenuEventListener mItemPopMenuEvent;

	public void SetEventListener(ItemPopMenuEventListener listener)
	{
		mItemPopMenuEvent = listener;
	}

	public void SetNGUIBaseItem(NGUIBaseItem nguiItemBase)
	{
		OnSetNGUIBaseItem(nguiItemBase);
	}

	protected abstract void OnSetNGUIBaseItem(NGUIBaseItem nguiBaseItem);

	protected string ConvertItemQualityToSpriteIconName(ItemBase itemBase)
	{
		return ConvertItemQualityToSpriteIconName(itemBase);
	}

	protected string ConvertItemQualityToSpriteIconName(NGUIBaseItem nguiBaseitem)
	{
		return nguiBaseitem.GetBackGroundColorStringByQuality();
	}

	protected string AddItemQualityColor(NGUIBaseItem nguiBaseitem, string str)
	{
		switch (nguiBaseitem.Quality)
		{
		case ItemQuality.Uncommon:
			return "[00ff12]" + str + "[-]";
		case ItemQuality.Rare:
			return "[00c6ff]" + str + "[-]";
		case ItemQuality.Epic:
			return "[f82eff]" + str + "[-]";
		case ItemQuality.Legendary:
			return "[ff9000]" + str + "[-]";
		default:
			return "[ffffff]" + str + "[-]";
		}
	}

	protected string CompareValue(string origString, float mine, float contrast, bool revert)
	{
		string text = origString;
		if (revert)
		{
			if (mine < contrast)
			{
				return "[ff0000]" + origString + "/down2[-]";
			}
			if (mine > contrast)
			{
				return "[00ff00]" + origString + "/up1[-]";
			}
			return "[ffffff]" + origString + "/right[-]";
		}
		if (mine > contrast)
		{
			return "[ff0000]" + origString + "/down2[-]";
		}
		if (mine < contrast)
		{
			return "[00ff00]" + origString + "/up1[-]";
		}
		return "[ffffff]" + origString + "/right[-]";
	}

	protected Color GetItemLevelColor(int itemLevel)
	{
		if (itemLevel > GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			return Color.red;
		}
		return new Color(0f, 0f, 0f);
	}

	private void OnClick()
	{
		if (mItemPopMenuEvent != null)
		{
			mItemPopMenuEvent.OnClickWindow();
		}
	}
}

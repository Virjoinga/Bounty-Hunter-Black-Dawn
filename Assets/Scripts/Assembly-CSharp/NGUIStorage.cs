using System.Collections.Generic;
using UnityEngine;

public class NGUIStorage : MonoBehaviour
{
	public int maxItemCount = 8;

	public int maxRows = 4;

	public int maxColumns = 4;

	public GameObject template;

	public UIWidget background;

	public int spacing = 128;

	public int padding = 10;

	private List<NGUIGameItem> mItems = new List<NGUIGameItem>();

	public List<NGUIGameItem> items
	{
		get
		{
			while (mItems.Count < maxItemCount)
			{
				mItems.Add(null);
			}
			return mItems;
		}
	}

	public NGUIGameItem GetItem(int slot)
	{
		return (slot >= items.Count) ? null : mItems[slot];
	}

	public NGUIGameItem Replace(int slot, NGUIGameItem item)
	{
		if (slot < maxItemCount)
		{
			NGUIGameItem result = items[slot];
			mItems[slot] = item;
			return result;
		}
		return item;
	}

	private void Start()
	{
		if (!(template != null))
		{
			return;
		}
		Transform parent = base.transform;
		int num = 0;
		Bounds bounds = default(Bounds);
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		int num2 = 0;
		for (int i = 0; i < maxColumns; i++)
		{
			for (int j = 0; j < maxRows; j++)
			{
				GameObject gameObject = Object.Instantiate(template) as GameObject;
				NGUITools.SetActive(gameObject, true);
				Transform transform = gameObject.transform;
				transform.parent = parent;
				transform.localPosition = new Vector3((float)padding + ((float)i + 0.5f) * (float)spacing, (float)(-padding) - ((float)j + 0.5f) * (float)spacing, 0f);
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				NGUIItemStorageSlot component = gameObject.GetComponent<NGUIItemStorageSlot>();
				if (component != null)
				{
					component.storage = this;
					component.slot = num;
				}
				bounds.Encapsulate(new Vector3((float)padding * 2f + (float)((i + 1) * spacing), (float)(-padding) * 2f - (float)((j + 1) * spacing), 0f));
				if (num2 >= maxItemCount)
				{
					gameObject.GetComponent<Collider>().enabled = false;
				}
				num2++;
				if (++num >= maxItemCount + (2 - itemInfoData.Bag_Extend_Time) * maxRows)
				{
					if (background != null)
					{
						background.transform.localScale = bounds.size;
					}
					return;
				}
			}
		}
		if (background != null)
		{
			background.transform.localScale = bounds.size;
		}
	}

	public void ClearItem()
	{
		items.Clear();
	}

	public virtual void Refresh()
	{
		int childCount = base.transform.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Object.Destroy(base.transform.GetChild(i).gameObject);
		}
		Start();
	}
}

using UnityEngine;

public class ShopItemStorageScript : NGUIStorage
{
	public ShopListType ShopType;

	private void Start()
	{
		RefreshStorage();
		UIGridX uIGridX = NGUITools.FindInParents<UIGridX>(base.gameObject);
		if (uIGridX != null)
		{
			uIGridX.repositionNow = true;
		}
	}

	public void RefreshStorage()
	{
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		if (!(template != null))
		{
			return;
		}
		Transform parent = base.transform;
		int num = 0;
		Bounds bounds = default(Bounds);
		for (int i = 0; i < maxColumns; i++)
		{
			for (int j = 0; j < maxRows; j++)
			{
				GameObject gameObject = Object.Instantiate(template) as GameObject;
				NGUITools.SetActive(gameObject, true);
				Transform transform2 = gameObject.transform;
				transform2.parent = parent;
				transform2.localPosition = new Vector3((float)padding + ((float)i + 0.5f) * (float)spacing, (float)(-padding) - ((float)j + 0.5f) * (float)spacing, 0f);
				transform2.localRotation = Quaternion.identity;
				transform2.localScale = Vector3.one;
				ShopItemSlotScript component = gameObject.GetComponent<ShopItemSlotScript>();
				if (component != null)
				{
					component.ShopType = ShopType;
					component.ListID = (byte)(i * maxRows + j);
				}
				bounds.Encapsulate(new Vector3((float)padding * 2f + (float)((i + 1) * spacing), (float)(-padding) * 2f - (float)((j + 1) * spacing), 0f));
				if (++num >= maxItemCount)
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
}

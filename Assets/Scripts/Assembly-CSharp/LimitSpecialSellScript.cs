using UnityEngine;

public class LimitSpecialSellScript : MonoBehaviour
{
	public GameObject SellItem;

	public GameObject WaitingLabel;

	public bool IsForLimitSell;

	public GameObject OffPriceObject;

	public UILabel NextRoundLabel;

	private void Update()
	{
		NGUIGameItem nGUIGameItem = null;
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (IsForLimitSell)
		{
			nGUIGameItem = GameApp.GetInstance().GetUserState().ItemInfoData.Shop_LimitSell;
			empty = "[00f9ff]";
			empty2 = LocalizationManager.GetInstance().GetString("MSG_LIMITED_COMING_SOON");
		}
		else
		{
			nGUIGameItem = GameApp.GetInstance().GetUserState().ItemInfoData.Shop_SpecialSell;
			empty = "[ffdc04]";
			empty2 = LocalizationManager.GetInstance().GetString("MSG_SALE_ITEM_WAITING");
			if (NextRoundLabel != null)
			{
				if (nGUIGameItem != null)
				{
					NextRoundLabel.text = LocalizationManager.GetInstance().GetString("MSG_SALE_ITEM_EXPIRED");
				}
				else
				{
					NextRoundLabel.text = LocalizationManager.GetInstance().GetString("MSG_LIMITED_NEXT_ROUND");
				}
			}
		}
		if (nGUIGameItem != null)
		{
			if (!SellItem.activeSelf)
			{
				SellItem.SetActive(true);
				if (ShopUIScript.mInstance.CurrentPage == ShopPageType.Equip || ShopUIScript.mInstance.CurrentPage == ShopPageType.Pill)
				{
					ShopUIScript.mInstance.ChangePage(ShopUIScript.mInstance.CurrentPage);
				}
			}
			if (OffPriceObject != null && !OffPriceObject.activeSelf)
			{
				OffPriceObject.SetActive(true);
				UILabel component = OffPriceObject.transform.Find("sell").GetComponent<UILabel>();
				if (component != null)
				{
					component.text = GameApp.GetInstance().GetUserState().ItemInfoData.SpecialOff + "%";
				}
			}
			if (WaitingLabel.activeSelf)
			{
				WaitingLabel.SetActive(false);
			}
		}
		else
		{
			if (SellItem.activeSelf)
			{
				SellItem.SetActive(false);
			}
			if (OffPriceObject != null && OffPriceObject.activeSelf)
			{
				OffPriceObject.SetActive(false);
			}
			if (!WaitingLabel.activeSelf)
			{
				WaitingLabel.SetActive(true);
			}
			WaitingLabel.GetComponent<UILabel>().text = empty + empty2 + "[-]";
		}
	}
}

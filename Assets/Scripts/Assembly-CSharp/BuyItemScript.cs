using UnityEngine;

public class BuyItemScript : MonoBehaviour
{
	private void OnClick()
	{
		bool isBagFull = false;
		if (GameApp.GetInstance().GetUserState().ItemInfoData.BackPackIsFull())
		{
			Debug.Log("Bag is full!");
			isBagFull = true;
		}
		if (ShopUIScript.mInstance.SelectedItem != null && ShopUIScript.mInstance.SelectedItem.ShopItem != null && ShopUIScript.mInstance.SelectedItem.CheckBeforeBuy())
		{
			ShopUIScript.mInstance.SelectedItem.AfterBuy(isBagFull);
		}
	}
}

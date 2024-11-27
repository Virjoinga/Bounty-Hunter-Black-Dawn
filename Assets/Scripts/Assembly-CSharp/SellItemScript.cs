using UnityEngine;

public class SellItemScript : MonoBehaviour
{
	private void OnClick()
	{
		if (ShopUIScript.mInstance.SelectedItem != null && ShopUIScript.mInstance.SelectedItem.ShopItem != null)
		{
			ShopUIScript.mInstance.SelectedItem.AfterSell();
		}
	}
}

using UnityEngine;

public class ShopChangePageScript : MonoBehaviour
{
	public ShopPageType Page;

	private void OnClick()
	{
		if (ShopUIScript.mInstance != null && Page != ShopUIScript.mInstance.CurrentPage)
		{
			ShopUIScript.mInstance.ChangePage(Page);
		}
	}
}

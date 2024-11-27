using UnityEngine;

public class ScrollShopListScript : MonoBehaviour
{
	public UIDraggablePanelAlign ShopList;

	public ScrollDirection Direction;

	private void OnClick()
	{
		if (ShopList != null)
		{
			switch (Direction)
			{
			case ScrollDirection.Up:
				ShopList.MoveToPrevIndexAlign();
				break;
			case ScrollDirection.Down:
				ShopList.MoveToNextIndexAlign();
				break;
			}
		}
	}
}

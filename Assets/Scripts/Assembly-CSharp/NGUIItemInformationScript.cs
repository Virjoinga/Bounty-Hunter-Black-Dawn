using System;

public class NGUIItemInformationScript : NGUIBackPackObject, ItemPopMenuEventListener
{
	protected bool isCanPickUp = true;

	private NGUIBaseItem lastNGUIBaseItem;

	private void OnEnable()
	{
		ItemBase nearestItem = UserStateHUD.GetInstance().NearestItem;
		if (nearestItem != null && nearestItem.ItemType != 7 && !GameApp.GetInstance().GetUserState().ItemInfoData.CanPickUpItem(nearestItem.mNGUIBaseItem))
		{
			isCanPickUp = false;
		}
		else
		{
			isCanPickUp = true;
		}
		if (nearestItem != null)
		{
			ItemPopMenu.instance.Show(nearestItem.mNGUIBaseItem, false, this);
			lastNGUIBaseItem = nearestItem.mNGUIBaseItem;
		}
	}

	private void Update()
	{
		ItemBase nearestItem = UserStateHUD.GetInstance().NearestItem;
		if (nearestItem != null && nearestItem.mNGUIBaseItem != lastNGUIBaseItem)
		{
			ItemPopMenu.instance.Refresh(nearestItem.mNGUIBaseItem);
			lastNGUIBaseItem = nearestItem.mNGUIBaseItem;
		}
	}

	private void OnDisable()
	{
		if (ItemPopMenu.instance != null)
		{
			ItemPopMenu.instance.Close();
		}
		lastNGUIBaseItem = null;
	}

	public void OnClickWindow()
	{
		ItemBase nearestItem = UserStateHUD.GetInstance().NearestItem;
		if (nearestItem != null)
		{
			if (isCanPickUp)
			{
				if (nearestItem.ItemType == 7 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					string[] array = nearestItem.gameObject.name.Split('_');
					short sequenceID = Convert.ToInt16(array[array.Length - 1]);
					PickUpItemRequest request = new PickUpItemRequest(sequenceID);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				else
				{
					nearestItem.PickUpItem();
				}
				UserStateHUD.GetInstance().NearestItem = null;
			}
			else
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_BAG_FULL_BRIEF"));
			}
		}
		NGUITools.SetActive(base.gameObject, false);
	}
}

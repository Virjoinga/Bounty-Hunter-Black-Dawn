using System;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
	public GameObject OpenBoxButton;

	public GameObject SearchBoxButton;

	public GameObject SearchItemButton;

	public NGUIItemInformationScript ItemInformation;

	private void Awake()
	{
		UIEventListener uIEventListener = UIEventListener.Get(OpenBoxButton);
		UIEventListener uIEventListener2 = uIEventListener;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickThumb));
		uIEventListener = UIEventListener.Get(SearchBoxButton);
		UIEventListener uIEventListener3 = uIEventListener;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickThumb));
		uIEventListener = UIEventListener.Get(SearchItemButton);
		UIEventListener uIEventListener4 = uIEventListener;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickThumb));
	}

	private void OnEnable()
	{
		NGUITools.SetActive(OpenBoxButton, false);
		NGUITools.SetActive(SearchBoxButton, false);
		NGUITools.SetActive(SearchItemButton, false);
	}

	private void Update()
	{
		ItemBase nearestItem = UserStateHUD.GetInstance().NearestItem;
		ChestScript nearestChest = UserStateHUD.GetInstance().NearestChest;
		if (nearestItem != null)
		{
			NGUITools.SetActive(ItemInformation.gameObject, true);
		}
		else
		{
			NGUITools.SetActive(ItemInformation.gameObject, false);
		}
		if (nearestChest != null && !nearestChest.IsAlreadyOpen)
		{
			List<short> requestItemIDs = nearestChest.GetRequestItemIDs();
			bool flag = false;
			if (!flag)
			{
				if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckHasBeenAccepted(nearestChest.mPrevQuestId))
				{
					NGUITools.SetActive(SearchBoxButton, true);
					return;
				}
				if (nearestChest.mPrevQuestId == 0 && nearestChest.mQuestCommonId == 0)
				{
					flag = true;
				}
				else if (nearestChest.mPrevQuestId == 0 || GameApp.GetInstance().GetUserState().m_questStateContainer.CheckSubQuestCompleted(nearestChest.mPrevQuestId))
				{
					for (int i = 0; i < requestItemIDs.Count; i++)
					{
						if (GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailable(requestItemIDs[i]))
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				if (nearestChest.mChestType == ChestType.Box)
				{
					NGUITools.SetActive(OpenBoxButton, true);
				}
				else
				{
					NGUITools.SetActive(SearchItemButton, true);
				}
			}
			else
			{
				NGUITools.SetActive(OpenBoxButton, false);
				NGUITools.SetActive(SearchBoxButton, false);
				NGUITools.SetActive(SearchItemButton, false);
			}
		}
		else
		{
			NGUITools.SetActive(OpenBoxButton, false);
			NGUITools.SetActive(SearchBoxButton, false);
			NGUITools.SetActive(SearchItemButton, false);
		}
	}

	private void OnClickThumb(GameObject go)
	{
		if (go.Equals(OpenBoxButton) || go.Equals(SearchItemButton))
		{
			ChestScript nearestChest = UserStateHUD.GetInstance().NearestChest;
			if (nearestChest != null)
			{
				nearestChest.OnLoot();
				UserStateHUD.GetInstance().NearestChest = null;
			}
			NGUITools.SetActive(go, false);
		}
		else
		{
			if (!go.Equals(SearchBoxButton))
			{
				return;
			}
			ChestScript nearestChest2 = UserStateHUD.GetInstance().NearestChest;
			if (nearestChest2 != null && nearestChest2.mSearchItemId != 0)
			{
				GameApp.GetInstance().GetUserState().ItemInfoData.AddStoryItem((short)nearestChest2.mSearchItemId);
				GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressItemCollection((short)nearestChest2.mSearchItemId);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					PickUpQuestItemRequest request = new PickUpQuestItemRequest((short)nearestChest2.mSearchItemId);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
	}
}

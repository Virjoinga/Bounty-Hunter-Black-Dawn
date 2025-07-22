using System;
using System.Collections.Generic;
using UnityEngine;

public class UIGamble : UIGameMenu, ItemPopMenuEventListener, UIMsgListener, UIRouletteListener
{
	private const float TIME_TO_SHOW_CONGRATULATIONS = 1500f;

	private const float TIME_TO_SHOW_LIGHT = 1000f;

	public GameObject m_ResetButton;

	public UISprite[] rouletteWeaponIconList;

	public Transform[] weaponIconList;

	public UIRoulette m_UIRoulette;

	public GameObject m_Light;

	public GameObject m_Quality;

	public UISprite m_Color;

	public GameObject m_Congratulations;

	public GameObject m_Mask;

	private NGUIBaseItem mNGUIBaseItem;

	private DateTime curTimeToShowCongratulations;

	private DateTime curTimeToShowLight;

	private bool bReadyToShowLight;

	private bool bReadyToShowCongratulations;

	private bool bLockRotate;

	private bool bHoverReset;

	private static byte prevPhase;

	private static bool isShow;

	private DateTime mLastUpdateTime;

	protected override void Awake()
	{
		base.Awake();
		m_Light.SetActive(false);
		m_Quality.SetActive(false);
		m_Mask.SetActive(false);
		m_Congratulations.SetActive(false);
		RefreshWindow();
		AddDelegate(m_ResetButton);
		m_UIRoulette.SetListener(this);
		bHoverReset = false;
		bLockRotate = false;
		bReadyToShowCongratulations = false;
	}

	private void Update()
	{
		if (GambleManagerAbandoned.GetInstance().IsUsing())
		{
			if (m_ResetButton.GetComponent<Collider>().enabled)
			{
				m_ResetButton.GetComponent<Collider>().enabled = false;
			}
		}
		else if (!m_ResetButton.GetComponent<Collider>().enabled)
		{
			m_ResetButton.GetComponent<Collider>().enabled = true;
		}
		if ((DateTime.Now - mLastUpdateTime).TotalMilliseconds > 200.0)
		{
			mLastUpdateTime = DateTime.Now;
			if (GambleManagerAbandoned.GetInstance().IsUsing())
			{
				m_Mask.SetActive(false);
			}
			else
			{
				m_Mask.SetActive(true);
			}
		}
		if ((DateTime.Now - curTimeToShowLight).TotalMilliseconds > 1000.0 && bReadyToShowLight)
		{
			ShowLight();
			bReadyToShowLight = false;
			bReadyToShowCongratulations = true;
			curTimeToShowCongratulations = DateTime.Now;
		}
		if ((DateTime.Now - curTimeToShowCongratulations).TotalMilliseconds > 1500.0 && bReadyToShowCongratulations)
		{
			ShowCongratulations();
			bReadyToShowCongratulations = false;
		}
	}

	private void RefreshWindow()
	{
		DeleteAllIcon();
		List<GambleItemAbility> gambleItemList = GambleManagerAbandoned.GetInstance().GetGambleItemList();
		CreateAllIcon(gambleItemList);
		RefreshRoulette(gambleItemList);
	}

	private void RefreshRoulette(List<GambleItemAbility> list)
	{
		for (int i = 0; i < rouletteWeaponIconList.Length; i++)
		{
			rouletteWeaponIconList[i].atlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
			rouletteWeaponIconList[i].spriteName = list[i % list.Count].BigIconName;
			rouletteWeaponIconList[i].MakePixelPerfect();
			rouletteWeaponIconList[i].transform.transform.localScale *= 1.2f;
		}
	}

	private void CreateAllIcon(List<GambleItemAbility> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			CreateSmallIcon(weaponIconList[i], list[i]);
		}
	}

	private void CreateSmallIcon(Transform weaponIcon, GambleItemAbility ability)
	{
		GameObject gameObject = GameApp.GetInstance().GetLootManager().CreateIcon(ability.Quality, ability.SmallIconName);
		gameObject.transform.parent = weaponIcon.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
	}

	private void DeleteAllIcon()
	{
		Transform[] array = weaponIconList;
		foreach (Transform weaponIcon in array)
		{
			DeleteSmallIcon(weaponIcon);
		}
	}

	private void DeleteSmallIcon(Transform weaponIcon)
	{
		ItemIcon componentInChildren = weaponIcon.GetComponentInChildren<ItemIcon>();
		if (componentInChildren != null)
		{
			UnityEngine.Object.Destroy(componentInChildren.gameObject);
		}
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailable(10502))
		{
			GameApp.GetInstance().GetUserState().ItemInfoData.AddStoryItem(10502);
			GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressItemCollection(10502);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PickUpQuestItemRequest request = new PickUpQuestItemRequest(10502);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public static void Show()
	{
		if (!isShow)
		{
			prevPhase = 6;
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(34, false, false, true);
			isShow = true;
		}
	}

	public static bool IsShow()
	{
		return isShow;
	}

	public static void Close()
	{
		if (isShow)
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(prevPhase, false, false, true);
			isShow = false;
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GAMBLE_RESET").Replace("%d", string.Empty + 5), 3, 8);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 8)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(5))
				{
					UIMsgBox.instance.CloseMessage();
					GambleManagerAbandoned.GetInstance().ResetWhenClickButton();
					GambleManagerAbandoned.GetInstance().RefreshItemList();
					RefreshWindow();
					m_Light.SetActive(false);
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 9)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.IAP);
			}
		}
		else if (whichMsg.EventId == 30 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}

	public void OnRouletteStop(int index)
	{
		Debug.Log("OnRouletteStop index : " + index);
		ItemBase itemBase = GambleManagerAbandoned.GetInstance().CreateItemByRouletteIndex(index);
		mNGUIBaseItem = itemBase.mNGUIBaseItem;
		m_Color.color = itemBase.mNGUIBaseItem.GetBackGroundColorByQuality();
		if (itemBase.ItemCanBePickedUp())
		{
			itemBase.PickUpItem();
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_BAG_FULL"), 2, 30);
			GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(itemBase.mNGUIBaseItem, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition() + Vector3.up, Vector3.zero);
			UnityEngine.Object.Destroy(itemBase.gameObject);
		}
		itemBase = null;
		bReadyToShowLight = true;
		curTimeToShowLight = DateTime.Now;
	}

	private void ShowLight()
	{
		m_Light.SetActive(true);
		m_Quality.SetActive(true);
	}

	private void ShowCongratulations()
	{
		m_Congratulations.SetActive(true);
		m_Congratulations.GetComponentInChildren<UIGambleItem>().Show(mNGUIBaseItem, this);
	}

	public void OnClickWindow()
	{
		m_Congratulations.SetActive(false);
		m_Light.SetActive(false);
		m_Quality.SetActive(false);
		GambleManagerAbandoned.GetInstance().UseFinish();
		bLockRotate = false;
		InGameMenuManager.GetInstance().Lock = false;
	}

	public void OnRouletteStart()
	{
		bLockRotate = true;
		InGameMenuManager.GetInstance().Lock = true;
	}

	public bool IsRouletteCanBeTouchInThisPos(Vector2 pos)
	{
		if (!GambleManagerAbandoned.GetInstance().IsUsing() || bLockRotate || bHoverReset || Vector2.Distance(pos, new Vector2(m_UIRoulette.CenterX, m_UIRoulette.CenterY)) > m_UIRoulette.radius + 150f)
		{
			return false;
		}
		return true;
	}

	public void OnRouletteFirstTouchAfterRotation()
	{
	}

	protected override void OnHoverThumb(GameObject go, bool isOver)
	{
		bHoverReset = isOver;
	}
}

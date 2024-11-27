using System.Collections.Generic;
using UnityEngine;

public class InGameMenuManager
{
	public const int INDEX_BAG = 0;

	public const int INDEX_MAP = 1;

	public const int INDEX_QUEST = 2;

	public const int INDEX_SKILL = 3;

	public const int INDEX_ACHIEVEMENT = 4;

	public const int INDEX_OPTION = 5;

	public const int INDEX_SHOP = 99;

	private static InGameMenuManager Instance;

	private GameObject m_CloseButton;

	private GameObject m_StateBar;

	private GameObject m_InGameMenu;

	private bool isEveryShow;

	private List<InGameMenuListener> listener = new List<InGameMenuListener>();

	private int previousPhase;

	private bool isInClose;

	public bool Lock { get; set; }

	private InGameMenuManager()
	{
		Lock = false;
	}

	public static InGameMenuManager GetInstance()
	{
		if (Instance == null)
		{
			Instance = new InGameMenuManager();
		}
		return Instance;
	}

	public void Show(int index)
	{
		if (index == 99)
		{
			ShowMenuForShop();
			return;
		}
		InGameMenu.CurrentIndex = index;
		InitBeforeShow();
		ShowMenuFirst(InGameMenu.CurrentIndex);
	}

	public void ShowMenu()
	{
		InGameMenu.CurrentIndex = 0;
		InitBeforeShow();
		ShowMenuFirst(InGameMenu.CurrentIndex);
	}

	public void ShowMenuForShop()
	{
		InGameMenu.CurrentIndex = 0;
		InitBeforeShow();
		ShowMenu(99);
	}

	private void InitBeforeShow()
	{
		previousPhase = 6;
		isEveryShow = true;
	}

	public void ShowStateBar()
	{
		if (m_StateBar == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("InGameMenu", "StateBar");
			m_StateBar = Object.Instantiate(original) as GameObject;
		}
	}

	public void ShowCloseButton()
	{
		if (m_CloseButton == null)
		{
			GameObject original = Resources.Load("InGameMenu/CloseBtn/CloseButton") as GameObject;
			m_CloseButton = Object.Instantiate(original) as GameObject;
		}
	}

	public void HideHUD()
	{
		HideHUD(true);
	}

	public void HideHUD(bool pause)
	{
		if (!(HUDManager.instance == null) && HUDManager.instance.GetRunningHUDType() != 0)
		{
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			HUDManager.instance.gameObject.SetActive(false);
			GameApp.GetInstance().GetGameScene().UIPause(pause);
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer != null)
			{
				localPlayer.InputController.Block = true;
			}
		}
	}

	public void ShowHUD()
	{
		if (!(HUDManager.instance == null) && HUDManager.instance.GetRunningHUDType() != 0)
		{
			if (Camera.main != null)
			{
				Transform transform = Camera.main.transform.Find("Npc_Collision");
				transform.GetComponent<Collider>().enabled = false;
			}
			HUDManager.instance.gameObject.SetActive(true);
			if (GameApp.GetInstance().GetGameScene() != null)
			{
				GameApp.GetInstance().GetGameScene().UIPause(false);
			}
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer != null)
			{
				localPlayer.InputController.Block = false;
			}
		}
	}

	public void HideStateBarAfterShow()
	{
		if (m_StateBar != null && StateBar.instance != null)
		{
			StateBar.instance.HideStateBar();
		}
	}

	public void ShowStateBarAfterHide()
	{
		if (m_StateBar != null && StateBar.instance != null)
		{
			StateBar.instance.ShowStateBar();
		}
	}

	public void HideCloseButtonAfterShow()
	{
		if (m_CloseButton != null && CloseButton.instance != null)
		{
			CloseButton.instance.m_CloseButton.SetActiveRecursively(false);
		}
	}

	public void ShowCloseButtonAfterHide()
	{
		if (m_CloseButton != null && CloseButton.instance != null)
		{
			CloseButton.instance.m_CloseButton.SetActiveRecursively(true);
		}
	}

	public void HideGameMenuButtonAfterShow()
	{
		if (m_InGameMenu != null)
		{
			m_InGameMenu.SetActiveRecursively(false);
		}
	}

	public void ShowGameMenuButtonAfterHide()
	{
		if (m_InGameMenu != null)
		{
			m_InGameMenu.SetActiveRecursively(true);
		}
	}

	public void Close(bool phaseChange)
	{
		isInClose = true;
		foreach (InGameMenuListener item in listener)
		{
			item.OnCloseButtonClick();
		}
		RemoveListener();
		isInClose = false;
		if (phaseChange)
		{
			if (isEveryShow)
			{
				isEveryShow = false;
				GameApp.GetInstance().GetUIStateManager().FrGoToPhase(previousPhase, false, false, true);
			}
		}
		else
		{
			isEveryShow = false;
		}
	}

	public void Close()
	{
		Close(true);
	}

	public void ShowGameMenuButton()
	{
		if (m_InGameMenu == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("InGameMenu", "InGameMenu");
			m_InGameMenu = Object.Instantiate(original) as GameObject;
		}
	}

	public void ShowShopMenuButton()
	{
		if (m_InGameMenu == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("ShopUI", "InGameMenuForShop");
			m_InGameMenu = Object.Instantiate(original) as GameObject;
		}
	}

	public void CloseMenuButton()
	{
		if (m_InGameMenu != null)
		{
			MemoryManager.FreeNGUI(m_InGameMenu);
			m_InGameMenu = null;
		}
	}

	public void CloseStarBar()
	{
		if (m_StateBar != null)
		{
			MemoryManager.FreeNGUI(m_StateBar);
			m_StateBar = null;
		}
	}

	public void CloseCloseButton()
	{
		if (m_CloseButton != null)
		{
			MemoryManager.FreeNGUI(m_CloseButton);
			m_CloseButton = null;
		}
	}

	public void CloseInstantly()
	{
		CloseInstantly(true);
	}

	public void CloseInstantly(bool showHUD)
	{
		CloseStarBar();
		CloseCloseButton();
		CloseMenuButton();
		if (showHUD)
		{
			ShowHUD();
		}
	}

	private void ShowMenuFirst(int index)
	{
		switch (index)
		{
		case 0:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(22, false, true, true);
			break;
		case 3:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(23, false, true, true);
			break;
		case 4:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(26, false, true, true);
			break;
		case 2:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(24, false, true, true);
			break;
		case 1:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(25, false, true, true);
			break;
		case 5:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(15, false, true, true);
			break;
		case 99:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(28, false, true, false);
			break;
		}
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_browse");
	}

	public void ShowMenu(int index)
	{
		switch (index)
		{
		case 0:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(22, false, false, true);
			break;
		case 3:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(23, false, false, true);
			break;
		case 4:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(26, false, false, true);
			break;
		case 2:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(24, false, false, true);
			break;
		case 1:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(25, false, false, true);
			break;
		case 5:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(15, false, false, true);
			break;
		case 99:
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(28, false, false, true);
			break;
		}
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_browse");
	}

	public void SetListener(InGameMenuListener l)
	{
		if (l != null && !listener.Contains(l))
		{
			listener.Add(l);
		}
	}

	public void RemoveListener()
	{
		if (!isInClose)
		{
			listener.Clear();
		}
	}

	public void RemoveListener(InGameMenuListener l)
	{
		if (!isInClose && listener.Contains(l))
		{
			listener.Remove(l);
		}
	}
}

using UnityEngine;

public class ItemPopMenu : MonoBehaviour
{
	public enum WindowType
	{
		None = 0,
		Weapon = 1,
		Shield = 2,
		Pill = 3,
		Grenade = 4,
		QuestItem = 5,
		Slot = 6
	}

	public static ItemPopMenu instance;

	public ItemPopMenuWindow m_WeaponWindow;

	public ItemPopMenuWindow m_ShieldWindow;

	public ItemPopMenuWindow m_PillWindow;

	public ItemPopMenuWindow m_GrenadeWindow;

	public ItemPopMenuWindow m_QuestItemWindow;

	public ItemPopMenuWindow m_SlotWindow;

	public GameObject m_Collider;

	public GameObject m_Panel;

	public GameObject m_Anchor;

	public GameObject m_Camera;

	private bool isColliderOn;

	private ItemPopMenuEventListener mItemPopMenuEventListener;

	private ItemPopMenuWindow m_CurrWindow;

	private bool isShow;

	private void Awake()
	{
		isShow = false;
		instance = this;
		CloseAll();
		InactivateSelf();
	}

	private void CloseAll()
	{
		NGUITools.SetActive(m_WeaponWindow.gameObject, false);
		NGUITools.SetActive(m_GrenadeWindow.gameObject, false);
		NGUITools.SetActive(m_QuestItemWindow.gameObject, false);
		NGUITools.SetActive(m_ShieldWindow.gameObject, false);
		NGUITools.SetActive(m_PillWindow.gameObject, false);
		NGUITools.SetActive(m_SlotWindow.gameObject, false);
		NGUITools.SetActive(m_Collider.gameObject, false);
	}

	private void Show(NGUIBaseItem nguiBaseItem, WindowType type)
	{
		ActivateSelf();
		switch (type)
		{
		case WindowType.Weapon:
			m_CurrWindow = m_WeaponWindow;
			break;
		case WindowType.QuestItem:
			m_CurrWindow = m_QuestItemWindow;
			break;
		case WindowType.Pill:
			m_CurrWindow = m_PillWindow;
			break;
		case WindowType.Shield:
			m_CurrWindow = m_ShieldWindow;
			break;
		case WindowType.Grenade:
			m_CurrWindow = m_GrenadeWindow;
			break;
		case WindowType.Slot:
			m_CurrWindow = m_SlotWindow;
			break;
		}
		m_CurrWindow.gameObject.SetActiveRecursively(true);
		m_CurrWindow.SetNGUIBaseItem(nguiBaseItem);
		m_CurrWindow.SetEventListener(mItemPopMenuEventListener);
		NGUITools.SetActive(m_Collider.gameObject, isColliderOn);
	}

	public void Show(NGUIBaseItem nguiBaseItem, bool touchOuterToClose, ItemPopMenuEventListener itemPopMenuEventListener)
	{
		WindowType windowType = GetWindowType(nguiBaseItem);
		if (!isShow && windowType != 0)
		{
			isShow = true;
			isColliderOn = touchOuterToClose;
			mItemPopMenuEventListener = itemPopMenuEventListener;
			Show(nguiBaseItem, windowType);
		}
	}

	public void Show(NGUIBaseItem nguiBaseItem, bool touchOuterToClose)
	{
		Show(nguiBaseItem, touchOuterToClose, null);
	}

	private WindowType GetWindowType(NGUIBaseItem nguiBaseItem)
	{
		if (nguiBaseItem.ItemClass == ItemClasses.StoryItem)
		{
			return WindowType.QuestItem;
		}
		if (nguiBaseItem.ItemClass == ItemClasses.W_Pills)
		{
			return WindowType.Pill;
		}
		if (nguiBaseItem.ItemClass == ItemClasses.U_Shield)
		{
			return WindowType.Shield;
		}
		if (nguiBaseItem.ItemClass == ItemClasses.Grenade)
		{
			return WindowType.Grenade;
		}
		if (nguiBaseItem.ItemClass == ItemClasses.V_Slot)
		{
			return WindowType.Slot;
		}
		if (nguiBaseItem.ItemClass == ItemClasses.AssultRifle || nguiBaseItem.ItemClass == ItemClasses.SubmachineGun || nguiBaseItem.ItemClass == ItemClasses.Pistol || nguiBaseItem.ItemClass == ItemClasses.Revolver || nguiBaseItem.ItemClass == ItemClasses.Shotgun || nguiBaseItem.ItemClass == ItemClasses.Sniper || nguiBaseItem.ItemClass == ItemClasses.RPG)
		{
			return WindowType.Weapon;
		}
		return WindowType.None;
	}

	public void Refresh(NGUIBaseItem nguiBaseItem)
	{
		Debug.Log("ItemClasses : " + nguiBaseItem.ItemClass);
		WindowType windowType = GetWindowType(nguiBaseItem);
		if (isShow && windowType != 0)
		{
			if (m_CurrWindow != null)
			{
				NGUITools.SetActive(m_CurrWindow.gameObject, false);
			}
			Show(nguiBaseItem, windowType);
		}
	}

	public void Close()
	{
		if (isShow)
		{
			mItemPopMenuEventListener = null;
			isShow = false;
			CloseAll();
			InactivateSelf();
		}
	}

	private void ActivateSelf()
	{
		base.gameObject.SetActive(true);
	}

	private void InactivateSelf()
	{
		base.gameObject.SetActive(false);
	}
}

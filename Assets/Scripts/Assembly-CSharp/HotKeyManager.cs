using System.Collections.Generic;
using UnityEngine;

public class HotKeyManager : MonoBehaviour
{
	private class EmptyListener : HotKeyListener
	{
		public void OnHotKeyEvent(UIButtonX.ButtonInfo info)
		{
		}
	}

	private const int TYPE_NONE = -2;

	private const int TYPE_ALL = -1;

	public const int TYPE_GRENADE = 0;

	public const int TYPE_RETURN = 1;

	public const int TYPE_AIM = 2;

	public const int TYPE_SWAP_OR_RELOAD = 3;

	public const int TYPE_SKILL_1 = 4;

	public const int TYPE_SKILL_2 = 5;

	public const int TYPE_MENU = 6;

	public const int TYPE_CHATBOX = 7;

	public const int TYPE_MELEE = 8;

	public const int TYPE_WEAPON1 = 9;

	public const int TYPE_WEAPON2 = 10;

	public const int TYPE_WEAPON3 = 11;

	public const int TYPE_INPUT = 13;

	public const int TYPE_SAVE = 14;

	public const int TYPE_CHATBOX_INTERFACE = 15;

	public const int TYPE_MAP = 16;

	public const int TYPE_MISSION = 17;

	public const int TYPE_SKILL_POINT = 18;

	public const int TYPE_MISSION_SWITCH = 19;

	public const int TYPE_OFF_LINE = 20;

	public const int TYPE_VS_RESULT = 21;

	public const int TYPE_PILL = 22;

	public const int TYPE_REBIRTH = 23;

	public const int TYPE_LAST_CHECKPOINT = 24;

	public GameObject m_Grenade;

	public GameObject m_Return;

	public GameObject m_Aim;

	public GameObject m_SwapOrReload;

	public GameObject m_Skill1;

	public GameObject m_Skill2;

	public GameObject m_Menu;

	public GameObject m_ChatBox;

	public GameObject m_Melee;

	public GameObject m_WeaponList;

	public GameObject m_Weapon1;

	public GameObject m_Weapon2;

	public GameObject m_Weapon3;

	public GameObject m_Interact;

	public GameObject m_Map;

	public GameObject m_Mission;

	public GameObject m_SkillPoint;

	public GameObject m_OffLine;

	public GameObject m_Pill;

	public GameObject m_FillAll;

	private HotKeyListener curListener;

	private List<int> typeForbiddenList = new List<int>();

	private void Awake()
	{
		setListener(new EmptyListener());
		m_WeaponList.SetActive(false);
	}

	public void SetAllActiveRecursively(bool state)
	{
		m_Grenade.SetActiveRecursively(state);
		m_Return.SetActiveRecursively(state);
		m_Aim.SetActiveRecursively(state);
		m_SwapOrReload.SetActiveRecursively(state);
		m_Skill1.SetActiveRecursively(state);
		m_Skill2.SetActiveRecursively(state);
		m_Menu.SetActiveRecursively(state);
		m_ChatBox.SetActiveRecursively(state);
		m_Melee.SetActiveRecursively(state);
		m_Interact.SetActiveRecursively(state);
		m_Map.SetActiveRecursively(state);
		m_Mission.SetActive(state);
		m_SkillPoint.SetActive(state);
		m_OffLine.SetActive(state);
	}

	public void setListener(HotKeyListener listener)
	{
		if (listener != null)
		{
			curListener = listener;
		}
		else
		{
			curListener = new EmptyListener();
		}
	}

	public void SendHotKeyEvent(UIButtonX.ButtonInfo info)
	{
		foreach (int typeForbidden in typeForbiddenList)
		{
			if (typeForbidden == info.buttonId)
			{
				return;
			}
		}
		curListener.OnHotKeyEvent(info);
	}

	public void OpenOrCloseWeaponList()
	{
		m_SwapOrReload.SendMessage("Play");
	}

	public void OpenWeaponList()
	{
		m_SwapOrReload.GetComponent<UITweenX>().PlayForward();
	}

	public void CloseWeaponList()
	{
		m_SwapOrReload.GetComponent<UITweenX>().PlayReverse();
	}

	public void ForbidAll()
	{
		for (int i = 0; i < 25; i++)
		{
			typeForbiddenList.Add(i);
		}
	}

	public void ForbidAllWithout(params int[] types)
	{
		for (int i = 0; i < 25; i++)
		{
			bool flag = false;
			foreach (int num in types)
			{
				if (num == i)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				typeForbiddenList.Add(i);
			}
		}
	}

	public void CancelFobid()
	{
		typeForbiddenList.Clear();
	}
}

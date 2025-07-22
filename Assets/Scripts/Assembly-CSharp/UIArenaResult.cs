using UnityEngine;

public class UIArenaResult : UIGameMenu
{
	public UILabel m_LabelExp;

	public UILabel m_LabelGold;

	public UILabel m_LabelMithril;

	public GameObject m_Item;

	public GameObject m_ItemIconPos;

	public GameObject m_ButtonOk;

	private static GameObject uiArena;

	private static int exp;

	private static int gold;

	private static int mithril;

	private static GameObject item;

	private static UIArenaResultListener listener;

	protected override void Awake()
	{
		base.Awake();
		AddDelegate(m_ButtonOk);
		m_LabelExp.text = string.Empty + exp;
		m_LabelGold.text = string.Empty + gold;
		m_LabelMithril.text = string.Empty + mithril;
		if (item == null)
		{
			NGUITools.SetActive(m_Item, false);
		}
		else
		{
			item.transform.parent = m_ItemIconPos.transform;
			item.transform.localPosition = Vector3.zero;
			item.transform.localEulerAngles = Vector3.zero;
			item.transform.localScale = Vector3.one;
		}
		SetMenuCloseOnDestroy(true);
	}

	public static void Show(int exp, int gold, int mithril, GameObject item, UIArenaResultListener listener)
	{
		if (uiArena == null)
		{
			UIArenaResult.exp = exp;
			UIArenaResult.gold = gold;
			UIArenaResult.mithril = mithril;
			UIArenaResult.item = item;
			UIArenaResult.listener = listener;
			GameObject original = ResourceLoad.GetInstance().LoadUI("Arena", "ArenaResult");
			uiArena = Object.Instantiate(original) as GameObject;
		}
	}

	public static void Close()
	{
		if (uiArena != null)
		{
			MemoryManager.FreeNGUI(uiArena);
			uiArena = null;
			item = null;
			if (listener != null)
			{
				listener.OnArenaResultConfirm();
			}
			listener = null;
		}
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		Close();
	}
}

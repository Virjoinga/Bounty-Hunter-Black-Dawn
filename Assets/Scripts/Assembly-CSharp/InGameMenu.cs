using UnityEngine;

public class InGameMenu : MonoBehaviour
{
	public const int BUTTON_0 = 0;

	public const int BUTTON_1 = 1;

	public const int BUTTON_2 = 2;

	public const int BUTTON_3 = 3;

	public const int BUTTON_4 = 4;

	public const int BUTTON_5 = 5;

	public const int BUTTON_SHOP = 99;

	public static InGameMenu instance;

	public static int SUM;

	public UIImageButton[] m_SmallIcon;

	public UIImageButton[] m_BigIcon;

	public UISprite[] m_TextIcon;

	public UIImageButton[] m_ImageButton;

	public UISprite[] m_ImageText;

	public GameObject m_Others;

	public GameObject m_Text;

	private bool bOpen;

	private int[] lockButton;

	private InGameMenuButtonListener listener;

	private static int Index;

	private int IndexForShop;

	public static int CurrentIndex
	{
		get
		{
			return Index;
		}
		set
		{
			Index = value;
		}
	}

	public static void ResetIndex()
	{
		Index = 0;
	}

	private void Awake()
	{
		instance = this;
		SUM = m_ImageButton.Length;
		Resort(Index);
		Lock();
	}

	private void OnEnable()
	{
		NGUITools.SetActive(m_Others, false);
		NGUITools.SetActive(m_Text, false);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Resort(int index)
	{
		int num = -1;
		for (int i = 0; i < SUM; i++)
		{
			if (i == 0)
			{
				m_ImageButton[i].hoverSprite = m_BigIcon[index].hoverSprite;
				m_ImageButton[i].normalSprite = m_BigIcon[index].normalSprite;
				m_ImageButton[i].pressedSprite = m_BigIcon[index].pressedSprite;
			}
			else
			{
				num++;
				if (num == index)
				{
					num++;
				}
				m_ImageButton[i].hoverSprite = m_SmallIcon[num].hoverSprite;
				m_ImageButton[i].normalSprite = m_SmallIcon[num].normalSprite;
				m_ImageButton[i].pressedSprite = m_SmallIcon[num].pressedSprite;
				m_ImageText[i - 1].spriteName = m_TextIcon[num].spriteName;
				m_ImageText[i - 1].MakePixelPerfect();
			}
			m_ImageButton[i].target.spriteName = m_ImageButton[i].normalSprite;
			m_ImageButton[i].target.MakePixelPerfect();
		}
	}

	private void OnButtonEvent(UIButtonX.ButtonInfo info)
	{
		if (lockButton.Length > 0)
		{
			int[] array = lockButton;
			foreach (int num in array)
			{
				if (num == info.buttonId)
				{
					return;
				}
			}
		}
		else if (InGameMenuManager.GetInstance().Lock)
		{
			return;
		}
		switch (info.buttonId)
		{
		case 0:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Pressing)
			{
				if (!bOpen)
				{
					base.gameObject.SendMessage("Play", SendMessageOptions.DontRequireReceiver);
					bOpen = true;
					if (NGUIBackPackUIScript.mInstance != null)
					{
						NGUIBackPackUIScript.mInstance.SetBackPackLockState(true);
					}
				}
			}
			else if (info.buttonEvent == UIButtonX.ButtonInfo.Event.NotifyEnd && bOpen)
			{
				base.gameObject.SendMessage("Play", SendMessageOptions.DontRequireReceiver);
				bOpen = false;
				if (NGUIBackPackUIScript.mInstance != null)
				{
					NGUIBackPackUIScript.mInstance.SetBackPackLockState(false);
				}
			}
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
			if ((info.buttonEvent == UIButtonX.ButtonInfo.Event.Drop || info.buttonEvent == UIButtonX.ButtonInfo.Event.Click) && bOpen)
			{
				base.gameObject.SendMessage("Play", SendMessageOptions.DontRequireReceiver);
				bOpen = false;
				if (NGUIBackPackUIScript.mInstance != null)
				{
					NGUIBackPackUIScript.mInstance.SetBackPackLockState(false);
				}
				if (Index < info.buttonId)
				{
					Index = info.buttonId;
				}
				else
				{
					Index = info.buttonId - 1;
				}
				InGameMenuManager.GetInstance().ShowMenu(Index);
				Resort(Index);
			}
			break;
		case 99:
			if ((info.buttonEvent != UIButtonX.ButtonInfo.Event.Drop && info.buttonEvent != 0) || !bOpen)
			{
				break;
			}
			base.gameObject.SendMessage("Play", SendMessageOptions.DontRequireReceiver);
			bOpen = false;
			if (Index < info.buttonId)
			{
				Index = info.buttonId;
			}
			else
			{
				Index = info.buttonId - 1;
			}
			if (ShopUIScript.mInstance != null)
			{
				if (ShopUIScript.mInstance.IsInBuyPage())
				{
					ShopUIScript.mInstance.ChangePage(ShopPageType.Sell);
				}
				else
				{
					ShopUIScript.mInstance.ChangePage(ShopPageType.Equip);
				}
			}
			Resort(IndexForShop);
			break;
		}
		if (listener != null)
		{
			listener.OnButtonEvent(info);
		}
	}

	public void Lock(params int[] button)
	{
		lockButton = button;
	}

	public void Unlock()
	{
		lockButton = new int[0];
	}

	public bool IsSpread()
	{
		return m_Others.activeSelf;
	}

	public void SetListener(InGameMenuButtonListener listener)
	{
		this.listener = listener;
	}

	public void RemoveListener()
	{
		listener = null;
	}
}

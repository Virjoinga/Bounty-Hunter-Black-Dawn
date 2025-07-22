using System.Collections.Generic;
using UnityEngine;

public class UIIAP : UIDelegateMenu, UIMsgListener
{
	public enum Type
	{
		IAP = 0,
		Exchange = 1
	}

	public const int PANEL_WIDTH = 750;

	public const int PANEL_HEIGHT = 400;

	public GameObject m_ButtonClose;

	public GameObject m_ButtonRestore;

	public BoxCollider m_block;

	public UICheckbox m_CheckBoxIap;

	public UICheckbox m_CheckBoxExchange;

	public GameObject m_DragPanelIap;

	public GameObject m_DragPanelExchange;

	public GameObject[] m_IapButtons;

	public GameObject[] m_ExchangeButtons;

	public UILabel[] m_ExchangePrices;

	public GameObject m_TitleIap;

	public GameObject m_TitleExchange;

	public GameObject m_CashLabel;

	public GameObject m_MithrilLabel;

	public BoxCollider leftCollision;

	public BoxCollider rightCollision;

	private Type type;

	private static GameObject m_IAP;

	private static Type FirstCheckedType;

	private static bool PauseEnable;

	private static bool CloseChange;

	public static IAPName iapName = IAPName.None;

	private void Awake()
	{
		AddDelegate(m_ButtonClose);
		AddDelegate(m_ButtonRestore);
		GameObject[] iapButtons = m_IapButtons;
		foreach (GameObject obj in iapButtons)
		{
			AddDelegate(obj);
		}
		GameObject[] exchangeButtons = m_ExchangeButtons;
		foreach (GameObject obj2 in exchangeButtons)
		{
			AddDelegate(obj2);
		}
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			m_ButtonRestore.SetActive(true);
			break;
		case AndroidConstant.Version.Kindle:
		case AndroidConstant.Version.MM:
		case AndroidConstant.Version.KindleCn:
			m_ButtonRestore.SetActive(false);
			break;
		}
	}

	private void Start()
	{
		initFirstPanel();
		initBackgroundCollision();
		initCollisionOutOfPanel();
		if (PauseEnable)
		{
			GameApp.GetInstance().GetGameScene().UIPause(true);
		}
	}

	private void OnDestroy()
	{
		if (PauseEnable)
		{
			GameApp.GetInstance().GetGameScene().UIPause(false);
		}
	}

	private void initFirstPanel()
	{
		if (FirstCheckedType == Type.IAP)
		{
			m_CheckBoxIap.isChecked = false;
			m_CheckBoxExchange.isChecked = true;
		}
		else if (FirstCheckedType == Type.Exchange)
		{
			m_CheckBoxIap.isChecked = true;
			m_CheckBoxExchange.isChecked = false;
		}
		int num = 0;
		foreach (KeyValuePair<ExchangeName, ExchangeItem> exchange in IAPShop.GetInstance().GetExchangeList())
		{
			m_ExchangePrices[num].text = LocalizationManager.GetInstance().GetString("MENU_IAP_GOLDS").Replace("%d", string.Empty + exchange.Value.Cash);
			num++;
		}
		NGUITools.SetActive(m_DragPanelIap, false);
		NGUITools.SetActive(m_DragPanelExchange, false);
	}

	private void initBackgroundCollision()
	{
		m_block.size = new Vector3(Screen.width, Screen.height, 0f);
	}

	private void initCollisionOutOfPanel()
	{
		int num = 375 + (Screen.width - 750) / 4;
		int num2 = 0;
		leftCollision.transform.localPosition = new Vector3(-num, num2, 0f);
		rightCollision.transform.localPosition = new Vector3(num, num2, 0f);
		leftCollision.size = new Vector3((Screen.width - 750) / 2, 400f, 0f);
		rightCollision.size = new Vector3((Screen.width - 750) / 2, 400f, 0f);
	}

	private void OnIAPActivate(bool isChecked)
	{
		NGUITools.SetActive(m_TitleIap, isChecked);
		NGUITools.SetActive(m_TitleExchange, !isChecked);
		NGUITools.SetActive(m_DragPanelIap, isChecked);
		NGUITools.SetActive(m_DragPanelExchange, !isChecked);
		m_CashLabel.SetActive(!isChecked);
		if (isChecked)
		{
			type = Type.IAP;
		}
		if (CloseChange)
		{
			m_CheckBoxIap.gameObject.SetActive(false);
			m_CheckBoxExchange.gameObject.SetActive(false);
		}
	}

	private void OnExchangeActivate(bool isChecked)
	{
		NGUITools.SetActive(m_TitleIap, !isChecked);
		NGUITools.SetActive(m_TitleExchange, isChecked);
		NGUITools.SetActive(m_DragPanelIap, !isChecked);
		NGUITools.SetActive(m_DragPanelExchange, isChecked);
		m_CashLabel.SetActive(isChecked);
		if (isChecked)
		{
			type = Type.Exchange;
		}
		if (CloseChange)
		{
			m_CheckBoxIap.gameObject.SetActive(false);
			m_CheckBoxExchange.gameObject.SetActive(false);
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (go.Equals(m_ButtonClose))
		{
			Close();
		}
		else if (go.Equals(m_ButtonRestore))
		{
			iapName = IAPName.Restore;
			AndroidPluginScript.GetRestorePurchse();
			UILoadingNet.m_instance.Show(LocalizationManager.GetInstance().GetString("MSG_IAP_FAILED"), 300f, 29, this);
		}
		else
		{
			Purchase(go);
		}
	}

	private IAPName GetButtonIapName(GameObject go, GameObject[] gos)
	{
		for (int i = 0; i < gos.Length; i++)
		{
			if (go.Equals(gos[i]))
			{
				IapItem component = go.GetComponent<IapItem>();
				return component.iapItemName;
			}
		}
		return IAPName.None;
	}

	private int IndexIn(GameObject go, GameObject[] gos)
	{
		for (int i = 0; i < gos.Length; i++)
		{
			if (go.Equals(gos[i]))
			{
				return i;
			}
		}
		return -1;
	}

	private void Purchase(GameObject go)
	{
		switch (type)
		{
		case Type.IAP:
		{
			IAPName buttonIapName = GetButtonIapName(go, m_IapButtons);
			PurchaseProduct(buttonIapName);
			break;
		}
		case Type.Exchange:
		{
			int index = IndexIn(go, m_ExchangeButtons);
			PurchaseGold(index);
			break;
		}
		}
	}

	private void PurchaseProduct(IAPName name)
	{
		if (GameApp.GetInstance().IsConnectedToInternet())
		{
			Debug.Log("PurchaseMithril : " + name);
			iapName = name;
			IAPItem iAPItem = IAPShop.GetInstance().GetIAPList()[iapName];
			AndroidPluginScript.CallPurchaseProduct(iAPItem.ID);
			UILoadingNet.m_instance.Show(LocalizationManager.GetInstance().GetString("MSG_IAP_FAILED"), 300f, 29, this);
		}
	}

	private void PurchaseGold(int index)
	{
		Debug.Log("PurchaseGold : " + index);
		GameApp.GetInstance().GetUserState().BuyCashWithMithril((ExchangeName)index);
	}

	private void Update()
	{
		if (iapName != IAPName.None)
		{
			switch (AndroidPluginScript.GetPurchaseStatus())
			{
			case 1:
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(iapName);
				iapName = IAPName.None;
				UILoadingNet.m_instance.Hide();
				break;
			default:
				iapName = IAPName.None;
				if (UILoadingNet.m_instance != null)
				{
					UILoadingNet.m_instance.Hide();
				}
				break;
			case 0:
				break;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Close();
		}
	}

	public static void Show(Type type)
	{
		Show(type, false);
	}

	public static void Show(Type type, bool pauseEnable)
	{
		Show(type, pauseEnable, false);
	}

	public static void Show(Type type, bool pauseEnable, bool closeChange)
	{
		if (m_IAP == null)
		{
			FirstCheckedType = type;
			GameObject original = ResourceLoad.GetInstance().LoadUI("IAP", "IAPUI");
			m_IAP = Object.Instantiate(original) as GameObject;
			PauseEnable = pauseEnable;
			CloseChange = closeChange;
			UIMemoryManager.IncreaseMemoryClearCounter(1);
			if (NGUIBackPackUIScript.mInstance != null)
			{
				NGUIBackPackUIScript.mInstance.SetBackPackBlockState(true);
			}
		}
	}

	public static void Close()
	{
		if (m_IAP != null)
		{
			MemoryManager.FreeNGUI(m_IAP, false);
			m_IAP = null;
			GameApp.GetInstance().Save();
			UIMemoryManager.CheckMemoryClear();
			if (NGUIBackPackUIScript.mInstance != null)
			{
				NGUIBackPackUIScript.mInstance.SetBackPackBlockState(false);
			}
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 29)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}
}

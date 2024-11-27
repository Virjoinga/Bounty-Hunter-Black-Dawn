using System.Collections.Generic;
using UnityEngine;

public class UIDecoration : UIDelegateMenu, UIMsgListener
{
	public GameObject m_StateCanNotBePurchased;

	public GameObject m_StateCanBePurchased;

	public GameObject m_StateCanBeEquipped;

	public GameObject m_StateEquipped;

	public UIDisk m_UIDisk;

	public UILabel m_DecorationName;

	public UILabel m_DecorationPrice;

	public GameObject m_ButtonPurchase;

	public GameObject m_ButtonPurchaseMithrilNotEnough;

	public GameObject m_ButtonEquip;

	public GameObject m_ButtonUnequip;

	public GameObject m_Information;

	public GameObject m_Options;

	public GameObject m_Model;

	public UILabel m_Tips;

	private int nextPart = -1;

	private int part = -1;

	private int index = -1;

	private PersonalDecorationManager manager;

	private void Awake()
	{
		AddDelegate(m_ButtonPurchase);
		AddDelegate(m_ButtonPurchaseMithrilNotEnough);
		AddDelegate(m_ButtonEquip);
		AddDelegate(m_ButtonUnequip);
		CloseAllState();
		if (GameApp.GetInstance().GetUserState().GetSex() == Sex.M)
		{
			m_Information.SetActive(false);
			m_Options.SetActive(false);
			m_Model.SetActive(false);
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (go.Equals(m_ButtonPurchase))
		{
			manager.Purchase(part, index);
			Refresh();
			GameApp.GetInstance().Save();
		}
		else if (go.Equals(m_ButtonEquip))
		{
			manager.Equip(part, index);
			Refresh();
			GameApp.GetInstance().Save();
			UIAvatarShop.instance.IsDecorationChanged = true;
		}
		else if (go.Equals(m_ButtonUnequip))
		{
			manager.Unequip(part);
			Refresh();
			GameApp.GetInstance().Save();
			UIAvatarShop.instance.IsDecorationChanged = true;
		}
		else if (go.Equals(m_ButtonPurchaseMithrilNotEnough))
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
		}
	}

	private void OnEnable()
	{
		if (UIAvatarShop.instance != null)
		{
			manager = DecorationManager.GetInstance().GetPersonalDecorationManager(GameApp.GetInstance().GetUserState());
			part = -1;
			index = -1;
		}
	}

	private void OnDisable()
	{
		manager = null;
	}

	private void CloseAllState()
	{
		m_StateCanNotBePurchased.SetActive(false);
		m_StateCanBePurchased.SetActive(false);
		m_StateCanBeEquipped.SetActive(false);
		m_StateEquipped.SetActive(false);
	}

	private void OnHeadActivate(bool isChecked)
	{
		if (isChecked)
		{
			RefreshDecoration(Global.TOTAL_ARMOR_HEAD_NUM, "Decoration/Head/");
			nextPart = 0;
			m_Tips.gameObject.SetActive(false);
			m_Information.SetActive(true);
			m_UIDisk.gameObject.SetActive(true);
		}
	}

	private void OnFaceActivate(bool isChecked)
	{
		if (isChecked)
		{
			nextPart = -1;
			m_UIDisk.Clear();
			m_UIDisk.gameObject.SetActive(false);
			m_Tips.gameObject.SetActive(true);
			m_Information.SetActive(false);
		}
	}

	private void OnWaistActivate(bool isChecked)
	{
		if (isChecked)
		{
			nextPart = -1;
			m_UIDisk.Clear();
			m_UIDisk.gameObject.SetActive(false);
			m_Tips.gameObject.SetActive(true);
			m_Information.SetActive(false);
		}
	}

	private void RefreshDecoration(int total, string path)
	{
		CharacterClass characterClass = GameApp.GetInstance().GetUserState().GetCharacterClass();
		m_UIDisk.Clear();
		for (int i = 0; i < total; i++)
		{
			GameObject gameObject = (GameObject)Resources.Load(string.Concat(path, characterClass, "/", i));
			if (gameObject != null)
			{
				GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
				gameObject2.layer = LayerMask.NameToLayer("UI");
				m_UIDisk.Add(gameObject2);
			}
		}
		m_UIDisk.Reset();
		m_UIDisk.Reposition();
	}

	private void Update()
	{
		if ((part != nextPart || index != m_UIDisk.GetIndex()) && nextPart > -1 && manager != null)
		{
			part = nextPart;
			index = m_UIDisk.GetIndex();
			Refresh();
		}
	}

	private void Refresh()
	{
		CloseAllState();
		List<Decoration> decorationList = manager.GetDecorationList(part);
		m_DecorationName.text = decorationList[index].Name;
		m_DecorationPrice.text = string.Empty + decorationList[index].Price;
		if (decorationList[index].CurrentState == Decoration.State.Purchased)
		{
			m_StateCanBeEquipped.SetActive(true);
			m_DecorationPrice.text = "[000000]" + m_DecorationPrice.text + "[-]";
		}
		else if (decorationList[index].CurrentState == Decoration.State.Equipped)
		{
			m_StateEquipped.SetActive(true);
			m_DecorationPrice.text = "[000000]" + m_DecorationPrice.text + "[-]";
		}
		else if (GameApp.GetInstance().GetGlobalState().GetMithril() < decorationList[index].Price)
		{
			m_StateCanNotBePurchased.SetActive(true);
			m_DecorationPrice.text = "[ff0000]" + m_DecorationPrice.text + "[-]";
		}
		else
		{
			m_StateCanBePurchased.SetActive(true);
			m_DecorationPrice.text = "[000000]" + m_DecorationPrice.text + "[-]";
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 9 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
			UIIAP.Show(UIIAP.Type.IAP);
		}
	}
}

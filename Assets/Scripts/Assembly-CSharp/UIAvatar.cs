using System.Collections.Generic;
using UnityEngine;

public class UIAvatar : UIDelegateMenu, UIMsgListener
{
	public GameObject m_StateCanNotBePurchased;

	public GameObject m_StateCanBePurchased;

	public GameObject m_StateCanBeEquipped;

	public GameObject m_StateEquipped;

	public UIDisk m_UIDisk;

	public UILabel m_AvatarName;

	public UILabel m_AvatarPrice;

	public GameObject m_ButtonPurchase;

	public GameObject m_ButtonPurchaseMithrilNotEnough;

	public GameObject m_ButtonEquip;

	public GameObject m_ButtonUnequip;

	public GameObject m_Information;

	public GameObject m_AvatarPoint;

	public Camera m_AvatarCamera;

	private int index = -1;

	private PersonalAvatarManager manager;

	private int lastMithril;

	private void Awake()
	{
		AddDelegate(m_ButtonPurchase);
		AddDelegate(m_ButtonPurchaseMithrilNotEnough);
		AddDelegate(m_ButtonEquip);
		CloseAllState();
		float num = 0f;
		float num2 = 0f;
		Transform parent = m_AvatarPoint.transform;
		while (parent != null && parent.gameObject.GetComponent<UIRoot>() == null)
		{
			num += parent.localPosition.x;
			num2 += parent.localPosition.y;
			parent = parent.parent;
		}
		float num3 = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		float num4 = (float)Screen.width * num3 / (float)Screen.height;
		float left = num * 2f / num4;
		float top = (num3 / 2f + num2) / num3;
		m_AvatarCamera.rect = new Rect(left, top, 1f, 1f);
		lastMithril = -1;
	}

	private void OnEnable()
	{
		index = -1;
		manager = AvatarManager.GetInstance().GetPersonalAvatarManager(GameApp.GetInstance().GetUserState());
		Debug.Log("OnEnable");
		CloseAllState();
		for (int i = 0; i < manager.GetList().Count; i++)
		{
			GameObject go = AvatarBuilder.GetInstance().CreateUIAvatarWithWeapon(GameApp.GetInstance().GetUserState().GetRoleState(), "UI", true, i);
			m_UIDisk.Add(go);
		}
		m_UIDisk.Reposition();
	}

	private void OnDisable()
	{
		Debug.Log("OnDisable");
		m_UIDisk.Clear();
		m_UIDisk.Reset();
	}

	private void CloseAllState()
	{
		m_StateCanNotBePurchased.SetActive(false);
		m_StateCanBePurchased.SetActive(false);
		m_StateCanBeEquipped.SetActive(false);
		m_StateEquipped.SetActive(false);
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (manager != null)
		{
			if (go.Equals(m_ButtonPurchase))
			{
				manager.Purchase(index);
				GameApp.GetInstance().Save();
			}
			else if (go.Equals(m_ButtonEquip))
			{
				manager.Equip(index);
				GameApp.GetInstance().Save();
				UIAvatarShop.instance.IsAvatarChanged = true;
			}
			else if (go.Equals(m_ButtonPurchaseMithrilNotEnough))
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
			}
			Refresh();
		}
	}

	private void Update()
	{
		if (UIAvatarShop.instance != null)
		{
			if (index != m_UIDisk.GetIndex())
			{
				index = m_UIDisk.GetIndex();
				Refresh();
			}
			if (lastMithril != GameApp.GetInstance().GetGlobalState().GetMithril())
			{
				lastMithril = GameApp.GetInstance().GetGlobalState().GetMithril();
				Refresh();
			}
		}
	}

	private void Refresh()
	{
		CloseAllState();
		if (manager != null && index != -1)
		{
			List<Avatar> list = manager.GetList();
			m_AvatarName.text = list[index].Name;
			m_AvatarPrice.text = string.Empty + list[index].Price;
			if (list[index].CurrentState == Avatar.State.Purchased)
			{
				m_StateCanBeEquipped.SetActive(true);
				m_AvatarPrice.text = "[000000]" + m_AvatarPrice.text + "[-]";
			}
			else if (list[index].CurrentState == Avatar.State.Equipped)
			{
				m_StateEquipped.SetActive(true);
				m_AvatarPrice.text = "[000000]" + m_AvatarPrice.text + "[-]";
			}
			else if (GameApp.GetInstance().GetGlobalState().GetMithril() < list[index].Price)
			{
				m_StateCanNotBePurchased.SetActive(true);
				m_AvatarPrice.text = "[ff0000]" + m_AvatarPrice.text + "[-]";
			}
			else
			{
				m_StateCanBePurchased.SetActive(true);
				m_AvatarPrice.text = "[000000]" + m_AvatarPrice.text + "[-]";
			}
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

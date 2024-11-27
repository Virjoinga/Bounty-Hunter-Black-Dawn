using UnityEngine;

public class UICharacterChoose : UIDelegateMenu, UIMsgListener
{
	private const int GROUP_OPEN_MENU_STEP_ONE = 0;

	private const int GROUP_OPEN_MENU_STEP_TWO = 1;

	private const int COUNT_CHARACTER_ROW = 5;

	private const int COUNT_CHARACTER = 10;

	public GameObject[] m_CharacterButton;

	public GameObject[] m_CharacterDeleteButton;

	public GameObject m_EnterGameButton;

	public GameObject m_BackButton;

	public GameObject m_UpArrowButton;

	public GameObject m_DownArrowButton;

	public GameObject m_MithrilShopButton;

	public UICharacterChooseInfo m_CharInfo;

	public UIDraggablePanelAlign m_DPA;

	public UILabel m_Mithril;

	public Camera m_AvatarCamera;

	public Transform m_AvatarPoint;

	private bool isConfirm;

	private UICharacterDataBase.Index curIndex;

	private UICharacterDataBase.Index whichWillbeDeleted;

	private UITweenX tween;

	private bool isAnimationPlay;

	private void Awake()
	{
		for (int i = 0; i < 10; i++)
		{
			AddDelegate(m_CharacterButton[i]);
			AddDelegate(m_CharacterDeleteButton[i]);
		}
		AddDelegate(m_EnterGameButton);
		AddDelegate(m_UpArrowButton);
		AddDelegate(m_DownArrowButton);
		AddDelegate(m_BackButton);
		AddDelegate(m_MithrilShopButton);
		tween = GetComponent<UITweenX>();
		isAnimationPlay = false;
		float num = 0f;
		Transform parent = m_AvatarPoint.transform;
		while (parent != null && parent.gameObject.GetComponent<UIRoot>() == null)
		{
			num += parent.localPosition.x;
			parent = parent.parent;
		}
		float num2 = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		float num3 = (float)Screen.width * num2 / (float)Screen.height;
		float left = num / num3;
		m_AvatarCamera.rect = new Rect(left, 0.1f, 1f, 1f);
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (isAnimationPlay)
		{
			return;
		}
		for (int i = 0; i < 10; i++)
		{
			if (go.Equals(m_CharacterButton[i]))
			{
				if (curIndex == (UICharacterDataBase.Index)i)
				{
					if (HasProfile())
					{
						Play(1, false, ReadyToEnterGame);
					}
					else
					{
						Play(1, false, HideMenuChoose);
					}
				}
				else if (!ChooseCharacter((UICharacterDataBase.Index)i))
				{
					Play(1, false, HideMenuChoose);
				}
				break;
			}
			if (go.Equals(m_CharacterDeleteButton[i]))
			{
				whichWillbeDeleted = (UICharacterDataBase.Index)i;
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_DELETE_ROLE_WARNING_CONFIRM"), 3);
				break;
			}
		}
		if (go.Equals(m_EnterGameButton))
		{
			if (HasProfile())
			{
				Play(1, false, ReadyToEnterGame);
			}
		}
		else if (go.Equals(m_BackButton))
		{
			Play(1, false, ReadyBackToMainMenu);
		}
		else if (go.Equals(m_UpArrowButton))
		{
			m_DPA.MoveToPrevIndexAlign();
		}
		else if (go.Equals(m_DownArrowButton))
		{
			m_DPA.MoveToNextIndexAlign();
		}
		else if (go.Equals(m_MithrilShopButton))
		{
			Play(1, false, ReadyToShowMenuMithril);
		}
	}

	private void Play(int group, bool forward, UITweenX.VoidAction action)
	{
		isAnimationPlay = true;
		if (forward)
		{
			tween.PlayForward(group, action);
		}
		else
		{
			tween.PlayReverse(group, action);
		}
	}

	private void PlayOpenMenuTwo()
	{
		Play(1, true, EndAnimation);
	}

	private void EndAnimation()
	{
		isAnimationPlay = false;
	}

	private void ReadyToEnterGame()
	{
		Play(0, false, EnterGame);
	}

	private void EnterGame()
	{
		if (!UICharacter.instance.EnterGame())
		{
			UIMsgBox.instance.ShowSystemMessage(this, LocalizationManager.GetInstance().GetString("MSG_RMS_LOAD_ROLE_FAILED_PROMPT"), 2, 58);
		}
	}

	private void ReadyBackToMainMenu()
	{
		Play(0, false, BackToMainMenu);
	}

	private void BackToMainMenu()
	{
		int phase = 2;
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(phase, false, false, false);
	}

	private void HideMenuChoose()
	{
		Play(0, false, ShowMenuCreate);
	}

	private void ShowMenuCreate()
	{
		UICharacter.instance.ShowMenuCreate();
	}

	private void ReadyToShowMenuMithril()
	{
		Play(0, false, ShowMenuMithril);
	}

	private void ShowMenuMithril()
	{
		UIAvatarShop.Show(GameApp.GetInstance().GetGlobalState().GetCurrRole());
	}

	private void OnEnable()
	{
		UICharacterDataBase.getInstance().UpdateData();
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		m_Mithril.text = string.Empty + globalState.GetMithril();
		ChooseCharacter((UICharacterDataBase.Index)globalState.GetLastCharacterIndex());
		m_UpArrowButton.SetActive(false);
		Play(0, true, PlayOpenMenuTwo);
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_browse");
	}

	private void SetArrow(GameObject go, bool visible)
	{
		if (visible)
		{
			if (!go.activeSelf)
			{
				go.SetActive(true);
			}
		}
		else if (go.activeSelf)
		{
			go.SetActive(false);
		}
	}

	private void UpdateArrow()
	{
		if (m_DPA.SelectedIndex == 0)
		{
			SetArrow(m_UpArrowButton, false);
			SetArrow(m_DownArrowButton, true);
		}
		else if (m_DPA.SelectedIndex == 5)
		{
			SetArrow(m_UpArrowButton, true);
			SetArrow(m_DownArrowButton, false);
		}
		else
		{
			SetArrow(m_UpArrowButton, true);
			SetArrow(m_DownArrowButton, true);
		}
	}

	private void Update()
	{
		UpdateArrow();
		if (HasProfile())
		{
			UIImageButton componentInChildren = m_CharacterButton[(int)curIndex].GetComponentInChildren<UIImageButton>();
			componentInChildren.target.spriteName = componentInChildren.pressedSprite;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Play(1, false, ReadyBackToMainMenu);
		}
	}

	private bool ChooseCharacter(UICharacterDataBase.Index index)
	{
		UIImageButton componentInChildren = m_CharacterButton[(int)curIndex].GetComponentInChildren<UIImageButton>();
		componentInChildren.target.spriteName = componentInChildren.normalSprite;
		curIndex = index;
		UICharacterData uICharacterData = UICharacterDataBase.getInstance().ChooseData(curIndex);
		if (uICharacterData.IsHasProfile)
		{
			m_CharInfo.ShowCharacterInfo(uICharacterData);
		}
		else
		{
			m_CharInfo.Clear();
		}
		return uICharacterData.IsHasProfile;
	}

	private bool HasProfile()
	{
		return UICharacterDataBase.getInstance().ChooseData(curIndex).IsHasProfile;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton eventID)
	{
		if (whichMsg.EventId == 58)
		{
			Debug.Log("OnButtonClick: UIMsgBox.EVENT_RMS_ERROR");
			UIMsgBox.instance.CloseMessage();
			UICharacterDataBase.getInstance().Delete(curIndex);
			UICharacterDataBase.getInstance().UpdateData();
			OnEnable();
			return;
		}
		switch (eventID)
		{
		case UIMsg.UIMsgButton.Cancel:
			isConfirm = false;
			UIMsgBox.instance.CloseMessage();
			break;
		case UIMsg.UIMsgButton.Ok:
			if (isConfirm)
			{
				isConfirm = false;
				UIMsgBox.instance.CloseMessage();
				UICharacterDataBase.getInstance().Delete(whichWillbeDeleted);
				UICharacterDataBase.getInstance().UpdateData();
				if (curIndex == whichWillbeDeleted)
				{
					ChooseCharacter(UICharacterDataBase.Index.Character1);
				}
				else if (curIndex > whichWillbeDeleted)
				{
					ChooseCharacter(curIndex - 1);
				}
			}
			else
			{
				isConfirm = true;
				UIMsgBox.instance.CloseMessage();
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_DELETE_ROLE_WARNING"), 3);
			}
			break;
		}
	}
}

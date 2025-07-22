using System.Collections.Generic;
using UnityEngine;

public class UICharacterCreate : UIDelegateMenu, UIMsgListener
{
	private const int GROUP_SHOW_MENU_STEP_ONE = 0;

	private const int GROUP_SHOW_MENU_STEP_TWO = 1;

	public UIImageButton m_ButtonMale;

	public UIImageButton m_ButtonFemale;

	public GameObject m_ButtonCreate;

	public GameObject m_ButtonBack;

	public UIDisk m_DiskMale;

	public UIDisk m_DiskFemale;

	public UICharacterCreateCamera m_CameraMale;

	public UICharacterCreateCamera m_CameraFemale;

	public UILabel m_LabelClassName;

	public UILabel m_LabelClassInfo;

	public UILabel m_LabelNickName;

	private UICharacterDataBase.CharSex curSex;

	private UICharacterDataBase.CharClass curIndex;

	private string curNickName;

	private UITweenX tween;

	private bool isAnimationPlay;

	private List<GameObject> avatarList;

	private List<WeaponType> weaponTypeList;

	private void Awake()
	{
		AddDelegate(m_ButtonMale.gameObject);
		AddDelegate(m_ButtonFemale.gameObject);
		AddDelegate(m_ButtonCreate);
		AddDelegate(m_ButtonBack);
		AddDelegate(m_DiskMale.gameObject);
		AddDelegate(m_DiskFemale.gameObject);
		avatarList = new List<GameObject>();
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Engineer, UICharacterDataBase.CharSex.Male));
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Prayer, UICharacterDataBase.CharSex.Male));
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Sniper, UICharacterDataBase.CharSex.Male));
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Soldier, UICharacterDataBase.CharSex.Male));
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Engineer, UICharacterDataBase.CharSex.Female));
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Prayer, UICharacterDataBase.CharSex.Female));
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Sniper, UICharacterDataBase.CharSex.Female));
		avatarList.Add(InitAvatar(UICharacterDataBase.CharClass.Soldier, UICharacterDataBase.CharSex.Female));
		weaponTypeList = new List<WeaponType>();
		weaponTypeList.Add(WeaponType.ShotGun);
		weaponTypeList.Add(WeaponType.Pistol);
		weaponTypeList.Add(WeaponType.Sniper);
		weaponTypeList.Add(WeaponType.AssaultRifle);
		weaponTypeList.Add(WeaponType.ShotGun);
		weaponTypeList.Add(WeaponType.Revolver);
		weaponTypeList.Add(WeaponType.Sniper);
		weaponTypeList.Add(WeaponType.SubMachineGun);
		tween = GetComponent<UITweenX>();
		isAnimationPlay = false;
	}

	private GameObject InitAvatar(UICharacterDataBase.CharClass charClass, UICharacterDataBase.CharSex charSex)
	{
		UIDisk uIDisk = ((charSex != 0) ? m_DiskFemale : m_DiskMale);
		GameObject avatar = UICharacterDataBase.getInstance().GetAvatar(charSex, charClass);
		Transform[] componentsInChildren = avatar.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			transform.gameObject.layer = LayerMask.NameToLayer("NPC");
		}
		uIDisk.Add(avatar);
		avatar.transform.localPosition = new Vector3(avatar.transform.position.x, 0f, avatar.transform.position.z);
		avatar.transform.localScale = new Vector3(1f, 1f, 1f);
		Object.Destroy(avatar.GetComponent<CharacterController>());
		uIDisk.Reposition();
		return avatar;
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (isAnimationPlay)
		{
			return;
		}
		if (go.Equals(m_ButtonMale.gameObject))
		{
			SelectStyle(UICharacterDataBase.CharSex.Male);
		}
		else if (go.Equals(m_ButtonFemale.gameObject))
		{
			SelectStyle(UICharacterDataBase.CharSex.Female);
		}
		else if (go.Equals(m_ButtonBack))
		{
			CloseMenu();
		}
		else if (go.Equals(m_ButtonCreate))
		{
			switch (UICharacterDataBase.getInstance().CheckData(curNickName, curSex, curIndex))
			{
			case UICharacterDataBase.CreateResult.Success:
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_CREATE_ROLE_SUCCESS"), 3, 2);
				break;
			case UICharacterDataBase.CreateResult.SameName:
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_CREATE_ROLE_SAMENAME"), 2, 3);
				break;
			case UICharacterDataBase.CreateResult.IllegalChar:
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_CREATE_ROLE_ILLEGALCHAR"), 2, 3);
				break;
			case UICharacterDataBase.CreateResult.Empty:
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_CREATE_ROLE_EMPTY"), 2, 3);
				break;
			}
		}
	}

	protected override void OnDoubleClickThumb(GameObject go)
	{
		if (go.Equals(m_DiskMale.gameObject))
		{
			m_CameraMale.Switch(false);
			m_CameraFemale.Switch(false);
		}
		else if (go.Equals(m_DiskFemale.gameObject))
		{
			m_CameraMale.Switch(false);
			m_CameraFemale.Switch(false);
		}
	}

	private void OnEnable()
	{
		SelectStyle(UICharacterDataBase.CharSex.Female);
		curIndex = UICharacterDataBase.CharClass.None;
		curNickName = null;
		m_LabelNickName.text = string.Empty;
		OpenMenu();
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_browse");
	}

	private void OpenMenu()
	{
		isAnimationPlay = true;
		tween.PlayForward(0, OpenMenuStepTwo);
	}

	private void OpenMenuStepTwo()
	{
		tween.PlayForward(1, EndAnimation);
	}

	private void EndAnimation()
	{
		isAnimationPlay = false;
	}

	private void CloseMenu()
	{
		isAnimationPlay = true;
		tween.PlayReverse(1, CloseMenuStepTwo);
	}

	private void CloseMenuStepTwo()
	{
		tween.PlayReverse(0, ShowMenuChoose);
	}

	private void CreateCharacter()
	{
		isAnimationPlay = true;
		tween.PlayReverse(1, HideMenu);
	}

	private void HideMenu()
	{
		m_DiskMale.gameObject.SetActive(false);
		m_DiskFemale.gameObject.SetActive(false);
		tween.PlayReverse(0, EnterGame);
	}

	private void EnterGame()
	{
		UICharacterDataBase.getInstance().CreateData(curNickName, curSex, curIndex);
		if (!UICharacter.instance.EnterGame())
		{
			UIMsgBox.instance.ShowSystemMessage(this, LocalizationManager.GetInstance().GetString("MSG_RMS_LOAD_ROLE_FAILED_PROMPT"), 2, 58);
		}
	}

	private void ShowMenuChoose()
	{
		isAnimationPlay = false;
		UICharacter.instance.ShowMenuChoose();
	}

	public void SelectStyle(UICharacterDataBase.CharSex sex)
	{
		if (sex == UICharacterDataBase.CharSex.Male)
		{
			m_DiskMale.gameObject.SetActive(true);
			m_DiskFemale.gameObject.SetActive(false);
			m_CameraMale.m_Camera.enabled = true;
			m_CameraFemale.m_Camera.enabled = false;
			curSex = sex;
		}
		else
		{
			m_DiskMale.gameObject.SetActive(false);
			m_DiskFemale.gameObject.SetActive(true);
			m_CameraMale.m_Camera.enabled = false;
			m_CameraFemale.m_Camera.enabled = true;
			curSex = sex;
		}
		for (int i = 0; i < avatarList.Count; i++)
		{
			if (avatarList[i].activeSelf)
			{
				GameObject gameObject = avatarList[i].transform.Find("Entity").gameObject;
				gameObject.GetComponent<Animation>().wrapMode = WrapMode.Loop;
				gameObject.GetComponent<Animation>().Play(GameApp.GetInstance().GetUserState().GetRoleState()
					.GetUIIdleAnimation(weaponTypeList[i]));
			}
		}
	}

	private UICharacterDataBase.CharClass GetCurIndex()
	{
		int result = 1;
		if (curSex == UICharacterDataBase.CharSex.Male)
		{
			result = m_DiskMale.GetIndex();
		}
		else if (curSex == UICharacterDataBase.CharSex.Female)
		{
			result = m_DiskFemale.GetIndex();
		}
		return (UICharacterDataBase.CharClass)result;
	}

	private void Update()
	{
		UICharacterDataBase.CharClass charClass = curIndex;
		curIndex = GetCurIndex();
		if (charClass != curIndex)
		{
			ChangeClassInfo(curIndex);
		}
		if (curSex == UICharacterDataBase.CharSex.Male)
		{
			m_ButtonMale.target.spriteName = m_ButtonMale.pressedSprite;
			m_ButtonMale.target.MakePixelPerfect();
			m_ButtonFemale.target.spriteName = m_ButtonFemale.normalSprite;
			m_ButtonFemale.target.MakePixelPerfect();
		}
		else
		{
			m_ButtonFemale.target.spriteName = m_ButtonFemale.pressedSprite;
			m_ButtonFemale.target.MakePixelPerfect();
			m_ButtonMale.target.spriteName = m_ButtonMale.normalSprite;
			m_ButtonMale.target.MakePixelPerfect();
		}
		curNickName = m_LabelNickName.text;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			CloseMenu();
		}
	}

	private void ChangeClassInfo(UICharacterDataBase.CharClass index)
	{
		m_LabelClassName.text = UICharacterDataBase.getInstance().GetClassName(index);
		m_LabelClassInfo.text = UICharacterDataBase.getInstance().GetClassInfo(index);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		switch (whichMsg.EventId)
		{
		case 58:
			UIMsgBox.instance.CloseMessage();
			OnEnable();
			break;
		case 2:
			if (buttonId == UIMsg.UIMsgButton.Cancel)
			{
				UIMsgBox.instance.CloseMessage();
				break;
			}
			UIMsgBox.instance.CloseMessage();
			CreateCharacter();
			break;
		case 3:
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				m_LabelNickName.text = string.Empty;
			}
			break;
		}
	}
}

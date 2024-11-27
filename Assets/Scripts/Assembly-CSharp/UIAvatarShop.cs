using UnityEngine;

public class UIAvatarShop : UIGameMenu
{
	public static UIAvatarShop instance;

	public GameObject m_Avatar;

	public GameObject windowAvatar;

	public GameObject windowDecoration;

	public GameObject m_CheckMarkAvatar;

	public GameObject m_CheckMarkDecoration;

	private GameObject m_Character;

	private bool isDecorationChanged;

	private bool isAvatarChanged;

	private float mLastUpdateAimTime;

	private UserState.RoleState roleState
	{
		get
		{
			if (GameApp.GetInstance().GetUserState().GetRoleName()
				.Equals(string.Empty))
			{
				return null;
			}
			return GameApp.GetInstance().GetUserState().GetRoleState();
		}
	}

	public GameObject Player
	{
		get
		{
			return m_Character;
		}
	}

	public bool IsDecorationChanged
	{
		get
		{
			return isDecorationChanged;
		}
		set
		{
			isDecorationChanged = value;
		}
	}

	public bool IsAvatarChanged
	{
		get
		{
			return isAvatarChanged;
		}
		set
		{
			isAvatarChanged = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		instance = this;
		CreateCharacterModel();
		RefreshDecorations();
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_browse");
	}

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		instance = null;
	}

	private void RefreshDecorations()
	{
		if (roleState != null)
		{
			AvatarBuilder.GetInstance().ChangeDecorations(Player, roleState);
		}
	}

	private void DestroyCharacterModel()
	{
		if (m_Character != null)
		{
			Object.Destroy(m_Character);
		}
	}

	private void CreateCharacterModel()
	{
		if (roleState != null)
		{
			m_Character = AvatarBuilder.GetInstance().CreateUIAvatarWithWeapon(roleState, m_Avatar, "UI", true);
		}
	}

	private void OnAvatarActivate(bool isChecked)
	{
		windowAvatar.SetActive(isChecked);
	}

	private void OnDecorationActivate(bool isChecked)
	{
		windowDecoration.SetActive(isChecked);
	}

	private void Update()
	{
		if (Time.time - mLastUpdateAimTime > 0.2f)
		{
			mLastUpdateAimTime = Time.time;
			if (isDecorationChanged)
			{
				isDecorationChanged = false;
				RefreshDecorations();
			}
			if (IsAvatarChanged)
			{
				IsAvatarChanged = false;
				DestroyCharacterModel();
				CreateCharacterModel();
			}
		}
	}

	public static void Show()
	{
		Show(null);
	}

	public static void Show(string roleName)
	{
		if (instance == null)
		{
			if (!string.IsNullOrEmpty(roleName))
			{
				GameApp.GetInstance().LoadUserDataLocal(roleName);
			}
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(39, false, false, false);
		}
	}

	public static void Close()
	{
		if (instance != null)
		{
			int phase = GameApp.GetInstance().GetUIStateManager().FrGetPreviousPhase();
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(phase, false, false, false);
		}
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}
}

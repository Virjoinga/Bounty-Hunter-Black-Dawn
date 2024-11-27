using UnityEngine;

public class UIOption : UIGameMenuNormal
{
	public GameObject m_ButtonSetting;

	public GameObject m_ButtonQuit;

	public GameObject m_Player;

	private GameObject m_Character;

	private bool bExit;

	private static bool isHideMenu;

	protected override void Awake()
	{
		base.Awake();
		UIEventListener uIEventListener = UIEventListener.Get(m_ButtonSetting);
		uIEventListener.onClick = OnClickButton;
		uIEventListener = UIEventListener.Get(m_ButtonQuit);
		uIEventListener.onClick = OnClickButton;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		m_Character = AvatarBuilder.GetInstance().CreateUIAvatarWithWeapon(GameApp.GetInstance().GetUserState().GetRoleState(), m_Player, "UI", true);
		if (isHideMenu)
		{
			InGameMenuManager.GetInstance().ShowStateBarAfterHide();
			InGameMenuManager.GetInstance().ShowCloseButtonAfterHide();
			InGameMenuManager.GetInstance().ShowGameMenuButtonAfterHide();
			isHideMenu = false;
		}
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (num != 0f && m_Character != null)
		{
			GameObject gameObject = m_Character.transform.Find("Entity").gameObject;
			AnimationState animationState = gameObject.GetComponent<Animation>()[GameApp.GetInstance().GetUserState().GetRoleState()
				.GetUIIdleAnimation()];
			float num2 = animationState.speed * num;
			animationState.time += num2;
			if (animationState.time > animationState.length)
			{
				animationState.time = 0f;
			}
		}
	}

	private void LateUpdate()
	{
		if (bExit)
		{
			bExit = false;
			InGameMenuManager.GetInstance().Close(false);
			TutorialManager.GetInstance().DestroyTutorialPrefab();
			SetMenuCloseOnDestroy(false);
			GameScene gameScene = GameApp.GetInstance().GetGameScene();
			gameScene.LeaveScene();
			GameApp.GetInstance().GetGameWorld().DestroyLocalPlayer();
			GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
			GameApp.GetInstance().CloseConnectionGameServer();
			GameApp.GetInstance().Save();
			GameApp.GetInstance().GetGameScene().UIPause(false);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				GameApp.GetInstance().CloseConnectionGameServer();
			}
			GameApp.GetInstance().ClearGameScene();
			Application.LoadLevel("StartMenu");
		}
	}

	private void OnClickButton(GameObject go)
	{
		if (go.Equals(m_ButtonSetting))
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(19, false, false, false);
			InGameMenuManager.GetInstance().HideStateBarAfterShow();
			InGameMenuManager.GetInstance().HideCloseButtonAfterShow();
			InGameMenuManager.GetInstance().HideGameMenuButtonAfterShow();
			isHideMenu = true;
		}
		else if (go.Equals(m_ButtonQuit))
		{
			bExit = true;
		}
	}
}

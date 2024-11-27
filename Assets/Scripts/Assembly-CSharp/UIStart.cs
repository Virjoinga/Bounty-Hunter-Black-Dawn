using UnityEngine;

public class UIStart : UIDelegateMenu, UIMsgListener
{
	public enum StartMode
	{
		Normal = 0,
		Pvp = 1,
		BossRush = 2
	}

	private static bool bHaveSkipTouchScreen;

	public static StartMode STARTMODE;

	public GameObject m_TouchScreen;

	public GameObject m_Menu;

	public GameObject m_ButtonSetting;

	public GameObject m_ButtonCredit;

	public GameObject m_ButtonPlay;

	public GameObject m_ButtonPvp;

	public GameObject m_SpritePvpLock;

	public GameObject m_ButtonBossRush;

	public GameObject m_SpriteBossRushLock;

	public GameObject m_ButtonIap;

	public GameObject m_ButtonGameCenter;

	private UITweenX tween;

	private float startTime;

	private void Awake()
	{
		AddDelegate(m_TouchScreen);
		AddDelegate(m_ButtonPlay);
		AddDelegate(m_ButtonCredit);
		AddDelegate(m_ButtonSetting);
		m_ButtonGameCenter.gameObject.SetActive(false);
		AddDelegate(m_ButtonIap);
		tween = GetComponent<UITweenX>();
		m_Menu.SetActive(false);
		if (GameApp.ShowFreyrGamesAds && AndroidConstant.version == AndroidConstant.Version.Kindle)
		{
			AndroidPluginScript.ShowFreyrGames(AndroidConstant.BUNDLEID_STARWARFARE_KINDLE, AndroidConstant.SHOW_START_DIALOG_START);
		}
		AndroidPluginScript.DoStart();
		if (TutorialManager.GetInstance().IsFirstTutorialOk() && TutorialManager.GetInstance().IsMapTutorialOk() && TutorialManager.GetInstance().IsShopTutorialOk())
		{
			m_SpritePvpLock.gameObject.SetActive(false);
			AddDelegate(m_ButtonPvp);
		}
		else
		{
			m_ButtonPvp.gameObject.SetActive(false);
		}
		m_ButtonBossRush.gameObject.SetActive(false);
		m_SpriteBossRushLock.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		m_TouchScreen.SetActive((!bHaveSkipTouchScreen) ? true : false);
		m_Menu.SetActive(bHaveSkipTouchScreen ? true : false);
		if (bHaveSkipTouchScreen)
		{
			tween.PlayForward();
		}
		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - startTime > 1f)
		{
			startTime = Time.time;
			if (!UIMsgBox.instance.IsMessageShow() && AdsManager.GetInstance().MithrilRewardsList.Count > 0)
			{
				AdsMithril adsMithril = AdsManager.GetInstance().MithrilRewardsList[0];
				AdsManager.GetInstance().MithrilRewardsList.RemoveAt(0);
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_REWARDS").Replace("%d", string.Empty + adsMithril.Rewards) + " " + adsMithril.AdCp, 2);
			}
		}
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		GameApp.GetInstance().Save();
		if (GameApp.ShowFreyrGamesAds)
		{
			int num = Random.Range(0, 10);
			int randomCount = AndroidPluginScript.GetRandomCount();
			if (num > randomCount)
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GAME_EXIT"), 3, 64);
			}
			else
			{
				AndroidPluginScript.ShowFreyrGames(AndroidConstant.BUNDLEID_STARWARFARE_KINDLE, AndroidConstant.SHOW_CLOSE_DIALOG_START);
			}
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GAME_EXIT"), 3, 64);
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (go.Equals(m_TouchScreen))
		{
			bHaveSkipTouchScreen = true;
			m_TouchScreen.SetActive(false);
			m_Menu.SetActive(true);
			tween.Play();
		}
		else if (go.Equals(m_ButtonCredit))
		{
			tween.PlayReverse(GoToCredit);
		}
		else if (go.Equals(m_ButtonSetting))
		{
			tween.PlayReverse(GoToSetting);
		}
		else if (go.Equals(m_ButtonPlay))
		{
			STARTMODE = StartMode.Normal;
			tween.PlayReverse(GoToCharacter);
		}
		else if (go.Equals(m_ButtonIap))
		{
			UIIAP.Show(UIIAP.Type.IAP, false, true);
		}
		else if (go.Equals(m_ButtonPvp))
		{
			STARTMODE = StartMode.Pvp;
			tween.PlayReverse(GoToCharacter);
		}
		else if (go.Equals(m_ButtonBossRush))
		{
			STARTMODE = StartMode.BossRush;
			tween.PlayReverse(GoToCharacter);
		}
		else if (!go.Equals(m_ButtonGameCenter))
		{
		}
	}

	private void GoToCredit()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(18, false, false, false);
	}

	private void GoToSetting()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(19, false, false, false);
	}

	private void GoToCharacter()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(20, false, false, false);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 63)
		{
			UIMsgBox.instance.CloseMessage();
		}
		else if (whichMsg.EventId == 64)
		{
			switch (buttonId)
			{
			case UIMsg.UIMsgButton.Ok:
				Application.Quit();
				break;
			case UIMsg.UIMsgButton.Cancel:
				UIMsgBox.instance.CloseMessage();
				break;
			}
		}
	}

	private void OnDestroy()
	{
		STARTMODE = StartMode.Normal;
	}
}

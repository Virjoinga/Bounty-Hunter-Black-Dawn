using UnityEngine;

public class UISetting : UIDelegateMenu, UIMsgListener
{
	public GameObject[] gamePlay;

	public GameObject[] visualSound;

	public UIImageButton m_VerticalCameraOption;

	public UIImageButton m_HorizontalCameraOption;

	public UISlider m_AimAssistSwitch;

	public UISlider m_BloodSprayingSwitch;

	public UISlider m_Sensitive;

	public UISlider m_BGM;

	public UISlider m_SFX;

	public GameObject m_ButtonGamePlay;

	public GameObject m_ButtonVisual;

	public GameObject m_ButtonVerticalCamera;

	public GameObject m_ButtonHoriontalCamera;

	public GameObject m_ButtonBack;

	public GameObject m_ButtonClose;

	public GameObject m_ButtonICloudUpLoad;

	public GameObject m_ButtonICloudDownLoad;

	public UILabel m_LabelUserID;

	public GameObject m_ICloud;

	private bool verticalCameraNormalEnable = true;

	private bool horizontalCameraNormalEnable = true;

	private bool aimAssistEnable;

	private bool bloodSprayingEnable = true;

	private float sensitiveValue = 0.5f;

	private float bgmValue = 0.5f;

	private float sfxValue = 0.5f;

	private UITweenX tween;

	public UILabel android_id;

	private void Awake()
	{
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			android_id.text = "gg:" + GameApp.GetInstance().UUID;
			break;
		case AndroidConstant.Version.Kindle:
		case AndroidConstant.Version.KindleCn:
			android_id.text = "kk:" + GameApp.GetInstance().UUID;
			break;
		case AndroidConstant.Version.MM:
			android_id.text = "mm:" + GameApp.GetInstance().UUID;
			break;
		}
		AddDelegate(m_ButtonGamePlay);
		AddDelegate(m_ButtonVisual);
		AddDelegate(m_ButtonVerticalCamera);
		AddDelegate(m_ButtonHoriontalCamera);
		AddDelegate(m_ButtonBack);
		AddDelegate(m_ButtonClose);
		if (m_ButtonICloudUpLoad != null)
		{
			AddDelegate(m_ButtonICloudUpLoad);
		}
		if (m_ButtonICloudDownLoad != null)
		{
			AddDelegate(m_ButtonICloudDownLoad);
		}
		if (m_ICloud != null)
		{
			m_ICloud.SetActive(false);
		}
		if (m_ButtonICloudUpLoad != null)
		{
			m_ButtonICloudUpLoad.SetActive(false);
		}
		if (m_ButtonICloudDownLoad != null)
		{
			m_ButtonICloudDownLoad.SetActive(false);
		}
		tween = GetComponent<UITweenX>();
		if (GlobalState.user_id == -1)
		{
			m_LabelUserID.gameObject.SetActive(false);
		}
		else
		{
			m_LabelUserID.text = "ID:" + GlobalState.user_id;
		}
	}

	private void OnEnable()
	{
		SetPanel(gamePlay, true);
		SetPanel(visualSound, false);
		ReadData();
		tween.PlayForward();
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_browse");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			tween.PlayReverse(BackToMainMenu);
			Save();
		}
	}

	private void SetPanel(GameObject[] go, bool active)
	{
		foreach (GameObject gameObject in go)
		{
			gameObject.SetActive(active);
		}
	}

	private void ReadData()
	{
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		verticalCameraNormalEnable = globalState.GetVertivalCameraNormal();
		horizontalCameraNormalEnable = globalState.GetHorizontalCamreraNormal();
		aimAssistEnable = globalState.GetAimAssist();
		bloodSprayingEnable = globalState.GetBloodSpraying();
		sensitiveValue = globalState.TouchInputSensitivity;
		bgmValue = globalState.GetMusicVolume();
		sfxValue = globalState.GetSoundVolume();
		ModifyCamera(m_VerticalCameraOption, verticalCameraNormalEnable);
		ModifyCamera(m_HorizontalCameraOption, horizontalCameraNormalEnable);
		m_AimAssistSwitch.sliderValue = ((!aimAssistEnable) ? 1 : 0);
		m_BloodSprayingSwitch.sliderValue = ((!bloodSprayingEnable) ? 1 : 0);
		m_Sensitive.sliderValue = sensitiveValue;
		m_BGM.sliderValue = bgmValue;
		m_SFX.sliderValue = sfxValue;
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (go.Equals(m_ButtonGamePlay))
		{
			SetPanel(gamePlay, true);
			SetPanel(visualSound, false);
		}
		else if (go.Equals(m_ButtonVisual))
		{
			SetPanel(gamePlay, false);
			SetPanel(visualSound, true);
		}
		else if (go.Equals(m_ButtonVerticalCamera))
		{
			verticalCameraNormalEnable = !verticalCameraNormalEnable;
			ModifyCamera(m_VerticalCameraOption, verticalCameraNormalEnable);
		}
		else if (go.Equals(m_ButtonHoriontalCamera))
		{
			horizontalCameraNormalEnable = !horizontalCameraNormalEnable;
			ModifyCamera(m_HorizontalCameraOption, horizontalCameraNormalEnable);
		}
		else if (go.Equals(m_ButtonBack) || go.Equals(m_ButtonClose))
		{
			tween.PlayReverse(BackToMainMenu);
			Save();
		}
		else if (m_ButtonICloudUpLoad != null && go.Equals(m_ButtonICloudUpLoad))
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				if (GameApp.GetInstance().SaveToCloud())
				{
					UIMsgBox.instance.ShowSystemMessage(this, LocalizationManager.GetInstance().GetString("MSG_RMS_ICLOUD_UPLOAD_SUCCESS"), 2, 55);
				}
				else
				{
					UIMsgBox.instance.ShowSystemMessage(this, LocalizationManager.GetInstance().GetString("MSG_RMS_ICLOUD_UPLOAD_FAILED"), 2, 54);
				}
			}
		}
		else if (m_ButtonICloudDownLoad != null && go.Equals(m_ButtonICloudDownLoad) && GameApp.GetInstance().IsConnectedToInternet())
		{
			UIMsgBox.instance.ShowSystemMessage(this, LocalizationManager.GetInstance().GetString("MSG_RMS_ICLOUD_DOWNLOAD_QUERY"), 3, 42);
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
	}

	private void BackToMainMenu()
	{
		int phase = GameApp.GetInstance().GetUIStateManager().FrGetPreviousPhase();
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(phase, false, false, false);
	}

	private void ModifyCamera(UIImageButton _option, bool enable)
	{
		string text = ((!enable) ? "reverse_n" : "normal_n");
		string pressedSprite = ((!enable) ? "reverse_d" : "normal_d");
		_option.normalSprite = text;
		_option.hoverSprite = text;
		_option.pressedSprite = pressedSprite;
		_option.gameObject.SendMessage("OnHover", true, SendMessageOptions.DontRequireReceiver);
	}

	private void SetAimAssist(float value)
	{
		Vector2 interval = new Vector2(0f, 0.5f);
		if (isBetween(value, interval))
		{
			if (!aimAssistEnable)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_option02");
			}
			m_AimAssistSwitch.sliderValue = 0.25f;
			aimAssistEnable = true;
		}
		else
		{
			if (aimAssistEnable)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_option02");
			}
			m_AimAssistSwitch.sliderValue = 0.75f;
			aimAssistEnable = false;
		}
	}

	private void SetBloodSpraying(float value)
	{
		Vector2 interval = new Vector2(0f, 0.5f);
		if (isBetween(value, interval))
		{
			if (!bloodSprayingEnable)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_option02");
			}
			m_BloodSprayingSwitch.sliderValue = 0.25f;
			bloodSprayingEnable = true;
		}
		else
		{
			if (bloodSprayingEnable)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_option02");
			}
			m_BloodSprayingSwitch.sliderValue = 0.75f;
			bloodSprayingEnable = false;
		}
	}

	private void SetSensitive(float value)
	{
		sensitiveValue = value;
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_option01");
	}

	private bool isBetween(float value, Vector2 interval)
	{
		return value >= interval.x && value <= interval.y;
	}

	private void SetSFX(float value)
	{
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_option01");
		sfxValue = value;
		GameApp.GetInstance().GetGlobalState().SetSoundVolume(value);
	}

	private void SetBGM(float value)
	{
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_option01");
		bgmValue = value;
		GameApp.GetInstance().GetGlobalState().SetMusicVolume(value);
	}

	private void Save()
	{
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		globalState.SetVerticalCameraNormal(verticalCameraNormalEnable);
		globalState.SetHorizontalCamreraNormal(horizontalCameraNormalEnable);
		globalState.SetAimAssist(aimAssistEnable);
		globalState.TouchInputSensitivity = sensitiveValue;
		globalState.SetMusicVolume(bgmValue);
		globalState.SetSoundVolume(sfxValue);
		globalState.SetBloodSpraying(bloodSprayingEnable);
		GameApp.GetInstance().SaveGlobalDataLocal();
	}
}

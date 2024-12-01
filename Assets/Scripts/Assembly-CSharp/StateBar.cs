using UnityEngine;

public class StateBar : UIDelegateMenu, UIMsgListener
{
	public static StateBar instance;

	public GameObject m_State;

	public UILabel m_LabelLevel;

	public UILabel m_LabelName;

	public UILabel m_LabelMithril;

	public UILabel m_LabelGP;

	public UILabel m_LabelSkillPoint;

	//public GameObject m_ButtonAddMithril;

	//public GameObject m_ButtonAddGP;

	public UIFilledSprite m_Exp;

	public UILabel m_LabelExp;

	public UISprite m_ClassIcon;

	public GameObject m_Test;

	public bool Lock;

	private float mLastUpdateTime;

	private void Awake()
	{
		instance = this;
		//AddDelegate(m_ButtonAddMithril);
		//AddDelegate(m_ButtonAddGP);
		m_Test.SetActive(false);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
	{
		if (m_State.activeSelf && Time.time - mLastUpdateTime > 0.2f)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			short charLevelForUI = userState.GetCharLevelForUI();
			m_ClassIcon.spriteName = UserStateHUD.GetInstance().GetUserClassIconName();
			m_ClassIcon.MakePixelPerfect();
			m_LabelLevel.text = "Lv." + charLevelForUI;
			m_LabelName.text = string.Empty + userState.GetRoleName();
			m_LabelGP.text = string.Empty + userState.GetCash();
			m_LabelMithril.text = string.Empty + GameApp.GetInstance().GetGlobalState().GetMithril();
			m_LabelSkillPoint.text = string.Empty + userState.SkillTreeManager.GetSkillPointsLeft();
			int exp = userState.GetExp();
			int num = Exp.RequiredLevelUpExp[charLevelForUI - 1];
			int num2 = Exp.RequiredLevelUpExp[charLevelForUI];
			m_Exp.fillAmount = (float)(exp - num) / (float)(num2 - num);
			m_LabelExp.text = exp + "/" + num2;
		}
	}

	public void HideStateBar()
	{
		NGUITools.SetActive(m_State, false);
	}

	public void ShowStateBar()
	{
		NGUITools.SetActive(m_State, true);
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (!Lock && !InGameMenuManager.GetInstance().Lock)
		{
            /*if (go.Equals(m_ButtonAddMithril))
			{
				UIIAP.Show(UIIAP.Type.IAP);
			}
			else if (go.Equals(m_ButtonAddGP))
			{
				UIIAP.Show(UIIAP.Type.Exchange);
			}
			else if (go.Equals(m_Test))
			{
				GameApp.GetInstance().GetUserState().EnterGodMode();
			}*/
            if (go.Equals(m_Test))
            {
                GameApp.GetInstance().GetUserState().EnterGodMode();
            }
        }
	}

	public void InvitationRecieved()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_INVITATION_IN_MENU"), 2, 33);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (buttonId == UIMsg.UIMsgButton.Ok && whichMsg.EventId == 33)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}
}

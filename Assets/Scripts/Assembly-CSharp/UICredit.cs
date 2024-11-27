using UnityEngine;

public class UICredit : UIDelegateMenu
{
	private static string NAME_BUTTON_BACK;

	private static string NAME_BUTTON_CLOSE;

	public GameObject m_ButtonBack;

	public GameObject m_ButtonClose;

	public UILabel m_Label;

	private UITweenX tween;

	private void Awake()
	{
		AddDelegate(m_ButtonBack, out NAME_BUTTON_BACK);
		AddDelegate(m_ButtonClose, out NAME_BUTTON_CLOSE);
		tween = GetComponent<UITweenX>();
		string newValue = GlobalState.version.Insert(GlobalState.version.Length - 2, ".");
		if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
		{
			m_Label.text = LocalizationManager.GetInstance().GetString("MENU_ABOUT").Replace("%d", "1.25.01");
		}
		else
		{
			m_Label.text = LocalizationManager.GetInstance().GetString("MENU_ABOUT").Replace("%d", newValue);
		}
	}

	private void OnEnable()
	{
		AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_browse");
		tween.PlayForward();
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (IsThisObject(go, NAME_BUTTON_BACK) || IsThisObject(go, NAME_BUTTON_CLOSE))
		{
			tween.PlayReverse(BackToMainMenu);
		}
	}

	private void BackToMainMenu()
	{
		int phase = GameApp.GetInstance().GetUIStateManager().FrGetPreviousPhase();
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(phase, false, false, false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			tween.PlayReverse(BackToMainMenu);
		}
	}
}

using UnityEngine;

public class UIArena : UIGameMenu
{
	public GameObject m_ButtonGo;

	public GameObject m_ButtonBack;

	public GameObject m_ButtonSurvival;

	public GameObject m_ButtonEliminate;

	public GameObject m_ModeSurvival;

	public GameObject m_ModeEliminate;

	private IArenaMode currMode;

	private static byte prevPhase;

	private static byte SceneID;

	protected override void Awake()
	{
		base.Awake();
		AddDelegate(m_ButtonGo);
		AddDelegate(m_ButtonBack);
		AddDelegate(m_ButtonSurvival);
		AddDelegate(m_ButtonEliminate);
		SetMenuCloseOnDestroy(true);
	}

	private void OnSurvivalActivate(bool isChecked)
	{
		if (isChecked)
		{
			currMode = GetMode(m_ModeSurvival);
		}
	}

	private void OnEliminateActivate(bool isChecked)
	{
		if (isChecked)
		{
			currMode = GetMode(m_ModeEliminate);
		}
	}

	private IArenaMode GetMode(GameObject go)
	{
		return go.GetComponent("IArenaMode") as IArenaMode;
	}

	public static void Show()
	{
		Show(GameApp.GetInstance().GetGameWorld().CurrentSceneID);
	}

	public new static void Show(byte sceneID)
	{
		SceneID = sceneID;
		prevPhase = 6;
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(29, false, false, true);
	}

	public static void Close()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(prevPhase, false, false, true);
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (go.Equals(m_ButtonGo))
		{
			currMode.Go(SceneID);
			Close();
		}
		else if (go.Equals(m_ButtonBack))
		{
			Close();
		}
	}

	protected override void OnDoubleClickThumb(GameObject go)
	{
		base.OnDoubleClickThumb(go);
		if (go.Equals(m_ButtonSurvival) || go.Equals(m_ButtonEliminate))
		{
			currMode.Go(SceneID);
			Close();
		}
	}
}

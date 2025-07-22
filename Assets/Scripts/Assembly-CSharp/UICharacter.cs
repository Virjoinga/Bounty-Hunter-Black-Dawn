using UnityEngine;

public class UICharacter : MonoBehaviour
{
	public static UICharacter instance;

	public GameObject m_Choose;

	public GameObject m_Create;

	private void Awake()
	{
		instance = this;
		m_Create.SetActive(false);
	}

	private void OnEnable()
	{
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public void ShowMenuChoose()
	{
		m_Choose.SetActive(true);
		m_Create.SetActive(false);
	}

	public void ShowMenuCreate()
	{
		m_Choose.SetActive(false);
		m_Create.SetActive(true);
	}

	public bool EnterGame()
	{
		if (GameApp.GetInstance().LoadUserDataLocal(GameApp.GetInstance().GetGlobalState().GetCurrRole()))
		{
			GameApp.GetInstance().GetUserState().InitGame();
			AchievementManager.GetInstance().Init();
			UserStateHUD.GetInstance().Init();
			if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckQuestCompletedWithCommonId(53))
			{
				GameApp.GetInstance().GetUserState().SetStageInstanceState("Instance5", 1);
			}
			if (UIStart.STARTMODE == UIStart.StartMode.Pvp)
			{
				GameApp.GetInstance().GetUIStateManager().FrGoToPhase(41, false, false, false);
			}
			else if (UIStart.STARTMODE == UIStart.StartMode.BossRush)
			{
				UIBossRush.Show();
			}
			else if (UIStart.STARTMODE == UIStart.StartMode.Normal)
			{
				GameApp.GetInstance().GetUserState().LoadLevel();
			}
			return true;
		}
		return false;
	}
}

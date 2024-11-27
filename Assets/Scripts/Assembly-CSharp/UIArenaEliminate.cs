using UnityEngine;

public class UIArenaEliminate : MonoBehaviour, IArenaMode
{
	private const int LEVEL_1 = 1;

	private const int LEVEL_2 = 2;

	private const int LEVEL_3 = 3;

	private const int LEVEL_4 = 4;

	private const int LEVEL_5 = 5;

	private const int LEVEL_6 = 6;

	public UICheckbox[] m_CheckBox;

	private int level;

	private bool isFirstRun = true;

	private SubMode mode = SubMode.Arena_Campain;

	private void Awake()
	{
		int num = 1;
		int charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		num = ((charLevel >= 1 && charLevel <= 10) ? 1 : ((charLevel >= 11 && charLevel <= 16) ? 2 : ((charLevel < 17 || charLevel > 22) ? 4 : 3)));
		m_CheckBox[num - 1].isChecked = true;
	}

	private void OnLevel1Activate(bool isChecked)
	{
		if (isChecked)
		{
			OnLevelActivate(1);
		}
	}

	private void OnLevel2Activate(bool isChecked)
	{
		if (isChecked)
		{
			OnLevelActivate(2);
		}
	}

	private void OnLevel3Activate(bool isChecked)
	{
		if (isChecked)
		{
			OnLevelActivate(3);
		}
	}

	private void OnLevel4Activate(bool isChecked)
	{
		if (isChecked)
		{
			OnLevelActivate(4);
		}
	}

	private void OnLevel5Activate(bool isChecked)
	{
		if (isChecked)
		{
			OnLevelActivate(5);
		}
	}

	private void OnLevel6Activate(bool isChecked)
	{
		if (isChecked)
		{
			OnLevelActivate(6);
		}
	}

	private void OnLevelActivate(int level)
	{
		this.level = level;
	}

	private void Update()
	{
	}

	public void Go(byte sceneID)
	{
		if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode(mode) && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetUserID(), InvitationRequest.Type.Arena, mode, sceneID, (short)level);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		else
		{
			Arena.GetInstance().Enter(mode, sceneID, level);
		}
	}
}

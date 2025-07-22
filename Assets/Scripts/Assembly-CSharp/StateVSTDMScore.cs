using UnityEngine;

public class StateVSTDMScore : MonoBehaviour
{
	public UILabel redScore;

	public UILabel blueScore;

	public GameObject redTeam;

	public GameObject blueTeam;

	public UIButtonX scoreButton;

	private void Start()
	{
		if (UserStateHUD.GetInstance().GetUserTeamName() == TeamName.Red)
		{
			redTeam.SetActive(true);
			blueTeam.SetActive(false);
		}
		else if (UserStateHUD.GetInstance().GetUserTeamName() == TeamName.Blue)
		{
			redTeam.SetActive(false);
			blueTeam.SetActive(true);
		}
		else
		{
			redTeam.SetActive(false);
			blueTeam.SetActive(false);
		}
		scoreButton.receiver = HUDManager.instance.m_HotKeyManager.gameObject;
	}

	private void Update()
	{
		redScore.text = string.Empty + UserStateHUD.GetInstance().GetVSBattleFieldState().RedTeam.Resource;
		blueScore.text = string.Empty + UserStateHUD.GetInstance().GetVSBattleFieldState().BlueTeam.Resource;
	}
}

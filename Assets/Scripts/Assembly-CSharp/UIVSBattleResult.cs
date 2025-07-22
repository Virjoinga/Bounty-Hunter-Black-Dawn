using System.Collections;
using UnityEngine;

public class UIVSBattleResult : UIDelegateMenu
{
	public enum Condition
	{
		InVS = 0,
		VSEnd = 1
	}

	public UIVSTeamManager redTeamManager;

	public UIVSTeamManager blueTeamManager;

	public GameObject backButton;

	public GameObject exitButton;

	public GameObject nextRoundInfo;

	private static UserStateHUD.VSUserTeam redTeam;

	private static UserStateHUD.VSUserTeam blueTeam;

	private static GameObject uiVSResult;

	private static Condition condition;

	private void Awake()
	{
		switch (condition)
		{
		case Condition.InVS:
			AddDelegate(backButton);
			exitButton.gameObject.SetActive(false);
			nextRoundInfo.gameObject.SetActive(false);
			break;
		case Condition.VSEnd:
			AddDelegate(exitButton);
			backButton.gameObject.SetActive(false);
			break;
		}
	}

	private void Start()
	{
		redTeamManager.UpdateTeam(redTeam);
		blueTeamManager.UpdateTeam(blueTeam);
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (go.Equals(backButton))
		{
			Close();
		}
		else if (go.Equals(exitButton))
		{
			GameApp.GetInstance().GetGameWorld().ExitMultiplayerMode();
		}
	}

	public static void ShowResult(UserStateHUD.VSUserTeam red, UserStateHUD.VSUserTeam blue, Condition con)
	{
		if (uiVSResult == null && red != null && blue != null)
		{
			redTeam = red;
			blueTeam = blue;
			condition = con;
			GameObject original = ResourceLoad.GetInstance().LoadUI("VS", "VSUIBattleResult");
			uiVSResult = Object.Instantiate(original) as GameObject;
		}
	}

	public static void Close()
	{
		if (uiVSResult != null)
		{
			MemoryManager.FreeNGUI(uiVSResult);
			uiVSResult = null;
		}
	}

	public static bool IsShow()
	{
		return false;
	}

	private void CountDownEnd()
	{
		StartCoroutine(DelayToCloseMenu());
	}

	private IEnumerator DelayToCloseMenu()
	{
		yield return new WaitForSeconds(2f);
		Close();
	}
}

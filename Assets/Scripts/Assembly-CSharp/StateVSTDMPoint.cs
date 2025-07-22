using System.Collections.Generic;
using UnityEngine;

public class StateVSTDMPoint : MonoBehaviour
{
	public GameObject pointContainer1;

	public GameObject pointContainer2;

	public GameObject pointBarContainer;

	public GameObject redCaptureIcon;

	public GameObject blueCaptureIcon;

	public GameObject redTeamIcon;

	public GameObject blueTeamIcon;

	public GameObject lockIcon;

	private VSTDMPoint[] mVSTDMPointList;

	private VSTDMPointBar[] mVSTDMPointBarList;

	private bool justEnterScene;

	private void Awake()
	{
		if (GameApp.GetInstance().GetGameWorld().IsVS1Scene())
		{
			mVSTDMPointList = pointContainer1.GetComponentsInChildren<VSTDMPoint>();
			pointContainer2.SetActive(false);
		}
		else
		{
			mVSTDMPointList = pointContainer2.GetComponentsInChildren<VSTDMPoint>();
			pointContainer1.SetActive(false);
		}
		mVSTDMPointBarList = pointBarContainer.GetComponentsInChildren<VSTDMPointBar>();
		redCaptureIcon.SetActive(false);
		blueCaptureIcon.SetActive(false);
		redTeamIcon.SetActive(false);
		blueTeamIcon.SetActive(false);
		lockIcon.SetActive(false);
		if (GameApp.GetInstance().GetGameWorld().Is1V1VSScene())
		{
			for (int i = 0; i < mVSTDMPointList.Length; i++)
			{
				if (i != 2)
				{
					mVSTDMPointList[i].disable = true;
				}
			}
			for (int j = 0; j < mVSTDMPointBarList.Length; j++)
			{
				mVSTDMPointBarList[j].disable = true;
			}
		}
		justEnterScene = true;
	}

	private void Update()
	{
		if (UserStateHUD.GetInstance().GetVSBattleFieldState().BlueTeam.Resource != 0 || UserStateHUD.GetInstance().GetVSBattleFieldState().RedTeam.Resource != 0)
		{
			if (justEnterScene && !EffectsCamera.instance.IsRunning())
			{
				justEnterScene = false;
				UserStateHUD.GetInstance().SetVSTDMCaptureSignVisible(UserStateHUD.GetInstance().GetUserTeamName());
			}
		}
		else
		{
			justEnterScene = false;
		}
		List<UserStateHUD.VSBattleFieldPoint> pointInfo = UserStateHUD.GetInstance().GetVSBattleFieldState().PointInfo;
		for (int i = 0; i < pointInfo.Count; i++)
		{
			mVSTDMPointList[i].SetPoint(pointInfo[i], UserStateHUD.GetInstance().GetVSBattleFieldState().NewBattle);
			if (i < pointInfo.Count - 1)
			{
				mVSTDMPointBarList[i].SetStatus(pointInfo[i], pointInfo[i + 1]);
			}
		}
		switch (UserStateHUD.GetInstance().GetVSTDMCaptureSignVisible())
		{
		case TeamName.Red:
			redCaptureIcon.SetActive(true);
			break;
		case TeamName.Blue:
			blueCaptureIcon.SetActive(true);
			break;
		}
		switch (UserStateHUD.GetInstance().GetVSTDMTeamSignVisible())
		{
		case TeamName.Red:
			redTeamIcon.SetActive(true);
			break;
		case TeamName.Blue:
			blueTeamIcon.SetActive(true);
			break;
		}
		if (UserStateHUD.GetInstance().GetVSTDMLockVisible())
		{
			lockIcon.SetActive(true);
		}
	}
}

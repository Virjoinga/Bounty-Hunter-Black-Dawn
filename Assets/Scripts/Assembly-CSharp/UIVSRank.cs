using System.Collections.Generic;
using UnityEngine;

public class UIVSRank : MonoBehaviour, UIMsgListener
{
	public UIGrid rankGrid;

	public UIVSUserInfoManager rankInfoManager;

	public UIVSUserInfo localPlayerRank;

	private void Start()
	{
		List<UserStateHUD.VSUserState> list = new List<UserStateHUD.VSUserState>();
		List<VSTDMRank> vsTDMRank = StatisticsManager.m_vsTDMRank;
		for (int i = 0; i < vsTDMRank.Count; i++)
		{
			UserStateHUD.VSUserState vSUserState = TransformTo(vsTDMRank[i]);
			vSUserState.Rank = i + 1;
			list.Add(vSUserState);
		}
		rankInfoManager.UpdatePlayer(list);
		rankGrid.repositionNow = true;
		VSTDMRank vSTDMRank = (VSTDMRank)GameApp.GetInstance().GetUserState().m_statistics[0];
		UserStateHUD.VSUserState vSUserState2 = TransformTo(vSTDMRank);
		bool flag = false;
		for (int j = 0; j < vsTDMRank.Count; j++)
		{
			if (vSTDMRank.Id == vsTDMRank[j].Id)
			{
				vSUserState2.Rank = j + 1;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			if (vsTDMRank.Count < 20)
			{
				vSUserState2.Rank = 2000;
			}
			else
			{
				int vsTDMRankCount = StatisticsManager.m_vsTDMRankCount;
				if (vsTDMRankCount == 0)
				{
					vSUserState2.Rank = 0;
				}
				else if (vSTDMRank.Score >= vsTDMRank[19].Score)
				{
					int num = 0;
					int num2 = 19;
					int num3 = (num + num2) / 2;
					while (num != num2)
					{
						num3 = (num + num2) / 2;
						if (vSTDMRank.Score >= vsTDMRank[num3].Score)
						{
							num2 = num3;
						}
						if (vSTDMRank.Score <= vsTDMRank[num3].Score)
						{
							num = num3;
						}
						if (num2 - num == 1)
						{
							break;
						}
					}
					vSUserState2.Rank = num3 + 1;
				}
				else
				{
					Debug.Log("totalPlayerCount = " + vsTDMRankCount + "  rank.Score = " + vSTDMRank.Score + "  rankList[19].Score = " + vsTDMRank[19].Score);
					vSUserState2.Rank = Mathf.RoundToInt((float)(vsTDMRankCount - 20) * (1f - (float)vSTDMRank.Score / (float)vsTDMRank[19].Score)) + 20;
				}
			}
		}
		localPlayerRank.UpdateInfo(vSUserState2);
		if (vSTDMRank.Bonus > 0)
		{
			GameApp.GetInstance().GetGlobalState().AddMithril(vSTDMRank.Bonus);
			GameApp.GetInstance().Save();
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("LOC_PVP_BONUS").Replace("%d", string.Empty + vSTDMRank.Bonus), 2);
			vSTDMRank.Bonus = 0;
		}
	}

	public UserStateHUD.VSUserState TransformTo(VSTDMRank rank)
	{
		UserStateHUD.VSUserState vSUserState = new UserStateHUD.VSUserState();
		vSUserState.ClassSpriteName = UserStateHUD.GetInstance().GetUserClassIcon(rank.CharacterClass);
		vSUserState.Name = rank.Name;
		vSUserState.Win = rank.Win;
		vSUserState.Lost = rank.Lost;
		vSUserState.Offline = rank.Offline;
		vSUserState.Bonus = rank.Score;
		return vSUserState;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}
}

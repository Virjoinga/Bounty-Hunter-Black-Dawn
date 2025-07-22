using UnityEngine;

public class SkillTreeConfirmButtonScript : MonoBehaviour
{
	protected const int LAYER = 4;

	private void OnClick()
	{
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		if (skillTreeManager.GetTotalPreAddPoints() == 0)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (i != 3)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < skillTreeManager.PreAddPoints[i, j]; k++)
					{
						skillTreeManager.GivePoint(i, j, skillTreeManager.PreAddSkillIDs[i, j]);
					}
				}
			}
			else
			{
				skillTreeManager.GivePoint(i, 0, skillTreeManager.PreAddFinalSkillID);
			}
		}
		skillTreeManager.ClearPreAddPoints();
		SkillTreeUIScript.mInstance.gameObject.BroadcastMessage("RefreshSkillTreeButton", true);
		SkillTreeUIScript.mInstance.gameObject.BroadcastMessage("GetSelfImageButtonIcon");
		SkillTreeUIScript.mInstance.RefreshSkillTree();
		SkillTreeUIScript.mInstance.Description.GetComponentInChildren<UILabel>().text = string.Empty;
		SkillTreeUIScript.mInstance.HideDescription();
		AudioManager.GetInstance().PlaySound("RPG_Audio/Player/player_skillup");
		AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.First_Gift, AchievementTrigger.Type.Data);
		AchievementManager.GetInstance().Trigger(trigger);
		skillTreeManager.CheckAchievement009();
	}
}

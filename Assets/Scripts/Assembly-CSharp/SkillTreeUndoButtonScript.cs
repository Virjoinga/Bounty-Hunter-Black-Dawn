using UnityEngine;

public class SkillTreeUndoButtonScript : MonoBehaviour
{
	private void OnClick()
	{
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		skillTreeManager.ClearPreAddPoints();
		SkillTreeUIScript.mInstance.gameObject.BroadcastMessage("RefreshSkillTreeButton", true);
		SkillTreeUIScript.mInstance.gameObject.BroadcastMessage("GetSelfImageButtonIcon");
		SkillTreeUIScript.mInstance.RefreshSkillTree();
		SkillTreeUIScript.mInstance.HideDescription();
		SkillTreeUIScript.mInstance.Description.GetComponentInChildren<UILabel>().text = string.Empty;
	}
}

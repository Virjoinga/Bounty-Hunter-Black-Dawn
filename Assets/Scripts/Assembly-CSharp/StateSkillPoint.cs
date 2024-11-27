using UnityEngine;

public class StateSkillPoint : MonoBehaviour
{
	public UITweenX tweenAll;

	public UITweenX tweenIcon;

	public UITweenX tweenNumber;

	private bool hasSkillPoint;

	private int lastSkillPoint;

	private void Enable()
	{
		lastSkillPoint = 0;
	}

	private void Update()
	{
		int skillPointsLeft = GameApp.GetInstance().GetUserState().SkillTreeManager.GetSkillPointsLeft();
		int num = skillPointsLeft - lastSkillPoint;
		if (num > 0)
		{
			tweenAll.PlayForward();
			lastSkillPoint = GameApp.GetInstance().GetUserState().SkillTreeManager.GetSkillPointsLeft();
		}
		else if (num < 0)
		{
			lastSkillPoint = GameApp.GetInstance().GetUserState().SkillTreeManager.GetSkillPointsLeft();
		}
		if (skillPointsLeft > 0 && !hasSkillPoint)
		{
			hasSkillPoint = true;
			tweenIcon.PlayForward();
			tweenNumber.PlayForward();
		}
		else if (skillPointsLeft == 0 && hasSkillPoint)
		{
			hasSkillPoint = false;
			tweenIcon.PlayReverse();
			tweenNumber.PlayReverse();
		}
	}
}

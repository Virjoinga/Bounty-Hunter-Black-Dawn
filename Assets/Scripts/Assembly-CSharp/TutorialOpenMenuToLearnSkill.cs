using UnityEngine;

public class TutorialOpenMenuToLearnSkill : TutorialScript
{
	private UITweenX tween;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			GameObject skillPoint = HUDManager.instance.m_HotKeyManager.m_SkillPoint;
			tween = skillPoint.GetComponent<StateSkillPoint>().tweenAll;
			tween.PlayForward(Play);
		}
	}

	private void Play()
	{
		if (!IsTutorialOk(TutorialManager.TutorialType.LearnSkill))
		{
			tween.PlayForward(Play);
		}
	}

	protected override void OnTutorialUpdate()
	{
		if (HUDManager.instance != null && HUDManager.instance.gameObject != null && !HUDManager.instance.gameObject.activeSelf)
		{
			PauseTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.OpenMenuToLearnSkill;
	}

	protected override bool IsTutorialCanStart()
	{
		if (IsTutorialOk(TutorialManager.TutorialType.LearnSkill))
		{
			EndTutorial();
			return false;
		}
		if (HUDManager.instance == null)
		{
			return false;
		}
		return HUDManager.instance.gameObject.activeSelf && GameApp.GetInstance().GetUserState().SkillTreeManager.GetSkillPointsLeft() > 0 && !IsTutorialOk(TutorialManager.TutorialType.LearnSkill);
	}
}

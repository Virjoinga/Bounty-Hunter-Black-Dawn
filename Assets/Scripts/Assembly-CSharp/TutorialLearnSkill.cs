public class TutorialLearnSkill : TutorialScript
{
	public IconFlash lightFrame;

	public IconFlash fingerIcon;

	protected override void OnTutorialStart()
	{
		InGameMenuManager.GetInstance().Lock = true;
		StateBar.instance.Lock = true;
	}

	protected override void OnTutorialEnd()
	{
		InGameMenuManager.GetInstance().Lock = false;
		StateBar.instance.Lock = false;
	}

	protected override void OnTutorialUpdate()
	{
		EndTutorial();
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.LearnSkill;
	}

	protected override bool IsTutorialCanStart()
	{
		return !IsTutorialOk(TutorialManager.TutorialType.LearnSkill) && InGameMenu.CurrentIndex == 3 && CloseButton.instance != null && GameApp.GetInstance().GetUserState().SkillTreeManager.GetSkillPointsLeft() > 0;
	}
}

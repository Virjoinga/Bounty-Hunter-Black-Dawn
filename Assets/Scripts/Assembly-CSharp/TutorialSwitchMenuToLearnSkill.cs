using UnityEngine;

public class TutorialSwitchMenuToLearnSkill : TutorialScript, InGameMenuButtonListener
{
	public IconFlash fingerIcon;

	public Transform m_InitPos;

	public Transform m_FinalPos;

	private bool isMove;

	protected override void OnTutorialStart()
	{
		InGameMenu.instance.SetListener(this);
		InGameMenuManager.GetInstance().Lock = true;
		InGameMenu.instance.Lock(1, 2, 4, 5, 99);
		StateBar.instance.Lock = true;
		NGUIBackPackUIScript.mInstance.SetBackPackLockState(true);
		isMove = false;
	}

	protected override void OnTutorialEnd()
	{
		InGameMenu.instance.RemoveListener();
		InGameMenuManager.GetInstance().Lock = false;
		InGameMenu.instance.Unlock();
		StateBar.instance.Lock = false;
		NGUIBackPackUIScript.mInstance.SetBackPackLockState(false);
	}

	protected override void OnTutorialUpdate()
	{
		if (isMove)
		{
			fingerIcon.transform.localPosition = Vector3.MoveTowards(fingerIcon.transform.localPosition, m_FinalPos.localPosition, 2f);
			if (Vector3.Distance(fingerIcon.transform.localPosition, m_FinalPos.localPosition) < 1f)
			{
				fingerIcon.transform.localPosition = m_InitPos.localPosition;
			}
			if (InGameMenu.instance != null && !InGameMenu.instance.IsSpread())
			{
				ResumeSplash();
			}
		}
	}

	public void OnButtonEvent(UIButtonX.ButtonInfo info)
	{
		if (info.buttonId == 0 && info.buttonEvent == UIButtonX.ButtonInfo.Event.Pressing)
		{
			PauseSplash();
		}
		else if ((info.buttonEvent == UIButtonX.ButtonInfo.Event.Click || info.buttonEvent == UIButtonX.ButtonInfo.Event.Drop) && info.buttonId == 3)
		{
			ResumeSplash();
			EndTutorial();
		}
	}

	private void PauseSplash()
	{
		fingerIcon.Pause();
		fingerIcon.Refresh();
		isMove = true;
	}

	private void ResumeSplash()
	{
		fingerIcon.Resume();
		isMove = false;
		fingerIcon.transform.localPosition = m_InitPos.localPosition;
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.LearnSkill;
	}

	protected override bool IsTutorialCanStart()
	{
		return false;
	}
}

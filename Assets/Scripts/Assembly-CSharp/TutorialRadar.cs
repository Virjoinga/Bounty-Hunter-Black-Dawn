using UnityEngine;

public class TutorialRadar : TutorialScript, UIMsgListener
{
	public UITweenX enemyTween;

	public UITweenX questTween;

	public UITweenX remotePlayerTween;

	public UILabel label;

	private bool bTouch;

	private UITweenX currentTween;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
			HUDManager.instance.m_HotKeyManager.ForbidAll();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			label.text = LocalizationManager.GetInstance().GetString("TUTOR_HINT_ENEMY_MARK");
			bTouch = false;
			questTween.gameObject.SetActive(false);
			remotePlayerTween.gameObject.SetActive(false);
			currentTween = enemyTween;
		}
	}

	protected override void OnTutorialUpdate()
	{
		if (bTouch)
		{
			return;
		}
		if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			if (Input.GetMouseButtonUp(0))
			{
				currentTween.PlayForward(PlayEnd);
			}
		}
		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			currentTween.PlayForward(PlayEnd);
		}
	}

	protected override void OnTutorialEnd()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = false;
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = false;
		}
	}

	private void PlayEnd()
	{
		if (currentTween == enemyTween)
		{
			currentTween = questTween;
			currentTween.gameObject.SetActive(true);
			label.text = LocalizationManager.GetInstance().GetString("TUTOR_HINT_MISSION_MARK");
		}
		else if (currentTween == questTween)
		{
			currentTween = remotePlayerTween;
			currentTween.gameObject.SetActive(true);
			label.text = LocalizationManager.GetInstance().GetString("TUTOR_HINT_TEAMMATE_MARK");
		}
		else if (currentTween == remotePlayerTween)
		{
			bTouch = true;
			label.gameObject.SetActive(false);
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.Rader;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.Map) && !IsTutorialOk(TutorialManager.TutorialType.Rader) && UserStateHUD.GetInstance().GetEnemyList().Count > 0 && CloseButton.instance == null;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 52)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				GameApp.GetInstance().GetGlobalState().AddMithril(100);
				GameApp.GetInstance().Save();
				EndTutorial();
			}
			UIMsgBox.instance.CloseMessage();
		}
	}
}

using UnityEngine;

public class TutorialRotateCamera : TutorialScript
{
	private Vector3 initForward;

	private float angle;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.CanFire = false;
			HUDManager.instance.m_HotKeyManager.ForbidAll();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			initForward = Camera.main.transform.forward;
			angle = 0f;
		}
	}

	protected override void OnTutorialEnd()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.CanFire = true;
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = false;
		}
	}

	protected override void OnTutorialUpdate()
	{
		Vector3 forward = Camera.main.transform.forward;
		angle += Vector3.Angle(forward, initForward);
		initForward = forward;
		if (angle > 60f)
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.RotateCamera;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.Move) && !IsTutorialOk(TutorialManager.TutorialType.RotateCamera);
	}
}

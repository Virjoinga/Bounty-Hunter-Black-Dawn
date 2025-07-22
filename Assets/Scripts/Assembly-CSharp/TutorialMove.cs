using UnityEngine;

public class TutorialMove : TutorialScript
{
	private Vector3 initPos;

	private float dis;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.RotateCamera = false;
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.CanFire = false;
			HUDManager.instance.m_HotKeyManager.ForbidAll();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			initPos = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition();
			dis = 0f;
		}
	}

	protected override void OnTutorialEnd()
	{
		if (HUDManager.instance != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.RotateCamera = true;
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.CanFire = true;
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = false;
		}
	}

	protected override void OnTutorialUpdate()
	{
		Vector3 position = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetPosition();
		dis += Vector3.Distance(position, initPos);
		initPos = position;
		if (dis > 6f)
		{
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.Move;
	}

	protected override bool IsTutorialCanStart()
	{
		return !IsTutorialOk(TutorialManager.TutorialType.Move);
	}
}

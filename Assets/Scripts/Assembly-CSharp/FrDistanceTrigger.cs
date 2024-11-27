using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("GameEvent")]
[HutongGames.PlayMaker.Tooltip("Custom Action...")]
public class FrDistanceTrigger : FsmStateAction
{
	[RequiredField]
	public FsmFloat distance;

	[HutongGames.PlayMaker.Tooltip("The Target.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	public FsmEvent sendEvent;

	private Transform playerTransform;

	public override void Reset()
	{
		gameObject = null;
		sendEvent = null;
		playerTransform = null;
	}

	public override void OnEnter()
	{
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		playerTransform = localPlayer.GetTransform();
		if (playerTransform != null)
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != 0) ? this.gameObject.GameObject.Value : base.Owner);
			Vector3 position = gameObject.transform.position;
			if (!(Vector3.Distance(playerTransform.position, position) <= distance.Value))
			{
			}
		}
	}
}

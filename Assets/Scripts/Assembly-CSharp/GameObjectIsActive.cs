using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("GameEvent")]
public class GameObjectIsActive : FsmStateAction
{
	[UIHint(UIHint.Variable)]
	[RequiredField]
	public FsmGameObject gameObject;

	public FsmEvent isActive;

	public FsmEvent isNotActive;

	[UIHint(UIHint.Variable)]
	public FsmBool storeResult;

	public bool everyFrame;

	private float mLastUpdateCheckTime;

	private float mStartTime;

	public override void Reset()
	{
		gameObject = null;
		isActive = null;
		isNotActive = null;
		storeResult = null;
		everyFrame = false;
		mLastUpdateCheckTime = -999f;
		mStartTime = Time.time;
	}

	public override void OnEnter()
	{
		mLastUpdateCheckTime = -999f;
		mStartTime = Time.time;
		DoIsGameObjectActive();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		if (Time.time - mStartTime > 3f)
		{
			base.Fsm.Event(isNotActive);
		}
		DoIsGameObjectActive();
	}

	private void DoIsGameObjectActive()
	{
		if (Time.time - mLastUpdateCheckTime > 0.3f)
		{
			mLastUpdateCheckTime = Time.time;
			bool active = gameObject.Value.active;
			if (storeResult != null)
			{
				storeResult.Value = active;
			}
			if (active)
			{
				base.Fsm.Event(isActive);
			}
		}
	}
}

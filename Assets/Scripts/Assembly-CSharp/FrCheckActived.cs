using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("GameEvent")]
[HutongGames.PlayMaker.Tooltip("Custion Action...")]
public class FrCheckActived : FsmStateAction
{
	[RequiredField]
	[UIHint(UIHint.Variable)]
	private GameObject gameObject;

	public FsmEvent isActive;

	public FsmEvent isNotActive;

	public override void Reset()
	{
		gameObject = null;
		isActive = null;
		isNotActive = null;
	}

	public override void OnEnter()
	{
		DoIsGameObjectActive();
		gameObject = base.Owner.GetComponent<ExplorItemList>().EffectObject;
	}

	private void DoIsGameObjectActive()
	{
		if (gameObject == null)
		{
			gameObject = base.Owner.GetComponent<ExplorItemList>().EffectObject;
		}
		if (gameObject.active)
		{
			base.Fsm.Event(isActive);
		}
		else
		{
			base.Fsm.Event(isNotActive);
		}
	}
}

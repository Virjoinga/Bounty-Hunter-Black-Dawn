using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Enables/Disables a Behaviour on a Game Object. Optionally reset the Behaviour on exit - useful if you only want the Behaviour to be active while this state is active.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class EnableBehaviour : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Behaviour)]
		public FsmString behaviour;

		[Tooltip("Optionally drag a component directly into this field")]
		public Component component;

		[RequiredField]
		public FsmBool enable;

		public FsmBool resetOnExit;

		private Behaviour componentTarget;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			component = null;
			enable = true;
			resetOnExit = true;
		}

		public override void OnEnter()
		{
			DoEnableBehaviour(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void DoEnableBehaviour(GameObject go)
		{
			if (!(go == null))
			{
				if (component != null)
				{
					componentTarget = component as Behaviour;
				}
				else
				{
					componentTarget = go.GetComponent(behaviour.Value) as Behaviour;
				}
				if (componentTarget == null)
				{
					LogWarning(" " + go.name + " missing behaviour: " + behaviour.Value);
				}
				else
				{
					componentTarget.enabled = enable.Value;
				}
			}
		}

		public override void OnExit()
		{
			if (!(componentTarget == null) && resetOnExit.Value)
			{
				componentTarget.enabled = !enable.Value;
			}
		}
	}
}

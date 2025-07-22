using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Invokes a Method in a Behaviour attached to a Game Object. See Unity InvokeMethod docs.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class InvokeMethod : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Script)]
		[RequiredField]
		public FsmString behaviour;

		[RequiredField]
		[UIHint(UIHint.Method)]
		public FsmString methodName;

		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		public FsmBool repeating;

		[HasFloatSlider(0f, 10f)]
		public FsmFloat repeatDelay;

		public FsmBool cancelOnExit;

		private MonoBehaviour component;

		public override void Reset()
		{
			gameObject = null;
			behaviour = null;
			methodName = string.Empty;
			delay = null;
			repeating = false;
			repeatDelay = 1f;
			cancelOnExit = false;
		}

		public override void OnEnter()
		{
			DoInvokeMethod(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void DoInvokeMethod(GameObject go)
		{
			if (!(go == null))
			{
				component = go.GetComponent(behaviour.Value) as MonoBehaviour;
				if (component == null)
				{
					LogWarning("InvokeMethod: " + go.name + " missing behaviour: " + behaviour.Value);
				}
				else if (repeating.Value)
				{
					component.InvokeRepeating(methodName.Value, delay.Value, repeatDelay.Value);
				}
				else
				{
					component.Invoke(methodName.Value, delay.Value);
				}
			}
		}

		public override void OnExit()
		{
			if (!(component == null) && cancelOnExit.Value)
			{
				component.CancelInvoke(methodName.Value);
			}
		}
	}
}

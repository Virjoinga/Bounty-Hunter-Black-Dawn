using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets or toggle the visibility on a game object.")]
	public class SetVisibility : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Should the object visibility be toggled?\nHas priority over the 'visible' setting")]
		public FsmBool toggle;

		[Tooltip("Should the object be set to visible or invisible?")]
		public FsmBool visible;

		[Tooltip("Resets to the initial visibility once\nit leaves the state")]
		public bool resetOnExit;

		private bool initialVisibility;

		public override void Reset()
		{
			gameObject = null;
			toggle = false;
			visible = false;
			resetOnExit = true;
			initialVisibility = false;
		}

		public override void OnEnter()
		{
			DoSetVisibility(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void DoSetVisibility(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			if (go.GetComponent<Renderer>() == null)
			{
				LogError("Missing Renderer!");
				return;
			}
			initialVisibility = go.GetComponent<Renderer>().isVisible;
			if (!toggle.Value)
			{
				go.GetComponent<Renderer>().enabled = visible.Value;
			}
			else
			{
				go.GetComponent<Renderer>().enabled = !go.GetComponent<Renderer>().isVisible;
			}
		}

		public override void OnExit()
		{
			if (resetOnExit)
			{
				ResetVisibility();
			}
		}

		private void ResetVisibility()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null && ownerDefaultTarget.GetComponent<Renderer>() != null)
			{
				ownerDefaultTarget.GetComponent<Renderer>().enabled = initialVisibility;
			}
		}
	}
}

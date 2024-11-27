using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Enables/Disables an FSM component on a Game Object.\nOptionally reverse the action on exit.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class EnableFSM : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object")]
		public FsmString fsmName;

		public FsmBool enable;

		public FsmBool resetOnExit;

		private PlayMakerFSM fsmComponent;

		public override void Reset()
		{
			gameObject = null;
			fsmName = string.Empty;
			enable = true;
			resetOnExit = true;
		}

		public override void OnEnter()
		{
			DoEnableFSM();
			Finish();
		}

		private void DoEnableFSM()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != 0) ? this.gameObject.GameObject.Value : base.Owner);
			if (gameObject == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(fsmName.Value))
			{
				PlayMakerFSM[] components = gameObject.GetComponents<PlayMakerFSM>();
				PlayMakerFSM[] array = components;
				foreach (PlayMakerFSM playMakerFSM in array)
				{
					if (playMakerFSM.FsmName == fsmName.Value)
					{
						fsmComponent = playMakerFSM;
						break;
					}
				}
			}
			else
			{
				fsmComponent = gameObject.GetComponent<PlayMakerFSM>();
			}
			if (fsmComponent == null)
			{
				LogError("Missing FsmComponent!");
			}
			else
			{
				fsmComponent.enabled = enable.Value;
			}
		}

		public override void OnExit()
		{
			if (!(fsmComponent == null) && resetOnExit.Value)
			{
				fsmComponent.enabled = !enable.Value;
			}
		}
	}
}

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Mass of a Game Object's Rigid Body.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetMass : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[HasFloatSlider(0.1f, 10f)]
		public FsmFloat mass;

		public override void Reset()
		{
			gameObject = null;
			mass = 1f;
		}

		public override void OnEnter()
		{
			DoSetMass();
			Finish();
		}

		private void DoSetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null) && !(ownerDefaultTarget.GetComponent<Rigidbody>() == null))
			{
				ownerDefaultTarget.GetComponent<Rigidbody>().mass = mass.Value;
			}
		}
	}
}

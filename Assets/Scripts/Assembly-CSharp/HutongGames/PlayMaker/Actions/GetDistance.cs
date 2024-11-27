using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Measures the Distance betweens 2 Game Objects and stores the result in a Float Variable.")]
	public class GetDistance : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmGameObject target;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			target = null;
			storeResult = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoGetDistance();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetDistance();
		}

		private void DoGetDistance()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != 0) ? this.gameObject.GameObject.Value : base.Owner);
			if (!(gameObject == null) && !(target.Value == null) && storeResult != null)
			{
				storeResult.Value = Vector3.Distance(gameObject.transform.position, target.Value.transform.position);
			}
		}
	}
}

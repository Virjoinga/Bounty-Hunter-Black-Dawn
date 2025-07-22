using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Applies an explosion Force to all Game Objects with a Rigid Body inside a Radius.")]
	[ActionCategory(ActionCategory.Physics)]
	public class Explosion : FsmStateAction
	{
		[RequiredField]
		public FsmVector3 center;

		[RequiredField]
		public FsmFloat force;

		[RequiredField]
		public FsmFloat radius;

		public FsmFloat upwardsModifier;

		public ForceMode forceMode;

		[UIHint(UIHint.Layer)]
		public FsmInt layer;

		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		public bool everyFrame;

		public override void Reset()
		{
			center = null;
			upwardsModifier = 0f;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoExplosion();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoExplosion();
		}

		private void DoExplosion()
		{
			Collider[] array = Physics.OverlapSphere(center.Value, radius.Value);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				if (collider.GetComponent<Rigidbody>() != null && ShouldApplyForce(collider.gameObject))
				{
					collider.GetComponent<Rigidbody>().AddExplosionForce(force.Value, center.Value, radius.Value, upwardsModifier.Value, forceMode);
				}
			}
		}

		private bool ShouldApplyForce(GameObject go)
		{
			int num = ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value);
			return ((1 << go.layer) & num) > 0;
		}
	}
}

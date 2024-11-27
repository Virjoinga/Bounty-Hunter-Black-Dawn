namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Substance")]
	[Tooltip("Set a named float property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	public class SetProceduralFloat : FsmStateAction
	{
		[RequiredField]
		public FsmMaterial substanceMaterial;

		[RequiredField]
		public FsmString floatProperty;

		[RequiredField]
		public FsmFloat floatValue;

		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;

		public override void Reset()
		{
			substanceMaterial = null;
			floatProperty = string.Empty;
			floatValue = 0f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetProceduralFloat();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetProceduralFloat();
		}

		private void DoSetProceduralFloat()
		{
		}
	}
}

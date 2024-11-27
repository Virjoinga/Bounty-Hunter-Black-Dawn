namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set a named bool property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	[ActionCategory("Substance")]
	public class SetProceduralBoolean : FsmStateAction
	{
		[RequiredField]
		public FsmMaterial substanceMaterial;

		[RequiredField]
		public FsmString boolProperty;

		[RequiredField]
		public FsmBool boolValue;

		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;

		public override void Reset()
		{
			substanceMaterial = null;
			boolProperty = string.Empty;
			boolValue = false;
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

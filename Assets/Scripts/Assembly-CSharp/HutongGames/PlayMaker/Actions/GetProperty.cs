namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the value of any public property or field on the targeted Unity Object and stores it in a variable. E.g., Drag and drop any component attached to a Game Object to access its properties.")]
	[ActionCategory(ActionCategory.UnityObject)]
	public class GetProperty : FsmStateAction
	{
		public FsmProperty targetProperty;

		public bool everyFrame;

		public override void Reset()
		{
			targetProperty = new FsmProperty();
			everyFrame = false;
		}

		public override void OnEnter()
		{
			targetProperty.GetValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			targetProperty.GetValue();
		}
	}
}

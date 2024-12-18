namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Placeholder for a missing action.\n\nUsually generated when the editor can't load an action, e.g., if the script was deleted, but can also be used as a TODO note.")]
	[ActionCategory(ActionCategory.Debug)]
	public class MissingAction : FsmStateAction
	{
		[Tooltip("The name of the missing action.")]
		public string actionName;
	}
}

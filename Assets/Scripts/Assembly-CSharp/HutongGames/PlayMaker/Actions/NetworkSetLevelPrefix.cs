using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the level prefix which will then be prefixed to all network ViewID numbers.\n\nThis prevents old network updates from straying into a new level from the previous level.\n\nThis can be set to any number and then incremented with each new level load. This doesn't add overhead to network traffic but just diminishes the pool of network ViewID numbers a little bit.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkSetLevelPrefix : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The level prefix which will then be prefixed to all network ViewID numbers.")]
		public FsmInt levelPrefix;

		public override void Reset()
		{
			levelPrefix = null;
		}

		public override void OnEnter()
		{
			if (levelPrefix.IsNone)
			{
				LogError("Network LevelPrefix not set");
				return;
			}
			Network.SetLevelPrefix(levelPrefix.Value);
			Finish();
		}
	}
}

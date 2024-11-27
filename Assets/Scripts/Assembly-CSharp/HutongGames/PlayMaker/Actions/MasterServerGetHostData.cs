using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get host data from the master server.")]
	[ActionCategory(ActionCategory.Network)]
	public class MasterServerGetHostData : FsmStateAction
	{
		[Tooltip("The index into the MasterServer Host List")]
		[RequiredField]
		public FsmInt hostIndex;

		[UIHint(UIHint.Variable)]
		[Tooltip("Does this server require NAT punchthrough?")]
		public FsmBool useNat;

		[UIHint(UIHint.Variable)]
		[Tooltip("The type of the game (e.g., 'MyUniqueGameType')")]
		public FsmString gameType;

		[Tooltip("The name of the game (e.g., 'John Does's Game')")]
		[UIHint(UIHint.Variable)]
		public FsmString gameName;

		[UIHint(UIHint.Variable)]
		[Tooltip("Currently connected players")]
		public FsmInt connectedPlayers;

		[UIHint(UIHint.Variable)]
		[Tooltip("Maximum players limit")]
		public FsmInt playerLimit;

		[Tooltip("Server IP address.")]
		[UIHint(UIHint.Variable)]
		public FsmString ipAddress;

		[Tooltip("Server port")]
		[UIHint(UIHint.Variable)]
		public FsmInt port;

		[Tooltip("Does the server require a password?")]
		[UIHint(UIHint.Variable)]
		public FsmBool passwordProtected;

		[UIHint(UIHint.Variable)]
		[Tooltip("A miscellaneous comment (can hold data)")]
		public FsmString comment;

		[UIHint(UIHint.Variable)]
		[Tooltip("The GUID of the host, needed when connecting with NAT punchthrough.")]
		public FsmString guid;

		public override void Reset()
		{
			hostIndex = null;
			useNat = null;
			gameType = null;
			gameName = null;
			connectedPlayers = null;
			playerLimit = null;
			ipAddress = null;
			port = null;
			passwordProtected = null;
			comment = null;
			guid = null;
		}

		public override void OnEnter()
		{
			GetHostData();
			Finish();
		}

		private void GetHostData()
		{
			int num = MasterServer.PollHostList().Length;
			int value = hostIndex.Value;
			if (value < 0 || value >= num)
			{
				LogError("MasterServer Host index out of range!");
				return;
			}
			HostData hostData = MasterServer.PollHostList()[value];
			if (hostData == null)
			{
				LogError("MasterServer HostData could not found at index " + value);
				return;
			}
			useNat.Value = hostData.useNat;
			gameType.Value = hostData.gameType;
			gameName.Value = hostData.gameName;
			connectedPlayers.Value = hostData.connectedPlayers;
			playerLimit.Value = hostData.playerLimit;
			ipAddress.Value = hostData.ip[0];
			port.Value = hostData.port;
			passwordProtected.Value = hostData.passwordProtected;
			comment.Value = hostData.comment;
			guid.Value = hostData.guid;
		}
	}
}

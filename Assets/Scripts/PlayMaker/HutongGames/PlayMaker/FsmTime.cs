using UnityEngine;

namespace HutongGames.PlayMaker
{
	public static class FsmTime
	{
		private static bool realtimeStarted;

		private static float playerPausedTime;

		private static float realtimeLastUpdate;

		private static int lastFrameCount;

		public static float RealtimeSinceStartup
		{
			get
			{
				if (realtimeStarted)
				{
					return Time.realtimeSinceStartup - playerPausedTime;
				}
				playerPausedTime = Time.realtimeSinceStartup;
				realtimeStarted = true;
				return 0f;
			}
		}

		public static void RealtimeBugFix()
		{
			realtimeStarted = true;
		}

		public static void Update()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (Time.frameCount == lastFrameCount)
			{
				playerPausedTime += realtimeSinceStartup - realtimeLastUpdate;
			}
			lastFrameCount = Time.frameCount;
			realtimeLastUpdate = realtimeSinceStartup;
		}
	}
}

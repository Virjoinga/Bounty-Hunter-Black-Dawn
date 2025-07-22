using UnityEngine;

public class AndroidAdsPluginScript
{
	public static void CallFlurryAds()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CallFlurryAds");
		}
		else
		{
			Debug.Log("CallFlurryAds");
		}
	}

	public static void CallTapjoyOfferWall()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CallTapjoyOfferWall");
		}
		else
		{
			Debug.Log("CallTapjoyOfferWall");
		}
	}

	public static void GetTapJoyPoints()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("GetTapJoyPoints");
		}
		else
		{
			Debug.Log("GetTapJoyPoints");
		}
	}

	public static void CallSponsorPayOfferWall()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CallSponsorPayOfferWall");
		}
		else
		{
			Debug.Log("CallSponsorPayOfferWall");
		}
	}

	public static void CallAdcolonyVideo()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CallAdcolonyVideo");
		}
		else
		{
			Debug.Log("CallAdcolonyVideo");
		}
	}

	public static void CallChartboostAds()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("ChartboostAds");
		}
		else
		{
			Debug.Log("ChartboostAds");
		}
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}
}

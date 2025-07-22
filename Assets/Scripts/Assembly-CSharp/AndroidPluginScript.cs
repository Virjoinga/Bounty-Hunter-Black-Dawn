using UnityEngine;

public class AndroidPluginScript
{
	public static string GetLanguage()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetLanguage", new object[0]);
		}
		return string.Empty;
	}

	public static string GetMacAddress()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetMacAddress", new object[0]);
		}
		return string.Empty;
	}

	public static string GetAndroidId()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetAndroidId", new object[0]);
		}
		return string.Empty;
	}

	public static void ShowFreyrGames(string bundleId, int type)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("ShowFreyrGames", bundleId, type);
		}
		else
		{
			Debug.Log("ShowFreyrGames = " + bundleId);
		}
	}

	public static void GetRestorePurchse()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("GetRestorePurchse");
		}
		else
		{
			Debug.Log("GetRestorePurchse");
		}
	}

	public static void DoStartAdsMethod()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("DoStartAdsMethod");
		}
		else
		{
			Debug.Log("DoStartAdsMethod");
		}
	}

	public static void DoStart()
	{
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			AndroidAdsPluginScript.GetTapJoyPoints();
			CheckIap();
			break;
		case AndroidConstant.Version.Kindle:
			GetRestorePurchse();
			break;
		}
	}

	public static void ShowCallOfArena()
	{
		string empty = string.Empty;
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			empty = AndroidConstant.URL_CALLOFARENA_GOOGLEPLAY;
			Application.OpenURL(empty);
			break;
		case AndroidConstant.Version.Kindle:
			OpenAwsUrl(AndroidConstant.BUNDLEID_CALLOFARENA_KINDLE);
			break;
		}
	}

	public static void ShowStarWarfare()
	{
		string empty = string.Empty;
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			empty = AndroidConstant.URL_STARWARFARE_GOOGLEPLAY;
			Application.OpenURL(empty);
			break;
		case AndroidConstant.Version.Kindle:
			OpenAwsUrl(AndroidConstant.BUNDLEID_STARWARFARE_KINDLE);
			break;
		}
	}

	public static int GetRandomCount()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<int>("GetRandomCount", new object[0]);
		}
		return 3;
	}

	public static void CallPurchaseProduct(string productId)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CallPurchaseProduct", productId);
		}
		else
		{
			Debug.Log("CallPurchaseProduct = " + productId);
		}
	}

	public static int GetPurchaseStatus()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<int>("GetPurchaseStatus", new object[0]);
		}
		return 0;
	}

	public static void DoPurchase()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("DoPurchase");
		}
		else
		{
			Debug.Log("DoPurchase");
		}
	}

	public static void FailPurchase()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("FailPurchase");
		}
		else
		{
			Debug.Log("FailPurchase");
		}
	}

	public static void CheckIap()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CheckIap");
		}
		else
		{
			Debug.Log("CheckIap");
		}
	}

	public static void OpenAwsUrl(string id)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("OpenAwsUrl", id);
		}
		else
		{
			Debug.Log("OpenAwsUrl = " + id);
		}
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}
}

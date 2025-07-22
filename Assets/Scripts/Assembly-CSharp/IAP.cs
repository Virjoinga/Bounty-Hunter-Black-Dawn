using System.Runtime.InteropServices;
using UnityEngine;

public class IAP
{
	[DllImport("__Internal")]
	protected static extern void PurchaseProduct(string productId, string productCount);

	public static void NowPurchaseProduct(string productId, string productCount)
	{
		Debug.Log("productId = " + productId);
		if (!FlurryScript.IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("purchaseProduct", productId, productCount);
		}
	}

	[DllImport("__Internal")]
	protected static extern int PurchaseStatus();

	public static int purchaseStatus(object stateInfo)
	{
		if (!FlurryScript.IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<int>("getPurchaseStatus", new object[0]);
		}
		return 0;
	}

	[DllImport("__Internal")]
	protected static extern int RestoreProduct();

	public static void NowRestoreProduct()
	{
		CurrentActivity.getInstance().JavaObject.Call("restoreDatabase");
	}

	[DllImport("__Internal")]
	protected static extern int ProvideContent();

	public static void provideContent()
	{
	}

	[DllImport("__Internal")]
	protected static extern int UserCanceled();

	public static void userCanceled()
	{
	}
}

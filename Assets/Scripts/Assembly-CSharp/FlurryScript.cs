using System.Runtime.InteropServices;
using UnityEngine;

public class FlurryScript
{
	[DllImport("__Internal")]
	protected static extern void _InitAds();

	public static void InitAds()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendAppStatus(byte status);

	public static void SendAppStatus(byte status)
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendAdScale(byte scale);

	public static void SendAdScale(byte scale)
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendFlurrySize(byte status, byte size);

	public static void SendFlurrySize(byte status, byte size)
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendSponsorPaySize(byte status, byte size);

	public static void SendSponsorPaySize(byte status, byte size)
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendChartboostSize(byte status, byte size);

	public static void SendChartboostSize(byte status, byte size)
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendFreyrSize(byte status, byte size);

	public static void SendFreyrSize(byte status, byte size)
	{
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}

	public static bool SaveUserDataToICloud(byte[] data, int stateKey)
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<bool>("SaveUserDataToICloud", new object[2] { data, stateKey });
		}
		Debug.Log("SaveUserDataToICloud");
		return true;
	}

	public static bool SaveGlobalDataToICloud(byte[] data)
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<bool>("SaveGlobalDataToICloud", new object[1] { data });
		}
		Debug.Log("SaveGlobalDataToICloud");
		return true;
	}

	public static bool Test(string name, int age, bool isMan)
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<bool>("Test", new object[3] { name, age, isMan });
		}
		Debug.Log("Test: Name = " + name + " AGE = " + age + " isMan = " + isMan);
		return true;
	}

	public static byte[] GetGlobalDataFromICloud()
	{
		byte[] result = null;
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call<byte[]>("GetGlobalDataFromICloud", new object[0]);
		}
		else
		{
			Debug.Log("GetGlobalDataFromICloud");
		}
		return result;
	}

	public static byte[] GetUserDataFromICloud(int stateKey)
	{
		byte[] result = null;
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call<byte[]>("GetUserDataFromICloud", new object[1] { stateKey });
		}
		else
		{
			Debug.Log("GetUserDataFromICloud");
		}
		return result;
	}
}

using System;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{
	public static event Action adViewDidReceiveAdEvent;

	public static event Action<string> adViewFailedToReceiveAdEvent;

	public static event Action interstitialDidReceiveAdEvent;

	public static event Action<string> interstitialFailedToReceiveAdEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void adViewDidReceiveAd(string empty)
	{
		if (AdMobManager.adViewDidReceiveAdEvent != null)
		{
			AdMobManager.adViewDidReceiveAdEvent();
		}
	}

	public void adViewFailedToReceiveAd(string error)
	{
		if (AdMobManager.adViewFailedToReceiveAdEvent != null)
		{
			AdMobManager.adViewFailedToReceiveAdEvent(error);
		}
	}

	public void interstitialDidReceiveAd(string empty)
	{
		if (AdMobManager.interstitialDidReceiveAdEvent != null)
		{
			AdMobManager.interstitialDidReceiveAdEvent();
		}
	}

	public void interstitialFailedToReceiveAd(string error)
	{
		if (AdMobManager.interstitialFailedToReceiveAdEvent != null)
		{
			AdMobManager.interstitialFailedToReceiveAdEvent(error);
		}
	}
}

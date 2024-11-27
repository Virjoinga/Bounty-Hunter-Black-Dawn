using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class MithrilRewardsScript : MonoBehaviour
{
	public static bool showDownloadIcon;

	public static int Mithril;

	private float time;

	private void Start()
	{
		time = Time.time;
	}

	private void Update()
	{
		if (Time.time - time > 2f)
		{
			time = Time.time;
			if (GameApp.GetInstance().GetGlobalState().bInit && Mithril > 0)
			{
				GameApp.GetInstance().GetGlobalState().AddMithril(Mithril);
				Mithril = 0;
				GameApp.GetInstance().Save();
			}
		}
	}

	public void SponsorPlayLaunchInterstitial()
	{
	}

	public void GETUDID(string udid)
	{
	}

	public void SetCurrentGround(string msg)
	{
		int num = int.Parse(msg);
		Debug.Log("SetCurrentGround int : " + num);
		GameApp.GetInstance().IsToBackground = num == 0;
	}

	public void VideoOver(string msg)
	{
		AdsManager.GetInstance().WatchVideo((InGameVideoAds)int.Parse(msg));
	}

	public void RewardMithril(string msg)
	{
		string[] array = msg.Split(':');
		string s = array[1];
		string value = array[0];
		int value2 = int.Parse(s);
		value2 = Mathf.Clamp(value2, 0, 2000);
		Mithril += value2;
		if ("1".Equals(value))
		{
			CurrentActivity.getInstance().JavaObject.Call("showTapjoyDialog", value2 + string.Empty);
		}
	}

	public void RewardCash(string msg)
	{
		int value = int.Parse(msg);
		value = Mathf.Clamp(value, 0, 3001);
		if (value > 0 && value <= 3000)
		{
			GameApp.GetInstance().GetUserState().AddCash(value);
			GameApp.GetInstance().Save();
		}
	}

	public void SetRewardStatus(string msg)
	{
	}

	public void ShowDownloadIcon()
	{
		showDownloadIcon = true;
	}

	public void HideDownloadIcon()
	{
		showDownloadIcon = false;
	}

	public void GetReward(string msg)
	{
		GameApp.GetInstance().httpRequestSent = false;
		GameApp.GetInstance().StartHttpRequestThread();
	}

	public void StartIAPValidation(string base64)
	{
		StartCoroutine(GetIAPValidation(base64));
	}

	private IEnumerator GetIAPValidation(string base64)
	{
		float startTime = Time.realtimeSinceStartup;
		bool timeOut = false;
		WWW iapWWW = null;
		byte[] encodedBytes = Encoding.UTF8.GetBytes(base64);
		string base64EncodedText = Convert.ToBase64String(encodedBytes);
		string base64UrlEncodedText = base64EncodedText.Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			iapWWW = new WWW("http://174.36.196.91:7671/IapStatServer/VerifyAgPay?receipt=" + base64UrlEncodedText + "&appcode=ag003&udid=" + GameApp.GetInstance().UUID);
			break;
		case AndroidConstant.Version.Kindle:
			iapWWW = new WWW("http://174.36.196.91:7671/IapStatServer/VerifyAwsPay?receipt=" + base64UrlEncodedText + "&appcode=ag003&udid=" + GameApp.GetInstance().UUID);
			break;
		}
		while (!iapWWW.isDone)
		{
			if (Time.realtimeSinceStartup - startTime > 40f)
			{
				timeOut = true;
				iapWWW.Dispose();
				break;
			}
			yield return new WaitForSeconds(0f);
		}
		if (timeOut)
		{
			Debug.Log("http timeout");
			if (base64.Length < GameApp.GetInstance().Base64MinLength)
			{
				AndroidPluginScript.FailPurchase();
			}
			else
			{
				AndroidPluginScript.DoPurchase();
			}
		}
		else if (iapWWW.error != null)
		{
			Debug.Log("http error:" + iapWWW.error);
			if (base64.Length < GameApp.GetInstance().Base64MinLength)
			{
				AndroidPluginScript.FailPurchase();
			}
			else
			{
				AndroidPluginScript.DoPurchase();
			}
		}
		else if (iapWWW.text.Equals("succ"))
		{
			Debug.Log("iap validation succ!");
			if (base64.Length < GameApp.GetInstance().Base64MinLength)
			{
				AndroidPluginScript.FailPurchase();
			}
			else
			{
				AndroidPluginScript.DoPurchase();
			}
		}
		else
		{
			Debug.Log("iap validation fail!");
			AndroidPluginScript.FailPurchase();
		}
	}

	public void ShowSponsorPay()
	{
	}

	public void RestoreIAPContent(string id)
	{
		if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
		{
			if (UILoadingNet.m_instance != null)
			{
				UILoadingNet.m_instance.Hide();
			}
			if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C99NewbieGift1).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C99NewbieGift1);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C199NewbieGift2).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C199NewbieGift2);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C199DoubleExp).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C199DoubleExp);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C499InfiniteSMGBullet).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C499InfiniteSMGBullet);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C499InfiniteRPGBullet).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C499InfiniteRPGBullet);
				UIIAP.iapName = IAPName.None;
			}
		}
		else if (AndroidConstant.version == AndroidConstant.Version.Kindle || AndroidConstant.version == AndroidConstant.Version.KindleCn)
		{
			if (UILoadingNet.m_instance != null)
			{
				UILoadingNet.m_instance.Hide();
			}
			if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C99NewbieGift1).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C99NewbieGift1);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C199NewbieGift2).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C199NewbieGift2);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C199DoubleExp).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C199DoubleExp);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C499InfiniteSMGBullet).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C499InfiniteSMGBullet);
				UIIAP.iapName = IAPName.None;
			}
			else if (id == IAPShop.GetInstance().GetIAPItem(IAPName.C499InfiniteRPGBullet).ID)
			{
				GameApp.GetInstance().GetGlobalState().DeliverIAPItem(IAPName.C499InfiniteRPGBullet);
				UIIAP.iapName = IAPName.None;
			}
		}
	}
}

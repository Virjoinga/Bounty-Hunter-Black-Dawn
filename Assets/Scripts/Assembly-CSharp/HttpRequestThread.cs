using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class HttpRequestThread
{
	public string CryptMD5String(string oriStr)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(oriStr);
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array = mD.ComputeHash(bytes);
		mD.Clear();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static string EncryptToSHA1(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
		bytes = hashAlgorithm.ComputeHash(bytes);
		hashAlgorithm.Clear();
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = bytes;
		foreach (byte b in array)
		{
			stringBuilder.AppendFormat("{0:X2}", b);
		}
		Debug.Log(stringBuilder.ToString());
		return stringBuilder.ToString();
	}

	public void DoWork()
	{
		try
		{
			string text = "http://174.36.196.91:8088/AdvertServer/GetAward";
			string uUID = GameApp.GetInstance().UUID;
			string text2 = CryptMD5String("ag003:ad001:" + uUID + ":32E002C8AF2E870E5D989A3D89B49A0334CD3721");
			string text3 = "appcode=ag003&adcode=ad001&udid=" + uUID + "&fhash=" + text2;
			Debug.Log("flurry: " + text3);
			Debug.Log(text + "?" + text3);
			string text4 = HttpRequest.Get(text + "?" + text3);
			Debug.Log("DoWork : " + text4);
			if (!text4.StartsWith("error&") && !text4.Equals("ok"))
			{
				AdsMithril adsMithril = new AdsMithril();
				adsMithril.AdCp = "Flurry";
				string[] array = text4.Split('#');
				string[] array2 = array;
				foreach (string text5 in array2)
				{
					string[] array3 = text5.Split('&');
					int num = int.Parse(array3[2]);
					if (num > 600)
					{
						num = 600;
					}
					MithrilRewardsScript.Mithril += num;
					adsMithril.Rewards += num;
				}
				Debug.Log(adsMithril.AdCp + " : " + MithrilRewardsScript.Mithril);
				AdsManager.GetInstance().MithrilRewardsList.Add(adsMithril);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("HttpRequest Exception:" + ex.Message);
		}
		try
		{
			string text6 = "http://174.36.196.91:8088/AdvertServer/GetAward";
			string uUID2 = GameApp.GetInstance().UUID;
			string text7 = CryptMD5String("ag003:ad002:" + uUID2 + ":32E002C8AF2E870E5D989A3D89B49A0334CD3721");
			string text8 = "appcode=ag003&adcode=ad002&udid=" + uUID2 + "&fhash=" + text7;
			Debug.Log(text6 + "?" + text8);
			string text9 = HttpRequest.Get(text6 + "?" + text8);
			Debug.Log("DoWork : " + text9);
			if (text9.StartsWith("error&") || text9.Equals("ok"))
			{
				return;
			}
			AdsMithril adsMithril2 = new AdsMithril();
			adsMithril2.AdCp = "SponsorPay";
			string[] array4 = text9.Split('#');
			string[] array5 = array4;
			foreach (string text10 in array5)
			{
				string[] array6 = text10.Split('&');
				int num2 = int.Parse(array6[2]);
				if (num2 > 600)
				{
					num2 = 600;
				}
				MithrilRewardsScript.Mithril += num2;
				adsMithril2.Rewards += num2;
			}
			Debug.Log(adsMithril2.AdCp + " : " + MithrilRewardsScript.Mithril);
			AdsManager.GetInstance().MithrilRewardsList.Add(adsMithril2);
		}
		catch (Exception ex2)
		{
			Debug.Log("HttpRequest Exception:" + ex2.Message);
		}
	}
}

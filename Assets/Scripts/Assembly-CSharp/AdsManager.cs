using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AdsManager
{
	private static AdsManager instance;

	public List<AdsMithril> MithrilRewardsList = new List<AdsMithril>();

	private byte ads_Status;

	private Dictionary<string, AdCode> adCodeDic = new Dictionary<string, AdCode>();

	private Dictionary<FreyrGame, FreyrGameStatus> freyrGameDic = new Dictionary<FreyrGame, FreyrGameStatus>();

	private Dictionary<InGameVideoAds, OtherAdsStatus> otherAdsDic = new Dictionary<InGameVideoAds, OtherAdsStatus>();

	public bool IsAdsOn
	{
		get
		{
			switch (AndroidConstant.version)
			{
			case AndroidConstant.Version.GooglePlay:
			case AndroidConstant.Version.Kindle:
				return true;
			case AndroidConstant.Version.MM:
			case AndroidConstant.Version.KindleCn:
				return false;
			default:
				return false;
			}
		}
	}

	private AdsManager()
	{
		adCodeDic.Add("ad001", new AdCode());
		adCodeDic.Add("ad002", new AdCode());
		adCodeDic.Add("ad003", new AdCode());
		adCodeDic.Add("ad004", new AdCode());
		adCodeDic.Add("ad005", new AdCode());
		adCodeDic.Add("ad006", new AdCode());
		freyrGameDic.Add(FreyrGame.StarWarfare, initStatus(FreyrGame.StarWarfare));
		freyrGameDic.Add(FreyrGame.CallOfArena, initStatus(FreyrGame.CallOfArena));
		otherAdsDic.Add(InGameVideoAds.Adcolony, initOtherAdsStatus(InGameVideoAds.Adcolony));
		otherAdsDic.Add(InGameVideoAds.Flurry, initOtherAdsStatus(InGameVideoAds.Flurry));
	}

	public static AdsManager GetInstance()
	{
		if (instance == null)
		{
			instance = new AdsManager();
		}
		return instance;
	}

	private FreyrGameStatus initStatus(FreyrGame game)
	{
		FreyrGameStatus freyrGameStatus = new FreyrGameStatus();
		freyrGameStatus.freyrGame = game;
		freyrGameStatus.videoTimes = 0;
		freyrGameStatus.lastYearToWatchVideo = 0;
		freyrGameStatus.lastDayToWatchVideo = 0;
		freyrGameStatus.clickIconOfReward = FreyrGameStatus.FreyrGameReward.Inactive;
		return freyrGameStatus;
	}

	private OtherAdsStatus initOtherAdsStatus(InGameVideoAds ads)
	{
		OtherAdsStatus otherAdsStatus = new OtherAdsStatus();
		otherAdsStatus.ads = ads;
		otherAdsStatus.videoTimes = 0;
		otherAdsStatus.lastDayToWatchVideo = 0;
		otherAdsStatus.lastYearToWatchVideo = 0;
		return otherAdsStatus;
	}

	public InGameVideoAds GetVideo()
	{
		bool flag = false;
		foreach (KeyValuePair<FreyrGame, FreyrGameStatus> item in freyrGameDic)
		{
			if (HasVideoToWatch(item.Value.freyrGame) && !GetGameClick(item.Value.freyrGame))
			{
				flag = true;
			}
		}
		if (flag)
		{
			return InGameVideoAds.Freyr;
		}
		if (GetInstance().IsAdsOn)
		{
			int num = ((otherAdsDic[InGameVideoAds.Adcolony].videoTimes < 3) ? (3 - otherAdsDic[InGameVideoAds.Adcolony].videoTimes) : 0);
			int num2 = ((otherAdsDic[InGameVideoAds.Flurry].videoTimes < 3) ? (3 - otherAdsDic[InGameVideoAds.Flurry].videoTimes) : 0);
			if (num == 0 && num2 == 0)
			{
				return InGameVideoAds.MoreGame;
			}
			Interval interval = new Interval(num2, num);
			return (InGameVideoAds)(interval.GetIndex() + 1);
		}
		return InGameVideoAds.None;
	}

	public bool HasAllFreyrVideosWatched()
	{
		return HasVideoToWatch(FreyrGame.StarWarfare) || HasVideoToWatch(FreyrGame.CallOfArena);
	}

	public bool HasVideoToWatch(FreyrGame game)
	{
		return freyrGameDic[game].videoTimes < 3 && (freyrGameDic[game].lastYearToWatchVideo < DateTime.Now.Year || (freyrGameDic[game].lastYearToWatchVideo == DateTime.Now.Year && freyrGameDic[game].lastDayToWatchVideo < DateTime.Now.DayOfYear));
	}

	private void CheckVideoWatch(InGameVideoAds ads)
	{
		if (otherAdsDic[ads].lastYearToWatchVideo < DateTime.Now.Year || (otherAdsDic[ads].lastYearToWatchVideo == DateTime.Now.Year && otherAdsDic[ads].lastDayToWatchVideo < DateTime.Now.DayOfYear))
		{
			otherAdsDic[ads].videoTimes = 0;
		}
	}

	public void WatchVideo(FreyrGame game)
	{
		freyrGameDic[game].videoTimes++;
		freyrGameDic[game].lastYearToWatchVideo = DateTime.Now.Year;
		freyrGameDic[game].lastDayToWatchVideo = DateTime.Now.DayOfYear;
	}

	public void WatchVideo(InGameVideoAds ads)
	{
		otherAdsDic[ads].videoTimes++;
		otherAdsDic[ads].lastYearToWatchVideo = DateTime.Now.Year;
		otherAdsDic[ads].lastDayToWatchVideo = DateTime.Now.DayOfYear;
	}

	public FreyrGame GetGameNotClick()
	{
		List<FreyrGameStatus> list = new List<FreyrGameStatus>();
		foreach (KeyValuePair<FreyrGame, FreyrGameStatus> item in freyrGameDic)
		{
			if (item.Value.clickIconOfReward == FreyrGameStatus.FreyrGameReward.Inactive)
			{
				list.Add(item.Value);
			}
		}
		if (list.Count == 0)
		{
			return FreyrGame.None;
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index].freyrGame;
	}

	public bool GetGameClick(FreyrGame game)
	{
		if (game == FreyrGame.None)
		{
			return false;
		}
		return freyrGameDic[game].clickIconOfReward != FreyrGameStatus.FreyrGameReward.Inactive;
	}

	public void SetGameClick(FreyrGame game)
	{
		if (game != FreyrGame.None && freyrGameDic[game].clickIconOfReward == FreyrGameStatus.FreyrGameReward.Inactive)
		{
			freyrGameDic[game].clickIconOfReward = FreyrGameStatus.FreyrGameReward.Available;
		}
	}

	public void CheckGameRewards()
	{
		foreach (KeyValuePair<FreyrGame, FreyrGameStatus> item in freyrGameDic)
		{
			if (item.Value.clickIconOfReward == FreyrGameStatus.FreyrGameReward.Available)
			{
				item.Value.clickIconOfReward = FreyrGameStatus.FreyrGameReward.Active;
				GameApp.GetInstance().GetGlobalState().AddMithril(70);
			}
		}
	}

	public void OpenGameURL(FreyrGame game)
	{
		string empty = string.Empty;
		if (game == FreyrGame.StarWarfare)
		{
			AndroidPluginScript.ShowStarWarfare();
		}
		else
		{
			AndroidPluginScript.ShowCallOfArena();
		}
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(freyrGameDic.Count);
		foreach (KeyValuePair<FreyrGame, FreyrGameStatus> item in freyrGameDic)
		{
			bw.Write((byte)item.Value.freyrGame);
			bw.Write((byte)item.Value.clickIconOfReward);
			bw.Write(item.Value.videoTimes);
			bw.Write(item.Value.lastYearToWatchVideo);
			bw.Write(item.Value.lastDayToWatchVideo);
		}
		bw.Write(otherAdsDic.Count);
		foreach (KeyValuePair<InGameVideoAds, OtherAdsStatus> item2 in otherAdsDic)
		{
			bw.Write((byte)item2.Value.ads);
			bw.Write(item2.Value.videoTimes);
			bw.Write(item2.Value.lastYearToWatchVideo);
			bw.Write(item2.Value.lastDayToWatchVideo);
		}
	}

	public void Load(BinaryReader br)
	{
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			FreyrGame key = (FreyrGame)br.ReadByte();
			freyrGameDic[key].clickIconOfReward = (FreyrGameStatus.FreyrGameReward)br.ReadByte();
			freyrGameDic[key].videoTimes = br.ReadByte();
			freyrGameDic[key].lastYearToWatchVideo = br.ReadInt32();
			freyrGameDic[key].lastDayToWatchVideo = br.ReadInt32();
		}
		num = br.ReadInt32();
		for (int j = 0; j < num; j++)
		{
			InGameVideoAds inGameVideoAds = (InGameVideoAds)br.ReadByte();
			otherAdsDic[inGameVideoAds].videoTimes = br.ReadByte();
			otherAdsDic[inGameVideoAds].lastYearToWatchVideo = br.ReadInt32();
			otherAdsDic[inGameVideoAds].lastDayToWatchVideo = br.ReadInt32();
			CheckVideoWatch(inGameVideoAds);
		}
	}

	public void DownLoadAdsStatus(BytesBuffer bb)
	{
		ads_Status = 0;
		if (bb.ReadShort() == 0)
		{
			ads_Status = bb.ReadByte();
			bb.ReadByte();
			if (ads_Status == 1)
			{
				GameApp.ShowFreyrGamesAds = true;
			}
		}
		Debug.Log("AdsStatus : " + ads_Status);
		Debug.Log("AdsStatus Finish");
	}

	public void DownLoadAdCode(BytesBuffer bb)
	{
		Debug.Log("DownLoadAdCode");
		short num = bb.ReadShort();
		Debug.Log("errorCode : " + num);
		if (num == 0)
		{
			byte b = bb.ReadByte();
			Debug.Log("ad_num : " + b);
			for (int i = 0; i < b; i++)
			{
				string text = bb.ReadStringShortLength();
				byte b2 = bb.ReadByte();
				byte b3 = bb.ReadByte();
				adCodeDic[text].Code = text;
				adCodeDic[text].Status = b2;
				adCodeDic[text].Size = b3;
				Debug.Log("code : " + text);
				Debug.Log("status : " + b2);
				Debug.Log("size : " + b3);
			}
		}
		Debug.Log("AdCode Finish");
	}
}

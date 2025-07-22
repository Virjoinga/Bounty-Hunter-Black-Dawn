using System;
using UnityEngine;

public class UIAdsVideo : UIGameMenu
{
	public GameObject m_Close;

	public GameObject m_StarWarfare;

	public GameObject m_CallOfArena;

	public UIAdsDownload m_UIAdsDownload;

	public UILabel[] m_Label;

	private DateTime lastTime = DateTime.Now;

	private bool bWaitForMsg;

	private FreyrGame lastGame = FreyrGame.None;

	private static byte prevPhase;

	protected override void Awake()
	{
		base.Awake();
		AddDelegate(m_Close);
		AddDelegate(m_StarWarfare);
		AddDelegate(m_CallOfArena);
		SetMenuCloseOnDestroy(true);
		if (!AdsManager.GetInstance().HasVideoToWatch(FreyrGame.StarWarfare) || AdsManager.GetInstance().GetGameClick(FreyrGame.StarWarfare))
		{
			m_StarWarfare.SetActive(false);
		}
		if (!AdsManager.GetInstance().HasVideoToWatch(FreyrGame.CallOfArena) || AdsManager.GetInstance().GetGameClick(FreyrGame.CallOfArena))
		{
			m_CallOfArena.SetActive(false);
		}
	}

	protected override byte InitMask()
	{
		return 0;
	}

	private void LateUpdate()
	{
		if (bWaitForMsg)
		{
			if ((DateTime.Now - lastTime).TotalSeconds > 15.0)
			{
				bWaitForMsg = false;
				if (AdsManager.GetInstance().IsAdsOn)
				{
					m_UIAdsDownload.Show(lastGame);
				}
				GameApp.GetInstance().GetGlobalState().AddMithril(15);
				GameApp.GetInstance().Save();
				UILabel[] label = m_Label;
				foreach (UILabel uILabel in label)
				{
					uILabel.gameObject.SetActive(false);
				}
			}
		}
		else if (!m_UIAdsDownload.IsShow())
		{
			UILabel[] label2 = m_Label;
			foreach (UILabel uILabel2 in label2)
			{
				uILabel2.gameObject.SetActive(true);
			}
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (go.Equals(m_Close))
		{
			Close();
		}
		else if (go.Equals(m_StarWarfare))
		{
			Handheld.PlayFullScreenMovie("Trailer_StarWarfare.mp4", Color.black, FullScreenMovieControlMode.Hidden);
			AdsManager.GetInstance().WatchVideo(FreyrGame.StarWarfare);
			lastGame = FreyrGame.StarWarfare;
			lastTime = DateTime.Now;
			bWaitForMsg = true;
			go.SetActive(false);
		}
		else if (go.Equals(m_CallOfArena))
		{
			Handheld.PlayFullScreenMovie("Trailer_CallOfArena.m4v", Color.black, FullScreenMovieControlMode.Hidden);
			AdsManager.GetInstance().WatchVideo(FreyrGame.CallOfArena);
			lastGame = FreyrGame.CallOfArena;
			lastTime = DateTime.Now;
			bWaitForMsg = true;
			go.SetActive(false);
		}
	}

	public static void Show()
	{
		prevPhase = 6;
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(35, false, false, true);
	}

	public static void Close()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(prevPhase, false, false, true);
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}
}

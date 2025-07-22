using UnityEngine;

public class UIAds : UIDelegateMenu
{
	public GameObject m_ButtonFlurryTryNewGames;

	public GameObject m_ButtonTapjoyMoreGames;

	public GameObject m_ButtonSponsorPay;

	public GameObject m_ButtonYouMi;

	public GameObject m_ButtonClose;

	public GameObject m_ButtonOurGame;

	public GameObject m_ButtonFacebook;

	public GameObject m_ButtonTwitter;

	public GameObject m_IconStarWarfare;

	public GameObject m_IconCallOfArena;

	public BoxCollider m_block;

	private FreyrGame oneOfFreyrGames;

	private static GameObject m_Ads;

	private void Awake()
	{
		AddDelegate(m_ButtonFlurryTryNewGames);
		AddDelegate(m_ButtonTapjoyMoreGames);
		AddDelegate(m_ButtonSponsorPay);
		AddDelegate(m_ButtonClose);
		AddDelegate(m_ButtonOurGame);
		AddDelegate(m_ButtonFacebook);
		AddDelegate(m_ButtonTwitter);
		AddDelegate(m_ButtonYouMi);
		m_ButtonYouMi.SetActive(false);
		if (AndroidConstant.version == AndroidConstant.Version.Kindle)
		{
			m_ButtonFlurryTryNewGames.SetActive(false);
			m_ButtonTapjoyMoreGames.SetActive(false);
			m_ButtonSponsorPay.SetActive(false);
		}
		oneOfFreyrGames = AdsManager.GetInstance().GetGameNotClick();
	}

	private void Start()
	{
		m_block.size = new Vector3(Screen.width, Screen.height, 0f);
		if (oneOfFreyrGames == FreyrGame.None)
		{
			m_ButtonOurGame.SetActive(false);
		}
		else if (oneOfFreyrGames == FreyrGame.StarWarfare)
		{
			m_IconCallOfArena.SetActive(false);
		}
		else if (oneOfFreyrGames == FreyrGame.CallOfArena)
		{
			m_IconStarWarfare.SetActive(false);
		}
		if (GameApp.GetInstance().GetGlobalState().GetFacebook())
		{
			m_ButtonFacebook.SetActive(false);
		}
		if (GameApp.GetInstance().GetGlobalState().GetTwitter())
		{
			m_ButtonTwitter.SetActive(false);
		}
		if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
		{
			AndroidAdsPluginScript.CallFlurryAds();
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (go.Equals(m_ButtonFlurryTryNewGames))
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				AndroidAdsPluginScript.CallFlurryAds();
			}
		}
		else if (go.Equals(m_ButtonTapjoyMoreGames))
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				AndroidAdsPluginScript.CallTapjoyOfferWall();
			}
		}
		else if (go.Equals(m_ButtonSponsorPay))
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				AndroidAdsPluginScript.CallSponsorPayOfferWall();
			}
		}
		else if (go.Equals(m_ButtonClose))
		{
			Close();
		}
		else if (go.Equals(m_ButtonOurGame))
		{
			m_ButtonOurGame.SetActive(false);
			AdsManager.GetInstance().SetGameClick(oneOfFreyrGames);
			GameApp.GetInstance().Save();
			AdsManager.GetInstance().OpenGameURL(oneOfFreyrGames);
		}
		else if (go.Equals(m_ButtonFacebook))
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				Application.OpenURL(UIConstant.FACEBOOK_HOME);
				GameApp.GetInstance().GetGlobalState().SetFacebook(true);
				m_ButtonFacebook.SetActive(false);
				GameApp.GetInstance().GetGlobalState().AddMithril(35);
				GameApp.GetInstance().Save();
			}
		}
		else if (go.Equals(m_ButtonTwitter) && GameApp.GetInstance().IsConnectedToInternet())
		{
			Application.OpenURL(UIConstant.TWITTER_HOME);
			GameApp.GetInstance().GetGlobalState().SetTwitter(true);
			m_ButtonTwitter.SetActive(false);
			GameApp.GetInstance().GetGlobalState().AddMithril(35);
			GameApp.GetInstance().Save();
		}
	}

	public static void Show()
	{
		if (m_Ads == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Ads", "AdsUI");
			m_Ads = Object.Instantiate(original) as GameObject;
			UIMemoryManager.IncreaseMemoryClearCounter(1);
			if (NGUIBackPackUIScript.mInstance != null)
			{
				NGUIBackPackUIScript.mInstance.SetBackPackBlockState(true);
			}
		}
	}

	public static void Close()
	{
		if (m_Ads != null)
		{
			MemoryManager.FreeNGUI(m_Ads, false);
			m_Ads = null;
			GameApp.GetInstance().Save();
			UIMemoryManager.CheckMemoryClear();
			if (NGUIBackPackUIScript.mInstance != null)
			{
				NGUIBackPackUIScript.mInstance.SetBackPackBlockState(false);
			}
		}
	}

	public static bool IsShow()
	{
		return m_Ads != null;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Close();
		}
	}
}

using UnityEngine;

public class UIAdsDownload : UIDelegateMenu
{
	public GameObject m_Close;

	public GameObject m_Download;

	public UISprite m_Icon;

	public UISprite m_Title;

	private FreyrGame game;

	private void Awake()
	{
		AddDelegate(m_Download);
		AddDelegate(m_Close);
		base.gameObject.SetActive(false);
	}

	public void Show(FreyrGame game)
	{
		base.gameObject.SetActive(true);
		m_Icon.spriteName = GetIconName(game);
		m_Icon.MakePixelPerfect();
		m_Title.spriteName = GetTitleName(game);
		m_Title.MakePixelPerfect();
		this.game = game;
	}

	public bool IsShow()
	{
		return base.gameObject.activeSelf;
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (go.Equals(m_Close))
		{
			base.gameObject.SetActive(false);
			if (!AdsManager.GetInstance().HasAllFreyrVideosWatched())
			{
				UIAdsVideo.Close();
			}
		}
		else if (go.Equals(m_Download))
		{
			AdsManager.GetInstance().SetGameClick(game);
			AdsManager.GetInstance().OpenGameURL(game);
			base.gameObject.SetActive(false);
			if (!AdsManager.GetInstance().HasAllFreyrVideosWatched())
			{
				UIAdsVideo.Close();
			}
		}
	}

	private string GetIconName(FreyrGame game)
	{
		switch (game)
		{
		case FreyrGame.StarWarfare:
			return "starwar";
		case FreyrGame.CallOfArena:
			return "rush";
		default:
			return string.Empty;
		}
	}

	private string GetTitleName(FreyrGame game)
	{
		switch (game)
		{
		case FreyrGame.StarWarfare:
			return "title_star";
		case FreyrGame.CallOfArena:
			return "title_runner";
		default:
			return string.Empty;
		}
	}
}

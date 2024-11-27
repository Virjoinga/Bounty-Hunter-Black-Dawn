using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
[Tooltip("Custion Action...")]
public class FrAds : FsmStateAction
{
	public override void OnEnter()
	{
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (TutorialManager.GetInstance().IsShopTutorialOk() && TutorialManager.GetInstance().IsFirstTutorialOk() && !GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			InGameVideoAds video = AdsManager.GetInstance().GetVideo();
			if ((video == InGameVideoAds.Freyr || AndroidConstant.version != 0 || AndroidConstant.version != AndroidConstant.Version.Kindle) && AndroidConstant.version != AndroidConstant.Version.MM && AndroidConstant.version != AndroidConstant.Version.KindleCn)
			{
				UIAdsVideo.Show();
			}
			else if (AndroidConstant.version == AndroidConstant.Version.GooglePlay || AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				switch (video)
				{
				case InGameVideoAds.Adcolony:
					AndroidAdsPluginScript.CallAdcolonyVideo();
					break;
				case InGameVideoAds.Flurry:
					AndroidAdsPluginScript.CallAdcolonyVideo();
					break;
				case InGameVideoAds.MoreGame:
					if (AndroidConstant.version == AndroidConstant.Version.GooglePlay || AndroidConstant.version == AndroidConstant.Version.Kindle)
					{
						UIAdsNoVideo.Show();
					}
					break;
				}
			}
		}
		Finish();
	}

	public override string ErrorCheck()
	{
		return null;
	}
}

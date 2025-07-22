using UnityEngine;

public class UIAdsButton : MonoBehaviour
{
	private void Start()
	{
		if (IsIconCanBeActive())
		{
			base.gameObject.SetActive(true);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private bool IsIconCanBeActive()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			return false;
		}
		if (GameApp.GetInstance().GetGameScene() != null && GameApp.GetInstance().GetGameScene().mapType != 0)
		{
			return false;
		}
		return AdsManager.GetInstance().IsAdsOn;
	}

	private void OnClick()
	{
		if (!(InGameMenu.instance != null) || !InGameMenu.instance.IsSpread())
		{
			UIAds.Show();
		}
	}
}

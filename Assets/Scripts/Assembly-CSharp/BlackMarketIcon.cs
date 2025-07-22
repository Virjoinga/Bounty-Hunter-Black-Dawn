using UnityEngine;

public class BlackMarketIcon : MonoBehaviour
{
	public GameObject m_Button;

	public UIFilledSprite m_Mask;

	public UITweenX m_Tween;

	public UILabel m_Price;

	public GameObject m_PriceObject;

	public static float BlackMarketCD;

	public static float BlackMarketTimeToReady;

	private bool bCDStart = true;

	private void Start()
	{
		m_Price.text = Mathf.CeilToInt(25f * BlackMarketTimeToReady / BlackMarketCD).ToString();
	}

	private void Update()
	{
		Timer mBlackMarketRefreshTimer = GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer;
		if (bCDStart)
		{
			m_Mask.fillAmount = 1f - mBlackMarketRefreshTimer.GetTimeNeededToNextReady() / BlackMarketCD;
			if (mBlackMarketRefreshTimer.Ready())
			{
				m_Tween.PlayForward();
				bCDStart = false;
				if (m_PriceObject.activeSelf)
				{
					m_PriceObject.SetActive(false);
				}
			}
			else if (ShopUIScript.mInstance != null && ShopUIScript.mInstance.CurrentPage != ShopPageType.BlackMarket)
			{
				if (!m_PriceObject.activeSelf)
				{
					m_PriceObject.SetActive(true);
				}
			}
			else
			{
				if (m_PriceObject.activeSelf)
				{
					m_PriceObject.SetActive(false);
				}
				m_Mask.fillAmount = 0f;
			}
		}
		else if (!mBlackMarketRefreshTimer.Ready())
		{
			bCDStart = true;
		}
		if (m_PriceObject.activeSelf)
		{
			m_Price.text = Mathf.CeilToInt(25f * BlackMarketTimeToReady / BlackMarketCD).ToString();
		}
	}
}

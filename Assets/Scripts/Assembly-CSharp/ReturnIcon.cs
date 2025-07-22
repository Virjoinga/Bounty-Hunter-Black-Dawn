using UnityEngine;

public class ReturnIcon : MonoBehaviour
{
	public GameObject m_Button;

	public UIFilledSprite m_Mask;

	public UITweenX m_Tween;

	private bool bCDStart;

	private void Update()
	{
		if (bCDStart)
		{
			m_Mask.fillAmount = ((!Arena.GetInstance().IsCurrentSceneArena()) ? (1f - UserStateHUD.GetInstance().GetPercentOfReturnCD()) : 0f);
			if (UserStateHUD.GetInstance().IsReturnCDOK())
			{
				m_Tween.PlayForward();
				bCDStart = false;
			}
		}
		else if (UserStateHUD.GetInstance().GetPercentOfReturnCD() < 1f)
		{
			bCDStart = true;
			m_Mask.fillAmount = 1f;
		}
		else if (m_Mask.fillAmount > 0f)
		{
			m_Mask.fillAmount = 0f;
		}
	}
}

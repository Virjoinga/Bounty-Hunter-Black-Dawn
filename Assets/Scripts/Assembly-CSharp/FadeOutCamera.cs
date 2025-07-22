using UnityEngine;

public class FadeOutCamera : MonoBehaviour
{
	public FadeAnimationScript m_fade;

	public FisheyeAnimationScript m_Fisheye;

	public void Show()
	{
		m_fade.FadeOutWhite();
		m_Fisheye.Act(4);
	}

	public bool IsEnd()
	{
		return m_fade.FadeOutComplete() && m_Fisheye.HasNoAction();
	}
}

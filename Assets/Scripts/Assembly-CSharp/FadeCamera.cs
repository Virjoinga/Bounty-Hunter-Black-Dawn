using UnityEngine;

public class FadeCamera : MonoBehaviour
{
	public FadeAnimationScript m_fade;

	public FisheyeAnimationScript m_Fisheye;

	public void Show()
	{
		m_fade.FadeOutWhite();
		m_Fisheye.Act(4);
	}
}

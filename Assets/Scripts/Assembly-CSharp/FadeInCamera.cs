using UnityEngine;

public class FadeInCamera : MonoBehaviour
{
	public FadeAnimationScript m_fade;

	public void Show()
	{
		m_fade.FadeInBlack();
	}

	public bool IsEnd()
	{
		return m_fade.FadeInComplete();
	}
}

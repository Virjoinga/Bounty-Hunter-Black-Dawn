using UnityEngine;

public class EffectsCamera : MonoBehaviour
{
	public enum Type
	{
		FadeIn = 0,
		Teleport = 1
	}

	public GameObject m_BlackScreen;

	public FadeOutControl m_FadeOut;

	public FadeInControl m_FadeIn;

	public static EffectsCamera instance;

	public bool m_bManual;

	private void Awake()
	{
		m_BlackScreen.SetActive(false);
		m_FadeOut.gameObject.SetActive(false);
		m_FadeIn.gameObject.SetActive(false);
		instance = this;
	}

	private void Start()
	{
		m_FadeOut.gameObject.SetActive(true);
		StartEffect(Type.Teleport, null);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public void StartEffect(Type type, EffectsCameraListener listener)
	{
		switch (type)
		{
		case Type.FadeIn:
			m_FadeIn.SetListener(listener);
			m_FadeIn.gameObject.SetActive(true);
			break;
		case Type.Teleport:
			m_FadeOut.SetListener(listener);
			m_FadeOut.gameObject.SetActive(true);
			m_FadeOut.aniEffects.SetState(0);
			break;
		}
	}

	public void RemoveListener()
	{
		m_FadeIn.SetListener(null);
		m_FadeOut.SetListener(null);
	}

	public bool IsRunning()
	{
		return m_FadeOut.IsRunning() || m_FadeIn.IsRunning();
	}
}

using UnityEngine;

public class FadeInControl : MonoBehaviour
{
	public FadeInCamera fadeIn;

	public GameObject blackScreen;

	private bool m_bRunning;

	private EffectsCameraListener listener;

	private void OnEnable()
	{
		if (TutorialManager.GetInstance().IsFirstTutorialOk())
		{
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
		}
		HUDManager.instance.m_HotKeyManager.ForbidAll();
		fadeIn.Show();
		m_bRunning = true;
	}

	private void Update()
	{
		if (fadeIn.IsEnd())
		{
			base.gameObject.SetActive(false);
			blackScreen.SetActive(true);
			if (TutorialManager.GetInstance().IsFirstTutorialOk())
			{
				Transform transform = Camera.main.transform.Find("Npc_Collision");
				transform.GetComponent<Collider>().enabled = false;
			}
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			m_bRunning = false;
			if (listener != null)
			{
				listener.OnEffectsEnd(EffectsCamera.Type.FadeIn);
			}
		}
	}

	public void SetListener(EffectsCameraListener listener)
	{
		this.listener = listener;
	}

	public bool IsRunning()
	{
		return m_bRunning;
	}
}

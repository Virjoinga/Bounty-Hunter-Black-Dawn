using UnityEngine;

public class FadeOutControl : MonoBehaviour
{
	public FadeOutCamera fadeOut;

	public AnimationCamera aniEffects;

	public GameObject blackScreen;

	private EffectsCameraListener listener;

	private bool m_bInputController;

	private bool m_bRunning;

	private void OnEnable()
	{
		aniEffects.gameObject.SetActive(true);
		fadeOut.gameObject.SetActive(false);
		m_bInputController = false;
		m_bRunning = true;
	}

	private void Update()
	{
		UIStateManager uIStateManager = GameApp.GetInstance().GetUIStateManager();
		if (uIStateManager != null && !m_bInputController && ((InGameUIScript)uIStateManager).bInitCompleted() && TutorialManager.GetInstance().IsEffectsCameraNeedUse())
		{
			m_bInputController = true;
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			HUDManager.instance.m_HotKeyManager.ForbidAll();
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
		}
		if (aniEffects.gameObject.activeSelf)
		{
			if (aniEffects.IsEnd())
			{
				aniEffects.m_Anim1.SetActive(false);
				aniEffects.m_Anim2.SetActive(false);
				aniEffects.gameObject.SetActive(false);
				fadeOut.gameObject.SetActive(true);
				fadeOut.Show();
				blackScreen.SetActive(false);
				SendResult(EffectsCamera.Type.Teleport);
			}
		}
		else
		{
			if (!fadeOut.IsEnd())
			{
				return;
			}
			base.gameObject.SetActive(false);
			if (TutorialManager.GetInstance().IsEffectsCameraNeedUse())
			{
				Transform transform2 = Camera.main.transform.Find("Npc_Collision");
				transform2.GetComponent<Collider>().enabled = false;
				HUDManager.instance.m_HotKeyManager.CancelFobid();
				if (GameApp.GetInstance().GetGameWorld() != null)
				{
					GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
						.InputController.Block = false;
				}
			}
		}
	}

	private void SendResult(EffectsCamera.Type type)
	{
		m_bRunning = false;
		if (listener != null)
		{
			listener.OnEffectsEnd(type);
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

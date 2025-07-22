using System;
using UnityEngine;

public class AnimationCamera : MonoBehaviour
{
	public const byte ANIM_STATE_LOOP = 0;

	public const byte ANIM_STATE_END = 1;

	private static float TIME_LOOP_WAITING = 2000f;

	private static float TIME_END_WAITING = 1000f;

	private static float TIMEOUT_ANIM = 2000f;

	protected byte m_state;

	public GameObject m_Anim1;

	public GameObject m_Anim2;

	public UILabel m_Tips;

	public GameObject m_endAnim;

	private static string ENDING_ANIM_NAME = "RPG_anim_023";

	private DateTime startTime;

	private DateTime endTime;

	private void Awake()
	{
		m_Anim1.SetActive(false);
		m_Anim2.SetActive(false);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			m_Tips.text = LocalizationManager.GetInstance().GetString(GameApp.GetInstance().GetGlobalState().GetTips(1));
		}
		else
		{
			m_Tips.text = LocalizationManager.GetInstance().GetString(GameApp.GetInstance().GetGlobalState().GetTips(0));
		}
	}

	private void Update()
	{
	}

	public bool IsEnd()
	{
		if (EffectsCamera.instance.m_bManual)
		{
			return false;
		}
		if (m_state == 0)
		{
			if (GameApp.GetInstance().GetSceneStreaingManager().IsSceneStreamingCompleted() && (DateTime.Now - startTime).TotalMilliseconds > (double)TIME_LOOP_WAITING)
			{
				SetState(1);
				return false;
			}
		}
		else
		{
			TimeSpan timeSpan = DateTime.Now - endTime;
			if (AnimationPlayed(ENDING_ANIM_NAME, 1f) || timeSpan.TotalMilliseconds > (double)TIME_END_WAITING)
			{
				return true;
			}
		}
		return false;
	}

	public bool AnimationPlayed(string name, float percent)
	{
		if (m_endAnim.GetComponent<Animation>()[name] == null)
		{
			return false;
		}
		if (m_endAnim.GetComponent<Animation>()[name].speed >= 0f)
		{
			if (m_endAnim.GetComponent<Animation>()[name].time >= m_endAnim.GetComponent<Animation>()[name].clip.length * percent)
			{
				return true;
			}
			return false;
		}
		if (m_endAnim.GetComponent<Animation>()[name].time <= m_endAnim.GetComponent<Animation>()[name].clip.length * (1f - percent))
		{
			return true;
		}
		return false;
	}

	public void SetState(byte state)
	{
		m_state = state;
		switch (m_state)
		{
		case 0:
			startTime = DateTime.Now;
			m_Anim1.SetActive(true);
			break;
		case 1:
			endTime = DateTime.Now;
			m_Anim1.SetActive(false);
			m_Anim2.SetActive(true);
			m_endAnim.GetComponent<Animation>()[ENDING_ANIM_NAME].wrapMode = WrapMode.ClampForever;
			m_endAnim.GetComponent<Animation>()[ENDING_ANIM_NAME].speed = 1f;
			m_endAnim.GetComponent<Animation>().CrossFade(ENDING_ANIM_NAME);
			break;
		}
	}

	public void Show()
	{
		startTime = DateTime.Now;
	}
}

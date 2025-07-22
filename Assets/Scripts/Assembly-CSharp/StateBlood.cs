using UnityEngine;

public class StateBlood : MonoBehaviour
{
	public UISprite m_Sprite;

	public UITweenX m_Tween;

	public TweenAlphaX m_TweenAlpha;

	private bool IsHeartbeat;

	private bool IsFirstDying;

	private void OnEnable()
	{
		m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 0f);
		m_TweenAlpha.enabled = false;
	}

	private void Update()
	{
		if (UserStateHUD.GetInstance().GetUserHp() == 0)
		{
			if (IsFirstDying)
			{
				m_Tween.Play();
				m_TweenAlpha.duration = 0.8f;
				IsFirstDying = false;
			}
		}
		else
		{
			IsFirstDying = true;
			UpdateHurt();
		}
	}

	private void UpdateHurt()
	{
		float num = (float)UserStateHUD.GetInstance().GetUserHp() * 2f / (float)UserStateHUD.GetInstance().GetUserMaxHp();
		if (UserStateHUD.GetInstance().GetUserShield() > 0)
		{
			num += (1f - num) * ((float)UserStateHUD.GetInstance().GetUserShield() / (float)UserStateHUD.GetInstance().GetUserMaxShield());
		}
		num = Mathf.Clamp(1f - num, 0f, 1f);
		m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, num);
	}

	private void OnHeartbeat()
	{
		if (UserStateHUD.GetInstance().GetUserHp() == 0)
		{
			if (UserStateHUD.GetInstance().IsUserDead())
			{
				m_Sprite.color = new Color(m_Sprite.color.r, m_Sprite.color.g, m_Sprite.color.b, 1f);
				return;
			}
			m_TweenAlpha.duration += 0.1f;
			m_Tween.Play();
		}
	}
}

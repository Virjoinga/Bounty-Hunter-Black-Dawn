using System;
using UnityEngine;

public class StateMissionUpdate : MonoBehaviour
{
	public UIPanel m_Panel;

	public TweenPanelClip m_TweenPanelClip;

	public GameObject m_Sprite;

	public UILabel m_Label;

	public UITweenX m_Tween;

	private UITweener[] tweenerList;

	private string nextStr;

	private DateTime mLastUpdateTime;

	private bool bPlay;

	private void Awake()
	{
		tweenerList = GetComponentsInChildren<UITweener>();
	}

	private void Start()
	{
		ClearData();
		bPlay = false;
	}

	public void SetMissionText(string str)
	{
		m_Label.text = str;
	}

	public void ClearData()
	{
		m_Panel.clipRange = m_TweenPanelClip.from;
		m_Sprite.SetActive(false);
		SetMissionText(string.Empty);
		nextStr = string.Empty;
		m_TweenPanelClip.enabled = false;
		bPlay = false;
	}

	public bool IsHasNoData()
	{
		return m_Label.text.Equals(string.Empty) && nextStr.Equals(string.Empty);
	}

	public bool IsSameText(string str)
	{
		return m_Label.text.Equals(str) || nextStr.Equals(str);
	}

	private void Update()
	{
		if (bPlay && (DateTime.Now - mLastUpdateTime).TotalSeconds > (double)UserStateHUD.GetInstance().GetMissionUpdateDelay())
		{
			bPlay = false;
			m_Sprite.SetActive(true);
			UserStateHUD.GetInstance().InfoBox.PushMissionUpdate();
			m_Tween.PlayForward(PlayReverse);
		}
	}

	public void PlayEffects(string str)
	{
		bPlay = true;
		mLastUpdateTime = DateTime.Now;
		nextStr = str;
	}

	private void PlayReverse()
	{
		SetMissionText(nextStr);
		nextStr = string.Empty;
		m_Tween.PlayReverse(HideSprte);
	}

	private void HideSprte()
	{
		m_Sprite.SetActive(false);
	}
}

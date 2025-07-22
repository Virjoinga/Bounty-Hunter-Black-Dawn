using System;
using UnityEngine;

public abstract class TutorialScript : MonoBehaviour
{
	public GameObject m_Context;

	private bool bStart;

	private bool bEnd;

	private DateTime mLastUpdateTime;

	private bool bEndTutorial;

	private void Awake()
	{
		bStart = false;
		bEnd = false;
		bEndTutorial = false;
		mLastUpdateTime = DateTime.Now;
		if (IsTutorialOk(GetType()))
		{
			bEnd = true;
			base.gameObject.SetActive(false);
		}
		else
		{
			Hide();
		}
	}

	protected void Hide()
	{
		m_Context.SetActive(false);
	}

	private void LateUpdate()
	{
		if (bEnd)
		{
			return;
		}
		if (bStart)
		{
			OnTutorialUpdate();
		}
		else if ((DateTime.Now - mLastUpdateTime).TotalMilliseconds > 50.0)
		{
			mLastUpdateTime = DateTime.Now;
			if (IsTutorialCanStart())
			{
				m_Context.SetActive(true);
				bStart = true;
				OnTutorialStart();
			}
		}
		if (bEndTutorial)
		{
			TutorialManager.GetInstance().SetTutorialOk(GetType());
		}
		if (IsTutorialOk(GetType()))
		{
			OnTutorialEnd();
			bEnd = true;
			base.gameObject.SetActive(false);
		}
	}

	protected void EndTutorial()
	{
		bEndTutorial = true;
	}

	protected void PauseTutorial()
	{
		m_Context.SetActive(false);
		bStart = false;
	}

	protected bool IsTutorialOk(TutorialManager.TutorialType type)
	{
		return TutorialManager.GetInstance().IsTutorialOk(type);
	}

	protected virtual void OnTutorialStart()
	{
	}

	protected virtual void OnTutorialUpdate()
	{
	}

	protected virtual void OnTutorialEnd()
	{
	}

	protected new abstract TutorialManager.TutorialType GetType();

	protected abstract bool IsTutorialCanStart();
}

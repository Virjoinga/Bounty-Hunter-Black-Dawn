using System.Collections.Generic;
using UnityEngine;

public class TweenScaleX : UITweener
{
	public Vector3 from = Vector3.one;

	public Vector3 to = Vector3.one;

	private Transform mTrans;

	public List<UITableSort> mTable = new List<UITableSort>();

	public Transform cachedTransform
	{
		get
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			return mTrans;
		}
	}

	public Vector3 scale
	{
		get
		{
			return cachedTransform.localScale;
		}
		set
		{
			cachedTransform.localScale = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		cachedTransform.localScale = from * (1f - factor) + to * factor;
		if (mTable == null)
		{
			return;
		}
		foreach (UITableSort item in mTable)
		{
			item.repositionNow = true;
		}
	}

	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.scale;
		tweenScale.to = scale;
		return tweenScale;
	}
}

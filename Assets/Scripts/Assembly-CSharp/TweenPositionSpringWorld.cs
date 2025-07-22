using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Position")]
public class TweenPositionSpringWorld : UITweener
{
	public Vector3 from;

	public Vector3 to;

	private Transform mTrans;

	public Vector3 wave;

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

	public Vector3 position
	{
		get
		{
			return cachedTransform.localPosition;
		}
		set
		{
			cachedTransform.localPosition = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		cachedTransform.localPosition = from * (1f - factor) + to * factor;
		cachedTransform.localPosition += wave * Mathf.Sin((float)Math.PI * factor);
	}

	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = tweenPosition.position;
		tweenPosition.to = pos;
		return tweenPosition;
	}
}

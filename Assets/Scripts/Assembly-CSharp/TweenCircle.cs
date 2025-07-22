using System;
using UnityEngine;

public class TweenCircle : UITweener
{
	public Vector3 center;

	public float radius;

	public float from;

	public float to;

	private Transform mTrans;

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
		float f = (from * (1f - factor) + to * factor) * (float)Math.PI / 180f;
		float x = radius * Mathf.Cos(f);
		float y = radius * Mathf.Sin(f);
		cachedTransform.localPosition = center + new Vector3(x, y, 0f);
	}
}

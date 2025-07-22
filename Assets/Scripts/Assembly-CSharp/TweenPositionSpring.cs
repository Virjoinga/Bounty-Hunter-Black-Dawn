using System;
using UnityEngine;

public class TweenPositionSpring : TweenPosition
{
	public Vector3 wave;

	protected override void OnUpdate(float factor, bool isFinished)
	{
		base.OnUpdate(factor, isFinished);
		base.cachedTransform.localPosition += wave * Mathf.Sin((float)Math.PI * factor);
	}
}

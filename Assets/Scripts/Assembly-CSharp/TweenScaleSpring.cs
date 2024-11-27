using System;
using UnityEngine;

public class TweenScaleSpring : TweenScale
{
	public Vector3 wave;

	protected override void OnUpdate(float factor, bool isFinished)
	{
		base.OnUpdate(factor, isFinished);
		base.cachedTransform.localScale += wave * Mathf.Sin((float)Math.PI * factor);
	}
}

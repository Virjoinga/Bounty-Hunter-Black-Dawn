using UnityEngine;

public class TweenPositionDelay : TweenPosition
{
	public float hold;

	private float Hold
	{
		get
		{
			return Mathf.Clamp(hold / duration, 0f, 1f);
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (factor > 1f - Hold)
		{
			base.cachedTransform.localPosition = to;
		}
		else
		{
			base.cachedTransform.localPosition = from * (1f - factor / (1f - Hold)) + to * factor / (1f - Hold);
		}
	}
}

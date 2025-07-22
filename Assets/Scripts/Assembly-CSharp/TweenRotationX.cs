public class TweenRotationX : TweenPosition
{
	protected override void OnUpdate(float factor, bool isFinished)
	{
		base.cachedTransform.eulerAngles = from * (1f - factor) + to * factor;
	}
}

public class TweenSpriteFillAmout : UITweener
{
	public UISprite sprite;

	public float startAmount;

	public float endAmount;

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (sprite != null)
		{
			sprite.fillAmount = startAmount + (endAmount - startAmount) * factor;
		}
	}
}

using UnityEngine;

public class TweenSpriteAlpha : UITweener
{
	public float from;

	public float to;

	public UISprite sprite;

	public bool blink;

	protected override void OnUpdate(float factor, bool isFinished)
	{
		float r = sprite.color.r;
		float g = sprite.color.g;
		float b = sprite.color.b;
		float num = 0f;
		if (blink)
		{
			factor = 1f - Mathf.Abs(factor - 0.5f) * 2f;
			num = from * (1f - factor) + to * factor;
		}
		else
		{
			num = from * (1f - factor) + to * factor;
		}
		sprite.color = new Color(r, g, b, num);
	}
}

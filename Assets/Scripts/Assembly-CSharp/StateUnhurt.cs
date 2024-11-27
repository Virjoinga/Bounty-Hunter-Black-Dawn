using UnityEngine;

public class StateUnhurt : MonoBehaviour
{
	public UISprite unhurtEffect;

	private void OnEnable()
	{
		unhurtEffect.alpha = 1f;
	}

	private void Update()
	{
		float r = unhurtEffect.color.r;
		float g = unhurtEffect.color.g;
		float b = unhurtEffect.color.b;
		unhurtEffect.color = new Color(r, g, b, 1f - UserStateHUD.GetInstance().GetRemainUnhurtTimePercent());
	}
}

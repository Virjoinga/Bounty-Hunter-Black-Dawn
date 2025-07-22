using UnityEngine;

public class VSTDMPointBar : MonoBehaviour
{
	public bool disable;

	public UISprite sprite;

	private void Start()
	{
		if (disable)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void SetStatus(UserStateHUD.VSBattleFieldPoint prevPoint, UserStateHUD.VSBattleFieldPoint nextPoint)
	{
		if (!disable)
		{
			if (prevPoint.Owner == 0)
			{
				sprite.spriteName = GetSpriteName(nextPoint.Owner);
			}
			else if (!prevPoint.IsCapturing)
			{
				sprite.spriteName = GetSpriteName(prevPoint.Owner);
			}
			else if (prevPoint.IsCapturing && nextPoint.IsCapturing)
			{
				sprite.spriteName = GetSpriteName(0);
			}
			sprite.MakePixelPerfect();
		}
	}

	private string GetSpriteName(byte team)
	{
		switch (team)
		{
		case 0:
			return "greyteam";
		case 1:
			return "redteam";
		case 2:
			return "blueteam";
		default:
			return string.Empty;
		}
	}
}

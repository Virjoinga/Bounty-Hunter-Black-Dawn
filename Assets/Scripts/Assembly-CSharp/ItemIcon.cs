using System;
using UnityEngine;

public class ItemIcon : MonoBehaviour
{
	private const float DELAY = 200f;

	public UISprite item;

	public UISprite background;

	private NGUIBaseItem mNGUIBaseItem;

	private DateTime startTime;

	private bool isPressed;

	private bool isPoped;

	public void SetItemIcon(string name)
	{
		item.spriteName = name;
		item.MakePixelPerfect();
	}

	public void SetBackgroundIcon(string name)
	{
		background.spriteName = name;
		background.MakePixelPerfect();
	}

	public void SetItemInfo(NGUIBaseItem nguiBaseItem)
	{
		mNGUIBaseItem = nguiBaseItem;
	}

	private void Update()
	{
		if (isPressed && !isPoped && (DateTime.Now - startTime).TotalMilliseconds > 200.0 && mNGUIBaseItem != null)
		{
			isPoped = true;
			ItemPopMenu.instance.Show(mNGUIBaseItem, true);
		}
	}

	private void OnPress(bool isPressed)
	{
		this.isPressed = isPressed;
		if (isPressed)
		{
			startTime = DateTime.Now;
		}
		else
		{
			isPoped = false;
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class TweenAlphaX : UITweener
{
	private List<UIWidget> list = new List<UIWidget>();

	public float from;

	public float to;

	private void Awake()
	{
		list.Clear();
		UISprite[] componentsInChildren = GetComponentsInChildren<UISprite>();
		UILabel[] componentsInChildren2 = GetComponentsInChildren<UILabel>();
		UISprite[] array = componentsInChildren;
		foreach (UISprite item in array)
		{
			list.Add(item);
		}
		UILabel[] array2 = componentsInChildren2;
		foreach (UILabel item2 in array2)
		{
			list.Add(item2);
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		foreach (UIWidget item in list)
		{
			float r = item.color.r;
			float g = item.color.g;
			float b = item.color.b;
			float a = from * (1f - factor) + to * factor;
			item.color = new Color(r, g, b, a);
		}
	}
}

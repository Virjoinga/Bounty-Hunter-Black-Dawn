using System;
using UnityEngine;

public class UICountDownButton : MonoBehaviour
{
	public int countDown = 10;

	public UILabel label;

	public GameObject receiver;

	public string functionName = "OnClick";

	private DateTime mLastUpdateTime;

	private bool bClick;

	public int TimeLeft
	{
		get
		{
			return countDown - (int)(DateTime.Now - mLastUpdateTime).TotalSeconds;
		}
	}

	private void OnEnable()
	{
		mLastUpdateTime = DateTime.Now;
		bClick = false;
	}

	private void Update()
	{
		int num = ((TimeLeft >= 0) ? TimeLeft : 0);
		if (label != null)
		{
			label.text = string.Empty + num;
		}
		if (!bClick && num == 0)
		{
			bClick = true;
			if (receiver == null)
			{
				SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				receiver.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}

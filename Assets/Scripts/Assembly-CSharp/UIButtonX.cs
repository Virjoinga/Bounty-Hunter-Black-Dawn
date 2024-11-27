using System;
using UnityEngine;

public class UIButtonX : MonoBehaviour
{
	public class ButtonInfo
	{
		public enum Event
		{
			Click = 0,
			Pressing = 1,
			ReleaseAfterPressing = 2,
			NotifyEnd = 3,
			Drop = 4
		}

		public int buttonId;

		public Event buttonEvent;

		public ButtonInfo(int id, Event _Event)
		{
			buttonId = id;
			buttonEvent = _Event;
		}
	}

	public GameObject switchTo;

	public GameObject close;

	public GameObject receiver;

	public string functionName;

	public int buttonId;

	public float dividingLine = 2f;

	public float aliveTime = -1f;

	public bool sendArgs = true;

	private bool isPressed;

	private bool isReleased;

	private bool isShort = true;

	private DateTime startPressTime;

	private DateTime startReleaseTime;

	private void Update()
	{
		if (isPressed && (DateTime.Now - startPressTime).TotalSeconds > (double)dividingLine && isShort)
		{
			isShort = false;
			SendMessage(ButtonInfo.Event.Pressing);
		}
		if (isReleased && (DateTime.Now - startReleaseTime).TotalSeconds > (double)aliveTime)
		{
			isReleased = false;
			SendMessage(ButtonInfo.Event.NotifyEnd);
		}
	}

	private void SendMessage(ButtonInfo.Event _Event)
	{
		if (receiver != null)
		{
			if (sendArgs)
			{
				receiver.SendMessage(functionName, new ButtonInfo(buttonId, _Event), SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				receiver.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void Click()
	{
		SendMessage(ButtonInfo.Event.Click);
		if (switchTo != null)
		{
			switchTo.SetActive(true);
		}
		if (close != null)
		{
			close.SetActive(false);
		}
	}

	private void OnClick()
	{
		if (isShort)
		{
			Click();
		}
	}

	private void OnDrop(GameObject go)
	{
		SendMessage(ButtonInfo.Event.Drop);
	}

	private void OnPress(bool isPressed)
	{
		this.isPressed = isPressed;
		if (aliveTime > 0f)
		{
			isReleased = !isPressed;
		}
		if (isPressed)
		{
			isShort = true;
			startPressTime = DateTime.Now;
			return;
		}
		if (!isShort)
		{
			SendMessage(ButtonInfo.Event.ReleaseAfterPressing);
		}
		startReleaseTime = DateTime.Now;
	}
}

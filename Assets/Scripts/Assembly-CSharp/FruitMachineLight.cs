using System;
using UnityEngine;

public class FruitMachineLight : MonoBehaviour
{
	public UISprite sprite;

	public string nameLightUp;

	public string nameLightOut;

	public float duration = 400f;

	private DateTime mLastUpdateTime;

	private bool mBlinkState;

	private bool mLightState;

	public void LightUp()
	{
		Light(nameLightUp, true);
	}

	public void LightOut()
	{
		Light(nameLightOut, false);
	}

	private void Light(string name, bool state)
	{
		Vector3 localPosition = sprite.transform.localPosition;
		sprite.spriteName = name;
		sprite.MakePixelPerfect();
		sprite.transform.localPosition = localPosition;
		mLightState = state;
	}

	public void Blink(bool state)
	{
		Blink(state, duration);
	}

	public void Blink(bool state, float duration)
	{
		mBlinkState = state;
		this.duration = duration;
	}

	private void Awake()
	{
		LightOut();
		mLastUpdateTime = DateTime.Now;
	}

	private void Update()
	{
		if (mBlinkState && (DateTime.Now - mLastUpdateTime).TotalMilliseconds > (double)duration)
		{
			if (Camera.main != null)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/fruit_machine_stop");
			}
			mLastUpdateTime = DateTime.Now;
			if (mLightState)
			{
				LightOut();
			}
			else
			{
				LightUp();
			}
		}
	}
}

using System;
using UnityEngine;

public class FruitMachineBonus : MonoBehaviour
{
	public UISprite state;

	public UILabel bonusLabel;

	public UIFilledSprite bonusBar;

	public ItemQuality itemQuality;

	private DateTime mBarUpdateTime;

	private DateTime mStateUpdateTime;

	private FruitMachineConfig mFruitMachineConfig;

	private void Start()
	{
		mBarUpdateTime = DateTime.Now;
		mStateUpdateTime = DateTime.Now;
		bonusBar.fillAmount = 0f;
	}

	private void Update()
	{
		if (mFruitMachineConfig == null)
		{
			return;
		}
		TimeSpan timeSpan = DateTime.Now - mBarUpdateTime;
		mBarUpdateTime = DateTime.Now;
		float bonusUseTimesPercent = mFruitMachineConfig.GetBonusUseTimesPercent(itemQuality);
		if (bonusUseTimesPercent >= bonusBar.fillAmount)
		{
			bonusBar.fillAmount += (bonusUseTimesPercent - bonusBar.fillAmount) * ((float)timeSpan.Milliseconds / 200f);
		}
		else
		{
			bonusBar.fillAmount += (1f - bonusBar.fillAmount) * ((float)timeSpan.Milliseconds / 200f);
			if ((double)bonusBar.fillAmount >= 0.98)
			{
				bonusBar.fillAmount = 0f;
			}
		}
		bonusLabel.text = mFruitMachineConfig.GetBonusPointDiscription(itemQuality);
		if (mFruitMachineConfig.IsBonusPointEnough(itemQuality))
		{
			if ((DateTime.Now - mStateUpdateTime).Milliseconds > 200)
			{
				mStateUpdateTime = DateTime.Now;
				string text = "libao_d";
				string spriteName = "libao_bk";
				if (state.spriteName.Equals(text))
				{
					state.spriteName = spriteName;
					state.MakePixelPerfect();
				}
				else
				{
					state.spriteName = text;
					state.MakePixelPerfect();
				}
			}
		}
		else
		{
			string text2 = "libao_bk";
			if (!state.spriteName.Equals(text2))
			{
				state.spriteName = text2;
				state.MakePixelPerfect();
			}
		}
	}

	public void SetFruitMachineConfig(FruitMachineConfig fruitMachineConfig)
	{
		mFruitMachineConfig = fruitMachineConfig;
	}
}

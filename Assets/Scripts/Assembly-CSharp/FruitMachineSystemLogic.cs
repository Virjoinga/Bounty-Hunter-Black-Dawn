using System;
using UnityEngine;

public class FruitMachineSystemLogic
{
	private static int REPEAT_TIMES = 10;

	private bool bStart;

	private int mCurrentIndex;

	private int mStartIndex;

	private int mEndIndex;

	private int mTotal = 1;

	private int mDelay;

	private DateTime mLastUpdateTime;

	private int mDelayIndex
	{
		get
		{
			return mTotal * 3 / 2;
		}
	}

	public FruitMachineSystemLogic(int total)
	{
		mTotal = Mathf.Max(1, total);
	}

	public int Update()
	{
		if (bStart)
		{
			if ((DateTime.Now - mLastUpdateTime).TotalMilliseconds > (double)mDelay)
			{
				mLastUpdateTime = DateTime.Now;
				mCurrentIndex++;
				if (mCurrentIndex - mStartIndex < mDelayIndex)
				{
					mDelay = (mDelayIndex - (mCurrentIndex - mStartIndex)) * mDelayIndex;
				}
				else if (mEndIndex - mCurrentIndex < mDelayIndex)
				{
					mDelay = (mDelayIndex - (mEndIndex - mCurrentIndex)) * mDelayIndex;
				}
				else
				{
					mDelay = 20;
				}
			}
			bStart = ((mCurrentIndex != mEndIndex) ? true : false);
		}
		return mCurrentIndex % mTotal;
	}

	public int Start(int index)
	{
		return Start(0, index);
	}

	public int Start(int startIndex, int endIndex)
	{
		bStart = true;
		mCurrentIndex = (mStartIndex = Mathf.Clamp(startIndex, 0, mTotal - 1));
		mEndIndex = Mathf.Clamp(endIndex, 0, mTotal - 1) + REPEAT_TIMES * mTotal;
		mLastUpdateTime = DateTime.Now;
		return mCurrentIndex;
	}

	public bool IsRun()
	{
		return bStart;
	}

	public void Skip()
	{
		bStart = false;
		mCurrentIndex = mEndIndex;
	}
}

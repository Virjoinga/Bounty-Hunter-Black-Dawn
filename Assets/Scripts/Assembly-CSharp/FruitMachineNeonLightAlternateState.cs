using System;

public class FruitMachineNeonLightAlternateState : FruitMachineNeonLightState
{
	private FruitMachineLight[] mFruitMachineLightList;

	private DateTime mLastUpdateTime;

	private bool bSingle;

	public FruitMachineNeonLightAlternateState(FruitMachineLight[] fruitMachineLightList)
		: base(fruitMachineLightList)
	{
		mFruitMachineLightList = fruitMachineLightList;
		mLastUpdateTime = DateTime.MinValue;
		bSingle = true;
	}

	protected override void OnUpdate()
	{
		if (!((DateTime.Now - mLastUpdateTime).TotalMilliseconds > 500.0))
		{
			return;
		}
		mLastUpdateTime = DateTime.Now;
		for (int i = 0; i < mFruitMachineLightList.Length; i++)
		{
			mFruitMachineLightList[i].LightOut();
			if ((bSingle && i % 2 == 1) || (!bSingle && i % 2 == 0))
			{
				mFruitMachineLightList[i].LightUp();
			}
		}
		bSingle = !bSingle;
	}
}

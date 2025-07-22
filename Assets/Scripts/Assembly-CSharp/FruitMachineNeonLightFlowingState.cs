using System;

public class FruitMachineNeonLightFlowingState : FruitMachineNeonLightState
{
	private FruitMachineLight[] mFruitMachineLightList;

	private DateTime mLastUpdateTime;

	private float mFlowingDelay;

	private int index;

	private bool revert;

	public FruitMachineNeonLightFlowingState(FruitMachineLight[] fruitMachineLightList, float flowingDelay)
		: base(fruitMachineLightList)
	{
		mFruitMachineLightList = fruitMachineLightList;
		mLastUpdateTime = DateTime.MinValue;
		index = 0;
		revert = false;
		mFlowingDelay = flowingDelay;
	}

	protected override void OnUpdate()
	{
		if ((DateTime.Now - mLastUpdateTime).TotalMilliseconds > (double)mFlowingDelay)
		{
			mLastUpdateTime = DateTime.Now;
			if (revert)
			{
				mFruitMachineLightList[index].LightOut();
			}
			else
			{
				mFruitMachineLightList[index].LightUp();
			}
			index++;
			if (index == mFruitMachineLightList.Length)
			{
				index = 0;
				revert = !revert;
			}
		}
	}
}

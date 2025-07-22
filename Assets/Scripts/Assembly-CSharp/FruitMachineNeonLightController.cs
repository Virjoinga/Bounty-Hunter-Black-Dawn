using UnityEngine;

public class FruitMachineNeonLightController : MonoBehaviour
{
	private FruitMachineLight[] mFruitMachineLightList;

	private FruitMachineNeonLightState state;

	private FruitMachineNeonLightBlinkType type;

	private void Awake()
	{
		mFruitMachineLightList = GetComponentsInChildren<FruitMachineLight>();
		if (mFruitMachineLightList.Length == 0)
		{
			Object.Destroy(this);
			return;
		}
		state = new FruitMachineNeonLightNoneState(mFruitMachineLightList);
		type = FruitMachineNeonLightBlinkType.None;
	}

	private void Update()
	{
		state.Update();
	}

	public void Blink(FruitMachineNeonLightBlinkType type)
	{
		if (this.type != type)
		{
			switch (type)
			{
			case FruitMachineNeonLightBlinkType.None:
				state = new FruitMachineNeonLightNoneState(mFruitMachineLightList);
				break;
			case FruitMachineNeonLightBlinkType.Alternate:
				state = new FruitMachineNeonLightAlternateState(mFruitMachineLightList);
				break;
			case FruitMachineNeonLightBlinkType.Flowing:
				state = new FruitMachineNeonLightFlowingState(mFruitMachineLightList, 20f);
				break;
			}
		}
	}
}

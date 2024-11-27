using UnityEngine;

public class FruitMachineNeonLightConsole : MonoBehaviour
{
	private FruitMachineNeonLightController[] mFruitMachineNeonLightControlList;

	private void Awake()
	{
		mFruitMachineNeonLightControlList = GetComponentsInChildren<FruitMachineNeonLightController>();
		if (mFruitMachineNeonLightControlList.Length == 0)
		{
			Object.Destroy(this);
		}
	}

	public void Blink(FruitMachineNeonLightBlinkType type)
	{
		FruitMachineNeonLightController[] array = mFruitMachineNeonLightControlList;
		foreach (FruitMachineNeonLightController fruitMachineNeonLightController in array)
		{
			fruitMachineNeonLightController.Blink(type);
		}
	}
}

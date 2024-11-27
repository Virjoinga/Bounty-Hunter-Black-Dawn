public abstract class FruitMachineNeonLightState
{
	public FruitMachineNeonLightState(FruitMachineLight[] fruitMachineLightList)
	{
		foreach (FruitMachineLight fruitMachineLight in fruitMachineLightList)
		{
			fruitMachineLight.LightOut();
		}
	}

	public void Update()
	{
		OnUpdate();
	}

	protected virtual void OnUpdate()
	{
	}
}

public class FruitMachineBundle
{
	private UIFruitMachine mUIFruitMachine;

	private FruitMachineIntent mFruitMachineIntent;

	public FruitMachineBundle(UIFruitMachine uiFruitMachine, FruitMachineIntent intent)
	{
		mUIFruitMachine = uiFruitMachine;
		mFruitMachineIntent = intent;
	}

	public UIFruitMachine GetUIFruitMachine()
	{
		return mUIFruitMachine;
	}

	public FruitMachineIntent GetFruitMachineIntent()
	{
		return mFruitMachineIntent;
	}
}

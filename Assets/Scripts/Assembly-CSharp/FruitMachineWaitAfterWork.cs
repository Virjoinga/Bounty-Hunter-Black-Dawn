using System;

public class FruitMachineWaitAfterWork : FruitMachineState
{
	private UIFruitMachine mUIFruitMachine;

	private FruitMachineLight[] mFruitMachineItemLightList;

	private FruitMachineLight[] mFruitMachineItemGuidingLightList;

	private int whichIndex;

	private DateTime mLastUpdateTime;

	private bool bBlink;

	protected override void OnStart(FruitMachineBundle bundle)
	{
		mUIFruitMachine = bundle.GetUIFruitMachine();
		mUIFruitMachine.LightOutAllItemLight();
		mFruitMachineItemLightList = mUIFruitMachine.GetFruitMachineItemLightList();
		mFruitMachineItemGuidingLightList = mUIFruitMachine.GetFruitMachineItemGuidingLightList();
		FruitMachineIntent fruitMachineIntent = bundle.GetFruitMachineIntent();
		if (fruitMachineIntent == null)
		{
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_NOT_WORK);
			return;
		}
		whichIndex = Convert.ToInt32(fruitMachineIntent.Get("ItemIndex"));
		mFruitMachineItemLightList[whichIndex].LightUp();
		mFruitMachineItemGuidingLightList[whichIndex].LightUp();
		mLastUpdateTime = DateTime.Now;
		bBlink = false;
		mUIFruitMachine.fruitMachineNeonLightConsole.Blink(FruitMachineNeonLightBlinkType.Alternate);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		TimeSpan timeSpan = DateTime.Now - mLastUpdateTime;
		if (timeSpan.TotalMilliseconds > 3500.0)
		{
			FruitMachineIntent fruitMachineIntent = new FruitMachineIntent();
			ItemBase item = mUIFruitMachine.GetGambleConfig().GetItem(whichIndex);
			fruitMachineIntent.Put("Item", item);
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_GET_ITEM, fruitMachineIntent);
			item = null;
		}
		else if (timeSpan.TotalMilliseconds > 1000.0 && !bBlink)
		{
			bBlink = true;
			mFruitMachineItemLightList[whichIndex].Blink(true, 50f);
			mFruitMachineItemGuidingLightList[whichIndex].Blink(true, 50f);
		}
	}

	protected override void OnExit()
	{
		base.OnExit();
		mFruitMachineItemLightList[whichIndex].Blink(false);
		mFruitMachineItemLightList = null;
		mFruitMachineItemGuidingLightList[whichIndex].Blink(false);
		mFruitMachineItemGuidingLightList = null;
		mUIFruitMachine = null;
	}
}

using UnityEngine;

public class FruitMachineWorking : FruitMachineState
{
	private UIFruitMachine mUIFruitMachine;

	private FruitMachineSystemLogic mFruitMachineSystemLogic;

	private FruitMachineLight[] mFruitMachineItemLightList;

	private FruitMachineLight[] mFruitMachineItemGuidingLightList;

	private int lastIndex;

	private bool bSkip;

	protected override void OnStart(FruitMachineBundle bundle)
	{
		mUIFruitMachine = bundle.GetUIFruitMachine();
		mFruitMachineItemLightList = mUIFruitMachine.GetFruitMachineItemLightList();
		mFruitMachineItemGuidingLightList = mUIFruitMachine.GetFruitMachineItemGuidingLightList();
		mFruitMachineSystemLogic = new FruitMachineSystemLogic(mFruitMachineItemLightList.Length);
		mFruitMachineSystemLogic.Start(11, mUIFruitMachine.GetGambleConfig().GetRandomItemId());
		mUIFruitMachine.fruitMachineNeonLightConsole.Blink(FruitMachineNeonLightBlinkType.Flowing);
		lastIndex = -1;
		bSkip = false;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		mUIFruitMachine.LightOutAllItemLight();
		int num = mFruitMachineSystemLogic.Update();
		mFruitMachineItemLightList[num].LightUp();
		mFruitMachineItemGuidingLightList[num].LightUp();
		if (lastIndex != num)
		{
			lastIndex = num;
			if (Camera.main != null)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/fruit_machine_rolling");
			}
		}
		if (!mFruitMachineSystemLogic.IsRun())
		{
			if (bSkip)
			{
				FruitMachineIntent fruitMachineIntent = new FruitMachineIntent();
				ItemBase item = mUIFruitMachine.GetGambleConfig().GetItem(num);
				fruitMachineIntent.Put("Item", item);
				item = null;
				mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_GET_ITEM, fruitMachineIntent);
			}
			else
			{
				FruitMachineIntent fruitMachineIntent2 = new FruitMachineIntent();
				fruitMachineIntent2.Put("ItemIndex", string.Empty + num);
				mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_WAIT_AFTER_WORK, fruitMachineIntent2);
			}
		}
	}

	protected override void OnClick(GameObject go)
	{
		base.OnClick(go);
		if (go.Equals(mUIFruitMachine.fruitMachineSkip))
		{
			mFruitMachineSystemLogic.Skip();
			bSkip = true;
		}
	}

	protected override void OnExit()
	{
		base.OnExit();
		mUIFruitMachine = null;
		mFruitMachineSystemLogic = null;
		mFruitMachineItemLightList = null;
		mFruitMachineItemGuidingLightList = null;
		bSkip = false;
	}
}

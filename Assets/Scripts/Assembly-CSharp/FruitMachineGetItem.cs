using UnityEngine;

public class FruitMachineGetItem : FruitMachineState, ItemPopMenuEventListener, UIMsgListener
{
	private UIFruitMachine mUIFruitMachine;

	protected override void OnStart(FruitMachineBundle bundle)
	{
		mUIFruitMachine = bundle.GetUIFruitMachine();
		FruitMachineIntent fruitMachineIntent = bundle.GetFruitMachineIntent();
		Debug.Log("intent : " + fruitMachineIntent);
		if (fruitMachineIntent == null)
		{
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_NOT_WORK);
			return;
		}
		ItemBase itemBase = (ItemBase)fruitMachineIntent.Get("Item");
		Debug.Log("itemBase : " + itemBase);
		NGUIBaseItem mNGUIBaseItem = itemBase.mNGUIBaseItem;
		if (itemBase.ItemCanBePickedUp())
		{
			itemBase.PickUpItem();
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_BAG_FULL"), 2, 30);
			GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(itemBase.mNGUIBaseItem, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition() + Vector3.up, Vector3.zero);
			Object.Destroy(itemBase.gameObject);
		}
		itemBase = null;
		GameApp.GetInstance().Save();
		mUIFruitMachine.fruitMachineCongratulations.SetActive(true);
		mUIFruitMachine.fruitMachineCongratulations.GetComponentInChildren<UIGambleItem>().Show(mNGUIBaseItem, this);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 30 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}

	public void OnClickWindow()
	{
		if (mUIFruitMachine.gambleType == GambleType.GoldFruitMachine && !GameApp.GetInstance().GetGlobalState().HaveRate())
		{
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_RATE);
		}
		else
		{
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_NOT_WORK);
		}
	}

	protected override void OnExit()
	{
		base.OnExit();
		mUIFruitMachine.fruitMachineCongratulations.SetActive(false);
		mUIFruitMachine = null;
	}
}

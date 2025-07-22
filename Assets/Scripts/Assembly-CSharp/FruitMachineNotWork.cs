using UnityEngine;

public class FruitMachineNotWork : FruitMachineState, UIMsgListener
{
	private UIFruitMachine mUIFruitMachine;

	protected override void OnStart(FruitMachineBundle bundle)
	{
		mUIFruitMachine = bundle.GetUIFruitMachine();
		mUIFruitMachine.LightOutAllItemLight();
		mUIFruitMachine.fruitMachineNeonLightConsole.Blink(FruitMachineNeonLightBlinkType.Alternate);
		mUIFruitMachine.fruitMachineSkip.gameObject.SetActive(false);
		mUIFruitMachine.fruitMachineReset.gameObject.SetActive(true);
	}

	protected override void OnClick(GameObject go)
	{
		base.OnClick(go);
		if (go.Equals(mUIFruitMachine.fruitMachineEsc))
		{
			UIFruitMachine.Close();
			return;
		}
		if (go.Equals(mUIFruitMachine.fruitMachineReset))
		{
			GambleConfig.GambleResult gambleResult = mUIFruitMachine.GetGambleConfig().Reset(GambleConfig.ResetType.ClickButton);
			if (gambleResult.Success)
			{
				mUIFruitMachine.RefreshItems();
			}
			else
			{
				UIMsgBox.instance.ShowMessage(this, gambleResult.Discription, 2, 9);
			}
			return;
		}
		FruitMachineConfig fruitMachineConfig = (FruitMachineConfig)mUIFruitMachine.GetGambleConfig();
		ItemBase itemBase = null;
		if (go.Equals(mUIFruitMachine.GetFruitMachineBonusList()[0].gameObject))
		{
			itemBase = fruitMachineConfig.UseBonusPoint(ItemQuality.Uncommon);
		}
		else if (go.Equals(mUIFruitMachine.GetFruitMachineBonusList()[1].gameObject))
		{
			itemBase = fruitMachineConfig.UseBonusPoint(ItemQuality.Rare);
		}
		else if (go.Equals(mUIFruitMachine.GetFruitMachineBonusList()[2].gameObject))
		{
			itemBase = fruitMachineConfig.UseBonusPoint(ItemQuality.Epic);
		}
		else if (go.Equals(mUIFruitMachine.GetFruitMachineBonusList()[3].gameObject))
		{
			itemBase = fruitMachineConfig.UseBonusPoint(ItemQuality.Legendary);
		}
		if (itemBase != null)
		{
			FruitMachineIntent fruitMachineIntent = new FruitMachineIntent();
			fruitMachineIntent.Put("Item", itemBase);
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_GET_ITEM, fruitMachineIntent);
		}
	}

	protected override void OnDrag(GameObject go, Vector2 delta)
	{
		base.OnDrag(go, delta);
		if (go.Equals(mUIFruitMachine.fruitMachineBar.gameObject) && delta.y < 0f && !mUIFruitMachine.fruitMachineBar.IsMax())
		{
			mUIFruitMachine.fruitMachineBar.PressOffsetY(delta.y);
			if (mUIFruitMachine.fruitMachineBar.IsMax() && Camera.main != null)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/fruit_machine_pull");
			}
		}
	}

	protected override void OnPress(GameObject go, bool isPressed)
	{
		base.OnPress(go, isPressed);
		if (isPressed || !go.Equals(mUIFruitMachine.fruitMachineBar.gameObject) || !mUIFruitMachine.fruitMachineBar.Release())
		{
			return;
		}
		GambleConfig.GambleResult gambleResult = mUIFruitMachine.GetGambleConfig().Use();
		if (gambleResult.Success)
		{
			if (Camera.main != null)
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/fruit_machine_release");
			}
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Banker_Loves_You);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_WORKING);
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, gambleResult.Discription, 2, 9);
		}
	}

	protected override void OnExit()
	{
		base.OnExit();
		mUIFruitMachine.fruitMachineSkip.gameObject.SetActive(true);
		mUIFruitMachine.fruitMachineReset.gameObject.SetActive(false);
		mUIFruitMachine = null;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 9 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
			UIIAP.Show(UIIAP.Type.IAP);
		}
	}
}

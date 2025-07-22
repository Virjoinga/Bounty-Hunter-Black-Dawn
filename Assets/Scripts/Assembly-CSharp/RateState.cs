using UnityEngine;

public class RateState : FruitMachineState, UIMsgListener
{
	private UIFruitMachine mUIFruitMachine;

	protected override void OnStart(FruitMachineBundle bundle)
	{
		mUIFruitMachine = bundle.GetUIFruitMachine();
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_FIVE_STARS").Replace("%d", string.Empty + 100), 3, 48);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 48)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				Application.OpenURL(UIConstant.RATE_PAGE_URL);
			}
			GameApp.GetInstance().GetGlobalState().SetRate(true);
			GameApp.GetInstance().Save();
			UIMsgBox.instance.CloseMessage();
			mUIFruitMachine.GoToNextState(UIFruitMachine.STATE_NOT_WORK);
		}
	}

	protected override void OnExit()
	{
		base.OnExit();
		mUIFruitMachine = null;
	}
}

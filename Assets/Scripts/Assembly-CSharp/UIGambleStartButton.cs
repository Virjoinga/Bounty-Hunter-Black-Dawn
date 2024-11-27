using System;
using UnityEngine;

public class UIGambleStartButton : UIDelegateMenu, UIMsgListener
{
	public GameObject validState;

	public GameObject invalidState;

	public UILabel costLabel;

	private DateTime mLastUpdateTime;

	private void Awake()
	{
		AddDelegate(validState);
	}

	private void Update()
	{
		if (!((DateTime.Now - mLastUpdateTime).TotalMilliseconds > 200.0))
		{
			return;
		}
		mLastUpdateTime = DateTime.Now;
		costLabel.text = GambleManagerAbandoned.GetInstance().GetPriceInfo();
		if (GambleManagerAbandoned.GetInstance().IsUsing())
		{
			if (validState.active)
			{
				validState.SetActiveRecursively(false);
			}
			if (!invalidState.active)
			{
				invalidState.SetActiveRecursively(true);
			}
		}
		else
		{
			if (!validState.active)
			{
				validState.SetActiveRecursively(true);
			}
			if (invalidState.active)
			{
				invalidState.SetActiveRecursively(false);
			}
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (GambleManagerAbandoned.GetInstance().Use())
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Banker_Loves_You);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, GambleManagerAbandoned.GetInstance().GetWarning(), 2, 9);
		}
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

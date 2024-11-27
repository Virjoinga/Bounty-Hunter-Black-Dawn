using UnityEngine;

public class TutorialEquipmentUpgrade : TutorialScript, UIMsgListener
{
	protected override void OnTutorialStart()
	{
		base.OnTutorialStart();
		if (HUDManager.instance != null)
		{
			HUDManager.instance.m_HotKeyManager.ForbidAll();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
		}
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_UPGRADE_STATION"), 2, 61);
	}

	protected override void OnTutorialEnd()
	{
		base.OnTutorialEnd();
		if (HUDManager.instance != null)
		{
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = false;
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = false;
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 61 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
			EndTutorial();
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.EquipmentUpgrade;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsNewPlayerTrigger() || IsSeniorPlayerTrigger();
	}

	private bool IsNewPlayerTrigger()
	{
		return !IsTutorialOk(TutorialManager.TutorialType.EquipmentUpgrade) && TutorialManager.GetInstance().IsShopTutorialOk() && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 28;
	}

	private bool IsSeniorPlayerTrigger()
	{
		return !IsTutorialOk(TutorialManager.TutorialType.EquipmentUpgrade) && (TutorialManager.GetInstance().IsMapTutorialOk() || (TutorialManager.GetInstance().IsShopTutorialOk() && !TutorialManager.GetInstance().IsMapTutorialOk()));
	}
}

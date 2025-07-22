using UnityEngine;

public class LeaveArenaScript : MonoBehaviour, UIMsgListener
{
	public float radius = 2f;

	private bool bOK = true;

	private bool bTeleportActive;

	private bool bShowingMsg;

	private void Start()
	{
	}

	private void Update()
	{
		if (bTeleportActive)
		{
			return;
		}
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null || !(localPlayer.GetTransform() != null))
		{
			return;
		}
		if (Vector3.Distance(localPlayer.GetTransform().position, base.transform.position) < radius)
		{
			if (bOK)
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_LEAVE_ARENA"), 3);
				bShowingMsg = true;
				bOK = false;
			}
		}
		else if (!bOK && !bShowingMsg)
		{
			bOK = true;
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		UIMsgBox.instance.CloseMessage();
		bShowingMsg = false;
		if (buttonId == UIMsg.UIMsgButton.Ok)
		{
			EnemySpawnScript.GetInstance().OnArenaGameEnd(GameApp.GetInstance().GetGameWorld().GetLocalPlayer(), false);
		}
	}
}

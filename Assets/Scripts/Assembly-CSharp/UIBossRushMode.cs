using UnityEngine;

public class UIBossRushMode : UIDelegateMenu
{
	public Mode mode = Mode.BossRushRookie;

	public GameObject[] buttonGos;

	private void Awake()
	{
		GameObject[] array = buttonGos;
		foreach (GameObject obj in array)
		{
			AddDelegate(obj);
		}
	}

	private void OnEnable()
	{
		UIBossRushTeam.BossRushMode = mode;
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		for (int i = 0; i < buttonGos.Length; i++)
		{
			if (go.Equals(buttonGos[i]))
			{
				if (GameApp.GetInstance().GetGameMode().TypeOfNetwork == NetworkType.Single)
				{
					GameApp.GetInstance().GetUIStateManager().FrGoToPhase(43, false, false, false);
					break;
				}
				InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetUserID(), InvitationRequest.Type.BossRush, SubMode.Story, GameApp.GetInstance().GetGameWorld().CurrentSceneID, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				break;
			}
		}
	}
}

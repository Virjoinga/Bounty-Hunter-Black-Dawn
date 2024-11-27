using UnityEngine;

public class UIVSBattleShop : MonoBehaviour
{
	public UILabel countdownNumber;

	public UITweenX numberTween;

	private Timer timer;

	private int curTimes;

	private static int times;

	private static GameObject uiVSBattleShop;

	private static bool IsReady;

	private void Awake()
	{
		curTimes = 0;
		timer = new Timer();
		timer.SetTimer(1f, true);
	}

	private void OnDisable()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
			if (localPlayer != null)
			{
				localPlayer.InputController.Block = false;
			}
		}
		if (HUDManager.instance != null && HUDManager.instance.m_HotKeyManager != null)
		{
			HUDManager.instance.m_HotKeyManager.CancelFobid();
		}
	}

	private void Update()
	{
		if (GameApp.GetInstance().GetGameWorld() != null)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
			HUDManager.instance.m_HotKeyManager.ForbidAllWithout(7);
		}
		if (!timer.Ready())
		{
			return;
		}
		if (curTimes < times)
		{
			countdownNumber.text = string.Empty + (times - curTimes);
			numberTween.Play();
			timer.Do();
			curTimes++;
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_match_count_down");
		}
		else if (!IsReady)
		{
			if (!UserStateHUD.GetInstance().IsUserDead())
			{
				Close();
			}
		}
		else
		{
			Close();
		}
	}

	public static bool ShowReady()
	{
		IsReady = true;
		return Show(10);
	}

	public static bool ShowRevive()
	{
		return Show(10);
	}

	public static bool Show(int duration)
	{
		if (uiVSBattleShop == null)
		{
			times = duration;
			GameObject original = ResourceLoad.GetInstance().LoadUI("VS", "VSUIBattleShop");
			uiVSBattleShop = Object.Instantiate(original) as GameObject;
			return true;
		}
		return false;
	}

	public static bool Close()
	{
		if (uiVSBattleShop != null)
		{
			MemoryManager.FreeNGUI(uiVSBattleShop);
			uiVSBattleShop = null;
			if (IsReady)
			{
				AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_match_start");
				UserStateHUD.GetInstance().SetVSTDMTeamSignVisible(UserStateHUD.GetInstance().GetUserTeamName());
				IsReady = false;
			}
			return true;
		}
		return false;
	}

	public static bool IsShow()
	{
		return uiVSBattleShop != null;
	}
}

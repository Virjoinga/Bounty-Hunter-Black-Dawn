using System;
using System.IO;
using UnityEngine;

public abstract class UIStateManager : MonoBehaviour
{
	public const byte PHASE_INIT = 0;

	public const byte PHASE_LOADING = 1;

	public const byte PHASE_MAINMENU = 2;

	public const byte PHASE_CUSTOMIZE = 3;

	public const byte PHASE_STORE = 4;

	public const byte PHASE_MAKE_PACKAGE = 5;

	public const byte PHASE_GAME = 6;

	public const byte PHASE_STATISTICS = 7;

	public const byte PHASE_OPTIONS = 8;

	public const byte PHASE_MULTI_PLAYER = 9;

	public const byte PHASE_STORE_PROPS = 10;

	public const byte PHASE_EXTRA = 11;

	public const byte PHASE_READY_GAME = 12;

	public const byte PHASE_STAGE_CHOISE = 13;

	public const byte PHASE_PAUSE = 14;

	public const byte PHASE_OPTIONS_INGAME = 15;

	public const byte PHASE_NET_STATISTICS = 16;

	public const byte PHASE_QUEST = 17;

	public const byte PHASE_CREDIT = 18;

	public const byte PHASE_SETTING = 19;

	public const byte PHASE_CHARACTER_OPERAION = 20;

	public const byte PHASE_BUBBLE = 21;

	public const byte PHASE_BAG = 22;

	public const byte PHASE_SKILL_TREE = 23;

	public const byte PHASE_QUEST_LOG = 24;

	public const byte PHASE_MINIMAP = 25;

	public const byte PHASE_ACHIEVEMENT = 26;

	public const byte PHASE_ENTRANCE = 27;

	public const byte PHASE_SHOP = 28;

	public const byte PHASE_ARENA = 29;

	public const byte PHASE_GAMBLE = 34;

	public const byte PHASE_ADSVIDEO = 35;

	public const byte PHASE_ADSNOVIDEO = 36;

	public const byte PHASE_VS_SINGLE = 37;

	public const byte PHASE_VS_COOP = 38;

	public const byte PHASE_AVATAR_SHOP = 39;

	public const byte PHASE_EQUIPMENT_UPGRADE = 40;

	public const byte PHASE_VS_MULTI_PLAYER = 41;

	public const byte PHASE_BOSS_RUSH = 42;

	public const byte PHASE_BOSS_RUSH_MULTI_PLAYER = 43;

	protected const byte PHASE_LOGO = 30;

	protected const byte PHASE_SPLASH = 31;

	protected const byte PHASE_GOTO_START_MENU = 32;

	protected const byte PHASE_TUTORIAL = 33;

	private const byte FR_EXCEPTION_RUN = 101;

	private int nPhasePrevious = -1;

	private int nPhaseCurrent = -1;

	private int nPhaseNext = -1;

	private bool bPhaseInit;

	private bool bSkipPhase;

	private bool bSkipInit;

	private bool bSkipClose;

	private bool bIgnorePrevious;

	private bool bChangePhase;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		try
		{
			if (bChangePhase)
			{
				bChangePhase = false;
				if (!bSkipClose)
				{
					FrClose(nPhaseCurrent);
				}
				if (!bIgnorePrevious)
				{
					nPhasePrevious = nPhaseCurrent;
				}
				nPhaseCurrent = nPhaseNext;
				if (!bSkipInit)
				{
					FrInit(nPhaseCurrent);
				}
				bPhaseInit = true;
				if (nPhaseNext != nPhaseCurrent)
				{
					bSkipPhase = true;
				}
			}
			else
			{
				bPhaseInit = false;
			}
			if (bSkipPhase)
			{
				bSkipPhase = false;
			}
			else
			{
				FrUpdate(nPhaseCurrent);
			}
		}
		catch (IOException e)
		{
			exceptionCaught(e, 101);
		}
		finally
		{
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			if (networkManager != null && networkManager.IsDisconnected)
			{
				GameApp.GetInstance().GetNetworkManager().CloseConnection();
			}
		}
	}

	public void FrGoToPhase(int phase)
	{
		FrGoToPhase(phase, false, false, false);
	}

	protected abstract void FrInit(int phase);

	protected abstract void FrClose(int phase);

	protected abstract void FrUpdate(int phase);

	public abstract void FrFree();

	public void FrGoToPhase(int phase, bool skipInit, bool skipClose, bool ignorePrevious)
	{
		bChangePhase = true;
		nPhaseNext = phase;
		bSkipInit = skipInit;
		bSkipClose = skipClose;
		bIgnorePrevious = ignorePrevious;
	}

	public bool FrIsInitPhase()
	{
		return bPhaseInit;
	}

	public int FrGetCurrentPhase()
	{
		return nPhaseCurrent;
	}

	public int FrGetPreviousPhase()
	{
		return nPhasePrevious;
	}

	public int FrGetNextPhase()
	{
		return nPhaseNext;
	}

	public static void exceptionCaught(Exception e, int location)
	{
		string message = location + ":" + e.ToString();
		Debug.Log(message);
	}

	private void SendLocalNotification(int hours)
	{
	}

	public void OnApplicationPause()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log("---OnApplicationPause---");
			Debug.Log("---ToBackground " + GameApp.GetInstance().IsToBackground + "---");
			if (GameApp.GetInstance().IsToBackground)
			{
				GameApp.GetInstance().IsToBackground = false;
				AchievementManager.GetInstance().Report();
				GameApp.GetInstance().Save();
			}
		}
	}

	public void OnApplicationQuit()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			if (networkManager != null)
			{
				networkManager.CloseConnection();
			}
		}
		Debug.Log("---OnApplicationQuit---");
		AchievementManager.GetInstance().Report();
		GameApp.GetInstance().Save();
	}
}

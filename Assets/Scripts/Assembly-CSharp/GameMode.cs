public class GameMode
{
	public NetworkType TypeOfNetwork { get; set; }

	public Mode ModePlay { get; set; }

	public PlayerStateNetwork PlayerStatus { get; set; }

	public SubMode SubModePlay { get; set; }

	public GameMode(NetworkType nType, Mode gMode)
	{
		TypeOfNetwork = nType;
		ModePlay = gMode;
		SubModePlay = SubMode.Story;
	}

	public bool IsSingle()
	{
		if (TypeOfNetwork == NetworkType.Single || GameApp.GetInstance().GetNetworkManager() == null)
		{
			return true;
		}
		return false;
	}

	public bool IsMultiPlayer()
	{
		NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
		if (networkManager == null || !networkManager.IsConnected() || TypeOfNetwork != NetworkType.MultiPlayer_Internet)
		{
			return false;
		}
		return true;
	}

	public bool IsConnNetwork()
	{
		NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
		if (networkManager == null || !networkManager.IsConnected())
		{
			return false;
		}
		return true;
	}

	public bool IsVSMode()
	{
		if (ModePlay == Mode.VS_FFA || ModePlay == Mode.VS_TDM || ModePlay == Mode.VS_CTF)
		{
			return true;
		}
		return false;
	}

	public bool IsCoopMode()
	{
		if (ModePlay == Mode.CamPain || ModePlay == Mode.Survival || ModePlay == Mode.Boss)
		{
			return true;
		}
		return false;
	}

	public bool IsTeamMode()
	{
		if (ModePlay == Mode.VS_TDM || ModePlay == Mode.VS_CTF)
		{
			return true;
		}
		return false;
	}

	public bool IsFreeForAllMode()
	{
		return ModePlay == Mode.VS_FFA;
	}

	public bool IsTeamDeathMatchMode()
	{
		return ModePlay == Mode.VS_TDM;
	}

	public bool IsCaptureTheFlagMode()
	{
		return ModePlay == Mode.VS_CTF;
	}

	public bool IsPlayingArenaSubMode()
	{
		if (SubModePlay == SubMode.Arena_Survival || SubModePlay == SubMode.Arena_Campain)
		{
			return true;
		}
		return false;
	}

	public bool IsPlayingArenaSubMode(SubMode mode)
	{
		if (mode == SubMode.Arena_Survival || mode == SubMode.Arena_Campain)
		{
			return true;
		}
		return false;
	}

	public bool IsPlayerBossRushSubMode()
	{
		if (SubModePlay == SubMode.Boss_Rush)
		{
			return true;
		}
		return false;
	}

	public bool IsPlayerBossRushSubMode(SubMode mode)
	{
		if (mode == SubMode.Boss_Rush)
		{
			return true;
		}
		return false;
	}
}

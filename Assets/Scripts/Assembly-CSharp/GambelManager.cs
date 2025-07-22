using System.Collections.Generic;
using System.IO;

public class GambelManager
{
	public const float TIME_OF_REFRESH = 600f;

	private static GambelManager instance;

	private List<GambleConfig> gambleConfigList;

	private GambelManager()
	{
		gambleConfigList = new List<GambleConfig>();
		gambleConfigList.Add(new MithrilFruitMachine());
		gambleConfigList.Add(new GoldFruitMachine());
	}

	public GambleConfig GetGambleConfig(GambleType type)
	{
		return gambleConfigList[(int)type];
	}

	public static GambelManager GetInstance()
	{
		if (instance == null)
		{
			instance = new GambelManager();
		}
		return instance;
	}

	public void Init()
	{
		foreach (GambleConfig gambleConfig in gambleConfigList)
		{
			gambleConfig.Init();
		}
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(gambleConfigList.Count);
		foreach (GambleConfig gambleConfig in gambleConfigList)
		{
			gambleConfig.Save(bw);
		}
	}

	public void Load(BinaryReader br)
	{
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			gambleConfigList[i].Load(br);
		}
	}

	public void UpdateTimer()
	{
		foreach (GambleConfig gambleConfig in gambleConfigList)
		{
			gambleConfig.Update();
		}
	}

	public void PauseTimer()
	{
		foreach (GambleConfig gambleConfig in gambleConfigList)
		{
			gambleConfig.Pause();
		}
	}

	public void ResumeTimer()
	{
		foreach (GambleConfig gambleConfig in gambleConfigList)
		{
			gambleConfig.Resume();
		}
	}

	public void ResetOnLine()
	{
		foreach (GambleConfig gambleConfig in gambleConfigList)
		{
			gambleConfig.Reset(GambleConfig.ResetType.Online);
		}
	}
}

using System.Collections.Generic;

public class AchievementManager
{
	private static AchievementManager instance;

	private List<Achievement> list = new List<Achievement>();

	public List<Achievement> List
	{
		get
		{
			return list;
		}
	}

	private AchievementManager()
	{
	}

	public static AchievementManager GetInstance()
	{
		if (instance == null)
		{
			instance = new AchievementManager();
		}
		return instance;
	}

	public void LoadConfig()
	{
		list.Clear();
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[49];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string data = unitDataTable.GetData(i, 1, string.Empty, false);
			data = LocalizationManager.GetInstance().GetString(data);
			string data2 = unitDataTable.GetData(i, 3, string.Empty, false);
			data2 = LocalizationManager.GetInstance().GetString(data2);
			byte icon = (byte)unitDataTable.GetData(i, 4, 0, false);
			string data3 = unitDataTable.GetData(i, 5, string.Empty, false);
			byte triggerType = (byte)unitDataTable.GetData(i, 6, 0, false);
			string data4 = unitDataTable.GetData(i, 7, string.Empty, false);
			string data5 = unitDataTable.GetData(i, 8, string.Empty, false);
			bool active = unitDataTable.GetData(i, 9, 0, false) == 1;
			Achievement item = CreateAchievement(triggerType, i, data, data2, icon, data3, data4, data5, active);
			list.Add(item);
		}
	}

	private Achievement CreateAchievement(byte triggerType, int id, string nameEN, string infoEN, byte icon, string reward, string targetNum, string additionalPart, bool active)
	{
		switch (triggerType)
		{
		case 0:
			return new NoLimitedAchievement(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active);
		case 1:
			return new ValueAchievement(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active);
		case 2:
			return new AccumulateAchievement(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active);
		case 3:
			return new LimitedAchievement(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active);
		default:
			return null;
		}
	}

	private void Add(Achievement achievement)
	{
		list.Add(achievement);
	}

	private void Clear()
	{
		list.Clear();
	}

	public void Trigger(AchievementTrigger trigger)
	{
		list[(int)trigger.ID].Trigger(trigger);
	}

	public void Init()
	{
		foreach (Achievement item in list)
		{
			AchievementTrigger trigger = AchievementTrigger.Create(item.ID, AchievementTrigger.Type.Reset);
			item.Trigger(trigger);
		}
	}

	public void Report()
	{
		foreach (Achievement item in list)
		{
			AchievementTrigger trigger = AchievementTrigger.Create(item.ID, AchievementTrigger.Type.Report);
			item.Trigger(trigger);
		}
	}
}

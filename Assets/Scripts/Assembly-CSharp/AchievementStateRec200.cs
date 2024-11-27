using System.Collections.Generic;
using System.IO;

public class AchievementStateRec200 : ISubRecordset
{
	public void SaveData(BinaryWriter bw)
	{
		List<Achievement> list = AchievementManager.GetInstance().List;
		bw.Write(list.Count);
		foreach (Achievement item in list)
		{
			item.Save(bw);
		}
	}

	public void LoadData(BinaryReader br)
	{
		List<Achievement> list = AchievementManager.GetInstance().List;
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			list[i].Load(br);
		}
	}
}

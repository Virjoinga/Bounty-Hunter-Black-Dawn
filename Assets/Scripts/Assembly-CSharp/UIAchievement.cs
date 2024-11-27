using System.Collections.Generic;

public class UIAchievement : UIGameMenuNormal
{
	public UIAchievementList list;

	private void Start()
	{
		List<Achievement> list = AchievementManager.GetInstance().List;
		foreach (Achievement item in list)
		{
			if (item.Active)
			{
				this.list.Add(item.Name, item.Info, !item.Finish);
			}
		}
		this.list.Reposition();
	}
}

using UnityEngine;

public class GambleDetails
{
	public int GetRandomItemIndex(int index)
	{
		if (!GameApp.GetInstance().GetUserState().GetRoleState()
			.gambelUsed && GameApp.GetInstance().GetUserState().GetCharLevel() <= 5)
		{
			Debug.Log("FIREST GAMBEL!!!");
			GameApp.GetInstance().GetUserState().GetRoleState()
				.gambelUsed = true;
			return 3;
		}
		Interval interval = null;
		switch (index)
		{
		case 0:
			interval = new Interval(0, 0, 2, 1000, 9499, 9499);
			return interval.GetIndex();
		case 1:
			interval = new Interval(0, 2, 20, 10000, 4989, 4989);
			return interval.GetIndex();
		case 2:
			interval = new Interval(0, 6, 60, 14000, 2967, 2967);
			return interval.GetIndex();
		case 3:
			interval = new Interval(0, 20, 100, 14000, 2940, 2940);
			return interval.GetIndex();
		case 4:
			interval = new Interval(2, 100, 200, 14000, 2849, 2849);
			return interval.GetIndex();
		case 5:
			interval = new Interval(100, 200, 400, 14000, 2650, 2650);
			return interval.GetIndex();
		case 6:
			interval = new Interval(200, 400, 800, 14000, 2300, 2300);
			return interval.GetIndex();
		case 7:
			interval = new Interval(400, 800, 1600, 14000, 1600, 1600);
			return interval.GetIndex();
		case 8:
			interval = new Interval(800, 1600, 3200, 14000, 200, 200);
			return interval.GetIndex();
		default:
			interval = new Interval(1600, 3200, 6400, 8800, 0, 0);
			return interval.GetIndex();
		}
	}

	private int GetMithril(int index)
	{
		switch (index)
		{
		case 0:
		case 1:
		case 2:
			return 0;
		case 3:
			return GetRangeIndex() * 1;
		case 4:
			return GetRangeIndex() * 2;
		default:
			return GetRangeIndex() * 3;
		}
	}

	private int GetRangeIndex()
	{
		return GameApp.GetInstance().GetUserState().GetCharLevel();
	}

	public bool Spend(int index)
	{
		bool result = true;
		int charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		int cash = GameApp.GetInstance().GetUserState().GetCash();
		switch (index)
		{
		case 1:
			if (cash < charLevel * 200)
			{
				result = false;
			}
			else
			{
				GameApp.GetInstance().GetUserState().Buy(charLevel * 200);
			}
			break;
		case 2:
			if (cash < charLevel * 400)
			{
				result = false;
			}
			else
			{
				GameApp.GetInstance().GetUserState().Buy(charLevel * 400);
			}
			break;
		default:
			if (!GameApp.GetInstance().GetGlobalState().BuyWithMithril(GetMithril(index)))
			{
				result = false;
			}
			break;
		case 0:
			break;
		}
		return result;
	}

	public string GetDescription(int index)
	{
		string empty = string.Empty;
		switch (index)
		{
		case 0:
			return "Free";
		case 1:
			return "$" + GameApp.GetInstance().GetUserState().GetCharLevel() * 200;
		case 2:
			return "$" + GameApp.GetInstance().GetUserState().GetCharLevel() * 400;
		default:
			return GetMithril(index) + " Mithril";
		}
	}

	public string GetWarning(int index)
	{
		string empty = string.Empty;
		switch (index)
		{
		case 0:
			return "Are you kidding me?";
		case 1:
		case 2:
			return LocalizationManager.GetInstance().GetString("MSG_GOLD_NOT_ENOUGH");
		default:
			return LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH");
		}
	}
}

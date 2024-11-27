using System;

public class ExchangeItem
{
	public string MithrilCost { get; set; }

	public int CashRatio { get; set; }

	public int Mithril
	{
		get
		{
			return Convert.ToInt32(MithrilCost);
		}
	}

	public int Cash
	{
		get
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			if (userState == null)
			{
				return 0;
			}
			return userState.GetCharLevel() * CashRatio * Mithril;
		}
	}
}

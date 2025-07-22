using System.Collections.Generic;

public class DecorationManager
{
	private static DecorationManager instance;

	private List<Decoration> headList = new List<Decoration>();

	private List<Decoration> faceList = new List<Decoration>();

	private List<Decoration> waistList = new List<Decoration>();

	private DecorationManager()
	{
	}

	public static DecorationManager GetInstance()
	{
		if (instance == null)
		{
			instance = new DecorationManager();
		}
		return instance;
	}

	public void LoadConfig()
	{
		LoadConfig(46, 0, headList);
		LoadConfig(47, 1, faceList);
		LoadConfig(48, 2, waistList);
	}

	private void LoadConfig(byte datatable, int part, List<Decoration> list)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[datatable];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string @string = LocalizationManager.GetInstance().GetString(unitDataTable.GetData(i, 0, string.Empty, false));
			int data = unitDataTable.GetData(i, 1, 0, false);
			list.Add(new Decoration(part, @string, data));
		}
	}

	public PersonalDecorationManager GetPersonalDecorationManager(UserState userState)
	{
		return new PersonalDecorationManager(userState, headList, faceList, waistList);
	}
}

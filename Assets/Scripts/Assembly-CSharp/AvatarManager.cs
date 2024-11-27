using System.Collections.Generic;

public class AvatarManager
{
	private static AvatarManager instance;

	private List<Avatar> avatarList = new List<Avatar>();

	private AvatarManager()
	{
	}

	public static AvatarManager GetInstance()
	{
		if (instance == null)
		{
			instance = new AvatarManager();
		}
		return instance;
	}

	public void LoadConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[50];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string @string = LocalizationManager.GetInstance().GetString(unitDataTable.GetData(i, 0, string.Empty, false));
			int data = unitDataTable.GetData(i, 1, 0, false);
			CharacterClass data2 = (CharacterClass)unitDataTable.GetData(i, 2, 0, false);
			Sex data3 = (Sex)unitDataTable.GetData(i, 3, 0, false);
			avatarList.Add(new Avatar(@string, data, data2, data3));
		}
	}

	public PersonalAvatarManager GetPersonalAvatarManager(UserState userState)
	{
		return new PersonalAvatarManager(userState, avatarList);
	}
}

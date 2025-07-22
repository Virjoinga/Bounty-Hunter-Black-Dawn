using System.Collections.Generic;
using UnityEngine;

public class UICharacterDataBase
{
	public enum CreateResult
	{
		Success = 0,
		IllegalChar = 1,
		SameName = 2,
		Empty = 3
	}

	public enum Index
	{
		Character1 = 0,
		Character2 = 1,
		Character3 = 2,
		Character4 = 3,
		Character5 = 4,
		Character6 = 5,
		Character7 = 6,
		Character8 = 7,
		Character9 = 8,
		Character10 = 9
	}

	public enum CharSex
	{
		Male = 0,
		Female = 1
	}

	public enum CharClass
	{
		None = -1,
		Engineer = 0,
		Prayer = 1,
		Sniper = 2,
		Soldier = 3
	}

	private static UICharacterDataBase instance;

	private int characterCount = 10;

	private int characterCountCreated;

	private UICharacterData[] CharacterDataList;

	private Index currIndex;

	private UICharacterDataBase()
	{
		Update();
	}

	public static UICharacterDataBase getInstance()
	{
		if (instance == null)
		{
			instance = new UICharacterDataBase();
		}
		return instance;
	}

	public string GetClassName(CharClass charclass)
	{
		switch (charclass)
		{
		case CharClass.Soldier:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_NAME_SOLDIER");
		case CharClass.Sniper:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_NAME_SNIPER");
		case CharClass.Prayer:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_NAME_PRAYER");
		case CharClass.Engineer:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_NAME_ENGINEER");
		default:
			return string.Empty;
		}
	}

	public string GetClassInfo(CharClass charclass)
	{
		switch (charclass)
		{
		case CharClass.Soldier:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_INFO_SOLDIER");
		case CharClass.Sniper:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_INFO_SNIPER");
		case CharClass.Prayer:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_INFO_PRAYER");
		case CharClass.Engineer:
			return LocalizationManager.GetInstance().GetString("LOC_MAIN_MENU_INFO_ENGINEER");
		default:
			return string.Empty;
		}
	}

	private void Update()
	{
		characterCountCreated = 0;
		CharacterDataList = null;
		CharacterDataList = new UICharacterData[characterCount];
		List<UserState.RoleState> roles = GameApp.GetInstance().GetRoles();
		for (int i = 0; i < characterCount; i++)
		{
			if (roles != null && i < roles.Count)
			{
				CharacterDataList[i] = UICharacterData.CreateData(roles[i]);
				characterCountCreated++;
			}
			else
			{
				CharacterDataList[i] = UICharacterData.CreateEmpty();
			}
		}
	}

	public void UpdateData()
	{
		Update();
	}

	public CreateResult CheckData(string nickName, CharSex sex, CharClass classId)
	{
		if (nickName == string.Empty)
		{
			return CreateResult.Empty;
		}
		UICharacterData[] characterDataList = CharacterDataList;
		foreach (UICharacterData uICharacterData in characterDataList)
		{
			if (uICharacterData.IsHasProfile && uICharacterData.CharName.Equals(nickName))
			{
				return CreateResult.SameName;
			}
		}
		foreach (char c in nickName)
		{
			if (!char.IsLetterOrDigit(c))
			{
				return CreateResult.IllegalChar;
			}
		}
		return CreateResult.Success;
	}

	public CreateResult CreateData(string nickName, CharSex sex, CharClass classId)
	{
		if (nickName == string.Empty)
		{
			return CreateResult.Empty;
		}
		UICharacterData[] characterDataList = CharacterDataList;
		foreach (UICharacterData uICharacterData in characterDataList)
		{
			if (uICharacterData.IsHasProfile && uICharacterData.CharName.Equals(nickName))
			{
				return CreateResult.SameName;
			}
		}
		foreach (char c in nickName)
		{
			if (!char.IsLetterOrDigit(c))
			{
				return CreateResult.IllegalChar;
			}
		}
		AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.The_Family_Photo);
		achievementTrigger.PutData(1, (int)(classId + 1));
		AchievementManager.GetInstance().Trigger(achievementTrigger);
		GameApp.GetInstance().GetGlobalState().SetLastCharacterIndex(characterCountCreated);
		GameApp.GetInstance().GetGlobalState().SetCurrRole(nickName);
		GameApp.GetInstance().GetGlobalState().AddRole(nickName);
		Sex sex2 = ((sex != 0) ? Sex.F : Sex.M);
		CharacterClass charClass = (CharacterClass)(classId + 1);
		GameApp.GetInstance().GetUserState().CreateNewRole(nickName, charClass, sex2);
		GameApp.GetInstance().Save();
		return CreateResult.Success;
	}

	public UICharacterData ChooseData(Index index)
	{
		if (CharacterDataList[(int)index].IsHasProfile)
		{
			GameApp.GetInstance().GetGlobalState().SetLastCharacterIndex((int)index);
		}
		GameApp.GetInstance().GetGlobalState().SetCurrRole(CharacterDataList[(int)index].CharName);
		currIndex = index;
		return CharacterDataList[(int)index];
	}

	public UICharacterData GetLastChooseData()
	{
		return GetData(currIndex);
	}

	public UICharacterData GetData(Index index)
	{
		return CharacterDataList[(int)index];
	}

	public void Delete(Index index)
	{
		GameApp.GetInstance().DeleteUserData(CharacterDataList[(int)index].CharName);
		GameApp.GetInstance().GetGlobalState().SetCurrRole(string.Empty);
		GameApp.GetInstance().GetGlobalState().RemoveRole(CharacterDataList[(int)index].CharName);
		GameApp.GetInstance().SaveGlobalDataLocal();
		CharacterDataList[(int)index] = UICharacterData.CreateEmpty();
	}

	public GameObject GetAvatar(CharSex sex, CharClass classId)
	{
		Sex sexType = ((sex != 0) ? Sex.F : Sex.M);
		CharacterClass charClass = (CharacterClass)(classId + 1);
		return AvatarBuilder.GetInstance().CreateUIAvatar(charClass, sexType);
	}

	public bool HasNoData()
	{
		bool result = true;
		UICharacterData[] characterDataList = CharacterDataList;
		foreach (UICharacterData uICharacterData in characterDataList)
		{
			if (uICharacterData.IsHasProfile)
			{
				result = false;
			}
		}
		return result;
	}
}

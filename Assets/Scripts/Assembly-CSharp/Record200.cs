using System;
using System.Collections.Generic;
using System.IO;

public class Record200 : IRecordset
{
	public static string version = "200";

	public void SaveGlobalData(BinaryWriter bw)
	{
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		AdsManager.GetInstance().Save(bw);
		TutorialManager.GetInstance().Save(bw);
		bw.Write(globalState.GetPlaySound());
		bw.Write(globalState.GetPlayMusic());
		bw.Write((int)(globalState.GetSoundVolume() * 100f));
		bw.Write((int)(globalState.GetMusicVolume() * 100f));
		bw.Write(globalState.GetVertivalCameraNormal());
		bw.Write(globalState.GetHorizontalCamreraNormal());
		bw.Write(globalState.GetAimAssist());
		bw.Write(globalState.GetBloodSpraying());
		bw.Write(globalState.GetMithril());
		bw.Write(globalState.GetSaveNum());
		bw.Write(globalState.GetGiftTimeSpan());
		bw.Write((int)(globalState.TouchInputSensitivity * 100f));
		bw.Write(globalState.GetTwitter());
		bw.Write(globalState.GetFacebook());
		bw.Write(globalState.GetDeadTimer());
		bw.Write(globalState.GetCurrRole());
		bw.Write(globalState.GetLastCharacterIndex());
		string value = GlobalState.GetLastLocalNotificationTime().ToShortDateString();
		bw.Write(value);
		List<string> rolesName = globalState.GetRolesName();
		bw.Write((byte)rolesName.Count);
		for (int i = 0; i < rolesName.Count; i++)
		{
			bw.Write(rolesName[i]);
		}
		bw.Write(globalState.HaveRate());
		AchievementStateRec200 achievementStateRec = new AchievementStateRec200();
		achievementStateRec.SaveData(bw);
	}

	public void LoadGlobalData(BinaryReader br)
	{
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		AdsManager.GetInstance().Load(br);
		TutorialManager.GetInstance().Load(br);
		bool playSound = br.ReadBoolean();
		bool playMusic = br.ReadBoolean();
		float soundVolume = (float)br.ReadInt32() / 100f;
		float musicVolume = (float)br.ReadInt32() / 100f;
		bool verticalCameraNormal = br.ReadBoolean();
		bool horizontalCamreraNormal = br.ReadBoolean();
		bool aimAssist = br.ReadBoolean();
		bool bloodSpraying = br.ReadBoolean();
		int mithril = br.ReadInt32();
		int saveNum = br.ReadInt32();
		byte giftTimeSpan = br.ReadByte();
		float touchInputSensitivity = (float)br.ReadInt32() / 100f;
		bool twitter = br.ReadBoolean();
		bool facebook = br.ReadBoolean();
		byte deadTimer = br.ReadByte();
		string currRole = br.ReadString();
		int lastCharacterIndex = br.ReadInt32();
		string s = br.ReadString();
		DateTime lastLocalNotificationTime = DateTime.Parse(s);
		byte b = br.ReadByte();
		List<RoleStateInfo> list = new List<RoleStateInfo>();
		for (int i = 0; i < b; i++)
		{
			string roleName = br.ReadString();
			RoleStateInfo roleStateInfo = new RoleStateInfo();
			roleStateInfo.RoleName = roleName;
			list.Add(roleStateInfo);
		}
		bool rate = br.ReadBoolean();
		AchievementStateRec200 achievementStateRec = new AchievementStateRec200();
		achievementStateRec.LoadData(br);
		globalState.SetPlaySound(playSound);
		globalState.SetPlayMusic(playMusic);
		globalState.SetSoundVolume(soundVolume);
		globalState.SetMusicVolume(musicVolume);
		globalState.SetVerticalCameraNormal(verticalCameraNormal);
		globalState.SetHorizontalCamreraNormal(horizontalCamreraNormal);
		globalState.SetAimAssist(aimAssist);
		globalState.SetBloodSpraying(bloodSpraying);
		globalState.SetMithril(mithril);
		globalState.SetSaveNum(saveNum);
		globalState.SetGiftTimeSpan(giftTimeSpan);
		globalState.TouchInputSensitivity = touchInputSensitivity;
		globalState.SetTwitter(twitter);
		globalState.SetFacebook(facebook);
		globalState.SetDeadTimer(deadTimer);
		globalState.SetCurrRole(currRole);
		globalState.SetRoles(list);
		globalState.SetLastCharacterIndex(lastCharacterIndex);
		globalState.SetRate(rate);
		GlobalState.SetLastLocalNotificationTime(lastLocalNotificationTime);
		AdsManager.GetInstance().CheckGameRewards();
	}

	public UserState.RoleState LoadRoleData(BinaryReader br)
	{
		UserState.RoleState roleState = new UserState.RoleState();
		string roleName = br.ReadString();
		byte charClass = br.ReadByte();
		byte sexType = br.ReadByte();
		int cash = br.ReadInt32();
		int exp = br.ReadInt32();
		short charLevel = br.ReadInt16();
		int num = br.ReadInt32();
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = br.ReadByte();
		}
		byte avatarID = br.ReadByte();
		int weaponType = br.ReadInt32();
		int weaponId = br.ReadInt32();
		short currentQuest = br.ReadInt16();
		byte lastPortalIndex = br.ReadByte();
		roleState.roleName = roleName;
		roleState.CharClass = (CharacterClass)charClass;
		roleState.SexType = (Sex)sexType;
		roleState.SetCash(cash);
		roleState.SetExp(exp);
		roleState.CharLevel = charLevel;
		roleState.decoration = array;
		roleState.avatarID = avatarID;
		roleState.weaponType = (WeaponType)weaponType;
		roleState.weaponId = weaponId;
		roleState.currentQuest = currentQuest;
		roleState.lastPortalIndex = lastPortalIndex;
		return roleState;
	}

	public void SaveData(BinaryWriter bw)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		bw.Write(userState.GetRoleName());
		bw.Write((byte)userState.GetCharacterClass());
		bw.Write((byte)userState.GetSex());
		bw.Write(userState.GetCash());
		bw.Write(userState.GetExp());
		bw.Write(userState.GetCharLevel());
		byte[] decoration = userState.GetDecoration();
		bw.Write(decoration.Length);
		for (int i = 0; i < decoration.Length; i++)
		{
			bw.Write(decoration[i]);
		}
		bw.Write(userState.GetAvatar());
		bw.Write((int)userState.GetWeaponType());
		bw.Write(userState.GetWeaponId());
		bw.Write(userState.GetCurrentQuest());
		bw.Write(userState.GetLastPortalIndex());
		bw.Write(userState.Enegy);
		bw.Write(userState.GetCurrentCityID());
		bw.Write(userState.GetStageState().Length);
		for (int j = 0; j < userState.GetStageState().Length; j++)
		{
			bw.Write(userState.GetStageState(j));
		}
		bw.Write(userState.GetStageInstanceState().Length);
		for (int k = 0; k < userState.GetStageInstanceState().Length; k++)
		{
			bw.Write(userState.GetStageInstanceState(k));
		}
		NPCStateRec100 nPCStateRec = new NPCStateRec100(userState.m_npcState);
		nPCStateRec.SaveData(bw);
		QuestStateRec100 questStateRec = new QuestStateRec100(userState.m_questStateContainer);
		questStateRec.SaveData(bw);
		ItemRec100 itemRec = new ItemRec100(userState.ItemInfoData);
		itemRec.SaveData(bw);
		userState.SkillTreeManager.Save(bw);
		userState.SaveBullets(bw);
		bw.Write(userState.GetRoleState().gambelUsed);
		userState.SaveDecorations(bw);
		userState.SaveAvatars(bw);
		GambelManager.GetInstance().Save(bw);
	}

	public void LoadData(BinaryReader br)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		int enegy = br.ReadInt32();
		byte currentCityID = br.ReadByte();
		byte[] array = new byte[Global.TOTAL_STAGE];
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			array[i] = br.ReadByte();
		}
		int num2 = br.ReadInt32();
		byte[] array2 = new byte[Global.TOTAL_STAGE_INSTANCE];
		for (int j = 0; j < num2; j++)
		{
			array2[j] = br.ReadByte();
		}
		NPCStateRec100 nPCStateRec = new NPCStateRec100(userState.m_npcState);
		nPCStateRec.LoadData(br);
		QuestStateRec100 questStateRec = new QuestStateRec100(userState.m_questStateContainer);
		questStateRec.LoadData(br);
		ItemRec100 itemRec = new ItemRec100(userState.ItemInfoData);
		itemRec.LoadData(br);
		userState.SkillTreeManager.Load(br);
		userState.LoadBullets(br);
		userState.GetRoleState().gambelUsed = br.ReadBoolean();
		userState.LoadDecorations(br);
		userState.LoadAvatars(br);
		userState.Enegy = enegy;
		userState.SetCurrentCityID(currentCityID);
		userState.SetStageState(array);
		userState.SetStageInstance(array2);
		GambelManager.GetInstance().Load(br);
	}
}
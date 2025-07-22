using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class GlobalState
{
	public const string key1 = "Please quit the app immediately";

	public const string md5Key1 = "b1e re sis 02a";

	public const string key2 = "Please quit the admob immediately";

	public const string md5Key2 = "b1e re sis 02b";

	public static string version = "125";

	public static string[] allVersions = new string[10] { "100", "101", "102", "110", "120", "121", "122", "123", "124", "125" };

	public static List<RoleStateInfo> roleStateICloud = new List<RoleStateInfo>();

	public static int user_id = -1;

	public bool bInit;

	protected string mithril;

	protected bool bPlayMusic = true;

	protected bool bPlaySound = true;

	protected float musicVolume = 1f;

	protected float soundVolume = 1f;

	protected int saveNum;

	protected bool bVerticalCamreraNormal = true;

	protected bool bHorizontalCamreraNormal = true;

	protected bool bAimAssist = true;

	protected bool bBloodSpraying = true;

	protected int lastCharacterIndex;

	protected byte giftTimeSpan;

	protected bool bTwitter;

	protected bool bFacebook;

	protected byte deadTimer = 2;

	protected List<RoleStateInfo> roleList = new List<RoleStateInfo>();

	protected string currRole = string.Empty;

	protected static DateTime lastLocalNotificationTime = DateTime.Now;

	protected bool HaveRated;

	protected bool IsBossRushOn;

	private IAPItemState iapItemState = new IAPItemState();

	protected bool isDoubleExp;

	protected bool isInfiniteSMGBullet;

	protected bool isInfiniteRPGBullet;

	protected bool isNewbieGift1;

	protected bool isNewbieGift2;

	public float TouchInputSensitivity { get; set; }

	public GlobalState()
	{
		bInit = false;
	}

	public void Init()
	{
		SetMithril(10);
		giftTimeSpan = 0;
		bPlayMusic = true;
		bPlaySound = true;
		musicVolume = 1f;
		soundVolume = 1f;
		bVerticalCamreraNormal = true;
		bHorizontalCamreraNormal = true;
		bAimAssist = true;
		bBloodSpraying = true;
		TouchInputSensitivity = 0.5f;
		bTwitter = false;
		bFacebook = false;
		roleList = new List<RoleStateInfo>();
		currRole = string.Empty;
		lastLocalNotificationTime = DateTime.Now.AddHours(-72.0);
		lastCharacterIndex = 0;
	}

	public void LoadConfig()
	{
		GameConfig.GetInstance().LoadDropConfig();
		GameConfig.GetInstance().LoadChestDropConfig();
		GameConfig.GetInstance().LoadEquipConfig();
		GameConfig.GetInstance().LoadEquipPrefixConfig();
		GameConfig.GetInstance().LoadChipPrefixConfig();
		GameConfig.GetInstance().LoadSkillConfig();
		GameConfig.GetInstance().LoadBuffConfig();
		GameConfig.GetInstance().LoadNpcConfig();
		GameConfig.GetInstance().LoadAreaConfig();
		GameConfig.GetInstance().LoadSceneConfig();
		GameConfig.GetInstance().LoadInstancePortalConfig();
		GameConfig.GetInstance().LoadEnemyConfig();
		GameConfig.GetInstance().LoadQuestEnemySpawnConfig();
		GameConfig.GetInstance().LoadSpecialItemConfig();
		QuestManager.GetInstance().LoadConfig();
		DecorationManager.GetInstance().LoadConfig();
		AvatarManager.GetInstance().LoadConfig();
		AchievementManager.GetInstance().LoadConfig();
		Exp.LoadConfig();
		GameConfig.GetInstance().LoadTipsConfig();
		Res2DManager.GetInstance().FreeDataTable(37);
		Res2DManager.GetInstance().FreeDataTable(29, 36);
		Res2DManager.GetInstance().FreeDataTable(49);
		Res2DManager.GetInstance().FreeDataTable(44);
		Res2DManager.GetInstance().FreeDataTable(45);
		Res2DManager.GetInstance().FreeDataTable(52);
		Res2DManager.GetInstance().FreeDataTable(46);
		Res2DManager.GetInstance().FreeDataTable(47);
		Res2DManager.GetInstance().FreeDataTable(48);
		Res2DManager.GetInstance().FreeDataTable(50);
		Res2DManager.GetInstance().FreeDataTable(0);
		Res2DManager.GetInstance().FreeDataTable(1);
		Res2DManager.GetInstance().FreeDataTable(2);
		Res2DManager.GetInstance().FreeDataTable(43);
		Res2DManager.GetInstance().FreeDataTable(73);
		Res2DManager.GetInstance().FreeDataTable(40);
		Res2DManager.GetInstance().FreeDataTable(41);
		Res2DManager.GetInstance().FreeDataTable(8);
		Res2DManager.GetInstance().FreeDataTable(38);
		Res2DManager.GetInstance().FreeDataTable(51);
		Res2DManager.GetInstance().FreeDataTable(74);
	}

	public void DeliverIAPItem(IAPName iapName)
	{
		Debug.Log("DeliverIAPItem " + iapName);
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		switch (iapName)
		{
		case IAPName.C199DoubleExp:
			if (!globalState.GetDoubleExp())
			{
				ItemExpScale itemExpScale = (ItemExpScale)GetIAPitemState().GetGlobalIAPItem(IAPItemState.ItemType.ExpScale);
				itemExpScale.Resume();
				globalState.SetDoubleExp(true);
			}
			break;
		case IAPName.C499InfiniteSMGBullet:
			if (!globalState.GetInfiniteSMGBullet())
			{
				ItemInfiniteBullet itemInfiniteBullet = (ItemInfiniteBullet)GetIAPitemState().GetGlobalIAPItem(IAPItemState.ItemType.InfiniteBullet);
				itemInfiniteBullet.Resume(WeaponType.SubMachineGun);
				globalState.SetInfiniteSMGBullet(true);
			}
			break;
		case IAPName.C499InfiniteRPGBullet:
			if (!globalState.GetInfiniteRPGBullet())
			{
				ItemInfiniteBullet itemInfiniteBullet2 = (ItemInfiniteBullet)GetIAPitemState().GetGlobalIAPItem(IAPItemState.ItemType.InfiniteBullet);
				itemInfiniteBullet2.Resume(WeaponType.RPG);
				globalState.SetInfiniteRPGBullet(true);
			}
			break;
		case IAPName.C99NewbieGift1:
			if (!globalState.GetNewbieGift1())
			{
				ItemNewbie1 itemNewbie2 = (ItemNewbie1)GetIAPitemState().GetGlobalIAPItem(IAPItemState.ItemType.Newbie1);
				itemNewbie2.Resume();
				globalState.SetNewbieGift1(true);
			}
			break;
		case IAPName.C199NewbieGift2:
			if (!globalState.GetNewbieGift2())
			{
				ItemNewbie2 itemNewbie = (ItemNewbie2)GetIAPitemState().GetGlobalIAPItem(IAPItemState.ItemType.Newbie2);
				itemNewbie.Resume();
				globalState.SetNewbieGift2(true);
			}
			break;
		default:
			AddMithril(IAPShop.GetInstance().GetIAPList()[iapName].Mithril);
			break;
		case IAPName.None:
			break;
		}
		GameApp.GetInstance().Save();
	}

	public int GetMithril()
	{
		return AntiCracking.DecryptBufferStr(mithril, "no_mod");
	}

	public void SetMithril(int _mithril)
	{
		mithril = AntiCracking.CryptBufferStr(Mathf.Min(Global.MAX_MITHRIL, _mithril), "no_mod");
	}

	public void AddMithril(int mithrilGot)
	{
		if (mithrilGot > 0)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			if (userState != null && userState.OperInfo != null)
			{
				userState.OperInfo.AddInfo(OperatingInfoType.GAIN_MITHRIL_COUNT, mithrilGot);
			}
		}
		SetMithril(Mathf.Clamp(GetMithril() + mithrilGot, 0, Global.MAX_MITHRIL));
	}

	public bool BuyWithMithril(int price)
	{
		if (GetMithril() >= price)
		{
			AddMithril(-price);
			return true;
		}
		return false;
	}

	public bool VerifySaveRole(string roleName)
	{
		if (!string.IsNullOrEmpty(roleName) && !roleName.Equals(string.Empty))
		{
			return true;
		}
		return false;
	}

	public string GetCurrRole()
	{
		return currRole;
	}

	public void SetCurrRole(string role)
	{
		currRole = role;
	}

	public List<RoleStateInfo> GetRoles()
	{
		return roleList;
	}

	public List<string> GetRolesName()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < roleList.Count; i++)
		{
			list.Add(roleList[i].RoleName);
		}
		return list;
	}

	public void RemoveRole(string roleName)
	{
		for (int i = 0; i < roleList.Count; i++)
		{
			if (roleList[i].RoleName == roleName)
			{
				roleList.RemoveAt(i);
				break;
			}
		}
	}

	public void SetStaticRoles()
	{
		foreach (RoleStateInfo role in roleList)
		{
			RoleStateInfo roleStateInfo = GetRoleStateICloud(role.RoleName);
			if (roleStateInfo != null)
			{
				role.MD5_Verify_Bytes = new byte[roleStateInfo.MD5_Verify_Bytes.Length];
				roleStateInfo.MD5_Verify_Bytes.CopyTo(role.MD5_Verify_Bytes, 0);
			}
		}
	}

	private RoleStateInfo GetRoleStateICloud(string roleName)
	{
		foreach (RoleStateInfo item in roleStateICloud)
		{
			if (roleName.Equals(item.RoleName))
			{
				return item;
			}
		}
		return null;
	}

	public void SetRoles(List<RoleStateInfo> roles)
	{
		roleList = roles;
	}

	public void AddRole(string roleName)
	{
		RoleStateInfo roleStateInfo = new RoleStateInfo();
		roleStateInfo.RoleName = roleName;
		roleList.Add(roleStateInfo);
	}

	public bool GetPlayMusic()
	{
		return bPlayMusic;
	}

	public void SetPlayMusic(bool isPlay)
	{
		bPlayMusic = isPlay;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetMusicMute(!bPlayMusic);
		}
	}

	public bool GetPlaySound()
	{
		return bPlaySound;
	}

	public void SetPlaySound(bool isPlay)
	{
		bPlaySound = isPlay;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetSoundMute(!bPlaySound);
		}
	}

	public float GetMusicVolume()
	{
		return musicVolume;
	}

	public void SetMusicVolume(float volume)
	{
		musicVolume = volume;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetMusicVolume(volume);
		}
	}

	public float GetSoundVolume()
	{
		return soundVolume;
	}

	public void SetSoundVolume(float volume)
	{
		soundVolume = volume;
		NGUITools.soundVolume = volume;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetSoundVolume(volume);
		}
	}

	public static void SetLastLocalNotificationTime(DateTime date)
	{
		lastLocalNotificationTime = date;
	}

	public static DateTime GetLastLocalNotificationTime()
	{
		return lastLocalNotificationTime;
	}

	public void SetVerticalCameraNormal(bool state)
	{
		bVerticalCamreraNormal = state;
	}

	public bool GetVertivalCameraNormal()
	{
		return bVerticalCamreraNormal;
	}

	public void SetHorizontalCamreraNormal(bool state)
	{
		bHorizontalCamreraNormal = state;
	}

	public bool GetHorizontalCamreraNormal()
	{
		return bHorizontalCamreraNormal;
	}

	public void SetAimAssist(bool state)
	{
		bAimAssist = state;
	}

	public bool GetAimAssist()
	{
		return bAimAssist;
	}

	public void SetBloodSpraying(bool state)
	{
		bBloodSpraying = state;
	}

	public bool GetBloodSpraying()
	{
		return bBloodSpraying;
	}

	public void SetTwitter(bool twitter)
	{
		bTwitter = twitter;
	}

	public bool GetTwitter()
	{
		return bTwitter;
	}

	public void SetFacebook(bool facebook)
	{
		bFacebook = facebook;
	}

	public bool GetFacebook()
	{
		return bFacebook;
	}

	public byte GetDeadTimer()
	{
		return deadTimer;
	}

	public void SetDeadTimer(byte timer)
	{
		deadTimer = timer;
	}

	public byte AtomicDeadTimer()
	{
		deadTimer++;
		deadTimer %= 3;
		return deadTimer;
	}

	public byte GetGiftTimeSpan()
	{
		return giftTimeSpan;
	}

	public void SetGiftTimeSpan(byte timeSpan)
	{
		giftTimeSpan = timeSpan;
	}

	public void AddGiftTimeSpan(byte num)
	{
		giftTimeSpan += num;
		giftTimeSpan %= 5;
	}

	public int GetSaveNum()
	{
		return saveNum;
	}

	public void SetSaveNum(int saveNum)
	{
		this.saveNum = saveNum;
	}

	public int GetLastCharacterIndex()
	{
		return lastCharacterIndex;
	}

	public void SetLastCharacterIndex(int index)
	{
		lastCharacterIndex = index;
	}

	public void WriteMithril(BytesBuffer buffer)
	{
		buffer.AddInt(GetMithril());
	}

	public void WriteSaveNum(BytesBuffer buffer)
	{
		buffer.AddInt(saveNum);
	}

	public void WriteTwitter(BytesBuffer buffer)
	{
		buffer.AddBool(bTwitter);
	}

	public void WriteFacebook(BytesBuffer buffer)
	{
		buffer.AddBool(bFacebook);
	}

	public IRecordset GetRecordset(string ver)
	{
		IRecordset result = null;
		if (ver.Equals(Record100.version))
		{
			result = new Record100();
		}
		else if (ver.Equals(Record101.version))
		{
			result = new Record101();
		}
		else if (ver.Equals(Record102.version))
		{
			result = new Record102();
		}
		else if (ver.Equals(Record110.version))
		{
			result = new Record110();
		}
		else if (ver.Equals(Record120.version))
		{
			result = new Record120();
		}
		else if (ver.Equals(Record121.version))
		{
			result = new Record121();
		}
		else if (ver.Equals(Record122.version))
		{
			result = new Record122();
		}
		else if (ver.Equals(Record123.version))
		{
			result = new Record123();
		}
		else if (ver.Equals(Record124.version))
		{
			result = new Record124();
		}
		else if (ver.Equals(Record125.version))
		{
			result = new Record125();
		}
		return result;
	}

	public void SaveData(BinaryWriter bw)
	{
		IRecordset recordset = GetRecordset(version);
		bw.Write(version);
		SetSaveNum(saveNum + 1);
		recordset.SaveGlobalData(bw);
	}

	public string LoadData(BinaryReader br)
	{
		string text = br.ReadString();
		IRecordset recordset = GetRecordset(text);
		if (recordset != null)
		{
			recordset.LoadGlobalData(br);
		}
		return text;
	}

	public byte[] CryptMD5Buffer(byte[] data, string md5key)
	{
		string empty = string.Empty;
		empty = ((AndroidConstant.version != 0) ? (md5key + GameApp.GetInstance().UUID) : (md5key + SystemInfo.deviceUniqueIdentifier));
		byte[] bytes = Encoding.ASCII.GetBytes(empty);
		byte[] array = new byte[data.Length];
		data.CopyTo(array, 0);
		for (int i = 0; i < bytes.Length; i++)
		{
			array[i] ^= bytes[i];
		}
		MD5 mD = new MD5CryptoServiceProvider();
		return mD.ComputeHash(array);
	}

	public bool VerifyMD5(byte[] original, byte[] md5, string md5key)
	{
		try
		{
			if (original == null || original.Length == 0)
			{
				return false;
			}
			byte[] array = new byte[original.Length];
			original.CopyTo(array, 0);
			array = CryptMD5Buffer(array, md5key);
			if (array == null || md5 == null)
			{
				return false;
			}
			if (array.Length != md5.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != md5[i])
				{
					return false;
				}
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public byte[] GetBytesFromStream(Stream stream)
	{
		byte[] array = new byte[stream.Length];
		int num = (int)stream.Length;
		int num2 = 0;
		while (num > 0)
		{
			int num3 = stream.Read(array, num2, num);
			if (num3 == 0)
			{
				break;
			}
			num2 += num3;
			num -= num3;
		}
		return array;
	}

	public byte[] CryptBuffer(byte[] data, string key)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(key);
		byte[] array = new byte[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			array[i] = (byte)(data[i] ^ bytes[i % bytes.Length]);
		}
		return array;
	}

	public byte[] DecryptBuffer(byte[] buffer, string key)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(key);
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] ^= bytes[i % bytes.Length];
		}
		return buffer;
	}

	public bool HaveRate()
	{
		return HaveRated;
	}

	public void SetRate(bool _isRate)
	{
		HaveRated = _isRate;
	}

	public string GetTips(byte type)
	{
		List<string> list = GameConfig.GetInstance().tipsConfig[type];
		if (list == null || list.Count < 1)
		{
			return "Tips Null";
		}
		int index = UnityEngine.Random.Range(0, list.Count - 1);
		return list[index];
	}

	public IAPItemState GetIAPitemState()
	{
		return iapItemState;
	}

	public void SetBossRush(bool state)
	{
		IsBossRushOn = state;
	}

	public bool GetBossRush()
	{
		return IsBossRushOn;
	}

	public bool GetDoubleExp()
	{
		return isDoubleExp;
	}

	public void SetDoubleExp(bool isDouble)
	{
		isDoubleExp = isDouble;
	}

	public bool GetInfiniteSMGBullet()
	{
		return isInfiniteSMGBullet;
	}

	public void SetInfiniteSMGBullet(bool isSMGBullet)
	{
		isInfiniteSMGBullet = isSMGBullet;
	}

	public bool GetInfiniteRPGBullet()
	{
		return isInfiniteRPGBullet;
	}

	public void SetInfiniteRPGBullet(bool isRPGBullet)
	{
		isInfiniteRPGBullet = isRPGBullet;
	}

	public bool GetNewbieGift1()
	{
		return isNewbieGift1;
	}

	public void SetNewbieGift1(bool isGift1)
	{
		isNewbieGift1 = isGift1;
	}

	public bool GetNewbieGift2()
	{
		return isNewbieGift2;
	}

	public void SetNewbieGift2(bool isGift2)
	{
		isNewbieGift2 = isGift2;
	}
}

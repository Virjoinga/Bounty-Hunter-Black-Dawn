using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class GameApp
{
	public enum loadRMS
	{
		RMSPass = 0,
		RMSException = 1,
		RMSEmpty = 2,
		RMSMismatch = 3
	}

	public enum loadIcloud
	{
		None = 0,
		Pass = 1,
		VersionMismatch = 2,
		Failed = 3
	}

	public const string GLOBAL_DATA_FILE_NAME = "BangOutMap";

	public const string FILE_NAME_SUFFIX = "_01";

	public const string FILE_NAME_SUFFIX_CRYPT = "_02";

	protected static GameApp instance;

	protected GameScene gameScene;

	protected GameWorld gameWorld;

	protected StreamingManager streamingManager;

	protected NetworkManager networkMgr;

	protected LootManager lootManager;

	protected UserState userState = new UserState();

	protected GlobalState globalState = new GlobalState();

	protected GameMode gameMode = new GameMode(NetworkType.Single, Mode.CamPain);

	protected UIStateManager m_uiStateMgr;

	protected VSManager mVSManager;

	public float AverageSendPacket;

	public float AverageSendByte;

	public float AverageReceivePacket;

	public float AverageReceiveByte;

	public bool httpRequestSent;

	public bool IsToBackground = true;

	public int Base64MinLength = 50;

	public static bool RMSException = false;

	public static DateTime UploadAnalysis = DateTime.Now.AddHours(-24.0);

	public static bool ShowFreyrGamesAds = false;

	public static int Count = 0;

	public DeviceOrientation PreviousOrientation { get; set; }

	public bool LogoFirstPop { get; set; }

	public string UUID { get; set; }

	public string DataPath { get; set; }

	public byte AppStatus { get; set; }

	public static GameApp GetInstance()
	{
		if (instance == null)
		{
			instance = new GameApp();
			instance.PreviousOrientation = DeviceOrientation.Portrait;
			instance.LogoFirstPop = true;
			Application.targetFrameRate = 60;
		}
		return instance;
	}

	public void SetUIStateManager(UIStateManager stateMgr)
	{
		m_uiStateMgr = stateMgr;
	}

	public UIStateManager GetUIStateManager()
	{
		return m_uiStateMgr;
	}

	public void ShowAdMobBanner()
	{
	}

	public bool IsConnectedToInternet()
	{
		bool result = false;
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			result = true;
		}
		return result;
	}

	public void StartHttpRequestThread()
	{
		if (IsConnectedToInternet() && !httpRequestSent)
		{
			HttpRequestThread @object = new HttpRequestThread();
			Thread thread = new Thread(@object.DoWork);
			thread.Start();
			httpRequestSent = true;
		}
	}

	public void SetGameMode(GameMode mode)
	{
		gameMode = mode;
	}

	public GameMode GetGameMode()
	{
		return gameMode;
	}

	public GameScene GetGameScene()
	{
		return gameScene;
	}

	public void CreateGameScene()
	{
		gameScene = new GameScene();
		Lobby.GetInstance().GetVSClock().Reset();
		byte cityID = 0;
		byte sceneID = 0;
		if (GameConfig.GetInstance().sceneConfig.ContainsKey(Application.loadedLevelName))
		{
			cityID = GameConfig.GetInstance().sceneConfig[Application.loadedLevelName].AreaID;
			sceneID = GameConfig.GetInstance().sceneConfig[Application.loadedLevelName].SceneID;
		}
		GetInstance().GetGameWorld().ChangeGameScene(cityID, sceneID);
		gameScene.Init();
		if (GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (!GameWorld.NeedGetSeneceState)
			{
				ChangeGameSceneRequest request = new ChangeGameSceneRequest(cityID, sceneID);
				networkMgr.SendRequest(request);
			}
			else
			{
				byte currentEquipWeaponSlot = GetInstance().GetUserState().ItemInfoData.CurrentEquipWeaponSlot;
				ElementType element = gameWorld.GetLocalPlayer().GetWeapon().mCurrentElementType;
				if (gameWorld.GetLocalPlayer().GetWeapon().IsAllElement())
				{
					element = ElementType.AllElement;
				}
				GetSceneStateRequest request2 = new GetSceneStateRequest(cityID, gameWorld.CurrentSceneID, gameWorld.GetLocalPlayer().MaxHp, gameWorld.GetLocalPlayer().MaxShield, gameWorld.GetLocalPlayer().ExtraShield, gameWorld.GetLocalPlayer().GetWeaponList(), currentEquipWeaponSlot, element);
				GetInstance().GetNetworkManager().SendRequest(request2);
				gameWorld.GetLocalPlayer().UpdateAllDamageParas();
			}
			if (GetInstance().GetGameMode().IsVSMode())
			{
				gameWorld.GetLocalPlayer().DropAtSpawnPositionVS();
			}
			else
			{
				gameWorld.GetLocalPlayer().DropAtSpawnPosition();
			}
		}
		else
		{
			GetGameWorld().GetLocalPlayer().SetSeatID(0);
			GetGameWorld().GetLocalPlayer().DropAtSpawnPosition();
		}
		PlayBGM();
	}

	private void PlayBGM()
	{
		string text = "RPG_Audio/BGM/BGM_c0_c3_menu";
		switch (Application.loadedLevelName)
		{
		case "City1":
			text = "RPG_Audio/BGM/BGM_c1";
			GetGameWorld().GetLocalPlayer().FootStepAudio = "RPG_Audio/Player/player_footstep02_c1_c2_i3";
			break;
		case "City2":
			text = "RPG_Audio/BGM/BGM_c2";
			GetGameWorld().GetLocalPlayer().FootStepAudio = "RPG_Audio/Player/player_footstep02_c1_c2_i3";
			break;
		case "Instance2":
		case "VS1":
		case "VS1_1v1":
			text = "RPG_Audio/BGM/BGM_i2_i3";
			GetGameWorld().GetLocalPlayer().FootStepAudio = "RPG_Audio/Player/player_footstep02_c1_c2_i3";
			break;
		case "Instance3":
			text = "RPG_Audio/BGM/BGM_i2_i3";
			GetGameWorld().GetLocalPlayer().FootStepAudio = "RPG_Audio/Player/player_footstep02_c1_c2_i3";
			break;
		case "Instance0":
		case "Instance1":
		case "Instance4":
		case "Instance5":
		case "VS2":
		case "VS2_1v1":
			text = "RPG_Audio/BGM/BGM_i0_i1_i4";
			GetGameWorld().GetLocalPlayer().FootStepAudio = "RPG_Audio/Player/player_footstep02_c1_c2_i3";
			break;
		default:
			text = "RPG_Audio/BGM/BGM_c0_c3_menu";
			GetGameWorld().GetLocalPlayer().FootStepAudio = "RPG_Audio/Player/player_footstep01_c0_i0_i1";
			break;
		}
		AudioManager.GetInstance().PlayMusic(text);
	}

	public GameWorld GetGameWorld()
	{
		return gameWorld;
	}

	public GameWorld InitGameWorld()
	{
		if (gameWorld == null)
		{
			gameWorld = new GameWorld();
		}
		return gameWorld;
	}

	public StreamingManager GetSceneStreaingManager()
	{
		if (streamingManager == null)
		{
			streamingManager = new StreamingManager();
		}
		return streamingManager;
	}

	public void CreateSceneStreamingManager()
	{
		streamingManager = new StreamingManager();
	}

	public void ClearGameScene()
	{
		gameScene = null;
	}

	public LootManager GetLootManager()
	{
		return lootManager;
	}

	public void CreateLootManager()
	{
		lootManager = new LootManager();
		lootManager.Init();
	}

	public void ClearLootManager()
	{
		lootManager = null;
	}

	public void EnterBossLevel()
	{
	}

	public NetworkManager CreateNetwork()
	{
		networkMgr = new NetworkManager();
		networkMgr.StartNetwork(networkMgr.strIP, networkMgr.port);
		return networkMgr;
	}

	public NetworkManager CreateNetwork(string strIP, int port)
	{
		networkMgr = new NetworkManager();
		networkMgr.StartNetwork(strIP, port);
		return networkMgr;
	}

	public void DestoryNetWork()
	{
		if (networkMgr != null)
		{
			networkMgr.CloseConnection();
			networkMgr = null;
		}
	}

	public NetworkManager GetNetworkManager()
	{
		return networkMgr;
	}

	public UserState GetUserState()
	{
		return userState;
	}

	public GlobalState GetGlobalState()
	{
		return globalState;
	}

	public void Loop(float deltaTime)
	{
		gameScene.Loop(deltaTime);
		gameWorld.CheckShopRefresh();
		gameWorld.CheckBlackMarketRefresh();
		gameWorld.CheckRolePlayTimer();
		GambelManager.GetInstance().UpdateTimer();
	}

	public void LateLoop(float deltaTime)
	{
		gameScene.LateLoop(deltaTime);
	}

	public void CloseConnectionGameServer()
	{
		GetInstance().GetGameMode().TypeOfNetwork = NetworkType.Single;
		GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
		GetInstance().GetGameMode().SubModePlay = SubMode.Story;
		if (GetInstance().GetGameWorld() != null)
		{
			GetInstance().GetGameWorld().RemoveAllRemotePlayers();
		}
		if (networkMgr != null)
		{
			networkMgr.CloseConnection();
			networkMgr.IsDisplayErrorBox = false;
		}
		Lobby.GetInstance().SetCurrentRoomID(-1);
	}

	public bool SaveToCloud()
	{
		return true;
	}

	public loadIcloud LoadFromICloud()
	{
		return loadIcloud.None;
	}

	private bool LoadFromICloud(string version, int latestSerialId)
	{
		return false;
	}

	public bool SaveGlobalDataICloud(string version, int serialId)
	{
		return false;
	}

	public void Save()
	{
		if (!globalState.bInit)
		{
			Debug.Log("no save in local");
			return;
		}
		SaveUserDataLocal(globalState.GetCurrRole());
		SaveGlobalDataLocal();
	}

	private string GetRecPath()
	{
		string result = Application.dataPath + "/../../Documents/";
		if (Application.platform == RuntimePlatform.Android)
		{
			result = Application.persistentDataPath + "/";
		}
		return result;
	}

	public bool SaveGlobalDataLocal()
	{
		string recPath = GetRecPath();
		if (!Directory.Exists(recPath))
		{
			Directory.CreateDirectory(recPath);
		}
		MemoryStream memoryStream = null;
		BinaryWriter binaryWriter = null;
		byte[] data = null;
		memoryStream = new MemoryStream();
		binaryWriter = new BinaryWriter(memoryStream);
		try
		{
			globalState.SaveData(binaryWriter);
			data = memoryStream.ToArray();
		}
		catch (Exception)
		{
			data = null;
			Debug.LogError("Save Local data Exception.........");
			return false;
		}
		finally
		{
			memoryStream.Close();
			binaryWriter.Close();
			memoryStream = null;
			binaryWriter = null;
		}
		data = globalState.CryptBuffer(data, "Please quit the app immediately");
		return WriteFileGlobalDataForLocal(recPath + "BangOutMap", data);
	}

	public bool SaveUserDataLocal(UserState userState, string recName)
	{
		if (!globalState.VerifySaveRole(recName))
		{
			Debug.Log("no save role data when name is null..");
			return false;
		}
		if (!recName.Equals(GetUserState().GetRoleName()))
		{
			Debug.Log("no save role data when name mismatch.." + GetUserState().GetRoleName());
			return false;
		}
		string recPath = GetRecPath();
		MemoryStream memoryStream = null;
		BinaryWriter binaryWriter = null;
		memoryStream = new MemoryStream();
		binaryWriter = new BinaryWriter(memoryStream);
		byte[] data = null;
		try
		{
			userState.SaveData(binaryWriter);
			data = memoryStream.ToArray();
		}
		catch (Exception)
		{
			data = null;
			Debug.Log("save UserState data failed...");
			return false;
		}
		finally
		{
			binaryWriter.Close();
			memoryStream.Close();
			binaryWriter = null;
			memoryStream = null;
		}
		data = globalState.CryptBuffer(data, "Please quit the admob immediately");
		return WriteFileUserDataForLocal(recPath + recName, data);
	}

	public bool SaveUserDataLocal(string recName)
	{
		return SaveUserDataLocal(userState, recName);
	}

	private bool WriteFileGlobalDataForLocal(string filePath, byte[] buffer)
	{
		if (buffer == null)
		{
			return false;
		}
		Stream stream = null;
		try
		{
			stream = File.Open(filePath + "_01", FileMode.Create);
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();
		}
		catch (Exception)
		{
			Debug.LogError("Write Local global data Exception.........");
			return false;
		}
		finally
		{
			stream.Close();
			stream = null;
		}
		WriteFileCrypt(filePath + "_02", buffer, "b1e re sis 02a");
		return true;
	}

	private bool WriteFileUserDataForLocal(string filePath, byte[] buffer)
	{
		if (buffer == null)
		{
			return false;
		}
		Stream stream = null;
		try
		{
			stream = File.Open(filePath + "_01", FileMode.Create);
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();
		}
		catch (Exception)
		{
			Debug.LogError("Write Local user data Exception.........");
			return false;
		}
		finally
		{
			stream.Close();
			stream = null;
		}
		byte[] array = WriteFileCrypt(filePath + "_02", buffer, "b1e re sis 02b");
		List<RoleStateInfo> roles = globalState.GetRoles();
		for (int i = 0; i < roles.Count; i++)
		{
			if (roles[i].RoleName == userState.GetRoleName())
			{
				roles[i].MD5_Verify_Bytes = new byte[array.Length];
				array.CopyTo(roles[i].MD5_Verify_Bytes, 0);
				break;
			}
		}
		return true;
	}

	private bool WriteFileUserDataForICloud(string filePath, byte[] buffer, string roleName)
	{
		if (buffer == null)
		{
			return false;
		}
		Stream stream = null;
		try
		{
			stream = File.Open(filePath + "_01", FileMode.Create);
			stream.Write(buffer, 0, buffer.Length);
			stream.Flush();
		}
		catch (Exception)
		{
			Debug.LogError("Write Local user data Exception.........");
			return false;
		}
		finally
		{
			stream.Close();
			stream = null;
		}
		byte[] array = WriteFileCrypt(filePath + "_02", buffer, "b1e re sis 02b");
		List<RoleStateInfo> roleStateICloud = GlobalState.roleStateICloud;
		RoleStateInfo roleStateInfo = new RoleStateInfo();
		roleStateInfo.RoleName = roleName;
		roleStateInfo.MD5_Verify_Bytes = new byte[array.Length];
		array.CopyTo(roleStateInfo.MD5_Verify_Bytes, 0);
		roleStateICloud.Add(roleStateInfo);
		return true;
	}

	private byte[] WriteFileCrypt(string FullFilePath, byte[] buffer, string md5Key)
	{
		byte[] array = null;
		Stream stream = null;
		array = new byte[buffer.Length];
		buffer.CopyTo(array, 0);
		array = globalState.CryptMD5Buffer(array, md5Key);
		stream = File.Open(FullFilePath, FileMode.Create);
		stream.Write(array, 0, array.Length);
		stream.Flush();
		stream.Close();
		Debug.Log("Crypt data..");
		return array;
	}

	public loadRMS Load()
	{
		string recPath = GetRecPath();
		Debug.Log("into load() begin: " + recPath);
		if (File.Exists(recPath + "BangOutMap_01") && File.Exists(recPath + "BangOutMap_02"))
		{
			Stream stream = null;
			Stream stream2 = null;
			byte[] buffer = null;
			try
			{
				stream = File.Open(recPath + "BangOutMap_01", FileMode.Open);
				stream2 = File.Open(recPath + "BangOutMap_02", FileMode.Open);
				byte[] bytesFromStream = globalState.GetBytesFromStream(stream);
				byte[] bytesFromStream2 = globalState.GetBytesFromStream(stream2);
				if (!globalState.VerifyMD5(bytesFromStream, bytesFromStream2, "b1e re sis 02a"))
				{
					stream.Close();
					stream2.Close();
					Debug.Log("global data validation failed...");
					return loadRMS.RMSMismatch;
				}
				buffer = globalState.DecryptBuffer(bytesFromStream, "Please quit the app immediately");
			}
			catch (Exception)
			{
				Debug.Log("open global data failed...");
				return loadRMS.RMSException;
			}
			finally
			{
				stream.Close();
				stream2.Close();
				stream = null;
				stream2 = null;
			}
			MemoryStream memoryStream = null;
			BinaryReader binaryReader = null;
			string text;
			try
			{
				memoryStream = new MemoryStream(buffer);
				binaryReader = new BinaryReader(memoryStream);
				text = globalState.LoadData(binaryReader);
			}
			catch (Exception)
			{
				Debug.Log("load global data failed...");
				return loadRMS.RMSException;
			}
			finally
			{
				binaryReader.Close();
				memoryStream.Close();
				binaryReader = null;
				memoryStream = null;
			}
			Debug.Log("Load() version: " + text);
			if (!text.Equals(GlobalState.version))
			{
				Debug.Log("version mismatch...");
				List<string> rolesName = globalState.GetRolesName();
				for (int i = 0; i < rolesName.Count; i++)
				{
					if (!LoadUserDataLocal(text, rolesName[i]))
					{
						Debug.Log("load user data exception in version mismatch...");
						return loadRMS.RMSException;
					}
					if (!SaveUserDataLocal(rolesName[i]))
					{
						Debug.Log("save user data exception in version mismatch...");
						return loadRMS.RMSException;
					}
				}
				if (!SaveGlobalDataLocal())
				{
					Debug.Log("save global data exception in version mismatch...");
					return loadRMS.RMSException;
				}
			}
			return loadRMS.RMSPass;
		}
		return loadRMS.RMSEmpty;
	}

	public bool LoadUserDataLocal(UserState userState, string roleName)
	{
		return LoadUserDataLocal(userState, GlobalState.version, roleName);
	}

	public bool LoadUserDataLocal(string roleName)
	{
		return LoadUserDataLocal(GlobalState.version, roleName);
	}

	private bool LoadUserDataLocal(UserState userState, string ver, string roleName)
	{
		string recPath = GetRecPath();
		if (File.Exists(recPath + roleName + "_01") && File.Exists(recPath + roleName + "_02"))
		{
			Stream stream = null;
			Stream stream2 = null;
			byte[] buffer = null;
			byte[] array = null;
			try
			{
				stream = File.Open(recPath + roleName + "_01", FileMode.Open);
				stream2 = File.Open(recPath + roleName + "_02", FileMode.Open);
				byte[] bytesFromStream = globalState.GetBytesFromStream(stream);
				array = globalState.GetBytesFromStream(stream2);
				if (!globalState.VerifyMD5(bytesFromStream, array, "b1e re sis 02b"))
				{
					stream.Close();
					stream2.Close();
					stream = null;
					stream2 = null;
					Debug.Log("UserState data validation failed...");
					return false;
				}
				buffer = globalState.DecryptBuffer(bytesFromStream, "Please quit the admob immediately");
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				stream.Close();
				stream2.Close();
				stream = null;
				stream2 = null;
			}
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			try
			{
				userState.LoadData(ver, binaryReader);
				List<RoleStateInfo> roles = globalState.GetRoles();
				byte[] n = null;
				for (int i = 0; i < roles.Count; i++)
				{
					if (roles[i].RoleName == roleName)
					{
						n = roles[i].MD5_Verify_Bytes;
						break;
					}
				}
				int num = Convert.ToInt32(ver);
				if (num >= 120 && !VerifyArrar(array, n))
				{
					return false;
				}
			}
			catch (Exception)
			{
				Debug.Log("load UserState data failed...");
				return false;
			}
			finally
			{
				binaryReader.Close();
				memoryStream.Close();
				binaryReader = null;
				memoryStream = null;
			}
			return true;
		}
		return false;
	}

	private bool VerifyArrar(byte[] o, byte[] n)
	{
		if (o == null || n == null)
		{
			return false;
		}
		if (o.Length != n.Length)
		{
			return false;
		}
		for (int i = 0; i < o.Length; i++)
		{
			if (o[i] != n[i])
			{
				return false;
			}
		}
		return true;
	}

	private bool LoadUserDataLocal(string ver, string roleName)
	{
		return LoadUserDataLocal(userState, ver, roleName);
	}

	public void DeleteUserData(string roleName)
	{
		string recPath = GetRecPath();
		if (File.Exists(recPath + roleName + "_01") && File.Exists(recPath + roleName + "_02"))
		{
			File.Delete(recPath + roleName + "_01");
			File.Delete(recPath + roleName + "_02");
		}
		Debug.Log("delete UserState data completed...");
	}

	public List<UserState.RoleState> GetRoles()
	{
		List<string> rolesName = globalState.GetRolesName();
		if (rolesName == null || rolesName.Count <= 0)
		{
			return null;
		}
		Debug.Log("rolesName : " + rolesName.Count);
		List<UserState.RoleState> list = new List<UserState.RoleState>();
		for (int i = 0; i < rolesName.Count; i++)
		{
			UserState.RoleState roleState = LoadRoleState(rolesName[i]);
			if (roleState != null)
			{
				list.Add(roleState);
			}
			else
			{
				globalState.RemoveRole(rolesName[i]);
			}
		}
		return list;
	}

	private UserState.RoleState LoadRoleState(string roleName)
	{
		string recPath = GetRecPath();
		UserState.RoleState result = null;
		if (File.Exists(recPath + roleName + "_01") && File.Exists(recPath + roleName + "_02"))
		{
			Stream stream = null;
			Stream stream2 = null;
			byte[] buffer = null;
			try
			{
				stream = File.Open(recPath + roleName + "_01", FileMode.Open);
				stream2 = File.Open(recPath + roleName + "_02", FileMode.Open);
				byte[] bytesFromStream = globalState.GetBytesFromStream(stream);
				byte[] bytesFromStream2 = globalState.GetBytesFromStream(stream2);
				if (!globalState.VerifyMD5(bytesFromStream, bytesFromStream2, "b1e re sis 02b"))
				{
					stream.Close();
					stream2.Close();
					Debug.Log("UserState.RoleState data validation failed...");
					return null;
				}
				buffer = globalState.DecryptBuffer(bytesFromStream, "Please quit the admob immediately");
			}
			catch (Exception)
			{
				buffer = null;
				return null;
			}
			finally
			{
				stream.Close();
				stream2.Close();
				stream = null;
				stream2 = null;
			}
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			try
			{
				result = userState.LoadRoleData(binaryReader);
			}
			catch (Exception)
			{
				Debug.Log("load UserState.RoleState data failed...");
				return null;
			}
			finally
			{
				binaryReader.Close();
				memoryStream.Close();
				binaryReader = null;
				memoryStream = null;
			}
		}
		return result;
	}

	public VSManager GetVSManager()
	{
		return mVSManager;
	}

	public void CreateVSManager()
	{
		Mode modePlay = gameMode.ModePlay;
		if (modePlay == Mode.VS_TDM)
		{
			mVSManager = new VSTDMManager();
		}
	}

	public bool IsIphone4()
	{
		return false;
	}
}

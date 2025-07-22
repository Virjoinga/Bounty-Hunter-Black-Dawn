using System.Collections.Generic;
using UnityEngine;

public class RobotUser : MonoBehaviour
{
	private RobotUserState mCurrentRobotState;

	private RobotUserState mNextRobotState;

	private RobotStateEvent mNextRobotStateEvent;

	private TimeManager timeMgr;

	private GameApp gameApp;

	private Lobby lobby;

	private InputInfo inputInfo;

	private UserState mUserState;

	private RobotGameScene mGameScene;

	private NetworkManager mNetworkManager;

	private List<Weapon> mWeaponList;

	private RobotSkillManager mSkillManager;

	private RobotRoom mRobotRoom;

	private RobotLogin mRobotLogin;

	private RobotMoveDirection mLastMoveDirection;

	public int UserID { get; set; }

	public int Hp { get; set; }

	public int MaxHp { get; set; }

	public int Shield { get; set; }

	public int MaxShield { get; set; }

	public bool VSInvitaionOK { get; set; }

	private void Awake()
	{
		VSInvitaionOK = false;
		timeMgr = new TimeManager();
		gameApp = new GameApp();
		lobby = new Lobby();
		inputInfo = new InputInfo();
		mUserState = new UserState();
		mGameScene = new RobotGameScene(this);
		mWeaponList = new List<Weapon>();
		mSkillManager = new RobotSkillManager();
		mRobotRoom = new RobotRoom();
		mRobotLogin = new RobotLogin();
		SwitchStateTo(RobotUserStateID.CreateRobot);
	}

	private void Update()
	{
		if (mNextRobotState == null)
		{
			if (mCurrentRobotState != null)
			{
				mCurrentRobotState.Update(mNextRobotStateEvent);
			}
		}
		else
		{
			if (mCurrentRobotState != null)
			{
				mCurrentRobotState.Destroy();
			}
			mCurrentRobotState = null;
			mCurrentRobotState = mNextRobotState;
			mNextRobotState = null;
			mCurrentRobotState.Create();
		}
		mNextRobotStateEvent = RobotStateEvent.None;
		if (mNetworkManager != null)
		{
			timeMgr.RobotLoop(mNetworkManager);
			mNetworkManager.SendData();
			mNetworkManager.ProcessRobotReceivedPackets(this);
		}
	}

	private RobotUserState GetState(RobotUserStateID id)
	{
		RobotUserState result = null;
		switch (id)
		{
		case RobotUserStateID.CreateRobot:
			result = new RobotCreateState();
			break;
		case RobotUserStateID.Login:
			result = new RobotLoginState();
			break;
		case RobotUserStateID.Room:
			result = new RobotRoomState();
			break;
		case RobotUserStateID.Playing:
			result = new RobotPlayingState();
			break;
		case RobotUserStateID.VSPlaying:
			result = new RobotVSPlayingState();
			break;
		}
		return result;
	}

	public void Notify(RobotStateEvent eventID)
	{
		mNextRobotStateEvent = eventID;
	}

	public void SwitchStateTo(RobotUserStateID id)
	{
		mNextRobotState = GetState(id);
		mNextRobotState.SetRobotUser(this);
	}

	public RobotRoom GetRobotRoom()
	{
		return mRobotRoom;
	}

	public RobotLogin GetRobotLogin()
	{
		return mRobotLogin;
	}

	public TimeManager GetTimeManager()
	{
		return timeMgr;
	}

	public GameApp GetGameApp()
	{
		return gameApp;
	}

	public Lobby GetLobby()
	{
		return lobby;
	}

	public InputInfo GetInputInfo()
	{
		return inputInfo;
	}

	public UserState GetUserState()
	{
		return mUserState;
	}

	public NetworkManager GetNetworkManager()
	{
		return mNetworkManager;
	}

	public RobotGameScene GetGameScene()
	{
		return mGameScene;
	}

	public List<Weapon> GetWeaponList()
	{
		return mWeaponList;
	}

	public RobotSkillManager GetSkillManager()
	{
		return mSkillManager;
	}

	public void DisconnectNetWork()
	{
		if (mNetworkManager != null)
		{
			mGameScene = new RobotGameScene(this);
			mNetworkManager.CloseRobotConnection();
			SwitchStateTo(RobotUserStateID.Login);
			mNetworkManager = null;
		}
	}

	public NetworkManager CreateNetwork(string strIP, int port)
	{
		mNetworkManager = new NetworkManager(true);
		mNetworkManager.StartNetworkRobot(this, strIP, port);
		return mNetworkManager;
	}

	public void SetRobotUser(string name, CharacterClass charClass, Sex sex)
	{
		GetUserState().CreateNewRole(name, charClass, sex);
		GetUserState().SetCharLevel((short)Random.Range(1, 21));
		GetUserState().InitDeafaultEquips();
		InitWeaponList(GetWeaponList(), GetUserState());
		MaxHp = 2 * (100 + GetUserState().GetCharLevel() * 20);
		Hp = 2 * (100 + GetUserState().GetCharLevel() * 20);
		MaxShield = 200;
		Shield = 200;
		mSkillManager.Init(this);
	}

	private void InitWeaponList(List<Weapon> weaponList, UserState userState)
	{
		weaponList.Clear();
		weaponList.Add(null);
		weaponList.Add(null);
		weaponList.Add(null);
		weaponList.Add(null);
		if (userState.ItemInfoData.IsWeapon1Equiped)
		{
			NGUIBaseItem baseItem = userState.ItemInfoData.Weapon1.baseItem;
			if (baseItem != null)
			{
				Weapon weapon = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem.ItemClass, baseItem.name);
				weapon.SetWeaponPropertyWithNGUIBaseItem(baseItem);
				weaponList[0] = weapon;
			}
		}
		if (userState.ItemInfoData.IsWeapon2Equiped)
		{
			NGUIBaseItem baseItem2 = userState.ItemInfoData.Weapon2.baseItem;
			if (baseItem2 != null)
			{
				Weapon weapon2 = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem2.ItemClass, baseItem2.name);
				weapon2.SetWeaponPropertyWithNGUIBaseItem(baseItem2);
				weaponList[1] = weapon2;
			}
		}
		if (userState.ItemInfoData.IsWeapon3Equiped)
		{
			NGUIBaseItem baseItem3 = userState.ItemInfoData.Weapon3.baseItem;
			if (baseItem3 != null)
			{
				Weapon weapon3 = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem3.ItemClass, baseItem3.name);
				weapon3.SetWeaponPropertyWithNGUIBaseItem(baseItem3);
				weaponList[2] = weapon3;
			}
		}
		if (userState.ItemInfoData.IsWeapon4Equiped)
		{
			NGUIBaseItem baseItem4 = userState.ItemInfoData.Weapon4.baseItem;
			if (baseItem4 != null)
			{
				Weapon weapon4 = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem4.ItemClass, baseItem4.name);
				weapon4.SetWeaponPropertyWithNGUIBaseItem(baseItem4);
				weaponList[3] = weapon4;
			}
		}
	}

	public void SendInput()
	{
		base.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360), 0f));
		int num = Random.Range(0, 100);
		if (num < 50)
		{
			inputInfo.fire = true;
		}
		else
		{
			inputInfo.fire = false;
		}
		SendPlayerInputRequest request = new SendPlayerInputRequest(inputInfo.fire, inputInfo.IsMoving());
		mNetworkManager.SendRequestAsRobot(request, this);
		SendTransformStateRequest request2 = new SendTransformStateRequest(base.gameObject.transform.position, base.gameObject.transform.eulerAngles, timeMgr.NetworkTime);
		mNetworkManager.SendRequestAsRobot(request2, this);
	}

	public void Move()
	{
		if (mLastMoveDirection == RobotMoveDirection.NONE)
		{
			mLastMoveDirection = RobotMoveDirection.FORWARD;
			inputInfo.moveDirection = Vector3.forward;
		}
		else if (mLastMoveDirection == RobotMoveDirection.FORWARD)
		{
			mLastMoveDirection = RobotMoveDirection.RIGHT;
			inputInfo.moveDirection = Vector3.right;
		}
		else if (mLastMoveDirection == RobotMoveDirection.RIGHT)
		{
			mLastMoveDirection = RobotMoveDirection.BACK;
			inputInfo.moveDirection = Vector3.back;
		}
		else if (mLastMoveDirection == RobotMoveDirection.BACK)
		{
			mLastMoveDirection = RobotMoveDirection.LEFT;
			inputInfo.moveDirection = Vector3.left;
		}
		else if (mLastMoveDirection == RobotMoveDirection.LEFT)
		{
			mLastMoveDirection = RobotMoveDirection.NONE;
			inputInfo.moveDirection = Vector3.zero;
		}
		base.gameObject.transform.Translate(7.5f * Time.deltaTime * inputInfo.moveDirection, Space.Self);
	}

	public void ResetMoveDirection()
	{
		mLastMoveDirection = RobotMoveDirection.NONE;
		inputInfo.moveDirection = Vector3.zero;
	}
}

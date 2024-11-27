using System;
using UnityEngine;

internal class PlayerLoginResponse : Response
{
	public const byte ACCOUNT_PASS = 0;

	public const byte ACCOUNT_NOT_EXIST = 1;

	public const byte ACCOUNT_PASSWORD_INCORRECT = 2;

	public const byte ACCOUNT_LOCK = 3;

	public const byte ACCOUNT_BLOCKED = 4;

	public const byte VERSION_MISMATCH = 5;

	public const byte ACCOUNT_WAS_DELETED = 6;

	public const byte LOGIN_FAIL = 7;

	public const byte SERVER_BUSYNESS = 8;

	public const byte SERVER_MAINTAINEANCE = 9;

	public const byte GUEST_LOGIN = 11;

	private bool loginSuccess;

	private int channelID;

	private string ip;

	private int port;

	private int userID;

	private int mithril;

	private short timeSpan;

	private int result;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		result = bytesBuffer.ReadByte();
		loginSuccess = false;
		string empty = string.Empty;
		Debug.Log("result..." + result);
		switch (result)
		{
		case 0:
		{
			loginSuccess = true;
			userID = bytesBuffer.ReadInt();
			empty = bytesBuffer.ReadString();
			mithril = bytesBuffer.ReadInt();
			timeSpan = bytesBuffer.ReadShort();
			string[] array = empty.Split(':');
			ip = array[0];
			port = Convert.ToInt32(array[1]);
			Debug.Log("account pass...");
			break;
		}
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		case 11:
		{
			loginSuccess = true;
			empty = bytesBuffer.ReadString();
			string[] array = empty.Split(':');
			ip = array[0];
			port = Convert.ToInt32(array[1]);
			break;
		}
		case 10:
			break;
		}
	}

	public override void ProcessLogic()
	{
		if (loginSuccess)
		{
			Debug.Log("loginSuccess...");
			GlobalState.user_id = userID;
			UITeam.giftDaily = false;
			UIVSTeam.giftDaily = false;
			UserState userState = GameApp.GetInstance().GetUserState();
			if (timeSpan == 1)
			{
				GameApp.GetInstance().GetGlobalState().AddGiftTimeSpan(1);
				if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 41)
				{
					UIVSTeam.giftDaily = true;
				}
				else
				{
					UITeam.giftDaily = true;
				}
				GambelManager.GetInstance().ResetOnLine();
				GlobalState.SetLastLocalNotificationTime(DateTime.Now);
			}
			else if (timeSpan > 1)
			{
				GameApp.GetInstance().GetGlobalState().SetGiftTimeSpan(0);
				if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 41)
				{
					UIVSTeam.giftDaily = true;
				}
				else
				{
					UITeam.giftDaily = true;
				}
				GambelManager.GetInstance().ResetOnLine();
				GlobalState.SetLastLocalNotificationTime(DateTime.Now);
			}
			GameApp.GetInstance().DestoryNetWork();
			LoginGameServer();
			userState.UserId = userID;
			RoleLoginRequest request = new RoleLoginRequest(userState.GetRoleName(), userID, 0);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			Debug.Log("login game server :" + ip + ":" + port);
			return;
		}
		GameApp.GetInstance().CloseConnectionGameServer();
		UIMsgListener listener = null;
		if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 9)
		{
			if (UITeam.m_instance == null)
			{
				return;
			}
			listener = UITeam.m_instance.m_roomList;
		}
		else if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 41)
		{
			if (UIVSTeam.m_instance == null)
			{
				return;
			}
			listener = UIVSTeam.m_instance.m_roomList;
		}
		switch (result)
		{
		case 9:
			UIMsgBox.instance.ShowSystemMessage(listener, LocalizationManager.GetInstance().GetString("MSG_NET_SERVER_MAINTAINEANCE"), 2, 14);
			break;
		case 3:
		case 4:
		case 7:
			UIMsgBox.instance.ShowSystemMessage(listener, LocalizationManager.GetInstance().GetString("MSG_NET_ACCOUNT_LOCKED"), 2, 15);
			break;
		case 5:
			UIMsgBox.instance.ShowSystemMessage(listener, LocalizationManager.GetInstance().GetString("MSG_NET_VERSION_MISMATCH"), 2, 16);
			break;
		case 6:
		case 8:
			break;
		}
	}

	public void LoginGameServer()
	{
		GameApp.GetInstance().CreateNetwork(ip, port);
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
	}
}

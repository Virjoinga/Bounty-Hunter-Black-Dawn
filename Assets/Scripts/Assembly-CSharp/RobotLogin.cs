using System;
using UnityEngine;

public class RobotLogin
{
	public void LoginAsRobot(RobotUser robot)
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			string text = "M_" + Environment.MachineName;
			robot.GetLobby().SetUserName(robot.GetUserState().GetRoleName());
			NetworkManager networkManager = robot.CreateNetwork("192.168.1.189", 10290);
			robot.GetTimeManager().Init();
			robot.GetTimeManager().setPeriod(3f);
			robot.GetTimeManager().LastSynTime = Time.time;
			robot.GetLobby().IsGuest = false;
			RoleLoginRequest request = new RoleLoginRequest(robot.GetUserState().GetRoleName(), 0, 1);
			robot.GetNetworkManager().SendRequestAsRobot(request, robot);
		}
		else
		{
			string empty = string.Empty;
			robot.GetLobby().SetUserName(robot.GetUserState().GetRoleName());
			NetworkManager networkManager2 = robot.CreateNetwork("192.168.1.189", 10290);
			robot.GetTimeManager().Init();
			robot.GetTimeManager().setPeriod(3f);
			robot.GetTimeManager().LastSynTime = Time.time;
			robot.GetLobby().IsGuest = false;
			RoleLoginRequest request2 = new RoleLoginRequest(robot.GetUserState().GetRoleName(), 0, 1);
			robot.GetNetworkManager().SendRequestAsRobot(request2, robot);
		}
	}

	public void LoginSuccess()
	{
	}
}

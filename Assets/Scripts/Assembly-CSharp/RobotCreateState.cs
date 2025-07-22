using UnityEngine;

public class RobotCreateState : RobotUserState
{
	protected override void OnCreate()
	{
		base.OnCreate();
		string name = "NewRobot" + GetRobotUser().UserID;
		CharacterClass charClass = (CharacterClass)Random.Range(1, 5);
		Sex sex = (Sex)Random.Range(0, 2);
		GetRobotUser().SetRobotUser(name, charClass, sex);
		GetRobotUser().SwitchStateTo(RobotUserStateID.Login);
	}
}

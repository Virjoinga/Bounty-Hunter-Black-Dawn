using System.Collections.Generic;
using UnityEngine;

public class RobotUserManager : MonoBehaviour
{
	private string robotInitialID = "500";

	private string robotNumber = "50";

	private bool bCreate;

	private List<RobotUser> robotList = new List<RobotUser>();

	private static RobotUserManager instance;

	public static RobotUserManager GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		LocalizationManager.GetInstance().currentLanguage = SystemLanguage.English.ToString();
	}

	private void Start()
	{
		Res2DManager.GetInstance().Init();
		Res2DManager.GetInstance().SetResData(39);
		Res2DManager.GetInstance().SetResData(0);
		Res2DManager.GetInstance().SetResData(1);
		Res2DManager.GetInstance().SetResData(2);
		Res2DManager.GetInstance().SetResData(3);
		Res2DManager.GetInstance().SetResData(4);
		Res2DManager.GetInstance().SetResData(5);
		Res2DManager.GetInstance().SetResData(6);
		Res2DManager.GetInstance().SetResData(7);
		Res2DManager.GetInstance().SetResData(8);
		Res2DManager.GetInstance().SetResData(9);
		Res2DManager.GetInstance().SetResData(10);
		Res2DManager.GetInstance().SetResData(11);
		Res2DManager.GetInstance().SetResData(12);
		Res2DManager.GetInstance().SetResData(13);
		Res2DManager.GetInstance().SetResData(14);
		Res2DManager.GetInstance().SetResData(15);
		Res2DManager.GetInstance().SetResData(16);
		Res2DManager.GetInstance().SetResData(17);
		Res2DManager.GetInstance().SetResData(18);
		Res2DManager.GetInstance().SetResData(19);
		Res2DManager.GetInstance().SetResData(20);
		Res2DManager.GetInstance().SetResData(21);
		Res2DManager.GetInstance().SetResData(22);
		Res2DManager.GetInstance().SetResData(23);
		Res2DManager.GetInstance().SetResData(24);
		Res2DManager.GetInstance().SetResData(25);
		Res2DManager.GetInstance().SetResData(26);
		Res2DManager.GetInstance().SetResData(27);
		Res2DManager.GetInstance().SetResData(28);
		Res2DManager.GetInstance().SetResData(43);
		Res2DManager.GetInstance().SetResData(73);
		Res2DManager.GetInstance().SetResData(40);
		Res2DManager.GetInstance().SetResData(41);
		Res2DManager.GetInstance().SetResData(42);
		Res2DManager.GetInstance().SetResData(44);
		Res2DManager.GetInstance().SetResData(45);
		Res2DManager.GetInstance().SetResData(52);
		Res2DManager.GetInstance().SetResData(46);
		Res2DManager.GetInstance().SetResData(47);
		Res2DManager.GetInstance().SetResData(48);
		Res2DManager.GetInstance().SetResData(50);
		Res2DManager.GetInstance().SetResData(49);
		Res2DManager.GetInstance().SetResData(38);
		Res2DManager.GetInstance().SetResData(51);
		Res2DManager.GetInstance().SetResData(37);
		Res2DManager.GetInstance().SetResData(29);
		Res2DManager.GetInstance().SetResData(30, 36);
		Res2DManager.GetInstance().SetResData(53, 59);
		Res2DManager.GetInstance().LoadResInit(1, -1);
		Res2DManager.GetInstance().LoadResPro();
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
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 120f, 20f), "机器人初始ID：");
		robotInitialID = GUI.TextField(new Rect(120f, 10f, 44f, 20f), robotInitialID, 5);
		GUI.Label(new Rect(10f, 50f, 120f, 20f), "总登录机器人数：");
		robotNumber = GUI.TextField(new Rect(120f, 50f, 44f, 20f), robotNumber, 5);
		if (GUI.Button(new Rect(10f, 100f, 110f, 100f), "开始创建机器人") && !bCreate)
		{
			bCreate = true;
			CreateRobot();
		}
	}

	private void CreateRobot()
	{
		int num = int.Parse(robotInitialID);
		int num2 = int.Parse(robotNumber);
		GameObject original = Resources.Load("Robot/Robot") as GameObject;
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.name = "Robot" + (num + i);
			RobotUser component = gameObject.GetComponent<RobotUser>();
			component.UserID = num + i;
			robotList.Add(component);
		}
	}
}

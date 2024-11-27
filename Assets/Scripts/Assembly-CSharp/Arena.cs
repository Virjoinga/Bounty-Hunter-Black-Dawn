using System.Collections.Generic;
using UnityEngine;

public class Arena : EffectsCameraListener
{
	private static Arena instance;

	private int citySceneID = 1;

	private int level;

	private SubMode mode;

	public int LastSceneID
	{
		get
		{
			return citySceneID;
		}
	}

	private Arena()
	{
	}

	public static Arena GetInstance()
	{
		if (instance == null)
		{
			instance = new Arena();
		}
		return instance;
	}

	public void Set(SubMode mode)
	{
		Set(mode, citySceneID, level);
	}

	public void Set(SubMode mode, int citySceneID)
	{
		Set(mode, citySceneID, level);
	}

	public void Set(SubMode mode, int citySceneID, int level)
	{
		this.mode = mode;
		this.citySceneID = citySceneID;
		this.level = level;
		EnemySpawnScript.InitSpawnTableID = level;
	}

	public void Enter(SubMode mode)
	{
		Enter(mode, citySceneID, level);
	}

	public void Enter(SubMode mode, int citySceneID)
	{
		Enter(mode, citySceneID, level);
	}

	public void Enter(SubMode mode, int citySceneID, int level)
	{
		Set(mode, citySceneID, level);
		Enter();
	}

	public void EnterBossRush()
	{
		Enter(SubMode.Boss_Rush, 50, 0);
	}

	public bool Enter()
	{
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(citySceneID);
		if (sceneConfig == null)
		{
			return false;
		}
		EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
		return true;
	}

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		EffectsCamera.instance.RemoveListener();
		GameApp.GetInstance().GetGameMode().SubModePlay = mode;
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		gameScene.LeaveScene();
		int arenaSceneID = GetArenaSceneID(citySceneID);
		Debug.Log("citySceneID : " + citySceneID);
		Debug.Log("arenaID : " + arenaSceneID);
		Debug.Log("EnemySpawnScript.InitSpawnTableID = " + EnemySpawnScript.InitSpawnTableID);
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(arenaSceneID);
		if (arenaSceneID < 0)
		{
			Debug.Log("Error Arena citySceneID : " + citySceneID);
		}
		Application.LoadLevel(sceneConfig.SceneFileName);
	}

	public int GetArenaSceneID(int citySceneID)
	{
		Dictionary<string, SceneConfig> sceneConfig = GameConfig.GetInstance().sceneConfig;
		foreach (SceneConfig value in sceneConfig.Values)
		{
			if (value.ArenaBelongToWhichSceneID == citySceneID)
			{
				return value.SceneID;
			}
		}
		return -1;
	}

	public bool IsArena(int sceneID)
	{
		Dictionary<string, SceneConfig> sceneConfig = GameConfig.GetInstance().sceneConfig;
		foreach (SceneConfig value in sceneConfig.Values)
		{
			if (sceneID == value.SceneID && value.ArenaBelongToWhichSceneID > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsCurrentSceneArena()
	{
		return IsArena(GameApp.GetInstance().GetGameWorld().CurrentSceneID);
	}
}

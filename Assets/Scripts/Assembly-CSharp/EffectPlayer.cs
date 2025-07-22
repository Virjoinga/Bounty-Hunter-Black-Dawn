using System;
using UnityEngine;

public class EffectPlayer
{
	public enum Type
	{
		LevelUp = 0,
		MissionAccomplished = 1,
		PlayerShieldBreak = 2,
		OthersShieldBreak = 3,
		ExtraShieldBreak = 4,
		HealingWave = 5,
		MeleeWave = 6,
		HealingEffect = 7,
		SpeedDown = 8,
		Spark = 9
	}

	private static EffectPlayer instance;

	private float z;

	private DateTime[] timers;

	private int[] delays;

	private EffectPlayer()
	{
		z = 0f;
		timers = new DateTime[10];
		delays = new int[10];
		delays[3] = 750;
		for (int i = 0; i < timers.Length; i++)
		{
			timers[i] = DateTime.MinValue;
		}
	}

	public static EffectPlayer GetInstance()
	{
		if (instance == null)
		{
			instance = new EffectPlayer();
		}
		return instance;
	}

	public GameObject Play(Type type)
	{
		if ((DateTime.Now - timers[(byte)type]).Milliseconds > delays[(byte)type])
		{
			timers[(byte)type] = DateTime.Now;
			string path = GetPath(type);
			GameObject gameObject = Resources.Load(path) as GameObject;
			if (gameObject == null || gameObject.GetComponent<AutoDestroyScript>() == null)
			{
				return null;
			}
			return UnityEngine.Object.Instantiate(gameObject) as GameObject;
		}
		return null;
	}

	private string GetPath(Type type)
	{
		string result = string.Empty;
		switch (type)
		{
		case Type.LevelUp:
			result = "HUD/Effect/LevelUp";
			break;
		case Type.MissionAccomplished:
			result = "HUD/Effect/MissionAccomplished";
			break;
		case Type.PlayerShieldBreak:
			result = "HUD/Effect/PlayerShieldBreak";
			break;
		case Type.OthersShieldBreak:
			result = "HUD/Effect/EnemyShieldBreak";
			break;
		case Type.ExtraShieldBreak:
			result = "RPG_effect/RPG_ProtectionCover_Destruction";
			break;
		case Type.HealingWave:
			result = "RPG_effect/RPG_HealingWave_001";
			break;
		case Type.MeleeWave:
			result = "RPG_effect/RPG_KnifeAttack_001";
			break;
		case Type.HealingEffect:
			result = "RPG_effect/RPG_MedicalTower_001";
			break;
		case Type.SpeedDown:
			result = "RPG_effect/RPG_SpeedDown";
			break;
		case Type.Spark:
			result = "HUD/Effect/SparkOfUpgrade";
			break;
		}
		return result;
	}

	public GameObject PlayLevelUp()
	{
		GameObject gameObject = Play(Type.LevelUp);
		if (gameObject != null)
		{
			gameObject.transform.parent = HUDManager.instance.m_InfoManager.transform;
			gameObject.transform.localPosition = new Vector3(0f, 150f, z);
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = new Vector3(180f, 180f, 1f);
			z = (z - 1f) % 50f - 50f;
		}
		return gameObject;
	}

	public GameObject PlayMissionAccomplished()
	{
		GameObject gameObject = Play(Type.MissionAccomplished);
		if (gameObject != null)
		{
			gameObject.transform.parent = HUDManager.instance.m_InfoManager.transform;
			gameObject.transform.localPosition = new Vector3(0f, 150f, z);
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = new Vector3(180f, 180f, 1f);
			z = (z - 1f) % 50f - 50f;
		}
		return gameObject;
	}

	public GameObject PlayLocalPlayerShieldBreak()
	{
		GameObject gameObject = Play(Type.PlayerShieldBreak);
		if (gameObject != null)
		{
			gameObject.transform.parent = HUDManager.instance.m_InfoManager.transform;
			gameObject.transform.localPosition = new Vector3(0f, -200f, 0f);
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = new Vector3(360f, 360f, 1f);
		}
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.Twitch);
		return gameObject;
	}

	public GameObject PlayOthersShieldBreak(Vector3 pos, Quaternion rotation, Vector3 scale)
	{
		GameObject gameObject = Play(Type.OthersShieldBreak);
		if (gameObject != null)
		{
			gameObject.transform.position = pos;
			gameObject.transform.rotation = rotation;
			gameObject.transform.localScale = scale;
		}
		return gameObject;
	}

	public GameObject PlayExtraShieldBreak(Vector3 pos)
	{
		GameObject gameObject = Play(Type.ExtraShieldBreak);
		if (gameObject != null)
		{
			gameObject.transform.position = pos;
		}
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.Twitch);
		return gameObject;
	}

	public GameObject PlayHealingWave(Vector3 pos)
	{
		GameObject gameObject = Play(Type.HealingWave);
		if (gameObject != null)
		{
			gameObject.transform.position = pos;
		}
		return gameObject;
	}

	public GameObject PlayMeleeWave()
	{
		GameObject gameObject = Play(Type.MeleeWave);
		if (gameObject != null)
		{
			gameObject.transform.position = Camera.main.transform.position;
			gameObject.transform.rotation = Camera.main.transform.rotation;
			gameObject.transform.parent = Camera.main.transform;
		}
		return gameObject;
	}

	public GameObject PlayHealingEffect()
	{
		GameObject gameObject = null;
		if (GameApp.GetInstance().GetGameWorld() != null && GameApp.GetInstance().GetGameWorld().GetLocalPlayer() != null)
		{
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			gameObject = Play(Type.HealingEffect);
			if (gameObject != null)
			{
				gameObject.transform.position = localPlayer.GetPosition() + Vector3.up * 1.5f + localPlayer.GetTransform().forward * 0.5f;
				gameObject.transform.parent = localPlayer.GetTransform();
			}
		}
		return gameObject;
	}

	public GameObject PlaySpeedDownEffect(Transform trans)
	{
		GameObject gameObject = null;
		if (GameApp.GetInstance().GetGameWorld() != null && trans != null)
		{
			gameObject = Play(Type.SpeedDown);
			if (gameObject != null)
			{
				gameObject.transform.position = trans.position + Vector3.up;
				gameObject.transform.parent = trans;
			}
		}
		return gameObject;
	}

	public GameObject PlaySparkOfUpgrade(Transform trans)
	{
		GameObject gameObject = null;
		if (trans != null)
		{
			gameObject = Play(Type.Spark);
			if (gameObject != null)
			{
				gameObject.transform.parent = trans;
				gameObject.transform.localPosition = new Vector3(600f, -445f, 330f);
				gameObject.transform.localEulerAngles = new Vector3(355f, 185f, 75f);
				gameObject.transform.localScale = Vector3.one;
			}
		}
		return gameObject;
	}
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TutorialManager
{
	public enum TutorialType
	{
		Move = 0,
		RotateCamera = 1,
		OpenMenuToEquipWeapon = 2,
		EquipWeapon = 3,
		Fire = 4,
		Reload = 5,
		ChangeWeapon = 6,
		OpenMenuToLearnSkill = 7,
		LearnSkill = 8,
		Map = 9,
		Rader = 10,
		ShopBuyFirstWeapon = 11,
		ShopRefresh = 12,
		ShopBulletExtend = 13,
		ShopBulletBuy = 14,
		ShopBuySell = 15,
		ShopBuyBack = 16,
		EquipmentUpgrade = 17
	}

	private static TutorialManager instance;

	private GameObject m_Tutorial;

	private bool[] unlock;

	private List<TutorialType> firstTutorialTypeList = new List<TutorialType>();

	private List<TutorialType> learnSkillTutorialTypeList = new List<TutorialType>();

	private List<TutorialType> mapTutorialTypeList = new List<TutorialType>();

	private List<TutorialType> raderTutorialTypeList = new List<TutorialType>();

	private List<TutorialType> shopTutorialTypeList = new List<TutorialType>();

	private List<TutorialType> equipmentUpgradeTutorialTypeList = new List<TutorialType>();

	private TutorialManager()
	{
		unlock = new bool[18];
		for (int i = 0; i < unlock.Length; i++)
		{
			unlock[i] = false;
		}
		firstTutorialTypeList.Add(TutorialType.Move);
		firstTutorialTypeList.Add(TutorialType.RotateCamera);
		firstTutorialTypeList.Add(TutorialType.OpenMenuToEquipWeapon);
		firstTutorialTypeList.Add(TutorialType.EquipWeapon);
		firstTutorialTypeList.Add(TutorialType.Fire);
		firstTutorialTypeList.Add(TutorialType.Reload);
		firstTutorialTypeList.Add(TutorialType.ChangeWeapon);
		learnSkillTutorialTypeList.Add(TutorialType.OpenMenuToLearnSkill);
		mapTutorialTypeList.Add(TutorialType.Map);
		raderTutorialTypeList.Add(TutorialType.Rader);
		shopTutorialTypeList.Add(TutorialType.ShopBuyFirstWeapon);
		shopTutorialTypeList.Add(TutorialType.ShopRefresh);
		shopTutorialTypeList.Add(TutorialType.ShopBulletExtend);
		shopTutorialTypeList.Add(TutorialType.ShopBulletBuy);
		shopTutorialTypeList.Add(TutorialType.ShopBuySell);
		shopTutorialTypeList.Add(TutorialType.ShopBuyBack);
		equipmentUpgradeTutorialTypeList.Add(TutorialType.EquipmentUpgrade);
	}

	public void Load(BinaryReader br)
	{
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			unlock[i] = br.ReadBoolean();
		}
		if (!IsFirstTutorialOk())
		{
			ClearTutorial(firstTutorialTypeList);
		}
		if (!IsLearnSkillTutorialOk())
		{
			ClearTutorial(learnSkillTutorialTypeList);
		}
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(unlock.Length);
		bool[] array = unlock;
		foreach (bool value in array)
		{
			bw.Write(value);
		}
	}

	public static TutorialManager GetInstance()
	{
		if (instance == null)
		{
			instance = new TutorialManager();
		}
		return instance;
	}

	public void SetTutorialOk(TutorialType type)
	{
		unlock[(byte)type] = true;
	}

	public bool IsTutorialOk(TutorialType type)
	{
		return unlock[(byte)type];
	}

	public bool IsLearnSkillTutorialOk()
	{
		return IsTutorialOk(learnSkillTutorialTypeList);
	}

	public bool IsShopTutorialOk()
	{
		return IsTutorialOk(shopTutorialTypeList);
	}

	public bool IsMapTutorialOk()
	{
		return IsTutorialOk(mapTutorialTypeList);
	}

	public bool IsRaderTutorialOk()
	{
		return IsTutorialOk(raderTutorialTypeList);
	}

	public bool IsFirstTutorialOk()
	{
		return IsTutorialOk(firstTutorialTypeList);
	}

	public bool IsEquipmentUpgradeTutorialOk()
	{
		return IsTutorialOk(equipmentUpgradeTutorialTypeList);
	}

	public bool IsTutorialOk(List<TutorialType> list)
	{
		bool result = true;
		for (int i = 0; i < list.Count; i++)
		{
			if (!unlock[(int)list[i]])
			{
				result = false;
			}
		}
		return result;
	}

	private void ClearTutorial(List<TutorialType> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			unlock[(int)list[i]] = false;
		}
	}

	public bool IsEffectsCameraNeedUse()
	{
		SceneConfig currentSceneConfig = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetCurrentSceneConfig();
		if (currentSceneConfig.FatherSceneID != currentSceneConfig.SceneID)
		{
			return IsMapTutorialOk() && IsRaderTutorialOk();
		}
		return IsFirstTutorialOk() && IsEquipmentUpgradeTutorialOk();
	}

	public bool IsCanCreateTutorial()
	{
		bool flag = false;
		bool[] array = unlock;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				flag = true;
			}
		}
		SceneConfig currentSceneConfig = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetCurrentSceneConfig();
		return (flag && currentSceneConfig.FatherSceneID == currentSceneConfig.SceneID) || !IsMapTutorialOk() || !IsRaderTutorialOk();
	}

	public bool CreateTutorialPrefab()
	{
		if (m_Tutorial == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Tutorial", "TutorialUI");
			m_Tutorial = Object.Instantiate(original) as GameObject;
			return true;
		}
		return false;
	}

	public bool DestroyTutorialPrefab()
	{
		if (m_Tutorial == null)
		{
			return false;
		}
		MemoryManager.FreeNGUI(m_Tutorial);
		m_Tutorial = null;
		return true;
	}
}

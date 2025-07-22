using System.Collections.Generic;
using UnityEngine;

public class NumberManager
{
	public const int DAMAGE_COUNT = 2;

	public const int EXP_COUNT = 1;

	private static NumberManager instance;

	private List<UINumber> uiDamageList;

	private List<UINumber> uiExpList;

	private NumberManager()
	{
		uiDamageList = new List<UINumber>();
		uiExpList = new List<UINumber>();
	}

	public static NumberManager GetInstance()
	{
		if (instance == null)
		{
			instance = new NumberManager();
		}
		return instance;
	}

	public void Init()
	{
		InitResource("Damage", uiDamageList, 2);
		InitResource("Exp", uiExpList, 1);
	}

	private void InitResource(string path, List<UINumber> numberList, int total)
	{
		GameObject original = ResourceLoad.GetInstance().LoadUI("Number", path);
		for (int i = 0; i < total; i++)
		{
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			UINumber[] componentsInChildren = gameObject.GetComponentsInChildren<UINumber>();
			UINumber[] array = componentsInChildren;
			foreach (UINumber item in array)
			{
				numberList.Add(item);
			}
		}
	}

	public void Clear()
	{
		Clear(uiDamageList);
		Clear(uiExpList);
	}

	private void Clear(List<UINumber> numberList)
	{
		foreach (UINumber number in numberList)
		{
			Object.Destroy(number.gameObject);
		}
		numberList.Clear();
	}

	public void ShowDamage(UserStateHUD.DamageHUD damageHUD)
	{
		for (int i = 0; i < uiDamageList.Count; i++)
		{
			if (!uiDamageList[i].gameObject.activeSelf)
			{
				string empty = string.Empty;
				empty = ((!damageHUD.Critical) ? (string.Empty + damageHUD.Damage) : (LocalizationManager.GetInstance().GetString("MSG_CRITICAL") + " " + damageHUD.Damage));
				empty = AddColor(empty, damageHUD.ElementType);
				uiDamageList[i].SetData(damageHUD.EnemyPosition, empty);
				uiDamageList[i].gameObject.SetActive(true);
				break;
			}
		}
	}

	public void ShowExp(Vector3 enemyPos, int exp)
	{
		for (int i = 0; i < uiExpList.Count; i++)
		{
			if (!uiExpList[i].gameObject.activeSelf)
			{
				string str = "[ffff00]" + exp + "EXP[-]";
				uiExpList[i].SetData(enemyPos, str);
				uiExpList[i].gameObject.SetActive(true);
				break;
			}
		}
	}

	public void ShowImmunity(Vector3 pos)
	{
		for (int i = 0; i < uiDamageList.Count; i++)
		{
			if (!uiDamageList[i].gameObject.activeSelf)
			{
				string str = "[FF0000]MISS![-]";
				uiDamageList[i].SetData(pos, str);
				uiDamageList[i].gameObject.SetActive(true);
				break;
			}
		}
	}

	private string AddColor(string text, ElementType type)
	{
		string text2 = "[ffffff]";
		switch (type)
		{
		case ElementType.Fire:
			text2 = "[ff9c00]";
			break;
		case ElementType.Shock:
			text2 = "[00d2ff]";
			break;
		case ElementType.Corrosive:
			text2 = "[00da8a]";
			break;
		}
		return text2 + text + "[-]";
	}
}

using UnityEngine;

internal class WeaponResourceConfig
{
	public static Renderer[] GetWeaponRenderers(GameObject weaponObj, int gunID)
	{
		return weaponObj.GetComponentsInChildren<Renderer>();
	}

	public static float GetBagSize(int bagID)
	{
		float result = 1f;
		switch (bagID)
		{
		case 14:
			result = 1.2f;
			break;
		case 15:
			result = 1.2f;
			break;
		case 16:
			result = 1.2f;
			break;
		case 17:
			result = 1.2f;
			break;
		case 18:
			result = 1.1f;
			break;
		}
		return result;
	}

	public static void RotateGun(GameObject weaponObj, int gunID)
	{
	}

	public static void RotateGunInUI(GameObject weaponObj, int gunID)
	{
		switch (gunID)
		{
		case 22:
		case 23:
		case 24:
		case 25:
		case 31:
		case 32:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(-30f, 270f, 0f));
			break;
		case 27:
		case 33:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(210f, 90f, 0f));
			break;
		case 28:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(-60f, 90f, 0f));
			break;
		default:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(240f, 270f, 0f));
			break;
		}
	}

	public static Transform GetWeaponGunFire(GameObject weaponObj, int gunID)
	{
		Transform result = weaponObj.transform.Find("Point01");
		switch (gunID)
		{
		case 22:
			result = weaponObj.transform.GetChild(2).Find("Point01");
			break;
		case 23:
			result = weaponObj.transform.GetChild(0).Find("Point01");
			break;
		case 24:
		case 25:
			result = weaponObj.transform.GetChild(1).Find("Point01");
			break;
		}
		return result;
	}
}

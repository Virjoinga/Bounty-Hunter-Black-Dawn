using System.Collections.Generic;
using System.IO;

public class ItemInfiniteBullet : GlobalIAPItem
{
	private Dictionary<WeaponType, bool> infiniteBulletDic = new Dictionary<WeaponType, bool>();

	public ItemInfiniteBullet()
	{
		infiniteBulletDic.Add(WeaponType.SubMachineGun, false);
		infiniteBulletDic.Add(WeaponType.RPG, false);
	}

	public void ReadData(BinaryReader br)
	{
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			WeaponType key = (WeaponType)br.ReadInt32();
			infiniteBulletDic[key] = br.ReadBoolean();
		}
	}

	public void WriteData(BinaryWriter bw)
	{
		bw.Write(infiniteBulletDic.Count);
		foreach (KeyValuePair<WeaponType, bool> item in infiniteBulletDic)
		{
			bw.Write((int)item.Key);
			bw.Write(item.Value);
		}
	}

	public void Resume()
	{
	}

	public void Resume(WeaponType weaponType)
	{
		if (infiniteBulletDic.ContainsKey(weaponType))
		{
			infiniteBulletDic[weaponType] = true;
		}
		else
		{
			infiniteBulletDic.Add(weaponType, true);
		}
	}

	public bool IsUnlimitedBullet(WeaponType weaponType)
	{
		if (!infiniteBulletDic.ContainsKey(weaponType))
		{
			return false;
		}
		return infiniteBulletDic[weaponType];
	}
}

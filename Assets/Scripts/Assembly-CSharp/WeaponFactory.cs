using System;

public class WeaponFactory
{
	protected static WeaponFactory instance;

	protected static WeaponType[] weaponIDToWeaponType = new WeaponType[36];

	public static WeaponFactory GetInstance()
	{
		if (instance == null)
		{
			instance = new WeaponFactory();
		}
		return instance;
	}

	public Weapon CreateWeapon(WeaponType wType)
	{
		Weapon weapon = null;
		switch (wType)
		{
		case WeaponType.SubMachineGun:
			return new SMG();
		case WeaponType.AssaultRifle:
			return new AssaultRifle();
		case WeaponType.Pistol:
			return new Pistol();
		case WeaponType.Revolver:
			return new Revolver();
		case WeaponType.ShotGun:
			return new ShotGun();
		case WeaponType.Sniper:
			return new Sniper();
		case WeaponType.RPG:
		case WeaponType.RocketLauncher:
			return new RocketLauncher();
		case WeaponType.GrenadeLauncher:
			return new GrenadeLauncher();
		case WeaponType.PlasmaNeo:
			return new PlasmaNeo();
		case WeaponType.LaserGun:
			return new LaserCannon();
		case WeaponType.LightBow:
			return new LightBow();
		case WeaponType.LightFist:
			return new LightFist();
		case WeaponType.MachineGun:
			return new MachineGun();
		case WeaponType.LaserRifle:
			return new LaserRifle();
		case WeaponType.AutoBow:
			return new AutoBow();
		case WeaponType.AutoRocketLauncher:
			return new AutoRocketLauncher();
		case WeaponType.Sword:
			return new Sword();
		case WeaponType.AdvancedShotGun:
			return new AdvancedShotGun();
		case WeaponType.AdvancedSword:
			return new AdvancedSword();
		default:
			return new AssaultRifle();
		}
	}

	public Weapon CreateWeapon(WeaponType wType, string prefabName)
	{
		Weapon weapon = null;
		weapon = CreateWeapon(wType);
		string value = prefabName[prefabName.Length - 2].ToString() + prefabName[prefabName.Length - 1];
		weapon.WeaponNameNumber = Convert.ToByte(value);
		return weapon;
	}

	public Weapon CreateThirdPersonWeapon(WeaponType wType, byte wNameNumber)
	{
		Weapon weapon = null;
		weapon = CreateWeapon(wType);
		weapon.WeaponNameNumber = wNameNumber;
		return weapon;
	}
}

using UnityEngine;

internal class PlayerFireRocketResponse : Response
{
	public byte type;

	public Vector3 pos;

	public Vector3 dir;

	public byte elementType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		type = bytesBuffer.ReadByte();
		short num = bytesBuffer.ReadShort();
		short num2 = bytesBuffer.ReadShort();
		short num3 = bytesBuffer.ReadShort();
		short num4 = bytesBuffer.ReadShort();
		short num5 = bytesBuffer.ReadShort();
		short num6 = bytesBuffer.ReadShort();
		elementType = bytesBuffer.ReadByte();
		float x = (float)num / 10f;
		float y = (float)num2 / 10f;
		float z = (float)num3 / 10f;
		pos = new Vector3(x, y, z);
		x = (float)num4 / 10f;
		y = (float)num5 / 10f;
		z = (float)num6 / 10f;
		dir = new Vector3(x, y, z);
	}

	public override void ProcessLogic()
	{
		string path = "RPG_effect/RPG_Grenade_Projectile";
		switch (type)
		{
		case 7:
		case 22:
			path = "RPG_effect/RPG_RPG01_Projectile";
			break;
		case 8:
			path = "RPG_effect/RPG_Grenade_Projectile";
			break;
		}
		GameObject original = Resources.Load(path) as GameObject;
		GameObject gameObject = Object.Instantiate(original, pos, Quaternion.LookRotation(dir)) as GameObject;
		if (type == 10 || type == 8)
		{
			GrenadePhysicsScript component = gameObject.GetComponent<GrenadePhysicsScript>();
			GrenadeShotScript component2 = gameObject.GetComponent<GrenadeShotScript>();
			component.dir = dir;
			component.life = 8f;
			component2.dir = dir;
			component2.flySpeed = 16f;
			component2.explodeRadius = 5f;
			component2.life = 3f;
			component2.damage = 10;
			component2.GunType = (WeaponType)type;
			component2.isLocal = false;
			component2.elementType = (ElementType)elementType;
		}
		else
		{
			ProjectileScript component3 = gameObject.GetComponent<ProjectileScript>();
			component3.dir = dir;
			component3.flySpeed = 16f;
			component3.explodeRadius = 5f;
			component3.life = 8f;
			component3.damage = 10;
			component3.GunType = (WeaponType)type;
			component3.isLocal = false;
			component3.elementType = (ElementType)elementType;
			if (type == 21)
			{
				component3.isPenerating = true;
			}
		}
	}
}

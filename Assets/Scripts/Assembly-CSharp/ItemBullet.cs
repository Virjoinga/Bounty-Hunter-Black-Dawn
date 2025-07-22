using UnityEngine;

public class ItemBullet : ItemBase
{
	public int Bullet;

	public WeaponType WeaponType;

	public override void generateItemProperties()
	{
		base.generateItemProperties();
	}

	public override void affectItemPropertiesByLevelAndFormula()
	{
	}

	public override void generateEquipmentSkills()
	{
	}

	protected override void GetIntoBackPack()
	{
	}

	protected override void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer == PhysicsLayer.PLAYER_COLLIDER)
		{
			Debug.Log("Player Touched Bullet!");
			UserStateHUD.GetInstance().InfoBox.PushBulletDescription(WeaponType, Bullet);
			GameApp.GetInstance().GetUserState().AddBulletByWeaponType(WeaponType, (short)Bullet);
			Object.Destroy(base.gameObject);
			if (AudioManager.GetInstance().IsPlayingSound("RPG_Audio/Item/Pick_Ammo"))
			{
				AudioManager.GetInstance().StopSound("RPG_Audio/Item/Pick_Ammo");
			}
			AudioManager.GetInstance().PlaySound("RPG_Audio/Item/Pick_Ammo");
			GameObject original = Resources.Load("RPG_effect/RPG_pick_white") as GameObject;
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.transform.parent = c.transform;
			gameObject.transform.localPosition = new Vector3(0f, 1f, 1f);
			gameObject.transform.localRotation = Quaternion.identity;
		}
	}

	private void Awake()
	{
		base.transform.rotation = Quaternion.identity;
	}

	public override void generateNGUIBaseItem()
	{
		mNGUIBaseItem.name = base.ItemName;
	}
}

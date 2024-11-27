using UnityEngine;

public class ItemFirstAid : ItemBase
{
	public int RecoverHP;

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
			Debug.Log("Player Touched FirstAid!");
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.RecoverHP((float)GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.MaxHp / 100f * (float)RecoverHP);
			Object.Destroy(base.gameObject);
			if (AudioManager.GetInstance().IsPlayingSound("RPG_Audio/Item/Pick_Medic"))
			{
				AudioManager.GetInstance().StopSound("RPG_Audio/Item/Pick_Medic");
			}
			AudioManager.GetInstance().PlaySound("RPG_Audio/Item/Pick_Medic");
			GameObject original = Resources.Load("RPG_effect/RPG_pick_green") as GameObject;
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

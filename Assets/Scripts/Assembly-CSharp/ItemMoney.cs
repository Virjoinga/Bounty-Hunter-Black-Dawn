using UnityEngine;

public class ItemMoney : ItemBase
{
	public int Money { get; set; }

	public override void generateItemProperties()
	{
		base.generateItemProperties();
		Money = (int)((float)Money * Random.Range(0.9f, 1.1f));
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
		if (c.GetComponent<Collider>().gameObject.layer != PhysicsLayer.PLAYER_COLLIDER)
		{
			return;
		}
		int num = Random.Range(0, 100);
		if (num < 50)
		{
			if (AudioManager.GetInstance().IsPlayingSound("Audio/pickup/pickup_money01"))
			{
				AudioManager.GetInstance().StopSound("Audio/pickup/pickup_money01");
			}
			AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_money01");
		}
		else
		{
			if (AudioManager.GetInstance().IsPlayingSound("Audio/pickup/pickup_money02"))
			{
				AudioManager.GetInstance().StopSound("Audio/pickup/pickup_money02");
			}
			AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_money02");
		}
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		GameObject gameObject = Object.Instantiate(original, c.transform.position + Vector3.up, Quaternion.identity) as GameObject;
		gameObject.transform.parent = c.transform;
		UserStateHUD.GetInstance().InfoBox.PushMoneyDescription(Money);
		GameApp.GetInstance().GetUserState().AddCash(Money);
		Object.Destroy(base.gameObject);
	}

	private void Awake()
	{
		base.transform.rotation = Quaternion.identity;
	}

	private new void Update()
	{
		if (!HasAddedToItemRoot)
		{
			GameObject itemRoot = GameApp.GetInstance().GetGameScene().GetItemRoot();
			if (null != itemRoot)
			{
				base.gameObject.transform.parent = itemRoot.transform;
			}
			HasAddedToItemRoot = true;
		}
	}
}

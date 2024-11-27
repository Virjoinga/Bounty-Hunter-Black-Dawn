using UnityEngine;

public abstract class ProtectedItem : ControllableUnit
{
	public EProtectedType ProtectedType { get; set; }

	public new bool IsMaster
	{
		get
		{
			return GameApp.GetInstance().GetGameMode().IsSingle() || Lobby.GetInstance().IsMasterPlayer;
		}
	}

	public ProtectedItem()
	{
		base.ControllableType = EControllableType.PROTECTED;
	}

	public override void Init()
	{
		base.Init();
		gameScene = GameApp.GetInstance().GetGameScene();
		base.Name = "P_" + base.ID;
		GameObject original = Resources.Load(GetResourcePath()) as GameObject;
		GameObject gameObject = Object.Instantiate(original, base.Position, base.Rotation) as GameObject;
		gameObject.name = base.Name;
		SetObject(gameObject);
		animation = entityObject.GetComponent<Animation>();
		mHitCheckCollider = entityObject.transform.Find("collision").gameObject.GetComponent<Collider>();
		mShieldRecoveryStartTimer.SetTimer(5f, true);
		StartInit();
	}

	public virtual void LateLoop(float deltaTime)
	{
	}
}

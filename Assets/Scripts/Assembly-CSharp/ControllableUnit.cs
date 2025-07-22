using UnityEngine;

public abstract class ControllableUnit : GameUnit
{
	public static ControllableUnitState INIT_STATE = new ControllableUnitInitState();

	public static ControllableUnitState IDLE_STATE = new ControllableUnitIdleState();

	public static ControllableUnitState DEAD_STATE = new ControllableUnitDeadState();

	public static ControllableUnitState DISAPPEAR_STATE = new ControllableUnitDisappearState();

	protected float startTime;

	protected ControllableUnitState mState;

	protected Vector3 mDeltaPosition = Vector3.zero;

	protected int mCurrentDeltaCount;

	protected int mMaxDeltaCount = 30;

	protected Collider mHitCheckCollider;

	public EControllableType ControllableType { get; set; }

	public byte ID { get; set; }

	public byte Level { get; set; }

	public float Duration { get; set; }

	public new string Name { get; set; }

	public bool IsMaster { get; set; }

	public Vector3 Position { get; set; }

	public Quaternion Rotation { get; set; }

	public ControllableUnit()
	{
		base.GameUnitType = EGameUnitType.CONTROLLABLE_ITEM;
	}

	public static GameObject GetControllableByCollider(Collider c)
	{
		return c.gameObject.transform.parent.gameObject;
	}

	public override bool InPlayingState()
	{
		if (mState == DEAD_STATE || mState == DISAPPEAR_STATE)
		{
			return false;
		}
		return true;
	}

	protected virtual string GetResourcePath()
	{
		return string.Empty;
	}

	public virtual Collider GetHitCheckCollider()
	{
		return mHitCheckCollider;
	}

	public virtual void Init()
	{
		mShieldRecoverySecondTimer.SetTimer(1f, true);
	}

	public virtual void Destroy()
	{
		Object.Destroy(entityObject);
	}

	public virtual void Loop(float deltaTime)
	{
		mState.NextState(this);
		DoShieldRecovery(deltaTime);
		if (IsMaster)
		{
			if (Time.time - startTime > Duration)
			{
				EndCurrentState();
				StartDisappear();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					ControllableItemDisappearRequest request = new ControllableItemDisappearRequest(ControllableType, ID);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		else if (mCurrentDeltaCount < mMaxDeltaCount)
		{
			entityTransform.position += mDeltaPosition / mMaxDeltaCount;
			mCurrentDeltaCount++;
		}
	}

	protected override void SendShieldRecoveryRequest()
	{
		if (IsMaster)
		{
			int deltaShield = Shield - mPrevShield;
			ControllableItemShieldRecoveryRequest request = new ControllableItemShieldRecoveryRequest(ControllableType, ID, deltaShield);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		mPrevShield = Shield;
	}

	public virtual void UpdatePosition(Vector3 position)
	{
		if (!IsMaster)
		{
			mDeltaPosition = position - entityTransform.position;
			mCurrentDeltaCount = 0;
		}
	}

	protected void SendTransform()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && IsMaster)
		{
			ControllableItemSendTransformRequest request = new ControllableItemSendTransformRequest(ControllableType, ID, entityTransform.position, entityTransform.rotation);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public virtual void SetState(ControllableUnitState newState)
	{
		mState = newState;
	}

	public virtual void OnHit(int damage)
	{
		if (!InPlayingState())
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			ControllableItemOnHitRequest request = new ControllableItemOnHitRequest(ControllableType, ID, damage);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			return;
		}
		ResetShieldRecoveryStartTimer();
		if (Shield > 0)
		{
			Shield -= damage;
			if (Shield < 0)
			{
				damage = -Shield;
				Shield = 0;
			}
		}
		if (Shield <= 0 && damage > 0)
		{
			Hp -= damage;
			if (Hp < 0)
			{
				Hp = 0;
			}
		}
		Debug.Log("hp: " + Hp + ",   shield: " + Shield);
		if (Hp <= 0)
		{
			EndCurrentState();
			StartDead();
		}
	}

	public virtual void OnHitResponse(int hp, int shield)
	{
		if (InPlayingState())
		{
			ResetShieldRecoveryStartTimer();
			Shield = shield;
			if (hp < Hp)
			{
				Hp = hp;
			}
			if (Hp <= 0)
			{
				EndCurrentState();
				StartDead();
			}
		}
		Debug.Log(Name + ",    hp: " + Hp + ",   shield: " + Shield);
	}

	public virtual void EndCurrentState()
	{
		if (mState == INIT_STATE)
		{
			EndInit();
		}
		else if (mState == IDLE_STATE)
		{
			EndIdle();
		}
		else if (mState == DEAD_STATE)
		{
			EndDead();
		}
		else if (mState == DISAPPEAR_STATE)
		{
			EndDisappear();
		}
	}

	protected virtual void StartInit()
	{
	}

	public virtual void DoInit()
	{
	}

	protected virtual void EndInit()
	{
	}

	protected virtual void StartIdle()
	{
	}

	public virtual void DoIdle()
	{
	}

	protected virtual void EndIdle()
	{
	}

	public virtual void StartDead()
	{
	}

	public virtual void DoDead()
	{
	}

	protected virtual void EndDead()
	{
	}

	public virtual void StartDisappear()
	{
	}

	public virtual void DoDisappear()
	{
	}

	protected virtual void EndDisappear()
	{
	}

	public virtual short GetPara1()
	{
		return 0;
	}

	public virtual short GetPara2()
	{
		return 0;
	}

	public virtual short GetPara3()
	{
		return 0;
	}

	public virtual short GetPara4()
	{
		return 0;
	}

	public virtual short GetPara5()
	{
		return 0;
	}

	public virtual short GetPara6()
	{
		return 0;
	}

	public virtual short GetPara7()
	{
		return 0;
	}

	public virtual short GetPara8()
	{
		return 0;
	}

	public virtual short GetPara9()
	{
		return 0;
	}

	public virtual short GetPara10()
	{
		return 0;
	}

	public virtual short GetPara11()
	{
		return 0;
	}

	public virtual short GetPara12()
	{
		return 0;
	}

	public virtual short GetPara13()
	{
		return 0;
	}

	public virtual short GetPara14()
	{
		return 0;
	}

	public virtual short GetPara15()
	{
		return 0;
	}

	public virtual short GetPara16()
	{
		return 0;
	}

	public virtual void InitValuesAndRanges(short para1, short para2, short para3, short para4, short para5, short para6, short para7, short para8, short para9, short para10, short para11, short para12, short para13, short para14, short para15, short para16)
	{
	}
}

using System.Collections.Generic;
using UnityEngine;

public class Traps : SummonedItem
{
	private const short RangeBonusSkillID = 1072;

	private const short BuffTimeBonusSkillID = 1077;

	private const short SlowDownBonusRateSkillID = 1084;

	private const short ExplosionDamageSkillID = 1088;

	public const int TRIGGER_DAMAGE = 100;

	public byte SlowDownEnemyRate = 40;

	public byte SlowDownBonusRate;

	public short SlowDownEnemyRange = 5;

	public short SlowDownEnemyTime = 10;

	public short SlowDownEnemyTimeBonus;

	public byte ExplosionDamageRate;

	public short ExplosionDamageRange = 10;

	private Timer mSlowDownEnemyTimer = new Timer();

	public Traps()
	{
		base.SummonedType = ESummonedType.TRAPS;
	}

	public override short GetPara1()
	{
		return (short)base.MaxHp;
	}

	public override short GetPara2()
	{
		return (short)base.MaxShield;
	}

	public override void InitValuesAndRanges(short para1, short para2, short para3, short para4, short para5, short para6, short para7, short para8, short para9, short para10, short para11, short para12, short para13, short para14, short para15, short para16)
	{
		base.MaxHp = para1;
		Hp = para1;
		base.MaxShield = para2;
		Shield = para2;
		if (!IsSameTeam())
		{
			GetObject().GetComponent<Renderer>().enabled = false;
			GetObject().transform.Find("RPG_Trap_001").gameObject.SetActive(false);
		}
	}

	protected override string GetResourcePath()
	{
		return "Controllable/Summoned/Traps";
	}

	public override void Init()
	{
		base.Init();
		mOwnerPlayer.AddSummoned(base.Name, this);
	}

	protected override void StartInit()
	{
		SetState(ControllableUnit.INIT_STATE);
		if (!base.IsMaster)
		{
			return;
		}
		foreach (CharacterInstantSkill triggerSkill in mOwnerPlayer.GetCharacterSkillManager().GetTriggerSkillList())
		{
			switch (triggerSkill.skillID)
			{
			case 1077:
				SlowDownEnemyTimeBonus = (short)Mathf.CeilToInt(triggerSkill.EffectValueY);
				Debug.Log("SlowDownEnemyTimeBonus = " + SlowDownEnemyTimeBonus);
				break;
			case 1084:
				SlowDownBonusRate = (byte)Mathf.CeilToInt(triggerSkill.EffectValueY);
				Debug.Log("SlowDownBonusRate = " + SlowDownBonusRate);
				break;
			case 1088:
				ExplosionDamageRate = (byte)Mathf.CeilToInt(triggerSkill.EffectValueX * 100f);
				ExplosionDamageRange = triggerSkill.Range;
				Debug.Log("EEEEEEEEEEEEEEEEEEEEE: " + ExplosionDamageRate + "---" + ExplosionDamageRange);
				break;
			}
		}
		foreach (CharacterStateSkill stateSkill in mOwnerPlayer.GetCharacterSkillManager().GetStateSkillList())
		{
			short skillID = stateSkill.skillID;
			if (skillID == 1072)
			{
				SlowDownEnemyRange = stateSkill.Range;
				Debug.Log("SlowDownEnemyRange = " + SlowDownEnemyRange);
			}
		}
	}

	public override void DoInit()
	{
		EndInit();
		StartIdle();
	}

	protected override void EndInit()
	{
	}

	protected override void StartIdle()
	{
		SetState(ControllableUnit.IDLE_STATE);
		mSlowDownEnemyTimer.SetTimer(1f, true);
	}

	public override void DoIdle()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
		if (!base.IsMaster || SlowDownEnemyRate <= 0 || !mSlowDownEnemyTimer.Ready())
		{
			return;
		}
		mSlowDownEnemyTimer.Do();
		bool flag = false;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
			{
				CharacterStateSkill characterStateSkill = new CharacterStateSkill();
				characterStateSkill.skillID = 10770;
				characterStateSkill.IsPermanent = false;
				characterStateSkill.Duration = (float)SlowDownEnemyTime + (float)SlowDownEnemyTimeBonus;
				characterStateSkill.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeMoveSpeed;
				characterStateSkill.BuffValueY1 = (float)(int)SlowDownEnemyRate + (float)(int)SlowDownBonusRate;
				characterStateSkill.FunctionType1 = BuffFunctionType.SpeedDown;
				if (remotePlayer != null && remotePlayer.InPlayingState() && !remotePlayer.IsSameTeam(localPlayer) && !(remotePlayer.GetObject() == null))
				{
					float num = Vector3.Distance(remotePlayer.GetPosition(), GetPosition());
					if (num < (float)SlowDownEnemyRange)
					{
						ChangeRemotePlayerStateRequest request = new ChangeRemotePlayerStateRequest(remotePlayer.GetUserID(), characterStateSkill, 1003);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						flag = true;
						ControllableItemDisappearRequest request2 = new ControllableItemDisappearRequest(base.ControllableType, base.ID);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Mine_Clearance, AchievementTrigger.Type.Data);
						achievementTrigger.PutData(1);
						AchievementManager.GetInstance().Trigger(achievementTrigger);
					}
				}
			}
		}
		else
		{
			Collider[] array = Physics.OverlapSphere(GetPosition(), SlowDownEnemyRange, 1 << PhysicsLayer.ENEMY);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Ray ray = new Ray(GetPosition(), collider.transform.position - GetPosition());
				float distance = Mathf.Sqrt((GetPosition() - collider.transform.position).sqrMagnitude);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY)) && hitInfo.collider.gameObject.layer != PhysicsLayer.ENEMY)
				{
					continue;
				}
				GameObject enemyByCollider = Enemy.GetEnemyByCollider(collider);
				if (!enemyByCollider.name.StartsWith("E_"))
				{
					continue;
				}
				Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider.name);
				if (enemyByID != null)
				{
					CharacterSkillManager characterSkillManager2 = enemyByID.GetCharacterSkillManager();
					CharacterStateSkill characterStateSkill2 = new CharacterStateSkill();
					characterStateSkill2.skillID = 10770;
					characterStateSkill2.IsPermanent = false;
					characterStateSkill2.Duration = (float)SlowDownEnemyTime + (float)SlowDownEnemyTimeBonus;
					characterStateSkill2.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
					characterStateSkill2.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeMoveSpeed;
					characterStateSkill2.BuffValueY1 = 0f - ((float)(int)SlowDownEnemyRate + (float)(int)SlowDownBonusRate);
					characterStateSkill2.FunctionType1 = BuffFunctionType.PropertyChange;
					characterSkillManager2.AddSkill(characterStateSkill2);
					characterStateSkill2.StartBuff();
					flag = true;
					Debug.Log(string.Concat(characterStateSkill2.FunctionType1, "|||||||||||", characterStateSkill2.BuffValueY1));
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
					{
						ControllableItemDisappearRequest request3 = new ControllableItemDisappearRequest(base.ControllableType, base.ID);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
						EnemySpeedDownRequest request4 = new EnemySpeedDownRequest(enemyByID.PointID, enemyByID.EnemyID, characterStateSkill2.skillID, (short)(characterStateSkill2.Duration * 10f), (byte)characterStateSkill2.BuffValueY1);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request4);
					}
					AchievementTrigger achievementTrigger2 = AchievementTrigger.Create(AchievementID.Mine_Clearance, AchievementTrigger.Type.Data);
					achievementTrigger2.PutData(1);
					AchievementManager.GetInstance().Trigger(achievementTrigger2);
				}
			}
		}
		if (ExplosionDamageRate > 0 && flag)
		{
			GameObject original = Resources.Load("RPG_effect/RPG_engineer_gun_transform_001") as GameObject;
			Object.Instantiate(original, entityTransform.position + Vector3.up * 0.5f, Quaternion.identity);
			TrapExplosion();
		}
		if (flag)
		{
			StartDisappear();
		}
	}

	protected override void EndIdle()
	{
	}

	public override void StartDead()
	{
		SetState(ControllableUnit.DEAD_STATE);
	}

	public override void DoDead()
	{
		gameScene.AddToDeletSummoned(this);
	}

	protected override void EndDead()
	{
	}

	public override void StartDisappear()
	{
		SetState(ControllableUnit.DISAPPEAR_STATE);
		GameObject original = Resources.Load("RPG_effect/RPG_engineer_gun_transform_001") as GameObject;
		Object.Instantiate(original, entityTransform.position + Vector3.up * 0.5f, Quaternion.identity);
	}

	public override void DoDisappear()
	{
		gameScene.AddToDeletSummoned(this);
	}

	protected override void EndDisappear()
	{
	}

	public void TrapExplosion()
	{
		Debug.Log("Explosion ! Range: " + ExplosionDamageRange + " Damage Rate: " + ExplosionDamageRate + "%");
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = Mathf.CeilToInt((float)(CharacterSkillManager.CalculateExplosionSkillDamage() * ExplosionDamageRate) * 0.01f);
		damageProperty.criticalAttack = false;
		damageProperty.isLocal = true;
		damageProperty.wType = WeaponType.NoGun;
		damageProperty.isPenetration = false;
		damageProperty.unitLevel = base.Level;
		damageProperty.weaponLevel = base.Level;
		Debug.Log("Explosion ! dp: " + damageProperty.damage);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (GameApp.GetInstance().GetGameMode().IsTeamMode())
				{
					Player remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(item.GetUserID());
					if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(mOwnerPlayer))
					{
						continue;
					}
				}
				Vector3 vector = item.GetTransform().position + new Vector3(0f, 1f, 0f);
				float sqrMagnitude = (vector - GetPosition()).sqrMagnitude;
				float num = ExplosionDamageRange * ExplosionDamageRange;
				if (sqrMagnitude < num)
				{
					Ray ray = new Ray(GetPosition(), vector - GetPosition());
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo, ExplosionDamageRange, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
					{
						damageProperty.damage = Mathf.CeilToInt((float)damageProperty.damage * (1f - 2f * Mathf.Sqrt(sqrMagnitude) / (float)(3 * ExplosionDamageRange)));
						Weapon weapon = mOwnerPlayer.GetWeapon();
						damageProperty.elementType = weapon.mCurrentElementType;
						weapon.CalculateElement(damageProperty, item, true);
						PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(mOwnerPlayer.GetUserID(), (short)damageProperty.damage, item.GetUserID(), damageProperty.isPenetration, (byte)damageProperty.elementType, damageProperty.criticalAttack, damageProperty.isTriggerDlementDot, damageProperty.elementDotDamage, damageProperty.elementDotTime, damageProperty.wType, DamageProperty.AttackerType._Traps);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
			}
			{
				foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
				{
					if (remotePlayer == null)
					{
						continue;
					}
					foreach (KeyValuePair<string, SummonedItem> summoned in remotePlayer.GetSummonedList())
					{
						if (summoned.Value == null || !summoned.Value.InPlayingState() || summoned.Value.IsSameTeam())
						{
							continue;
						}
						Vector3 vector2 = summoned.Value.GetTransform().position + new Vector3(0f, 1f, 0f);
						float sqrMagnitude2 = (vector2 - GetPosition()).sqrMagnitude;
						float num2 = ExplosionDamageRange * ExplosionDamageRange;
						if (sqrMagnitude2 < num2)
						{
							Ray ray2 = new Ray(GetPosition(), vector2 - GetPosition());
							RaycastHit hitInfo2;
							if (Physics.Raycast(ray2, out hitInfo2, ExplosionDamageRange, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.SUMMONED)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
							{
								damageProperty.damage = Mathf.CeilToInt((float)damageProperty.damage * (1f - 2f * Mathf.Sqrt(sqrMagnitude2) / (float)(3 * ExplosionDamageRange)));
								ControllableItemOnHitRequest request2 = new ControllableItemOnHitRequest(summoned.Value.ControllableType, summoned.Value.ID, damageProperty.damage);
								GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
							}
						}
					}
				}
				return;
			}
		}
		Collider[] array = Physics.OverlapSphere(GetPosition(), ExplosionDamageRange, 1 << PhysicsLayer.ENEMY);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			Ray ray3 = new Ray(GetPosition(), collider.transform.position - GetPosition());
			float distance = Mathf.Sqrt((GetPosition() - collider.transform.position).sqrMagnitude);
			RaycastHit hitInfo3;
			if (Physics.Raycast(ray3, out hitInfo3, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY)) && hitInfo3.collider.gameObject.layer != PhysicsLayer.ENEMY)
			{
				continue;
			}
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider.name);
				if (enemyByID != null)
				{
					damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up * 2f;
					enemyByID.HitEnemy(damageProperty);
				}
			}
		}
	}
}

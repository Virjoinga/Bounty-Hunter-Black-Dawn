using System.Collections.Generic;
using UnityEngine;

public class HealingStation : SummonedItem
{
	private const short RecoverBonusSkillID = 1055;

	private const short DamageReduceSkillID = 1060;

	private const short ShieldRecoverSkillID = 1061;

	private const short SlowDownEnemySkillID = 1063;

	private const short CriticalBonusSkillID = 1067;

	private const short ExplosionDamageSkillID = 1068;

	public byte HPRecoverRate = 1;

	public byte HPRecoverBonus;

	public short HPRecvoerRange = 10;

	public byte DamageReduceRate;

	public short DamageReduceRange = 10;

	protected string DamageReduceIcon = string.Empty;

	public byte ShieldRecoverSkillPercentage;

	public short ShieldRecoverRange = 10;

	private bool ShieldRecoverSkillAdded;

	public byte SlowDownEnemyRate;

	public short SlowDownEnemyRange = 10;

	public byte CriticalBonusRate;

	public short CriticalBonusRange;

	protected string CriticalBonusIcon = string.Empty;

	public byte ExplosionDamageRate;

	public short ExplosionDamageRange;

	private Timer mHPRecoverTimer = new Timer();

	private Timer mDamageReduceTimer = new Timer();

	private Timer mSlowDownEnemyTimer = new Timer();

	private Timer mCriticalBonusTimer = new Timer();

	private Timer mRangeDamageTimer = new Timer();

	public HealingStation()
	{
		base.SummonedType = ESummonedType.HEALING_STATION;
	}

	public override short GetPara1()
	{
		return HPRecoverBonus;
	}

	public override short GetPara2()
	{
		return HPRecvoerRange;
	}

	public override short GetPara3()
	{
		return DamageReduceRate;
	}

	public override short GetPara4()
	{
		return DamageReduceRange;
	}

	public override short GetPara5()
	{
		return ShieldRecoverSkillPercentage;
	}

	public override short GetPara6()
	{
		return ShieldRecoverRange;
	}

	public override short GetPara7()
	{
		return SlowDownEnemyRate;
	}

	public override short GetPara8()
	{
		return SlowDownEnemyRange;
	}

	public override short GetPara9()
	{
		return CriticalBonusRate;
	}

	public override short GetPara10()
	{
		return CriticalBonusRange;
	}

	public override void InitValuesAndRanges(short para1, short para2, short para3, short para4, short para5, short para6, short para7, short para8, short para9, short para10, short para11, short para12, short para13, short para14, short para15, short para16)
	{
		HPRecoverBonus = (byte)para1;
		HPRecvoerRange = para2;
		DamageReduceRate = (byte)para3;
		DamageReduceRange = para4;
		ShieldRecoverSkillPercentage = (byte)para5;
		ShieldRecoverRange = para6;
		SlowDownEnemyRate = (byte)para7;
		SlowDownEnemyRange = para8;
		CriticalBonusRate = (byte)para9;
		CriticalBonusRange = para10;
	}

	protected override string GetResourcePath()
	{
		return "Controllable/Summoned/HealingStation";
	}

	public override void Init()
	{
		base.Init();
		mOwnerPlayer.AddSummoned(base.Name, this);
		CreateNavMeshAgent();
		DamageReduceIcon = GameConfig.GetInstance().skillConfig[10601].IconName;
		CriticalBonusIcon = GameConfig.GetInstance().skillConfig[10671].IconName;
	}

	protected override void StartInit()
	{
		SetState(ControllableUnit.INIT_STATE);
		if (base.IsMaster)
		{
			foreach (CharacterInstantSkill triggerSkill in mOwnerPlayer.GetCharacterSkillManager().GetTriggerSkillList())
			{
				switch (triggerSkill.skillID)
				{
				case 1055:
					HPRecoverBonus = (byte)Mathf.CeilToInt(triggerSkill.EffectValueX * 100f);
					Debug.Log(triggerSkill.EffectValueX + "++++" + HPRecoverBonus);
					HPRecvoerRange = triggerSkill.Range;
					break;
				case 1061:
					ShieldRecoverSkillPercentage = (byte)(triggerSkill.EffectValueX * 100f);
					ShieldRecoverRange = triggerSkill.Range;
					break;
				case 1063:
				{
					SkillConfig skillConfig = GameConfig.GetInstance().skillConfig[triggerSkill.skillID * 10 + triggerSkill.SkillLevel];
					CharacterStateSkill characterStateSkill = new CharacterStateSkill((short)skillConfig.X1);
					SlowDownEnemyRate = (byte)Mathf.CeilToInt(characterStateSkill.BuffValueX1 * 100f);
					SlowDownEnemyRange = triggerSkill.Range;
					break;
				}
				case 1068:
					ExplosionDamageRate = (byte)(triggerSkill.EffectValueX * 100f);
					ExplosionDamageRange = triggerSkill.Range;
					Debug.Log("EEEEEEEEEEEEEEEEEEEEE: " + ExplosionDamageRate + "---" + ExplosionDamageRange);
					break;
				}
			}
			foreach (CharacterStateSkill stateSkill in mOwnerPlayer.GetCharacterSkillManager().GetStateSkillList())
			{
				switch (stateSkill.skillID)
				{
				case 1060:
					DamageReduceRate = (byte)stateSkill.BuffValueY1;
					DamageReduceRange = stateSkill.Range;
					break;
				case 1067:
					CriticalBonusRate = (byte)stateSkill.BuffValueY1;
					CriticalBonusRange = stateSkill.Range;
					break;
				}
			}
		}
		ShieldRecoverSkillAdded = false;
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
		mHPRecoverTimer.SetTimer(1f, true);
		mDamageReduceTimer.SetTimer(1f, true);
		mSlowDownEnemyTimer.SetTimer(1f, true);
		mCriticalBonusTimer.SetTimer(1f, true);
		mRangeDamageTimer.SetTimer(1f, true);
	}

	public override void DoIdle()
	{
		PlaySoundSingle("RPG_Audio/Summoned/HealBeacon/healbeacon_healing");
		DoCheckLogic();
	}

	protected override void EndIdle()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
		characterSkillManager.RemoveSkillByID(10610);
		ShieldRecoverSkillAdded = false;
	}

	public override void StartDead()
	{
		SetState(ControllableUnit.DEAD_STATE);
		GameObject original = Resources.Load("RPG_effect/RPG_engineer_gun_transform_001") as GameObject;
		Object.Instantiate(original, entityTransform.position + Vector3.up * 0.5f, Quaternion.identity);
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer.HealingEffect != null && !localPlayer.IsInstantHealing)
		{
			localPlayer.RemoveHealingEffect();
		}
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
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer.HealingEffect != null && !localPlayer.IsInstantHealing)
		{
			localPlayer.RemoveHealingEffect();
		}
	}

	public override void DoDisappear()
	{
		gameScene.AddToDeletSummoned(this);
	}

	protected override void EndDisappear()
	{
	}

	protected void DoCheckLogic()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
		float num = Vector3.Distance(localPlayer.GetTransform().position, GetTransform().position);
		bool flag = false;
		if (IsSameTeam() && localPlayer.CanRecoverHPState() && num < (float)HPRecvoerRange)
		{
			if (localPlayer.HealingEffect == null)
			{
				localPlayer.HealingEffect = EffectPlayer.GetInstance().PlayHealingEffect();
				localPlayer.IsInstantHealing = false;
			}
			if (mHPRecoverTimer.Ready())
			{
				mHPRecoverTimer.Do();
				localPlayer.RecoverHP((float)(localPlayer.MaxHp * (HPRecoverRate + HPRecoverBonus)) * 0.01f);
				flag = true;
			}
		}
		else
		{
			mHPRecoverTimer.SetTimer(1f, false);
			if (localPlayer.HealingEffect != null && !localPlayer.IsInstantHealing)
			{
				localPlayer.RemoveHealingEffect();
			}
		}
		if (IsSameTeam() && DamageReduceRate > 0 && num < (float)DamageReduceRange)
		{
			if (mDamageReduceTimer.Ready())
			{
				mDamageReduceTimer.Do();
				CharacterStateSkill characterStateSkill = new CharacterStateSkill();
				characterStateSkill.skillID = 10600;
				characterStateSkill.IsPermanent = false;
				characterStateSkill.Duration = 1f;
				characterStateSkill.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeDamageReduction;
				characterStateSkill.BuffValueY1 = (int)DamageReduceRate;
				characterStateSkill.FunctionType1 = BuffFunctionType.PropertyChange;
				characterStateSkill.IconName = DamageReduceIcon;
				characterSkillManager.AddSkill(characterStateSkill);
				characterStateSkill.StartBuff();
			}
		}
		else
		{
			mDamageReduceTimer.SetTimer(1f, false);
		}
		if (IsSameTeam() && ShieldRecoverSkillPercentage > 0 && num < (float)ShieldRecoverRange)
		{
			if (!ShieldRecoverSkillAdded)
			{
				CharacterInstantSkill characterInstantSkill = new CharacterInstantSkill();
				characterInstantSkill.skillID = 10610;
				characterInstantSkill.STriggerType = SkillTriggerType.OnKillEnemy;
				characterInstantSkill.STriggerTypeSubValue = 4;
				characterInstantSkill.SEffectType = SkillEffectType.RecoverShield;
				characterInstantSkill.CoolDownTime = 0f;
				characterInstantSkill.CoolDownTimeInit = characterInstantSkill.CoolDownTime;
				characterInstantSkill.EffectValueX = (float)(int)ShieldRecoverSkillPercentage * 0.01f;
				characterInstantSkill.EffectValueY = 0f;
				characterInstantSkill.SkillType = SkillTypes.TriggeredSkill;
				characterSkillManager.AddSkill(characterInstantSkill);
				ShieldRecoverSkillAdded = true;
			}
		}
		else if (ShieldRecoverSkillAdded)
		{
			characterSkillManager.RemoveSkillByID(10610);
			ShieldRecoverSkillAdded = false;
		}
		if (IsSameTeam() && CriticalBonusRate > 0 && num < (float)CriticalBonusRange)
		{
			if (mCriticalBonusTimer.Ready())
			{
				mCriticalBonusTimer.Do();
				CharacterStateSkill characterStateSkill2 = new CharacterStateSkill();
				characterStateSkill2.skillID = 10670;
				characterStateSkill2.IsPermanent = false;
				characterStateSkill2.Duration = 1f;
				characterStateSkill2.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill2.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeCriticalRate;
				characterStateSkill2.BuffValueY1 = (int)CriticalBonusRate;
				characterStateSkill2.FunctionType1 = BuffFunctionType.PropertyChange;
				characterStateSkill2.TargetTypes = new List<SkillTargetType>();
				characterStateSkill2.TargetTypes.Add(SkillTargetType.AllWeapon);
				characterStateSkill2.IconName = CriticalBonusIcon;
				characterSkillManager.AddSkill(characterStateSkill2);
				characterStateSkill2.StartBuff();
			}
		}
		else
		{
			mCriticalBonusTimer.SetTimer(1f, false);
		}
		if (SlowDownEnemyRate > 0 && mSlowDownEnemyTimer.Ready())
		{
			mSlowDownEnemyTimer.Do();
			if (base.IsMaster && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				CharacterStateSkill characterStateSkill3 = new CharacterStateSkill();
				characterStateSkill3.skillID = 10630;
				characterStateSkill3.IsPermanent = false;
				characterStateSkill3.Duration = 5f;
				characterStateSkill3.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill3.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeMoveSpeed;
				characterStateSkill3.BuffValueY1 = (int)SlowDownEnemyRate;
				characterStateSkill3.FunctionType1 = BuffFunctionType.PropertyChange;
				foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
				{
					if (remotePlayer != null && remotePlayer.InPlayingState() && !remotePlayer.IsSameTeam(localPlayer) && !(remotePlayer.GetObject() == null))
					{
						float num2 = Vector3.Distance(remotePlayer.GetPosition(), GetPosition());
						if (num2 < (float)SlowDownEnemyRange)
						{
							ChangeRemotePlayerStateRequest request = new ChangeRemotePlayerStateRequest(remotePlayer.GetUserID(), characterStateSkill3, 1063);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
					}
				}
			}
			else
			{
				CharacterStateSkill characterStateSkill4 = new CharacterStateSkill();
				characterStateSkill4.skillID = 10630;
				characterStateSkill4.IsPermanent = false;
				characterStateSkill4.Duration = 5f;
				characterStateSkill4.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill4.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeMoveSpeed;
				characterStateSkill4.BuffValueY1 = -SlowDownEnemyRate;
				characterStateSkill4.FunctionType1 = BuffFunctionType.PropertyChange;
				foreach (KeyValuePair<string, Enemy> enemy in GameApp.GetInstance().GetGameScene().GetEnemies())
				{
					if (enemy.Value.InPlayingState() && enemy.Value != null && !(enemy.Value.GetObject() == null))
					{
						float num3 = Vector3.Distance(enemy.Value.GetPosition(), GetPosition());
						if (num3 < (float)SlowDownEnemyRange)
						{
							CharacterSkillManager characterSkillManager2 = enemy.Value.GetCharacterSkillManager();
							characterSkillManager2.AddSkill(characterStateSkill4);
							characterStateSkill4.StartBuff();
							Debug.Log("Slow Down " + SlowDownEnemyRate + "% to " + enemy.Value.Name);
						}
					}
				}
			}
		}
		if (base.IsMaster && ExplosionDamageRate > 0 && mRangeDamageTimer.Ready())
		{
			mRangeDamageTimer.Do();
			RangeDamage();
		}
		if (!base.IsMaster || !flag)
		{
			return;
		}
		int num4 = 1;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			float num5 = Vector3.Distance(item.GetTransform().position, GetTransform().position);
			if (num5 < (float)HPRecvoerRange)
			{
				num4++;
			}
		}
		AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Miracle, AchievementTrigger.Type.Data);
		achievementTrigger.PutData(num4);
		AchievementManager.GetInstance().Trigger(achievementTrigger);
	}

	public void RangeDamage()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		Debug.Log("Explosion ! Range: " + ExplosionDamageRange + " Damage Rate: " + ExplosionDamageRate + "%");
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.criticalAttack = false;
		damageProperty.isLocal = true;
		damageProperty.wType = WeaponType.NoGun;
		damageProperty.isPenetration = false;
		damageProperty.unitLevel = base.Level;
		damageProperty.weaponLevel = base.Level;
		damageProperty.attackerType = DamageProperty.AttackerType._HealingStation;
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
						damageProperty.damage = CalculateRangeDamage(item);
						Weapon weapon = mOwnerPlayer.GetWeapon();
						damageProperty.elementType = ElementType.NoElement;
						PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(localPlayer.GetUserID(), (short)damageProperty.damage, item.GetUserID(), damageProperty.isPenetration, (byte)damageProperty.elementType, damageProperty.criticalAttack, damageProperty.isTriggerDlementDot, damageProperty.elementDotDamage, damageProperty.elementDotTime, damageProperty.wType, DamageProperty.AttackerType._HealingStation);
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
								damageProperty.damage = CalculateRangeDamage(summoned.Value);
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
			if (Physics.Raycast(ray3, out hitInfo3, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.ENEMY_HEAD)) && hitInfo3.collider.gameObject.layer != PhysicsLayer.ENEMY && hitInfo3.collider.gameObject.layer != PhysicsLayer.ENEMY_HEAD)
			{
				continue;
			}
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider.name);
				if (enemyByID != null)
				{
					damageProperty.damage = CalculateRangeDamage(enemyByID);
					damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up * 2f;
					enemyByID.HitEnemy(damageProperty);
				}
			}
		}
	}

	public int CalculateRangeDamage(GameUnit unit)
	{
		int num = 0;
		num = Mathf.CeilToInt((float)(unit.MaxShield + unit.MaxHp) * 0.03f);
		if (num >= 200)
		{
			num = 200;
		}
		if (num <= 1)
		{
			num = 1;
		}
		return num;
	}
}

using UnityEngine;

internal class EnemyStateResponse : Response
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected short mSpeedRate;

	protected EnemyStateConst mState;

	protected short mPositionX;

	protected short mPositionY;

	protected short mPositionZ;

	protected short mExtraPointX;

	protected short mExtraPointY;

	protected short mExtraPointZ;

	protected byte mData;

	protected bool hasExtraPoint()
	{
		return mState == EnemyStateConst.PATROL || mState == EnemyStateConst.GO_BACK || mState == EnemyStateConst.HUMAN_CIRCUITOUS_MOVE || mState == EnemyStateConst.HUMAN_RUN_TO_COVER || mState == EnemyStateConst.OBSIDIAN_SPAWN || mState == EnemyStateConst.OBSIDIAN_PATROL || mState == EnemyStateConst.OBSIDIAN_FLY_AROUND || mState == EnemyStateConst.OBSIDIAN_DIVE_END || mState == EnemyStateConst.GIANT_DODGE_LEFT || mState == EnemyStateConst.GIANT_DODGE_RIGHT || mState == EnemyStateConst.GHOST_DODGE_LEFT || mState == EnemyStateConst.GHOST_DODGE_RIGHT || mState == EnemyStateConst.MERCENARY_BOSS_DASH_READY || mState == EnemyStateConst.WORM_DRILL_OUT01 || mState == EnemyStateConst.WORM_DRILL_OUT02 || mState == EnemyStateConst.WORM_DRILL_OUT03 || mState == EnemyStateConst.CYBERSHOOT_DODGE || mState == EnemyStateConst.CYPHER_MOVE || mState == EnemyStateConst.TERMINATOR_MOVE || mState == EnemyStateConst.FP_FLY_TO || mState == EnemyStateConst.FC_PATROL || mState == EnemyStateConst.FLOAT_PATROL || mState == EnemyStateConst.FLOAT_GO_BACK || mState == EnemyStateConst.FLOAT_SHOT_LASER;
	}

	protected bool hasData()
	{
		return mState == EnemyStateConst.MERCENARY_BOSS_IDLE_FIRE || mState == EnemyStateConst.MERCENARY_BOSS_START_ATTCK || mState == EnemyStateConst.HUMAN_RUN_TO_COVER || mState == EnemyStateConst.HUMAN_MOVE_COVER_EXPOSE || mState == EnemyStateConst.HUMAN_MOVE_COVER_HIDE || mState == EnemyStateConst.HUMAN_LEAN_CROUCH_FIRE || mState == EnemyStateConst.HUMAN_LEAN_CROUCH || mState == EnemyStateConst.HUMAN_FULL_COVER_FIRE || mState == EnemyStateConst.HUMAN_FULL_COVER_STANDUP || mState == EnemyStateConst.HUMAN_CROUCH || mState == EnemyStateConst.HUMAN_FIRE_COVER_EXPOSE_START || mState == EnemyStateConst.TERMINATOR_MISSILE_READY || mState == EnemyStateConst.TERMINATOR_GRENADE_START || mState == EnemyStateConst.TERMINATOR_LASER || mState == EnemyStateConst.TERMINATOR_JUMP_START || mState == EnemyStateConst.FLOAT_METEOR;
	}

	protected bool NeedEndCurrentState()
	{
		return mState != EnemyStateConst.TERMINATOR_MOVE && mState != EnemyStateConst.TERMINATOR_REST && mState != EnemyStateConst.FLOAT_SHOT_LASER && mState != EnemyStateConst.FC_SHOT_MISSILE && mState != EnemyStateConst.FC_SHOT_RED_MISSILE;
	}

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mState = (EnemyStateConst)bytesBuffer.ReadByte();
		mPointID = bytesBuffer.ReadByte();
		mEnemyID = bytesBuffer.ReadByte();
		mSpeedRate = bytesBuffer.ReadShort();
		mPositionX = bytesBuffer.ReadShort();
		mPositionY = bytesBuffer.ReadShort();
		mPositionZ = bytesBuffer.ReadShort();
		if (hasExtraPoint())
		{
			mExtraPointX = bytesBuffer.ReadShort();
			mExtraPointY = bytesBuffer.ReadShort();
			mExtraPointZ = bytesBuffer.ReadShort();
		}
		if (hasData())
		{
			mData = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID("E_" + mPointID + "_" + mEnemyID);
		if (enemyByID == null || !enemyByID.InPlayingState())
		{
			return;
		}
		if (NeedEndCurrentState())
		{
			enemyByID.EndCurrentState();
		}
		Vector3 vector = new Vector3((float)mPositionX / 10f, (float)mPositionY / 10f, (float)mPositionZ / 10f);
		enemyByID.UpdatePosition(vector, mState);
		Debug.Log(string.Concat("EnemyStateResponse: ", vector, " State: ", mState));
		switch (mState)
		{
		case EnemyStateConst.IDLE:
			enemyByID.StartEnemyIdle();
			break;
		case EnemyStateConst.ATTACK:
			enemyByID.StartEnemyAttack();
			break;
		case EnemyStateConst.GO_BACK:
			enemyByID.StartGoBack(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			break;
		case EnemyStateConst.CATCHING:
			enemyByID.StartCatching();
			break;
		case EnemyStateConst.PATROL:
			enemyByID.StartPatrol(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			break;
		case EnemyStateConst.AWAKE:
			enemyByID.StartAwake();
			break;
		case EnemyStateConst.HUMAN_RUN_TO_COVER:
		{
			EnemyHuman enemyHuman14 = enemyByID as EnemyHuman;
			if (enemyHuman14 != null)
			{
				enemyHuman14.StartHumanRunToCover(mData, new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.HUMAN_MOVE:
		{
			EnemyHuman enemyHuman11 = enemyByID as EnemyHuman;
			if (enemyHuman11 != null)
			{
				enemyHuman11.StartHumanMove();
			}
			break;
		}
		case EnemyStateConst.HUMAN_STAND_FIRE:
		{
			EnemyHuman enemyHuman8 = enemyByID as EnemyHuman;
			if (enemyHuman8 != null)
			{
				enemyHuman8.StartHumanStandFire();
			}
			break;
		}
		case EnemyStateConst.HUMAN_CIRCUITOUS_MOVE:
		{
			EnemyHuman enemyHuman5 = enemyByID as EnemyHuman;
			if (enemyHuman5 != null)
			{
				enemyHuman5.StartHumanCircuitousMove(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.HUMAN_MOVE_COVER_EXPOSE:
		{
			EnemyHuman enemyHuman3 = enemyByID as EnemyHuman;
			if (enemyHuman3 != null)
			{
				enemyHuman3.StartHumanMoveCoverExpose(mData);
			}
			break;
		}
		case EnemyStateConst.HUMAN_MOVE_COVER_HIDE:
		{
			EnemyHuman enemyHuman2 = enemyByID as EnemyHuman;
			if (enemyHuman2 != null)
			{
				enemyHuman2.StartHumanMoveCoverHide(mData);
			}
			break;
		}
		case EnemyStateConst.HUMAN_LEAN_CROUCH_FIRE:
		{
			EnemyHuman enemyHuman = enemyByID as EnemyHuman;
			if (enemyHuman != null)
			{
				enemyHuman.StartHumanLeanCrouchFire(mData);
			}
			break;
		}
		case EnemyStateConst.HUMAN_LEAN_CROUCH:
		{
			EnemyHuman enemyHuman13 = enemyByID as EnemyHuman;
			if (enemyHuman13 != null)
			{
				enemyHuman13.StartHumanLeanCrouch(mData);
			}
			break;
		}
		case EnemyStateConst.HUMAN_FULL_COVER_FIRE:
		{
			EnemyHuman enemyHuman12 = enemyByID as EnemyHuman;
			if (enemyHuman12 != null)
			{
				enemyHuman12.StartHumanFullCoverFire(mData);
			}
			break;
		}
		case EnemyStateConst.HUMAN_FULL_COVER_STANDUP:
		{
			EnemyHuman enemyHuman10 = enemyByID as EnemyHuman;
			if (enemyHuman10 != null)
			{
				enemyHuman10.StartHumanFullCoverStandup(mData);
			}
			break;
		}
		case EnemyStateConst.HUMAN_CROUCH:
		{
			EnemyHuman enemyHuman9 = enemyByID as EnemyHuman;
			if (enemyHuman9 != null)
			{
				enemyHuman9.StartHumanCrouch(mData);
			}
			break;
		}
		case EnemyStateConst.HUMAN_STANDUP:
		{
			EnemyHuman enemyHuman7 = enemyByID as EnemyHuman;
			if (enemyHuman7 != null)
			{
				enemyHuman7.StartHumanStandup();
			}
			break;
		}
		case EnemyStateConst.HUMAN_RELOAD:
		{
			EnemyHuman enemyHuman6 = enemyByID as EnemyHuman;
			if (enemyHuman6 != null)
			{
				enemyHuman6.StartHumanReload();
			}
			break;
		}
		case EnemyStateConst.HUMAN_FIRE_COVER_EXPOSE_START:
		{
			EnemyHuman enemyHuman4 = enemyByID as EnemyHuman;
			if (enemyHuman4 != null)
			{
				enemyHuman4.StartHumanFireCoverExposeStart(mData);
			}
			break;
		}
		case EnemyStateConst.HATI_JUMP:
		{
			Hati hati = enemyByID as Hati;
			if (hati != null)
			{
				hati.StartHatiJump();
			}
			break;
		}
		case EnemyStateConst.OBSIDIAN_SPAWN:
		{
			Obsidian obsidian5 = enemyByID as Obsidian;
			if (obsidian5 != null)
			{
				obsidian5.StartObsidianSpawn(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.OBSIDIAN_PATROL:
		{
			Obsidian obsidian4 = enemyByID as Obsidian;
			if (obsidian4 != null)
			{
				obsidian4.StartObsidianPatrol(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.OBSIDIAN_FLY_AROUND:
		{
			Obsidian obsidian3 = enemyByID as Obsidian;
			if (obsidian3 != null)
			{
				obsidian3.StartObsidianFlyAround(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.OBSIDIAN_DIVE_START:
		{
			Obsidian obsidian2 = enemyByID as Obsidian;
			if (obsidian2 != null)
			{
				obsidian2.StartObsidianDiveStart();
			}
			break;
		}
		case EnemyStateConst.OBSIDIAN_DIVE_END:
		{
			Obsidian obsidian = enemyByID as Obsidian;
			if (obsidian != null)
			{
				obsidian.StartObsidianDiveEnd(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.GIANT_DODGE_LEFT:
		{
			Giant giant5 = enemyByID as Giant;
			if (giant5 != null)
			{
				giant5.StartGiantDodgeLeft(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.GIANT_DODGE_RIGHT:
		{
			Giant giant4 = enemyByID as Giant;
			if (giant4 != null)
			{
				giant4.StartGiantDodgeRight(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.GIANT_FIST_ATTACK:
		{
			Giant giant3 = enemyByID as Giant;
			if (giant3 != null)
			{
				giant3.StartGiantFistAttack();
			}
			break;
		}
		case EnemyStateConst.GIANT_GROUND_ATTACK:
		{
			Giant giant2 = enemyByID as Giant;
			if (giant2 != null)
			{
				giant2.StartGiantGroundAttack();
			}
			break;
		}
		case EnemyStateConst.GIANT_RUN_ATTACK:
		{
			Giant giant = enemyByID as Giant;
			if (giant != null)
			{
				giant.StartGiantRunAttack();
			}
			break;
		}
		case EnemyStateConst.GHOST_DODGE_LEFT:
		{
			Ghost ghost5 = enemyByID as Ghost;
			if (ghost5 != null)
			{
				ghost5.StartGhostDodgeLeft(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.GHOST_DODGE_RIGHT:
		{
			Ghost ghost4 = enemyByID as Ghost;
			if (ghost4 != null)
			{
				ghost4.StartGhostDodgeRight(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.GHOST_DOUBLE_ATTACK:
		{
			Ghost ghost3 = enemyByID as Ghost;
			if (ghost3 != null)
			{
				ghost3.StartGhostDoubleAttack();
			}
			break;
		}
		case EnemyStateConst.GHOST_JUMP_ATTACK:
		{
			Ghost ghost2 = enemyByID as Ghost;
			if (ghost2 != null)
			{
				ghost2.StartGhostJumpAttack();
			}
			break;
		}
		case EnemyStateConst.GHOST_STICK_ATTACK:
		{
			Ghost ghost = enemyByID as Ghost;
			if (ghost != null)
			{
				ghost.StartGhostStickAttack();
			}
			break;
		}
		case EnemyStateConst.SHELL_SINGLE_ATTACK:
		{
			Shell shell6 = enemyByID as Shell;
			if (shell6 != null)
			{
				shell6.StartShellSingleAttack();
			}
			break;
		}
		case EnemyStateConst.SHELL_DOUBLE_ATTACK:
		{
			Shell shell5 = enemyByID as Shell;
			if (shell5 != null)
			{
				shell5.StartShellDoubleAttack();
			}
			break;
		}
		case EnemyStateConst.SHELL_SPIN_START:
		{
			Shell shell4 = enemyByID as Shell;
			if (shell4 != null)
			{
				shell4.StartShellSpinStart();
			}
			break;
		}
		case EnemyStateConst.SHELL_MISSILE_START:
		{
			Shell shell3 = enemyByID as Shell;
			if (shell3 != null)
			{
				shell3.StartShellMissileStart();
			}
			break;
		}
		case EnemyStateConst.SHELL_SCREW_IN:
		{
			Shell shell2 = enemyByID as Shell;
			if (shell2 != null)
			{
				shell2.StartShellScrewIn();
			}
			break;
		}
		case EnemyStateConst.SHELL_TAUNT:
		{
			Shell shell = enemyByID as Shell;
			if (shell != null)
			{
				shell.StartShellTaunt();
			}
			break;
		}
		case EnemyStateConst.MERCENARY_BOSS_AIR_UP:
		{
			MercenaryBoss mercenaryBoss7 = enemyByID as MercenaryBoss;
			if (mercenaryBoss7 != null)
			{
				mercenaryBoss7.StartMercenaryBossAirUp();
			}
			break;
		}
		case EnemyStateConst.MERCENARY_BOSS_AIR_ATTACK:
		{
			MercenaryBoss mercenaryBoss6 = enemyByID as MercenaryBoss;
			if (mercenaryBoss6 != null)
			{
				mercenaryBoss6.StartMercenaryBossAirAttack();
			}
			break;
		}
		case EnemyStateConst.MERCENARY_BOSS_AIR_DOWN:
		{
			MercenaryBoss mercenaryBoss5 = enemyByID as MercenaryBoss;
			if (mercenaryBoss5 != null)
			{
				mercenaryBoss5.StartMercenaryBossAirDown();
			}
			break;
		}
		case EnemyStateConst.MERCENARY_BOSS_START_ATTCK:
		{
			MercenaryBoss mercenaryBoss4 = enemyByID as MercenaryBoss;
			if (mercenaryBoss4 != null)
			{
				mercenaryBoss4.StartMercenaryBossStartAttack(mData);
			}
			break;
		}
		case EnemyStateConst.MERCENARY_BOSS_DASH_READY:
		{
			MercenaryBoss mercenaryBoss3 = enemyByID as MercenaryBoss;
			if (mercenaryBoss3 != null)
			{
				mercenaryBoss3.StartMercenaryBossDashReady(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.MERCENARY_BOSS_IDLE_FIRE:
		{
			MercenaryBoss mercenaryBoss2 = enemyByID as MercenaryBoss;
			if (mercenaryBoss2 != null)
			{
				mercenaryBoss2.StartMercenaryBossFirstIdleFire(mData);
			}
			break;
		}
		case EnemyStateConst.MERCENARY_BOSS_ROCKET_READY:
		{
			MercenaryBoss mercenaryBoss = enemyByID as MercenaryBoss;
			if (mercenaryBoss != null)
			{
				mercenaryBoss.StartMercenaryBossRocketReady();
			}
			break;
		}
		case EnemyStateConst.SPIT_MELEE_ATTACK:
		{
			Spit spit2 = enemyByID as Spit;
			if (spit2 != null)
			{
				spit2.StartMeleeAttack();
			}
			break;
		}
		case EnemyStateConst.SPIT_SPIT01:
		{
			Spit spit = enemyByID as Spit;
			if (spit != null)
			{
				spit.StartSpit01();
			}
			break;
		}
		case EnemyStateConst.WORM_DRILL_OUT_TAUNT:
		{
			Worm worm8 = enemyByID as Worm;
			if (worm8 != null)
			{
				worm8.StartDrillOutTaunt();
			}
			break;
		}
		case EnemyStateConst.WORM_DRILL_OUT01:
		{
			Worm worm7 = enemyByID as Worm;
			if (worm7 != null)
			{
				worm7.StartDrillOut01(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.WORM_DRILL_OUT02:
		{
			Worm worm6 = enemyByID as Worm;
			if (worm6 != null)
			{
				worm6.StartDrillOut02(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.WORM_DRILL_OUT03:
		{
			Worm worm5 = enemyByID as Worm;
			if (worm5 != null)
			{
				worm5.StartDrillOut03(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.WORM_ATTACK_01:
		{
			Worm worm4 = enemyByID as Worm;
			if (worm4 != null)
			{
				worm4.StartAttack01();
			}
			break;
		}
		case EnemyStateConst.WORM_ATTACK_02:
		{
			Worm worm3 = enemyByID as Worm;
			if (worm3 != null)
			{
				worm3.StartAttack02();
			}
			break;
		}
		case EnemyStateConst.WORM_ATTACK_03:
		{
			Worm worm2 = enemyByID as Worm;
			if (worm2 != null)
			{
				worm2.StartAttack03();
			}
			break;
		}
		case EnemyStateConst.WORM_DRILL_IN:
		{
			Worm worm = enemyByID as Worm;
			if (worm != null)
			{
				worm.StartDrillIn();
			}
			break;
		}
		case EnemyStateConst.MONK_ATTACK_01:
		{
			Monk monk7 = enemyByID as Monk;
			if (monk7 != null)
			{
				monk7.StartEnemyAttack01();
			}
			break;
		}
		case EnemyStateConst.MONK_ATTACK_02:
		{
			Monk monk6 = enemyByID as Monk;
			if (monk6 != null)
			{
				monk6.StartEnemyAttack02();
			}
			break;
		}
		case EnemyStateConst.MONK_ATTACK_03:
		{
			Monk monk5 = enemyByID as Monk;
			if (monk5 != null)
			{
				monk5.StartEnemyAttack03();
			}
			break;
		}
		case EnemyStateConst.MONK_ATTACK_02CON:
		{
			Monk monk4 = enemyByID as Monk;
			if (monk4 != null)
			{
				monk4.StartEnemyAttack02Con();
			}
			break;
		}
		case EnemyStateConst.MONK_DENFENSE01:
		{
			Monk monk3 = enemyByID as Monk;
			if (monk3 != null)
			{
				monk3.StartDefense01();
			}
			break;
		}
		case EnemyStateConst.MONK_DENFENSE03:
		{
			Monk monk2 = enemyByID as Monk;
			if (monk2 != null)
			{
				monk2.StartDefense03();
			}
			break;
		}
		case EnemyStateConst.MONK_RUSH_ATTACK:
		{
			Monk monk = enemyByID as Monk;
			if (monk != null)
			{
				monk.StartRushAttack();
			}
			break;
		}
		case EnemyStateConst.CYBERSHOOT_JUMP:
		{
			Cybershoot cybershoot4 = enemyByID as Cybershoot;
			if (cybershoot4 != null)
			{
				cybershoot4.StartCybershootJump();
			}
			break;
		}
		case EnemyStateConst.CYBERSHOOT_DODGE:
		{
			Cybershoot cybershoot3 = enemyByID as Cybershoot;
			if (cybershoot3 != null)
			{
				cybershoot3.StartCybershootDodge(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.CYBERSHOOT_FIRE:
		{
			Cybershoot cybershoot2 = enemyByID as Cybershoot;
			if (cybershoot2 != null)
			{
				cybershoot2.StartCybershootFire();
			}
			break;
		}
		case EnemyStateConst.CYBERSHOOT_CANNON:
		{
			Cybershoot cybershoot = enemyByID as Cybershoot;
			if (cybershoot != null)
			{
				cybershoot.StartCybershootCannon();
			}
			break;
		}
		case EnemyStateConst.CYPHER_MOVE:
		{
			Cypher cypher3 = enemyByID as Cypher;
			if (cypher3 != null)
			{
				cypher3.StartCypherMove(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.CYPHER_FIRE:
		{
			Cypher cypher2 = enemyByID as Cypher;
			if (cypher2 != null)
			{
				cypher2.StartCypherFire();
			}
			break;
		}
		case EnemyStateConst.CYPHER_CANNON:
		{
			Cypher cypher = enemyByID as Cypher;
			if (cypher != null)
			{
				cypher.StartCypherCannon();
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_STAMP:
		{
			Terminator terminator8 = enemyByID as Terminator;
			if (terminator8 != null)
			{
				terminator8.StartTerminatorStamp();
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_MISSILE_READY:
		{
			Terminator terminator7 = enemyByID as Terminator;
			if (terminator7 != null)
			{
				terminator7.StartTerminatorMissileReady(mData);
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_FIRE:
		{
			Terminator terminator6 = enemyByID as Terminator;
			if (terminator6 != null)
			{
				terminator6.StartTerminatorFire();
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_GRENADE_START:
		{
			Terminator terminator5 = enemyByID as Terminator;
			if (terminator5 != null)
			{
				terminator5.StartTerminatorGrenadeStart(mData);
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_JUMP_START:
		{
			Terminator terminator4 = enemyByID as Terminator;
			if (terminator4 != null)
			{
				terminator4.StartTerminatorJumpStart(mData);
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_LASER:
		{
			Terminator terminator3 = enemyByID as Terminator;
			if (terminator3 != null)
			{
				terminator3.StartTerminatorLaser(mData);
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_MOVE:
		{
			Terminator terminator2 = enemyByID as Terminator;
			if (terminator2 != null)
			{
				terminator2.StartTerminatorMove(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.TERMINATOR_REST:
		{
			Terminator terminator = enemyByID as Terminator;
			if (terminator != null)
			{
				terminator.StartTerminatorRest();
			}
			break;
		}
		case EnemyStateConst.FP_FLY:
		{
			FloatProtector floatProtector6 = enemyByID as FloatProtector;
			if (floatProtector6 != null)
			{
				floatProtector6.StartFPFly();
			}
			break;
		}
		case EnemyStateConst.FP_FLY_ATTACK:
		{
			FloatProtector floatProtector5 = enemyByID as FloatProtector;
			if (floatProtector5 != null)
			{
				floatProtector5.StartFPFlyAttack();
			}
			break;
		}
		case EnemyStateConst.FP_FLY_BACK:
		{
			FloatProtector floatProtector4 = enemyByID as FloatProtector;
			if (floatProtector4 != null)
			{
				floatProtector4.StartFPFlyBack();
			}
			break;
		}
		case EnemyStateConst.FP_FLY_TO:
		{
			FloatProtector floatProtector3 = enemyByID as FloatProtector;
			if (floatProtector3 != null)
			{
				floatProtector3.StartFlyToTarget(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.FP_INACTIVE:
		{
			FloatProtector floatProtector2 = enemyByID as FloatProtector;
			if (floatProtector2 != null)
			{
				floatProtector2.StartFPInactive();
			}
			break;
		}
		case EnemyStateConst.FP_READY_CHANGE:
		{
			FloatProtector floatProtector = enemyByID as FloatProtector;
			if (floatProtector != null)
			{
				floatProtector.StartReadyChange();
			}
			break;
		}
		case EnemyStateConst.FC_PATROL:
		{
			FloatControler floatControler6 = enemyByID as FloatControler;
			if (floatControler6 != null)
			{
				floatControler6.StartFCPatrol(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.FC_SHOT_MISSILE:
		{
			FloatControler floatControler5 = enemyByID as FloatControler;
			if (floatControler5 != null)
			{
				floatControler5.StartShotMissile();
			}
			break;
		}
		case EnemyStateConst.FC_SHOT_RED_MISSILE:
		{
			FloatControler floatControler4 = enemyByID as FloatControler;
			if (floatControler4 != null)
			{
				floatControler4.StartShotRedMissile();
			}
			break;
		}
		case EnemyStateConst.FC_LASER_LOCK_RAGE:
		{
			FloatControler floatControler3 = enemyByID as FloatControler;
			if (floatControler3 != null)
			{
				floatControler3.StartFCLaserLockRage();
			}
			break;
		}
		case EnemyStateConst.FC_LASER_LOCK:
		{
			FloatControler floatControler2 = enemyByID as FloatControler;
			if (floatControler2 != null)
			{
				floatControler2.StartFCLaserLock();
			}
			break;
		}
		case EnemyStateConst.FC_FLY_BACK:
		{
			FloatControler floatControler = enemyByID as FloatControler;
			if (floatControler != null)
			{
				floatControler.StartFCFlyBack();
			}
			break;
		}
		case EnemyStateConst.FLOAT_PATROL:
		{
			FloatCore floatCore10 = enemyByID as FloatCore;
			if (floatCore10 != null)
			{
				floatCore10.StartFloatPatrol(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.FLOAT_CHANGE_TO_2:
		{
			FloatCore floatCore9 = enemyByID as FloatCore;
			if (floatCore9 != null)
			{
				floatCore9.StartFloatChangeTo2();
			}
			break;
		}
		case EnemyStateConst.FLOAT_CHANGE_TO_3:
		{
			FloatCore floatCore8 = enemyByID as FloatCore;
			if (floatCore8 != null)
			{
				floatCore8.StartFloatChangeTo3();
			}
			break;
		}
		case EnemyStateConst.FLOAT_SHOT_LASER:
		{
			FloatCore floatCore7 = enemyByID as FloatCore;
			if (floatCore7 != null)
			{
				floatCore7.StartShotLaser(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.FLOAT_FLY_BACK:
		{
			FloatCore floatCore6 = enemyByID as FloatCore;
			if (floatCore6 != null)
			{
				floatCore6.StartFloatFlyBack();
			}
			break;
		}
		case EnemyStateConst.FLOAT_CAUTION:
		{
			FloatCore floatCore5 = enemyByID as FloatCore;
			if (floatCore5 != null)
			{
				floatCore5.StartFloatCaution();
			}
			break;
		}
		case EnemyStateConst.FLOAT_METEOR:
		{
			FloatCore floatCore4 = enemyByID as FloatCore;
			if (floatCore4 != null)
			{
				floatCore4.StartFloatMeteor(mData);
			}
			break;
		}
		case EnemyStateConst.FLOAT_GO_BACK:
		{
			FloatCore floatCore3 = enemyByID as FloatCore;
			if (floatCore3 != null)
			{
				floatCore3.StartFloatGoBack(new Vector3((float)mExtraPointX / 10f, (float)mExtraPointY / 10f, (float)mExtraPointZ / 10f));
			}
			break;
		}
		case EnemyStateConst.FLOAT_LASER:
		{
			FloatCore floatCore2 = enemyByID as FloatCore;
			if (floatCore2 != null)
			{
				floatCore2.StartFloatLaser();
			}
			break;
		}
		case EnemyStateConst.FLOAT_CATCHING:
		{
			FloatCore floatCore = enemyByID as FloatCore;
			if (floatCore != null)
			{
				floatCore.StartFloatCatching();
			}
			break;
		}
		case EnemyStateConst.SPIT_SPIT02:
		case EnemyStateConst.SPIT_SPIT03:
			break;
		}
	}
}

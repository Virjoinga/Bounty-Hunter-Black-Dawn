using UnityEngine;

public class EnemyStateRequest : Request
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected EnemyStateConst mState;

	protected short mPositionX;

	protected short mPositionY;

	protected short mPositionZ;

	protected short mExtraPointX;

	protected short mExtraPointY;

	protected short mExtraPointZ;

	protected byte mByteData;

	protected short mShortData;

	protected short mSpeedRate;

	public EnemyStateRequest(byte pointID, byte enemyID, EnemyStateConst state, Vector3 position)
	{
		requestID = 106;
		mPointID = pointID;
		mEnemyID = enemyID;
		mState = state;
		mPositionX = (short)(position.x * 10f);
		mPositionY = (short)(position.y * 10f);
		mPositionZ = (short)(position.z * 10f);
	}

	public EnemyStateRequest(byte pointID, byte enemyID, EnemyStateConst state, Vector3 position, Vector3 extraPoint)
	{
		requestID = 106;
		mPointID = pointID;
		mEnemyID = enemyID;
		mState = state;
		mPositionX = (short)(position.x * 10f);
		mPositionY = (short)(position.y * 10f);
		mPositionZ = (short)(position.z * 10f);
		mExtraPointX = (short)(extraPoint.x * 10f);
		mExtraPointY = (short)(extraPoint.y * 10f);
		mExtraPointZ = (short)(extraPoint.z * 10f);
	}

	public EnemyStateRequest(byte pointID, byte enemyID, EnemyStateConst state, Vector3 position, byte byteData)
	{
		requestID = 106;
		mPointID = pointID;
		mEnemyID = enemyID;
		mState = state;
		mPositionX = (short)(position.x * 10f);
		mPositionY = (short)(position.y * 10f);
		mPositionZ = (short)(position.z * 10f);
		mByteData = byteData;
	}

	public EnemyStateRequest(byte pointID, byte enemyID, EnemyStateConst state, Vector3 position, Vector3 extraPoint, byte byteData)
	{
		requestID = 106;
		mPointID = pointID;
		mEnemyID = enemyID;
		mState = state;
		mPositionX = (short)(position.x * 10f);
		mPositionY = (short)(position.y * 10f);
		mPositionZ = (short)(position.z * 10f);
		mExtraPointX = (short)(extraPoint.x * 10f);
		mExtraPointY = (short)(extraPoint.y * 10f);
		mExtraPointZ = (short)(extraPoint.z * 10f);
		mByteData = byteData;
	}

	public EnemyStateRequest(byte pointID, byte enemyID, EnemyStateConst state, Vector3 position, short speedRate)
	{
		requestID = 106;
		mPointID = pointID;
		mEnemyID = enemyID;
		mState = state;
		mPositionX = (short)(position.x * 10f);
		mPositionY = (short)(position.y * 10f);
		mPositionZ = (short)(position.z * 10f);
		mSpeedRate = speedRate;
	}

	public EnemyStateRequest(byte pointID, byte enemyID, EnemyStateConst state, Vector3 position, Vector3 extraPoint, short speedRate)
	{
		requestID = 106;
		mPointID = pointID;
		mEnemyID = enemyID;
		mState = state;
		mPositionX = (short)(position.x * 10f);
		mPositionY = (short)(position.y * 10f);
		mPositionZ = (short)(position.z * 10f);
		mExtraPointX = (short)(extraPoint.x * 10f);
		mExtraPointY = (short)(extraPoint.y * 10f);
		mExtraPointZ = (short)(extraPoint.z * 10f);
		mSpeedRate = speedRate;
	}

	public EnemyStateRequest(byte pointID, byte enemyID, EnemyStateConst state, Vector3 position, Vector3 extraPoint, byte byteData, short speedRate)
	{
		requestID = 106;
		mPointID = pointID;
		mEnemyID = enemyID;
		mState = state;
		mPositionX = (short)(position.x * 10f);
		mPositionY = (short)(position.y * 10f);
		mPositionZ = (short)(position.z * 10f);
		mExtraPointX = (short)(extraPoint.x * 10f);
		mExtraPointY = (short)(extraPoint.y * 10f);
		mExtraPointZ = (short)(extraPoint.z * 10f);
		mByteData = byteData;
		mSpeedRate = speedRate;
	}

	protected bool hasExtraPoint()
	{
		return mState == EnemyStateConst.PATROL || mState == EnemyStateConst.GO_BACK || mState == EnemyStateConst.HUMAN_CIRCUITOUS_MOVE || mState == EnemyStateConst.HUMAN_RUN_TO_COVER || mState == EnemyStateConst.OBSIDIAN_SPAWN || mState == EnemyStateConst.OBSIDIAN_PATROL || mState == EnemyStateConst.OBSIDIAN_FLY_AROUND || mState == EnemyStateConst.OBSIDIAN_DIVE_END || mState == EnemyStateConst.GIANT_DODGE_LEFT || mState == EnemyStateConst.GIANT_DODGE_RIGHT || mState == EnemyStateConst.GHOST_DODGE_LEFT || mState == EnemyStateConst.GHOST_DODGE_RIGHT || mState == EnemyStateConst.MERCENARY_BOSS_DASH_READY || mState == EnemyStateConst.WORM_DRILL_OUT01 || mState == EnemyStateConst.WORM_DRILL_OUT02 || mState == EnemyStateConst.WORM_DRILL_OUT03 || mState == EnemyStateConst.CYBERSHOOT_DODGE || mState == EnemyStateConst.CYPHER_MOVE || mState == EnemyStateConst.TERMINATOR_MOVE || mState == EnemyStateConst.FP_FLY_TO || mState == EnemyStateConst.FC_PATROL || mState == EnemyStateConst.FLOAT_PATROL || mState == EnemyStateConst.FLOAT_GO_BACK || mState == EnemyStateConst.FLOAT_SHOT_LASER;
	}

	protected bool hasData()
	{
		return mState == EnemyStateConst.MERCENARY_BOSS_IDLE_FIRE || mState == EnemyStateConst.MERCENARY_BOSS_START_ATTCK || mState == EnemyStateConst.HUMAN_RUN_TO_COVER || mState == EnemyStateConst.HUMAN_MOVE_COVER_EXPOSE || mState == EnemyStateConst.HUMAN_MOVE_COVER_HIDE || mState == EnemyStateConst.HUMAN_LEAN_CROUCH_FIRE || mState == EnemyStateConst.HUMAN_LEAN_CROUCH || mState == EnemyStateConst.HUMAN_FULL_COVER_FIRE || mState == EnemyStateConst.HUMAN_FULL_COVER_STANDUP || mState == EnemyStateConst.HUMAN_CROUCH || mState == EnemyStateConst.HUMAN_FIRE_COVER_EXPOSE_START || mState == EnemyStateConst.TERMINATOR_MISSILE_READY || mState == EnemyStateConst.TERMINATOR_GRENADE_START || mState == EnemyStateConst.TERMINATOR_LASER || mState == EnemyStateConst.TERMINATOR_JUMP_START || mState == EnemyStateConst.FLOAT_METEOR;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = null;
		int num = 11;
		if (hasExtraPoint())
		{
			num += 6;
		}
		if (hasData())
		{
			num++;
		}
		bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte((byte)mState);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddByte(mEnemyID);
		bytesBuffer.AddShort(mSpeedRate);
		bytesBuffer.AddShort(mPositionX);
		bytesBuffer.AddShort(mPositionY);
		bytesBuffer.AddShort(mPositionZ);
		if (hasExtraPoint())
		{
			bytesBuffer.AddShort(mExtraPointX);
			bytesBuffer.AddShort(mExtraPointY);
			bytesBuffer.AddShort(mExtraPointZ);
		}
		if (hasData())
		{
			bytesBuffer.AddByte(mByteData);
		}
		return bytesBuffer.GetBytes();
	}
}

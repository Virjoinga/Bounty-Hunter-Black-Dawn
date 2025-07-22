using UnityEngine;

public class PlayerHitPlayerRequest : Request
{
	protected int mAttackerID;

	protected int targetPlayerID;

	protected short damage;

	protected bool mIsPenetration;

	protected byte mElementType;

	protected bool mIsCritical;

	protected bool mIsTriggerElementDot;

	protected short mElementDotDamage;

	protected short mElementDotTime;

	protected byte mWeaponType;

	protected byte mAttackerType;

	public PlayerHitPlayerRequest(int attackerID, short damage, int playerID, bool isPenetration, byte elementType, bool isCritical, bool isTriggerElementDot, float elementDotDamage, int elementDotTime, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		requestID = 115;
		mAttackerID = attackerID;
		this.damage = (short)Mathf.CeilToInt((float)damage * 0.4f);
		targetPlayerID = playerID;
		mIsPenetration = isPenetration;
		mElementType = elementType;
		mIsCritical = isCritical;
		mWeaponType = (byte)weaponType;
		mAttackerType = (byte)attackerType;
		mIsTriggerElementDot = isTriggerElementDot;
		mElementDotDamage = (short)(elementDotDamage * 10f);
		mElementDotTime = (short)elementDotTime;
	}

	public override byte[] GetBody()
	{
		byte b = 16;
		if (mIsTriggerElementDot)
		{
			b += 4;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(b);
		bytesBuffer.AddShort(damage);
		bytesBuffer.AddInt(mAttackerID);
		bytesBuffer.AddInt(targetPlayerID);
		bytesBuffer.AddBool(mIsPenetration);
		bytesBuffer.AddByte(mElementType);
		bytesBuffer.AddBool(mIsCritical);
		bytesBuffer.AddByte(mWeaponType);
		bytesBuffer.AddByte(mAttackerType);
		bytesBuffer.AddBool(mIsTriggerElementDot);
		if (mIsTriggerElementDot)
		{
			bytesBuffer.AddShort(mElementDotDamage);
			bytesBuffer.AddShort(mElementDotTime);
		}
		return bytesBuffer.GetBytes();
	}
}

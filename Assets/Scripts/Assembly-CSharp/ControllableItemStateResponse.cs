using UnityEngine;

internal class ControllableItemStateResponse : Response
{
	protected EControllableType mType;

	protected byte mId;

	protected ControllableStateConst mStateId;

	protected short mPositionX;

	protected short mPositionY;

	protected short mPositionZ;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mType = (EControllableType)bytesBuffer.ReadByte();
		mId = bytesBuffer.ReadByte();
		mStateId = (ControllableStateConst)bytesBuffer.ReadByte();
		mPositionX = bytesBuffer.ReadShort();
		mPositionY = bytesBuffer.ReadShort();
		mPositionZ = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		if (mType != 0)
		{
			return;
		}
		SummonedItem summonedByID = GameApp.GetInstance().GetGameScene().GetSummonedByID("S_" + mId);
		if (summonedByID == null)
		{
			return;
		}
		summonedByID.EndCurrentState();
		Vector3 position = new Vector3((float)mPositionX / 10f, (float)mPositionY / 10f, (float)mPositionZ / 10f);
		summonedByID.UpdatePosition(position);
		switch (mStateId)
		{
		case ControllableStateConst.ENGINEER_GUN_RUN:
		{
			EngineerGun engineerGun2 = summonedByID as EngineerGun;
			if (engineerGun2 != null)
			{
				engineerGun2.StartEngineerGunRun();
			}
			break;
		}
		case ControllableStateConst.ENGINEER_GUN_FIRE:
		{
			EngineerGun engineerGun4 = summonedByID as EngineerGun;
			if (engineerGun4 != null)
			{
				engineerGun4.StartEngineerGunFire();
			}
			break;
		}
		case ControllableStateConst.ENGINEER_GUN_CANNON:
		{
			EngineerGun engineerGun5 = summonedByID as EngineerGun;
			if (engineerGun5 != null)
			{
				engineerGun5.StartEngineerGunCannon();
			}
			break;
		}
		case ControllableStateConst.ENGINEER_GUN_RUN_FIRE:
		{
			EngineerGun engineerGun3 = summonedByID as EngineerGun;
			if (engineerGun3 != null)
			{
				engineerGun3.StartEngineerGunRunFire();
			}
			break;
		}
		case ControllableStateConst.ENGINEER_GUN_RUN_CANNON:
		{
			EngineerGun engineerGun = summonedByID as EngineerGun;
			if (engineerGun != null)
			{
				engineerGun.StartEngineerGunRunCannon();
			}
			break;
		}
		case ControllableStateConst.KNOCKED:
			summonedByID.StartKnocked();
			break;
		}
	}
}

using System.Collections.Generic;

public class GetSceneStateRequest : Request
{
	public byte cityID;

	public byte sceneID;

	public int playerHp;

	public int playerShield;

	public int playerExtraShield;

	private byte mCurrentWeaponIndex;

	private WeaponInfo[] mWeaponInfoList;

	protected ElementType elementType;

	public GetSceneStateRequest(byte cityID, byte sceneID, int hp, int shield, int extraShield, List<Weapon> weaponList, byte currentWeaponIndex, ElementType element)
	{
		requestID = 101;
		this.cityID = cityID;
		this.sceneID = sceneID;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			playerHp = hp;
		}
		else
		{
			playerHp = hp;
		}
		playerShield = shield;
		playerExtraShield = extraShield;
		mWeaponInfoList = new WeaponInfo[Global.BAG_MAX_NUM];
		for (int i = 0; i < Global.BAG_MAX_NUM; i++)
		{
			mWeaponInfoList[i] = default(WeaponInfo);
			if (i < weaponList.Count && weaponList[i] != null)
			{
				mWeaponInfoList[i].mWeaponType = weaponList[i].GetWeaponType();
				mWeaponInfoList[i].mWeaponNameNumber = weaponList[i].WeaponNameNumber;
			}
			else
			{
				mWeaponInfoList[i].mWeaponType = WeaponType.NoGun;
				mWeaponInfoList[i].mWeaponNameNumber = 0;
			}
		}
		mCurrentWeaponIndex = currentWeaponIndex;
		elementType = element;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(16 + 2 * Global.BAG_MAX_NUM);
		bytesBuffer.AddByte(cityID);
		bytesBuffer.AddByte(sceneID);
		bytesBuffer.AddInt(playerHp);
		bytesBuffer.AddInt(playerShield);
		bytesBuffer.AddInt(playerExtraShield);
		WeaponInfo[] array = mWeaponInfoList;
		for (int i = 0; i < array.Length; i++)
		{
			WeaponInfo weaponInfo = array[i];
			bytesBuffer.AddByte((byte)weaponInfo.mWeaponType);
			bytesBuffer.AddByte(weaponInfo.mWeaponNameNumber);
		}
		bytesBuffer.AddByte(mCurrentWeaponIndex);
		bytesBuffer.AddByte((byte)elementType);
		return bytesBuffer.GetBytes();
	}
}

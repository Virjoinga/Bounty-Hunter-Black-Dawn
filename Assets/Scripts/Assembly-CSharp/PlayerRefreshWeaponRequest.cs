using System.Collections.Generic;

public class PlayerRefreshWeaponRequest : Request
{
	private WeaponInfo[] mWeaponInfoList;

	private byte mCurrentWeaponIndex;

	protected ElementType elementType;

	public PlayerRefreshWeaponRequest(List<Weapon> weaponList, byte currentWeaponIndex, ElementType element)
	{
		requestID = 143;
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
		BytesBuffer bytesBuffer = new BytesBuffer(1 + 2 * Global.BAG_MAX_NUM + 1);
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

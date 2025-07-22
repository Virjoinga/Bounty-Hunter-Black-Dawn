using System.Collections.Generic;

public class PlayerInfo
{
	public int channelID;

	public string roleName;

	public byte seatID;

	public byte currentCityID;

	public byte currentSceneID;

	public byte bagIdOfWeapon;

	public byte currentElementTypeOfWeapon;

	public byte characterClass;

	public byte sex;

	public short characterLevel;

	public int hp;

	public int maxhp;

	public int shield;

	public int maxshield;

	public int extrashield;

	public byte avatarID;

	public WeaponInfo[] weapons = new WeaponInfo[Global.BAG_MAX_NUM];

	public byte[] armors = new byte[Global.DECORATION_PART_NUM];

	public List<SummonedInfo> summonedList = new List<SummonedInfo>();
}

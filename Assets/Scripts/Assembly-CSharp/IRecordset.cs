using System.IO;

public interface IRecordset
{
	void SaveGlobalData(BinaryWriter bw);

	void LoadGlobalData(BinaryReader br);

	void SaveData(BinaryWriter bw);

	void LoadData(BinaryReader br);

	UserState.RoleState LoadRoleData(BinaryReader br);
}

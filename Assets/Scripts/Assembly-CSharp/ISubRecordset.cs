using System.IO;

public interface ISubRecordset
{
	void SaveData(BinaryWriter bw);

	void LoadData(BinaryReader br);
}

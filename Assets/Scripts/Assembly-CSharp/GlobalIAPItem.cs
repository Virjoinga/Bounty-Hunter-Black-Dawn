using System.IO;

public interface GlobalIAPItem
{
	void ReadData(BinaryReader br);

	void WriteData(BinaryWriter bw);

	void Resume();
}

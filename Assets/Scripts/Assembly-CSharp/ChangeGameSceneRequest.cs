public class ChangeGameSceneRequest : Request
{
	public byte cityID;

	public byte sceneID;

	public int playerHp;

	public ChangeGameSceneRequest(byte cityID, byte sceneID)
	{
		requestID = 134;
		this.cityID = cityID;
		this.sceneID = sceneID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(cityID);
		bytesBuffer.AddByte(sceneID);
		return bytesBuffer.GetBytes();
	}
}

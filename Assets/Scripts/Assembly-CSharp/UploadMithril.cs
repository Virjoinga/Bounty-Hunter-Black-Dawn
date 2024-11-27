public class UploadMithril : Request
{
	protected BytesBuffer br;

	public UploadMithril()
	{
		requestID = 16;
		byte b = 4;
		br = new BytesBuffer(b);
		GameApp.GetInstance().GetGlobalState().WriteMithril(br);
	}

	public override byte[] GetBody()
	{
		return br.GetBytes();
	}
}

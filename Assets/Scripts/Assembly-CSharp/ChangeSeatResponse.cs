internal class ChangeSeatResponse : Response
{
	public byte success;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		success = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		UILoadingNet.m_instance.Hide();
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
	}
}

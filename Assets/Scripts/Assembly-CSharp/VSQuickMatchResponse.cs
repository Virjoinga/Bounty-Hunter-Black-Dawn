internal class VSQuickMatchResponse : Response
{
	private bool mStopMatch;

	private byte mInterval;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mStopMatch = bytesBuffer.ReadBool();
		mInterval = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		if (mStopMatch)
		{
			UIVS.instance.NotifyStopMatch();
		}
		else
		{
			UIVS.instance.NotifyMatch1v1Fail(mInterval);
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		if (mStopMatch)
		{
			robot.GetGameScene().OnStopMatch();
		}
		else
		{
			robot.GetGameScene().OnVsFail(mInterval);
		}
	}
}

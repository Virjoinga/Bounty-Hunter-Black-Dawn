using System.Collections.Generic;

internal class VSTDMCreateTargetPointInfoResponse : Response
{
	protected short mWinMaxScore;

	protected short mRedScore;

	protected short mBlueScore;

	protected byte mTargetPointCount;

	protected List<TargetPointInfoStructure> mTargetPoints = new List<TargetPointInfoStructure>();

	protected List<short> mMaxCaptureTimes = new List<short>();

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mWinMaxScore = bytesBuffer.ReadShort();
		mRedScore = bytesBuffer.ReadShort();
		mBlueScore = bytesBuffer.ReadShort();
		mTargetPointCount = bytesBuffer.ReadByte();
		for (int i = 0; i < mTargetPointCount; i++)
		{
			TargetPointInfoStructure targetPointInfoStructure = new TargetPointInfoStructure();
			targetPointInfoStructure.pointID = bytesBuffer.ReadByte();
			targetPointInfoStructure.owner = bytesBuffer.ReadByte();
			targetPointInfoStructure.capturingTime = bytesBuffer.ReadShort();
			short item = bytesBuffer.ReadShort();
			mTargetPoints.Add(targetPointInfoStructure);
			mMaxCaptureTimes.Add(item);
		}
	}

	public override void ProcessLogic()
	{
		VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
		if (vSTDMManager != null)
		{
			vSTDMManager.SetWinMaxScore(mWinMaxScore);
			vSTDMManager.RedTeam.SetScore(mRedScore);
			vSTDMManager.BlueTeam.SetScore(mBlueScore);
			vSTDMManager.pointInfo.Clear();
			for (int i = 0; i < mTargetPoints.Count; i++)
			{
				TargetPointInfo targetPointInfo = new TargetPointInfo();
				targetPointInfo.Init(mTargetPoints[i].pointID, mTargetPoints[i].owner, mTargetPoints[i].capturingTime, mMaxCaptureTimes[i]);
				int pointID = mTargetPoints[i].pointID;
				vSTDMManager.pointInfo.Add(pointID, targetPointInfo);
			}
		}
	}
}

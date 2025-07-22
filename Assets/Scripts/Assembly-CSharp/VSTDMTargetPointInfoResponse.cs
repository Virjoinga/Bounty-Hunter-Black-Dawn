using System.Collections.Generic;

internal class VSTDMTargetPointInfoResponse : Response
{
	protected short mRedScore;

	protected short mBlueScore;

	protected byte mTargetPointCount;

	protected List<TargetPointInfoStructure> mTargetPoints = new List<TargetPointInfoStructure>();

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mRedScore = bytesBuffer.ReadShort();
		mBlueScore = bytesBuffer.ReadShort();
		mTargetPointCount = bytesBuffer.ReadByte();
		for (int i = 0; i < mTargetPointCount; i++)
		{
			TargetPointInfoStructure targetPointInfoStructure = new TargetPointInfoStructure();
			targetPointInfoStructure.pointID = bytesBuffer.ReadByte();
			targetPointInfoStructure.owner = bytesBuffer.ReadByte();
			targetPointInfoStructure.capturingTime = bytesBuffer.ReadShort();
			mTargetPoints.Add(targetPointInfoStructure);
		}
	}

	public override void ProcessLogic()
	{
		VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
		if (vSTDMManager == null)
		{
			return;
		}
		vSTDMManager.RedTeam.SetScore(mRedScore);
		vSTDMManager.BlueTeam.SetScore(mBlueScore);
		for (int i = 0; i < mTargetPoints.Count; i++)
		{
			int pointID = mTargetPoints[i].pointID;
			if (vSTDMManager.pointInfo.ContainsKey(pointID))
			{
				byte owner = vSTDMManager.pointInfo[pointID].GetOwner();
				vSTDMManager.pointInfo[pointID].SetOwner(mTargetPoints[i].owner);
				vSTDMManager.pointInfo[pointID].SetTime(mTargetPoints[i].capturingTime);
			}
		}
	}
}

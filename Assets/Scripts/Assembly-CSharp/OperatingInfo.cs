using System.Collections.Generic;
using UnityEngine;

public class OperatingInfo : IInfoHandle
{
	public int mIndex = -1;

	public List<int> mInfo = new List<int>(48);

	public OperatingInfo()
	{
		Init();
	}

	public void Init()
	{
		mIndex = -1;
		mInfo.Clear();
		for (int i = 0; i < 48; i++)
		{
			mInfo.Add(0);
		}
		AddInfo(OperatingInfoType.GAIN_MITHRIL_COUNT, 10);
	}

	public byte[] WriteToBuffer()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		BytesBuffer bytesBuffer = new BytesBuffer(255);
		bytesBuffer.AddInt(mIndex);
		bytesBuffer.AddInt(int.Parse(GlobalState.version));
		bytesBuffer.AddStringShortLength(userState.GetRoleName());
		bytesBuffer.AddByte((byte)userState.GetCharLevel());
		bytesBuffer.AddInt(userState.GetCash());
		bytesBuffer.AddInt(mInfo[23] / 1000);
		bytesBuffer.AddInt(mInfo[21]);
		bytesBuffer.AddInt(mInfo[25]);
		bytesBuffer.AddInt(mInfo[24]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[43]));
		bytesBuffer.AddInt(mInfo[10]);
		bytesBuffer.AddInt(mInfo[20]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[42]));
		bytesBuffer.AddInt(mInfo[9]);
		bytesBuffer.AddInt(mInfo[19]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[34]));
		bytesBuffer.AddInt(mInfo[1]);
		bytesBuffer.AddInt(mInfo[11]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[26]));
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[35]));
		bytesBuffer.AddInt(mInfo[2]);
		bytesBuffer.AddInt(mInfo[12]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[27]));
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[36]));
		bytesBuffer.AddInt(mInfo[3]);
		bytesBuffer.AddInt(mInfo[13]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[28]));
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[37]));
		bytesBuffer.AddInt(mInfo[4]);
		bytesBuffer.AddInt(mInfo[14]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[29]));
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[38]));
		bytesBuffer.AddInt(mInfo[5]);
		bytesBuffer.AddInt(mInfo[15]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[30]));
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[39]));
		bytesBuffer.AddInt(mInfo[6]);
		bytesBuffer.AddInt(mInfo[16]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[31]));
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[40]));
		bytesBuffer.AddInt(mInfo[7]);
		bytesBuffer.AddInt(mInfo[17]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[32]));
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[41]));
		bytesBuffer.AddInt(mInfo[8]);
		bytesBuffer.AddInt(mInfo[18]);
		bytesBuffer.AddByte((byte)Mathf.Min(127, mInfo[33]));
		bytesBuffer.AddInt(mInfo[44]);
		bytesBuffer.AddByte(1);
		bytesBuffer.AddInt(GlobalState.user_id);
		bytesBuffer.AddInt(mInfo[45]);
		bytesBuffer.AddInt(mInfo[46]);
		bytesBuffer.AddInt(mInfo[47]);
		return bytesBuffer.GetBytes();
	}

	public void AfterUpload()
	{
		for (int i = 0; i < 48; i++)
		{
			if (i != 0 && i != 22 && i != 47)
			{
				mInfo[i] = 0;
			}
		}
	}

	public void AddInfo(OperatingInfoType infoType, int value)
	{
		List<int> list;
		List<int> list2 = (list = mInfo);
		int index;
		int index2 = (index = (int)infoType);
		index = list[index];
		list2[index2] = index + value;
	}
}

public class TargetPointInfo
{
	protected int mID;

	protected byte mOwner;

	protected short mTime;

	protected short mCaptureTime;

	public void Init(int id, byte owner, short time, short captureTime)
	{
		mID = id;
		mOwner = owner;
		mTime = time;
		mCaptureTime = captureTime;
	}

	public int GetPointID()
	{
		return mID;
	}

	public byte GetOwner()
	{
		return mOwner;
	}

	public void SetOwner(byte owner)
	{
		switch (owner)
		{
		case 0:
			mOwner = owner;
			break;
		case 1:
			mOwner = owner;
			break;
		case 2:
			mOwner = owner;
			break;
		}
	}

	public short GetTime()
	{
		return mTime;
	}

	public void SetTime(short time)
	{
		mTime = time;
	}

	public short GetCaptureTime()
	{
		return mCaptureTime;
	}
}

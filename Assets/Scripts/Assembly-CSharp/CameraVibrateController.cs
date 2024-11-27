using UnityEngine;

public class CameraVibrateController
{
	public enum Direction
	{
		Middle = 0,
		Left = 1,
		Right = 2,
		Twitch = 3,
		EarthQuake = 4,
		Explosion = 5
	}

	public abstract class VibrateProcess
	{
		private float mLeftTime;

		private float mOutWeight;

		private float mInWeight;

		private float mStartTime;

		private float mOutTime;

		private float mInTime;

		private Vector3 mStartOffset;

		private int twitch = 1;

		public void Start(Vector3 offset)
		{
			mStartOffset = offset;
			mLeftTime = GetLifeTime();
			mOutWeight = GetOutWeight();
			mInWeight = GetInWeight();
			mOutTime = mLeftTime * mOutWeight / (mOutWeight + mInWeight);
			mInTime = mLeftTime * mInWeight / (mOutWeight + mInWeight);
			mStartTime = Time.time;
		}

		public Vector3 Update()
		{
			if (GetTwitch())
			{
				twitch = -twitch;
			}
			if (Time.time - mStartTime <= mOutTime)
			{
				return OnUpdateOut((Time.time - mStartTime) / mOutTime) * twitch;
			}
			if (Time.time - mStartTime <= mInTime + mOutTime)
			{
				return OnUpdateIn((Time.time - mStartTime - mOutTime) / mInTime) * twitch;
			}
			return GetOriginalOffset();
		}

		public bool IsEnd()
		{
			return Time.time - mStartTime > mInTime + mOutTime;
		}

		private Vector3 OnUpdateOut(float factor)
		{
			return mStartOffset - mStartOffset * factor + GetTargetOffset() * factor;
		}

		private Vector3 OnUpdateIn(float factor)
		{
			return GetTargetOffset() - GetTargetOffset() * factor + GetOriginalOffset() * factor;
		}

		protected virtual float GetLifeTime()
		{
			return 0.3f;
		}

		protected virtual float GetOutWeight()
		{
			return 1f;
		}

		protected virtual float GetInWeight()
		{
			return 10f;
		}

		protected virtual Vector3 GetOriginalOffset()
		{
			return Vector3.zero;
		}

		protected virtual bool GetTwitch()
		{
			return false;
		}

		protected abstract Vector3 GetTargetOffset();
	}

	public class NoVibrateProcess : VibrateProcess
	{
		protected override Vector3 GetTargetOffset()
		{
			return Vector3.zero;
		}
	}

	public class MiddleVibrateProcess : VibrateProcess
	{
		private Vector3 offset = new Vector3(-2f, 0f, 0f);

		protected override Vector3 GetTargetOffset()
		{
			return offset;
		}
	}

	public class LeftVibrateProcess : VibrateProcess
	{
		private Vector3 offset = new Vector3(0f, -3f, 3f);

		protected override Vector3 GetTargetOffset()
		{
			return offset;
		}
	}

	public class RightVibrateProcess : VibrateProcess
	{
		private Vector3 offset = new Vector3(0f, 3f, -3f);

		protected override Vector3 GetTargetOffset()
		{
			return offset;
		}
	}

	public class TwitchVibrateProcess : VibrateProcess
	{
		private Vector3 offset = new Vector3(0f, 2f, 0f);

		protected override Vector3 GetTargetOffset()
		{
			return offset;
		}

		protected override float GetLifeTime()
		{
			return 0.2f;
		}

		protected override float GetOutWeight()
		{
			return 1f;
		}

		protected override float GetInWeight()
		{
			return 1f;
		}

		protected override bool GetTwitch()
		{
			return true;
		}
	}

	public class EarthQuakeVibrateProcess : VibrateProcess
	{
		private float X = 1f;

		private float Y = 1f;

		private float Z = 1f;

		protected override Vector3 GetTargetOffset()
		{
			return new Vector3(Random.Range(0f - X, X), Random.Range(0f - Y, Y), Random.Range(0f - Z, Z));
		}

		protected override float GetLifeTime()
		{
			return 0.25f;
		}

		protected override float GetOutWeight()
		{
			return 1f;
		}

		protected override float GetInWeight()
		{
			return 1000f;
		}
	}

	public class ExplosionVibrateProcess : VibrateProcess
	{
		private Vector3 startOffset;

		private float changeOffsetTime;

		public ExplosionVibrateProcess()
		{
			startOffset = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
			changeOffsetTime = Time.time;
		}

		protected override Vector3 GetTargetOffset()
		{
			if (Time.time - changeOffsetTime > 0.01f)
			{
				startOffset = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3)) * 0.95f;
				changeOffsetTime = Time.time;
			}
			return startOffset;
		}

		protected override float GetLifeTime()
		{
			return 1f;
		}

		protected override float GetOutWeight()
		{
			return 1f;
		}

		protected override float GetInWeight()
		{
			return 100f;
		}
	}

	private Vector3 cameraRotation = Vector3.zero;

	private VibrateProcess mVibrateProcess;

	private bool isCanVibrate = true;

	public Vector3 CameraRotation
	{
		get
		{
			return cameraRotation;
		}
		set
		{
			cameraRotation = value;
		}
	}

	public void Init()
	{
		Start(new NoVibrateProcess());
	}

	public void Process()
	{
		cameraRotation = mVibrateProcess.Update();
		if (!isCanVibrate && mVibrateProcess.IsEnd())
		{
			isCanVibrate = true;
		}
	}

	public bool Vibrate(Direction dir)
	{
		if (isCanVibrate)
		{
			switch (dir)
			{
			case Direction.Middle:
				Start(new MiddleVibrateProcess());
				return true;
			case Direction.Left:
				Start(new LeftVibrateProcess());
				return true;
			case Direction.Right:
				Start(new RightVibrateProcess());
				return true;
			case Direction.Twitch:
				Start(new TwitchVibrateProcess());
				return true;
			case Direction.EarthQuake:
				Start(new EarthQuakeVibrateProcess());
				return true;
			case Direction.Explosion:
				Start(new ExplosionVibrateProcess());
				return true;
			default:
				return false;
			}
		}
		return false;
	}

	public bool VibrateUntilEnd(Direction dir)
	{
		bool flag = Vibrate(dir);
		if (flag)
		{
			isCanVibrate = false;
		}
		return flag;
	}

	private void Start(VibrateProcess process)
	{
		mVibrateProcess = process;
		mVibrateProcess.Start(cameraRotation);
	}
}

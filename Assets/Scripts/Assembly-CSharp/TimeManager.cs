using System;
using UnityEngine;

public class TimeManager
{
	public enum EPingType
	{
		LOOP = 0,
		ONCE = 1
	}

	private int mSendTimes;

	private int mReceiveTimes;

	private int mMaxSendTimes;

	private float mPeriod;

	private static TimeManager instance;

	public short Ping;

	public int AdjustTime;

	protected float[] fivePings;

	private float lastRequestTime;

	private bool synchronized;

	private float lastServerTime;

	private float lastLocalTime;

	private bool running = true;

	private float averagePing;

	private int pingCount;

	private readonly int averagePingCount = 10;

	private float[] pingValues;

	private DateTime[] timeBeforeSyncArray;

	private int pingValueIndex;

	private int currentPingId;

	private int nextPingId;

	private EPingType mType;

	private Timer pingSendTimer = new Timer();

	private Timer synNetworkTimeTimer;

	private int synTimeCount;

	private bool quickSynTimeDone;

	public float LastSynTime;

	public int InterplolationBackTime = 450;

	public bool resetClock;

	protected bool synStart;

	public bool NetworkTimeSynchronized
	{
		get
		{
			return quickSynTimeDone;
		}
	}

	public int NetworkTime
	{
		get
		{
			return (int)((Time.time - lastLocalTime) * 1000f + lastServerTime) + AdjustTime;
		}
	}

	public int AveragePing
	{
		get
		{
			return (int)averagePing;
		}
	}

	public static TimeManager GetInstance()
	{
		if (instance == null)
		{
			instance = new TimeManager();
		}
		return instance;
	}

	public void Init()
	{
		pingValues = new float[averagePingCount];
		fivePings = new float[5];
		synStart = false;
		pingCount = 0;
		pingValueIndex = 0;
		running = true;
		mReceiveTimes = 0;
		mSendTimes = 0;
		synNetworkTimeTimer = null;
		pingSendTimer.SetTimer(0.3f, false);
	}

	public void InitNetworkTimer()
	{
		synNetworkTimeTimer = new Timer();
		synNetworkTimeTimer.SetTimer(0.3f, true);
		quickSynTimeDone = false;
		synTimeCount = 0;
	}

	public void SynStop()
	{
		synStart = false;
	}

	public void SynStart()
	{
		synStart = true;
	}

	public void setMaxLoopTimes(int times)
	{
		mMaxSendTimes = times;
		if (mMaxSendTimes > -1)
		{
			mType = EPingType.ONCE;
		}
		else
		{
			mType = EPingType.LOOP;
		}
	}

	public void setPeriod(float period)
	{
		mPeriod = period;
	}

	public void Synchronize(int timeValue, short id, long timeAtSend)
	{
		DateTime now = DateTime.Now;
		long currentTimeLong = GetCurrentTimeLong();
		float num = currentTimeLong - timeAtSend;
		Ping = (short)num;
		if (synStart)
		{
			if (synNetworkTimeTimer == null)
			{
				InitNetworkTimer();
			}
			if (synNetworkTimeTimer.Ready() || resetClock)
			{
				synTimeCount++;
				if (synTimeCount < 5)
				{
					fivePings[synTimeCount] = num;
				}
				if (synTimeCount >= 5 && !quickSynTimeDone)
				{
					synNetworkTimeTimer.SetTimer(20f, true);
					quickSynTimeDone = true;
					pingSendTimer.SetTimer(1f, false);
					float num2 = 0f;
					float num3 = 100000f;
					int num4 = 0;
					int num5 = 0;
					float num6 = 0f;
					for (int i = 0; i < 5; i++)
					{
						if (fivePings[i] > num2)
						{
							num2 = fivePings[i];
							num4 = i;
						}
						if (fivePings[i] < num3)
						{
							num3 = fivePings[i];
							num5 = i;
						}
					}
					int num7 = 0;
					for (int j = 0; j < 5; j++)
					{
						if (j != num4 && j != num5)
						{
							num6 += fivePings[j];
							num7++;
						}
					}
					float num8 = num6 / (float)num7;
					lastServerTime = (float)timeValue + num8 / 2f;
					lastLocalTime = Time.time;
					InterplolationBackTime = Mathf.Max(300, (int)(num8 + 200f));
				}
				if (synTimeCount > 5)
				{
					float num9 = (float)AveragePing / 2f;
					lastServerTime = (float)timeValue + num9;
					lastLocalTime = Time.time;
					InterplolationBackTime = Mathf.Max(300, GetInstance().AveragePing + 200);
				}
				synNetworkTimeTimer.Do();
				resetClock = false;
			}
		}
		else
		{
			synNetworkTimeTimer = null;
		}
		CalculateAveragePing(num);
		LastSynTime = Time.time;
	}

	public void ResyncClock()
	{
		resetClock = true;
	}

	public long GetCurrentTimeLong()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		return (long)timeSpan.TotalMilliseconds;
	}

	public bool Loop()
	{
		if (pingSendTimer.Ready())
		{
			DateTime now = DateTime.Now;
			long currentTimeLong = GetCurrentTimeLong();
			TimeSynchronizeRequest request = new TimeSynchronizeRequest(0, currentTimeLong);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			pingSendTimer.Do();
		}
		if (Time.time - LastSynTime > 30f)
		{
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			if (networkManager != null)
			{
				networkManager.CloseConnection();
			}
		}
		return true;
	}

	public bool RobotLoop(NetworkManager networkMgr)
	{
		if (pingSendTimer.Ready())
		{
			DateTime now = DateTime.Now;
			long currentTimeLong = GetCurrentTimeLong();
			TimeSynchronizeRequest request = new TimeSynchronizeRequest(0, currentTimeLong);
			networkMgr.SendRequest(request);
			pingSendTimer.Do();
		}
		return false;
	}

	private void CalculateAveragePing(float ping)
	{
		pingValues[pingValueIndex] = ping;
		pingValueIndex++;
		if (pingValueIndex >= averagePingCount)
		{
			pingValueIndex = 0;
		}
		if (pingCount < averagePingCount)
		{
			pingCount++;
		}
		float num = 0f;
		for (int i = 0; i < pingCount; i++)
		{
			num += pingValues[i];
		}
		averagePing = num / (float)pingCount;
	}
}

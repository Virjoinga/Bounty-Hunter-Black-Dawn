using System;
using System.IO;
using UnityEngine;

public class Achievement
{
	private string mName;

	private string mInfo;

	private byte mIcon;

	private int[] mReward;

	protected int[] mTargetNum;

	protected int[] mAdditionalPart;

	private bool active;

	private AchievementID id;

	private bool finish;

	public string Name
	{
		get
		{
			return mName;
		}
	}

	public string Info
	{
		get
		{
			return mInfo;
		}
	}

	public byte Icon
	{
		get
		{
			return mIcon;
		}
	}

	public bool Finish
	{
		get
		{
			return finish;
		}
	}

	public bool Active
	{
		get
		{
			return active;
		}
	}

	public AchievementID ID
	{
		get
		{
			return id;
		}
	}

	public Achievement(int id, string name, string info, byte icon, string reward, string targetNum, string additionalPart, bool active)
	{
		mName = name;
		mInfo = info;
		mIcon = icon;
		mReward = SplitStringToInt32(reward);
		mTargetNum = SplitStringToInt32(targetNum);
		mAdditionalPart = SplitStringToInt32(additionalPart);
		this.active = active;
		this.id = (AchievementID)id;
		finish = false;
		if (mTargetNum.Length != mAdditionalPart.Length)
		{
			Debug.Log("Achievement " + name + " ERROE.");
		}
	}

	private int[] SplitStringToInt32(string str)
	{
		string[] array = SplitString(str);
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = Convert.ToInt32(array[i]);
		}
		return array2;
	}

	private byte[] SplitStringToByte(string str)
	{
		string[] array = SplitString(str);
		byte[] array2 = new byte[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = Convert.ToByte(array[i]);
		}
		return array2;
	}

	private string[] SplitString(string str)
	{
		string[] separator = new string[1] { "|" };
		int num = 0;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == '|')
			{
				num++;
			}
		}
		return str.Split(separator, num + 1, StringSplitOptions.None);
	}

	public void Trigger(AchievementTrigger trigger)
	{
		if (active && trigger.ID == id)
		{
			switch (trigger.TriggerType)
			{
			case AchievementTrigger.Type.Start:
				OnStart();
				break;
			case AchievementTrigger.Type.Stop:
				OnStop();
				break;
			case AchievementTrigger.Type.Reset:
				OnReset();
				break;
			case AchievementTrigger.Type.Data:
				OnDataChange(trigger.Data, trigger.AdditionalPart);
				break;
			case AchievementTrigger.Type.Report:
				ReportAchievement();
				break;
			}
		}
	}

	protected void DoFinish()
	{
		Debug.Log(string.Concat("Achievement ", id, " finish."));
		finish = true;
	}

	protected void ReportAchievement()
	{
		int num = (int)(id + 1);
		string text = ((num >= 10) ? (string.Empty + num) : ("0" + num));
		string text2 = "com.ifreyr.bo01.achievement_" + text;
	}

	protected virtual void OnDataChange(int data, int additionalPart)
	{
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(finish);
		OnSave(bw);
	}

	public void Load(BinaryReader br)
	{
		finish = br.ReadBoolean();
		OnLoad(br);
	}

	protected virtual void OnSave(BinaryWriter bw)
	{
	}

	protected virtual void OnLoad(BinaryReader br)
	{
	}

	protected virtual void OnStart()
	{
	}

	protected virtual void OnStop()
	{
	}

	protected virtual void OnReset()
	{
	}

	protected virtual float GetAchievementPercent()
	{
		return 0f;
	}
}

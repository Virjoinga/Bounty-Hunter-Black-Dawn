using System.IO;
using UnityEngine;

public class AccumulateAchievement : Achievement
{
	private int[] currNum;

	public AccumulateAchievement(int id, string nameEN, string infoEN, byte icon, string reward, string targetNum, string additionalPart, bool active)
		: base(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active)
	{
		currNum = new int[mTargetNum.Length];
	}

	protected override void OnDataChange(int data, int additionalPart)
	{
		int num = mTargetNum.Length;
		for (int i = 0; i < num; i++)
		{
			if (mAdditionalPart[i] == 0 || mAdditionalPart[i] == additionalPart)
			{
				currNum[i] += data;
			}
		}
		bool flag = true;
		for (int j = 0; j < num; j++)
		{
			if (currNum[j] < mTargetNum[j])
			{
				flag = false;
			}
		}
		if (flag)
		{
			DoFinish();
		}
	}

	protected override void OnSave(BinaryWriter bw)
	{
		bw.Write(currNum.Length);
		int[] array = currNum;
		foreach (int value in array)
		{
			bw.Write(value);
		}
	}

	protected override void OnLoad(BinaryReader br)
	{
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			currNum[i] = br.ReadInt32();
		}
	}

	protected override float GetAchievementPercent()
	{
		float num = 0f;
		int num2 = mTargetNum.Length;
		for (int i = 0; i < num2; i++)
		{
			if (mTargetNum[i] != 0)
			{
				num += Mathf.Min((float)currNum[i] * 100f / (float)mTargetNum[i], 100f);
			}
		}
		float num3 = 0f;
		if (num2 > 0)
		{
			num3 = num / (float)num2;
		}
		return (!base.Finish) ? num3 : 100f;
	}
}

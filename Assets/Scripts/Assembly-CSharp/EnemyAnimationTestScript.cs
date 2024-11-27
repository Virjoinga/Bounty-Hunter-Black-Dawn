using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTestScript : MonoBehaviour
{
	public int LoopTimes = 1;

	public string AnimationName;

	private List<string> mClipNames;

	private int mCurrentId;

	private int mMaxCount;

	private void Start()
	{
		mClipNames = new List<string>();
		foreach (AnimationState item in base.GetComponent<Animation>())
		{
			if (item.clip.name != "Take 001")
			{
				item.wrapMode = WrapMode.Loop;
				mClipNames.Add(item.clip.name);
			}
		}
		mCurrentId = 0;
		mMaxCount = mClipNames.Count;
		base.GetComponent<Animation>().Play(mClipNames[mCurrentId]);
	}

	private void Update()
	{
		if (mCurrentId < mMaxCount && base.GetComponent<Animation>().IsPlaying(mClipNames[mCurrentId]) && base.GetComponent<Animation>()[mClipNames[mCurrentId]].time >= base.GetComponent<Animation>()[mClipNames[mCurrentId]].clip.length * (float)LoopTimes)
		{
			mCurrentId++;
			if (mCurrentId < mMaxCount)
			{
				base.GetComponent<Animation>().Play(mClipNames[mCurrentId]);
				AnimationName = mClipNames[mCurrentId];
			}
			else
			{
				base.GetComponent<Animation>().Stop();
			}
		}
	}
}

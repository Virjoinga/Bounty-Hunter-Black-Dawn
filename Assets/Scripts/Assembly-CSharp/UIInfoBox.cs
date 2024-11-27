using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIInfoBox : MonoBehaviour
{
	public float Height = 40f;

	public float Speed = 0.5f;

	private UIPanel mUIPanel;

	private float mLeft;

	private float mRight;

	private float mTop;

	private float mBottom;

	private List<string> mStrList;

	private UIInfoBoxContent[] mUIInfoBoxContents;

	private List<UIInfoBoxContent> mInfoList;

	private int mMaxCount;

	public float Left
	{
		get
		{
			return mLeft;
		}
	}

	public float Right
	{
		get
		{
			return mRight;
		}
	}

	public float Top
	{
		get
		{
			return mTop;
		}
	}

	public float Bottom
	{
		get
		{
			return mBottom;
		}
	}

	private void Awake()
	{
		mUIPanel = GetComponent<UIPanel>();
		if (mUIPanel != null)
		{
			mLeft = mUIPanel.clipRange.x - mUIPanel.clipRange.z / 2f;
			mRight = mUIPanel.clipRange.x + mUIPanel.clipRange.z / 2f;
			mTop = mUIPanel.clipRange.y + mUIPanel.clipRange.w / 2f;
			mBottom = mUIPanel.clipRange.y - mUIPanel.clipRange.w / 2f;
		}
		else
		{
			mLeft = (mRight = (mTop = (mBottom = 0f)));
		}
		mUIInfoBoxContents = GetComponentsInChildren<UIInfoBoxContent>();
		mStrList = new List<string>();
		mInfoList = new List<UIInfoBoxContent>();
		mMaxCount = mUIInfoBoxContents.Length;
	}

	private void OnEnable()
	{
		UIInfoBoxContent[] array = mUIInfoBoxContents;
		foreach (UIInfoBoxContent uIInfoBoxContent in array)
		{
			uIInfoBoxContent.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (mStrList.Count > 0 && mInfoList.Count < mMaxCount)
		{
			UIInfoBoxContent[] array = mUIInfoBoxContents;
			foreach (UIInfoBoxContent uIInfoBoxContent in array)
			{
				if (!uIInfoBoxContent.gameObject.activeSelf)
				{
					Debug.Log("mStrList[0] : " + mStrList[0]);
					uIInfoBoxContent.SetInfo(mStrList[0]);
					uIInfoBoxContent.transform.localPosition = new Vector3(uIInfoBoxContent.transform.localPosition.x, Top - (float)mInfoList.Count * Height, uIInfoBoxContent.transform.localPosition.z);
					uIInfoBoxContent.gameObject.SetActive(true);
					mStrList.RemoveAt(0);
					mInfoList.Add(uIInfoBoxContent);
					break;
				}
			}
		}
		if (mInfoList.Count <= 0)
		{
			return;
		}
		List<UIInfoBoxContent> list = new List<UIInfoBoxContent>();
		int num = 0;
		if (mInfoList.Count == mMaxCount)
		{
			num = 1;
		}
		for (int j = 0; j < mInfoList.Count; j++)
		{
			if (mInfoList[j].gameObject.activeSelf)
			{
				if (Top - (float)(j - num) * Height <= mInfoList[j].transform.localPosition.y)
				{
					mInfoList[j].transform.localPosition = new Vector3(mInfoList[j].transform.localPosition.x, Top - (float)(j - num) * Height, mInfoList[j].transform.localPosition.z);
					if (mInfoList[j].transform.localPosition.y == Top + Height)
					{
						mInfoList[j].gameObject.SetActive(false);
						list.Add(mInfoList[j]);
					}
				}
				else
				{
					mInfoList[j].transform.localPosition += new Vector3(0f, 1f, 0f);
				}
			}
			else
			{
				list.Add(mInfoList[j]);
			}
		}
		foreach (UIInfoBoxContent item in list)
		{
			mInfoList.Remove(item);
		}
		list.Clear();
	}

	public void AddInfo(string str)
	{
		mStrList.Add(str);
	}
}

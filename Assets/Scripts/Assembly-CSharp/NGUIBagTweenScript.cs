using UnityEngine;

public class NGUIBagTweenScript : UIDelegateMenu
{
	public GameObject Target;

	public GameObject PrePage;

	public GameObject NextPage;

	public Vector3 TweenVector;

	private string mPrePageName;

	private string mNextPageName;

	private bool mInit;

	private int mIndex;

	private int mMaxIndex;

	private bool mButtonEnable = true;

	public void Start()
	{
		mInit = false;
		mIndex = 0;
		mButtonEnable = true;
		if (Target != null && PrePage != null && NextPage != null)
		{
			AddDelegate(PrePage, out mPrePageName);
			AddDelegate(NextPage, out mNextPageName);
		}
	}

	public void Update()
	{
		if (!mInit)
		{
			mInit = true;
			DisableUnvisibleItem();
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (mButtonEnable)
		{
			if (IsThisObject(go, mPrePageName))
			{
				Play(true);
			}
			else if (IsThisObject(go, mNextPageName))
			{
				Play(false);
			}
		}
	}

	public void Play(bool forward)
	{
		if (!(Target != null))
		{
			return;
		}
		bool flag = false;
		Vector3 localPosition = Target.transform.localPosition;
		if (forward)
		{
			if (mIndex > 0)
			{
				localPosition += TweenVector;
				mIndex--;
				flag = true;
			}
		}
		else if (mIndex < mMaxIndex)
		{
			localPosition -= TweenVector;
			mIndex++;
			flag = true;
		}
		if (flag)
		{
			DisableUnvisibleItem();
			mButtonEnable = false;
			TweenPosition.Begin(Target, 1f, localPosition);
			TweenPosition component = Target.GetComponent<TweenPosition>();
			component.eventReceiver = base.gameObject;
			component.callWhenFinished = "UnlockButton";
		}
	}

	public void DisableUnvisibleItem()
	{
		int childCount = Target.transform.GetChild(0).GetChildCount();
		mMaxIndex = Mathf.CeilToInt((float)childCount / 5f) - 2;
		for (int i = 0; i < childCount; i++)
		{
			if (i < mIndex * 5 || i > mIndex * 5 + 9)
			{
				Transform child = Target.transform.GetChild(0).GetChild(i);
				if (child != null && child.GetComponent<Collider>() != null)
				{
					child.GetComponent<Collider>().enabled = false;
					QualityEffectScript componentInChildren = child.GetComponentInChildren<QualityEffectScript>();
					if (componentInChildren != null)
					{
						componentInChildren.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				Transform child2 = Target.transform.GetChild(0).GetChild(i);
				if (child2 != null && child2.GetComponent<Collider>() != null)
				{
					child2.GetComponent<Collider>().enabled = true;
				}
			}
		}
	}

	public void UnlockButton()
	{
		mButtonEnable = true;
		int childCount = Target.transform.GetChild(0).GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			if (i < mIndex * 5 || i > mIndex * 5 + 9)
			{
				continue;
			}
			Transform child = Target.transform.GetChild(0).GetChild(i);
			if (!(child != null) || !(child.GetComponent<Collider>() != null))
			{
				continue;
			}
			for (int j = 0; j < child.GetChildCount(); j++)
			{
				if (child.GetChild(j).tag == TagName.QUALITY_EFFECT)
				{
					child.GetChild(j).gameObject.SetActive(true);
				}
			}
		}
	}

	public bool IsButtonEnable()
	{
		return mButtonEnable;
	}
}

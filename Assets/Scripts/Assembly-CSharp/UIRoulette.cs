using System;
using UnityEngine;

public class UIRoulette : MonoBehaviour
{
	public Transform m_Center;

	public float radius = 100f;

	public float angleToBeStatic = 90f;

	public float sensitivity = 1f;

	public float friction = -0.1f;

	public GameObject m_IconContainer;

	public GameObject eventReceiver;

	private Transform[] icons;

	private float force;

	private FingerTrick fingerTrick;

	private bool bRotate;

	private int times;

	private bool isFirstTouch;

	private UIRouletteListener mUIRouletteListener;

	private float mCenterX;

	private float mCenterY;

	public int Index
	{
		get
		{
			if (DeltaRad == 0f)
			{
				return -1;
			}
			return (int)(base.transform.eulerAngles.z / DeltaRad);
		}
	}

	public float CenterX
	{
		get
		{
			return mCenterX;
		}
	}

	public float CenterY
	{
		get
		{
			return mCenterY;
		}
	}

	private float DeltaRad
	{
		get
		{
			if (icons == null)
			{
				return 0f;
			}
			return 360 / icons.Length;
		}
	}

	private int RadNumber
	{
		get
		{
			if (icons == null)
			{
				return 0;
			}
			return icons.Length;
		}
	}

	private float Dir
	{
		get
		{
			if (force == 0f)
			{
				return 1f;
			}
			return (int)Mathf.Sign(force);
		}
	}

	private int NextIndex
	{
		get
		{
			return GambleManagerAbandoned.GetInstance().GetRandomItemId();
		}
	}

	private void Awake()
	{
		InitData();
		InitIcon();
	}

	private void InitData()
	{
		mCenterX = m_Center.localPosition.x + (float)(Screen.width / 2);
		Transform parent = m_Center.parent;
		while (parent != null)
		{
			mCenterX += parent.localPosition.x;
			parent = parent.parent;
		}
		mCenterY = m_Center.localPosition.y + (float)(Screen.height / 2);
		parent = m_Center.parent;
		while (parent != null)
		{
			mCenterY += parent.localPosition.y;
			parent = parent.parent;
		}
		angleToBeStatic %= 360f;
		angleToBeStatic += 360f;
		angleToBeStatic %= 360f;
		if (radius < 0f)
		{
			radius = 0f - radius;
		}
		float num = radius * 1.5f;
		float x = CenterX - num;
		float y = CenterY - num;
		fingerTrick = new FingerTrick(range: new Vector4(x, y, num * 2f, num * 2f), callBack: base.gameObject);
		bRotate = false;
		isFirstTouch = true;
	}

	private void InitIcon()
	{
		Transform[] componentsInChildren = m_IconContainer.GetComponentsInChildren<Transform>();
		int num = componentsInChildren.Length - 1;
		if (num < 2)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		icons = new Transform[num];
		m_IconContainer.transform.localPosition = new Vector3(m_Center.localPosition.x, m_Center.localPosition.y, m_IconContainer.transform.localPosition.z);
		for (int i = 0; i < num; i++)
		{
			icons[i] = componentsInChildren[i + 1];
			float f = (float)Math.PI * (angleToBeStatic - (float)i * DeltaRad) / 180f;
			float x = radius * Mathf.Cos(f);
			float y = radius * Mathf.Sin(f);
			float z = 0f;
			icons[i].transform.localPosition = new Vector3(x, y, z);
			icons[i].transform.localEulerAngles = new Vector3(0f, 0f, 360f - (float)i * DeltaRad);
		}
	}

	public void SetListener(UIRouletteListener _UIRouletteListener)
	{
		mUIRouletteListener = _UIRouletteListener;
	}

	private void Update()
	{
		fingerTrick.Update();
		UpdateRoulette();
	}

	private void UpdateRoulette()
	{
		float z = force * sensitivity;
		base.transform.eulerAngles += new Vector3(0f, 0f, z);
		if (!bRotate)
		{
			return;
		}
		if (times > 0)
		{
			times--;
		}
		else
		{
			force = Mathf.Max(0f, Mathf.Abs(force) + friction) * Dir;
		}
		if (force == 0f)
		{
			bRotate = false;
			int num = Mathf.RoundToInt(base.transform.eulerAngles.z / DeltaRad) * Mathf.RoundToInt(DeltaRad);
			base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, num);
			if (mUIRouletteListener != null)
			{
				mUIRouletteListener.OnRouletteStop(Index);
			}
			isFirstTouch = true;
		}
	}

	private void OnFingerDrag2(Vector4 pos)
	{
		if (bRotate)
		{
			return;
		}
		bool flag = true;
		if (mUIRouletteListener != null)
		{
			flag = mUIRouletteListener.IsRouletteCanBeTouchInThisPos(new Vector2(pos.x, pos.y));
		}
		if (!flag)
		{
			return;
		}
		float f = pos.x - CenterX;
		float f2 = pos.y - CenterY;
		force = pos.w * Mathf.Sign(f) - pos.z * Mathf.Sign(f2);
		force = Mathf.Clamp(force, -40f, 0f);
		force = ((!(force < -5f)) ? 0f : force);
		if (isFirstTouch)
		{
			if (mUIRouletteListener != null)
			{
				mUIRouletteListener.OnRouletteFirstTouchAfterRotation();
			}
			isFirstTouch = false;
		}
	}

	private void OnFingerRelease()
	{
		Debug.Log("OnFingerRelease : " + force);
		if (!bRotate && Mathf.Abs(force) != 0f)
		{
			bRotate = true;
			force = Mathf.Max(1f, Mathf.Abs(force)) * Dir;
			int num = Mathf.Abs((int)(force / friction));
			force = (0f - friction) * (float)num * Dir;
			float num2 = force * (float)num + (float)(num * (num - 1)) * friction * Dir / 2f;
			int num3 = (int)((float)NextIndex * DeltaRad);
			int num4 = (int)base.transform.eulerAngles.z;
			int num5 = ((int)((float)num4 + num2 * sensitivity) % 360 + 360) % 360;
			int num6 = ((Dir != 1f) ? (num5 - num3 + 360) : (num3 - num5 + 360));
			times = Mathf.Abs((int)((float)num6 / (force * sensitivity)));
			float z = ((float)num6 - Mathf.Abs(force * (float)times * sensitivity)) * Dir;
			base.transform.eulerAngles += new Vector3(0f, 0f, z);
			if (mUIRouletteListener != null)
			{
				mUIRouletteListener.OnRouletteStart();
			}
		}
	}
}

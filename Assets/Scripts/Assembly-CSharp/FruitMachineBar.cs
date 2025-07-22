using UnityEngine;

public class FruitMachineBar : MonoBehaviour
{
	public GameObject bar;

	public Transform barTop;

	public Transform barBottom;

	private SpringSimulate mSpringSimulate = new SpringSimulate();

	private float mYTop
	{
		get
		{
			return barTop.localPosition.y;
		}
	}

	private float mYBottom
	{
		get
		{
			return barBottom.localPosition.y;
		}
	}

	private float mBarLength
	{
		get
		{
			return mYTop - mYBottom;
		}
	}

	private float mY
	{
		get
		{
			return bar.transform.localPosition.y;
		}
		set
		{
			bar.transform.localPosition = new Vector3(bar.transform.localPosition.x, value, bar.transform.localPosition.z);
		}
	}

	private void Awake()
	{
		mY = barTop.localPosition.y;
	}

	private void Update()
	{
		mY = mYTop - mBarLength * mSpringSimulate.Update();
	}

	public void PressY(float y)
	{
		y = Mathf.Clamp(y, mYBottom, mYTop);
		mY = y;
		mSpringSimulate.Stretch((mYTop - y) / mBarLength);
	}

	public void PressOffsetY(float y)
	{
		PressY(mY + y);
	}

	public bool Release()
	{
		return mSpringSimulate.Release();
	}

	public bool IsMax()
	{
		return mSpringSimulate.IsMax();
	}
}

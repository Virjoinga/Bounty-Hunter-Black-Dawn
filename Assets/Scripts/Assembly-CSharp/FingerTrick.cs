using UnityEngine;

public class FingerTrick
{
	private GameObject mCallBack;

	private Vector4 mRange;

	private bool b2ndTouch;

	private float lastDistance;

	public FingerTrick(GameObject callBack, Vector4 range)
	{
		mCallBack = callBack;
		mRange = range;
		b2ndTouch = false;
		lastDistance = 0f;
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			mCallBack.SendMessage("OnFingerPress", SendMessageOptions.DontRequireReceiver);
		}
		if (Input.GetMouseButtonUp(0))
		{
			mCallBack.SendMessage("OnFingerRelease", SendMessageOptions.DontRequireReceiver);
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			if (Input.touchCount == 1)
			{
				b2ndTouch = false;
				Touch touch = Input.GetTouch(0);
				if (IsTouchIn(touch.position) && mCallBack != null)
				{
					mCallBack.SendMessage("OnFingerDrag", touch.deltaPosition, SendMessageOptions.DontRequireReceiver);
					mCallBack.SendMessage("OnFingerDrag2", new Vector4(touch.position.x, touch.position.y, touch.deltaPosition.x, touch.deltaPosition.y), SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				if (Input.touchCount != 2)
				{
					return;
				}
				Touch touch2 = Input.GetTouch(0);
				Touch touch3 = Input.GetTouch(1);
				if (!IsTouchIn(touch2.position) || !IsTouchIn(touch3.position))
				{
					return;
				}
				if (b2ndTouch)
				{
					float num = Vector2.Distance(touch2.position, touch3.position);
					if (mCallBack != null)
					{
						mCallBack.SendMessage("OnFingerStretch", (num - lastDistance) / 100f, SendMessageOptions.DontRequireReceiver);
					}
					lastDistance = num;
				}
				else
				{
					b2ndTouch = true;
					lastDistance = Vector2.Distance(touch2.position, touch3.position);
				}
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (IsTouchIn(Input.mousePosition))
			{
				Vector2 vector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
				if (mCallBack != null)
				{
					mCallBack.SendMessage("OnFingerDrag", vector * 10f, SendMessageOptions.DontRequireReceiver);
					mCallBack.SendMessage("OnFingerDrag2", new Vector4(Input.mousePosition.x, Input.mousePosition.y, vector.x * 10f, vector.y * 10f), SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") != 0f && IsTouchIn(Input.mousePosition))
		{
			Vector2 vector2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			if (mCallBack != null)
			{
				mCallBack.SendMessage("OnFingerStretch", Input.GetAxis("Mouse ScrollWheel") * 10f, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private bool IsTouchIn(Vector2 point)
	{
		if (mRange.x < point.x && mRange.x + mRange.z > point.x && mRange.y < point.y && mRange.y + mRange.w > point.y)
		{
			return true;
		}
		return false;
	}
}

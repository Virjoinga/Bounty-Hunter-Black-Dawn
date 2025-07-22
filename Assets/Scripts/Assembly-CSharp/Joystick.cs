using UnityEngine;

public class Joystick : MonoBehaviour
{
	public GameObject inner;

	public GameObject outer;

	public float radius = 60f;

	private Vector2 initInnerPos;

	private void Start()
	{
		Debug.Log("Joystick Start..........." + base.gameObject.name);
		initInnerPos = new Vector2(inner.transform.localPosition.x, inner.transform.localPosition.y);
	}

	private void OnDrag(Vector2 delta)
	{
		Vector2 vector = new Vector2(inner.transform.localPosition.x, inner.transform.localPosition.y) + delta;
		if ((vector - initInnerPos).magnitude > radius)
		{
			vector = GetPT(initInnerPos, vector, radius);
		}
		inner.transform.localPosition = new Vector3(vector.x, vector.y, inner.transform.localPosition.z);
	}

	private Vector2 GetPT(Vector2 s, Vector2 e, float r)
	{
		float num6;
		float num7;
		float num8;
		float num9;
		if (s.x != e.x)
		{
			if (s.y != e.y)
			{
				float num = (s.y - e.y) / (s.x - e.x);
				float num2 = s.y - num * s.x;
				float num3 = num * num + 1f;
				float num4 = 2f * num2 * num - 2f * s.y * num - 2f * s.x;
				float num5 = s.x * s.x + (num2 - s.y) * (num2 - s.y) - r * r;
				num6 = (-1f * num4 - Mathf.Sqrt(num4 * num4 - 4f * num3 * num5)) / num3 / 2f;
				num7 = (-1f * num4 + Mathf.Sqrt(num4 * num4 - 4f * num3 * num5)) / num3 / 2f;
				num8 = num * num6 + num2;
				num9 = num * num7 + num2;
			}
			else
			{
				num8 = s.y;
				num9 = s.y;
				num6 = s.x + r;
				num7 = s.x - r;
			}
		}
		else
		{
			num6 = s.x;
			num7 = s.x;
			num8 = s.y + r;
			num9 = s.y - r;
		}
		float x;
		float y;
		if ((num6 - e.x) * (num6 - e.x) + (num8 - e.y) * (num8 - e.y) > (num7 - e.x) * (num7 - e.x) + (num9 - e.y) * (num9 - e.y))
		{
			x = num7;
			y = num9;
		}
		else
		{
			x = num6;
			y = num8;
		}
		Vector2 result = default(Vector2);
		result.x = x;
		result.y = y;
		return result;
	}

	private void OnPress(bool isPressed)
	{
		if (!isPressed)
		{
			inner.transform.localPosition = initInnerPos;
		}
	}
}

using System;
using UnityEngine;

public class MathUtil
{
	public static float GetAngleBetweenHorizontal(Vector3 v1, Vector3 v2)
	{
		return GetAngleHorizontal(v1) - GetAngleHorizontal(v2);
	}

	public static float GetAngleBetweenUserHorizontal(Transform target)
	{
		return GetAngleBetweenUserHorizontal(target.position);
	}

	public static float GetAngleBetweenUserHorizontal(GameObject target)
	{
		return GetAngleBetweenUserHorizontal(target.transform);
	}

	public static float GetAngleBetweenUserHorizontal(Vector3 targetPos)
	{
		Transform transform = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetTransform();
		Vector3 forward = transform.forward;
		Vector3 normalized = (targetPos - transform.position).normalized;
		return GetAngleBetweenHorizontal(forward, normalized);
	}

	public static float GetAngleHorizontal(Vector3 direction)
	{
		if (direction.x == 0f)
		{
			if (direction.z < 0f)
			{
				return -90f;
			}
			return 90f;
		}
		float num = Mathf.Atan(direction.z / direction.x) * 180f / (float)Math.PI;
		if (direction.x < 0f)
		{
			return num + 180f;
		}
		num += 360f;
		return num % 360f;
	}

	public static float GetAngleBetweenVertical(Vector3 v1, Vector3 v2)
	{
		return GetAngleVertical(v1) - GetAngleVertical(v2);
	}

	public static float GetAngleBetweenUserVertical(Transform target)
	{
		return GetAngleBetweenUserVertical(target.position);
	}

	public static float GetAngleBetweenUserVertical(GameObject target)
	{
		return GetAngleBetweenUserVertical(target.transform);
	}

	public static float GetAngleBetweenUserVertical(Vector3 targetPos)
	{
		Transform transform = Camera.main.transform;
		Vector3 forward = transform.forward;
		Vector3 normalized = (targetPos - transform.position).normalized;
		return GetAngleBetweenVertical(forward, normalized);
	}

	public static float GetAngleVertical(Vector3 direction)
	{
		if (direction.z == 0f)
		{
			if (direction.y < 0f)
			{
				return -90f;
			}
			return 90f;
		}
		float num = Mathf.Atan(direction.y / direction.z) * 180f / (float)Math.PI;
		if (direction.z < 0f)
		{
			return num + 180f;
		}
		num += 360f;
		return num % 360f;
	}
}

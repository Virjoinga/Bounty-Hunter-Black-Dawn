using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDisk : MonoBehaviour
{
	private List<Transform> m_Trans = new List<Transform>();

	public float radius = 5f;

	public float offset = -40f;

	public bool onlyY = true;

	public Vector3 offsetVector = Vector3.zero;

	private bool isPressed;

	private static int nextIndex;

	private void Awake()
	{
		Reset();
		Reposition();
	}

	public void Reposition()
	{
		int count = m_Trans.Count;
		float num = 1f / (float)count;
		isPressed = false;
		for (int i = 0; i < count; i++)
		{
			float x = Mathf.Sin((float)Math.PI * 2f * num * (float)i) * radius;
			float y = 0f;
			float z = Mathf.Cos((float)Math.PI * 2f * num * (float)i) * radius;
			m_Trans[i].localPosition = new Vector3(x, y, z);
			float x2 = m_Trans[i].localEulerAngles.x;
			float num2 = 360f * num * (float)i;
			float z2 = m_Trans[i].localEulerAngles.z;
			if (onlyY)
			{
				m_Trans[i].localEulerAngles = new Vector3(x2, num2 + offset, z2);
			}
			else
			{
				m_Trans[i].localEulerAngles = new Vector3(x2 + offsetVector.x, num2 + offsetVector.y, z2 + offsetVector.z);
			}
		}
		ModifyPos(nextIndex);
	}

	public void Reset()
	{
		base.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		nextIndex = 0;
	}

	public void Add(GameObject go)
	{
		go.transform.parent = base.transform;
		m_Trans.Add(go.transform);
	}

	public void Clear()
	{
		foreach (Transform tran in m_Trans)
		{
			UnityEngine.Object.Destroy(tran.gameObject);
		}
		m_Trans.Clear();
	}

	private void Update()
	{
		if (!isPressed)
		{
			ModifyPos(nextIndex);
		}
	}

	private void ModifyPos(int nextIndex)
	{
		if (m_Trans.Count >= 1)
		{
			float num = 360f / (float)m_Trans.Count;
			int num2 = 5;
			float num3 = (float)nextIndex * num + 180f;
			num3 = ((!(num3 > 360f)) ? (360f - num3) : (720f - num3));
			float num4 = 0f;
			num4 = ((num3 != 0f || !(base.transform.localEulerAngles.y > 360f - num)) ? (num3 - base.transform.localEulerAngles.y) : (360f - base.transform.localEulerAngles.y));
			num4 = ((!(num4 > 0f)) ? ((!(num4 < (float)(-num2))) ? num4 : ((float)(-num2))) : ((!(num4 > (float)num2)) ? num4 : ((float)num2)));
			base.transform.localEulerAngles += new Vector3(0f, num4, 0f);
		}
	}

	private void OnPress(bool isPressed)
	{
		this.isPressed = isPressed;
		if (!isPressed)
		{
			float num = 360 / m_Trans.Count;
			float num2 = 360f - (base.transform.localEulerAngles.y + 180f) % 360f;
			nextIndex = Mathf.RoundToInt(num2 / num) % m_Trans.Count;
		}
	}

	public int GetIndex()
	{
		return nextIndex;
	}
}

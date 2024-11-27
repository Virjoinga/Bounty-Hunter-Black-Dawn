using System;
using UnityEngine;

public class UILoadingScript : IgnoreTimeScale
{
	private Vector3 m_rotationSpeed = new Vector3(0f, 0f, -60f);

	private DateTime m_rotateTimer;

	private void Start()
	{
		m_rotateTimer = DateTime.Now;
	}

	private void Update()
	{
		if ((DateTime.Now - m_rotateTimer).TotalMilliseconds >= 100.0 && base.transform != null)
		{
			base.transform.Rotate(m_rotationSpeed);
			m_rotateTimer = DateTime.Now;
		}
	}
}

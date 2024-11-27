using System;
using UnityEngine;

internal class UILoadingNet : IgnoreTimeScale
{
	public static UILoadingNet m_instance;

	private DateTime m_time;

	private byte m_msgEvent;

	private string m_context;

	private UIMsgListener m_listener;

	public GameObject m_loading;

	private Vector3 m_rotationSpeed = new Vector3(0f, 0f, -60f);

	private DateTime m_rotateTimer;

	private float m_timeout;

	private bool m_bSystem;

	private bool m_bRunning;

	public GameObject m_collider;

	private void Awake()
	{
		NGUITools.SetActive(base.gameObject, false);
		BoxCollider component = m_collider.GetComponent<BoxCollider>();
		component.center = Vector3.zero;
		component.size = new Vector3(Screen.width, Screen.height, 0f);
		m_instance = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!m_bRunning)
		{
			return;
		}
		if ((DateTime.Now - m_rotateTimer).TotalMilliseconds >= 200.0)
		{
			m_loading.transform.Rotate(m_rotationSpeed);
			m_rotateTimer = DateTime.Now;
		}
		if (!((DateTime.Now - m_time).TotalSeconds >= (double)m_timeout))
		{
			return;
		}
		if (m_msgEvent != 0)
		{
			if (m_bSystem)
			{
				UIMsgBox.instance.ShowSystemMessage(m_listener, m_context, 2, m_msgEvent);
			}
			else
			{
				UIMsgBox.instance.ShowMessage(m_listener, m_context, 2, m_msgEvent);
			}
		}
		Hide();
	}

	public void Show(string txt, float timeout, byte msgEvent, UIMsgListener listener)
	{
		NGUITools.SetActive(base.gameObject, true);
		m_context = txt;
		m_timeout = timeout;
		m_msgEvent = msgEvent;
		m_listener = listener;
		m_rotateTimer = DateTime.Now;
		m_time = DateTime.Now;
		m_bRunning = true;
		m_bSystem = false;
	}

	public void ShowSystem(string txt, float timeout, byte msgEvent, UIMsgListener listener)
	{
		Show(txt, timeout, msgEvent, listener);
		m_bSystem = true;
	}

	public void Hide()
	{
		m_bRunning = false;
		NGUITools.SetActive(base.gameObject, false);
	}

	private void OnDestroy()
	{
		m_instance = null;
	}
}

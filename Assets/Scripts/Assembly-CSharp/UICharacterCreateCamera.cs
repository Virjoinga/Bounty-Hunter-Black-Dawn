using UnityEngine;

public class UICharacterCreateCamera : MonoBehaviour
{
	public enum State
	{
		StaticIn = 0,
		StaticOut = 1,
		ZoomIn = 2,
		ZoomOut = 3
	}

	public Camera m_Camera;

	public Vector3 m_InPos;

	public Vector3 m_InAngle;

	public Vector3 m_OutPos;

	public Vector3 m_OutAngle;

	public float m_Sensitivity = 1f;

	public State m_ActionOnStart;

	private State state;

	private Vector3 CurPos
	{
		get
		{
			return m_Camera.transform.localPosition;
		}
		set
		{
			m_Camera.transform.localPosition = value;
		}
	}

	private Vector3 CurAngle
	{
		get
		{
			return m_Camera.transform.localEulerAngles;
		}
		set
		{
			m_Camera.transform.localEulerAngles = value;
		}
	}

	private float Sensitivity
	{
		get
		{
			return m_Sensitivity * Time.deltaTime;
		}
	}

	private void Awake()
	{
		if (m_Camera == null)
		{
			m_Camera = GetComponent<Camera>();
		}
		if (m_Camera == null)
		{
			Object.Destroy(this);
		}
	}

	private void Start()
	{
		state = m_ActionOnStart;
	}

	private void Update()
	{
		switch (state)
		{
		case State.ZoomIn:
			CurPos += (m_InPos - CurPos) * Sensitivity;
			CurAngle += (m_InAngle - CurAngle) * Sensitivity;
			if (Vector3.Distance(CurPos, m_InPos) < 2f)
			{
				state = State.StaticIn;
			}
			break;
		case State.ZoomOut:
			CurPos += (m_OutPos - CurPos) * Sensitivity;
			CurAngle += (m_OutAngle - CurAngle) * Sensitivity;
			if (Vector3.Distance(CurPos, m_OutPos) < 2f)
			{
				state = State.StaticOut;
			}
			break;
		case State.StaticIn:
			CurPos = m_InPos;
			CurAngle = m_InAngle;
			break;
		case State.StaticOut:
			CurPos = m_OutPos;
			CurAngle = m_OutAngle;
			break;
		}
	}

	public void ZoomIn(bool instantly)
	{
		if (instantly)
		{
			state = State.StaticIn;
		}
		else
		{
			state = State.ZoomIn;
		}
	}

	public void ZoomOut(bool instantly)
	{
		if (instantly)
		{
			state = State.StaticOut;
		}
		else
		{
			state = State.ZoomOut;
		}
	}

	public void Switch(bool instantly)
	{
		if (state == State.StaticIn || state == State.ZoomIn)
		{
			ZoomOut(instantly);
		}
		else
		{
			ZoomIn(instantly);
		}
	}
}

using UnityEngine;

public class PositionTweenScript : MonoBehaviour
{
	public float m_distance;

	private float m_time;

	private Vector3 m_fromPos;

	private Vector3 m_toPos;

	private Transform m_transform;

	private bool m_forward;

	private void Start()
	{
		m_time = Time.time;
		m_transform = base.transform;
	}

	public void SetTweenPos(Vector3 from, Vector3 to)
	{
		m_fromPos = from;
		m_toPos = to;
	}

	private void Update()
	{
		if (Time.time - m_time >= 1f)
		{
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			if (gameWorld != null)
			{
				LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
				if (localPlayer != null)
				{
					float num = Vector3.Distance(m_transform.position, localPlayer.GetTransform().position);
					if (num <= m_distance)
					{
						m_forward = true;
					}
					else
					{
						m_forward = false;
					}
				}
			}
			m_time = Time.time;
		}
		if (m_forward)
		{
			if (m_transform.position != m_toPos)
			{
				m_transform.position = Vector3.Lerp(m_transform.position, m_toPos, 5f * Time.deltaTime);
			}
		}
		else if (m_transform.position != m_fromPos)
		{
			m_transform.position = Vector3.Lerp(m_transform.position, m_fromPos, 5f * Time.deltaTime);
		}
	}
}

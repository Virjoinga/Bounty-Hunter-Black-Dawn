using UnityEngine;

public class UIWaiting : MonoBehaviour
{
	private float m_TimeOut;

	private float m_StartTime;

	public static UIWaiting m_instance;

	private BoxCollider m_block;

	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	private void Start()
	{
		m_instance = this;
		m_block = base.gameObject.GetComponent<Collider>() as BoxCollider;
		m_block.size = new Vector3(Screen.width, Screen.height, 0f);
	}

	private void Destroy()
	{
		m_instance = null;
	}

	private void Show(float time)
	{
		m_StartTime = Time.time;
		m_TimeOut = time;
		base.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (Time.time - m_StartTime > m_TimeOut)
		{
			base.gameObject.SetActive(false);
		}
	}
}

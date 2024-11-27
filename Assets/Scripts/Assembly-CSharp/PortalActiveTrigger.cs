using UnityEngine;

public class PortalActiveTrigger : MonoBehaviour
{
	public GameObject m_Board;

	public Animation m_PortalAni;

	private bool triggerEnabled;

	private void Awake()
	{
		triggerEnabled = false;
	}

	private void OnTriggerEnter(Collider obj)
	{
		if (!triggerEnabled)
		{
			triggerEnabled = true;
			GameObject original = Resources.Load("RPG_effect/RPG_portal_002") as GameObject;
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.transform.parent = m_Board.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			if (m_PortalAni != null)
			{
				m_PortalAni.Play();
			}
		}
	}
}

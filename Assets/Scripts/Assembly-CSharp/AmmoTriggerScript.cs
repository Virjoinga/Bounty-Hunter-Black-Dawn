using UnityEngine;

public class AmmoTriggerScript : MonoBehaviour
{
	private bool m_enter;

	private Timer m_timer = new Timer();

	private void Start()
	{
		m_enter = false;
		m_timer.SetTimer(0.3f, false);
	}

	private void OnTriggerStay(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer != PhysicsLayer.PLAYER_COLLIDER || !m_timer.Ready())
		{
			return;
		}
		m_timer.Do();
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
		if (!(component != null))
		{
			return;
		}
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 normalized = (vector - transform.position).normalized;
		Ray ray = new Ray(transform.position + 1.8f * normalized, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 30f, 1 << PhysicsLayer.ITEMS))
		{
			if (hitInfo.collider.gameObject.tag == TagName.AMMO_SYSTEM)
			{
				m_enter = true;
				UserStateHUD.GetInstance().SetShowFillAllButton(true);
			}
			else
			{
				m_enter = false;
				UserStateHUD.GetInstance().SetShowFillAllButton(false);
			}
		}
		else
		{
			m_enter = false;
			UserStateHUD.GetInstance().SetShowFillAllButton(false);
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer == PhysicsLayer.PLAYER_COLLIDER)
		{
			m_enter = false;
			UserStateHUD.GetInstance().SetShowFillAllButton(false);
		}
	}
}

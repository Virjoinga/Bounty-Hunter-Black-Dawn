using UnityEngine;

public class JoystickManager : MonoBehaviour
{
	public Joystick m_MoveJoystick;

	public Joystick m_ShootJoystick;

	private Vector3 offset = new Vector3(Screen.width, 0f, 0f);

	private UIRoot uiRoot;

	private void Awake()
	{
		uiRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		UserStateHUD.GetInstance().Joystick.MoveCenter = GetMoveJoystickCenter();
		UserStateHUD.GetInstance().Joystick.ShootCenter = GetShootJoystickCenter();
		UserStateHUD.GetInstance().Joystick.Radius = GetMoveJoystickRadius();
		UserStateHUD.GetInstance().Joystick.IsInit = true;
	}

	public void SetAllActiveRecursively(bool state)
	{
		if (UserStateHUD.GetInstance().Joystick.IsFixed)
		{
			m_MoveJoystick.gameObject.SetActiveRecursively(state);
		}
		else
		{
			m_MoveJoystick.gameObject.SetActiveRecursively(false);
		}
		m_ShootJoystick.gameObject.SetActiveRecursively(state);
	}

	private Vector2 GetMoveJoystickCenter()
	{
		return m_MoveJoystick.transform.localPosition + offset;
	}

	private Vector2 GetShootJoystickCenter()
	{
		Vector3 localPosition = m_ShootJoystick.transform.localPosition;
		Transform parent = base.transform.parent;
		while (parent != null && parent.GetComponent<UIRoot>() == null)
		{
			localPosition += parent.localPosition;
			parent = parent.parent;
		}
		float num = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		localPosition = localPosition * Screen.height / num;
		Debug.Log("************************");
		Debug.Log("(pos + offset) : " + (localPosition + offset));
		return localPosition + offset;
	}

	private float GetMoveJoystickRadius()
	{
		float num = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		return m_MoveJoystick.radius * (float)Screen.height / num;
	}

	private float GetShootJoystickRadius()
	{
		float num = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		return m_ShootJoystick.radius * (float)Screen.height / num;
	}

	private void Update()
	{
	}
}

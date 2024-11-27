using UnityEngine;

public class UISendEvent : MonoBehaviour
{
	public GameObject m_eventReceiver;

	public string m_functionName = "OnKey";

	private SendEventType m_SendEventType;

	private void Start()
	{
	}

	private void OnPress(bool isDown)
	{
		if (isDown)
		{
			if (m_eventReceiver == null)
			{
				m_eventReceiver = base.gameObject;
			}
			m_eventReceiver.SendMessage(m_functionName, SendEventType.keyPressed, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			if (m_eventReceiver == null)
			{
				m_eventReceiver = base.gameObject;
			}
			m_eventReceiver.SendMessage(m_functionName, SendEventType.keyPressed, SendMessageOptions.DontRequireReceiver);
		}
	}
}

using UnityEngine;

public class UIMsgButton : MonoBehaviour
{
	public UIMsg.UIMsgButton eventID;

	public UIMsg m_UIMsg;

	private void OnClick()
	{
		if (m_UIMsg != null)
		{
			m_UIMsg.HandleEvent(eventID);
		}
	}
}

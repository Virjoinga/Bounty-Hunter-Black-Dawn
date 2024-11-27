using UnityEngine;

public class ChatManager : MonoBehaviour
{
	public ChatBox m_ChatBox;

	public void SetAllActiveRecursively(bool state)
	{
		m_ChatBox.gameObject.SetActive(state);
	}

	public void PopOrPushInputBar()
	{
		if (base.gameObject.activeSelf)
		{
			m_ChatBox.Play();
		}
	}
}

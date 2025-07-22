using UnityEngine;

public class UIVSSeat : MonoBehaviour, UIMsgListener
{
	public int m_Seat;

	public GameObject m_PlayerInfoContainer;

	public GameObject m_EmptyInfoContainer;

	private GameObject m_PlayerInfo;

	private void Awake()
	{
		m_PlayerInfoContainer.SetActive(false);
		m_EmptyInfoContainer.SetActive(true);
	}

	public void SetPlayer(GameObject playerInfo)
	{
		if (playerInfo == null)
		{
			if (m_PlayerInfo != null)
			{
				Object.Destroy(m_PlayerInfo);
			}
			m_PlayerInfoContainer.SetActive(false);
			m_EmptyInfoContainer.SetActive(true);
		}
		else
		{
			m_PlayerInfoContainer.SetActive(true);
			m_EmptyInfoContainer.SetActive(false);
			m_PlayerInfo = playerInfo;
			m_PlayerInfo.transform.parent = m_PlayerInfoContainer.transform;
			m_PlayerInfo.transform.localPosition = Vector3.zero;
			m_PlayerInfo.transform.localEulerAngles = Vector3.zero;
			m_PlayerInfo.transform.localScale = Vector3.one;
		}
	}

	private void OnClick()
	{
		if (m_PlayerInfo == null)
		{
			UIVSTeam.m_instance.m_roomList.ChangeSeat(m_Seat);
			UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT"), 5f, 18, this);
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		Debug.Log("onButton....");
		if (whichMsg.EventId == 12)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}
}

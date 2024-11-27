using UnityEngine;

public class UIVSTeam : UIGameMenuNormal
{
	public static UIVSTeam m_instance;

	public UIVSRoomList m_roomList;

	public UIVSCreateRoom m_createRoom;

	public GameObject m_template;

	public GameObject m_password;

	public GameObject m_rewards;

	public GameObject m_rank;

	public static bool giftDaily;

	protected override void Awake()
	{
		base.Awake();
		m_roomList.gameObject.SetActive(false);
		m_createRoom.gameObject.SetActive(false);
		m_password.SetActive(false);
		m_rewards.SetActive(false);
		m_template.SetActive(false);
		m_rank.SetActive(false);
		m_instance = this;
		SetMenuCloseOnDestroy(true);
	}

	public void SetMenuClose(bool state)
	{
		SetMenuCloseOnDestroy(state);
	}

	protected override byte InitMask()
	{
		return 2;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		m_instance = null;
	}

	public void ShowRoomList()
	{
		m_roomList.gameObject.SetActive(true);
	}
}

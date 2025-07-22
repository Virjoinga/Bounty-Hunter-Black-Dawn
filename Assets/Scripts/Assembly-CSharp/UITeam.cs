using UnityEngine;

public class UITeam : UIGameMenuNormal
{
	public static UITeam m_instance;

	public UIRoomList m_roomList;

	public GameObject m_createRoom;

	public GameObject m_template;

	public GameObject m_password;

	public GameObject m_rewards;

	public static bool giftDaily;

	protected override void Awake()
	{
		base.Awake();
		m_roomList.gameObject.SetActive(false);
		m_createRoom.SetActive(false);
		m_password.SetActive(false);
		m_rewards.SetActive(false);
		m_template.SetActive(false);
		m_instance = this;
		SetMenuCloseOnDestroy(true);
	}

	public void SetMenuClose(bool state)
	{
		SetMenuCloseOnDestroy(state);
	}

	private void Start()
	{
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

	private void Update()
	{
	}
}

using UnityEngine;

public class UIBossRushTeam : UIGameMenuNormal
{
	public static UIBossRushTeam m_instance;

	public UIBossRushRoomList m_roomList;

	public GameObject m_createRoom;

	public GameObject m_template;

	public GameObject m_password;

	public GameObject m_rewards;

	public static bool giftDaily;

	private static Mode bossRushMode = Mode.BossRushElite;

	public static Mode BossRushMode
	{
		get
		{
			return bossRushMode;
		}
		set
		{
			if (value != Mode.BossRushElite)
			{
				bossRushMode = Mode.BossRushRookie;
			}
			else
			{
				bossRushMode = Mode.BossRushElite;
			}
		}
	}

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

using UnityEngine;

public class ArrowScript : MonoBehaviour
{
	protected float lastUpdateTime;

	public TeamName m_team;

	public byte[] m_state = new byte[5];

	protected GameObject m_arrowObj;

	private void Start()
	{
	}

	private void Update()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		TeamName team = localPlayer.Team;
		if (m_arrowObj == null)
		{
			m_arrowObj = base.transform.GetChild(0).gameObject;
		}
		if (!(m_arrowObj != null))
		{
			return;
		}
		if (team != m_team)
		{
			m_arrowObj.SetActive(false);
		}
		else
		{
			if (!(Time.time - lastUpdateTime >= 1f))
			{
				return;
			}
			lastUpdateTime = Time.time;
			for (int i = 0; i < m_state.Length; i++)
			{
				if (m_state[i] == 1 && StateEqual(i))
				{
					m_arrowObj.SetActive(true);
					return;
				}
			}
			m_arrowObj.SetActive(false);
		}
	}

	private bool StateEqual(int state)
	{
		VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
		if (vSTDMManager == null)
		{
			return false;
		}
		for (int i = 0; i < Global.ALL_STATE.GetLength(1); i++)
		{
			if (vSTDMManager.pointInfo.ContainsKey(i))
			{
				if (vSTDMManager.pointInfo[i].GetOwner() != Global.ALL_STATE[state, i])
				{
					return false;
				}
				continue;
			}
			return false;
		}
		return true;
	}
}

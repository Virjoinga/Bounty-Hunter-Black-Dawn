using UnityEngine;

public class BossArea : MonoBehaviour
{
	public const int BOSS_FIGHT_SPAWN_POINT_1 = 0;

	public const int BOSS_FIGHT_SPAWN_POINT_2 = 1;

	public const int BOSS_FIGHT_SPAWN_POINT_3 = 2;

	public const int BOSS_FIGHT_SPAWN_POINT_4 = 3;

	public const int BOSS_AGAIN_FIGHT_SPAWN_POINT_1 = 4;

	public GameObject[] m_SpawnPoint;

	public BossAreaTrigger m_Trigger;

	public static BossArea instance;

	private void Awake()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public void SetPosition(LocalPlayer player, int pos)
	{
		if (pos < 0 || pos > m_SpawnPoint.Length - 1)
		{
			return;
		}
		player.GetTransform().position = m_SpawnPoint[pos].transform.position;
		player.GetTransform().rotation = m_SpawnPoint[pos].transform.rotation;
		if (player.IsLocal())
		{
			FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
			if (null != component)
			{
				component.AngelH = m_SpawnPoint[pos].transform.rotation.eulerAngles.y;
			}
		}
	}
}

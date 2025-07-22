using UnityEngine;

public class EnemySpawnTriggerScript : MonoBehaviour
{
	public QuestEnemySpawnScript SpawnScript;

	private Timer mCheckQuestTimer = new Timer();

	private void Start()
	{
		mCheckQuestTimer.SetTimer(1f, false);
		SetTriggerEnable(false);
	}

	private void Update()
	{
		if (!(SpawnScript == null) && mCheckQuestTimer.Ready())
		{
			mCheckQuestTimer.Do();
			bool triggerEnable = GameApp.GetInstance().GetUserState().m_questStateContainer.IsTriggerForRespawnEnemy(SpawnScript.QuestId);
			SetTriggerEnable(triggerEnable);
		}
	}

	public void SetTriggerEnable(bool enable)
	{
		Collider component = base.gameObject.GetComponent<Collider>();
		if (null != component)
		{
			component.enabled = enable;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.PLAYER_COLLIDER && SpawnScript != null)
		{
			SpawnScript.StartSpawn();
		}
	}
}

using UnityEngine;

public class InfoManager : MonoBehaviour
{
	public GameObject m_UserState;

	public GameObject m_BuffState;

	public GameObject m_EnemyState;

	public GameObject m_DamageState;

	public GameObject m_SightHeadState;

	public GameObject m_InfoBoxState;

	public GameObject m_RemotePlayerState;

	public GameObject m_MapState;

	public GameObject m_Aim;

	public GameObject m_WeaponBullet;

	public GameObject m_GrenadeBullet;

	public GameObject m_SummonState;

	public GameObject m_UnhurtState;

	public void SetAllActiveRecursively(bool state)
	{
		m_UserState.SetActiveRecursively(state);
		m_BuffState.SetActiveRecursively(state);
		m_EnemyState.SetActiveRecursively(state);
		m_DamageState.SetActiveRecursively(state);
		m_SightHeadState.SetActiveRecursively(state);
		m_InfoBoxState.SetActiveRecursively(state);
		m_RemotePlayerState.SetActiveRecursively(state);
		m_MapState.SetActiveRecursively(state);
		m_Aim.SetActiveRecursively(state);
		m_WeaponBullet.SetActiveRecursively(state);
		m_GrenadeBullet.SetActiveRecursively(state);
		m_SummonState.SetActiveRecursively(state);
		m_UnhurtState.SetActiveRecursively(state);
	}
}

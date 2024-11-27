using UnityEngine;

public class DyingManager : MonoBehaviour
{
	public GameObject m_Saver;

	public GameObject m_Dead;

	public void SetAllActiveRecursively(bool state)
	{
		m_Saver.SetActiveRecursively(state);
		m_Dead.SetActiveRecursively(state);
	}
}

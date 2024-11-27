using UnityEngine;

public class VSHUDManager : MonoBehaviour
{
	public GameObject m_VSTDMPointState;

	public GameObject m_VSTDMScoreState;

	public void SetAllActiveRecursively(bool state)
	{
		m_VSTDMPointState.SetActive(state);
		m_VSTDMScoreState.SetActive(state);
	}
}

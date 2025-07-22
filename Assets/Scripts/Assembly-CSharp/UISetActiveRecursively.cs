using UnityEngine;

public class UISetActiveRecursively : MonoBehaviour
{
	public GameObject target;

	public bool m_bState = true;

	private void OnClick()
	{
		NGUITools.SetActive(target, m_bState);
	}
}

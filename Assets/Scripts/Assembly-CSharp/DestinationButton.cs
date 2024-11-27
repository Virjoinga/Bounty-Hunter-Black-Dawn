using UnityEngine;

public class DestinationButton : MonoBehaviour
{
	public UILabel m_DestinationName;

	public GameObject m_QuestMark;

	public void SetName(string name)
	{
		m_DestinationName.text = name;
	}

	public void AddQuestMark(GameObject mark)
	{
		mark.transform.parent = m_QuestMark.transform;
		mark.transform.localPosition = Vector3.zero;
		mark.transform.localEulerAngles = Vector3.zero;
		mark.SetActive(true);
	}
}

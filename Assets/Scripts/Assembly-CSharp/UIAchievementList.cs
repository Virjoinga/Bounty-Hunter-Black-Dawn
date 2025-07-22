using UnityEngine;

public class UIAchievementList : MonoBehaviour
{
	public UIGrid m_Container;

	public GameObject m_Sample;

	public Transform m_Sample_Icon;

	public UILabel m_Sample_Name;

	public UILabel m_Sample_Info;

	public Transform m_Icon_Container;

	public GameObject m_LockIcon;

	public GameObject m_UnlockIcon;

	private void Start()
	{
		NGUITools.SetActive(m_Sample, false);
		NGUITools.SetActive(m_Icon_Container.gameObject, false);
	}

	public void Add(string name, string info, bool isLock)
	{
		m_Sample_Name.text = name;
		m_Sample_Info.text = info;
		if (isLock)
		{
			ModifyParent(m_LockIcon.transform, m_Sample_Icon);
			ModifyParent(m_UnlockIcon.transform, m_Icon_Container);
		}
		else
		{
			ModifyParent(m_LockIcon.transform, m_Icon_Container);
			ModifyParent(m_UnlockIcon.transform, m_Sample_Icon);
		}
		GameObject gameObject = Object.Instantiate(m_Sample) as GameObject;
		NGUITools.SetActive(gameObject, true);
		gameObject.transform.parent = m_Container.transform;
		gameObject.transform.eulerAngles = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
	}

	private void ModifyParent(Transform child, Transform parent)
	{
		child.parent = parent;
		child.localPosition = Vector3.zero;
		child.eulerAngles = Vector3.zero;
	}

	public void Reposition()
	{
		m_Container.Reposition();
	}
}

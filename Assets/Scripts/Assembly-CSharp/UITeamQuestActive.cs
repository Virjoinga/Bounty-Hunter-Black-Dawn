using UnityEngine;

public class UITeamQuestActive : MonoBehaviour
{
	public UILabel m_text;

	public int m_id;

	public GameObject m_background;

	public GameObject m_owner;

	public GameObject m_flag;

	public GameObject m_state;

	public UILabel m_level;

	private void Start()
	{
	}

	public void SetText(string text)
	{
		if (!string.IsNullOrEmpty(text) && text != null)
		{
			m_text.text = text;
		}
	}

	public void SetBackground(string name)
	{
		UISlicedSprite component = m_flag.GetComponent<UISlicedSprite>();
		component.spriteName = name;
		component.MakePixelPerfect();
	}

	public void SetState(string name)
	{
		UISprite component = m_state.GetComponent<UISprite>();
		component.spriteName = name;
		component.MakePixelPerfect();
	}

	public void ClearOwner()
	{
		foreach (Transform item in m_owner.transform)
		{
			Object.Destroy(item.gameObject);
		}
	}

	public void AddOwner(GameObject seatObj)
	{
		seatObj.transform.parent = m_owner.transform;
		seatObj.transform.localPosition = Vector3.zero;
		seatObj.transform.localRotation = Quaternion.identity;
		seatObj.transform.localScale = Vector3.one;
	}

	public void SetLevel(string text, Color color)
	{
		if (!string.IsNullOrEmpty(text) && text != null)
		{
			m_level.text = text;
			m_level.color = color;
		}
	}

	private void OnClick()
	{
		UITeamQuest component = UIQuest.m_instance.m_teamQuest.GetComponent<UITeamQuest>();
		component.SetSelectedQuest(m_id);
	}
}

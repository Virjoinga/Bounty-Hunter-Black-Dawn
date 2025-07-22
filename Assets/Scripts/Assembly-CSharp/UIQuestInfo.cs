using UnityEngine;

public class UIQuestInfo : MonoBehaviour
{
	public int m_id;

	public UILabel m_name;

	public UILabel m_level;

	public void SetQuestInfo(int id, string name, string level)
	{
		m_id = id;
		SetName(name);
		SetLevel(level);
	}

	public void SetName(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			m_name.text = text;
		}
	}

	public void SetLevel(string level)
	{
		if (!string.IsNullOrEmpty(level))
		{
			m_level.text = level;
		}
	}
}

using UnityEngine;

public class UILog : MonoBehaviour
{
	public UILabel m_text;

	public GameObject m_background;

	public GameObject m_flag;

	public GameObject m_state;

	public UILabel m_level;

	private int m_id;

	private void Start()
	{
	}

	public void SetId(int commonId)
	{
		m_id = commonId;
	}

	public int GetId()
	{
		return m_id;
	}

	public void SetFlag(string name)
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

	private void OnClick()
	{
		UILogs component = UIQuest.m_instance.m_logs.GetComponent<UILogs>();
		component.Selection(m_id);
	}

	public void SetText(string text)
	{
		if (!string.IsNullOrEmpty(text) && text != null)
		{
			m_text.text = text;
		}
	}

	public void SetLevel(string text, Color color)
	{
		if (!string.IsNullOrEmpty(text) && text != null)
		{
			m_level.text = text;
			m_level.color = color;
		}
	}
}

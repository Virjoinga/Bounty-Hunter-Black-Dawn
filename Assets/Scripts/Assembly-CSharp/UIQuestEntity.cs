using System.Collections.Generic;
using UnityEngine;

public class UIQuestEntity : MonoBehaviour
{
	public UILabel m_text;

	public GameObject m_background;

	public GameObject m_state;

	public GameObject m_flag;

	public UILabel m_level;

	protected QuestPhase m_phase;

	protected List<short> m_quests = new List<short>();

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

	public void SetPhase(QuestPhase phase)
	{
		m_phase = phase;
	}

	public QuestPhase GetPhase()
	{
		return m_phase;
	}

	public void ClearQuest()
	{
		m_quests.Clear();
	}

	public void AddQuest(short questId)
	{
		m_quests.Add(questId);
	}

	public List<short> GetQuests()
	{
		return m_quests;
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
		UIQuestEntities component = UIQuest.m_instance.m_quests.GetComponent<UIQuestEntities>();
		component.Selection(m_id, false);
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

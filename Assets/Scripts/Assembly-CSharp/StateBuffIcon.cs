using UnityEngine;

public class StateBuffIcon : MonoBehaviour
{
	private int time;

	public UILabel m_TimeLabel;

	public UISprite m_BuffSprite;

	private bool visible;

	public new string name { get; set; }

	public int Time
	{
		set
		{
			time = value;
		}
	}

	private void Awake()
	{
		m_BuffSprite.atlas = SkillTreeMgr.CharacterSkillIconAtlas;
		visible = true;
	}

	private void Start()
	{
		m_BuffSprite.spriteName = name;
	}

	private void Update()
	{
		m_TimeLabel.text = string.Empty + time;
	}

	public void Blink()
	{
		m_BuffSprite.color = new Color(m_BuffSprite.color.r, m_BuffSprite.color.g, m_BuffSprite.color.b, visible ? 1 : 0);
		visible = !visible;
	}
}

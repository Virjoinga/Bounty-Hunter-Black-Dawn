using System;
using UnityEngine;

public class UILabelRolling : MonoBehaviour
{
	private enum Movement
	{
		Static = 0,
		Right = 1,
		Left = 2
	}

	public UILabel m_UILabel;

	public float disPerSec = 1f;

	public float staticTime = 2500f;

	private string lastText = string.Empty;

	private int lastLineWidth;

	private int lastValue;

	private UIPanel m_UIPanel;

	private Vector3 offset = Vector3.zero;

	private Vector2 ClipLeftTopPos;

	private Vector2 ClipSize;

	private Movement movement;

	private DateTime lastTime;

	private DateTime lastStaticTime;

	private Vector3 UILabelInitPosition = Vector3.zero;

	public bool HasTextChanged
	{
		get
		{
			if (lastText.Equals(m_UILabel.text) && lastLineWidth == m_UILabel.lineWidth)
			{
				return false;
			}
			lastText = m_UILabel.text;
			lastLineWidth = m_UILabel.maxLineCount;
			return true;
		}
	}

	public int TextWidth
	{
		get
		{
			if (HasTextChanged)
			{
				lastValue = GetTextWidth(m_UILabel);
			}
			return lastValue;
		}
	}

	private Vector2 TextLeftTopPos
	{
		get
		{
			return m_UILabel.transform.localPosition + offset;
		}
	}

	private void Awake()
	{
		Transform parent = base.gameObject.transform;
		bool flag = true;
		while (flag)
		{
			m_UIPanel = parent.gameObject.GetComponent<UIPanel>();
			if (m_UIPanel == null)
			{
				parent = parent.parent;
				if (parent == null)
				{
					flag = false;
				}
				else
				{
					offset += parent.localPosition;
				}
			}
			else
			{
				flag = false;
			}
		}
		UILabelInitPosition = m_UILabel.transform.localPosition;
		ClipLeftTopPos = new Vector2(m_UIPanel.clipRange.x - m_UIPanel.clipRange.z / 2f, m_UIPanel.clipRange.y - m_UIPanel.clipRange.w / 2f);
		ClipSize = new Vector2(m_UIPanel.clipRange.z, m_UIPanel.clipRange.w);
	}

	private void Start()
	{
		UIFont font = m_UILabel.font;
		TryToMove();
		lastTime = DateTime.Now;
		lastStaticTime = DateTime.Now;
	}

	private void TryToMove()
	{
		if (ClipSize.x >= (float)TextWidth)
		{
			movement = Movement.Static;
		}
		else
		{
			movement = Movement.Left;
		}
	}

	private void Update()
	{
		TimeSpan timeSpan = DateTime.Now - lastTime;
		lastTime = DateTime.Now;
		switch (movement)
		{
		case Movement.Static:
			if ((DateTime.Now - lastStaticTime).TotalMilliseconds > (double)staticTime)
			{
				TryToMove();
				lastStaticTime = DateTime.Now;
			}
			break;
		case Movement.Left:
			m_UILabel.transform.localPosition -= new Vector3((float)timeSpan.TotalMilliseconds * disPerSec, 0f, 0f);
			if (TextLeftTopPos.x + (float)TextWidth < ClipLeftTopPos.x)
			{
				movement = Movement.Static;
				m_UILabel.transform.localPosition = UILabelInitPosition;
			}
			lastStaticTime = DateTime.Now;
			break;
		}
		GetTextWidth(m_UILabel);
	}

	private int GetTextWidth(UILabel label)
	{
		float num = label.font.CalculatePrintedSize(label.text, label.supportEncoding, label.symbolStyle).x * m_UILabel.transform.localScale.x;
		return (int)num;
	}

	public void ResetLabelPosition()
	{
		m_UILabel.transform.localPosition = UILabelInitPosition;
	}
}

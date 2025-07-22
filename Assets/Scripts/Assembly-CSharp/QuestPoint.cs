using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestPoint
{
	public class QuestPosition
	{
		public Vector2 m_pos = Vector2.zero;

		public QuestPointState m_state;

		public void SetState(QuestPointState state)
		{
			m_state = state;
		}
	}

	public short m_siteId;

	public List<QuestPosition> m_position = new List<QuestPosition>();

	public void SetPos(string strPos)
	{
		if (string.IsNullOrEmpty(strPos))
		{
			return;
		}
		string[] array = strPos.Split(',');
		if (array == null || array.Length <= 0)
		{
			Debug.Log("error quest points....");
			return;
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Split(':');
			if (array3 == null || array3.Length <= 1)
			{
				Debug.Log("error quest point....");
				break;
			}
			QuestPosition questPosition = new QuestPosition();
			questPosition.m_pos.x = Convert.ToInt32(array3[0]);
			questPosition.m_pos.y = Convert.ToInt32(array3[1]);
			m_position.Add(questPosition);
		}
	}

	public List<QuestPosition> GetPos()
	{
		return m_position;
	}
}

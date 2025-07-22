using System;
using UnityEngine;

public class UIPlayerQuestInfo : MonoBehaviour
{
	public int m_id;

	public UILabel m_name;

	public UILabel m_level;

	public UISprite m_role;

	public UISprite m_seat;

	public void SetPlayerInfo(int id, string name, int level, int role, int seat)
	{
		m_id = id;
		SetName(name);
		SetLevel(level);
		SetRole(role);
		SetSeat(seat);
	}

	public void SetName(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			m_name.text = text;
		}
	}

	public void SetLevel(int level)
	{
		m_level.text = Convert.ToString(level);
	}

	public void SetRole(int role)
	{
		string userClassIcon = UserStateHUD.GetInstance().GetUserClassIcon((CharacterClass)role);
		UISprite component = m_role.GetComponent<UISprite>();
		component.spriteName = userClassIcon;
	}

	public void SetSeat(int id)
	{
		if (!GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			UISprite component = m_seat.GetComponent<UISprite>();
			component.spriteName = UIConstant.QUEST_SEAT_SPRITE[id];
			component.MakePixelPerfect();
		}
	}
}

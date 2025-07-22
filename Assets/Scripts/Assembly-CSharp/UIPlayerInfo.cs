using System;
using UnityEngine;

public class UIPlayerInfo : MonoBehaviour
{
	public UILabel m_name;

	public UILabel m_level;

	public UISprite m_role;

	public GameObject m_LocalSign;

	public void SetPlayerInfo(string name, int level, int role, bool isLocal)
	{
		SetName(name);
		SetLevel(level);
		SetRole(role);
		if (m_LocalSign != null)
		{
			if (isLocal)
			{
				m_LocalSign.SetActive(true);
			}
			else
			{
				m_LocalSign.SetActive(false);
			}
		}
	}

	public void SetPlayerInfo(string name, int level, int role)
	{
		SetPlayerInfo(name, level, role, false);
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
		component.transform.localPosition = new Vector3(0f, 0f, -1f);
		component.spriteName = userClassIcon;
	}
}

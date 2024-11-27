using UnityEngine;

public class UICharacterData
{
	private UserState.RoleState m_State;

	public bool IsHasProfile
	{
		get
		{
			return m_State != null;
		}
	}

	public int CharLevel
	{
		get
		{
			return (m_State != null) ? m_State.CharLevel : 0;
		}
	}

	public string CharClass
	{
		get
		{
			return (m_State != null) ? m_State.CharClass.ToString() : string.Empty;
		}
	}

	public string CharName
	{
		get
		{
			return (m_State != null) ? m_State.roleName : string.Empty;
		}
	}

	public int CharGp
	{
		get
		{
			return (m_State != null) ? m_State.GetCash() : 0;
		}
	}

	public string CharQuest
	{
		get
		{
			return GameApp.GetInstance().GetUserState().m_questStateContainer.GetName(m_State.currentQuest);
		}
	}

	public GameObject CharAvatar
	{
		get
		{
			return (m_State != null) ? AvatarBuilder.GetInstance().CreateUIAvatarWithWeapon(m_State, false) : null;
		}
	}

	public UserState.RoleState State
	{
		get
		{
			return m_State;
		}
	}

	private UICharacterData()
	{
	}

	public static UICharacterData CreateEmpty()
	{
		UICharacterData uICharacterData = new UICharacterData();
		uICharacterData.m_State = null;
		return uICharacterData;
	}

	public static UICharacterData CreateData(UserState.RoleState state)
	{
		UICharacterData uICharacterData = new UICharacterData();
		uICharacterData.m_State = state;
		return uICharacterData;
	}
}

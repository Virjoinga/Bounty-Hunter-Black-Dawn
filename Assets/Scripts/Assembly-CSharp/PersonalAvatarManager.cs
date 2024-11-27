using System.Collections.Generic;
using UnityEngine;

public class PersonalAvatarManager
{
	private UserState userState;

	private List<Avatar> avatarList;

	public PersonalAvatarManager(UserState userState, List<Avatar> avatarList)
	{
		this.userState = userState;
		this.avatarList = new List<Avatar>();
		foreach (Avatar avatar in avatarList)
		{
			if (avatar.CharClass == userState.GetCharacterClass() && avatar.CharSex == userState.GetSex())
			{
				this.avatarList.Add(avatar);
			}
		}
		Refresh();
	}

	public List<Avatar> GetList()
	{
		return avatarList;
	}

	private void Refresh()
	{
		for (int i = 0; i < avatarList.Count; i++)
		{
			Debug.Log("i : " + i);
			avatarList[i].SetState((Avatar.State)userState.GetAllAvatar()[i]);
		}
	}

	public bool Purchase(int index)
	{
		if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(avatarList[index].Price))
		{
			Set(index, Avatar.State.Purchased);
			return true;
		}
		return false;
	}

	public void Equip(int index)
	{
		for (int i = 0; i < avatarList.Count; i++)
		{
			if (avatarList[i].CurrentState == Avatar.State.Equipped)
			{
				Set(i, Avatar.State.Purchased);
				break;
			}
		}
		Set(index, Avatar.State.Equipped);
		userState.SetAvatar((byte)index);
	}

	private void Set(int index, Avatar.State state)
	{
		userState.GetAllAvatar()[index] = (byte)state;
		Refresh();
	}
}

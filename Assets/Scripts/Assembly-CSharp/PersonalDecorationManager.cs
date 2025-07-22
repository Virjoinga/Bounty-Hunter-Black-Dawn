using System.Collections.Generic;

public class PersonalDecorationManager
{
	private UserState userState;

	private List<Decoration> headList;

	private List<Decoration> faceList;

	private List<Decoration> waistList;

	public PersonalDecorationManager(UserState userState, List<Decoration> headList, List<Decoration> faceList, List<Decoration> waistList)
	{
		this.userState = userState;
		this.headList = headList;
		this.faceList = faceList;
		this.waistList = waistList;
		Refresh();
	}

	private void Refresh()
	{
		Refresh(userState.GetDecorationHead(), headList, Global.TOTAL_ARMOR_HEAD_NUM);
		Refresh(userState.GetDecorationFace(), faceList, Global.TOTAL_ARMOR_FACE_NUM);
		Refresh(userState.GetDecorationWaist(), waistList, Global.TOTAL_ARMOR_WAIST_NUM);
	}

	private void Refresh(byte[] decorationsInfo, List<Decoration> decorationList, int total)
	{
		for (int i = 0; i < total; i++)
		{
			if (i < decorationList.Count)
			{
				decorationList[i].SetState((Decoration.State)decorationsInfo[i]);
			}
		}
	}

	public List<Decoration> GetDecorationList(int part)
	{
		List<Decoration> result = null;
		switch (part)
		{
		case 0:
			result = headList;
			break;
		case 1:
			result = faceList;
			break;
		case 2:
			result = waistList;
			break;
		}
		return result;
	}

	public void Purchase(int part, int index)
	{
		List<Decoration> decorationList = GetDecorationList(part);
		if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(decorationList[index].Price))
		{
			SetDecorations(part, index, Decoration.State.Purchased);
			Refresh();
		}
	}

	public void Equip(int part, int index)
	{
		Unequip(part);
		byte[] decoration = userState.GetDecoration();
		decoration[part] = (byte)(index + 1);
		userState.SetDecoration(decoration);
		SetDecorations(part, index, Decoration.State.Equipped);
		Refresh();
	}

	public void Unequip(int part)
	{
		byte[] decoration = userState.GetDecoration();
		if (decoration[part] > 0)
		{
			SetDecorations(part, decoration[part] - 1, Decoration.State.Purchased);
			decoration[part] = 0;
			userState.SetDecoration(decoration);
		}
		Refresh();
	}

	private void SetDecorations(int part, int index, Decoration.State state)
	{
		byte[] array = null;
		switch (part)
		{
		case 0:
			array = userState.GetDecorationHead();
			array[index] = (byte)state;
			userState.SetDecorationHead(array);
			break;
		case 1:
			array = userState.GetDecorationFace();
			array[index] = (byte)state;
			userState.SetDecorationFace(array);
			break;
		case 2:
			array = userState.GetDecorationWaist();
			array[index] = (byte)state;
			userState.SetDecorationWaist(array);
			break;
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class UIVSUserInfoManager : MonoBehaviour
{
	public GameObject userContainer;

	private UIVSUserInfo[] infoList;

	private void Awake()
	{
		infoList = userContainer.GetComponentsInChildren<UIVSUserInfo>(true);
	}

	public void UpdatePlayer(Dictionary<string, UserStateHUD.GameUnitHUD> dic)
	{
		List<UserStateHUD.VSUserState> list = new List<UserStateHUD.VSUserState>();
		foreach (KeyValuePair<string, UserStateHUD.GameUnitHUD> item in dic)
		{
			list.Add(UIVSUserInfo.CreateInfo(item.Value));
		}
		UpdatePlayer(list);
	}

	public void UpdatePlayer(List<UserStateHUD.VSUserState> list)
	{
		for (int i = 0; i < infoList.Length; i++)
		{
			if (i < list.Count)
			{
				if (!infoList[i].gameObject.activeSelf)
				{
					infoList[i].gameObject.SetActive(true);
				}
				infoList[i].UpdateInfo(list[i]);
			}
			else if (infoList[i].gameObject.activeSelf)
			{
				infoList[i].gameObject.SetActive(false);
			}
		}
	}
}

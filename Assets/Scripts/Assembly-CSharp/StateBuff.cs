using System.Collections.Generic;
using UnityEngine;

public class StateBuff : MonoBehaviour
{
	[SerializeField]
	private UIGridX grid;

	[SerializeField]
	private StateBuffIcon clone;

	private List<StateBuffIcon> m_BuffList = new List<StateBuffIcon>();

	private void OnEnable()
	{
		NGUITools.SetActive(clone.gameObject, false);
	}

	private void Update()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		UserStateHUD.UserBuffHUD[] userBuffs = UserStateHUD.GetInstance().GetUserBuffs();
		UserStateHUD.UserBuffHUD[] array = userBuffs;
		foreach (UserStateHUD.UserBuffHUD userBuffHUD in array)
		{
			bool flag = false;
			foreach (StateBuffIcon buff in m_BuffList)
			{
				if ((buff.name == null && userBuffHUD.Name == null) || buff.name == userBuffHUD.Name)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				StateBuffIcon stateBuffIcon = Object.Instantiate(clone) as StateBuffIcon;
				NGUITools.SetActive(stateBuffIcon.gameObject, true);
				stateBuffIcon.transform.parent = base.transform;
				stateBuffIcon.transform.localScale = clone.transform.localScale;
				stateBuffIcon.transform.rotation = clone.transform.rotation;
				stateBuffIcon.transform.position = clone.transform.position;
				stateBuffIcon.name = userBuffHUD.Name;
				m_BuffList.Add(stateBuffIcon);
				grid.repositionNow = true;
			}
		}
		int num = 0;
		while (num < m_BuffList.Count)
		{
			bool flag2 = false;
			UserStateHUD.UserBuffHUD[] array2 = userBuffs;
			foreach (UserStateHUD.UserBuffHUD userBuffHUD2 in array2)
			{
				if ((userBuffHUD2.Name == null && m_BuffList[num].name == null) || userBuffHUD2.Name == m_BuffList[num].name)
				{
					m_BuffList[num].Time = (int)userBuffHUD2.Time;
					flag2 = true;
					num++;
					break;
				}
			}
			if (!flag2)
			{
				Object.Destroy(m_BuffList[num].gameObject);
				m_BuffList.RemoveAt(num);
				grid.repositionNow = true;
			}
		}
	}
}

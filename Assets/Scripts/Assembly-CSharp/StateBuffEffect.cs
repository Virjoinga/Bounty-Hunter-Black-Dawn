using System.Collections.Generic;
using UnityEngine;

public class StateBuffEffect : MonoBehaviour
{
	public GameObject m_Avatar;

	public GameObject m_Morphine;

	private List<GameObject> m_EffectList = new List<GameObject>();

	private float mLastUpdateTime;

	private void Awake()
	{
		m_EffectList.Add(m_Avatar);
		m_EffectList.Add(m_Morphine);
	}

	private void OnEnable()
	{
		mLastUpdateTime = Time.time;
	}

	private void Update()
	{
		if (!(Time.time - mLastUpdateTime > 0.15f))
		{
			return;
		}
		mLastUpdateTime = Time.time;
		UserStateHUD.UserBuffHUD[] userBuffs = UserStateHUD.GetInstance().GetUserBuffs();
		bool[] array = new bool[m_EffectList.Count];
		UserStateHUD.UserBuffHUD[] array2 = userBuffs;
		foreach (UserStateHUD.UserBuffHUD userBuffHUD in array2)
		{
			if (userBuffHUD.BuffEffect != UserStateHUD.UserBuffHUD.FullScreenEffect.None)
			{
				array[(int)userBuffHUD.BuffEffect] = true;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] && !m_EffectList[j].activeSelf)
			{
				m_EffectList[j].SetActive(true);
			}
			else if (!array[j] && m_EffectList[j].activeSelf)
			{
				m_EffectList[j].SetActive(false);
			}
		}
	}
}

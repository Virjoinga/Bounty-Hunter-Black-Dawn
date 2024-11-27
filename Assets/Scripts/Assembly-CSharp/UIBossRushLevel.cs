using System.Collections.Generic;
using UnityEngine;

public class UIBossRushLevel : MonoBehaviour
{
	[SerializeField]
	private UIBossRushRewards[] rewards;

	private List<UIBossRushRewards> rewardsList = new List<UIBossRushRewards>();

	private void Awake()
	{
		UIBossRushRewards[] array = rewards;
		foreach (UIBossRushRewards item in array)
		{
			rewardsList.Add(item);
		}
	}

	public List<UIBossRushRewards> GetRewardList()
	{
		return rewardsList;
	}
}

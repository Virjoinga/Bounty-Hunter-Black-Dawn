using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorItemBlockScript : MonoBehaviour
{
	protected List<ExploreItemStates> ItemExplorableList = new List<ExploreItemStates>();

	public byte BlockID { get; set; }

	public List<ExploreItemStates> GetList()
	{
		return ItemExplorableList;
	}

	private IEnumerator Start()
	{
		while (!InGameUIScript.bInited)
		{
			yield return 0;
		}
		string[] nameArr = base.gameObject.name.Split('_');
		BlockID = Convert.ToByte(nameArr[nameArr.Length - 1].Split('(')[0]);
		Debug.Log("Explore BlockID: " + BlockID);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (!GameApp.GetInstance().GetGameWorld().HasExploreBlock(BlockID))
			{
				RequireExploreItemBlockRequest requireRequest = new RequireExploreItemBlockRequest(BlockID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(requireRequest);
			}
			else
			{
				RefreshExplorableStates();
			}
		}
		else if (!GameApp.GetInstance().GetGameWorld().HasExploreBlock(BlockID))
		{
			Debug.Log("Init Explore Block");
			Init();
			CreateExplores();
			RefreshExplorableStates();
		}
		else
		{
			UpdateExplore();
		}
	}

	public void Init()
	{
		ItemExplorableList.Clear();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			ItemExplorableList.Add(ExploreItemStates.EFFECT_INVISIBLE);
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			ExplorItemList component = gameObject.GetComponent<ExplorItemList>();
			GameApp.GetInstance().GetGameWorld().AddExploreItem(BlockID, component.ID_in_Block, ItemExplorableList[i], (short)component.m_questId);
		}
	}

	public void CreateExplores()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			ExplorItemList component = gameObject.GetComponent<ExplorItemList>();
			if (GameApp.GetInstance().GetUserState().m_questStateContainer.IsAciveQuest(component.m_questId))
			{
				component.EffectObject.SetActiveRecursively(false);
				float num = UnityEngine.Random.Range(0f, 1f);
				if (num < component.AppearRates[0])
				{
					ItemExplorableList[i] = ExploreItemStates.EFFECT_VISIBLE;
				}
			}
			else
			{
				gameObject.SetActiveRecursively(false);
				ItemExplorableList[i] = ExploreItemStates.OBJECT_DISABLE;
			}
			GameApp.GetInstance().GetGameWorld().AddExploreItem(BlockID, component.ID_in_Block, ItemExplorableList[i], (short)component.m_questId);
			QuestManager.GetInstance().m_questLst[component.m_questId].SetQuestPoint(component.ExplorItemIDs[0], component.m_posId, QuestPointState.enable);
			gameObject.GetComponent<PlayMakerFSM>().enabled = true;
		}
	}

	private void OnDisable()
	{
	}

	public void UpdateExplore()
	{
		ExploreItemBlockInfo explorItemBlock = GameApp.GetInstance().GetGameWorld().GetExplorItemBlock(BlockID);
		if (explorItemBlock == null)
		{
			Debug.Log("UpdateExplore block is null: " + BlockID);
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			ExplorItemList component = gameObject.GetComponent<ExplorItemList>();
			Debug.Log("UpdateExplore i: " + i);
			if (explorItemBlock.mExplorableStateDictionary.ContainsKey(component.ID_in_Block) && explorItemBlock.mExplorableStateDictionary[component.ID_in_Block].mState == ExploreItemStates.OBJECT_DISABLE && GameApp.GetInstance().GetUserState().m_questStateContainer.IsAciveQuest(component.m_questId))
			{
				Debug.Log("explorItemList.m_questId: " + component.m_questId);
				component.EffectObject.SetActiveRecursively(false);
				float num = UnityEngine.Random.Range(0f, 1f);
				if (num < component.AppearRates[0])
				{
					GameApp.GetInstance().GetGameWorld().AddExploreItem(BlockID, component.ID_in_Block, ExploreItemStates.EFFECT_VISIBLE, (short)component.m_questId);
					QuestManager.GetInstance().m_questLst[component.m_questId].SetQuestPoint(component.ExplorItemIDs[0], component.m_posId, QuestPointState.enable);
					gameObject.GetComponent<PlayMakerFSM>().enabled = true;
				}
			}
		}
		RefreshExplorableStates();
	}

	public void RefreshExplorableStates()
	{
		ExploreItemBlockInfo explorItemBlock = GameApp.GetInstance().GetGameWorld().GetExplorItemBlock(BlockID);
		if (explorItemBlock.mExplorableStateDictionary.Count == 0)
		{
			Debug.Log("no explore item?");
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			gameObject.GetComponent<PlayMakerFSM>().enabled = true;
			ExplorItemList component = gameObject.GetComponent<ExplorItemList>();
			component.EffectObject.SetActiveRecursively(false);
			if (explorItemBlock.mExplorableStateDictionary[component.ID_in_Block].mState == ExploreItemStates.EFFECT_VISIBLE)
			{
				if (!component.gameObject.activeSelf)
				{
					component.gameObject.SetActive(true);
				}
				component.EffectObject.SetActiveRecursively(true);
			}
			else if (explorItemBlock.mExplorableStateDictionary[component.ID_in_Block].mState == ExploreItemStates.OBJECT_DISABLE)
			{
				component.gameObject.SetActiveRecursively(false);
			}
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class UIBubble : MonoBehaviour
{
	public static UIBubble m_instance;

	public GameObject m_normal;

	public GameObject m_topics;

	public static string m_text = string.Empty;

	public static short m_npcId;

	public static NpcBubbleState m_bubbleState;

	private bool m_playing;

	protected QuestScript m_npc;

	public GameObject m_template;

	private TopicData m_currTopic;

	private List<TopicData> m_topicsData = new List<TopicData>();

	private void Awake()
	{
		m_normal.SetActive(false);
		m_topics.SetActive(false);
		m_template.SetActive(false);
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		m_npc = gameScene.GetQuestScript(m_npcId);
		m_instance = this;
	}

	private void Start()
	{
	}

	public void Init()
	{
		m_topicsData.Clear();
		List<TopicType> topicData = m_npc.GetTopicData();
		List<QuestScript.QuestData> questData = m_npc.GetQuestData();
		m_topicsData.Clear();
		foreach (TopicType item in topicData)
		{
			TopicData topicData2 = new TopicData();
			topicData2.m_type = item;
			m_topicsData.Add(topicData2);
		}
		UserState userState = GameApp.GetInstance().GetUserState();
		int count = questData.Count;
		for (int i = 0; i < count; i++)
		{
			int id = questData[i].m_id;
			int commonId = questData[i].m_commonId;
			Quest quest = QuestManager.GetInstance().m_questLst[id];
			if ((questData[i].m_type == QuestScript.QuestAccType.Assign || questData[i].m_type == QuestScript.QuestAccType.AssignAndSubmit) && userState.m_questStateContainer.CheckCanBeAccepted(id))
			{
				TopicData topicData3 = new TopicData();
				topicData3.m_type = TopicType.Mission;
				m_topicsData.Add(topicData3);
				break;
			}
			if ((questData[i].m_type == QuestScript.QuestAccType.Submit || questData[i].m_type == QuestScript.QuestAccType.AssignAndSubmit) && userState.m_questStateContainer.CheckHasBeenAcceptedWithCommonID(commonId))
			{
				TopicData topicData4 = new TopicData();
				topicData4.m_type = TopicType.Mission;
				m_topicsData.Add(topicData4);
				break;
			}
		}
	}

	public void Logic()
	{
		Debug.Log("m_topicsData.Count: " + m_topicsData.Count);
		bool flag = false;
		string text = string.Empty;
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (QuestState item in userState.m_questStateContainer.m_accStateLst)
		{
			Quest quest = item.m_quest;
			if (quest.m_questType != QuestType.Messenger && quest.m_questType != QuestType.Dialog)
			{
				continue;
			}
			foreach (KeyValuePair<int, QuestState.Conter> item2 in item.m_conter)
			{
				if (item2.Value.m_type != QuestConterType.NPC || item2.Value.m_id != m_npcId)
				{
					continue;
				}
				QuestState.Conter value = item2.Value;
				if (value.m_currNum >= value.m_maxNum)
				{
					continue;
				}
				flag = true;
				if (quest.m_questType == QuestType.Messenger)
				{
					text = LocalizationManager.GetInstance().GetString(((MessengerQuest)quest).m_context);
				}
				else if (quest.m_questType == QuestType.Dialog)
				{
					text = LocalizationManager.GetInstance().GetString(((DialogQuest)quest).m_context);
				}
				break;
			}
		}
		if (flag)
		{
			m_text = text;
			m_bubbleState = NpcBubbleState.QUEST_TALK_STATE;
			m_topics.SetActive(false);
			m_normal.SetActive(true);
		}
		else if (m_topicsData.Count == 1)
		{
			SetCurrentTopic(0);
			ShowTopic();
		}
		else if (m_topicsData.Count > 1)
		{
			ShowTopics();
		}
		else
		{
			InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
			inGameUIScript.FrGoToPhase(6, false, false, false);
		}
	}

	public void ShowTopics()
	{
		if (m_topics != null)
		{
			NGUITools.SetActive(m_topics, true);
		}
	}

	public void SetCurrentTopic(int index)
	{
		m_currTopic = m_topicsData[index];
	}

	public void ShowTopic()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		Debug.Log("m_currTopic.m_type: " + m_currTopic.m_type);
		if (m_currTopic.m_type == TopicType.Talk)
		{
			string dialog = userState.m_npcState.GetDialog(m_npc.gameObject.name);
			string text = dialog.Replace("[s]", "|");
			string[] array = text.Split('|');
			m_text = LocalizationManager.GetInstance().GetString(array[Random.Range(0, array.Length)]);
			m_npcId = (short)m_npc.GetNpcId();
			m_bubbleState = NpcBubbleState.TALK_STATE;
			m_topics.SetActive(false);
			m_normal.SetActive(true);
		}
		else if (m_currTopic.m_type == TopicType.Trade)
		{
			InGameMenuManager.GetInstance().ShowMenuForShop();
		}
		else if (m_currTopic.m_type == TopicType.Mission)
		{
			InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
			UIQuest.m_npcId = m_npcId;
			inGameUIScript.FrGoToPhase(17, false, false, false);
		}
		else if (m_currTopic.m_type == TopicType.Arena)
		{
			UIArena.Show();
		}
	}

	public void Show()
	{
		m_playing = true;
	}

	public void Close()
	{
		m_playing = false;
	}

	public bool GetState()
	{
		return m_playing;
	}

	private void OnDestroy()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
			if (localPlayer != null)
			{
				localPlayer.SetState(Player.IDLE_STATE);
			}
		}
		m_instance = null;
	}

	private void Update()
	{
	}

	public List<TopicData> GetTopics()
	{
		return m_topicsData;
	}

	public void StartTurnRound()
	{
		m_npc.GetNpc().StartTurnRoundTime();
	}

	public void SetNpcState(NpcBubbleState state)
	{
		switch (state)
		{
		case NpcBubbleState.ASSIGNABLE_STATE:
			m_npc.GetNpc().PlayAnimationAllLayers(AnimationString.NPC_ASSIGNABLE, WrapMode.ClampForever);
			m_npc.GetNpc().State = Npc.ASSIGNABLE_STATE;
			break;
		case NpcBubbleState.ASSIGNED_STATE:
			m_npc.GetNpc().PlayAnimationAllLayers(AnimationString.NPC_ASSIGNED, WrapMode.ClampForever);
			m_npc.GetNpc().State = Npc.ASSIGNED_STATE;
			break;
		default:
			m_npc.GetNpc().PlayAnimationAllLayers(AnimationString.NPC_TALK, WrapMode.ClampForever);
			m_npc.GetNpc().State = Npc.TALK_STATE;
			break;
		}
	}

	public void UpdateFlag()
	{
		m_npc.UpdateFlag();
	}
}

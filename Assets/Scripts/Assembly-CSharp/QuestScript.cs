using System.Collections.Generic;
using UnityEngine;

public class QuestScript : MonoBehaviour
{
	public enum QuestAccType
	{
		None = 0,
		Assign = 1,
		Submit = 2,
		AssignAndSubmit = 3
	}

	public enum AcceptFrom
	{
		Npc = 0,
		Board = 1
	}

	public class QuestData
	{
		public int m_id;

		public int m_commonId;

		public QuestAccType m_type;
	}

	private const string m_flag = "flag";

	public AcceptFrom m_acceptForm;

	private List<TopicType> m_topicData = new List<TopicType>();

	private List<QuestData> m_questData = new List<QuestData>();

	public GameObject m_prompt;

	private PromptType m_promtType;

	protected short m_npcId;

	private Vector3 m_promptScaleFrom = new Vector3(1f, 1f, 1f);

	private Vector3 m_promptScaleTo = new Vector3(1.5f, 1.5f, 1f);

	private Color m_talkActiveColor = new Color(40f / 51f, 0.2f, 5f / 51f, 1f);

	private Color m_talkInactiveColor = new Color(25f / 51f, 0.2f, 2f / 51f, 1f);

	private Color m_assignableActiveColor = new Color(10f / 51f, 67f / 85f, 5f / 51f, 1f);

	private Color m_assignableInactiveColor = new Color(10f / 51f, 0.6f, 2f / 51f, 1f);

	private Color m_assignedActiveColor = new Color(10f / 51f, 5f / 51f, 40f / 51f, 1f);

	private Color m_assignedInactiveColor = new Color(5f / 51f, 5f / 51f, 25f / 51f, 1f);

	private string m_promptAnimName = "RPG_anim_061";

	private bool m_promptInit;

	private byte m_promptState;

	private void Start()
	{
		Init();
		UpdateFlag();
		m_promptInit = true;
	}

	private void Init()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		m_npcId = userState.m_npcState.GetId(base.name);
		m_questData.Clear();
		Dictionary<int, Quest> questLst = QuestManager.GetInstance().m_questLst;
		foreach (int key in questLst.Keys)
		{
			QuestAccType questAccType = QuestAccType.None;
			if (questLst[key].m_accQuestNpcId == m_npcId && questLst[key].m_finQuestNpcId == m_npcId)
			{
				questAccType = QuestAccType.AssignAndSubmit;
			}
			else if (questLst[key].m_accQuestNpcId == m_npcId)
			{
				questAccType = QuestAccType.Assign;
			}
			else if (questLst[key].m_finQuestNpcId == m_npcId)
			{
				questAccType = QuestAccType.Submit;
			}
			if (questAccType != 0)
			{
				QuestData questData = new QuestData();
				questData.m_id = questLst[key].m_id;
				questData.m_commonId = questLst[key].m_commonId;
				questData.m_type = questAccType;
				m_questData.Add(questData);
			}
		}
		m_topicData.Clear();
		if (!string.IsNullOrEmpty(userState.m_npcState.GetDialog(base.name)))
		{
			m_topicData.Add(TopicType.Talk);
		}
		if (userState.m_npcState.GetType(base.name) == NpcType.Businessman)
		{
			m_topicData.Add(TopicType.Trade);
		}
		else if (userState.m_npcState.GetType(base.name) == NpcType.Barkeeper)
		{
			m_topicData.Add(TopicType.Arena);
		}
		Transform transform = base.transform.Find("Name");
		if (transform != null && transform.GetChildCount() == 0)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("NpcName", "Name");
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.transform.parent = transform;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "NameSign";
			UIPlayerName component = gameObject.GetComponent<UIPlayerName>();
			if (component != null)
			{
				component.SetName(userState.m_npcState.GetDisplayName(base.name), new Color(0f, 62f / 85f, 0.12156863f, 1f));
			}
		}
	}

	public int GetNpcId()
	{
		return m_npcId;
	}

	public List<QuestData> GetQuestData()
	{
		return m_questData;
	}

	public List<TopicType> GetTopicData()
	{
		return m_topicData;
	}

	public Npc GetNpc()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return null;
		}
		return gameScene.GetNpc(m_npcId);
	}

	public PromptType GetPromptType()
	{
		return m_promtType;
	}

	public void SetPrompt(PromptType type)
	{
		m_promptInit = true;
		if (m_promtType == type)
		{
			return;
		}
		m_promtType = type;
		if (m_prompt.transform.childCount > 0)
		{
			GameObject gameObject = m_prompt.transform.Find("flag").gameObject;
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
		switch (type)
		{
		case PromptType.Assignable:
		{
			GameObject original4 = Resources.Load("RPG_effect/RPG_FH_003") as GameObject;
			GameObject gameObject5 = Object.Instantiate(original4) as GameObject;
			gameObject5.transform.parent = m_prompt.transform;
			gameObject5.transform.localPosition = Vector3.zero;
			gameObject5.name = "flag";
			break;
		}
		case PromptType.Assigned:
		{
			GameObject original3 = Resources.Load("RPG_effect/RPG_FH_002") as GameObject;
			GameObject gameObject4 = Object.Instantiate(original3) as GameObject;
			gameObject4.transform.parent = m_prompt.transform;
			gameObject4.transform.localPosition = Vector3.zero;
			gameObject4.name = "flag";
			break;
		}
		case PromptType.CanSubmit:
		{
			GameObject original2 = Resources.Load("RPG_effect/RPG_FH_001") as GameObject;
			GameObject gameObject3 = Object.Instantiate(original2) as GameObject;
			gameObject3.transform.parent = m_prompt.transform;
			gameObject3.transform.localPosition = Vector3.zero;
			gameObject3.name = "flag";
			break;
		}
		case PromptType.Talk:
		{
			GameObject original = Resources.Load("RPG_effect/RPG_FH_005") as GameObject;
			GameObject gameObject2 = Object.Instantiate(original) as GameObject;
			gameObject2.transform.parent = m_prompt.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.name = "flag";
			break;
		}
		case PromptType.None:
			break;
		}
	}

	private void SetPromptTweenScale(Vector3 from, Vector3 to)
	{
		m_promptScaleFrom = from;
		m_promptScaleTo = to;
	}

	private void Update()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		Transform transform = localPlayer.GetTransform();
		if (!(transform != null))
		{
			return;
		}
		GameObject gameObject = base.gameObject;
		Vector3 position = gameObject.transform.position;
		if (m_prompt.transform.GetChildCount() <= 0)
		{
			return;
		}
		if (Vector3.Distance(transform.position, position) <= 5f)
		{
			if (m_promptState == 1 || m_promptInit)
			{
				m_promptInit = false;
				TweenScale.Begin(m_prompt, 0.2f, m_promptScaleTo);
				GameObject gameObject2 = m_prompt.transform.Find("flag").gameObject;
				GameObject gameObject3 = gameObject2.transform.GetChild(0).gameObject;
				gameObject3.GetComponent<Animation>()[m_promptAnimName].wrapMode = WrapMode.Loop;
				gameObject3.GetComponent<Animation>().Play(m_promptAnimName);
			}
			m_promptState = 0;
		}
		else
		{
			if (m_promptState == 0 || m_promptInit)
			{
				m_promptInit = false;
				TweenScale.Begin(m_prompt, 0.2f, m_promptScaleFrom);
				GameObject gameObject4 = m_prompt.transform.Find("flag").gameObject;
				GameObject gameObject5 = gameObject4.transform.GetChild(0).gameObject;
				gameObject5.GetComponent<Animation>().Stop();
			}
			m_promptState = 1;
		}
	}

	public bool UpdateFlag()
	{
		Debug.Log(base.gameObject.name);
		UserState userState = GameApp.GetInstance().GetUserState();
		PromptType promptType = PromptType.None;
		if (userState.m_npcState.GetState(base.name).Equals("Talk"))
		{
			SetPrompt(PromptType.Talk);
			return false;
		}
		foreach (QuestState item in userState.m_questStateContainer.m_accStateLst)
		{
			Quest quest = item.m_quest;
			if (quest.m_questType != QuestType.Messenger && quest.m_questType != QuestType.Dialog)
			{
				continue;
			}
			foreach (KeyValuePair<int, QuestState.Conter> item2 in item.m_conter)
			{
				if (item2.Value.m_type == QuestConterType.NPC && item2.Value.m_id == m_npcId)
				{
					QuestState.Conter value = item2.Value;
					if (value.m_currNum < value.m_maxNum)
					{
						SetPrompt(PromptType.Talk);
						return false;
					}
				}
			}
		}
		for (int i = 0; i < m_questData.Count; i++)
		{
			int id = m_questData[i].m_id;
			int commonId = m_questData[i].m_commonId;
			switch (m_questData[i].m_type)
			{
			case QuestAccType.AssignAndSubmit:
				if (userState.m_questStateContainer.CheckCanBeSubmitted(id))
				{
					SetPrompt(PromptType.CanSubmit);
					return true;
				}
				if (userState.m_questStateContainer.CheckHasBeenAcceptedWithCommonID(commonId) && promptType != PromptType.Assignable)
				{
					promptType = PromptType.Assigned;
				}
				else if (userState.m_questStateContainer.CheckCanBeAccepted(id))
				{
					promptType = PromptType.Assignable;
				}
				break;
			case QuestAccType.Submit:
				if (userState.m_questStateContainer.CheckCanBeSubmitted(id))
				{
					SetPrompt(PromptType.CanSubmit);
					return true;
				}
				if (userState.m_questStateContainer.CheckHasBeenAcceptedWithCommonID(commonId) && promptType != PromptType.Assignable)
				{
					promptType = PromptType.Assigned;
				}
				break;
			case QuestAccType.Assign:
				if (userState.m_questStateContainer.CheckCanBeAccepted(id))
				{
					promptType = PromptType.Assignable;
				}
				break;
			}
		}
		SetPrompt(promptType);
		return false;
	}
}

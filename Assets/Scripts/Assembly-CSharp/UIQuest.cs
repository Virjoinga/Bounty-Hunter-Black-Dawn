using System;
using System.Collections.Generic;
using UnityEngine;

public class UIQuest : UIGameMenuNormal
{
	public static UIQuest m_instance;

	public static short m_npcId;

	public GameObject m_quests;

	public GameObject m_logs;

	public GameObject m_teamQuest;

	public GameObject m_bonus;

	public GameObject m_template;

	public GameObject m_subDetails;

	public GameObject m_subObjectives;

	public GameObject m_subReward;

	protected QuestScript m_npc;

	protected override void Awake()
	{
		base.Awake();
		m_quests.SetActive(false);
		m_logs.SetActive(false);
		m_teamQuest.SetActive(false);
		m_template.SetActive(false);
		m_bonus.SetActive(false);
		m_instance = this;
	}

	private void Start()
	{
	}

	protected override byte InitMask()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		m_instance = null;
	}

	private void Update()
	{
	}

	public void ShowQuests()
	{
		m_quests.SetActive(true);
		Show(3);
	}

	public void ShowLogs()
	{
		m_logs.SetActive(true);
		Show(7);
	}

	public void ShowTeamQuest()
	{
		m_teamQuest.SetActive(true);
		Show(7);
	}

	public bool IsActived()
	{
		return false;
	}

	public void SetQuestNpc(short npcId)
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		m_npc = gameScene.GetQuestScript(npcId);
	}

	public void SetQuestNpc(GameObject npc)
	{
		m_npc = npc.GetComponent<QuestScript>();
	}

	public List<QuestScript.QuestData> GetQuestData()
	{
		return m_npc.GetQuestData();
	}

	public QuestScript.AcceptFrom GetAcceptFrom()
	{
		return m_npc.m_acceptForm;
	}

	public void AddDetails(string descStr, Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(m_subDetails) as GameObject;
		gameObject.transform.parent = parent;
		Transform trans = gameObject.transform;
		UIDragPanelContentsAlign component = gameObject.GetComponent<UIDragPanelContentsAlign>();
		component.draggablePanel = parent.parent.GetComponent<UIDraggablePanelAlign>();
		GameObject gameObject2 = gameObject.transform.Find("Label").gameObject;
		UILabel component2 = gameObject2.GetComponent<UILabel>();
		component2.text = descStr;
		GameObject gameObject3 = gameObject.transform.Find("Title").gameObject;
		UILabel component3 = gameObject3.GetComponent<UILabel>();
		component3.text = LocalizationManager.GetInstance().GetString("LOC_UI_DETAILS");
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		BoxCollider boxCollider = gameObject.GetComponent<Collider>() as BoxCollider;
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(trans);
		boxCollider.center = new Vector3(bounds.center.x, bounds.center.y, 0f);
		boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
	}

	public void AddObjective(string objective, Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(m_subObjectives) as GameObject;
		gameObject.transform.parent = parent;
		Transform trans = gameObject.transform;
		UIDragPanelContentsAlign component = gameObject.GetComponent<UIDragPanelContentsAlign>();
		component.draggablePanel = parent.parent.GetComponent<UIDraggablePanelAlign>();
		GameObject gameObject2 = gameObject.transform.Find("Label").gameObject;
		UILabel component2 = gameObject2.GetComponent<UILabel>();
		component2.text = objective;
		GameObject gameObject3 = gameObject.transform.Find("Title").gameObject;
		UILabel component3 = gameObject3.GetComponent<UILabel>();
		component3.text = LocalizationManager.GetInstance().GetString("LOC_UI_OBJECTIVE");
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		BoxCollider boxCollider = gameObject.GetComponent<Collider>() as BoxCollider;
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(trans);
		boxCollider.center = new Vector3(bounds.center.x, bounds.center.y, 0f);
		boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
	}

	public void AddRewards(Quest quest, Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(m_subReward) as GameObject;
		gameObject.transform.parent = parent;
		Transform trans = gameObject.transform;
		UIDragPanelContentsAlign component = gameObject.GetComponent<UIDragPanelContentsAlign>();
		component.draggablePanel = parent.parent.GetComponent<UIDraggablePanelAlign>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[quest.m_commonId];
		foreach (Quest item in list)
		{
			QuestAward questAward = (QuestAward)item.m_award;
			num += questAward.m_exp;
			num2 += questAward.m_cash;
			num3 += questAward.m_mithril;
		}
		GameObject gameObject2 = gameObject.transform.Find("Title").gameObject;
		UILabel component2 = gameObject2.GetComponent<UILabel>();
		component2.text = LocalizationManager.GetInstance().GetString("LOC_UI_REWARD");
		GameObject gameObject3 = gameObject.transform.Find("Exp").gameObject;
		UILabel component3 = gameObject3.GetComponent<UILabel>();
		component3.text = "[ffba00]" + LocalizationManager.GetInstance().GetString("EXP:") + "[-][ffeea0]" + Convert.ToString(num) + "[-]";
		GameObject gameObject4 = gameObject.transform.Find("GP").gameObject;
		UILabel component4 = gameObject4.GetComponent<UILabel>();
		component4.text = "[ffba00]" + LocalizationManager.GetInstance().GetString("GP:") + "[-][ffeea0]" + Convert.ToString(num2) + "[-]";
		GameObject gameObject5 = gameObject.transform.Find("MITHRIL").gameObject;
		UILabel component5 = gameObject5.GetComponent<UILabel>();
		component5.text = "[ffba00]" + LocalizationManager.GetInstance().GetString("MITHRIL:") + "[-][ffeea0]" + Convert.ToString(num3) + "[-]";
		GameObject gameObject6 = gameObject.transform.Find("Item").gameObject;
		float num5 = 0f;
		foreach (Quest item2 in list)
		{
			QuestAward questAward2 = (QuestAward)item2.m_award;
			foreach (QuestAward.ItemAward item3 in questAward2.m_itemAwardLst)
			{
				if (item3.m_itemNum > 0)
				{
					GameObject iconForSpecialItem = GameApp.GetInstance().GetLootManager().GetIconForSpecialItem(item3.m_itemId, item3.m_itemNameNum);
					UIPanel component6 = iconForSpecialItem.GetComponent<UIPanel>();
					UnityEngine.Object.Destroy(component6);
					iconForSpecialItem.transform.parent = gameObject6.transform;
					iconForSpecialItem.transform.localPosition = new Vector3(num5, 0f, 0f);
					iconForSpecialItem.transform.localScale = Vector3.one;
					num5 -= 85f;
				}
			}
		}
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		BoxCollider boxCollider = gameObject.GetComponent<Collider>() as BoxCollider;
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(trans);
		boxCollider.center = new Vector3(bounds.center.x, bounds.center.y, 0f);
		boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
	}

	public Color GetColorForQuestDifficulty(int level)
	{
		int num = level - GameApp.GetInstance().GetUserState().GetCharLevel();
		if (num <= -2)
		{
			return UIConstant.COLOR_QUEST_DIFFICULTY[0];
		}
		if (num <= -1)
		{
			return UIConstant.COLOR_QUEST_DIFFICULTY[1];
		}
		switch (num)
		{
		case 0:
			return UIConstant.COLOR_QUEST_DIFFICULTY[2];
		case 1:
			return UIConstant.COLOR_QUEST_DIFFICULTY[3];
		default:
			return UIConstant.COLOR_QUEST_DIFFICULTY[4];
		}
	}

	public void UpdateFlag()
	{
		m_npc.UpdateFlag();
	}
}

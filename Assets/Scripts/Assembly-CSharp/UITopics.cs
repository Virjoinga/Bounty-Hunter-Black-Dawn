using System;
using System.Collections.Generic;
using UnityEngine;

public class UITopics : MonoBehaviour
{
	public GameObject m_missionTemplate;

	public GameObject m_storeTemplate;

	public GameObject m_talkTemplate;

	public GameObject m_arenaTemplate;

	public GameObject m_container;

	public GameObject m_background;

	public GameObject m_close;

	public GameObject m_collider;

	protected List<GameObject> m_topics = new List<GameObject>();

	private void Start()
	{
		BoxCollider component = m_collider.GetComponent<BoxCollider>();
		component.center = Vector3.zero;
		component.size = new Vector3(Screen.width, Screen.height, 0f);
		UIEventListener uIEventListener = UIEventListener.Get(m_close);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickClose));
		UIEventListener uIEventListener2 = UIEventListener.Get(m_collider);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickClose));
	}

	private void OnEnable()
	{
		if (UIBubble.m_instance != null)
		{
			CreateTopics();
		}
	}

	private void OnDisable()
	{
		DestoryTopics();
	}

	public void CreateTopics()
	{
		DestoryTopics();
		UserState userState = GameApp.GetInstance().GetUserState();
		Transform parent = m_container.transform;
		List<TopicData> topics = UIBubble.m_instance.GetTopics();
		int count = topics.Count;
		Debug.Log("count: " + count);
		for (int i = 0; i < count; i++)
		{
			TopicData topicData = topics[i];
			GameObject gameObject = null;
			gameObject = ((topicData.m_type != TopicType.Mission) ? ((topicData.m_type != TopicType.Trade) ? ((topicData.m_type != TopicType.Arena) ? (UnityEngine.Object.Instantiate(m_talkTemplate) as GameObject) : (UnityEngine.Object.Instantiate(m_arenaTemplate) as GameObject)) : (UnityEngine.Object.Instantiate(m_storeTemplate) as GameObject)) : (UnityEngine.Object.Instantiate(m_missionTemplate) as GameObject));
			NGUITools.SetActive(gameObject, true);
			Transform transform = gameObject.transform;
			UITopic component = gameObject.transform.GetChild(0).GetComponent<UITopic>();
			component.SetId(i);
			transform.parent = parent;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			m_topics.Add(gameObject);
		}
		Vector3 localScale = m_background.transform.localScale;
		UIGrid component2 = m_container.GetComponent<UIGrid>();
		m_background.transform.localScale = new Vector3(localScale.x, localScale.y / 3f * (float)m_topics.Count, localScale.z);
		m_container.transform.localPosition = new Vector3(m_container.transform.localPosition.x, m_container.transform.localPosition.y + component2.cellHeight * (float)(m_topics.Count - 1) * 0.5f, m_container.transform.localPosition.z);
		Vector3 localScale2 = m_close.transform.GetChild(0).localScale;
		m_close.transform.localPosition = new Vector3(m_background.transform.localPosition.x + m_background.transform.localScale.x / 2f - localScale2.x * 1.5f, m_background.transform.localPosition.y + m_background.transform.localScale.y / 2f - localScale2.y, 0f);
		component2.repositionNow = true;
	}

	public void OnClickClose(GameObject go)
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		DestoryTopics();
	}

	public void DestoryTopics()
	{
		if (m_topics == null)
		{
			return;
		}
		foreach (GameObject topic in m_topics)
		{
			UnityEngine.Object.Destroy(topic);
		}
		m_topics.Clear();
	}
}

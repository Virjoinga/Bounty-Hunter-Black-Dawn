using System;
using UnityEngine;

public class UIQuestBonus : MonoBehaviour
{
	public UILabel m_exp;

	public UILabel m_gold;

	public UILabel m_mithril;

	public GameObject m_itemObj;

	private void Start()
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		component.center = Vector3.zero;
		component.size = new Vector3(Screen.width, Screen.height, -10f);
		UIEventListener uIEventListener = UIEventListener.Get(base.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickConfirm));
	}

	private void OnDisable()
	{
		if (!(m_itemObj != null))
		{
			return;
		}
		foreach (Transform item in m_itemObj.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void SetBonus(int gold, int exp, int mithril)
	{
		m_exp.text = Convert.ToString(exp);
		m_gold.text = Convert.ToString(gold);
		m_mithril.text = Convert.ToString(mithril);
	}

	public void AddItem(GameObject item)
	{
		item.transform.parent = m_itemObj.transform;
		item.transform.localPosition = Vector3.zero;
		item.transform.localRotation = Quaternion.identity;
		item.transform.localScale = Vector3.one;
	}

	public void OnClickConfirm(GameObject go)
	{
		base.gameObject.SetActive(false);
		UIQuestEntities component = UIQuest.m_instance.m_quests.GetComponent<UIQuestEntities>();
		component.Submit();
	}
}

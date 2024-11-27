using System;
using UnityEngine;

public class UINormal : MonoBehaviour
{
	private string[] m_text;

	public UITextList m_content;

	public GameObject m_next;

	public GameObject m_close;

	protected byte m_curLine;

	protected bool m_bDrag;

	private void Start()
	{
		BoxCollider component = m_next.GetComponent<BoxCollider>();
		component.center = Vector3.zero;
		component.size = new Vector3(Screen.width, Screen.height, 0f);
		UIEventListener uIEventListener = UIEventListener.Get(m_next);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickConfirm));
		UIEventListener uIEventListener2 = UIEventListener.Get(m_content.gameObject);
		uIEventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener2.onPress, new UIEventListener.BoolDelegate(OnPressPanel));
		UIEventListener uIEventListener3 = UIEventListener.Get(m_content.gameObject);
		uIEventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener3.onDrag, new UIEventListener.VectorDelegate(OnDragPanel));
		UIEventListener uIEventListener4 = UIEventListener.Get(m_close);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickClose));
		m_curLine = 0;
		m_bDrag = false;
	}

	private void OnEnable()
	{
		if (UIBubble.m_instance != null)
		{
			Debug.Log("init bubble....");
			InitText(UIBubble.m_text);
		}
	}

	public void InitText(string text)
	{
		string text2 = text.Replace("[n]", "|");
		m_text = text2.Split('|');
		m_curLine = 0;
		SetText(m_text[0]);
		UIBubble.m_instance.StartTurnRound();
	}

	public void SetText(string text)
	{
		if (m_content != null)
		{
			m_content.Clear();
			m_content.Add(text);
			UIBubble.m_instance.SetNpcState(UIBubble.m_bubbleState);
		}
	}

	public void OnClickConfirm(GameObject go)
	{
		if (m_curLine < m_text.Length - 1)
		{
			m_curLine++;
			SetText(m_text[m_curLine]);
		}
		else
		{
			OnClickClose(null);
		}
	}

	private void OnPressPanel(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			m_bDrag = false;
			return;
		}
		if (!m_bDrag)
		{
			OnClickConfirm(go);
		}
		m_bDrag = false;
	}

	private void OnDragPanel(GameObject go, Vector2 delta)
	{
		if (!m_bDrag && Mathf.Abs(delta.y) >= 10f)
		{
			m_bDrag = true;
		}
	}

	private void OnClickClose(GameObject go)
	{
		if (UIBubble.m_bubbleState == NpcBubbleState.QUEST_TALK_STATE)
		{
			GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressTalkWithNPC(UIBubble.m_npcId);
		}
		if (TutorialManager.GetInstance().IsShopTutorialOk())
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		}
		else
		{
			InGameMenuManager.GetInstance().ShowMenuForShop();
		}
		UIBubble.m_instance.SetNpcState(UIBubble.m_bubbleState);
		UIBubble.m_instance.UpdateFlag();
	}
}

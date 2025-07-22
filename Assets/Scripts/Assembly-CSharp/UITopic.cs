using UnityEngine;

public class UITopic : MonoBehaviour
{
	public UISlicedSprite m_background;

	private int m_id;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetId(int id)
	{
		m_id = id;
	}

	private void OnClick()
	{
		UIBubble.m_instance.SetCurrentTopic(m_id);
		UIBubble.m_instance.ShowTopic();
	}
}

using UnityEngine;

public class UICharacterChooseInfo : MonoBehaviour
{
	public new UILabel name;

	public UILabel gp;

	public UILabel quest;

	public GameObject avatar;

	public GameObject content;

	public GameObject warning;

	private GameObject m_Character;

	private void Update()
	{
	}

	public void ShowCharacterInfo(UICharacterData data)
	{
		if (m_Character != null)
		{
			Object.Destroy(m_Character);
		}
		content.SetActive(true);
		warning.SetActive(false);
		name.text = data.CharName;
		gp.text = string.Empty + data.CharGp;
		quest.text = data.CharQuest;
		m_Character = AvatarBuilder.GetInstance().CreateUIAvatarWithWeapon(data.State, avatar, "NPC", true);
	}

	public void Clear()
	{
		if (m_Character != null)
		{
			Object.Destroy(m_Character);
			m_Character = null;
		}
		content.SetActive(false);
		warning.SetActive(true);
	}
}

using UnityEngine;

public class UICharacterChooseButton : MonoBehaviour
{
	private static byte STATE_NO_RECORD;

	private static byte STATE_HAS_PROFILE = 1;

	private static byte STATE_NOT_HAS_PROFILE = 2;

	public UICharacterDataBase.Index index;

	public UILabel m_LabelCharInfo;

	public GameObject m_ButtonNew;

	public GameObject m_ButtonChoose;

	private byte state;

	private UICharacterData lastData;

	private void OnEnable()
	{
		state = STATE_NO_RECORD;
	}

	private void Update()
	{
		UICharacterData data = UICharacterDataBase.getInstance().GetData(index);
		if (lastData != null && lastData != data)
		{
			state = STATE_NO_RECORD;
		}
		if (data.IsHasProfile && state != STATE_HAS_PROFILE)
		{
			state = STATE_HAS_PROFILE;
			m_LabelCharInfo.text = "Lv." + data.CharLevel;
			m_ButtonNew.SetActive(false);
			m_ButtonChoose.SetActive(true);
			lastData = data;
		}
		else if (!data.IsHasProfile && state != STATE_NOT_HAS_PROFILE)
		{
			state = STATE_NOT_HAS_PROFILE;
			m_ButtonNew.SetActive(true);
			m_ButtonChoose.SetActive(false);
		}
	}
}

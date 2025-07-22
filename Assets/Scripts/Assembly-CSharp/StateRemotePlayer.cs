using System.Collections.Generic;
using UnityEngine;

public class StateRemotePlayer : MonoBehaviour
{
	public UISprite m_ClassIcon;

	public UILabel m_LabelName;

	public UIFilledSprite m_MaxHp;

	public UIFilledSprite m_CurHp;

	public UIFilledSprite m_MaxSp;

	public UIFilledSprite m_CurSp;

	public UISprite m_DyingBg;

	public UISprite m_RoomMasterIcon;

	public int id;

	private bool enable;

	private void OnEnable()
	{
		Close();
	}

	private void Update()
	{
		Dictionary<string, UserStateHUD.GameUnitHUD> remotePlayerList = UserStateHUD.GetInstance().GetRemotePlayerList();
		int num = 0;
		foreach (KeyValuePair<string, UserStateHUD.GameUnitHUD> item in remotePlayerList)
		{
			if (num < id)
			{
				num++;
				continue;
			}
			if (!enable)
			{
				m_LabelName.gameObject.SetActive(true);
				m_MaxHp.gameObject.SetActive(true);
				m_CurHp.gameObject.SetActive(true);
				m_MaxSp.gameObject.SetActive(false);
				m_CurSp.gameObject.SetActive(true);
				m_ClassIcon.gameObject.SetActive(true);
				enable = true;
			}
			if (item.Value.IsRoomMaster)
			{
				if (!m_RoomMasterIcon.gameObject.activeSelf)
				{
					m_RoomMasterIcon.gameObject.SetActive(true);
				}
			}
			else if (m_RoomMasterIcon.gameObject.activeSelf)
			{
				m_RoomMasterIcon.gameObject.SetActive(false);
			}
			if (!m_ClassIcon.spriteName.Equals(item.Value.ClassIconSpriteName))
			{
				m_ClassIcon.spriteName = item.Value.ClassIconSpriteName;
			}
			if (!m_LabelName.color.Equals(item.Value.Color))
			{
				m_LabelName.color = item.Value.Color;
			}
			m_LabelName.text = item.Value.LevelStr + " " + item.Value.Name;
			m_CurHp.fillAmount = item.Value.HpPercent;
			m_CurSp.fillAmount = item.Value.SpPercent;
			if (!item.Value.IsAlive)
			{
				if (!m_DyingBg.gameObject.activeSelf)
				{
					m_DyingBg.gameObject.SetActive(true);
					UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_TEAMMATE_DYING"));
				}
			}
			else if (m_DyingBg.gameObject.activeSelf)
			{
				m_DyingBg.gameObject.SetActive(false);
			}
			return;
		}
		if (enable)
		{
			Close();
		}
	}

	private void Close()
	{
		m_ClassIcon.gameObject.SetActive(false);
		m_LabelName.gameObject.SetActive(false);
		m_MaxHp.gameObject.SetActive(false);
		m_CurHp.gameObject.SetActive(false);
		m_MaxSp.gameObject.SetActive(false);
		m_CurSp.gameObject.SetActive(false);
		m_DyingBg.gameObject.SetActive(false);
		m_RoomMasterIcon.gameObject.SetActive(false);
		enable = false;
	}
}

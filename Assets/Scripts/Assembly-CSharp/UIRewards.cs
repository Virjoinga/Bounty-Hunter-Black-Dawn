using System;
using UnityEngine;

public class UIRewards : MonoBehaviour
{
	public UILabel m_itemName;

	public UILabel m_itemNum;

	public UISprite m_itemIcon;

	public UILabel m_mission;

	public UILabel m_description;

	private void Start()
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		component.center = Vector3.zero;
		component.size = new Vector3(Screen.width, Screen.height, -20f);
		m_mission.text = LocalizationManager.GetInstance().GetString("LOC_PLAY_DAILY_MISSION");
		m_description.text = LocalizationManager.GetInstance().GetString("LOC_EARN_MORE_TITLE_DAILY");
	}

	private void OnEnable()
	{
		switch (GameApp.GetInstance().GetGlobalState().GetGiftTimeSpan())
		{
		case 0:
			SetRewards(LocalizationManager.GetInstance().GetString("LOC_UI_MITHRIL"), "reward_mithril", UIConstant.GIFT_DAILY[0]);
			break;
		case 1:
			SetRewards(LocalizationManager.GetInstance().GetString("LOC_UI_MITHRIL"), "reward_mithril", UIConstant.GIFT_DAILY[1]);
			break;
		case 2:
			SetRewards(LocalizationManager.GetInstance().GetString("LOC_UI_MITHRIL"), "reward_mithril", UIConstant.GIFT_DAILY[2]);
			break;
		case 3:
			SetRewards(LocalizationManager.GetInstance().GetString("LOC_UI_MITHRIL"), "reward_mithril", UIConstant.GIFT_DAILY[3]);
			break;
		case 4:
			SetRewards(LocalizationManager.GetInstance().GetString("LOC_UI_MITHRIL"), "reward_mithril", UIConstant.GIFT_DAILY[4]);
			break;
		default:
			SetRewards(LocalizationManager.GetInstance().GetString("LOC_UI_MITHRIL"), "reward_mithril", UIConstant.GIFT_DAILY[0]);
			Debug.LogError("gift daily error....");
			break;
		}
	}

	public void SetRewards(string name, string iconName, int num)
	{
		UISprite component = m_itemIcon.GetComponent<UISprite>();
		component.spriteName = iconName;
		component.MakePixelPerfect();
		m_itemNum.text = "+" + Convert.ToString(num);
		m_itemName.text = name;
	}

	private void OnClick()
	{
		UITeam.giftDaily = false;
		switch (GameApp.GetInstance().GetGlobalState().GetGiftTimeSpan())
		{
		case 0:
			GameApp.GetInstance().GetGlobalState().AddMithril(UIConstant.GIFT_DAILY[0]);
			break;
		case 1:
			GameApp.GetInstance().GetGlobalState().AddMithril(UIConstant.GIFT_DAILY[1]);
			break;
		case 2:
			GameApp.GetInstance().GetGlobalState().AddMithril(UIConstant.GIFT_DAILY[2]);
			break;
		case 3:
			GameApp.GetInstance().GetGlobalState().AddMithril(UIConstant.GIFT_DAILY[3]);
			break;
		case 4:
			GameApp.GetInstance().GetGlobalState().AddMithril(UIConstant.GIFT_DAILY[4]);
			break;
		default:
			Debug.LogError("onclick gift daily error....");
			break;
		}
		base.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
	}
}

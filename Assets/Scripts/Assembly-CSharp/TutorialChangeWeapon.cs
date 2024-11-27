using UnityEngine;

public class TutorialChangeWeapon : TutorialScript
{
	public GameObject m_ChangeWeapon;

	public GameObject m_OpenList;

	private int startSlot;

	protected override void OnTutorialStart()
	{
		if (HUDManager.instance != null)
		{
			HUDManager.instance.m_HotKeyManager.ForbidAllWithout(3, 9);
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = true;
		}
		m_ChangeWeapon.SetActive(false);
		startSlot = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserState()
			.ItemInfoData.CurrentEquipWeaponSlot;
	}

	protected override void OnTutorialUpdate()
	{
		if (startSlot != GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserState()
			.ItemInfoData.CurrentEquipWeaponSlot)
		{
			EndTutorial();
		}
		else if (UserStateHUD.GetInstance().IsWeaponListOpen())
		{
			if (!m_ChangeWeapon.activeSelf)
			{
				m_ChangeWeapon.SetActive(true);
			}
			if (m_OpenList.activeSelf)
			{
				m_OpenList.SetActive(false);
			}
		}
		else
		{
			if (m_ChangeWeapon.activeSelf)
			{
				m_ChangeWeapon.SetActive(false);
			}
			if (!m_OpenList.activeSelf)
			{
				m_OpenList.SetActive(true);
			}
		}
	}

	protected override void OnTutorialEnd()
	{
		if (HUDManager.instance != null)
		{
			HUDManager.instance.m_HotKeyManager.CancelFobid();
			Transform transform = Camera.main.transform.Find("Npc_Collision");
			transform.GetComponent<Collider>().enabled = false;
		}
	}

	protected override TutorialManager.TutorialType GetType()
	{
		return TutorialManager.TutorialType.ChangeWeapon;
	}

	protected override bool IsTutorialCanStart()
	{
		return IsTutorialOk(TutorialManager.TutorialType.Reload) && !IsTutorialOk(TutorialManager.TutorialType.ChangeWeapon);
	}
}

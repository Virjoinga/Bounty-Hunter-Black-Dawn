using UnityEngine;

public class SkillTreeUIScript : UIGameMenuNormal, UIMsgListener
{
	public const int UNLOCK_FIRST_GREEN_SLOT_MITHRIL = 300;

	public const int UNLOCK_SECOND_GREEN_SLOT_MITHRIL = 600;

	public const int UNLOCK_THIRD_GREEN_SLOT_MITHRIL = 900;

	public const int EXTRA_POINT_MAX_COUNT = 27;

	public static SkillTreeUIScript mInstance;

	public UILabel TotalLeftPoint;

	public GameObject ChildObject;

	public GameObject Background;

	public GameObject Description;

	public GameObject CloseDescription;

	public SkillTreeAddPointButtonScript AddPointButton;

	public GameObject ClearPointsButton;

	public GameObject BuySkillPointButton;

	public GameObject ClassSkillSlot;

	public GameObject SkillLayer_Level2;

	public GameObject SkillLayer_Level7;

	public GameObject SkillLayer_Level12;

	public GameObject FinalSkillSlot;

	public GameObject SkillSelectPanel_Red;

	public GameObject SkillSelectPanel_Blue;

	public GameObject SkillSelectPanel_Yellow;

	public GameObject GreenSlot_Level2;

	public GameObject GreenSlot_Level7;

	public GameObject GreenSlot_Level12;

	public UISprite DescriptionIcon;

	public GameObject FirstLineBackground;

	private string BackgroundName;

	private string ClearPointsButtonName;

	private string BuySkillPointName;

	public GameObject unlockbtn;

	public short SkillIDInDescription { get; set; }

	public bool IsAlreadyOpen { get; set; }

	protected override void Awake()
	{
		base.Awake();
		mInstance = this;
		ShowUI();
	}

	protected override void OnDestroy()
	{
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		skillTreeManager.ClearPreAddPoints();
		SkillTreeButtonScript.SelectedSkillButton = null;
		GameApp.GetInstance().Save();
		base.OnDestroy();
		mInstance = null;
	}

	private void Update()
	{
	}

	public void ShowUI()
	{
		ChildObject.SetActive(true);
		RefreshSkillTree();
		DescriptionIcon.atlas = SkillTreeMgr.CharacterSkillIconAtlas;
		AddDelegate(Background, out BackgroundName);
		AddDelegate(ClearPointsButton, out ClearPointsButtonName);
		AddDelegate(BuySkillPointButton, out BuySkillPointName);
		IsAlreadyOpen = true;
	}

	public void HideUI()
	{
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		skillTreeManager.ClearPreAddPoints();
		NGUITools.SetActive(ChildObject, false);
		IsAlreadyOpen = false;
	}

	public void RefreshSkillTree()
	{
		TotalLeftPoint.text = "[00ff00]" + GameApp.GetInstance().GetUserState().SkillTreeManager.GetSkillPointsLeft() + "[-]";
		UIFrameManager.GetInstance().DeleteFrame(base.gameObject);
		if (GameApp.GetInstance().GetUserState().SkillTreeManager.GetExtraPointPurchased() >= 27)
		{
			BuySkillPointButton.SetActive(false);
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (IsThisObject(go, BackgroundName))
		{
			HideDescription();
			UIFrameManager.GetInstance().DeleteFrame(base.gameObject);
		}
		else if (IsThisObject(go, ClearPointsButtonName))
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_RESET_SKILL_POINT").Replace("%d", string.Empty + 100), 3, 31);
		}
		else if (IsThisObject(go, BuySkillPointName))
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_BUY_SKILL_POINT").Replace("%d", string.Empty + CalculateExtraPointPrice()), 3, 34);
		}
	}

	public void ClearPoints()
	{
		GameApp.GetInstance().GetUserState().SkillTreeManager.ClearPreAddPoints();
		GameApp.GetInstance().GetUserState().SkillTreeManager.ClearAllPoints();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetCharacterSkillManager()
			.ClearAllSkills();
		SummonedItem masterSummonedItem = GameApp.GetInstance().GetGameScene().GetMasterSummonedItem();
		if (masterSummonedItem != null && masterSummonedItem.SummonedType != ESummonedType.TRAPS)
		{
			GameApp.GetInstance().GetGameScene().AddToDeletSummoned(masterSummonedItem);
		}
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.RemoveHealingEffect();
		if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.ExtraShield > 0)
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.ClearExtraShield();
		}
		base.gameObject.BroadcastMessage("RefreshSkillTreeButton", true);
		base.gameObject.BroadcastMessage("GetSelfImageButtonIcon");
		RefreshSkillTree();
		HideDescription();
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 31)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(100))
				{
					UIMsgBox.instance.CloseMessage();
					ClearPoints();
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 34)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				int num = CalculateExtraPointPrice();
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(num))
				{
					UIMsgBox.instance.CloseMessage();
					SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
					skillTreeManager.AddSkillPoint();
					skillTreeManager.AddExtraPointPurchased();
					skillTreeManager.ClearPreAddPoints();
					base.gameObject.BroadcastMessage("RefreshSkillTreeButton", true);
					base.gameObject.BroadcastMessage("GetSelfImageButtonIcon");
					RefreshSkillTree();
					if (skillTreeManager.GetExtraPointPurchased() >= 27)
					{
						BuySkillPointButton.SetActive(false);
					}
					GameApp.GetInstance().GetUserState().OperInfo.AddInfo(OperatingInfoType.SKILL_POINT_MITHRIL, num);
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 9)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.IAP);
			}
		}
		else if (whichMsg.EventId == 65)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(300))
				{
					SkillTreeMgr skillTreeManager2 = GameApp.GetInstance().GetUserState().SkillTreeManager;
					skillTreeManager2.SkillLayer[0].Slot[2].SlotEnabled = true;
					if (unlockbtn != null)
					{
						RemoveLock(unlockbtn.GetComponent<SkillTreeButtonScript>());
						unlockbtn = null;
					}
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 66)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(600))
				{
					SkillTreeMgr skillTreeManager3 = GameApp.GetInstance().GetUserState().SkillTreeManager;
					skillTreeManager3.SkillLayer[1].Slot[2].SlotEnabled = true;
					if (unlockbtn != null)
					{
						RemoveLock(unlockbtn.GetComponent<SkillTreeButtonScript>());
						unlockbtn = null;
					}
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else
		{
			if (whichMsg.EventId != 67)
			{
				return;
			}
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(900))
				{
					SkillTreeMgr skillTreeManager4 = GameApp.GetInstance().GetUserState().SkillTreeManager;
					skillTreeManager4.SkillLayer[2].Slot[2].SlotEnabled = true;
					if (unlockbtn != null)
					{
						RemoveLock(unlockbtn.GetComponent<SkillTreeButtonScript>());
						unlockbtn = null;
					}
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
	}

	public void HideDescription()
	{
		UIFrameManager.GetInstance().DeleteFrame(base.gameObject);
		Description.GetComponentInChildren<UILabel>().text = string.Empty;
		AddPointButton.gameObject.SetActive(false);
		DescriptionIcon.gameObject.SetActive(false);
		FirstLineBackground.SetActive(false);
		SkillTreeButtonScript.SelectedSkillButton = null;
		SkillSelectPanel_Red.SetActive(false);
		SkillSelectPanel_Blue.SetActive(false);
		SkillSelectPanel_Yellow.SetActive(false);
		SkillIDInDescription = 0;
	}

	public void RemoveLock(SkillTreeButtonScript button)
	{
		Transform transform = button.transform.Find("Lock");
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
		}
		Transform transform2 = button.transform.Find("Price");
		if (transform2 != null)
		{
			transform2.gameObject.SetActive(false);
		}
	}

	public void UnlockGreenSlot(GameObject gameObject)
	{
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		if (!skillTreeManager.SkillLayer[0].Slot[2].SlotEnabled && gameObject == GreenSlot_Level2)
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_MITHRIL_CONFIRMATION").Replace("%d", 300.ToString()), 3, 65);
			unlockbtn = null;
			unlockbtn = gameObject;
		}
		else if (!skillTreeManager.SkillLayer[1].Slot[2].SlotEnabled && gameObject == GreenSlot_Level7)
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_MITHRIL_CONFIRMATION").Replace("%d", 600.ToString()), 3, 66);
			unlockbtn = null;
			unlockbtn = gameObject;
		}
		else if (!skillTreeManager.SkillLayer[2].Slot[2].SlotEnabled && gameObject == GreenSlot_Level12)
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_MITHRIL_CONFIRMATION").Replace("%d", 900.ToString()), 3, 67);
			unlockbtn = null;
			unlockbtn = gameObject;
		}
	}

	public int CalculateExtraPointPrice()
	{
		int num = 0;
		int extraPointPurchased = GameApp.GetInstance().GetUserState().SkillTreeManager.GetExtraPointPurchased();
		if (extraPointPurchased == 0)
		{
			return 100;
		}
		if (extraPointPurchased == 1)
		{
			return 105;
		}
		if (extraPointPurchased >= 27)
		{
			return 10000;
		}
		int num2 = (int)((float)(extraPointPurchased + 3) * 0.2f);
		float num3 = (float)(extraPointPurchased + 3) * 0.2f;
		return (int)((float)(num2 * num2 * 300 - (num2 - 1) * 600) + (num3 - (float)num2) * 25f);
	}
}

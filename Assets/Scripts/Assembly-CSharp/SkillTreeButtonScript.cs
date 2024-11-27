using UnityEngine;

public class SkillTreeButtonScript : UIGameMenuNormal, UIMsgListener
{
	protected const string YellowPrefix = "[ffd36b]";

	protected const string GreenPrefix = "[00ff36]";

	protected const string ColorSuffix = "[-]";

	public UILabel SkillName;

	public UILabel LevelLabel;

	public UIImageButton SelfImageButton;

	public bool IsForSlot = true;

	public int Layer;

	public int Type;

	public int Order;

	protected short mSkillID;

	protected int mLevel;

	private bool isLighted;

	private static string AddPointButtonName;

	public static SkillTreeButtonScript SelectedSkillButton;

	public int SlotType { get; set; }

	private new void OnEnable()
	{
		SelfImageButton = base.gameObject.GetComponent<UIImageButton>();
		SelfImageButton.target.atlas = SkillTreeMgr.CharacterSkillIconAtlas;
		Transform transform = base.transform.Find("info");
		if (transform != null)
		{
			Transform transform2 = transform.Find("LevelBackground");
			if (transform2 != null && transform2.GetComponent<UISprite>() != null)
			{
				transform2.GetComponent<UISprite>().spriteName = "skillLV1";
				transform2.GetComponent<UISprite>().MakePixelPerfect();
			}
		}
		if (!IsForSlot)
		{
			Transform transform3 = base.transform.Find("Unlight");
			if (transform3 != null && transform3.GetComponent<UISprite>() != null)
			{
				transform3.GetComponent<UISprite>().atlas = SkillTreeMgr.CharacterSkillIconAtlas;
			}
		}
		if (Layer != -1)
		{
			Transform transform4 = base.transform.Find("info");
			if (transform4 != null)
			{
				NGUITools.SetActive(transform4.gameObject, false);
			}
		}
		RefreshSkillTreeButton(true);
		GetSelfImageButtonIcon();
		if (!IsForSlot)
		{
			AddDelegate(SkillTreeUIScript.mInstance.AddPointButton.gameObject, out AddPointButtonName);
		}
	}

	private void OnDisable()
	{
		RemoveDelegate(SkillTreeUIScript.mInstance.AddPointButton.gameObject);
	}

	private void OnClick()
	{
		if (IsForSlot)
		{
			UIFrameManager.GetInstance().DeleteFrame(SkillTreeUIScript.mInstance.gameObject);
		}
		else
		{
			UIFrameManager.GetInstance().DeleteFrame(base.transform.parent.parent.gameObject);
		}
		UIFrameManager.GetInstance().CreateFrame(base.gameObject, new Vector2(12f, 12f), -1f, 3);
		SkillTreeUIScript mInstance = SkillTreeUIScript.mInstance;
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		if (Layer == -1 || Layer == 3 || skillTreeManager.SkillLayer[Layer].Slot[Type].SlotEnabled)
		{
			if (mInstance.SkillIDInDescription != mSkillID || mSkillID == 0 || IsForSlot)
			{
				if (IsForSlot)
				{
					SelectedSkillButton = this;
					if (Layer == -1)
					{
						mInstance.SkillSelectPanel_Red.SetActive(false);
						mInstance.SkillSelectPanel_Blue.SetActive(false);
						mInstance.SkillSelectPanel_Yellow.SetActive(false);
					}
					else if (Type != 3)
					{
						mInstance.SkillSelectPanel_Red.SetActive(true);
						mInstance.SkillSelectPanel_Blue.SetActive(true);
						mInstance.SkillSelectPanel_Yellow.SetActive(false);
						SkillTreeButtonScript[] componentsInChildren = mInstance.SkillSelectPanel_Red.GetComponentsInChildren<SkillTreeButtonScript>();
						foreach (SkillTreeButtonScript skillTreeButtonScript in componentsInChildren)
						{
							skillTreeButtonScript.Layer = Layer;
							skillTreeButtonScript.SlotType = Type;
							skillTreeButtonScript.OpenInPanel();
							skillTreeButtonScript.RefreshSkillTreeButton(true);
							skillTreeButtonScript.GetSelfImageButtonIcon();
						}
						SkillTreeButtonScript[] componentsInChildren2 = mInstance.SkillSelectPanel_Blue.GetComponentsInChildren<SkillTreeButtonScript>();
						foreach (SkillTreeButtonScript skillTreeButtonScript2 in componentsInChildren2)
						{
							skillTreeButtonScript2.Layer = Layer;
							skillTreeButtonScript2.SlotType = Type;
							skillTreeButtonScript2.OpenInPanel();
							skillTreeButtonScript2.RefreshSkillTreeButton(true);
							skillTreeButtonScript2.GetSelfImageButtonIcon();
						}
					}
					else
					{
						mInstance.SkillSelectPanel_Red.SetActive(false);
						mInstance.SkillSelectPanel_Blue.SetActive(false);
						mInstance.SkillSelectPanel_Yellow.SetActive(true);
						SkillTreeButtonScript[] componentsInChildren3 = mInstance.SkillSelectPanel_Yellow.GetComponentsInChildren<SkillTreeButtonScript>();
						foreach (SkillTreeButtonScript skillTreeButtonScript3 in componentsInChildren3)
						{
							skillTreeButtonScript3.Layer = Layer;
							skillTreeButtonScript3.SlotType = Type;
							skillTreeButtonScript3.OpenInPanel();
							skillTreeButtonScript3.RefreshSkillTreeButton(true);
							skillTreeButtonScript3.GetSelfImageButtonIcon();
						}
					}
				}
				else if (Type != SlotType)
				{
					SetSkillLevel(0);
				}
				ShowDescription();
				return;
			}
			if (isLighted && Layer != -1 && skillTreeManager.CanAddPointForSlot(Layer, SlotType, mSkillID))
			{
				if (Layer != 3)
				{
					if (skillTreeManager.GetTotalPreAddPoints() < skillTreeManager.GetSkillPointsLeft())
					{
						skillTreeManager.PreAddSkillIDs[Layer, SlotType] = mSkillID;
						skillTreeManager.PreAddPoints[Layer, SlotType]++;
						SkillTreeUIScript.mInstance.gameObject.BroadcastMessage("RefreshSkillTreeButton", false);
						SkillTreeUIScript.mInstance.gameObject.BroadcastMessage("GetSelfImageButtonIcon");
						SelectedSkillButton.SetLevelInfo(skillTreeManager.SkillLayer[Layer].Slot[SlotType].GetLevel() + skillTreeManager.PreAddPoints[Layer, SlotType], false);
						SkillTreeUIScript.mInstance.TotalLeftPoint.text = "[ffd36b]" + (skillTreeManager.GetSkillPointsLeft() - skillTreeManager.GetTotalPreAddPoints()) + "[-]";
					}
					else
					{
						UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_NOT_ENOUGH_SKILL_POINT"), 2, 10);
					}
				}
				else if (skillTreeManager.FinalSkillSlot.GetSkillID() == 0 && !skillTreeManager.AddPrePointForFinalSkill)
				{
					if (skillTreeManager.GetTotalPreAddPoints() < skillTreeManager.GetSkillPointsLeft())
					{
						skillTreeManager.PreAddFinalSkillID = mSkillID;
						skillTreeManager.AddPrePointForFinalSkill = true;
						SelectedSkillButton.GetSelfImageButtonIcon();
						SelectedSkillButton.SetLevelInfo(1, false);
						SkillTreeUIScript.mInstance.TotalLeftPoint.text = "[ffd36b]" + (skillTreeManager.GetSkillPointsLeft() - skillTreeManager.GetTotalPreAddPoints()) + "[-]";
					}
					else
					{
						UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_NOT_ENOUGH_SKILL_POINT"), 2, 10);
					}
				}
			}
			ShowDescription();
		}
		else if (IsForSlot && Type == 2 && Layer != -1 && Layer != 3)
		{
			mInstance.UnlockGreenSlot(base.gameObject);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		CheckUnlight();
	}

	public void RefreshSkillTreeButton(bool needRefreshLevelInfo)
	{
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		int num = 0;
		bool flag = true;
		if (IsForSlot)
		{
			if (Layer != -1)
			{
				if (Layer != 3)
				{
					num = skillTreeManager.SkillLayer[Layer].Slot[Type].GetLevel();
					SetSkillID(skillTreeManager.SkillLayer[Layer].Slot[Type].GetSkillID());
					SetSkillLevel(num);
					SlotType = Type;
				}
				else
				{
					num = skillTreeManager.FinalSkillSlot.GetLevel();
					SetSkillID(skillTreeManager.FinalSkillSlot.GetSkillID());
					SetSkillLevel(num);
				}
			}
			else if (Layer == -1)
			{
				num = 1;
				SetSkillID(skillTreeManager.ClassSkillSlot.GetSkillID());
				SetSkillLevel(num);
			}
			if (needRefreshLevelInfo)
			{
				SetLevelInfo(num, true);
			}
			if (Layer == -1)
			{
				flag = false;
			}
			else if (Type < 3)
			{
				if (num == 5)
				{
					flag = false;
				}
			}
			else if (Type == 3 && num == 1)
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (IsForSlot)
			{
				if (!skillTreeManager.CanAddPoint(Layer))
				{
					Transform transform = base.transform.Find("info");
					if (transform != null)
					{
						NGUITools.SetActive(transform.gameObject, false);
					}
					Transform transform2 = base.transform.Find("Unlight");
					if (transform2 != null)
					{
						NGUITools.SetActive(transform2.gameObject, true);
					}
					isLighted = false;
				}
			}
			else
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				flag2 = ((Type == SlotType || SlotType == 2 || SlotType == 3) ? true : false);
				int num2 = SkillTreeMgr.ENOUGH_LEVEL_FOR_LAYER[Layer];
				flag3 = ((GameApp.GetInstance().GetUserState().GetCharLevel() >= num2) ? true : false);
				if (Layer != -1 && Layer != 3)
				{
					if (skillTreeManager.SkillLayer[Layer].Slot[SlotType].GetSkillID() == 0 && skillTreeManager.PreAddSkillIDs[Layer, SlotType] == 0)
					{
						if ((skillTreeManager.SkillLayer[Layer].HaveSkill(mSkillID) || skillTreeManager.HavePreAddSkill(Layer, mSkillID)) && skillTreeManager.SkillLayer[Layer].Slot[SlotType].GetSkillID() != mSkillID && skillTreeManager.PreAddSkillIDs[Layer, SlotType] != mSkillID)
						{
							flag4 = true;
						}
					}
					else if (skillTreeManager.SkillLayer[Layer].Slot[SlotType].GetSkillID() != mSkillID && skillTreeManager.PreAddSkillIDs[Layer, SlotType] != mSkillID)
					{
						flag4 = true;
					}
				}
				else if (Layer == 3 && (skillTreeManager.FinalSkillSlot.GetSkillID() != 0 || skillTreeManager.PreAddFinalSkillID != 0) && skillTreeManager.FinalSkillSlot.GetSkillID() != mSkillID && skillTreeManager.PreAddFinalSkillID != mSkillID)
				{
					flag4 = true;
				}
				if (flag2 && flag3 && !flag4)
				{
					Transform transform3 = base.transform.Find("Unlight");
					if (transform3 != null)
					{
						transform3.gameObject.SetActive(false);
					}
					isLighted = true;
				}
				else
				{
					Transform transform4 = base.transform.Find("Unlight");
					if (transform4 != null)
					{
						NGUITools.SetActive(transform4.gameObject, true);
					}
					isLighted = false;
				}
			}
		}
		if (IsForSlot && Type == 2 && Layer != -1 && Layer != 3 && skillTreeManager.SkillLayer[Layer].Slot[Type].SlotEnabled)
		{
			SkillTreeUIScript.mInstance.RemoveLock(this);
		}
	}

	protected void GetSelfImageButtonIcon()
	{
		if (SelfImageButton == null)
		{
			return;
		}
		string empty = string.Empty;
		short num = 0;
		if (mSkillID != 0)
		{
			num = mSkillID;
		}
		else if (IsForSlot)
		{
			SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
			num = ((Layer == 3) ? skillTreeManager.PreAddFinalSkillID : skillTreeManager.PreAddSkillIDs[Layer, Type]);
		}
		if (num != 0)
		{
			SkillConfig skillConfig = GameConfig.GetInstance().skillConfig[num * 10 + 1];
			empty = skillConfig.IconName;
			SelfImageButton.target.gameObject.SetActive(true);
			SelfImageButton.normalSprite = empty;
			SelfImageButton.hoverSprite = empty;
			SelfImageButton.pressedSprite = empty;
			SelfImageButton.target.spriteName = empty;
			Transform transform = base.transform.Find("info");
			if (transform != null)
			{
				transform.gameObject.SetActive(true);
			}
		}
		else
		{
			SelfImageButton.target.gameObject.SetActive(false);
			Transform transform2 = base.transform.Find("info");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(false);
			}
		}
	}

	protected void ShowDescription()
	{
		SkillTreeUIScript.mInstance.SkillIDInDescription = mSkillID;
		GameObject description = SkillTreeUIScript.mInstance.Description;
		SkillTreeUIScript.mInstance.FirstLineBackground.SetActive(true);
		SkillTreeUIScript.mInstance.DescriptionIcon.gameObject.SetActive(true);
		SkillTreeUIScript.mInstance.DescriptionIcon.spriteName = SelfImageButton.target.spriteName;
		SkillTreeUIScript.mInstance.Description.GetComponentInChildren<UIDraggablePanel>().ResetPosition();
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		if (Layer != -1 && skillTreeManager.CanAddPointForSlot(Layer, SlotType, mSkillID))
		{
			if (IsForSlot || isLighted)
			{
				SkillTreeUIScript.mInstance.AddPointButton.gameObject.SetActive(true);
			}
			else
			{
				SkillTreeUIScript.mInstance.AddPointButton.gameObject.SetActive(false);
			}
		}
		else
		{
			SkillTreeUIScript.mInstance.AddPointButton.gameObject.SetActive(false);
		}
		short num = mSkillID;
		int num2 = mLevel;
		if (Layer != -1 && Layer != 3 && isLighted)
		{
			num2 = skillTreeManager.SkillLayer[Layer].Slot[SlotType].GetLevel() + skillTreeManager.PreAddPoints[Layer, SlotType];
		}
		if (mSkillID != 0)
		{
			int num3 = num * 10 + num2;
			if (num2 == 0)
			{
				num3++;
			}
			SkillConfig skillConfig = GameConfig.GetInstance().skillConfig[num3];
			if (skillConfig.CurrentDescribValue.Equals(string.Empty) && skillConfig.NextDescribValue.Equals(string.Empty))
			{
				GetDescribValue(num3, num2);
			}
			if (SkillName != null)
			{
				SkillName.text = skillConfig.Name;
			}
			UILabel componentInChildren = description.GetComponentInChildren<UILabel>();
			componentInChildren.text = LocalizationManager.GetInstance().GetString(skillConfig.Description1) + "\n";
			switch (num)
			{
			case 1026:
				componentInChildren.text = componentInChildren.text.Replace("%d", string.Empty + CharacterSkillManager.CalculateExplosionSkillDamage() * 10);
				break;
			case 1088:
				componentInChildren.text = componentInChildren.text.Replace("%d", string.Empty + CharacterSkillManager.CalculateExplosionSkillDamage());
				break;
			}
			if (Layer == -1 || Type == 3)
			{
				return;
			}
			if (num2 != 0)
			{
				componentInChildren.text += skillConfig.CurrentDescribValue;
				componentInChildren.text = componentInChildren.text + " " + LocalizationManager.GetInstance().GetString(skillConfig.Description2) + "\n";
				if (skillConfig.NextDescribValue != string.Empty)
				{
					componentInChildren.text += skillConfig.NextDescribValue;
					componentInChildren.text = componentInChildren.text + " " + LocalizationManager.GetInstance().GetString(skillConfig.Description2) + "\n";
				}
			}
			else
			{
				componentInChildren.text += "Current Rank: ------\n";
				string currentDescribValue = skillConfig.CurrentDescribValue;
				currentDescribValue = currentDescribValue.Replace("Current Rank", "Next Rank");
				componentInChildren.text += currentDescribValue;
				componentInChildren.text = componentInChildren.text + " " + LocalizationManager.GetInstance().GetString(skillConfig.Description2) + "\n";
			}
		}
		else
		{
			description.GetComponentInChildren<UILabel>().text = string.Empty;
			SkillTreeUIScript.mInstance.AddPointButton.gameObject.SetActive(false);
			SkillTreeUIScript.mInstance.DescriptionIcon.gameObject.SetActive(false);
			SkillTreeUIScript.mInstance.FirstLineBackground.SetActive(false);
		}
	}

	protected void GetDescribValue(int configKey, int skillLevel)
	{
		SkillConfig skillConfig = GameConfig.GetInstance().skillConfig[configKey];
		SkillConfig skillConfig2 = null;
		if (GameConfig.GetInstance().skillConfig.ContainsKey(configKey + 1))
		{
			skillConfig2 = GameConfig.GetInstance().skillConfig[configKey + 1];
		}
		else if (skillLevel == 1)
		{
			return;
		}
		skillConfig.CurrentDescribValue = "Current Rank: [00eaff]";
		skillConfig.NextDescribValue = "Next Rank: [00eaff]";
		switch (skillConfig.FunctionType1)
		{
		case SkillFunctionType.PropertyChange:
		{
			int x = skillConfig.X1;
			if (x % 100 == 4 || x % 100 == 32)
			{
				skillConfig.CurrentDescribValue += -skillConfig.Y1;
			}
			else
			{
				skillConfig.CurrentDescribValue += skillConfig.Y1;
			}
			if (x / 100 == 1)
			{
				skillConfig.CurrentDescribValue += "%";
			}
			if (skillConfig2 != null)
			{
				if (x % 100 == 4 || x % 100 == 32)
				{
					skillConfig.NextDescribValue += -skillConfig2.Y1;
				}
				else
				{
					skillConfig.NextDescribValue += skillConfig2.Y1;
				}
				if (x / 100 == 1)
				{
					skillConfig.NextDescribValue += "%";
				}
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		}
		case SkillFunctionType.CreateBuff:
		{
			short num2 = (short)skillConfig.X1;
			CharacterStateSkill characterStateSkill2 = new CharacterStateSkill(num2);
			BuffConfig buffConfig2 = GameConfig.GetInstance().buffConfig[num2];
			skillConfig.CurrentDescribValue = buffConfig2.CurrentDescribValue;
			if (skillConfig2 != null)
			{
				skillConfig.NextDescribValue = buffConfig2.NextDescribValue;
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		}
		case SkillFunctionType.MakeHeal:
		case SkillFunctionType.ShieldRecover:
			skillConfig.CurrentDescribValue = skillConfig.CurrentDescribValue + skillConfig.X1 + "%";
			if (skillConfig2 != null)
			{
				skillConfig.NextDescribValue = skillConfig.NextDescribValue + skillConfig2.X1 + "%";
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		case SkillFunctionType.BulletRecover:
			skillConfig.CurrentDescribValue = skillConfig.CurrentDescribValue + skillConfig.X1 + "%";
			if (skillConfig2 != null)
			{
				skillConfig.NextDescribValue = skillConfig.NextDescribValue + skillConfig2.X1 + "%";
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		case SkillFunctionType.ExtraBuff:
		{
			short num3 = (short)skillConfig.Y1;
			CharacterStateSkill characterStateSkill3 = new CharacterStateSkill(num3);
			BuffConfig buffConfig3 = GameConfig.GetInstance().buffConfig[num3];
			skillConfig.CurrentDescribValue = buffConfig3.CurrentDescribValue;
			if (skillConfig2 != null)
			{
				skillConfig.NextDescribValue = buffConfig3.NextDescribValue;
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		}
		case SkillFunctionType.ShortCD:
			skillConfig.CurrentDescribValue = skillConfig.CurrentDescribValue + "-" + skillConfig.Y1;
			if (skillConfig2 != null)
			{
				skillConfig.NextDescribValue = skillConfig.NextDescribValue + "-" + skillConfig2.Y1;
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		case SkillFunctionType.SummonDurationChange:
		case SkillFunctionType.BuffDurationChange:
			skillConfig.CurrentDescribValue += skillConfig.Y1;
			if (skillConfig2 != null)
			{
				skillConfig.NextDescribValue += skillConfig2.Y1;
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		case SkillFunctionType.KeepHealing:
		{
			short num = (short)skillConfig.X1;
			CharacterStateSkill characterStateSkill = new CharacterStateSkill(num);
			BuffConfig buffConfig = GameConfig.GetInstance().buffConfig[num];
			skillConfig.CurrentDescribValue = buffConfig.CurrentDescribValue;
			if (skillConfig2 != null)
			{
				skillConfig.NextDescribValue = buffConfig.NextDescribValue;
			}
			else
			{
				skillConfig.NextDescribValue += "------";
			}
			break;
		}
		}
		skillConfig.CurrentDescribValue += "[-]";
		skillConfig.NextDescribValue += "[-]";
	}

	public void SetLevelInfo(int number, bool isRealPoint)
	{
		if (LevelLabel != null)
		{
			int num = 5;
			if (Layer == -1 || Type == 3)
			{
				num = 1;
			}
			if (isRealPoint)
			{
				LevelLabel.text = "[00ff36]" + number + "/" + num + "[-]";
			}
			else
			{
				LevelLabel.text = "[ffd36b]" + number + "/" + num + "[-]";
			}
		}
	}

	protected void CheckUnlight()
	{
		if (!IsForSlot || isLighted)
		{
			return;
		}
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		int num = 0;
		num = ((Layer != -1 && Layer != 3) ? skillTreeManager.SkillLayer[Layer].Slot[Type].GetLevel() : ((Layer != 3) ? 1 : skillTreeManager.FinalSkillSlot.GetLevel()));
		bool flag = false;
		if (Layer == -1)
		{
			flag = true;
		}
		else if (Type < 3)
		{
			if (num == 5)
			{
				flag = true;
			}
		}
		else if (Type == 3 && num == 1)
		{
			flag = true;
		}
		if (flag || skillTreeManager.CanAddPoint(Layer))
		{
			Transform transform = base.transform.Find("Unlight");
			if (transform != null)
			{
				NGUITools.SetActive(transform.gameObject, false);
			}
			isLighted = true;
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (IsThisObject(go, AddPointButtonName) && SelectedSkillButton != null && mSkillID == SkillTreeUIScript.mInstance.SkillIDInDescription)
		{
			OnClick();
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}

	public void SetSkillID(short id)
	{
		mSkillID = id;
	}

	public short GetSkillID()
	{
		return mSkillID;
	}

	public void SetSkillLevel(int level)
	{
		mLevel = level;
	}

	public int GetSkillLevel()
	{
		return mLevel;
	}

	public void OpenInPanel()
	{
		SkillTreeMgr skillTreeManager = GameApp.GetInstance().GetUserState().SkillTreeManager;
		SetSkillID(skillTreeManager.GetSkillId(Layer, Type, Order));
	}
}

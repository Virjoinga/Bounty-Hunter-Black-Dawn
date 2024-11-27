using System.IO;
using UnityEngine;

public class SkillTreeMgr
{
	protected const int LAYER = 4;

	protected const int SKILL_TYPES = 4;

	protected const int ORDER_NUM = 3;

	public static UIAtlas CharacterSkillIconAtlas = null;

	protected string skillPointsLeft;

	protected string extraPointPurchased;

	public static int[] ENOUGH_LEVEL_FOR_LAYER = new int[4] { 2, 7, 12, 17 };

	protected static short[,,] SkillIDs = new short[4, 4, 3];

	public SkillSlot ClassSkillSlot = new SkillSlot(-1, SkillSlotType.ClassSkill);

	public SkillLayer[] SkillLayer = new SkillLayer[4];

	public SkillSlot FinalSkillSlot = new SkillSlot(3, SkillSlotType.YellowSlot);

	public short[,] PreAddSkillIDs = new short[4, 3];

	public int[,] PreAddPoints = new int[4, 3];

	public short PreAddFinalSkillID;

	public bool AddPrePointForFinalSkill;

	protected short ClassSkillID { get; set; }

	public SkillTreeMgr()
	{
		Init();
	}

	public void Init()
	{
		SetSkillPointLeft(0);
		SetExtraPointPurchased(0);
		InitPreAddPoints();
		for (int i = 0; i < 4; i++)
		{
			SkillLayer[i] = new SkillLayer();
			SkillLayer[i].Init(i);
		}
		FinalSkillSlot.SetSkillID(0);
		FinalSkillSlot.SetLevel(0);
	}

	public void InitSkillIDs()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[42];
		CharacterClass characterClass = GameApp.GetInstance().GetUserState().GetCharacterClass();
		string text = "SkillTree/Res/Icon/" + (byte)characterClass + "/IconSkill" + (byte)characterClass;
		Debug.Log(text);
		CharacterSkillIconAtlas = (Resources.Load(text) as GameObject).GetComponent<UIAtlas>();
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			int data = unitDataTable.GetData(i, 0, 0, false);
			if (data == (int)characterClass)
			{
				int num = (sbyte)unitDataTable.GetData(i, 2, 0, false);
				if (num != -1)
				{
					int num2 = unitDataTable.GetData(i, 3, 0, false) - 1;
					int data2 = unitDataTable.GetData(i, 4, 0, false);
					short num3 = (short)unitDataTable.GetData(i, 5, 0, false);
					SkillIDs[num, num2, data2] = num3;
				}
				else
				{
					short classSkillID = (short)unitDataTable.GetData(i, 5, 0, false);
					ClassSkillID = classSkillID;
					ClassSkillSlot.SetSkillID(ClassSkillID);
					ClassSkillSlot.SetLevel(1);
				}
			}
		}
	}

	public void InitSkillIDs(RobotUser robotUser)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[42];
		CharacterClass characterClass = robotUser.GetUserState().GetCharacterClass();
		string text = "SkillTree/Res/Icon/" + (byte)characterClass + "/IconSkill" + (byte)characterClass;
		Debug.Log(text);
		CharacterSkillIconAtlas = (Resources.Load(text) as GameObject).GetComponent<UIAtlas>();
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			int data = unitDataTable.GetData(i, 0, 0, false);
			if (data == (int)characterClass)
			{
				int num = unitDataTable.GetData(i, 1, 0, false) - 1;
				if (num != -1)
				{
					int num2 = unitDataTable.GetData(i, 2, 0, false) - 1;
					int num3 = unitDataTable.GetData(i, 3, 0, false) - 1;
					short num4 = (short)unitDataTable.GetData(i, 4, 0, false);
					SkillIDs[num, num2, num3] = num4;
				}
				else
				{
					short classSkillID = (short)unitDataTable.GetData(i, 4, 0, false);
					ClassSkillID = classSkillID;
				}
			}
		}
	}

	public int GetTreePoints(int treeIndex)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			num += SkillLayer[i].GetTotalPoints();
		}
		return num + FinalSkillSlot.GetLevel();
	}

	public void GivePoint(int layerID, int type, int order)
	{
	}

	public void GivePoint(int layerID, int slotType, short skillID)
	{
		if (skillID != 0 && GetSkillPointsLeft() > 0)
		{
			UseSkillPoint();
			if (layerID != 3)
			{
				SkillLayer[layerID].Slot[slotType].LearnSkill(skillID);
				Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				localPlayer.GetCharacterSkillManager().AddSkillByID(skillID, SkillLayer[layerID].Slot[slotType].GetLevel());
			}
			else
			{
				FinalSkillSlot.LearnSkill(skillID);
				Player localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				localPlayer2.GetCharacterSkillManager().AddSkillByID(skillID, 1);
			}
		}
	}

	public int GetSkillPointsLeft()
	{
		return AntiCracking.DecryptBufferStr(skillPointsLeft, "skillZ");
	}

	public void SetSkillPointLeft(int _point)
	{
		skillPointsLeft = AntiCracking.CryptBufferStr(_point, "skillZ");
	}

	public void AddSkillPoint()
	{
		int skillPointLeft = GetSkillPointsLeft() + 1;
		SetSkillPointLeft(skillPointLeft);
	}

	public void UseSkillPoint()
	{
		int skillPointLeft = GetSkillPointsLeft() - 1;
		SetSkillPointLeft(skillPointLeft);
	}

	public int GetExtraPointPurchased()
	{
		return AntiCracking.DecryptBufferStr(extraPointPurchased, "skillZ");
	}

	public void SetExtraPointPurchased(int _extraPoint)
	{
		extraPointPurchased = AntiCracking.CryptBufferStr(_extraPoint, "skillZ");
	}

	public void AddExtraPointPurchased()
	{
		int num = GetExtraPointPurchased() + 1;
		SetExtraPointPurchased(num);
	}

	public short GetSkillId(int layerID, int type, int order)
	{
		if (layerID == -1)
		{
			return ClassSkillID;
		}
		return SkillIDs[layerID, type, order];
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(GetSkillPointsLeft());
		bw.Write(GetExtraPointPurchased());
		for (int i = 0; i < 4; i++)
		{
			SkillLayer[i].Save(bw);
		}
		FinalSkillSlot.Save(bw);
	}

	public void Load(BinaryReader br)
	{
		int skillPointLeft = br.ReadInt32();
		SetSkillPointLeft(skillPointLeft);
		int num = br.ReadInt32();
		SetExtraPointPurchased(num);
		for (int i = 0; i < 4; i++)
		{
			SkillLayer[i].Load(br);
		}
		FinalSkillSlot.Load(br);
	}

	public int GetPreAddLayerPoints(int layerID)
	{
		int num = 0;
		if (layerID != 3)
		{
			for (int i = 0; i < 3; i++)
			{
				num += PreAddPoints[layerID, i];
			}
		}
		else
		{
			num += (AddPrePointForFinalSkill ? 1 : 0);
		}
		return num;
	}

	public int GetTotalPreAddPoints()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				num += PreAddPoints[i, j];
			}
		}
		if (AddPrePointForFinalSkill)
		{
			num++;
		}
		return num;
	}

	public void ClearPreAddPoints()
	{
		InitPreAddPoints();
	}

	public void InitPreAddPoints()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				PreAddSkillIDs[i, j] = 0;
				PreAddPoints[i, j] = 0;
			}
		}
		PreAddFinalSkillID = 0;
		AddPrePointForFinalSkill = false;
		SkillTreeButtonScript.SelectedSkillButton = null;
	}

	public bool OneTreeIsFullyUpgraded()
	{
		return false;
	}

	public void ClearAllPoints()
	{
		int treePoints = GetTreePoints(0);
		for (int i = 0; i < 4; i++)
		{
			SkillLayer[i].ClearPoints();
		}
		FinalSkillSlot.ClearSlot();
		int num = GetSkillPointsLeft();
		num += treePoints;
		SetSkillPointLeft(num);
	}

	public void CheckAchievement009()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			num += SkillLayer[i].LearnedSkillCounts();
		}
		Debug.Log(num);
		if (num >= 6)
		{
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Grow_UP, AchievementTrigger.Type.Data);
			AchievementManager.GetInstance().Trigger(trigger);
		}
	}

	public bool CanAddPoint(int layerID)
	{
		bool flag = GameApp.GetInstance().GetUserState().GetCharLevel() >= ENOUGH_LEVEL_FOR_LAYER[layerID];
		bool flag2 = false;
		int num = 0;
		for (int i = 0; i < layerID; i++)
		{
			num += SkillLayer[i].GetTotalPoints() + GetPreAddLayerPoints(i);
		}
		if (num >= 5 * layerID)
		{
			flag2 = true;
		}
		return flag && flag2;
	}

	public bool CanAddPointForSlot(int layerID, int slotType, short skillID)
	{
		bool flag = false;
		bool flag2 = false;
		if (layerID != 3)
		{
			if (SkillLayer[layerID].Slot[slotType].GetLevel() + PreAddPoints[layerID, slotType] < 5)
			{
				short skillID2 = SkillLayer[layerID].Slot[slotType].GetSkillID();
				short num = PreAddSkillIDs[layerID, slotType];
				if ((skillID2 == 0 && num == 0 && skillID != 0) || (skillID2 != 0 && num == 0 && skillID == skillID2) || (skillID2 == 0 && num != 0 && skillID == num) || (skillID2 != 0 && num != 0 && skillID == skillID2 && skillID == num))
				{
					flag = true;
				}
			}
			if ((SkillLayer[layerID].HaveSkill(skillID) || HavePreAddSkill(layerID, skillID)) && SkillLayer[layerID].Slot[slotType].GetSkillID() != skillID && PreAddSkillIDs[layerID, slotType] != skillID)
			{
				flag2 = true;
			}
		}
		else if (FinalSkillSlot.GetLevel() + (AddPrePointForFinalSkill ? 1 : 0) < 1)
		{
			short skillID3 = FinalSkillSlot.GetSkillID();
			short preAddFinalSkillID = PreAddFinalSkillID;
			if ((skillID3 == 0 && preAddFinalSkillID == 0 && skillID != 0) || (skillID3 != 0 && preAddFinalSkillID == 0 && skillID == skillID3) || (skillID3 == 0 && preAddFinalSkillID != 0 && skillID == preAddFinalSkillID))
			{
				flag = true;
			}
		}
		return CanAddPoint(layerID) && flag && !flag2;
	}

	public bool HavePreAddSkill(int layer, short skillID)
	{
		for (int i = 0; i < 3; i++)
		{
			if (PreAddSkillIDs[layer, i] == skillID)
			{
				return true;
			}
		}
		return false;
	}
}

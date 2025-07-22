using System.IO;

public class SkillLayer
{
	public SkillSlot[] Slot = new SkillSlot[3];

	public int Layer { get; set; }

	public void Init(int layer)
	{
		Layer = layer;
		Slot[0] = new SkillSlot(Layer, SkillSlotType.RedSlot);
		Slot[1] = new SkillSlot(Layer, SkillSlotType.BlueSlot);
		Slot[2] = new SkillSlot(Layer, SkillSlotType.GreenSlot);
		Slot[2].SlotEnabled = false;
	}

	public int GetTotalPoints()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			num += Slot[i].GetLevel();
		}
		return num;
	}

	public void LoadSkill(CharacterSkillManager csManager)
	{
		for (int i = 0; i < 3; i++)
		{
			if (Slot[i].GetSkillID() != 0 && Slot[i].GetLevel() > 0)
			{
				csManager.AddSkillByID(Slot[i].GetSkillID(), Slot[i].GetLevel());
			}
		}
	}

	public void ClearPoints()
	{
		for (int i = 0; i < 3; i++)
		{
			Slot[i].ClearSlot();
		}
	}

	public void Save(BinaryWriter bw)
	{
		for (int i = 0; i < 3; i++)
		{
			Slot[i].Save(bw);
		}
	}

	public void Load(BinaryReader br)
	{
		for (int i = 0; i < 3; i++)
		{
			Slot[i].Load(br);
		}
	}

	public bool HaveSkill(short skillID)
	{
		for (int i = 0; i < 3; i++)
		{
			if (Slot[i].GetSkillID() == skillID)
			{
				return true;
			}
		}
		return false;
	}

	public int LearnedSkillCounts()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (Slot[i].GetSkillID() != 0)
			{
				num++;
			}
		}
		return num;
	}
}

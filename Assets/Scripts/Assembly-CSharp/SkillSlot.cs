using System.IO;

public class SkillSlot
{
	public SkillSlotType SlotType;

	public bool SlotEnabled = true;

	protected short mSkillID;

	protected int mLevel;

	public int Layer { get; set; }

	public SkillSlot(int layer, SkillSlotType slotType)
	{
		Layer = layer;
		SlotType = slotType;
	}

	public short GetSkillID()
	{
		return mSkillID;
	}

	public void SetSkillID(short id)
	{
		mSkillID = id;
	}

	public int GetLevel()
	{
		return mLevel;
	}

	public void SetLevel(int level)
	{
		mLevel = level;
	}

	public string SkillIconName()
	{
		return string.Empty;
	}

	public void ClearSlot()
	{
		mSkillID = 0;
		mLevel = 0;
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(mSkillID);
		bw.Write(mLevel);
		bw.Write(SlotEnabled);
	}

	public void Load(BinaryReader br)
	{
		mSkillID = br.ReadInt16();
		mLevel = br.ReadInt32();
		SlotEnabled = br.ReadBoolean();
	}

	public void LearnSkill(short skillID)
	{
		if (mSkillID != 0 && mSkillID != skillID)
		{
			return;
		}
		if (mSkillID == 0)
		{
			mSkillID = skillID;
			mLevel = 1;
		}
		else if (mSkillID == skillID)
		{
			int num = 5;
			if (SlotType == SkillSlotType.YellowSlot || SlotType == SkillSlotType.ClassSkill)
			{
				num = 1;
			}
			if (mLevel < num)
			{
				mLevel++;
			}
		}
	}

	public bool HaveSkill()
	{
		return mSkillID != 0 && mLevel > 0;
	}
}

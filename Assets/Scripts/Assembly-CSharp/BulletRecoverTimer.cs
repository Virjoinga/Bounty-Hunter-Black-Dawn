using System.Collections.Generic;

public class BulletRecoverTimer
{
	public short SkillID;

	public List<SkillTargetType> wTypes = new List<SkillTargetType>();

	public int TriggeredCount;

	public int TotalCount;

	public int RecoverPercentageTick;

	public Timer timer = new Timer();
}

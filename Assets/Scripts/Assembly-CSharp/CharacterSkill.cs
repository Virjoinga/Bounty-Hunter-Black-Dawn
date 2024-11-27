using System.Collections.Generic;

public abstract class CharacterSkill
{
	public short skillID { get; set; }

	public string Name { get; set; }

	public byte SkillLevel { get; set; }

	public SkillTarget Target { get; set; }

	public List<SkillTargetType> TargetTypes { get; set; }

	public SkillTypes SkillType { get; set; }

	public short Distance { get; set; }

	public short Range { get; set; }

	public string IconName { get; set; }

	public string ResourceName { get; set; }

	public string Description1 { get; set; }

	public string Description2 { get; set; }

	public abstract void ApplySkill(GameUnit unit);

	public abstract bool IsInstantSkill();

	public virtual void ReadGeneralSkillProperty(SkillConfig config)
	{
		SkillLevel = config.Level;
		Name = config.Name;
		SkillType = config.SkillType;
		Target = config.Target;
		TargetTypes = new List<SkillTargetType>();
		TargetTypes.Add(config.TargetTypes[0]);
		TargetTypes.Add(config.TargetTypes[1]);
		TargetTypes.Add(config.TargetTypes[2]);
		Distance = config.Distance;
		Range = config.Range;
		IconName = config.IconName;
		ResourceName = config.ResourceName;
		Description1 = config.Description1;
		Description2 = config.Description2;
	}
}

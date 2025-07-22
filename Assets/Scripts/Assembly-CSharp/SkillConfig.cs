using System.Collections.Generic;

public class SkillConfig
{
	public short ID { get; set; }

	public byte Level { get; set; }

	public string Name { get; set; }

	public SkillTypes SkillType { get; set; }

	public SkillTriggerType STriggerType { get; set; }

	public short STriggerTypeSubValue { get; set; }

	public SkillTarget Target { get; set; }

	public List<SkillTargetType> TargetTypes { get; set; }

	public float ApplyDelay { get; set; }

	public short Distance { get; set; }

	public short Range { get; set; }

	public float CoolDownTime { get; set; }

	public SkillFunctionType FunctionType1 { get; set; }

	public int X1 { get; set; }

	public int Y1 { get; set; }

	public SkillFunctionType FunctionType2 { get; set; }

	public int X2 { get; set; }

	public int Y2 { get; set; }

	public SkillFunctionType FunctionType3 { get; set; }

	public int X3 { get; set; }

	public int Y3 { get; set; }

	public string IconName { get; set; }

	public string ResourceName { get; set; }

	public string Description1 { get; set; }

	public string Description2 { get; set; }

	public string CurrentDescribValue { get; set; }

	public string NextDescribValue { get; set; }
}

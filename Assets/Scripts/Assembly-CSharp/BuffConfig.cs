public class BuffConfig
{
	public short ID { get; set; }

	public byte Level { get; set; }

	public string Name { get; set; }

	public byte BuffType { get; set; }

	public BuffFunctionType FunctionType1 { get; set; }

	public int X1 { get; set; }

	public int Y1 { get; set; }

	public BuffFunctionType FunctionType2 { get; set; }

	public int X2 { get; set; }

	public int Y2 { get; set; }

	public BuffFunctionType FunctionType3 { get; set; }

	public int X3 { get; set; }

	public int Y3 { get; set; }

	public string IconName { get; set; }

	public string ResourceName { get; set; }

	public string Description1 { get; set; }

	public string Description2 { get; set; }

	public string CurrentDescribValue { get; set; }

	public string NextDescribValue { get; set; }
}

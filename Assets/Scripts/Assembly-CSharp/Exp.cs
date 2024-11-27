public class Exp
{
	public static int[] RequiredLevelUpExp = new int[Global.MAX_CHAR_LEVEL + 1];

	public static void LoadConfig()
	{
		RequiredLevelUpExp[0] = 0;
		for (int i = 1; i <= Global.MAX_CHAR_LEVEL; i++)
		{
			RequiredLevelUpExp[i] = RequiredLevelUpExp[i - 1] + 150 * ((i + 1) * (i + 1) - 1);
		}
	}
}

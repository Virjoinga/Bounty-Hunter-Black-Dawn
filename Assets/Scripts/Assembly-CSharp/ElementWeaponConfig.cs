public class ElementWeaponConfig
{
	public enum ElementWeaponType
	{
		SHOTGUN = 0,
		REVOLVER = 1,
		PISTOL = 2,
		RPG = 3,
		SNIPER = 4,
		SMG = 5,
		ASSAULT = 6,
		GRENADE = 7
	}

	public static float[] FireDotDamage = new float[7] { 0f, 0.3f, 0.6f, 0.9f, 1.2f, 1.4f, 1.6f };

	public static int[] FireDotTime = new int[7] { 0, 2, 2, 2, 2, 3, 3 };

	public static float[] ShockDotDamage = new float[7] { 0f, 0.8f, 1.1f, 1.4f, 1.7f, 2.2f, 3f };

	public static int[] ShockDotTime = new int[7] { 0, 1, 1, 1, 1, 1, 1 };

	public static float[] CorrosiveDotDamage = new float[7] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f, 1.2f };

	public static int[] CorrosiveDotTime = new int[7] { 0, 3, 3, 3, 3, 4, 4 };

	public static float[] FleshDamageBias = new float[4] { 1.2f, 1f, 1f, 1f };

	public static float[] MechanicalDamageBias = new float[4] { 1f, 1f, 1.2f, 1f };

	public static float[] ShieldDamageBias = new float[4] { 1f, 1.5f, 1f, 1f };

	public static float[] ShieldToFleshBias = new float[4] { 1.7142859f, 0.4f, 1f, 1f };

	public static float[] ShieldToMechanicalBias = new float[4] { 1.1428572f, 0.59999996f, 1.5f, 1f };

	public static int[] ElementDotTriggerTime = new int[8] { 6, 3, 24, 2, 3, 18, 9, 1 };

	public static float[] ElementDotTriggerBase = new float[8] { 0.1f, 0.3f, 0.25f, 0.15f, 0.3f, 0.15f, 0.1f, 1f };

	public static float[] ElementDotTriggerScale = new float[8] { 0.03f, 0.02f, 0.04f, 0.03f, 0.03f, 0.02f, 0.02f, 0f };
}

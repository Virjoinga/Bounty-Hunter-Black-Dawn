using UnityEngine;

public class CharacterMakeDamageSkill : CharacterInstantSkill
{
	public ElementType ElementType { get; set; }

	public override void ApplySkill(GameUnit unit)
	{
		if (CanApplySkill())
		{
			if (unit is Player)
			{
				Player player = unit as Player;
				player.ThrowGrenadeSkill(this);
			}
			lastCastTime = Time.time;
		}
	}
}

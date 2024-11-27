using UnityEngine;

public class TalkState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		if (player.Knife.activeSelf)
		{
			player.CancelMeleeAttack();
		}
		AudioManager.GetInstance().StopSound(player.FootStepAudio);
		player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.Idle, WrapMode.Loop);
	}
}

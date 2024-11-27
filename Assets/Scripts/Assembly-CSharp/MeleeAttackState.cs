using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : PlayerState
{
	private List<string> HitEnemyKeys = new List<string>();

	public override void NextState(Player player, float deltaTime)
	{
		bool inAimState = player.InAimState;
		InputController inputController = player.inputController;
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		WeaponType weaponType = player.GetWeapon().GetWeaponType();
		if (player.IsLocal())
		{
			if (inAimState)
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomIn(deltaTime);
			}
			else
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomOut(deltaTime);
			}
			player.Move(inputController.inputInfo.moveDirection);
			if (inputController.inputInfo.IsMoving() && !player.DYING_STATE.InDyingState)
			{
				player.PlayWalkSound();
			}
			if (player.IsPlayingAnimation(AnimationString.MeleeAttack))
			{
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					HitEnemyForPVP(player);
				}
				else
				{
					HitEnemy(player);
				}
			}
		}
		if (player.IsPlayingAnimation(AnimationString.MeleeAttack) && player.AnimationPlayed(AnimationString.MeleeAttack, 1f))
		{
			if (player.IsLocal())
			{
				ClearHitEnemies();
			}
			player.CancelMeleeAttack();
			player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.Idle, WrapMode.Loop);
			player.SetState(Player.IDLE_STATE);
		}
	}

	private void HitEnemy(Player player)
	{
		foreach (KeyValuePair<string, Enemy> enemy in GameApp.GetInstance().GetGameScene().GetEnemies())
		{
			if (!enemy.Value.InPlayingState() || HitEnemyKeys.Contains(enemy.Key) || enemy.Value == null || enemy.Value.GetObject() == null)
			{
				continue;
			}
			Collider[] componentsInChildren = enemy.Value.GetObject().GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren)
			{
				if (collider.gameObject.layer != PhysicsLayer.ENEMY && collider.gameObject.layer != PhysicsLayer.ENEMY_HEAD)
				{
					continue;
				}
				Vector3 position = Camera.main.transform.position;
				Vector3 position2 = collider.transform.position;
				Vector3 vector = Camera.main.transform.InverseTransformPoint(position2);
				if (!(0f < vector.z) || !(vector.z < 3.5f) || !(-2f < vector.x) || !(vector.x < 2f) || !(-0.5f < vector.y) || !(vector.y < 1f))
				{
					continue;
				}
				Ray ray = new Ray(position, position2 - position);
				float distance = Mathf.Sqrt((position - position2).sqrMagnitude);
				RaycastHit hitInfo;
				if (!Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.ENEMY_HEAD)) || hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY || hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD)
				{
					AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/Knife/knf_melee_hit", hitInfo.point);
					bool flag = false;
					float num = Random.Range(0f, 1f);
					if (hitInfo.collider != null && (hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD || num < player.GetWeapon().CriticalRate))
					{
						flag = true;
					}
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.hitForce = ray.direction * 2f;
					damageProperty.damage = player.MeleeATK;
					if (flag)
					{
						damageProperty.damage = (int)((float)damageProperty.damage * player.GetWeapon().CriticalDamage);
					}
					damageProperty.criticalAttack = flag;
					damageProperty.hitpoint = hitInfo.point;
					damageProperty.isLocal = true;
					damageProperty.wType = WeaponType.Melee;
					damageProperty.isPenetration = false;
					damageProperty.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
					damageProperty.weaponLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
					enemy.Value.HitEnemy(damageProperty);
					HitEnemyKeys.Add(enemy.Key);
					break;
				}
			}
		}
	}

	private void HitEnemyForPVP(Player player)
	{
		foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
		{
			if (remotePlayer == null || !remotePlayer.InPlayingState() || remotePlayer.IsSameTeam(player) || HitEnemyKeys.Contains(remotePlayer.GetDisplayName()) || remotePlayer.GetObject() == null)
			{
				continue;
			}
			Collider[] componentsInChildren = remotePlayer.GetObject().GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren)
			{
				if (collider.gameObject.layer != PhysicsLayer.REMOTE_PLAYER)
				{
					continue;
				}
				Vector3 position = Camera.main.transform.position;
				Vector3 position2 = collider.transform.position;
				Vector3 vector = Camera.main.transform.InverseTransformPoint(position2);
				if (!(0f < vector.z) || !(vector.z < 3.2f) || !(-1.2f < vector.x) || !(vector.x < 1.2f) || !(-2f < vector.y) || !(vector.y < 0.5f))
				{
					continue;
				}
				Ray ray = new Ray(position, position2 - position);
				float distance = Mathf.Sqrt((position - position2).sqrMagnitude);
				RaycastHit hitInfo;
				if (!Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.REMOTE_PLAYER)) || hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
				{
					AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/Knife/knf_melee_hit", hitInfo.point);
					bool flag = false;
					float num = Random.Range(0f, 1f);
					if (hitInfo.collider != null && num < player.GetWeapon().CriticalRate)
					{
						flag = true;
					}
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.hitForce = ray.direction * 2f;
					damageProperty.damage = player.MeleeATK;
					if (flag)
					{
						damageProperty.damage = (int)((float)damageProperty.damage * player.GetWeapon().CriticalDamage);
					}
					damageProperty.criticalAttack = flag;
					damageProperty.hitpoint = hitInfo.point;
					damageProperty.isLocal = true;
					damageProperty.wType = WeaponType.Melee;
					damageProperty.isPenetration = false;
					damageProperty.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
					damageProperty.weaponLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(player.GetUserID(), (short)damageProperty.damage, remotePlayer.GetUserID(), false, (byte)damageProperty.elementType, damageProperty.criticalAttack, damageProperty.isTriggerDlementDot, damageProperty.elementDotDamage, damageProperty.elementDotTime, damageProperty.wType, DamageProperty.AttackerType._PlayerOrEnemy);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					HitEnemyKeys.Add(remotePlayer.GetDisplayName());
					break;
				}
			}
		}
		foreach (RemotePlayer remotePlayer2 in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
		{
			if (remotePlayer2 == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, SummonedItem> summoned in remotePlayer2.GetSummonedList())
			{
				if (summoned.Value == null || !summoned.Value.InPlayingState() || summoned.Value.IsSameTeam() || HitEnemyKeys.Contains(summoned.Key) || summoned.Value.GetObject() == null)
				{
					continue;
				}
				Collider[] componentsInChildren2 = summoned.Value.GetObject().GetComponentsInChildren<Collider>();
				foreach (Collider collider2 in componentsInChildren2)
				{
					if (collider2.gameObject.layer != PhysicsLayer.SUMMONED)
					{
						continue;
					}
					Vector3 position3 = Camera.main.transform.position;
					Vector3 position4 = collider2.transform.position;
					Vector3 vector2 = Camera.main.transform.InverseTransformPoint(position4);
					Debug.Log(summoned.Key + vector2);
					if (!(0f < vector2.z) || !(vector2.z < 3.2f) || !(-1.2f < vector2.x) || !(vector2.x < 1.2f) || !(-1.5f < vector2.y) || !(vector2.y < 0.7f))
					{
						continue;
					}
					Ray ray2 = new Ray(position3, position4 - position3);
					float distance2 = Mathf.Sqrt((position3 - position4).sqrMagnitude);
					RaycastHit hitInfo2;
					if (!Physics.Raycast(ray2, out hitInfo2, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) || hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
					{
						AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/Knife/knf_melee_hit", hitInfo2.point);
						bool flag2 = false;
						float num2 = Random.Range(0f, 1f);
						if (hitInfo2.collider != null && num2 < player.GetWeapon().CriticalRate)
						{
							flag2 = true;
						}
						DamageProperty damageProperty2 = new DamageProperty();
						damageProperty2.hitForce = ray2.direction * 2f;
						damageProperty2.damage = player.MeleeATK;
						if (flag2)
						{
							damageProperty2.damage = (int)((float)damageProperty2.damage * player.GetWeapon().CriticalDamage);
						}
						damageProperty2.criticalAttack = flag2;
						damageProperty2.hitpoint = hitInfo2.point;
						damageProperty2.isLocal = true;
						damageProperty2.wType = WeaponType.Melee;
						damageProperty2.isPenetration = false;
						damageProperty2.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
						damageProperty2.weaponLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
						ControllableItemOnHitRequest request2 = new ControllableItemOnHitRequest(summoned.Value.ControllableType, summoned.Value.ID, damageProperty2.damage);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						HitEnemyKeys.Add(summoned.Key);
						break;
					}
				}
			}
		}
	}

	public void ClearHitEnemies()
	{
		HitEnemyKeys.Clear();
	}
}

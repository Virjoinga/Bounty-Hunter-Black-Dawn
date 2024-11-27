using UnityEngine;

public class CharacterSummonSkill : CharacterInstantSkill
{
	public float Duration { get; set; }

	public float DurationInit { get; set; }

	public byte SummonedType { get; set; }

	public override void ApplySkill(GameUnit unit)
	{
		if (!CanApplySkill())
		{
			return;
		}
		if (base.SEffectType == SkillEffectType.Summon)
		{
			Debug.Log("Summon!");
			Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			Ray ray = new Ray(localPlayer.GetTransform().position, localPlayer.GetTransform().forward);
			RaycastHit[] array = Physics.RaycastAll(ray, 3f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.ENEMY_HEAD) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.REMOTE_PLAYER));
			if (array.Length != 0)
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_SKILL_IN_WRONG_PLACE"));
				return;
			}
			RaycastHit hitInfo = default(RaycastHit);
			ray = new Ray(localPlayer.GetTransform().position + localPlayer.GetTransform().forward * 3f + Vector3.up * 2f, Vector3.down);
			if (!Physics.Raycast(ray, out hitInfo, 5f, (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.WALL)))
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_SKILL_IN_WRONG_PLACE"));
				return;
			}
			if (hitInfo.collider.gameObject.layer == PhysicsLayer.WALL)
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_SKILL_IN_WRONG_PLACE"));
				return;
			}
			SummonedItem summonedItem = null;
			switch (SummonedType)
			{
			case 0:
				summonedItem = new EngineerGun();
				break;
			case 1:
				summonedItem = new HealingStation();
				break;
			case 2:
				summonedItem = new Traps();
				break;
			}
			if (summonedItem == null)
			{
				return;
			}
			summonedItem.ID = GameApp.GetInstance().GetGameScene().GetNextSummonedID();
			summonedItem.Level = (byte)GameApp.GetInstance().GetUserState().GetCharLevel();
			summonedItem.Duration = Duration;
			summonedItem.MaxHp = 100 * summonedItem.Level * 2;
			summonedItem.Hp = summonedItem.MaxHp;
			summonedItem.MaxShield = 100 * summonedItem.Level * 2;
			summonedItem.Shield = summonedItem.MaxShield;
			summonedItem.ShieldRecovery = 20;
			summonedItem.IsMaster = true;
			summonedItem.SetOwnerPlayer(localPlayer);
			summonedItem.Position = localPlayer.GetTransform().position + localPlayer.GetTransform().forward * 3f;
			summonedItem.Rotation = Quaternion.LookRotation(localPlayer.GetTransform().forward);
			if (SummonedType == 2)
			{
				hitInfo = default(RaycastHit);
				ray = new Ray(summonedItem.Position + Vector3.up * 2f, Vector3.down);
				if (Physics.Raycast(ray, out hitInfo, 10f, 1 << PhysicsLayer.FLOOR))
				{
					summonedItem.Position = hitInfo.point;
					summonedItem.Rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
				}
			}
			summonedItem.Init();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				ControllableItemCreateRequest request = new ControllableItemCreateRequest(EControllableType.SUMMONED, summonedItem.ID, summonedItem.Level, (byte)summonedItem.SummonedType, summonedItem.GetTransform().position, summonedItem.GetTransform().rotation, summonedItem.GetPara1(), summonedItem.GetPara2(), summonedItem.GetPara3(), summonedItem.GetPara4(), summonedItem.GetPara5(), summonedItem.GetPara6(), summonedItem.GetPara7(), summonedItem.GetPara8(), summonedItem.GetPara9(), summonedItem.GetPara10(), summonedItem.GetPara11(), summonedItem.GetPara12(), summonedItem.GetPara13(), summonedItem.GetPara14(), summonedItem.GetPara15(), summonedItem.GetPara16());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		if (SummonedType == 2)
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Finally_you_got_caught, AchievementTrigger.Type.Data);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
		}
		lastCastTime = Time.time;
	}
}

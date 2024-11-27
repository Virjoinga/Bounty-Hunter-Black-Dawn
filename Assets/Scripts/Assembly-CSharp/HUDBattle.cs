using System;
using System.Collections.Generic;
using UnityEngine;

public class HUDBattle : HUDTemplate, HotKeyListener
{
	private float mLastUpdateAimTime;

	private bool bDyingState;

	public GameObject TestPrefab;

	public GameObject TestPrefab2;

	private List<GameObject> TestList = new List<GameObject>();

	private List<GameObject> TestList2 = new List<GameObject>();

	protected override void OnInit()
	{
		UserStateHUD.GetInstance().AddLocalPlayer();
	}

	protected override void OnShow()
	{
		UserStateHUD.GetInstance().InfoBox.CloseNumberInfo();
		HUDManager.instance.m_Joystick.SetAllActiveRecursively(true);
		HUDManager.instance.m_HotKeyManager.SetAllActiveRecursively(true);
		HUDManager.instance.m_InfoManager.SetAllActiveRecursively(true);
		NGUITools.SetActive(HUDManager.instance.m_InfoManager.m_Aim, false);
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
			if (localPlayer != null && localPlayer.InAimState)
			{
				localPlayer.Aim(true);
			}
		}
		HUDManager.instance.m_ChatManager.SetAllActiveRecursively(false);
		HUDManager.instance.m_OtherManager.SetAllActiveRecursively(true);
		HUDManager.instance.m_HotKeyManager.m_OffLine.SetActive(false);
		NGUITools.SetActive(HUDManager.instance.m_OtherManager.m_Achievement, false);
		NGUITools.SetActive(HUDManager.instance.m_OtherManager.m_SaveIcon, false);
		HUDManager.instance.m_DyingManager.SetAllActiveRecursively(true);
		HUDManager.instance.m_DyingManager.m_Saver.SetActive(false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Grenade, UserStateHUD.GetInstance().IsHasGrenade());
		HUDManager.instance.m_InfoManager.m_UnhurtState.SetActive(false);
		HUDManager.instance.m_HotKeyManager.m_FillAll.SetActive(false);
	}

	protected override void OnHide()
	{
		CloseWeaponList();
		HUDManager.instance.m_Joystick.SetAllActiveRecursively(false);
		HUDManager.instance.m_HotKeyManager.SetAllActiveRecursively(false);
		HUDManager.instance.m_InfoManager.SetAllActiveRecursively(false);
		HUDManager.instance.m_ChatManager.SetAllActiveRecursively(false);
		HUDManager.instance.m_OtherManager.SetAllActiveRecursively(false);
		HUDManager.instance.m_DyingManager.SetAllActiveRecursively(false);
	}

	protected override void OnUpdate()
	{
		if (Time.time - mLastUpdateAimTime > 0.2f)
		{
			mLastUpdateAimTime = Time.time;
			OnHandleRay(CreateRay());
			GameObject fillAll = HUDManager.instance.m_HotKeyManager.m_FillAll;
			if (UserStateHUD.GetInstance().IsShowFillAllButton())
			{
				if (!fillAll.activeSelf)
				{
					fillAll.SetActive(true);
				}
			}
			else if (fillAll.activeSelf)
			{
				fillAll.SetActive(false);
			}
			if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.IsUnhurtNow())
			{
				if (!HUDManager.instance.m_InfoManager.m_UnhurtState.activeSelf)
				{
					HUDManager.instance.m_InfoManager.m_UnhurtState.SetActive(true);
					HUDManager.instance.m_OtherManager.m_InjuredEffect.SetActive(false);
				}
			}
			else if (HUDManager.instance.m_InfoManager.m_UnhurtState.activeSelf)
			{
				HUDManager.instance.m_InfoManager.m_UnhurtState.SetActive(false);
				HUDManager.instance.m_OtherManager.m_InjuredEffect.SetActive(true);
			}
			if (UserStateHUD.GetInstance().IsUserDying() || UserStateHUD.GetInstance().IsUserDead())
			{
				if (!bDyingState)
				{
					bDyingState = true;
					OnPlayerDead();
				}
			}
			else
			{
				if (bDyingState)
				{
					bDyingState = false;
					OnPlayerRespawn();
				}
				if (GameApp.GetInstance().GetUserState().ItemInfoData.HasPillInBag())
				{
					if (!HUDManager.instance.m_HotKeyManager.m_Pill.activeSelf)
					{
						NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Pill, true);
					}
				}
				else if (HUDManager.instance.m_HotKeyManager.m_Pill.activeSelf)
				{
					NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Pill, false);
				}
				if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetWeapon()
					.GetWeaponType() == WeaponType.RPG)
				{
					if (HUDManager.instance.m_HotKeyManager.m_Aim.activeSelf)
					{
						NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Aim, false);
					}
				}
				else if (!HUDManager.instance.m_HotKeyManager.m_Aim.activeSelf)
				{
					NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Aim, true);
				}
				if (!UserStateHUD.GetInstance().IsHasGrenade())
				{
					if (HUDManager.instance.m_HotKeyManager.m_Grenade.activeSelf)
					{
						NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Grenade, false);
					}
				}
				else if (!HUDManager.instance.m_HotKeyManager.m_Grenade.activeSelf)
				{
					NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Grenade, true);
				}
			}
		}
		if (!Input.GetKeyDown(KeyCode.T))
		{
		}
	}

	protected virtual void OnHandleRay(Ray ray)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.ENEMY_HEAD) | (1 << PhysicsLayer.REMOTE_PLAYER)))
		{
			if (hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY || hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD)
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(GetParentName(hitInfo.collider.gameObject));
				if (enemyByID.CanBeHit())
				{
					UserStateHUD.GetInstance().SetTargetAimed(enemyByID);
				}
			}
			else if (hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(Convert.ToInt32(GetParentName(hitInfo.collider.gameObject)));
				UserStateHUD.GetInstance().SetTargetAimed(remotePlayerByUserID);
			}
			else
			{
				UserStateHUD.GetInstance().CancelTargetAimed();
			}
		}
		if (Physics.Raycast(ray, out hitInfo, 4f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.ITEMS)))
		{
			if (hitInfo.collider.gameObject.layer == PhysicsLayer.ITEMS)
			{
				ItemBase itemBase = hitInfo.collider.GetComponent<ItemBase>();
				if (itemBase == null)
				{
					itemBase = NGUITools.FindInParents<ItemBase>(hitInfo.collider.gameObject);
				}
				ChestScript chestScript = null;
				if (itemBase == null)
				{
					chestScript = hitInfo.collider.GetComponent<ChestScript>();
					if (chestScript == null)
					{
						chestScript = NGUITools.FindInParents<ChestScript>(hitInfo.collider.gameObject);
					}
				}
				UserStateHUD.GetInstance().ShowInteractButton(itemBase, chestScript);
			}
			else
			{
				UserStateHUD.GetInstance().HideInteractButton();
			}
		}
		else
		{
			UserStateHUD.GetInstance().HideInteractButton();
		}
	}

	private Ray CreateRay()
	{
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 normalized = (vector - transform.position).normalized;
		Ray result = new Ray(transform.position + 1.8f * normalized, normalized);
		Debug.DrawRay(transform.position + 1.8f * normalized, normalized * 1000f, Color.green);
		return result;
	}

	protected virtual void OnPlayerDead()
	{
		NGUITools.SetActive(HUDManager.instance.m_InfoManager.m_UserState, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Menu, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Grenade, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Aim, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Return, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Mission, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Skill1, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Skill2, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_SkillPoint, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Map, false);
		NGUITools.SetActive(HUDManager.instance.m_InfoManager.m_MapState, false);
		NGUITools.SetActive(HUDManager.instance.m_InfoManager.m_GrenadeBullet, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Pill, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_SwapOrReload, false);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Melee, false);
		NGUITools.SetActive(HUDManager.instance.m_Joystick.m_ShootJoystick.gameObject, false);
	}

	protected virtual void OnPlayerRespawn()
	{
		NGUITools.SetActive(HUDManager.instance.m_InfoManager.m_UserState, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Menu, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Grenade, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Return, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Mission, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Skill1, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Skill2, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_SkillPoint, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Map, true);
		NGUITools.SetActive(HUDManager.instance.m_InfoManager.m_MapState, true);
		NGUITools.SetActive(HUDManager.instance.m_InfoManager.m_GrenadeBullet, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Pill, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_SwapOrReload, true);
		NGUITools.SetActive(HUDManager.instance.m_HotKeyManager.m_Melee, true);
		NGUITools.SetActive(HUDManager.instance.m_Joystick.m_ShootJoystick.gameObject, true);
	}

	protected string GetParentName(GameObject go)
	{
		GameObject gameObject = go;
		int num = 0;
		while (gameObject.transform.parent != null && num < 20)
		{
			gameObject = gameObject.transform.parent.gameObject;
			num++;
		}
		return gameObject.name;
	}

	protected override void OnClose()
	{
		UserStateHUD.GetInstance().RemoveLocalPlayer();
	}

	public virtual void OnHotKeyEvent(UIButtonX.ButtonInfo info)
	{
		switch (info.buttonId)
		{
		case 2:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				Aim();
			}
			break;
		case 6:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				ShowMenu();
			}
			break;
		case 3:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.Reload();
				CloseWeaponList();
			}
			else if (info.buttonEvent == UIButtonX.ButtonInfo.Event.NotifyEnd)
			{
				CloseWeaponList();
			}
			else if (!UserStateHUD.GetInstance().IsUserDying() && !UserStateHUD.GetInstance().IsUserDead() && info.buttonEvent == UIButtonX.ButtonInfo.Event.Pressing)
			{
				OpenWeaponList();
			}
			break;
		case 9:
		case 10:
		case 11:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click || info.buttonEvent == UIButtonX.ButtonInfo.Event.Drop)
			{
				if (!UserStateHUD.GetInstance().IsUserDying() && !UserStateHUD.GetInstance().IsUserDead())
				{
					UserStateHUD.GetInstance().ChangeWeapon(info.buttonId - 9);
				}
				CloseWeaponList();
			}
			break;
		case 4:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				UserStateHUD.GetInstance().Skill1.Apply();
			}
			break;
		case 5:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				UserStateHUD.GetInstance().Skill2.Apply();
			}
			break;
		case 8:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.MeleeAttack();
			}
			break;
		case 0:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.ThrowGrenade();
			}
			break;
		case 1:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				ReturnMenu.GetInstance().Show();
			}
			break;
		case 16:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				InGameMenuManager.GetInstance().Show(1);
			}
			break;
		case 17:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				InGameMenuManager.GetInstance().Show(2);
			}
			break;
		case 18:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				InGameMenuManager.GetInstance().Show(3);
			}
			break;
		case 19:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				UserStateHUD.GetInstance().SetMissionUpdateDelay(0f);
				UserStateHUD.GetInstance().SwitchToNextMission();
				UserStateHUD.GetInstance().UpdateMission();
			}
			break;
		case 22:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				GameApp.GetInstance().GetUserState().ItemInfoData.UsePill();
			}
			break;
		case 23:
		{
			if (info.buttonEvent != 0)
			{
				break;
			}
			LocalPlayer localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer2.GetInstanceRespawnCost() <= GameApp.GetInstance().GetUserState().GetCash())
			{
				GameApp.GetInstance().GetUserState().Buy(localPlayer2.GetInstanceRespawnCost());
				localPlayer2.InstanceRespawn();
				for (int i = 0; i < 8; i++)
				{
					WeaponType type = (WeaponType)(i + 1);
					GameApp.GetInstance().GetUserState().AddBulletByWeaponType(type, 1000);
				}
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					PlayerRecoverFromDyingRequest request = new PlayerRecoverFromDyingRequest(100);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				else
				{
					localPlayer2.DYING_STATE.OnRecoverFromDying(localPlayer2.MaxHp, localPlayer2.MaxShield);
				}
			}
			break;
		}
		case 24:
			if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Click)
			{
				LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				localPlayer.DYING_STATE.OnDead();
			}
			break;
		case 7:
		case 12:
		case 13:
		case 14:
		case 15:
		case 20:
		case 21:
			break;
		}
	}

	protected void Aim()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer.CanAim())
		{
			if (localPlayer.InAimState)
			{
				localPlayer.Aim(false);
			}
			else
			{
				localPlayer.Aim(true);
			}
		}
	}

	protected void ShowMenu()
	{
		if (GameApp.GetInstance().GetUIStateManager().FrGetNextPhase() == 6)
		{
			InGameMenuManager.GetInstance().ShowMenu();
			AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_ok");
		}
	}

	protected void OpenWeaponList()
	{
		if (!UserStateHUD.GetInstance().IsWeaponListOpen())
		{
			HUDManager.instance.m_HotKeyManager.OpenWeaponList();
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.RotateCamera = false;
			UserStateHUD.GetInstance().SetWeaponListOpen(true);
		}
	}

	protected void CloseWeaponList()
	{
		if (UserStateHUD.GetInstance().IsWeaponListOpen())
		{
			HUDManager.instance.m_HotKeyManager.CloseWeaponList();
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.RotateCamera = true;
			UserStateHUD.GetInstance().SetWeaponListOpen(false);
		}
	}
}

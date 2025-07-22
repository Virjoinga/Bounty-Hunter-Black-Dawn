using System;
using UnityEngine;

public class HUDCoopBattle : HUDMultiplayBattle
{
	private RemotePlayer remotePlayer;

	private float mLastUpdateAimTime;

	protected override void OnShow()
	{
		base.OnShow();
		HUDManager.instance.m_DyingManager.m_Saver.SetActive(true);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	protected override void OnHandleRay(Ray ray)
	{
		base.OnHandleRay(ray);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)))
		{
			if (hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(Convert.ToInt32(GetParentName(hitInfo.collider.gameObject)));
				LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(Convert.ToInt32(GetParentName(hitInfo.collider.gameObject)));
				if (remotePlayerByUserID != null)
				{
					if (remotePlayerByUserID.DYING_STATE.InDyingState && !localPlayer.DYING_STATE.InDyingState)
					{
						remotePlayer = remotePlayerByUserID;
						UserStateHUD.GetInstance().SetDyingRemotoPlayerInSight(base.gameObject);
					}
					else
					{
						remotePlayer = null;
						UserStateHUD.GetInstance().SetDyingRemotoPlayerInSight(null);
					}
				}
				else
				{
					remotePlayer = null;
					UserStateHUD.GetInstance().SetDyingRemotoPlayerInSight(null);
				}
			}
			else
			{
				remotePlayer = null;
				UserStateHUD.GetInstance().SetDyingRemotoPlayerInSight(null);
			}
		}
		else
		{
			remotePlayer = null;
		}
	}

	public override void OnHotKeyEvent(UIButtonX.ButtonInfo info)
	{
		base.OnHotKeyEvent(info);
		int buttonId = info.buttonId;
		if (buttonId != 14)
		{
			return;
		}
		if (info.buttonEvent == UIButtonX.ButtonInfo.Event.Pressing)
		{
			if (remotePlayer != null)
			{
				PlayerFirstAidTeammateRequest request = new PlayerFirstAidTeammateRequest(remotePlayer.GetUserID(), FirstAidPhase.Begin);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				Debug.Log("Start To Save");
			}
		}
		else if (info.buttonEvent == UIButtonX.ButtonInfo.Event.ReleaseAfterPressing)
		{
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (UserStateHUD.GetInstance().StopToSave() && !localPlayer.GetFirstAidTimer().Ready() && remotePlayer != null)
			{
				PlayerFirstAidTeammateRequest request2 = new PlayerFirstAidTeammateRequest(remotePlayer.GetUserID(), FirstAidPhase.Failed);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				Debug.Log("Stop To Save");
			}
		}
	}

	protected void OnSaveSuccess()
	{
		if (UserStateHUD.GetInstance().StopToSave())
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.This_is_Friendship);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			Debug.Log("Save Success");
		}
	}
}

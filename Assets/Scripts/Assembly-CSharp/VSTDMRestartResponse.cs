using System.Collections.Generic;
using UnityEngine;

internal class VSTDMRestartResponse : Response
{
	protected byte mBuyBulletTime;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mBuyBulletTime = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
			if (localPlayer != null)
			{
				localPlayer.GetCharacterSkillManager().ResetAllInstantSkills();
				localPlayer.GetCharacterSkillManager().RemoveAllBuff();
				localPlayer.DropAtSpawnPositionVS();
				localPlayer.ClearExtraShield();
				localPlayer.GetUserState().GetVSTDMBattleState().Init();
				localPlayer.ClearSummonedList();
			}
			List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item != null)
				{
					UserState userState = item.GetUserState();
					if (userState != null)
					{
						userState.GetVSTDMBattleState().Init();
					}
				}
			}
		}
		UIVSBattleResult.Close();
		Debug.Log("Buy Bullet Time......." + mBuyBulletTime);
		UserStateHUD.GetInstance().GetVSBattleFieldState().Clear();
		UIVSBattleShop.ShowReady();
	}
}

using UnityEngine;

public class PlayThirdPersonSkillEffectResponse : Response
{
	protected int playerID;

	protected PlayThirdPersonSkillEffectRequest.SkillEffectType effectType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		effectType = (PlayThirdPersonSkillEffectRequest.SkillEffectType)bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null && effectType == PlayThirdPersonSkillEffectRequest.SkillEffectType.HealingWave)
		{
			EffectPlayer.GetInstance().PlayHealingWave(remotePlayerByUserID.GetPosition() + Vector3.up * 1f);
		}
	}
}

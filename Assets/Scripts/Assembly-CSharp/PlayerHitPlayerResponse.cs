using UnityEngine;

internal class PlayerHitPlayerResponse : Response
{
	protected int attackerID;

	protected int playerID;

	protected int m_extrashield;

	protected int m_shield;

	protected int m_hp;

	protected bool block;

	protected int mDamage;

	protected bool mIsCritical;

	protected WeaponType mWeaponType;

	protected DamageProperty.AttackerType mAttackerType;

	protected ElementType mElementType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		attackerID = bytesBuffer.ReadInt();
		playerID = bytesBuffer.ReadInt();
		m_extrashield = bytesBuffer.ReadInt();
		m_shield = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadInt();
		block = bytesBuffer.ReadBool();
		mDamage = bytesBuffer.ReadInt();
		mIsCritical = bytesBuffer.ReadBool();
		mWeaponType = (WeaponType)bytesBuffer.ReadByte();
		mAttackerType = (DamageProperty.AttackerType)bytesBuffer.ReadByte();
		mElementType = (ElementType)bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			localPlayer.UnderAttackSetHP(m_hp);
			localPlayer.UnderAttackSetShield(m_shield);
			localPlayer.UnderAttackSetExtraShield(m_extrashield);
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID == null)
		{
			return;
		}
		remotePlayerByUserID.UnderAttackSetHP(m_hp);
		remotePlayerByUserID.UnderAttackSetShield(m_shield);
		remotePlayerByUserID.UnderAttackSetExtraShield(m_extrashield);
		if (!block)
		{
			if (mIsCritical)
			{
				remotePlayerByUserID.CreateDeadBlood();
			}
			else if (GameApp.GetInstance().GetGlobalState().GetBloodSpraying() && remotePlayerByUserID.GetLastBloodEffectTimer().Ready())
			{
				gameScene.GetEffectPool(EffectPoolType.HIT_BLOOD).CreateObject(remotePlayerByUserID.GetTransform().position + Vector3.up * 1.5f, Vector3.zero, Quaternion.identity);
				remotePlayerByUserID.GetLastBloodEffectTimer().Do();
			}
			remotePlayerByUserID.ApplySkillsForAttacker(attackerID, mIsCritical, mWeaponType, mAttackerType);
			UserStateHUD.GetInstance().PushDamage(mDamage, mIsCritical, mElementType, remotePlayerByUserID.GetPosition() + Vector3.up * 1.5f);
		}
	}
}

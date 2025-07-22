internal class ChangeRemotePlayerStateResponse : Response
{
	protected int targetPlayerID;

	protected short skillID;

	protected short iconID;

	protected short duration;

	protected byte modifierOfBuff;

	protected byte propertyChangeType;

	protected short buffValueY;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		targetPlayerID = bytesBuffer.ReadInt();
		skillID = bytesBuffer.ReadShort();
		iconID = bytesBuffer.ReadShort();
		duration = bytesBuffer.ReadShort();
		modifierOfBuff = bytesBuffer.ReadByte();
		propertyChangeType = bytesBuffer.ReadByte();
		buffValueY = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (targetPlayerID == channelID)
		{
			CharacterStateSkill characterStateSkill = new CharacterStateSkill();
			characterStateSkill.skillID = skillID;
			characterStateSkill.IsPermanent = false;
			characterStateSkill.Duration = (float)duration / 10f;
			characterStateSkill.ModifierOfBuff1 = (BuffModifier)modifierOfBuff;
			characterStateSkill.PropertyChangeTypeOfBuff1 = (PropertyChangeType)propertyChangeType;
			characterStateSkill.BuffValueY1 = buffValueY;
			characterStateSkill.FunctionType1 = BuffFunctionType.PropertyChange;
			characterStateSkill.IconName = GameConfig.GetInstance().skillConfig[iconID * 10 + 1].IconName;
			CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
			characterSkillManager.AddSkill(characterStateSkill);
			characterStateSkill.StartBuff();
			if (propertyChangeType == 19)
			{
				PlayRemotePlayerBuffEffectRequest request = new PlayRemotePlayerBuffEffectRequest(8);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}
}

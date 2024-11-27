using UnityEngine;

public class ChangeRemotePlayerStateRequest : Request
{
	protected int targetPlayerID;

	protected short skillID;

	protected short iconID;

	protected short duration;

	protected byte modifierOfBuff;

	protected byte propertyChangeType;

	protected short buffValueY;

	public ChangeRemotePlayerStateRequest(int targetID, CharacterStateSkill skill, short iconID)
	{
		requestID = 192;
		targetPlayerID = targetID;
		skillID = skill.skillID;
		duration = (short)Mathf.CeilToInt(skill.Duration * 10f);
		if (skill.FunctionType1 == BuffFunctionType.SpeedDown)
		{
			modifierOfBuff = 1;
			propertyChangeType = 19;
			buffValueY = (short)(0f - skill.BuffValueY1);
		}
		else
		{
			modifierOfBuff = (byte)skill.ModifierOfBuff1;
			propertyChangeType = (byte)skill.PropertyChangeTypeOfBuff1;
			buffValueY = (short)skill.BuffValueY1;
		}
		this.iconID = iconID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(14);
		bytesBuffer.AddInt(targetPlayerID);
		bytesBuffer.AddShort(skillID);
		bytesBuffer.AddShort(iconID);
		bytesBuffer.AddShort(duration);
		bytesBuffer.AddByte(modifierOfBuff);
		bytesBuffer.AddByte(propertyChangeType);
		bytesBuffer.AddShort(buffValueY);
		return bytesBuffer.GetBytes();
	}
}

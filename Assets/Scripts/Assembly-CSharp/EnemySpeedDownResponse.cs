using UnityEngine;

internal class EnemySpeedDownResponse : Response
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected short mSpeedDownSkillID;

	protected short mDuration;

	protected byte mSpeedDownRate;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPointID = bytesBuffer.ReadByte();
		mEnemyID = bytesBuffer.ReadByte();
		mSpeedDownSkillID = bytesBuffer.ReadShort();
		mDuration = bytesBuffer.ReadShort();
		mSpeedDownRate = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		Enemy enemy = GameApp.GetInstance().GetGameWorld().GetEnemy(mPointID, mEnemyID);
		if (enemy != null && enemy.InPlayingState())
		{
			CharacterStateSkill characterStateSkill = new CharacterStateSkill();
			characterStateSkill.skillID = mSpeedDownSkillID;
			characterStateSkill.IsPermanent = false;
			characterStateSkill.Duration = (float)mDuration / 10f;
			characterStateSkill.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
			characterStateSkill.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeMoveSpeed;
			characterStateSkill.BuffValueY1 = mSpeedDownRate - 256;
			characterStateSkill.FunctionType1 = BuffFunctionType.PropertyChange;
			Debug.Log(characterStateSkill.Duration + "|||||||||||" + characterStateSkill.BuffValueY1);
			CharacterSkillManager characterSkillManager = enemy.GetCharacterSkillManager();
			characterSkillManager.AddSkill(characterStateSkill);
			characterStateSkill.StartBuff();
		}
	}
}

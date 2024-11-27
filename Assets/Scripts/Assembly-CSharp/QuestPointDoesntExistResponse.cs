using UnityEngine;

internal class QuestPointDoesntExistResponse : Response
{
	private byte mPointID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPointID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.QUEST_ENEMY_SPAWN);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			QuestEnemySpawnScript component = gameObject.GetComponent<QuestEnemySpawnScript>();
			if (null != component && component.PointID == mPointID)
			{
				component.InitEnemyFromGameWorld();
				break;
			}
		}
	}
}

using UnityEngine;

internal class OpenChestResponse : Response
{
	protected short m_chestID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_chestID = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.CHEST);
		if (array == null)
		{
			return;
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			ChestScript component = gameObject.GetComponent<ChestScript>();
			if (component != null && component.GetChestId() == m_chestID)
			{
				component.PlayOpen();
				component.IsAlreadyOpen = true;
				break;
			}
		}
	}
}

using UnityEngine;

internal class PickUpItemResponse : Response
{
	protected short m_sequenceID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_sequenceID = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameObject gameObject = GameObject.Find("QuestItem_" + m_sequenceID);
		if (gameObject != null)
		{
			ItemBase component = gameObject.GetComponent<ItemBase>();
			if (component != null)
			{
				component.PickUpItem();
			}
		}
	}
}

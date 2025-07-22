using UnityEngine;

internal class ItemSpawnResponse : Response
{
	protected short m_sequenceID;

	protected short m_px;

	protected short m_py;

	protected short m_pz;

	protected short m_fx;

	protected short m_fy;

	protected short m_fz;

	protected short m_specialID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_sequenceID = bytesBuffer.ReadShort();
		m_px = bytesBuffer.ReadShort();
		m_py = bytesBuffer.ReadShort();
		m_pz = bytesBuffer.ReadShort();
		m_fx = bytesBuffer.ReadShort();
		m_fy = bytesBuffer.ReadShort();
		m_fz = bytesBuffer.ReadShort();
		m_specialID = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		Vector3 position = new Vector3((float)m_px / 10f, (float)m_py / 10f, (float)m_pz / 10f);
		Vector3 force = new Vector3((float)m_fx / 10f, (float)m_fy / 10f, (float)m_fz / 10f);
		GameApp.GetInstance().GetLootManager().SpawnQuestItem(position, force, m_specialID, m_sequenceID);
	}
}

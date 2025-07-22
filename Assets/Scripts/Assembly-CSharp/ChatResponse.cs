using UnityEngine;

public class ChatResponse : Response
{
	private string m_name;

	private string m_text;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		bytesBuffer.ReadInt();
		m_name = bytesBuffer.ReadString();
		m_text = bytesBuffer.ReadString();
		Debug.Log(m_name);
		Debug.Log(m_text);
	}

	public override void ProcessLogic()
	{
		UserStateHUD.GetInstance().ChatBox.Add(m_name + ":" + m_text);
		Debug.Log("ProcessLogic : " + m_text);
	}
}

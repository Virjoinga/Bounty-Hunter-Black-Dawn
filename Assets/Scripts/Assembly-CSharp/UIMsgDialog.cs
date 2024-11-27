using UnityEngine;

public class UIMsgDialog : MonoBehaviour
{
	public UIMsg msg;

	public UITextList textList;

	private void OnEnable()
	{
		textList.Add(msg.m_Information.text);
	}
}

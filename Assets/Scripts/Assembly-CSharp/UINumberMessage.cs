using UnityEngine;

public class UINumberMessage : MonoBehaviour
{
	public UILabel m_Str;

	public UILabel m_Number;

	public UITweenX m_Tween;

	private void OnEnable()
	{
		m_Str.gameObject.SetActive(false);
		m_Number.gameObject.SetActive(false);
	}

	public void SetMessage(string str, int number)
	{
		if (str.Equals("/Hide"))
		{
			m_Str.gameObject.SetActive(false);
			m_Number.gameObject.SetActive(false);
			return;
		}
		m_Str.gameObject.SetActive(true);
		m_Number.gameObject.SetActive(true);
		m_Str.text = str;
		m_Number.text = string.Empty + number;
		m_Tween.Play();
	}
}

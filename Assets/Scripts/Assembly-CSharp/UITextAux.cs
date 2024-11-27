using System.Text.RegularExpressions;
using UnityEngine;

public class UITextAux : MonoBehaviour
{
	public UIInput m_input;

	private void Start()
	{
		if (m_input == null)
		{
			m_input = GetComponent<UIInput>();
		}
	}

	private void OnInput(string text)
	{
		if (m_input != null)
		{
			m_input.text = Regex.Replace(m_input.text, "[^0-9]", string.Empty);
		}
	}
}

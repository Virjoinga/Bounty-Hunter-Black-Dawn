using UnityEngine;

public class UILocalization : MonoBehaviour
{
	private string mLanguage;

	public string key;

	private void OnEnable()
	{
		if (mLanguage != LocalizationManager.GetInstance().currentLanguage)
		{
			UIWidget component = GetComponent<UIWidget>();
			UILabel uILabel = component as UILabel;
			if (string.IsNullOrEmpty(key))
			{
				key = uILabel.text;
			}
			string @string = LocalizationManager.GetInstance().GetString(key);
			if (uILabel != null)
			{
				uILabel.text = @string;
			}
			mLanguage = LocalizationManager.GetInstance().currentLanguage;
		}
	}
}

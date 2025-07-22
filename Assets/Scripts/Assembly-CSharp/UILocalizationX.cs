using UnityEngine;

public class UILocalizationX : MonoBehaviour
{
	public int numberIfExist;

	private string mLanguage;

	private void OnEnable()
	{
		if (mLanguage != LocalizationManager.GetInstance().currentLanguage)
		{
			UILabel component = GetComponent<UILabel>();
			if (component != null)
			{
				string text = LocalizationManager.GetInstance().GetString(component.text).Replace("%d", string.Empty + numberIfExist);
				component.text = text;
				mLanguage = LocalizationManager.GetInstance().currentLanguage;
			}
		}
	}
}
